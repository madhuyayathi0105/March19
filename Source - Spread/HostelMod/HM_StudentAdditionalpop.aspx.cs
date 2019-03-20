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
using System.Drawing;
using System.Text;
public partial class HM_StudentAdditionalpop : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string college = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Boolean Cellclick = false;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    static string buttonvalue = "";
    int i = 0;
    int j = 0;
    string getday = "";
    string gettoday = "";
    string sql = "";
    int commcount;
    string buildvalue = "";
    string build = "";
    string itemheader = "";
    string commname = "";
    string hostel = "";
    string hostelcode = "";
    static string hostel_name_code = "";
    string dtaccessdate = "";
    string dtaccesstime = "";
    string rollno = "";
    string hosid = "";
    string regno = "";
    string name = "";
    string degree = "";
    string hostlnm = "";
    string desc = "";
    string date = "";
    string amount = "";
    string degreecode = "";
    bool flag_true = false;
    private EventArgs e;
    private object sender;
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
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lbl_norec.Text = "";
        if (!IsPostBack)
        {
            buttonvalue = "";
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            //Div1.Visible = false;
            Fpspread2.Visible = false;
            rdb_cumulative.Checked = true;
            txt_hostelname.Attributes.Add("readonly", "readonly");
            txt_hostelname2.Attributes.Add("readonly", "readonly");
            txt_degree.Attributes.Add("readonly", "readonly");
            txt_degree1.Attributes.Add("readonly", "readonly");
            txt_branch.Attributes.Add("readonly", "readonly");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindhostelname();
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindhostelname1();
            bindbatch();
            binddegree();
            bindbranch(college);
            description();
            btn_ok.Visible = false;
            btn_exit1.Visible = false;
            hostel_name_code = "";
            btn_go_Click(sender, e);
        }
        // theivamani 14.11.15
        lbl_errormsg.Visible = false;
    }
    //main student additional
    public void bindhostelname()
    {
        try
        {
            ds.Clear();
            cbl_hostelname.Items.Clear();
            //string selecthostel = "select HostelMasterPK,HostelName from HM_HostelMaster order by HostelName";// where CollegeCode='" + collegecode1 + "'
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_hostelname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostlname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_hostelname.Text = "--Select--";
            cb_hostelname.Checked = false;
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname.Items.Count)
                {
                    cb_hostelname.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        txt_rollno.Enabled = true;
        txt_rollno.Enabled = false;
        txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
        popupstudaddinl.Visible = true;
        txt_regno.Text = "";
        txt_rollno.Text = "";
        Txtid.Text = "";
        txt_name.Text = "";
        txt_degree.Text = "";
        txt_hostelname1.Text = "";
        description();
        txt_description.Text = "";
        txt_amount.Text = "";
        btn_save.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        //Added By Saranyadevi 14.2.2018
        txt_degree.Enabled = true;
        lbl_degree.Enabled = true;
        lbl_rollno.Visible = true;
        txt_rollno.Visible = true;
        lbl_regno.Enabled = true;
        txt_regno.Enabled = true;
        lbl_name.Enabled = true;
        txt_name.Enabled = true;
        lbl_stu.Visible = false;
        lbl_Sturollno.Visible = false;
        lblstudent.Visible = false;
        lblstudentcount.Visible = false;
        txt_stu.Visible = false;
    }
    // columnorder
    public void cb_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            int j = 0;
            if (cb_column.Checked == true)
            {
                ItemList.Clear();
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    if (rdb_cumulative.Checked == true)
                    {
                        if (cblcolumnorder.Items[i].Text != "Date" && cblcolumnorder.Items[i].Text != "Description")
                        {
                            ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                            Itemindex.Add(si);
                            cblcolumnorder.Items[5].Enabled = false;
                            cblcolumnorder.Items[6].Enabled = false;
                            // cblcolumnorder.Items[5]. = false;
                            cblcolumnorder.Items[5].Selected = false;
                            cblcolumnorder.Items[6].Selected = false;
                        }
                    }
                    else
                    {
                        ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                        Itemindex.Add(si);
                    }
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                for (i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    tborder.Text = tborder.Text + ItemList[i].ToString();
                    tborder.Text = tborder.Text + "(" + (j).ToString() + ")  ";
                }
            }
            else
            {
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
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
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void lb_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            cb_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_columnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index;
            string value = "";
            string result = "";
            string sindex = "";
            cb_column.Checked = false;
            cblcolumnorder.Items[0].Selected = true;
            cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    if (rdb_cumulative.Checked == true)
                    {
                        if (cblcolumnorder.Items[index].Text != "Date" && cblcolumnorder.Items[index].Text != "Description")
                        {
                            ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                            Itemindex.Add(sindex);
                        }
                    }
                    else
                    {
                        ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                        Itemindex.Add(sindex);
                    }
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            for (i = 0; i < ItemList.Count; i++)
            {
                tborder.Text = tborder.Text + ItemList[i].ToString();
                tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";
            }
            if (ItemList.Count == 22)
            {
                cb_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        string from = "";
        string to = "";
        string hostel1 = "";
        int index;
        string colno = "";
        Printcontrol.Visible = false;
        Hashtable columnhash = new Hashtable();
        columnhash.Add("Roll_No", "Roll No");
        columnhash.Add("Reg_No", "Reg No");
        columnhash.Add("Stud_Name", "Name");
        columnhash.Add("Degree", "Degree");
        columnhash.Add("HostelName", "Hostel Name");
        columnhash.Add("Transdate", "Date");
        columnhash.Add("MasterValue", "Description");
        columnhash.Add("Amount", "Amount");
        columnhash.Add("id", "Student Id");
        if (ItemList.Count == 0)
        {
            ItemList.Add("Roll_No");
            ItemList.Add("Reg_No");
            ItemList.Add("id");
            ItemList.Add("Stud_Name");
          
        }
        from = Convert.ToString(txt_fromdate.Text);
        string[] splitdate = from.Split('-');
        splitdate = splitdate[0].Split('/');
        DateTime dt = new DateTime();
        if (splitdate.Length > 0)
        {
            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
        }
        getday = dt.ToString("MM/dd/yyyy");
        to = Convert.ToString(txt_todate.Text);
        string[] splitdate1 = to.Split('-');
        splitdate1 = splitdate1[0].Split('/');
        DateTime dt1 = new DateTime();
        if (splitdate1.Length > 0)
        {
            dt1 = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
        }
        gettoday = dt1.ToString("MM/dd/yyyy");
        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                if (hostel1 == "")
                {
                    hostel1 = "" + cbl_hostelname.Items[i].Value.ToString() + "";
                }
                else
                {
                    hostel1 = hostel1 + "'" + "," + "'" + cbl_hostelname.Items[i].Value.ToString() + "";
                }
            }
        }
        if (txt_hostelname.Text != "--Select--")
        {
            if (rdb_cumulative.Checked == true)
            {
                sql = " select SUM(AdditionalAmt) as Amount,hr.APP_No,r.Roll_No,r.Stud_Name ,hr.id,r.Reg_No,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name)as Degree,CONVERT(varchar(10),TransDate ,103)as Transdate,m.MasterValue  from HT_StudAdditionalDet sd,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr,CO_MasterValues m where m.MasterCode=sd.AdditionalDesc and m.MasterCriteria='Expense' and sd.App_No =r.App_No and sd.App_No =hr.APP_No and hr.HostelMasterFK =hm.HostelMasterPK and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id and TransDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and hm.HostelMasterPK in ('" + hostel1 + "') and ISNULL(IsVacated,'0')='0' group by hr.APP_No,hr.id, r.Roll_No,r.Stud_Name,r.Reg_No ,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name),Transdate,MasterValue";
                cblcolumnorder.Items[5].Enabled = false;
                cblcolumnorder.Items[6].Enabled = false;
                // cblcolumnorder.Items[5]. = false;
                cblcolumnorder.Items[5].Selected = false;
                cblcolumnorder.Items[6].Selected = false;
            }
            else if (rdb_detail.Checked == true)
            {
                sql = " select distinct AdditionalAmt as Amount,hr.APP_No,r.Roll_No,r.Stud_Name ,hr.id,r.Reg_No,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name)as Degree, convert(varchar(10), TransDate,103)as Transdate, sd.StudAdditionalpk,sd.AdditionalDesc,m.MasterValue from HT_StudAdditionalDet sd,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr,CO_MasterValues m where m.MasterCode=sd.AdditionalDesc and m.MasterCriteria='Expense' and sd.App_No =r.App_No and sd.App_No =hr.APP_No and hr.HostelMasterFK =hm.HostelMasterPK and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id and TransDate between '" + dt.ToString("MM/dd/yyyy") + "'and '" + dt1.ToString("MM/dd/yyyy") + "' and hm.HostelMasterPK in ('" + hostel1 + "') and ISNULL(IsVacated,'0')='0' order by r.Roll_No";
            }
            //Fpspread1.DataBind();        
            //Fpspread1.Sheets[0].ColumnCount = 7;                    
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                pheaderfilter.Visible = true;
                pcolumnorder.Visible = true;
                mainspread.Visible = true;
                Fpspread1.Visible = true;
                div_report.Visible = true;
                lbl_errormsg.Visible = false;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.SheetCorner.ColumnCount = 0;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                Fpspread1.Sheets[0].AutoPostBack = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;
                Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    colno = Convert.ToString(ds.Tables[0].Columns[j]);
                    if (ItemList.Contains(Convert.ToString(colno)))
                    {
                        index = ItemList.IndexOf(Convert.ToString(colno));
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[i, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                    //  Fpspread1.Sheets[0].Cells[i, 0].Note = Convert.ToString(ds.Tables[0].Rows[i]["StudAdditionalpk"]);
                    if (rdb_cumulative.Checked == false)
                    {
                        Fpspread1.Sheets[0].Cells[i, 0].Note = Convert.ToString(ds.Tables[0].Rows[i]["TransDate"]);
                        Fpspread1.Sheets[0].Cells[i, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["StudAdditionalpk"]);
                        Fpspread1.Sheets[0].Cells[i, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["AdditionalDesc"]);
                    }
                    for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                        {
                            index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                            if (Convert.ToString(ds.Tables[0].Columns[j]) != "Amount")
                            {
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Right;
                            }
                        }
                    }
                }
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.SaveChanges();
                mainspread.Visible = true;
                Fpspread1.Visible = true;
                div_report.Visible = true;
            }
            else
            {
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                mainspread.Visible = false;
                Fpspread1.Visible = false;
                div_report.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
            }
        }
        else
        {
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            mainspread.Visible = false;
            Fpspread1.Visible = false;
            div_report.Visible = false;
            lbl_errormsg.Visible = true;
            lbl_errormsg.Text = "Kindly Select Hostel Name";
        }
    }
    protected void rdb_cumulative_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            mainspread.Visible = false;
            Fpspread1.Visible = false;
            div_report.Visible = false;
            cb_column.Checked = false;
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
                lnk_columnorder.Visible = false;
                ItemList.Clear();
                Itemindex.Clear();
                // cblcolumnorder.Items[0].Enabled = false;
                cblcolumnorder.Items[5].Enabled = false;
                cblcolumnorder.Items[6].Enabled = false;
                cblcolumnorder.Items[5].Selected = false;
                cblcolumnorder.Items[6].Selected = false;
            }
            tborder.Text = "";
            tborder.Visible = false;
            if (rdb_cumulative.Checked == true)
            {
                cblcolumnorder.Items[0].Selected = true;
                cblcolumnorder.Items[1].Selected = true;
                cblcolumnorder.Items[2].Selected = true;
            }
        }
        catch
        {
        }
    }
    protected void rdb_detail_checkedchanged(object sender, EventArgs e)
    {
        try
        {
            pheaderfilter.Visible = false;
            pcolumnorder.Visible = false;
            mainspread.Visible = false;
            Fpspread1.Visible = false;
            div_report.Visible = false;
            cb_column.Checked = false;
            for (i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                cblcolumnorder.Items[i].Selected = false;
                lnk_columnorder.Visible = false;
                ItemList.Clear();
                Itemindex.Clear();
                // cblcolumnorder.Items[0].Enabled = false;
                cblcolumnorder.Items[5].Enabled = true;
                cblcolumnorder.Items[6].Enabled = true;
            }
            tborder.Text = "";
            tborder.Visible = false;
            if (rdb_detail.Checked == true)
            {
                cblcolumnorder.Items[0].Selected = true;
                cblcolumnorder.Items[1].Selected = true;
                cblcolumnorder.Items[2].Selected = true;
            }
        }
        catch
        {
        }
    }
    protected void Fpspread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            if (rdb_cumulative.Checked == false)
            {
                Cellclick = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Fpspread1_SelectedIndexChanged(object sender, EventArgs e)
    
    {
        if (Cellclick == true)
        {
            try
            {
                txt_rollno.Enabled = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                btn_save.Visible = false;
                int activerow = 0;
                activerow = Convert.ToInt32(Fpspread1.ActiveSheetView.ActiveRow.ToString());
                string studd_addpk = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string desc_val = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                for (i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        Fpspread1.Sheets[0].SelectionBackColor = Color.Orange;
                        Fpspread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
                string roll = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                string date = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Note);
                Session["studd_addpk"] = Convert.ToString(studd_addpk);
                Session["desc_val"] = Convert.ToString(desc_val);
                //  StudAdditionalpk ='" + Convert.ToString( Session["studd_addpk"]) + "'
                string[] splitdate = date.Split('-');
                splitdate = splitdate[0].Split('/');
                DateTime dt = new DateTime();
                if (splitdate.Length > 0)
                {
                    dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                }
                ds.Clear();
                sql = " select AdditionalAmt as Amount,hr.APP_No,r.Roll_No,hr.id,r.Stud_Name ,r.degree_code,r.Reg_No,hm.HostelMasterPK,hm.HostelName ,(c.Course_Name+' - '+dt.Dept_Name)as Degree, CONVERT(varchar(10),sd.TransDate,103) as TransDate,sd.AdditionalDesc, sd.StudAdditionalpk from HT_StudAdditionalDet sd,Registration r,Degree d,Department dt,Course c,HM_HostelMaster hm,HT_HostelRegistration hr where sd.App_No =r.App_No and sd.App_No =hr.APP_No and hr.HostelMasterFK =hm.HostelMasterPK and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id  and sd.StudAdditionalpk ='" + studd_addpk + "'";
                //sql = "select add_Amount as Amount, r.Reg_No,r.Roll_No,r.Stud_Name,Description,d.Degree_Code ,h.Hostel_code,h.Hostel_Name ,CONVERT(varchar(10),Entry_Date,103) as Entry_Date,(c.Course_Name+' - '+dt.Dept_Name)as Degree from StudentAdditional_Details sd,Registration r,Hostel_Details h,Degree d,Department dt,Course c where sd.Roll_No =r.Roll_No and sd.Hostel_Code =h.Hostel_code and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and C.Course_Id =d.Course_Id and r.Roll_No ='" + roll + "' and Entry_Date ='" + dt.ToString("MM/dd/yyyy") + "'";
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    rollno = Convert.ToString(ds.Tables[0].Rows[0]["Roll_No"]);
                    if (rollno != "")
                    {
                        txt_rollno.Text = Convert.ToString(rollno);
                    }
                    regno = Convert.ToString(ds.Tables[0].Rows[0]["Reg_No"]);
                    if (regno != "")
                    {
                        txt_regno.Text = Convert.ToString(regno);
                    }
                    hosid = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                    if (hosid != "")
                    {
                        Txtid.Text = Convert.ToString(hosid);
                    }
                    name = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                    if (name != "")
                    {
                        txt_name.Text = Convert.ToString(name);
                    }
                    degree = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                    if (degree != "")
                    {
                        txt_degree.Text = Convert.ToString(degree);
                    }
                    hostlnm = Convert.ToString(ds.Tables[0].Rows[0]["HostelName"]);
                    if (hostlnm != "")
                    {
                        txt_hostelname1.Text = Convert.ToString(hostlnm);
                    }
                    //string valu = Convert.ToString(ds.Tables[0].Rows[0]["AdditionalDesc"]);
                    //string description = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCode='" + valu + "'");
                    ddl_description.SelectedIndex = ddl_description.Items.IndexOf(ddl_description.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["AdditionalDesc"])));
                    //desc = description;
                    //if (desc != "")
                    //{
                    //    txt_description.Text = Convert.ToString(desc);
                    //    ddl_description.SelectedItem.Text = "Others";
                    //}
                    date = Convert.ToString(ds.Tables[0].Rows[0]["TransDate"]);
                    if (date != "")
                    {
                        txt_date.Text = Convert.ToString(date);
                    }
                    amount = Convert.ToString(ds.Tables[0].Rows[0]["Amount"]);
                    if (amount != "")
                    {
                        txt_amount.Text = Convert.ToString(amount);
                    }
                    Session["hostelcode1"] = Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterPK"]);
                    Session["degreecode1"] = Convert.ToString(ds.Tables[0].Rows[0]["Degree_Code"]);
                    popupstudaddinl.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
    //popup student additional
    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }
    }
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
        //    string batch = "";
        //    if (cb1.Checked == true)
        //    {
        //        batch = "@" + " Batch : " + cbl_batch.SelectedItem.ToString() + "-" + " Degree :" + cbl_degree.SelectedItem.Text.ToString() + "-" + " Branch :" + cbl_branch.SelectedItem.Text.ToString();
        //    }
        //    string pagename = "HostelRegistration.aspx";
        //    string degreedetails = "Hostel Registration" + batch + date;
        //    Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
        //    Printcontrol.Visible = true;
        //}
        //catch (Exception ex)
        //{
        //}
        try
        {
            string hostelname = "";
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            if (cb_hostelname.Checked == true)
            {
                hostelname = "@" + " Hostel : " + cbl_hostelname.SelectedItem.ToString();
            }
            string pagename = "HM_StudentAdditionalpop.aspx";
            string student = "Student Additional Collection Report" + hostelname + date;
            Printcontrol.loadspreaddetails(Fpspread1, pagename, student);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popupstudaddinl.Visible = false;
    }
    protected void btn_rollno_Click(object sender, EventArgs e)
    {
        try
        {
            lbl_errormsg1.Visible = false;
            popupselectstd.Visible = true;
            bindhostelname1();
            bindbatch();
            binddegree();
            bindbranch(college);
            //  Div1.Visible = false;
            Fpspread2.Visible = false;
            btn_ok.Visible = false;
            btn_exit1.Visible = false;
            txt_rollno1.Text = "";
            lbl_count.Visible = false;
        }
        catch
        {
        }
    }
    public void description()
    {
        try
        {
            string headerquery = "";
            ddl_description.Items.Clear();
            headerquery = "select distinct MasterValue,MasterCode from HT_StudAdditionalDet h,CO_MasterValues m where h.AdditionalDesc=m.MasterCode and CollegeCode='" + collegecode1 + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_description.DataSource = ds;
                ddl_description.DataTextField = "MasterValue";
                ddl_description.DataValueField = "MasterCode";
                ddl_description.DataBind();
                ddl_description.Items.Insert(0, "Select");
                ddl_description.Items.Insert(ddl_description.Items.Count, "Others");
            }
            else
            {
                ddl_description.Items.Insert(0, "Select");
                ddl_description.Items.Insert(ddl_description.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + collegecode1 + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + collegecode1 + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        string expnc = "";
        int ins = 0;
        if (ddl_description.SelectedItem.Value != "Select")
        {
            if (ddl_description.SelectedItem.Value != "Others")
            {
                expnc = Convert.ToString(ddl_description.SelectedItem.Value);
            }
            else
            {
                string doc_prty1 = Convert.ToString(txt_description.Text);
                expnc = subjectcodenew("Expense", doc_prty1);
            }
        }
       
        date = Convert.ToString(txt_date.Text);
        string[] splitdate = date.Split('-');
        splitdate = splitdate[0].Split('/');
        DateTime dt = new DateTime();
        if (splitdate.Length > 0)
        {
            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
        }
        regno = Convert.ToString(txt_regno.Text);
        if (txt_rollno.Visible == true)
        {
            rollno = Convert.ToString(txt_rollno.Text);
        }
        if (txt_stu.Visible == true)
        {
            rollno = Convert.ToString(lbl_Sturollno.Text);
        }
        name = Convert.ToString(txt_name.Text);
        degree = Convert.ToString(txt_degree.Text);
        hostlnm = Convert.ToString(txt_hostelname1.Text);
        desc = Convert.ToString(ddl_description.SelectedItem.Value);
        amount = Convert.ToString(txt_amount.Text);
      
        //Added By Saranyadevi 14.2.2018
        string rollnum = string.Empty;
        string[] split = rollno.Split(';');
        if (split.Length > 0)
        {
            for (int i = 0; i < split.Length; i++)
            {
                rollnum = Convert.ToString(split[i]);
                if (!string.IsNullOrEmpty(rollnum) && !string.IsNullOrEmpty(expnc) && !string.IsNullOrEmpty(amount))
                {
                    string app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + rollnum + "'");
                    string sql = "";
                    ds.Clear();
                    //sql = " insert into HT_StudAdditionalDet(MemType,App_No,TransDate,AdditionalAmt,AdditionalDesc) values('1','" + app_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                    sql = "if exists (select * from HT_StudAdditionalDet where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and AdditionalDesc='" + expnc + "') update HT_StudAdditionalDet set MemType='1',App_No='" + app_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',AdditionalAmt='" + amount + "',AdditionalDesc='" + expnc + "' where App_No='" + app_no + "' and TransDate='" + dt.ToString("MM/dd/yyyy") + "' and AdditionalDesc='" + expnc + "' else insert into HT_StudAdditionalDet(MemType,App_No,TransDate,AdditionalAmt,AdditionalDesc) values('1','" + app_no + "','" + dt.ToString("MM/dd/yyyy") + "','" + amount + "','" + expnc + "')";
                    ins = d2.update_method_wo_parameter(sql, "Text");
                }
            }
        }
        if (ins > 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Saved Successfully";
            lblstudent.Visible = false;
            lblstudentcount.Visible = false;
            btn_addnew_Click(sender, e);
            //btn_go_Click(sender, e);
            ////popupstudaddinl.Visible = false;
            //buttonvalue = "Save";
            //  savedetails();
            btn_go_Click(sender, e);
            txt_rollno.Text = "";
            txt_regno.Text = "";
            Txtid.Text = "";
            txt_name.Text = "";
            txt_degree.Text = "";
            txt_hostelname1.Text = "";
            txt_amount.Text = "";
            txt_stu.Text = "";
            lblstudent.Visible = false;
            lblstudentcount.Visible = false;
            ddl_description.SelectedItem.Text = "Select";
        }
    }
    //protected void savedetails()
    //{
    //    try
    //    {
    //        int iv;
    //        dtaccessdate = DateTime.Now.ToString();
    //        dtaccesstime = DateTime.Now.ToLongTimeString();
    //        regno = Convert.ToString(txt_regno.Text);
    //        rollno = Convert.ToString(txt_rollno.Text);
    //        name = Convert.ToString(txt_name.Text);
    //        degree = Convert.ToString(txt_degree.Text);
    //        hostlnm = Convert.ToString(txt_hostelname1.Text);
    //        desc = Convert.ToString(ddl_description.SelectedItem.Text);
    //        if (desc.Trim() == "Others")
    //        {
    //            desc = Convert.ToString(txt_description.Text);
    //        }
    //        date = Convert.ToString(txt_date.Text);
    //        string[] splitdate = date.Split('-');
    //        splitdate = splitdate[0].Split('/');
    //        DateTime dt = new DateTime();
    //        if (splitdate.Length > 0)
    //        {
    //            dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
    //        }
    //        getday = dt.ToString("MM/dd/yyyy");
    //        amount = Convert.ToString(txt_amount.Text);
    //        hostel = Convert.ToString(Session["hostelcode1"]);
    //        degreecode = Convert.ToString(Session["degreecode1"]);
    //        sql = "if exists (select * from StudentAdditional_Details  where Entry_Date ='" + dt.ToString("MM/dd/yyyy") + "' and College_Code ='" + collegecode1 + "' and  Roll_No ='" + rollno + "') update StudentAdditional_Details set Access_Date ='" + dtaccessdate + "' ,Access_Time ='" + dtaccesstime + "' ,Description ='" + desc + "'  ,Add_Amount ='" + amount + "' ,Hostel_Code ='" + hostel + "'  where Entry_Date ='" + dt.ToString("MM/dd/yyyy") + "'  and College_Code ='" + collegecode1 + "' and  Roll_No ='" + rollno + "' else  insert into StudentAdditional_Details (Access_Date,Access_Time,Roll_No  ,Description,Add_Amount,Hostel_Code,College_Code,Entry_Date)   values ('" + dtaccessdate + "','" + dtaccesstime + "','" + rollno + "'  ,'" + desc + "','" + amount + "','" + hostel + "','" + collegecode1 + "'  ,'" + dt.ToString("MM/dd/yyyy") + "')";
    //        iv = d2.update_method_wo_parameter(sql, "Text");
    //        if (iv != 0)
    //        {
    //            if (buttonvalue == "Save")
    //            {
    //                imgdiv2.Visible = true;
    //                lbl_alert.Text = "Saved Successfully";
    //                btn_addnew_Click(sender, e);
    //                btn_go_Click(sender, e);
    //                //popupstudaddinl.Visible = false;
    //            }
    //            else if (buttonvalue == "Update")
    //            {
    //                imgdiv2.Visible = true;
    //                lbl_alert.Text = "Updated Successfully";
    //                btn_go_Click(sender, e);
    //                popupstudaddinl.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            string expnc = "";
            if (ddl_description.SelectedItem.Value != "Select")
            {
                if (ddl_description.SelectedItem.Value != "Others")
                {
                    expnc = Convert.ToString(ddl_description.SelectedItem.Value);
                }
                else
                {
                    string doc_prty1 = Convert.ToString(txt_description.Text);
                    expnc = subjectcodenew("Expense", doc_prty1);
                }
            }
            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            regno = Convert.ToString(txt_regno.Text);
            ///rollno = Convert.ToString(txt_rollno.Text);
            name = Convert.ToString(txt_name.Text);
            degree = Convert.ToString(txt_degree.Text);
            hostlnm = Convert.ToString(txt_hostelname1.Text);
            desc = Convert.ToString(ddl_description.SelectedItem.Value);
            amount = Convert.ToString(txt_amount.Text);
            string app_no = d2.GetFunction("select app_no from Registration where Roll_No='" + txt_rollno.Text + "'");
            string sql = "";
            ds.Clear();
            sql = "  update HT_StudAdditionalDet set MemType ='1', App_No ='" + app_no + "',TransDate='" + dt.ToString("MM/dd/yyyy") + "',AdditionalAmt='" + amount + "' where  StudAdditionalpk ='" + Convert.ToString(Session["studd_addpk"]) + "' ";
            sql = sql + " update CO_MasterValues set  MasterValue='" + txt_description.Text + "'where  MasterCode='" + Convert.ToString(Session["desc_val"]) + "' ";
            //   AdditionalDesc='" + expnc + "'
            int ins = d2.update_method_wo_parameter(sql, "Text");
            ///  string description = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCode='" + valu + "'");
            imgdiv2.Visible = true;
            lbl_alert.Text = "Updated Successfully";
            // btn_addnew_Click(sender, e);
            //btn_go_Click(sender, e);
            ////popupstudaddinl.Visible = false;
            buttonvalue = "Update";
            //  savedetails();
            btn_go_Click(sender, e);
            txt_rollno.Text = "";
            txt_regno.Text = "";
            txt_name.Text = "";
            txt_degree.Text = "";
            txt_hostelname1.Text = "";
            txt_amount.Text = "";
            ddl_description.SelectedItem.Text = "Select";
            popupstudaddinl.Visible = false;
            //  savedetails();
        }
        catch (Exception ex)
        {
        }
    }
    protected void delete()
    {
        try
        {
            surediv.Visible = false;
            int y;
            string del = "";
            rollno = Convert.ToString(txt_rollno.Text);
            date = Convert.ToString(txt_date.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");
            //del = "delete StudentAdditional_Details where Roll_No ='" + rollno + "' and Entry_Date ='" + dt.ToString("MM/dd/yyyy") + "'";
            del = "delete HT_StudAdditionalDet where  StudAdditionalpk ='" + Convert.ToString(Session["studd_addpk"]) + "'";
            y = d2.update_method_wo_parameter(del, "Text");
            if (y != 0)
            {
                ds.Clear();
                bindhostelname1();
                btn_go_Click(sender, e);
                popupstudaddinl.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
            }
        }
        catch (Exception ex)
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
                lbl_sure.Text = "Do you want delete this record?";
            }
        }
        catch
        {
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        popupstudaddinl.Visible = false;
    }
    public void bindhostelname1()
    {
        try
        {
            ds.Clear();
            cbl_hostelname2.Items.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname2.DataSource = ds;
                cbl_hostelname2.DataTextField = "HostelName";
                cbl_hostelname2.DataValueField = "HostelMasterPK";
                cbl_hostelname2.DataBind();
                if (cbl_hostelname2.Items.Count > 0)
                {
                    for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                    {
                        cbl_hostelname2.Items[i].Selected = true;
                        if (hostel_name_code == "")
                        {
                            hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                        }
                        else
                        {
                            hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                        }
                    }
                    txt_hostelname2.Text = "Hostel(" + cbl_hostelname2.Items.Count + ")";
                }
            }
            else
            {
                txt_hostelname2.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    protected void cbl_hostelname2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_hostelname2.Text = "--Select--";
            cb_hostelname2.Checked = false;
            commcount = 0;
            for (i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname2.Text = "Hostel(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname2.Items.Count)
                {
                    cb_hostelname2.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    protected void cb_hostelname2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname2.Checked == true)
            {
                for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = true;
                    if (hostel_name_code == "")
                    {
                        hostel_name_code = Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                    else
                    {
                        hostel_name_code = hostel_name_code + "'" + "," + "'" + Convert.ToString(cbl_hostelname2.Items[i].Value);
                    }
                }
                txt_hostelname2.Text = "Hostel(" + (cbl_hostelname2.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = false;
                }
                txt_hostelname2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            hat.Clear();
            //string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            //ds = d2.select_method(sqlyear, hat, "Text");
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
    protected void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void binddegree()
    {
        try
        {
            ds.Clear();
            cbl_degree.Items.Clear();
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            //ds = d2.select_method_wo_parameter(query, "Text");
            ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (i = 0; i < cbl_degree.Items.Count; i++)
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
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree1.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree1.Text = "--Select--";
                txt_degree1.Text = "--Select--";
            }
            else
            {
                txt_degree1.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree1.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
            }
            else
            {
                for (i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree1.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            bindbranch(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            cbl_branch.Items.Clear();
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_degree.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (itemheader.Trim() != "")
            {
                ds = d2.select_method(commname, hat, "Text");
                //ds = d2.BindBranch(singleuser, group_user, cbl_degree.SelectedValue, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (i = 0; i < cbl_branch.Items.Count; i++)
                        {
                            cbl_branch.Items[i].Selected = true;
                        }
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }
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
        catch (Exception ex)
        {
        }
    }
    protected void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch.Items.Count)
                {
                    cb_branch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
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
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> getroll(string prefixText)
    //{
    //    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection sqlconn = new SqlConnection(cs))
    //    {
    //        sqlconn.Open();
    //        SqlCommand cmd = new SqlCommand("select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' ", sqlconn);
    //        cmd.Parameters.AddWithValue("@roll", prefixText);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        List<string> name = new List<string>();
    //        for (i = 0; i < dt.Rows.Count; i++)
    //        {
    //            name.Add(dt.Rows[i]["Roll_No"].ToString());
    //        }
    //        return name;
    //    }
    //}
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        if (hostel_name_code.Trim() != "")
        {
            query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and HostelMasterPK in ('" + hostel_name_code + "') and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";
            //query = "select distinct R.Roll_No from Registration r,Hostel_StudentDetails h where r.roll_no =h.roll_no and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Hostel_Code in ('" + hostel_name_code + "') and R.roll_no like '" + prefixText + "%' ";
        }
        else
        {
            query = "select distinct top 10 r.Roll_No from Registration as r join HT_HostelRegistration as hs on r.app_no=hs.APP_No join HM_HostelMaster  as hd on hs.HostelMasterFK=hd.HostelMasterPK where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc ";
        }
        name = ws.Getname(query);
        return name;
    }
    protected void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    if (hostel == "")
                    {
                        hostel = "" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        hostel = hostel + "'" + "," + "'" + cbl_hostelname2.Items[i].Value.ToString() + "";
                    }
                }
            }
            lbl_errormsg1.Visible = false;
            Fpspread2.SaveChanges();
            Fpspread2.DataBind();
            Fpspread2.CommandBar.Visible = false;
            // Fpspread2.Sheets[0].FrozenColumnCount = 2;
            Fpspread2.SheetCorner.ColumnCount = 0;
            Fpspread2.Sheets[0].RowCount = 1;
            Fpspread2.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            ds.Clear();
            if (txt_rollno1.Text != "")
            {
                //sql = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_No =hs.Roll_No and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.Degree_Code in ('" + itemheader + "') and hs.Hostel_Code in('" + hostel + "')";
                sql = " select r.APP_No,r.Reg_No,hs.id, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and isnull(IsDiscontinued,'0')=0 and isnull(IsSuspend,'0')=0  and isnull(IsVacated ,'0')=0   and r.Roll_No like  '" + txt_rollno1.Text + "' order by r.Roll_No";
            }
            else
            {
                sql = "select r.APP_No,r.Reg_No, hs.id, r.Roll_No,r.Roll_admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK  ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.APP_No and hs.HostelMasterfK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and isnull(IsDiscontinued,'0')=0 and isnull(IsSuspend,'0')=0  and isnull(IsVacated ,'0')=0   and d.Degree_Code in('" + itemheader + "') and hs.HostelMasterFK in('" + hostel + "') and r.Batch_Year='" + ddl_batch.SelectedValue + "' order by Roll_No";//add r.Batch_Year='"+ddl_batch.SelectedValue+"' magesh 16.3.18
            }
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread2.Sheets[0].AutoPostBack = false;
                //Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[0].Locked = true;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[1].Locked = true;
                Fpspread2.Columns[1].Width = 130;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[2].Locked = true;
                Fpspread2.Columns[2].Width = 130;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Id";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[3].Locked = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[4].Locked = true;
                Fpspread2.Columns[4].Width = 170;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                //Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread2.Sheets[0].Columns[4].Locked = true;
                //Fpspread2.Columns[4].Width = 400;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Hostel Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[5].Locked = true;
                Fpspread2.Columns[5].Width = 150;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Select";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[5].Width = 150;
                FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.CheckBoxCellType chk1 = new FarPoint.Web.Spread.CheckBoxCellType();
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpspread2.Width = 636;
                int studcount = 0;
                //magesh 10.4.18
                chk1.AutoPostBack = true;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].CellType = chk1;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                //for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    studcount++;
                //    Fpspread2.Sheets[0].RowCount++;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["department"]);
                //    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Name"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]);
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                //}
                int sno = 0;
                studcount = 0;
                for (int row1 = 0; row1 < cbl_branch.Items.Count; row1++)
                {
                    if (cbl_branch.Items[row1].Selected)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(cbl_branch.Items[row1].Value) + "'";
                        DataView dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            Fpspread2.Sheets[0].RowCount = Fpspread2.Sheets[0].RowCount + 1;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["Degree"]);
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].AddSpanCell(Fpspread2.Sheets[0].RowCount - 1, 0, 1, 6);
                            sno++;
                            for (int row = 0; row < dv.Count; row++)
                            {
                                studcount++;
                                // sno++;
                                Fpspread2.Sheets[0].RowCount++;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["Roll_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[row]["Degree"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dv[row]["Reg_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txt;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["id"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dv[row]["Stud_Name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["department"]);
                                ////Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dv[row]["HostelName"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(dv[row]["HostelMasterPK"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].CellType = chk;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            }
                        }
                    }
                }
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                // theivamani 14.11.15
                lbl_count.Visible = true;
                lbl_count.Text = "No of Student :" + studcount.ToString();
                Fpspread2.SaveChanges();
                Fpspread2.Width =750;
                //Div1.Visible = true;
                Fpspread2.Visible = true;
                btn_ok.Visible = true;
                btn_exit1.Visible = true;
            }
            else
            {
                // Div1.Visible = false;
                Fpspread2.Visible = false;
                lbl_errormsg1.Visible = true;
                //theivamani 14.11.15
                lbl_count.Visible = false;
                lbl_errormsg1.Text = "No Records Found";
                btn_ok.Visible = false;
                btn_exit1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_ok_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            string name = "";
            string degree = "";
            string degreecode = "";
            string selectStud = "";
            Fpspread2.SaveChanges();
            activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
            //Fpspread2.Sheets[0].AutoPostBack = true;
            //int activerow = 0;
            //activerow = Convert.ToInt32(Fpspread2.ActiveSheetView.ActiveRow.ToString());
            //Added By Saranyadevi 13.2.2018
            string rollno = string.Empty;
            string id = string.Empty;
            string StudentName = string.Empty;
            string hostelname = string.Empty;
            string selectrollno = string.Empty;
            string selectStudentName = string.Empty;
            string selecthostelname = string.Empty;
            ArrayList hostelnameArr = new ArrayList();
            int studCount = 0;
            Fpspread2.SaveChanges();
            for (int row = 0; row < Fpspread2.Sheets[0].RowCount; row++)
            {
                int selected = 0;
                int.TryParse(Convert.ToString(Fpspread2.Sheets[0].Cells[row, 6].Value), out selected);
                //magesh 10.4.18
                if (row == 0)
                    selected = 0;
                if (selected == 1)
                {
                    rollno = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 1].Text).Trim();
                    id = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 3].Text).Trim();
                    StudentName = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 4].Text).Trim();
                    hostelname = Convert.ToString(Fpspread2.Sheets[0].Cells[row, 5].Text).Trim();
                    if (String.IsNullOrEmpty(selectrollno))
                        selectrollno = rollno;
                    else
                        selectrollno += ";" + rollno;
                    if (String.IsNullOrEmpty(selectStudentName))
                        selectStudentName = StudentName;
                    else
                        selectStudentName += ";" + StudentName;
                    //if (String.IsNullOrEmpty(selecthostelname))
                    //    selecthostelname = hostelname;
                    //else
                    //    if (!selecthostelname.Contains(hostelname))
                    //    {
                    //        selecthostelname += ";" + hostelname;
                    //    }
                    if (String.IsNullOrEmpty(selecthostelname))
                    {
                        selecthostelname = hostelname;
                        hostelnameArr.Add(hostelname);
                    }
                    else
                        if (!hostelnameArr.Contains(hostelname))
                        {
                            selecthostelname += ";" + hostelname;
                            hostelnameArr.Add(hostelname);
                        }
                    studCount++;
                }
            }
            if (studCount == 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Please Select Any Student";
                return;
            }
            if (studCount > 1)
            {
                txt_hostelname1.Text = selecthostelname;
                lbl_Sturollno.Text = selectrollno;
                ViewState["NoOfStudents"] = studCount;
                lblstudent.Visible = true;
                lblstudentcount.Visible = true;
                string stucnt = Convert.ToString(ViewState["NoOfStudents"]);
                lblstudentcount.Text = stucnt;
                //clearpopup();
                txt_degree.Enabled = false;
                lbl_degree.Enabled = false;
                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                lbl_regno.Enabled = false;
                txt_regno.Enabled = false;
                lbl_name.Enabled = false;
                txt_name.Enabled = false;
                lbl_stu.Visible = true;
                lbl_Sturollno.Visible = false;
                txt_stu.Visible = true;
                txt_stu.Text = stucnt;
                popupselectstd.Visible = false;
            }
            else
            {
                lblstudent.Visible = false;
                lblstudentcount.Visible = false;
                rollno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_rollno.Text = rollno;
                id = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                Txtid.Text = id;
                regno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                txt_regno.Text = regno;
                name = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                txt_name.Text = name;
                degree = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                Session["degreecode1"] = Convert.ToString(degreecode);
                txt_degree.Text = degree;
                hostel = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow),5].Text;
                hostelcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                Session["hostelcode1"] = Convert.ToString(hostelcode);
                txt_hostelname1.Text = hostel;
                txt_degree.Enabled = true;
                lbl_degree.Enabled = true;
                lbl_rollno.Visible = true;
                txt_rollno.Visible = true;
                lbl_regno.Enabled = true;
                txt_regno.Enabled = true;
                lbl_name.Enabled = true;
                txt_name.Enabled = true;
                lbl_stu.Visible = false;
                lbl_Sturollno.Visible = false;
                txt_stu.Visible = false;
                popupselectstd.Visible = false;
                popupstudaddinl.Visible = true;
            }
            //for (i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
            //{
            //    if (i == Convert.ToInt32(activerow))
            //    {
            //        Fpspread2.Sheets[0].Rows[i].BackColor = Color.LightBlue;
            //        Fpspread2.Sheets[0].SelectionBackColor = Color.Orange;
            //        Fpspread2.Sheets[0].SelectionForeColor = Color.White;
            //    }
            //    else
            //    {
            //        Fpspread2.Sheets[0].Rows[i].BackColor = Color.White;
            //    }
            //}
            //if (activerow.Trim() != "" && activecol.Trim() != "")
            //{
            //    rollno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            //    txt_rollno.Text = rollno;
            //    regno = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            //    txt_regno.Text = regno;
            //    name = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            //    txt_name.Text = name;
            //    degree = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            //    degreecode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
            //    Session["degreecode1"] = Convert.ToString(degreecode);
            //    txt_degree.Text = degree;
            //    hostel = Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
            //    hostelcode = Convert.ToString(Fpspread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
            //    Session["hostelcode1"] = Convert.ToString(hostelcode);
            //    txt_hostelname1.Text = hostel;
            //    popupselectstd.Visible = false;
            //    popupstudaddinl.Visible = true;
            //}
        }
        catch (Exception ex)
        {
        }
     }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
        //surediv.Visible = false;
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
    }
    //public void degree()
    //{
    //    try
    //    {
    //        user_code = Session["usercode"].ToString();
    //        college_code = Session["collegecode"].ToString();
    //        singleuser = Session["single_user"].ToString();
    //        group_user = Session["group_code"].ToString();
    //        if (group_user.Contains(';'))
    //        {
    //            string[] group_semi = group_user.Split(';');
    //            group_user = group_semi[0].ToString();
    //        }
    //        hat.Clear();
    //        hat.Add("single_user", singleuser.ToString());
    //        hat.Add("group_code", group_user);
    //        hat.Add("college_code", college_code);
    //        hat.Add("user_code", user_code);
    //        ds = d2.select_method("bind_degree", hat, "sp");
    //        int count1 = ds.Tables[0].Rows.Count;
    //        if (count1 > 0)
    //        {
    //            cbl_degree.DataSource = ds;
    //            cbl_degree.DataTextField = "course_name";
    //            cbl_degree.DataValueField = "course_id";
    //            cbl_degree.DataBind();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //} 
    //theivamani 14.11.15
    protected void txt_rollno_txtchange(object sender, EventArgs e)
    {
        try
        {
            string rollno = Convert.ToString(txt_rollno.Text);
            //string selectquery = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_No =hs.Roll_No and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.Roll_No ='" + txt_rollno.Text + "'";
            string selectquery = " select r.Roll_No,r.Reg_No,r.Roll_Admit,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.HostelMasterPK ,hd.HostelName  from Registration r,HM_HostelMaster hd,HT_HostelRegistration hs,Degree d,Department dt,Course c where r.App_No =hs.App_No and hs.HostelMasterFK =hd.HostelMasterPK and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No = '" + txt_rollno.Text + "'";
            //   Tables[0].Rows[rolcount]["App_No"]
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                rollno = Convert.ToString(txt_rollno.Text);
                string regno = Convert.ToString(ds.Tables[0].Rows[0]["Reg_No"]);
                string stuname = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                string deg = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                string hostelname = Convert.ToString(ds.Tables[0].Rows[0]["HostelName"]);
                txt_regno.Text = regno;
                txt_name.Text = stuname;
                txt_degree.Text = deg;
                txt_hostelname1.Text = hostelname;
            }
        }
        catch
        {
        }
    }
    //protected void Fpspread2_CellClick(object sender, EventArgs e)
    //{
    //    try
    //    {
    //            Cellclick = true;
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    //protected void Fpspread2_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Cellclick == true)
    //    {
    //        try
    //        {
    //            int activerow = 0;
    //            activerow = Convert.ToInt32(Fpspread2.ActiveSheetView.ActiveRow.ToString());
    //            for (i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
    //            {
    //                if (i == Convert.ToInt32(activerow))
    //                {
    //                    Fpspread2.Sheets[0].Rows[i].BackColor = Color.LightBlue;
    //                    Fpspread2.Sheets[0].SelectionBackColor = Color.Orange;
    //                    Fpspread2.Sheets[0].SelectionForeColor = Color.White;
    //                }
    //                else
    //                {
    //                    Fpspread2.Sheets[0].Rows[i].BackColor = Color.White;
    //                }
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //        }
    //    }
    //}
    protected void txt_fromdate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void txt_todate_Textchanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_todate.Text != "" && txt_fromdate.Text != "")
            {
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Text = "Enter ToDate greater than or equal to the FromDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    //magesh 10.4.18
    protected void Fpspread2_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = Convert.ToString(e.SheetView.ActiveRow).Trim();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(Fpspread2.Sheets[0].RowCount); j++)
            {
                string actcol = Convert.ToString(e.SheetView.ActiveColumn).Trim();
                string seltext = Convert.ToString(e.EditValues[Convert.ToInt16(actcol)]).Trim();
                if (seltext != "System.Object")
                    Fpspread2.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = Convert.ToString(seltext).Trim();
            }
            flag_true = true;
        }
    }
}