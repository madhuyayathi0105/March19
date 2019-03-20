using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Net;
using System.IO;
using System.Configuration;
public partial class DueFineAmountAllotForStudent : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string usercode = string.Empty;
    int i = 0;
    Boolean Cellclick;
    static byte roll = 0;
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
                bindsem();
                bindHeader();
                bindLedger();
                bindFeesHeader();
                bindFeesLedger();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //  txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                //  txt_todate.Attributes.Add("readonly", "readonly");
                Dropdownload();
                //  ddlpurpose_SelectedIndexChanged(sender, e);
            }
        }
        catch (Exception ex) { }
    }

    #region College
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
        { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindHeader();
            bindLedger();
        }
        catch
        {
        }
    }

    #endregion
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);

    }
    #region stream

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode1 + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }
            binddeg();
        }
        catch
        { }
    }
    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            string stream = ddlstream.SelectedItem.Text.ToString();
            string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + clgvalue + "'";
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
                    for (i = 0; i < cbl_batch.Items.Count; i++)
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

            cbl_degree.Items.Clear();
            string clgvalue = ddl_collegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
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
                    for (i = 0; i < cbl_degree.Items.Count; i++)
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
            string batch2 = "";
            for (i = 0; i < cbl_batch.Items.Count; i++)
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

            string collegecode = ddl_collegename.SelectedItem.Value.ToString();
            if (batch2 != "" && degree != "")
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
                        for (i = 0; i < cbl_dept.Items.Count; i++)
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

            bindsem();
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cb_dept, cbl_dept, txt_dept, lbldept.Text, "--Select--");

            bindsem();
        }
        catch { }
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

    //protected void bindsem()
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
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + clgvalue + "' ";
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

    #region headerandledger
    public void bindHeader()
    {
        try
        {

            cblheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + ddl_collegename.SelectedItem.Value + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblheader.DataSource = ds;
                cblheader.DataTextField = "HeaderName";
                cblheader.DataValueField = "HeaderPK";
                cblheader.DataBind();
                for (int i = 0; i < cblheader.Items.Count; i++)
                {
                    cblheader.Items[i].Selected = true;
                }
                txtheader.Text = "Header(" + cblheader.Items.Count + ")";
                cbheader.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void bindLedger()
    {
        try
        {
            cblledger.Items.Clear();
            string hed = "";
            for (int i = 0; i < cblheader.Items.Count; i++)
            {
                if (cblheader.Items[i].Selected == true)
                {
                    if (hed == "")
                    {
                        hed = cblheader.Items[i].Value.ToString();
                    }
                    else
                    {
                        hed = hed + "','" + "" + cblheader.Items[i].Value.ToString() + "";
                    }
                }
            }


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + ddl_collegename.SelectedItem.Value + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblledger.DataSource = ds;
                cblledger.DataTextField = "LedgerName";
                cblledger.DataValueField = "LedgerPK";
                cblledger.DataBind();
                for (int i = 0; i < cblledger.Items.Count; i++)
                {
                    cblledger.Items[i].Selected = true;
                }
                txtledger.Text = "Ledger(" + cblledger.Items.Count + ")";
                cbledger.Checked = true; ;

            }
            else
            {
                for (int i = 0; i < cblledger.Items.Count; i++)
                {
                    cblledger.Items[i].Selected = false;
                }
                txtledger.Text = "--Select--";
                cbledger.Checked = false; ;
            }

        }
        catch
        {
        }
    }
    public void cbheader_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbheader, cblheader, txtheader, "Header", "--Select--");
            bindLedger();
        }
        catch (Exception ex)
        { }
    }

    public void cblheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbheader, cblheader, txtheader, "Header", "--Select--");
            bindLedger();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbledger_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");

        }
        catch (Exception ex)
        { }
    }
    public void cblledger_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbledger, cblledger, txtledger, "Ledger", "--Select--");
        }
        catch (Exception ex)
        { }
    }
    #endregion

    #region Button go

    protected DataSet loadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {

            #region getvalue

            if (ddl_collegename.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_collegename.SelectedItem.Value);
            }
            string batch1 = Convert.ToString(getCblSelectedValue(cbl_batch));
            string degcourseid = Convert.ToString(getCblSelectedValue(cbl_degree));
            string deptdegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            string sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            string headerid = Convert.ToString(getCblSelectedValue(cblheader));
            string ledgerid = Convert.ToString(getCblSelectedValue(cblledger));
            string duration = Convert.ToString(txtduration.Text);
            // string sem = Convert.ToString(ddlsemester.SelectedItem.Value);
            string fromdate = Convert.ToString(txt_fromdate.Text);
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
            {
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            }
            DateTime dt = Convert.ToDateTime(fromdate);

            if (duration != "")
            {
                int dur = Convert.ToInt32(duration);
                for (int i = 0; i < dur; i++)
                {
                    dt = dt.AddDays(1);
                }
                string fnldt = dt.ToString("dd/MM/yyyy");
                string[] date = fnldt.Split('/');
                if (date.Length == 3)
                {
                    fromdate = date[1].ToString() + "/" + date[0].ToString() + "/" + date[2].ToString();
                }
            }
            // string todate = Convert.ToString(txt_todate.Text);
            //string[] frdate = fromdate.Split('/');
            //if (frdate.Length == 3)
            //{
            //    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            //}
            //string[] tdate = todate.Split('/');
            //if (tdate.Length == 3)
            //{
            //    todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            //}

            #endregion

            #region query

            //feeallot
            string SelQ = " select f.app_no,r.stud_name,r.Roll_No,r.Reg_no,r.roll_admit,r.Batch_Year,r.degree_code,SUM(isnull(TotalAmount,'0')-isnull(PaidAmount,'0')) as balamt,f.FeeCategory  from FT_FeeAllot f,applyn a,Registration r where f.app_no=a.App_No and a.app_no=r.App_No and isnull(f.BalAmount,0)>0  AND a.IsConfirm = 1 AND Admission_Status = 1 and r.college_code ='" + collegecode1 + "' ";
            if (batch1 != "")
                SelQ = SelQ + " and r.Batch_Year in ('" + batch1 + "')";

            if (deptdegcode != "")
                SelQ = SelQ + "  and  r.Degree_Code in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  f.FeeCategory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  f.HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  f.LedgerFK in ('" + ledgerid + "')";

            SelQ = SelQ + " group by  f.app_no,r.stud_name,r.Roll_No,r.Reg_no,r.roll_admit,r.Batch_Year,r.degree_code,f.FeeCategory  ";
            //finemaster degreecode
            SelQ = SelQ + "  select distinct DegreeCode from FM_FineMaster where CollegeCode='" + collegecode1 + "'";
            if (deptdegcode != "")
                SelQ = SelQ + "  and  DegreeCode in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  FeeCatgory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  LedgerFK in ('" + ledgerid + "')";

            if (fromdate != "")
                SelQ = SelQ + " and  DueDate <='" + fromdate + "'";

            //finemaster HeaderFK
            SelQ = SelQ + "  select distinct HeaderFK from FM_FineMaster where CollegeCode='" + collegecode1 + "'";
            if (deptdegcode != "")
                SelQ = SelQ + "  and  DegreeCode in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  FeeCatgory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  LedgerFK in ('" + ledgerid + "')";

            if (fromdate != "")
                SelQ = SelQ + " and  DueDate <='" + fromdate + "'";

            //finemaster LedgerFK
            SelQ = SelQ + "  select distinct LedgerFK from FM_FineMaster where CollegeCode='" + collegecode1 + "'";
            if (deptdegcode != "")
                SelQ = SelQ + "  and  DegreeCode in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  FeeCatgory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  LedgerFK in ('" + ledgerid + "')";

            if (fromdate != "")
                SelQ = SelQ + " and  DueDate <='" + fromdate + "'";

            //finemaster feecatagory
            SelQ = SelQ + "  select distinct FeeCatgory from FM_FineMaster where CollegeCode='" + collegecode1 + "'";
            if (deptdegcode != "")
                SelQ = SelQ + "  and  DegreeCode in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  FeeCatgory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  LedgerFK in ('" + ledgerid + "')";

            if (fromdate != "")
                SelQ = SelQ + " and  DueDate <='" + fromdate + "'";

            // SelQ = SelQ + " group by DueDate";

            SelQ = SelQ + " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode1 + "'";

            SelQ = SelQ + " select TextCode,TextVal  from TextValTable where TextCriteria ='FEECA' and college_code ='" + collegecode1 + "'";
            SelQ = SelQ + "   select distinct CONVERT(varchar(10),DueDate,103) as DueDate from FM_FineMaster where CollegeCode='" + collegecode1 + "'";
            if (deptdegcode != "")
                SelQ = SelQ + "  and  DegreeCode in ('" + deptdegcode + "')";

            if (sem != "")
                SelQ = SelQ + " and  FeeCatgory in ('" + sem + "')";

            if (headerid != "")
                SelQ = SelQ + " and  HeaderFK in ('" + headerid + "')";

            if (ledgerid != "")
                SelQ = SelQ + " and  LedgerFK in ('" + ledgerid + "')";

            if (fromdate != "")
            {
                // SelQ = SelQ + " and  DueDate ='" + fromdate + "'";
            }

            #endregion

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            // }
        }
        catch { }

        return dsload;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = loadDataset();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {
            loadStudentDetails();
            ddlpurpose_SelectedIndexChanged(sender, e);
        }
        else
        {
            //if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count != 0)
            //{
            if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count != 0 && ds.Tables[0].Rows.Count == 0)
            {
                print.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                divspread.Visible = false;
                FpSpread1.Visible = false;
                txtduration.Text = "";
                btnsendsms.Visible = false;
                txtmessage.Visible = false;
                divbtn.Visible = false;
                FpSpread2.Visible = false;
                divpur.Visible = false;
                tdfees.Visible = false;

                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
            else if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count != 0 && ds.Tables[0].Rows.Count != 0)
            {
                string date = "";
                string duration = Convert.ToString(txtduration.Text);
                int durcnt = Convert.ToInt32(duration);
                string fromdate = Convert.ToString(txt_fromdate.Text);
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                {
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
                }
                DateTime dt = Convert.ToDateTime(fromdate);
                //for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                //{
                date = Convert.ToString(ds.Tables[7].Rows[0]["DueDate"]);
                //break;
                //}
                print.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                divspread.Visible = false;
                FpSpread1.Visible = false;
                txtduration.Text = "";
                btnsendsms.Visible = false;
                txtmessage.Visible = false;
                divbtn.Visible = false;
                FpSpread2.Visible = false;
                divpur.Visible = false;
                imgdiv2.Visible = true;
                tdfees.Visible = false;
                lbl_alert.Text = "Your Last DueDate is " + date + "! Please Select Correct DueDate";
            }
            else if (ds.Tables.Count > 0 && ds.Tables[1].Rows.Count == 0)
            {
                print.Visible = false;
                lblvalidation1.Text = "";
                txtexcelname.Text = "";
                divspread.Visible = false;
                FpSpread1.Visible = false;
                txtduration.Text = "";
                btnsendsms.Visible = false;
                txtmessage.Visible = false;
                divbtn.Visible = false;
                FpSpread2.Visible = false;
                divpur.Visible = false;
                tdfees.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "DueDate Not Available! Please Set the DueDate";
            }
            //}
        }

    }

    protected void loadStudentDetails()
    {
        try
        {
            #region design
            RollAndRegSettings();
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].ColumnCount = 10;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.DoubleCellType txtamt = new FarPoint.Web.Spread.DoubleCellType();
            cball.AutoPostBack = true;
            cb.AutoPostBack = false;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[0].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            // FpSpread1.Sheets[0].Columns[1].Locked = true;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[2].Locked = true;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[3].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Admission No ";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].Locked = true;


            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Batch Year";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[6].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = lbldept.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[7].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = lblsem.Text;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
            FpSpread1.Sheets[0].Columns[8].Locked = true;

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Balance Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].ForeColor = ColorTranslator.FromHtml("#000000");
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[9].Locked = true;


            FpSpread1.Sheets[0].Columns[0].Width = 35;
            FpSpread1.Sheets[0].Columns[1].Width = 62;
            FpSpread1.Sheets[0].Columns[2].Width = 178;
            FpSpread1.Sheets[0].Columns[3].Width = 133;
            FpSpread1.Sheets[0].Columns[4].Width = 133;
            FpSpread1.Sheets[0].Columns[5].Width = 133;
            FpSpread1.Sheets[0].Columns[6].Width = 85;
            FpSpread1.Sheets[0].Columns[7].Width = 210;
            FpSpread1.Sheets[0].Columns[8].Width = 103;
            FpSpread1.Sheets[0].Columns[9].Width = 137;
            spreadColumnVisible();
            #endregion

            #region values
            //header
            string hedid = "";
            for (int hed = 0; hed < ds.Tables[2].Rows.Count; hed++)
            {
                if (hedid == "")
                    hedid = Convert.ToString(ds.Tables[2].Rows[hed]["HeaderFK"]);
                else
                    hedid = hedid + "'" + "," + "'" + Convert.ToString(ds.Tables[2].Rows[hed]["HeaderFK"]);
            }
            //ledger
            string ledid = "";
            for (int led = 0; led < ds.Tables[3].Rows.Count; led++)
            {
                if (ledid == "")
                    ledid = Convert.ToString(ds.Tables[3].Rows[led]["LedgerFK"]);
                else
                    ledid = ledid + "'" + "," + "'" + Convert.ToString(ds.Tables[3].Rows[led]["LedgerFK"]);
            }
            //feecat
            string feecat = "";
            for (int feet = 0; feet < ds.Tables[4].Rows.Count; feet++)
            {
                if (feecat == "")
                    feecat = Convert.ToString(ds.Tables[4].Rows[feet]["FeeCatgory"]);
                else
                    feecat = feecat + "'" + "," + "'" + Convert.ToString(ds.Tables[4].Rows[feet]["FeeCatgory"]);
            }
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[0, 1].CellType = cball;
            FpSpread1.Sheets[0].Cells[0, 1].Value = 0;
            for (int deg = 0; deg < ds.Tables[1].Rows.Count; deg++)
            {
                string duedate = Convert.ToString(ds.Tables[7].Rows[0]["DueDate"]);
                string degreecode = Convert.ToString(ds.Tables[1].Rows[deg]["Degreecode"]);
                DataView dvallot = new DataView();
                ds.Tables[0].DefaultView.RowFilter = "Degree_code='" + degreecode + "' ";
                //and FeeCategory in('" + feecat + "')  and Headerfk in('" + hedid + "') and LedgerFK in ('" + ledid + "')
                dvallot = ds.Tables[0].DefaultView;
                if (dvallot.Count > 0)
                {
                    for (int cnt = 0; cnt < dvallot.Count; cnt++)
                    {
                        FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(cnt + 1);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dvallot[cnt]["app_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cb;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Value = 0;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dvallot[cnt]["stud_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = duedate;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvallot[cnt]["Roll_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dvallot[cnt]["Reg_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvallot[cnt]["roll_admit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dvallot[cnt]["Batch_Year"]);
                        string Degreename = "";
                        DataView Dview = new DataView();
                        if (ds.Tables[5].Rows.Count > 0)
                        {
                            ds.Tables[5].DefaultView.RowFilter = "Degree_code='" + Convert.ToString(dvallot[cnt]["Degree_code"]) + "'";
                            Dview = ds.Tables[5].DefaultView;
                            if (Dview.Count > 0)
                            {
                                Degreename = Convert.ToString(Dview[0]["degreename"]);
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Degreename;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dvallot[cnt]["Degree_code"]);
                        string TextName = "";
                        DataView dvfee = new DataView();
                        if (ds.Tables[6].Rows.Count > 0)
                        {
                            ds.Tables[6].DefaultView.RowFilter = "TextCode='" + Convert.ToString(dvallot[cnt]["FeeCategory"]) + "'";
                            dvfee = ds.Tables[6].DefaultView;
                            if (dvfee.Count > 0)
                            {
                                TextName = Convert.ToString(dvfee[0]["TextVal"]);
                            }
                        }
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = TextName;
                        //  FpSpread1.Sheets[0].Columns[6].Visible = false;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(dvallot[cnt]["FeeCategory"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvallot[cnt]["balamt"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = txtamt;
                    }
                }

            }
            #endregion

            #region visible
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            FpSpread1.Height = 430;
            divspread.Visible = true;
            FpSpread1.Visible = true;
            print.Visible = true;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            FpSpread1.ShowHeaderSelection = false;
            btnsendsms.Visible = true;
            txtmessage.Visible = true;
            divbtn.Visible = true;
            FpSpread2.Visible = true;
            divpur.Visible = true;
            tdfees.Visible = true;
            #endregion


        }
        catch { }
    }

    protected void FpSpread1_OnButtonCommand(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            for (int sel = 0; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
            {
                string value = Convert.ToString(FpSpread1.Sheets[0].Cells[0, 1].Value);
                if (value == "1")
                {
                    FpSpread1.Sheets[0].Cells[sel, 1].Value = 1;
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[sel, 1].Value = 0;
                }

            }
        }
        catch { }
    }
    #endregion

    #region Button sms Send
    protected void btnsendsms_Click(object sender, EventArgs e)
    {
        try
        {
            bool check = false;
            string appno = "";
            string feecat = "";
            string degcode = "";
            string messag = txtmessage.Text.ToString();
            FpSpread1.SaveChanges();
            if (messag != "")
            {
                if (FpSpread1.Rows.Count > 1)
                {
                    for (int sel = 0; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
                    {
                        if (sel == 0)
                            continue;
                        string value = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 1].Value);
                        if (value == "1")
                        {
                            appno = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 0].Tag);
                            // feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 6].Tag);
                            degcode = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 5].Tag);
                            string duedate = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 2].Tag);
                            if (appno != "" && appno != "0" && duedate != "" & duedate != "0" && degcode != "" && degcode != "0" && messag != "")
                            {
                                SmsRights(appno, duedate, degcode, messag);
                                check = true;
                            }
                        }
                    }
                    if (check == false)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please Select Any One Student";
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any One Template";
            }
        }
        catch { }
    }

    protected void SmsRights(string appno, string duedate, string Degcode, string MSGs)
    {
        try
        {
            bool check = false;
            string selMblno = "";
            string msg = "";
            string mblno = "";
            string rights = d2.GetFunction("select value from master_settings where settings='Send Sms Right' and usercode='" + usercode + "'");
            string membrs = d2.GetFunction("select value from master_settings where settings='Send Sms Rights' and usercode='" + usercode + "'");

            // string dueDate = d2.GetFunction("select CONVERT(varchar(10),duedate,103) as duedate from FM_FineMaster where FeeCatgory='" + feecat + "' and DegreeCode='" + Degcode + "' and CollegeCode='" + ddl_collegename.SelectedItem.Value + "'");
            string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            DataSet dsmt = d2.select_method_wo_parameter(strquery, "Text");
            string collname = "";
            if (dsmt.Tables[0].Rows.Count > 0)
            {
                collname = dsmt.Tables[0].Rows[0]["collname"].ToString();
            }
            //if (duedate.Trim() != "0" || duedate.Trim() != "")
            //{
            //    msg = "Dear Parent, Fee payment due date is  " + duedate + "  Please Pay by due date to avoid fine and all inconveniences. " + collname + " Reminder: Dear Parent, Fee pending  is found against your wards  account. Please pay immediately  to avoid progressive fine.";
            //}
            msg = MSGs;
            if (rights == "1")
            {
                if (membrs != "0")
                {
                    string[] splival = membrs.Split(',');
                    for (int sel = 0; sel < splival.Length; sel++)
                    {
                        if (splival[sel] == "1")
                        {
                            //mblno = d2.GetFunction("select Student_Mobile  from applyn where app_no='" + appno + "'");
                            // selMblno = "8608759542";
                            selMblno = d2.GetFunction("select Student_Mobile  from applyn where app_no='" + appno + "'");
                            if (selMblno != "0")
                            {
                                sendsms(appno, selMblno, msg);
                                check = true;
                            }
                        }
                        if (splival[sel] == "2")
                        {
                            // mblno = d2.GetFunction("select parentF_Mobile  from applyn where app_no='" + appno + "'");
                            selMblno = d2.GetFunction("select parentF_Mobile  from applyn where app_no='" + appno + "'");
                            // selMblno = "8608759542";
                            if (selMblno != "0")
                            {
                                sendsms(appno, selMblno, msg);
                                check = true;
                            }
                        }
                        if (splival[sel] == "3")
                        {
                            // mblno = d2.GetFunction("select parentM_Mobile  from applyn where app_no='" + appno + "'");
                            selMblno = d2.GetFunction("select parentM_Mobile  from applyn where app_no='" + appno + "'");
                            //  selMblno = "8608759542";
                            if (selMblno != "0")
                            {
                                sendsms(appno, selMblno, msg);
                                check = true;
                            }
                        }
                    }

                }
                if (check == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "SMS Send Successfully";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Set the Rights To Send Sms";
                return;
            }
        }
        catch { }
    }

    public void sendsms(string app, string mblno, string Msg)
    {
        try
        {
            // string Mobile_no = d2.GetFunction("select parentF_Mobile from applyn where app_no='" + app + "'");
            string user_id = "";
            string SenderID = "";
            string Password = "";
            string todaydate = System.DateTime.Now.ToString("dd/MM/yyyy");
            string[] splitdate = todaydate.Split('/');
            DateTime dt1 = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            string ssr = "select * from Track_Value where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(ssr, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                user_id = Convert.ToString(ds.Tables[0].Rows[0]["SMS_User_ID"]);
            }

            if (user_id.Trim() != "")
            {
                string getval = d2.GetUserapi(user_id);
                string[] spret = getval.Split('-');
                if (spret.GetUpperBound(0) == 1)
                {

                    SenderID = spret[0].ToString();
                    Password = spret[0].ToString();

                }
                string strpath = "http://dnd.airsmsmarketing.info/api/sendmsg.php?user=" + SenderID + "&pass=" + Password + "&sender=" + SenderID + "&phone=" + mblno + "&text=" + Msg + "&priority=ndnd&stype=normal";
                string isst = "0";

                smsreport(strpath, isst, dt1, mblno, Msg);
            }

        }
        catch
        {

        }
    }

    public void smsreport(string uril, string isstaff, DateTime dt1, string phone, string msg)
    {
        try
        {
            string phoneno = phone;
            string message = msg;
            string date = dt1.ToString("MM/dd/yyyy") + ' ' + DateTime.Now.ToString("hh:mm:ss");
            WebRequest request = WebRequest.Create(uril);
            WebResponse response = request.GetResponse();
            Stream data = response.GetResponseStream();
            StreamReader sr = new StreamReader(data);
            string strvel = sr.ReadToEnd();
            string groupmsgid = "";
            groupmsgid = strvel;
            int sms = 0;
            string smsreportinsert = "";

            smsreportinsert = "insert into smsdeliverytrackmaster (mobilenos,groupmessageid,message,college_code,isstaff,date)values( '" + phoneno + "','" + groupmsgid + "','" + message + "','" + ddl_collegename.SelectedItem.Value + "','" + isstaff + "','" + date + "')";
            sms = d2.update_method_wo_parameter(smsreportinsert, "Text");

        }
        catch (Exception ex)
        {

        }

    }

    #endregion

    #region print

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            string degreedetails = "";
            string pagename = "";
            degreedetails = "Student Sms Report";
            pagename = "StudentfeeAllotReport.aspx";
            Printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }


    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Student Sms Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }

    }
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        txtamount.Text = "";
        imgdiv2.Visible = false;
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


    //sms template adding and deleting
    #region Sms template add and delete
    //purpose dropdown load
    public void Dropdownload()
    {
        try
        {
            DataSet ds1 = new DataSet();
            ds1.Dispose();
            ds1.Reset();
            string strpurposename = "select purpose,temp_code from FT_sms_purpose where college_code = '" + ddl_collegename.SelectedValue.ToString() + "'";
            ds1 = d2.select_method_wo_parameter(strpurposename, "Text");
            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddlpurpose.DataSource = ds1;
                ddlpurpose.DataTextField = "Purpose";
                ddlpurpose.DataValueField = "temp_code";
                ddlpurpose.DataBind();
                ddlpurpose.Items.Insert(0, "Select");
                //ddlpurpose.Items.Add(" ");
                //ddlpurpose.Text = " ";

                ddlpurposemsg.DataSource = ds1;
                ddlpurposemsg.DataTextField = "Purpose";
                ddlpurposemsg.DataValueField = "temp_code";
                ddlpurposemsg.DataBind();
                //ddlpurposemsg.Items.Add(" ");
                //ddlpurposemsg.Text = " ";
                ddlpurposemsg.Items.Insert(0, "Select");
            }
        }
        catch
        { }
    }

    //selected index changed
    protected void ddlpurpose_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Spread2Go();

        FpSpread2.Visible = true;

        try
        {

            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpread2.Sheets[0].ColumnHeaderVisible = false;
            FpSpread2.Sheets[0].SheetCorner.Columns[0].Visible = false;
            FpSpread2.Visible = true;
            FpSpread2.Sheets[0].AutoPostBack = true;

            //lblpurpose1.Visible = true;
            ddlpurpose.Visible = true;
            FpSpread2.Sheets[0].RowCount = 1;
            FpSpread2.Sheets[0].ColumnCount = 2;
            FpSpread2.Columns[1].Width = 900;

            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "S.No";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Locked = true;
            FpSpread1.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = ColorTranslator.FromHtml("#000000");

            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Bold = true;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = "Template";
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Locked = true;
            FpSpread1.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            string gfg = ddlpurpose.SelectedValue.ToString();
            string gfvgj = ddlpurposemsg.Text;


            if (gfg == " ")
            {
                ds.Dispose();
                ds.Reset();

                string spread2query = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from FT_sms_template";
                ds = d2.select_method_wo_parameter(spread2query, "Text");
            }
            else
            {
                string spread2query1 = "select ROW_NUMBER() OVER (ORDER BY  Template) as SrNo,Template from FT_sms_template where temp_code = " + ddlpurpose.SelectedValue + "";
                ds = d2.select_method_wo_parameter(spread2query1, "Text");
            }


            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int dscnt = 0; dscnt < ds.Tables[0].Rows.Count; dscnt++)
                {
                    FpSpread2.Sheets[0].RowCount++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["SrNo"]);

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[dscnt]["Template"]);
                }
            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
            FpSpread2.SaveChanges();
        }
        catch
        {

        }
    }

    //add template 
    protected void btnaddtemplate_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread2.Visible = true;
            // UpdatePanel1.Visible = true;
            // UpdatePanel2.Visible = true;
            divempedit.Visible = true;
            templatepanel.Visible = true;
            lblpurpose.Visible = true;
            btnplus.Visible = true;
            btnminus.Visible = true;
            ddlpurpose.Visible = true;
            txtpurposemsg.Visible = true;
            btnsave.Visible = true;
            btnexit.Visible = true;
            lblerror.Visible = false;
            Dropdownload();
        }
        catch
        {

        }
    }

    //button plus detail add
    protected void btnplus_Click(object sender, EventArgs e)
    {
        try
        {
            divempedit.Visible = true;
            templatepanel.Visible = true;
            purposepanel.Visible = true;
            divtempsecond.Visible = true;
            lblpurposecaption.Visible = true;
            txtpurposecaption.Visible = true;
            txtpurposecaption.Text = "";
            btnpurposeadd.Visible = true;
            btnpurposeexit.Visible = true;
        }
        catch
        {
        }
    }

    //button minus delete the details
    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strdelpurpose = "Delete from FT_sms_purpose where temp_code = '" + ddlpurposemsg.SelectedValue + "'";
            i = d2.update_method_wo_parameter(strdelpurpose, "Text");
            if (i == 1)
            {
                Dropdownload();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Purpose deleted Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Purpose deleted Failed";
            }
        }
        catch
        {

        }
    }

    //delete the template 
    protected void btndeletetemplate_Click(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            string txtmsg = txtmessage.Text.ToString();
            if (Cellclick == true)
            {
                if (txtmsg.Trim() != "")
                {

                    string activerow = "";
                    string activecol = "";
                    activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                    activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                    int ar;
                    int ac;
                    ar = Convert.ToInt32(activerow.ToString());
                    ac = Convert.ToInt32(activecol.ToString());
                    if (ar != -1)
                    {
                        string msg = FpSpread2.Sheets[0].GetText(ar, 1);
                        string strdeletequery = "delete   FT_sms_template where Template='" + msg + "'";
                        int vvv = d2.update_method_wo_parameter(strdeletequery, "Text");

                        if (vvv == 1)
                        {
                            txtmessage.Text = "";
                            ddlpurpose_SelectedIndexChanged(sender, e);
                            Dropdownload();
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Delete Template Succefully";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "Delete Template  failed";
                        }
                    }

                    //Spread2Go();
                    Cellclick = false;
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Any One Template";
                }
            }
        }
        catch
        {

        }
    }

    //button save
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {

            string txtmsg = txtpurposemsg.Text.ToString();
            if (txtmsg != "")
            {
                int i = 0;
                string strsavequery = "insert into FT_sms_template (temp_code,Template,college_code)values( '" + ddlpurposemsg.SelectedValue.ToString() + "','" + txtpurposemsg.Text.ToString() + "','" + ddl_collegename.SelectedValue.ToString() + "')";
                i = d2.update_method_wo_parameter(strsavequery, "Text");
                if (i == 1)
                {
                    divempedit.Visible = false;
                    Dropdownload();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Template added Succefully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Template added failed";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Reason";
            }
        }

        //Spread2Go();
        catch
        {
        }

    }

    //button exit
    protected void btnexit_Click(object sender, EventArgs e)
    {
        try
        {
            divempedit.Visible = false;
            templatepanel.Visible = false;
            purposepanel.Visible = false;
            divtempsecond.Visible = false;
            Dropdownload();
        }
        catch
        {
        }
    }
    //spread function

    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }
    protected void FpSpread2_SelectedIndexChanged(Object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                string activerow = "";
                string activecol = "";
                activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                int ar;
                int ac;
                ar = Convert.ToInt32(activerow.ToString());
                ac = Convert.ToInt32(activecol.ToString());
                if (ar != -1)
                {
                    txtmessage.Text = FpSpread2.Sheets[0].GetText(ar, 1);
                }
                Cellclick = false;
            }
        }
        catch
        {
        }
    }

    //purpose adding the details
    protected void btnpurposeadd_Click(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            string strtxtpurpose = string.Empty;
            strtxtpurpose = txtpurposecaption.Text;
            if (strtxtpurpose != "")
            {
                string strinsertpurpose = "insert into FT_sms_purpose (Purpose,college_code) values ( '" + strtxtpurpose + "','" + ddl_collegename.SelectedValue.ToString() + "')";
                i = d2.update_method_wo_parameter(strinsertpurpose, "Text");
                //  txtpurposecaption.Text = "";
                if (i == 1)
                {
                    //lblerror.Text = "Purpose added Successfully";
                    //lblerror.Visible = true;
                    Dropdownload();
                    ddlpurposemsg.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    ddlpurpose.SelectedIndex = ddlpurposemsg.Items.IndexOf(ddlpurposemsg.Items.FindByText(txtpurposecaption.Text.Trim()));
                    divtempsecond.Visible = false;
                    purposepanel.Visible = false;
                    divempedit.Visible = true;
                    templatepanel.Visible = true;
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Purpose added Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Purpose added failed";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter the Purpose";
            }
            txtpurposecaption.Text = "";
            //Spread2Go();
        }
        catch
        {
        }

    }
    protected void btnpurposeexit_Click(object sender, EventArgs e)
    {
        try
        {
            divempedit.Visible = true;
            templatepanel.Enabled = true;
            purposepanel.Visible = false;
            divtempsecond.Visible = false;
        }
        catch
        {
        }
    }

    protected void txtpurposemsg_TextChanged(object sender, EventArgs e)
    {

    }

    #endregion

    //fees save to student
    #region fees allot for student

    protected void btnfeesave_Click(object sender, EventArgs e)
    {
        try
        {
            int save = 0;
            bool saveval = false;
            string header = Convert.ToString(ddl_trheader.SelectedItem.Value);
            string ledger = Convert.ToString(ddl_trledger.SelectedItem.Value);
            string amount = Convert.ToString(txtamount.Text);
            string finYearid = d2.getCurrentFinanceYear(usercode, ddl_collegename.SelectedItem.Value );

            if (header != "" && ledger != "" && amount != "0" && amount != "")
            {
                FpSpread1.SaveChanges();
                for (int sel = 0; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
                {
                    string value = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 1].Value);
                    if (value == "1")
                    {
                        string appno = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 0].Tag);
                        string feecat = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 6].Tag);
                        if (appno != "")
                        {
                            string updateFeeallot = "if exists (select * from FT_FeeAllot where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + feecat + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',FeeAmount='" + amount + "',DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + amount + "',RefundAmount='0',IsFeeDeposit='1',PayMode='1',FeeCategory='" + feecat + "',PaidStatus='0',DueAmount='0',FineAmount='0',BalAmount='" + amount + "',paidamount='" + amount + "' where LedgerFK in('" + ledger + "') and HeaderFK in('" + header + "') and FeeCategory in('" + feecat + "') and  FinYearFK='" + finYearid + "' and App_No in('" + appno + "') else   INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount,IsFeeDeposit,FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + appno + ", " + ledger + "," + header + ",'" + amount + "','0','0','0','" + amount + "','0','1','','1','" + feecat + "','','0','','0','0','" + amount + "'," + finYearid + ")";
                            d2.update_method_wo_parameter(updateFeeallot, "Text");
                            saveval = true;
                        }
                    }
                    else
                        save++;
                }
                if (FpSpread1.Sheets[0].RowCount == save)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Please Select Any One Student";
                }
                else
                {
                    if (saveval == true)
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Not Saved";
                    }
                }
            }
        }
        catch { }

    }
    protected void ddl_trheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        bindFeesLedger();
    }


    public void bindFeesHeader()
    {
        try
        {

            ddl_trheader.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + ddl_collegename.SelectedItem.Value + "  ";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_trheader.DataSource = ds;
                ddl_trheader.DataTextField = "HeaderName";
                ddl_trheader.DataValueField = "HeaderPK";
                ddl_trheader.DataBind();
                //  ddl_trheader.Items.Insert(0, "--Select--");
            }
        }
        catch
        {
        }
    }
    public void bindFeesLedger()
    {
        try
        {
            ddl_trledger.Items.Clear();
            string hed = "";
            if (ddl_trheader.Items.Count > 0)
            {
                hed = Convert.ToString(ddl_trheader.SelectedItem.Value);
            }


            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + ddl_collegename.SelectedItem.Value + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_trledger.DataSource = ds;
                ddl_trledger.DataTextField = "LedgerName";
                ddl_trledger.DataValueField = "LedgerPK";
                ddl_trledger.DataBind();
                // ddl_trledger.Items.Insert(0, "--Select--");
            }
            else
            {
                // ddl_trledger.Items.Insert(0, "--Select--");
            }

        }
        catch
        {
        }
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
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(1);
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
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
            }
            else if (roll == 1)
            {
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
            }
            else if (roll == 2)
            {
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = false;

            }
            else if (roll == 3)
            {
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = false;
            }
            else if (roll == 4)
            {
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = true;
            }
            else if (roll == 5)
            {
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = false;
            }
            else if (roll == 6)
            {
                FpSpread1.Columns[3].Visible = false;
                FpSpread1.Columns[4].Visible = true;
                FpSpread1.Columns[5].Visible = true;
            }
            else if (roll == 7)
            {
                FpSpread1.Columns[3].Visible = true;
                FpSpread1.Columns[4].Visible = false;
                FpSpread1.Columns[5].Visible = true;
            }
        }
        catch { }
    }

    #endregion

    // last modified 04-10-2016 sudhagar
}