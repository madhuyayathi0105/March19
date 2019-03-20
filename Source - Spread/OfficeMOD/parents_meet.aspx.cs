using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;

public partial class parents_meet : System.Web.UI.Page
{
    string usercode = "";

    string singleuser = "";
    string group_user = "";
    string collegecode = "";
    string srisql = "";
    string srisql1 = "";
    string srisql2 = "";
    int sno = 0;
    Boolean Cellclick = false;
    Boolean checkvisited = false;
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    Hashtable hat = new Hashtable();
    DAccess2 da = new DAccess2();
    DataSet temp = new DataSet();
    DataSet temp1 = new DataSet();
    DataSet temp2 = new DataSet();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    FarPoint.Web.Spread.CheckBoxCellType cbct = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType cbct1 = new FarPoint.Web.Spread.CheckBoxCellType();
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
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

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode = Session["Collegecode"].ToString();
        if (!IsPostBack)
        {
            rbvisited.Checked = true;
            tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyy");
            tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");
            tbstart_date.Attributes.Add("ReadOnly", "ReadOnly");
            txtstrdate.Attributes.Add("ReadOnly", "ReadOnly");
            tbend_date.Attributes.Add("ReadOnly", "ReadOnly");
            txtrepodate.Text = DateTime.Now.ToString("dd/MM/yyy");
            txtstrdate.Text = DateTime.Now.ToString("dd/MM/yyy");
            txtrepodate.Attributes.Add("ReadOnly", "ReadOnly");
            txtstaff.Attributes.Add("ReadOnly", "ReadOnly");
            ddlstaff.Visible = false;
            txt_search.Visible = false;
            lblsearchby.Visible = false;

            pnlmsgboxupdate1.Visible = false;
            Fpspread.Sheets[0].RowCount = 0;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            //  Fpspread.Sheets[0].AutoPostBack = false;
            Fpspread.CommandBar.Visible = false;
            Fpspread.Visible = false;
            btnmeet.Visible = false;

            Fpspread.Sheets[0].ColumnCount = 9;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "SMS Date";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch-Degree-Department-Semester";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Purpose";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;

            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Staff Name";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;


            Fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Attendance Remarks";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;

            Fpspread.Sheets[0].Columns[0].Width = 40;
            Fpspread.Sheets[0].Columns[1].Width = 50;
            Fpspread.Sheets[0].Columns[2].Width = 80;
            Fpspread.Sheets[0].Columns[3].Width = 120;
            Fpspread.Sheets[0].Columns[4].Width = 80;
            Fpspread.Sheets[0].Columns[5].Width = 100;
            Fpspread.Sheets[0].Columns[6].Width = 170;
            Fpspread.Sheets[0].Columns[7].Width = 120;
            Fpspread.Sheets[0].Columns[8].Width = 180;
            Fpspread.Width = 960;
            Fpspread.Sheets[0].GridLineColor = Color.Black;


            Fpspread.Sheets[0].Columns[0].Locked = true;
            for (int k = 2; k < 9; k++)
            {
                Fpspread.Sheets[0].Columns[k].Locked = true;
            }

            for (int ix = 0; ix < 8; ix++)
            {
                Fpspread.Sheets[0].Columns[ix].Font.Size = FontUnit.Medium;
                Fpspread.Sheets[0].Columns[ix].Font.Name = "Book Antiqua";
                Fpspread.Sheets[0].Columns[ix].HorizontalAlign = HorizontalAlign.Center;
                Fpspread.Sheets[0].Columns[ix].Font.Bold = true;
            }

            fsstaff.Sheets[0].AutoPostBack = true;
            fsstaff.CommandBar.Visible = false;
            FarPoint.Web.Spread.StyleInfo styles = new FarPoint.Web.Spread.StyleInfo();
            styles.Font.Size = 10;
            styles.Font.Bold = true;
            fsstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(styles);
            fsstaff.Sheets[0].AllowTableCorner = true;
            fsstaff.Sheets[0].RowHeader.Visible = false;

            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;

            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            fsstaff.Sheets[0].DefaultColumnWidth = 50;
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            fsstaff.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;

            fsstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            fsstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            fsstaff.Sheets[0].DefaultStyle.Font.Bold = false;
            fsstaff.SheetCorner.Cells[0, 0].Font.Bold = true;

            fsstaff.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            fsstaff.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;

            //fsstaff.Sheets[0].AutoPostBack = true;
            fsstaff.Sheets[0].ColumnCount = 3;
            fsstaff.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fsstaff.Sheets[0].ColumnHeader.Columns[1].Label = "Staff Name";
            fsstaff.Sheets[0].ColumnHeader.Columns[2].Label = "Staff Code";

            fsstaff.Sheets[0].Columns[0].Width = 80;
            fsstaff.Sheets[0].Columns[1].Width = 300;
            fsstaff.Sheets[0].Columns[2].Width = 100;

            fsstaff.Sheets[0].Columns[0].Locked = true;
            fsstaff.Sheets[0].Columns[1].Locked = true;
            fsstaff.Sheets[0].Columns[2].Locked = true;

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

    int rowcounts = 0;
    protected void btngo_click(object sender, EventArgs e)
    {
        temp1.Clear();

        srisql = " select registration.stud_name, parents_meet.staff_name,parents_meet.staff_code,att_remark,visited,parents_meet.roll_no, send_date,purpose,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details from parents_meet,registration,Degree,course  where registration.roll_no =parents_meet.roll_no and degree.course_id=course.course_id and degree.Degree_code=registration.Degree_code and registration.delflag=0 and registration.exam_flag<>'DEBAR' and registration.cc=0";
        temp1 = da.select_method_wo_parameter(srisql, "Text");
        Fpspread.Sheets[0].RowCount = 0;

        Fpspread.Sheets[0].RowCount++;
        Fpspread.Visible = true;
        btnmeet.Visible = true;
        bind_spread();
        if (rowcounts == 1)
        {
            lblerroe.Text = "No Records Found";
            lblerroe.Visible = true;
            btnmeet.Visible = false;
            Fpspread.Visible = false;
        }
        else
        {
            lblerroe.Visible = false;
            Fpspread.Visible = true;
        }

    }

    public void bind_spread()
    {


        for (int i = 0; i < temp1.Tables[0].Rows.Count; i++)
        {

            Fpspread.Sheets[0].RowCount++;
            Fpspread.Sheets[0].Cells[0, 1].CellType = cbct1;
            cbct1.AutoPostBack = true;

            Fpspread.Sheets[0].SpanModel.Add(0, 2, 1, 5);
            Fpspread.Sheets[0].Cells[0, 1].Text = "";
            Fpspread.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;

            cbct.AutoPostBack = false;

            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].CellType = cbct;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = temp1.Tables[0].Rows[i]["stud_name"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Tag = temp1.Tables[0].Rows[i]["roll_no"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].Text = temp1.Tables[0].Rows[i]["roll_no"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].Tag = temp1.Tables[0].Rows[i]["send_date"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;


            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].Text = temp1.Tables[0].Rows[i]["send_date"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].CellType = txt;


            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].Text = temp1.Tables[0].Rows[i]["details"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].CellType = txt;

            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 6].Text = temp1.Tables[0].Rows[i]["purpose"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 6].CellType = txt;

            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].Text = temp1.Tables[0].Rows[i]["staff_name"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 7].CellType = txt;

            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].Text = temp1.Tables[0].Rows[i]["att_remark"].ToString();
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].CellType = txt;


            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 8].Font.Bold = true;

            Fpspread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpspread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            Fpspread.Visible = true;



        }
        rowcounts = Fpspread.Sheets[0].RowCount;
        for (int k = 1; k < Fpspread.Sheets[0].RowCount; k++)
        {


            string rno = Convert.ToString(Fpspread.Sheets[0].Cells[k, 3].Tag);
            if (rno != "")
            {
                if (rbvisited.Checked == false)
                {
                    if (temp1.Tables[0].Rows[k - 1]["visited"].ToString() == "" || temp1.Tables[0].Rows[k - 1]["visited"].ToString() == null)
                    {
                        sno++;
                        Fpspread.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                        Fpspread.Sheets[0].Rows[k].Visible = true;

                    }
                    else
                    {
                        Fpspread.Sheets[0].Rows[k].Visible = false;
                        rowcounts--;
                    }


                }
                if (rbvisited.Checked == true)
                {
                    if (temp1.Tables[0].Rows[k - 1]["visited"].ToString() == "Yes" || temp1.Tables[0].Rows[k - 1]["visited"].ToString() != "")
                    {
                        sno++;
                        Fpspread.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                    }
                    else
                    {
                        Fpspread.Sheets[0].Rows[k].Visible = false;
                        rowcounts--;
                    }


                }









            }


        }

        int rowcount2 = Fpspread.Sheets[0].RowCount;


        Fpspread.Sheets[0].PageSize = 25 + (rowcount2 * 20);

    }
    protected void btnpointsadd_Click(object sender, EventArgs e)
    {
        panelref.Visible = true;
        //panelref.Attributes.Add("style", "width:200px; height:80px; top:762px; left:791px; position: absolute;");
        capref.InnerHtml = "Points";
        panelref.Visible = true;
        txtfoc.Focus();
    }
    protected void btnpointsremove_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlpoints.Items.Count > 0)
            {
                string collegecode = Session["collegecode"].ToString();
                string reason = ddlpoints.SelectedItem.ToString();
                if (reason.Trim().ToLower() != "all" && reason.Trim() != "")
                {
                    string strquery = "delete textvaltable where TextVal='" + reason + "' and TextCriteria='pont' and college_code='" + collegecode + "'";
                    int a = da.update_method_wo_parameter(strquery, "Text");
                    bindrefpoints();
                }
            }
            panelref.Visible = false;

        }
        catch
        {
        }
    }
    protected void btnaddref_Click(object sender, EventArgs e)
    {

        srisql1 = " if not exists (select * from textvaltable where TextVal='" + txt_ref.Text + "' and Textcriteria='pont' and college_code='" + collegecode + "')  begin  insert into textvaltable values ('" + txt_ref.Text + "','pont','" + collegecode + "') end";

        hat.Clear();
        int a = da.insert_method(srisql1, hat, "Text");

        //if (a != 0)
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

        //}
        //else
        //{
        //    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Already Exists Successfully')", true);
        //}
        bindrefpoints();
        panelref.Visible = false;
    }
    protected void btnexitref_Click(object sender, EventArgs e)
    {
        panelref.Visible = false;
        txtfoc.Focus();
    }
    protected void f_CheckedChanged(object sender, EventArgs e)
    {

        if (txt_amt.Visible == true)
        {
            txt_amt.Visible = false;
            lblmessage.Visible = false;
            // txtfoc.Focus();
        }
        else
        {
            txt_amt.Visible = true;
            lblmessage.Visible = false;
            // txtfoc.Focus();
        }
    }
    protected void s_CheckedChanged(object sender, EventArgs e)
    {
        if (s.Checked == true)
        {
            lblstartdt.Visible = true;
            txtstrdate.Visible = true;
            lblsusdays.Visible = true;
            txtsusdays.Visible = true;
        }
        else
        {
            lblstartdt.Visible = false;
            txtstrdate.Visible = false;
            lblsusdays.Visible = false;
            txtsusdays.Visible = false;
        }

    }

    protected void btnmeet_Click(object sender, EventArgs e)
    {
        int count_check = 0;
        txtstaff.Text = "";
        txtsusdays.Text = "";
        txt_amt.Text = "";
        Fpspread.SaveChanges();
        for (int j = 1; j < Fpspread.Sheets[0].RowCount; j++)
        {

            if (Convert.ToInt32(Fpspread.Sheets[0].Cells[j, 1].Value) == 1)
            {
                count_check++;
            }
        }
        if (count_check == 0)
        {
            lblerroe.Text = "Please Select Any Student";
            lblerroe.Visible = true;
            btnmeet.Visible = true;
            return;

        }
        lblerroe.Visible = false;
        ddlpoints.Attributes.Add("onfocus", "rcity()");
        //mpemsgboxupdate.Show();
        Fpspread.SaveChanges();
        bindrefpoints();
        txtfoc.Focus();
        pnlmsgboxupdate1.Visible = true;

    }
    public void bindrefpoints()
    {
        srisql = "select * from textvaltable where textcriteria = 'pont'";
        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");
        ddlpoints.DataSource = ds;
        ddlpoints.DataTextField = "TextVal";
        ddlpoints.DataBind();

        // ddlpoints.Items.Insert(0, "All");
    }

    protected void Fpspread1_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(Fpspread.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < Fpspread.Sheets[0].RowCount; i++)
            {
                Fpspread.Sheets[0].Cells[i, 1].Value = 1;
                //btncheckadd.Focus();

                //FpSpreadcheck.SaveChanges();


            }

        }

        else if (Convert.ToInt32(Fpspread.Sheets[0].Cells[0, 1].Value) == 0)
        {
            for (int i = 0; i < Fpspread.Sheets[0].RowCount; i++)
            {
                Fpspread.Sheets[0].Cells[i, 1].Value = 0;
                // btncheckadd.Focus();
                //FpSpreadcheck.SaveChanges();

            }

        }





    }
    protected void Fpspread1_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;

    }
    protected void Fpspread1_PreRender(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {


            string activerow = Fpspread.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(activerow.ToString()) >= 0)
            {
            }



        }
    }

    protected void tbstart_date_OnTextChanged(object sender, EventArgs e)
    {


        try
        {
            DateTime dtnow = DateTime.Now;
            lblerroe.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = tbstart_date.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 > dtnow)
                {
                    lblerroe.Visible = false;
                    lblerroe.Text = "Please Enter Valid From date";
                    lblerroe.Visible = true;
                    tbstart_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                    Fpspread.Visible = false;



                }

            }




            else if (tbend_date.Text == "")
            {
                lblerroe.Visible = false;
                lblerroe.Text = "Please Enter to date";
                lblerroe.Visible = true;
                Fpspread.Visible = false;

            }
        }
        catch
        {

        }

    }

    protected void txtstrdate_OnTextChanged(object sender, EventArgs e)
    {


        try
        {
            DateTime dtnow = DateTime.Now;
            lblerroe.Visible = false;
            string datefad, dtfromad;
            string datefromad;
            string yr4, m4, d4;
            datefad = txtstrdate.Text.ToString();
            string[] split4 = datefad.Split(new Char[] { '/' });
            if (split4.Length == 3)
            {
                datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                yr4 = split4[2].ToString();
                m4 = split4[1].ToString();
                d4 = split4[0].ToString();
                dtfromad = m4 + "/" + d4 + "/" + yr4;
                DateTime dt1 = Convert.ToDateTime(dtfromad);
                if (dt1 < dtnow)
                {
                    //lblerroe.Visible = false;
                    //lblerroe.Text = "Please Enter Valid From date";
                    //lblerroe.Visible = true;
                    txtstrdate.Text = DateTime.Now.ToString("dd/MM/yyy");
                    //Fpspread.Visible = false;
                    lblmessage.Text = "Please Enter Valid From date";
                    lblmessage.Visible = true;



                }

            }




            else if (tbend_date.Text == "")
            {
                lblerroe.Visible = false;
                lblerroe.Text = "Please Enter to date";
                lblerroe.Visible = true;
                Fpspread.Visible = false;

            }
        }
        catch
        {

        }

    }
    protected void tbend_date_OnTextChanged(object sender, EventArgs e)
    {
        try
        {

            DateTime dtnow1 = DateTime.Now;
            string date2ad;
            string datetoad;
            string yr5, m5, d5;
            date2ad = tbend_date.Text.ToString();
            string[] split5 = date2ad.Split(new Char[] { '/' });



            if (split5.Length == 3)
            {
                datetoad = split5[0].ToString() + "/" + split5[1].ToString() + "/" + split5[2].ToString();
                yr5 = split5[2].ToString();
                m5 = split5[1].ToString();
                d5 = split5[0].ToString();
                datetoad = m5 + "/" + d5 + "/" + yr5;
                DateTime dt2 = Convert.ToDateTime(datetoad);

                if (dt2 > dtnow1)
                {
                    lblerroe.Visible = false;
                    lblerroe.Text = "Please Enter Valid To Date";
                    lblerroe.Visible = true;
                    btnmeet.Visible = false;
                    tbend_date.Text = DateTime.Now.ToString("dd/MM/yyy");
                    Fpspread.Visible = false;

                    goto label1;

                }
                else
                {
                    lblerroe.Visible = false;
                    Fpspread.Visible = true;
                    btnmeet.Visible = true;

                }
            }






            if (tbstart_date.Text != "" && tbend_date.Text != "")
            {
                lblerroe.Visible = false;
                string datefad, dtfromad;
                string datefromad;
                string yr4, m4, d4;
                datefad = tbstart_date.Text.ToString();
                string[] split4 = datefad.Split(new Char[] { '/' });
                if (split4.Length == 3)
                {
                    datefromad = split4[0].ToString() + "/" + split4[1].ToString() + "/" + split4[2].ToString();
                    yr4 = split4[2].ToString();
                    m4 = split4[1].ToString();
                    d4 = split4[0].ToString();
                    dtfromad = m4 + "/" + d4 + "/" + yr4;


                    string adatetoad;
                    string ayr5, am5, ad5;
                    date2ad = tbend_date.Text.ToString();
                    string[] asplit5 = date2ad.Split(new Char[] { '/' });
                    if (split5.Length == 3)
                    {
                        adatetoad = asplit5[0].ToString() + "/" + asplit5[1].ToString() + "/" + asplit5[2].ToString();
                        ayr5 = asplit5[2].ToString();
                        am5 = asplit5[1].ToString();
                        ad5 = asplit5[0].ToString();
                        adatetoad = am5 + "/" + ad5 + "/" + ayr5;
                        DateTime dt1 = Convert.ToDateTime(dtfromad);
                        DateTime dt2 = Convert.ToDateTime(adatetoad);

                        TimeSpan ts = dt2 - dt1;

                        int days = ts.Days;
                        if (days < 0)
                        {
                            lblerroe.Text = "From Date Can't Be Greater Than To Date";
                            tbend_date.Text = "";
                            tbstart_date.Text = "";
                            lblerroe.Visible = true;
                            Fpspread.Visible = false;

                        }
                    }
                }

            }

        label1: ;
        }
        catch
        {

        }



    }
    protected void btnstaff_click(object sender, EventArgs e)
    {
        pnlmsgboxupdate1.Visible = false;
        panel8.Visible = true;

        fsstaff.Visible = true;
        btnstaffadd.Text = "Ok";
        fsstaff.Sheets[0].RowCount = 0;
        BindCollege();
        loadstaffdep(collegecode);

        bind_stafType();
        bind_design();
        loadfsstaff();


    }

    protected void fsstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
        string activecol = fsstaff.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        // mpedirect.Show();
        panel8.Visible = true;

    }


    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
        //mpedirect.Show();
        panel8.Visible = true;
    }
    protected void ddl_stftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        //loadfsstaff();

        bind_design();
        loadfsstaff();
        //mpedirect.Show();
        panel8.Visible = true;
    }
    protected void ddl_design_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
        //mpedirect.Show();
        panel8.Visible = true;
        //bind_design();

    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;
        //loadfsstaff();
    }
    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        fsstaff.Sheets[0].RowCount = 0;

        loadfsstaff();
    }
    protected void fsstaff_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }

    protected void btnstaffadd_Click(object sender, EventArgs e)
    {
        string activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
        if (Convert.ToInt32(activerow.ToString()) > 0)
        {

            string name_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string des_active = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            txtstaff.Text = name_active.ToString();

            txtstaff_co.Text = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
        }
        panel8.Visible = false;
        lblmessage.Visible = false;
        pnlmsgboxupdate1.Visible = true;

    }
    protected void exitpop_Click(object sender, EventArgs e)
    {
        panel8.Visible = false;
    }


    void BindCollege()
    {
        srisql = "select collname,college_code from collinfo";
        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");


        ddlcollege.DataSource = ds;
        ddlcollege.DataTextField = "collname";
        ddlcollege.DataValueField = "college_code";
        ddlcollege.DataBind();
    }
    void loadstaffdep(string collegecode)
    {

        srisql = "select distinct dept_name,dept_code from hrdept_master where college_code=" + Session["collegecode"] + "";

        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");
        ddldepratstaff.DataSource = ds;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        ddldepratstaff.Items.Insert(0, "All");
    }
    void bind_stafType()
    {

        srisql = "SELECT DISTINCT StfType FROM StaffTrans T,HrDept_Master D WHERE T.Dept_Code = D.Dept_Code AND T.Latestrec = 1 and d.college_code=" + Session["collegecode"] + "";
        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_stftype.DataSource = ds;
            ddl_stftype.DataTextField = "StfType";
            ddl_stftype.DataValueField = "StfType";
            ddl_stftype.DataBind();
            ddl_stftype.Items.Insert(0, "All");
        }
    }
    void bind_design()
    {
        string sql = string.Empty;

        if (ddl_stftype.SelectedItem.ToString() == "All")
        {
            sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + "";
        }
        else
        {
            sql = "SELECT distinct Desig_Name FROM StaffTrans T,staffmaster m,Desig_Master G WHERE t.staff_code = m.staff_code and T.Desig_Code = G.Desig_Code AND Latestrec = 1 and G.collegecode=" + Session["collegecode"] + " and stftype='" + ddl_stftype.Text + "'";
        }

        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {

            ddl_design.DataSource = ds;
            ddl_design.DataTextField = "Desig_Name";
            ddl_design.DataValueField = "Desig_Name";
            ddl_design.DataBind();
            ddl_design.Items.Insert(0, "All");

        }
    }
    protected void loadfsstaff()
    {
        string sql = "";
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";
                }
            }
            else
            {
                //sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_name = '" + ddldepratstaff.Text + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "' and (staffmaster.college_code =hrdept_master.college_code)";
                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)";

            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code)";
            }
            else if (ddlcollege.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";
            }

            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0";

            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString() == "All")
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege.SelectedValue + "'";

            }
        fsstaff.Sheets[0].RowCount = 0;
        fsstaff.SaveChanges();

        FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();

        fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
        fsstaff.Sheets[0].SpanModel.Add(fsstaff.Sheets[0].RowCount - 1, 0, 1, 3);
        //fsstaff.Sheets[0].AutoPostBack = false;
        string bindspread = sql;

        string design_name = string.Empty;
        string dept_all = string.Empty;
        string design_all = string.Empty;

        if (ddl_design.Items.Count > 0)
        {
            design_name = ddl_design.SelectedItem.ToString();

        }

        for (int cnt = 1; cnt < ddldepratstaff.Items.Count; cnt++)
        {
            if (dept_all == "")
            {
                dept_all = ddldepratstaff.Items[cnt].Value;
            }
            else
            {
                dept_all = dept_all + "','" + ddldepratstaff.Items[cnt].Value;
            }

        }

        for (int cnt = 1; cnt < ddl_design.Items.Count; cnt++)
        {
            if (dept_all == "")
            {
                design_all = ddl_design.Items[cnt].Value;
            }
            else
            {
                design_all = design_all + "','" + ddl_design.Items[cnt].Value;
            }
        }

        string Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name='" + design_name + "' and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";

        if (ddldepratstaff.SelectedItem.ToString() == "All" && ddl_design.SelectedItem.ToString() == "All")
        {
            Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code  and h.dept_code in ('" + dept_all + "') and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
        }
        else if (ddldepratstaff.SelectedItem.ToString() == "All")
        {
            Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code in ('" + dept_all + "') and d.desig_name='" + design_name + "' and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
        }
        else if (ddl_design.SelectedItem.ToString() == "All")
        {

            Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
        }

        if (ddl_stftype.SelectedItem.ToString() != "All")
        {
            Sql_Query = "select distinct s.staff_code,s.staff_name from staffmaster s,hrdept_master h,desig_master d,stafftrans st where s.staff_code=st.staff_code and st.Dept_Code = h.Dept_Code and d.desig_code=st.desig_code and s.college_code = h.college_code and s.college_code = d.collegecode and h.dept_code = '" + ddldepratstaff.SelectedValue.ToString() + "' and d.desig_name in ('" + design_all + "') and s.college_code='" + ddlcollege.SelectedValue.ToString() + "' and stftype = '" + ddl_stftype.SelectedItem.ToString() + "' and resign = 0 and settled = 0 and latestrec=1";
        }


        DataSet dsbindspread = new DataSet();
        dsbindspread.Clear();
        dsbindspread = da.select_method_wo_parameter(Sql_Query, "Text");

        //mpedirect.Show();
        panel8.Visible = true;

        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();


                fsstaff.Sheets[0].RowCount = fsstaff.Sheets[0].RowCount + 1;
                fsstaff.Sheets[0].Rows[fsstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].Text = name;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].Text = code;
                fsstaff.Sheets[0].Cells[fsstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                fsstaff.Sheets[0].AutoPostBack = false;
            }
            int rowcount = fsstaff.Sheets[0].RowCount;

            fsstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            fsstaff.SaveChanges();
        }
    }




    protected void btncncl_click(object sender, EventArgs e)
    {

        //txtfoc.Focus();
        pnlmsgboxupdate1.Visible = false;


    }


    int a = 0;
    protected void btnok_click(object sender, EventArgs e)
    {
        try
        {
            txtfoc.Focus();
            string roll_no_d1 = "", send_date_d2 = "";
            if (txtstaff.Text.Trim() == "")
            {
                lblmessage.Text = "Please Select The Staff";
                lblmessage.Visible = true;
                return;
            }
            if (s.Checked == true)
            {

                if (txtsusdays.Text.Trim() == "")
                {
                    lblmessage.Text = "Please Enter The Days";
                    lblmessage.Visible = true;
                    return;

                }
                else
                {
                    lblmessage.Visible = false;
                }

            }

            string staff_code_d1 = "", staff_name_d2 = "", visited_d3 = "", date_d4 = "", fpoint_data_d5 = "", action = "", ponts = "", remarks = "";
            pnlmsgboxupdate1.Visible = false;
            date_d4 = txtrepodate.Text.ToString();
            string[] ddate = date_d4.Split('/');
            date_d4 = ddate[1] + "/" + ddate[0] + "/" + ddate[2];
            //if (ddlpoints.SelectedIndex.ToString() == "0")
            //{

            //    for (int i = 0; i < ddlpoints.Items.Count; i++)
            //    {
            //        if (ponts == "")
            //        {
            //            ponts = ddlpoints.Items[i].Text;
            //        }
            //        else
            //        {
            //            ponts = ponts +","+ ddlpoints.Items[i].Text;
            //        }
            //    }

            //}
            //else
            //{
            ponts = ddlpoints.SelectedItem.Text;

            // }

            if (d.Checked == true)
            {
                if (action == "")
                {
                    action = "Dismissal";
                }
                else
                {
                    action = action + ";" + "Dismissal";
                }


            }
            if (w.Checked == true)
            {
                if (action == "")
                {
                    action = "Warning";
                }
                else
                {
                    action = action + ";" + "Warning";
                }

            }
            if (s.Checked == true)
            {
                if (action == "")
                {
                    action = " Suspension=" + txtsusdays.Text.ToString() + " Days";
                }
                else
                {
                    action = action + ";" + " Suspension=" + txtsusdays.Text.ToString() + " Days";
                }

            }

            if (f.Checked == true)
            {
                if (txt_amt.Text.Trim() != "")
                {
                    if (action == "")
                    {
                        action = "Fine=Rs." + txt_amt.Text + "";
                    }
                    else
                    {
                        action = action + ";" + "Fine=Rs." + txt_amt.Text + "";
                    }
                }
                else
                {
                    lblmessage.Text = "Please Enter The Amount";
                    lblmessage.Visible = true;
                    pnlmsgboxupdate1.Visible = true;
                    btnok.Focus();
                    return;
                }

            }

            if (action == "")
            {
                lblmessage.Text = "Please Check The Action";
                lblmessage.Visible = true;
                pnlmsgboxupdate1.Visible = true;
                return;
                //fpoint_data_d5 = txtrepodate.Text.ToString() + " ;" + ponts;
            }
            else
            {
                fpoint_data_d5 = txtrepodate.Text.ToString() + "; " + ponts + "; " + action;
                lblmessage.Visible = false;
            }
            // remarks = date_d4 + ";" + ponts + ";" + action;

            for (int j = 1; j < Fpspread.Sheets[0].RowCount; j++)
            {

                if (Convert.ToInt32(Fpspread.Sheets[0].Cells[j, 1].Value) == 1)
                {
                    //Fpspread.Sheets[0].Cells[j, 4].Text = ddlissu.SelectedItem.ToString();
                    //Fpspread.Sheets[0].Cells[j, 5].Text = txtissuedate.Text.ToString();
                    roll_no_d1 = Fpspread.Sheets[0].Cells[j, 3].Tag.ToString();
                    send_date_d2 = Fpspread.Sheets[0].Cells[j, 2].Tag.ToString();
                    string ddtdate = send_date_d2;
                    string[] ddate11 = send_date_d2.Split('/');
                    send_date_d2 = ddate11[1] + "/" + ddate11[0] + "/" + ddate11[2];
                    Fpspread.Sheets[0].Cells[j, 7].Text = txtstaff.Text.ToString();
                    Fpspread.Sheets[0].Cells[j, 7].Tag = txtstaff_co.Text;
                    Fpspread.Sheets[0].Cells[j, 8].Text = fpoint_data_d5;
                    staff_code_d1 = txtstaff_co.Text;
                    staff_name_d2 = txtstaff.Text.ToString();
                    visited_d3 = "yes";

                    binddata(roll_no_d1, ddtdate, staff_code_d1, staff_name_d2, visited_d3, fpoint_data_d5);

                    if (s.Checked == true)
                    {
                        srisql = " select r.Current_Semester,r.degree_code from parents_meet pm,Registration r where r.Roll_No=pm.Roll_No and pm.Roll_No='" + roll_no_d1 + "'";
                        string txtddlbrachadd = "";
                        string txtddlsemadd = "";
                        DataSet ds123 = new DataSet();
                        ds123.Clear();
                        ds123 = da.select_method_wo_parameter(srisql, "Text");
                        txtddlsemadd = ds123.Tables[0].Rows[0][0].ToString();
                        txtddlbrachadd = ds123.Tables[0].Rows[0][1].ToString();

                        string[] ddate12 = txtstrdate.Text.Split('/');


                        int splitdate = Convert.ToInt32(ddate12[0]);
                        int splitmonth = Convert.ToInt32(ddate12[1]);
                        int splityear = Convert.ToInt32(ddate12[2]);
                        int monthyear = splityear * 12 + splitmonth;
                        string curr_date = splitmonth + "-" + splitdate + "-" + splityear;
                        string noofhrs = da.GetFunction("select No_of_hrs_per_day from PeriodAttndSchedule where degree_code=" + txtddlbrachadd + " and semester=" + txtddlsemadd + "");
                        if (noofhrs.Trim() != "" && noofhrs != "0" && noofhrs != null)
                        {
                            DateTime datesus = Convert.ToDateTime(curr_date);
                            int day = Convert.ToInt32(txtsusdays.Text);
                            string datecolumn = "";
                            string attvalue = "";
                            string dateattvalue = "";
                            for (int date = 0; date < day; date++)
                            {
                                datesus = datesus.AddDays(date);
                                string dateva = datesus.Day.ToString();
                                string queryholi = "select * from holidayStudents where degree_code=" + txtddlbrachadd + " and semester=" + txtddlsemadd + " and holiday_date='" + datesus.ToString() + "'";
                                DataSet dsholiday = da.select_method(queryholi, hat, "Text");
                                if (dsholiday.Tables[0].Rows.Count == 0)
                                {
                                    for (int i = 1; i <= Convert.ToInt32(noofhrs); i++)
                                    {
                                        if (datecolumn == "")
                                        {
                                            datecolumn = "d" + dateva + "d" + i + "";
                                            attvalue = "9";
                                            dateattvalue = "d" + dateva + "d" + i + "=9";
                                        }
                                        else
                                        {
                                            datecolumn = "" + datecolumn + "," + "d" + dateva + "d" + i + "";
                                            attvalue = attvalue + ',' + "9";
                                            dateattvalue = dateattvalue + ',' + "d" + dateva + "d" + i + "=9";
                                        }
                                    }
                                }
                            }

                            string strmonthyear = da.GetFunction("Select month_year from attendance where roll_no='" + roll_no_d1 + "' and month_year=" + monthyear + "");
                            if (strmonthyear != "0" && strmonthyear != null && strmonthyear.Trim() != "")
                            {
                                string insquery = "update attendance set " + dateattvalue + " where roll_no='" + roll_no_d1 + "' and month_year=" + monthyear + "";
                                int bb = da.update_method_wo_parameter(insquery, "Text");
                            }
                            else
                            {
                                string insquery = "insert into attendance(roll_no,month_year," + datecolumn + ") values('" + roll_no_d1 + "'," + monthyear + "," + attvalue + ")";
                                int bb = da.update_method_wo_parameter(insquery, "Text");
                            }
                        }

                    }








                }


            }
            if (a != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

            }
        }
        catch
        {
        }
    }
    public void binddata(string roll_no_d1, string send_date_d2, string stff_code, string stff_name, string visited, string remarks)
    {

        //string dd = dt.Date;
        string srisql = "  if exists(select * from parents_meet where  Roll_No='" + roll_no_d1 + "' and send_date='" + send_date_d2 + "')  begin update parents_meet set att_remark='" + remarks + "',staff_code='" + stff_code + "',staff_name='" + stff_name + "',visited='" + visited + "'   where  Roll_No='" + roll_no_d1 + "' and send_date='" + send_date_d2 + "'   end ";

        hat.Clear();
        a = da.insert_method(srisql, hat, "Text");


    }

}