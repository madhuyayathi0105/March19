using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;

public partial class StudentwiseArrearStatus : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DAccess2 da = new DAccess2();
    Hashtable hast = new Hashtable();
    Hashtable ht = new Hashtable();
    DataSet dset = new DataSet();

    string val22 = "";
    int cnt = 0;
    string buildvalue1 = "";
    string buildvalue2 = "";
    string buildvalue3 = "";
    string build2 = "";
    int count = 0;

    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
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
            if (!IsPostBack)
            {
                bindMonthandYear();
                binddegree();
                binddept();
                bindsubname();

            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void dropdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            binddept();
            lblerrormsg.Visible = false;
            bindsubname();
        }
        catch (Exception ex)
        {
        }
    }

    protected void dd_dept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsubname();
            lblerrormsg.Visible = false;
            grid1common.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            grid2general.Visible = false;
            g2btnexcel.Visible = false;
            g2btnprint.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    protected void go_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", false);
        }
        catch
        {
        }
    }

    protected void chklstselc_subtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dd_subtype.SelectedItem.Text == "Common")
            {
                bindsubname();
               // Panel22.Visible = false;
                lbldegree.Visible = false;
                dd_degree.Visible = false;
                lbldept.Visible = false;
                dd_dept.Visible = false;
                grid1common.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
                grid2general.Visible = false;
                g2btnexcel.Visible = false;
                g2btnprint.Visible = false;

            }
            else
            {
                bindsubname();
               // Panel22.Visible = true;
                lbldegree.Visible = true;
                dd_degree.Visible = true;
                lbldept.Visible = true;
                dd_dept.Visible = true;
                grid1common.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
                grid2general.Visible = false;
                g2btnexcel.Visible = false;
                g2btnprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindMonthandYear()
    {
        try
        {
            // ddexm_month.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "0"));
            ddexm_month.Items.Insert(0, new System.Web.UI.WebControls.ListItem("January", "1"));
            ddexm_month.Items.Insert(1, new System.Web.UI.WebControls.ListItem("February", "2"));
            ddexm_month.Items.Insert(2, new System.Web.UI.WebControls.ListItem("March", "3"));
            ddexm_month.Items.Insert(3, new System.Web.UI.WebControls.ListItem("April", "4"));
            ddexm_month.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
            ddexm_month.Items.Insert(5, new System.Web.UI.WebControls.ListItem("June", "6"));
            ddexm_month.Items.Insert(6, new System.Web.UI.WebControls.ListItem("July", "7"));
            ddexm_month.Items.Insert(7, new System.Web.UI.WebControls.ListItem("August", "8"));
            ddexm_month.Items.Insert(8, new System.Web.UI.WebControls.ListItem("September", "9"));
            ddexm_month.Items.Insert(9, new System.Web.UI.WebControls.ListItem("October", "10"));
            ddexm_month.Items.Insert(10, new System.Web.UI.WebControls.ListItem("November", "11"));
            ddexm_month.Items.Insert(11, new System.Web.UI.WebControls.ListItem("December", "12"));

            int year;
            year = Convert.ToInt16(DateTime.Today.Year);
            dd_year.Items.Clear();
            for (int l = 0; l <= 7; l++)
            {
                dd_year.Items.Add(Convert.ToString(year - l));
            }
            //dd_year.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Select", "1"));
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsubname()
    {
        try
        {
            chcklist_subname.Items.Clear();
            string sql = "";
            if (ddexm_month.SelectedValue != "" && dd_year.SelectedValue != "")
            {
                if (dd_subtype.SelectedItem.Text == "Common")
                {
                    //sql = "select distinct s.subject_name from exmtt_det et,exmtt e,subject s where s.subject_no=et.subject_no and e.exam_code=et.exam_code and e.exam_Month='" + ddexm_month.SelectedValue + "' and e.Exam_Year='" + dd_year.SelectedValue + "'  and s.CommonSub=1 order by s.subject_name";
                    sql = "select distinct s.subject_name from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code and e.exam_Month='" + ddexm_month.SelectedValue + "' and e.Exam_year='" + dd_year.SelectedValue + "' and s.CommonSub=1 order by s.subject_name";
                }
                else
                {
                    //sql = "select distinct s.subject_name from exmtt_det et,exmtt e,subject s where s.subject_no=et.subject_no and e.exam_code=et.exam_code and e.exam_Month='" + ddexm_month.SelectedValue + "' and e.Exam_Year='" + dd_year.SelectedValue + "' order by s.subject_name";
                    sql = "select distinct s.subject_name from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code and  e.exam_Month='" + ddexm_month.SelectedValue + "' and e.Exam_year='" + dd_year.SelectedValue + "' and e.degree_code='" + dd_dept.Text.ToString() + "' order by s.subject_name";
                }
            }
            else
            {
                if (dd_subtype.SelectedItem.Text == "Common")
                {
                    //sql = "select distinct s.subject_name from exmtt_det et,exmtt e,subject s where s.subject_no=et.subject_no and e.exam_code=et.exam_code  and s.CommonSub=1 order by s.subject_name";
                    sql = "select distinct s.subject_name from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code and s.CommonSub=1 order by s.subject_name";
                }
                else
                {
                    //sql = "select distinct s.subject_name from exmtt_det et,exmtt e,subaject s where s.subject_no=et.subject_no and e.exam_code=et.exam_code order by s.subject_name";
                    sql = "select distinct s.subject_name from Mark_Entry m,Exam_Details e,subject s where m.subject_no=s.subject_no and m.exam_code=e.exam_code order by s.subject_name";
                }
            }
            ds = da.select_method_wo_parameter(sql, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                chcklist_subname.DataSource = ds;
                chcklist_subname.DataTextField = "subject_name";
                chcklist_subname.DataValueField = "subject_name";
                chcklist_subname.DataBind();
            }
            if (chcklist_subname.Items.Count > 0)
            {
                int cout = 0;
                for (int i = 0; i < chcklist_subname.Items.Count; i++)
                {
                    cout++;
                    chcklist_subname.Items[i].Selected = true;
                }
                chck_subname.Checked = true;
                txt_subname.Text = "Subject Name (" + cout + ")";
            }
            else
            {
                chck_subname.Checked = false;
                txt_subname.Text = "---Select---";
            }
        }

        catch (Exception ex)
        {
        }
    }

    protected void dd_exmmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsubname();
            grid1common.Visible = false;
            g1btnprint.Visible = false;
            g1btnexcel.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void dd_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindsubname();
            grid1common.Visible = false;
            g1btnprint.Visible = false;
            g1btnexcel.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void binddegree()
    {
        try
        {
            dd_degree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hast.Clear();
            hast.Add("single_user", singleuser);
            hast.Add("group_code", group_user);
            hast.Add("college_code", collegecode);
            hast.Add("user_code", usercode);
            ds = da.select_method("bind_degree", hast, "sp");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                dd_degree.DataSource = ds;
                dd_degree.DataTextField = "course_name";
                dd_degree.DataValueField = "course_id";
                dd_degree.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddept()
    {
        try
        {
            hast.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hast.Add("single_user", singleuser);
            hast.Add("group_code", group_user);
            hast.Add("course_id", dd_degree.SelectedValue);
            hast.Add("college_code", collegecode);
            hast.Add("user_code", usercode);
            ds = da.select_method("bind_branch", hast, "sp");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                dd_dept.DataSource = ds;
                dd_dept.DataTextField = "dept_name";
                dd_dept.DataValueField = "degree_code";
                dd_dept.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chcksubname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (chck_subname.Checked == true)
            {
                for (int i = 0; i < chcklist_subname.Items.Count; i++)
                {

                    if (chck_subname.Checked == true)
                    {
                        chcklist_subname.Items[i].Selected = true;
                        txt_subname.Text = "Subject Name (" + (chcklist_subname.Items.Count) + ")";
                        build1 = chcklist_subname.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "," + build1;
                        }
                    }
                }
            }

            else
            {
                for (int i = 0; i < chcklist_subname.Items.Count; i++)
                {
                    chcklist_subname.Items[i].Selected = false;
                    txt_subname.Text = "---Select---";

                    chcklist_subname.ClearSelection();
                    chck_subname.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void cheklistsubname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chck_subname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < chcklist_subname.Items.Count; i++)
            {
                if (chcklist_subname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_subname.Text = "---Select---";
                    build = chcklist_subname.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + build;
                    }
                }
            }
            if (seatcount == chcklist_subname.Items.Count)
            {
                txt_subname.Text = "Subject Name (" + seatcount.ToString() + ")";
                chck_subname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_subname.Text = "---Select---";
                chck_subname.Text = "Select All";
            }
            else
            {
                txt_subname.Text = "Subject Name (" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            if (dd_subtype.SelectedItem.Text != "General")
            {
                DataTable dt = new DataTable();
                Hashtable hst = new Hashtable();
                DataRow dr = null;
                ArrayList add = new ArrayList();
                dt.Columns.Add("S.No", typeof(string));
                dt.Columns.Add("Reg.No", typeof(string));
                dt.Columns.Add("Student Name", typeof(string));
                for (int r = 0; r < chcklist_subname.Items.Count; r++)
                {
                    if (chcklist_subname.Items[r].Selected == true)
                    {
                        build2 = chcklist_subname.Items[r].Value;
                        buildvalue1 = chcklist_subname.Items[r].Text;
                        //    }
                        //}
                        //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and result  in('fail') and passorfail in(0) and s.subject_name='" + buildvalue1 + "' ";
                        //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and y.semester=e.current_semester and e.current_semester=rt.Current_Semester and e.batch_year=rt.Batch_Year and e.degree_code=rt.degree_code and result  in('fail') and passorfail  in(0) and e.Exam_year=" + dd_year.SelectedValue.ToString() + " and e.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and s.subject_name='" + buildvalue1 + "' ";
                        int sno1 = 0;
                        string common = "select distinct rt.Reg_No,rt.Stud_Name,s.subject_name,s.subject_code from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt ,Exam_Details ex where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no  and m.exam_code=ex.exam_code and ex.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and ex.Exam_year=" + dd_year.SelectedValue.ToString() + " and result  in('fail','WHD','AAA') and passorfail  in(0) and s.subject_name='" + buildvalue1 + "'";
                        //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt ,exmtt_det et,exmtt e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and s.subject_no=et.subject_no and m.subject_no=et.subject_no  and  e.exam_code=et.exam_code and result  in('fail') and passorfail  in(0)  and e.Exam_year=" + dd_year.SelectedValue.ToString() + " and e.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and s.subject_name='" + buildvalue1 + "'";
                        DataSet ds3 = new DataSet();
                        ds3 = d2.select_method(common, hast, "Text");
                        if (ds3.Tables[0].Rows.Count > 0)
                        {
                            string subinfo = "select distinct subject_name from subject where subject_name!=''";
                            ds = d2.select_method_wo_parameter(subinfo, "text");


                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int ik = 0; ik < chcklist_subname.Items.Count; ik++)
                                {
                                    string sub = chcklist_subname.Items[ik].Text;
                                    DataView dv1 = new DataView();
                                    if (chcklist_subname.Items[ik].Selected == true)
                                    {

                                        if (ds3.Tables[0].Rows.Count > 0)
                                        {
                                            ds3.Tables[0].DefaultView.RowFilter = "subject_name='" + sub + "'";
                                            dv1 = ds3.Tables[0].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                DataRow dr11 = null;
                                                dr11 = dt.NewRow();
                                                dr11[0] = dv1[0]["subject_code"].ToString() + "-" + sub;
                                                dt.Rows.Add(dr11);
                                                add.Add(dt.Rows.Count);
                                                for (int jk = 0; jk < dv1.Count; jk++)
                                                {
                                                    dr = dt.NewRow();
                                                    dr[0] = jk + 1;
                                                    dr[1] = dv1[jk]["Reg_No"].ToString();
                                                    dr[2] = dv1[jk]["Stud_Name"].ToString();
                                                    dt.Rows.Add(dr);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lblerrormsg.Visible = true;
                                lblerrormsg.Text = "No Records Found";
                                grid1common.Visible = false;
                                g1btnprint.Visible = false;
                                g1btnexcel.Visible = false;
                            }
                        }
                        else
                        {
                            lblerrormsg.Visible = true;
                            lblerrormsg.Text = "No Records Found";
                            grid1common.Visible = false;
                            g1btnprint.Visible = false;
                            g1btnexcel.Visible = false;
                            //g2btnprint.Visible = false;
                            //g2btnexcel.Visible = false;
                        }
                    }
                }
                grid1common.DataSource = dt;
                grid1common.DataBind();
                grid1common.Visible = true;
                lblerrormsg.Visible = false;
                g1btnprint.Visible = true;
                g1btnexcel.Visible = true;
                if (add.Count > 0)
                {
                    for (int a = 0; a < add.Count; a++)
                    {
                        string row = Convert.ToString(add[a]);
                        int row1 = 0;
                        row1 = Convert.ToInt32(row) - 1;
                        grid1common.Rows[row1].Cells[0].ColumnSpan = 3;
                        grid1common.Rows[row1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        grid1common.Rows[row1].Cells[0].ForeColor = System.Drawing.Color.Black;
                        grid1common.Rows[row1].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                        grid1common.Rows[row1].Cells[1].Visible = false;
                        grid1common.Rows[row1].Cells[2].Visible = false;
                    }
                }
                else
                {
                    lblerrormsg.Visible = true;
                    lblerrormsg.Text = "No Records Found";
                    grid1common.Visible = false;
                    g1btnprint.Visible = false;
                    g1btnexcel.Visible = false;
                }
            }
            else
            {

                if (dd_subtype.SelectedItem.Text == "General")
                {
                    DataTable dt = new DataTable();
                    Hashtable hst = new Hashtable();
                    DataRow dr = null;
                    ArrayList add = new ArrayList();
                    ArrayList snoarray = new ArrayList();

                    //if (dd_degree.SelectedItem.Text == "true")
                    //{
                    //    if (dd_dept.SelectedItem.Text == "true")
                    //    {

                    //    }
                    //}

                    dt.Columns.Add("S.No", typeof(string));
                    dt.Columns.Add("Reg.No", typeof(string));
                    dt.Columns.Add("Student Name", typeof(string));

                    for (int r = 0; r < chcklist_subname.Items.Count; r++)
                    {
                        if (chcklist_subname.Items[r].Selected == true)
                        {
                            build2 = chcklist_subname.Items[r].Value;
                            buildvalue1 = chcklist_subname.Items[r].Text;

                            //}
                            //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and result  in('fail') and passorfail in(0) and s.subject_name='" + buildvalue1 + "' ";
                            //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt,Exam_Details e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and e.exam_code=m.exam_code and e.degree_code=d.Degree_Code and y.degree_code=e.degree_code and y.semester=e.current_semester and e.current_semester=rt.Current_Semester and e.batch_year=rt.Batch_Year and e.degree_code=rt.degree_code and result  in('fail') and passorfail  in(0) and e.Exam_year=" + dd_year.SelectedValue.ToString() + " and e.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and s.subject_name='" + buildvalue1 + "' ";
                            //string common = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name, rt.degree_code, rd.Dept_name from subject s,syllabus_master y,mark_entry m, Department rd, Degree d,Registration rt ,exmtt_det et,exmtt e where s.syll_code = y.syll_code and s.subject_no  = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and s.subject_no=et.subject_no and m.subject_no=et.subject_no and e.exam_code=et.exam_code and result  in('fail') and passorfail in(0) and e.Exam_year=" + dd_year.SelectedValue.ToString() + " and e.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and rt.degree_code='" + dd_degree.SelectedValue.ToString() + "' and rd.Dept_Name='" + dd_dept.SelectedValue.ToString() + "' ";
                            //string general = "select distinct rt.Roll_No,rt.Stud_Name,s.subject_name from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt ,exmtt_det et,exmtt e where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no and s.subject_no=et.subject_no and m.subject_no=et.subject_no and e.exam_code=et.exam_code and result  in('fail') and passorfail  in(0)  and e.Exam_year=" + dd_year.SelectedValue.ToString() + " and e.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and e.degree_code='" + dd_dept.SelectedValue + "' and s.subject_name='" + buildvalue1 + "'";

                            string general = "select distinct rt.Reg_No,rt.Stud_Name,s.subject_name,s.subject_code from subject s,syllabus_master y,mark_entry m, Degree d,Registration rt ,Exam_Details ex where s.syll_code = y.syll_code and s.subject_no = m.subject_no and d.Degree_Code=y.degree_code and rt.Roll_No=m.roll_no  and m.exam_code=ex.exam_code and ex.Exam_Month=" + ddexm_month.SelectedValue.ToString() + " and ex.Exam_year=" + dd_year.SelectedValue.ToString() + " and rt.degree_code='" + dd_dept.SelectedValue + "' and result  in('fail','WHD','AAA') and passorfail  in(0) and s.subject_name='" + buildvalue1 + "'";
                            DataSet ds3 = new DataSet();
                            ds3 = d2.select_method(general, hast, "Text");
                            if (ds3.Tables[0].Rows.Count > 0)
                            {
                                string subinfo = "select distinct subject_name from subject where subject_name!=''";
                                ds = d2.select_method_wo_parameter(subinfo, "text");


                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int ik = 0; ik < chcklist_subname.Items.Count; ik++)
                                    {
                                        snoarray.Clear();
                                        count = 0;
                                        string sub = chcklist_subname.Items[ik].Text;
                                        DataView dv1 = new DataView();
                                        if (chcklist_subname.Items[ik].Selected == true)
                                        {
                                            if (ds3.Tables[0].Rows.Count > 0)
                                            {
                                                ds3.Tables[0].DefaultView.RowFilter = "subject_name='" + sub + "'";
                                                dv1 = ds3.Tables[0].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    DataRow dr11 = null;
                                                    dr11 = dt.NewRow();
                                                    dr11[0] = dv1[0]["subject_code"].ToString() + "-" + sub;
                                                    dt.Rows.Add(dr11);
                                                    add.Add(dt.Rows.Count);
                                                    for (int jk = 0; jk < dv1.Count; jk++)
                                                    {
                                                        if (!snoarray.Contains(Convert.ToString(dv1[jk]["Reg_No"]) + "-" + Convert.ToString(dv1[jk]["Stud_Name"])))
                                                        {
                                                            count++;
                                                            snoarray.Add(Convert.ToString(dv1[jk]["Reg_No"]) + "-" + Convert.ToString(dv1[jk]["Stud_Name"]));
                                                        }
                                                        dr = dt.NewRow();
                                                        dr[0] = count;
                                                        dr[1] = dv1[jk]["Reg_No"].ToString();
                                                        dr[2] = dv1[jk]["Stud_Name"].ToString();
                                                        dt.Rows.Add(dr);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    lblerrormsg.Visible = true;
                                    lblerrormsg.Text = "No Records Found";
                                    grid2general.Visible = false;
                                    g2btnprint.Visible = false;
                                    g2btnexcel.Visible = false;
                                }
                            }
                            else
                            {
                                lblerrormsg.Visible = true;
                                lblerrormsg.Text = "No Records Found";
                                grid2general.Visible = false;
                                g2btnprint.Visible = false;
                                g2btnexcel.Visible = false;
                                //g2btnprint.Visible = false;
                                //g2btnexcel.Visible = false;
                            }
                        }
                    }
                    //}
                    grid2general.DataSource = dt;
                    grid2general.DataBind();
                    grid2general.Visible = true;
                    lblerrormsg.Visible = false;
                    g2btnprint.Visible = true;
                    g2btnexcel.Visible = true;
                    if (add.Count > 0)
                    {
                        for (int a = 0; a < add.Count; a++)
                        {
                            string row = Convert.ToString(add[a]);
                            int row1 = 0;
                            row1 = Convert.ToInt32(row) - 1;
                            grid2general.Rows[row1].Cells[0].ColumnSpan = 3;
                            grid2general.Rows[row1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            grid2general.Rows[row1].Cells[0].ForeColor = System.Drawing.Color.Black;
                            grid2general.Rows[row1].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                            grid2general.Rows[row1].Cells[1].Visible = false;
                            grid2general.Rows[row1].Cells[2].Visible = false;
                        }
                    }
                    else
                    {
                        lblerrormsg.Visible = true;
                        lblerrormsg.Text = "No Records Found";
                        grid2general.Visible = false;
                        g2btnprint.Visible = false;
                        g2btnexcel.Visible = false;
                    }

                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }
    }


    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
        }
    }


    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Studentwise Arrear Status.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();
            lb.Text = dd_degree.SelectedItem.Text + "-" + dd_dept.SelectedItem.Text;
            lb.Style.Add("height", "200px");
            lb.Style.Add("text-decoration", "none");
            lb.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            lb.Style.Add("font-size", "14px");
            lb.Style.Add("text-align", "center");
            lb.RenderControl(hw);
            grid1common.AllowPaging = false;
            grid1common.HeaderRow.Style.Add("width", "15%");
            grid1common.HeaderRow.Style.Add("font-size", "10px");
            grid1common.HeaderRow.Style.Add("text-align", "center");
            grid1common.Style.Add("text-decoration", "none");
            grid1common.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            grid1common.Style.Add("font-size", "8px");
            grid1common.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A3, 7f, 7f, 7f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            Paragraph p = new Paragraph();
            string txt = "";
            p.Add(txt);
            pdfDoc.Open();
            pdfDoc.Add(p);
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }



    protected void btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Studentwise Arrear Status.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grid1common.AllowPaging = false;
                btngo_Click(sender, e);

                grid1common.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in grid1common.HeaderRow.Cells)
                {
                    cell.BackColor = grid1common.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid1common.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid1common.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid1common.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid1common.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void g1btnprint1_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=Studentwise Arrear Status.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            Label lb = new Label();
            lb.Text = dd_degree.SelectedItem.Text + "-" + dd_dept.SelectedItem.Text;
            lb.Style.Add("height", "200px");
            lb.Style.Add("text-decoration", "none");
            lb.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            lb.Style.Add("font-size", "14px");
            lb.Style.Add("text-align", "center");
            lb.RenderControl(hw);

            grid2general.AllowPaging = false;

            grid2general.HeaderRow.Style.Add("width", "15%");
            grid2general.HeaderRow.Style.Add("font-size", "10px");
            grid2general.HeaderRow.Style.Add("text-align", "center");
            grid2general.Style.Add("text-decoration", "none");
            grid2general.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            grid2general.Style.Add("font-size", "8px");

            grid2general.RenderControl(hw);

            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A3, 7f, 7f, 7f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            Paragraph p = new Paragraph();
            string txt = "";
            p.Add(txt);
            pdfDoc.Open();
            pdfDoc.Add(p);
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }
    }

    protected void btnexcel1_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Studentwise Arrear Status.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                grid2general.AllowPaging = false;
                btngo_Click(sender, e);

                grid2general.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in grid2general.HeaderRow.Cells)
                {
                    cell.BackColor = grid2general.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in grid2general.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = grid2general.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = grid2general.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                grid2general.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
        }
    }
}