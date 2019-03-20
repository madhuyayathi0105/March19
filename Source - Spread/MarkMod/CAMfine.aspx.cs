using System;//-----------------modified on 26.04.12
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
//using System.Windows.Forms;
using System.Data.SqlClient;
using System.Drawing;

public partial class CAMfine : System.Web.UI.Page
{
    string strsec;
    string code;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection mycon2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection readcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd;
    string exampresent = "";
    string atten;
    string text = "";
    string strdayflag;
    string regularflag = "";
    string genderflag = "";
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string strorder = "";
    Boolean cellclick;
    DataSet ds_load = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

    //--------------new for printmaster start 25.04.12
    DataSet dsprint = new DataSet();
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string form_heading_name = "";
    string batch_degree_branch = "";
    int chk_secnd_clmn = 0;
    int right_logo_clmn = 0;
    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    //'----------------end printmaster
    //[Serializable()]
    //public class MyImg : ImageCellType
    //{
    //    public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
    //    {

    //        //'------------clg left logo
    //        System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
    //        img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img.Width = Unit.Percentage(100);
    //        img.Height = Unit.Percentage(50);

    //        return img;

    //        //'-------------clg right logo
    //        System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
    //        img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
    //        img2.Width = Unit.Percentage(100);
    //        img2.Height = Unit.Percentage(50);

    //        return img2;

    //    }
    //}

    public DataSet Bind_Degree(string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    public DataSet Bind_Dept(string degree_code, string college_code, string user_code)
    {
        SqlConnection dcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
        dcon.Close();
        dcon.Open();
        SqlCommand cmd = new SqlCommand("select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id= " + degree_code + " and degree.college_code=" + college_code + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + user_code + "", dcon);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        return ds;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }  
            lblnorec.Visible = false;
            if (!Page.IsPostBack)
            {
                //finesettings();
                string mrange1;
                TextBox1.Attributes.Add("Readonly", "Readonly");
                mrange1 = "select * from failfine";
                btnmasterprint.Visible = false;
                con.Close();
                con.Open();
                SqlDataAdapter adaSyll1 = new SqlDataAdapter(mrange1, con);
                DataSet ds = new DataSet();
                adaSyll1.Fill(ds, "ds");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Ddlrange.Items.Clear();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Ddlrange.Items.Add(ds.Tables[0].Rows[i]["range"].ToString());

                    }
                }
                else
                {
                    Ddlrange.Items.Add("0-9");
                    Ddlrange.Items.Add("10-20");
                    Ddlrange.Items.Add("20-40");
                    Ddlrange.Items.Add("Absentees");
                }
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                Session["Studflag"] = "0";
                try
                {
                    btnmasterprint.Visible = false;

                    terminalbtn.Checked = true;
                    acronymradio.Checked = true;
                    btnExcel.Visible = false;
                    //Added By Srinath 27/2/2013
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;

                    collegecode = Session["collegecode"].ToString();
                    usercode = Session["usercode"].ToString();
                    singleuser = Session["single_user"].ToString();
                    group_user = Session["group_code"].ToString();

                    //'@@@@@@@@@@@@@@@@ new mythili 23.04.12 for display the header
                    FpEntry.Sheets[0].ColumnHeader.RowCount = 2;
                    FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                    style1.Font.Size = 10;
                    style1.Font.Bold = true;
                    style1.Font.Size = FontUnit.Medium;
                    style1.Font.Name = "Book Antiqua";
                    style1.HorizontalAlign = HorizontalAlign.Center;
                    style1.ForeColor = Color.Black;
                    style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpEntry.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpEntry.Sheets[0].AllowTableCorner = true;
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";

                    FpEntry.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    FpEntry.Sheets[0].DefaultStyle.Font.Bold = false;
                    FpEntry.CommandBar.Visible = true;

                    FpEntry.Sheets[0].AutoPostBack = true;
                    FpEntry.Sheets[0].Columns[0].Width = 120;
                    FpEntry.Sheets[0].Columns[1].Width = 120;

                    FpEntry.Sheets[0].Columns[2].Width = 200;
                    FpEntry.Sheets[0].Columns[3].Width = 250;

                    FarPoint.Web.Spread.IntegerCellType intgrcel3 = new FarPoint.Web.Spread.IntegerCellType();
                    FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();

                    intgrcel3.ErrorMessage = "Enter valid fine amount";
                    intgrcel3.MaximumValue = 100000;
                    FpSpread1.Sheets[0].Columns[1].CellType = intgrcel3;
                    FpSpread1.Sheets[0].Columns[2].CellType = intgrcel3;
                    FpEntry.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                    FpEntry.Sheets[0].AllowTableCorner = true;
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
                    FpEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 2, 1);
                    FpEntry.Sheets[0].SheetCorner.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = style1;
                    FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                    FpSpread1.CommandBar.Visible = false;

