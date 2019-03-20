using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI;
using System.Configuration;

public partial class facultywiseresultanalysis : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    int count = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable hat2 = new Hashtable();
    string sql = "";
    //added by rajasekar 08/10/2018
    DataTable dtl = new DataTable();
    DataRow dtrow = null;
    ArrayList testcount = new ArrayList();
    ArrayList totnoofstudnote = new ArrayList();


    //============================//

    protected void Page_Load(object sender, EventArgs e)
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
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        if (!IsPostBack)
        {

            Bindcollege();
            collegecode = ddlcollege.SelectedValue.ToString();
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();

            showdata.Visible = false;
            Printcontrol.Visible = false;

        }
    }

    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
            else
            {
                errmsg.Text = "Set college rights to the staff";
                errmsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            chklsbatch.Items.Clear();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = false;
                        txtbatch.Text = "---Select---";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            errmsg.Visible = false;
            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstdegree.DataSource = ds2;
                chklstdegree.DataTextField = "course_name";
                chklstdegree.DataValueField = "course_id";
                chklstdegree.DataBind();
                chklstdegree.Items[0].Selected = true;
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
                if (chkdegree.Checked == true)
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    for (int i = 0; i < chklstdegree.Items.Count; i++)
                    {
                        chklstdegree.Items[i].Selected = false;
                        txtdegree.Text = "---Select---";
                    }
                }
                txtdegree.Enabled = true;
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        course_id = course_id + "," + "" + chklstdegree.Items[i].Value.ToString() + "";
                    }
                }
            }
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    chklstbranch.DataSource = ds2;
                    chklstbranch.DataTextField = "dept_name";
                    chklstbranch.DataValueField = "degree_code";
                    chklstbranch.DataBind();
                    chklstbranch.Items[0].Selected = true;
                    for (int i = 0; i < chklstbranch.Items.Count; i++)
                    {
                        chklstbranch.Items[i].Selected = true;
                        if (chklstbranch.Items[i].Selected == true)
                        {
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
                    if (chkbranch.Checked == true)
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chklstbranch.Items[i].Selected = true;
                            txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < chklstbranch.Items.Count; i++)
                        {
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void bindtest()
    {
        string batch_year = "";
        string degree_code = "";
        chkltest.Items.Clear();
        txttest.Text = "---Select---";

        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                if (batch_year.Trim() == "")
                {
                    batch_year = chklsbatch.Items[i].Text.ToString();
                }
                else
                {
                    batch_year = batch_year + "','" + chklsbatch.Items[i].Text.ToString();
                }
            }
        }

        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (degree_code.Trim() == "")
                {
                    degree_code = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    degree_code = degree_code + "','" + chklstbranch.Items[i].Value.ToString();
                }
            }
        }
        if (degree_code.Trim() == "")
        {
            degree_code = "0";
        }
        if (batch_year.Trim() == "")
        {
            batch_year = "0";
        }

        sql = "select distinct criteria from CriteriaForInternal ci, syllabus_master sm where ci.syll_code=sm.syll_code and degree_code in ('" + degree_code + "')   and Batch_Year in ('" + batch_year + "') ";
        ds2.Clear();
        ds2 = d2.select_method_wo_parameter(sql, "Text");
        if (ds2.Tables[0].Rows.Count > 0)
        {
            chkltest.DataSource = ds2;
            chkltest.DataTextField = "criteria";
            chkltest.DataValueField = "criteria";
            chkltest.DataBind();
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                chkltest.Items[i].Selected = true;
                if (chkltest.Items[i].Selected == true)
                {
                    count += 1;
                }
                if (chkltest.Items.Count == count)
                {
                    chktest.Checked = true;
                }
            }
            if (chktest.Checked == true)
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = true;
                    txttest.Text = "Test(" + (chkltest.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = false;
                    txttest.Text = "---Select---";
                }
            }
        }

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            if (chkbatch.Checked == true)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
            //BindDegree(singleuser, group_user, collegecode, usercode);
            //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == chklsbatch.Items.Count)
                {
                    chkbatch.Checked = true;
                }
            }
            //BindDegree(singleuser, group_user, collegecode, usercode);
            //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            Showgrid.Visible = false;
            errmsg.Visible = false;
            Printcontrol.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            Showgrid.Visible = false;
            errmsg.Visible = false;
            Printcontrol.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            bindtest();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            if (chkbranch.Checked == true)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }
            bindtest();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }
            bindtest();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chktest_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            if (chktest.Checked == true)
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = true;
                }
                txttest.Text = "Test(" + (chkltest.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chkltest.Items.Count; i++)
                {
                    chkltest.Items[i].Selected = false;
                }
                chktest.Checked = false;
                txttest.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chkltest_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            showdata.Visible = false;
            errmsg.Visible = false;
            Showgrid.Visible = false;
            Printcontrol.Visible = false;
            string clg = "";
            int commcount = 0;
            txttest.Text = "--Select--";
            chktest.Checked = false;
            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                if (chkltest.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txttest.Text = "Test(" + commcount.ToString() + ")";
                if (commcount == chkltest.Items.Count)
                {
                    chktest.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {

            btnPrint11();
            Printcontrol.Visible = false;
            
            ArrayList arr = new ArrayList();
            arr.Add("TOTAL NO OF APPEARED");
            arr.Add("TOTAL NO OF   ABSENT  ");
            arr.Add("TOTAL NO OF STUDENTS PASSED");
            arr.Add("TOTAL NO OF STUDENTS FAILED");
            arr.Add("PASS %");
            arr.Add("SUBJECT AVG");
            string batch_year = "";
            string degree_code = "";
            string test = "";
            errmsg.Visible = false;
            errmsg.Text = "";

            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (batch_year.Trim() == "")
                    {
                        batch_year = chklsbatch.Items[i].Text.ToString();
                    }
                    else
                    {
                        batch_year = batch_year + "','" + chklsbatch.Items[i].Text.ToString();
                    }
                }

            }

            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (degree_code.Trim() == "")
                    {
                        degree_code = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        degree_code = degree_code + "','" + chklstbranch.Items[i].Value.ToString();
                    }
                }
            }

            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                if (chkltest.Items[i].Selected == true)
                {
                    if (test.Trim() == "")
                    {
                        test = chkltest.Items[i].Text.ToString();
                    }
                    else
                    {
                        test = test + "','" + chkltest.Items[i].Text.ToString();
                    }
                }
            }

            if (batch_year.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Atleast One Batch Year";
                clear();
                return;
            }

            if (degree_code.Trim() == "")
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Select Atleast One Branch";
                clear();
                return;
            }

            if (test.Trim() == "")
            {
                errmsg.Visible = true;
                if (chkltest.Items.Count == 0)
                    errmsg.Text = "No Test Found";
                else
                    errmsg.Text = "Please Select Atleast One Test";
                clear();
                return;
            }

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtrow = dtl.NewRow();
            dtl.Rows.Add(dtrow);

            dtl.Columns.Add("S.No.", typeof(string));

            dtl.Rows[0][0] = "S.No.";

            dtl.Columns.Add("YEAR", typeof(string));

            dtl.Rows[0][1] = "YEAR";

            dtl.Columns.Add("SUBJECT NAME", typeof(string));

            dtl.Rows[0][2] = "SUBJECT NAME";

            dtl.Columns.Add("CLASS", typeof(string));

            dtl.Rows[0][3] = "CLASS";

            dtl.Columns.Add("STAFF NAME", typeof(string));

            dtl.Rows[0][4] = "STAFF NAME";

            dtl.Columns.Add("TOTAL NO OF STUDENTS", typeof(string));

            dtl.Rows[0][5] = "TOTAL NO OF STUDENTS";

            ArrayList criteriacol = new ArrayList();
            int startcol = 0;
            string dd = "";



            for (int i = 0; i < chkltest.Items.Count; i++)
            {
                if (chkltest.Items[i].Selected == true)
                {
                    //FpSpread1.Sheets[0].ColumnCount = FpSpread1.Sheets[0].ColumnCount + arr.Count;
                    

                    startcol = dtl.Columns.Count;
                    
                    for (int j = 0; j < arr.Count; j++)
                    {
                        


                        dtl.Columns.Add(arr[j].ToString() + dd, typeof(string));


                        dtl.Rows[1][dtl.Columns.Count - 1] = arr[j].ToString();

                        dtl.Rows[0][dtl.Columns.Count - 1] = chkltest.Items[i].Text.ToString();

                       

                    }
                    dd = dd + " ";

                    
                    dtl.Rows[0][startcol] = chkltest.Items[i].Text.ToString();

                    testcount.Add(chkltest.Items[i].Text.ToString());

                    criteriacol.Add(startcol + ";" + chkltest.Items[i].Text.ToString());
                    
                }
            }
            

            DataSet dspass = new DataSet();
            //  sql = "select count(sc.roll_no) stucount,sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,c.criteria,c.Criteria_no,s.subject_code,s.subject_name,s.subject_no,e.exam_code,e.min_mark,e.max_mark from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,subjectChooser sc where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.subject_no=s.subject_no and sc.roll_no=r.Roll_No and sc.subject_no=s.subject_no and e.sections=r.Sections and sc.semester=r.Current_Semester and sy.Batch_Year in('" + batch_year + "') and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' group by sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,c.criteria,c.Criteria_no,s.subject_code,s.subject_name,s.subject_no,e.exam_code,e.min_mark,e.max_mark  order by sy.Batch_Year desc,sy.degree_code,r.Current_Semester,r.Sections,c.criteria,s.subject_name ;  select count(sc.roll_no) stucount,sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no from Registration r,syllabus_master sy,subject s,subjectChooser sc where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and sy.syll_code=s.syll_code and sc.roll_no=r.Roll_No and sc.subject_no=s.subject_no and sc.semester=r.Current_Semester and sy.Batch_Year in('" + batch_year + "') and r.degree_code in('" + degree_code + "')   and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' group by sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no  order by r.Current_Semester,s.subject_name,sy.Batch_Year desc,sy.degree_code,r.Sections;   select staff_name,subject_no,batch_year,sections from staffmaster sm,staff_selector ss where sm.staff_code=ss.staff_code;select Degree_Code,Acronym from degree";
            //and r.Batch_Year in ('" + batch_year + "') and r.degree_code in ('" + degree_code + "')

            //-----------------------------  Start Commented By Malang Raja T Reason : Due Timeout Error in queries ----------------------------- 


            //sql = " select count(sc.roll_no) stucount,sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no from Registration r,syllabus_master sy,subject s,subjectChooser sc where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and r.Current_Semester=sy.semester and sy.syll_code=s.syll_code and sc.roll_no=r.Roll_No and sc.subject_no=s.subject_no and sc.semester=r.Current_Semester   and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.college_code='" + collegecode + "'  group by sy.Batch_Year,sy.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no  order by r.Current_Semester,s.subject_name,sy.Batch_Year desc,sy.degree_code,r.Sections;   select staff_name,subject_no,batch_year,sections from staffmaster sm,staff_selector ss where sm.staff_code=ss.staff_code and sm.college_code='" + collegecode + "';select Degree_Code,Acronym from degree where college_code='" + collegecode + "'";


            //----------------------------- End Commented By Malang Raja T Reason : Due Timeout Error in queries ----------------------------- 


            //-----------------------------   Start Added By Malang Raja T   Reason :  Due Timeout Error in queries  ----------------------------- 


            sql = " select count(sc.roll_no) stucount,r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no from Registration r,subject s,subjectChooser sc where  sc.roll_no=r.Roll_No and sc.subject_no=s.subject_no and sc.semester=r.Current_Semester   and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and r.college_code='" + collegecode + "' group by r.Batch_Year,r.degree_code,r.Current_Semester,r.Sections,s.subject_code,s.subject_name,s.subject_no  order by r.Current_Semester,s.subject_name,r.Batch_Year desc,r.degree_code,r.Sections;   select staff_name,subject_no,batch_year,sections from staffmaster sm,staff_selector ss where sm.staff_code=ss.staff_code and sm.college_code='" + collegecode + "';select Degree_Code,Acronym from degree where college_code='" + collegecode + "'";


            //-----------------------------   End Added By Malang Raja T  Reason :  Due Timeout Error in queries  ----------------------------- 
            ds2.Clear();
            ds2 = d2.select_method_wo_parameter(sql, "Text");

            string addfilter = "  and  sy.Batch_Year in ('" + batch_year + "') and r.degree_code in('" + degree_code + "') and c.criteria in('" + test + "') ";
            //sql = "select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,sy.Batch_Year,sy.semester,r.degree_code,r.Sections from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and (re.marks_obtained>=e.min_mark or re.marks_obtained=-2 or re.marks_obtained=-3) " + addfilter + " order by s.subject_no,sy.Batch_Year,sy.semester,r.degree_code,c.criteria; select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,sy.Batch_Year,sy.semester,r.degree_code,r.Sections from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and (re.marks_obtained<e.min_mark and re.marks_obtained<>'-2' and re.marks_obtained<>'-3') " + addfilter + " order by s.subject_no,sy.Batch_Year,sy.semester,r.degree_code,c.criteria; select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,sy.Batch_Year,sy.semester,r.degree_code,r.Sections from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and (re.marks_obtained>=0 or re.marks_obtained='-2' or re.marks_obtained='-3') " + addfilter + " order by s.subject_no,sy.Batch_Year,sy.semester,r.degree_code,c.criteria;  select r.Roll_No,s.subject_name,s.subject_code,s.subject_no,c.criteria,c.Criteria_no,e.exam_code,re.marks_obtained,sy.Batch_Year,sy.semester,r.degree_code,r.Sections from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no and re.marks_obtained='-1' " + addfilter + " order by s.subject_no,sy.Batch_Year,sy.semester,r.degree_code,c.criteria;select sum(re.marks_obtained), s.subject_no,c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.semester,r.degree_code,r.Sections from Registration r,syllabus_master sy,subject s,CriteriaForInternal c,Exam_type e,Result re where r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=s.syll_code and sy.syll_code=c.syll_code and  c.Criteria_no=e.criteria_no and e.exam_code=re.exam_code and s.subject_no=e.subject_no and r.Roll_No=re.roll_no  and (re.marks_obtained>=0) " + addfilter + " group by s.subject_no,c.criteria,c.Criteria_no,e.exam_code,sy.Batch_Year,sy.semester, r.degree_code,r.Sections order by s.subject_no,sy.Batch_Year,sy.semester,r.degree_code,c.criteria";


            hat2.Clear();
            hat2.Add("Batch_degree_criteria", addfilter);
            
            dspass.Clear();
            //dspass = d2.select_method_wo_parameter(sql, "Text");
            dspass = d2.select_method("FacultyWiseResultAnalysis", hat2, "sp");//rajasekar

            string subjectcode = "";
            string year = "";
            string sem = "";
            string subjectname = "";
            string staffname = "";
            string classsec = "";
            string subjectno = "";
            string batchyear = "";
            string secdetails = "";
            string totalnostud = "";
            string fpvaluefield = "";
            double percentage = 0;
            double avg = 0;
            int sno = 0;
            DataView dvdegree = new DataView();
            DataView dvsubjects = new DataView();
            ds2.Tables[0].DefaultView.RowFilter = "Batch_Year in ('" + batch_year + "') and degree_code in ('" + degree_code + "')";
            dvsubjects = ds2.Tables[0].DefaultView;
            if (dvsubjects.Count > 0)
            {
                for (int i = 0; i < dvsubjects.Count; i++)
                {
                    sem = dvsubjects[i]["Current_Semester"].ToString();
                    totalnostud = dvsubjects[i]["stucount"].ToString();
                    if (sem.Trim() == "1" || sem.Trim() == "2")
                    {
                        year = "I";
                    }
                    if (sem.Trim() == "3" || sem.Trim() == "4")
                    {
                        year = "II";
                    }
                    if (sem.Trim() == "5" || sem.Trim() == "6")
                    {
                        year = "III";
                    }
                    if (sem.Trim() == "7" || sem.Trim() == "8")
                    {
                        year = "IV";
                    }

                    if (subjectcode != dvsubjects[i]["subject_code"].ToString())
                    {
                        sno++;
                        subjectname = dvsubjects[i]["subject_name"].ToString().ToUpper();
                        subjectcode = dvsubjects[i]["subject_code"].ToString();
                    }
                    subjectno = dvsubjects[i]["subject_no"].ToString();
                    batchyear = dvsubjects[i]["Batch_Year"].ToString();
                    ds2.Tables[2].DefaultView.RowFilter = "Degree_Code='" + dvsubjects[i]["degree_code"].ToString() + "'";
                    dvdegree = ds2.Tables[2].DefaultView;
                    if (dvdegree.Count > 0)
                    {
                        classsec = dvdegree[0]["Acronym"].ToString();
                    }
                    if (classsec != "")
                    {
                        classsec = classsec + "  " + dvsubjects[i]["Sections"].ToString();
                    }
                    secdetails = "";
                    if (dvsubjects[i]["Sections"].ToString().Trim() != "")
                    {
                        secdetails = "and sections='" + dvsubjects[i]["Sections"].ToString() + "'";
                    }

                    ds2.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + "";
                    dvdegree = ds2.Tables[1].DefaultView;
                    if (dvdegree.Count > 0)
                    {
                        staffname = dvdegree[0]["staff_name"].ToString();
                    }

                    
                    dtrow = dtl.NewRow();
                    dtl.Rows.Add(dtrow);

                    fpvaluefield = subjectno + "-" + batchyear + "-" + sem + "-" + dvsubjects[i]["degree_code"].ToString() + "-" + dvsubjects[i]["Sections"].ToString();
                    


                    dtl.Rows[i + 2][0] = Convert.ToString(sno);

                    dtl.Rows[i + 2][1] = Convert.ToString(year);

                    dtl.Rows[i + 2][2] = Convert.ToString(subjectname);

                    dtl.Rows[i + 2][3] = Convert.ToString(classsec);

                    dtl.Rows[i + 2][4] = Convert.ToString(staffname);

                    dtl.Rows[i + 2][5] = Convert.ToString(totalnostud);


                    totnoofstudnote.Add(fpvaluefield);
                    // fpvaluefield = "";

                }

            }
            

            string degreecc = "";
            string sectr = "";
            string subcriterianame = "";


            for (int i = 2; i < dtl.Rows.Count; i++)
            {
                if ( i == dtl.Rows.Count-1)
                {
                }
                

                fpvaluefield = totnoofstudnote[i - 2].ToString();
                
                string[] splitvalue = fpvaluefield.Split('-');

                subjectno = "";
                batchyear = "";
                sem = "";
                degreecc = "";
                sectr = "";
                string critername = "";
                if (splitvalue.Length == 5)
                {
                    subjectno = splitvalue[0].ToString();
                    batchyear = splitvalue[1].ToString();
                    sem = splitvalue[2].ToString();
                    degreecc = splitvalue[3].ToString();
                    sectr = splitvalue[4].ToString();
                }

                for (int j = 0; j < criteriacol.Count; j++)
                {
                    critername = criteriacol[j].ToString();
                    string[] splitcritername = critername.Split(';');
                    critername = splitcritername[1].ToString();
                    int criterno = Convert.ToInt32(splitcritername[0].ToString());
                    double appeared = 0;
                    if (sectr.Trim() != "")
                    {
                        secdetails = "and sections='" + sectr + "'";
                    }

                    //pass
                    

                    subcriterianame = dtl.Columns[criterno].ColumnName;

                    dspass.Tables[0].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    dvdegree = dspass.Tables[0].DefaultView;
                    if (dvdegree.Count > 0)
                    {
                        

                        dtl.Rows[i][criterno + 2] = Convert.ToString(dvdegree.Count);
                    }
                    else
                    {
                        //FpSpread1.Sheets[0].Cells[i, criterno + 2].Text = "";
                       

                        dtl.Rows[i][criterno + 2] = Convert.ToString(dvdegree.Count);
                    }
                    string passcount = Convert.ToString(dvdegree.Count);

                    //------------------------ Modifieid By Malang Raja on 18/02/2016 -------------------------


                    //appeared
                    dspass.Tables[2].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    dvdegree = dspass.Tables[2].DefaultView;
                    if (dvdegree.Count > 0)
                    {
                        

                        dtl.Rows[i][criterno] = Convert.ToString(dvdegree.Count);

                        appeared = dvdegree.Count;
                    }
                    else
                    {
                        //FpSpread1.Sheets[0].Cells[i, criterno].Text = "";
                        

                        dtl.Rows[i][criterno] = Convert.ToString(dvdegree.Count);

                        appeared = dvdegree.Count;
                    }


                    //if( FpSpread1.Sheets[0].Cells[i, 5].Text.ToString().Trim()!="")
                    //{
                    //    percentage = Convert.ToDouble(FpSpread1.Sheets[0].Cells[i, 5].Text.ToString());
                    //    percentage = (dvdegree.Count / percentage) * 100;
                    //    percentage = Math.Round(percentage,2);
                    //    FpSpread1.Sheets[0].Cells[i, criterno + 4].Text = Convert.ToString(percentage);
                    //}

                    //------------------------ Modifieid By Malang Raja on 18/02/2016 -------------------------


                    if ( dtl.Rows[i][criterno].ToString().Trim() != "")
                    {
                        

                        percentage = Convert.ToDouble(dtl.Rows[i][criterno].ToString());  
                        double passcnt = 0;
                        Double.TryParse(passcount, out passcnt);
                        if (passcnt != 0 && percentage != 0)
                        {
                            percentage = (passcnt / percentage) * 100;
                            percentage = Math.Round(percentage, 2);
                        }
                        else
                        {
                            percentage = 0;
                        }
                        

                        dtl.Rows[i][criterno + 4] = Convert.ToString(percentage);
                    }
                    else
                    {
                        percentage = 0;
                        percentage = Math.Round(percentage, 2);
                        
                        //FpSpread1.Sheets[0].Cells[i, criterno + 4].Text = "";


                        dtl.Rows[i][criterno + 4] = Convert.ToString(percentage);
                    }

                    //fail
                    dspass.Tables[1].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    dvdegree = dspass.Tables[1].DefaultView;
                    //if (dvdegree.Count > 0)
                    //{
                   

                    dtl.Rows[i][criterno + 3] = Convert.ToString(dvdegree.Count);

                    //}
                    //else
                    //{
                    //    FpSpread1.Sheets[0].Cells[i, criterno + 3].Text = "";
                    //}



                    //Absent Count
                    dspass.Tables[3].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    dvdegree = dspass.Tables[3].DefaultView;
                    //if (dvdegree.Count > 0)
                    //{
                    

                    dtl.Rows[i][criterno + 1] = Convert.ToString(dvdegree.Count);

                    //}
                    //else
                    //{
                    //    FpSpread1.Sheets[0].Cells[i, criterno + 1].Text = "";
                    //}

                    //AVG Count
                    dspass.Tables[4].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    dvdegree = dspass.Tables[4].DefaultView;
                    if (dvdegree.Count > 0)
                    {
                        if (dvdegree[0][0].ToString().Trim() != "")
                        {
                            

                            percentage = Convert.ToDouble(dtl.Rows[i][criterno].ToString());

                            if (percentage != 0)
                            {
                                percentage = (Convert.ToDouble(dvdegree[0][0].ToString()) / percentage);
                                percentage = Math.Round(percentage, 2);
                                

                                dtl.Rows[i][criterno + 5] = Convert.ToString(percentage);
                            }
                            else
                            {
                                percentage = 0;
                                percentage = Math.Round(percentage, 2);
                                

                                dtl.Rows[i][criterno + 5] = Convert.ToString(percentage);
                            }
                            
                        }
                        else
                        {
                            percentage = 0;
                            percentage = Math.Round(percentage, 2);
                            

                            dtl.Rows[i][criterno + 5] = Convert.ToString(percentage);
                        }
                    }
                    else
                    {
                        percentage = 0;
                        percentage = Math.Round(percentage, 2);
                        

                        dtl.Rows[i][criterno + 5] = Convert.ToString(percentage);
                    }
                    //dspass.Tables[4].DefaultView.RowFilter = "subject_no='" + subjectno + "' and batch_year='" + batchyear + "' " + secdetails + " and semester='" + sem + "' and degree_code = '" + degreecc + "' and criteria ='" + critername + "'  ";
                    //dvdegree = dspass.Tables[4].DefaultView;
                    //FpSpread1.Sheets[0].Cells[i, criterno+4].Text = Convert.ToString(dvdegree.Count);

                }
            }
            if ( dtl.Rows.Count == 0)
            {
                Showgrid.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                clear();
            }
            else
            {
                
                showdata.Visible = true;
                
                errmsg.Visible = false;
                errmsg.Text = "";
                

                 Showgrid.DataSource = dtl;
                 Showgrid.DataBind();
                 Showgrid.Visible = true;
                 Showgrid.HeaderRow.Visible = false;


                 int dtrowcount = dtl.Rows.Count;
                 int rowspanstart0 = 0;
                 int rowspanstart1 = 0;
                 int rowspanstart2 = 0;
                 int rowspanstart4 = 0;
                 int colspan = 6;
                  



                 for (int i = 0; i < Showgrid.Rows.Count; i++)
                 {
                     int rowspancount0 = 0;
                     int rowspancount1 = 0;
                     int rowspancount2 = 0;
                     int rowspancount4 = 0;

                     if (i != dtrowcount - 1)
                     {
                         if (rowspanstart0 == i)
                         {
                             for (int k = rowspanstart0 + 1; Showgrid.Rows[i].Cells[0].Text == Showgrid.Rows[k].Cells[0].Text; k++)
                             {
                                 rowspancount0++;
                                 if (k == dtrowcount - 1)
                                     break;
                             }
                             rowspanstart0++;
                         }
                         if (rowspanstart1 == i)
                         {
                             for (int k = rowspanstart1 + 1; Showgrid.Rows[i].Cells[1].Text == Showgrid.Rows[k].Cells[1].Text; k++)
                             {
                                 rowspancount1++;
                                 if (k == dtrowcount - 1)
                                     break;
                             }
                             rowspanstart1++;
                         }
                         if (rowspanstart2 == i)
                         {
                             for (int k = rowspanstart2 + 1; Showgrid.Rows[i].Cells[2].Text == Showgrid.Rows[k].Cells[2].Text; k++)
                             {
                                 rowspancount2++;
                                 if (k == dtrowcount - 1)
                                     break;
                             }
                             rowspanstart2++;
                         }
                         if (rowspanstart4 == i)
                         {
                             for (int k = rowspanstart4 + 1; Showgrid.Rows[i].Cells[4].Text == Showgrid.Rows[k].Cells[4].Text; k++)
                             {
                                 rowspancount4++;
                                 if (k == dtrowcount - 1)
                                     break;
                             }
                             rowspanstart4++;
                         }

                         if (rowspancount0 != 0)
                         {
                             rowspanstart0 = rowspanstart0 + rowspancount0;

                             Showgrid.Rows[i].Cells[0].RowSpan = rowspancount0 + 1;
                             for (int a = i; a < rowspanstart0 - 1; a++)
                                 Showgrid.Rows[a + 1].Cells[0].Visible = false;

                         }
                         if (rowspancount1 != 0)
                         {
                             rowspanstart1 = rowspanstart1 + rowspancount1;

                             Showgrid.Rows[i].Cells[1].RowSpan = rowspancount1 + 1;
                             for (int a = i; a < rowspanstart1 - 1; a++)
                                 Showgrid.Rows[a + 1].Cells[1].Visible = false;

                         }

                         if (rowspancount2 != 0)
                         {
                             rowspanstart2 = rowspanstart2 + rowspancount2;

                             Showgrid.Rows[i].Cells[2].RowSpan = rowspancount2 + 1;
                             for (int a = i; a < rowspanstart2 - 1; a++)
                                 Showgrid.Rows[a + 1].Cells[2].Visible = false;

                         }
                         if (rowspancount4 != 0)
                         {
                             rowspanstart4 = rowspanstart4 + rowspancount4;

                             Showgrid.Rows[i].Cells[4].RowSpan = rowspancount4 + 1;
                             for (int a = i; a < rowspanstart4 - 1; a++)
                                 Showgrid.Rows[a + 1].Cells[4].Visible = false;

                         }




                     }

                     for (int j = 0; j < dtl.Columns.Count; j++)
                     {
                         if (i == 0 || i==1)
                         {
                             Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;
                             Showgrid.Rows[i].Cells[j].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                             Showgrid.Rows[i].Cells[j].BorderColor = Color.Black;
                             Showgrid.Rows[i].Cells[j].Font.Bold = true;

                             if (i == 0)
                             {
                                 if (j == colspan)
                                 {
                                     Showgrid.Rows[i].Cells[j].ColumnSpan = 6;
                                     for (int a = j + 1; a < j+6; a++)
                                         Showgrid.Rows[i].Cells[a].Visible = false;
                                     colspan += 6;
                                 }
                                 else if (j < 6)
                                 {
                                     Showgrid.Rows[i].Cells[j].RowSpan = 2;
                                     for (int a = i; a < 1; a++)
                                         Showgrid.Rows[a + 1].Cells[j].Visible = false;
                                 }

                             }
                         }
                         else
                         {
                             if (j != 2 && j != 3 && j != 4)
                             {
                                 Showgrid.Rows[i].Cells[j].HorizontalAlign = HorizontalAlign.Center;

                             }
                         }
                     }





                 }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void clear()
    {
        showdata.Visible = false;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lblrptname.Visible = false;
                
                d2.printexcelreportgrid(Showgrid, reportname);
            }
            else
            {
                lblrptname.Text = "Please Enter Your Report Name";
                lblrptname.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {

        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Overall College Faculty Wise Result Analysis Report";
            string pagename = "facultywiseresultanalysis.aspx";
            //if (FpSpread1.Visible == true)
            //{
            //    Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            //}
            string ss = Session["usercode"].ToString();
            Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
            Printcontrol.Visible = true;
        }
        catch
        {

        }
    }
    public void btnPrint11()
    {
        string college_code = Convert.ToString(ddlcollege.SelectedValue);
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
        spReportName.InnerHtml = "Overall College Faculty Wise Result Analysis Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
    
    public override void VerifyRenderingInServerForm(Control control)
    { }
}