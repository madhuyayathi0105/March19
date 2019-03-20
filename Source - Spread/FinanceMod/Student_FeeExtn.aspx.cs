using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Configuration;

public partial class Student_FeeExtn : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();

    string usercode = string.Empty;
    static string collegecode = string.Empty;
    static string collegecode1 = string.Empty;
    string sessstream = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string[] duespl = new string[2];
    // static string chosedmode = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    static string clgCode = "";

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
            collegecode = Convert.ToString(Session["collegecode"]);
            sessstream = Convert.ToString(Session["streamcode"]);
            lbl_str.Text = sessstream;
            if (!IsPostBack)
            {
                setLabelText();
                bindclg();
                if (ddl_college.Items.Count > 0)
                {
                    collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                    clgCode = Convert.ToString(ddl_college.SelectedItem.Value);
                }
                loadsem();
                txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_datewithamnt.Visible = false;
                lblvalidation1.Visible = false;
                loadfromsetting();
                loadfinanceyear();
            }
            //bindclg();
            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
                clgCode = Convert.ToString(ddl_college.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("Student_FeeExtn.aspx", false);
    }

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
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Fee Extention Report";
            string pagename = "Student_FeeExtn.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblvalidation1.Visible = false;
        }
        catch
        {

        }
    }

    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getrno(string prefixText)
    //{
    //    WebService ws = new WebService();
    //    List<string> name = new List<string>();
    //    string query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code='" + collegecode1 + "'";
    //    name = ws.Getname(query);
    //    return name;
    //}
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
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code='" + clgCode + "' order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code='" + clgCode + "' order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code='" + clgCode + "' order by Roll_admit asc";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' and college_code='" + clgCode + "' order by app_formno asc";
                }
            }
            else if (personmode == 1)
            {
                //staff query
            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_college.SelectedIndex = ddl_college.Items.IndexOf(ddl_college.Items.FindByValue(ddl_college.SelectedItem.Value));
            collegecode1 = Convert.ToString(ddl_college.SelectedItem.Value);
            clgCode = Convert.ToString(ddl_college.SelectedItem.Value);
            loadsem();
            loadfinanceyear();
        }
        catch
        {

        }
    }

    protected void txt_rerollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string rollno = Convert.ToString(txt_rerollno.Text);
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Sections ,r.Batch_Year,d.Degree_Code,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code ";
            if (rollno != "" && rollno != null)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        query = query + "and r.Roll_no='" + rollno + "' and d.college_code=" + collegecode1 + "";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        query = query + "and r.Reg_No='" + rollno + "' and d.college_code=" + collegecode1 + "";
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        query = query + "and r.Roll_Admit='" + rollno + "' and d.college_code=" + collegecode1 + "";
                }
                else
                {
                    query = "select a.parent_name,a.Batch_Year,stud_name,c.Course_Name,dt.Dept_Name,c.Course_Name+' - '+ dt.Dept_Name as degree,''Sections,a.college_code,a.Degree_Code ,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and admission_status =0 and isconfirm ='1' and app_formno = '" + rollno + "' and d.college_code='" + collegecode1 + "'";
                }

                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // txt_rerollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_rename.Text = ds.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_rebatch.Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_redegree.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_redept.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_resec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                        ddl_college.SelectedValue = ds.Tables[0].Rows[i]["college_code"].ToString();
                        txt_restrm.Text = ds.Tables[0].Rows[i]["type"].ToString();
                        string ledgerfk = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        ViewState["degid"] = ledgerfk;
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "' and college_code='" + collegecode1 + "'");
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "' and college_code='" + collegecode1 + "'");
                    }
                    if (rollno != "")
                        image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                }
            }
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                txt_rerollno.Text = "";
                txt_rebatch.Text = "";
                txt_redegree.Text = "";
                txt_redept.Text = "";
                txt_resec.Text = "";
                ddl_college.SelectedIndex = 0;
                txt_restrm.Text = "";
                txt_rename.Text = "";
                image3.ImageUrl = "";
            }
            bindspread();
        }
        catch
        {

        }
        if (FpSpread1.Visible)
        {
            tblExpo.Visible = true;
            tblAddConc.Visible = true;
        }
        else
        {
            tblExpo.Visible = false;
            tblAddConc.Visible = false;
        }
    }

    protected void chk_AddConc_Change(object sender, EventArgs e)
    {
        radAmtConc.Checked = true;
        radPerConc.Checked = false;
        if (chk_AddConc.Checked == true)
        {
            ddlAddConc.Enabled = true;
            btnAddConc.Enabled = true;
            bindReason();
        }
        else
        {
            ddlAddConc.Enabled = false;
            btnAddConc.Enabled = false;
            ddlAddConc.Items.Clear();
        }
    }



    protected void btnAddConc_Click(object sender, EventArgs e)
    {
        bool check = false;
        Dictionary<string, string> dictAddConc = new Dictionary<string, string>();
        string feeCat = string.Empty;
        string deductReas = string.Empty;
        string degreeCode = string.Empty;
        if (cbl_sem.Items.Count > 0)
        {
            for (int jk = 0; jk < cbl_sem.Items.Count; jk++)
            {
                if (cbl_sem.Items[jk].Selected == true)
                {
                    if (feeCat.Trim() == "")
                        feeCat = Convert.ToString(cbl_sem.Items[jk].Value);
                    else
                        feeCat = feeCat + "','" + Convert.ToString(cbl_sem.Items[jk].Value);
                }
            }
        }
        degreeCode = Convert.ToString(ViewState["degid"]);
        if (ddlAddConc.Items.Count > 0 && ddlAddConc.SelectedItem.Text != "Select")
            deductReas = Convert.ToString(ddlAddConc.SelectedItem.Value);
        string finyearfk = Convert.ToString(ddlfinyear.SelectedValue);
        if (!string.IsNullOrEmpty(deductReas) && !string.IsNullOrEmpty(degreeCode))
        {
            string appNno = getAppNo();
            string eduLevel = d2.GetFunction("select distinct c.edu_level from registration r, degree d,course c,department dt where r.degree_code=d.degree_code and d.course_id=c.course_id and d.dept_code = dt.dept_code and d.college_code='" + ddl_college.SelectedValue + "' and r.app_no='" + appNno + "'");
            string SelQ = "select * from FM_ConcessionRefundSettings where degree_code='" + degreeCode + "' and Fee_Category in('" + feeCat + "') and ConsDesc='" + deductReas + "' and Refmode='1' and finyearfk='" + finyearfk + "' and edu_level='" + eduLevel + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dictAddConc = conceDict(degreeCode, feeCat, deductReas, finyearfk, eduLevel);
                if (dictAddConc.Count > 0)
                {
                    string deductRes = string.Empty;
                    string finyrfk = string.Empty;
                    string appno = string.Empty;
                    FpSpread1.SaveChanges();
                    for (int spr = 0; spr < FpSpread1.Sheets[0].Rows.Count; spr++)
                    {
                        //double value = 0;
                        //double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 1].Value), out value);
                        //if (value == 1)
                        //{
                        double feeAmt = 0;
                        double totalAmt = 0;
                        double PaidAmnt = 0;
                        double concesAmt = 0;
                        double balAmt = 0;
                        string FeeCat = Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 2].Tag);
                        string HdrFK = Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 3].Tag);
                        string LdgrFK = Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 3].Note);
                        appno = Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 5].Note);
                        finyrfk = Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 6].Note);

                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 5].Text), out feeAmt);
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 6].Text), out concesAmt);
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 7].Text), out totalAmt);
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 8].Text), out PaidAmnt);
                        double.TryParse(Convert.ToString(FpSpread1.Sheets[0].Cells[spr, 9].Text), out balAmt);
                        string strtext = FeeCat + "-" + HdrFK + "-" + LdgrFK;
                        if (dictAddConc.ContainsKey(strtext))
                        {
                            string getVal = Convert.ToString(dictAddConc[strtext]);
                            string[] splstr = getVal.Split('-');
                            if (splstr.Length > 0)
                            {
                                double deductAmt = 0;
                                double excessAmt = 0;
                                if (rblconsType.SelectedIndex == 0)//concession with original amt
                                {
                                    #region
                                    if (splstr[1] == "PER")
                                    {
                                        deductAmt = (feeAmt / 100) * Convert.ToDouble(splstr[0]);
                                        concesAmt = deductAmt;
                                        totalAmt = feeAmt;
                                        totalAmt -= deductAmt;
                                        if (PaidAmnt > totalAmt)
                                        {
                                            excessAmt = PaidAmnt - totalAmt;
                                            PaidAmnt = excessAmt;
                                            balAmt = 0;
                                        }
                                        else
                                            balAmt = totalAmt - PaidAmnt;
                                        deductRes = splstr[2];
                                    }
                                    else
                                    {
                                        deductAmt = Convert.ToDouble(splstr[0]);
                                        concesAmt = deductAmt;
                                        totalAmt = feeAmt;
                                        totalAmt -= deductAmt;
                                        if (PaidAmnt > totalAmt)
                                        {
                                            excessAmt = PaidAmnt - totalAmt;
                                            PaidAmnt = excessAmt;
                                            balAmt = 0;
                                        }
                                        else
                                            balAmt = totalAmt - PaidAmnt;
                                        deductRes = splstr[2];
                                    }
                                    #endregion
                                }
                                else//already given amt
                                {
                                    #region
                                    if (splstr[1] == "PER")
                                    {
                                        deductAmt = (totalAmt / 100) * Convert.ToDouble(splstr[0]);
                                        concesAmt += deductAmt;
                                        totalAmt -= deductAmt;
                                        if (PaidAmnt > totalAmt)
                                        {
                                            excessAmt = PaidAmnt - totalAmt;
                                            PaidAmnt = excessAmt;
                                            balAmt = 0;
                                        }
                                        else
                                            balAmt = totalAmt - PaidAmnt;
                                        deductRes = splstr[2];
                                    }
                                    else
                                    {
                                        deductAmt = Convert.ToDouble(splstr[0]);
                                        concesAmt += deductAmt;
                                        totalAmt -= deductAmt;
                                        if (PaidAmnt > totalAmt)
                                        {
                                            excessAmt = PaidAmnt - totalAmt;
                                            PaidAmnt = excessAmt;
                                            balAmt = 0;
                                        }
                                        else
                                            balAmt = totalAmt - PaidAmnt;
                                        deductRes = splstr[2];
                                    }
                                    #endregion
                                }
                                check = updateFeeallot(totalAmt, PaidAmnt, concesAmt, balAmt, FeeCat, HdrFK, LdgrFK, deductRes, finyrfk, appno);
                                if (excessAmt != 0)
                                {
                                    check = movetoExcess(appno, excessAmt, FeeCat, HdrFK, LdgrFK, finyrfk);
                                }
                            }
                        }
                        //}
                    }
                    if (check)
                    {
                        txt_rerollno_TextChanged(sender, e);
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Concession Updated ";
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Concession Ledger Not Available!";
                    }
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Please Select Concession!";
                }
            }
        }
        else
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select Concession!";
        }

    }

    protected Dictionary<string, string> conceDict(string degreeCode, string feeCat, string deductReas, string finyearfk, string eduLevel)
    {
        Dictionary<string, string> conceDict = new Dictionary<string, string>();
        try
        {

            string SelQ = "select * from FM_ConcessionRefundSettings where degree_code='" + degreeCode + "' and Fee_Category in('" + feeCat + "') and ConsDesc='" + deductReas + "' and Refmode='1' and finyearfk='" + finyearfk + "' and edu_level='" + eduLevel + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int cnc = 0; cnc < ds.Tables[0].Rows.Count; cnc++)
                {
                    double ConcAmnt = 0;
                    if (!conceDict.ContainsKey(Convert.ToString(ds.Tables[0].Rows[cnc]["Fee_Category"]) + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["HeaderFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["LedgerFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["degree_code"])))
                    {
                        string mode = string.Empty;
                        if (radPerConc.Checked == true)
                        {
                            double.TryParse(Convert.ToString(ds.Tables[0].Rows[cnc]["ConsPer"]), out ConcAmnt);
                            mode = "PER";
                        }
                        else if (radAmtConc.Checked == true)
                        {
                            double.TryParse(Convert.ToString(ds.Tables[0].Rows[cnc]["ConsAmt"]), out ConcAmnt);
                            mode = "AMT";
                        }
                        if (ConcAmnt != 0)
                        {
                            conceDict.Add(Convert.ToString(ds.Tables[0].Rows[cnc]["Fee_Category"]) + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["HeaderFK"]) + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["LedgerFK"]), Convert.ToString(ConcAmnt) + "-" + mode + "-" + Convert.ToString(ds.Tables[0].Rows[cnc]["ConsDesc"]));
                        }
                    }
                }
            }
        }
        catch { }
        return conceDict;
    }

    protected bool updateFeeallot(double totalAmt, double PaidAmnt, double deductAmt, double balAmt, string FeeCat, string HdrFK, string LdgrFK, string deductRes, string finyrfk, string appno)
    {
        bool save = false;
        string updQ = "update ft_feeallot set totalamount='" + totalAmt + "',paidamount='" + PaidAmnt + "',balamount='" + balAmt + "',deductamout='" + deductAmt + "',deductreason='" + deductRes + "' where feecategory='" + FeeCat + "' and headerfk='" + HdrFK + "' and ledgerfk='" + LdgrFK + "' and finyearfk='" + finyrfk + "' and app_no='" + appno + "' and memtype='1'";
        int upd = d2.update_method_wo_parameter(updQ, "Text");
        if (upd > 0)
            save = true;
        return save;
    }

    protected bool movetoExcess(string appNo, double excessAmt, string FeeCat, string HdrFK, string LdgrFK, string finyrfk)
    {
        bool save = false;
        string insExcess = "  if exists(select * from ft_excessdet where excesstransdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and feecategory='" + FeeCat + "' and finyearfk='" + finyrfk + "' and excesstype='2' and app_no='" + appNo + "')update ft_excessdet set excessamt=isnull(excessamt,'0')+'" + excessAmt + "',adjamt=isnull(adjamt,'0')+'0',balanceamt=isnull(balanceamt,'0')+'" + excessAmt + "' where excesstransdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'  and feecategory='" + FeeCat + "' and finyearfk='" + finyrfk + "' and excesstype='2' and app_no='" + appNo + "' else insert into ft_excessdet(excesstransdate,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,finyearfk,feecategory) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','" + appNo + "','1','2','" + excessAmt + "','0','" + excessAmt + "','" + finyrfk + "','" + FeeCat + "')";
        int insex = d2.update_method_wo_parameter(insExcess, "Text");
        if (insex == 1)
        {
            string excesspk = d2.GetFunction(" select excessdetPk from ft_excessdet where excesstransdate='" + DateTime.Now.ToString("MM/dd/yyyy") + "' and feecategory='" + FeeCat + "' and finyearfk='" + finyrfk + "' and excesstype='2'");
            if (excesspk != "0")
            {
                string insledexces = "  if exists(select * from FT_ExcessLedgerDet where headerfk='" + HdrFK + "' and ledgerfk='" + LdgrFK + "' and excessdetfk='" + excesspk + "' and feecategory='" + FeeCat + "' and finyearfk='" + finyrfk + "' ) update FT_ExcessLedgerDet set excessamt=isnull(excessamt,'0')+'" + excessAmt + "',adjamt=isnull(adjamt,'0')+'0',balanceamt=isnull(balanceamt,'0')+'" + excessAmt + "'  where headerfk='" + HdrFK + "' and ledgerfk='" + LdgrFK + "' and excessdetfk='" + excesspk + "' and feecategory='" + FeeCat + "' and finyearfk='" + finyrfk + "' else insert into FT_ExcessLedgerDet(headerfk,ledgerfk,excessamt,adjamt,balanceamt,excessdetfk,feecategory,finyearfk) values('" + HdrFK + "','" + LdgrFK + "','" + excessAmt + "','0','" + excessAmt + "','" + excesspk + "','" + FeeCat + "','" + finyrfk + "')";
                int insexs = d2.update_method_wo_parameter(insledexces, "Text");
                save = true;
            }
        }
        return save;
    }

    private void bindReason()
    {
        ddlAddConc.Items.Clear();
        ds.Tables.Clear();
        string college = ddl_college.SelectedItem.Value.ToString();
        string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='DedRe' and college_code ='" + college + "'";
        ds = d2.select_method_wo_parameter(sql, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlAddConc.DataSource = ds;
            ddlAddConc.DataTextField = "TextVal";
            ddlAddConc.DataValueField = "TextCode";
            ddlAddConc.DataBind();
            ddlAddConc.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddlAddConc.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    protected void typegrid_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[1].Text = lbl_sem.Text;
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //  e.Row.Cells[6].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Concession$" + e.Row.RowIndex);
                string value = "Concession$" + e.Row.RowIndex;

                e.Row.Cells[6].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Concession$" + e.Row.RowIndex);
                //e.Row.Cells[9].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[10].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check1$" + e.Row.RowIndex);
                //e.Row.Cells[11].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[12].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check2$" + e.Row.RowIndex);
                //e.Row.Cells[13].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[14].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check3$" + e.Row.RowIndex);
                //e.Row.Cells[15].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[16].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check4$" + e.Row.RowIndex);
                //e.Row.Cells[17].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[18].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check5$" + e.Row.RowIndex);
                //e.Row.Cells[19].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[20].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check6$" + e.Row.RowIndex);
                //e.Row.Cells[21].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[22].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check7$" + e.Row.RowIndex);
                //e.Row.Cells[23].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[24].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check8$" + e.Row.RowIndex);
                //e.Row.Cells[25].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[26].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check9$" + e.Row.RowIndex);
                //e.Row.Cells[27].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Date$" + e.Row.RowIndex);
                //e.Row.Cells[28].Attributes["onchange"] = Page.ClientScript.GetPostBackEventReference(this.gridViewpop, "Check10$" + e.Row.RowIndex);
            }
        }
        catch
        {

        }
    }

    protected void gridViewpop_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            double cons1 = 0.00;
            double cons2 = 0.00;
            double cons3 = 0.00;
            double cons4 = 0.00;
            double cons5 = 0.00;
            double cons6 = 0.00;
            double cons7 = 0.00;
            double cons8 = 0.00;
            double cons9 = 0.00;
            double cons10 = 0.00;
            double totcons = 0.00;
            double bal = 0.00;
            if (e.CommandName == "Check1" || e.CommandName == "Check2" || e.CommandName == "Check3" || e.CommandName == "Check4" || e.CommandName == "Check5" || e.CommandName == "Check6" || e.CommandName == "Check7" || e.CommandName == "Check8" || e.CommandName == "Check9" || e.CommandName == "Check10")
            {
                int row = Convert.ToInt32(e.CommandArgument);
                TextBox txtcons1 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess1");
                if (txtcons1.Text.Trim() != "")
                {
                    cons1 = Convert.ToDouble(txtcons1.Text);
                }
                else
                {
                    cons1 = 0.00;
                }
                TextBox txtcons2 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess2");
                if (txtcons2.Text.Trim() != "")
                {
                    cons2 = Convert.ToDouble(txtcons2.Text);
                }
                else
                {
                    cons2 = 0.00;
                }
                TextBox txtcons3 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess3");
                if (txtcons3.Text.Trim() != "")
                {
                    cons3 = Convert.ToDouble(txtcons3.Text);
                }
                else
                {
                    cons3 = 0.00;
                }
                TextBox txtcons4 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess4");
                if (txtcons4.Text.Trim() != "")
                {
                    cons4 = Convert.ToDouble(txtcons4.Text);
                }
                else
                {
                    cons4 = 0.00;
                }
                TextBox txtcons5 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess5");
                if (txtcons5.Text.Trim() != "")
                {
                    cons5 = Convert.ToDouble(txtcons5.Text);
                }
                else
                {
                    cons5 = 0.00;
                }
                TextBox txtcons6 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess6");
                if (txtcons6.Text.Trim() != "")
                {
                    cons6 = Convert.ToDouble(txtcons6.Text);
                }
                else
                {
                    cons6 = 0.00;
                }
                TextBox txtcons7 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess7");
                if (txtcons7.Text.Trim() != "")
                {
                    cons7 = Convert.ToDouble(txtcons7.Text);
                }
                else
                {
                    cons7 = 0.00;
                }
                TextBox txtcons8 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess8");
                if (txtcons8.Text.Trim() != "")
                {
                    cons8 = Convert.ToDouble(txtcons8.Text);
                }
                else
                {
                    cons8 = 0.00;
                }
                TextBox txtcons9 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess9");
                if (txtcons9.Text.Trim() != "")
                {
                    cons9 = Convert.ToDouble(txtcons9.Text);
                }
                else
                {
                    cons9 = 0.00;
                }
                TextBox txtcons10 = (TextBox)gridViewpop.Rows[row].FindControl("txt_addconcess10");
                if (txtcons10.Text.Trim() != "")
                {
                    cons10 = Convert.ToDouble(txtcons10.Text);
                }
                else
                {
                    cons10 = 0.00;
                }
                Label balamnt = (Label)gridViewpop.Rows[row].FindControl("lbl_balamnt");
                if (balamnt.Text == "" || balamnt.Text == "0.00")
                {
                    bal = 0.00;
                }
                else
                {
                    bal = Convert.ToDouble(balamnt.Text);
                }
                if (txt_datewithamnt.Text == "1")
                {
                    if (cons1 > bal || cons1 < bal)
                    {
                        txtcons1.Text = "";
                    }
                    else
                    {
                        txtcons1.Text = Convert.ToString(cons1);
                    }
                }
                if (txt_datewithamnt.Text == "2")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            totcons = cons1 + cons2;
                            if (totcons > bal || totcons < bal)
                            {
                                txtcons2.Text = "";
                            }
                            else
                            {
                                txtcons2.Text = Convert.ToString(cons2);
                            }
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons2.Text = "";
                    }
                }
                if (txt_datewithamnt.Text == "3")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                totcons = cons1 + cons2 + cons3;
                                if (totcons > bal || totcons < bal)
                                {
                                    txtcons3.Text = "";
                                }
                                else
                                {
                                    txtcons3.Text = Convert.ToString(cons3);
                                }
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons3.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons3.Text = "";
                    }
                }
                if (txt_datewithamnt.Text == "4")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    totcons = cons1 + cons2 + cons3 + cons4;
                                    if (totcons > bal || totcons < bal)
                                    {
                                        txtcons4.Text = "";
                                    }
                                    else
                                    {
                                        txtcons4.Text = Convert.ToString(cons4);
                                    }
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons4.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons4.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons4.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "5")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        totcons = cons1 + cons2 + cons3 + cons4 + cons5;
                                        if (totcons > bal || totcons < bal)
                                        {
                                            txtcons5.Text = "";
                                        }
                                        else
                                        {
                                            txtcons5.Text = Convert.ToString(cons5);
                                        }
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons5.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons5.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons5.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons5.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "6")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        if (txtcons6.Text.Trim() != "")
                                        {
                                            totcons = cons1 + cons2 + cons3 + cons4 + cons5 + cons6;
                                            if (totcons > bal || totcons < bal)
                                            {
                                                txtcons6.Text = "";
                                            }
                                            else
                                            {
                                                txtcons6.Text = Convert.ToString(cons6);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please Enter the Amount5!";
                                        txtcons6.Text = "";
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons6.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons6.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons6.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons6.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "7")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        if (txtcons6.Text.Trim() != "")
                                        {
                                            if (txtcons7.Text.Trim() != "")
                                            {
                                                totcons = cons1 + cons2 + cons3 + cons4 + cons5 + cons6 + cons7;
                                                if (totcons > bal || totcons < bal)
                                                {
                                                    txtcons7.Text = "";
                                                }
                                                else
                                                {
                                                    txtcons7.Text = Convert.ToString(cons7);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Please Enter the Amount6!";
                                            txtcons7.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please Enter the Amount5!";
                                        txtcons7.Text = "";
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons7.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons7.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons7.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons7.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "8")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        if (txtcons6.Text.Trim() != "")
                                        {
                                            if (txtcons7.Text.Trim() != "")
                                            {
                                                if (txtcons8.Text.Trim() != "")
                                                {
                                                    totcons = cons1 + cons2 + cons3 + cons4 + cons5 + cons6 + cons7 + cons8;
                                                    if (totcons > bal || totcons < bal)
                                                    {
                                                        txtcons8.Text = "";
                                                    }
                                                    else
                                                    {
                                                        txtcons8.Text = Convert.ToString(cons8);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                alertpopwindow.Visible = true;
                                                lblalerterr.Visible = true;
                                                lblalerterr.Text = "Please Enter the Amount7!";
                                                txtcons8.Text = "";
                                            }
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Please Enter the Amount6!";
                                            txtcons8.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please Enter the Amount5!";
                                        txtcons8.Text = "";
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons8.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons8.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons8.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons8.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "9")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        if (txtcons6.Text.Trim() != "")
                                        {
                                            if (txtcons7.Text.Trim() != "")
                                            {
                                                if (txtcons8.Text.Trim() != "")
                                                {
                                                    if (txtcons9.Text.Trim() != "")
                                                    {
                                                        totcons = cons1 + cons2 + cons3 + cons4 + cons5 + cons6 + cons7 + cons8 + cons9;
                                                        if (totcons > bal || totcons < bal)
                                                        {
                                                            txtcons9.Text = "";
                                                        }
                                                        else
                                                        {
                                                            txtcons9.Text = Convert.ToString(cons9);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Visible = true;
                                                    lblalerterr.Text = "Please Enter the Amount8!";
                                                    txtcons9.Text = "";
                                                }
                                            }
                                            else
                                            {
                                                alertpopwindow.Visible = true;
                                                lblalerterr.Visible = true;
                                                lblalerterr.Text = "Please Enter the Amount7!";
                                                txtcons9.Text = "";
                                            }
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Please Enter the Amount6!";
                                            txtcons9.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please Enter the Amount5!";
                                        txtcons9.Text = "";
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons9.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons9.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons9.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons9.Text = "";
                    }
                }

                if (txt_datewithamnt.Text == "10")
                {
                    if (txtcons1.Text.Trim() != "")
                    {
                        if (txtcons2.Text.Trim() != "")
                        {
                            if (txtcons3.Text.Trim() != "")
                            {
                                if (txtcons4.Text.Trim() != "")
                                {
                                    if (txtcons5.Text.Trim() != "")
                                    {
                                        if (txtcons6.Text.Trim() != "")
                                        {
                                            if (txtcons7.Text.Trim() != "")
                                            {
                                                if (txtcons8.Text.Trim() != "")
                                                {
                                                    if (txtcons9.Text.Trim() != "")
                                                    {
                                                        if (txtcons10.Text.Trim() != "")
                                                        {
                                                            totcons = cons1 + cons2 + cons3 + cons4 + cons5 + cons6 + cons7 + cons8 + cons9 + cons10;
                                                            if (totcons > bal || totcons < bal)
                                                            {
                                                                txtcons10.Text = "";
                                                            }
                                                            else
                                                            {
                                                                txtcons10.Text = Convert.ToString(cons10);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        alertpopwindow.Visible = true;
                                                        lblalerterr.Visible = true;
                                                        lblalerterr.Text = "Please Enter the Amount9!";
                                                        txtcons10.Text = "";
                                                    }
                                                }
                                                else
                                                {
                                                    alertpopwindow.Visible = true;
                                                    lblalerterr.Visible = true;
                                                    lblalerterr.Text = "Please Enter the Amount8!";
                                                    txtcons10.Text = "";
                                                }
                                            }
                                            else
                                            {
                                                alertpopwindow.Visible = true;
                                                lblalerterr.Visible = true;
                                                lblalerterr.Text = "Please Enter the Amount7!";
                                                txtcons10.Text = "";
                                            }
                                        }
                                        else
                                        {
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Please Enter the Amount6!";
                                            txtcons10.Text = "";
                                        }
                                    }
                                    else
                                    {
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Please Enter the Amount5!";
                                        txtcons10.Text = "";
                                    }
                                }
                                else
                                {
                                    alertpopwindow.Visible = true;
                                    lblalerterr.Visible = true;
                                    lblalerterr.Text = "Please Enter the Amount4!";
                                    txtcons10.Text = "";
                                }
                            }
                            else
                            {
                                alertpopwindow.Visible = true;
                                lblalerterr.Visible = true;
                                lblalerterr.Text = "Please Enter the Amount3!";
                                txtcons10.Text = "";
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount2!";
                            txtcons10.Text = "";
                        }
                    }
                    else
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Please Enter the Amount1!";
                        txtcons10.Text = "";
                    }
                }
            }
            else if (e.CommandName == "Date")
            {


            }
            else if (e.CommandName == "Concession")
            {

            }
        }
        catch
        {

        }
    }

    protected void typebound(object sender, EventArgs e)
    {
        try
        {
            if (gridViewpop.Rows.Count > 0)
            {
                for (int i = 0; i < gridViewpop.Rows.Count; i++)
                {
                    string typevalue = ((gridViewpop.Rows[i].FindControl("typelnk1") as Label).Text);
                    if (typevalue.ToString().ToUpper() == "DAY")
                    {
                        (gridViewpop.Rows[i].FindControl("typeextendlilnk") as Label).Text = "Govt Aided Stream (Day)";
                    }
                    if (typevalue.ToString().ToUpper() == "EVENING")
                    {
                        (gridViewpop.Rows[i].FindControl("typeextendlilnk") as Label).Text = "Self Financed Stream (Evening)";
                    }
                    if (typevalue.ToString().Trim() == "MCA")
                    {
                        (gridViewpop.Rows[i].FindControl("typeextendlilnk") as Label).Text = "MCA-Self Financed Stream (Day)";
                    }
                }
            }
        }
        catch
        {
        }
    }

    //protected void txt_addconcess_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        double totamnt = 0.00;
    //        Label lblfee = new Label();
    //        Label lblfeecode = new Label();
    //        Label tot = new Label();
    //        Label header = new Label();
    //        Label ledger = new Label();
    //        Label balamt = new Label();
    //        string selQ = "";
    //        string balamnt = "";
    //        string app_no = "";
    //        lblfeeamt.Text = "";
    //        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
    //        {
    //            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
    //            {

    //                app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "'");
    //                //  query = query + "and r.Roll_no='" + rollno + "' and d.college_code=" + collegecode1 + "";
    //            }
    //            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
    //            {
    //                app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + txt_rerollno.Text.Trim() + "'");
    //                // query = query + "and r.Reg_No='" + rollno + "' and d.college_code=" + collegecode1 + "";
    //            }
    //            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
    //            {
    //                app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + txt_rerollno.Text.Trim() + "'");
    //                //  query = query + "and r.Roll_Admit='" + rollno + "' and d.college_code=" + collegecode1 + "";
    //            }
    //        }

    //        else
    //        {
    //            app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "'");

    //        }
    //        int rowindex = rowIndxClicked();

    //        if (gridViewpop.Rows.Count > 0)
    //        {
    //            int rowcnt = 0;
    //            foreach (GridViewRow gvpopro in gridViewpop.Rows)
    //            {
    //                if (rowindex == rowcnt)
    //                {


    //                    header = (Label)gvpopro.Cells[2].FindControl("lbl_hdridpop");
    //                    ledger = (Label)gvpopro.Cells[3].FindControl("lbl_lgrid");
    //                    lblfee = (Label)gvpopro.Cells[4].FindControl("lbl_fee");
    //                    lblfeecode = (Label)gvpopro.Cells[1].FindControl("lbl_feecode");
    //                    tot = (Label)gvpopro.Cells[10].FindControl("lbl_tot");
    //                    TextBox txtconcess = (TextBox)gvpopro.Cells[7].FindControl("txt_concess");
    //                    TextBox txtaddconcess = (TextBox)gvpopro.Cells[8].FindControl("txt_addconcess");
    //                    //tmpconsamt
    //                    Label tmpconsamt = (Label)gvpopro.Cells[9].FindControl("tmpconsamt");

    //                    balamt = (Label)gvpopro.Cells[11].FindControl("lbl_balamnt");
    //                    if (txtconcess.Text == "")
    //                    {
    //                        txtconcess.Text = "0.00";
    //                    }
    //                    if (txtaddconcess.Text == "")
    //                    {
    //                        txtaddconcess.Text = "0.00";
    //                    }
    //                    if (lblfee.Text == "")
    //                    {
    //                        lblfee.Text = "0.00";
    //                    }
    //                    double paidamt=0;
    //                    selQ = "select BalAmount from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "'";
    //                    balamnt = d2.GetFunction(selQ);
    //                     string PaidAmt = d2.GetFunction("select ISNULL(PaidAmount,'0') as paid  from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "' and headerfk='" + header.Text + "' and ledgerfk='" + ledger.Text + "'");
    //                      double.TryParse(PaidAmt, out paidamt);
    //                    if (cb_concesstbl.Checked == true)
    //                    {
    //                        double lastconsamt = 0;
    //                        double.TryParse(Convert.ToString(tmpconsamt.Text), out lastconsamt);
    //                        double concess = (Convert.ToDouble(txtconcess.Text) + Convert.ToDouble(txtaddconcess.Text)) - lastconsamt;
    //                        (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = Convert.ToString(txtaddconcess.Text);
    //                        if (txtaddconcess.Text != "0.00" && txtaddconcess.Text != "" && Convert.ToDouble(lblfee.Text) != 0.00)
    //                        {
    //                            if (Convert.ToDouble(balamnt) >= concess || Convert.ToDouble(balamnt) >= Convert.ToDouble(txtaddconcess.Text))
    //                            {
    //                                totamnt = Convert.ToDouble(lblfee.Text) - concess;
    //                                txtconcess.Text = Convert.ToString(concess);
    //                                tot.Text = totamnt.ToString();
    //                                balamt.Text = Convert.ToString(totamnt - paidamt);
    //                            }
    //                            else
    //                            {
    //                                txtaddconcess.Text = "0.00";
    //                                txtconcess.Text = Convert.ToString(Convert.ToDouble(txtconcess.Text) - lastconsamt);
    //                                totamnt = Convert.ToDouble(lblfee.Text) - Convert.ToDouble(txtconcess.Text);
    //                                tot.Text = totamnt.ToString();
    //                                balamt.Text = Convert.ToString(totamnt - paidamt);

    //                                (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
    //                                (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
    //                                alertpopwindow.Visible = true;
    //                                lblalerterr.Visible = true;
    //                                lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";

    //                            }
    //                        }
    //                        else
    //                        {
    //                            double oldamt = 0;
    //                            double.TryParse(Convert.ToString(lblfeeamt.Text), out oldamt);
    //                            //string oldamt = Convert.ToString(lblfeeamt.Text);
    //                            double fnlcons = Convert.ToDouble(txtconcess.Text) - lastconsamt;
    //                            txtconcess.Text = Convert.ToString(fnlcons);
    //                            totamnt = Convert.ToDouble(lblfee.Text) - concess;
    //                            tot.Text = totamnt.ToString();
    //                            balamt.Text = Convert.ToString(totamnt - paidamt);
    //                            lblfeeamt.Text = "";
    //                        }

    //                    }
    //                    else
    //                    {
    //                        double lastconsamt = 0;
    //                        double.TryParse(Convert.ToString(tmpconsamt.Text), out lastconsamt);
    //                        double balval = 0;
    //                        double concess = Convert.ToDouble(txtconcess.Text);
    //                        double decrsamt = Convert.ToDouble(txtaddconcess.Text);
    //                        (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = Convert.ToString(decrsamt);
    //                        if (concess != 0 && concess != 0.00 && decrsamt != 0 && decrsamt != 0.00)
    //                        {
    //                            if (concess >= decrsamt)
    //                            {
    //                                balval = (concess - decrsamt) + lastconsamt;
    //                                if (balval != 0 && balval > 0)
    //                                {
    //                                    totamnt = Convert.ToDouble(lblfee.Text) - balval;
    //                                    txtconcess.Text = Convert.ToString(balval);
    //                                    tot.Text = totamnt.ToString();
    //                                    balamt.Text = Convert.ToString(totamnt - paidamt);
    //                                }
    //                                else
    //                                {
    //                                    txtaddconcess.Text = "0.00";
    //                                    totamnt = Convert.ToDouble(lblfee.Text) - Convert.ToDouble(balval);
    //                                    txtconcess.Text = Convert.ToString(balval);
    //                                    tot.Text = totamnt.ToString();
    //                                    balamt.Text = Convert.ToString(totamnt - paidamt);

    //                                }
    //                            }
    //                            else
    //                            {

    //                                txtconcess.Text = Convert.ToString(lastconsamt + concess);
    //                                totamnt = Convert.ToDouble(lblfee.Text) - Convert.ToDouble(txtconcess.Text);
    //                                tot.Text = totamnt.ToString();
    //                                balamt.Text = Convert.ToString(totamnt - paidamt);
    //                                (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
    //                                (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
    //                                alertpopwindow.Visible = true;
    //                                lblalerterr.Visible = true;
    //                                lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";
    //                            }

    //                        }
    //                        else
    //                        {
    //                            if (concess >= decrsamt)
    //                            {
    //                                txtconcess.Text = Convert.ToString(concess + lastconsamt);
    //                                totamnt = Convert.ToDouble(lblfee.Text) - Convert.ToDouble(txtconcess.Text);
    //                                tot.Text = totamnt.ToString();
    //                                balamt.Text = Convert.ToString(totamnt - paidamt);
    //                                txtaddconcess.Text = "";
    //                                (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
    //                            }
    //                            else
    //                            {
    //                                txtconcess.Text = Convert.ToString((concess + lastconsamt) - decrsamt);
    //                                totamnt = Convert.ToDouble(lblfee.Text) - Convert.ToDouble(txtconcess.Text);
    //                                tot.Text = totamnt.ToString();
    //                                balamt.Text = Convert.ToString(totamnt - paidamt);
    //                                txtaddconcess.Text = "";
    //                                (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
    //                            }
    //                            //  alertpopwindow.Visible = true;
    //                            //   lblalerterr.Visible = true;
    //                            // lblalerterr.Text = "Don't have Amount to Deduct!";
    //                        }

    //                    }
    //                }
    //                rowcnt++;
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    protected void txt_addconcess_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string app_no = "";
            string balamnt = "";
            #region appno

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            }
            else
                app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");

            #endregion
            int rowindex = rowIndxClicked();
            if (gridViewpop.Rows.Count > 0)
            {
                int rowcnt = 0;
                foreach (GridViewRow gvpopro in gridViewpop.Rows)
                {
                    if (rowindex == rowcnt)
                    {
                        Label header = (Label)gvpopro.Cells[2].FindControl("lbl_hdridpop");
                        Label ledger = (Label)gvpopro.Cells[3].FindControl("lbl_lgrid");
                        Label lblfee = (Label)gvpopro.Cells[4].FindControl("lbl_fee");
                        Label lblfeecode = (Label)gvpopro.Cells[1].FindControl("lbl_feecode");
                        Label totamt = (Label)gvpopro.Cells[10].FindControl("lbl_tot");
                        TextBox txtconcess = (TextBox)gvpopro.Cells[7].FindControl("txt_concess");
                        TextBox txtaddconcess = (TextBox)gvpopro.Cells[8].FindControl("txt_addconcess");
                        Label balamt = (Label)gvpopro.Cells[11].FindControl("lbl_balamnt");
                        //tmpconsamt
                        Label tmpconsamt = (Label)gvpopro.Cells[9].FindControl("tmpconsamt");
                        if (txtconcess.Text == "")
                            txtconcess.Text = "0.00";

                        if (txtaddconcess.Text == "")
                            txtaddconcess.Text = "0.00";

                        balamnt = d2.GetFunction("select BalAmount from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "' and HeaderFK='" + header.Text + "' and LedgerFK='" + ledger.Text + "'");

                        string PaidAmt = d2.GetFunction("select ISNULL(PaidAmount,'0') as paid  from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "' and headerfk='" + header.Text + "' and ledgerfk='" + ledger.Text + "'");
                        double paidamt = 0;
                        double.TryParse(PaidAmt, out paidamt);
                        double feeamt = 0;
                        double consamt = 0;
                        double addconsamt = 0;
                        double Lstconsamt = 0;
                        double BalAmt = 0;
                        double totalconsamt = 0;
                        double totalfeeamt = 0;
                        double.TryParse(Convert.ToString(lblfee.Text), out feeamt);
                        double.TryParse(Convert.ToString(txtconcess.Text), out consamt);
                        double.TryParse(Convert.ToString(txtaddconcess.Text), out addconsamt);
                        double.TryParse(Convert.ToString(tmpconsamt.Text), out Lstconsamt);
                        double.TryParse(Convert.ToString(balamt.Text), out BalAmt);

                        if (cb_concesstbl.Checked == true)
                        {
                            #region Concession add
                            (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = Convert.ToString(addconsamt);
                            if (addconsamt != 0 && addconsamt != 0.00 && feeamt != 0)
                            {
                                totalconsamt = consamt + addconsamt - Lstconsamt;
                                if (BalAmt > 0)
                                {
                                    BalAmt += Lstconsamt;
                                    if (BalAmt >= addconsamt)
                                    {
                                        totalfeeamt = (feeamt - totalconsamt);
                                        txtconcess.Text = Convert.ToString(totalconsamt);
                                        totamt.Text = Convert.ToString(totalfeeamt);
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                    }
                                    else
                                    {
                                        txtaddconcess.Text = "0.00";
                                        txtconcess.Text = Convert.ToString(consamt - Lstconsamt);
                                        totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                        totamt.Text = Convert.ToString(totalfeeamt);
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                        (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";
                                    }
                                }
                                else
                                {
                                    if (Lstconsamt >= addconsamt)
                                    {
                                        // txtaddconcess.Text = "0.00";
                                        txtconcess.Text = Convert.ToString((consamt - Lstconsamt) + addconsamt);
                                        totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                        totamt.Text = Convert.ToString(totalfeeamt);
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                    }
                                    else
                                    {
                                        txtaddconcess.Text = "0.00";
                                        txtconcess.Text = Convert.ToString(consamt - Lstconsamt);
                                        totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                        totamt.Text = Convert.ToString(totalfeeamt);
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                        (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";
                                    }
                                }
                            }
                            else
                            {
                                double fnlcons = consamt - Lstconsamt;
                                txtconcess.Text = Convert.ToString(fnlcons);
                                totalfeeamt = feeamt - fnlcons;
                                totamt.Text = Convert.ToString(totalfeeamt);
                                balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                            }
                            #endregion
                        }
                        else
                        {
                            #region
                            double balval = 0;
                            (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = Convert.ToString(addconsamt);
                            if (addconsamt != 0 && addconsamt != 0.00)
                            {
                                //consamt != 0 && consamt != 0.00 &&
                                if (consamt > 0)
                                {
                                    if (consamt >= addconsamt)
                                    {
                                        balval = (consamt - addconsamt) + Lstconsamt;
                                        if (balval != 0 && balval > 0)
                                        {
                                            totalfeeamt = feeamt - balval;
                                            txtconcess.Text = Convert.ToString(balval);
                                            totamt.Text = Convert.ToString(totalfeeamt);
                                            balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        }
                                        else
                                        {
                                            //  txtaddconcess.Text = "0.00";
                                            totalfeeamt = feeamt - balval;
                                            txtconcess.Text = Convert.ToString(balval);
                                            totamt.Text = totalfeeamt.ToString();
                                            balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        }
                                    }
                                    else
                                    {
                                        balval = (consamt + Lstconsamt);
                                        if (balval >= addconsamt)
                                        {
                                            balval = (consamt + Lstconsamt) - addconsamt;
                                            txtconcess.Text = Convert.ToString(balval);
                                            totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                            totamt.Text = totalfeeamt.ToString();
                                            balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                            // (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                            // (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";                                           
                                        }
                                        else
                                        {
                                            txtconcess.Text = Convert.ToString(Lstconsamt + consamt);
                                            totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                            totamt.Text = totalfeeamt.ToString();
                                            balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                            (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                            (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
                                            alertpopwindow.Visible = true;
                                            lblalerterr.Visible = true;
                                            lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";
                                        }
                                    }
                                }
                                else
                                {
                                    if (Lstconsamt >= addconsamt)
                                    {
                                        txtconcess.Text = Convert.ToString((Lstconsamt + consamt) - addconsamt);
                                        totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                        totamt.Text = totalfeeamt.ToString();
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        // (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                        // (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
                                    }
                                    else
                                    {
                                        txtconcess.Text = Convert.ToString((Lstconsamt + consamt));
                                        totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                        totamt.Text = totalfeeamt.ToString();
                                        balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                        (gvpopro.Cells[9].FindControl("tmpconsamt") as Label).Text = "0";
                                        (gvpopro.Cells[8].FindControl("txt_addconcess") as TextBox).Text = "0";
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Deduct Amount/Balance Amount should be less than Fee Amount!";
                                    }
                                }

                            }
                            else
                            {
                                txtconcess.Text = Convert.ToString(consamt + Lstconsamt);
                                totalfeeamt = feeamt - Convert.ToDouble(txtconcess.Text);
                                totamt.Text = totalfeeamt.ToString();
                                balamt.Text = Convert.ToString(totalfeeamt - paidamt);
                                txtaddconcess.Text = "";
                            }
                            #endregion
                        }
                    }
                    rowcnt++;
                }
            }

        }
        catch { }
    }


    protected void txtfeemat_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            string app_no = "";
            double totamnt = 0.00;
            string oldamt = "";
            string rollno = Convert.ToString(txt_rerollno.Text).Trim();
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    app_no = d2.GetFunction("select app_no from Registration where roll_no='" + rollno + "' and college_code='" + collegecode1 + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + rollno + "' and college_code='" + collegecode1 + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + rollno + "' and college_code='" + collegecode1 + "'");
            }
            else
                app_no = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "' and college_code='" + collegecode1 + "'");
            int rowindex = rowIndxClicked();
            if (gridViewpop.Rows.Count > 0)
            {
                int rowcnt = 0;
                foreach (GridViewRow gvpopro in gridViewpop.Rows)
                {
                    if (rowcnt == rowindex)
                    {
                        //header
                        Label hedfk = (Label)gvpopro.Cells[2].FindControl("lbl_hdridpop");
                        Label ledfk = (Label)gvpopro.Cells[3].FindControl("lbl_lgrid");

                        Label lblfee = (Label)gvpopro.Cells[4].FindControl("lbl_fee");
                        Label lblfeecode = (Label)gvpopro.Cells[1].FindControl("lbl_feecode");
                        Label tot = (Label)gvpopro.Cells[10].FindControl("lbl_tot");
                        TextBox addtxtfeeamt = (TextBox)gvpopro.Cells[5].FindControl("txtfeemat");
                        //tmpfeeamt
                        Label tmpfeeamt = (Label)gvpopro.Cells[6].FindControl("tmptxtamt");

                        TextBox txtconcess = (TextBox)gvpopro.Cells[7].FindControl("txt_concess");
                        TextBox txtaddconcess = (TextBox)gvpopro.Cells[8].FindControl("txt_addconcess");
                        //tmpconsamt
                        Label tmpconsamt = (Label)gvpopro.Cells[9].FindControl("tmpconsamt");
                        Label balamt = (Label)gvpopro.Cells[11].FindControl("lbl_balamnt");
                        if (addtxtfeeamt.Text.Trim() == "")
                            addtxtfeeamt.Text = "0.00";

                        if (lblfee.Text == "")
                            lblfee.Text = "0.00";

                        string selQ = "select BalAmount from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "'";
                        string balamnt = d2.GetFunction(selQ);


                        if (cbfeeamtadd.Checked == true)
                        {
                            double lastfeeamt = 0;
                            double.TryParse(Convert.ToString(tmpfeeamt.Text), out lastfeeamt);
                            double concess = Convert.ToDouble(txtconcess.Text);
                            double totalfeeamt = (Convert.ToDouble(lblfee.Text) + Convert.ToDouble(addtxtfeeamt.Text)) - lastfeeamt;
                            (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = Convert.ToString(addtxtfeeamt.Text);
                            if (addtxtfeeamt.Text != "0.00" && addtxtfeeamt.Text != "" && Convert.ToDouble(lblfee.Text) != 0.00)
                            {
                                if (totalfeeamt > 0)
                                {
                                    lblfee.Text = Convert.ToString(totalfeeamt);
                                    totamnt = totalfeeamt - concess;
                                    tot.Text = Convert.ToString(totamnt);
                                    balamt.Text = tot.Text.ToString();
                                }
                            }
                            else
                            {
                                double oldfeeval = Convert.ToDouble(lblfee.Text);
                                lblfee.Text = Convert.ToString(oldfeeval - lastfeeamt);
                                totamnt = Convert.ToDouble(lblfee.Text) - concess;
                                tot.Text = totamnt.ToString();
                                balamt.Text = tot.Text.ToString();
                                lblfeeamt.Text = "";
                            }
                        }
                        else
                        {
                            string PaidAmt = d2.GetFunction("select ISNULL(PaidAmount,'0') as paid  from Ft_Feeallot where App_No='" + app_no + "' and FeeCategory='" + lblfeecode.Text + "' and headerfk='" + hedfk.Text + "' and ledgerfk='" + ledfk.Text + "'");
                            double paidamt = 0;
                            double.TryParse(PaidAmt, out paidamt);
                            double lastfeeamt = 0;
                            double.TryParse(Convert.ToString(tmpfeeamt.Text), out lastfeeamt);
                            double concess = Convert.ToDouble(txtconcess.Text);
                            double feeamt = Convert.ToDouble(lblfee.Text);
                            double addfeeamt = Convert.ToDouble(addtxtfeeamt.Text);
                            double totalfeeamt = (feeamt - addfeeamt) + lastfeeamt;
                            //totalfeeamt
                            double totfeeAmt = feeamt - (paidamt + concess);

                            //  totamnt = totalfeeamt - concess;
                            totamnt = totfeeAmt;
                            (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = Convert.ToString(addfeeamt);
                            if (addtxtfeeamt.Text != "0.00" && addtxtfeeamt.Text != "0" && addtxtfeeamt.Text != "" && Convert.ToDouble(lblfee.Text) != 0.00)
                            {
                                if (totfeeAmt > 0)
                                {
                                    totfeeAmt += lastfeeamt;
                                    if (totfeeAmt >= addfeeamt)
                                    {
                                        lblfeeamt.Text = Convert.ToString(addtxtfeeamt.Text);
                                        lblfee.Text = Convert.ToString(totalfeeamt);
                                        totamnt = totalfeeamt - concess;
                                        if (totamnt > 0)
                                        {
                                            tot.Text = Convert.ToString(totamnt);
                                            balamt.Text = Convert.ToString(totamnt - paidamt);
                                        }
                                    }
                                    else
                                    {
                                        addtxtfeeamt.Text = "0.00";
                                        lblfee.Text = Convert.ToString(feeamt + lastfeeamt);
                                        (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = "0";
                                        (gvpopro.Cells[5].FindControl("txtfeemat") as TextBox).Text = "0";
                                        totamnt = Convert.ToDouble(lblfee.Text) - concess;
                                        tot.Text = Convert.ToString(totamnt);
                                        balamt.Text = Convert.ToString(totamnt - paidamt);
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Deduct FeeAmount/Balance Amount should be less than Total Fee Amount!";
                                    }
                                }
                                else
                                {
                                    if (lastfeeamt >= addfeeamt)
                                    {
                                        //   addtxtfeeamt.Text = "0.00";
                                        lblfee.Text = Convert.ToString((feeamt + lastfeeamt) - addfeeamt);
                                        // (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = "0";
                                        // (gvpopro.Cells[5].FindControl("txtfeemat") as TextBox).Text = "0";
                                        totamnt = Convert.ToDouble(lblfee.Text) - concess;
                                        tot.Text = Convert.ToString(totamnt);
                                        balamt.Text = Convert.ToString(totamnt - paidamt);
                                    }
                                    else
                                    {
                                        addtxtfeeamt.Text = "0.00";
                                        lblfee.Text = Convert.ToString(feeamt + lastfeeamt);
                                        (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = "0";
                                        (gvpopro.Cells[5].FindControl("txtfeemat") as TextBox).Text = "0";
                                        totamnt = Convert.ToDouble(lblfee.Text) - concess;
                                        tot.Text = Convert.ToString(totamnt);
                                        balamt.Text = Convert.ToString(totamnt - paidamt);
                                        alertpopwindow.Visible = true;
                                        lblalerterr.Visible = true;
                                        lblalerterr.Text = "Deduct FeeAmount/Balance Amount should be less than Total Fee Amount!";
                                    }
                                }
                            }
                            else
                            {
                                (gvpopro.Cells[6].FindControl("tmptxtamt") as Label).Text = "0";
                                (gvpopro.Cells[5].FindControl("txtfeemat") as TextBox).Text = "0";
                                double oldfeeval = Convert.ToDouble(lblfee.Text);
                                lblfee.Text = Convert.ToString(oldfeeval + lastfeeamt);
                                totamnt = Convert.ToDouble(lblfee.Text) - concess;
                                tot.Text = totamnt.ToString();
                                balamt.Text = Convert.ToString(totamnt - paidamt);
                            }
                        }
                    }
                    rowcnt++;
                }

            }
        }
        catch { }
    }

    protected void txt_dueext1_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dsdate = new DateTime();
            string app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                Label hdrfk = (Label)ro.Cells[2].FindControl("lbl_hdridpop");
                Label ledgefk = (Label)ro.Cells[3].FindControl("lbl_lgrid");
                Label feeid = (Label)ro.Cells[1].FindControl("lbl_feecode");

                string selq = "select top 1 convert(varchar(10),ExtDueDate,103) as ExtDueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFK='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' order by ExtDueDate desc";
                string gotdue = d2.GetFunction(selq);
                string[] spl = gotdue.Split('/');
                dsdate = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                TextBox txtdue = (TextBox)ro.Cells[10].FindControl("txt_dueext1");
                string due = txtdue.Text;
                duespl = due.Split('/');
                dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                if (dueext < dsdate)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Due Extention should be greater than Due Date!";
                    txtdue.Text = dsdate.ToString("dd/MM/yyyy");
                }
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess1_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            double cons1 = 0.00;
            double bal = 0.00;
            if (txt_datewithamnt.Text == "1")
            {
                foreach (GridViewRow gvro in gridViewpop.Rows)
                {
                    TextBox txtaddcons1 = (TextBox)gvro.Cells[10].FindControl("txt_addconcess1");
                    Label balamnt = (Label)gvro.Cells[8].FindControl("lbl_balamnt");
                    if (txtaddcons1.Text.Trim() != "")
                    {
                        cons1 = Convert.ToDouble(txtaddcons1.Text);
                        if (balamnt.Text.Trim() != "")
                        {
                            bal = Convert.ToDouble(balamnt.Text);
                            if (cons1 > bal || cons1 < bal)
                            {
                                txtaddcons1.Text = "";
                            }
                            else
                            {
                                txtaddcons1.Text = Convert.ToString(cons1);
                            }
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void txt_dueext2_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[10].FindControl("txt_dueext1");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[12].FindControl("txt_dueext2");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due2 should be greater than the Due1!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess2_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            //string CtrlID = string.Empty;
            int rowid = 0;
            //if (Request.Form["__EVENTTARGET"] != null && Request.Form["__EVENTTARGET"] != string.Empty)
            //{
            //    CtrlID = Request.Form["__EVENTTARGET"];
            //    string row = CtrlID.Split('$')[1];
            //    row = row.Replace("ctl", "");
            //    rowid = Convert.ToInt32(row);
            //}

            double cons1 = 0.00;
            double cons2 = 0.00;
            double totcons = 0.00;
            double bal = 0.00;
            //TextBox txtaddcons1 = (TextBox)gridViewpop.Rows[rowid - 1].Cells[10].FindControl("txt_addconcess1");
            //TextBox txtaddcons2 = (TextBox)rowid.Cells[12].FindControl("txt_addconcess2");
            //Label balamnt = (Label)rowid.Cells[8].FindControl("lbl_balamnt");
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow gvro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtaddcons1 = (TextBox)gvro.Cells[10].FindControl("txt_addconcess1");
                    TextBox txtaddcons2 = (TextBox)gvro.Cells[12].FindControl("txt_addconcess2");
                    Label balamnt = (Label)gvro.Cells[8].FindControl("lbl_balamnt");
                    if (txtaddcons1.Text == "")
                    {
                        txtaddcons1.Text = "0.00";
                    }
                    if (txtaddcons2.Text.Trim() != "")
                    {
                        cons1 = Convert.ToDouble(txtaddcons1.Text);
                        cons2 = Convert.ToDouble(txtaddcons2.Text);
                        totcons = cons1 + cons2;
                        if (balamnt.Text.Trim() != "")
                        {
                            bal = Convert.ToDouble(balamnt.Text);
                        }
                    }
                    if (txt_datewithamnt.Text == "2")
                    {
                        if (txtaddcons1.Text.Trim() != "")
                        {
                            if (totcons > bal || totcons < bal)
                            {
                                txtaddcons2.Text = "";
                            }
                            else
                            {
                                txtaddcons2.Text = Convert.ToString(cons2);
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "Please Enter the Amount1!";
                            txtaddcons2.Text = "";
                        }
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_dueext3_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[12].FindControl("txt_dueext2");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[14].FindControl("txt_dueext3");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due3 should be greater than the Due2!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess3_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext4_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[14].FindControl("txt_dueext3");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[16].FindControl("txt_dueext4");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due4 should be greater than the Due3!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess4_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext5_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[16].FindControl("txt_dueext4");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[18].FindControl("txt_dueext5");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due5 should be greater than the Due4!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess5_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext6_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[18].FindControl("txt_dueext5");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[20].FindControl("txt_dueext6");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due6 should be greater than the Due5!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess6_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext7_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[20].FindControl("txt_dueext6");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[22].FindControl("txt_dueext7");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due7 should be greater than the Due6!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess7_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext8_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[22].FindControl("txt_dueext7");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[24].FindControl("txt_dueext8");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due8 should be greater than the Due7!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess8_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext9_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[24].FindControl("txt_dueext8");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[26].FindControl("txt_dueext9");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due9 should be greater than the Due8!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess9_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void txt_dueext10_OnTextChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime dueext = new DateTime();
            DateTime dueext1 = new DateTime();
            int rowindex = rowIndxClicked();
            int rowcnt = 0;
            foreach (GridViewRow ro in gridViewpop.Rows)
            {
                if (rowindex == rowcnt)
                {
                    TextBox txtdue = (TextBox)ro.Cells[26].FindControl("txt_dueext9");
                    string due = txtdue.Text;
                    duespl = due.Split('/');
                    dueext = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    TextBox txtdue1 = (TextBox)ro.Cells[28].FindControl("txt_dueext10");
                    string due1 = txtdue1.Text;
                    duespl = due1.Split('/');
                    dueext1 = Convert.ToDateTime(duespl[1] + "/" + duespl[0] + "/" + duespl[2]);
                    if (dueext > dueext1)
                    {
                        txtdue1.Text = due;
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "The Due10 should be greater than the Due9!";
                        return;
                    }
                    else
                    {
                        txtdue1.Text = due1;
                    }
                }
                rowcnt++;
            }
        }
        catch
        {

        }
    }

    protected void txt_addconcess10_OnTextChanged(object sender, EventArgs e)
    {

    }

    protected void cb_datewithamnt_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_datewithamnt.Checked == true)
            {
                txt_datewithamnt.Visible = true;
                txt_datewithamnt.Text = "";
                cbcons.Checked = false;
                cb_concesstbl.Checked = false;
                cbconsdecrs.Checked = false;
                cbfeeamt.Checked = false;
                cbfeeamtadd.Checked = false;
                cbfeeamtdecrs.Checked = false;

                cb_concesstbl.Enabled = false;
                cbconsdecrs.Enabled = false;
                cbfeeamtadd.Enabled = false;
                cbfeeamtdecrs.Enabled = false;
            }
            else
            {
                txt_datewithamnt.Visible = false;
                txt_datewithamnt.Text = "";
            }
        }
        catch
        {

        }
    }

    protected void cb_comdt_Changed(object sender, EventArgs e)
    {
        if (cb_comdt.Checked == true)
        {
            txt_comdt.Enabled = true;
            cbcons.Checked = false;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
            cbfeeamt.Checked = false;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;

            cb_concesstbl.Enabled = false;
            cbconsdecrs.Enabled = false;
            cbfeeamtadd.Enabled = false;
            cbfeeamtdecrs.Enabled = false;
        }
        else
        {
            txt_comdt.Enabled = false;
        }
    }

    public string GetAppNo()
    {
        string MyAppNo = string.Empty;
        try
        {
            switch (Convert.ToInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    MyAppNo = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "'  and college_code='" + collegecode1 + "'");
                    break;
                case 1:
                    MyAppNo = d2.GetFunction("select app_no from Registration where Reg_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    break;
                case 2:
                    MyAppNo = d2.GetFunction("select app_no from Registration where Roll_admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    break;
                case 3:
                    MyAppNo = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    break;
            }
        }
        catch { }
        return MyAppNo;
    }

    public void bindspread()
    {
        try
        {
            string feeallopk = string.Empty;
            string sysDate = string.Empty;
            string feecato = "";
            string app_no = "";
            DataView dvnew = new DataView();

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    if (feecato.Trim() == "")
                    {
                        feecato = "" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        feecato = feecato + "'" + "," + "'" + cbl_sem.Items[i].Value.ToString() + "";
                    }
                }
            }

            if (feecato.Trim() != "")
            {
                string selectQ = "";
                string paid = "";

                app_no = GetAppNo();
                string finyearfk = Convert.ToString(ddlfinyear.SelectedValue);
                if (app_no != "" && app_no != "0")
                {
                    selectQ = " select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,isnull(DeductAmout,'0') as DeductAmout,DeductReason,TotalAmount,RefundAmount,PaidAmount,BalAmount,FromGovtAmt,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate,app_no,finyearfk from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No ='" + app_no + "' and m.CollegeCode='" + collegecode1 + "' and FeeCategory in('" + feecato + "') and f.finyearfk in('" + finyearfk + "')";
                    if (cb_showall.Checked == false)
                    {
                        selectQ = selectQ + " and BalAmount <>0";
                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQ, "Text");

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount = 0;
                            FpSpread1.Sheets[0].ColumnCount = 0;
                            FpSpread1.CommandBar.Visible = false;
                            FpSpread1.Sheets[0].AutoPostBack = false;
                            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                            FpSpread1.Sheets[0].RowHeader.Visible = false;
                            FpSpread1.Sheets[0].ColumnCount = 10;

                            FarPoint.Web.Spread.TextCellType txtcell = new FarPoint.Web.Spread.TextCellType();

                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.Black;
                            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[0].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            // FpSpread1.Sheets[0].Columns[0].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = lbl_sem.Text;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[2].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Header";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[3].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Ledger";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[4].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Fee Amount";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[5].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Concession/Deduction";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[6].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Total Amount";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[7].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Paid";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[8].Locked = true;

                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Balance";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Border.BorderColor = System.Drawing.Color.Black;
                            //FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Border.BorderColorTop = System.Drawing.Color.Black;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Columns[9].Locked = true;

                            FarPoint.Web.Spread.CheckBoxCellType chkbox = new FarPoint.Web.Spread.CheckBoxCellType();
                            chkbox.AutoPostBack = false;

                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                                string hdrfk = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                                string ledgerfk = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                                string appno = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
                                string finyrfk = Convert.ToString(ds.Tables[0].Rows[row]["finyearfk"]);
                                string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + collegecode1 + "");
                                string deg = d2.GetFunction("select degree_code from registration where  college_code=" + collegecode1 + " and app_no='" + app_no + "'");

                                FpSpread1.Sheets[0].RowCount++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(cursem);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = feecat;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["LedgerPk"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["FeeAmount"]), 0));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Note = appno;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["DeductAmout"]), 0));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Note = finyrfk;

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]), 0));
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                if (Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]) == "")
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "0.00";
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]), 0));
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]), 0));
                                double bal = Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Border.BorderColor = System.Drawing.Color.Black;
                                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Border.BorderColorTop = System.Drawing.Color.Black;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                                string selq = "select App_No,HeaderFK,LedgerFK,DueAmount,DueDate,ExtDueAmount,convert(varchar(10),ExtDueDate,103) as ExtDueDate,ExtReason,FinYearFK,DeductAmount,UserCode,FeeCategory,Extention_count from FeesDueExt where App_No='" + app_no + "'";
                                selq = selq + " select distinct extention_count  from FeesDueExt where App_No='" + app_no + "'";
                                DataSet dsnew = new DataSet();
                                dsnew.Clear();
                                dsnew = d2.select_method_wo_parameter(selq, "Text");
                                if (dsnew.Tables[1].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsnew.Tables[1].Rows.Count; i++)
                                    {
                                        dsnew.Tables[0].DefaultView.RowFilter = " FeeCategory = '" + feecat.ToString() + "' AND HeaderFK = '" + hdrfk.ToString() + "' AND LedgerFK = '" + ledgerfk.ToString() + "' and  extention_count ='" + Convert.ToString(dsnew.Tables[1].Rows[i]["extention_count"]) + "'";
                                        dvnew = dsnew.Tables[0].DefaultView;
                                        if (dvnew.Count > 0)
                                        {
                                            for (int ik = 0; ik < dvnew.Count; ik++)
                                            {
                                                FpSpread1.Sheets[0].RowCount++;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ik + 1);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, 1);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txtcell;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dvnew[ik]["ExtDueDate"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                //FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1,2, 1, 3);

                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvnew[ik]["ExtDueAmount"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 5, 1, 5);
                                            }
                                        }
                                    }
                                }


                                #region Fine Type for College -- Common - 0; everyledger - 1
                                int fineFeesType = 0;
                                int.TryParse(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='FineFeesType' and college_code=" + collegecode1 + ""), out fineFeesType);
                                bool fineAdded = false;
                                bool ReaddfineAdded = false;
                                bool monthlyfineadded = false;
                                bool boolSchool = false;
                                double fineAmount = 0;
                                double readfineamount = 0;
                                #endregion
                                Dictionary<string, double> dtfintFeecat = new Dictionary<string, double>();
                                Dictionary<string, double> dtrefintFeecat = new Dictionary<string, double>();
                                Dictionary<string, string> dtfinfk = new Dictionary<string, string>();
                                Dictionary<string, string> dtfinfkRe = new Dictionary<string, string>();
                                Dictionary<string, string> dtfeecat = new Dictionary<string, string>();
                                ArrayList arFeecatNEw = new ArrayList();
                                ArrayList arFeecatReAd = new ArrayList();
                                string batchYear = txt_rebatch.Text;
                                if (!boolSchool)//college fine
                                {
                                    #region college
                                    // string feevalue = Convert.ToString(ds.Tables[0].Rows[i]["TextVal"]);
                                    if (!dtfeecat.ContainsKey(feecat))
                                    {
                                        dtfeecat.Add(feecat, cursem);
                                    }
                                    bool boolFine = false;
                                    if (bal != 0)
                                        boolFine = true;

                                    if (!fineAdded)
                                    {
                                        sysDate = DateTime.Now.ToString("dd/MM/yyyy");
                                        #region Fine Calculation
                                        if (boolFine)
                                        {
                                            //Added by saranya on 4April2018
                                            string FineCancel = "select * from FT_FineCancelSetting where app_no='" + app_no + "' and FeeCategory in('" + feecat + "')";
                                            DataSet dsfinecnl = new DataSet();
                                            dsfinecnl = d2.select_method_wo_parameter(FineCancel, "Text");
                                            if (dsfinecnl.Tables[0].Rows.Count > 0)
                                            {

                                            }
                                            //==========================//
                                            else
                                            {
                                                string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode1 + " and DegreeCode='" + deg + "' and H.Headerpk ='" + hdrfk.ToString() + "' and L.LedgerPK='" + ledgerfk.ToString() + "' and FeeCatgory in ('" + feecat + "') and isnull(FineSettingType,0)='0' and BatchYear='" + batchYear + "' select distinct holiday_date from holidaystudents where degree_code=" + deg + "";//and f.finyearfk in('" + finyearfK + "') 

                                                DataSet dsFine = new DataSet();
                                                dsFine = d2.select_method_wo_parameter(fineQ, "Text");

                                                if (dsFine.Tables.Count > 0)
                                                {
                                                    if (dsFine.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                                        {
                                                            string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                            string finefinfk = Convert.ToString(dsFine.Tables[0].Rows[fn]["finyearfk"]);
                                                            DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                                            // DateTime curDate = DateTime.Now.Date;
                                                            string recptDt = sysDate.Split('/')[1] + "/" + sysDate.Split('/')[0] + "/" + sysDate.Split('/')[2];
                                                            double tempPaid = 0;
                                                            double.TryParse(Convert.ToString(d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where App_No='" + app_no + "' and headerfk='" + hdrfk.ToString() + "' and ledgerfk='" + ledgerfk.ToString() + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'")), out tempPaid);
                                                            //  string paidamount = d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'");

                                                            DateTime curDate = Convert.ToDateTime(recptDt);
                                                            #region Check for Due Extension
                                                            DateTime dueExt = due;
                                                            string dueDtstring = "select  max(ExtDueDate) as DueDate from FeesDueExt where App_No='" + app_no + "' and  FeeCategory='" + feecat + "' and HeaderFK='" + hdrfk.ToString() + "' and LedgerFK='" + ledgerfk.ToString() + "'";
                                                            try
                                                            {
                                                                dueExt = Convert.ToDateTime(d2.GetFunction(dueDtstring).Trim());
                                                            }
                                                            catch { dueExt = due; }
                                                            // DateTime.TryParse(d2.GetFunction(dueDtstring).Trim(), out dueExt);
                                                            due = dueExt;
                                                            #endregion

                                                            if (tempPaid == 0)
                                                            {
                                                                if (fineType == "1")
                                                                {
                                                                    //common - No holiday
                                                                    if (due < curDate)
                                                                    {
                                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                }
                                                                else if (fineType == "2")
                                                                {
                                                                    //Per day - 
                                                                    for (; due < curDate; due = due.AddDays(1))
                                                                    {
                                                                        bool addFine = true;
                                                                        if (dsFine.Tables.Count > 1 && dsFine.Tables[1].Rows.Count > 0)
                                                                        {
                                                                            dsFine.Tables[1].DefaultView.RowFilter = " holiday_date ='" + due + "'";
                                                                            DataView dvFinDt = dsFine.Tables[1].DefaultView;
                                                                            if (dvFinDt.Count > 0)
                                                                                addFine = false;
                                                                        }

                                                                        if (addFine)
                                                                            fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                }
                                                                else if (fineType == "3")
                                                                {
                                                                    //Per week
                                                                    TimeSpan td = curDate - due;
                                                                    int difference = td.Days;
                                                                    int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                                                    int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                                                    if (difference <= to_day && difference >= fromday)
                                                                    {
                                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                    if (!dtfinfk.ContainsKey(feecat))
                                                                        dtfinfk.Add(feecat, finefinfk);
                                                                }
                                                            }

                                                        }
                                                        if (fineAmount > 0 && fineFeesType == 0)
                                                        {
                                                            if (!dtfintFeecat.ContainsKey(feecat))
                                                                dtfintFeecat.Add(feecat, fineAmount);
                                                            else
                                                                dtfintFeecat[feecat] += fineAmount;
                                                            //fineAdded = true;
                                                            fineAmount = 0;
                                                            if (!arFeecatNEw.Contains(feecat))
                                                                arFeecatNEw.Add(feecat);
                                                        }
                                                    }
                                                }
                                                if (fineAmount > 0 && fineFeesType == 0)
                                                {
                                                    fineAdded = true;
                                                }
                                                if (fineAmount > 0 && fineFeesType == 1)
                                                {
                                                    if (!dtfintFeecat.ContainsKey(feecat))
                                                        dtfintFeecat.Add(feecat, fineAmount);
                                                    //else
                                                    //    dtfintFeecat[feecat] += fineAmount;
                                                    //fineAdded = true;
                                                    fineAmount = 0;
                                                    if (!arFeecatNEw.Contains(feecat))
                                                        arFeecatNEw.Add(feecat);
                                                }
                                            }
                                        }
                                        #endregion
                                        #region fine Calculation for perday wise based balance amount
                                        #region Fine Calculation
                                        if (boolFine)
                                        {
                                            //Added by saranya on 4April2018
                                            string FineCancel = "select * from FT_FineCancelSetting where app_no='" + app_no + "' and FeeCategory in('" + feecat + "')";
                                            DataSet dsfinecnl = new DataSet();
                                            dsfinecnl = d2.select_method_wo_parameter(FineCancel, "Text");
                                            if (dsfinecnl.Tables[0].Rows.Count > 0)
                                            {

                                            }
                                            //==========================//
                                            else
                                            {
                                                string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode1 + " and DegreeCode='" + deg + "' and H.Headerpk ='" + hdrfk + "' and L.LedgerPK='" + ledgerfk + "' and FeeCatgory in ('" + feecat + "') and isnull(FineSettingType,0)='3' and BatchYear='" + batchYear + "' select distinct holiday_date from holidaystudents where degree_code=" + deg + "";//and f.finyearfk in('" + finyearfK + "') 

                                                DataSet dsFine = new DataSet();
                                                dsFine = d2.select_method_wo_parameter(fineQ, "Text");

                                                if (dsFine.Tables.Count > 0)
                                                {
                                                    if (dsFine.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                                        {
                                                            string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                            string finefinfk = Convert.ToString(dsFine.Tables[0].Rows[fn]["finyearfk"]);
                                                            DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                                            // DateTime curDate = DateTime.Now.Date;
                                                            string recptDt = sysDate.Split('/')[1] + "/" + sysDate.Split('/')[0] + "/" + sysDate.Split('/')[2];
                                                            double tempPaid = 0;
                                                            double.TryParse(Convert.ToString(d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where App_No='" + app_no + "' and headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'")), out tempPaid);
                                                            //  string paidamount = d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'");

                                                            DateTime curDate = Convert.ToDateTime(recptDt);
                                                            #region Check for Due Extension
                                                            DateTime dueExt = due;
                                                            string dueDtstring = "select  max(ExtDueDate) as DueDate from FeesDueExt where App_No='" + app_no + "' and  FeeCategory='" + feecat + "' and HeaderFK='" + hdrfk + "' and LedgerFK='" + ledgerfk + "'";
                                                            try
                                                            {
                                                                dueExt = Convert.ToDateTime(d2.GetFunction(dueDtstring).Trim());
                                                            }
                                                            catch { dueExt = due; }
                                                            // DateTime.TryParse(d2.GetFunction(dueDtstring).Trim(), out dueExt);
                                                            due = dueExt;
                                                            #endregion

                                                            if (tempPaid == 0)
                                                            {
                                                                if (fineType == "1")
                                                                {
                                                                    //common - No holiday
                                                                    if (due < curDate)
                                                                    {
                                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                }
                                                                else if (fineType == "2")
                                                                {
                                                                    //Per day - 
                                                                    for (; due < curDate; due = due.AddDays(1))
                                                                    {
                                                                        bool addFine = true;
                                                                        if (dsFine.Tables.Count > 1 && dsFine.Tables[1].Rows.Count > 0)
                                                                        {
                                                                            dsFine.Tables[1].DefaultView.RowFilter = " holiday_date ='" + due + "'";
                                                                            DataView dvFinDt = dsFine.Tables[1].DefaultView;
                                                                            if (dvFinDt.Count > 0)
                                                                                addFine = false;
                                                                        }
                                                                        double balfine = 0;
                                                                        double balsetfine = 0;
                                                                        //fine per day with header and ledger
                                                                        string Balamountfine = d2.GetFunction("select balamount from ft_feeallot where app_no='" + app_no + "' and headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "'");
                                                                        string balamountsetfine = d2.GetFunction("select amount from ft_perdayindividualsetting where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "'");
                                                                        double.TryParse(Balamountfine, out balfine);
                                                                        double.TryParse(balamountsetfine, out balsetfine);
                                                                        if (balsetfine < balfine)
                                                                        {
                                                                            if (addFine)
                                                                                fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                        }
                                                                    }
                                                                }
                                                                else if (fineType == "3")
                                                                {
                                                                    //Per week
                                                                    TimeSpan td = curDate - due;
                                                                    int difference = td.Days;
                                                                    int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                                                    int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                                                    if (difference <= to_day && difference >= fromday)
                                                                    {
                                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                    if (!dtfinfk.ContainsKey(feecat))
                                                                        dtfinfk.Add(feecat, finefinfk);
                                                                }
                                                            }

                                                        }
                                                        if (fineAmount > 0 && fineFeesType == 0)
                                                        {
                                                            if (!dtfintFeecat.ContainsKey(feecat))
                                                                dtfintFeecat.Add(feecat, fineAmount);
                                                            else
                                                                dtfintFeecat[feecat] += fineAmount;
                                                            //fineAdded = true;
                                                            fineAmount = 0;
                                                            if (!arFeecatNEw.Contains(feecat))
                                                                arFeecatNEw.Add(feecat);
                                                        }
                                                    }
                                                }
                                                if (fineAmount > 0 && fineFeesType == 0)
                                                {
                                                    fineAdded = true;
                                                }
                                                if (fineAmount > 0 && fineFeesType == 1)
                                                {
                                                    if (!dtfintFeecat.ContainsKey(feecat))
                                                        dtfintFeecat.Add(feecat, fineAmount);
                                                    //else
                                                    //    dtfintFeecat[feecat] += fineAmount;
                                                    //fineAdded = true;
                                                    fineAmount = 0;
                                                    if (!arFeecatNEw.Contains(feecat))
                                                        arFeecatNEw.Add(feecat);
                                                }
                                            }
                                        }
                                        #endregion

                                        #endregion
                                    }
                                    //Re-admission fees settings
                                    if (!ReaddfineAdded)
                                    {
                                        #region Fine Calculation
                                        if (boolFine)
                                        {
                                            string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode1 + " and DegreeCode='" + ledgerfk + "' and H.Headerpk ='" + hdrfk + "' and L.LedgerPK='" + ledgerfk + "' and FeeCatgory in ('" + feecat + "') and isnull(FineSettingType,0)='1' and BatchYear='" + batchYear + "' select distinct holiday_date from holidaystudents where degree_code=" + ledgerfk + "";//f.finyearfk in('" + finyearfK + "')

                                            DataSet dsFine = new DataSet();
                                            dsFine = d2.select_method_wo_parameter(fineQ, "Text");
                                            if (dsFine.Tables.Count > 0)
                                            {
                                                if (dsFine.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                                    {
                                                        string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                        string finefinfk = Convert.ToString(dsFine.Tables[0].Rows[fn]["finyearfk"]);
                                                        DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                                        //  DateTime curDate = DateTime.Now.Date;
                                                        string recptDt = sysDate.Split('/')[1] + "/" + sysDate.Split('/')[0] + "/" + sysDate.Split('/')[2];
                                                        //  string paidamount = d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'");
                                                        double tempPaid = 0;
                                                        double.TryParse(Convert.ToString(d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where App_No='" + app_no + "' and  headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'")), out tempPaid);
                                                        DateTime curDate = Convert.ToDateTime(recptDt);
                                                        #region Check for Due Extension
                                                        DateTime dueExt = due;
                                                        string dueDtstring = "select  max(ExtDueDate) as DueDate from FeesDueExt where App_No='" + app_no + "' and  FeeCategory='" + feecat + "' and HeaderFK='" + hdrfk + "' and LedgerFK='" + ledgerfk + "'";
                                                        try
                                                        {
                                                            dueExt = Convert.ToDateTime(d2.GetFunction(dueDtstring).Trim());
                                                        }
                                                        catch { dueExt = due; }
                                                        // DateTime.TryParse(d2.GetFunction(dueDtstring).Trim(), out dueExt);
                                                        due = dueExt;
                                                        #endregion

                                                        if (tempPaid == 0)
                                                        {
                                                            if (fineType == "1")
                                                            {
                                                                //common - No holiday
                                                                if (due < curDate)
                                                                {
                                                                    readfineamount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                }
                                                            }
                                                            if (!dtfinfkRe.ContainsKey(feecat))
                                                                dtfinfkRe.Add(feecat, finefinfk);
                                                        }
                                                        #region other setting

                                                        //else if (fineType == "2")
                                                        //{
                                                        //    //Per day - 
                                                        //    for (; due < curDate; due = due.AddDays(1))
                                                        //    {
                                                        //        bool addFine = true;
                                                        //        if (dsFine.Tables.Count > 1 && dsFine.Tables[1].Rows.Count > 0)
                                                        //        {
                                                        //            dsFine.Tables[1].DefaultView.RowFilter = " holiday_date ='" + due + "'";
                                                        //            DataView dvFinDt = dsFine.Tables[1].DefaultView;
                                                        //            if (dvFinDt.Count > 0)
                                                        //                addFine = false;
                                                        //        }
                                                        //        if (addFine)
                                                        //            readfineamount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                        //    }
                                                        //}
                                                        //else if (fineType == "3")
                                                        //{
                                                        //    //Per week
                                                        //    TimeSpan td = curDate - due;
                                                        //    int difference = td.Days;
                                                        //    int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                                        //    int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                                        //    if (difference <= to_day && difference >= fromday)
                                                        //    {
                                                        //        readfineamount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                        //    }
                                                        //}
                                                        #endregion
                                                    }
                                                    if (readfineamount > 0 && fineFeesType == 0)
                                                    {
                                                        ReaddfineAdded = true;
                                                        if (!dtrefintFeecat.ContainsKey(feecat))
                                                            dtrefintFeecat.Add(feecat, readfineamount);
                                                        else
                                                            dtrefintFeecat[feecat] += readfineamount;
                                                        readfineamount = 0;
                                                        if (!arFeecatReAd.Contains(feecat))
                                                            arFeecatReAd.Add(feecat);
                                                    }
                                                }
                                            }
                                            if (readfineamount > 0 && fineFeesType == 0)
                                            {
                                                ReaddfineAdded = true;
                                            }
                                        }
                                        if (readfineamount > 0 && fineFeesType == 1)
                                        {
                                            ReaddfineAdded = true;
                                            if (!dtrefintFeecat.ContainsKey(feecat))
                                                dtrefintFeecat.Add(feecat, readfineamount);
                                            else
                                                dtrefintFeecat[feecat] += readfineamount;
                                            readfineamount = 0;
                                            if (!arFeecatReAd.Contains(feecat))
                                                arFeecatReAd.Add(feecat);
                                        }
                                        #endregion
                                    }
                                    if (!monthlyfineadded)//ABARNA
                                    {
                                        #region Fine Calculation
                                        if (boolFine)
                                        {
                                            //Added by saranya on 4April2018
                                            string FineCancel = "select * from FT_FineCancelSetting where app_no='" + app_no + "' and FeeCategory in('" + feecat + "')";
                                            DataSet dsfinecnl = new DataSet();
                                            dsfinecnl = d2.select_method_wo_parameter(FineCancel, "Text");
                                            if (dsfinecnl.Tables[0].Rows.Count > 0)
                                            {

                                            }
                                            //==========================//
                                            else
                                            {
                                                string fineQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as FineAmt, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType,finemonth from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode1 + " and DegreeCode='" + ledgerfk + "' and H.Headerpk ='" + hdrfk + "' and L.LedgerPK='" + ledgerfk + "' and FeeCatgory in ('" + feecat + "') and isnull(FineSettingType,0)='2' and BatchYear='" + batchYear + "' select distinct holiday_date from holidaystudents where degree_code=" + ledgerfk + "";//and f.finyearfk in('" + finyearfK + "') 

                                                DataSet dsFine = new DataSet();

                                                dsFine = d2.select_method_wo_parameter(fineQ, "Text");
                                                //string finemonth = string.Empty;
                                                int finemonth = 0;
                                                if (dsFine.Tables.Count > 0)
                                                {
                                                    if (dsFine.Tables[0].Rows.Count > 0)
                                                    {
                                                        for (int fn = 0; fn < dsFine.Tables[0].Rows.Count; fn++)
                                                        {
                                                            finemonth = Convert.ToInt16(dsFine.Tables[0].Rows[fn]["finemonth"]);
                                                            string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                            string finefinfk = Convert.ToString(dsFine.Tables[0].Rows[fn]["finyearfk"]);
                                                            DateTime due = Convert.ToDateTime(dsFine.Tables[0].Rows[fn]["DueDate"]);
                                                            // DateTime curDate = DateTime.Now.Date;
                                                            string recptDt = sysDate.Split('/')[1] + "/" + sysDate.Split('/')[0] + "/" + sysDate.Split('/')[2];
                                                            double tempPaid = 0;
                                                            double.TryParse(Convert.ToString(d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where App_No='" + app_no + "' and headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'")), out tempPaid);
                                                            //  string paidamount = d2.GetFunction("select sum(debit) as debit from ft_findailytransaction where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory in ('" + feecat + "') and transdate>'" + due + "'");

                                                            DateTime curDate = Convert.ToDateTime(recptDt);
                                                            #region Check for Due Extension
                                                            DateTime dueExt = due;
                                                            string dueDtstring = "select  max(ExtDueDate) as DueDate from FeesDueExt where App_No='" + app_no + "' and  FeeCategory='" + feecat + "' and HeaderFK='" + hdrfk + "' and LedgerFK='" + ledgerfk + "'";
                                                            try
                                                            {
                                                                dueExt = Convert.ToDateTime(d2.GetFunction(dueDtstring).Trim());
                                                            }
                                                            catch { dueExt = due; }
                                                            // DateTime.TryParse(d2.GetFunction(dueDtstring).Trim(), out dueExt);
                                                            due = dueExt;
                                                            #endregion

                                                            if (tempPaid == 0)
                                                            {
                                                                if (fineType == "1")
                                                                {
                                                                    //common - No holiday
                                                                    if (due < curDate)
                                                                    {
                                                                        double allotmonth = Convert.ToDouble(d2.GetFunction("select allotmonth from ft_feeallotmonthly where balamount>0 and feeallotpk='" + feeallopk + "'"));
                                                                        if (allotmonth == finemonth)
                                                                        {
                                                                            fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                        }
                                                                    }
                                                                }
                                                                else if (fineType == "2")
                                                                {
                                                                    //Per day - 
                                                                    for (; due < curDate; due = due.AddDays(1))
                                                                    {
                                                                        bool addFine = true;
                                                                        if (dsFine.Tables.Count > 1 && dsFine.Tables[1].Rows.Count > 0)
                                                                        {
                                                                            dsFine.Tables[1].DefaultView.RowFilter = " holiday_date ='" + due + "'";
                                                                            DataView dvFinDt = dsFine.Tables[1].DefaultView;
                                                                            if (dvFinDt.Count > 0)
                                                                                addFine = false;
                                                                        }
                                                                        if (addFine)
                                                                            fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);
                                                                    }
                                                                }
                                                                else if (fineType == "3")
                                                                {
                                                                    //Per week
                                                                    TimeSpan td = curDate - due;
                                                                    int difference = td.Days;
                                                                    int fromday = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["FromDay"]);
                                                                    int to_day = Convert.ToInt32(dsFine.Tables[0].Rows[fn]["ToDay"]);

                                                                    if (difference <= to_day && difference >= fromday)
                                                                    {
                                                                        fineAmount += Convert.ToDouble(dsFine.Tables[0].Rows[fn]["FineAmt"]);

                                                                    }
                                                                    if (!dtfinfk.ContainsKey(feecat))
                                                                        dtfinfk.Add(feecat, finefinfk);
                                                                }
                                                            }

                                                        }
                                                        if (fineAmount > 0 && fineFeesType == 0)//abarna
                                                        {
                                                            if (!dtfintFeecat.ContainsKey(feecat + "," + finemonth + "," + ledgerfk))
                                                                dtfintFeecat.Add(feecat + "," + finemonth + "," + ledgerfk, fineAmount);
                                                            //else
                                                            //    dtfintFeecat[feecat] += fineAmount;
                                                            //fineAdded = true;
                                                            fineAmount = 0;
                                                            if (!arFeecatNEw.Contains(feecat))
                                                                arFeecatNEw.Add(feecat);
                                                            //if (!dtmonth.ContainsKey(feecat))//abarna
                                                            //    dtmonth.Add(feecat,finemonth);
                                                            //else
                                                            //    dtmonth[feecat] += finemonth;
                                                        }
                                                    }
                                                }
                                                if (fineAmount > 0 && fineFeesType == 0)
                                                {
                                                    fineAdded = true;
                                                }
                                                if (fineAmount > 0 && fineFeesType == 1)
                                                {
                                                    if (!dtfintFeecat.ContainsKey(feecat))
                                                        dtfintFeecat.Add(feecat, fineAmount);
                                                    //else
                                                    //    dtfintFeecat[feecat] += fineAmount;
                                                    //fineAdded = true;
                                                    fineAmount = 0;
                                                    if (!arFeecatNEw.Contains(feecat))
                                                        arFeecatNEw.Add(feecat);
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                if (!boolSchool)//college fine added
                                {
                                    #region college


                                    #region Fine Adjustment
                                    double ovrAllBalAmt = 0;
                                    try
                                    {
                                        //DataTable table;
                                        //table = tbl_Student;

                                        //// Declare an object variable.
                                        //object sumObject;
                                        //sumObject = table.Compute("Sum(BalAmt)", "");
                                        //double.TryParse(sumObject.ToString(), out ovrAllBalAmt);
                                        //for (int bal = 0; bal < ds.Rows.Count; bal++)
                                        //{
                                        //    string balAmti = Convert.ToString(tbl_Student.Rows[bal]["BalAmt"]).Trim();
                                        //    if (balAmti != "")
                                        //    {
                                        //        ovrAllBalAmt += Convert.ToDouble(balAmti);
                                        //    }
                                        //}
                                    }
                                    catch { ovrAllBalAmt = 0; }
                                    string name = string.Empty;

                                    if (dtfintFeecat.Count > 0)//ovrAllBalAmt > 0 &&
                                    {
                                        string query = "select ledgerfk,feecategory from ft_feeallot where balamount>0 and app_no='" + app_no + "' and headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory='" + feecat + "'";
                                        DataSet s = d2.select_method_wo_parameter(query, "text");
                                        for (int j = 0; j < s.Tables[0].Rows.Count; j++)
                                        {
                                            ledgerfk = Convert.ToString(s.Tables[0].Rows[j]["ledgerfk"]);
                                            string feecategory = Convert.ToString(s.Tables[0].Rows[j]["feecategory"]);
                                            string linkName = batchYear + "-" + deg + "-" + "FineLedgerValue" + "-" + ledgerfk;
                                            //   string fineLegHedQ = d2.GetFunction(" select Linkvalue from New_InsSettings where LinkName='" + linkName + "' and user_code ='" + usercode + "' and college_code  in (" + collegecode1 + ")");
                                            string fineLegHedQ = " select distinct Linkvalue from New_InsSettings where LinkName='" + linkName + "' and user_code ='" + usercode + "' and college_code  in (" + collegecode1 + ")";

                                            DataSet setfine = d2.select_method_wo_parameter(fineLegHedQ, "text");
                                            for (int k = 0; k < setfine.Tables[0].Rows.Count; k++)
                                            {
                                                if (fineLegHedQ != "0" && dtfintFeecat.Count > 0)
                                                {
                                                    string head = Convert.ToString(setfine.Tables[0].Rows[k]["Linkvalue"]);

                                                    //   string fineType = Convert.ToString(dsFine.Tables[0].Rows[fn]["FineType"]);
                                                    //string finehdrfk = fineLegHedQ.Split('~')[2];
                                                    //string fineLgrId = fineLegHedQ.Split('~')[3];
                                                    string finehdrfk = head.Split('~')[2];

                                                    string fineLgrId = head.Split('~')[3];//modified by abarna
                                                    string fineLgrId1 = head.Split('~')[4];//abarna
                                                    string fineHdrName = d2.GetFunction(" select headername from fm_headermaster where headerpk=" + finehdrfk + " and CollegeCode=" + collegecode1 + "");
                                                    string fineLgrName = d2.GetFunction("  select ledgername from fm_ledgermaster where ledgerpk=" + fineLgrId + " and HeaderFK=" + finehdrfk + " and CollegeCode=" + collegecode1 + "");

                                                    string finelgrname1 = d2.GetFunction("select ledgername from fm_ledgermaster where ledgerpk=" + fineLgrId1 + " and HeaderFK=" + finehdrfk + " and CollegeCode=" + collegecode1 + "");
                                                    fineAmount = 0;
                                                    foreach (KeyValuePair<string, double> fine in dtfintFeecat)
                                                    {
                                                        //if (ledgerfk == fine.Key.Split(',')[2])
                                                        //{

                                                        string sbfine = string.Empty;
                                                        string feestr = string.Empty;

                                                        //string month=string .Empty ;//abarna
                                                        sbfine = Convert.ToString(fine.Key + "$" + fine.Value);
                                                        fineAmount = fine.Value;
                                                        string finefeecat = fine.Key.Split(',')[0];
                                                        if (dtfeecat.ContainsKey(finefeecat))
                                                            feestr = Convert.ToString(dtfeecat[finefeecat]);

                                                        string fineval = d2.GetFunction("select amount from ft_fineedit where headerfk='" + hdrfk + "' and ledgerfk='" + ledgerfk + "' and feecategory='" + feecategory + "' and app_no='" + app_no + "'");
                                                        if (fineval != "" && fineval != "0")
                                                        {
                                                            fineAmount = Convert.ToDouble (fineval);
                                                        }

                                                        FpSpread1.Sheets[0].RowCount++;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                                        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = Color.LightGreen;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = chkbox;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = "FINE" + "-" + feestr;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = "FINE" + "," + Convert.ToString(sbfine);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(fineHdrName);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(finehdrfk);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(fineLgrId);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(fineLgrName);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(fineLgrId);
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(Math.Round(Convert.ToDouble(fineAmount), 0));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Note = appno;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = "0";
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Note = finyrfk;

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Math.Round(Convert.ToDouble(fineAmount), 0));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                                                        if (Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]) == "")
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "0.00";
                                                        }
                                                        else
                                                        {
                                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = "0.00";
                                                            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(Math.Round(Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]), 0));
                                                        }
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(Math.Round(Convert.ToDouble(fineAmount), 0));
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Border.BorderColor = System.Drawing.Color.Black;
                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Border.BorderColorTop = System.Drawing.Color.Black;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";



                                                        //if (dtmonth.ContainsKey(fine.Key))//abarna
                                                        //    month = Convert.ToString(dtmonth[fine.Key]);
                                                        //DataRow drFine = tbl_Student.NewRow();
                                                        //drFine["Header_ID"] = finehdrfk;
                                                        //drFine["Header_Name"] = fineHdrName;
                                                        //drFine["Fee_Code"] = fineLgrId;
                                                        ////   drFine["Fee_Type"] = fineLgrName;
                                                        //drFine["Fee_Type"] = finelgrname1;
                                                        //name = fineLgrId;
                                                        //drFine["ChlTaken"] = "0";
                                                        ////drFine["TextVal"] = "FINE";
                                                        //drFine["TextVal"] = "FINE" + "-" + feestr;
                                                        //// drFine["TextCode"] = "-1";
                                                        //drFine["TextCode"] = Convert.ToString(sbfine);
                                                        //drFine["Fee_Amount"] = fineAmount;
                                                        //drFine["Deduct"] = "0";
                                                        //drFine["Total"] = fineAmount;
                                                        //drFine["PaidAmt"] = "0";
                                                        //drFine["BalAmt"] = fineAmount;
                                                        //drFine["ToBePaid"] = "0";
                                                        //drFine["Monthly"] = "0";
                                                        //drFine["Scholar"] = "0";
                                                        //drFine["CautionDep"] = "0";
                                                        ////tbl_Student.Rows.Add(drFine);
                                                        ////  string balAmti = Convert.ToString(tbl_Student.Rows[bal]["BalAmt"]).Trim();
                                                        //tbl_Student.Rows.InsertAt(drFine, 0);
                                                        //}


                                                    }
                                                }
                                            }
                                        }
                                    }
                                    #endregion
                                    //Re-Admission fees settings

                                    #endregion
                                }

                            }
                            FpSpread1.Visible = true;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Height = 300;
                            FpSpread1.Width = 950;
                            FpSpread1.ShowHeaderSelection = false;







                        }
                        else
                        {
                            FpSpread1.Visible = false;
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "No Records Found!";
                        }

                    }
                    else
                    {
                        FpSpread1.Visible = false;
                    }
                }
                else
                {
                    FpSpread1.Visible = false;
                }
            }
            else
            {
                FpSpread1.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

        }
        catch
        {

        }
    }

    public void bindgridpop()
    {
        try
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Sno");
            dt.Columns.Add("sem");
            dt.Columns.Add("semcode");
            dt.Columns.Add("Header");
            dt.Columns.Add("HeaderFk");
            dt.Columns.Add("Ledger");
            dt.Columns.Add("LedgerFk");
            dt.Columns.Add("FeeAmnt");
            dt.Columns.Add("addfeeamt");
            dt.Columns.Add("tmptxtfeeamt");
            dt.Columns.Add("concess");
            dt.Columns.Add("addconcess");
            dt.Columns.Add("tmptxtconsamt");
            dt.Columns.Add("totAmnt");
            dt.Columns.Add("balamnt");
            dt.Columns.Add("DDt");
            dt.Columns.Add("DDt1");
            dt.Columns.Add("addconcess1");
            dt.Columns.Add("DDt2");
            dt.Columns.Add("addconcess2");
            dt.Columns.Add("DDt3");
            dt.Columns.Add("addconcess3");
            dt.Columns.Add("DDt4");
            dt.Columns.Add("addconcess4");
            dt.Columns.Add("DDt5");
            dt.Columns.Add("addconcess5");
            dt.Columns.Add("DDt6");
            dt.Columns.Add("addconcess6");
            dt.Columns.Add("DDt7");
            dt.Columns.Add("addconcess7");
            dt.Columns.Add("DDt8");
            dt.Columns.Add("addconcess8");
            dt.Columns.Add("DDt9");
            dt.Columns.Add("addconcess9");
            dt.Columns.Add("DDt10");
            dt.Columns.Add("addconcess10");
            dt.Columns.Add("Reason");

            string feecato = "";
            string ledgers = string.Empty;
            for (int led = 0; led < FpSpread1.Sheets[0].Rows.Count; led++)
            {
                byte ischecked = Convert.ToByte(FpSpread1.Sheets[0].Cells[led, 1].Value);
                if (ischecked == 1)
                    if (ledgers.Equals(string.Empty))
                    {
                        ledgers = Convert.ToString(FpSpread1.Sheets[0].Cells[led, 4].Tag);
                        feecato = Convert.ToString(FpSpread1.Sheets[0].Cells[led, 2].Tag);
                    }
                    else
                    {
                        ledgers += "," + Convert.ToString(FpSpread1.Sheets[0].Cells[led, 4].Tag);
                        feecato += "," + Convert.ToString(FpSpread1.Sheets[0].Cells[led, 2].Tag);
                    }
            }
            if (ledgers.Trim().Equals(string.Empty))
            { ledgers = "0"; }

            DataRow dr;
            if (feecato.Trim() != "")
            {
                string selectQ = "";
                string app_no = "";
                //app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "'");
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    }
                }
                else
                {
                    app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                bool fee = false;
                if (app_no != "")
                {
                    string deg = d2.GetFunction("select degree_code from registration where  college_code=" + collegecode1 + " and app_no='" + app_no + "'");
                    if (feecato.Contains("FINE"))
                    {
                        fee = true;
                        feecato = feecato.Split(',')[1];
                        feecato = feecato.Split('$')[0];
                        selectQ = "select FineMasterPK, FineType, FromDay, ToDay, isnull(FineAmount,0) as feeamount,isnull(FineAmount,0) as totalamount,isnull(FineAmount,0) as balamount, DueDate, F.HeaderFk, Ledgerfk, Feecatgory, Degreecode, F.collegecode, LedgerName, HeaderName,( select (convert(varchar(10),finyearstart,103)+'-'+convert(varchar(10),finyearend,103)) as finyearfk from fm_finyearmaster fm where f.finyearfk=fm.finyearpk)as finyear,f.finyearfk,Stud_FineSettingType,'0' DeductAmout from Fm_FInemaster F,FM_LedgerMaster L,FM_HeaderMaster H where f.CollegeCode=L.CollegeCode and F.CollegeCode=h.CollegeCode and f.HeaderFK=h.HeaderPK and f.HeaderFK=l.HeaderFK and f.LedgerFK=l.LedgerPK and Duedate<GETDATE() and F.CollegeCode=" + collegecode1 + " and DegreeCode='" + deg + "' and L.LedgerPK='" + ledgers + "' and FeeCatgory in ('" + feecato + "') and BatchYear='" + txt_rebatch.Text + "'";
                    }
                    else
                    {
                        selectQ = "select HeaderPK,HeaderName,LedgerPK,LedgerName,LedgerFK,f.HeaderFK,AllotDate,FeeCategory,PayMode,FeeAmount,DeductAmout,DeductReason,TotalAmount,RefundAmount,FromGovtAmt,BalAmount,convert(varchar(10),DueDate,103) as DueDate,FineAmount,convert(varchar(10),PayStartDate,103) as  PayStartDate from FM_HeaderMaster m,FM_LedgerMaster l,FT_FeeAllot f where m.HeaderPK = l.HeaderFK and LedgerName not in ('cash','Income & Expenditure','Income','Expenditure') and m.HeaderPK=f.HeaderFK and l.LedgerPK=f.LedgerFK and App_No ='" + app_no + "' and m.CollegeCode='" + collegecode1 + "' and FeeCategory in(" + feecato + ") and l.ledgerpk in (" + ledgers + ")";
                        if (cb_showall.Checked == false)
                        {
                            selectQ = selectQ + " and BalAmount <>0";
                        }
                        selectQ = selectQ + " Order by LedgerFK";

                    }
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selectQ, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                string feecat = string.Empty;
                                if (fee)
                                {
                                    feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCatgory"]);
                                }
                                else
                                {
                                    feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                                }
                                string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + collegecode1 + "");
                                dr = dt.NewRow();
                                dr["Sno"] = row + 1;
                                dr["sem"] = cursem;
                                dr["semcode"] = feecat;
                                dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                                dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                                dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                                dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                                dr["FeeAmnt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                                dr["concess"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                                dr["totAmnt"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                                dr["balamnt"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                                dt.Rows.Add(dr);
                            }
                        }
                        else
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Visible = true;
                            lblalerterr.Text = "No Records Found!";
                        }
                    }
                }
                if (dt.Rows.Count > 0)
                {
                    gridViewpop.DataSource = dt;
                    gridViewpop.DataBind();
                    gridViewpop.Columns[5].Visible = false;
                    gridViewpop.Columns[8].Visible = false;
                }
                else
                {
                    gridViewpop.DataSource = null;
                    gridViewpop.DataBind();
                }
                if (gridViewpop.Rows.Count > 0)
                {
                    string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='FERES' and college_code ='" + collegecode1 + "'";
                    DataSet dsreason = new DataSet();
                    dsreason.Clear();
                    dsreason = d2.select_method_wo_parameter(sql, "Text");
                    foreach (GridViewRow gvrow in gridViewpop.Rows)
                    {
                        for (int i = 12; i < gridViewpop.Columns.Count - 1; i++)
                        {
                            gridViewpop.Columns[i].Visible = false;
                        }
                        //TextBox txtAmt = (TextBox)gvrow.Cells[9].FindControl("txt_dueext1");
                        TextBox concs = (TextBox)gvrow.Cells[8].FindControl("txt_concess");
                        TextBox addcons = (TextBox)gvrow.Cells[9].FindControl("txt_addconcess");
                        DropDownList drp = (DropDownList)gvrow.Cells[32].FindControl("ddl_reason");
                        //txtAmt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        drp.Items.Clear();
                        if (dsreason.Tables.Count > 0)
                        {
                            if (dsreason.Tables[0].Rows.Count > 0)
                            {
                                drp.DataSource = dsreason;
                                drp.DataTextField = "TextVal";
                                drp.DataValueField = "TextCode";
                                drp.DataBind();
                                drp.Items.Insert(0, new ListItem("Select", "0"));
                            }
                            else
                            {
                                drp.Items.Insert(0, new ListItem("Select", "0"));
                            }
                        }
                    }
                    bindaddreason();
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select any one semester!";
            }
        }
        catch
        {

        }
    }

    protected void btnsavepop_click(object sender, EventArgs e)
    {
        try
        {
            int amntcount = 0;
            int datecount = 0;
            int inscount = 0;
            bool saveflage = false;

            string finid = d2.getCurrentFinanceYear(usercode, collegecode1);
            if (cb_datewithamnt.Checked == true)
            {
                if (gridViewpop.Rows.Count > 0)
                {

                    foreach (GridViewRow gv in gridViewpop.Rows)
                    {
                        Label hdrfk = (Label)gv.Cells[2].FindControl("lbl_hdridpop");
                        Label ledgefk = (Label)gv.Cells[3].FindControl("lbl_lgrid");
                        Label feeid = (Label)gv.Cells[1].FindControl("lbl_feecode");

                        TextBox txtdueamnt = new TextBox();
                        txtdueamnt.ID = "txt_addconcess";
                        string txtamntid = txtdueamnt.ID;
                        string amntcode = "";
                        TextBox txtamnt = new TextBox();

                        TextBox txtduedt = new TextBox();
                        txtduedt.ID = "txt_dueext";
                        string txtid = txtduedt.ID;
                        string txtcode = "";
                        string txtcodecom = "";
                        TextBox txtgetdue = new TextBox();
                        TextBox txtgetduecom = new TextBox();
                        string[] spldue = new string[2];
                        DateTime duedate = DateTime.Now;
                        DateTime duecomdate = DateTime.Now;
                        string value = Convert.ToString(txt_datewithamnt.Text);
                        int colcount = 0;
                        if (value.Trim() != "")
                        {
                            colcount = Convert.ToInt32(value);
                        }
                        int colidx = 9;
                        colidx++;
                        for (int j = 1; j <= colcount; j++)
                        {
                            amntcode = txtamntid + (j).ToString();
                            txtamnt = (TextBox)gv.Cells[colidx].FindControl(amntcode);
                            if (txtamnt.Text.Trim() == "")
                            {
                                amntcount++;
                            }
                        }
                        for (int k = 2; k <= colcount; k++)
                        {
                            txtcode = txtid + (k).ToString();
                            txtgetdue = (TextBox)gv.Cells[colidx].FindControl(txtcode);
                            if (txtgetdue.Text.Trim() != "")
                            {
                                spldue = Convert.ToString(txtgetdue.Text).Split('/');
                                duedate = Convert.ToDateTime(spldue[1] + "/" + spldue[0] + "/" + spldue[2]);
                            }
                            txtcodecom = txtid + (k - 1).ToString();
                            txtgetduecom = (TextBox)gv.Cells[colidx].FindControl(txtcodecom);
                            if (txtgetduecom.Text.Trim() != "")
                            {
                                spldue = Convert.ToString(txtgetduecom.Text).Split('/');
                                duecomdate = Convert.ToDateTime(spldue[1] + "/" + spldue[0] + "/" + spldue[2]);
                            }
                            if (duecomdate >= duedate)
                            {
                                datecount++;
                            }
                        }
                    }
                }
                if (amntcount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Please Enter all Amount!";
                }
                else if (datecount > 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Due Extention Should be greater than Due Amount!";
                }
                else
                {
                    string insquery = "";
                    int feeext = 0;
                    Label lbldue = new Label();
                    string[] spldue = new string[2];
                    DateTime duedate = DateTime.Now;
                    DateTime dtext = DateTime.Now;
                    string app_no = "";
                    string roll = Convert.ToString(txt_rerollno.Text);
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                    {
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                            app_no = d2.GetFunction("select app_no from Registration where roll_no='" + roll + "' and college_code='" + ddl_college.SelectedItem.Value + "'");
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                            app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + roll + "' and college_code='" + ddl_college.SelectedItem.Value + "'");
                        if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                            app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + roll + "' and college_code='" + ddl_college.SelectedItem.Value + "'");
                    }
                    else
                        app_no = d2.GetFunction("select app_no from applyn where app_formno='" + roll + "' and college_code='" + ddl_college.SelectedItem.Value + "'");

                    string SQL = "select distinct top 1 Extention_count from FeesDueExt where App_No='" + app_no + "' order by Extention_count desc";
                    string extcount = d2.GetFunction(SQL);
                    int countext = 0;
                    if (extcount != "" && extcount != "0")
                        countext = Convert.ToInt32(extcount);
                    if (countext != 0)
                        feeext = countext + 1;
                    else
                        feeext = 1;

                    if (gridViewpop.Rows.Count > 0)
                    {
                        TextBox txtduedt = new TextBox();
                        txtduedt.ID = "txt_dueext";
                        string txtid = txtduedt.ID;
                        string txtcode = "";
                        TextBox txtgetdue = new TextBox();

                        TextBox txtdueamnt = new TextBox();
                        txtdueamnt.ID = "txt_addconcess";
                        string txtamntid = txtdueamnt.ID;
                        string amntcode = "";
                        TextBox txtamnt = new TextBox();

                        foreach (GridViewRow gvrows in gridViewpop.Rows)
                        {
                            double getbal = 0.00;
                            string value1 = Convert.ToString(txt_datewithamnt.Text);
                            int colcount = 0;
                            if (value1.Trim() != "")
                                colcount = Convert.ToInt32(value1);
                            int colidx = 9;
                            colidx++;
                            for (int j = 1; j <= colcount; j++)
                            {
                                txtcode = txtid + (j).ToString();
                                txtgetdue = (TextBox)gvrows.Cells[colidx].FindControl(txtcode);
                                amntcode = txtamntid + (j).ToString();
                                txtamnt = (TextBox)gvrows.Cells[colidx].FindControl(amntcode);
                                if (txtgetdue.Text.Trim() != "")
                                {
                                    spldue = Convert.ToString(txtgetdue.Text).Split('/');
                                    duedate = Convert.ToDateTime(spldue[1] + "/" + spldue[0] + "/" + spldue[2]);
                                }

                                Label hdrfk = (Label)gvrows.Cells[2].FindControl("lbl_hdridpop");
                                Label ledgefk = (Label)gvrows.Cells[3].FindControl("lbl_lgrid");
                                Label feeid = (Label)gvrows.Cells[1].FindControl("lbl_feecode");
                                Label balamnt = (Label)gvrows.Cells[11].FindControl("lbl_balamnt");
                                getbal = Convert.ToDouble(balamnt.Text);
                                DropDownList extreason = (DropDownList)gvrows.Cells[30].FindControl("ddl_reason");
                                TextBox dedamnt = (TextBox)gvrows.Cells[8].FindControl("txt_addconcess");
                                string deduct = "";
                                if (dedamnt.Text.Trim() != "" && dedamnt.Text.Trim() != "0.00")
                                    deduct = dedamnt.Text;
                                else
                                    deduct = "0.00";
                                string selextdue = "Select distinct DueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text.ToString() + "' and LedgerFK='" + ledgefk.Text.ToString() + "' and FeeCategory='" + feeid.Text.ToString() + "'";
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(selextdue, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string sQL = "Select top 1 ExtDueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text.ToString() + "' and LedgerFK='" + ledgefk.Text.ToString() + "' and FeeCategory='" + feeid.Text.ToString() + "' order by ExtDueDate desc";
                                        string extdue = d2.GetFunction(sQL);
                                        if (extdue.Trim() != "" && extdue.Trim() != "0")
                                        {
                                            string[] splextdue = extdue.Split('/');
                                            dtext = Convert.ToDateTime(splextdue[0] + "/" + splextdue[1] + "/" + splextdue[2]);
                                        }
                                        else
                                            dtext = duedate;
                                    }
                                    else
                                    {
                                        string seldue = "SELECT distinct DueDate FROM  FM_FineMaster where DegreeCode = '" + ViewState["degid"].ToString() + "' AND FeeCatgory = '" + feeid.Text.ToString() + "' AND HeaderFK = '" + hdrfk.Text.ToString() + "' AND LedgerFK = '" + ledgefk.Text.ToString() + "'";
                                        string duest = d2.GetFunction(seldue);
                                        if (duest.Trim() != "" && duest.Trim() != "0")
                                        {
                                            string[] splduest = duest.Split('/');
                                            dtext = Convert.ToDateTime(splduest[0] + "/" + splduest[1] + "/" + splduest[2]);
                                        }
                                        else
                                            dtext = duedate;
                                    }
                                }
                                insquery = "if exists (select * from FeesDueExt where App_No='" + app_no + "' and ExtDueDate='" + duedate.ToString("MM/dd/yyyy") + "'  and FeeCategory='" + feeid.Text + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFK='" + ledgefk.Text + "') update FeesDueExt set HeaderFK='" + hdrfk.Text + "',LedgerFK='" + ledgefk.Text + "',DueAmount='" + balamnt.Text + "',ExtDueAmount='" + txtamnt.Text + "',ExtReason='" + extreason.SelectedItem.Value + "',FinYearFK='" + finid + "',DeductAmount='" + deduct + "',UserCode='" + usercode + "' where App_No='" + app_no + "' and ExtDueDate='" + duedate.ToString("MM/dd/yyyy") + "' and FeeCategory='" + feeid.Text + "' else Insert into FeesDueExt(App_No,HeaderFK,LedgerFK,DueAmount,ExtDueAmount,DueDate,ExtDueDate,ExtReason,FinYearFK,DeductAmount,UserCode,FeeCategory,Extention_count) values ('" + app_no + "','" + hdrfk.Text + "','" + ledgefk.Text + "','" + balamnt.Text + "','" + txtamnt.Text + "','" + dtext.ToString("MM/dd/yyyy") + "','" + duedate.ToString("MM/dd/yyyy") + "','" + extreason.SelectedItem.Value + "','" + finid + "','" + deduct + "','" + usercode + "','" + feeid.Text + "','" + feeext + "')";
                                if (deduct.Trim() != "" && deduct.Trim() != "0.00")
                                {
                                    insquery = insquery + "Update FT_FeeAllot set DeductAmout=(DeductAmout+'" + deduct + "'),TotalAmount=(TotalAmount-'" + deduct + "') where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                }
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                            }
                        }
                    }
                }
                if (inscount > 0)
                {
                    bindgridpop();
                    bindspread();
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                }
            }
            else
            {
                string app_no = "";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        app_no = d2.GetFunction("select app_no from Registration where Reg_No='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        app_no = d2.GetFunction("select app_no from Registration where Roll_Admit='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                }
                else
                    app_no = d2.GetFunction("select app_no from applyn where app_formno='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
                foreach (GridViewRow gvrows in gridViewpop.Rows)
                {
                    Label hdrfk = (Label)gvrows.Cells[2].FindControl("lbl_hdridpop");
                    Label ledgefk = (Label)gvrows.Cells[3].FindControl("lbl_lgrid");
                    Label feeid = (Label)gvrows.Cells[1].FindControl("lbl_feecode");
                    Label balamnt = (Label)gvrows.Cells[11].FindControl("lbl_balamnt");
                    double getbal = Convert.ToDouble(balamnt.Text);
                    DropDownList extreason = (DropDownList)gvrows.Cells[30].FindControl("ddl_reason");
                    //  TextBox dedamnt = (TextBox)gvrows.Cells[6].FindControl("txt_addconcess");txt_concess
                    TextBox dedamnt = (TextBox)gvrows.Cells[6].FindControl("txt_concess");
                    Label tmpdemt = (Label)gvrows.Cells[8].FindControl("tmpconsamt");
                    Label totamt = (Label)gvrows.Cells[10].FindControl("lbl_tot");

                    Label lblfeeamt = (Label)gvrows.Cells[4].FindControl("lbl_fee");
                    //  lblfee = (Label)gvpopro.Cells[4].FindControl("lbl_fee");
                    TextBox txtaddfeeamt = (TextBox)gvrows.Cells[5].FindControl("txtfeemat");
                    if (cbcons.Checked == true)
                    {
                        string deduct = "";
                        if (dedamnt.Text.Trim() != "" && dedamnt.Text.Trim() != "0.00")
                            deduct = dedamnt.Text;
                        else
                            deduct = "0.00";
                        if (cb_concesstbl.Checked == true)
                        {
                            if (deduct.Trim() != "" && deduct.Trim() != "0.00")
                            {
                                string insquery = "Update FT_FeeAllot set DeductAmout='" + deduct + "',TotalAmount=('" + totamt.Text + "'),BalAmount=('" + balamnt.Text + "') where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                insquery = insquery + "if exists(select * from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "')update FeesDueExt set DeductAmount='" + deduct + "' where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' else insert into FeesDueExt (App_No,HeaderFK,LedgerFK,DeductAmount,UserCode,FeeCategory,FinYearFK,ExtReason) values ('" + app_no + "','" + hdrfk.Text + "','" + ledgefk.Text + "','" + deduct + "','" + usercode + "','" + feeid.Text + "','" + finid + "','" + extreason.SelectedItem.Value + "')";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                                if (inscount > 0)
                                    saveflage = true;
                            }
                        }
                        else
                        {
                            if (deduct.Trim() != "" && deduct.Trim() != "0.00")
                            {
                                string insquery = "Update FT_FeeAllot set DeductAmout='" + deduct + "',TotalAmount=('" + totamt.Text + "'),BalAmount=('" + balamnt.Text + "') where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                inscount = d2.update_method_wo_parameter(insquery, "Text");
                                string UpdateQ = "update FeesDueExt set DeductAmount='" + deduct + "',ExtReason='" + extreason.SelectedItem.Value + "' where  App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                int upd = d2.update_method_wo_parameter(UpdateQ, "Text");
                                if (inscount > 0 && upd > 0)
                                    saveflage = true;
                            }
                        }
                    }
                    else
                    {
                        string addfee = "";
                        string UpdateQ = "";
                        int update = 0;
                        string ledgers = string.Empty;
                        string feecato = string.Empty;
                        for (int led = 0; led < FpSpread1.Sheets[0].Rows.Count; led++)
                        {
                            byte ischecked = Convert.ToByte(FpSpread1.Sheets[0].Cells[led, 1].Value);
                            if (ischecked == 1)
                                if (ledgers.Equals(string.Empty))
                                {
                                    ledgers = Convert.ToString(FpSpread1.Sheets[0].Cells[led, 4].Tag);
                                    feecato = Convert.ToString(FpSpread1.Sheets[0].Cells[led, 2].Tag);
                                }
                                else
                                {
                                    ledgers += "," + Convert.ToString(FpSpread1.Sheets[0].Cells[led, 4].Tag);
                                    feecato += "," + Convert.ToString(FpSpread1.Sheets[0].Cells[led, 2].Tag);
                                }
                        }


                        if (txtaddfeeamt.Text.Trim() != "" && txtaddfeeamt.Text.Trim() != "0.00")
                            addfee = txtaddfeeamt.Text;
                        else
                            addfee = "0.00";
                        if (cbfeeamtadd.Checked == true)
                        {
                            if (feecato.Contains("FINE"))
                            {
                                UpdateQ = "if exists(select * from ft_fineedit where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "')update ft_fineedit set amount='" + lblfeeamt.Text + "' where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' else insert into ft_fineedit (App_No,HeaderFK,LedgerFK,amount,FeeCategory,reason) values ('" + app_no + "','" + hdrfk.Text + "','" + ledgefk.Text + "','" + lblfeeamt.Text + "','" + feeid.Text + "','" + extreason.SelectedItem.Value + "')";
                                update = d2.update_method_wo_parameter(UpdateQ, "Text");
                            }
                            else
                            {
                                UpdateQ = "Update FT_FeeAllot set FeeAmount='" + lblfeeamt.Text + "',TotalAmount='" + totamt.Text + "',BalAmount='" + balamnt.Text + "' where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                update = d2.update_method_wo_parameter(UpdateQ, "Text");
                            }
                            if (update != 0 && update != 0)
                                saveflage = true;
                        }
                        else
                        {
                            if (feecato.Contains("FINE"))
                            {
                                UpdateQ = "if exists(select * from ft_fineedit where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "')update ft_fineedit set amount='" + lblfeeamt.Text + "' where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' else insert into ft_fineedit (App_No,HeaderFK,LedgerFK,amount,FeeCategory,ExtReason) values ('" + app_no + "','" + hdrfk.Text + "','" + ledgefk.Text + "','" + lblfeeamt.Text + "','" + feeid.Text + "','" + extreason.SelectedItem.Value + "')";
                                update = d2.update_method_wo_parameter(UpdateQ, "Text");
                            }
                            else
                            {
                                UpdateQ = "Update FT_FeeAllot set FeeAmount='" + lblfeeamt.Text + "',TotalAmount='" + totamt.Text + "',BalAmount='" + balamnt.Text + "' where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFk='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "'";
                                update = d2.update_method_wo_parameter(UpdateQ, "Text");
                            }
                            if (update != 0 && update != 0)
                                saveflage = true;
                        }
                    }
                }
                if (saveflage == true)
                {
                    bindgridpop();
                    btnsavepop.Enabled = false;
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "Saved Successfully";
                }
            }
        }
        catch
        {

        }
    }

    protected void btnextpop_click(object sender, EventArgs e)
    {
        bindspread();
        popfine.Visible = false;
    }

    protected void ddl_AmtPerc_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void chk_refCommon_OnCheckedChanged(object sender, EventArgs e)
    {

    }

    protected void ddl_refheader_OnSelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnext_Click(object sender, EventArgs e)
    {
        bool check = false;
        for (int sel = 0; sel < FpSpread1.Sheets[0].Rows.Count; sel++)
        {
            string value = Convert.ToString(FpSpread1.Sheets[0].Cells[sel, 1].Value);
            if (value == "1")
            {
                check = true;
            }
        }
        if (check == true)
        {
            popfine.Visible = true;
            cbcons.Checked = false;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
            cbfeeamt.Checked = false;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;

            cb_concesstbl.Enabled = false;
            cbconsdecrs.Enabled = false;
            cbfeeamtdecrs.Enabled = false;
            cbfeeamtadd.Enabled = false;


            divpopup.Visible = true;
            gridViewpop.Visible = true;
            cb_comdt.Checked = false;
            cb_comreason.Checked = false;
            cb_concesstbl.Checked = false;
            cb_datewithamnt.Checked = false;
            bindaddreason();
            bindgridpop();
            btnadddate_Click(sender, e);
            txt_datewithamnt.Text = "";
            txt_datewithamnt.Visible = false;
            if (ddl_detre.Items.Count > 0)
                ddl_detre.SelectedIndex = 0;
            btnsavepop.Enabled = false;
            string app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            if (gridViewpop.Rows.Count > 0)
            {
                foreach (GridViewRow gv in gridViewpop.Rows)
                {
                    Label lblhdr = (Label)gv.Cells[2].FindControl("lbl_hdridpop");
                    Label lblledge = (Label)gv.Cells[3].FindControl("lbl_lgrid");
                    Label feeid = (Label)gv.Cells[1].FindControl("lbl_feecode");
                    string sel = "select top 1 convert(varchar(10),ExtDueDate,103) as ExtDueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + lblhdr.Text + "' and LedgerFK='" + lblledge.Text + "' and FeeCategory='" + feeid.Text + "' order by Extduedate desc";
                    string date = d2.GetFunction(sel);
                    string[] spl = new string[2];
                    if (date.Trim() != "0" && date != "")
                    {
                        spl = date.Split('/');
                        DateTime dtspl = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                        txt_comdt.Text = dtspl.AddDays(1).ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txt_comdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    }
                }
            }
        }
        else
        {
            popfine.Visible = false;
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Please Select Any One Semester!";
        }


    }

    protected void imagepopclose_click(object sender, EventArgs e)
    {
        bindspread();
        popfine.Visible = false;
    }

    //protected void cb_comdt_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_comdt.Checked == true)
    //        {
    //            foreach (GridViewRow gvro in gridViewpop.Rows)
    //            {
    //                TextBox txtdue = (TextBox)gvro.Cells[9].FindControl("txt_dueext");
    //                txtdue.Text = txt_comdt.Text;
    //            }
    //        }
    //        else
    //        {
    //            foreach (GridViewRow gvro in gridViewpop.Rows)
    //            {
    //                TextBox txtdue = (TextBox)gvro.Cells[9].FindControl("txt_dueext");
    //                txtdue.Text = "";
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void txt_comdt_OnTextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (txt_comdt.Text.Trim() != "")
    //        {
    //            foreach (GridViewRow gvro in gridViewpop.Rows)
    //            {
    //                TextBox txtdue = (TextBox)gvro.Cells[9].FindControl("txt_dueext");
    //                txtdue.Text = txt_comdt.Text;
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void ddl_detre_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_comreason.Checked == true)
    //        {
    //            if (ddl_detre.SelectedIndex != 0 && ddl_detre.SelectedItem.Text.Trim() != "Select")
    //            {
    //                foreach (GridViewRow gvro in gridViewpop.Rows)
    //                {
    //                    DropDownList drp = (DropDownList)gvro.Cells[10].FindControl("ddl_reason");
    //                    drp.SelectedValue = ddl_detre.SelectedValue;
    //                }
    //            }
    //            else
    //            {
    //                foreach (GridViewRow gvro in gridViewpop.Rows)
    //                {
    //                    DropDownList drp = (DropDownList)gvro.Cells[10].FindControl("ddl_reason");
    //                    drp.SelectedIndex = 0;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            foreach (GridViewRow gvro in gridViewpop.Rows)
    //            {
    //                DropDownList drp = (DropDownList)gvro.Cells[10].FindControl("ddl_reason");
    //                drp.SelectedIndex = 0;
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    //protected void cb_comreason_OnCheckedChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cb_comreason.Checked == true)
    //        {
    //            ddl_detre_OnSelectedIndexChanged(sender, e);
    //        }
    //        else
    //        {
    //            foreach (GridViewRow gvro in gridViewpop.Rows)
    //            {
    //                DropDownList drp = (DropDownList)gvro.Cells[10].FindControl("ddl_reason");
    //                drp.SelectedIndex = 0;
    //            }
    //            ddl_detre.SelectedIndex = 0;
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}

    protected void btnadddate_Click(object sender, EventArgs e)
    {
        try
        {
            string app_no = d2.GetFunction("select app_no from Registration where roll_no='" + txt_rerollno.Text.Trim() + "' and college_code='" + collegecode1 + "'");
            if (cb_comdt.Checked == true)
            {
                int count = 0;
                string getdue = "";
                string gotdue = "";
                DateTime dueextdt = DateTime.Now;
                DateTime duedt = DateTime.Now;
                string[] spldueext = new string[2];
                DateTime dueext = DateTime.Now;
                TextBox txtduemain = new TextBox();
                if (gridViewpop.Rows.Count > 0)
                {
                    foreach (GridViewRow gvrows in gridViewpop.Rows)
                    {
                        Label hdrfk = (Label)gvrows.Cells[2].FindControl("lbl_hdridpop");
                        Label ledgefk = (Label)gvrows.Cells[3].FindControl("lbl_lgrid");
                        Label feeid = (Label)gvrows.Cells[1].FindControl("lbl_feecode");
                        txtduemain = (TextBox)gvrows.Cells[12].FindControl("txt_dueext1");
                        string selq = "select top 1 convert(varchar(10),ExtDueDate,103) as ExtDueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFK='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' order by ExtDueDate desc";
                        selq = selq + " select distinct DueDate from FM_FineMaster where HeaderFK='" + hdrfk.Text + "' and LedgerFK='" + ledgefk.Text + "' and FeeCatgory='" + feeid.Text + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selq, "Text");
                        if (ds.Tables.Count > 0)
                        {
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                getdue = Convert.ToString(ds.Tables[0].Rows[0]["ExtDueDate"]);
                                string[] splgetdue = getdue.Split('/');
                                duedt = Convert.ToDateTime(splgetdue[1] + "/" + splgetdue[0] + "/" + splgetdue[2]);
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                gotdue = Convert.ToString(ds.Tables[1].Rows[0]["DueDate"]);
                                dueextdt = Convert.ToDateTime(gotdue);
                            }
                            spldueext = txt_comdt.Text.Split('/');
                            dueext = Convert.ToDateTime(spldueext[1] + "/" + spldueext[0] + "/" + spldueext[2]);
                            if (duedt > dueext && dueextdt > dueext)
                            {
                                count++;
                            }
                        }
                        else
                        {
                            txtduemain.Text = txt_comdt.Text;
                        }
                    }
                    if (count > 0)
                    {
                        txt_comdt.Text = duedt.AddDays(1).ToString("dd/MM/yyyy");
                        alertpopwindow.Visible = true;
                        lblalerterr.Visible = true;
                        lblalerterr.Text = "Due Extension should be less than Due Date!";
                    }
                    else
                    {
                        foreach (GridViewRow gvro in gridViewpop.Rows)
                        {
                            TextBox txtdue = (TextBox)gvro.Cells[10].FindControl("txt_dueext1");
                            txtdue.Text = txt_comdt.Text;
                        }
                    }
                }
                btnsavepop.Enabled = true;
            }
            else
            {
                foreach (GridViewRow gvro in gridViewpop.Rows)
                {
                    TextBox txtdue = (TextBox)gvro.Cells[12].FindControl("txt_dueext1");
                    txtdue.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                txt_comdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }

            if (cb_comreason.Checked == true)
            {
                if (ddl_detre.SelectedIndex != 0 && ddl_detre.SelectedItem.Text.Trim() != "Select")
                {
                    foreach (GridViewRow gvro in gridViewpop.Rows)
                    {
                        DropDownList drp = (DropDownList)gvro.Cells[30].FindControl("ddl_reason");
                        drp.SelectedValue = ddl_detre.SelectedValue;
                    }
                }
                else
                {
                    foreach (GridViewRow gvro in gridViewpop.Rows)
                    {
                        DropDownList drp = (DropDownList)gvro.Cells[30].FindControl("ddl_reason");
                        drp.SelectedIndex = 0;
                    }
                    ddl_detre.SelectedIndex = 0;
                }
            }
            else
            {
                foreach (GridViewRow gvro in gridViewpop.Rows)
                {
                    DropDownList drp = (DropDownList)gvro.Cells[32].FindControl("ddl_reason");
                    drp.SelectedIndex = 0;
                }
                ddl_detre.SelectedIndex = 0;
            }

            if (cb_concesstbl.Checked == true || cbconsdecrs.Checked == true)
            {
                if (cb_concesstbl.Checked == true)
                {
                    gridViewpop.Columns[8].HeaderText = "Additional Concession";
                }
                else
                {
                    gridViewpop.Columns[8].HeaderText = "Deduct Concession";
                }
                gridViewpop.Columns[8].Visible = true;

            }
            else
            {
                gridViewpop.Columns[8].Visible = false;
                //  gridViewpop.Columns[7].HeaderText = "Deduct Concession";
            }
            if (cbfeeamtadd.Checked == true || cbfeeamtdecrs.Checked == true)
            {
                if (cbfeeamtadd.Checked == true)
                {
                    gridViewpop.Columns[5].HeaderText = "Additional Feeamount";
                }
                else
                {
                    gridViewpop.Columns[5].HeaderText = "Deduct Feeamount";
                }
                gridViewpop.Columns[5].Visible = true;

            }
            else
            {
                gridViewpop.Columns[5].Visible = false;
                // gridViewpop.Columns[5].HeaderText = "Deduct Feeamount";
            }

            if (cb_datewithamnt.Checked == true && txt_datewithamnt.Text.Trim() != "")
            {
                int colcount = Convert.ToInt32(txt_datewithamnt.Text);
                if (colcount <= 10)
                {
                    TextBox txtduedt = new TextBox();
                    txtduedt.ID = "txt_dueext";
                    string txtid = txtduedt.ID;
                    string txtcode = "";
                    TextBox txtcons = new TextBox();
                    txtcons.ID = "txt_addconcess";
                    string consid = txtcons.ID;
                    string txtconscode = "";
                    foreach (GridViewRow gvrow in gridViewpop.Rows)
                    {
                        Label hdrfk = (Label)gvrow.Cells[2].FindControl("lbl_hdridpop");
                        Label ledgefk = (Label)gvrow.Cells[3].FindControl("lbl_lgrid");
                        Label feeid = (Label)gvrow.Cells[1].FindControl("lbl_feecode");
                        int colidx = 11;
                        colidx++;
                        for (int j = 1; j <= 10; j++)
                        {
                            string selq = "select top 1 convert(varchar(10),ExtDueDate,103) as ExtDueDate from FeesDueExt where App_No='" + app_no + "' and HeaderFK='" + hdrfk.Text + "' and LedgerFK='" + ledgefk.Text + "' and FeeCategory='" + feeid.Text + "' order by Extduedate desc";
                            string date = d2.GetFunction(selq);
                            string[] spl = new string[2];
                            if (date.Trim() != "0" && date != "")
                            {
                                spl = date.Split('/');
                                DateTime dtspl = Convert.ToDateTime(spl[1] + "/" + spl[0] + "/" + spl[2]);
                                txtcode = txtid + (j).ToString();
                                TextBox txtgetdue = (TextBox)gvrow.Cells[colidx].FindControl(txtcode);
                                txtgetdue.Text = dtspl.AddDays(1).ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                txtcode = txtid + (j).ToString();
                                TextBox txtgetdue = (TextBox)gvrow.Cells[colidx].FindControl(txtcode);
                                txtgetdue.Text = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                            txtconscode = consid + (j).ToString();
                            TextBox txtgetcons = (TextBox)gvrow.Cells[colidx].FindControl(txtconscode);
                            txtgetcons.Text = "";
                        }
                        for (int i = colidx; i < (colidx + (colcount * 2)); i++)
                        {
                            gridViewpop.Columns[i].Visible = true;
                        }
                        for (int j = (colidx + (colcount * 2)); j < gridViewpop.Columns.Count - 1; j++)
                        {
                            gridViewpop.Columns[j].Visible = false;
                        }
                    }
                }
                else
                {
                    txt_datewithamnt.Text = "";
                    alertpopwindow.Visible = true;
                    lblalerterr.Visible = true;
                    lblalerterr.Text = "The Due Extention Exceed,It allows 10 Due Extention!";
                }
            }
            else
            {
                int colidx = 13;
                for (int i = colidx; i < gridViewpop.Columns.Count - 1; i++)
                {
                    gridViewpop.Columns[i].Visible = false;
                }

            }
            btnsavepop.Enabled = true;
        }
        catch
        {

        }
    }

    protected void ddl_reason_OnSelectedIndexChanged(object sender, EventArgs e)
    {

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
                    txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
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
        bindspread();
    }

    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sem.Text = "--Select--";
        string sem = "";
        cb_sem.Checked = false;
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
                    txt_sem.Text = "Semester(" + commcount.ToString() + ")";
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
        bindspread();
    }

    protected void btn_addreason_Click(object sender, EventArgs e)
    {
        try
        {
            if (lbl_addreason.Text == "Add Reason")
            {
                if (txt_addreason.Text != "")
                {
                    string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='FERES' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_addreason.Text + "' where TextVal ='" + txt_addreason.Text + "' and TextCriteria ='FERES' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_addreason.Text + "','FERES','" + collegecode1 + "')";
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
                    bindgridpop();
                    txt_addreason.Text = "";
                }
                else
                {
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

    protected void btn_exitaddreason_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addreason.Visible = false;
        txt_addreason.Text = "";
    }

    protected void btn_plus_detre_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addreason.Visible = true;
        lbl_addreason.Text = "Add Reason";
        lblerror.Visible = false;
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
                string sql = "delete from TextValTable where TextCode='" + ddl_detre.SelectedItem.Value.ToString() + "' and TextCriteria='FERES' and college_code='" + collegecode1 + "' ";
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
            bindgridpop();
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

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();

            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindaddreason()
    {
        try
        {
            ddl_detre.Items.Clear();
            ds.Clear();
            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='FERES' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "Text");
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


    protected void loadsem()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
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
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len(textval),textval asc";
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
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by len(textval),textval asc";
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
    public void loadfromsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(list1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
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
                    txt_rerollno.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rerollno.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rerollno.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rerollno.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
            }

        }
        catch { }

    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_rerollno.Text = "";
            txt_rename.Text = "";
            txt_rebatch.Text = "";
            txt_restrm.Text = "";
            txt_redegree.Text = "";
            txt_redept.Text = "";
            txt_sem.Text = "";
            txt_resec.Text = "";
            image3.ImageUrl = "";
            bindspread();
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rerollno.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rerollno.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rerollno.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rerollno.Attributes.Add("Placeholder", "App No");
                    chosedmode = 3;
                    break;
            }
        }
        catch
        { }
    }

    #region Consession Add
    protected void cbcons_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbcons.Checked == true)
        {
            cb_concesstbl.Enabled = true;
            cbconsdecrs.Enabled = true;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
            cbfeeamtadd.Enabled = false;
            cbfeeamtdecrs.Enabled = false;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;
            bindgridpop();

        }
        else
        {
            cb_concesstbl.Enabled = false;
            cbconsdecrs.Enabled = false;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
            cbfeeamtadd.Enabled = true;
            cbfeeamtdecrs.Enabled = true;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;
            // bindgridpop();
        }
    }

    protected void cbfeeamt_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbfeeamt.Checked == true)
        {
            cbfeeamtadd.Enabled = true;
            cbfeeamtdecrs.Enabled = true;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;
            cb_concesstbl.Enabled = false;
            cbconsdecrs.Enabled = false;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
            bindgridpop();
        }
        else
        {
            cbfeeamtadd.Enabled = false;
            cbfeeamtdecrs.Enabled = false;
            cbfeeamtadd.Checked = false;
            cbfeeamtdecrs.Checked = false;
            cb_concesstbl.Enabled = true;
            cbconsdecrs.Enabled = true;
            cb_concesstbl.Checked = false;
            cbconsdecrs.Checked = false;
        }
    }

    protected void cb_concesstbl_Changed(object sender, EventArgs e)
    {
        if (cb_concesstbl.Checked == true)
        {
            gridViewpop.Columns[8].HeaderText = "Additional Concession";
            bindgridpop();
        }

    }
    protected void cbconsdecrs_Changed(object sender, EventArgs e)
    {
        if (cbconsdecrs.Checked == true)
        {
            gridViewpop.Columns[8].HeaderText = "Deduct Concession";
            bindgridpop();
        }
    }
    protected void cbfeeamtadd_Changed(object sender, EventArgs e)
    {
        if (cbfeeamtadd.Checked == true)
        {
            gridViewpop.Columns[5].HeaderText = "Additional Feeamount";
            bindgridpop();
        }


    }
    protected void cbfeeamtdecrs_Changed(object sender, EventArgs e)
    {
        if (cbfeeamtdecrs.Checked == true)
        {
            gridViewpop.Columns[5].HeaderText = "Deduct Feeamount";
            bindgridpop();
        }
    }
    #endregion

    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[3].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }

    //public int rowindexclicked()
    //{
    //    int rowindx = -1;
    //    try
    //    {
    //        Control id = GetPostBackControl(this.Page);
    //        string rno = Convert.ToString();
    //    }
    //    catch { }
    //}

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
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lbl_sem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void cb_showall_Changed(object sender, EventArgs e)
    {
        txt_rerollno_TextChanged(sender, e);
    }

    // last modified 22.06.2017 sudhagar

    protected void btn_go_Click(object sender, EventArgs e)
    {
        txt_rerollno_TextChanged(sender, e);
    }

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

    protected string getAppNo()
    {
        string appNo = string.Empty;
        try
        {
            string rollNo = Convert.ToString(txt_rerollno.Text);
            string selQ = string.Empty;
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    selQ = " select app_no from registration where Roll_no='" + rollNo + "' and college_code=" + ddl_college.SelectedValue + "";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    selQ = "select app_no from registration where Reg_No='" + rollNo + "' and college_code=" + ddl_college.SelectedValue + "";
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    selQ = "select app_no from registration where Roll_Admit='" + rollNo + "' and college_code=" + ddl_college.SelectedValue + "";
            }
            else
            {
                selQ = "select app_no from applyn where app_formno='" + rollNo + "' and college_code=" + ddl_college.SelectedValue + " and admission_status =0 and isconfirm ='1'";
            }
            if (!string.IsNullOrEmpty(selQ))
            {
                appNo = d2.GetFunction(selQ);
            }
        }
        catch { appNo = "0"; }
        return appNo;
    }
}