                    //@@@@@@@@@@
                    FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0].Text = "S.No";
                    FpEntry.Sheets[0].SheetCornerSpanModel.Add(FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2, 2, 1);
                    FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3, 2, 1);
                    FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0].Border.BorderColorBottom = Color.White;

                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = "Roll No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Bold = true;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Name = "Book Antiqua";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Text = "Admission No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Bold = true;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Name = "Book Antiqua";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Text = "Reg No";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Bold = true;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Name = "Book Antiqua";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Text = "Student Name";
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Bold = true;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Size = FontUnit.Medium;
                    FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Name = "Book Antiqua";

                    FpEntry.Sheets[0].SheetCorner.Rows[FpEntry.Sheets[0].SheetCorner.RowCount - 2].BackColor = Color.AliceBlue;
                    FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");


                    //@@@@@@@@@
                    if (Session["usercode"] != "")
                    {
                        string Master1 = "";
                        Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                        readcon.Close();
                        readcon.Open();
                        SqlDataReader mtrdr;

                        SqlCommand mtcmd = new SqlCommand(Master1, readcon);
                        mtrdr = mtcmd.ExecuteReader();
                        strdayflag = "";
                        while (mtrdr.Read())
                        {
                            if (mtrdr.HasRows == true)
                            {
                                if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Rollflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Regflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                                {
                                    Session["Studflag"] = "1";
                                }
                                if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                                {
                                    strdayflag = " and (registration.Stud_Type='Day Scholar'";
                                }
                                if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                                {
                                    if (strdayflag != "" && strdayflag != "\0")
                                    {
                                        strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                                    }
                                    else
                                    {
                                        strdayflag = " and (registration.Stud_Type='Hostler'";
                                    }
                                }
                                if (mtrdr["settings"].ToString() == "Regular")
                                {
                                    regularflag = "and ((registration.mode=1)";

                                    // Session["strvar"] = Session["strvar"] + " and (mode=1)";
                                }
                                if (mtrdr["settings"].ToString() == "Lateral")
                                {
                                    if (regularflag != "")
                                    {
                                        regularflag = regularflag + " or (registration.mode=3)";
                                    }
                                    else
                                    {
                                        regularflag = regularflag + " and ((registration.mode=3)";
                                    }
                                    //Session["strvar"] = Session["strvar"] + " and (mode=3)";
                                }
                                if (mtrdr["settings"].ToString() == "Transfer")
                                {
                                    if (regularflag != "")
                                    {
                                        regularflag = regularflag + " or (registration.mode=2)";
                                    }
                                    else
                                    {
                                        regularflag = regularflag + " and ((registration.mode=2)";
                                    }

                                }

                                if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                                {
                                    genderflag = " and (sex='0'";
                                }
                                if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                                {
                                    if (genderflag != "" && genderflag != "\0")
                                    {
                                        genderflag = genderflag + " or sex='1'";
                                    }
                                    else
                                    {
                                        genderflag = " and (sex='1'";
                                    }
                                }
                            }
                        }
                        if (strdayflag != "")
                        {
                            strdayflag = strdayflag + ")";
                        }
                        Session["strvar"] = strdayflag;
                        if (regularflag != "")
                        {
                            regularflag = regularflag + ")";
                        }
                        Session["strvar"] = Session["strvar"] + regularflag;
                        if (genderflag != "")
                        {
                            genderflag = genderflag + ")";
                        }
                        Session["strvar"] = Session["strvar"] + regularflag + genderflag;
                    }

                    //------------condn for print master
                    if (Request.QueryString["val"] != null)
                    {
                        string get_pageload_value = Request.QueryString["val"];
                        if (get_pageload_value.ToString() != null)
                        {
                            string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                            string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val
                            bindbatch();
                            ddlBatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());
                            binddegree();
                            ddlDegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                            if (ddlDegree.Text != "")
                            {
                                bindbranch();
                                ddlBranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                            }
                            else
                            {
                                lblnorec.Text = "Give degree rights to the staff";
                                lblnorec.Visible = true;
                            }
                            //bind semester
                            bindsem();
                            ddlSem.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                            //bind section
                            bindsec();
                            ddlSec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());
                            //bing test
                            GetTest();
                            ddlTest.SelectedIndex = Convert.ToInt32(spl_pageload_val[5].ToString());
                            //bind subject
                            GetSubject();
                            string[] spl_criteria_val = spl_load_val[1].Split('-');
                            if (spl_criteria_val.GetUpperBound(0) > 0)
                            {
                                for (int crt = 0; crt < spl_criteria_val.GetUpperBound(0) + 1; crt++)
                                {
                                    for (int xx = 0; xx < ddlSubject.Items.Count; xx++)
                                    {
                                        if (ddlSubject.Items[xx].Value == spl_criteria_val[crt].ToString())
                                        {
                                            ddlSubject.Items[xx].Selected = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int crt = 0; crt < spl_criteria_val.GetUpperBound(0) + 1; crt++)
                                {
                                    for (int xx = 0; xx < ddlSubject.Items.Count; xx++)
                                    {
                                        if (ddlSubject.Items[xx].Value == spl_criteria_val[0].ToString())
                                        {
                                            ddlSubject.Items[xx].Selected = true;
                                        }
                                    }
                                }
                            }

                            btnGo_Click(sender, e);
                            // func_Print_Master_Setting();

                            //function_footer();
                        }
                    }
                    else
                    {
                        bindbatch();
                        binddegree();
                        if (ddlDegree.Text != "")
                        {
                            bindbranch();
                        }
                        else
                        {
                            lblnorec.Text = "Give degree rights to the staff";
                            lblnorec.Visible = true;
                        }
                        //bind semester
                        bindsem();
                        //bind section
                        bindsec();
                        //bing test
                        GetTest();
                        //bind subject
                        GetSubject();

                    }
                }
                catch
                {
                }
            }

            FpSpread1.Attributes.Add("onmouseup", "__doPostBack('FpSpread1','OnPreRender,' + FpSpread1.ActiveRow + ',' + FpSpread1.ActiveCol)");
        }
        catch (Exception ex) { }
    }

    //'----------------------func for footer
    public void function_footer()
    {
        //----------------start for setting the footer
        if (dsprint.Tables[0].Rows[0]["footer"].ToString() != "")
        {

            footer_count = Convert.ToInt16(dsprint.Tables[0].Rows[0]["footer"].ToString());
            FpEntry.Sheets[0].RowCount += 3;
            footer_text = dsprint.Tables[0].Rows[0]["footer_name"].ToString();
            string[] footer_text_split = footer_text.Split(',');

            int count_span = FpEntry.Sheets[0].ColumnCount / footer_count;

            if (footer_text_split.GetUpperBound(0) > 0)
            {
                for (footer_balanc_col = 0; footer_balanc_col < footer_text_split.GetUpperBound(0) + 1; footer_balanc_col++)
                {
                    if (footer_balanc_col == 0)
                    {
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Text = footer_text_split[footer_balanc_col].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col].Font.Bold = true;
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, footer_balanc_col, 1, count_span + 1);
                    }
                    else
                    {
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Text = footer_text_split[footer_balanc_col].ToString();
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Size = FontUnit.Medium;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Font.Bold = true;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].HorizontalAlign = HorizontalAlign.Left;

                        //@@@@@@@@@ set the row border color white in footer
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorBottom = Color.White;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span].Border.BorderColorTop = Color.White;
                        FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.White;
                        //@@@@@@@ span the columns for foote text
                        FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, footer_balanc_col + count_span, 1, FpEntry.Sheets[0].ColumnCount);
                    }

                }
            }
            else
            {
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Text = footer_text;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Font.Bold = true;
                FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;

            }
            FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 3, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
            //   FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 2, 0, 1, FpEntry.Sheets[0].ColumnCount);
            FpEntry.Sheets[0].SpanModel.Add(FpEntry.Sheets[0].RowCount - 1, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 3, 0].Border.BorderColor = Color.White;
            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 2, 0].Border.BorderColor = Color.White;
            FpEntry.Sheets[0].Cells[FpEntry.Sheets[0].RowCount - 1, 0].Border.BorderColor = Color.White;
        }
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            if (dsprint.Tables[0].Rows[0]["column_fields"].ToString() == string.Empty)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Select Atleast One Column From The TreeView";
                FpEntry.Visible = false;
                btnmasterprint.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                //TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
            }
            else
            {
                lblnorec.Visible = false;
                lblnorec.Text = "";
                FpEntry.Visible = true;
                btnmasterprint.Visible = true;
                //Buttontotal.Visible = true;
                //lblrecord.Visible = true;
                //DropDownListpage.Visible = true;
                //TextBoxother.Visible = true;
                ////lblpage.Visible = true;
                //TextBoxpage.Visible = true;
                btnExcel.Visible = true;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                FpEntry.Height = 600;


            }
        }
    }
    public void func_Print_Master_Setting()
    {

        hat.Clear();
        hat.Add("college_code", Session["collegecode"].ToString());
        hat.Add("form_name", "CAMfine.aspx");
        dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        if (dsprint.Tables[0].Rows.Count > 0)
        {
            for (int newlp = 0; newlp <= FpEntry.Sheets[0].ColumnCount - 1; newlp++)
            {
                FpEntry.Sheets[0].Columns[newlp].Visible = false;
            }


            if (dsprint.Tables[0].Rows.Count > 0)
            {
                if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != string.Empty)
                {
                    string new_hdr_text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                    string[] spl_hdr_text = new_hdr_text.Split(',');
                    if (spl_hdr_text.GetUpperBound(0) > 0)
                    {
                        FpEntry.Sheets[0].ColumnHeader.RowCount += spl_hdr_text.GetUpperBound(0) + 1;
                    }
                    else
                    {
                        FpEntry.Sheets[0].ColumnHeader.RowCount++;
                    }
                }
                FpEntry.Height = 600;
            }

            string printvar = "";
            int span_cnt = 0;
            int col_count = 0;
            int child_span_count = 0;
            printvar = dsprint.Tables[0].Rows[0]["column_fields"].ToString();
            string[] split_printvar = printvar.Split(',');
            for (int splval = 0; splval <= split_printvar.GetUpperBound(0); splval++)
            {
                span_cnt = 0;
                string[] split_star = split_printvar[splval].Split('*');
                if (split_star.GetUpperBound(0) > 0)
                {
                    for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount - 1; col_count++)
                    {
                        if (FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Text == split_star[0])//(FpEntry.Sheets[0].ColumnHeader.RowCount - 4)
                        {
                            child_span_count = 0;
                            string[] split_star_doller = split_star[1].Split('$');
                            for (int doller_count = 1; doller_count < split_star_doller.GetUpperBound(0); doller_count++)
                            {
                                for (int child_node = col_count; child_node < col_count + split_star_doller.GetUpperBound(0) - 1; child_node++)
                                {
                                    if (FpEntry.Sheets[0].ColumnHeader.Cells[7, child_node].Text == split_star_doller[doller_count])//(FpEntry.Sheets[0].ColumnHeader.RowCount - 3)
                                    {
                                        span_cnt++;
                                        if (span_cnt == 1 && child_node == col_count + 1)
                                        {
                                            // FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, col_count + 1].Text = split_star[0].ToString();//(FpEntry.Sheets[0].ColumnHeader.RowCount - 4)
                                            col_count++;
                                        }

                                        if (child_node != col_count)
                                        {
                                            span_cnt = child_node - (child_span_count - 1);
                                        }
                                        else
                                        {
                                            child_span_count = col_count;
                                        }
                                        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount -2,child_node,1,2);
                                        // FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, child_node].Text = split_star[0].ToString();
                                        // FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, child_node].Text = split_star_doller[doller_count].ToString();//(FpEntry.Sheets[0].ColumnHeader.RowCount - 4)
                                        FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), child_node].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                        FpEntry.Sheets[0].Columns[child_node].Visible = true;

                                        final_print_col_cnt++;
                                        if (span_cnt == split_star_doller.GetUpperBound(0) - 1)
                                        {
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (col_count = 0; col_count < FpEntry.Sheets[0].ColumnCount; col_count++)
                    {
                        if (FpEntry.Sheets[0].ColumnHeader.Cells[6, col_count].Text == split_printvar[splval])//(FpEntry.Sheets[0].ColumnHeader.RowCount - 4)
                        {
                            FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0].Text = "S.No";
                            FpEntry.Sheets[0].SheetCornerSpanModel.Add(FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0, 2, 1);
                            FpEntry.Sheets[0].SheetCorner.Cells[(FpEntry.Sheets[0].SheetCorner.RowCount - 1), 0].BackColor = Color.AliceBlue;
                            FpEntry.Sheets[0].SheetCorner.Cells[(FpEntry.Sheets[0].SheetCorner.RowCount - 1), 0].Border.BorderColorBottom = Color.White;
                            FpEntry.Sheets[0].SheetCorner.Cells[(FpEntry.Sheets[0].SheetCorner.RowCount - 1), 0].Border.BorderColorTop = Color.White;


                            FpEntry.Sheets[0].Columns[col_count].Visible = true;
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, col_count, 2, 1);//FpEntry.Sheets[0].ColumnHeader.RowCount - 4
                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count].Text = split_printvar[splval].ToString();//(FpEntry.Sheets[0].ColumnHeader.RowCount - 4)
                            final_print_col_cnt++;
                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Text = " ";
                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].Border.BorderColorTop = Color.White;
                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 1), col_count].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            FpEntry.Sheets[0].ColumnHeader.Cells[(FpEntry.Sheets[0].ColumnHeader.RowCount - 2), col_count].Border.BorderColorBottom = Color.White;
                            break;
                        }
                    }
                }
            }


            //---------------- setting the new header name text start
            if (dsprint.Tables[0].Rows[0]["new_header_name"].ToString() != "")
            {
                //-----check the columns which one is first visible
                for (int chk_clm_vsbl = 0; chk_clm_vsbl < FpEntry.Sheets[0].ColumnCount; chk_clm_vsbl++)
                {
                    if (FpEntry.Sheets[0].Columns[chk_clm_vsbl].Visible == true)
                    {
                        int strwindexcnt = 1;
                        string new_hdr_text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                        string[] spl_hdr_text = new_hdr_text.Split(',');
                        if (spl_hdr_text.GetUpperBound(0) > 0)
                        {
                            for (int strw = 6; strw < FpEntry.Sheets[0].ColumnHeader.RowCount - 2; strw++)
                            {
                                if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
                                {
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].Text = spl_hdr_text[strwindexcnt - 1].ToString();
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Left;

                                }
                                else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
                                {
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].Text = spl_hdr_text[strwindexcnt - 1].ToString();
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Center;

                                }
                                else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
                                {
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].Text = spl_hdr_text[strwindexcnt - 1].ToString();
                                    FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Right;

                                }
                                FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(strw, chk_clm_vsbl, 1, FpEntry.Sheets[0].ColumnCount);
                                strwindexcnt++;

                                FpEntry.Sheets[0].ColumnHeader.Cells[strw, chk_clm_vsbl].Border.BorderColorBottom = Color.Black;
                            }
                        }
                        else
                        {
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 3, chk_clm_vsbl].Text = dsprint.Tables[0].Rows[0]["new_header_name"].ToString();
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 3, chk_clm_vsbl, 1, FpEntry.Sheets[0].ColumnCount);
                            if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Left")
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 3, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Center")
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 3, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (dsprint.Tables[0].Rows[0]["header_align"].ToString() == "Right")
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 3, chk_clm_vsbl].HorizontalAlign = HorizontalAlign.Right;
                            }

                        }
                        break;
                    }
                }//end for chk first visible
                //'--------------end new hdr text
            }//end for chk_clm_visible
            //---------------------------------------------------------------------
        }
    }
    public void func_final_row_header()
    {

        FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0].Text = "S.No";
        FpEntry.Sheets[0].SheetCornerSpanModel.Add(FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0, 2, 1);
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1, 2, 1);
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2, 2, 1);
        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3, 2, 1);
        FpEntry.Sheets[0].SheetCorner.Cells[FpEntry.Sheets[0].SheetCorner.RowCount - 2, 0].Border.BorderColorBottom = Color.White;

        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Text = "Roll No";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Text = "Admission No";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 1].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Text = "Reg No";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 2].Font.Name = "Book Antiqua";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Text = "Student Name";
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].HorizontalAlign = HorizontalAlign.Center;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Bold = true;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Size = FontUnit.Medium;
        FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 3].Font.Name = "Book Antiqua";

        FpEntry.Sheets[0].SheetCorner.Rows[FpEntry.Sheets[0].SheetCorner.RowCount - 2].BackColor = Color.AliceBlue;
        FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 2].BackColor = ColorTranslator.FromHtml("#0CA6CA");
        FpEntry.Sheets[0].ColumnHeader.Rows[FpEntry.Sheets[0].ColumnHeader.RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");
    }
    public void function_header()
    {
        //hat.Clear();
        //hat.Add("college_code", Session["collegecode"].ToString());
        //hat.Add("form_name", "CAMfine.aspx");
        //dsprint = d2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");
        //if (dsprint.Tables[0].Rows.Count > 0)
        //{

        //    if (dsprint.Tables[0].Rows[0]["college_name"].ToString() != string.Empty)
        //    {
        //        collnamenew1 = dsprint.Tables[0].Rows[0]["college_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address1"].ToString() != "")
        //    {
        //        address1 = dsprint.Tables[0].Rows[0]["address1"].ToString();
        //        address = address1;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["address2"].ToString() != "")
        //    {
        //        address2 = dsprint.Tables[0].Rows[0]["address2"].ToString();
        //        address = address1 + "-" + address2;

        //    }
        //    if (dsprint.Tables[0].Rows[0]["address3"].ToString() != "")
        //    {
        //        district = dsprint.Tables[0].Rows[0]["address3"].ToString();
        //        address = address1 + "-" + address2 + "-" + district;
        //    }

        //    if (dsprint.Tables[0].Rows[0]["phoneno"].ToString() != "")
        //    {
        //        Phoneno = dsprint.Tables[0].Rows[0]["phoneno"].ToString();
        //        phnfax = "Phone :" + " " + Phoneno;
        //    }
        //    if (dsprint.Tables[0].Rows[0]["faxno"].ToString() != "")
        //    {
        //        Faxno = dsprint.Tables[0].Rows[0]["faxno"].ToString();
        //        phnfax = phnfax + "Fax  :" + " " + Faxno;
        //    }

        //    if ((dsprint.Tables[0].Rows[0]["email"].ToString() != ""))
        //    {
        //        email = "E-Mail:" + dsprint.Tables[0].Rows[0]["email"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["website"].ToString() != "")
        //    {
        //        email = email + " " + "Web Site:" + dsprint.Tables[0].Rows[0]["website"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["form_heading_name"].ToString() != "")
        //    {
        //        form_heading_name = dsprint.Tables[0].Rows[0]["form_heading_name"].ToString();
        //    }
        //    if (dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString() != "")
        //    {
        //        batch_degree_branch = dsprint.Tables[0].Rows[0]["batch_degree_branch"].ToString();
        //    }

        //}
        ////'------------------------------------load the clg information
        //else if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        //{
        //    string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
        //    SqlCommand collegecmd = new SqlCommand(college, con);
        //    SqlDataReader collegename;
        //    con.Close();
        //    con.Open();
        //    collegename = collegecmd.ExecuteReader();
        //    if (collegename.HasRows)
        //    {

        //        while (collegename.Read())
        //        {
        //            collnamenew1 = collegename["collname"].ToString();
        //            address1 = collegename["address1"].ToString();
        //            address2 = collegename["address2"].ToString();
        //            district = collegename["district"].ToString();
        //            address = address1 + "-" + address2 + "-" + district;
        //            Phoneno = collegename["phoneno"].ToString();
        //            Faxno = collegename["faxno"].ToString();
        //            phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
        //            email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
        //        }
        //    }
        //    con.Close();
        //}


        //for (int hdr_col = 0; hdr_col < FpEntry.Sheets[0].ColumnCount; hdr_col++)
        //{
        //    if (final_print_col_cnt == 1)
        //    {
        //        if (FpEntry.Sheets[0].Columns[hdr_col].Visible == true)
        //        {
        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].Text = batch_degree_branch;
        //            FpEntry.Width = 500;

        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorRight = Color.White;

        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Border.BorderColorBottom = Color.White;


        //            break;

        //        }
        //    }

        //    else if (final_print_col_cnt == FpEntry.Sheets[0].ColumnCount)
        //    {
        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = collnamenew1;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Text = address;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Text = phnfax;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Text = email;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Text = form_heading_name;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Text = batch_degree_branch;

        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);
        //        FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, 0, 1, FpEntry.Sheets[0].ColumnCount - 1);


        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorRight = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorRight = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorRight = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorRight = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;

        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].HorizontalAlign = HorizontalAlign.Center;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[5, 0].HorizontalAlign = HorizontalAlign.Center;

        //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorBottom = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[3, 0].Border.BorderColorBottom = Color.White;
        //        FpEntry.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorBottom = Color.White;


        //        break;

        //    }
        //    else if (final_print_col_cnt < FpEntry.Sheets[0].ColumnCount)
        //    {
        //        if (FpSpread1.Sheets[0].Columns[hdr_col].Visible == true)
        //        {

        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(1, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(2, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(3, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(4, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);
        //            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(5, hdr_col, 1, FpEntry.Sheets[0].ColumnCount - 2);



        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Text = collnamenew1;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Text = address;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Text = phnfax;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Text = email;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Text = form_heading_name;


        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].HorizontalAlign = HorizontalAlign.Center;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[6, hdr_col].HorizontalAlign = HorizontalAlign.Center;

        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Border.BorderColorRight = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[5, hdr_col].Border.BorderColorRight = Color.White;

        //            FpEntry.Sheets[0].ColumnHeader.Cells[0, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[1, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[2, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[3, hdr_col].Border.BorderColorBottom = Color.White;
        //            FpEntry.Sheets[0].ColumnHeader.Cells[4, hdr_col].Border.BorderColorBottom = Color.White;


        //            break;

        //        }
        //    }
        //}



        //FpEntry.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        //FpEntry.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        //FpEntry.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorBottom = Color.White;
        //FpEntry.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorBottom = Color.White;
        //FpEntry.Sheets[0].ColumnHeader.Rows[4].Border.BorderColorBottom = Color.White;


    }
    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load = d2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlBatch.DataSource = ds_load;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlBatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
    }
    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = group_semi[0].ToString();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);

        ds_load = d2.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }
    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
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
        ds_load = d2.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }
    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load = d2.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds_load;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {

        //Modified by Srinath 27/2/2013
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            string[] reportnamespilt = reportname.Split(' ');
            int upperebound = Convert.ToInt32(reportnamespilt.GetUpperBound(0));
            if (upperebound == 0)
            {
                lblnorec.Visible = false;
                lblnorec.Text = "";
                d2.printexcelreport(FpEntry, reportname);
                txtexcelname.Text = "";


            }
            else
            {
                lblnorec.Text = "Please Don't Give Space Just Add Special Character In Your Report Name";
                lblnorec.Visible = true;
            }
        }
        else
        {
            lblnorec.Text = "Please Enter Your Report Name";
            lblnorec.Visible = true;
        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        //Control cntUpdateBtn = FpSpread1.FindControl("Update");
        //Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        //Control cntCopyBtn = FpSpread1.FindControl("Copy");
        //Control cntCutBtn = FpSpread1.FindControl("Clear");
        //Control cntPasteBtn = FpSpread1.FindControl("Paste");
        Control cntPageNextBtn = FpEntry.FindControl("Next");
        Control cntPagePreviousBtn = FpEntry.FindControl("Prev");
        // Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntPageNextBtn != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            //tc = (TableCell)cntCancelBtn.Parent;
            //tr.Cells.Remove(tc);


            //tc = (TableCell)cntCopyBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntCutBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPasteBtn.Parent;
            //tr.Cells.Remove(tc);

            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

            ////tc = (TableCell)cntPagePrintBtn.Parent;
            ////tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }

    public void GetTest()
    {
        con.Close();
        con.Open();
        string SyllabusYr;
        string SyllabusQry;
        SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
        SyllabusYr = GetFunction(SyllabusQry.ToString());

        string Sqlstr;
        Sqlstr = "";

        if (SyllabusYr != null && SyllabusYr != "")
        {
            Sqlstr = "select criteria,criteria_no from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " order by criteria";
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();
            con.Close();
            con.Open();
            sqlAdapter1.Fill(titles);
            ddlTest.DataSource = titles;
            ddlTest.DataValueField = "Criteria_No";
            ddlTest.DataTextField = "Criteria";
            ddlTest.DataBind();
            //ddlTest.Items.Insert(0, new ListItem("--Select--", "-1"));
        }
        else
        {
            ddlTest.Items.Clear();
        }
    }

    public string GetFunction(string sqlQuery)
    {

        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
        cmd.Connection = con;
        con.Open();
        drnew = cmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }

    }



    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        ddlSubject.Items.Clear();
        TextBox1.Text = "";

        ddlBranch.Items.Clear();
        con.Open();
        string course_id = ddlDegree.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        bindbranch();
        //bind semester
        bindsem();
        //bind section
        bindsec();
        //bing test
        GetTest();
        //bind subject
        GetSubject();

    }
    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;

        ddlSubject.Items.Clear();
        TextBox1.Text = "";

        bindsem();
        //bind section
        bindsec();
        //bing test
        GetTest();
        //bind subject
        GetSubject();
        if (!Page.IsPostBack == false)
        {
            ddlSem.Items.Clear();
        }
        try
        {
            if (ddlBranch.SelectedIndex.ToString() != "")
            {
                bindsem();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    public void BindSectionDetail()
    {
        //string branch = ddlBranch.SelectedValue.ToString();
        //string batch = ddlBatch.SelectedValue.ToString();
        //DataSet ds = ClsAttendanceAccess.GetsectionDetail(batch.ToString(), branch.ToString());
        //if (ds.Tables[0].Rows.Count > 0)
        //{


        //    ddlSec.DataSource = ds;
        //    ddlSec.DataTextField = "Sections";
        //    ddlSec.DataValueField = "Sections";
        //    ddlSec.DataBind();
        //    ddlSec.Items.Insert(0, new ListItem("--Select--", "-1"));
        //}
        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con.Close();
        con.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        //ddlSec.Items.Insert(0, new ListItem("", "-1"));
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //Label8.Visible = false;
                GetTest();
            }
            else
            {
                ddlSec.Enabled = true;

            }
        }
        else
        {
            ddlSec.Enabled = false;
            //Label8.Visible = false;
            GetTest();
        }
    }
    public void bindsem()
    {

        //--------------------semester load
        ddlSem.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Close();
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSem.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }

            }
        }
        else
        {
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            ddlSem.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());

                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSem.Items.Add(i.ToString());
                    }
                }
            }

            dr1.Close();
        }
        //FpMarkEntry.Visible = false;
        con.Close();
    }
    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        string collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        //ddlSem.Items.Insert(0, new ListItem("", "-1"));
        if (ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            ddlSem.Items.Clear();
            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSem.Items.Add(i.ToString());

                }
                else if (first_year == true && i != 2)
                {
                    ddlSem.Items.Add(i.ToString());
                }

            }
        }
    }


    public void SpreadBind()
    {

        try
        {

            SqlDataReader reader;
            string SyllabusYr;
            string SyllabusQry;
            string sqlTest = "";
            string strsec = "";
            string sections = "";
            string batch = "";
            string degreecode = "";
            string subno = "";
            string semester = "";
            string display = "";
            string exam_code = "";
            string criteria_no = "";
            int rowcnt = 0;
            string rollno = "";
            string resmaxmrk = "";
            string resminmrk = "";
            string resduration = "";
            string resnewmaxmrk = "";
            string resnewminmrk = "";
            string bindnote = "";
            string criteria = "";
            string subject_code = "";
            string subject_name = "";
            string fine = "";
            string fineset = "";
            string marks = "";
            string sptvar = "";
            string code = "";
            string chkmark = "";
            string total = "";
            int res = 0;
            int colfine = 0;
            int countval = 0;
            string sp = "";
            string sp1 = "";
            int flag = 0;

            string examdate = "";
            string gdate = "";
            string gmonth = "";
            string gyear = "";
            string monthyear = "";
            string Date = "";
            int present = 0;
            string leavetype = "";

            batch = ddlBatch.SelectedValue.ToString();
            degreecode = ddlDegree.SelectedValue.ToString();
            sections = ddlSec.SelectedValue.ToString();
            semester = ddlSem.SelectedValue.ToString();
            criteria_no = ddlTest.SelectedValue.ToString();

            //FpEntry.Sheets[0].FrozenColumnCount = 5;
            //FpEntry.Sheets[0].AutoPostBack = false;
            //FpEntry.Width = 750;

            //ADDED BY GOWTHAM

            string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

            if (orderby_Setting == "")
            {
                strorder = "ORDER BY registration.Roll_No";
            }
            else
            {
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY registration.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY Registration.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No,Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY Registration.Reg_No,Registration.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY Registration.Roll_No,Registration.Stud_Name";
                }
            }

            /////


            FpEntry.Sheets[0].RowCount = 0;
            FpEntry.Sheets[0].RowCount += 1;
            string bind = "";
            SyllabusQry = "select syll_code from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
            SyllabusYr = GetFunction(SyllabusQry.ToString());
            string sqlStr = "";


            if (!Page.IsPostBack == false)
            {
                if (sections.ToString() == "All" || sections.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = "and exam_type.sections='" + sections.ToString() + "'";
                }
            }
            if ((Session["Staff_Code"].ToString() == "") || (Session["Staff_Code"].ToString() != ""))
            {

                for (int i = 0; i < ddlSubject.Items.Count; i++)
                {
                    if (ddlSubject.Items[i].Selected == true)
                    {
                        if (code == "")
                        {
                            // FpEntry.Width = 750;
                            code = ddlSubject.Items[i].Value;
                            text = ddlSubject.Items[i].Text;
                            TextBox1.Text = text;
                        }
                        else
                        {
                            //  FpEntry.Width = 1100;
                            code = code + "," + ddlSubject.Items[i].Value;
                            TextBox1.Text = text + "," + ddlSubject.Items[i].Text;
                        }
                    }
                }

                //'------------------------------------------- Query for Displaying the STUDENT DETAILS
                //sqlStr = "select distinct len(registration.Roll_No) as len_rollno, registration.Roll_No as RollNumber, registration.Reg_No as RegistrationNumber,registration.stud_name as Student_Name,registration.stud_type as StudentType,registration.App_No as ApplicationNumber from registration, applyn a where a.app_no=registration.app_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + "   and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + "  and RollNo_Flag<>0 and cc=0 and exam_flag <> 'DEBAR' and delflag=0  " + strsec + " " + Session["strvar"] + " order by  roll_no,len_rollno ";
                //  sqlStr = "Select distinct result.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.stud_type from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") order by result.roll_no ";
                //    sqlStr = "Select distinct len(result.roll_no) ,result.roll_no as roll,registration.Roll_Admit as adm_no,registration.reg_no as regno,registration.stud_name as studname,registration.stud_type as studtype from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") order by result.roll_no,len(result.roll_no) ";//new
                //added by gowtham:



                if (CheckBox1.Checked != true)
                {
                    sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.stud_type from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") " + strorder + "";
                }
                else
                {
                    string listrange = Ddlrange.SelectedValue.ToString();
                    if (listrange != "" && listrange != null)
                    {
                        if (listrange != "Absentees")
                        {

                            string[] listsplit = listrange.Split(new Char[] { '-' });

                            sp = listsplit[0].ToString();

                            sp1 = listsplit[1].ToString();
                            sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + " " + Session["strvar"] + " and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                        }
                        else if (listrange == "Absentees")
                        {
                            //  string list1 = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,case when result.marks_obtained=-1 then 'AAA' end as absencount from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + " and result.marks_obtained= -1  and exam_type.subjecT_no in(" + code + " )  and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' " + strorder + "";
                            sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained = -1 " + Session["strvar"] + " and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                        }
                    }


                }
            }
            else if (Session["Staff_Code"].ToString() != "")
            {
                //    sqlStr = "Select distinct len(result.roll_no),result.roll_no as roll,registration.Roll_Admit as adm_no,registration.reg_no as regno,registration.stud_name as studname,registration.stud_type as studtype from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") order by result.roll_no,len(result.roll_no) "; //new 
                //sqlStr = "Select distinct result.roll_no ,registration.Roll_Admit,registration.reg_no ,registration.stud_name,registration.stud_type from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") order by result.roll_no ";

                //Added by gowtham



                if (CheckBox1.Checked != true)
                {
                    sqlStr = "Select distinct result.roll_no ,registration.roll_no,registration.Roll_Admit,registration.reg_no ,registration.stud_name,registration.stud_type from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") " + strorder + "";

                }
                else
                {
                    string listrange = Ddlrange.SelectedValue.ToString();
                    if (listrange != "" && listrange != null)
                    {
                        if (listrange != "Absentees")
                        {
                            string[] listsplit = listrange.Split(new Char[] { '-' });

                            sp = listsplit[0].ToString();

                            sp1 = listsplit[1].ToString();
                            sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + "  and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                        }
                        else if (listrange == "Absentees")
                        {
                            //  string list1 = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,case when result.marks_obtained=-1 then 'AAA' end as absencount from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + " and result.marks_obtained= -1  and exam_type.subjecT_no in(" + code + " )  and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' " + strorder + "";
                            sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained = -1 and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                        }
                    }


                }
                con.Close();
            }
            //if (CheckBox1.Checked != true)
            //{

            con.Close();
            lblnorec.Visible = false;
            //Buttontotal.Visible = true;
            //lblrecord.Visible = true;
            //DropDownListpage.Visible = true;
            ////TextBoxother.Visible = true;
            //lblpage.Visible = true;
            //TextBoxpage.Visible = true;
            FpEntry.Visible = true;
            btnmasterprint.Visible = true;
            btnExcel.Visible = true;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            if (sqlStr != "")
            {
                FpEntry.Sheets[0].ColumnCount = 5;
                FpEntry.Sheets[0].RowCount = 0;
                con.Open();
                SqlDataAdapter adaSyll = new SqlDataAdapter(sqlStr, con);
                DataSet ds = new DataSet();
                adaSyll.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
                    {

                        FpEntry.Sheets[0].RowCount++;
                        FpEntry.Sheets[0].Rows[irow].Border.BorderColor = Color.Black;
                        FpEntry.Sheets[0].Cells[irow, 1].Text = ds.Tables[0].Rows[irow]["Roll_Admit"].ToString();
                        FpEntry.Sheets[0].Cells[irow, 0].Text = ds.Tables[0].Rows[irow]["roll_no"].ToString();
                        FpEntry.Sheets[0].Cells[irow, 2].Text = ds.Tables[0].Rows[irow]["reg_no"].ToString();
                        FpEntry.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["stud_name"].ToString();
                        FpEntry.Sheets[0].Cells[irow, 4].Text = ds.Tables[0].Rows[irow]["stud_type"].ToString();
                        lblnorec.Text = "";
                        lblnorec.Visible = false;

                    }
                }


                FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
                FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;



                if (sections.ToString() == "All" || sections.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and exam_type.sections='" + sections.ToString() + "'";
                }
                if (code != null && code != "" && SyllabusYr != null && SyllabusYr != "")
                {
                    sqlTest = "select distinct exam_type.subject_no,s.subject_code,exam_type.min_mark as minmark,s.subject_name,s.acronym,exam_type.exam_date From subject as s,subjectchooser as sc,exam_type where  s.subject_no = sc.subject_no and s.subject_no=exam_type.subject_no and s.syll_code= " + SyllabusYr + "  and exam_type.subjecT_no  in( " + code + " ) " + strsec + " and sc.semester =" + ddlSem.SelectedValue.ToString() + " and exam_type.criteria_no=" + ddlTest.SelectedValue.ToString() + " and exam_type.subjecT_no in(" + code + ") Order by subject_name";

                    SqlCommand cmd = new SqlCommand(sqlTest, con);
                    cmd.Connection = con;
                    reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        flag = 1;

                        while (reader.Read())
                        {
                            FpEntry.Sheets[0].ColumnHeader.RowCount = 2;
                            string acro = "";
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 4].Text = "Student Type";
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 4, 2, 1);
                            FpEntry.Sheets[0].Columns[4].Width = 150;
                            rowcnt = Convert.ToInt32(FpEntry.Sheets[0].RowCount) - 1;
                            FpEntry.Sheets[0].ColumnCount = Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) + 2;
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, FpEntry.Sheets[0].ColumnCount - 2, 1, 2);
                            FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 60;
                            FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 2].Width = 60;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Text = "Fine";
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Text = "Mark";
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].HorizontalAlign = HorizontalAlign.Center;

                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Bold = true;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Size = FontUnit.Medium;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Name = "Book Antiqua";
                            //FpEntry.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Right;

                            examdate = reader["exam_date"].ToString();
                            subject_name = reader["subject_name"].ToString();
                            acro = reader["acronym"].ToString();

                            if (SubjectRadio.Checked == true)
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Text = Convert.ToString(subject_name);
                            }
                            else if (acronymradio.Checked == true)
                            {
                                FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Text = Convert.ToString(acro);
                            }

                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Font.Bold = true;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Font.Size = FontUnit.Medium;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2].Font.Name = "Book Antiqua";
                            FpEntry.SaveChanges();

                            for (res = 0; res <= Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1; res++)
                            {
                                int colco = 0;
                                colco = Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2;
                                colfine = Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1;
                                rollno = FpEntry.Sheets[0].Cells[res, 0].Text;
                                string resultmark = "";
                                if (rollno != "" && rollno != null)
                                {
                                    if (CheckBox1.Checked != true)
                                    {
                                        resultmark = "Select result.roll_no,result.marks_obtained, registration.Stud_Type,registration.app_no from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subjecT_no =" + reader["subject_no"] + " " + strsec + " and exam_type.criteria_no=" + ddlTest.SelectedValue.ToString() + " " + Session["strvar"] + "and result.roll_no='" + rollno + "'";
                                    }
                                    else if (CheckBox1.Checked == true)
                                    {
                                        string listrange = Ddlrange.SelectedValue.ToString();
                                        if (listrange != "" && listrange != null)
                                        {
                                            if (listrange != "Absentees")
                                            {
                                                string[] listsplit = listrange.Split(new Char[] { '-' });

                                                sp = listsplit[0].ToString();

                                                sp1 = listsplit[1].ToString();
                                                // sqlStr = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + "  and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                                                resultmark = "Select result.roll_no,result.marks_obtained, registration.Stud_Type,registration.app_no from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subjecT_no =" + reader["subject_no"] + " " + strsec + " and exam_type.criteria_no=" + ddlTest.SelectedValue.ToString() + " " + Session["strvar"] + " and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + " and result.roll_no='" + rollno + "'";
                                            }
                                            else if (listrange == "Absentees")
                                            {
                                                //  string list1 = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,case when result.marks_obtained=-1 then 'AAA' end as absencount from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + " and result.marks_obtained= -1  and exam_type.subjecT_no in(" + code + " )  and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' " + strorder + "";
                                                sqlStr = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "   and result.marks_obtained = -1 and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
                                                resultmark = "Select result.roll_no,result.marks_obtained, registration.Stud_Type,registration.app_no from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + " and exam_type.subjecT_no =" + reader["subject_no"] + " " + strsec + " and exam_type.criteria_no=" + ddlTest.SelectedValue.ToString() + " " + Session["strvar"] + " and result.marks_obtained= -1 and result.roll_no='" + rollno + "'";
                                            }
                                        }
                                    }
                                    SqlCommand command1 = new SqlCommand(resultmark, mycon);
                                    command1.Connection = mycon;

                                    mycon.Open();
                                    SqlDataReader resreader;
                                    resreader = command1.ExecuteReader();
                                    while (resreader.Read())
                                    {
                                        if (resreader.HasRows == true)
                                        {
                                            string mark = resreader["marks_obtained"].ToString();
                                            FpEntry.Sheets[0].Cells[res, colco].HorizontalAlign = HorizontalAlign.Center;
                                            if (mark != null && mark != "")
                                            {
                                                switch (mark)
                                                {
                                                    case "-1":

                                                        mark = "AAA";
                                                        break;
                                                    case "-2":
                                                        mark = "EL";
                                                        break;
                                                    case "-3":
                                                        mark = "EOD";
                                                        break;
                                                    case "-4":
                                                        mark = "ML";
                                                        break;
                                                    case "-5":
                                                        mark = "SOD";
                                                        break;
                                                    case "-6":
                                                        mark = "NSS";
                                                        break;
                                                    case "-7":
                                                        mark = "NJ";
                                                        break;
                                                    case "-8":
                                                        mark = "S";
                                                        break;
                                                    case "-9":
                                                        mark = "L";
                                                        break;
                                                    case "-10":
                                                        mark = "NCC";
                                                        break;
                                                    case "-11":
                                                        mark = "HS";
                                                        break;
                                                    case "-12":
                                                        mark = "PP";
                                                        break;
                                                    case "-13":
                                                        mark = "SYOD";
                                                        break;
                                                    case "-14":
                                                        mark = "COD";
                                                        break;
                                                    case "-15":
                                                        mark = "OOD";
                                                        break;
                                                    case "-16":
                                                        mark = "OD";
                                                        break;
                                                    case "-17":
                                                        mark = "LA";
                                                        break;

                                                    //*Added By subburaj 21.08.2014*******
                                                    case "-18":
                                                        mark = "RAA";
                                                        break;
                                                    //***END***********//
                                                }
                                            }//end loop for mark!=null

                                            FpEntry.Sheets[0].Cells[res, colco].Text = mark.ToString();
                                            FpEntry.Sheets[0].Cells[res, colco].HorizontalAlign = HorizontalAlign.Center;

                                            if ((mark.ToString() != "-1") && (mark.ToString() != "-2") && (mark.ToString() != "-3") && (mark.ToString() != "-4") && (mark.ToString() != "-5") && (mark.ToString() != "-6") && (mark.ToString() != "-7") && (mark.ToString() != "-8") && (mark.ToString() != "-9") && (mark.ToString() != "-10") && (mark.ToString() != "-11") && (mark.ToString() != "-12") && (mark.ToString() != "-13") && (mark.ToString() != "-14") && (mark.ToString() != "-15") && (mark.ToString() != "-16") && mark != "EL" && mark != "P" && mark != "ML" && mark != "SOD" && mark != "NSS" && mark != "H" && mark != "NJ" && mark != "S" && mark != "L" && mark != "EOD" && mark != "AAA" && mark != "HS" && mark != "PP" && mark != "SYOD" && mark != "NCC" && mark != "COD" && mark != "LA" && mark != "OOD" && mark != "OD" && mark != "RAA")//Added by Subburaj 21.08.2014
                                            {
                                                if (Convert.ToDouble(mark) < Convert.ToDouble(reader["minmark"].ToString()))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colco].ForeColor = Color.Red;
                                                    FpEntry.Sheets[0].Cells[res, colco].Font.Underline = true;
                                                    FpEntry.Sheets[0].Cells[res, colco].Font.Name = "Book Antiqua";
                                                    FpEntry.Sheets[0].Cells[res, colco].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }

                                        }


                                    }
                                }
                                mycon.Close();
                                chkmark = FpEntry.Sheets[0].Cells[res, colco].Text;
                                if (chkmark != "EL" && chkmark != "P" && chkmark != "ML" && chkmark != "SOD" && chkmark != "NSS" && chkmark != "H" && chkmark != "NJ" && chkmark != "S" && chkmark != "L" && chkmark != "EOD" && chkmark != "" && chkmark != "NCC" && chkmark != "HS" && chkmark != "PP" && chkmark != "SYOD" && chkmark != "COD" && chkmark != "OOD" && chkmark != "OD" && chkmark != "RAA")
                                {
                                    mycon.Close();
                                    int newchkmark = 0;
                                    if (int.TryParse(chkmark, out newchkmark))
                                        newchkmark = Convert.ToInt32(chkmark);

                                    if (chkmark != "AAA" && chkmark != "A" && newchkmark > Convert.ToInt32(reader["minmark"].ToString()))//&& newchkmark  > 49
                                    {
                                        FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                        FpEntry.Sheets[0].Cells[res, colfine].Text = "0";
                                    }
                                    else
                                    {
                                        if (Unitbtn.Checked == true)
                                        {

                                            fineset = "select range,unit_amount from failfine ";
                                            SqlCommand cmmd = new SqlCommand(fineset, mycon);
                                            mycon.Open();
                                            SqlDataReader dr = cmmd.ExecuteReader();
                                            while (dr.Read())
                                            {
                                                string range = dr["range"].ToString();
                                                if (chkmark != "AAA" && chkmark != "A" && chkmark != "OD" && range != "Absentees" && range != "OD" && range != "Leave")
                                                {
                                                    if (range != "Leave")
                                                    {

                                                        string[] split4 = range.Split(new Char[] { '-' });
                                                        sp = split4[0].ToString();
                                                        int i = split4.Count();
                                                        if (i == 2)
                                                        {
                                                            sp1 = split4[1].ToString();
                                                            if ((Convert.ToDouble(chkmark) >= (Convert.ToDouble(split4[0]))) && (Convert.ToDouble(chkmark) <= (Convert.ToDouble(split4[1]))))
                                                            {
                                                                FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                                FpEntry.Sheets[0].SetText(res, colfine, dr["unit_amount"].ToString());
                                                            }
                                                        }
                                                        else if (i == 1)
                                                        {
                                                            string ranges = "select unit_amount from failfine where range='" + sp + "'";
                                                            SqlCommand cmmd1 = new SqlCommand(ranges, mycon2);

                                                            mycon2.Open();
                                                            SqlDataReader dr5 = cmmd1.ExecuteReader();
                                                            while (dr5.Read())
                                                            {
                                                                FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                                FpEntry.Sheets[0].SetText(res, colfine, dr5["unit_amount"].ToString());
                                                            }
                                                            mycon2.Close();
                                                        }
                                                    }
                                                }

                                                if ((range == "Absentees") && (chkmark == "AAA" || chkmark == "A"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["unit_amount"].ToString());

                                                }

                                                if ((range == "OD") && (chkmark == "OD"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["unit_amount"].ToString());

                                                }
                                                if ((range == "Leave") && (chkmark == "L"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["unit_amount"].ToString());
                                                }

                                            }

                                        }

                                        else if (terminalbtn.Checked == true)
                                        {
                                            fineset = "select range,model_amount from failfine ";
                                            SqlCommand cmmd = new SqlCommand(fineset, mycon);

                                            mycon.Open();
                                            SqlDataReader dr = cmmd.ExecuteReader();
                                            while (dr.Read())
                                            {
                                                string range = dr["range"].ToString();
                                                if (chkmark != "AAA" && chkmark != "A" && chkmark != "OD" && range != "Absentees" && range != "OD" && range != "Leave")
                                                {
                                                    if (range != "Leave")
                                                    {

                                                        string[] split4 = range.Split(new Char[] { '-' });
                                                        int i = split4.Count();
                                                        sp = split4[0].ToString();
                                                        if (i == 2)
                                                        {
                                                            sp1 = split4[1].ToString();
                                                            if ((Convert.ToDouble(chkmark) >= (Convert.ToInt32(split4[0]))) && (Convert.ToDouble(chkmark) <= (Convert.ToInt32(split4[1]))))
                                                            {
                                                                FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                                FpEntry.Sheets[0].SetText(res, colfine, dr["model_amount"].ToString());

                                                            }
                                                        }
                                                        else if (i == 1)
                                                        {
                                                            string ranges = "select model_amount from failfine where range='" + sp + "'";
                                                            SqlCommand cmmd1 = new SqlCommand(ranges, mycon2);

                                                            mycon2.Open();
                                                            SqlDataReader dr5 = cmmd1.ExecuteReader();
                                                            while (dr5.Read())
                                                            {
                                                                FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                                FpEntry.Sheets[0].SetText(res, colfine, dr5["model_amount"].ToString());
                                                            }
                                                            mycon2.Close();
                                                        }
                                                    }

                                                }

                                                if ((range == "Absentees") && (chkmark == "AAA" || chkmark == "A"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["model_amount"].ToString());

                                                }

                                                if ((range == "OD") && (chkmark == "OD"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["model_amount"].ToString());

                                                }
                                                if ((range == "Leave") && (chkmark == "L"))
                                                {
                                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                                    FpEntry.Sheets[0].SetText(res, colfine, dr["model_amount"].ToString());
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    FpEntry.Sheets[0].Cells[res, colfine].Text = "0";
                                    FpEntry.Sheets[0].Cells[res, colfine].HorizontalAlign = HorizontalAlign.Right;
                                    FpEntry.Sheets[0].Cells[res, colco].HorizontalAlign = HorizontalAlign.Center;
                                }
                                mycon.Close();
                            }
                        }
                        if (FpEntry.Sheets[0].RowCount > 0)
                        {
                            FpEntry.Sheets[0].ColumnCount = Convert.ToInt32(FpEntry.Sheets[0].ColumnCount) + 1;
                            FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, FpEntry.Sheets[0].ColumnCount - 1, 2, 1);
                            FpEntry.Sheets[0].Columns[FpEntry.Sheets[0].ColumnCount - 1].Width = 150;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Text = "Total Fine Amount";
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].HorizontalAlign = HorizontalAlign.Center;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Bold = true;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Size = FontUnit.Medium;
                            FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].Font.Name = "Book Antiqua";

                            for (int rowcount = 0; rowcount <= Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1; rowcount++)
                            {

                                for (int count = 6; count <= Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 2; count = count + 2)
                                {
                                    string s1 = FpEntry.Sheets[0].Cells[rowcount, count].Text;
                                    if (s1 != "")
                                    {
                                        countval = countval + Convert.ToInt32(FpEntry.Sheets[0].Cells[rowcount, count].Text);
                                    }
                                    else
                                    {
                                        countval = countval + 0;
                                    }
                                }
                                FpEntry.Sheets[0].Cells[rowcount, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1].HorizontalAlign = HorizontalAlign.Right;
                                FpEntry.Sheets[0].SetText(rowcount, Convert.ToInt16(FpEntry.Sheets[0].ColumnCount) - 1, countval.ToString());
                                countval = 0;
                            }
                        }
                        else
                        {
                            FpEntry.Visible = false;
                            btnmasterprint.Visible = false;
                            btnExcel.Visible = false;
                            //Added By Srinath 27/2/2013
                            txtexcelname.Visible = false;
                            lblrptname.Visible = false;
                        }
                    }
                }
                if (flag == 0)
                {
                    Buttontotal.Visible = false;
                    lblrecord.Visible = false;
                    DropDownListpage.Visible = false;
                    ////TextBoxother.Visible = false;
                    lblpage.Visible = false;
                    FpEntry.Sheets[0].RowCount = 0;
                    TextBoxpage.Visible = false;
                    FpEntry.Visible = false;
                    btnmasterprint.Visible = false;
                    lblnorec.Visible = true;
                }
                con.Close();
            }
            //}

            //if (CheckBox1.Checked == true)
            //{



            //    btnmasterprint.Visible = true;
            //    FpEntry.Visible = true;

            //    string listrange = Ddlrange.SelectedValue.ToString();
            //    if(listrange!="" && listrange !=null)
            //    {
            //    if (listrange != "Absentees")
            //    {
            //        FpEntry.Sheets[0].ColumnHeader.RowCount = 0;
            //        FpEntry.Sheets[0].ColumnHeader.RowCount = 1;
            //        FpEntry.Sheets[0].ColumnCount = 0;

            //        FpEntry.Sheets[0].ColumnCount = 6;

            //        FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";



            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No";
            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Admission No";
            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            //        FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Marks Obtained";


            //        FpEntry.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            //        //FpEntry.Sheets[0].ColumnHeader.Columns[5].Width = 200;
            //      //  FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 0, 2, 1);
            //        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(FpEntry.Sheets[0].ColumnHeader.RowCount - 2, 5, 2, 1);

            //        FpEntry.Sheets[0].RowCount = 0;
            //        string[] listsplit = listrange.Split(new Char[] { '-' });

            //        sp = listsplit[0].ToString();

            //        sp1 = listsplit[1].ToString();
            //        //Modified by gowtham
            //        //string list = " Select result.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "  and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + " and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'  order by result.roll_no";
            //        //sqlStr = "Select distinct result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.stud_type from applyn,result,exam_type,criteriaforinternal,registration where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year = '" + ddlBatch.SelectedValue.ToString() + "'" + strsec + "" + Session["strvar"] + " and exam_type.subject_no in  (" + code + ") " + strorder + "";
            //        string list = " Select result.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,result.marks_obtained from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + "  and result.marks_obtained>=" + sp + " and result.marks_obtained<=" + sp1 + " and exam_type.subjecT_no in(" + code + " ) and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'" + strorder + "";
            //        if (list != "")
            //        {
            //            con.Open();
            //            SqlDataAdapter adaSyll1 = new SqlDataAdapter(list, con);
            //            DataSet ds = new DataSet();
            //            adaSyll1.Fill(ds, "ds");
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                lblnorec.Visible = false;
            //                flag = 1;
            //            }
            //           // FpEntry.Sheets[0].ColumnHeader.RowCount = 0;
            //           // FpEntry.Sheets[0].ColumnHeader.RowCount = 1;

            //            //FpEntry.DataSource = ds.Tables[0];
            //           // FpEntry.DataBind();
            //            //FpEntry.SaveChanges();
            //            //added by gowtham  --------------------
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
            //                {

            //                    FpEntry.Sheets[0].RowCount++;
            //                    FpEntry.Sheets[0].Rows[irow].Border.BorderColor = Color.Black;
            //                    FpEntry.Sheets[0].Cells[irow, 1].Text = ds.Tables[0].Rows[irow]["Roll_Admit"].ToString();
            //                    FpEntry.Sheets[0].Cells[irow, 0].Text = ds.Tables[0].Rows[irow]["roll_no"].ToString();
            //                    FpEntry.Sheets[0].Cells[irow, 2].Text = ds.Tables[0].Rows[irow]["reg_no"].ToString();
            //                    FpEntry.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["stud_name"].ToString();
            //                    FpEntry.Sheets[0].Cells[irow, 4].Text = ds.Tables[0].Rows[irow]["stud_type"].ToString();
            //                    FpEntry.Sheets[0].Cells[irow, 5].Text = ds.Tables[0].Rows[irow]["marks_obtained"].ToString();

            //                }
            //            }

            //            ////FpEntry.DataSource = ds.Tables[0];
            //            ////FpEntry.DataBind();

            //            //FpEntry.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            //            //FpEntry.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            //            //FpEntry.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            //            //-----------------
            //         }
            //     }
            //        else if (listrange == "Absentees")
            //        {

            //            FpEntry.Sheets[0].ColumnHeader.RowCount = 0;
            //            FpEntry.Sheets[0].ColumnHeader.RowCount = 1;
            //            FpEntry.Sheets[0].ColumnCount = 0;

            //            FpEntry.Sheets[0].ColumnCount = 6;

            //            FpEntry.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";



            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Roll No";
            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Admission No";
            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            //            FpEntry.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Marks Obtained";


            //            FpEntry.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            //             //Modified by gowtham
            //            //string list1 = "Select result.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,case when result.marks_obtained=-1 then 'AAA' end from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + " and result.marks_obtained= -1  and exam_type.subjecT_no in(" + code + " )  and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "'  order by result.roll_no";
            //            string list1 = "Select result.roll_no,registration.roll_no,registration.Roll_Admit,registration.reg_no,registration.stud_name,registration.Stud_Type,case when result.marks_obtained=-1 then 'AAA' end as absencount from result,exam_type,criteriaforinternal,registration,applyn where registration.roll_no=result.roll_no and registration.app_no=applyn.app_no and cc=0 and delflag=0 and exam_flag<>'DEBAR' and criteriaforinternal.criteria_no = exam_type.criteria_no and exam_type.exam_code = result.exam_code and exam_type.batch_year =" + ddlBatch.SelectedValue.ToString() + "" + strsec + " and result.marks_obtained= -1  and exam_type.subjecT_no in(" + code + " )  and exam_type.criteria_no ='" + ddlTest.SelectedValue.ToString() + "' " + strorder + "";
            //            if (list1 != "")
            //            {
            //                FpEntry.Sheets[0].RowCount = 0;
            //                con.Open();
            //                SqlDataAdapter adaSyll1 = new SqlDataAdapter(list1, con);
            //                DataSet ds = new DataSet();
            //                adaSyll1.Fill(ds, "ds");
            //                if (ds.Tables[0].Rows.Count > 0)
            //                {
            //                    lblnorec.Visible = false;
            //                    flag = 1;
            //                }
            //                if (ds.Tables[0].Rows.Count > 0)
            //                {
            //                    for (int irow = 0; irow < ds.Tables[0].Rows.Count; irow++)
            //                    {

            //                        FpEntry.Sheets[0].RowCount++;
            //                        FpEntry.Sheets[0].Rows[irow].Border.BorderColor = Color.Black;
            //                        FpEntry.Sheets[0].Cells[irow, 1].Text = ds.Tables[0].Rows[irow]["Roll_Admit"].ToString();
            //                        FpEntry.Sheets[0].Cells[irow, 0].Text = ds.Tables[0].Rows[irow]["roll_no"].ToString();
            //                        FpEntry.Sheets[0].Cells[irow, 2].Text = ds.Tables[0].Rows[irow]["reg_no"].ToString();
            //                        FpEntry.Sheets[0].Cells[irow, 3].Text = ds.Tables[0].Rows[irow]["stud_name"].ToString();
            //                        FpEntry.Sheets[0].Cells[irow, 4].Text = ds.Tables[0].Rows[irow]["stud_type"].ToString();
            //                        FpEntry.Sheets[0].Cells[irow, 5].Text = ds.Tables[0].Rows[irow]["absencount"].ToString();

            //                    }
            //                }
            //                //FpEntry.DataSource = ds.Tables[0];
            //                //FpEntry.DataBind();
            //                //FpEntry.SaveChanges();
            //             }

            //        }

            //      if (flag == 0)
            //      {

            //          Buttontotal.Visible = false;
            //          lblrecord.Visible = false;
            //          DropDownListpage.Visible = false;
            //          //TextBoxother.Visible = false;
            //          lblpage.Visible = false;
            //          TextBoxpage.Visible = false;
            //          FpEntry.Visible = false;
            //          btnmasterprint.Visible = false;
            //          btnExcel.Visible = false;
            //          //Added By Srinath 27/2/2013
            //          txtexcelname.Visible = false;
            //          lblrptname.Visible = false;
            //          lblnorec.Visible = true;
            //          lblnorec.Text = "There are no records found";
            //    }
            //}

            //}

        }
        catch
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            //TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            FpEntry.Visible = false;
            btnmasterprint.Visible = false;
        }
    }



    //public void getfailfine()
    // {
    //    string mrange1;
    //    // string rang1;
    //    mrange1 = "select * from failfine";

    //        con.Open();
    //        SqlDataAdapter adaSyll1 = new SqlDataAdapter(mrange1, con);
    //        DataSet ds = new DataSet();
    //        adaSyll1.Fill(ds, "ds");
    //        FineSet.DataSource = ds.Tables[0];
    //        FineSet.DataBind();

    //       con.Close();

    //}
    public string getattval(int att_leavetype)
    {
        switch (att_leavetype)
        {
            case 1:

                atten = "P";
                break;
            case 2:
                atten = "A";
                break;
            case 3:
                atten = "OD";
                break;
            case 4:
                atten = "ML";
                break;
            case 5:
                atten = "SOD";
                break;
            case 6:
                atten = "NSS";
                break;
            case 7:
                atten = "H";
                break;
            case 8:
                atten = "NJ";
                break;
            case 9:
                atten = "S";
                break;
            case 10:
                atten = "L";
                break;
        }
        return atten;

    }
    public string Getdate(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        mycon1.Close();
        mycon1.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(exampresent, con);
        SqlCommand cmd5a = new SqlCommand(sqlstr);
        cmd5a.Connection = mycon1;
        SqlDataReader drnew;
        drnew = cmd5a.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "";
        }
        mycon1.Close();
    }
    public void savefailfine()
    {

        string selectrow;
        string deleterow;
        string insertrow;

        selectrow = "select * from failfine";
        SqlCommand comm = new SqlCommand(selectrow, con);
        con.Open();
        SqlDataReader resreader;
        resreader = comm.ExecuteReader();
        while (resreader.Read())
        {
            if (resreader.HasRows == true)
            {
                mycon.Close();
                deleterow = "delete from failfine";
                SqlCommand comm1 = new SqlCommand(deleterow, mycon);
                mycon.Open();
                comm1.ExecuteNonQuery();

            }

        }

        FpSpread1.SaveChanges();

        for (int row = 0; row <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; row++)
        {

            int rangecol = Convert.ToInt16((FpSpread1.Sheets[0].ColumnCount) - 3);

            int unitcol = Convert.ToInt16(FpSpread1.Sheets[0].ColumnCount) - 2;

            int terminalcol = Convert.ToInt16(FpSpread1.Sheets[0].ColumnCount) - 1;
            string rangeval = Convert.ToString(FpSpread1.Sheets[0].Cells[row, rangecol].Text);
            int unitval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, unitcol].Text);
            int terminalval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[row, terminalcol].Text);
            if (rangeval != "" && rangeval != null && unitval != null && terminalval != null)
                con.Close();
            insertrow = "insert into failfine values('" + rangeval + "'," + unitval + "," + terminalval + ")";
            SqlCommand comm2 = new SqlCommand(insertrow, con);
            con.Open();
            comm2.ExecuteNonQuery();
        }
        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
    }


    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FpSpread1.Visible = false;
        //Button8.Visible = false;
        ////Button8.Enabled = false;

        //Button6.Visible = false;
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        ddlSubject.Items.Clear();
        TextBox1.Text = "";
        //Label7.Visible = false;
        ddlTest.SelectedIndex = -1;
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }


        //bind section
        BindSectionDetail();
        //bing test
        GetTest();
        //bind subject
        GetSubject();
    }
    public void GetSubject()
    {
        con.Close();
        con.Open();
        string sems = "";
        string section = ddlSec.SelectedValue.ToString();
        ddlSubject.Items.Clear();
        if (!Page.IsPostBack == false)
        {
            strsec = "";

            if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
            {
                strsec = "";

            }
            else
            {
                strsec = " and exam_type.Sections='" + section.ToString() + "'";

            }

            if (ddlSem.SelectedValue == "")
            {

                sems = "";
            }
            else
            {

                sems = "and SM.semester=" + ddlSem.SelectedValue.ToString() + "";
            }

        }
        //string SyllabusYr;
        //string SyllabusQry;
        //SyllabusQry = "select syllabus_year from syllabus_master where degree_code=" + ddlBranch.SelectedValue.ToString() + " and semester =" + ddlSem.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + "";
        //SyllabusYr = GetFunction(SyllabusQry.ToString());
        string Sqlstr;
        Sqlstr = "";
        //if (SyllabusYr != "")
        //{
        if (Session["Staff_Code"].ToString() == "")
        {
            // Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,exam_type where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and syllabus_master.semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSem.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 "+ strsec + " and exam_flag <> 'DEBAR'";
            Sqlstr = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and  S.subtype_no = Sem.subtype_no and promote_count=1  order by subject_code ";
        }
        else if (Session["Staff_Code"].ToString() != "")
        {
            // Sqlstr = "select distinct subject_name,subject.subject_no,subject_code from subject,sub_sem,syllabus_master,subjectchooser,registration,exam_type where sub_sem.syll_Code = subject.syll_code and subject.subtype_no = sub_sem.subtype_no and sub_sem.promote_count =1 and subject.syll_code=syllabus_master.syll_code and syllabus_master.degree_code=" + ddlBranch.SelectedValue.ToString() + " and syllabus_master.semester=" + ddlSem.SelectedValue.ToString() + " and syllabus_master.batch_year=" + ddlBatch.SelectedValue.ToString() + " and syllabus_year=" + SyllabusYr.ToString() + " and subject.subject_no =subjectchooser.subject_no and subjectchooser.roll_no=registration.roll_no and registration.degree_code=" + ddlBranch.SelectedValue.ToString() + " and registration.current_semester>=" + ddlSem.SelectedValue.ToString() + " and registration.batch_year=" + ddlBatch.SelectedValue.ToString() + " and RollNo_Flag<>0 and cc=0 " + strsec + " and exam_flag <> 'DEBAR'";
            Sqlstr = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + ddlBranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year='" + ddlBatch.SelectedValue.ToString() + "' and S.subtype_no = Sem.subtype_no and promote_count=1 and staff_code='" + Session["Staff_Code"].ToString() + "'  order by subject_code "; //new as per ind sub attend
        }
        con.Close();
        if (Sqlstr != "")
        {
            SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(Sqlstr, con);
            DataSet titles = new DataSet();

            con.Open();
            sqlAdapter1.Fill(titles);
            if (titles.Tables[0].Rows.Count > 0)
            {
                ddlSubject.Enabled = true;
                ddlSubject.DataSource = titles;
                ddlSubject.DataValueField = "Subject_No";
                ddlSubject.DataTextField = "Subject_Name";
                ddlSubject.DataBind();
            }
            else
            {
                ddlSubject.Enabled = false;
            }
        }
        //}

        //con.Close();
    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        TextBoxother.Text = "";
        if (DropDownListpage.Text == "Others")
        {
            TextBoxpage.Text = "";
            panels.Visible = false;
            ddlSubject.Visible = false;
            TextBoxother.Visible = true;
            TextBoxother.Focus();

        }
        else
        {
            //TextBoxother.Visible = false;
            FpEntry.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            //CalculateTotalPages();
        }
        if (FpEntry.Sheets[0].PageSize != 10)
        {
            FpEntry.Height = 200 + (10 * FpEntry.Sheets[0].PageSize);
        }
        else if ((FpEntry.Sheets[0].PageSize != 10) || (FpEntry.Sheets[0].PageSize != 20))
        {
            FpEntry.Height = 500;
        }
    }


    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "  Pages : " + Session["totalPages"];
        //Buttontotal.Visible = true;
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    FpEntry.Visible = true;
                    btnmasterprint.Visible = true;
                    TextBoxpage.Text = "";
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = "";
                }
                else
                {
                    LabelE.Visible = false;
                    FpEntry.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpEntry.Visible = true;
                    btnmasterprint.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {

            if (TextBoxother.Text != "")
            {
                panels.Visible = true;
                ddlSubject.Visible = true;
                FpEntry.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                //CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {

        Session["column_header_row_count"] = Convert.ToString(FpEntry.ColumnHeader.RowCount);
        DateTime date_today = DateTime.Now;
        int yr_now = Convert.ToInt32(date_today.ToString("yyyy"));
        string academyear = (yr_now.ToString() + "-" + (yr_now + 1).ToString());
        string degreedetails = "MONTHLY AND MODEL EXAMINATION FINE REPORT" + '@' + "Degree :" + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '[' + ddlBranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlSem.SelectedItem.ToString() + '@' + "Test Name:" + ddlTest.SelectedItem.ToString();
        //string degreedetails = "Branchwise Subject Analysis" + '@' + "Test:" + ddltest.SelectedItem.ToString();
        string pagename = "CAMfine.aspx";
        Printcontrol.loadspreaddetails(FpEntry, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            //Added By Subburaj 03/09/2014******//
            if (ddlTest.Items.Count <= 0)
            {
                lblnorec.Text = "Please Select Valid Semester";
                lblnorec.Visible = true;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnmasterprint.Visible = false;
                FpEntry.Visible = false;
                return;
            }
            int countsubject = 0;
            lblnorec.Text = "";
            for (int i = 0; i < ddlSubject.Items.Count; i++)
            {
                if (ddlSubject.Items[i].Selected == true)
                {
                    countsubject++;
                }
            }
            if (countsubject <= 0)
            {
                lblnorec.Text = "Please Select Any one Subject";
                lblnorec.Visible = true;

                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                btnmasterprint.Visible = false;
                FpEntry.Visible = false;
                return;
            }
            TextBox1.Attributes.Add("Readonly", "Readonly");
            //*******End**************//
            int count = 0;
            btnExcel.Visible = true;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = true;
            lblrptname.Visible = true;
            lblnorec.Visible = false;
            if (CheckBox1.Checked == true)
            {
                for (int i = 0; i < ddlSubject.Items.Count; i++)
                {
                    if (ddlSubject.Items[i].Selected == true)
                    {

                        count++;
                    }
                }
                if (count == 0)
                {
                    ddlSubject.ClearSelection();
                    TextBox1.Text = "";

                }
                else
                {

                }
            }
            //FpEntry.Sheets[0].RowCount = 2;
            //TextBoxother.Visible = false;
            TextBoxother.Text = "";
            TextBoxpage.Text = "";
            FpEntry.CurrentPage = 0;
            //Ddlrange.Visible = true;
            CheckBox1.Visible = true;
            Label2.Visible = true;
            if (ddlBatch.SelectedIndex == 0)
            {
                //Label3.Visible = true;
            }
            if (ddlDegree.SelectedIndex == 0)
            {

                //Label4.Visible = true;
            }
            if (ddlBranch.SelectedIndex.ToString() == "")
            {
                //Label6.Visible = true;
            }
            if (ddlSem.SelectedIndex == 0)
            {
                //Label7.Visible = true;
            }
            if (ddlSec.SelectedIndex == 0)
            {
                if (ddlSec.Enabled == true)
                {
                    //Label8.Visible = true;
                }
                else
                {
                    //Label8.Visible = false;
                }
            }
            if (ddlTest.SelectedIndex == 0)
            {
                //Label9.Visible = true;
            }

            if (ddlSubject.SelectedValue == "")
            {
                Label10.Visible = true;
                FpEntry.Visible = false;
                btnmasterprint.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                lblnorec.Visible = false;
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                //TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
            }
            if (ddlSubject.SelectedValue != "")
            {
                // FpEntry.Sheets[0].ColumnHeader.RowCount = 1;

                SpreadBind();
                //func_final_row_header();
                function_header();
                //'-------- func for logo
                func_for_logo();



            }
            if (Session["Rollflag"].ToString() == "0")
            {
                FpEntry.Sheets[0].ColumnHeader.Columns[0].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpEntry.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                FpEntry.Sheets[0].ColumnHeader.Columns[4].Visible = false;
            }
            if (!Page.IsPostBack == false)
            {
                string strsec = "";
                if (ddlSec.Text.ToString() == "All" || ddlSec.Text.ToString() == "")
                {
                    strsec = "";
                }
                else
                {
                    strsec = " and registration.sections='" + ddlSec.SelectedValue.ToString() + "'";
                }

            }

            if (Convert.ToInt32(FpEntry.Sheets[0].RowCount) == 0)
            {

                FpEntry.Visible = false;
                btnmasterprint.Visible = false;
                btnExcel.Visible = false;
                //Added By Srinath 27/2/2013
                txtexcelname.Visible = false;
                lblrptname.Visible = false;



            }
            else
            {
                lblnorec.Visible = false;
                //Buttontotal.Visible = true;
                ////lblrecord.Visible = true;
                //DropDownListpage.Visible = true;
                //TextBoxother.Visible = false;
                //lblpage.Visible = true;
                //TextBoxpage.Visible;
                FpEntry.Visible = true;
                btnmasterprint.Visible = true;
                //  FpEntry.ActiveSheetView.AutoPostBack = false;
                FpEntry.Sheets[0].PageSize = 10;
                //FpEntry.Sheets[0].Columns[3].Width = 220;
                FpEntry.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpEntry.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpEntry.Pager.Align = HorizontalAlign.Right;
                FpEntry.Pager.Font.Bold = true;
                FpEntry.Pager.Font.Name = "Book Antiqua";
                FpEntry.Pager.ForeColor = Color.DarkGreen;
                FpEntry.Pager.BackColor = Color.Beige;
                FpEntry.Pager.BackColor = Color.AliceBlue;
                //FpEntry.ActiveSheetView.SpanModel.Add((Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1), 0, 1, 2);
                //FpEntry.Sheets[0].SetText(Convert.ToInt16(FpEntry.Sheets[0].RowCount) - 1, 0, "Average");
                Double totalRows = 0;
                totalRows = Convert.ToInt32(FpEntry.Sheets[0].RowCount);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / FpEntry.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + " Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();



                if (totalRows >= 10)
                {

                    FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpEntry.Height = 335;

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    FpEntry.Height = 100;
                }
                else
                {
                    FpEntry.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(FpEntry.Sheets[0].PageSize.ToString());
                    FpEntry.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
                FpEntry.Height = 200 + (10 * Convert.ToInt32(totalRows));
                FpEntry.Width = 300 + 50 * (FpEntry.Sheets[0].ColumnCount - 3);
            }

            FarPoint.Web.Spread.TextCellType textcell = new FarPoint.Web.Spread.TextCellType();
            FpEntry.Sheets[0].Columns[0].CellType = textcell;

            FpEntry.Sheets[0].Columns[1].CellType = textcell;

            FpEntry.Sheets[0].Columns[2].CellType = textcell;
            // TextBox1.Enabled = false;

        }
        catch
        {

        }
    }
    public void func_for_logo()
    {
        //*****************set the logo for header left logo
        //MyImg mi3 = new MyImg();
        //mi3.ImageUrl = "Handler/Handler2.ashx?";
        //FpEntry.Sheets[0].SheetCorner.Cells[0, 0].CellType = mi3;
        //FpEntry.Sheets[0].SheetCornerSpanModel.Add(0, 0, 6, 1);
        ////************** set the right logo
        //for (int logo_col = 0; logo_col < FpEntry.Sheets[0].ColumnCount; logo_col++)
        //{
        //    if (FpEntry.Sheets[0].Columns[logo_col].Visible == true)
        //    {
        //        right_logo_clmn = logo_col;
        //    }
        //}
        //FpEntry.Sheets[0].SheetCorner.Columns[0].Width = 100;
        //MyImg mi4 = new MyImg();
        //mi4.ImageUrl = "Handler/Handler5.ashx?";
        //FpEntry.Sheets[0].ColumnHeaderSpanModel.Add(0, right_logo_clmn, 6, 1);
        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].CellType = mi4;
        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].Border.BorderColorLeft = Color.White;
        //FpEntry.Sheets[0].ColumnHeader.Cells[0, right_logo_clmn].Border.BorderColorBottom = Color.Black;
    }
    protected void ddlTest_SelectedIndexChanged1(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        TextBox1.Text = "";
        ddlSubject.Visible = true;
        //Label9.Visible = false;
        GetSubject();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        //Label8.Visible = false;
        GetTest();
    }
    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        ddlSubject.Items.Clear();
        con.Close();
        con.Open();

        string collegecode = Session["collegecode"].ToString();
        string usercode = Session["UserCode"].ToString();
        //con.Open();
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            binddegree();
            bindbranch();
            bindsem();
            if (ddlDegree.Text != "")
            {
                //bindbranch();

                //bindsem();

                //bindsec();

                GetTest();

                GetSubject();
            }
            else
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Give degree rights to the staff";
            }
        }
    }

    protected void ddlSubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        lblnorec.Visible = false;
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        LabelE.Visible = false;
        //Label10.Visible = false;

    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {
        if (CheckBox1.Checked != true)
        {
            Ddlrange.Visible = false;
            FpEntry.Visible = false;
            btnmasterprint.Visible = false;
            btnExcel.Visible = false;
            //Added By Srinath 27/2/2013
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            lblnorec.Visible = false;
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            //TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
            LabelE.Visible = false;

            //Ddlrange.Items.Clear();
        }
        else
        {
            Ddlrange.Visible = true;
            //Ddlrange.Items.Clear();
            //// Ddlrange.SelectedIndex = 0;
            //string mrange;
            //string rang;
            //mrange = "select range from failfine";
            //SqlCommand comm = new SqlCommand(mrange, con);
            //con.Open();
            //SqlDataReader resreader;
            //resreader = comm.ExecuteReader();
            //int i=0;
            //while (resreader.Read())
            //{
            //    if(i<4)
            //     {
            //    if (resreader.HasRows == true)
            //        {                    
            //        rang = resreader["range"] + "";
            //        Ddlrange.Items.Add(rang);
            //        }

            //     }
            //i++;
            //}

        }
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        //MessageBox.Show("select GO Plzz");
    }


    protected void Button5_Click(object sender, EventArgs e)
    {
        savefailfine();
        finesettings();

    }

    protected void Button6_Click(object sender, EventArgs e)
    {
        LabelE.Visible = false;
        lblnorec.Visible = false;
        //Label12.Visible = false;
        btnGo.Enabled = true;
        FpSpread1.Visible = false;
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        Button8.Visible = false;
        btnmasterprint.Visible = false;
        Button6.Visible = false;

    }
    protected void FpSpread1_SelectedIndexChanged(Object sender, EventArgs e)
    {
        //if (cellclick == true)
        {
            int actcol = FpSpread1.ActiveSheetView.ActiveColumn;
            int actrow = FpSpread1.ActiveSheetView.ActiveRow;
            if ((FpSpread1.Sheets[0].ColumnCount - 1) == actcol && (FpSpread1.Sheets[0].RowCount - 1) == actrow)
            {
                //FpSpread1.Attributes.Add("onmouseup", "__doPostBack('FpSpread1','CellClick,' + FpSpread1.ActiveRow + ',' + FpSpread1.ActiveCol)");

                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
            }

            //FpSpread1.ActiveSheetView.AutoPostBack = false;
        }
        //FpSpread1.Sheets[0].AutoPostBack = false;
        //FpSpread1.ActiveSheetView.AutoPostBack = false;
    }
    protected void FpSpread1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSpread1_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        cellclick = true;
    }
    protected void FpEntry_EditCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSpread1_ActiveRowChanged(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {

    }
    protected void FpSpread1_ActiveSheetChanged(object sender, EventArgs e)
    {

    }
    protected void FpSpread1_SaveOrLoadSheetState(object sender, FarPoint.Web.Spread.SheetViewStateEventArgs e)
    {

    }
    protected void FpSpread1_Disposed(object sender, EventArgs e)
    {
        Button8.Enabled = true;
    }
    protected void TextBox1_TextChanged1(object sender, EventArgs e)
    {
        ddlSubject.Visible = true;
    }
    protected void ddlSubject_SelectedIndexChanged1(object sender, EventArgs e)
    {
        int subj_cnt = 0;
        Label10.Visible = false;
        for (int i = 0; i < ddlSubject.Items.Count; i++)
        {

            if (ddlSubject.Items[i].Selected == true)
            {
                subj_cnt++;
                if (text == "")
                {
                    code = ddlSubject.Items[i].Value;
                    text = ddlSubject.Items[i].Text;
                    TextBox1.Text = text;
                }
                else
                {
                    code = code + "," + ddlSubject.Items[i].Value;
                    TextBox1.Text = text + "," + ddlSubject.Items[i].Text;

                }

            }
            else if (ddlSubject.Items[i].Selected != true)
            {
                TextBox1.Text = text + "";
            }


            TextBox1.Text = "Subject(" + (subj_cnt) + ")";

        }

    }


    protected void FpSpread1_EditCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Button8.Enabled = true;
    }


    protected void TextBox1_Init(object sender, EventArgs e)
    {
        ddlSubject.Visible = true;
    }
    public void finesettings()
    {
        //FpSpread1.Attributes.Add("onmouseup", "__doPostBack('FpSpread1','CellClick,' + FpSpread1.ActiveRow + ',' + FpSpread1.ActiveCol)");
        Buttontotal.Visible = false;
        lblrecord.Visible = false;
        DropDownListpage.Visible = false;
        //TextBoxother.Visible = false;
        lblpage.Visible = false;
        TextBoxpage.Visible = false;
        //Label12.Visible = true;
        btnGo.Enabled = false;
        FpEntry.Visible = false;
        btnmasterprint.Visible = false;
        btnExcel.Visible = false;
        //Added By Srinath 27/2/2013
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        FpSpread1.Visible = true;
        Button8.Visible = true;
        //Button8.Enabled = false;

        Button6.Visible = true;
        string mrange1;

        mrange1 = "select * from failfine";
        btnmasterprint.Visible = false;
        con.Close();
        con.Open();
        SqlDataAdapter adaSyll1 = new SqlDataAdapter(mrange1, con);
        DataSet ds = new DataSet();
        adaSyll1.Fill(ds, "ds");
        if (ds.Tables[0].Rows.Count > 0)
        {
            //FpSpread1.Sheets[0].Columns[2].CellType = intgrcel3;
            FpSpread1.Sheets[0].Columns[0].Width = 140;
            FpSpread1.Sheets[0].Columns[1].Width = 140;
            FpSpread1.Sheets[0].Columns[2].Width = 140;
            FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Never;
            FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Range";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unit Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Terminal Amount";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
            FpSpread1.DataSource = ds.Tables[0];
            FpSpread1.DataBind();
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 10;
            style1.Font.Bold = true;
            style1.Font.Size = FontUnit.Medium;

            FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            FpSpread1.Sheets[0].AllowTableCorner = true;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            FarPoint.Web.Spread.TextCellType intgrcel3 = new FarPoint.Web.Spread.TextCellType();
            Ddlrange.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Ddlrange.Items.Add(ds.Tables[0].Rows[i]["range"].ToString());

            }



            //FpSpread1.Sheets[0].Columns[1].CellType = intgrcel3;
        }
        else
        {

            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Bold = true;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].SheetCorner.Cells[0, 0].Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].RowCount = 4;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FarPoint.Web.Spread.TextCellType intgrcel3 = new FarPoint.Web.Spread.TextCellType();
            //FpSpread1.Sheets[0].Columns[1].CellType = intgrcel3;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Range";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Unit Amount";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Terminal Amount";
            FpSpread1.Sheets[0].Cells[0, 0].Text = "0-9";
            FpSpread1.Sheets[0].Cells[0, 1].Text = "0";
            FpSpread1.Sheets[0].Cells[0, 2].Text = "0";
            FpSpread1.Sheets[0].Cells[1, 0].Text = "10-20";
            FpSpread1.Sheets[0].Cells[1, 1].Text = "0";
            FpSpread1.Sheets[0].Cells[1, 2].Text = "0";
            FpSpread1.Sheets[0].Cells[2, 0].Text = "20-40";
            FpSpread1.Sheets[0].Cells[2, 1].Text = "0";
            FpSpread1.Sheets[0].Cells[2, 2].Text = "0";
            FpSpread1.Sheets[0].Cells[3, 0].Text = "Absentees";
            FpSpread1.Sheets[0].Cells[3, 1].Text = "0";
            FpSpread1.Sheets[0].Cells[3, 2].Text = "0";

            Ddlrange.Items.Add("0-9");
            Ddlrange.Items.Add("10-20");
            Ddlrange.Items.Add("20-40");
            Ddlrange.Items.Add("Absentees");
        }
    }
    protected void Finesettings_Click(object sender, EventArgs e)
    {

        finesettings();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        string selected_criteria = "";
        Session["page_redirect_value"] = ddlBatch.SelectedIndex + "," + ddlDegree.SelectedIndex + "," + ddlBranch.SelectedIndex + "," + ddlSem.SelectedIndex + "," + ddlSec.SelectedIndex + "," + ddlTest.SelectedIndex;

        //************************ radio btn test
        if (Unitbtn.Enabled == true)
        {
            Session["page_redirect_value"] += "," + Unitbtn.Text;
        }
        else if (terminalbtn.Enabled == true)
        {
            Session["page_redirect_value"] += "," + Unitbtn.Text;
        }
        //********************** radio btn subname acronym
        if (SubjectRadio.Enabled == true)
        {
            Session["page_redirect_value"] += "," + SubjectRadio.Text;
        }
        else if (acronymradio.Enabled == true)
        {
            Session["page_redirect_value"] += "," + acronymradio.Text;
        }

        //********************* set the subject criteria
        if (ddlSubject.Items.Count > 0)
        {
            for (int criteria = 0; criteria < ddlSubject.Items.Count; criteria++)
            {
                if (ddlSubject.Items[criteria].Selected == true)
                {
                    if (selected_criteria == "")
                    {
                        selected_criteria = ddlSubject.Items[criteria].Value;
                    }
                    else
                    {
                        selected_criteria = selected_criteria + "-" + ddlSubject.Items[criteria].Value;
                    }
                }
            }
        }

        Session["page_redirect_value"] += "$" + selected_criteria.ToString();



        SpreadBind();
        function_header();
        //func_final_row_header();
        func_for_logo();


        int srtcnt = 0;
        int subheadrname = 0;
        string clmnheadrname = "";
        int child_sub_count = 0;
        string subhdrtext = "";
        string subcolumntext = "";
        Boolean child_flag = false;

        int total_clmn_count = FpEntry.Sheets[0].ColumnCount;
        for (srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        {
            if (FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
            {
                subcolumntext = "";
                if (clmnheadrname == "")
                {
                    clmnheadrname = FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text.Replace("&", "and").ToString();
                }
                else
                {
                    if (child_flag == false)
                    {
                        clmnheadrname = clmnheadrname + "," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text.Replace("&", "and").ToString();
                    }
                    else
                    {
                        clmnheadrname = clmnheadrname + "$)," + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text.Replace("&", "and").ToString();
                    }
                }
                child_flag = false;
            }
            else
            {
                child_flag = true;
                if (subcolumntext == "")
                {
                    if (srtcnt != 0)
                    {
                        for (int te = srtcnt - 1; te <= srtcnt; te++)
                        {
                            if (te == srtcnt - 1)
                            {
                                clmnheadrname = clmnheadrname + "* ($" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "* ($" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                            }
                            else
                            {
                                clmnheadrname = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
                                subcolumntext = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

                            }
                        }
                    }
                }
                else
                {
                    subcolumntext = subcolumntext + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                    clmnheadrname = clmnheadrname + "$" + FpEntry.Sheets[0].ColumnHeader.Cells[FpEntry.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
                }
            }
        }

        string dis_hdng_batch = "Batch Year " + "- " + ddlBatch.SelectedItem.ToString() + " Course " + "- " + ddlDegree.SelectedItem.ToString() + "-" + ddlBranch.SelectedItem.ToString();
        string dis_hdng_sec = "";

        if (ddlSec.Enabled == true && ddlSec.SelectedItem.ToString() != "")
            dis_hdng_sec = "Semester " + "- " + ddlSem.SelectedItem.ToString() + "  " + "Sections " + "- " + ddlSec.SelectedItem.ToString();
        else
            dis_hdng_sec = "Semester " + "- " + ddlSem.SelectedItem.ToString();

        Response.Redirect("Print_Master_Setting_New.aspx?ID=" + clmnheadrname.ToString() + ":" + "CAMfine.aspx" + ":" + dis_hdng_batch + "@" + dis_hdng_sec + ":" + "CAM fine Report");


    }
}
