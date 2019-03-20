using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class StudentTransferHostelTransport : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;
    static int personmode = 0;
    int userCode = 0;
    static byte roll = 0;
    static int admis = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds1 = new DataSet();
    DataSet ds = new DataSet();
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
            userCode = Convert.ToInt32(Convert.ToString(Session["usercode"]));
            // collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            // collegecode1 = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                loadfromsetting();
                // bindsem();
                // bindHeaderLedger();

                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                rbmode_Selected(sender, e);
                RollAndRegSettings();
                // rbremove_Selected(sender, e);
            }
        }
        catch (Exception ex) { }
    }

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
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    #region Entry


    //protected void bindsem()
    //{
    //    try
    //    {
    //        ddlfeecat.Items.Clear();

    //        if (Session["clgcode"] != null)
    //            collegecode = Convert.ToString(Session["clgcode"]);
    //        else
    //            collegecode = Convert.ToString(Session["collegecode"]);
    //        string sem = "";
    //        string semyear = "select * from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'";
    //        DataSet dsset = new DataSet();
    //        dsset.Clear();
    //        dsset = d2.select_method_wo_parameter(semyear, "Text");
    //        if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
    //        {
    //            string value = Convert.ToString(dsset.Tables[0].Rows[0]["LinkValue"]);
    //            if (value == "1")
    //            {
    //                string SelectQ = "select * from textvaltable where TextCriteria = 'FEECA'and (textval like '%Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(SelectQ, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                    ddlfeecat.DataSource = ds;
    //                    ddlfeecat.DataTextField = "TextVal";
    //                    ddlfeecat.DataValueField = "TextCode";
    //                    ddlfeecat.DataBind();
    //                    ddlfeecat.Items.Insert(0, "Select");
    //                }
    //            }
    //            else
    //            {
    //                string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
    //                ds.Clear();
    //                ds = d2.select_method_wo_parameter(settingquery, "Text");
    //                if (ds.Tables[0].Rows.Count > 0)
    //                {
    //                    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
    //                    if (linkvalue == "0")
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            ddlfeecat.DataSource = ds;
    //                            ddlfeecat.DataTextField = "TextVal";
    //                            ddlfeecat.DataValueField = "TextCode";
    //                            ddlfeecat.DataBind();
    //                            ddlfeecat.Items.Insert(0, "Select");
    //                        }
    //                    }
    //                    else
    //                    {
    //                        string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc";
    //                        ds.Clear();
    //                        ds = d2.select_method_wo_parameter(semesterquery, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
    //                            ddlfeecat.DataSource = ds;
    //                            ddlfeecat.DataTextField = "TextVal";
    //                            ddlfeecat.DataValueField = "TextCode";
    //                            ddlfeecat.DataBind();
    //                            ddlfeecat.Items.Insert(0, "Select");
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch { }
    //}

    protected void bindsem()
    {
        try
        {
            ddlfeecat.Items.Clear();
            //cb_sem.Checked = false;
            //txt_sem.Text = "--Select--";
            if (Session["clgcode"] != null)
                collegecode = Convert.ToString(Session["clgcode"]);
            else
                collegecode = Convert.ToString(Session["collegecode"]);
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(collegecode, usercode, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlfeecat.DataSource = ds;
                ddlfeecat.DataTextField = "TextVal";
                ddlfeecat.DataValueField = "TextCode";
                ddlfeecat.DataBind();
            }
        }
        catch { }
    }

    protected void bindHeaderLedger()
    {
        try
        {
            ddlhed.Items.Clear();
            ddlled.Items.Clear();
            if (txt_roll.Text != "")
            {
                string rollno = Convert.ToString(txt_roll.Text);
                string appno = studAppno(rollno);
                if (appno != "0")
                {
                    if (Session["clgcode"] != null)
                        collegecode = Convert.ToString(Session["clgcode"]);
                    else
                        collegecode = Convert.ToString(Session["collegecode"]);

                    string headerval = d2.GetFunction("select linkvalue from New_InsSettings where LinkName='TransportLedgerValue' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                    if (headerval != "0")
                    {
                        string hedcode = headerval.Split(',')[0];
                    }

                    string Selq = " select h.headerpk,h.headername,l.ledgerpk,l.ledgername from fm_headermaster h, fm_ledgermaster l where h.headerpk=l.headerfk and l.collegecode='" + collegecode + "' and h.headerpk='" + headerval.Split(',')[0] + "' and l.ledgerpk='" + headerval.Split(',')[1] + "'";
                    DataSet dsset = new DataSet();
                    dsset.Clear();
                    dsset = d2.select_method_wo_parameter(Selq, "Text");
                    if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        ddlhed.DataSource = dsset;
                        ddlhed.DataTextField = "headername";
                        ddlhed.DataValueField = "headerpk";
                        ddlhed.DataBind();

                        ddlled.DataSource = dsset;
                        ddlled.DataTextField = "ledgername";
                        ddlled.DataValueField = "ledgerpk";
                        ddlled.DataBind();
                    }
                }
            }
        }
        catch { }
    }

    protected void hostelhedledger()
    {
        try
        {
            ddlhed.Items.Clear();
            ddlled.Items.Clear();
            if (txt_roll.Text != "")
            {
                string rollno = Convert.ToString(txt_roll.Text);
                string appno = studAppno(rollno);
                if (appno != "0")
                {
                    string hostelfi = d2.GetFunction(" select hostelmasterfk from ht_hostelregistration where app_no='" + appno + "'");
                    string selQ = "  select hosteladmfeeheaderfk,hosteladmfeeledgerfk,h.headername,l.ledgername,h.headerpk,l.ledgerpk from hm_hostelmaster ht,fm_headermaster h,fm_ledgermaster l where h.headerpk=l.headerfk and ht.hosteladmfeeheaderfk=h.headerpk and ht.hosteladmfeeledgerfk=l.ledgerpk and hostelmasterpk='" + hostelfi + "'";
                    DataSet dsset = new DataSet();
                    dsset.Clear();
                    dsset = d2.select_method_wo_parameter(selQ, "Text");
                    if (dsset.Tables.Count > 0 && dsset.Tables[0].Rows.Count > 0)
                    {
                        ddlhed.DataSource = dsset;
                        ddlhed.DataTextField = "headername";
                        ddlhed.DataValueField = "headerpk";
                        ddlhed.DataBind();

                        ddlled.DataSource = dsset;
                        ddlled.DataTextField = "ledgername";
                        ddlled.DataValueField = "ledgerpk";
                        ddlled.DataBind();
                    }
                }
                else
                    Response.Write("<script>alert('Please Enter Valid Number')</script>");
            }
            //else
            //    Response.Write("<script>alert('Please Enter Valid Number')</script>");
        }
        catch { }
    }


    #region roll no

    public void loadfromsetting()
    {
        try
        {
            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(lst4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    // txt_roll.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // txt_roll.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // txt_roll.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // txt_roll.Text = "App No";
                    chosedmode = 3;
                    break;
            }



        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_roll.Text = "";
            txt_name.Text = "";
            txt_colg.Text = "";
            txt_strm.Text = "";
            txt_batch.Text = "";
            txt_degree.Text = "";
            txt_dept.Text = "";
            txt_sem.Text = "";
            txt_sec.Text = "";
            txt_seattype.Text = "";
            image2.ImageUrl = "";

            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {

                case 0:
                    txt_roll.Attributes.Add("placeholder", "Roll No");
                    //  rbl_rollno.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                    txt_roll.Attributes.Add("placeholder", "Reg No");
                    // rbl_rollno.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                    txt_roll.Attributes.Add("placeholder", "Admin No");
                    // rbl_rollno.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                    txt_roll.Attributes.Add("placeholder", "App No");
                    // rbl_rollno.Text = "App No";
                    chosedmode = 3;
                    break;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
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
                //and (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0)
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_No like '" + prefixText + "%' order by Roll_No asc ";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Reg_No like '" + prefixText + "%' order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration r where (r.cc=1 or r.cc=0) and (r.Exam_Flag<>'debar' or r.DelFlag=1)  and (r.Exam_Flag like '%debar' or r.DelFlag=0) and Roll_admit like '" + prefixText + "%' order by Roll_admit asc";
                }
                else
                {
                    if (admis == 2)
                    {
                        query = "  select  top 100 app_formno from applyn a ,Registration r where a.app_no=r.App_No and admission_status =1 and selection_status=1 and isconfirm ='1' and DelFlag =0 and app_formno like '" + prefixText + "%' order by app_formno asc";
                    }
                    else
                    {
                        query = "  select  top 100 app_formno from applyn where isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno asc";
                    }
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

    #endregion

    public void txt_roll_TextChanged(object sender, EventArgs e)
    {
        try
        {

            string rollno = Convert.ToString(txt_roll.Text);
            string cursem = "";
            string query = "";
            string transCheck = string.Empty;

            if (!string.IsNullOrEmpty(rollno))
            {
                if (rbremove.SelectedIndex == 0)
                {
                    query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type    from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code and isnull(Bus_RouteID,'')<>'' and isnull(Boarding,'')<>'' and isnull(VehID,'')<>'' and isnull(Trans_PayType,'')<>'' and  Traveller_Date is not null ";
                }
                else
                {
                    query = " select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,seattype,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,(select TextVal from TextValTable where TextCode =ISNULL( a.seattype,0)) as Seat_Type    from applyn a,Registration r,ht_hostelregistration ht ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.app_no=ht.app_no and a.app_no=ht.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code  and isnull(isvacated,'0')<>'1'";
                }
                //and r.Roll_no='" + rollno + "' and d.college_code=" + collegecode1 + "";

                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                        query = query + "and r.Roll_no='" + rollno + "'";

                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        query = query + "and r.Reg_No='" + rollno + "' ";

                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        query = query + "and r.Roll_Admit='" + rollno + "'";
                }
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(query, "Text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        txt_name.Text = ds1.Tables[0].Rows[i]["stud_name"].ToString();
                        txt_batch.Text = ds1.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_degree.Text = ds1.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_dept.Text = ds1.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_seattype.Text = ds1.Tables[0].Rows[i]["Seat_Type"].ToString();
                        cursem = ds1.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_sem.Text = cursem;
                        txt_colg.Text = ds1.Tables[0].Rows[i]["collname"].ToString();
                        txt_strm.Text = ds1.Tables[0].Rows[i]["type"].ToString();
                        txt_sec.Text = ds1.Tables[0].Rows[i]["Sections"].ToString();
                        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        Session["clgcode"] = Convert.ToString(ds1.Tables[0].Rows[i]["college_code"]);
                        if (Session["clgcode"] != null)
                            collegecode1 = Convert.ToString(Session["clgcode"]);
                        else
                            collegecode1 = Convert.ToString(Session["collegecode"]);
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "'");

                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "'");

                    image2.ImageUrl = "~/Handler4.ashx?rollno=" + rollno;
                    enableval();
                    bindsem();
                    if (rbremove.SelectedIndex == 1)
                        hostelhedledger();
                    else
                        bindHeaderLedger();

                    ddlfeecat_OnSelectedIndexChanged(sender, e);
                }
                else
                    clear();
            }
            else
            {
                clear();
                if (rbremove.SelectedIndex == 1)
                    hostelhedledger();
                else
                    bindHeaderLedger();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }
    protected void enableval()
    {
        try
        {
            txt_batch.Enabled = false;
            txt_degree.Enabled = false;
            txt_dept.Enabled = false;
            txt_sec.Enabled = false;
            txt_seattype.Enabled = false;
            txt_sem.Enabled = false;
            txt_colg.Enabled = false;
            txt_strm.Enabled = false;
            txt_name.Enabled = false;
        }
        catch { }
    }
    protected void clear()
    {
        try
        {
            txt_roll.Text = "";
            txt_batch.Text = "";
            txt_degree.Text = "";
            txt_dept.Text = "";
            txt_sec.Text = "";
            txt_seattype.Text = "";
            txt_sem.Text = "";
            txt_colg.Text = "";
            txt_strm.Text = "";
            txt_name.Text = "";
            txtexamt.Text = "";
            txtpaidamt.Text = "";
            ddlfeecat.SelectedIndex = 0;
        }
        catch { }
    }


    protected void btntransfer_Click(object sender, EventArgs e)
    {
        try
        {

            bool save = false;
            bool validate = false;
            string feecat = string.Empty;
            string hdid = string.Empty;
            string ldid = string.Empty;
            double paid = 0;
            double excess = 0;
            double.TryParse(Convert.ToString(txtpaidamt.Text), out paid);
            double.TryParse(Convert.ToString(txtexamt.Text), out excess);
            if (Session["clgcode"] != null)
                collegecode1 = Convert.ToString(Session["clgcode"]);
            else
                collegecode1 = Convert.ToString(Session["collegecode"]);
            string rollno = Convert.ToString(txt_roll.Text);
            if (ddlfeecat.Items.Count > 0)
                feecat = Convert.ToString(ddlfeecat.SelectedItem.Value);
            if (ddlhed.Items.Count > 0)
                hdid = Convert.ToString(ddlhed.SelectedItem.Value);
            if (ddlled.Items.Count > 0)
                ldid = Convert.ToString(ddlled.SelectedItem.Value);

            if (paid != 0 && paid != 0.0)
            {
                if (excess != 0 && excess != 0.0)
                {
                    if (paid >= excess)
                        validate = true;
                }
            }
            else
            {
                if (excess == 0 && excess == 0.0)
                    validate = true;
            }


            if (validate == true && feecat.Trim() != "" && hdid != "" && ldid != "")
            {
                string fromdate = txt_date.Text;
                string[] frdate = fromdate.Split('/');
                if (frdate.Length == 3)
                    fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

                if (!string.IsNullOrEmpty(rollno))
                {
                    string app_no = studAppno(rollno);
                    if (app_no != "0")
                    {
                        string transcode = Convert.ToString(d2.GetFunction("select Transcode from ft_findailytransaction f,registration r where r.app_no=f.app_no and f.app_no='" + app_no + "' and f.feecategory='" + feecat + "' and f.headerfk='" + hdid + "' and f.ledgerfk='" + ldid + "'"));

                        save = Remove(app_no, fromdate, hdid, ldid, paid, excess, feecat, transcode);
                    }
                    if (save == true)
                    {
                        clear();
                        // Response.Write("<script>alert('Saved Successfully')</script>");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Saved Successfully";
                    }
                    else
                    {
                        // Response.Write("<script>alert('Please Enter Correct Values')</script>");
                        imgdiv2.Visible = true;
                        lbl_alert.Text = "Please Enter Correct Values";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "You Enter Wrong Number";
                    //Response.Write("<script>alert('You Enter Wrong Number')</script>");
                }
            }
            else
            {
                txtexamt.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Below or Equal amount to Paid Amount";
                // Response.Write("<script>alert('Please Enter Below or Equal amount to Paid Amount')</script>");
            }
        }
        catch { }
    }

    protected bool Remove(string app_no, string dateval, string hdid, string ldid, double paid, double excess, string feecat, string transcode)
    {
        bool save = false;
        try
        {
            bool regsave = false;
            string UpdReg = "";
            int type = 0;
            if (rbremove.SelectedIndex == 0)
            {
                UpdReg = " update registration set Bus_RouteID='',Boarding='',VehID='',Seat_No='',Trans_PayType='',Traveller_Date = '',IsCanceledStage='0' where app_no='" + app_no + "'";
                d2.update_method_wo_parameter(UpdReg, "text");
                regsave = true;
                type = 1;
            }
            else
            {
                UpdReg = " update ht_hostelregistration set isvacated='1',vacateddate='" + dateval + "' where app_no='" + app_no + "'";
                d2.update_method_wo_parameter(UpdReg, "text");
                regsave = true;
                type = 2;
            }

            if (regsave)
            {
                string finYearid = d2.getCurrentFinanceYear(usercode, collegecode1);
                string insQ = " insert into CO_HostelAndTransportRemove(AppNo,RemoveType,RemoveDate,HeaderFk,Ledgerfk,PaidAmount,ExcessAmount,Feecategroy) values('" + app_no + "','" + type + "','" + dateval + "','" + hdid + "','" + ldid + "','" + paid + "','" + excess + "','" + feecat + "')";
                d2.update_method_wo_parameter(insQ, "text");

                if (excess != 0 && excess != 0.0)
                {
                    string exces = " if exists (select * from FT_ExcessDet where app_no='" + app_no + "' and feeCategory='" + feecat + "' and FinYearFk='" + finYearid + "' and excesstype='1') update  FT_ExcessDet set excessamt= isnull(excessamt,0) +'" + excess + "',adjamt= isnull(adjamt,0) +'0',balanceamt= isnull(balanceamt,0) + '" + excess + "', excesstransdate='" + dateval + "', dailytranscode='" + transcode + "' where app_no='" + app_no + "'  and excesstype='1' and feeCategory='" + feecat + "' and FinYearFk='" + finYearid + "' else insert into FT_ExcessDet(excesstransdate,transtime,dailytranscode,app_no,memtype,excesstype,excessamt,adjamt,balanceamt,feeCategory,FinYearFk) values('" + dateval + "','" + DateTime.Now.ToShortTimeString() + "','" + transcode + "','" + app_no + "','1','1','" + excess + "','0','" + excess + "','" + feecat + "','" + finYearid + "')";
                    d2.update_method_wo_parameter(exces, "text");
                    string excessdetpk = d2.GetFunction("select ExcessDetPK  from FT_ExcessDet where App_No='" + app_no + "' and excessType=1");
                    if (excessdetpk != "0")
                    {
                        //string exceleg = " insert into FT_ExcessLedgerDet(headerfk,ledgerfk,excessamt,adjamt,balanceamt ,excessdetfk,feeCategory,FinYearFk)values('" + hdid + "','" + ldid + "','" + excess + "','" + excess + "','" + excess + "','" + excessdetpk + "','" + feecat + "','" + finYearid + "')";
                        string exceleg = " if exists (select * from FT_ExcessLedgerDet where headerfk='" + hdid + "' and ledgerfk='" + ldid + "' and feeCategory='" + feecat + "' and FinYearFk='" + finYearid + "' and excessdetfk='" + excessdetpk + "') update FT_ExcessLedgerDet set excessamt=isnull(excessamt,0)+'" + excess + "',adjamt=adjamt+'0',balanceamt=isnull(balanceamt,0)+'" + excess + "' where headerfk='" + hdid + "' and ledgerfk='" + ldid + "' and feeCategory='" + feecat + "' and excessdetfk='" + excessdetpk + "' and FinYearFk='" + finYearid + "' else insert into FT_ExcessLedgerDet (headerfk,ledgerfk,excessamt,adjamt,balanceamt ,excessdetfk,feeCategory,FinYearFk)values('" + hdid + "','" + ldid + "','" + excess + "','0','" + excess + "','" + excessdetpk + "','" + feecat + "','" + finYearid + "')";

                        d2.update_method_wo_parameter(exceleg, "text");
                        save = true;
                    }
                }
            }
        }
        catch { }
        return save;
    }


    #endregion

    protected void rbmode_Selected(object sender, EventArgs e)
    {
        if (rbmode.SelectedIndex == 0)
        {
            diventry.Visible = true;
            divreport.Visible = false;
            pnlContents.Visible = false;
            btnExport.Visible = false;
            clear();
            tdentry.Visible = true;
        }
        else
        {
            btnExport.Visible = false;
            pnlContents.Visible = false;
            divreport.Visible = true;
            diventry.Visible = false;
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            bindrptclg();
            tdentry.Visible = false;
            loadType();
        }
    }

    protected void rbremove_Selected(object sender, EventArgs e)
    {
        if (rbremove.SelectedIndex == 0)
        {
            //  ddlfeecat.SelectedIndex = 0;
            bindHeaderLedger();
            clear();
        }
        else
        {
            // ddlfeecat.SelectedIndex = 0;
            hostelhedledger();
            ddlfeecat_OnSelectedIndexChanged(sender, e);
            clear();
        }
    }

    protected void ddlfeecat_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (rbremove.SelectedIndex == 0)
            //{
            string feecode = "";
            string headcode = "";
            string ledcode = "";
            string rollno = "";
            string appno = "";
            double Amt = 0;
            if (txt_roll.Text != "")
            {
                rollno = Convert.ToString(txt_roll.Text);
                appno = studAppno(rollno);
                if (ddlfeecat.Items.Count > 0)
                    feecode = Convert.ToString(ddlfeecat.SelectedItem.Value);
                if (ddlhed.Items.Count > 0)
                    headcode = Convert.ToString(ddlhed.SelectedItem.Value);
                if (ddlled.Items.Count > 0)
                    ledcode = Convert.ToString(ddlled.SelectedItem.Value);

                if (appno != "0" && feecode != "" && headcode != "" && ledcode != "")
                    Amt = Transport(appno, feecode, headcode, ledcode);

                txtpaidamt.Text = Convert.ToString(Amt);
            }
            //}
        }
        catch { }
    }
    protected double Transport(string appno, string feecode, string hedcode, string ledcode)
    {
        double PaidAmt = 0;
        try
        {
            double.TryParse(Convert.ToString(d2.GetFunction("select sum(debit) as paid from ft_findailytransaction f,registration r where r.app_no=f.app_no and f.app_no='" + appno + "' and f.feecategory='" + feecode + "' and f.headerfk='" + hedcode + "' and f.ledgerfk='" + ledcode + "'")), out PaidAmt);
        }
        catch { }
        return PaidAmt;
    }

    protected string studAppno(string rollno)
    {
        string appno = string.Empty;
        try
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                appno = d2.GetFunction(" select App_No from Registration where Roll_No='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                appno = d2.GetFunction(" select App_No from Registration where reg_no='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                appno = d2.GetFunction(" select App_No from Registration where Roll_admit='" + rollno + "'");

            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
                appno = d2.GetFunction(" select app_no from applyn where app_formno='" + rollno + "'");
        }
        catch { }
        return appno;
    }

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }

    protected void loadType()
    {
        try
        {
            ddltype.Items.Clear();
            ddltype.Items.Add(new ListItem("All", "0"));
            ddltype.Items.Add(new ListItem("Transport", "1"));
            ddltype.Items.Add(new ListItem("Hostel", "2"));
        }
        catch { }
    }

    #region Report

    public void bindrptclg()
    {
        try
        {
            ds.Clear();
            ddlrptclg.Items.Clear();

            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlrptclg.DataSource = ds;
                ddlrptclg.DataTextField = "collname";
                ddlrptclg.DataValueField = "college_code";
                ddlrptclg.DataBind();
            }
            //bindBtch();
            //binddeg();
            //binddept();
            //bindsem();
            //bindsect();
            //bindstream();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "TransferRefund"); }
    }

    protected void ddlrptclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void btnrptgo_Click(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            ds = loadDatasetDet();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RollAndRegSettings();
                loadReportDet(ds);
            }
            else
            {
                gdrpt.Visible = false;
                btnExport.Visible = false;
                pnlContents.Visible = false;
                //Response.Write("<script>alert('No Record Found')</script>");
                imgdiv2.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
        catch { }
    }

    protected DataSet loadDatasetDet()
    {
        DataSet dsload = new DataSet();
        try
        {
            string collegecode = "";
            string modetype = "";
            if (ddlrptclg.Items.Count > 0)
                collegecode = Convert.ToString(ddlrptclg.SelectedItem.Value);

            if (ddltype.Items.Count > 0)
            {
                if (ddltype.SelectedIndex != 0)
                    modetype = Convert.ToString(ddltype.SelectedItem.Value);
            }
            string fromdate = txt_fromdate.Text;
            string todate = txt_todate.Text;
            string[] frdate = fromdate.Split('/');
            if (frdate.Length == 3)
                fromdate = frdate[1].ToString() + "/" + frdate[0].ToString() + "/" + frdate[2].ToString();

            string[] tdate = todate.Split('/');
            if (tdate.Length == 3)
                todate = tdate[1].ToString() + "/" + tdate[0].ToString() + "/" + tdate[2].ToString();

            string SeleQ = " select r.roll_no,r.reg_no,r.roll_admit,r.stud_name, hr.AppNo,RemoveType, convert(varchar(10),RemoveDate,103) as RemoveDate ,HeaderFk,Ledgerfk,PaidAmount as paid,ExcessAmount as excess,Feecategroy,r.degree_code from CO_HostelAndTransportRemove hr,registration r where hr.appno=r.app_no  and removedate between '" + fromdate + "' and '" + todate + "' and r.college_code='" + collegecode + "' ";
            if (modetype != "")
                SeleQ += " and RemoveType in('" + modetype + "')";

            SeleQ += " select d.Degree_Code,(c.Course_Name +'-'+ dt.Dept_Name) as degreename,(c.Course_Name +'-'+dt.dept_acronym)as dept_acronym from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id ";
            //and d.college_code ='" + collegecode + "'
            SeleQ += " select textcode,textval from textvaltable where textcriteria='FEECA' and college_code='" + collegecode + "'";
            SeleQ += " select h.headerpk,h.headername,l.ledgerpk,l.ledgername from fm_headermaster h, fm_ledgermaster l where h.headerpk=l.headerfk and l.collegecode='" + collegecode + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SeleQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void loadReportDet(DataSet ds)
    {
        try
        {
            double paidtot = 0;
            double excessTot = 0;
            double paid = 0;
            double excess = 0;
            Dictionary<string, int> dictcol = new Dictionary<string, int>();
            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("Sno");
            dtrpt.Columns.Add("Roll No");
            dtrpt.Columns.Add("Reg No");
            dtrpt.Columns.Add("Admission No");
            dtrpt.Columns.Add("Name");
            dtrpt.Columns.Add("Date");
            dtrpt.Columns.Add(lbldept.Text);
            dtrpt.Columns.Add("Type");
            dtrpt.Columns.Add("Feecategory");
            dtrpt.Columns.Add("Header");
            dtrpt.Columns.Add("Ledger");
            dtrpt.Columns.Add("Paid Amount");
            dtrpt.Columns.Add("Excess Amount");
            DataRow drrpt;
            if (dtrpt.Columns.Count > 0)
            {
                for (int dsrow = 0; dsrow < ds.Tables[0].Rows.Count; dsrow++)
                {
                    drrpt = dtrpt.NewRow();
                    drrpt["Sno"] = Convert.ToString(dsrow + 1);
                    drrpt["Roll No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["roll_no"]);
                    drrpt["Reg No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["reg_no"]);
                    drrpt["Admission No"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["Roll_Admit"]);
                    drrpt["Name"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["stud_name"]);
                    drrpt["Date"] = Convert.ToString(ds.Tables[0].Rows[dsrow]["RemoveDate"]);
                    string frdept = deptName(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["degree_code"]));
                    drrpt[lbldept.Text] = frdept;
                    int type = 0;
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[dsrow]["RemoveType"]), out type);
                    if (type == 1)
                        drrpt["Type"] = "Transport";
                    else
                        drrpt["Type"] = "Hostel";

                    string frclg = feeval(ds, Convert.ToString(ds.Tables[0].Rows[dsrow]["Feecategroy"]));
                    drrpt["Feecategory"] = frclg;
                    DataView Dview = new DataView();
                    if (ds.Tables[3].Rows.Count > 0)
                    {
                        ds.Tables[3].DefaultView.RowFilter = "headerpk='" + Convert.ToString(ds.Tables[0].Rows[dsrow]["headerfk"]) + "' and ledgerpk='" + Convert.ToString(ds.Tables[0].Rows[dsrow]["ledgerfk"]) + "'";
                        Dview = ds.Tables[3].DefaultView;
                        if (Dview.Count > 0)
                        {
                            drrpt["Header"] = Convert.ToString(Dview[0]["headername"]);
                            drrpt["Ledger"] = Convert.ToString(Dview[0]["ledgername"]);
                        }
                    }

                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[dsrow]["paid"]), out paid);
                    double.TryParse(Convert.ToString(ds.Tables[0].Rows[dsrow]["excess"]), out excess);
                    drrpt["Paid Amount"] = Convert.ToString(paid);
                    drrpt["Excess Amount"] = Convert.ToString(excess);
                    paidtot += paid;
                    excessTot += excess;
                    dtrpt.Rows.Add(drrpt);
                }
                drrpt = dtrpt.NewRow();
                drrpt["Sno"] = Convert.ToString("Total");
                dictcol.Add(Convert.ToString("Total" + "-" + Convert.ToInt32(dtrpt.Rows.Count)), Convert.ToInt32(dtrpt.Rows.Count));
                drrpt["Paid Amount"] = Convert.ToString(paidtot);
                drrpt["Excess Amount"] = Convert.ToString(paidtot);
                dtrpt.Rows.Add(drrpt);
            }
            if (dtrpt.Rows.Count > 0)
            {
                gdrpt.DataSource = dtrpt;
                gdrpt.DataBind();
                columnCount();
                gdrpt.Visible = true;
                btnExport.Visible = true;
                pnlContents.Visible = true;
                printCollegeDet();
                gridColumnsVisible();
                spanGridColumnns(dictcol);
            }
        }
        catch { }
    }

    protected void columnCount()
    {
        try
        {
            int Cnt = gdrpt.Rows[0].Cells.Count;
            if (Cnt > 10)
                btnExport.Text = "Print A3 Format";
            else
                btnExport.Text = "Print A4 Format";
        }
        catch { }
    }
    protected void printCollegeDet()
    {
        try
        {
            string colquery = "select collname,university,address1 ,address2,address3+' - '+pincode as address3 from collinfo where college_code=" + ddlrptclg.SelectedItem.Value + " ";

            string collegename = "";
            string add1 = "";
            string add2 = "";
            string add3 = "";
            string univ = "";
            string feedet = "";
            ds = d2.select_method_wo_parameter(colquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                add1 = Convert.ToString(ds.Tables[0].Rows[0]["address1"]);
                add2 = Convert.ToString(ds.Tables[0].Rows[0]["address2"]);
                add3 = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                add1 += " " + add2;
                spCollege.InnerText = collegename;
                spAffBy.InnerText = add1;
                spController.InnerText = add3;
                spSeating.InnerText = univ;
                spDateSession.InnerText = "STUDENT TRANSFER DETAILS-" + DateTime.Now.ToString("dd.MM.yyyy") + "";
            }
        }
        catch { }
    }

    protected string deptName(DataSet ds, string deptcode)
    {
        string Degreename = "";
        try
        {
            DataView Dview = new DataView();
            if (ds.Tables[1].Rows.Count > 0)
            {
                ds.Tables[1].DefaultView.RowFilter = "Degree_code='" + deptcode + "'";
                Dview = ds.Tables[1].DefaultView;
                if (Dview.Count > 0)
                    Degreename = Convert.ToString(Dview[0]["degreename"]);
            }
        }
        catch { }
        return Degreename;
    }
    protected string feeval(DataSet ds, string clgcode)
    {
        string collname = "";
        try
        {
            DataView Dview = new DataView();
            if (ds.Tables[2].Rows.Count > 0)
            {
                ds.Tables[2].DefaultView.RowFilter = "Textcode='" + clgcode + "'";
                Dview = ds.Tables[2].DefaultView;
                if (Dview.Count > 0)
                    collname = Convert.ToString(Dview[0]["textval"]);
            }
        }
        catch { }
        return collname;
    }



    protected void gdrpt_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        ////Add CSS class on header row.
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.CssClass = "header";
            e.Row.Cells[0].Width = 50;
            e.Row.Cells[1].Width = 400;
            e.Row.Cells[2].Width = 500;
            e.Row.Cells[3].Width = 300;
            e.Row.Cells[4].Width = 300;
            e.Row.Cells[5].Width = 300;
            e.Row.Cells[6].Width = 300;
            e.Row.Cells[7].Width = 300;
            e.Row.Cells[8].Width = 300;
            e.Row.Cells[9].Width = 300;
            // e.Row.Cells[10].Width = 250;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
        }
        //column merge first and last
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            if (e.Row.Cells[0].Text.Trim() == "Total")
            {
                e.Row.Cells[0].ColumnSpan = 2;
                e.Row.Cells.RemoveAt(1);
                e.Row.Cells[0].BackColor = Color.YellowGreen;
                e.Row.Cells[0].Font.Bold = true;
                e.Row.Cells[0].Font.Size = 12;
            }
        }
    }

    protected void spanGridColumnns(Dictionary<string, int> gdcol)
    {
        try
        {
            foreach (KeyValuePair<string, int> gdval in gdcol)
            {
                int rowCnt = Convert.ToInt32(gdval.Value.ToString());
                string rowVal = gdval.Key.ToString();
                string spltxt = rowVal.Contains('-') ? rowVal.Split('-')[0].ToString() : "";
                int Cnt = gdrpt.Rows[rowCnt].Cells.Count;
                if (gdrpt.Rows[rowCnt].Cells[0].Text.Trim() == spltxt)
                {
                    for (int i = 0; i < gdrpt.Rows[rowCnt].Cells.Count; i++)
                    {
                        gdrpt.Rows[rowCnt].Cells[i].BackColor = Color.YellowGreen;
                        gdrpt.Rows[rowCnt].Cells[i].Font.Bold = true;
                        gdrpt.Rows[rowCnt].Cells[i].Font.Size = 12;
                    }
                }
            }
        }
        catch { }
    }
    protected void gridColumnsVisible()
    {
        try
        {
            int a = gdrpt.Columns.Count;

            for (int i = 0; i < gdrpt.Rows.Count; i++)
            {
                if (roll == 0)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = true;
                    gdrpt.HeaderRow.Cells[2].Visible = true;
                    gdrpt.HeaderRow.Cells[3].Visible = true;
                    gdrpt.Rows[i].Cells[1].Visible = true;
                    gdrpt.Rows[i].Cells[2].Visible = true;
                    gdrpt.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 1)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = true;
                    gdrpt.HeaderRow.Cells[2].Visible = true;
                    gdrpt.HeaderRow.Cells[3].Visible = true;
                    gdrpt.Rows[i].Cells[1].Visible = true;
                    gdrpt.Rows[i].Cells[2].Visible = true;
                    gdrpt.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 2)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = true;
                    gdrpt.HeaderRow.Cells[2].Visible = false;
                    gdrpt.HeaderRow.Cells[3].Visible = false;
                    gdrpt.Rows[i].Cells[1].Visible = true;
                    gdrpt.Rows[i].Cells[2].Visible = false;
                    gdrpt.Rows[i].Cells[3].Visible = false;
                }
                else if (roll == 3)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = false;
                    gdrpt.HeaderRow.Cells[2].Visible = true;
                    gdrpt.HeaderRow.Cells[3].Visible = false;
                    gdrpt.Rows[i].Cells[1].Visible = false;
                    gdrpt.Rows[i].Cells[2].Visible = true;
                    gdrpt.Rows[i].Cells[3].Visible = false;
                }
                else if (roll == 4)
                {
                    // gdrpt.Rows[i].
                    gdrpt.HeaderRow.Cells[1].Visible = false;
                    gdrpt.HeaderRow.Cells[2].Visible = false;
                    gdrpt.HeaderRow.Cells[3].Visible = true;
                    gdrpt.Rows[i].Cells[1].Visible = false;
                    gdrpt.Rows[i].Cells[2].Visible = false;
                    gdrpt.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 5)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = true;
                    gdrpt.HeaderRow.Cells[2].Visible = true;
                    gdrpt.HeaderRow.Cells[3].Visible = false;
                    gdrpt.Rows[i].Cells[1].Visible = true;
                    gdrpt.Rows[i].Cells[2].Visible = true;
                    gdrpt.Rows[i].Cells[3].Visible = false;

                }
                else if (roll == 6)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = false;
                    gdrpt.HeaderRow.Cells[2].Visible = true;
                    gdrpt.HeaderRow.Cells[3].Visible = true;
                    gdrpt.Rows[i].Cells[1].Visible = false;
                    gdrpt.Rows[i].Cells[2].Visible = true;
                    gdrpt.Rows[i].Cells[3].Visible = true;
                }
                else if (roll == 7)
                {
                    gdrpt.HeaderRow.Cells[1].Visible = true;
                    gdrpt.HeaderRow.Cells[2].Visible = false;
                    gdrpt.HeaderRow.Cells[3].Visible = true;
                    gdrpt.Rows[i].Cells[1].Visible = true;
                    gdrpt.Rows[i].Cells[2].Visible = false;
                    gdrpt.Rows[i].Cells[3].Visible = true;
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

        lbl.Add(lblclg);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        lbl.Add(lblsem);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);

        //
        //lbl.Add(lblclgs);
        //lbl.Add(lbl_str2);
        //lbl.Add(lbldegs);
        //lbl.Add(lbldepts);
        //lbl.Add(lblsems);
        //fields.Add(0);
        //fields.Add(1);
        //fields.Add(2);
        //fields.Add(3);
        //fields.Add(4);
        //     


        lbl.Add(lblrptclg);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);

    }

    // last modified 12-11-2016 sudhagar
}