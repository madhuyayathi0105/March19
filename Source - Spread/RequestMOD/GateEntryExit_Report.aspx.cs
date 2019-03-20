using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;
public partial class GateEntryExit_Report : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Hashtable ht = new Hashtable();
    Hashtable htable = new Hashtable();
    static Hashtable hasvalue = new Hashtable();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string gatepassperimissiontype = string.Empty;
    string group_user = string.Empty;
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
            //    if (!Request.FilePath.Contains("ReqM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Request"].Trim();
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

            if (!Request.FilePath.Contains("RequestHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/RequestMOD/RequestHome.aspx");
                    return;
                }
            }

            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]);
            usercode = Convert.ToString(Session["usercode"]);
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                gatepassperimissiontype = d2.GetFunction("select value from Master_Settings where settings='Gatepass Request Type'");//  and usercode='"+UserCode+"'");
                bindhostel();
                bindcollege();
                BindBatch();
                binddegree();
                bindbranch();
                loadhour();
                loadmin();
                loadstatus();
                loadappstatus();
                loadenter();
                loadstutype();
                timevalue();
                txt_roll.Visible = true;
                txtfrmdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodt.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Gate Entry Exit Report";
            string pagename = "GateEntryExit_Report.aspx";

            string hostelname = "";
            if (ddlhostel.Items.Count > 0)
                hostelname = Convert.ToString(ddlhostel.SelectedItem.Text);
         
            degreedetails =degreedetails + '@' + "Hostel Name: " + hostelname+"";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRoll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        string getrollq = "select distinct top(10) Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%'";
        roll = ws.Getname(getrollq);
        return roll;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetReg(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        string getrollq = "select distinct top(10) Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%'";
        roll = ws.Getname(getrollq);
        return roll;
    }
    protected void ddl_searchby_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchby.SelectedItem.Text == "Roll No" && ddl_searchby.SelectedIndex == 0)
        {
            txt_roll.Visible = true;
            txt_reg.Visible = false;
        }
        if (ddl_searchby.SelectedItem.Text == "Reg No" && ddl_searchby.SelectedIndex == 1)
        {
            txt_reg.Visible = true;
            txt_roll.Visible = false;
        }

    }
    protected void ddl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
        }
        catch
        {
        }
    }
    protected void chkdtfrm_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdtfrm.Checked == true)
            {
                txtfrmdt.Enabled = true;
                txttodt.Enabled = true;
            }
            else
            {
                txtfrmdt.Enabled = false;
                txttodt.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected void cbtimefrm_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbtimefrm.Checked == true)
            {
                ddlhourreq.Enabled = true;
                ddlminreq.Enabled = true;
                ddlsessionreq.Enabled = true;
                ddlendhourreq.Enabled = true;
                ddlendminreq.Enabled = true;
                ddlenssessionreq.Enabled = true;
            }
            else
            {
                ddlhourreq.Enabled = false;
                ddlminreq.Enabled = false;
                ddlsessionreq.Enabled = false;
                ddlendhourreq.Enabled = false;
                ddlendminreq.Enabled = false;
                ddlenssessionreq.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected void chk_batch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chk_batch.Checked == true)
            {
                txt_batch.Enabled = true;
                txt_degree.Enabled = true;
                txt_branch.Enabled = true;
            }
            else
            {
                txt_batch.Enabled = false;
                txt_degree.Enabled = false;
                txt_branch.Enabled = false;
            }
        }
        catch
        {
        }
    }
    protected void chk_entry_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_entry.Checked == true || chk_exit.Checked == true)
        {
            chkdtfrm.Enabled = true;
            cbtimefrm.Enabled = true;
        }
        else
        {
            chkdtfrm.Enabled = false;
            cbtimefrm.Enabled = false;
        }
    }
    protected void chk_exit_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_entry.Checked == true || chk_exit.Checked == true)
        {
            chkdtfrm.Enabled = true;
            cbtimefrm.Enabled = true;
        }
        else
        {
            chkdtfrm.Enabled = false;
            cbtimefrm.Enabled = false;
        }
    }
    protected void cb_col_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_col.Text = "--Select--";
            if (cb_col.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_col.Items.Count; i++)
                {
                    cbl_col.Items[i].Selected = true;
                }
                txt_col.Text = "College(" + (cbl_col.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_col.Items.Count; i++)
                {
                    cbl_col.Items[i].Selected = false;
                }
                txt_col.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_col_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_col.Checked = false;
            txt_col.Text = "--Select--";
            for (int i = 0; i < cbl_col.Items.Count; i++)
            {
                if (cbl_col.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_col.Items.Count)
                {
                    cb_col.Checked = true;
                }
                txt_col.Text = "College(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            bindbranch();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_branch.Text = "--Select--";
            if (cb_branch.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_branch.Checked = false;
            txt_branch.Text = "--Select--";
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_status_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_status.Text = "--Select--";
            if (cb_status.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = true;
                }
                txt_status.Text = "Status(" + (cbl_status.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = false;
                }
                txt_status.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_status.Checked = false;
            txt_status.Text = "--Select--";
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                if (cbl_status.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_status.Items.Count)
                {
                    cb_status.Checked = true;
                }
                txt_status.Text = "Status(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_appstatus_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_appstatus.Text = "--Select--";
            if (cb_appstatus.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_appstatus.Items.Count; i++)
                {
                    cbl_appstatus.Items[i].Selected = true;
                }
                txt_appstatus.Text = "Status(" + (cbl_appstatus.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_appstatus.Items.Count; i++)
                {
                    cbl_appstatus.Items[i].Selected = false;
                }
                txt_appstatus.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_appstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_appstatus.Checked = false;
            txt_appstatus.Text = "--Select--";
            for (int i = 0; i < cbl_appstatus.Items.Count; i++)
            {
                if (cbl_appstatus.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_appstatus.Items.Count)
                {
                    cb_appstatus.Checked = true;
                }
                txt_appstatus.Text = "Status(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_enter_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_enter.Text = "--Select--";
            if (cb_enter.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_enter.Items.Count; i++)
                {
                    cbl_enter.Items[i].Selected = true;
                }
                txt_enter.Text = "Entered(" + (cbl_enter.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_enter.Items.Count; i++)
                {
                    cbl_enter.Items[i].Selected = false;
                }
                txt_enter.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_enter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_enter.Checked = false;
            txt_enter.Text = "--Select--";
            for (int i = 0; i < cbl_enter.Items.Count; i++)
            {
                if (cbl_enter.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_enter.Items.Count)
                {
                    cb_enter.Checked = true;
                }
                txt_enter.Text = "Entered(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();
                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    cblcolumnorder.Items[0].Enabled = false;
                }
                tborder.Text = "";
                tborder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    if (tborder.Text == "")
                    {
                        ItemList.Add("Roll No");
                       
                    }

                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();
                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_studtype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_studtype.Text = "--Select--";
            if (cb_studtype.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = true;
                }
                txt_studtype.Text = "Student Type(" + (cbl_studtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = false;
                }
                txt_studtype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_studtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cb_studtype.Checked = false;
            txt_studtype.Text = "--Select--";
            for (int i = 0; i < cbl_studtype.Items.Count; i++)
            {
                if (cbl_studtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_studtype.Items.Count)
                {
                    cb_studtype.Checked = true;
                }
                txt_studtype.Text = "Student Type(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            string nowdesd = Convert.ToString(DateTime.Now.ToString("H:mm:tt"));
            string collegeid = "";
            string stud_type = "";
            string inandout = "";
            string appstatus = "";
            string batchyear = "";
            string degreecode = "";
            string branchcode = "";
            string getentry = "";
            string getexit = "";
            string getlate = "";
            ItemList.Clear();
            int count = 0;
            DateTime dtfrm = new DateTime();
            DateTime dtto = new DateTime();
            if (chkdtfrm.Checked == true)
            {
                string frmdt = txtfrmdt.Text;
                string[] splfrm = frmdt.Split('/');
                string newfrm = Convert.ToString(splfrm[1] + "/" + splfrm[0] + "/" + splfrm[2]);
                dtfrm = Convert.ToDateTime(newfrm);
                string todt = txttodt.Text;
                string[] splto = todt.Split('/');
                string newto = Convert.ToString(splto[1] + "/" + splto[0] + "/" + splto[2]);
                dtto = Convert.ToDateTime(newto);
            }
            if (cbtimefrm.Checked == true)
            {
                getentry = ddlhourreq.SelectedItem.Text + ":" + ddlminreq.SelectedItem.Text + " " + ddlsessionreq.SelectedItem.Text;
                getexit = ddlendhourreq.SelectedItem.Text + ":" + ddlendminreq.SelectedItem.Text + " " + ddlenssessionreq.SelectedItem.Text;
            }
            if (cbl_col.Items.Count > 0)
            {
                for (int i = 0; i < cbl_col.Items.Count; i++)
                {
                    if (cbl_col.Items[i].Selected == true)
                    {
                        if (collegeid == "")
                        {
                            collegeid = Convert.ToString(cbl_col.Items[i].Value);
                        }
                        else
                        {
                            collegeid = collegeid + "'" + "," + "'" + Convert.ToString(cbl_col.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_studtype.Items.Count > 0)
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    if (cbl_studtype.Items[i].Selected == true)
                    {
                        if (stud_type == "")
                        {
                            stud_type = Convert.ToString(cbl_studtype.Items[i].Text);
                        }
                        else
                        {
                            stud_type = stud_type + "'" + "," + "'" + Convert.ToString(cbl_studtype.Items[i].Text);
                        }
                    }
                }
            }
            if (cbl_status.Items.Count > 0)
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    if (cbl_status.Items[i].Selected == true)
                    {
                        if (inandout == "")
                        {
                            inandout = Convert.ToString(cbl_status.Items[i].Value);
                        }
                        else
                        {
                            inandout = inandout + "'" + "," + "'" + Convert.ToString(cbl_status.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_appstatus.Items.Count > 0)
            {
                for (int i = 0; i < cbl_appstatus.Items.Count; i++)
                {
                    if (cbl_appstatus.Items[i].Selected == true)
                    {
                        if (appstatus == "")
                        {
                            appstatus = Convert.ToString(cbl_appstatus.Items[i].Value);
                        }
                        else
                        {
                            appstatus = appstatus + "'" + "," + "'" + Convert.ToString(cbl_appstatus.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (batchyear == "")
                        {
                            batchyear = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            batchyear = batchyear + "'" + "," + "'" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (degreecode == "")
                        {
                            degreecode = Convert.ToString(cbl_degree.Items[i].Value);
                        }
                        else
                        {
                            degreecode = degreecode + "'" + "," + "'" + Convert.ToString(cbl_degree.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_branch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        if (branchcode == "")
                        {
                            branchcode = Convert.ToString(cbl_branch.Items[i].Value);
                        }
                        else
                        {
                            branchcode = branchcode + "'" + "," + "'" + Convert.ToString(cbl_branch.Items[i].Value);
                        }
                    }
                }
            }
            if (cbl_enter.Items.Count > 0)
            {
                for (int i = 0; i < cbl_enter.Items.Count-1; i++)
                {
                    if (cbl_enter.Items[i].Selected == true)
                    {
                        if (getlate == "")
                        {
                            getlate = Convert.ToString(cbl_enter.Items[i].Value);
                        }
                        else
                        {
                            getlate = getlate + "'" + "," + "'" + Convert.ToString(cbl_enter.Items[i].Value);
                        }
                    }
                }
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                if (cblcolumnorder.Items[i].Selected == true)
                {
                    ht.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                    string colvalue = cblcolumnorder.Items[i].Text;
                    if (ItemList.Contains(colvalue) == false)
                    {
                        ItemList.Add(cblcolumnorder.Items[i].Text);
                    }
                    tborder.Text = "";
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        tborder.Text = tborder.Text + "  " + ItemList[j].ToString();
                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")";
                    }
                }
                else
                {
                    ItemList.Remove(cblcolumnorder.Items[i].Text);
                }
                cblcolumnorder.Items[0].Enabled = false;
            }
            if (ItemList.Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                    ht.Add(cblcolumnorder.Items[i].Text, cblcolumnorder.Items[i].Value);
                    string colvalue = cblcolumnorder.Items[i].Text;
                    if (ItemList.Contains(colvalue) == false)
                    {
                        ItemList.Add(cblcolumnorder.Items[i].Text);
                    }
                    tborder.Text = "";
                    for (int j = 0; j < ItemList.Count; j++)
                    {
                        tborder.Text = tborder.Text + "  " + ItemList[j].ToString();
                        tborder.Text = tborder.Text + "(" + (j + 1).ToString() + ")  ";
                    }
                }
            }
          
            //magesh 26.5.18
            string selectquery = "select distinct g.App_No,convert(varchar,GateReqExitDate,103) as GateReqExitDate,rq.GateReqExitTime,c.Course_Name as HostelName,(c.Course_Name +'-'+dt.Dept_Name+'-'+CONVERT(varchar(10), r.Current_Semester)+'-'+Sections) as Degree ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype,CASE WHEN ReqAppStatus=1 THEN 'Approved' when ReqAppStatus=0 then 'Un Approved' End ReqAppStatus,case when stu_Relationship=1 then a.parent_name when stu_Relationship='2' then a.mother when stu_Relationship='3' then  a.guardian_name end stu_Relationship,CASE when gatetype = 1  and ExpectedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'   and ExpectedTime<'" + Convert.ToString(DateTime.Now.ToString("hh:mm:tt")) + "'   THEN 'Not Entered' when gatetype = 1  and ExpectedDate<'" + DateTime.Now.ToString("MM/dd/yyyy") + "'      THEN 'Not Entered' WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',r.Roll_No,r.Stud_Name,g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',hs.id from RQ_Requisition rq,Degree d,Department dt,Course c,GateEntryExit g,applyn a,Registration r left join HT_HostelRegistration hs on r.App_No=hs.APP_No left join HM_HostelMaster hd on hs.HostelMasterFK=hd.HostelMasterPK where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  and a.app_no=r.App_No and  rq.GateReqExitDate =g.GatepassExitdate and g.GateMemType=1  and rq.RequisitionPK=g.RequestFk and g.App_No=r.App_No ";

          


            //string selectquery = "select distinct g.App_No,convert(varchar,GateReqExitDate,103) as GateReqExitDate,rq.GateReqExitTime,hd.HostelName,(c.Course_Name +'-'+dt.Dept_Name+'-'+CONVERT(varchar(10), r.Current_Semester)+'-'+Sections) as Degree ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype,CASE WHEN ReqAppStatus=1 THEN 'Approved' when ReqAppStatus=0 then 'Un Approved' End ReqAppStatus,CASE WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',r.Roll_No,r.Stud_Name,g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate' from RQ_Requisition rq,HT_HostelRegistration hs,HM_HostelMaster hd,Degree d,Department dt,Course c,GateEntryExit g,applyn a,Registration r where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and hd.HostelMasterPK=hs.HostelMasterFK and a.app_no=r.App_No and hs.APP_No=r.App_No and rq.GateReqExitDate =g.GatepassExitdate and g.GateMemType=1 and hd.HostelMasterPK='" + ddlhostel.SelectedItem.Value + "' and rq.RequisitionPK=g.RequestFk and g.App_No=r.App_No ";// and g.App_No=a.app_no and rq.ReqAppNo=g.App_No and rq.ReqAppNo=r.App_No and rq.ReqAppNo=a.App_No
            //barath without request 08.11.2016

            // string woreqfilter = " select distinct CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' End ReqAppStatus, g.GatepassentryTime as GateReqExitTime,g.App_No,convert(varchar,GatepassExitdate,103) as GateReqExitDate,convert ( varchar(10),gatepassexitdate,103)as gatepassexitdate,hd.HostelName,(c.Course_Name +'-'+dt.Dept_Name+'-'+CONVERT(varchar(10), r.Current_Semester)+'-'+Sections) as Degree ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype,CASE WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as 'GatePassDate',r.Roll_No,r.Stud_Name,g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate' from HT_HostelRegistration hs,HM_HostelMaster hd,Degree d,Department dt,Course c,GateEntryExit g,applyn a,Registration r where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and hd.HostelMasterPK=hs.HostelMasterFK and g.App_No=a.app_no and a.app_no=r.App_No and hs.APP_No=r.App_No and g.GateMemType=1 and hd.HostelMasterPK='" + Convert.ToString(ddlhostel.SelectedItem.Value) + "' ";
           
            string woreqfilter = " select distinct CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' End ReqAppStatus, g.GatepassentryTime as GateReqExitTime,g.App_No,convert(varchar,GatepassExitdate,103) as GateReqExitDate,convert ( varchar(10),gatepassexitdate,103)as gatepassexitdate,hd.HostelName,(c.Course_Name +'-'+dt.Dept_Name+'-'+CONVERT(varchar(10), r.Current_Semester)+'-'+Sections) as Degree ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype,case when stu_Relationship=1 then a.parent_name when stu_Relationship='2' then a.mother when stu_Relationship='3' then  a.guardian_name end stu_Relationship,CASE when gatetype = 1  and ExpectedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'   and ExpectedTime<'" + Convert.ToString(DateTime.Now.ToString("hh:mm:tt")) + "'   THEN 'Not Entered' when gatetype = 1  and ExpectedDate<'" + DateTime.Now.ToString("MM/dd/yyyy") + "'      THEN 'Not Entered' WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as'GatePassDate',r.Roll_No,r.Stud_Name,g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',hs.id from Degree d,Department dt,Course c,GateEntryExit g,applyn a,Registration r left join HT_HostelRegistration hs on r.App_No=hs.APP_No left join HM_HostelMaster hd on hs.HostelMasterFK=hd.HostelMasterPK where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and g.App_No=a.app_no and a.app_no=r.App_No and g.GateMemType=1 ";
            if (Chktutor.Checked == true)
            {
                string staffapp = d2.GetFunction("select appl_id  from staff_appl_master a, staffmaster s where a.appl_no=s.appl_no and staff_code='" + txttutor.Text + "'");
                woreqfilter = " select distinct CASE WHEN isapproval=1 THEN 'Approved' when isapproval=0 then 'Un Approved' End ReqAppStatus, g.GatepassentryTime as GateReqExitTime,g.App_No,convert(varchar,GatepassExitdate,103) as GateReqExitDate,convert ( varchar(10),gatepassexitdate,103)as gatepassexitdate,hd.HostelName,(c.Course_Name +'-'+dt.Dept_Name+'-'+CONVERT(varchar(10), r.Current_Semester)+'-'+Sections) as Degree ,convert(varchar,GatepassExitdate,103) as GatepassExitdate,g.GatepassExittime,convert(varchar,GatepassEntrydate,103) as GatepassEntrydate,g.GatepassEntrytime,CASE WHEN gatetype = 1 THEN 'Out' when gatetype=0  then 'In' END gatetype,case when stu_Relationship=1 then a.parent_name when stu_Relationship='2' then a.mother when stu_Relationship='3' then  a.guardian_name end stu_Relationship,CASE when gatetype = 1  and ExpectedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'   and ExpectedTime<'" + Convert.ToString(DateTime.Now.ToString("hh:mm:tt")) + "'   THEN 'Not Entered' when gatetype = 1  and ExpectedDate<'" + DateTime.Now.ToString("MM/dd/yyyy") + "'      THEN 'Not Entered' WHEN islate=0 THEN 'On Time' WHEN islate=1 THEN 'Late Time' End islate,convert(varchar,GatePassDate,103) as'GatePassDate',r.Roll_No,r.Stud_Name,g.Purpose,g.GatePassTime,g.ExpectedTime,convert(varchar,ExpectedDate,103) as 'ExpectedDate',hs.id from Degree d,Department dt,Course c,GateEntryExit g,applyn a,Registration r left join HT_HostelRegistration hs on r.App_No=hs.APP_No left join HM_HostelMaster hd on hs.HostelMasterFK=hd.HostelMasterPK where d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and g.App_No=a.app_no and a.app_no=r.App_No and g.GateMemType=1  and g.app_no in(select APP_no from CO_StudentTutor  where StaffMasterFK='" + staffapp + "')";
            }
            
            string studenttype = string.Empty;
            string HostelFk = string.Empty;
            if (stud_type.Trim() != "")
            {
                if (cbl_studtype.Items[0].Selected == true)
                {
                    studenttype = "'" + cbl_studtype.Items[0].Text + "'";
                    HostelFk = "''";
                }
                if (cbl_studtype.Items[1].Selected == true)
                {
                    if (!string.IsNullOrEmpty(studenttype))
                        studenttype += ",";
                    if (!string.IsNullOrEmpty(HostelFk))
                        HostelFk += ",";
                    HostelFk += "'" + Convert.ToString(ddlhostel.SelectedItem.Value) + "'";
                    studenttype += "'" + cbl_studtype.Items[1].Text + "'";
                }
                selectquery += " and r.Stud_type in(" + studenttype + ")";
                woreqfilter += " and r.Stud_type in(" + studenttype + ")";
            }
            woreqfilter += " and isnull(hd.HostelMasterPK,'') in(" + HostelFk + ")";
            if (collegeid.Trim() != "")
            {
                selectquery = selectquery + " and CONVERT(Varchar(10),g.College_Code) in('" + collegeid + "')";
            }
            if (inandout.Trim() != "")
            {
                selectquery += " and g.GateType in('" + inandout + "')";
                woreqfilter += " and g.GateType in('" + inandout + "')";
            }
            if (appstatus.Trim() != "")
            {
                selectquery = selectquery + "  and rq.ReqAppStatus in('" + appstatus + "')";
            }
            if (getlate.Trim() != "")
            {
               
                selectquery = selectquery + "  and g.islate in('" + getlate + "')";
                woreqfilter += "  and g.islate in('" + getlate + "')";
            }
            else
            {
                string quers = string.Empty;
                if (cbl_enter.Items[2].Selected == true)
                {
                    quers = "and ((gatetype = 1  and ExpectedDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "'   and ExpectedTime<'" + Convert.ToString(DateTime.Now.ToString("hh:mm:tt")) + "')  or   (gatetype = 1  and ExpectedDate<'" + DateTime.Now.ToString("MM/dd/yyyy") + "'))";
                }
                else
                {

                }
            }
            if (txt_roll.Text.Trim() != "")
            {
                if (chk_batch.Checked == true)
                {
                    selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.Branch_code in('" + branchcode + "')";
                    woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.Branch_code in('" + branchcode + "')";
                }
                if (chk_entry.Checked == true && chk_exit.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                }
                else if (chk_entry.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) <='" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += "  and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) <='" + getexit + "'";
                    }
                    //else
                    //{
                    //    selectquery = selectquery + " and CONVERT(Varchar(10),g.College_Code) in('" + collegeid + "') and g.GateType in('" + inandout + "') and rq.ReqAppStatus in('" + appstatus + "')  and r.Roll_No in('" + txt_roll.Text.Trim() + "') and hd.HostelMasterPK='" + ddlhostel.SelectedItem.Value + "'";
                    //}
                }
                else if (chk_exit.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassExittime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += " and r.Roll_No in('" + txt_roll.Text.Trim() + "') and CONVERT(nvarchar(100),GatepassExittime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                }
                else
                {
                    selectquery = selectquery + " and r.Roll_No in('" + txt_roll.Text.Trim() + "')";
                    woreqfilter += "  and r.Roll_No in('" + txt_roll.Text.Trim() + "')";
                }
            }
            else
            {
                if (chk_batch.Checked == true)
                {
                    selectquery = selectquery + " and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.Branch_code in('" + branchcode + "')";
                    woreqfilter += "  and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.Branch_code in('" + branchcode + "')";
                }
                if (chk_entry.Checked == true && chk_exit.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += "  and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += " and g.GatepassEntrydate='" + dtfrm.ToString("MM/dd/yyyy") + "' and g.GatepassExitdate='" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                        woreqfilter += " and CONVERT(nvarchar(100),GatepassEntrytime ,100) >= '" + getentry + "' and CONVERT(nvarchar(100),GatepassExittime ,100) <='" + getexit + "'";
                    }
                    //else
                    //{
                    //    selectquery = selectquery + " and CONVERT(Varchar(10),g.College_Code) in('" + collegeid + "') and g.GateType in('" + inandout + "') and rq.ReqAppStatus in('" + appstatus + "')  and r.Roll_No in('" + txt_roll.Text.Trim() + "') and hd.HostelMasterPK='" + ddlhostel.SelectedItem.Value + "'";
                    //}
                }
                else if (chk_entry.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentry + "' and '" + getexit + "'";
                        woreqfilter += " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentry + "' and '" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += " and g.GatepassEntrydate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentry + "' and '" + getexit + "'";
                        woreqfilter += "  and CONVERT(nvarchar(100),GatepassEntrytime ,100) between '" + getentry + "' and '" + getexit + "'";
                    }
                    //else
                    //{
                    //    selectquery = selectquery + " and CONVERT(Varchar(10),g.College_Code) in('" + collegeid + "') and g.GateType in('" + inandout + "') and rq.ReqAppStatus in('" + appstatus + "')  and r.Roll_No in('" + txt_roll.Text.Trim() + "') and hd.HostelMasterPK='" + ddlhostel.SelectedItem.Value + "'";
                    //}
                }
                else if (chk_exit.Checked == true)
                {
                    if (chkdtfrm.Checked == true && cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentry + "' and '" + getexit + "'";
                        woreqfilter += " and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "' and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentry + "' and '" + getexit + "'";
                    }
                    else if (chkdtfrm.Checked == true)
                    {
                        selectquery = selectquery + " and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                        woreqfilter += "  and g.GatepassExitdate between '" + dtfrm.ToString("MM/dd/yyyy") + "' and '" + dtto.ToString("MM/dd/yyyy") + "'";
                    }
                    else if (cbtimefrm.Checked == true)
                    {
                        selectquery = selectquery + " and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentry + "' and '" + getexit + "'";
                        woreqfilter += "  and CONVERT(nvarchar(100),GatepassExittime ,100) between '" + getentry + "' and '" + getexit + "'";
                    }
                }
                //else
                //{
                //    selectquery = selectquery + " and CONVERT(Varchar(10),g.College_Code) in('" + collegeid + "') and g.GateType in('" + inandout + "') and rq.ReqAppStatus in('" + appstatus + "') and hd.HostelMasterPK='" + ddlhostel.SelectedItem.Value + "'";
                //}
            }
            ds.Clear();
            if (selectquery.Trim() != "")
            {
                string qurys = "";
                if (!string.IsNullOrEmpty(group_user) && group_user.Trim() != "0")
                {
                    if (group_user.Contains(';'))
                    {
                        string[] group_semi = group_user.Split(';');
                        group_user = group_semi[0].ToString();
                    }
                    qurys = " and group_code='" + group_user + "'";
                }
                else if (!string.IsNullOrEmpty(usercode) && usercode.Trim() != "0")
                    qurys = " and usercode='" + usercode + "'";//magesh 11.1.18
                gatepassperimissiontype = d2.GetFunction("select value from Master_Settings where settings='Gatepass Request Type' " + qurys + "");//  and usercode='"+UserCode+"'");
                if (gatepassperimissiontype.Trim() != "0")
                {
                    selectquery = woreqfilter;
                }
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = false;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    for (int i = 0; i < ItemList.Count; i++)
                    {
                        string value1 = ItemList[i].ToString();
                        int a = value1.Length;
                        Fpspread1.Sheets[0].ColumnCount++;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Text = ItemList[i].ToString();
                        Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[Fpspread1.Sheets[0].ColumnHeader.RowCount - 1, Fpspread1.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[Fpspread1.Sheets[0].ColumnCount - 1].Locked = true;
                    }
                   
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string cun = "0";
                        if (cbl_enter.Items[2].Selected == true && cbl_enter.Items[0].Selected == true && cbl_enter.Items[1].Selected==true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Not Entered" || Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "On Time" ||   Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Late Time")
                            {
                                cun = "1";
                            }
                        }
                        else  if (cbl_enter.Items[2].Selected == true && cbl_enter.Items[0].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Not Entered" || Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "On Time")
                            {
                                cun = "1";
                            }
                        }
                            else  if (cbl_enter.Items[2].Selected == true && cbl_enter.Items[1].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Not Entered" || Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Late Time")
                            {
                                cun = "1";
                            }
                        }
                        else if (cbl_enter.Items[1].Selected == true && cbl_enter.Items[0].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "On Time" || Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Late Time")
                            {
                                cun = "1";
                            }
                        }
                        else if (cbl_enter.Items[2].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Not Entered")
                            {
                                cun = "1";
                            }
                        }
                        else if (cbl_enter.Items[0].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "On Time")
                            {
                                cun = "1";
                            }
                        }
                        else if (cbl_enter.Items[1].Selected == true)
                        {
                            if (Convert.ToString(ds.Tables[0].Rows[i]["islate"]) == "Late Time")
                            {
                                cun = "1";
                            }
                        }
                        if(cun=="1")
                        {
                        Fpspread1.Sheets[0].RowCount++;
                        count++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Columns[0].Locked = true;
                        int c = 0;
                        for (int j = 0; j < ItemList.Count; j++)
                        {
                            string k = Convert.ToString(ItemList[j]);
                            string names = Convert.ToString(ht[k].ToString());
                            string l = Convert.ToString(ht[k].ToString()).ToUpperInvariant();
                            c++;
                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                            Fpspread1.Sheets[0].Columns[1].CellType = textcel_type;
                            if (names.Trim() == "islate")
                            {

                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Text = ds.Tables[0].Rows[i][l].ToString();
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Font.Size = FontUnit.Medium;
                            if (names == "Stud_Name" || names == "Purpose" || names == "HostelName")
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 250;
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, c].Column.Width = 100;
                            }
                        }
                        }
                    }
                    Fpspread1.Visible = true;
                    rptprint.Visible = true;
                    pcolumnorder.Visible = true;
                    pheaderfilter.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    if (CheckBox_column.Checked == true)
                    {
                        Fpspread1.Width = 950;
                        Fpspread1.Height = 350;
                    }
                    else
                    {
                        Fpspread1.Width = 700;
                        Fpspread1.Height = 350;
                    }
                    txt_roll.Text = "";
                    if (cblcolumnorder.Items.Count > 0)
                    {
                        for (int i = cblcolumnorder.Items.Count - 1; i >= 3; i--)
                        {
                            cblcolumnorder.Items[i].Selected = false;
                        }
                        CheckBox_column.Checked = false;
                        tborder.Text = "";
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    pcolumnorder.Visible = false;
                    pheaderfilter.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No records found";
                }
            }
        }
        catch
        {
        }
    }
    public void loadhour()
    {
        try
        {
            ddlhourreq.Items.Clear();
            ddlendhourreq.Items.Clear();
            for (int i = 1; i <= 12; i++)
            {
                ddlhourreq.Items.Add(Convert.ToString(i));
                ddlendhourreq.Items.Add(Convert.ToString(i));
                ddlhourreq.SelectedIndex = ddlhourreq.Items.Count - 1;
                ddlendhourreq.SelectedIndex = ddlendhourreq.Items.Count - 1;
            }
        }
        catch
        {
        }
    }
    public void loadmin()
    {
        try
        {
            ddlminreq.Items.Clear();
            ddlendminreq.Items.Clear();
            for (int i = 0; i <= 59; i++)
            {
                string val = Convert.ToString(i);
                if (val.Length == 1)
                {
                    val = "0" + val;
                }
                ddlminreq.Items.Add(val);
                ddlendminreq.Items.Add(val);
            }
        }
        catch
        {
        }
    }
    public void timevalue()
    {
        string time = DateTime.Now.ToString("HH:mm:ss");
        string hrr = "";
        string[] ay = time.Split(':');
        string val_hr = ay[0].ToString();
        int hr = Convert.ToInt16(val_hr);
        if (val_hr == "01")
        {
            hrr = "1";
        }
        else if (val_hr == "02")
        {
            hrr = "2";
        }
        else if (val_hr == "03")
        {
            hrr = "3";
        }
        else if (val_hr == "04")
        {
            hrr = "4";
        }
        else if (val_hr == "05")
        {
            hrr = "5";
        }
        else if (val_hr == "06")
        {
            hrr = "6";
        }
        else if (val_hr == "07")
        {
            hrr = "7";
        }
        else if (val_hr == "08")
        {
            hrr = "8";
        }
        else if (val_hr == "09")
        {
            hrr = "9";
        }
        else if (val_hr == "13")
        {
            hrr = "1";
        }
        else if (val_hr == "14")
        {
            hrr = "2";
        }
        else if (val_hr == "15")
        {
            hrr = "3";
        }
        else if (val_hr == "16")
        {
            hrr = "4";
        }
        else if (val_hr == "17")
        {
            hrr = "5";
        }
        else if (val_hr == "18")
        {
            hrr = "6";
        }
        else if (val_hr == "19")
        {
            hrr = "7";
        }
        else if (val_hr == "20")
        {
            hrr = "8";
        }
        else if (val_hr == "21")
        {
            hrr = "9";
        }
        else if (val_hr == "22")
        {
            hrr = "10";
        }
        else if (val_hr == "23")
        {
            hrr = "11";
        }
        else if (val_hr == "24")
        {
            hrr = "12";
        }
        if (val_hr == "10" || val_hr == "11" || val_hr == "12")
        {
            ddlhourreq.Text = val_hr;
            ddlminreq.Text = ay[1].ToString();
            ddlendhourreq.Text = val_hr;
            ddlendminreq.Text = ay[1].ToString();
        }
        else
        {
            ddlhourreq.Text = hrr;
            ddlminreq.Text = ay[1].ToString();
            ddlendhourreq.Text = hrr;
            ddlendminreq.Text = ay[1].ToString();
        }
        if (val_hr == "12" || val_hr == "13" || val_hr == "14" || val_hr == "15" || val_hr == "16" || val_hr == "17" || val_hr == "18" || val_hr == "19" || val_hr == "20" || val_hr == "21" || val_hr == "22" || val_hr == "23" || val_hr == "24")
        {
            ddlsessionreq.Text = "PM";
            ddlenssessionreq.Text = "PM";
        }
        else
        {
            ddlsessionreq.Text = "AM";
            ddlenssessionreq.Text = "AM";
        }
    }
    public void bindhostel()
    {
        try
        {
            string itemname = "select HostelMasterPK ,HostelName  from HM_HostelMaster  order by HostelMasterPK ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhostel.DataSource = ds;
                ddlhostel.DataTextField = "HostelName";
                ddlhostel.DataValueField = "HostelMasterPK";
                ddlhostel.DataBind();
            }
            else
            {
                ddlhostel.Items.Insert(0, "Select");
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            cbl_col.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_col.DataSource = ds;
                cbl_col.DataTextField = "collname";
                cbl_col.DataValueField = "college_code";
                cbl_col.DataBind();
                if (cbl_col.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_col.Items.Count; row++)
                    {
                        cbl_col.Items[row].Selected = true;
                    }
                    cb_col.Checked = true;
                    txt_col.Text = "College(" + cbl_col.Items.Count + ")";
                }
                else
                {
                    cb_col.Checked = false;
                    txt_col.Text = "--Select--";
                }
            }
            else
            {
                cb_col.Checked = false;
                txt_col.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void BindBatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_batch.Items.Count; row++)
                    {
                        cbl_batch.Items[row].Selected = true;
                    }
                    cb_batch.Checked = true;
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                }
                else
                {
                    cb_batch.Checked = false;
                    txt_batch.Text = "--Select--";
                }
            }
            else
            {
                cb_batch.Checked = false;
                txt_batch.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void binddegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + collegecode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_degree.Items.Count; row++)
                    {
                        cbl_degree.Items[row].Selected = true;
                    }
                    cb_degree.Checked = true;
                    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                }
                else
                {
                    cb_degree.Checked = false;
                    txt_degree.Text = "--Select--";
                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }
            bindbranch();
        }
        catch
        {
        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string degree = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        if (degree == "")
                        {
                            degree = Convert.ToString(cbl_degree.Items[i].Value);
                        }
                        else
                        {
                            degree = degree + "'" + "," + "'" + Convert.ToString(cbl_degree.Items[i].Value);
                        }
                    }
                }
            }
            if (degree != "")
            {
                string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + degree + "') and degree.college_code in ('" + collegecode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_branch.Items.Count; row++)
                        {
                            cbl_branch.Items[row].Selected = true;
                        }
                        cb_branch.Checked = true;
                        txt_branch.Text = "Department(" + cbl_branch.Items.Count + ")";
                    }
                    else
                    {
                        cb_branch.Checked = false;
                        txt_branch.Text = "--Select--";
                    }
                }
                else
                {
                    cb_branch.Checked = false;
                    txt_branch.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    public void loadstatus()
    {
        try
        {
            cbl_status.Items.Clear();
            cbl_status.Items.Add(new ListItem("In", "0"));
            cbl_status.Items.Add(new ListItem("Out", "1"));
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                cbl_status.Items[i].Selected = true;
            }
            txt_status.Text = "Status(" + cbl_status.Items.Count + ")";
            cb_status.Checked = true;
        }
        catch
        {
        }
    }
    public void loadappstatus()
    {
        try
        {
            cbl_appstatus.Items.Clear();
            cbl_appstatus.Items.Add(new ListItem("Approved", "1"));
            cbl_appstatus.Items.Add(new ListItem("Un Approved", "0"));
            for (int i = 0; i < cbl_appstatus.Items.Count; i++)
            {
                cbl_appstatus.Items[i].Selected = true;
            }
            txt_appstatus.Text = "Status(" + cbl_appstatus.Items.Count + ")";
            cb_appstatus.Checked = true;
        }
        catch
        {
        }
    }
    public void loadenter()
    {
        try
        {
            cbl_enter.Items.Clear();
            cbl_enter.Items.Add(new ListItem("On Time", "0"));
            cbl_enter.Items.Add(new ListItem("Late Time", "1"));
            cbl_enter.Items.Add(new ListItem("Not Enter", "2"));
            for (int i = 0; i < cbl_enter.Items.Count; i++)
            {
                cbl_enter.Items[i].Selected = true;
            }
            txt_enter.Text = "Entered(" + cbl_enter.Items.Count + ")";
            cb_enter.Checked = true;
        }
        catch
        {
        }
    }
    public void loadstutype()
    {
        try
        {
            string collegeid = "";
            if (cbl_col.Items.Count > 0)
            {
                for (int i = 0; i < cbl_col.Items.Count; i++)
                {
                    if (cbl_col.Items[i].Selected == true)
                    {
                        if (collegeid == "")
                        {
                            collegeid = Convert.ToString(cbl_col.Items[i].Value);
                        }
                        else
                        {
                            collegeid = collegeid + "'" + "," + "'" + Convert.ToString(cbl_col.Items[i].Value);
                        }
                    }
                }
            }
            if (collegeid != "")
            {
                cbl_studtype.Items.Clear();
                string deptquery = "select distinct Stud_Type from Registration where college_code in('" + collegeid + "') and Stud_Type is not null and Stud_Type<>''";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_studtype.DataSource = ds;
                    cbl_studtype.DataTextField = "Stud_Type";
                    cbl_studtype.DataBind();
                    if (cbl_studtype.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_studtype.Items.Count; i++)
                        {
                            cbl_studtype.Items[i].Selected = true;
                        }
                        txt_studtype.Text = "Student Type(" + cbl_studtype.Items.Count + ")";
                        cb_studtype.Checked = true;
                    }
                }
                else
                {
                    cb_studtype.Checked = false;
                    txt_studtype.Text = "--Select--";
                }
            }
            else
            {
                cb_studtype.Checked = false;
                txt_studtype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Gettutor(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        string getrollq = "select distinct staff_code  from staff_appl_master sam,staffmaster sm,CO_StudentTutor co where   sam.appl_no = sm.appl_no and co.StaffMasterFK=sam.appl_id and staff_code like '" + prefixText + "%'";
        roll = ws.Getname(getrollq);
        return roll;
    }
    protected void Chktutor_OnCheckedChanged(object sender, EventArgs e)
    {
        if (Chktutor.Checked == true)
            txttutor.Enabled = true;
        else
            txttutor.Enabled = false;
    }
}