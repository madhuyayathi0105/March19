using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Text;
using System.Configuration;
public partial class DeclarationForm : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    int i = 0;
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

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
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
                setLabelText();
                bindcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtfrmdate.Attributes.Add("readonly", "readonly");
                txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txttodate.Attributes.Add("readonly", "readonly");
                loadstream();
                loadedulevel();
                BindBatch();
                Bindcourse();
                binddept();
                loadsetting();
            }
        }
        catch (Exception ex) { }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("Default.aspx", false);
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(FpSpread, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblsmserror.Text = "";
            txtexcel.Text = "";
            string degreedetails = "Admission Print Format";
            string pagename = "AdmissionPrint.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            loadstream();
            loadedulevel();
            BindBatch();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    protected void edulevel_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            Bindcourse();
            binddept();
            if (ddledulevel.SelectedItem.Text == "UG")
            {
                cbatbtnme.Enabled = true;
                cbclgtme.Enabled = true;
                cbpartlang.Enabled = true;
                cbatbtnme.Checked = false;
                cbclgtme.Checked = false;
                cbpartlang.Checked = false;
                cbclgtme_OnCheckedChanged(sender, e);
            }
            else if (ddledulevel.SelectedItem.Text == "PG")
            {
                cbatbtnme.Enabled = false;
                cbclgtme.Enabled = true;
                cbpartlang.Enabled = false;
                cbatbtnme.Checked = false;
                cbclgtme.Checked = false;
                cbpartlang.Checked = false;
                cbclgtme_OnCheckedChanged(sender, e);
            }
        }
        catch { }
    }
    protected void batch_SelectedIndexChange(object sender, EventArgs e)
    {
    }
    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdegree.Checked == true)
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = true;
                }
                txt_degree.Text = lblDeg.Text + "(" + Convert.ToString(cbldegree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch { }
    }
    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            cbdegree.Checked = false;
            int count = 0;
            for (i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = lblDeg.Text + "(" + count + ")";
                if (count == cbldegree.Items.Count)
                {
                    cbdegree.Checked = true;
                }
            }
            binddept();
        }
        catch { }
    }
    protected void cbdepartment_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdepartment1.Checked == true)
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = true;
                }
                txt_department.Text = lblBran.Text + "(" + Convert.ToString(cbldepartment.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_department.Text = "--Select--";
            cbdepartment1.Checked = false;
            int count = 0;
            for (i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_department.Text = lblBran.Text + "(" + count + ")";
                if (count == cbldepartment.Items.Count)
                {
                    cbdepartment1.Checked = true;
                }
            }
        }
        catch { }
    }
    public bool checkok()
    {
        bool check = false;
        FpSpread.SaveChanges();
        try
        {
            for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
            {
                byte selval = Convert.ToByte(FpSpread.Sheets[0].Cells[i, 1].Value);
                if (selval == 1)
                {
                    check = true;
                }
            }
        }
        catch { }
        return check;
    }
    protected void Fpspread_command(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string selval = Convert.ToString(FpSpread.Sheets[0].Cells[0, 1].Value);
            if (selval == "1")
            {
                for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].Cells[i, 1].Value = 1;
                }
            }
            else
            {
                for (i = 1; i < FpSpread.Sheets[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].Cells[i, 1].Value = 0;
                }
            }
        }
        catch { }
    }
    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            string[] ay = txtfrmdate.Text.Split('/');
            string[] ay1 = txttodate.Text.Split('/');
            string currdate = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dt1 = new DateTime();
            DateTime dt2 = new DateTime();
            dt1 = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
            dt2 = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
            Printcontrol.Visible = false;
            string degreecode = "";
            string batchyear = "";
            string type = "";
            string edulevel = "";
            degreecode = GetSelectedItemsValueAsString(cbldepartment);
            batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
            type = Convert.ToString(ddltype.SelectedItem.Text);
            edulevel = Convert.ToString(ddledulevel.SelectedItem.Text);
            string selquery = "";
            string add = "";
            if (txt_searchstudname.Text != "")
            {
                add = "and stud_name='" + Convert.ToString(txt_searchstudname.Text) + "'";
            }
            else if (txt_searchappno.Text != "")
            {
                add = "and app_formno='" + Convert.ToString(txt_searchappno.Text) + "'";
            }
            if (txt_searchstudname.Text != "" || txt_searchappno.Text != "")
            {
                if (ddlstatus.SelectedItem.Text.Trim() == "Applied")
                {
                    selquery = " select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),date_applied,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='0' and isnull(Admission_Status,'0') ='0'  and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'  " + add + " order by c.Course_Id,d.Degree_Code,date_applied";
                }
                else if (ddlstatus.SelectedItem.Text.Trim() == "Shortlist")
                {
                    selquery = " select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),AdmitedDate,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='1' and isnull(Admission_Status,'0') ='0' and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'   " + add + " order by c.Course_Id,d.Degree_Code,AdmitedDate";
                }
                else if (ddlstatus.SelectedItem.Text.Trim() == "Admitted")
                {
                    selquery = "select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),AdmitedDate,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='1' and isnull(Admission_Status,'0')  ='1' and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'   " + add + "  order by c.Course_Id,d.Degree_Code,AdmitedDate";
                }
            }
            else
            {
                string degree = string.Empty;
                if (!string.IsNullOrEmpty(degreecode))
                    degree = " and d.Degree_Code in ('" + degreecode + "')";
                if (ddlstatus.SelectedItem.Text.Trim() == "Applied")
                {
                    selquery = " select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),date_applied,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='0' and isnull(Admission_Status,'0') ='0' and c.type ='" + type + "' and c.Edu_Level ='" + edulevel + "'  and date_applied between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'   " + add + " " + degree + " order by c.Course_Id,d.Degree_Code,date_applied";
                }
                else if (ddlstatus.SelectedItem.Text.Trim() == "Shortlist")
                {
                    selquery = " select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),AdmitedDate,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='1' and isnull(Admission_Status,'0') ='0' and c.type ='" + type + "' and c.Edu_Level ='" + edulevel + "'  and AdmitedDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'   " + add + "  " + degree + " order by c.Course_Id,d.Degree_Code,AdmitedDate";
                }
                else if (ddlstatus.SelectedItem.Text.Trim() == "Admitted")
                {
                    selquery = "select app_no,app_formno,stud_name,a.batch_year,(c.Course_Name +'-'+dt.Dept_Name)as Department,Student_Mobile,StuPer_Id,CONVERT(varchar(10),AdmitedDate,103) as AdmitedDate   from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and IsConfirm ='1' and ISNULL( selection_status,'0') ='1' and isnull(Admission_Status,'0')  ='1' and c.type ='" + type + "' and c.Edu_Level ='" + edulevel + "' and AdmitedDate between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "' and a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' " + add + "  " + degree + " order by c.Course_Id,d.Degree_Code,AdmitedDate";
                }
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region design
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 9;
                FpSpread.Sheets[0].AutoPostBack = false;
                FpSpread.CommandBar.Visible = false;
                FpSpread.Sheets[0].RowHeader.Visible = false;
                FpSpread.Sheets[0].FrozenRowCount = 1;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                FpSpread.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[0].Font.Bold = true;
                FpSpread.Columns[0].Locked = true;
                FpSpread.Columns[0].Width = 50;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[1].Font.Bold = true;
                FpSpread.Columns[1].Width = 80;
                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;
                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;
                FarPoint.Web.Spread.DoubleCellType txtmbl = new FarPoint.Web.Spread.DoubleCellType();
                FarPoint.Web.Spread.DoubleCellType appfromno = new FarPoint.Web.Spread.DoubleCellType();
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App Form No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[2].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[2].Font.Bold = true;
                FpSpread.Columns[2].Locked = true;
                FpSpread.Columns[2].Width = 125;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[3].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[3].Font.Bold = true;
                FpSpread.Columns[3].Locked = true;
                FpSpread.Columns[3].Width = 175;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch Year";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[4].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[4].Font.Bold = true;
                FpSpread.Columns[4].Locked = true;
                FpSpread.Columns[4].Width = 95;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblDeg.Text + "/" + lblBran.Text;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[5].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[5].Font.Bold = true;
                FpSpread.Columns[5].Locked = true;
                FpSpread.Columns[5].Width = 200;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mobile No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[6].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[6].Font.Bold = true;
                FpSpread.Columns[6].Locked = true;
                FpSpread.Columns[6].Width = 100;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Date";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[7].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[7].Font.Bold = true;
                FpSpread.Columns[7].Locked = true;
                FpSpread.Columns[7].Width = 105;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Email Id";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                FpSpread.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].Columns[8].Font.Size = FontUnit.Medium;
                FpSpread.Sheets[0].Columns[8].Font.Bold = true;
                FpSpread.Columns[8].Locked = true;
                FpSpread.Columns[8].Width = 237;
                #endregion
                FpSpread.Sheets[0].RowCount++;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                    FpSpread.Sheets[0].Cells[0, 1].CellType = chkall;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].CellType = appfromno;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Department"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["Student_Mobile"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].CellType = txtmbl;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["AdmitedDate"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["StuPer_Id"]);
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                mainpgeerr.Visible = false;
                FpSpread.Visible = true;
                //btncoverprint.Visible = true;
                //btninsurprnt.Visible = true;
                rprint.Visible = true;
                FpSpread.Height = 500;
                FpSpread.Width = 950;
                btn_pdf.Visible = true;
                lblsmserror.Text = "";
                txtexcel.Text = "";
            }
            else
            {
                FpSpread.Visible = false;
                rprint.Visible = false;
                mainpgeerr.Visible = true;
                btn_pdf.Visible = false;
                lblsmserror.Text = "";
                txtexcel.Text = "";
                mainpgeerr.Text = "No Record Found!";
            }
        }
        catch (Exception ex)
        { mainpgeerr.Visible = true; mainpgeerr.Text = ex.ToString(); }
    }
    protected void btninsurprnt_click(object sender, EventArgs e)
    {
        try
        {
            if (checkok() == true)
            {
            }
            else
            {
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "Please Select Any one Student!";
            }
        }
        catch { }
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
            ddl_collegename.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.Enabled = true;
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch (Exception e) { }
    }
    public void binddept()
    {
        try
        {
            cbldepartment.Items.Clear();
            string build = "";
            string build2 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            build2 = GetSelectedItemsValueAsString(cbldegree);
            if (build != "" && build2 != "")
            {
                string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + clgcode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                    if (cbldepartment.Items.Count > 0)
                    {
                        for (i = 0; i < cbldepartment.Items.Count; i++)
                        {
                            cbldepartment.Items[i].Selected = true;
                        }
                        cbdepartment1.Checked = true;
                        txt_department.Text = lblBran.Text + "(" + cbldepartment.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdepartment1.Checked = false;
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }
    public void loadstream()
    {
        try
        {
            ddltype.Items.Clear();
            collegecode1 = ddl_collegename.SelectedItem.Value;
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    public void loadedulevel()
    {
        try
        {
            ds.Clear();
            ddledulevel.Items.Clear();
            string itemheader = "";
            string deptquery = "";
            if (ddltype.Enabled)
            {
                itemheader = Convert.ToString(ddltype.SelectedItem.Value);
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + itemheader + "') and college_code in ('" + clgcode + "') order by Edu_Level desc";
            }
            else
            {
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in ('" + clgcode + "') order by Edu_Level desc";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddledulevel.DataSource = ds;
                ddledulevel.DataTextField = "Edu_Level";
                ddledulevel.DataBind();
            }
            Bindcourse();
            binddept();
        }
        catch { }
    }
    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch { }
    }
    public void Bindcourse()
    {
        try
        {
            cbldegree.Items.Clear();
            string build = "";
            string build1 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            if (build != "")
            {
                string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + clgcode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                if (ddltype.Enabled)
                {
                    build1 = Convert.ToString(ddltype.SelectedItem.Value);
                    deptquery = deptquery + " and type in ('" + build1 + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "course_name";
                    cbldegree.DataValueField = "course_id";
                    cbldegree.DataBind();
                    if (cbldegree.Items.Count > 0)
                    {
                        for (i = 0; i < cbldegree.Items.Count; i++)
                        {
                            cbldegree.Items[i].Selected = true;
                        }
                        cbdegree.Checked = true;
                        txt_degree.Text = lblDeg.Text + "(" + cbldegree.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdegree.Checked = false;
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch (Exception ex) { }
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int j = 0; j < cblSelected.Items.Count; j++)
            {
                if (cblSelected.Items[j].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[j].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[j].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    protected void btncoverprint_click(object sender, EventArgs e)
    {
        try
        {
            if (checkok() == true)
            {
                Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
                Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
                Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
                Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypage;
                for (i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    FpSpread.SaveChanges();
                    string val = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                    if (val == "1")
                    {
                        string appformno = Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text);
                        mypage = mydoc.NewPage();
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo.jpg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo.jpg"));
                            mypage.Add(LogoImage, 20, 20, 200);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/image/logo1.jpg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/image/logo1.jpg"));
                            mypage.Add(LogoImage, 500, 20, 200);
                        }
                        string collquery = "";
                        collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + collegecode1 + "";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(collquery, "Text");
                        string collegename = "";
                        string collegeaddress = "";
                        string collegedistrict = "";
                        string phonenumber = "";
                        string fax = "";
                        string email = "";
                        string website = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collegename = Convert.ToString(ds.Tables[0].Rows[0]["collname"]) + "(" + Convert.ToString(ds.Tables[0].Rows[0]["category"]) + ")";
                            collegeaddress = Convert.ToString(ds.Tables[0].Rows[0]["address1"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address2"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                            collegedistrict = Convert.ToString(ds.Tables[0].Rows[0]["district"]) + "," + Convert.ToString(ds.Tables[0].Rows[0]["state"]) + "-" + Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                            phonenumber = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                            fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]); ;
                            email = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                            website = Convert.ToString(ds.Tables[0].Rows[0]["website"]);
                        }
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 110, 10, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegename);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 20, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegeaddress);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 30, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, collegedistrict);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 40, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Phone No: " + phonenumber + ", Fax:" + fax);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 50, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, email);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 110, 60, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, website);
                        mypage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 110, 80, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, "Application Form for Insurance");
                        mypage.Add(ptc);
                        int y = 60;
                        int line1 = 50;
                        int line2 = 250;
                        string degreecode = GetSelectedItemsValueAsString(cbldepartment);
                        string getstudinfn = "select stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(getstudinfn, "Text");
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "1.Name");
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 70, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "2.Sex");
                            mypage.Add(ptc);
                            string gender = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "0")
                            {
                                gender = "Male";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "1")
                            {
                                gender = "Female";
                            }
                            else if (Convert.ToString(ds.Tables[0].Rows[0]["sex"]) == "2")
                            {
                                gender = "TransGender";
                            }
                            else
                            {
                                gender = "";
                            }
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 100, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(gender));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "3.Age&Date Of Birth");
                            mypage.Add(ptc);
                            string age = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["age"]) != "" && Convert.ToString(ds.Tables[0].Rows[0]["age"]) != null)
                            {
                                age = Convert.ToString(ds.Tables[0].Rows[0]["age"]);
                            }
                            else
                            {
                                if (Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != null && Convert.ToString(ds.Tables[0].Rows[0]["dob"]) != "")
                                {
                                    int curryear = Convert.ToInt32(DateTime.Now.Year);
                                    DateTime dt = Convert.ToDateTime(Convert.ToString(ds.Tables[0].Rows[0]["dob1"]));
                                    int dobyear = dt.Year;
                                    age = Convert.ToString(curryear - dobyear);
                                }
                            }
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(age));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 30, y + 130, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["dob"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "4.Blood Group");
                            mypage.Add(ptc);
                            string blood = "";
                            string bldgrp = d2.GetFunction("select TextVal from TextValTable where TextCriteria like 'bgrou' and college_code='" + collegecode1 + "' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]) + "'");
                            if (bldgrp.Trim() != "" && bldgrp.Trim() != "0")
                            {
                                blood = bldgrp;
                            }
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(blood));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 190, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "5.Identification Marks");
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 190, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["idmark"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 220, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "6.Course&Year Of Study");
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 60, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 150, y + 220, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 250, 350, 30), System.Drawing.ContentAlignment.MiddleLeft, "7.Name,Age,Occupation & Monthly Income Details:");
                            mypage.Add(ptc);
                            Gios.Pdf.PdfTable table2 = mydoc.NewTable(Fontsmall, 4, 5, 1);
                            table2 = mydoc.NewTable(Fontsmall, 4, 5, 1);
                            table2.VisibleHeaders = false;
                            table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table2.Columns[0].SetWidth(75);
                            table2.Columns[1].SetWidth(150);
                            table2.Columns[2].SetWidth(75);
                            table2.Columns[3].SetWidth(100);
                            table2.Columns[4].SetWidth(100);
                            table2.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 0).SetContent("Relation");
                            table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 1).SetContent("Name");
                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("D.O.B");
                            table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 3).SetContent("Occupation");
                            table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 4).SetContent("Monthly Income in Rs.");
                            table2.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 0).SetContent("Father");
                            table2.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(1, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]));
                            table2.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["fatherdob"]));
                            string fatheroccupation = "";
                            string getfatherocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='foccu' and college_code='" + collegecode1 + "'");
                            if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
                            {
                                fatheroccupation = getfatherocc;
                            }
                            table2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 3).SetContent(Convert.ToString(fatheroccupation));
                            string fatherinc = "";
                            string getfatherinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='fin' and college_code='" + collegecode1 + "'");
                            if (getfatherinc.Trim() != "" && getfatherinc.Trim() != "0")
                            {
                                fatherinc = getfatherinc;
                            }
                            table2.Cell(1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 4).SetContent(Convert.ToString(fatherinc));
                            table2.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 0).SetContent("Mother");
                            table2.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(2, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["mother"]));
                            table2.Cell(2, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["motherdob"]));
                            string motheroccupation = "";
                            string getmotherocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='moccu' and college_code='" + collegecode1 + "'");
                            if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
                            {
                                motheroccupation = getmotherocc;
                            }
                            table2.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 3).SetContent(Convert.ToString(getmotherocc));
                            string motherinc = "";
                            string getmotherinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='min' and college_code='" + collegecode1 + "'");
                            if (getmotherinc.Trim() != "" && getmotherinc.Trim() != "0")
                            {
                                motherinc = getmotherinc;
                            }
                            table2.Cell(2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 4).SetContent(Convert.ToString(motherinc));
                            table2.Cell(3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 0).SetContent("Guardian");
                            table2.Cell(3, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                            table2.Cell(3, 1).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["guardian_name"]));
                            table2.Cell(3, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 2).SetContent(Convert.ToString(ds.Tables[0].Rows[0]["Guardiandob"]));
                            string guardianoccupation = "";
                            string getguardianocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='moccu' and college_code='" + collegecode1 + "'");
                            if (getguardianocc.Trim() != "" && getguardianocc.Trim() != "0")
                            {
                                guardianoccupation = getguardianocc;
                            }
                            table2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 3).SetContent(Convert.ToString(guardianoccupation));
                            string guardianinc = "";
                            string getguardianinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='min' and college_code='" + collegecode1 + "'");
                            if (getguardianinc.Trim() != "" && getguardianinc.Trim() != "0")
                            {
                                guardianinc = getguardianinc;
                            }
                            table2.Cell(3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 4).SetContent(Convert.ToString(guardianinc));
                            Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, y + 280, 500, 550));
                            mypage.Add(myprov_pdfpage1);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 340, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "8.Residential Address");
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 340, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 370, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["Streetp"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 400, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["cityp"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2 + 90, y + 400, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_pincodep"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 430, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(ds.Tables[0].Rows[0]["parent_statep"]));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 460, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "9.Any Physical Disability");
                            mypage.Add(ptc);
                            string visualhandy = "";
                            if (Convert.ToString(ds.Tables[0].Rows[0]["visualhandy"]) == "0")
                            {
                                visualhandy = "No";
                            }
                            else
                            {
                                visualhandy = "Yes";
                            }
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line2, y + 460, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(visualhandy));
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, line1, y + 600, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date:");
                            mypage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, line2 + 100, y + 600, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Student");
                            mypage.Add(ptc);
                        }
                        mypage.SaveToDocument();
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "InsuranceFormat" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                    Response.End();
                }
                mainpgeerr.Visible = false;
            }
            else
            {
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "Please Select Any one Student!";
            }
        }
        catch { }
    }
    protected void btn_pdf_click(object sender, EventArgs e)
    {
        try
        {
            string edulevel = Convert.ToString(ddledulevel.SelectedItem.Text);
            if (edulevel.Trim() == "UG")
            {
                // ugPdfapplication();              
                pdfapplication();
            }
            else if (edulevel.Trim() == "PG")
            {
                pgPdfapplication();
            }
            else
            {
                MphilPdfapplication();
            }
        }
        catch { }
    }
    public void pdfapplication()
    {
        try
        {
            loadsetting();
            string checkvalue = "";
            DAccess2 da = new DAccess2();
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            //Gios.Pdf.PdfDocument mydocument = null;
            // mydocument.PageCount = 0;
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            FpSpread.SaveChanges();
            string spread = "";
            string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            DataSet coll = da.select_method_wo_parameter(strquery, "Text");
            for (int i = 1; i < FpSpread.Sheets[0].RowCount; i++)
            {
                checkvalue = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                if (checkvalue == "1")
                {
                    mypdfpage = mydocument.NewPage();
                    mypdfpage1 = mydocument.NewPage();
                    string app_no = Convert.ToString(FpSpread.Sheets[0].Cells[Convert.ToInt32(i), 0].Tag);
                    Session["pdfapp_no"] = Convert.ToString(app_no);
                    string university = "";
                    string collname = "";
                    string address1 = "";
                    string address2 = "";
                    string address3 = "";
                    string pincode = "";
                    string affliated = "";
                    if (coll.Tables[0].Rows.Count > 0)
                    {
                        collname = coll.Tables[0].Rows[0]["collname"].ToString();
                        address1 = coll.Tables[0].Rows[0]["address1"].ToString();
                        address2 = coll.Tables[0].Rows[0]["address2"].ToString();
                        address3 = coll.Tables[0].Rows[0]["address3"].ToString();
                        pincode = coll.Tables[0].Rows[0]["pincode"].ToString();
                        affliated = coll.Tables[0].Rows[0]["affliatedby"].ToString();
                    }
                    string query = "select IsExService,parentF_Mobile,Degree_Code,(select textval from textvaltable where TextCode=bldgrp) bldgrp,parent_income,emailp,mother,motherocc,mIncome,parentM_Mobile,emailM,guardian_name,guardian_mobile,emailg,Aadharcard_no,place_birth,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,(select textval from textvaltable where convert(varchar,TextCode)=convert(varchar, DistinctSport))DistinctSport,dis_sports,(select textval from textvaltable where convert(varchar,TextCode)=convert(varchar, co_curricular)) co_curricular,parent_addressC,Streetc,Cityc, (select textval from textvaltable where TextCode=parent_statec) parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,(select textval from textvaltable where TextCode=parent_statep) parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet ,case when visualhandy='1' then 'Visually Challanged' when islearningdis='1' then 'Learning Disability' when handy='1' then 'Handy' else CONVERT(varchar(20), isdisabledisc) end disabilitydet ,(select textval from textvaltable where TextCode=convert(varchar(100), isnull(a.partlanguage,0)))partlanguage  from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "'";
                    query = query + " select instaddress,course_entno,(select textval from textvaltable where convert(varchar, textcode)=convert(varchar,course_code)) course_code,university_code,Institute_name,percentage,instaddress,medium,Xmedium,branch_code ,(select textval from textvaltable where convert(varchar, textcode)=convert(varchar,Part1Language))Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                    query = query + " select * from perv_marks_history ";
                    query = query + " select photo from StdPhoto where  app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                    ds1.Clear();
                    ds1 = d2.select_method_wo_parameter(query, "text");
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        int left1 = 1;
                        int left2 = 225;
                        int left4 = 470;
                        string[] split = collname.Split('(');
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                        //{
                        //    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                        //    // mypdfpage.Add(LogoImage, 20, 40, 250);
                        //    mypdfpage.Add(LogoImage, 30, 30, 250);
                        //}
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg"));
                            mypdfpage.Add(LogoImage, 20, 44, 320);
                        }
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg")))
                        {
                            try
                            {
                                string leftlogo = "Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ")";
                                MemoryStream memoryStream = new MemoryStream();
                                byte[] file = (byte[])coll.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 1)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg"));
                                mypdfpage.Add(LogoImage, 20, 44, 320);
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                            catch { }
                        }
                        int coltop = 15;
                        PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Application No:  " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No: ");
                        //   " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(To be allotted by the College Office)");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + Convert.ToString("(Autonomous)"));
                        mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                        //                                                 new PdfArea(mydocument, 110, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(Autonomous)"));
                        //mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -20, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -20, coltop - 20, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "U.G.COURSES - (" + Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " - " + (Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 3) + ")");
                        mypdfpage.Add(ptc);
                        string clgtime = d2.GetFunction("select ':'+space(1) +textval as textval from textvaltable where TextCriteria='Ctime' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                        if (clgtime.Trim() == "0")
                        {
                            clgtime = "";
                        }
                        string Timing = "";
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "13")
                        {
                            Timing = "(SHIFT - I " + clgtime + ")"; //: 8.30 AM - 1.30 PM
                        }
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "14")
                        {
                            Timing = "(SHIFT - II " + clgtime + ")";//: 2.15 PM - 6.40 PM
                        }
                        //if (cbclgtme.Checked == true)
                        //{
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Timing);
                        mypdfpage.Add(ptc);
                        //}
                        ////////photo/////////
                        string imgPhoto = string.Empty;
                        byte[] photoid = new byte[0];
                        if (ds1.Tables[3].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows[0][0] != null && Convert.ToString(ds1.Tables[3].Rows[0][0]) != "")
                            {
                                photoid = (byte[])(ds1.Tables[3].Rows[0][0]);
                            }
                        }
                        string appformno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                        {
                            imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                        }
                        else
                        {
                            try
                            {
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                                {
                                    MemoryStream memoryStream = new MemoryStream();
                                    memoryStream.Write(photoid, 0, photoid.Length);
                                    if (photoid.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            catch { }
                        }
                        if (imgPhoto.Trim() == string.Empty)
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, 40, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Affix");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 50, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Passport size");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 60, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "photograph");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + sign + ".jpeg")))
                            //{
                            try
                            {
                                PdfImage studimg = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg, 458, 44, 250);
                            }
                            catch { }
                            //}
                        }
                        coltop = coltop + 40;
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "[Please read the Prospectus carefully before filling up the application form. Use CAPITAL LETTERS only]");
                        // mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        left1 = 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "COURSE APPLIED FOR");
                        mypdfpage.Add(ptc);
                        string courseid = d2.GetFunction("select c.Course_Name from Degree d,course c where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Course_Id=c.Course_Id");
                        string deptname = d2.GetFunction("select Dept_Name from Degree d,Department dd where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Dept_Code=dd.Dept_Code");
                        left1 = 140;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + courseid + "-" + deptname + "");
                        mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                     new PdfArea(mydocument, left1 - 90, coltop + 30, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(Session["gradutation"]) + "-" + Convert.ToString(Session["course"]) + "");
                        //mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                        //                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "* Subject to approval of affiliation from the University of Madras");
                        //mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 15;
                        //if (cbpartlang.Checked == true)
                        //{
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop + 15, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PART -I LANGUAGE :    ______________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 65;
                        //}
                        //else
                        //{
                        //    coltop = coltop + 80;
                        //}
                        //if (ds1.Tables[1].Rows.Count > 0)
                        //{
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["partlanguage"]).Trim() != "0" && Convert.ToString(ds1.Tables[0].Rows[0]["partlanguage"]).Trim().ToUpper() != "---SELECT---")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, left1 + 125, coltop - 50, 600, 50), System.Drawing.ContentAlignment.TopLeft, Convert.ToString(ds1.Tables[0].Rows[0]["partlanguage"]));
                            mypdfpage.Add(ptc);
                        }
                        //}
                        //////////////////////////for office/////////////////////
                        PdfArea pa13 = new PdfArea(mydocument, 14, coltop - 23, 560, 60);
                        PdfRectangle pr13 = new PdfRectangle(mydocument, pa13, Color.Black);
                        mypdfpage.Add(pr13);
                        coltop -= 35;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "For office use:");
                        mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,new PdfArea());
                        coltop = coltop + 30;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in   : _________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 295, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "on  ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 310, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " _____________________");
                        mypdfpage.Add(ptc);
                        if (cbatbtnme.Checked == true)
                        {
                            left4 = 475;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, left4, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "AT / BT /NME");
                            mypdfpage.Add(ptc);
                        }
                        left1 = 20;
                        coltop = coltop + 32;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Allied - 1         : ____________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 275, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Allied - 2     : ________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 65, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant's Name (In English)");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 200, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "  " + ds1.Tables[0].Rows[0]["stud_name"] + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant's Name (In Tamil)");
                        mypdfpage.Add(ptc);
                        coltop += 22;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Address for Communication");
                        mypdfpage.Add(ptc);
                        left1 = 350;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Permanent Address");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 15, coltop + 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop += 20;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop + 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string address = "";
                        address = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]) + "," + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]);
                        string address_value = "";
                        address_value = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]) + "," + Convert.ToString(ds1.Tables[0].Rows[0]["Streetc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 8, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 280, coltop - 8, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address_value) + "");
                        mypdfpage.Add(ptc);
                        coltop += 20;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressfist = "";
                        addressfist = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string addressfist1 = "";
                        addressfist1 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressscond = "";
                        addressscond = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]);
                        string addressscond1 = "";
                        addressscond1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop - 2, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 100, coltop - 2, 270, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodep"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 + 100, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodec"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 14;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, left1 + 100, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "Mobile No:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 300 + 100, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, "Nationality");
                        mypdfpage.Add(ptc);
                        string nationality = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 280, 50), System.Drawing.ContentAlignment.TopLeft, ": " + nationality + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Aadhar Card No");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["Aadharcard_no"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Religion & Community");
                        mypdfpage.Add(ptc);
                        string relig = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["religion"]));
                        string comm = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["community"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + relig + " & " + comm);//+ "      (Attach photocopy)"
                        mypdfpage.Add(ptc);
                        string caste = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["caste"]));
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Caste");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + caste + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother Tongue");
                        mypdfpage.Add(ptc);
                        string mothertong = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + mothertong + "");
                        mypdfpage.Add(ptc);
                        string bldgrp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["DistinctSport"]) == "" || Convert.ToString(ds1.Tables[0].Rows[0]["DistinctSport"]) == "0")
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Distinction in Sports : No.");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Distinction in Sports : " + Convert.ToString(ds1.Tables[0].Rows[0]["DistinctSport"]) + " - " + Convert.ToString(ds1.Tables[0].Rows[0]["dis_sports"]) + " ( bring relevant documents at the time of Admission)");
                            mypdfpage.Add(ptc);
                        } coltop = coltop + 20;
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]) == "" || Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]) == "0")
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Extra Curricular Activites / Co-Curricular Activites : No");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Extra Curricular Activites / Co-Curricular Activites  : " + Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]) + " ( bring relevant documents at the time of Admission)");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether differently-abled ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "1" || Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "True")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes. " + Convert.ToString(ds1.Tables[0].Rows[0]["disabilitydet"]) + "" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether son of Ex-serviceman ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }

                        //Added by saranya on 29/05/2018
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hostel accommodation");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["CampusReq"]) == "True")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }

                        ///////////////////////////////////

                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF THE PARENTS/GUARDIAN ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in English)");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]).ToUpper() + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        mypdfpage.Add(ptc);
                        string occcp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]));
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + occcp + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        string income = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_income"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + income + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailp"]) + "");
                        mypdfpage.Add(ptc);
                        ////////////////////////////////page2///////////////////////////////////////
                        coltop = 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________________________________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["mother"]).ToUpper() + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "___________________________________________");
                        mypdfpage1.Add(ptc);
                        string moth_occ = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["motherocc"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moth_occ + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 333, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income ");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 405, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        string moth_income = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mIncome"]));
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 405, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moth_income + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parentM_Mobile"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail ID");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 325, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 325, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailM"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Guardian's Name (if living with guardian)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                              new PdfArea(mydocument, 225, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["guardian_name"]) + "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 115, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 115, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["guardian_mobile"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail ID");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 330, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, 330, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["emailg"]) + "");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "PARTICULARS OF PREVIOUS ACADEMIC RECORD");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 45;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Qualifying exam passed");
                        mypdfpage1.Add(ptc);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[1].Rows[0]["course_code"] + ""));
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the Board");
                        //mypdfpage1.Add(ptc);
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(Session["bordoruniversity"]) + "");
                        //mypdfpage1.Add(ptc);
                        //coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Institution last attended");
                        mypdfpage1.Add(ptc);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[1].Rows[0]["Institute_name"]) + "");
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(With Address & Contact Nos)");
                        mypdfpage1.Add(ptc);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[1].Rows[0]["instaddress"]) + "");
                            mypdfpage1.Add(ptc);
                        }
                        //Added by saranya on 30May2018//

                        string Vocational_stream = d2.GetFunction("select Vocational_stream from Stud_prev_details where app_no='" + app_no + "'");

                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Vocational");
                        mypdfpage1.Add(ptc);

                        if (Convert.ToString(Vocational_stream) == "True" || Convert.ToString(Vocational_stream) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes");
                            mypdfpage1.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage1.Add(ptc);
                        }
                        //=============================//

                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Language studied in X-Std");
                        mypdfpage1.Add(ptc);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            string medium = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["medium"]));
                            string Xmedium = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["Xmedium"]));
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " " + Xmedium + " ");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 300, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Language studied in XII-Std");
                            mypdfpage1.Add(ptc);
                            
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, 300 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + medium + "");
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 25;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "EXTRACT OF THE MARK STATEMENT(S) OF THE QUALIFYING EXAMINATION PASSED ");
                        mypdfpage1.Add(ptc);
                        #region mark details
                        ////// table////////
                        string subjectname = "";
                        string finalmarkandgrade = "";
                        string subjectwisemark = "";
                        string Month = "";
                        string year1 = "";
                        string regno = "";
                        string nofoattempts = "";
                        string max_mark = "";
                        int maxtotal = 0;
                        int mintotal = 0;
                        string grade = "";
                        DataView dv = new DataView();
                        int count = 0;
                        Session["subjectwisemark"] = null;
                        ds1.Tables[2].DefaultView.RowFilter = " course_entno='" + Convert.ToString(ds1.Tables[1].Rows[0]["course_entno"]) + "' ";
                        dv = ds1.Tables[2].DefaultView;
                        if (dv.Count > 0)
                        {
                            for (int u = 0; u < dv.Count; u++)
                            {
                                count++;
                                grade = Convert.ToString(dv[u]["grade"]);
                                if (grade != "")
                                {
                                    finalmarkandgrade = Convert.ToString(dv[u]["grade"]);
                                }
                                else
                                {
                                    finalmarkandgrade = Convert.ToString(dv[u]["acual_marks"]);
                                }
                                subjectname = Convert.ToString(dv[u]["psubjectno"]);
                                Month = Convert.ToString(dv[u]["pass_month"]);
                                year1 = Convert.ToString(dv[u]["pass_year"]);
                                regno = Convert.ToString(dv[u]["registerno"]);
                                nofoattempts = Convert.ToString(dv[u]["noofattempt"]);
                                max_mark = Convert.ToString(dv[u]["max_marks"]);
                                if (subjectname.Trim() != "")
                                {
                                    if (subjectwisemark == "")
                                    {
                                        subjectwisemark = subjectname + "-" + finalmarkandgrade + "-" + Month + "-" + year1 + "-" + regno + "-" + nofoattempts + "-" + max_mark;
                                    }
                                    else
                                    {
                                        subjectwisemark = subjectwisemark + "/" + subjectname + "-" + finalmarkandgrade + "-" + Month + "-" + year1 + "-" + regno + "-" + nofoattempts + "-" + max_mark;
                                    }
                                    if (maxtotal == 0)
                                    {
                                        maxtotal = Convert.ToInt32(max_mark);
                                    }
                                    else
                                    {
                                        maxtotal = maxtotal + Convert.ToInt32(max_mark);
                                    }
                                    if (grade == "")
                                    {
                                        if (mintotal == 0)
                                        {
                                            mintotal = Convert.ToInt32(finalmarkandgrade);
                                        }
                                        else
                                        {
                                            mintotal = mintotal + Convert.ToInt32(finalmarkandgrade);
                                        }
                                    }
                                }
                            }
                        }
                        string[] splittablevlaue;
                        if (subjectwisemark.Trim() != "")
                        {
                            Session["subjectwisemark"] = subjectwisemark.ToString();
                        }
                        Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontsmall, count + 1 + 1, 7, 1);
                        table2 = mydocument.NewTable(Fontsmall, count + 1 + 1, 7, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Columns[4].SetWidth(100);
                        table2.Columns[5].SetWidth(100);
                        table2.Columns[6].SetWidth(100);
                        table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Subjects");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Register No");
                        if (grade == "")
                        {
                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("Mark");
                        }
                        else
                        {
                            table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(0, 2).SetContent("Grade");
                        }
                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 3).SetContent("Maximum Marks");
                        table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 4).SetContent("Month");
                        table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 5).SetContent("Year");
                        table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 6).SetContent("No.of Attempts");
                        int count_value = 0;
                        string tablevalue1 = Convert.ToString(Session["subjectwisemark"]);
                        if (tablevalue1.Trim() != "")
                        {
                            splittablevlaue = tablevalue1.Split('/');
                            if (splittablevlaue.Length > 0)
                            {
                                for (int add = 0; add <= splittablevlaue.GetUpperBound(0); add++)
                                {
                                    count_value++;
                                    string[] firstvalue = splittablevlaue[add].Split('-');
                                    if (firstvalue.Length > 0)
                                    {
                                        subjectname = Convert.ToString(firstvalue[0]);
                                        string subjectname1 = "";
                                        string selectquery = "select Textval from textvaltable where TextCode='" + subjectname + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "'";
                                        ds.Clear();
                                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            subjectname1 = Convert.ToString(ds.Tables[0].Rows[0]["Textval"]);
                                        }
                                        table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table2.Cell(add + 1, 0).SetContent(subjectname1);
                                        table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 1).SetContent(Convert.ToString(firstvalue[4]));
                                        table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 2).SetContent(Convert.ToString(firstvalue[1]));
                                        table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 3).SetContent(Convert.ToString(firstvalue[6]));
                                        if (firstvalue[2] != "")
                                        {
                                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(add + 1, 4).SetContent(Convert.ToString(firstvalue[2].First().ToString().ToUpper() + firstvalue[2].Substring(1)));
                                            // Month.First().ToString().ToUpper() + Month.Substring(1)
                                        }
                                        else
                                        {
                                            table2.Cell(add + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(add + 1, 4).SetContent("");
                                        }
                                        table2.Cell(add + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 5).SetContent(Convert.ToString(firstvalue[3]));
                                        table2.Cell(add + 1, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 6).SetContent(Convert.ToString(firstvalue[5]));
                                    }
                                }
                                table2.Cell(count_value + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(count_value + 1, 0).SetContent("Total Marks Secured");
                                foreach (PdfCell pr in table2.CellRange(count_value + 1, 0, count_value + 1, 0).Cells)
                                {
                                    pr.ColSpan = 2;
                                }
                                table2.Cell(count_value + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 2).SetContent("" + mintotal + "");
                                table2.Cell(count_value + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 3).SetContent("" + maxtotal + "");
                                foreach (PdfCell pr in table2.CellRange(count_value + 1, 4, count_value + 1, 4).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                            }
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop + 30, 550, 550));
                        mypdfpage1.Add(myprov_pdfpage1);
                        #endregion
                        /////////////////////////////bottom////////////////////////
                        //coltop = coltop + 200;
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        //mypdfpage1.Add(ptc);
                        //coltop = coltop + 10;
                        //ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        //mypdfpage1.Add(ptc);
                        ////???? ??????????????? ????????? ????????? ??????????, ????????. ???? ?????????????? ??????????????? ?????? ????????????? ???????????? ????????. ??????????? ????????? ???????????? ?????? ????????? ????? ????????. ????????, ??????? ????????? ????????? ??????????????? ??????????????.
                        coltop = coltop + 160;
                        // coltop = coltop + 200;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "List of enclosures :");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(i)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(ii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 415, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iv)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(v)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(vi)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Declaration:");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 55, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " regulations of the college, and will not take part in any activity prejudical to the interest of the college.");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        //???? ??????????????? ????????? ????????? ??????????, ????????. ???? ?????????????? ??????????????? ?????? ????????????? ???????????? ????????. ??????????? ????????? ???????????? ?????? ????????? ????? ????????. ????????, ??????? ????????? ????????? ??????????????? ??????????????.
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "________________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Parent/Guardian");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Student");
                        mypdfpage1.Add(ptc);
                        bool falge = false;
                        if (falge == false)
                        {
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "----------------------------------------------------------------FOR OFFICE USE ONLY------------------------------------------------------------");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Interviewed on ______________________________________    Interviewed by  ______________________________________ ");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "by");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(Staff No:");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 425, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________)");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 55;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 60;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place :");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date :");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                        mypdfpage1.Add(ptc);
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        //{
                        //    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        //    mypdfpage.Add(LogoImage, 25, 40, 400);
                        //}
                        /////////////////2ND header/////////////
                        PdfArea pa12 = new PdfArea(mydocument, 110, 40, 344, 120);
                        PdfRectangle pr12 = new PdfRectangle(mydocument, pa12, Color.Black);
                        mypdfpage.Add(pr12);
                        /////////////////////right photo//////////////////
                        PdfArea pa4 = new PdfArea(mydocument, 454, 40, 120, 120);
                        PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                        mypdfpage.Add(pr4);
                        /////////////////1st header/////////////
                        PdfArea pa5 = new PdfArea(mydocument, 110, 40, 344, 60);
                        PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
                        mypdfpage.Add(pr5);
                        /////////////////page//////////////
                        PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                        PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                        mypdfpage.Add(pr3);
                        mypdfpage1.Add(pr3);
                        //////////////////addressleft/////////////
                        PdfArea pa9 = new PdfArea(mydocument, 14, 357, 270, 100);
                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                        mypdfpage.Add(pr9);
                        ////////////////addressright/////////////
                        //294.5
                        PdfArea pa90 = new PdfArea(mydocument, 284.5, 357, 287, 100);
                        PdfRectangle pr90 = new PdfRectangle(mydocument, pa90, Color.Black);
                        mypdfpage.Add(pr90);
                        ////////////////////email\\\\\\\\\\\\\\\\\\\\\\\
                        //PdfArea pa91 = new PdfArea(mydocument, 14, 520, 555, 30);
                        //PdfRectangle pr91 = new PdfRectangle(mydocument, pa91, Color.Black);
                        //mypdfpage.Add(pr91);
                        mypdfpage.SaveToDocument();
                        mypdfpage1.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
        }
        catch
        {
        }
    }
    protected void pgPdfapplication()
    {
        try
        {
            loadsetting();
            string collegeName = string.Empty;
            string collegeCateg = string.Empty;
            string collegeAff = string.Empty;
            string collegeAdd = string.Empty;
            string collegePhone = string.Empty;
            string collegeFax = string.Empty;
            string collegeWeb = string.Empty;
            string collegeEmai = string.Empty;
            string collegePin = string.Empty;
            string City = string.Empty;
            string shift = "";
            //  PDF CONTENT
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage2 = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            FpSpread.SaveChanges(); //contentDiv.InnerHtml = ""; StringBuilder pghtml = new StringBuilder();
            string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            for (int sel = 1; sel < FpSpread.Sheets[0].Rows.Count; sel++)
            {
                int value = Convert.ToInt32(FpSpread.Sheets[0].Cells[sel, 1].Value);
                if (value == 1)
                {
                    mypdfpage = mydocument.NewPage();
                    mypdfpage1 = mydocument.NewPage();
                    string appno = Convert.ToString(FpSpread.Sheets[0].Cells[sel, 0].Tag);
                    if (appno != "")
                    {
                        int left1 = 1;
                        int left2 = 225;
                        int left4 = 470;
                        Session["pdfapp_no"] = appno;
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        }
                        string[] split = collname.Split('(');
                        string query = "select IsExService,parentF_Mobile,Degree_Code,bldgrp,( select textval from textvaltable where   textcriteria='fin' and convert(varchar,textcode)=convert(varchar,parent_income) )parent_income,emailp,mother,motherocc,( select textval from textvaltable where   textcriteria='fin' and convert(varchar,textcode)=convert(varchar,mIncome) )mIncome,parentM_Mobile,emailM,guardian_name,guardian_mobile,emailg,Aadharcard_no,place_birth,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet,case when visualhandy='1' then 'Visually Challanged' when islearningdis='1' then 'Learning Disability' when handy='1' then 'Handy' else CONVERT(varchar(20), isdisabledisc) end disabilitydet  from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' ";//and college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        query = query + " select percentage,majorallied_percent,major_percent,instaddress,course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                        query = query + " select * from perv_marks_history ";
                        query = query + " select photo from StdPhoto where  app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        //mark history
                        string coursno = d2.GetFunction("select course_entno from Stud_prev_details where app_no='" + appno + "'");
                        string SelMQ = "select psubjectno,registerno,acual_marks,max_marks,(pass_month+'-'+pass_year)as passmnth from perv_marks_history where course_entno='" + coursno + "'";
                        DataSet dsm = new DataSet();
                        dsm.Clear();
                        dsm = d2.select_method_wo_parameter(SelMQ, "Text");
                        string monandyear = "";
                        if (dsm.Tables[0].Rows.Count > 0)
                        {
                            monandyear = "- " + Convert.ToString(dsm.Tables[0].Rows[0]["passmnth"]);
                        }
                        //course and dept
                        string SelctC = "select Course_Name,Dept_Name from Course c,Degree d,Department dt where c.Course_Id=d.Course_Id  and dt.Dept_Code=d.Dept_Code and  d.Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "'";
                        DataSet dsc = new DataSet();
                        dsc.Clear();
                        dsc = d2.select_method_wo_parameter(SelctC, "Text");
                        string regno = "";
                        // string applno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        string draftno = "";
                        string bankname = "";
                        string branch = "";
                        string course = Convert.ToString(dsc.Tables[0].Rows[0]["Course_Name"]);
                        string subj = Convert.ToString(dsc.Tables[0].Rows[0]["Dept_Name"]);
                        string name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                        string padd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]);
                        string padd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]);
                        string padd3 = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string padd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]));
                        string cadd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]);
                        string cadd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetc"]);
                        string cadd3 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        string cadd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]));
                        string email = Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]);
                        string mblno = Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]);
                        string nation = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        string dob = Convert.ToString(ds1.Tables[0].Rows[0]["dob"]);
                        string placbth = Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]);
                        string relig = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["religion"]));
                        string communty = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["community"]));
                        string bldgrp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]));
                        string mothtng = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        string disbld = Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]);
                        if (disbld == "1")
                            disbld = "YES";
                        else
                            disbld = "NO";
                        string exman = Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]);
                        if (exman == "1")
                            exman = "YES";
                        else
                            exman = "NO";
                        string fname = Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]);
                        string foccup = Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]);
                        if (foccup == "0")
                            foccup = "";
                        else
                            foccup = subjectcode(foccup);
                        string fannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["parent_income"]);
                        string fcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]);
                        string femailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailp"]);
                        string mname = Convert.ToString(ds1.Tables[0].Rows[0]["mother"]);
                        string moccup = Convert.ToString(ds1.Tables[0].Rows[0]["motherocc"]);
                        if (moccup == "0")
                            moccup = "";
                        else
                            moccup = subjectcode(moccup);
                        string mannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["mIncome"]);
                        string mcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentM_Mobile"]);
                        string memailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailM"]);
                        string gname = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_name"]);
                        string gcntno = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_mobile"]);
                        string gemailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailg"]);
                        string qlexampas = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["course_code"]));
                        string nameofuni = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["university_code"]));
                        string instlstatnd = Convert.ToString(ds1.Tables[1].Rows[0]["Institute_name"]);
                        string regnumb = Convert.ToString(ds1.Tables[1].Rows[0]["registration_no"]);
                        string Timing = "";
                        string clgtime = d2.GetFunction("select ':'+space(1) +textval as textval from textvaltable where TextCriteria='Ctime' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                        if (clgtime.Trim() == "0")
                        {
                            clgtime = "";
                        }
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "13")
                        {
                            Timing = "(SHIFT - I " + clgtime + ")"; //: 8.30 AM - 1.30 PM
                        }
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "14")
                        {
                            Timing = "(SHIFT - II " + clgtime + ")";//: 2.15 PM - 6.40 PM
                        }
                        //string photo = "";
                        //byte[] photoid = new byte[0];
                        //if (ds1.Tables[3].Rows.Count > 0)
                        //{
                        //    if (ds1.Tables[3].Rows[0][0] != null && Convert.ToString(ds1.Tables[3].Rows[0][0]) != "")
                        //    {
                        //        photoid = (byte[])(ds1.Tables[3].Rows[0][0]);
                        //        if (photoid.Length > 0)
                        //        {
                        //            photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                        //        }
                        //    }
                        //}
                        //string txt1 = "    * Application number for Downloaded application will be alloted & initimated to the candidates by the college office on its receipts along with the prescribed application fee of Rs. in the form of crossed DD drawn in favour of the The Prinicipal,The New  College,chennai 600 014";
                        //string txtmsg = string.Empty;// "[Please read the prospectus carfully filling up the application form. Use CAPITAL LETTERS only]";
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                        //{
                        //    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                        //    // mypdfpage.Add(LogoImage, 20, 40, 250);
                        //    mypdfpage.Add(LogoImage, 30, 30, 250);
                        //}
                        int coltop = 15;
                        PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Application No:  " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No: ");//+ regnumb
                        //   " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(To be allotted by the College Office)");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " (Autonomous)");
                        mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                        //                                                 new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString());
                        //mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "P.G.COURSES - (" + Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " - " + (Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 2) + ")");
                        mypdfpage.Add(ptc);
                        //if (cbclgtme.Checked == true)
                        //{
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Timing);
                        mypdfpage.Add(ptc);
                        //}
                        string imgPhoto = string.Empty;
                        byte[] photoid = new byte[0];
                        if (ds1.Tables[3].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows[0][0] != null && Convert.ToString(ds1.Tables[3].Rows[0][0]) != "")
                            {
                                photoid = (byte[])(ds1.Tables[3].Rows[0][0]);
                            }
                        }
                        string appformno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                        {
                            imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                        }
                        else
                        {
                            try
                            {
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                                {
                                    MemoryStream memoryStream = new MemoryStream();
                                    memoryStream.Write(photoid, 0, photoid.Length);
                                    if (photoid.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            catch { }
                        }
                        if (imgPhoto.Trim() == string.Empty)
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, 40, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Affix");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 50, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Passport size");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 60, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "photograph");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            try
                            {
                                PdfImage studimg = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg, 460, 44, 250);
                            }
                            catch { }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg"));
                            mypdfpage.Add(LogoImage, 20, 44, 320);
                        }
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg")))
                        {
                            try
                            {
                                string leftlogo = "Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ")";
                                MemoryStream memoryStream = new MemoryStream();
                                byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                memoryStream.Write(file, 0, file.Length);
                                if (file.Length > 1)
                                {
                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + leftlogo + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo(" + ddl_collegename.SelectedItem.Value.ToString() + ").jpeg"));
                                mypdfpage.Add(LogoImage, 20, 44, 320);
                                memoryStream.Dispose();
                                memoryStream.Close();
                            }
                            catch { }
                        }
                        //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                        //{
                        //    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                        //    mypdfpage.Add(LogoImage, 30, 50, 250);
                        //}
                        /////////////////////right photo//////////////////
                        PdfArea pa4 = new PdfArea(mydocument, 454, 40, 120, 120);
                        PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                        mypdfpage.Add(pr4);
                        //left logo
                        PdfArea collogoA = new PdfArea(mydocument, 15, 40, 120, 120);
                        PdfRectangle collogoR = new PdfRectangle(mydocument, collogoA, Color.Black);
                        mypdfpage.Add(collogoR);
                        /////////////////1st header/////////////
                        PdfArea pa5 = new PdfArea(mydocument, 140, 100, 310, 60);
                        PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
                        mypdfpage.Add(pr5);
                        /////////////////page//////////////
                        PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                        PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                        mypdfpage.Add(pr3);
                        mypdfpage1.Add(pr3);
                        //////////////////////////for office/////////////////////
                        //PdfArea pa13 = new PdfArea(mydocument, 14, 280, 560, 60);
                        //PdfRectangle pr13 = new PdfRectangle(mydocument, pa13, Color.Black);
                        //mypdfpage.Add(pr13);
                        //////////////////addressleft/////////////
                        PdfArea pa9 = new PdfArea(mydocument, 14, 224, 270, 110);
                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                        mypdfpage.Add(pr9);
                        ////////////////addressright/////////////
                        //294.5
                        PdfArea pa90 = new PdfArea(mydocument, 284.5, 224, 287, 110);
                        PdfRectangle pr90 = new PdfRectangle(mydocument, pa90, Color.Black);
                        mypdfpage.Add(pr90);
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "[Please read the Prospectus carefully before filling up the application form. Use CAPITAL LETTERS only]");
                        // mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        left1 = 15;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "COURSE APPLIED FOR");
                        mypdfpage.Add(ptc);
                        string courseid = d2.GetFunction("select c.Course_Name from Degree d,course c where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Course_Id=c.Course_Id");
                        string deptname = d2.GetFunction("select Dept_Name from Degree d,Department dd where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Dept_Code=dd.Dept_Code");
                        left1 = 140;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + courseid + "-" + deptname + "");
                        mypdfpage.Add(ptc);
                        //left1 = 230;
                        //coltop = coltop + 32;
                        //ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                        //                                new PdfArea(mydocument, left1, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Particulars of the applicant");
                        //mypdfpage.Add(ptc);
                        //mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant Name : ");
                        mypdfpage.Add(ptc);
                        left1 = 140;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " " + name + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Address for Communication");
                        mypdfpage.Add(ptc);
                        left1 = 350;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Permanent Address");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop + 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string address = "";
                        coltop = coltop + 20;
                        address = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 300, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address) + "");
                        mypdfpage.Add(ptc);
                        string address_value = "";
                        address_value = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 280, coltop - 2, 300, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address_value) + "");
                        mypdfpage.Add(ptc);
                        coltop += 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 5;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressfist = "";
                        addressfist = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string addressfist1 = "";
                        addressfist1 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressscond = "";
                        addressscond = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]);
                        string addressscond1 = "";
                        addressscond1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 100, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodep"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 + 100, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodec"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 14;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, left1 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mobile No:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 300 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Nationality");
                        mypdfpage.Add(ptc);
                        string nationality = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + nationality + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Aadhar Card No");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["Aadharcard_no"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Religion & Community");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + relig + " & " + communty);// + "      (Attach photocopy)"
                        mypdfpage.Add(ptc);
                        string caste = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["caste"]));
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Caste");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + caste + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother Tongue");
                        mypdfpage.Add(ptc);
                        string mothertong = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + mothertong + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + bldgrp + "");//Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"])
                        mypdfpage.Add(ptc);
                        coltop = coltop + 30;
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]) == "" || Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]) == "0")
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS : NO.");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            string co_curricular = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["co_curricular"]));
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS : " + Convert.ToString(co_curricular) + " ( bring relevant documents at the time of Admission)");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether differently-abled ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "1" || Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "True")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "YES. " + Convert.ToString(ds1.Tables[0].Rows[0]["disabilitydet"]) + " " + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "NO." + "");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether son of Ex-serviceman ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "YES." + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "NO." + "");
                            mypdfpage.Add(ptc);
                        }
                        //Added by saranya on 29/05/2018
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hostel accommodation");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["CampusReq"]) == "True")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        ///////////////////////////////////

                        coltop = coltop + 20;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 180, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF THE PARENTS/GUARDIAN ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + fname.ToUpper() + "");
                        mypdfpage.Add(ptc);
                        //coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                               new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        //mypdfpage.Add(ptc);
                        //string occcp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]));
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + foccup + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + fannulincm + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + fcontno + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + femailid + "");
                        mypdfpage.Add(ptc);
                        //mother name
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + mname.ToUpper() + "");
                        mypdfpage.Add(ptc);
                        //coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                               new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        //mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moccup + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + mannulincm + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + mcontno + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + memailid + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 180, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF PREVIOUS RECORD");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Qualifying exam passed");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + qlexampas + " " + monandyear);
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop += 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the University");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + nameofuni + "");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Institution last attended");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + instlstatnd + "");
                        mypdfpage.Add(ptc);
                        left1 = 300;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Register No");
                        //mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + regnumb + "");
                        //mypdfpage.Add(ptc);
                        coltop = 20;
                        left1 = 100;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "EXTRACT OF THE MARK STATEMENT(S) OF THE QUALIFYING EXAMINATION PASSED ");
                        mypdfpage1.Add(ptc);
                        int cal_maxmark = 0;
                        int cal_autalmark = 0;
                        int ccount = 0; string subjectwisemark = "";
                        if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                        {
                            string nameval = "";
                            //pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;border:1px solid black;'><tr><th style='border:1px solid black;'>Subject</th><th style='border:1px solid black;'>Marks Obtained</th><th style='border:1px solid black;'>Max Marks</th><th style='border:1px solid black;'>Month & year of Passing</th></tr>");
                            for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
                            {
                                ccount++;
                                string subcode = Convert.ToString(dsm.Tables[0].Rows[i]["psubjectno"]);
                                nameval = subjectcode(subcode);
                                string autalmark = Convert.ToString(dsm.Tables[0].Rows[i]["acual_marks"]);
                                string maxmark = Convert.ToString(dsm.Tables[0].Rows[i]["max_marks"]);
                                string pasyr = Convert.ToString(dsm.Tables[0].Rows[i]["passmnth"]);
                                string regino = Convert.ToString(dsm.Tables[0].Rows[i]["registerno"]);
                                if (subjectwisemark == "")
                                {
                                    subjectwisemark = nameval + "-" + autalmark + "-" + maxmark + "-" + pasyr;
                                }
                                else
                                {
                                    subjectwisemark = subjectwisemark + "/" + nameval + "-" + autalmark + "-" + maxmark + "-" + pasyr;
                                }
                                //pghtml.Append("<tr><td style='border:1px solid black;width:200px'>" + nameval + "</td><td  style='border:1px solid black;'><center>" + autalmark + "</center></td><td  style='border:1px solid black;'><center>" + maxmark + "</center></td><td  style='border:1px solid black;'><center>" + pasyr + "</center></td></tr>");
                                if (cal_maxmark == 0)
                                {
                                    cal_maxmark = Convert.ToInt32(maxmark);
                                }
                                else
                                {
                                    cal_maxmark = cal_maxmark + Convert.ToInt32(maxmark);
                                }
                                if (cal_autalmark == 0)
                                {
                                    //cal_autalmark = Convert.ToInt32(autalmark);
                                    int.TryParse(autalmark, out cal_autalmark);
                                }
                                else
                                {
                                    int finvalD = 0;
                                    int.TryParse(autalmark, out finvalD);
                                    cal_autalmark = cal_autalmark + Convert.ToInt32(finvalD);
                                }
                            }
                        }
                        int tolpercentage = cal_autalmark / ccount;
                        string[] splittablevlaue;
                        if (subjectwisemark.Trim() != "")
                        {
                            ViewState["subjectwisemark"] = subjectwisemark.ToString();
                        }
                        Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontsmall, ccount + 1 + 1, 4, 1);
                        table2 = mydocument.NewTable(Fontsmall, ccount + 1 + 1, 4, 1);
                        table2.VisibleHeaders = false;
                        table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table2.Columns[0].SetWidth(100);
                        table2.Columns[1].SetWidth(100);
                        table2.Columns[2].SetWidth(100);
                        table2.Columns[3].SetWidth(100);
                        table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 0).SetContent("Subjects");
                        table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 1).SetContent("Marks Obtained");
                        table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 2).SetContent("Max Marks");
                        table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table2.Cell(0, 3).SetContent("Month & year of Passing");
                        int count_value = 0;
                        string tablevalue1 = Convert.ToString(ViewState["subjectwisemark"]);
                        if (tablevalue1.Trim() != "")
                        {
                            splittablevlaue = tablevalue1.Split('/');
                            if (splittablevlaue.Length > 0)
                            {
                                for (int add = 0; add <= splittablevlaue.GetUpperBound(0); add++)
                                {
                                    count_value++;
                                    string[] firstvalue = splittablevlaue[add].Split('-');
                                    if (firstvalue.Length > 0)
                                    {
                                        table2.Cell(add + 1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table2.Cell(add + 1, 0).SetContent(Convert.ToString(firstvalue[0]));
                                        table2.Cell(add + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 1).SetContent(Convert.ToString(firstvalue[1]));
                                        table2.Cell(add + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 2).SetContent(Convert.ToString(firstvalue[2]));
                                        table2.Cell(add + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(add + 1, 3).SetContent(Convert.ToString(firstvalue[3]) + "-" + Convert.ToString(firstvalue[4]));
                                    }
                                }
                                table2.Cell(count_value + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(count_value + 1, 0).SetContent("Total Marks Scord/Max.Marks");
                                //foreach (PdfCell pr in table2.CellRange(count_value + 1, 0, count_value + 1, 0).Cells)
                                //{
                                //    pr.ColSpan = 2;
                                //}
                                table2.Cell(count_value + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 1).SetContent(cal_autalmark);
                                table2.Cell(count_value + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 2).SetContent(cal_maxmark);
                                table2.Cell(count_value + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(count_value + 1, 3).SetContent("Percentage " + tolpercentage + " % ");
                            }
                        }
                        Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop + 30, 550, 550));
                        mypdfpage1.Add(myprov_pdfpage1);
                        coltop = Convert.ToInt32(myprov_pdfpage1.Area.Height) + 80;
                        left1 = 15;
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total percentage of marks in all subjects(Language/major/Allied/Ancillary/Elective inclusive ofTheory and Practical) : " + Convert.ToString(ds1.Tables[1].Rows[0]["percentage"]));
                            mypdfpage1.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total % of Marks in Major subjects alone(Including theory & Practicals) : " + Convert.ToString(ds1.Tables[1].Rows[0]["majorallied_percent"]));
                            mypdfpage1.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals : " + Convert.ToString(ds1.Tables[1].Rows[0]["major_percent"]));
                            mypdfpage1.Add(ptc);
                        }
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "List of enclosures :");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(i)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(ii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 415, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iv)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(v)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(vi)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Declaration:");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 55, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " regulations of the college, and will not take part in any activity prejudical to the interest of the college. Failing so, I agree");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " to abide by any disciplinary action taken against me.");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        bool checkpage = false;
                        if (coltop + 100 > 800)
                        {
                            mypdfpage2 = mydocument.NewPage();
                            coltop = 30;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                               new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            //mypdfpage2.Add(ptc);
                            //coltop += 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                              new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the student");
                            //mypdfpage2.Add(ptc);
                            //coltop -= 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                               new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            //mypdfpage2.Add(ptc);
                            //coltop += 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                              new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            //mypdfpage2.Add(ptc);
                            //mypdfpage1.Add(pr3);
                            checkpage = true;
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Parent/Guardian");
                            mypdfpage2.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Student");
                            mypdfpage2.Add(ptc);
                            bool falge = false;
                            if (falge == false)
                            {
                                coltop = coltop + 25;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "----------------------------------------------------------------FOR OFFICE USE ONLY------------------------------------------------------------");
                                mypdfpage2.Add(ptc);
                                coltop = coltop + 25;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Interviewed on ______________________________________    Interviewed by  ______________________________________ ");
                                mypdfpage2.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in");
                                mypdfpage2.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                                mypdfpage2.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "by");
                                mypdfpage2.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________");
                                mypdfpage2.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(Staff No:");
                                mypdfpage2.Add(ptc);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 425, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________)");
                                mypdfpage2.Add(ptc);
                                coltop = coltop + 55;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________");
                                mypdfpage2.Add(ptc);
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                                mypdfpage2.Add(ptc);
                            }
                            coltop = coltop + 60;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place :");
                            mypdfpage2.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                            mypdfpage2.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date :");
                            mypdfpage2.Add(ptc);
                        }
                        else
                        {
                            //coltop += 30;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                               new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            //mypdfpage2.Add(ptc);
                            //coltop += 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                              new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the student");
                            //mypdfpage2.Add(ptc);
                            //coltop -= 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                               new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            //mypdfpage2.Add(ptc);
                            //coltop += 20;
                            //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                            //                              new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            //mypdfpage2.Add(ptc);
                            coltop += 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Parent/Guardian");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the Student");
                            coltop = coltop + 25;
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "----------------------------------------------------------------FOR OFFICE USE ONLY------------------------------------------------------------");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 25;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Interviewed on ______________________________________    Interviewed by  ______________________________________ ");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Admitted in");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "by");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________________");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 375, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(Staff No:");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 425, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "____________________)");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 55;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 470, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage1.Add(ptc);
                            coltop = coltop + 60;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place :");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " ");
                            mypdfpage1.Add(ptc);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date :");
                            mypdfpage1.Add(ptc);
                        }
                        mypdfpage.SaveToDocument();
                        mypdfpage1.SaveToDocument();
                        if (checkpage == true)
                        {
                            PdfArea panelA = new PdfArea(mydocument, 14, 14, 565, 810);
                            PdfRectangle panelR = new PdfRectangle(mydocument, panelA, Color.Black);
                            mypdfpage2.Add(panelR);
                            mypdfpage2.SaveToDocument();
                        }
                        #region html print
                        //registration
                        /*pghtml.Append("<table cellpadding='0'cellspacing='0'<tr><td style='align:left;width:430px'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Application No: " + applno + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Register No :</span></td><td><span style='font-size:12px;font-family-Times New Roman;'>" + regno + "</span><td></tr></table>");
                        //logo and clg details
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;'><tr><td style='align:left;  border:1px solid black;'><img src='../college/Left_Logo.jpeg' alt='' style='height:80px; width:70px;'/></td><td colspan='6' style='font-size:12px;font-family:Times New Roman;font-weight:bold; border:1px solid black;text-align:center;'><span>" + collname + "<br>" + address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA" + "<br>" + affliated + "</span><br><hr><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> APPLICATION FOR ADMISSION<br>P.G.COURSES - (2016-19)<br>" + Timing + "</span></td><td colspan='3' style='font-size:15px;font-family:Times New Roman;font-weight:bold; height:90px; width:20%; border:1px solid black;'><img src=" + photo + " style='height:80px; width:70px;'/></td></tr><tr><td colspan='10' style='font-size:10px;'>" + txtmsg + "</td></tr></table>");
                        //stud name
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='align:left;'><tr><td colspan='4' style='align:left;'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Course applied for:</span></td><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;'><span style='font-size:12px;font-family-Times New Roman;'>" + course + "-" + subj + " </span></td></tr><br></br><tr><td colspan='5'></td><td colspan='5'><u><p style='text-align:center;font-size:15px;font-family:Times New Roman;font-weight:bold;'>Particulars of the applicant</p></u></td></tr><br><tr><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;''>Name:</td><td colspan='4'><span style='font-size:12px;font-family-Times New Roman;'>" + name + "</span></td></tr></table><br>");
                        //address
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px; border:1px solid black;'><tr><th width='50%' style='border:1px solid black;'>Address for Communication(with pincode no)</th><th width='50%' style='border:1px solid black;'>Permanent Address(with pincode no)</th><tr><tr><td width='50%' style='border:1px solid black;'>" + padd1 + "</td><td width='50%' style='border:1px solid black;'>" + cadd1 + "</td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd2 + "</td><td width='50%' style='border:1px solid black;'> " + cadd2 + " </td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd3 + "</td><td width='50%' style='border:1px solid black;'> " + cadd3 + "</td></tr><tr><tr><td width='50%' style='border:1px solid black;'>" + padd4 + "</td><td width='50%' style='border:1px solid black;'> " + cadd4 + "</td></tr><tr><td style='align:left; border:1px solid black;'>Email<span>: " + email + "</span></td><td style='align:left; border:1px solid black;'>Mobile no<span>: " + mblno + "</span></td></tr></table><br>");
                        //nation
                        int cal_maxmark = 0;
                        int cal_autalmark = 0;
                        int ccount = 0;
                        pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Nationality</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + nation + "</span></td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Date of birth</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + dob + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Place of birth</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + placbth + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Religion</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + relig + "</span></td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Community</span></td><td colspan='10' style='align:left;'>:<span style='font-size:12px;font-family-Times New Roman;'> " + communty + "</span></td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Blood Group</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + bldgrp + "</span></td><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Mother Tongue</span></td><td colspan='4'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mothtng + "</span></td></tr><tr><td colspan='10'><hr></td></tr><tr><td colspan='3'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether Differently-abled</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + disbld + "</span></td><td colspan='7'>  If yes, attach relevant documents</td></tr><tr><td colspan='3'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether son of Ex-Serviceman</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + exman + "</span><br></td><td colspan='7'>  If yes, attach relevant documents<br><br></td></tr><tr><td colspan='10' style='align:center;'><u><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><center>Particular of the Parent/Guardian</center></span></u><br></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Father</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + fname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='2'>: <span style='font-size:12px;font-family-Times New Roman;'>" + foccup + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='2'>: <span style='font-size:12px;font-family-Times New Roman;'>Rs. " + fannulincm + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + fcontno + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + femailid + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Mother</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + moccup + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> Rs. " + mannulincm + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mcontno + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + memailid + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the guardian</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gcntno + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id<span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gemailid + "</span><br><br></td></tr><tr><td colspan='10'><center><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><u>Particulars of previous record</u><br></span></center><br></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Qualifying exam passed</span></td><td colspan='5'>:<span style='font-size:12px;font-family-Times New Roman;'> " + qlexampas + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the University</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + nameofuni + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Inistitution last attended:</span></td><td colspan='5'>:<span style='font-size:12px;font-family-Times New Roman;'> " + instlstatnd + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Register No</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + regnumb + "</span><td></tr></table><br><br><br><br><br>");
                        pghtml.Append("<table style='width:650px;'><tr><td colspan='10'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Extract of the mark statement/s of the qualifying examination (attach attested copies)</span></td></tr></table><br>");
                        if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                        {
                            string nameval = "";
                            pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;border:1px solid black;'><tr><th style='border:1px solid black;'>Subject</th><th style='border:1px solid black;'>Marks Obtained</th><th style='border:1px solid black;'>Max Marks</th><th style='border:1px solid black;'>Month & year of Passing</th></tr>");
                            for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
                            {
                                ccount++;
                                string subcode = Convert.ToString(dsm.Tables[0].Rows[i]["psubjectno"]);
                                nameval = subjectcode(subcode);
                                string autalmark = Convert.ToString(dsm.Tables[0].Rows[i]["acual_marks"]);
                                string maxmark = Convert.ToString(dsm.Tables[0].Rows[i]["max_marks"]);
                                string pasyr = Convert.ToString(dsm.Tables[0].Rows[i]["passmnth"]);
                                string regino = Convert.ToString(dsm.Tables[0].Rows[i]["registerno"]);
                                pghtml.Append("<tr><td style='border:1px solid black;width:200px'>" + nameval + "</td><td  style='border:1px solid black;'><center>" + autalmark + "</center></td><td  style='border:1px solid black;'><center>" + maxmark + "</center></td><td  style='border:1px solid black;'><center>" + pasyr + "</center></td></tr>");
                                if (cal_maxmark == 0)
                                {
                                    cal_maxmark = Convert.ToInt32(maxmark);
                                }
                                else
                                {
                                    cal_maxmark = cal_maxmark + Convert.ToInt32(maxmark);
                                }
                                if (cal_autalmark == 0)
                                {
                                    cal_autalmark = Convert.ToInt32(autalmark);
                                }
                                else
                                {
                                    cal_autalmark = cal_autalmark + Convert.ToInt32(autalmark);
                                }
                            }
                            pghtml.Append("</table>");
                        }
                        int tolpercentage = cal_autalmark / ccount;
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;border:1px solid black;'><tr><td style='border:1px solid black; width:255px'>Total Marks Scord/Max.Marks: </td><td style='border:1px solid black;width:186px'><center>" + cal_autalmark + "/" + cal_maxmark + "</center></td><td style='border:1px solid black;width:154px'>Percentage: </td><td style='border:1px solid black;'>" + tolpercentage + "%</td></tr></table><br/>");
                        pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Total percentage of marks in all subjects(Language/major/Allied/Ancillary/Elective inclusive ofTheory and Practical)</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["percentage"]) + "</span></td></tr></table>");
                        pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Total % of Marks in Major subjects alone(Including theory & Practicals)</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["majorallied_percent"]) + "</span></td></tr></table>");
                        pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["major_percent"]) + "</span></td></tr></table>");
                        pghtml.Append("<table><tr><td><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>List of enclosures:</span></td></tr><tr><td colspan='3'>1_________________</td><td colspan='2'>2_________________________</td><td colspan='4'>3_________________</td></tr><tr><td colspan='3'>4_________________</td><td colspan='2'>5_________________________</td><td colspan='4'>6_________________</td></tr><tr><td colspan='10' ><span style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'>Declaration :</span>&nbsp;&nbsp;I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and regulations of the college, and will not take part in any activity prejudicial to the interests of the college. Failing so, I agree to abide by any disciplinary action taken against me.</td></tr><tr><td colspan='10'><span style='font-size:14px;font-family:Times New Roman;font-weight:bold;'>உறுதி மொழி :</span>&nbsp;&nbsp;<span style='font-size:12px;font-family-Times New Roman;'>மேலே குறிப்பிட்டுள்ள விவரங்கள் உண்மையானவை, சரியானவை. நான் இக்கல்லூரியின் சட்டங்களுக்கும் நடத்தை விதிகளுக்கும் கட்டுப்பட்டு நடப்பேன். கல்லூரிக்கு அவப்பெயர் விளைவிக்கும்  எந்தச் செயலிலும் ஈடுபட மாட்டேன். தவறினால், தாங்கள் எடுக்கும் எவ்வகையான நடவடிக்கைக்கும் கட்டுப்படுவேன்.</span></td></tr></table>");
                        //pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;' >Place: </td></tr><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'>Date: </td></tr></table>");
                        pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='width:50%; align:left;'><center>____________________</center></td><td colspan='5' style='width:50%; align:right;'></td></tr><tr><td colspan='5' style='width:50%; align:left;'><center>Signature of the student</center></td><td colspan='5' style='width:50%; align:right;'></td></tr></table><br>");
                        //pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'><center>______________________________For office use only _______________________________</center> </td></tr></table><br>");
                        //pghtml.Append("<table style='width;650px;'><tr><td colspan='10'>Interviewed on_________________________________________ </td></tr></table><br>");
                        //pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='align:left;'>Admitted in__________</td><td colspan='5' style='float:right;'><center>by________(Staff No:_________)<br>signature of the staff</center></td></tr></table><br>");
                        //pghtml.Append("<table><tr><td colspan='3'style='align:left;'>Admitted in_____________</td><td colspan='6'><center></center>by______________________</td><td colspan='3' style='align:right;'>(staff no:____________)</td></tr></table>");
                        pghtml.Append("<center><table style='width;650px;'><tr><td colspan='10'><center>________________</center></td></tr><tr><td colspan='10' ><center>Principal</center></td></tr></table></center>");
                        pghtml.Append("</td></tr></table></div>");
                        contentDiv.InnerHtml += pghtml.ToString();*/
                        #endregion
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
            //ScriptManager.RegisterStartupScript(this, GetType(), "btn_pdf", "PrintDiv();", true);
            //contentDiv.Visible = true;
        }
        catch
        { }
    }
    protected void MphilPdfapplication()
    {
        try
        {
            loadsetting();
            string collegeName = string.Empty;
            string collegeCateg = string.Empty;
            string collegeAff = string.Empty;
            string collegeAdd = string.Empty;
            string collegePhone = string.Empty;
            string collegeFax = string.Empty;
            string collegeWeb = string.Empty;
            string collegeEmai = string.Empty;
            string collegePin = string.Empty;
            string City = string.Empty;
            string shift = "";
            //  PDF CONTENT
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage1 = mydocument.NewPage();
            Gios.Pdf.PdfPage mypdfpage2 = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            FpSpread.SaveChanges();
            contentDiv.InnerHtml = "";
            StringBuilder pghtml = new StringBuilder();
            for (int sel = 1; sel < FpSpread.Sheets[0].Rows.Count; sel++)
            {
                int value = Convert.ToInt32(FpSpread.Sheets[0].Cells[sel, 1].Value);
                if (value == 1)
                {
                    int left1 = 1;
                    int left2 = 225;
                    int left4 = 470;
                    mypdfpage = mydocument.NewPage();
                    mypdfpage1 = mydocument.NewPage();
                    string appno = Convert.ToString(FpSpread.Sheets[0].Cells[sel, 0].Tag);
                    if (appno != "")
                    {
                        Session["pdfapp_no"] = appno;
                        string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        }
                        string[] split = collname.Split('(');
                        string query = "select IsExService,parentF_Mobile,Degree_Code,bldgrp,(select textval from textvaltable where textcriteria='fin' and convert(varchar,textcode)=convert(varchar,parent_income)parent_income,emailp,mother,motherocc,(select textval from textvaltable where   textcriteria='fin' and convert(varchar,textcode)=convert(varchar,mIncome)mIncome,parentM_Mobile,emailM,guardian_name,guardian_mobile,emailg,Aadharcard_no,place_birth,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        query = query + " select InternalPercentage,ExternalPercentage,percentage,majorallied_percent,major_percent,instaddress,course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                        query = query + " select * from perv_marks_history ";
                        query = query + " select photo from StdPhoto where  app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        //mark history
                        string coursno = d2.GetFunction("select course_entno from Stud_prev_details where app_no='" + appno + "'");
                        string SelMQ = "select grade,InternalGrade,ExternalGrade,InternalMark,ExternalMark,psubjectno,registerno,acual_marks,max_marks,(pass_month+'-'+pass_year)as passmnth from perv_marks_history where course_entno='" + coursno + "'";
                        DataSet dsm = new DataSet();
                        dsm.Clear();
                        dsm = d2.select_method_wo_parameter(SelMQ, "Text");
                        //course and dept
                        string SelctC = "select Course_Name,Dept_Name from Course c,Degree d,Department dt where c.Course_Id=d.Course_Id  and dt.Dept_Code=d.Dept_Code and  d.Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "'";
                        DataSet dsc = new DataSet();
                        dsc.Clear();
                        dsc = d2.select_method_wo_parameter(SelctC, "Text");
                        string regno = "";
                        string applno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        string draftno = "";
                        string bankname = "";
                        string branch = "";
                        string course = Convert.ToString(dsc.Tables[0].Rows[0]["Course_Name"]);
                        string subj = Convert.ToString(dsc.Tables[0].Rows[0]["Dept_Name"]);
                        string name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                        string padd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]);
                        string padd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]);
                        string padd3 = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string padd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]));
                        string cadd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]);
                        string cadd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetc"]);
                        string cadd3 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        string cadd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]));
                        string email = Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]);
                        string mblno = Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]);
                        string nation = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        string dob = Convert.ToString(ds1.Tables[0].Rows[0]["dob"]);
                        string placbth = Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]);
                        string relig = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["religion"]));
                        string communty = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["community"]));
                        string bldgrp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]));
                        string mothtng = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        string disbld = Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]);
                        string regnumb = Convert.ToString(ds1.Tables[1].Rows[0]["registration_no"]);
                        if (disbld == "1" || disbld == "True")
                            disbld = "YES";
                        else
                            disbld = "NO";
                        string exman = Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]);
                        if (exman == "1" || exman == "True")
                            exman = "YES";
                        else
                            exman = "NO";
                        string fname = Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]);
                        string foccup = Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]);
                        if (foccup == "0")
                            foccup = "";
                        else
                            foccup = subjectcode(foccup);
                        string fannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["parent_income"]);
                        string fcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]);
                        string femailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailp"]);
                        string mname = Convert.ToString(ds1.Tables[0].Rows[0]["mother"]);
                        string moccup = Convert.ToString(ds1.Tables[0].Rows[0]["motherocc"]);
                        if (moccup == "0")
                            moccup = "";
                        else
                            moccup = subjectcode(moccup);
                        string mannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["mIncome"]);
                        string mcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentM_Mobile"]);
                        string memailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailM"]);
                        string gname = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_name"]);
                        string gcntno = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_mobile"]);
                        string gemailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailg"]);
                        string qlexampas = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["course_code"]));
                        string nameofuni = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["university_code"]));
                        string instlstatnd = Convert.ToString(ds1.Tables[1].Rows[0]["Institute_name"]);
                        string ISGARDE = Convert.ToString(ds1.Tables[1].Rows[0]["isgrade"]);
                        string Timing = "";
                        string clgtime = d2.GetFunction("select ':'+space(1) +textval as textval from textvaltable where TextCriteria='Ctime' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                        if (clgtime.Trim() == "0")
                        {
                            clgtime = "";
                        }
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "13")
                        {
                            Timing = "(SHIFT - I " + clgtime + ")"; //: 8.30 AM - 1.30 PM
                        }
                        if (Convert.ToString(ddl_collegename.SelectedItem.Value) == "14")
                        {
                            Timing = "(SHIFT - II " + clgtime + ")";//: 2.15 PM - 6.40 PM
                        }
                        int coltop = 15;
                        PdfTextArea ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 20, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Application No:  " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Registration No: " + regnumb);
                        //   " + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]) + "
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(fontitalic, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(To be allotted by the College Office)");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, -40, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]));
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                         new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString("(Autonomous)"));
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -22, coltop, 650, 50), System.Drawing.ContentAlignment.MiddleCenter, address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, -5, coltop - 20, 600, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                        mypdfpage.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "APPLICATION FOR ADMISSION");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "M.phil.COURSES - (2016-19)");
                        mypdfpage.Add(ptc);
                        if (cbclgtme.Checked == true)
                        {
                            coltop = coltop + 15;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, Timing);
                            mypdfpage.Add(ptc);
                        }
                        string imgPhoto = string.Empty;
                        byte[] photoid = new byte[0];
                        if (ds1.Tables[3].Rows.Count > 0)
                        {
                            if (ds1.Tables[3].Rows[0][0] != null && Convert.ToString(ds1.Tables[3].Rows[0][0]) != "")
                            {
                                photoid = (byte[])(ds1.Tables[3].Rows[0][0]);//photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                            }
                        }
                        string appformno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                        {
                            imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                        }
                        else
                        {
                            try
                            {
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg")))
                                {
                                    MemoryStream memoryStream = new MemoryStream();
                                    memoryStream.Write(photoid, 0, photoid.Length);
                                    if (photoid.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + appformno + ".jpeg");
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            catch { }
                        }
                        if (imgPhoto.Trim() == string.Empty)
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, left2, 40, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Affix");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 50, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "Passport size");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, left2, 60, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "photograph");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            try
                            {
                                PdfImage studimg = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg, 460, 50, 250);
                            }
                            catch { }
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                            mypdfpage.Add(LogoImage, 30, 50, 250);
                        }
                        /////////////////////right photo//////////////////
                        PdfArea pa4 = new PdfArea(mydocument, 454, 40, 120, 120);
                        PdfRectangle pr4 = new PdfRectangle(mydocument, pa4, Color.Black);
                        mypdfpage.Add(pr4);
                        //left logo
                        PdfArea collogoA = new PdfArea(mydocument, 15, 40, 120, 120);
                        PdfRectangle collogoR = new PdfRectangle(mydocument, collogoA, Color.Black);
                        mypdfpage.Add(collogoR);
                        /////////////////1st header/////////////
                        PdfArea pa5 = new PdfArea(mydocument, 140, 100, 310, 60);
                        PdfRectangle pr5 = new PdfRectangle(mydocument, pa5, Color.Black);
                        mypdfpage.Add(pr5);
                        /////////////////page//////////////
                        PdfArea pa1 = new PdfArea(mydocument, 14, 14, 565, 810);//14, 12, 560, 825);
                        PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                        mypdfpage.Add(pr3);
                        mypdfpage1.Add(pr3);
                        //////////////////////////for office/////////////////////
                        //PdfArea pa13 = new PdfArea(mydocument, 14, 280, 560, 60);
                        //PdfRectangle pr13 = new PdfRectangle(mydocument, pa13, Color.Black);
                        //mypdfpage.Add(pr13);
                        //////////////////addressleft/////////////
                        PdfArea pa9 = new PdfArea(mydocument, 14, 223, 270, 110);
                        PdfRectangle pr9 = new PdfRectangle(mydocument, pa9, Color.Black);
                        mypdfpage.Add(pr9);
                        ////////////////addressright/////////////
                        //294.5
                        PdfArea pa90 = new PdfArea(mydocument, 284.5, 223, 287, 110);
                        PdfRectangle pr90 = new PdfRectangle(mydocument, pa90, Color.Black);
                        mypdfpage.Add(pr90);
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, "[Please read the Prospectus carefully before filling up the application form. Use CAPITAL LETTERS only]");
                        // mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        left1 = 15;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "COURSE APPLIED FOR");
                        mypdfpage.Add(ptc);
                        string courseid = d2.GetFunction("select c.Course_Name from Degree d,course c where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Course_Id=c.Course_Id");
                        string deptname = d2.GetFunction("select Dept_Name from Degree d,Department dd where Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "' and d.Dept_Code=dd.Dept_Code");
                        left1 = 140;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + courseid + "-" + deptname + "");
                        mypdfpage.Add(ptc);
                        //left1 = 230;
                        //coltop = coltop + 32;
                        //ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                        //                                new PdfArea(mydocument, left1, coltop - 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Particulars of the applicant");
                        //mypdfpage.Add(ptc);
                        //mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Applicant Name : ");
                        mypdfpage.Add(ptc);
                        left1 = 140;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "  " + name + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Address for Communication");
                        mypdfpage.Add(ptc);
                        left1 = 350;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Permanent Address");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop + 5, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string address = "";
                        coltop = coltop + 20;
                        address = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address) + "");
                        mypdfpage.Add(ptc);
                        string address_value = "";
                        address_value = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(address_value) + "");
                        mypdfpage.Add(ptc);
                        coltop += 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 5;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressfist = "";
                        addressfist = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string addressfist1 = "";
                        addressfist1 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressfist1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        string addressscond = "";
                        addressscond = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]);
                        string addressscond1 = "";
                        addressscond1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 280, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(addressscond1) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 100, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodep"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Pincode:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 + 100, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_pincodec"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 14;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "E-mail:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                new PdfArea(mydocument, left1 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 300 - 5, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mobile No:");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, 300 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 35;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Nationality");
                        mypdfpage.Add(ptc);
                        string nationality = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + nationality + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Date of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Aadhar Card No");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["Aadharcard_no"]) + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Place of Birth");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Religion & Community");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + relig + " & " + communty + "      (Attach photocopy)");
                        mypdfpage.Add(ptc);
                        string caste = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["caste"]));
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                               new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Caste");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + caste + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Blood Group");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 350, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother Tongue");
                        mypdfpage.Add(ptc);
                        string mothertong = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 450, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + mothertong + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]) + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 30;
                        if (Convert.ToString(Session["co_curricular"]) != "-")
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS ");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Distinction / Participation in Sports / Athletics / NCC / NSS : " + Convert.ToString(Session["co_curricular"]) + " ( bring relevant documents at the time of Admission)");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________________________________________________________________________________________");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether differently-abled ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "YES" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                     new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Whether son of Ex-serviceman ");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]) == "1")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes" + " / If yes, bring relevant documents at the time of Admission");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }

                        //Added by saranya on 29/05/2018
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Hostel accommodation");
                        mypdfpage.Add(ptc);
                        if (Convert.ToString(ds1.Tables[0].Rows[0]["CampusReq"]) == "True")
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "Yes");
                            mypdfpage.Add(ptc);
                        }
                        else
                        {
                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + "No" + "");
                            mypdfpage.Add(ptc);
                        }
                        ///////////////////////////////////
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 180, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF THE PARENTS/GUARDIAN ");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + fname + "");
                        mypdfpage.Add(ptc);
                        //coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                               new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        //mypdfpage.Add(ptc);
                        //string occcp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]));
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + foccup + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + fannulincm + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + fcontno + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + femailid + "");
                        mypdfpage.Add(ptc);
                        //mother name
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Mother's Name ");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, ": " + mname + "");
                        mypdfpage.Add(ptc);
                        //coltop = coltop + 20;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                               new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Father's Name (in Tamil)");
                        //mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Occupation");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_________________________________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 75, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + moccup + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 340, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Annual Income");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 410, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 410, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + mannulincm + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, left1 + 2, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Contact No.");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + mcontno + "");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 90, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "__________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Email ID");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "______________________________");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 285, coltop - 2, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + memailid + "");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, left1 + 180, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "PARTICULARS OF PREVIOUS RECORD");
                        mypdfpage.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Qualifying exam passed");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + qlexampas + "");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop += 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Name of the University");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + nameofuni + "");
                        mypdfpage.Add(ptc);
                        left1 = 15;
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Institution last attended");
                        mypdfpage.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                 new PdfArea(mydocument, left1 + 150, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + instlstatnd + "");
                        mypdfpage.Add(ptc);
                        left1 = 300;
                        //ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Register No");
                        //mypdfpage.Add(ptc);
                        //ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                        //                          new PdfArea(mydocument, left1 + 100, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + regnumb + "");
                        //mypdfpage.Add(ptc);
                        coltop = 20;
                        left1 = 100;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "EXTRACT OF THE MARK STATEMENT(S) OF THE QUALIFYING EXAMINATION PASSED ");
                        mypdfpage1.Add(ptc);
                        int cal_maxmark = 0;
                        int cal_autalmark = 0;
                        int ccount = 0;
                        int cal_intermark = 0;
                        int cal_extermark = 0;
                        if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                        {
                            int tablecount = 0;
                            if (ISGARDE == "False")
                                tablecount = 6;
                            else
                                tablecount = 5;
                            string nameval = "";
                            Gios.Pdf.PdfTable table2 = mydocument.NewTable(Fontsmall, dsm.Tables[0].Rows.Count + 2, tablecount, 1);
                            table2 = mydocument.NewTable(Fontsmall, dsm.Tables[0].Rows.Count + 2, tablecount, 1);
                            if (ISGARDE == "False")
                            {
                                table2.VisibleHeaders = false;
                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table2.Columns[0].SetWidth(200);
                                table2.Columns[1].SetWidth(80);
                                table2.Columns[2].SetWidth(80);
                                table2.Columns[3].SetWidth(80);
                                table2.Columns[4].SetWidth(80);
                                table2.Columns[5].SetWidth(80);
                                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 0).SetContent("Subjects");
                                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 1).SetContent("Internal Mark");
                                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 2).SetContent("External Mark");
                                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 3).SetContent("Marks Obtained");
                                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 4).SetContent("Max Marks");
                                table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 5).SetContent("Month & year of Passing");
                            }
                            else
                            {
                                table2.VisibleHeaders = false;
                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table2.Columns[0].SetWidth(100);
                                table2.Columns[1].SetWidth(100);
                                table2.Columns[2].SetWidth(100);
                                table2.Columns[3].SetWidth(100);
                                table2.Columns[4].SetWidth(100);
                                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 0).SetContent("Subjects");
                                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 1).SetContent("Internal Grade");
                                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 2).SetContent("External Grade");
                                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 3).SetContent("Grade");
                                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 4).SetContent("Month & year of Passing");
                            }
                            for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
                            {
                                ccount++;
                                string subcode = Convert.ToString(dsm.Tables[0].Rows[i]["psubjectno"]);
                                nameval = subjectcode(subcode);
                                string autalmark = Convert.ToString(dsm.Tables[0].Rows[i]["acual_marks"]);
                                string maxmark = Convert.ToString(dsm.Tables[0].Rows[i]["max_marks"]);
                                string pasyr = Convert.ToString(dsm.Tables[0].Rows[i]["passmnth"]);
                                string grd = Convert.ToString(dsm.Tables[0].Rows[i]["grade"]);
                                string regino = Convert.ToString(dsm.Tables[0].Rows[i]["registerno"]);
                                string inter = "";
                                string exter = "";
                                if (ISGARDE == "False")
                                {
                                    inter = Convert.ToString(dsm.Tables[0].Rows[i]["InternalMark"]);
                                    exter = Convert.ToString(dsm.Tables[0].Rows[i]["ExternalMark"]);
                                }
                                else
                                {
                                    inter = Convert.ToString(dsm.Tables[0].Rows[i]["InternalGrade"]);
                                    exter = Convert.ToString(dsm.Tables[0].Rows[i]["ExternalGrade"]);
                                }
                                if (ISGARDE == "False")
                                {
                                    if (cal_maxmark == 0)
                                    {
                                        cal_maxmark = Convert.ToInt32(maxmark);
                                    }
                                    else
                                    {
                                        cal_maxmark = cal_maxmark + Convert.ToInt32(maxmark);
                                    }
                                    if (cal_autalmark == 0)
                                    {
                                        cal_autalmark = Convert.ToInt32(autalmark);
                                    }
                                    else
                                    {
                                        cal_autalmark = cal_autalmark + Convert.ToInt32(autalmark);
                                    }
                                    if (cal_intermark == 0)
                                    {
                                        cal_intermark = Convert.ToInt32(inter);
                                    }
                                    else
                                    {
                                        cal_intermark = cal_intermark + Convert.ToInt32(inter);
                                    }
                                    if (cal_extermark == 0)
                                    {
                                        cal_extermark = Convert.ToInt32(exter);
                                    }
                                    else
                                    {
                                        cal_extermark = cal_extermark + Convert.ToInt32(exter);
                                    }
                                }
                                if (ISGARDE == "False")
                                {
                                    table2.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(i + 1, 0).SetContent(Convert.ToString(nameval));
                                    table2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 1).SetContent(Convert.ToString(inter));
                                    table2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 2).SetContent(Convert.ToString(exter));
                                    table2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 3).SetContent(Convert.ToString(autalmark));
                                    table2.Cell(i + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 4).SetContent(Convert.ToString(maxmark));
                                    table2.Cell(i + 1, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 5).SetContent(Convert.ToString(pasyr));
                                }
                                else
                                {
                                    table2.Cell(i, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table2.Cell(i + 1, 0).SetContent(Convert.ToString(nameval));
                                    table2.Cell(i + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 1).SetContent(Convert.ToString(inter));
                                    table2.Cell(i + 1, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 2).SetContent(Convert.ToString(exter));
                                    table2.Cell(i + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 3).SetContent(Convert.ToString(grd));
                                    table2.Cell(i + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(i + 1, 4).SetContent(Convert.ToString(pasyr));
                                }
                            }
                            string tolinterpercentage = "";
                            string tolexterpercentage = "";
                            string tolpercentage1 = "";
                            if (ISGARDE == "False")
                            {
                                int tolpercentage = cal_autalmark / ccount;
                                int tolinterpercentage1 = cal_intermark / ccount;
                                int tolexterpercentage1 = cal_extermark / ccount;
                                tolpercentage1 = Convert.ToString(tolpercentage);
                                tolinterpercentage = Convert.ToString(tolinterpercentage1);
                                tolexterpercentage = Convert.ToString(tolexterpercentage1);
                                table2.Cell(ccount + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(ccount + 1, 0).SetContent("Total Marks Scord/Max.Marks");
                                table2.Cell(ccount + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 1).SetContent(Convert.ToString(cal_autalmark + "/" + cal_maxmark));
                                table2.Cell(ccount + 1, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(ccount + 1, 2).SetContent("Internal Percentage:" + tolinterpercentage + "%");
                                table2.Cell(ccount + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 3).SetContent("External Percentage:" + tolexterpercentage + "%");
                                table2.Cell(ccount + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 4).SetContent("Total Percentage: " + tolpercentage + "%");
                            }
                            else
                            {
                                tolinterpercentage = "";
                                tolexterpercentage = "";
                                table2.Cell(ccount + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(ccount + 1, 0).SetContent("Total Marks Scord/Max.Marks");
                                table2.Cell(ccount + 1, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 1).SetContent(Convert.ToString(cal_autalmark + "/" + cal_maxmark));
                                table2.Cell(ccount + 1, 2).SetContentAlignment(ContentAlignment.MiddleRight);
                                table2.Cell(ccount + 1, 2).SetContent("Internal Percentage:" + tolinterpercentage + "%");
                                table2.Cell(ccount + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 3).SetContent("External Percentage:" + tolexterpercentage + "%");
                                table2.Cell(ccount + 1, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(ccount + 1, 4).SetContent("Total Percentage: " + tolpercentage1 + "%");
                            }
                            Gios.Pdf.PdfTablePage myprov_pdfpage1 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, coltop + 30, 550, 550));
                            mypdfpage1.Add(myprov_pdfpage1);
                            coltop = Convert.ToInt32(myprov_pdfpage1.Area.Height) + 80;
                        }
                        left1 = 15;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total percentage of marks in all subjects(Language/major/Allied/Ancillary/Elective inclusive ofTheory and Practical) : " + Convert.ToString(ds1.Tables[1].Rows[0]["percentage"]));
                        mypdfpage1.Add(ptc);
                        coltop += 20;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total % of Marks in Major subjects alone(Including theory & Practicals) : " + Convert.ToString(ds1.Tables[1].Rows[0]["majorallied_percent"]));
                        mypdfpage1.Add(ptc);
                        coltop += 20;
                        ptc = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals : " + Convert.ToString(ds1.Tables[1].Rows[0]["major_percent"]));
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 40;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "List of enclosures :");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(i)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(ii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iii)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 415, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(iv)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 225, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(v)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 235, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 400, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "(vi)");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 420, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_____________________________");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 20;
                        ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Declaration:");
                        mypdfpage1.Add(ptc);
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 55, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " regulations of the college, and will not take part in any activity prejudical to the interest of the college.");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 15;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                         new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, " Failing so, I agree to abide by any disciplinary action taken against me.");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        coltop = coltop + 10;
                        ptc = new PdfTextArea(tamil, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, left1 + 25, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "");
                        mypdfpage1.Add(ptc);
                        bool checkpage = false;
                        if (coltop + 100 > 800)
                        {
                            mypdfpage2 = mydocument.NewPage();
                            coltop = 30;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            mypdfpage2.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the student");
                            mypdfpage2.Add(ptc);
                            coltop -= 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            mypdfpage2.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage2.Add(ptc);
                            checkpage = true;
                        }
                        else
                        {
                            coltop += 30;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            mypdfpage1.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 35, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Signature of the student");
                            mypdfpage1.Add(ptc);
                            coltop -= 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "_______________________________");
                            mypdfpage1.Add(ptc);
                            coltop += 20;
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 280, coltop, 600, 50), System.Drawing.ContentAlignment.TopLeft, "Principal");
                            mypdfpage1.Add(ptc);
                        }
                        mypdfpage.SaveToDocument();
                        mypdfpage1.SaveToDocument();
                        if (checkpage == true)
                            mypdfpage2.SaveToDocument();
                        #region html print
                        /* string txt1 = "    * Application number for Downloaded application will be alloted & initimated to the candidates by the college office on its receipts along with the prescribed application fee of Rs. in the form of crossed DD drawn in favour of the The Prinicipal,The New  College,chennai 600 014";
                         string txtmsg = string.Empty;// "[Please read the prospectus carfully filling up the application form. Use CAPITAL LETTERS only]";
                         //registration
                         //registration
                         pghtml.Append("<table cellpadding='0'cellspacing='0'<tr><td style='align:left;width:430px'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Application No: " + applno + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Register No :</span></td><td><span style='font-size:12px;font-family-Times New Roman;'>" + regno + "</span><td></tr></table><br/>");
                         //logo and clg details
                         pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;'><tr><td style='align:left;  border:1px solid black;'><img src='" + "college/Left_Logo.jpeg" + "' style='height:80px; width:70px;'/></td><td colspan='6' style='font-size:12px;font-family:Times New Roman;font-weight:bold; border:1px solid black;text-align:center;'><span>" + collname + "<br>" + address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA" + "<br>" + affliated + "</span><br><hr><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> APPLICATION FOR ADMISSION<br>P.G.COURSES - (2016-19)<br>" + Timing + "</span></td><td colspan='3' style='font-size:15px;font-family:Times New Roman;font-weight:bold; height:90px; width:20%; border:1px solid black;'><img src=" + photo + " style='height:80px; width:70px;'/></td></tr><tr><td colspan='10' style='font-size:10px;'>" + txtmsg + "</td></tr></table>");
                         //stud name
                         pghtml.Append("<table cellspacing='0' cellpadding='0' style='align:left;'><tr><td colspan='4' style='align:left;'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Course applied for:</span></td><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;'><span style='font-size:12px;font-family-Times New Roman;'>" + course + "-" + subj + " </span></td></tr><br/></br><tr><td colspan='5'></td><td colspan='5'><u><p style='text-align:center;font-size:15px;font-family:Times New Roman;font-weight:bold;'>Particulars of the applicant</p></u></td></tr><br><tr><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;''>Name:</td><td colspan='4'><span style='font-size:12px;font-family-Times New Roman;'>" + name + "</span></td></tr></table><br>");
                         //address
                         pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px; border:1px solid black;'><tr><th width='50%' style='border:1px solid black;'>Address for Communication(with pincode no)</th><th width='50%' style='border:1px solid black;'>Permanent Address(with pincode no)</th><tr><tr><td width='50%' style='border:1px solid black;'>" + padd1 + "</td><td width='50%' style='border:1px solid black;'>" + cadd1 + "</td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd2 + "</td><td width='50%' style='border:1px solid black;'> " + cadd2 + " </td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd3 + "</td><td width='50%' style='border:1px solid black;'> " + cadd3 + "</td></tr><tr><tr><td width='50%' style='border:1px solid black;'>" + padd4 + "</td><td width='50%' style='border:1px solid black;'> " + cadd4 + "</td></tr><tr><td style='align:left; border:1px solid black;'>Email<span>: " + email + "</span></td><td style='align:left; border:1px solid black;'>Mobile no<span>: " + mblno + "</span></td></tr></table><br>");
                         //nation
                         int cal_maxmark = 0;
                         int cal_autalmark = 0;
                         int ccount = 0;
                         int cal_intermark = 0;
                         int cal_extermark = 0;
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Nationality</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + nation + "</span></td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Date of birth</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + dob + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Place of birth</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + placbth + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Religion</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + relig + "</span></td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Community</span></td><td colspan='10' style='align:left;'>:<span style='font-size:12px;font-family-Times New Roman;'> " + communty + "</span> &nbsp;&nbsp;&nbsp;</td></tr><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Blood Group</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + bldgrp + "</span></td><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Mother Tongue</span></td><td colspan='4'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mothtng + "</span></td></tr><tr><td colspan='10'><hr></td></tr><tr><td colspan='3'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether Differently-abled</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + disbld + "</span></td><td colspan='7'>  If yes, attach relevant documents</td></tr><tr><td colspan='3'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether son of Ex-Serviceman</span></td><td>:<span style='font-size:12px;font-family-Times New Roman;'> " + exman + "</span><br></td><td colspan='7'>  If yes, attach relevant documents<br><br></td></tr><tr><td colspan='10' style='align:center;'><u><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><center>Particular of the Parent/Guardian</center></span></u><br></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Father</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + fname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='2'>: <span style='font-size:12px;font-family-Times New Roman;'>" + foccup + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='2'>: <span style='font-size:12px;font-family-Times New Roman;'>Rs. " + fannulincm + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + fcontno + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + femailid + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Mother</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> " + moccup + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='2'>:<span style='font-size:12px;font-family-Times New Roman;'> Rs. " + mannulincm + "</span></td><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + mcontno + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + memailid + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the guardian</span></td><td colspan='8'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gname + "</span></td></tr><tr><td><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gcntno + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id<span></td><td colspan='3'>:<span style='font-size:12px;font-family-Times New Roman;'> " + gemailid + "</span><br><br></td></tr><tr><td colspan='10'><center><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><u>Particulars of previous record</u><br></span></center><br></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Qualifying exam passed</span></td><td colspan='5'>:<span style='font-size:12px;font-family-Times New Roman;'> " + qlexampas + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the University</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + nameofuni + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Inistitution last attended:</span></td><td colspan='5'>:<span style='font-size:12px;font-family-Times New Roman;'> " + instlstatnd + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Register No</span></td><td colspan='3'>: <span style='font-size:12px;font-family-Times New Roman;'>" + regnumb + "</span><td></tr></table><br/><br/><br/><br/><br/><br/><br/><br/>");
                         pghtml.Append("<table style='width:650px;'><tr><td colspan='10'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Extract of the mark statement/s of the qualifying examination (attach attested copies)</span></td></tr></table><br>");
                         if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                         {
                             string nameval = "";
                             if (ISGARDE == "False")
                             {
                                 pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:755px;border:1px solid black;'><tr><th style='border:1px solid black;'>Subject</th><th style='border:1px solid black;'>Internal Mark</th><th style='border:1px solid black;'>External Mark</th><th style='border:1px solid black;'>Marks Obtained</th><th style='border:1px solid black;'>Max Marks</th><th style='border:1px solid black;'>Month & year of Passing</th></tr>");
                             }
                             else
                             {
                                 pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:755px;border:1px solid black;'><tr><th style='border:1px solid black;'>Subject</th><th style='border:1px solid black;'>Internal Grade</th><th style='border:1px solid black;'>External Grade</th><th style='border:1px solid black;'>Grade</th><th style='border:1px solid black;'>Month & year of Passing</th></tr>");
                             }
                             for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
                             {
                                 ccount++;
                                 string subcode = Convert.ToString(dsm.Tables[0].Rows[i]["psubjectno"]);
                                 nameval = subjectcode(subcode);
                                 string autalmark = Convert.ToString(dsm.Tables[0].Rows[i]["acual_marks"]);
                                 string maxmark = Convert.ToString(dsm.Tables[0].Rows[i]["max_marks"]);
                                 string pasyr = Convert.ToString(dsm.Tables[0].Rows[i]["passmnth"]);
                                 string grd = Convert.ToString(dsm.Tables[0].Rows[i]["grade"]);
                                 string regino = Convert.ToString(dsm.Tables[0].Rows[i]["registerno"]);
                                 string inter = "";
                                 string exter = "";
                                 if (ISGARDE == "False")
                                 {
                                     inter = Convert.ToString(dsm.Tables[0].Rows[i]["InternalMark"]);
                                     exter = Convert.ToString(dsm.Tables[0].Rows[i]["ExternalMark"]);
                                 }
                                 else
                                 {
                                     inter = Convert.ToString(dsm.Tables[0].Rows[i]["InternalGrade"]);
                                     exter = Convert.ToString(dsm.Tables[0].Rows[i]["ExternalGrade"]);
                                 }
                                 if (ISGARDE == "False")
                                 {
                                     if (cal_maxmark == 0)
                                     {
                                         cal_maxmark = Convert.ToInt32(maxmark);
                                     }
                                     else
                                     {
                                         cal_maxmark = cal_maxmark + Convert.ToInt32(maxmark);
                                     }
                                     if (cal_autalmark == 0)
                                     {
                                         cal_autalmark = Convert.ToInt32(autalmark);
                                     }
                                     else
                                     {
                                         cal_autalmark = cal_autalmark + Convert.ToInt32(autalmark);
                                     }
                                     if (cal_intermark == 0)
                                     {
                                         cal_intermark = Convert.ToInt32(inter);
                                     }
                                     else
                                     {
                                         cal_intermark = cal_intermark + Convert.ToInt32(inter);
                                     }
                                     if (cal_extermark == 0)
                                     {
                                         cal_extermark = Convert.ToInt32(exter);
                                     }
                                     else
                                     {
                                         cal_extermark = cal_extermark + Convert.ToInt32(exter);
                                     }
                                 }
                                 if (ISGARDE == "False")
                                 {
                                     pghtml.Append("<tr><td style='border:1px solid black;'>" + nameval + "</td><td  style='border:1px solid black;'><center>" + inter + "</center></td><td  style='border:1px solid black;'><center>" + exter + "</center></td><td  style='border:1px solid black;'><center>" + autalmark + "</center></td><td  style='border:1px solid black;'><center>" + maxmark + "</center></td><td  style='border:1px solid black;'><center>" + pasyr + "</center></td></tr>");
                                 }
                                 else
                                 {
                                     pghtml.Append("<tr><td style='border:1px solid black;'>" + nameval + "</td><td  style='border:1px solid black;'><center>" + inter + "</center></td><td  style='border:1px solid black;'><center>" + exter + "</center></td><td  style='border:1px solid black;'><center>" + grd + "</center></td><td  style='border:1px solid black;'><center>" + pasyr + "</center></td></tr>");
                                 }
                             }
                             pghtml.Append("</table>");
                         }
                         string tolinterpercentage = "";
                         string tolexterpercentage = "";
                         string tolpercentage1 = "";
                         if (ISGARDE == "False")
                         {
                             int tolpercentage = cal_autalmark / ccount;
                             int tolinterpercentage1 = cal_intermark / ccount;
                             int tolexterpercentage1 = cal_extermark / ccount;
                             tolpercentage1 = Convert.ToString(tolpercentage);
                             tolinterpercentage = Convert.ToString(tolinterpercentage1);
                             tolexterpercentage = Convert.ToString(tolexterpercentage1);
                             pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:755px;border:1px solid black;'><tr><td style='border:1px solid black; width:255px'>Total Marks Scord/Max.Marks: " + cal_autalmark + "/" + cal_maxmark + "</center></td><td style='border:1px solid black; width:160px'>Internal Percentage:" + tolinterpercentage + "%</td><td style='border:1px solid black; width:163px'>External Percentage:" + tolexterpercentage + "%</td><td style='border:1px solid black;width:154px'>Total Percentage: " + tolpercentage + "%</td></tr></table>");
                         }
                         else
                         {
                             tolinterpercentage = "";
                             tolexterpercentage = "";
                             // pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:755px;border:1px solid black;'><tr><td style='border:1px solid black; width:255px'>Total Marks Scord/Max.Marks: " + cal_autalmark + "/" + cal_maxmark + "</center></td><td style='border:1px solid black; width:160px'>Internal Percentage:" + tolinterpercentage + "%</td><td style='border:1px solid black; width:163px'>External Percentage:" + tolexterpercentage + "%</td><td style='border:1px solid black;width:154px'>Total Percentage: " + tolpercentage + "%</td></tr></table>");
                         }
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Total percentage of marks in all subjects(Language/major/Allied/Ancillary/Elective inclusive ofTheory and Practical)</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["percentage"]) + "</span></td></tr></table>");
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Total % of Marks in Major subjects alone(Including theory & Practicals)</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["majorallied_percent"]) + "</span></td></tr></table>");
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> Total percentage marks in major/Allied/Ancillary subjects alone inclusive of Theory and Practicals</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["major_percent"]) + "</span></td></tr></table>");
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> Total percentage of Internal Mark</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["InternalPercentage"]) + "</span></td></tr></table>");
                         pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'> Total percentage of External Mark</span></td><td colspan='8'>: <span style='font-size:12px;font-family-Times New Roman;'>" + Convert.ToString(ds1.Tables[1].Rows[0]["ExternalPercentage"]) + "</span></td></tr></table>");
                         pghtml.Append("<table><tr><td><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>List of enclosures:</span></td></tr><tr><td colspan='3'>1_________________</td><td colspan='2'>2_________________________</td><td colspan='4'>3_________________</td></tr><tr><td colspan='3'>4_________________</td><td colspan='2'>5_________________________</td><td colspan='4'>6_________________</td></tr><tr><td colspan='10' ><span style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'>Declaration :</span>&nbsp;&nbsp;I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and regulations of the college, and will not take part in any activity prejudicial to the interests of the college. Failing so, I agree to abide by any disciplinary action taken against me.</td></tr><tr><td colspan='10'><span style='font-size:14px;font-family:Times New Roman;font-weight:bold;'>உறுதி மொழி :</span>&nbsp;&nbsp;<span style='font-size:12px;font-family-Times New Roman;'>மேலே குறிப்பிட்டுள்ள விவரங்கள் உண்மையானவை, சரியானவை. நான் இக்கல்லூரியின் சட்டங்களுக்கும் நடத்தை விதிகளுக்கும் கட்டுப்பட்டு நடப்பேன். கல்லூரிக்கு அவப்பெயர் விளைவிக்கும்  எந்தச் செயலிலும் ஈடுபட மாட்டேன். தவறினால், தாங்கள் எடுக்கும் எவ்வகையான நடவடிக்கைக்கும் கட்டுப்படுவேன்.</span></td></tr></table>");
                         //pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;' >Place: </td></tr><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'>Date: </td></tr></table>");
                         pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='width:50%; align:left;'><center>____________________</center></td><td colspan='5' style='width:50%; align:right;'></td></tr><tr><td colspan='5' style='width:50%; align:left;'><center>Signature of the student</center></td><td colspan='5' style='width:50%; align:right;'></td></tr></table><br>");
                         //pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'><center>______________________________For office use only _______________________________</center> </td></tr></table><br>");
                         //pghtml.Append("<table style='width;650px;'><tr><td colspan='10'>Interviewed on_________________________________________ </td></tr></table><br>");
                         //pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='align:left;'>Admitted in__________</td><td colspan='5' style='float:right;'><center>by________(Staff No:_________)<br>signature of the staff</center></td></tr></table><br>");
                         //pghtml.Append("<table><tr><td colspan='3'style='align:left;'>Admitted in_____________</td><td colspan='6'><center></center>by______________________</td><td colspan='3' style='align:right;'>(staff no:____________)</td></tr></table>");
                         pghtml.Append("<center><table style='width;650px;'><tr><td colspan='10'><center>________________</center></td></tr><tr><td colspan='10' ><center>Principal</center></td></tr></table></center>");
                         pghtml.Append("</td></tr></table></div>");
                         contentDiv.InnerHtml += pghtml.ToString();*/
                        #endregion
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
            //contentDiv.Visible = true;
            //ScriptManager.RegisterStartupScript(this, GetType(), "btn_pdf", "PrintDiv();", true);
        }
        catch
        { }
    }
    protected void ugPdfapplication()
    {
        try
        {
            string collegeName = string.Empty;
            string collegeCateg = string.Empty;
            string collegeAff = string.Empty;
            string collegeAdd = string.Empty;
            string collegePhone = string.Empty;
            string collegeFax = string.Empty;
            string collegeWeb = string.Empty;
            string collegeEmai = string.Empty;
            string collegePin = string.Empty;
            string City = string.Empty;
            string shift = "";
            FpSpread.SaveChanges();
            for (int sel = 0; sel < FpSpread.Sheets[0].Rows.Count; sel++)
            {
                if (sel == 0)
                    continue;
                int value = Convert.ToInt32(FpSpread.Sheets[0].Cells[sel, 1].Value);
                if (value == 1)
                {
                    string appno = Convert.ToString(FpSpread.Sheets[0].Cells[sel, 0].Tag);
                    if (appno != "")
                    {
                        Session["pdfapp_no"] = appno;
                        string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        }
                        string query = "select IsExService,parentF_Mobile,Degree_Code,bldgrp,parent_income,emailp,mother,motherocc,mIncome,parentM_Mobile,emailM,guardian_name,guardian_mobile,emailg,Aadharcard_no,place_birth,app_formno,CONVERT(varchar(10), date_applied,103) as date_applied,stud_name,sex,Relationship,parent_name,CONVERT(varchar(10), dob,103) as dob ,parent_occu,mother_tongue,religion,citizen,community,caste,TamilOrginFromAndaman,IsExService,handy,visualhandy,first_graduate,CampusReq,DistinctSport,co_curricular,parent_addressC,Streetc,Cityc,parent_statec,Countryc,parent_pincodec,Student_Mobile,StuPer_Id,parent_phnoc,alter_mobileno,parent_addressP,Streetp,cityp,parent_statep,Countryp,parent_pincodep,parent_phnop,degree_code,batch_year,college_code,SubCaste,isdisable ,isdisabledisc,islearningdis,missionarydisc,MissionaryChild,seattype,current_semester,ncccadet from applyn a where a.app_no='" + Convert.ToString(Session["pdfapp_no"]) + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        query = query + " select instaddress,course_entno,course_code,university_code,Institute_name,percentage,instaddress,medium,branch_code ,Part1Language,Part2Language,Vocational_stream,isgrade,uni_state,registration_no,type_semester,majorallied_percent,major_percent,type_major,tancet_mark from Stud_prev_details where app_no ='" + Convert.ToString(Session["pdfapp_no"]) + "' ";
                        query = query + " select * from perv_marks_history ";
                        ds1.Clear();
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        //mark history
                        string coursno = d2.GetFunction("select course_entno from Stud_prev_details where app_no='" + appno + "'");
                        string SelMQ = "select psubjectno,registerno,acual_marks,max_marks,(pass_month+'-'+pass_year)as passmnth,noofattempt from perv_marks_history where course_entno='" + coursno + "'";
                        DataSet dsm = new DataSet();
                        dsm.Clear();
                        dsm = d2.select_method_wo_parameter(SelMQ, "Text");
                        //course and dept
                        string SelctC = "select Course_Name,Dept_Name from Course c,Degree d,Department dt where c.Course_Id=d.Course_Id  and dt.Dept_Code=d.Dept_Code and  d.Degree_Code='" + Convert.ToString(ds1.Tables[0].Rows[0]["Degree_Code"]) + "'";
                        DataSet dsc = new DataSet();
                        dsc.Clear();
                        dsc = d2.select_method_wo_parameter(SelctC, "Text");
                        string regno = "";
                        string applno = Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]);
                        string langxth = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["medium"]));
                        string langxiith = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["medium"]));
                        string course = Convert.ToString(dsc.Tables[0].Rows[0]["Course_Name"]);
                        string subj = Convert.ToString(dsc.Tables[0].Rows[0]["Dept_Name"]);
                        string name = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                        string padd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]);
                        string padd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]);
                        string padd3 = Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]);
                        string padd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statep"]));
                        string cadd1 = Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressC"]);
                        string cadd2 = Convert.ToString(ds1.Tables[0].Rows[0]["Streetc"]);
                        string cadd3 = Convert.ToString(ds1.Tables[0].Rows[0]["Cityc"]);
                        string cadd4 = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["parent_statec"]));
                        string email = Convert.ToString(ds1.Tables[0].Rows[0]["StuPer_Id"]);
                        string mblno = Convert.ToString(ds1.Tables[0].Rows[0]["Student_Mobile"]);
                        string nation = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["citizen"]));
                        string dob = Convert.ToString(ds1.Tables[0].Rows[0]["dob"]);
                        string placbth = Convert.ToString(ds1.Tables[0].Rows[0]["place_birth"]);
                        string relig = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["religion"]));
                        string communty = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["community"]));
                        string bldgrp = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"]));
                        string mothtng = subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["mother_tongue"]));
                        string disbld = Convert.ToString(ds1.Tables[0].Rows[0]["isdisable"]);
                        if (Convert.ToBoolean(disbld) == true)
                            disbld = "YES";
                        else
                            disbld = "NO";
                        string exman = Convert.ToString(ds1.Tables[0].Rows[0]["IsExService"]);
                        if (exman == "1")
                            exman = "YES";
                        else
                            exman = "NO";
                        string fname = Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]);
                        string foccup = Convert.ToString(ds1.Tables[0].Rows[0]["parent_occu"]);
                        if (foccup == "0")
                            foccup = "";
                        else
                            foccup = subjectcode(foccup);
                        string fannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["parent_income"]);
                        string fcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]);
                        string femailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailp"]);
                        string mname = Convert.ToString(ds1.Tables[0].Rows[0]["mother"]);
                        string moccup = Convert.ToString(ds1.Tables[0].Rows[0]["motherocc"]);
                        if (moccup == "0")
                            moccup = "";
                        else
                            moccup = subjectcode(moccup);
                        string mannulincm = Convert.ToString(ds1.Tables[0].Rows[0]["mIncome"]);
                        string mcontno = Convert.ToString(ds1.Tables[0].Rows[0]["parentM_Mobile"]);
                        string memailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailM"]);
                        string gname = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_name"]);
                        string gcntno = Convert.ToString(ds1.Tables[0].Rows[0]["guardian_mobile"]);
                        string gemailid = Convert.ToString(ds1.Tables[0].Rows[0]["emailg"]);
                        string qlexampas = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["course_code"]));
                        string nameofuni = subjectcode(Convert.ToString(ds1.Tables[1].Rows[0]["university_code"]));
                        string instlstatnd = Convert.ToString(ds1.Tables[1].Rows[0]["Institute_name"]);
                        string Timing = "";
                        if (cbclgtme.Checked == true)
                        {
                            if (txtclg.Text != "")
                            {
                                Timing = Convert.ToString(txtclg.Text);
                            }
                            else
                            {
                                Timing = "";
                            }
                        }
                        string atbt = "";
                        if (cbatbtnme.Checked == true)
                        {
                            atbt = "AT/BT/NME";
                        }
                        else
                        {
                            atbt = "";
                        }
                        string photo = "";
                        string txt1 = "    * Application number for Downloaded application will be alloted & initimated to the candidates by the college office on its receipts along with the prescribed application fee of Rs. in the form of crossed DD drawn in favour of the The Prinicipal,The New  College,chennai 600 014";
                        string txtmsg = "[Please read the prospectus carfully filling up the application form. Use CAPITAL LETTERS only]";
                        contentDiv.InnerHtml = "";
                        StringBuilder pghtml = new StringBuilder();
                        //registration
                        pghtml.Append("<div style='padding-left:5px;height:900px; width:650px;'><table><tr><td colspan='5'><table cellpadding='0' cellspacing='0' style='width:650px;'><tr><td colspan='5' style='font-family:Times New Roman;font-weight:bold;font-size:15px;'>Application No:<u>" + regno + "</u></td><td colspan='5' style='font-family:Times New Roman;font-weight:bold;font-size:15px;'>Registration No: <u>" + regno + "</u></td></tr></table>");
                        //logo and clg details
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;'><tr><td style='align:left;  border:1px solid black;'><img src='" + "college/Left_Logo.jpeg" + "' style='height:80px; width:70px;'/></td><td colspan='6' style='font-size:12px;font-family:Times New Roman;font-weight:bold; border:1px solid black;text-align:center;'><span>" + collname + "<br>" + address1 + " , " + address2 + " , " + address3 + " - " + pincode + ".  INDIA" + "<br>" + affliated + "</span><br><hr><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>APPLICATION FOR ADMISSION<br>U.G.COURSES - (2016-19)<br>" + Timing + "</span></td><td colspan='3' style='font-size:15px;font-family:Times New Roman;font-weight:bold; height:90px; width:20%; border:1px solid black;'>" + photo + "</td></tr><tr><td colspan='10' style='font-size:10px;'>" + txtmsg + "</td></tr></table>");
                        //stud name
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='align:left;'><tr><td colspan='4' style='align:left;'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Course applied for:</span></td><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;'><span style='font-family:Times New Roman;font-size:12px;'>" + course + "-" + subj + " </span></td></tr>");
                        if (cbpartlang.Checked == true)
                        {
                            pghtml.Append("<tr><td colspan='2'>PART-I Language</td><td colspan='8'>_______________________________________________________<br></td></tr>");
                        }
                        pghtml.Append("<tr><td colspan='10'><br><table cellspacing='0' cellpadding='0' style='border:1px solid black; width:650px;'><tr><td colspan='10' style='text-align:center; border:1px solid black;'>For office use:</td></tr><tr><td colspan='5' style='border:1px solid black;'>Admitted in &nbsp;____________________</td><td colspan='5' style='text-align:left; border:1px solid black;'>on &nbsp;____________________&nbsp;&nbsp;&nbsp; " + atbt + "</td></tr><tr><td colspan='5' style='border:1px solid black;'>Allied-1:&nbsp;___________________________</td><td colspan='5' style='border:1px solid black;'>Allied-2:&nbsp;_____________________________</td></tr></table><br></td></tr><tr><td style='font-size:15px;font-family:Times New Roman;font-weight:bold;''>Name:</td><td colspan='4'><span style='font-family:Times New Roman;font-size:12px;'>" + name + "</span></td></tr></table><br>");
                        //address
                        pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px; border:1px solid black;'><tr><th width='50%' style='border:1px solid black;'>Address for Communication(with pincode no)</th><th width='50%' style='border:1px solid black;'>Permanent Address(with pincode no)</th><tr><tr><td width='50%' style='border:1px solid black;'>" + padd1 + "</td><td width='50%' style='border:1px solid black;'>" + cadd1 + "</td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd2 + "</td><td width='50%' style='border:1px solid black;'> " + cadd2 + " </td></tr><tr><td width='50%' style='border:1px solid black;'>" + padd3 + "</td><td width='50%' style='border:1px solid black;'> " + cadd3 + "</td></tr><tr><tr><td width='50%' style='border:1px solid black;'>" + padd4 + "</td><td width='50%' style='border:1px solid black;'> " + cadd4 + "</td></tr><tr><td style='align:left; border:1px solid black;'>Email<span>: " + email + "</span></td><td style='align:left; border:1px solid black;'>Mobile no<span>: " + mblno + "</span></td></tr></table><br><br>");
                        //nation
                        pghtml.Append("<table cellpadding='0' cellspacing='0'><tr><td colspan='4' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Nationality</span></td><td colspan='6'>: <span style='font-family:Times New Roman;font-size:12px;'>" + nation + "</span></td></tr><tr><td colspan='4' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Date of birth</span></td><td colspan='6'>:<span style='font-family:Times New Roman;font-size:12px;'> " + dob + "</span></td></tr><tr><td colspan='4'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Place of birth</span></td><td colspan='6'>:<span style='font-family:Times New Roman;font-size:12px;'> " + placbth + "</span></td></tr><tr><td colspan='4'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Religion&community</span></td><td colspan='6'>:<span style='font-family:Times New Roman;font-size:12px;'> " + relig + " & " + communty + " </span>&nbsp;&nbsp;(attach photocopy)</td></tr><tr><td colspan='4' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Blood Group</span></td><td colspan='2'>:<span style='font-family:Times New Roman;font-size:12px;'> " + bldgrp + "</span></td><td style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Mother Tongue</span></td><td colspan='2'>: <span style='font-family:Times New Roman;font-size:12px;'>" + mothtng + "</span></td></tr><tr><td colspan='10'>Distinction / Participation in Sports / Athletics / NCC / NSS: (Specify and attach copies of documents)</td></tr><tr><td colspan='10'><hr></td></tr><tr><td colspan='4'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether Differently-abled</span></td><td>:<span style='font-family:Times New Roman;font-size:12px;'> " + disbld + "</span></td><td colspan='6'>  If yes, attach relevant documents</td></tr><tr><td colspan='4'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Whether son of Ex-Serviceman</span></td><td>: <span style='font-family:Times New Roman;font-size:12px;'>" + exman + "</span><br></td><td colspan='6'>  If yes, attach relevant documents<br><br></td></tr><tr><td colspan='10' style='align:center;'><u><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><center>Particular of the Parent/Guardian</center></span></u><br></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Father</span></td><td colspan='8'>:<span style='font-family:Times New Roman;font-size:12px;'> " + fname + "</span></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='3'>: <span style='font-family:Times New Roman;font-size:12px;'>" + foccup + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> Rs. " + fannulincm + "</span></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>: <span style='font-family:Times New Roman;font-size:12px;'>" + fcontno + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='3'>: <span style='font-family:Times New Roman;font-size:12px;'>" + femailid + "</span></td></tr><tr><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Mother</span></td><td colspan='8'>:<span style='font-family:Times New Roman;font-size:12px;'> " + mname + "</span></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Occupation</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> " + moccup + "</span></td><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Annual Income</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> Rs. " + mannulincm + "</span></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> " + mcontno + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> " + memailid + "</span></td></tr><tr><td colspan='3' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the guardian<br>(if living with guardian)</span></td><td colspan='7'>:<span style='font-family:Times New Roman;font-size:12px;'> " + gname + "</span></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Contact no</span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> " + gcntno + "</span></td><td colspan='2' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Email-id<span></td><td colspan='3'>:<span style='font-family:Times New Roman;font-size:12px;'> " + gemailid + "</span><br><br><br><br></td></tr><tr><td colspan='10'><center><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'><u>Particulars of previous Academic Record</u><br></span></center><br></td></tr><tr><td colspan='2'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Qualifying exam passed</span></td><td colspan='8'>: " + qlexampas + "&nbsp;&nbsp;&nbsp;</td></tr><tr><td colspan='3' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Name of the Board</span></td><td colspan='7'>:<span style='font-family:Times New Roman;font-size:12px;'> " + nameofuni + "</span></td></tr><tr><td colspan='3' style='align:left;'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>Inistitution last attended:</span></td><td colspan='7'>:<span style='font-family:Times New Roman;font-size:12px;'> " + instlstatnd + "</span></td></tr><tr><td colspan='2'>Language Studied in X-std</td><td colspan='3'>&nbsp;" + langxth + "</td><td colspan='3'>Language Studied in XII-std</td><td colspan='2'>&nbsp;<span style='font-family:Times New Roman;font-size:12px;'>" + langxiith + "</span></td></tr></table><br><br>");
                        pghtml.Append("<table style='width:650px;'><tr><td colspan='10'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>Extract of the mark statement/s of the qualifying examination (attach attested copies)</span></td></tr></table><br>");
                        if (dsm.Tables.Count > 0 && dsm.Tables[0].Rows.Count > 0)
                        {
                            string nameval = "";
                            double actlmk = 0;
                            double totmk = 0;
                            double cnt = 0;
                            pghtml.Append("<table cellspacing='0' cellpadding='0' style='width:650px;border:1px solid black;'><tr><th>Name of the Subject</th><th style='border:1px solid black;'>Marks Secured</th><th style='border:1px solid black;'>Maximum Marks</th><th style='border:1px solid black;'>Month & year of Passing</th><th style='border:1px solid black;'>No of Attempts</th></tr>");
                            for (int i = 0; i < dsm.Tables[0].Rows.Count; i++)
                            {
                                string subcode = Convert.ToString(dsm.Tables[0].Rows[i]["psubjectno"]);
                                nameval = subjectcode(subcode);
                                string autalmark = Convert.ToString(dsm.Tables[0].Rows[i]["acual_marks"]);
                                actlmk += Convert.ToDouble(autalmark);
                                string maxmark = Convert.ToString(dsm.Tables[0].Rows[i]["max_marks"]);
                                totmk += Convert.ToDouble(maxmark);
                                string pasyr = Convert.ToString(dsm.Tables[0].Rows[i]["passmnth"]);
                                string regino = Convert.ToString(dsm.Tables[0].Rows[i]["registerno"]);
                                string noofatmpt = Convert.ToString(dsm.Tables[0].Rows[i]["noofattempt"]);
                                cnt += Convert.ToDouble(noofatmpt);
                                pghtml.Append("<tr><td style='border:1px solid black;'>" + nameval + "</td><td  style='border:1px solid black;'><center>" + autalmark + "</center></td><td  style='border:1px solid black;'><center>" + maxmark + "</center></td><td  style='border:1px solid black;'><center>" + pasyr + "</center></td><td  style='border:1px solid black;'><center>" + noofatmpt + "</center></td></tr>");
                            }
                            pghtml.Append("<tr><td style='border:1px solid black; text-align:right;'>Total Mark Secured</td><td style='border:1px solid black; text-align:center;'>" + actlmk + "</td><td style='border:1px solid black; text-align:center;'>" + totmk + "</td><td tyle='border:1px solid black; text-align:center;'></td><td style='border:1px solid black; text-align:center;'>" + cnt + "</td></tr>");
                            pghtml.Append("</table>");
                        }
                        pghtml.Append("<table><tr><td><span style='font-size:15px;font-family:Times New Roman;font-weight:bold;'>List of enclosures:</span></td></tr><tr><td colspan='3'>1_________________</td><td colspan='2'>2_________________________</td><td colspan='4'>3_________________</td></tr><tr><td colspan='3'>4_________________</td><td colspan='2'>5_________________________</td><td colspan='4'>6_________________</td></tr><tr><td colspan='10'><span style='font-size:15px;font-family:Times New Roman;font-weight:bold; top-left:30px;'>Declaration :&nbsp;&nbsp;</span>I declare that the particulars furnished above are true and correct. I submit that I will abide by the rules and regulations of the college, and will not take part in any activity prejudicial to the interests of the college. Failing so, I agree to abide by any disciplinary action taken against me.</td></tr><tr><td colspan='10'><span style='font-size:12px;font-family:Times New Roman;font-weight:bold;'>உறுதி மொழி:</span>&nbsp;&nbsp;<span style='font-family:Times New Roman;font-size:12px;'>மேலே குறிப்பிட்டுள்ள விவரங்கள் உண்மையானவை, சரியானவை. நான் இக்கல்லூரியின் சட்டங்களுக்கும் நடத்தை விதிகளுக்கும் கட்டுப்பட்டு நடப்பேன். கல்லூரிக்கு அவப்பெயர் விளைவிக்கும்  எந்தச் செயலிலும் ஈடுபட மாட்டேன். தவறினால், தாங்கள் எடுக்கும் எவ்வகையான நடவடிக்கைக்கும் கட்டுப்படுவேன்.</span></td></tr></table>");
                        pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-family:Times New Roman; font-weight:bold;font-size:15px;'>Place: </td></tr><tr><td colspan='10' style='font-family:Times New Roman; font-weight:bold;font-size:15px;'>Date: </td></tr></table>");
                        pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='width:50%; align:left;'><center>____________________</center></td><td colspan='5' style='width:50%; align:right;'><center>___________________________________________________</center></td></tr><tr><td colspan='5' style='width:50%; align:left;'><center>Signature of the student</center></td><td colspan='5' style='width:50%; align:right;'><center>Counter Signature of the parent/Guardian</center></td></tr></table><br>");
                        pghtml.Append("<table style='width;650px;'><tr><td colspan='10' style='font-family:Times New Roman; font-weight:bold;font-size:15px;'><center>______________________________For office use only _______________________________</center> </td></tr></table><br>");
                        pghtml.Append("<table style='width;650px;'><tr><td colspan='10'>Interviewed on_________________________________________ </td></tr></table><br>");
                        //pghtml.Append("<table style='width;650px;'><tr><td colspan='5' style='align:left;'>Admitted in__________</td><td colspan='5' style='float:right;'><center>by________(Staff No:_________)<br>signature of the staff</center></td></tr></table><br>");
                        pghtml.Append("<table><tr><td colspan='3'style='align:left;'>Admitted in_____________</td><td colspan='6'><center></center>by______________________</td><td colspan='3' style='align:right;'>(staff no:____________)</td></tr></table>");
                        pghtml.Append("<center><table style='width;650px;'><tr><td colspan='10'><center>________________</center></td></tr><tr><td colspan='10' ><center>Principal</center></td></tr></table></center>");
                        pghtml.Append("</td></tr></table></div>");
                        contentDiv.InnerHtml += pghtml.ToString();
                    }
                }
            }
            contentDiv.Visible = true;
            ScriptManager.RegisterStartupScript(this, GetType(), "btn_pdf", "PrintDiv();", true);
        }
        catch
        { }
    }
    public string subjectcode(string textcri)
    {
        string subjec_no = " - ";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "'";// and college_code ='" + Session["collegecode"].ToString() + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
                if (subjec_no.Trim() == "")
                {
                    subjec_no = " - ";
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    public string subjectcode(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code ='" + ddl_collegename.SelectedItem.Value + "' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
            }
            else
            {
                string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + Convert.ToString(ddl_collegename.SelectedItem.Value) + " and TextVal='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    public void txtfrmdate_TextChanged(object sender, EventArgs e)
    {
        string dt = txtfrmdate.Text;
        string[] Split = dt.Split('/');
        DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        string current = DateTime.Now.ToString("dd/MM/yyyy");
        Split = current.Split('/');
        DateTime prvedate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        if (fromdate > prvedate)
        {
            txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Select Valid Date";
            //;
        }
    }
    public void txttodate_TextChanged(object sender, EventArgs e)
    {
        string dt = txttodate.Text;
        string[] Split = dt.Split('/');
        DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        string current = DateTime.Now.ToString("dd/MM/yyyy");
        Split = current.Split('/');
        DateTime prvedate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        if (fromdate > prvedate)
        {
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            imgdiv2.Visible = true;
            lbl_alert.Text = "Kindly Select Valid Date";
            //;
        }
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void ddlstatus_SelectedIndexChange(object sender, EventArgs e)
    {
        FpSpread.Visible = false;
        rprint.Visible = false;
        btn_pdf.Visible = false;
        lblsmserror.Text = "";
        txtexcel.Text = "";
    }
    protected void cbclgtme_OnCheckedChanged(object sender, EventArgs e)
    {
        if (cbclgtme.Checked == true)
        {
            txtclg.Enabled = true;
            txtclg.Text = "";
        }
        else
        {
            txtclg.Enabled = false;
            txtclg.Text = "";
        }
    }
    public void txt_searchstudname_TextChanged(object sender, EventArgs e)
    {
        if (txt_searchstudname.Text != "")
        {
            txt_searchappno.Text = "";
            btngo_click(sender, e);
        }
    }
    public void txt_searchappno_TextChanged(object sender, EventArgs e)
    {
        if (txt_searchappno.Text != "")
        {
            txt_searchstudname.Text = "";
            btngo_click(sender, e);
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select stud_name from applyn where isconfirm ='1' and ISNULL(admission_status,'0')=0 and stud_name like '" + prefixText + "%' and College_code='" + clgcode + "'";
        // string query = "select a.stud_name+'-'+ISNULL(  a.parent_name,'')+'-'+c.Course_Name+'-'+dt.Dept_Name+'-'+r.Roll_No,r.Roll_No from applyn a,Registration r ,Degree d,course c,Department dt  where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.CC=0 and r.DelFlag =0 and r.Exam_Flag <>'DEBAR' and a.stud_name like '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select app_formno from applyn where isconfirm ='1' and ISNULL(admission_status,'0')=0 and app_formno like '" + prefixText + "%' and College_code='" + clgcode + "'";
        name = ws.Getname(query);
        return name;
    }
    public void lnk_setting_Click(object sender, EventArgs e)
    {
        addsetting.Visible = true;
    }
    public void ImageButton_close_Click(object sender, EventArgs e)
    {
        addsetting.Visible = false;
    }
    public void btn_settingsave_Click(object sender, EventArgs e)
    {
        string qur = "";
        string linkname = "Declaration Form Setting";
        string columnvalue = "";
        string lang = "0";
        string at = "0";
        string time = "0";
        if (cbpartlang.Checked == true)
        {
            lang = "1";
        }
        if (cbclgtme.Checked == true)
        {
            time = "1";
            if (txtclg.Text == "")
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Timing Should Not Be Empty";
                return;
            }
        }
        if (cbatbtnme.Checked == true)
        {
            at = "1";
        }
        columnvalue = lang + "," + time + "," + at;
        qur = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddl_collegename.SelectedItem.Value + "' ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + ddl_collegename.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + ddl_collegename.SelectedItem.Value + "')";
        int s = d2.update_method_wo_parameter(qur, "text");
        qur = "  if exists(select * from textvaltable where TextCriteria='Ctime' and college_code='" + ddl_collegename.SelectedItem.Value + "' ) update textvaltable set TextVal='" + txtclg.Text + "' where TextCriteria='Ctime'  and college_code='" + ddl_collegename.SelectedItem.Value + "' else insert into textvaltable (TextCriteria,TextVal,college_code)values('Ctime','" + txtclg.Text + "','" + ddl_collegename.SelectedItem.Value + "')";
        s = d2.update_method_wo_parameter(qur, "text");
        if (s != 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Saved Successfully";
        }
    }
    public void loadsetting()
    {
        try
        {
            string qur = "";
            string linkname = "Declaration Form Setting";
            qur = "select * from New_InsSettings where LinkName='" + linkname + "' and user_code='" + usercode + "' and college_code='" + ddl_collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(qur, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string value = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                string[] sp = value.Split(',');
                if (sp[0] == "1")
                {
                    cbpartlang.Checked = true;
                }
                else
                {
                    cbpartlang.Checked = false;
                }
                if (sp[1] == "1")
                {
                    cbclgtme.Checked = true;
                    txtclg.Enabled = true;
                    txtclg.Text = d2.GetFunction("select TextVal from textvaltable where TextCriteria='Ctime' and college_code='" + ddl_collegename.SelectedItem.Value + "'");
                }
                else
                {
                    cbclgtme.Checked = false;
                    txtclg.Text = "";
                    txtclg.Enabled = false;
                }
                if (sp[2] == "1")
                {
                    cbatbtnme.Checked = true;
                }
                else
                {
                    cbatbtnme.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
        lbl.Add(lbl_collegename);
        fields.Add(0);
        lbl.Add(lblStr);
        fields.Add(1);
        lbl.Add(lblDeg);
        fields.Add(2);
        lbl.Add(lblBran);
        fields.Add(3);
        //lbl.Add(lbl_org_sem);
        //fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}
/*ug,pg and mphil pdf print generation 04.01.16*/