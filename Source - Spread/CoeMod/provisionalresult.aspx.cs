using System;//=====================================on 26/12/2011
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;

public partial class provisionalresult : System.Web.UI.Page
{
    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(70);
            return img;


        }
    }



    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd1;
    SqlCommand cmd;
    Hashtable has = new Hashtable();
    Hashtable result_has = new Hashtable();
    DataSet ds_getvalues = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 dacc = new DAccess2();


    //------------------declare variables
    double ext_mark_int = 0;
    double tot_ext_mark_int = 0;
    double final_mark = 0;
    int convert_mark = 0;
    int subject_count = 0;
    int column_count = 0;
    int increment_subject = 0;
    int set_subj_code = 3, col_temp = 0;
    string collegecode = "", usercode = "";
    int inc_stud_cnt = 0;
    string stud_rollno = "";
    string stud_name = "";
    string int_mark = "";
    string ext_mark = "";
    string tot_mark = "";
    int row_count = 0;
    int sno = 0;
    string result_porf = "";
    string temp_rollno = "";
    string collnamenew1 = "", address1 = "", address2 = "", address3 = "", pincode = "", categery = "", Affliated = "";
    string sem_val = "";
    string month = "";
    string mon_yr = "";
    string group_user = "", singleuser = "";
    string int_mark_aftermod = "", ext_mark_aftermod = "", tot_mark_aftermod = "";

    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            if (!Page.IsPostBack)
            {
                //==========set header
                if (Session["provisionalresult"].ToString() == "provisionalresult")
                {
                    lblhead.Text = "Provisional Result Report - Befor Moderation";
                }
                else if (Session["provisionalresult"].ToString() == "Tabulated Mark Statement")
                {
                    lblhead.Text = "Tabulated Mark Statement";
                }
                //====================set font
                pageddltxt.Visible = false;
                setpanel.Visible = false;
                errlbl.Visible = false;
                pagesetpanel.Visible = false;
                provisional_spread.Visible = false;
                provisional_spread.Sheets[0].AutoPostBack = true;
                provisional_spread.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                provisional_spread.ActiveSheetView.DefaultStyle.Font.Name = "Book Antique";
                provisional_spread.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                provisional_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                provisional_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                provisional_spread.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antique";
                RadioHeader.Checked = true;


                //===========bind data in ddl
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();


                //if (Convert.ToString(Session["value"]) == "1")//==========back button visible
                //{
                //    LinkButton3.Visible = false;
                //    LinkButton2.Visible = true;
                //}
                //else
                //{
                //    LinkButton3.Visible = true;
                //    LinkButton2.Visible = false;
                //}
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {

        Radiowithoutheader.Checked = false;
        RadioHeader.Checked = false;
        errlbl.Visible = false;


        loadsubject();

    }

    public void loadsubject()
    {
        try
        {
            string checkval = "";
            string section_val = "";
            ddlpage.Items.Clear();
            provisional_spread.CurrentPage = 0;
            provisional_spread.Sheets[0].ColumnCount = 0;
            provisional_spread.Sheets[0].RowCount = 0;
            provisional_spread.Sheets[0].RowHeader.Visible = false;

            if (chk_arrear.Checked == true && chk_regular.Checked == false)
            {
                checkval = "2";
            }
            else if (chk_arrear.Checked == false && chk_regular.Checked == true)
            {
                checkval = "1";
            }
            else
            {
                checkval = "";
            }

            if (ddlsec.Enabled != false)
            {
                section_val = ddlsec.SelectedItem.ToString();
            }
            else
            {
                section_val = "";
            }
            has.Clear();
            has.Add("degree_code", ddlbranch.SelectedValue.ToString());
            has.Add("sem", ddlduration.SelectedValue.ToString());
            has.Add("batch_year", ddlbatch.SelectedValue.ToString());
            has.Add("checkval", checkval);
            has.Add("section", section_val);
            ds_getvalues = dacc.select_method("provisional_getsubject_student", has, "sp");
            if (ds_getvalues.Tables[0].Rows.Count > 0)
            {
                noreclbl.Visible = false;
                provisional_spread.Visible = true;
                subject_count = ds_getvalues.Tables[0].Rows.Count;
                column_count = (subject_count * 4) + 4;
                provisional_spread.Sheets[0].ColumnCount = column_count;//=============increment column count
                provisional_spread.Sheets[0].Columns[1].Width = 400;
                provisional_spread.Sheets[0].Columns[1].Width = 100;
                provisional_spread.Sheets[0].Columns[2].Width = 150;
                provisional_spread.Sheets[0].Columns[column_count - 1].Width = 400;
                provisional_spread.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;

                provisional_spread.Sheets[0].ColumnHeader.RowCount = 7;
                provisional_spread.Sheets[0].ColumnHeader.Rows[0].Visible = true;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Visible = true;
                provisional_spread.Sheets[0].ColumnHeader.Rows[2].Visible = true;
                provisional_spread.Sheets[0].ColumnHeader.Rows[4].HorizontalAlign = HorizontalAlign.Center;
                provisional_spread.Sheets[0].ColumnHeader.Rows[5].HorizontalAlign = HorizontalAlign.Center;
                provisional_spread.Sheets[0].ColumnHeader.Rows[6].HorizontalAlign = HorizontalAlign.Center;
                provisional_spread.Sheets[0].ColumnHeader.Rows[3].HorizontalAlign = HorizontalAlign.Center;


                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 12;
                style.Font.Bold = true;
                provisional_spread.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                provisional_spread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                provisional_spread.Sheets[0].AllowTableCorner = true;
                provisional_spread.Sheets[0].SheetCorner.Cells[0, 0].Text = "  ";
                //=====================header



                MyImg mi = new MyImg();
                mi.ImageUrl = "~/images/10BIT001.jpeg";
                mi.ImageUrl = "Handler/Handler2.ashx?";
                MyImg mi2 = new MyImg();
                mi2.ImageUrl = "~/images/10BIT001.jpeg";
                mi2.ImageUrl = "Handler/Handler5.ashx?";


                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,isnull(logo1,'') as logo1,isnull(logo2,'') as logo2 from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                    //string str = "select isnull(collname, ' ') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(aaa,'') as aaa,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode,isnull(logo1,'') as logo1,isnull(logo2,'') as logo2 from collinfo where college_code='" + Session["collegecode"].ToString() + "'";

                    SqlCommand collegecmd = new SqlCommand(str, con);
                    SqlDataReader collegename;
                    con.Close();
                    con.Open();
                    collegename = collegecmd.ExecuteReader();
                    if (collegename.HasRows)
                    {

                        while (collegename.Read())
                        {

                            collnamenew1 = collegename["collname"].ToString();
                            address1 = collegename["address1"].ToString();
                            address2 = collegename["address2"].ToString();
                            address3 = collegename["address3"].ToString();
                            pincode = collegename["pincode"].ToString();
                            categery = collegename["category"].ToString();
                            Affliated = collegename["affliated"].ToString();


                        }
                    }
                    con.Close();
                }
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, column_count - 4);
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1 + address1 + ", " + address2 + ", " + address3 + ", " + pincode + ".";
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                provisional_spread.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorLeft = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, column_count - 1].Border.BorderColorTop = Color.White;


                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, column_count - 4);
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 2].Text = categery + ", Affiliated to " + Affliated + ".";
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColor = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;

                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 2);
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(0, (column_count - 2), 2, 2);
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, column_count - 2].CellType = mi2;
                //===================================


                SqlDataReader dr;
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand("select exam_month,exam_year from exam_details where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedValue.ToString() + " and current_semester=" + ddlduration.SelectedItem.ToString() + "", con);
                dr = cmd.ExecuteReader();
                dr.Read();
                if (dr.HasRows == true)
                {
                    getmonth(dr["exam_month"].ToString());
                    mon_yr = month + " " + dr["exam_year"].ToString();
                }


                getsemester(ddlduration.SelectedItem.ToString());
                provisional_spread.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorTop = Color.White;
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, column_count);

                if (Session["provisionalresult"].ToString() == "provisionalresult")
                {
                    provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].Text = "PROVISIONAL RESULT OF " + ddldegree.SelectedItem.ToString() + "[" + ddlbranch.SelectedItem.ToString() + "] " + sem_val + " SEMESTER EXAMIATIONS " + mon_yr + " - BEFORE MODERATION"; ;
                }
                else if (Session["provisionalresult"].ToString() == "Tabulated Mark Statement")
                {
                    provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Tabulated Mark Statement";
                }

                provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;

                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, 3);
                provisional_spread.Sheets[0].ColumnHeader.Cells[3, 0].Text = "     Course Code";

                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, 0, 1, 3);
                provisional_spread.Sheets[0].ColumnHeader.Cells[4, 0].Text = "     Max. Marks";

                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(5, 0, 1, 3);
                provisional_spread.Sheets[0].ColumnHeader.Cells[5, 0].Text = "     Min. Marks";

                provisional_spread.Sheets[0].ColumnHeader.Cells[6, 0].Text = "S.No";
                provisional_spread.Sheets[0].ColumnHeader.Cells[6, 1].Text = "Register No.";
                provisional_spread.Sheets[0].ColumnHeader.Cells[6, 2].Text = "Student Name";



                for (increment_subject = 0; increment_subject < subject_count; increment_subject++)
                {
                    provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, set_subj_code, 1, 4);

                    provisional_spread.Sheets[0].ColumnHeader.Cells[3, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["subject_code"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[3, set_subj_code].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["subject_no"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 1].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 1].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 2].Text = ds_getvalues.Tables[0].Rows[increment_subject]["maxtotal"].ToString();

                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["min_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["min_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 1].Text = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 1].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 2].Text = ds_getvalues.Tables[0].Rows[increment_subject]["mintotal"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 2].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["mintotal"].ToString();

                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code].Text = "CA";
                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code + 1].Text = "ES";
                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code + 2].Text = "Tot";

                    provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, (set_subj_code + 3), 3, 1);
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, (set_subj_code + 3)].Text = "P/F";

                    set_subj_code = set_subj_code + 4;
                }
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, (provisional_spread.Sheets[0].ColumnCount - 1), 4, 1);
                provisional_spread.Sheets[0].ColumnHeader.Cells[3, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "Result";

                loadstudents();




            }
            else
            {
                provisional_spread.Visible = false;
                noreclbl.Visible = true;
                pagesetpanel.Visible = false;
                setpanel.Visible = false;
                noreclbl.Text = "No Subject(s) Available";
            }
        }
        catch (Exception ex)
        {
            noreclbl.Text = ex.ToString();
            noreclbl.Visible = true;
        }
    }


    public string getmonth(string mname)
    {

        if (mname == "1")
        {
            month = "January";
            return month;
        }
        else if (mname == "2")
        {
            month = "February";

        }
        else if (mname == "3")
        {
            month = "March";

        }
        else if (mname == "4")
        {
            month = "April";

        }
        else if (mname == "5")
        {
            month = "May";

        }
        else if (mname == "6")
        {
            month = "June";

        }
        else if (mname == "7")
        {
            month = "July";

        }
        else if (mname == "8")
        {
            month = "August";

        }
        else if (mname == "9")
        {
            month = "September";
        }
        else if (mname == "10")
        {
            month = "October";
        }
        else if (mname == "11")
        {
            month = "November";

        }
        else if (mname == "12")
        {
            month = "December";

        }
        return month;
    }

    public void getsemester(string semval)
    {

        switch (semval)
        {
            case "1":
                sem_val = "FIRST";
                break;
            case "2":
                sem_val = "SECOND";
                break;
            case "3":
                sem_val = "THIRD";
                break;
            case "4":
                sem_val = "FOURTH";
                break;
            case "5":
                sem_val = "FIFTH";
                break;
            case "6":
                sem_val = "SIXTH";
                break;
            case "7":
                sem_val = "SEVENTH";
                break;
            case "8":
                sem_val = "EIGHT";
                break;
            case "9":
                sem_val = "NINTH";
                break;
            case "10":
                sem_val = "TENTH";
                break;

        }
    }
    public void loadstudents()
    {
        string subj_no = "";
        string result = "";
        int setting_value = 0;
        int mark_convert = 0;
        string attnd_perc = "";
        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
        if (ds_getvalues.Tables[1].Rows.Count > 0)
        {
            provisional_spread.Visible = true;
            noreclbl.Visible = false;
            setpanel.Visible = false;
            pagesetpanel.Visible = false;

            ////-------------------get master setting value for  mark conversion
            //SqlDataReader dr_coe;
            //con.Close();
            //con.Open();
            //cmd = new SqlCommand("select value from COE_Master_Settings where settings='Mark Conversion'", con);
            //dr_coe = cmd.ExecuteReader();
            //if (dr_coe.HasRows == true)
            //{
            //    if (dr_coe.Read())
            //    {
            //        setting_value = int.Parse(dr_coe["value"].ToString());
            //        if (setting_value == 0)
            //        {
            //            SqlDataReader dr_coe2;
            //            con.Close();
            //            con.Open();
            //            cmd1 = new SqlCommand("select value from COE_Master_Settings where settings='Mark Value'", con);
            //            dr_coe2 = cmd1.ExecuteReader();

            //            if (dr_coe2.HasRows == true)
            //            {
            //                if (dr_coe2.Read())
            //                {
            //                    mark_convert = int.Parse(dr_coe2["value"].ToString());
            //                }

            //            }

            //        }
            //    }
            //    else
            //    {
            //        noreclbl.Visible = true;
            //        noreclbl.Text = "Update Master Setting For Mark";
            //        provisional_spread.Visible = false;
            //        pagesetpanel.Visible = false;
            //        setpanel.Visible = false;
            //        return;
            //    }
            //}
            //else
            //{
            //    noreclbl.Visible = true;
            //    noreclbl.Text = "Update Master Setting For Mark";
            //    provisional_spread.Visible = false;
            //    pagesetpanel.Visible = false;
            //    setpanel.Visible = false;
            //    return;
            //}
            //-----------------------------------------------

            for (inc_stud_cnt = 0; inc_stud_cnt < ds_getvalues.Tables[1].Rows.Count; inc_stud_cnt++)
            {


                stud_rollno = ds_getvalues.Tables[1].Rows[inc_stud_cnt]["roll_no"].ToString();
                if (temp_rollno != stud_rollno)
                {
                    result_has.Clear();
                    provisional_spread.Sheets[0].RowCount++;
                    col_temp = 3;
                    temp_rollno = stud_rollno;
                }

                row_count = provisional_spread.Sheets[0].RowCount - 1;


                //-----------------------get value
                stud_name = ds_getvalues.Tables[1].Rows[inc_stud_cnt]["stud_name"].ToString();

                if (Session["provisionalresult"].ToString() == "provisionalresult")
                {
                    if (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedexternal"].ToString() == "10000")
                    {
                        int_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualInternalmark"].ToString());
                        ext_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualExternalmark"].ToString());
                        tot_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualTotal"].ToString());
                    }
                    else
                    {
                        int_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedinternal"].ToString());
                        ext_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedexternal"].ToString());
                        tot_mark = Convert.ToString(Convert.ToDouble(int_mark) + Convert.ToDouble(ext_mark));
                    }
                }
                else if (Session["provisionalresult"] == "Tabulated Mark Statement")
                {
                    if (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedexternal"].ToString() == "10000")
                    {
                        int_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualInternalmark"].ToString());
                        ext_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualExternalmark"].ToString());
                        tot_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["ActualTotal"].ToString());
                    }
                    else
                    {
                        int_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedinternal"].ToString());
                        ext_mark = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["convertedexternal"].ToString());
                        tot_mark = Convert.ToString(Convert.ToDouble(int_mark) + Convert.ToDouble(ext_mark));

                    }
                }
                result = (ds_getvalues.Tables[1].Rows[inc_stud_cnt]["result"].ToString());



                //-----------------------set value
                if (col_temp == 3)
                {
                    sno++;
                    provisional_spread.Sheets[0].Cells[row_count, 0].Text = sno.ToString();
                    provisional_spread.Sheets[0].Cells[row_count, 1].CellType = txt;
                    provisional_spread.Sheets[0].Cells[row_count, 1].Text = stud_rollno.ToString();
                    provisional_spread.Sheets[0].Cells[row_count, 2].Text = stud_name.ToString();
                }

                subj_no = ds_getvalues.Tables[1].Rows[inc_stud_cnt]["subject_no"].ToString();

                if (subj_no == provisional_spread.ColumnHeader.Cells[3, col_temp].Tag.ToString())
                {
                    // provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = int_mark.ToString();
                    //  if (setting_value == 1)
                    //Hiiden By srinath 25/5/2015
                    //{
                    //    provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = ext_mark.ToString();
                    //}
                    //End
                    //else if (setting_value == 0)
                    //{                        
                    //     ext_mark_int = Convert.ToDouble(ext_mark);
                    //     tot_ext_mark_int = Convert.ToDouble(provisional_spread.Sheets[0].ColumnHeader.Cells[4, col_temp + 1].Tag.ToString());
                    //     final_mark = Convert.ToDouble((ext_mark_int *mark_convert)/ tot_ext_mark_int)  ;

                    //    //---
                    //     //-------convert
                    //     decimal avgstudent1 = Convert.ToDecimal(final_mark);
                    //     decimal avgstudent2 = Math.Round(avgstudent1);
                    //     double avgstudent3 = Convert.ToDouble(avgstudent2);
                    //     attnd_perc = Convert.ToString(avgstudent3);
                    //     //=========
                    //     convert_mark =Convert.ToInt16( attnd_perc );
                    //    //----
                    //    provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = convert_mark.ToString();
                    //}

                    //Added By Srinath 25/5/2015

                    if (int_mark.ToString().Trim() == "-1")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = "A";
                        result = "Fail";
                    }
                    else if (int_mark.ToString().Trim() == "-2")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = "NE";
                        result = "Fail";
                    }
                    else if (int_mark.ToString().Trim() == "-3")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = "RA";
                        result = "Fail";
                    }
                    else
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = int_mark.ToString();
                    }

                    if (ext_mark.Trim() == "-1")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = "A";
                        result = "Fail";
                    }
                    else if (ext_mark.Trim() == "-2")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = "NE";
                        result = "Fail";
                    }
                    else if (ext_mark.Trim() == "-3")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = "RA";
                        result = "Fail";
                    }
                    else
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = ext_mark.ToString();
                    }
                    double num = 0;
                    if (double.TryParse(int_mark, out num) && double.TryParse(ext_mark, out num))
                    {
                        if (Convert.ToDouble(int_mark) < 0 && Convert.ToDouble(ext_mark) < 0)
                        {
                            tot_mark = "0";
                        }
                        else if (Convert.ToDouble(int_mark) < 0 && Convert.ToDouble(ext_mark) >= 0)
                        {
                            tot_mark = ext_mark.ToString();
                        }
                        else if (Convert.ToDouble(int_mark) >= 0 && Convert.ToDouble(ext_mark) < 0)
                        {
                            tot_mark = int_mark.ToString();
                        }
                    }

                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = tot_mark.ToString();
                    string min_mark = "";
                    string max_mark = "";
                    min_mark = provisional_spread.Sheets[0].ColumnHeader.Cells[5, col_temp].Tag.ToString();
                    max_mark = provisional_spread.Sheets[0].ColumnHeader.Cells[5, col_temp + 1].Tag.ToString();

                    if ((result == "Pass" || result == "Fail" || result == null) && double.Parse(int_mark.ToString()) >= double.Parse(min_mark) && double.Parse(ext_mark.ToString()) >= double.Parse(max_mark) && int_mark != "AAA" && ext_mark != "AAA")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = "P";
                    }
                    else
                    {
                        if ((result == "Pass" || result == "Fail" || result == null))
                        {
                            provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = "F";
                        }
                        else
                        {
                            provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = result;
                        }
                        if ((result != "SA" && result != "S"))
                        {
                            result_has.Add(subj_no, subj_no);
                        }

                    }
                }
                else
                {
                    provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = "-";
                    inc_stud_cnt = inc_stud_cnt - 1;

                }

                if (result_has.Count > 0)
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "F";
                }
                else
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "P";
                }
                try
                {
                    if (temp_rollno != ds_getvalues.Tables[1].Rows[inc_stud_cnt + 1]["roll_no"].ToString())
                    {
                        for (int tem = col_temp + 4; tem < (provisional_spread.Sheets[0].ColumnCount - 1); tem++)
                        {
                            provisional_spread.Sheets[0].Cells[row_count, tem].Text = "-";
                        }
                    }
                }
                catch
                {
                }
                col_temp = col_temp + 4;

            }

            if (Convert.ToInt32(provisional_spread.Sheets[0].RowCount) != 0)
            {

                Double totalRows = 0;
                totalRows = Convert.ToInt32(provisional_spread.Sheets[0].RowCount);

                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    provisional_spread.Sheets[0].PageSize = 10;
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    provisional_spread.Height = 410;
                    provisional_spread.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    provisional_spread.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    provisional_spread.Height = 200;
                }
                else
                {
                    provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(provisional_spread.Sheets[0].PageSize.ToString());
                    provisional_spread.Height = 30 + (38 * Convert.ToInt32(totalRows));
                }
                if (Convert.ToInt32(provisional_spread.Sheets[0].RowCount) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    provisional_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    CalculateTotalPages();
                }
                Session["totalPages"] = (int)Math.Ceiling(totalRows / provisional_spread.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];


            }
        }
        else
        {
            pagesetpanel.Visible = false;
            setpanel.Visible = false;
            noreclbl.Visible = true;
            noreclbl.Text = "No Student(s) Available";
            provisional_spread.Visible = false;
        }
    }

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

    public void bindbatch()
    {

        int year2;
        year2 = Convert.ToInt16(DateTime.Today.Year);
        ddlbatch.Items.Clear();
        for (int l = 0; l <= 10; l++)
        {

            ddlbatch.Items.Add(Convert.ToString(year2 - l));

        }


        //ddlbatch.Items.Clear();
        //ds = dacc.select_method_wo_parameter("bind_batch", "sp");
        //int count = ds.Tables[0].Rows.Count;
        //if (count > 0)
        //{
        //    ddlbatch.DataSource = ds;
        //    ddlbatch.DataTextField = "batch_year";
        //    ddlbatch.DataValueField = "batch_year";
        //    ddlbatch.DataBind();
        //}
        //int count1 = ds.Tables[1].Rows.Count;
        //if (count > 0)
        //{
        //    int max_bat = 0;
        //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
        //    ddlbatch.SelectedValue = max_bat.ToString();
        //    con.Close();
        //}
    }

    public void binddegree()
    {
        ddldegree.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
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
        ds = dacc.select_method("bind_degree", has, "sp");
        int count1 = ds.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddldegree.DataSource = ds;
            ddldegree.DataTextField = "course_name";
            ddldegree.DataValueField = "course_id";
            ddldegree.DataBind();
        }
    }

    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        has.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
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

        ds = dacc.select_method("bind_branch", has, "sp");
        int count2 = ds.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlbranch.DataSource = ds;
            ddlbranch.DataTextField = "dept_name";
            ddlbranch.DataValueField = "degree_code";
            ddlbranch.DataBind();
        }

    }

    public void bindsem()
    {
        ddlduration.Items.Clear();
        string duration = "";
        Boolean first_year = false;
        has.Clear();
        collegecode = Session["collegecode"].ToString();
        has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("college_code", collegecode);
        ds = dacc.select_method("bind_sem", has, "sp");
        int count3 = ds.Tables[0].Rows.Count;
        if (count3 > 0)
        {
            ddlduration.Enabled = true;
            duration = ds.Tables[0].Rows[0][0].ToString();
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
            {
                if (first_year == false)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }
                else if (first_year == true && loop_val != 2)
                {
                    ddlduration.Items.Add(loop_val.ToString());
                }

            }
        }
        else
        {
            count3 = ds.Tables[1].Rows.Count;
            if (count3 > 0)
            {
                ddlduration.Enabled = true;
                duration = ds.Tables[1].Rows[0][0].ToString();
                first_year = Convert.ToBoolean(ds.Tables[1].Rows[0][1].ToString());
                for (int loop_val = 1; loop_val <= Convert.ToInt16(duration); loop_val++)
                {
                    if (first_year == false)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }
                    else if (first_year == true && loop_val != 2)
                    {
                        ddlduration.Items.Add(loop_val.ToString());
                    }

                }
            }
            else
            {
                ddlduration.Enabled = false;
            }
        }

    }

    public void bindsec()
    {
        ddlsec.Items.Clear();
        has.Clear();
        has.Add("batch_year", ddlbatch.SelectedValue.ToString());
        has.Add("degree_code", ddlbranch.SelectedValue);
        ds = dacc.select_method("bind_sec", has, "sp");
        int count5 = ds.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlsec.DataSource = ds;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        bindbranch();
        bindsem();
        bindsec();
        noreclbl.Visible = false;
        provisional_spread.Visible = false;
        setpanel.Visible = false;
        pagesetpanel.Visible = false;
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        bindsem();
        bindsec();
        noreclbl.Visible = false;
        provisional_spread.Visible = false;
        setpanel.Visible = false;
        pagesetpanel.Visible = false;
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        bindsec();
        noreclbl.Visible = false;
        provisional_spread.Visible = false;
        setpanel.Visible = false;
        pagesetpanel.Visible = false;
    }
    protected void ddlduration_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsec();
        noreclbl.Visible = false;
        provisional_spread.Visible = false;
        setpanel.Visible = false;
        pagesetpanel.Visible = false;
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {

        noreclbl.Visible = false;
        provisional_spread.Visible = false;
        setpanel.Visible = false;
        pagesetpanel.Visible = false;
    }
    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(provisional_spread.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / provisional_spread.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }
    protected void pageddltxt_TextChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
        noreclbl.Visible = false;
        provisional_spread.CurrentPage = 0;
        pagesearch_txt.Text = "";
        try
        {
            if (pageddltxt.Text != string.Empty)
            {
                if (provisional_spread.Sheets[0].RowCount >= Convert.ToInt16(pageddltxt.Text.ToString()) && Convert.ToInt16(pageddltxt.Text.ToString()) != 0)
                {
                    errlbl.Visible = false;
                    provisional_spread.Sheets[0].PageSize = Convert.ToInt16(pageddltxt.Text.ToString());
                    CalculateTotalPages();
                }
                else
                {
                    errlbl.Visible = true;
                    errlbl.Text = "Please Enter valid Record count";
                    pageddltxt.Text = "";
                }
            }
        }
        catch
        {
            errlbl.Visible = true;
            errlbl.Text = "Please Enter valid Record count";
            pageddltxt.Text = "";
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {


        noreclbl.Visible = false;
        errlbl.Visible = false;
        provisional_spread.CurrentPage = 0;
        pagesearch_txt.Text = "";
        pagesearch_txt.Text = "";
        pageddltxt.Text = "";
        pageddltxt.Text = "";
        if (DropDownListpage.Text == "Others")
        {

            pageddltxt.Visible = true;
            pageddltxt.Focus();

        }
        else
        {
            pageddltxt.Visible = false;
            provisional_spread.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
    }

    protected void pagesearch_txt_TextChanged(object sender, EventArgs e)
    {
        errlbl.Visible = false;
        noreclbl.Visible = false;
        if (pagesearch_txt.Text.Trim() != string.Empty)
        {
            if (Convert.ToInt64(pagesearch_txt.Text) > Convert.ToInt64(Session["totalPages"]))
            {
                errlbl.Visible = true;
                errlbl.Text = "Exceed The Page Limit";
                provisional_spread.Visible = true;
                pagesearch_txt.Text = " ";
            }
            else if (Convert.ToInt64(pagesearch_txt.Text) == 0)
            {
                errlbl.Visible = true;
                errlbl.Text = "Page search should be more than 0";
                provisional_spread.Visible = true;
                pagesearch_txt.Text = " ";
            }

            else
            {
                errlbl.Visible = false;
                provisional_spread.CurrentPage = Convert.ToInt16(pagesearch_txt.Text) - 1;
                provisional_spread.Visible = true;
            }
        }
    }
    protected void RadioHeader_CheckedChanged(object sender, EventArgs e)
    {
        provisional_spread.Sheets[0].ColumnHeader.Rows[0].Visible = true;
        provisional_spread.Sheets[0].ColumnHeader.Rows[1].Visible = true;
        provisional_spread.Sheets[0].ColumnHeader.Rows[2].Visible = true;

        int i = 0;
        ddlpage.Items.Clear();
        int totrowcount = provisional_spread.Sheets[0].RowCount;
        int pages = totrowcount / 25;
        int intialrow = 1;
        int remainrows = totrowcount % 25;
        if (provisional_spread.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 25;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (i = 0; i < provisional_spread.Sheets[0].RowCount; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(provisional_spread.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / provisional_spread.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                provisional_spread.Height = 100;
            }
            else
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(provisional_spread.Sheets[0].PageSize.ToString());
                provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(provisional_spread.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //   provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }

            noreclbl.Visible = true;
            setpanel.Visible = false;


        }
        else
        {

            noreclbl.Visible = false;
            setpanel.Visible = false;
        }
    }
    protected void Radiowithoutheader_CheckedChanged(object sender, EventArgs e)
    {
        int i = 0;
        ddlpage.Items.Clear();
        int totrowcount = provisional_spread.Sheets[0].RowCount;
        int pages = totrowcount / 25;
        int intialrow = 1;
        int remainrows = totrowcount % 25;
        if (provisional_spread.Sheets[0].RowCount > 0)
        {
            int i5 = 0;
            ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
            for (i = 1; i <= pages; i++)
            {
                i5 = i;

                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                intialrow = intialrow + 25;
            }
            if (remainrows > 0)
            {
                i = i5 + 1;
                ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
            }
        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (i = 0; i < provisional_spread.Sheets[0].RowCount; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(provisional_spread.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / provisional_spread.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                provisional_spread.Height = 100;
            }
            else
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(provisional_spread.Sheets[0].PageSize.ToString());
                provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(provisional_spread.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //  provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }
            setpanel.Visible = false;
        }
        else
        {
            setpanel.Visible = false;
        }
    }
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RadioHeader.Checked == true)
        {

            for (int i = 0; i < provisional_spread.Sheets[0].RowCount; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= provisional_spread.Sheets[0].RowCount)
            {
                end = provisional_spread.Sheets[0].RowCount;
            }
            int rowstart = provisional_spread.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = provisional_spread.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = true;
            }
            provisional_spread.Sheets[0].ColumnHeader.Rows[0].Visible = true;
            provisional_spread.Sheets[0].ColumnHeader.Rows[1].Visible = true;
            provisional_spread.Sheets[0].ColumnHeader.Rows[2].Visible = true;

        }
        else if (Radiowithoutheader.Checked == true)
        {

            for (int i = 0; i < provisional_spread.Sheets[0].RowCount; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
            int end = start + 24;
            if (end >= provisional_spread.Sheets[0].RowCount)
            {
                end = provisional_spread.Sheets[0].RowCount;
            }
            int rowstart = provisional_spread.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = provisional_spread.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
            {
                provisional_spread.Sheets[0].ColumnHeader.Rows[0].Visible = true;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Visible = true;
                provisional_spread.Sheets[0].ColumnHeader.Rows[2].Visible = true;

            }
            else
            {
                provisional_spread.Sheets[0].ColumnHeader.Rows[0].Visible = false;
                provisional_spread.Sheets[0].ColumnHeader.Rows[1].Visible = false;
                provisional_spread.Sheets[0].ColumnHeader.Rows[2].Visible = false;

            }

        }
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < provisional_spread.Sheets[0].RowCount; i++)
            {
                provisional_spread.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(provisional_spread.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / provisional_spread.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Height = 335;

            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                provisional_spread.Height = 100;
            }
            else
            {
                provisional_spread.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(provisional_spread.Sheets[0].PageSize.ToString());
                provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(provisional_spread.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                provisional_spread.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                //  provisional_spread.Height = 100 + (10 * Convert.ToInt32(totalRows));
                CalculateTotalPages();
            }
            setpanel.Visible = false;
        }
        else
        {
            setpanel.Visible = false;

        }
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntPageNextBtn = provisional_spread.FindControl("Next");
        Control cntPagePreviousBtn = provisional_spread.FindControl("Prev");
        if ((cntPageNextBtn != null))
        {

            TableCell tc = (TableCell)cntPageNextBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntPagePreviousBtn.Parent;
            tr.Cells.Remove(tc);

        }

        base.Render(writer);
    }
}
