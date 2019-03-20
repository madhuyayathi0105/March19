using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using InsproDataAccess;
using System.Data.SqlClient;
using System.Drawing;
using FarPoint.Web.Spread;

public partial class ExamArrearList : System.Web.UI.Page
{
    InsproDirectAccess dir = new InsproDirectAccess();
    SqlConnection mysql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_name = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_p = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_header = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subj = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subject2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sub = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sname = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    Boolean Cellclick;
    string CollegeCode;
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    static Hashtable arrcount = new Hashtable();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    string be = "", cse = "";
    string note1 = "", note2 = "";
    string post = "";

    int col_cnt = 0;

    DataSet ds_load = new DataSet();
    DAccess2 daccess2 = new DAccess2();
    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(110);
            //img.Height = Unit.Percentage(80);
            return img;


        }
    }
    public class MyImg1 : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(60);
            img.Height = Unit.Percentage(70);
            return img;


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

                FpSpread1.Visible = false;
                sprdviewrcrd.Visible = false;
                sprdviewrcrd.CommandBar.Visible = true;
                sprdviewrcrd.Sheets[0].ColumnHeader.RowCount = 6;
                sprdviewrcrd.Sheets[0].RowHeader.Visible = false;
                sprdviewrcrd.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                sprdviewrcrd.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                sprdviewrcrd.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                sprdviewrcrd.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                sprdviewrcrd.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                sprdviewrcrd.Sheets[0].DefaultStyle.Font.Bold = false;
                sprdviewrcrd.Sheets[0].ColumnCount = 4;
                sprdviewrcrd.Sheets[0].Columns[0].Width = 40;
                sprdviewrcrd.Sheets[0].Columns[1].Width = 70;
                sprdviewrcrd.Sheets[0].Columns[2].Width = 170;
                sprdviewrcrd.Sheets[0].Columns[3].Width = 110;
                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, 0].Text = "Sl. No";
                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, 1].Text = "Batch Year";
                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[1].HorizontalAlign = HorizontalAlign.Center;
                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, 2].Text = "Branch";
                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[2].HorizontalAlign = HorizontalAlign.Center;
                sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Semester";
                sprdviewrcrd.Sheets[0].ColumnHeader.Columns[3].HorizontalAlign = HorizontalAlign.Center;

                bindbatch();

                binddegree();
                bindbranch();

            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            ddlbatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            int i1 = 1;
            for (int i = 0; i < count; i++)
            {
                ddlbatch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + "", "" + ds_load.Tables[0].Rows[i]["batch_year"].ToString() + ""));
                i1++;
            }
        }
        int count1 = ds_load.Tables[1].Rows.Count;
        if (count > 0)
        {
            int max_bat = 0;
            max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            ddlbatch.SelectedValue = max_bat.ToString();
            con.Close();
        }
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
                i1++;
                ddlbranch.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
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
            i1++;
            ddldegree.Items.Insert(i1, new System.Web.UI.WebControls.ListItem("All", "0"));
        }
    }

    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        binddegree();
        bindbranch();
        //bindsem();

    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbranch();
        //bindsem();

    }

    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void txtnoofarr_TextChanged(object sender, EventArgs e)
    {

    }
    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value;
            }
        }

        return null;
    }
    protected void btngenerate_Click(object sender, EventArgs e)
    {
        arrcount.Clear();
        int sl_no = 1;
        int po = 0;
        int yyy = 1;
        string getrollno = "";
        int semarrearcount = 0;
        string batchyear = ddlbatch.SelectedItem.Text;
        string degree_val = ddldegree.SelectedValue.ToString();
        string vv = ddldegree.SelectedItem.Text;
        string degreecode = ddlbranch.SelectedValue.ToString();
        string bwww = ddlbranch.SelectedItem.Text;


        sprdviewrcrd.Sheets[0].ColumnCount = 4;
        sprdviewrcrd.Sheets[0].RowCount = 0;

        getrollno = "select distinct current_semester,batch_year,r.degree_code,d.course_id,c.course_name,d.acronym from registration r,degree d,course c where ";
        if (ddlbatch.SelectedItem.Text != "All")
        {
            getrollno = getrollno + " batch_year=" + batchyear + " and ";
        }
        if (ddldegree.SelectedItem.Text != "All")
        {
            getrollno = getrollno + "c.course_id=" + degree_val + " and ";
        }
        if (ddlbranch.SelectedItem.Text != "All")
        {
            getrollno = getrollno + " r.degree_code= " + degreecode + " and ";
        }

        getrollno = getrollno + " cc=0 and r.degree_code=d.degree_code and c.course_id=d.course_id and delflag =0 and exam_flag <>'Debar' ";

        SqlDataAdapter dagetrollno = new SqlDataAdapter(getrollno, con);
        DataSet dsgetrollno = new DataSet();
        con.Close();
        con.Open();
        dagetrollno.Fill(dsgetrollno);

        for (int ff = 0; ff < dsgetrollno.Tables[0].Rows.Count; ff++)
        {

            int new_var = 0;
            string batchyeartbl = "";
            string cur_semtbl = "";
            string degreecodetbl = "";
            string rollno = "", deg = "", sem3 = "";



            batchyeartbl = dsgetrollno.Tables[0].Rows[ff]["batch_year"].ToString();
            cur_semtbl = dsgetrollno.Tables[0].Rows[ff]["current_semester"].ToString();
            Session["qqq"] = cur_semtbl;
            degreecodetbl = dsgetrollno.Tables[0].Rows[ff]["degree_code"].ToString();

            deg = dsgetrollno.Tables[0].Rows[ff]["course_id"].ToString();
            be = dsgetrollno.Tables[0].Rows[ff]["course_name"].ToString();

            cse = dsgetrollno.Tables[0].Rows[ff]["acronym"].ToString();
            sprdviewrcrd.Sheets[0].RowCount = sprdviewrcrd.Sheets[0].RowCount + 1;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].Text = sl_no.ToString();
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 1].Text = batchyeartbl;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].Text = be + "-" + cse;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].Note = degreecodetbl;

            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 3].Text = cur_semtbl;
            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

            con_name.Close();
            con_name.Close();
            SqlCommand studinfo_new = new SqlCommand("proc_arrear_roll", con_name);
            studinfo_new.CommandType = CommandType.StoredProcedure;
            studinfo_new.Parameters.AddWithValue("@batchyear_p", batchyeartbl);
            studinfo_new.Parameters.AddWithValue("@degreecode_p", degreecodetbl);
            studinfo_new.Parameters.AddWithValue("@cur_sem_p", cur_semtbl);
            SqlDataAdapter ada_roll = new SqlDataAdapter(studinfo_new);
            DataSet ds_roll = new DataSet();
            ada_roll.Fill(ds_roll);



            for (int sub = 0; sub < ds_roll.Tables[0].Rows.Count; sub++)
            {

                rollno = ds_roll.Tables[0].Rows[sub]["roll_no"].ToString();

                //string arrsub = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from  mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='pass' and ltrim(rtrim(roll_no))='"+rollno +"') and ltrim(rtrim(roll_no))='"+rollno +"' and Semester >= 1 and Semester <= "+cur_semtbl +" ) order by smas.semester , scode";

                con_p.Close();
                con_p.Open();
                SqlCommand studinfo = new SqlCommand("proc_arrear", con_p);
                studinfo.CommandType = CommandType.StoredProcedure;
                studinfo.Parameters.AddWithValue("@rollno_p", rollno);
                studinfo.Parameters.AddWithValue("@cur_sem_p", cur_semtbl);
                SqlDataAdapter daarrsub = new SqlDataAdapter(studinfo);
                DataSet dsarrsub = new DataSet();
                daarrsub.Fill(dsarrsub);
                post = "";
                if (dsarrsub.Tables[0].Rows.Count > 0)
                {
                    for (int arrsubcount = 0; arrsubcount < dsarrsub.Tables[0].Rows.Count; arrsubcount++)
                    {
                        semarrearcount = dsarrsub.Tables[0].Rows.Count;
                        if (post == "")
                        {
                            post = dsarrsub.Tables[0].Rows[arrsubcount]["scode"].ToString();
                        }
                        else
                        {
                            post = post + "." + dsarrsub.Tables[0].Rows[arrsubcount]["scode"].ToString();
                        }
                    }
                }
                else
                {
                    semarrearcount = 0;
                }


                if (arrcount.Contains(degreecodetbl + "," + batchyeartbl + "," + semarrearcount))
                {
                    string prevroll = Convert.ToString(GetCorrespondingKey(degreecodetbl + "," + batchyeartbl + "," + semarrearcount, arrcount));
                    string newroll = rollno + "-" + post + "," + prevroll;
                    arrcount[degreecodetbl + "," + batchyeartbl + "," + semarrearcount] = newroll;


                }
                else
                {
                    arrcount.Add(degreecodetbl + "," + batchyeartbl + "," + semarrearcount, rollno + "-" + post);
                }

            }

            int noofarr = 0;

            if (txtnoofarr.Text == "")
            {
                txtnoofarr.Text = "0";
            }
            if (txtnoofarr.Text != "0")
            {
                int ffff = sprdviewrcrd.Sheets[0].ColumnCount;
                noofarr = Convert.ToInt32(txtnoofarr.Text);
                for (int i = 0; i <= noofarr; i++)
                {
                    if (yyy <= noofarr)
                    {
                        sprdviewrcrd.Sheets[0].ColumnCount = sprdviewrcrd.Sheets[0].ColumnCount + 1;


                        sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].Width = 100;
                        sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, sprdviewrcrd.Sheets[0].ColumnCount - 1].Text = Convert.ToString(i);
                        sprdviewrcrd.Sheets[0].Columns[sprdviewrcrd.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                    foreach (DictionaryEntry parameter in arrcount)
                    {


                        int noofstud = 0;
                        string b_year = "";
                        string d_code = "";
                        string s_codee = "";
                        string subcount = Convert.ToString(parameter.Key);
                        string Rollno = Convert.ToString(parameter.Value);
                        string[] splitsubcount = subcount.Split(new char[] { ',' });
                        d_code = splitsubcount[0].ToString();
                        b_year = splitsubcount[1].ToString();
                        subcount = splitsubcount[2].ToString();

                        //s_codee = splitsubcount[3].ToString();
                        if (Convert.ToInt32(subcount) == i && b_year == batchyeartbl && d_code == degreecodetbl)
                        {
                            string[] split = Rollno.Split(new char[] { ',' });

                            noofstud = split.GetUpperBound(0);
                            noofstud = noofstud + 1;

                            if (new_var == 0)
                            {
                                //sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].Text = ds_roll.Tables[1].Rows[0][0].ToString();
                                //sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            }

                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, (4 + i)].Text = noofstud.ToString();
                            sprdviewrcrd.Sheets[0].ColumnHeader.Columns[(4 + i)].HorizontalAlign = HorizontalAlign.Center;
                            sprdviewrcrd.Sheets[0].Cells[sprdviewrcrd.Sheets[0].RowCount - 1, (4 + i)].HorizontalAlign = HorizontalAlign.Center;
                            sprdviewrcrd.Sheets[0].Columns[(4 + i)].Width = 110;

                            new_var++;
                        }

                    }


                }



            }

            yyy = 200;
            sl_no++;

        }

        sprdviewrcrd.Visible = true;
        sprdviewrcrd.Sheets[0].PageSize = sprdviewrcrd.Sheets[0].RowCount;
        Session["col_cnt"] = sprdviewrcrd.Sheets[0].ColumnCount - 4;
        FpSpread1.Visible = false;
        logo_settings();


    }


    protected void sprdviewrcrd_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        Cellclick = true;
    }
    protected void sprdviewrcrd_SelectedIndexChanged(Object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 3;
        FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
        FpSpread1.Sheets[0].RowCount = 1;
        int sss_no = 1;
        int rrr = 0;
        string query_sub = "", query_sname = "";
        string batch_year_new = "";
        string degreecode_new = "";
        string sem_new = "";
        Hashtable particularsubject = new Hashtable();
        if (Cellclick == true)
        {
            string arrear_count = "";
            string activerow = "";
            string activecol = "";
            activerow = sprdviewrcrd.ActiveSheetView.ActiveRow.ToString();
            activecol = sprdviewrcrd.ActiveSheetView.ActiveColumn.ToString();

            batch_year_new = sprdviewrcrd.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text.ToString();
            degreecode_new = sprdviewrcrd.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Note;
            sem_new = sprdviewrcrd.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text.ToString();

            DataTable dtstudent = dir.selectDataTable("select stud_name,roll_no from registration where batch_year=" + batch_year_new + " and degree_code=" + degreecode_new + " and current_semester=" + sem_new + "");

            foreach (DictionaryEntry parameter in arrcount)
            {
                string subcount = Convert.ToString(parameter.Key);
                string Rollno = Convert.ToString(parameter.Value);
                string[] splitsubcount = subcount.Split(new char[] { ',' });
                subcount = splitsubcount[2].ToString();
                string noofarr = sprdviewrcrd.Sheets[0].ColumnHeader.Cells[5, Convert.ToInt32(activecol)].Text;
                arrear_count = sprdviewrcrd.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text.ToString();
                if (subcount == noofarr)
                {
                    string[] split = Rollno.Split(new char[] { ',' });

                    for (int i = 0; i < Convert.ToInt32(arrear_count); i++)
                    {
                        string rollnosplited = split[i].ToString();
                        string[] subjectarr = rollnosplited.Split(new char[] { '-' });
                        string rollnosplited1 = subjectarr[0].ToString();
                        string subjectcodesplited1 = subjectarr[1].ToString();
                        string[] subjectarr1 = subjectcodesplited1.Split(new char[] { '.' });
                        for (int j = 0; j < Convert.ToInt32(noofarr); j++)
                        {
                            string failedsubjec = subjectarr1[j].ToString();
                            if (particularsubject.Contains(failedsubjec))
                            {
                                string getrollsplited = Convert.ToString(GetCorrespondingKey(failedsubjec, particularsubject));
                                particularsubject[failedsubjec] = getrollsplited + "," + rollnosplited1;

                            }
                            else
                            {
                                particularsubject.Add(failedsubjec, rollnosplited1);

                            }
                        }
                    }
                }
                if (subcount == "0" && noofarr == "0")
                {
                    FpSpread1.Sheets[0].Cells[0, 0].Text = "Sl. No";
                    FpSpread1.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].Columns[0].Width = 60;
                    FpSpread1.Sheets[0].Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Width = 100;
                    FpSpread1.Sheets[0].Cells[0, 2].Text = "Name Of The Student";
                    FpSpread1.Sheets[0].Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].Columns[2].Width = 152;
                    string[] split = Rollno.Split(new char[] { ',' });

                    for (int i = 0; i < Convert.ToInt32(arrear_count); i++)
                    {
                        string rollnosplited = split[i].ToString();
                        string[] subjectarr = rollnosplited.Split(new char[] { '-' });
                        string rollnosplited1 = subjectarr[0].ToString();
                        FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sss_no.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text =Convert.ToString(rollnosplited1);
                        sss_no++;
                        if (dtstudent.Rows.Count > 0)
                        {
                            dtstudent.DefaultView.RowFilter = "roll_no='" + rollnosplited1 + "'";
                            DataView dvName = dtstudent.DefaultView;
                            if(dvName.Count>0)
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text =Convert.ToString(dvName[0]["stud_name"]);

                        }

                        //query_sname = "select stud_name from registration where batch_year=" + batch_year_new + " and degree_code=" + degreecode_new + " and current_semester=" + sem_new + " and roll_no='" + rollnosplited1 + "'";
                        //SqlCommand com_sname = new SqlCommand(query_sname, con_sname);
                        //SqlDataReader dr_sname = com_sname.ExecuteReader();
                        //dr_sname.Read();
                        //if (dr_sname.HasRows == true)
                        //{
                        //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dr_sname["stud_name"].ToString();
                        //}
                    }
                }

            }
            foreach (DictionaryEntry parameter in particularsubject)
            {
                string subcount_new = Convert.ToString(parameter.Key);
                con_sub.Close();
                con_sub.Open();
                query_sub = "select distinct subject_name from subject as S,syllabus_master as SM,subjectchooser as SC,Sub_sem as Sem where S.subject_no=SC.Subject_no and s.syll_code=SM.syll_code and degree_code='" + degreecode_new + "' and SM.Semester<=" + sem_new + " and batch_year=" + batch_year_new + " and S.subtype_no = Sem.subtype_no and promote_count=1 and subject_code='" + subcount_new + "'order by subject_name";
                SqlCommand com_sub = new SqlCommand(query_sub, con_sub);
                SqlDataReader dr_sub = com_sub.ExecuteReader();
                FpSpread1.Sheets[0].Cells[0, 0].Text = "Sl. No";
                FpSpread1.Sheets[0].Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].Columns[0].Width = 60;
                FpSpread1.Sheets[0].Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Cells[0, 2].Text = "Name Of The Student";
                FpSpread1.Sheets[0].Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].Columns[2].Width = 152;
                while (dr_sub.Read())
                {
                    string Rollno_new = Convert.ToString(parameter.Value);
                    string[] splitrollno = Rollno_new.Split(new char[] { ',' });
                    int rol_no_cnt = splitrollno.GetUpperBound(0);
                    rol_no_cnt = rol_no_cnt + 1;

                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 1, 1, 2);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dr_sub["subject_name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                    for (rrr = 0; rrr < rol_no_cnt; rrr++)
                    {
                        FpSpread1.Sheets[0].RowCount++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sss_no.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = splitrollno[rrr].ToString();
                        con_sname.Close();
                        con_sname.Open();

                        query_sname = "select stud_name from registration where batch_year=" + batch_year_new + " and degree_code=" + degreecode_new + " and current_semester=" + sem_new + " and roll_no='" + splitrollno[rrr] + "'";
                        SqlCommand com_sname = new SqlCommand(query_sname, con_sname);
                        SqlDataReader dr_sname = com_sname.ExecuteReader();
                        dr_sname.Read();
                        if (dr_sname.HasRows == true)
                        {
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dr_sname["stud_name"].ToString();
                        }
                        sss_no++;
                    }
                }
            }
            sprdviewrcrd.Visible = false;
            //sprdviewrcrd.SaveChanges();
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
            FpSpread1.Visible = true;
            FpSpread1.SaveChanges();

        }


        //sprdviewrcrd.Visible = false;


    }
    public void logo_settings()
    {
        string query_header = "";
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        sprdviewrcrd.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        sprdviewrcrd.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        sprdviewrcrd.Sheets[0].AllowTableCorner = true;

        sprdviewrcrd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
        sprdviewrcrd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
        sprdviewrcrd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
        sprdviewrcrd.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        sprdviewrcrd.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
        sprdviewrcrd.Sheets[0].ColumnHeader.Rows[5].BackColor = Color.AliceBlue;

        col_cnt = Convert.ToInt32(Session["col_cnt"].ToString());
        sprdviewrcrd.Sheets[0].ColumnCount = 4 + col_cnt;
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler/Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler/Handler5.ashx?";

        con_header.Close();
        con_header.Open();
        query_header = "select collname,address3,pincode from collinfo where college_code=" + Session["collegecode"] + "";
        SqlCommand com_header = new SqlCommand(query_header, con_header);
        SqlDataReader sdr_header;
        sdr_header = com_header.ExecuteReader();
        while (sdr_header.Read())
        {

            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 2);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;



            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, sprdviewrcrd.Sheets[0].ColumnCount - 3);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 2].Text = sdr_header.GetString(0) + ".";
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;



            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, sprdviewrcrd.Sheets[0].ColumnCount - 3);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, 2].Text = sdr_header.GetString(1) + " - " + sdr_header.GetString(2) + ".";
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;

            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, sprdviewrcrd.Sheets[0].ColumnCount - 3);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, 2].Text = "";
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;

            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, sprdviewrcrd.Sheets[0].ColumnCount - 3);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, 2].Text = "Students Arrear Status";
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, 2].HorizontalAlign = HorizontalAlign.Center;
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;

            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, sprdviewrcrd.Sheets[0].ColumnCount - 3);//5th row span
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[4, 2].Text = "";
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[4, 2].ForeColor = Color.White;



            sprdviewrcrd.Sheets[0].ColumnHeaderSpanModel.Add(0, sprdviewrcrd.Sheets[0].ColumnCount - 1, 5, 1);
            sprdviewrcrd.Sheets[0].ColumnHeader.Cells[0, sprdviewrcrd.Sheets[0].ColumnCount - 1].CellType = mi2;


        }
    }
}

