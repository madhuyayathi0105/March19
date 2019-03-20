using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class certificateissues : System.Web.UI.Page
{
    string usercode = "";
    string strsec1 = string.Empty;
    string singleuser = "";
    string group_user = "";
    string collegecode = "";
    string strbatch = "";
    string strbranch = "";
    string strdegree = "";
    string strsem = "";
    string strsec = "";
    string srisql = "";
    string srisql1 = "";
    string srisql2 = "";
    static int serialcount = 0;

    string stafcode = "";
    int chltypecount = 0;
    Boolean checkissued = false;
    int a = 0;
    int count3 = 0;
    Boolean Cellclick = false;
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
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        collegecode = Session["Collegecode"].ToString();

        if (!IsPostBack)
        {
            ddlstaff.Visible = false;
            txt_search.Visible = false;
            lblsearchby.Visible = false;
            btnissue.Visible = false;

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
            txtissuedate.Text = DateTime.Now.ToString("dd/MM/yyy");
            txtissuedate.Attributes.Add("Readonly", "Readonly");
            txtissueper.Attributes.Add("Readonly", "Readonly");
            lblmessage1.Visible = false;
            load_college();
            bindbatch();
            binddegree(collegecode);


            for (int i = 0; i < chkldegree.Items.Count; i++)
            {
                chkldegree.Items[i].Selected = true;

            }

            for (int i = 0; i < chklbranch.Items.Count; i++)
            {
                chklbranch.Items[i].Selected = true;
            }
            bindsem();
            for (int i = 0; i < chklsem.Items.Count; i++)
            {
                chklsem.Items[i].Selected = true;
            }
            BindSectionDetail();

            chkltype.Items.Add("With Dues");
            chkltype.Items.Add("No Dues");
            chkltype.Items.Add("Issued");
            chkltype.Items.Add("NotIssued");

            ddlissu.Items.Add("Hall Ticket");

            Fpspread1.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            int issx = 11;
            for (int hh = 10; hh <= 60; hh++)
            {


                ddlintimemm.Items.Insert(issx, hh.ToString());
                issx++;

            }


            pnlmsgboxupdate1.Visible = false;




        }
    }
    public void load_college()
    {
        try
        {
            ds.Clear();
            ddlcoollege.Items.Clear();

            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";// modified by Sangeetha on 5/05/2014

            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcoollege.DataSource = ds;
                ddlcoollege.DataTextField = "collname";
                ddlcoollege.DataValueField = "college_code";
                ddlcoollege.DataBind();
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

            chklbatch.Items.Clear();

            hat.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";

            ds = da.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                chklbatch.DataSource = ds;
                chklbatch.DataTextField = "batch_year";
                chklbatch.DataValueField = "batch_year";
                chklbatch.DataBind();
            }

            for (int i = 0; i < chklbatch.Items.Count; i++)
            {
                chklbatch.Items[i].Selected = true;
            }

        }
        catch
        {
        }


    }


    public void binddegree(string collvalue)
    {
        try
        {
            ds.Clear();
            chkldegree.ClearSelection();
            string college = collvalue;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            //string cid = ddldegree.SelectedValue.ToString();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            // hat.Add("course_id", cid);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            string query = "";

            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + college + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + college + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }

            ds = da.select_method_wo_parameter(query, "Text");
            chkldegree.DataSource = ds;
            chkldegree.DataTextField = "course_name";
            chkldegree.DataValueField = "course_id";
            chkldegree.DataBind();
            string colid = "";
            for (int i = 0; i < chkldegree.Items.Count; i++)
            {
                if (chkldegree.Items[i].Selected == true)
                {
                    if (colid == "")
                    {
                        colid = chkldegree.SelectedValue.ToString();
                    }
                    else
                    {
                        colid = colid + "'" + "," + "'" + chkldegree.SelectedValue.ToString();
                    }
                }
            }


            bindbranch(colid);



        }
        catch
        {

        }



    }

    public void bindbranch(string branch)
    {
        try
        {
            chklbranch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = da.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklbranch.DataSource = ds;
                    chklbranch.DataTextField = "dept_name";
                    chklbranch.DataValueField = "degree_code";
                    chklbranch.DataBind();
                }
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
            string brnch = "";
            string btch = "";
            chklsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            ds.Clear();
            for (int k = 0; k < chklbranch.Items.Count; k++)
            {
                if (chklbranch.Items[k].Selected == true)
                {
                    if (brnch == "")
                    {
                        brnch = chklbranch.Items[k].Value;
                    }
                    else
                    {
                        brnch = brnch + "'" + "," + "'" + chklbranch.Items[k].Value;
                    }

                }

            }
            for (int k = 0; k < chklbatch.Items.Count; k++)
            {
                if (chklbatch.Items[k].Selected == true)
                {
                    if (btch == "")
                    {
                        btch = chklbatch.Items[k].Value;
                    }
                    else
                    {
                        btch = btch + "'" + "," + "'" + chklbatch.Items[k].Value;
                    }

                }

            }
            string sqlsem = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code in ('" + brnch + "') and batch_year in ('" + btch + "') and college_code='" + ddlcoollege.SelectedValue.ToString() + "' order by NDurations desc ";
            ds = da.select_method_wo_parameter(sqlsem, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        chklsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        chklsem.Items.Add(i.ToString());
                    }

                }
            }
            else
            {
                ds.Clear();
                string sqlsem1 = "select distinct duration,first_year_nonsemester  from degree where degree_code='" + brnch + "' and college_code='" + ddlcoollege.SelectedValue.ToString() + "'  order by NDurations desc";
                chklsem.Items.Clear();
                ds = da.select_method_wo_parameter(sqlsem1, "Text");


                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            chklsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            chklsem.Items.Add(i.ToString());
                        }
                    }
                }


            }


        }
        catch
        {
        }
    }

    protected void ddlintimehh_SelectedIndexChanged(object sender, EventArgs e)
    {

        if (ddlintimehh.SelectedValue.ToString() == "HH" || ddlintimehh.SelectedValue.ToString() == "")
        {

            lblmessage.Text = "Please Select Valid Start Time";
            lblmessage.Visible = true;

        }
        else
        {
            lblmessage.Visible = false;
            //txtfoc.Focus();


        }
    }
    protected void ddlintimemm_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlintimemm.SelectedValue.ToString() == "MM" || ddlintimemm.SelectedValue.ToString() == "")
        {
            lblmessage.Text = "Please Select Valid Start Time";
            lblmessage.Visible = true;

        }
        else
        {

            lblmessage.Visible = false;
            //txtfoc.Focus();
        }
    }



    protected void tbbatch_TextChanged(object sender, EventArgs e)
    {
        try
        {

        }
        catch
        {
        }
    }
    protected void tbdegree_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string courseid = "";
            chklbranch.Items.Clear();

            string collegecode = ddlcoollege.SelectedValue.ToString();
            string usercode = Session["usercode"].ToString();
            // string course_id = ddldegree.SelectedValue.ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            //  string deg = ddldegree.SelectedItem.ToString();
            for (int k = 0; k < chkldegree.Items.Count; k++)
            {
                if (chkldegree.Items[k].Selected == true)
                {
                    if (courseid == "")
                    {
                        courseid = chkldegree.Items[k].Value;

                    }
                    else
                    {
                        courseid = courseid + "," + chkldegree.Items[k].Value;
                    }
                }
            }


        }
        catch
        {
        }
    }
    protected void tbbranch_TextChanged(object sender, EventArgs e)
    {

        if (!Page.IsPostBack == false)
        {
            chklsem.Items.Clear();
        }
        try
        {
            chklsem.Items.Clear();
            bindsem();


        }
        catch
        {

        }

    }
    protected void tbsem_TextChanged(object sender, EventArgs e)
    {
        try
        {
        }
        catch
        {
        }
    }
    protected void Chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chkbatch.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklbatch.Items)
                {
                    li.Selected = true;
                    tbbatch.Text = "Batch(" + (chklbatch.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklbatch.Items)
                {
                    li.Selected = false;
                    tbbatch.Text = "--Select--";
                }
            }

        }
        catch
        {
        }
    }
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string courseid = "";
            int batchcount = 0;
            string value = "";
            string code = "";
            if (chkdegree.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chkldegree.Items)
                {
                    li.Selected = true;
                    tbdegree.Text = "Degree(" + (chkldegree.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chkldegree.Items)
                {
                    li.Selected = false;
                    tbdegree.Text = "--Select--";

                }
            }




            for (int i = 0; i < chkldegree.Items.Count; i++)
            {
                if (chkldegree.Items[i].Selected == true)
                {
                    if (courseid == "")
                    {
                        courseid = chkldegree.Items[i].Value;

                    }
                    else
                    {
                        courseid = courseid + "'" + "," + "'" + chkldegree.Items[i].Value;
                    }
                    value = chkldegree.Items[i].Text;
                    code = chkldegree.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbdegree.Text = "Degree(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbdegree.Text = "--Select--";

            bindbranch(courseid);



        }
        catch
        {
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkbranch.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklbranch.Items)
                {
                    li.Selected = true;
                    tbbranch.Text = "Branch(" + (chklbranch.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklbranch.Items)
                {
                    li.Selected = false;
                    tbbranch.Text = "--Select--";
                }
            }
            BindSectionDetail();
            bindsem();

        }
        catch
        {
        }
    }
    protected void chksem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chksem.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsem.Items)
                {
                    li.Selected = true;
                    tbsem.Text = "Sem(" + (chklsem.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsem.Items)
                {
                    li.Selected = false;
                    tbsem.Text = "--Select--";
                }
            }

        }
        catch
        {
        }
    }

    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chksec.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsec.Items)
                {
                    li.Selected = true;
                    tbsec.Text = "Sec(" + (chklsec.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsec.Items)
                {
                    li.Selected = false;
                    tbsec.Text = "--Select--";
                }
            }

        }
        catch
        {
        }
    }
    protected void chktype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chktype.Checked == true)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chkltype.Items)
                {
                    li.Selected = true;
                    tbtype.Text = "Type(" + (chkltype.Items.Count) + ")";

                }
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in chkltype.Items)
                {
                    li.Selected = false;
                    tbtype.Text = "--Select--";
                }
            }

        }
        catch
        {
        }
    }
    protected void chklbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklbatch.Items.Count; i++)
            {
                if (chklbatch.Items[i].Selected == true)
                {

                    value = chklbatch.Items[i].Text;
                    code = chklbatch.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbbatch.Text = "Batch(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbbatch.Text = "--Select--";
        }
        catch
        {

        }
    }

    protected void chkldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string courseid = "";
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chkldegree.Items.Count; i++)
            {
                if (chkldegree.Items[i].Selected == true)
                {
                    if (courseid == "")
                    {
                        courseid = chkldegree.Items[i].Value;

                    }
                    else
                    {
                        courseid = courseid + "'" + "," + "'" + chkldegree.Items[i].Value;
                    }
                    value = chkldegree.Items[i].Text;
                    code = chkldegree.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbdegree.Text = "Degree(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbdegree.Text = "--Select--";

            bindbranch(courseid);

        }
        catch
        {

        }
    }

    protected void chklbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;





            for (int i = 0; i < chklbranch.Items.Count; i++)
            {
                if (chklbranch.Items[i].Selected == true)
                {
                    batchcount = batchcount + 1;
                    tbbranch.Text = "Branch(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbbranch.Text = "--Select--";
            BindSectionDetail();
            bindsem();
        }
        catch
        {

        }
    }

    protected void chklsem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklsem.Items.Count; i++)
            {
                if (chklsem.Items[i].Selected == true)
                {

                    value = chklsem.Items[i].Text;
                    code = chklsem.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbsem.Text = "Sem(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbsem.Text = "--Select--";
        }
        catch
        {

        }
    }

    protected void chklsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chklsec.Items.Count; i++)
            {
                if (chklsem.Items[i].Selected == true)
                {

                    value = chklsec.Items[i].Text;
                    code = chklsec.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbsec.Text = "Sec(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbsec.Text = "--Select--";
        }
        catch
        {

        }
    }

    public void BindSectionDetail()
    {
        try
        {
            strbatch = "";
            strbranch = "";

            for (int i = 0; i < chklbatch.Items.Count; i++)
            {
                if (chklbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklbranch.Items.Count; i++)
            {
                if (chklbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            //strbranch = chklstbranch.SelectedValue.ToString();


            chklsec.Items.Clear();
            if (strbranch.ToString() != "" && strbatch.ToString() != "")
            {
                ds.Dispose();
                ds.Reset();
                ds = da.BindSectionDetail(strbatch, strbranch);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsec.Items.Insert(0, " ");
                    chklsec.DataSource = ds;
                    chklsec.DataTextField = "sections";
                    chklsec.DataBind();
                    chklsec.Items.Insert(0, " ");
                    if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                    {
                        chklsec.Enabled = false;
                    }
                    else
                    {
                        chklsec.Enabled = true;
                        chklsec.SelectedIndex = chklsec.Items.Count - 2;
                        chklsec.Items[0].Selected = true;
                        for (int i = 0; i < chklsec.Items.Count; i++)
                        {
                            chklsec.Items[i].Selected = true;
                            if (chklsec.Items[i].Selected == true)
                            {
                                count3 += 1;
                            }
                            if (chklsec.Items.Count == count3)
                            {
                                //chksec.Checked = true;
                            }
                        }
                    }
                }

                else
                {
                    chklsec.Items[0].Selected = true;
                }
            }
        }
        catch
        {

        }

    }

    protected void chkltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";


            for (int i = 0; i < chkltype.Items.Count; i++)
            {
                if (chkltype.Items[i].Selected == true)
                {

                    value = chkltype.Items[i].Text;
                    code = chkltype.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                    tbtype.Text = "Type(" + batchcount.ToString() + ")";
                }

            }

            if (batchcount == 0)
                tbtype.Text = "--Select--";
            return;

        }
        catch
        {

        }
    }
    protected void ddlcoollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string value = ddlcoollege.SelectedValue.ToString();

            binddegree(value);
        }
        catch
        {
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

    protected void btnok_click(object sender, EventArgs e)
    {
        try
        {
            int checkvaluecount = 0;
            txtfoc.Focus();

            string d1 = "", d2 = "", d3 = "", d4 = "", d5 = "", d6 = "", time_sr = "";
            pnlmsgboxupdate1.Visible = false;
            if (ddlintimehh.SelectedItem.Text == "HH" || ddlintimemm.SelectedItem.Text == "MM")
            {
                lblmessage.Text = "Please Select The Correct Date";
                lblmessage.Visible = true;
                btngo.Focus();
                return;
            }

            time_sr = ddlintimehh.SelectedItem.Text + ":" + ddlintimemm.SelectedItem.Text + " " + ddlintimeses.SelectedItem.Text;

            if (txtissueper.Text.Trim() == "")
            {
                lblmessage.Text = "Please Select The Staff";
                lblmessage.Visible = true;
                btngo.Focus();
                return;
            }

            lblmessage.Visible = false;
            // string ss= Fpspread1.Sheets[0].Cells[1, 1].Value.ToString();
            for (int j = 1; j < Fpspread1.Sheets[0].RowCount; j++)
            {

                if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[j, 1].Value) == 1)
                {
                    checkvaluecount++;
                    Fpspread1.Sheets[0].Cells[j, 5].Text = ddlissu.SelectedItem.ToString();
                    Fpspread1.Sheets[0].Cells[j, 6].Text = txtissuedate.Text.ToString();
                    Fpspread1.Sheets[0].Cells[j, 7].Text = txtissueper.Text.ToString();
                    Fpspread1.Sheets[0].Cells[j, 7].Tag = txtstaff_co.Text;
                    d1 = txtstaff_co.Text;
                    d2 = Fpspread1.Sheets[0].Cells[j, 3].Tag.ToString();
                    d3 = ddlissu.SelectedItem.ToString();
                    d4 = txtissuedate.Text.ToString();
                    string[] ddate = d4.Split('/');
                    d4 = ddate[1] + "/" + ddate[0] + "/" + ddate[2];

                    d5 = time_sr;
                    d6 = "Issued";
                    binddata(d1, d2, d3, d4, d5, d6);

                }


            }
            if (a != 0)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);

            }
            if (checkvaluecount == 0)
            {
                lblmessage.Text = "Please Select Atleast One Student";
                lblmessage.Visible = true;
                btngo.Focus();
                return;
            }


        }
        catch
        {
        }
    }

    public void binddata(string dd1, string dd2, string dd3, string dd4, string dd5, string dd6)
    {
        //update cert_issue set Roll_No='',typeofcert='',issue_date='',issue_time='',info='' where staff_code=''
        srisql = "if exists(select * from cert_issue where  Roll_No='" + dd2 + "' and typeofcert='" + dd3 + "') begin update cert_issue set Roll_No='" + dd2 + "',typeofcert='" + dd3 + "',issue_date='" + dd4 + "',issue_time='" + dd5 + "',info='" + dd6 + "',staff_code='" + dd1 + "' where   Roll_No='" + dd2 + "'  end  else begin insert into cert_issue values('" + dd1 + "','" + dd2 + "','" + dd3 + "','" + dd4 + "','" + dd5 + "','" + dd6 + "') end";

        hat.Clear();
        a = da.insert_method(srisql, hat, "Text");




    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            Fpspread1.Visible = false;

            lblmessage.Visible = true;
            int checkcount_batch = 0;
            int checkcount_deg = 0;
            int checkcount_bran = 0;
            int checkcount_sem = 0;
            int checkcount_sec = 0;
            for (int i = 0; i < chklbatch.Items.Count; i++)
            {
                if (chklbatch.Items[i].Selected == true)
                {
                    checkcount_batch++;
                    if (strbatch == "")
                    {
                        strbatch = chklbatch.Items[i].Value.ToString();
                    }
                    else
                    {
                        strbatch = strbatch + "'" + "," + "'" + chklbatch.Items[i].Value.ToString();
                    }

                }


            }
            if (checkcount_batch == 0)
            {
                lblmessage.Text = "Please Select Any Batch";
                return;

            }


            for (int i = 0; i < chkldegree.Items.Count; i++)
            {
                if (chkldegree.Items[i].Selected == true)
                {
                    if (strdegree == "")
                    {
                        checkcount_deg++;
                        strdegree = chkldegree.Items[i].Value.ToString();

                    }
                    else
                    {
                        strdegree = strdegree + "'" + "," + "'" + chkldegree.Items[i].Value.ToString();
                    }
                }


            }
            if (checkcount_deg == 0)
            {
                lblmessage.Text = "Please Select Any Degree";
                return;

            }


            for (int i = 0; i < chklbranch.Items.Count; i++)
            {
                if (chklbranch.Items[i].Selected == true)
                {
                    checkcount_bran++;
                    if (strbranch == "")
                    {

                        strbranch = chklbranch.Items[i].Value.ToString();


                    }
                    else
                    {

                        strbranch = strbranch + "'" + "," + "'" + chklbranch.Items[i].Value.ToString();

                    }

                }
            }
            if (checkcount_bran == 0)
            {
                lblmessage.Text = "Please Select Any Branch";
                return;

            }

            for (int i = 0; i < chklsem.Items.Count; i++)
            {
                if (chklsem.Items[i].Selected == true)
                {
                    checkcount_sem++;
                    if (strsem == "")
                    {

                        strsem = chklsem.Items[i].Value.ToString();


                    }
                    else
                    {

                        strsem = strsem + "'" + "," + "'" + chklsem.Items[i].Value.ToString();

                    }

                }
            }
            if (checkcount_sem == 0)
            {
                lblmessage.Text = "Please Select Any Semester";
                return;

            }

            for (int i = 0; i < chklsec.Items.Count; i++)
            {
                if (chklsec.Items[i].Selected == true)
                {
                    checkcount_sec++;
                    if (strsec == "")
                    {

                        strsec = chklsec.Items[i].Value.ToString();


                    }
                    else
                    {

                        strsec = strsec + "'" + "," + "'" + chklsec.Items[i].Value.ToString();

                    }

                }
            }
            if (chklsec.Items.Count == 0)
            {

            }
            else
            {
                if (checkcount_sec == 0)
                {
                    lblmessage.Text = "Please Select Any Section";

                    return;

                }
            }






            bindspread();
        }
        catch
        {
        }
    }
    protected void Fpspread_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
        }
        catch
        {
        }
    }
    protected void Fpspread_PreRender(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {
            }
            catch
            {
            }
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {

    }
    protected void btnxl_Click(object sender, EventArgs e)
    {

    }
    protected void btnissue_Click(object sender, EventArgs e)
    {
        Fpspread1.SaveChanges();
        //mpemsgboxupdate.Show();
        int count45 = 0;
        for (int j = 1; j < Fpspread1.Sheets[0].RowCount; j++)
        {

            if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[j, 1].Value) == 1)
            {
                count45++;
            }
        }
        if (count45 == 0)
        {
            lblmessage.Visible = true;
            lblmessage.Text = "Please Select Any Student";
            return;
        }

        lblmessage.Visible = false;
        txtissueper.Text = "";
        //txtfoc.Focus();
        pnlmsgboxupdate1.Visible = true;

    }


    protected void fsstaff_CellClick(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        string activerow = fsstaff.ActiveSheetView.ActiveRow.ToString();
        string activecol = fsstaff.ActiveSheetView.ActiveColumn.ToString();
        Cellclick = true;
        // mpedirect.Show();
        panel8.Visible = true;

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
        //txtfoc.Focus();


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
            txtissueper.Text = name_active.ToString();

            txtstaff_co.Text = fsstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
        }
        panel8.Visible = false;

        pnlmsgboxupdate1.Visible = true;
        //txtfoc.Focus();

    }
    protected void exitpop_Click(object sender, EventArgs e)
    {
        panel8.Visible = false;
        pnlmsgboxupdate1.Visible = true;
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

        txtfoc.Focus();
        pnlmsgboxupdate1.Visible = false;


    }

    protected void Fpspread1_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value) == 1)
        {
            for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
            {
                Fpspread1.Sheets[0].Cells[i, 1].Value = 1;
                //btncheckadd.Focus();

                //FpSpreadcheck.SaveChanges();


            }

        }

        else if (Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value) == 0)
        {
            for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
            {
                Fpspread1.Sheets[0].Cells[i, 1].Value = 0;
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


            string activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            if (Convert.ToInt32(activerow.ToString()) >= 0)
            {
            }



        }
    }



    public void bindspread()
    {
        try
        {

            lblmessage.Visible = false;
            btnissue.Visible = true;
            Fpspread1.Visible = true;
            Fpspread1.Sheets[0].Visible = true;
            Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread1.Sheets[0].RowCount = 0;

            // Fpspread1.Sheets[0].SheetCorner.Rows[0].Visible = false;
            Fpspread1.Sheets[0].RowHeader.Visible = false;
            Fpspread1.Sheets[0].Columns.Count = 8;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;


            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch-Degree-Department-Semester";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Type of Certificate";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Issue Date";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;

            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Issue Person";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;

            Fpspread1.Sheets[0].Columns[0].Width = 50;
            Fpspread1.Sheets[0].Columns[1].Width = 50;
            Fpspread1.Sheets[0].Columns[2].Width = 90;
            Fpspread1.Sheets[0].Columns[3].Width = 200;
            Fpspread1.Sheets[0].Columns[4].Width = 150;
            Fpspread1.Sheets[0].Columns[5].Width = 150;
            Fpspread1.Sheets[0].Columns[6].Width = 80;
            Fpspread1.Sheets[0].Columns[7].Width = 130;
            Fpspread1.Width = 960;
            Fpspread1.Sheets[0].GridLineColor = Color.Black;


            Fpspread1.Sheets[0].Columns[0].Locked = true;
            for (int k = 2; k < 8; k++)
            {
                Fpspread1.Sheets[0].Columns[k].Locked = true;
            }
            for (int k = 0; k < chkltype.Items.Count; k++)
            {
                if (chkltype.Items[k].Selected == true)
                {
                    chltypecount++;

                }
            }
            for (int ix = 0; ix < 8; ix++)
            {
                Fpspread1.Sheets[0].Columns[ix].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].Columns[ix].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[ix].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[ix].Font.Bold = true;
            }


            //if (chkltype.Items[2].Selected == true)
            //{
            //    checkissued = true;

            //}

            ArrayList addarray = new ArrayList();
            for (int i = 0; i < chkltype.Items.Count; i++)
            {
                if (chkltype.Items[i].Selected == true)
                {
                    addarray.Add(chkltype.Items[i].Text);
                }
            }

            bind_1sthalfspread();

            if (Fpspread1.Sheets[0].RowCount == 0)
            {
                Fpspread1.Visible = false;
                lblmessage.Visible = true;
                lblmessage.Text = "No Records Found";
                btnissue.Visible = false;

            }

            if (chltypecount == 0 || chltypecount == 4)
            {


                chltypecount = 1;
                bind_data_03();
                return;
            }






            //if (addarray.Count > 0)
            //{
            //    for (int k = 0; k < addarray.Count; k++)
            //    {
            if (chkltype.Items[0].Selected == true)
            {
                if (chltypecount == 1 || chltypecount == 3)
                {
                    if (chkltype.Items[1].Selected == false)
                    {
                        chltypecount = 1;
                        bind_data_01();

                        if (serialcount < 1)
                        {
                            Fpspread1.Visible = false;
                            lblmessage.Visible = true;
                            lblmessage.Text = "No Records Found";
                            btnissue.Visible = false;
                            return;
                        }

                    }
                    else if (chltypecount == 3)
                    {
                        if (chkltype.Items[3].Selected == false)
                        {
                            checkissued = true;
                            bind_data_03();
                            if (serialcount < 1)
                            {
                                Fpspread1.Visible = false;
                                lblmessage.Visible = true;
                                lblmessage.Text = "No Records Found";
                                btnissue.Visible = false;
                                return;
                            }

                        }
                    }
                }
                else if (chltypecount == 2 && chkltype.Items[1].Selected == true)
                {
                    checkissued = true;
                    bind_data_01();
                    if (serialcount < 1)
                    {
                        Fpspread1.Visible = false;
                        lblmessage.Visible = true;
                        lblmessage.Text = "No Records Found";
                        btnissue.Visible = false;
                        return;
                    }

                }
                else if (chltypecount == 2)
                {
                    if (chkltype.Items[2].Selected == true)
                    {
                        checkissued = true;
                        bind_data_03();
                        if (serialcount < 1)
                        {
                            Fpspread1.Visible = false;
                            lblmessage.Visible = true;
                            lblmessage.Text = "No Records Found";
                            btnissue.Visible = false;
                            return;
                        }

                    }
                    else
                    {
                        bind_data_03();
                        if (serialcount < 1)
                        {
                            Fpspread1.Visible = false;
                            lblmessage.Visible = true;
                            lblmessage.Text = "No Records Found";
                            btnissue.Visible = false;
                            return;
                        }

                    }

                }





            }
            else if (chkltype.Items[1].Selected == true)
            {
                if (chltypecount == 1)
                {
                    bind_data_02();
                    if (serialcount < 1)
                    {
                        Fpspread1.Visible = false;
                        lblmessage.Visible = true;
                        lblmessage.Text = "No Records Found";
                        btnissue.Visible = false;
                        return;
                    }

                }
                else if (chltypecount == 2)
                {
                    if (chkltype.Items[2].Selected == true)
                    {
                        checkissued = true;
                        bind_data_02();
                        if (serialcount < 1)
                        {
                            Fpspread1.Visible = false;
                            lblmessage.Visible = true;
                            lblmessage.Text = "No Records Found";
                            btnissue.Visible = false;
                            return;
                        }

                    }
                    else if (chkltype.Items[3].Selected == true)
                    {
                        bind_data_02();
                        if (serialcount < 1)
                        {
                            Fpspread1.Visible = false;
                            lblmessage.Visible = true;
                            lblmessage.Text = "No Records Found";
                            btnissue.Visible = false;
                            return;
                        }
                    }


                }
                else if (chltypecount == 3 && chkltype.Items[2].Selected == true && chkltype.Items[3].Selected)
                {
                    chltypecount = 1;
                    bind_data_02();
                    if (serialcount < 1)
                    {
                        Fpspread1.Visible = false;
                        lblmessage.Visible = true;
                        lblmessage.Text = "No Records Found";
                        btnissue.Visible = false;
                        return;
                    }

                    return;
                }




            }
            else if (chkltype.Items[2].Selected == true)
            {
                if (chltypecount == 1)
                {
                    checkissued = true;

                    chltypecount = 0;
                    bind_data_03();

                    if (serialcount < 1)
                    {
                        Fpspread1.Visible = false;
                        lblmessage.Visible = true;
                        lblmessage.Text = "No Records Found";
                        btnissue.Visible = false;
                        return;
                    }

                }
                else if (chltypecount == 2 && chkltype.Items[3].Selected == true)
                {
                    chltypecount = 1;
                    bind_data_01();
                    if (serialcount < 1)
                    {
                        Fpspread1.Visible = false;
                        lblmessage.Visible = true;
                        lblmessage.Text = "No Records Found";
                        btnissue.Visible = false;
                        return;
                    }
                }

            }
            else if (chkltype.Items[3].Selected == true)
            {
                chltypecount = 0;

                bind_data_03();
                if (serialcount < 1)
                {
                    Fpspread1.Visible = false;
                    lblmessage.Visible = true;
                    lblmessage.Text = "No Records Found";
                    btnissue.Visible = false;
                    return;
                }

            }

            //    }

            //}








            Fpspread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpspread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;


            Fpspread1.SaveChanges();






        }
        catch
        {

        }
    }

    public void bind_data_02()
    {
        //srisql = "select roll_no as rollno, stud_name as studentname,reg_no,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit from registration,Degree,course where registration.degree_code in ('" + strbranch + "') and batch_year in ('" + strbatch + "')  and Degree.Degree_Code=registration.Degree_Code   and current_semester in ('" + strsem + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  ORDER BY  course.Course_Name";

        if (tbrollno.Text.Trim() != "")
        {
            srisql = "select  Convert(nvarchar(50),roll_no)+'-'+Convert(nvarchar(50),stud_name) ,reg_no,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit,roll_no as rollno,Stud_Name as studentname  from registration,Degree,course where registration.degree_code in ('" + strbranch + "') and batch_year in ('" + strbatch + "')  and Degree.Degree_Code=registration.Degree_Code and Roll_No='" + tbrollno.Text + "'   and current_semester in ('" + strsem + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  ORDER BY  course.Course_Name,Registration.Roll_No,degree.Degree_Code";

        }
        else
        {
            srisql = "select  Convert(nvarchar(50),roll_no)+'-'+Convert(nvarchar(50),stud_name) ,reg_no,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit,roll_no as rollno,Stud_Name as studentname  from registration,Degree,course where registration.degree_code in ('" + strbranch + "') and batch_year in ('" + strbatch + "')  and Degree.Degree_Code=registration.Degree_Code and current_semester in ('" + strsem + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  ORDER BY  course.Course_Name,Registration.Roll_No,degree.Degree_Code";
        }
        temp.Clear();
        int sno = 0;
        temp = da.select_method_wo_parameter(srisql, "Text");
        Fpspread1.Sheets[0].RowCount = 0;
        Fpspread1.Sheets[0].RowCount++;
        int bal = 1;
        if (temp.Tables[0].Rows.Count > 0)
        {
            // Fpspread1.Sheets[0].RowCount++;
            for (int g = 0; g < temp.Tables[0].Rows.Count; g++)
            {
                string troll = temp.Tables[0].Rows[g][0].ToString();
                string tradmin = temp.Tables[0].Rows[g][3].ToString();
                srisql1 = "select SUM(total) as fees from fee_allot f,fee_info i,Registration r where r.Roll_Admit  = f.roll_admit and f.fee_code = i.fee_code and f.roll_admit  = '" + tradmin + "'  group by f.roll_admit";
                srisql2 = " select SUM(credit) as fees from dailytransaction d,fee_info fe where name  = '" + troll + "'  and d.fee_code=fe.fee_code and  fe.fee_type<>'Excess Amount' and vouchertype=1   group by name";
                temp1.Clear();
                temp2.Clear();
                temp1 = da.select_method_wo_parameter(srisql1, "Text");
                temp2 = da.select_method_wo_parameter(srisql2, "Text");
                if (temp1.Tables[0].Rows.Count > 0)
                {
                    if (temp2.Tables[0].Rows.Count > 0)
                    {
                        int feeamunt = Convert.ToInt32(temp1.Tables[0].Rows[0][0]);
                        int cre = Convert.ToInt32(temp2.Tables[0].Rows[0][0]);
                        bal = feeamunt - cre;
                    }



                }
                else
                {
                    bal = 0;
                }

                if (bal <= 0)
                {

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[0, 1].CellType = cbct1;
                    cbct1.AutoPostBack = true;

                    Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 5);
                    Fpspread1.Sheets[0].Cells[0, 1].Text = "";
                    Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;

                    cbct.AutoPostBack = false;
                    //  Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount- 1 , 0].Text = Convert.ToString(sno);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = cbct;



                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = temp.Tables[0].Rows[g]["rollno"].ToString();

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = temp.Tables[0].Rows[g]["studentname"].ToString();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = temp.Tables[0].Rows[g]["rollno"].ToString();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Note = temp.Tables[0].Rows[g]["Roll_Admit"].ToString();


                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;


                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = temp.Tables[0].Rows[g]["details"].ToString();
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txt;

                    Fpspread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                    Fpspread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                    Fpspread1.Visible = true;





                }



            }
            srisql = " select sm.staff_name,ci.Roll_No,typeofcert,ci.issue_date,ci.info from cert_issue ci,staffmaster sm where ci.staff_code=sm.staff_code";
            ds1.Clear();
            ds1 = da.select_method_wo_parameter(srisql, "Text");
            int checkcount = 0;
            for (int k = 1; k < Fpspread1.Sheets[0].RowCount; k++)
            {
                string rno = Convert.ToString(Fpspread1.Sheets[0].Cells[k, 3].Tag);
                if (rno != "")
                {
                    if (chltypecount != 1)
                    {
                        if (checkissued == false)
                        {
                            sno++;
                            Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                        }
                    }
                    else if (chltypecount == 1)
                    {
                        sno++;
                        Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                    }

                    DataView dv_demand_data = new DataView();
                    ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + rno + "'";
                    dv_demand_data = ds1.Tables[0].DefaultView;
                    int count4 = 0;
                    string infos = "";
                    count4 = dv_demand_data.Count;
                    if (count4 > 0)
                    {

                        if (chltypecount != 1)
                        {
                            if (checkissued == false)
                            {
                                sno--;
                                Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                                Fpspread1.Sheets[0].Rows[k].Visible = false;

                            }
                        }
                        checkcount++;
                        infos = dv_demand_data[0]["info"].ToString();

                        if (infos != "")
                        {
                            Fpspread1.Sheets[0].Cells[k, 5].Text = dv_demand_data[0]["typeofcert"].ToString();
                            string date01 = dv_demand_data[0]["issue_date"].ToString();
                            string[] spdate = date01.Split(' ');
                            string[] sdate1 = spdate[0].Split('/');
                            string xdate = sdate1[1] + "/" + sdate1[0] + "/" + sdate1[2];
                            Fpspread1.Sheets[0].Cells[k, 6].Text = xdate.ToString();
                            Fpspread1.Sheets[0].Cells[k, 7].Text = dv_demand_data[0]["staff_name"].ToString();
                            serialcount++;
                            if (chltypecount != 1)
                            {
                                if (checkissued == true)
                                {
                                    sno++;
                                    Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno);
                                    //Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                                else if (checkissued == false)
                                {
                                    Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                            }
                        }

                    }
                    else if (checkissued == true && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = false;
                    }
                    else if (checkissued == false && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = true;
                    }


                }

            }
            if (checkcount == 0)
            {
                Fpspread1.Visible = false;
                lblmessage.Visible = true;
                lblmessage.Text = "No Records Found";
                btnissue.Visible = false;
                return;
            }
            Fpspread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpspread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            int rowcount = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Sheets[0].PageSize = 25 + (rowcount * 20);
            Fpspread1.SaveChanges();
            //return;
        }
        else
        {
            Fpspread1.Visible = false;
            lblmessage.Visible = true;
            lblmessage.Text = "No Records Found";
            btnissue.Visible = false;
            return;

        }
    }



    public void bind_1sthalfspread()
    {
        if (tbrollno.Text.Trim() != "")
        {
            srisql = "select roll_no as rollno, stud_name as studentname,reg_no,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit from registration,Degree,course where registration.degree_code in ('" + strbranch + "') and batch_year in ('" + strbatch + "')  and Degree.Degree_Code=registration.Degree_Code and Roll_No='" + tbrollno.Text.Trim() + "'   and current_semester in ('" + strsem + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  ORDER BY  course.Course_Name,Registration.Roll_No,degree.Degree_Code";

        }
        else
        {
            srisql = "select roll_no as rollno, stud_name as studentname,reg_no,Convert(nvarchar(50),Batch_Year)+'-'+Convert(nvarchar(50),course.Course_Name)+'-'+ Convert(nvarchar(50),Degree.Acronym)+'-'+ Convert(nvarchar(50),current_semester)+'  Sem'  as details,registration.Roll_Admit from registration,Degree,course where registration.degree_code in ('" + strbranch + "') and batch_year in ('" + strbatch + "')  and Degree.Degree_Code=registration.Degree_Code and current_semester in ('" + strsem + "') and course.Course_Id=degree.Course_Id and RollNo_Flag<>0 and cc=0 and delflag=0 and exam_flag <> 'DEBAR'  ORDER BY  course.Course_Name,Registration.Roll_No,degree.Degree_Code";
        }

        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");

        if (ds.Tables[0].Rows.Count > 0)
        {
            Fpspread1.Sheets[0].RowCount++;

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                Fpspread1.Sheets[0].RowCount++;

                Fpspread1.Sheets[0].Cells[0, 1].CellType = cbct1;
                cbct1.AutoPostBack = true;

                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 5);
                Fpspread1.Sheets[0].Cells[0, 1].Text = "";
                Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[0, 1].VerticalAlign = VerticalAlign.Middle;

                cbct.AutoPostBack = false;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = cbct;



                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[i]["rollno"].ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;

                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[i]["studentname"].ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = ds.Tables[0].Rows[i]["rollno"].ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Note = ds.Tables[0].Rows[i]["Roll_Admit"].ToString();


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;


                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[i]["details"].ToString();
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txt;

                Fpspread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
                Fpspread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

                Fpspread1.Visible = true;




            }
        }


    }



    public void bind_data_01()
    {

        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");
        srisql = " select sm.staff_name,ci.Roll_No,typeofcert,ci.issue_date,ci.info from cert_issue ci,staffmaster sm where ci.staff_code=sm.staff_code";
        ds1.Clear();
        ds1 = da.select_method_wo_parameter(srisql, "Text");



        int sno01 = 0;
        //if (chkltype.Items[2].Selected==true)
        //{
        if (ds.Tables[0].Rows.Count > 0)
        {
            // Fpspread1.Sheets[0].RowCount++;



            for (int k = 1; k < Fpspread1.Sheets[0].RowCount; k++)
            {


                string rno = Convert.ToString(Fpspread1.Sheets[0].Cells[k, 3].Tag);
                if (rno != "")
                {

                    if (chltypecount != 1)
                    {
                        if (checkissued == false)
                        {
                            sno01++;
                            Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                        }
                    }
                    else if (chltypecount == 1)
                    {
                        sno01++;
                        Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                    }

                    DataView dv_demand_data = new DataView();
                    ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + rno + "'";
                    dv_demand_data = ds1.Tables[0].DefaultView;
                    int count4 = 0;
                    count4 = dv_demand_data.Count;
                    string infos = "";
                    if (count4 > 0)
                    {
                        if (chltypecount != 1)
                        {
                            if (checkissued == false)
                            {
                                sno01--;
                                Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                                Fpspread1.Sheets[0].Rows[k].Visible = false;

                            }
                        }
                        //if (chkltype.Items[2].Selected==true)
                        //{
                        infos = dv_demand_data[0]["info"].ToString();

                        if (infos != "")
                        {

                            Fpspread1.Sheets[0].Cells[k, 5].Text = dv_demand_data[0]["typeofcert"].ToString();
                            string date01 = dv_demand_data[0]["issue_date"].ToString();
                            string[] spdate = date01.Split(' ');
                            string[] sdate1 = spdate[0].Split('/');
                            string xdate = sdate1[1] + "/" + sdate1[0] + "/" + sdate1[2];
                            Fpspread1.Sheets[0].Cells[k, 6].Text = xdate.ToString();
                            Fpspread1.Sheets[0].Cells[k, 7].Text = dv_demand_data[0]["staff_name"].ToString();
                            Fpspread1.Sheets[0].Rows[k].Visible = true;

                            serialcount++;


                            if (chltypecount != 1)
                            {
                                if (checkissued == true)
                                {
                                    sno01++;
                                    Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                                    //Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                                else if (checkissued == false)
                                {
                                    Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                            }




                        }



                        else
                        {
                            Fpspread1.Sheets[0].Rows[k].Visible = false;
                        }



                    }
                    else if (checkissued == true && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = false;
                    }
                    else if (checkissued == false && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = true;
                    }


                }


            }
            int rowcount2 = Fpspread1.Sheets[0].RowCount;

            if (rowcount2 < 3)
            {
                Fpspread1.Height = 500;
            }
            else
            {
                //Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].PageSize = 25 + (rowcount2 * 20);
            }


        }
        else
        {
            Fpspread1.Visible = false;
            lblmessage.Visible = true;
            lblmessage.Text = "No Records Found";
            btnissue.Visible = false;
        }
    }

    public void bind_data_03()
    {
        ds.Clear();
        ds = da.select_method_wo_parameter(srisql, "Text");
        srisql = " select sm.staff_name,ci.Roll_No,typeofcert,ci.issue_date,ci.info from cert_issue ci,staffmaster sm where ci.staff_code=sm.staff_code";
        ds1.Clear();
        ds1 = da.select_method_wo_parameter(srisql, "Text");

        int sno01 = 0;

        if (ds.Tables[0].Rows.Count > 0)
        {
            // Fpspread1.Sheets[0].RowCount++;



            for (int k = 1; k < Fpspread1.Sheets[0].RowCount; k++)
            {
                string rno = Convert.ToString(Fpspread1.Sheets[0].Cells[k, 3].Tag);
                if (rno != "")
                {
                    if (chltypecount != 1)
                    {
                        if (checkissued == false)
                        {
                            sno01++;
                            Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                        }
                    }
                    else if (chltypecount == 1)
                    {
                        sno01++;
                        Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                    }
                    DataView dv_demand_data = new DataView();
                    ds1.Tables[0].DefaultView.RowFilter = "roll_no='" + rno + "'";
                    dv_demand_data = ds1.Tables[0].DefaultView;
                    int count4 = 0;
                    count4 = dv_demand_data.Count;
                    string infos = "";
                    if (count4 > 0)
                    {
                        if (chltypecount != 1)
                        {
                            if (checkissued == false)
                            {
                                sno01--;
                                Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                                Fpspread1.Sheets[0].Rows[k].Visible = false;
                            }
                        }

                        infos = dv_demand_data[0]["info"].ToString();

                        if (infos != "")
                        {

                            Fpspread1.Sheets[0].Cells[k, 5].Text = dv_demand_data[0]["typeofcert"].ToString();
                            string date01 = dv_demand_data[0]["issue_date"].ToString();
                            string[] spdate = date01.Split(' ');
                            string[] sdate1 = spdate[0].Split('/');
                            string xdate = sdate1[1] + "/" + sdate1[0] + "/" + sdate1[2];
                            Fpspread1.Sheets[0].Cells[k, 6].Text = xdate.ToString();
                            Fpspread1.Sheets[0].Cells[k, 7].Text = dv_demand_data[0]["staff_name"].ToString();
                            Fpspread1.Sheets[0].Rows[k].Visible = true;
                            serialcount++;
                            if (chltypecount != 1)
                            {
                                if (checkissued == true)
                                {
                                    sno01++;
                                    Fpspread1.Sheets[0].Cells[k, 0].Text = Convert.ToString(sno01);
                                    //Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                                else if (checkissued == false)
                                {
                                    Fpspread1.Sheets[0].Rows[k].Visible = false;
                                }
                            }



                        }

                    }
                    else if (checkissued == true && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = false;
                    }
                    else if (checkissued == false && chltypecount != 1)
                    {
                        Fpspread1.Sheets[0].Rows[k].Visible = true;
                    }


                }

            }
 Fpspread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            Fpspread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;

            int rowcount = Fpspread1.Sheets[0].RowCount;

            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            Fpspread1.Sheets[0].PageSize = 25 + (rowcount * 20);
            Fpspread1.Height = 500;
            Fpspread1.SaveChanges();


        }
        else
        {
            Fpspread1.Visible = false;
            lblmessage.Visible = true;
            lblmessage.Text = "No Records Found";
            btnissue.Visible = false;
        }
    }


}