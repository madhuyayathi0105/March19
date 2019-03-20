using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Collections;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Globalization;

public partial class smartcardmenu_report : System.Web.UI.Page
{
    bool cellclick = false;
    bool cellclick1 = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    Hashtable hat = new Hashtable();

    string q1 = "";
    int i = 0; int k = 0;
    double studcount = 0; double staffcount = 0; double otherscount = 0; double grandtotal = 0; double totalmembercount = 0;
    double detailqty = 0; double detailcost = 0; double cumlativeqty = 0; double culativecost = 0;
    bool tablecountcheck = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("hostelindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HostelMod/hostelindex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            lblvalidation1.Text = "";
            if (!IsPostBack)
            {
                rdbtype1.Items[0].Selected = true;
                rdbtype.Items[0].Selected = true;
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_fromdate.Attributes.Add("Readonly", "Readonly");
                txt_todate.Attributes.Add("Readonly", "Readonly");
                CalendarExtender2.EndDate = DateTime.Now;
                CalendarExtender1.EndDate = DateTime.Now;
                bindmonth();
                bindyear();
                bindcollege();
                bindmess();
                bindsession();
            }
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
        }
    }
    protected void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        mulview.ActiveViewIndex = Convert.ToInt32(rdbtype.SelectedValue);
        clear();
    }
    protected void rdbtype1_SelectedIndexChanged(object sender, EventArgs e)
    {
        muldetail.ActiveViewIndex = Convert.ToInt32(rdbtype1.SelectedValue);
        FpSpread1.Visible = false;
        clear();
    }
    protected void cbl_canteen_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_canteen, cbl_canteen, txt_canteenname, "Canteen Name");
        bindsession();
    }
    protected void cbl_canteen_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_canteen, cbl_canteen, txt_canteenname, "Canteen Name");
        bindsession();
    }
    protected void cb_sessionname_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sessionname, cbl_sessionname, txt_sessionname, "Session Name");
    }
    protected void cbl_sessionname_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sessionname, cbl_sessionname, txt_sessionname, "Session Name");
    }

    protected void cb_type_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_type, cbl_type, txt_type, "Type");
        clear();
    }
    protected void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_type, cbl_type, txt_type, "Type");
        clear();
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst)
    {
        try
        {
            int sel = 0;
            int count = 0;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = dipst + "(" + count + ")";
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
            else
            {
                txt.Text = "--Select--";
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst)
    {
        try
        {
            int sel = 0;
            txt.Text = "--Select--";
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
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
                txt.Text = "--Select--";
            }
        }
        catch { }
    }

    public void bindcollege()
    {
        ds.Clear();
        ds = d2.select_method_wo_parameter("select college_code,collname from collinfo ", "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_collgname.DataSource = ds;
            ddl_collgname.DataTextField = "collname";
            ddl_collgname.DataValueField = "college_code";
            ddl_collgname.DataBind();
        }
    }
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
    }
    protected void bindsession()
    {
        try
        {
            string messfk = returnwithsinglecodevalue(cbl_canteen);
            ds = d2.select_method_wo_parameter("select SessionName,SessionMasterPK from HM_SessionMaster where MessMasterFK in('" + messfk + "')", "Text");
            cbl_sessionname.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_sessionname.DataSource = ds;
                cbl_sessionname.DataTextField = "SessionName";
                cbl_sessionname.DataValueField = "SessionMasterPK";
                cbl_sessionname.DataBind();
                if (cbl_sessionname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sessionname.Items.Count; i++)
                    {
                        cbl_sessionname.Items[i].Selected = true;
                    }
                    txt_sessionname.Text = "Session Name(" + cbl_sessionname.Items.Count + ")";
                }
                else
                {
                    txt_sessionname.Text = "--Select--";
                }
            }
            else
            {
                txt_sessionname.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void bindmess()
    {
        try
        {
            ds.Clear(); q1 = "";
            if (ddl_collgname.Items.Count > 0)
            {
                q1 = Convert.ToString(ddl_collgname.SelectedItem.Value);
            }
            ds = d2.Bindmess_basedonrights(usercode, q1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_canteen.DataSource = ds;
                cbl_canteen.DataTextField = "MessName";
                cbl_canteen.DataValueField = "MessMasterPK";
                cbl_canteen.DataBind();
                if (cbl_canteen.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_canteen.Items.Count; i++)
                    {
                        cbl_canteen.Items[i].Selected = true;
                    }
                    txt_canteenname.Text = "Canteen Name(" + cbl_canteen.Items.Count + ")";
                }
                else
                {
                    txt_canteenname.Text = "--Select--";
                }
            }
            else
            {
                txt_canteenname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_collgname_selectedindexchange(object sender, EventArgs e)
    {
        bindmess();
        bindsession();
    }
    protected void txt_studregno_TextChange(object sender, EventArgs e)
    {
        OnTextChange(txt_studregno);
    }
    protected void txt_studrollsearch_TextChange(object sender, EventArgs e)
    {
        OnTextChange(txt_studrollsearch);
    }
    protected void txt_studnamesearch_TextChange(object sender, EventArgs e)
    {
        OnTextChange(txt_studnamesearch);
    }
    protected void OnTextChange(TextBox txt)
    {
        try
        {
            string condition = "";
            if (txt.ID == "txt_studrollsearch" || txt.ID == "txt_studregno" || txt.ID == "txt_studnamesearch")
            {
                if (txt.ID == "txt_studnamesearch")
                {
                    condition = " and Stud_Name='" + txt_studnamesearch.Text + "'";
                }
                else if (txt.ID == "txt_studrollsearch")
                {
                    condition = " and Roll_No='" + txt_studrollsearch.Text + "'";
                }
                else if (txt.ID == "txt_studregno")
                {
                    condition = " and Reg_No='" + txt_studregno.Text + "'";
                }
                if (condition.Trim() != "")
                {
                    q1 = "";
                    q1 = " select roll_no,Stud_Name,Stud_Type,r.degree_code,Branch_code,Batch_Year,Sections,((CONVERT(varchar(max), r.Batch_Year)+' - '+C.Course_Name+' - '+dt.dept_acronym+ case when sections='' then '' else ' - '+ (sections) end)) as batch from Registration r,Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id " + condition + "";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    clear();
                }
            }
            else if (txt.ID == "txt_staffcodesearch" || txt.ID == "txt_staffnamesearch")
            {

            }
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getothername(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct OthersName from HT_Menu_purchase where OthersName<>'' and  OthersName like  '" + prefixText + "%' order by OthersName asc";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode1(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster s,staff_appl_master a where s.resign =0 and s.settled =0  and s.appl_no = a.appl_no  and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            if (prefixText.Trim() != "")
            {
                string query = "";
                WebService ws = new WebService();

                query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' ";//and college_code=" + collegecodestat + " 

                name = ws.Getname(query);
            }
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstudentreg(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            if (prefixText.Trim() != "")
            {
                string query = "";
                WebService ws = new WebService();

                query = "select reg_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and reg_no like '" + prefixText + "%' ";//and college_code=" + collegecodestat + " 

                name = ws.Getname(query);
            }
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getstudentname(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            if (prefixText.Trim() != "")
            {
                string query = "";
                WebService ws = new WebService();

                query = "select Stud_Name  from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Name like '" + prefixText + "%'  ";

                name = ws.Getname(query);
            }
            return name;
        }
        catch { return name; }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getmenu(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct MenuName  from HM_MenuMaster where  MenuName like '" + prefixText + "%' order by MenuName";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["MenuName"].ToString());
            }
        }
        return name;
    }
    protected void ddlstudenttype_onselectedindexchange(object sender, EventArgs e)
    {
        mulddlstud.ActiveViewIndex = Convert.ToInt32(ddlstudenttype.SelectedItem.Value);
        clear();
    }

    protected void clear()
    {
        txt_studrollsearch.Text = "";
        txt_studnamesearch.Text = "";
        txt_studregno.Text = "";
        txt_staffcodesearch1.Text = "";
        txt_staffnamesearch1.Text = "";
        txt_othername.Text = "";

        ddl_frommonth.SelectedIndex = 0;
        ddl_tomonth.SelectedIndex = 0;

        FpSpread1.Visible = false;
        FpSpread2.Visible = false;
        FpSpread4.Visible = false;
        rptprint2.Visible = false;
        rptprint1.Visible = false;
        rptprint.Visible = false;
    }

    protected void btn_Go_Click(object sender, EventArgs e)
    {
        try
        {
            string sessionFk = returnwithsinglecodevalue(cbl_sessionname);
            string messfk = returnwithsinglecodevalue(cbl_canteen);
            DateTime dt = new DateTime();
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            DateTime dtm = new DateTime();
            DateTime dtm1 = new DateTime();
            string firstdate = Convert.ToString(txt_fromdate.Text);
            string seconddate = Convert.ToString(txt_todate.Text);
            string[] split = firstdate.Split('/');
            dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
            string[] split1 = seconddate.Split('/');
            dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);
            FpSpread1.Visible = false;
            FpSpread2.Visible = false;
            FpSpread4.Visible = false;
            dt2 = dt; bool checktype = false;
            if (rdbtype1.SelectedIndex == 0)
            {
                #region cumlative
                q1 = ""; string wise = ""; tablecountcheck = false;
                if (rdbtype.SelectedIndex == 0)
                {
                    #region Datewise
                    Fpreadheaderbindmethod("S No/Order Date/Student Count/Staff Count/Others Count/Total Count/Order Cost", FpSpread1, "True");
                    while (dt2 <= dt1)
                    {
                        wise = " and OrderDate between '" + dt2.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'";
                        q1 = "";
                        q1 += "  select COUNT(MemType),convert(varchar(10), OrderDate,103)OrderDate,MemType from HT_Menu_purchase where MemType='1' and CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') " + wise + " group by MemType,OrderDate order by OrderDate";

                        q1 += "   select COUNT(MemType),convert(varchar(10), OrderDate,103)OrderDate,MemType from HT_Menu_purchase where CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') and MemType=2 " + wise + " group by MemType,OrderDate order by OrderDate";

                        q1 += "  select COUNT(MemType),convert(varchar(10), OrderDate,103)OrderDate,MemType from HT_Menu_purchase where CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') and MemType=3 " + wise + " group by MemType,OrderDate order by OrderDate";

                        q1 += "  select Isnull(SUM(OrderCost),0)Ordercost,convert(varchar(10), OrderDate,103)OrderDate,MemType  from HT_Menu_purchase p,HT_Menu_purchase_det pd where p.MenuPurchasePK=pd.MenuPurchaseFK and CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "')  group by MemType,OrderDate order by OrderDate";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q1, "Text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            k++;
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderDate"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";


                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dt2.ToString("MM/dd/yyyy"));

                            double total = 0; double totalcast = 0;
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0][0]);
                                total += Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                                studcount += Convert.ToDouble(ds.Tables[0].Rows[0][0]);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = " - ";
                            }
                            if (ds.Tables[1].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[0][0]);
                                total += Convert.ToDouble(ds.Tables[1].Rows[0][0]);
                                staffcount += Convert.ToDouble(ds.Tables[1].Rows[0][0]);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = " - ";
                            }
                            if (ds.Tables[2].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[2].Rows[0][0]);
                                total += Convert.ToDouble(ds.Tables[2].Rows[0][0]);
                                otherscount += Convert.ToDouble(ds.Tables[2].Rows[0][0]);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = " - ";
                            }
                            if (ds.Tables[3].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[3].Rows[0][0]);
                                DataView dv = new DataView();
                                ds.Tables[3].DefaultView.RowFilter = " orderdate='" + dt2.ToString("dd/MM/yyyy") + "' ";
                                dv = ds.Tables[3].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        totalcast += Convert.ToDouble(dv[m][0]);
                                    }
                                }
                                else
                                {
                                    totalcast = 0;
                                }
                                grandtotal += Convert.ToDouble(totalcast);
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = " - ";
                            }
                            totalmembercount += total;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(totalcast);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Visible = true;
                            rptprint.Visible = true; tablecountcheck = true;
                        }
                        dt2 = dt2.AddDays(1);
                    }

                    Grandtotalbind();
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Height = 210;
                    FpSpread1.Width = 670;
                    #endregion
                }
                if (rdbtype.SelectedIndex == 1)
                {
                    #region monthwise

                    Fpreadheaderbindmethod("S No/Month/Student Count/Staff Count/Others Count/Total Count/Order Cost", FpSpread1, "True");
                    wise = " ";
                    if (Convert.ToInt32(ddl_frommonth.SelectedItem.Value) <= Convert.ToInt32(ddl_tomonth.SelectedItem.Value))
                    {
                        for (i = Convert.ToInt32(ddl_frommonth.SelectedItem.Value); i <= Convert.ToInt32(ddl_tomonth.SelectedItem.Value); i++)
                        {
                            int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), i);
                            dtm = Convert.ToDateTime(Convert.ToString((i)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            dtm1 = Convert.ToDateTime(Convert.ToString((i)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            wise = " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                            q1 = "";
                            q1 += "  select COUNT(appno) from HT_Menu_purchase where CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') and MemType=1 " + wise + " ";
                            q1 += "  select COUNT(appno) from HT_Menu_purchase where CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') and MemType=2 " + wise + "";
                            q1 += "  select COUNT(appno) from HT_Menu_purchase where CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') and MemType=3 " + wise + " ";
                            q1 += " select  Isnull(SUM(OrderCost),0)Ordercost from HT_Menu_purchase p,HT_Menu_purchase_det pd where p.MenuPurchasePK=pd.MenuPurchaseFK and CanteenFK in('" + messfk + "') and SessionFK in('" + sessionFk + "') " + wise + " ";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(q1, "Text");

                            bindmonthwisespread();
                            FpSpread1.Height = 310;
                            FpSpread1.Width = 770;
                            tablecountcheck = true;
                        }
                        Grandtotalbind();
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    }
                    else
                    {
                        clear();
                        alertwindow.Visible = true;
                        lbl_alert.Visible = true;
                        lbl_alert.Text = "From Month Greater then To Month";
                        return;
                    }

                    #endregion
                }
                if (tablecountcheck == false)
                {
                    clear();
                    alertwindow.Visible = true;
                    lbl_alert.Text = "No Records Founds";
                    lbl_alert.Visible = true;
                }
                #endregion
            }
            if (rdbtype1.SelectedIndex == 1)
            {
                #region Details
                if (Convert.ToString(ddl_collgname.SelectedItem.Value).Trim() != "" && sessionFk.Trim() != "")
                {
                    Fpreadheaderbindmethod("S No-50/Order Date-100/Canteen Name-200/Name-250/Session Name-200/Menu Name-200/Order Qty-150/Order Cost-150", FpSpread1, "False"); tablecountcheck = false;
                    string wise = "";
                    if (rdbtype.SelectedIndex == 0)
                    {
                        wise = " and OrderDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "'";
                    }
                    if (rdbtype.SelectedIndex == 1)
                    {
                        int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), Convert.ToInt32(ddl_tomonth.SelectedItem.Value));

                        dtm = Convert.ToDateTime(Convert.ToString((ddl_frommonth.SelectedItem.Text)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                        dtm1 = Convert.ToDateTime(Convert.ToString((ddl_tomonth.SelectedItem.Text)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                        wise = " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                    }
                    string Appno = "";
                    if (txt_studrollsearch.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = d2.getappno(txt_studrollsearch.Text.Trim());
                        wise = " and appno='" + Appno + "'";

                    }
                    if (txt_studregno.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = getappnoREG(txt_studregno.Text.Trim());
                        wise = " and appno='" + Appno + "'";
                    }
                    if (txt_studnamesearch.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = getappno(txt_studnamesearch.Text.Trim());
                        wise = " and appno='" + Appno + "'";
                    }
                    if (txt_staffcodesearch1.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = getstaffappid(txt_staffcodesearch1.Text.Trim());
                        wise = " and appno='" + Appno + "'";
                    }
                    if (txt_staffnamesearch1.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = getstaffappid_pname(txt_staffnamesearch1.Text.Trim());
                        wise = " and appno='" + Appno + "'";
                    }
                    if (txt_othername.Text.Trim() != "")
                    {
                        wise = "";
                        Appno = txt_othername.Text.Trim();
                        wise = " and OthersName='" + Appno + "'";
                    }
                    string canteenquery = "";
                    if (txt_studmenuname.Text.Trim() != "")
                    {
                        canteenquery = "  and MenuName='" + txt_studmenuname.Text + "'";
                    }
                    else
                    {
                        canteenquery = " ";
                    }
                    if (cbl_type.Items[0].Selected == true)
                    {
                        q1 = " select CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,Stud_Name,sm.SessionName,m.MenuName,OrderQty,OrderCost  from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,Registration r,HM_MessMaster mm where mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and r.App_No=p.Appno and m.CollegeCode in('" + Convert.ToString(ddl_collgname.SelectedItem.Value) + "') and p.SessionFK in('" + sessionFk + "')  " + canteenquery + " and MemType=1  and p.canteenfk in('" + messfk + "')   ";
                        q1 = q1 + "" + wise + " order by OrderDate,Stud_Name";
                        checktype = true;
                    }
                    string q2 = "";
                    if (cbl_type.Items[1].Selected == true)
                    {
                        q2 = " select CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,staff_name as Stud_Name,sm.SessionName,m.MenuName,OrderQty,OrderCost  from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,staffmaster s ,staff_appl_master sa,HM_MessMaster mm where s.appl_no =sa.appl_no and sa.appl_id=p.appno and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and m.CollegeCode in('" + Convert.ToString(ddl_collgname.SelectedItem.Value) + "') and p.SessionFK in('" + sessionFk + "') " + canteenquery + " and p.canteenfk in('" + messfk + "')  and MemType=2 ";
                        q2 = q2 + "" + wise + " order by OrderDate,staff_name";
                        checktype = true;
                    }
                    string q3 = "";
                    if (cbl_type.Items[2].Selected == true)
                    {
                        q3 = " select CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,SessionName,OthersName as Stud_Name,m.MenuName,OrderQty,OrderCost  from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,HM_MessMaster mm where mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and m.CollegeCode in('" + Convert.ToString(ddl_collgname.SelectedItem.Value) + "') and p.SessionFK in('" + sessionFk + "') " + canteenquery + " and p.canteenfk in('" + messfk + "')  and MemType=3 ";
                        q3 = q3 + "" + wise + " order by OrderDate,OthersName";
                        checktype = true;
                    }
                    if (q1.Trim() != "")
                    {
                        ds.Clear(); detailqty = 0; detailcost = 0;
                        ds = d2.select_method_wo_parameter(q1, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Student Details";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Detailswise(0);
                                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            }
                            detailgranttotal();
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            tablecountcheck = true;
                            FpSpread1.Height = 500;
                            FpSpread1.Width = 950;
                        }
                    }
                    if (q2.Trim() != "")
                    {
                        ds.Clear(); detailqty = 0; detailcost = 0;
                        ds = d2.select_method_wo_parameter(q2, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Staff Details";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Detailswise(0);
                                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            }
                            detailgranttotal(); tablecountcheck = true;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            FpSpread1.Height = 500;
                            FpSpread1.Width = 950;
                        }
                    }
                    if (q3.Trim() != "")
                    {
                        ds.Clear(); detailqty = 0; detailcost = 0;
                        ds = d2.select_method_wo_parameter(q3, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread1.Sheets[0].RowCount++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Others Details";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 8);
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Detailswise(0);
                                FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            }
                            detailgranttotal(); tablecountcheck = true;
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Visible = true;
                            rptprint.Visible = true;
                            FpSpread1.Height = 500;
                            FpSpread1.Width = 950;
                        }
                        if (tablecountcheck == false)
                        {
                            clear();
                            alertwindow.Visible = true;
                            lbl_alert.Text = "No Records Founds";
                            lbl_alert.Visible = true;
                        }
                        if (q1 == "" && q2 == "" && q3 == "")
                        {
                            clear();
                            alertwindow.Visible = true;
                            lbl_alert.Text = "No Records Founds";
                            lbl_alert.Visible = true;
                        }
                    }
                    if (checktype == false)
                    {
                        txt_type.Focus();
                        alertwindow.Visible = true;
                        lbl_alert.Text = "Please Select User Type";
                        lbl_alert.Visible = true;
                    }
                }
                #endregion
            }
        }
        catch (Exception ex)
        {
            alertwindow.Visible = true;
            lbl_alert.Text = ex.ToString();
            lbl_alert.Visible = true;
        }
    }
    protected void Detailswise(int table)
    {
        FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[table].Rows[i]["OrderDate"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[table].Rows[i]["MessName"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[table].Rows[i]["Stud_Name"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[table].Rows[i]["SessionName"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[table].Rows[i]["MenuName"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[table].Rows[i]["OrderQty"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[table].Rows[i]["OrderCost"]);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

        detailqty += Convert.ToDouble(ds.Tables[table].Rows[i]["OrderQty"]);
        detailcost += Convert.ToDouble(ds.Tables[table].Rows[i]["OrderCost"]);

    }
    protected void detailgranttotal()
    {
        FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = "Grand Total";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(detailqty);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(detailcost);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].ForeColor = Color.Purple;

    }
    protected void Grandtotalbind()
    {
        FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = "Grand Total";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(studcount);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(staffcount);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(otherscount);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(totalmembercount);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(grandtotal);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].ForeColor = Color.Purple;
    }
    protected void bindmonthwisespread()
    {
        k++;
        FpSpread1.Sheets[0].RowCount++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(k);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ddl_frommonth.Items[i - 1].Text);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ddl_frommonth.Items[i - 1].Value);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

        double total = 0; double totalcast = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[0][0]);
            total += Convert.ToDouble(ds.Tables[0].Rows[0][0]);
            studcount += Convert.ToDouble(ds.Tables[0].Rows[0][0]);
        }
        else
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = " - ";
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[0][0]);
            total += Convert.ToDouble(ds.Tables[1].Rows[0][0]);
            staffcount += Convert.ToDouble(ds.Tables[1].Rows[0][0]);
        }
        else
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = " - ";
        }
        if (ds.Tables[2].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[2].Rows[0][0]);
            total += Convert.ToDouble(ds.Tables[2].Rows[0][0]);
            otherscount += Convert.ToDouble(ds.Tables[2].Rows[0][0]);
        }
        else
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = " - ";
        }
        if (ds.Tables[3].Rows.Count > 0)
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[3].Rows[0][0]);
            totalcast += Convert.ToDouble(ds.Tables[3].Rows[0][0]);
            grandtotal += Convert.ToDouble(ds.Tables[3].Rows[0][0]);
        }
        else
        {
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = " - ";
        }

        totalmembercount += total;

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(total);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(totalcast);
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
        FpSpread1.Visible = true;
    }
    public void Fpreadheaderbindmethod(string headername, FarPoint.Web.Spread.FpSpread spreadname, string AutoPostBack)
    {
        try
        {
            string[] header = headername.Split('/');
            int k = 0;
            if (AutoPostBack.Trim().ToUpper() == "TRUE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = true;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (head.Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 50;
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = head;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = 200;
                        }
                    }
                }
            }
            else if (AutoPostBack.Trim().ToUpper() == "FALSE")
            {
                if (header.Length > 0)
                {
                    spreadname.Sheets[0].RowCount = 0;
                    spreadname.Sheets[0].ColumnCount = 0;
                    spreadname.CommandBar.Visible = false;
                    spreadname.Sheets[0].AutoPostBack = false;
                    spreadname.Sheets[0].ColumnHeader.RowCount = 1;
                    spreadname.Sheets[0].RowHeader.Visible = false;

                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    spreadname.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    foreach (string head in header)
                    {
                        k++;
                        string[] width = head.Split('-');
                        spreadname.Sheets[0].ColumnCount = Convert.ToInt32(header.Length);
                        if (Convert.ToString(width[0]).Trim().ToUpper() == "S.NO")
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                        else
                        {
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Text = Convert.ToString(width[0]);
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Bold = true;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].HorizontalAlign = HorizontalAlign.Center;
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Name = "Book Antiqua";
                            spreadname.Sheets[0].ColumnHeader.Cells[0, k - 1].Font.Size = FontUnit.Medium;
                            spreadname.Columns[k - 1].Width = Convert.ToInt32(width[1]);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            alertwindow.Visible = true;
            lbl_alert.Visible = true;
            lbl_alert.Font.Size = FontUnit.Smaller;
            lbl_alert.Text = ex.ToString();
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertwindow.Visible = false;
    }
    protected void bindyear()
    {
        int year = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        for (int l = 0; l < 15; l++)
        {
            ddl_year.Items.Add(Convert.ToString(year));
            year--;
        }
    }
    protected void bindmonth()
    {
        DateTime dt = new DateTime(2000, 1, 1);
        for (int m = 0; m < 12; m++)
        {
            ddl_frommonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
            ddl_tomonth.Items.Add(new ListItem(dt.AddMonths(m).ToString("MMMM"), (m + 1).ToString().TrimStart('0')));
        }
    }
    protected void FpSpread1_cellclick(object sender, EventArgs e)
    {
        cellclick = true;
    }
    protected void FpSpread1_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                if (rdbtype1.SelectedIndex == 0)
                {
                    q1 = "";
                    string activerow = FpSpread1.ActiveSheetView.ActiveRow.ToString();
                    string activecol = FpSpread1.ActiveSheetView.ActiveColumn.ToString();
                    string sessionFk = returnwithsinglecodevalue(cbl_sessionname);
                    string messfk = returnwithsinglecodevalue(cbl_canteen);
                    DateTime dt = new DateTime();
                    DateTime dt1 = new DateTime();
                    DateTime dtm = new DateTime();
                    DateTime dtm1 = new DateTime();
                    string firstdate = Convert.ToString(txt_fromdate.Text);
                    string seconddate = Convert.ToString(txt_todate.Text);
                    string[] split = firstdate.Split('/');
                    dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                    string[] split1 = seconddate.Split('/');
                    dt1 = Convert.ToDateTime(split1[1] + "/" + split1[0] + "/" + split1[2]);

                    string columnheadername = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(activecol)].Text);
                    string datetag = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    string empty = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);

                    if (columnheadername.Trim() == "Student Count")
                    {
                        #region Student Count

                        q1 = "  select memtype,appno, CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,Stud_Name,sm.SessionName ,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,Registration r,HM_MessMaster mm where p.MenuPurchasePK=pd.MenuPurchaseFK and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and r.App_No=p.Appno and p.SessionFK in('" + sessionFk + "')  and MemType=1  and p.canteenfk in('" + messfk + "')   ";
                        if (rdbtype.SelectedIndex == 0)
                        {
                            q1 = q1 + " and OrderDate between '" + datetag + "' and '" + datetag + "'";
                        }
                        if (rdbtype.SelectedIndex == 1)
                        {
                            int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), Convert.ToInt32(empty));
                            dtm = Convert.ToDateTime(Convert.ToString((empty)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            dtm1 = Convert.ToDateTime(Convert.ToString((empty)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            q1 = q1 + " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                        }
                        q1 = q1 + "group by memtype,appno,OrderDate,MessName,Stud_Name,sm.SessionName  order by OrderDate,Stud_Name";
                        Fpreadheaderbindmethod("S No/Order Date/Canteen Name/Student Name/Session Name/Total Cost", FpSpread2, "true");
                        #endregion
                    }
                    else if (columnheadername.Trim() == "Staff Count")
                    {
                        #region Staff Count

                        q1 = " select memtype,appno, CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,staff_name as Stud_Name,sm.SessionName,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,staffmaster s ,staff_appl_master sa,HM_MessMaster mm where p.MenuPurchasePK =pd.MenuPurchaseFK and s.appl_no =sa.appl_no and sa.appl_id=p.appno and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and p.SessionFK in('" + sessionFk + "') and p.canteenfk in('" + messfk + "')  and MemType=2 ";
                        if (rdbtype.SelectedIndex == 0)
                        {
                            q1 = q1 + " and OrderDate between '" + datetag + "' and '" + datetag + "'";
                        }
                        if (rdbtype.SelectedIndex == 1)
                        {
                            int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), Convert.ToInt32(empty));
                            dtm = Convert.ToDateTime(Convert.ToString((empty)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            dtm1 = Convert.ToDateTime(Convert.ToString((empty)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));
                            q1 = q1 + " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                        }
                        q1 = q1 + "group by memtype,appno,OrderDate,MessName,staff_name,sm.SessionName order by OrderDate,staff_name";

                        Fpreadheaderbindmethod("S No/Order Date/Canteen Name/Staff Name/Session Name/Total Cost", FpSpread2, "true");
                        #endregion
                    }
                    else if (columnheadername.Trim() == "Others Count")
                    {
                        #region Others Count

                        q1 = "  select memtype,appno,CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,SessionName,OthersName as Stud_Name ,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd, HM_SessionMaster sm,HM_MessMaster mm where p.MenuPurchasePK=pd.MenuPurchaseFK  and mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and  p.SessionFK in('" + sessionFk + "') and p.canteenfk in('" + messfk + "')  and MemType=3 ";
                        if (rdbtype.SelectedIndex == 0)
                        {
                            q1 = q1 + " and OrderDate between '" + datetag + "' and '" + datetag + "'";
                        }
                        if (rdbtype.SelectedIndex == 1)
                        {
                            int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), Convert.ToInt32(empty));

                            dtm = Convert.ToDateTime(Convert.ToString((empty)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                            dtm1 = Convert.ToDateTime(Convert.ToString((empty)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                            q1 = q1 + " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                        }
                        q1 = q1 + "group by memtype,appno,OrderDate,MessName,OthersName ,sm.SessionName order by OrderDate,OthersName";

                        Fpreadheaderbindmethod("S No/Order Date/Canteen Name/Others Name/Session Name/Total Cost", FpSpread2, "true");
                        #endregion
                    }
                    else if (columnheadername.Trim() == "Total Count")
                    {
                        #region Total Count

                        string wise = "";
                        if (rdbtype.SelectedIndex == 0)
                        {
                            wise = " and OrderDate between '" + datetag + "' and '" + datetag + "'";
                        }
                        if (rdbtype.SelectedIndex == 1)
                        {
                            int todayend = DateTime.DaysInMonth(Convert.ToInt32(ddl_year.SelectedItem.Text), Convert.ToInt32(empty));

                            dtm = Convert.ToDateTime(Convert.ToString((empty)) + "/" + 01 + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                            dtm1 = Convert.ToDateTime(Convert.ToString((empty)) + "/" + todayend + "/" + Convert.ToString(ddl_year.SelectedItem.Text));

                            wise = " and OrderDate between '" + dtm.ToString("MM/dd/yyyy") + "' and '" + dtm1.ToString("MM/dd/yyyy") + "'";
                        }

                        Fpreadheaderbindmethod("S No/Order Date/Canteen Name/Name/Session Name/Total Cost", FpSpread2, "true");

                        q1 = "   select memtype,appno, CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,Stud_Name,sm.SessionName ,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,Registration r,HM_MessMaster mm where p.MenuPurchasePK=pd.MenuPurchaseFK and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and r.App_No=p.Appno and p.SessionFK in('" + sessionFk + "')  and p.canteenfk in('" + messfk + "')  and MemType=1   " + wise + " group by memtype,appno,OrderDate,MessName,Stud_Name,sm.SessionName order by OrderDate,Stud_Name";

                        q1 += "  select memtype,appno, CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,staff_name as Stud_Name,sm.SessionName,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,staffmaster s ,staff_appl_master sa,HM_MessMaster mm where p.MenuPurchasePK =pd.MenuPurchaseFK and s.appl_no =sa.appl_no and sa.appl_id=p.appno and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and  p.SessionFK in('" + sessionFk + "')  and p.canteenfk in('" + messfk + "')  and MemType=2  " + wise + " group by memtype,appno,OrderDate,MessName,staff_name,sm.SessionName order by OrderDate,staff_name";

                        q1 += "    select memtype,appno,CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,SessionName,OthersName as Stud_Name ,SUM(OrderCost)Totalcost from HT_Menu_purchase p,HT_Menu_purchase_det pd, HM_SessionMaster sm,HM_MessMaster mm where p.MenuPurchasePK=pd.MenuPurchaseFK  and mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.MessMasterFK and p.SessionFK=sm.SessionMasterPK and p.SessionFK in('" + sessionFk + "')  and p.canteenfk in('" + messfk + "')  and MemType=3  " + wise + " group by memtype,appno,OrderDate,MessName,OthersName ,sm.SessionName order by OrderDate,OthersName";

                        ds.Clear();
                        ds = d2.select_method_wo_parameter(q1, "text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Student Details";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 6);
                            for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                binddetails(0);
                            }
                            double consumtotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(Totalcost)", ""));

                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.Purple;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consumtotal);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        }
                        if (ds.Tables[1].Rows.Count > 0)
                        {
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Staff Details";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 6);
                            for (i = 0; i < ds.Tables[1].Rows.Count; i++)
                            {
                                binddetails(1);
                            } double consumtotal = Convert.ToDouble(ds.Tables[1].Compute("Sum(Totalcost)", ""));

                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.Purple;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consumtotal);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        }
                        if (ds.Tables[2].Rows.Count > 0)
                        {
                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = "Others Details";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].BackColor = Color.LightGray;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            FpSpread2.Sheets[0].SpanModel.Add(FpSpread2.Sheets[0].RowCount - 1, 0, 1, 6);
                            for (i = 0; i < ds.Tables[2].Rows.Count; i++)
                            {
                                binddetails(2);
                            }
                            double consumtotal = Convert.ToDouble(ds.Tables[2].Compute("Sum(Totalcost)", ""));

                            FpSpread2.Sheets[0].RowCount++;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.Purple;

                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consumtotal);
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        }
                        if (ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[0].Rows.Count > 0)
                        {
                            FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);

                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                            FpSpread2.Visible = true;
                            rptprint1.Visible = true;
                            FpSpread2.Height = 500;
                            FpSpread2.Width = 950;
                        }
                        else
                        {
                            clear();
                            alertwindow.Visible = true;
                            lbl_alert.Text = "No Records Founds";
                            lbl_alert.Visible = true;
                        }
                        #endregion
                    }
                    #region Details

                    if (columnheadername.Trim() != "Total Count")
                    {
                        if (q1.Trim() != "")
                        {
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(q1, "text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    binddetails(0);
                                    FpSpread2.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread2.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread2.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    FpSpread2.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                }
                                double consumtotal = Convert.ToDouble(ds.Tables[0].Compute("Sum(Totalcost)", ""));

                                FpSpread2.Sheets[0].RowCount++;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = "Grand Total";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].ForeColor = Color.Purple;

                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(consumtotal);
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;
                                FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                                FpSpread2.Visible = true;
                                FpSpread2.Height = 500;
                                FpSpread2.Width = 950;
                            }
                            else
                            {
                                clear();
                                alertwindow.Visible = true;
                                lbl_alert.Text = "No Records Founds";
                                lbl_alert.Visible = true;
                            }
                        }
                    }
                    #endregion
                }
            }
        }
        catch (Exception ex)
        {
            alertwindow.Visible = true;
            lbl_alert.Text = ex.ToString();
            lbl_alert.Visible = true;
        }
    }
    protected void binddetails(int tbl)
    {
        FpSpread2.Sheets[0].RowCount++;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[tbl].Rows[i]["OrderDate"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[tbl].Rows[i]["memtype"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[tbl].Rows[i]["MessName"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[tbl].Rows[i]["appno"]);

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[tbl].Rows[i]["Stud_Name"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[tbl].Rows[i]["SessionName"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[tbl].Rows[i]["Totalcost"]);
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popwindow_menudet.Visible = false;
    }

    protected void FpSpread2_CellClick(object sender, EventArgs e)
    {
        cellclick1 = true;
    }
    protected void FpSpread2_Selectedindexchange(object sender, EventArgs e)
    {

        if (cellclick1 == true)
        {
            try
            {
                string memtype = "";
                string activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
                string activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
                memtype = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string Appno = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string name = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                q1 = "";
                if (memtype.Trim() == "1")
                {
                    q1 = " select CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,Stud_Name,sm.SessionName, m.MenuName,OrderQty, OrderCost  from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,Registration r,HM_MessMaster mm where mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.SessionMasterPK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and r.App_No=p.Appno and MemType=1 and Appno='" + Appno + "' order by OrderDate,MenuName";
                    Fpreadheaderbindmethod("S No-50/Order Date-100/Canteen Name-200/Student Name-250/Session Name-200/Menu Name-200/Order Qty-150/Order Cost-150", FpSpread4, "False");

                }
                else if (memtype.Trim() == "2")
                {
                    q1 = " select CONVERT(varchar(10), OrderDate,103)OrderDate,MessName,staff_name as Stud_Name,sm.SessionName,m.MenuName,OrderQty,OrderCost  from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,staffmaster s ,staff_appl_master sa,HM_MessMaster mm where s.appl_no =sa.appl_no and sa.appl_id=p.appno and  mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.SessionMasterPK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and MemType=2 and Appno='" + Appno + "' order by OrderDate,staff_name,MenuName ";
                    Fpreadheaderbindmethod("S No-50/Order Date-100/Canteen Name-200/Staff Name-250/Session Name-200/Menu Name-200/Order Qty-150/Order Cost-150", FpSpread4, "False");
                }
                else if (memtype.Trim() == "3")
                {
                    q1 = "  select CONVERT(varchar(10), OrderDate,103) OrderDate,MessName,SessionName,OthersName as Stud_Name,m.MenuName, OrderQty,OrderCost from HT_Menu_purchase p,HT_Menu_purchase_det pd,HM_SessionMaster sm,HM_MenuMaster m,HM_MessMaster mm where mm.MessMasterPK=p.canteenfk and sm.MessMasterFK=mm.MessMasterPK and p.canteenfk=sm.SessionMasterPK and p.MenuPurchasePK=pd.MenuPurchaseFK and m.MenuMasterPK=pd.MenuFK and p.SessionFK=sm.SessionMasterPK and MemType=3 and OthersName='" + name + "'  order by OrderDate,OthersName ";

                    Fpreadheaderbindmethod("S No-50/Order Date-100/Canteen Name-200/Others Name-250/Session Name-200/Menu Name-200/Order Qty-150/Order Cost-150", FpSpread4, "False");

                }
                if (q1.Trim() != "")
                {
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cumlativeqty = 0; culativecost = 0;
                        for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FpSpread4.Sheets[0].RowCount++;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderDate"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["MessName"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["SessionName"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuName"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderQty"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["OrderCost"]);
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            string orderqty = Convert.ToString(ds.Tables[0].Rows[i]["OrderQty"]);
                            if (orderqty.Trim() == "")
                                orderqty = "0";
                            cumlativeqty += Convert.ToDouble(orderqty);

                            string cumcost = Convert.ToString(ds.Tables[0].Rows[i]["OrderCost"]);
                            if (cumcost.Trim() == "")
                                cumcost = "0";
                            culativecost += Convert.ToDouble(cumcost);


                            FpSpread4.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread4.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread4.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            FpSpread4.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        }
                        FpSpread4.Sheets[0].RowCount++;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Text = "Grand Total";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 5].ForeColor = Color.Purple;

                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(cumlativeqty);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 6].ForeColor = Color.Purple;

                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(culativecost);
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        FpSpread4.Sheets[0].Cells[FpSpread4.Sheets[0].RowCount - 1, 7].ForeColor = Color.Purple;

                        FpSpread4.Sheets[0].PageSize = FpSpread4.Sheets[0].RowCount;
                        FpSpread4.Visible = true;
                        rptprint2.Visible = true;
                        FpSpread4.Height = 400;
                        FpSpread4.Width = 950;
                        popwindow_menudet.Visible = true;
                    }
                    else
                    {
                        clear();
                        alertwindow.Visible = true;
                        lbl_alert.Text = "No Records Founds";
                        lbl_alert.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                alertwindow.Visible = true;
                lbl_alert.Text = ex.ToString();
                lbl_alert.Visible = true;
            }
        }
    }

    protected string getappno(string studentname)
    {
        string name = d2.GetFunction("select app_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Name='" + studentname + "'");
        return name;
    }
    protected string getappnoREG(string REGNO)
    {
        string name = d2.GetFunction("select app_no from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No='" + REGNO + "'");
        return name;
    }
    protected string getstaffappid(string staffcode)
    {
        string name = d2.GetFunction(" select appl_id  from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and resign =0 and settled =0 and staff_code ='" + staffcode + "'");
        return name;
    }
    protected string getstaffappid_pname(string staffname)
    {
        string name = d2.GetFunction(" select appl_id  from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and resign =0 and settled =0 and staff_name ='" + staffname + "'");
        return name;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.Trim() != "")
            {
                Excelgenerate(FpSpread1, reportname);
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
    protected void btnExcel_Click1(object sender, EventArgs e)
    {
        try
        {
            string reportname = lblrptname1.Text;
            if (reportname.Trim() != "")
            {
                Excelgenerate(FpSpread2, reportname);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation2.Text = "Please Enter Your Report Name";
                lblvalidation2.Visible = true;
                lblrptname1.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click1(object sender, EventArgs e)
    {
        try
        {
            printpdf(FpSpread2, "Smartcardmenu_report", "Cumlative Detail Report");
        }
        catch
        {
        }
    }
    protected void Excelgenerate(FarPoint.Web.Spread.FpSpread spreadname, string ReportName)
    {
        if (ReportName.ToString().Trim() != "")
        {
            d2.printexcelreport(spreadname, ReportName);
        }
    }
    protected void btnExcel_Click2(object sender, EventArgs e)
    {
        try
        {
            string reportname = lblrptname2.Text;
            if (reportname.Trim() != "")
            {
                Excelgenerate(FpSpread4, reportname);
                lblvalidation2.Visible = false;
            }
            else
            {
                lblvalidation3.Text = "Please Enter Your Report Name";
                lblvalidation3.Visible = true;
                lblrptname2.Focus();
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click2(object sender, EventArgs e)
    {
        try
        {
            popwindow_menudet.Visible = false;
            printpdf(FpSpread4, "Smartcardmenu_report", "Cumlative Detail Report");
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            q1 = "";
            if (rdbtype1.SelectedIndex == 0)
            {
                q1 = "Cumlative Detail Report";
            }
            if (rdbtype1.SelectedIndex == 1)
            {
                q1 = "Detail Report";
            }
            printpdf(FpSpread1, "Smartcardmenu_report", q1);
        }
        catch
        {
        }
    }
    protected void printpdf(FarPoint.Web.Spread.FpSpread spreadname, string Pagename, string ReportName)
    {
        Printcontrol.loadspreaddetails(spreadname, Pagename, ReportName);
        Printcontrol.Visible = true;
    }

    protected void txt_staffcodesearch1_txtchange(object sender, EventArgs e)
    {
        string app = getstaffappid(txt_staffcodesearch1.Text.Trim());
        if (app.Trim() == "0")
        {
            txt_staffcodesearch1.Text = "";
        }
    }
    protected void txt_staffnamesearch1_txtchange(object sender, EventArgs e)
    {
        string app = getstaffappid_pname(txt_staffnamesearch1.Text.Trim());
        if (app.Trim() == "0")
        {
            txt_staffnamesearch1.Text = "";
        }
    }
    protected void txt_othername_ontextchange(object sender, EventArgs e)
    {
        string app = d2.GetFunction("select OthersName from HT_Menu_purchase where MemType='3' and OthersName='" + (txt_staffnamesearch1.Text.Trim() + "'"));
        if (app.Trim() == "0")
        {
            txt_othername.Text = "";
        }
    }
}