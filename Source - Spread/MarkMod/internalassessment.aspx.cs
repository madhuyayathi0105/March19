using System; //----------------modified on 12.04.12,modified on 08.06.12(Total,AdmissionNO,Sgnature clmn visible false)
//modified on 29.06.12 by  PRABHA (added chkbox list for subtype, sub name and code in header)
//modified on 02.07.12,05.07.12(print setting changes)
//====modified on 06.07.12(clmn hdr disply,spanning,ISOCODE)
//==modifed on 24.07.12 (call the func_header wn click go),disable func_print_master_setting in btngenerate
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Drawing;
using System.Web.UI.DataVisualization.Charting;


public partial class internalassessment : System.Web.UI.Page
{
    
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    string CollegeCode;
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    Hashtable arrcount = new Hashtable();
    string Master = "";
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    DataSet ds_load = new DataSet();
    DAccess2 daccess2 = new DAccess2();
    static Boolean forschoolsetting = false;// Added by sridharan

    //'---------------------for print master start
    string maxintmark = "";
    int child_sub_count = 0;
    int final_print_col_cnt = 0;
    string footer_text = "";
    int temp_count = 0;
    int split_col_for_footer = 0;
    int footer_balanc_col = 0;
    int footer_count = 0;
    string strorder = "";
    string strregorder = "";
    ////----------------------------------------new myth 08.12
    string collnamenew1 = "";
    string address1 = "";
    string address2 = "";
    string address = "";
    string address3 = "";//added on 24.07.12
    string pincode = "";//''
    string state = "";//''
    string category = "";//''
    string affliated = "";//''
    string affliatedby = "";//''
    string Phoneno = "";
    string Faxno = "";
    string phnfax = "";
    int subjectcount = 0;
    string district = "";
    string email = "";
    string website = "";
    string form_heading_name = "";
    string batch_degree_branch = "";
    string new_header_string = "";
    string[] new_header_string_split;
    DataSet dsprint = new DataSet();
    DataSet examds = new DataSet();
    int right_logo_clmn = 0;
    string affiliated = "";
    string includePastout = string.Empty;


    //added by rajasekar 20/09/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    int coln = 0;
    int firstcolcount = 0;
 
    ArrayList subno = new ArrayList();
    ArrayList minmark = new ArrayList();
    ArrayList maxmark = new ArrayList();
    ArrayList subcode = new ArrayList();
    ArrayList subacr = new ArrayList();
    ArrayList rollnum = new ArrayList();
    


    //----------------------end print master

    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con4 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con5 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection studcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    //protected override void Render(System.Web.UI.HtmlTextWriter writer)
    //{

    //    Control cntUpdateBtn = sprdviewrcrd.FindControl("Update");
    //    Control cntCancelBtn = sprdviewrcrd.FindControl("Cancel");
    //    Control cntCopyBtn = sprdviewrcrd.FindControl("Copy");
    //    Control cntCutBtn = sprdviewrcrd.FindControl("Clear");
    //    Control cntPasteBtn = sprdviewrcrd.FindControl("Paste");
    //    Control cntPagePrintBtn = sprdviewrcrd.FindControl("Print");

    //    if ((cntUpdateBtn != null))
    //    {

    //        TableCell tc = (TableCell)cntUpdateBtn.Parent;
    //        TableRow tr = (TableRow)tc.Parent;

    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntCancelBtn.Parent;
    //        tr.Cells.Remove(tc);


    //        tc = (TableCell)cntCopyBtn.Parent;
    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntCutBtn.Parent;
    //        tr.Cells.Remove(tc);

    //        tc = (TableCell)cntPasteBtn.Parent;
    //        tr.Cells.Remove(tc);


    //    }

