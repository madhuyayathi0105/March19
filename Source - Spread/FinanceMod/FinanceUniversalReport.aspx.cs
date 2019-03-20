using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Configuration;
public partial class FinanceUniversalReport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reuse = new ReuasableMethods();
    DataSet ds = new DataSet();
    connection cs = new connection();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static ArrayList colord = new ArrayList();
    static byte roll = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Session["collegecode"] == null)
            //    Response.Redirect("~/Default.aspx");
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
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadcollege();
                if (ddlcollegename.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
                bindBtch();
                binddeg();
                binddept();
                // bindsem();
                rblsemType_Selected(sender, e);
                bindsec();
                loadHeader();
                loadfinanceyear();
                LoadIncludeSetting();
                loadStudentMode();
                loadseat();
                loadreligion();
                loadcommunity();
                loadGender();
                loadpaid();
                bindroute();
                bindvechileid();
                loadvechilestage();
                loadHostel();
                loadBuilding();
                loadRoomType();
                loadroom();
                rblrptType_Selected(sender, e);
                getAcademicYear();
                // columnType();
                loadquota();
            }
            divcolorder.Attributes.Add("Style", "display:none;");
            filterVisible();
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        }
        catch
        {
        }

    }

    protected void filterVisible()
    {
        fldstud.Attributes.Add("Style", "display:none;");
        sub_quotafield.Attributes.Add("Style", "display:none;");//abarna
        // cbIncStud.Checked = false;
        if (cbIncStud.Checked)
        {
            fldstud.Attributes.Add("Style", "display:block;");
            sub_quotafield.Attributes.Add("Style", "display:block;");//abarna
            cbIncStud.Checked = true;
        }
        fldTrans.Attributes.Add("Style", "display:none;");
        // cbIncTrans.Checked = false;
        if (cbIncTrans.Checked)
        {
            fldTrans.Attributes.Add("Style", "display:block;");
            cbIncTrans.Checked = true;
        }
        fldHstl.Attributes.Add("Style", "display:none;");
        // cbIncHstl.Checked = false;
        if (cbIncHstl.Checked)
        {
            fldHstl.Attributes.Add("Style", "display:block;");
            cbIncHstl.Checked = true;
        }
        txtFromRange.Enabled = false;
        txtToRange.Enabled = false;
        if (cbRange.Checked)
        {
            txtFromRange.Attributes.Add("Style", "height: 20px;width: 60px;");
            txtToRange.Attributes.Add("Style", "height: 20px;width: 60px;");
            txtFromRange.Enabled = true;
            txtToRange.Enabled = true;
        }
    }

    #region college
    public void loadcollege()
    {
        ddlcollegename.Items.Clear();
        reuse.bindCollegeToDropDown(usercode, ddlcollegename);
    }
    protected void ddlcollegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlcollegename.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            }
            bindBtch();
            binddeg();
            binddept();
            // bindsem();
            rblsemType_Selected(sender, e);
            bindsec();
            loadHeader();
            loadfinanceyear();
            LoadIncludeSetting();
            loadStudentMode();
            loadseat();
            loadreligion();
            loadcommunity();
            loadGender();
            bindroute();
            bindvechileid();
            loadvechilestage();
            loadHostel();
            loadBuilding();
            loadRoomType();
            loadroom();
            columnType();
            getAcademicYear();
            loadquota();
        }
        catch
        {
        }
    }
    #endregion

    #region batch
    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
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
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cbl_batch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_batch, cbl_batch, sampleTxt, lblbatch.Text, "--Select--");
        binddeg();
        binddept();
    }
    #endregion

    #region degree
    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            cbl_degree.Items.Clear();
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + clgvalue + "'";
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
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_degree, cbl_degree, sampleTxt, lbldeg.Text, "--Select--");
        binddept();
    }
    #endregion

    #region dept
    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
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
            string collegecode = ddlcollegename.SelectedItem.Value.ToString();
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
                        cb_dept.Checked = true;
                    }
                }
            }
        }
        catch { }
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_dept, cbl_dept, sampleTxt, "Department", "--Select--");
        bindsec();
        rblsemType_Selected(sender, e);
    }
    #endregion

    #region sem
    protected void rblsemType_Selected(object sender, EventArgs e)
    {
        columnType();
        ddlMainreport_Selected(sender, e);
        txtexcelname.Text = string.Empty;
        spreadDet.Visible = false;
        print.Visible = false;
        if (rblsemType.SelectedIndex == 0)
        {
            Dictionary<string, string> dtSet = getSettingFeeCat();
            if (dtSet.Count == 0)
                loadYear();
            else
                loadSettingYear(dtSet);
        }
        else
        {
            bindsem();
        }
    }
    protected void loadYear()
    {
        cbl_sem.Items.Clear();
        string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
        string linkName = string.Empty;
        string cbltext = string.Empty;
        d2.featDegreeCode = featDegcode;
        ds = d2.loadFeecategory(Convert.ToString(ddlcollegename.SelectedItem.Value), usercode, ref linkName);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (linkName == "Term")
            {
                Dictionary<string, string> htSem = feeCatVal(linkName);
                foreach (KeyValuePair<string, string> feeVal in htSem)
                {
                    cbl_sem.Items.Add(new ListItem(feeVal.Key, feeVal.Value));
                }
            }
            else if (linkName == "Year")
            {
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "TextVal";
                cbl_sem.DataValueField = "TextCode";
                cbl_sem.DataBind();
            }
            else if (linkName == "Semester")
            {
                Dictionary<string, string> htSem = feeCatVal(linkName);
                foreach (KeyValuePair<string, string> feeVal in htSem)
                {
                    cbl_sem.Items.Add(new ListItem(feeVal.Key, feeVal.Value));
                }
            }
        }
        if (cbl_sem.Items.Count > 0)
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = true;
            }
            cb_sem.Checked = true;
        }
    }
    protected void loadSettingYear(Dictionary<string, string> htSem)
    {
        cbl_sem.Items.Clear();
        foreach (KeyValuePair<string, string> feeVal in htSem)
        {
            cbl_sem.Items.Add(new ListItem(feeVal.Key, feeVal.Value));
        }
        if (cbl_sem.Items.Count > 0)
        {
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                cbl_sem.Items[i].Selected = true;
            }
            cb_sem.Checked = true;
        }
    }
    protected Dictionary<string, string> feeCatVal(string type)
    {
        Dictionary<string, string> htsem = new Dictionary<string, string>();
        try
        {
            string feecatg = "";
            string cblvalue = "";
            string cbltext = "";
            string selQ = " select textval,textcode from textvaltable where textcriteria='FEECA' and  college_code='" + collegecode + "'";
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int sem = 0; sem < dsval.Tables[0].Rows.Count; sem++)
                {
                    cblvalue = Convert.ToString(dsval.Tables[0].Rows[sem]["textcode"]);
                    cbltext = Convert.ToString(dsval.Tables[0].Rows[sem]["textval"]);
                    if (type == "Semester")
                    {
                        string[] feesem = cbltext.Split(' ');
                        if (feesem[0] == "1" || feesem[0] == "2")
                        {
                            if (!htsem.ContainsKey("1 Year"))
                                htsem.Add(Convert.ToString("1 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["1 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("1 Year");
                                htsem.Add(Convert.ToString("1 Year"), feecatg);
                            }
                        }
                        else if (feesem[0] == "3" || feesem[0] == "4")
                        {
                            if (!htsem.ContainsKey("2 Year"))
                                htsem.Add(Convert.ToString("2 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["2 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("2 Year");
                                htsem.Add(Convert.ToString("2 Year"), feecatg);
                            }
                        }
                        else if (feesem[0] == "5" || feesem[0] == "6")
                        {
                            if (!htsem.ContainsKey("3 Year"))
                                htsem.Add(Convert.ToString("3 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["3 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("3 Year");
                                htsem.Add(Convert.ToString("3 Year"), feecatg);
                            }
                        }
                        else if (feesem[0] == "7" || feesem[0] == "8")
                        {
                            if (!htsem.ContainsKey("4 Year"))
                                htsem.Add(Convert.ToString("4 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["4 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("4 Year");
                                htsem.Add(Convert.ToString("4 Year"), feecatg);
                            }
                        }
                        else if (feesem[0] == "8" || feesem[0] == "9")
                        {
                            if (!htsem.ContainsKey("5 Year"))
                                htsem.Add(Convert.ToString("5 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["5 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("5 Year");
                                htsem.Add(Convert.ToString("5 Year"), feecatg);
                            }
                        }
                    }
                    else if (type == "Term")
                    {
                        string[] feesem = cbltext.Split(' ');
                        if (feesem[1] == "1" || feesem[1] == "2" || feesem[1] == "3" || feesem[1] == "4" || feesem[1] == "5")
                        {
                            if (!htsem.ContainsKey("1 Year"))
                                htsem.Add(Convert.ToString("1 Year"), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem["1 Year"]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove("1 Year");
                                htsem.Add(Convert.ToString("1 Year"), feecatg);
                            }
                        }
                    }
                }
            }
            ViewState["feecat"] = htsem;
        }
        catch { }
        return htsem;
    }
    protected Dictionary<string, string> getFeecode()
    {
        Dictionary<string, string> htsem = new Dictionary<string, string>();
        try
        {
            string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            string type = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            DataSet dsval = d2.loadFeecategory(Convert.ToString(ddlcollegename.SelectedItem.Value), usercode, ref type);
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                string feecatg = "";
                string cblvalue = "";
                //string selQ = " select textval,textcode from textvaltable where textcriteria='FEECA' and  college_code='" + collegecode + "'";
                //DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    for (int sem = 0; sem < dsval.Tables[0].Rows.Count; sem++)
                    {
                        cblvalue = Convert.ToString(dsval.Tables[0].Rows[sem]["textcode"]);
                        cbltext = Convert.ToString(dsval.Tables[0].Rows[sem]["textval"]);
                        if (rblsemType.SelectedIndex == 0)
                        {
                            #region
                            if (type == "Semester")
                            {
                                #region semester
                                string[] feesem = cbltext.Split(' ');
                                if (feesem[0] == "1" || feesem[0] == "2")
                                {
                                    if (!htsem.ContainsKey("1"))
                                        htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["1"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("1");
                                        htsem.Add(Convert.ToString("1"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "3" || feesem[0] == "4")
                                {
                                    if (!htsem.ContainsKey("2"))
                                        htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["2"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("2");
                                        htsem.Add(Convert.ToString("2"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "5" || feesem[0] == "6")
                                {
                                    if (!htsem.ContainsKey("3"))
                                        htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["3"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("3");
                                        htsem.Add(Convert.ToString("3"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "7" || feesem[0] == "8")
                                {
                                    if (!htsem.ContainsKey("4"))
                                        htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["4"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("4");
                                        htsem.Add(Convert.ToString("4"), feecatg);
                                    }
                                }
                                else if (feesem[0] == "8" || feesem[0] == "9")
                                {
                                    if (!htsem.ContainsKey("5"))
                                        htsem.Add(Convert.ToString("5"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["5"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("5");
                                        htsem.Add(Convert.ToString("5"), feecatg);
                                    }
                                }
                                #endregion
                            }
                            else if (type == "Term")
                            {
                                string[] feesem = cbltext.Split(' ');
                                if (feesem[1] == "1" || feesem[1] == "2" || feesem[1] == "3" || feesem[1] == "4" || feesem[1] == "5")
                                {
                                    if (!htsem.ContainsKey("1"))
                                        htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["1"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("1");
                                        htsem.Add(Convert.ToString("1"), feecatg);
                                    }
                                }
                            }
                            else if (type == "Year")
                            {
                                #region year
                                string[] feesem = cbltext.Split(' ');
                                if (feesem[0] == "1")
                                {
                                    if (!htsem.ContainsKey("1"))
                                        htsem.Add(Convert.ToString("1"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["1"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("1");
                                        htsem.Add(Convert.ToString("1"), feecatg);
                                    }
                                }
                                if (feesem[0] == "2")
                                {
                                    if (!htsem.ContainsKey("2"))
                                        htsem.Add(Convert.ToString("2"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["2"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("2");
                                        htsem.Add(Convert.ToString("2"), feecatg);
                                    }
                                }
                                if (feesem[0] == "3")
                                {
                                    if (!htsem.ContainsKey("3"))
                                        htsem.Add(Convert.ToString("3"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["3"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("3");
                                        htsem.Add(Convert.ToString("3"), feecatg);
                                    }
                                }
                                if (feesem[0] == "4")
                                {
                                    if (!htsem.ContainsKey("4"))
                                        htsem.Add(Convert.ToString("4"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["4"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("4");
                                        htsem.Add(Convert.ToString("4"), feecatg);
                                    }
                                }
                                if (feesem[0] == "5")
                                {
                                    if (!htsem.ContainsKey("5"))
                                        htsem.Add(Convert.ToString("5"), Convert.ToString(cblvalue));
                                    else
                                    {
                                        feecatg = Convert.ToString(htsem["5"]);
                                        feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                        htsem.Remove("5");
                                        htsem.Add(Convert.ToString("5"), feecatg);
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                        else
                        {
                            if (!htsem.ContainsKey(cbltext))
                                htsem.Add(Convert.ToString(cbltext), Convert.ToString(cblvalue));
                            else
                            {
                                feecatg = Convert.ToString(htsem[cbltext]);
                                feecatg = feecatg + "'" + "," + "'" + cblvalue;
                                htsem.Remove(cbltext);
                                htsem.Add(Convert.ToString(cbltext), feecatg);
                            }
                        }
                    }
                }
            }
            ViewState["feecat"] = htsem;
        }
        catch { }
        return htsem;
    }
    protected string getfeeValue(string linkName)
    {
        string feeCode = string.Empty;
        try
        {
            StringBuilder sbFeecat = new StringBuilder();
            if (linkName == "Term")
            {
                string termStr = " and( textval like'" + linkName + " 1%' or textval like'" + linkName + " 2%' or textval like'" + linkName + " 3%' or textval like'" + linkName + " 4%' or textval like'" + linkName + " 5%' or textval like'" + linkName + " 6%') ";
                string selQ = " select  distinct  textval,textcode,len(isnull(textval,1000)) from textvaltable t where college_code='" + ddlcollegename.SelectedItem.Value + "' and textcriteria='FEECA' " + termStr + " order by len(isnull(textval,1000)),textval asc";
                DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                    {
                        sbFeecat.Append(Convert.ToString(dsval.Tables[0].Rows[row]["textcode"]) + "','");
                    }
                    if (sbFeecat.Length > 0)
                    {
                        sbFeecat.Remove(sbFeecat.Length - 3, 3);
                        feeCode = Convert.ToString(sbFeecat);
                    }
                }
            }
        }
        catch { }
        return feeCode;
    }
    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_sem, cbl_sem, sampleTxt, "Semester", "--Select--");
        bindsec();
    }
    protected void bindsem()
    {
        try
        {
            string featDegcode = Convert.ToString(getCblSelectedValue(cbl_dept));
            cbl_sem.Items.Clear();
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            d2.featDegreeCode = featDegcode;
            ds = d2.loadFeecategory(Convert.ToString(ddlcollegename.SelectedItem.Value), usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (linkName == "Term")
                {
                    string termStr = " and( textval like'" + linkName + " 1%' or textval like'" + linkName + " 2%' or textval like'" + linkName + " 3%' or textval like'" + linkName + " 4%' or textval like'" + linkName + " 5%' or textval like'" + linkName + " 6%') ";
                    string selQ = " select  distinct  textval,textcode,len(isnull(textval,1000)) from textvaltable t where college_code='" + ddlcollegename.SelectedItem.Value + "' and textcriteria='FEECA' " + termStr + " order by len(isnull(textval,1000)),textval asc";
                    DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
                    {
                        cbl_sem.DataSource = dsval;
                        cbl_sem.DataTextField = "TextVal";
                        cbl_sem.DataValueField = "TextCode";
                        cbl_sem.DataBind();
                    }
                }
                else
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "TextVal";
                    cbl_sem.DataValueField = "TextCode";
                    cbl_sem.DataBind();
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        cbltext = Convert.ToString(cbl_sem.Items[i].Text);
                    }
                    cb_sem.Checked = true;
                }
            }
        }
        catch { }
    }
    #endregion

    #region sec
    public void bindsec()
    {
        try
        {
            ListItem item = new ListItem("Empty", " ");
            cbl_sect.Items.Clear();
            string build = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        else
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                    }
                }
            }
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(clgvalue);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    cbl_sect.Items.Insert(0, item);
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                        }
                        cb_sect.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_sect, cbl_sect, sampleTxt, "Section", "--Select--");
    }
    #endregion

    #region headerandledger
    public void loadHeader()
    {
        try
        {
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studhed.Items.Clear();
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + clgvalue + "  ";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studhed.DataSource = ds;
                chkl_studhed.DataTextField = "HeaderName";
                chkl_studhed.DataValueField = "HeaderPK";
                chkl_studhed.DataBind();
                for (int i = 0; i < chkl_studhed.Items.Count; i++)
                {
                    chkl_studhed.Items[i].Selected = true;
                }
                chk_studhed.Checked = true;
                ledgerload();
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
            string clgvalue = ddlcollegename.SelectedItem.Value.ToString();
            chkl_studled.Items.Clear();
            string hed = "";
            for (int i = 0; i < chkl_studhed.Items.Count; i++)
            {
                if (chkl_studhed.Items[i].Selected == true)
                {
                    if (hed == "")
                        hed = chkl_studhed.Items[i].Value.ToString();
                    else
                        hed = hed + "','" + "" + chkl_studhed.Items[i].Value.ToString() + "";
                }
            }
            string query1 = " SELECT LedgerPK,LedgerName FROM FM_LedgerMaster L,FS_LedgerPrivilage P WHERE L.LedgerPK = P.LedgerFK   AND P.CollegeCode = L.CollegeCode AND P. UserCode = " + usercode + " AND  Ledgermode='0' and L.CollegeCode = " + clgvalue + "  and L.HeaderFK in('" + hed + "')  order by isnull(l.priority,1000), l.ledgerName asc ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chkl_studled.DataSource = ds;
                chkl_studled.DataTextField = "LedgerName";
                chkl_studled.DataValueField = "LedgerPK";
                chkl_studled.DataBind();
                for (int i = 0; i < chkl_studled.Items.Count; i++)
                {
                    chkl_studled.Items[i].Selected = true;
                }
                chk_studled.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void chkl_studhed_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(chk_studhed, chkl_studhed, sampleTxt, lblheader.Text, "--Select--");
        ledgerload();
    }
    public void chkl_studled_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(chk_studled, chkl_studled, sampleTxt, "Ledger", "--Select--");
    }
    #endregion

    #region financial year
    public void loadfinanceyear()
    {
        try
        {
            string fnalyr = "";
            string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode='" + collegecode + "'  order by FinYearPK desc";
            ds.Dispose();
            ds.Reset();
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
                chkfyear.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void chklsfyear_selected(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(chkfyear, chklsfyear, sampleTxt, "Finance Year", "--Select--");
    }
    #endregion

    #region Include student category setting
    protected void checkdicon_Changed(object sender, EventArgs e)
    {
        try
        {
            if (checkdicon.Checked == true)
            {
                LoadIncludeSetting();
            }
            else
            {
                cblinclude.Items.Clear();
            }
        }
        catch { }
    }
    private void LoadIncludeSetting()
    {
        try
        {
            cblinclude.Items.Clear();
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Course Completed", "1"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Debar", "2"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Discontinue", "3"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Cancel", "4"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Prolong absent", "5"));//added by abarna 1.12.2017
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Apply", "6"));
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("TC Issued", "7"));//abarna
            cblinclude.Items.Add(new System.Web.UI.WebControls.ListItem("Readmission", "8"));//abarna
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    cblinclude.Items[i].Selected = true;
                }
                cbinclude.Checked = true;
            }
        }
        catch { }
    }
    protected void cblinclude_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbinclude, cblinclude, sampleTxt, "Include Setting", "--Select--");
    }
    //modified by abarna 1.12.2017
    protected string getStudCategory()
    {
        string strInclude = string.Empty;
        try
        {
            #region includem
            string cc = "";
            string debar = "";
            string disc = "";
            string cancel = "";
            string pro = "";
            string tcapply = "";
            string tcissued = "";//abarna
            string readmit = "";
            if (cblinclude.Items.Count > 0)
            {
                for (int i = 0; i < cblinclude.Items.Count; i++)
                {
                    if (cblinclude.Items[i].Selected == true)
                    {
                        if (cblinclude.Items[i].Value == "1")
                            cc = " r.cc=1  ";//and  r.ProlongAbsent=0
                        if (cblinclude.Items[i].Value == "2")
                            debar = " r.Exam_Flag like '%debar'";
                        if (cblinclude.Items[i].Value == "3")
                            disc = "r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0 ";
                        if (cblinclude.Items[i].Value == "4")
                            cancel = "  r.DelFlag=2";
                        if (cblinclude.Items[i].Value == "5")
                            pro = " r.ProlongAbsent=1 and r.DelFlag=1";
                        if (cblinclude.Items[i].Value == "6")
                            tcapply = " isnull(r.IsTcIssued,'0')=0 and r.tcapply=1";
                        if (cblinclude.Items[i].Value == "7")
                            tcissued = "r.IsTcIssued=1";//abarna
                        if (cblinclude.Items[i].Value == "8")
                            readmit = "r.app_no in(select app_no from Readmission)";//abarna
                    }
                }
            }
            if (checkdicon.Checked)
            {
                if (cc != "")
                    strInclude = "(r.cc=1)";// and  r.ProlongAbsent=0
                if (debar != "")
                {
                    if (strInclude != "")
                    {
                        //strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        // strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.Exam_Flag like '%debar')";
                    }
                }
                if (disc != "")
                {
                    if (strInclude != "")
                    {
                        strInclude = strInclude.TrimEnd(')');
                        strInclude += " or ";
                        strInclude += " (r.DelFlag=1 and isnull(r.ProlongAbsent,'0')=0)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += " r.DelFlag=1 and  isnull(r.ProlongAbsent,'0')=0)";
                    }
                }
                if (cancel != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "  (r.DelFlag=2)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "  r.DelFlag=2)";
                    }
                }
                if (pro != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += " (r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.ProlongAbsent=1 and r.DelFlag=1)";
                    }
                }
                if (tcapply != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "isnull(r.IsTcIssued,'0')=0 and r.tcapply=1)";
                    }
                }
                if (tcissued != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.IsTcIssued=1)";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.IsTcIssued=1)";
                    }
                }
                if (readmit != "")
                {
                    if (strInclude != "")
                    {
                        // strInclude = strInclude.TrimEnd(')');
                        strInclude += " or";
                        strInclude += "(r.app_no in(select app_no from Readmission))";
                    }
                    else
                    {
                        strInclude += "(";
                        strInclude += "r.app_no in(select app_no from Readmission))";
                    }
                }
                if (strInclude != "")

                    strInclude = "and (" + strInclude + ")";
            }
            //if (!checkdicon.Checked)
            //{
            //    if (cc != "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0)  and r.Exam_Flag<>'debar' and r.DelFlag=0";
            //    if (cc == "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0  and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc == "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0  and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //2
            //    if (cc != "" && debar != "" && disc == "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and r.DelFlag=0";
            //    if (cc != "" && debar == "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    //
            //    if (cc == "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    //3
            //    if (cc != "" && debar != "" && disc != "" && cancel == "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or r.DelFlag=0)";
            //    if (cc != "" && debar == "" && disc != "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and r.Exam_Flag<>'debar' and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc != "" && debar != "" && disc == "" && cancel != "")
            //        strInclude = " and (" + cc + " or r.cc=0) and (" + debar + " or r.Exam_Flag<>'debar') and (" + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = " and r.cc=0 and (" + debar + " or r.Exam_Flag<>'debar') and (" + disc + " or " + cancel + " or r.DelFlag=0)";
            //    if (cc == "" && debar == "" && disc == "" && cancel == "")
            //        strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
            //    if (cc != "" && debar != "" && disc != "" && cancel != "")
            //        strInclude = "";
            //}
            else
            {
                strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0 and isnull(r.ProlongAbsent,'0')=0";

                //if (cc != "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and " + cc + "";
                //if (cc == "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and " + debar + "";
                //if (cc == "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and " + disc + "";
                //if (cc == "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and " + cancel + "";
                ////2
                //if (cc != "" && debar != "" && disc == "" && cancel == "")
                //    strInclude = " and( " + cc + " or " + debar + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + cancel + ")";
                ////
                //if (cc == "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + debar + " or " + disc + ")";
                //if (cc == "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + cancel + ")";
                ////
                //if (cc == "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + disc + " or " + cancel + ")";
                ////3
                //if (cc != "" && debar != "" && disc != "" && cancel == "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + ")";
                //if (cc != "" && debar == "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + disc + " or " + cancel + ")";
                //if (cc != "" && debar != "" && disc == "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + cancel + ")";
                //if (cc == "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + debar + " or " + disc + " or " + cancel + ")";
                //if (cc == "" && debar == "" && disc == "" && cancel == "")
                //    strInclude = " and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";
                //if (cc != "" && debar != "" && disc != "" && cancel != "")
                //    strInclude = " and (" + cc + " or " + debar + " or " + disc + " or " + cancel + ")";
            }
            #endregion
        }
        catch { }
        return strInclude;
    }
    #endregion

    #region student Mode
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_type, cbl_type, sampleTxt, "Type", "--Select--");
    }
    protected void loadStudentMode()
    {
        try
        {
            cbl_type.Items.Clear();
            if (checkSchoolSetting() == 0)
            {
                cbl_type.Items.Add(new ListItem("Old ", "1"));
                cbl_type.Items.Add(new ListItem("New", "3"));
                // cbl_type.Items.Add(new ListItem("Transfer", "2"));
            }
            else
            {
                cbl_type.Items.Add(new ListItem("Regular", "1"));
                cbl_type.Items.Add(new ListItem("Lateral", "3"));
                cbl_type.Items.Add(new ListItem("Transfer", "2"));
                cbl_type.Items.Add(new ListItem("IrRegular", "4"));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                cb_type.Checked = true;
            }
        }
        catch { }
    }
    protected Dictionary<string, string> getstudMode()
    {
        Dictionary<string, string> studMode = new Dictionary<string, string>();
        for (int i = 0; i < cbl_type.Items.Count; i++)
        {
            studMode.Add(cbl_type.Items[i].Text, cbl_type.Items[i].Value);
        }
        return studMode;
    }
    #endregion

    #region student type
    protected void cbl_stutype_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_stutype, cbl_stutype, sampleTxt, "Student Type", "--Select--");
    }
    #endregion

    #region seat type
    protected void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_seat, cbl_seat, sampleTxt, "Seat", "--Select--");
        loadquota();
    }
    public void loadseat()
    {
        try
        {
            cbl_seat.Items.Clear();
            ListItem item = new ListItem("Empty", "0");
            string seat = "";
            string deptquery = "select distinct TextCode,TextVal from TextValTable  where TextCriteria='seat' and college_code='" + collegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();
                cbl_seat.Items.Insert(0, item);
            }
        }
        catch
        {
        }
    }
    #endregion

    #region sub quota

    public void loadquota()//abarna
    {
        try
        {
            cbl_quota.Items.Clear();
            ListItem item = new ListItem("Empty", "0");
            //  string seat = "";

            string SelQuery = "select distinct textcode as code from TextValTable  where TextCriteria='seat' and college_code in('" + collegecode + "')";
            //string itemheader = getFilterValues(SelQuery);
            string itemheader = "";
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    if (itemheader == "")
                        itemheader = cbl_seat.Items[i].Value.ToString();
                    else
                        itemheader = itemheader + "','" + "" + cbl_seat.Items[i].Value.ToString() + "";
                }
            }
            string deptquery = "select distinct quotaname,quotaid from stu_quotaseetinges where settype in('" + itemheader + "') and  collegecode in('" + collegecode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_quota.DataSource = ds;
                cbl_quota.DataTextField = "quotaname";
                cbl_quota.DataValueField = "quotaid";
                cbl_quota.DataBind();
                cbl_quota.Items.Insert(0, item);
            }
        }
        catch
        {
        }
    }
    protected string getFilterValues(string SelQuery)
    {
        string getValues = string.Empty;
        try
        {
            string[] headerFK = new string[0];
            DataSet dsval = d2.select_method_wo_parameter(SelQuery, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    Array.Resize(ref headerFK, headerFK.Length + 1);
                    headerFK[headerFK.Length - 1] = Convert.ToString(dsval.Tables[0].Rows[row]["code"]);
                }
                getValues = string.Join("','", headerFK);
            }
        }
        catch { getValues = string.Empty; }
        return getValues;
    }


    #endregion

    #region religion
    public void loadreligion()
    {
        try
        {
            string religion = "";
            cbl_religion.Items.Clear();
            ListItem item = new ListItem("Empty", "0");
            string reliquery = "SELECT Distinct religion,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + collegecode + "'";
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
                    cbl_religion.Items.Insert(0, item);
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_religion, cbl_religion, sampleTxt, "Religion", "--Select--");
    }
    #endregion

    #region community
    public void loadcommunity()
    {
        try
        {
            cbl_community.Items.Clear();
            ListItem item = new ListItem("Empty", "0");
            string comm = "";
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code ='" + collegecode + "'";
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
                    cbl_community.Items.Insert(0, item);
                }
            }
        }
        catch
        {
        }
    }
    protected void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cb_community, cbl_community, sampleTxt, "Community", "--Select--");
    }
    #endregion

    #region gender
    protected void loadGender()
    {
        cblgender.Items.Clear();
        cblgender.Items.Add(new ListItem("Male", "0"));
        cblgender.Items.Add(new ListItem("Female", "1"));
    }
    #endregion

    #region paymentmode
    public void loadpaid()
    {
        try
        {
            chkl_paid.Items.Clear();
            d2.BindPaymodeToCheckboxList(chkl_paid, usercode, collegecode);
            if (chkl_paid.Items.Count > 0)
            {
                for (int i = 0; i < chkl_paid.Items.Count; i++)
                {
                    chkl_paid.Items[i].Selected = true;
                }
                chk_paid.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void chkl_paid_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(chk_paid, chkl_paid, sampleTxt, "Paid", "--Select--");
    }
    #endregion

    #region Route
    public void bindroute()
    {
        try
        {
            cblroute.Items.Clear();
            ds.Clear();
            string selqry = "select distinct Route_ID from routemaster order by Route_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblroute.DataSource = ds;
                cblroute.DataTextField = "Route_ID";
                cblroute.DataValueField = "Route_ID";
                cblroute.DataBind();
                if (cblroute.Items.Count > 0)
                {
                    for (int i = 0; i < cblroute.Items.Count; i++)
                    {
                        cblroute.Items[i].Selected = true;
                    }
                    cbroute.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void cblroute_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbroute, cblroute, sampleTxt, "Route", "--Select--");
        // binddept();
        loadvechilestage();
        bindvechileid();
    }
    #endregion

    #region vechile id
    public void bindvechileid()
    {
        try
        {
            cblvechile.Items.Clear();
            string route = "";
            for (int i = 0; i < cblroute.Items.Count; i++)
            {
                if (cblroute.Items[i].Selected == true)
                {
                    if (route == "")
                    {
                        route = Convert.ToString(cblroute.Items[i].Value);
                    }
                    else
                    {
                        route += "','" + Convert.ToString(cblroute.Items[i].Value);
                    }
                }
            }
            ds.Clear();
            string selqry = "select distinct Veh_ID from vehicle_master where route in('" + route + "')  order by Veh_ID";
            //if (stream != "")
            //{
            //    selqry = selqry + " and type  in('" + stream + "')";
            //}
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblvechile.DataSource = ds;
                cblvechile.DataTextField = "Veh_ID";
                cblvechile.DataValueField = "Veh_ID";
                cblvechile.DataBind();
                if (cblvechile.Items.Count > 0)
                {
                    for (int i = 0; i < cblvechile.Items.Count; i++)
                    {
                        cblvechile.Items[i].Selected = true;
                    }
                    cbvechile.Checked = true;
                }
                loadvechilestage();
            }
        }
        catch { }
    }
    protected void cblvechile_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbvechile, cblvechile, sampleTxt, "Vechile ID", "--Select--");
        // binddept();
        loadvechilestage();
    }
    #endregion

    #region Stage
    public void loadvechilestage()
    {
        string sqlquery = string.Empty;
        string filter = "";
        cblstage.Items.Clear();
        //   cblstage.Items.Insert(0, new ListItem("All", "-1"));
        string route = "";
        for (int i = 0; i < cblroute.Items.Count; i++)
        {
            if (cblroute.Items[i].Selected == true)
            {
                if (route == "")
                {
                    route = Convert.ToString(cblroute.Items[i].Value);
                }
                else
                {
                    route += "','" + Convert.ToString(cblroute.Items[i].Value);
                }
            }
        }
        string vechile = "";
        for (int i = 0; i < cblvechile.Items.Count; i++)
        {
            if (cblvechile.Items[i].Selected == true)
            {
                if (vechile == "")
                {
                    vechile = Convert.ToString(cblvechile.Items[i].Value);
                }
                else
                {
                    vechile += "','" + Convert.ToString(cblvechile.Items[i].Value);
                }
            }
        }
        if (route != "-1")
        {
            filter = " and v.Route in('" + route + "')";
        }
        if (vechile != "-1")
        {
            filter = filter + ' ' + "and r.Veh_ID in('" + vechile + "')";
        }
        sqlquery = "select distinct Stage_Name from routemaster r,vehicle_master v where Stage_Name is not null and Stage_Name<>'' and v.Veh_ID=r.Veh_ID " + filter + "";
        ds = d2.select_method_wo_parameter(sqlquery, "txt");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Boolean e1 = isNumeric(ds.Tables[0].Rows[i]["Stage_Name"].ToString(), System.Globalization.NumberStyles.Integer);
                if (e1)
                {
                    string Get_Stage = d2.GetFunction("select distinct Stage_Name from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    string Get_Stage_id = d2.GetFunction("select distinct Stage_id from stage_master where Stage_id = '" + ds.Tables[0].Rows[i]["Stage_Name"].ToString() + "'");
                    cblstage.Items.Add(new ListItem(Get_Stage, Get_Stage_id));//Added By SRinath 8/10/2013
                }
                else
                {
                    cblstage.Items.Add(ds.Tables[0].Rows[i]["Stage_Name"].ToString());
                }
            }
            if (cblstage.Items.Count > 0)
            {
                for (int i = 0; i < cblstage.Items.Count; i++)
                {
                    cblstage.Items[i].Selected = true;
                }
                cbstage.Checked = true;
            }
        }
        // cblstage.SelectedIndex = 0;
    }
    protected void cblstage_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbstage, cblstage, sampleTxt, "Stage", "--Select--");
        // binddept();        
    }
    public bool isNumeric(string val, System.Globalization.NumberStyles NumberStyle)
    {
        Double result;
        return Double.TryParse(val, NumberStyle,
            System.Globalization.CultureInfo.CurrentCulture, out result);
    }
    #endregion

    #region hostel
    protected void loadHostel()
    {
        try
        {
            ds.Clear();
            cblhstlname.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblhstlname.DataSource = ds;
                cblhstlname.DataTextField = "HostelName";
                cblhstlname.DataValueField = "HostelMasterPK";
                cblhstlname.DataBind();
                if (cblhstlname.Items.Count > 0)
                {
                    for (int i = 0; i < cblhstlname.Items.Count; i++)
                    {
                        cblhstlname.Items[i].Selected = true;
                    }
                    cbhstlname.Checked = true;
                }
                loadBuilding();
            }
        }
        catch
        {
        }
    }
    protected void cblhstlname_SelectedIndexChange(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbhstlname, cblhstlname, sampleTxt, "Hostel", "--Select--");
        loadBuilding();
    }
    #endregion

    #region building name
    public void loadBuilding()
    {
        try
        {
            string locbuild = "";
            for (int i = 0; i < cblhstlname.Items.Count; i++)
            {
                if (cblhstlname.Items[i].Selected == true)
                {
                    string builname = cblhstlname.Items[i].Value;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            cblbuilding.Items.Clear();
            string bul = d2.GetBuildingCode_inv(locbuild);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblbuilding.DataSource = ds;
                cblbuilding.DataTextField = "Building_Name";
                cblbuilding.DataValueField = "code";
                cblbuilding.DataBind();
                for (int i = 0; i < cblbuilding.Items.Count; i++)
                {
                    cblbuilding.Items[i].Selected = true;
                }
                cbbuilding.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbbuilding_SelectedIndexChange(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbbuilding, cblbuilding, sampleTxt, "Building", "--Select--");
    }
    #endregion

    #region room type
    protected void loadRoomType()
    {
        try
        {
            cblroomtype.Items.Clear();
            string itemname = "select distinct Room_type from Room_Detail where isnull(room_type,'')<>'' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblroomtype.DataSource = ds;
                cblroomtype.DataTextField = "Room_Type";
                cblroomtype.DataValueField = "Room_Type";
                cblroomtype.DataBind();
                if (cblroomtype.Items.Count > 0)
                {
                    for (int i = 0; i < cblroomtype.Items.Count; i++)
                    {
                        cblroomtype.Items[i].Selected = true;
                    }
                    cbroomtype.Checked = true;
                }
                loadroom();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    protected void cblroomtype_SelectedIndexChange(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbroomtype, cblroomtype, sampleTxt, "Room Type", "--Select--");
        loadroom();
    }
    #endregion

    #region Room Name
    public void loadroom()
    {
        try
        {
            string buildname = "";
            for (int i = 0; i < cblbuilding.Items.Count; i++)
            {
                if (cblbuilding.Items[i].Selected == true)
                {
                    string builname = cblbuilding.Items[i].Text;
                    if (buildname == "")
                        buildname = builname;
                    else
                        buildname = buildname + "'" + "," + "'" + builname;
                }
            }
            string roomtype = "";
            for (int i = 0; i < cblroomtype.Items.Count; i++)
            {
                if (cblroomtype.Items[i].Selected == true)
                {
                    string room = cblroomtype.Items[i].Text;
                    if (roomtype == "")
                        roomtype = room;
                    else
                        roomtype = roomtype + "'" + "," + "'" + room;
                }
            }
            cblrommName.Items.Clear();
            //ds = d2.BindRoom(floorname, buildname);changed at sairam 29.09.16
            string itemname = "select distinct Room_Name,Roompk from Room_Detail where Building_Name in('" + buildname + "') and room_type in('" + roomtype + "') order by Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cblrommName.DataSource = ds;
                cblrommName.DataTextField = "Room_Name";
                cblrommName.DataValueField = "Roompk";
                cblrommName.DataBind();
                for (int i = 0; i < cblrommName.Items.Count; i++)
                {
                    cblrommName.Items[i].Selected = true;
                }
                cbrommName.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblrommName_SelectedIndexChange(object sender, EventArgs e)
    {
        TextBox sampleTxt = new TextBox();
        CallCheckboxListChange(cbrommName, cblrommName, sampleTxt, "Room Name", "--Select--");
    }
    #endregion

    protected void rbFeesType_Selected(object sender, EventArgs e)
    {
        txtexcelname.Text = string.Empty;
        spreadDet.Visible = false;
        print.Visible = false;
    }

    protected void getFilterDetails()
    {
    }

    /// <summary>
    /// COLUMN ORDER
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// 

    #region colorder
    protected void lnkcolorder_Click(object sender, EventArgs e)
    {
        txtcolorder.Text = string.Empty;
        txtallot.Text = string.Empty;
        loadcolumnorder();
        columnType();
        // loadcolumns();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        //divcolorder.Visible = true;
    }
    public void loadcolumnorder()
    {
        cblcolumnorder.Items.Clear();
        cblcolumnorderAlt.Items.Clear();
        cblcolumnorder.Items.Add(new ListItem("Roll No", "1"));
        cblcolumnorder.Items.Add(new ListItem("Reg No", "2"));
        cblcolumnorder.Items.Add(new ListItem("Admission No", "3"));
        cblcolumnorder.Items.Add(new ListItem("Student Name", "4"));
        cblcolumnorder.Items.Add(new ListItem("Admitted Date", "5"));
        cblcolumnorder.Items.Add(new ListItem("Course", "6"));
        cblcolumnorder.Items.Add(new ListItem("Department", "7"));
        cblcolumnorder.Items.Add(new ListItem("Semester", "8"));
        cblcolumnorder.Items.Add(new ListItem("Section", "9"));
        cblcolumnorder.Items.Add(new ListItem("Student Mode", "10"));
        cblcolumnorder.Items.Add(new ListItem("Student Type", "11"));
        cblcolumnorder.Items.Add(new ListItem("SeatType", "12"));
        cblcolumnorder.Items.Add(new ListItem("Quota", "31"));//abarna
        cblcolumnorder.Items.Add(new ListItem("Community", "13"));
        cblcolumnorder.Items.Add(new ListItem("Religion", "14"));
        cblcolumnorder.Items.Add(new ListItem("Student Mobile No", "15"));
        cblcolumnorder.Items.Add(new ListItem("Father Mobile No", "16"));
        cblcolumnorder.Items.Add(new ListItem("Mother Mobile No", "17"));
        cblcolumnorder.Items.Add(new ListItem("Route", "18"));
        cblcolumnorder.Items.Add(new ListItem("Vehicle", "19"));
        cblcolumnorder.Items.Add(new ListItem("Stage", "20"));
        cblcolumnorder.Items.Add(new ListItem("Hostel", "21"));
        cblcolumnorder.Items.Add(new ListItem("Building", "22"));
        cblcolumnorder.Items.Add(new ListItem("Room Type", "23"));
        cblcolumnorder.Items.Add(new ListItem("Room Name", "24"));
        cblcolumnorder.Items.Add(new ListItem("Permanent Address", "32"));//abarna
        cblcolumnorder.Items.Add(new ListItem("Contact Address", "33"));//abarna
        if (rblrptType.SelectedIndex == 0)
        {
            cblcolumnorderAlt.Items.Add(new ListItem("Allot", "25"));
            cblcolumnorderAlt.Items.Add(new ListItem("Concession", "26"));
            cblcolumnorderAlt.Items.Add(new ListItem("Scholarship", "27"));
            cblcolumnorderAlt.Items.Add(new ListItem("Total", "28"));
            cblcolumnorderAlt.Items.Add(new ListItem("Paid", "29"));
            cblcolumnorderAlt.Items.Add(new ListItem("Balance", "30"));
            cblcolumnorderAlt.Items.Add(new ListItem("Excess Amount", "34"));//abarna
        }
        else
        {
            cblcolumnorderAlt.Items.Add(new ListItem("Allot", "25"));
            cblcolumnorderAlt.Items.Add(new ListItem("Concession", "26"));
            cblcolumnorderAlt.Items.Add(new ListItem("Demand", "27"));
            cblcolumnorderAlt.Items.Add(new ListItem("Receipt", "28"));
            cblcolumnorderAlt.Items.Add(new ListItem("Balance", "29"));
            cblcolumnorderAlt.Items.Add(new ListItem("Excess Amount", "34"));//abarna
        }
        //cblcolumnorder.Items.Add(new ListItem("Cash", "28"));
        //cblcolumnorder.Items.Add(new ListItem("Cheque", "29"));
        //cblcolumnorder.Items.Add(new ListItem("DD", "30"));
        //cblcolumnorder.Items.Add(new ListItem("Challan", "31"));
        //cblcolumnorder.Items.Add(new ListItem("Online", "32"));
        //cblcolumnorder.Items.Add(new ListItem("Card", "33"));
    }
    protected Hashtable htcolumnValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("Roll No", "r.roll_no as [Roll No]");
            htcol.Add("Reg No", "r.reg_no as [Reg No]");
            htcol.Add("Admission No", "r.roll_admit as [Admission No]");
            htcol.Add("Student Name", "r.stud_name as [Student Name]");
            htcol.Add("Admitted Date", "convert(varchar(10),r.adm_date,103) as [Admitted Date]");
            htcol.Add("Course", "(cast(r.batch_year as nvarchar(10))+'-'+(select c.course_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0))) as [Course]");
            htcol.Add("Department", "(select dt.dept_acronym from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0)) as [Department]");
            htcol.Add("Semester", "r.current_semester as [Semester]");
            htcol.Add("Section", "isnull(r.sections,'') as [sections]");
            htcol.Add("Student Mode", "case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' when r.mode='4' then 'Irregular' end [Mode]");
            htcol.Add("Student Type", "r.Stud_Type as [Student Type]");
            htcol.Add("SeatType", "(select TextVal from TextValtable where TExtCode=isnull(a.seattype,0)) as [SeatType]");
            htcol.Add("Quota", "(select distinct quotaname from stu_quotaseetinges where quotaid=isnull(a.quota,0)) as [Quota]");//abarna
            htcol.Add("Religion", "(select TextVal from TextValtable where TExtCode=isnull(a.religion,0)) as [Religion]");
            htcol.Add("Community", "(select TextVal from TextValtable where TExtCode=isnull(a.community,0)) as [Community]");
            htcol.Add("Student Mobile No", "Student_Mobile as [Student Mobile No]");
            htcol.Add("Father Mobile No", "parentF_Mobile as [Father Mobile No]");
            htcol.Add("Mother Mobile No", "parentM_Mobile as [Mother Mobile No]");
            htcol.Add("Route", "r.Bus_RouteID as [Route]");
            htcol.Add("Vehicle", "r.VehID as [Vehicle]");
            htcol.Add("Stage", "(select distinct stage_name from stage_master  where cast(r.Boarding as int)=stage_id) as [Stage]");
            htcol.Add("Hostel", "(select hn.hostelname from ht_hostelregistration hr,hm_hostelmaster hn where hr.hostelmasterfk=hn.hostelmasterpk and hr.app_no=r.app_no ) as [Hostel]");
            htcol.Add("Building", "(select bl.building_name from ht_hostelregistration hr,building_master bl where hr.buildingfk=bl.code and hr.app_no=r.app_no ) as [Building]");
            htcol.Add("Room Type", "(select ro.room_type from ht_hostelregistration hr,room_detail ro where hr.roomfk=ro.roompk and hr.app_no=r.app_no ) as [Room Type]");
            htcol.Add("Room Name", "(select ro.room_name from ht_hostelregistration hr,room_detail ro where hr.roomfk=ro.roompk and hr.app_no=r.app_no ) as [Room Name]");
            htcol.Add("Permanent Address", "a.parent_addressP+''+a.Streetp+''+a.parent_pincodep as [Permanent Address]");//abarna
            htcol.Add("Contact Address", "a.parent_addressC+''+a.Streetc+''+a.parent_pincodec as [Contact Address]");//abarna
            if (rblrptType.SelectedIndex == 0)
            {
                htcol.Add("Allot", "Sum(Feeamount) as [Allot]");
                htcol.Add("Concession", "Sum(Deductamout) as [Concession]");
                htcol.Add("Scholarship", "Sum(FromGovtAmt) as [Scholarship]");
                htcol.Add("Total", "Sum(Totalamount) as [Total]");
                htcol.Add("Paid", "Sum(paidamount) as [Paid]");
                htcol.Add("Balance", "Sum(balamount) as [Balance]");
                htcol.Add("Excess Amount", "'' as [ExcessAmount]");

            }
            else
            {
                htcol.Add("Allot", "Sum(Feeamount) as [Allot]");
                htcol.Add("Concession", "Sum(Deductamout) as [Concession]");
                htcol.Add("Demand", "Sum(Totalamount) as [Demand]");
                htcol.Add("Receipt", "Sum(paidamount) as [Receipt]");
                htcol.Add("Balance", "Sum(balamount) as [Balance]");
                htcol.Add("Excess Amount", "'' as [ExcessAmount]");

            }
            //htcol.Add("Cash", "Cash");
            //htcol.Add("Cheque", "Cheque");
            //htcol.Add("DD", "DD");
            //htcol.Add("Challan", "Challan");
            //htcol.Add("Online", "Online");
            //htcol.Add("Card", "Card");
        }
        catch { }
        return htcol;
    }
    protected Hashtable htcolumnHeaderValue()
    {
        Hashtable htcol = new Hashtable();
        try
        {
            htcol.Add("r.roll_no as [Roll No]", "Roll No");
            htcol.Add("r.reg_no as [Reg No]", "Reg No");
            htcol.Add("r.roll_admit as [Admission No]", "Admission No");
            htcol.Add("r.stud_name as [Student Name]", "Student Name");
            htcol.Add("convert(varchar(10),r.adm_date,103) as [Admitted Date]", "Admitted Date");
            htcol.Add("(cast(r.batch_year as nvarchar(10))+'-'+(select c.course_name from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0))) as [Course]", "Course");
            htcol.Add("(select dt.dept_acronym from degree d,department dt,course c where d.dept_code=dt.dept_code and c.course_id=d.course_id and d.degree_code=isnull( r.degree_code,0)) as [Department]", "Department");
            htcol.Add("r.current_semester as [Semester]", "Semester");
            htcol.Add("isnull(r.sections,'') as [sections]", "Section");
            htcol.Add("case when r.mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' when r.mode='4' then 'Irregular' end [Mode]", "Student Mode");
            htcol.Add("r.Stud_Type as [Student Type]", "Student Type");
            htcol.Add("(select TextVal from TextValtable where TExtCode=isnull(a.seattype,0)) as [SeatType]", "SeatType");
            htcol.Add("(select distinct quotaname from stu_quotaseetinges where quotaid=isnull(a.quota,0)) as [Quota]", "Quota");//abarna
            htcol.Add("(select TextVal from TextValtable where TExtCode=isnull(a.religion,0)) as [Religion]", "Religion");
            htcol.Add("(select TextVal from TextValtable where TExtCode=isnull(a.community,0)) as [Community]", "Community");
            htcol.Add("Student_Mobile as [Student Mobile No]", "Student Mobile No");
            htcol.Add("parentF_Mobile as [Father Mobile No]", "Father Mobile No");
            htcol.Add("parentM_Mobile as [Mother Mobile No]", "Mother Mobile No");
            htcol.Add("r.Bus_RouteID as [Route]", "Route");
            htcol.Add("r.VehID as [Vehicle]", "Vehicle");
            htcol.Add("(select distinct stage_name from stage_master  where cast(r.Boarding as int)=stage_id) as [Stage]", "Stage");
            htcol.Add("(select hn.hostelname from ht_hostelregistration hr,hm_hostelmaster hn where hr.hostelmasterfk=hn.hostelmasterpk and hr.app_no=r.app_no ) as [Hostel]", "Hostel");
            htcol.Add("(select bl.building_name from ht_hostelregistration hr,building_master bl where hr.buildingfk=bl.code and hr.app_no=r.app_no ) as [Building]", "Building");
            htcol.Add("(select ro.room_type from ht_hostelregistration hr,room_detail ro where hr.roomfk=ro.roompk and hr.app_no=r.app_no ) as [Room Type]", "Room Type");
            htcol.Add("(select ro.room_name from ht_hostelregistration hr,room_detail ro where hr.roomfk=ro.roompk and hr.app_no=r.app_no ) as [Room Name]", "Room Name");
            htcol.Add("a.parent_addressP+''+a.Streetp+''+a.parent_pincodep as [Permanent Address]", "Permanent Address");//abarna
            htcol.Add("a.parent_addressC+''+a.Streetc+''+a.parent_pincodec as [Contact Address]", "Contact Address");//abarna
            if (rblrptType.SelectedIndex == 0)
            {
                htcol.Add("Sum(Feeamount) as [Allot]", "Allot");
                htcol.Add("Sum(Deductamout) as [Concession]", "Concession");
                htcol.Add("Sum(FromGovtAmt) as [Scholarship]", "Scholarship");
                htcol.Add("Sum(Totalamount) as [Total]", "Total");
                htcol.Add("Sum(paidamount) as [Paid]", "Paid");
                htcol.Add("Sum(balamount) as [Balance]", "Balance");
                htcol.Add("'' as [ExcessAmount]", "ExcessAmount");
            }
            else
            {
                htcol.Add("Sum(Feeamount) as [Allot]", "Allot");
                htcol.Add("Sum(Deductamout) as [Concession]", "Concession");
                htcol.Add("Sum(Totalamount) as [Demand]", "Demand");
                htcol.Add("Sum(paidamount) as [Receipt]", "Receipt");
                htcol.Add("Sum(balamount) as [Balance]", "Balance");
                htcol.Add("'' as [ExcessAmount]", "ExcessAmount");

            }
        }
        catch { }
        return htcol;
    }
    protected void btncolorderOK_Click(object sender, EventArgs e)
    {
        // loadcolumns();
        divcolorder.Visible = true;
        if (getsaveColumnOrder())
        {
            divcolorder.Attributes.Add("Style", "display:none;");
        }
    }
    protected bool getsaveColumnOrder()
    {
        bool boolSave = false;
        try
        {
            string strText = string.Empty;
            string strTextAlt = string.Empty;
            if (cblcolumnorder.Items.Count > 0)
                strText = Convert.ToString(getCblSelectedTextwithout(cblcolumnorder));
            if (cblcolumnorderAlt.Items.Count > 0)
                strTextAlt = Convert.ToString(getCblSelectedTextwithout(cblcolumnorderAlt));
            if (!string.IsNullOrEmpty(strText))
                strText = Convert.ToString(txtcolorder.Text);
            if (!string.IsNullOrEmpty(strTextAlt))
                strTextAlt = Convert.ToString(txtallot.Text);
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0" && !string.IsNullOrEmpty(strText))
            {
                if (!string.IsNullOrEmpty(strTextAlt))
                    strText += "$" + strTextAlt;
                string SelQ = " if exists (select * from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "')update New_InsSettings set linkvalue='" + strText + "' where  LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "' else insert into New_InsSettings(LinkName,linkvalue,user_code,college_code) values('" + linkName + "','" + strText + "','" + usercode + "','" + Usercollegecode + "')";
                int insQ = d2.update_method_wo_parameter(SelQ, "Text");
                boolSave = true;
                getOrderBySelectedColumn();
                if (rblrptType.SelectedIndex == 1)
                    getRangeSelectedColumn();
            }
            if (!boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please select corresponding values!')", true);
            }
        }
        catch { }
        return boolSave;
    }
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
            string linkname = "DFCR column order settings";
            string columnvalue = "";
            int clsupdate = 0;
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (columncount() == true)
            {
                if (cblcolumnorder.Items.Count > 0)
                {
                    colord.Clear();
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                            if (columnvalue == "")
                                columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                            else
                                columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                        }
                    }
                }
            }
            else if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    colord.Clear();
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colord.Add(Convert.ToString(valuesplit[k]));
                                if (columnvalue == "")
                                    columnvalue = Convert.ToString(valuesplit[k]);
                                else
                                    columnvalue = columnvalue + ',' + Convert.ToString(valuesplit[k]);
                            }
                        }
                    }
                }
            }
            else
            {
                colord.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    colord.Add(Convert.ToString(cblcolumnorder.Items[i].Value));
                    if (columnvalue == "")
                        columnvalue = Convert.ToString(cblcolumnorder.Items[i].Value);
                    else
                        columnvalue = columnvalue + ',' + Convert.ToString(cblcolumnorder.Items[i].Value);
                }
            }
            if (columnvalue != "" && columnvalue != null)
            {
                string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "') update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode + "' else insert into New_InsSettings (LinkName,LinkValue,usercode,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + collegecode + "')";
                clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
            }
            if (clsupdate == 1)
            {
                string sel = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + collegecode + "' ";
                DataSet dscolor = new DataSet();
                dscolor.Clear();
                dscolor = d2.select_method_wo_parameter(sel, "Text");
                if (dscolor.Tables.Count > 0)
                {
                    int count = 0;
                    if (dscolor.Tables[0].Rows.Count > 0)
                    {
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
                                        count++;
                                    }
                                    if (count == cblcolumnorder.Items.Count)
                                        cb_column.Checked = true;
                                    else
                                        cb_column.Checked = false;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
    }
    protected Dictionary<int, string> getHeaderCol()
    {
        Dictionary<int, string> hdColumn = new Dictionary<int, string>();
        hdColumn.Add(1, "Allot");
        hdColumn.Add(2, "Concession");
        hdColumn.Add(3, "Scholarship");
        hdColumn.Add(4, "Total");
        hdColumn.Add(5, "Paid");
        hdColumn.Add(6, "Balance");
        return hdColumn;
    }
    protected Dictionary<int, string> getPaymodeCol()
    {
        Dictionary<int, string> payModeCol = new Dictionary<int, string>();
        for (int row = 0; row < chkl_paid.Items.Count; row++)
        {
            if (chkl_paid.Items[row].Selected)
            {
                if (!payModeCol.ContainsKey(Convert.ToInt32(chkl_paid.Items[row].Value)))
                {
                    payModeCol.Add(Convert.ToInt32(chkl_paid.Items[row].Value), Convert.ToString(chkl_paid.Items[row].Text));
                }
            }
        }
        if (payModeCol.Count > 0)
            payModeCol.Add(7, "Total");
        return payModeCol;
    }
    #endregion

    #region report type added dropdown
    //protected void btnAdd_OnClick(object sender, EventArgs e)
    //{
    //}
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        selectReportType();
    }
    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }
    //type save
    protected void btnaddtype_Click(object sender, EventArgs e)
    {
        try
        {
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string strDesc = Convert.ToString(txtdesc.Text);
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string linkName = string.Empty;
                if (rblrptType.SelectedIndex == 0)
                    linkName = "FinanceUniverselReportDet";
                else
                    linkName = "FinanceUniverselReportCum";
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkName + "' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkName + "' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','" + linkName + "','" + Usercollegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
            }
            columnType();
            divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        }
        catch { }
    }
    public void columnType()
    {
        string Usercollegecode = string.Empty;
        //if (Session["collegecode"] != null)
        //    Usercollegecode = Convert.ToString(Session["collegecode"]);
        if (ddlcollegename.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        ddlreport.Items.Clear();
        ddlMainreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkName = string.Empty;
            if (rblrptType.SelectedIndex == 0)
                linkName = "FinanceUniverselReportDet";
            else
                linkName = "FinanceUniverselReportCum";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkName + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                //main search filter
                ddlMainreport.DataSource = ds;
                ddlMainreport.DataTextField = "MasterValue";
                ddlMainreport.DataValueField = "MasterCode";
                ddlMainreport.DataBind();
                // ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
                getOrderBySelectedColumn();
                if (rblrptType.SelectedIndex == 1)
                    getRangeSelectedColumn();
            }
            else
            {
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
                ddlMainreport.Items.Insert(0, new ListItem("Select", "0"));
                ddlordBy.Items.Clear();
                ddlordBy.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    protected void selectReportType()
    {
        try
        {
            bool boolClear = false;
            bool boolcheck = false;
            string getName = string.Empty;
            txtcolorder.Text = string.Empty;
            string strText = string.Empty;
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlreport.SelectedItem.Text);
            string frstName = string.Empty;
            string sndName = string.Empty;
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                getName = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'and user_code='" + usercode + "' ");
                if (!string.IsNullOrEmpty(getName) && getName != "0")
                {
                    bool boolcolOrd = false;
                    string[] mainrpt = getName.Split('$');//for two type of column order
                    foreach (string firstN in mainrpt)
                    {
                        string[] splName = firstN.Split(',');
                        if (splName.Length > 0)
                        {
                            if (!boolcolOrd)
                            {
                                frstName = Convert.ToString(mainrpt[0]);
                                for (int sprow = 0; sprow < splName.Length; sprow++)
                                {
                                    for (int flt = 0; flt < cblcolumnorder.Items.Count; flt++)
                                    {
                                        if (splName[sprow].Trim() == cblcolumnorder.Items[flt].Text.Trim())
                                        {
                                            cblcolumnorder.Items[flt].Selected = true;
                                            boolcheck = true;
                                            // strText += cblcolumnorder.Items[flt].Text;
                                        }
                                    }
                                    boolcolOrd = true;
                                    boolClear = true;
                                }
                            }
                            else
                            {
                                sndName = Convert.ToString(mainrpt[1]);
                                for (int sprow = 0; sprow < splName.Length; sprow++)
                                {
                                    for (int flt = 0; flt < cblcolumnorderAlt.Items.Count; flt++)
                                    {
                                        if (splName[sprow].Trim() == cblcolumnorderAlt.Items[flt].Text.Trim())
                                        {
                                            cblcolumnorderAlt.Items[flt].Selected = true;
                                            boolcheck = true;
                                            boolClear = true;
                                            // strText += cblcolumnorder.Items[flt].Text;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (!boolClear)
            {
                txtcolorder.Text = string.Empty;
                txtallot.Text = string.Empty;
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
                for (int i = 0; i < cblcolumnorderAlt.Items.Count; i++)
                {
                    cblcolumnorderAlt.Items[i].Selected = false;
                }
                cb_column.Checked = false;
            }
            if (boolcheck)
            {
                txtcolorder.Text = frstName;
                txtallot.Text = sndName;
            }
        }
        catch { }
    }
    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        //if (Session["collegecode"] != null)
        //    Usercollegecode = Convert.ToString(Session["collegecode"]);
        if (ddlcollegename.Items.Count > 0)
            Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            int delQ = 0;
            string linkNames = string.Empty;
            if (rblrptType.SelectedIndex == 0)
                linkNames = "FinanceUniverselReportDet";
            else
                linkNames = "FinanceUniverselReportCum";
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'", "Text")), out delQ);
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='" + linkNames + "' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);
        }
        if (delMQ > 0)
        {
            txtcolorder.Text = string.Empty;
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
            }
            cb_column.Checked = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
        columnType();
        divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
    }
    #endregion


    protected void rblrptType_Selected(object sender, EventArgs e)
    {
        ddlMainreport_Selected(sender, e);
        txtexcelname.Text = string.Empty;
        spreadDet.Visible = false;
        print.Visible = false;
        columnType();
        tdPaid.Visible = false;
        tdRange.Visible = false;
        if (rblrptType.SelectedIndex == 0)
            tdPaid.Visible = true;
        else
            tdRange.Visible = true;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        bool boolCheck = false;
        bool schoolCheck = false;
        if (checkSchoolSetting() == 0)
            schoolCheck = true;
        string groupStr = string.Empty;
        string AltColumn = string.Empty;

        string selColumn = getSelectedColumn(ref AltColumn);//get selected column name
        ds.Clear();
        if (rblrptType.SelectedIndex == 0)//detailed report
        {
            ds = getDetails(selColumn, AltColumn);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!schoolCheck)//college
                {
                    if (rblsemType.SelectedIndex == 0)
                        bindSpreadColumnYearwise(selColumn, AltColumn, ds);
                    else
                        bindSpreadColumnAllotedFeecategory(selColumn, AltColumn, ds);
                }
                else//school
                {
                    if (rblsemType.SelectedIndex == 0)
                        bindSpreadColumnYearwiseSchool(selColumn, AltColumn, ds);
                    else
                        bindSpreadColumnAllotedFeecategorySchool(selColumn, AltColumn, ds);
                }
            }
            else
                boolCheck = true;
        }
        else//cumulative report
        {
            ds = getDetailsCumulative(selColumn, AltColumn);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
            {
                if (!schoolCheck)//college
                {
                    if (rblsemType.SelectedIndex == 0)
                        bindSpreadColumnYearwiseCum(selColumn, AltColumn, ds);
                    else
                        bindSpreadColumnAllotedFeecategoryCum(selColumn, AltColumn, ds);
                }
                else//school
                {
                    if (rblsemType.SelectedIndex == 0)
                        bindSpreadColumnYearwiseCumSchool(selColumn, AltColumn, ds);
                    else
                        bindSpreadColumnAllotedFeecategoryCumSchool(selColumn, AltColumn, ds);
                }
            }
            else
                boolCheck = true;
        }
        if (boolCheck)
        {
            txtexcelname.Text = string.Empty;
            spreadDet.Visible = false;
            print.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
        }
    }

    #region detailed
    protected DataSet getDetails(string selColumn, string AltColumn)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;
            string sec = string.Empty;
            string studMode = string.Empty;
            string seatType = string.Empty;
            string studCatg = string.Empty;
            string studType = string.Empty;
            string religioN = string.Empty;
            string communitY = string.Empty;
            string feeCat = string.Empty;
            string hdFK = string.Empty;
            string lgFK = string.Empty;
            string fnlYR = string.Empty;
            string routeID = string.Empty;
            string vehID = string.Empty;
            string stagE = string.Empty;
            string hstlName = string.Empty;
            string buildName = string.Empty;
            string roomType = string.Empty;
            string roomName = string.Empty;
            string payMode = string.Empty;
            string hdOrLeg = string.Empty;
            string grPhdOrLeg = string.Empty;
            string colmHdLg = string.Empty;
            string curSem = string.Empty;
            string gendeR = string.Empty;
            string secText = string.Empty;
            string seatText = string.Empty;
            string religtxt = string.Empty;
            string commtxt = string.Empty;
            string strOrderBy = string.Empty;
            string subquota = string.Empty;//abarna
            //-------------------------------------------
            //date filter added by abarna
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            //-------------------------------------------
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedValue);
            if (cbl_batch.Items.Count > 0)
                batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (cbl_dept.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (cbl_sem.Items.Count > 0)
                feeCat = Convert.ToString(getCblSelectedValue(cbl_sem));
            if (cbl_sect.Items.Count > 0)
            {
                sec = Convert.ToString(getCblSelectedValue(cbl_sect));
                secText = Convert.ToString(getCblSelectedText(cbl_sect));
            }
            if (chkl_studhed.Items.Count > 0)
                hdFK = Convert.ToString(getCblSelectedValue(chkl_studhed));
            if (chkl_studled.Items.Count > 0)
                lgFK = Convert.ToString(getCblSelectedValue(chkl_studled));
            if (chklsfyear.Items.Count > 0)
                fnlYR = Convert.ToString(getCblSelectedValue(chklsfyear));
            if (chkl_paid.Items.Count > 0)
                payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            if (cbIncStud.Checked)//student value value if available only
            {
                if (cbl_type.Items.Count > 0)
                    studMode = Convert.ToString(getCblSelectedValue(cbl_type));
                if (cbl_seat.Items.Count > 0)
                {
                    seatType = Convert.ToString(getCblSelectedValue(cbl_seat));
                    seatText = Convert.ToString(getCblSelectedText(cbl_seat));
                }
                if (cbl_quota.Items.Count > 0)
                {
                    subquota = Convert.ToString(getCblSelectedValue(cbl_quota));//abarna
                }
                if (cblinclude.Items.Count > 0)

                    studCatg = getStudCategory();


                //Convert.ToString(getCblSelectedValue(cblinclude));
                if (cbl_stutype.Items.Count > 0)
                    studType = Convert.ToString(getCblSelectedValue(cbl_stutype));
                if (cbl_religion.Items.Count > 0)
                {
                    religioN = Convert.ToString(getCblSelectedValue(cbl_religion));
                    religtxt = Convert.ToString(getCblSelectedText(cbl_religion));
                }
                if (cbl_community.Items.Count > 0)
                {
                    communitY = Convert.ToString(getCblSelectedValue(cbl_community));
                    commtxt = Convert.ToString(getCblSelectedText(cbl_community));
                }
                if (cblgender.Items.Count > 0)
                    gendeR = Convert.ToString(getCblSelectedValue(cblgender));
            }
            if (cbIncTrans.Checked)//transport value if available only
            {
                if (cblroute.Items.Count > 0)
                    routeID = Convert.ToString(getCblSelectedValue(cblroute));
                if (cblvechile.Items.Count > 0)
                    vehID = Convert.ToString(getCblSelectedValue(cblvechile));
                if (cblstage.Items.Count > 0)
                    stagE = Convert.ToString(getCblSelectedValue(cblstage));
            }
            if (cbIncHstl.Checked)//hostel value if available only
            {
                if (cblhstlname.Items.Count > 0)
                    hstlName = Convert.ToString(getCblSelectedValue(cblhstlname));
                if (cblbuilding.Items.Count > 0)
                    buildName = Convert.ToString(getCblSelectedValue(cblbuilding));
                if (cblroomtype.Items.Count > 0)
                    roomType = Convert.ToString(getCblSelectedValue(cblroomtype));
                if (cblrommName.Items.Count > 0)
                    roomName = Convert.ToString(getCblSelectedText(cblrommName));
            }
            string strFinYrFk = string.Empty;
            string strActualFk = string.Empty;
            if (checkSchoolSetting() == 0)//school setting added here
            {
                strFinYrFk = ",f.finyearfk";
                strActualFk = ",f.actualfinyearfk";
            }
            if (rbFeesType.SelectedIndex == 0)
            {
                hdOrLeg = ",f.Headerfk,f.feecategory,r.app_no,r.batch_year" + strFinYrFk + "";
                grPhdOrLeg = " f.Headerfk,f.feecategory,r.app_no,r.batch_year " + strFinYrFk + "";
                colmHdLg = " distinct f.Headerfk as PK";
            }
            else
            {
                hdOrLeg = ",f.ledgerfk,f.headerfk,f.feecategory,r.app_no,r.batch_year " + strFinYrFk + "";
                grPhdOrLeg = " f.ledgerfk,f.headerfk,f.feecategory,r.app_no,r.batch_year " + strFinYrFk + "";
                colmHdLg = " distinct f.ledgerfk as PK,f.headerfk as hdFK";
            }
            if (rblsemType.SelectedIndex == 0)
            {
                curSem = getCurrentSemester(batch, collegecode, getStudCategory());
            }
            if (ddlordBy.Items.Count > 0)//order by column 
            {
                strOrderBy = " order by " + Convert.ToString(ddlordBy.SelectedValue) + "";
                if (ddlordBy.SelectedItem.Text == "Department")
                {
                    strOrderBy = " order by r.degree_code";
                }
            }
            string strPaid = string.Empty;
            if (!string.IsNullOrEmpty(AltColumn) && AltColumn.Contains("Sum(paidamount) as [Paid]") && !AltColumn.Contains("Sum(Feeamount) as [Allot]") && !AltColumn.Contains("Sum(Deductamout) as [Concession]") && !AltColumn.Contains("Sum(FromGovtAmt) as [Scholarship]") && !AltColumn.Contains("Sum(Totalamount) as [Total]") && !AltColumn.Contains("Sum(balamount) as [Balance]"))
                strPaid = " having sum(paidamount)>0";
            else if (!string.IsNullOrEmpty(AltColumn) && !AltColumn.Contains("Sum(paidamount) as [Paid]") && !AltColumn.Contains("Sum(Feeamount) as [Allot]") && !AltColumn.Contains("Sum(Deductamout) as [Concession]") && !AltColumn.Contains("Sum(FromGovtAmt) as [Scholarship]") && !AltColumn.Contains("Sum(Totalamount) as [Total]") && AltColumn.Contains("Sum(balamount) as [Balance]"))
                strPaid = " having sum(balamount)>0";
            #endregion
            string selQ = string.Empty;
            if (!cbIncHstl.Checked)//except hostel
            {
                #region Query
                if (!string.IsNullOrEmpty(selColumn))
                    selColumn = " distinct r.app_no,r.current_semester,r.batch_year,r.degree_code" + strFinYrFk + ", " + selColumn;
                //student details
                selQ = " select " + selColumn + " from registration r,ft_feeallot f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " and isnull(f.istransfer,'0')='0'";//and( isnull(f.totalamount,'0')<>'0'
                //if (!string.IsNullOrEmpty(payMode))
                //    selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (!string.IsNullOrEmpty(strOrderBy))
                    selQ += strOrderBy;
                // selQ += strPaid;
                selQ += " select " + AltColumn + "" + hdOrLeg + " from registration r,ft_feeallot f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')   " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                if (!string.IsNullOrEmpty(payMode))
                    selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                selQ += " group by " + grPhdOrLeg + "";
                selQ += strPaid;
                //get header or ledgerfk to bind spread column header            
                selQ += " select " + colmHdLg + " from registration r,ft_feeallot f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "') " + studCatg + " and isnull(f.istransfer,'0')='0'";
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                selQ += " order by f.headerfk";
                //Paymode value get cash,cheque,dd,challan,online 
                selQ += " select r.app_no,paymode,sum(debit) as paid,feecategory,r.batch_year" + strActualFk + " from registration r,ft_findailytransaction f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "') and isnull(f.iscanceled,'0')='0' and isnull(paid_Istransfer,'0')='0' " + studCatg + " ";
                if (!string.IsNullOrEmpty(payMode))
                    selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (cbdate.Checked)
                    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";//abarna added 03.09.2018
                selQ += " group by r.app_no,paymode,feecategory,r.batch_year" + strActualFk + "";


                //excess
                selQ += " select sum(isnull(f.ExcessAmt,0)-isnull(f.AdjAmt,0)) as BalanceAmt,f.app_no,f.feecategory,ft.headerfk,ft.ledgerfk from registration r,FT_ExcessDet f,applyn a,FT_ExcessLedgerDet ft where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and ft.headerfk in('" + hdFK + "') and ft.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " ";//and isnull(f.totalamount,'0')<>'0'
                //  if (!string.IsNullOrEmpty(payMode))
                //  selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                //if (cbdate.Checked)
                //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "' ";//abarna 03.09.2018
                selQ += " and ft.FeeCategory =f.FeeCategory and ft.ExcessDetFK =f.ExcessDetPK";
                selQ += " group by f.app_no,f.feecategory,ft.headerfk,ft.ledgerfk,r.batch_year" + strFinYrFk + "";
                // selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance
                #endregion
            }
            else
            {
                #region Query
                if (!string.IsNullOrEmpty(selColumn))
                    selColumn = " distinct r.app_no,r.current_semester,r.batch_year,r.degree_code" + strFinYrFk + ", " + selColumn;
                //student details
                selQ = " select " + selColumn + " from registration r,ft_feeallot f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (!string.IsNullOrEmpty(strOrderBy))
                    selQ += strOrderBy;
                selQ += " select " + AltColumn + "" + hdOrLeg + " from registration r,ft_feeallot f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')   and f.finyearfk in('" + fnlYR + "') " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                if (!string.IsNullOrEmpty(payMode))
                    selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                selQ += " group by " + grPhdOrLeg + "";
                selQ += strPaid;
                //get header or ledgerfk to bind spread column header            
                selQ += " select " + colmHdLg + " from registration r,ft_feeallot f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "') " + studCatg + " and isnull(f.istransfer,'0')='0'";
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                selQ += " order by f.headerfk";
                //Paymode value get cash,cheque,dd,challan,online 
                selQ += " select r.app_no,paymode,sum(debit) as paid,feecategory,r.batch_year" + strActualFk + " from registration r,ft_findailytransaction f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "') and isnull(f.iscanceled,'0')='0' and isnull(paid_Istransfer,'0')='0' " + studCatg + "";
                if (!string.IsNullOrEmpty(payMode))
                    selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (cbdate.Checked)
                    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";//abarna added 03.09.2018
                selQ += " group by r.app_no,paymode,feecategory,r.batch_year" + strActualFk + "";


                //excess
                selQ += " select sum(isnull(f.ExcessAmt,0)-isnull(f.AdjAmt,0)) as BalanceAmt,f.app_no,f.feecategory from registration r,FT_ExcessDet f,applyn a,ht_hostelregistration htr,room_detail rd,FT_ExcessLedgerDet ft where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and ft.headerfk in('" + hdFK + "') and ft.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " ";//and isnull(f.totalamount,'0')<>'0'
                //  if (!string.IsNullOrEmpty(payMode))
                //  selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                //if (cbdate.Checked)
                //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "' ";//abarna 03.09.2018
                selQ += " and ft.FeeCategory =f.FeeCategory and ft.ExcessDetFK =f.ExcessDetPK";
                selQ += " group by f.app_no,f.feecategory,ft.headerfk,ft.ledgerfk,r.batch_year" + strFinYrFk + "";
                // selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance
                #endregion
            }
            //allot and paid detials
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { dsload.Clear(); }
        return dsload;
    }
    protected string getCurrentSemester(string batch, string collegecode, string strRen)
    {
        string curSem = string.Empty;
        StringBuilder sbCurSem = new StringBuilder();

        string selQ = "select distinct current_semester from registration r where r.batch_year in('" + batch + "') and r.college_code='" + collegecode + "' " + strRen + "";

        DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
        if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
        {
            for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
            {
                sbCurSem.Append(Convert.ToString(dsval.Tables[0].Rows[row]["current_semester"]) + "','");
            }
            if (sbCurSem.Length > 0)
            {
                sbCurSem.Remove(sbCurSem.Length - 3, 3);
                curSem = Convert.ToString(sbCurSem);
            }
        }
        return curSem;
    }
    //college detailed functions
    protected void bindSpreadColumnYearwise(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            if (rbFeesType.SelectedIndex == 0)
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            else
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            #region Column header Bind
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int mergeCount = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                int col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
                mergeCount++;
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            Hashtable htHDName = getHeaderFK();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            if (rbFeesType.SelectedIndex == 0)
            {
                #region header
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    string hedName = Convert.ToString(htHDName[hdFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, firstCol, 1, totcolcnt);
                    }
                }
                #endregion
            }
            else
            {
                #region Ledger
                spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
                string oldHDFK = string.Empty;
                bool boolOld = false;
                string hdFK = string.Empty;
                int oldHDCnt = 0;
                int totOldCnt = 0;
                ArrayList arHdFK = new ArrayList();
                Hashtable htName = getHDName();
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string ldFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["HdFK"]);
                    string hedName = Convert.ToString(htHDName[ldFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolOld)
                                oldHDCnt = alTcol;
                            boolOld = true;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + ldFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                            totOldCnt++;
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, firstCol, 1, totcolcnt);
                    }
                    if (!arHdFK.Contains(hdFK))
                    {
                        if (arHdFK.Count > 0)
                        {
                            string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                            totOldCnt = 0;
                            boolOld = false;
                        }
                        oldHDFK = hdFK;//old headerfk 
                        arHdFK.Add(hdFK);
                    }
                }
                if (arHdFK.Count > 0)
                {
                    string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                }
                //oldHDFK = hdFK;//old headerfk 
                //arHdFK.Add(hdFK);
                //boolOld = false;
                //totOldCnt = 0;
                #endregion
            }
            #region paymode
            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int check = 0;
            bool boolPayCol = false;
            int totcolcntPay = 0;
            int rowCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
                rowCnt = 1;
            else
                rowCnt = 2;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva = spreadDet.Sheets[0].ColumnCount++;
                    if (!boolPayCol)
                        check = checkva;
                    boolPayCol = true;
                    int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), colPay);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                    totcolcntPay++;
                }
            }
            if (totcolcntPay > 0)//header name bind
            {
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Text = "Paymode";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].ColumnCount++;
                int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Paid";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotPaid.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                spreadDet.Sheets[0].ColumnCount++;
                colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Balance";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotBal.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                if (rbFeesType.SelectedIndex == 0)
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 1, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 2, spreadDet.Sheets[0].ColumnCount - 1);
                }
                else
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 2, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 3, spreadDet.Sheets[0].ColumnCount - 1);
                }
            }
            #endregion
            #endregion
            #endregion
            #region value

            Hashtable htAbstract = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            string dvName = string.Empty;
            bool boolLedger = false;
            int tblCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
            {
                dvName = " headerfk";
                tblCnt = 4;
            }
            else
            {
                dvName = " ledgerfk";
                boolLedger = true;
                tblCnt = 5;
            }
            DataTable dtYears = getFeeCat();
            Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                DataSet dsNormal = ds.Copy();
                try
                {
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                    DataSet dsFinal = new DataSet();
                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        DataTable dtHeader = ds.Tables[2].DefaultView.ToTable();
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "'";
                            DataTable dtYear = ds.Tables[0].DefaultView.ToTable();
                            ds.Tables[1].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            ds.Tables[3].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtPaid = ds.Tables[3].DefaultView.ToTable();
                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtYear);
                                dsFinal.Tables.Add(dtAllot);
                                dsFinal.Tables.Add(dtHeader);
                                dsFinal.Tables.Add(dtPaid);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtYear);
                                dsFinal.Merge(dtAllot);
                                dsFinal.Merge(dtHeader);
                                dsFinal.Merge(dtPaid);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
            string clgCode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
            {
                double totAllotAmt = 0;
                bool boolFees = false;
                int row = 0;
                string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                string batchYear = Convert.ToString(dtStudMain.Rows[dsRow]["batch_year"]);
                string curSemCode = string.Empty;
                if (!cbAcdYear.Checked)
                {
                    curSem = getCurYear(curSem);
                    if (dtYears.Rows.Count > 0)
                    {
                        curSemCode = getHeaderFK(dtYears, curSem);
                    }
                    if (curSemCode == string.Empty && getFeeCode.ContainsKey(curSem))
                        curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                }
                else
                {
                    if (getAcdYear.ContainsKey(clgCode + "$" + batchYear))
                    {
                        curSemCode = Convert.ToString(getAcdYear[clgCode + "$" + batchYear]);
                        curSemCode = curSemCode.Replace(",", "','");
                    }
                }
                if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                {
                    for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)//headername
                    {
                        string headerfk = string.Empty;
                        string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                        if (boolLedger)
                            headerfk = Convert.ToString(ds.Tables[2].Rows[hd]["hdfk"]);
                        string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "'";
                        ds.Tables[1].DefaultView.RowFilter = strHeader;
                        DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                        if (dtAllot.Rows.Count > 0)
                        {
                            if (!boolFees)
                            {
                                spreadDet.Sheets[0].RowCount++;
                                row = spreadDet.Sheets[0].RowCount - 1;
                                spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(++serialNo);
                                int colIncnt = 0;
                                for (int dsCol = 4; dsCol < dtStudMain.Columns.Count; dsCol++)
                                {
                                    colIncnt++;
                                    spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                    string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                    switch (colName.Trim())
                                    {
                                        case "Admission No":
                                        case "Roll No":
                                        case "Reg No":
                                            spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                            break;
                                    }
                                }
                                boolFees = true;
                            }
                            bool boolallot = false;
                            for (int alt = 0; alt < dtAllot.Columns.Count - tblCnt; alt++)
                            {
                                string hashValue = string.Empty;
                                string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                if (boolLedger)
                                    hashValue = headerfk + "-" + hdFK + "-" + colName;
                                else
                                    hashValue = hdFK + "-" + colName;
                                int ColCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                double Amt = 0;
                                if (colName != "ExcessAmount")
                                {
                                    double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + colName + ")", "")), out Amt);
                                }
                                else
                                {
                                    string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "'";
                                    ds.Tables[4].DefaultView.RowFilter = strHeaderexcess;
                                    DataTable dtAllotExcess = ds.Tables[4].DefaultView.ToTable();
                                    if (dtAllotExcess.Rows.Count > 0)
                                    {
                                        Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                    }
                                }
                                //double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);

                                if ((colName == "Allot" && !boolallot) || (colName == "Total" && !boolallot))
                                {
                                    totAllotAmt += Amt;
                                    boolallot = true;
                                }
                                spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                if (!htTotal.ContainsKey(ColCnt))
                                    htTotal.Add(ColCnt, Convert.ToString(Amt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                    amount += Amt;
                                    htTotal.Remove(ColCnt);
                                    htTotal.Add(ColCnt, Convert.ToString(amount));
                                }
                                //abstract
                                string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(ColCnt);
                                if (!htAbstract.ContainsKey(abstKey))
                                    htAbstract.Add(abstKey, Convert.ToString(Amt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                    amount += Amt;
                                    htAbstract.Remove(abstKey);
                                    htAbstract.Add(abstKey, Convert.ToString(amount));
                                }
                            }
                        }
                    }
                }
                if (ds.Tables[3].Rows.Count > 0 && boolFees)
                {
                    #region paymode
                    double totPaidAmt = 0;
                    for (int s = 0; s < chkl_paid.Items.Count; s++)
                    {
                        if (chkl_paid.Items[s].Selected == true)
                        {
                            string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                            string strVal = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and paymode='" + payModeVal + "'";
                            int curColCnt = 0;
                            double paiAmount = 0;
                            int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                            ds.Tables[3].DefaultView.RowFilter = strVal;
                            DataTable dvhd = ds.Tables[3].DefaultView.ToTable();
                            if (dvhd.Rows.Count > 0)
                            {
                                for (int i = 0; i < dvhd.Rows.Count; i++)
                                {
                                    double temp = 0;
                                    double.TryParse(Convert.ToString(dvhd.Rows[i]["paid"]), out temp);
                                    paiAmount += temp;
                                    totPaidAmt += temp;
                                }
                                if (!htTotal.ContainsKey(curColCnt))
                                    htTotal.Add(curColCnt, Convert.ToString(paiAmount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[curColCnt]), out amount);
                                    amount += paiAmount;
                                    htTotal.Remove(curColCnt);
                                    htTotal.Add(curColCnt, Convert.ToString(amount));
                                }
                                //abstract
                                string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(curColCnt);
                                if (!htAbstract.ContainsKey(abstKey))
                                    htAbstract.Add(abstKey, Convert.ToString(paiAmount));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                    amount += paiAmount;
                                    htAbstract.Remove(abstKey);
                                    htAbstract.Add(abstKey, Convert.ToString(amount));
                                }
                            }
                            if (paiAmount != 0)
                                spreadDet.Sheets[0].Cells[row, curColCnt].Text = Convert.ToString(paiAmount);
                            else
                                spreadDet.Sheets[0].Cells[row, curColCnt].Text = "-";
                            if (payModeVal == "1")
                                spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                            else if (payModeVal == "2")
                                spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                            else if (payModeVal == "3")
                                spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                            else if (payModeVal == "4")
                                spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                            else if (payModeVal == "5")
                                spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                            spreadDet.Sheets[0].Columns[curColCnt].Visible = false;
                            if (cbPaymode.Checked)
                            {
                                spreadDet.Sheets[0].Columns[curColCnt].Visible = true;
                            }
                        }
                    }
                    int colcnt = spreadDet.Sheets[0].ColumnCount - 2;
                    if (totPaidAmt != 0)
                    {
                        spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(totPaidAmt);
                        if (!htTotal.ContainsKey(colcnt))
                            htTotal.Add(colcnt, Convert.ToString(totPaidAmt));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                            amount += totPaidAmt;
                            htTotal.Remove(colcnt);
                            htTotal.Add(colcnt, Convert.ToString(amount));
                        }
                        //abstract
                        string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(colcnt);
                        if (!htAbstract.ContainsKey(abstKey))
                            htAbstract.Add(abstKey, Convert.ToString(totPaidAmt));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                            amount += totPaidAmt;
                            htAbstract.Remove(abstKey);
                            htAbstract.Add(abstKey, Convert.ToString(amount));
                        }
                    }
                    else
                        spreadDet.Sheets[0].Cells[row, colcnt].Text = "-";
                    colcnt = spreadDet.Sheets[0].ColumnCount - 1;
                    if (totAllotAmt != 0)
                    {
                        double balAmt = totAllotAmt - totPaidAmt;
                        spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(balAmt);
                        if (!htTotal.ContainsKey(colcnt))
                            htTotal.Add(colcnt, Convert.ToString(balAmt));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                            amount += balAmt;
                            htTotal.Remove(colcnt);
                            htTotal.Add(colcnt, Convert.ToString(amount));
                        }
                        //abstract
                        string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(colcnt);
                        if (!htAbstract.ContainsKey(abstKey))
                            htAbstract.Add(abstKey, Convert.ToString(balAmt));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                            amount += balAmt;
                            htAbstract.Remove(abstKey);
                            htAbstract.Add(abstKey, Convert.ToString(amount));
                        }
                    }
                    #endregion
                }
            }
            #region grandtot
            if (htTotal.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                double grandvalues = 0;
                mergeCount++;
                for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
                //abstract
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                double grandvalue = 0;
                foreach (KeyValuePair<string, string> curYr in getFeeCode)
                {
                    string curVal = Convert.ToString(curYr.Key);
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                    //mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                }
            }
            #endregion
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    protected void bindSpreadColumnAllotedFeecategory(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            if (rbFeesType.SelectedIndex == 0)
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            else
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            #region Column header Bind
            int mergeCount = 0;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                int col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                mergeCount++;
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            Hashtable htHDName = getHeaderFK();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            if (rbFeesType.SelectedIndex == 0)
            {
                #region header
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    string hedName = Convert.ToString(htHDName[hdFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, firstCol, 1, totcolcnt);
                    }
                }
                #endregion
            }
            else
            {
                #region Ledger
                spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
                string oldHDFK = string.Empty;
                bool boolOld = false;
                string hdFK = string.Empty;
                int oldHDCnt = 0;
                int totOldCnt = 0;
                ArrayList arHdFK = new ArrayList();
                Hashtable htName = getHDName();
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string ldFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["HdFK"]);
                    string hedName = Convert.ToString(htHDName[ldFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolOld)
                                oldHDCnt = alTcol;
                            boolOld = true;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + ldFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                            totOldCnt++;
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, firstCol, 1, totcolcnt);
                    }
                    if (!arHdFK.Contains(hdFK))
                    {
                        if (arHdFK.Count > 0)
                        {
                            string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                            totOldCnt = 0;
                            boolOld = false;
                        }
                        oldHDFK = hdFK;//old headerfk 
                        arHdFK.Add(hdFK);
                    }
                }
                if (arHdFK.Count > 0)
                {
                    string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                }
                //oldHDFK = hdFK;//old headerfk 
                //arHdFK.Add(hdFK);
                //boolOld = false;
                //totOldCnt = 0;
                #endregion
            }
            #region paymode
            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int check = 0;
            bool boolPayCol = false;
            int totcolcntPay = 0;
            int rowCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
                rowCnt = 1;
            else
                rowCnt = 2;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva = spreadDet.Sheets[0].ColumnCount++;
                    if (!boolPayCol)
                        check = checkva;
                    boolPayCol = true;
                    int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), colPay);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                    totcolcntPay++;
                }
            }
            if (totcolcntPay > 0)//header name bind
            {
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Text = "Paymode";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].ColumnCount++;
                int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Paid";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotPaid.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                spreadDet.Sheets[0].ColumnCount++;
                colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Balance";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotBal.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                if (rbFeesType.SelectedIndex == 0)
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 1, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 2, spreadDet.Sheets[0].ColumnCount - 1);
                }
                else
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 2, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 3, spreadDet.Sheets[0].ColumnCount - 1);
                }
            }
            #endregion
            #endregion
            #endregion
            #region value
            Hashtable htTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            string dvName = string.Empty;
            bool boolLedger = false;
            int tblCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
            {
                dvName = " headerfk";
                tblCnt = 4;
            }
            else
            {
                dvName = " ledgerfk";
                boolLedger = true;
                tblCnt = 5;
            }
            Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                DataSet dsNormal = ds.Copy();
                try
                {
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                    DataSet dsFinal = new DataSet();
                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        DataTable dtHeader = ds.Tables[2].DefaultView.ToTable();
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "'";
                            DataTable dtYear = ds.Tables[0].DefaultView.ToTable();
                            ds.Tables[1].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            ds.Tables[3].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtPaid = ds.Tables[3].DefaultView.ToTable();
                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtYear);
                                dsFinal.Tables.Add(dtAllot);
                                dsFinal.Tables.Add(dtHeader);
                                dsFinal.Tables.Add(dtPaid);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtYear);
                                dsFinal.Merge(dtAllot);
                                dsFinal.Merge(dtHeader);
                                dsFinal.Merge(dtPaid);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
            for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
            {
                string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                string curSemCode = string.Empty;
                int row = 0;
                ++serialNo;
                //if (getFeeCode.ContainsKey(curSem))
                //    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);  
                foreach (KeyValuePair<string, string> getSem in getFeeCode)
                {
                    double totAllotAmt = 0;
                    bool boolRowCr = false;
                    bool boolPAy = false;
                    curSemCode = Convert.ToString(getSem.Value);
                    if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                    {
                        #region header and ledger bind
                        for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)//headername
                        {
                            string headerfk = string.Empty;
                            string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                            if (boolLedger)
                                headerfk = Convert.ToString(ds.Tables[2].Rows[hd]["hdfk"]);
                            string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "'";
                            ds.Tables[1].DefaultView.RowFilter = strHeader;
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            if (dtAllot.Rows.Count > 0)
                            {
                                if (!boolRowCr)//each semester row will be created here
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    row = spreadDet.Sheets[0].RowCount - 1;
                                    int colIncnt = 0;
                                    for (int dsCol = 4; dsCol < dtStudMain.Columns.Count; dsCol++)
                                    {
                                        spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(serialNo);
                                        colIncnt++;
                                        spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                        string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                        switch (colName.Trim())
                                        {
                                            case "Admission No":
                                            case "Roll No":
                                            case "Reg No":
                                                spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                                break;
                                        }
                                    }
                                    boolRowCr = true;
                                }
                                bool boolallot = false;
                                for (int alt = 0; alt < dtAllot.Columns.Count - tblCnt; alt++)
                                {
                                    string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                    string hashValue = string.Empty;
                                    if (boolLedger)
                                        hashValue = headerfk + "-" + hdFK + "-" + colName;
                                    else
                                        hashValue = hdFK + "-" + colName;
                                    int ColCnt = 0;
                                    int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                    double Amt = 0;
                                    if (colName != "ExcessAmount")
                                    {
                                        double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                    }
                                    else
                                    {
                                        string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "'";
                                        ds.Tables[3].DefaultView.RowFilter = strHeaderexcess;
                                        DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                        if (dtAllotExcess.Rows.Count > 0)
                                        {
                                            Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                        }
                                    }

                                    if ((colName == "Allot" && !boolallot) || (colName == "Total" && !boolallot))
                                    {
                                        totAllotAmt += Amt;
                                        boolallot = true;
                                    }
                                    spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                    if (!htTotal.ContainsKey(ColCnt))
                                        htTotal.Add(ColCnt, Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                        amount += Amt;
                                        htTotal.Remove(ColCnt);
                                        htTotal.Add(ColCnt, Convert.ToString(amount));
                                    }
                                    boolPAy = true;
                                }
                            }
                        }
                        #endregion
                    }
                    if (ds.Tables[3].Rows.Count > 0 && boolPAy)
                    {
                        #region paymode
                        double totPaidAmt = 0;
                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                        {
                            if (chkl_paid.Items[s].Selected == true)
                            {
                                string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                string strVal = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and paymode='" + payModeVal + "'";
                                int curColCnt = 0;
                                double paiAmount = 0;
                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                ds.Tables[3].DefaultView.RowFilter = strVal;
                                DataTable dvhd = ds.Tables[3].DefaultView.ToTable();
                                if (dvhd.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dvhd.Rows.Count; i++)
                                    {
                                        double temp = 0;
                                        double.TryParse(Convert.ToString(dvhd.Rows[i]["paid"]), out temp);
                                        paiAmount += temp;
                                        totPaidAmt += temp;
                                    }
                                    if (!htTotal.ContainsKey(curColCnt))
                                        htTotal.Add(curColCnt, Convert.ToString(paiAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[curColCnt]), out amount);
                                        amount += paiAmount;
                                        htTotal.Remove(curColCnt);
                                        htTotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                }
                                if (paiAmount != 0)
                                    spreadDet.Sheets[0].Cells[row, curColCnt].Text = Convert.ToString(paiAmount);
                                else
                                    spreadDet.Sheets[0].Cells[row, curColCnt].Text = "-";
                                if (payModeVal == "1")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                else if (payModeVal == "2")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                else if (payModeVal == "3")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                else if (payModeVal == "4")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                else if (payModeVal == "5")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                spreadDet.Sheets[0].Columns[curColCnt].Visible = false;
                                if (cbPaymode.Checked)
                                {
                                    spreadDet.Sheets[0].Columns[curColCnt].Visible = true;
                                }
                            }
                        }
                        int colcnt = spreadDet.Sheets[0].ColumnCount - 2;
                        if (totPaidAmt != 0)
                        {
                            spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(totPaidAmt);
                            if (!htTotal.ContainsKey(colcnt))
                                htTotal.Add(colcnt, Convert.ToString(totPaidAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                amount += totPaidAmt;
                                htTotal.Remove(colcnt);
                                htTotal.Add(colcnt, Convert.ToString(amount));
                            }
                        }
                        else
                            spreadDet.Sheets[0].Cells[row, spreadDet.Sheets[0].ColumnCount - 1].Text = "-";
                        colcnt = spreadDet.Sheets[0].ColumnCount - 1;
                        if (totAllotAmt != 0)
                        {
                            double balAmt = totAllotAmt - totPaidAmt;
                            spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(balAmt);
                            if (!htTotal.ContainsKey(colcnt))
                                htTotal.Add(colcnt, Convert.ToString(balAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                amount += balAmt;
                                htTotal.Remove(colcnt);
                                htTotal.Add(colcnt, Convert.ToString(amount));
                            }
                        }
                        #endregion
                    }
                }
            }
            for (int mer = 0; mer < mergeCount; mer++)
            {
                spreadDet.Sheets[0].SetColumnMerge(mer, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #region grandtot
            if (htTotal.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                double grandvalues = 0;
                mergeCount++;
                for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                }
            }
            #endregion
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    //school detailed functions
    protected void bindSpreadColumnYearwiseSchool(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            if (rbFeesType.SelectedIndex == 0)
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            else
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            #region Column header Bind
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int mergeCount = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                int col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
                mergeCount++;
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            Hashtable htHDName = getHeaderFK();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            if (rbFeesType.SelectedIndex == 0)
            {
                #region header
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    string hedName = Convert.ToString(htHDName[hdFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, firstCol, 1, totcolcnt);
                    }
                }
                #endregion
            }
            else
            {
                #region Ledger
                spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
                string oldHDFK = string.Empty;
                bool boolOld = false;
                string hdFK = string.Empty;
                int oldHDCnt = 0;
                int totOldCnt = 0;
                ArrayList arHdFK = new ArrayList();
                Hashtable htName = getHDName();
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string ldFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["HdFK"]);
                    string hedName = Convert.ToString(htHDName[ldFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolOld)
                                oldHDCnt = alTcol;
                            boolOld = true;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + ldFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                            totOldCnt++;
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, firstCol, 1, totcolcnt);
                    }
                    if (!arHdFK.Contains(hdFK))
                    {
                        if (arHdFK.Count > 0)
                        {
                            string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                            totOldCnt = 0;
                            boolOld = false;
                        }
                        oldHDFK = hdFK;//old headerfk 
                        arHdFK.Add(hdFK);
                    }
                }
                if (arHdFK.Count > 0)
                {
                    string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                }
                //oldHDFK = hdFK;//old headerfk 
                //arHdFK.Add(hdFK);
                //boolOld = false;
                //totOldCnt = 0;
                #endregion
            }
            #region paymode
            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int check = 0;
            bool boolPayCol = false;
            int totcolcntPay = 0;
            int rowCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
                rowCnt = 1;
            else
                rowCnt = 2;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva = spreadDet.Sheets[0].ColumnCount++;
                    if (!boolPayCol)
                        check = checkva;
                    boolPayCol = true;
                    int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), colPay);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                    totcolcntPay++;
                }
            }
            if (totcolcntPay > 0)//header name bind
            {
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Text = "Paymode";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].ColumnCount++;
                int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Paid";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotPaid.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                spreadDet.Sheets[0].ColumnCount++;
                colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Balance";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotBal.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                if (rbFeesType.SelectedIndex == 0)
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 1, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 2, spreadDet.Sheets[0].ColumnCount - 1);
                }
                else
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 2, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 3, spreadDet.Sheets[0].ColumnCount - 1);
                }
            }
            #endregion
            #endregion
            #endregion
            #region value
            Hashtable htAbstract = new Hashtable();
            Hashtable htTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            string dvName = string.Empty;
            bool boolLedger = false;
            int tblCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
            {
                dvName = " headerfk";
                tblCnt = 5;
            }
            else
            {
                dvName = " ledgerfk";
                boolLedger = true;
                tblCnt = 6;
            }
            DataTable dtYears = getFeeCat();
            for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            {
                bool boolFinYr = false;
                if (!chklsfyear.Items[fnlYr].Selected)
                    continue;
                string strFinlYr = Convert.ToString(chklsfyear.Items[fnlYr].Text);
                string FinlYrValue = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + FinlYrValue + "'";
                DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
                {
                    double totAllotAmt = 0;
                    bool boolFees = false;
                    int row = 0;
                    string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                    string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                    curSem = getCurYear(curSem);
                    string curSemCode = string.Empty;
                    if (dtYears.Rows.Count > 0)
                    {
                        curSemCode = getHeaderFK(dtYears, curSem);
                    }
                    if (curSemCode == string.Empty && getFeeCode.ContainsKey(curSem))
                        curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                    //if (getFeeCode.ContainsKey(curSem))
                    //    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                    if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                    {
                        for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)//headername
                        {
                            string headerfk = string.Empty;
                            string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                            if (boolLedger)
                                headerfk = Convert.ToString(ds.Tables[2].Rows[hd]["hdfk"]);
                            string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "' and finyearfk='" + FinlYrValue + "'";
                            ds.Tables[1].DefaultView.RowFilter = strHeader;
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            if (dtAllot.Rows.Count > 0)
                            {
                                if (!boolFees)
                                {
                                    if (!boolFinYr)//finyear text bind here
                                    {
                                        spreadDet.Sheets[0].RowCount++;
                                        row = spreadDet.Sheets[0].RowCount - 1;
                                        spreadDet.Sheets[0].Cells[row, 0].Text = strFinlYr;
                                        spreadDet.Sheets[0].SpanModel.Add(row, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                        spreadDet.Sheets[0].Rows[row].BackColor = Color.Green;
                                        boolFinYr = true;
                                    }
                                    spreadDet.Sheets[0].RowCount++;
                                    row = spreadDet.Sheets[0].RowCount - 1;
                                    spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(++serialNo);
                                    int colIncnt = 0;
                                    for (int dsCol = 5; dsCol < dtStudMain.Columns.Count; dsCol++)
                                    {
                                        colIncnt++;
                                        spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                        string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                        switch (colName.Trim())
                                        {
                                            case "Admission No":
                                            case "Roll No":
                                            case "Reg No":
                                                spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                                break;
                                        }
                                    }
                                    boolFees = true;
                                }
                                bool boolallot = false;
                                for (int alt = 0; alt < dtAllot.Columns.Count - tblCnt; alt++)
                                {
                                    string hashValue = string.Empty;
                                    string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                    if (boolLedger)
                                        hashValue = headerfk + "-" + hdFK + "-" + colName;
                                    else
                                        hashValue = hdFK + "-" + colName;
                                    int ColCnt = 0;
                                    int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                    double Amt = 0;
                                    //double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                    if (colName != "ExcessAmount")
                                    {
                                        double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + colName + ")", "")), out Amt);
                                    }
                                    else
                                    {
                                        string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "' and finyearfk='" + FinlYrValue + "'";
                                        ds.Tables[3].DefaultView.RowFilter = strHeaderexcess;
                                        DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                        if (dtAllotExcess.Rows.Count > 0)
                                        {
                                            Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                        }
                                    }

                                    if ((colName == "Allot" && !boolallot) || (colName == "Total" && !boolallot))
                                    {
                                        totAllotAmt += Amt;
                                        boolallot = true;
                                    }
                                    spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                    if (!htTotal.ContainsKey(ColCnt))
                                        htTotal.Add(ColCnt, Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                        amount += Amt;
                                        htTotal.Remove(ColCnt);
                                        htTotal.Add(ColCnt, Convert.ToString(amount));
                                    }
                                    //abstract
                                    string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(ColCnt);
                                    if (!htAbstract.ContainsKey(abstKey))
                                        htAbstract.Add(abstKey, Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                        amount += Amt;
                                        htAbstract.Remove(abstKey);
                                        htAbstract.Add(abstKey, Convert.ToString(amount));
                                    }
                                }
                            }
                        }
                    }
                    if (ds.Tables[3].Rows.Count > 0 && boolFees)
                    {
                        #region paymode
                        double totPaidAmt = 0;
                        for (int s = 0; s < chkl_paid.Items.Count; s++)
                        {
                            if (chkl_paid.Items[s].Selected == true)
                            {
                                string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                string strVal = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and paymode='" + payModeVal + "' and actualfinyearfk='" + FinlYrValue + "'";
                                int curColCnt = 0;
                                double paiAmount = 0;
                                int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                ds.Tables[3].DefaultView.RowFilter = strVal;
                                DataTable dvhd = ds.Tables[3].DefaultView.ToTable();
                                if (dvhd.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dvhd.Rows.Count; i++)
                                    {
                                        double temp = 0;
                                        double.TryParse(Convert.ToString(dvhd.Rows[i]["paid"]), out temp);
                                        paiAmount += temp;
                                        totPaidAmt += temp;
                                    }
                                    if (!htTotal.ContainsKey(curColCnt))
                                        htTotal.Add(curColCnt, Convert.ToString(paiAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[curColCnt]), out amount);
                                        amount += paiAmount;
                                        htTotal.Remove(curColCnt);
                                        htTotal.Add(curColCnt, Convert.ToString(amount));
                                    }
                                    //abstract
                                    string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(curColCnt);
                                    if (!htAbstract.ContainsKey(abstKey))
                                        htAbstract.Add(abstKey, Convert.ToString(paiAmount));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                        amount += paiAmount;
                                        htAbstract.Remove(abstKey);
                                        htAbstract.Add(abstKey, Convert.ToString(amount));
                                    }
                                }
                                if (paiAmount != 0)
                                    spreadDet.Sheets[0].Cells[row, curColCnt].Text = Convert.ToString(paiAmount);
                                else
                                    spreadDet.Sheets[0].Cells[row, curColCnt].Text = "-";
                                if (payModeVal == "1")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                else if (payModeVal == "2")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                else if (payModeVal == "3")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                else if (payModeVal == "4")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                else if (payModeVal == "5")
                                    spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                spreadDet.Sheets[0].Columns[curColCnt].Visible = false;
                                if (cbPaymode.Checked)
                                {
                                    spreadDet.Sheets[0].Columns[curColCnt].Visible = true;
                                }
                            }
                        }
                        int colcnt = spreadDet.Sheets[0].ColumnCount - 2;
                        if (totPaidAmt != 0)
                        {
                            spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(totPaidAmt);
                            if (!htTotal.ContainsKey(colcnt))
                                htTotal.Add(colcnt, Convert.ToString(totPaidAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                amount += totPaidAmt;
                                htTotal.Remove(colcnt);
                                htTotal.Add(colcnt, Convert.ToString(amount));
                            }
                            //abstract
                            string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(colcnt);
                            if (!htAbstract.ContainsKey(abstKey))
                                htAbstract.Add(abstKey, Convert.ToString(totPaidAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                amount += totPaidAmt;
                                htAbstract.Remove(abstKey);
                                htAbstract.Add(abstKey, Convert.ToString(amount));
                            }
                        }
                        else
                            spreadDet.Sheets[0].Cells[row, colcnt].Text = "-";
                        colcnt = spreadDet.Sheets[0].ColumnCount - 1;
                        if (totAllotAmt != 0)
                        {
                            double balAmt = totAllotAmt - totPaidAmt;
                            spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(balAmt);
                            if (!htTotal.ContainsKey(colcnt))
                                htTotal.Add(colcnt, Convert.ToString(balAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                amount += balAmt;
                                htTotal.Remove(colcnt);
                                htTotal.Add(colcnt, Convert.ToString(amount));
                            }
                            //abstract
                            string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(colcnt);
                            if (!htAbstract.ContainsKey(abstKey))
                                htAbstract.Add(abstKey, Convert.ToString(balAmt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                amount += balAmt;
                                htAbstract.Remove(abstKey);
                                htAbstract.Add(abstKey, Convert.ToString(amount));
                            }
                        }
                        #endregion
                    }
                }
                #region grandtot
                if (htTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    //abstract
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                    double grandvalue = 0;
                    foreach (KeyValuePair<string, string> curYr in getFeeCode)
                    {
                        string curVal = Convert.ToString(curYr.Key);
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                        //mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                    }
                    htTotal.Clear();
                    htAbstract.Clear();
                }
                #endregion
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    protected void bindSpreadColumnAllotedFeecategorySchool(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 2;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            if (rbFeesType.SelectedIndex == 0)
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            else
                spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            #region Column header Bind
            int mergeCount = 0;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                int col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                mergeCount++;
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            Hashtable htHDName = getHeaderFK();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            if (rbFeesType.SelectedIndex == 0)
            {
                #region header
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    string hedName = Convert.ToString(htHDName[hdFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[1, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[0, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, firstCol, 1, totcolcnt);
                    }
                }
                #endregion
            }
            else
            {
                #region Ledger
                spreadDet.Sheets[0].ColumnHeader.RowCount = 3;
                string oldHDFK = string.Empty;
                bool boolOld = false;
                string hdFK = string.Empty;
                int oldHDCnt = 0;
                int totOldCnt = 0;
                ArrayList arHdFK = new ArrayList();
                Hashtable htName = getHDName();
                for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)
                {
                    bool boolHd = false;
                    int firstCol = 0;
                    int totcolcnt = 0;
                    string ldFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                    hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["HdFK"]);
                    string hedName = Convert.ToString(htHDName[ldFK.Trim()]);
                    if (!string.IsNullOrEmpty(AltColumn))
                    {
                        foreach (string column in splHDCol)//student main columns bind here
                        {
                            string columnTxt = string.Empty;
                            int col = 0;
                            int alTcol = spreadDet.Sheets[0].ColumnCount++;
                            if (!boolOld)
                                oldHDCnt = alTcol;
                            boolOld = true;
                            if (!boolHd)
                                firstCol = alTcol;
                            boolHd = true;
                            columnTxt = Convert.ToString(htColumn[column.Trim()]);
                            col = spreadDet.Sheets[0].ColumnCount - 1;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Text = columnTxt;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[2, col].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
                            totcolcnt++;
                            htColCnt.Add(hdFK + "-" + ldFK + "-" + columnTxt, spreadDet.Sheets[0].ColumnCount - 1);
                            totOldCnt++;
                        }
                    }
                    if (totcolcnt > 0)//header name bind
                    {
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Text = hedName;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].ForeColor = ColorTranslator.FromHtml("#000000");
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Bold = true;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Name = "Book Antiqua";
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].Font.Size = FontUnit.Medium;
                        spreadDet.Sheets[0].ColumnHeader.Cells[1, firstCol].HorizontalAlign = HorizontalAlign.Center;
                        spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(1, firstCol, 1, totcolcnt);
                    }
                    if (!arHdFK.Contains(hdFK))
                    {
                        if (arHdFK.Count > 0)
                        {
                            string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                            spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                            spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                            totOldCnt = 0;
                            boolOld = false;
                        }
                        oldHDFK = hdFK;//old headerfk 
                        arHdFK.Add(hdFK);
                    }
                }
                if (arHdFK.Count > 0)
                {
                    string headerN = Convert.ToString(htName[oldHDFK.Trim()]);
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Text = headerN;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[0, oldHDCnt].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, oldHDCnt, 1, totOldCnt);
                }
                //oldHDFK = hdFK;//old headerfk 
                //arHdFK.Add(hdFK);
                //boolOld = false;
                //totOldCnt = 0;
                #endregion
            }
            #region paymode
            int checkva = 0;
            Hashtable htPayCol = new Hashtable();
            int check = 0;
            bool boolPayCol = false;
            int totcolcntPay = 0;
            int rowCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
                rowCnt = 1;
            else
                rowCnt = 2;
            for (int s = 0; s < chkl_paid.Items.Count; s++)
            {
                if (chkl_paid.Items[s].Selected == true)
                {
                    checkva = spreadDet.Sheets[0].ColumnCount++;
                    if (!boolPayCol)
                        check = checkva;
                    boolPayCol = true;
                    int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                    htPayCol.Add(Convert.ToString(chkl_paid.Items[s].Value), colPay);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Text = Convert.ToString(chkl_paid.Items[s].Text);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Tag = Convert.ToString(chkl_paid.Items[s].Value);
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Bold = true;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Name = "Book Antiqua";
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].Font.Size = FontUnit.Medium;
                    spreadDet.Sheets[0].ColumnHeader.Cells[rowCnt, colPay].HorizontalAlign = HorizontalAlign.Center;
                    spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                    totcolcntPay++;
                }
            }
            if (totcolcntPay > 0)//header name bind
            {
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Text = "Paymode";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, check].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].ColumnCount++;
                int colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Paid";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotPaid.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                spreadDet.Sheets[0].ColumnCount++;
                colPay = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Text = "Total Balance";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, colPay].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[colPay].HorizontalAlign = HorizontalAlign.Right;
                spreadDet.Sheets[0].Columns[colPay].Visible = false;
                if (cbtotBal.Checked)
                    spreadDet.Sheets[0].Columns[colPay].Visible = true;
                if (rbFeesType.SelectedIndex == 0)
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 1, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 2, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 2, spreadDet.Sheets[0].ColumnCount - 1);
                }
                else
                {
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, check, 2, totcolcntPay);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, spreadDet.Sheets[0].ColumnCount - 2, 3, 1);
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, colPay, 3, spreadDet.Sheets[0].ColumnCount - 1);
                }
            }
            #endregion
            #endregion
            #endregion
            #region value
            Hashtable htTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            string dvName = string.Empty;
            bool boolLedger = false;
            int tblCnt = 0;
            if (rbFeesType.SelectedIndex == 0)
            {
                dvName = " headerfk";
                tblCnt = 5;
            }
            else
            {
                dvName = " ledgerfk";
                boolLedger = true;
                tblCnt = 6;
            }
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            // DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
            for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            {
                bool boolFinYr = false;
                if (!chklsfyear.Items[fnlYr].Selected)
                    continue;
                string strFinlYr = Convert.ToString(chklsfyear.Items[fnlYr].Text);
                string FinlYrValue = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + FinlYrValue + "'";
                DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
                {
                    string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                    string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                    string curSemCode = string.Empty;
                    int row = 0;
                    ++serialNo;
                    //if (getFeeCode.ContainsKey(curSem))
                    //    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);  
                    foreach (KeyValuePair<string, string> getSem in getFeeCode)
                    {
                        double totAllotAmt = 0;
                        bool boolRowCr = false;
                        bool boolPAy = false;
                        curSemCode = Convert.ToString(getSem.Value);
                        if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                        {
                            #region header and ledger bind
                            for (int hd = 0; hd < ds.Tables[2].Rows.Count; hd++)//headername
                            {
                                string headerfk = string.Empty;
                                string hdFK = Convert.ToString(ds.Tables[2].Rows[hd]["PK"]);
                                if (boolLedger)
                                    headerfk = Convert.ToString(ds.Tables[2].Rows[hd]["hdfk"]);
                                string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "' and finyearfk='" + FinlYrValue + "'";
                                ds.Tables[1].DefaultView.RowFilter = strHeader;
                                DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                                if (dtAllot.Rows.Count > 0)
                                {
                                    if (!boolRowCr)//each semester row will be created here
                                    {
                                        if (!boolFinYr)//finyear text bind here
                                        {
                                            spreadDet.Sheets[0].RowCount++;
                                            row = spreadDet.Sheets[0].RowCount - 1;
                                            spreadDet.Sheets[0].Cells[row, 0].Text = strFinlYr;
                                            spreadDet.Sheets[0].SpanModel.Add(row, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                            spreadDet.Sheets[0].Rows[row].BackColor = Color.Green;
                                            boolFinYr = true;
                                        }
                                        spreadDet.Sheets[0].RowCount++;
                                        row = spreadDet.Sheets[0].RowCount - 1;
                                        int colIncnt = 0;
                                        for (int dsCol = 5; dsCol < dtStudMain.Columns.Count; dsCol++)
                                        {
                                            spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(serialNo);
                                            colIncnt++;
                                            spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                            string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                            switch (colName.Trim())
                                            {
                                                case "Admission No":
                                                case "Roll No":
                                                case "Reg No":
                                                    spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                                    break;
                                            }
                                        }
                                        boolRowCr = true;
                                    }
                                    bool boolallot = false;
                                    for (int alt = 0; alt < dtAllot.Columns.Count - tblCnt; alt++)
                                    {
                                        string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                        string hashValue = string.Empty;
                                        if (boolLedger)
                                            hashValue = headerfk + "-" + hdFK + "-" + colName;
                                        else
                                            hashValue = hdFK + "-" + colName;
                                        int ColCnt = 0;
                                        int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                        double Amt = 0;

                                        if (colName != "ExcessAmount")
                                        {
                                            double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                        }
                                        else
                                        {
                                            string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and " + dvName + "='" + hdFK + "' and finyearfk='" + FinlYrValue + "'";
                                            ds.Tables[3].DefaultView.RowFilter = strHeaderexcess;
                                            DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                            if (dtAllotExcess.Rows.Count > 0)
                                            {
                                                Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                            }
                                        }

                                        if ((colName == "Allot" && !boolallot) || (colName == "Total" && !boolallot))
                                        {
                                            totAllotAmt += Amt;
                                            boolallot = true;
                                        }
                                        spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                        if (!htTotal.ContainsKey(ColCnt))
                                            htTotal.Add(ColCnt, Convert.ToString(Amt));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                            amount += Amt;
                                            htTotal.Remove(ColCnt);
                                            htTotal.Add(ColCnt, Convert.ToString(amount));
                                        }
                                        boolPAy = true;
                                    }
                                }
                            }
                            #endregion
                        }
                        if (ds.Tables[3].Rows.Count > 0 && boolPAy)
                        {
                            #region paymode
                            double totPaidAmt = 0;
                            for (int s = 0; s < chkl_paid.Items.Count; s++)
                            {
                                if (chkl_paid.Items[s].Selected == true)
                                {
                                    string payModeVal = Convert.ToString(chkl_paid.Items[s].Value);
                                    string strVal = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and paymode='" + payModeVal + "' and actualfinyearfk='" + FinlYrValue + "'";
                                    int curColCnt = 0;
                                    double paiAmount = 0;
                                    int.TryParse(Convert.ToString(htPayCol[payModeVal]), out curColCnt);
                                    ds.Tables[3].DefaultView.RowFilter = strVal;
                                    DataTable dvhd = ds.Tables[3].DefaultView.ToTable();
                                    if (dvhd.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < dvhd.Rows.Count; i++)
                                        {
                                            double temp = 0;
                                            double.TryParse(Convert.ToString(dvhd.Rows[i]["paid"]), out temp);
                                            paiAmount += temp;
                                            totPaidAmt += temp;
                                        }
                                        if (!htTotal.ContainsKey(curColCnt))
                                            htTotal.Add(curColCnt, Convert.ToString(paiAmount));
                                        else
                                        {
                                            double amount = 0;
                                            double.TryParse(Convert.ToString(htTotal[curColCnt]), out amount);
                                            amount += paiAmount;
                                            htTotal.Remove(curColCnt);
                                            htTotal.Add(curColCnt, Convert.ToString(amount));
                                        }
                                    }
                                    if (paiAmount != 0)
                                        spreadDet.Sheets[0].Cells[row, curColCnt].Text = Convert.ToString(paiAmount);
                                    else
                                        spreadDet.Sheets[0].Cells[row, curColCnt].Text = "-";
                                    if (payModeVal == "1")
                                        spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#F08080");
                                    else if (payModeVal == "2")
                                        spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#D3D3D3");
                                    else if (payModeVal == "3")
                                        spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FFA500");
                                    else if (payModeVal == "4")
                                        spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#90EE90");
                                    else if (payModeVal == "5")
                                        spreadDet.Sheets[0].Cells[row, curColCnt].BackColor = ColorTranslator.FromHtml("#FAFAD2");
                                    spreadDet.Sheets[0].Columns[curColCnt].Visible = false;
                                    if (cbPaymode.Checked)
                                    {
                                        spreadDet.Sheets[0].Columns[curColCnt].Visible = true;
                                    }
                                }
                            }
                            int colcnt = spreadDet.Sheets[0].ColumnCount - 2;
                            if (totPaidAmt != 0)
                            {
                                spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(totPaidAmt);
                                if (!htTotal.ContainsKey(colcnt))
                                    htTotal.Add(colcnt, Convert.ToString(totPaidAmt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                    amount += totPaidAmt;
                                    htTotal.Remove(colcnt);
                                    htTotal.Add(colcnt, Convert.ToString(amount));
                                }
                            }
                            else
                                spreadDet.Sheets[0].Cells[row, spreadDet.Sheets[0].ColumnCount - 1].Text = "-";
                            colcnt = spreadDet.Sheets[0].ColumnCount - 1;
                            if (totAllotAmt != 0)
                            {
                                double balAmt = totAllotAmt - totPaidAmt;
                                spreadDet.Sheets[0].Cells[row, colcnt].Text = Convert.ToString(balAmt);
                                if (!htTotal.ContainsKey(colcnt))
                                    htTotal.Add(colcnt, Convert.ToString(balAmt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[colcnt]), out amount);
                                    amount += balAmt;
                                    htTotal.Remove(colcnt);
                                    htTotal.Add(colcnt, Convert.ToString(amount));
                                }
                            }
                            #endregion
                        }
                    }
                }
                for (int mer = 0; mer < mergeCount; mer++)
                {
                    spreadDet.Sheets[0].SetColumnMerge(mer, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                #region grandtot
                if (htTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    htTotal.Clear();
                }
                #endregion
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    #endregion

    #region cumulative
    protected DataSet getDetailsCumulative(string selColumn, string AltColumn)
    {
        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string degree = string.Empty;
            string sec = string.Empty;
            string studMode = string.Empty;
            string seatType = string.Empty;
            string studCatg = string.Empty;
            string studType = string.Empty;
            string religioN = string.Empty;
            string communitY = string.Empty;
            string feeCat = string.Empty;
            string hdFK = string.Empty;
            string lgFK = string.Empty;
            string fnlYR = string.Empty;
            string routeID = string.Empty;
            string vehID = string.Empty;
            string stagE = string.Empty;
            string hstlName = string.Empty;
            string buildName = string.Empty;
            string roomType = string.Empty;
            string roomName = string.Empty;
            string payMode = string.Empty;
            string hdOrLeg = string.Empty;
            string grPhdOrLeg = string.Empty;
            string colmHdLg = string.Empty;
            string curSem = string.Empty;
            string gendeR = string.Empty;
            string strOrderBy = string.Empty;
            string subquota = string.Empty;
            //-------------------------------------------
            //date filter added by abarna
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();
            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();
            //-------------------------------------------
            if (ddlcollegename.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollegename.SelectedValue);
            if (cbl_batch.Items.Count > 0)
                batch = Convert.ToString(getCblSelectedValue(cbl_batch));
            if (cbl_dept.Items.Count > 0)
                degree = Convert.ToString(getCblSelectedValue(cbl_dept));
            if (cbl_sem.Items.Count > 0)
                feeCat = Convert.ToString(getCblSelectedValue(cbl_sem));
            if (cbl_sect.Items.Count > 0)
                sec = Convert.ToString(getCblSelectedValue(cbl_sect));
            if (chkl_studhed.Items.Count > 0)
                hdFK = Convert.ToString(getCblSelectedValue(chkl_studhed));
            if (chkl_studled.Items.Count > 0)
                lgFK = Convert.ToString(getCblSelectedValue(chkl_studled));
            if (chklsfyear.Items.Count > 0)
                fnlYR = Convert.ToString(getCblSelectedValue(chklsfyear));
            if (chkl_paid.Items.Count > 0)
                payMode = Convert.ToString(getCblSelectedValue(chkl_paid));
            if (cbIncStud.Checked)//student value value if available only
            {
                if (cbl_type.Items.Count > 0)
                    studMode = Convert.ToString(getCblSelectedValue(cbl_type));
                if (cbl_seat.Items.Count > 0)
                    seatType = Convert.ToString(getCblSelectedValue(cbl_seat));
                if (cbl_quota.Items.Count > 0)
                {
                    subquota = Convert.ToString(getCblSelectedValue(cbl_quota));//abarna
                }
                if (cblinclude.Items.Count > 0)

                    studCatg = getStudCategory();

                //studCatg = getStudCategory();
                //Convert.ToString(getCblSelectedValue(cblinclude));
                if (cbl_stutype.Items.Count > 0)
                    studType = Convert.ToString(getCblSelectedValue(cbl_stutype));
                if (cbl_religion.Items.Count > 0)
                    religioN = Convert.ToString(getCblSelectedValue(cbl_religion));
                if (cbl_community.Items.Count > 0)
                    communitY = Convert.ToString(getCblSelectedValue(cbl_community));
                if (cblgender.Items.Count > 0)
                    gendeR = Convert.ToString(getCblSelectedValue(cblgender));
            }
            if (cbIncTrans.Checked)//transport value if available only
            {
                if (cblroute.Items.Count > 0)
                    routeID = Convert.ToString(getCblSelectedValue(cblroute));
                if (cblvechile.Items.Count > 0)
                    vehID = Convert.ToString(getCblSelectedValue(cblvechile));
                if (cblstage.Items.Count > 0)
                    stagE = Convert.ToString(getCblSelectedValue(cblstage));
            }
            if (cbIncHstl.Checked)//hostel value if available only
            {
                if (cblhstlname.Items.Count > 0)
                    hstlName = Convert.ToString(getCblSelectedValue(cblhstlname));
                if (cblbuilding.Items.Count > 0)
                    buildName = Convert.ToString(getCblSelectedValue(cblbuilding));
                if (cblroomtype.Items.Count > 0)
                    roomType = Convert.ToString(getCblSelectedValue(cblroomtype));
                if (cblrommName.Items.Count > 0)
                    roomName = Convert.ToString(getCblSelectedText(cblrommName));
            }
            string strFinYrFk = string.Empty;
            string strActualFk = string.Empty;
            if (checkSchoolSetting() == 0)//school setting added here
            {
                strFinYrFk = ",f.finyearfk";
                strActualFk = ",f.actualfinyearfk";
            }
            if (rbFeesType.SelectedIndex == 0)
            {
                hdOrLeg = ",f.Headerfk,f.feecategory,r.app_no" + strFinYrFk + " ";
                grPhdOrLeg = " f.Headerfk,f.feecategory,r.app_no" + strFinYrFk + " ";
                colmHdLg = " distinct f.Headerfk as PK";
            }
            else
            {
                hdOrLeg = ",f.ledgerfk,f.headerfk,f.feecategory,r.app_no" + strFinYrFk + "";
                grPhdOrLeg = " f.ledgerfk,f.headerfk,f.feecategory,r.app_no" + strFinYrFk + " ";
                colmHdLg = " distinct f.ledgerfk as PK,f.headerfk as hdFK";
            }
            if (rblsemType.SelectedIndex == 0)
            {
                curSem = getCurrentSemester(batch, collegecode, getStudCategory());
            }
            if (ddlordBy.Items.Count > 0)//order by column 
            {
                if (Convert.ToString(ddlordBy.SelectedValue) == "Section")
                    strOrderBy = " order by r.current_semester, " + Convert.ToString(ddlordBy.SelectedValue) + "";
                else
                    strOrderBy = " order by " + Convert.ToString(ddlordBy.SelectedValue) + "";
                if (ddlordBy.SelectedItem.Text == "Department")
                {
                    strOrderBy = " order by r.degree_code";
                }
            }
            string strPaid = string.Empty;
            if (!string.IsNullOrEmpty(AltColumn) && AltColumn.Contains("Sum(paidamount) as [Receipt]") && !AltColumn.Contains("Sum(balamount) as [Balance]") && !AltColumn.Contains("Sum(totalamount) as [Demand]"))
                strPaid = " having sum(paidamount)>0";
            else if (!string.IsNullOrEmpty(AltColumn) && AltColumn.Contains("Sum(balamount) as [Balance]") && !AltColumn.Contains("Sum(paidamount) as [Receipt]") && !AltColumn.Contains("Sum(totalamount) as [Demand]"))
                strPaid = " having sum(balamount)>0";
            double rangeFrom = 0;
            double rangeTo = 0;
            string strRangeCondition = string.Empty;
            if (cbRange.Checked)//not use && ddlRange.Items.Count != 0
            {
                double.TryParse(Convert.ToString(txtFromRange.Text), out rangeFrom);
                double.TryParse(Convert.ToString(txtToRange.Text), out rangeTo);
                //Commented by Saranya on 11/01/2018
                //if (ddlRange.SelectedItem.Text.Trim() == "Receipt")
                //{
                //    if (!string.IsNullOrEmpty(strPaid))
                //        strRangeCondition = " and sum(paidamount) between '" + rangeFrom + "' and '" + rangeTo + "'";
                //    else
                //        strRangeCondition = " having sum(paidamount) between '" + rangeFrom + "' and '" + rangeTo + "'";
                //}
                //else if (ddlRange.SelectedItem.Text.Trim() == "Balance")
                //{
                //    if (!string.IsNullOrEmpty(strPaid))
                //        strRangeCondition = " and sum(balamount) between '" + rangeFrom + "' and '" + rangeTo + "'";
                //    else
                //        strRangeCondition = " having sum(balamount) between '" + rangeFrom + "' and '" + rangeTo + "'";
                //}
            }
            #endregion
            string selQ = string.Empty;
            if (!cbIncHstl.Checked)
            {
                #region Query
                if (!string.IsNullOrEmpty(selColumn))
                    selColumn = " distinct r.app_no,r.current_semester,r.batch_year,r.degree_code" + strFinYrFk + ", " + selColumn;
                //student details
                selQ = " select " + selColumn + " from registration r,ft_feeallot f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (!string.IsNullOrEmpty(strOrderBy))
                    selQ += strOrderBy;
                //allot
                if (!string.IsNullOrEmpty(AltColumn))
                    AltColumn = AltColumn + ",f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += " select " + AltColumn + " from registration r,ft_feeallot f,applyn a where  f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                // if (!string.IsNullOrEmpty(payMode))
                // selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                selQ += " group by f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance
                //paid

                selQ += " select sum(isnull(debit,'0'))-sum(isnull(CREDIT,'0')) as paid,f.app_no,f.feecategory,r.batch_year,f.ActualFinYearFK  from registration r,ft_findailytransaction f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.ActualFinYearFK in('" + fnlYR + "') and f.paymode in('" + payMode + "') and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0'  " + studCatg + "";//and isnull(f.totalamount,'0')<>'0'
                // if (!string.IsNullOrEmpty(payMode))
                // selQ += " and f.paymode in('" + payMode + "')";

                //selQ += " select sum(isnull(debit,'0'))as paid,f.app_no,f.feecategory,r.batch_year,f.ActualFinYearFK  from registration r,ft_findailytransaction f,applyn a where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.ActualFinYearFK in('" + fnlYR + "') and f.paymode in('" + payMode + "') and isnull(paid_Istransfer,'0')='0' and isnull(iscanceled,'0')='0' and transtype='1' " + studCatg + "";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(routeID))
                    selQ += " and bus_routeid in('" + routeID + "')";
                if (!string.IsNullOrEmpty(vehID))
                    selQ += " and vehid in('" + vehID + "')";
                if (!string.IsNullOrEmpty(stagE))
                    selQ += " and boarding in('" + stagE + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (cbdate.Checked)
                    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";//abarna 03.09.2018
                selQ += " group by f.app_no,f.feecategory,r.batch_year,f.ActualFinYearFK";
                selQ += strPaid + strRangeCondition;
                //excess
                selQ += " select sum(isnull(f.ExcessAmt,0)-isnull(f.AdjAmt,0)) as BalanceAmt,f.app_no,f.feecategory from registration r,FT_ExcessDet f,applyn a,FT_ExcessLedgerDet ft where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no   and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and ft.headerfk in('" + hdFK + "') and ft.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " ";//and isnull(f.totalamount,'0')<>'0'
                //  if (!string.IsNullOrEmpty(payMode))
                //  selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                //if (cbdate.Checked)
                //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "' ";//abarna 03.09.2018
                selQ += " and ft.FeeCategory =f.FeeCategory and ft.ExcessDetFK =f.ExcessDetPK";
                selQ += " group by f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance
                #endregion
            }
            else
            {
                #region Query
                if (!string.IsNullOrEmpty(selColumn))
                    selColumn = " distinct r.app_no,r.current_semester,r.batch_year,r.degree_code" + strFinYrFk + ", " + selColumn;
                //student details
                selQ = " select " + selColumn + " from registration r,ft_feeallot f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')   " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                if (!string.IsNullOrEmpty(studMode))
                    selQ += " and r.mode in('" + studMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (!string.IsNullOrEmpty(strOrderBy))
                    selQ += strOrderBy;
                if (!string.IsNullOrEmpty(AltColumn))
                    AltColumn = AltColumn + ",f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += " select " + AltColumn + " from registration r,ft_feeallot f,applyn a,ht_hostelregistration htr,room_detail rd where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and f.headerfk in('" + hdFK + "') and f.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " and isnull(f.istransfer,'0')='0'";//and isnull(f.totalamount,'0')<>'0'
                //  if (!string.IsNullOrEmpty(payMode))
                //  selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                if (cbdate.Checked)
                    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "'";//abarna 03.09.2018
                selQ += " group by f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance
                // selQ += "select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt,app_no from FT_ExcessDet F,registration r WHERE r.app_no=F.app_no and  App_No='" + + "' group by App_No";
                //excess
                //excess
                selQ += " select sum(isnull(f.ExcessAmt,0)-isnull(f.AdjAmt,0)) as BalanceAmt,f.app_no,f.feecategory from registration r,FT_ExcessDet f,applyn a,ht_hostelregistration htr,room_detail rd,FT_ExcessLedgerDet ft where f.app_no=r.app_no and r.app_no=a.app_no and f.app_no=a.app_no  and htr.roomfk=rd.roompk and htr.app_no=r.app_no and htr.app_no=f.app_no and htr.app_no=a.app_no and r.college_code='" + collegecode + "' and r.batch_year in('" + batch + "') and r.degree_code in('" + degree + "') and f.feecategory in('" + feeCat + "') and isnull(r.sections,'') in('" + sec + "','') and ft.headerfk in('" + hdFK + "') and ft.ledgerfk in('" + lgFK + "')  and f.finyearfk in('" + fnlYR + "')  " + studCatg + " ";//and isnull(f.totalamount,'0')<>'0'
                //  if (!string.IsNullOrEmpty(payMode))
                //  selQ += " and f.paymode in('" + payMode + "')";
                if (!string.IsNullOrEmpty(studType))
                    selQ += " and r.Stud_Type in('" + studType + "')";
                if (!string.IsNullOrEmpty(seatType))
                    selQ += " and isnull(a.seattype,'0') in('" + seatType + "')";
                if (!string.IsNullOrEmpty(subquota))
                    selQ += " and isnull(a.quota,'0') in('" + subquota + "')";//abarna
                if (!string.IsNullOrEmpty(communitY))
                    selQ += " and isnull(A.community,'0') in('" + communitY + "')";
                if (!string.IsNullOrEmpty(religioN))
                    selQ += " and isnull(A.religion,'0') in('" + religioN + "')";
                if (!string.IsNullOrEmpty(curSem))
                    selQ += " and r.current_semester in('" + curSem + "')";
                if (!string.IsNullOrEmpty(hstlName))
                    selQ += " and htr.hostelmasterfk in('" + hstlName + "')";
                if (!string.IsNullOrEmpty(buildName))
                    selQ += " and htr.buildingfk in('" + buildName + "')";
                if (!string.IsNullOrEmpty(roomName))
                    selQ += " and rd.room_name in('" + roomName + "')";
                if (!string.IsNullOrEmpty(roomType))
                    selQ += " and room_type in('" + roomType + "')";
                if (!string.IsNullOrEmpty(gendeR))
                    selQ += " and a.sex in('" + gendeR + "')";
                //if (cbdate.Checked)
                //    selQ += " and Transdate between '" + fromdate + "' and '" + todate + "' ";//abarna 03.09.2018
                selQ += " and ft.FeeCategory =f.FeeCategory and ft.ExcessDetFK =f.ExcessDetPK";
                selQ += " group by f.app_no,f.feecategory,r.batch_year" + strFinYrFk + "";
                selQ += strPaid + strRangeCondition; //having statement to filter the paid and balance

                #endregion
            }
            //allot and paid detials
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(selQ, "Text");
        }
        catch { dsload.Clear(); }
        return dsload;
    }
    //college cumulative method
    protected void bindSpreadColumnYearwiseCum(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            #region Column header Bind
            int mergeCount = 0;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int col = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
                mergeCount++;
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            foreach (string column in splHDCol)//allot,paid,balance
            {
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                string columnTxt = Convert.ToString(htColumn[column.Trim()]);
                htColCnt.Add(columnTxt, col);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
            }
            #endregion
            #endregion
            #region value
            Hashtable htAbstract = new Hashtable();
            ArrayList arHdFK = new ArrayList();
            string oldSection = string.Empty;
            Hashtable htTotal = new Hashtable();
            Hashtable gdhtTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            bool boolSec = false;
            if (!string.IsNullOrEmpty(selColumn) && selColumn.Contains("@isnull(r.sections,'') as [sections]"))
                boolSec = true;
            DataTable dtYears = getFeeCat();
            Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                DataSet dsNormal = ds.Copy();
                try
                {
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                    DataSet dsFinal = new DataSet();
                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        //DataTable dtHeader = ds.Tables[2].DefaultView.ToTable();
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "'";
                            DataTable dtYear = ds.Tables[0].DefaultView.ToTable();
                            ds.Tables[1].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            // ds.Tables[3].DefaultView.RowFilter = " batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            // DataTable dtPaid = ds.Tables[3].DefaultView.ToTable();
                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtYear);
                                dsFinal.Tables.Add(dtAllot);
                                //   dsFinal.Tables.Add(dtHeader);
                                //  dsFinal.Tables.Add(dtPaid);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtYear);
                                dsFinal.Merge(dtAllot);
                                //dsFinal.Merge(dtHeader);
                                // dsFinal.Merge(dtPaid);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            string clgCode = Convert.ToString(ddlcollegename.SelectedValue);
            DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
            for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
            {
                bool RowSec = false;
                bool boolFees = false;
                int row = 0;
                string studSec = string.Empty;
                if (boolSec)
                    studSec = Convert.ToString(dtStudMain.Rows[dsRow]["sections"]);
                string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                string batchYear = Convert.ToString(dtStudMain.Rows[dsRow]["batch_year"]);
                string curSemCode = string.Empty;
                if (!cbAcdYear.Checked)
                {
                    curSem = getCurYear(curSem);
                    if (dtYears.Rows.Count > 0)
                    {
                        curSemCode = getHeaderFK(dtYears, curSem);
                    }
                    if (curSemCode == string.Empty && getFeeCode.ContainsKey(curSem))
                        curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                    //if (curSemCode==string.Empty && getFeeCode.ContainsKey(curSem))
                    //    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                }
                else
                {
                    if (getAcdYear.ContainsKey(clgCode + "$" + batchYear))
                    {
                        curSemCode = Convert.ToString(getAcdYear[clgCode + "$" + batchYear]);
                        curSemCode = curSemCode.Replace(",", "','");
                    }
                }
                if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                {
                    string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "')";
                    ds.Tables[1].DefaultView.RowFilter = strHeader;
                    DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                    //modified by sudhagar 05.10.2017 for range option
                    #region Range

                    bool boolRange = false;
                    if (cbRange.Checked && ddlRange.Items.Count > 0 && dtAllot.Rows.Count > 0)
                    {
                        double rangeAmt = 0;
                        string strRange = string.Empty;
                        if (ddlRange.SelectedItem.Text.Trim() == "Receipt")
                            strRange = "Receipt";
                        else
                            strRange = "Balance";
                        double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + strRange + ")", "")), out rangeAmt);
                        double rangeFrom = 0;
                        double rangeTo = 0;
                        double.TryParse(Convert.ToString(txtFromRange.Text), out rangeFrom);
                        double.TryParse(Convert.ToString(txtToRange.Text), out rangeTo);
                        if (rangeFrom <= rangeAmt && rangeAmt <= rangeTo)
                            boolRange = true;
                    }
                    else
                        boolRange = true;
                    #endregion
                    if (dtAllot.Rows.Count > 0 && boolRange)
                    {
                        if (!boolFees)
                        {
                            spreadDet.Sheets[0].RowCount++;
                            row = spreadDet.Sheets[0].RowCount - 1;
                            spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(++serialNo);
                            int colIncnt = 0;
                            for (int dsCol = 4; dsCol < dtStudMain.Columns.Count; dsCol++)
                            {
                                colIncnt++;
                                spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                switch (colName.Trim())
                                {
                                    case "Admission No":
                                    case "Roll No":
                                    case "Reg No":
                                        spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                        break;
                                }
                            }
                            boolFees = true;
                            RowSec = true;
                        }
                        double total = 0;
                        double paid = 0;
                        for (int alt = 0; alt < dtAllot.Columns.Count - 3; alt++)
                        {
                            string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                            int ColCnt = 0;
                            int.TryParse(Convert.ToString(htColCnt[colName]), out ColCnt);
                            double Amt = 0;
                            //double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                            if (colName != "ExcessAmount")
                            {
                                double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + colName + ")", "")), out Amt);
                            }
                            else
                            {
                                string strHeadervalue = " app_no='" + appNo + "'and  feecategory in('" + curSemCode + "')";
                                ds.Tables[3].DefaultView.RowFilter = strHeadervalue;
                                DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                if (dtAllotExcess.Rows.Count > 0)
                                {
                                    Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                }
                            }
                            if (colName == "Demand")
                            {

                                total = Amt;
                            }
                            if (colName == "Receipt")
                            {
                                //strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and ActualFinYearFK='" + FinlYrValue + "'";
                                //ds.Tables[2].DefaultView.RowFilter = strHeader;
                                //DataTable dtPaid = ds.Tables[2].DefaultView.ToTable();
                                //double.TryParse(Convert.ToString(dtPaid.Compute("sum(paid)", "")), out Amt);
                                paid = Amt;
                            }
                            if (colName == "Balance")
                            {
                                double bal = 0;
                                bal = total - paid;
                                Amt = bal;
                            }
                            spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                            if (!htTotal.ContainsKey(ColCnt))
                                htTotal.Add(ColCnt, Convert.ToString(Amt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                amount += Amt;
                                htTotal.Remove(ColCnt);
                                htTotal.Add(ColCnt, Convert.ToString(amount));
                            }
                            //abstract
                            string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(ColCnt);
                            if (!htAbstract.ContainsKey(abstKey))
                                htAbstract.Add(abstKey, Convert.ToString(Amt));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                amount += Amt;
                                htAbstract.Remove(abstKey);
                                htAbstract.Add(abstKey, Convert.ToString(amount));
                            }
                        }
                    }
                    else
                    {
                    }
                }
                if (RowSec && !string.IsNullOrEmpty(studSec) && !arHdFK.Contains(studSec))//sections total
                {
                    #region Sectionwise total
                    if (arHdFK.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                        double grandvalues = 0;
                        int tempcnt = mergeCount;
                        for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                            if (!gdhtTotal.ContainsKey(j))
                                gdhtTotal.Add(j, Convert.ToString(grandvalues));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                                amount += grandvalues;
                                gdhtTotal.Remove(j);
                                gdhtTotal.Add(j, Convert.ToString(amount));
                            }
                        }
                        htTotal.Clear();
                    }
                    oldSection = studSec;//old headerfk 
                    arHdFK.Add(studSec);
                    #endregion
                }
            }
            #region final Sectionwise total
            if (boolSec && arHdFK.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                double grandvalues = 0;
                int tempcnt = mergeCount;
                for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    if (!gdhtTotal.ContainsKey(j))
                        gdhtTotal.Add(j, Convert.ToString(grandvalues));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                        amount += grandvalues;
                        gdhtTotal.Remove(j);
                        gdhtTotal.Add(j, Convert.ToString(amount));
                    }
                }
                htTotal.Clear();
            }
            #endregion
            if (boolSec)
            {
                #region grandtot
                if (gdhtTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(gdhtTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    //abstract
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                    double grandvalue = 0;
                    foreach (KeyValuePair<string, string> curYr in getFeeCode)
                    {
                        string curVal = Convert.ToString(curYr.Key);
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                        //mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                    }
                }
                #endregion
            }
            else
            {
                #region grandtot
                if (htTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                    //abstract
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                    double grandvalue = 0;
                    foreach (KeyValuePair<string, string> curYr in getFeeCode)
                    {
                        string curVal = Convert.ToString(curYr.Key);
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                        //mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                    }
                }
                #endregion
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    protected void bindSpreadColumnAllotedFeecategoryCum(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            #region Column header Bind
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int col = 0;
            int mergeCount = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                mergeCount++;
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            foreach (string column in splHDCol)//student main columns bind here
            {
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                string columnTxt = Convert.ToString(htColumn[column.Trim()]);
                htColCnt.Add(columnTxt, col);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
            }
            #endregion
            #endregion
            #region value
            ArrayList arHdFK = new ArrayList();
            string oldSection = string.Empty;
            Hashtable htTotal = new Hashtable();
            Hashtable gdhtTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            bool boolSec = false;
            if (!string.IsNullOrEmpty(selColumn) && selColumn.Contains("@isnull(r.sections,'') as [sections]"))
                boolSec = true;
            Dictionary<string, string> getAcdYear = new Dictionary<string, string>();
            if (cbAcdYear.Checked)
            {
                #region Academic Year
                DataSet dsNormal = ds.Copy();
                try
                {
                    string acdYear = Convert.ToString(ddlAcademic.SelectedItem.Text);
                    getAcdYear = getOldSettings(acdYear);//(acdYear, clgCode);
                    DataSet dsFinal = new DataSet();
                    if (getAcdYear.Count > 0)
                    {
                        bool boolDs = false;
                        foreach (KeyValuePair<string, string> getVal in getAcdYear)
                        {
                            string feeCate = getVal.Value.Replace(",", "','");
                            ds.Tables[0].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "'";
                            DataTable dtYear = ds.Tables[0].DefaultView.ToTable();
                            ds.Tables[1].DefaultView.RowFilter = "batch_year='" + getVal.Key.Split('$')[1] + "' and feecategory in('" + feeCate + "')";
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            if (!boolDs)
                            {
                                dsFinal.Reset();
                                dsFinal.Tables.Add(dtYear);
                                dsFinal.Tables.Add(dtAllot);
                                boolDs = true;
                            }
                            else
                            {
                                dsFinal.Merge(dtYear);
                                dsFinal.Merge(dtAllot);
                            }
                        }
                    }
                    if (dsFinal.Tables.Count > 0 && dsFinal.Tables[0].Rows.Count > 0)
                    {
                        ds.Reset();
                        ds = dsFinal.Copy();
                    }
                }
                catch
                {
                    ds.Reset();
                    ds = dsNormal.Copy();
                }
                #endregion
            }
            DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
            for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
            {
                bool RowSec = false;
                string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                string studSec = string.Empty;
                if (boolSec)
                    studSec = Convert.ToString(dtStudMain.Rows[dsRow]["sections"]);
                string curSemCode = string.Empty;
                int row = 0;
                bool boolRoll = false;
                //++serialNo;
                foreach (KeyValuePair<string, string> getSem in getFeeCode)
                {
                    bool boolRowCr = false;
                    bool boolPAy = false;
                    curSemCode = Convert.ToString(getSem.Value);
                    if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                    {
                        string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') ";
                        ds.Tables[1].DefaultView.RowFilter = strHeader;
                        DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                        //modified by sudhagar 05.10.2017 for range option
                        #region Range
                        double rangeAmt = 0;
                        bool boolRange = false;
                        if (cbRange.Checked && ddlRange.Items.Count > 0 && dtAllot.Rows.Count > 0)
                        {
                            string strRange = string.Empty;
                            if (ddlRange.SelectedItem.Text.Trim() == "Receipt")
                                strRange = "Receipt";
                            else
                                strRange = "Balance";
                            double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + strRange + ")", "")), out rangeAmt);
                            double rangeFrom = 0;
                            double rangeTo = 0;
                            double.TryParse(Convert.ToString(txtFromRange.Text), out rangeFrom);
                            double.TryParse(Convert.ToString(txtToRange.Text), out rangeTo);
                            if (rangeFrom <= rangeAmt && rangeAmt <= rangeTo)
                                boolRange = true;
                        }
                        else
                            boolRange = true;
                        #endregion
                        if (dtAllot.Rows.Count > 0 && boolRange)
                        {
                            if (!boolRoll)
                                ++serialNo;
                            boolRoll = true;
                            if (!boolRowCr)//each semester row will be created here
                            {
                                spreadDet.Sheets[0].RowCount++;
                                row = spreadDet.Sheets[0].RowCount - 1;
                                int colIncnt = 0;
                                for (int dsCol = 4; dsCol < dtStudMain.Columns.Count; dsCol++)
                                {
                                    spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(serialNo);
                                    colIncnt++;
                                    spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                    string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                    switch (colName.Trim())
                                    {
                                        case "Admission No":
                                        case "Roll No":
                                        case "Reg No":
                                            spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                            break;
                                    }
                                }
                                boolRowCr = true;
                                RowSec = true;
                            }
                            for (int alt = 0; alt < dtAllot.Columns.Count - 3; alt++)
                            {
                                string hashValue = string.Empty;
                                hashValue = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                int ColCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                double Amt = 0;
                                if (hashValue != "ExcessAmount")
                                {
                                    double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                }
                                else
                                {
                                    string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') ";
                                    ds.Tables[3].DefaultView.RowFilter = strHeaderexcess;
                                    DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                    if (dtAllotExcess.Rows.Count > 0)
                                    {
                                        Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                    }
                                }

                                spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                if (!htTotal.ContainsKey(ColCnt))
                                    htTotal.Add(ColCnt, Convert.ToString(Amt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                    amount += Amt;
                                    htTotal.Remove(ColCnt);
                                    htTotal.Add(ColCnt, Convert.ToString(amount));
                                }
                                boolPAy = true;
                            }
                        }
                    }
                }
                if (RowSec && !string.IsNullOrEmpty(studSec) && !arHdFK.Contains(studSec))//sections total
                {
                    #region Sectionwise total
                    if (arHdFK.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                        double grandvalues = 0;
                        int tempcnt = mergeCount;
                        for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                            if (!gdhtTotal.ContainsKey(j))
                                gdhtTotal.Add(j, Convert.ToString(grandvalues));
                            else
                            {
                                double amount = 0;
                                double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                                amount += grandvalues;
                                gdhtTotal.Remove(j);
                                gdhtTotal.Add(j, Convert.ToString(amount));
                            }
                        }
                        htTotal.Clear();
                    }
                    oldSection = studSec;//old headerfk 
                    arHdFK.Add(studSec);
                    #endregion
                }
            }
            for (int mer = 0; mer < mergeCount; mer++)
            {
                spreadDet.Sheets[0].SetColumnMerge(mer, FarPoint.Web.Spread.Model.MergePolicy.Always);
            }
            #region final Sectionwise total
            if (boolSec && arHdFK.Count > 0)
            {
                spreadDet.Sheets[0].Rows.Count++;
                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                double grandvalues = 0;
                int tempcnt = mergeCount;
                for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                {
                    double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    if (!gdhtTotal.ContainsKey(j))
                        gdhtTotal.Add(j, Convert.ToString(grandvalues));
                    else
                    {
                        double amount = 0;
                        double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                        amount += grandvalues;
                        gdhtTotal.Remove(j);
                        gdhtTotal.Add(j, Convert.ToString(amount));
                    }
                }
                htTotal.Clear();
            }
            #endregion
            if (boolSec)
            {
                #region grandtot
                if (gdhtTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(gdhtTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                }
                #endregion
            }
            else
            {
                #region grandtot
                if (htTotal.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                    double grandvalues = 0;
                    mergeCount++;
                    for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                    }
                }
                #endregion
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    //school cumulative method
    protected void bindSpreadColumnYearwiseCumSchool(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            #region Column header Bind
            int mergeCount = 0;
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int col = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                if (rbFeesType.SelectedIndex == 0)
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 2, 1);
                else
                    spreadDet.Sheets[0].ColumnHeaderSpanModel.Add(0, col, 3, 1);
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
                mergeCount++;
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            foreach (string column in splHDCol)//allot,paid,balance
            {
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                string columnTxt = Convert.ToString(htColumn[column.Trim()]);
                htColCnt.Add(columnTxt, col);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
            }
            #endregion
            #endregion
            #region value
            Hashtable htAbstract = new Hashtable();
            ArrayList arHdFK = new ArrayList();
            string oldSection = string.Empty;
            Hashtable htTotal = new Hashtable();
            Hashtable gdhtTotal = new Hashtable();
            DataTable dtYears = getFeeCat();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            bool boolSec = false;
            if (!string.IsNullOrEmpty(selColumn) && selColumn.Contains("@isnull(r.sections,'') as [sections]"))
                boolSec = true;
            for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            {
                bool boolFinYr = false;
                if (!chklsfyear.Items[fnlYr].Selected)
                    continue;
                string strFinlYr = Convert.ToString(chklsfyear.Items[fnlYr].Text);
                string FinlYrValue = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + FinlYrValue + "'";
                DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                //DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
                {
                    bool RowSec = false;
                    bool boolFees = false;
                    int row = 0;
                    string studSec = string.Empty;
                    if (boolSec)
                        studSec = Convert.ToString(dtStudMain.Rows[dsRow]["sections"]);
                    string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                    string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                    curSem = getCurYear(curSem);
                    string curSemCode = string.Empty;
                    if (dtYears.Rows.Count > 0)
                    {
                        curSemCode = getHeaderFK(dtYears, curSem);
                    }
                    if (curSemCode == string.Empty && getFeeCode.ContainsKey(curSem))
                        curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                    //if (curSemCode == string.Empty && getFeeCode.ContainsKey(curSem))
                    //    curSemCode = Convert.ToString(getFeeCode[curSem.Trim()]);
                    if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                    {
                        string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and finyearfk='" + FinlYrValue + "'";
                        ds.Tables[1].DefaultView.RowFilter = strHeader;
                        DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                        #region Range
                        double rangeAmt = 0;
                        bool boolRange = false;
                        if (cbRange.Checked && ddlRange.Items.Count > 0 && dtAllot.Rows.Count > 0)
                        {
                            string strRange = string.Empty;
                            if (ddlRange.SelectedItem.Text.Trim() == "Receipt")
                                strRange = "Receipt";
                            else
                                strRange = "Balance";
                            double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + strRange + ")", "")), out rangeAmt);
                            double rangeFrom = 0;
                            double rangeTo = 0;
                            double.TryParse(Convert.ToString(txtFromRange.Text), out rangeFrom);
                            double.TryParse(Convert.ToString(txtToRange.Text), out rangeTo);
                            if (rangeFrom <= rangeAmt && rangeAmt <= rangeTo)
                                boolRange = true;
                        }
                        else
                            boolRange = true;
                        #endregion
                        if (dtAllot.Rows.Count > 0 && boolRange)
                        {
                            if (!boolFees)
                            {
                                if (!boolFinYr)//finyear text bind here
                                {
                                    spreadDet.Sheets[0].RowCount++;
                                    row = spreadDet.Sheets[0].RowCount - 1;
                                    spreadDet.Sheets[0].Cells[row, 0].Text = strFinlYr;
                                    spreadDet.Sheets[0].SpanModel.Add(row, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                    spreadDet.Sheets[0].Rows[row].BackColor = Color.Green;
                                    boolFinYr = true;
                                }
                                spreadDet.Sheets[0].RowCount++;
                                row = spreadDet.Sheets[0].RowCount - 1;
                                spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(++serialNo);
                                int colIncnt = 0;
                                for (int dsCol = 5; dsCol < dtStudMain.Columns.Count; dsCol++)
                                {
                                    colIncnt++;
                                    spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                    string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                    switch (colName.Trim())
                                    {
                                        case "Admission No":
                                        case "Roll No":
                                        case "Reg No":
                                            spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                            break;
                                    }
                                }
                                boolFees = true;
                                RowSec = true;
                            }
                            //added by abarna 09.02.2018 ---------

                            double total = 0;
                            double paid = 0;
                            //---------------------
                            for (int alt = 0; alt < dtAllot.Columns.Count - 4; alt++)
                            {
                                string colName = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                int ColCnt = 0;
                                int.TryParse(Convert.ToString(htColCnt[colName]), out ColCnt);
                                double Amt = 0;
                                //double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                if (colName != "ExcessAmount")
                                {
                                    double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + colName + ")", "")), out Amt);
                                }
                                else
                                {
                                    string strHeadervalue = " app_no='" + appNo + "'and  feecategory in('" + curSemCode + "')";
                                    ds.Tables[3].DefaultView.RowFilter = strHeadervalue;
                                    DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                    if (dtAllotExcess.Rows.Count > 0)
                                    {
                                        Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                    }
                                }

                                //added by abarna 
                                if (colName == "Demand")
                                {

                                    total = Amt;
                                }
                                if (colName == "Receipt")
                                {
                                    strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and ActualFinYearFK='" + FinlYrValue + "'";
                                    ds.Tables[2].DefaultView.RowFilter = strHeader;
                                    DataTable dtPaid = ds.Tables[2].DefaultView.ToTable();
                                    double.TryParse(Convert.ToString(dtPaid.Compute("sum(paid)", "")), out Amt);
                                    paid = Amt;
                                }
                                if (colName == "Balance")
                                {
                                    double bal = 0;
                                    bal = total - paid;
                                    Amt = bal;
                                }
                                //----------------------
                                spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                if (!htTotal.ContainsKey(ColCnt))
                                    htTotal.Add(ColCnt, Convert.ToString(Amt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                    amount += Amt;
                                    htTotal.Remove(ColCnt);
                                    htTotal.Add(ColCnt, Convert.ToString(amount));
                                }
                                //abstract
                                string abstKey = Convert.ToString(curSem) + "$" + Convert.ToString(ColCnt);
                                if (!htAbstract.ContainsKey(abstKey))
                                    htAbstract.Add(abstKey, Convert.ToString(Amt));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(htAbstract[abstKey]), out amount);
                                    amount += Amt;
                                    htAbstract.Remove(abstKey);
                                    htAbstract.Add(abstKey, Convert.ToString(amount));
                                }
                            }
                        }
                    }
                    if (RowSec && !string.IsNullOrEmpty(studSec) && !arHdFK.Contains(studSec))//sections total
                    {
                        #region Sectionwise total
                        if (arHdFK.Count > 0)
                        {
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                            double grandvalues = 0;
                            int tempcnt = mergeCount;
                            for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                                if (!gdhtTotal.ContainsKey(j))
                                    gdhtTotal.Add(j, Convert.ToString(grandvalues));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                                    amount += grandvalues;
                                    gdhtTotal.Remove(j);
                                    gdhtTotal.Add(j, Convert.ToString(amount));
                                }
                            }
                            htTotal.Clear();
                        }
                        oldSection = studSec;//old headerfk 
                        arHdFK.Add(studSec);
                        #endregion
                    }
                }
                #region final Sectionwise total
                if (boolSec && arHdFK.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    double grandvalues = 0;
                    int tempcnt = mergeCount;
                    for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        if (!gdhtTotal.ContainsKey(j))
                            gdhtTotal.Add(j, Convert.ToString(grandvalues));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                            amount += grandvalues;
                            gdhtTotal.Remove(j);
                            gdhtTotal.Add(j, Convert.ToString(amount));
                        }
                    }
                    htTotal.Clear();
                }
                #endregion
                if (boolSec)
                {
                    #region grandtot
                    if (gdhtTotal.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        double grandvalues = 0;
                        mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(gdhtTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                        //abstract
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                        double grandvalue = 0;
                        foreach (KeyValuePair<string, string> curYr in getFeeCode)
                        {
                            string curVal = Convert.ToString(curYr.Key);
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                            //mergeCount++;
                            for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                            }
                        }
                        gdhtTotal.Clear();
                        htAbstract.Clear();
                    }
                    #endregion
                }
                else
                {
                    #region grandtot
                    if (htTotal.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        double grandvalues = 0;
                        mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                        //abstract
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Abstract";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Gold;
                        double grandvalue = 0;
                        foreach (KeyValuePair<string, string> curYr in getFeeCode)
                        {
                            string curVal = Convert.ToString(curYr.Key);
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, mergeCount - 1].Text = curVal + " Year";
                            //mergeCount++;
                            for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(htAbstract[curVal + "$" + j]), out grandvalues);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                            }
                        }
                        htTotal.Clear();
                        htAbstract.Clear();
                    }
                    #endregion
                }
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    protected void bindSpreadColumnAllotedFeecategoryCumSchool(string selColumn, string AltColumn, DataSet ds)
    {
        try
        {
            #region design
            RollAndRegSettings();
            spreadDet.Sheets[0].RowCount = 0;
            spreadDet.Sheets[0].ColumnCount = 0;
            spreadDet.CommandBar.Visible = false;
            spreadDet.Sheets[0].AutoPostBack = true;
            spreadDet.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet.Sheets[0].RowHeader.Visible = false;
            spreadDet.Sheets[0].ColumnCount = 1;
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
            #region Column header Bind
            int rollNo = 0;
            int regNo = 0;
            int admNo = 0;
            bool boolroll = false;
            int col = 0;
            int mergeCount = 0;
            FarPoint.Web.Spread.TextCellType txtroll = new FarPoint.Web.Spread.TextCellType();
            Hashtable htColumn = htcolumnHeaderValue();
            selColumn = selColumn.Replace("],", "]@");
            string[] splMinCol = selColumn.Split('@');
            foreach (string column in splMinCol)//student main columns bind here
            {
                string columnTxt = Convert.ToString(htColumn[column]);
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                mergeCount++;
                switch (columnTxt.Trim())
                {
                    case "Admission No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 150;
                        admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Roll No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Reg No":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].Width = 110;
                        regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        boolroll = true;
                        break;
                    case "Semester":
                        spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                        break;
                }
            }
            if (boolroll)//roll ,reg and admission no hide
                spreadColumnVisible(rollNo, regNo, admNo);
            Hashtable htColCnt = new Hashtable();
            AltColumn = AltColumn.Replace("],", "]@");
            string[] splHDCol = AltColumn.Split('@');
            foreach (string column in splHDCol)//student main columns bind here
            {
                spreadDet.Sheets[0].ColumnCount++;
                col = spreadDet.Sheets[0].ColumnCount - 1;
                string columnTxt = Convert.ToString(htColumn[column.Trim()]);
                htColCnt.Add(columnTxt, col);
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Text = columnTxt;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].ForeColor = ColorTranslator.FromHtml("#000000");
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Bold = true;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Name = "Book Antiqua";
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].Font.Size = FontUnit.Medium;
                spreadDet.Sheets[0].ColumnHeader.Cells[0, col].HorizontalAlign = HorizontalAlign.Center;
                spreadDet.Sheets[0].Columns[col].HorizontalAlign = HorizontalAlign.Right;
            }
            #endregion
            #endregion
            #region value
            ArrayList arHdFK = new ArrayList();
            string oldSection = string.Empty;
            Hashtable htTotal = new Hashtable();
            Hashtable gdhtTotal = new Hashtable();
            Dictionary<string, string> getFeeCode = getFeecode();//get current sem code
            int serialNo = 0;
            bool boolSec = false;
            if (!string.IsNullOrEmpty(selColumn) && selColumn.Contains("@isnull(r.sections,'') as [sections]"))
                boolSec = true;
            for (int fnlYr = 0; fnlYr < chklsfyear.Items.Count; fnlYr++)
            {
                bool boolFinYr = false;
                if (!chklsfyear.Items[fnlYr].Selected)
                    continue;
                string strFinlYr = Convert.ToString(chklsfyear.Items[fnlYr].Text);
                string FinlYrValue = Convert.ToString(chklsfyear.Items[fnlYr].Value);
                ds.Tables[0].DefaultView.RowFilter = "finyearfk='" + FinlYrValue + "'";
                DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                // DataTable dtStudMain = ds.Tables[0].DefaultView.ToTable();
                for (int dsRow = 0; dsRow < dtStudMain.Rows.Count; dsRow++)
                {
                    bool RowSec = false;
                    string appNo = Convert.ToString(dtStudMain.Rows[dsRow]["app_no"]);
                    string curSem = Convert.ToString(dtStudMain.Rows[dsRow]["current_semester"]);
                    string studSec = string.Empty;
                    if (boolSec)
                        studSec = Convert.ToString(dtStudMain.Rows[dsRow]["sections"]);
                    string curSemCode = string.Empty;
                    int row = 0;
                    bool boolRoll = false;
                    //++serialNo;
                    foreach (KeyValuePair<string, string> getSem in getFeeCode)
                    {
                        bool boolRowCr = false;
                        bool boolPAy = false;
                        curSemCode = Convert.ToString(getSem.Value);
                        if (ds.Tables[1].Rows.Count > 0 && !string.IsNullOrEmpty(curSemCode))
                        {
                            string strHeader = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') and finyearfk='" + FinlYrValue + "'";
                            ds.Tables[1].DefaultView.RowFilter = strHeader;
                            DataTable dtAllot = ds.Tables[1].DefaultView.ToTable();
                            #region Range added by abarna 04.06.2018
                            double rangeAmt = 0;
                            bool boolRange = false;
                            if (cbRange.Checked && ddlRange.Items.Count > 0 && dtAllot.Rows.Count > 0)
                            {
                                string strRange = string.Empty;
                                if (ddlRange.SelectedItem.Text.Trim() == "Receipt")
                                    strRange = "Receipt";
                                else
                                    strRange = "Balance";
                                double.TryParse(Convert.ToString(dtAllot.Compute("sum(" + strRange + ")", "")), out rangeAmt);
                                double rangeFrom = 0;
                                double rangeTo = 0;
                                double.TryParse(Convert.ToString(txtFromRange.Text), out rangeFrom);
                                double.TryParse(Convert.ToString(txtToRange.Text), out rangeTo);
                                if (rangeFrom <= rangeAmt && rangeAmt <= rangeTo)
                                    boolRange = true;
                            }
                            else
                                boolRange = true;
                            #endregion
                            if (dtAllot.Rows.Count > 0 && boolRange)
                            {
                                if (!boolRoll)
                                    ++serialNo;
                                boolRoll = true;
                                if (!boolRowCr)//each semester row will be created here
                                {
                                    if (!boolFinYr)//finyear text bind here
                                    {
                                        spreadDet.Sheets[0].RowCount++;
                                        row = spreadDet.Sheets[0].RowCount - 1;
                                        spreadDet.Sheets[0].Cells[row, 0].Text = strFinlYr;
                                        spreadDet.Sheets[0].SpanModel.Add(row, 0, 1, spreadDet.Sheets[0].ColumnCount - 1);
                                        spreadDet.Sheets[0].Rows[row].BackColor = Color.Green;
                                        boolFinYr = true;
                                    }
                                    spreadDet.Sheets[0].RowCount++;
                                    row = spreadDet.Sheets[0].RowCount - 1;
                                    int colIncnt = 0;
                                    for (int dsCol = 5; dsCol < dtStudMain.Columns.Count; dsCol++)
                                    {
                                        spreadDet.Sheets[0].Cells[row, 0].Text = Convert.ToString(serialNo);
                                        colIncnt++;
                                        spreadDet.Sheets[0].Cells[row, colIncnt].Text = Convert.ToString(dtStudMain.Rows[dsRow][dsCol]);
                                        string colName = Convert.ToString(dtStudMain.Columns[dsCol].ColumnName);
                                        switch (colName.Trim())
                                        {
                                            case "Admission No":
                                            case "Roll No":
                                            case "Reg No":
                                                spreadDet.Sheets[0].Cells[row, colIncnt].CellType = txtroll;
                                                break;
                                        }
                                    }
                                    boolRowCr = true;
                                    RowSec = true;
                                }
                                for (int alt = 0; alt < dtAllot.Columns.Count - 4; alt++)
                                {
                                    string hashValue = string.Empty;
                                    hashValue = Convert.ToString(dtAllot.Columns[alt].ColumnName);
                                    int ColCnt = 0;
                                    int.TryParse(Convert.ToString(htColCnt[hashValue]), out ColCnt);
                                    double Amt = 0;
                                    if (hashValue != "ExcessAmount")
                                    {
                                        double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                    }
                                    else
                                    {
                                        string strHeaderexcess = " app_no='" + appNo + "' and  feecategory in('" + curSemCode + "') ";
                                        ds.Tables[3].DefaultView.RowFilter = strHeaderexcess;
                                        DataTable dtAllotExcess = ds.Tables[3].DefaultView.ToTable();
                                        if (dtAllotExcess.Rows.Count > 0)
                                        {
                                            Amt = Convert.ToDouble(dtAllotExcess.Rows[0]["balanceamt"]);
                                        }
                                    }
                                    //double.TryParse(Convert.ToString(dtAllot.Rows[0][alt]), out Amt);
                                    spreadDet.Sheets[0].Cells[row, ColCnt].Text = Convert.ToString(Amt);
                                    if (!htTotal.ContainsKey(ColCnt))
                                        htTotal.Add(ColCnt, Convert.ToString(Amt));
                                    else
                                    {
                                        double amount = 0;
                                        double.TryParse(Convert.ToString(htTotal[ColCnt]), out amount);
                                        amount += Amt;
                                        htTotal.Remove(ColCnt);
                                        htTotal.Add(ColCnt, Convert.ToString(amount));
                                    }
                                    boolPAy = true;
                                }
                            }
                        }
                    }
                    if (RowSec && !string.IsNullOrEmpty(studSec) && !arHdFK.Contains(studSec))//sections total
                    {
                        #region Sectionwise total
                        if (arHdFK.Count > 0)
                        {
                            spreadDet.Sheets[0].Rows.Count++;
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                            spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                            spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                            double grandvalues = 0;
                            int tempcnt = mergeCount;
                            for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                            {
                                double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                                spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                                if (!gdhtTotal.ContainsKey(j))
                                    gdhtTotal.Add(j, Convert.ToString(grandvalues));
                                else
                                {
                                    double amount = 0;
                                    double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                                    amount += grandvalues;
                                    gdhtTotal.Remove(j);
                                    gdhtTotal.Add(j, Convert.ToString(amount));
                                }
                            }
                            htTotal.Clear();
                        }
                        oldSection = studSec;//old headerfk 
                        arHdFK.Add(studSec);
                        #endregion
                    }
                }
                for (int mer = 0; mer < mergeCount; mer++)
                {
                    spreadDet.Sheets[0].SetColumnMerge(mer, FarPoint.Web.Spread.Model.MergePolicy.Always);
                }
                #region final Sectionwise total
                if (boolSec && arHdFK.Count > 0)
                {
                    spreadDet.Sheets[0].Rows.Count++;
                    spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Section (" + oldSection + ") Total";
                    spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                    spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.YellowGreen;
                    double grandvalues = 0;
                    int tempcnt = mergeCount;
                    for (int j = ++tempcnt; j < spreadDet.Sheets[0].ColumnCount; j++)
                    {
                        double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        if (!gdhtTotal.ContainsKey(j))
                            gdhtTotal.Add(j, Convert.ToString(grandvalues));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(gdhtTotal[j]), out amount);
                            amount += grandvalues;
                            gdhtTotal.Remove(j);
                            gdhtTotal.Add(j, Convert.ToString(amount));
                        }
                    }
                    htTotal.Clear();
                }
                #endregion
                if (boolSec)
                {
                    #region grandtot
                    if (gdhtTotal.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        double grandvalues = 0;
                        mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(gdhtTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                        gdhtTotal.Clear();
                    }
                    #endregion
                }
                else
                {
                    #region grandtot
                    if (htTotal.Count > 0)
                    {
                        spreadDet.Sheets[0].Rows.Count++;
                        spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, 0].Text = "Grand Total";
                        spreadDet.Sheets[0].SpanModel.Add(spreadDet.Sheets[0].Rows.Count - 1, 0, 1, 2);
                        spreadDet.Sheets[0].Rows[spreadDet.Sheets[0].Rows.Count - 1].BackColor = Color.Green;
                        double grandvalues = 0;
                        mergeCount++;
                        for (int j = mergeCount; j < spreadDet.Sheets[0].ColumnCount; j++)
                        {
                            double.TryParse(Convert.ToString(htTotal[j]), out grandvalues);
                            spreadDet.Sheets[0].Cells[spreadDet.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(grandvalues);
                        }
                        htTotal.Clear();
                    }
                    #endregion
                }
            }
            #endregion
            spreadDet.Sheets[0].PageSize = spreadDet.Sheets[0].RowCount;
            // payModeLabels(htPayCol);
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            spreadDet.Visible = true;
            print.Visible = true;
            getPrintSettings();
            //  spreadDet.Height = 200 + height;
            spreadDet.SaveChanges();
        }
        catch { }
    }
    #endregion

    protected string getCurYear(string curSem)
    {
        string curYear = string.Empty;
        try
        {
            switch (curSem)
            {
                case "1":
                case "2":
                    curYear = "1";
                    break;
                case "3":
                case "4":
                    curYear = "2";
                    break;
                case "5":
                case "6":
                    curYear = "3";
                    break;
                case "7":
                case "8":
                    curYear = "4";
                    break;
                case "9":
                case "10":
                    curYear = "5";
                    break;
            }
        }
        catch { }
        return curYear;
    }

    protected string getSelectedColumn(ref string AltColumn)
    {
        string val = string.Empty;
        try
        {
            StringBuilder strCol = new StringBuilder();
            StringBuilder altstrCol = new StringBuilder();
            Hashtable htcolumn = htcolumnValue();
            string Usercollegecode = string.Empty;
            //if (Session["collegecode"] != null)
            //    Usercollegecode = Convert.ToString(Session["collegecode"]);
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedItem.Value);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        bool boolcheck = false;
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempSel = Convert.ToString(htcolumn[splVal[row].Trim()]);
                                    if (!boolcheck)
                                        strCol.Append(tempSel + ",");
                                    else
                                        altstrCol.Append(tempSel + ",");
                                }
                            }
                            if (strCol.Length > 0)//&& grpstrCol.Length > 0
                            {
                                if (!boolcheck)
                                {
                                    strCol.Remove(strCol.Length - 1, 1);
                                    val = Convert.ToString(strCol);
                                    boolcheck = true;
                                }
                                else
                                {
                                    altstrCol.Remove(altstrCol.Length - 1, 1);
                                    AltColumn = Convert.ToString(altstrCol);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return val;
    }

    protected Hashtable getHeaderFK()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            if (rbFeesType.SelectedIndex == 0)
                selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + collegecode + "') ";
            else
                selQFK = "   select distinct ledgername as name,ledgerpk as pk from fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and h.collegecode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
    }

    protected Hashtable getHDName()
    {
        Hashtable hthdName = new Hashtable();
        try
        {
            string selQFK = string.Empty;
            selQFK = "  select distinct headerpk as pk,headername as name from fm_headermaster where collegecode in('" + collegecode + "') ";
            DataSet dsval = d2.select_method_wo_parameter(selQFK, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < dsval.Tables[0].Rows.Count; row++)
                {
                    if (!hthdName.ContainsKey(Convert.ToString(dsval.Tables[0].Rows[row]["pk"])))
                        hthdName.Add(Convert.ToString(dsval.Tables[0].Rows[row]["pk"]), Convert.ToString(dsval.Tables[0].Rows[row]["name"]));
                }
            }
        }
        catch { hthdName.Clear(); }
        return hthdName;
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
    protected void spreadColumnVisible(int rollNo, int regNo, int admNo)
    {
        try
        {
            #region
            if (roll == 0)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 1)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 2)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 3)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 4)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 5)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = false;
            }
            else if (roll == 6)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = false;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = true;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            else if (roll == 7)
            {
                if (rollNo > 0)
                    spreadDet.Columns[rollNo].Visible = true;
                if (regNo > 0)
                    spreadDet.Columns[regNo].Visible = false;
                if (admNo > 0)
                    spreadDet.Columns[admNo].Visible = true;
            }
            #endregion
        }
        catch { }
    }
    #endregion

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
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

    private string getCblSelectedTextwithout(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
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

    #region Print

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet, reportname);
                // lblvalidation1.Visible = false;
            }
            else
            {
                // lblvalidation1.Text = "Please Enter Your  Report Name";
                //  lblvalidation1.Visible = true;
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
            string deptname = "";
            string dept = "";
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected)
                {
                    dept = cbl_dept.Items[i].Text;
                    if (deptname == "")
                        deptname = dept;
                    else
                        deptname = deptname + "," + dept;
                }
            }

            degreedetails = "Finance Universal Report" + '\n' + '@' + "Department : " + deptname;

            //+ '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            pagename = "FinanceUniversalReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 0, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            lblvalidation1.Text = "";
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Finance Universal Report";
            pagename = "FinanceUniversalReport.aspx";
            Printcontrolhed.loadspreaddetails(spreadDet, pagename, degreedetails, 1, Convert.ToString(Session["usercode"]));
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

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
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        if (checkSchoolSetting() == 0)
        {
            lblbatch.Text = "Year";
            lblheader.Text = "Fees";
        }
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    //order by 

    protected void ddlMainreport_Selected(object sender, EventArgs e)
    {
        getOrderBySelectedColumn();
        if (rblrptType.SelectedIndex == 1)
            getRangeSelectedColumn();
    }

    protected void getOrderBySelectedColumn()//load column if only available in column order report
    {
        string val = string.Empty;
        try
        {
            ddlordBy.Items.Clear();
            string Usercollegecode = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedValue);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    string tempName = splVal[row];
                                    if (splVal[row].Trim() == "Roll No")
                                        ddlordBy.Items.Add(new ListItem("Roll No", "r.roll_no"));
                                    else if (splVal[row].Trim() == "Reg No")
                                        ddlordBy.Items.Add(new ListItem("Reg No", "r.reg_no"));
                                    else if (splVal[row].Trim() == "Admission No")
                                        ddlordBy.Items.Add(new ListItem("Admission No", "r.roll_admit"));
                                    else if (splVal[row].Trim() == "Student Name")
                                        ddlordBy.Items.Add(new ListItem("Student Name", "r.stud_name"));
                                    else if (splVal[row].Trim() == "Section")
                                        ddlordBy.Items.Add(new ListItem("Section", "isnull(r.sections,'')"));
                                    else if (splVal[row].Trim() == "Department")
                                        ddlordBy.Items.Add(new ListItem("Department", "[Department]"));
                                    //Department
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        // return val;
    }

    protected void getRangeSelectedColumn()//load column if only available in column order report
    {
        string val = string.Empty;
        try
        {
            ddlRange.Items.Clear();
            string Usercollegecode = string.Empty;
            if (ddlcollegename.Items.Count > 0)
                Usercollegecode = Convert.ToString(ddlcollegename.SelectedValue);
            string linkName = string.Empty;
            if (ddlMainreport.Items.Count > 0 && ddlMainreport.SelectedItem.Text != "Select")
                linkName = Convert.ToString(ddlMainreport.SelectedItem.Text);
            if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string selQ = d2.GetFunction("  select LinkValue from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "' and user_code='" + usercode + "'");
                if (!string.IsNullOrEmpty(selQ) && selQ != "0")
                {
                    string[] splCol = selQ.Split('$');
                    if (splCol.Length > 0)
                    {
                        foreach (string spfirst in splCol)
                        {
                            string[] splVal = spfirst.Split(',');
                            if (splVal.Length > 0)
                            {
                                for (int row = 0; row < splVal.Length; row++)
                                {
                                    if (splVal[row].Trim() == "Receipt")
                                        ddlRange.Items.Add(new ListItem("Receipt", "Receipt"));
                                    else if (splVal[row].Trim() == "Balance")
                                        ddlRange.Items.Add(new ListItem("Balance", "Balance"));
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        // return val;
    }


    //protected void cutofcalculation_Click(object sender, EventArgs e)
    //{
    //    cbl_degree_OnSelectedIndexChanged(sender, e);
    //}

    #region old
    //protected void getitem()
    //{
    //    MultiCheckCombo1.ClearAll();
    //    string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
    //    ds = d2.select_method_wo_parameter(selectQuery, "Text");
    //    SqlCommand cmd = new SqlCommand(selectQuery);
    //    cmd.Connection = cs.CreateConnection();
    //    SqlDataReader dr = cmd.ExecuteReader();
    //    dr.Read();
    //    //MultiCheckCombo1.AddItems(dr, "" + dr["collname"] + "", "" + dr["college_code"] + "");
    //    //MultiCheckCombo1.AddItems(dr, "collname", "college_code");
    //    MultiCheckCombo1.AddItemss(ds, "collname", "college_code");
    //    //string val = MultiCheckCombo1.Value;
    //}
    //protected void getitemAnother()
    //{
    //    // string value = MultiCheckCombo1.val;
    //    string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + usercode + " and cp.college_code=cf.college_code";
    //    ds = d2.select_method_wo_parameter(selectQuery, "Text");
    //    SqlCommand cmd = new SqlCommand(selectQuery);
    //    cmd.Connection = cs.CreateConnection();
    //    SqlDataReader dr = cmd.ExecuteReader();
    //    dr.Read();
    //    //MultiCheckCombo1.AddItems(dr, "" + dr["collname"] + "", "" + dr["college_code"] + "");
    //    //MultiCheckCombo1.AddItems(dr, "collname", "college_code");
    //    MultiCombo2.AddItemss(ds, "collname", "college_code");
    //}
    #endregion

    //added by sudhagar 28.08.2017

    public void getAcademicYear()
    {
        try
        {
            string fnalyr = "";
            // string getfinanceyear = "select distinct convert(nvarchar(15),FinYearStart,103) sdate,convert(nvarchar(15),FinYearEnd,103) edate,FinYearPK from FM_FinYearMaster where CollegeCode in('" + collegecode + "')  order by FinYearPK desc";
            string getfinanceyear = "SELECT distinct ACD_YEAR FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD WHERE  AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK  AND  ACD_COLLEGE_CODE IN('" + collegecode + "') order by ACD_YEAR desc";
            ds.Dispose();
            ds.Reset();
            ddlAcademic.Items.Clear();
            ds = d2.select_method_wo_parameter(getfinanceyear, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string fdatye = ds.Tables[0].Rows[i]["ACD_YEAR"].ToString();
                    ddlAcademic.Items.Insert(0, new System.Web.UI.WebControls.ListItem(fdatye, fdatye));
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected Dictionary<string, string> getOldSettings(string acdYears)
    {
        Dictionary<string, string> htAcademic = new Dictionary<string, string>();
        try
        {
            string settingType = string.Empty;
            if (rblTypeNew.SelectedIndex == 0)
                settingType = "0";
            else if (rblTypeNew.SelectedIndex == 1)
                settingType = "1";
            else if (rblTypeNew.SelectedIndex == 2)
                settingType = "2";
            string collegecode = Convert.ToString(ddlcollegename.SelectedValue);
            string selQ = " SELECT distinct ACD_COLLEGE_CODE,c.collname,ACD_YEAR,ACD_BATCH_YEAR,ACD_FEECATEGORY,t.textval FROM FT_ACADEMICYEAR AY,FT_ACADEMICYEAR_DETAILED AYD,collinfo c,textvaltable t WHERE c.college_code=ay.acd_college_code and AY.ACA_YEAR_PK=AYD.ACA_YEAR_FK and textcriteria='FEECA' and t.textcode=ayd.ACD_FEECATEGORY and t.college_code=ay.ACD_COLLEGE_CODE AND  ACD_COLLEGE_CODE IN('" + collegecode + "') and ACD_YEAR in('" + acdYears + "') and ACD_SETTING_TYPE='" + settingType + "' order by ACD_COLLEGE_CODE  asc,ACD_YEAR desc,ACD_FEECATEGORY asc";
            DataSet dsPrevAMount = d2.select_method_wo_parameter(selQ, "Text");
            if (dsPrevAMount.Tables.Count > 0 && dsPrevAMount.Tables[0].Rows.Count > 0)
            {
                DataTable dtAcdYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_COLLEGE_CODE", "collname");
                DataTable dtBatchYear = dsPrevAMount.Tables[0].DefaultView.ToTable(true, "ACD_YEAR", "ACD_BATCH_YEAR", "ACD_COLLEGE_CODE");
                DataTable dtFeecat = dsPrevAMount.Tables[0].DefaultView.ToTable();
                if (dtAcdYear.Rows.Count > 0)
                {
                    int Sno = 0;
                    for (int row = 0; row < dtAcdYear.Rows.Count; row++)
                    {
                        Sno++;
                        string acdYear = Convert.ToString(dtAcdYear.Rows[row]["ACD_YEAR"]);
                        string clgCode = Convert.ToString(dtAcdYear.Rows[row]["ACD_COLLEGE_CODE"]);
                        dtBatchYear.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                        DataTable dtBatch = dtBatchYear.DefaultView.ToTable();
                        if (dtBatch.Rows.Count > 0)
                        {
                            for (int bat = 0; bat < dtBatch.Rows.Count; bat++)
                            {
                                string acdBatchYear = Convert.ToString(dtBatch.Rows[bat]["ACD_BATCH_YEAR"]);
                                dtFeecat.DefaultView.RowFilter = "ACD_YEAR='" + acdYear + "' and ACD_BATCH_YEAR='" + acdBatchYear + "' and ACD_COLLEGE_CODE='" + clgCode + "'";
                                DataTable dtFee = dtFeecat.DefaultView.ToTable();
                                if (dtFee.Rows.Count > 0)
                                {
                                    StringBuilder sbSem = new StringBuilder();
                                    StringBuilder sbSemStr = new StringBuilder();
                                    for (int fee = 0; fee < dtFee.Rows.Count; fee++)
                                    {
                                        string feecaT = Convert.ToString(dtFee.Rows[fee]["ACD_FEECATEGORY"]);
                                        string feecaTStr = Convert.ToString(dtFee.Rows[fee]["textval"]);
                                        sbSem.Append(feecaT + ",");
                                        // sbSemStr.Append(feecaTStr + ",");
                                    }
                                    if (sbSem.Length > 0)
                                        sbSem.Remove(sbSem.Length - 1, 1);
                                    if (!htAcademic.ContainsKey(clgCode + "$" + acdBatchYear))
                                        htAcademic.Add(clgCode + "$" + acdBatchYear, Convert.ToString(sbSem));
                                    //if (sbSemStr.Length > 0)
                                    //    sbSemStr.Remove(sbSemStr.Length - 1, 1);                              
                                }
                            }
                        }
                    }
                }
            }
        }
        catch { }
        return htAcademic;
    }

    protected DataTable getFeeCat()
    {
        DataTable dtVal = new DataTable();
        try
        {
            string clgcode = ddlcollegename.SelectedItem.Value.ToString();
            string sql = "select textval,semestertermcode,yearcode from FeeCatagoryYearMatching f,textvaltable t,collinfo c where textcriteria='FEECA' and c.college_code='" + clgcode + "' and t.textcode=f.SemesterTermcode and c.college_code=f.collegeCode";
            DataSet finals = d2.select_method_wo_parameter(sql, "Text");
            dtVal = finals.Tables[0].DefaultView.ToTable();
        }
        catch { dtVal.Clear(); }
        return dtVal;
    }

    protected string getHeaderFK(DataTable dsval, string year)
    {
        string getSemVal = string.Empty;
        try
        {
            StringBuilder sbStr = new StringBuilder();
            dsval.DefaultView.RowFilter = "yearcode='" + year + "'";
            DataTable dtSem = dsval.DefaultView.ToTable();
            for (int row = 0; row < dtSem.Rows.Count; row++)
            {
                sbStr.Append(Convert.ToString(dtSem.Rows[row]["semestertermcode"]) + "','");
            }
            if (sbStr.Length > 0)
            {
                sbStr.Remove(sbStr.Length - 3, 3);
                getSemVal = Convert.ToString(sbStr);
            }
        }
        catch { getSemVal = string.Empty; }
        return getSemVal;
    }

    protected Dictionary<string, string> getSettingFeeCat()
    {
        Dictionary<string, string> htsem = new Dictionary<string, string>();
        try
        {
            string feecatg = "";
            string yeaR = "";
            string SemCode = "";
            string clgcode = ddlcollegename.SelectedItem.Value.ToString();
            string selQ = "select textval,semestertermcode,yearcode from FeeCatagoryYearMatching f,textvaltable t,collinfo c where textcriteria='FEECA' and c.college_code='" + clgcode + "' and t.textcode=f.SemesterTermcode and c.college_code=f.collegeCode"; ;
            DataSet dsval = d2.select_method_wo_parameter(selQ, "Text");
            if (dsval.Tables.Count > 0 && dsval.Tables[0].Rows.Count > 0)
            {
                for (int sem = 0; sem < dsval.Tables[0].Rows.Count; sem++)
                {
                    yeaR = Convert.ToString(dsval.Tables[0].Rows[sem]["yearcode"]);
                    SemCode = Convert.ToString(dsval.Tables[0].Rows[sem]["semestertermcode"]);
                    if (yeaR == "1")
                    {
                        if (!htsem.ContainsKey("1 Year"))
                            htsem.Add(Convert.ToString("1 Year"), SemCode);
                        else
                        {
                            feecatg = Convert.ToString(htsem["1 Year"]);
                            feecatg = feecatg + "'" + "," + "'" + SemCode;
                            htsem.Remove("1 Year");
                            htsem.Add(Convert.ToString("1 Year"), feecatg);
                        }
                    }
                    else if (yeaR == "2")
                    {
                        if (!htsem.ContainsKey("2 Year"))
                            htsem.Add(Convert.ToString("2 Year"), SemCode);
                        else
                        {
                            feecatg = Convert.ToString(htsem["2 Year"]);
                            feecatg = feecatg + "'" + "," + "'" + SemCode;
                            htsem.Remove("2 Year");
                            htsem.Add(Convert.ToString("2 Year"), feecatg);
                        }
                    }
                    else if (yeaR == "3")
                    {
                        if (!htsem.ContainsKey("3 Year"))
                            htsem.Add(Convert.ToString("3 Year"), SemCode);
                        else
                        {
                            feecatg = Convert.ToString(htsem["3 Year"]);
                            feecatg = feecatg + "'" + "," + "'" + SemCode;
                            htsem.Remove("3 Year");
                            htsem.Add(Convert.ToString("3 Year"), feecatg);
                        }
                    }
                    else if (yeaR == "4")
                    {
                        if (!htsem.ContainsKey("4 Year"))
                            htsem.Add(Convert.ToString("4 Year"), SemCode);
                        else
                        {
                            feecatg = Convert.ToString(htsem["4 Year"]);
                            feecatg = feecatg + "'" + "," + "'" + SemCode;
                            htsem.Remove("4 Year");
                            htsem.Add(Convert.ToString("4 Year"), feecatg);
                        }
                    }
                    else if (yeaR == "5")
                    {
                        if (!htsem.ContainsKey("5 Year"))
                            htsem.Add(Convert.ToString("5 Year"), SemCode);
                        else
                        {
                            feecatg = Convert.ToString(htsem["5 Year"]);
                            feecatg = feecatg + "'" + "," + "'" + SemCode;
                            htsem.Remove("5 Year");
                            htsem.Add(Convert.ToString("5 Year"), feecatg);
                        }
                    }
                }
            }
            //ViewState["feecat"] = htsem;
        }
        catch { }
        return htsem;
    }
}

