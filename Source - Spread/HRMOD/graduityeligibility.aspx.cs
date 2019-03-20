using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Drawing;
using System.Collections;
using System.Text;
using System.Web.UI;
using System.Web.Services;
using System.Text.RegularExpressions;



public partial class graduityeligibility : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"].ToString());
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    int colheder;
    int colgross;
    int col;
    string sql;
    string sql1 = "";
    string strdept = "";
    string strcategory = "";
    Hashtable hatpre = new Hashtable();
    Hashtable splallow = new Hashtable();
    Hashtable hat = new Hashtable();
    Hashtable ColumnWidth = new Hashtable();
    Hashtable ColumnAdjWid = new Hashtable();
    static Hashtable getcol = new Hashtable();
    DataSet dssmssalary = new DataSet();
    SortedDictionary<string, string> deduct = new SortedDictionary<string, string>();

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
            usercode = Convert.ToString(Session["usercode"]);
            if (!IsPostBack)
            {
                bindcollege();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                    collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
                    allowance();
                    common();
                }
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
            }
        }
        catch (Exception ex) { }
    }


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_code like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_code";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and ISNULL(Discontinue,'0')='0' and staff_name like  '" + prefixText + "%' and college_code='" + collegecode + "' order by staff_name";
        name = ws.Getname(query);
        return name;
    }
    protected void ddlcollege_Change(object sender, EventArgs e)
    {
        collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
        collegecode1 = Convert.ToString(ddlcollege.SelectedItem.Value);
        allowance();
        common();

    }
    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.Enabled = true;
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception e) { }
    }

    protected void cb_allow_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cbl_allow_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cb_allow, cbl_allow, txt_allow, "Allowance");
    }
    protected void cbcommon_CheckedChange(object sender, EventArgs e)
    {
        chkchange(cbcommon, cblcommon, txtcommon, "Common");

    }
    protected void cblcommon_SelectedIndexChange(object sender, EventArgs e)
    {
        chklstchange(cbcommon, cblcommon, txtcommon, "Common");
    }

    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "--Select--";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                    count = count + 1;
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                    chkchange.Checked = true;
            }
        }
        catch { }
    }
    protected void allowance()
    {
        try
        {
            ds.Clear();
            cbl_allow.Items.Clear();

            string item = "select allowances from incentives_master where college_code = '" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_allow.DataSource = ds;
                string st = Convert.ToString(ds.Tables[0].Rows[0]["allowances"]);
                string[] split = st.Split(';');
                for (int row = 0; row < split.Length; row++)
                {
                    string staff = split[row];
                    string[] split1 = staff.Split('\\');
                    if (split1.Length > 1)
                    {
                        string stafftype = split1[0];
                        cbl_allow.Items.Add(stafftype);

                    }
                }
                if (cbl_allow.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_allow.Items.Count; i++)
                    {
                        cbl_allow.Items[i].Selected = true;
                    }
                    txt_allow.Text = "Allowance (" + cbl_allow.Items.Count + ")";
                    cb_allow.Checked = true;
                }
            }
            else
            {
                txt_allow.Text = "--Select--";
                cb_allow.Checked = false;

            }
        }
        catch { }
    }

    protected void common()
    {
        try
        {
            cblcommon.Items.Clear();
            cblcommon.Items.Add("Basic");
            cblcommon.Items.Add("Grade Pay");
            cblcommon.Items.Add("Pay Band");
            if (cblcommon.Items.Count > 0)
            {
                for (int i = 0; i < cblcommon.Items.Count; i++)
                {
                    cblcommon.Items[i].Selected = true;
                }
                txtcommon.Text = "Common (" + cblcommon.Items.Count + ")";
                cbcommon.Checked = true;
            }

        }
        catch (Exception ex)
        {

        }
    }
    protected void btn_selection(object sender, EventArgs e)
    {
        grdgratuity.Visible = false;
        if (txtcommon.Text.Trim() == "--Select" && txt_allow.Text.Trim() == "--Select--")
        {
            grdgratuity.Visible = false;
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Any Item\");", true);

            return;
        }
        else
        {
            if (cbgross.Checked == false)
            {
                string getallval = "";
                int count = 0;
                DataTable dtl = new DataTable();
                DataRow dtrow = null;
                dtl.Columns.Add("gratuityCal", typeof(string));


                if (cblcommon.Items.Count > 0)
                {
                    for (int ro = 0; ro < cblcommon.Items.Count; ro++)
                    {
                        if (cblcommon.Items[ro].Selected == true)
                        {
                            string gettext = Convert.ToString(cblcommon.Items[ro].Text);
                            dtrow = dtl.NewRow();
                            dtrow["gratuityCal"] = Convert.ToString(gettext);
                            dtl.Rows.Add(dtrow);
                        }
                    }
                }

                if (cbl_allow.Items.Count > 0)
                {

                    for (int ro = 0; ro < cbl_allow.Items.Count; ro++)
                    {
                        if (cbl_allow.Items[ro].Selected == true)
                        {
                            string gettext = Convert.ToString(cbl_allow.Items[ro].Text);
                            dtrow = dtl.NewRow();
                            dtrow["gratuityCal"] = Convert.ToString(gettext);
                            dtl.Rows.Add(dtrow);
                        }
                    }


                }
                div1.Visible = true;
                grdgratuity.DataSource = dtl;
                grdgratuity.DataBind();
                grdgratuity.Visible = true;
                btn_Set.Visible = true;
            }

            if (cbgross.Checked == true)
            {
                DataTable dtl = new DataTable();
                DataRow dtrow = null;
                dtl.Columns.Add("gratuityCal", typeof(string));
                dtrow = dtl.NewRow();
                dtrow["gratuityCal"] = Convert.ToString("Gross Pay");
                dtl.Rows.Add(dtrow);
                div1.Visible = true;
                grdgratuity.DataSource = dtl;
                grdgratuity.DataBind();
                grdgratuity.Visible = true;
                btn_Set.Visible = true;
            }

           
        }
        for (int l = 0; l < grdgratuity.Rows.Count; l++)
        {
            foreach (GridViewRow row in grdgratuity.Rows)
            {
                foreach (TableCell cell in row.Cells)
                {
                    grdgratuity.Rows[l].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    

                }
            }
        }

    }
    protected void grdgratuity_RowDataBound(object sende, GridViewRowEventArgs e)
    {
    }
    protected void cbgross_changed(object sender, EventArgs e)
    {
        if (cbgross.Checked == true)
        {
            txtcommon.Text = "--Select--";
            txt_allow.Text = "--Select--";
            txtcommon.Enabled = false;
            txt_allow.Enabled = false;
            grdgratuity.Visible = false;
            btn_Set.Visible = false;
        }
        if (cbgross.Checked == false)
        {
            txtcommon.Enabled = true;
            txt_allow.Enabled = true;
            allowance();
            common();
            grdgratuity.Visible = false;
            btn_Set.Visible = false;
        
        }
    }
    protected void btn_setclick(object sender, EventArgs e)
    {
        try
        {
            string getallval = "";
            foreach (GridViewRow gvrow in grdgratuity.Rows)
            {
                int RowCnt = Convert.ToInt32(gvrow.RowIndex);
                Label gettext = (Label)grdgratuity.Rows[RowCnt].FindControl("gratuity");
                string value = Convert.ToString(gettext.Text);


                if (getallval.Trim() == "")
                    getallval = value;
                else
                    getallval = getallval + "+" + value;
            }
            if (getallval.Trim() != "")
            {
               
                string Linkname = "gratuity";
                string insquery = " if exists(select * from New_InsSettings where LinkName='" + Linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "') update New_InsSettings set LinkValue='" + getallval + "' where LinkName='" + Linkname + "' and user_code='" + usercode + "' and college_code='" + collegecode1 + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + Linkname + "','" + getallval + "','" + usercode + "','" + collegecode1 + "')";
                int inscount = d2.update_method_wo_parameter(insquery, "Text");
                if (inscount > 0)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                }
            }

        }
        catch (Exception ex)
        { 
        
        }
    }
}