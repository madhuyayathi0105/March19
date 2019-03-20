using System;//=====================================on 29/12/2011,24/1/2012,10/3/12(sp->query)
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;

public partial class aftermoderation_external : System.Web.UI.Page
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
    SqlConnection con_main = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_main2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_main3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlCommand cmd1;
    SqlCommand cmd;
    Hashtable has = new Hashtable();
    Hashtable result_has_before = new Hashtable();
    Hashtable result_has = new Hashtable();
    DataSet ds_getvalues = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 dacc = new DAccess2();
    DataSet ds_getvalues2 = new DataSet();
    DataSet ds_getvalues3 = new DataSet();

    //-----------------------data declaration
    string usercode = "";
    string collegecode = "";
    string collnamenew1 = "", address1 = "", address2 = "", address3 = "", pincode = "", categery = "", Affliated = "";
    string sem_val = "", month = "";
    string subj_no = "";
    string result = "";
    string stud_rollno = "";
    string temp_rollno = "";
    string stud_name = "";
    string tot_mark = "";
    int sno = 0;
    string ext_mark = "";
    string int_mark = "";
    int setting_value = 0;
    int mark_convert = 0;
    int inc_stud_cnt = 0;
    int col_temp = 0;
    int row_count = 0;
    int ext_mark_int = 0, tot_ext_mark_int = 0, convert_mark = 0;
    double final_mark = 0;
    int moderate_stud_count = 0;
    int stud_count_temp = 0;
    int added_mod_mark = 0;
    string min_mark = "";
    string max_mark = "";
    double ext_final_mod_mark = 0;
    string group_user = "", singleuser = "";

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
        catch (Exception ex) { }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        Radiowithoutheader.Checked = false;
        RadioHeader.Checked = false;
        errlbl.Visible = false;
        pageddltxt.Visible = false;
        loadsubject();
    }


    public string GetFunction(string Att_strqueryst)
    {
        string sqlstr;
        sqlstr = Att_strqueryst;
        con.Close();
        con.Open();
        SqlDataReader drnew;
        SqlCommand cmd;
        cmd = new SqlCommand(sqlstr, con);
        cmd.Connection = con;
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


    public void loadsubject()
    {
        string mon_yr = "";
        int subject_count = 0;
        int column_count = 0;
        int set_subj_code = 3;
        string checkval = "";
        string checkval_val = "";
        string section_val = "";
        string check_moderatio = "";
        string check_moderatio_val = "";
        ddlpage.Items.Clear();
        provisional_spread.CurrentPage = 0;
        provisional_spread.Sheets[0].ColumnCount = 0;
        provisional_spread.Sheets[0].RowCount = 0;
        provisional_spread.Sheets[0].RowHeader.Visible = false;

        if (chk_arrear.Checked == true && chk_regular.Checked == false)
        {
            checkval = "2";
            checkval_val = " and Mark_Entry.attempts<>1";
        }
        else if (chk_arrear.Checked == false && chk_regular.Checked == true)
        {
            checkval = "1";
            checkval_val = " and Mark_Entry.attempts=1";
        }
        else
        {
            checkval = "";
            checkval_val = "";
        }

        if (ddlsec.SelectedValue.ToString() != "" && ddlsec.SelectedValue.ToString() != "All")
        {
            section_val = " and registration.sections='" + ddlsec.SelectedItem.ToString() + "'";
        }
        else
        {
            section_val = "";
        }

        if (chk_mod.Checked == true)
        {
            check_moderatio = "1";
            check_moderatio_val = "  and moderation.roll_no=mark_entry.roll_no and moderation.passmark<>0 ";
        }
        else
        {
            check_moderatio = "0";
            check_moderatio_val = "";
        }

        string exam_code = GetFunction("select top 1 exam_code from exam_details where degree_Code =" + ddlbranch.SelectedValue.ToString() + "  and current_semester =" + ddlduration.SelectedValue.ToString() + "  and batch_year =" + ddlbatch.SelectedValue.ToString() + "");
        //has.Clear();
        //has.Add("degree_code", ddlbranch.SelectedValue.ToString());
        //has.Add("sem", ddlduration.SelectedValue.ToString());
        //has.Add("batch_year",ddlbatch.SelectedValue.ToString());
        //has.Add("section", section_val);
        //has.Add("checkval", checkval);
        //has.Add("exam_code", exam_code);
        //has.Add("check_mod", check_moderatio);
        //ds_getvalues = dacc.select_method("after_getsubject_student", has, "sp");

        //=======================================================
        if (exam_code != "")
        {
            con_main.Close();
            con_main.Open();
            // string string_main = "select distinct mark_entry.subject_no,subject.subject_code,subject.min_ext_marks,subject.min_int_marks,subject.max_ext_marks,subject.max_int_marks,subject.mintotal,subject.maxtotal,mark_entry.attempts From exam_application,exam_appl_details,mark_entry,subject,registration  where exam_application.exam_code="+exam_code+"  and exam_appl_details.appl_no=exam_application.appl_no and subject.subject_no=mark_entry.subject_no and exam_application.roll_no=mark_entry.roll_no and mark_entry.subject_no=exam_appl_details.subject_no and mark_entry.roll_no=registration.roll_no "+checkval_val+" and exam_application.exam_code=mark_entry.exam_code and  registration.degree_Code ="+ddlbranch.SelectedValue.ToString()+" and registration.current_semester ="+ddlduration.SelectedValue.ToString()+" and  registration.batch_year="+ddlbatch.SelectedValue.ToString()+" order by mark_entry.subject_no ";
            string string_main = "select distinct s.subject_no,s.subject_name,s.subject_code,max_int_marks,min_ext_marks,max_ext_marks,min_int_marks,s.mintotal,s.maxtotal from exam_details ed,mark_entry,subject s,exam_application ea,exam_appl_details ead where ed.exam_code=Mark_Entry.exam_code  and Mark_Entry.exam_code=" + exam_code + "  and s.subject_no=Mark_Entry.subject_no and ead.appl_no=ea.appl_no and ea.roll_no=Mark_Entry.roll_no " + checkval_val + " and ed.degree_code=" + ddlbranch.SelectedValue.ToString() + " and ed.current_semester=" + ddlduration.SelectedValue.ToString() + " and ed.batch_year=" + ddlbatch.SelectedValue.ToString() + "";
            SqlCommand cmd_main = new SqlCommand(string_main, con_main);
            SqlDataAdapter da_main = new SqlDataAdapter(cmd_main);
            da_main.Fill(ds_getvalues);


            con_main2.Close();
            con_main2.Open();
            string string_main2 = "select distinct mark_entry.roll_no,mark_entry.subject_no,subject.subject_code,isnull (internal_mark,0) as Internal,isnull(external_mark,0) as Externalmark,isnull(bfm_external,10000) as convertedexternal,isnull(bfm_internal,10000) as convertedinternal,isnull (actual_total,0) as Total, result,registration.stud_name,mark_entry.attempts  From exam_application,exam_appl_details,mark_entry,subject,registration,moderation  where exam_application.exam_code=" + exam_code + "  and exam_appl_details.appl_no=exam_application.appl_no and subject.subject_no=mark_entry.subject_no and exam_application.roll_no=mark_entry.roll_no and mark_entry.subject_no=exam_appl_details.subject_no and mark_entry.roll_no=registration.roll_no " + checkval_val + " and  exam_application.exam_code=mark_entry.exam_code " + section_val + " and  registration.degree_Code =" + ddlbranch.SelectedValue.ToString() + " and registration.current_semester =" + ddlduration.SelectedValue.ToString() + " and  registration.batch_year =" + ddlbatch.SelectedValue.ToString() + "  " + check_moderatio_val + "";
            SqlCommand cmd_main2 = new SqlCommand(string_main2, con_main2);
            SqlDataAdapter da_main2 = new SqlDataAdapter(cmd_main2);
            da_main2.Fill(ds_getvalues2);


            con_main3.Close();
            con_main3.Open();
            string string_main3 = "select moderation.roll_no, passmark,exam_code,subject_no from moderation,registration where moderation.degree_code=" + ddlbranch.SelectedValue.ToString() + "  and moderation.semester=" + ddlduration.SelectedValue.ToString() + "  and moderation.batch_year=" + ddlbatch.SelectedValue.ToString() + " and registration.degree_code=moderation.degree_code and registration.current_semester=moderation.semester and registration.batch_year=moderation.batch_year " + section_val + " and  exam_code=" + exam_code + "  and registration.roll_no=moderation.roll_no order by moderation.roll_no,subject_no";
            SqlCommand cmd_main3 = new SqlCommand(string_main3, con_main3);
            SqlDataAdapter da_main3 = new SqlDataAdapter(cmd_main3);
            da_main3.Fill(ds_getvalues3);


            //======================================



            if (ds_getvalues.Tables[0].Rows.Count > 0)
            {
                setpanel.Visible = false;
                noreclbl.Visible = false;
                provisional_spread.Visible = true;
                subject_count = ds_getvalues.Tables[0].Rows.Count;
                column_count = (subject_count * 6) + 5;
                provisional_spread.Sheets[0].ColumnCount = column_count;//=============increment column count

                provisional_spread.Sheets[0].Columns[0].Width = 400;
                provisional_spread.Sheets[0].Columns[1].Width = 100;
                provisional_spread.Sheets[0].Columns[2].Width = 200;
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

                provisional_spread.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorBottom = Color.White;
                provisional_spread.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorBottom = Color.White;
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
                provisional_spread.Sheets[0].ColumnHeader.Cells[2, 0].Text = "PROVISIONAL RESULT OF " + ddldegree.SelectedItem.ToString() + "[ " + ddlbranch.SelectedItem.ToString() + "] " + sem_val + " SEMESTER EXAMIATIONS " + mon_yr + " - AFTER MODERATION"; ;
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

                provisional_spread.Sheets[0].ColumnHeader.Columns[2].Width = 200;

                int increment_subject = 0;
                for (increment_subject = 0; increment_subject < subject_count; increment_subject++)
                {
                    provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, set_subj_code, 1, 6);

                    provisional_spread.Sheets[0].ColumnHeader.Cells[3, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["subject_code"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[3, set_subj_code].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["subject_no"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["max_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 1].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 1].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 2].Text = "MM";
                    provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, set_subj_code + 2, 3, 1);
                    //provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code+3].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 3].Text = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 3].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["max_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 4].Text = ds_getvalues.Tables[0].Rows[increment_subject]["maxtotal"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[4, set_subj_code + 5].Text = "P/F";
                    provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(4, set_subj_code + 5, 3, 1);


                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code].Text = ds_getvalues.Tables[0].Rows[increment_subject]["min_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["min_int_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 1].Text = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 1].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 3].Text = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 3].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["min_ext_marks"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 4].Text = ds_getvalues.Tables[0].Rows[increment_subject]["mintotal"].ToString();
                    provisional_spread.Sheets[0].ColumnHeader.Cells[5, set_subj_code + 4].Tag = ds_getvalues.Tables[0].Rows[increment_subject]["mintotal"].ToString();

                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code].Text = "CA";
                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code + 1].Text = "ES/BM";
                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code + 3].Text = "ES/AM";
                    provisional_spread.Sheets[0].ColumnHeader.Cells[6, set_subj_code + 4].Text = "Tot";



                    set_subj_code = set_subj_code + 6;
                }
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, (provisional_spread.Sheets[0].ColumnCount - 2), 4, 1);
                provisional_spread.Sheets[0].ColumnHeader.Cells[3, (provisional_spread.Sheets[0].ColumnCount - 2)].Text = "RBM";
                provisional_spread.Sheets[0].ColumnHeaderSpanModel.Add(3, (provisional_spread.Sheets[0].ColumnCount - 1), 4, 1);
                provisional_spread.Sheets[0].ColumnHeader.Cells[3, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "RAM";

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
    }

    public void loadstudents()
    {
        if (ds_getvalues2.Tables[0].Rows.Count > 0)
        {
            provisional_spread.Visible = true;
            noreclbl.Visible = false;
            setpanel.Visible = false;
            pagesetpanel.Visible = false;

            ////-------------------get master setting value for  mark conversion
            SqlDataReader dr_coe;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select value from COE_Master_Settings where settings='Mark Conversion'", con);
            dr_coe = cmd.ExecuteReader();
            if (dr_coe.HasRows == true)
            {
                if (dr_coe.Read())
                {
                    setting_value = int.Parse(dr_coe["value"].ToString());
                    if (setting_value == 0)
                    {
                        SqlDataReader dr_coe2;
                        con.Close();
                        con.Open();
                        cmd1 = new SqlCommand("select value from COE_Master_Settings where settings='Mark Value'", con);
                        dr_coe2 = cmd1.ExecuteReader();

                        if (dr_coe2.HasRows == true)
                        {
                            if (dr_coe2.Read())
                            {
                                mark_convert = int.Parse(dr_coe2["value"].ToString());
                            }

                        }

                    }

                }
                else
                {
                    noreclbl.Visible = true;
                    noreclbl.Text = "Update Master Setting For Mark";
                    provisional_spread.Visible = false;
                    pagesetpanel.Visible = false;
                    setpanel.Visible = false;
                    return;
                }
            }
            else
            {
                noreclbl.Visible = true;
                noreclbl.Text = "Update Master Setting For Mark";
                provisional_spread.Visible = false;
                pagesetpanel.Visible = false;
                setpanel.Visible = false;
                return;
            }
            ////-----------------------------------------------


            moderate_stud_count = ds_getvalues3.Tables[0].Rows.Count;
            for (inc_stud_cnt = 0; inc_stud_cnt < ds_getvalues2.Tables[0].Rows.Count; inc_stud_cnt++)
            {


                stud_rollno = ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["roll_no"].ToString();
                if (temp_rollno != stud_rollno)
                {
                    result_has.Clear();
                    result_has_before.Clear();
                    provisional_spread.Sheets[0].RowCount++;
                    col_temp = 3;
                    temp_rollno = stud_rollno;
                }

                row_count = provisional_spread.Sheets[0].RowCount - 1;


                //-----------------------get value

                stud_name = ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["stud_name"].ToString();
                result = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["result"].ToString());
                if (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["convertedinternal"].ToString() == "10000")
                {
                    int_mark = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["Internal"].ToString());
                    ext_mark = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["Externalmark"].ToString());
                    tot_mark = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["Total"].ToString());
                }
                else
                {
                    int_mark = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["convertedinternal"].ToString());
                    ext_mark = (ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["convertedexternal"].ToString());
                    tot_mark = Convert.ToString(Convert.ToDouble(int_mark) + Convert.ToDouble(ext_mark));
                }


                //-----------------------set value
                if (col_temp == 3)
                {
                    sno++;
                    provisional_spread.Sheets[0].Cells[row_count, 0].Text = sno.ToString();
                    provisional_spread.Sheets[0].Cells[row_count, 1].Text = stud_rollno.ToString();
                    provisional_spread.Sheets[0].Cells[row_count, 2].Text = stud_name.ToString();
                }

                subj_no = ds_getvalues2.Tables[0].Rows[inc_stud_cnt]["subject_no"].ToString();

                if (subj_no == provisional_spread.ColumnHeader.Cells[3, col_temp].Tag.ToString())
                {
                    provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = int_mark.ToString();
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = ext_mark.ToString();

                    if (moderate_stud_count > 0)
                    {
                        if (moderate_stud_count > stud_count_temp)
                        {
                            if (stud_rollno == ds_getvalues3.Tables[0].Rows[stud_count_temp]["roll_no"].ToString())
                            {
                                if (subj_no == ds_getvalues3.Tables[0].Rows[stud_count_temp]["subject_no"].ToString())
                                {

                                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = ds_getvalues3.Tables[0].Rows[stud_count_temp]["passmark"].ToString();

                                    stud_count_temp++;
                                }
                                else
                                {
                                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "0";
                                }
                            }
                            else
                            {
                                provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "0";
                            }
                        }
                        else
                        {
                            provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "0";
                        }
                    }
                    else
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "0";
                    }
                    string max_mark_ext = "";
                    string max_mark_int = "";
                    string min_mark_ext = "";
                    string min_mark_int = "";

                    added_mod_mark = Convert.ToInt16(provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text);
                    ext_final_mod_mark = ((Convert.ToDouble(ext_mark)) + added_mod_mark);
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = ext_final_mod_mark.ToString();
                    min_mark_ext = provisional_spread.Sheets[0].ColumnHeader.Cells[5, col_temp + 1].Tag.ToString();
                    min_mark_int = provisional_spread.Sheets[0].ColumnHeader.Cells[5, col_temp].Tag.ToString();
                    max_mark_ext = provisional_spread.Sheets[0].ColumnHeader.Cells[4, col_temp + 1].Tag.ToString();
                    max_mark_int = provisional_spread.Sheets[0].ColumnHeader.Cells[4, col_temp].Tag.ToString();

                    double mod_total_ext = 0;
                    //=============total
                    if (setting_value == 0)
                    {
                        mod_total_ext = Math.Round(Convert.ToDouble((ext_final_mod_mark * mark_convert) / Convert.ToDouble(max_mark_ext)));
                    }
                    else
                    {
                        mod_total_ext = Convert.ToDouble(ext_final_mod_mark);
                    }

                    double mod_tot_int = Convert.ToDouble(int_mark);
                    double round_value_tot = mod_total_ext + mod_tot_int;

                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 4].Text = round_value_tot.ToString();
                    //=========================

                    if ((result == "Pass" || result == "Fail" || result == null) && Convert.ToDouble(int_mark.ToString()) >= int.Parse(min_mark_int) && Convert.ToDouble(ext_final_mod_mark.ToString()) >= int.Parse(min_mark_ext) && int_mark != "AAA" && ext_mark != "AAA" && int_mark != "0" && ext_mark != "0")
                    {
                        provisional_spread.Sheets[0].Cells[row_count, col_temp + 5].Text = "P";
                    }
                    else
                    {
                        if ((result == "Pass" || result == "Fail" || result == null))
                        {
                            provisional_spread.Sheets[0].Cells[row_count, col_temp + 5].Text = "F";
                        }
                        else
                        {
                            provisional_spread.Sheets[0].Cells[row_count, col_temp + 5].Text = result;
                        }
                        if ((result != "Pass" && result != "SA" && result != "S"))
                        {
                            result_has.Add(subj_no, subj_no);
                        }

                    }

                    //--------------------befor mod result
                    if ((result == "Pass" || result == "Fail" || result == null) && Convert.ToDouble(int_mark.ToString()) >= int.Parse(min_mark_int) && Convert.ToDouble(ext_mark.ToString()) >= int.Parse(min_mark_ext) && int_mark != "AAA" && ext_mark != "AAA" && int_mark != "0" && ext_mark != "0")
                    {
                    }
                    else
                    {
                        if ((result != "Pass" && result != "SA" && result != "S"))
                        {
                            result_has_before.Add(subj_no, subj_no);
                        }

                    }
                    //---------------------------------------

                }
                else
                {
                    provisional_spread.Sheets[0].Cells[row_count, col_temp].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 1].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 2].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 3].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 4].Text = "-";
                    provisional_spread.Sheets[0].Cells[row_count, col_temp + 5].Text = "-";

                    inc_stud_cnt = inc_stud_cnt - 1;

                }

                if (result_has.Count > 0)
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "Fail";
                }
                else
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 1)].Text = "Pass";
                }

                //----before mod
                if (result_has_before.Count > 0)
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 2)].Text = "Fail";
                }
                else
                {
                    provisional_spread.Sheets[0].Cells[row_count, (provisional_spread.Sheets[0].ColumnCount - 2)].Text = "Pass";
                }
                //===================

                try
                {
                    if (temp_rollno != ds_getvalues2.Tables[0].Rows[inc_stud_cnt + 1]["roll_no"].ToString())
                    {
                        for (int tem = col_temp + 6; tem < (provisional_spread.Sheets[0].ColumnCount - 2); tem++)
                        {
                            provisional_spread.Sheets[0].Cells[row_count, tem].Text = "-";
                        }
                    }
                }
                catch
                {
                }
                col_temp = col_temp + 6;

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
            ddlsec.Items.Insert(0, "All");
            ddlsec.Enabled = true;
        }
        else
        {
            ddlsec.Enabled = false;
        }
    }


    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {

        //bindbranch();
        //bindsem();
        //bindsec();
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
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);

    }
}