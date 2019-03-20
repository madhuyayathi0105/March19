using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;
using System.IO;
using System.Collections;
using BalAccess;


public partial class StaffReport : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    DataSet dssrall = new DataSet();
    DataSet ds = new DataSet();
    SqlDataAdapter da = new SqlDataAdapter();
    Hashtable hat = new Hashtable();
    DAccess2 oda = new DAccess2();
    DAccess2 d2 = new DAccess2();
    SqlCommand cmddept, cmdcate, cmddesi, cmdtype;
    string user_code, college, sqlcmddepartment, sqlcmddesignation, sqlcmdcategory, sqlcmdstafftype, sqlcmdall;
    DataSet dssr = new DataSet();
    public int gldeptcode, gdesicode;
    public string gstafftype, gcategory;
    static int bloodcnt = 0;
    static int typecnt = 0;
    static int catcnt = 0;
    static int seatcnt = 0;
    string strcategory = "";
    string strdept = "";
    string strstafftype = "";
    string strdesi = "";
    //int expcountmasteryear;
    //int expcountmastermonth;
    //int exptrialyear;
    //int exptrialmonth;
    //int nonmasteryear;
    //int nonmastermonth;
    //int nontrialyear;
    //int nontrialmonth;
    //int totalyear;
    //int totalmonth;
    int researchcountmastr;
    int nationalmastercount, internationalmastercount;
    int patentmastercount;
    int pgmyguide, bookpubcount;
    int phdmyguide;
    int otheraoolcount, hracount, dacount, tempothercountallo;
    Boolean Cellclick = false;
    string appno;
    //Added By Srinath 1/4/2013
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";

    //==========these variables declared by Manikandan=====================
    int exp_in_months = 0;
    int exp_in_years = 0;
    string join_date = string.Empty;
    //============================End======================================

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
            //            Response.Redirect(redrURI, false);
            //            return;
            //        }
            //    }
            //}
            //if (Session["collegecode"] == null)
            //{
            //    string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
            //    Response.Redirect(redrURI, false);
            //    return;
            //}
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            lblother.Visible = false;
            if (!IsPostBack)
            {
                user_code = Session["UserCode"].ToString();
                college = Session["collegecode"].ToString();
                retrivedepartment(college);
                retrivedesignation(college);
                retrivecategory(college);
                retrivestafftype(college);
                btnexportexcel.Visible = false;
                btnprintmaster.Visible = false;
                // Spread Default Design

                FpSpread1.Sheets[0].AutoPostBack = true;
                FpSpread1.CommandBar.Visible = true;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.SheetCorner.Columns[0].Visible = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                // style1.BackColor = Color.White;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].SheetCorner.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                //FpSpread1.Sheets[0].RowHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Width = 100;

                //---------------page number

                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.Visible = false;
                btnprintmaster.Visible = false;

            }
            lblother.Visible = false;
        }
        catch (Exception ex) { }
    }

    // method for go button
    protected void btgo_Click(object sender, EventArgs e)
    {
        methodgo();
    }

    // load function for department

    public void retrivedepartment(string college)
    {

        try
        {
            chkselect.Checked = false;
            cbldepttype.Visible = true;
            cbldepttype.Items.Clear();
            ds.Clear();
            ListItem lsitem = new ListItem();
            // ds = oda.loaddepartment(college);
            string deptquery = "";
            Hashtable hat = new Hashtable();
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
            }
            if (deptquery != "")
            {
                ds = oda.select_method(deptquery, hat, "Text");
                cbldepttype.DataSource = ds;
                cbldepttype.DataTextField = "dept_name";
                cbldepttype.DataValueField = "Dept_Code";
                cbldepttype.DataBind();
            }
            if (cbldepttype.Items.Count > 0)
            {
                for (int i = 0; i < cbldepttype.Items.Count; i++)
                {
                    cbldepttype.Items[i].Selected = true;
                }
                chkselect.Checked = true;
                tbseattype.Text = "Department (" + cbldepttype.Items.Count + ")";
            }

        }
        catch (Exception e)
        {
            lblother.Text = e.ToString();
        }

    }

    // load function for designation
    public void retrivedesignation(string college)
    {
        try
        {
            cbldesi.Visible = true;
            cbldesi.Items.Clear();
            ds.Clear();
            ListItem lsitem = new ListItem();
            ds = oda.loaddesignation(college);
            cbldesi.DataSource = ds;
            cbldesi.DataTextField = "desig_name";
            cbldesi.DataValueField = "Desig_Code";
            cbldesi.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < cbldesi.Items.Count; i++)
                {
                    cbldesi.Items[i].Selected = true;
                }
                chkdesi.Checked = true;
                txtdesi.Text = "Designation (" + cbldesi.Items.Count + ")";
            }
        }
        catch (Exception e)
        {
            lblother.Text = e.ToString();
        }
    }

    // load function for category

    public void retrivecategory(string college)
    {
        try
        {
            cblcategory.Visible = true;
            cblcategory.Items.Clear();
            ds.Clear();
            ListItem lsitem = new ListItem();
            ds = oda.loadcategory(college);
            cblcategory.DataSource = ds;
            cblcategory.DataTextField = "category_name";
            cblcategory.DataValueField = "Category_Code";
            cblcategory.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < cblcategory.Items.Count; i++)
                {
                    cblcategory.Items[i].Selected = true;
                }
                chkcategory.Checked = true;
                txtcategory.Text = "Category (" + cblcategory.Items.Count + ")";
            }
        }
        catch (Exception e)
        {
            lblother.Text = e.ToString();
        }
    }

    // load function for stafftype

    public void retrivestafftype(string college)
    {
        try
        {
            cblstafftype.Visible = true;
            cblstafftype.Items.Clear();
            ds.Clear();
            ListItem lsitem = new ListItem();
            ds = oda.loadstafftype(college);
            cblstafftype.DataSource = ds;
            cblstafftype.DataTextField = "StfType";
            cblstafftype.DataBind();
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < cblstafftype.Items.Count; i++)
                {
                    cblstafftype.Items[i].Selected = true;
                }
                chkstafftype.Checked = true;
                txtstafftype.Text = "Staff Type (" + cblstafftype.Items.Count + ")";
            }
        }
        catch (Exception e)
        {
            lblother.Text = e.ToString();
        }
    }

    public void methodgo()
    {
        // sqlcmdall = "SELECT Title,Appl_Name,A.Mid_Name,NameAcr,Sex,family_info,Per_Address,Per_Address1,Per_Pincode,PCity,PState,Religion,Caste,Date_of_Birth,PANGIRNumber,per_phone,Per_MobileNo,Email,Per_Fax,G.Desig_Name,CASE WHEN StfNature = 'full' THEN 'FT' ELSE 'PT' END StfNature,P.NetAdd,T.StfStatus,T.FacultyType,T.PayType,T.Programme as Programme,D.Dept_Name as Course,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,PFNumber,Join_Date,replace ( Qualification,'''\''','$') as Qualification,Experience_info,Research_Info,BankAccount,Bank_Name,Branch_Name,IFSC_Code,Journal_Publication,Patent_Received,Project_Grants,Guide_Ship,Books_published,CASE WHEN IsPhy = 1 THEN 'Y' ELSE 'N' END IsPhy,CASE WHEN Minority = 1 THEN 'Y' ELSE 'N' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Y' ELSE 'N' END AICTE_Comm,CASE WHEN AICTE_Grants = 1 THEN 'Y' ELSE 'N' END AICTE_Grants,P.BSalary,P.Allowances FROM Staff_Appl_Master A,StaffMaster M,StaffTrans T,Desig_Master G,HrDept_Master D,MonthlyPay P WHERE A.Appl_No = M.Appl_No AND A.College_Code = M.College_Code AND M.Staff_Code = T.Staff_Code AND T.Desig_Code = G.Desig_Code And T.Dept_Code= D.Dept_Code And M.College_Code=D.College_Code AND M.College_Code = G.CollegeCode AND M.Staff_Code = P.Staff_Code AND M.College_Code = P.College_Code AND T.Latestrec = 1 AND P.Latestrec = 1";
        //sqlcmdall="SELECT Title,Appl_Name,A.Mid_Name,NameAcr,Sex,family_info,Per_Address,Per_Address1,Per_Pincode,PCity,PState,Religion,Caste,Convert (Nvarchar(12),Date_of_Birth,103) as Date_of_Birth ,PANGIRNumber,per_phone,Per_MobileNo,Email,Per_Fax,G.Desig_Name,CASE WHEN StfNature = 'full' THEN 'FT' ELSE 'PT' END StfNature,P.NetAdd,T.StfStatus,T.FacultyType,T.PayType,T.Programme as Programme,D.Dept_Name as Course,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,PFNumber,Convert (Nvarchar(12),Join_Date,103) as Join_Date,replace ( Qualification,'''''','$') as Qualification,Experience_info,Research_Info,BankAccount,Bank_Name,Branch_Name,IFSC_Code,Journal_Publication,Patent_Received,Project_Grants,Guide_Ship,Books_published,CASE WHEN IsPhy = 1 THEN 'Y' ELSE 'N' END IsPhy,CASE WHEN Minority = 1 THEN 'Y' ELSE 'N' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Y' ELSE 'N' END AICTE_Comm,CASE WHEN AICTE_Grants = 1 THEN 'Y' ELSE 'N' END AICTE_Grants,P.BSalary,P.Allowances FROM Staff_Appl_Master A,StaffMaster M,StaffTrans T,Desig_Master G,HrDept_Master D,MonthlyPay P WHERE A.Appl_No = M.Appl_No AND A.College_Code = M.College_Code AND M.Staff_Code = T.Staff_Code AND T.Desig_Code = G.Desig_Code And T.Dept_Code= D.Dept_Code And M.College_Code=D.College_Code AND M.College_Code = G.CollegeCode AND M.Staff_Code = P.Staff_Code AND M.College_Code = P.College_Code AND T.Latestrec = 1 AND P.Latestrec = 1";
        sqlcmdall = "SELECT M.staff_code,Title,Appl_Name,A.Mid_Name,NameAcr,Sex,family_info,Per_Address,Per_Address1,Per_Pincode,PCity,PState,Religion,Caste,Convert (Nvarchar(12),Date_of_Birth,103) as Date_of_Birth ,PANGIRNumber,per_phone,Per_MobileNo,Email,Per_Fax,G.Desig_Name,CASE WHEN StfNature = 'full' THEN 'FT' ELSE 'PT' END StfNature,T.StfStatus,T.FacultyType,T.PayType,T.Programme as Programme,D.Dept_Name as Course,CASE WHEN PayMode = 0 THEN 'Cash' WHEN PayMode = 1 THEN 'Cheque' ELSE 'Credit to Bank Account' END PayMode,PFNumber,Convert (Nvarchar(12),Join_Date,103) as Join_Date,replace ( Qualification,'''''','$') as Qualification,Experience_info,Research_Info,BankAccount,Bank_Name,Branch_Name,IFSC_Code,Journal_Publication,Patent_Received,Project_Grants,Guide_Ship,Books_published,CASE WHEN IsPhy = '1' THEN 'Y' ELSE 'N' END IsPhy,CASE WHEN Minority = 1 THEN 'Y' ELSE 'N' END IsMin,'' as IsFirstYr,'' as IsFYCommon,'' as FYCommonSub,CASE WHEN AICTE_Comm = 1 THEN 'Y' ELSE 'N' END AICTE_Comm,CASE WHEN AICTE_Grants = 1 THEN 'Y' ELSE 'N' END AICTE_Grants FROM Staff_Appl_Master A ";
        sqlcmdall = sqlcmdall + "INNER JOIN StaffMaster M ON A.Appl_No = M.Appl_No AND A.College_Code = M.College_Code INNER JOIN StaffTrans T ON M.Staff_Code = T.Staff_Code ";
        sqlcmdall = sqlcmdall + "INNER JOIN Desig_Master G ON T.Desig_Code = G.Desig_Code AND M.College_Code = G.CollegeCode INNER JOIN HrDept_Master D ON T.Dept_Code= D.Dept_Code And M.College_Code=D.College_Code ";
        sqlcmdall = sqlcmdall + "WHERE ((M.Resign = 0 AND Settled = 0) and (M.Discontinue=0 or M.Discontinue is null)) AND T.Latestrec = 1 ";

        int itemcount = 0;


        for (itemcount = 0; itemcount < cbldepttype.Items.Count; itemcount++)
        {
            if (cbldepttype.Items[itemcount].Selected == true)
            {
                if (strdept == "")
                    strdept = "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
                else
                    strdept = strdept + "," + "'" + cbldepttype.Items[itemcount].Value.ToString() + "'";
            }
        }

        if (strdept != "")
        {
            strdept = " in(" + strdept + ")";
        }
        else
        {
            btnexportexcel.Visible = false;
            btnprintmaster.Visible = false;
            FpSpread1.Visible = false;
            lblother.Visible = true;
            lblother.Text = "Please Select The Department And Then Proceed";
            return;
        }
        sqlcmdall = sqlcmdall + " and T.Dept_Code  " + strdept + "";

        itemcount = 0;
        for (itemcount = 0; itemcount < cbldesi.Items.Count; itemcount++)
        {
            if (cbldesi.Items[itemcount].Selected == true)
            {
                if (strdesi == "")
                    strdesi = "'" + cbldesi.Items[itemcount].Value.ToString() + "'";
                else
                    strdesi = strdesi + "," + "'" + cbldesi.Items[itemcount].Value.ToString() + "'";
            }
        }
        if (strdesi != "")
        {
            strdesi = " in(" + strdesi + ")";
        }
        else
        {
            btnexportexcel.Visible = false;
            btnprintmaster.Visible = false;
            FpSpread1.Visible = false;
            lblother.Visible = true;
            lblother.Text = "Please Select The Designation And Then Proceed";
            return;
        }
        sqlcmdall = sqlcmdall + " and T.Desig_Code  " + strdesi + "";

        itemcount = 0;
        for (itemcount = 0; itemcount < cblcategory.Items.Count; itemcount++)
        {
            if (cblcategory.Items[itemcount].Selected == true)
            {
                if (strcategory == "")
                    strcategory = "'" + cblcategory.Items[itemcount].Value.ToString() + "'";
                else
                    strcategory = strcategory + "," + "'" + cblcategory.Items[itemcount].Value.ToString() + "'";
            }
        }
        if (strcategory != "")
        {
            strcategory = " in (" + strcategory + ")";
        }
        else
        {
            btnexportexcel.Visible = false;
            btnprintmaster.Visible = false;
            FpSpread1.Visible = false;
            lblother.Visible = true;
            lblother.Text = "Please Select The Category And Then Proceed";
            return;
        }
        sqlcmdall = sqlcmdall + "  and T.Category_Code" + strcategory + "";

        itemcount = 0;
        for (itemcount = 0; itemcount < cblstafftype.Items.Count; itemcount++)
        {
            if (cblstafftype.Items[itemcount].Selected == true)
            {
                if (strstafftype == "")
                    strstafftype = "'" + cblstafftype.Items[itemcount].Value.ToString() + "'";
                else
                    strstafftype = strstafftype + "," + "'" + cblstafftype.Items[itemcount].Value.ToString() + "'";
            }
        }
        if (strstafftype != "")
        {
            strstafftype = " in(" + strstafftype + ")";
        }
        else
        {
            btnexportexcel.Visible = false;
            btnprintmaster.Visible = false;
            FpSpread1.Visible = false;
            lblother.Visible = true;
            lblother.Text = "Please Select The Staff Type And Then Proceed";
            return;
        }
        sqlcmdall = sqlcmdall + " and T.StfType  " + strstafftype + "  and resign = 0 and settled = 0 and isnull(Discontinue,'0')='0'";
        sqlcmdall = sqlcmdall + " select m.BSalary,m.Allowances,NetAdd,m.staff_code from staffmaster s,stafftrans t,monthlypay m,hrdept_master h,desig_master d where s.staff_code =t.staff_code and s.staff_code =m.staff_code and m.staff_code =s.staff_code and t.dept_code=h.dept_code and t.desig_code=d.desig_code and s.college_code=m.college_code and s.college_code=h.college_code and s.college_code=d.collegeCode and t.latestrec ='1' and resign =0 and settled =0 and isnull(Discontinue,'0')='0' and t.dept_code " + strdept + " and t.desig_code " + strdesi + " and t.category_code " + strcategory + " and t.stftype " + strstafftype + " order by fdate desc";

        retrivedetailsfull(sqlcmdall);
    }

    //method for go button

    public void retrivedetailsfull(string sqlcmdallm)
    {
        try
        {
            FpSpread1.Sheets[0].ColumnCount = 63;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Title";
            FpSpread1.Sheets[0].Columns[1].Width = 50;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Surname";
            FpSpread1.Sheets[0].Columns[2].Width = 150;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "First Name";
            FpSpread1.Sheets[0].Columns[3].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Middle Name";
            FpSpread1.Sheets[0].Columns[4].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
            FpSpread1.Sheets[0].Columns[5].Width = 100;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Father's Name";
            FpSpread1.Sheets[0].Columns[6].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Mother's Name";
            FpSpread1.Sheets[0].Columns[7].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Address Line1";
            FpSpread1.Sheets[0].Columns[8].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Address Line2";
            FpSpread1.Sheets[0].Columns[9].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Postal Code";
            FpSpread1.Sheets[0].Columns[10].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "City/Village";
            FpSpread1.Sheets[0].Columns[11].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 12].Text = "State";
            FpSpread1.Sheets[0].Columns[12].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Religion";
            FpSpread1.Sheets[0].Columns[13].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 14].Text = "Caste";
            FpSpread1.Sheets[0].Columns[14].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 15].Text = "Date of Birth";
            FpSpread1.Sheets[0].Columns[15].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 16].Text = "PAN";
            FpSpread1.Sheets[0].Columns[16].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 17].Text = "STD Code";
            FpSpread1.Sheets[0].Columns[17].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Land Line #";
            FpSpread1.Sheets[0].Columns[18].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Mobile Phone #";
            FpSpread1.Sheets[0].Columns[19].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Email Address";
            FpSpread1.Sheets[0].Columns[20].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 21].Text = "Fax Phone #";
            FpSpread1.Sheets[0].Columns[21].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 22].Text = "Exact Designation";
            FpSpread1.Sheets[0].Columns[22].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 23].Text = "Appointment FT/PT";
            FpSpread1.Sheets[0].Columns[23].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 24].Text = "Gross Pay per Month";
            FpSpread1.Sheets[0].Columns[24].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Appointment Type";
            FpSpread1.Sheets[0].Columns[25].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 26].Text = "Faculty Type";
            FpSpread1.Sheets[0].Columns[26].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 27].Text = "PayScale";
            FpSpread1.Sheets[0].Columns[27].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 28].Text = "Programme";
            FpSpread1.Sheets[0].Columns[28].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 29].Text = "Course";
            FpSpread1.Sheets[0].Columns[29].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 30].Text = "Salary Mode";
            FpSpread1.Sheets[0].Columns[30].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 31].Text = "PF Number";
            FpSpread1.Sheets[0].Columns[31].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 32].Text = "Date of Joining";
            FpSpread1.Sheets[0].Columns[32].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 33].Text = "Doctorate Degree";
            FpSpread1.Sheets[0].Columns[33].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 34].Text = "PG Degree";
            FpSpread1.Sheets[0].Columns[34].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 35].Text = "UG Degree";
            FpSpread1.Sheets[0].Columns[35].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 36].Text = "Other Qualification's";
            FpSpread1.Sheets[0].Columns[36].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 37].Text = "Area of Specialization";
            FpSpread1.Sheets[0].Columns[37].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 38].Text = "Teaching Experience in Years";
            FpSpread1.Sheets[0].Columns[38].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 39].Text = "Total Work Experience in Years";
            FpSpread1.Sheets[0].Columns[39].Width = 200;
            //============================================================
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 40].Text = "Total Work Experience in Months";
            FpSpread1.Sheets[0].Columns[39].Width = 200;
            //============================================================
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 41].Text = "Research Experience in Years";
            FpSpread1.Sheets[0].Columns[40].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 42].Text = "BankAccountNumber";
            FpSpread1.Sheets[0].Columns[41].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 43].Text = "BankName";
            FpSpread1.Sheets[0].Columns[42].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 44].Text = "Bank Branch Name";
            FpSpread1.Sheets[0].Columns[43].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 45].Text = "IFSC Code";
            FpSpread1.Sheets[0].Columns[44].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 46].Text = "National Publications";
            FpSpread1.Sheets[0].Columns[45].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 47].Text = "Patents";
            FpSpread1.Sheets[0].Columns[46].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 48].Text = "No. of PG Projects Guided";
            FpSpread1.Sheets[0].Columns[47].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 49].Text = "No. of Doctorate Students Guided";
            FpSpread1.Sheets[0].Columns[48].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 50].Text = "International Publications";
            FpSpread1.Sheets[0].Columns[49].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 51].Text = "No of books Published";
            FpSpread1.Sheets[0].Columns[50].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 52].Text = "Is Physically Handicapped";
            FpSpread1.Sheets[0].Columns[51].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 53].Text = "Minority Indicator";
            FpSpread1.Sheets[0].Columns[52].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 54].Text = "First Yr teacher";
            FpSpread1.Sheets[0].Columns[53].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 55].Text = "FY/Common Subject Teacher?";
            FpSpread1.Sheets[0].Columns[54].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 56].Text = "FY/Common Subject";
            FpSpread1.Sheets[0].Columns[55].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 57].Text = "Would you like to work as Expert Member on various committees of AICTE";
            FpSpread1.Sheets[0].Columns[56].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 58].Text = "Have you ever applied to AICTE for any grants/assistance";
            FpSpread1.Sheets[0].Columns[57].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 59].Text = "Basic Pay in Rs.";
            FpSpread1.Sheets[0].Columns[58].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 60].Text = "DA %";
            FpSpread1.Sheets[0].Columns[59].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 61].Text = "HRA in Rs.";
            FpSpread1.Sheets[0].Columns[60].Width = 200;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 62].Text = "Other Allowances in Rs.";
            FpSpread1.Sheets[0].Columns[61].Width = 200;
            //Start======Added by Manikandan 21/08/2013===========
            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[59].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[60].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[61].HorizontalAlign = HorizontalAlign.Right;
            FpSpread1.Sheets[0].Columns[62].HorizontalAlign = HorizontalAlign.Right;
            //=======================End==========================
            dssrall = oda.select_method(sqlcmdallm, hat, "Text");

            for (int c = 0; c < FpSpread1.Sheets[0].ColumnCount; c++)
            {
                FpSpread1.Sheets[0].Columns[c].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Columns[c].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Columns[c].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Columns[c].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[c].VerticalAlign = VerticalAlign.Middle;
            }

            FarPoint.Web.Spread.TextCellType tb = new FarPoint.Web.Spread.TextCellType();
            FpSpread1.Sheets[0].Columns[42].CellType = tb;

            DataView dvnew = new DataView();

            if (dssrall != null && dssrall.Tables[0] != null && dssrall.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
                btnexportexcel.Visible = true;

                for (int row_cnt = 0; row_cnt < dssrall.Tables[0].Rows.Count; row_cnt++)
                {
                    int expcountmasteryear = 0;
                    int expcountmastermonth = 0;
                    int exptrialyear = 0;
                    int exptrialmonth = 0;
                    int nonmasteryear = 0;
                    int nonmastermonth = 0;
                    int nontrialyear = 0;
                    int nontrialmonth = 0;
                    int totalyear = 0;
                    int totalmonth = 0;

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].RowCount);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Title"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["NameAcr"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Appl_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Mid_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Sex"]);

                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["family_info"]) != "" && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["family_info"]) != null)
                    {
                        string textfamily = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["family_info"]);

                        string[] familyprocessone = textfamily.Split(new Char[] { '\\' });

                        for (int v = 0; v <= familyprocessone.GetUpperBound(0); v++)
                        {
                            if (familyprocessone[v] != null && familyprocessone[v] != "")
                            {
                                string[] familycheck = familyprocessone[v].Split(';');
                                if (familycheck.GetUpperBound(0) >= 4)
                                {
                                    if (familycheck[4].ToUpper() == "FATHER")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(familycheck[1]);
                                    }
                                    if (familycheck[4].ToUpper() == "MOTHER")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(familycheck[1]);
                                    }
                                }
                            }
                        }
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Per_Address"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Per_Address1"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Per_Pincode"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PCity"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PState"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 13].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Religion"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 14].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Caste"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 15].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Date_of_Birth"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 16].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PANGIRNumber"]);


                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["per_phone"]) != "" && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["per_phone"]) != null)
                    {

                        string[] phonesplit = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["per_phone"]).Split(new Char[] { '-' });
                        if (phonesplit.GetUpperBound(0) >= 0)
                        {
                            if (phonesplit.GetUpperBound(0) >= 1)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 17].Text = Convert.ToString(phonesplit[0]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18].Text = Convert.ToString(phonesplit[1]);
                            }
                            else if (phonesplit.GetUpperBound(0) >= 0)
                            {

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 18].Text = Convert.ToString(phonesplit[0]);
                            }
                        }
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 19].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Per_MobileNo"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 20].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Email"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 21].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Per_Fax"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 22].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Desig_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 23].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["StfNature"]);
                    dssrall.Tables[1].DefaultView.RowFilter = " staff_code='" + Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["staff_code"]) + "'";
                    dvnew = dssrall.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 24].Text = Convert.ToString(dvnew[0]["netadd"]);
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 24].Text = "";
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 25].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["StfStatus"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 26].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["FacultyType"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 27].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PayType"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 28].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Programme"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 29].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Course"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 30].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PayMode"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 31].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["PFNumber"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 32].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Join_Date"]);

                    //===============added by Manikandan on 10/10/2013==================
                    join_date = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Join_Date"]);
                    string[] split_joindate = join_date.Split(new char[] { '/' });
                    DateTime date_of_join = Convert.ToDateTime(split_joindate[1] + "/" + split_joindate[0] + "/" + split_joindate[2]);
                    DateTime curr_date = System.DateTime.Now;

                    double now_date = Convert.ToDouble(DateTime.Now.ToString("yyyy.MMdd"));
                    double staff_datejoin = Convert.ToDouble(date_of_join.ToString("yyyy.MMdd"));

                    int tot_exp_withjoindate = Convert.ToInt32(now_date - staff_datejoin);

                    int tot_exp_in_months = Convert.ToInt32((curr_date.Year - date_of_join.Year) * 12 + curr_date.Month - date_of_join.Month);
                    int tot_month_prev = 0;
                    //==Sample=======Year Calculation=====
                    //var now = float.Parse(DateTime.Now.ToString("yyyy.MMdd"));
                    //var dob = float.Parse(dateOfBirth.ToString("yyyy.MMdd"));
                    //var age = (int)(now - dob);

                    //====Sample====Month calculation========
                    // 1). int months = (Date2.Year - Date1.Year) * 12 + Date2.Month - Date1.Month;
                    // 2). TimeSpan ts = dt2.Subtract(dt1);
                    //double days = (double)ts.TotalHours / (24);
                    //double months = days / 30.4;
                    //========================

                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Qualification"]) != "" && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Qualification"]) != null)
                    {
                        string text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Qualification"]);

                        string[] qualificationprocessone = text.Split(new Char[] { '\\' });

                        for (int v = 0; v <= qualificationprocessone.GetUpperBound(0); v++)
                        {
                            if (qualificationprocessone[v] != null && qualificationprocessone[v] != "")
                            {
                                string[] qualificationcheck = qualificationprocessone[v].Split(';');
                                if (qualificationcheck.GetUpperBound(0) >= 4)
                                {
                                    if (qualificationcheck[1] == "Ph.D")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 33].Text = "Y";
                                    }
                                    else
                                    {
                                        if (FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 33].Text != "Y")
                                        {
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 33].Text = "N";
                                        }
                                    }

                                    if (qualificationcheck[1] == "Post Graduate" || qualificationcheck[1] == "PG")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 34].Text = Convert.ToString(qualificationcheck[2]);
                                    }

                                    if (qualificationcheck[1] == "Under Graduate" || qualificationcheck[1] == "UG")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 35].Text = Convert.ToString(qualificationcheck[2]);
                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 34].Text = Convert.ToString(qualificationprocessone[3]);
                                    }

                                    if (qualificationcheck[1] == "Diploma")
                                    {
                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 36].Text = Convert.ToString(qualificationcheck[3]);
                                    }

                                    if (qualificationcheck[1] == "Under Graduate" || qualificationcheck[1] == "UG")
                                    {

                                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 37].Text = Convert.ToString(qualificationcheck[3]);
                                    }

                                }
                            }
                        }
                    }


                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Experience_info"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Experience_info"]) != "")
                    {
                        string expinfostring = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Experience_info"]);

                        string[] expinfoprocessone = expinfostring.Split(new Char[] { '\\' });

                        for (int v = 0; v <= expinfoprocessone.GetUpperBound(0); v++)
                        {
                            if (expinfoprocessone[v] != null && expinfoprocessone[v] != "")
                            {
                                string[] expinfocheck = expinfoprocessone[v].Split(';');
                                if (expinfocheck.GetUpperBound(0) >= 8)
                                {
                                    if (expinfocheck[5] == "Teaching")
                                    {
                                        if (expinfocheck[6] != null && expinfocheck[6] != "" && expinfocheck[7] != null && expinfocheck[7] != "")
                                        {
                                            exptrialyear = Convert.ToInt32(expinfocheck[6]);
                                            exptrialmonth = Convert.ToInt32(expinfocheck[7]);

                                            expcountmasteryear += exptrialyear;
                                            expcountmastermonth = expcountmastermonth + exptrialmonth;


                                            if (expcountmastermonth > 12)
                                            {
                                                expcountmasteryear += 1;
                                                expcountmastermonth = expcountmastermonth - 12;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (expinfocheck[6] != null && expinfocheck[6] != "" && expinfocheck[7] != null && expinfocheck[7] != "")
                                        {
                                            try
                                            {
                                                nontrialyear = Convert.ToInt32(expinfocheck[6]);
                                                nontrialmonth = Convert.ToInt32(expinfocheck[7]);

                                                nonmasteryear += nontrialyear;
                                                nonmastermonth = nonmastermonth + nontrialmonth;


                                                if (nonmastermonth > 12)
                                                {
                                                    nonmasteryear += 1;
                                                    nonmastermonth = nonmastermonth - 12;
                                                }
                                            }
                                            catch
                                            {
                                                lblother.Visible = true;
                                                lblother.Text = "Please Change The Experiance Details Of This Staff :" + Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Appl_Name"] + "   and Serial.No: " + Convert.ToString(FpSpread1.Sheets[0].RowCount));
                                            }
                                        }
                                    }

                                    totalyear = expcountmastermonth + nonmasteryear;
                                    totalmonth = expcountmastermonth + nonmastermonth;

                                    //totalyear = expcountmasteryear + nonmasteryear;
                                    //totalmonth = expcountmastermonth + nonmastermonth;

                                    if (totalmonth > 12)
                                    {
                                        totalyear = +1;
                                        totalmonth = totalmonth - 12;
                                    }

                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 38].Text = expcountmasteryear.ToString();
                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 39].Text = Convert.ToString(totalyear+tot_exp_withjoindate);
                                }
                            }
                        }

                    }

                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Research_Info"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Experience_info"]) != "")
                    {
                        string researchinfostring = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Research_Info"]);

                        string[] researchinfoprocessone = researchinfostring.Split(new Char[] { '\\' });

                        if (researchinfoprocessone.GetUpperBound(0) >= 0)
                        {
                            researchcountmastr = 1 - researchinfoprocessone.GetLength(0);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 41].Text = researchcountmastr.ToString();
                        }

                    }
                    int extra_year = (totalmonth + tot_exp_in_months) / 12;
                    int extra_month = extra_year / 12;
                    //if (extra_month >= 1)
                    //{
                    //}
                    //=============place changed from 759============
                    tot_month_prev = totalyear * 12;

                    int temp_1 = tot_exp_in_months + tot_month_prev + nonmastermonth;
                    int final_total = temp_1 / 12;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 38].Text = expcountmasteryear.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 39].Text = Convert.ToString(final_total);//totalyear + tot_exp_withjoindate + extra_month);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 40].Text = Convert.ToString(tot_exp_in_months + tot_month_prev + nonmastermonth);
                    //======================End==================

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 42].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["BankAccount"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 43].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Bank_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 44].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Branch_Name"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 45].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["IFSC_Code"]);



                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Journal_Publication"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Journal_Publication"]) != "")
                    {

                        string Jpublication = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Journal_Publication"]);

                        string[] Jpubprocessone = Jpublication.Split(new Char[] { '\\' });

                        for (int v = 0; v <= Jpubprocessone.GetUpperBound(0); v++)
                        {
                            if (Jpubprocessone.GetUpperBound(0) >= 0)
                            {
                                if (Jpubprocessone[1] == "National")
                                {
                                    nationalmastercount = +1;
                                }
                                else if (Jpubprocessone[1] == "International")
                                {
                                    internationalmastercount = +1;
                                }

                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 46].Text = nationalmastercount.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 47].Text = internationalmastercount.ToString();
                            }
                        }
                    }



                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Patent_Received"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Patent_Received"]) != "")
                    {


                        string patentstr = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Patent_Received"]);

                        string[] patentprocessone = patentstr.Split(new Char[] { '\\' });

                        for (int v = 0; v <= patentprocessone.GetUpperBound(0); v++)
                        {
                            if (patentprocessone.GetUpperBound(0) >= 0)
                            {
                                if (patentprocessone[1] != "" && patentprocessone[1] != null)
                                {
                                    patentmastercount = +1;
                                }


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 47].Text = patentmastercount.ToString();

                            }
                        }
                    }

                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Project_Grants"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Project_Grants"]) != "")
                    {


                        string pgstr = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Project_Grants"]);

                        string[] pgprocessone = pgstr.Split(new Char[] { '\\' });

                        for (int v = 0; v <= pgprocessone.GetUpperBound(0); v++)
                        {
                            if (pgprocessone.GetUpperBound(0) >= 0)
                            {
                                if (pgprocessone[1] != "" && pgprocessone[1] != null)
                                {
                                    pgmyguide = +1;
                                }


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 48].Text = pgmyguide.ToString();

                            }
                        }
                    }


                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Guide_Ship"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Guide_Ship"]) != "")
                    {


                        string phdstr = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Guide_Ship"]);

                        string[] phdprocessone = phdstr.Split(new Char[] { '\\' });

                        for (int v = 0; v <= phdprocessone.GetUpperBound(0); v++)
                        {
                            if (phdprocessone.GetUpperBound(0) >= 0)
                            {
                                if (phdprocessone[1] != "" && phdprocessone[1] != null)
                                {
                                    phdmyguide = +1;
                                }


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 49].Text = phdmyguide.ToString();

                            }
                        }
                    }

                    if (Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Books_published"]) != null && Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Books_published"]) != "")
                    {


                        string bookpubstr = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["Books_published"]);

                        string[] bookpubprocessone = bookpubstr.Split(new Char[] { '\\' });

                        for (int v = 0; v <= bookpubprocessone.GetUpperBound(0); v++)
                        {
                            if (bookpubprocessone.GetUpperBound(0) >= 0)
                            {
                                if (bookpubprocessone[1] != "" && bookpubprocessone[1] != null)
                                {
                                    bookpubcount = +1;
                                }


                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 51].Text = bookpubcount.ToString();

                            }
                        }
                    }

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 52].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["IsPhy"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 53].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["IsMin"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 54].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["IsFirstYr"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 55].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["IsFYCommon"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 56].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["FYCommonSub"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 57].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["AICTE_Comm"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 58].Text = Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["AICTE_Grants"]);
                    dssrall.Tables[1].DefaultView.RowFilter = " staff_code='" + Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["staff_code"]) + "'";
                    dvnew = dssrall.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 59].Text = Convert.ToString(dvnew[0]["BSalary"]);
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 59].Text = "";
                    }

                    dssrall.Tables[1].DefaultView.RowFilter = " staff_code='" + Convert.ToString(dssrall.Tables[0].Rows[row_cnt]["staff_code"]) + "'";
                    dvnew = dssrall.Tables[1].DefaultView;
                    if (dvnew.Count > 0)
                    {
                        if (Convert.ToString(dvnew[0]["Allowances"]) != "" && Convert.ToString(dvnew[0]["Allowances"]) != null)
                        {
                            string textallow = Convert.ToString(dvnew[0]["Allowances"]);

                            string[] allowprocessone = textallow.Split(new Char[] { '\\' });

                            for (int v = 0; v <= allowprocessone.GetUpperBound(0); v++)
                            {
                                if (allowprocessone[v] != null && allowprocessone[v] != "")
                                {
                                    string[] aloowcheck = allowprocessone[v].Split(';');
                                    if (aloowcheck.GetUpperBound(0) >= 2)
                                    {
                                        string[] spge = aloowcheck[2].ToString().Split('-');
                                        string getval = spge[0].ToString();
                                        if (getval.Trim() == "")
                                        {
                                            getval = "0";
                                        }
                                        Double num = 0;
                                        if (!Double.TryParse(getval, out num))
                                        {
                                            getval = "0";
                                        }
                                        Double chba = Convert.ToDouble(getval);
                                        chba = Math.Round(chba, 0, MidpointRounding.AwayFromZero);
                                        getval = chba.ToString();
                                        if (aloowcheck[0].Trim().ToUpper() == "DA")
                                        {
                                            dacount = Convert.ToInt32(getval);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 60].Text = dacount.ToString();
                                        }
                                        else if (aloowcheck[0].Trim().ToUpper() == "HRA")
                                        {
                                            hracount = Convert.ToInt32(getval);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 61].Text = hracount.ToString();
                                        }
                                        else if (aloowcheck[0].Trim().ToUpper() == "INCNT")
                                        {
                                            hracount = Convert.ToInt32(getval);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 62].Text = getval.ToString();

                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                con.Close();
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.Visible = true;
                btnprintmaster.Visible = true;
            }
            else
            {
                lblother.Visible = true;
                lblother.Text = "No Records Found";
                FpSpread1.Visible = false;
                btnexportexcel.Visible = false;
                btnprintmaster.Visible = false;
            }
        }
        catch (Exception e)
        {
            lblother.Visible = true;
            lblother.Text = e.ToString();
            d2.sendErrorMail(e, Convert.ToString(Session["collegecode"]), "StaffReport.aspx");
        }
    }





    //category dropdown Extender

    protected void LinkButtoncategory_Click(object sender, EventArgs e)
    {

        cblcategory.ClearSelection();
        bloodcnt = 0;
        txtcategory.Text = "---Select---";
    }

    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcategory.Checked == true)
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = true;
                txtcategory.Text = "Category(" + (cblcategory.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblcategory.Items.Count; i++)
            {
                cblcategory.Items[i].Selected = false;
                txtcategory.Text = "---Select---";
            }
        }
    }

    protected void cblcategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cblcategory.Items.Count; i++)
        {
            if (cblcategory.Items[i].Selected == true)
            {
                value = cblcategory.Items[i].Text;

                code = cblcategory.Items[i].Value.ToString();
                bloodcount = bloodcount + 1;
                txtcategory.Text = "Category(" + bloodcount.ToString() + ")";
            }
        }
        if (bloodcount == 0)
        {
            txtcategory.Text = "---Select---";
        }
        else
        {
            Label lbl = bloodlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl2-" + code.ToString();
            ImageButton ib = bloodimage();
            ib.ID = "imgbut2_" + code.ToString();
            ib.Click += new ImageClickEventHandler(bloodimg_Click);
        }
        bloodcnt = bloodcount;
    }

    public Label bloodlabel()
    {
        Label lbc = new Label();

        ViewState["lbloodcontrol"] = true;
        return (lbc);
    }

    public ImageButton bloodimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;


        ViewState["ibloodcontrol"] = true;
        return (imc);
    }

    public void bloodimg_Click(object sender, ImageClickEventArgs e)
    {
        bloodcnt = bloodcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblcategory.Items[r].Selected = false;
        txtcategory.Text = "Blood Group(" + bloodcnt.ToString() + ")";
        if (txtcategory.Text == "Blood Group(0)")
        {

            txtcategory.Text = "---Select---";
        }

    }


    //department dropdown extender



    protected void LinkButtonseattype_Click(object sender, EventArgs e)
    {
        cbldepttype.ClearSelection();
        seatcnt = 0;
        tbseattype.Text = "---Select---";

    }

    protected void chkselect_CheckedChanged(object sender, EventArgs e)
    {
        if (chkselect.Checked == true)
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = true;
                tbseattype.Text = "Department(" + (cbldepttype.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbldepttype.Items.Count; i++)
            {
                cbldepttype.Items[i].Selected = false;
                tbseattype.Text = "---Select---";
            }
        }
    }

    protected void cbldepttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        pseattype.Focus();

        int seatcount = 0;
        string value = "";
        string code = "";


        for (int i = 0; i < cbldepttype.Items.Count; i++)
        {
            if (cbldepttype.Items[i].Selected == true)
            {

                value = cbldepttype.Items[i].Text;
                code = cbldepttype.Items[i].Value.ToString();
                seatcount = seatcount + 1;
                tbseattype.Text = "Department(" + seatcount.ToString() + ")";
            }

        }

        if (seatcount == 0)
            tbseattype.Text = "---Select---";
        else
        {
            Label lbl = seatlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = seatimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(seatimg_Click);
        }
        seatcnt = seatcount;

    }

    public Label seatlabel()
    {
        Label lbc = new Label();

        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton seatimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    public void seatimg_Click(object sender, ImageClickEventArgs e)
    {
        seatcnt = seatcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldepttype.Items[r].Selected = false;

        tbseattype.Text = "Department(" + seatcnt.ToString() + ")";
        if (tbseattype.Text == "Department(0)")
        {
            tbseattype.Text = "---Select---";

        }


    }


    // staff type dropdown extender

    protected void LinkButtonstafftype_Click(object sender, EventArgs e)
    {

        cblstafftype.ClearSelection();
        catcnt = 0;
        txtstafftype.Text = "---Select---";

    }

    protected void chkstafftype_CheckedChanged(object sender, EventArgs e)
    {
        if (chkstafftype.Checked == true)
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = true;
                txtstafftype.Text = "Staff Type(" + (cblstafftype.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblstafftype.Items.Count; i++)
            {
                cblstafftype.Items[i].Selected = false;
                txtstafftype.Text = "---Select---";
            }
        }
    }

    protected void cblstafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";

        for (int i = 0; i < cblstafftype.Items.Count; i++)
        {
            if (cblstafftype.Items[i].Selected == true)
            {
                value = cblstafftype.Items[i].Text;

                code = cblstafftype.Items[i].Value.ToString();
                bloodcount = bloodcount + 1;
                txtstafftype.Text = "Staff Type(" + bloodcount.ToString() + ")";
            }
        }
        if (bloodcount == 0)
        {
            txtstafftype.Text = "---Select---";
        }
        else
        {
            Label lbl = bloodlabelsf();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl2-" + code.ToString();
            ImageButton ib = bloodimagesf();
            ib.ID = "imgbut2_" + code.ToString();
            ib.Click += new ImageClickEventHandler(bloodimgsf_Click);
        }
        catcnt = bloodcount;
    }

    public Label bloodlabelsf()
    {
        Label lbc = new Label();
        ViewState["istafftypecontrol"] = true;
        return (lbc);
    }

    public ImageButton bloodimagesf()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["istafftypecontrol"] = true;
        return (imc);
    }

    public void bloodimgsf_Click(object sender, ImageClickEventArgs e)
    {
        catcnt = catcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cblstafftype.Items[r].Selected = false;
        txtstafftype.Text = "Staff type(" + catcnt.ToString() + ")";
        if (txtstafftype.Text == "Staff type(0)")
        {

            txtstafftype.Text = "---Select---";
        }
    }


    //designation dropdown extender


    protected void LinkButtondesi_Click(object sender, EventArgs e)
    {

        cbldesi.ClearSelection();
        typecnt = 0;
        txtdesi.Text = "---Select---";

    }

    protected void chkdesi_CheckedChanged(object sender, EventArgs e)
    {
        if (chkdesi.Checked == true)
        {
            for (int i = 0; i < cbldesi.Items.Count; i++)
            {
                cbldesi.Items[i].Selected = true;
                txtdesi.Text = "Designation(" + (cbldesi.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cbldesi.Items.Count; i++)
            {
                cbldesi.Items[i].Selected = false;
                txtdesi.Text = "---Select---";
            }
        }
    }

    protected void cbldesi_SelectedIndexChanged(object sender, EventArgs e)
    {
        int bloodcount = 0;
        string value = "";
        string code = "";
        for (int i = 0; i < cbldesi.Items.Count; i++)
        {
            if (cbldesi.Items[i].Selected == true)
            {
                value = cbldesi.Items[i].Text;

                code = cbldesi.Items[i].Value.ToString();
                bloodcount = bloodcount + 1;
                txtdesi.Text = "Designation(" + bloodcount.ToString() + ")";
            }
        }
        if (bloodcount == 0)
        {
            txtdesi.Text = "---Select---";
        }
        else
        {
            Label lbl = bloodlabelsf();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl2-" + code.ToString();
            ImageButton ib = bloodimagesf();
            ib.ID = "imgbut2_" + code.ToString();
            ib.Click += new ImageClickEventHandler(bloodimgsf_Click);
        }
        typecnt = bloodcount;
    }

    public Label bloodlabeldesi()
    {
        Label lbc = new Label();
        ViewState["idesignationcontrol"] = true;
        return (lbc);
    }

    public ImageButton bloodimagedesi()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["idesignationcontrol"] = true;
        return (imc);
    }

    public void bloodimgdesi_Click(object sender, ImageClickEventArgs e)
    {
        typecnt = typecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        cbldesi.Items[r].Selected = false;
        txtdesi.Text = "Desi Group(" + typecnt.ToString() + ")";
        if (txtdesi.Text == "Desi Group(0)")
        {
            txtdesi.Text = "---Select---";
        }

    }


    // grid to excel convert


    protected void btnexportexcel_Click(object sender, EventArgs e)
    {
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = "";
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "Staff Report" + i;
                //FpSpread1.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); 
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================
            }
            catch
            {
                i++;
                goto e;

            }
        }
        // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"]
        string page_name = string.Empty;
        string degreedetails = string.Empty;
        string sel_category = string.Empty;

        Session["column_header_row_count"] = FpSpread1.Sheets[0].ColumnHeader.RowCount;

        degreedetails = "Staff Report";
        string pagename = "staffleavereport.aspx";


        Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}