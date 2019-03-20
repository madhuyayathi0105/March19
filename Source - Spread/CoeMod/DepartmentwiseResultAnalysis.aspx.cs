using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System.Configuration;

public partial class DepartmentwiseResultAnalysis : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string val = "";
    //string examcode = "";
    //string vall2 = "";
    string mainvalue = "";
    string val11 = "";
    string branchval1 = "";
    string branch = "";
    string strquery = "";
    int cnt = 0;
    string val22 = "";

    int tot = 0;
    int tot1 = 0;

    int tott = 0;
    int tot11 = 0;

    int tott1 = 0;
    int tot12 = 0;

    int tott2 = 0;
    int tot13 = 0;

    int tott3 = 0;
    int tot14 = 0;

    int t1 = 0;
    int t2 = 0;
    int t3 = 0;
    int t4 = 0;
    int t5 = 0;

    int g = 0;

    Hashtable hat = new Hashtable();
    DAccess2 da = new DAccess2();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();



    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            deptwiseresultanalysisgrid.Visible = false;
            deptwiseresultanalysisexternalgrid.Visible = false;
            g1btnexcel.Visible = false;
            g1btnprint.Visible = false;
            lblerror.Visible = false;
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
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch(val);
                bindsemester();
                bindtestname(val11);
                //bindexammonth(vall2);

            }
        }
        catch(Exception ex)
        {
        }
    }

    public void bindcollege()
    {
        try
        {
            string columnfield = "";
            usercode = Session["UserCode"].ToString();
            group_code = Session["group_code"].ToString();
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hat, "sp");
            ddcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddcollege.DataSource = ds;
                ddcollege.DataTextField = "collname";
                ddcollege.DataValueField = "college_code";
                ddcollege.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            dropbatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropbatch.DataSource = ds;
                dropbatch.DataTextField = "batch_year";
                dropbatch.DataValueField = "batch_year";
                dropbatch.DataBind();

            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                dropbatch.SelectedValue = max_bat.ToString();

            }
            dropbatch.Text = "batch (" + 1 + ")";
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddcollegeselect(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            //bindbatch();
            bindbranch(val);
            bindtestname(val22);

            // bindbranch(chcklistbranch.SelectedItem.ToString());
        }
        catch (Exception ex)
        {

        }
    }

    public void binddegree()
    {
        try
        {
            int count = 0;
            chcklistdegree.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddcollege.SelectedItem.Value;
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            DataSet ds1 = new DataSet();
            ds1 = da.select_method("bind_degree", hat, "sp");
            int count1 = ds1.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                chcklistdegree.DataSource = ds1;
                chcklistdegree.DataTextField = "course_name";
                chcklistdegree.DataValueField = "course_id";
                chcklistdegree.DataBind();
            }
            if (chcklistdegree.Items.Count > 0)
            {
                for (int j = 0; j < chcklistdegree.Items.Count; j++)
                {
                    count++;
                    chcklistdegree.Items[j].Selected = true;
                    txtdegree.Text = "Degree " + "(" + count + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbranch(string val)
    {
        try
        {
            int count = 0;
            string mainvalue = "";
            if (chcklistdegree.Items.Count > 0)
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {
                    if (chcklistdegree.Items[i].Selected == true)
                    {
                        string subvalue = "";
                        subvalue = chcklistdegree.Items[i].Value;
                        if (mainvalue == "")
                        {
                            mainvalue = subvalue;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + subvalue;
                        }


                    }
                }
                if (mainvalue.Trim() != "")
                {
                    ds.Clear();
                    ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), mainvalue, ddcollege.SelectedItem.Value, Session["usercode"].ToString());
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        chcklistbranch.DataSource = ds;
                        chcklistbranch.DataTextField = "dept_name";
                        chcklistbranch.DataValueField = "degree_code";
                        chcklistbranch.DataBind();
                    }

                }
                if (chcklistbranch.Items.Count > 0)
                {
                    for (int h = 0; h < chcklistbranch.Items.Count; h++)
                    {
                        count++;
                        txtbranch.Text = "Branch " + "(" + count + ")";
                        chcklistbranch.Items[h].Selected = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }


    protected void checkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (chckdegree.Checked == true)
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {

                    if (chckdegree.Checked == true)
                    {
                        chcklistdegree.Items[i].Selected = true;
                        txtdegree.Text = "Degree (" + (chcklistdegree.Items.Count) + ")";
                        build1 = chcklistdegree.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                bindbranch(buildvalue1);
                // bindtestname(buildvalue1);
                bindsemester();
            }
            else
            {
                for (int i = 0; i < chcklistdegree.Items.Count; i++)
                {
                    chcklistdegree.Items[i].Selected = false;
                    txtdegree.Text = "---Select---";
                    txtbranch.Text = "--Select--";
                    chcklistbranch.ClearSelection();
                    chckbranch.Checked = false;
                }
            }

        }
        catch (Exception ex)
        {

        }
    }


    protected void cheklist_Degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chckdegree.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < chcklistdegree.Items.Count; i++)
            {
                if (chcklistdegree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtdegree.Text = "--Select--";
                    build = chcklistdegree.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            bindbranch(buildvalue);
            //    bindtestname(buildvalue);
            bindsemester();

            if (seatcount == chcklistdegree.Items.Count)
            {
                txtdegree.Text = "Degree (" + seatcount.ToString() + ")";
                chckdegree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtdegree.Text = "--Select--";
                txtbranch.Text = "--Select--";
            }
            else
            {
                txtdegree.Text = "Degree (" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {

        }
    }



    protected void checkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (chckbranch.Checked == true)
            {
                for (int i = 0; i < chcklistbranch.Items.Count; i++)
                {

                    if (chckbranch.Checked == true)
                    {
                        chcklistbranch.Items[i].Selected = true;
                        txtbranch.Text = "Branch (" + (chcklistbranch.Items.Count) + ")";
                        build1 = chcklistbranch.Items[i].Value.ToString();
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
                //bindtestname(buildvalue1);
            }

            else
            {
                for (int i = 0; i < chcklistbranch.Items.Count; i++)
                {
                    chcklistbranch.Items[i].Selected = false;
                    txtbranch.Text = "--Select--";

                    chcklistbranch.ClearSelection();
                    chckbranch.Checked = false;
                }
            }
            bindtestname(buildvalue1);
        }
        catch (Exception ex)
        {

        }
    }

    protected void cheklistBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chckbranch.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < chcklistbranch.Items.Count; i++)
            {
                if (chcklistbranch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txtbranch.Text = "--Select--";
                    build = chcklistbranch.Items[i].Value.ToString();
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
            bindtestname(buildvalue);
            if (seatcount == chcklistbranch.Items.Count)
            {
                txtbranch.Text = "Branch (" + seatcount.ToString() + ")";
                chckbranch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txtbranch.Text = "--Select--";
                chckbranch.Text = "Select All";
            }
            else
            {
                txtbranch.Text = "Branch (" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (dropreporttype.SelectedItem.Text == "Internal")
            {
                //Panel21.Visible = true;
                Panel22.Visible = true;
                lbltestname.Visible = true;
                txttestname.Visible = true;
                UpdatePanel2.Visible = true;
                g2btnexcel.Visible = false;
                g2btnprint.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
            }
            else
            {
                Panel22.Visible = true;
                //Panel21.Visible = false;
                lbltestname.Visible = false;
                txttestname.Visible = false;
                UpdatePanel2.Visible = false;
                g2btnexcel.Visible = false;
                g2btnprint.Visible = false;
                g1btnexcel.Visible = false;
                g1btnprint.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindtestname(string val22)
    {
        try
        {
            txttestname.Text = "---Select---";
            chcktestname.Checked = false;
            int cout = 0;
            string query = "";
            int i = 0;
            chcklisttestname.Items.Clear();
            if (val22 != "")
            {
                //query = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and semester='" + dropsem.SelectedValue.ToString() + "' and degree_code in (" + val22 + ") and syllabus_year in (select syllabus_year from syllabus_master where semester='" + dropsem.SelectedValue.ToString() + "' and batch_year='" + dropbatch.SelectedValue.ToString() + "') group by degree_code, criteria order by criteria";
                query = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.college_code='" + ddcollege.Text.ToString() + "' and r.batch_year='" + dropbatch.Text.ToString() + "' and s.semester='" + dropsem.SelectedItem.ToString() + "' order by criteria asc";
            }
            else
            {
                query = "select distinct c.criteria from criteriaforinternal c,registration r,syllabus_master s where r.degree_code=s.degree_code and r.batch_year=s.batch_year and c.syll_code=s.syll_code and cc=0 and delflag=0 and r.exam_flag<>'debar' and r.college_code='" + ddcollege.Text.ToString() + "' and r.batch_year='" + dropbatch.Text.ToString() + "' and s.semester='" + dropsem.SelectedItem.ToString() + "' order by criteria asc";
                //query = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and semester='" + dropsem.SelectedValue.ToString() + "' and syllabus_year in (select syllabus_year from syllabus_master where semester='" + dropsem.SelectedValue.ToString() + "' and batch_year='" + dropbatch.SelectedValue.ToString() + "') group by degree_code, criteria order by criteria";
                //query = "select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and semester='" + dropsem.SelectedValue.ToString() + "' and syllabus_year in (select syllabus_year from syllabus_master where  semester='" + dropsem.SelectedValue.ToString() + "' and batch_year='" + dropbatch.SelectedValue.ToString() + "') and batch_year='" + dropbatch.SelectedValue.ToString() + "' order by criteria";
            }
            ds = da.select_method_wo_parameter(query, "Text");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                chcklisttestname.DataSource = ds;
                chcklisttestname.DataTextField = "criteria";
                chcklisttestname.DataValueField = "criteria";
                chcklisttestname.DataBind();
            }

            if (chcklisttestname.Items.Count > 0)
            {

                for (i = 0; i < chcklisttestname.Items.Count; i++)
                {

                    cout++;
                    chcklisttestname.Items[i].Selected = true;
                    txttestname.Text = "Test " + "(" + cout + ")";
                    chcktestname.Checked = true;

                }
                //chcktestname.Checked = true;
            }
            else
            {
                //chcklisttestname.Items[i].Selected = false;
                txttestname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void checktestname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";

            if (chcktestname.Checked == true)
            {
                for (int i = 0; i < chcklisttestname.Items.Count; i++)
                {

                    if (chcktestname.Checked == true)
                    {
                        chcklisttestname.Items[i].Selected = true;
                        txttestname.Text = "Test (" + (chcklisttestname.Items.Count) + ")";
                        build1 = chcklisttestname.Items[i].Value.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }

                    }
                }
            }
            else
            {
                for (int i = 0; i < chcklisttestname.Items.Count; i++)
                {
                    chcklisttestname.Items[i].Selected = false;
                    txttestname.Text = "--Select--";

                    chcklisttestname.ClearSelection();
                    chcktestname.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }


    protected void cheklisttestname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;

            chcktestname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < chcklisttestname.Items.Count; i++)
            {
                if (chcklisttestname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txttestname.Text = "Select All";
                    build = chcklisttestname.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }
                }
            }

            //  bindtestname(buildvalue);

            if (seatcount == chcklisttestname.Items.Count)
            {
                txttestname.Text = "Test (" + seatcount.ToString() + ")";
                chcktestname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txttestname.Text = "--Select--";
                chcktestname.Text = "Select All";
                //chcktestname.Text = "--Select--";
            }
            else
            {
                txttestname.Text = "Test (" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }


    protected void dropbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string bn = "";
            binddegree();
            bindbranch(bn);
            bindtestname(bn);
            bindsemester();

        }
        catch (Exception ex)
        {
        }
    }


    protected void go_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    protected void dropsem_selectedIndex(object sender, EventArgs e)
    {
        bindtestname(val22);
    }

    public void bindsemester()
    {
        try
        {
            dropsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;

            string strbranch = "";
            for (int b = 0; b < chcklistbranch.Items.Count; b++)
            {
                if (chcklistbranch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = chcklistbranch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + chcklistbranch.Items[b].Value;
                    }
                }
            }
            //bindtestname(strbranch);
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddcollege.SelectedValue.ToString() + " and batch_year=" + dropbatch.Text.ToString() + " " + strbranch + " order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        dropsem.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        dropsem.Items.Add(i.ToString());
                    }
                }
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddcollege.SelectedValue.ToString() + " " + strbranch + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

                    for (i = 1; i <= duration; i++)
                    {
                        if (first_year == false)
                        {
                            dropsem.Items.Add(i.ToString());
                        }
                        else if (first_year == true && i != 2)
                        {
                            dropsem.Items.Add(i.ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    //        string mainvalue = "";
    //        if (chcklistbranch.Items.Count > 0)
    //        {
    //            for (int i = 0; i < chcklistbranch.Items.Count; i++)
    //            {
    //                if (chcklistbranch.Items[i].Selected == true)
    //                {
    //                    if (mainvalue == "")
    //                    {
    //                        mainvalue = chcklistbranch.Items[i].Value;
    //                    }
    //                    else
    //                    {
    //                        mainvalue = mainvalue + "," + chcklistbranch.Items[i].Value;
    //                    }
    //                }
    //            }
    //        }
    //        dropsem.Items.Clear();
    //        ds.Clear();
    //        if (mainvalue.Trim() != "")
    //        {
    //            ds = da.select_method_wo_parameter("select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddcollege.SelectedValue.ToString() + " and batch_year=" + dropbatch.SelectedValue.ToString() + " and degree_code in(" + mainvalue + ") order by NDurations desc", "text");
    //            int count5 = ds.Tables[0].Rows.Count;
    //            if (count5 > 0)
    //            {
    //                count5 = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
    //                for (int i = 1; i <= count5; i++)
    //                {
    //                    dropsem.Items.Add(Convert.ToString(i));
    //                }
    //            }
    //        }
    //        else
    //        {
    //            dropsem.Enabled = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}

    //  rajesh

    //try
    //{

    //    if (chcklistbranch.Items.Count > 0)
    //    {
    //        for (int i = 0; i < chcklistbranch.Items.Count; i++)
    //        {
    //            if (chcklistbranch.Items[i].Selected == true)
    //            {
    //                if (mainvalue == "")
    //                {
    //                    mainvalue = chcklistbranch.Items[i].Value;
    //                }
    //                else
    //                {
    //                    mainvalue = mainvalue + "," + chcklistbranch.Items[i].Value;
    //                }
    //            }
    //        }
    //    }
    //    dropsem.Items.Clear();
    //    ds.Clear();
    //    if (mainvalue.Trim() != "")
    //    {
    //        ds = da.BindSem(mainvalue, dropbatch.SelectedItem.Text, ddcollege.SelectedItem.Value);

    //        int count = ds.Tables[0].Rows.Count;
    //        if (count > 0)
    //        {
    //            count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
    //            for (int i = 1; i <= count; i++)
    //            {
    //                dropsem.Items.Add(Convert.ToString(i));
    //            }
    //        }
    //        else
    //        {
    //            dropsem.Enabled = false;
    //        }
    //    }
    //    else
    //    {
    //        dropsem.Enabled = false;
    //    }
    //}

    //  rajesh


    protected void g2btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DepartmentwiseResultAnalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            deptwiseresultanalysisgrid.AllowPaging = false;
            deptwiseresultanalysisgrid.HeaderRow.Style.Add("width", "15%");
            deptwiseresultanalysisgrid.HeaderRow.Style.Add("font-size", "10px");
            deptwiseresultanalysisgrid.HeaderRow.Style.Add("text-align", "center");
            deptwiseresultanalysisgrid.Style.Add("text-decoration", "none");
            deptwiseresultanalysisgrid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            deptwiseresultanalysisgrid.Style.Add("font-size", "8px");
            btngo_Click(sender, e);
            deptwiseresultanalysisgrid.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A2);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }

    }

    protected void g2btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DepartmentwiseResultAnalysis.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                deptwiseresultanalysisgrid.AllowPaging = false;
                btngo_Click(sender, e);

                deptwiseresultanalysisgrid.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in deptwiseresultanalysisgrid.HeaderRow.Cells)
                {
                    cell.BackColor = deptwiseresultanalysisgrid.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in deptwiseresultanalysisgrid.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = deptwiseresultanalysisgrid.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = deptwiseresultanalysisgrid.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                deptwiseresultanalysisgrid.RenderControl(hw);

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

    protected void g1btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=DepartmentwiseResultAnalysis.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            deptwiseresultanalysisexternalgrid.AllowPaging = false;
            deptwiseresultanalysisexternalgrid.HeaderRow.Style.Add("width", "15%");
            deptwiseresultanalysisexternalgrid.HeaderRow.Style.Add("font-size", "10px");
            deptwiseresultanalysisexternalgrid.HeaderRow.Style.Add("text-align", "center");
            deptwiseresultanalysisexternalgrid.Style.Add("text-decoration", "none");
            deptwiseresultanalysisexternalgrid.Style.Add("font-family", "Arial, Helvetica, sans-serif;");
            deptwiseresultanalysisexternalgrid.Style.Add("font-size", "8px");
            btngo_Click(sender, e);
            deptwiseresultanalysisexternalgrid.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A2);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }
        catch (Exception ex)
        {
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void g1btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=DepartmentwiseResultAnalysis.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                deptwiseresultanalysisexternalgrid.AllowPaging = false;
                btngo_Click(sender, e);

                deptwiseresultanalysisexternalgrid.HeaderRow.BackColor = System.Drawing.Color.White;
                foreach (TableCell cell in deptwiseresultanalysisexternalgrid.HeaderRow.Cells)
                {
                    cell.BackColor = deptwiseresultanalysisexternalgrid.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in deptwiseresultanalysisexternalgrid.Rows)
                {
                    row.BackColor = System.Drawing.Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = deptwiseresultanalysisexternalgrid.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = deptwiseresultanalysisexternalgrid.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                deptwiseresultanalysisexternalgrid.RenderControl(hw);

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


    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            if (dropreporttype.SelectedItem.Text != "External")
            {
                DataRow dr = null;
                DataTable dt1 = new DataTable();
                DataSet dset = new DataSet();
                DataView dv2 = new DataView();
                DataView dv3 = new DataView();
                ArrayList add = new ArrayList();
                DataRow drow1 = null;
                string course = "";
                string course1 = "";

                dt1.Columns.Add("S.No", typeof(string));
                dt1.Columns.Add("BRANCH", typeof(string));
                dt1.Columns.Add("Total", typeof(string));
                dt1.Columns.Add("Attended", typeof(string));
                dt1.Columns.Add("Passed", typeof(string));
                dt1.Columns.Add("Failed", typeof(string));
                dt1.Columns.Add("Absent", typeof(string));
                dt1.Columns.Add("Pass Percentage", typeof(double));

                for (int ij = 0; ij < chcklisttestname.Items.Count; ij++)
                {
                    if (chcklisttestname.Items[ij].Selected == true)
                    {
                        count = count + 1;

                        course = chcklisttestname.Items[ij].Value.ToString();
                        course1 = chcklisttestname.Items[ij].Text;
                        g = ij;

                        //if (course1 == "")
                        //{
                        //    course1 = course;
                        //}
                        //else
                        //{
                        //    course1 = course1 + "'" + "," + "'" + course;
                        //}                      
                        //for (int m = 0; m < chcklisttestname.Items.Count; m++)
                        //{
                        //    //course = chcklisttestname.Items[ij].Value.ToString();
                        //    //course1 = chcklisttestname.Items[ij].Text;
                        //}

                        //string strquery1 = "select count(distinct r.roll_no) as Pass, rt.degree_code from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code select count(distinct r.roll_no) as Fail, rt.degree_code from result r,subjectchooser su,exam_type ex,registration rt,criteriaforinternal c where r.roll_no=rt.roll_no and ex.exam_code=r.exam_code and r.roll_no=su.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and ex.subject_no=su.subject_no and (r.marks_obtained<ex.min_mark and r.marks_obtained<>'-3' and r.marks_obtained<>'-2' or r.marks_obtained<>'-1') and r.marks_obtained='-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code select count(distinct r.roll_no) as Absent, rt.degree_code from result r,registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code select count(distinct rt.roll_no) as Attended, rt.degree_code from result r, registration rt, subjectchooser su, exam_type ex , criteriaforinternal c where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "')  group by rt.degree_code select count(distinct rt.roll_no) as Total, rt.degree_code from result r,registration rt,subjectchooser su,exam_type ex ,criteriaforinternal c where r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code";
                        // string strquery1 = "select  count(distinct r.roll_no) as Pass, rt.degree_code from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and  rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained>ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and  c.criteria in ('" + course1 + "') and r.roll_no not in(select distinct r.roll_no from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and  rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and  c.criteria in ('" + course1 + "'))group by rt.degree_code select count(distinct r.roll_no) as Fail, rt.degree_code from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and  rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2')   and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and  c.criteria in ('" + course1 + "')group by rt.degree_code select count(distinct r.roll_no) as Absent, rt.degree_code from result r,registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code select count(distinct rt.roll_no) as Attended, rt.degree_code from result r, registration rt,subjectchooser su, exam_type ex , criteriaforinternal c where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "')  group by rt.degree_code select count(distinct rt.roll_no) as Total, rt.degree_code from result r,registration rt,subjectchooser su,exam_type ex ,criteriaforinternal c where r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('Cs')  group by rt.degree_code";

                        int sno1 = 0;
                        DataSet ds = new DataSet();
                        //string strquery1 = "select count(distinct r.roll_no) as 'pass',rt.degree_code from result r,exam_type ex,subjectchooser su,subject s,registration rt ,CriteriaForInternal c where r.roll_no=rt.roll_no and r.roll_no=su.roll_no and su.subject_no=ex.subject_no and s.subType_no=su.subtype_no and s.subject_no=su.subject_no and s.subject_no=ex.subject_no and c.Criteria_no=ex.criteria_no and r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.Current_Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') and rt.Roll_No not in (select distinct rt.roll_no from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where su.Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.Current_Semester=su.semester and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1' and rt.cc=0 and rt.exam_flag  <> 'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "')) and rt.Roll_No not in (select distinct rt.roll_no from result r,registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no  and  rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and  ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and su.Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "')) group by rt.degree_code";
                        //strquery1 = strquery1 + " select count(distinct rt.roll_no) as fail, rt.degree_code from result r,exam_type ex,subjectchooser su,registration rt,criteriaforinternal c where su.Semester= '" + dropsem.SelectedValue.ToString() + "' and r.roll_no=rt.roll_no and r.roll_no=su.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and su.subject_no=ex.subject_no and  r.exam_code=ex.exam_code and (r.marks_obtained<ex.min_mark or  r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained>='-1' and rt.cc=0 and rt.exam_flag <> 'DEBAR' and rt.delflag=0 and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code";
                        //strquery1 = strquery1 + " select count(distinct rt.roll_no) as absent, rt.degree_code from result r,registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no  and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and su.Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code";
                        //strquery1 = strquery1 + " select count(distinct rt.roll_no) as Attended, rt.degree_code from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no  and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and su.Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') and rt.Roll_No not in(select distinct rt.Roll_No from result r,registration rt,exam_type ex,subjectchooser su ,criteriaforinternal c where r.marks_obtained<0 and (r.marks_obtained<>'-2' and  r.marks_obtained<>'-3' and r.marks_obtained<>'-7' ) and r.roll_no=su.roll_no  and rt.college_code=rt.college_code and su.subject_no=ex.subject_no and r.exam_code=ex.exam_code and ex.criteria_no=c.Criteria_no and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and rt.RollNo_Flag<>0 and su.Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year= '" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "')) group by rt.degree_code";
                        //strquery1 = strquery1 + " select count(distinct rt.roll_no) as total, rt.degree_code from result r, registration rt,subjectchooser su,exam_type ex , criteriaforinternal c where (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and marks_obtained<>'-1' and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no  and ex.exam_code=r.exam_code and su.roll_no=r.roll_no and rt.college_code=rt.college_code and  ex.criteria_no=c.Criteria_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and su.Semester= '" + dropsem.SelectedValue.ToString() + "' and rt.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and c.criteria in ('" + course1 + "') group by rt.degree_code";
                        //dset = da.select_method(strquery1, hat, "Text");
                        // if (dset.Tables[0].Rows.Count > 0 || dset.Tables[1].Rows.Count > 0 || dset.Tables[2].Rows.Count > 0 || dset.Tables[3].Rows.Count > 0 || dset.Tables[4].Rows.Count > 0)
                        {
                            //dt1.Columns.Add("S.No", typeof(string));
                            //dt1.Columns.Add("BRANCH", typeof(string));
                            //dt1.Columns.Add("Total", typeof(string));
                            //dt1.Columns.Add("Attended", typeof(string));
                            //dt1.Columns.Add("Passed", typeof(string));
                            //dt1.Columns.Add("Failed", typeof(string));
                            //dt1.Columns.Add("Absent", typeof(string));
                            //dt1.Columns.Add("Pass Percentage", typeof(double));

                            //int g1 = 0;

                            DataRow dr11 = null;
                            dr11 = dt1.NewRow();
                            dr11[0] = course1;
                            dt1.Rows.Add(dr11);
                            add.Add(dt1.Rows.Count);

                            for (int i = 0; i < chcklistbranch.Items.Count; i++)
                            {
                                if (chcklistbranch.Items[i].Selected == true)
                                {
                                    branch = chcklistbranch.Items[i].Text;
                                    branchval1 = chcklistbranch.Items[i].Value;
                                    cnt = cnt + 1;
                                    drow1 = dt1.NewRow();
                                    sno1 = sno1 + 1;
                                    drow1[0] = sno1;
                                    string acr = "select Acronym from Degree where Degree_Code=" + branchval1 + "";
                                    ds = da.select_method_wo_parameter(acr, "text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        drow1[1] = ds.Tables[0].Rows[0]["Acronym"].ToString();
                                    }

                                    string strqueryval = "select count(distinct re1.roll_no) as Total from Registration r1,syllabus_master sy1,CriteriaForInternal c1,Exam_type e1,Result re1 where r1.Batch_Year=sy1.Batch_Year and r1.degree_code=sy1.degree_code and r1.Batch_Year=e1.batch_year and r1.Roll_No=re1.roll_no and sy1.syll_code=c1.syll_code and c1.Criteria_no=e1.criteria_no and e1.exam_code=re1.exam_code and r1.cc=0 and r1.DelFlag=0 and r1.Exam_Flag<>'debar' and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' and  c1.criteria='" + course1 + "' ";
                                    strqueryval = strqueryval + " select Count(distinct re1.roll_no) as Attended from Registration r1,syllabus_master sy1,CriteriaForInternal c1,Exam_type e1,Result re1 where r1.Batch_Year=sy1.Batch_Year and r1.degree_code=sy1.degree_code and r1.Batch_Year=e1.batch_year and r1.Roll_No=re1.roll_no and sy1.syll_code=c1.syll_code and r1.cc=0 and r1.DelFlag=0 and r1.Exam_Flag<>'debar' and c1.Criteria_no=e1.criteria_no and e1.exam_code=re1.exam_code and re1.marks_obtained<>'-1' and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' and  c1.criteria='" + course1 + "'";
                                    strqueryval = strqueryval + " select Count(distinct re.roll_no) as pass from Registration r,Exam_type e,result re,CriteriaForInternal c,syllabus_master sy,subject s where r.Roll_No=re.roll_no and r.batch_year=e.batch_year and r.Batch_Year=sy.Batch_Year and r.degree_code=sy.degree_code and sy.syll_code=c.syll_code and e.criteria_no=c.Criteria_no and e.exam_code=re.exam_code and s.syll_code=sy.syll_code and s.subject_no=e.subject_no and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "' and c.criteria='" + course1 + "' and r.degree_code='" + branchval1 + "' and re.marks_obtained>=e.min_mark and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' and re.roll_no not in(select re1.roll_no from Registration r1,syllabus_master sy1,CriteriaForInternal c1,Exam_type e1,Result re1 where r1.Batch_Year=sy1.Batch_Year and r1.degree_code=sy1.degree_code and r1.Batch_Year=e1.batch_year and r1.Roll_No=re1.roll_no and sy1.syll_code=c1.syll_code and c1.Criteria_no=e1.criteria_no and e1.exam_code=re1.exam_code  and (re1.marks_obtained<e1.min_mark and re1.marks_obtained<>'-3' ) and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' and  c1.criteria='" + course1 + "')";
                                    strqueryval = strqueryval + " select count( distinct re1.roll_no) as 'Fail' from Registration r1,syllabus_master sy1,CriteriaForInternal c1,Exam_type e1,Result re1 where r1.Batch_Year=sy1.Batch_Year and r1.degree_code=sy1.degree_code and r1.Batch_Year=e1.batch_year and r1.Roll_No=re1.roll_no and sy1.syll_code=c1.syll_code and c1.Criteria_no=e1.criteria_no and e1.exam_code=re1.exam_code  and re1.marks_obtained<e1.min_mark and r1.cc=0 and r1.DelFlag=0 and r1.Exam_Flag<>'debar' and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' and  c1.criteria='" + course1 + "'";
                                    strqueryval = strqueryval + " select count(distinct re1.roll_no) as Absent from Registration r1,syllabus_master sy1,CriteriaForInternal c1,Exam_type e1,Result re1 where r1.Batch_Year=sy1.Batch_Year and r1.degree_code=sy1.degree_code and r1.Batch_Year=e1.batch_year and r1.Roll_No=re1.roll_no and sy1.syll_code=c1.syll_code and c1.Criteria_no=e1.criteria_no and e1.exam_code=re1.exam_code and re1.marks_obtained='-1' and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' and  c1.criteria='" + course1 + "' and r1.cc=0 and r1.DelFlag=0 and r1.Exam_Flag<>'debar'";
                                    DataSet dsdetailcoll = d2.select_method_wo_parameter(strqueryval, "Text");

                                    // dset.Tables[3].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                    //dv2 = dset.Tables[3].DefaultView;
                                    dsdetailcoll.Tables[1].DefaultView.RowFilter = "";
                                    dv2 = dsdetailcoll.Tables[1].DefaultView;
                                    if (dv2.Count > 0)
                                    {
                                        drow1[3] = dv2[0]["Attended"].ToString();

                                        tot12 = Convert.ToInt32(dv2[0]["Attended"]);
                                        if (tott1 != tot12)
                                        {
                                            if (tott1 == 0)
                                            {
                                                tott1 = tot12;
                                            }
                                            else
                                            {
                                                tott1 = tot + tot12;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        drow1[3] = "0";
                                        tot12 = 0;
                                    }
                                    //dset.Tables[0].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                    //dv3 = dset.Tables[0].DefaultView;
                                    dsdetailcoll.Tables[2].DefaultView.RowFilter = "";
                                    dv3 = dsdetailcoll.Tables[2].DefaultView;
                                    if (dv3.Count > 0)
                                    {
                                        drow1[4] = dv3[0]["Pass"].ToString();
                                        tot11 = Convert.ToInt32(dv3[0]["Pass"]);
                                        if (tott != tot11)
                                        {
                                            if (tott == 0)
                                            {
                                                tott = tot11;
                                            }
                                            else
                                            {
                                                tott = tott + tot11;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        drow1[4] = "0";
                                        tot11 = 0;
                                    }
                                    // dset.Tables[1].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                    // DataView dv4 = dset.Tables[1].DefaultView;
                                    dsdetailcoll.Tables[3].DefaultView.RowFilter = "";
                                    DataView dv4 = dsdetailcoll.Tables[3].DefaultView;
                                    if (dv4.Count > 0)
                                    {
                                        drow1[5] = dv4[0]["Fail"].ToString();
                                        tot13 = Convert.ToInt32(dv4[0]["Fail"]);
                                        if (tott2 != tot13)
                                        {
                                            if (tott2 == 0)
                                            {
                                                tott2 = tot13;
                                            }
                                            else
                                            {
                                                tott2 = tott2 + tot13;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        drow1[5] = "0";
                                        tot13 = 0;
                                    }
                                    //dset.Tables[2].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                    //DataView dv5 =dset.Tables[2].DefaultView;
                                    dsdetailcoll.Tables[4].DefaultView.RowFilter = "";
                                    DataView dv5 = dsdetailcoll.Tables[4].DefaultView;
                                    if (dv5.Count > 0)
                                    {
                                        drow1[6] = dv5[0]["Absent"].ToString();
                                        tot14 = Convert.ToInt32(dv5[0]["Absent"]);
                                        if (tott3 != tot14)
                                        {
                                            if (tott3 == 0)
                                            {
                                                tott3 = tot14;
                                            }
                                            else
                                            {
                                                tott3 = tott3 + tot14;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        drow1[6] = "0";
                                        tot14 = 0;
                                    }
                                    //dset.Tables[4].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                    //DataView dv6 = dset.Tables[4].DefaultView;
                                    dsdetailcoll.Tables[0].DefaultView.RowFilter = "";
                                    DataView dv6 = dsdetailcoll.Tables[0].DefaultView;
                                    if (dv6.Count > 0)
                                    {
                                        //drow1[2] = dv6[0]["Total"].ToString();
                                        //tot1 = Convert.ToInt32(dv6[0]["Total"]);

                                        drow1[2] = dv6[0]["Total"].ToString();
                                        tot1 = Convert.ToInt32(dv6[0]["Total"]);
                                        if (tot != tot1)
                                        {
                                            if (tot == 0)
                                            {
                                                tot = tot1;
                                            }
                                            else
                                            {
                                                tot = tot + tot1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        drow1[2] = "0";
                                        tot1 = 0;
                                    }
                                    string finalpercent = "";
                                    if (dv3.Count > 0 && dv2.Count > 0)
                                    {
                                        int pas = (Convert.ToInt32(dv3[0]["Pass"].ToString()));
                                        int total = (Convert.ToInt32(dv2[0]["Attended"].ToString()));
                                        double passpercentage = 0;
                                        passpercentage = (((Convert.ToDouble(pas)) / (Convert.ToDouble(total)) * 100));
                                        double passper = Math.Round(passpercentage, 2);
                                        finalpercent = Convert.ToString(passper);

                                        if (finalpercent != "")
                                        {
                                            drow1[7] = finalpercent;
                                        }
                                        g = i;
                                    }
                                    else
                                    {
                                        drow1[7] = "0";
                                    }
                                    dt1.Rows.Add(drow1);
                                    t1 = tot1 + t1;
                                    t2 = tot12 + t2;
                                    t3 = tot11 + t3;
                                    t4 = tot13 + t4;
                                    t5 = tot14 + t5;
                                }
                                g = cnt;
                            }
                            if (cnt > 0)
                            {
                                dr = dt1.NewRow();
                                dr[0] = "Total";
                                dr[2] = Convert.ToString(Math.Round(Convert.ToDouble(t1)));
                                dr[3] = Convert.ToString(Math.Round(Convert.ToDouble(t2)));
                                dr[4] = Convert.ToString(Math.Round(Convert.ToDouble(t3)));
                                dr[5] = Convert.ToString(Math.Round(Convert.ToDouble(t4)));
                                dr[6] = Convert.ToString(Math.Round(Convert.ToDouble(t5)));
                                dt1.Rows.Add(dr);

                                // r
                                add.Add(dt1.Rows.Count + "-" + "Total");
                                // r

                            }

                            // c
                            string gpercent = "";
                            if (t3 > 0 && t2 > 0)
                            {
                                int passs = t3;
                                int totall = t2;
                                double ppercentage = 0;
                                ppercentage = (((Convert.ToDouble(passs)) / (Convert.ToDouble(totall)) * 100));
                                double passpr = Math.Round(ppercentage, 2);
                                gpercent = Convert.ToString(passpr);

                                if (gpercent != "")
                                {
                                    dr[7] = gpercent;
                                }
                            }
                            else
                            {
                                dr[7] = "0";
                            }
                            // c

                            deptwiseresultanalysisgrid.DataSource = dt1;
                            deptwiseresultanalysisgrid.DataBind();
                            t1 = 0;
                            t2 = 0;
                            t3 = 0;
                            t4 = 0;
                            t5 = 0;

                            // r
                            if (deptwiseresultanalysisgrid.Rows.Count > 0)
                            {
                                //individualreprtsportsgrid.Columns[0].ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                                deptwiseresultanalysisgrid.Visible = true;
                                deptwiseresultanalysisexternalgrid.Visible = false;
                                g2btnexcel.Visible = true;
                                g2btnprint.Visible = true;
                                g1btnprint.Visible = false;
                                g1btnexcel.Visible = false;
                            }
                            else
                            {
                                lblerror.Visible = true;
                                lblerror.Text = "No Records Found";
                                deptwiseresultanalysisgrid.Visible = false;
                                g1btnprint.Visible = false;
                                g1btnexcel.Visible = false;
                                g2btnprint.Visible = false;
                                g2btnexcel.Visible = false;
                            }
                            // r

                            // r
                            if (add.Count > 0)
                            {
                                for (int a = 0; a < add.Count; a++)
                                {
                                    string row = Convert.ToString(add[a]);
                                    int row1 = 0;
                                    if (row.Contains("-") == true)
                                    {
                                        string[] split = row.Split('-');
                                        if (split.Length > 0)
                                        {
                                            // r
                                            row1 = Convert.ToInt32(split[0]);
                                            row1 = row1 - 1;
                                            // r
                                            deptwiseresultanalysisgrid.Rows[row1].Cells[0].ColumnSpan = 2;
                                            deptwiseresultanalysisgrid.Rows[row1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                            deptwiseresultanalysisgrid.Rows[row1].Cells[0].ForeColor = System.Drawing.Color.Black;
                                            deptwiseresultanalysisgrid.Rows[row1].Cells[1].Visible = false;
                                            deptwiseresultanalysisexternalgrid.Visible = false;

                                            //deptwiseresultanalysisgrid.Rows[deptwiseresultanalysisgrid.Rows.Count - 1].Cells[0].ColumnSpan = 2;
                                            //deptwiseresultanalysisgrid.Rows[deptwiseresultanalysisgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                            //deptwiseresultanalysisgrid.Rows[deptwiseresultanalysisgrid.Rows.Count - 1].Cells[0].ForeColor = Color.Black;
                                            //deptwiseresultanalysisgrid.Rows[deptwiseresultanalysisgrid.Rows.Count - 1].Cells[1].Visible = false;
                                            //deptwiseresultanalysisgrid.Visible = true;
                                            //g2btnexcel.Visible = true;
                                            //g2btnprint.Visible = true;
                                            //lblerror.Visible = false;                                          
                                            //g1btnexcel.Visible = false;
                                            //g1btnprint.Visible = false;
                                        }
                                    }
                                    else
                                    {
                                        row1 = Convert.ToInt32(row) - 1;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[0].ColumnSpan = 8;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[0].ForeColor = System.Drawing.Color.Black;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[1].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[2].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[3].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[4].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[5].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[6].Visible = false;
                                        deptwiseresultanalysisgrid.Rows[row1].Cells[7].Visible = false;
                                        //deptwiseresultanalysisgrid.Rows[row1].Cells[8].Visible = false;
                                    }
                                }
                            }
                            // r
                        }
                        //else
                        //{
                        //    lblerror.Visible = true;
                        //    lblerror.Text = "No Records Found";
                        //    deptwiseresultanalysisgrid.Visible = false;
                        //    deptwiseresultanalysisexternalgrid.Visible = false;
                        //    g1btnprint.Visible = false;
                        //    g1btnexcel.Visible = false;
                        //    g2btnprint.Visible = false;
                        //    g2btnexcel.Visible = false;
                        //}
                    }
                    //else
                    //{
                    //    lblerror.Visible = true;
                    //    lblerror.Text = "No Records Found";
                    //    deptwiseresultanalysisgrid.Visible = false;
                    //    deptwiseresultanalysisexternalgrid.Visible = false;
                    //    g1btnprint.Visible = false;
                    //    g1btnexcel.Visible = false;
                    //    g2btnprint.Visible = false;
                    //    g2btnexcel.Visible = false;
                    //}
                }
            }
            else
            {
                int gn = 0;
                if (dropreporttype.SelectedItem.Text == "External")
                {
                    DataRow dr = null;
                    DataTable dt1 = new DataTable();
                    DataSet dset = new DataSet();
                    DataView dv2 = new DataView();
                    DataView dv3 = new DataView();
                    DataRow drow1 = null;
                    ArrayList add = new ArrayList();
                    //    string strquery = "select COUNT(result) as Pass, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.current_semester=r.Current_Semester and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result='Pass' group by r.degree_code select COUNT(result) as Fail, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.current_semester=r.Current_Semester and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result='Fail' group by r.degree_code select COUNT(result) as Absent, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.current_semester=r.Current_Semester and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result='AA' group by r.degree_code select COUNT(result) as Attended, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.current_semester=r.Current_Semester and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result!='AA' group by r.degree_code select COUNT(r.roll_no)as Total, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.degree_code=r.degree_code and e.batch_year=r.Batch_Year and e.current_semester=r.Current_Semester and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 group by r.degree_code";
                    //string strquery = "select COUNT(distinct r.roll_no) as Pass, r.degree_code from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.college_code=" + ddcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='pass' and m.attempts=1 and m.roll_no not in        (    select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.college_code=" + ddcollege.SelectedValue + " and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result in ('fail','AAA') and m.attempts=1 )  group by r.degree_code  select COUNT(distinct r.roll_no) as fail, r.degree_code from mark_entry m,Exam_Details e,Registration r   where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + ddcollege.SelectedValue + "  and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and    r.exam_flag <>'DEBAR'  and r.delflag=0 and result='fail' and m.attempts=1 group by r.degree_code  select COUNT(distinct r.roll_no) as Absent, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + ddcollege.SelectedValue + "  and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR'  and r.delflag=0 and result='AAA' and m.attempts=1 group by r.degree_code  select COUNT(distinct m.roll_no) as Attended, r.degree_code from mark_entry m,Exam_Details e,Registration r where  e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + ddcollege.SelectedValue + "  and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and m.attempts=1 and m.roll_no not in (select distinct r.roll_no from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and r.college_code=" + ddcollege.SelectedValue + " and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result='AAA' and m.attempts=1)  group by r.degree_code  select COUNT( distinct r.roll_no)as Total, r.degree_code from mark_entry m,Exam_Details e,Registration r where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.degree_code=r.degree_code and e.batch_year=r.Batch_Year and r.college_code=" + ddcollege.SelectedValue + "  and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and e.current_semester='" + dropsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and m.attempts=1 group by r.degree_code";
                    // dset = da.select_method(strquery, hat, "Text");
                    // if (dset.Tables[0].Rows.Count > 0 || dset.Tables[1].Rows.Count > 0 || dset.Tables[2].Rows.Count > 0 || dset.Tables[3].Rows.Count > 0 || dset.Tables[4].Rows.Count > 0)
                    {
                        dt1.Columns.Add("S.No", typeof(string));
                        dt1.Columns.Add("BRANCH", typeof(string));
                        dt1.Columns.Add("Total", typeof(string));
                        dt1.Columns.Add("Attended", typeof(string));
                        dt1.Columns.Add("Passed", typeof(string));
                        dt1.Columns.Add("Failed", typeof(string));
                        dt1.Columns.Add("Absent", typeof(string));
                        dt1.Columns.Add("Pass Percentage", typeof(double));

                        for (int i = 0; i < chcklistbranch.Items.Count; i++)
                        {
                            if (chcklistbranch.Items[i].Selected == true)
                            {
                                branch = chcklistbranch.Items[i].Text;
                                branchval1 = chcklistbranch.Items[i].Value;


                                string strextertotquery = "select Count(distinct m.roll_no) as Total from Registration r,Exam_Details ed,mark_entry m,syllabus_master sy,subject s where r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Roll_No=m.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and ed.degree_code=sy.degree_code and ed.batch_year=sy.Batch_Year and ed.current_semester=sy.semester and s.syll_code=sy.syll_code and m.subject_no=s.subject_no and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.degree_code='" + branchval1 + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "'";
                                strextertotquery = strextertotquery + " select Count(distinct m.roll_no) as Attended from Registration r,Exam_Details ed,mark_entry m,syllabus_master sy,subject s where r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Roll_No=m.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and ed.degree_code=sy.degree_code and ed.batch_year=sy.Batch_Year and ed.current_semester=sy.semester and m.subject_no=s.subject_no and s.syll_code=sy.syll_code  and m.result<>'AAA' and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.degree_code='" + branchval1 + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "' ";
                                strextertotquery = strextertotquery + " select Count(distinct m.roll_no) as Pass from Registration r,Exam_Details ed,mark_entry m,syllabus_master sy,subject s where r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Roll_No=m.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and ed.degree_code=sy.degree_code and ed.batch_year=sy.Batch_Year and ed.current_semester=sy.semester and m.subject_no=s.subject_no and m.result='pass' and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.degree_code='" + branchval1 + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "' and s.syll_code=sy.syll_code  and m.roll_no not in(select m1.roll_no from Registration r1,Exam_Details ed1,mark_entry m1,syllabus_master sy1,subject s1 where r1.Batch_Year=ed1.batch_year and r1.degree_code=ed1.degree_code and r1.Roll_No=m1.roll_no and r1.degree_code=sy1.degree_code and r1.Batch_Year=ed1.batch_year and ed1.exam_code=m1.exam_code and ed1.degree_code=sy1.degree_code and ed1.batch_year=sy1.Batch_Year and ed1.current_semester=sy1.semester and s1.syll_code=sy1.syll_code and m1.subject_no=s1.subject_no and m1.result<>'pass' and r1.Batch_Year='" + dropbatch.SelectedValue.ToString() + "'  and r1.degree_code='" + branchval1 + "' and sy1.semester='" + dropsem.SelectedValue.ToString() + "' )";
                                strextertotquery = strextertotquery + " select Count(distinct m.roll_no) as fail from Registration r,Exam_Details ed,mark_entry m,syllabus_master sy,subject s where r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Roll_No=m.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and ed.degree_code=sy.degree_code and ed.batch_year=sy.Batch_Year and ed.current_semester=sy.semester and m.subject_no=s.subject_no and s.syll_code=sy.syll_code  and m.result<>'pass' and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.degree_code='" + branchval1 + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "'";
                                strextertotquery = strextertotquery + " select Count(distinct m.roll_no) as Absent from Registration r,Exam_Details ed,mark_entry m,syllabus_master sy,subject s where r.Batch_Year=ed.batch_year and r.degree_code=ed.degree_code and r.Roll_No=m.roll_no and r.degree_code=sy.degree_code and r.Batch_Year=ed.batch_year and ed.exam_code=m.exam_code and ed.degree_code=sy.degree_code and ed.batch_year=sy.Batch_Year and ed.current_semester=sy.semester and m.subject_no=s.subject_no and s.syll_code=sy.syll_code  and m.result='AAA' and r.Batch_Year='" + dropbatch.SelectedValue.ToString() + "' and r.degree_code='" + branchval1 + "' and sy.semester='" + dropsem.SelectedValue.ToString() + "'";
                                DataSet dsexter = d2.select_method_wo_parameter(strextertotquery, "Text");
                                drow1 = dt1.NewRow();
                                //drow1[0] = i + 1;
                                string acr = "select Acronym from Degree where Degree_Code=" + branchval1 + "";
                                ds = da.select_method_wo_parameter(acr, "text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    drow1[1] = ds.Tables[0].Rows[0]["Acronym"].ToString();
                                }

                                //dr = dt1.NewRow();
                                if (!add.Contains(drow1[1]))
                                {
                                    count++;
                                    add.Add(drow1[1]);
                                }
                                drow1[0] = count;

                                //dset.Tables[3].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                //dv2 = dset.Tables[3].DefaultView;

                                dsexter.Tables[1].DefaultView.RowFilter = "";
                                dv2 = dsexter.Tables[1].DefaultView;
                                if (dv2.Count > 0)
                                {
                                    drow1[3] = dv2[0]["Attended"].ToString();
                                    tot12 = Convert.ToInt32(dv2[0]["Attended"]);
                                    if (tott1 != tot12)
                                    {
                                        if (tott1 == 0)
                                        {
                                            tott1 = tot12;
                                        }
                                        else
                                        {
                                            tott1 = tot + tot12;
                                        }
                                    }
                                }
                                else
                                {
                                    drow1[3] = "0";
                                    tot12 = 0;
                                }
                                //dset.Tables[0].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                //dv3 = dset.Tables[0].DefaultView;
                                dsexter.Tables[2].DefaultView.RowFilter = "";
                                dv3 = dsexter.Tables[2].DefaultView;
                                if (dv3.Count > 0)
                                {
                                    drow1[4] = dv3[0]["Pass"].ToString();
                                    tot11 = Convert.ToInt32(dv3[0]["Pass"]);
                                    if (tott != tot11)
                                    {
                                        if (tott == 0)
                                        {
                                            tott = tot11;
                                        }
                                        else
                                        {
                                            tott = tott + tot11;
                                        }
                                    }
                                }
                                else
                                {
                                    drow1[4] = "0";
                                    tot11 = 0;
                                }
                                //dset.Tables[1].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                //DataView dv4 = dset.Tables[1].DefaultView;
                                dsexter.Tables[3].DefaultView.RowFilter = "";
                                DataView dv4 = dsexter.Tables[3].DefaultView;
                                if (dv4.Count > 0)
                                {
                                    drow1[5] = dv4[0]["Fail"].ToString();
                                    tot13 = Convert.ToInt32(dv4[0]["Fail"]);
                                    if (tott2 != tot13)
                                    {
                                        if (tott2 == 0)
                                        {
                                            tott2 = tot13;
                                        }
                                        else
                                        {
                                            tott2 = tott2 + tot13;
                                        }
                                    }
                                }
                                else
                                {
                                    drow1[5] = "0";
                                    tot13 = 0;
                                }
                                //dset.Tables[2].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                //DataView dv5 = dset.Tables[2].DefaultView;
                                dsexter.Tables[4].DefaultView.RowFilter = "";
                                DataView dv5 = dsexter.Tables[4].DefaultView;
                                if (dv5.Count > 0)
                                {
                                    drow1[6] = dv5[0]["Absent"].ToString();
                                    tot14 = Convert.ToInt32(dv5[0]["Absent"]);
                                    if (tott3 != tot14)
                                    {
                                        if (tott3 == 0)
                                        {
                                            tott3 = tot14;
                                        }
                                        else
                                        {
                                            tott3 = tott3 + tot14;
                                        }
                                    }
                                }
                                else
                                {
                                    drow1[6] = "0";
                                    tot14 = 0;
                                }
                                //dset.Tables[4].DefaultView.RowFilter = "degree_code='" + branchval1 + "'";
                                //DataView dv6 = dset.Tables[4].DefaultView;
                                dsexter.Tables[0].DefaultView.RowFilter = "";
                                DataView dv6 = dsexter.Tables[0].DefaultView;
                                if (dv6.Count > 0)
                                {
                                    drow1[2] = dv6[0]["Total"].ToString();
                                    tot1 = Convert.ToInt32(dv6[0]["Total"]);
                                    if (tot != tot1)
                                    {
                                        gn++;
                                        if (tot == 0)
                                        {
                                            tot = tot1;
                                        }
                                        else
                                        {
                                            tot = tot + tot1;
                                        }
                                    }
                                }
                                else
                                {
                                    drow1[2] = "0";
                                    tot1 = 0;

                                }

                                string finalpercent = "";
                                if (dv3.Count > 0 && dv2.Count > 0)
                                {
                                    int pas = (Convert.ToInt32(dv3[0]["Pass"].ToString()));
                                    int total = (Convert.ToInt32(dv2[0]["Attended"].ToString()));
                                    double passpercentage = 0;
                                    passpercentage = (((Convert.ToDouble(pas)) / (Convert.ToDouble(total)) * 100));
                                    double passper = Math.Round(passpercentage, 2);
                                    finalpercent = Convert.ToString(passper);

                                    if (finalpercent != "")
                                    {
                                        drow1[7] = finalpercent;
                                    }
                                }
                                else
                                {
                                    drow1[7] = "0";
                                }
                                dt1.Rows.Add(drow1);
                                t1 = tot1 + t1;
                                t2 = tot12 + t2;
                                t3 = tot11 + t3;
                                t4 = tot13 + t4;
                                t5 = tot14 + t5;
                            }
                        }
                        dr = dt1.NewRow();
                        dr[0] = "Total";
                        dr[2] = Convert.ToString(Math.Round(Convert.ToDouble(t1)));
                        dr[3] = Convert.ToString(Math.Round(Convert.ToDouble(t2)));
                        dr[4] = Convert.ToString(Math.Round(Convert.ToDouble(t3)));
                        dr[5] = Convert.ToString(Math.Round(Convert.ToDouble(t4)));
                        dr[6] = Convert.ToString(Math.Round(Convert.ToDouble(t5)));
                        dt1.Rows.Add(dr);
                        // c
                        string gpercent = "";
                        if (t3 > 0 && t2 > 0)
                        {
                            int passs = t3;
                            int totall = t2;
                            double ppercentage = 0;
                            ppercentage = (((Convert.ToDouble(passs)) / (Convert.ToDouble(totall)) * 100));
                            double passpr = Math.Round(ppercentage, 2);
                            gpercent = Convert.ToString(passpr);

                            if (gpercent != "")
                            {
                                dr[7] = gpercent;
                            }
                        }
                        else
                        {
                            dr[7] = "0";
                        }
                        // c
                        if (gn != 0)
                        {
                            deptwiseresultanalysisexternalgrid.DataSource = dt1;
                            deptwiseresultanalysisexternalgrid.DataBind();
                        }
                        else
                        {
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                            deptwiseresultanalysisexternalgrid.Visible = false;
                            g1btnprint.Visible = false;
                            g1btnexcel.Visible = false;
                            g2btnprint.Visible = false;
                            g2btnexcel.Visible = false;
                            return;
                        }
                        //if (deptwiseresultanalysisexternalgrid.Rows.Count > 0)
                        //{
                        //      deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].ColumnSpan = 2;
                        deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                        deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].ForeColor = System.Drawing.Color.Black;
                        //deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].BackColor = Color.Gainsboro;
                        deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[1].Visible = false;
                        //deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[2].Visible = false;
                        //}
                        deptwiseresultanalysisexternalgrid.Visible = true;
                        g1btnexcel.Visible = true;
                        g1btnprint.Visible = true;
                        lblerror.Visible = false;
                        deptwiseresultanalysisgrid.Visible = false;
                    }
                    //else
                    //{
                    //    lblerror.Visible = true;
                    //    lblerror.Text = "No Records Found";
                    //    deptwiseresultanalysisexternalgrid.Visible = false;
                    //    g1btnprint.Visible = false;
                    //    g1btnexcel.Visible = false;
                    //    g2btnprint.Visible = false;
                    //    g2btnexcel.Visible = false;
                    //}
                }

            }
        }
        catch (Exception ex)
        {
            lblerror.Text = ex.ToString();
            lblerror.Visible = false;
        }
    }

    protected void bound(object sender, EventArgs e)
    {
        try
        {
            for (int i = deptwiseresultanalysisexternalgrid.Rows.Count - 1; i > 0; i--)
            {
                GridViewRow row = deptwiseresultanalysisexternalgrid.Rows[i];
                GridViewRow previousRow = deptwiseresultanalysisexternalgrid.Rows[i - 1];
                for (int j = 0; j <= 1; j++)
                {
                    string merge1 = row.Cells[j].Text;
                    string merge2 = previousRow.Cells[j].Text;
                    if (merge1.ToString() == merge2.ToString())
                    {
                        if (previousRow.Cells[j].RowSpan == 0)
                        {
                            if (row.Cells[j].RowSpan == 0)
                            {
                                previousRow.Cells[j].RowSpan += 2;
                            }
                            else
                            {
                                previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
                            }
                            row.Cells[j].Visible = false;
                        }
                    }
                }
            }
        }
        //if (deptwiseresultanalysisexternalgrid.Rows.Count > 0)
        //{
        //    deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //}

        catch
        {

        }
    }

    protected void bound1(object sender, EventArgs e)
    {
        try
        {

            //GridViewRow row = new GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Normal);
            //TableHeaderCell cell = new TableHeaderCell();
            //cell.Text = "KONGU ENGINEERING COLLEGE";
            //cell.ColumnSpan = 8;
            //row.Controls.Add(cell);
            //deptwiseresultanalysisgrid.HeaderRow.Parent.Controls.AddAt(0, row);
            //if (deptwiseresultanalysisexternalgrid.Rows.Count > 0)
            //{
            //    deptwiseresultanalysisexternalgrid.Rows[deptwiseresultanalysisexternalgrid.Rows.Count - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            //}
        }
        catch
        {

        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
        }
    }


    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[7].HorizontalAlign = HorizontalAlign.Center;
        }
    }

}

