using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Configuration;

public partial class Hosteladmissionsettings : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string loadval = "";
    static string colval = "";
    static string printval = "";
    protected void Page_Load(object sender, EventArgs e)
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
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {
            BindCollege();
            load();
            columnordertype();
        }
    }
    protected void LinkButtonselectall_Click(object sender, EventArgs e)
    {
        try
        {
            ItemList.Clear();
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                string si = Convert.ToString(i);
                lb_selectcolumn.Items[i].Selected = true;
                ItemList.Add(lb_selectcolumn.Items[i].Text.ToString());
                Itemindex.Add(si);
            }
            txtcolumn.Visible = true;
            txtcolumn.Text = "";
            lnk_columnordr.Visible = true;
            txtcolumn.Text = "";
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (txtcolumn.Text != "")
                {
                    txtcolumn.Text = txtcolumn.Text + "," + ItemList[i].ToString();
                }
                else
                {
                    txtcolumn.Text = txtcolumn.Text + ItemList[i].ToString();
                }
            }
            if (lb_selectcolumn.Items.Count > 50)
                txtcolumn.Height = 250;
            else
                txtcolumn.Height = 100;
        }
        catch
        { }
    }
    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        lb_selectcolumn.ClearSelection();
        txtcolumn.Text = "";
        txtcolumn.Height = 100;
    }
    protected void lb_selectcolumn_Selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            string value = "";
            int index;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (lb_selectcolumn.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(lb_selectcolumn.Items[index].Text.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(lb_selectcolumn.Items[index].Text.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (lb_selectcolumn.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(lb_selectcolumn.Items[i].Text.ToString());
                    Itemindex.Remove(sindex);
                }
            }
            lnk_columnordr.Visible = true;
            txtcolumn.Visible = true;
            txtcolumn.Text = "";
            for (int i = 0; i < lb_selectcolumn.Items.Count; i++)
            {
                if (txtcolumn.Text == "")
                {
                    txtcolumn.Text = ItemList[i].ToString() + "(" + (i + 1) + ")";
                }
                else
                {
                    txtcolumn.Text = txtcolumn.Text + "," + ItemList[i].ToString() + "(" + (i + 1) + ")";
                }
            }
            if (ItemList.Count > 50)
                txtcolumn.Height = 250;
            else
                txtcolumn.Height = 100;
            if (ItemList.Count == 0)
            {
                txtcolumn.Visible = false;
                lnk_columnordr.Visible = false;
            }
        }
        catch { }
    }
    protected void btnok_click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Text != "Select")
        {
            if (txtcolumn.Text.Trim() != "")
            {
                // int val = 0;
                //if (rdoformat1.Checked == true)
                //    val = 0;
                //else
                //    val = 1;

                //string sql = "if exists ( select * from CO_MasterValues where MasterCriteria ='Hosteladmissioncolumnsettings' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "' and mastercode='" + ddl_coltypeadd.SelectedItem.Value + "' ) update CO_MasterValues set mastercriteria1 ='" + val + "' where MasterCriteria ='Hosteladmissioncolumnsettings' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "' and mastercode='" + ddl_coltypeadd + "'";
                //int insert = d2.update_method_wo_parameter(sql, "TEXT");

                savecolumnorder();
                lblalerterr.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Saved Successfully";
            }
            else
            {
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select atleast one colunm then proceed!";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select Report Type";
        }
    }
    public void savecolumnorder()
    {
        string columnvalue = "";
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = "";
        if (txtcolumn.Text.Trim() != "")
        {
            if (ItemList.Count > 0)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    val = Convert.ToString(lb_selectcolumn.Items.FindByText(ItemList[i].ToString()).Value);
                    if (columnvalue == "")
                    {
                        columnvalue = val;
                    }
                    else
                    {
                        columnvalue = columnvalue + ',' + val;
                    }
                }
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlcollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
        int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
    }
    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='Hosteladmissioncolumnsettings' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                txtcolumn.Text = "";
                ItemList.Clear();
                Itemindex.Clear();
                lb_selectcolumn.ClearSelection();
                lbl_alert.Text = "Deleted Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
            columnordertype();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }
    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Hosteladmissioncolumnsettings' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='Hosteladmissioncolumnsettings' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','Hosteladmissioncolumnsettings','" + ddlcollege.SelectedItem.Value + "')";
            int insert = d2.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Added Successfully";
                txt_description11.Text = "";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Text = "Enter the description";
        }
        columnordertype();
    }
    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex != 0)
        {
            viewcolumorder();
        }
        else
        {
            Itemindex.Clear();
            ItemList.Clear();
            lb_selectcolumn.ClearSelection();
            txtcolumn.Text = "";
            txtcolumn.Height = 100;
        }
    }
    public void viewcolumorder()
    {
        try
        {
            lb_selectcolumn.ClearSelection();
            txtcolumn.Text = "";
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    if (sp.Length > 50)
                        txtcolumn.Height = 250;
                    else
                        txtcolumn.Height = 100;
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext();
                        lb_selectcolumn.Items.FindByValue(colval).Selected = true;
                        if (!Itemindex.Contains(colval))
                        {
                            ItemList.Add(loadval);
                            Itemindex.Add(colval);
                        }
                        if (txtcolumn.Text.Trim() == "")
                            txtcolumn.Text = loadval + "(" + (y + 1) + ")";
                        else
                            txtcolumn.Text = txtcolumn.Text + "," + loadval + "(" + (y + 1) + ")";
                    }
                }
            }
        }
        catch
        {
        }
    }
    void BindCollege()
    {
        try
        {
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }

            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
    public void columnordertype()
    {
        ddl_coltypeadd.Items.Clear();
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='Hosteladmissioncolumnsettings' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_coltypeadd.DataSource = ds;
            ddl_coltypeadd.DataTextField = "MasterValue";
            ddl_coltypeadd.DataValueField = "MasterCode";
            ddl_coltypeadd.DataBind();
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }
    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "1"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "2"));
        lb_selectcolumn.Items.Add(new ListItem("Application Date", "3"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "4"));
        lb_selectcolumn.Items.Add(new ListItem("Mobile No", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Email_Id", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Alternative Course", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Name", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "10"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Nationality", "13"));
        lb_selectcolumn.Items.Add(new ListItem("Occupation", "14"));
        lb_selectcolumn.Items.Add(new ListItem("Remarks", "15"));
        lb_selectcolumn.Items.Add(new ListItem("Application ID", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Batch Year", "17"));
        lb_selectcolumn.Items.Add(new ListItem("Course", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Department", "19"));
        lb_selectcolumn.Items.Add(new ListItem("Semester", "20"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Name", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Address", "22"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Month", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Year", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Marks", "25"));
        lb_selectcolumn.Items.Add(new ListItem("Total Percentage", "26"));
        lb_selectcolumn.Items.Add(new ListItem("State", "27"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "28"));
        lb_selectcolumn.Items.Add(new ListItem("TANCET Mark", "29"));
        lb_selectcolumn.Items.Add(new ListItem("Island", "30"));
        lb_selectcolumn.Items.Add(new ListItem("Ex serviceman", "31"));
        lb_selectcolumn.Items.Add(new ListItem("Differently abled", "32"));
        lb_selectcolumn.Items.Add(new ListItem("First generation", "33"));
        lb_selectcolumn.Items.Add(new ListItem("Sports", "34"));
        lb_selectcolumn.Items.Add(new ListItem("Co Curricular Activites", "35"));
        lb_selectcolumn.Items.Add(new ListItem("BankReferenceNo", "36"));
        lb_selectcolumn.Items.Add(new ListItem("BankReferenceDate", "37"));
        lb_selectcolumn.Items.Add(new ListItem("Vocational", "38"));
        lb_selectcolumn.Items.Add(new ListItem("TotalFess", "39"));
        lb_selectcolumn.Items.Add(new ListItem("Paid", "40"));
        lb_selectcolumn.Items.Add(new ListItem("NoOfAttempt", "41"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel Request", "42"));
        lb_selectcolumn.Items.Add(new ListItem("City", "43"));

    }
    public void loadtext()
    {
        try
        {
            Hashtable columnheadertxt = new Hashtable();
            columnheadertxt.Add("1", "Student Name-stud_name");
            columnheadertxt.Add("2", "DOB-dob");
            columnheadertxt.Add("3", "Application Date-date_applied");
            columnheadertxt.Add("4", "Address-parent_addressP");
            columnheadertxt.Add("5", "Mobile No-Student_Mobile");
            columnheadertxt.Add("6", "Email_Id-StuPer_Id");
            columnheadertxt.Add("7", "Alternative Course-Alternativedegree_code");
            columnheadertxt.Add("8", "Gender-sex");
            columnheadertxt.Add("9", "Parent Name-parent_name");
            columnheadertxt.Add("10", "Religion-religion");
            columnheadertxt.Add("11", "Community-community");
            columnheadertxt.Add("12", "Caste-caste");
            columnheadertxt.Add("13", "Nationality-citizen");
            columnheadertxt.Add("14", "Occupation-parent_occu");
            columnheadertxt.Add("15", "Remarks-remarks");
            columnheadertxt.Add("16", "Application ID-app_formno");
            columnheadertxt.Add("17", "Batch Year-Batch_Year");
            columnheadertxt.Add("18", "Course-Course_Name");
            columnheadertxt.Add("19", "Department-Dept_Name");
            columnheadertxt.Add("20", "Semester-Current_Semester");
            columnheadertxt.Add("21", "Institute Name-Institute_Name");
            columnheadertxt.Add("22", "Institute Address-instaddress");
            columnheadertxt.Add("23", "Pass Month-PassMonth");
            columnheadertxt.Add("24", "Pass Year-PassYear");
            columnheadertxt.Add("25", "Marks-securedmark");
            columnheadertxt.Add("26", "Total Percentage-percentage");
            columnheadertxt.Add("27", "State-parent_statep");
            columnheadertxt.Add("28", "Mother Tongue-mother_tongue");
            columnheadertxt.Add("29", "TANCET Mark-tancet_mark");
            columnheadertxt.Add("30", "Island-TamilOrginFromAndaman");
            columnheadertxt.Add("31", "Ex serviceman-IsExService");
            columnheadertxt.Add("32", "Differently abled-isdisable");
            columnheadertxt.Add("33", "First generation-first_graduate");
            columnheadertxt.Add("34", "Sports-DistinctSport");
            columnheadertxt.Add("35", "Co Curricular Activites-co_curricular");
            columnheadertxt.Add("36", "BankReferenceNo-ApplBankRefNumber");
            columnheadertxt.Add("37", "BankReferenceDate-applbankrefdate");
            columnheadertxt.Add("38", "Vocational-vocational_stream");
            columnheadertxt.Add("39", "TotalFess-totalfees");
            columnheadertxt.Add("40", "Paid-PaidAmount");
            columnheadertxt.Add("41", "NoOfAttempt-noofattempts");
            columnheadertxt.Add("42", "Hostel Request-CampusReq");
            columnheadertxt.Add("43", "City-cityp");

            string header = Convert.ToString(columnheadertxt[colval]);
            string[] headername = header.Split('-');
            loadval = Convert.ToString(headername[0]);
            printval = Convert.ToString(headername[1]);
        }
        catch { }
    }
}