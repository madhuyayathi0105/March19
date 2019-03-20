using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Text;
using System.Net;
using System.Configuration;

public partial class StudentPayment : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    ReuasableMethods reUse = new ReuasableMethods();
    DataSet ds = new DataSet();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static bool usBasedRights = false;
    static int chosedmode = 0;
    static int personmode = 0;
    static double partamt1 = 0;
    static double partamt2 = 0;
    double tobepaidtotalamt = 0;
    double baltotalamt = 0;
    double tottotamt = 0;
    double paitotamt = 0;
    double excessAmt = 0;
    static string collegecodestat = string.Empty;
    static byte BalanceType = 0;

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
                bindclg();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedValue);
                    collegecodestat = Convert.ToString(ddlcollege.SelectedValue);
                }
                LoadFromSettings();
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txt_date1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date1.Attributes.Add("readonly", "readonly");
                txt_rcptno.Text = generateReceiptNo();
                // clear();
                bank();
                bindOtherbankname();
                cardType();
                rcptsngle.Visible = true;
            }
            maintainPaymedeState();
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedValue);
                collegecodestat = Convert.ToString(ddlcollege.SelectedValue);
            }
        }
        catch (Exception ex) { }
    }

    public void maintainPaymedeState()
    {
        div_cheque.Attributes.Add("style", "display:none");
        div_card.Attributes.Add("style", "display:none");
        if (rb_cash.Checked)
        {
            div_cheque.Attributes.Add("style", "display:none");
            div_card.Attributes.Add("style", "display:none");

            tdChQDD.Attributes.Add("style", "display:none");
            tdCard.Attributes.Add("style", "display:none");
            div_NEFT.Attributes.Add("style", "display:none");
        }
        else if (rb_cheque.Checked)
        {
            div_cheque.Attributes.Add("style", "display:block");
            lbl_chqno.Attributes.Add("style", "display:block");
            txt_chqno.Attributes.Add("style", "display:block");

            div_card.Attributes.Add("style", "display:none");
            lbl_ddno.Attributes.Add("style", "display:none");
            txt_ddno.Attributes.Add("style", "display:none");
            txt_ddnar.Attributes.Add("style", "display:none");

            tdChQDD.Attributes.Add("style", "display:block");
            tdCard.Attributes.Add("style", "display:none");
            div_NEFT.Attributes.Add("style", "display:none");
        }
        else if (rb_dd.Checked)
        {
            div_cheque.Attributes.Add("style", "display:block");
            lbl_ddno.Attributes.Add("style", "display:block");
            txt_ddno.Attributes.Add("style", "display:block");
            txt_ddnar.Attributes.Add("style", "display:block");

            div_card.Attributes.Add("style", "display:none");
            lbl_chqno.Attributes.Add("style", "display:none");
            txt_chqno.Attributes.Add("style", "display:none");

            tdChQDD.Attributes.Add("style", "display:block");
            tdCard.Attributes.Add("style", "display:none");
            div_NEFT.Attributes.Add("style", "display:none");
        }
        else if (rb_card.Checked)
        {
            div_cheque.Attributes.Add("style", "display:none");
            div_card.Attributes.Add("style", "display:block");

            tdChQDD.Attributes.Add("style", "display:none");
            tdCard.Attributes.Add("style", "display:block");
            div_NEFT.Attributes.Add("style", "display:none");
        }
        else if (rb_NEFT.Checked)
        {
            div_cheque.Attributes.Add("style", "display:none");
            div_card.Attributes.Add("style", "display:none");

            tdChQDD.Attributes.Add("style", "display:none");
            tdCard.Attributes.Add("style", "display:none");
            div_NEFT.Attributes.Add("style", "display:block");
        }
    }
    protected void ddlcollege_indexChanged(object sender, EventArgs e)
    {
        clear();
        bank();
    }
    public void LoadYearSemester()
    {
        try
        {
            cbl_sem.Items.Clear();
            cb_sem.Checked = false;
            txt_sem.Text = "--Select--";
            string linkName = string.Empty;
            DataSet dsSemYear = new DataSet();
            dsSemYear = d2.loadFeecategory(Convert.ToString(collegecode), usercode, ref linkName);
            if (dsSemYear.Tables.Count > 0)
            {
                if (dsSemYear.Tables[0].Rows.Count > 0)
                {
                    cbl_sem.DataSource = dsSemYear;
                    cbl_sem.DataTextField = "TextVal";
                    cbl_sem.DataValueField = "TextCode";
                    cbl_sem.DataBind();
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                    }
                    txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
                    cb_sem.Checked = true;
                }
            }

        }
        catch (Exception ex) { }
    }
    public void LoadFromSettings()
    {
        try
        {
            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }


            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + collegecode + ")").Trim());

            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard No - smart_serial_no
                rbl_rollno.Items.Add(lst5);
            }

            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    lbl_rollno3.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                case2:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    lbl_rollno3.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                case3:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    lbl_rollno3.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                case4:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    lbl_rollno3.Text = "App No";
                    chosedmode = 3;
                    break;
                case 4:
                    txt_rollno.Attributes.Add("placeholder", "Smartcard No");
                    lbl_rollno3.Text = "SmartCard No";
                    chosedmode = 4;
                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }
        }
        catch (Exception ex) { }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
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
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by Roll_admit asc";
                }
                else if (chosedmode == 4)
                {
                    query = "select  top 100 smart_serial_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by smart_serial_no asc";
                }
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void txt_rollno_Changed(object sender, EventArgs e)
    {
        textRoll();
    }
    private void textRoll()
    {
        string appNo = "-1";
        try
        {
            string sql = "";
            string name = "";
            string degree = "";
            string stType = "";
            string fname = "";
            string query = "";
            DataSet dsload = new DataSet();
            string roll_no = Convert.ToString(txt_rollno.Text.Trim());
            string cursemvalue = "1";
            if (roll_no != "")
            {

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    //Added by saranya//
                    sql = "select * from registration where college_code='" + collegecode + "' ";
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        sql += " and Roll_No like '" + roll_no + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        sql += " and Reg_No like '" + roll_no + "'";
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        sql += " and Roll_Admit like '" + roll_no + "'";
                    }
                    dsload.Clear();
                    dsload = d2.select_method_wo_parameter(sql, "text");
                    //==================================================//
                    if (dsload.Tables[0].Rows.Count > 0)
                    {

                        query = "select r.Roll_No,r.Roll_Admit,r.app_no,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.dept_acronym as Degree,(select TextVal from TextValTable where TextCode=(select seattype from Applyn where app_no=r.app_no) and TextCriteria='seat' ) as StType,(select parent_name from applyn where app_no=r.app_no) as fname, ISNULL( type,'') as type,R.Current_Semester  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.college_code='" + collegecode + "'  ";

                        if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                        {
                            //roll no
                            query += " and r.Roll_No like '" + roll_no + "'";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                        {
                            //reg no
                            query += " and r.Reg_No like '" + roll_no + "'";
                        }
                        else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                        {
                            //Admin no
                            query += " and r.Roll_Admit like '" + roll_no + "'";
                        }
                        //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                        //{
                        //    //Smart card No
                        //    query += " and r.smart_serial_no like '" + txt_Smartno.Text.Trim() + "'";
                        //}
                        //else
                        //{
                        //    query = "";
                        //}
                    }
                    else
                    {
                        query = "select r.app_formno,r.app_no,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.dept_acronym as Degree,(select TextVal from TextValTable where TextCode=(select seattype from Applyn where app_no=r.app_no) and TextCriteria='seat' ) as StType,(select parent_name from applyn where app_no=r.app_no) as fname, ISNULL( type,'') as type,R.Current_Semester  from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.college_code='" + collegecode + "'   and r.app_formno like '" + roll_no + "'";
                    }
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                            stType = Convert.ToString(ds.Tables[0].Rows[i]["stType"]);
                            fname = Convert.ToString(ds.Tables[0].Rows[i]["fname"]);
                            //  lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                            appNo = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                            {
                                cursemvalue = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                            }
                        }
                    }
                }

                txt_name.Text = name;
                txt_dept.Text = degree;
                txt_SeatType.Text = stType;
                txt_FatherName.Text = fname;
                LoadYearSemester();
                loadGridStudent();
            }
            else
            {
                txt_totamt.Text = "0.00";
                txt_paidamt.Text = "0.00";
                txt_balamt.Text = "0.00";



                txt_name.Text = "";
                txt_dept.Text = "";
                txt_SeatType.Text = "";
                txt_FatherName.Text = "";
                grid_Details.DataSource = null;
                grid_Details.DataBind();
                cb_sem.Checked = false;
                cbl_sem.Items.Clear();
                txt_sem.Text = "";

            }
        }
        catch (Exception ex) { }

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
            txt_rollno.Text = roll_no;
            txt_rollno_Changed(sender, e);
        }
        catch (Exception ex) { }
    }

    public void loadGridStudent()
    {
        try
        {
            partamt1 = 0;
            partamt2 = 0;
            btnSave.Visible = false;
            lblStudStatus.Visible = false;
            string ledgerNameScl = string.Empty;
            string finYearFK = string.Empty;
            bool boolSchool = false;
            txt_totamt.Text = "";
            txt_paidamt.Text = "";
            txt_balamt.Text = "";
            txt_rcptno.Text = generateReceiptNo();
            string roll_no = string.Empty;
            string semyear = "";
            string appnoNew = string.Empty;
            string degcode = string.Empty;
            string batchYear = string.Empty;
            string currSem = string.Empty;
            int studemode = 0;
            roll_no = txt_rollno.Text.Trim();
            string excessType = string.Empty;
            string exType = string.Empty;
            string journalType = string.Empty;
            if (rblPaymode.SelectedIndex == 0)
            {
                excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='0'";
                exType = "excesstype = '1'";
                journalType = " ex_journalentry='0'";
            }
            else if (rblPaymode.SelectedIndex == 1)
            {
                excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='1'";
                exType = "excesstype = '1'";
                journalType = " ex_journalentry='1'";
            }
            else
            {
                excessType = " and excesstype='2' and isnull(ex_journalentry,'0')='0'";
                exType = "excesstype = '2'";
                journalType = " ex_journalentry='0'";
            }
            semyear = Convert.ToString(getCblSelectedValue(cbl_sem));
            #region Table Structure and Query
            DataTable tbl_Student = new DataTable();
            DataSet dsload = new DataSet();
            tbl_Student.Columns.Add("TextVal");
            tbl_Student.Columns.Add("app_no");
            tbl_Student.Columns.Add("TextCode");
            tbl_Student.Columns.Add("Header_ID");
            tbl_Student.Columns.Add("Header_Name");
            tbl_Student.Columns.Add("Fee_Code");
            tbl_Student.Columns.Add("Fee_Type");
            tbl_Student.Columns.Add("Total");
            tbl_Student.Columns.Add("finyearfk");
            tbl_Student.Columns.Add("PaidAmt");
            tbl_Student.Columns.Add("BalAmt");
            tbl_Student.Columns.Add("ToBePaid");
            string selectQuery = "";
            string sql = "";
            string queryRollApp = "";
            //Added by saranya//
            sql = "select * from registration where college_code='" + collegecode + "' ";
            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                //roll no
                sql += " and Roll_No like '" + roll_no + "'";
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                //reg no
                sql += " and Reg_No like '" + roll_no + "'";
            }
            else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                //Admin no
                sql += " and Roll_Admit like '" + roll_no + "'";
            }
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(sql, "text");
            //==================================================//
            if (dsload.Tables[0].Rows.Count > 0)
            {
                queryRollApp = "SELECT A.app_no,Roll_No,Reg_No,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,R.Current_Semester,R.Roll_admit,G.degree_code,R.batch_year,r.mode  FROM applyn A,Registration R,Degree G,Course C,Department D WHERE A.app_no = R.App_No AND R.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code  and r.college_code='" + collegecode + "'  and  ";
                //CC=0 and DelFlag =0 and Exam_Flag <>'Debar'  and
                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    queryRollApp += "  Roll_No = '" + roll_no + "' ";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    queryRollApp += "   Reg_No = '" + roll_no + "' ";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    queryRollApp += "  R.Roll_admit = '" + roll_no + "' ";
                }
                else
                {
                    queryRollApp = " SELECT a.app_formno,A.app_no,A.Stud_Name,Course_Name+'-'+Dept_Name Degree,A.Current_Semester,G.degree_code,A.batch_year,a.mode  FROM applyn A,Degree G,Course C,Department D WHERE  A.degree_code = G.Degree_Code AND G.Course_Id = C.Course_Id AND G.college_code = C.college_code AND G.Dept_Code = D.Dept_Code AND G.college_code = D.college_code  and a.college_code='" + collegecode + "'  and   app_formno  = '" + roll_no + "' ";
                }
            }
            else
            {
                queryRollApp = "select r.app_formno as Roll_Admit,r.app_no,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.dept_acronym as Degree,(select TextVal from TextValTable where TextCode=(select seattype from Applyn where app_no=r.app_no) and TextCriteria='seat' ) as StType,(select parent_name from applyn where app_no=r.app_no) as fname, ISNULL( type,'') as type,R.Current_Semester,r.batch_year,r.mode  from applyn r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.college_code='" + collegecode + "'   and r.app_formno like '" + roll_no + "'";
            }
            DataSet dsRollApp = new DataSet();
            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
            if (dsRollApp.Tables.Count > 0)
            {
                if (dsRollApp.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                    {
                        roll_no = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_Admit"]);
                    }
                    else
                    {
                        roll_no = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                    }
                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                    degcode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["degree_code"]);
                    batchYear = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                    currSem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Current_Semester"]);
                    int.TryParse(Convert.ToString(dsRollApp.Tables[0].Rows[0]["mode"]), out studemode);
                }
            }
            // appnoNew = "18270";
            if (checkSchoolSetting() == 0)
            {
                selectQuery = "select h.headername,exl.headerfk,l.ledgername,exl.ledgerfk,t.textval,exl.feecategory,isnull(exl.excessamt,'0') as excessamt,isnull(exl.adjamt,'0') as adjamt,isnull(exl.balanceamt,'0') as balanceamt,exl.FinYearFK from ft_excessdet ex,ft_excessledgerdet exl,fm_headermaster h,fm_ledgermaster l,textvaltable t where ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk and t.textcriteria='FEECA' and t.textcode=ex.feecategory and t.textcode=exl.feecategory and ex.app_no='" + appnoNew + "' and exl.feecategory in('" + semyear + "') " + excessType + "";
            }
            else
            {
                selectQuery = "select h.headername,exl.headerfk,l.ledgername,exl.ledgerfk,t.textval,exl.feecategory,isnull(exl.excessamt,'0') as excessamt,isnull(exl.adjamt,'0') as adjamt,isnull(exl.balanceamt,'0') as balanceamt from ft_excessdet ex,ft_excessledgerdet exl,fm_headermaster h,fm_ledgermaster l,textvaltable t where ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk and t.textcriteria='FEECA' and t.textcode=ex.feecategory and t.textcode=exl.feecategory and ex.app_no='" + appnoNew + "' and exl.feecategory in('" + semyear + "') " + excessType + "";
            }

            #endregion

            DataSet ds_stud = new DataSet();
            ds_stud.Clear();
            try
            {
                ds_stud = d2.select_method_wo_parameter(selectQuery, "Text");
            }
            catch { }
            string actualfinyearfk = string.Empty;
            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
            if (ds_stud.Tables.Count > 0 && ds_stud.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_stud.Tables[0].Rows.Count; i++)
                {
                    DataRow dr_Student = tbl_Student.NewRow();

                    string feecode = Convert.ToString(ds_stud.Tables[0].Rows[i]["feecategory"]);
                    string feeVal = Convert.ToString(ds_stud.Tables[0].Rows[i]["textval"]);
                    string ldFK = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]);
                    string ldName = Convert.ToString(ds_stud.Tables[0].Rows[i]["ledgername"]);
                    string hdFK = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]);
                    string hdName = Convert.ToString(ds_stud.Tables[0].Rows[i]["headername"]);
                    if (checkSchoolSetting() == 0)
                    {
                        actualfinyearfk = Convert.ToString(ds_stud.Tables[0].Rows[i]["FinYearFK"]);
                        dr_Student["finyearfk"] = actualfinyearfk;
                    }
                    dr_Student["app_no"] = appnoNew;
                    dr_Student["TextVal"] = feeVal;
                    dr_Student["TextCode"] = feecode;
                    dr_Student["Header_ID"] = hdFK;
                    dr_Student["Header_Name"] = hdName;
                    dr_Student["Fee_Code"] = ldFK;
                    dr_Student["Fee_Type"] = ldName;
                    double excessAmt = 0;
                    double adjAmt = 0;
                    double balAmt = 0;
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["excessamt"]), out excessAmt);
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["adjamt"]), out adjAmt);
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["balanceamt"]), out balAmt);

                    dr_Student["Total"] = excessAmt;
                    dr_Student["PaidAmt"] = adjAmt;
                    dr_Student["BalAmt"] = balAmt;
                    dr_Student["ToBePaid"] = "0";
                    tbl_Student.Rows.Add(dr_Student);
                }
                if (tbl_Student.Rows.Count > 0 && txt_rollno.Text.Trim() != "")
                {
                    grid_Details.DataSource = tbl_Student;
                    grid_Details.DataBind();
                    grid_Details.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    grid_Details.DataSource = null;
                    grid_Details.DataBind();
                    grid_Details.Visible = false;
                    btnSave.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Fees')", true);
                }
            }
            else
            {
                try
                {
                    grid_Details.DataSource = null;
                    grid_Details.DataBind();
                    grid_Details.Visible = false;
                    btnSave.Visible = false;
                }
                catch { }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Fees')", true);
            }
        }
        catch (Exception ex)
        {
            grid_Details.DataSource = null;
            grid_Details.DataBind();
            grid_Details.Visible = false;
            btnSave.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
        }

    }

    protected void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                Fpspread1.SaveChanges();
                string rollno = "";
                string rolladmit = "";
                string degreename1 = "";
                string name1 = "";
                string degreecode1 = "";
                string regno1 = "";
                string smartno = string.Empty;

                string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
                string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
                if (actrow != "-1")
                {
                    rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                    rolladmit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                    degreename1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 7].Text);
                    degreecode1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 7].Tag);
                    name1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 6].Text);
                    regno1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Text);
                    smartno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Text);

                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        //  rollno = rollno;
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        //reg no
                        // rollno = regno1;
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        // rollno = rolladmit;
                    }

                }
                Fpspread1.Sheets[0].ActiveRow = -1;
                Fpspread1.Sheets[0].ActiveColumn = -1;
                Fpspread1.SaveChanges();
                txt_rollno.Text = Convert.ToString(rollno);
                txt_rollno_Changed(sender, e);
                // Session["degreecodenew"] = Convert.ToString(degreecode1);
                popwindow.Visible = false;
            }
        }
        catch (Exception ex) { }
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
        catch (Exception ex) { }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread1.SaveChanges();
            string feecat = string.Empty;
            string ddlstream = Convert.ToString(ddl_strm.SelectedItem.Value);
            string batch = Convert.ToString(ddl_batch1.SelectedItem.Value);
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree2));
            string branch = Convert.ToString(getCblSelectedValue(cbl_branch1));
            string sec = Convert.ToString(getCblSelectedValue(cbl_sec2));
            string selqry = " select r.app_no,r.Roll_No,r.Reg_No,r.roll_admit,r.Stud_Name,a.app_formno,r.batch_year,r.Current_Semester,r.sections,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,smart_serial_no from applyn a,Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and d.Degree_Code in ('" + branch + "') and r.Batch_Year='" + batch + "'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
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

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 80;
                Fpspread1.Sheets[0].Columns[1].Locked = false;

                Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 130;

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

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
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
                Fpspread1.Sheets[0].Columns[7].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].SetColumnMerge(7, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Columns[7].Width = 270;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

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
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    //Smartcard no
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    //if (smartDisp == 0)
                    //    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    //else if (smartDisp == 1)
                    //    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    //else if (smartDisp == 2 || smartDisp == 3)
                    //    Fpspread1.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    //App no
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App No";
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                }

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
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

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                }
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                Fpspread1.SaveChanges();

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
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "ChallanReceipt");
        }
    }

    protected void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        string name = Convert.ToString(ViewState["name"]);
        CallCheckBoxChangedEvent(cbl_sem, cb_sem, txt_sem, name);
    }
    protected void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string name = Convert.ToString(ViewState["name"]);
        CallCheckBoxListChangedEvent(cbl_sem, cb_sem, txt_sem, name);
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
    protected void btn_search_Click(object sender, EventArgs e)
    {
        textRoll();
    }

    protected void grid_Details_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            double partamt;
            if (partamt1 == 0 && partamt2 == 0)
            {
                partamt2 = 1;
                if (txttobepaid.Text.Trim() != "")
                {
                    partamt = Convert.ToDouble(txttobepaid.Text.Trim());
                    partamt1 = partamt;
                }
                else
                {
                    partamt = 0;
                }
            }
            else
            {
                partamt = partamt1;
            }
            CheckBox cbox_selectLedger = (CheckBox)e.Row.Cells[1].FindControl("cb_selectLedger");
            TextBox txtexess = (TextBox)e.Row.Cells[5].FindControl("txt_tot_amt");
            TextBox txtpaid = (TextBox)e.Row.Cells[6].FindControl("txt_paid_amt");
            TextBox txtBal = (TextBox)e.Row.Cells[7].FindControl("txt_bal_amt");
            TextBox txttobe = (TextBox)e.Row.Cells[8].FindControl("txt_tobepaid_amt");
            txtexess.Attributes.Add("readonly", "readonly");
            txtpaid.Attributes.Add("readonly", "readonly");
            txtBal.Attributes.Add("readonly", "readonly");
            double tobePaid = 0;
            double temptobePaid = 0;
            double total = 0;
            double tempTotal = 0;
            double paid = 0;
            double tempPaid = 0;
            double balamt = 0;
            double tempbalamt = 0;
            double.TryParse(Convert.ToString(txt_totamt.Text), out total);
            double.TryParse(Convert.ToString(txtexess.Text), out tempTotal);

            double.TryParse(Convert.ToString(txttobepaid.Text), out tobePaid);
            double.TryParse(Convert.ToString(txttobe.Text), out temptobePaid);

            double.TryParse(Convert.ToString(txt_paidamt.Text), out paid);
            double.TryParse(Convert.ToString(txtpaid.Text), out tempPaid);

            double.TryParse(Convert.ToString(txt_balamt.Text), out balamt);
            double.TryParse(Convert.ToString(txtBal.Text), out tempbalamt);

            if (total != 0)
                txt_totamt.Text = Convert.ToString(total + tempTotal);
            else
                txt_totamt.Text = Convert.ToString(tempTotal);

            if (paid != 0)
                txt_paidamt.Text = Convert.ToString(paid + tempPaid);
            else
                txt_paidamt.Text = Convert.ToString(tempPaid);

            if (balamt != 0)
                txt_balamt.Text = Convert.ToString(balamt + tempbalamt);
            else
                txt_balamt.Text = Convert.ToString(tempbalamt);

            if (partamt1 != 0)
            {
                if (tempbalamt >= partamt)
                {
                    txttobe.Text = Convert.ToString(partamt);
                    txtBal.Text = Convert.ToString(tempTotal - (Convert.ToDouble(txttobe.Text) + tempPaid));
                    partamt = 0;
                    partamt1 = partamt;
                }
                else
                {
                    double tempAllot = partamt - tempbalamt;
                    txttobe.Text = Convert.ToString(tempbalamt);
                    txtBal.Text = "0";
                    partamt = tempAllot;
                    partamt1 = partamt;
                }
                cbox_selectLedger.Checked = true;
            }
        }
    }
    protected void grid_Details_DataBound(object sender, EventArgs e)//added by abarna 19.02.2018
    {
        try
        {
            double paiColor = 0;
            double totColor = 0;
            for (int i = 0; i < grid_Details.Rows.Count; i++)
            {
                TextBox txttotamt = (TextBox)grid_Details.Rows[i].FindControl("txt_tot_amt");
                TextBox txtpaiamt = (TextBox)grid_Details.Rows[i].FindControl("txt_paid_amt");

                if (txttotamt.Text.Trim() != "")
                {
                    totColor = Convert.ToDouble(txttotamt.Text.Trim());
                }
                if (txtpaiamt.Text.Trim() != "")
                {
                    paiColor = Convert.ToDouble(txtpaiamt.Text.Trim());
                }

                Color clr = new Color();
                if (paiColor == totColor)
                {
                    //Full fees paid 
                    clr = Color.FromArgb(144, 238, 144);
                }
                else if (paiColor > 0 && totColor > 0)
                {
                    //If Partial Paid
                    clr = Color.FromArgb(255, 182, 193);
                }
                else
                {
                    clr = Color.White;
                }
                for (int j = 0; j < grid_Details.Columns.Count; j++)
                {
                    grid_Details.Rows[i].Cells[j].BackColor = clr;
                }
            }
        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode1, "ChallanReceipt"); 
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    protected void ddl_strm_OnIndexChange(object sender, EventArgs e)
    {
        binddegree2();
        bindbranch1();
        bindsec2();
    }
    public void bindType()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            }
            ddl_strm.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
            }
            if (ddl_strm.Items.Count > 0)
            {
                if (streamEnabled() == 1)
                    ddl_strm.Enabled = true;
                else
                    ddl_strm.Enabled = false;
            }
            else
                ddl_strm.Enabled = false;
        }
        catch
        { }
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
        catch (Exception ex) { }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";
            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " ";
            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode + ") ";
            if (ddl_strm.Enabled)//if (txt_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
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
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
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
        catch (Exception ex) { }
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
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                //commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') ";
            }
            else
            {
                //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code ";
            }
            if (branch.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
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
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
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
        catch (Exception ex) { }
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
        catch (Exception ex) { }
    }
    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }
    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }
    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }
    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'")), out strValue);
        return strValue;
    }
    public string generateReceiptNo()
    {
        string recno = string.Empty;
        try
        {
            int receno = 0;
            string recacr = string.Empty;
            string recnoprev = string.Empty;

            //string fincyr = d2.GetFunction("select LinkValue from InsSettings where LinkName='Current Financial Year' and college_code=" + ddlcollege.SelectedItem.Value + "");//comment by abarna
            string fincyr = d2.getCurrentFinanceYear(usercode, collegecode);//abarna
            string accountid = "";// d2.GetFunction(" select acct_id from acctinfo where college_code ='" + collegecode1 + "'");
            // lblaccid.Text = accountid;
            //string secondreciptqurey = "SELECT RcptStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)";
            string secondreciptqurey = "SELECT VouchStNo from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
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

                //string acronymquery = d2.GetFunction("SELECT RcptAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)");
                string acronymquery = d2.GetFunction("SELECT VouchAcr from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'");
                recacr = acronymquery;


                //int size = Convert.ToInt32(d2.GetFunction("SELECT  RcptSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + finYearid + " and CollegeCode=" + collegecode1 + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings)"));

                int size = Convert.ToInt32(d2.GetFunction("SELECT  VouchSize from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK=" + fincyr + " and CollegeCode=" + ddlcollege.SelectedItem.Value + " and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'"));

                string recenoString = receno.ToString();

                if (size != recenoString.Length && size > recenoString.Length)
                {
                    while (size != recenoString.Length)
                    {
                        recenoString = "0" + recenoString;
                    }
                }
                recno = recacr + recenoString;
                ViewState["receno"] = Convert.ToString(recenoString);
                //lstrcpt.Text = Convert.ToString(receno);
            }

            return recno;
        }
        catch { return recno; }
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

    protected void btnSave_Click(object sender, EventArgs e)
    {
        getSave();
    }
    protected void getSave()
    {
        try
        {
            string amount = string.Empty;
            if (rbl_Refund.Text == "Student")
            {
                bool boolCheck = false;
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                string vochNo = Convert.ToString(txt_rcptno.Text);
                string vochDt = Convert.ToString(txt_date.Text);
                vochDt = vochDt.Split('/')[1] + "/" + vochDt.Split('/')[0] + "/" + vochDt.Split('/')[2];
                string excessType = string.Empty;
                string exType = string.Empty;
                string journalType = string.Empty;
                string exTypeVal = string.Empty;
                string journalTypeVal = string.Empty;
                string rollno = txt_rollno.Text;

                if (rblPaymode.SelectedIndex == 0)
                {
                    excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='0'";
                    exType = "1";
                    journalType = "0";
                }
                else if (rblPaymode.SelectedIndex == 1)
                {
                    excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='1'";
                    exType = "1";
                    journalType = "1";
                }
                else
                {
                    excessType = " and excesstype='2' and isnull(ex_journalentry,'0')=0";
                    exType = "2";
                    journalType = "0";
                }
                #region cheque or DD or Card Details
                string newbankname = string.Empty;
                string newbankcode = string.Empty;
                string ddCheDt = string.Empty;
                ddCheDt = vochDt;
                if (rb_cheque.Checked || rb_dd.Checked)
                {
                    if (ddl_bkname.SelectedItem.Text.ToUpper() == "OTHERS")
                    {
                        newbankname = ddlotherBank.SelectedItem.Text;

                        //aruna 15dec2017
                        #region
                        //newbankcode = subjectcode("BName", newbankname);
                        newbankcode = Convert.ToString(ddlotherBank.SelectedValue);
                        #endregion
                        //txt_other.Text = "";
                    }
                    else
                    {
                        if (ddl_bkname.SelectedIndex != 0)
                        {
                            newbankname = ddl_bkname.SelectedItem.Text;
                            newbankcode = ddl_bkname.SelectedItem.Value;
                        }
                        // txt_other.Text = "";
                    }
                }
                else if (rb_card.Checked)
                {
                    if (ddlCardType.SelectedItem.Text.ToUpper() == "OTHERS")
                    {
                        newbankname = txtCardType.Text.Trim();

                        //aruna 15dec2017
                        #region
                        // newbankcode = subjectcode("CardT", newbankname);
                        newbankcode = Convert.ToString(ddlotherBank.SelectedValue);
                        #endregion
                        txtCardType.Text = "";
                    }
                    else
                    {
                        if (ddlCardType.SelectedIndex != 0)
                        {
                            newbankname = ddlCardType.SelectedItem.Text;
                            newbankcode = ddlCardType.SelectedItem.Value;
                        }
                        txtCardType.Text = "";
                    }
                }
                #endregion
                string PayMode = string.Empty;
                string checkDDno = string.Empty;
                string branch = string.Empty;
                if (rb_cash.Checked)
                {
                    //  mode = "cash";
                    PayMode = "1";
                    //  dtchkdd = "";
                }
                else if (rb_cheque.Checked)
                {
                    //  mode = "cheque";
                    PayMode = "2";
                    checkDDno = txt_chqno.Text.Trim();
                    branch = txt_branch.Text.Trim();
                    ddCheDt = Convert.ToString(txt_date1.Text);
                    ddCheDt = ddCheDt.Split('/')[1] + "/" + ddCheDt.Split('/')[0] + "/" + ddCheDt.Split('/')[2];
                }
                else if (rb_dd.Checked)
                {
                    // mode = "dd";
                    PayMode = "3";
                    checkDDno = txt_ddno.Text.Trim();
                    branch = txt_branch.Text.Trim();
                    ddCheDt = Convert.ToString(txt_date1.Text);
                    ddCheDt = ddCheDt.Split('/')[1] + "/" + ddCheDt.Split('/')[0] + "/" + ddCheDt.Split('/')[2];
                }
                else if (rb_card.Checked)
                {
                    //mode = "card";
                    PayMode = "6";
                    checkDDno = txtLast4No.Text.Trim();
                    branch = newbankname.Trim();
                }
                else if (rb_NEFT.Checked)
                {
                    PayMode = "7";
                    checkDDno = txt_NEFTNO.Text.Trim();
                }
                Dictionary<string, string> getSem = getFeeWise();
                string exDetFk = string.Empty;
                bool boolFirst = false;
                string appNo = string.Empty;
                //string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                foreach (GridViewRow row in grid_Details.Rows)
                {
                    CheckBox cbSel = (CheckBox)row.FindControl("cb_selectLedger");
                    if (cbSel.Checked)
                    {
                        Label lblappNo = (Label)row.FindControl("lblappNo");
                        appNo = Convert.ToString(lblappNo.Text);
                        Label lblhdName = (Label)row.FindControl("lbl_hdrName");
                        Label lblhdFK = (Label)row.FindControl("lbl_hdrid");
                        Label lblldName = (Label)row.FindControl("lbl_feetype");
                        Label lblldFK = (Label)row.FindControl("lbl_feecode");
                        Label lblfeecode = (Label)row.FindControl("lbl_textCode");
                        Label lblfeetxt = (Label)row.FindControl("lbl_textval");
                        TextBox txtExcess = (TextBox)row.FindControl("txt_tot_amt");
                        TextBox txtadjamt = (TextBox)row.FindControl("txt_paid_amt");
                        TextBox txtbalamt = (TextBox)row.FindControl("txt_bal_amt");
                        TextBox txttobepaid = (TextBox)row.FindControl("txt_tobepaid_amt");
                        Label finyearactual = (Label)row.FindControl("lbl_finyear");
                        double excessAmt = 0;
                        double paidAmt = 0;
                        double balAmt = 0;
                        double tobePaid = 0;
                        double.TryParse(Convert.ToString(txtExcess.Text), out excessAmt);
                        double.TryParse(Convert.ToString(txtadjamt.Text), out paidAmt);
                        double.TryParse(Convert.ToString(txtbalamt.Text), out balAmt);
                        double.TryParse(Convert.ToString(txttobepaid.Text), out tobePaid);
                        if (tobePaid != 0)
                        {
                            if (getSem.ContainsKey(lblfeecode.Text))
                            {
                                string amt = Convert.ToString(getSem[lblfeecode.Text]);
                                if (checkSchoolSetting() == 0)
                                {
                                    string updQ = "if exists(select * from ft_excessdet where app_no='" + lblappNo.Text + "' and finyearfk='" + finYearid + "' and actualfinyearfk='" + finyearactual.Text + "' and feecategory='" + lblfeecode.Text + "'" + excessType + ")update ft_excessdet set adjamt=isnull(adjamt,'0')+'" + amt + "',balanceamt=isnull(balanceamt,'0')-'" + amt + "' where app_no='" + lblappNo.Text + "' and finyearfk='" + finYearid + "' and actualfinyearfk='" + finyearactual.Text + "' and  feecategory='" + lblfeecode.Text + "' " + excessType + " else insert into ft_excessdet (excesstransdate,transtime,dailytranscode,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,finyearfk,feecategory,ex_journalentry,actualfinyearfk) values('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + vochNo + "','" + lblappNo.Text + "','" + exType + "','','" + amt + "','0','" + amt + "','" + finYearid + "','" + lblfeecode.Text + "','" + journalType + "','" + finyearactual.Text + "')";
                                    int upd = d2.update_method_wo_parameter(updQ, "Text");
                                    exDetFk = d2.GetFunction("select excessdetpk from ft_excessdet where app_no='" + lblappNo.Text + "' and finyearfk='" + finYearid + "' and actualfinyearfk='" + finyearactual.Text + "'and feecategory='" + lblfeecode.Text + "'" + excessType + " ");
                                    getSem.Remove(lblfeecode.Text);
                                }
                                else
                                {

                                    string updQ = "if exists(select * from ft_excessdet where app_no='" + lblappNo.Text + "' and feecategory='" + lblfeecode.Text + "'" + excessType + ")update ft_excessdet set adjamt=isnull(adjamt,'0')+'" + amt + "',balanceamt=isnull(balanceamt,'0')-'" + amt + "' where app_no='" + lblappNo.Text + "' and feecategory='" + lblfeecode.Text + "' " + excessType + " else insert into ft_excessdet (excesstransdate,transtime,dailytranscode,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,finyearfk,feecategory,ex_journalentry) values('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + vochNo + "','" + lblappNo.Text + "','" + exType + "','','" + amt + "','0','" + amt + "','" + finYearid + "','" + lblfeecode.Text + "','" + journalType + "')";
                                    int upd = d2.update_method_wo_parameter(updQ, "Text");
                                    exDetFk = d2.GetFunction("select excessdetpk from ft_excessdet where app_no='" + lblappNo.Text + "' and feecategory='" + lblfeecode.Text + "'" + excessType + " ");
                                    getSem.Remove(lblfeecode.Text);
                                }
                            }
                            if (exDetFk != "0")
                            {
                                if (checkSchoolSetting() == 0)
                                {
                                    string insQ = " if exists(select * from ft_excessledgerdet where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and finyearfk='" + finyearactual.Text + "')update ft_excessledgerdet set adjamt=isnull(adjamt,'0')+'" + tobePaid + "',balanceamt=isnull(balanceamt,'0')-'" + tobePaid + "' where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and finyearfk='" + finyearactual.Text + "' and finyearfk='" + finyearactual.Text + "' else insert into ft_excessledgerdet (headerfk,ledgerfk,excessamt,adjamt,balanceamt,excessdetfk,feecategory,finyearfk) values('" + lblhdFK.Text + "','" + lblldFK.Text + "','" + excessAmt + "','" + tobePaid + "','" + balAmt + "','" + exDetFk + "','" + lblfeecode.Text + "','" + finyearactual.Text + "')";
                                    int upds = d2.update_method_wo_parameter(insQ, "Text");
                                    boolCheck = true;
                                }
                                else
                                {
                                    string insQ = " if exists(select * from ft_excessledgerdet where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "')update ft_excessledgerdet set adjamt=isnull(adjamt,'0')+'" + tobePaid + "',balanceamt=isnull(balanceamt,'0')-'" + tobePaid + "' where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' else insert into ft_excessledgerdet (headerfk,ledgerfk,excessamt,adjamt,balanceamt,excessdetfk,feecategory,finyearfk) values('" + lblhdFK.Text + "','" + lblldFK.Text + "','" + excessAmt + "','" + tobePaid + "','" + balAmt + "','" + exDetFk + "','" + lblfeecode.Text + "','" + finYearid + "')";
                                    int upds = d2.update_method_wo_parameter(insQ, "Text");
                                    boolCheck = true;
                                }

                                string actualFinYearFk = string.Empty;
                                if (rblPaymode.SelectedIndex == 2)
                                {
                                    if (checkSchoolSetting() == 0)
                                    {

                                        actualFinYearFk = d2.GetFunction("select finyearfk from ft_feeallot where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isrefund='1' and finyearfk='" + finyearactual.Text + "'");
                                    }
                                    else
                                    {
                                        actualFinYearFk = d2.GetFunction("select finyearfk from ft_feeallot where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isrefund='1'");
                                    }
                                }
                                string INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,credit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,DDNo,DDDate,DDBankCode,DDBankBranch,isdeposited,entryusercode,Transtype,narration,deposite_bankfk,receipttype,ActualFinYearFK) values('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + vochNo + "','1','" + lblldFK.Text + "','" + lblhdFK.Text + "','" + lblfeecode.Text + "','" + tobePaid + "','" + finYearid + "','" + lblappNo.Text + "','0','1','" + PayMode + "','" + checkDDno + "','" + vochDt + "','" + newbankcode + "','" + branch + "','1','" + usercode + "','1','','" + newbankcode + "','6','" + actualFinYearFk + "')";
                                int updss = d2.update_method_wo_parameter(INSdaily, "Text");
                                if (updss > 0 && (PayMode == "2" || PayMode == "3"))
                                {
                                    string insqry = "if exists ( select * from FT_FinBankTransaction where DailyTransID ='" + vochNo + "' and FinYearFK ='" + finYearid + "' and PayMode in('" + PayMode + "') and EntryUserCode='" + usercode + "') update FT_FinBankTransaction set TransDate='" + vochDt + "',TransTime='" + DateTime.Now.ToShortTimeString() + "',IsDeposited='1',IsCleared='1',IsBounced='0' where DailyTransID ='" + vochNo + "' and PayMode in('" + PayMode + "') and FinYearFK ='" + finYearid + "' and EntryUserCode='" + usercode + "' else insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK,EntryUserCode) values ('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + newbankcode + "','" + PayMode + "','" + vochNo + "','1','1','1','0','" + tobePaid + "','" + finYearid + "','" + usercode + "')";
                                    int bankupd = d2.update_method_wo_parameter(insqry, "Text");
                                }
                                if (checkSchoolSetting() == 0)
                                {
                                    if (rblPaymode.SelectedIndex == 2)
                                    {
                                        string updAllot = " if exists(select * from ft_feeallot where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isnull(paidamount,'0')<>'0' and finyearfk='" + actualFinYearFk + "') update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + tobePaid + "',balamount=isnull(balamount,'0')+'" + tobePaid + "' where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isnull(paidamount,'0')<>'0' and finyearfk='" + actualFinYearFk + "'";
                                        int updsss = d2.update_method_wo_parameter(updAllot, "Text");
                                    }
                                }
                                else
                                {
                                    if (rblPaymode.SelectedIndex == 2)
                                    {
                                        string updAllot = " if exists(select * from ft_feeallot where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isnull(paidamount,'0')<>'0') update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + tobePaid + "',balamount=isnull(balamount,'0')+'" + tobePaid + "' where app_no='" + lblappNo.Text + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + lblfeecode.Text + "' and isnull(paidamount,'0')<>'0'";
                                        int updsss = d2.update_method_wo_parameter(updAllot, "Text");
                                    }
                                }
                            }
                            amount = Convert.ToString(tobePaid);
                        }
                    }
                }
                if (boolCheck)
                {
                    getVoucherUpdate(collegecode, finYearid);
                    transferReceipt("Voucher", appNo, collegecode, vochDt, vochNo);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    clear();

                    //==========================Added by Saranya on 10/04/2018=============================//
                    int savevalue = 1;
                    string entrycode = Session["Entry_Code"].ToString();

                    string formname = "VoucherRefund";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");
                    IPHostEntry host;
                    string localip = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());
                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localip = ip.ToString();
                        }
                    }
                    string details = "RollNo - " + rollno + ": CollegeCode - " + collegecode + ": VocherNo -" + vochNo + ": Date - " + toa + ": Amount -" + amount + " ";
                    string ctsname = "";
                    if (savevalue == 1)
                    {
                        ctsname = "Student Voucher Refund";
                    }
                    string hostName = Dns.GetHostName();
                    d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);

                    //============================================================================//
                }
            }
            if (rbl_Refund.Text == "Staff")
            {
                bool boolCheck = false;
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                string vochNo = Convert.ToString(txt_rcptno.Text);
                string vochDt = Convert.ToString(txt_date.Text);
                vochDt = vochDt.Split('/')[1] + "/" + vochDt.Split('/')[0] + "/" + vochDt.Split('/')[2];
                string excessType = string.Empty;
                string exType = string.Empty;
                string journalType = string.Empty;
                string exTypeVal = string.Empty;
                string journalTypeVal = string.Empty;
                string rollno = txt_staffid.Text;

                if (rblPaymode.SelectedIndex == 0)
                {
                    excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='0'";
                    exType = "1";
                    journalType = "0";
                }
                else if (rblPaymode.SelectedIndex == 1)
                {
                    excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='1'";
                    exType = "1";
                    journalType = "1";
                }
                else
                {
                    excessType = " and excesstype='2' and isnull(ex_journalentry,'0')=0";
                    exType = "2";
                    journalType = "0";
                }
                #region cheque or DD or Card Details
                string newbankname = string.Empty;
                string newbankcode = string.Empty;
                string ddCheDt = string.Empty;
                ddCheDt = vochDt;
                if (rb_cheque.Checked || rb_dd.Checked)
                {
                    if (ddl_bkname.SelectedItem.Text.ToUpper() == "OTHERS")
                    {
                        newbankname = ddlotherBank.SelectedItem.Text;

                        //aruna 15dec2017
                        #region
                        //newbankcode = subjectcode("BName", newbankname);
                        newbankcode = Convert.ToString(ddlotherBank.SelectedValue);
                        #endregion
                        //txt_other.Text = "";
                    }
                    else
                    {
                        if (ddl_bkname.SelectedIndex != 0)
                        {
                            newbankname = ddl_bkname.SelectedItem.Text;
                            newbankcode = ddl_bkname.SelectedItem.Value;
                        }
                        // txt_other.Text = "";
                    }
                }
                else if (rb_card.Checked)
                {
                    if (ddlCardType.SelectedItem.Text.ToUpper() == "OTHERS")
                    {
                        newbankname = txtCardType.Text.Trim();

                        //aruna 15dec2017
                        #region
                        // newbankcode = subjectcode("CardT", newbankname);
                        newbankcode = Convert.ToString(ddlotherBank.SelectedValue);
                        #endregion
                        txtCardType.Text = "";
                    }
                    else
                    {
                        if (ddlCardType.SelectedIndex != 0)
                        {
                            newbankname = ddlCardType.SelectedItem.Text;
                            newbankcode = ddlCardType.SelectedItem.Value;
                        }
                        txtCardType.Text = "";
                    }
                }
                #endregion
                string PayMode = string.Empty;
                string checkDDno = string.Empty;
                string branch = string.Empty;
                if (rb_cash.Checked)
                {
                    //  mode = "cash";
                    PayMode = "1";
                    //  dtchkdd = "";
                }
                else if (rb_cheque.Checked)
                {

                    //  mode = "cheque";
                    PayMode = "2";
                    checkDDno = txt_chqno.Text.Trim();
                    branch = txt_branch.Text.Trim();
                    ddCheDt = Convert.ToString(txt_date1.Text);
                    ddCheDt = ddCheDt.Split('/')[1] + "/" + ddCheDt.Split('/')[0] + "/" + ddCheDt.Split('/')[2];
                }
                else if (rb_dd.Checked)
                {
                    // mode = "dd";
                    PayMode = "3";
                    checkDDno = txt_ddno.Text.Trim();
                    branch = txt_branch.Text.Trim();
                    ddCheDt = Convert.ToString(txt_date1.Text);
                    ddCheDt = ddCheDt.Split('/')[1] + "/" + ddCheDt.Split('/')[0] + "/" + ddCheDt.Split('/')[2];
                }
                else if (rb_card.Checked)
                {
                    //mode = "card";
                    PayMode = "6";
                    checkDDno = txtLast4No.Text.Trim();
                    branch = newbankname.Trim();
                }
                else if (rb_NEFT.Checked)//abarna
                {
                    //mode = "card";
                    PayMode = "7";
                    checkDDno = txt_NEFTNO.Text.Trim();
                    //branch = newbankname.Trim();
                }
                Dictionary<string, string> getSem = getFeeWise();
                string exDetFk = string.Empty;
                bool boolFirst = false;
                string appNo = string.Empty;
                //string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
                foreach (GridViewRow row in grid_Details.Rows)
                {
                    CheckBox cbSel = (CheckBox)row.FindControl("cb_selectLedger");
                    if (cbSel.Checked)
                    {
                        string Staffid = txt_staffid.Text.Trim();

                        appNo = d2.GetFunction("select appl_id from staff_appl_master Sa,staffmaster Sm where sm.appl_no=sa.appl_no and staff_code='" + Staffid + "' and sa.college_code='" + ddlcollege.SelectedValue + "'");
                        //Label lblappNo = (Label)row.FindControl("lblappNo");
                        //appNo = Convert.ToString(lblappNo.Text);
                        Label lblhdName = (Label)row.FindControl("lbl_hdrName");
                        Label lblhdFK = (Label)row.FindControl("lbl_hdrid");
                        Label lblldName = (Label)row.FindControl("lbl_feetype");
                        Label lblldFK = (Label)row.FindControl("lbl_feecode");
                        //Label lblfeecode = (Label)row.FindControl("lbl_textCode");
                        //Label lblfeetxt = (Label)row.FindControl("lbl_textval");
                        TextBox txtExcess = (TextBox)row.FindControl("txt_tot_amt");
                        TextBox txtadjamt = (TextBox)row.FindControl("txt_paid_amt");
                        TextBox txtbalamt = (TextBox)row.FindControl("txt_bal_amt");
                        TextBox txttobepaid = (TextBox)row.FindControl("txt_tobepaid_amt");
                        double excessAmt = 0;
                        double paidAmt = 0;
                        double balAmt = 0;
                        double tobePaid = 0;
                        double.TryParse(Convert.ToString(txtExcess.Text), out excessAmt);
                        double.TryParse(Convert.ToString(txtadjamt.Text), out paidAmt);
                        double.TryParse(Convert.ToString(txtbalamt.Text), out balAmt);
                        double.TryParse(Convert.ToString(txttobepaid.Text), out tobePaid);
                        string feecategory = string.Empty;
                        if (tobePaid != 0)
                        {
                            //if (getSem.ContainsKey(lblfeecode.Text))
                            //{
                            //string amt = Convert.ToString(getSem[lblfeecode.Text]);
                            string updQ = "if exists(select * from ft_excessdet where app_no='" + appNo + "' and feecategory='" + feecategory + "'" + excessType + ")update ft_excessdet set adjamt=isnull(adjamt,'0')+'" + tobePaid + "',balanceamt=isnull(balanceamt,'0')-'" + tobePaid + "' where app_no='" + appNo + "' and feecategory='" + feecategory + "' " + excessType + " else insert into ft_excessdet (excesstransdate,transtime,dailytranscode,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,finyearfk,feecategory,ex_journalentry) values('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + vochNo + "','" + appNo + "','" + exType + "','','" + tobePaid + "','0','" + tobePaid + "','" + finYearid + "','" + feecategory + "','" + journalType + "')";
                            int upd = d2.update_method_wo_parameter(updQ, "Text");
                            exDetFk = d2.GetFunction("select excessdetpk from ft_excessdet where app_no='" + appNo + "' and feecategory='" + feecategory + "'" + excessType + " ");
                            // getSem.Remove(lblfeecode.Text);
                            //}
                            if (exDetFk != "0")
                            {
                                string insQ = " if exists(select * from ft_excessledgerdet where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + feecategory + "')update ft_excessledgerdet set adjamt=isnull(adjamt,'0')+'" + tobePaid + "',balanceamt=isnull(balanceamt,'0')-'" + tobePaid + "' where excessdetfk='" + exDetFk + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + feecategory + "' else insert into ft_excessledgerdet (headerfk,ledgerfk,excessamt,adjamt,balanceamt,excessdetfk,feecategory,finyearfk) values('" + lblhdFK.Text + "','" + lblldFK.Text + "','" + excessAmt + "','" + tobePaid + "','" + balAmt + "','" + exDetFk + "','" + feecategory + "','" + finYearid + "')";
                                int upds = d2.update_method_wo_parameter(insQ, "Text");
                                boolCheck = true;
                                string actualFinYearFk = finYearid;
                                string INSdaily = "insert into FT_FinDailyTransaction (TransDate,TransTime,TransCode,MemType,LedgerFK,HeaderFK,FeeCategory,credit,FinYearFK,App_No,IsCanceled,IsCollected,paymode,DDNo,DDDate,DDBankCode,DDBankBranch,isdeposited,entryusercode,Transtype,narration,deposite_bankfk,receipttype,ActualFinYearFK) values('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + vochNo + "','2','" + lblldFK.Text + "','" + lblhdFK.Text + "','" + feecategory + "','" + tobePaid + "','" + finYearid + "','" + appNo + "','0','1','" + PayMode + "','" + checkDDno + "','" + vochDt + "','" + newbankcode + "','" + branch + "','1','" + usercode + "','1','','" + newbankcode + "','6','" + actualFinYearFk + "')";
                                int updss = d2.update_method_wo_parameter(INSdaily, "Text");
                                if (updss > 0 && (PayMode == "2" || PayMode == "3"))
                                {
                                    string insqry = "if exists ( select * from FT_FinBankTransaction where DailyTransID ='" + vochNo + "' and FinYearFK ='" + finYearid + "' and PayMode in('" + PayMode + "') and EntryUserCode='" + usercode + "') update FT_FinBankTransaction set TransDate='" + vochDt + "',TransTime='" + DateTime.Now.ToShortTimeString() + "',IsDeposited='1',IsCleared='1',IsBounced='0' where DailyTransID ='" + vochNo + "' and PayMode in('" + PayMode + "') and FinYearFK ='" + finYearid + "' and EntryUserCode='" + usercode + "' else insert into FT_FinBankTransaction (TransDate,TransTime,BankFK,PayMode,DailyTransID,IsDeposited,IsCleared,IsBounced,Credit,Debit,FinYearFK,EntryUserCode) values ('" + vochDt + "','" + DateTime.Now.ToShortTimeString() + "','" + newbankcode + "','" + PayMode + "','" + vochNo + "','1','1','1','0','" + tobePaid + "','" + finYearid + "','" + usercode + "')";
                                    int bankupd = d2.update_method_wo_parameter(insqry, "Text");
                                }
                                if (rblPaymode.SelectedIndex == 2)
                                {
                                    string updAllot = " if exists(select * from ft_feeallot where app_no='" + appNo + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + feecategory + "' and isnull(paidamount,'0')<>'0') update ft_feeallot set paidamount=isnull(paidamount,'0')-'" + tobePaid + "',balamount=isnull(balamount,'0')+'" + tobePaid + "' where app_no='" + appNo + "' and headerfk='" + lblhdFK.Text + "' and ledgerfk='" + lblldFK.Text + "' and feecategory='" + feecategory + "' and isnull(paidamount,'0')<>'0'";
                                    int updsss = d2.update_method_wo_parameter(updAllot, "Text");
                                }
                            }
                            amount = Convert.ToString(tobePaid);
                        }
                    }
                }
                if (boolCheck)
                {
                    getVoucherUpdate(collegecode, finYearid);
                    transferReceipt("Voucher", appNo, collegecode, vochDt, vochNo);
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    clear();

                    //==========================Added by Saranya on 10/04/2018=============================//
                    int savevalue = 1;
                    string entrycode = Session["Entry_Code"].ToString();
                    string formname = "VoucherRefund";
                    string toa = DateTime.Now.ToString("MM/dd/yyy hh:mm:ss tt");
                    string doa = DateTime.Now.ToString("MM/dd/yyy");

                    IPHostEntry host;
                    string localip = "";
                    host = Dns.GetHostEntry(Dns.GetHostName());

                    foreach (IPAddress ip in host.AddressList)
                    {
                        if (ip.AddressFamily.ToString() == "InterNetwork")
                        {
                            localip = ip.ToString();
                        }
                    }
                    string details = "StaffCode - " + rollno + ": CollegeCode - " + collegecode + ": VocherNo -" + vochNo + ": Date - " + toa + ": Amount -" + amount + " ";
                    string ctsname = "";
                    if (savevalue == 1)
                    {
                        ctsname = "Staff Voucher Refund";
                    }
                    string hostName = Dns.GetHostName();
                    d2.insertEinanceUserActionLog(entrycode, formname, 1, toa, doa, details, ctsname, localip);
                    //============================================================================//
                }
            }
        }
        catch { }
    }
    protected void getVoucherUpdate(string collegecode, string finYearid)
    {
        try
        {
            string uprec = "update FM_FinCodeSettings set VouchStNo=" + ViewState["receno"] + "+1 where IsHeader=0 and FinYearFK='" + finYearid + "' and collegecode ='" + collegecode + "' and FromDate = (select MAX(FromDate) from FM_FinCodeSettings where IsHeader=0 and FinYearFK='" + finYearid + "' and collegecode ='" + collegecode + "' and isnull(onlineRec,'')<>'1') and isnull(onlineRec,'')<>'1'";
            int uprecno = d2.update_method_wo_parameter(uprec, "Text");
            txt_rcptno.Text = generateReceiptNo();
        }
        catch { }
    }

    protected Dictionary<string, string> getFeeWise()
    {
        Dictionary<string, string> getFee = new Dictionary<string, string>();
        try
        {
            ArrayList arSem = new ArrayList();
            foreach (GridViewRow row in grid_Details.Rows)
            {
                CheckBox cbSel = (CheckBox)row.FindControl("cb_selectLedger");
                if (cbSel.Checked)
                {
                    Label lblappNo = (Label)row.FindControl("lblappNo");
                    Label lblhdName = (Label)row.FindControl("lbl_hdrName");
                    Label lblhdFK = (Label)row.FindControl("lbl_hdrid");
                    Label lblldName = (Label)row.FindControl("lbl_feetype");
                    Label lblldFK = (Label)row.FindControl("lbl_feecode");
                    Label lblfeecode = (Label)row.FindControl("lbl_textCode");
                    Label lblfeetxt = (Label)row.FindControl("lbl_textval");
                    TextBox txtExcess = (TextBox)row.FindControl("txt_tot_amt");
                    TextBox txtadjamt = (TextBox)row.FindControl("txt_paid_amt");
                    TextBox txtbalamt = (TextBox)row.FindControl("txt_bal_amt");
                    TextBox txttobepaid = (TextBox)row.FindControl("txt_tobepaid_amt");
                    double excessAmt = 0;
                    double paidAmt = 0;
                    double balAmt = 0;
                    double tobePaid = 0;
                    double.TryParse(Convert.ToString(txtExcess.Text), out excessAmt);
                    double.TryParse(Convert.ToString(txtadjamt.Text), out paidAmt);
                    double.TryParse(Convert.ToString(txtbalamt.Text), out balAmt);
                    double.TryParse(Convert.ToString(txttobepaid.Text), out tobePaid);
                    if (tobePaid != 0)
                    {
                        if (!getFee.ContainsKey(lblfeecode.Text))
                            getFee.Add(lblfeecode.Text, Convert.ToString(tobePaid));
                        else
                        {
                            double amount = 0;
                            double.TryParse(Convert.ToString(getFee[lblfeecode.Text]), out amount);
                            amount += tobePaid;
                            getFee.Remove(lblfeecode.Text);
                            getFee.Add(lblfeecode.Text, Convert.ToString(amount));
                        }
                    }
                }
            }
        }
        catch { }
        return getFee;
    }

    public void transferReceipt(string dupReceipt, string AppNo, string collegecode1, string recptDt, string recptNo)
    {
        //PAVAI College and School
        // FpSpread1.SaveChanges();
        try
        {
            #region Voucherprint for student
            if (rbl_Refund.Text == "Student")
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
                {
                    string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                    //  finYearid = Convert.ToString(ddlfinyear.SelectedItem.Value);
                    byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                    byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                    //Document Settings

                    bool createPDFOK = false;
                    Div3.InnerHtml = "";
                    StringBuilder sbHtml = new StringBuilder();
                    string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                    int heightvar = 0;
                    sbHtml.Clear();
                    int officeCopyHeight = 0;
                    StringBuilder sbHtmlCopy = new StringBuilder();
                    string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                    if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                    {
                        string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch  from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        {
                            string rollno = string.Empty;
                            string studname = string.Empty;
                            string receiptno = string.Empty;
                            string name = string.Empty;
                            string batch_year = string.Empty;

                            string app_formno = string.Empty;
                            string appnoNew = string.Empty;
                            string Regno = string.Empty;
                            string Roll_admit = string.Empty;
                            string section = string.Empty;
                            string currentSem = string.Empty;

                            string batchYrSem = string.Empty;

                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                            //string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                            string mode = string.Empty;
                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                            string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]).Trim();
                            string modePaySng = string.Empty;
                            string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                            string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                            string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                            string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);

                            DataTable uniqueCols = dsDet.Tables[0].DefaultView.ToTable(true, "PayMode");
                            if (uniqueCols.Rows.Count > 0)
                            {
                                for (int a = 0; a < uniqueCols.Rows.Count; a++)
                                {
                                    switch (Convert.ToString(uniqueCols.Rows[a][0]).Trim())
                                    {
                                        case "1":
                                            mode += "Cash,";
                                            break;
                                        case "2":
                                            mode += "Cheque,";
                                            break;
                                        case "3":
                                            mode += "DD,";
                                            break;
                                        case "6":
                                            mode += "Card";
                                            break;
                                        case "7":
                                            mode += "Neft";
                                            break;
                                    }
                                }
                                mode = mode.TrimEnd(',');
                            }
                            else
                            {
                                switch (paymode)
                                {
                                    case "1":
                                        mode = "Cash";
                                        break;
                                    case "2":
                                        mode = "Cheque";
                                        //mode = "Cheque - No:" + ddNo;
                                        modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                        //mode += modePaySng;
                                        break;
                                    case "3":
                                        mode = "DD";
                                        //mode = "DD - No:" + ddNo;
                                        modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                        //mode += modePaySng;
                                        break;
                                    case "4":
                                        mode = "Challan";
                                        break;
                                    case "5":
                                        mode = "Online Payment";
                                        break;
                                    case "6":
                                        mode = "Card";
                                        modePaySng = "\n\nCard : " + ddBanks;
                                        break;
                                    case "7":
                                        mode = "Neft";
                                        modePaySng = "\n\nNeftNo : " + ddnos;
                                        break;
                                    default:
                                        mode = "Others";
                                        break;
                                }
                            }

                            string queryRollApp;


                            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                            {
                                queryRollApp = "select r.Roll_No,a.app_formno,a.app_no, r.Reg_No,r.Stud_Name,r.Roll_admit,r.sections,r.batch_year,r.current_semester  from Registration r,applyn a where r.App_No=a.app_no and r.app_no='" + AppNo + "'";
                            }
                            else
                            {
                                queryRollApp = "select app_formno as Roll_No,app_formno,app_no,app_formno as  Reg_No,Stud_Name,app_formno as Roll_admit,'' sections,batch_year,current_Semester  from applyn where app_no='" + AppNo + "'";
                            }
                            DataSet dsRollApp = new DataSet();
                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                            if (dsRollApp.Tables.Count > 0)
                            {
                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                {
                                    rollno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_No"]);
                                    app_formno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_formno"]);
                                    appnoNew = Convert.ToString(dsRollApp.Tables[0].Rows[0]["app_no"]);
                                    Regno = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Reg_No"]);
                                    Roll_admit = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Roll_admit"]);
                                    studname = Convert.ToString(dsRollApp.Tables[0].Rows[0]["Stud_Name"]);
                                    batch_year = Convert.ToString(dsRollApp.Tables[0].Rows[0]["batch_year"]);
                                    section = Convert.ToString(dsRollApp.Tables[0].Rows[0]["sections"]).ToUpper();
                                    currentSem = Convert.ToString(dsRollApp.Tables[0].Rows[0]["current_Semester"]).ToUpper();
                                }
                                else
                                    appnoNew = AppNo;
                            }
                            else
                                appnoNew = AppNo;
                            name = rollno + "-" + studname;

                            //Print Region
                            #region Print Option For Receipt
                            try
                            {
                                //Fields to print

                                #region Settings Input
                                //Header Div Values
                                byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);

                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                #endregion

                                #region Students Input


                                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
                                {
                                    colquery += " select a.Current_Semester,a.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,a.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,'' Boarding,a.mother,a.parent_name,ISNULL( type,'') as type,'' Sections  from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and a.App_No='" + appnoNew + "' and d.college_code=" + collegecode1 + "";
                                }
                                else
                                {
                                    colquery += " select r.Current_Semester,r.Degree_code,(c.Course_Name +' - '+ dt.Dept_Name) as department, (c.Course_Name +' - '+ dt.dept_acronym) as dept_acronym,r.Batch_Year,(select TextVal  from TextValTable where TextCode = a.seattype) as seattype ,r.Boarding,a.mother,a.parent_name,isnull(r.Sections,'') as Sections from Registration r, applyn a,Degree d,Department dt,Course c where a.app_no=r.App_No and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and r.App_No='" + appnoNew + "' and r.college_code=" + collegecode1 + " ";
                                }


                                string collegename = "";
                                string add1 = "";
                                string add2 = "";
                                string add3 = "";
                                string univ = "";
                                string deg = "";
                                string cursem = "";
                                string batyr = "";
                                string seatty = "";
                                string board = "";
                                string mothe = "";
                                string fathe = "";
                                string stream = "";
                                double deductionamt = 0;
                                string strMem = string.Empty;
                                string TermOrSem = string.Empty;
                                string classdisplay = "Class Name ";
                                string rollDisplay = string.Empty;
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                    }

                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (checkSchoolSetting() == 0)
                                        {
                                            classdisplay = "Class Name ";
                                            TermOrSem = "Term";
                                        }
                                        else
                                        {
                                            classdisplay = "Dept Name ";
                                            TermOrSem = "Semester";
                                        }
                                        //if (degACR == 0)
                                        //{
                                        // deg = Convert.ToString(ds.Tables[1].Rows[0]["department"]);
                                        //}
                                        //else
                                        //{
                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_acronym"]);
                                        //}
                                        cursem = Convert.ToString(ds.Tables[1].Rows[0]["Current_Semester"]);
                                        batyr = Convert.ToString(ds.Tables[1].Rows[0]["Batch_Year"]);
                                        seatty = Convert.ToString(ds.Tables[1].Rows[0]["seattype"]);
                                        board = Convert.ToString(ds.Tables[1].Rows[0]["Boarding"]);
                                        mothe = Convert.ToString(ds.Tables[1].Rows[0]["mother"]);
                                        fathe = Convert.ToString(ds.Tables[1].Rows[0]["parent_name"]);
                                        //sec = " " + Convert.ToString(ds.Tables[1].Rows[0]["Sections"]);
                                        if (checkSchoolSetting() == 0)
                                        {
                                            strMem = "Admission No";
                                        }
                                        else
                                        {
                                            strMem = rbl_rollno.SelectedItem.Text.Trim();
                                            if (Convert.ToInt32(rbl_rollno.SelectedValue) == 0)
                                            {
                                                Roll_admit = rollno;
                                            }
                                            else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 1)
                                            {
                                                Roll_admit = Regno;
                                            }
                                            else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 2)
                                            {
                                                //Roll_admit = Roll_admit;
                                            }
                                            else if (Convert.ToInt32(rbl_rollno.SelectedValue) == 3)
                                            {
                                                Roll_admit = app_formno;
                                            }
                                        }
                                    }
                                }
                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                try
                                {
                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                }
                                catch { }
                                #endregion
                                string degString = string.Empty;
                                //Line3

                                degString = deg;//.Split('-')[0].ToUpper();


                                string[] className = degString.Split('-');
                                if (className.Length > 1)
                                {
                                    degString = className[1];
                                }
                                //string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "'");//commented by saranya on 28/12/2017
                                string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
                                #region Receipt Header

                                //  sbHtml.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                                sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                                sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                                //sbHtmlCopy.Append("<div style=' width:790px; height:#officeCopyHeight#px;'></div>");
                                //sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                                sbHtmlCopy.Append("<div style='height:#officeCopyHeight#px; width:790px;'></div>");
                                if (ColName == 1)
                                {
                                    sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                    sbHtml.Append("<br/>");

                                    sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                    sbHtmlCopy.Append("<br/>");
                                }
                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + Roll_admit + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + studname.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td><td style='width:100px; '>" + TermOrSem + " </td><td style='width:160px; '>: " + currentSem + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                #endregion

                                #region Receipt Body

                                sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                                sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                                string selectQuery = "";

                                int sno = 0;
                                int indx = 0;
                                double totalamt = 0;
                                double balanamt = 0;
                                double curpaid = 0;
                                // double paidamount = 0;


                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();


                                ////New
                                //if (!rb_Journal.Checked)//changed by sudhagar 12.08.2017 for transfer and journal receipt same process
                                //{
                                //  selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3'  group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk and istransfer='1'
                                //}
                                //else
                                //{
                                // selHeadersQ = " select SUM(Credit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='1' and istransfer='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0 ";//,A.Feeallotpk
                                // }

                                selHeadersQ = "      select SUM(Credit) as TakenAmt,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l  where d.HeaderFK =h.HeaderPK     and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='1'  group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0";



                                selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                                //fine amount added by sudhagar 31.01.2017
                                selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'   group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";
                                //New End
                                //if (!rb_Journal.Checked || rb_Journal.Checked)
                                //{
                                //    selHeadersQ += " select SUM(debit) as TakenAmt,SUM(A.FeeAmount) as FeeAmount, isnull(sum(BalAmount),0) as BalAmount,isnull(sum(DeductAmout),0) as DeductAmout,isnull(sum(TotalAmount),0) as TotalAmount,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l,FT_FeeAllot A  where d.HeaderFK =h.HeaderPK  and d.FeeCategory =A.FeeCategory and d.App_No=a.App_No  and d.LedgerFK=a.LedgerFK and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='3' and istransfer='0' group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(debit,'0'))>0 and sum(isnull(credit,'0'))=0";//,A.Feeallotpk
                                //}
                                DataView dv = new DataView();
                                if (selHeadersQ != string.Empty)
                                {
                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            Hashtable htHdrAmt = new Hashtable();
                                            Hashtable htHdrName = new Hashtable();
                                            // Hashtable htfeecat = new Hashtable();
                                            int ledgCnt = 0;
                                            Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                            Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string disphdr = string.Empty;
                                                double allotamt0 = 0;
                                                double deductAmt0 = 0;
                                                double totalAmt0 = 0;
                                                double paidAmt0 = 0;
                                                double balAmt0 = 0;
                                                double creditAmt0 = 0;

                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                //   totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                // deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + appnoNew + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                #region Monthwise
                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                string FeeAllotPk = string.Empty;
                                                //string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                int monWisemon = 0;
                                                int monWiseYea = 0;
                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                if (monWisemon > 0 && monWiseYea > 0)
                                                {
                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                    DataSet dsMonwise = new DataSet();
                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                    {
                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                    }
                                                }
                                                else
                                                {
                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                }
                                                #endregion

                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                sno++;

                                                totalamt += Convert.ToDouble(totalAmt0);
                                                balanamt += Convert.ToDouble(balAmt0);
                                                curpaid += Convert.ToDouble(creditAmt0);

                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                indx++;
                                                createPDFOK = true;
                                                //if (!rb_Journal.Checked || rb_Journal.Checked)
                                                //{
                                                //    if (disphdr != "")
                                                //        disphdr += "-" + "(DR_J)";
                                                //}
                                                //else
                                                //{
                                                if (disphdr != "")
                                                    disphdr += "-" + "(DR_J)";
                                                // }
                                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                //officeCopyHeight -= 20;
                                                ledgCnt++;
                                            }

                                            if (BalanceType == 1)
                                            {
                                                balanamt = retBalance(appnoNew);
                                            }

                                            #region DD Narration
                                            string modeMulti = string.Empty;
                                            bool multiCash = false;
                                            bool multiChk = false;
                                            bool multiDD = false;
                                            bool multiCard = false;
                                            bool multiNeft = false;

                                            DataSet dtMulBnkDetails = new DataSet();
                                            //     dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");
                                            dtMulBnkDetails = d2.select_method_wo_parameter(" select distinct bankname,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(credit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD' when PayMode=7 then 'NEFT' else 'Card' end as Mode,narration from fm_finbankmaster b,FT_FinDailyTransaction f where bankpk=deposite_bankfk and  f.transcode='" + recptNo.Trim() + "' and f.App_No ='" + appnoNew + "' and PayMode in(1,2,3,6,7) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by bankname,DDNo,DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");
                                            if (rb_NEFT.Checked)
                                            {
                                                dtMulBnkDetails = d2.select_method_wo_parameter(" select distinct DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(credit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD' when PayMode=7 then 'NEFT' else 'Card' end as Mode,narration from FT_FinDailyTransaction f where  f.transcode='" + recptNo.Trim() + "' and f.App_No ='" + appnoNew + "' and PayMode in(1,2,3,6,7) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by DDNo,DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");
                                            }

                                            string ddnar = string.Empty;
                                            string remarks = string.Empty;
                                            //double modeht = 40;
                                            if (narration != 0)
                                            {
                                                if (dtMulBnkDetails.Tables.Count > 0)
                                                {
                                                    int sn = 1;
                                                    for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                    {
                                                        string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                                        if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                        {
                                                            multiCash = true;
                                                            continue;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                        {
                                                            multiChk = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                                        {
                                                            multiDD = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][4]).ToUpper() == "NEFT")
                                                        {
                                                            multiNeft = true;
                                                            strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][4]);
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][0];
                                                            continue;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                                        {
                                                            multiCard = true;
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                            sn++;
                                                            continue;
                                                        }

                                                        ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                        sn++;
                                                    }
                                                    //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                    //modeht += 20;

                                                }
                                                remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                                if (remarks.Trim() == "0")
                                                    remarks = string.Empty;
                                                else
                                                {
                                                    remarks = "\n" + remarks;
                                                }
                                                ddnar += remarks;

                                                //if (excessRemaining(appnoNew) > 0)
                                                //    ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(appnoNew);

                                            }

                                            if (multiCash)
                                            {
                                                modeMulti += "Cash,";
                                            }
                                            if (multiChk)
                                            {
                                                modeMulti += "Cheque,";
                                            }
                                            if (multiDD)
                                            {
                                                modeMulti += "DD,";
                                            }
                                            if (multiCard)
                                            {
                                                modeMulti += "Card";
                                            }
                                            if (multiNeft)
                                            {
                                                modeMulti += "Neft";
                                            }
                                            modeMulti = modeMulti.TrimEnd(',');
                                            if (modeMulti != "")
                                            {
                                                mode = modeMulti;
                                            }
                                            //ddnar += remarks;
                                            #endregion

                                            double totalamount = curpaid;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                            sbHtml.Append("</table></div><br>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                            //sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            ////  sbHtml.Append("</table></div><br>");

                                            //sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                            //if (curpaid != 0)
                                            //{
                                            //    if (BalanceType == 1)
                                            //    {
                                            //        balanamt = retBalance(appnoNew);
                                            //    }

                                            //    #region DD Narration
                                            //    modeMulti = string.Empty;
                                            //    multiCash = false;
                                            //    multiChk = false;
                                            //    multiDD = false;
                                            //    multiCard = false;

                                            //    dtMulBnkDetails = new DataSet();
                                            //    dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                            //    ddnar = string.Empty;
                                            //    remarks = string.Empty;
                                            //    //double modeht = 40;
                                            //    if (narration != 0)
                                            //    {
                                            //        if (dtMulBnkDetails.Tables.Count > 0)
                                            //        {
                                            //            int sn = 1;
                                            //            for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                            //            {
                                            //                string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                            //                if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                            //                {
                                            //                    multiCash = true;
                                            //                    continue;
                                            //                }
                                            //                else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                            //                {
                                            //                    multiChk = true;
                                            //                }
                                            //                else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                            //                {
                                            //                    multiDD = true;
                                            //                }
                                            //                else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                            //                {
                                            //                    multiCard = true;
                                            //                    ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                            //                    sn++;
                                            //                    continue;
                                            //                }

                                            //                ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                            //                sn++;
                                            //            }
                                            //            //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                            //            //modeht += 20;

                                            //        }
                                            //        remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + appnoNew + "' and isnull(iscanceled,0)=0");
                                            //        if (remarks.Trim() == "0")
                                            //            remarks = string.Empty;
                                            //        else
                                            //        {
                                            //            remarks = "\n" + remarks;
                                            //        }
                                            //        ddnar += remarks;

                                            //        if (excessRemaining(appnoNew) > 0)
                                            //            ddnar += " Excess Amount Rs. : " + excessRemaining(appnoNew);

                                            //    }

                                            //    if (multiCash)
                                            //    {
                                            //        modeMulti += "Cash,";
                                            //    }
                                            //    if (multiChk)
                                            //    {
                                            //        modeMulti += "Cheque,";
                                            //    }
                                            //    if (multiDD)
                                            //    {
                                            //        modeMulti += "DD,";
                                            //    }
                                            //    if (multiCard)
                                            //    {
                                            //        modeMulti += "Card";
                                            //    }
                                            //    modeMulti = modeMulti.TrimEnd(',');
                                            //    if (modeMulti != "")
                                            //    {
                                            //        mode = modeMulti;
                                            //    }
                                            //    //ddnar += remarks;
                                            //    #endregion


                                            //    totalamount = curpaid;
                                            //    sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Student copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                            //}

                                            //sbHtml.Append("</table></div><br>");

                                            //if (curpaid != 0)
                                            //{
                                            //    sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");
                                            //}


                                            //debit amount


                                            if (ledgCnt == 1)
                                                officeCopyHeight += 290; //270;
                                            else if (ledgCnt == 2)
                                                officeCopyHeight += 260; //240;
                                            else if (ledgCnt == 3)
                                                officeCopyHeight += 230;//210;
                                            else if (ledgCnt == 4)
                                                officeCopyHeight += 200;//180;
                                            else if (ledgCnt >= 5)
                                                officeCopyHeight += 155;// 170;// 150;
                                            // heightvar += officeCopyHeight;
                                            sbHtmlCopy.Append("</table></div><br>");
                                            sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());
                                        }
                                    }
                                }
                                sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
                                #endregion

                                Div3.InnerHtml += sbHtml.ToString();

                            }
                            catch (Exception ex)
                            {
                                d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
                            }
                            finally
                            {
                            }
                            createPDFOK = true;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                        }
                    }
                    //    }
                    //}
                            #endregion
                    #region To print the Receipt
                    if (createPDFOK)
                    {
                        #region New Print
                        //Div3.InnerHtml += sbHtml.ToString();
                        Div3.Visible = true;

                        ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);

                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt Cannot Be Generated')", true);
                    }
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Print Settings')", true);
                }
            }
            #endregion

            #region VoucherPrint for Staff Added by saranya on 06/04/2018

            if (rbl_Refund.Text == "Staff")
            {
                string queryPrint = "select * from FM_RcptChlPrintSettings where collegecode ='" + collegecode1 + "'";
                DataSet dsPri = new DataSet();
                dsPri = d2.select_method_wo_parameter(queryPrint, "Text");
                if (dsPri.Tables.Count > 0 && dsPri.Tables[0].Rows.Count > 0)
                {
                    string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                    //  finYearid = Convert.ToString(ddlfinyear.SelectedItem.Value);
                    byte ColName = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsCollegeName"]);
                    byte studOffiCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["PageType"]);
                    //Document Settings

                    bool createPDFOK = false;
                    Div3.InnerHtml = "";
                    StringBuilder sbHtml = new StringBuilder();
                    string collname = d2.GetFunction("select collname from collinfo where college_code ='" + collegecode1 + "'");
                    int heightvar = 0;
                    sbHtml.Clear();
                    int officeCopyHeight = 0;
                    StringBuilder sbHtmlCopy = new StringBuilder();
                    string confirmChk = d2.GetFunction(" select distinct Transcode from FT_FinDailyTransaction where TransCode='" + recptNo.Trim() + "' and App_No=" + AppNo + " and isnull(IsCanceled,0)=0");
                    if (confirmChk != null && confirmChk != "" && confirmChk != "0")
                    {
                        string chlnDet = "select Convert(varchar(10),TransDate,103) as TransDate, TransTime, TransCode, MemType, App_No, LedgerFK, HeaderFK, FeeCategory,  Debit, PayMode,   TransType, FinYearFK,Receipttype,DDNo,convert(varchar(10),DDDate,103) as DDDate,(select textval from textvaltable where textcode =DDBankCode) as Bank,DDBankBranch  from FT_FinDailyTransaction where TransCode='" + recptNo + "' and App_No ='" + AppNo + "'";
                        DataSet dsDet = d2.select_method_wo_parameter(chlnDet, "Text");
                        if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
                        {
                            string staffCode = string.Empty;
                            string staffName = string.Empty;
                            string receiptno = string.Empty;
                            string deptName = string.Empty;
                            string name = string.Empty;
                            string staff_applid = string.Empty;
                            string rcptTime = Convert.ToString(dsDet.Tables[0].Rows[0]["TransTime"]);
                            //string recptDt = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);

                            string mode = string.Empty;
                            string paymode = Convert.ToString(dsDet.Tables[0].Rows[0]["PayMode"]);
                            string rcptType = Convert.ToString(dsDet.Tables[0].Rows[0]["Receipttype"]);
                            string ddNo = Convert.ToString(dsDet.Tables[0].Rows[0]["ddNo"]).Trim();
                            string modePaySng = string.Empty;
                            string dddates = Convert.ToString(dsDet.Tables[0].Rows[0]["DDDate"]);
                            string ddnos = Convert.ToString(dsDet.Tables[0].Rows[0]["DDNo"]);
                            string ddBanks = Convert.ToString(dsDet.Tables[0].Rows[0]["Bank"]);
                            string ddBrans = Convert.ToString(dsDet.Tables[0].Rows[0]["DDBankBranch"]);

                            DataTable uniqueCols = dsDet.Tables[0].DefaultView.ToTable(true, "PayMode");
                            if (uniqueCols.Rows.Count > 0)
                            {
                                for (int a = 0; a < uniqueCols.Rows.Count; a++)
                                {
                                    switch (Convert.ToString(uniqueCols.Rows[a][0]).Trim())
                                    {
                                        case "1":
                                            mode += "Cash,";
                                            break;
                                        case "2":
                                            mode += "Cheque,";
                                            break;
                                        case "3":
                                            mode += "DD,";
                                            break;
                                        case "6":
                                            mode += "Card";
                                            break;
                                        case "7":
                                            mode += "Neft";
                                            break;
                                    }
                                }
                                mode = mode.TrimEnd(',');
                            }
                            else
                            {
                                switch (paymode)
                                {
                                    case "1":
                                        mode = "Cash";
                                        break;
                                    case "2":
                                        mode = "Cheque";
                                        //mode = "Cheque - No:" + ddNo;
                                        modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                        //mode += modePaySng;
                                        break;
                                    case "3":
                                        mode = "DD";
                                        //mode = "DD - No:" + ddNo;
                                        modePaySng = "\n\nChequeNo : " + ddnos + " Bank : " + ddBanks + "\n\nBranch :" + ddBrans + " Date  : " + dddates;
                                        //mode += modePaySng;
                                        break;
                                    case "4":
                                        mode = "Challan";
                                        break;
                                    case "5":
                                        mode = "Online Payment";
                                        break;
                                    case "6":
                                        mode = "Card";
                                        modePaySng = "\n\nCard : " + ddBanks;
                                        break;
                                    case "7":
                                        mode = "Neft";
                                        modePaySng = "\n\nNEFT NO : " + ddnos;
                                        break;
                                    default:
                                        mode = "Others";
                                        break;
                                }
                            }


                            string queryRollApp = "select staff_name,staff_code,sa.dept_name,sa.college_code,sa.appl_id  from staff_appl_master Sa,staffmaster Sm where sa.appl_no=Sm.appl_no and Sa.college_code='" + collegecode + "' and sa.appl_id='" + AppNo + "'";

                            DataSet dsRollApp = new DataSet();
                            dsRollApp = d2.select_method_wo_parameter(queryRollApp, "Text");
                            if (dsRollApp.Tables.Count > 0)
                            {
                                if (dsRollApp.Tables[0].Rows.Count > 0)
                                {
                                    staffCode = Convert.ToString(dsRollApp.Tables[0].Rows[0]["staff_code"]);
                                    staffName = Convert.ToString(dsRollApp.Tables[0].Rows[0]["staff_name"]);
                                    deptName = Convert.ToString(dsRollApp.Tables[0].Rows[0]["dept_name"]);
                                    staff_applid = Convert.ToString(dsRollApp.Tables[0].Rows[0]["appl_id"]);
                                }
                                else
                                    staff_applid = AppNo;
                            }
                            else
                                staff_applid = AppNo;
                            name = staffCode + "-" + staffName;

                            //Print Region
                            #region Print Option For Receipt
                            try
                            {
                                //Fields to print

                                #region Settings Input
                                //Header Div Values
                                byte narration = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsNarration"]);

                                byte studCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsStudCopy"]);
                                byte officopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsOfficeCopy"]);
                                byte transCopy = Convert.ToByte(dsPri.Tables[0].Rows[0]["IsTransportCopy"]);

                                #endregion

                                #region Students Input


                                string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + collegecode1 + " ";
                                colquery += " select staff_name,staff_code,sa.dept_name,sa.college_code,sa.appl_id  from staff_appl_master Sa,staffmaster Sm where sa.appl_no=Sm.appl_no and Sa.college_code='" + collegecode + "' and sa.appl_id='" + AppNo + "'";


                                string collegename = "";
                                string add1 = "";
                                string add2 = "";
                                string add3 = "";
                                string univ = "";
                                string deg = "";
                                string cursem = "";
                                string batyr = "";
                                string seatty = "";
                                string board = "";
                                string mothe = "";
                                string fathe = "";
                                string stream = "";
                                double deductionamt = 0;
                                string strMem = string.Empty;
                                //string TermOrSem = string.Empty;
                                string classdisplay = "Class Name ";
                                string rollDisplay = string.Empty;
                                ds.Clear();
                                ds = d2.select_method_wo_parameter(colquery, "Text");
                                if (ds.Tables.Count > 0)
                                {
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                                        add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                                        add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                                        add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                                        univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                                    }

                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (checkSchoolSetting() == 0)
                                        {
                                            classdisplay = "Class Name ";
                                            // TermOrSem = "Term";
                                        }
                                        else
                                        {
                                            classdisplay = "Dept Name ";
                                            // TermOrSem = "Semester";
                                        }
                                        deg = Convert.ToString(ds.Tables[1].Rows[0]["dept_name"]);
                                        strMem = "Staff Code";
                                    }
                                }
                                string acaYear = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");
                                try
                                {
                                    acaYear = acaYear.Split(',')[0] + "-" + acaYear.Split(',')[1];
                                }
                                catch { }
                                #endregion
                                string degString = string.Empty;
                                //Line3

                                degString = deg;//.Split('-')[0].ToUpper();


                                string[] className = degString.Split('-');
                                if (className.Length > 1)
                                {
                                    degString = className[1];
                                }
                                //string entryUserCode = d2.GetFunction("select distinct entryusercode from ft_findailytransaction where app_no='" + AppNo + "'");//commented by saranya on 28/12/2017
                                string userName = d2.GetFunction("select Full_Name,User_Id from UserMaster where User_code='" + usercode + "'").Trim();
                                #region Receipt Header

                                //  sbHtml.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");

                                sbHtml.Append("<div style='height: 560px;width:795px; border:0px solid black; margin-left:5px;'><table cellpadding='0' cellspacing='0' style='text-align:center; width: 785px; ' class='classBold10'><tr><td style='padding-left:5px;'>");
                                sbHtml.Append("<div style=' width:790px; height:100px;'></div>");

                                //sbHtmlCopy.Append("<div style=' width:790px; height:#officeCopyHeight#px;'></div>");
                                //sbHtmlCopy.Append("<div style='margin-top:#officeCopyHeight#px; width:790px;'></div>");
                                sbHtmlCopy.Append("<div style='height:#officeCopyHeight#px; width:790px;'></div>");
                                if (ColName == 1)
                                {
                                    sbHtml.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                    sbHtml.Append("<br/>");

                                    sbHtmlCopy.Append("<center> <span style='text-align:right; width:785px;font-size:14px;height:60px;'> " + collname + "</span> </center>");
                                    sbHtmlCopy.Append("<br/>");
                                }
                                sbHtml.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + "  </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + staffCode + " </td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>:" + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + staffName.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                sbHtmlCopy.Append("<table border='1' rules='rows' style='width:785px; border:1px solid black; ' class='classBold10'><tr><td style='text-align:center; font-size:15px;  '  colspan='6'>" + dupReceipt + " </td></tr><tr><td style='width:80px; '>" + strMem + " </td><td style='width:240px; '>: " + staffCode + "</td><td style='width:80px; '>Date </td><td style='width:120px; '>: " + recptDt + "</td><td style='width:100px; '>Journal No </td><td style='width:160px; '>: " + recptNo + "</td></tr><tr><td style='width:80px; '>Name </td><td style='width:240px; ' >: " + staffName.ToUpper() + "</td><td style='width:80px; '>" + classdisplay + "</td><td style='width:120px; ' >: " + degString + "</td></tr><tr><td style='width:80px; '>Collected By </td><td style='width:400px; ' colspan='2' >: " + userName + "</td><td style='width:140px; text-align:right; ' colspan='2'>Mode of Payment </td><td style='width:160px;' >: " + mode + "</td></tr></table>");

                                #endregion

                                #region Receipt Body

                                sbHtml.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                                sbHtmlCopy.Append("<div><table  style='width:785px;  border:1px solid black; ' border='1' rules='rows'  class='classBold10' cellpadding='5'><tr style='height:30px;'><td style='text-align:center; width:40px;font-size:14px;'>Sl.No</td><td style='width:635px;font-size:14px;'>Particulars</td><td style='width:100px;text-align:right;font-size:14px;'>Amount</td><td style='text-align:right;width:10px;'></td></tr>");

                                string selectQuery = "";

                                int sno = 0;
                                int indx = 0;
                                double totalamt = 0;
                                double balanamt = 0;
                                double curpaid = 0;
                                // double paidamount = 0;


                                string selHeadersQ = string.Empty;
                                DataSet dsHeaders = new DataSet();

                                selHeadersQ = "      select SUM(Credit) as TakenAmt,l.LedgerName as DispName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName  from FT_FinDailyTransaction d,FM_HeaderMaster h,FM_LedgerMaster l  where d.HeaderFK =h.HeaderPK     and D.LedgerFK=l.LedgerPK and  transcode='" + recptNo + "' and d.App_No ='" + AppNo + "' and isnull(d.transtype,'0')='1'  group by l.LedgerName,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,H.HeaderName having sum(isnull(Credit,'0'))>0 and sum(isnull(debit,'0'))=0";

                                selHeadersQ += "  select distinct r.college_code,f.feecategory,r.degree_code,isnull(monthcode,'0')as monthcode,MonthYear from Fee_degree_match fd,registration r,FT_FinDailyTransaction f  where fd.college_code=r.college_code and f.app_no=r.app_no and f.feecategory=fd.feecategory and r.degree_code=fd.degree_code and r.college_code='" + collegecode1 + "' and r.App_No ='" + AppNo + "'";

                                selHeadersQ += " select SUM(debit) as TakenAmt,D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk,h.headername  from FT_FinDailyTransaction d,fm_headermaster h  where d.headerfk=h.headerpk and  d.transcode='" + recptNo + "' and d.App_No ='" + AppNo + "'   group by D.LedgerFK,d.HeaderFK,D.FeeCategory,D.DailyTransPk ,h.headername";

                                DataView dv = new DataView();
                                if (selHeadersQ != string.Empty)
                                {
                                    string rcptDatee = recptDt.Split('/')[2] + "-" + recptDt.Split('/')[1] + "-" + recptDt.Split('/')[0];
                                    dsHeaders.Clear();
                                    dsHeaders = d2.select_method_wo_parameter(selHeadersQ, "Text");

                                    if (dsHeaders.Tables.Count > 0)
                                    {
                                        if (dsHeaders.Tables[0].Rows.Count > 0)
                                        {
                                            Hashtable htHdrAmt = new Hashtable();
                                            Hashtable htHdrName = new Hashtable();
                                            // Hashtable htfeecat = new Hashtable();
                                            int ledgCnt = 0;
                                            Dictionary<string, string> htfeecat = new Dictionary<string, string>();
                                            Dictionary<string, double> htfeeAmt = new Dictionary<string, double>();
                                            for (int head = 0; head < dsHeaders.Tables[0].Rows.Count; head++)
                                            {
                                                string disphdr = string.Empty;
                                                double allotamt0 = 0;
                                                double deductAmt0 = 0;
                                                double totalAmt0 = 0;
                                                double paidAmt0 = 0;
                                                double balAmt0 = 0;
                                                double creditAmt0 = 0;

                                                creditAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TakenAmt"]);
                                                //   totalAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["TotalAmount"]);
                                                //balAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["BalAmount"]);

                                                //paidAmt0 = totalAmt0 - balAmt0;
                                                // deductAmt0 = Convert.ToDouble(dsHeaders.Tables[0].Rows[head]["DeductAmout"]);
                                                disphdr = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DispName"]);
                                                string feecatcode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                string feecode = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeCategory"]);
                                                string ledgFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["LedgerFK"]);
                                                string hdrFK = Convert.ToString(dsHeaders.Tables[0].Rows[head]["headerfk"]);

                                                string paidAMtQ = "select isnull(sum(debit),0) from FT_FinDailyTransaction where app_no = '" + staff_applid + "' and ledgerfk='" + ledgFK + "' and headerfk='" + hdrFK + "' and FeeCategory='" + feecatcode + "' and isnull(IsCanceled,0)=0  and    transcode<>'" + recptNo + "' and transdate <='" + rcptDatee + "' and convert(datetime,TransTime) < '" + rcptTime + "'";
                                                paidAmt0 = Convert.ToDouble(d2.GetFunction(paidAMtQ));

                                                #region Monthwise
                                                string DailyTransPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["DailyTransPk"]);
                                                string FeeAllotPk = string.Empty;
                                                //string FeeAllotPk = Convert.ToString(dsHeaders.Tables[0].Rows[head]["FeeAllotPk"]);
                                                int monWisemon = 0;
                                                int monWiseYea = 0;
                                                string monWiseMonQ = "select Monthvalue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                string monWiseYeaQ = "select YearValue from FT_FinDailyTransactionDetailMonthWise where DailyTransFK=" + DailyTransPk + " and isCancel=0";
                                                int.TryParse(d2.GetFunction(monWiseMonQ).Trim(), out monWisemon);
                                                int.TryParse(d2.GetFunction(monWiseYeaQ).Trim(), out monWiseYea);

                                                if (monWisemon > 0 && monWiseYea > 0)
                                                {
                                                    string selMonWiseAmtQ = "select isnull(AllotAmount,0) as AllotAmount,isnull(PaidAMount,0) as PaidAMount,isnull(BalAmount,0) as BalAmount from FT_FeeallotMonthly where FeeAllotPK=" + FeeAllotPk + " and AllotMonth=" + monWisemon + " and AllotYear=" + monWiseYea + "";
                                                    DataSet dsMonwise = new DataSet();
                                                    dsMonwise = d2.select_method_wo_parameter(selMonWiseAmtQ, "Text");
                                                    if (dsMonwise.Tables.Count > 0 && dsMonwise.Tables[0].Rows.Count > 0)
                                                    {
                                                        totalAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["AllotAmount"]);
                                                        paidAmt0 = Convert.ToDouble(dsMonwise.Tables[0].Rows[0]["PaidAmount"]);
                                                        disphdr += "-" + reUse.returnMonthName(monWisemon) + "-" + monWiseYea;
                                                        balAmt0 = totalAmt0 - paidAmt0;
                                                    }
                                                }
                                                else
                                                {
                                                    balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                }
                                                #endregion

                                                //balAmt0 = totalAmt0 - paidAmt0 - creditAmt0;
                                                feecatcode = d2.GetFunction("select textval from TextValTable where TextCode=" + feecatcode + " and college_code=" + collegecode1 + "");
                                                sno++;

                                                totalamt += Convert.ToDouble(totalAmt0);
                                                balanamt += Convert.ToDouble(balAmt0);
                                                curpaid += Convert.ToDouble(creditAmt0);

                                                deductionamt += Convert.ToDouble(deductAmt0);

                                                indx++;
                                                createPDFOK = true;
                                                //if (!rb_Journal.Checked || rb_Journal.Checked)
                                                //{
                                                //    if (disphdr != "")
                                                //        disphdr += "-" + "(DR_J)";
                                                //}
                                                //else
                                                //{
                                                if (disphdr != "")
                                                    disphdr += "-" + "(DR_J)";
                                                // }
                                                sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");

                                                sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'>" + sno + "</td><td style='width:635px;'>" + disphdr + "</td><td style='width:100px;text-align:right;'>" + returnIntegerPart(creditAmt0) + "." + returnDecimalPart(creditAmt0) + "</td><td style='text-align:right;width:10px;'></td></tr>");
                                                //officeCopyHeight -= 20;
                                                ledgCnt++;
                                            }

                                            if (BalanceType == 1)
                                            {
                                                balanamt = retBalance(staff_applid);
                                            }

                                            #region DD Narration
                                            string modeMulti = string.Empty;
                                            bool multiCash = false;
                                            bool multiChk = false;
                                            bool multiDD = false;
                                            bool multiCard = false;
                                            bool multiNeft = false;
                                            DataSet dtMulBnkDetails = new DataSet();
                                            //     dtMulBnkDetails = d2.select_method_wo_parameter("select (select TextVal  from textvaltable where TextCriteria = 'BName' and TextCode=DDBankCode) as Bank,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(debit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  else 'Card' end as Mode,narration  from ft_findailytransaction where app_no=" + appnoNew + " and TransCode='" + recptNo.Trim() + "' and PayMode in(1,2,3,6) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by (DDNo),DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");
                                            dtMulBnkDetails = d2.select_method_wo_parameter(" select distinct bankname,DDNo,DDBankBranch,Convert(varchar(10),DDDate,103) as DDDate,SUM(credit) as Amount,case when PayMode=2 then 'Cheque' when PayMode=1 then 'Cash'  when PayMode=3 then 'DD'  when PayMode=7 then 'NEFT' else 'Card' end as Mode,narration from fm_finbankmaster b,FT_FinDailyTransaction f where bankpk=deposite_bankfk and  f.transcode='" + recptNo.Trim() + "' and f.App_No ='" + staff_applid + "' and PayMode in(1,2,3,6,7) and FinYearFK=" + finYearid + " and isnull(IsCanceled,0)=0 group by bankname,DDNo,DDDate,DDBankCode,DDBankBranch,PayMode,narration", "Text");

                                            string ddnar = string.Empty;
                                            string remarks = string.Empty;
                                            //double modeht = 40;
                                            if (narration != 0)
                                            {
                                                if (dtMulBnkDetails.Tables.Count > 0)
                                                {
                                                    int sn = 1;
                                                    for (int z = 0; z < dtMulBnkDetails.Tables[0].Rows.Count; z++)
                                                    {
                                                        string strMode = Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]);
                                                        if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CASH")
                                                        {
                                                            multiCash = true;
                                                            continue;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CHEQUE")
                                                        {
                                                            multiChk = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                                        {
                                                            multiDD = true;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "DD")
                                                        {
                                                            multiNeft = true;
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1];
                                                            continue;
                                                        }
                                                        else if (Convert.ToString(dtMulBnkDetails.Tables[0].Rows[z][5]).ToUpper() == "CARD")
                                                        {
                                                            multiCard = true;
                                                            ddnar += "\n" + strMode + "  No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + "\nCard :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                            sn++;
                                                            continue;
                                                        }

                                                        ddnar += "\n" + strMode + " No : " + dtMulBnkDetails.Tables[0].Rows[z][1] + " Bank : " + dtMulBnkDetails.Tables[0].Rows[z][0] + "\nBranch :" + dtMulBnkDetails.Tables[0].Rows[z][2] + " Date  : " + dtMulBnkDetails.Tables[0].Rows[z][3] + " Amount : " + dtMulBnkDetails.Tables[0].Rows[z][4] + "/-";
                                                        sn++;
                                                    }
                                                    //modeht = dtMulBnkDetails.Tables[0].Rows.Count * 15;
                                                    //modeht += 20;

                                                }
                                                remarks = d2.GetFunction("select narration from ft_findailytransaction where TransCode='" + recptNo.Trim() + "' and app_no='" + staff_applid + "' and isnull(iscanceled,0)=0");
                                                if (remarks.Trim() == "0")
                                                    remarks = string.Empty;
                                                else
                                                {
                                                    remarks = "\n" + remarks;
                                                }
                                                ddnar += remarks;

                                                if (excessRemaining(staff_applid) > 0)
                                                    ddnar += " Excess/Advance Amount Rs. : " + excessRemaining(staff_applid);

                                            }

                                            if (multiCash)
                                            {
                                                modeMulti += "Cash,";
                                            }
                                            if (multiChk)
                                            {
                                                modeMulti += "Cheque,";
                                            }
                                            if (multiDD)
                                            {
                                                modeMulti += "DD,";
                                            }
                                            if (multiCard)
                                            {
                                                modeMulti += "Card,";
                                            }
                                            if (multiNeft)
                                            {
                                                modeMulti += "Neft";
                                            }
                                            modeMulti = modeMulti.TrimEnd(',');
                                            if (modeMulti != "")
                                            {
                                                mode = modeMulti;
                                            }
                                            //ddnar += remarks;
                                            #endregion

                                            double totalamount = curpaid;
                                            sbHtml.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:55px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;' colspan='3'>Verified by <span style='padding-left:200px;'>Staff copy</span><span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");

                                            sbHtml.Append("</table></div><br>");

                                            sbHtmlCopy.Append("<tr  style='height:30px;'><td style='text-align:center; width:40px;'></td><td style='width:635px;font-size:14px;'>Total</td><td style='width:100px;text-align:right;font-size:12px;'>" + returnIntegerPart(totalamount) + "." + returnDecimalPart(totalamount) + "</td><td style='text-align:right;width:10px;'></td></tr><tr><td colspan='3'>" + ddnar + "</td></tr><tr><td colspan='3' style='text-align:left; width:785px;font-size:14px;height:60px;'><span>Rupees " + DecimalToWords((decimal)totalamount) + " Only.<br/><br/><br/><br/></span></td></tr><tr><td style='text-align:left; width:785px;font-size:14px;height:15px;'  colspan='3'>Verified by  <span style='padding-left:200px;'>Office copy</span> <span style='padding-left:200px;'>Cashier's Sign and Seal</span></td></tr><tr  style='border:1px solid white;'><td>&nbsp;</td></tr>");


                                            if (ledgCnt == 1)
                                                officeCopyHeight += 290; //270;
                                            else if (ledgCnt == 2)
                                                officeCopyHeight += 260; //240;
                                            else if (ledgCnt == 3)
                                                officeCopyHeight += 230;//210;
                                            else if (ledgCnt == 4)
                                                officeCopyHeight += 200;//180;
                                            else if (ledgCnt >= 5)
                                                officeCopyHeight += 155;// 170;// 150;
                                            // heightvar += officeCopyHeight;
                                            sbHtmlCopy.Append("</table></div><br>");
                                            sbHtmlCopy.Replace("#officeCopyHeight#", officeCopyHeight.ToString());
                                        }
                                    }
                                }
                                sbHtml.Append((studOffiCopy == 1 ? sbHtmlCopy.ToString() : string.Empty) + "</td></tr></table></div>");
                                #endregion

                                Div3.InnerHtml += sbHtml.ToString();

                            }
                            catch (Exception ex)
                            {
                                d2.sendErrorMail(ex, collegecode1, "studentpayment");
                            }
                            finally
                            {
                            }
                            createPDFOK = true;
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
                        }
                    }
                    //    }
                    //}
                            #endregion
                    #region To print the Receipt
                    if (createPDFOK)
                    {
                        #region New Print
                        //Div3.InnerHtml += sbHtml.ToString();
                        Div3.Visible = true;

                        ScriptManager.RegisterStartupScript(this, GetType(), "InvokeButton", "PrintDiv();", true);

                        #endregion
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Receipt Cannot Be Generated')", true);
                    }
                    #endregion
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Print Settings')", true);
                }
            }
            #endregion
        }
        catch (Exception ex)
        {
            // d2.sendErrorMail(ex, collegecode1, "CancelReceipDuplicate");
        }

    }


    //Reusable Methods
    private double retBalance(string appNo)
    {
        double ovBalAMt = 0;
        if (BalanceType == 1)
        {
            double.TryParse(d2.GetFunction(" select sum(isnull(totalAmount,0)-isnull(paidAmount,0)) as BalanceAmt from ft_feeallot where app_no =" + appNo + " and IsTransfer='0'"), out ovBalAMt);
        }
        return ovBalAMt;
    }
    private double excessRemaining(string appnoNew)
    {
        string excessamtQ = d2.GetFunction("select sum(isnull(ExcessAmt,0)-isnull(AdjAmt,0)) as BalanceAmt from FT_ExcessDet WHERE  App_No=" + appnoNew + " ");

        double excessamtValue = 0;
        double.TryParse(excessamtQ, out excessamtValue);
        return excessamtValue;
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
    public static string ConvertNumbertoWords(int number)
    {
        if (number == 0)
            return "Zero";
        if (number < 0)
            return "minus " + ConvertNumbertoWords(Math.Abs(number));
        string words = "";
        if ((number / 100000) > 0)
        {
            words += ConvertNumbertoWords(number / 100000) + " Lakhs";
            number %= 100000;
        }
        if ((number / 1000) > 0)
        {
            words += ConvertNumbertoWords(number / 1000) + " Thousand ";
            number %= 1000;
        }
        if ((number / 100) > 0)
        {
            words += ConvertNumbertoWords(number / 100) + "  Hundred ";
            number %= 100;
        }
        if (number > 0)
        {
            if (words != "")
                words += "And ";
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

        words = ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
        if (decPortion > 0)
        {
            words += " and ";
            words += ConvertNumbertoWords(intPortion);//NumberToWords(intPortion)
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

    public string generateBarcode(string barCode)
    {
        string urlImg = Server.MapPath("~/BarCode/" + "barcodeimg" + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".Jpeg");
        System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
        using (Bitmap bitMap = new Bitmap(barCode.Length * 10, 20))
        {
            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M", 16);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + barCode + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                //bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //byte[] byteImage = ms.ToArray();

                //Convert.ToBase64String(byteImage);
                //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);


                bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bitMap.Save(urlImg, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return urlImg;
        }

    }
    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }
    protected void clear()
    {
        txt_rollno.Text = string.Empty;
        txt_name.Text = string.Empty;
        txt_dept.Text = string.Empty;
        txt_SeatType.Text = string.Empty;
        txt_FatherName.Text = string.Empty;
        txt_totamt.Text = string.Empty;
        txt_paidamt.Text = string.Empty;
        txt_balamt.Text = string.Empty;
        txttobepaid.Text = string.Empty;
        grid_Details.Visible = false;
        rblPaymode.SelectedIndex = 0;
        txt_branch.Text = string.Empty;
        txt_ddno.Text = string.Empty;
        txt_ddnar.Text = string.Empty;
        txt_date1.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_chqno.Text = string.Empty;
        txtCardType.Text = string.Empty;
        txt_NEFTNO.Text = string.Empty;
    }


    public void bank()
    {
        try
        {
            ddl_bkname.Items.Clear();
            string query = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster where collegecode='" + Convert.ToString(ddlcollege.SelectedValue) + "'";
            //string queru = "select TextCode,TextVal  from textvaltable where TextCriteria = 'BName' and college_code='" + collegecode + "'";
            DataSet dsBank = d2.select_method_wo_parameter(query, "Text");

            if (dsBank.Tables.Count > 0 && dsBank.Tables[0].Rows.Count > 0)
            {
                ddl_bkname.DataSource = dsBank;
                ddl_bkname.DataTextField = "BankName";
                ddl_bkname.DataValueField = "BankPK";
                ddl_bkname.DataBind();
            }
            ddl_bkname.Items.Insert(0, "Select");
            ddl_bkname.Items.Insert(ddl_bkname.Items.Count, "Others");
        }
        catch (Exception ex) { }
    }
    public void cardType()
    {
        try
        {
            ddlCardType.Items.Clear();
            string queru = "select TextCode,TextVal  from textvaltable where TextCriteria = 'CardT'";
            DataSet dsCard = d2.select_method_wo_parameter(queru, "Text");

            if (dsCard.Tables.Count > 0 && dsCard.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCard;
                ddlCardType.DataTextField = "TextVal";
                ddlCardType.DataValueField = "TextCode";
                ddlCardType.DataBind();
            }
            ddlCardType.Items.Insert(0, "Select");
            ddlCardType.Items.Insert(ddlCardType.Items.Count, "Others");
        }
        catch (Exception ex) { }
    }
    public void bindOtherbankname()
    {
        ddlotherBank.Items.Clear();
        //string selquery = "select distinct (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster where collegecode<>'" + ddlcollege.SelectedItem.Value + "'";
        string selquery = "select (BankName+'-'+AccNo) as BankName,BankPK from FM_FinBankMaster";
        ds.Clear();
        ds = d2.select_method_wo_parameter(selquery, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlotherBank.DataSource = ds;
            ddlotherBank.DataTextField = "BankName";
            ddlotherBank.DataValueField = "BankPK";
            ddlotherBank.DataBind();
            ddlotherBank.Items.Insert(0, "Select");
        }
        else
            ddlotherBank.Items.Insert(0, "Select");
    }
    public string subjectcode(string textcri, string subjename)
    {
        //for new bank
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode + " and TextVal='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + collegecode + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + collegecode + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                    }
                }
            }
        }
        catch (Exception ex) { }
        return subjec_no;
    }
    //public string getCurrentFinanceYear(string userCode, string collegeCode)
    //{
    //    string value = string.Empty;
    //    string ddCollected = "select LinkValue from InsSettings where LinkName='Current Financial Year' and  FinuserCode ='" + userCode + "' and college_code ='" + collegeCode + "'";
    //    value = d2.GetFunction(ddCollected).Trim();
    //    value = value == "0" ? string.Empty : value;
    //    return value;
    //}


    #region Added by saranya for name search option on 27/03/2018

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetName(string prefixText)
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        Hashtable studhash = new Hashtable();

        if (prefixText.Length > 0)
        {
            string[] nameval = prefixText.Split(' ');
            string query = string.Empty;
            string name_VAL = string.Empty;
            for (int i = 0; i < nameval.Length; i++)
            {
                name_VAL += "%" + nameval[i] + "%";
            }

            if (nameval.Length > 0)
            {
                query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No+'-'+r.Reg_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + name_VAL + "'  and r.college_code='" + collegecodestat + "'";
            }
            else
            {
                query = "select  top 100 a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No+'-'+r.Reg_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '%" + prefixText + "%' and r.college_code='" + collegecodestat + "'";
            }
            studhash = ws.GetNameSearch(query);
            if (studhash.Count > 0)
            {
                foreach (DictionaryEntry p in studhash)
                {
                    string studname = Convert.ToString(p.Key);
                    name.Add(studname);
                }
            }
        }
        return name;
    }
    #endregion


    //Staff Division
    protected void txtstaffid_Changed(object sender, EventArgs e)
    {
        string name = string.Empty;
        string degree = string.Empty;
        string college = string.Empty;
        string staffId = Convert.ToString(txt_staffid.Text.Trim());

        if (staffId != "")
        {
            string query = " select appl_id ,h.dept_name,h.dept_code,s.staff_name,s.staff_code,c.collname  from collinfo c,staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code and s.staff_code ='" + staffId + "' and s.college_Code in('" + collegecode + "') ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        name = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                        degree = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);
                        college = Convert.ToString(ds.Tables[0].Rows[i]["collname"]);
                    }
                }
            }
            txt_staffName.Text = name;
            txt_staffDept.Text = degree;
            loadGridStaff();
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();
            query = " select staff_code from staffmaster where resign<>1 and staff_code like '" + prefixText + "%' and college_code='" + collegecodestat + "' order by staff_code asc";
            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        string query = " select top 100 staff_name+'-'+staff_code from staffmaster where resign<>1 and staff_name like '" + prefixText + "%' and college_code='" + collegecodestat + "'  order by staff_name asc";
        DataSet dsN = new DataSet();
        List<string> name = new List<string>();
        try
        {
            name = ws.Getname(query);
        }
        catch { }
        return name;
    }

    protected void rblPaymode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblPaymode.Text == "Refund")
        {
            refundStudOrStaff.Visible = true;

        }
        if (rblPaymode.Text == "Advance" || rblPaymode.Text == "Excess")
        {
            refundStudOrStaff.Visible = false;
        }
    }

    protected void rblRefund_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbl_Refund.Text == "Student")
        {
            rcptsngle.Visible = true;
            StaffRefund.Visible = false;
            rblPaymode.Items[0].Enabled = true;
            rblPaymode.Items[1].Enabled = true;
            grid_Details.Visible = false;
            txt_staffid.Text = "";
            txt_staffName.Text = "";
            txt_staffDept.Text = "";
            txt_balamt.Text = "";
            txt_totamt.Text = "";
            txt_paidamt.Text = "";
            txttobepaid.Text = "";
        }
        if (rbl_Refund.Text == "Staff")
        {
            rcptsngle.Visible = false;
            StaffRefund.Visible = true;
            rbl_rollno.Visible = false;
            rblPaymode.Items[0].Enabled = false;
            rblPaymode.Items[1].Enabled = false;
            grid_Details.Visible = false;
            txt_balamt.Text = "";
            txt_totamt.Text = "";
            txt_paidamt.Text = "";
            txttobepaid.Text = "";
            txt_rollno.Text = "";
            txt_name.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_SeatType.Text = "";
            txt_FatherName.Text = "";
        }
    }

    protected void btn_Staffsearch_Click(object sender, EventArgs e)
    {
        string staffcode = "-1";
        try
        {
            string name = "";
            string dept = "";
            string stType = "";
            string fname = "";

            string Staff_id = Convert.ToString(txt_staffid.Text.Trim());

            if (Staff_id != "")
            {

                string query = " select staff_name,staff_code,sa.dept_name,sa.college_code  from staff_appl_master Sa,staffmaster Sm where sa.appl_no=Sm.appl_no and Sa.college_code='" + collegecode + "' and Sm.staff_code='" + Staff_id + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);
                            dept = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"]);

                            staffcode = Convert.ToString(ds.Tables[0].Rows[i]["staff_code"]);

                        }
                    }
                }

                txt_staffName.Text = name;
                txt_staffDept.Text = dept;
                loadGridStaff();
            }
            else
            {
                txt_totamt.Text = "0.00";
                txt_paidamt.Text = "0.00";
                txt_balamt.Text = "0.00";
                txt_staffName.Text = "";
                txt_staffDept.Text = "";

                grid_Details.DataSource = null;
                grid_Details.DataBind();


            }
        }
        catch (Exception ex) { }
    }

    public void loadGridStaff()
    {
        try
        {
            partamt1 = 0;
            partamt2 = 0;
            btnSave.Visible = false;
            lblStudStatus.Visible = false;
            string ledgerNameScl = string.Empty;
            string finYearFK = string.Empty;
            bool boolSchool = false;
            txt_totamt.Text = "";
            txt_paidamt.Text = "";
            txt_balamt.Text = "";
            txt_rcptno.Text = generateReceiptNo();
            string roll_no = string.Empty;
            string semyear = "";
            string appnoNew = string.Empty;
            string degcode = string.Empty;
            string batchYear = string.Empty;
            string currSem = string.Empty;
            int studemode = 0;
            roll_no = txt_staffid.Text.Trim();
            string excessType = string.Empty;
            string exType = string.Empty;
            string journalType = string.Empty;
            if (rblPaymode.SelectedIndex == 0)
            {
                excessType = " and excesstype='1' and isnull(ex_journalentry,'0')='0'";
                exType = "excesstype = '1'";
                journalType = " ex_journalentry='0'";
            }
            if (rblPaymode.SelectedIndex == 2)
            {
                excessType = " and excesstype='2' and isnull(ex_journalentry,'0')='0'";
                exType = "excesstype = '2'";
                journalType = " ex_journalentry='0'";
            }

            #region Table Structure and Query
            DataTable tbl_Student = new DataTable();
            tbl_Student.Columns.Add("TextVal");
            tbl_Student.Columns.Add("app_no");
            tbl_Student.Columns.Add("TextCode");
            tbl_Student.Columns.Add("Header_ID");
            tbl_Student.Columns.Add("Header_Name");
            tbl_Student.Columns.Add("Fee_Code");
            tbl_Student.Columns.Add("Fee_Type");
            tbl_Student.Columns.Add("Total");
            tbl_Student.Columns.Add("PaidAmt");
            tbl_Student.Columns.Add("BalAmt");
            tbl_Student.Columns.Add("ToBePaid");
            tbl_Student.Columns.Add("finyearfk");
            string selectQuery = "";
            string Staffid = txt_staffid.Text.Trim();

            string app_no = d2.GetFunction("select appl_id from staff_appl_master Sa,staffmaster Sm where sm.appl_no=sa.appl_no and staff_code='" + Staffid + "' and sa.college_code='" + ddlcollege.SelectedValue + "'");

            // appnoNew = "18270";
            selectQuery = "select h.headername,exl.headerfk,l.ledgername,exl.ledgerfk,exl.feecategory,isnull(exl.excessamt,'0') as excessamt,isnull(exl.adjamt,'0') as adjamt,isnull(exl.balanceamt,'0') as balanceamt from ft_excessdet ex,ft_excessledgerdet exl,fm_headermaster h,fm_ledgermaster l where ex.excessdetpk=exl.excessdetfk and ex.feecategory=exl.feecategory and h.headerpk=l.headerfk and exl.headerfk=h.headerpk and exl.headerfk=l.headerfk and exl.ledgerfk=l.ledgerpk and ex.app_no='" + app_no + "' and exl.feecategory in('" + semyear + "') " + excessType + "";

            #endregion

            DataSet ds_stud = new DataSet();
            ds_stud.Clear();
            try
            {
                ds_stud = d2.select_method_wo_parameter(selectQuery, "Text");
            }
            catch { }

            string finYearid = d2.getCurrentFinanceYear(usercode, collegecode);
            if (ds_stud.Tables.Count > 0 && ds_stud.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds_stud.Tables[0].Rows.Count; i++)
                {
                    DataRow dr_Student = tbl_Student.NewRow();

                    string feecode = Convert.ToString(ds_stud.Tables[0].Rows[i]["feecategory"]);
                    //string feeVal = Convert.ToString(ds_stud.Tables[0].Rows[i]["textval"]);
                    string ldFK = Convert.ToString(ds_stud.Tables[0].Rows[i]["LedgerFK"]);
                    string ldName = Convert.ToString(ds_stud.Tables[0].Rows[i]["ledgername"]);
                    string hdFK = Convert.ToString(ds_stud.Tables[0].Rows[i]["HeaderFK"]);
                    string hdName = Convert.ToString(ds_stud.Tables[0].Rows[i]["headername"]);
                    dr_Student["app_no"] = appnoNew;
                    //dr_Student["TextVal"] = feeVal;
                    //dr_Student["TextCode"] = feecode;
                    dr_Student["Header_ID"] = hdFK;
                    dr_Student["Header_Name"] = hdName;
                    dr_Student["Fee_Code"] = ldFK;
                    dr_Student["Fee_Type"] = ldName;
                    double excessAmt = 0;
                    double adjAmt = 0;
                    double balAmt = 0;
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["excessamt"]), out excessAmt);
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["adjamt"]), out adjAmt);
                    double.TryParse(Convert.ToString(ds_stud.Tables[0].Rows[i]["balanceamt"]), out balAmt);

                    dr_Student["Total"] = excessAmt;
                    dr_Student["PaidAmt"] = adjAmt;
                    dr_Student["BalAmt"] = balAmt;
                    dr_Student["ToBePaid"] = "0";
                    tbl_Student.Rows.Add(dr_Student);
                }
                if (tbl_Student.Rows.Count > 0 && txt_staffid.Text.Trim() != "")
                {
                    grid_Details.DataSource = tbl_Student;
                    grid_Details.DataBind();
                    grid_Details.Visible = true;
                    btnSave.Visible = true;
                }
                else
                {
                    grid_Details.DataSource = null;
                    grid_Details.DataBind();
                    grid_Details.Visible = false;
                    btnSave.Visible = false;
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Fees')", true);
                }
            }
            else
            {
                try
                {
                    grid_Details.DataSource = null;
                    grid_Details.DataBind();
                    grid_Details.Visible = false;
                    btnSave.Visible = false;
                }
                catch { }
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Add Fees')", true);
            }
        }
        catch (Exception ex)
        {
            grid_Details.DataSource = null;
            grid_Details.DataBind();
            grid_Details.Visible = false;
            btnSave.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Records Found')", true);
        }

    }

    //Lookup Staff
    protected void btn_staffLook_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = true;
        ddlsearch1_OnSelectedIndexChanged(sender, e);
        btn_staffOK.Visible = false;
        btn_exitstaff.Visible = false;
        Fpspread2.Visible = false;
        lbl_errormsgstaff.Visible = false;
    }

    protected void btn_staffOK_Click(object sender, EventArgs e)
    {
        try
        {

            string actrow = "";
            string actcol = "";
            actrow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            actcol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            if (actrow.Trim() != "" && actrow.Trim() != "-1")
            {
                string staff = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                string appno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Tag);
                string staffcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                txt_staffid.Text = staffcode;
                txtstaffid_Changed(sender, e);

            }
            div_staffLook.Visible = false;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentPayment"); }
    }

    protected void btn_exitstaff_Click(object sender, EventArgs e)
    {
        div_staffLook.Visible = false;
    }

    protected void ddlsearch1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txtsearch1.Text = "";
        txtsearch1c.Text = "";
        txtsearch1c.Visible = false;
        txtsearch1.Visible = false;
        if (ddlsearch1.SelectedIndex == 0)
        {
            txtsearch1.Visible = true;
            Label1.Text = "Search By Name";
        }
        else
        {
            txtsearch1c.Visible = true;
            Label1.Text = "Search By Code";
        }
    }

    protected void Fpspread2staff_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentPayment"); }
    }

    protected void btn_go2Staff_Click(object sender, EventArgs e)
    {
        try
        {
            string clgcode = string.Empty;
            clgcode = getClgCode();
            div_staffLook.Visible = true;
            if (collegecode != null)
            {
                string selq = "";
                if (txtsearch1.Text.Trim() != "")
                {
                    string sname = string.Empty;
                    try
                    {
                        sname = txtsearch1.Text.Trim().Split('-')[0];
                    }
                    catch { sname = txtsearch1.Text.Trim(); }
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_name like '" + Convert.ToString(sname) + "%'";
                }
                else if (txtsearch1c.Text.Trim() != "")
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') and staff_code='" + Convert.ToString(txtsearch1c.Text) + "'";
                }
                else
                {
                    selq = "select appl_id ,h.dept_name,s.staff_name,s.staff_code  from staffmaster s,staff_appl_master a,hrdept_master h where s.appl_no =a.appl_no and a.dept_code =h.dept_code  and s.college_code in('" + clgcode + "') order by PrintPriority";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread2.Sheets[0].RowCount = 0;
                    Fpspread2.Sheets[0].ColumnCount = 0;
                    Fpspread2.CommandBar.Visible = false;
                    Fpspread2.Sheets[0].AutoPostBack = false;
                    Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread2.Sheets[0].RowHeader.Visible = false;
                    Fpspread2.Sheets[0].ColumnCount = 3;
                    Fpspread2.Sheets[0].Columns[0].Width = 60;
                    Fpspread2.Sheets[0].Columns[1].Width = 170;
                    Fpspread2.Sheets[0].Columns[2].Width = 360;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                    FarPoint.Web.Spread.TextCellType chkall = new FarPoint.Web.Spread.TextCellType();


                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        Fpspread2.Sheets[0].RowCount++;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = chkall;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[row]["appl_id"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    }
                    Fpspread2.Visible = true;
                    // div2.Visible = true;
                    lbl_errormsgstaff.Visible = false;
                    Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                    Fpspread2.Width = 620;
                    Fpspread2.Height = 210;
                    if (Fpspread2.Sheets[0].RowCount > 0)
                    {
                        btn_staffOK.Visible = true;
                        btn_exitstaff.Visible = true;
                    }
                    else
                    {
                        btn_staffOK.Visible = false;
                        btn_exitstaff.Visible = false;
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "StudentPayment"); }
    }

    protected string getClgCode()
    {
        string clgCode = string.Empty;
        try
        {
            StringBuilder sbClg = new StringBuilder();
            for (int row = 0; row < ddlcollege.Items.Count; row++)
            {
                sbClg.Append(Convert.ToString(ddlcollege.Items[row].Value) + "','");
            }
            if (sbClg.Length > 0)
            {
                clgCode = Convert.ToString(sbClg.Remove(sbClg.Length - 3, 3));
            }
        }
        catch { }
        return clgCode;
    }

}