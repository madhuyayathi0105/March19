using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.Web.UI;
using System.Text;
using System.Configuration;

public partial class ReceiptJpr : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;

    string selectQuery = "";
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    static Hashtable studhash = new Hashtable();
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                setLabelTextlookup();
                LoadFromSettings();
                bindGrid("-1");
                LoadYearSemester();
                txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_rdate.Attributes.Add("ReadOnly", "ReadOnly");

                txt_recolg.Attributes.Add("ReadOnly", "ReadOnly");
                txt_rebatch.Attributes.Add("ReadOnly", "ReadOnly");
                txt_redegree.Attributes.Add("ReadOnly", "ReadOnly");
                txt_redept.Attributes.Add("ReadOnly", "ReadOnly");
                txt_resem.Attributes.Add("ReadOnly", "ReadOnly");
                txt_resec.Attributes.Add("ReadOnly", "ReadOnly");
                txt_tostudentsrcpt.Attributes.Add("ReadOnly", "ReadOnly");
            }
        }
        catch (Exception ex) { }
    }
    protected void lb3_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void rb_single_Change(object sender, EventArgs e)
    {
        div_Single.Visible = true;
        txt_rerollno.Text = "";
        //txt_Narration.Text = "";
        // cb_Narrration.Checked = false;
        txt_rerollno_TextChanged(sender, e);
        div_Multiple.Visible = false;
        txt_tostudentsrcpt.Text = "0";
        imgAlert.Visible = false;
        setLabelText();
    }
    protected void rb_multiple_Change(object sender, EventArgs e)
    {
        div_Single.Visible = false;
        txt_rerollno.Text = "";
        txt_rerollno_TextChanged(sender, e);
        div_Multiple.Visible = true;
        txt_tostudentsrcpt.Text = "0";
        imgAlert.Visible = false;
        setLabelText();
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
                txt_rerollno.Attributes.Add("placeholder", "Roll No");
                lbl_rollno3.Text = "Roll No";
                chosedmode = 0;
                break;
            case 1:
                txt_rerollno.Attributes.Add("placeholder", "Reg No");
                lbl_rollno3.Text = "Reg No";
                chosedmode = 1;
                break;
            case 2:
                txt_rerollno.Attributes.Add("placeholder", "Admin No");
                lbl_rollno3.Text = "Admin No";
                chosedmode = 2;
                break;
            case 3:
                txt_rerollno.Attributes.Add("placeholder", "App No");
                lbl_rollno3.Text = "App No";
                chosedmode = 3;
                break;
        }
        txt_rerollno_TextChanged(sender, e);
    }
    public void LoadFromSettings()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rerollno.Attributes.Add("placeholder", "Roll No");
                    lbl_rollno3.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rerollno.Attributes.Add("placeholder", "Reg No");
                    lbl_rollno3.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rerollno.Attributes.Add("placeholder", "Admin No");
                    lbl_rollno3.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rerollno.Attributes.Add("placeholder", "App No");
                    lbl_rollno3.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void bindGrid(string appno)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("YearSem");
        dt.Columns.Add("Header");
        dt.Columns.Add("HeaderFk");
        dt.Columns.Add("Ledger");
        dt.Columns.Add("LedgerFk");
        dt.Columns.Add("FeeCategory");
        dt.Columns.Add("Concession");
        dt.Columns.Add("Paid");
        dt.Columns.Add("Balance");
        dt.Columns.Add("Total");
        dt.Columns.Add("FeeAmt");

        DataRow dr;
        double total = 0;
        double balance = 0;
        double paid = 0;
        string semYear = string.Empty;
        for (int i = 0; i < cbl_sem.Items.Count; i++)
        {
            if (cbl_sem.Items[i].Selected)
            {
                if (semYear == string.Empty)
                {
                    semYear = Convert.ToString(cbl_sem.Items[i].Value);
                }
                else
                {
                    semYear += "," + Convert.ToString(cbl_sem.Items[i].Value);
                }
            }
        }

        string app_no = appno;
        if (app_no != "" && semYear != string.Empty)
        {
            string selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.BalAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0   and r.App_No=" + app_no + " and r.college_code=" + collegecode1 + " and F.FeeCategory in(" + semYear + ") ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");

            if (ds.Tables.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string feecat = Convert.ToString(ds.Tables[0].Rows[row]["FeeCategory"]);
                    string cursem = d2.GetFunction("select textval from TextValTable where TextCode=" + feecat + " and college_code=" + collegecode1 + "");
                    dr = dt.NewRow();
                    dr["Sno"] = row + 1;
                    dr["YearSem"] = cursem;
                    dr["Header"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderName"]);
                    dr["HeaderFk"] = Convert.ToString(ds.Tables[0].Rows[row]["HeaderFK"]);
                    dr["Ledger"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerName"]);
                    dr["LedgerFk"] = Convert.ToString(ds.Tables[0].Rows[row]["LedgerFK"]);
                    dr["FeeCategory"] = feecat;
                    dr["Concession"] = Convert.ToString(ds.Tables[0].Rows[row]["DeductAmout"]);
                    dr["Paid"] = Convert.ToString(ds.Tables[0].Rows[row]["PaidAmount"]);
                    dr["Balance"] = Convert.ToString(ds.Tables[0].Rows[row]["BalAmount"]);
                    dr["Total"] = Convert.ToString(ds.Tables[0].Rows[row]["TotalAmount"]);
                    dr["FeeAmt"] = Convert.ToString(ds.Tables[0].Rows[row]["FeeAmount"]);
                    dt.Rows.Add(dr);

                    total += Convert.ToDouble(ds.Tables[0].Rows[row]["TotalAmount"]);
                    balance += Convert.ToDouble(ds.Tables[0].Rows[row]["BalAmount"]);
                    paid += Convert.ToDouble(ds.Tables[0].Rows[row]["PaidAmount"]);
                }
            }
        }
        if (dt.Rows.Count > 0)
        {
            gridView3.DataSource = dt;
            gridView3.DataBind();
            lbl_grid3_bal.Text = "Rs." + balance.ToString();
            lbl_grid3_paid.Text = "Rs." + paid.ToString();
            lbl_grid3_tot.Text = "Rs." + total.ToString();
            tblgrid3.Visible = true;
            btn_Print.Visible = true;
        }
        else
        {
            gridView3.DataSource = null;
            gridView3.DataBind();
            tblgrid3.Visible = false;
            btn_Print.Visible = false;
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        //"select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        //student query
        if (chosedmode == 0)
        {
            query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' order by Roll_No asc";
        }
        else if (chosedmode == 1)
        {
            query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%'  order by Reg_No asc ";
        }
        else if (chosedmode == 2)
        {
            query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%'  order by Roll_admit asc ";
        }
        else
        {
            query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%'  order by app_formno asc ";
        }
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetAppFormno(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select top 100 app_formno,app_no from applyn where  app_formno like '" + prefixText + "%' and  isconfirm='1' and isnull(admission_status,'0')='0'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        string query = "select top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";

        studhash = ws.Getnamevalue(query);
        if (studhash.Count > 0)
        {
            foreach (DictionaryEntry p in studhash)
            {
                string studname = Convert.ToString(p.Key);
                name.Add(studname);
            }
        }
        // name = ws.Getname(query);
        return name;
    }
    public void txt_rerollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //txt_Narration.Text = "";
            //cb_Narrration.Checked = false;
            string rollno = Convert.ToString(txt_rerollno.Text);
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,r.app_no   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code ";

            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                //roll no
                query += " and r.Roll_no='" + rollno + "'";
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                //reg no
                query += "  and R.Reg_No = '" + rollno + "' ";
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                //Admin no
                query += " and R.Roll_admit = '" + rollno + "' ";
            }
            else
            {
                query += "  and a.app_formno  = '" + rollno + "' ";
            }

            string appno = string.Empty;
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(query, "Text");
            if (ds1.Tables.Count > 0)
            {
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    //txt_rerollno.Text = ds1.Tables[0].Rows[i]["Roll_no"].ToString();
                    txt_rename.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    txt_rebatch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                    txt_redegree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                    txt_redept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                    txt_resec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                    txt_resem.Text = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                    txt_recolg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                    appno = ds1.Tables[0].Rows[i]["app_no"].ToString();

                }
                image3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                image3.Visible = true;

            }
            if (ds1.Tables.Count == 0 || ds1.Tables[0].Rows.Count == 0 || rollno == "")
            {
                txt_rerollno.Text = "";
                txt_rebatch.Text = "";
                txt_redegree.Text = "";
                txt_redept.Text = "";
                txt_resec.Text = "";
                txt_resem.Text = "";
                txt_recolg.Text = "";
                txt_rename.Text = "";
                image3.ImageUrl = "";
                image3.Visible = false;
            }
            if (rbl_PartFull.SelectedIndex != 0)
            {

                if (appno != "" && appno != "0")
                {
                    string balamt = d2.GetFunction("select sum(isnull(BalAmount,0)) as balAmt from FT_FeeAllot  f,FM_LedgerMaster L where App_No=" + appno + "  and l.ledgerpk=f.LedgerFK  and l.LedgerMode=0");
                    if (balamt != "" && balamt != "0")
                    {
                        if (Convert.ToDouble(balamt) == 0)
                        {
                            bindGrid(appno);
                        }
                        else
                        {
                            gridView3.DataSource = null;
                            gridView3.DataBind();
                            btn_Print.Visible = false;
                            tblgrid3.Visible = false;
                            //imgAlert.Visible = true;
                            //lbl_alert.Text = "Full Amount Not Paid";
                        }
                    }
                    else
                    {
                        gridView3.DataSource = null;
                        gridView3.DataBind();
                        btn_Print.Visible = false;
                        tblgrid3.Visible = false;
                        //imgAlert.Visible = true;
                        //lbl_alert.Text = "Fee Details Not Found";
                    }

                }
                else
                {
                    gridView3.DataSource = null;
                    gridView3.DataBind();
                    btn_Print.Visible = false;
                    tblgrid3.Visible = false;
                    //imgAlert.Visible = true;
                    //lbl_alert.Text = "Roll Number Not Valid";
                }
            }
            else
            {
                bindGrid(appno);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_sem.Text = "Semester/Year";
            if (cb_sem.Checked)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester/Year(" + cbl_sem.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            txt_rerollno_TextChanged(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            txt_sem.Text = "Semester/Year";
            int cnt = 0;
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    cnt++;
                }
            }
            txt_sem.Text = "Semester/Year(" + cnt + ")";
            if (cnt == cbl_sem.Items.Count)
            {
                cb_sem.Checked = true;
            }
            txt_rerollno_TextChanged(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        imgAlert.Visible = false;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    //Lookup
    protected void btn_roll_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindType();
        bindbatch1();
        binddegree2();
        bindbranch1();
        bindsec2();
        txt_rollno3.Text = "";
        //btn_go_Click(sender, e);
        btn_studOK.Visible = false;

        btn_exitstud.Visible = false;
        Fpspread1.Visible = false;
        lbl_errormsg.Visible = false;
        setLabelTextlookup();
    }
    protected void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                Fpspread1.SaveChanges();

                string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
                string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();

                int count = 0;

                for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        count++;
                    }
                }
                txt_tostudentsrcpt.Text = count.ToString();

                popwindow.Visible = false;
                if (count == 0)
                {
                    btn_print2.Visible = false;
                }
                else
                {
                    btn_print2.Visible = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread1.SaveChanges();
            string itemheader = "";
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                }
            }
            string section = "";
            for (int i = 0; i < cbl_sec2.Items.Count; i++)
            {
                if (cbl_sec2.Items[i].Selected == true)
                {
                    if (section == "")
                    {
                        section = "" + cbl_sec2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        section = section + "'" + "," + "" + "'" + cbl_sec2.Items[i].Value.ToString() + "";
                    }
                }
            }


            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);

            string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY len(r.Roll_No),r.Roll_No";
                }
                else if (strorderby == "1")
                {
                    strorderby = "ORDER BY len(r.Reg_No),r.Reg_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY len(r.Roll_No),r.Roll_No,r.Stud_Name";
                }
                else
                {
                    strorderby = "";
                }
            }


            if (txt_rollno3.Text == "")
            {
                selectquery = "select Roll_No,Roll_Admit,R.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No,a.app_formno  from Registration r,applyn a,Degree d,Department dt,Course c where  r.App_No=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and R.Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  and r.Sections in ('" + section + "') ";
                selectquery += strorderby;
            }
            else
            {
                selectquery = "select Roll_No,Roll_Admit,R.Stud_Name,d.Degree_Code,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No,a.app_formno  from Registration r,applyn a,Degree d,Department dt,Course c where  r.App_No=a.app_no and r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' ";
                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    selectquery += " and r.Roll_no='" + txt_rollno3.Text + "'";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    selectquery += "  and R.Reg_No = '" + txt_rollno3.Text + "' ";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    selectquery += " and R.Roll_admit = '" + txt_rollno3.Text + "' ";
                }
                else
                {
                    selectquery += "  and a.app_formno  = '" + txt_rollno3.Text + "' ";
                }

                selectquery += strorderby;
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 8;

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 80;
                Fpspread1.Sheets[0].Columns[1].Locked = false;

                //Fpspread1.Sheets[0].Columns[1].Visible = true;


                Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll Admit";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "App No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Columns[5].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Columns[6].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Columns[7].Width = 250;

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }
                else
                {
                    //App no

                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = true;
                }

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    //
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = txtAppno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["app_formno"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                }
                Fpspread1.SaveChanges();
                Fpspread1.Visible = true;
                lbl_errormsg.Visible = false;


                Fpspread1.SaveChanges();
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
            }
            else
            {
                Fpspread1.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        txt_tostudentsrcpt.Text = "0";
    }
    public void bindType()
    {
        try
        {
            cbl_strm.Items.Clear();
            cb_strm.Checked = false;
            txt_strm.Text = "--Select--";
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and type<>'' order by type asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_strm.DataSource = ds;
                cbl_strm.DataTextField = "type";
                cbl_strm.DataValueField = "type";
                cbl_strm.DataBind();
                if (cbl_strm.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_strm.Items.Count; i++)
                    {
                        cbl_strm.Items[i].Selected = true;
                    }
                    txt_strm.Text = "Stream(" + cbl_strm.Items.Count + ")";
                    cb_strm.Checked = true;
                }
                txt_strm.Enabled = true;
            }
            else
            {
                txt_strm.Enabled = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cb_strm_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_strm.Text = "---Select---";
            if (cb_strm.Checked)
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    cbl_strm.Items[i].Selected = true;
                }
                txt_strm.Text = "Stream (" + cbl_strm.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    cbl_strm.Items[i].Selected = false;

                }
            }
            binddegree2();
            bindbranch1();
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cbl_strm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cnt = 0;
            txt_strm.Text = "---Select---";
            cb_strm.Checked = false;

            for (int i = 0; i < cbl_strm.Items.Count; i++)
            {
                if (cbl_strm.Items[i].Selected == true)
                {
                    cnt++;
                    txt_strm.Text = "Stream (" + cnt + ")";
                }
            }
            if (cnt == cbl_strm.Items.Count)
            {
                cb_strm.Checked = true;
            }
            binddegree2();
            bindbranch1();
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch2.Text = "--Select--";
            cb_branch1.Checked = false;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = lbl_branch2.Text + "(" + commcount.ToString() + ")";
                if (commcount == cbl_branch1.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch1.Checked == true)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch2.Text = lbl_branch2.Text + "(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree2.Checked = false;

            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }

            if (seatcount == cbl_degree2.Items.Count)
            {
                txt_degree2.Text = lbl_degree2.Text + "(" + seatcount.ToString() + ")";
                cb_degree2.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree2.Text = "--Select--";
            }
            else
            {
                txt_degree2.Text = lbl_degree2.Text + "(" + seatcount.ToString() + ")";
            }
            bindbranch1();
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree2.Checked == true)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    if (cb_degree2.Checked == true)
                    {
                        cbl_degree2.Items[i].Selected = true;
                        txt_degree2.Text = lbl_degree2.Text + "(" + (cbl_degree2.Items.Count) + ")";

                    }
                }

            }
            else
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = false;
                    txt_degree2.Text = "--Select--";
                    txt_branch2.Text = "--Select--";
                    cbl_branch1.ClearSelection();
                    cb_branch1.Checked = false;
                }
            }
            bindbranch1();
            bindsec2();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec2.Checked == true)
            {
                for (int i = 0; i < cbl_sec2.Items.Count; i++)
                {

                    cbl_sec2.Items[i].Selected = true;
                    txt_sec2.Text = "Section(" + (cbl_sec2.Items.Count) + ")";
                }

            }
            else
            {
                for (int i = 0; i < cbl_sec2.Items.Count; i++)
                {
                    cbl_sec2.Items[i].Selected = false;

                }
                txt_sec2.Text = "--Select--";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_sec2.Checked = false;
            txt_sec2.Text = "--Select--";

            for (int i = 0; i < cbl_sec2.Items.Count; i++)
            {
                if (cbl_sec2.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }

            if (seatcount == cbl_sec2.Items.Count)
            {
                txt_sec2.Text = "Section(" + seatcount.ToString() + ")";
                cb_sec2.Checked = true;
            }
            else
            {
                txt_sec2.Text = "Section(" + seatcount.ToString() + ")";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            if (cbl_strm.Items.Count > 0)
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    if (cbl_strm.Items[i].Selected == true)
                    {
                        if (stream == "")
                        {
                            stream = Convert.ToString(cbl_strm.Items[i].Value);
                        }
                        else
                        {
                            stream = stream + "'" + "," + "'" + Convert.ToString(cbl_strm.Items[i].Value);
                        }
                    }
                }
            }
            txt_degree2.Text = "--Select--";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            if (txt_strm.Enabled)
            {
                query += "  and course.type in ('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = lbl_degree2.Text + "(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        // branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                        branch += "," + cbl_degree2.Items[i].Value.ToString();
                    }
                }
            }
            string commname = "";
            //if (branch != "")
            //{
            //    commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            //}
            //else
            //{
            //    commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            //}
            if (branch.Trim() != "")
            {
               // ds = d2.select_method_wo_parameter(commname, "Text");
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, branch, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = lbl_branch2.Text + "(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void bindsec2()
    {
        try
        {
            cbl_sec2.Items.Clear();
            txt_sec2.Text = "--Select--";
            ListItem item = new ListItem("Empty", " ");
            if (ddl_batch1.Items.Count > 0)
            {
                string strbatch = Convert.ToString(ddl_batch1.SelectedItem.Value);
                string branch = "";
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "" + "," + "" + "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (branch != "")
                {
                    DataSet dsSec = d2.BindSectionDetail(strbatch, branch);
                    if (dsSec.Tables.Count > 0)
                    {
                        if (dsSec.Tables[0].Rows.Count > 0)
                        {
                            cbl_sec2.DataSource = dsSec;
                            cbl_sec2.DataTextField = "sections";
                            cbl_sec2.DataValueField = "sections";
                            cbl_sec2.DataBind();


                        }
                    }
                    cbl_sec2.Items.Insert(0, item);
                    for (int i = 0; i < cbl_sec2.Items.Count; i++)
                    {
                        cbl_sec2.Items[i].Selected = true;
                    }
                    cb_sec2.Checked = true;
                    txt_sec2.Text = "Section(" + cbl_sec2.Items.Count + ")";

                }
            }


        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public void LoadYearSemester()
    {
        try
        {
            ddl_semrcpt.Items.Clear();
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            string linkvalue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");

            if (linkvalue != "")
            {
                DataSet dsSemYear = new DataSet();
                string query = "";
                string semyear = "select Linkvalue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";

                if (d2.GetFunction(semyear).Trim() == "1")
                {
                    query = "selECT	* from textvaltable where TextCriteria ='FEECA' and (textval like '%Semester' or textval like '%Year')  and college_code=" + collegecode1 + " order by len(textval),textval asc";
                }
                else
                {
                    if (linkvalue == "0")
                    {
                        query = "selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '% semester' and college_code=" + collegecode1 + " order by len(textval),textval asc";
                    }
                    else
                    {
                        query = " selECT	* from textvaltable where TextCriteria ='FEECA' and textval like '% Year' and college_code=" + collegecode1 + " order by len(textval),textval asc";
                    }
                }
                dsSemYear = d2.select_method_wo_parameter(query, "Text");
                if (dsSemYear.Tables[0].Rows.Count > 0)
                {
                    ddl_semrcpt.DataSource = dsSemYear;
                    ddl_semrcpt.DataTextField = "TextVal";
                    ddl_semrcpt.DataValueField = "TextCode";
                    ddl_semrcpt.DataBind();

                    cbl_sem.DataSource = dsSemYear;
                    cbl_sem.DataTextField = "TextVal";
                    cbl_sem.DataValueField = "TextCode";
                    cbl_sem.DataBind();


                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                    }
                    txt_sem.Text = "Semester/Year(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    public string generateReceiptNo(out string rcptacr)
    {
        string recno = string.Empty;
        rcptacr = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);

            string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            DataSet dsrecYr = new DataSet();
            dsrecYr = d2.select_method_wo_parameter(secondreciptqurey, "Text");
            if (dsrecYr.Tables[0].Rows.Count > 0)
            {
                recnoprev = Convert.ToString(dsrecYr.Tables[0].Rows[0][0]);
                if (recnoprev != "")
                {
                    int recno_cur = Convert.ToInt32(recnoprev);
                    receno = recno_cur;
                }

                string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recenoString;
                rcptacr = recacr;
            }

            return recno;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); return recno; }
    }
    //Print Area
    protected void btn_Print_Click(object sender, EventArgs e)
    {
        try
        {
            if (gridView3.Rows.Count > 0)
            {
                //Document Settings
                PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(15.2, 20.2));
                Font Fontboldhead = new Font("Book Antiqua", 12, FontStyle.Bold);
                Font FontNorm = new Font("Book Antiqua", 12, FontStyle.Regular);
                Font FontTableHead = new Font("Book Antiqua", 12, FontStyle.Bold);
                Font FontTable = new Font("Book Antiqua", 12, FontStyle.Regular);
                bool createPDF = false;

                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();

                string studname = string.Empty;
                string rollno = string.Empty;
                string deg = string.Empty;
                string curYr = string.Empty;
                string rcptacr = string.Empty;
                string rcptno = generateReceiptNo(out rcptacr);
                string receiptno = rcptacr + rcptno;
                string appno = string.Empty;
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                try
                {
                    rollno = txt_rerollno.Text.Trim();
                    studname = txt_rename.Text.Trim().Split('-')[0];
                    deg = txt_redegree.Text.Trim() + "-" + txt_redept.Text.Trim();

                    string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.dept_acronym, dt.Dept_Name,C.type,a.app_no,r.Current_Semester   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code ";
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        query += " and r.Roll_no='" + rollno + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        query += "  and R.Reg_No = '" + rollno + "' ";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        query += " and R.Roll_admit = '" + rollno + "' ";
                    }
                    else
                    {
                        query += "  and a.app_formno  = '" + rollno + "' ";
                    }

                    ds1 = d2.select_method_wo_parameter(query, "Text");
                    string app_no = string.Empty;
                    if (ds1.Tables.Count > 0)
                    {
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            studname = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                            deg = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + "-" + Convert.ToString(ds1.Tables[0].Rows[0]["dept_acronym"]);
                            app_no = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]).Trim();
                            curYr = romanLetter(returnYearforSem(Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]))) + " Year ";
                            //deg = curYr + deg;
                        }
                    }

                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        appno = d2.GetFunction("select app_no from registration where Roll_no='" + rollno + "'");
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        appno = d2.GetFunction("select app_no from registration where Reg_No='" + rollno + "'");
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        appno = d2.GetFunction("select app_no from registration where Roll_admit='" + rollno + "'");
                    }
                    else
                    {
                        appno = d2.GetFunction("select app_no from applyn where app_formno='" + rollno + "'");
                    }
                }
                catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }

                PdfPage rcptpage = recptDoc.NewPage();

                //sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");
                sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");

                PdfTextArea dateText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 350, 110, 50, 20), ContentAlignment.MiddleLeft, txt_rdate.Text.Trim());
                rcptpage.Add(dateText);
                PdfTextArea rcptNoText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 55, 120, 200, 20), ContentAlignment.MiddleLeft, "Receipt No. " + receiptno);
                rcptpage.Add(rcptNoText);




                PdfTable tableparts = recptDoc.NewTable(FontTableHead, 3, 1, 7);
                tableparts.VisibleHeaders = false;
                tableparts.Cell(0, 0).SetContent(studname.ToUpper());
                tableparts.Cell(0, 0).SetFont(FontTableHead);
                tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(1, 0).SetContent(rollno.ToUpper());
                tableparts.Cell(1, 0).SetFont(FontTableHead);
                tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                tableparts.Cell(2, 0).SetContent(deg.ToUpper());
                tableparts.Cell(2, 0).SetFont(FontTableHead);
                tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 150, 135, 300, 200));
                rcptpage.Add(addtabletopage1);

                double total = 0;
                int rows = 0;
                //Count Rows
                bool AddYearOk = true;
                foreach (GridViewRow gRow in gridView3.Rows)
                {
                    Label lblpaid = (Label)gRow.FindControl("lbl_paid");
                    CheckBox cbSel = (CheckBox)gRow.FindControl("cb_Sel");
                    if (lblpaid.Text != "" && cbSel.Checked)
                    {
                        double paidAmt = Convert.ToDouble(lblpaid.Text);
                        if (paidAmt > 0)
                        {
                            if (AddYearOk)
                            {
                                Label lblfeecatName = (Label)gRow.FindControl("lbl_yearsem");
                                try
                                {
                                    deg = romanLetter(lblfeecatName.Text.Split(' ')[0]) + " " + lblfeecatName.Text.Split(' ')[1] + " " + deg;
                                }
                                catch { deg = curYr + deg; }
                                AddYearOk = false;
                            }
                            rows++;
                        }
                    }
                }

                //sbHtml.Append("<table class='classBold12' style='width:430px; height:50px;' cellpadding='5'><tr><td style='padding-left:360px; padding-top:84px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");
                sbHtml.Append("<table class='classBold12' style='width:460px; height:60px;' cellpadding='7'><tr><td style='padding-left:260px; padding-top:70px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;padding-top:-650px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");


                PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 3, 5);
                tableparts1.VisibleHeaders = false;
                tableparts1.Columns[0].SetWidth(204);
                tableparts1.Columns[1].SetWidth(62);
                tableparts1.Columns[2].SetWidth(28);

                //sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:56px;'><table class='classBold12' cellpadding='4' >");
                sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:80px;'><table class='classBold12' cellpadding='4' >");
                int indx = 0;
                foreach (GridViewRow gRow in gridView3.Rows)
                {
                    Label lblhdrid = (Label)gRow.FindControl("lbl_hdrid");
                    Label lblhdrname = (Label)gRow.FindControl("lbl_hdr");
                    Label lbllgrid = (Label)gRow.FindControl("lbl_lgrid");
                    Label lbllgrname = (Label)gRow.FindControl("lbl_lgr");
                    Label lblfeecat = (Label)gRow.FindControl("lbl_feecat");

                    Label lblpaid = (Label)gRow.FindControl("lbl_paid");
                    CheckBox cbSel = (CheckBox)gRow.FindControl("cb_Sel");

                    if (lblpaid.Text != "" && cbSel.Checked)
                    {
                        double paidAmt = Convert.ToDouble(lblpaid.Text);
                        total += paidAmt;
                        if (paidAmt > 0)
                        {
                            createPDF = true;
                            //tableparts1.Cell(indx, 0).SetContent(lbllgrname.Text);
                            //tableparts1.Cell(indx, 0).SetFont(FontTable);
                            //tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tableparts1.Cell(indx, 1).SetContent(returnIntegerPart(paidAmt));
                            tableparts1.Cell(indx, 1).SetFont(FontTable);
                            tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                            tableparts1.Cell(indx, 2).SetContent(returnDecimalPart(paidAmt));
                            tableparts1.Cell(indx, 2).SetFont(FontTable);
                            tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                            indx++;

                            //sbHtml.Append("<tr><td style='padding-left:235px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");
                            sbHtml.Append("<tr><td style='padding-left:290px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                            #region update ReceiptNumber
                            string upQ = " update ft_findailytransaction set TransCode='" + receiptno + "' where app_no='" + appno + "' and ledgerfk='" + lbllgrid.Text + "' and Headerfk='" + lblhdrid.Text + "' and Feecategory='" + lblfeecat.Text + "'   and isnull(Iscanceled,0)=0";
                            d2.update_method_wo_parameter(upQ, "Text");
                            #endregion
                        }
                    }
                }
                //sbHtml.Append("</table></div>");

                //sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:5px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:245px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:20px;'>&nbsp;&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div></center></div>");

                sbHtml.Append("</table></div>");
                sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:10px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:280px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:25px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");


                PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 45, 232, 346, 218));
                rcptpage.Add(addtabletopage2);

                PdfTextArea amtWords = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 50, 400, 250, 100), ContentAlignment.MiddleLeft, DecimalToWords((decimal)total) + " Rupees Only.");
                rcptpage.Add(amtWords);

                PdfTable tableparts3 = recptDoc.NewTable(FontTable, 1, 3, 5);
                tableparts3.VisibleHeaders = false;

                tableparts3.Columns[0].SetWidth(204);
                tableparts3.Columns[1].SetWidth(62);
                tableparts3.Columns[2].SetWidth(28);

                tableparts3.Cell(0, 0).SetContent(" ");

                tableparts3.Cell(0, 1).SetContent(returnIntegerPart(total));
                tableparts3.Cell(0, 1).SetFont(FontTable);
                tableparts3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                tableparts3.Cell(0, 2).SetContent(returnDecimalPart(total));
                tableparts3.Cell(0, 2).SetFont(FontTable);
                tableparts3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                PdfTablePage addtabletopage3 = tableparts3.CreateTablePage(new PdfArea(recptDoc, 45, 450, 346, 28));
                rcptpage.Add(addtabletopage3);


                //sbHtml.Append("</td></tr></table></div>");
                sbHtml.Append("</td></tr></table></div></center></div>");
                contentDiv.InnerHtml += sbHtml.ToString();
                rcptpage.SaveToDocument();

                #region Update Receipt No

                string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + rcptno + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                d2.update_method_wo_parameter(updateRecpt, "Text");

                #endregion
                if (createPDF)
                {
                    //txt_Narration.Text = "";
                    //cb_Narrration.Checked = false;
                    #region New Print
                    //contentDiv.InnerHtml += sbHtml.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                    #endregion
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Ledgers Available To Print";
                }
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "No Ledgers Available To Print";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }
    }
    protected void btn_print2_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_tostudentsrcpt.Text != "" && txt_tostudentsrcpt.Text != "0")
            {
                //Document Settings
                PdfDocument recptDoc = new PdfDocument(PdfDocumentFormat.InCentimeters(15.2, 20.2));
                Font Fontboldhead = new Font("Book Antiqua", 12, FontStyle.Bold);
                Font FontNorm = new Font("Book Antiqua", 12, FontStyle.Regular);
                Font FontTableHead = new Font("Book Antiqua", 12, FontStyle.Bold);
                Font FontTable = new Font("Book Antiqua", 12, FontStyle.Regular);
                bool createPDF = false;

                contentDiv.InnerHtml = "";
                StringBuilder sbHtml = new StringBuilder();


                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    sbHtml.Clear();
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        #region For Every Student
                        string rcptacr = string.Empty;
                        string rcptno = generateReceiptNo(out rcptacr);
                        string receiptno = rcptacr + rcptno;

                        string rollno = string.Empty;

                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            //roll no
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 3].Text);
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            //reg no
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 4].Text);
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            //Admin no
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Text);
                        }
                        else
                        {
                            rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 5].Text);
                        }

                        string studname = string.Empty;
                        string deg = string.Empty;
                        string curYr = string.Empty;

                        string query = "select a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name, dt.dept_acronym ,dt.Dept_Name,C.type,a.app_no,r.Current_Semester   from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code ";
                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            //roll no
                            query += " and r.Roll_no='" + rollno + "'";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            //reg no
                            query += "  and R.Reg_No = '" + rollno + "' ";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            //Admin no
                            query += " and R.Roll_admit = '" + rollno + "' ";
                        }
                        else
                        {
                            query += "  and a.app_formno  = '" + rollno + "' ";
                        }

                        ds1 = d2.select_method_wo_parameter(query, "Text");
                        string app_no = string.Empty;
                        if (ds1.Tables.Count > 0)
                        {
                            if (ds1.Tables[0].Rows.Count > 0)
                            {
                                studname = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                                deg = Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]) + "-" + Convert.ToString(ds1.Tables[0].Rows[0]["dept_acronym"]);
                                app_no = Convert.ToString(ds1.Tables[0].Rows[0]["app_no"]).Trim();
                                curYr = romanLetter(returnYearforSem(Convert.ToString(ds1.Tables[0].Rows[0]["Current_Semester"]).Trim())) + " Year ";
                                //deg = curYr + deg;

                                try
                                {
                                    deg = romanLetter(ddl_semrcpt.SelectedItem.Text.Split(' ')[0]) + " " + ddl_semrcpt.SelectedItem.Text.Split(' ')[1] + " " + deg;
                                }
                                catch { deg = curYr + deg; }
                            }
                        }

                        if (rbl_PartFull.SelectedIndex != 0)
                        {
                            string appno = app_no;
                            if (appno != "" && appno != "0")
                            {
                                string balamt = d2.GetFunction("select sum(isnull(BalAmount,0)) as balAmt from FT_FeeAllot  f,FM_LedgerMaster L where App_No=" + appno + "  and l.ledgerpk=f.LedgerFK  and l.LedgerMode=0  ");
                                if (balamt != "" && balamt != "0")
                                {
                                    if (Convert.ToDouble(balamt) == 0)
                                    {
                                        //sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");
                                        sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");
                                        #region Create Document
                                        PdfPage rcptpage = recptDoc.NewPage();

                                        PdfTextArea dateText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 350, 110, 50, 20), ContentAlignment.MiddleLeft, txt_rdate.Text.Trim());
                                        rcptpage.Add(dateText);
                                        PdfTextArea rcptNoText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 55, 120, 200, 20), ContentAlignment.MiddleLeft, "Receipt No." + receiptno);
                                        rcptpage.Add(rcptNoText);

                                        PdfTable tableparts = recptDoc.NewTable(FontTableHead, 3, 1, 7);
                                        tableparts.VisibleHeaders = false;
                                        tableparts.Cell(0, 0).SetContent(studname.ToUpper());
                                        tableparts.Cell(0, 0).SetFont(FontTableHead);
                                        tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                        tableparts.Cell(1, 0).SetContent(rollno.ToUpper());
                                        tableparts.Cell(1, 0).SetFont(FontTableHead);
                                        tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                        tableparts.Cell(2, 0).SetContent(deg.ToUpper());
                                        tableparts.Cell(2, 0).SetFont(FontTableHead);
                                        tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                        PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 150, 135, 300, 200));
                                        rcptpage.Add(addtabletopage1);
                                        //sbHtml.Append("<table class='classBold12' style='width:430px; height:50px;' cellpadding='5'><tr><td style='padding-left:360px; padding-top:84px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

                                        sbHtml.Append("<table class='classBold12' style='width:460px; height:60px;' cellpadding='7'><tr><td style='padding-left:260px; padding-top:70px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;padding-top:-650px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

                                        if (app_no != "" && app_no != "0")
                                        {
                                            string selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.BalAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0   and r.App_No=" + app_no + " and r.college_code=" + collegecode1 + " ";
                                            DataSet dsPaid = new DataSet();
                                            dsPaid = d2.select_method_wo_parameter(selectQ, "Text");

                                            if (dsPaid.Tables.Count > 0)
                                            {
                                                if (dsPaid.Tables[0].Rows.Count > 0)
                                                {
                                                    int rows = dsPaid.Tables[0].Rows.Count;
                                                    double total = 0;
                                                    PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 3, 5);
                                                    tableparts1.VisibleHeaders = false;
                                                    tableparts1.Columns[0].SetWidth(204);
                                                    tableparts1.Columns[1].SetWidth(62);
                                                    tableparts1.Columns[2].SetWidth(28);
                                                    int indx = 0;
                                                    //sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:56px;'><table class='classBold12' cellpadding='4' >");

                                                    sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:80px;'><table class='classBold12' cellpadding='4' >");

                                                    for (int row = 0; row < dsPaid.Tables[0].Rows.Count; row++)
                                                    {
                                                        string ledgername = Convert.ToString(dsPaid.Tables[0].Rows[row]["LedgerName"]);
                                                        string ledgerid = Convert.ToString(dsPaid.Tables[0].Rows[row]["LedgerFK"]);
                                                        string hdrid = Convert.ToString(dsPaid.Tables[0].Rows[row]["HeaderFK"]);
                                                        string feecat = Convert.ToString(dsPaid.Tables[0].Rows[row]["FeeCategory"]);
                                                        double paidAmt = Convert.ToDouble(dsPaid.Tables[0].Rows[row]["PaidAmount"]);

                                                        total += paidAmt;
                                                        if (paidAmt > 0)
                                                        {
                                                            createPDF = true;
                                                            //tableparts1.Cell(indx, 0).SetContent(ledgername);
                                                            //tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                            //tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                            tableparts1.Cell(indx, 1).SetContent(returnIntegerPart(paidAmt));
                                                            tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                            tableparts1.Cell(indx, 2).SetContent(returnDecimalPart(paidAmt));
                                                            tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                            tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                            indx++;
                                                            //sbHtml.Append("<tr><td style='padding-left:235px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                                                            sbHtml.Append("<tr><td style='padding-left:290px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                                                            #region update ReceiptNumber
                                                            string upQ = " update ft_findailytransaction set TransCode='" + receiptno + "' where app_no='" + app_no + "' and ledgerfk='" + ledgerid + "' and Headerfk='" + hdrid + "' and Feecategory='" + feecat + "' and isnull(Iscanceled,0)=0 ";
                                                            d2.update_method_wo_parameter(upQ, "Text");
                                                            #endregion
                                                        }
                                                    }

                                                    //sbHtml.Append("</table></div>");
                                                    //sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:5px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:245px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:20px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");

                                                    sbHtml.Append("</table></div>");
                                                    sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:10px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:280px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:25px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");

                                                    PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 45, 232, 346, 218));
                                                    rcptpage.Add(addtabletopage2);

                                                    PdfTextArea amtWords = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 50, 400, 250, 100), ContentAlignment.MiddleLeft, DecimalToWords((decimal)total) + " Rupees Only.");
                                                    rcptpage.Add(amtWords);

                                                    PdfTable tableparts3 = recptDoc.NewTable(FontTable, 1, 3, 5);
                                                    tableparts3.VisibleHeaders = false;

                                                    tableparts3.Columns[0].SetWidth(204);
                                                    tableparts3.Columns[1].SetWidth(62);
                                                    tableparts3.Columns[2].SetWidth(28);

                                                    tableparts3.Cell(0, 0).SetContent(" ");

                                                    tableparts3.Cell(0, 1).SetContent(returnIntegerPart(total));
                                                    tableparts3.Cell(0, 1).SetFont(FontTable);
                                                    tableparts3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    tableparts3.Cell(0, 2).SetContent(returnDecimalPart(total));
                                                    tableparts3.Cell(0, 2).SetFont(FontTable);
                                                    tableparts3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                    PdfTablePage addtabletopage3 = tableparts3.CreateTablePage(new PdfArea(recptDoc, 45, 450, 346, 28));
                                                    rcptpage.Add(addtabletopage3);
                                                    rcptpage.SaveToDocument();
                                                    #region Update Receipt No

                                                    string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + rcptno + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                                    d2.update_method_wo_parameter(updateRecpt, "Text");

                                                    #endregion
                                                }
                                            }
                                        }

                                        #endregion
                                        //sbHtml.Append("</td></tr></table></div></center></div>");
                                        sbHtml.Append("</td></tr></table></div></center></div>");
                                        contentDiv.InnerHtml += sbHtml.ToString();
                                    }
                                    else
                                    {
                                        gridView3.DataSource = null;
                                        gridView3.DataBind();
                                        btn_Print.Visible = false;
                                        tblgrid3.Visible = false;
                                        imgAlert.Visible = true;
                                        lbl_alert.Text = "Full Amount Not Paid";
                                    }
                                }
                                else
                                {
                                    gridView3.DataSource = null;
                                    gridView3.DataBind();
                                    btn_Print.Visible = false;
                                    tblgrid3.Visible = false;
                                    imgAlert.Visible = true;
                                    lbl_alert.Text = "Fee Details Not Found";
                                }

                            }
                            else
                            {
                                gridView3.DataSource = null;
                                gridView3.DataBind();
                                btn_Print.Visible = false;
                                tblgrid3.Visible = false;
                                imgAlert.Visible = true;
                                lbl_alert.Text = "Roll Number Not Valid";
                            }
                        }
                        else
                        {
                            #region Create Document
                            PdfPage rcptpage = recptDoc.NewPage();

                            //sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");

                            sbHtml.Append("<div style='padding-left:5px;height: 900px; width:595px;'><center><div style='padding-left:130px;height: 763.5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 574.5px; ' class='classBold12'><tr><td>");

                            PdfTextArea dateText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 350, 110, 50, 20), ContentAlignment.MiddleLeft, txt_rdate.Text.Trim());
                            rcptpage.Add(dateText);
                            PdfTextArea rcptNoText = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 55, 120, 200, 20), ContentAlignment.MiddleLeft, "Receipt No." + receiptno);
                            rcptpage.Add(rcptNoText);

                            PdfTable tableparts = recptDoc.NewTable(FontTableHead, 3, 1, 7);
                            tableparts.VisibleHeaders = false;
                            tableparts.Cell(0, 0).SetContent(studname.ToUpper());
                            tableparts.Cell(0, 0).SetFont(FontTableHead);
                            tableparts.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tableparts.Cell(1, 0).SetContent(rollno.ToUpper());
                            tableparts.Cell(1, 0).SetFont(FontTableHead);
                            tableparts.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            tableparts.Cell(2, 0).SetContent(deg.ToUpper());
                            tableparts.Cell(2, 0).SetFont(FontTableHead);
                            tableparts.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                            PdfTablePage addtabletopage1 = tableparts.CreateTablePage(new PdfArea(recptDoc, 150, 135, 300, 200));
                            rcptpage.Add(addtabletopage1);
                            //sbHtml.Append("<table class='classBold12' style='width:430px; height:50px;' cellpadding='5'><tr><td style='padding-left:360px; padding-top:84px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

                            sbHtml.Append("<table class='classBold12' style='width:460px; height:60px;' cellpadding='7'><tr><td style='padding-left:260px; padding-top:70px; text-align:right;'><BR>" + txt_rdate.Text.Trim() + "</td></tr><tr><td style='padding-left:0px;'>Receipt No. " + receiptno + "</td></tr><tr><td style='padding-left:150px;padding-top:-650px;'>" + studname.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + rollno.ToUpper() + "</td></tr><tr><td style='padding-left:150px;'>" + deg.ToUpper() + "</td></tr></table>");

                            if (app_no != "" && app_no != "0")
                            {
                                string selectQ = " select f.HeaderFK,h.HeaderName,f.LedgerFK,l.LedgerName,F.FeeCategory,isnull(F.DeductAmout,0) as DeductAmout,isnull(F.PaidAmount,0) as PaidAmount,isnull(F.BalAmount,0) as BalAmount,isnull(F.TotalAmount,0) as TotalAmount,isnull(F.FeeAmount,0) as FeeAmount   from FT_FeeAllot f,Registration R,FM_HeaderMaster H,FM_LedgerMaster L where r.app_no=f.App_No and F.HeaderFK=H.HeaderPK and L.HeaderFK=H.HeaderPK and L.LedgerPK=F.LedgerFK and r.college_code=h.CollegeCode and L.CollegeCode=r.college_code and isnull(PaidAmount,0)<>0   and r.App_No=" + app_no + " and r.college_code=" + collegecode1 + " ";
                                DataSet dsPaid = new DataSet();
                                dsPaid = d2.select_method_wo_parameter(selectQ, "Text");

                                if (dsPaid.Tables.Count > 0)
                                {
                                    if (dsPaid.Tables[0].Rows.Count > 0)
                                    {
                                        int rows = dsPaid.Tables[0].Rows.Count;
                                        double total = 0;
                                        PdfTable tableparts1 = recptDoc.NewTable(FontTable, rows, 3, 5);
                                        tableparts1.VisibleHeaders = false;
                                        tableparts1.Columns[0].SetWidth(204);
                                        tableparts1.Columns[1].SetWidth(62);
                                        tableparts1.Columns[2].SetWidth(28);
                                        int indx = 0;
                                        //sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:56px;'><table class='classBold12' cellpadding='4' >");

                                        sbHtml.Append("<div style='width:380px; height:215px; padding-left:80px; padding-top:80px;'><table class='classBold12' cellpadding='4' >");

                                        for (int row = 0; row < dsPaid.Tables[0].Rows.Count; row++)
                                        {
                                            string ledgername = Convert.ToString(dsPaid.Tables[0].Rows[row]["LedgerName"]);
                                            string ledgerid = Convert.ToString(dsPaid.Tables[0].Rows[row]["LedgerFK"]);
                                            string hdrid = Convert.ToString(dsPaid.Tables[0].Rows[row]["HeaderFK"]);
                                            string feecat = Convert.ToString(dsPaid.Tables[0].Rows[row]["FeeCategory"]);
                                            double paidAmt = Convert.ToDouble(dsPaid.Tables[0].Rows[row]["PaidAmount"]);

                                            total += paidAmt;
                                            if (paidAmt > 0)
                                            {
                                                createPDF = true;
                                                //tableparts1.Cell(indx, 0).SetContent(ledgername);
                                                //tableparts1.Cell(indx, 0).SetFont(FontTable);
                                                //tableparts1.Cell(indx, 0).SetContentAlignment(ContentAlignment.MiddleLeft);

                                                tableparts1.Cell(indx, 1).SetContent(returnIntegerPart(paidAmt));
                                                tableparts1.Cell(indx, 1).SetFont(FontTable);
                                                tableparts1.Cell(indx, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                tableparts1.Cell(indx, 2).SetContent(returnDecimalPart(paidAmt));
                                                tableparts1.Cell(indx, 2).SetFont(FontTable);
                                                tableparts1.Cell(indx, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                                                indx++;

                                                //sbHtml.Append("<tr><td style='padding-left:235px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                                                sbHtml.Append("<tr><td style='padding-left:290px; text-align:right; width:60px;'>" + returnIntegerPart(paidAmt) + "</td><td style=' text-align:right; width:30px;'>&nbsp;&nbsp;&nbsp;&nbsp;" + returnDecimalPart(paidAmt) + "</td></tr>");

                                                #region update ReceiptNumber
                                                string upQ = " update ft_findailytransaction set TransCode='" + receiptno + "' where app_no='" + app_no + "' and ledgerfk='" + ledgerid + "' and Headerfk='" + hdrid + "' and Feecategory='" + feecat + "'  and isnull(Iscanceled,0)=0 ";
                                                d2.update_method_wo_parameter(upQ, "Text");
                                                #endregion
                                            }
                                        }

                                        //sbHtml.Append("</table></div>");
                                        //sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:5px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:245px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:20px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");
                                        sbHtml.Append("</table></div>");
                                        sbHtml.Append("<div style='height:60px;' ><table class='classBold12' style='width:380px; padding-left:50px; padding-top:10px;' cellpadding='5'><tr><td style='padding-left:5px;padding-right:70px;'>" + DecimalToWords((decimal)total) + " Rupees Only.</td></tr><tr><td style='padding-left:280px;text-align:right;'><span style='padding-left:50px;text-align:right;width:60px;'>" + returnIntegerPart(total) + "</span></td><td style=' text-align:right; width:30px;padding-left:25px;'>&nbsp;&nbsp;" + returnDecimalPart(total) + "</td></tr></table></div>");

                                        PdfTablePage addtabletopage2 = tableparts1.CreateTablePage(new PdfArea(recptDoc, 45, 232, 346, 218));
                                        rcptpage.Add(addtabletopage2);

                                        PdfTextArea amtWords = new PdfTextArea(Fontboldhead, Color.Black, new PdfArea(recptDoc, 50, 400, 250, 100), ContentAlignment.MiddleLeft, DecimalToWords((decimal)total) + " Rupees Only.");
                                        rcptpage.Add(amtWords);


                                        PdfTable tableparts3 = recptDoc.NewTable(FontTable, 1, 3, 5);
                                        tableparts3.VisibleHeaders = false;

                                        tableparts3.Columns[0].SetWidth(204);
                                        tableparts3.Columns[1].SetWidth(62);
                                        tableparts3.Columns[2].SetWidth(28);

                                        tableparts3.Cell(0, 0).SetContent(" ");

                                        tableparts3.Cell(0, 1).SetContent(returnIntegerPart(total));
                                        tableparts3.Cell(0, 1).SetFont(FontTable);
                                        tableparts3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        tableparts3.Cell(0, 2).SetContent(returnDecimalPart(total));
                                        tableparts3.Cell(0, 2).SetFont(FontTable);
                                        tableparts3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);

                                        PdfTablePage addtabletopage3 = tableparts3.CreateTablePage(new PdfArea(recptDoc, 45, 450, 346, 28));
                                        rcptpage.Add(addtabletopage3);
                                        rcptpage.SaveToDocument();
                                        #region Update Receipt No

                                        string updateRecpt = " update FM_FinCodeSettings set RcptStNo=" + rcptno + "+1 where collegecode =" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings  where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
                                        d2.update_method_wo_parameter(updateRecpt, "Text");

                                        #endregion
                                    }
                                }
                            }

                            //sbHtml.Append("</td></tr></table></div></center></div>");
                            sbHtml.Append("</td></tr></table></div></center></div>");
                            contentDiv.InnerHtml += sbHtml.ToString();

                            #endregion

                        }
                        #endregion

                    }
                }

                #region Print Output
                if (createPDF)
                {
                    #region New Print
                    //contentDiv.InnerHtml += sbHtml.ToString();
                    contentDiv.Visible = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);
                    #endregion
                }
                else
                {
                    imgAlert.Visible = true;
                    lbl_alert.Text = "No Ledgers Available To Print";
                }
                #endregion
            }
            else
            {
                imgAlert.Visible = true;
                lbl_alert.Text = "No Students Selected";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "ReceiptJpr"); }

    }
    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            switch (numeral)
            {
                case "1":
                    romanLettervalue = "I";
                    break;
                case "2":
                    romanLettervalue = "II";
                    break;
                case "3":
                    romanLettervalue = "III";
                    break;
                case "4":
                    romanLettervalue = "IV";
                    break;
                case "5":
                    romanLettervalue = "V";
                    break;
                case "6":
                    romanLettervalue = "VI";
                    break;
                case "7":
                    romanLettervalue = "VII";
                    break;
                case "8":
                    romanLettervalue = "VIII";
                    break;
                case "9":
                    romanLettervalue = "IX";
                    break;
                case "10":
                    romanLettervalue = "X";
                    break;
            }
        }
        return romanLettervalue;
    }
    public string DecimalToWords(decimal number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + DecimalToWords(Math.Abs(number));

        string words = "";

        int intPortion = (int)number;
        decimal fraction = (number - intPortion) * 100;
        int decPortion = (int)fraction;

        words = NumberToWords(intPortion);
        if (decPortion > 0)
        {
            words += " And ";
            words += NumberToWords(decPortion);
            words += " Paise ";
        }
        return words;
    }
    public string NumberToWords(int number)
    {
        if (number == 0)
            return "Zero";

        if (number < 0)
            return "Minus " + NumberToWords(Math.Abs(number));

        string words = "";

        if ((number / 1000000) > 0)
        {
            words += NumberToWords(number / 1000000) + " Million ";
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            words += NumberToWords(number / 1000) + " Thousand ";
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            words += NumberToWords(number / 100) + " Hundred ";
            number %= 100;
        }

        if (number > 0)
        {
            if (words != "")
                words += "and ";

            var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            if (number < 20)
                words += unitsMap[number];
            else
            {
                words += tensMap[number / 10];
                if ((number % 10) > 0)
                    words += " " + unitsMap[number % 10];
            }
        }

        return words;
    }
    public void isContainsDecimal(double myValue)
    {
        bool hasFractionalPart = (myValue - Math.Round(myValue) != 0);
    }
    public string returnIntegerPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 0)
        {
            strVal = strvalArr[0];
        }
        return strVal;
    }
    public string returnDecimalPart(double value)
    {
        string strVal = value.ToString();
        string[] strvalArr = strVal.Split('.');
        if (strvalArr.Length > 1)
        {
            strVal = strvalArr[1];
            if (strVal.Length >= 2)
            {
                strVal = strVal.Substring(0, 2);
            }
            else
            {
                while (2 != strVal.Length)
                {
                    strVal = strVal + "0";
                }
            }
        }
        else
        {
            strVal = "00";
        }
        return strVal;
    }
    //Code Ended by Idhris - Last modified : 22-06-2016

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
        if (rb_single.Checked == true)
        {
            lbl.Add(lblclg);
            lbl.Add(lbldeg);
            lbl.Add(lbldept);
            fields.Add(0);
            fields.Add(2);
            fields.Add(3);
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
        else
        {
            lbl.Add(lbl_semrcpt);
            fields.Add(4);
            new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
        }
    }
    //lockup
    private void setLabelTextlookup()
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
        lbl.Add(lbl_stream);
        lbl.Add(lbl_degree2);
        lbl.Add(lbl_branch2);
        lbl.Add(lblsem);

        fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 04-10-2016 sudhagar
    protected void gridView3_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[2].Text = lblsem.Text;
        }
    }
}