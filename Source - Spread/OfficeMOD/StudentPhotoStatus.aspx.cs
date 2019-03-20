using System;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
public partial class StudentPhotoStatus : System.Web.UI.Page
{
    #region "Load Details"
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string sqlbatch = string.Empty;
    string sqlbatchquery = string.Empty;
    string strdegree = string.Empty;
    string sqldegree = string.Empty;
    string sqlbranch = string.Empty;
    string sqlbranchquery = string.Empty;
    string sqlsec = string.Empty;
    string sqlsecquery = string.Empty;
    string sqlphototquery = string.Empty;
    int count = 0;
    int count1 = 0;
    int count2 = 0;
    int count3 = 0;
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
    SqlCommand cmd = new SqlCommand();
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //public static List<string> GetListofCountries(string prefixText)
    //{
    //    string cs = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    using (SqlConnection sqlconn = new SqlConnection(cs))
    //    {
    //        sqlconn.Open();
    //        SqlCommand cmd = new SqlCommand("select Stage_id,Stage_Name,Address,District from stage_master where Stage_Name like '" + prefixText + "%' ", sqlconn);
    //        cmd.Parameters.AddWithValue("@Stage_Name", prefixText);
    //        SqlDataAdapter da = new SqlDataAdapter(cmd);
    //        DataTable dt = new DataTable();
    //        da.Fill(dt);
    //        List<string> CountryNames = new List<string>();
    //        for (int i = 0; i < dt.Rows.Count; i++)
    //        {
    //            //CountryNames.Add(dt.Rows[i]["stud_name"].ToString() + "|" + dt.Rows[i]["roll_no"].ToString() + "|" + dt.Rows[i]["reg_no"].ToString() + "\n\n");
    //            CountryNames.Add(dt.Rows[i]["Stage_Name"].ToString());
    //        }
    //        return CountryNames;
    //    }
    //}
    //[System.Web.Script.Services.ScriptMethod()]
    //[System.Web.Services.WebMethod]
    //public static List<string> GetCity(string prefixText)
    //{
    //    DataTable dt = new DataTable();
    //    // string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
    //    // SqlConnection con = new SqlConnection(constr);
    //    //  con.Open();
    //    // SqlCommand cmd = new SqlCommand("select * from City where CityName like @City+'%'", con);
    //    // cmd.Parameters.AddWithValue("@City", prefixText);
    //    // SqlDataAdapter adp = new SqlDataAdapter(cmd);
    //    // adp.Fill(ddtt);]
    //    string my = "select  textval from textvaltable where TextCriteria='state'";
    //    DataSet da = new DataSet();
    //    DAccess2 d3 = new DAccess2();
    //    da = d3.select_method_wo_parameter(my, "Text");
    //    List<string> CityNames = new List<string>();
    //    for (int i = 0; i < da.Tables[0].Rows.Count; i++)
    //    {
    //        CityNames.Add(da.Tables[0].Rows[i][0].ToString());
    //    }
    //    return CityNames;
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
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
        lblphotoerr.Visible = false;
        if (!IsPostBack)
        {
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Studflag"] = "0";
            string Master1 = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                Master1 = "select * from Master_Settings where group_code=" + Session["group_code"] + "";
            }
            else
            {
                Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            }
            DataSet dsmaseter = d2.select_method_wo_parameter(Master1, "Text");
            if (dsmaseter.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsmaseter.Tables[0].Rows.Count; i++)
                {
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Register No" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsmaseter.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && dsmaseter.Tables[0].Rows[i]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }
            Fpstudentphoto.Visible = false;
            Fpstudentphoto.CommandBar.Visible = false;
            Fpstudentphoto.Sheets[0].SheetName = " ";
            Fpstudentphoto.Sheets[0].SheetCorner.Columns[0].Visible = false;
            Fpstudentphoto.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
            Fpstudentphoto.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
            Fpstudentphoto.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
            Fpstudentphoto.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            Fpstudentphoto.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            Fpstudentphoto.Sheets[0].DefaultStyle.Font.Bold = false;
            FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
            style1.Font.Size = 12;
            style1.Font.Bold = true;
            style1.HorizontalAlign = HorizontalAlign.Center;
            style1.ForeColor = System.Drawing.Color.Black;
            Fpstudentphoto.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpstudentphoto.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
            Fpstudentphoto.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            Fpstudentphoto.Sheets[0].AllowTableCorner = true;
            Fpstudentphoto.Sheets[0].AutoPostBack = false;
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranch(singleuser, group_user, course_id, collegecode, usercode);
            BindSectionDetailmult(collegecode);
            // BindSectionDetail(strbatch, strbranch);
            Fpstudentphoto.Visible = false;
            errmsg.Visible = false;
            btnxl.Visible = false;
            lblnorec.Visible = false;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            mult();
        }
    }
    //Bind Batch
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
    //Bind Degree
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
    //Bind Branch
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
    //Bind Branch Multiple
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
            BindSectionDetailmult(collegecode);
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }
    // Bind section Multiple
    public void BindSectionDetailmult(string collegecode)
    {
        try
        {
            int takecount = 0;
            //strbranch = chklstbranch.SelectedValue.ToString();
            chklssec.Items.Clear();
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindSectionDetailmult(collegecode);
            if (ds2.Tables[0].Rows.Count > 0)
            {
                takecount = ds2.Tables[0].Rows.Count;
                chklssec.DataSource = ds2;
                chklssec.DataTextField = "sections";
                chklssec.DataBind();
                chklssec.Items.Insert(takecount, "Empty");
                if (Convert.ToString(ds2.Tables[0].Columns["sections"]) == string.Empty)
                {
                    chklssec.Enabled = false;
                }
                else
                {
                    chklssec.Enabled = true;
                    chklssec.SelectedIndex = chklssec.Items.Count - 2;
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
            errmsg.Text = "Please Choose Section";
            //  errmsg.Visible = true;
        }
    }
    public void mult()
    {
        string my = "select textval,textcode from TextValTable where TextCriteria='caste' and textval<>''";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(my, "Text");
        ddltotal.DataSource = ds;
        ddltotal.DataTextField = "textval";
        ddltotal.DataValueField = "textcode";
        ddltotal.DataBind();
        ddltotal.Items.Insert(0, "--Select--");
    }
    protected void ddlselectindechanged(object sender, EventArgs e)
    {
        try
        {
            string my = "";
            if (ddlfilter.SelectedIndex == 0)
            {
                my = "select textval,textcode from TextValTable where TextCriteria='caste' and textval<>''";
            }
            else if (ddlfilter.SelectedIndex == 1)
            {
                my = "select textval,textcode from TextValTable where TextCriteria='bgrou' and textval<>''";
            }
            else if (ddlfilter.SelectedIndex == 2)
            {
                my = "select textval,textcode from TextValTable where TextCriteria='seat' and textval<>''";
            }
            else if (ddlfilter.SelectedIndex == 3)
            {
                my = "select textval,textcode from TextValTable where TextCriteria='comm' and textval<>''";
            }
            else if (ddlfilter.SelectedIndex == 4)
            {
                my = " select  textval,textcode  from TextValTable where TextCriteria='dis' and textval<>''";
            }
            else if (ddlfilter.SelectedIndex == 5)
            {
                my = "select  textcode,  textval from textvaltable where TextCriteria='state'";
            }
            else if (ddlfilter.SelectedIndex == 6)
            {
                my = "select   textval,textcode  from textvaltable where TextCriteria='coun' and textval<>''";
            }
            DataSet ds = new DataSet();
            ds = d2.select_method_wo_parameter(my, "Text");
            ddltotal.DataSource = ds;
            ddltotal.DataTextField = "textval";
            ddltotal.DataValueField = "textcode";
            ddltotal.DataBind();
            ddltotal.Items.Insert(0, "--Select--");
        }
        catch
        {
        }
    }
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
                chkbatch.Checked = false;
            }
            else if (commcount == chklsbatch.Items.Count)
            {
                chkbatch.Checked = true;
            }
            else
            {
                chkbatch.Checked = false;
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
                txtbranch.Text = "--Select--";
                chkbranch.Checked = false;
            }
            else if (commcount == chklstbranch.Items.Count)
            {
                chkbranch.Checked = true;
            }
            else
            {
                chkbranch.Checked = false;
            }
            BindSectionDetailmult(collegecode);
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
                txtdegree.Text = "--Select--";
            }
            if (commcount == chklstdegree.Items.Count)
            {
                chkdegree.Checked = true;
            }
            else
            {
                chkdegree.Checked = false;
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
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
            if (commcount == 0)
            {
                txtsec.Text = "--Select--";
            }
            if (commcount == chklssec.Items.Count)
            {
                chksec.Checked = true;
            }
            else
            {
                chksec.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkphoto_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkphoto.Checked == true)
            {
                for (int i = 0; i < chklsphoto.Items.Count; i++)
                {
                    chklsphoto.Items[i].Selected = true;
                    txtphoto.Text = "Photo(" + (chklsphoto.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklsphoto.Items.Count; i++)
                {
                    chklsphoto.Items[i].Selected = false;
                    txtphoto.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklsphoto_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklsphoto.Items.Count; i++)
            {
                if (chklsphoto.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtphoto.Text = "Photo(" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtphoto.Text = "--Select--";
            }
            if (commcount == chklsphoto.Items.Count)
            {
                chkphoto.Checked = true;
            }
            else
            {
                chkphoto.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chklscategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklscategory.Items.Count; i++)
            {
                if (chklscategory.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtcategory.Text = "Category (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtcategory.Text = "--Select--";
            }
            if (commcount == chklscategory.Items.Count)
            {
                chkcategory.Checked = true;
            }
            else
            {
                chkcategory.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkcategory.Checked == true)
            {
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = true;
                    txtcategory.Text = "Category(" + (chklscategory.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < chklscategory.Items.Count; i++)
                {
                    chklscategory.Items[i].Selected = false;
                    txtcategory.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }
    #endregion
    //Go Function
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            int itemcount = 0;
            if (txtbatch.Text != "--Select--" || chklsbatch.Items.Count > 0)
            {
                itemcount = 0;
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
            if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count > 0)
            {
                itemcount = 0;
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
            if (chklssec.Items.Count > 0)
            {
                if (txtsec.Text != "---Select---" || chklssec.Items.Count != null)
                {
                    itemcount = 0;
                    for (itemcount = 0; itemcount < chklssec.Items.Count - 1; itemcount++)
                    {
                        if (chklssec.Items[itemcount].Selected == true)
                        {
                            if (sqlsec == "")
                                sqlsec = "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                            else
                                sqlsec = sqlsec + "," + "'" + chklssec.Items[itemcount].Value.ToString() + "'";
                        }
                    }
                    if (sqlsec != "")
                    {
                        sqlsec = "r.sections in(" + sqlsec + ")";
                    }
                    if (sqlsec != "" && chklssec.Items[itemcount].Selected == true)
                    {
                        sqlsecquery = " or r.sections is null or r.sections='' or r.sections='-1'";
                        sqlsecquery = "and ( " + sqlsec + "  " + sqlsecquery + ")";
                    }
                    else if (sqlsec == "" && chklssec.Items[itemcount].Selected == true)
                    {
                        sqlsecquery = "  r.sections is null or r.sections='' or r.sections='-1'";
                        sqlsecquery = "and ( " + sqlsec + "  " + sqlsecquery + ")";
                    }
                    else if (sqlsec == "" && chklssec.Items[itemcount].Selected == false)
                    {
                        sqlsecquery = "";
                    }
                }
            }
            //Modified By Srinath 23/09/2014==========================================Start=====================================================
            Boolean retflag = false;
            Boolean cateflag = false;
            for (itemcount = 0; itemcount < chklscategory.Items.Count; itemcount++)
            {
                if (chklscategory.Items[itemcount].Selected == true)
                {
                    cateflag = true;
                }
            }
            //string photocondition = "";
            //string conaval = "";
            //if (txtphoto.Text == "Photo(1)")
            //{
            //    if (txtcategory.Text != "Both" && txtcategory.Text != "---Select---")
            //    {
            //        itemcount = 0;
            //        for (itemcount = 0; itemcount < chklscategory.Items.Count; itemcount++)
            //        {
            //            if (chklscategory.Items[itemcount].Selected == true)
            //            {
            //                cateflag = true;
            //                if (chklsphoto.SelectedIndex == 0)
            //                {
            //                    string gettext = chklscategory.Items[itemcount].Text;
            //                    string va = "";
            //                    string val = "";
            //                    if (gettext == "Student")
            //                    {
            //                        va = "s.Photo";
            //                        val = "sphoto";
            //                    }
            //                    else if (gettext == "Father")
            //                    {
            //                        va = "s.f_photo";
            //                        val = "fphoto";
            //                    }
            //                    else if (gettext == "Mother")
            //                    {
            //                        va = "s.m_photo";
            //                        val = "mphoto";
            //                    }
            //                    else if (gettext == "Guardian")
            //                    {
            //                        va = "s.g_photo";
            //                        val = "gphoto";
            //                    }
            //                    if (conaval == "")
            //                    {
            //                        conaval = " and(" + va + " is not null";
            //                        photocondition = " and (" + val + " is not null";
            //                    }
            //                    else
            //                    {
            //                        conaval = conaval + " and " + va + " is not null";
            //                        photocondition = photocondition + " and (" + val + " is not null";
            //                    }
            //                }
            //                else if (chklsphoto.SelectedIndex == 1)
            //                {
            //                    string gettext = chklscategory.Items[itemcount].Text;
            //                    string va = "";
            //                    string val = "";
            //                    if (gettext == "Student")
            //                    {
            //                        va = "s.Photo";
            //                    }
            //                    else if (gettext == "Father")
            //                    {
            //                        va = "s.f_photo";
            //                    }
            //                    else if (gettext == "Mother")
            //                    {
            //                        va = "s.m_photo";
            //                    }
            //                    else if (gettext == "Guardian")
            //                    {
            //                        va = "s.g_photo";
            //                    }
            //                    if (conaval == "")
            //                    {
            //                        conaval = " and (" + va + " is null";
            //                        photocondition = " and (" + val + " is null";
            //                    }
            //                    else
            //                    {
            //                        conaval = conaval + " and (" + va + " is null";
            //                        photocondition = photocondition + " and (" + val + " is null";
            //                    }
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (chklsphoto.SelectedIndex == 0)
            //        {
            //            conaval = "and (s.Photo is not null and s.f_photo is not null and s.m_photo is not null and s.g_photo is not null";
            //            photocondition = " and (sPhoto is not null and fphoto is not null and mphoto is not null and gphoto is not null";
            //        }
            //        else if (chklsphoto.SelectedIndex == 1)
            //        {
            //            conaval = "and (s.Photo is null and s.f_photo is null and s.m_photo is null and s.g_photo is null";
            //            photocondition = " and (sPhoto is null and fphoto is null and mphoto is null and gphoto is null";
            //        }
            //    }
            //}
            //==========================================End=====================================================
            string filter = "";
            if (ddltotal.SelectedItem.Text != "--Select--")
            {
                if (ddlfilter.SelectedIndex == 0)
                {
                    filter = " and a.caste=" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 1)
                {
                    filter = "and a.bldgrp=" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 2)
                {
                    filter = " and a.seattype=" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 3)
                {
                    filter = " and a.community=" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 4)
                {
                    filter = " and a.Districtp =" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 5)
                {
                    filter = " and a.stateg=" + ddltotal.SelectedItem.Value + "";
                }
                else if (ddlfilter.SelectedIndex == 6)
                {
                    filter = " and a. Countryp='" + ddltotal.SelectedItem.Text + "'";
                }
            }
            //Modified By Srinath 23/09/2014
            //if (conaval != "")
            //{
            //    conaval = conaval + " )";
            //    photocondition = photocondition + " )";
            //}
            string strorder = "ORDER BY r.Roll_No";
            string serialno = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
            if (serialno.Trim() == "1")
            {
                strorder = "ORDER BY r.serialno";
            }
            else
            {
                string orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                if (orderby_Setting == "0")
                {
                    strorder = "ORDER BY r.Roll_No";
                }
                else if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY r.Reg_No";
                }
                else if (orderby_Setting == "2")
                {
                    strorder = "ORDER BY r.Stud_Name";
                }
                else if (orderby_Setting == "0,1,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,1")
                {
                    strorder = "ORDER BY r.Roll_No,r.Reg_No";
                }
                else if (orderby_Setting == "1,2")
                {
                    strorder = "ORDER BY r.Reg_No,r.Stud_Name";
                }
                else if (orderby_Setting == "0,2")
                {
                    strorder = "ORDER BY r.Roll_No,r.Stud_Name";
                }
            }
            //Modified By Srinath 23/09/2014
            // sqlphototquery = "select ROW_NUMBER() OVER (" + strorder + ") As SrNo,r.roll_no,r.reg_no,r.stud_name,CASE WHEN A.Sex = 0 THEN 'Male' ELSE 'Female' END Gender,case when s.photo is null then 'Not Available' else 'Available' end student_photo,case when s.f_photo is null then 'Not Available' else 'Available' end as father_photo,case when m_photo is null then 'Not Available' else 'Available' end  as mother_photo,case when g_photo is null then 'Not Available' else 'Available' end as gar_photo,a.app_no,r.serialno from registration r ,StdPhoto s,applyn a where s.app_no=r.app_no and a.app_no=r.App_No " + filter + " and CC=0 and DelFlag=0 and Exam_Flag<>'debar' " + sqlbatchquery + " " + sqlbranchquery + " " + sqlsecquery + " " + conaval + " " + strorder + "";
            string getquery = "select ROW_NUMBER() OVER (" + strorder + ") As SrNo,r.roll_no,r.reg_no,r.stud_name,CASE WHEN A.Sex = 0 THEN 'Male' ELSE 'Female' END Gender,a.app_no,r.serialno from registration r ,applyn a where a.app_no=r.App_No and CC=0 and DelFlag=0 and Exam_Flag<>'debar' " + sqlbatchquery + " " + sqlbranchquery + " " + sqlsecquery + "  " + filter + " " + strorder + "";
            getquery = getquery + " ; select r.roll_no,case when s.Photo is not null then 'Available' else 'Not Available' end as sphoto,case when s.f_photo is not null then 'Available' else 'Not Available' end as fphoto,case when s.m_photo is not null then 'Available' else 'Not Available' end as mphoto,case when s.g_photo is not null then 'Available' else 'Not Available' end as gphoto from registration r,stdphoto s where r.app_no=s.app_no and r.cc=0 and r.delflag=0 and r.exam_flag<>'debar' " + sqlbatchquery + " " + sqlbranchquery + " " + sqlsecquery + " ";
            DataSet dsphoto = d2.select_method_wo_parameter(getquery, "Text");
            errmsg.Visible = false;
            lblnorec.Visible = false;
            Fpstudentphoto.Visible = true;
            btnxl.Visible = true;
            Printcontrol.Visible = false;
            btnprintmaster.Visible = true;
            lblrptname.Visible = true;
            txtexcelname.Visible = true;
            Fpstudentphoto.Visible = true;
            Fpstudentphoto.Sheets[0].ColumnCount = 0;
            Fpstudentphoto.Sheets[0].ColumnHeader.Visible = true;
            Fpstudentphoto.Sheets[0].ColumnHeader.RowCount = 1;
            Fpstudentphoto.Sheets[0].RowCount = 0;
            Fpstudentphoto.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;
            Fpstudentphoto.Sheets[0].ColumnHeader.Rows[0].Height = 40;
            //ds2.Dispose();
            //ds2.Reset();
            //ds2 = d2.BindstudentPhoto(sqlphototquery);
            if (dsphoto.Tables[0].Rows.Count > 0)
            {
                Fpstudentphoto.Sheets[0].AutoPostBack = false;
                Fpstudentphoto.Sheets[0].ColumnCount = 10;
                FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                btn.Text = "View/New";
                btn.CommandName = "ButtonClickHandler";
                Fpstudentphoto.Sheets[0].Columns[9].CellType = btn;
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Gender";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Student Photo";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Father Photo";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Mother Photo";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Guardian Photo";
                Fpstudentphoto.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Add Photo";
                int columnwidth = 1150;
                Fpstudentphoto.Sheets[0].Columns[0].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[1].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[2].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[3].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[4].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[5].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[6].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[7].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[8].CellType = txt;
                Fpstudentphoto.Sheets[0].Columns[0].Width = 50;
                Fpstudentphoto.Sheets[0].Columns[1].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[2].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[3].Width = 200;
                Fpstudentphoto.Sheets[0].Columns[4].Width = 50;
                Fpstudentphoto.Sheets[0].Columns[5].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[6].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[7].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[8].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[9].Width = 100;
                Fpstudentphoto.Sheets[0].Columns[0].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[1].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[2].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[3].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[4].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[5].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[6].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[7].Locked = true;
                Fpstudentphoto.Sheets[0].Columns[8].Locked = true;
                Fpstudentphoto.Sheets[0].ColumnHeader.Columns[1].Visible = false;
                Fpstudentphoto.Sheets[0].ColumnHeader.Columns[2].Visible = false;
                Fpstudentphoto.Sheets[0].ColumnHeader.Columns[4].Visible = false;
                if (Session["Rollflag"].ToString() != "0")
                {
                    Fpstudentphoto.Sheets[0].ColumnHeader.Columns[1].Visible = true;
                    columnwidth = columnwidth - 100;
                }
                if (Session["Regflag"].ToString() != "0")
                {
                    Fpstudentphoto.Sheets[0].ColumnHeader.Columns[2].Visible = true;
                    columnwidth = columnwidth - 100;
                }
                Fpstudentphoto.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                int srno = 0;
                for (int i = 0; i < dsphoto.Tables[0].Rows.Count; i++)
                {
                    dsphoto.Tables[1].DefaultView.RowFilter = " roll_no='" + dsphoto.Tables[0].Rows[i]["roll_no"].ToString() + "' ";
                    DataView dvphoto = dsphoto.Tables[1].DefaultView;
                    string sphoto = "Not Available";
                    string fphoto = "Not Available";
                    string mphoto = "Not Available";
                    string gphoto = "Not Available";
                    if (dvphoto.Count > 0)
                    {
                        sphoto = dvphoto[0]["sphoto"].ToString();
                        fphoto = dvphoto[0]["fphoto"].ToString();
                        mphoto = dvphoto[0]["mphoto"].ToString();
                        gphoto = dvphoto[0]["gphoto"].ToString();
                    }
                    Boolean recflag = true;
                    if (txtphoto.Text == "Photo(1)")
                    {
                        string status = "Available";
                        if (chklsphoto.SelectedIndex == 1)
                        {
                            status = "Not Available";
                        }
                        if (chklscategory.Items[0].Selected == true || cateflag == false)
                        {
                            if (sphoto != status)
                            {
                                recflag = false;
                            }
                        }
                        if (chklscategory.Items[1].Selected == true || cateflag == false)
                        {
                            if (fphoto != status)
                            {
                                recflag = false;
                            }
                        }
                        if (chklscategory.Items[2].Selected == true || cateflag == false)
                        {
                            if (mphoto != status)
                            {
                                recflag = false;
                            }
                        }
                        if (chklscategory.Items[3].Selected == true || cateflag == false)
                        {
                            if (gphoto != status)
                            {
                                recflag = false;
                            }
                        }
                    }
                    if (recflag == true)
                    {
                        retflag = true;
                        srno++;
                        Fpstudentphoto.Sheets[0].RowCount++;
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 1].Text = dsphoto.Tables[0].Rows[i]["roll_no"].ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 1].Tag = dsphoto.Tables[0].Rows[i]["app_no"].ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 2].Text = dsphoto.Tables[0].Rows[i]["reg_no"].ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 3].Text = dsphoto.Tables[0].Rows[i]["stud_name"].ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 4].Text = dsphoto.Tables[0].Rows[i]["Gender"].ToString();
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 5].Text = sphoto;
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 6].Text = fphoto;
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 7].Text = mphoto;
                        Fpstudentphoto.Sheets[0].Cells[Fpstudentphoto.Sheets[0].RowCount - 1, 8].Text = gphoto;
                    }
                }
                Fpstudentphoto.Sheets[0].PageSize = Fpstudentphoto.Sheets[0].RowCount;
                int height = 100;
                for (int i = 0; i < Fpstudentphoto.Sheets[0].RowCount; i++)
                {
                    height = height + Fpstudentphoto.Sheets[0].Rows[i].Height;
                }
                Fpstudentphoto.Height = height;
                Fpstudentphoto.Width = columnwidth;
                Fpstudentphoto.SaveChanges();
            }
            if (retflag == false)
            {
                lblnorec.Visible = true;
                lblnorec.Text = "No Records Found";
                Fpstudentphoto.Visible = false;
                btnxl.Visible = false;
                Printcontrol.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void ButtonClickHandler(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                if (appno != null && appno.Trim() != "")
                {
                    string Name = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 3].Text.ToString();
                    string Roll = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Text.ToString();
                    panelphoto.Visible = true;
                    lblcaption.Text = "" + Name + "(" + Roll + ") Photo's Details";
                    loadimage(appno);
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btnstuph_Click(object sender, EventArgs e)
    {
        try
        {
            if (fulstudp.HasFile)
            {
                if (fulstudp.FileName.EndsWith(".jpg") || fulstudp.FileName.EndsWith(".jpeg") || fulstudp.FileName.EndsWith(".JPG") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".png"))
                {
                    string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
                    if (ar != "-1" && ar.Trim() != "" && ar != null)
                    {
                        string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                        //cmd.CommandType = CommandType.Text;
                        //cmd.Connection = ssql;
                        //cmd.CommandText = "update stdphoto set photo=@SDocData where app_no=@appno";
                        int fileSize = fulstudp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulstudp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@SDocData", SqlDbType.Image, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.Int, 50);
                        //uploadedsubject_name.Value = appno;
                        //cmd.Parameters.Add(uploadedsubject_name);
                        //ssql.Close();
                        //ssql.Open();
                        //int result = cmd.ExecuteNonQuery();
                        PhotoUpload("photo", appno, fileSize, documentBinary);//barath 10.01.18 
                        loadimage(appno);
                    }
                }
                else
                {
                    lblphotoerr.Visible = true;
                    lblphotoerr.Text = "Selected file format is Not allowed";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btnfaph_Click(object sender, EventArgs e)
    {
        try
        {
            if (fulfatp.HasFile)
            {
                if (fulfatp.FileName.EndsWith(".jpg") || fulfatp.FileName.EndsWith(".jpeg") || fulfatp.FileName.EndsWith(".JPG") || fulfatp.FileName.EndsWith(".gif") || fulfatp.FileName.EndsWith(".png"))
                {
                    string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
                    if (ar != "-1" && ar.Trim() != "" && ar != null)
                    {
                        string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                        //cmd.CommandType = CommandType.Text;
                        //cmd.Connection = ssql;
                        //cmd.CommandText = "update stdphoto set f_photo=@FDocData where app_no=@appno";
                        int fileSize = fulfatp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulfatp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@FDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.VarChar, 50);
                        //uploadedsubject_name.Value = appno;
                        //cmd.Parameters.Add(uploadedsubject_name);
                        //ssql.Close();
                        //ssql.Open();
                        //int result = cmd.ExecuteNonQuery();
                        PhotoUpload("f_photo", appno, fileSize, documentBinary);
                        loadimage(appno);
                    }
                }
                else
                {
                    lblphotoerr.Visible = true;
                    lblphotoerr.Text = "Selected file format is Not allowed";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btnmotph_Click(object sender, EventArgs e)
    {
        try
        {
            if (fulmp.HasFile)
            {
                if (fulmp.FileName.EndsWith(".jpg") || fulmp.FileName.EndsWith(".jpeg") || fulmp.FileName.EndsWith(".JPG") || fulmp.FileName.EndsWith(".gif") || fulmp.FileName.EndsWith(".png"))
                {
                    string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
                    if (ar != "-1" && ar.Trim() != "" && ar != null)
                    {
                        string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                        //cmd.CommandType = CommandType.Text;
                        //cmd.Connection = ssql;
                        //cmd.CommandText = "update stdphoto set m_photo=@MDocData where app_no=@appno";
                        int fileSize = fulmp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulmp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@MDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.VarChar, 50);
                        //uploadedsubject_name.Value = appno;
                        //cmd.Parameters.Add(uploadedsubject_name);
                        //ssql.Close();
                        //ssql.Open();
                        //int result = cmd.ExecuteNonQuery();
                        PhotoUpload("m_photo", appno, fileSize, documentBinary);
                        loadimage(appno);
                    }
                }
                else
                {
                    lblphotoerr.Visible = true;
                    lblphotoerr.Text = "Selected file format is Not allowed";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btngurph_Click(object sender, EventArgs e)
    {
        try
        {
            if (fulguar.HasFile)
            {
                if (fulguar.FileName.EndsWith(".jpg") || fulguar.FileName.EndsWith(".jpeg") || fulguar.FileName.EndsWith(".JPG") || fulguar.FileName.EndsWith(".gif") || fulguar.FileName.EndsWith(".png"))
                {
                    string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
                    if (ar != "-1" && ar.Trim() != "" && ar != null)
                    {
                        string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                        //cmd.CommandType = CommandType.Text;
                        //cmd.Connection = ssql;
                        //cmd.CommandText = "update stdphoto set g_photo=@MDocData where app_no=@appno";
                        int fileSize = fulguar.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulguar.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@MDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.VarChar, 50);
                        //uploadedsubject_name.Value = appno;
                        //cmd.Parameters.Add(uploadedsubject_name);
                        //ssql.Close();
                        //ssql.Open();
                        //int result = cmd.ExecuteNonQuery();
                        PhotoUpload("g_photo", appno, fileSize, documentBinary);
                        loadimage(appno);
                    }
                }
                else
                {
                    lblphotoerr.Visible = true;
                    lblphotoerr.Text = "Selected file format is Not allowed";
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            sqlphototquery = "";
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                string appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                //cmd.CommandType = CommandType.Text;
                //cmd.Connection = ssql;
                //===================Add Student Photo========================
                if (fulstudp.HasFile)
                {
                    if (fulstudp.FileName.EndsWith(".jpg") || fulstudp.FileName.EndsWith(".jpeg") || fulstudp.FileName.EndsWith(".JPG") || fulstudp.FileName.EndsWith(".gif") || fulstudp.FileName.EndsWith(".png"))
                    {
                        int fileSize = fulstudp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulstudp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@SDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //if (sqlphototquery == "")
                        //{
                        //    sqlphototquery = "update stdphoto set Photo=@SDocData";
                        //}
                        //else
                        //{
                        //    sqlphototquery = sqlphototquery + ",Photo=@SDocData";
                        //}
                        PhotoUpload("photo", appno, fileSize, documentBinary);//barath 10.01.18 
                    }
                    else
                    {
                        lblphotoerr.Visible = true;
                        lblphotoerr.Text = "Selected file format is Not allowed";
                    }
                }
                //===================Add Father Photo========================
                if (fulfatp.HasFile)
                {
                    if (fulfatp.FileName.EndsWith(".jpg") || fulfatp.FileName.EndsWith(".jpeg") || fulfatp.FileName.EndsWith(".JPG") || fulfatp.FileName.EndsWith(".gif") || fulfatp.FileName.EndsWith(".png"))
                    {
                        int fileSize = fulfatp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulfatp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@FDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        //if (sqlphototquery == "")
                        //{
                        //    sqlphototquery = "update stdphoto set f_photo=@FDocData";
                        //}
                        //else
                        //{
                        //    sqlphototquery = sqlphototquery + ",f_photo=@FDocData";
                        //}
                        PhotoUpload("f_photo", appno, fileSize, documentBinary);//barath 10.01.18 
                    }
                    else
                    {
                        lblphotoerr.Visible = true;
                        lblphotoerr.Text = "Selected file format is Not allowed";
                    }
                }
                //===================Add Mother Photo========================
                if (fulmp.HasFile)
                {
                    if (fulmp.FileName.EndsWith(".jpg") || fulmp.FileName.EndsWith(".jpeg") || fulmp.FileName.EndsWith(".JPG") || fulmp.FileName.EndsWith(".gif") || fulmp.FileName.EndsWith(".png"))
                    {
                        if (sqlphototquery == "")
                        {
                            sqlphototquery = "update stdphoto set m_photo=@MDocData";
                        }
                        else
                        {
                            sqlphototquery = sqlphototquery + ",m_photo=@MDocData";
                        }
                        int fileSize = fulmp.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulmp.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@MDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        PhotoUpload("m_photo", appno, fileSize, documentBinary);//barath 10.01.18 
                    }
                    else
                    {
                        lblphotoerr.Visible = true;
                        lblphotoerr.Text = "Selected file format is Not allowed";
                    }
                }
                //===========Add Gurdian Photo=====================
                if (fulguar.HasFile)
                {
                    if (fulguar.FileName.EndsWith(".jpg") || fulguar.FileName.EndsWith(".jpeg") || fulguar.FileName.EndsWith(".JPG") || fulguar.FileName.EndsWith(".gif") || fulguar.FileName.EndsWith(".png"))
                    {
                        //if (sqlphototquery == "")
                        //{
                        //    sqlphototquery = "update stdphoto set g_photo=@GDocData";
                        //}
                        //else
                        //{
                        //    sqlphototquery = sqlphototquery + ",g_photo=@GDocData";
                        //}
                        int fileSize = fulguar.PostedFile.ContentLength;
                        byte[] documentBinary = new byte[fileSize];
                        fulguar.PostedFile.InputStream.Read(documentBinary, 0, fileSize);
                        //SqlParameter uploadedDocument = new SqlParameter("@GDocData", SqlDbType.Binary, fileSize);
                        //uploadedDocument.Value = documentBinary;
                        //cmd.Parameters.Add(uploadedDocument);
                        PhotoUpload("g_photo", appno, fileSize, documentBinary);//barath 10.01.18 
                    }
                    else
                    {
                        lblphotoerr.Visible = true;
                        lblphotoerr.Text = "Selected file format is Not allowed";
                    }
                }
                //if (sqlphototquery != "")
                //{
                //cmd.CommandText = sqlphototquery + " where app_no=@appno";
                //SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.VarChar, 50);
                //uploadedsubject_name.Value = appno;
                //cmd.Parameters.Add(uploadedsubject_name);
                //ssql.Close();
                //ssql.Open();
                //int result = cmd.ExecuteNonQuery();
                //loadimage(appno);
                //}
                loadimage(appno);
            }
            panelphoto.Visible = false;
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Save Successfully')", true);
            btngo_Click(sender, e);
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    public void loadimage(string appno)
    {
        try
        {
            string query = "select Photo,f_photo,m_photo,g_photo from StdPhoto where app_no='" + appno + "'";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(query, "Text");
            imgstudp.ImageUrl = null;
            imgfatp.ImageUrl = null;
            imgmotp.ImageUrl = null;
            imggurp.ImageUrl = null;
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Photo"] != null)
                {
                    imgstudp.Visible = true;
                    imgstudp.ImageUrl = "~/Handler/Handler3.ashx?id=" + appno;
                }
                else
                {
                    imgstudp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["f_photo"] != null)
                {
                    imgfatp.Visible = true;
                    imgfatp.ImageUrl = "~/Handler/Handler7.ashx?id=" + appno;
                }
                else
                {
                    imgfatp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["m_photo"] != null)
                {
                    imgmotp.Visible = true;
                    imgmotp.ImageUrl = "~/Handler/Handler8.ashx?id=" + appno;
                }
                else
                {
                    imgmotp.Visible = false;
                }
                if (ds.Tables[0].Rows[0]["g_photo"] != null)
                {
                    imggurp.Visible = true;
                    imggurp.ImageUrl = "~/Handler/Handler9.ashx?id=" + appno;
                }
                else
                {
                    imggurp.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btnexit_Click(object sender, EventArgs e)
    {
        panelphoto.Visible = false;
        btngo_Click(sender, e);
    }
    //Export Excel Report 
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpstudentphoto, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    //Print Function
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = "Pending Parents Photo Report";
        string pagename = "PendingParentshoto.aspx";
        Printcontrol.loadspreaddetails(Fpstudentphoto, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
    //**************PHOTO DOWNLOAD ADDED BY SUBBURAJ  08/09/2014*********************//
    protected void Btndownload_Click(object sender, EventArgs e)
    {
        System.Drawing.Image img = null;
        string appno = "";
        string sFilePath = "";
        byte[] bytearray = null;
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = ssql;
                cmd.CommandText = "select photo from stdphoto where app_no=" + appno + "";
                //byte[] buffer = null;
                ssql.Open();
                SqlDataReader MyReader = cmd.ExecuteReader();
                if (MyReader.Read())
                {
                    //buffer = (byte[])cmd.ExecuteScalar();
                    bytearray = (byte[])MyReader["photo"];
                    MyReader.Close();
                }
                ssql.Close();
                using (MemoryStream ms = new MemoryStream(bytearray))
                {
                    img = System.Drawing.Image.FromStream(ms);
                    string targetPath = Server.MapPath("~/Report/");//barath 10.01.18
                    if (!System.IO.Directory.Exists(targetPath))
                        System.IO.Directory.CreateDirectory(targetPath);
                    img.Save(Server.MapPath("~/Report/" + appno + ".jpg"));
                    sFilePath = Server.MapPath("~/Report/" + appno + ".jpg");
                }
                FileInfo fi = new FileInfo(sFilePath);
                Response.ContentType = fi.Extension;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                Response.TransmitFile(fi.FullName);
                Response.End();
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void Btndownload1_Click(object sender, EventArgs e)
    {
        System.Drawing.Image img = null;
        string appno = "";
        string sFilePath = "";
        byte[] bytearray = null;
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = ssql;
                cmd.CommandText = "select f_photo from stdphoto where app_no=" + appno + "";
                //byte[] buffer = null;
                ssql.Open();
                SqlDataReader MyReader = cmd.ExecuteReader();
                if (MyReader.Read())
                {
                    //buffer = (byte[])cmd.ExecuteScalar();
                    bytearray = (byte[])MyReader["f_photo"];
                    MyReader.Close();
                }
                ssql.Close();
                using (MemoryStream ms = new MemoryStream(bytearray))
                {
                    img = System.Drawing.Image.FromStream(ms);
                    string targetPath = Server.MapPath("~/Tempimage/");//barath 10.01.18
                    if (!System.IO.Directory.Exists(targetPath))
                        System.IO.Directory.CreateDirectory(targetPath);
                    img.Save(Server.MapPath("~/Tempimage/" + appno + ".jpg"));
                    sFilePath = Server.MapPath("~/Tempimage/" + appno + ".jpg");
                }
                FileInfo fi = new FileInfo(sFilePath);
                Response.ContentType = fi.Extension;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                Response.TransmitFile(fi.FullName);
                Response.End();
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void btndownload2_Click(object sender, EventArgs e)
    {
        System.Drawing.Image img = null;
        string appno = "";
        string sFilePath = "";
        byte[] bytearray = null;
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = ssql;
                cmd.CommandText = "select m_photo from stdphoto where app_no=" + appno + "";
                //byte[] buffer = null;
                ssql.Open();
                SqlDataReader MyReader = cmd.ExecuteReader();
                if (MyReader.Read())
                {
                    //buffer = (byte[])cmd.ExecuteScalar();
                    bytearray = (byte[])MyReader["m_photo"];
                    MyReader.Close();
                }
                ssql.Close();
                using (MemoryStream ms = new MemoryStream(bytearray))
                {
                    img = System.Drawing.Image.FromStream(ms);
                    string targetPath = Server.MapPath("~/Tempimage/");//barath 10.01.18
                    if (!System.IO.Directory.Exists(targetPath))
                        System.IO.Directory.CreateDirectory(targetPath);
                    img.Save(Server.MapPath("~/Tempimage/" + appno + ".jpg"));
                    sFilePath = Server.MapPath("~/Tempimage/" + appno + ".jpg");
                }
                FileInfo fi = new FileInfo(sFilePath);
                Response.ContentType = fi.Extension;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                Response.TransmitFile(fi.FullName);
                Response.End();
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    protected void Btndownload3_Click(object sender, EventArgs e)
    {
        System.Drawing.Image img = null;
        string appno = "";
        string sFilePath = "";
        byte[] bytearray = null;
        try
        {
            string ar = Fpstudentphoto.ActiveSheetView.ActiveRow.ToString();
            if (ar != "-1" && ar.Trim() != "" && ar != null)
            {
                appno = Fpstudentphoto.Sheets[0].Cells[int.Parse(ar), 1].Tag.ToString();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = ssql;
                cmd.CommandText = "select g_photo from stdphoto where app_no=" + appno + "";
                //byte[] buffer = null;
                ssql.Open();
                SqlDataReader MyReader = cmd.ExecuteReader();
                if (MyReader.Read())
                {
                    //buffer = (byte[])cmd.ExecuteScalar();
                    bytearray = (byte[])MyReader["g_photo"];
                    MyReader.Close();
                }
                ssql.Close();
                using (MemoryStream ms = new MemoryStream(bytearray))
                {
                    img = System.Drawing.Image.FromStream(ms);
                    img.Save(Server.MapPath("~/Tempimage/" + appno + ".jpg"));
                    sFilePath = Server.MapPath("~/Tempimage/" + appno + ".jpg");
                }
                FileInfo fi = new FileInfo(sFilePath);
                Response.ContentType = fi.Extension;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + fi.Name);
                Response.TransmitFile(fi.FullName);
                Response.End();
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
    }
    //***********************************************END*********************************************//
    protected int PhotoUpload(string ColumnName, string AppNo, int FileSize, byte[] DocDocument)//barath 10.01.2018
    {
        int Result = 0;
        try
        {
            string InsPhoto = "if exists (select " + ColumnName + " from StdPhoto where app_no=@appno) update StdPhoto set " + ColumnName + "=@photoid where app_no=@appno else insert into StdPhoto (app_no," + ColumnName + ") values(@appno,@photoid)";
            SqlCommand cmd = new SqlCommand(InsPhoto, ssql);
            SqlParameter uploadedsubject_name = new SqlParameter("@appno", SqlDbType.Int, 50);
            uploadedsubject_name.Value = AppNo;
            cmd.Parameters.Add(uploadedsubject_name);
            uploadedsubject_name = new SqlParameter("@photoid", SqlDbType.Binary, FileSize);
            uploadedsubject_name.Value = DocDocument;
            cmd.Parameters.Add(uploadedsubject_name);
            ssql.Close();
            ssql.Open();
            Result = cmd.ExecuteNonQuery();
            ssql.Close();
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, Convert.ToString(collegecode), "StudentPhotoStatus");
        }
        return Result;
    }
}