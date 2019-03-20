using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.Configuration;


public partial class bonafidekongu : System.Web.UI.Page
{
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string user_code = "";
    string college_code = "";
    string collegecode = "";
    string query = "";
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hat = new Hashtable();
    string sectionvalue = "";
    Boolean flag_true = false;
    Boolean saveflag = false;
    Boolean resultflag = false;
    string[] data;
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
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
        lblerr.Visible = false;
        collegecode = Session["Collegecode"].ToString();
        //ddlreason.Enabled = false;
        if (!IsPostBack)
        {

            // btnaddr.Visible = false;
            // btndelr.Visible = false;
            ddlreason.Attributes.Add("onfocus", "flg()");
            reason();
            btngenerate.Visible = false;
           Panel9.Visible = false;
            Fpspread.Visible = false;
            bindclg();
            bindbatch();
            binddegree();
            bindbranch();
            bindsem();
            BindSectionDetail();

        }
    }
    protected void logout_btn_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    public void bindclg()
    {
        try
        {
            ddlclg.Items.Clear();
            hat.Clear();
            user_code = Session["usercode"].ToString();
            college_code = ddlclg.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string clgname = "select college_code,collname from collinfo";
            if (clgname != "")
            {
                ds = da.select_method(clgname, hat, "Text");
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
            for (int i = 0; i < ddlclg.Items.Count; i++)
            {
                ddlclg.Items[i].Selected = false;
            }
        }
        catch
        {

        }
    }
    public void bindbatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = da.select_method_wo_parameter("select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' order by batch_year select max(batch_year)from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ", "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();

            }
        }
        catch
        {
        }
    }


    public DataSet Bind_Degree(string user_code)
    {
        string college_code = ddlclg.SelectedValue.ToString();

        string deg = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        ds = da.select_method_wo_parameter(deg, "Text");
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string user_code)
    {
        string college_code = ddlclg.SelectedValue.ToString();
        string dept = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "";
        ds = da.select_method_wo_parameter(dept, "Text");
        return ds;
    }

    public void binddegree()
    {
        try
        {
            ddldeg.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlclg.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_degree", hat, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddldeg.DataSource = ds;
                ddldeg.DataTextField = "course_name";
                ddldeg.DataValueField = "course_id";
                ddldeg.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbranch()
    {
        try
        {
            ddlbranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddlclg.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldeg.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds = da.select_method("bind_branch", hat, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindsem()
    {
        try
        {
            collegecode = ddlclg.SelectedValue.ToString();
            ddlsem.Items.Clear();
            string sm = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.Text.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + collegecode + "";
            ds = da.select_method_wo_parameter(sm, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int duration = Convert.ToInt32(ds.Tables[0].Rows[0]["ndurations"].ToString());


                for (int i = 1; i <= duration; i++)
                {
                    ddlsem.Items.Add(i.ToString());
                }

            }
            else
            {
                string sm1 = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
                ds = da.select_method_wo_parameter(sm1, "Text");
                int drtn = Convert.ToInt32(ds.Tables[0].Rows[0]["duration"].ToString());

                for (int i = 1; i <= drtn; i++)
                {
                    ddlsem.Items.Add(i.ToString());
                }

            }
        }
        catch
        {
        }
    }

    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        bindsem();
        BindSectionDetail();
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        BindSectionDetail();
    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindSectionDetail();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbranch.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            string course_id = ddldeg.SelectedValue.ToString();
            bindbranch();
            bindsem();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsem();
            if (!Page.IsPostBack == false)
            {
                ddlsem.Items.Clear();
            }
            try
            {

                bindsem();
                BindSectionDetail();
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                Response.Write(s);
            }
        }
        catch
        {
        }
    }


    public void BindSectionDetail()
    {
        try
        {
            string strbatch = ddlbatch.SelectedValue.ToString();
            string strbranch = ddlbranch.SelectedValue.ToString();
            //string strbranch1 = ddlbranch.SelectedItem.Text.ToString();
            string sem = ddlsem.SelectedItem.Text.ToString();
            ddlsec.Items.Clear();
            ds.Dispose();
            ds.Reset();
            query = "select distinct sections from registration where batch_year =" + strbatch + " and degree_code =" + strbranch + " and current_semester='" + sem + "' and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'";
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                ddlsec.Items.Insert(0, "All");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    ddlsec.Enabled = false;
                }
                else
                {

                    ddlsec.Enabled = true;
                }
            }
            else
            {
                ddlsec.Items.Insert(0, "All");
                ddlsec.Enabled = false;
            }
        }
        catch
        {

        }

    }

    public void reason()
    {
        try
        {
            query = "select TextVal,TextCode FROM textvaltable where TextCriteria='bona' ";
            ds = da.select_method_wo_parameter(query, "Text");
            ddlreason.DataSource = ds;
            ddlreason.DataTextField = "TextVal";
            ddlreason.DataValueField = "TextCode";
            ddlreason.DataBind();
            data = new string[ds.Tables[0].Rows.Count];
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                data[i] = ds.Tables[0].Rows[i]["TextVal"].ToString();
            }
        }
        catch
        {
        }
    }

    protected void btngo_click(object sender, EventArgs e)
    {
        loadspread();
    }
    public void loadspread()
    {
        try
        {
            Fpspread.Sheets[0].ColumnCount = 6;
            Fpspread.Sheets[0].RowCount = 0;
            Fpspread.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread.Sheets[0].ColumnHeader.Height = 40;
            Fpspread.Sheets[0].RowHeader.Visible = false;
            //Fpspread.Sheets[0].ColumnCount = 4;
            Fpspread.CommandBar.Visible = false;
            Fpspread.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpspread.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            Fpspread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].SheetCorner.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            Fpspread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            Fpspread.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpspread.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Student Name";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Type";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reason";
            Fpspread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";
            Fpspread.Width = 543;
            Fpspread.Height = 400;
            Fpspread.Sheets[0].Columns[0].Width = 50;
            Fpspread.Sheets[0].Columns[1].Width = 75;
            Fpspread.Sheets[0].Columns[2].Width = 150;
            Fpspread.Sheets[0].Columns[3].Width = 100;
            Fpspread.Sheets[0].Columns[4].Width = 100;
            Fpspread.Sheets[0].Columns[5].Width = 50;
            Fpspread.Sheets[0].Columns[0].Locked = true;
            Fpspread.Sheets[0].Columns[1].Locked = true;
            Fpspread.Sheets[0].Columns[2].Locked = true;
            Fpspread.Sheets[0].Columns[3].Locked = true;
            //Fpspread.Sheets[0].Columns[4].Locked = true;
            Fpspread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            Fpspread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
            Fpspread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpspread.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            reason();
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.ComboBoxCellType combo = new FarPoint.Web.Spread.ComboBoxCellType(data);

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            combo.ShowButton = true;
            combo.UseValue = true;
            // combo.CssClass=
            // combo.DefaultStyle.Font.Name = "Book Antiqua";

            if (ddlbonafide.SelectedItem.Text != "General")
            {
                Fpspread.Sheets[0].Columns[4].Visible = false;
                Fpspread.Width = 442;
            }
            else
            {
                Fpspread.Sheets[0].Columns[4].Visible = true;
                Fpspread.Width = 543;
            }

            string batchyear = ddlbatch.SelectedValue.ToString();
            string degree_code = ddlbranch.SelectedValue.ToString();
            string semester = ddlsem.SelectedValue.ToString();
            string section = ddlsec.SelectedItem.ToString();
            string clgcode = ddlclg.SelectedValue.ToString();
            string studtype = ddltype.SelectedItem.ToString();
            string strsection = "";

            if (section == "All")
            {
                DataSet dssection = da.BindSectionDetail(batchyear, degree_code);
                if (dssection.Tables[0].Rows.Count > 0)
                {
                    for (int sec = 0; sec < dssection.Tables[0].Rows.Count; sec++)
                    {
                        if (strsection == "")
                        {
                            strsection = dssection.Tables[0].Rows[sec]["sections"].ToString();
                        }
                        else
                        {
                            strsection = strsection + '\\' + dssection.Tables[0].Rows[sec]["sections"].ToString();
                        }
                    }
                }
                else
                {
                    strsection = "";
                }
            }
            else
            {
                strsection = "" + ddlsec.SelectedValue.ToString() + "";
            }
            int cn = 0;
            for (int k = 0; k < 1; k++)
            {
                Fpspread.Sheets[0].RowCount++;
                FarPoint.Web.Spread.CheckBoxCellType cbx = new FarPoint.Web.Spread.CheckBoxCellType();
                Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].CellType = cbx;
                cbx.AutoPostBack = true;
                string[] sectionspilt = strsection.Split('\\');

                for (int scet = 0; scet <= sectionspilt.GetUpperBound(0); scet++)
                {
                    string chksectionvalue = sectionspilt[scet].ToString();

                    if (chksectionvalue == "")
                    {
                        sectionvalue = "";
                    }
                    else
                    {
                        sectionvalue = chksectionvalue.ToString();
                    }
                    if (ddltype.SelectedItem.Text != "Both")
                    {
                        if (sectionvalue != "")
                        {
                            query = "select distinct Roll_No,Stud_Type,Stud_Name from Registration where Stud_Type='" + studtype + "' and Batch_Year='" + batchyear + "' and Current_Semester='" + semester + "' and degree_code='" + degree_code + "' and college_code='" + clgcode + "' and Sections='" + sectionvalue + "'  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by Roll_No";
                            ds = da.select_method_wo_parameter(query, "Text");
                        }
                        else
                        {

                            query = "select distinct Roll_No,Stud_Type,Stud_Name from Registration where Stud_Type='" + studtype + "' and Batch_Year='" + batchyear + "' and Current_Semester='" + semester + "' and degree_code='" + degree_code + "' and college_code='" + clgcode + "' and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by Roll_No";
                            ds = da.select_method_wo_parameter(query, "Text");
                        }
                    }
                    else
                    {
                        if (sectionvalue != "")
                        {
                            query = "select distinct Roll_No,Stud_Type,Stud_Name from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' and college_code='" + clgcode + "' and Sections='" + sectionvalue + "' and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by Roll_No";
                            ds = da.select_method_wo_parameter(query, "Text");
                        }
                        else
                        {
                            query = "select distinct Roll_No,Stud_Type,Stud_Name from Registration where Batch_Year='" + batchyear + "' and degree_code='" + degree_code + "' and Current_Semester='" + semester + "' and college_code='" + clgcode + "' and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0 order by Roll_No";
                            ds = da.select_method_wo_parameter(query, "Text");
                        }

                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            resultflag = true;
                            Fpspread.Sheets[0].RowCount++;
                            cn++;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].CellType = txt;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].CellType = txt;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].CellType = txt;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].CellType = txt;
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 0].Text = cn.ToString();
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["Stud_Name"].ToString();
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["Stud_Type"].ToString();
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].CellType = combo;
                            //Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 4].Text = combo.ToString();
                            Fpspread.Sheets[0].Cells[Fpspread.Sheets[0].RowCount - 1, 5].CellType = cb;
                            cb.AutoPostBack = true;
                            // Fpspread.Sheets[0].Rows.Add(0, 4, 3, 1);
                        }
                        Fpspread.Visible = true;
                        btngenerate.Visible = true;
                        lblerr.Visible = false;
                    }

                }
                if (resultflag == false)
                {
                    Fpspread.Visible = false;
                    lblerr.Visible = true;
                    lblerr.Text = "No Records Found";
                    btngenerate.Visible = false;
                }
                //Fpspread.Sheets[0].PageSize = Fpspread.Sheets[0].RowCount;
            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }




    protected void btngenerate_click(object sender, EventArgs e)
    {
        try
        {
            Fpspread.SaveChanges();
            bindpdf();
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }
    protected void Fpspread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            //string actrow = e.SheetView.ActiveRow.ToString();
            string actrow = e.CommandArgument.ToString();
            if (flag_true == false && actrow == "0")
            {
                for (int j = 0; j < Convert.ToInt16(Fpspread.Sheets[0].RowCount); j++)
                {
                    string actcol = e.SheetView.ActiveColumn.ToString();
                    string seltext = e.EditValues[5].ToString();
                    if (seltext != "System.Object" && seltext != "Selector For All")
                    {
                        Fpspread.Sheets[0].Cells[j, 5].Text = seltext.ToString();
                    }
                }
                flag_true = true;
            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }

    public void bindpdf()
    {
        try
        {
            Fpspread.SaveChanges();
            Font Fontbold = new Font("Times New Roman", 14, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 22, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 16, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall1 = new Font("Times New Roman", 14, FontStyle.Regular);
            Font Fontsmall2 = new Font("Times New Roman", 16, FontStyle.Regular);
            Font Fontsmall3 = new Font("Lucida Calligraphy", 10, FontStyle.Regular);
            collegecode = ddlclg.SelectedValue.ToString();

            string date = System.DateTime.Now.ToString("dd/MM/yyy");
            query = "select value from master_settings where settings='Academic year'";
            ds = da.select_method_wo_parameter(query, "Text");
            string finyr = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                string[] yearset = ds.Tables[0].Rows[0]["value"].ToString().Split(',');
                finyr = yearset[0] + " - " + yearset[1];
            }
            else
            {
                finyr = "";
            }

            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            //Gios.Pdf.PdfDocument myprovdoc1 = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);

            Gios.Pdf.PdfDocument myprovdoc1 = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);

            for (int i = 1; i < Fpspread.Sheets[0].Rows.Count; i++)
            {

                int isval = 0;
                isval = Convert.ToInt32(Fpspread.Sheets[0].Cells[i, 5].Value);
                if (isval == 1)
                {
                    if (ddlbonafide.SelectedItem.Text == "General")
                    {

                        saveflag = true;
                        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();

                        int y = 40;


                        PdfArea tete = new PdfArea(myprovdoc, 40, 40, 760, 510);
                        PdfRectangle pr1 = new PdfRectangle(myprovdoc, tete, Color.Black);
                        myprov_pdfpage.Add(pr1);

                        PdfArea tete1 = new PdfArea(myprovdoc, 45, 45, 750, 500);
                        PdfRectangle pr2 = new PdfRectangle(myprovdoc, tete1, Color.Black, 3);
                        myprov_pdfpage.Add(pr2);
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 50, 65, 360);
                        }

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            PdfImage LogoImage1 = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage1, 705, 65, 360);
                        }

                        string clgquery = "select UPPER(collname)as collname,category,university,UPPER(address3) as address3, UPPER(district) as district,UPPER (state) as state,pincode,affliatedby from collinfo where college_code='" + collegecode + "'";
                        ds = da.select_method_wo_parameter(clgquery, "Text");

                        string clgname = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        string category = Convert.ToString(ds.Tables[0].Rows[0]["category"]);
                        string univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                        string add = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                        string pin = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                        string afliated = Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]);
                        string dist = Convert.ToString(ds.Tables[0].Rows[0]["district"]);
                        string state = Convert.ToString(ds.Tables[0].Rows[0]["state"]);
                        string studname = Convert.ToString(Fpspread.Sheets[0].Cells[i, 2].Text);
                        string studroll = Convert.ToString(Fpspread.Sheets[0].Cells[i, 1].Text);
                        string studtype = Convert.ToString(Fpspread.Sheets[0].Cells[i, 3].Text);
                        string reason = Convert.ToString(Fpspread.Sheets[0].Cells[i, 4].Text);
                        string year = Convert.ToString(ddlbatch.SelectedItem.Text);
                        string degcode = Convert.ToString(ddlbranch.SelectedValue);
                        string branch = Convert.ToString(ddlbranch.SelectedItem.Text);
                        string sem = Convert.ToString(ddlsem.SelectedItem.Text);
                        string hrn = "";
                        string hnm = "";
                        string crntsem = "";
                        string prntnm = "";
                        if (studtype == "Hostler" || studtype == "HOSTLER")
                        {
                            query = "select a.Room_Name,b.Hostel_Name,c.Current_Semester,d.parent_name from Hostel_StudentDetails a,Hostel_Details b,Registration c,applyn d where a.Roll_Admit=c.Roll_Admit and a.Hostel_Code=b.Hostel_code and d.app_no=c.App_No and c.Roll_No='" + studroll + "' and c.degree_code='" + degcode + "' and c.Batch_Year='" + year + "' and c.college_code='" + collegecode + "'";
                            ds = da.select_method_wo_parameter(query, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                hrn = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                                hnm = Convert.ToString(ds.Tables[0].Rows[0]["Hostel_Name"]);
                            }
                        }
                        query = "select b.parent_name,b.current_semester from Registration a,applyn b where a.App_No=b.app_no and a.Roll_No='" + studroll + "' and a.degree_code='" + degcode + "' and a.Batch_Year='" + year + "' and a.college_code='" + collegecode + "'";
                        ds = da.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            crntsem = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);
                            prntnm = Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
                        }

                        string finval = "";
                        if (sem == "1" || sem == "2")
                        {
                            finval = "I";
                        }
                        else if (sem == "3" || sem == "4")
                        {
                            finval = "II";
                        }
                        else if (sem == "5" || sem == "6")
                        {
                            finval = "III";
                        }
                        else if (sem == "7" || sem == "8")
                        {
                            finval = "IV";
                        }
                        else if (sem == "9" || sem == "10")
                        {
                            finval = "V";
                        }

                        PdfTextArea ptc1 = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                         new PdfArea(myprovdoc, 20, y + 25, 820, 30), System.Drawing.ContentAlignment.MiddleCenter, clgname);
                        myprov_pdfpage.Add(ptc1);

                        PdfTextArea ptc2 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(myprovdoc, 20, y + 40, 820, 30), System.Drawing.ContentAlignment.MiddleCenter, "(An " + category + " Institution)");

                        myprov_pdfpage.Add(ptc2);

                        PdfTextArea ptc3 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                       new PdfArea(myprovdoc, 20, y + 60, 820, 30), System.Drawing.ContentAlignment.MiddleCenter, "(" + afliated + ")");

                        myprov_pdfpage.Add(ptc3);

                        PdfTextArea ptc4 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc, 20, y + 80, 820, 30), System.Drawing.ContentAlignment.MiddleCenter, add + "-" + pin + "  " + dist + "  " + state + "  " + "INDIA");

                        myprov_pdfpage.Add(ptc4);

                        PdfTextArea ptc5 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                        new PdfArea(myprovdoc, 40, y + 115, 650, 30), System.Drawing.ContentAlignment.BottomRight, "Date: " + date);

                        myprov_pdfpage.Add(ptc5);
                        PdfArea tete2 = new PdfArea(myprovdoc, 303, y + 140, 240, 30);
                        PdfRectangle pr3 = new PdfRectangle(myprovdoc, tete2, Color.Black);
                        myprov_pdfpage.Add(pr3);

                        PdfTextArea ptc6 = new PdfTextArea(Fontbold2, System.Drawing.Color.Black,
                                                                       new PdfArea(myprovdoc, 305, y + 145, 220, 30), System.Drawing.ContentAlignment.BottomRight, "C E R T I F I C A T E");

                        myprov_pdfpage.Add(ptc6);



                        string strgetcontend = "This is to certify that Selvan/Selvi " + studname.ToUpper() + ", Roll No " + studroll.ToUpper() + " S/o, D/o Mr." + prntnm + ", is a bonafide Student of " + clgname + " studying in  " + finval + "  Year " + ddldeg.SelectedItem.Text + " " + branch + " course during the academic year " + finyr + ".";

                        int coltop = 200;
                        string[] spval = strgetcontend.Split(' ');
                        string setval1 = "";
                        string setetxtva = "";
                        int lenofline = 95;
                        for (int spa1 = 0; spa1 <= spval.GetUpperBound(0); spa1++)
                        {
                            if (setval1 == "")
                            {
                                setval1 = spval[spa1].ToString();
                            }
                            else
                            {
                                setval1 = setval1 + " " + spval[spa1].ToString();
                            }
                            if (setval1.Length > lenofline || spa1 == spval.GetUpperBound(0))
                            {
                                coltop = coltop + 20;
                                if (spa1 == spval.GetUpperBound(0))
                                {
                                    setetxtva = setetxtva + " " + spval[spa1].ToString();
                                }
                                int lali = 60;
                                if (lenofline == 95)
                                {
                                    lali = 140;
                                }
                                PdfTextArea ptc55 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                             new PdfArea(myprovdoc, lali, coltop, 800, 50), System.Drawing.ContentAlignment.MiddleLeft, setetxtva);
                                myprov_pdfpage.Add(ptc55);
                                setval1 = spval[spa1].ToString();
                                lenofline = 105;
                            }
                            setetxtva = setval1;
                        }
                        string hostelorbonafied = "He / She is Day Scholar."; coltop = coltop + 20;
                        if (studtype.ToLower() == "day scholar" || studtype == "DAY SCHOLAR")
                        {
                        }
                        else
                        {
                            hostelorbonafied = "He / She is staying in our " + hnm + " Hostel. Room No: " + hrn + "";

                        }

                        PdfTextArea ptc19 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                 new PdfArea(myprovdoc, 140, coltop, 750, 100), System.Drawing.ContentAlignment.MiddleLeft, hostelorbonafied);

                        myprov_pdfpage.Add(ptc19);

                        coltop = coltop + 20;
                        PdfTextArea ptc20 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                              new PdfArea(myprovdoc, 140, coltop, 750, 100), System.Drawing.ContentAlignment.MiddleLeft, "This Certificate is issued to apply for " + reason + "");

                        myprov_pdfpage.Add(ptc20);



                        myprov_pdfpage.SaveToDocument();
                    }
                    if (ddlbonafide.SelectedItem.Text == "Passport")
                    {

                        saveflag = true;
                        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc1.NewPage();

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = myprovdoc1.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 10, 5, 400);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            PdfImage LogoImage1 = myprovdoc1.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage1, 505, 5, 400);
                        }


                        string clgquery = "select collname,category,affliatedby,university,address3,district,state,pincode,acr,phoneno,faxno,email,website from collinfo where college_code='" + collegecode + "'";
                        ds = da.select_method_wo_parameter(clgquery, "Text");

                        string clgname = Convert.ToString(ds.Tables[0].Rows[0]["collname"]);
                        string category = Convert.ToString(ds.Tables[0].Rows[0]["category"]);
                        string univ = Convert.ToString(ds.Tables[0].Rows[0]["university"]);
                        string add = Convert.ToString(ds.Tables[0].Rows[0]["address3"]);
                        string pin = Convert.ToString(ds.Tables[0].Rows[0]["pincode"]);
                        string afliated = Convert.ToString(ds.Tables[0].Rows[0]["affliatedby"]);
                        string dist = Convert.ToString(ds.Tables[0].Rows[0]["district"]);
                        string state = Convert.ToString(ds.Tables[0].Rows[0]["state"]);
                        string acr = Convert.ToString(ds.Tables[0].Rows[0]["acr"]);
                        string offph = Convert.ToString(ds.Tables[0].Rows[0]["phoneno"]);
                        string fax = Convert.ToString(ds.Tables[0].Rows[0]["faxno"]);
                        string mail = Convert.ToString(ds.Tables[0].Rows[0]["email"]);
                        string web = Convert.ToString(ds.Tables[0].Rows[0]["website"]);

                        string studname = Convert.ToString(Fpspread.Sheets[0].Cells[i, 2].Text);
                        string studroll = Convert.ToString(Fpspread.Sheets[0].Cells[i, 1].Text);
                        string studtype = Convert.ToString(Fpspread.Sheets[0].Cells[i, 3].Text);
                        string year = Convert.ToString(ddlbatch.SelectedItem.Text);
                        string degcode = Convert.ToString(ddlbranch.SelectedValue);
                        string branch = Convert.ToString(ddlbranch.SelectedItem.Text);
                        string course = Convert.ToString(ddldeg.SelectedItem.Text);
                        string sem = Convert.ToString(ddlsem.SelectedItem.Text);
                        string hrn = "";
                        string hnm = "";
                        string crntsem = "";
                        string prntnm = "";
                        string prntadd = "";
                        string dob = "";
                        string dob1 = "";
                        string nationality = "";
                        string sex = "";
                        if (studtype == "Hostler" || studtype == "HOSTLER")
                        {
                            query = "select a.Room_Name,b.Hostel_Name,c.Current_Semester,d.parent_name,d.parent_addressP,d.dob,d.citizen,d.Streetp,d.Cityp,d.Districtp,d.parent_pincodep,d.sex from Hostel_StudentDetails a,Hostel_Details b,Registration c,applyn d where a.Roll_Admit=c.Roll_Admit and a.Hostel_Code=b.Hostel_code and d.app_no=c.App_No and c.Roll_No='" + studroll + "' and c.degree_code='" + degcode + "' and c.Batch_Year='" + year + "' and c.college_code='" + collegecode + "'";
                            ds = da.select_method_wo_parameter(query, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                hrn = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);
                                hnm = Convert.ToString(ds.Tables[0].Rows[0]["Hostel_Name"]);
                            }
                        }

                        query = "select b.parent_name,b.current_semester,b.parent_addressP,b.dob,b.citizen,b.Streetp,b.Cityp,b.Districtp,b.parent_pincodep,b.sex from Registration a,applyn b where a.App_No=b.app_no and a.Roll_No='" + studroll + "' and a.degree_code='" + degcode + "' and a.Batch_Year='" + year + "' and a.college_code='" + collegecode + "'";
                        ds = da.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            crntsem = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);
                            prntnm = Convert.ToString(ds.Tables[0].Rows[0]["parent_name"]);
                            prntadd = Convert.ToString(ds.Tables[0].Rows[0]["parent_addressP"]) + "," + ds.Tables[0].Rows[0]["Streetp"].ToString() + "," + ds.Tables[0].Rows[0]["Cityp"].ToString() + "," + ds.Tables[0].Rows[0]["Districtp"].ToString() + "-" + ds.Tables[0].Rows[0]["parent_pincodep"].ToString();
                            nationality = Convert.ToString(ds.Tables[0].Rows[0]["citizen"]);
                            sex = ds.Tables[0].Rows[0]["sex"].ToString();
                            string db = Convert.ToString(ds.Tables[0].Rows[0]["dob"]);
                            if (db != "")
                            {
                                string[] d1 = db.Split(' ');
                                dob = d1[0];
                                string[] d2 = dob.Split('/');
                                dob1 = d2[1] + "/" + d2[0] + "/" + d2[2];
                            }
                            else
                            {
                                dob1 = "";
                            }
                        }


                        string finval = "";
                        if (sem == "1" || sem == "2")
                        {
                            finval = "First Year";
                        }
                        else if (sem == "3" || sem == "4")
                        {
                            finval = "Second Year";
                        }
                        else if (sem == "5" || sem == "6")
                        {
                            finval = "Third Year";
                        }
                        else if (sem == "7" || sem == "8")
                        {
                            finval = "Fourth Year";
                        }
                        else if (sem == "9" || sem == "10")
                        {
                            finval = "Fifth Year";
                        }

                        string s = "";
                        string ss = "";
                        if (sex == "1")
                        {
                            s = "She";
                            ss = "Selvi";
                        }

                        else if (sex == "0")
                        {
                            s = "He";
                            ss = "Selvan";
                        }
                        else
                        {
                            s = "He/She";
                            ss = "Selvan/Selvi";
                        }
                        int coltop = 15;
                        PdfTextArea ptc1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                       new PdfArea(myprovdoc1, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, clgname);
                        myprov_pdfpage.Add(ptc1);
                        coltop = coltop + 15;
                        PdfTextArea ptc2 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc1, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, add + "," + dist + "-" + pin);

                        myprov_pdfpage.Add(ptc2);
                        coltop = coltop + 15;
                        PdfTextArea ptc3 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc1, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, category);

                        myprov_pdfpage.Add(ptc3);
                        coltop = coltop + 10;
                        PdfTextArea ptc4 = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                                     new PdfArea(myprovdoc1, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, "(" + afliated + ")");

                        myprov_pdfpage.Add(ptc4);
                        coltop = coltop + 10;
                        PdfTextArea ptc5 = new PdfTextArea(Fontsmall3, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc1, 20, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "==============================================================================================");

                        myprov_pdfpage.Add(ptc5);
                        coltop = coltop + 10;
                        PdfTextArea ptc6 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Ref: " + acr + "/PASSPORT/B.E/B.Tech/" + finyr + "                                                                                                            Date: " + date);

                        myprov_pdfpage.Add(ptc6);
                        coltop = coltop + 40;
                        PdfTextArea ptc7 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, "BONAFIDE CERTIFICATE");

                        myprov_pdfpage.Add(ptc7);
                        PdfTextArea ptc8 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                  new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleCenter, "_______________________");
                        myprov_pdfpage.Add(ptc8);

                        string strgetcondent = "This is to certify that " + ss + " " + studname + ", Roll No " + studroll + " D/o,S/o Mr " + prntnm + " Studying in " + finval + " " + course + " " + branch + " in our College during the academic year " + finyr + ".";
                        string[] spval = strgetcondent.Split(' ');
                        string setval1 = "";
                        string setetxtva = "";
                        int lenofline = 65;
                        for (int spa1 = 0; spa1 <= spval.GetUpperBound(0); spa1++)
                        {
                            if (setval1 == "")
                            {
                                setval1 = spval[spa1].ToString();
                            }
                            else
                            {
                                setval1 = setval1 + " " + spval[spa1].ToString();
                            }
                            if (setval1.Length > lenofline || spa1 == spval.GetUpperBound(0))
                            {
                                coltop = coltop + 25;
                                if (spa1 == spval.GetUpperBound(0))
                                {
                                    setetxtva = setetxtva + " " + spval[spa1].ToString();
                                }
                                int lali = 25;
                                if (lenofline == 65)
                                {
                                    lali = 100;
                                }
                                PdfTextArea ptc55 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                             new PdfArea(myprovdoc1, lali, coltop, 800, 30), System.Drawing.ContentAlignment.MiddleLeft, setetxtva);
                                myprov_pdfpage.Add(ptc55);
                                setval1 = spval[spa1].ToString();
                                lenofline = 66;
                            }
                            setetxtva = setval1;
                        }

                        if (studtype.Trim().ToLower() == "day scholar" || studtype == "DAY SCHOLAR")
                        {
                            strgetcondent = "" + s + " is Day Scholar.";
                        }
                        else
                        {
                            strgetcondent = "" + s + " is staying in our " + hnm + "Hostel Room No : " + hrn + "";
                        }

                        coltop = coltop + 30;
                        PdfTextArea ptc13 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                               new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, strgetcondent);
                        myprov_pdfpage.Add(ptc13);

                        string ntnlty = "";
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            ds1.Dispose();
                            ds1.Clear();
                            if (nationality != "")
                            {
                                query = "select TextVal from textvaltable where TextCode='" + nationality + "'";
                                ds1 = da.select_method_wo_parameter(query, "Text");
                            }

                            if (ds1.Tables.Count > 0)
                            {
                                if (ds1.Tables[0].Rows.Count > 0)
                                {
                                    ntnlty = Convert.ToString(ds1.Tables[0].Rows[0]["TextVal"]);
                                }
                                else
                                {
                                    ntnlty = "";
                                }
                            }
                        }
                        coltop = coltop + 30;
                        PdfTextArea ptc14 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Nationality : " + ntnlty);

                        myprov_pdfpage.Add(ptc14);

                        coltop = coltop + 30;
                        PdfTextArea ptc15 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                   new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "DOB :" + dob1);

                        myprov_pdfpage.Add(ptc15);
                        coltop = coltop + 30;
                        PdfTextArea ptc16 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc1, 25, coltop, 650, 30), System.Drawing.ContentAlignment.MiddleLeft, "(" + prntadd + ")");

                        myprov_pdfpage.Add(ptc16);
                        coltop = coltop + 30;
                        PdfTextArea ptc17 = new PdfTextArea(Fontsmall2, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc1, 25, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "This certificate is issued to apply for Passport.");

                        myprov_pdfpage.Add(ptc17);

                        coltop = coltop + 30;
                        PdfArea tete = new PdfArea(myprovdoc, 50, coltop, 90, 100);
                        PdfRectangle pr1 = new PdfRectangle(myprovdoc, tete, Color.Black);
                        myprov_pdfpage.Add(pr1);

                        PdfArea tete1 = new PdfArea(myprovdoc, 55, coltop + 5, 80, 90);
                        PdfRectangle pr2 = new PdfRectangle(myprovdoc, tete1, Color.Black);
                        myprov_pdfpage.Add(pr2);

                        coltop = coltop + 30;
                        PdfTextArea ptc18 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                    new PdfArea(myprovdoc1, 60, coltop, 70, 30), System.Drawing.ContentAlignment.MiddleCenter, "Affix Stamp Size Photo here");
                        myprov_pdfpage.Add(ptc18);

                        coltop = coltop + 80;
                        PdfTextArea ptc19 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                     new PdfArea(myprovdoc1, 30, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "___________________________________________________________________________________________________________");
                        myprov_pdfpage.Add(ptc19);

                        coltop = coltop + 5;
                        PdfTextArea ptc20 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc1, 30, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "___________________________________________________________________________________________________________");
                        myprov_pdfpage.Add(ptc20);

                        coltop = coltop + 20;
                        PdfTextArea ptc21 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc1, 30, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Ph.Nos:  Office: " + offph + "    Fax : " + fax);
                        myprov_pdfpage.Add(ptc21);

                        coltop = coltop + 15;
                        PdfTextArea ptc22 = new PdfTextArea(Fontsmall1, System.Drawing.Color.Black,
                                                                      new PdfArea(myprovdoc1, 30, coltop, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "E-mail : " + mail + "  Website : " + web);
                        myprov_pdfpage.Add(ptc22);

                        myprov_pdfpage.SaveToDocument();

                    }
                }
            }
            if (saveflag == false)
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select Atleast Anyone Detail";
            }
            else
            {

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    // Response.Buffer = true;
                    // Response.Clear();
                    string szPath = appPath + "/Report/";
                    string szFile = "";
                    if (ddlbonafide.SelectedItem.Text == "General")
                    {
                        szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "General.pdf";
                        myprovdoc.SaveToFile(szPath + szFile);

                    }
                    else
                    {
                        szFile = DateTime.Now.ToString("ddMMyyyyhhmmsstt") + "Passport.pdf";
                        myprovdoc1.SaveToFile(szPath + szFile);
                    }

                    string getpath = szFile;
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + getpath + "");
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);

                }

            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }
    //protected void ddlreason_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    btnaddr.Visible = true;
    //    btndelr.Visible = true;
    //}
    protected void btnaddr_click(object sender, EventArgs e)
    {
        Panel9.Visible = true;

    }
    protected void btndelr_click(object sender, EventArgs e)
    {
        try
        {
            query = "delete FROM textvaltable where TextVal='" + ddlreason.SelectedItem.Text + "'";
            int a = da.update_method_wo_parameter(query, "Text");
            if (a == 1)
            {
                reason();
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }

    protected void btnadd1_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtadd.Text != "")
            {
                query = "select * FROM textvaltable where TextVal='" + txtadd.Text + "' ";
                ds = da.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Reason Already Exists')", true);
                    txtadd.Text = "";
                    reason();
                    Panel9.Visible = false;
                }
                else
                {
                    query = "insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + txtadd.Text + "','bona','" + Session["collegecode"].ToString() + "')";
                    int a = da.update_method_wo_parameter(query, "Text");
                    if (a == 1)
                    {
                        txtadd.Text = "";
                        reason();
                        Panel9.Visible = false;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Added Successfully')", true);
                    }
                }
            }
            else
            {
                reason();
                Panel9.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }

    protected void btnexit1_Click(object sender, EventArgs e)
    {
        try
        {
            Panel9.Visible = false;
            reason();
        }
        catch (Exception ex)
        {
            lblerr.Visible = true;
            lblerr.Text = ex.ToString();
        }
    }
    protected void ddlbonafide_change(object sender, EventArgs e)
    {
        try
        {
            if (ddlbonafide.SelectedItem.Text == "General")
            {
                ddlreason.Enabled = true;
            }
            else
            {
                ddlreason.Enabled = false;
            }
        }
        catch
        {
        }
    }
}