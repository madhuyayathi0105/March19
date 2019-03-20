using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

public partial class FeesStructureReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
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
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstrm();
                bindBtch();
                binddeg();
                binddept();
                loadheader();
                ledgerload();
                loadfinanceyear();
                rbheader.Checked = true;
                rbledger.Checked = false;
                loadseat();
                loadsem();
            }
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }

    #region Load college
    public void loadcollege()
    {
        ddl_collegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddl_collegename);
    }

    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            loadheader();
            ledgerload();
            loadseat();
            loadsem();
            loadfinanceyear();
            FpSpread1.Visible = false;
            print.Visible = false;
        }
        catch
        {
        }
    }
    #endregion

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode1 + "'  order by FinYearPK desc";
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
        try
        {
            CallCheckboxListChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");

            //loadheader();
        }
        catch (Exception ex)
        { }
    }
    protected void chkfyear_changed(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(chkfyear, chklsfyear, txtfyear, "Finance Year", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region stream

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);

            string selqry = "select distinct type  from Course where college_code ='" + collegecode1 + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();

            }
            // reuse.bindStreamToDropDown(ddlstream, collegecode1);
            if (ddlstream.Items.Count > 0)
            {
                if (streamEnabled() == 1)
                    ddlstream.Enabled = true;
                else
                    ddlstream.Enabled = false;
            }
            else
                ddlstream.Enabled = false;
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            string stream = ddlstream.SelectedItem.Text.ToString();
            string strStream = string.Empty;
            if (!string.IsNullOrEmpty(stream))
                strStream = " and type  in('" + stream + "')";
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  " + strStream + " and d.college_code='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");

            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "Course_Name";
                cbl_degree.DataValueField = "Course_Id";
                cbl_degree.DataBind();
            }
            for (int j = 0; j < cbl_degree.Items.Count; j++)
            {
                cbl_degree.Items[j].Selected = true;
                cb_degree.Checked = true;
            }
            txt_degree.Text = lbldeg.Text + "(" + cbl_degree.Items.Count + ")";
            binddept();
        }
        catch { }
    }
    #endregion

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
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cb_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_batch, cbl_batch, txt_batch, "Batch", "--Select--");
            binddeg();
            binddept();
        }
        catch { }
    }
    #endregion

    #region degree

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            string stream = "";
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode1 + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
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
        try
        {
            CallCheckboxChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_degree, cbl_degree, txt_degree, lbldeg.Text, "--Select--");
            binddept();
        }
        catch { }
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
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
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
            if (batch2 != "" && degree != "")
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
        try
        {
            CallCheckboxChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");
        }
        catch { }
    }
    #endregion

    #region headerandledger

    //public void loadheader()
    //{
    //    try
    //    {
    //        if (ddl_collegename.Items.Count > 0)
    //        {
    //            collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
    //        }
    //        ddlheader.Items.Clear();
    //        string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
    //        ds = d2.select_method_wo_parameter(query, "Text");
    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            ddlheader.DataSource = ds;
    //            ddlheader.DataTextField = "HeaderName";
    //            ddlheader.DataValueField = "HeaderPK";
    //            ddlheader.DataBind();
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    public void loadheader()
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            cblhedg.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName,hd_priority FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + " order by len(isnull(hd_priority,10000)),hd_priority asc";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblhedg.DataSource = ds;
                cblhedg.DataTextField = "HeaderName";
                cblhedg.DataValueField = "HeaderPK";
                cblhedg.DataBind();

                for (int i = 0; i < cblhedg.Items.Count; i++)
                {
                    cblhedg.Items[i].Selected = true;
                }
                txthedg.Text = "Header(" + cblhedg.Items.Count + ")";
                cbhedg.Checked = true;
            }
            else
            {
                for (int i = 0; i < cblhedg.Items.Count; i++)
                {
                    cblhedg.Items[i].Selected = false;
                }
                txthedg.Text = "--Select--";
                cbhedg.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void ledgerload()
    {
        try
        {
            cblledg.Items.Clear();
            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string headeid = "";
            headeid = Convert.ToString(getCblSelectedValue(cblhedg));
            //if (ddlheader.Items.Count > 0)
            //{
            //    headeid = Convert.ToString(ddlheader.SelectedItem.Value);
            //}
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + collegecode1 + " and L.HeaderFK in('" + headeid + "') ";
            //if (headeid != "")
            //    query1 = query1 + " and L.HeaderFK in('" + headeid + "')";

            query1 += " order by len(isnull(l.priority,1000)) , l.priority asc";

            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblledg.DataSource = ds;
                cblledg.DataTextField = "LedgerName";
                cblledg.DataValueField = "LedgerPK";
                cblledg.DataBind();
                for (int i = 0; i < cblledg.Items.Count; i++)
                {
                    cblledg.Items[i].Selected = true;
                }
                txtledg.Text = "Ledger(" + cblledg.Items.Count + ")";
                cbledg.Checked = true;
            }
            else
            {
                for (int i = 0; i < cblledg.Items.Count; i++)
                {
                    cblledg.Items[i].Selected = false;
                }
                txtledg.Text = "--Select--";
                cbledg.Checked = false; ;
            }

        }
        catch
        {
        }
    }

    //protected void ddlheader_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    ledgerload();
    //}

    public void cbhedg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbhedg, cblhedg, txthedg, "Header", "--Select--");
            ledgerload();
        }
        catch (Exception ex)
        { }
    }
    public void cblhedg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbhedg, cblhedg, txthedg, "Header", "--Select--");
            ledgerload();
        }
        catch (Exception ex)
        { }
    }

    public void cbledg_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbledg, cblledg, txtledg, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void cblledg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbledg, cblledg, txtledg, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region Radio button Event

    protected void rbheader_Changed(object sender, EventArgs e)
    {
        //txthedg.Enabled = true;
        loadheader();
        ledgerload();
    }
    protected void rbledger_Changed(object sender, EventArgs e)
    {
        //ddlheader.Enabled = false;
        //ddlheader.Items.Clear();
        ledgerload();
    }

    #endregion

    #region button go

    protected DataSet loadDatset()
    {
        DataSet dsload = new DataSet();
        try
        {
            if (ddl_collegename.Items.Count > 0)
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);

            string headerid = "";
            string Finyearvalue = "";
            string batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            string deptcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            Finyearvalue = Convert.ToString(getCblSelectedValue(chklsfyear));
            headerid = Convert.ToString(getCblSelectedValue(cblhedg));
            string ledgerid = Convert.ToString(getCblSelectedValue(cblledg));
            string seatType = Convert.ToString(getCblSelectedValue(cbl_seat));
            string feeCat = Convert.ToString(getCblSelectedValue(cbl_sem));
            string SelectQ = "";
            string strSel = string.Empty;
            string strGp = string.Empty;
            if (rblMode.SelectedIndex == 0)
            {
                strSel = ",f.headerfk,h.headername";
                strGp = ",f.headerfk,h.headername ";
            }
            else
            {
                strSel = ",LedgerFK,l.ledgername,l.priority";
                strGp = ",ledgerfk,l.ledgername,l.priority  order by len(isnull(l.priority,1000)) , l.priority asc ";
            }

            if (!string.IsNullOrEmpty(collegecode1) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(deptcode) && !string.IsNullOrEmpty(headerid) && !string.IsNullOrEmpty(ledgerid) && !string.IsNullOrEmpty(seatType) && !string.IsNullOrEmpty(feeCat) && !string.IsNullOrEmpty(Finyearvalue))
            {
                SelectQ = "select  distinct batchyear, DegreeCode,len(isnull(dept_priority,1000)),dept_priority from FT_FeeAllotDegree f,FM_HeaderMaster h,FM_LedgerMaster l,degree d,department dt,course c where l.HeaderFK=h.HeaderPK and l.LedgerPK=f.LedgerFK and l.HeaderFK=f.HeaderFK and d.degree_code=f.degreecode and d.course_id=c.course_id and d.dept_code=dt.dept_code";
                if (batch != "")
                    SelectQ = SelectQ + " and BatchYear in('" + batch + "')";
                if (deptcode != "")
                    SelectQ = SelectQ + " and DegreeCode in('" + deptcode + "')";
                if (headerid != "")
                    SelectQ = SelectQ + " and f.HeaderFK in('" + headerid + "') ";
                if (ledgerid != "")
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "') ";
                if (ledgerid != "")
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "') ";
                if (Finyearvalue != "")
                    SelectQ = SelectQ + " and f.FinYearFK in ('" + Finyearvalue + "') ";
                if (seatType != "")
                    SelectQ += " and f.seattype in('" + seatType + "')";
                if (feeCat != "")
                    SelectQ += " and f.feecategory in('" + feeCat + "')";
                SelectQ += " order by len(isnull(dept_priority,1000)),dept_priority asc";
                //  and A.FinYearFK in ('" + Finyearvalue + "') ";
                // SelectQ = SelectQ + "  order by isnull(priority,1000), ledgerName asc,DegreeCode ";
                SelectQ = SelectQ + " select batchyear,DegreeCode,SUM(TotalAmount) as totalamt" + strSel + " from FT_FeeAllotDegree f,FM_HeaderMaster h,FM_LedgerMaster l where l.HeaderFK=h.HeaderPK and l.LedgerPK=f.LedgerFK and l.HeaderFK=f.HeaderFK";
                if (batch != "")
                    SelectQ = SelectQ + " and BatchYear in('" + batch + "')";
                if (deptcode != "")
                    SelectQ = SelectQ + " and DegreeCode in('" + deptcode + "')";
                if (headerid != "")
                    SelectQ = SelectQ + " and f.HeaderFK in('" + headerid + "') ";
                if (ledgerid != "")
                    SelectQ = SelectQ + " and LedgerFK in('" + ledgerid + "') ";
                if (Finyearvalue != "")
                    SelectQ = SelectQ + " and f.FinYearFK in ('" + Finyearvalue + "') ";
                if (seatType != "")
                    SelectQ += " and f.seattype in('" + seatType + "')";
                if (feeCat != "")
                    SelectQ += " and f.feecategory in('" + feeCat + "')";
                SelectQ = SelectQ + " group by batchyear,DegreeCode" + strGp + "";

                SelectQ = SelectQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";
                SelectQ += "   select headername,h.headerpk,ledgerpk,ledgername from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode='" + collegecode1 + "' and l.HeaderFK in('" + headerid + "') and LedgerPK in('" + ledgerid + "') ";
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(SelectQ, "Text");
            }
        }
        catch { dsload = null; }
        return dsload;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDatset();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspreadValues();
            }
            else
            {
                FpSpread1.Visible = false;
                print.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record found";
            }
        }
        catch { }
    }

    protected void loadspreadValues()
    {
        try
        {
            #region design
            int value = 0;
            Hashtable htledg = new Hashtable();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            //  FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].VerticalAlign = VerticalAlign.Middle;
            FpSpread1.Sheets[0].Columns[1].VerticalAlign = VerticalAlign.Middle;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;


            //if (value == 1)
            //{

            Hashtable hthedg = new Hashtable();
            DataView dvhed = new DataView();
            if (cblhedg.Items.Count > 0)
            {
                if (rblMode.SelectedIndex == 0)
                {
                    #region header
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    for (int hed = 0; hed < cblhedg.Items.Count; hed++)
                    {
                        if (!cblhedg.Items[hed].Selected)
                            continue;
                        FpSpread1.Sheets[0].ColumnCount++;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = cblhedg.Items[hed].Text;
                        htledg.Add(Convert.ToString(cblhedg.Items[hed].Value), Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    }
                    #endregion
                }
                else
                {
                    #region Ledger
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
                    int hedcnt = 2;
                    bool plus = true;
                    for (int hed = 0; hed < cblhedg.Items.Count; hed++)
                    {
                        if (cblhedg.Items[hed].Selected == true)
                        {
                            if (plus == true)
                            {
                                hedcnt++;
                                plus = false;
                            }
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
                            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
                            int cnt = 0;
                            ds.Tables[3].DefaultView.RowFilter = "headerpk='" + cblhedg.Items[hed].Value + "'";
                            dvhed = ds.Tables[3].DefaultView;
                            for (int sel = 0; sel < dvhed.Count; sel++)
                            {
                                FpSpread1.Sheets[0].ColumnCount++;
                                cnt++;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dvhed[sel]["ledgername"]);
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(dvhed[sel]["ledgerpk"]);
                                htledg.Add(Convert.ToString(dvhed[sel]["ledgerpk"]), Convert.ToString(FpSpread1.Sheets[0].ColumnCount - 1));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                            }
                            if (cnt != 0)
                            {
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Text = cblhedg.Items[hed].Text;
                                hthedg.Add(Convert.ToString(cblhedg.Items[hed].Value), Convert.ToString(hedcnt));
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].ForeColor = ColorTranslator.FromHtml("#000000");
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Bold = true;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, hedcnt].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, hedcnt, 1, cnt);
                                hedcnt += cnt;
                            }
                        }
                    }
                    #endregion
                }

                FpSpread1.Sheets[0].ColumnCount++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
            }
            #endregion

            #region value
            DataView Dview = new DataView();
            Hashtable htfnltot = new Hashtable();
            Hashtable grandtotal = new Hashtable();
            Hashtable grandhtfnl = new Hashtable();
            int sno = 0;
            Hashtable thfinltot = new Hashtable();
            bool boolcheck = false;
            for (int year = 0; year < cbl_batch.Items.Count; year++)
            {
                if (cbl_batch.Items[year].Selected)
                {
                    ds.Tables[0].DefaultView.RowFilter = "batchyear='" + cbl_batch.Items[year].Value + "'";
                    DataView dvyr = ds.Tables[0].DefaultView;
                    if (dvyr.Count > 0)
                    {
                        for (int sel = 0; sel < dvyr.Count; sel++)
                        {
                            sno++;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dvyr[sel]["batchyear"]);
                            string Degreename = "";
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                ds.Tables[2].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvyr[sel]["DegreeCode"]) + "'";
                                Dview = ds.Tables[2].DefaultView;
                                if (Dview.Count > 0)
                                {
                                    Degreename = Convert.ToString(Dview[0]["degreename"]);
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Degreename;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                }
                            }

                            double ledgamt = 0;
                            string ledgeramt = "";
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                if (rblMode.SelectedIndex == 0)
                                {
                                    for (int ledg = 0; ledg < cblhedg.Items.Count; ledg++)
                                    {
                                        #region Header
                                        if (!cblhedg.Items[ledg].Selected)
                                            continue;
                                        DataView dv = new DataView();
                                        ds.Tables[1].DefaultView.RowFilter = " DegreeCode='" + Convert.ToString(ds.Tables[0].Rows[sel]["DegreeCode"]) + "' and headerfk='" + Convert.ToString(cblhedg.Items[ledg].Value) + "' and batchyear='" + cbl_batch.Items[year].Value + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        int colcount = Convert.ToInt32(htledg[Convert.ToString(cblhedg.Items[ledg].Value)]);
                                        if (dv.Count > 0)
                                        {
                                            boolcheck = true;
                                            double.TryParse(Convert.ToString(dv[0]["totalamt"]), out ledgamt);
                                            ledgeramt = Convert.ToString(dv[0]["totalamt"]);
                                            //final column value
                                            int colval = Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1);
                                            if (!htfnltot.ContainsKey(colval))
                                                htfnltot.Add(colval, Convert.ToString(ledgamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htfnltot[colval]), out amount);
                                                amount += ledgamt;
                                                htfnltot.Remove(colval);
                                                htfnltot.Add(colval, Convert.ToString(amount));
                                            }
                                            //grand total for individual dept
                                            if (!grandtotal.ContainsKey(colcount))
                                                grandtotal.Add(colcount, Convert.ToString(ledgamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[colcount]), out amount);
                                                amount += ledgamt;
                                                grandtotal.Remove(colcount);
                                                grandtotal.Add(colcount, Convert.ToString(amount));
                                            }
                                        }
                                        else
                                        {
                                            ledgeramt = "0";
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = ledgeramt;
                                        #endregion
                                    }
                                }
                                else
                                {
                                    for (int ledg = 0; ledg < cblledg.Items.Count; ledg++)
                                    {
                                        #region ledger
                                        if (!cblledg.Items[ledg].Selected)
                                            continue;
                                        DataView dv = new DataView();
                                        ds.Tables[1].DefaultView.RowFilter = "DegreeCode='" + Convert.ToString(ds.Tables[0].Rows[sel]["DegreeCode"]) + "' and LedgerFK='" + Convert.ToString(cblledg.Items[ledg].Value) + "' and batchyear='" + cbl_batch.Items[year].Value + "'";
                                        dv = ds.Tables[1].DefaultView;
                                        int colcount = Convert.ToInt32(htledg[Convert.ToString(cblledg.Items[ledg].Value)]);
                                        if (dv.Count > 0)
                                        {
                                            boolcheck = true;
                                            double.TryParse(Convert.ToString(dv[0]["totalamt"]), out ledgamt);
                                            ledgeramt = Convert.ToString(dv[0]["totalamt"]);

                                            //final column value
                                            int colval = Convert.ToInt32(FpSpread1.Sheets[0].RowCount - 1);
                                            if (!htfnltot.ContainsKey(colval))
                                                htfnltot.Add(colval, Convert.ToString(ledgamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(htfnltot[colval]), out amount);
                                                amount += ledgamt;
                                                htfnltot.Remove(colval);
                                                htfnltot.Add(colval, Convert.ToString(amount));
                                            }

                                            //grand total for individual dept
                                            if (!grandtotal.ContainsKey(colcount))
                                                grandtotal.Add(colcount, Convert.ToString(ledgamt));
                                            else
                                            {
                                                double amount = 0;
                                                double.TryParse(Convert.ToString(grandtotal[colcount]), out amount);
                                                amount += ledgamt;
                                                grandtotal.Remove(colcount);
                                                grandtotal.Add(colcount, Convert.ToString(amount));
                                            }
                                        }
                                        else
                                        {
                                            ledgeramt = "0";
                                        }
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount].Text = ledgeramt;
                                        #endregion
                                    }
                                }

                                //final column value
                                string finalvalue = Convert.ToString(htfnltot[FpSpread1.Sheets[0].RowCount - 1]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = finalvalue;
                                int grdcolcount = Convert.ToInt32(FpSpread1.Sheets[0].ColumnCount - 1);
                                //grandhtfnl
                                if (!grandhtfnl.ContainsKey(grdcolcount))
                                    grandhtfnl.Add(grdcolcount, Convert.ToString(finalvalue));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(grandhtfnl[grdcolcount]), out amount);
                                    amount += Convert.ToDouble(finalvalue);
                                    grandhtfnl.Remove(grdcolcount);
                                    grandhtfnl.Add(grdcolcount, Convert.ToString(amount));
                                }
                            }
                        }

                        FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = ColorTranslator.FromHtml("#4870BE");
                        double grandvalue = 0;
                        for (int j = 3; j < FpSpread1.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(grandtotal[j]), out grandvalue);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);

                            if (!thfinltot.ContainsKey(j))
                                thfinltot.Add(j, Convert.ToString(grandvalue));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(thfinltot[j]), out amount);
                                amount += Convert.ToDouble(grandvalue);
                                thfinltot.Remove(j);
                                thfinltot.Add(j, Convert.ToString(amount));
                            }
                        }
                        string fnlgrd = Convert.ToString(grandhtfnl[FpSpread1.Sheets[0].ColumnCount - 1]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlgrd);
                        if (!thfinltot.ContainsKey(FpSpread1.Sheets[0].ColumnCount - 1))
                            thfinltot.Add(FpSpread1.Sheets[0].ColumnCount - 1, Convert.ToString(fnlgrd));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(thfinltot[FpSpread1.Sheets[0].ColumnCount - 1]), out amount);
                            amount += Convert.ToDouble(fnlgrd);
                            thfinltot.Remove(FpSpread1.Sheets[0].ColumnCount - 1);
                            thfinltot.Add(FpSpread1.Sheets[0].ColumnCount - 1, Convert.ToString(amount));
                        }
                        grandtotal.Clear();
                        htfnltot.Clear();
                        grandhtfnl.Clear();
                    }
                }
            }

            if (boolcheck)
            {
                FpSpread1.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                double grandvalues = 0;
                for (int j = 3; j < thfinltot.Count; j++)
                {
                    double.TryParse(Convert.ToString(thfinltot[j]), out grandvalues);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
                string fnlgrdw = Convert.ToString(thfinltot[FpSpread1.Sheets[0].ColumnCount - 1]);
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(fnlgrdw);
                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #endregion

            #region visible

            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Visible = true;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            #endregion
        }
        catch { }
    }

    #endregion

    protected void btn_errorclose_Click(object seneder, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    #region Print
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
                if (rbheader.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your HeaderWise Report Name";
                }
                else if (rbledger.Checked == true)
                {
                    lblvalidation1.Text = "Please Enter Your LedgerWise Report Name";
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
            degreedetails = "Fees Structure Report";
            pagename = "FeesStructureReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
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
        lbl.Add(lbl_str1);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lbl_sem);
        fields.Add(0);
        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region Seat type and feecategory added by sudhagar

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
            cbl_sem.Items.Clear();
            txt_sem.Text = "--Select--";
            cb_sem.Checked = false;
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
    #endregion

    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'")), out strValue);
        return strValue;
    }

    // last modified 09.09.2017 sudhagar
    protected void rblMode_Selected(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        print.Visible = false;
        lblvalidation1.Text = "";
        txtexcelname.Text = "";
    }
}