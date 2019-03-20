using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;

public partial class Default5 : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string sqlbatch=string.Empty;
    string sqlbatchquery=string.Empty;
    string strdegree = string.Empty;
    string sqldegree = string.Empty;
    string sqlbranch = string.Empty;
    string sqlbranchquery = string.Empty;
    string sqlsec = string.Empty;
    string sqlsecquery = string.Empty;
    string studtype = string.Empty;
    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["con"]);
    string PageLogOut = "";
    
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
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("TransportIndex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/TransportMod/TransportIndex.aspx");
                return;
            }
        }
        usercode = Session["usercode"].ToString();
        collegecode = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();

        if (!IsPostBack)
        {
            setLabelText();
            Fpstudenttransport.Width = 1000;
            Fpstudenttransport.Sheets[0].AutoPostBack = true;
            Fpstudenttransport.CommandBar.Visible = true;
            Fpstudenttransport.Sheets[0].SheetName = " ";
            Fpstudenttransport.Sheets[0].SheetCorner.Columns[0].Visible = false;
            Fpstudenttransport.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fpstudenttransport.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            Fpstudenttransport.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpstudenttransport.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpstudenttransport.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstudenttransport.Sheets[0].DefaultStyle.Font.Bold = false;

            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = System.Drawing.Color.Black;
            Fpstudenttransport.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpstudenttransport.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpstudenttransport.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpstudenttransport.Sheets[0].AllowTableCorner = true;

           
            Fpstudenttransport.Visible = false;
            //btnxl.Visible = false;
            //btnprintmaster.Visible = false;
            LabelE.Visible = false;
            lblnorec.Visible = false;
            errmsg.Visible = false;



            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSectransport(strbatch, strbranch);
            BindSectionDetail(strbatch, strbranch);
           // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }

    //  Batch load-------

    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
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
            }


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    // Degree load function
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
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                    if (chklstdegree.Items[i].Selected == true)
                    {
                        count1 += 1;
                    }
                    if (chklstdegree.Items.Count == count1)
                    {
                        chkdegree.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    }

    // Branch load function-------

    public void BindBranch(string singleuser, string group_user, string course_id, string collegecode, string usercode)
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
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count2 += 1;
                    }
                    if (chklstbranch.Items.Count == count2)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }

    }

    // section laod function

    public void BindSectransport(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
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

            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                //chklstsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
                    chklssec.Items[0].Selected = true;
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        if (chklssec.Items[i].Selected == true)
                        {
                            count3 += 1;
                        }
                        if (chklssec.Items.Count == count3)
                        {
                            chksec.Checked = true;
                        }
                    }
                }
            }
            else
            {
                chklssec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }
    }

    // check box load function

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
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = true;
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        count2 += 1;
                    }
                    if (chklstbranch.Items.Count == count2)
                    {
                        chkbranch.Checked = true;
                    }
                }
            }
            BindSectionDetail(strbatch, strbranch);
        }
           
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }

    //  batch checkbox load

    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        try
        {
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
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    // bind batch check box load function

    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
        
        string clg = "";
        int commcount = 0;
        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
                if (clg == "")
                {
                    clg = chklsbatch.Items[i].Value.ToString();
                }
                else
                {
                    clg = clg + "','" + chklsbatch.Items[i].Value;
                }
            }
        }
       
        if (commcount == 0)
        {
            txtbatch.Text = "--Select--";
        }
        con.Close();
        //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
    }
       catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }

    } 
    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstdegree.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstdegree.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkbranch_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            string clg = "";
            int commcount = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                    if (clg == "")
                    {
                        clg = chklstbranch.Items[i].Value.ToString();
                    }
                    else
                    {
                        clg = clg + "','" + chklstbranch.Items[i].Value;
                    }
                }
            }

            if (commcount == 0)
            {
                txtbatch.Text = "--Select--";
            }

            BindSectionDetail(strbatch, strbranch);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindSectionDetail(string strbatch, string strbranch)
    {
        try
        {
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        strbatch = strbatch + "," + "'" + chklsbatch.Items[i].Value.ToString() + "'";
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
         
            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetail(strbatch, strbranch);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                //chklstsection.Items.Insert(0, "All");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
                    //chklstsection.Items[0].Selected = true;
                    for (int i = 0; i < chklssec.Items.Count; i++)
                    {
                        chklssec.Items[i].Selected = true;
                        if (chklssec.Items[i].Selected == true)
                        {
                            count3 += 1;
                        }
                        if (chklssec.Items.Count == count3)
                        {
                            chksec.Checked = true;
                        }
                    }
                }
            }
            else
            {
                chklssec.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = " Please Select the Branch";
        }

    }
    protected void chksec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            if (chksec.Checked == true)
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = true;
                    txtsec.Text = "Section(" + (chklssec.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklssec.Items.Count; i++)
                {
                    chklssec.Items[i].Selected = false;
                    txtsec.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklstsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklssec.Items.Count; i++)
            {
                if (chklssec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtsec.Text = "Section(" + commcount.ToString() + ")";

                }
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
           

            if (txtbatch.Text != "--Select--" || chklsbatch.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklsbatch.Items.Count; itemcount++)
                {
                    if (chklsbatch.Items[itemcount].Selected == true)
                    {
                        if (sqlbatch == "")
                            sqlbatch = "'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                        else
                            sqlbatch = sqlbatch + "," + "'" + chklsbatch.Items[itemcount].Value.ToString() + "'";
                    }
                }


                if (sqlbatch != "")
                {
                    sqlbatch = " in(" + sqlbatch + ")";
                    sqlbatchquery = " and r.batch_year  " + sqlbatch + "";
                }
                else
                {
                    sqlbatchquery = " ";

                }
            }

            if (txtdegree.Text != "---Select---" || chklstdegree.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklstdegree.Items.Count; itemcount++)
                {
                    if (chklstdegree.Items[itemcount].Selected == true)
                    {
                        if (strdegree == "")
                            strdegree = "'" + chklstdegree.Items[itemcount].Value.ToString() + "'";
                        else
                            strdegree = strdegree + "," + "'" + chklstdegree.Items[itemcount].Value.ToString() + "'";
                    }
                }

                if (strdegree != "")
                {
                    sqldegree = " in(" + strdegree + ")";
                    sqldegree = " and r.degree_code  " + strdegree + "";
                }
                else
                {
                    sqldegree = " ";

                }
            }

            if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
            {
                int itemcount = 0;


                for (itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
                {
                    if (chklstbranch.Items[itemcount].Selected == true)
                    {
                        if (sqlbranch == "")
                            sqlbranch = "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                        else
                            sqlbranch = sqlbranch + "," + "'" + chklstbranch.Items[itemcount].Value.ToString() + "'";
                    }
                }

                if (sqlbranch != "")
                {
                    sqlbranch = " in(" + sqlbranch + ")";
                    sqlbranchquery = " and r.degree_code  " + sqlbranch + "";
                }
                else
                {
                    sqlbranchquery = " ";

                }
            }

            if (txtsec.Text != "---Select---" || chklssec.Items.Count != null)
            {
                int itemcount = 0;

                int cnt = 0;
                for (itemcount = 0; itemcount < chklssec.Items.Count; itemcount++)
                {
                    if (chklssec.Items[itemcount].Selected == true)
                    {
                        cnt++;
                        if (sqlsec == "")
                            sqlsec = "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                        else
                            sqlsec = sqlsec + "," + "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                    }
                }

                if (cnt == chklssec.Items.Count)
                {
                    if (sqlsec == "")
                    {
                        sqlsec = "''";
                    }
                    else
                    {
                        sqlsec = sqlsec + "," + "''";
                    }
                }

                if (sqlsec != "")
                {
                    sqlsec = " in(" + sqlsec + ")";
                    sqlsecquery = " and isnull(r.sections,'')  " + sqlsec + "";
                }
                else
                {
                    sqlsecquery = " ";

                }
            }
            studtype = ddlstutype.Text;//added by rajasekar 10/09/2018


            if (sqldegree != " " || sqlbranchquery != " ")
            {
                

                Fpstudenttransport.Visible = true;
                Fpstudenttransport.Sheets[0].RowCount = 0;
                Fpstudenttransport.Sheets[0].ColumnCount = 0;
                Fpstudenttransport.Sheets[0].ColumnHeader.Visible = true;
                Fpstudenttransport.Sheets[0].ColumnCount++;

                Fpstudenttransport.Sheets[0].AutoPostBack = true;
                Fpstudenttransport.Sheets[0].ColumnHeader.RowCount = 1;
                Fpstudenttransport.Sheets[0].ColumnCount = 11;
                Fpstudenttransport.Sheets[0].RowCount = 0;
                Fpstudenttransport.Sheets[0].PageSize = 5000;

                ds2.Dispose();
                ds2.Reset();
                ds2 = d2.Bindstudenttransport(sqlbatchquery, sqlbranchquery, sqlsecquery, studtype);//modified by rajasekar 10/09/2018
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    errmsg.Visible = false;
                    lblnorec.Visible = false;
                    Fpstudenttransport.Visible = true;
                    //btnxl.Visible = true;
                    //btnprintmaster.Visible = true;

                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Gender";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Type";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Route";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Transport No";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Starting Point";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Hostel Name";
                    Fpstudenttransport.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Room No";


                    Fpstudenttransport.Sheets[0].Columns[0].Width = 50;
                    Fpstudenttransport.Sheets[0].Columns[1].Width = 150;
                    Fpstudenttransport.Sheets[0].Columns[2].Width = 150;
                    Fpstudenttransport.Sheets[0].Columns[3].Width = 250;
                    Fpstudenttransport.Sheets[0].Columns[4].Width = 80;
                    Fpstudenttransport.Sheets[0].Columns[5].Width = 120;
                    Fpstudenttransport.Sheets[0].Columns[6].Width = 50;
                    Fpstudenttransport.Sheets[0].Columns[7].Width = 50;
                    Fpstudenttransport.Sheets[0].Columns[8].Width = 80;
                    Fpstudenttransport.Sheets[0].Columns[9].Width = 200;
                    Fpstudenttransport.Sheets[0].Columns[10].Width = 80;

                    int sno = 0;
                    for (int rolcount = 0; rolcount < ds2.Tables[0].Rows.Count; rolcount++)
                    {
                        sno++;
                        Fpstudenttransport.Sheets[0].RowCount = Fpstudenttransport.Sheets[0].RowCount + 1;
                        Fpstudenttransport.Sheets[0].Rows[Fpstudenttransport.Sheets[0].RowCount - 1].Font.Bold = false;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 1].Text = ds2.Tables[0].Rows[rolcount]["Roll_No"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 2].CellType = new FarPoint.Web.Spread.TextCellType();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 2].Text = ds2.Tables[0].Rows[rolcount]["Reg_No"].ToString(); ;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 3].Text = ds2.Tables[0].Rows[rolcount]["Stud_Name"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 4].Text = ds2.Tables[0].Rows[rolcount]["Gender"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 5].Text = ds2.Tables[0].Rows[rolcount]["Stud_Type"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 6].Text = ds2.Tables[0].Rows[rolcount]["Bus_RouteID"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 7].Text = ds2.Tables[0].Rows[rolcount]["VehID"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 8].Text = ds2.Tables[0].Rows[rolcount]["Boarding"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 9].Text = ds2.Tables[0].Rows[rolcount]["Hostel_Name"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 10].Text = ds2.Tables[0].Rows[rolcount]["Room_Name"].ToString();
                        Fpstudenttransport.Sheets[0].Cells[Fpstudenttransport.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                    }
                }
                else
                {
                    lblnorec.Visible = true;
                    Fpstudenttransport.Visible = false;
                    //btnxl.Visible = false;
                    //btnprintmaster.Visible = false;
                }
            }
            else
            {
                Fpstudenttransport.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "Please Select Branch and Degree";
                
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
       
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = "";
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "Student Transport Report" + i;
                //Fpstudenttransport.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
                //Aruna on 26feb2013============================
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")

                Fpstudenttransport.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
                //=============================================

            }
            catch
            {
                i++;
                goto e;

            }
           // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        string section = string.Empty;

        Session["column_header_row_count"] = Fpstudenttransport.Sheets[0].ColumnHeader.RowCount;

        degreedetails = "Individual Student Fee Status Report";
        string pagename = "Individual_student_Fee_Status.aspx";
        Printcontrol.loadspreaddetails(Fpstudenttransport, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    private void setLabelText()
    {
        string grouporusercode = string.Empty;
        if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Convert.ToString(Session["group_code"]).Trim() + "";
        }
        else if (Session["usercode"] != null)
        {
            grouporusercode = " usercode=" + Convert.ToString(Session["usercode"]).Trim() + "";
        }
        List<Label> lbl = new List<Label>();
        List<byte> fields = new List<byte>();
       // lbl.Add(Label3);
        //lbl.Add(lbl_stream);
        lbl.Add(lbldegree);
        lbl.Add(lblbranch);
        //lbl.Add(lbl_sem);
       // fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 22-10-2016 sudhagar
}