    //    base.Render(writer);
    //}
    public void bindbatch()
    {
        ////batch modified by raj
        ddlbatch.Items.Clear();
        string sqlstring = string.Empty;
        int max_bat = 0;
        con.Close();
        con.Open();


        cmd = new SqlCommand(" select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>''  and delflag=0 and exam_flag<>'debar' order by batch_year", con);
        SqlDataAdapter da1 = new SqlDataAdapter(cmd);
        DataSet ds1 = new DataSet();
        da1.Fill(ds1);
        ddlbatch.DataSource = ds1;
        ddlbatch.DataValueField = "batch_year";
        ddlbatch.DataBind();
        //----------------display max year value 
        sqlstring = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' ";
        max_bat = Convert.ToInt32(GetFunction(sqlstring));
        ddlbatch.SelectedValue = max_bat.ToString();
        con.Close();
        //binddegree();
        //ddlbatch.Items.Clear();
        //ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
        //int count = ds_load.Tables[0].Rows.Count;
        //if (count > 0)
        //{
        //    //ddlbatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
        //    int i1 = 0;
        //    for (int i = 0; i < count; i++)
        //    {
        //        ddlbatch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + ""));
        //        i1++;
        //    }
        //}
        //int count1 = ds_load.Tables[1].Rows.Count;
        //if (count > 0)
        //{
        //    int max_bat = 0;
        //    max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
        //    ddlbatch.SelectedValue = max_bat.ToString();
        //    con.Close();
        //}
    }
    public void bindbranch()
    {

        ddlbranch.Items.Clear();
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (ddldegree.SelectedItem.Text != "All")
        {
            hat.Clear();

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser.ToString());
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldegree.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            ds_load = daccess2.select_method("bind_branch", hat, "sp");
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {

                int i1 = 0;
                for (int i = 0; i < count2; i++)
                {
                    ddlbranch.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["dept_name"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                    i1 = i;
                }
                //i1++;
                //ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
        }
        else if (ddldegree.SelectedItem.Text == "All")
        {
            string bindbranch = "select distinct degree.degree_code,department.dept_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " ";
            SqlDataAdapter dabindbranch = new SqlDataAdapter(bindbranch, con);
            DataSet dsbindbranch = new DataSet();
            con.Close();
            con.Open();
            dabindbranch.Fill(dsbindbranch);
            if (dsbindbranch.Tables[0].Rows.Count > 0)
            {
                int i1 = 0;
                for (int i = 0; i < dsbindbranch.Tables[0].Rows.Count; i++)
                {
                    ddlbranch.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + dsbindbranch.Tables[0].Rows[i]["dept_name"].ToString() + "", "" + dsbindbranch.Tables[0].Rows[i]["degree_code"].ToString() + ""));
                    i1 = i;
                }
                i1++;
                ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
            }
        }
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
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess2.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {


            int i1 = 0;
            for (int i = 0; i < count1; i++)
            {
                ddldegree.Items.Insert(i, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["course_name"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["course_id"].ToString() + ""));
                i1 = i;
            }
            //i1++;
            //ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
        }
    }
    public void bindsem()
    {
        try
        {
            //--------------------semester load
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            con.Close();
            con.Open();
            SqlDataReader dr;
            if (ddlbranch.SelectedItem.Text != "All")
            {
                cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and batch_year=" + ddlbatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
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
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }

                    }
                    //i++;
                    //ddlsem.Items.Insert(i, new System.Web.UI.WebControls.ListItem("All"," "));
                    //ddlsem.Items.Add("All");
                }
                else
                {
                    dr.Close();
                    SqlDataReader dr1;
                    cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlbranch.SelectedValue.ToString() + " and college_code=" + Session["collegecode"] + "", con);
                    ddlsem.Items.Clear();
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
                                ddlsem.Items.Add(i.ToString());
                            }
                            else if (first_year == true && i != 2)
                            {
                                ddlsem.Items.Add(i.ToString());
                            }

                        }
                        //i++;
                        //ddlsem.Items.Insert(i, new System.Web.UI.WebControls.ListItem("All", " "));
                        //ddlsem.Items.Add("All");
                    }

                    dr1.Close();
                }
            }
            //     ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
            con.Close();
            if (ddlbranch.SelectedItem.Text == "All")
            {
                con.Close();
                con.Open();
                SqlDataReader dr2;
                cmd = new SqlCommand("select top 1 duration,first_year_nonsemester from degree where college_code=" + Session["collegecode"] + " order by duration desc", con);
                dr2 = cmd.ExecuteReader();
                dr2.Read();
                if (dr2.HasRows == true)
                {
                    first_year = Convert.ToBoolean(dr2[1].ToString());
                    duration = Convert.ToInt16(dr2[0].ToString());
                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            ddlsem.Items.Add(i.ToString());
                        }

                    }
                }
            }
        }
        catch
        {
        }
    }
    public void bindsec()
    {
        ddlsec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlbatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlbranch.SelectedValue.ToString());
        ds_load = daccess2.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlsec.DataSource = ds_load;
            ddlsec.DataTextField = "sections";
            ddlsec.DataValueField = "sections";
            ddlsec.DataBind();
            ddlsec.Enabled = true;
            ddlsec.Items.Add("All");
        }
        else
        {
            ddlsec.Enabled = false;
            ddlsec.Items.Add("All");
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string staff_code = "";
        staff_code = (string)Session["staff_code"];
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
        lblerr.Visible = false;
        lblnorec.Visible = false;
        lblsub_name_code.Visible = false;
        txtsub_name_code.Visible = false;
        Panel4.Visible = false;
        chckstaff.Visible = false;
        chcksec.Visible = false;
       
        if (!Page.IsPostBack)
        {
            Master = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            con3.Close();
            con3.Open();
            SqlDataReader mtrdr;
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            SqlCommand mtcmd = new SqlCommand(Master, con3);
            chkRoundoff1.Checked = true;
            mtrdr = mtcmd.ExecuteReader();
            {
                if (mtrdr.HasRows)
                {
                    while (mtrdr.Read())
                    {
                        if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                    }

                }
            }
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnExcel.Visible = false;
            
            Showgrid.Visible = false;
            btnmasterprint.Visible = false;
            btnDirectPrint.Visible = false;
            pageset_pnl.Visible = false;
            //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Stud Name";
            //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Roll No";
            


            for (int c = 0; c < chklscolumn.Items.Count; c++)
            {
                chklscolumn.Items[c].Selected = true;
            }

            if (Request.QueryString["val"] != null)
            {
                string get_pageload_value = Request.QueryString["val"];
                if (get_pageload_value.ToString() != null)
                {
                    string[] spl_load_val = get_pageload_value.Split('$');//split criteria value and other val
                    string[] spl_pageload_val = spl_load_val[0].Split(',');//split the bat,deg,bran,sem,sec val
                    bindbatch();
                    ddlbatch.SelectedIndex = Convert.ToInt32(spl_pageload_val[0].ToString());

                    binddegree();
                    ddldegree.SelectedIndex = Convert.ToInt32(spl_pageload_val[1].ToString());
                    if (ddldegree.Text != "")
                    {
                        bindbranch();
                        ddlbranch.SelectedIndex = Convert.ToInt32(spl_pageload_val[2].ToString());
                    }
                    else
                    {
                        lblnorec.Text = "Give degree rights to the staff";
                        lblnorec.Visible = true;

                    }

                    bindsem();
                    ddlsem.SelectedIndex = Convert.ToInt32(spl_pageload_val[3].ToString());
                    bindsec();
                    ddlsec.SelectedIndex = Convert.ToInt32(spl_pageload_val[4].ToString());

                    //--------------------------------29/6/12 PRABHA
                    bindsubject_type();
                    bindsubject();
                    //-------------------------------------added on 05.07.12 subjtype
                    string[] spl_subj_type = spl_load_val[1].Split('-');
                    if (spl_subj_type.GetUpperBound(0) > 0)
                    {
                        for (int subj = 0; subj < spl_subj_type.GetUpperBound(0) + 1; subj++)
                        {
                            if (chkbxlistsubjtype.Items[subj].Value == spl_subj_type[subj].ToString())
                            {
                                chkbxlistsubjtype.Items[Convert.ToInt32(subj)].Selected = true;
                            }
                        }
                    }
                    //------------------------------added on 05.07.12 subj 
                    string[] spl_subj = spl_load_val[2].Split('-');
                    if (spl_subj.GetUpperBound(0) > 0)
                    {
                        for (int subj = 0; subj < spl_subj.GetUpperBound(0) + 1; subj++)
                        {
                            if (chkbxlistsubj.Items[subj].Value == spl_subj[subj].ToString())
                            {
                                chkbxlistsubj.Items[Convert.ToInt32(subj)].Selected = true;
                            }
                        }
                    }
                    //---------------------
                    string[] spl_subjcode = spl_load_val[3].Split('-');
                    if (spl_subjcode.GetUpperBound(0) > 0)
                    {
                        for (int subj = 0; subj < spl_subjcode.GetUpperBound(0) + 1; subj++)
                        {
                            if (chkbxlisisub_name_code.Items[subj].Value == spl_subjcode[subj].ToString())
                            {
                                chkbxlisisub_name_code.Items[Convert.ToInt32(subj)].Selected = true;
                            }
                        }
                    }

                    //---------------------
                    btngenerate_Click(sender, e);
                    //func_Print_Master_Setting();
                    //func_header();
                    //function_footer();

                }
            }
            else
            {



                bindbatch();
                binddegree();
                if (ddldegree.Items.Count > 0)
                {
                    bindbranch();
                    bindsem();
                    bindsec();
                    bindsubject_type();
                    bindsubject();
                }
                if (ddldegree.Text == "")
                {
                    lblnorec.Text = "Give degree rights to the staff";
                    lblnorec.Visible = true;
                    return;
                }
            }
            // Added By Sridharan 12 Mar 2015
            //{
            string grouporusercodeschool = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercodeschool = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercodeschool = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }
            DataSet schoolds = new DataSet();
            string sqlschool = "select * from Master_Settings where settings='schoolorcollege' and " + grouporusercodeschool + "";
            schoolds.Clear();
            schoolds.Dispose();
            schoolds = daccess2.select_method_wo_parameter(sqlschool, "Text");
            if (schoolds.Tables[0].Rows.Count > 0)
            {
                string schoolvalue = schoolds.Tables[0].Rows[0]["value"].ToString();
                if (schoolvalue.Trim() == "0")
                {
                    forschoolsetting = true;
                    //lblcollege.Text = "School";
                    lblbatch.Text = "Year";
                    lbldegree.Text = "School Type";
                    lblbranch.Text = "Standard";
                    lblsem.Text = "Term";
                    //Label1.Text = "Test Mark R11-Continuous Assessment Report";
                    //lbldeg.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 229px;    position: absolute;    top: 210px;");
                    //tbdeg.Attributes.Add("Style", "   font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 328px;    margin-right: 15px;    position: absolute;    top: 210px;    width: 100px;");
                    //lblbranch.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 439px;    position: absolute;    top: 212px;    width: 90px;");
                    //txtbranch.Attributes.Add("Style", "font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    height: 20px;    left: 509px;    position: absolute;    top: 210px;    width: 180px;");
                    //lblsection.Attributes.Add("Style", " color: Black;    display: inline-block;    font-family: Book Antiqua;    font-size: medium;    font-weight: bold;    left: 702px;    position: absolute;    top: 211px;    width: 100px;");


                }
                else
                {
                    forschoolsetting = false;
                }
            }
            else
            {
                forschoolsetting = false;
            }

            //} Sridharan
        }
    }

    public void bindsubject_type()
    {
        chkbxlistsubjtype.Items.Clear();
        DataSet ds_subjtype = new DataSet();
        con.Close();
        con.Open();
        string subjtype_str = "Select distinct(SS.Subject_Type),s.subtype_no  from Subject as s, Sub_Sem as ss ,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and  SS.Syll_Code =S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code = " + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedItem.ToString() + " and SMas.Semester = " + ddlsem.SelectedItem.ToString() + " ";
        SqlDataAdapter da_subjtype = new SqlDataAdapter(subjtype_str, con);
        da_subjtype.Fill(ds_subjtype);
        if (ds_subjtype.Tables[0].Rows.Count > 0)
        {
            for (int row_cnt = 0; row_cnt < ds_subjtype.Tables[0].Rows.Count; row_cnt++)
            {
                chkbxlistsubjtype.Items.Add(ds_subjtype.Tables[0].Rows[row_cnt][0].ToString());
                chkbxlistsubjtype.Items[row_cnt].Value = ds_subjtype.Tables[0].Rows[row_cnt][1].ToString();
                chkbxlistsubjtype.Items[row_cnt].Selected = true;
            }
            txtsubjtype.Text = "Subject Type(" + ds_subjtype.Tables[0].Rows.Count + ")";
            txtsubjtype.Enabled = true;
            chksubjtype.Checked = true;
        }

        else
        {
            txtsubjtype.Text = "";
            txtsubjtype.Enabled = false;
        }

    }
    public void bindsubject()
    {
        chkbxlistsubj.Items.Clear();
        string get_subjtype_no = "";
        for (int item_cnt = 0; item_cnt < chkbxlistsubjtype.Items.Count; item_cnt++)
        {
            if (chkbxlistsubjtype.Items[item_cnt].Selected == true)
            {
                if (get_subjtype_no == "")
                {
                    get_subjtype_no = "(" + chkbxlistsubjtype.Items[item_cnt].Value;
                }
                else
                {
                    get_subjtype_no = get_subjtype_no + "," + chkbxlistsubjtype.Items[item_cnt].Value;
                }
            }
        }
        if (get_subjtype_no != "")
        {
            get_subjtype_no = get_subjtype_no + ")";


            DataSet ds_subj = new DataSet();
            con.Close();
            con.Open();
            string subj_str = "";
            //staff_code = (string)Session["staff_code"];
            if (Session["Staff_Code"].ToString() == "")
            {
                subj_str = "Select Distinct s.subject_name,s.subject_no,SS.Subject_Type ,s.subtype_no  from Subject as s, Sub_Sem as ss ,Syllabus_Master as SMas where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and  SS.Syll_Code =S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code = " + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedItem.ToString() + " and SMas.Semester = " + ddlsem.SelectedItem.ToString() + " and s.subtype_no in " + get_subjtype_no + "";
            }
            else if (Session["Staff_Code"].ToString() != "")
            {
                subj_str = "Select Distinct s.subject_name,s.subject_no,SS.Subject_Type ,s.subtype_no  from Subject as s, Sub_Sem as ss ,Syllabus_Master as SMas,staff_selector stsel where SMas.Syll_Code = S.Syll_Code and SMas.Syll_Code = SS.Syll_Code and  SS.Syll_Code =S.Syll_Code and S.SubType_no = SS.Subtype_no and SS.Promote_Count = 1 and SMas.Degree_Code = " + ddlbranch.SelectedValue.ToString() + " and SMas.Batch_Year =" + ddlbatch.SelectedItem.ToString() + " and SMas.Semester = " + ddlsem.SelectedItem.ToString() + " and s.subtype_no in " + get_subjtype_no + " and s.subject_no= stsel.subject_no and stsel.staff_code='" + Session["Staff_Code"].ToString() + "'";
            }
            SqlDataAdapter da_subj = new SqlDataAdapter(subj_str, con);
            da_subj.Fill(ds_subj);
            if (ds_subj.Tables[0].Rows.Count > 0)
            {
                for (int row_cnt = 0; row_cnt < ds_subj.Tables[0].Rows.Count; row_cnt++)
                {
                    chkbxlistsubj.Items.Add(ds_subj.Tables[0].Rows[row_cnt][0].ToString());
                    chkbxlistsubj.Items[row_cnt].Value = ds_subj.Tables[0].Rows[row_cnt][1].ToString();
                    chkbxlistsubj.Items[row_cnt].Selected = true;
                }
            }

            txtsubj.Text = "Subject(" + ds_subj.Tables[0].Rows.Count + ")";
            txtsubj.Enabled = true;
            chksubj.Checked = true;
        }
        else
        {
            txtsubj.Text = "";
            txtsubj.Enabled = false;
        }
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnmasterprint.Visible = false;
        btnDirectPrint.Visible = false;
        pageset_pnl.Visible = false;
        txtmrkoutof.Text = "";
        //binddegree();
        //bindbranch();
        //bindsem();
        //bindsec();
        bindsubject_type();
        bindsubject();
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnmasterprint.Visible = false;
        btnDirectPrint.Visible = false;
        pageset_pnl.Visible = false;
        txtmrkoutof.Text = "";
        bindbranch();
        bindsem();
        bindsec();
        bindsubject_type();
        bindsubject();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnmasterprint.Visible = false;
        btnDirectPrint.Visible = false;
        pageset_pnl.Visible = false;
        txtmrkoutof.Text = "";
        bindsem();
        bindsec();
        bindsubject_type();
        bindsubject();
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
        btnExcel.Visible = false;
        bindsec();
        bindsubject_type();
        bindsubject();
        lblnorec.Visible = false;
        Showgrid.Visible = false;
        btnmasterprint.Visible = false;
        btnDirectPrint.Visible = false;
        pageset_pnl.Visible = false;
        txtmrkoutof.Text = "";
    }
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
       // Session["column_header_row_count"] = sprdviewrcrd.Sheets[0].ColumnHeader.RowCount;

        string sections = ddlsec.SelectedValue.ToString();
        if (sections.ToString() == "All" || sections.ToString() == string.Empty || sections.ToString() == "-1")
        {
            sections = "";
        }
        else
        {
            sections = " Sec-" + sections + "";
        }

        DateTime date_today = DateTime.Now;
        int yr_now = Convert.ToInt32(date_today.ToString("yyyy"));
        string academyear = (yr_now.ToString() + "-" + (yr_now + 1).ToString());
        int semye = Convert.ToInt32(ddlsem.SelectedItem.ToString());
        string semval = "ODD SEMESTER";
        if (semye % 2 == 0)
        {
            semval = "EVEN SEMESTER";
        }
        string acdemicyear = daccess2.GetFunction("select value from master_settings where settings='Academic year'");
        string[] spa = acdemicyear.Split(',');
        if (spa.GetUpperBound(0) == 1)
        {
            semval = semval + " , " + spa[0] + " -" + spa[1];
        }
        string degreedetails = semval + "$CONSOLIDATED MARK LIST - INTERNAL ASSESSMENT" + '@' + "Degree :" + ddlbatch.SelectedItem.ToString() + '-' + ddldegree.SelectedItem.ToString() + '[' + ddlbranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlsem.SelectedItem.ToString() + sections;
        string pagename = "internalassessment.aspx";
        //Printcontrol.loadspreaddetails(sprdviewrcrd, pagename, degreedetails);
        string ss = Session["usercode"].ToString();
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;


    }
    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con.Close();
        con.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con);
        SqlDataReader drnew;
        SqlCommand cmd = new SqlCommand(sqlstr);
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
    public void filteration()
    {

        string orderby_Setting = GetFunction("select value from master_Settings where settings='order_by'");

        if (orderby_Setting == "")
        {
            strorder = "ORDER BY r.Roll_No";
            strregorder = "ORDER BY registration.Roll_No";
        }
        else
        {
            if (orderby_Setting == "0")
            {
                strorder = "ORDER BY r.Roll_No";
                strregorder = "ORDER BY registration.Roll_No";
            }
            else if (orderby_Setting == "1")
            {
                strorder = "ORDER BY r.Reg_No";
                strregorder = "ORDER BY registration.Reg_No";
            }
            else if (orderby_Setting == "2")
            {
                strregorder = "ORDER BY registration.Stud_Name";
                strorder = "ORDER BY r.Stud_Name";
            }
            else if (orderby_Setting == "0,1,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,1")
            {
                strorder = "ORDER BY r.Roll_No,r.Reg_No";
                strregorder = "ORDER BY registration.Roll_No,registration.Reg_No";
            }
            else if (orderby_Setting == "1,2")
            {
                strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Reg_No,registration.Stud_Name";
            }
            else if (orderby_Setting == "0,2")
            {
                strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                strregorder = "ORDER BY registration.Roll_No,registration.Stud_Name";
            }
        }

    }
    protected void btngenerate_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            Printcontrol.Visible = false;
            Showgrid.Visible = false;
            btnmasterprint.Visible = false;
            btnDirectPrint.Visible = false;
            btnExcel.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Text = "";
            Boolean studflag = false;
            FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();
            string degrecode = ddlbranch.SelectedValue.ToString();
            string batchyear = ddlbatch.SelectedValue.ToString();
            string sem = ddlsem.SelectedValue.ToString();
            string sec = "";
            string secval = "";
            if (ddlsec.Enabled == true && ddlsec.Items.Count > 0)
            {
                if (ddlsec.SelectedIndex.ToString() != "-1" && ddlsec.SelectedItem.ToString() != "All")
                {
                    sec = ddlsec.SelectedItem.ToString();
                    secval = " and sections='" + sec + "'";
                }
            }
            


            Dictionary<string, double> dicsubpresent = new Dictionary<string, double>();
            Dictionary<string, double> dicsubpass = new Dictionary<string, double>();
            Dictionary<string, double> dicsubfail = new Dictionary<string, double>();
            Dictionary<string, double> dicsubabsent = new Dictionary<string, double>();
            Dictionary<string, double> dicsubod = new Dictionary<string, double>();
            Dictionary<string, double> dicsubtotal = new Dictionary<string, double>();

            Hashtable hatsubject = new Hashtable();
            for (int su = 0; su < chkbxlistsubj.Items.Count; su++)
            {
                if (chkbxlistsubj.Items[su].Selected == true)
                {
                    hatsubject.Add(chkbxlistsubj.Items[su].Value.ToString(), chkbxlistsubj.Items[su].Value.ToString());
                }
            }
            if (hatsubject.Count == 0)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select The Subject And Then Proceed";
                return;
            }

            string getoutof = txtmrkoutof.Text.ToString();
            Double conveoutof = 0;
            if (getoutof.Trim() != "")
            {
                conveoutof = Convert.ToDouble(getoutof);
            }

            string sect = string.Empty;
            if (ddlsec.Enabled == true)
            {
                if (ddlsec.SelectedItem.Text.ToString().ToLower() == "all")
                {
                    sect = string.Empty;
                }
                else
                {
                  //  sect = ddlsec.SelectedValue.ToString();
                    sect =  ddlsec.SelectedValue.ToString() ;
                }
               // sect=" and sections='"+secti+"'";
            }


            CheckPassedOut();
            hat.Clear();
            filteration();
            string filterwithsection = "batch_year ='" + ddlbatch.SelectedValue.ToString() + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester >= '" + ddlsem.SelectedValue.ToString() + "' and RollNo_Flag<>0 "+includePastout+" and delflag=0 and  exam_flag <> 'DEBAR' and delflag=0  " + strregorder + " ";
            string filterwithoutsection = "batch_year ='" + ddlbatch.SelectedValue.ToString() + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester >= '" + ddlsem.SelectedValue.ToString() + "' and sections='" + ddlsec.SelectedValue.ToString() + "' and RollNo_Flag<>0 " + includePastout + " and delflag=0 and  exam_flag <> 'DEBAR' and delflag=0  " + strregorder + " ";
            hat.Add("@batchyear", Convert.ToInt32(ddlbatch.SelectedValue.ToString()));
            hat.Add("@degreecode", Convert.ToInt32(ddlbranch.SelectedValue.ToString()));
            hat.Add("@cur_sem", Convert.ToInt32(ddlsem.SelectedValue.ToString()));
            hat.Add("@sections", ddlsec.SelectedValue.ToString());
           // hat.Add("@sections", sect);
            hat.Add("@filterwithsection", filterwithsection.ToString());
            hat.Add("@filterwithoutsection", filterwithoutsection.ToString());
            DataSet examds = daccess2.select_method("internalassessgetsubject", hat, "sp");
           
            if (examds.Tables[0].Rows.Count > 0)
            {


                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);

                dtrow = dtl.NewRow();
                dtl.Rows.Add(dtrow);
                int colu = 0;

                if (chklscolumn.Items[0].Selected == true)
                {
                    
                    dtl.Columns.Add("S.No", typeof(string));
                    dtl.Rows[0][colu] = "S.No";
                    colu++;
                }
                

                
                if (chklscolumn.Items[1].Selected == true)
                {
                    
                    dtl.Columns.Add("ROLL NO", typeof(string));
                    dtl.Rows[0][colu] = "ROLL NO";
                    colu++;
                }
               

                
                if (chklscolumn.Items[2].Selected == true)
                {
                    
                    dtl.Columns.Add("REG NO", typeof(string));
                    dtl.Rows[0][colu] = "REG NO";
                    colu++;
                }
                

                
                if (chklscolumn.Items[3].Selected == true)
                {
                    
                    dtl.Columns.Add("STUDENT NAME", typeof(string));
                    dtl.Rows[0][colu] = "STUDENT NAME";
                    colu++;
                }
                

                
                if (chklscolumn.Items[4].Selected == true)
                {
                    
                    dtl.Columns.Add("STUDENT TYPE", typeof(string));
                    dtl.Rows[0][colu] = "STUDENT TYPE";
                    colu++;
                }
                

                firstcolcount = dtl.Columns.Count;
                string subjectnos = "";
                int subcout = 0;
                Boolean subflga = false;
                int ff = 0;
                for (int subcount = 0; subcount < examds.Tables[0].Rows.Count; subcount++)
                {
                    string subjectcode = examds.Tables[0].Rows[subcount]["Subject_Code"].ToString();
                    string subjectno = examds.Tables[0].Rows[subcount]["Subject_no"].ToString();
                    string minintmark = examds.Tables[0].Rows[subcount]["min_int_marks"].ToString();
                    string subjecttype = examds.Tables[0].Rows[subcount]["Subject_Type"].ToString();
                    string subject_name = examds.Tables[0].Rows[subcount]["subject_name"].ToString();
                    string subjectacron = examds.Tables[0].Rows[subcount]["acronym"].ToString();
                    string maxintmark = examds.Tables[0].Rows[subcount]["max_int_marks"].ToString();
                    if (hatsubject.Contains(subjectno))
                    {
                        
                        subflga = true;
                        subcout++;
                        if (subjectnos == "")
                        {
                            subjectnos = subjectno;
                        }
                        else
                        {
                            subjectnos = subjectnos + "," + subjectno;
                        }

                        

                        

                        if (chklscolumn.Items[5].Selected == true)
                        {
                            

                            
                            dtl.Columns.Add(subjectacron, typeof(string));
                            if(ff==0)
                                dtl.Rows[0][colu] = "MARKS OBTAINED IN THE SUBJECTS";
                            dtl.Rows[1][colu] = subjectcode;
                            dtl.Rows[2][colu] = subjectacron;
                            
                            colu++;

                            
                            minmark.Add(minintmark);
                            
                            maxmark.Add(maxintmark);
                            subno.Add(subjectno);
                            
                            subcode.Add(subjectcode);
                            subacr.Add(subjectacron);
                            
                            ff++;
                        }
                        

                        dicsubpresent.Add(subjectno, 0);
                        dicsubpass.Add(subjectno, 0);
                        dicsubfail.Add(subjectno, 0);
                        dicsubod.Add(subjectno, 0);
                        dicsubtotal.Add(subjectno, 0);
                        dicsubabsent.Add(subjectno, 0);
                    }
                }

                if (subflga == false)
                {
                    
                    Showgrid.Visible = false;
                    btnmasterprint.Visible = false;
                    btnDirectPrint.Visible = false;
                    btnExcel.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    txtexcelname.Text = "";
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    return;
                }
                
                if (chklscolumn.Items[6].Selected == true)
                {
                    
                    dtl.Columns.Add("TOTAL", typeof(string));
                    dtl.Rows[0][colu] = "TOTAL";
                    colu++;

                }
                

                
                if (chklscolumn.Items[7].Selected == true)
                {
                    
                    dtl.Columns.Add("PERCENTAGE", typeof(string));
                    dtl.Rows[0][colu] = "PERCENTAGE";
                    colu++;
                }
                

                
                if (chklscolumn.Items[8].Selected == true)
                {
                    
                    dtl.Columns.Add("RANK", typeof(string));
                    dtl.Rows[0][colu] = "RANK";
                    colu++;
                }
                

                
                if (chklscolumn.Items[9].Selected == true)
                {
                    
                    dtl.Columns.Add("NO. OF SUBJECTS FAILED", typeof(string));
                    dtl.Rows[0][colu] = "NO. OF SUBJECTS FAILED";
                    colu++;
                }
                


                if (subjectnos.Trim() != "")
                {
                    int delma = daccess2.update_method_wo_parameter("Delete_Rank_Table", "sp");
                    string getmark = "Select distinct total as markobt,subject_no,roll_no from camarks where subject_no in (" + subjectnos + ") order by subject_no asc";
                    DataSet dscammrk = daccess2.select_method_wo_parameter(getmark, "Text");
                    int srno = 0;
                   
                    for (int rollcount = 0; rollcount < examds.Tables[1].Rows.Count; rollcount++)
                    {
                        string regno = examds.Tables[1].Rows[rollcount]["reg_no"].ToString();
                        string rollno = examds.Tables[1].Rows[rollcount]["roll_no"].ToString();
                        string admissionno = examds.Tables[1].Rows[rollcount]["Roll_Admit"].ToString();
                        string studname = examds.Tables[1].Rows[rollcount]["stud_name"].ToString();
                        string stutype = examds.Tables[1].Rows[rollcount]["stud_type"].ToString();
                        Double totalmark = 0;
                        Double totalmaxmark = 0;
                        int nooffailure = 0;
                        int noofsubject = 0;

                        
                        srno++;
                       

                        dtrow = dtl.NewRow();
                        coln = 0;
                        if (chklscolumn.Items[0].Selected == true)
                        {
                            dtrow[coln] = Convert.ToString(srno);
                            coln++;
                        }

                        if (chklscolumn.Items[1].Selected == true)
                        {
                            dtrow[coln] = rollno;
                            coln++;
                           

                            rollnum.Add(rollno);

                        }

                        if (chklscolumn.Items[2].Selected == true)
                        {
                            dtrow[coln] = regno;
                            coln++;
                        }
                        if (chklscolumn.Items[3].Selected == true)
                        {
                            dtrow[coln] = studname;
                            coln++;
                        }
                        if (chklscolumn.Items[4].Selected == true)
                        {
                            dtrow[coln] = stutype;
                            coln++;
                        }
                        Boolean stuflag = false;
                        for (int c = 0; c < subno.Count; c++)
                        {
                            
                            string gsubno = subno[c].ToString();
                            string gminmark = minmark[c].ToString();
                            string gmaxmark = maxmark[c].ToString();

                            
                            Double subminmark = 0;
                            Double submaxmark = 0;
                            if (gminmark.Trim() != "")
                            {
                                subminmark = Convert.ToDouble(gminmark);
                            }
                            if (gmaxmark.Trim() != "")
                            {
                                submaxmark = Convert.ToDouble(gmaxmark);
                            }

                            if (conveoutof > 0)
                            {
                                //subminmark = subminmark / submaxmark * conveoutof;
                            }
                            dscammrk.Tables[0].DefaultView.RowFilter = "roll_no='" + rollno + "' and subject_no='" + gsubno + "'";
                            DataView dvmark = dscammrk.Tables[0].DefaultView;
                            if (dvmark.Count > 0)
                            {
                                stuflag = true;
                                noofsubject++;
                                studflag = true;
                                Double stucam = 0;
                                string stumark = dvmark[0]["markobt"].ToString();
                                if (stumark.Trim() != "")
                                {
                                    stucam = Convert.ToDouble(stumark);
                                    if (chkRoundoff1.Checked)
                                    {
                                        if (conveoutof > 0)
                                        {
                                            stucam = stucam / submaxmark * conveoutof;
                                            stucam = Math.Round(stucam, 0, MidpointRounding.AwayFromZero);
                                        }
                                        totalmark = totalmark + Convert.ToDouble(stucam);
                                    }
                                    else
                                    {
                                        if (conveoutof > 0)
                                        {
                                            stucam = stucam / submaxmark * conveoutof;
                                            stucam = Math.Round(stucam, 1, MidpointRounding.AwayFromZero);
                                        }
                                        totalmark = totalmark + Convert.ToDouble(stucam);
                                    }
                                }
                                if (conveoutof > 0)
                                {
                                    submaxmark = submaxmark / submaxmark * conveoutof;
                                }
                                totalmaxmark = totalmaxmark + submaxmark;
                               

                                if (chklscolumn.Items[5].Selected == true)
                                {
                                    dtrow[coln] = stucam.ToString();
                                    coln++;
                                }




                                if (subminmark >= 0)
                                {
                                    Double precoun = dicsubpresent[gsubno];
                                    precoun++;
                                    dicsubpresent[gsubno] = precoun;


                                    precoun = dicsubtotal[gsubno];
                                    precoun = precoun + stucam;
                                    dicsubtotal[gsubno] = precoun;
                                }
                                else if (subminmark == -1)
                                {
                                    Double precoun = dicsubabsent[gsubno];
                                    precoun++;
                                    dicsubabsent[gsubno] = precoun;
                                }
                                else if (subminmark == -3)
                                {
                                    Double precoun = dicsubod[gsubno];
                                    precoun++;
                                    dicsubod[gsubno] = precoun;
                                }

                                if (stucam < subminmark)
                                {
                                    nooffailure++;
                                    
                                    if (stucam >= 0)
                                    {
                                        Double precoun = dicsubfail[gsubno];
                                        precoun++;
                                        dicsubfail[gsubno] = precoun;
                                    }
                                }
                                else
                                {
                                    
                                    Double precoun = dicsubpass[gsubno];
                                    precoun++;
                                    dicsubpass[gsubno] = precoun;
                                }
                            }
                            else
                            {

                                if (chklscolumn.Items[5].Selected == true)
                                {
                                    dtrow[coln] = string.Empty ;
                                    coln++;
                                }
                            }
                        }
                       
                        if (stuflag == true)
                        {
                            //Double totper = totalmark *100;
                            //totper = Math.Round(totper, 1, MidpointRounding.AwayFromZero);
                            //Double getpercentage = totper / totalmaxmark;
                            Double getpercentage = totalmark / totalmaxmark * 100;
                            //if (conveoutof > 0)
                            //{
                            //    getpercentage=totalmark / totalmaxmark * conveoutof;
                            //}
                            if (getpercentage.ToString().Trim().ToLower() == "nan" || getpercentage.ToString().Trim().ToLower() == "infinity")
                            {
                                getpercentage = 0;
                            }
                            if (!chkRoundoff1.Checked)
                            {
                                getpercentage = Math.Round(getpercentage, 1, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                getpercentage = Math.Round(getpercentage, 2, MidpointRounding.AwayFromZero);
                            }

                            if (nooffailure == 0 && studflag == true)
                            {
                                hat.Clear();
                                hat.Add("RollNumber", rollno.ToString());
                                hat.Add("criteria_no", "");
                                hat.Add("Total", totalmark.ToString());
                                hat.Add("avg", getpercentage.ToString());
                                hat.Add("rank", "");
                                int o = daccess2.insert_method("INSERT_RANK", hat, "sp");
                            }
                            

                            if (chklscolumn.Items[6].Selected == true)
                            {
                                dtrow[coln] = totalmark.ToString();
                                coln++;
                            }
                            if (chklscolumn.Items[7].Selected == true)
                            {
                                dtrow[coln] = getpercentage.ToString();
                                coln++;
                            }
                            if (chklscolumn.Items[9].Selected == true)
                            {
                                dtrow["NO. OF SUBJECTS FAILED"] = nooffailure.ToString();
                                coln++;
                            }
                        }

                        dtl.Rows.Add(dtrow);
                    
                    }
                    int rank_row_count = 0;
                    if (studflag == true)
                    {
                        DataSet ds3 = daccess2.select_method_wo_parameter("SELECT_RANK", "sp");
                        for (int rank = 0; rank < ds3.Tables[0].Rows.Count; rank++)
                        {
                            string rrol = ds3.Tables[0].Rows[rank]["Rollno"].ToString().Trim().ToLower(); ;
                            for (int ro = 0; ro < examds.Tables[1].Rows.Count; ro++)
                            {
                                string getrol = rollnum[ro].ToString(); 
                                if (rrol == getrol)
                                {
                                    rank_row_count++;
                                    
                                    if (chklscolumn.Items[9].Selected == true)
                                    {
                                        
                                        dtl.Rows[ro+3]["RANK"] = rank_row_count.ToString();

                                    }
                                    
                                    
                                }
                            }
                        }
                        if (chklscolumn.Items[10].Selected == true || chklscolumn.Items[11].Selected == true || chklscolumn.Items[12].Selected == true || chklscolumn.Items[13].Selected == true || chklscolumn.Items[14].Selected == true || chklscolumn.Items[15].Selected == true || chklscolumn.Items[16].Selected == true || chklscolumn.Items[17].Selected == true || chklscolumn.Items[18].Selected == true)
                        {
                            
                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "Test Subject Consolidate Details";
                            dtl.Rows.Add(dtrow);
  
                        }

                        int passrow = 0;
                        int failrow = 0;
                        int odrow = 0;
                        int presentrow = 0;
                        int absentrow = 0;
                        int passpersenrow = 0;
                        int classaveragerow = 0;
                        if (chklscolumn.Items[10].Selected == true)
                        {
                            
                            presentrow = dtl.Rows.Count;


                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO. OF STUDENT PRESENT";
                            dtl.Rows.Add(dtrow);
  
                        }
                        if (chklscolumn.Items[11].Selected == true)
                        {

                            absentrow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO. OF STUDENTS ABSENT";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[12].Selected == true)
                        {
                            
                            odrow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO. OF STUDENTS ON OD";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[13].Selected == true)
                        {

                            passrow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO. OF STUDENTS PASSED";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[14].Selected == true)
                        {

                            failrow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO. OF STUDENTS FAILED";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[15].Selected == true)
                        {

                            passpersenrow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "PASS %";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[16].Selected == true)
                        {

                            classaveragerow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "CLASS AVERAGE";
                            dtl.Rows.Add(dtrow);
                        }
                        string strstaffdetails = "select s.subject_code,s.subject_name,s.subject_no,st.Sections,st.staff_code,sm.staff_name from staff_selector st,subject s,staffmaster sm where s.subject_no=st.subject_no and sm.staff_code=st.staff_code and s.subject_no in(" + subjectnos + ") " + secval + "";
                        DataSet dsstaff = daccess2.select_method_wo_parameter(strstaffdetails, "text");

                        int subrown = 0;
                        if (chklscolumn.Items[17].Selected == true)
                        {

                            subrown = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "SUBJECT NAME";
                            dtl.Rows.Add(dtrow);
                        }
                        int staffrow = 0;
                        if (chklscolumn.Items[18].Selected == true)
                        {

                            staffrow = dtl.Rows.Count;


                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "STAFF NAME";
                            dtl.Rows.Add(dtrow);
                        }

                        int staffcoderow = 0;
                        if (chklscolumn.Items[19].Selected == true)
                        {

                            staffcoderow = dtl.Rows.Count;

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "STAFF CODE";
                            dtl.Rows.Add(dtrow);
                        }

                        Double gettotalpercent = 0;

                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("PASS %", typeof(double));
                        dt2.Columns.Add("Subject", typeof(string));

                        int cc = firstcolcount;

                        for (int c = 0; c < subno.Count; c++)
                        {
                            

                            string gsubno = subno[c].ToString();

                            Double presentcount = dicsubpresent[gsubno];
                            Double absentcont = dicsubabsent[gsubno];
                            Double odcount = dicsubod[gsubno];
                            Double passcount = dicsubpass[gsubno];
                            Double failcount = dicsubfail[gsubno];
                            Double totalmark = dicsubtotal[gsubno];

                            Double passpercentage = passcount / presentcount * 100;
                            passpercentage = Math.Round(passpercentage, 2, MidpointRounding.AwayFromZero);

                            if (passpercentage.ToString().Trim().ToLower() == "nan" || passpercentage.ToString().Trim().ToLower() == "infinity")
                            {
                                passpercentage = 0;
                            }

                            gettotalpercent = gettotalpercent + passpercentage;


                            totalmark = totalmark / Convert.ToDouble(examds.Tables[1].Rows.Count);
                            totalmark = Math.Round(totalmark, 2, MidpointRounding.AwayFromZero);


                            DataRow dr2 = dt2.NewRow();
                            dr2[0] = passpercentage.ToString();
                            
                            dr2[1] = subcode[c].ToString() + " - " + subacr[c].ToString();
                            dt2.Rows.Add(dr2);

                            
                           

                            if (presentrow > 0)
                            {
                                
                                if (chklscolumn.Items[10].Selected == true)
                                {
                                    dtl.Rows[presentrow][cc] = presentcount.ToString();
                                }
                            }
                            if (absentrow > 0)
                            {
                                

                                if (chklscolumn.Items[11].Selected == true)
                                {
                                    dtl.Rows[absentrow][cc] = absentcont.ToString();
                                }
                            }
                            if (odrow > 0)
                            {
                                

                                if (chklscolumn.Items[12].Selected == true)
                                {
                                    dtl.Rows[odrow][cc] = odcount.ToString();
                                }
                            }
                            if (passrow > 0)
                            {
                                

                                if (chklscolumn.Items[13].Selected == true)
                                {
                                    dtl.Rows[passrow][cc] = passcount.ToString();
                                }
                            }
                            if (failrow > 0)
                            {
                                

                                if (chklscolumn.Items[14].Selected == true)
                                {
                                    dtl.Rows[failrow][cc] = failcount.ToString();
                                }
                            }
                            if (passpersenrow > 0)
                            {
                                

                                if (chklscolumn.Items[15].Selected == true)
                                {
                                    dtl.Rows[passpersenrow][cc] = passpercentage.ToString();
                                }
                            }
                            if (classaveragerow > 0)
                            {
                                

                                if (chklscolumn.Items[16].Selected == true)
                                {
                                    dtl.Rows[classaveragerow][cc] = totalmark.ToString();
                                }
                            }

                            if (staffrow > 0 || staffcoderow > 0)
                            {
                                string staffname = "";
                                string staffcode = "";
                                string subjename = "";
                                dsstaff.Tables[0].DefaultView.RowFilter = "Subject_no='" + gsubno + "'";
                                DataView dvstaff = dsstaff.Tables[0].DefaultView;
                                for (int st = 0; st < dvstaff.Count; st++)
                                {
                                    subjename = dvstaff[st]["subject_name"].ToString();
                                    if (staffname == "")
                                    {
                                        staffname = dvstaff[st]["staff_name"].ToString();
                                        staffcode = dvstaff[st]["staff_code"].ToString();
                                    }
                                    else
                                    {
                                        staffname = staffname + ", " + dvstaff[st]["staff_name"].ToString();
                                        staffcode = staffcode + " ," + dvstaff[st]["staff_code"].ToString();
                                    }
                                }
                                if (subrown > 0)
                                {
                                    
                                    if (chklscolumn.Items[17].Selected == true)
                                    {
                                        dtl.Rows[subrown][cc] = subjename.ToString();
                                    }
                                }
                                if (staffrow > 0)
                                {
                                    

                                    if (chklscolumn.Items[18].Selected == true)
                                    {
                                        dtl.Rows[staffrow][cc] = staffname.ToString();
                                    }
                                }
                                if (staffcoderow > 0)
                                {
                                    

                                    if (chklscolumn.Items[19].Selected == true)
                                    {
                                        dtl.Rows[staffcoderow][cc] = staffcode.ToString();
                                    }
                                }
                            }
                            cc++;
                        }
                        if (chklscolumn.Items[20].Selected == true || chklscolumn.Items[21].Selected == true || chklscolumn.Items[22].Selected == true)
                        {
                            

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "Test Consolidate Details";
                            dtl.Rows.Add(dtrow);
                        }

                        if (chklscolumn.Items[20].Selected == true)
                        {

                            

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "OVERALL PASS PERCENTAGE";
                            coln = firstcolcount;


                            Double overallpassper = rank_row_count / Convert.ToDouble(examds.Tables[1].Rows.Count) * 100;
                            overallpassper = Math.Round(overallpassper, 2, MidpointRounding.AwayFromZero);

                            dtrow[coln] = overallpassper.ToString();

                            

                            dtl.Rows.Add(dtrow);


                        }

                        if (chklscolumn.Items[21].Selected == true)
                        {
                            


                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "NO OF STUDENTS ALL CLEARED";
                            coln = firstcolcount;
                            dtrow[coln] = rank_row_count.ToString();
                            dtl.Rows.Add(dtrow);

                            
                        }
                        if (chklscolumn.Items[22].Selected == true)
                        {
                            

                            dtrow = dtl.NewRow();
                            coln = 0;
                            dtrow[coln] = "SCRIPT WISE PASS PERCENTAGE";
                            coln = firstcolcount;
                           

                            gettotalpercent = gettotalpercent / Convert.ToDouble(subcout);
                            gettotalpercent = Math.Round(gettotalpercent, 2, MidpointRounding.AwayFromZero);
                            //if (rank_row_count == 0)
                            //{
                            //    gettotalpercent = 0;
                            //}

                            dtrow[coln] = gettotalpercent.ToString();
                            dtl.Rows.Add(dtrow);

                           
                        }

                        attedancechart.DataSource = dt2;
                        attedancechart.DataBind();
                        attedancechart.Visible = true;
                        attedancechart.Enabled = false;
                        attedancechart.ChartAreas[0].AxisX.RoundAxisValues();
                        attedancechart.ChartAreas[0].AxisX.Minimum = 0;
                        attedancechart.ChartAreas[0].AxisX.Interval = 1;
                        attedancechart.Series["Series1"].IsValueShownAsLabel = true;
                        attedancechart.Series[0].ChartType = SeriesChartType.Column;
                        attedancechart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        attedancechart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                        attedancechart.ChartAreas[0].AxisX.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        attedancechart.ChartAreas[0].AxisY.TitleFont = new System.Drawing.Font("Book Antiqua", 12, FontStyle.Bold);
                        attedancechart.ChartAreas[0].AxisX.Title = "Subject";
                        attedancechart.ChartAreas[0].AxisY.Title = "PASS %";
                        attedancechart.Series["Series1"].XValueMember = "Subject";
                        attedancechart.Series["Series1"].YValueMembers = "PASS %";
                        attedancechart.ChartAreas[0].AxisX.TitleForeColor = System.Drawing.Color.Black;
                        attedancechart.ChartAreas[0].AxisY.TitleForeColor = System.Drawing.Color.Black;
                        attedancechart.ChartAreas[0].AxisX.LabelStyle.ForeColor = System.Drawing.Color.Black;
                        attedancechart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Verdana", 8f);
                        attedancechart.ChartAreas[0].AxisY.LabelStyle.ForeColor = System.Drawing.Color.Black;
                    }
                }


                
                Showgrid.Visible = true;
                


                if (dtl.Rows.Count > 0)
                {
                    Showgrid.DataSource = dtl;
                    Showgrid.DataBind();
                    Showgrid.Visible = true;
                    Showgrid.HeaderRow.Visible = false;

                    for (int i = 0; i < Showgrid.Rows.Count; i++)
                    {



                        for (int j = 0; j < Showgrid.HeaderRow.Cells.Count; j++)
                        {

                            if (i == 0 || i == 1 || i==2)
                            {
                                Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                Showgrid.Rows[i].Cells[j].Font.Bold = true;

                                if (i == 0)
                                {

                                    if (j < firstcolcount || j >= firstcolcount + subcode.Count)
                                    {
                                        Showgrid.Rows[i].Cells[j].RowSpan = 3;
                                        for (int a = i; a < 2; a++)
                                            Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                    }
                                    else if (firstcolcount == j)
                                    {
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = subcode.Count;
                                        for (int a = j + 1; a < j + subcode.Count; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;

                                        
                                    }

                                }
                            }
                            else
                            {
                                if (Showgrid.HeaderRow.Cells[j].Text == "REG NO" || Showgrid.HeaderRow.Cells[j].Text == "ROLL NO" || Showgrid.HeaderRow.Cells[j].Text == "STUDENT NAME" || Showgrid.HeaderRow.Cells[j].Text == "STUDENT TYPE")
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;


                                }

                                else
                                {
                                    Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                                    if (Showgrid.HeaderRow.Cells[j].Text == "RANK")
                                    {
                                        if (Showgrid.Rows[i].Cells[j].Text != "&nbsp;")
                                        {
                                            int rr = Convert.ToInt32(Showgrid.Rows[i].Cells[j].Text);
                                            if (rr <= 10)
                                            {
                                                Showgrid.Rows[i].Cells[j].BackColor = Color.LightPink;
                                            }
                                        }
                                    }
                                    if (Showgrid.Rows[i].Cells[j].Text == "Test Subject Consolidate Details" || Showgrid.Rows[i].Cells[j].Text == "Test Consolidate Details")
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;
                                        Showgrid.Rows[i].ForeColor = Color.White;
                                        Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                                        Showgrid.Rows[i].Cells[j].BackColor = Color.White;
                                        Showgrid.Rows[i].Cells[j].ColumnSpan = Showgrid.Rows[i].Cells.Count;
                                        for (int a = 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;


                                    }
                                    else if (Showgrid.Rows[i].Cells[j].Text == "NO. OF STUDENT PRESENT" || Showgrid.Rows[i].Cells[j].Text == "NO. OF STUDENTS ABSENT" || Showgrid.Rows[i].Cells[j].Text == "NO. OF STUDENTS ON OD" || Showgrid.Rows[i].Cells[j].Text == "NO. OF STUDENTS PASSED" || Showgrid.Rows[i].Cells[j].Text == "NO. OF STUDENTS FAILED" || Showgrid.Rows[i].Cells[j].Text == "PASS %" || Showgrid.Rows[i].Cells[j].Text == "CLASS AVERAGE" || Showgrid.Rows[i].Cells[j].Text == "SUBJECT NAME" || Showgrid.Rows[i].Cells[j].Text == "STAFF NAME" || Showgrid.Rows[i].Cells[j].Text == "STAFF CODE" || Showgrid.Rows[i].Cells[j].Text == "OVERALL PASS PERCENTAGE" || Showgrid.Rows[i].Cells[j].Text == "NO OF STUDENTS ALL CLEARED" || Showgrid.Rows[i].Cells[j].Text == "SCRIPT WISE PASS PERCENTAGE")
                                    {
                                        Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Left;

                                        Showgrid.Rows[i].Cells[j].ColumnSpan = firstcolcount;
                                        for (int a = 1; a < firstcolcount; a++)
                                            Showgrid.Rows[i].Cells[a].Visible = false;

                                        if (Showgrid.Rows[i].Cells[j].Text == "OVERALL PASS PERCENTAGE" || Showgrid.Rows[i].Cells[j].Text == "NO OF STUDENTS ALL CLEARED" || Showgrid.Rows[i].Cells[j].Text == "SCRIPT WISE PASS PERCENTAGE")
                                        {


                                            Showgrid.Rows[i].Cells[firstcolcount].HorizontalAlign = HorizontalAlign.Left;

                                            Showgrid.Rows[i].Cells[firstcolcount].ColumnSpan = Showgrid.Rows[i].Cells.Count;
                                            for (int a = firstcolcount + 1; a < Showgrid.Rows[i].Cells.Count; a++)
                                                Showgrid.Rows[i].Cells[a].Visible = false;
                                        }
                                    }





                                }


                            }
                        }

                    }
                }
              
            }
            if (studflag == false)
            {
                
                Showgrid.Visible = false;
                btnmasterprint.Visible = false;
                btnDirectPrint.Visible = false;
                btnExcel.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Text = "";
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
            }
            else
            {
                
                Showgrid.Visible = true;
                btnmasterprint.Visible = true;
                btnDirectPrint.Visible = true;
                btnExcel.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
            }
            
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    //protected void btngenerate_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int checkcnt = 0;

    //        Hashtable subject_has = new Hashtable();
    //        sprdviewrcrd.Sheets[0].ColumnHeader.RowCount = 2;
    //        int sub_type_temp_var = 0;
    //        bool flag_temp = false;
    //        Hashtable hashselectsubno = new Hashtable();
    //        ArrayList ard = new ArrayList();
    //        sprdviewrcrd.Visible = false;
    //        btnmasterprint.Visible = false;
    //        pageset_pnl.Visible = false;
    //        lblnorec.Visible = false;
    //        sprdviewrcrd.Sheets[0].SheetName = " ";
    //        sprdviewrcrd.Sheets[0].ColumnCount = 5;

    //        FarPoint.Web.Spread.TextCellType txttype = new FarPoint.Web.Spread.TextCellType();
    //        sprdviewrcrd.Sheets[0].Columns[2].CellType = txttype;
    //        filteration();
    //        string filterwithsection = "batch_year ='" + ddlbatch.SelectedValue.ToString() + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester >= '" + ddlsem.SelectedValue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and  exam_flag <> 'DEBAR' and delflag=0  " + strregorder + " ";
    //        string filterwithoutsection = "batch_year ='" + ddlbatch.SelectedValue.ToString() + "' and  degree_code='" + ddlbranch.SelectedValue.ToString() + "' and current_semester >= '" + ddlsem.SelectedValue.ToString() + "' and sections='" + ddlsec.SelectedValue.ToString() + "' and RollNo_Flag<>0 and cc=0 and delflag=0 and  exam_flag <> 'DEBAR' and delflag=0  " + strregorder + " ";
    //        con.Close();
    //        con.Open();
    //        SqlCommand examcmd = new SqlCommand("internalassessgetsubject", con);
    //        examcmd.CommandType = CommandType.StoredProcedure;
    //        examcmd.Parameters.AddWithValue("@batchyear", Convert.ToInt32(ddlbatch.SelectedValue.ToString()));
    //        examcmd.Parameters.AddWithValue("@degreecode", Convert.ToInt32(ddlbranch.SelectedValue.ToString()));
    //        examcmd.Parameters.AddWithValue("@cur_sem", Convert.ToInt32(ddlsem.SelectedValue.ToString()));
    //        examcmd.Parameters.AddWithValue("@sections", ddlsec.SelectedValue.ToString());
    //        examcmd.Parameters.AddWithValue("@filterwithsection", filterwithsection.ToString());
    //        examcmd.Parameters.AddWithValue("@filterwithoutsection", filterwithoutsection.ToString());
    //        lblrptname.Visible = false;
    //        txtexcelname.Visible = false;
    //        lblnorec.Visible = false;
    //        btnExcel.Visible = false;
    //        SqlDataAdapter examda = new SqlDataAdapter(examcmd);
    //        examda.Fill(examds);
    //        sprdviewrcrd.Sheets[0].RowCount = 0;
    //        double total = 0;
    //        if (examds.Tables[1].Rows.Count > 0)
    //        {
    //            int sno = 0;

    //            int flag = 0;
    //            for (int rollcount = 0; rollcount < examds.Tables[1].Rows.Count; rollcount++)
    //            {
    //                string getsubjectno = "";

    //                sno++;
    //                int markbindcolcount = 4;
    //                string regno = examds.Tables[1].Rows[rollcount]["reg_no"].ToString();
    //                string rollno = examds.Tables[1].Rows[rollcount]["roll_no"].ToString();
    //                string admissionno = examds.Tables[1].Rows[rollcount]["Roll_Admit"].ToString();
    //                string studname = examds.Tables[1].Rows[rollcount]["stud_name"].ToString();
    //                sprdviewrcrd.Sheets[0].RowCount = sprdviewrcrd.Sheets[0].RowCount + 1;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 1].Text = admissionno;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 3].Text = regno;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 3].CellType = txttype;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Text = studname;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].Text = rollno;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].CellType = txttype;

    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Border.BorderColorTop = Color.Black;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 1].Border.BorderColorTop = Color.Black;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].Border.BorderColorTop = Color.Black;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 3].Border.BorderColorTop = Color.Black;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Border.BorderColorTop = Color.Black;
    //                if (examds.Tables[0].Rows.Count > 0)
    //                {
    //                    lblrptname.Visible = true;
    //                    txtexcelname.Visible = true;
    //                    btnExcel.Visible = true;
    //                    int first_true_col = 0;
    //                    int subjectcount = 0;
    //                    int flag1 = 0;
    //                    int startcol = 5;
    //                    int subjectforspan = 0;
    //                    string bindsubjecttype = "";
    //                    sprdviewrcrd.Visible = true;
    //                    btnmasterprint.Visible = true;
    //                    pageset_pnl.Visible = true;

    //                    for (int subcount = 0; subcount < examds.Tables[0].Rows.Count; subcount++)
    //                    {
    //                        subjectcount++;
    //                        markbindcolcount++;
    //                        subjectforspan++;
    //                        string subjectcode = examds.Tables[0].Rows[subcount]["Subject_Code"].ToString();
    //                        string subjectno = examds.Tables[0].Rows[subcount]["Subject_no"].ToString();
    //                        maxintmark = examds.Tables[0].Rows[subcount]["max_int_marks"].ToString();
    //                        string subjecttype = examds.Tables[0].Rows[subcount]["Subject_Type"].ToString();
    //                        string subject_name = examds.Tables[0].Rows[subcount]["subject_name"].ToString();//----------29/6/12 PRABHA
    //                        if (ard.Contains(subjectno) == false)
    //                        {
    //                            ard.Add(subjectno);
    //                        }

    //                        //---------------------------------------29/6/12  PRABHA
    //                        if (getsubjectno == "")
    //                        {
    //                            getsubjectno = subjectno;

    //                        }
    //                        else
    //                        {
    //                            getsubjectno = getsubjectno + "," + subjectno;
    //                        }
    //                        //  string get_subjtype_no = "";
    //                        for (int item_cnt = 0; item_cnt < chkbxlistsubj.Items.Count; item_cnt++)
    //                        {
    //                            if (chkbxlistsubj.Items[item_cnt].Selected == true)
    //                            {
    //                                if (!subject_has.ContainsKey(chkbxlistsubj.Items[item_cnt].Value))
    //                                {
    //                                    subject_has.Add(chkbxlistsubj.Items[item_cnt].Value, chkbxlistsubj.Items[item_cnt].Value);
    //                                }

    //                                //if (getsubjectno == "")
    //                                //{
    //                                //    getsubjectno = "(" + chkbxlistsubjtype.Items[item_cnt].Value;
    //                                //}
    //                                //else
    //                                //{
    //                                //    getsubjectno = getsubjectno + "," + chkbxlistsubjtype.Items[item_cnt].Value;
    //                                //}
    //                            }
    //                        }
    //                        //if (getsubjectno != "")
    //                        // {
    //                        //     getsubjectno = getsubjectno + ")";
    //                        // }
    //                        //--------------------------------------

    //                        if (flag == 0)
    //                        {
    //                            sprdviewrcrd.Sheets[0].ColumnCount = sprdviewrcrd.Sheets[0].ColumnCount + 1;
    //                            sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Width = 200;
    //                            sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Locked = true;
    //                            hashselectsubno.Add(subjectno, sprdviewrcrd.Sheets[0].ColumnCount - 1);
    //                            // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Tag = maxintmark;
    //                            if (subjecttype != bindsubjecttype)
    //                            {
    //                                if (flag1 == 1)
    //                                {
    //                                    sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, subjectforspan);
    //                                    sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Text = bindsubjecttype;
    //                                    sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Font.Size = FontUnit.Medium;
    //                                    sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Font.Bold = true;
    //                                    sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Border.BorderColorBottom = Color.Black;
    //                                    //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, startcol].Tag = "1";
    //                                    //  sub_type_temp_var = 0;                                 
    //                                }
    //                                flag1 = 1;
    //                                startcol = sprdviewrcrd.Sheets[0].ColumnCount - 1;
    //                                subjectforspan = 0;
    //                            }
    //                            //sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(7, 5, 1, subjectcount);
    //                            //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[7, 5].Text = "Maximum Mark";

    //                            //-------------------------29/6/12 PRABHA
    //                            if (chkbxlisisub_name_code.Items[0].Selected == true & chkbxlisisub_name_code.Items[1].Selected == true)
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = subject_name + " - " + subjectcode;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            else if (chkbxlisisub_name_code.Items[0].Selected == true & chkbxlisisub_name_code.Items[1].Selected == false)
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = subject_name;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            else if (chkbxlisisub_name_code.Items[0].Selected == false & chkbxlisisub_name_code.Items[1].Selected == true)
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = subjectcode;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            else
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = subject_name + " - " + subjectcode;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Tag = subjectno;
    //                            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
    //                            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorTop = Color.Black;
    //                            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.Black;
    //                            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorRight = Color.Black;

    //                            if (subject_has.ContainsKey(subjectno))
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Visible = true;
    //                                if (flag_temp == false)
    //                                {
    //                                    sub_type_temp_var = sprdviewrcrd.Sheets[0].ColumnCount - 1;
    //                                    //sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(7, sprdviewrcrd.Sheets[0].ColumnCount - 1, 1, subjectcount - (sprdviewrcrd.Sheets[0].ColumnCount - 1 - 5));
    //                                    //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[7, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = "Maximum Mark";
    //                                    flag_temp = true;
    //                                }

    //                                //sub_type_temp_var++;
    //                                if (startcol == 0)
    //                                {
    //                                    startcol = sprdviewrcrd.Sheets[0].ColumnCount;
    //                                }
    //                            }
    //                            else
    //                            {
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Visible = false;
    //                                sub_type_temp_var++;
    //                            }


    //                            //---------------------------------------------------------------------------

    //                            if (txtmrkoutof.Text == "")
    //                            {
    //                                //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = maxintmark;
    //                                //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorBottom = Color.Black;
    //                                //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Border.BorderColorTop = Color.Black;
    //                                // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                //sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            else
    //                            {
    //                                // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = txtmrkoutof.Text;
    //                                // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                                // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                            }
    //                            if (sprdviewrcrd.Sheets[0].ColumnHeader.Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Visible == true)
    //                            {
    //                                bindsubjecttype = subjecttype;
    //                            }
    //                            if (subjectcount == examds.Tables[0].Rows.Count)
    //                            {
    //                                subjectforspan++;
    //                                sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, startcol, 1, subjectforspan);
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Text = subjecttype;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Font.Size = FontUnit.Medium;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Font.Bold = true;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Border.BorderColorBottom = Color.Black;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Border.BorderColorTop = Color.Black;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Border.BorderColorLeft = Color.Black;
    //                                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, startcol].Border.BorderColorRight = Color.Black;
    //                            }
    //                        }
    //                        if (rollcount == 0)
    //                        {
    //                            //sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(2, sub_type_temp_var, 1, subjectcount - (sub_type_temp_var - 5));
    //                            // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, sub_type_temp_var].Text = "Maximum Mark";
    //                            // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, sub_type_temp_var].Border.BorderColorRight = Color.Black;
    //                            // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, sub_type_temp_var].Font.Size = FontUnit.Medium;
    //                            // sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, sub_type_temp_var].Font.Bold = true;
    //                        }

    //                    }
    //                    if (flag == 0)
    //                    {
    //                        sprdviewrcrd.Sheets[0].ColumnCount = sprdviewrcrd.Sheets[0].ColumnCount + 2;
    //                        sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 2].Width = 80;
    //                        sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Width = 100;
    //                        sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, sprdviewrcrd.Sheets[0].ColumnCount - 2, 2, 1);
    //                        sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, sprdviewrcrd.Sheets[0].ColumnCount - 1, 2, 1);
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 2].Text = "Total";
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = "Signature of the Candidate";
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 1].Font.Bold = true;
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
    //                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 2].Font.Bold = true;
    //                    }
    //                    flag = 1;
    //                }

    //                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[sprdviewrcrd.Sheets[0].ColumnCount - 2].Visible = false; //08.06.12
    //                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Visible = false; //08.06.12
    //                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[1].Visible = false; //08.06.12

    //                string getmark = "Select distinct total as markobt,subject_no from camarks where subject_no in (" + getsubjectno + ") and roll_no='" + rollno + "' order by subject_no asc";
    //                SqlDataAdapter dagetmark = new SqlDataAdapter(getmark, con1);
    //                DataSet dsgetmark = new DataSet();
    //                con.Close();
    //                con.Open();
    //                dagetmark.Fill(dsgetmark);
    //                if (dsgetmark.Tables[0].Rows.Count > 0)
    //                {

    //                    total = 0;
    //                    int columcount = 5;
    //                    for (int obtmark = 0; obtmark < dsgetmark.Tables[0].Rows.Count; obtmark++)
    //                    {
    //                        string obtainedmark = dsgetmark.Tables[0].Rows[obtmark]["markobt"].ToString();
    //                        if (dsgetmark.Tables[0].Rows[obtmark]["markobt"].ToString() != "" && dsgetmark.Tables[0].Rows[obtmark]["markobt"].ToString() != null)// added by sridhar 28 aug 2014 ---start
    //                        {
    //                            checkcnt++;
    //                            string obtainedsubno = dsgetmark.Tables[0].Rows[obtmark]["subject_no"].ToString();
    //                            string getsubjectnoheader = Convert.ToString(sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, columcount].Tag);
    //                            foreach (DictionaryEntry parameter1 in hashselectsubno)
    //                            {
    //                                getsubjectnoheader = Convert.ToString(parameter1.Key);
    //                                string getexmcde = Convert.ToString(parameter1.Value);
    //                                if (obtainedsubno == getsubjectnoheader)
    //                                {
    //                                    if (txtmrkoutof.Text == "")
    //                                    {
    //                                        if (obtainedmark != "")
    //                                        {
    //                                            total = total + Convert.ToDouble(obtainedmark);
    //                                        }
    //                                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, Convert.ToInt32(getexmcde)].Text = obtainedmark;
    //                                    }
    //                                    else
    //                                    {
    //                                        // maxintmark = Convert.ToString(sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, Convert.ToInt32(getexmcde)].Tag);
    //                                        string outofmark = txtmrkoutof.Text;
    //                                        decimal showmark = 0;
    //                                        if ((obtainedmark != "") && (obtainedmark != "0"))
    //                                        {
    //                                            showmark = Convert.ToDecimal(Convert.ToDecimal(obtainedmark) / Convert.ToDecimal(maxintmark));
    //                                        }

    //                                        showmark = Math.Round(showmark, 2);
    //                                        double showmark1 = Convert.ToDouble(showmark);
    //                                        string showmark2 = Convert.ToString(Convert.ToDouble(showmark1) * Convert.ToInt32(outofmark));
    //                                        total = total + Convert.ToDouble(showmark2);
    //                                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, Convert.ToInt32(getexmcde)].Text = showmark2;
    //                                    }
    //                                }
    //                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, Convert.ToInt32(getexmcde)].HorizontalAlign = HorizontalAlign.Center;
    //                            }
    //                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, columcount].HorizontalAlign = HorizontalAlign.Center;
    //                            if (columcount < (sprdviewrcrd.Sheets[0].ColumnCount - 1))//Added By Srinath 23/5/2014
    //                                columcount++;
    //                        }
    //                    }

    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, sprdviewrcrd.Sheets[0].ColumnCount - 2].Text = Convert.ToString(total);
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, sprdviewrcrd.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;
    //                }
    //                if (checkcnt == 0)
    //                {
    //                    sprdviewrcrd.Visible = false;
    //                    btnmasterprint.Visible = false;
    //                    pageset_pnl.Visible = false;
    //                    lblnorec.Visible = true;
    //                    lblrptname.Visible = false;
    //                    txtexcelname.Visible = false;
    //                    btnExcel.Visible = false;

    //                }
    //            }
    //            //Added By Srinath 23/5/2014
    //            sprdviewrcrd.Sheets[0].RowCount++;
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = "Mean Value";
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //            sprdviewrcrd.Sheets[0].SpanModel.Add(sprdviewrcrd.Sheets[0].RowCount - 1, 0, 1, 5);



    //            for (int i = 5; i < sprdviewrcrd.Sheets[0].ColumnCount; i++)
    //            {
    //                Double marks = 0;
    //                int stucount = 0;
    //                for (int j = 0; j < sprdviewrcrd.Sheets[0].RowCount - 1; j++)
    //                {
    //                    string val = sprdviewrcrd.Sheets[0].Cells[j, i].Text.ToString();
    //                    if (val != null && val.Trim() != "")
    //                    {
    //                        marks = marks + Convert.ToDouble(val);
    //                        stucount++;
    //                    }
    //                }
    //                if (marks > 0)
    //                {
    //                    Double mean = marks / stucount;
    //                    mean = Math.Round(mean, 2, MidpointRounding.AwayFromZero);
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].Text = mean.ToString();
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].Font.Bold = true;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
    //                }

    //            }

    //            sprdviewrcrd.Sheets[0].RowCount++;

    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = "Staff Signature";
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
    //            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //            sprdviewrcrd.Sheets[0].SpanModel.Add(sprdviewrcrd.Sheets[0].RowCount - 1, 0, 1, 5);


    //            for (int i = 5; i < sprdviewrcrd.Sheets[0].ColumnCount; i++)
    //            {
    //                Double marks = 1;
    //                int stucount = 0;
    //                for (int j = 0; j < sprdviewrcrd.Sheets[0].RowCount - 1; j++)
    //                {
    //                    string val = sprdviewrcrd.Sheets[0].Cells[j, i].Text.ToString();
    //                    if (val != null && val.Trim() != "")
    //                    {
    //                        marks = marks + Convert.ToDouble(val);
    //                        stucount++;
    //                    }
    //                }
    //                if (marks > 0)
    //                {
    //                    Double mean = marks / stucount;
    //                    mean = Math.Round(mean, 2, MidpointRounding.AwayFromZero);
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].Text = mean.ToString();
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].Font.Bold = true;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].ForeColor = Color.White;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, i].HorizontalAlign = HorizontalAlign.Center;
    //                }

    //            }

    //            string section11 = "";
    //            if (ddlsec.Enabled == true)
    //            {
    //                if (ddlsec.SelectedItem.Text == "All")
    //                {
    //                    section11 = "";
    //                }
    //                else
    //                {
    //                    section11 = "and st.Sections='" + ddlsec.SelectedItem.Text + "'";
    //                }
    //            }
    //            else
    //            {
    //                section11 = "";
    //            }
    //            DataSet ds11 = new DataSet();
    //            DataView dv = new DataView();
    //            ArrayList arsec = new ArrayList();
    //            //string bindquery = "select distinct s.subject_name,s.subject_code,s.subject_no,st.staff_code,sm.staff_name,st.Sections,r.degree_code,r.Batch_Year,r.Current_Semester from subject s,sub_sem ss,syllabus_master sy,Registration r,staff_selector st,staffmaster sm where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and st.Sections=r.Sections and sm.staff_code=st.staff_code and s.subject_no=st.subject_no and r.Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and r.degree_code=" + ddlbranch.SelectedItem.Value + " and r.Current_Semester=" + ddlsem.SelectedItem.Value + " " + section11 + " order by s.subject_no,st.Sections";
    //            string bindquery = "select distinct s.subject_name,s.subject_code,s.subject_no,st.staff_code,sm.staff_name,st.Sections,sy.degree_code,sy.Batch_Year,sy.Semester from subject s,sub_sem ss,syllabus_master sy,staff_selector st,staffmaster sm where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.subType_no=s.subType_no and sm.staff_code=st.staff_code and s.subject_no=st.subject_no and sy.Batch_Year=" + ddlbatch.SelectedValue.ToString() + " and sy.degree_code=" + ddlbranch.SelectedItem.Value + " and sy.Semester=" + ddlsem.SelectedItem.Value + " " + section11 + " order by s.subject_no,st.Sections";
    //            ds11.Clear();
    //            ds11 = daccess2.select_method_wo_parameter(bindquery, "Text");
    //            if (ds11.Tables[0].Rows.Count > 0)
    //            {
    //                //added by annyutha 06dec 2014//
    //                sprdviewrcrd.Sheets[0].RowCount++;
    //                //-----------------------------------Start--------------------------------Modify By M.SakthiPriya 10-12-2014
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = "S.No";
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
    //                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Font.Bold = true;
    //                sprdviewrcrd.Sheets[0].SpanModel.Add(sprdviewrcrd.Sheets[0].RowCount - 1, 0, 1, 4);
    //                if (sprdviewrcrd.Sheets[0].ColumnCount > 4)
    //                {
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Text = "Subject Code";
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Font.Bold = true;
    //                }
    //                if (sprdviewrcrd.Sheets[0].ColumnCount > 5)
    //                {
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].Text = "Subject Name";
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].Font.Bold = true;
    //                }
    //                if (sprdviewrcrd.Sheets[0].ColumnCount > 6)
    //                {
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].Text = "Staff Name";
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
    //                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].Font.Bold = true;
    //                }
    //                if (sprdviewrcrd.Sheets[0].ColumnCount > 7)
    //                {
    //                    if (chckstaff.Checked == true)
    //                    {
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].Text = "Staff Code";
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].Font.Bold = true;
    //                    }

    //                }
    //                if (sprdviewrcrd.Sheets[0].ColumnCount > 8)
    //                {
    //                    if (chcksec.Checked == true)
    //                    {
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].Text = "Section";
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
    //                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].Font.Bold = true;
    //                    }
    //                }
    //                //---------------------------------End---------------------------------------Modify By M.SakthiPriya 10-12-2014
    //                //***end*****//
    //                for (int sec = 0; sec < ds11.Tables[0].Rows.Count; sec++)
    //                {
    //                    string addsec = Convert.ToString(ds11.Tables[0].Rows[sec]["Sections"]);
    //                    if (addsec.Trim() != "" && addsec.ToString() != null)
    //                    {
    //                        if (arsec.Contains(addsec.ToString()) == false)
    //                        {
    //                            arsec.Add(ds11.Tables[0].Rows[sec]["Sections"].ToString());
    //                        }
    //                    }

    //                }

    //                if (ard.Count > 0)
    //                {
    //                    int ij = 0;
    //                    for (int ar = 0; ar < ard.Count; ar++)
    //                    {
    //                        if (sprdviewrcrd.Sheets[0].RowCount > 0)
    //                        {

    //                            string mainvalue = "";
    //                            if (arsec.Count > 0)
    //                            {

    //                                string concatsubject = "";
    //                                for (int sec = 0; sec < arsec.Count; sec++)
    //                                {

    //                                    ds11.Tables[0].DefaultView.RowFilter = "subject_no=" + ard[ar].ToString() + " and Sections='" + arsec[sec].ToString() + "'";
    //                                    dv = ds11.Tables[0].DefaultView;

    //                                    if (dv.Count > 0)
    //                                    {
    //                                        string section1111 = "";
    //                                        for (int kk = 0; kk < dv.Count; kk++)
    //                                        {
    //                                            ij++;
    //                                            string subjectname = dv[kk]["subject_name"].ToString();
    //                                            string subjectcode = dv[kk]["subject_code"].ToString();
    //                                            string staffname = dv[kk]["staff_name"].ToString();
    //                                            string staffcode = dv[kk]["staff_code"].ToString();
    //                                            section1111 = dv[kk]["Sections"].ToString();
    //                                            //added by annyutha //
    //                                            sprdviewrcrd.Sheets[0].RowCount++;
    //                                            //---------------------------------Start---------------------------------------Modify By M.SakthiPriya 10-12-2014
    //                                            sprdviewrcrd.Sheets[0].SpanModel.Add(sprdviewrcrd.Sheets[0].RowCount - 1, 0, 1, 4);
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = ij.ToString();
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                            if (sprdviewrcrd.Sheets[0].ColumnCount > 4)
    //                                            {
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Text = subjectcode.ToString();
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
    //                                            }
    //                                            if (sprdviewrcrd.Sheets[0].ColumnCount > 5)
    //                                            {
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].Text = subjectname.ToString();
    //                                            }
    //                                            if (sprdviewrcrd.Sheets[0].ColumnCount > 6)
    //                                            {
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].Text = staffname.ToString();
    //                                            }
    //                                            if (sprdviewrcrd.Sheets[0].ColumnCount > 7)
    //                                            {
    //                                                if (chckstaff.Checked == true)
    //                                                {
    //                                                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].Text = staffcode.ToString();

    //                                                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
    //                                                }
    //                                            }
    //                                            if (sprdviewrcrd.Sheets[0].ColumnCount > 8)
    //                                            {
    //                                                if (chcksec.Checked == true)
    //                                                {
    //                                                    sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].Text = section1111.ToString();
    //                                                }
    //                                            }
    //                                            //---------------------------------End---------------------------------------Modify By M.SakthiPriya 10-12-2014
    //                                            //end//
    //                                            //if (mainvalue == "")
    //                                            //{
    //                                            //    if (concatsubject == "")
    //                                            //    {
    //                                            //        concatsubject = subjectcode + " - " + subjectname + " - " + staffname + "[" + staffcode + "]";
    //                                            //    }
    //                                            //    else
    //                                            //    {
    //                                            //        concatsubject = concatsubject + ":" + staffname + "[" + staffcode + "]";
    //                                            //    }
    //                                            //}
    //                                            //else
    //                                            //{
    //                                            //    if (mainvalue == concatsubject)
    //                                            //    {
    //                                            //        concatsubject = concatsubject + "," + staffname + "[" + staffcode + "]";
    //                                            //    }
    //                                            //    else
    //                                            //    {
    //                                            //        concatsubject = concatsubject + ":" + staffname + "[" + staffcode + "]";
    //                                            //    }
    //                                            //}

    //                                        }
    //                                        //if (mainvalue == "")
    //                                        //{
    //                                        //    mainvalue = concatsubject + "-" + section1111;
    //                                        //    concatsubject = mainvalue;
    //                                        //}
    //                                        //else
    //                                        //{
    //                                        //    mainvalue = concatsubject + "-" + section1111;
    //                                        //    concatsubject = mainvalue;
    //                                        //}


    //                                    }
    //                                }


    //                            }
    //                            else
    //                            {
    //                                ds11.Tables[0].DefaultView.RowFilter = "subject_no=" + ard[ar].ToString() + "";
    //                                dv = ds11.Tables[0].DefaultView;
    //                                if (dv.Count > 0)
    //                                {
    //                                    string concatsubject = "";

    //                                    for (int kk = 0; kk < dv.Count; kk++)
    //                                    {
    //                                        ij++;
    //                                        string section1111 = "-";
    //                                        string subjectname = dv[kk]["subject_name"].ToString();
    //                                        string subjectcode = dv[kk]["subject_code"].ToString();
    //                                        string staffname = dv[kk]["staff_name"].ToString();
    //                                        string staffcode = dv[kk]["staff_code"].ToString();

    //                                        sprdviewrcrd.Sheets[0].RowCount++;
    //                                        //---------------------------------Start---------------------------------------Modify By M.SakthiPriya 10-12-2014
    //                                        sprdviewrcrd.Sheets[0].SpanModel.Add(sprdviewrcrd.Sheets[0].RowCount - 1, 0, 1, 4);
    //                                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = ij.ToString();
    //                                        sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                                        if (sprdviewrcrd.Sheets[0].ColumnCount > 4)
    //                                        {
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Text = subjectcode.ToString();
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
    //                                        }
    //                                        if (sprdviewrcrd.Sheets[0].ColumnCount > 5)
    //                                        {
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 5].Text = subjectname.ToString();
    //                                        }
    //                                        if (sprdviewrcrd.Sheets[0].ColumnCount > 6)
    //                                        {
    //                                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 6].Text = staffname.ToString();
    //                                        }
    //                                        if (sprdviewrcrd.Sheets[0].ColumnCount > 7)
    //                                        {
    //                                            if (chckstaff.Checked == true)
    //                                            {
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 7].Text = staffcode.ToString();
    //                                            }
    //                                        }
    //                                        if (sprdviewrcrd.Sheets[0].ColumnCount > 8)
    //                                        {
    //                                            if (chcksec.Checked == true)
    //                                            {
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].Text = section1111.ToString();
    //                                                sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
    //                                            }
    //                                        }
    //                                        //---------------------------------End---------------------------------------Modify By M.SakthiPriya 10-12-2014
    //                                        //   sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 11].Text = section1111.ToString();

    //                                    }


    //                                }
    //                            }
    //                        }

    //                    }
    //                }
    //            }


    //        }
    //        else
    //        {
    //            sprdviewrcrd.Visible = false;
    //            btnmasterprint.Visible = false;
    //            lblnorec.Visible = true;
    //            pageset_pnl.Visible = false;
    //            lblrptname.Visible = false;
    //            txtexcelname.Visible = false;
    //            btnExcel.Visible = false;
    //        }
    //        int subcount1 = examds.Tables[0].Rows.Count;
    //        int rowcount = sprdviewrcrd.Sheets[0].RowCount;
    //        sprdviewrcrd.Height = (rowcount * 20) + 80;
    //        sprdviewrcrd.Sheets[0].PageSize = (rowcount * 20) + 80;
    //        sprdviewrcrd.Width = (subcount1 * 80) + 450; //08.06.12


    //        Session["sheetcorner"] = sprdviewrcrd.Sheets[0].ColumnHeader.RowCount;

    //        //lblrptname.Visible = false;
    //        //txtexcelname.Visible = false;
    //        //btnExcel.Visible = false;           
    //        if (Convert.ToInt32(sprdviewrcrd.Sheets[0].RowCount) != 0)
    //        {

    //            Double totalRows = 0;
    //            totalRows = Convert.ToInt32((sprdviewrcrd.Sheets[0].RowCount));

    //            DropDownListpage.Items.Clear();
    //            if (totalRows >= 10)
    //            {
    //                sprdviewrcrd.Sheets[0].PageSize = 10;
    //                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
    //                {
    //                    DropDownListpage.Items.Add((k + 10).ToString());
    //                }
    //                DropDownListpage.Items.Add("Others");
    //                sprdviewrcrd.Height = 410;
    //                sprdviewrcrd.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
    //                sprdviewrcrd.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

    //            }
    //            else if (totalRows == 0)
    //            {
    //                DropDownListpage.Items.Add("0");
    //                sprdviewrcrd.Height = 200;
    //            }
    //            else
    //            {
    //                sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt32(totalRows);
    //                DropDownListpage.Items.Add(sprdviewrcrd.Sheets[0].PageSize.ToString());
    //                sprdviewrcrd.Height = 30 + (38 * Convert.ToInt32(totalRows));
    //            }
    //            if (Convert.ToInt32(totalRows) > 10)
    //            {
    //                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
    //                sprdviewrcrd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
    //                //CalculateTotalPages();
    //            }
    //            Session["totalPages"] = (int)Math.Ceiling(totalRows / sprdviewrcrd.Sheets[0].PageSize);
    //            // Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];


    //        }

    //    }
    //    catch
    //    {
    //    }

    //}
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsubject_type();
        bindsubject();
    }
    protected void btnPrint_Click(object sender, EventArgs e)
    {
        ////PrintMaster = true;
        //string selected_criteria = "";
        //string selected_subj = "";
        //string selected_subcode_name = "";
        ////===========================added on 05.07.12
        //if (chkbxlistsubj.Items.Count > 0)
        //{
        //    for (int criteria = 0; criteria < chkbxlistsubj.Items.Count; criteria++)
        //    {
        //        if (chkbxlistsubj.Items[criteria].Selected == true)
        //        {
        //            if (selected_subj == "")
        //            {
        //                selected_subj = chkbxlistsubj.Items[criteria].Value;
        //            }
        //            else
        //            {
        //                selected_subj = selected_subj + "-" + chkbxlistsubj.Items[criteria].Value;
        //            }
        //        }
        //    }
        //}
        ////=====================added 02.07.12  //subject type
        //if (chkbxlistsubjtype.Items.Count > 0)
        //{
        //    for (int criteria = 0; criteria < chkbxlistsubjtype.Items.Count; criteria++)
        //    {
        //        if (chkbxlistsubjtype.Items[criteria].Selected == true)
        //        {
        //            if (selected_criteria == "")
        //            {
        //                selected_criteria = chkbxlistsubjtype.Items[criteria].Value;
        //            }
        //            else
        //            {
        //                selected_criteria = selected_criteria + "-" + chkbxlistsubjtype.Items[criteria].Value;
        //            }
        //        }
        //    }
        //}
        ////===================== added on 05.07.12
        //if (chkbxlisisub_name_code.Items.Count > 0)
        //{
        //    for (int criteria = 0; criteria < chkbxlisisub_name_code.Items.Count; criteria++)
        //    {
        //        if (chkbxlisisub_name_code.Items[criteria].Selected == true)
        //        {
        //            if (selected_subcode_name == "")
        //            {
        //                selected_subcode_name = chkbxlisisub_name_code.Items[criteria].Value;
        //            }
        //            else
        //            {
        //                selected_subcode_name = selected_subcode_name + "-" + chkbxlisisub_name_code.Items[criteria].Value;
        //            }
        //        }
        //    }
        //}

        //string tbx_subjtype_value = txtsubjtype.Text.ToString();
        //string tbx_subj_value = txtsubj.Text.ToString();
        //string tbx_subjcode_name = txtsub_name_code.Text.ToString();

        ////=================================================
        //btngenerate_Click(sender, e);
        //string clmnheadrname = "";
        //string subhdrtext = "";
        //int srtcnt = 0;
        //int subheadrname = 0;
        //string get_clm_tag = "";
        //Boolean child_flag = false;
        //string subcolumntext = "";

        //int total_clmn_count = sprdviewrcrd.Sheets[0].ColumnCount - 2;

        //for (srtcnt = 0; srtcnt < total_clmn_count; srtcnt++)
        //{
        //    if (sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text != "")
        //    {
        //        if (sprdviewrcrd.Sheets[0].Columns[srtcnt].Visible == true)
        //        {
        //            subcolumntext = "";
        //            if (clmnheadrname == "")
        //            {
        //                clmnheadrname = sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //            }
        //            else
        //            {
        //                if (child_flag == false)
        //                {
        //                    //if (sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Total" && sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Admission No" && sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Signature of the Candidate") //08.06.12
        //                    //{
        //                    clmnheadrname = clmnheadrname + "," + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                    //}
        //                }
        //                else
        //                {
        //                    //if (sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Total" && sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Admission No" && sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 4, srtcnt].Text != "Signature of the Candidate") //08.06.12
        //                    //{
        //                    clmnheadrname = clmnheadrname + "$)," + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 2, srtcnt].Text;
        //                    // }
        //                }
        //            }
        //            child_flag = false;
        //        }
        //    }
        //    else
        //    {
        //        child_flag = true;
        //        if (subcolumntext == "")
        //        {
        //            if (srtcnt != 0)
        //            {
        //                for (int te = srtcnt - 1; te <= srtcnt; te++)
        //                {
        //                    if (te == srtcnt - 1)
        //                    {
        //                        clmnheadrname = clmnheadrname + "* ($" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                        subcolumntext = clmnheadrname + "* ($" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                    }
        //                    else
        //                    {
        //                        clmnheadrname = clmnheadrname + "$" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, te].Text;
        //                        subcolumntext = clmnheadrname + "$" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, te].Text;

        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            subcolumntext = subcolumntext + "$" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //            clmnheadrname = clmnheadrname + "$" + sprdviewrcrd.Sheets[0].ColumnHeader.Cells[sprdviewrcrd.Sheets[0].ColumnHeader.RowCount - 1, srtcnt].Text;
        //        }
        //    }
        //}
        //clmnheadrname = clmnheadrname + "$)";
        //string bat_deg_branch = "";
        //if (ddlsec.Text != "")
        //{
        //    bat_deg_branch = ddlbatch.SelectedItem.Text + "[" + ddldegree.SelectedItem.Text + "]" + ddlbranch.SelectedItem.Text + "-" + ddlsem.SelectedItem.Text + "-" + ddlsec.SelectedItem.Text;
        //}
        //else
        //{
        //    bat_deg_branch = ddlbatch.SelectedItem.Text + "[" + ddldegree.SelectedItem.Text + "]" + ddlbranch.SelectedItem.Text + "-" + ddlsem.SelectedItem.Text;
        //}



        //Session["page_redirect_value"] = ddlbatch.SelectedIndex + "," + ddldegree.SelectedIndex + "," + ddlbranch.SelectedIndex + "," + ddlsem.SelectedIndex + "," + ddlsec.SelectedIndex + "$" + selected_criteria.ToString() + "$" + selected_subj.ToString() + "$" + selected_subcode_name.ToString();
        ////Session["redirect_query_string"] = clmnheadrname.ToString() + ":" + "internalassessment.aspx" + ":" + "Consolidated Internal Marks" + ":" + bat_deg_branch.ToString();
        ////Response.Redirect("Print_Master_Setting_new.aspx?ID=" + clmnheadrname.ToString() + ":" + "internalassessment.aspx" + ":" + "Consolidated Internal Marks" + ":" + bat_deg_branch.ToString());

        //Session["redirect_query_string"] = clmnheadrname.ToString() + ":" + "internalassessment.aspx" + ":" + ddlbatch.SelectedItem.ToString() + " Batch - " + ddldegree.SelectedItem.ToString() + "-" + ddlbranch.SelectedItem.ToString() + "[ " + ddlsem.SelectedItem.ToString() + "  Semester ] " + ddlsec.SelectedItem.Text + " :" + "Consolidated Internal Marks";
        //Response.Redirect("Print_Master_Setting_new.aspx");


    }




    //---------func beginning for footer

    //'-------------------------------------------------------------------------------
    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e) //modified on 07.04.12
    {
        ////sprdviewrcrd.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        //// sprdviewrcrd.Sheets[0].ColumnHeader.Rows[7].Visible = false;
        ////sprdviewrcrd.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //// sprdviewrcrd.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //hat.Clear();
        //hat.Add("college_code", Session["collegecode"].ToString());
        //hat.Add("form_name", "internalassessment.aspx");
        //dsprint = daccess2.select_method("PROC_PRINT_MASTER_SETTINGS", hat, "sp");

        //if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "0")
        //{
        //    // SpreadBind();
        //    for (int i = 0; i < sprdviewrcrd.Sheets[0].RowCount - 3; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 14;
        //    if (end >= sprdviewrcrd.Sheets[0].RowCount)
        //    {
        //        end = sprdviewrcrd.Sheets[0].RowCount;
        //    }
        //    int rowstart = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = true;

        //        if (dsprint.Tables[0].Rows[0]["footer_name"].ToString() != string.Empty)
        //        {
        //            if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0") //all pages footer
        //            {
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = true;
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = true;
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = true;
        //            }
        //            else //last page footer
        //            {
        //                if (ddlpage.SelectedIndex == (ddlpage.Items.Count - 1))
        //                {
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = true;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = true;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = true;
        //                }
        //                else
        //                {
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = false;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = false;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = false;
        //                }
        //            }
        //        }
        //    }
        //    for (int h = 0; h < sprdviewrcrd.Sheets[0].ColumnHeader.RowCount; h++)   //visible the clmn header rowcount
        //    {
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[h].Visible = true;

        //    }
        //    sprdviewrcrd.Height = 100 + (sprdviewrcrd.Sheets[0].RowCount * 10);
        //}
        //else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "1")
        //{
        //    //  SpreadBind();
        //    for (int i = 0; i < sprdviewrcrd.Sheets[0].RowCount; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 14;
        //    if (end >= sprdviewrcrd.Sheets[0].RowCount)
        //    {
        //        end = sprdviewrcrd.Sheets[0].RowCount;
        //    }
        //    int rowstart = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = true;

        //        if (dsprint.Tables[0].Rows[0]["footer_name"].ToString() != string.Empty)
        //        {
        //            if (dsprint.Tables[0].Rows[0]["footer_flag_value"].ToString() == "0") //all pages footer
        //            {
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = true;
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = true;
        //                sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = true;
        //            }
        //            else //last page footer
        //            {
        //                if (ddlpage.SelectedIndex == (ddlpage.Items.Count - 1))
        //                {
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = true;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = true;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = true;
        //                }
        //                else
        //                {
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 3].Visible = false;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 2].Visible = false;
        //                    sprdviewrcrd.Sheets[0].Rows[sprdviewrcrd.Sheets[0].RowCount - 1].Visible = false;
        //                }
        //            }
        //        }
        //    }
        //    if (Convert.ToInt32(ddlpage.SelectedValue.ToString()) == 1)
        //    {
        //        for (int h = 0; h < sprdviewrcrd.Sheets[0].ColumnHeader.RowCount; h++)
        //        {
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[h].Visible = true;

        //            sprdviewrcrd.Height = 100 + (sprdviewrcrd.Sheets[0].RowCount * 10);
        //        }
        //    }
        //    else
        //    {
        //        for (int h = 0; h < sprdviewrcrd.Sheets[0].ColumnHeader.RowCount; h++)
        //        {
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[h].Visible = false;
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[7].Visible = false;
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //            sprdviewrcrd.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //        }
        //    }
        //}
        //else if (dsprint.Tables[0].Rows[0]["header_flag_value"].ToString() == "2")
        //{
        //    for (int i = 0; i < sprdviewrcrd.Sheets[0].RowCount; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = false;
        //    }
        //    int start = Convert.ToInt32(ddlpage.SelectedValue.ToString());
        //    int end = start + 14;
        //    if (end >= sprdviewrcrd.Sheets[0].RowCount)
        //    {
        //        end = sprdviewrcrd.Sheets[0].RowCount;
        //    }
        //    int rowstart = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(start);
        //    int rowend = sprdviewrcrd.Sheets[0].RowCount - Convert.ToInt32(end);
        //    for (int i = start - 1; i < end; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = true;
        //    }

        //    for (int h = 0; h < sprdviewrcrd.Sheets[0].ColumnHeader.RowCount; h++)
        //    {
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[h].Visible = false;
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[7].Visible = false;
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //    }
        //    sprdviewrcrd.Height = (sprdviewrcrd.Sheets[0].RowCount * 10);
        //}

        //////'-----------------------------------------------------------------------------
        //if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        //{
        //    for (int i = 0; i < sprdviewrcrd.Sheets[0].RowCount; i++)
        //    {
        //        sprdviewrcrd.Sheets[0].Rows[i].Visible = true;
        //    }
        //    Double totalRows = 0;
        //    totalRows = Convert.ToInt32(sprdviewrcrd.Sheets[0].RowCount);
        //    Session["totalPages"] = (int)Math.Ceiling(totalRows / sprdviewrcrd.Sheets[0].PageSize);
        //    //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //    DropDownListpage.Items.Clear();
        //    if (totalRows >= 10)
        //    {
        //        sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
        //        {
        //            DropDownListpage.Items.Add((k + 10).ToString());
        //        }
        //        DropDownListpage.Items.Add("Others");
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        sprdviewrcrd.Height = 335;

        //    }
        //    else if (totalRows == 0)
        //    {
        //        DropDownListpage.Items.Add("0");
        //        sprdviewrcrd.Height = 100;
        //    }
        //    else
        //    {
        //        sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt32(totalRows);
        //        DropDownListpage.Items.Add(sprdviewrcrd.Sheets[0].PageSize.ToString());
        //        sprdviewrcrd.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //    }
        //    if (Convert.ToInt32(sprdviewrcrd.Sheets[0].RowCount) > 10)
        //    {
        //        DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
        //        sprdviewrcrd.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
        //        //  sprdviewrcrd.Height = 100 + (10 * Convert.ToInt32(totalRows));
        //        // CalculateTotalPages();
        //    }

        //}
        //else
        //{


        //}

        //sprdviewrcrd.Height = (sprdviewrcrd.Sheets[0].RowCount * 10);
    }
    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    //sprdviewrcrd.Visible = true;
                    Showgrid.Visible = true;
                    btnmasterprint.Visible = true;
                    btnDirectPrint.Visible = true;
                    pageset_pnl.Visible = true;
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
                    lblrptname.Visible = true;
                    txtexcelname.Visible = true;
                    btnExcel.Visible = true;
                    LabelE.Visible = false;
                    //sprdviewrcrd.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    //sprdviewrcrd.Visible = true;
                    Showgrid.Visible = true;
                    btnmasterprint.Visible = true;
                    btnDirectPrint.Visible = true;
                    pageset_pnl.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = "";
        }
    }
    void CalculateTotalPages()
    {
        //Double totalRows = 0;
        //totalRows = Convert.ToInt32(sprdviewrcrd.Sheets[0].RowCount);
        //Session["totalPages"] = (int)Math.Ceiling(totalRows / sprdviewrcrd.Sheets[0].PageSize);
        //Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        //Buttontotal.Visible = true;
    }
    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {

        try
        {

            if (TextBoxother.Text != "")
            {

                //sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                // CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = "";
        }
    }
    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        //LabelE.Visible = false;
        //TextBoxother.Text = "";
        //if (DropDownListpage.Text == "Others")
        //{

        //    TextBoxother.Visible = true;
        //    TextBoxother.Focus();

        //}
        //else
        //{
        //    lblrptname.Visible = true;
        //    txtexcelname.Visible = true;
        //    btnExcel.Visible = true;
        //    TextBoxother.Visible = false;
        //    //sprdviewrcrd.Visible = true;
        //    Showgrid.Visible = true;
        //    btnmasterprint.Visible = true;
        //    pageset_pnl.Visible = true;
        //    //sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        //    // CalculateTotalPages();
        //    //  sprdviewrcrd.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
        //}
        //sprdviewrcrd.SaveChanges();
        //sprdviewrcrd.CurrentPage = 0;

        //for (int rowhdr = 0; rowhdr < sprdviewrcrd.Sheets[0].ColumnHeader.RowCount; rowhdr++)
        //{
        //    sprdviewrcrd.Sheets[0].ColumnHeader.Rows[rowhdr].Visible = true;
        //    sprdviewrcrd.Sheets[0].ColumnHeader.Rows[8].Visible = false;
        //    sprdviewrcrd.Sheets[0].ColumnHeader.Rows[7].Visible = false;
        //    sprdviewrcrd.Sheets[0].ColumnHeader.Rows[6].Visible = false;
        //    sprdviewrcrd.Sheets[0].ColumnHeader.Rows[5].Visible = false;
        //}
    }
    public void function_radioheader()
    {
        //ddlpage.Items.Clear();
        //int totrowcount = sprdviewrcrd.Sheets[0].RowCount;
        //int pages = totrowcount / 14;
        //int intialrow = 1;
        //int remainrows = totrowcount % 14;
        //int i = 0;
        //if (sprdviewrcrd.Sheets[0].RowCount > 0)
        //{
        //    int i5 = 0;
        //    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //    for (i = 1; i <= pages; i++)
        //    {
        //        i5 = i;

        //        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //        intialrow = intialrow + 14;
        //    }
        //    if (remainrows > 0)
        //    {
        //        i = i5 + 1;
        //        ddlpage.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
        //    }
        //}

    }
    protected void chkbxlistsubjtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        int chk_tru_cnt = 0;
        for (int item_cnt = 0; item_cnt < chkbxlistsubjtype.Items.Count; item_cnt++)
        {
            if (chkbxlistsubjtype.Items[item_cnt].Selected == true)
            {
                chk_tru_cnt++;
            }
        }
        txtsubjtype.Text = "Subject Type(" + chk_tru_cnt + ")";
        if (chk_tru_cnt == chkbxlistsubjtype.Items.Count)
        {
            chksubjtype.Checked = true;
        }
        else
        {
            chksubjtype.Checked = false;
        }
        bindsubject();
    }
    protected void chkbxlistsubj_SelectedIndexChanged(object sender, EventArgs e)
    {
        int chk_tru_cnt = 0;
        for (int item_cnt = 0; item_cnt < chkbxlistsubj.Items.Count; item_cnt++)
        {
            if (chkbxlistsubj.Items[item_cnt].Selected == true)
            {
                chk_tru_cnt++;
            }
        }
        txtsubj.Text = "Subject(" + chk_tru_cnt + ")";
        if (chk_tru_cnt == chkbxlistsubj.Items.Count)
        {
            chksubj.Checked = true;
        }
        else
        {
            chksubj.Checked = false;
        }
    }
    protected void chksubjtype_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubjtype.Checked == true)
        {
            for (int item_cnt = 0; item_cnt < chkbxlistsubjtype.Items.Count; item_cnt++)
            {
                chkbxlistsubjtype.Items[item_cnt].Selected = true;
            }
            txtsubjtype.Text = "Subject Type(" + chkbxlistsubjtype.Items.Count + ")";
        }
        else
        {
            for (int item_cnt = 0; item_cnt < chkbxlistsubjtype.Items.Count; item_cnt++)
            {
                chkbxlistsubjtype.Items[item_cnt].Selected = false;
            }
            txtsubjtype.Text = "Subject Type(0)";
        }
        bindsubject();
    }

    protected void chksubj_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubj.Checked == true)
        {
            for (int item_cnt = 0; item_cnt < chkbxlistsubj.Items.Count; item_cnt++)
            {
                chkbxlistsubj.Items[item_cnt].Selected = true;
            }
            txtsubj.Text = "Subject(" + chkbxlistsubj.Items.Count + ")";
        }
        else
        {
            for (int item_cnt = 0; item_cnt < chkbxlistsubj.Items.Count; item_cnt++)
            {
                chkbxlistsubj.Items[item_cnt].Selected = false;
            }
            txtsubj.Text = "Subject(0)";
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string reportname = txtexcelname.Text;
        if (reportname.ToString().Trim() != "")
        {
            //daccess2.printexcelreport(sprdviewrcrd, reportname.ToString().Trim());

            daccess2.printexcelreportgrid(Showgrid, reportname);
            lblerr.Visible = false;
        }
        else
        {
            lblerr.Text = "Please Enter Your Report Name";
            lblerr.Visible = true;
        }

    }

    protected void chkbxlisisub_name_code_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtsub_name_code.Text = "";
        if (chkbxlisisub_name_code.Items[0].Selected == true)
        {
            txtsub_name_code.Text = chkbxlisisub_name_code.Items[0].Text.ToString();
        }
        if (chkbxlisisub_name_code.Items[1].Selected == true)
        {
            if (txtsub_name_code.Text.ToString().Trim() != "")
            {
                txtsub_name_code.Text = txtsub_name_code.Text.ToString() + "," + chkbxlisisub_name_code.Items[1].Text.ToString();
            }
            else
            {
                txtsub_name_code.Text = chkbxlisisub_name_code.Items[1].Text.ToString();
            }
        }
    }
    protected void chklscolumn_SelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        //sprdviewrcrd.Visible = false;
        Showgrid.Visible = false;
        btnmasterprint.Visible = false;
        btnDirectPrint.Visible = false;
        btnExcel.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Text = "";
    }
    protected void CheckPassedOut()
    {
        if (!chkincludepastout.Checked)
        {
            includePastout = "and CC=0";
        }
    }
    protected void includepastout_CheckedChanged(object sender, EventArgs e)
    {

    }
    public void btnPrint11()
    {
        DAccess2 d2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = d2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Internal Assessment";



    }

    public override void VerifyRenderingInServerForm(Control control)
    { }
    
}