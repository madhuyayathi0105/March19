using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Reflection;
using System.Drawing;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.IO;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Web.UI;


public partial class SmsGroupCreation : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string columnfield = string.Empty;
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 d2 = new DAccess2();
    string PageLogOut = "";
    string deptvalue = "";
    int i;
    int count2 = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
        //    //****************************************************//
        //    string sess = Convert.ToString(Session["IsLogin"]);
        //    PageLogOut = Convert.ToString(Session["PageLogout"]);
        //    if (sess == "")
        //    {
        //    }
        //    else
        //    {
        //        if (!Request.FilePath.Contains("smsM"))
        //        {
        //            string strPreviousPage = "";
        //            if (Request.UrlReferrer != null)
        //            {
        //                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //            }
        //            if (strPreviousPage == "")
        //            {
        //                string redrURI = ConfigurationManager.AppSettings["SMS"].Trim();
        //                Response.Redirect(redrURI, false);
        //                return;
        //            }
        //        }
        //    }
        //    if (Session["collegecode"] == null)
        //    {
        //        string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
        //        Response.Redirect(redrURI, false);
        //        return;
        //    }
        //    //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("sms"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/SMSMOD/sms.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                BindCollege();
                bindstafftype();
                BindDesignation();
                bindept();
                columnType();
                CheckBox1_CheckedChanged(sender, e);
                Chkboxstafftype_CheckedChanged(sender, e);
                chkdesignation_CheckedChanged(sender, e);

            }
        }
        catch (Exception ex) { }
    }
    #region "College Dropdown Selected Index Changed Event"
    public void BindCollege()
    {
        try
        {
            if (!IsPostBack)
            {
                Session["QueryString"] = "";
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    columnfield = " and group_code='" + group_user + "'";
                }
                else
                {
                    columnfield = " and user_code='" + Session["usercode"] + "'";
                }
                hat.Clear();
                hat.Add("column_field", columnfield.ToString());
                ds.Dispose();
                ds.Reset();
                ds = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    // ddlcollege_SelectedIndexChanged(sender, e);
                }
                //PageLoad(sender, e);
            }
        }
        catch
        {
        }
    }
    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //TextBox1.Text = "---Select---";
            // txtstafftype.Text = "---Select---";
            // txtdesignation.Text = "---Select---";
            FpSpread1.Visible = false;
            BindCollege();
            bindstafftype();
            BindDesignation();
            bindept();
            columnType();
            CheckBox1_CheckedChanged(sender, e);
            Chkboxstafftype_CheckedChanged(sender, e);
            chkdesignation_CheckedChanged(sender, e);




        }
        catch
        {
        }
    }
    #endregion

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox1.Checked == true)
            {
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    CheckBoxList1.Items[i].Selected = true;
                    TextBox1.Text = "Department(" + (CheckBoxList1.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    CheckBoxList1.Items[i].Selected = false;
                    TextBox1.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    protected void CheckBoxList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int branchcount = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected == true)
                {
                    value = CheckBoxList1.Items[i].Text;
                    code = CheckBoxList1.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    TextBox1.Text = "Department(" + branchcount.ToString() + ")";
                }
            }
        }
        catch
        {
        }
    }
    protected void Chkboxstafftype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chkboxstafftype.Checked == true)
            {
                for (int i = 0; i < Chhliststafftype.Items.Count; i++)
                {
                    Chhliststafftype.Items[i].Selected = true;
                    txtstafftype.Text = "Stafftype(" + (Chhliststafftype.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < Chhliststafftype.Items.Count; i++)
                {
                    Chhliststafftype.Items[i].Selected = false;
                    txtstafftype.Text = "---Select---";
                }
            }
            // bind_design();
        }
        catch
        {
        }
    }
    protected void Chhliststafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int branchcount = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < Chhliststafftype.Items.Count; i++)
            {
                if (Chhliststafftype.Items[i].Selected == true)
                {
                    value = Chhliststafftype.Items[i].Text;
                    code = Chhliststafftype.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    txtstafftype.Text = "Stafftype(" + branchcount.ToString() + ")";
                }
            }
            // bind_design();
        }
        catch
        {
        }
    }
    protected void chkdesignation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdesignation.Checked == true)
            {
                for (int i = 0; i < chklstdesignation.Items.Count; i++)
                {
                    chklstdesignation.Items[i].Selected = true;
                    txtdesignation.Text = "Designation(" + (chklstdesignation.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklstdesignation.Items.Count; i++)
                {
                    chklstdesignation.Items[i].Selected = false;
                    txtdesignation.Text = "---Select---";
                }
            }
        }
        catch
        {
        }
    }
    protected void chklstdesignation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int branchcount = 0;
            string value = "";
            string code = "";
            for (int i = 0; i < chklstdesignation.Items.Count; i++)
            {
                if (chklstdesignation.Items[i].Selected == true)
                {
                    value = chklstdesignation.Items[i].Text;
                    code = chklstdesignation.Items[i].Value.ToString();
                    branchcount = branchcount + 1;
                    txtdesignation.Text = "Designation(" + branchcount.ToString() + ")";
                }
            }
        }
        catch
        {
        }
    }
    #region "Load Function for Department Details"
    //public void BindDepartment()
    //{
    //    try
    //    {
    //        count = 0;
    //        ds2.Dispose();
    //        ds2.Reset();
    //        ds2 = d2.loaddepartment(ddlcollege.SelectedValue.ToString());
    //        chklstbranch.DataSource = ds;
    //        chklstbranch.DataTextField = "dept_name";
    //        chklstbranch.DataValueField = "Dept_Code";
    //        chklstbranch.DataBind();
    //        for (int i = 0; i < chklstbranch.Items.Count; i++)
    //        {
    //            chklstbranch.Items[i].Selected = true;
    //            if (chklstbranch.Items[i].Selected == true)
    //            {
    //                count += 1;
    //            }
    //            if (chklstbranch.Items.Count == count)
    //            {
    //                chkbranch.Checked = true;
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    #endregion
    public void bindstafftype()
    {
        try
        {
            SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
            SqlDataAdapter cmstafftype = new SqlDataAdapter("SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1", mysql);
            mysql.Close();
            mysql.Open();
            DataSet ds = new DataSet();
            cmstafftype.Fill(ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Chhliststafftype.DataSource = ds;
                Chhliststafftype.DataValueField = "StfType";
                Chhliststafftype.DataTextField = "StfType";
                Chhliststafftype.DataBind();
                for (int i = 0; i < Chhliststafftype.Items.Count; i++)
                {
                    Chhliststafftype.Items[i].Selected = true;
                    if (Chhliststafftype.Items[i].Selected == true)
                    {
                        count2 += 1;
                    }
                    if (Chhliststafftype.Items.Count == count2)
                    {
                        Chkboxstafftype.Checked = true;
                    }
                }
                //ddl_stftype.DataSource = ds;
                //ddl_stftype.DataTextField = "StfType";
                //ddl_stftype.DataValueField = "StfType";
                //ddl_stftype.DataBind();
            }
            mysql.Close();
        }
        catch
        {
        }
    }
    #region "Load Function for Designation Details"
    public void BindDesignation()
    {
        try
        {
            int count = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.binddesi(ddlcollege.SelectedValue.ToString());
            chklstdesignation.DataSource = ds2;
            chklstdesignation.DataValueField = "desig_code";
            chklstdesignation.DataTextField = "desig_name";
            chklstdesignation.DataBind();
            chklstdesignation.SelectedIndex = chklstdesignation.Items.Count - 1;
            for (int i = 0; i < chklstdesignation.Items.Count; i++)
            {
                chklstdesignation.Items[i].Selected = true;
                if (chklstdesignation.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chklstdesignation.Items.Count == count)
                {
                    chkdesignation.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    #endregion
    public void bindept()
    {
        try
        {
            int count = 0;
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.loaddepartment(ddlcollege.SelectedValue.ToString());
            CheckBoxList1.DataSource = ds2;
            CheckBoxList1.DataTextField = "dept_name";
            CheckBoxList1.DataValueField = "Dept_Code";
            CheckBoxList1.DataBind();
            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                CheckBoxList1.Items[i].Selected = true;
                if (CheckBoxList1.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (CheckBoxList1.Items.Count == count)
                {
                    CheckBox1.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void ddlreport_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void btnDel_OnClick(object sender, EventArgs e)
    {
        deleteReportType();
    }
    protected void deleteReportType()
    {
        int delMQ = 0;
        string Usercollegecode = string.Empty;
        Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
        string linkName = string.Empty;
        if (ddlreport.Items.Count > 0 && ddlreport.SelectedItem.Text != "Select")
            linkName = Convert.ToString(ddlreport.SelectedItem.Text);
        if (!string.IsNullOrEmpty(linkName) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;

            linkCriteria = "smsGroup";
            int delQ = 0;
            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete from New_InsSettings where LinkName='" + linkName + "'  and college_code='" + Usercollegecode + "'", "Text")), out delQ);

            int.TryParse(Convert.ToString(d2.update_method_wo_parameter("delete  from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and mastervalue='" + linkName + "'  and collegecode='" + Usercollegecode + "'", "Text")), out delMQ);

        }
        if (delMQ > 0)
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
        }
        else
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Failed')", true);
        columnType();
    }

    protected void btnGo_OnClick(object sender, EventArgs e)
    {
    }

    protected void btnaddtype_Click(object sender, EventArgs e)
    {
        try
        {

            string Usercollegecode = string.Empty;
            Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
            string strDesc = Convert.ToString(txtdesc.Text);
            string linkCriteria = string.Empty;
            linkCriteria = "smsGroup";
            if (!string.IsNullOrEmpty(strDesc) && !string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
            {
                string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "') update CO_MasterValues set MasterValue ='" + strDesc + "' where MasterValue ='" + strDesc + "' and MasterCriteria ='" + linkCriteria + "' and CollegeCode ='" + Usercollegecode + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + strDesc + "','" + linkCriteria + "','" + Usercollegecode + "')";
                int insert = d2.update_method_wo_parameter(sql, "Text");
                if (insert > 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true); txtdesc.Text = string.Empty;
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter report type')", true);
            }
            columnType();
            // divcolorder.Attributes.Add("Style", "height: 100%; display:block; z-index: 1000; width: 100%; background-color: rgba(54, 25, 25, .2); position: absolute; top: 0;left: 0px;");
        }
        catch { }
    }
    public void columnType()
    {
        string Usercollegecode = string.Empty;
        Usercollegecode = Convert.ToString(ddlcollege.SelectedValue);
        ddlreport.Items.Clear();
        if (!string.IsNullOrEmpty(Usercollegecode) && Usercollegecode != "0")
        {
            string linkCriteria = string.Empty;

            linkCriteria = "smsGroup";
            string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='" + linkCriteria + "' and CollegeCode='" + Usercollegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlreport.DataSource = ds;
                ddlreport.DataTextField = "MasterValue";
                ddlreport.DataValueField = "MasterCode";
                ddlreport.DataBind();
                // ddlreport.Items.Insert(0, new ListItem("Select", "0"));              
            }
            else
            {
                ddlreport.Items.Insert(0, new ListItem("Select", "0"));
            }
        }
    }
    protected void btnstaffgo_Click(object sender, EventArgs e)
    {
        try
        {

            FpSpread1.Visible = false;
            btnSave.Visible = true;
            string staffvalue = "";
            string designvalue = "";
            string stafftyp = "";
            staffvalue = Convert.ToString(getCblSelectedValue(Chhliststafftype));
            deptvalue = Convert.ToString(getCblSelectedValue(CheckBoxList1));
            designvalue = Convert.ToString(getCblSelectedValue(chklstdesignation));
            if (staffvalue.Trim() != "")
            {
                stafftyp = " and st.stftype in ('" + staffvalue + "')";
            }
            if (deptvalue.Trim() != "")
            {
                deptvalue = " and st.dept_code in ( '" + deptvalue + "' )";
            }
            if (designvalue.Trim() != "")
            {
                designvalue = "and st.desig_code in ( '" + designvalue + "')";
            }
            string strstaffdetail = "select distinct sm.staff_code,sm.staff_name,st.stftype,sam.per_mobileno,sam.email,(select MasterValue from CO_MasterValues m where  sm.sms_groupCode=m.MasterCode and mastercriteria='smsGroup') as groupName,sm.sms_groupCode,h.dept_name ,sam.appl_id as App_No from staffmaster sm,stafftrans st,staff_appl_master sam, hrdept_master h where st.staff_code=sm.staff_code and sm.appl_no = sam.appl_no  and st.dept_code =h.dept_code and latestrec = 1 and sm.college_code = " + ddlcollege.SelectedValue.ToString() + " " + deptvalue + " " + designvalue + " " + stafftyp + " and resign = 0 and settled = 0 ";

            DataSet dsDet = d2.select_method_wo_parameter(strstaffdetail, "text");
            if (dsDet.Tables.Count > 0 && dsDet.Tables[0].Rows.Count > 0)
            {
                //loadgrid(dsDet);
                loadspread(dsDet);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('No Record Found!')", true);
            }
            // loadgrid(d2.select_method_wo_parameter(strstaffdetail, "text"));

        }
        catch
        {
        }

    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        string groupCode = Convert.ToString(ddlreport.SelectedItem.Value);
        if (getCheckCount() && !string.IsNullOrEmpty(groupCode))
        {
            getSaveDetails(groupCode);
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Select Group/Staff Details!')", true);
        }
    }
    //protected bool getCheckCount()
    //{
    //    bool boolSave = false;
    //    foreach (GridViewRow row in FpSpread1.Rows)
    //    {
    //        CheckBox cbsel = (CheckBox)row.FindControl("cbselect");
    //        if (!cbsel.Checked)
    //            continue;
    //        boolSave = true;
    //    }
    //    return boolSave;
    //}
    protected bool getCheckCount()
    {
        FpSpread1.SaveChanges();
        bool boolSave = false;
        for (int i = 0; i < FpSpread1.Rows.Count; i++)
        {

            byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
            if (check == 1)
            {
                //string groupname = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                //string staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                //string updQ = "update staffmaster set sms_groupname='" + groupname + "' where staff_code='" + staffcode + "'";
                //int upd = d2.update_method_wo_parameter(updQ, "Text");
                //continue;
                boolSave = true;
            }

        }
        return boolSave;
    }
    protected void getSaveDetails(string groupCode)
    {
        try
        {
            bool boolSave = false;

            for (int i = 1; i < FpSpread1.Rows.Count; i++)
            {
                byte check = Convert.ToByte(FpSpread1.Sheets[0].Cells[i, 1].Value);
                if (check == 1)
                {


                    string staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Text);
                    //   string staffcode = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);


                    string updQ = "update staffmaster set sms_groupCode='" + groupCode + "' where staff_code='" + staffcode + "'";
                    int upd = d2.update_method_wo_parameter(updQ, "Text");
                    boolSave = true;
                }


            }

            if (boolSave)
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Save Successfully')", true);
            }
        }
        catch { }
    }

    //private void loadgrid(DataSet ds)
    //{
    //    try
    //    {
    //        DataTable dt = new DataTable();
    //        dt.Columns.Add("SNo");
    //        dt.Columns.Add("staff_code");
    //        dt.Columns.Add("staff_name");
    //        dt.Columns.Add("stftype");
    //        dt.Columns.Add("per_mobileno");
    //        dt.Columns.Add("email");
    //        dt.Columns.Add("appno");
    //        dt.Columns.Add("sms_groupCode");
    //        DataRow drow;
    //        int rowcount = 0;
    //        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
    //        {
    //            drow = dt.NewRow();
    //            drow["SNo"] = Convert.ToString(++rowcount);
    //            drow["staff_code"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_code"]);
    //            drow["staff_name"] = Convert.ToString(ds.Tables[0].Rows[row]["staff_name"]);
    //            drow["stftype"] = Convert.ToString(ds.Tables[0].Rows[row]["stftype"]);
    //            drow["per_mobileno"] = Convert.ToString(ds.Tables[0].Rows[row]["per_mobileno"]);
    //            drow["email"] = Convert.ToString(ds.Tables[0].Rows[row]["email"]);
    //            drow["appno"] = Convert.ToString(ds.Tables[0].Rows[row]["app_no"]);
    //            drow["sms_groupCode"] = Convert.ToString(ds.Tables[0].Rows[row]["sms_groupCode"]);


    //            dt.Rows.Add(drow);
    //        }
    //        if (dt.Rows.Count > 0)
    //        {
    //            gdReport.DataSource = dt;
    //            gdReport.DataBind();
    //            divGrid.Visible = true;
    //            btnSave.Visible = true;
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}
    private void loadspread(DataSet ds)
    {
        try
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("SNo");
            dt.Columns.Add("Select");
            dt.Columns.Add("Staff Code");
            dt.Columns.Add("Staff Name");
            dt.Columns.Add("Staff Type");
            dt.Columns.Add("Staff-MobileNo");
            dt.Columns.Add("Staff-EmailId");
            dt.Columns.Add("Group Name");

            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = false;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            for (int row = 0; row < dt.Columns.Count; row++)
            {
                FpSpread1.Sheets[0].ColumnCount++;
                string col = Convert.ToString(dt.Columns[row].ColumnName);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Text = col;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].ForeColor = ColorTranslator.FromHtml("#000000");
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, row].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

                //int rollNo = 0;
                //int regNo = 0;
                //int admNo = 0;
                //bool boolroll=false;
                switch (col)
                {
                    case "SNo":
                        FpSpread1.Sheets[0].Columns[row].Width = 50;
                        break;
                    case "Select":
                        FpSpread1.Sheets[0].Columns[row].Width = 50;
                        break;
                    case "Staff Code":
                        FpSpread1.Sheets[0].Columns[row].Width = 100;
                        //admNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        //boolroll = true;
                        break;
                    case "Staff Name":
                        FpSpread1.Sheets[0].Columns[row].Width = 150;
                        //rollNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        //boolroll = true;
                        break;
                    case "Staff Type":
                        FpSpread1.Sheets[0].Columns[row].Width = 150;
                        //regNo = Convert.ToInt32(spreadDet.Sheets[0].ColumnCount - 1);
                        //boolroll = true;
                        break;
                    case "Staff-MobileNo":
                        FpSpread1.Sheets[0].Columns[row].Width = 150;
                        break;
                    case "Staff-EmailId":
                        FpSpread1.Sheets[0].Columns[row].Width = 200;
                        break;
                    case "Group Name":
                        FpSpread1.Sheets[0].Columns[row].Width = 120;
                        break;
                    //case "Semester":
                    //    spreadDet.Sheets[0].Columns[spreadDet.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    //    break;
                }

            }

            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            cball.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = false;
            DataRow drow;
            int rowcount = 0;
            int height = 0;
            FpSpread1.Sheets[0].RowCount++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = cball;

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                FpSpread1.Sheets[0].RowCount++;
                height += 10;
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    if (col == 0)
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(++rowcount);
                    else if (col == 1)
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].CellType = cb;
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(ds.Tables[0].Rows[row][col - 2]);
                    }
                }
                //drow = dt.NewRow();
                //drow["SNo"] = Convert.ToString(++rowcount);
                //drow["Roll No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                //drow["Reg No"] = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                //drow["Addmission No"] = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                //drow["Student Name"] = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                //drow["Course"] = Convert.ToString(ds.Tables[0].Rows[row]["course"]);
                //drow["Stream"] = stream;
                //drow["BatchYear"] = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                //drow["Branch"] = Convert.ToString(ds.Tables[0].Rows[row]["Branch"]);
                //drow["Department"] = Convert.ToString(ds.Tables[0].Rows[row]["deptname"]);
                //drow["Q1"] = Convert.ToString(ds.Tables[0].Rows[row]["param_1"]);
                //drow["Q2"] = Convert.ToString(ds.Tables[0].Rows[row]["param_2"]);
                //drow["Q3"] = Convert.ToString(ds.Tables[0].Rows[row]["param_3"]);
                //drow["Q4"] = Convert.ToString(ds.Tables[0].Rows[row]["param_4"]);
                //drow["Q5"] = Convert.ToString(ds.Tables[0].Rows[row]["param_5"]);
                //drow["Q6"] = Convert.ToString(ds.Tables[0].Rows[row]["param_6"]);
                //drow["Q7"] = Convert.ToString(ds.Tables[0].Rows[row]["param_7"]);
                //drow["Q8"] = Convert.ToString(ds.Tables[0].Rows[row]["param_8"]);
                //dt.Rows.Add(drow);


            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.SaveChanges();
            //FpSpread1.Height = height;

            FpSpread1.Width = 1000;
            FpSpread1.Height = 500;
            FpSpread1.Visible = true;
            print.Visible = true;



        }

        catch { }
    }

    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }
    #region Print
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
                lblvalidation1.Text = "Please Enter Your  Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }
    public void btnprintmaster_click(object sender, EventArgs e)
    {
        try
        {
            //// lblvalidation1.Text = "";
            //string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            //string ledgerAcr = getledgerAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            degreedetails = "Sms Group Creation";
            //\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            // degreedetails = "Institutionwise Balance Report\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "Ledger : " + '@' + ledgerAcr;
            pagename = "SmsGroupCreation.aspx";
            printcontrolhed.loadspreaddetails(FpSpread1, pagename, degreedetails);
            printcontrolhed.Visible = true;
        }
        catch { }
    }
    protected void getPrintSettings()
    {
        try
        {
            //barath 15.03.17
            #region Excel print settings
            string usertype = "";
            if (usercode.Trim() != "")
                usertype = " and usercode='" + usercode + "'";
            else if (group_user.Trim() != "")
                usertype = " and group_code='" + group_user + "'";
            string printset = d2.GetFunction("select value from Master_Settings where settings='Excel and Pdf Print Settings' " + usertype + " ");
            if (printset != "")
            {
                if (printset.Contains("E"))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                }
                if (printset.Contains("P"))
                {
                    btnprintmasterhed.Visible = true;
                }
                if (printset == "0")
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    btnprintmasterhed.Visible = true;
                }
            }
            #endregion
        }
        catch { }
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
    private string getCblSelectedTempText(CheckBoxList cblSelected)
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
                        selectedText.Append("," + Convert.ToString(cblSelected.Items[sel].Text));
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
}
    #endregion
