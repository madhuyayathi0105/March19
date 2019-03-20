using System;
using System.Collections;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Drawing;
using System.Configuration;

public partial class Internal_Report : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string examcodeval = string.Empty;
    string strgrade = string.Empty;
    string strsec = string.Empty;
    string sturollno = string.Empty;
    string strsubcrd = string.Empty;
    string graders = string.Empty;
    string sqlstr = string.Empty;
    string strsection = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbatchyear = string.Empty;
    string strbranch = string.Empty;
    string strsem = string.Empty;

    double passcnt = 0;
    double totapp = 0;
    double passpercen = 0;
    double passpercen_round = 0;
    //double colpasscnt = 0;
    //double coltotapp = 0;
    double totpasspercen = 0;
    double totpasspercen_round = 0;
    double rowapp = 0;
    double rowpasspercen = 0;
    //double colapp = 0;
    //double colpasspercen = 0;
    double rowtotapp = 0;
    double rowtotpasscnt = 0;
    //double coltotapp = 0;
    //double coltotpasscnt = 0;
    double rowpercen = 0;
    double rowpercen_round = 0;
    //double colpercen = 0;
    //double colpercen_round = 0;
    double appcnt = 0;
    double totpasscnt = 0;

    string syllcode = string.Empty;
    string subname = string.Empty;
    string section = string.Empty;
    string colheader = string.Empty;
    string rowheader = string.Empty;
    string staff_name = string.Empty;

    static int batchcnt = 0;
    static int degreecnt = 0;
    static int branchcnt = 0;
    static int sectioncnt = 0;
    static int subjectcnt = 0;

    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    int count4 = 0;

    Hashtable hat = new Hashtable();
    Hashtable hashappcnt = new Hashtable();
    Hashtable hashpasscnt = new Hashtable();
    Hashtable hashcolappcnt = new Hashtable();
    Hashtable hashcolpasscnt = new Hashtable();

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet1();
    DataSet ds2 = new DataSet1();

    DataSet dsbind = new DataSet();
    DataTable dt = new DataTable();
    DataTable data = new DataTable();
    //---------Page_Load Functions-------
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
        lblxlerr.Visible = false;
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();
        lblxlerr.Visible = false;
        errmsg.Visible = false;
        if (!IsPostBack)
        {

            Showgrid.Visible = false;
            btnmasterprint.Visible = false;
            btnPrint.Visible = false;
            //Added By Srianth 28/2/2013
            btnxl.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            //btnxl.Visible = false;
            //btnpdf.Visible = false;
            //norecordlbl.Visible = false;
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            if (chklstdegree.Items.Count >= 1)
            {
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                BindSectionDetail(strbatch, strbranch);
                BindSubject(strbatch, strbranch, strsem, strsec);
                BindTest(strbatch, strbranch);
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Give degree rights to staff";
            }
        }
    }

    //------Load Function for the Batch Details-----
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbatch.DataSource = ds2;
                chklstbatch.DataTextField = "Batch_year";
                chklstbatch.DataValueField = "Batch_year";
                chklstbatch.DataBind();
                chklstbatch.SelectedIndex = chklstbatch.Items.Count - 2;
                txtbatch.Text = "Batch(1)";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Degree Details-----
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
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
                txtdegree.Text = "Degree(1)";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Branch Details-----
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
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
            //course_id = chklstdegree.SelectedValue.ToString();
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstbranch.DataSource = ds2;
                chklstbranch.DataTextField = "dept_name";
                chklstbranch.DataValueField = "degree_code";
                chklstbranch.DataBind();
                chklstbranch.Items[0].Selected = true;
                txtbranch.Text = "Branch(1)";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Section Details-----
    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            //strbranch = chklstbranch.SelectedValue.ToString();
            chklstsection.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstsection.DataSource = ds2;
                chklstsection.DataTextField = "sections";
                chklstsection.DataBind();
                //chklstsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklstsection.Enabled = false;
                }
                else
                {
                    chklstsection.Enabled = true;
                    chklstsection.SelectedIndex = chklstsection.Items.Count - 2;
                    for (int i = 0; i < chklstsection.Items.Count; i++)
                    {
                        chklstsection.Items[i].Selected = true;
                        if (chklstsection.Items[i].Selected == true)
                        {
                            count3 += 1;
                        }
                        if (chklstsection.Items.Count == count3)
                        {
                            chksection.Checked = true;
                            txtsection.Text = "Section(" + count3 + ")";
                        }
                    }
                }
                chklstsection.Items.Insert(0, "Empty Section");
            }
            else
            {
                chklstsection.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Semester Details-----
    //public void BindSem(string strbranch, string strbatchyear, string collegecode)
    //{
    //    try
    //    {
    //        for (int j = 0; j < chklstbatch.Items.Count; j++)
    //        {
    //            if (chklstbatch.Items[j].Selected == true)
    //            {
    //                if (strbatchyear == "")
    //                {
    //                    strbatchyear = "'" + chklstbatch.Items[j].Value.ToString() + "'";
    //                }
    //                else
    //                {
    //                    strbatchyear = strbatchyear + "," + "'" + chklstbatch.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        for (int j = 0; j < chklstbranch.Items.Count; j++)
    //        {
    //            if (chklstbranch.Items[j].Selected == true)
    //            {
    //                if (strbranch == "")
    //                {
    //                    strbranch = "'" + chklstbranch.Items[j].Value.ToString() + "'";
    //                }
    //                else
    //                {
    //                    strbranch = strbranch + "," + "'" + chklstbranch.Items[j].Value.ToString() + "'";
    //                }
    //            }
    //        }
    //        //strbatchyear = chklstbatch.Text.ToString();
    //        //strbranch = chklstbranch.SelectedValue.ToString();
    //        ddlsemester.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        ds2.Dispose();
    //        ds2.Reset();
    //        ds2 = d2.BindSem(strbranch, strbatchyear, collegecode);
    //        if (ds2.Tables[0].Rows.Count > 0)
    //        {
    //            int rowcount = Convert.ToInt32(ds2.Tables[0].Rows.Count);
    //            first_year = Convert.ToBoolean(Convert.ToString(ds2.Tables[0].Rows[rowcount-1][1]).ToString());
    //            duration = Convert.ToInt32(Convert.ToString(ds2.Tables[0].Rows[rowcount-1][0]).ToString());
    //            for (i = 1; i <= duration; i++)
    //            {
    //                if (first_year == false)
    //                {
    //                    ddlsemester.Items.Add(i.ToString());
    //                }
    //                else if (first_year == true && i != 2)
    //                {
    //                    ddlsemester.Items.Add(i.ToString());
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Text = ex.ToString();
    //    }
    //}
    //------Load Function for the Subject Details-----
    public void BindSubject(string strbatch, string strbranch, string strsem, string strsec)
    {
        try
        {
            //strbatch = chklstbatch.SelectedValue.ToString();
            //strbranch = chklstbranch.SelectedValue.ToString();
            //strsem = ddlsemester.SelectedValue.ToString();
            //strsec = chklstsection.SelectedValue.ToString();
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                if (chklstsection.Items[i].Selected == true)
                {
                    if (chklstsection.Items[i].Text.ToString() == "Empty Section")
                    {
                        if (strsec == "")
                        {
                            strsec = "'','-1'";
                        }
                        else
                        {
                            strsec = strsec + "," + "'','-1'";
                        }
                    }
                    else
                    {
                        if (strsec == "")
                        {
                            strsec = "'" + chklstsection.Items[i].Value.ToString() + "'";
                        }
                        else
                        {
                            strsec = strsec + "," + "'" + chklstsection.Items[i].Value.ToString() + "'";
                        }
                    }
                }
            }
            ds2.Dispose();
            ds2.Reset();
            //BindparticularstaffSubject(string strbatch, string strbranch, string strsem, string strsec,string staffcode)
            if (Session["Staff_Code"].ToString() == "")
            {
                ds2 = d2.BindSubject(strbatch, strbranch, strsem, strsec);
            }
            else if (Session["Staff_Code"].ToString() != "")
            {
                ds2 = d2.BindparticularstaffSubject(strbatch, strbranch, strsem, strsec, Session["Staff_Code"].ToString());
            }
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklstsubject.DataSource = ds2;
                chklstsubject.DataTextField = "subject_name";
                chklstsubject.DataValueField = "subject_name";
                chklstsubject.DataBind();
                //chklstsubject.SelectedIndex = chklstsubject.Items.Count - 1;
                for (int i = 0; i < chklstsubject.Items.Count; i++)
                {
                    chklstsubject.Items[i].Selected = true;
                    if (chklstsubject.Items[i].Selected == true)
                    {
                        count4 += 1;
                    }
                    if (chklstsubject.Items.Count == count4)
                    {
                        chksubject.Checked = true;
                        txtsubject.Text = "Subject(" + chklstsubject.Items.Count + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the Test Details-----
    public void BindTest(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                if (chklstbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklstbatch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    if (strbranch == "")
                    {
                        strbranch = "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbranch = strbranch + "," + "'" + chklstbranch.Items[i].Value.ToString() + "'";
                    }
                }
            }
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindTest(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddltest.DataSource = ds2;
                ddltest.DataTextField = "criteria";
                ddltest.DataValueField = "criteria";
                ddltest.DataBind();
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    //------Load Function for the DropdownBox Details------
    //protected void ddlsemester_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        //norecordlbl.Visible = false;
    //        FpSpread1.Visible = false;
    //        //btnxl.Visible = false;
    //        //btnpdf.Visible = false;
    //        //if (!Page.IsPostBack == false)
    //        //{
    //        //    ddlsemester.Items.Clear();
    //        //}
    //        BindSectionDetail(strbatch, strbranch);
    //        BindSubject(strbatch, strbranch, strsem,strsec);
    //        BindTest(strbatch,strbranch);
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Text = ex.ToString();
    //    }
    //}
    //----------Batch Dropdown Extender-----------------
    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = true;
                txtbatch.Text = "Batch(" + (chklstbatch.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstbatch.Items.Count; i++)
            {
                chklstbatch.Items[i].Selected = false;
                txtbatch.Text = "---Select---";
            }
        }
    }

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbatch.Focus();
        int batchcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstbatch.Items.Count; i++)
        {
            if (chklstbatch.Items[i].Selected == true)
            {
                value = chklstbatch.Items[i].Text;
                code = chklstbatch.Items[i].Value.ToString();
                batchcount = batchcount + 1;
                txtbatch.Text = "Batch(" + batchcount.ToString() + ")";
            }
        }
        if (batchcount == 0)
            txtbatch.Text = "---Select---";
        else
        {
            Label lbl = batchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = batchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(batchimg_Click);
        }
        batchcnt = batchcount;
        if (chklstdegree.Items.Count >= 1)
        {
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }

    protected void LinkButtonbatch_Click(object sender, EventArgs e)
    {
        chklstbatch.ClearSelection();
        batchcnt = 0;
        txtbatch.Text = "---Select---";
    }

    public void batchimg_Click(object sender, ImageClickEventArgs e)
    {
        batchcnt = batchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbatch.Items[r].Selected = false;
        txtbatch.Text = "Batch(" + batchcnt.ToString() + ")";
        if (txtbatch.Text == "Batch(0)")
        {
            txtbatch.Text = "---Select---";
        }
    }

    public Label batchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton batchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //----------Degree Dropdown Extender-----------------
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
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
        if (chklstdegree.Items.Count != 0)
        {
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        pdegree.Focus();
        int degreecount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstdegree.Items.Count; i++)
        {
            if (chklstdegree.Items[i].Selected == true)
            {
                value = chklstdegree.Items[i].Text;
                code = chklstdegree.Items[i].Value.ToString();
                degreecount = degreecount + 1;
                txtdegree.Text = "Degree(" + degreecount.ToString() + ")";
            }
        }
        if (degreecount == 0)
            txtdegree.Text = "---Select---";
        else
        {
            Label lbl = degreelabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = degreeimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(degreeimg_Click);
        }
        degreecnt = degreecount;
        if (degreecount != 0)
        {
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }

    protected void LinkButtondegree_Click(object sender, EventArgs e)
    {
        chklstdegree.ClearSelection();
        degreecnt = 0;
        txtdegree.Text = "---Select---";
    }

    public void degreeimg_Click(object sender, ImageClickEventArgs e)
    {
        degreecnt = degreecnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstdegree.Items[r].Selected = false;
        txtdegree.Text = "Degree(" + degreecnt.ToString() + ")";
        if (txtdegree.Text == "Degree(0)")
        {
            txtdegree.Text = "---Select---";
        }
    }

    public Label degreelabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton degreeimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //----------Branch Dropdown Extender-----------------
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
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
                chklstbranch.Items[i].Selected = false;
                txtbranch.Text = "---Select---";
            }
        }
        if (chklstbranch.Items.Count != 0)
        {
            BindSectionDetail(strbatch, strbranch);
            BindSubject(strbatch, strbranch, strsem, strsec);
            BindTest(strbatch, strbranch);
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        pbranch.Focus();
        int branchcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                value = chklstbranch.Items[i].Text;
                code = chklstbranch.Items[i].Value.ToString();
                branchcount = branchcount + 1;
                txtbranch.Text = "Branch(" + branchcount.ToString() + ")";
            }
        }
        if (branchcount == 0)
            txtbranch.Text = "---Select---";
        else
        {
            Label lbl = branchlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = branchimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(branchimg_Click);
        }
        branchcnt = branchcount;
        //BindSem(strbranch, strbatchyear, collegecode);
        if (branchcount != 0)
        {
            BindSectionDetail(strbatch, strbranch);
            BindSubject(strbatch, strbranch, strsem, strsec);
            BindTest(strbatch, strbranch);
        }
    }

    protected void LinkButtonbranch_Click(object sender, EventArgs e)
    {
        chklstbranch.ClearSelection();
        branchcnt = 0;
        txtbranch.Text = "---Select---";
    }

    public void branchimg_Click(object sender, ImageClickEventArgs e)
    {
        branchcnt = branchcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstbranch.Items[r].Selected = false;
        txtdegree.Text = "Branch(" + branchcnt.ToString() + ")";
        if (txtdegree.Text == "Branch(0)")
        {
            txtdegree.Text = "---Select---";
        }
    }

    public Label branchlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton branchimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //----------Section Dropdown Extender-----------------
    protected void chksection_CheckedChanged(object sender, EventArgs e)
    {
        if (chksection.Checked == true)
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = true;
                txtsection.Text = "Section(" + (chklstsection.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsection.Items.Count; i++)
            {
                chklstsection.Items[i].Selected = false;
                txtsection.Text = "---Select---";
            }
        }
    }

    protected void chklstsection_SelectedIndexChanged(object sender, EventArgs e)
    {
        psection.Focus();
        int sectioncount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstsection.Items.Count; i++)
        {
            if (chklstsection.Items[i].Selected == true)
            {
                value = chklstsection.Items[i].Text;
                code = chklstsection.Items[i].Value.ToString();
                sectioncount = sectioncount + 1;
                txtsection.Text = "Section(" + sectioncount.ToString() + ")";
            }
        }
        if (sectioncount == 0)
            txtsection.Text = "---Select---";
        else
        {
            Label lbl = sectionlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = sectionimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(sectionimg_Click);
        }
        sectioncnt = sectioncount;
        BindSubject(strbatch, strbranch, strsem, strsec);
        BindTest(strbatch, strbranch);
    }

    protected void LinkButtonsection_Click(object sender, EventArgs e)
    {
        chklstsection.ClearSelection();
        sectioncnt = 0;
        txtsection.Text = "---Select---";
    }

    public void sectionimg_Click(object sender, ImageClickEventArgs e)
    {
        sectioncnt = sectioncnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsection.Items[r].Selected = false;
        txtsection.Text = "Section(" + sectioncnt.ToString() + ")";
        if (txtsection.Text == "Section(0)")
        {
            txtsection.Text = "---Select---";
        }
    }

    public Label sectionlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton sectionimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    //----------Subject Dropdown Extender-----------------
    protected void chksubject_CheckedChanged(object sender, EventArgs e)
    {
        if (chksubject.Checked == true)
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = true;
                txtsubject.Text = "Subject(" + (chklstsubject.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklstsubject.Items.Count; i++)
            {
                chklstsubject.Items[i].Selected = false;
                txtsubject.Text = "---Select---";
            }
        }
    }

    protected void chklstsubject_SelectedIndexChanged(object sender, EventArgs e)
    {
        psubject.Focus();
        int subjectcount = 0;
        string value = string.Empty;
        string code = string.Empty;
        for (int i = 0; i < chklstsubject.Items.Count; i++)
        {
            if (chklstsubject.Items[i].Selected == true)
            {
                value = chklstsubject.Items[i].Text;
                code = chklstsubject.Items[i].Value.ToString();
                subjectcount = subjectcount + 1;
                txtsubject.Text = "Subject(" + subjectcount.ToString() + ")";
            }
        }
        if (subjectcount == 0)
            txtsubject.Text = "---Select---";
        else
        {
            Label lbl = subjectlabel();
            lbl.Text = " " + value + " ";
            lbl.ID = "lbl1-" + code.ToString();
            ImageButton ib = subjectimage();
            ib.ID = "imgbut1_" + code.ToString();
            ib.Click += new ImageClickEventHandler(subjectimg_Click);
        }
        subjectcnt = subjectcount;
        BindTest(strbatch, strbranch);
    }

    protected void LinkButtonsubject_Click(object sender, EventArgs e)
    {
        chklstsubject.ClearSelection();
        subjectcnt = 0;
        txtsubject.Text = "---Select---";
    }

    public void subjectimg_Click(object sender, ImageClickEventArgs e)
    {
        subjectcnt = subjectcnt - 1;
        ImageButton b = sender as ImageButton;
        int r = Convert.ToInt32(b.CommandArgument);
        chklstsubject.Items[r].Selected = false;
        txtsubject.Text = "Subject(" + sectioncnt.ToString() + ")";
        if (txtsubject.Text == "Subject(0)")
        {
            txtsubject.Text = "---Select---";
        }
    }

    public Label subjectlabel()
    {
        Label lbc = new Label();
        ViewState["lseatcontrol"] = true;
        return (lbc);
    }

    public ImageButton subjectimage()
    {
        ImageButton imc = new ImageButton();
        imc.ImageUrl = "xb.jpeg";
        imc.Height = 9;
        imc.Width = 9;
        ViewState["iseatcontrol"] = true;
        return (imc);
    }

    public object GetCorrespondingKey(object key, Hashtable hashTable)
    {
        IDictionaryEnumerator e = hashTable.GetEnumerator();
        while (e.MoveNext())
        {
            if (e.Key.ToString() == key.ToString())
            {
                return e.Value.ToString();
            }
        }
        return null;
    }

    //------Method for the Go Button -----
    protected void btnmasterprint_Click(object sender, EventArgs e)
    {
        //Session["column_header_row_count"] = FpSpread1.Sheets[0].ColumnHeader.RowCount;
        DateTime date_today = DateTime.Now;
        int yr_now = Convert.ToInt32(date_today.ToString("yyyy"));
        string academyear = (yr_now.ToString() + "-" + (yr_now + 1).ToString());
        //string degreedetails = "TEST & EXAMINATION RESULT-CONSOLIDATED REPORT" + '@' + "Degree :" + ddlBatch.SelectedItem.ToString() + '-' + ddlDegree.SelectedItem.ToString() + '[' + ddlBranch.SelectedItem.ToString() + ']' + '-' + "Sem-" + ddlSemYr.SelectedItem.ToString() + '@' + "Academic Year:" + academyear + '@' + "Month & Year of exam :" + ddlMonth.SelectedItem.ToString() + '&' + ddlYear.SelectedItem.ToString() + '@' + "Test/Exam Name:" + ddlTest.SelectedItem.ToString();
        string ss = null;
        string degreedetails = "Branchwise Subject Analysis" + '@' + "Test:" + ddltest.SelectedItem.ToString();
        string pagename = "Internal_Report.aspx";
        Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
        Printcontrol.Visible = true;
    }

    //protected void btngo_Click1(object sender, EventArgs e)
    //{
    //    try
    //    {

    //        DataTable data = new DataTable();
    //        DataRow drow;
    //        Dictionary<int, string> dicsyllsec = new Dictionary<int, string>();
    //        string syllcode = "";
    //        string sect = "";
    //        data.Columns.Add("SNo", typeof(string));
    //        data.Columns.Add("Subject", typeof(string));

    //        int sno = 0;
    //        int colcunt = 1;
    //        string sectval = string.Empty;
    //        for (int sc = 0; sc < chklstsection.Items.Count; sc++)
    //        {
    //            if (chklstsection.Items[sc].Selected == true)
    //            {
    //                if (chklstsection.Items[sc].Text.Trim().ToLower() == "empty section")
    //                {
    //                    if (sectval == "")
    //                    {
    //                        sectval = "''";
    //                    }
    //                    else
    //                    {
    //                        sectval = sectval + ",''";
    //                    }
    //                }
    //                else
    //                {
    //                    if (sectval == "")
    //                    {
    //                        sectval = "'" + chklstsection.Items[sc].Text + "'";
    //                    }
    //                    else
    //                    {
    //                        sectval = sectval + ",'" + chklstsection.Items[sc].Text + "'";
    //                    }
    //                }
    //            }
    //        }
    //        if (sectval.Trim() != "")
    //        {
    //            sectval = " and r.Sections in (" + sectval + ")";
    //        }
    //        for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
    //        {
    //            if (chklstbatch.Items[batch].Selected == true)
    //            {
    //                for (int branch = 0; branch < chklstbranch.Items.Count; branch++)
    //                {
    //                    if (chklstbranch.Items[branch].Selected == true)
    //                    {
    //                        ds.Dispose();
    //                        ds.Reset();
    //                        string strsql = "select distinct acronym,case when sections='-1' then '' else sections end as sections,batch_year,d.degree_code,current_semester from registration r,degree d where r.degree_code=d.degree_code and r.batch_year=" + chklstbatch.Items[batch].ToString() + " and d.degree_code=" + chklstbranch.Items[branch].Value + " " + sectval + " and cc=0 and delflag=0 and exam_flag<>'debar'";
    //                        ds = d2.select_method(strsql, hat, "Text");
    //                        if (ds.Tables[0].Rows.Count > 0)
    //                        {
    //                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
    //                            {
    //                                colcunt++;
    //                                data.Columns.Add("" + ds.Tables[0].Rows[i]["batch_year"] + " " + ds.Tables[0].Rows[i]["acronym"] + " " + ds.Tables[0].Rows[i]["sections"] + "", typeof(string));

    //                                ds1.Dispose();
    //                                ds1.Reset();
    //                                string strsql1 = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  and r.degree_code = " + ds.Tables[0].Rows[i]["degree_code"] + " and r.batch_year =" + ds.Tables[0].Rows[i]["batch_year"] + " and semester=" + ds.Tables[0].Rows[i]["current_semester"] + "";
    //                                ds1 = d2.select_method(strsql1, hat, "Text");
    //                                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
    //                                {
    //                                    syllcode = Convert.ToString(ds1.Tables[0].Rows[0]["syll_code"]);
    //                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds1.Tables[0].Rows[0]["syll_code"]);
    //                                }
    //                                sect = Convert.ToString(ds.Tables[0].Rows[i]["sections"]);
    //                                //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds.Tables[0].Rows[i]["sections"];
    //                                dicsyllsec.Add(colcunt, syllcode + "-" + sect);
    //                                Showgrid.Visible = true;
    //                                btnmasterprint.Visible = true;
    //                                //Added By Srianth 28/2/2013
    //                                btnxl.Visible = true;
    //                                txtexcelname.Visible = true;
    //                                lblrptname.Visible = true;
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        data.Columns.Add("Subject Pass %", typeof(string));

    //        for (int subject = 0; subject < chklstsubject.Items.Count; subject++)
    //        {
    //            if (chklstsubject.Items[subject].Selected == true)
    //            {
    //                sno++;
    //                drow = data.NewRow();
    //                drow["SNo"] = sno.ToString();
    //                drow["Subject"] = chklstsubject.Items[subject].Text;
    //                data.Rows.Add(drow);
    //            }
    //        }
    //        Boolean rowflag = false;
    //        if (data.Columns.Count > 2)
    //        {
    //            drow = data.NewRow();

    //            drow["Subject"] = "All Pass";
    //            data.Rows.Add(drow);
    //            //FpSpread1.Sheets[0].RowCount++;
    //            //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "All Pass";
    //            for (int rowcnt = 0; rowcnt < data.Rows.Count; rowcnt++)
    //            {
    //                for (int colcnt = 1; colcnt < data.Columns.Count; colcnt++)
    //                {
    //                    string value = dicsyllsec[colcnt];
    //                    string[] split = value.Split('-');
    //                    syllcode = Convert.ToString(split[0]);
    //                    subname = Convert.ToString(data.Rows[rowcnt][1]);
    //                    section = Convert.ToString(split[1]);
    //                    if (colcnt != data.Columns.Count - 1)
    //                    {
    //                        if (syllcode.ToString() != "")
    //                        {
    //                            string strexmcode = "select distinct exam_code,staff_code,c.min_mark from subject s,criteriaforinternal c,exam_type e where c.criteria_no=e.criteria_no and s.syll_code=c.syll_code and s.subject_no=e.subject_no and criteria= '" + ddltest.SelectedValue.ToString() + "' and subject_name='" + subname + "' and s.syll_code=" + syllcode + " and sections='" + section + "'";
    //                            ds.Dispose();
    //                            ds.Reset();
    //                            ds = d2.select_method(strexmcode, hat, "Text");
    //                            if (ds.Tables[0].Rows.Count > 0)
    //                            {
    //                                rowflag = true;
    //                                ds1.Dispose();
    //                                ds1.Reset();
    //                                string nofapp = "select count(marks_obtained) as 'No.of Appeared' from result r,registration rt where r.exam_code=" + ds.Tables[0].Rows[0]["exam_code"].ToString() + " and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 ";
    //                                ds1 = d2.select_method(nofapp, hat, "Text");
    //                                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
    //                                {
    //                                    totapp = Convert.ToInt32(ds1.Tables[0].Rows[0]["No.of Appeared"]);
    //                                }
    //                                ds1.Dispose();
    //                                ds1.Reset();
    //                                string passcount = "select count(marks_obtained) as 'Pass Count' from result where  exam_code=" + ds.Tables[0].Rows[0]["exam_code"].ToString() + "  and (marks_obtained>= " + ds.Tables[0].Rows[0]["min_mark"].ToString() + " or marks_obtained='-3' or marks_obtained='-2')";
    //                                ds1 = d2.select_method(passcount, hat, "Text");
    //                                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
    //                                {
    //                                    passcnt = Convert.ToInt32(ds1.Tables[0].Rows[0]["Pass Count"]);
    //                                }
    //                                ds1.Dispose();
    //                                ds1.Reset();
    //                                string staffcode = "select staff_name from staffmaster where staff_code= '" + ds.Tables[0].Rows[0]["staff_code"].ToString() + "'";
    //                                ds1 = d2.select_method(staffcode, hat, "Text");
    //                                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
    //                                {
    //                                    staff_name = Convert.ToString(ds1.Tables[0].Rows[0]["staff_name"]);
    //                                }
    //                                passpercen = Convert.ToDouble((passcnt / totapp) * 100);
    //                                passpercen_round = Math.Round(passpercen, 2);
    //                                FpSpread1.Sheets[0].Columns[colcnt].Width = 250;
    //                                FpSpread1.Sheets[0].Cells[rowcnt, colcnt].Text = passcnt + "(" + passpercen_round + ")" + "--" + staff_name + "";
    //                                FpSpread1.Sheets[0].Cells[rowcnt, colcnt].Font.Name = "Book Antiqua";
    //                                appcnt += totapp;
    //                                totpasscnt += passcnt;
    //                            }
    //                            if (colcnt > 0)
    //                            {
    //                                colheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcnt].Text;
    //                                if (hashappcnt.Contains(Convert.ToString(colheader)))
    //                                {
    //                                    rowapp = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashappcnt));
    //                                    rowapp += totapp;
    //                                    hashappcnt[Convert.ToString(colheader)] = rowapp;
    //                                }
    //                                else
    //                                {
    //                                    hashappcnt.Add(Convert.ToString(colheader), totapp);
    //                                }
    //                                if (hashpasscnt.Contains(Convert.ToString(colheader)))
    //                                {
    //                                    rowpasspercen = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashpasscnt));
    //                                    rowpasspercen += passcnt;
    //                                    hashpasscnt[Convert.ToString(colheader)] = rowpasspercen;
    //                                }
    //                                else
    //                                {
    //                                    hashpasscnt.Add(Convert.ToString(colheader), passcnt);
    //                                }
    //                                totapp = 0;
    //                                passcnt = 0;
    //                            }
    //                        }
    //                    }
    //                    rowtotapp = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashappcnt));
    //                    rowtotpasscnt = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashpasscnt));
    //                    if (rowtotpasscnt != 0 && rowtotapp != 0)
    //                    {
    //                        for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
    //                        {
    //                            if (i == FpSpread1.Sheets[0].RowCount - 1)
    //                            {
    //                                rowpercen = Convert.ToDouble((rowtotpasscnt / rowtotapp) * 100);
    //                                rowpercen_round = Math.Round(rowpercen, 2);
    //                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(rowpercen_round);
    //                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = rowtotpasscnt + "(" + rowpercen_round + ")";
    //                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = string.Empty;
    //                    }
    //                }
    //                if (appcnt != 0 && totpasscnt != 0)
    //                {
    //                    totpasspercen = Convert.ToDouble((totpasscnt / appcnt) * 100);
    //                    totpasspercen_round = Math.Round(totpasspercen, 2);
    //                    FpSpread1.Sheets[0].Cells[rowcnt, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totpasspercen_round);
    //                    FpSpread1.Sheets[0].Cells[rowcnt, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
    //                    appcnt = 0;
    //                    totpasscnt = 0;
    //                }
    //                else
    //                {
    //                    FpSpread1.Sheets[0].Cells[rowcnt, FpSpread1.Sheets[0].ColumnCount - 1].Text = string.Empty;
    //                }
    //            }
    //        }
    //        if (rowflag == false)
    //        {
    //            FpSpread1.Visible = false;
    //            btnmasterprint.Visible = false;
    //            btnxl.Visible = false;
    //            txtexcelname.Visible = false;
    //            lblrptname.Visible = false;
    //            errmsg.Visible = true;
    //            errmsg.Text = "No Records Found";
    //        }
    //        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
    //        FpSpread1.SaveChanges();
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Text = ex.ToString();
    //    }
    //}

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            ArrayList arrColHdrNames1 = new ArrayList();

            DataRow drow;
            Dictionary<int, string> dicsyllsec = new Dictionary<int, string>();
            string syllcode = "";
            string sect = "";

            arrColHdrNames1.Add("S.No");
            arrColHdrNames1.Add("Subject");
            data.Columns.Add("SNo", typeof(string));
            data.Columns.Add("Subject", typeof(string));

            int sno = 0;
            int colcunt = 1;
            string sectval = string.Empty;
            for (int sc = 0; sc < chklstsection.Items.Count; sc++)
            {
                if (chklstsection.Items[sc].Selected == true)
                {
                    if (chklstsection.Items[sc].Text.Trim().ToLower() == "empty section")
                    {
                        if (sectval == "")
                        {
                            sectval = "''";
                        }
                        else
                        {
                            sectval = sectval + ",''";
                        }
                    }
                    else
                    {
                        if (sectval == "")
                        {
                            sectval = "'" + chklstsection.Items[sc].Text + "'";
                        }
                        else
                        {
                            sectval = sectval + ",'" + chklstsection.Items[sc].Text + "'";
                        }
                    }
                }
            }
            if (sectval.Trim() != "")
            {
                sectval = " and r.Sections in (" + sectval + ")";
            }
            for (int batch = 0; batch < chklstbatch.Items.Count; batch++)
            {
                if (chklstbatch.Items[batch].Selected == true)
                {
                    for (int branch = 0; branch < chklstbranch.Items.Count; branch++)
                    {
                        if (chklstbranch.Items[branch].Selected == true)
                        {
                            ds.Dispose();
                            ds.Reset();
                            string strsql = "select distinct acronym,case when sections='-1' then '' else sections end as sections,batch_year,d.degree_code,current_semester from registration r,degree d where r.degree_code=d.degree_code and r.batch_year=" + chklstbatch.Items[batch].ToString() + " and d.degree_code=" + chklstbranch.Items[branch].Value + " " + sectval + " and cc=0 and delflag=0 and exam_flag<>'debar'";
                            ds = d2.select_method(strsql, hat, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    colcunt++;
                                    arrColHdrNames1.Add("" + ds.Tables[0].Rows[i]["batch_year"] + " " + ds.Tables[0].Rows[i]["acronym"] + " " + ds.Tables[0].Rows[i]["sections"] + "");
                                    data.Columns.Add("" + ds.Tables[0].Rows[i]["batch_year"] + " " + ds.Tables[0].Rows[i]["acronym"] + " " + ds.Tables[0].Rows[i]["sections"] + "", typeof(string));

                                    ds1.Dispose();
                                    ds1.Reset();
                                    string strsql1 = "select distinct syll_code from syllabus_master s,registration r where r.degree_code=s.degree_code and r.batch_year=s.batch_year and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar'  and r.degree_code = '" + ds.Tables[0].Rows[i]["degree_code"] + "' and r.batch_year ='" + ds.Tables[0].Rows[i]["batch_year"] + "' and semester='" + ds.Tables[0].Rows[i]["current_semester"] + "'";
                                    ds1 = d2.select_method(strsql1, hat, "Text");
                                    if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                    {
                                        syllcode = Convert.ToString(ds1.Tables[0].Rows[0]["syll_code"]);
                                        //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Note = Convert.ToString(ds1.Tables[0].Rows[0]["syll_code"]);
                                    }
                                    sect = Convert.ToString(ds.Tables[0].Rows[i]["sections"]);
                                    //FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Tag = ds.Tables[0].Rows[i]["sections"];
                                    dicsyllsec.Add(colcunt, syllcode + "-" + sect);


                                    Showgrid.Visible = true;
                                    btnmasterprint.Visible = true;
                                    btnPrint.Visible = true;
                                    //Added By Srianth 28/2/2013
                                    btnxl.Visible = true;
                                    txtexcelname.Visible = true;
                                    lblrptname.Visible = true;
                                }
                            }
                        }
                    }
                }
            }
            arrColHdrNames1.Add("Subject Pass %");
            data.Columns.Add("Subject Pass %", typeof(string));


            DataRow drHdr0 = data.NewRow();
            for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                drHdr0[grCol] = arrColHdrNames1[grCol];

            data.Rows.Add(drHdr0);

            Boolean rowflag = false;
            Dictionary<int, int> dicrowspansubpass = new Dictionary<int, int>();
            Dictionary<int, int> dicnotrowspan = new Dictionary<int, int>();
            int row = 0;
            int rowcount = 0;

            if (data.Columns.Count > 2)
            {
                //for (int rowcnt = 0; rowcnt < FpSpread1.Sheets[0].RowCount; rowcnt++)
                //{

                for (int subject = 0; subject < chklstsubject.Items.Count; subject++)
                {
                    if (chklstsubject.Items[subject].Selected == true)
                    {
                        sno++;
                        row = row + rowcount;
                        drow = data.NewRow();
                        drow["SNo"] = sno.ToString();
                        drow["Subject"] = chklstsubject.Items[subject].Text;
                        data.Rows.Add(drow);
                        int rowcnt = data.Rows.Count - 1;
                        int currentStartRow = rowcnt;
                        int rowSpanCount = 1;
                        int maxRowCount = 1;
                        bool isNotHave = false;
                        rowcount = 0;
                        for (int colcnt = 2; colcnt < data.Columns.Count - 1; colcnt++)
                        {
                            string value = dicsyllsec[colcnt];
                            string[] split = value.Split('-');
                            syllcode = Convert.ToString(split[0]);
                            subname = Convert.ToString(data.Rows[rowcnt][1]);
                            section = Convert.ToString(split[1]);


                            string batchYear = d2.GetFunctionv("select batch_year from syllabus_master where syll_code='" + syllcode + "'");
                            bool isStudentStaffSelector = CheckStudentStaffSelector(batchYear);
                            string qryStaffSelectorBatchYear = string.Empty;
                            string staffCode = string.Empty;
                            rowcnt = currentStartRow;
                            if (colcnt != data.Columns.Count - 1)
                            {
                                if (syllcode.ToString() != "")
                                {
                                    string strexmcode = "select distinct exam_code,ss.staff_code,c.min_mark from subject s,criteriaforinternal c,exam_type e,staff_selector ss where c.criteria_no=e.criteria_no and s.syll_code=c.syll_code and s.subject_no=e.subject_no and ss.subject_no=s.subject_no and criteria= '" + ddltest.SelectedValue.ToString() + "' and subject_name='" + subname + "' and s.syll_code=" + syllcode + " and e.sections='" + section + "' and ss.Sections='" + section + "'";
                                    ds.Dispose();
                                    ds.Reset();
                                    ds = d2.select_method(strexmcode, hat, "Text");
                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        rowcount = ds.Tables[0].Rows.Count;
                                        isNotHave = true;
                                        int columnCurrent = colcnt;
                                        for (int staffRow = 0; staffRow < ((isStudentStaffSelector) ? ds.Tables[0].Rows.Count : 1); staffRow++)
                                        {
                                            if (staffRow != 0)
                                            {
                                                if (columnCurrent == colcnt && data.Rows.Count - 1 == rowcnt)
                                                {
                                                    // FpSpread1.Sheets[0].RowCount++;
                                                    drow = data.NewRow();
                                                    data.Rows.Add(drow);
                                                    rowcnt = data.Rows.Count - 1;
                                                }
                                                else
                                                {
                                                    if (data.Rows.Count - 1 >= rowcnt)
                                                        rowcnt++;
                                                }
                                                sno++;
                                                //rowcount++;

                                                data.Rows[rowcnt][0] = sno.ToString();
                                                //drow["Subject"] = chklstsubject.Items[subject].Text;
                                                data.Rows[rowcnt][1] = chklstsubject.Items[subject].Text;

                                            }
                                            if (rowSpanCount < ((isStudentStaffSelector) ? ds.Tables[0].Rows.Count : 1))
                                            {
                                                rowSpanCount = ((isStudentStaffSelector) ? ds.Tables[0].Rows.Count : 1);
                                            }
                                            rowflag = true;
                                            ds1.Dispose();
                                            ds1.Reset();
                                            //string nofapp = "select count(marks_obtained) as 'No.of Appeared' from result r,registration rt where r.exam_code=" + ds.Tables[0].Rows[staffRow]["exam_code"].ToString() + " and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 ";
                                            staffCode = Convert.ToString(ds.Tables[0].Rows[staffRow]["staff_code"]).Trim();
                                            if (isStudentStaffSelector)
                                            {
                                                qryStaffSelectorBatchYear = " and sc.StaffCode like '%" + staffCode + "%'";
                                            }
                                            string nofapp = " select count(r.roll_no) as 'No.of Appeared' from result r,registration rt,subjectChooser sc,Exam_type e where e.exam_code=r.exam_code  and (marks_obtained>=0 or marks_obtained='-2' or marks_obtained='-3') and r.roll_no=rt.roll_no and r.roll_no=sc.roll_no and sc.roll_no=rt.Roll_No and sc.subject_no=e.subject_no  and rt.cc=0 and rt.exam_flag <>'DEBAR' and rt.delflag=0 and rt.RollNo_Flag<>0 and r.exam_code='" + Convert.ToString(ds.Tables[0].Rows[staffRow]["exam_code"]).Trim() + "' " + qryStaffSelectorBatchYear;
                                            ds1 = d2.select_method(nofapp, hat, "Text");
                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                            {
                                                totapp = Convert.ToInt32(ds1.Tables[0].Rows[0]["No.of Appeared"]);
                                            }
                                            ds1.Dispose();
                                            ds1.Reset();
                                            //string passcount = "select count(marks_obtained) as 'Pass Count' from result where  exam_code=" + ds.Tables[staffRow].Rows[0]["exam_code"].ToString() + "  and (marks_obtained>= " + ds.Tables[0].Rows[staffRow]["min_mark"].ToString() + " or marks_obtained='-3' or marks_obtained='-2')";
                                            string passcount = "select count(marks_obtained) as 'Pass Count' from result r ,subjectChooser sc,Exam_type e  where  e.exam_code=r.exam_code and (marks_obtained>= '" + Convert.ToString(ds.Tables[0].Rows[staffRow]["min_mark"]).Trim() + "' or marks_obtained='-3' or marks_obtained='-2') and sc.subject_no=e.subject_no and r.roll_no=sc.roll_no  and r.exam_code='" + Convert.ToString(ds.Tables[0].Rows[staffRow]["exam_code"]).Trim() + "'  " + qryStaffSelectorBatchYear;
                                            ds1 = d2.select_method(passcount, hat, "Text");
                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                            {
                                                passcnt = Convert.ToInt32(ds1.Tables[0].Rows[0]["Pass Count"]);
                                            }
                                            ds1.Dispose();
                                            ds1.Reset();
                                            string staffcode = "select staff_name from staffmaster where staff_code= '" + ds.Tables[0].Rows[staffRow]["staff_code"].ToString() + "'";
                                            ds1 = d2.select_method(staffcode, hat, "Text");
                                            if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                                            {
                                                staff_name = Convert.ToString(ds1.Tables[0].Rows[0]["staff_name"]);
                                            }
                                            if (totapp > 0)
                                                passpercen = Convert.ToDouble((passcnt / totapp) * 100);
                                            passpercen_round = Math.Round(passpercen, 2);

                                            data.Rows[rowcnt][colcnt] = passcnt + "(" + passpercen_round + ")" + "--" + staff_name + "";


                                            appcnt += totapp;
                                            totpasscnt += passcnt;
                                            if (colcnt > 0)
                                            {
                                                colheader = data.Columns[colcnt].ColumnName;

                                                //colheader = FpSpread1.Sheets[0].ColumnHeader.Cells[0, colcnt].Text;
                                                if (hashappcnt.Contains(Convert.ToString(colheader)))
                                                {
                                                    rowapp = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashappcnt));
                                                    rowapp += totapp;
                                                    hashappcnt[Convert.ToString(colheader)] = rowapp;
                                                }
                                                else
                                                {
                                                    hashappcnt.Add(Convert.ToString(colheader), totapp);
                                                }
                                                if (hashpasscnt.Contains(Convert.ToString(colheader)))
                                                {
                                                    rowpasspercen = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashpasscnt));
                                                    rowpasspercen += passcnt;
                                                    hashpasscnt[Convert.ToString(colheader)] = rowpasspercen;
                                                }
                                                else
                                                {
                                                    hashpasscnt.Add(Convert.ToString(colheader), passcnt);
                                                }
                                                totapp = 0;
                                                passcnt = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            //rowtotapp = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashappcnt));
                            //rowtotpasscnt = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashpasscnt));
                            //if (rowtotpasscnt != 0 && rowtotapp != 0)
                            //{
                            //    for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                            //    {
                            //        if (i == FpSpread1.Sheets[0].RowCount - 1)
                            //        {
                            //            rowpercen = Convert.ToDouble((rowtotpasscnt / rowtotapp) * 100);
                            //            rowpercen_round = Math.Round(rowpercen, 2);
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = Convert.ToString(rowpercen_round);
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = rowtotpasscnt + "(" + rowpercen_round + ")";
                            //            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                            //        }
                            //    }
                            //}
                            //else
                            //{
                            //    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, colcnt].Text = string.Empty;
                            //}
                        }
                        dicrowspansubpass.Add(row + 1, rowcount);

                        if (!isNotHave)
                        {
                            data.Rows.Remove(drow);
                        }
                        else
                        {
                            if (appcnt != 0 && totpasscnt != 0)
                            {
                                totpasspercen = Convert.ToDouble((totpasscnt / appcnt) * 100);
                                totpasspercen_round = Math.Round(totpasspercen, 2);
                                data.Rows[currentStartRow][data.Columns.Count - 1] = Convert.ToString(totpasspercen_round);
                                dicnotrowspan.Add(currentStartRow + 1, data.Columns.Count - 1);
                                //FpSpread1.Sheets[0].Cells[currentStartRow, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(totpasspercen_round);
                                //FpSpread1.Sheets[0].Cells[currentStartRow, FpSpread1.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                //FpSpread1.Sheets[0].AddSpanCell(currentStartRow, FpSpread1.Sheets[0].ColumnCount - 1, rowSpanCount, 1);
                                appcnt = 0;
                                totpasscnt = 0;
                            }
                            else
                            {
                                data.Rows[currentStartRow][data.Columns.Count - 1] = "";
                                //  FpSpread1.Sheets[0].Cells[currentStartRow, FpSpread1.Sheets[0].ColumnCount - 1].Text = string.Empty;
                            }
                        }

                    }
                }
                drow = data.NewRow();
                drow["subject"] = "Total Pass";
                data.Rows.Add(drow);

                for (int colcnt = 2; colcnt < data.Columns.Count; colcnt++)
                {
                    colheader = data.Columns[colcnt].ColumnName;
                    rowtotapp = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashappcnt));
                    rowtotpasscnt = Convert.ToDouble(GetCorrespondingKey(Convert.ToString(colheader), hashpasscnt));
                    if (rowtotpasscnt != 0 && rowtotapp != 0)
                    {

                        rowpercen = Convert.ToDouble((rowtotpasscnt / rowtotapp) * 100);
                        rowpercen_round = Math.Round(rowpercen, 2);
                        data.Rows[data.Rows.Count - 1][colcnt] = rowtotpasscnt + "(" + rowpercen_round + ")";


                    }
                    else
                    {
                        data.Rows[data.Rows.Count - 1][colcnt] = "";
                    }
                }


            }
            if (rowflag == false)
            {
                Showgrid.Visible = false;
                btnmasterprint.Visible = false;
                btnPrint.Visible = false;
                btnxl.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            if (data.Columns.Count > 0 && data.Rows.Count > 0)
            {
                Showgrid.DataSource = data;
                Showgrid.DataBind();
                Showgrid.Visible = true;
                btnmasterprint.Visible = true;
                btnPrint.Visible = true;
                btnxl.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;


                Showgrid.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Showgrid.Rows[0].Font.Bold = true;
                Showgrid.Rows[0].HorizontalAlign = HorizontalAlign.Center;

                int col = data.Columns.Count;
                int rowval = data.Rows.Count;
               // Showgrid.Rows[rowval - 1].Cells[1].Font.Bold = true;
             //   Showgrid.Rows[rowval - 1].Cells[0].ColumnSpan = 2;
                //Showgrid.Rows[rowval - 1].Cells[1].HorizontalAlign = HorizontalAlign.Center;
                //Showgrid.Rows[rowval - 1].Cells[2].Font.Bold = true;
               // Showgrid.Rows[rowval - 1].Cells[col].Font.Bold = true;
                Showgrid.HeaderRow.Visible = false;
              

            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
               // e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Center;
                //for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                //    e.Row.Cells[grCol].Visible = false;

            }


        }
        catch
        {
        }
    }

    //------Method for the Excel Coversion -----
    protected void btnxl_Click(object sender, EventArgs e)
    {
        // Session["column_header_row_count"] = Convert.ToString(FpSpread1.ColumnHeader.RowCount);
        string reportname = txtexcelname.Text.ToString().Trim();
        if (reportname != "")
        {
            d2.printexcelreportgrid(Showgrid, reportname);
        }
        else
        {
            lblxlerr.Visible = true;
            lblxlerr.Text = "Please Enter Report Name";
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    private bool CheckStudentStaffSelector(string batchYear)
    {
        bool isStudentStaffSelector = false;
        try
        {
            string minimumabsentsms = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Studnet Staff Selector' and college_code='" + Session["collegecode"].ToString().Trim() + "'");
            string[] splitminimumabsentsms = minimumabsentsms.Split('-');
            if (splitminimumabsentsms.Length == 2)
            {
                int batchyearsetting = 0;
                int.TryParse(Convert.ToString(splitminimumabsentsms[1]).Trim(), out batchyearsetting);
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    if (Convert.ToInt32(batchYear.ToString()) >= batchyearsetting)
                    {
                        isStudentStaffSelector = true;
                    }
                }
            }
            else if (splitminimumabsentsms.Length > 0)
            {
                if (splitminimumabsentsms[0].ToString() == "1")
                {
                    isStudentStaffSelector = true;
                }
            }
            //if (isStudentStaffSelector)
            //{
            //    qryStudeStaffSelector = " and sc.staffcode like '%" + staffcode + "%'";
            //}
        }
        catch
        {
        }
        return isStudentStaffSelector;
    }

    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
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
        spReportName.InnerHtml = "Branchwise Subject Analysis";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }

}