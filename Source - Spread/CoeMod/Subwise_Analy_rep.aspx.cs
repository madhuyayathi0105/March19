using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using InsproDataAccess;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;



public partial class Subwise_Analy_rep : System.Web.UI.Page
{

    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string val = "";
    string examcode = "";
    string vall2 = "";
    string mainvalue = "";
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    Hashtable hast = new Hashtable();
    InsproDirectAccess dir = new InsproDirectAccess();


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
            lblnorec.Text = "";
            // txtexcelname.Text = "";
            lblmsg.Text = "";
            Label1.Visible = false;

            if (!IsPostBack)
            {
                bindcollege();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                bindtestname();
                // Internalchat.Visible = false;
                // Externalchart.Visible = false;
                lblmsg.Visible = false;
                //    btngo_OnClick(sender, e);

            }
            if (ddlrepttype.SelectedItem.Text == "External")
            {
                first.Attributes.Add("style", "display:none");

            }
            else
            {
                first.Attributes.Add("style", "display:block");
            }
        }
        catch
        {
        }
    }

    protected void log_OnClick(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }


    public void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = "";
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
            hast.Clear();
            hast.Add("column_field", columnfield.ToString());
            ds = da.select_method("bind_college", hast, "sp");
            ddlclg.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {

                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
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
            ddlbatch.Items.Clear();
            ds = da.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
            int count1 = ds.Tables[1].Rows.Count;
            if (count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                ddlbatch.SelectedValue = max_bat.ToString();

            }

            ddlbatch.Text = "batch(" + 1 + ")";




        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlsemselect(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        bindsec();
        bindtestname();
        internalgrid.Visible = false;
        Externalgrid.Visible = false;
        Chart1.Visible = false;
        Externalchart.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        fpspread.Visible = false;

        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }
    protected void ddlsecselect(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        //  bindsec();
        bindtestname();
        internalgrid.Visible = false;
        Externalgrid.Visible = false;
        Chart1.Visible = false;
        Externalchart.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        fpspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }
    protected void ddlbatch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        bindsem();
        bindsec();
        bindtestname();
        internalgrid.Visible = false;
        Chart1.Visible = false;
        Externalchart.Visible = false;
        Externalgrid.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        fpspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }

    protected void ddlrepttype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        if (ddlrepttype.SelectedItem.Text == "Internal")
        {
            txttest.Visible = true;
            pnltest.Visible = true;
            cbtest.Visible = true;
            cbltest.Visible = true;
            Externalgrid.Visible = false;
            Externalchart.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
        }
        else if (ddlrepttype.SelectedItem.Text == "External")
        {
            txttest.Visible = false;
            pnltest.Visible = false;
            cbtest.Visible = false;
            cbltest.Visible = false;
            internalgrid.Visible = false;
            Chart1.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
        }
        fpspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;

    }


    public void binddegree()
    {
        try
        {
            ds.Clear();
            ds = da.BindDegree(Session["single_user"].ToString(), Session["group_code"].ToString(), ddlclg.SelectedItem.Value, Session["usercode"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();
            }

            //int count1 = ds.Tables[1].Rows.Count;
            //if (count1 > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
            //    ddldegree.SelectedValue = max_bat.ToString();

            //}
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        bindbranch();
        bindsem();
        bindsec();
        bindtestname();
        internalgrid.Visible = false;
        Chart1.Visible = false;
        Externalchart.Visible = false;
        Externalgrid.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        fpspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }
    protected void ddlclg_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        binddegree();
        bindbranch();
        bindsem();
        bindtestname();
        internalgrid.Visible = false;
        Externalgrid.Visible = false;
        Chart1.Visible = false;
        Externalchart.Visible = false;
        Excel.Visible = false;
        Print.Visible = false;
        fpspread.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Visible = false;
    }
    public void bindbranch()
    {
        try
        {

            string mainvalue = ddldegree.SelectedValue;
            cblbranch.Items.Clear();
            if (mainvalue.Trim() != "")
            {
                ds.Clear();
                ds = da.BindBranchMultiple(Session["single_user"].ToString(), Session["group_code"].ToString(), mainvalue, ddlclg.SelectedItem.Value, Session["usercode"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblbranch.DataSource = ds;
                    cblbranch.DataTextField = "dept_name";
                    cblbranch.DataValueField = "degree_code";
                    cblbranch.DataBind();
                }

            }
            else
            {
                cblbranch.Items.Clear();
            }

            if (cblbranch.Items.Count > 0)
            {
                int cout = 0;
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cout++;
                    cblbranch.Items[i].Selected = true;
                    if (cblbranch.Items[i].Selected == false)
                    {
                        txtbranch.Text = "---Select---";
                    }

                    else
                    {
                        cbbranch.Checked = true;
                        txtbranch.Text = "Branch(" + cout + ")";
                    }
                }

            }
            if (cbbranch.Checked == false)
            {
                cbtest.Checked = false;

                txttest.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }


    public void bindsem()
    {
        //try
        //{
        //    Boolean first_year;
        //    first_year = false;
        //    string mainvalue = "";
        //    if (cblbranch.Items.Count > 0)
        //    {
        //        for (int i = 0; i < cblbranch.Items.Count; i++)
        //        {
        //            if (cblbranch.Items[i].Selected == true)
        //            {
        //                if (mainvalue == "")
        //                {
        //                    mainvalue = cblbranch.Items[i].Value;
        //                }
        //                else
        //                {
        //                    mainvalue = mainvalue + "," + cblbranch.Items[i].Value;
        //                }
        //            }

        //        }


        //    }
        //    ddlsem.Items.Clear();
        //    ds.Clear();
        //    if (mainvalue.Trim() != "")
        //    {
        //        ds = da.BindSem(mainvalue, ddlbatch.SelectedItem.Text, ddlclg.SelectedItem.Value);

        //        int count5 = ds.Tables[0].Rows.Count;
        //        if (count5 > 0)
        //        {
        //            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
        //            count5 = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
        //            for (int i = 1; i <= count5; i++)
        //            {
        //                if (first_year == false)
        //                {
        //                    ddlsem.Items.Add(i.ToString());
        //                }
        //                else if (first_year == true && i != 2)
        //                {
        //                    ddlsem.Items.Add(i.ToString());
        //                }
        //                //ddlsem.Items.Add(Convert.ToString(i));
        //            }
        //            ddlsem.Enabled = true;
        //        }
        //        else
        //        {
        //            ddlsem.Enabled = false;
        //        }
        //    }
        //    else
        //    {
        //        ddlsem.Enabled = false;
        //    }

        //}
        //catch (Exception ex)
        //{

        //}
        try
        {
            ddlsem.Items.Clear();
            Boolean first_year;
            first_year = false;
            int duration = 0;
            int i = 0;
            string strquery = "";
            string strbranch = "";
            for (int b = 0; b < cblbranch.Items.Count; b++)
            {
                if (cblbranch.Items[b].Selected == true)
                {
                    if (strbranch.Trim() == "")
                    {
                        strbranch = cblbranch.Items[b].Value;
                    }
                    else
                    {
                        strbranch = strbranch + ',' + cblbranch.Items[b].Value;
                    }
                }
            }
            if (strbranch.Trim() != "")
            {
                strbranch = " and degree_code in(" + strbranch + ")";
            }

            strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlclg.SelectedValue.ToString() + " and batch_year=" + ddlbatch.SelectedItem.Text + " " + strbranch + " order by NDurations desc";
            ds.Reset();
            ds.Dispose();
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
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
                ddlsem.Enabled = true;
            }
            else
            {
                strquery = "select distinct duration,first_year_nonsemester  from degree where college_code=" + ddlclg.SelectedValue.ToString() + " " + strbranch + " order by duration desc";
                ds.Reset();
                ds.Dispose();
                ds = da.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
                    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());

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
        catch (Exception ex)
        {
        }
    }

    public void bindsec()
    {
        try
        {
            string mainvalue = "";
            if (cblbranch.Items.Count > 0)
            {
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    if (cblbranch.Items[i].Selected == true)
                    {
                        if (mainvalue == "")
                        {
                            mainvalue = cblbranch.Items[i].Value;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + cblbranch.Items[i].Value;
                        }
                    }

                }


            }
            ddlsec.Items.Clear();
            //hast.Clear();
            //hast.Add("batch_year", ddlbatch.SelectedValue.ToString());
            //hast.Add("degree_code", mainvalue);
            ds = da.select_method_wo_parameter("select distinct sections from registration where batch_year=" + ddlbatch.SelectedValue.ToString() + " and degree_code in (" + mainvalue + ") and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' ", "text");
            int count5 = ds.Tables[0].Rows.Count;
            if (count5 > 0)
            {

                ddlsec.DataSource = ds;
                ddlsec.DataTextField = "sections";
                ddlsec.DataValueField = "sections";
                ddlsec.DataBind();
                ddlsec.Items.Insert(0, "ALL");
                ddlsec.Enabled = true;

            }
            else
            {
                ddlsec.Enabled = false;
            }
        }
        catch (Exception ex)
        {

        }
    }



    protected void bindtestname()
    {
        try
        {
            cbltest.Items.Clear();

            string build6 = "";
            string buildvalue6 = "";
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    build6 = cblbranch.Items[i].Value.ToString();
                    if (buildvalue6 == "")
                    {
                        buildvalue6 = build6;
                    }
                    else
                    {
                        buildvalue6 = buildvalue6 + "'" + "," + "'" + build6;

                    }
                }
            }

            ds = da.select_method_wo_parameter("select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in('" + buildvalue6 + "')  and semester='" + ddlsem.SelectedValue.ToString() + "' and syllabus_year in (select syllabus_year from syllabus_master where degree_code in ('" + buildvalue6 + "') and semester ='" + ddlsem.SelectedValue.ToString() + "') and batch_year='" + ddlbatch.SelectedValue.ToString() + "' and batch_year='" + ddlbatch.SelectedValue.ToString() + "' order by criteria ", "Text");
            // ds = da.select_method_wo_parameter("select distinct criteria from criteriaforinternal,syllabus_master where criteriaforinternal.syll_code=syllabus_master.syll_code and degree_code in (45) and semester=1  and syllabus_year in (select syllabus_year from syllabus_master where degree_code in (45) and semester =1    and batch_year=2013)   and batch_year=2013 order by criteria", "Text");

            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                cbltest.DataSource = ds;
                cbltest.DataTextField = "criteria";
                cbltest.DataValueField = "criteria";
                cbltest.DataBind();
            }

            if (cbltest.Items.Count > 0)
            {
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = true;

                }
                cbtest.Checked = true;
                txttest.Text = "Test Name(" + cout + ")";

            }
            else
            {
                cbtest.Checked = false;
                txttest.Text = "---Select---";
            }

        }
        catch (Exception ex)
        {

        }
    }


    protected void cbbranch_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (cbbranch.Checked == true)
            {
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                string hd = "";
                string hed = "";
                int cout = 0;
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cout++;
                    cblbranch.Items[i].Selected = true;
                    hd = cblbranch.Items[i].Value.ToString();
                    if (hed == "")
                    {
                        hed = hd;
                    }
                    else
                    {
                        hed = hed + "'" + "," + "'" + hd;

                    }
                }
                cbbranch.Checked = true;
                txtbranch.Text = "Branch(" + cout + ")";
                //   binddegree();

            }

            else
            {
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                int cout = 0;
                for (int i = 0; i < cblbranch.Items.Count; i++)
                {
                    cout++;
                    cblbranch.Items[i].Selected = false;
                    cbbranch.Checked = false;
                    txtbranch.Text = "---Select---";
                    txttest.Text = "---Select---";
                }
                //cbbranch.Checked = false;
                //txtbranch.Text = "---Select---";

            }
            bindsem();
            bindsec();
            bindtestname();
            fpspread.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
        }

        catch
        {
        }
    }

    protected void cblbranch_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;

            // int cout = 0;
            // string br = "";
            // string bran = "";
            // cbbranch.Checked = false;
            // for (int i = 0; i < cblbranch.Items.Count; i++)
            // {
            //     if (cblbranch.Items[i].Selected == true)
            //     {
            //         cout++;
            //         br = cblbranch.Items[i].Value.ToString();
            //         if (bran == "")
            //         {
            //             bran = br;

            //         }
            //         else
            //         {
            //             bran = bran + "'" + "," + "'" + br;
            //             txttest.Text = "---Select---";
            //         }

            //     }

            //     cbtest.Checked = false;
            //     txttest.Text = "---Select---";
            // }

            //txtbranch.Text = "Branch(" + cout + ")";
            // bindtestname();



            int cout = 0;
            cbbranch.Checked = false;
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    cout++;

                }

            }

            //   cbbranch.Checked = true;
            if (cout > 0)
            {
                txtbranch.Text = "Branch(" + cout + ")";
            }
            else
            {
                txttest.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            bindsem();
            bindsec();
            bindtestname();
            internalgrid.Visible = false;
            Chart1.Visible = false;
            Externalchart.Visible = false;
            Externalgrid.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            fpspread.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
        }

        catch (Exception ex)
        {

        }
    }
    protected void cbtest_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            if (cbtest.Checked == true)
            {
                string hd = "";
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                string hed = "";
                int cout = 0;
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = true;
                    hd = cbltest.Items[i].Value.ToString();
                    if (hed == "")
                    {
                        hed = hd;
                    }
                    else
                    {
                        hed = hed + "'" + "," + "'" + hd;

                    }
                }
                cbtest.Checked = true;
                if (cout != 0)
                {
                    txttest.Text = "Test Name(" + cout + ")";
                }
                else
                {
                    txttest.Text = "---Select---";
                }
                bindbranch();
            }

            else
            {
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
                int cout = 0;
                for (int i = 0; i < cbltest.Items.Count; i++)
                {
                    cout++;
                    cbltest.Items[i].Selected = false;

                }
                cbtest.Checked = false;
                txttest.Text = "---Select---";

            }
            fpspread.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;

        }

        catch (Exception ex)
        {
        }

    }

    protected void cbltest_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            int cout = 0;
            string br = "";
            internalgrid.Visible = false;
            Externalgrid.Visible = false;
            Chart1.Visible = false;
            Printcontrol.Visible = false;
            Externalchart.Visible = false;
            Excel.Visible = false;
            Print.Visible = false;
            fpspread.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            string bran = "";
            cbtest.Checked = false;
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    cout++;
                    br = cbltest.Items[i].Value.ToString();
                    if (bran == "")
                    {
                        bran = br;
                    }
                    else
                    {
                        bran = bran + "'" + "," + "'" + br;

                    }

                }

            }
            if (cout != 0)
            {
                txttest.Text = "Test Name(" + cout + ")";
            }
            else
            {
                txttest.Text = "---Select---";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void btngo_OnClick(object sender, EventArgs e)
    {
        try
        {
            txtexcelname.Text = "";
            int rop = 0;
            int hnop = 0;
            int subnm = 1;
            Printcontrol.Visible = false;
            ArrayList subjectname = new ArrayList();
            Hashtable hn = new Hashtable();
            Hashtable hn1 = new Hashtable();
            string build6 = "";
            string buildvalue6 = "";
            ArrayList addarray = new ArrayList();
            ArrayList al = new ArrayList();
            DataView dv = new DataView();
            DataView dv1 = new DataView();
            int brncnt = 0;
            for (int i = 0; i < cblbranch.Items.Count; i++)
            {
                if (cblbranch.Items[i].Selected == true)
                {
                    brncnt++;
                    build6 = cblbranch.Items[i].Value.ToString();
                    if (buildvalue6 == "")
                    {
                        buildvalue6 = build6;
                    }
                    else
                    {
                        buildvalue6 = buildvalue6 + "'" + "," + "'" + build6;

                    }
                }
            }

            if (brncnt == 0)
            {
                lblmsg.Text = "Please Select Branch";
                lblmsg.Visible = true;
                return;
            }

            string build = "";
            string buildval = "";
            int subcnt = 0;
            for (int i = 0; i < cbltest.Items.Count; i++)
            {
                if (cbltest.Items[i].Selected == true)
                {
                    subcnt++;
                    build = cbltest.Items[i].Value.ToString();
                    if (buildval == "")
                    {
                        buildval = build;
                    }
                    else
                    {
                        buildval = buildval + "'" + "," + "'" + build;

                    }
                    //Internalchat.Series.Add(build);
                }
            }
            if (subcnt == 0 && txttest.Visible == true)
            {
                lblmsg.Text = "Please Select Test";
                lblmsg.Visible = true;
                return;
            }
            lblmsg.Text = "No Records Found";
            lblmsg.Visible = false;
            DataTable dt = new DataTable();
            DataRow row = null;
            DataTable dt1 = new DataTable();
            DataTable dt2 = new DataTable();
            DataTable chartperc = new DataTable();
            ArrayList arrl = new ArrayList();

            DataRow row1 = null;
            DataRow row2 = null;
            DataRow row3 = null;
            DataRow row4 = null;
            if (ddlrepttype.SelectedItem.Text == "Internal")
            {
                //  string SQL = "select distinct s.subject_code,c.criteria,sm.staff_name,s.subject_name,d.Acronym,d.Degree_Code from subject s,staff_selector ss,staffmaster sm,syllabus_master sy,sub_sem sb, criteriaforinternal c,Degree d where c.syll_code=s.syll_code and c.syll_code=sy.syll_code and  c.syll_code=sb.syll_code and  s.subject_no=ss.subject_no and sm.staff_code=ss.staff_code and   sy.syll_code=sb.syll_code and   sb.subtype_no=s.subtype_no and d.Degree_Code=sy.degree_code  and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and sy.semester='" + ddlsem.SelectedValue.ToString() + "'   and sb.promote_count=1 and ss.Sections='" + ddlsec.SelectedValue.ToString() + "' and d.Degree_Code in('" + buildvalue6 + "') and c.criteria in ('" + buildval + "') order by d.Acronym,sm.staff_name,s.subject_name asc";
                string SQL = "select distinct e.exam_code ,s.subject_code,s.subject_no,c.criteria,sm.staff_name,s.subject_name,d.Acronym,d.Degree_Code,ss.Sections from subject s,staff_selector ss,staffmaster sm,syllabus_master sy,sub_sem sb, criteriaforinternal c,Degree d,Exam_type e where c.syll_code=s.syll_code and c.syll_code=sy.syll_code and ss.Sections=e.sections and  c.syll_code=sb.syll_code and  s.subject_no=ss.subject_no and sm.staff_code=ss.staff_code and   sy.syll_code=sb.syll_code and    sb.subtype_no=s.subtype_no and d.Degree_Code=sy.degree_code  and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and   e.criteria_no=c.Criteria_no and e.batch_year=ss.batch_year and  e.subject_no=s.subject_no and   sy.semester='" + ddlsem.SelectedItem.Text + "'   and sb.promote_count=1  and d.Degree_Code in  ('" + buildvalue6 + "') and c.criteria in   ('" + buildval + "')  ";
                if (ddlsec.Enabled == true)
                {
                    if (ddlsec.Text != "ALL")
                    {
                        SQL = SQL + " and e.Sections='" + ddlsec.SelectedItem.Text + "'";
                    }
                }
                SQL = SQL + " order by s.subject_name,sm.staff_name,c.criteria asc";
                // string SQL = "select distinct e.exam_code ,s.subject_code,s.subject_no,c.criteria,sm.staff_name,s.subject_name,d.Acronym,d.Degree_Code from subject s,staff_selector ss,staffmaster sm,syllabus_master sy,sub_sem sb, criteriaforinternal c,Degree d,Exam_type e where c.syll_code=s.syll_code and c.syll_code=sy.syll_code and  c.syll_code=sb.syll_code and  s.subject_no=ss.subject_no and sm.staff_code=ss.staff_code and   sy.syll_code=sb.syll_code and    sb.subtype_no=s.subtype_no and d.Degree_Code=sy.degree_code  and sy.batch_year='" + ddlbatch.SelectedValue.ToString() + "'  and   e.criteria_no=c.Criteria_no and e.batch_year=ss.batch_year and e.sections=ss.Sections and e.subject_no=s.subject_no and   sy.semester='" + ddlsem.SelectedValue.ToString() + "'   and sb.promote_count=1 and ss.Sections='" + ddlsec.SelectedValue.ToString() + "' and d.Degree_Code in  ('" + buildvalue6 + "') and c.criteria in   ('" + buildval + "')   order by d.Acronym,sm.staff_name,s.subject_name asc";
                ds = da.select_method_wo_parameter(SQL, "Text");

                dt.Columns.Add("S.No", typeof(string));
                dt.Columns.Add("Subject Code", typeof(string));
                dt.Columns.Add("Subject Name", typeof(string));
                dt.Columns.Add("Staff Name", typeof(string));
                dt.Columns.Add("Test Name", typeof(string));
                dt.Columns.Add("Pass%", typeof(string));



                dt2.Columns.Add("Subject Code", typeof(string));
                //    dt2.Columns.Add("Pass%", typeof(string));
                row4 = dt2.NewRow();
                row2 = dt2.NewRow();
                row3 = dt2.NewRow();
                row4[0] = "";
                dt2.Rows.Add(row4);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    double interpercent = 0;

                    // int count = 0;
                    for (int i = 0; i < cblbranch.Items.Count; i++)
                    {

                        if (cblbranch.Items[i].Selected == true)
                        {

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                al.Clear();
                                hast.Clear();
                                ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + cblbranch.Items[i].Value + "' ";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    row = dt.NewRow();
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            row[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text + "-" + ddlsec.SelectedItem.Text;
                                        }
                                        else
                                        {
                                            row[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        row[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text;
                                    }
                                    dt.Rows.Add(row);
                                    addarray.Add(dt.Rows.Count);
                                }

                                int l = 0;
                                int count1 = 0;

                                for (int a = 0; a < dv.Count; a++)
                                {
                                    l++;
                                    string serialno = Convert.ToString(dv[a]["subject_name"]);
                                    if (!hast.ContainsKey(Convert.ToString(dv[a]["subject_name"])))
                                    {
                                        if (!al.Contains(serialno))
                                        {
                                            count1++;
                                            al.Add(serialno);

                                            l = 0;
                                            hast.Add(Convert.ToString(dv[a]["subject_name"]), Convert.ToString(dv[a]["subject_name"]));
                                        }
                                    }

                                    //count++;
                                    row = dt.NewRow();
                                    //  row2 = dt2.NewRow();

                                    if (a == 0)
                                    {
                                        row[1] = Convert.ToString(dv[a]["subject_code"]);
                                        row[2] = Convert.ToString(dv[a]["subject_name"]);
                                        row[0] = count1;

                                    }
                                    else
                                    {
                                        if (Convert.ToString(dv[a]["subject_name"]) != Convert.ToString(dv[a - 1]["subject_name"]))
                                        {
                                            row[0] = count1;
                                            row[2] = Convert.ToString(dv[a]["subject_name"]);
                                            row[1] = Convert.ToString(dv[a]["subject_code"]);
                                        }


                                    }
                                    row[0] = count1;
                                    row[2] = Convert.ToString(dv[a]["subject_name"]);
                                    row[1] = Convert.ToString(dv[a]["subject_code"]);
                                    row[3] = Convert.ToString(dv[a]["staff_name"]);
                                    // row[2] = Convert.ToString(dv[a]["subject_name"]);
                                    // if (l == 0)
                                    {
                                        row[4] = Convert.ToString(dv[a]["criteria"]);
                                    }
                                    //else
                                    //{
                                    //    if (a != 0)
                                    //    {
                                    //        if (Convert.ToString(dv[a]["criteria"]) != Convert.ToString(dv[a - 1]["criteria"]))
                                    //        {
                                    //            row[4] = Convert.ToString(dv[a]["criteria"]);
                                    //        }
                                    //    }

                                    //}
                                    string testcretria = dv[a]["subject_code"].ToString() + dv[a]["criteria"].ToString();


                                    //row[4] = Convert.ToString(dv[a]["exam_code"]);



                                    string sec = "";
                                    string excode = Convert.ToString(dv[a]["exam_code"]);
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            sec = "rt.Sections='" + ddlsec.SelectedValue.ToString() + "' and";
                                        }
                                        else
                                        {
                                            sec = "";
                                        }
                                    }
                                    string sc = "";
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            sc = " and ex.Sections='" + ddlsec.SelectedItem.Text + "'";
                                        }

                                    }
                                    string sqlquery = "select count(distinct r.roll_no)  as 'PASS_COUNT' from result r,exam_type ex,subjectchooser su,registration rt where " + sec + "   r.roll_no=rt.roll_no and r.exam_code='" + excode + "'  and  r.roll_no=su.roll_no and su.subject_no=ex.subject_no and    r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and  rt.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "'  and rt.degree_code in ('" + cblbranch.Items[i].Value + "') and su.semester=" + ddlsem.SelectedItem.Text + " " + sc + " select count(distinct rt.roll_no) as 'PRESENT_COUNT' from result r,registration rt,subjectchooser su,exam_type ex  where r.exam_code='" + excode + "' and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')and  marks_obtained<>'-1'  and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code  and su.roll_no=r.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and rt.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and rt.degree_code = ('" + cblbranch.Items[i].Value + "') and su.semester=" + ddlsem.SelectedItem.Text + " " + sc + "";
                                    DataSet ds1 = new DataSet();
                                    ds1 = da.select_method_wo_parameter(sqlquery, "text");
                                    double pass = Convert.ToDouble(ds1.Tables[0].Rows[0]["PASS_COUNT"]);
                                    double present = Convert.ToDouble(ds1.Tables[1].Rows[0]["PRESENT_COUNT"]);
                                    interpercent = 0;
                                    if (pass != 0)
                                    {
                                        if (present != Convert.ToDouble("0.0"))
                                        {
                                            rop = 1;
                                            interpercent = pass / present * 100;
                                        }
                                        interpercent = Math.Round(interpercent, 2);


                                    }
                                    row[5] = Convert.ToString(interpercent);
                                    //dt.Rows.Add(row);

                                    string str = (Convert.ToString(dv[a]["subject_code"]) + "%" + Convert.ToString(interpercent) + "%" + Convert.ToString(dv[a]["criteria"]));
                                    if (!arrl.Contains(str))
                                    {

                                        //row2[0] = Convert.ToString(dv[a]["subject_code"]);
                                        //row2[1] = interpercent;
                                        //dt2.Rows.Add(row2);

                                        arrl.Add(str);
                                        if (!hn1.ContainsKey(Convert.ToString(dv[a]["subject_code"])))
                                        {
                                            dt2.Columns.Add(Convert.ToString(dv[a]["subject_code"]), typeof(string));
                                            hn1.Add(Convert.ToString(dv[a]["subject_code"]), subnm);
                                            subnm++;
                                            int smn = Convert.ToInt32(hn1[(dv[a]["subject_code"])].ToString());
                                            row4[smn] = Convert.ToString(dv[a]["subject_code"]);
                                        }
                                        //if (!hn.ContainsKey(Convert.ToString(dv[a]["criteria"])))
                                        //{

                                        //    hn.Add(Convert.ToString(dv[a]["criteria"]), hnop);
                                        //    hnop++;
                                        //    string mn = hn[Convert.ToString(dv[a]["criteria"])].ToString();
                                        //    int smn = Convert.ToInt32(hn1[(dv[a]["subject_code"])].ToString());
                                        //    if (mn == "0")
                                        //    {
                                        //        //  row2[0] = 0;
                                        //        //row2[1] = 1;
                                        //        row2[0] = Convert.ToString(dv[a]["criteria"]);
                                        //        if (Convert.ToString(interpercent) != "")
                                        //        {
                                        //            row2[smn] = interpercent;
                                        //        }
                                        //        else
                                        //        {
                                        //            row2[smn] = 0;
                                        //        }
                                        //        //dt2.Rows.Add(row2);
                                        //    }

                                        //}
                                        //else
                                        //{
                                        //    string mn = hn[Convert.ToString(dv[a]["criteria"])].ToString();
                                        //    int smn = Convert.ToInt32(hn1[(dv[a]["subject_code"])].ToString());
                                        //    if (mn == "0")
                                        //    {
                                        //        //   row2[0] = 0;
                                        //        //row2[1] = 1;
                                        //        if (Convert.ToString(interpercent) != "")
                                        //        {
                                        //            row2[smn] = interpercent;
                                        //        }
                                        //        else
                                        //        {
                                        //            row2[smn] = 0;
                                        //        }
                                        //        //  dt2.Rows.Add(row2);
                                        //    }
                                        //    else
                                        //    {
                                        //        //  row2[0] = 0;
                                        //        // row2[1] = 1;
                                        //        if (Convert.ToString(interpercent) != "")
                                        //        {
                                        //            row3[smn] = interpercent;
                                        //        }
                                        //        else
                                        //        {
                                        //            row3[smn] = 0;
                                        //        }
                                        //        // dt2.Rows.Add(row3);
                                        //    }
                                        //}

                                    }

                                    //if()
                                    //{
                                    //}
                                    //dt2.Rows.Add(row4);


                                    dt.Rows.Add(row);
                                }
                            }
                            txttest.Visible = true;
                            if (dt.Rows.Count > 0)
                            {
                                string[] columnNames = (from dc in dt.Columns.Cast<DataColumn>()
                                                        select dc.ColumnName).ToArray();
                                fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                                fpspread.Sheets[0].RowCount = 0;
                                fpspread.Height = 500;
                                fpspread.Width = 800;
                                fpspread.CommandBar.Visible = false;
                                fpspread.Sheets[0].SheetCorner.ColumnCount = 0;
                                fpspread.Sheets[0].RowHeader.Visible = false;

                                FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                                style2.Font.Size = 13;
                                style2.Font.Name = "Book Antiqua";
                                style2.Font.Bold = true;
                                style2.HorizontalAlign = HorizontalAlign.Center;
                                style2.ForeColor = System.Drawing.Color.White;
                                style2.BackColor = ColorTranslator.FromHtml("#008080");
                                fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                                fpspread.Sheets[0].AutoPostBack = true;

                                fpspread.Sheets[0].ColumnCount = columnNames.GetUpperBound(0) + 1;

                                for (int ia = 0; ia <= columnNames.GetUpperBound(0); ia++)
                                {
                                    fpspread.Sheets[0].ColumnHeader.Cells[0, ia].Text = columnNames[ia].ToString();
                                }

                                for (int ia = 0; ia < dt.Rows.Count; ia++)
                                {
                                    fpspread.Sheets[0].Rows.Count++;
                                    for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                                    {
                                        if (dt.Rows[ia][1].ToString().Trim() == "")
                                        {
                                            fpspread.Sheets[0].SpanModel.Add(ia, 0, 1, 6);
                                            fpspread.Sheets[0].Rows[ia].BackColor = ColorTranslator.FromHtml("#67CEBA");
                                        }

                                        fpspread.Sheets[0].Cells[fpspread.Sheets[0].Rows.Count - 1, j].Text = dt.Rows[ia][j].ToString();
                                        fpspread.Sheets[0].Cells[ia, 0].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[ia, 0].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 1].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 2].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 3].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 4].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 4].VerticalAlign = VerticalAlign.Middle;
                                        fpspread.Sheets[0].Cells[ia, 5].VerticalAlign = VerticalAlign.Middle;

                                        //fpspread.Sheets[0].Cells[ia, 1].HorizontalAlign = HorizontalAlign.Center;
                                        //fpspread.Sheets[0].Cells[ia, 2].HorizontalAlign = HorizontalAlign.Center;
                                        //fpspread.Sheets[0].Cells[ia, 3].HorizontalAlign = HorizontalAlign.Center;
                                        //fpspread.Sheets[0].Cells[ia, 4].HorizontalAlign = HorizontalAlign.Center;
                                        fpspread.Sheets[0].Cells[ia, 5].HorizontalAlign = HorizontalAlign.Center;


                                    }
                                }
                                fpspread.Sheets[0].Columns[0].Width = 50;
                                fpspread.Sheets[0].Columns[1].Width = 70;
                                fpspread.Sheets[0].Columns[2].Width = 220;
                                fpspread.Sheets[0].Columns[3].Width = 150;
                                fpspread.Sheets[0].Columns[4].Width = 50;
                                fpspread.Sheets[0].Columns[0].Locked = true;
                                fpspread.Sheets[0].Columns[1].Locked = true;
                                fpspread.Sheets[0].Columns[2].Locked = true;
                                fpspread.Sheets[0].Columns[3].Locked = true;
                                fpspread.Sheets[0].Columns[4].Locked = true;
                                fpspread.Sheets[0].Columns[5].Locked = true;
                                fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                fpspread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                fpspread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                fpspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                                fpspread.SaveChanges();
                                fpspread.Visible = true;
                                lblrptname.Visible = true;
                                txtexcelname.Visible = true;

                            }
                            for (int k = 0; k < fpspread.Sheets[0].Columns.Count; k++)
                            {
                                fpspread.Sheets[0].Columns[k].Font.Bold = true;
                                fpspread.Sheets[0].Columns[k].Font.Size = FontUnit.Medium;
                                fpspread.Sheets[0].Columns[k].Font.Name = "Book Antiqua";
                            }
                            internalgrid.DataSource = dt;
                            internalgrid.DataBind();
                            internalgrid.Visible = false;




                            //if (dt2.Rows.Count > 0)
                            //{
                            //    for (int se = 0; se < dt2.Rows.Count; se++)
                            //    {
                            //        //string[] splitvalue = Convert.ToString(arrl[se]).Split('%');
                            //        //if (splitvalue.Length > 0)
                            //        //{
                            //        //    string subjectname1 = splitvalue[0];
                            //        //    string percentage = splitvalue[1];
                            //        //    string testname = splitvalue[2];
                            //        Internalchat.Series.Add(Convert.ToString(dt2.Rows[se][0]));
                            //        //Internalchat.Series.Add("Pass % (" + build + ")");
                            //        //Internalchat.Series[se].Points.AddXY(subjectname1 + "-" + testname, percentage);
                            //        //Internalchat.Series[subjectname1 + "-" + testname].IsValueShownAsLabel = true;
                            //        //Internalchat.Series[se].YValuesPerPoint = 2;


                            //    }
                            //}
                            Externalgrid.Visible = false;
                            lblmsg.Visible = false;
                            Excel.Visible = true;
                            Print.Visible = true;

                            if (addarray.Count > 0)
                            {

                                for (int j = 0; j < addarray.Count; j++)
                                {
                                    int g = Convert.ToInt16(addarray[j]);

                                    internalgrid.Rows[g - 1].Cells[0].ColumnSpan = 6;
                                    internalgrid.Rows[g - 1].Cells[0].ColumnSpan = 6;
                                    internalgrid.Rows[g - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                    internalgrid.Rows[g - 1].Cells[0].ForeColor = System.Drawing.Color.DarkBlue;
                                    internalgrid.Rows[g - 1].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                                    internalgrid.Rows[g - 1].Cells[1].Visible = false;
                                    internalgrid.Rows[g - 1].Cells[2].Visible = false;
                                    internalgrid.Rows[g - 1].Cells[3].Visible = false;
                                    internalgrid.Rows[g - 1].Cells[4].Visible = false;
                                    internalgrid.Rows[g - 1].Cells[5].Visible = false;


                                }
                            }

                        }
                        if (rop == 0)
                        {
                            lblmsg.Visible = true;
                            lblmsg.Text = "No Records Found";
                            internalgrid.Visible = false;
                            Externalgrid.Visible = false;
                            Chart1.Visible = false;
                            Externalchart.Visible = false;
                            Excel.Visible = false;
                            Print.Visible = false;
                        }
                    }

                    if (brncnt > 1)
                    {
                        if (ddlsec.Enabled == false)
                        {
                            Label1.Visible = true;
                            Label1.Text = " More Than One Branch Not Allowed For This Chart";
                            Chart1.Visible = false;
                        }
                        else
                        {
                            if (ddlsec.SelectedItem.Text.ToLower().Trim() == "all")
                            {
                                Label1.Visible = true;
                                Label1.Text = " More Than One Branch Not Allowed For This Chart";
                                Chart1.Visible = false;
                            }
                            else
                            {
                                Label1.Visible = true;
                                Label1.Text = " More Than One Branch Not Allowed For This Chart";
                                Chart1.Visible = false;
                            }
                        }


                    }
                    else
                    {


                        //dt2.Rows.Add(row2);
                        //dt2.Rows.Add(row3);


                        Chart1.Series.Clear();
                        int series = 0;
                        for (int i = 0; i < cbltest.Items.Count; i++)
                        {
                            if (cbltest.Items[i].Selected == true)
                            {
                                row2 = dt2.NewRow();
                                row2[0] = cbltest.Items[i].ToString();
                                dt2.Rows.Add(row2);

                                Chart1.Series.Add(cbltest.Items[i].ToString());

                                Chart1.Series[series].BorderWidth = 2;
                                series++;
                            }
                        }



                        Chart1.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.Black;
                        Chart1.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.Black;
                        string chartcblbranch = "";
                        for (int i = 0; i < cblbranch.Items.Count; i++)
                        {
                            if (cblbranch.Items[i].Selected == true)
                            {
                                chartcblbranch = cblbranch.Items[i].Value;
                            }
                        }

                        for (int m = 1; m < dt2.Rows.Count; m++)
                        {
                            for (int n = 1; n < dt2.Columns.Count; n++)
                            {
                                string chatcreteria = dt2.Rows[m][0].ToString();
                                string chatexamcode = "";
                                string chatsubjectcode = dt2.Rows[0][n].ToString();
                                ds.Tables[0].DefaultView.RowFilter = "criteria='" + chatcreteria + "' and  subject_code = '" + chatsubjectcode + "'";
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    chatexamcode = Convert.ToString(dv[0][0].ToString());

                                    string sec = "";
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            sec = "rt.Sections='" + ddlsec.SelectedValue.ToString() + "' and";
                                        }
                                        else
                                        {
                                            sec = "";
                                        }
                                    }
                                    string sc = "";
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            sc = " and ex.Sections='" + ddlsec.SelectedItem.Text + "'";
                                        }

                                    }

                                    string sqlquery = "select count(distinct r.roll_no)  as 'PASS_COUNT' from result r,exam_type ex,subjectchooser su,registration rt where " + sec + " r.roll_no=rt.roll_no and r.exam_code='" + chatexamcode + "'  and  r.roll_no=su.roll_no and su.subject_no=ex.subject_no and    r.exam_code=ex.exam_code and (r.marks_obtained>=ex.min_mark or r.marks_obtained='-3' or r.marks_obtained='-2') and r.marks_obtained<>'-1'  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0  and  rt.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "'  and rt.degree_code in ('" + chartcblbranch + "') and su.semester=" + ddlsem.SelectedItem.Text + " " + sc + " select count(distinct rt.roll_no) as 'PRESENT_COUNT' from result r,registration rt,subjectchooser su,exam_type ex  where r.exam_code='" + chatexamcode + "' and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3')and  marks_obtained<>'-1'  and r.roll_no=rt.roll_no and su.subject_no=ex.subject_no and ex.exam_code=r.exam_code  and su.roll_no=r.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0  and rt.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and rt.degree_code = ('" + chartcblbranch + "') and su.semester=" + ddlsem.SelectedItem.Text + " " + sc + "";
                                    DataSet ds1 = new DataSet();
                                    ds1 = da.select_method_wo_parameter(sqlquery, "text");
                                    double pass = Convert.ToDouble(ds1.Tables[0].Rows[0]["PASS_COUNT"]);
                                    double present = Convert.ToDouble(ds1.Tables[1].Rows[0]["PRESENT_COUNT"]);
                                    interpercent = 0;
                                    if (pass != 0)
                                    {
                                        if (present != Convert.ToDouble("0.0"))
                                        {
                                            rop = 1;
                                            interpercent = pass / present * 100;
                                        }
                                        interpercent = Math.Round(interpercent, 2);


                                    }
                                    //row[5] = Convert.ToString(interpercent);
                                    dt2.Rows[m][n] = Convert.ToString(interpercent);

                                }
                                else
                                {
                                    dt2.Rows[m][n] = Convert.ToString(0);
                                }

                            }

                        }

                        for (int chart_i = 0; chart_i < dt2.Columns.Count - 1; chart_i++)
                        {
                            for (int chart_j = 0; chart_j < dt2.Rows.Count - 1; chart_j++)
                            {
                                string subnncode = dt2.Rows[0][chart_i + 1].ToString();
                                string m1 = dt2.Rows[chart_j + 1][chart_i + 1].ToString();
                                Chart1.Series[chart_j].Points.AddXY(subnncode, m1);
                                Chart1.Series[chart_j].IsValueShownAsLabel = true;
                            }
                        }

                        //for (int chart_i = 0; chart_i < dt2.Columns.Count - 1; chart_i++)
                        //{

                        //    string subnncode = dt2.Rows[0][chart_i + 1].ToString();
                        //    string m1 = dt2.Rows[1][chart_i + 1].ToString();
                        //    string m2 = dt2.Rows[2][chart_i + 1].ToString();
                        //    if (m1.Trim() == "")
                        //    {
                        //        m1 = "0";
                        //    }
                        //    if (m2.Trim() == "")
                        //    {
                        //        m2 = "0";
                        //    }
                        //    Chart1.Series[1].Points.AddXY(subnncode, m1);

                        //    Chart1.Series[0].Points.AddXY(subnncode, m2);
                        //}

                        //Chart1.Series[0].IsValueShownAsLabel = true;
                        //Chart1.Series[1].IsValueShownAsLabel = true;

                        Chart1.Visible = true;


                    }

                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records Found";
                    internalgrid.Visible = false;
                    Externalgrid.Visible = false;
                    Chart1.Visible = false;
                    Externalchart.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }

            }

            else if (ddlrepttype.SelectedItem.Text == "External")
            {
                int gk = 0;
                string SQL1 = string.Empty;
                string selectQ = "select * from  staff_selector ss,subject s,syllabus_master sy  where s.subject_no=ss.subject_no and  s.syll_code=sy.syll_code and sy.Batch_Year='"+ddlbatch.SelectedValue.ToString()+"' and sy.semester='"+ddlsem.SelectedValue.ToString()+"'  and sy.degree_code in('" + buildvalue6 + "')";
                DataTable dtstaffSel = dir.selectDataTable(selectQ);//rajkumar on 29-5-2018
                if (dtstaffSel.Rows.Count > 0)
                {
                    //SQL1 = "select distinct r.roll_no,s.subject_code,s.subject_no, sm.staff_name,s.subject_name,result,r.Degree_Code from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where e.exam_code=m.exam_code and  r.Sections= ss.Sections and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code  and e.degree_code=r.degree_code and ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code in('" + buildvalue6 + "') and  e.current_semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in('AAA','UA','WHD','Fail','')  ";

                    SQL1 = "select distinct r.roll_no,s.subject_code,s.subject_no, sm.staff_name,s.subject_name,result,r.Degree_Code from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where e.exam_code=m.exam_code and  ISNULL(r.Sections,'')= ISNULL(ss.Sections,'') and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code  and e.degree_code=r.degree_code and ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code in('" + buildvalue6 + "') and  e.current_semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in('AAA','UA','WHD','Fail','')  ";//modified by rajasekar on 16/08/2018
                    if (ddlsec.Enabled == true)
                    {
                        if (ddlsec.Text != "ALL")
                        {
                            SQL1 = SQL1 + " and ss.Sections=('" + ddlsec.SelectedItem.Text + "')";
                        }
                    }
                    SQL1 = SQL1 + " order by s.subject_code,sm.staff_name";
                 
                }
                else
                {
                    SQL1 = "select distinct r.roll_no,s.subject_code,s.subject_no,ISNULL('','') staff_name ,s.subject_name,result,r.Degree_Code from mark_entry m,Exam_Details e,Registration r,subject s where e.exam_code=m.exam_code and   m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year and e.degree_code=r.degree_code and s.subject_no=m.subject_no and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code in('" + buildvalue6 + "') and  e.current_semester='" + ddlsem.SelectedValue.ToString() + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in('AAA','UA','WHD','Fail','')";
                    if (ddlsec.Enabled == true)//Rajkumar on 29-5-2018
                    {
                        if (ddlsec.Text != "ALL")
                        {
                            SQL1 = SQL1 + " and r.Sections=('" + ddlsec.SelectedItem.Text + "')";
                        }
                    }
                    SQL1 = SQL1 + " order by s.subject_code";
                }
                
                //string SQL1 = " select r.Batch_Year,r.roll_no, sm.staff_name,s.subject_name,result from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where e.exam_code=m.exam_code and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code and ss.Sections=r.Sections and ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and e.current_semester=r.Current_Semester and r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and  e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result='Pass'";

             
                ds = da.select_method_wo_parameter(SQL1, "Text");
                // dv = ds.Tables[0].DefaultView;

                dt1.Columns.Add("S.No", typeof(string));
                dt1.Columns.Add("Subject Code", typeof(string));
                dt1.Columns.Add("Subject Name", typeof(string));
                dt1.Columns.Add("Staff Name", typeof(string));
                dt1.Columns.Add("Pass%", typeof(string));


                string sec = "";

                if (ddlsec.Enabled == false)
                {
                    sec = "";
                }
                else
                {
                    sec = ddlsec.SelectedItem.Text;
                }
                if (ds.Tables[0].Rows.Count > 0)
                {

                    DataView dv22 = new DataView();
                    DataTable data = new DataTable();
                    ArrayList staffdet = new ArrayList();
                    int count2 = 0;
                    for (int i = 0; i < cblbranch.Items.Count; i++)
                    {

                        if (cblbranch.Items[i].Selected == true)
                        {

                            if (ds.Tables[0].Rows.Count > 0)
                            {

                                //  string subno = Convert.ToString(ds.Tables[0].Rows[i]["subject_no"]);
                                ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + cblbranch.Items[i].Value + "' ";
                                dv1 = ds.Tables[0].DefaultView;
                                if (dv1.Count > 0)
                                {
                                    gk = 1;
                                    row1 = dt1.NewRow();
                                    if (ddlsec.Enabled == true)
                                    {
                                        if (ddlsec.Text != "ALL")
                                        {
                                            row1[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text + "-" + ddlsec.SelectedItem.Text;
                                        }
                                        else
                                        {
                                            row1[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text;
                                        }
                                    }
                                    else
                                    {
                                        row1[0] = ddlbatch.SelectedItem.Text + "-" + ddldegree.SelectedItem.Text + "-" + cblbranch.Items[i].Text + "-" + ddlsem.SelectedItem.Text;
                                    }
                                    dt1.Rows.Add(row1);
                                    addarray.Add(dt1.Rows.Count);
                                    data = dv1.ToTable();
                                    for (int a = 0; a < dv1.Count; a++)
                                    {
                                        int h = 0;
                                        string serial = Convert.ToString(dv1[a]["subject_name"]);
                                        //if (!al.Contains(serial))
                                        //{
                                        //    count2++;
                                        //    al.Add(serial);
                                        //}

                                        //row1 = dt1.NewRow();
                                        //row1[0] = count2;


                                        if (a == 0)
                                        {
                                            count2++;
                                            row1 = dt1.NewRow();
                                            h++;
                                            row1[0] = count2;
                                            row1[1] = Convert.ToString(dv1[a]["subject_code"]);
                                            row1[2] = Convert.ToString(dv1[a]["subject_name"]);
                                            row1[3] = Convert.ToString(dv1[a]["staff_name"]);
                                        }
                                        else
                                        {
                                            if (Convert.ToString(dv1[a]["subject_no"]) != Convert.ToString(dv1[a - 1]["subject_no"]))
                                            {
                                                count2++;
                                                row1 = dt1.NewRow();
                                                h++;
                                                row1[0] = count2;
                                                row1[1] = Convert.ToString(dv1[a]["subject_code"]);
                                                row1[2] = Convert.ToString(dv1[a]["subject_name"]);
                                            }
                                            if (!staffdet.Contains(dv1[a]["subject_no"] + "-" + dv1[a]["staff_name"]))
                                            {
                                                if (h == 0)
                                                {
                                                    row1 = dt1.NewRow();
                                                }
                                                h++;
                                                row1[0] = count2;
                                                row1[1] = Convert.ToString(dv1[a]["subject_code"]);
                                                row1[2] = Convert.ToString(dv1[a]["subject_name"]);
                                                row1[3] = Convert.ToString(dv1[a]["staff_name"]);
                                                staffdet.Add(dv1[a]["subject_no"] + "-" + dv1[a]["staff_name"]);
                                            }
                                        }
                                        // row1[2] = Convert.ToString(dv1[a]["staff_name"]);
                                        string subno = Convert.ToString(dv1[a]["subject_no"]);

                                        dv22 = new DataView(data);
                                        dv22.RowFilter = "Degree_Code='" + cblbranch.Items[i].Value + "' and subject_no='" + subno + "'";

                                        //ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + cblbranch.Items[i].Value + "' and subject_no='" + subno + "'";
                                        //dv22 = ds.Tables[0].DefaultView;
                                        // dv1.RowFilter = "(Degree_Code'" + cblbranch.Items[i].Value + "')and(ubject_no'" + subno + "') ";

                                        //string sqlquery2 = "   select r.roll_no,s.subject_no, sm.staff_name,s.subject_name,result from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where  e.exam_code=m.exam_code  and e.batch_year=r.Batch_Year and   r.Sections=ss.Sections and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code and  ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and  r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + cblbranch.Items[i].Value + "' and  e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in ('AAA','WHD','UA','')  and r.degree_code='" + cblbranch.Items[i].Value + "' and s.subject_no='" + subno + "' ";
                                        //if (ddlsec.Enabled == true)
                                        //{
                                        //    if (ddlsec.Text != "ALL")
                                        //    {
                                        //        sqlquery2 = sqlquery2 + " and ss.Sections=('" + ddlsec.SelectedItem.Text + "')";
                                        //    }
                                        //}
                                        //sqlquery2 = sqlquery2 + "order by s.subject_code";

                                        string sqlquery2 = string.Empty;
                                        if (dtstaffSel.Rows.Count > 0)//rajkumar on 29-5-2018
                                        {
                                             //sqlquery2 = "   select r.roll_no,s.subject_no, sm.staff_name,s.subject_name,result from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where  e.exam_code=m.exam_code  and e.batch_year=r.Batch_Year and   r.Sections=ss.Sections and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code and  ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and  r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + cblbranch.Items[i].Value + "' and  e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in ('AAA','WHD','UA','')  and r.degree_code='" + cblbranch.Items[i].Value + "' and s.subject_no='" + subno + "' ";
                                            sqlquery2 = "   select r.roll_no,s.subject_no, sm.staff_name,s.subject_name,result from mark_entry m,Exam_Details e,Registration r,staffmaster sm,staff_selector ss ,subject s where  e.exam_code=m.exam_code  and e.batch_year=r.Batch_Year and   ISNULL(r.Sections,'')= ISNULL (ss.Sections,'') and m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no and ss.subject_no=s.subject_no and ss.staff_code=sm.staff_code and  ss.batch_year=r.Batch_Year and m.subject_no=ss.subject_no and  r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + cblbranch.Items[i].Value + "' and  e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in ('AAA','WHD','UA','')  and r.degree_code='" + cblbranch.Items[i].Value + "' and s.subject_no='" + subno + "' ";//modified by rajasekar on 16/08/2018
                                            if (ddlsec.Enabled == true)
                                            {
                                                if (ddlsec.Text != "ALL")
                                                {
                                                    sqlquery2 = sqlquery2 + " and ss.Sections=('" + ddlsec.SelectedItem.Text + "')";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            sqlquery2 = "   select r.roll_no,s.subject_no, ISNULL('','') as staff_name,s.subject_name,result from mark_entry m,Exam_Details e,Registration r,subject s where  e.exam_code=m.exam_code  and e.batch_year=r.Batch_Year and   m.roll_no=r.Roll_No and e.batch_year=r.Batch_Year  and s.subject_no=m.subject_no  and  r.Batch_Year='" + ddlbatch.SelectedValue.ToString() + "' and r.degree_code='" + cblbranch.Items[i].Value + "' and  e.current_semester='" + ddlsem.SelectedItem.Text + "' and r.cc=0 and  r.exam_flag <>'DEBAR' and r.delflag=0 and result not in ('AAA','WHD','UA','')  and r.degree_code='" + cblbranch.Items[i].Value + "' and s.subject_no='" + subno + "'  ";
                                            
                                            if (ddlsec.Enabled == true)
                                            {
                                                if (ddlsec.Text != "ALL")
                                                {
                                                    sqlquery2 = sqlquery2 + " and r.Sections=('" + ddlsec.SelectedItem.Text + "')";
                                                }
                                            }
                                        }
                                        DataSet ds2 = new DataSet();
                                        ds2 = da.select_method_wo_parameter(sqlquery2, "text");
                                        double pass = dv22.Count;
                                        double present = ds2.Tables[0].Rows.Count;
                                        double externalpercent = 0;
                                        externalpercent = pass / present * 100;
                                        externalpercent = Math.Round(externalpercent, 2);
                                        row1[4] = Convert.ToString(externalpercent);
                                        if (h != 0)
                                        {
                                            dt1.Rows.Add(row1);
                                        }



                                    }
                                }
                            }
                        }


                    }
                    if (gk != 0)
                    {
                        txttest.Visible = false;

                        if (dt1.Rows.Count > 0)
                        {
                            string[] columnNames = (from dc in dt1.Columns.Cast<DataColumn>()
                                                    select dc.ColumnName).ToArray();
                            fpspread.Sheets[0].ColumnHeader.RowCount = 1;
                            fpspread.Sheets[0].RowCount = 0;
                            fpspread.Height = 500;
                            fpspread.Width = 800;
                            fpspread.CommandBar.Visible = false;
                            fpspread.Sheets[0].SheetCorner.ColumnCount = 0;
                            fpspread.Sheets[0].RowHeader.Visible = false;

                            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
                            style2.Font.Size = 13;
                            style2.Font.Name = "Book Antiqua";
                            style2.Font.Bold = true;
                            style2.HorizontalAlign = HorizontalAlign.Center;
                            style2.ForeColor = System.Drawing.Color.White;
                            style2.BackColor = ColorTranslator.FromHtml("#008080");
                            fpspread.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);
                            fpspread.Sheets[0].AutoPostBack = true;

                            fpspread.Sheets[0].ColumnCount = columnNames.GetUpperBound(0) + 1;

                            for (int i = 0; i <= columnNames.GetUpperBound(0); i++)
                            {
                                fpspread.Sheets[0].ColumnHeader.Cells[0, i].Text = columnNames[i].ToString();
                            }

                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                fpspread.Sheets[0].Rows.Count++;
                                for (int j = 0; j < fpspread.Sheets[0].ColumnCount; j++)
                                {
                                    if (dt1.Rows[i][1].ToString().Trim() == "")
                                    {
                                        fpspread.Sheets[0].SpanModel.Add(i, 0, 1, 5);
                                        fpspread.Sheets[0].Rows[i].BackColor = ColorTranslator.FromHtml("#67CEBA");
                                    }

                                    fpspread.Sheets[0].Cells[fpspread.Sheets[0].Rows.Count - 1, j].Text = dt1.Rows[i][j].ToString();
                                    fpspread.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[i, 0].VerticalAlign = VerticalAlign.Middle;
                                    //fpspread.Sheets[0].Cells[ia, 1].HorizontalAlign = HorizontalAlign.Center;
                                    //fpspread.Sheets[0].Cells[ia, 2].HorizontalAlign = HorizontalAlign.Center;
                                    fpspread.Sheets[0].Cells[i, 3].VerticalAlign = VerticalAlign.Middle;
                                    fpspread.Sheets[0].Cells[i, 1].VerticalAlign = VerticalAlign.Middle;
                                    fpspread.Sheets[0].Cells[i, 2].VerticalAlign = VerticalAlign.Middle;

                                    fpspread.Sheets[0].Cells[i, 4].HorizontalAlign = HorizontalAlign.Center;
                                }
                            }
                            fpspread.Sheets[0].Columns[0].Width = 50;
                            fpspread.Sheets[0].Columns[1].Width = 70;
                            fpspread.Sheets[0].Columns[2].Width = 220;
                            fpspread.Sheets[0].Columns[3].Width = 150;
                            fpspread.Sheets[0].Columns[4].Width = 50;

                            fpspread.Sheets[0].Columns[0].Locked = true;
                            fpspread.Sheets[0].Columns[1].Locked = true;
                            fpspread.Sheets[0].Columns[2].Locked = true;
                            fpspread.Sheets[0].Columns[3].Locked = true;
                            fpspread.Sheets[0].Columns[4].Locked = true;
                            fpspread.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                            fpspread.Sheets[0].PageSize = fpspread.Sheets[0].RowCount;
                            fpspread.SaveChanges();
                            fpspread.Visible = true;
                            lblrptname.Visible = true;
                            txtexcelname.Visible = true;



                        }

                        Externalgrid.DataSource = dt1;
                        Externalgrid.DataBind();
                        Externalgrid.Visible = false;
                        Externalchart.Visible = true;
                        ArrayList dupsubj = new ArrayList();
                        DataTable chatgrid = new DataTable();
                        //chatgrid.Columns.Add("S.No", typeof(string));
                        chatgrid.Columns.Add("Subject Code", typeof(string));
                        // chatgrid.Columns.Add("Subject Name", typeof(string));
                        // chatgrid.Columns.Add("Staff Name", typeof(string));
                        chatgrid.Columns.Add("Pass%", typeof(string));
                        dupsubj.Clear();
                        chatgrid.Clear();
                        for (int ij = 0; ij < dt1.Rows.Count; ij++)
                        {
                            if (!dupsubj.Contains(dt1.Rows[ij][1].ToString()))
                            {
                                if (dt1.Rows[ij][1].ToString().Trim() != "")
                                {
                                    row1 = chatgrid.NewRow();
                                    row1[0] = Convert.ToString(dt1.Rows[ij][1]);
                                    row1[1] = Convert.ToString(dt1.Rows[ij][4]);
                                    //row1[2] = Convert.ToString(dt1.Rows[ij][2]);
                                    //row1[3] = Convert.ToString(dt1.Rows[ij][3]);
                                    //row1[4] = Convert.ToString(dt1.Rows[ij][4]);
                                    dupsubj.Add(dt1.Rows[ij][1].ToString());
                                    chatgrid.Rows.Add(row1);
                                }
                                else
                                {
                                    dupsubj.Clear();
                                }

                            }

                        }
                        for (int k = 0; k < fpspread.Sheets[0].Columns.Count; k++)
                        {
                            fpspread.Sheets[0].Columns[k].Font.Bold = true;
                            fpspread.Sheets[0].Columns[k].Font.Size = FontUnit.Medium;
                            fpspread.Sheets[0].Columns[k].Font.Name = "Book Antiqua";
                        }

                        Externalchart.DataSource = chatgrid;
                        Externalchart.DataBind();
                        Externalchart.ChartAreas[0].AxisX.RoundAxisValues();
                        Externalchart.ChartAreas[0].AxisX.Minimum = 0;
                        Externalchart.ChartAreas[0].AxisX.Interval = 1;
                        Externalchart.Series[0].XValueMember = "subject code";
                        Externalchart.Series[0].YValueMembers = "Pass%";
                        Externalchart.Series[0].IsValueShownAsLabel = true;
                        Externalchart.Series[0].ChartType = SeriesChartType.Column;
                        Externalchart.Series[0].YValuesPerPoint = 2;
                        //Externalchart.ChartAreas[0].AxisX.LabelStyle.Angle = 90;
                        internalgrid.Visible = false;
                        Chart1.Visible = false;
                        lblmsg.Visible = false;
                        Excel.Visible = true;
                        Print.Visible = true;

                    }
                    else
                    {
                        lblmsg.Visible = true;
                        lblmsg.Text = "No Records Found";
                        internalgrid.Visible = false;
                        Externalgrid.Visible = false;
                        Chart1.Visible = false;
                        Externalchart.Visible = false;
                        Excel.Visible = false;
                        Print.Visible = false;
                        return;
                    }

                    if (addarray.Count > 0)
                    {

                        for (int j = 0; j < addarray.Count; j++)
                        {
                            int g = Convert.ToInt16(addarray[j]);
                            Externalgrid.Rows[g - 1].Cells[0].ColumnSpan = 5;
                            Externalgrid.Rows[g - 1].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                            Externalgrid.Rows[g - 1].Cells[0].ForeColor = System.Drawing.Color.DarkBlue;
                            Externalgrid.Rows[g - 1].Cells[0].BackColor = System.Drawing.Color.Gainsboro;
                            Externalgrid.Rows[g - 1].Cells[1].Visible = false;
                            Externalgrid.Rows[g - 1].Cells[2].Visible = false;
                            Externalgrid.Rows[g - 1].Cells[3].Visible = false;
                            Externalgrid.Rows[g - 1].Cells[4].Visible = false;
                        }
                    }

                }
                else
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "No Records Found";
                    internalgrid.Visible = false;
                    Externalgrid.Visible = false;
                    Chart1.Visible = false;
                    Externalchart.Visible = false;
                    Excel.Visible = false;
                    Print.Visible = false;
                }

            }

            else
            {
                lblmsg.Visible = true;
                //lblmsg.Text = "No Records Found";
                internalgrid.Visible = false;
                Externalgrid.Visible = false;
                Chart1.Visible = false;
                Externalchart.Visible = false;
                Excel.Visible = false;
                Print.Visible = false;
            }


        }
        catch (Exception ex)
        {
            // lblTestname.Text = ex.ToString();
        }
    }

    protected void internalgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[2].Width = 200;
                e.Row.Cells[3].Width = 160;
                e.Row.Cells[1].Width = 100;

            }
        }

        catch (Exception ex)
        {

        }

    }
    protected void Externalgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.BackColor = System.Drawing.Color.WhiteSmoke;
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                //e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Center;
                e.Row.Cells[1].Width = 100;
                e.Row.Cells[3].Width = 350;


            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void internalgrid_OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        GridViewRow gvr = internalgrid.Rows[e.NewEditIndex];

        string serialNumber = gvr.Cells[2].Text;
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < 4; i++)
                {
                    if (previousRow.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }

            }
        }
    }

    public class GridDecoratorr
    {
        public static void MergeRows1(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                for (int i = 0; i < 3; i++)
                {
                    if (previousRow.Cells[i].Text == "&nbsp;")
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                    {
                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                               previousRow.Cells[i].RowSpan + 1;
                        previousRow.Cells[i].Visible = false;
                    }
                }

            }
        }
    }

    protected void internalgrid_OnPreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(internalgrid);
    }

    protected void Externalgrid_OnPreRender(object sender, EventArgs e)
    {
        GridDecoratorr.MergeRows1(Externalgrid);
    }


    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            //Response.ClearContent();
            //Response.AddHeader("content-disposition",
            //    "attachment;filename=SubjectwiseReport.xls");
            //Response.ContentType = "applicatio/excel";
            //StringWriter sw = new StringWriter();
            //btngo_OnClick(sender, e);
            //HtmlTextWriter htm = new HtmlTextWriter(sw);
            //internalgrid.RenderControl(htm);
            //Externalgrid.RenderControl(htm);
            //Response.Write(sw.ToString());
            //Response.End();

            try
            {
                string print = "";
                string appPath = HttpContext.Current.Server.MapPath("~");
                string strexcelname = "";
                if (appPath != "")
                {
                    strexcelname = txtexcelname.Text;
                    appPath = appPath.Replace("\\", "/");
                    if (strexcelname != "")
                    {
                        print = strexcelname;
                        //FpEntry.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
                        //Aruna on 26feb2013============================
                        string szPath = appPath + "/Report/";
                        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                        fpspread.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                        Response.Clear();
                        Response.ClearHeaders();
                        Response.ClearContent();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/vnd.ms-excel";
                        Response.Flush();
                        Response.WriteFile(szPath + szFile);
                        lblnorec.Text = "";


                        //=============================================
                    }
                    else
                    {
                        txtexcelname.Focus();
                        lblnorec.Text = "Please Enter Your Report Name";
                        lblnorec.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                // lblnorec.Text = ex.ToString();
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


    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {
            //Response.ContentType = "application/pdf";
            //Response.AddHeader("content-disposition", "attachment;filename=SubjectWiseReport.pdf");
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //StringWriter sw = new StringWriter();
            //HtmlTextWriter hw = new HtmlTextWriter(sw);
            //if (ddlrepttype.SelectedItem.Text == "Internal")
            //{
            //    btngo_OnClick(sender, e);
            //    internalgrid.AllowPaging = false;
            //    internalgrid.HeaderRow.Style.Add("width", "15%");
            //    internalgrid.HeaderRow.Style.Add("font-size", "12px");
            //    internalgrid.HeaderRow.Style.Add("text-align", "center");
            //    internalgrid.Style.Add("text-decoration", "none");
            //    internalgrid.Style.Add("font-family", "Bood Antiqua;");
            //    internalgrid.Style.Add("font-size", "10px");
            //    internalgrid.RenderControl(hw);
            //}
            //else
            //{
            //    btngo_OnClick(sender, e);
            //    Externalgrid.AllowPaging = false;
            //    Externalgrid.HeaderRow.Style.Add("width", "15%");
            //    Externalgrid.HeaderRow.Style.Add("font-size", "12px");
            //    Externalgrid.HeaderRow.Style.Add("text-align", "center");
            //    Externalgrid.Style.Add("text-decoration", "none");
            //    Externalgrid.Style.Add("font-family", "Bood Antiqua;");
            //    Externalgrid.Style.Add("font-size", "10px");
            //    Externalgrid.RenderControl(hw);
            //}
            //StringReader sr = new StringReader(sw.ToString());
            //Document pdfDoc = new Document(PageSize.A4, 7f, 7f, 7f, 0f);
            //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            //Paragraph p = new Paragraph();
            //string txt = "";
            //p.Add(txt);
            //pdfDoc.Open();
            //pdfDoc.Add(p);
            //htmlparser.Parse(sr);
            //pdfDoc.Close();
            //Response.Write(pdfDoc);
            //Response.End();


            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy") + "@";
            string pagename = "Subwise_Analy_rep.aspx";
            string degreedetails = " Subjectwise Analysis Report " + date;
            Printcontrol.loadspreaddetails(fpspread, pagename, degreedetails);
            Printcontrol.Visible = true;

        }

        catch (Exception ex)
        {

        }

    }



}