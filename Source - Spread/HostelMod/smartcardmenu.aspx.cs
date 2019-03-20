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

public partial class smartcardmenu : System.Web.UI.Page
{
    bool cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collegecodestat = string.Empty;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    static ArrayList arrmenuitem = new ArrayList();
    string q1 = ""; //static double subtotal = 0;
    string AppNo = "";
    Hashtable hat = new Hashtable();
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
            collegecodestat = collegecode1;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                //lblheader.Text = Convert.ToString(d2.GetFunction("select collname from collinfo where college_code='" + collegecode1 + "'"));
                clear();

                bindcollege();
                bindcanteen();
                bindsession();
                binddegree();
                branch();
                bindbatch();
                bindstaffdepartmentpopup();
                rdbtype.Items[0].Selected = true;
                txt_studentname.Attributes.Add("readonly", "readonly");
                txt_studenttype.Attributes.Add("readonly", "readonly");
                txt_degree.Attributes.Add("readonly", "readonly");
                ViewState["subamttotal"] = null;
                txt_staffnamesearch1.Visible = true;
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_canteenname_selectedindex(object sender, EventArgs e)
    {
        bindsession();
    }
    protected void bindsession()
    {
        //string rigths = d2.GetFunction(" select value from Master_Settings where settings='Mess Rights'  and usercode='" + usercode + "' and value<>''");
        //string r = rigths.Replace(",", "','");
        string r = "";
        if (ddl_canteenname.Items.Count > 0)
        {
            r = Convert.ToString(ddl_canteenname.SelectedItem.Value);
            string deptquery = " select  SessionMasterPK,SessionName  from HM_SessionMaster where MessMasterFK in ('" + r + "') order by SessionMasterPK ";
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_sessionname.DataSource = ds;
                ddl_sessionname.DataTextField = "SessionName";
                ddl_sessionname.DataValueField = "SessionMasterPK";
                ddl_sessionname.DataBind();

                ddl_staffsessionname.DataSource = ds;
                ddl_staffsessionname.DataTextField = "SessionName";
                ddl_staffsessionname.DataValueField = "SessionMasterPK";
                ddl_staffsessionname.DataBind();

                ddl_othersessionname.DataSource = ds;
                ddl_othersessionname.DataTextField = "SessionName";
                ddl_othersessionname.DataValueField = "SessionMasterPK";
                ddl_othersessionname.DataBind();

            }
        }
    }

    protected void bindsessionname(Label sessionlbl)
    {
        string stime = DateTime.Now.ToString("h:mm:ss tt");
        DateTime dt = new DateTime();
        dt = DateTime.Now.AddHours(2);
        string days = Convert.ToString(dt.ToString("h:mm:ss tt"));

        string sessionname = d2.GetFunction("select SessionName from HM_SessionMaster  where convert(datetime, SessionStartTime) >= '" + DateTime.Now.ToString("h:mm:ss tt") + "' and convert(datetime, SessionCloseTime) <= '" + days + "' ");
        if (sessionname.Trim() != "0")
            sessionlbl.Text = sessionname;
        else
            sessionlbl.Text = "Please Login Session Time";
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
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%' ";
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

                query = "select Stud_Name  from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Name like '" + prefixText + "%'  ";//and college_code=" + collegecodestat + "

                name = ws.Getname(query);
            }
            return name;
        }
        catch { return name; }
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
    protected void ddlstudenttype_onselectedindexchange(object sender, EventArgs e)
    {
        lbl_smartroll.Visible = false;
        txt_smartrollno.Visible = false;
        if (ddlstudenttype.SelectedIndex == 0)
        {
            txt_studrollsearch.Visible = true;
            txt_studnamesearch.Visible = false;
            txt_smartcardsearch.Visible = false;
            txt_studregno.Visible = false;
            txt_studrollsearch.Text = "";
            txt_studnamesearch.Text = "";
            txt_smartcardsearch.Text = "";
            txt_studregno.Text = "";
        }
        else if (ddlstudenttype.SelectedIndex == 1)
        {
            txt_studnamesearch.Visible = true;
            txt_smartcardsearch.Visible = false;
            txt_studrollsearch.Visible = false;
            txt_studregno.Visible = false;
            txt_studrollsearch.Text = "";
            txt_studnamesearch.Text = "";
            txt_smartcardsearch.Text = "";
            txt_studregno.Text = "";
        }
        else if (ddlstudenttype.SelectedIndex == 2)
        {
            txt_studregno.Visible = true;
            txt_studrollsearch.Visible = false;
            txt_studnamesearch.Visible = false;
            txt_smartcardsearch.Visible = false;
            txt_studrollsearch.Text = "";
            txt_studnamesearch.Text = "";
            txt_studregno.Text = "";
            txt_smartcardsearch.Text = "";
        }
        else if (ddlstudenttype.SelectedIndex == 3)
        {
            txt_smartcardsearch.Visible = true;
            txt_studrollsearch.Visible = false;
            txt_studnamesearch.Visible = false;
            txt_studregno.Visible = false;
            txt_studrollsearch.Text = "";
            txt_studnamesearch.Text = "";
            txt_studregno.Text = "";
            txt_smartcardsearch.Text = "";
            lbl_smartroll.Visible = true;
            txt_smartrollno.Visible = true;
        }
        clear();
    }
    protected void ddl_stafftype_onselectedindexchange(object sender, EventArgs e)
    {
        if (ddl_stafftype.SelectedIndex == 0)
        {
            txt_staffcodesearch.Visible = true;
            txt_staffnamesearch.Visible = false;
            txt_staffsmartcardsearch.Visible = false;
            txt_staffcodesearch.Text = "";
            txt_staffnamesearch.Text = "";
            txt_staffsmartcardsearch.Text = "";
        }
        else if (ddl_stafftype.SelectedIndex == 1)
        {
            txt_staffnamesearch.Visible = true;
            txt_staffsmartcardsearch.Visible = false;
            txt_staffcodesearch.Visible = false;
            txt_staffcodesearch.Text = "";
            txt_staffnamesearch.Text = "";
            txt_staffsmartcardsearch.Text = "";
        }
        else if (ddl_stafftype.SelectedIndex == 2)
        {
            txt_staffsmartcardsearch.Visible = true;
            txt_staffcodesearch.Visible = false;
            txt_staffnamesearch.Visible = false;
            txt_staffcodesearch.Text = "";
            txt_staffnamesearch.Text = "";
            txt_staffsmartcardsearch.Text = "";
        }
    }
    protected void clear()
    {
        arrmenuitem.Clear();
        Session["menuitemdt"] = null;
        SelectMenuitemGrid.Visible = false;

        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        txt_staffsmartcardsearch.Text = "";
        txt_studentname.Text = "";
        txt_studenttype.Text = "";
        txt_degree.Text = "";
        lbl_studimage.ImageUrl = "~/images/dummyimg.png";

        txt_studrollsearch.Text = "";
        txt_studnamesearch.Text = "";
        txt_studregno.Text = "";
        txt_smartcardsearch.Text = "";
        txt_staffname.Text = "";
        txt_staffdegree.Text = "";
        staffimg.ImageUrl = "~/images/dummyimg.png";

        txt_othersname.Text = "";
        txt_othermenu.Text = "";
        txt_othermenuqty.Text = "";
    }
    protected void rdbtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        MultiView1.ActiveViewIndex = Convert.ToInt32(rdbtype.SelectedValue);
        clear();

        if (Convert.ToInt32(rdbtype.SelectedValue) == 0)
        {
            ViewState["subamttotal"] = 0;
            txt_studrollsearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 1)
        {
            ViewState["subamttotal"] = 0;
            txt_staffcodesearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 2)
        {
            ViewState["subamttotal"] = 0;
            txt_othersname.Focus();
        }
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
    protected void txt_smartcardsearch_TextChange(object sender, EventArgs e)
    {
        OnTextChange(txt_smartcardsearch);
    }
    protected void txt_staffcodesearch_textchange(object sender, EventArgs e)
    {
        OnTextChange(txt_staffcodesearch);
    }
    protected void txt_staffnamesearch_textchange(object sender, EventArgs e)
    {
        OnTextChange(txt_staffnamesearch);
    }
    protected void OnTextChange(TextBox txt)
    {
        try
        {
            string condition = "";
            if (txt.ID == "txt_studrollsearch" || txt.ID == "txt_studregno" || txt.ID == "txt_studnamesearch" || txt.ID == "txt_smartcardsearch")
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
                else if (txt.ID == "txt_smartcardsearch")
                {
                    condition = " and smart_serial_no='" + txt_smartcardsearch.Text + "'";
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
                    txt_studentname.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                    txt_studenttype.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]);
                    txt_degree.Text = Convert.ToString(ds.Tables[0].Rows[0]["batch"]);
                    string rollno = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);

                    txt_smartrollno.Text = Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]);

                    lbl_studimage.Visible = true;
                    lbl_studimage.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + rollno;
                    Session["menuitemdt"] = null;
                    txt_studmenuname.Text = "";
                    txt_studmenuqty.Text = "";
                    txt_studmenuname.Focus();
                    SelectMenuitemGrid.Visible = false;
                    ViewState["subamttotal"] = 0;
                }
            }
            else if (txt.ID == "txt_staffcodesearch" || txt.ID == "txt_staffnamesearch")
            {
                if (txt.ID == "txt_staffcodesearch")
                {
                    condition = " and s.staff_Code='" + txt_staffcodesearch.Text + "'";
                }
                else if (txt.ID == "txt_staffnamesearch")
                {
                    condition = " and s.staff_name='" + txt_staffnamesearch.Text + "'";
                }
                if (condition.Trim() != "")
                {
                    q1 = "";
                    q1 = "  select s.staff_Code,s.staff_name,desig_name from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 " + condition + "";
                }
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_staffname.Text = ds.Tables[0].Rows[0]["staff_name"].ToString();
                    txt_staffdegree.Text = ds.Tables[0].Rows[0]["desig_name"].ToString();
                    string staffcode = ds.Tables[0].Rows[0]["staff_Code"].ToString();
                    staffimg.Visible = true;
                    staffimg.ImageUrl = "~/Handler/staffphoto.ashx?staff_code=" + staffcode;
                    Session["menuitemdt"] = null;
                    txt_staffmenu.Text = "";
                    txt_studmenuqty.Text = "";
                    txt_staffmenu.Focus();
                    SelectMenuitemGrid.Visible = false;
                    ViewState["subamttotal"] = 0;
                }
                else
                {
                    txt_staffcodesearch.Text = "";
                    txt_staffname.Text = "";
                    txt_staffdegree.Text = "";
                    txt_staffcodesearch.Focus();
                }
            }
        }
        catch { }
    }
    protected DataSet menuaddgrid(Button btn)
    {
        ds.Clear();
        if (btn.ID == "btn_studadd")
        {
            if (txt_studmenuname.Text.Trim() != "")
            {
                q1 = " select top(1) MenuMasterPK,MenuAmount,MenuName from HM_MenuCostMaster mc,HM_MenuMaster m where mc.MenuMasterFK=m.MenuMasterPK and m.MenuName in('" + txt_studmenuname.Text + "')  order by Menucost_Date desc";//and MenuQty ='" + txt_studmenuqty.Text + "'
                ds = d2.select_method_wo_parameter(q1, "text");
            }
        }
        else if (btn.ID == "btn_staffadd")
        {
            if (txt_staffmenu.Text.Trim() != "")
            {
                q1 = " select top(1) MenuMasterPK,MenuAmount,MenuName from HM_MenuCostMaster mc,HM_MenuMaster m where mc.MenuMasterFK=m.MenuMasterPK and m.MenuName in('" + txt_staffmenu.Text + "')  order by Menucost_Date desc";//and MenuQty ='" + txt_studmenuqty.Text + "'
                ds = d2.select_method_wo_parameter(q1, "text");
            }
        }
        else if (btn.ID == "btn_otheradd")
        {
            if (txt_othermenu.Text.Trim() != "")
            {
                q1 = " select top(1) MenuMasterPK,MenuAmount,MenuName from HM_MenuCostMaster mc,HM_MenuMaster m where mc.MenuMasterFK=m.MenuMasterPK and m.MenuName in('" + txt_othermenu.Text + "')  order by Menucost_Date desc";//and MenuQty ='" + txt_studmenuqty.Text + "'
                ds = d2.select_method_wo_parameter(q1, "text");
            }
        }
        return ds;
    }
    protected void btn_studadd_Onclick(object sender, EventArgs e)
    {
        if (txt_studmenuqty.Text.Trim() != "" && txt_studmenuname.Text.Trim() != "")
        {
            bindmenuitem(txt_studmenuqty, btn_studadd);
            txt_studmenuname.Text = "";
            txt_studmenuqty.Text = "";
            txt_studmenuname.Focus();
        }
    }
    protected void btn_staffadd_Onclick(object sender, EventArgs e)
    {
        if (txt_staffmenuqty.Text.Trim() != "" && txt_staffmenu.Text.Trim() != "")
        {
            bindmenuitem(txt_staffmenuqty, btn_staffadd);
            txt_staffmenu.Text = "";
            txt_staffmenuqty.Text = "";
            txt_staffmenu.Focus();
        }
    }
    protected void btn_otheradd_Onclick(object sender, EventArgs e)
    {
        if (txt_othermenuqty.Text.Trim() != "" && txt_othermenu.Text.Trim() != "")
        {
            bindmenuitem(txt_othermenuqty, btn_otheradd);
            txt_othermenu.Text = "";
            txt_othermenuqty.Text = "";
            txt_othermenu.Focus();
        }
    }

    protected void bindmenuitem(TextBox txt, Button btn)
    {
        DataRow dr;
        if (txt.Text.Trim() != "")
        {
            dt_columnsadd();
            ds1 = menuaddgrid(btn);
            string qty = txt.Text; double totalamt = 0;
            if (Session["menuitemdt"] == null)
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        dr = dt.NewRow();
                        dr[0] = Convert.ToString(ds1.Tables[0].Rows[0]["MenuName"]);
                        dr[1] = Convert.ToString(ds1.Tables[0].Rows[0]["MenuMasterPK"]);
                        dr[2] = Convert.ToString(ds1.Tables[0].Rows[0]["MenuAmount"]);
                        dr[3] = Convert.ToString(qty);
                        string menuamt = Convert.ToString(ds1.Tables[0].Rows[0]["MenuAmount"]);
                        if (menuamt.Trim() == "")
                            menuamt = "0";
                        if (qty.Trim() == "")
                            qty = "0";
                        totalamt = Convert.ToDouble(qty) * Convert.ToDouble(menuamt);
                        dr[4] = Convert.ToString(totalamt);
                        //subtotal += totalamt;
                        ViewState["subamttotal"] = Convert.ToDouble(ViewState["subamttotal"]) + totalamt;
                        dt.Rows.Add(dr);
                        arrmenuitem.Add(Convert.ToString(ds1.Tables[0].Rows[0]["MenuMasterPK"]));
                    }
                    dr = dt.NewRow();
                    dr[0] = Convert.ToString("");
                    dr[1] = Convert.ToString("");
                    dr[2] = Convert.ToString("");
                    dr[3] = Convert.ToString("Pay Amount");
                    dr[4] = Convert.ToString(ViewState["subamttotal"]);
                    dt.Rows.Add(dr);
                    SelectMenuitemGrid.DataSource = dt;
                    SelectMenuitemGrid.DataBind();
                    SelectMenuitemGrid.Rows[SelectMenuitemGrid.Rows.Count - 1].ForeColor = Color.DarkRed;
                    SelectMenuitemGrid.Visible = true;

                    dt.Rows.RemoveAt(SelectMenuitemGrid.Rows.Count - 1);
                    Session["menuitemdt"] = dt;
                }
            }
            else
            {
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    dt = (DataTable)Session["menuitemdt"];
                    if (!arrmenuitem.Contains(Convert.ToString(ds1.Tables[0].Rows[0]["MenumasterPK"])))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString(ds1.Tables[0].Rows[0]["MenuName"]);
                            dr[1] = Convert.ToString(ds1.Tables[0].Rows[0]["MenumasterPK"]);
                            dr[2] = Convert.ToString(ds1.Tables[0].Rows[0]["MenuAmount"]);
                            dr[3] = Convert.ToString(qty);
                            string menuamt = Convert.ToString(ds1.Tables[0].Rows[0]["MenuAmount"]);
                            if (menuamt.Trim() == "")
                                menuamt = "0";
                            if (qty.Trim() == "")
                                qty = "0";
                            totalamt = Convert.ToDouble(qty) * Convert.ToDouble(menuamt);
                            dr[4] = Convert.ToString(totalamt);
                            //subtotal += totalamt;
                            ViewState["subamttotal"] = Convert.ToDouble(ViewState["subamttotal"]) + totalamt;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[0] = Convert.ToString("");
                            dr[1] = Convert.ToString("");
                            dr[2] = Convert.ToString("");
                            dr[3] = Convert.ToString("Pay Amount");
                            dr[4] = Convert.ToString(ViewState["subamttotal"]);
                            dt.Rows.Add(dr);
                            SelectMenuitemGrid.DataSource = dt;
                            SelectMenuitemGrid.DataBind();
                            SelectMenuitemGrid.Rows[SelectMenuitemGrid.Rows.Count - 1].ForeColor = Color.DarkRed;
                            SelectMenuitemGrid.Visible = true;
                            dt.Rows.RemoveAt(SelectMenuitemGrid.Rows.Count - 1);
                            Session["menuitemdt"] = dt;
                            arrmenuitem.Add(Convert.ToString(ds1.Tables[0].Rows[0]["MenumasterPK"]));
                        }
                    }
                }
            }
        }
        else
        {

        }
    }
    protected void dt_columnsadd()
    {
        dt.Columns.Add("Menu Name");
        dt.Columns.Add("MenumasterFK");
        dt.Columns.Add("Cost");
        dt.Columns.Add("Menu Quantity");
        dt.Columns.Add("Total Cost");
    }
    protected void btn_save_Onclick(object sender, EventArgs e)
    {
        string memtype = "";
        string sessionfk = "";
        string othername = "";

        if (SelectMenuitemGrid.Rows.Count > 0)
        {
            if (Convert.ToInt32(rdbtype.SelectedValue) == 0)
            {
                if (ddlstudenttype.SelectedItem.Value == "0")
                {
                    AppNo = d2.getappno(txt_studrollsearch.Text);
                }
                else if (ddlstudenttype.SelectedItem.Value == "1")
                {
                    //AppNo = getappno(txt_studnamesearch.Text);
                    AppNo = d2.getappno(txt_smartrollno.Text);
                }
                else if (ddlstudenttype.SelectedItem.Value == "2")
                {
                    AppNo = getappnoREG(txt_studregno.Text);
                }
                else if (ddlstudenttype.SelectedItem.Value == "3")
                {
                    AppNo = d2.getappno(txt_smartrollno.Text);
                }
                memtype = "1";
                sessionfk = Convert.ToString(ddl_sessionname.SelectedItem.Value);
            }
            else if (Convert.ToInt32(rdbtype.SelectedValue) == 1)
            {
                if (ddl_stafftype.SelectedItem.Value == "0")
                {
                    AppNo = getstaffappid(txt_staffcodesearch.Text);
                }
                else if (ddl_stafftype.SelectedItem.Value == "1")
                {
                    AppNo = getstaffappid_pname(txt_staffnamesearch.Text);
                }
                else if (ddl_stafftype.SelectedItem.Value == "2")
                {
                    AppNo = getstaffappid_pname(txt_staffname.Text);
                }
                memtype = "2";
                sessionfk = Convert.ToString(ddl_staffsessionname.SelectedItem.Value);
            }
            else if (Convert.ToInt32(rdbtype.SelectedValue) == 2)
            {
                memtype = "3";
                sessionfk = Convert.ToString(ddl_othersessionname.SelectedItem.Value);
                othername = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Convert.ToString(txt_othersname.Text));
            }
            if (sessionfk.Trim() == "")
            {
                return;
            }
            int ins = 0;
            if (sessionfk.Trim() != "" && Convert.ToString(ddl_canteenname.SelectedItem.Value).Trim() != "")
            {
                q1 = "";
                q1 = "insert into HT_Menu_purchase (Appno,SessionFK,MemType,OrderDate,OthersName,CanteenFK) values('" + AppNo + "','" + sessionfk + "','" + memtype + "','" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "','" + othername + "','" + Convert.ToString(ddl_canteenname.SelectedItem.Value) + "')";
                ins = d2.update_method_wo_parameter(q1, "Text");

                string menupurchasepk = d2.GetFunction(" select MenuPurchasePK from HT_Menu_purchase where Appno='" + AppNo + "' and OrderDate='" + Convert.ToString(System.DateTime.Now.ToString("MM/dd/yyyy")) + "' order by OrderDate desc");

                for (int i = 0; i < SelectMenuitemGrid.Rows.Count - 1; i++)
                {
                    string MenumasterFK = Convert.ToString((SelectMenuitemGrid.Rows[i].FindControl("lbl_menumasterfk") as Label).Text);
                    string Cost = Convert.ToString((SelectMenuitemGrid.Rows[i].FindControl("lbl_Cost") as Label).Text);
                    string qty = Convert.ToString((SelectMenuitemGrid.Rows[i].FindControl("txt_quantity") as Label).Text);

                    q1 = "";
                    q1 = "insert into HT_Menu_purchase_det(MenuPurchaseFK,MenuFK,OrderQty,OrderCost) values ('" + menupurchasepk + "','" + MenumasterFK + "','" + qty + "','" + Cost + "') ";
                    int insdet = d2.update_method_wo_parameter(q1, "text");
                }
            }
            else
            {
                alertwindow.Visible = true;
                lbl_alert.Text = "Please Select Session Name & Canteen Name";
            }
            if (ins != 0)
            {
                clear();
                btn_errorclose.Focus();
                alertwindow.Visible = true;
                lbl_alert.Text = "Saved Successfully";

            }
        }
        if (Convert.ToInt32(rdbtype.SelectedValue) == 0)
        {
            if (txt_studrollsearch.Visible == true)
                txt_studrollsearch.Focus();
            if (txt_studentname.Visible == true)
                txt_studentname.Focus();
            if (txt_staffsmartcardsearch.Visible == true)
                txt_staffsmartcardsearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 1)
        {
            txt_staffcodesearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 2)
        {
            txt_othersname.Focus();
        }
        btn_errorclose.Focus();
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
    protected void btn1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = true;
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            string deptcode = returnwithsinglecodevalue(cbl_branch);

            q1 = " select r.App_No,roll_no,r.reg_no,r.stud_name,g.Degree_Code,CONVERT(varchar(4), r.Batch_Year)+'-'+course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and r.Batch_Year ='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and g.Degree_Code in('" + deptcode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpreadheaderbindmethod("S No-50/Roll No-100/Reg No-100/Student Name-200/Department-430", Fpspread2, "false");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["reg_no"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["branch"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread2.Columns[0].Locked = true;
                    Fpspread2.Columns[1].Locked = true;
                    Fpspread2.Columns[2].Locked = true;
                    Fpspread2.Columns[3].Locked = true;
                    Fpspread2.Columns[4].Locked = true;
                }
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                Fpspread2.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;
            }
            else
            {
                lbl_errormsg1.Text = "No Records Founds";
                btn_ok.Visible = false;
                btn_exit1.Visible = false;
                Fpspread2.Visible = false;
            }
        }
        catch { }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
        menuloop.Visible = false;
    }
    protected void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_degree, cbl_degree, txt_degree1, "Degree");
        branch();
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_degree, cbl_degree, txt_degree1, "Degree");
        branch();
    }
    protected void cb_branch_ChekedChange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_branch, cbl_branch, txt_branch, "Branch");
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_branch, cbl_branch, txt_branch, "Branch");
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


    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            cellclick = true;
        }
        catch { }
    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        string activerow = "";
        activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
        string rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
        txt_studrollsearch.Text = rollno;
        ddlstudenttype.SelectedIndex = 0;
        popupselectstd.Visible = false;
        txt_studrollsearch_TextChange(sender, e);
        txt_studmenuname.Focus();
    }
    protected void btn_staffsave_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            string rollno = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
            txt_staffcodesearch.Text = rollno;
            ddl_stafftype.SelectedIndex = 0;
            popupstaffcode1.Visible = false;
            txt_staffcodesearch_textchange(sender, e);
            txt_staffmenu.Focus();
        }
        catch { }
    }
    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }
    protected void btn_staffselectgo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            string sql = "";
            int rowcount;
            string condition = "";
            if (txt_staffnamesearch.Text != "" || txt_staffcodesearch.Text.Trim() != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 0)
                {
                    condition = " and s.Staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "' ";
                }

                if (ddl_searchbystaff.SelectedIndex == 1)
                {
                    condition = " and s.staff_code ='" + Convert.ToString(txt_staffcodesearch.Text) + "' ";
                }
                sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' " + condition + "   order by s.staff_code";
            }
            else
            {
                if (ddl_department3.SelectedItem.Text == "All")
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' order by s.staff_code";
                }
                else
                {
                    sql = "select a.appl_id,s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,d.desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d,staff_appl_master a where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and s.appl_no = a.appl_no and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code  and d.collegeCode=a.college_code and collegeCode='" + Convert.ToString(ddl_college2.SelectedItem.Value) + "' and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "') order by s.staff_code";
                }
            }
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpreadheaderbindmethod("S.No-50/Staff Code-100/Staff Name-230/Department-250/Designation-200", Fpstaff, "false");

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["appl_id"]);

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpstaff.Columns[0].Locked = true;
                    Fpstaff.Columns[1].Locked = true;
                    Fpstaff.Columns[2].Locked = true;
                    Fpstaff.Columns[3].Locked = true;
                    Fpstaff.Columns[4].Locked = true;
                }
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 345;
                Fpstaff.Width = 846;
                btn_staffsave1.Visible = true;
                btn_staffexit.Visible = true;
                Fpstaff.Visible = true;
                div1.Visible = true;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                Fpstaff.Visible = false;
                btn_staffsave1.Visible = false;
                btn_staffexit.Visible = false;
                div1.Visible = false;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btn_staffpop_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = true;
    }
    protected void menunamelookup_Click(object sender, EventArgs e)
    {
        menuloop.Visible = true;
    }
    protected void btn_ok1_Click(object sender, EventArgs e)
    {
        string activerow = "";
        activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
        string menuname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);

        if (rdbtype.SelectedIndex == 0)
        {
            txt_studmenuname.Text = menuname;
        }
        else if (rdbtype.SelectedIndex == 1)
        {
            txt_staffmenu.Text = menuname;
        }
        else if (rdbtype.SelectedIndex == 2)
        {
            txt_othermenu.Text = menuname;
        }

        ddlstudenttype.SelectedIndex = 0;
        menuloop.Visible = false;
        txt_studmenuqty.Focus();
    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        menuloop.Visible = false;
    }
    protected void btnmenugo_Click(object sender, EventArgs e)
    {
        try
        {
            string mess1 = "";
            Label3.Text = "";
            if (ddl_menutype.SelectedItem.Text == "All")
            {
                mess1 = "0','1";
            }
            else if (ddl_menutype.SelectedItem.Text == "Veg")
            {
                mess1 = "0";
            }
            else if (ddl_menutype.SelectedItem.Text == "Non-Veg")
            {
                mess1 = "1";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(" select MenuCode,MenuName from HM_MenuMaster where MenuType in('" + mess1 + "')", "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpreadheaderbindmethod("S No-50/Menu Code-150/Menu Name-250", Fpspread1, "false");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuCode"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["MenuName"]);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpspread1.Columns[0].Locked = true;
                        Fpspread1.Columns[1].Locked = true;
                        Fpspread1.Columns[2].Locked = true;
                    }
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    Fpspread1.Visible = true;
                    btn_ok1.Visible = true;
                    btn_exit2.Visible = true;
                }
                else
                {
                    btn_ok1.Visible = false;
                    Fpspread1.Visible = false;
                    btn_exit2.Visible = false;
                    Label3.Text = "No Records Founds";
                }
            }
        }
        catch
        { }
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
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
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

            ddl_college2.DataSource = ds;
            ddl_college2.DataTextField = "collname";
            ddl_college2.DataValueField = "college_code";
            ddl_college2.DataBind();

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
    protected void binddegree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }

            ds = d2.select_method_wo_parameter(query, "Text");
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
                    txt_degree1.Text = "Degree(" + cbl_degree.Items.Count + ")";
                }
                else
                {
                    txt_degree1.Text = "--Select--";
                }
            }
            else
            {
                txt_degree1.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void branch()
    {
        try
        {
            string query1 = "";
            string buildvalue1 = "";
            txt_branch.Text = "--Select--";
            cbl_branch.Items.Clear();
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = "" + cbl_degree.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "" + "'" + cbl_degree.Items[i].Value.ToString() + "";
                        }
                    }
                }
                query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_collgname.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code";
                ds = d2.select_method(query1, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Degree(" + cbl_branch.Items.Count + ")";
                    }
                    else
                    {
                        txt_branch.Text = "--Select--";
                    }
                }
                else
                {
                    txt_branch.Text = "--Select--";
                }
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_collgname_selectedindexchange(object sender, EventArgs e)
    {
        binddegree();
        branch();
    }
    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        alertwindow.Visible = false;
        if (Convert.ToInt32(rdbtype.SelectedValue) == 0)
        {
            txt_studrollsearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 1)
        {
            txt_staffcodesearch.Focus();
        }
        else if (Convert.ToInt32(rdbtype.SelectedValue) == 2)
        {
            txt_othersname.Focus();
        }
    }

    protected void ddl_searchbystaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchbystaff.SelectedItem.Text == "Staff Name")
        {
            txt_staffnamesearch1.Visible = true;
            txt_staffcodesearch1.Visible = false;
            txt_staffnamesearch1.Text = "";

        }
        else if (ddl_searchbystaff.SelectedItem.Text == "Staff Code")
        {
            txt_staffcodesearch1.Visible = true;
            txt_staffnamesearch1.Visible = false;
            txt_staffnamesearch1.Text = "";
        }
    }
    protected void ddl_college2_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bindstaffdepartmentpopup();
        }
        catch
        {
        }
    }
    public void bindstaffdepartmentpopup()
    {
        try
        {
            ds.Clear();
            string clgcode = "";
            if (ddl_college2.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddl_college2.SelectedItem.Value);
            }
            ds = d2.loaddepartment(clgcode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_department3.DataSource = ds;
                ddl_department3.DataTextField = "dept_name";
                ddl_department3.DataValueField = "dept_code";
                ddl_department3.DataBind();

                ddl_department3.Items.Insert(0, "All");
            }

        }
        catch { }
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }

    protected void bindcanteen()
    {
        try
        {
            ds.Clear();
            ddl_canteenname.Items.Clear();
            ds = d2.Bindmess_basedonrights(usercode, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_canteenname.DataSource = ds;
                ddl_canteenname.DataTextField = "MessName";
                ddl_canteenname.DataValueField = "MessMasterPK";
                ddl_canteenname.DataBind();
            }
        }
        catch { }
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
}