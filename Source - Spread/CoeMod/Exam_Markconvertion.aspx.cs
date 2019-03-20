using System;//--------------modified on 3/7/12(convert decimal to string for tot val),11/7/12(makr coversion table change)
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using FarPoint.Web.Spread;


public partial class Exam_Markconvertion : System.Web.UI.Page
{

    string CollegeCode;
    SqlCommand cmd;
    Hashtable hat = new Hashtable();
    Hashtable arrcount = new Hashtable();
    string Master1 = "";
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    DataSet ds_load = new DataSet();
    DAccess2 daccess2 = new DAccess2();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con3 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection getsql = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    [Serializable()]
    public class MyImg : ImageCellType
    {

        //public override Control paintcell(string id, System.Web.UI.WebControls.TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object value, Boolean upperLevel)
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
            img.Width = Unit.Percentage(80);
            img.Height = Unit.Percentage(90);
            return img;


        }
    }
    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {

        Control cntUpdateBtn = FpSpread1.FindControl("Update");
        Control cntCancelBtn = FpSpread1.FindControl("Cancel");
        Control cntCopyBtn = FpSpread1.FindControl("Copy");
        Control cntCutBtn = FpSpread1.FindControl("Clear");
        Control cntPasteBtn = FpSpread1.FindControl("Paste");
        Control cntPagePrintBtn = FpSpread1.FindControl("Print");

        if ((cntUpdateBtn != null))
        {

            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;

            tr.Cells.Remove(tc);

            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);


            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);

            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);


        }

        base.Render(writer);
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
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                con3.Close();
                con3.Open();
                SqlDataReader mtrdr;
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                SqlCommand mtcmd = new SqlCommand(Master1, con3);
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
                bindbatch();
                binddegree();
                bindbranch();

                bindsem();
                bindsec();
                bindmonthyear();
                lblerror.Visible = false;
                FpSpread1.Visible = false;
                FpSpread1.CommandBar.Visible = true;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].SheetCorner.RowCount = 3;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FpSpread1.Sheets[0].Columns[0].Locked = true;
                FpSpread1.Sheets[0].Columns[1].Locked = true;
                FpSpread1.Sheets[0].Columns[2].Locked = true;
                FpSpread1.Sheets[0].Columns[3].Locked = true;
                FpSpread1.Sheets[0].Columns[0].Width = 80;
                FpSpread1.Sheets[0].Columns[1].Width = 100;
                FpSpread1.Sheets[0].Columns[2].Width = 150;
                FpSpread1.Sheets[0].Columns[3].Width = 100;
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);
                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 3, 1);
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Sno";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Reg No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Stud Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindmonthyear()
    {
        if (ddlbatch.SelectedValue.ToString() != "" && ddlbranch.SelectedValue.ToString() != "" && ddlsem.SelectedValue.ToString() != "")
        {
            ddlMonth.Items.Clear();
            ddlYear.Items.Clear();
            string bindmonthyearquery = "select exam_month,exam_year from exam_details where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlsem.SelectedValue.ToString() + " order by exam_month asc";
            SqlDataAdapter damonthyearquery = new SqlDataAdapter(bindmonthyearquery, con1);
            DataSet dsmonthyearquery = new DataSet();
            con1.Close();
            con1.Open();
            damonthyearquery.Fill(dsmonthyearquery);
            if (dsmonthyearquery.Tables[0].Rows.Count > 0)
            {
                string exammonth = "";
                string examyear = "";
                for (int bind = 0; bind < dsmonthyearquery.Tables[0].Rows.Count; bind++)
                {
                    exammonth = dsmonthyearquery.Tables[0].Rows[bind]["exam_month"].ToString();
                    examyear = dsmonthyearquery.Tables[0].Rows[bind]["exam_year"].ToString();
                    if (exammonth == "1")
                    {
                        exammonth = "Jan";
                    }
                    else if (exammonth == "2")
                    {
                        exammonth = "Feb";
                    }
                    else if (exammonth == "3")
                    {
                        exammonth = "Mar";
                    }
                    else if (exammonth == "4")
                    {
                        exammonth = "Apr";
                    }
                    else if (exammonth == "5")
                    {
                        exammonth = "May";
                    }
                    else if (exammonth == "6")
                    {
                        exammonth = "Jun";
                    }
                    else if (exammonth == "7")
                    {
                        exammonth = "Jul";
                    }
                    else if (exammonth == "8")
                    {
                        exammonth = "Aug";
                    }
                    else if (exammonth == "9")
                    {
                        exammonth = "Sep";
                    }
                    else if (exammonth == "10")
                    {
                        exammonth = "Oct";
                    }
                    else if (exammonth == "11")
                    {
                        exammonth = "Nov";
                    }
                    else if (exammonth == "12")
                    {
                        exammonth = "Dec";
                    }

                    ddlMonth.Items.Insert(bind, new System.Web.UI.WebControls.ListItem("" + exammonth + "  ", "" + dsmonthyearquery.Tables[0].Rows[bind]["exam_month"].ToString() + ""));
                    ddlYear.Items.Insert(bind, new System.Web.UI.WebControls.ListItem("" + dsmonthyearquery.Tables[0].Rows[bind]["exam_year"].ToString() + "  ", "" + dsmonthyearquery.Tables[0].Rows[bind]["exam_year"].ToString() + ""));
                }
            }
        }
    }
    public void MonthandYear()
    {

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


        int year;
        //year = Convert.ToInt16(DateTime.Today.Year);
        year = 2012;
        ddlYear.Items.Clear();
        for (int l = 0; l <= 20; l++)
        {

            ddlYear.Items.Add(Convert.ToString(year - l));

        }
        ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
    }
    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;



    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;

    }
    public void bindbatch()
    {
        ddlbatch.Items.Clear();
        ds_load = daccess2.select_method_wo_parameter("bind_batch", "sp");
        int count = ds_load.Tables[0].Rows.Count;
        if (count > 0)
        {
            //ddlbatch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("All", "0"));
            int i1 = 0;
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

        }
        else
        {
            ddlsec.Enabled = false;

        }
    }
    protected void ddldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMonth.Items.Clear();
        ddlYear.Items.Clear();
        bindbranch();
        bindsem();
        bindsec();
        bindmonthyear();
    }
    protected void ddlbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMonth.Items.Clear();
        ddlYear.Items.Clear();
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;
        bindsem();
        bindsec();
        bindmonthyear();
    }
    protected void ddlsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMonth.Items.Clear();
        ddlYear.Items.Clear();
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;
        bindsec();
        bindmonthyear();

    }
    protected void btnconvert_Click(object sender, EventArgs e)
    {

        string attmp = GetFunction("select top 1 isnull(attempts,'') as attempts from coe_attmaxmark where collegecode=" + Session["collegecode"].ToString() + "");
        int attmpt = 0;
        if (Convert.ToString(attmp).Trim() != "")
        {
            attmpt = Convert.ToInt16(attmp);
        }
        FpSpread1.Visible = false;
        string usercode = Session["usercode"].ToString();
        if (ddlMonth.SelectedValue.ToString() != "" && ddlYear.SelectedValue.ToString() != "")
        {
            if (ddlexamtype.SelectedItem.Text != "")
            {
                if (usercode != "" && usercode != "0")
                {
                    int flag = 0;
                    FpSpread1.Sheets[0].RowCount = 0;
                    FpSpread1.Visible = false;

                    int convertinter = 0;
                    int converexter = 0;
                    string totsub = "";
                    string subjecttype = "";
                    FpSpread1.Sheets[0].RowCount = 0;
                    string sections = "";
                    if (ddlexamtype.SelectedItem.Text == "Theory")
                    {
                        subjecttype = "0";
                        string getconvert = "select * from COE_Master_Settings where settings in('convert_th_internal','convert_th_external')";
                        SqlDataAdapter dacollectquery = new SqlDataAdapter(getconvert, con1);
                        DataSet dscollectquery = new DataSet();
                        con1.Close();
                        con1.Open();
                        dacollectquery.Fill(dscollectquery);
                        if (dscollectquery.Tables[0].Rows.Count > 0)
                        {
                            flag = 1;
                            convertinter = Convert.ToInt32(dscollectquery.Tables[0].Rows[0]["value"]);
                            converexter = Convert.ToInt32(dscollectquery.Tables[0].Rows[1]["value"]);
                        }
                    }
                    else if (ddlexamtype.SelectedItem.Text == "Practical")
                    {
                        subjecttype = "1";
                        string getconvert = "select * from COE_Master_Settings where settings in('convert_prac_internal','convert_prac_external')";
                        SqlDataAdapter dacollectquery = new SqlDataAdapter(getconvert, con1);
                        DataSet dscollectquery = new DataSet();
                        con1.Close();
                        con1.Open();
                        dacollectquery.Fill(dscollectquery);
                        if (dscollectquery.Tables[0].Rows.Count > 0)
                        {
                            flag = 1;
                            convertinter = Convert.ToInt32(dscollectquery.Tables[0].Rows[0]["value"]);
                            converexter = Convert.ToInt32(dscollectquery.Tables[0].Rows[1]["value"]);

                        }
                    }
                    if (ddlsec.Enabled == true)
                    {
                        sections = ddlsec.SelectedItem.Text;
                    }
                    else
                    {
                        sections = "";
                    }
                    if (flag == 1)
                    {
                        FpSpread1.Sheets[0].ColumnCount = 4;
                        FpSpread1.Sheets[0].RowCount = 0;
                        string examcodequery = "select exam_code,batch_year from exam_details where exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedValue.ToString() + " and degree_code=" + ddlbranch.SelectedValue.ToString() + " and current_semester=" + ddlsem.SelectedValue.ToString() + "";
                        SqlDataAdapter daexamcodequery = new SqlDataAdapter(examcodequery, con1);
                        DataSet dsexamcodequery = new DataSet();
                        con1.Close();
                        con1.Open();
                        daexamcodequery.Fill(dsexamcodequery);
                        if (dsexamcodequery.Tables[0].Rows.Count > 0)
                        {
                            lblerror.Visible = false;
                            string examcode = "";
                            string batchyear = "";
                            for (int excode = 0; excode < dsexamcodequery.Tables[0].Rows.Count; excode++)
                            {
                                examcode = dsexamcodequery.Tables[0].Rows[excode]["exam_code"].ToString();
                                batchyear = dsexamcodequery.Tables[0].Rows[excode]["batch_year"].ToString();
                                SqlCommand getsubjectcmd = new SqlCommand("exammarkconvertion", con);
                                getsubjectcmd.CommandType = CommandType.StoredProcedure;
                                getsubjectcmd.Parameters.AddWithValue("@batchyear", Convert.ToInt32(batchyear));
                                getsubjectcmd.Parameters.AddWithValue("@degreecode", Convert.ToInt32(ddlbranch.SelectedValue.ToString()));
                                getsubjectcmd.Parameters.AddWithValue("@semester", Convert.ToInt32(ddlsem.SelectedValue.ToString()));
                                getsubjectcmd.Parameters.AddWithValue("@examcode", examcode);
                                getsubjectcmd.Parameters.AddWithValue("@subjecttype", subjecttype);
                                getsubjectcmd.Parameters.AddWithValue("@sections", sections);
                                SqlDataAdapter getsubjectcmdda = new SqlDataAdapter(getsubjectcmd);
                                DataSet getsubjectcmdds = new DataSet();
                                getsubjectcmdda.Fill(getsubjectcmdds);
                                if (getsubjectcmdds.Tables[1].Rows.Count > 0)
                                {

                                    for (int subj = 0; subj < getsubjectcmdds.Tables[1].Rows.Count; subj++)
                                    {
                                        string subjectno = getsubjectcmdds.Tables[1].Rows[subj]["subject_no"].ToString();
                                        string subjectname = getsubjectcmdds.Tables[1].Rows[subj]["subject_name"].ToString();
                                        if (totsub == "")
                                        {
                                            totsub = subjectno;
                                        }
                                        else
                                        {
                                            totsub = totsub + "," + subjectno;
                                        }
                                        FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + 4;
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 4, 1, 4);
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 4, 1, 2);
                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 2, 1, 2);
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Text = subjectname;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 4].Tag = subjectno;
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 4].Text = "Before Convertion";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 2].Text = "After Convertion";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 4].Text = "I";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].Text = "E";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 2].Text = "CI";
                                        FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = "CE";
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Width = 40;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 2].Width = 40;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 3].Width = 40;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 4].Width = 40;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 1].Locked = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 2].Locked = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 3].Locked = true;
                                        FpSpread1.Sheets[0].ColumnHeader.Columns[FpSpread1.Sheets[0].ColumnCount - 4].Locked = true;
                                    }
                                }
                                if (totsub != "")
                                {
                                    if (getsubjectcmdds.Tables[0].Rows.Count > 0)
                                    {
                                        int sno = 0;
                                        string roll_no = "";
                                        string regno = "";
                                        string studname = "";
                                        for (int rolno = 0; rolno < getsubjectcmdds.Tables[0].Rows.Count; rolno++)
                                        {
                                            sno++;
                                            roll_no = getsubjectcmdds.Tables[0].Rows[rolno]["roll_no"].ToString();
                                            regno = getsubjectcmdds.Tables[0].Rows[rolno]["reg_no"].ToString();
                                            studname = getsubjectcmdds.Tables[0].Rows[rolno]["stud_name"].ToString();
                                            if (roll_no != "")
                                            {
                                                FpSpread1.Visible = true;
                                                FpSpread1.Sheets[0].RowCount = FpSpread1.Sheets[0].RowCount + 1;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = regno;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = studname;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = roll_no;
                                                //Modified by srinath 6/6/2014
                                                //string collectquery = "select roll_no,actual_internal_mark,actual_external_mark,mark_entry.subject_no,s.subject_name,s.max_int_marks,s.max_ext_marks,s.min_ext_marks,s.min_int_marks,s.mintotal,mark_entry.attempts from mark_entry,subject s where s.subject_no=mark_entry.subject_no and mark_entry.subject_no in(" + totsub + ") and roll_no='" + roll_no + "' and exam_code=" + examcode + " order by mark_entry.subject_no asc";
                                                string collectquery = "select roll_no,actual_internal_mark,actual_external_mark,mark_entry.subject_no,s.subject_name,s.max_int_marks,s.max_ext_marks,s.min_ext_marks,s.min_int_marks,s.mintotal,mark_entry.attempts,mark_entry.internal_mark,mark_entry.external_mark from mark_entry,subject s where s.subject_no=mark_entry.subject_no and mark_entry.subject_no in(" + totsub + ") and roll_no='" + roll_no + "' and exam_code=" + examcode + " order by mark_entry.subject_no asc";
                                                SqlDataAdapter dacollectquery = new SqlDataAdapter(collectquery, con1);
                                                DataSet dscollectquery = new DataSet();
                                                con1.Close();
                                                con1.Open();
                                                dacollectquery.Fill(dscollectquery);
                                                if (dscollectquery.Tables[0].Rows.Count > 0)
                                                {

                                                    string subjectno = "";
                                                    string rollno = "";
                                                    string internalmark = "0";
                                                    string externalmark = "0";

                                                    //====added by gowtham=======
                                                    string actinternalmark = "0";
                                                    string actexternalmark = "0";
                                                    string acttotal = "0";
                                                    //=========end=====================

                                                    string submaxint = "0";
                                                    string submaxext = "0";
                                                    string subname = "";

                                                    for (int getmark = 0; getmark < dscollectquery.Tables[0].Rows.Count; getmark++)
                                                    {
                                                        //Aruna======================================================
                                                        int actualattmpt = Convert.ToInt16(dscollectquery.Tables[0].Rows[getmark]["attempts"]);
                                                        if ((attmpt != 0) && (actualattmpt >= attmpt))
                                                        {
                                                            goto L1;
                                                        }
                                                        //===========================================================
                                                        int colcount = 3;
                                                        for (int colcounttag = 3; colcounttag < FpSpread1.Sheets[0].ColumnCount - 1; colcounttag++)
                                                        {
                                                            string getsubjectno = Convert.ToString(FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcounttag + 1].Tag);
                                                            subjectno = dscollectquery.Tables[0].Rows[getmark]["subject_no"].ToString();
                                                            if (getsubjectno == subjectno)
                                                            {
                                                                rollno = dscollectquery.Tables[0].Rows[getmark]["roll_no"].ToString();
                                                                internalmark = dscollectquery.Tables[0].Rows[getmark]["actual_internal_mark"].ToString();
                                                                externalmark = dscollectquery.Tables[0].Rows[getmark]["actual_external_mark"].ToString();
                                                                actinternalmark = dscollectquery.Tables[0].Rows[getmark]["internal_mark"].ToString();
                                                                actexternalmark = dscollectquery.Tables[0].Rows[getmark]["external_mark"].ToString();
                                                                submaxint = dscollectquery.Tables[0].Rows[getmark]["max_int_marks"].ToString();
                                                                submaxext = dscollectquery.Tables[0].Rows[getmark]["max_ext_marks"].ToString();
                                                                subname = dscollectquery.Tables[0].Rows[getmark]["subject_name"].ToString();
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 1].Text = internalmark;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 2].Text = externalmark;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 1].HorizontalAlign = HorizontalAlign.Center;
                                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                if (convertinter > 0)
                                                                {
                                                                    //Internal-----------------------
                                                                    decimal afterconvertint = 0;
                                                                    string afterconvertint1 = "0";
                                                                    if (internalmark != "0" && internalmark.Trim().ToString() != "" && convertinter != 0)
                                                                    {
                                                                        afterconvertint = Convert.ToDecimal(internalmark) / Convert.ToDecimal(100);
                                                                        afterconvertint = afterconvertint * convertinter;
                                                                        afterconvertint = Math.Round(afterconvertint, 2);
                                                                        afterconvertint1 = Convert.ToString(afterconvertint);
                                                                    }
                                                                    //------------------------------

                                                                    //=================added by gowtham=====================//

                                                                    decimal convertint = 0;
                                                                    string convertint1 = "0";
                                                                    if (actinternalmark != "0" && actinternalmark.Trim().ToString() != "" && convertinter != 0)
                                                                    {
                                                                        convertint = Convert.ToDecimal(actinternalmark) / Convert.ToDecimal(100);
                                                                        convertint = convertint * convertinter;
                                                                        convertint = Math.Round(convertint, 2);
                                                                        convertint1 = Convert.ToString(convertint);
                                                                    }

                                                                    //=========================end==============================//

                                                                    //External-----------------------
                                                                    decimal aftercovertext = 0;
                                                                    string aftercovertext1 = "0";
                                                                    if (externalmark != "0" && externalmark.Trim().ToString() != "" && converexter != 0)
                                                                    {
                                                                        aftercovertext = Convert.ToDecimal(externalmark) / Convert.ToDecimal(100);
                                                                        aftercovertext = aftercovertext * converexter;
                                                                        aftercovertext = Math.Round(aftercovertext, 2);
                                                                        aftercovertext1 = Convert.ToString(aftercovertext);
                                                                    }
                                                                    //-------------------------------


                                                                    //============================added by gowtham=========================

                                                                    decimal covertext = 0;
                                                                    string covertext1 = "0";
                                                                    if (actexternalmark != "0" && actexternalmark.Trim().ToString() != "" && converexter != 0)
                                                                    {
                                                                        covertext = Convert.ToDecimal(actexternalmark) / Convert.ToDecimal(100);
                                                                        covertext = covertext * converexter;
                                                                        covertext = Math.Round(covertext, 2);
                                                                        covertext1 = Convert.ToString(covertext);
                                                                    }

                                                                    //=========================end========================================

                                                                    //Total--------------------------
                                                                    decimal total = (Convert.ToDecimal(afterconvertint1) + Convert.ToDecimal(aftercovertext1));
                                                                    //  decimal total1 = Math.Round(Convert.ToDecimal(total));
                                                                    decimal total1_deci = (Math.Round(total, 2));
                                                                    string total1 = total1_deci.ToString();
                                                                    //-------------------------------

                                                                    //=====================added by gowtham===========================


                                                                    decimal alltotal = (Convert.ToDecimal(convertint1) + Convert.ToDecimal(covertext1));
                                                                    //  decimal total1 = Math.Round(Convert.ToDecimal(total));
                                                                    decimal total_deci = (Math.Round(alltotal, 2));
                                                                    string total2 = total_deci.ToString();


                                                                    //===================end=========================================

                                                                    //Result-------------------------
                                                                    string resultstatus = "Fail";
                                                                    int passorfail = 0;
                                                                    if ((Convert.ToDouble(afterconvertint1) >= Convert.ToDouble(dscollectquery.Tables[0].Rows[getmark]["min_int_marks"])) && (Convert.ToDouble(aftercovertext1) >= Convert.ToDouble(dscollectquery.Tables[0].Rows[getmark]["min_ext_marks"])) && (Convert.ToDouble(total1) >= Convert.ToDouble(dscollectquery.Tables[0].Rows[getmark]["mintotal"])))
                                                                    {
                                                                        resultstatus = "Pass";
                                                                        passorfail = 1;
                                                                    }
                                                                    //-----------------------------

                                                                    string updatequery = "update mark_entry set actual_internal_mark=" + convertint1 + ",actual_external_mark=" + covertext1 + ", actual_total=" + total2 + ", internal_mark=" + afterconvertint1 + ",external_mark=" + aftercovertext1 + ", total=" + total1 + ",result='" + resultstatus + "',passorfail='" + passorfail + "' where roll_no='" + rollno + "' and subject_no=" + subjectno + " and exam_code=" + examcode + "";
                                                                    SqlCommand createdummycmd = new SqlCommand(updatequery, con1);
                                                                    con1.Close();
                                                                    con1.Open();
                                                                    createdummycmd.ExecuteNonQuery();
                                                                    string updatequery1 = "update camarks set total=" + afterconvertint1 + " where roll_no='" + rollno + "' and subject_no=" + subjectno + " and exam_code=" + examcode + "";
                                                                    SqlCommand createdummycmd1 = new SqlCommand(updatequery1, con1);
                                                                    con1.Close();
                                                                    con1.Open();
                                                                    createdummycmd1.ExecuteNonQuery();
                                                                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Converted Successfully')", true);
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 3].Text = afterconvertint1;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 4].Text = aftercovertext1;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 3].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcount + 4].HorizontalAlign = HorizontalAlign.Center;
                                                                }
                                                                //

                                                                colcounttag = FpSpread1.Sheets[0].ColumnCount + 1;
                                                            }
                                                            else
                                                            {
                                                                colcounttag = colcounttag + 3;
                                                                colcount = colcount + 4;
                                                            }

                                                        }
                                                    L1: actualattmpt = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                lblerror.Visible = true;
                                                lblerror.Text = "Students Not Found";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        lblerror.Visible = true;
                                        lblerror.Text = "Students Not Found";
                                    }
                                }
                                else
                                {
                                    lblerror.Visible = true;
                                    lblerror.Text = "Subjects Not Found";
                                }
                                if (Session["Rollflag"] == "0")
                                {

                                    FpSpread1.Sheets[0].Columns[3].Visible = false;
                                }
                                if (Session["Regflag"] == "0")
                                {

                                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                                }
                                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                FpSpread1.Width = 400 + getsubjectcmdds.Tables[1].Rows.Count * 160;
                                FpSpread1.Height = 60 + FpSpread1.Sheets[0].RowCount * 25;
                            }
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                            FpSpread1.Visible = false;
                        }
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Fill Convert Mark in Master Wizard and Proceed";
                        FpSpread1.Visible = false;
                    }
                }
                else
                {
                    lblerror.Visible = true;
                    lblerror.Text = "User Code Not Passed";
                    FpSpread1.Visible = false;
                }
            }
            else
            {
                lblerror.Visible = true;
                lblerror.Text = "Select Exam Type";
                FpSpread1.Visible = false;
            }
        }
        else
        {
            lblerror.Visible = true;
            lblerror.Text = "Selct Exam Month and Year";
            FpSpread1.Visible = false;
        }
    }
    protected void ddlexamtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Visible = false;
    }
    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlMonth.Items.Clear();
        ddlYear.Items.Clear();
        bindbranch();
        binddegree();
        bindsem();
        bindsec();
        bindmonthyear();
    }
    protected void ddlsec_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    public string GetFunction(string Att_strqueryst)
    {

        string sqlstr;
        sqlstr = Att_strqueryst;
        getsql.Close();
        getsql.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, getsql);
        SqlDataReader dr_get;
        SqlCommand cmd_get = new SqlCommand(sqlstr);
        cmd_get.Connection = getsql;
        dr_get = cmd_get.ExecuteReader();
        dr_get.Read();

        if (dr_get.HasRows == true)
        {
            return dr_get[0].ToString();
        }
        else
        {
            return "";
        }

    }
}