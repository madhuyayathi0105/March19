using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Configuration;
public partial class subjectwise_report : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();

    string group_user = "", singleuser = "", usercode = "", collegecode = "", group_code = "";
    string strquery = "";
    static string scrno = "";
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();
    protected void Page_Load(object sender, EventArgs e)
    {
        lblerrormsg.Visible = false;

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
        if (!IsPostBack)
        {
            bindschool();
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            //bindsubject();
        }
    }

    public void bindschool()
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
            ds.Clear();
            ds = d2.select_method("bind_college", hat, "sp");
            ddschool.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschool.DataSource = ds;
                ddschool.DataTextField = "collname";
                ddschool.DataValueField = "college_code";
                ddschool.DataBind();
            }
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }


    public void bindyear()
    {
        try
        {
            dropyear.Items.Clear();
            ds.Clear();
            ds = d2.select_method_wo_parameter("bind_batch", "sp");
            int count = ds.Tables[0].Rows.Count;
            if (count > 0)
            {
                dropyear.DataSource = ds;
                dropyear.DataTextField = "batch_year";
                dropyear.DataValueField = "batch_year";
                dropyear.DataBind();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                int max_bat = 0;
                max_bat = Convert.ToInt32(ds.Tables[1].Rows[0][0].ToString());
                dropyear.SelectedValue = max_bat.ToString();
            }
            dropyear.Text = "batch (" + 1 + ")";
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void bindschooltype()
    {
        try
        {
            ddschooltype.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = ddschool.SelectedItem.Value;
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
            ds.Clear();
            ds = d2.select_method("bind_degree", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddschooltype.DataSource = ds;
                ddschooltype.DataTextField = "course_name";
                ddschooltype.DataValueField = "course_id";
                ddschooltype.DataBind();
            }
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void bindstandard()
    {
        try
        {
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddschooltype.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds.Clear();
            ds = d2.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddstandard.DataSource = ds;
                ddstandard.DataTextField = "dept_name";
                ddstandard.DataValueField = "degree_code";
                ddstandard.DataBind();
            }
            bindsubject();
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    public void bindterm()
    {
        cblterm.Items.Clear();
        DataSet studgradeds = new DataSet();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        string strstandard = "";

        if (ddstandard.SelectedValue != "")
        {
            strstandard = ddstandard.SelectedValue;
        }

        if (strstandard.Trim() != "")
        {
            strstandard = " and degree_code in(" + strstandard + ")";
        }

        string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
        studgradeds.Reset();
        studgradeds.Dispose();
        //  studgradeds = d2.select_method_wo_parameter(strquery, "Text");
        studgradeds = d2.BindSem(ddstandard.Text.ToString(), dropyear.Text.ToString(), ddschool.SelectedValue.ToString());
        if (studgradeds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    cblterm.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    cblterm.Items.Add(i.ToString());
                }
            }

            if (cblterm.Items.Count > 0)
            {
                bindsec();
                int cout = 0;
                for (int iq = 0; iq < cblterm.Items.Count; iq++)
                {
                    cout++;
                    cblterm.Items[iq].Selected = true;
                }
                cbterm.Checked = true;
                txtterm.Text = "Term (" + cout + ")";
            }
            else
            {
                cbterm.Checked = false;
                txtterm.Text = "-Select-";
            }
        }
    }

    //public void bindterm()
    //{
    //    try
    //    {
    //        //dropterm.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        string strstandard = "";

    //        if (ddstandard.SelectedValue != "")
    //        {
    //            strstandard = ddstandard.SelectedValue;
    //        }

    //        if (strstandard.Trim() != "")
    //        {
    //            strstandard = " and degree_code in(" + strstandard + ")";
    //        }

    //        strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddschool.SelectedValue.ToString() + " and batch_year=" + dropyear.Text.ToString() + " and degree_code=" + ddstandard.Text.ToString() + " order by NDurations desc";
    //        ds.Reset();
    //        ds.Dispose();

    //        ds = d2.BindSem(ddstandard.Text.ToString(), dropyear.Text.ToString(), ddschool.SelectedValue.ToString());

    //        if (ds.Tables[0].Rows.Count > 0)
    //        {
    //            cblterm.DataSource = ds;
    //            cblterm.DataTextField = "ndurations";
    //            cblterm.DataValueField = "ndurations";
    //            cblterm.DataBind();
    //        }

    //        if (cblterm.Items.Count > 0)
    //        {
    //            int cout = 0;
    //            for (int iq = 0; iq < cblterm.Items.Count; iq++)
    //            {
    //                cout++;
    //                cblterm.Items[iq].Selected = true;
    //            }
    //            cbterm.Checked = true;
    //            txtterm.Text = "Term (" + cout + ")";
    //        }
    //        else
    //        {
    //            cbterm.Checked = false;
    //            txtterm.Text = "-Select-";
    //        }
    //        //if (ds.Tables[0].Rows.Count > 0)
    //        //{
    //        //    first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
    //        //    duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
    //        //    for (i = 1; i <= duration; i++)
    //        //    {
    //        //        if (first_year == false)
    //        //        {
    //        //            dropterm.Items.Add(i.ToString());
    //        //        }
    //        //        else if (first_year == true && i != 2)
    //        //        {
    //        //            dropterm.Items.Add(i.ToString());
    //        //        }
    //        //    }
    //        //    dropterm.Items.Insert(0, "ALL");
    //        //}
    //        bindsubject();
    //        FpSpread1.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}

    public void bindsec()
    {
        try
        {

            //dropsec.Enabled = false;
            //dropsec.Items.Clear();
            hat.Clear();
            ds.Clear();
            ds = d2.BindSectionDetail(dropyear.SelectedValue, ddstandard.SelectedValue);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblsec.Items.Clear();
                cblsec.DataSource = ds;
                cblsec.DataTextField = "sections";
                cblsec.DataValueField = "sections";
                cblsec.DataBind();
            }
            else
            {
                txtsec.Text = "-Select-";
                cbsec.Checked = false;
                //cblsec.Items.Clear();
            }

            if (cblsec.Items.Count > 0)
            {
                int cout = 0;
                for (int iq = 0; iq < cblsec.Items.Count; iq++)
                {
                    cout++;
                    cblsec.Items[iq].Selected = true;
                }
                cbsec.Checked = true;
                txtsec.Text = "Term (" + cout + ")";
            }
            else
            {
                cbsec.Checked = false;
                txtsec.Text = "-Select-";
            }

            //int count5 = ds.Tables[0].Rows.Count;
            //if (count5 > 0)
            //{
            //    dropsec.DataSource = ds;
            //    dropsec.DataTextField = "sections";
            //    dropsec.DataValueField = "sections";
            //    dropsec.DataBind();
            //    dropsec.Enabled = true;
            //    dropsec.Items.Insert(0, "ALL");
            //}

            //else
            //{
            //    dropsec.Enabled = false;
            //}
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void cbsec_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbsec.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    cout++;
                    cblsec.Items[i].Selected = true;
                    cbsec.Checked = true;
                    txtsec.Text = "Sec (" + cout + ")";
                }
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblsec.Items.Count; i++)
                {
                    cout++;
                    cblsec.Items[i].Selected = false;
                    txtsec.Text = "-Select-";
                    cbsec.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbsec.Checked = false;
            txtsec.Text = "-Select-";
            for (int i = 0; i < cblsec.Items.Count; i++)
            {
                if (cblsec.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                txtsec.Text = "Sec (" + cout + ")";
                if (cout == cblsec.Items.Count)
                {
                    cbsec.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    public void bindsubject()
    {
        try
        {
            ddlsubject.Items.Clear();
            hat.Clear();
            ds.Clear();

            string buildvalue1 = "";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    string build1 = cblterm.Items[i].Value.ToString();
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

            string subject = "select  distinct(s.subject_name),subject_code from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + dropyear.SelectedValue + "'  and degree_code = '" + ddstandard.SelectedValue + "' and semester in ('" + buildvalue1 + "') ; ";
            ds = d2.select_method_wo_parameter(subject, "text");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlsubject.DataSource = ds;
                ddlsubject.DataTextField = "subject_name";
                ddlsubject.DataValueField = "subject_code";
                ddlsubject.DataBind();
            }
            else
            {
            }
            //FpSpread1.Visible = false;
            //lblexportxl.Visible = false;
            //txtexcell.Visible = false;
            //btnexcel.Visible = false;
            //btnprint.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
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
    protected void ddschool_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindyear();
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            bindsubject();

            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void dropyear_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindschooltype();
            bindstandard();
            bindterm();
            bindsec();
            bindsubject();

            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void dropschooltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindstandard();
            bindterm();
            bindsec();
            bindsubject();

            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    protected void ddstandard_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindterm();
            bindsec();
            bindsubject();

            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
    //protected void dropterm_OnSelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        bindsec();

    //        FpSpread1.Visible = false;
    //        lblerrormsg.Visible = false;
    //    }
    //    catch (Exception ex)
    //    {
    //        lblerrormsg.Text = ex.ToString();
    //        lblerrormsg.Visible = true;
    //    }
    //}

    protected void cbterm_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbterm.Checked == true)
            {
                int cout = 0;
                for (int i = 0; i < cblterm.Items.Count; i++)
                {
                    cout++;
                    cblterm.Items[i].Selected = true;

                }
                cbterm.Checked = true;
                txtterm.Text = "Term (" + cout + ")";
            }
            else
            {
                int cout = 0;
                for (int i = 0; i < cblterm.Items.Count; i++)
                {
                    cout++;
                    cblterm.Items[i].Selected = false;

                }
                cbterm.Checked = false;
                txtterm.Text = "-Select-";
            }
            if (cblterm.Items.Count > 0)
            {
                bindsec();
            }
            bindsubject();
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void cblterm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            cbterm.Checked = false;
            txtterm.Text = "-Select-";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    cout++;
                }
            }
            if (cout > 0)
            {
                bindsec();
                txtterm.Text = "Term (" + cout + ")";
                if (cout == cblterm.Items.Count)
                {
                    cbterm.Checked = true;
                }
            }
            bindsubject();
        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    protected void dropsec_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            lblexportxl.Visible = false;
            txtexcell.Visible = false;
            btnexcel.Visible = false;
            btnprint.Visible = false;
            lblerrormsg.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddlsubject_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblerrormsg.Visible = false;
        lblexportxl.Visible = false;
        txtexcell.Visible = false;
        btnexcel.Visible = false;
        btnprint.Visible = false;
    }


    public string loadmarkat(string mr)
    {
        string strgetval = "";
        if (mr == "-1")
        {
            strgetval = "AAA";
        }
        else if (mr == "-2")
        {
            strgetval = "EL";
        }
        else if (mr == "-3")
        {
            strgetval = "EOD";
        }
        else if (mr == "-4")
        {
            strgetval = "ML";
        }
        else if (mr == "-5")
        {
            strgetval = "SOD";
        }
        else if (mr == "-6")
        {
            strgetval = "NSS";
        }
        else if (mr == "-7")
        {
            strgetval = "NJ";
        }
        else if (mr == "-8")
        {
            strgetval = "S";
        }
        else if (mr == "-9")
        {
            strgetval = "L";
        }
        else if (mr == "-10")
        {
            strgetval = "NCC";
        }
        else if (mr == "-11")
        {
            strgetval = "HS";
        }
        else if (mr == "-12")
        {
            strgetval = "PP";
        }
        else if (mr == "-13")
        {
            strgetval = "SYOD";
        }
        else if (mr == "-14")
        {
            strgetval = "COD";
        }
        else if (mr == "-15")
        {
            strgetval = "OOD";
        }
        else if (mr == "-16")
        {
            strgetval = "OD";
        }
        else if (mr == "-17")
        {
            strgetval = "LA";
        }
        else if (mr == "-18")
        {
            strgetval = "RAA";
        }
        return strgetval;
    }


    protected void btnexcel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcell.Text;

            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
                lblerror.Visible = false;
            }
            else
            {
                lblerror.Text = "Please Enter Your Report Name";
                lblerror.Visible = true;
            }
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void btnprint_OnClick(object sender, EventArgs e)
    {
        try
        {
            //string degreedetails = "PERFORMANCE COMPARISON REPORT" + '@' + "                                                                                                  " + "BATCHWISE PERFORMANCE COMPARISON" + '@';

            string degreedetails = "  " + '@' + "                       Subject Name: " + ddlsubject.SelectedItem.Text + "                                                        " + "Standard: " + ddstandard.SelectedItem.Text + "                                              " + "Year: " + dropyear.SelectedItem.Text + '@';
            string pagename = "pcreport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            //ArrayList gradesettings = new ArrayList();
            //gradesettings.Clear();

            //gradesettings.Add("T1");
            //gradesettings.Add("T2");
            //gradesettings.Add("T3");

            ArrayList gradecol = new ArrayList();
            gradecol.Clear();


            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnCount = 0;
            FpSpread1.Sheets[0].SheetCorner.ColumnCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 3;
            FpSpread1.Sheets[0].ColumnCount = 3;
            FpSpread1.CommandBar.Visible = false;
            FpSpread1.Sheets[0].AutoPostBack = true;

            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style2 = new FarPoint.Web.Spread.StyleInfo();
            style2.Font.Size = 13;
            style2.Font.Name = "Book Antiqua";
            style2.Font.Bold = true;
            style2.HorizontalAlign = HorizontalAlign.Center;
            style2.ForeColor = System.Drawing.Color.White;
            style2.BackColor = System.Drawing.Color.Teal;
            FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style2);

            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Admn. No.";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name of the Student";

            FpSpread1.Sheets[0].Columns[2].Width = 70;
            FpSpread1.Sheets[0].Columns[2].Width = 150;

            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 3, 1);
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 3, 1);

            string batch_year = dropyear.SelectedItem.Text;
            string degree_code = ddstandard.SelectedValue;
            string subject_code = ddlsubject.SelectedValue;
            string semester = "", istype = "", criteriano = "", conversionvalue = "", subno_query4 = "";
            string convertinsvla = ""; string exmark = "";

            ArrayList fabtotal = new ArrayList();

            int cnt = 0, kz = 0, kz1 = 0; Hashtable ht = new Hashtable();
            int cnt1 = 0, sno = 1, jm = 1, sm = 1, ms = 1, lm = 0; Hashtable ht1 = new Hashtable();
            double val = 0.0, val1 = 0.0, val2 = 0.0, val3 = 0.0, val4 = 0.0, val5 = 0.0, val6 = 0.0, val7 = 0.0, val8 = 0.0, totalab = 0.0;

            string buildvalue1 = ""; string crtiriano = "", sem = "";
            for (int i = 0; i < cblterm.Items.Count; i++)
            {
                if (cblterm.Items[i].Selected == true)
                {
                    string build1 = cblterm.Items[i].Value.ToString();
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

            //// ----------------- start
            //string query1 = "select r.App_No,Roll_No,Reg_No,roll_admit,CONVERT(VARCHAR(30),r.Adm_Date,103) AS adm_date,r.stud_name,r.Batch_Year,r.degree_code,d.Dept_Name, r.Sections ,r.Current_Semester,CONVERT(VARCHAR, dob, 103) as dob,parent_name,mother,parent_addressP,Streetp,Cityp,parent_pincodep,student_mobile from Registration r,applyn a,Degree g,Department d,course c where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code and g.Course_Id = c.Course_Id  and g.college_code = c.college_code and r.Current_Semester in ('" + buildvalue1 + "') and r.degree_code='" + degree_code + "' ";

            string query1 = "SELECT distinct r.Roll_No,R.Stud_Name,a.sex,r.Roll_Admit FROM Registration R,Applyn A WHERE R.App_No = A.App_No and r.college_code='" + ddschool.SelectedValue + "' and r.Batch_Year='" + batch_year + "' and r.degree_code='" + degree_code + "' and r.CC=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' order by r.Roll_No";
            DataSet dset = d2.select_method_wo_parameter(query1, "text");

            if (dset.Tables[0].Rows.Count > 0)
            {
                for (int h = 0; h < dset.Tables[0].Rows.Count; h++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    sno++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txtceltype;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = dset.Tables[0].Rows[h]["Roll_Admit"].ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = dset.Tables[0].Rows[h]["Roll_Admit"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = dset.Tables[0].Rows[h]["Stud_Name"].ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = dset.Tables[0].Rows[h]["Stud_Name"].ToString();

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;

                    FpSpread1.Visible = true;
                    lblexportxl.Visible = true;
                    txtexcell.Visible = true;
                    btnexcel.Visible = true;
                    btnprint.Visible = true;
                    Printcontrol.Visible = false;
                }
            }

            DataSet dset5 = new DataSet();
            ArrayList add = new ArrayList();
            ArrayList addgrd = new ArrayList();

            DataTable dtab = new DataTable();
            dtab.Columns.Add("columnno");
            dtab.Columns.Add("criteriano");
            dtab.Columns.Add("criterianame");
            dtab.Columns.Add("semestr");

            DataTable dtab1 = new DataTable();
            dtab1.Columns.Add("columnno");
            dtab1.Columns.Add("criteriano");
            dtab1.Columns.Add("criterianame");

            if (ddlsubject.Items.Count > 0)
            {
                if (dset.Tables[0].Rows.Count > 0)
                {
                    for (int rl = 0; rl < dset.Tables[0].Rows.Count; rl++)
                    {
                        string dvroll = dset.Tables[0].Rows[rl]["Roll_No"].ToString();

                        string query3 = "SELECT distinct  Istype,CRITERIA_NO,y.semester,M.Conversion_value FROM internal_cam_calculation_master_setting M,syllabus_master Y WHERE M.syll_code = Y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + buildvalue1 + "')  and CRITERIA_NO <>''  order by semester";
                        DataSet dset2 = d2.select_method_wo_parameter(query3, "text");

                        if (dset2.Tables[0].Rows.Count > 0)
                        {
                            // ---------------- loop for get total semester.. eg: 1, 2, 3
                            for (int q3 = 0; q3 < dset2.Tables[0].Rows.Count; q3++)
                            {
                                sem = dset2.Tables[0].Rows[q3]["semester"].ToString();
                                FpSpread1.Visible = true;

                                if (!ht.ContainsKey(dset2.Tables[0].Rows[q3]["semester"].ToString() + "-" + dset.Tables[0].Rows[rl]["Roll_No"].ToString()))
                                {
                                    ht.Add(dset2.Tables[0].Rows[q3]["semester"].ToString() + "-" + dset.Tables[0].Rows[rl]["Roll_No"].ToString(), cnt);
                                    cnt++;
                                    dset2.Tables[0].DefaultView.RowFilter = "semester='" + sem + "'";
                                    DataView dvsem = dset2.Tables[0].DefaultView;

                                    if (dvsem.Count > 0)
                                    {
                                        string semestr1 = dvsem[0]["semester"].ToString();

                                        if (!ht1.ContainsKey(dvsem[0]["semester"].ToString()))
                                        {
                                            ht1.Add(dvsem[0]["semester"].ToString(), cnt1);
                                            cnt1++;
                                            // -------------- loop for get Semester.. eg: 1 
                                            for (int dsem = 0; dsem < dvsem.Count; dsem++)
                                            {
                                                crtiriano = dvsem[dsem]["CRITERIA_NO"].ToString(); // -- get Criteria no.. eg: 178,179,180,181
                                                string istype1 = dvsem[dsem]["istype"].ToString();

                                                string[] arrsc = crtiriano.Split(',');
                                                if (arrsc.Length > 0)
                                                {
                                                    // -------- using Getfunction() ************** query4 for get subject no.. eg: 863
                                                    subno_query4 = d2.GetFunction("select  subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + sem + "') and subject_code='" + subject_code + "' order by subject_no ; ");

                                                    string query5 = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + sem + "') and roll_no='" + dvroll + "'  and Criteria_no='" + crtiriano + "' and s.subject_no='" + subno_query4 + "'";
                                                    dset5 = d2.select_method_wo_parameter(query5, "text");

                                                    if (dset5.Tables[0].Rows.Count > 0)
                                                    {
                                                        string convertinsvla1 = dset5.Tables[0].Rows[0]["conversion"].ToString();
                                                        string critno = dset5.Tables[0].Rows[0]["Criteria_no"].ToString();
                                                        exmark = dset5.Tables[0].Rows[0]["Exammark"].ToString();

                                                        // --------------- loop for split criteria no.. eg: 178
                                                        foreach (string splittype in arrsc)
                                                        {
                                                            string query6 = "select distinct  reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + subno_query4 + "' and et.subject_no=sc.subject_no and et.criteria_no in ('" + splittype + "') and r.roll_no='" + dvroll + "'  ORDER BY len(reg.roll_no),reg.roll_no";
                                                            DataSet dset6 = d2.select_method_wo_parameter(query6, "text");

                                                            if (dset6.Tables[0].Rows.Count > 0)
                                                            {
                                                                convertinsvla = dset6.Tables[0].Rows[0]["max_mark"].ToString();
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "Term " + sem;
                                                                dtab1.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, sem, "Semestr");

                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = istype1;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = istype1;
                                                                if (istype1.Contains('S'))
                                                                {
                                                                    FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "SA";
                                                                }
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = convertinsvla;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Tag = splittype;
                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, splittype, istype1, sem);
                                                                kz++; kz1++;
                                                            }
                                                        }

                                                        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - kz1, 1, kz1);
                                                        kz1 = 0;

                                                        //// -------------- adding additional columns start
                                                        lm++;
                                                        if (dsem != lm)
                                                        {
                                                            if (sm == 1)
                                                            {
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA (a)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "FA (a)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = convertinsvla1;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Tag = critno;
                                                                scrno = critno;
                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, critno, "FA (a)", sem);

                                                                val = val + Convert.ToDouble(convertinsvla1);
                                                                lm = 0; sm++; kz++;
                                                            }
                                                            else
                                                            {
                                                                val2 = val + Convert.ToDouble(convertinsvla);
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Total Marks";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(val2);
                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, "Total", "Total Marks", sem);
                                                                kz++;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Grade";
                                                                gradecol.Add(FpSpread1.Sheets[0].ColumnCount - 1);
                                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(1, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, "Grade", "Grade", sem);
                                                                lm++; kz++;
                                                            }
                                                        }
                                                        else if (dsem == lm)
                                                        {
                                                            if (ms == 1)
                                                            {
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA (b)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "FA (b)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = convertinsvla1;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Tag = critno;
                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, critno, "FA (b)", sem);

                                                                val = val + Convert.ToDouble(convertinsvla1);
                                                                kz++;
                                                                FpSpread1.Sheets[0].ColumnCount++;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Term " + sem;
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Text = "FA Total (a+b)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].ColumnCount - 1].Tag = "FA Total (a+b)";
                                                                FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(val);

                                                                dtab.Rows.Add(FpSpread1.Sheets[0].ColumnCount - 1, scrno + "," + critno, "FA Total (a+b)", sem);
                                                                lm++; ms++; kz++;
                                                            }
                                                        }
                                                        //// -------------- adding additional columns end
                                                        //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - kz, 1, kz);
                                                        //kz = 0;
                                                    }
                                                }
                                            }
                                            sm = 1; lm = 0; ms = 1; val = 0.0;

                                            if (dset5.Tables[0].Rows.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - kz, 1, kz);
                                                kz = 0;
                                            }
                                            else
                                            {
                                                //FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - kz, 1, kz);
                                                //kz = 0;
                                            }
                                        }
                                    }
                                }
                            }

                            //// ---------------- mark bind start
                            int dtp = dtab1.Rows.Count;
                            string h_sem = "";
                            for (int col = 0; col < dtab.Rows.Count; col++)
                            {
                                string h_columno = Convert.ToString(dtab.Rows[col]["columnno"].ToString());
                                string h_criteriano = Convert.ToString(dtab.Rows[col]["criteriano"].ToString());
                                string h_colname = Convert.ToString(dtab.Rows[col]["criterianame"].ToString());
                                h_sem = Convert.ToString(dtab.Rows[col]["semestr"].ToString());

                                subno_query4 = d2.GetFunction("select  subject_no from subject s,syllabus_master y where s.syll_code = y.syll_code and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + h_sem + "') and subject_code='" + subject_code + "' order by subject_no ; ");

                                // ------------- add start
                                string[] arr = h_criteriano.Split(',');
                                int len = Convert.ToInt32(arr.Length);
                                if (len != 1)
                                {
                                    if (h_colname != "FA Total (a+b)")
                                    {
                                        string query5a = "SELECT * FROM tbl_Cam_Calculation C,internal_cam_calculation_master_setting S,syllabus_master y WHERE C.Istype = S.Istype  AND C.syll_code = Y.SYLL_CODE and c.subject_no=s.subject_no  and y.Batch_Year = '" + batch_year + "' and degree_code = '" + degree_code + "' and semester in ('" + h_sem + "') and roll_no='" + dvroll + "'  and Criteria_no='" + h_criteriano + "' and s.subject_no='" + subno_query4 + "'";
                                        DataSet dset5a = d2.select_method_wo_parameter(query5a, "text");

                                        if (dset5a.Tables[0].Rows.Count > 0)
                                        {
                                            string convertinsvla1 = dset5a.Tables[0].Rows[0]["conversion"].ToString();
                                            string critno = dset5a.Tables[0].Rows[0]["Criteria_no"].ToString();
                                            string exmark1 = dset5a.Tables[0].Rows[0]["Exammark"].ToString();

                                            FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(exmark1);
                                            FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = exmark1;
                                            FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;

                                            val4 = val4 + Convert.ToDouble(exmark1);
                                        }
                                        else
                                        {
                                            FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString("0");
                                            FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = "0";
                                            FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(val4);
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = val4;
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                }
                                else
                                {
                                    if (h_colname != "Total Marks")
                                    {
                                        if (h_colname != "Grade")
                                        {
                                            string findfatotal = "select distinct  reg.roll_no,r.marks_obtained,r.exam_code,et.max_mark,len(reg.roll_no),reg.reg_no,reg.serialno,reg.stud_name from result r,registration reg,exam_type et,subjectchooser sc  where r.exam_code=et.exam_code  and reg.roll_no=r.roll_no and sc.roll_no=reg.roll_no and reg.cc=0 and reg.delflag=0 and reg.exam_flag <>'Debar'  and et.subject_no='" + subno_query4 + "' and et.subject_no=sc.subject_no and et.criteria_no in ('" + h_criteriano + "') and r.roll_no='" + dvroll + "'  ORDER BY len(reg.roll_no),reg.roll_no";
                                            DataSet dsfindfatotal = d2.select_method_wo_parameter(findfatotal, "text");

                                            if (dsfindfatotal.Tables[0].Rows.Count > 0)
                                            {
                                                double markobtained = Convert.ToDouble(dsfindfatotal.Tables[0].Rows[0]["marks_obtained"].ToString());

                                                if (markobtained >= 0)
                                                {
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(markobtained);
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = markobtained;
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                                    val5 = markobtained;
                                                }
                                                else
                                                {
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Text = loadmarkat(Convert.ToString(markobtained));
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = markobtained;
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                                }
                                            }
                                            else
                                            {
                                                FpSpread1.Sheets[0].Cells[rl, col + 3].Text = "0";
                                                FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                            }
                                        }
                                        else
                                        {
                                            //convertedvalue= Convert.ToDouble(FpSpread1.Sheets[0].ColumnHeader.Cells[2, col + 2].Text.ToString());
                                            val6 = val4 + val5;
                                            //val6=(val6/convertedvalue);
                                            //val6 = val6 * 100;
                                            // -------------- add grade start
                                            string gradequery1 = "SELECT * from Grade_Master where Semester='" + h_sem + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val6 + "' between Frange and Trange";
                                            DataSet dsgradechk = d2.select_method_wo_parameter(gradequery1, "Text");
                                            if (dsgradechk.Tables[0].Rows.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = val6;
                                                FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;

                                            }
                                            else
                                            {
                                                gradequery1 = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val6 + "' between Frange and Trange";
                                                dsgradechk.Clear();
                                                dsgradechk = d2.select_method_wo_parameter(gradequery1, "Text");
                                                if (dsgradechk.Tables[0].Rows.Count > 0)
                                                {
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = val6;
                                                    FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;
                                                }


                                            }
                                            // -------------- add grade end

                                            addgrd.Add(val6);
                                            val6 = 0.0; val4 = 0.0; val5 = 0.0;
                                        }
                                    }
                                    else
                                    {
                                        val6 = val4 + val5;
                                        val7 = val7 + val6;
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].Text = Convert.ToString(val6);
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].Tag = val6;
                                        FpSpread1.Sheets[0].Cells[rl, col + 3].HorizontalAlign = HorizontalAlign.Center;

                                        add.Add(val7);
                                        //val6 = 0.0; val4 = 0.0; val5 = 0.0;
                                    }
                                }
                                // ------------- add end
                                //}
                                if (jm == 1)
                                {
                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Overall Marks";
                                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
                                    FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(val7);
                                    FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    //val7 = 0.0;

                                    FpSpread1.Sheets[0].ColumnCount++;
                                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Overall Grade";


                                    //val7 = (val7 / overallconverted);
                                    //        val7 = val7 * 100;

                                    // ---------- add grade start 1
                                    string gradequery2 = "SELECT * from Grade_Master where Semester='" + h_sem + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val7 + "' between Frange and Trange";
                                    DataSet dsgradechk = d2.select_method_wo_parameter(gradequery2, "Text");
                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Tag = val6;
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                    else
                                    {
                                        gradequery2 = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val7 + "' between Frange and Trange";
                                        dsgradechk.Clear();
                                        dsgradechk = d2.select_method_wo_parameter(gradequery2, "Text");
                                        if (dsgradechk.Tables[0].Rows.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Tag = val6;
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        }


                                    }
                                    // ---------- add grade end 1

                                    val7 = 0.0;
                                    jm++;
                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 2].Text = Convert.ToString(val7);
                                    // ---------- add grade start 2
                                    //val7 = (val7 / overallconverted);
                                    //val7 = val7 * 100;
                                    string gradequery3 = "SELECT * from Grade_Master where Semester='" + h_sem + "' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val7 + "' between Frange and Trange";
                                    DataSet dsgradechk = d2.select_method_wo_parameter(gradequery3, "Text");
                                    if (dsgradechk.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Tag = val6;
                                        FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;

                                    }
                                    else
                                    {


                                        gradequery3 = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + val7 + "' between Frange and Trange";
                                        dsgradechk.Clear();
                                        dsgradechk = d2.select_method_wo_parameter(gradequery3, "Text");
                                        if (dsgradechk.Tables[0].Rows.Count > 0)
                                        {
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].Tag = val6;
                                            FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        }


                                    }
                                    // ---------- add grade end 2

                                    FpSpread1.Sheets[0].Cells[rl, FpSpread1.Sheets[0].ColumnCount - 2].HorizontalAlign = HorizontalAlign.Center;

                                    //val7 = 0.0;
                                }
                            }
                            val7 = 0.0;
                            //// ---------------- mark bind end
                        }
                    }

                    FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 3, 1);
                }
                else
                {
                    FpSpread1.Visible = false;
                    lblexportxl.Visible = false;
                    txtexcell.Visible = false;
                    btnexcel.Visible = false;
                    btnprint.Visible = false;
                    lblerrormsg.Text = "No Records Found";
                    lblerrormsg.Visible = true;
                }
            }
            else
            {
                FpSpread1.Visible = false;
                lblexportxl.Visible = false;
                txtexcell.Visible = false;
                btnexcel.Visible = false;
                btnprint.Visible = false;
                lblerrormsg.Text = "Please Select Subject";
                lblerrormsg.Visible = true;
            }

            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                double overallconvert = 0;
                DataSet dsgradechk = new DataSet();
                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {
                    if (gradecol.Count > 0)
                    {
                        for (int j = 0; j < gradecol.Count; j++)
                        {
                            string colnoo = gradecol[j].ToString();
                            double valuemark = Convert.ToDouble(FpSpread1.Sheets[0].Cells[i, (Convert.ToInt32(colnoo) - 1)].Text.ToString());
                            double convrtedvalue = Convert.ToDouble(FpSpread1.Sheets[0].ColumnHeader.Cells[2, (Convert.ToInt32(colnoo) - 1)].Text.ToString());
                            if (i == 0)
                            {
                                overallconvert = overallconvert + convrtedvalue;
                            }
                            valuemark = (valuemark / convrtedvalue);
                            valuemark = valuemark * 100;
                            string gradequery3 = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + valuemark + "' between Frange and Trange";
                            dsgradechk.Clear();
                            dsgradechk = d2.select_method_wo_parameter(gradequery3, "Text");
                            if (dsgradechk.Tables[0].Rows.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colnoo)].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                                //FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colnoo)].Tag = val6;

                            }

                        }
                    }


                }


                for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count; i++)
                {

                    double valuemark = Convert.ToDouble(FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 2].Text.ToString());
                    //double convrtedvalue = Convert.ToDouble(FpSpread1.Sheets[0].ColumnHeader.Cells[2, FpSpread1.Sheets[0].ColumnCount - 3].Text.ToString());
                    //  overallconvert = overallconvert + convrtedvalue;
                    valuemark = (valuemark / overallconvert);
                    valuemark = valuemark * 100;
                    string gradequery3 = "SELECT * from Grade_Master where Semester='0' and College_Code='" + Session["collegecode"] + "' and Degree_Code='" + degree_code + "' and batch_year='" + batch_year + "'  and Criteria ='General' and  '" + valuemark + "' between Frange and Trange";
                    dsgradechk.Clear();
                    dsgradechk = d2.select_method_wo_parameter(gradequery3, "Text");
                    if (dsgradechk.Tables[0].Rows.Count > 0)
                    {
                        FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(dsgradechk.Tables[0].Rows[0]["Mark_Grade"].ToString());
                        //FpSpread1.Sheets[0].Cells[i, Convert.ToInt32(colnoo)].Tag = val6;

                    }


                }
            }

            if (FpSpread1.Sheets[0].ColumnCount == 3)
            {
                FpSpread1.Visible = false;
                prntrpt.Visible = false;
                lblerrormsg.Text = "No Records Found";
                lblerrormsg.Visible = true;
            }
            else
            {
                lblerrormsg.Visible = false;
                FpSpread1.Visible = true;
                prntrpt.Visible = true;
            }
            //}
            //// ----------------- end

        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }
    }
}