
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class DepatmentwiseCollectionReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
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
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (cblclg.Items.Count > 0)
                    getCollegecode();
                loadcollege();
                bindsem();
                loadpaid();
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_todate.Attributes.Add("readonly", "readonly");

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
            if (cblclg.Items.Count > 0)
                getCollegecode();
        }
        catch (Exception ex) { }
    }

    #region college
    public void loadcollege()
    {
        try
        {
            cblclg.Items.Clear();
            cbclg.Checked = false;
            txtclg.Text = "---Select---";
            ds.Clear();
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
                    for (int i = 0; i < cblclg.Items.Count; i++)
                    {
                        cblclg.Items[i].Selected = true;
                    }
                    txtclg.Text = lblclg.Text + "(" + cblclg.Items.Count + ")";
                    cbclg.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cbclg_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        getCollegecode();
        bindsem();
    }
    protected void cblclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cbclg, cblclg, txtclg, lblclg.Text, "--Select--");
        getCollegecode();
        bindsem();
    }
    #endregion

    private string getCollegecode()
    {
        collegecode = string.Empty;
        if (cblclg.Items.Count > 0)
        {
            for (int i = 0; i < cblclg.Items.Count; i++)
            {
                if (cblclg.Items[i].Selected)
                {
                    if (collegecode == string.Empty)
                        collegecode = Convert.ToString(cblclg.Items[i].Value);
                    else
                        collegecode += "'" + "," + "'" + Convert.ToString(cblclg.Items[i].Value);
                }
            }
        }
        return collegecode;
    }

    #region sem
    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sem, cbl_sem, txt_sem, lblsem.Text, "--Select--");
    }

    protected void bindsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            string linkName = string.Empty;
            string cbltext = string.Empty;

            string SelQ = " select count(textcode),college_code from textvaltable where TextCriteria = 'FEECA'and textval like '%Term%' and textval not like '-1%' and college_code in('" + collegecode + "') group by college_code order by count(textcode) desc";
            DataSet dsval = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                string colgcode = Convert.ToString(dsval.Tables[0].Rows[0]["college_code"]);
                string featDegcode = Convert.ToString(getDegreeCode(colgcode));
                d2.featDegreeCode = featDegcode;
                ds = d2.loadFeecategory(colgcode, usercode, ref linkName);
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
        }
        catch { }
    }

    protected string getDegreeCode(string collegecode)
    {
        string degecode = string.Empty;
        string selqry = " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code ='" + collegecode + "'";
        DataSet dsdeg = d2.select_method_wo_parameter(selqry, "Text");
        if (dsdeg.Tables.Count > 0 && dsdeg.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dsdeg.Tables[0].Rows.Count; i++)
            {
                if (degecode == string.Empty)
                    degecode = Convert.ToString(dsdeg.Tables[0].Rows[i]["Degree_Code"]);
                else
                    degecode += "'" + "," + "'" + Convert.ToString(dsdeg.Tables[0].Rows[i]["Degree_Code"]);
            }
        }
        return degecode;
    }

    public void loadFeecategory(ref  Dictionary<string, string> feecatText, ref Dictionary<string, int> feecatValue)
    {
        DataSet dsset = new DataSet();
        try
        {
            if (cblclg.Items.Count > 0)
                collegecode = getCollegecode();
            // Dictionary<string, string> feecatText = new Dictionary<string, string>();
            string linkValue = string.Empty;
            string linkName = string.Empty;
            string SelectQ = string.Empty;
            string sem = Convert.ToString(getCblSelectedText(cbl_sem));
            string SelQ = " select count(textcode),college_code from textvaltable where TextCriteria = 'FEECA'and textval like '%Term%' and textval not like '-1%' and college_code in('" + collegecode + "') group by college_code order by count(textcode) desc";
            DataSet dsval = d2.select_method_wo_parameter(SelQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                string colgcode = Convert.ToString(dsval.Tables[0].Rows[0]["college_code"]);
                string featDegcode = Convert.ToString(getDegreeCode(colgcode));
                d2.featDegreeCode = featDegcode;
                ds = d2.loadFeecategory(colgcode, usercode, ref linkName);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (linkName == "SemesterandYear")
                    {
                        SelectQ = "  select textcode,textval,college_code from textvaltable where TextCriteria = 'FEECA' and (textval in('" + sem + "')) and textval not like '-1%' and college_code in('" + collegecode + "') order by college_code,len(textval), textval asc";
                        dsset.Clear();
                        dsset = d2.select_method_wo_parameter(SelectQ, "Text");
                        feecat(dsset, ref feecatText, ref feecatValue);
                    }
                    else
                    {
                        if (linkName == "Semester")
                        {
                            SelectQ = "  select textcode,textval,college_code from textvaltable where TextCriteria = 'FEECA'and textval in('" + sem + "') and textval not like '-1%' and college_code in('" + collegecode + "') order by college_code,len(textval), textval asc";
                            dsset.Clear();
                            dsset = d2.select_method_wo_parameter(SelectQ, "Text");
                            feecat(dsset, ref feecatText, ref feecatValue);
                        }
                        else if (linkName == "Year")
                        {
                            SelectQ = "  select textcode,textval,college_code from textvaltable where TextCriteria = 'FEECA'and textval in('" + sem + "') and textval not like '-1%' and college_code in('" + collegecode + "') order by college_code,len(textval), textval asc";
                            dsset.Clear();
                            dsset = d2.select_method_wo_parameter(SelectQ, "Text");
                            feecat(dsset, ref feecatText, ref feecatValue);
                        }
                        else if (linkName == "Term")
                        {
                            // SelectQ = "  select textcode,textval,college_code from textvaltable where TextCriteria = 'FEECA'and textval in('" + sem + "') and textval not like '-1%' and college_code in('" + collegecode + "') order by college_code,len(textval), textval asc";
                            SelectQ = "select distinct textval,TextCode,len(textval),t.college_code from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term%' and textval not like '-1%' and t.college_code ='" + collegecode + "' ";
                            if (!string.IsNullOrEmpty(featDegcode))
                                SelectQ += "  and f.degree_code in('" + featDegcode + "') ";
                            SelectQ += " order by len(textval),textval asc";
                            dsset.Clear();
                            dsset = d2.select_method_wo_parameter(SelectQ, "Text");
                            feecat(dsset, ref feecatText, ref feecatValue);
                        }
                    }
                }
            }
        }
        catch { dsset.Clear(); }

    }

    protected Dictionary<string, string> feecat(DataSet dsset, ref Dictionary<string, string> feecatText, ref  Dictionary<string, int> feecatValue)
    {
        //Dictionary<string, string> feecatValue = new Dictionary<string, string>();
        // Dictionary<string, string> feecatText = new Dictionary<string, string>();
        for (int i = 0; i < dsset.Tables[0].Rows.Count; i++)
        {
            string txt = Convert.ToString(dsset.Tables[0].Rows[i]["textval"]);
            string val = Convert.ToString(dsset.Tables[0].Rows[i]["textcode"]);
            feecatValue.Add(val, Convert.ToInt32(dsset.Tables[0].Rows[i]["college_code"]));
            feecatText.Add(val, txt);
        }
        return feecatText;
    }

    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
            //chkl_paid.Items.Add(new ListItem("Cash", "1"));
            //chkl_paid.Items.Add(new ListItem("Cheque", "2"));
            //chkl_paid.Items.Add(new ListItem("DD", "3"));
            //chkl_paid.Items.Add(new ListItem("Challan", "4"));
            //chkl_paid.Items.Add(new ListItem("Online", "5"));
            //chkl_paid.Items.Add(new ListItem(" Card", "6"));
            PaymodeToCheckboxList(chkl_paid, usercode, collegecode);
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
        try
        {
            CallCheckboxChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(chk_paid, chkl_paid, txt_paid, "Paid", "--Select--");
        }
        catch
        { }
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

    protected void btngo_Click(object sender, EventArgs e)
    {
        Dictionary<string, int> feecatValue = new Dictionary<string, int>();
        Dictionary<string, string> feecatText = new Dictionary<string, string>();
        loadFeecategory(ref  feecatText, ref  feecatValue);
        ds.Clear();
        ds = dsloadDetails(feecatValue);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
        {

            spreadLoadDetailed(ds, feecatValue, feecatText);
        }
        else
        {
            lblvalidation1.Text = string.Empty;
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            tblpaymode.Visible = false;
            print.Visible = false;
            lbl_alert.Text = "No Record Found";
            imgdiv2.Visible = true;

        }
        //  spreadLoadDetailed(ds);
    }

    protected DataSet dsloadDetails(Dictionary<string, int> feecatValue)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string feecatg = string.Empty;
            if (feecatValue.Count > 0)
            {
                foreach (KeyValuePair<string, int> value in feecatValue)
                {
                    if (feecatg == string.Empty)
                        feecatg = value.Key.ToString();
                    else
                        feecatg += "'" + "," + "'" + value.Key.ToString();
                }
            }
            string sem = "";
            string paid = "";
            string SelQ = "";
            string strRecon = string.Empty;
            if (cblclg.Items.Count > 0)
                collegecode = getCollegecode();
            sem = Convert.ToString(getCblSelectedValue(cbl_sem));
            paid = Convert.ToString(getCblSelectedValue(chkl_paid));
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            string strtype = string.Empty;

            string strReg = " and r.cc=0  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            if (cbbfrecon.Checked)
                strRecon = " and ISNULL(IsCanceled,'0')<>'1'";
            else
                strRecon = " and ( f.IsDeposited='1' and ISNULL( f.IsBounced,0)=0 and ISNULL(IsCollected,0)=1)  and ISNULL(IsCanceled,'0')<>'1'";
            string applynStr = " AND r.IsConfirm = 1  and isnull(is_enroll,'0')<>'2'";//AND Admission_Status = 1
            #endregion
            #region Query old
            //SelQ = " select distinct degree_code,college_code from registration r where college_code in('" + collegecode + "')    " + strReg + "";
            //if (cbbeforeadm.Checked)
            //{
            //    SelQ += " union select distinct degree_code,college_code from applyn r where college_code in('" + collegecode + "') " + applynStr + "";
            //}
            //SelQ += " order by college_code,degree_code";

            //SelQ += " select r.degree_code,sum(totalamount) as totalamount,f.feecategory,r.college_code from Registration r,ft_feeallot f where r.app_no=f.app_no  and f.feecategory in('" + sem + "') and r.college_code in('" + collegecode + "') " + strReg + " group by r.degree_code,f.feecategory,r.college_code having sum(totalamount)>0";
            //if (cbbeforeadm.Checked)
            //{
            //    SelQ += " union select r.degree_code,sum(totalamount) as totalamount,f.feecategory,r.college_code from applyn r,ft_feeallot f where r.app_no=f.app_no  and f.feecategory in('" + sem + "') and r.college_code in('" + collegecode + "') " + applynStr + "  group by r.degree_code,f.feecategory,r.college_code having sum(totalamount)>0";
            //}
            //SelQ += " select distinct sum(debit) as debit,f.feecategory,r.degree_code,r.college_code from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + "  and f.feecategory in('" + sem + "')  and f.paymode in('" + paid + "')  and r.college_code in('" + collegecode + "') " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "' group by f.feecategory" + strtype + ",r.degree_code,r.college_code having sum(debit)>0 ";
            //if (cbbeforeadm.Checked)
            //{
            //    SelQ += " union select distinct sum(debit) as debit,f.feecategory,r.degree_code,r.college_code from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + "  and f.feecategory in('" + sem + "')  and f.paymode in('" + paid + "')  and r.college_code in('" + collegecode + "')  and f.Transdate between '" + fromdate + "' and '" + todate + "' " + applynStr + " group by f.feecategory" + strtype + ",r.degree_code,r.college_code having sum(debit)>0 ";
            //}
            //SelQ += " select d.Degree_Code,(dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym,d.college_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";

            //dsload.Clear();
            //dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion

            #region Query
            SelQ = " select distinct degree_code,college_code from registration r where college_code in('" + collegecode + "')    " + strReg + "";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union select distinct degree_code,college_code from applyn r where college_code in('" + collegecode + "') " + applynStr + "";
            }
            SelQ += " order by college_code,degree_code";

            SelQ += " select degree_code,sum(totalamount) as totalamount,feecategory,college_code from(";
            SelQ += " select r.degree_code,totalamount,f.feecategory,r.college_code from Registration r,ft_feeallot f where r.app_no=f.app_no  and f.feecategory in('" + sem + "') and r.college_code in('" + collegecode + "') " + strReg + " and isnull(totalamount,'0')>0";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select r.degree_code,totalamount,f.feecategory,r.college_code from applyn r,ft_feeallot f where r.app_no=f.app_no  and f.feecategory in('" + sem + "') and r.college_code in('" + collegecode + "') " + applynStr + "  ";
            }
            SelQ += ") tbl group by degree_code,feecategory,college_code";

            SelQ += " select distinct sum(debit) as debit,feecategory,degree_code,college_code from(";
            SelQ += " select  debit,f.feecategory,r.degree_code,r.college_code from ft_findailytransaction f,registration r where f.app_no=r.app_no " + strRecon + "  and f.feecategory in('" + sem + "')  and f.paymode in('" + paid + "')  and r.college_code in('" + collegecode + "') " + strReg + " and f.Transdate between '" + fromdate + "' and '" + todate + "' and isnull(debit,'0')>0  and isnull(actualfinyearfk,'0')<>'0'";
            if (cbbeforeadm.Checked)
            {
                SelQ += " union all select debit,f.feecategory,r.degree_code,r.college_code from ft_findailytransaction f,applyn r where f.app_no=r.app_no " + strRecon + "  and f.feecategory in('" + sem + "')  and f.paymode in('" + paid + "')  and r.college_code in('" + collegecode + "')  and f.Transdate between '" + fromdate + "' and '" + todate + "' " + applynStr + " and isnull(debit,'0')>0 and isnull(actualfinyearfk,'0')<>'0'";
            }
            SelQ += ") tbl group by feecategory" + strtype + ",degree_code,college_code ";

            SelQ += " select d.Degree_Code,(dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym,d.college_code from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and d.college_code in('" + collegecode + "')";

            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelQ, "Text");
            #endregion
        }
        catch { dsload.Clear(); }
        return dsload;
    }

    protected void spreadLoadDetailed(DataSet ds, Dictionary<string, int> feecatValue, Dictionary<string, string> feecatText)
    {
        try
        {
            #region design

            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 2;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            spreadDet.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Text = lbldept.Text;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].ForeColor = ColorTranslator.FromHtml("#000000");
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            string hdrTxtValue = "";
            Hashtable htcolindex = new Hashtable();
            Hashtable htPayCol = new Hashtable();
            bool colsem = false;
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                int col = 0;
                if (cbl_sem.Items[i].Selected)
                {
                    colsem = true;
                    col = spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(cbl_sem.Items[i].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(cbl_sem.Items[i].Value);
                    hdrTxtValue = Convert.ToString(cbl_sem.Items[i].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;


                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Allot";
                    htcolindex.Add(hdrTxtValue + "-" + "Allot", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Paid";
                    htcolindex.Add(hdrTxtValue + "-" + "Paid", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;

                    spreadDet.Sheets[0].ColumnCount++;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Balance";
                    htcolindex.Add(hdrTxtValue + "-" + "Balance", spreadDet.Sheets[0].ColumnCount - 1);
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    #region
                    //for (int s = 0; s < chkl_paid.Items.Count; s++)
                    //{
                    //    if (chkl_paid.Items[s].Selected == true)
                    //    {
                    //        checkva++;
                    //        if (checkva > 1)
                    //            spreadDet.Sheets[0].ColumnCount++;

                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    //        htcolindex.Add(Convert.ToString(hdrTxtValue + "-" + chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    //        if (!htPayCol.ContainsKey(chkl_paid.Items[s].Value))
                    //            htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), spreadDet.Sheets[0].ColumnCount - 1);
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //        spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //        colsem = true;
                    //    }
                    //}
                    //if (colsem)
                    //{
                    //    checkva++;
                    //    spreadDet.Sheets[0].ColumnCount++;
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Text = "Total";
                    //    htcolindex.Add(Convert.ToString(hdrTxtValue + "-" + "Total"), spreadDet.Sheets[0].ColumnCount - 1);
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].ForeColor = ColorTranslator.FromHtml("#000000");
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Bold = true;
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                    //    spreadDet.Sheets[0].ColumnHeader.Cells[1, spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Right;
                    //    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, checkva);
                    //}
                    #endregion
                }
                if (colsem)
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 1, 3);
                    colsem = false;
                }
            }
            #endregion
            #region value
            int height = 0;
            Hashtable grandtotal = new Hashtable();
            Hashtable fnlgrandtotal = new Hashtable();
            int rowcnt = 0;
            for (int i = 0; i < cblclg.Items.Count; i++)
            {
                bool clgbool = true;
                bool clgchkbool = false;
                int fstrowCnt = 0;
                if (cblclg.Items[i].Selected)
                {
                    ds.Tables[0].DefaultView.RowFilter = "College_code='" + Convert.ToString(cblclg.Items[i].Value) + "'";
                    DataView dvdeg = ds.Tables[0].DefaultView;
                    if (dvdeg.Count > 0)
                    {
                        for (int row = 0; row < dvdeg.Count; row++)
                        {
                            bool rowbool = false;
                            bool cblbool = true;
                            string degcode = Convert.ToString(dvdeg[row]["degree_code"]);
                            if (feecatValue.ContainsValue(Convert.ToInt32(cblclg.Items[i].Value)))
                            {
                                Dictionary<string, int> tempfeecatValue = feecatValue.Where(p => p.Value == Convert.ToInt32(cblclg.Items[i].Value)).ToDictionary(p => p.Key, p => p.Value);
                                foreach (KeyValuePair<string, int> fee in tempfeecatValue)
                                {
                                    double allotAmount = 0;
                                    double paidAmount = 0;
                                    string feeCode = Convert.ToString(fee.Key);
                                    string str = feecatText.ContainsKey(feeCode) ? str = feecatText[feeCode].ToString() : str = "";
                                    //allot
                                    ds.Tables[1].DefaultView.RowFilter = "degree_code='" + degcode + "' and feecategory='" + feeCode + "' and college_code='" + cblclg.Items[i].Value + "'";
                                    DataView dvallot = ds.Tables[1].DefaultView;
                                    rowbool = true;
                                    clgchkbool = true;
                                    int curColCnt = 0;
                                    int.TryParse(Convert.ToString(htcolindex[str + "-" + "Allot"]), out curColCnt);
                                    if (dvallot.Count > 0)
                                        double.TryParse(Convert.ToString(dvallot[0]["TotalAmount"]), out allotAmount);
                                    if (!grandtotal.ContainsKey(curColCnt))
                                        grandtotal.Add(curColCnt, Convert.ToString(allotAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                        amount += allotAmount;
                                        grandtotal.Remove(curColCnt);
                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                    }

                                    if (clgbool)
                                        fstrowCnt = spreadDet.Sheets[0].RowCount++;
                                    if (cblbool)
                                        spreadDet.Sheets[0].RowCount++;
                                    if (allotAmount != 0)
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(allotAmount);
                                    else
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].ForeColor = Color.Blue;
                                    //paid
                                    ds.Tables[2].DefaultView.RowFilter = "degree_code='" + degcode + "' and feecategory='" + feeCode + "' and college_code='" + cblclg.Items[i].Value + "'";
                                    DataView dvpaid = ds.Tables[2].DefaultView;
                                    int.TryParse(Convert.ToString(htcolindex[str + "-" + "Paid"]), out curColCnt);
                                    if (dvpaid.Count > 0)
                                        double.TryParse(Convert.ToString(dvpaid[0]["debit"]), out paidAmount);
                                    if (!grandtotal.ContainsKey(curColCnt))
                                        grandtotal.Add(curColCnt, Convert.ToString(paidAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                        amount += paidAmount;
                                        grandtotal.Remove(curColCnt);
                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                    if (paidAmount != 0)
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(paidAmount);
                                    else
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";
                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].ForeColor = Color.Green;
                                    //balance
                                    int.TryParse(Convert.ToString(htcolindex[str + "-" + "Balance"]), out curColCnt);
                                    double balamount = allotAmount - paidAmount;
                                    if (balamount != 0)
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = Convert.ToString(balamount);
                                    else
                                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].Text = "-";

                                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, curColCnt].ForeColor = Color.Red;
                                    if (!grandtotal.ContainsKey(curColCnt))
                                        grandtotal.Add(curColCnt, Convert.ToString(balamount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(grandtotal[curColCnt]), out amount);
                                        amount += balamount;
                                        grandtotal.Remove(curColCnt);
                                        grandtotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                    cblbool = false;
                                    clgbool = false;
                                }
                            }
                            if (rowbool)
                            {
                                #region dept name
                                rowcnt++;
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(rowcnt);
                                DataView Dview = new DataView();
                                string Degreename = string.Empty;
                                string Acrname = string.Empty;
                                if (ds.Tables[3].Rows.Count > 0)
                                {
                                    ds.Tables[3].DefaultView.RowFilter = "Degree_code='" + degcode + "' and college_code='" + cblclg.Items[i].Value + "'";
                                    Dview = ds.Tables[3].DefaultView;
                                    if (Dview.Count > 0)
                                    {
                                        Degreename = Convert.ToString(Dview[0]["degreename"]);
                                        Acrname = Convert.ToString(Dview[0]["dept_acronym"]);
                                    }
                                }
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].RowCount - 1, 1].Text = Degreename;
                                height += 15;
                                #endregion
                            }
                        }
                    }
                    if (clgchkbool)
                    {
                        #region colg name

                        spreadDet.Sheets[0].Cells[fstrowCnt, 0].Text = Convert.ToString(cblclg.Items[i].Text);
                        spreadDet.Sheets[0].SpanModel.Add(fstrowCnt, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                        spreadDet.Sheets[0].Rows[fstrowCnt].BackColor = Color.Green;
                        spreadDet.Sheets[0].Rows[fstrowCnt].ForeColor = Color.White;

                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                        double value = 0;
                        for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(grandtotal[j]), out value);
                            if (value != 0)
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(value);
                            else
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = "-";
                            if (!fnlgrandtotal.ContainsKey(j))
                                fnlgrandtotal.Add(j, Convert.ToString(value));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(fnlgrandtotal[j]), out amount);
                                amount += value;
                                fnlgrandtotal.Remove(j);
                                fnlgrandtotal.Add(j, Convert.ToString(amount));
                            }
                        }
                        #endregion
                    }
                }
            }

            #region grandtot
            // spreadDet.Sheets[0].PageSize = ds.Tables[0].Rows.Count + 1;
            spreadDet.Sheets[0].Rows.Count++;
            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].ForeColor = Color.Black;
            double grandvalue = 0;
            for (int j = 2; j < spreadDet.Sheets[0].ColumnCount; j++)
            {
                double.TryParse(Convert.ToString(fnlgrandtotal[j]), out grandvalue);
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalue);
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            spreadDet.Height = 100 + height;
            spreadDet.SaveChanges();
            //  payModeLabels(htPayCol);
            #endregion
        }
        catch { }
    }

    protected void payModeLabels(Hashtable htpay)
    {
        lblc2.Visible = false;
        lblc3.Visible = false;
        lblc5.Visible = false;
        lblc1.Visible = false;
        lblc4.Visible = false;
        lblcard.Visible = false;
        foreach (DictionaryEntry row in htpay)
        {
            if (row.Key.ToString() == "1")
                lblc2.Visible = true;
            if (row.Key.ToString() == "2")
                lblc3.Visible = true;
            if (row.Key.ToString() == "3")
                lblc5.Visible = true;
            if (row.Key.ToString() == "4")
                lblc1.Visible = true;
            if (row.Key.ToString() == "5")
                lblc4.Visible = true;
            if (row.Key.ToString() == "6")
                lblcard.Visible = true;
        }
        tblpaymode.Visible = true;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
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
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Fees Structure Report";
            pagename = "FeesStructureReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails);
            Printcontrolhed.Visible = true;
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

        lbl.Add(lblclg);
        // lbl.Add(lbl_str1);
        //  lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        // fields.Add(1);
        //fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    #region paymode setting
    public void PaymodeToCheckboxList(CheckBoxList cblpaymode, string usercode, string collegecode)
    {
        try
        {
            int inclpayRights = 0;
            string payValue = string.Empty;
            Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
            inclpayRights = paymodeRightsCheck(usercode, collegecode, ref  payValue);
            if (inclpayRights == 1 && payValue != "0")
            {
                string[] splvalue = payValue.Split(',');
                if (splvalue.Length > 0)
                {
                    dtpaymode = dtPaymodeValue();
                    for (int row = 0; row < splvalue.Length; row++)
                    {
                        if (dtpaymode.ContainsKey(Convert.ToInt32(splvalue[row])))
                        {
                            string modestr = dtpaymode[Convert.ToInt32(splvalue[row])];
                            cblpaymode.Items.Add(new System.Web.UI.WebControls.ListItem(modestr, Convert.ToString(splvalue[row])));
                        }
                    }
                }
            }
            else
                cblpaymode.Items.Clear();
        }
        catch { cblpaymode.Items.Clear(); }
    }

    private int paymodeRightsCheck(string usercode, string collegecode, ref string payValue)
    {
        int paymodRghts = 0;
        Int32.TryParse(Convert.ToString(d2.GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettings' and user_code ='" + usercode + "'")), out paymodRghts);
        if (paymodRghts == 1)
        {
            payValue = Convert.ToString(d2.GetFunction("select Linkvalue from New_InsSettings where LinkName='IncludePaymodeSettingsValue' and user_code ='" + usercode + "' "));
        }
        return paymodRghts;
    }

    private Dictionary<int, string> dtPaymodeValue()
    {
        Dictionary<int, string> dtpaymode = new Dictionary<int, string>();
        dtpaymode.Add(1, "Cash");
        dtpaymode.Add(2, "Cheque");
        dtpaymode.Add(3, "DD");
        dtpaymode.Add(4, "Challan");
        dtpaymode.Add(5, "Online");
        dtpaymode.Add(6, "Card");
        return dtpaymode;
    }
    #endregion
}