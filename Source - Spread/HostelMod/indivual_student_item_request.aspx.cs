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

public partial class indivual_student_item_request : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    string college = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string Hostelcode = "";
    string course_id = string.Empty;

    DAccess2 da = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

    bool check = false;
    int commcount;
    string itemheader = "";
    string commname = "";
    string hostel = "";
    string hostelcode = "";
    string sql = "";
    int spreadflag = 0;
    string studtype = "";

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

        if (!IsPostBack)
        {

            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;
            txt_date.Attributes.Add("readonly", "readonly");
            txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            rdb_both.Checked = true;
            both_OnChecked_Change(sender, e);
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindhostelname();
            // bindstudenttype();
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;
            btn_go1_Click(sender, e);
            rdb_hostelr.Checked = true;

            btn_ok.Visible = false;
            btn_exit2.Visible = false;
            Fpspread3.Sheets[0].RowCount = 0;
            Fpspread3.Sheets[0].ColumnCount = 0;
            Fpspread3.Visible = false;
            //  btn_spreaddelete.Visible = false;

        }
        lbl_error.Visible = false;

    }
    protected void cb_date_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_date.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        if (cb_date.Checked == false)
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
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
                    lbl_alerterr.Text = "Enter From Date Less than or Equal to the To Date";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
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
                    lbl_alerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }

            }
        }
        catch (Exception ex)
        {
        }

        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
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
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {

                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {

                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);
                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }

            }
            tborder.Text = colname12;
            if (ItemList.Count == 11)
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
    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string si = "";
            int j = 0;
            int i;
            if (CheckBox_column.Checked == true)
            {
                for (i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
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
    public void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void hosteler_OnChecked_Change(object sender, EventArgs e)
    {
        if (rdb_hos.Checked == true)
        {
            lbl_hostelname.Visible = true;
            txt_hostelname.Visible = true;
            Panel6.Visible = true;
            studtype = "Hostler";
            bindhostelname();
        }
        else
        {
            lbl_hostelname.Visible = false;
            txt_hostelname.Visible = false;
            Panel6.Visible = false;
        }
    }

    protected void daysscholor_OnChecked_Change(object sender, EventArgs e)
    {
        if (rdb_day.Checked == true)
        {
            lbl_hostelname.Visible = false;
            txt_hostelname.Visible = false;
            Panel6.Visible = false;
            studtype = "Day Scholar";
        }
    }
    protected void both_OnChecked_Change(object sender, EventArgs e)
    {
        if (rdb_both.Checked == true)
        {
            lbl_hostelname.Visible = true;
            txt_hostelname.Visible = true;
            Panel6.Visible = true;
            studtype = "Day Scholar','Hostler";
            bindhostelname();
        }
    }

    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            string hoscode = "";
            string batch = "";
            string degree = "";
            string branch = "";
            string semester = "";
            string section = "";
            string studtype = "";
            int index;
            string colno = "";
            int j = 0;

            //for batch
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    string batch1 = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = batch1;
                    }
                    else
                    {
                        batch = batch + "'" + "," + "'" + batch1;
                    }
                }
            }

            //for degree
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    string degree1 = cbl_degree.Items[i].Value.ToString();
                    if (degree == "")
                    {
                        degree = degree1;
                    }
                    else
                    {
                        degree = degree + "'" + "," + "'" + degree1;
                    }
                }
            }

            //for branch
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    string branch1 = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = branch1;
                    }
                    else
                    {
                        branch = branch + "'" + "," + "'" + branch1;
                    }
                }
            }

            //for semester
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    string semester1 = cbl_sem.Items[i].Text.ToString();
                    if (semester == "")
                    {
                        semester = semester1;
                    }
                    else
                    {
                        semester = semester + "'" + "," + "'" + semester1;
                    }
                }
            }

            //for section
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    string section1 = cbl_sec.Items[i].Value.ToString();
                    if (section == "")
                    {
                        section = section1;
                    }
                    else
                    {
                        section = section + "'" + "," + "'" + section1;
                    }
                }
            }

            //for hostelcode
            /*01.10.15*/
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                    if (hoscode == "")
                    {
                        hoscode = hoscode1;
                    }
                    else
                    {
                        hoscode = hoscode + "'" + "," + "'" + hoscode1;
                        Hostelcode = Convert.ToString(hoscode);
                    }
                }
            }

            //for student type
            //for (int i = 0; i < cbl_stutype.Items.Count; i++)
            //{
            //    if (cbl_stutype.Items[i].Selected == true)
            //    {
            //        string studtype1 = cbl_stutype.Items[i].Value.ToString();
            //        if (studtype == "")
            //        {
            //            studtype = studtype1;
            //        }
            //        else
            //        {
            //            studtype = studtype + "'" + "," + "'" + studtype1;
            //        }
            //    }
            //}


            if (rdb_both.Checked == true)
            {
                studtype = "Day Scholar','Hostler";
                //bindhostelname();
            }
            else if (rdb_hos.Checked == true)
            {
                studtype = "Hostler";
                //bindhostelname();
            }
            else if (rdb_day.Checked == true)
            {
                studtype = "Day Scholar";
            }


            Hashtable columnhash = new Hashtable();
            columnhash.Clear();

            int colinc = 0;
            columnhash.Add("Roll_No", "Roll No");
            columnhash.Add("Stud_Name", "Name");
            columnhash.Add("Stud_Type", "Student Type");
            columnhash.Add("TotItemQty", "Total No Of Item");
            columnhash.Add("ReqDate", "Request Date");
            columnhash.Add("Student_Mobile", "Mobile Number");
            columnhash.Add("parent_phnop", "Phone Number");
            columnhash.Add("Course_Name", "Degree");
            columnhash.Add("Dept_Name", "Branch");
            columnhash.Add("Current_Semester", "Semester");
            columnhash.Add("Sections", "Section");


            if (ItemList.Count == 0)
            {
                ItemList.Add("Roll_No");
                ItemList.Add("Stud_Name");
                ItemList.Add("Stud_Type");
                ItemList.Add("TotItemQty");
            }
            for (int i = 0; i <= 3; i++)
            {
                cblcolumnorder.Items[i].Selected = true;
                lnk_columnorder.Visible = true;

                //tborder.Visible = true;

            }
            cblcolumnorder_SelectedIndexChanged(sender, e);




            if (txt_rollnum.Text != "")
            {
                sql = "select r.Stud_Name,r.Roll_No,r.Stud_Type,convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, a.Student_Mobile,parent_phnop,ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir where r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and  ir.Roll_No='" + txt_rollnum.Text + "' and ir.AppStatus='0' and r.Stud_Type in('" + studtype + "') ";
            }
            else if (txt_name.Text != "")
            {
                sql = "select r.Stud_Name,r.Roll_No,r.Stud_Type,convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, a.Student_Mobile,parent_phnop,ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir where r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and r.Stud_Name='" + txt_name.Text + "' and ir.AppStatus='0' and r.Stud_Type in('" + studtype + "')";
            }
            else if (cb_date.Checked == true)
            {
                sql = "select r.Stud_Name,r.Roll_No,r.Stud_Type,convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, a.Student_Mobile,parent_phnop,ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir where r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and convert(varchar,convert(datetime,ir.ReqDate,103),103) between '" + txt_fromdate.Text + "' and '" + txt_todate.Text + "' and ir.AppStatus='0' and r.Stud_Type in('" + studtype + "')";
            }
            else
            {
                sql = "select r.Stud_Name,ir.Roll_No,r.Stud_Type,convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, a.Student_Mobile,parent_phnop,ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir where r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and a.batch_year in ('" + batch + "') and D.Degree_Code in('" + branch + "') and r.Current_Semester in('" + semester + "') and Sections in('" + section + "','') and r.Stud_Type in('" + studtype + "') and ir.AppStatus='0'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");

            if (txt_batch.Text.Trim() != "--Select--" && txt_degree.Text.Trim() != "--Select--" && txt_degree.Text.Trim() != "--Select--" && txt_branch.Text.Trim() != "--Select--" && txt_sem.Text.Trim() != "--Select--" && txt_sec.Text.Trim() != "--Select--")
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    pcolumnorder.Visible = true;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    //Fpspread1.Sheets[0].ColumnCount = 11;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;

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
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

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
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                        Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                        for (j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                                Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                                Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                                Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;
                            }
                        }
                    }
                    rptprint.Visible = true;
                    Fpspread1.Visible = true;
                    div1.Visible = true;
                    lbl_error.Visible = false;
                    pheaderfilter.Visible = true;
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                }
                else
                {
                    rptprint.Visible = false;
                    //imgdiv2.Visible = true;
                    lbl_error.Visible = true;
                    pheaderfilter.Visible = false;
                    lbl_error.Text = "No records found";
                    div1.Visible = false;
                    Fpspread1.Visible = false;

                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                rptprint.Visible = false;
                lbl_error.Visible = true;
                pheaderfilter.Visible = false;
                lbl_error.Text = "Please Select All Fields";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread_render(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                spreadflag = 1;
                Session["sflag"] = spreadflag;
                poperrjs.Visible = true;
                btn_save.Visible = false;
                btn_exit.Visible = false;
                btn_update.Visible = true;
                btn_delete.Visible = true;
                btn_spreaddelete.Visible = true;

                string activerow = "";
                string activecol = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                if (activerow.Trim() != "" && activecol != "0")
                {
                    string rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                    //string roll_admit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    // string studname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                    // string degree = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                    //string hosname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                    //string hoscode = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + hosname + "'");
                    txt_rollno.Text = Convert.ToString(rollnum);
                    //btn_go2_Click(sender, e);
                    string stud_type = d2.GetFunction("select Stud_Type from Registration where Roll_No='" + txt_rollno.Text + "'");
                    string query = "";
                    if (stud_type == "Day Scholar")
                    {
                        query = "select convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',ir.StudItemRequestMasterID,ir.TotItemQty,r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Stud_Type,r.Current_Semester,Sections,a.Student_Mobile,parent_phnop from Registration r,applyn a,Degree d,Department dt,Course c,StudItemRequestMaster as ir where r.Roll_No=ir.Roll_No and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code and dt.Dept_Code =d.Dept_Code  and d.Course_Id =c.Course_Id and ir.AppStatus='0' and ir.Roll_No ='" + txt_rollno.Text + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txt_name1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);

                            txt_degree1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);

                            txt_branch1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);

                            txt_sem1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);

                            txt_sec1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sections"]);

                            txt_mono.Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);

                            txt_phoneno.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);

                            txt_date.Text = Convert.ToString(ds.Tables[0].Rows[0]["ReqDate"]);

                            txt_totnoofitem.Text = Convert.ToString(ds.Tables[0].Rows[0]["TotItemQty"]);

                            int reqid = Convert.ToInt16(ds.Tables[0].Rows[0]["StudItemRequestMasterID"]);
                            Session["ReqID"] = reqid;

                            //string studtype = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]);
                            //if (studtype == rdb_dayscholar.Text)
                            //{
                            rdb_dayscholar.Enabled = true;
                            rdb_dayscholar.Checked = true;
                            rdb_hostelr.Enabled = false;
                            rdb_hostelr.Checked = false;
                            //}
                            //else if (studtype == rdb_hostelr.Text)
                            //{
                            //    rdb_hostelr.Enabled = true;
                            //    rdb_dayscholar.Enabled = false;
                            //}

                            string sql = "select * from StudItemRequestDetail ird,StudItemRequestMaster ir,StudItemMaster im,TextValTable tv where ird.StudItemRequestMasterID=ir.StudItemRequestMasterID and ird.StudItemMasterID=im.StudItemMasterID and im.StudItemCode=tv.TextCode and ir.AppStatus='0' and ird.AppStatus='0' and ir.Roll_No='" + txt_rollno.Text + "' and ir.TotItemQty='" + Convert.ToInt16(txt_totnoofitem.Text) + "' and convert(varchar,convert(datetime,ir.ReqDate,103),103)='" + txt_date.Text + "'";
                            loadspread2(sql);
                            popupselectstd.Visible = false;
                            poperrjs.Visible = true;
                            btn_rollno.Enabled = false;
                            txt_date.Enabled = false;
                        }
                    }
                    else if (stud_type == "Hostler")
                    {
                        query = "select convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',ir.StudItemRequestMasterID,ir.TotItemQty,r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Stud_Type,r.Current_Semester,Sections,a.Student_Mobile,parent_phnop,hd.Hostel_Name,hs.Room_Name from Registration r,applyn a,Degree d,Department dt,Course c,Hostel_StudentDetails hs,Hostel_Details hd,StudItemRequestMaster as ir where r.Roll_No=ir.Roll_No and r.Roll_Admit=hs.Roll_Admit and hs.Hostel_Code=hd.Hostel_Code and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and ir.Roll_No ='" + txt_rollno.Text + "' and ir.AppStatus='0'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(query, "Text");

                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txt_name1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);

                            txt_degree1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);

                            txt_branch1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);

                            txt_sem1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);

                            txt_sec1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sections"]);

                            txt_mono.Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);

                            txt_phoneno.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);

                            txt_hostelname1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Hostel_Name"]);

                            txt_roomno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);

                            txt_date.Text = Convert.ToString(ds.Tables[0].Rows[0]["ReqDate"]);

                            txt_totnoofitem.Text = Convert.ToString(ds.Tables[0].Rows[0]["TotItemQty"]);

                            int reqid = Convert.ToInt16(ds.Tables[0].Rows[0]["StudItemRequestMasterID"]);
                            Session["ReqID"] = reqid;

                            //string studtype = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]);
                            //if (studtype == rdb_dayscholar.Text)
                            //{
                            rdb_hostelr.Enabled = true;
                            rdb_hostelr.Checked = true;
                            rdb_dayscholar.Enabled = false;
                            rdb_dayscholar.Checked = false;
                            //}
                            //else if (studtype == rdb_hostelr.Text)
                            //{
                            //    rdb_hostelr.Enabled = true;
                            //    rdb_dayscholar.Enabled = false;
                            //}

                            string sql = "select * from StudItemRequestDetail ird,StudItemRequestMaster ir,StudItemMaster im,TextValTable tv where ird.StudItemRequestMasterID=ir.StudItemRequestMasterID and ird.StudItemMasterID=im.StudItemMasterID and im.StudItemCode=tv.TextCode and ir.AppStatus='0' and ird.AppStatus='0' and ir.Roll_No='" + txt_rollno.Text + "' and ir.TotItemQty='" + Convert.ToInt16(txt_totnoofitem.Text) + "' and convert(varchar,convert(datetime,ir.ReqDate,103),103)='" + txt_date.Text + "'";
                            loadspread2(sql);
                            popupselectstd.Visible = false;
                            poperrjs.Visible = true;
                            btn_rollno.Enabled = false;
                            txt_date.Enabled = false;
                        }

                    }


                }
                else
                {
                    poperrjs.Visible = false;
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Select any column from column order";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_rollno.Text != "" && txt_totnoofitem.Text != "" && txt_date.Text != "")
            {
                string sql = "";
                string sql1 = "";
                int query = 0;
                int query1 = 0;
                string itemname = "";
                string specification = "";
                int quantity = 0;
                string quantity1 = "";
                Fpspread2.SaveChanges();
                DateTime reqdate = new DateTime();
                string dt = Convert.ToString(txt_date.Text);

                string[] split = dt.Split('/');
                reqdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    if (itemname == "")
                    {
                        itemname = "" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }
                    else
                    {
                        itemname = itemname + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }
                }
                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    if (quantity1 == "")
                    {
                        quantity1 = "" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        quantity1 = quantity1 + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                }
                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    if (specification == "")
                    {
                        specification = "" + Fpspread2.Sheets[0].Cells[i, 4].Text + "";
                    }
                    else
                    {
                        specification = specification + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 4].Text + "";
                    }
                }

                string[] separators = { ",", "'" };
                string[] iname = itemname.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string[] qty = quantity1.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string[] ispec = specification.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                sql = "if exists (select * from StudItemRequestMaster where Roll_No='" + txt_rollno.Text + "' and ReqDate='" + reqdate + "' and AppStatus='0') update StudItemRequestMaster set Roll_No='" + txt_rollno.Text + "' , ReqDate='" + reqdate + "',TotItemQty='" + Convert.ToInt16(txt_totnoofitem.Text) + "',AppStatus='0' where Roll_No='" + txt_rollno.Text + "' and ReqDate='" + reqdate + "' and AppStatus='0'  else insert into StudItemRequestMaster(Roll_No,ReqDate,TotItemQty,AppStatus) values('" + txt_rollno.Text + "','" + reqdate + "','" + Convert.ToInt16(txt_totnoofitem.Text) + "','0')";
                query = d2.update_method_wo_parameter(sql, "TEXT");

                for (int i = 0; i < Convert.ToInt16(txt_totnoofitem.Text); i++)
                {
                    if (Fpspread2.Sheets[0].Cells[i, 2].Text == "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Select any item to request";
                    }
                    else if (Fpspread2.Sheets[0].Cells[i, 3].Text == "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Enter quantity for an item";
                    }
                    else if (Fpspread2.Sheets[0].Cells[i, 4].Text == "")
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Enter specification for an item";
                    }
                }
                for (int i = 0; i < Convert.ToInt16(txt_totnoofitem.Text); i++)
                {
                    string icode = d2.GetFunction("select TextCode from TextValTable where TextVal='" + iname[i].ToString() + "' and TextCriteria='Sitem'");
                    string itemid = d2.GetFunction("select StudItemMasterID from StudItemMaster where StudItemCode='" + icode + "'");
                    int iqty = Convert.ToInt16(qty[i]);
                    string ispecification = ispec[i];
                    int reqid = Convert.ToInt16(d2.GetFunction("select StudItemRequestMasterID from StudItemRequestMaster where AppStatus='0' and Roll_No='" + txt_rollno.Text + "' and ReqDate='" + reqdate + "'"));
                    if (reqid != 0 && query != 0)
                    {
                        sql1 = "if exists (select * from StudItemRequestDetail where StudItemRequestMasterID='" + reqid + "' and StudItemMasterID='" + itemid + "' and AppStatus='0') update StudItemRequestDetail set StudItemRequestMasterID='" + reqid + "',StudItemMasterID='" + itemid + "',StudItemReqQty='" + iqty + "',StudItemSpec='" + ispecification + "',AppStatus='0' where StudItemRequestMasterID='" + reqid + "' and StudItemMasterID='" + itemid + "' and AppStatus='0' else insert into StudItemRequestDetail (StudItemRequestMasterID,StudItemMasterID,StudItemReqQty,StudItemSpec,AppStatus) values('" + reqid + "','" + itemid + "','" + iqty + "','" + ispecification + "','0')";
                        //if exists (select * from StudItemRequestDetail where StudItemRequestMasterID='"+reqid+"' and AppStatus='0') update StudItemRequestDetail set StudItemRequestMasterID='"+reqid+"',StudItemMasterID='"+itemid+"',StudItemReqQty='"+iqty+"',StudItemSpec='"+ispecification+"',AppStatus='0' where StudItemRequestMasterID='"+reqid+"' and AppStatus='0' else
                        query1 = d2.update_method_wo_parameter(sql1, "TEXT");
                    }
                }
                if (query != 0 && query1 != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Updated Successfully";
                    //clearaddnewpopup();
                    poperrjs.Visible = false;
                    btn_go_Click(sender, e);
                }
            }
            else if (txt_totnoofitem.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Select any item by click on ? button";
            }
            else if (txt_rollno.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Select any student by click on ? button";
            }
            else if (txt_date.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Select the date";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_spreaddelete_Click(object sender, EventArgs e)
    {
        try
        {
            string itemname = "";
            int delete = 0;
            spreadflag = Convert.ToInt16(Session["sflag"]);
            for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
            {
                Fpspread2.SaveChanges();
                int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                if (checkval == 1)
                {
                    if (itemname == "")
                    {
                        itemname = "" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }
                    else
                    {
                        itemname = itemname + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }

                    string[] separators = { ",", "'" };
                    string[] iname = itemname.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string sql = "";
                    if (spreadflag != 0)
                    {
                        int reqid = Convert.ToInt16(Session["ReqID"].ToString());

                        for (int j = 0; j < iname.Length; j++)
                        {
                            int itemid = Convert.ToInt16(d2.GetFunction("select ird.StudItemMasterID from StudItemMaster im,TextValTable tv,StudItemRequestDetail ird where im.StudItemCode=tv.TextCode and im.StudItemMasterID=ird.StudItemMasterID and tv.TextVal='" + iname[j] + "' and ird.AppStatus='0'"));

                            string sql1 = "delete from StudItemRequestDetail where StudItemRequestMasterID='" + reqid + "' and AppStatus='0' and StudItemMasterID='" + itemid + "'";
                            delete = d2.update_method_wo_parameter(sql1, "TEXT");

                        }
                        if (delete != 0)
                        {
                            //  Fpspread2.Sheets[0].RemoveRows(Convert.ToInt32(Fpspread2.Sheets[0].Cells[i,0].Value), iname.Length);
                            int totitem = Convert.ToInt16(txt_totnoofitem.Text) - delete;
                            txt_totnoofitem.Text = Convert.ToString(totitem);
                            d2.update_method_wo_parameter("update StudItemRequestMaster set TotItemQty='" + Convert.ToInt16(txt_totnoofitem.Text) + "' where StudItemRequestMasterID='" + reqid + "' and AppStatus='0'", "TEXT");
                            Fpspread2.Sheets[0].RemoveRows(i, delete);
                        }
                    }
                    else
                    {
                        int totitem = Convert.ToInt16(txt_totnoofitem.Text) - iname.Length;
                        txt_totnoofitem.Text = Convert.ToString(totitem);
                        Fpspread2.Sheets[0].RemoveRows(i, 1);
                    }
                }
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
            int reqid = Convert.ToInt16(Session["ReqID"].ToString());
            string sql = "delete from StudItemRequestMaster where StudItemRequestMasterID='" + reqid + "' and Roll_No='" + txt_rollno.Text + "' and AppStatus='0'";
            string sql1 = "delete from StudItemRequestDetail where StudItemRequestMasterID='" + reqid + "' and AppStatus='0'"; //ir,StudItemRequestDetail ird ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            int delete1 = d2.update_method_wo_parameter(sql1, "TEXT");
            if (delete != 0 && delete1 != 0)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Deleted Successfully";
                poperrjs.Visible = false;

                //  btn_go_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        WebService ws = new WebService();
        string query1 = "";
        List<string> name = new List<string>();

        // if (Hostelcode.Trim() != "")
        //{
        //    query1 = "select R.stud_name from Registration r,Hostel_StudentDetails h where r.Roll_Admit =h.Roll_Admit and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Hostel_Code in ('" + Hostelcode + "') and R.stud_name like '" + prefixText + "%' ";
        //}
        //else
        //{
        query1 = "select stud_name from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Stud_Type in('Day Scholar','Hostler') and stud_name like '" + prefixText + "%'";
        //query1 = "select R.stud_name  from Registration r,Hostel_StudentDetails h where r.Roll_Admit =h.Roll_Admit and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and r.Stud_Type in('Day Scholar','Hostler') and R.stud_name  like '" + prefixText + "%' ";
        //}    
        name = ws.Getname(query1);

        return name;
    }
    public void rdb_dayscholar_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_dayscholar.Checked == true)
        {
            lbl_hostelname1.Visible = false;
            txt_hostelname1.Visible = false;
            lbl_roomno.Visible = false;
            txt_roomno.Visible = false;
            txt_rollno.Text = "";
            txt_name1.Text = "";
            txt_degree1.Text = "";
            txt_branch1.Text = "";
            txt_sem1.Text = "";
            txt_sec1.Text = "";
            txt_mono.Text = "";
            txt_phoneno.Text = "";
        }

        else
        {
        }
    }
    public void rdb_hostelr_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_hostelr.Checked == true)
        {
            lbl_hostelname1.Visible = true;
            txt_hostelname1.Visible = true;
            lbl_roomno.Visible = true;
            txt_roomno.Visible = true;
        }
        else
        {
        }
    }
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                count++;
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
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();

        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";


            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_batch.Checked = false;
                    build = cbl_batch.Items[i].Value.ToString();
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


            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();

        }
        catch (Exception ex)
        {

        }
    }
    public void BindBatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (int row = 0; row < cbl_batch.Items.Count; row++)
                {
                    cbl_batch.Items[row].Selected = true;
                }
                txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                cb_batch.Checked = true;
            }
            else
            {
                txt_batch.Text = "--Select--";
            }
        }
        catch
        {
        }

    }
    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_degree.Text = "--Select--";

            if (cb_degree.Checked == true)
            {
                count++;
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
                    //txt_degree.Text = "--Select--";
                    //txtbranch.Text = "--Select--";
                    //chklstbranch.ClearSelection();
                    //chkbranch.Checked = false;
                }
                txt_degree.Text = "--Select--";
            }


            bindbranch();
            bindsem();
            bindsec();
            // bindhostelname();

        }
        catch (Exception ex)
        {
        }

    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            int i = 0;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
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
                txt_degree.Text = "Degree (" + commcount.ToString() + ")";
            }


            bindbranch();
            bindsem();
            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }

    }
    public void BindDegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string build = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
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

                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_CheckedChanged(object sender, EventArgs e)
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


            bindsem();
            bindsec();
            //bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_sem.Items.Clear();

            int commcount = 0;
            cb_branch.Checked = false;
            txt_branch.Text = "--Select--";
            int commcount1 = 0;

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

            bindsem();
            bindsec();
            //bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            cbl_branch.Items.Clear();
            string course_id = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int row = 0; row < cbl_degree.Items.Count; row++)
                {
                    if (cbl_degree.Items[row].Selected == true)
                    {
                        if (course_id == "")
                        {
                            course_id = Convert.ToString(cbl_degree.Items[row].Value);
                        }
                        else
                        {
                            course_id = course_id + "," + Convert.ToString(cbl_degree.Items[row].Value);
                        }
                    }
                }

            }
            if (course_id != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
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
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_branch.Checked = false;
                txt_branch.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {

                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";

            }

            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }

    }

    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds1.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {

                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;

                    }
                }
            }
        }
        build = "";
        if (cbl_batch.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {

                if (cbl_batch.Items[i].Selected == true)
                {
                    build = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = build;
                    }
                    else
                    {
                        batch = batch + "," + build;

                    }

                }
            }

        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            ds1 = d2.BindSem(branch, batch, collegecode1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds1.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = "Sem(" + cbl_sem.Items.Count + ")";
                }
            }
        }

    }
    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sec.Text = "--Select--";
            if (cb_sec.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Semester(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
            //bindhostelname();
        }


        catch (Exception ex)
        {

        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;

            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sec.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec.Items.Count)
                {

                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + commcount.ToString() + ")";

            }
            //bindhostelname();


        }

        catch (Exception ex)
        {

        }
    }
    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_sec.Text = "---Select---";
            cb_sec.Checked = false;
            string build = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(collegecode1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                        cb_sec.Checked = true;
                    }

                }
            }
            else
            {
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txt_hostelname.Text = "--Select--";
        if (cb_hostelname.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = true;
            }
            txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                cbl_hostelname.Items[i].Selected = false;
            }
        }
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_hostelname.Checked = false;
            int commcount = 0;

            txt_hostelname.Text = "--Select--";
            for (i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_hostelname.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_hostelname.Items.Count)
                {

                    cb_hostelname.Checked = true;
                }
                txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {

        }


    }
    public void bindhostelname()
    {
        try
        {
            ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "Hostel_Name";
                cbl_hostelname.DataValueField = "Hostel_code";
                cbl_hostelname.DataBind();
                if (cbl_hostelname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        cbl_hostelname.Items[i].Selected = true;
                    }
                    txt_hostelname.Text = "Hostel Name(" + cbl_hostelname.Items.Count + ")";
                    cb_hostelname.Checked = true;
                }
            }
        }
        catch
        {

        }

    }
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_rollno.Text != "" && txt_totnoofitem.Text != "" && txt_date.Text != "")
            {
                string sql = "";
                string sql1 = "";
                int query = 0;
                int query1 = 0;
                string itemname = "";
                string specification = "";
                int quantity = 0;
                string quantity1 = "";
                Fpspread2.SaveChanges();
                DateTime reqdate = new DateTime();
                string dt = Convert.ToString(txt_date.Text);

                string[] split = dt.Split('/');
                reqdate = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);

                for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
                {
                    if (itemname == "")
                    {
                        itemname = "" + Fpspread2.Sheets[0].Cells[i, 2].Tag + "";
                    }
                    else
                    {
                        itemname = itemname + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Tag + "";
                    }
                    if (quantity1 == "")
                    {
                        quantity1 = "" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        quantity1 = quantity1 + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                    if (specification == "")
                    {
                        specification = "" + Fpspread2.Sheets[0].Cells[i, 4].Text + "";
                    }
                    else
                    {
                        specification = specification + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 4].Text + "";
                    }
                }

                if (quantity1.Trim() != "" && specification.Trim() != "")
                {
                    string[] separators = { ",", "'" };
                    string[] iname = itemname.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] qty = quantity1.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] ispec = specification.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    sql = "insert into StudItemRequestMaster(Roll_No,ReqDate,TotItemQty,AppStatus) values('" + txt_rollno.Text + "','" + reqdate + "','" + Convert.ToInt16(txt_totnoofitem.Text) + "','0')";
                    query = d2.update_method_wo_parameter(sql, "TEXT");
                    for (int i = 0; i < Convert.ToInt16(txt_totnoofitem.Text); i++)
                    {
                        // string icode = d2.GetFunction("select TextCode from TextValTable where TextVal='" + iname[i].ToString() + "' and TextCriteria='Sitem'");
                        string itemid = d2.GetFunction("select StudItemMasterID from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem' and t.TextCode='" + iname[i] + "'");
                        string iqty = Convert.ToString(qty[i]);
                        string ispecification = ispec[i];
                        int reqid = Convert.ToInt16(d2.GetFunction("select StudItemRequestMasterID from StudItemRequestMaster where AppStatus='0' and Roll_No='" + txt_rollno.Text + "' and ReqDate='" + reqdate + "'"));
                        if (reqid != 0 && query != 0)
                        {
                            sql1 = " insert into StudItemRequestDetail (StudItemRequestMasterID,StudItemMasterID,StudItemReqQty,StudItemSpec,AppStatus) values('" + reqid + "','" + itemid + "','" + iqty + "','" + ispecification + "','0')";
                            //if exists (select * from StudItemRequestDetail where StudItemRequestMasterID='"+reqid+"' and AppStatus='0') update StudItemRequestDetail set StudItemRequestMasterID='"+reqid+"',StudItemMasterID='"+itemid+"',StudItemReqQty='"+iqty+"',StudItemSpec='"+ispecification+"',AppStatus='0' where StudItemRequestMasterID='"+reqid+"' and AppStatus='0' else
                            query1 = d2.update_method_wo_parameter(sql1, "TEXT");
                        }
                    }

                    if (query != 0 && query1 != 0)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Visible = true;
                        lbl_alerterr.Text = "Saved Successfully";
                        clearaddnewpopup();
                        btn_go1_Click(sender, e);
                        //poperrjs.Visible = false;
                        //btn_go_Click(sender, e);
                    }
                    else
                    {

                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Visible = true;
                    lbl_alerterr.Text = "Please enter quantity and specification";
                }
            }
            else
            {

                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "Please select all fields";
                imgdiv2.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        clearaddnewpopup();
        btn_go1_Click(sender, e);
        poperrjs.Visible = true;
        btn_save.Visible = true;
        btn_exit.Visible = true;
        btn_update.Visible = false;
        btn_delete.Visible = false;
        // btn_spreaddelete.Visible = false;
        rdb_hostelr.Checked = true;
        rdb_hostelr.Enabled = true;
        rdb_dayscholar.Checked = false;
        rdb_dayscholar.Enabled = true;
        btn_rollno.Enabled = true;
        txt_date.Enabled = true;
    }


    //***************************01.10.15*********************************

    //protected void cb_stutype_CheckedChanged(object sender, EventArgs e)
    //{
    //    int cout = 0;

    //    txt_stutype.Text = "--Select--";
    //    if (cb_stutype.Checked == true)
    //    {
    //        cout++;
    //        for (int i = 0; i < cbl_stutype.Items.Count; i++)
    //        {
    //            cbl_stutype.Items[i].Selected = true;
    //        }
    //        txt_stutype.Text = "Student Type(" + (cbl_stutype.Items.Count) + ")";
    //    }
    //    else
    //    {
    //        for (int i = 0; i < cbl_stutype.Items.Count; i++)
    //        {
    //            cbl_stutype.Items[i].Selected = false;
    //        }
    //    }
    //}
    //protected void cbl_stutype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int i = 0;
    //        cb_stutype.Checked = false;
    //        int commcount = 0;

    //        txt_stutype.Text = "--Select--";
    //        for (i = 0; i < cbl_stutype.Items.Count; i++)
    //        {
    //            if (cbl_stutype.Items[i].Selected == true)
    //            {
    //                commcount = commcount + 1;
    //                cb_stutype.Checked = false;
    //            }
    //        }
    //        if (commcount > 0)
    //        {
    //            if (commcount == cbl_stutype.Items.Count)
    //            {

    //                cb_stutype.Checked = true;
    //            }
    //            txt_stutype.Text = "Student Type(" + commcount.ToString() + ")";
    //        }
    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}
    //public void bindstudenttype()
    //{
    //    try
    //    {
    //        string selectquery = "select distinct Stud_Type  from Registration where isnull(Stud_Type,'')<>''";
    //        ds = d2.select_method_wo_parameter(selectquery, "Text");

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_stutype.DataSource = ds;
    //            cbl_stutype.DataTextField = "Stud_Type";
    //            cbl_stutype.DataValueField = "Stud_Type";
    //            cbl_stutype.DataBind();
    //            if (cbl_stutype.Items.Count > 0)
    //            {
    //                for (int i = 0; i < cbl_stutype.Items.Count; i++)
    //                {
    //                    cbl_stutype.Items[i].Selected = true;
    //                }
    //                txt_stutype.Text = "Student Type(" + cbl_stutype.Items.Count + ")";
    //                cb_stutype.Checked = true;
    //            }
    //        }
    //    }
    //    catch
    //    {

    //    }
    //}
    //***************************************************************
    protected void btn_rollno_click(object sender, EventArgs e)
    {
        try
        {

            bindhostelname1();
            bindbatch1();
            binddegree1();
            bindbranch1(college);
            lbl_errormsg1.Visible = false;
            popupselectstd.Visible = true;

            Fpspread3.Visible = false;
            btn_ok.Visible = false;
            btn_exit2.Visible = false;
            txt_rollno1.Text = "";
            lbl_errorsearch1.Visible = false;
            if (rdb_dayscholar.Checked == true)
            {
                hos.Visible = false;
                hos1.Visible = false;
                lbl_hostelname2.Visible = false;
                txt_hostelname2.Visible = false;
                phstlnm.Visible = false;
            }
            else
            {
                hos.Visible = true;
                hos1.Visible = true;
                lbl_hostelname2.Visible = true;
                txt_hostelname2.Visible = true;
                phstlnm.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btn_totnoofitem_Click(object sender, EventArgs e)
    {

        binditemname();
        txt_itemsearch.Text = "";
        if (Fpspread2.Sheets[0].RowCount != 0)
        {
            itemnamediv.Visible = true;
            btn_go3_Click(sender, e);
        }
        else
        {
            itemnamediv.Visible = true;
            gvdatass.Visible = false;
            div2.Visible = false;
            btn_ok1.Visible = false;
            btn_exit3.Visible = false;
        }
        // btn_go3_Click(sender, e);
    }
    public void btn_go1_Click(object sender, EventArgs e)
    {
        try
        {
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 5;


            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[0].Width = 50;
            Fpspread2.Columns[0].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Left;
            Fpspread2.Columns[2].Width = 100;
            Fpspread2.Columns[2].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[3].Width = 100;
            Fpspread2.Columns[3].Locked = true;


            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Specification";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Left;
            Fpspread2.Columns[4].Width = 130;
            Fpspread2.Columns[4].Locked = true;

            FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
            db.ErrorMessage = "Only Allow Numbers between 1 to 10";
            db.MinimumValue = 1;
            db.MaximumValue = 10;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread2.Columns[1].Width = 50;
            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
            check.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
            check1.AutoPostBack = false;
            FarPoint.Web.Spread.TextCellType txtspecification = new FarPoint.Web.Spread.TextCellType();
            //for (int row = 0; row < 1; row++)
            //{
            //    Fpspread2.Sheets[0].RowCount++;
            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = db;
            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = txtspecification;
            //    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            //}
            Fpspread2.Visible = true;
            //div1.Visible = true;
            //lblerror.Visible = false;
            //btnsave.Visible = true;
            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }
    public void bindhostelname1()
    {
        try
        {
            ds.Clear();
            cbl_hostelname2.Items.Clear();
            //string itemname = "select Hostel_code,Hostel_Name  from Hostel_Details order by Hostel_code";
            //ds = d2.select_method_wo_parameter(itemname, "Text");
            ds = d2.BindHostel(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname2.DataSource = ds;
                cbl_hostelname2.DataTextField = "Hostel_Name";
                cbl_hostelname2.DataValueField = "Hostel_Code";
                cbl_hostelname2.DataBind();
                if (cbl_hostelname2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
                    {
                        cbl_hostelname2.Items[i].Selected = true;
                    }
                    txt_hostelname2.Text = "Hostel Name(" + cbl_hostelname2.Items.Count + ")";
                    cb_hostelname2.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cb_hostelname2_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname2.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
                {
                    cbl_hostelname2.Items[i].Selected = true;
                }
                txt_hostelname2.Text = "Hostel Name(" + cbl_hostelname2.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
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
    protected void cbl_hostelname2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_hostelname2.Text = "--Select--";
            cb_hostelname2.Checked = false;
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
            {
                if (cbl_hostelname2.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_hostelname2.Text = "Hostel Name(" + commcount.ToString() + ")";
                if (commcount == cbl_hostelname2.Items.Count)
                {
                    cb_hostelname2.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname2(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetItemName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = " select distinct TextVal from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem'";
        name = ws.Getname(query);
        return name;
    }
    public void bindbatch1()
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
    public void binddegree1()
    {
        try
        {
            ds.Clear();
            cbl_degree1.Items.Clear();
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            //ds = d2.select_method_wo_parameter(query, "Text");
            ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree1.DataSource = ds;
                cbl_degree1.DataTextField = "course_name";
                cbl_degree1.DataValueField = "course_id";
                cbl_degree1.DataBind();
                if (cbl_degree1.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree1.Items.Count; i++)
                    {
                        cbl_degree1.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree1.Items.Count + ")";
                    cb_degree1.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }
        }

        catch
        {

        }
    }
    public void cbl_degree1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            string build = "";
            string buildvalue = "";
            for (int i = 0; i < cbl_degree1.Items.Count; i++)
            {
                if (cbl_degree1.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch2.Text = "--Select--";
                    build = cbl_degree1.Items[i].Value.ToString();
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
            bindbranch1(buildvalue);
            if (seatcount == cbl_degree1.Items.Count)
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree1.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree2.Text = "--Select--";
                txt_degree2.Text = "--Select--";
            }
            else
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
            }
            // bindbranch(college);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree1_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree1.Checked == true)
            {
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    if (cb_degree1.Checked == true)
                    {
                        cbl_degree1.Items[i].Selected = true;
                        txt_degree2.Text = "Degree(" + (cbl_degree1.Items.Count) + ")";
                        build1 = cbl_degree1.Items[i].Value.ToString();
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
                bindbranch1(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree1.Items.Count; i++)
                {
                    cbl_degree1.Items[i].Selected = false;
                    txt_degree2.Text = "--Select--";
                    txt_branch2.Text = "--Select--";
                    cbl_degree1.ClearSelection();
                    cb_branch1.Checked = false;
                }
            }
            bindbranch1(college);
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch1(string branch)
    {
        try
        {
            //  cbl_degree1.Items.Clear();
            for (int i = 0; i < cbl_degree1.Items.Count; i++)
            {
                if (cbl_degree1.Items[i].Selected == true)
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
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();
                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                        cb_branch1.Checked = true;
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }

    }
    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_branch2.Text = "--Select--";
            cb_branch1.Checked = false;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch1.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch1_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            if (cb_branch1.Checked == true)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch2.Text = "Branch(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    public void binditemname()
    {
        try
        {
            ds.Clear();
            cbl_itemname.Items.Clear();
            ds = d2.select_method_wo_parameter("select distinct TextVal,TextCode from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem'", "TEXT");
            //d2.BindDegree(singleuser, group_user, collegecode1, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname.DataSource = ds;
                cbl_itemname.DataTextField = "TextVal";
                cbl_itemname.DataValueField = "TextCode";
                cbl_itemname.DataBind();
                if (cbl_itemname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemname.Items.Count; i++)
                    {
                        cbl_itemname.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Items(" + cbl_itemname.Items.Count + ")";
                    cb_itemname.Checked = true;
                }
                else
                {
                    txt_itemname.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }
        }

        catch
        {

        }
    }
    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        try
        {

            if (cb_itemname.Checked == true)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = true;
                }
                txt_itemname.Text = "Items(" + (cbl_itemname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    cbl_itemname.Items[i].Selected = false;
                }
                txt_itemname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void cbl_itemname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            commcount = 0;
            txt_itemname.Text = "--Select--";
            cb_itemname.Checked = false;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_itemname.Text = "Items(" + commcount.ToString() + ")";
                if (commcount == cbl_itemname.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            int sno = 0;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                }
            }
            for (int i = 0; i < cbl_hostelname2.Items.Count; i++)
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
            string batch = ddl_batch.SelectedItem.Text;
            lbl_errormsg1.Visible = false;
            Fpspread3.SaveChanges();
            Fpspread3.DataBind();
            Fpspread3.CommandBar.Visible = false;
            // Fpspread2.Sheets[0].FrozenColumnCount = 2;
            Fpspread3.SheetCorner.ColumnCount = 0;
            Fpspread3.Sheets[0].RowCount = 0;
            Fpspread3.Sheets[0].ColumnCount = 5;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            ds.Clear();
            if (rdb_hostelr.Checked == true)
            {
                if (txt_rollno1.Text == "")
                {
                    //sql = "select r.Roll_Admit,r.Roll_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and d.Degree_Code in ('" + itemheader + "') and hs.Hostel_Code in('" + hostel + "')";
                    sql = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and cc=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and d.Degree_Code in ('" + itemheader + "') and hs.Hostel_Code in('" + hostel + "') and r.Batch_Year='" + batch + "'";

                }
                else
                {
                    //sql = "select r.Roll_Admit,r.Roll_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and r.Roll_No ='" + txt_rollno1.Text + "'";
                    sql = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and cc=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and r.Roll_No ='" + txt_rollno1.Text + "' and r.Batch_Year='" + batch + "'";

                }
                Fpspread3.Width = 710;
            }
            else
            {
                if (txt_rollno1.Text == "")
                {
                    //sql = "select r.Roll_Admit,r.Roll_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and d.Degree_Code in ('" + itemheader + "') and hs.Hostel_Code in('" + hostel + "')";
                    sql = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and cc=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type <>'Hostler' and d.Degree_Code in('" + itemheader + "') and r.Batch_Year='" + batch + "'";

                }
                else
                {
                    //sql = "select r.Roll_Admit,r.Roll_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,hd.Hostel_code ,hd.Hostel_Name  from Registration r,Hostel_Details hd,Hostel_StudentDetails hs,Degree d,Department dt,Course c where r.Roll_Admit =hs.Roll_Admit and hs.Hostel_Code =hd.Hostel_code and r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type ='Hostler' and r.Roll_No ='" + txt_rollno1.Text + "'";
                    sql = "select r.Roll_No,r.Reg_No,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and cc=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and r.Stud_Type <>'Hostler' and r.Roll_No ='" + txt_rollno1.Text + "' and r.Batch_Year='" + batch + "'";

                }
                Fpspread3.Width = 710;
            }
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                Fpspread3.Sheets[0].AutoPostBack = false;
                //Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread3.Sheets[0].Columns[0].Locked = true;
                Fpspread3.Columns[0].Width = 50;

                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread3.Sheets[0].Columns[1].Locked = true;
                Fpspread3.Columns[1].Width = 130;

                //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                //Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread3.Sheets[0].Columns[2].Locked = true;
                //Fpspread3.Columns[2].Width = 130;

                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread3.Sheets[0].Columns[2].Locked = true;
                Fpspread3.Columns[2].Width = 170;

                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Degree";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread3.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread3.Sheets[0].Columns[3].Locked = true;
                Fpspread3.Columns[3].Width = 400;
                if (rdb_hostelr.Checked == true)
                {

                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Hostel Name";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpspread3.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].Columns[4].Locked = true;
                    Fpspread3.Sheets[0].Columns[4].Visible = true;
                    Fpspread3.Columns[4].Width = 150;
                    Fpspread3.Width = 710;
                }
                else
                {
                    Fpspread3.Sheets[0].Columns[4].Visible = false;
                    Fpspread3.Width = 710;
                }



                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    Fpspread3.Sheets[0].RowCount++;

                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";


                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Roll_No"]);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Reg_No"]);
                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    //Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    if (rdb_hostelr.Checked == true)
                    {
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Name"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["Hostel_Code"]);
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpspread3.Sheets[0].Cells[Fpspread3.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    }


                }
                //theivamani 6.11.15
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No of Student :" + sno.ToString();
                Fpspread3.Sheets[0].PageSize = Fpspread3.Sheets[0].RowCount;
                Fpspread3.SaveChanges();
                Fpspread3.Visible = true;
                btn_ok.Visible = true;
                btn_exit2.Visible = true;
                lbl_errormsg1.Visible = false;
            }
            else
            {
                Fpspread3.Visible = false;
                lbl_errormsg1.Visible = true;
                lbl_errormsg1.Text = "No Records Found";
                btn_ok.Visible = false;
                btn_exit2.Visible = false;
                lbl_errorsearch1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }

    }
    protected void btn_go3_Click(object sender, EventArgs e)
    {
        try
        {
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }
            ds.Clear();
            string sql = "";
            if (txt_itemsearch.Text != "")
            {
                sql = "select distinct TextVal,TextCode from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem' and TextVal='" + txt_itemsearch.Text + "'";
            }
            else
            {
                if (itemheader != "")
                {
                    sql = "select distinct TextVal,TextCode from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem' and TextCode in('" + itemheader + "')";
                }
                else
                {
                    sql = "select distinct TextVal,TextCode from TextValTable t,StudItemMaster s where t.TextCode=s.StudItemCode and TextCriteria='Sitem'";
                }
            }
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvdatass.DataSource = ds.Tables[0];
                gvdatass.DataBind();
                gvdatass.Visible = true;
                div2.Visible = true;
                btn_ok1.Visible = true;
                btn_exit3.Visible = true;
            }
            else
            {
                gvdatass.Visible = false;
                div2.Visible = false;
                btn_ok1.Visible = false;
                btn_exit3.Visible = false;
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "No records found";

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
            string rollno = "";

            Fpspread3.SaveChanges();
            activerow = Fpspread3.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread3.ActiveSheetView.ActiveColumn.ToString();


            for (int i = 0; i < Fpspread3.Sheets[0].RowCount; i++)
            {
                if (i == Convert.ToInt32(activerow))
                {

                    Fpspread3.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                    Fpspread3.Sheets[0].SelectionBackColor = Color.Orange;
                    Fpspread3.Sheets[0].SelectionForeColor = Color.White;
                }
                else
                {
                    Fpspread3.Sheets[0].Rows[i].BackColor = Color.White;
                }
            }
            if (activerow.Trim() != "" && activecol.Trim() != "")
            {

                rollno = Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_rollno.Text = rollno;
                string query = "select r.Stud_Name,r.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections,a.Student_Mobile,parent_phnop from Registration r,applyn a,Degree d,Department dt,Course c where r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and Roll_No ='" + rollno + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txt_name1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);

                    txt_degree1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);

                    txt_branch1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);

                    txt_sem1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);

                    txt_sec1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sections"]);

                    txt_mono.Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);

                    txt_phoneno.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);

                    hostel = Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                    hostelcode = Convert.ToString(Fpspread3.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                    Session["hostelcode1"] = Convert.ToString(hostelcode);
                    txt_hostelname1.Text = hostel;

                    popupselectstd.Visible = false;
                    poperrjs.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_ok1_Click(object sender, EventArgs e)
    {
        DataTable dt = new DataTable();
        DataRow dr;
        dt.Columns.Add("TextVal");
        dt.Columns.Add("TextCode");
        int count = 0;
        string itemname = "";
        string itemcode = "";
        foreach (DataListItem gvrow in gvdatass.Items)
        {
            CheckBox chkSelect = (gvrow.FindControl("CheckBox2") as CheckBox);
            if (chkSelect.Checked)
            {
                count++;

                Label lbl_itemname = (Label)gvrow.FindControl("lbl_itemname");
                itemname = lbl_itemname.Text;

                Label lbl_itemcode = (Label)gvrow.FindControl("lbl_itemcode");
                itemcode = lbl_itemcode.Text;

                dr = dt.NewRow();
                dt.Clear();
                dr[0] = Convert.ToString(itemname);
                dr[1] = Convert.ToString(itemcode);
                dt.Rows.Add(dr);
                if (dt.Rows.Count > 0)
                {
                    itemnamediv.Visible = false;
                }

                //Fpspread2.Sheets[0].RowCount = 0;
                //Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = false;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.Sheets[0].ColumnCount = 5;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[0].Width = 50;
                Fpspread2.Columns[0].Locked = true;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[2].Width = 100;
                Fpspread2.Columns[2].Locked = true;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[3].Width = 100;
                Fpspread2.Columns[3].Locked = false;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Specification";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Columns[4].Width = 130;
                Fpspread2.Columns[4].Locked = false;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                check.AutoPostBack = true;
                FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                check1.AutoPostBack = false;
                //Fpspread2.Sheets[0].RowCount++;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check;
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
                db.ErrorMessage = "Only Allow Numbers between 1 to 10";
                db.MinimumValue = 1;
                db.MaximumValue = 10;

                FarPoint.Web.Spread.TextCellType txtspecification = new FarPoint.Web.Spread.TextCellType();

                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    Fpspread2.Sheets[0].RowCount++;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Fpspread2.Sheets[0].RowCount);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check1;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dt.Rows[row]["TextVal"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dt.Rows[row]["TextCode"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = db;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].BackColor = Color.DarkGray;

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].CellType = txtspecification;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].BackColor = Color.DarkGray;


                }
                Fpspread2.Visible = true;
                txt_totnoofitem.Text = Convert.ToString(Fpspread2.Sheets[0].RowCount);
                //div1.Visible = true;
                //lblerror.Visible = false;
                //btnsave.Visible = true;
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
            }

        }
        if (count == 0)
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Please select any item";
        }

    }
    protected void btn_exit2_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    protected void btn_exit3_Click(object sender, EventArgs e)
    {
        itemnamediv.Visible = false;
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupselectstd.Visible = false;
    }
    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        itemnamediv.Visible = false;
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void clearaddnewpopup()
    {
        txt_rollno.Text = txt_totnoofitem.Text = txt_hostelname1.Text = txt_roomno.Text = txt_name1.Text = txt_degree1.Text = "";
        txt_branch1.Text = txt_sem1.Text = txt_sec1.Text = txt_mono.Text = txt_phoneno.Text = "";
        Fpspread2.Sheets[0].RowCount = 0;
        Fpspread2.Sheets[0].ColumnCount = 0;
    }
    public void loadspread2(string sqlcmd)
    {
        ds = d2.select_method_wo_parameter(sqlcmd, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 5;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[0].Width = 50;
            Fpspread2.Columns[0].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[2].Width = 100;
            Fpspread2.Columns[2].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[3].Width = 100;
            Fpspread2.Columns[3].Locked = false;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Specification";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[4].Width = 130;
            Fpspread2.Columns[4].Locked = false;

            FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
            db.ErrorMessage = "Only Allow Numbers between 1 to 10";
            db.MinimumValue = 1;
            db.MaximumValue = 10;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
            check.AutoPostBack = true;
            FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
            check1.AutoPostBack = false;

            FarPoint.Web.Spread.TextCellType txtspecification = new FarPoint.Web.Spread.TextCellType();

            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                Fpspread2.Sheets[0].RowCount++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = check1;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemReqQty"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = db;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemSpec"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].CellType = txtspecification;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

            }
            Fpspread2.Visible = true;
            //div1.Visible = true;
            //lblerror.Visible = false;
            //btnsave.Visible = true;
            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
        }

    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                da.printexcelreport(Fpspread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {

        }

    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Item Request Report";
            string pagename = "indivual_student_item_request.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}