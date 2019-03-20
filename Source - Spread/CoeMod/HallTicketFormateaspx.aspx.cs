using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Gios.Pdf;
using System.Globalization;
using InsproDataAccess;
using System.Configuration;

public partial class HallTicketFormateaspx : System.Web.UI.Page
{
    InsproDirectAccess dir = new InsproDirectAccess();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    string group_user = "", singleuser = "", usercode = "", collegecode = string.Empty;
    Boolean flag_true = false;
    ArrayList alv = new ArrayList();
    Hashtable hashmark = new Hashtable();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerror.Visible = false;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            collegecode = Session["collegecode"].ToString();
            if (!IsPostBack)
            {
                bindbatch();
                binddegree();
                bindbranch();
                clear();
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                int year1;
                year1 = Convert.ToInt16(DateTime.Today.Year);
                ddlYear.Items.Clear();
                for (int l = 0; l <= 10; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year1 + 1 - l));
                }
                ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                clear();
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
            ddlbatch.Items.Clear();
            ds = da.BindBatch();
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            ddldegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_degree", has, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void bindbranch()
    {
        try
        {
            has.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("course_id", ddldegree.SelectedValue);
            has.Add("college_code", collegecode);
            has.Add("user_code", usercode);
            ds = da.select_method("bind_branch", has, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void loadyear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds.Reset();
            ds.Dispose();
            ds = da.Examyear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlYear.DataSource = ds;
                ddlYear.DataTextField = "Exam_year";
                ddlYear.DataBind();
            }
            else
            {
                ddlYear.Enabled = false;
                ddlMonth.Enabled = false;
            }
        }
        catch
        {
        }
    }

    public void loadmonth()
    {
        try
        {
            ddlMonth.Items.Clear();
            string year = ddlYear.Text.ToString();
            ds.Reset();
            ds.Dispose();
            ds = da.Exammonth(year);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlMonth.DataSource = ds;
                ddlMonth.DataTextField = "monthName";
                ddlMonth.DataValueField = "Exam_month";
                ddlMonth.DataBind();
            }
            else
            {
                ddlMonth.Enabled = false;
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
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void clear()
    {
        FpSpread1.Visible = false;
        btnprint.Visible = false;
        rbformate3.Visible = false;
        rbformate4.Visible = false;
        rbformate5.Visible = false;
        rbformate1.Visible = false;
        rbformate2.Visible = false;
        rbformat6.Visible = false;
        CheckArrear.Visible = false;
        chkboxvdate.Visible = false;
        CheckBox1.Visible = false;
        cbpractical.Visible = false;
        cbsignature.Visible = false;
        chkheadimage.Visible = false;
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            bindbranch();
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void Buttongo_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 5;
            FpSpread1.Sheets[0].Columns[0].Width = 40;
            FpSpread1.Sheets[0].Columns[1].Width = 80;
            FpSpread1.Sheets[0].Columns[2].Width = 130;
            FpSpread1.Sheets[0].Columns[3].Width = 180;
            FpSpread1.Sheets[0].Columns[4].Width = 90;
            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].RowHeader.Visible = false;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].SheetCorner.RowCount = 2;
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 4, 2, 1);
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Select";
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = 0;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell1;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].FrozenRowCount = 1;
            chkcell1.AutoPostBack = true;
            FpSpread1.Sheets[0].AutoPostBack = false;
            //  string examyear = ddlYear.SelectedValue.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string year = ddlbatch.SelectedValue.ToString();
            string degree = ddldegree.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string depart_code = ddlbranch.SelectedValue.ToString();
            string batchyearatt = ddlbatch.SelectedValue.ToString();
            string studinfo = "select distinct len(r.reg_no),r.reg_no,r.stud_name,r.roll_no,r.batch_year,convert(varchar,a.dob,103) dob,r.Current_Semester from registration r, exam_application ea,Exam_Details ed,applyn a where ed.exam_code=ea.exam_code and ea.roll_no=r.Roll_No and r.App_No=a.app_no and r.degree_code=" + depart_code + " and r.batch_year='" + ddlbatch.SelectedValue.ToString() + "' and ed.exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.exam_year='" + ddlYear.SelectedValue.ToString() + "' order by len(r.reg_no),r.reg_no,r.stud_name";
            DataSet dsstudinfo = da.select_method_wo_parameter(studinfo, "Text");
            if (dsstudinfo.Tables[0].Rows.Count > 0)
            {
                btnprint.Visible = true;
                rbformate1.Visible = true;
                rbformate2.Visible = true;
                rbformate1.Checked = true;
                rbformate2.Checked = false;
                rbformat6.Checked = false;
                rbformat6.Visible = true;
                rbformate3.Visible = true;
                rbformate3.Checked = false;
                rbformate4.Checked = false;
                rbformate5.Visible = false;
                rbformate4.Visible = true;
                rbformate5.Visible = true;
                rbformate5.Checked = false;
                CheckArrear.Visible = false;
                chkboxvdate.Visible = false;
                CheckBox1.Visible = false;
                cbpractical.Visible = false;
                cbsignature.Visible = false;
                chkheadimage.Visible = true;
                int sno = 0;
                for (int studcount = 0; studcount < dsstudinfo.Tables[0].Rows.Count; studcount++)
                {
                    string regno = string.Empty;
                    string studname = string.Empty;
                    string rollno = string.Empty;
                    FpSpread1.Visible = true;
                    sno++;
                    batchyear = dsstudinfo.Tables[0].Rows[studcount]["batch_year"].ToString();
                    regno = dsstudinfo.Tables[0].Rows[studcount]["reg_no"].ToString();
                    studname = dsstudinfo.Tables[0].Rows[studcount]["stud_name"].ToString();
                    rollno = dsstudinfo.Tables[0].Rows[studcount]["roll_no"].ToString();//Current_Semester
                    string sems = dsstudinfo.Tables[0].Rows[studcount]["Current_Semester"].ToString();
                    FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno + "";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = batchyear;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = rollno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Note = rollno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = sems;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = regno;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = studname;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Note = dsstudinfo.Tables[0].Rows[studcount]["dob"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = chkcell;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                }
            }
            else
            {
                clear();
                lblerror.Text = "No Records Found";
                lblerror.Visible = true;
            }
            string totalrows = FpSpread1.Sheets[0].RowCount.ToString();
            FpSpread1.Sheets[0].PageSize = (Convert.ToInt32(totalrows) * 20) + 40;
            FpSpread1.Height = (Convert.ToInt32(totalrows) * 20) + 40;
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string actrow = e.SheetView.ActiveRow.ToString();
        if (flag_true == false && actrow == "0")
        {
            for (int j = 1; j < Convert.ToInt16(FpSpread1.Sheets[0].RowCount); j++)
            {
                string actcol = e.SheetView.ActiveColumn.ToString();
                string seltext = e.EditValues[Convert.ToInt16(actcol)].ToString();
                if (seltext != "System.Object")
                    FpSpread1.Sheets[0].Cells[j, Convert.ToInt16(actcol)].Text = seltext.ToString();
            }
            flag_true = true;
        }
    }

    protected void Radiochanged(object sender, EventArgs e)
    {
        CheckArrear.Visible = false;
        chkboxvdate.Visible = false;
        CheckBox1.Visible = false;
        cbpractical.Visible = false;
        cbsignature.Visible = false;
        cbpractical.Checked = false;
        cbsignature.Checked = false;
        CheckArrear.Checked = false;
        chkboxvdate.Checked = false;
        CheckBox1.Checked = false;
        if (rbformate2.Checked == true || rbformat6.Checked == true)
        {
            CheckArrear.Visible = true;
            chkboxvdate.Visible = true;
            CheckBox1.Visible = true;
            cbpractical.Visible = true;
            cbsignature.Visible = true;
            CheckArrear.Checked = true;
            chkboxvdate.Checked = true;
            CheckBox1.Checked = true;
        }
        if (rbformate3.Checked == true)
        {
            chkboxvdate.Visible = true;
            CheckBox1.Visible = true;
            CheckBox1.Checked = true;
            cbpractical.Visible = true;
            cbpractical.Checked = true;
        }
    }

    protected void btnprint_Click(object sender, EventArgs e)
    {
        try
        {
            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
            {
                DataSet dsstuphoto = da.select_method_wo_parameter("select fileupload from tbl_notification where viewrs='Printmaster' and College_Code='" + Session["collegecode"].ToString() + "'", "Text");
                if (dsstuphoto.Tables[0].Rows.Count > 0)
                {
                    if (dsstuphoto.Tables[0].Rows[0]["fileupload"] != null && dsstuphoto.Tables[0].Rows[0]["fileupload"].ToString().Trim() != "")
                    {
                        byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["fileupload"];
                        MemoryStream memoryStream = new MemoryStream();
                        memoryStream.Write(file, 0, file.Length);
                        if (file.Length > 0)
                        {
                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                            System.Drawing.Image thumb = imgx.GetThumbnailImage(2630, 440, null, IntPtr.Zero);
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                thumb.Save(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                            }
                        }
                        memoryStream.Dispose();
                        memoryStream.Close();
                    }
                }
            }
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            if (rbformate1.Checked == true)
            {
                hallticketformat1();
            }
            else if (rbformate2.Checked == true)
            {
                hallticketformat2();
            }
            else if (rbformate3.Checked == true)
            {
                hallticketformat3();
            }
            else if (rbformate4.Checked == true)
            {
                hallticketformat4();
            }
            else if (rbformate5.Checked == true)
            {
                hallticketformat5();
            }
            else if (rbformat6.Checked == true)
            {
                hallticketformat6();
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void hallticketformat1()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontboldbig = new Font("Book Antiqua", 21, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string examyear = ddlYear.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            Boolean halfflag = false;
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select s.Photo from stdphoto s where r.app_no=s.app_no) as Photo,cc from Registration r where  r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");
                string examsupplysql = "select distinct ea.Roll_No,s.subject_name,s.subject_code,s.subject_no,et.start_time,et.end_time,convert(varchar(15),et.exam_date,103) as edate,et.exam_session,right(CONVERT(nvarchar(100),et.start_time,100),7) as start,right(CONVERT(nvarchar(100),et.end_time,100),7) as end1,exam_session,et.exam_date";
                examsupplysql = examsupplysql + " from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no ";
                examsupplysql = examsupplysql + " and ead.subject_no=et.subject_no and ead.subject_no=s.subject_no and et.subject_no=s.subject_no and et.exam_code in(select e.exam_code from  exmtt e";
                examsupplysql = examsupplysql + " where e.exam_code=et.exam_code and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "' and e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' ) and ed.Exam_month='" + exammonth + "' and ed.Exam_year='" + examyear + "'";
                examsupplysql = examsupplysql + " and ed.batch_year='" + batch + "' and ed.degree_code='" + degreecode + "' order by ea.roll_no,et.exam_date,et.exam_session desc ";
                DataSet dsexamsub = d2.select_method_wo_parameter(examsupplysql, "Text");
                string forenon = string.Empty;
                string afterenon = string.Empty;
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }
                string collname = string.Empty;
                string address = string.Empty;
                string pincode = string.Empty;
                string university = string.Empty;
                string category = string.Empty;
                if (dshall.Tables.Count > 0 && dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    string ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad2;
                        }
                        else
                        {
                            address = ad2;
                        }
                    }
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad3;
                        }
                        else
                        {
                            address = ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address != "")
                        {
                            address = address + "- " + pincode;
                        }
                        else
                        {
                            address = pincode;
                        }
                    }
                }
                DataSet supplymsubds = new DataSet();
                string strsupplymsub = string.Empty;
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    Double coltop = 0;
                    int isval = 0;
                    string s = FpSpread1.Sheets[0].Cells[res, 4].Text;
                    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        string name = FpSpread1.Sheets[0].Cells[res, 3].Text.ToString();
                        string regno = FpSpread1.Sheets[0].Cells[res, 2].Text.ToString();
                        string rollno = FpSpread1.Sheets[0].Cells[res, 1].Text.ToString();
                        string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rollno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                        supplymsubds.Clear();
                        supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                        for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                        {
                            if (strsupplymsub == "")
                            {
                                strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                            else
                            {
                                strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                        }
                        dsexamsub.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                        DataView dvhall = dsexamsub.Tables[0].DefaultView;
                        int stuexamsubcount = dvhall.Count;
                        PdfTextArea ptc;
                        if (stuexamsubcount > 0)
                        {
                            halfflag = true;
                            mypdfpage = mydocument.NewPage();
                            if (chkheadimage.Checked == true)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 15, 10, 220);
                                }
                                coltop = 140;
                            }
                            else
                            {
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                mypdfpage.Add(ptc);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 30, 10, 500);
                                }
                            }
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                            mypdfpage.Add(ptc);
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            //{
                            //    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            //    mypdfpage.Add(leftimage, 740, 10, 500);
                            //}
                            if ((afterenon.Trim() != "" && afterenon != null) || (forenon.Trim() != "" && forenon != null))
                            {
                                Double cot1 = coltop + 5;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 15, cot1, 800, 47), System.Drawing.ContentAlignment.MiddleLeft, "EXAM TIMINGS");
                                mypdfpage.Add(ptc);
                                Double sethe = cot1;
                                int he = 30;
                                if ((forenon.Trim() != "" && forenon != null))
                                {
                                    cot1 = cot1 + 8;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Forenoon  " + forenon + " ");
                                    mypdfpage.Add(ptc);
                                    he = he + 6;
                                    sethe = sethe + 5;
                                }
                                if ((afterenon.Trim() != "" && afterenon != null))
                                {
                                    cot1 = cot1 + 12;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Afternoon " + afterenon + " ");
                                    mypdfpage.Add(ptc);
                                    he = he + 6;
                                    sethe = sethe + 5;
                                }
                                PdfArea tete = new PdfArea(mydocument, 10, sethe, 190, he);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                mypdfpage.Add(pr1);
                            }
                            string batyera = string.Empty;
                            dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + regno + "'";
                            DataView dvphoto = dshall.Tables[3].DefaultView;
                            if (dvphoto.Count > 0)
                            {
                                string roll = dvphoto[0]["roll_no"].ToString();
                                string currsem = dvphoto[0]["current_semester"].ToString();
                                string ccval = dvphoto[0]["cc"].ToString();
                                if (ccval.Trim() != "1" && ccval.Trim().ToLower() != "true")
                                {
                                    if (currsem.Trim() == "1" || currsem.Trim() == "2")
                                    {
                                        batyera = "I";
                                    }
                                    else if (currsem.Trim() == "3" || currsem.Trim() == "4")
                                    {
                                        batyera = "II";
                                    }
                                    else if (currsem.Trim() == "5" || currsem.Trim() == "6")
                                    {
                                        batyera = "III";
                                    }
                                    else if (currsem.Trim() == "7" || currsem.Trim() == "8")
                                    {
                                        batyera = "IV";
                                    }
                                    else if (currsem.Trim() == "9" || currsem.Trim() == "10")
                                    {
                                        batyera = "V";
                                    }
                                }
                                else
                                {
                                    batyera = "PRIVATE";
                                }
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 730, coltop - 40, 300);
                                }
                            }
                            coltop = coltop + 60;
                            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, 2, 3, 4);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.VisibleHeaders = false;
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(150);
                            table.Columns[2].SetWidth(50);
                            table.Cell(0, 1).SetFont(Fontboldd);
                            table.Cell(0, 2).SetFont(Fontboldd);
                            table.Cell(0, 0).SetFont(Fontboldd);
                            table.Cell(0, 0).SetContent("REG.NO");
                            table.Cell(0, 1).SetContent("NAME AND CLASS OF THE CANDIDATE");
                            table.Cell(0, 2).SetContent("MONTH & YEAR");
                            table.Cell(1, 1).SetFont(Fontboldd);
                            table.Cell(1, 2).SetFont(Fontboldd);
                            table.Cell(1, 0).SetFont(Fontboldd);
                            table.Cell(1, 0).SetContent(regno);
                            table.Cell(1, 1).SetContent(name + " (" + batyera + "  " + course + " " + degree + " )");
                            table.Cell(1, 2).SetContent(ddlMonth.SelectedItem.ToString() + " - " + ddlYear.Text.ToString());
                            table.Cell(1, 1).SetFont(Fontbold);
                            table.Cell(1, 2).SetFont(Fontbold);
                            table.Cell(1, 0).SetFont(Fontbold);
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage);
                            Double getheigh = newpdftabpage.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 20;
                            Gios.Pdf.PdfTable subtable = mydocument.NewTable(Fontsmall1, stuexamsubcount + 1, 7, 6);
                            subtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            subtable.VisibleHeaders = false;
                            subtable.Columns[0].SetWidth(30);
                            subtable.Columns[1].SetWidth(50);
                            subtable.Columns[2].SetWidth(150);
                            subtable.Columns[3].SetWidth(50);
                            subtable.Columns[4].SetWidth(40);
                            subtable.Columns[5].SetWidth(50);
                            subtable.Columns[6].SetWidth(30);
                            subtable.Cell(0, 1).SetFont(Fontbold1);
                            subtable.Cell(0, 2).SetFont(Fontbold1);
                            subtable.Cell(0, 3).SetFont(Fontbold1);
                            subtable.Cell(0, 4).SetFont(Fontbold1);
                            subtable.Cell(0, 5).SetFont(Fontbold1);
                            subtable.Cell(0, 6).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetContent("S.No");
                            subtable.Cell(0, 1).SetContent("CODE");
                            subtable.Cell(0, 2).SetContent("TITLE OF THE PAPER");
                            subtable.Cell(0, 3).SetContent(" DATE ");
                            subtable.Cell(0, 4).SetContent("SESSION");
                            subtable.Cell(0, 5).SetContent("HALL / ROOM");
                            subtable.Cell(0, 6).SetContent("SEAT");
                            int srno = 0;
                            for (int subc = 0; subc < dvhall.Count; subc++)
                            {
                                srno++;
                                //Boolean subjecttype = Convert.ToBoolean(dvhall[subc]["lab"].ToString());
                                string subcode = dvhall[subc]["subject_code"].ToString();
                                string subname = dvhall[subc]["subject_name"].ToString();
                                string edate = dvhall[subc]["edate"].ToString();
                                string ses = dvhall[subc]["exam_session"].ToString();
                                string subjectno = dvhall[subc]["subject_no"].ToString();
                                string room = string.Empty;
                                string seatno = string.Empty;
                                string[] sp = edate.Split('/');
                                dshall.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and edate='" + sp[1] + '/' + sp[0] + '/' + sp[2] + "' and ses_sion='" + ses + "' and regno='" + regno + "'";
                                DataView dvsea = dshall.Tables[1].DefaultView;
                                if (dvsea.Count > 0)
                                {
                                    room = dvsea[0]["roomno"].ToString();
                                    seatno = dvsea[0]["seat_no"].ToString();
                                }
                                subtable.Cell(srno, 0).SetContent(srno.ToString());
                                subtable.Cell(srno, 1).SetContent(subcode);
                                subtable.Cell(srno, 2).SetContent(subname);
                                subtable.Cell(srno, 3).SetContent(edate);
                                subtable.Cell(srno, 4).SetContent(ses);
                                subtable.Cell(srno, 5).SetContent(room);
                                subtable.Cell(srno, 6).SetContent(seatno);
                                subtable.Cell(srno, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                subtable.Cell(srno, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            }
                            Gios.Pdf.PdfTablePage newpdftabpage1 = subtable.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage1);
                            getheigh = newpdftabpage1.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 50;
                            PdfArea tete1 = new PdfArea(mydocument, 10, coltop - 50, 825, 175);
                            PdfRectangle pr2 = new PdfRectangle(mydocument, tete1, Color.Black);
                            mypdfpage.Add(pr2);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            mypdfpage.Add(ptc);
                            MemoryStream memoryStream1 = new MemoryStream();
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            {
                                if (dshall.Tables[0].Rows[0]["principal_sign"] != null && dshall.Tables[0].Rows[0]["principal_sign"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                                    {
                                        byte[] file = (byte[])dshall.Tables[0].Rows[0]["principal_sign"];
                                        memoryStream1.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream1, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream1.Dispose();
                                        memoryStream1.Close();
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/principal_sign.jpeg"));
                                mypdfpage.Add(LogoImage, 670, coltop + 20, 300);
                            }
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 679, coltop + 80, 200, 50), System.Drawing.ContentAlignment.MiddleLeft, "Principal");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop + 100, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Instructions :");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 130, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(i)   During the examinations,students should produce Hall-Tickets and ID cards to the Invigilators.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 140, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(ii)  Students should enter the examination Hall ten minutes before the commencement of the examination.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 150, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iii) Students shall not bring cell phones and programmable calculators inside the Examination Hall.");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            lblerror.Visible = false;
                        }
                    }
                }
                if (halfflag == true)
                {
                    lblerror.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    lblerror.Text = "Please Select the Student and then Proceed";
                    lblerror.Visible = true;
                }
            }
            else
            {
                lblerror.Text = "Please Select Exam Month And Year";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void hallticketformat2()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            string district = string.Empty;
            string state = string.Empty;
            Font Fontbold = new Font("Times New Roman", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            string affliatedby = string.Empty;
            string catgory = string.Empty;
            string collnamenew1 = string.Empty;
            string address1 = string.Empty;
            string address3 = string.Empty;
            string pincode = string.Empty;
            string address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
            string university = string.Empty;
            string affiliated = string.Empty;
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(32.5, 45));
            Gios.Pdf.PdfPage mypdfpage;
            DataSet sk = new DataSet();
            string college = "select isnull(collname,'') as collname,university,district,state,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
            DataSet dscoll = d2.select_method_wo_parameter(college, "Text");
            if (dscoll.Tables[0].Rows.Count > 0)
            {
                affliatedby = dscoll.Tables[0].Rows[0]["affliated"].ToString();
                catgory = dscoll.Tables[0].Rows[0]["category"].ToString();
                collnamenew1 = dscoll.Tables[0].Rows[0]["collname"].ToString();
                address1 = dscoll.Tables[0].Rows[0]["address1"].ToString();
                address3 = dscoll.Tables[0].Rows[0]["address3"].ToString();
                pincode = dscoll.Tables[0].Rows[0]["pincode"].ToString();
                address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                university = dscoll.Tables[0].Rows[0]["university"].ToString();
                district = dscoll.Tables[0].Rows[0]["district"].ToString();
                state = dscoll.Tables[0].Rows[0]["state"].ToString();
                // affiliated = "Affiliated  to" + " " + affliatedby;
                affiliated = affliatedby;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedIndex.ToString();
            string exammonthnew = ddlMonth.SelectedItem.Text;
            string examyear = ddlYear.SelectedValue.ToString();
            string exam_code = d2.GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + "");
            string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent,r.roll_no from applyn a,registration r where a.app_no=r.app_no and r.degree_code='" + degreecode + "' and r.batch_year='" + batch + "'";
            DataSet ds3 = d2.select_method_wo_parameter(dateofbirth, "Text");
            string timequer = "select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_code=ex.exam_code and e.exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' order by start desc";
            DataSet dstime = d2.select_method_wo_parameter(timequer, "Text");
            string time = string.Empty;
            string time1 = string.Empty;
            dstime.Tables[0].DefaultView.RowFilter = " exam_session='F.N'";
            DataView dvse = dstime.Tables[0].DefaultView;
            if (dvse.Count > 0)
            {
                time = "FN - Forenoon " + dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
            }
            dstime.Tables[0].DefaultView.RowFilter = " exam_session='A.N'";
            dvse = dstime.Tables[0].DefaultView;
            if (dvse.Count > 0)
            {
                time1 = "AN - Afternoon " + dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
            }
            string arrsubinclu = " and ead.attempts='0'";
            if (CheckArrear.Checked == true)
            {
                arrsubinclu = string.Empty;
            }
            string applsubjectquery = "select ea.roll_no,ed.current_semester,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,ss.Lab,(select distinct et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') edate from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no " + arrsubinclu + " and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batch + " order by edate,ea.roll_no,ead.attempts,s.subject_code";
            DataSet dssubappl = d2.select_method_wo_parameter(applsubjectquery, "Text");
            string subexamderquery = "select ead.subject_no,et.exam_date,et.exam_session,CONVERT(nvarchar(15),et.exam_date,103) edate from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year='" + batch + "' and et.exam_code='" + exam_code + "' order by et.exam_date,et.exam_session ";
            DataSet dssubedate = d2.select_method_wo_parameter(subexamderquery, "Text");
            int degreesem = Convert.ToInt32(d2.GetFunction("select Duration from Degree where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "'"));
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread1.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        int sno = 0;
                        string stdroll = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        string rollnosub = FpSpread1.Sheets[0].Cells[res, 1].Note;
                        string exammonthnew1 = monthinwords(exammonthnew);
                        string stuname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread1.Sheets[0].Cells[res, 2].Text;
                        string sem = string.Empty;
                        if (cbpractical.Checked == true)
                        {
                            dssubappl.Tables[0].DefaultView.RowFilter = "roll_no='" + stdroll + "'";
                        }
                        else
                        {
                            dssubappl.Tables[0].DefaultView.RowFilter = "roll_no='" + stdroll + "' and Lab <> 1";
                        }
                        DataView dvsubapllquery = dssubappl.Tables[0].DefaultView;
                        int cnt = dvsubapllquery.Count;
                        if (dvsubapllquery.Count > 0)
                        {
                            sem = dvsubapllquery[0]["current_semester"].ToString();
                            if (Convert.ToInt32(sem) > degreesem)
                            {
                                sem = "Private";
                            }
                        }
                        MemoryStream memoryStream = new MemoryStream();
                        string photoquery = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + rollnosub + "')";
                        DataSet dsphoto = d2.select_method_wo_parameter(photoquery, "Text");
                        if (dsphoto.Tables[0].Rows.Count > 0)
                        {
                            byte[] file = (byte[])dsphoto.Tables[0].Rows[0]["photo"];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            memoryStream.Dispose();
                            memoryStream.Close();
                        }
                        string dob = string.Empty;
                        ds3.Tables[0].DefaultView.RowFilter = "roll_no='" + stdroll + "'";
                        DataView dvdob = ds3.Tables[0].DefaultView;
                        if (dvdob.Count > 0)
                        {
                            dob = dvdob[0]["dobstudent"].ToString();
                        }
                        mypdfpage = mydoc.NewPage();
                        if (chkheadimage.Checked == true)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 35, 25, 220);
                            }
                        }
                        //else
                        //{
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 18, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);
                        PdfTextArea ptc1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 18, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);// added by sridhar 11 sep 2014
                        PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 18, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "(" + affiliated + ")");
                        PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 18, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of the Controller of Examinations");
                        mypdfpage.Add(ptc);
                        mypdfpage.Add(ptc1);
                        mypdfpage.Add(pts);
                        mypdfpage.Add(pts1);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 30, 25, 220);
                        }
                        // }
                        PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 18, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "UG/PG Degree End Semester Examinations " + ddlMonth.SelectedItem.ToString() + "  " + ddlYear.SelectedItem.ToString() + "");
                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 18, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                        mypdfpage.Add(pts2);
                        mypdfpage.Add(pts3);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                        {
                            PdfImage leftimage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"));
                            mypdfpage.Add(leftimage, 710, 25, 300);
                        }
                        PdfArea tete = new PdfArea(mydoc, 10, 10, 876, 964);

                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontsmall, 3, 4, 3);
                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1.VisibleHeaders = false;
                        table1.Columns[0].SetWidth(150);
                        table1.Columns[1].SetWidth(180);
                        table1.Columns[2].SetWidth(100);
                        table1.Columns[3].SetWidth(100);
                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 0).SetContent("Registration Number");
                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 1).SetContent(regnumber);
                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 2).SetContent("Semester");
                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 3).SetContent(sem);
                        table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 0).SetContent("Name");
                        table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 1).SetContent(stuname);
                        table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 2).SetContent("Date of Birth");
                        table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 3).SetContent(dob);
                        table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 0).SetContent("Degree& Branch");
                        table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 1).SetContent(course + "-" + degree);
                        foreach (PdfCell pc in table1.CellRange(2, 1, 2, 1).Cells)
                            pc.ColSpan = 3;
                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 160, 790, 500));
                        mypdfpage.Add(newpdftabpage1);
                        mypdfpage.Add(pr1);
                        Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 4);
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table.VisibleHeaders = false;
                        table.Columns[0].SetWidth(50);
                        table.Columns[1].SetWidth(100);
                        table.Columns[2].SetWidth(100);
                        table.Columns[3].SetWidth(100);
                        table.Columns[4].SetWidth(350);
                        table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 0).SetContent("SI.No");
                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 1).SetContent("Date");
                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 2).SetContent("Session");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Sub.Code");
                        table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 4).SetContent("Subject Title");
                        int val = 0;
                        int srno = 0;
                        if (exam_code != "")
                        {
                            for (int sap = 0; sap < dvsubapllquery.Count; sap++)
                            {
                                val++;
                                srno++;
                                string subjectcode = dvsubapllquery[sap]["subject_code"].ToString();
                                string subname = dvsubapllquery[sap]["subject_name"].ToString();
                                string slab = dvsubapllquery[sap]["lab"].ToString();
                                string subno = dvsubapllquery[sap]["subject_no"].ToString();
                                string exdate = string.Empty;
                                string exsess = string.Empty;
                                if (slab.Trim().ToLower() == "true" || slab.Trim() == "1")
                                {
                                    if (chkboxvdate.Checked == true)
                                    {
                                        dssubedate.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'";
                                        DataView dvsubdate = dssubedate.Tables[0].DefaultView;
                                        if (dvsubdate.Count > 0)
                                        {
                                            exdate = dvsubdate[0]["edate"].ToString();
                                            exsess = dvsubdate[0]["exam_session"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (CheckBox1.Checked == true)
                                    {
                                        dssubedate.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'";
                                        DataView dvsubdate = dssubedate.Tables[0].DefaultView;
                                        if (dvsubdate.Count > 0)
                                        {
                                            exdate = dvsubdate[0]["edate"].ToString();
                                            exsess = dvsubdate[0]["exam_session"].ToString();
                                        }
                                    }
                                }
                                if (exdate == "")
                                {
                                    foreach (PdfCell pc in table.CellRange(val, 1, val, 1).Cells)
                                        pc.ColSpan = 2;
                                }
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 0).SetContent(srno.ToString());
                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 1).SetContent(exdate);
                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 2).SetContent(exsess);
                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 3).SetContent(subjectcode);
                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(val, 4).SetContent(subname);
                            }
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 240, 790, 1000));
                            mypdfpage.Add(newpdftabpage);
                            PdfTextArea pt123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 12, 800, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________");
                            PdfTextArea ptc21 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 17, 820, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "No. of Subjects Registered : " + cnt);
                            PdfTextArea ptc212 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 80, 820, 800, 50), System.Drawing.ContentAlignment.MiddleRight, time);
                            PdfTextArea ptc2123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 80, 835, 800, 50), System.Drawing.ContentAlignment.MiddleRight, time1);
                            PdfTextArea pt122 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                            new PdfArea(mydoc, 12, 845, 880, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________");
                            PdfTextArea pts31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 17, 934, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            if (cbsignature.Checked)
                            {
                                PdfTextArea pts311 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 353, 934, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Head of the Department");
                                mypdfpage.Add(pts311);
                            }
                            PdfTextArea pts41 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 80, 934, 800, 50), System.Drawing.ContentAlignment.MiddleRight, "Controller of Examinations");
                            PdfTextArea pt1222 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 12, 945, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "_____________________________________________________________________________________________________________________________");
                            PdfTextArea pts51 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 17, 963, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                            mypdfpage.Add(pt123);
                            mypdfpage.Add(ptc21);
                            mypdfpage.Add(pt122);
                            mypdfpage.Add(pts31);
                            mypdfpage.Add(pts41);
                            mypdfpage.Add(pt1222);
                            mypdfpage.Add(pts51);
                            mypdfpage.Add(ptc212);
                            mypdfpage.Add(ptc2123);

                            mypdfpage.SaveToDocument();
                        }
                        FpSpread1.Sheets[0].Cells[res, 4].Value = 0;
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Format1.pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void Oldhallticketformat3()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedIndex.ToString();
            string exammonthnew = ddlMonth.SelectedItem.Text;
            string examyear = ddlYear.SelectedValue.ToString();
            string coename = string.Empty;
            string princ = string.Empty;
            // string strqueryexamtimetable = "select s.subject_code,et.subject_no,convert(nvarchar(15),et.exam_date,103) edate, et.exam_session,RIGHT(CONVERT(VARCHAR,et.start_time,100),7) stime,RIGHT(CONVERT(VARCHAR,et.end_time,100),7) etime from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no AND e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "'";//commended by madhumathi on 20/04/2018

            string strqueryexamtimetable = "select s.subject_code,et.subject_no,convert(nvarchar(15),et.exam_date,103) edate, et.exam_session,RIGHT(CONVERT(VARCHAR,et.start_time,100),7) stime,RIGHT(CONVERT(VARCHAR,et.end_time,100),7) etime from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no AND e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "' order by s.subjectpriority";//Altered by madhumathi on 20/04/2018
            strqueryexamtimetable = strqueryexamtimetable + " select reg_no,roll_no,current_semester,(select s.Photo from stdphoto s where r.app_no=s.app_no) as Photo,cc from Registration r where  r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";

            DataSet dsextime = d2.select_method_wo_parameter(strqueryexamtimetable, "Text");
            string applsubjectquery = "select ea.roll_no,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,sy.semester,ss.Lab,(select et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') edate from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batch + " order by edate,ea.roll_no,ead.attempts,s.subject_code";
            DataSet dssubappl = d2.select_method_wo_parameter(applsubjectquery, "Text");

            string selectQ = "select eb.AppNo,eb.Batch,convert(nvarchar(15),eb.ExamDate,103) as edate,eb.ExamSession,SubNo from examtheorybatch eb,Exam_Details ed where ed.exam_code=eb.ExamCode and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "'  and ed.batch_year=" + batch + "  and ed.degree_code='" + degreecode + "'";
            DataTable dtBatch = dir.selectDataTable(selectQ);

            Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 7, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            string strquery = "select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = string.Empty;
            string aff = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                string[] strpa = aff.Split(',');
                aff = strpa[0];
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                princ = ds.Tables[0].Rows[0]["principal"].ToString();

            }
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread1.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                        string sem = string.Empty;
                        string rollno = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        string rollnosub = FpSpread1.Sheets[0].Cells[res, 1].Note;
                        string exammonthnew1 = monthinwords(exammonthnew);
                        string stuname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread1.Sheets[0].Cells[res, 2].Text;
                        string dob = FpSpread1.Sheets[0].Cells[res, 3].Note.ToString();
                        PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        mypdfpage.Add(pr1);
                        PdfArea tetep = new PdfArea(mydoc, 460, 15, 120, 145);
                        PdfRectangle pr1p = new PdfRectangle(mydoc, tetep, Color.Black);
                        mypdfpage.Add(pr1p);
                        int coltop = 25;
                        // DataTable dvphoto = new DataTable();
                        if (chkheadimage.Checked == true)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 500);
                            }
                            coltop = 60;
                        }
                        else
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 20, 20, 400);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/right_logo" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/right_logo" + Session["collegecode"].ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 380, 20, 400);
                            }
                            else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/right_logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/right_logo.jpeg"));
                                mypdfpage.Add(LogoImage, 380, 20, 400);
                            }
                            PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, Collegename);
                            mypdfpage.Add(ptc);
                            coltop = coltop + 20;
                            PdfTextArea ptc02 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, aff);
                            mypdfpage.Add(ptc02);
                        }
                        //student photo on 14-5-2018 by Rajkumar for sns 
                        dsextime.Tables[1].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        DataView dvphoto = dsextime.Tables[1].DefaultView;
                        MemoryStream memoryStream = new MemoryStream();
                        if (dvphoto.Count > 0)
                        {
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                            {
                                if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                                    {
                                        byte[] file = (byte[])dvphoto[0]["photo"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                            }
                        }
                        //-------------------------------------------
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"));
                            mypdfpage.Add(LogoImage, 475, 35, 300);
                        }
                        PdfTextArea ptcpho = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 492, 5, 450, 30), System.Drawing.ContentAlignment.MiddleLeft, "Photo of the Candidate");
                        mypdfpage.Add(ptcpho);
                        coltop = coltop + 15;
                        if (ddldegree.SelectedItem.ToString().ToLower().Contains("m.phil"))
                        {
                            PdfTextArea ptc03 = new PdfTextArea(font2small, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, ddldegree.SelectedItem.ToString().ToString() + " EXAMNINATIONS - " + ddlMonth.SelectedItem.ToString().ToUpper() + " " + examyear);
                            mypdfpage.Add(ptc03);
                        }
                        else
                        {
                            PdfTextArea ptc03 = new PdfTextArea(font2small, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, "SEMESTER EXAMNINATIONS - " + ddlMonth.SelectedItem.ToString().ToUpper() + " " + examyear);
                            mypdfpage.Add(ptc03);
                        }
                        coltop = coltop + 15;
                        PdfTextArea ptc031a = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                        mypdfpage.Add(ptc031a);
                        coltop = coltop + 25;
                        PdfArea tetet = new PdfArea(mydoc, 15, coltop, 445, 60);
                        PdfRectangle pr1t = new PdfRectangle(mydoc, tetet, Color.Black);
                        mypdfpage.Add(pr1t);
                        PdfTextArea ptc07 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Register Number");
                        mypdfpage.Add(ptc07);
                        PdfTextArea ptc08na = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + regnumber);
                        mypdfpage.Add(ptc08na);
                        PdfTextArea ptc071 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 350, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "DOB");
                        mypdfpage.Add(ptc071);
                        PdfTextArea ptc071a = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 375, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + dob);
                        mypdfpage.Add(ptc071a);
                        coltop = coltop + 20;
                        PdfTextArea ptc08 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                        mypdfpage.Add(ptc08);
                        PdfTextArea ptc08na1 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + stuname.ToString().ToUpper() + "");
                        mypdfpage.Add(ptc08na1);
                        coltop = coltop + 20;
                        PdfTextArea ptcsem = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 20, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree & Branch");
                        mypdfpage.Add(ptcsem);
                        PdfTextArea ptcsem1 = new PdfTextArea(font2small, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 130, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + "");
                        mypdfpage.Add(ptcsem1);
                        coltop = coltop + 10;
                        dssubappl.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                        DataView dvexamappl = dssubappl.Tables[0].DefaultView;
                        coltop = coltop + 10;
                        int lonehei = (dvexamappl.Count + 1) * 14;
                        //if (dvexamappl.Count > 16 && dvexamappl.Count < 29)
                        //{
                        //    lonehei = 225;
                        //    lonehei = lonehei + 20;
                        //}
                        //else if (dvexamappl.Count > 30)
                        //{
                        //    lonehei = 330;
                        //    lonehei = lonehei -10;
                        //}
                        //else
                        //{
                        //    lonehei = lonehei + 25;
                        //}

                        lonehei = 330;
                        lonehei = lonehei + 10;

                        PdfArea tlinerect = new PdfArea(mydoc, 15, coltop + 20, 565, 0.01);
                        PdfRectangle plimerecyt = new PdfRectangle(mydoc, tlinerect, Color.Black);
                        mypdfpage.Add(plimerecyt);
                        PdfArea tline = new PdfArea(mydoc, 30, coltop, 0.01, lonehei);
                        PdfRectangle plime = new PdfRectangle(mydoc, tline, Color.Black);
                        mypdfpage.Add(plime);
                        PdfArea tline2 = new PdfArea(mydoc, 100, coltop, 0.01, lonehei);
                        PdfRectangle plime2 = new PdfRectangle(mydoc, tline2, Color.Black);
                        mypdfpage.Add(plime2);
                        PdfArea tline3 = new PdfArea(mydoc, 163, coltop, 0.01, lonehei);
                        PdfRectangle plime3 = new PdfRectangle(mydoc, tline3, Color.Black);
                        mypdfpage.Add(plime3);
                        PdfArea tline4 = new PdfArea(mydoc, 418, coltop, 0.01, lonehei);
                        PdfRectangle plime4 = new PdfRectangle(mydoc, tline4, Color.Black);
                        mypdfpage.Add(plime4);

                        //if (dvexamappl.Count > 15 && dvexamappl.Count <29)
                        //{
                        //    PdfArea tline5 = new PdfArea(mydoc, 335, coltop, 0.01, lonehei);
                        //    PdfRectangle plime5 = new PdfRectangle(mydoc, tline5, Color.Black);
                        //    mypdfpage.Add(plime5);
                        //    PdfArea tline6 = new PdfArea(mydoc, 375, coltop, 0.01, lonehei);
                        //    PdfRectangle plime6 = new PdfRectangle(mydoc, tline6, Color.Black);
                        //    mypdfpage.Add(plime6);
                        //    PdfArea tline7 = new PdfArea(mydoc, 435, coltop, 0.01, lonehei);
                        //    PdfRectangle plime7 = new PdfRectangle(mydoc, tline7, Color.Black);
                        //    mypdfpage.Add(plime7);
                        //}
                        //else if (dvexamappl.Count > 30)
                        //{
                        //    PdfArea tline5 = new PdfArea(mydoc, 335, coltop, 0.01, lonehei);
                        //    PdfRectangle plime5 = new PdfRectangle(mydoc, tline5, Color.Black);
                        //    mypdfpage.Add(plime5);
                        //    PdfArea tline6 = new PdfArea(mydoc, 375, coltop, 0.01, lonehei);
                        //    PdfRectangle plime6 = new PdfRectangle(mydoc, tline6, Color.Black);
                        //    mypdfpage.Add(plime6);
                        //    PdfArea tline7 = new PdfArea(mydoc, 435, coltop, 0.01, lonehei);
                        //    PdfRectangle plime7 = new PdfRectangle(mydoc, tline7, Color.Black);
                        //    mypdfpage.Add(plime7);
                        //}
                        //else
                        //{
                        //    PdfArea tline5 = new PdfArea(mydoc, 345, coltop, 0.01, lonehei);
                        //    PdfRectangle plime5 = new PdfRectangle(mydoc, tline5, Color.Black);
                        //    mypdfpage.Add(plime5);
                        //    PdfArea tline6 = new PdfArea(mydoc, 385, coltop, 0.01, lonehei);
                        //    PdfRectangle plime6 = new PdfRectangle(mydoc, tline6, Color.Black);
                        //    mypdfpage.Add(plime6);
                        //    PdfArea tline7 = new PdfArea(mydoc, 460, coltop, 0.01, lonehei);
                        //    PdfRectangle plime7 = new PdfRectangle(mydoc, tline7, Color.Black);
                        //    mypdfpage.Add(plime7);
                        //}


                        //PdfArea tline5 = new PdfArea(mydoc, 335, coltop, 0.01, lonehei);
                        //PdfRectangle plime5 = new PdfRectangle(mydoc, tline5, Color.Black);
                        //mypdfpage.Add(plime5);
                        //PdfArea tline6 = new PdfArea(mydoc, 375, coltop, 0.01, lonehei);
                        //PdfRectangle plime6 = new PdfRectangle(mydoc, tline6, Color.Black);
                        //mypdfpage.Add(plime6);
                        //PdfArea tline7 = new PdfArea(mydoc, 435, coltop, 0.01, lonehei);
                        //PdfRectangle plime7 = new PdfRectangle(mydoc, tline7, Color.Black);
                        //mypdfpage.Add(plime7);
                        PdfTextArea ptsrnor = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, 15, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sem");
                        mypdfpage.Add(ptsrnor);
                        PdfTextArea ptcsubcoder = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 55, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sub.Code");
                        mypdfpage.Add(ptcsubcoder);
                        PdfTextArea ptcsubnamer = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 110, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date - Session");
                        mypdfpage.Add(ptcsubnamer);
                        PdfTextArea ptcarrearr = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                                                                     new PdfArea(mydoc, 195, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Title");
                        mypdfpage.Add(ptcarrearr);




                        PdfTextArea ptsrnor1 = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 450, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Hall Superintendent  Signature");
                        mypdfpage.Add(ptsrnor1);

                        //command by rajkumar 14-12-2018
                        //PdfTextArea ptcsubcoder1 = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                        //                                       new PdfArea(mydoc, 338, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sub.Code");
                        //mypdfpage.Add(ptcsubcoder1);
                        //PdfTextArea ptcsubnamer1 = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                        //                                       new PdfArea(mydoc, 385, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Date - Session");
                        //mypdfpage.Add(ptcsubnamer1);
                        //PdfTextArea ptcarrearr1 = new PdfTextArea(font4bold, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydoc, 500, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Subject Title");
                        //mypdfpage.Add(ptcarrearr1);
                        //-------------------------------

                        int setlefty = coltop;
                        int settopty = coltop;
                        int sl1 = 22;
                        int sl2 = 35;
                        int sl3 = 105;
                        int sl4 = 165;
                        for (int sn = 0; sn < dvexamappl.Count; sn++)
                        {
                            string subcode = dvexamappl[sn]["subject_code"].ToString();
                            string subname = dvexamappl[sn]["subject_name"].ToString();
                            string subno = dvexamappl[sn]["subject_no"].ToString();
                            string chckname = subname.Trim().ToLower();
                            string subsem = dvexamappl[sn]["semester"].ToString();
                            //if (dvexamappl.Count < 29)
                            //{
                            //    if (sn == 15)
                            //    {
                            //        settopty = coltop;
                            //        sl1 = 325;
                            //        sl2 = 340;
                            //        sl3 = 375;
                            //        sl4 = 437;
                            //    }
                            //}
                            //else
                            //{
                            //    if (sn ==20)
                            //    {
                            //        settopty = coltop;
                            //        sl1 = 325;
                            //        sl2 = 340;
                            //        sl3 = 375;
                            //        sl4 = 437;
                            //    }
                            //}
                            if (sn == 20)
                            {
                                settopty = coltop;
                                sl1 = 325;
                                sl2 = 340;
                                sl3 = 375;
                                sl4 = 437;
                            }
                            settopty = settopty + 15;
                            PdfTextArea ptsrno = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                    new PdfArea(mydoc, sl1, settopty, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subsem.ToString());
                            mypdfpage.Add(ptsrno);
                            PdfTextArea ptcsubcode = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, sl2, settopty, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subcode.ToString());
                            mypdfpage.Add(ptcsubcode);
                            dsextime.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "'";
                            DataView dvtimetable = dsextime.Tables[0].DefaultView;
                            string appNo = da.GetFunction("select App_No from Registration where Roll_No='" + rollno + "'");
                            dtBatch.DefaultView.RowFilter = "AppNo='" + appNo + "' and SubNo='" + subno + "'";
                            DataView dtDateSes = dtBatch.DefaultView;
                            string date = string.Empty;
                            string sess = string.Empty;
                            if (dvtimetable.Count > 0)
                            {
                                date = dvtimetable[0]["edate"].ToString();
                                sess = dvtimetable[0]["exam_session"].ToString();
                            }
                            if (dtDateSes.Count > 0)
                            {
                                date = dtDateSes[0]["edate"].ToString();
                                sess = dtDateSes[0]["ExamSession"].ToString();
                            }
                            PdfTextArea ptcedaye = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, sl3, settopty, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, date + " - " + sess);
                            mypdfpage.Add(ptcedaye);


                            PdfTextArea ptcsubname = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, sl4, settopty, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, subname.ToString());
                            mypdfpage.Add(ptcsubname);
                        }
                        PdfTextArea ptcsubname1 = new PdfTextArea(font4small, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, sl4 + 2, settopty + 13, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "***End of Statement***");
                        mypdfpage.Add(ptcsubname1);

                        //if (dvexamappl.Count > 15)
                        //{
                        //    coltop = coltop + 225;
                        //}
                        //else
                        //{
                        //    coltop = settopty;
                        //}
                        coltop = coltop + 225;
                        coltop = coltop + 15;
                        PdfArea tetefina;
                        //if (dvexamappl.Count > 15 && dvexamappl.Count <29)
                        //    tetefina = new PdfArea(mydoc, 15, coltop + 26, 565, 30);
                        //else if(dvexamappl.Count > 30)
                        //    tetefina = new PdfArea(mydoc, 15, coltop + 80, 565, 30);
                        //else
                        //    tetefina = new PdfArea(mydoc, 15, coltop + 15, 565, 30);

                        tetefina = new PdfArea(mydoc, 15, coltop + 100, 565, 30);

                        PdfRectangle pr1final = new PdfRectangle(mydoc, tetefina, Color.Black);
                        mypdfpage.Add(pr1final);
                        PdfTextArea ptcsubreg;
                        PdfTextArea ptcfn;
                        PdfTextArea ptcan;
                        //if (dvexamappl.Count > 15 && dvexamappl.Count < 29)
                        //{
                        //    ptcsubreg = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                      new PdfArea(mydoc, 30, coltop + 17, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "No.of Subject Registered  - " + dvexamappl.Count.ToString());
                        //    mypdfpage.Add(ptcsubreg);
                        //    ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                     new PdfArea(mydoc, 400, coltop + 17, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "FN - FORENOON 10.00 AM - 1.00 PM");
                        //    mypdfpage.Add(ptcfn);
                        //    coltop = coltop + 15;
                        //    ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                      new PdfArea(mydoc, 400, coltop + 20, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                        //    mypdfpage.Add(ptcan);
                        //}
                        //else if (dvexamappl.Count > 30)
                        //{
                        //    ptcsubreg = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                    new PdfArea(mydoc, 30, coltop + 72, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "No.of Subject Registered  - " + dvexamappl.Count.ToString());
                        //    mypdfpage.Add(ptcsubreg);
                        //    ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                     new PdfArea(mydoc, 400, coltop + 72, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "FN - FORENOON 10.00 AM - 1.00 PM");
                        //    mypdfpage.Add(ptcfn);
                        //    coltop = coltop + 15;
                        //    ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                      new PdfArea(mydoc, 400, coltop + 75, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                        //    mypdfpage.Add(ptcan);
                        //}
                        //else
                        //{
                        //     ptcsubreg = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                        new PdfArea(mydoc, 30, coltop + 10, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "No.of Subject Registered  - " + dvexamappl.Count.ToString());
                        //    mypdfpage.Add(ptcsubreg);
                        //     ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                      new PdfArea(mydoc, 400, coltop + 10, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "FN - FORENOON 10.00 AM - 1.00 PM");
                        //    mypdfpage.Add(ptcfn);
                        //    coltop = coltop + 15;
                        //    ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                      new PdfArea(mydoc, 400, coltop + 10, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                        //    mypdfpage.Add(ptcan);
                        //}

                        ptcsubreg = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 30, coltop + 92, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "No.of Subject Registered  - " + dvexamappl.Count.ToString());
                        mypdfpage.Add(ptcsubreg);
                        ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 400, coltop + 92, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "FN - FORENOON 10.00 AM - 1.00 PM");
                        mypdfpage.Add(ptcfn);
                        coltop = coltop + 15;
                        ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 400, coltop + 95, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "AN - AFTERNOON 2.00 PM - 5.00 PM");
                        mypdfpage.Add(ptcan);

                        coltop = 735;

                        PdfTextArea ptcstisign = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 30, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                        mypdfpage.Add(ptcstisign);


                        //PdfTextArea ptprincname = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 180, coltop - 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, princ);
                        //mypdfpage.Add(ptprincname);


                        PdfTextArea ptcprinc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 180, coltop - 5, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Signature of the Principal with Seal");
                        mypdfpage.Add(ptcprinc);


                        //PdfTextArea ptccoename = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 380, coltop - 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, coename);
                        //mypdfpage.Add(ptccoename);


                        PdfTextArea ptccontroller = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 380, coltop - 5, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                        mypdfpage.Add(ptccontroller);


                        coltop = coltop + 15;
                        PdfArea tetefinanote = new PdfArea(mydoc, 15, coltop + 10, 565, 65);
                        PdfRectangle pr1note = new PdfRectangle(mydoc, tetefinanote, Color.Black);
                        mypdfpage.Add(pr1note);
                        PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        mypdfpage.Add(ptcsnote);
                        coltop = coltop + 15;

                        PdfTextArea ptcsnote1 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. If any discrepancy is found in the Hall Ticket,report to the C.O.E office immediately");
                        mypdfpage.Add(ptcsnote1);

                        coltop = coltop + 15;

                        PdfTextArea ptcsnote2 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Verify the dates / sessions mentioned in the time table posted in the college notice board / Website");
                        mypdfpage.Add(ptcsnote2);
                        coltop = coltop + 15;
                        PdfTextArea ptcsnote21 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 20, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Printed On: " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                        mypdfpage.Add(ptcsnote21);


                        mypdfpage.SaveToDocument();
                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                Response.Buffer = true;
                Response.Clear();
                string szPath = appPath + "/Report/";
                string szFile = "HALLTICKET" + DateTime.Now.ToString("ddMMyyyyHHmmsstt") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void hallticketformat6()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            string district = string.Empty;
            string state = string.Empty;
            Font Fontbold = new Font("Times New Roman", 18, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 15, FontStyle.Regular);
            Font Fontsmall1 = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            string affliatedby = string.Empty;
            string catgory = string.Empty;
            string collnamenew1 = string.Empty;
            string address1 = string.Empty;
            string address3 = string.Empty;
            string pincode = string.Empty;
            string address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
            string university = string.Empty;
            string affiliated = string.Empty;
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage;
            DataSet sk = new DataSet();
            string college = "select isnull(collname,'') as collname,university,district,state,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
            DataSet dscoll = d2.select_method_wo_parameter(college, "Text");
            if (dscoll.Tables[0].Rows.Count > 0)
            {
                affliatedby = dscoll.Tables[0].Rows[0]["affliated"].ToString();
                catgory = dscoll.Tables[0].Rows[0]["category"].ToString();
                collnamenew1 = dscoll.Tables[0].Rows[0]["collname"].ToString();
                address1 = dscoll.Tables[0].Rows[0]["address1"].ToString();
                address3 = dscoll.Tables[0].Rows[0]["address3"].ToString();
                pincode = dscoll.Tables[0].Rows[0]["pincode"].ToString();
                address = address1 + ", " + " " + address3 + "-" + " " + pincode + ".";
                university = dscoll.Tables[0].Rows[0]["university"].ToString();
                district = dscoll.Tables[0].Rows[0]["district"].ToString();
                state = dscoll.Tables[0].Rows[0]["state"].ToString();
                //  affiliated = "Affiliated  to" + " " + affliatedby;
                //  affiliated = catgory + " " + affliatedby;
                affiliated = affliatedby;

            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string add1 = "";
            string add2 = "";
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedIndex.ToString();
            string exammonthnew = ddlMonth.SelectedItem.Text;
            string examyear = ddlYear.SelectedValue.ToString();
            string exam_code = d2.GetFunction("select distinct exam_code from exmtt where degree_code=" + degreecode + " and exam_month=" + exammonth + " and exam_year=" + examyear + " and batchfrom=" + batch + "");
            string dateofbirth = "select convert(varchar(20),a.dob,103) as dobstudent,r.roll_no from applyn a,registration r where a.app_no=r.app_no and r.degree_code='" + degreecode + "' and r.batch_year='" + batch + "'";
            DataSet ds3 = d2.select_method_wo_parameter(dateofbirth, "Text");
            string timequer = "select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_code=ex.exam_code order by start desc";
            DataSet dstime = d2.select_method_wo_parameter(timequer, "Text");
            string time = string.Empty;
            string time1 = string.Empty;
            dstime.Tables[0].DefaultView.RowFilter = " exam_session='F.N'";
            DataView dvse = dstime.Tables[0].DefaultView;
            if (dvse.Count > 0)
            {
                time = "FN - Forenoon " + dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
            }
            dstime.Tables[0].DefaultView.RowFilter = " exam_session='A.N'";
            dvse = dstime.Tables[0].DefaultView;
            if (dvse.Count > 0)
            {
                time1 = "AN - Afternoon " + dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
            }
            string arrsubinclu = " and ead.attempts='0'";
            if (CheckArrear.Checked == true)
            {
                arrsubinclu = string.Empty;
            }
            //string applsubjectquery = "select ea.roll_no,ed.current_semester,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,ss.Lab,(select et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') edate from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no " + arrsubinclu + " and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batch + " order by edate,ea.roll_no,ead.attempts,s.subject_code";
            string applsubjectquery = "select ea.roll_no,ed.current_semester,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,ss.Lab,(select et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') edate,(select et.exam_session from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') esession  from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no " + arrsubinclu + " and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batch + " order by edate,ea.roll_no,ead.attempts,esession desc";
            DataSet dssubappl = d2.select_method_wo_parameter(applsubjectquery, "Text");
            string subexamderquery = "select ead.subject_no,et.exam_date,et.exam_session,CONVERT(nvarchar(15),et.exam_date,103) edate from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=et.subject_no and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year='" + batch + "' and et.exam_code='" + exam_code + "' order by et.exam_date,et.exam_session";
            DataSet dssubedate = d2.select_method_wo_parameter(subexamderquery, "Text");
            int degreesem = Convert.ToInt32(d2.GetFunction("select Duration from Degree where Degree_Code='" + ddlbranch.SelectedValue.ToString() + "'"));
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread1.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        int sno = 0;
                        string stdroll = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        string rollnosub = FpSpread1.Sheets[0].Cells[res, 1].Note;
                        string exammonthnew1 = monthinwords(exammonthnew);
                        string stuname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread1.Sheets[0].Cells[res, 2].Text;
                        string sem = string.Empty;
                        dssubappl.Tables[0].DefaultView.RowFilter = "roll_no='" + stdroll + "'";
                        DataView dvsubapllquery = dssubappl.Tables[0].DefaultView;
                        int cnt = dvsubapllquery.Count;
                        if (dvsubapllquery.Count > 0)
                        {
                            sem = dvsubapllquery[0]["current_semester"].ToString();
                            if (Convert.ToInt32(sem) > degreesem)
                            {
                                sem = "Private";
                            }
                        }
                        MemoryStream memoryStream = new MemoryStream();
                        string photoquery = "select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + rollnosub + "')";
                        DataSet dsphoto = d2.select_method_wo_parameter(photoquery, "Text");
                        if (dsphoto.Tables[0].Rows.Count > 0)
                        {
                            byte[] file = (byte[])dsphoto.Tables[0].Rows[0]["photo"];
                            memoryStream.Write(file, 0, file.Length);
                            if (file.Length > 0)
                            {
                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                                {
                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                }
                            }
                            memoryStream.Dispose();
                            memoryStream.Close();
                        }
                        string dob = string.Empty;
                        ds3.Tables[0].DefaultView.RowFilter = "roll_no='" + stdroll + "'";
                        DataView dvdob = ds3.Tables[0].DefaultView;
                        if (dvdob.Count > 0)
                        {
                            dob = dvdob[0]["dobstudent"].ToString();
                        }
                        mypdfpage = mydoc.NewPage();
                        if (chkheadimage.Checked == true)
                        {
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                mypdfpage.Add(LogoImage, 25, 10, 290);
                            }
                        }
                        else
                        {

                            string[] adsplit = affliatedby.Split('.');
                            PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 0, 20, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);
                            //PdfTextArea ptc1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                   new PdfArea(mydoc, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);// added by sridhar 11 sep 2014
                            //  PdfTextArea pts = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //new PdfArea(mydoc, 0, 40, 800, 50), ////System.Drawing.ContentAlignment.MiddleCenter, "(" + affiliated + ")");
                            if (adsplit.Length == 2)
                            {
                                add1 = adsplit[0];
                                add2 = adsplit[1];
                            }
                            PdfTextArea pts = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 0, 40, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "(" + add1 + "");
                            PdfTextArea pts3ad = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 0, 60, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + add2 + ")");
                            //PdfTextArea pts1 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                  new PdfArea(mydoc, 0, 80, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "Office of the Controller of Examinations");
                            mypdfpage.Add(ptc);
                            //  mypdfpage.Add(ptc1);
                            mypdfpage.Add(pts);
                            mypdfpage.Add(pts3ad);
                            // mypdfpage.Add(pts1);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));

                                mypdfpage.Add(LogoImage, 30, 25, 300);
                            }
                        }
                        //PdfTextArea pts2 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                        //                                  new PdfArea(mydoc, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "UG/PG Degree End Semester Examinations " + ddlMonth.SelectedItem.ToString() + "  " + ddlYear.SelectedItem.ToString() + "");
                        PdfTextArea pts3 = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 100, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");

                        string sems1 = string.Empty;
                        string yr = string.Empty;
                        if (sem == "1")
                        {
                            sems1 = "I";
                            yr = "I";
                        }
                        else if (sem == "2")
                        {
                            sems1 = "II";
                            yr = "I";
                        }
                        else if (sem == "3")
                        {
                            sems1 = "III";
                            yr = "II";
                        }
                        else if (sem == "4")
                        {
                            sems1 = "IV";
                            yr = "II";
                        }
                        else if (sem == "5")
                        {
                            sems1 = "V";
                            yr = "III";
                        }
                        else if (sem == "6")
                        {
                            sems1 = "VI";
                            yr = "III";
                        }
                        else if (sem == "7")
                        {
                            sems1 = "VII";
                            yr = "IV";
                        }
                        else if (sem == "8")
                        {
                            sems1 = "VIII";
                            yr = "IV";
                        }
                        else if (sem == "9")
                        {
                            sems1 = "IX";
                            yr = "V";
                        }
                        PdfTextArea pts4 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 0, 120, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, course + " " + yr + " Year - SEM" + " " + sems1 + "-MID Examinations" + " " + ddlMonth.SelectedItem.ToString() + " - " + ddlYear.SelectedItem.ToString());
                        // mypdfpage.Add(pts2);
                        mypdfpage.Add(pts3);
                        mypdfpage.Add(pts4);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg")))
                        {
                            PdfImage leftimage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollnosub + ".jpeg"));
                            mypdfpage.Add(leftimage, 685, 25, 300);
                        }
                        PdfArea tete = new PdfArea(mydoc, 25, 20, 800, 510);//magesh

                        PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                        Gios.Pdf.PdfTable table1 = mydoc.NewTable(Fontsmall, 3, 4, 1);

                        table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table1.VisibleHeaders = false;
                        table1.Columns[0].SetWidth(150);
                        table1.Columns[1].SetWidth(180);
                        table1.Columns[2].SetWidth(100);
                        table1.Columns[3].SetWidth(100);
                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 0).SetContent("Registration Number");
                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 1).SetContent(regnumber);
                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 2).SetContent("Semester");
                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(0, 3).SetContent(sem);
                        table1.Cell(1, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 0).SetContent("Name");
                        table1.Cell(1, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 1).SetContent(stuname);
                        table1.Cell(1, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 2).SetContent("Date of Birth");
                        table1.Cell(1, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(1, 3).SetContent(dob);
                        table1.Cell(2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 0).SetContent("Degree& Branch");
                        table1.Cell(2, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                        table1.Cell(2, 1).SetContent(course + "-" + degree);
                        foreach (PdfCell pc in table1.CellRange(2, 1, 2, 1).Cells)
                            pc.ColSpan = 3;
                        Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 160, 700, 500));
                        mypdfpage.Add(newpdftabpage1);
                        mypdfpage.Add(pr1);
                        Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 1);
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        table.VisibleHeaders = false;
                        table.Columns[0].SetWidth(50);
                        table.Columns[1].SetWidth(100);
                        table.Columns[2].SetWidth(100);
                        table.Columns[3].SetWidth(100);
                        table.Columns[4].SetWidth(350);
                        table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 0).SetContent("SI.No");
                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 1).SetContent("Date");
                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 2).SetContent("Session");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Sub.Code");
                        table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 4).SetContent("Subject Title");
                        int val = 0;
                        int srno = 0;
                        if (exam_code != "")
                        {
                            for (int sap = 0; sap < dvsubapllquery.Count; sap++)
                            {
                                val++;
                                srno++;
                                string subjectcode = dvsubapllquery[sap]["subject_code"].ToString();
                                string subname = dvsubapllquery[sap]["subject_name"].ToString();
                                string slab = dvsubapllquery[sap]["lab"].ToString();
                                string subno = dvsubapllquery[sap]["subject_no"].ToString();
                                string exdate = string.Empty;
                                string exsess = string.Empty;
                                if (slab.Trim().ToLower() == "true" || slab.Trim() == "1")
                                {
                                    if (chkboxvdate.Checked == true)
                                    {
                                        dssubedate.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'";
                                        DataView dvsubdate = dssubedate.Tables[0].DefaultView;
                                        if (dvsubdate.Count > 0)
                                        {
                                            exdate = dvsubdate[0]["edate"].ToString();
                                            exsess = dvsubdate[0]["exam_session"].ToString();
                                        }
                                    }
                                }
                                else
                                {
                                    if (CheckBox1.Checked == true)
                                    {
                                        dssubedate.Tables[0].DefaultView.RowFilter = "subject_no='" + subno + "'";
                                        DataView dvsubdate = dssubedate.Tables[0].DefaultView;
                                        if (dvsubdate.Count > 0)
                                        {
                                            exdate = dvsubdate[0]["edate"].ToString();
                                            exsess = dvsubdate[0]["exam_session"].ToString();
                                        }
                                    }
                                }
                                if (exdate == "")
                                {
                                    foreach (PdfCell pc in table.CellRange(val, 1, val, 1).Cells)
                                        pc.ColSpan = 2;
                                }
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 0).SetContent(srno.ToString());
                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 1).SetContent(exdate);
                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 2).SetContent(exsess);
                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 3).SetContent(subjectcode);
                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(val, 4).SetContent(subname);
                            }
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 50, 230, 700, 1000));
                            mypdfpage.Add(newpdftabpage);
                            //PdfTextArea pt123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                 new PdfArea(mydoc, 25, 445, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");
                            //PdfTextArea ptc21 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                  new PdfArea(mydoc, 30, 470, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "No. of Subjects Registered : " + cnt);

                            //PdfTextArea pt122 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                new PdfArea(mydoc, 25, 500, 880, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");
                            PdfTextArea pts31 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 30, 445, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            PdfTextArea pts41 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 0, 445, 800, 50), System.Drawing.ContentAlignment.MiddleRight, "Signature of the HOD");

                            PdfTextArea ptc212 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, 465, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, time);
                            PdfTextArea ptc2123 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                              new PdfArea(mydoc, 30, 480, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, time1);
                            //PdfTextArea pt1222 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                             new PdfArea(mydoc, 25, 490, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________________________________________________________________________________________");
                            //PdfTextArea pts51 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                            //                                  new PdfArea(mydoc, 30, 490, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Note: If any discrepancies are found in the Hall Ticket, report to the COE office immediately.");
                            //mypdfpage.Add(pt123);
                            // mypdfpage.Add(ptc21);
                            // mypdfpage.Add(pt122);
                            mypdfpage.Add(pts31);
                            mypdfpage.Add(pts41);
                            // mypdfpage.Add(pt1222);
                            // mypdfpage.Add(pts51);
                            mypdfpage.Add(ptc212);
                            mypdfpage.Add(ptc2123);
                            mypdfpage.SaveToDocument();
                        }
                        FpSpread1.Sheets[0].Cells[res, 4].Value = 0;
                    }
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Format1.pdf";
                    mydoc.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    protected string monthinwords(string month)
    {
        string month1 = string.Empty;
        if (month == "Jan")
        {
            month1 = "January";
        }
        else if (month == "Feb")
        {
            month1 = "February";
        }
        else if (month == "Mar")
        {
            month1 = "March";
        }
        else if (month == "Apr")
        {
            month1 = "April";
        }
        else if (month == "May")
        {
            month1 = "May";
        }
        else if (month == "Jun")
        {
            month1 = "June";
        }
        else if (month == "Jul")
        {
            month1 = "July";
        }
        else if (month == "Aug")
        {
            month1 = "August";
        }
        else if (month == "Sep")
        {
            month1 = "September";
        }
        else if (month == "Oct")
        {
            month1 = "October";
        }
        else if (month == "Nov")
        {
            month1 = "November";
        }
        else if (month == "Dec")
        {
            month1 = "December";
        }
        return month1;
    }

    public void hallticketformat4()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            Font Fontbold = new Font("Book Antiqua", 17, FontStyle.Bold);
            Font Fontboldd = new Font("Book Antiqua", 17, FontStyle.Regular);
            Font Fontboldbig = new Font("Book Antiqua", 20, FontStyle.Bold);
            Font Fontbold1 = new Font("Book Antiqua", 12, FontStyle.Bold);
            Font Fontbold2 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Font Fontsmall = new Font("Book Antiqua", 13, FontStyle.Regular);
            Font Fontsmall1 = new Font("Book Antiqua", 15, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.InCentimeters(30, 40));
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            string degreecode = ddlbranch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string examyear = ddlYear.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedValue.ToString();
            Boolean halfflag = false;
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                string strquery = "select * from collinfo where  college_code='" + Session["collegecode"].ToString() + "' ;";
                strquery = strquery + " Select  * from exam_seating where degree_code='" + degreecode + "'";
                strquery = strquery + " select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_code=ex.exam_code order by start desc";
                strquery = strquery + " select reg_no,roll_no,current_semester,(select s.Photo from stdphoto s where r.app_no=s.app_no) as Photo,cc from Registration r where  r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";
                DataSet dshall = d2.select_method_wo_parameter(strquery, "Text");
                string examsupplysql = "select distinct ea.Roll_No,s.subjectpriority,s.subject_name,s.subject_code,s.subject_no,et.start_time,et.end_time,convert(varchar(15),et.exam_date,103) as edate,et.exam_session,right(CONVERT(nvarchar(100),et.start_time,100),7) as start,right(CONVERT(nvarchar(100),et.end_time,100),7) as end1,exam_session,et.exam_date";
                examsupplysql = examsupplysql + " from Exam_Details ed,exam_application ea,exam_appl_details ead,exmtt_det et,subject s where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no ";
                examsupplysql = examsupplysql + " and ead.subject_no=et.subject_no and ead.subject_no=s.subject_no and et.subject_no=s.subject_no and et.exam_code in(select e.exam_code from  exmtt e";
                examsupplysql = examsupplysql + " where e.exam_code=et.exam_code and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "' and e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' ) and ed.Exam_month='" + exammonth + "' and ed.Exam_year='" + examyear + "'";
                examsupplysql = examsupplysql + " and ed.batch_year='" + batch + "' and ed.degree_code='" + degreecode + "' order by ea.roll_no,s.subjectpriority,et.exam_date,et.exam_session desc ";
                DataSet dsexamsub = d2.select_method_wo_parameter(examsupplysql, "Text");
                string forenon = string.Empty;
                string afterenon = string.Empty;
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='F.N'";
                DataView dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    forenon = dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                }
                dshall.Tables[2].DefaultView.RowFilter = " exam_session='A.N'";
                dvse = dshall.Tables[2].DefaultView;
                if (dvse.Count > 0)
                {
                    afterenon = dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                }
                string collname = string.Empty;
                string address = string.Empty;
                string pincode = string.Empty;
                string university = string.Empty;
                string category = string.Empty;
                if (dshall.Tables.Count > 0 && dshall.Tables[0].Rows.Count > 0)
                {
                    collname = dshall.Tables[0].Rows[0]["collname"].ToString();
                    string ad1 = dshall.Tables[0].Rows[0]["address1"].ToString();
                    string ad2 = dshall.Tables[0].Rows[0]["address2"].ToString();
                    string ad3 = dshall.Tables[0].Rows[0]["address3"].ToString();
                    university = dshall.Tables[0].Rows[0]["university"].ToString();
                    category = dshall.Tables[0].Rows[0]["category"].ToString();
                    pincode = dshall.Tables[0].Rows[0]["pincode"].ToString();
                    if (ad1 != "" && ad1 != null)
                    {
                        address = ad1;
                    }
                    if (ad2 != "" && ad2 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad2;
                        }
                        else
                        {
                            address = ad2;
                        }
                    }
                    if (ad3 != "" && ad3 != null)
                    {
                        if (address != "")
                        {
                            address = address + " ," + ad3;
                        }
                        else
                        {
                            address = ad3;
                        }
                    }
                    if (pincode != "" && pincode != null)
                    {
                        if (address != "")
                        {
                            address = address + "- " + pincode;
                        }
                        else
                        {
                            address = pincode;
                        }
                    }
                }
                DataSet supplymsubds = new DataSet();
                string strsupplymsub = string.Empty;
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    Double coltop = 0;
                    int isval = 0;
                    string s = FpSpread1.Sheets[0].Cells[res, 4].Text;
                    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        string name = FpSpread1.Sheets[0].Cells[res, 3].Text.ToString();
                        string regno = FpSpread1.Sheets[0].Cells[res, 2].Text.ToString();
                        string rollno = FpSpread1.Sheets[0].Cells[res, 1].Text.ToString();
                        string applyedsubject = "select ea.subject_no  from Exam_Details ed,exam_appl_details ea,exam_application e,subject s, syllabus_master sy,sub_sem su where ed.exam_code =e.exam_code  and e.appl_no =ea.appl_no   and  s.subject_no =ea.subject_no   and  su.syll_code =sy.syll_code and su.subType_no =s.subType_no   and  sy.syll_code =s.syll_code and e.roll_no ='" + rollno + "' and e.Exam_type=4 and ed.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and ed.Exam_year='" + ddlYear.SelectedValue.ToString() + "'";
                        supplymsubds.Clear();
                        supplymsubds = d2.select_method_wo_parameter(applyedsubject, "text");
                        for (int i = 0; i < supplymsubds.Tables[0].Rows.Count; i++)
                        {
                            if (strsupplymsub == "")
                            {
                                strsupplymsub = supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                            else
                            {
                                strsupplymsub = strsupplymsub + "','" + supplymsubds.Tables[0].Rows[i]["subject_no"].ToString();
                            }
                        }
                        dsexamsub.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                        DataView dvhall = dsexamsub.Tables[0].DefaultView;
                        int stuexamsubcount = dvhall.Count;
                        if (stuexamsubcount > 0)
                        {
                            halfflag = true;
                            mypdfpage = mydocument.NewPage();
                            PdfTextArea ptc;
                            if (chkheadimage.Checked == true)
                            {
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 20, 20, 225);
                                }
                                coltop = 170;
                            }
                            else
                            {
                                coltop = coltop + 10;
                                ptc = new PdfTextArea(Fontboldbig, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, collname + " ( " + category + " )");
                                mypdfpage.Add(ptc);
                                coltop = coltop + 20;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, address);
                                mypdfpage.Add(ptc);
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                    mypdfpage.Add(LogoImage, 30, 10, 500);
                                }
                            }
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 0, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleCenter, "HALL TICKET");
                            mypdfpage.Add(ptc);
                            //if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            //{
                            //    PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            //    mypdfpage.Add(leftimage, 740, 10, 500);
                            //}
                            if ((afterenon.Trim() != "" && afterenon != null) || (forenon.Trim() != "" && forenon != null))
                            {
                                Double cot1 = coltop + 5;
                                ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 15, cot1, 800, 47), System.Drawing.ContentAlignment.MiddleLeft, "EXAM TIMINGS");
                                mypdfpage.Add(ptc);
                                Double sethe = cot1;
                                int he = 30;
                                if ((forenon.Trim() != "" && forenon != null))
                                {
                                    cot1 = cot1 + 8;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Forenoon  " + forenon + " ");
                                    mypdfpage.Add(ptc);
                                    he = he + 6;
                                    sethe = sethe + 5;
                                }
                                if ((afterenon.Trim() != "" && afterenon != null))
                                {
                                    cot1 = cot1 + 12;
                                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydocument, 15, cot1, 800, 51), System.Drawing.ContentAlignment.MiddleLeft, "Afternoon " + afterenon + " ");
                                    mypdfpage.Add(ptc);
                                    he = he + 6;
                                    sethe = sethe + 5;
                                }
                                PdfArea tete = new PdfArea(mydocument, 10, sethe, 190, he);
                                PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                mypdfpage.Add(pr1);
                            }
                            string batyera = string.Empty;
                            dshall.Tables[3].DefaultView.RowFilter = "reg_no='" + regno + "'";
                            DataView dvphoto = dshall.Tables[3].DefaultView;
                            if (dvphoto.Count > 0)
                            {
                                string roll = dvphoto[0]["roll_no"].ToString();
                                string currsem = dvphoto[0]["current_semester"].ToString();
                                string ccval = dvphoto[0]["cc"].ToString();
                                if (ccval.Trim() != "1" && ccval.Trim().ToLower() != "true")
                                {
                                    if (currsem.Trim() == "1" || currsem.Trim() == "2")
                                    {
                                        batyera = "I";
                                    }
                                    else if (currsem.Trim() == "3" || currsem.Trim() == "4")
                                    {
                                        batyera = "II";
                                    }
                                    else if (currsem.Trim() == "5" || currsem.Trim() == "6")
                                    {
                                        batyera = "III";
                                    }
                                    else if (currsem.Trim() == "7" || currsem.Trim() == "8")
                                    {
                                        batyera = "IV";
                                    }
                                    else if (currsem.Trim() == "9" || currsem.Trim() == "10")
                                    {
                                        batyera = "V";
                                    }
                                }
                                else
                                {
                                    batyera = "PRIVATE";
                                }
                                MemoryStream memoryStream = new MemoryStream();
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                    {
                                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                        {
                                            byte[] file = (byte[])dvphoto[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                                thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                    }
                                }
                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg")))
                                {
                                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + roll + ".jpeg"));
                                    mypdfpage.Add(LogoImage, 730, coltop - 40, 300);
                                }
                            }
                            coltop = coltop + 60;
                            Gios.Pdf.PdfTable table = mydocument.NewTable(Fontbold, 2, 3, 4);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.VisibleHeaders = false;
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(150);
                            table.Columns[2].SetWidth(50);
                            table.Cell(0, 1).SetFont(Fontboldd);
                            table.Cell(0, 2).SetFont(Fontboldd);
                            table.Cell(0, 0).SetFont(Fontboldd);
                            table.Cell(0, 0).SetContent("REG.NO");
                            table.Cell(0, 1).SetContent("NAME AND CLASS OF THE CANDIDATE");
                            table.Cell(0, 2).SetContent("MONTH & YEAR");
                            table.Cell(1, 1).SetFont(Fontboldd);
                            table.Cell(1, 2).SetFont(Fontboldd);
                            table.Cell(1, 0).SetFont(Fontboldd);
                            table.Cell(1, 0).SetContent(regno);
                            table.Cell(1, 1).SetContent(name + " (" + batyera + "  " + course + " " + degree + " )");
                            table.Cell(1, 2).SetContent(ddlMonth.SelectedItem.ToString() + " - " + ddlYear.Text.ToString());
                            table.Cell(1, 1).SetFont(Fontbold);
                            table.Cell(1, 2).SetFont(Fontbold);
                            table.Cell(1, 0).SetFont(Fontbold);
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage);
                            Double getheigh = newpdftabpage.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 20;
                            Gios.Pdf.PdfTable subtable = mydocument.NewTable(Fontsmall1, stuexamsubcount + 1, 7, 6);
                            subtable.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            subtable.VisibleHeaders = false;
                            subtable.Columns[0].SetWidth(30);
                            subtable.Columns[1].SetWidth(50);
                            subtable.Columns[2].SetWidth(150);
                            subtable.Columns[3].SetWidth(50);
                            subtable.Columns[4].SetWidth(40);
                            subtable.Columns[5].SetWidth(50);
                            subtable.Columns[6].SetWidth(30);
                            subtable.Cell(0, 1).SetFont(Fontbold1);
                            subtable.Cell(0, 2).SetFont(Fontbold1);
                            subtable.Cell(0, 3).SetFont(Fontbold1);
                            subtable.Cell(0, 4).SetFont(Fontbold1);
                            subtable.Cell(0, 5).SetFont(Fontbold1);
                            subtable.Cell(0, 6).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetFont(Fontbold1);
                            subtable.Cell(0, 0).SetContent("S.No");
                            subtable.Cell(0, 1).SetContent("CODE");
                            subtable.Cell(0, 2).SetContent("TITLE OF THE PAPER");
                            subtable.Cell(0, 3).SetContent(" DATE ");
                            subtable.Cell(0, 4).SetContent("SESSION");
                            subtable.Cell(0, 5).SetContent("HALL / ROOM");
                            subtable.Cell(0, 6).SetContent("SEAT");
                            int srno = 0;
                            for (int subc = 0; subc < dvhall.Count; subc++)
                            {
                                srno++;
                                //Boolean subjecttype = Convert.ToBoolean(dvhall[subc]["lab"].ToString());
                                string subcode = dvhall[subc]["subject_code"].ToString();
                                string subname = dvhall[subc]["subject_name"].ToString();
                                string edate = dvhall[subc]["edate"].ToString();
                                string ses = dvhall[subc]["exam_session"].ToString();
                                string subjectno = dvhall[subc]["subject_no"].ToString();
                                string room = string.Empty;
                                string seatno = string.Empty;
                                string[] sp = edate.Split('/');
                                dshall.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and edate='" + sp[1] + '/' + sp[0] + '/' + sp[2] + "' and ses_sion='" + ses + "' and regno='" + regno + "'";
                                DataView dvsea = dshall.Tables[1].DefaultView;
                                if (dvsea.Count > 0)
                                {
                                    room = dvsea[0]["roomno"].ToString();
                                    seatno = dvsea[0]["seat_no"].ToString();
                                }
                                subtable.Cell(srno, 0).SetContent(srno.ToString());
                                subtable.Cell(srno, 1).SetContent(subcode);
                                subtable.Cell(srno, 2).SetContent(subname);
                                subtable.Cell(srno, 3).SetContent(edate);
                                subtable.Cell(srno, 4).SetContent(ses);
                                subtable.Cell(srno, 5).SetContent(room);
                                subtable.Cell(srno, 6).SetContent(seatno);
                                subtable.Cell(srno, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                subtable.Cell(srno, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                subtable.Cell(srno, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                            }
                            Gios.Pdf.PdfTablePage newpdftabpage1 = subtable.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 10, coltop, 825, 1000));
                            mypdfpage.Add(newpdftabpage1);
                            getheigh = newpdftabpage1.Area.Height;
                            getheigh = Math.Round(getheigh, 0);
                            coltop = coltop + getheigh + 50;
                            PdfArea tete1 = new PdfArea(mydocument, 10, coltop - 50, 825, 175);
                            PdfRectangle pr2 = new PdfRectangle(mydocument, tete1, Color.Black);
                            mypdfpage.Add(pr2);
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 20, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                            mypdfpage.Add(ptc);
                            MemoryStream memoryStream1 = new MemoryStream();
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coe_signature.jpeg")))
                            {
                                if (dshall.Tables[0].Rows[0]["coe_signature"] != null && dshall.Tables[0].Rows[0]["coe_signature"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/coe_signature.jpeg")))
                                    {
                                        byte[] file = (byte[])dshall.Tables[0].Rows[0]["coe_signature"];
                                        memoryStream1.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream1, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/coe_signature.jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream1.Dispose();
                                        memoryStream1.Close();
                                    }
                                }
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/coe_signature.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/coe_signature.jpeg"));
                                mypdfpage.Add(LogoImage, 670, coltop + 20, 300);
                            }
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                        new PdfArea(mydocument, 650, coltop + 80, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Controller of Examinations");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 30;
                            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 20, coltop + 100, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, "Instructions :");
                            mypdfpage.Add(ptc);
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 130, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(i)   During the examinations,students should produce Hall-Tickets and ID cards to the Invigilators.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 140, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(ii)  Students should enter the examination Hall ten minutes before the commencement of the examination.");
                            mypdfpage.Add(ptc);
                            coltop = coltop + 10;
                            ptc = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(mydocument, 100, coltop + 150, 800, 60), System.Drawing.ContentAlignment.MiddleLeft, "(iii) Students shall not bring cell phones and programmable calculators inside the Examination Hall.");
                            mypdfpage.Add(ptc);
                            mypdfpage.SaveToDocument();
                            lblerror.Visible = false;
                        }
                    }
                }
                if (halfflag == true)
                {
                    lblerror.Visible = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "ExamHallTicket.pdf";
                        mydocument.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                else
                {
                    lblerror.Text = "Please Select the Student and then Proceed";
                    lblerror.Visible = true;
                }
            }
            else
            {
                lblerror.Text = "Please Select Exam Month And Year";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }

    public void hallticketformat5()
    {
        try
        {

            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            System.Drawing.Font Fontbold1 = new System.Drawing.Font("Algerian", 13, FontStyle.Bold);
            System.Drawing.Font font2bold = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Bold);
            System.Drawing.Font font2small = new System.Drawing.Font("Palatino Linotype", 11, FontStyle.Regular);
            System.Drawing.Font font3bold = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Bold);
            System.Drawing.Font font3small = new System.Drawing.Font("Palatino Linotype", 9, FontStyle.Regular);
            System.Drawing.Font font4bold = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);
            System.Drawing.Font font4small = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Regular);
            System.Drawing.Font font4smallnew = new System.Drawing.Font("Palatino Linotype", 7, FontStyle.Bold);

            string batchyear = Convert.ToString(ddlbatch.SelectedValue);
            string degree_code = Convert.ToString(ddlbranch.SelectedValue);
            string exammonth1 = ddlMonth.SelectedValue.ToString();
            string examyear = ddlYear.SelectedValue.ToString();
            string strqueryexamtimetable = "select s.subject_code,et.subject_no,convert(nvarchar(15),et.exam_date,103) edate, et.exam_session,RIGHT(CONVERT(VARCHAR,et.start_time,100),7) stime,RIGHT(CONVERT(VARCHAR,et.end_time,100),7) etime from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no AND e.batchFrom='" + batchyear + "' and e.degree_code='" + degree_code + "' and e.Exam_month='" + exammonth1 + "' and e.Exam_year='" + examyear + "' order by s.subjectpriority";//Altered by madhumathi on 20/04/2018
            strqueryexamtimetable = strqueryexamtimetable + " select reg_no,roll_no,current_semester,(select s.Photo from stdphoto s where r.app_no=s.app_no) as Photo,cc from Registration r where  r.degree_code='" + degree_code + "' and r.Batch_Year='" + batchyear + "'";

            string getdegInfo = da.GetFunction(" select (c.Course_Name+'-'+de.Dept_Name) as dept from course c,Department de,Degree d where d.Dept_Code=de.Dept_Code and d.Course_Id=c.Course_Id and d.Degree_Code='" + degree_code + "'");

            DataSet dsextime = d2.select_method_wo_parameter(strqueryexamtimetable, "Text");
            string applsubjectquery = "select ea.roll_no,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,sy.semester,ss.Lab,(select et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth1 + "' and e.exam_year='" + examyear + "') edate from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ed.degree_code='" + degree_code + "' and ed.exam_month='" + exammonth1 + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batchyear + " order by edate,ea.roll_no,ead.attempts,s.subject_code";
            DataSet dssubappl = d2.select_method_wo_parameter(applsubjectquery, "Text");
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    string rollno = FpSpread1.Sheets[0].Cells[res, 1].Text;
                    string rollnosub = FpSpread1.Sheets[0].Cells[res, 1].Note;
                    string exammonth = monthinwords(ddlMonth.SelectedValue.ToString());
                    string stuname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                    string regNo = FpSpread1.Sheets[0].Cells[res, 2].Text;
                    string dob = FpSpread1.Sheets[0].Cells[res, 3].Note.ToString();
                    string sem = Convert.ToString(FpSpread1.Sheets[0].Cells[res, 1].Tag);
                    //string edulevel = Session["Edulevel"].ToString();
                    string collegecode = Session["collegecode"].ToString();
                    string coename = string.Empty; ;

                    DataSet dsColInfo = d2.select_method_wo_parameter("select UPPER(collname)+' ('+UPPER(Category)+')' as Line1,UPPER(district)+' - '+pincode as distr,affliatedby from collinfo where college_code=" + collegecode + "", "Text");
                    string Line1 = string.Empty;
                    string Line2 = string.Empty;
                    string Line3 = string.Empty;
                    string Line4 = string.Empty;
                    string Line5 = string.Empty;
                    if (dsColInfo.Tables.Count > 0 && dsColInfo.Tables[0].Rows.Count > 0)
                    {
                        Line1 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["Line1"]).Trim();
                        try
                        {
                            string[] affli = Convert.ToString(dsColInfo.Tables[0].Rows[0]["affliatedby"]).Trim().Split('\\');
                            Line2 = affli[0].Split(',')[0];
                            Line4 = "(" + affli[2].Split(',')[0] + ")";
                            Line3 = affli[1].Split(',')[0];
                        }
                        catch { }
                        Line5 = Convert.ToString(dsColInfo.Tables[0].Rows[0]["distr"]).Trim();
                    }
                    string Line6 = "HALL TICKET";
                    string Line7 = "SEMESTER EXAMINATION - " + exammonth + " " + examyear;
                    string Line8 = "COURSE - " + getdegInfo;
                    string studName = "STUDENT NAME : " + stuname.ToUpper();
                    string rollNumber = "ROLL NO : " + rollno.ToUpper();
                    string regNumber = "REG.NO : " + regNo.ToUpper();

                    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();

                    int coltop = 15;
                    #region Top
                    //Div 1
                    PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 170);
                    Gios.Pdf.PdfRectangle pr1 = new Gios.Pdf.PdfRectangle(mydoc, tete, Color.Black);
                    mypdfpage.Add(pr1);

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode + ".jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + collegecode + ".jpeg"));
                        mypdfpage.Add(LogoImage, 20, 20, 400);
                    }
                    PdfTextArea pdfSince = new PdfTextArea(font3small, System.Drawing.Color.Black, new PdfArea(mydoc, 35, 84, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "Since 1951");
                    mypdfpage.Add(pdfSince);
                    //student photo on 14-5-2018 by Rajkumar for sns 
                    dsextime.Tables[1].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                    DataView dvphoto = dsextime.Tables[1].DefaultView;
                    MemoryStream memoryStream = new MemoryStream();
                    if (dvphoto.Count > 0)
                    {
                        if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                        {
                            if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                            {
                                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                                {
                                    byte[] file = (byte[])dvphoto[0]["photo"];
                                    memoryStream.Write(file, 0, file.Length);
                                    if (file.Length > 0)
                                    {
                                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                    }
                                    memoryStream.Dispose();
                                    memoryStream.Close();
                                }
                            }
                        }
                    }
                    //-------------------------------------------
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"));
                        mypdfpage.Add(LogoImage, 500, 20, 400);
                    }

                    PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 50, coltop, 460, 30), System.Drawing.ContentAlignment.MiddleCenter, Line1);
                    mypdfpage.Add(ptc);
                    coltop += 25;
                    PdfTextArea ptc1 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line2);
                    mypdfpage.Add(ptc1);
                    coltop += 20;
                    PdfTextArea ptc2 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line3);
                    mypdfpage.Add(ptc2);
                    coltop += 20;
                    PdfTextArea ptc3 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line4);
                    mypdfpage.Add(ptc3);
                    coltop += 20;
                    PdfTextArea ptc4 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line5);
                    mypdfpage.Add(ptc4);
                    coltop += 20;
                    PdfTextArea ptc5 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line6);
                    mypdfpage.Add(ptc5);
                    coltop += 20;
                    PdfTextArea ptc6 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line7);
                    mypdfpage.Add(ptc6);
                    coltop += 20;
                    PdfTextArea ptc7 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 50, coltop, 460, 20), System.Drawing.ContentAlignment.MiddleCenter, Line8);
                    mypdfpage.Add(ptc7);
                    coltop += 35;
                    //DIv 2
                    PdfTextArea ptc8 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 25, coltop, 300, 20), System.Drawing.ContentAlignment.MiddleLeft, studName);
                    mypdfpage.Add(ptc8);

                    PdfTextArea ptc9 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 325, coltop, 125, 20), System.Drawing.ContentAlignment.MiddleLeft, rollNumber);
                    mypdfpage.Add(ptc9);

                    PdfTextArea ptc10 = new PdfTextArea(font2bold, System.Drawing.Color.Black,
                                                     new PdfArea(mydoc, 450, coltop, 125, 20), System.Drawing.ContentAlignment.MiddleLeft, regNumber);
                    mypdfpage.Add(ptc10);

                    coltop += 25;

                    #endregion
                    #region Middle

                    PdfTable tableTimeTable = mydoc.NewTable(font3bold, 11, 12, 5);
                    tableTimeTable.VisibleHeaders = false;
                    tableTimeTable.SetBorders(Color.Black, 1.0, BorderType.CompleteGrid);

                    //Headers
                    tableTimeTable.Cell(0, 0).SetContent("S. NO");
                    tableTimeTable.Cell(0, 1).SetContent("DATE");
                    tableTimeTable.Cell(0, 2).SetContent("COURSE CODE");
                    tableTimeTable.Cell(0, 3).SetContent("INITIALS OF HALL SUPDT.");
                    tableTimeTable.Cell(0, 4).SetContent("S. NO");
                    tableTimeTable.Cell(0, 5).SetContent("DATE");
                    tableTimeTable.Cell(0, 6).SetContent("COURSE CODE");
                    tableTimeTable.Cell(0, 7).SetContent("INITIALS OF HALL SUPDT.");
                    tableTimeTable.Cell(0, 8).SetContent("S. NO");
                    tableTimeTable.Cell(0, 9).SetContent("DATE");
                    tableTimeTable.Cell(0, 10).SetContent("COURSE CODE");
                    tableTimeTable.Cell(0, 11).SetContent("INITIALS OF HALL SUPDT.");
                    tableTimeTable.Columns[0].SetWidth(20);
                    tableTimeTable.Columns[1].SetWidth(55);
                    tableTimeTable.Columns[2].SetWidth(70);
                    tableTimeTable.Columns[3].SetWidth(65);
                    tableTimeTable.Columns[4].SetWidth(20);
                    tableTimeTable.Columns[5].SetWidth(55);
                    tableTimeTable.Columns[6].SetWidth(70);
                    tableTimeTable.Columns[7].SetWidth(65);
                    tableTimeTable.Columns[8].SetWidth(20);
                    tableTimeTable.Columns[9].SetWidth(55);
                    tableTimeTable.Columns[10].SetWidth(70);
                    tableTimeTable.Columns[11].SetWidth(65);

                    #region Coursecode retrieval

                    DataTable dtTImeTable = new DataTable();
                    dtTImeTable.Columns.Add("sem", typeof(string));
                    dtTImeTable.Columns.Add("subcode", typeof(string));
                    dtTImeTable.Columns.Add("subno", typeof(string));
                    dtTImeTable.Columns.Add("Date", typeof(DateTime));
                    dtTImeTable.Columns.Add("session", typeof(string));
                    dtTImeTable.Columns.Add("subject", typeof(string));
                    dssubappl.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                    DataView dvexamappl = dssubappl.Tables[0].DefaultView;
                    for (int sn = 0; sn < dvexamappl.Count; sn++)
                    {
                        string subcode = dvexamappl[sn]["subject_code"].ToString();
                        string subname = dvexamappl[sn]["subject_name"].ToString();
                        string subno = dvexamappl[sn]["subject_no"].ToString();
                        string chckname = subname.Trim().ToLower();
                        string subsem = dvexamappl[sn]["semester"].ToString();
                        string session = string.Empty;
                        string date = string.Empty;
                        DateTime dat = new DateTime();
                        dsextime.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "'";
                        DataView dvtimetable = dsextime.Tables[0].DefaultView;
                        if (dvtimetable.Count > 0)
                        {
                            date = Convert.ToString(dvtimetable[0]["edate"]);
                            string[] split = date.Split('/');
                            //dat = Convert.ToDateTime(split[0] + "/" + split[1] + "/" + split[2]);
                            dat = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                            session = dvtimetable[0]["exam_session"].ToString();
                        }

                        dtTImeTable.Rows.Add(subsem, subcode, subno, dat, session, subname);
                    }
                    DataView view = dtTImeTable.AsDataView();
                    view.Sort = "Date ASC, session DESC";

                    int tblRowIndx = 1;
                    int tblColIndx = 0;
                    int round = 1;
                    int tblSNo = 1;
                    int totalSub = view.Count;
                    if (totalSub > 30)
                    {
                        totalSub = 30;
                    }
                    for (int sn = 0; sn < totalSub; sn++, tblRowIndx++, tblSNo++)
                    {
                        string subcode = Convert.ToString(view[sn]["subcode"]);
                        string subname = Convert.ToString(view[sn]["subject"]);
                        string subno = Convert.ToString(view[sn]["subno"]);
                        string chckname = subname.Trim().ToLower();
                        string subsem = Convert.ToString(view[sn]["sem"]);
                        string date = Convert.ToDateTime(view[sn]["date"]).ToString("dd/MM/yyyy");
                        string session = Convert.ToString(view[sn]["session"]);
                        DateTime dtDate = new DateTime();
                        DateTime.TryParseExact(date, "dd/MM/yyyy", null, DateTimeStyles.None, out dtDate);
                        if (subsem.Trim().ToLower() != sem.Trim().ToLower())
                        {
                            subcode += "*";
                        }
                        else { subcode += " "; }

                        if (tblRowIndx == 11)
                        {
                            tblRowIndx = 1;
                            tblColIndx = (4 * round);

                            round++;
                        }

                        tableTimeTable.Cell(tblRowIndx, tblColIndx).SetContent(tblSNo);
                        DateTime dtDefault = new DateTime();
                        if (dtDate != dtDefault)
                        {
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 1)).SetContent(date + " / " + session);
                        }
                        tableTimeTable.Cell(tblRowIndx, (tblColIndx + 1)).SetFont(font4smallnew);
                        tableTimeTable.Cell(tblRowIndx, (tblColIndx + 2)).SetContent(subcode);
                        //tableTimeTable.Cell(tblRowIndx, (tblColIndx+3)).SetContent("INITIALS OF HALL SUPDT.");
                    }
                    if (tblSNo < 11)
                    {
                        for (; tblSNo < 11; tblSNo++)
                        {
                            tableTimeTable.Cell(tblSNo, 0).SetContent(tblSNo);
                        }
                    }
                    #endregion

                    PdfTablePage addtabletopage1 = tableTimeTable.CreateTablePage(new PdfArea(mydoc, 10, coltop - 5, 570, 300));
                    mypdfpage.Add(addtabletopage1);

                    #endregion

                    #region Bottom

                    coltop = 552;
                    PdfTextArea ptcBIS = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 70, coltop - 60, 425, 30), System.Drawing.ContentAlignment.MiddleLeft, "(* indicates Arrear Paper, If there is any discrepancy in the Hall-Ticket contact the C.O.E. Immediately)");
                    mypdfpage.Add(ptcBIS);

                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/coesign" + collegecode + ".jpeg")))
                    {
                        Gios.Pdf.PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/coesign" + collegecode + ".jpeg"));
                        mypdfpage.Add(LogoImage, 440, coltop - 40, 600);
                    }

                    PdfTextArea ptcBS = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 20, coltop, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                    mypdfpage.Add(ptcBS);

                    PdfTextArea ptcBCS = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 400, coltop, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "CONTROLLER OF EXAMINATIONS");
                    mypdfpage.Add(ptcBCS);

                    PdfArea tete3 = new PdfArea(mydoc, 15, 575, 565, 250);
                    Gios.Pdf.PdfRectangle pr3 = new Gios.Pdf.PdfRectangle(mydoc, tete3, Color.Black);
                    mypdfpage.Add(pr3);

                    coltop += 20;
                    PdfTextArea ptcB1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 50, coltop, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "INSTRUCTIONS TO THE CANDIDATE");
                    mypdfpage.Add(ptcB1);
                    coltop += 2;
                    PdfTextArea ptcB2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 50, coltop, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "_______________________________");
                    mypdfpage.Add(ptcB2);
                    coltop += 20;
                    string newline = "1.\tCandidates must sign the Hall Ticket when it is issued. They must bring the Hall Ticket and Identity Card to every examination and\n\t\t\t produce the same on demand.";
                    PdfTextArea ptcB3 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB3);
                    coltop += 20;
                    newline = "2.\tCandidates shall not be permitted to enter the examination hall after the expiry of 30 minutes from the commencement of examination.";
                    PdfTextArea ptcB4 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB4);
                    coltop += 20;
                    newline = "3.\tCandidates shall not be allowed to leave the examination hall before the expiry of 45 minutes from the commencement of examination.";
                    PdfTextArea ptcB5 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB5);
                    coltop += 20;
                    newline = "4.\tIf the candidates are found in possession of any written and printed material, cell phone and programmable calculator in the\n\t\t\t examination hall shall be liable for disciplinary action.";
                    PdfTextArea ptcB6 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                             new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB6);
                    coltop += 20;
                    newline = "5.\tCandidates indulging in Malpractice of any kind will be severely dealt with.";
                    PdfTextArea ptcB7 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB7);
                    coltop += 20;
                    newline = "6.\tCandidate's seating arrangement is in rotation.";
                    PdfTextArea ptcB8 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB8);
                    coltop += 2;
                    PdfTextArea ptcB9 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "\t\t______________________________________");
                    mypdfpage.Add(ptcB9);
                    coltop += 20;
                    PdfTextArea ptcB10 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 50, coltop, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "PUNISHMENT FOR MALPRACTICE");
                    mypdfpage.Add(ptcB10);
                    coltop += 2;
                    PdfTextArea ptcB11 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 50, coltop, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "_____________________________");
                    mypdfpage.Add(ptcB11);
                    coltop += 20;
                    newline = @"i)\tPossession of Hand written\\ Xerox material\\ Mobile phone - Particular paper will be cancelled.";
                    PdfTextArea ptcB12 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 70, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB12);
                    coltop += 20;
                    newline = @"ii)\tWriting the examination by copying Hand written\\ Xerox material\\ Mobile phone - Particular and Subsequent papers will be\n\t\t\t cancelled.";
                    PdfTextArea ptcB13 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 70, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB13);
                    coltop += 20;
                    newline = @"iii)\tFor Repeating the Malpractice - All the papers of the semester will be cancelled.";
                    PdfTextArea ptcB14 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 70, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                    mypdfpage.Add(ptcB14);
                    coltop += 20;

                    #endregion
                    mypdfpage.SaveToDocument();
                }
            }

            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                Response.Buffer = true;
                Response.Clear();
                string szPath = appPath + "/Report/";
                //string szFile = "" + Session["regno"].ToString() + DateTime.Now.ToString("ddMMyyyyHHmmsstt") + ".pdf";
                string szFile = "Examhallticket" + ".pdf";
                mydoc.SaveToFile(szPath + szFile);

                Response.ClearHeaders();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);

            }
        }
        catch
        {

        }

    }

    public string loadexammonthtext()
    {
        string sextexammonth = "";
        try
        {

            string exammonth = ddlMonth.SelectedValue.ToString();
            if (exammonth == "1")
            {
                sextexammonth = "JAN";
            }
            else if (exammonth == "2")
            {
                sextexammonth = "FEB";
            }
            else if (exammonth == "3")
            {
                sextexammonth = "MAR";
            }
            else if (exammonth == "4")
            {
                sextexammonth = "APR";
            }
            else if (exammonth == "5")
            {
                sextexammonth = "MAY";
            }
            else if (exammonth == "6")
            {
                sextexammonth = "JUN";
            }
            else if (exammonth == "7")
            {
                sextexammonth = "JUL";
            }
            else if (exammonth == "8")
            {
                sextexammonth = "AUG";
            }
            else if (exammonth == "9")
            {
                sextexammonth = "SEP";
            }
            else if (exammonth == "10")
            {
                sextexammonth = "OCT";
            }
            else if (exammonth == "11")
            {
                sextexammonth = "NOV";
            }
            else
            {
                sextexammonth = "DEC";
            }

        }
        catch
        {
            return "";
        }
        return sextexammonth;
    }
    public string loadexammonthFULLtext()
    {
        string sextexammonth = "";
        try
        {

            string exammonth = ddlMonth.SelectedValue.ToString();
            if (exammonth == "1")
            {
                sextexammonth = "JANUARY";
            }
            else if (exammonth == "2")
            {
                sextexammonth = "FEBRUARY";
            }
            else if (exammonth == "3")
            {
                sextexammonth = "MARCH";
            }
            else if (exammonth == "4")
            {
                sextexammonth = "APRIL";
            }
            else if (exammonth == "5")
            {
                sextexammonth = "MAY";
            }
            else if (exammonth == "6")
            {
                sextexammonth = "JUNE";
            }
            else if (exammonth == "7")
            {
                sextexammonth = "JULY";
            }
            else if (exammonth == "8")
            {
                sextexammonth = "AUGUST";
            }
            else if (exammonth == "9")
            {
                sextexammonth = "SEPTEMBER";
            }
            else if (exammonth == "10")
            {
                sextexammonth = "OCTOBER";
            }
            else if (exammonth == "11")
            {
                sextexammonth = "NOVEMBER";
            }
            else
            {
                sextexammonth = "DECEMBER";
            }

        }
        catch
        {
            return "";
        }
        return sextexammonth;
    }

    public void hallticketformat3()
    {
        try
        {
            FpSpread1.SaveChanges();
            int selectedcount = 0;
            for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                if (isval == 1)
                {
                    selectedcount++;
                }
            }
            if (selectedcount == 0)
            {
                lblerror.Visible = true;
                lblerror.Text = "Please Select the Student and then Proceed";
                return;
            }
            string degreecode = ddlbranch.SelectedValue.ToString();
            string batch = ddlbatch.SelectedValue.ToString();
            string degree = ddlbranch.SelectedItem.ToString();
            string course = ddldegree.SelectedItem.ToString();
            string exammonth = ddlMonth.SelectedIndex.ToString();
            string exammonthnew = ddlMonth.SelectedItem.Text;
            string examyear = ddlYear.SelectedValue.ToString();
            string coename = string.Empty;
            string princ = string.Empty;
            // string strqueryexamtimetable = "select s.subject_code,et.subject_no,convert(nvarchar(15),et.exam_date,103) edate, et.exam_session,RIGHT(CONVERT(VARCHAR,et.start_time,100),7) stime,RIGHT(CONVERT(VARCHAR,et.end_time,100),7) etime from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no AND e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "'";//commended by madhumathi on 20/04/2018

            string strqueryexamtimetable = "select s.subject_code,et.subject_no,convert(nvarchar(15),et.exam_date,103) edate, et.exam_session,RIGHT(CONVERT(VARCHAR,et.start_time,100),7) stime,RIGHT(CONVERT(VARCHAR,et.end_time,100),7) etime from exmtt e,exmtt_det et,subject s where e.exam_code=et.exam_code and et.subject_no=s.subject_no AND e.batchFrom='" + batch + "' and e.degree_code='" + degreecode + "' and e.Exam_month='" + exammonth + "' and e.Exam_year='" + examyear + "' order by s.subjectpriority";//Altered by madhumathi on 20/04/2018
            strqueryexamtimetable = strqueryexamtimetable + " select reg_no,roll_no,current_semester,(select s.Photo from stdphoto s where r.app_no=s.app_no) as Photo,cc from Registration r where  r.degree_code='" + degreecode + "' and r.Batch_Year='" + batch + "'";

            DataSet dsextime = d2.select_method_wo_parameter(strqueryexamtimetable, "Text");
            string applsubjectquery = "select ea.roll_no,ead.attempts,s.subject_code,s.subject_name,ead.subject_no,sy.semester,ss.Lab,(select et.exam_date from exmtt e,exmtt_det et where e.exam_code=et.exam_code and et.subject_no=ead.subject_no and e.exam_month='" + exammonth + "' and e.exam_year='" + examyear + "') edate from Exam_Details ed,exam_application ea,exam_appl_details ead,subject s,sub_sem ss,syllabus_master sy where ed.exam_code=ea.exam_code and ea.appl_no=ead.appl_no and ead.subject_no=s.subject_no and s.subType_no=ss.subType_no  and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ed.degree_code='" + degreecode + "' and ed.exam_month='" + exammonth + "' and ed.exam_year='" + examyear + "' and ed.batch_year=" + batch + " order by edate,ea.roll_no,ead.attempts,s.subject_code";
            DataSet dssubappl = d2.select_method_wo_parameter(applsubjectquery, "Text");

            string selectQ = "select eb.AppNo,eb.Batch,convert(nvarchar(15),eb.ExamDate,103) as edate,eb.ExamSession,SubNo from examtheorybatch eb,Exam_Details ed where ed.exam_code=eb.ExamCode and ed.Exam_Month='" + exammonth + "' and ed.Exam_year='" + examyear + "'  and ed.batch_year=" + batch + "  and ed.degree_code='" + degreecode + "'";
            DataTable dtBatch = dir.selectDataTable(selectQ);

            Font Fontbold1 = new Font("Algerian", 15, FontStyle.Bold);
            Font font2bold = new Font("Palatino Linotype", 11, FontStyle.Bold);
            Font font2small = new Font("Palatino Linotype", 11, FontStyle.Regular);
            Font font2small1 = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font3bold = new Font("Palatino Linotype", 9, FontStyle.Bold);
            Font font3small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Font font4bold = new Font("Palatino Linotype", 7, FontStyle.Bold);
            Font font4small = new Font("Palatino Linotype", 9, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            string strquery = "select * from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            string Collegename = string.Empty;
            string aff = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                Collegename = ds.Tables[0].Rows[0]["Collname"].ToString();
                aff = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                string[] strpa = aff.Split(',');
                aff = strpa[0];
                coename = ds.Tables[0].Rows[0]["coe"].ToString();
                princ = ds.Tables[0].Rows[0]["principal"].ToString();

            }
            if ((ddlMonth.SelectedValue.ToString() != "0") && (ddlYear.SelectedValue.ToString() != "0"))
            {
                FpSpread1.SaveChanges();
                for (int res = 1; res <= Convert.ToInt32(FpSpread1.Sheets[0].RowCount) - 1; res++)
                {
                    int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[res, 4].Value);
                    if (isval == 1)
                    {
                        Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                        string sem = string.Empty;
                        string rollno = FpSpread1.Sheets[0].Cells[res, 1].Text;
                        string rollnosub = FpSpread1.Sheets[0].Cells[res, 1].Note;
                        string exammonthnew1 = monthinwords(exammonthnew);
                        string stuname = FpSpread1.Sheets[0].Cells[res, 3].Text;
                        string regnumber = FpSpread1.Sheets[0].Cells[res, 2].Text;
                        string dob = FpSpread1.Sheets[0].Cells[res, 3].Note.ToString();
                      //  PdfArea tete = new PdfArea(mydoc, 15, 15, 565, 810);
                      //  PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                      //  mypdfpage.Add(pr1);
                       // PdfArea tetep = new PdfArea(mydoc, 460, 15, 120, 145);
                       // PdfRectangle pr1p = new PdfRectangle(mydoc, tetep, Color.Black);
                       // mypdfpage.Add(pr1p);
                        int coltop = 25;
                        // DataTable dvphoto = new DataTable();
                        //if (chkheadimage.Checked == true)
                        //{
                        //    if (File.Exists(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg")))
                        //    {
                        //        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/coeimages/printheader" + Session["collegecode"].ToString() + ".jpeg"));
                        //        mypdfpage.Add(LogoImage, 20, 20, 500);
                        //    }
                        //    coltop = 60;
                        //}
                        //else
                        //{
                        //    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg")))
                        //    {
                        //        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo" + Session["collegecode"].ToString() + ".jpeg"));
                        //        mypdfpage.Add(LogoImage, 20, 20, 400);
                        //    }
                        //    else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        //    {
                        //        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        //        mypdfpage.Add(LogoImage, 20, 20, 400);
                        //    }
                        //    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/right_logo" + Session["collegecode"].ToString() + ".jpeg")))
                        //    {
                        //        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/right_logo" + Session["collegecode"].ToString() + ".jpeg"));
                        //        mypdfpage.Add(LogoImage, 380, 20, 400);
                        //    }
                        //    else if (File.Exists(HttpContext.Current.Server.MapPath("~/college/right_logo.jpeg")))
                        //    {
                        //        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/right_logo.jpeg"));
                        //        mypdfpage.Add(LogoImage, 380, 20, 400);
                        //    }
                        //    PdfTextArea ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                           new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, Collegename);
                        //    mypdfpage.Add(ptc);
                          coltop = coltop + 20;
                        //    PdfTextArea ptc02 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                            new PdfArea(mydoc, 30, coltop, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, aff);
                        //    mypdfpage.Add(ptc02);
                        //}
                        //student photo on 14-5-2018 by Rajkumar for sns 
                        dsextime.Tables[1].DefaultView.RowFilter = "roll_no='" + rollno + "'";
                        DataView dvphoto = dsextime.Tables[1].DefaultView;
                        MemoryStream memoryStream = new MemoryStream();
                        if (dvphoto.Count > 0)
                        {
                            if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                            {
                                if (dvphoto[0]["photo"] != null && dvphoto[0]["photo"].ToString().Trim() != "")
                                {
                                    if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                                    {
                                        byte[] file = (byte[])dvphoto[0]["photo"];
                                        memoryStream.Write(file, 0, file.Length);
                                        if (file.Length > 0)
                                        {
                                            System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                            System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                                            thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                }
                            }
                        }
                        //-------------------------------------------
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg")))
                        {
                            PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + rollno + ".jpeg"));
                            mypdfpage.Add(LogoImage, 428, 170, 300);
                        }
                    
                        coltop = coltop + 25;
                       
                        coltop = coltop + 65;

                        int printmonth = 0;
                        int.TryParse(Convert.ToString(ddlMonth.SelectedValue), out printmonth);//Deepali 14.5.18
                        DropDownList pntddlMonth = new DropDownList();
                        pntddlMonth.Items.Clear();
                        pntddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                        pntddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                        pntddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                        pntddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                        pntddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                        pntddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
                        pntddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                        pntddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                        pntddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                        pntddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                        pntddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                        pntddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                        pntddlMonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                        if (printmonth == 1)
                        {
                            int yr = Convert.ToInt32(ddlYear.SelectedItem.Text) - 1;
                            PdfTextArea ptc031a = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                   new PdfArea(mydoc, 80, coltop + 15, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, "SEMESTER END EXAMINATIONS - DEC "+Convert.ToString(yr)+"/" + pntddlMonth.Items[printmonth].Text.ToString().ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
                            mypdfpage.Add(ptc031a);
                        }
                        else
                        {
                            PdfTextArea ptc031a = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 80, coltop + 15, 450, 30), System.Drawing.ContentAlignment.MiddleCenter, "SEMESTER END EXAMINATIONS - " + pntddlMonth.Items[printmonth - 1].Text.ToString().ToUpper() +" "+ddlYear.SelectedItem.Text.ToString()+ "/" + pntddlMonth.Items[printmonth].Text.ToString().ToUpper() + " " + ddlYear.SelectedItem.Text.ToString() + "");
                            mypdfpage.Add(ptc031a);
                        }
                        coltop = coltop + 38;
                        //PdfArea tetet = new PdfArea(mydoc, 15, coltop, 445, 60);
                        //PdfRectangle pr1t = new PdfRectangle(mydoc, tetet, Color.Black);
                        //mypdfpage.Add(pr1t);
                        PdfTextArea ptc07 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 58, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Register Number");
                        mypdfpage.Add(ptc07);
                        PdfTextArea ptc08na = new PdfTextArea(font2small1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 165, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + regnumber);
                      
                        coltop = coltop + 20;
                        PdfTextArea ptc08 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 58, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name");
                        mypdfpage.Add(ptc08);
                        PdfTextArea ptc08na1 = new PdfTextArea(font2small1, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 168, coltop-2, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + stuname.ToString().ToUpper() + "");
                        mypdfpage.Add(ptc08na1);
                        coltop = coltop + 20;
                        PdfTextArea ptcsem = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                            new PdfArea(mydoc, 58, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Degree & Branch");
                        mypdfpage.Add(ptcsem);
                       
                        PdfTextArea ptcsem1 = new PdfTextArea(font2small1, System.Drawing.Color.Black,
                                                                                new PdfArea(mydoc, 168, coltop-2, 400, 30), System.Drawing.ContentAlignment.MiddleLeft,": "+ ddldegree.SelectedItem.ToString() + " - " + ddlbranch.SelectedItem.ToString() + "");
                            mypdfpage.Add(ptcsem1);
                       
                       
                        coltop = coltop + 20;

                        mypdfpage.Add(ptc08na);
                        PdfTextArea ptc071 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 58, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "DOB");
                        mypdfpage.Add(ptc071);
                        PdfTextArea ptc071a = new PdfTextArea(font2small1, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 168, coltop-2, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + dob);
                        mypdfpage.Add(ptc071a);

                        mypdfpage.Add(ptc08na);
                        PdfTextArea ptc0711 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 290, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Gender");
                        mypdfpage.Add(ptc0711);
                        string gender = string.Empty;
                        string gen = d2.GetFunction("select sex from applyn where app_no in (select app_no from registration where reg_no='" + regnumber + "')");
                        if (!string.IsNullOrEmpty(gen))
                        {
                            if (gen == "0")
                            {
                                gender = "Male";
                            }
                            else if (gen == "1")
                            {
                                gender = "Female";
                            }
                            else
                            {
                                gender = "Trans-Gender";
                            }
                        }
                        PdfTextArea ptc0711a = new PdfTextArea(font2small1, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 330, coltop-1, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, ": " + gender);
                        mypdfpage.Add(ptc0711a);

                        coltop = coltop + 10;

                        dssubappl.Tables[0].DefaultView.RowFilter = " roll_no='" + rollno + "'";
                        DataView dvexamappl = dssubappl.Tables[0].DefaultView;
                        DataTable dtTImeTable = new DataTable();
                        dtTImeTable.Columns.Add("sem", typeof(string));
                        dtTImeTable.Columns.Add("Date", typeof(string));
                        dtTImeTable.Columns.Add("subcode", typeof(string));
                        dtTImeTable.Columns.Add("subject", typeof(string));
                        coltop = coltop + 10;

                        int lonehei = (dvexamappl.Count + 1) * 14;



                        for (int sn = 0; sn < dvexamappl.Count; sn++)
                        {
                            string subcode = dvexamappl[sn]["subject_code"].ToString();
                            string subname = dvexamappl[sn]["subject_name"].ToString();
                            string subno = dvexamappl[sn]["subject_no"].ToString();
                            string chckname = subname.Trim().ToLower();
                            string subsem = dvexamappl[sn]["semester"].ToString();
                            string slab = dvexamappl[sn]["lab"].ToString();


                            dsextime.Tables[0].DefaultView.RowFilter = "subject_code='" + subcode + "'";

                            DataView dvtimetable = dsextime.Tables[0].DefaultView;
                            string appNo = da.GetFunction("select App_No from Registration where Roll_No='" + rollno + "'");
                            dtBatch.DefaultView.RowFilter = "AppNo='" + appNo + "' and SubNo='" + subno + "'";
                            DataView dtDateSes = dtBatch.DefaultView;
                            string date = string.Empty;
                            string sess = string.Empty;
                            if (slab.Trim().ToLower() == "true" || slab.Trim() == "1")
                            {
                                if (chkboxvdate.Checked == true)
                                {

                                    if (dvtimetable.Count > 0)
                                    {
                                        date = dvtimetable[0]["edate"].ToString();
                                        sess = dvtimetable[0]["exam_session"].ToString();
                                    }
                                    if (dtDateSes.Count > 0)
                                    {
                                        date = dtDateSes[0]["edate"].ToString();
                                        sess = dtDateSes[0]["ExamSession"].ToString();
                                    }
                                }
                            }
                            else
                            {
                                if (CheckBox1.Checked == true)
                                {
                                    if (dvtimetable.Count > 0)
                                    {
                                        date = dvtimetable[0]["edate"].ToString();
                                        sess = dvtimetable[0]["exam_session"].ToString();
                                    }
                                    if (dtDateSes.Count > 0)
                                    {
                                        date = dtDateSes[0]["edate"].ToString();
                                        sess = dtDateSes[0]["ExamSession"].ToString();
                                    }
                                }
                            }
                            if (slab.Trim().ToLower() == "true" || slab.Trim() == "1")
                            {
                                if (cbpractical.Checked == true)
                                {
                                        dtTImeTable.Rows.Add(subsem, date + " - " + sess, subcode, subname);
                                }
                            }
                            else
                            {
                               
                                    dtTImeTable.Rows.Add(subsem, date + " - " + sess, subcode, subname);
                            }

                        }
                        DataView view = dtTImeTable.AsDataView();
                        view.Sort = "Date ASC";

                        int tblRowIndx = 1;
                        int tblColIndx = 0;
                        int tblSNo = 1;
                        int totalSub = view.Count;
                        PdfTable tableTimeTable = mydoc.NewTable(font3bold, totalSub + 1, 5, 5);
                        tableTimeTable.VisibleHeaders = false;
                        tableTimeTable.SetBorders(Color.Black, 1.0, BorderType.CompleteGrid);
                        tableTimeTable.Columns[0].SetWidth(30);
                        tableTimeTable.Columns[1].SetWidth(120);
                        tableTimeTable.Columns[2].SetWidth(100);
                        tableTimeTable.Columns[3].SetWidth(200);
                        tableTimeTable.Columns[4].SetWidth(120);
                        //Headers

                        tableTimeTable.Cell(0, 0).SetContent("Sem");
                        tableTimeTable.Cell(0, 1).SetContent("Date - Session");
                        tableTimeTable.Cell(0, 2).SetContent("Sub.Code");
                        tableTimeTable.Cell(0, 3).SetContent("Subject Title");//
                        tableTimeTable.Cell(0, 4).SetContent("Hall Superintendent  Signature");

                        for (int sn = 0; sn < totalSub; sn++, tblRowIndx++, tblSNo++)
                        {
                            string subcode = Convert.ToString(view[sn]["subcode"]);
                            string subname = Convert.ToString(view[sn]["subject"]);
                            string chckname = subname.Trim().ToLower();
                            string subsem = Convert.ToString(view[sn]["sem"]);
                            string date = Convert.ToString(view[sn]["date"]);
                            string dateSess = date;

                            tableTimeTable.Cell(tblRowIndx, (tblColIndx)).SetContent(subsem);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 1)).SetContent(date);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 1)).SetFont(font4small);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 2)).SetContent(subcode);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 2)).SetFont(font4small);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 3)).SetContent(subname);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 3)).SetContentAlignment(ContentAlignment.TopLeft);
                            tableTimeTable.Cell(tblRowIndx, (tblColIndx + 3)).SetFont(font4small);

                            //tableTimeTable.Cell(tblRowIndx, (tblColIndx+3)).SetContent("INITIALS OF HALL SUPDT.");
                        }
                        coltop = coltop + 50;
                        PdfTablePage addtabletopage1 = tableTimeTable.CreateTablePage(new PdfArea(mydoc, 58, coltop - 5, 480, 300));
                        mypdfpage.Add(addtabletopage1);

                        coltop = coltop + 225;
                        coltop = coltop + 15;
                        PdfArea tetefina;

                        tetefina = new PdfArea(mydoc, 58, coltop + 93, 480, 30);

                        PdfRectangle pr1final = new PdfRectangle(mydoc, tetefina, Color.Black);
                        mypdfpage.Add(pr1final);
                        PdfTextArea ptcsubreg;
                        PdfTextArea ptcfn;
                        PdfTextArea ptcan;

                        ptcsubreg = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 60, coltop + 86, 410, 30), System.Drawing.ContentAlignment.MiddleLeft, "No.of Subject Registered  - " + dvexamappl.Count.ToString());
                        mypdfpage.Add(ptcsubreg);

                        string timequer = "select distinct right(convert(nvarchar(100),ex.start_time,100),7) as start,right(convert(nvarchar(100),ex.end_time,100),7) as end1,ex.exam_session from exmtt e,exmtt_det ex  where ex.start_time<> ex.end_time and e.Exam_month='" + ddlMonth.SelectedValue.ToString() + "' and e.exam_code=ex.exam_code and e.exam_year='" + ddlYear.SelectedItem.Text.ToString() + "' order by start desc";
                        DataSet dstime = d2.select_method_wo_parameter(timequer, "Text");
                        string time = string.Empty;
                        string time1 = string.Empty;
                        dstime.Tables[0].DefaultView.RowFilter = " exam_session='F.N'";
                        DataView dvse = dstime.Tables[0].DefaultView;
                        if (dvse.Count > 0)
                        {
                           // time = "FN - Forenoon " + dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString();
                            ptcfn = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 370, coltop + 86, 410, 30), System.Drawing.ContentAlignment.MiddleLeft, "FN - FORENOON " + dvse[0]["start"].ToString() + " - " + dvse[0]["end1"].ToString()+"");
                            mypdfpage.Add(ptcfn);
                            coltop = coltop + 15;
                        }
                        dstime.Tables[0].DefaultView.RowFilter = " exam_session='A.N'";
                        dvse = dstime.Tables[0].DefaultView;
                        if (dvse.Count > 0)
                        {
                            //time1 = "AN - Afternoon " + dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString();
                            ptcan = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 370, coltop + 86, 410, 30), System.Drawing.ContentAlignment.MiddleLeft, "AN - AFTERNOON " + dvse[dvse.Count - 1]["start"].ToString() + " - " + dvse[dvse.Count - 1]["end1"].ToString()+"");
                            mypdfpage.Add(ptcan);
                        }


                       
                       
                       

                        coltop = 735;

                        //PdfTextArea ptcstisign = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                                    new PdfArea(mydoc, 30, coltop - 5, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Candidate");
                        //mypdfpage.Add(ptcstisign);


                        ////PdfTextArea ptprincname = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        ////                                                   new PdfArea(mydoc, 180, coltop - 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, princ);
                        ////mypdfpage.Add(ptprincname);


                        //PdfTextArea ptcprinc = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 180, coltop - 5, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Signature of the Principal with Seal");
                        //mypdfpage.Add(ptcprinc);


                        ////PdfTextArea ptccoename = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        ////                                                   new PdfArea(mydoc, 380, coltop - 15, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, coename);
                        ////mypdfpage.Add(ptccoename);


                        //PdfTextArea ptccontroller = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 380, coltop - 5, 200, 30), System.Drawing.ContentAlignment.MiddleCenter, "Controller of Examinations");
                        //mypdfpage.Add(ptccontroller);


                        coltop = coltop + 15;

                        //PdfArea tetefinanote = new PdfArea(mydoc, 15, coltop + 10, 565, 65);
                        //PdfRectangle pr1note = new PdfRectangle(mydoc, tetefinanote, Color.Black);
                        //mypdfpage.Add(pr1note);
                        //PdfTextArea ptcsnote = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                    new PdfArea(mydoc, 30, coltop, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Note :");
                        //mypdfpage.Add(ptcsnote);
                        //coltop = coltop + 15;

                        //PdfTextArea ptcsnote1 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "1. If any discrepancy is found in the Hall Ticket,report to the C.O.E office immediately");
                        //mypdfpage.Add(ptcsnote1);

                        //coltop = coltop + 15;

                        //PdfTextArea ptcsnote2 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                   new PdfArea(mydoc, 50, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "2. Verify the dates / sessions mentioned in the time table posted in the college notice board / Website");
                        //mypdfpage.Add(ptcsnote2);
                        //coltop = coltop + 15;
                        //PdfTextArea ptcsnote21 = new PdfTextArea(font3bold, System.Drawing.Color.Black,
                        //                                                 new PdfArea(mydoc, 20, coltop, 500, 30), System.Drawing.ContentAlignment.MiddleLeft, "Printed On: " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                        //mypdfpage.Add(ptcsnote21);


                        //Gios.Pdf.PdfPage mypdfpage1 = mydoc.NewPage();
                        //int coltop1 = 30;
                        //PdfArea tete3 = new PdfArea(mydoc, 15, 30, 565, 250);
                        //Gios.Pdf.PdfRectangle pr3 = new Gios.Pdf.PdfRectangle(mydoc, tete3, Color.Black);
                        //mypdfpage1.Add(pr3);



                        //PdfTextArea ptcB1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 50, coltop1, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "INSTRUCTIONS TO THE CANDIDATE");
                        //mypdfpage1.Add(ptcB1);
                        //coltop1 += 2;
                        //PdfTextArea ptcB2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 50, coltop1, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "_______________________________");
                        //mypdfpage1.Add(ptcB2);
                        //coltop1 += 20;
                        //coltop += 20;
                        //string newline = "1.\tCandidates must sign the Hall Ticket when it is issued. They must bring the Hall Ticket and Identity Card to every examination and\n\t\t\t produce the same on demand.";
                        //PdfTextArea ptcB3 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB3);
                        //coltop1 += 20;
                        //newline = "2.\tCandidates shall not be permitted to enter the examination hall after the expiry of 30 minutes from the commencement of examination.";
                        //PdfTextArea ptcB4 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB4);
                        //coltop1 += 20;
                        //newline = "3.\tCandidates shall not be allowed to leave the examination hall before the expiry of 45 minutes from the commencement of examination.";
                        //PdfTextArea ptcB5 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                             new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB5);
                        //coltop1 += 20;
                        //newline = "4.\tIf the candidates are found in possession of any written and printed material, cell phone and programmable calculator in the\n\t\t\t examination hall shall be liable for disciplinary action.";
                        //PdfTextArea ptcB6 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                         new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB6);
                        //coltop1 += 20;
                        //newline = "5.\tCandidates indulging in Malpractice of any kind will be severely dealt with.";
                        //PdfTextArea ptcB7 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                       new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB7);
                        //coltop1 += 20;
                        //newline = "6.\tCandidate's seating arrangement is in rotation.";
                        //PdfTextArea ptcB8 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                     new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB8);
                        //coltop1 += 2;
                        //PdfTextArea ptcB9 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                     new PdfArea(mydoc, 25, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "\t\t______________________________________");
                        //mypdfpage1.Add(ptcB9);
                        //coltop1 += 20;
                        //PdfTextArea ptcB10 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 50, coltop1, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "PUNISHMENT FOR MALPRACTICE");
                        //mypdfpage1.Add(ptcB10);
                        //coltop1 += 2;
                        //PdfTextArea ptcB11 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                        //                                              new PdfArea(mydoc, 50, coltop1, 525, 30), System.Drawing.ContentAlignment.MiddleCenter, "_____________________________");
                        //mypdfpage1.Add(ptcB11);
                        //coltop1 += 20;
                        //newline = @"i)\tPossession of Hand written\\ Xerox material\\ Mobile phone - Particular paper will be cancelled.";
                        //PdfTextArea ptcB12 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                      new PdfArea(mydoc, 70, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB12);
                        //coltop1 += 20;
                        //newline = @"ii)\tWriting the examination by copying Hand written\\ Xerox material\\ Mobile phone - Particular and Subsequent papers will be\n\t\t\t cancelled.";
                        //PdfTextArea ptcB13 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydoc, 70, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB13);
                        //coltop1 += 20;
                        //newline = @"iii)\tFor Repeating the Malpractice - All the papers of the semester will be cancelled.";
                        //PdfTextArea ptcB14 = new PdfTextArea(font3small, System.Drawing.Color.Black,
                        //                                   new PdfArea(mydoc, 70, coltop1, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, newline);
                        //mypdfpage1.Add(ptcB14);
                        //coltop1 += 20;

                        mypdfpage.SaveToDocument();
                      //  mypdfpage1.SaveToDocument();


                    }
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                Response.Buffer = true;
                Response.Clear();
                string szPath = appPath + "/Report/";
                string szFile = "HALLTICKET" + DateTime.Now.ToString("ddMMyyyyHHmmsstt") + ".pdf";
                mydoc.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = true;
        }
    }


}