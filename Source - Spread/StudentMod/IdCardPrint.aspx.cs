using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Configuration;

public partial class IdCardPrint : System.Web.UI.Page
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
                txt_validdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_validdate.Attributes.Add("readonly", "readonly");
                loadstream();
                loadedulevel();
                BindBatch();
                Bindcourse();
                binddept();
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
        Response.Redirect("~/Default.aspx", false);
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
            degreecode = GetSelectedItemsValueAsString(cbldepartment);
            batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
            lbl_validdate.Visible = false;
            txt_validdate.Visible = false;

            string enrolltype = Convert.ToString(ddlenroll.SelectedItem.Value);
            string selquery = "";
            if (enrolltype.Trim() == "1")
            {
                selquery = "select app_formno,stud_name,batch_year,(c.Course_Name+' - '+Dept_Name) as Dept_Name from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='1' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + batchyear + "' and a.degree_code in ('" + degreecode + "') and enrollmentcard ='1' and admitcard_date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'  order by enrollment_card_date";
            }
            else
            {
                selquery = "select app_formno,r.stud_name,r.batch_year,(c.Course_Name+' - '+Dept_Name) as Dept_Name from applyn a,registration r,Degree d,Department dt,Course C where isconfirm='1' and r.app_no=a.app_no and admission_status ='1' and selection_status ='1' and is_enroll ='2' and r.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and r.batch_year ='" + batchyear + "' and r.degree_code in ('" + degreecode + "')   order by adm_date";//and adm_date between '" + dt1.ToString("MM/dd/yyyy") + "' and '" + dt2.ToString("MM/dd/yyyy") + "'

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread.Sheets[0].RowCount = 0;
                FpSpread.Sheets[0].ColumnCount = 6;
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
                FpSpread.Columns[0].Locked = true;
                FpSpread.Columns[0].Width = 50;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread.Columns[1].Width = 80;

                FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                chkcell.AutoPostBack = false;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "App Form No";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread.Columns[2].Locked = true;
                FpSpread.Columns[2].Width = 125;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread.Columns[3].Locked = true;
                FpSpread.Columns[3].Width = 175;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch Year";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                FpSpread.Columns[4].Locked = true;
                FpSpread.Columns[4].Width = 175;

                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Text = lblDeg.Text + "/" + lblBran.Text;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                FpSpread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                FpSpread.Columns[5].Locked = true;
                FpSpread.Columns[5].Width = 200;

                FpSpread.Sheets[0].RowCount++;
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread.Sheets[0].RowCount++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[0, 1].CellType = chkall;
                    FpSpread.Sheets[0].Cells[0, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[0, 1].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].CellType = chkcell;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Value = 0;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_formno"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["stud_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["batch_year"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Bold = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                mainpgeerr.Visible = false;
                FpSpread.Visible = true;
                //btncoverprint.Visible = true;
                //btninsurprnt.Visible = true;
                rprint.Visible = true;
                FpSpread.Height = 500;
                FpSpread.Width = 820;
                btn_pdf.Visible = true;
                lbl_validdate.Visible = true;
                txt_validdate.Visible = true;
            }
            else
            {
                FpSpread.Visible = false;
                //btncoverprint.Visible = false;
                //btninsurprnt.Visible = false;
                rprint.Visible = false;
                mainpgeerr.Visible = true;
                btn_pdf.Visible = false;
                mainpgeerr.Text = "No Record Found!";
                lbl_validdate.Visible = false;
                txt_validdate.Visible = false;
            }
        }
        catch { }
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
            if (ddl_collegename.Items.Count > 0)
            {
                build = Convert.ToString(ddledulevel.SelectedItem.Value);
                build2 = GetSelectedItemsValueAsString(cbldegree);
                if (build != "" && build2 != "")
                {
                    string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
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
        }
        catch (Exception ex) { }
    }

    public void loadstream()
    {
        try
        {
            if (ddl_collegename.Items.Count > 0)
            {
                ddltype.Items.Clear();
                string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "')";
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
            if (ddl_collegename.Items.Count > 0)
            {
                if (ddltype.Enabled)
                {
                    itemheader = Convert.ToString(ddltype.SelectedItem.Value);
                    deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + itemheader + "') and college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "') order by Edu_Level desc";
                }
                else
                {
                    deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "') order by Edu_Level desc";
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
            if (ddl_collegename.Items.Count > 0)
            {
                cbldegree.Items.Clear();
                string build = "";
                string build1 = "";
                build = Convert.ToString(ddledulevel.SelectedItem.Value);
                if (build != "")
                {
                    string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
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
                        collquery = "select collname,category,university,address1,address2,address3,phoneno,faxno,email,website,district,state,pincode  from collinfo where college_Code=" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "";
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

                        string getstudinfn = "select stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='2' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and batch_year ='" + Convert.ToString(ddlbatch.SelectedItem.Text) + "' and a.degree_code in ('" + degreecode + "') and app_formno='" + appformno + "'";
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
                            string bldgrp = d2.GetFunction("select TextVal from TextValTable where TextCriteria like 'bgrou' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[0]["bldgrp"]) + "'");
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
                            string getfatherocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='foccu' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                            if (getfatherocc.Trim() != "" && getfatherocc.Trim() != "0")
                            {
                                fatheroccupation = getfatherocc;
                            }
                            table2.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(1, 3).SetContent(Convert.ToString(fatheroccupation));

                            string fatherinc = "";
                            string getfatherinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='fin' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
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
                            string getmotherocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='moccu' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                            if (getmotherocc.Trim() != "" && getmotherocc.Trim() != "0")
                            {
                                motheroccupation = getmotherocc;
                            }
                            table2.Cell(2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(2, 3).SetContent(Convert.ToString(getmotherocc));

                            string motherinc = "";
                            string getmotherinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='min' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
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
                            string getguardianocc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='moccu' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
                            if (getguardianocc.Trim() != "" && getguardianocc.Trim() != "0")
                            {
                                guardianoccupation = getguardianocc;
                            }
                            table2.Cell(3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table2.Cell(3, 3).SetContent(Convert.ToString(guardianoccupation));

                            string guardianinc = "";
                            string getguardianinc = d2.GetFunction("select TextVal from TextValTable where TextCriteria='min' and college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "'");
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
    public void btn_pdf_click(object sender, EventArgs e)
    {
        //pdf();
        try
        {
            string checkvalue = "";


            if (checkok() == true)
            {
                DAccess2 da = new DAccess2();
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                Font header = new Font("Arial", 7, FontStyle.Bold);
                Font header1 = new Font("Arial", 7, FontStyle.Bold);
                Font Fonthead = new Font("Arial", 2, FontStyle.Bold);
                Font Fontbold1 = new Font("Times New Roman", 7, FontStyle.Bold);
                Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
                Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
                Font Fontsmall = new Font("Arial", 7, FontStyle.Regular);
                Font Fontsmalll = new Font("Arial", 6, FontStyle.Regular);
                Font FontsmallBold = new Font("Arial", 6, FontStyle.Bold);
                Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
                Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                FpSpread.SaveChanges();
                int count = 0;

                int x = 14;
                int y = 12; int w = 241; int h = 152; int x1 = 340; int y1 = 12; int w1 = 241; int h1 = 152;
                int liney = 0;
                int liney1 = 0;
                int linex = 0;
                int linew = 300;
                int lineh = 100;
                int clgx1 = 0; int clgy1 = 0; int clgw1 = 0; int clgh1 = 0; int clgy2 = 0;
                int clgy3 = 0; int clgy4 = 0;
                int headx = 0; int heady = 0; int headw = 0; int headh = 0;
                int hrc1 = 0; int hry = 0; int hrc2 = 0; int arc1 = 0; int hrc3 = 0; int arc3 = 0; int pnc = 0;
                int logoy = 0; int logow = 0; int studimg = 0; int imgy = 0; int backy = 0; int hold = 0;
                int dd = 0;
                int spreadCNt = 0;
                for (int i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {
                        spreadCNt++;
                    }
                }
                for (int i = 1; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);
                    if (checkvalue == "1")
                    {
                        string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        string phone = "";
                        string fax = "";
                        string email = "";
                        string website = "";
                        string category = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            category = ds.Tables[0].Rows[0]["category"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            phone = ds.Tables[0].Rows[0]["phoneno"].ToString();
                            fax = ds.Tables[0].Rows[0]["faxno"].ToString();
                            email = ds.Tables[0].Rows[0]["email"].ToString();
                            website = ds.Tables[0].Rows[0]["website"].ToString();
                        }

                        count++;
                        if (count == 1)
                        {
                            mypdfpage = mydocument.NewPage();
                        }

                        string app_no = Convert.ToString(Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text));
                        Session["pdfapp_no"] = Convert.ToString(app_no);
                        string enrolltype = Convert.ToString(ddlenroll.SelectedItem.Value);

                        string isenro = "";
                        bool isEnrollment = false;
                        if (enrolltype.Trim() == "1")
                        {
                            isenro = " and is_enroll ='1'";
                            isEnrollment = true;
                        }
                        else
                        {
                            isenro = "";
                        }
                        PdfTextArea ptc;
                        string query = "select '' reg_no,parentF_Mobile,app_formno,type, a.stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,a.batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where a.degree_code=d.Degree_Code and a.college_code=d.college_code and d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and isconfirm='1' and admission_status ='1' and selection_status ='1' and  app_formno='" + app_no + "'";

                        if (!isEnrollment)
                            query = "select r.reg_no,parentF_Mobile,app_formno,type, r.stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,r.batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from Registration r,applyn a,Degree d,Department dt,Course C where r.app_no=a.app_no and r.degree_code=d.degree_code and isconfirm='1' and admission_status ='1' and selection_status ='1' " + isenro + "  and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and  app_formno='" + app_no + "'";
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            linex = 14;
                            linew = 300;
                            lineh = 100;
                            clgw1 = 132;
                            clgh1 = 27;
                            clgx1 = 68;
                            headw = 132; headh = 27; headx = 68; pnc = 190;
                            hrc1 = 85; hrc2 = 67;
                            #region ///..........  /// ..........id size...............

                            if (count == 1)
                            {
                                x = 14;
                                y = 12;
                                w = 241;
                                h = 152;
                                x1 = 340;
                                y1 = 12;
                                w1 = 241;
                                h1 = 152;
                                liney = 50;
                                liney1 = 140;
                                clgy1 = 13;
                                clgy2 = 20;
                                clgy3 = 27;
                                clgy4 = 34;
                                heady = 54;
                                hrc2 = 67;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 14;
                                studimg = 78;
                                imgy = 58;
                                backy = 14;
                                hold = 140;
                            }
                            else if (count == 2)
                            {
                                x = 14;
                                y = 221;
                                w = 241;
                                h = 152;
                                x1 = 340;
                                y1 = 221;
                                w1 = 241;
                                h1 = 152;
                                liney = 259;
                                liney1 = 349;
                                clgy1 = 222;
                                clgy2 = 229;
                                clgy3 = 236;
                                clgy4 = 243;
                                heady = 265;
                                backy = 221 + 2;
                                hrc2 = 278;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                studimg = 221 + 66;
                                logoy = 224;
                                hold = 349;
                                imgy = 46 + 221;
                            }
                            else if (count == 3)
                            {
                                x = 14;
                                y = 430;
                                w = 241;
                                h = 152;
                                imgy = 46 + 430;
                                x1 = 340;
                                y1 = 430;
                                w1 = 241;
                                h1 = 152;
                                liney = 468;
                                liney1 = 558;
                                clgy1 = 431;
                                clgy2 = 438;
                                clgy3 = 445;
                                clgy4 = 452;
                                heady = 476;
                                studimg = 430 + 66;
                                hrc2 = 489;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 432;
                                hold = 558;
                                backy = 430 + 2;
                            }
                            else if (count == 4)
                            {
                                x = 14;
                                imgy = 46 + 639;
                                y = 639;
                                w = 241;
                                h = 152;
                                studimg = 639 + 66;
                                x1 = 340;
                                y1 = 639;
                                w1 = 241;
                                h1 = 152;
                                liney = 677;
                                liney1 = 767;
                                clgy1 = 640;
                                clgy2 = 647;
                                clgy3 = 654;
                                clgy4 = 661;
                                heady = 683;
                                backy = 639 + 2;
                                hrc2 = 696;
                                arc1 = 130;
                                hrc3 = 170;
                                arc3 = 215;
                                logoy = 642;
                                hold = 767;
                            }
                            #endregion

                            #region
                            ///.........................................
                            PdfArea pa1 = new PdfArea(mydocument, x, y, w, h);
                            PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                            mypdfpage.Add(pr3);

                            PdfArea pa2 = new PdfArea(mydocument, x1, y1, w1, h1);
                            PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                            mypdfpage.Add(pr22);


                            PdfArea pa3 = new PdfArea(mydocument, x, liney1 + 10, 240, 13);
                            if (Convert.ToString(ds1.Tables[0].Rows[0]["type"]) == "DAY")
                            {
                                PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Maroon);
                                pr222.Fill(Color.Maroon);
                                mypdfpage.Add(pr222);
                            }
                            else if (Convert.ToString(ds1.Tables[0].Rows[0]["type"]) == "Evening")
                            {
                                PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Green);
                                pr222.Fill(Color.Green);
                                mypdfpage.Add(pr222);
                            }
                            else
                            {
                                PdfRectangle pr222 = new PdfRectangle(mydocument, pa3, Color.Maroon);
                                pr222.Fill(Color.Maroon);
                                mypdfpage.Add(pr222);
                            }



                            //...................................................
                            #endregion

                            #region
                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                               new PdfArea(mydocument, linex, liney - 15, linew, lineh), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                          new PdfArea(mydocument, linex, liney1, linew, lineh), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                            mypdfpage.Add(ptc);


                            #endregion

                            #region ///............... college details...............
                            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, clgx1, clgy1 - 3, clgw1 + 30, clgh1), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(collname) + " (" + category + ")");

                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, clgx1, clgy2, clgw1 + 30, clgh1), System.Drawing.ContentAlignment.MiddleCenter, address1 + "," + address3 + "," + pincode);
                            mypdfpage.Add(ptc);

                            /// 
                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                       new PdfArea(mydocument, clgx1, clgy3, clgw1, clgh1), System.Drawing.ContentAlignment.MiddleCenter, "ph:" + phone + "," + "Fax:" + fax);
                            //mypdfpage.Add(ptc);

                            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                      new PdfArea(mydocument, clgx1, clgy4, clgw1, clgh1), System.Drawing.ContentAlignment.MiddleCenter, "Email:" + email + "," + "Website:" + website);
                            //mypdfpage.Add(ptc);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                                mypdfpage.Add(LogoImage, 25, logoy, 990);

                            }

                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select logo1 from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "' and logo1 is not null", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["logo1"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, logoy, 990);
                            }
                            #endregion

                            string studname = Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]);
                            string stud_name = studname.Length.ToString();

                            #region /// ...............stud detail......................

                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, headx + 4, heady - 15, headw + 20, headh), System.Drawing.ContentAlignment.MiddleCenter, "STUDENT IDENTITY CARD TEMPORARY");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, hrc1, hrc2 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "STUDENT NAME");
                            mypdfpage.Add(ptc);

                            if (Convert.ToInt32(stud_name) < 28)
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, arc1 + 12, hrc2 - 15, headw + 30, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]));
                                mypdfpage.Add(ptc);
                            }
                            else
                            {
                                ptc = new PdfTextArea(Fontsmalll, System.Drawing.Color.Black,
                                                                                                    new PdfArea(mydocument, arc1 + 12, hrc2 - 15, headw + 30, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]));
                                mypdfpage.Add(ptc);
                            }

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 10 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "COURSE");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, arc1 + 12, hrc2 + 10 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]));
                            mypdfpage.Add(ptc);
                            if (isEnrollment)
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, hrc1, hrc2 + 20 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "ADMISSION NO");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, arc1 + 12, hrc2 + 20 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]));
                                mypdfpage.Add(ptc);
                            }
                            else
                            {
                                hrc2 -= 10;
                            }
                            //add by saranya(3.10.2017)

                            if (!isEnrollment)
                            {
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, hrc1, hrc2 + 30 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "REG NO");
                                mypdfpage.Add(ptc);
                                //string regno = d2.GetFunction("select ");
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, arc1 + 12, hrc2 + 30 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Reg_No"]));
                                mypdfpage.Add(ptc);
                            }
                            else
                            {
                                hrc2 -= 10;
                            }





                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 40 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, lblBran.Text.ToUpper());
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, arc1 + 12, hrc2 + 40 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 50 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "VALID UPTO");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, arc1 + 12, hrc2 + 50 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + txt_validdate.Text);
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, hrc1, hrc2 + 60 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "STREAM");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, arc1 + 12, hrc2 + 60 - 15, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["type"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 190, hrc2 + 50 + 12, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "PRINCIPAL");
                            mypdfpage.Add(ptc);
                            #endregion

                            #region /// ...............stud detail back......................

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 350, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "DATE OF BIRTH");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 410, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 480, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "BLOOD GROUP  :" + subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"])));
                            mypdfpage.Add(ptc);


                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydocument, 520, backy, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 350, backy + 10, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "FATHER NAME");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 410, backy + 10, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 350, backy + 40, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "ADDRESS:");
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 350, backy + 60, headw + 80, headh), System.Drawing.ContentAlignment.MiddleLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 350, backy + 70, headw + 50, headh), System.Drawing.ContentAlignment.MiddleLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 350, backy + 80, headw + 50, headh), System.Drawing.ContentAlignment.MiddleLeft, "" + Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 350, backy + 90, headw + 50, headh), System.Drawing.ContentAlignment.MiddleLeft, "Ph:" + Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]));
                            mypdfpage.Add(ptc);

                            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 500, hold, headw, headh), System.Drawing.ContentAlignment.MiddleLeft, "HOLDERS SIGNATURE");
                            mypdfpage.Add(ptc);
                            #endregion

                            #region // stud pht
                            string imgPhoto = string.Empty;

                            if (imgPhoto.Trim() == string.Empty)
                            {
                                string roll = d2.GetFunction("select app_no from applyn  where app_formno='" + app_no + "'");
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select photo from stdphoto where app_no='" + roll + "'", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["photo"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }

                            }

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg")))
                            {
                                imgPhoto = HttpContext.Current.Server.MapPath("~/Student Photo/" + app_no + ".jpg");
                                PdfImage studimg1 = mydocument.NewImage(imgPhoto);
                                mypdfpage.Add(studimg1, 25, studimg - 18, 520);
                            }



                            #endregion

                            #region // principal sign
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            {
                                MemoryStream memoryStream = new MemoryStream();
                                ds.Dispose();
                                ds.Reset();
                                ds = d2.select_method_wo_parameter("select principal_sign from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "' and principal_sign is not null", "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    byte[] file = (byte[])ds.Tables[0].Rows[0]["principal_sign"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);

                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"));
                                mypdfpage.Add(LogoImage, 190, hrc2 + 40, 990);
                            }
                            #endregion
                        }

                        //if (count == 4 || spreadCNt <= 4)
                        //{
                        //    count = 0;
                        //    mypdfpage.SaveToDocument();
                        //    spreadCNt -= 4;
                        //}

                        if (count == 4)
                        {
                            count = 0;
                            mypdfpage.SaveToDocument();
                            spreadCNt -= 4;
                        }

                        if (spreadCNt < 4)
                        {
                            if (count == spreadCNt)
                            {
                                mypdfpage.SaveToDocument();
                            }

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

            }


            else
            {
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "Please Select Any one Student!";
            }
        }
        catch
        { }

    }

    public void pdf()
    {
        try
        {

            string checkvalue = "";
            if (checkok() == true)
            {
                DAccess2 da = new DAccess2();
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();

                Font header = new Font("Arial", 6, FontStyle.Bold);
                Font header1 = new Font("Arial", 4, FontStyle.Bold);
                Font Fonthead = new Font("Arial", 2, FontStyle.Bold);
                Font Fontbold1 = new Font("Times New Roman", 6, FontStyle.Bold);
                Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
                Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
                Font Fontsmall = new Font("Arial", 5, FontStyle.Regular);
                Font FontsmallBold = new Font("Arial", 5, FontStyle.Bold);
                Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
                Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
                FpSpread.SaveChanges();
                int count = 0;
                FpSpread.SaveChanges();


                for (int i = 0; i < FpSpread.Sheets[0].RowCount; i++)
                {
                    checkvalue = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Value);

                    if (checkvalue == "1")
                    {
                        if (count < 1)
                        {
                            mypdfpage = mydocument.NewPage();
                        }
                        string strquery = "Select * from collinfo where college_code='" + ddl_collegename.SelectedItem.Value + "'";
                        DataSet ds = da.select_method_wo_parameter(strquery, "Text");
                        string university = "";
                        string collname = "";
                        string address1 = "";
                        string address2 = "";
                        string address3 = "";
                        string pincode = "";
                        string affliated = "";
                        string phone = "";
                        string fax = "";
                        string email = "";
                        string website = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                            address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                            address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                            pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                            affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                            phone = ds.Tables[0].Rows[0]["phoneno"].ToString();
                            fax = ds.Tables[0].Rows[0]["faxno"].ToString();
                            email = ds.Tables[0].Rows[0]["email"].ToString();
                            website = ds.Tables[0].Rows[0]["website"].ToString();
                        }

                        count++;
                        string app_no = Convert.ToString(Convert.ToString(FpSpread.Sheets[0].Cells[i, 2].Text));
                        Session["pdfapp_no"] = Convert.ToString(app_no);
                        PdfTextArea ptc;
                        string query = "select parentF_Mobile,app_formno,type, stud_name,c.Course_Name,sex,age,Convert(varchar(10),dob,103) as dob,dob as dob1,bldgrp,idmark,Dept_Name,batch_year,mother,parent_income,motherocc,mIncome,parent_occu,guardian_name,Guardian_income,Guardian_occ,Convert(varchar(10),Guardiandob,103) as Guardiandob,Convert(varchar(10),fatherdob,103) as fatherdob,Convert(varchar(10),motherdob,103) as motherdob,isdisable,parent_name,parent_addressP,Streetp,cityp,parent_pincodep,parent_statep,visualhandy from applyn a,Degree d,Department dt,Course C where isconfirm='1' and admission_status ='1' and selection_status ='1' and is_enroll ='2' and a.degree_code =d.Degree_Code and c.Course_Id =d.Course_Id and d.Dept_Code =dt.Dept_Code and  app_formno='" + app_no + "'";
                        ds1 = d2.select_method_wo_parameter(query, "text");
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            if (count == 1)
                            {

                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                                    new PdfArea(mydocument, 14, 50, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);

                                PdfArea pa1 = new PdfArea(mydocument, 14, 12, 241, 152);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);

                                PdfArea pa2 = new PdfArea(mydocument, 340, 12, 241, 152);
                                PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr22);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                              new PdfArea(mydocument, 14, 140, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);

                                ///............... college details...............
                                ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 68, 13, 132, 27), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(collname));

                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydocument, 68, 20, 132, 27), System.Drawing.ContentAlignment.MiddleCenter, address1 + "," + address3 + "," + pincode);
                                mypdfpage.Add(ptc);

                                /// 
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 68, 27, 132, 27), System.Drawing.ContentAlignment.MiddleCenter, "ph:" + phone + "," + "Fax:" + fax);
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 68, 34, 132, 27), System.Drawing.ContentAlignment.MiddleCenter, "Email:" + email + "," + "Website:" + website);
                                mypdfpage.Add(ptc);

                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/left_logo.jpg"));
                                    mypdfpage.Add(LogoImage, 25, 14, 560);

                                }
                                /// ...............stud detail......................
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 68, 54, 132, 27), System.Drawing.ContentAlignment.MiddleCenter, "STUDENT IDENTITY CARD TEMPORARY");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 85, 67, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "STUDENT NAME");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 130, 67, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["stud_name"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 85, 77, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "COURSE");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 130, 77, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Course_Name"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 170, 77, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "ADMISSION NO");
                                mypdfpage.Add(ptc);


                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 215, 77, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["app_formno"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 85, 87, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "DEPT");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 130, 87, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Dept_Name"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 85, 97, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "VALID UPTO");
                                mypdfpage.Add(ptc);
                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 85, 107, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "STREAM");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 130, 107, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["type"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydocument, 190, 130, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "PRICIPAL");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(mydocument, 350, 14, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "DATE OF BIRTH");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                  new PdfArea(mydocument, 420, 14, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["dob"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, 480, 14, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "BLOOD GROUP" + subjectcode(Convert.ToString(ds1.Tables[0].Rows[0]["bldgrp"])));
                                mypdfpage.Add(ptc);


                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydocument, 520, 14, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydocument, 350, 24, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "FATHER NAME");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 420, 24, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_name"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 350, 34, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "ADDRESS");
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 350, 54, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["parent_addressP"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 350, 68, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["Streetp"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 350, 72, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["cityp"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 350, 85, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, ":" + Convert.ToString(ds1.Tables[0].Rows[0]["parentF_Mobile"]));
                                mypdfpage.Add(ptc);

                                ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 500, 140, 132, 27), System.Drawing.ContentAlignment.MiddleLeft, "HOLDERS SIGNATURE");
                                mypdfpage.Add(ptc);

                                string imgPhoto = string.Empty;
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + app_no + ".jpg")))
                                {
                                    imgPhoto = HttpContext.Current.Server.MapPath("~/Upload/ApplicantPhoto/" + app_no + ".jpg");
                                }
                                if (imgPhoto.Trim() == string.Empty)
                                {
                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 9, 58, 65, 50), System.Drawing.ContentAlignment.MiddleCenter, "Affix");
                                    mypdfpage.Add(ptc);

                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 9, 68, 65, 50), System.Drawing.ContentAlignment.MiddleCenter, "Passport size");
                                    mypdfpage.Add(ptc);


                                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 9, 78, 65, 50), System.Drawing.ContentAlignment.MiddleCenter, "photograph");
                                    mypdfpage.Add(ptc);
                                }
                                else
                                {

                                    try
                                    {
                                        PdfImage studimg = mydocument.NewImage(imgPhoto);
                                        mypdfpage.Add(studimg, 25, 78, 560);
                                    }
                                    catch { }

                                }
                            }

                            else if (count == 2)
                            {
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                                   new PdfArea(mydocument, 14, 259, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);
                                PdfArea pa1 = new PdfArea(mydocument, 14, 221, 241, 152);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);

                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                             new PdfArea(mydocument, 14, 349, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);

                                PdfArea pa2 = new PdfArea(mydocument, 340, 221, 241, 152);
                                PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr22);
                            }
                            else if (count == 3)
                            {
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                                  new PdfArea(mydocument, 14, 468, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);
                                PdfArea pa1 = new PdfArea(mydocument, 14, 430, 241, 152);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                            new PdfArea(mydocument, 14, 558, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);
                                PdfArea pa2 = new PdfArea(mydocument, 340, 430, 241, 152);
                                PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr22);
                            }

                            else if (count == 4)
                            {
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                                  new PdfArea(mydocument, 14, 677, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);
                                PdfArea pa1 = new PdfArea(mydocument, 14, 639, 241, 152);
                                PdfRectangle pr3 = new PdfRectangle(mydocument, pa1, Color.Black);
                                mypdfpage.Add(pr3);
                                ptc = new PdfTextArea(Fonttimes, System.Drawing.Color.Black,

                                                            new PdfArea(mydocument, 14, 767, 300, 100), System.Drawing.ContentAlignment.TopLeft, "________________________________________________");
                                mypdfpage.Add(ptc);
                                PdfArea pa2 = new PdfArea(mydocument, 340, 639, 241, 152);
                                PdfRectangle pr22 = new PdfRectangle(mydocument, pa2, Color.Black);
                                mypdfpage.Add(pr22);
                                count = 0;
                            }

                            mypdfpage.SaveToDocument();

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



            }
            else
            {
                mainpgeerr.Visible = true;
                mainpgeerr.Text = "Please Select Any one Student!";
            }
        }
        catch (Exception ex) { }

    }


    public string subjectcode(string textcri)
    {
        string subjec_no = "";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "' and college_code ='" + Session["collegecode"].ToString() + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
            }

        }
        catch
        {

        }
        return subjec_no;
    }

    public void txt_validdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //string dt = txt_validdate.Text;
            //string[] Split = dt.Split('/');
            //DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            //string current = DateTime.Now.ToString("dd/MM/yyyy");
            //Split = current.Split('/');
            //DateTime prvedate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
            //if (fromdate < prvedate)
            //{
            //    txt_validdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //    imgdiv2.Visible = true;
            //    lbl_alert.Text = "Kindly Select Valid Date";
            //    //;
            //}
        }
        catch
        {
        }
    }
    public void txtfrmdate_TextChanged(object sender, EventArgs e)
    {
        //string dt = txtfrmdate.Text;
        //string[] Split = dt.Split('/');
        //DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        //string current = DateTime.Now.ToString("dd/MM/yyyy");
        //Split = current.Split('/');
        //DateTime prvedate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        //if (fromdate > prvedate)
        //{
        //    txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //    imgdiv2.Visible = true;
        //    lbl_alert.Text = "Kindly Select Valid Date";
        //    //;
        //}
    }
    public void txttodate_TextChanged(object sender, EventArgs e)
    {
        //string dt = txttodate.Text;
        //string[] Split = dt.Split('/');
        //DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        //string current = DateTime.Now.ToString("dd/MM/yyyy");
        //Split = current.Split('/');
        //DateTime prvedate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);
        //if (fromdate > prvedate)
        //{
        //    txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        //    imgdiv2.Visible = true;
        //    lbl_alert.Text = "Kindly Select Valid Date";
        //    //;
        //}
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
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