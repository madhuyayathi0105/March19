using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Collections;
using InsproDataAccess;
using System.Text;
using System.Configuration;

public partial class NewAlterBatchallocation : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    InsproDirectAccess dirAcc = new InsproDirectAccess();

    string usercode = "", collegecode = "", singleuser = "", group_user = string.Empty;

    enum UserAct { FView, Fsave, Fupdate, FDelete, FReport, FChange };

    enum InsModules { MStudent, MStaff, MOffice, MAcademic, MFinance, MLibrary, MHostel, MHr, MInventory, MWizard, MAdmin };

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("ScheduleHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/ScheduleMOD/ScheduleHome.aspx");
                    return;
                }
            }
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if (!Page.IsPostBack)
            {
                batch_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                batch_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;

                txtFromDate.Attributes.Add("ReadOnly", "ReadOnly");
                selbtn.Visible = false;
                batch_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                batch_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                batch_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                batch_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                batch_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                batch_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;


                sml_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sml_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sml_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                sml_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sml_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sml_spread.ActiveSheetView.RowHeader.DefaultStyle.Font.Bold = true;
                sml_spread.CommandBar.Visible = false;

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 13;
                style.Font.Bold = true;
                batch_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                batch_spread.Sheets[0].AllowTableCorner = true;
                batch_spread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                sml_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 13;
                style1.Font.Bold = true;
                sml_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                sml_spread.Sheets[0].AllowTableCorner = true;
                sml_spread.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                bcntlbl.Visible = false;
                btctxt.Visible = false;
                bcntddl.Visible = false;
                bcntddllbl.Visible = false;
                btnsave.Enabled = false;
                delbtn.Enabled = false;
                errlbl.Visible = false;

                batchpanel.Visible = false;
                deglbl.Visible = false;
                branlbl.Visible = false;
                seclbl.Visible = false;
                semlbl.Visible = false;
                fmlbl.Visible = false;

                sfrlbl.Enabled = false;
                sfmtxt.Enabled = false;
                stolbl.Enabled = false;
                stotxt.Enabled = false;
                selbtn.Enabled = false;

                panel_sp1.Visible = false;
                Panel_sp2.Visible = false;


                string dt = DateTime.Today.ToShortDateString();
                string[] dsplit = dt.Split(new Char[] { '/' });
                Session["curr_year"] = dsplit[2].ToString();
                txtFromDate.Text = dsplit[1].ToString() + "/" + dsplit[0].ToString() + "/" + dsplit[2].ToString();

                bindbatch();
                binddegree();
                ddlduration.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
                ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
                string strdayflag = string.Empty;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                string grouporusercode = string.Empty;
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master1 = "select * from Master_Settings where " + grouporusercode + "";
                DataSet dsmasetr = d2.select_method_wo_parameter(Master1, "Text");
                if (dsmasetr.Tables[0].Rows.Count > 0)
                {
                    for (int m = 0; m < dsmasetr.Tables[0].Rows.Count; m++)
                    {
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Roll No" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Register No" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Student_Type" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Days Scholor" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            strdayflag = " and (Stud_Type='Day Scholar'";
                        }
                        if (dsmasetr.Tables[0].Rows[m]["settings"].ToString() == "Hostel" && dsmasetr.Tables[0].Rows[m]["value"].ToString() == "1")
                        {
                            if (strdayflag != "" && strdayflag != "\0")
                            {
                                strdayflag = strdayflag + " or Stud_Type='Hostler'";
                            }
                            else
                            {
                                strdayflag = " and (Stud_Type='Hostler'";
                            }
                        }
                    }
                }
                if (strdayflag != "")
                {
                    strdayflag = strdayflag + ")";
                }
                Session["strvar"] = strdayflag;
            }
        }
        catch (Exception ex) { }

    }

    public void bindbatch()
    {
        try
        {
            string sqlstr = " select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year";
            DataSet ds1 = d2.select_method_wo_parameter(sqlstr, "Text");
            ddlbatch.DataSource = ds1;
            ddlbatch.DataValueField = "batch_year";
            ddlbatch.DataBind();

            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' ";
            int max_bat = Convert.ToInt32(d2.GetFunction(sqlstr));
            ddlbatch.SelectedValue = max_bat.ToString();
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    public void binddegree()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds = d2.select_method("bind_degree", hat, "sp");
            ddldegree.DataSource = ds;
            ddldegree.DataValueField = "course_id";
            ddldegree.DataTextField = "course_name";
            ddldegree.DataBind();

            ddldegree.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    public void bindsem()
    {
        try
        {
            ddlduration.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strsemquery = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
            DataSet dssem = d2.select_method_wo_parameter(strsemquery, "Text");
            if (dssem.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlduration.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                strsemquery = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "";
                dssem.Dispose();
                dssem.Reset();
                dssem = d2.select_method_wo_parameter(strsemquery, "Text");
                if (dssem.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(dssem.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(dssem.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlduration.Items.Add(i.ToString());
                        }
                    }
                }
            }
            ddlduration.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsec();
    }

    public void bindsec()
    {
        try
        {
            ddlsec.Items.Clear();
            ddlsec.Enabled = false;
            string strsecquery = " select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and delflag=0 and exam_flag<>'Debar' and sections<>'-1' and isnull(sections,'')<>''";
            DataSet ds = d2.select_method_wo_parameter(strsecquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataBind();
                ddlsec.Enabled = true;
            }
            ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
            semlbl.Visible = false;
            seclbl.Visible = false;
            fmlbl.Visible = false;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddlbranch.Items.Clear();
            string collegecode = Session["collegecode"].ToString();
            string usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            string course_id = ddldegree.SelectedValue.ToString();
            DataSet ds = d2.BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
            ddlbranch.Items.Insert(0, new ListItem("--Select--", "-1"));
            deglbl.Visible = false;
            branlbl.Visible = false;
            semlbl.Visible = false;
            seclbl.Visible = false;
            fmlbl.Visible = false;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        branlbl.Visible = false;
        semlbl.Visible = false;
        seclbl.Visible = false;
        fmlbl.Visible = false;
        bindsem();
        bindsec();
    }

    protected void btctxt_TextChanged(object sender, EventArgs e)
    {
        bcntddl.Items.Clear();
        string numbatch = string.Empty;
        int b_val = 0;
        numbatch = btctxt.Text.ToString();
        if (numbatch != "" && numbatch != "0")
        {
            bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
            for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
            {
                bcntddl.Items.Add("B" + b_val.ToString());

            }
            btnsave.Enabled = true;
            delbtn.Enabled = true;
            btn2sv.Enabled = true;
        }
        else
        {
            errlbl.Visible = true;
            errlbl.Text = "Select Number of batch";
            btnsave.Enabled = false;
            delbtn.Enabled = false;
            btn2sv.Enabled = false;
        }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        btctxt.Text = string.Empty;
        batch_spread.CurrentPage = 0;
        if (ddldegree.Text == "-1" || ddldegree.Text == "")
        {
            errlbl.Text = "Select Any degree";
            errlbl.Visible = true;
            return;
        }
        else if (ddlbranch.Text == "-1" || ddlbranch.Text == "")
        {
            errlbl.Text = "Select Any branch";
            errlbl.Visible = true;
            return;
        }
        else if (ddlduration.Text == "" || ddlduration.Text == "-1")
        {
            errlbl.Text = "Select Any  semester";
            errlbl.Visible = true;
            return;
        }
        else if (ddlsec.Enabled == true && ddlsec.Text == "")
        {
            errlbl.Text = "Select Any section";
            errlbl.Visible = true;
        }
        else if (ddlsec.Enabled == true && ddlsec.Text == "-1")
        {
            errlbl.Text = "Select Any section";
            errlbl.Visible = true;
        }
        else if (txtFromDate.Text == "")
        {
            errlbl.Text = "Select Any Date";
            errlbl.Visible = true;
        }

        else if (txtFromDate.Text != "" && ddldegree.SelectedValue != "-1" && ddlbranch.SelectedValue != "-1" && ddlduration.SelectedValue != "-1")//Modified by Manikandan from above Line on 20/08/2013
        {
            loadbatch();
        }
    }

    public void loadbatch()
    {
        try
        {
            bcntlbl.Visible = true;
            btctxt.Visible = true;
            bcntddl.Visible = true;
            bcntddllbl.Visible = true;
            batch_spread.Sheets[0].RowCount = 0;
            sml_spread.Sheets[0].ColumnCount = 0;
            sml_spread.Sheets[0].RowCount = 0;
            bcntddl.Items.Clear();
            batch_spread.CommandBar.Visible = false;
            batch_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            Fieldset5.Visible = false;
            batch_spread.Sheets[0].ColumnCount = 7;
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Select";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 5].Text = "App No";
            batch_spread.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Batch";

            batch_spread.Sheets[0].Columns[0].Width = 60;
            batch_spread.Sheets[0].Columns[1].Width = 80;
            batch_spread.Sheets[0].Columns[2].Width = 80;
            batch_spread.Sheets[0].Columns[3].Width = 160;
            batch_spread.Sheets[0].Columns[4].Width = 60;
            batch_spread.Sheets[0].Columns[5].Width = 60;
            batch_spread.Sheets[0].Columns[6].Width = 60;
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            batch_spread.Sheets[0].Columns[0].CellType = chkcell;
            chkcell.AutoPostBack = true;
            batch_spread.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            batch_spread.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            batch_spread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            batch_spread.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            batch_spread.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
            batch_spread.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            batch_spread.Sheets[0].Columns[1].CellType = txt;
            batch_spread.Sheets[0].Columns[2].CellType = txt;
            batch_spread.Sheets[0].Columns[4].Visible = false;
            batch_spread.Sheets[0].Columns[5].Visible = false;

            int col_val = 0;
            int row_val = 0;
            int maxrow = 0;
            string strsql = string.Empty;
            string date1 = string.Empty;
            string fmdate = string.Empty;
            Boolean noflag = false;
            string dateval = string.Empty;
            string strDay = string.Empty;
            string strsec = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                if (ddlsec.Text.ToString() == "" || ddlsec.Text.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and LTRIM(RTRIM(ISNULL(sections,'')))='" + ddlsec.Text.ToString() + "'";
                }
            }
            if (Session["Rollflag"].ToString() == "0")
            {
                batch_spread.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                batch_spread.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                batch_spread.Sheets[0].ColumnHeader.Columns[4].Visible = false;
            }

            date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            if (date_fm.GetUpperBound(0) == 2)
            {
                if (Convert.ToInt16(date_fm[0].ToString()) <= 31 && Convert.ToInt16(date_fm[1].ToString()) <= 12 && Convert.ToInt16(date_fm[0].ToString()) <= Convert.ToInt16(Session["curr_year"]))
                {

                    fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
                    dateval = date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString();
                    DateTime head_date = Convert.ToDateTime(dateval.ToString());
                    DateTime dt1 = Convert.ToDateTime(fmdate.ToString());
                    chkcell.AutoPostBack = true;
                    strDay = head_date.ToString("ddd");
                    if (strDay != "Sun")
                    {
                        string strorder = "ORDER BY Roll_No";
                        string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                        if (serialno == "1")
                        {
                            strorder = "order by serialno";
                        }
                        else
                        {
                            string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                            if (orderby_Setting == "0")
                            {
                                strorder = "ORDER BY Roll_No";
                            }
                            else if (orderby_Setting == "1")
                            {
                                strorder = "ORDER BY Reg_No";
                            }
                            else if (orderby_Setting == "2")
                            {
                                strorder = "ORDER BY Stud_Name";
                            }
                            else if (orderby_Setting == "0,1,2")
                            {
                                strorder = "ORDER BY Roll_No,Reg_No,Stud_Name";
                            }
                            else if (orderby_Setting == "0,1")
                            {
                                strorder = "ORDER BY Roll_No,Reg_No";
                            }
                            else if (orderby_Setting == "1,2")
                            {
                                strorder = "ORDER BY Reg_No,Stud_Name";
                            }
                            else if (orderby_Setting == "0,2")
                            {
                                strorder = "ORDER BY Roll_No,Stud_Name";
                            }
                        }

                        string strstubatchquery = "select sc.roll_no,sc.semester,sc.Batch,sc.fromdate,sc.todate from Registration r,subjectChooser_New sc,subject s,sub_sem ss where sc.roll_no=r.Roll_No and s.subject_no=sc.subject_no and sc.subtype_no=s.subtype_no and s.subtype_no=ss.subtype_no and ss.lab=1 and sc.fromdate='" + fmdate.ToString() + "' and sc.todate='" + fmdate.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'DEBAR' and r.batch_year='" + ddlbatch.Text.ToString() + " 'and r.degree_code='" + ddlbranch.SelectedValue.ToString() + "' and sc.semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and r.roll_no<>'' and isnull(sc.batch,'')<>''";
                        DataSet dsstubatch = d2.select_method_wo_parameter(strstubatchquery, "Text");

                        strsql = "select distinct roll_no,reg_no,stud_name,stud_type,app_no,serialno from registration where cc=0 and delflag=0 and exam_flag<>'DEBAR' and batch_year='" + ddlbatch.Text.ToString() + "' and degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester='" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and roll_no<>'' " + strorder + "";
                        DataSet dsstu = d2.select_method_wo_parameter(strsql, "Text");
                        if (dsstu.Tables[0].Rows.Count > 0)
                        {
                            for (int r = 0; r < dsstu.Tables[0].Rows.Count; r++)
                            {
                                noflag = true;
                                batch_spread.Sheets[0].RowCount = batch_spread.Sheets[0].RowCount + 1;
                                batch_spread.Sheets[0].Cells[batch_spread.Sheets[0].RowCount - 1, 0].CellType = chkcell;
                                chkcell.AutoPostBack = true;
                                for (col_val = 1; col_val <= 5; col_val++)
                                {
                                    batch_spread.Sheets[0].Columns[col_val].Locked = true;
                                    row_val = batch_spread.Sheets[0].RowCount;
                                    batch_spread.Sheets[0].Cells[row_val - 1, col_val].Text = Convert.ToString(dsstu.Tables[0].Rows[r][col_val - 1]);
                                    batch_spread.Sheets[0].Cells[row_val - 1, col_val].Font.Name = "Book Antiqua";
                                    batch_spread.Sheets[0].Cells[row_val - 1, col_val].Font.Size = FontUnit.Medium;
                                    batch_spread.Sheets[0].Cells[row_val - 1, col_val + 1].Locked = true;
                                }

                                dsstubatch.Tables[0].DefaultView.RowFilter = "roll_no='" + dsstu.Tables[0].Rows[r]["roll_no"].ToString() + "' and semester = " + ddlduration.SelectedValue.ToString() + "  and fromdate='" + fmdate.ToString() + "' and todate='" + fmdate.ToString() + "'";
                                DataView dvstubatch = dsstubatch.Tables[0].DefaultView;
                                if (dvstubatch.Count > 0)
                                {
                                    batch_spread.Sheets[0].Cells[row_val - 1, 0].Font.Name = "Book Antiqua";
                                    batch_spread.Sheets[0].Cells[row_val - 1, 0].Font.Size = FontUnit.Medium;
                                    batch_spread.Sheets[0].Cells[(row_val - 1), 6].Text = dvstubatch[0]["Batch"].ToString();
                                    batch_spread.Sheets[0].Cells[row_val - 1, 1].Locked = true;
                                }
                                else
                                {
                                    batch_spread.Sheets[0].Cells[row_val - 1, 0].Font.Name = "Book Antiqua";
                                    batch_spread.Sheets[0].Cells[row_val - 1, 0].Font.Size = FontUnit.Medium;
                                    batch_spread.Sheets[0].Cells[row_val - 1, 1].Locked = true;
                                    batch_spread.Sheets[0].Cells[(row_val - 1), 6].Text = string.Empty;
                                }
                            }
                        }

                        strsql = "select distinct s.Batch from subjectChooser_New s,Registration r where  r.Roll_No=s.roll_no and batch_year=" + ddlbatch.Text.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedValue.ToString() + "" + strsec.ToString() + " and s.Batch<>''";
                        DataSet ds = d2.select_method(strsql, hat, "Text");
                        Checkboxlistbatch.Items.Clear();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Checkboxlistbatch.DataSource = ds;
                            Checkboxlistbatch.DataTextField = "Batch";
                            Checkboxlistbatch.DataValueField = "Batch";
                            Checkboxlistbatch.DataBind();
                        }

                        if (noflag == false)
                        {
                            Panel_sp2.Visible = false;
                            errlbl.Visible = true;
                            errlbl.Text = "No data found on that day";
                            bcntlbl.Visible = false;
                            btctxt.Visible = false;
                            bcntddl.Visible = false;
                            bcntddllbl.Visible = false;
                        }
                        else
                        {
                            batchpanel.Visible = true;
                            bcntlbl.Visible = true;
                            btctxt.Visible = true;
                            bcntddl.Visible = true;
                            bcntddllbl.Visible = true;
                            batch_spread.SaveChanges();
                            Panel_sp2.Visible = true;
                            errlbl.Visible = false;

                            enablesave();
                            maxrow = batch_spread.Sheets[0].RowCount;
                            loaddays();
                        }

                    }
                    else
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Sunday can not be accepted";
                        bcntlbl.Visible = false;
                        btctxt.Visible = false;
                        bcntddl.Visible = false;
                        bcntddllbl.Visible = false;
                        return;
                    }
                    if (Convert.ToInt32(batch_spread.Sheets[0].RowCount) > 0)
                    {
                        panel_sp1.Visible = true;
                        Double totalRows = 0;
                        totalRows = Convert.ToInt32(batch_spread.Sheets[0].RowCount);
                        Session["totalPages"] = (int)Math.Ceiling(totalRows / batch_spread.Sheets[0].PageSize);
                    }
                }
                else
                {
                    fmlbl.Visible = true;
                    fmlbl.Text = "Enter Valid date";
                }
            }
            else
            {
                fmlbl.Visible = true;
                fmlbl.Text = "Enter Valid date";
            }
            batch_spread.Sheets[0].SheetCorner.Cells[0, 0].BackColor = Color.AliceBlue;
            batch_spread.Sheets[0].PageSize = batch_spread.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
            errlbl.Text = ex.ToString();
            errlbl.Visible = true;
        }
    }

    public void enablesave()
    {
        if (CheckBox1.Checked == false)
        {
            btnsave.Enabled = false;
            btn2sv.Enabled = false;
            delbtn.Enabled = false;
        }
        else
        {
            btnsave.Enabled = true;
            btn2sv.Enabled = true;
            delbtn.Enabled = true;
        }
    }

    public void loaddays()
    {
        try
        {
            string scode = string.Empty;
            int l = 0;
            int intNHrs = 0;
            string[] WkArr = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            string strsec = string.Empty;
            string strsecval = string.Empty;
            int SchOrder = 0;
            int nodays = 0;
            string ini_day = string.Empty;
            string stt = string.Empty;
            string date1 = string.Empty;
            string fmdate = string.Empty;
            string sql = string.Empty;
            int IntRCtr = 0;
            int row = 0;
            string tagsub = string.Empty;
            Boolean flag = false;

            string sectionName = string.Empty;
            string todate = string.Empty;
            string startdate = string.Empty;
            string start_dayorder = string.Empty;
            string qrySection = string.Empty;
            if (ddlsec.Items.Count > 0)
            {
                string secValue = Convert.ToString(ddlsec.SelectedValue).Trim().ToLower();
                if (string.IsNullOrEmpty(secValue) || secValue == "all" || ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsec = string.Empty;
                    sectionName = string.Empty;
                    qrySection = string.Empty;
                }
                else
                {
                    sectionName = Convert.ToString(ddlsec.SelectedValue).Trim();
                    qrySection = " and LTRIM(RTRIM(ISNULL(ct.TT_sec,'')))='" + sectionName + "'";
                    strsec = " and LTRIM(RTRIM(ISNULL(registration.sections,'')))='" + sectionName + "'";
                }
            }

            sml_spread.Sheets[0].RowCount = 0;
            sml_spread.Sheets[0].ColumnCount = 0;

            string strsyllcode = d2.GetFunction("select syll_code from syllabus_master where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and Batch_Year = " + ddlbatch.SelectedValue.ToString() + "");
            if (strsyllcode.Trim() != "" && strsyllcode.Trim() != "0")
            {
                scode = strsyllcode;
                sml_spread.Visible = true;
                sml_spread.Sheets[0].ColumnCount += 1;
                sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                sml_spread.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Day";
                sml_spread.Sheets[0].ColumnCount += 1;
                sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                sml_spread.Sheets[0].Columns[sml_spread.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;

                sml_spread.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Hour";

                sml_spread.Sheets[0].Columns[0].Locked = true;
                sml_spread.Sheets[0].Columns[1].Locked = true;
                if (ddlsec.Items.Count > 0)
                {
                    string secValue = Convert.ToString(ddlsec.SelectedValue).Trim().ToLower();
                    if (string.IsNullOrEmpty(secValue) || secValue == "all" || ddlsec.SelectedValue.ToString() == "-1")
                    {
                        strsecval = string.Empty;
                    }
                    else
                    {
                        strsecval = " and LTRIM(RTRIM(ISNULL(sections,'')))='" + ddlsec.SelectedValue.ToString() + "'";
                    }
                }

                string strseminf = "Select No_of_hrs_per_day,schorder,nodays from periodattndschedule where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + "";
                DataSet dsseminf = d2.select_method_wo_parameter(strseminf, "text");
                if (dsseminf.Tables[0].Rows.Count > 0)
                {
                    if ((dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"].ToString()) != "")
                    {
                        intNHrs = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["No_of_hrs_per_day"]);
                        SchOrder = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["schorder"]);
                        nodays = Convert.ToInt16(dsseminf.Tables[0].Rows[0]["nodays"]);
                    }
                }

                date1 = txtFromDate.Text.ToString();
                string[] date_fm = date1.Split(new Char[] { '/' });
                fmdate = date_fm[2].ToString() + "/" + date_fm[1].ToString() + "/" + date_fm[0].ToString();
                Session["todate"] = fmdate.ToString();
                DateTime dt1 = Convert.ToDateTime(fmdate.ToString());

                string strstadquery = "select start_date,end_date,starting_dayorder from seminfo where degree_code=" + ddlbranch.SelectedValue.ToString() + " and semester=" + ddlduration.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + "";
                DataSet dsstar = d2.select_method_wo_parameter(strstadquery, "Text");
                if (dsstar.Tables[0].Rows.Count > 0)
                {
                    if ((dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "" && (dsstar.Tables[0].Rows[0]["start_date"].ToString()) != "\0")
                    {
                        start_dayorder = dsstar.Tables[0].Rows[0]["starting_dayorder"].ToString();
                        string[] tmpdate = dsstar.Tables[0].Rows[0]["start_date"].ToString().Split(new char[] { ' ' });
                        startdate = tmpdate[0].ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Update semester Information')", true);
                        return;
                    }
                }

                if (intNHrs > 0)
                {
                    if (SchOrder != 0)
                    {
                        ini_day = dt1.ToString("ddd");
                    }
                    else
                    {
                        todate = txtFromDate.Text.Trim().ToString();
                        string[] spd = todate.Split('/');
                        string curdate = spd[1] + '/' + spd[0] + '/' + spd[2];
                        ini_day = d2.findday(curdate, ddlbranch.SelectedItem.Value.ToString(), ddlduration.SelectedItem.ToString(), ddlbatch.SelectedItem.ToString(), startdate.ToString(), Convert.ToString(nodays), Convert.ToString(start_dayorder));//Added by Manikandan 25/07/2013
                    }
                }


                string getlabsub = " Select subjecT_no,subjecT_code from subject,sub_sem where sub_sem.subtype_no = subject.subtype_no and (sub_sem.Lab = 1 or sub_sem.projThe=1) and sub_sem.syll_code = subject.syll_code and subject.syll_code=" + scode.ToString() + "";
                DataSet dslasu = d2.select_method_wo_parameter(getlabsub, "Text");

                //sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";
                //DataSet dsalter = d2.select_method_wo_parameter(sql, "Text");

                #region New Altered batch allocation

                //Idhris
                string ttname = dirAcc.selectScalarString("select distinct tt_name,TT_date from tt_classtimetable ct where TT_colCode='" + collegecode + "' and TT_degCode='" + ddlbranch.SelectedValue + "' and TT_batchyear='" + ddlbatch.SelectedValue + "' and TT_sem='" + ddlduration.SelectedValue + "' " + qrySection + " and TT_date <='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2]) + "'  order by TT_date desc --(select max(TT_date) from tt_classtimetable where TT_colCode='" + collegecode + "' and TT_degCode='" + ddlbranch.SelectedValue + "' and TT_batchyear='" + ddlbatch.SelectedValue + "' and TT_sem='" + ddlduration.SelectedValue + "' " + qrySection + " and TT_date<='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2]) + "' )");

                DataTable dtSelect = dirAcc.selectDataTable("select TT_subno,TT_staffcode,TT_Day,TT_Hour,isnull(Lab,'0') as Lab,isnull(Elective,'0') as Elective,isnull(s.practicalPair,0) as practicalpair,do.Daydiscription,(select room_name from Room_Detail rd where rd.roompk = ctd.TT_Room ) as Room from TT_ClassTimetable ct, TT_altertimetabledet ctd,subject s,sub_sem ss,TT_Day_Dayorder do where ctd.TT_Day =do.TT_Day_DayorderPK and  s.subject_no = ctd.TT_subno and s.syll_code=ss.syll_code and s.subType_no = ss.subType_no and ct.TT_ClassPK = ctd.TT_ClassFk and isnull(Lab,'0')='1' and ct.TT_colCode='" + collegecode + "' and ct.TT_degCode='" + ddlbranch.SelectedValue + "' and ct.TT_batchyear='" + ddlbatch.SelectedValue + "' and ct.TT_sem='" + ddlduration.SelectedValue + "' " + qrySection + " and ct.TT_name='" + ttname + "'  and ctd.TT_Alterdate='" + (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[0] + "/" + txtFromDate.Text.Split('/')[2]) + "'  ");

                DataTable dtNewTT = new DataTable();
                dtNewTT.Columns.Add("degree_code");
                dtNewTT.Columns.Add("class_advisor");
                dtNewTT.Columns.Add("Semester");
                dtNewTT.Columns.Add("batch_year");
                dtNewTT.Columns.Add("TTName");
                dtNewTT.Columns.Add("FromDate");
                dtNewTT.Columns.Add("LastRec");
                dtNewTT.Columns.Add("Sections");

                DataRow drNewConstruct = dtNewTT.NewRow();
                drNewConstruct["degree_code"] = ddlbranch.SelectedValue;
                drNewConstruct["class_advisor"] = string.Empty;
                drNewConstruct["Semester"] = ddlduration.SelectedValue.ToString();
                drNewConstruct["batch_year"] = ddlbatch.SelectedValue;
                drNewConstruct["TTName"] = ttname.Trim();
                drNewConstruct["FromDate"] = (txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[1] + "/" + txtFromDate.Text.Split('/')[2]);
                drNewConstruct["LastRec"] = "1";
                drNewConstruct["Sections"] = sectionName;

                for (int i = 0; i < nodays; i++)
                {
                    string curday = string.Empty;
                    string curdayFull = string.Empty;
                    switch (i)
                    {
                        case 0:
                            curdayFull = "Monday";
                            curday = "mon";
                            break;
                        case 1:
                            curdayFull = "Tuesday";
                            curday = "tue";
                            break;
                        case 2:
                            curdayFull = "Wednesday";
                            curday = "wed";
                            break;
                        case 3:
                            curdayFull = "Thursday";
                            curday = "thu";
                            break;
                        case 4:
                            curdayFull = "Friday";
                            curday = "fri";
                            break;
                        case 5:
                            curdayFull = "Saturday";
                            curday = "sat";
                            break;
                        case 6:
                            curdayFull = "Sunday";
                            curday = "sun";
                            break;
                    }
                    for (int hrsI = 1; hrsI <= intNHrs; hrsI++)
                    {
                        dtNewTT.Columns.Add(curday + hrsI);

                        dtSelect.DefaultView.RowFilter = "Daydiscription='" + curdayFull + "' and TT_Hour='" + hrsI + "'";
                        DataTable dtFilter = dtSelect.DefaultView.ToTable();
                        if (dtFilter.Rows.Count > 0)
                        {
                            StringBuilder sbNew = new StringBuilder();
                            for (int dtI = 0; dtI < dtFilter.Rows.Count; dtI++)
                            {
                                string subno = Convert.ToString(dtFilter.Rows[dtI]["TT_subno"]);
                                string stfcode = Convert.ToString(dtFilter.Rows[dtI]["TT_staffcode"]);
                                string elect = Convert.ToString(dtFilter.Rows[dtI]["Elective"]);
                                string Lab = Convert.ToString(dtFilter.Rows[dtI]["Lab"]);
                                string practicalpair = Convert.ToString(dtFilter.Rows[dtI]["practicalpair"]);
                                string differenciator = "S";
                                string room = Convert.ToString(dtFilter.Rows[dtI]["room"]);
                                if (elect.ToLower() == "true")
                                {
                                    differenciator = "E";
                                }
                                else if (Lab.ToLower() == "true")
                                {
                                    differenciator = "L";
                                }
                                else if (practicalpair != "0")
                                {
                                    differenciator = "C";
                                }

                                sbNew.Append(subno + "-" + stfcode + "-" + differenciator + "-" + room + ";");
                            }
                            drNewConstruct[curday + hrsI] = sbNew.ToString();
                        }
                    }
                }
                dtNewTT.Rows.Add(drNewConstruct);

                DataSet dsalter = new DataSet();
                dsalter.Tables.Add(dtNewTT);
                #endregion

                DataTable dtv = dslasu.Tables[0];
                Hashtable hatsubject = new Hashtable();
                string validsunno = string.Empty;

                if (dsalter.Tables.Count > 0 && dsalter.Tables[0].Rows.Count <= 0)
                {
                    errlbl.Text = "No Records Found";
                    errlbl.Visible = true;
                    batchpanel.Visible = false;
                    return;
                }
                errlbl.Visible = false;
                if (intNHrs > 0 && nodays > 0 && nodays <= 7)
                {
                    for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
                    {
                        stt = ini_day + IntRCtr;

                        string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
                        string[] sp = schdeva.Split(';');
                        Boolean getflag = false;
                        string othsub = string.Empty;
                        for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
                        {
                            string val = sp[hr].ToString();
                            if (val.Trim() != "" && val != null)
                            {
                                string[] spsub = val.Split('-');
                                if (spsub.GetUpperBound(0) > 1)
                                {
                                    dtv.DefaultView.RowFilter = " subject_no='" + spsub[0] + "'";
                                    DataView dt = dtv.DefaultView;
                                    if (dt.Count > 0)
                                    {
                                        getflag = true;
                                    }
                                    if (othsub == "")
                                    {
                                        othsub = spsub[0];
                                    }
                                    else
                                    {
                                        othsub = othsub + ',' + spsub[0];
                                    }
                                }
                            }
                        }
                        if (getflag == true)
                        {
                            string[] val = othsub.Split(',');
                            for (int k = 0; k <= val.GetUpperBound(0); k++)
                            {
                                string gva = val[k];
                                if (!hatsubject.Contains(gva))
                                {
                                    hatsubject.Add(gva, stt);
                                    if (validsunno == "")
                                    {
                                        validsunno = gva;
                                    }
                                    else
                                    {
                                        validsunno = validsunno + ',' + gva;
                                    }
                                }
                                else
                                {
                                    string gphr = hatsubject[gva].ToString();
                                    gphr = gphr + ',' + stt;
                                    hatsubject[gva] = gphr;
                                }
                            }
                        }
                    }
                }
                DataSet ds_subjectnum = new DataSet();
                string subjectnumber = string.Empty;
                if (validsunno != "")
                {
                    subjectnumber = "select subjecT_no,subjecT_code from subject where subject_no in(" + validsunno + ")";

                }
                else
                {
                    subjectnumber = "select subjecT_no,subjecT_code from subject ";

                }
                ds_subjectnum = d2.select_method(subjectnumber, hat, "Text");
                for (int suc = 0; suc < ds_subjectnum.Tables[0].Rows.Count; suc++)
                {
                    sml_spread.Sheets[0].ColumnCount += 1;
                    sml_spread.Sheets[0].ColumnHeader.Cells[0, (sml_spread.Sheets[0].ColumnCount) - 1].Text = ds_subjectnum.Tables[0].Rows[suc]["Subject_Code"].ToString();
                    sml_spread.Sheets[0].ColumnHeader.Cells[0, (sml_spread.Sheets[0].ColumnCount) - 1].Tag = ds_subjectnum.Tables[0].Rows[suc]["subjecT_no"].ToString();
                }


                //sql = "select * from alternate_schedule where degree_Code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + fmdate.ToString() + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + strsecval + "";//this query modified by Manikandan from above commented query on 27/08/2013
                for (int i = 0; i < dsalter.Tables[0].Rows.Count; i++)
                {
                    if (intNHrs > 0 && nodays > 0 && nodays <= 7)
                    {
                        for (IntRCtr = 1; IntRCtr <= intNHrs; IntRCtr++)
                        {
                            stt = ini_day + IntRCtr;
                            string schdeva = dsalter.Tables[0].Rows[0][stt].ToString();
                            string[] sp = schdeva.Split(';');
                            Boolean getflag = false;
                            string othsub = string.Empty;
                            for (int hr = 0; hr <= sp.GetUpperBound(0); hr++)
                            {

                                string val = sp[hr].ToString();
                                if (val.Trim() != "" && val != null)
                                {
                                    string[] spsub = val.Split('-');
                                    if (spsub.GetUpperBound(0) > 1)
                                    {
                                        string subjectnu = spsub[0].ToString();
                                        Boolean valiflag = false;
                                        if (hatsubject.Contains(subjectnu))
                                        {
                                            string gethr = hatsubject[subjectnu].ToString();
                                            string[] spi = gethr.Split(',');
                                            for (int lo = 0; lo <= spi.GetUpperBound(0); lo++)
                                            {
                                                string valhr = spi[lo].ToString();
                                                if (valhr.Trim().ToLower() == stt.Trim().ToLower())
                                                {
                                                    valiflag = true;
                                                }
                                            }
                                        }
                                        int col_cntt = 0;

                                        if (sml_spread.Sheets[0].ColumnCount > 2)
                                        {
                                            for (col_cntt = 2; col_cntt < sml_spread.Sheets[0].ColumnCount; col_cntt++)
                                            {
                                                if (valiflag == true)
                                                {
                                                    if (col_cntt == 2 && hr == 0)
                                                    {
                                                        sml_spread.Sheets[0].RowCount = sml_spread.Sheets[0].RowCount + 1;
                                                    }
                                                    tagsub = sml_spread.Sheets[0].ColumnHeader.Cells[0, col_cntt].Tag.ToString();

                                                    if (subjectnu == tagsub)
                                                    {
                                                        Panel_sp2.Visible = true;
                                                        row = sml_spread.Sheets[0].RowCount;
                                                        sml_spread.Sheets[0].Cells[row - 1, 0].Text = ini_day;
                                                        sml_spread.Sheets[0].Cells[row - 1, 1].Text = IntRCtr.ToString();
                                                        sml_spread.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                                                        sml_spread.Sheets[0].Columns[0].Font.Size = FontUnit.Medium;

                                                        sml_spread.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                                                        sml_spread.Sheets[0].Columns[1].Font.Size = FontUnit.Medium;
                                                        FarPoint.Web.Spread.ComboBoxCellType chkcell = new FarPoint.Web.Spread.ComboBoxCellType();
                                                        sml_spread.Sheets[0].Columns[col_cntt].CellType = chkcell;

                                                        string strstubatchquery = "select distinct batch from subjectchooser_New,registration,sub_sem,subject Where subjectchooser_New.roll_no = registration.roll_no And registration.degree_code =" + ddlbranch.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " " + strsec.ToString() + " and sub_sem.lab=1 and subjectchooser_New.subtype_no=sub_sem.subtype_no and subjectchooser_New.subject_no=subject.subject_no and semester =" + ddlduration.SelectedValue.ToString() + " and LTRIM(RTRIM(ISNULL(Batch,'')))<>' ' and LTRIM(RTRIM(ISNULL(Batch,'')))<>'-1' and batch is not null and ltrim(rtrim((batch)))<>''";
                                                        DataSet bat_set = d2.select_method_wo_parameter(strstubatchquery, "Text");

                                                        FarPoint.Web.Spread.ComboBoxCellType batch_no = new FarPoint.Web.Spread.ComboBoxCellType();
                                                        batch_no.DataSource = bat_set;
                                                        batch_no.DataTextField = "batch";
                                                        batch_no.DataValueField = "batch";
                                                        sml_spread.ActiveSheetView.Cells[row - 1, col_cntt].CellType = batch_no;
                                                        sml_spread.Sheets[0].Cells[row - 1, col_cntt].CellType = batch_no;
                                                        sml_spread.Sheets[0].Cells[row - 1, col_cntt].BackColor = Color.CornflowerBlue;
                                                        sml_spread.SaveChanges();
                                                        flag = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < sml_spread.Sheets[0].RowCount; i++)
                {
                    for (int j = 0; j < sml_spread.Sheets[0].ColumnCount; j++)
                    {
                        if (sml_spread.ActiveSheetView.Cells[i, j].BackColor == Color.CornflowerBlue)
                        {
                            sml_spread.ActiveSheetView.Cells[i, j].Locked = false;
                        }
                        else
                        {
                            sml_spread.ActiveSheetView.Cells[i, j].Locked = true;
                        }
                    }
                }
            }
            if (flag == false)
            {
                Panel_sp2.Visible = false;
            }

            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

            int IntRowCtr = 0;
            string Day_Value = string.Empty;
            string Hour_Value = string.Empty;
            int intColCtr = 0;
            string SnoStr = string.Empty;
            string Setbatch1 = string.Empty;
            string strsecvall = string.Empty;

            if (ddlsec.Items.Count > 0)
            {
                string secValue = Convert.ToString(ddlsec.SelectedValue).Trim().ToLower();
                if (string.IsNullOrEmpty(secValue) || secValue == "all" || ddlsec.SelectedValue.ToString() == "-1")
                {
                    strsecvall = string.Empty;
                }
                else
                {
                    strsecvall = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                }
            }
            for (IntRowCtr = 0; IntRowCtr < sml_spread.Sheets[0].RowCount; IntRowCtr++)
            {
                Day_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 0].Text;
                Hour_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 1].Text;
                if (Hour_Value != "" && Day_Value != "")
                {
                    for (intColCtr = 2; intColCtr < sml_spread.Sheets[0].ColumnCount; intColCtr++)
                    {
                        Setbatch1 = string.Empty;
                        SnoStr = sml_spread.Sheets[0].ColumnHeader.Cells[0, intColCtr].Tag.ToString();
                        if (SnoStr != "")
                        {
                            string stubatchquery = "Select distinct stu_batch from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and Day_Value ='" + Day_Value + "' and Hour_Value = " + Hour_Value + "" + strsecvall + "  and fdate='" + fmdate.ToString() + "' and tdate='" + fmdate.ToString() + "' and  Subject_No = " + SnoStr.ToString() + "";
                            DataSet dsstubatch = d2.select_method_wo_parameter(stubatchquery, "Text");
                            if (dsstubatch.Tables[0].Rows.Count > 0)
                            {
                                for (int s = 0; s < dsstubatch.Tables[0].Rows.Count; s++)
                                {
                                    if (Setbatch1 == "")
                                    {
                                        Setbatch1 = dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                    }
                                    else
                                    {
                                        Setbatch1 = Setbatch1 + "," + dsstubatch.Tables[0].Rows[s]["Stu_Batch"].ToString();
                                    }
                                    if (sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].BackColor == Color.CornflowerBlue)//this line added by Manikandan 27/08/2013
                                    {
                                        string[] spiltbatch = Setbatch1.Split(',');
                                        if (spiltbatch.GetUpperBound(0) > 0)
                                        {
                                            sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].CellType = txt;
                                            sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text = Setbatch1;
                                        }
                                        else
                                        {
                                            sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text = Setbatch1;
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        int maxrow = 0;
        maxrow = batch_spread.Sheets[0].RowCount;
        if (maxrow != 0 && CheckBox1.Checked == true)
        {
            sfrlbl.Enabled = true;
            sfmtxt.Enabled = true;
            stolbl.Enabled = true;
            stotxt.Enabled = true;
            sfmtxt.Text = string.Empty;
            stotxt.Text = string.Empty;
            maxrow = batch_spread.Sheets[0].RowCount;
            selbtn.Enabled = true;
            btnsave.Enabled = true;
            delbtn.Enabled = true;
            btn2sv.Enabled = true;
        }
        else
        {
            sfrlbl.Enabled = false;
            sfmtxt.Enabled = false;
            stolbl.Enabled = false;
            stotxt.Enabled = false;
            selbtn.Enabled = false;
            sfmtxt.Text = string.Empty;
            stotxt.Text = string.Empty;

            btnsave.Enabled = false;
            delbtn.Enabled = false;
            btn2sv.Enabled = false;
        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            batch_spread.CurrentPage = 0;
            Boolean selectflag = false;
            int isval;
            string col_val = string.Empty;
            Boolean savflag = false;

            bcntlbl.Visible = true;
            btctxt.Visible = true;
            bcntddl.Visible = true;
            bcntddllbl.Visible = true;

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());

            if (btctxt.Text != "")
            {
                string x = string.Empty;
                x = bcntddl.SelectedIndex.ToString();
                if (bcntddl.SelectedIndex.ToString() != "0" && bcntddl.SelectedIndex.ToString() != "" && bcntddl.SelectedIndex.ToString() != "-1" && bcntddl.SelectedIndex.ToString() != "--Select--")
                {
                    batch_spread.SaveChanges();
                    if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
                    {
                        int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
                        int totxt = Convert.ToInt32(stotxt.Text.ToString());

                        for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
                        {
                            if (readspread <= batch_spread.Sheets[0].RowCount - 1)
                            {
                                batch_spread.Sheets[0].Cells[readspread, 0].Value = 1;
                            }
                        }
                    }
                    if (bcntddl.Items.Count == 0 || bcntddl.Enabled == false)
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Please select the batch";
                        return;
                    }

                    for (int i = 0; i < batch_spread.Sheets[0].RowCount; i++)
                    {
                        isval = Convert.ToInt32(batch_spread.Sheets[0].Cells[i, 0].Value);
                        if (isval == 1)
                        {
                            btnsave.Enabled = true;
                            btn2sv.Enabled = true;
                            delbtn.Enabled = true;
                            selectflag = true;
                            col_val = batch_spread.Sheets[0].Cells[i, 1].Text;
                            string strdelsubnew = "delete subjectchooser_New  where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                            int insupaddelquery = d2.update_method_wo_parameter(strdelsubnew, "Text");

                            batch_spread.Sheets[0].Cells[i, 6].Text = bcntddl.SelectedValue.ToString();
                            selectflag = true;
                            savflag = true;

                            for (int k = 2; k < sml_spread.Sheets[0].ColumnCount; k++)
                            {
                                string Subno = sml_spread.Sheets[0].ColumnHeader.Cells[0, k].Tag.ToString();
                                string subtypeno = d2.GetFunction("select subtype_no from subject where subject_no='" + Subno + "'");

                                string insupdaquery = "if not exists(Select * from subjectchooser_New where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and subject_no='" + Subno + "')";
                                insupdaquery = insupdaquery + " insert into subjectchooser_New(semester,roll_no,subject_no,subtype_no,batch,fromdate,todate)values('" + ddlduration.SelectedValue.ToString() + "','" + col_val.ToString() + "','" + Subno.ToString() + "','" + subtypeno.ToString() + "','" + bcntddl.SelectedValue.ToString() + "','" + dtf.ToString("MM/dd/yyyy") + "','" + dtf.ToString("MM/dd/yyyy") + "')";
                                insupdaquery = insupdaquery + " else update subjectchooser_New set batch='" + bcntddl.SelectedValue.ToString() + "'  where roll_no='" + col_val.ToString() + "'and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and subject_no='" + Subno + "'";

                                insupaddelquery = d2.update_method_wo_parameter(insupdaquery, "Text");
                            }
                        }
                    }
                    if (selectflag == false)
                    {
                        errlbl.Visible = true;
                        errlbl.Text = "Please select atleast one student";
                    }

                    if (savflag == true)
                    {
                        errlbl.Visible = true;
                        sfmtxt.Text = string.Empty;
                        stotxt.Text = string.Empty;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Batch allocated successfully')", true);
                        loadbatch();


                        bcntddl.Items.Clear();
                        string numbatch = string.Empty;
                        int b_val = 0;
                        numbatch = btctxt.Text.ToString();
                        if (numbatch != "" && numbatch != "0")
                        {
                            bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                            for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                            {
                                bcntddl.Items.Add("B" + b_val.ToString());
                            }
                        }
                    }
                    batch_spread.SaveChanges();
                }
                else
                {
                    errlbl.Visible = true;
                    errlbl.Text = "Please select the batch";
                }
            }
            else
            {
                errlbl.Visible = true;
                errlbl.Text = "Please select the no of batches";
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void btn2sv_Click(object sender, EventArgs e)
    {
        try
        {
            int intColCtr = 0;
            int IntRowCtr = 0;
            string Day_Value = string.Empty;
            int intSi = 0;
            string Hour_Value = string.Empty;
            string SnoStr = string.Empty;
            string strSchText = string.Empty;
            string strsec = string.Empty;
            string Stu_B = string.Empty;
            string sql = string.Empty;
            int i = 0;
            Boolean savflag = false;

            string sec = string.Empty;
            if (ddlsec.Enabled == false)
            {
                strsec = string.Empty;
            }

            else if (ddlsec.SelectedValue.ToString() != "-1" && ddlsec.SelectedValue.ToString().Trim() != "")
            {
                strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
                sec = ddlsec.SelectedValue.ToString();
            }
            else
            {
                strsec = string.Empty;
            }

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());

            if (sml_spread.Sheets[0].RowCount > 0 && sml_spread.Sheets[0].ColumnCount > 2)
            {
                sql = "Delete from LabAlloc_New where degree_code = '" + ddlbranch.SelectedValue.ToString() + "' and semester = '" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and Batch_Year ='" + ddlbatch.SelectedValue.ToString() + "' and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int dellabnew = d2.update_method_wo_parameter(sql, "Text");
            }

            sml_spread.SaveChanges();
            for (IntRowCtr = 0; IntRowCtr < sml_spread.Sheets[0].RowCount; IntRowCtr++)//--------------------row increment
            {
                Day_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 0].Text;
                Hour_Value = sml_spread.Sheets[0].Cells[IntRowCtr, 1].Text;
                if (Day_Value != "" && Hour_Value != "")
                {
                    for (intColCtr = 2; intColCtr < sml_spread.Sheets[0].ColumnCount; intColCtr++)//--col increment
                    {
                        if (sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text != "" && sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text != null)
                        {
                            SnoStr = sml_spread.Sheets[0].ColumnHeader.Cells[0, intColCtr].Tag.ToString();
                            Stu_B = sml_spread.Sheets[0].Cells[IntRowCtr, intColCtr].Text;
                            if (Stu_B.Trim() != "" && Stu_B != null)
                            {
                                string[] Stu_Batch = Stu_B.Split(new Char[] { ',' });

                                if (Stu_Batch.GetUpperBound(0) >= 0)
                                {
                                    for (i = 0; i <= Stu_Batch.GetUpperBound(0); i++)
                                    {
                                        if (SnoStr.ToString().Trim() != "" && Stu_Batch[i].ToString().Trim() != "" && SnoStr != null && Stu_Batch[i] != null)
                                        {
                                            sql = "select " + Day_Value + "" + Hour_Value + " from alternate_schedule where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and fromdate='" + dtf.ToString("MM/dd/yyyy") + "' and  batch_year = " + ddlbatch.SelectedValue.ToString() + "" + strsec + "  and  " + Day_Value + "" + Hour_Value + " is not null";
                                            DataSet dsalsem = d2.select_method_wo_parameter(sql, "Text");
                                            if (dsalsem.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsalsem.Tables[0].Rows[0][0].ToString().Trim() != "" && dsalsem.Tables[0].Rows[0][0] != null)
                                                {
                                                    strSchText = dsalsem.Tables[0].Rows[0][0].ToString();
                                                    string[] ArSchText1 = strSchText.Split(new Char[] { ';' });
                                                    if (ArSchText1.GetUpperBound(0) >= 0)
                                                    {
                                                        for (intSi = 0; intSi <= ArSchText1.GetUpperBound(0); intSi++)
                                                        {
                                                            if (ArSchText1[intSi].ToString().Trim() != "" && ArSchText1[intSi] != null)
                                                            {
                                                                string[] CntSchText1 = ArSchText1[intSi].Split(new Char[] { '-' });
                                                                if (CntSchText1.GetUpperBound(0) >= 0)
                                                                {
                                                                    if (CntSchText1[0].ToString() == SnoStr)
                                                                    {
                                                                        string strquer = "if not exists (Select * from LabAlloc_New where degree_code = " + ddlbranch.SelectedValue.ToString() + " and semester = " + ddlduration.SelectedValue.ToString() + " and batch_year = " + ddlbatch.SelectedValue.ToString() + " and staff_code='" + CntSchText1[1].ToString() + "' and Day_Value = '" + Day_Value.ToString() + "' and Hour_Value = " + Hour_Value.ToString() + " and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "' and SubjecT_no =" + SnoStr.ToString() + " " + strsec + " and Stu_Batch='" + Stu_Batch[i].ToString() + "')";
                                                                        strquer = strquer + " insert into LabAlloc_New(Degree_Code,Semester,Batch_Year,Sections,Subject_No,Day_Value,Hour_Value,Stu_Batch,Staff_Code,fdate,tdate)values('" + ddlbranch.SelectedValue.ToString() + "','" + ddlduration.SelectedValue.ToString() + "','" + ddlbatch.SelectedValue.ToString() + "','" + sec + "','" + SnoStr.ToString() + "','" + Day_Value.ToString() + "','" + Hour_Value.ToString() + "','" + Stu_Batch[i].ToString() + "','" + CntSchText1[1].ToString() + "','" + dtf.ToString("MM/dd/yyyy") + "','" + dtf.ToString("MM/dd/yyyy") + "')";
                                                                        int updsin = d2.update_method_wo_parameter(strquer, "Text");
                                                                        savflag = true;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (savflag == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Batch allocated Successfully')", true);
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void selbtn_Click(object sender, EventArgs e)
    {
        try
        {
            string frm_date = string.Empty;
            string to_date = string.Empty;
            int maxrow = 0;
            int i = 0;

            frm_date = sfmtxt.Text;
            to_date = stotxt.Text;
            maxrow = batch_spread.Sheets[0].RowCount;

            if (sfmtxt.Text == "")
            {
                errlbl.Visible = true;
                errlbl.Text = "Please enter from value";
                sfmtxt.Focus();
            }

            if (stotxt.Text == "")
            {
                errlbl.Visible = true;
                errlbl.Text = "Please enter to value";
                stotxt.Focus();
            }

            if (Convert.ToInt16(frm_date.ToString()) <= 0)
            {
                errlbl.Visible = true;
                errlbl.Text = "From value cannot be less than 1";
                sfmtxt.Text = maxrow.ToString();
            }


            if (Convert.ToInt16(frm_date.ToString()) > maxrow)
            {
                errlbl.Visible = true;
                errlbl.Text = "From value cannot be greater than total no of students";
                sfmtxt.Text = maxrow.ToString();
            }

            if (Convert.ToInt16(to_date.ToString()) <= 0)
            {
                errlbl.Visible = true;
                errlbl.Text = "To value cannot be less than 1";
                stotxt.Text = maxrow.ToString();
            }

            if (Convert.ToInt16(to_date.ToString()) > maxrow)
            {
                errlbl.Visible = true;
                errlbl.Text = "To value cannot be greater than total no of students";
                sfmtxt.Text = maxrow.ToString();
            }


            for (i = Convert.ToInt16(frm_date); i <= Convert.ToInt16(to_date); i++)
            {
                batch_spread.Sheets[0].Cells[i - 1, 0].Value = 1;
            }

            if (Convert.ToInt16(frm_date.ToString()) > Convert.ToInt16(to_date.ToString()))
            {
                errlbl.Visible = true;
                errlbl.Text = "From value cannot be greater than To value";
                sfmtxt.Focus();
            }

            bcntddl.Items.Clear();
            string numbatch = string.Empty;
            int b_val = 0;
            numbatch = btctxt.Text.ToString();
            if (numbatch != "" && numbatch != "0")
            {
                bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                {
                    bcntddl.Items.Add("B" + b_val.ToString());

                }
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void delbtn_Click(object sender, EventArgs e)
    {
        try
        {
            string SqlStr = string.Empty;
            int isval = 0;
            string roll_no = string.Empty;
            batch_spread.CurrentPage = 0;
            string strsec = string.Empty;
            string sql = string.Empty;
            Boolean delsml = false;
            Boolean blnDelete = false;

            string date1 = txtFromDate.Text.ToString();
            string[] date_fm = date1.Split(new Char[] { '/' });
            DateTime dtf = Convert.ToDateTime(date_fm[1].ToString() + "/" + date_fm[0].ToString() + "/" + date_fm[2].ToString());

            batch_spread.SaveChanges();
            if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
            {
                int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
                int totxt = Convert.ToInt32(stotxt.Text.ToString());

                for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
                {
                    if (readspread <= batch_spread.Sheets[0].RowCount - 1)
                    {
                        batch_spread.Sheets[0].Cells[readspread, 0].Value = 1;
                    }
                }
            }

            for (int i = 0; i < batch_spread.Sheets[0].RowCount; i++)
            {
                isval = Convert.ToInt32(batch_spread.Sheets[0].Cells[i, 0].Value);
                if (isval == 1)
                {
                    if (batch_spread.Sheets[0].Cells[i, 6].Text != "")
                    {
                        blnDelete = true;
                        roll_no = batch_spread.Sheets[0].Cells[i, 1].Text;
                        SqlStr = "delete subjectchooser_New  where roll_no='" + roll_no + "' and semester='" + ddlduration.SelectedItem.ToString() + "' and fromdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                        int delequery = d2.update_method_wo_parameter(SqlStr, "Text");
                    }
                }
            }

            if (ddlsec.Enabled == false)
            {
                strsec = string.Empty;
            }

            else if (ddlsec.SelectedValue.ToString() != "-1" && ddlsec.SelectedValue.ToString().Trim() != "")
            {
                strsec = " and sections='" + ddlsec.SelectedValue.ToString() + "'";
            }
            else
            {
                strsec = string.Empty;
            }

            if (sml_spread.Sheets[0].RowCount > 0 && sml_spread.Sheets[0].ColumnCount > 2)
            {
                sql = "Delete from LabAlloc_New where degree_code = '" + ddlbranch.SelectedValue.ToString() + "' and semester = '" + ddlduration.SelectedValue.ToString() + "' " + strsec.ToString() + " and Batch_Year ='" + ddlbatch.SelectedValue.ToString() + "' and fdate='" + dtf.ToString("MM/dd/yyyy") + "' and tdate='" + dtf.ToString("MM/dd/yyyy") + "'";
                int delquery = d2.update_method_wo_parameter(sql, "Text");
                delsml = true;
            }
            if (blnDelete == true || delsml == true)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Deleted successfully')", true);
            }
            bcntddl.Items.Clear();
            string numbatch = string.Empty;
            int b_val = 0;
            numbatch = btctxt.Text.ToString();
            if (numbatch != "" && numbatch != "0")
            {
                bcntddl.Items.Insert(0, new ListItem("--Select--", "-1"));
                for (b_val = 1; b_val <= Convert.ToInt16(numbatch.ToString()); b_val++)
                {
                    bcntddl.Items.Add("B" + b_val.ToString());
                }
            }
            loadbatch();
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        ddlsec.Items.Clear();
        ddlsec.Items.Insert(0, new ListItem("--Select--", "-1"));
        ddlsec.Enabled = true;
    }

    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        seclbl.Visible = false;
    }

    protected void txtFromDate_TextChanged(object sender, EventArgs e)
    {
    }

    protected void bcntddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
    }

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void stotxt_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (sfmtxt.Text != "" && sfmtxt.Text != "0" && stotxt.Text != "" && stotxt.Text != "0")
            {
                if (Convert.ToInt32(sfmtxt.Text) <= Convert.ToInt32(stotxt.Text))
                {
                    if (Convert.ToInt32(stotxt.Text) <= batch_spread.Sheets[0].RowCount)
                    {
                        errlbl.Visible = false;
                        int frmtxt = Convert.ToInt32(sfmtxt.Text.ToString());
                        int totxt = Convert.ToInt32(stotxt.Text.ToString());

                        for (int readspread = frmtxt - 1; readspread < totxt; readspread++)
                        {
                            if (readspread <= batch_spread.Sheets[0].RowCount - 1)
                            {
                                batch_spread.Sheets[0].Cells[readspread, 0].Value = 1;
                            }
                        }
                    }
                    else
                    {
                        errlbl.Text = "Only " + batch_spread.Sheets[0].RowCount + " are available";
                        errlbl.Visible = true;
                        stotxt.Text = string.Empty;
                    }
                }
            }
            btnsave.Enabled = true;
            delbtn.Enabled = true;
            btn2sv.Enabled = true;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        try
        {
            int ar = 0;
            int ac = 0;
            string value = string.Empty;
            ar = sml_spread.ActiveSheetView.ActiveRow;
            ac = sml_spread.ActiveSheetView.ActiveColumn;
            if (ac > 1)
            {
                Checkboxlistbatch.Visible = true;
                Button3.Visible = true;
                Fieldset5.Visible = true;
                string batchbb = sml_spread.Sheets[0].Cells[ar, ac].Text;


                string[] batc = batchbb.Split(',');
                if (batc.GetUpperBound(0) > 0)
                {
                    for (int uu = 0; uu <= batc.GetUpperBound(0); uu++)
                    {
                        string bvv = batc[uu].ToString();
                        for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
                        {
                            value = Checkboxlistbatch.Items[i].Text;

                            if (bvv == value)
                            {
                                Checkboxlistbatch.Items[i].Selected = true;
                            }

                        }
                    }
                }
                else
                {
                    for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
                    {
                        value = Checkboxlistbatch.Items[i].Text;

                        if (batchbb == value)
                        {
                            Checkboxlistbatch.Items[i].Selected = true;
                        }
                        else
                        {
                            Checkboxlistbatch.Items[i].Selected = false;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void Checkboxlistbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string value = string.Empty;
            string code = string.Empty;

            for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
            {
                if (Checkboxlistbatch.Items[i].Selected == true)
                {
                    value = Checkboxlistbatch.Items[i].Text;
                    code = Checkboxlistbatch.Items[i].Value.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void Button3_Click(object sender, EventArgs e)
    {
        try
        {
            string value = string.Empty;
            string code = string.Empty;
            string batchva = string.Empty;
            sml_spread.SaveChanges();

            for (int i = 0; i < Checkboxlistbatch.Items.Count; i++)
            {
                if (Checkboxlistbatch.Items[i].Selected == true)
                {
                    value = Checkboxlistbatch.Items[i].Text;
                    code = Checkboxlistbatch.Items[i].Value.ToString();
                    if (batchva == "")
                    {
                        batchva = value;
                    }
                    else
                    {
                        batchva = batchva + ',' + value;
                    }
                }
            }
            int ar = 0;
            int ac = 0;
            ar = sml_spread.ActiveSheetView.ActiveRow;
            ac = sml_spread.ActiveSheetView.ActiveColumn;

            if (ac > 1)
            {
                if (sml_spread.Sheets[0].Cells[ar, ac].BackColor == Color.CornflowerBlue)
                {
                    FarPoint.Web.Spread.TextCellType btva = new FarPoint.Web.Spread.TextCellType();
                    sml_spread.Sheets[0].Cells[ar, ac].CellType = btva;
                    sml_spread.Sheets[0].Cells[ar, ac].Text = batchva;
                    sml_spread.Sheets[0].Cells[ar, ac].Locked = true;
                    Checkboxlistbatch.Visible = false;
                }
            }

            Button3.Visible = false;
            Fieldset5.Visible = false;
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

    protected void batch_spread_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            Boolean actflag = false;
            string actrow = e.CommandArgument.ToString();
            if (bcntddl.SelectedIndex.ToString() != "0" && bcntddl.SelectedIndex.ToString() != "" && bcntddl.SelectedIndex.ToString() != "-1" && bcntddl.SelectedIndex.ToString() != "--Select--")
            {
                for (int i = 0; i < batch_spread.Sheets[0].RowCount; i++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(batch_spread.Sheets[0].Cells[i, 0].Value);
                    if (isval == 1)
                    {
                        actflag = true;
                        i = batch_spread.Sheets[0].RowCount;
                    }
                }
                string val = e.EditValues[0].ToString();
                if (val.Trim().ToLower() == "true")
                {
                    actflag = true;
                }
                if (actflag == true)
                {
                    btnsave.Enabled = true;
                    delbtn.Enabled = true;
                }

            }
        }
        catch (Exception ex)
        {
            errlbl.Visible = true;
            errlbl.Text = ex.ToString();
        }
    }

}
















