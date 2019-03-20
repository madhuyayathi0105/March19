using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Web.Services;
using System.Drawing;

public partial class HM_MessMaster : System.Web.UI.Page
{
    public object sender { get; set; }
    public EventArgs e { get; set; }

    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable yearhash = new Hashtable();
    bool check = false;
    DateTime dt = new DateTime();
    DateTime dt1 = new DateTime();
    int commcount = 0;
    int i = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lbl_alert.ForeColor = Color.Red;
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
            if (!IsPostBack)
            {
                bindmessmaster();
                //bindhostelname();
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Visible = false;
                btn_go_Click(sender, e);

            }
            lblvalidation1.Visible = false;
            errorlable.Visible = false;
        }
        catch
        {
            Response.Redirect("~/Default.aspx");
        }
    }
    protected void ddl_messmaster_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        clrSpread();
    }
    protected void txt_search_OnTextChanged(object sender, EventArgs e)
    {
        clrSpread();
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        btn_go_Click(sender, e);
    }

    [WebMethod]
    public static string CheckUserName(string MessName)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string user_name = MessName;
            if (user_name.Trim() != "" && user_name != null)
            {
                string query = dd.GetFunction("select distinct MessName,MessMasterPK from HM_MessMaster  where MessName ='" + user_name + "'");
                if (query.Trim() == "" || query == null || query == "0" || query == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            //returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    public void bindmessmaster()
    {
        try
        {
            ddl_messmaster.Items.Clear();
            string selectQuery = "select MessMasterPK,MessName,MessAcr from HM_MessMaster order by MessMasterPK asc";//where CollegeCode=" + collegecode1 + " 
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_messmaster.DataSource = ds;
                ddl_messmaster.DataTextField = "MessName";
                ddl_messmaster.DataValueField = "MessMasterPK";
                ddl_messmaster.DataBind();
            }
            ddl_messmaster.Items.Insert(0, "All");
        }
        catch
        {
            ddl_messmaster.Items.Clear();
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct MessName from HM_MessMaster WHERE MessName like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    public void clear()
    {
        try
        {
            txt_messname.Text = "";
            txt_messacr.Text = "";
            txt_startyear.Text = "";
        }
        catch
        {

        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            clear();
            btn_save.Visible = true;
            btn_update.Visible = false;
            btn_delete.Visible = false;
            poperrjs.Visible = true;
        }
        catch { }

    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void txtyear_Onchange(object sender, EventArgs e)
    {
        try
        {
            int year2 = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
            int txtyear = Convert.ToInt32(txt_startyear.Text);

            int oldyear = Convert.ToInt32(oldyeartxt.Text);
            if (oldyear <= txtyear && year2 >= txtyear)
            {

            }
            else
            {
                txt_startyear.Text = "";
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Enter Valid Year";
            }
        }
        catch
        {
            txt_startyear.Text = "";
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Enter Valid Year";
        }
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string messname = Convert.ToString(txt_messname.Text.First().ToString().ToUpper() + txt_messname.Text.Substring(1));
            messname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(messname);
            string messacr = Convert.ToString(txt_messacr.Text);
            string year = "";
            year = Convert.ToString(txt_startyear.Text);

            collegecode = collegecode1;
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            if (messname.Trim() != "" && messacr.Trim() != "")
            {
                string insertstorequery = "insert into HM_MessMaster (MessAcr,MessName,MessStartYear) values ('" + messacr.ToUpper() + "','" + messname + "','" + year + "')";
                int inster = d2.update_method_wo_parameter(insertstorequery, "Text");
                if (inster != 0)
                {
                    bindmessmaster();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Saved Successfully";
                    btn_go_Click(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Problem While Saving. Try Later";
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            string selectquery = "";
            collegecode = collegecode1;
            DataView dv = new DataView();

            if (txt_search.Text.Trim() != "")
            {
                selectquery = "select m.MessAcr,m.MessMasterPK,m.MessName from HM_MessMaster m where m.MessName='" + txt_search.Text + "' ";//and m.CollegeCode =" + collegecode + "

                selectquery = selectquery + "  select distinct m.MessAcr,m.MessMasterPK,m.MessName,m.MessStartYear  from HM_MessMaster m where  m.MessName='" + txt_search.Text + "'  ";//m.CollegeCode  ='" + collegecode + "'  and

            }
            else
            {
                if (ddl_messmaster.SelectedItem.Text != "All")
                {
                    selectquery = "select m.MessAcr,m.MessMasterPK,m.MessName from HM_MessMaster m where  m.MessMasterPK in (" + ddl_messmaster.SelectedItem.Value + ") ";//and m.CollegeCode =" + collegecode + "

                    selectquery = selectquery + "  select distinct m.MessAcr,m.MessMasterPK,m.MessName,m.MessStartYear  from HM_MessMaster m where m.MessMasterPK='" + ddl_messmaster.SelectedItem.Value + "'  ";// m.CollegeCode  ='" + collegecode + "'  and

                }
                else
                {
                    selectquery = "select m.MessAcr,m.MessMasterPK,m.MessName from HM_MessMaster m ";//where  m.CollegeCode =" + collegecode + "";
                    selectquery = selectquery + "  select distinct m.MessAcr,m.MessMasterPK,m.MessName,m.MessStartYear  from HM_MessMaster m ";//where m.CollegeCode  ='" + collegecode + "' ";
                }
            }

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[1].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].Columns.Count = 4;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[0].Width = 50;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Mess Acronym";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Mess Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[2].Width = 180;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Start Year";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[3].Width = 100;

                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Columns[4].Width = 200;

                for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                {
                    //string concate = "";
                    //string concatecode = "";
                    //ds.Tables[0].DefaultView.RowFilter = "MessID ='" + Convert.ToString(ds.Tables[1].Rows[row]["MessID"]) + "'";
                    //dv = ds.Tables[0].DefaultView;
                    //if (dv.Count > 0)
                    //{
                    //    for (int i = 0; i < dv.Count; i++)
                    //    {
                    //        if (concate == "")
                    //        {
                    //            concate = Convert.ToString(dv[i]["Hostel_Name"]);
                    //            concatecode = Convert.ToString(dv[i]["Hostel_code"]);
                    //        }
                    //        else
                    //        {
                    //            concate = concate + " , " + Convert.ToString(dv[i]["Hostel_Name"]);
                    //            concatecode = concatecode + " , " + Convert.ToString(dv[i]["Hostel_code"]);
                    //        }
                    //    }
                    //}
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[1].Rows[row]["MessAcr"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[1].Rows[row]["MessMasterPK"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[1].Rows[row]["MessName"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[1].Rows[row]["MessStartYear"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(concate);
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(concatecode);
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                Fpspread1.Visible = true;
                rptprint.Visible = true;
                div1.Visible = true;
                errorlable.Visible = false;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            }
            else
            {
                div1.Visible = false;
                errorlable.Visible = true;
                errorlable.Text = "No Records Found";
                Fpspread1.Visible = false;
                rptprint.Visible = false;
            }

            txt_search.Text = "";
        }
        catch
        {

        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Mess Master Report";
            string pagename = "HM_messmaster.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }

    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
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
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                poperrjs.Visible = true;
                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                collegecode = collegecode1;
                Session["MessCode"] = null;
                if (activerow.Trim() != "")
                {
                    string messacr = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    string messname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    string messcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                    Session["MessCode"] = Convert.ToString(messcode);
                    string year1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    txt_messname.Text = Convert.ToString(messname);
                    txt_messacr.Text = Convert.ToString(messacr);
                    txt_startyear.Text = year1;
                    btn_save.Visible = false;
                    btn_update.Visible = true;
                    btn_delete.Visible = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string messname = Convert.ToString(txt_messname.Text.First().ToString().ToUpper() + txt_messname.Text.Substring(1));
            string messacr = Convert.ToString(txt_messacr.Text);
            string year1 = "";

            year1 = Convert.ToString(txt_startyear.Text);

            collegecode = collegecode1;
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            if (messname.Trim() != "" && messacr.Trim() != "")
            {
                int stroecode = Convert.ToInt32(Session["MessCode"]);
                string insertstorequery = "update HM_MessMaster set MessName='" + messname + "',MessAcr='" + messacr + "',MessStartYear='" + year1 + "' where MessMasterPK='" + Convert.ToString(Session["MessCode"]) + "' ";
                int inster = d2.update_method_wo_parameter(insertstorequery, "Text");
                if (inster != 0)
                {
                    string laststorecode = d2.GetFunction("select MessMasterPK from HM_MessMaster  order by MessMasterPK desc");
                    bindmessmaster();
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Updated Successfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Problem While Updating. Try Later";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Fill all the Values";
            }
        }
        catch
        {

        }
    }
    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            int stroecode = Convert.ToInt32(Session["MessCode"]);
            string delete = " delete from HM_MessMaster where MessMasterPK ='" + stroecode + "'";
            int upnow = d2.update_method_wo_parameter(delete, "Text");
            if (upnow != 0)
            {
                bindmessmaster();
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
                btn_go_Click(sender, e);
            }
        }
        catch
        {
        }
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
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        if (btn_save.Visible)
        {
            btn_addnew_Click(sender, e);
        }
        else
        {
            poperrjs.Visible = false;
            btn_go_Click(sender, e);
        }
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        poperrjs.Visible = true;
    }
    public void clrSpread()
    {
        Printcontrol.Visible = false;
        Fpspread1.Visible = false;
        rptprint.Visible = false;
        div1.Visible = false;
    }
}