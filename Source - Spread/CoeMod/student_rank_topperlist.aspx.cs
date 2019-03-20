using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FarPoint.Web.Spread;
using Gios.Pdf;
using System.Configuration;

public partial class student_rank_topperlist : System.Web.UI.Page
{
    string funcsubno = string.Empty;
    string funcsubname = string.Empty;
    string funcsubcode = string.Empty;
    string funcresult = string.Empty;
    string funcsemester = string.Empty;
    string funccredit = string.Empty;
    string funcgrade = string.Empty;
    string previousgrade = string.Empty;
    string mark = string.Empty;
    string calculate = string.Empty;

    [Serializable()]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(75);
            return img;
        }
    }

    string ccva = string.Empty;
    string strgrade = string.Empty;
    double creditval = 0;
    double finalgpa1 = 0;
    double creditsum1 = 0;
    double gpacal1 = 0;
    string strsubcrd = string.Empty;
    string examcodeval = string.Empty;
    double strtot = 0;
    double strgradetempfrm = 0;
    double strgradetempto = 0;
    string strtotgrac = string.Empty;
    string strgradetempgrade = string.Empty;
    string usercode = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    double total1 = 0;
    SqlCommand cmd;
    connection connection = new connection();
    DataSet daload = new DataSet();
    SqlDataAdapter adaload;
    DataSet ds1 = new DataSet();
    int count = 0;
    string roll_value = string.Empty;
    string query_header = string.Empty;
    DataSet dsfind = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    Hashtable hrank = new Hashtable();

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    public void clear()
    {
        btnprintmaster.Visible = false;
        FpSpread1.Visible = false;
        errmsg.Visible = false;
        btnxl.Visible = false;
        lblnorec.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = false;
        rdover.Checked = true;
        ddlyear.Visible = false;
        ddlmonth.Visible = false;
        lblyear.Visible = false;
        lblmonth.Visible = false;
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
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            errmsg.Visible = false;
            lblnorec.Visible = false;
            if (!IsPostBack)
            {
                bindcollege();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                //loadmonth();
                //bindexamyear();
                BindExamYear();
                BindExamMonth();
                FpSpread1.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                FpSpread1.CommandBar.Visible = false;
                clear();
            }
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            ds = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (ds.Tables[0].Rows[k]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[k]["value"].ToString() == "1")
                    {
                        Session["Studflag"] = "1";
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void bindcollege()
    {
        try
        {
            string group_code = Session["group_code"].ToString();
            string columnfield = string.Empty;
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
            ds = d2.select_method("bind_college", hat, "sp");
            ddlclg.Items.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
            BindBatch();
            BindDegree(singleuser, group_user, collegecode, usercode);
        }
        catch (Exception ex)
        {
        }
    }

    public void BindBatch()
    {
        try
        {
            count = 0;
            chklsbatch.Items.Clear();
            chkbatch.Checked = false;
            txtbatch.Text = "---Select---";
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    count += 1;
                }
                if (count > 0)
                {
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            chklstdegree.Items.Clear();
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            collegecode = ddlclg.SelectedItem.Value;
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
                count = 0;
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
                if (count > 0)
                {
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                        txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
                    }
                }
                else
                {
                    chkdegree.Checked = false;
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            chklstbranch.Items.Clear();
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
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            collegecode = ddlclg.SelectedValue.ToString();
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
                count = 0;
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
                if (count > 0)
                {
                    if (chklstbranch.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                        txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
                    }
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
            }
            // BindSectionDetailmult(collegecode);
        }
        catch (Exception ex)
        {
            errmsg.Text = "Please Select the Degree";
        }
    }

    public string getsemester(string semester)
    {
        string sem = string.Empty;
        string gsem3 = string.Empty;
        string year = string.Empty;
        sem = semester;
        if (sem == "1")
        {
            gsem3 = "I";
            year = "I";
        }
        else if (sem == "2")
        {
            gsem3 = "II";
            year = "I";
        }
        else if (sem == "3")
        {
            gsem3 = "III";
            year = "II";
        }
        else if (sem == "4")
        {
            gsem3 = "IV";
            year = "II";
        }
        else if (sem == "5")
        {
            gsem3 = "V";
            year = "III";
        }
        else if (sem == "6")
        {
            gsem3 = "VI";
            year = "III";
        }
        else if (sem == "7")
        {
            gsem3 = "VII";
            year = "IV";
        }
        else if (sem == "8")
        {
            gsem3 = "VIII";
            year = "IV";
        }
        else if (sem == "9")
        {
            gsem3 = "IX";
            year = "V";
        }
        else if (sem == "10")
        {
            gsem3 = "X";
            year = "V";
        }
        return gsem3;
    }

    public void bindexamyear()
    {
        DataSet dsbindexamyear = new DataSet();
        SqlDataAdapter sqldap = new SqlDataAdapter();
        string batchquery = "select distinct Exam_year from Exam_Details order by  Exam_year asc ";
        dsbindexamyear = d2.select_method(batchquery, hat, "text ");
        if (dsbindexamyear.Tables.Count > 0 && dsbindexamyear.Tables[0].Rows.Count > 0)
        {
            ddlyear.DataSource = dsbindexamyear;
            ddlyear.DataTextField = "Exam_year";
            ddlyear.DataValueField = "Exam_year";
            ddlyear.DataBind();
            ddlyear.Items.Insert(0, " ");
        }
    }

    protected void loadmonth()
    {
        ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        ddlmonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Jan", "1"));
        ddlmonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Feb", "2"));
        ddlmonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Mar", "3"));
        ddlmonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("Apr", "4"));
        ddlmonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("May", "5"));
        ddlmonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jun", "6"));
        ddlmonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Jul", "7"));
        ddlmonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Aug", "8"));
        ddlmonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Sep", "9"));
        ddlmonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Oct", "10"));
        ddlmonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Nov", "11"));
        ddlmonth.Items.Insert(12, new System.Web.UI.WebControls.ListItem("Dec", "12"));
    }

    /// <summary>
    /// Added By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            if (ddlclg.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    collegeCode = " and coll_code in (" + collegeCode + ")";
                }
            }
            if (chklsbatch.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    batchYear = " and batch_year in (" + batchYear + ")";
                }
            }
            if (chklstbranch.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklstbranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    degreeCode = " and degree_code in (" + degreeCode + ")";
                }
            }
            //if (ddlsem.Items.Count > 0)
            //{
            //    semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            //}
            ddlyear.Items.Clear();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(collegeCode))
            {
                string qry = "select distinct Exam_year from exam_details where Exam_year<>'0' " + collegeCode + degreeCode + batchYear + " order by Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlyear.DataSource = ds;
                    ddlyear.DataTextField = "Exam_year";
                    ddlyear.DataValueField = "Exam_year";
                    ddlyear.DataBind();
                }
                //ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
            }
            ddlyear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch (Exception ex)
        {
            //lblErrSearch.Text = Convert.ToString(ex);
            //lblErrSearch.Visible = true;
            //d2.sendErrorMail(ex, (ddlCollege.Items.Count > 0 ? Convert.ToString(ddlCollege.SelectedValue).Trim() : ((Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "13")), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            if (ddlclg.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlclg.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(collegeCode))
                        {
                            collegeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            collegeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(collegeCode))
                {
                    collegeCode = " and coll_code in (" + collegeCode + ")";
                }
            }
            if (chklsbatch.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklsbatch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(batchYear))
                        {
                            batchYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            batchYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(batchYear))
                {
                    batchYear = " and batch_year in (" + batchYear + ")";
                }
            }
            if (chklstbranch.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in chklstbranch.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(degreeCode))
                        {
                            degreeCode = "'" + li.Value + "'";
                        }
                        else
                        {
                            degreeCode += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(degreeCode))
                {
                    degreeCode = " and degree_code in (" + degreeCode + ")";
                }
            }
            //if (ddlsem.Items.Count > 0)
            //{
            //    semester = Convert.ToString(ddlsem.SelectedItem.Text).Trim();
            //}
            string ExamYear = string.Empty;
            if (ddlyear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlyear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    ExamYear = " and Exam_year in (" + ExamYear + ")";
                }
            }
            ddlmonth.Items.Clear();
            if (!string.IsNullOrEmpty(batchYear) && !string.IsNullOrEmpty(degreeCode) && !string.IsNullOrEmpty(collegeCode))
            {
                string qry = "select distinct Exam_Month,upper(convert(varchar(3),DateAdd(month,Exam_Month,-1))) as Month_Name from exam_details where Exam_Month<>'0' " + collegeCode + degreeCode + batchYear + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlmonth.DataSource = ds;
                    ddlmonth.DataTextField = "Month_Name";
                    ddlmonth.DataValueField = "Exam_Month";
                    ddlmonth.DataBind();
                }
            }
            else
            {
            }
            ddlmonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
        }
        catch
        {
        }
    }

    protected void ddlclg_click(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        BindBatch();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        //loadmonth();
        //bindexamyear();
        BindExamYear();
        BindExamMonth();
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
                    if (rdsem.Checked == true)
                    {
                        chkbatch.Checked = false;
                        chklsbatch.Items[i].Selected = false;
                        txtbatch.Text = "---Select---";
                    }
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
            BindExamYear();
            BindExamMonth();
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
            FpSpread1.Visible = false;
            lblreptname.Visible = false;
            txtreptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            int i = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            string clg = string.Empty;
            int commcount = 0;
            for (i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    if (rdsem.Checked == true)
                    {
                        if (commcount > 1)
                        {
                            chklsbatch.Items[i].Selected = false;
                            if (clg == "")
                            {
                                clg = chklsbatch.Items[i].Value.ToString();
                            }
                            else
                            {
                                clg = clg + "','" + chklsbatch.Items[i].Value;
                            }
                        }
                        else
                        {
                            //ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"You Cannot Select More then one Batch Year\");", true);
                        }
                    }
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
            if (rdover.Checked == true)
            {
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            }
            if (rdsem.Checked == true)
            {
                if (commcount == 1)
                {
                    BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                }
                else
                {
                    chklsbatch.ClearSelection();
                    txtbatch.Text = "--Select--";
                    FpSpread1.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    lblreptname.Visible = false;
                    txtreptname.Visible = false;
                    lblnorec.Visible = true;
                    lblnorec.Text = "You Cannot Select More then one batch year";
                }
                if (commcount > 1)
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"You Cannot Select More then One Batch Year\");", true);
                }
            }
            BindExamYear();
            BindExamMonth();
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
            BindExamYear();
            BindExamMonth();
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
            FpSpread1.Visible = false;
            lblreptname.Visible = false;
            txtreptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            txtbranch.Text = "---Select---";
            chkbranch.Checked = false;
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
                txtbranch.Text = "---Select---";
            }
            if (commcount > 0)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
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
            BindExamYear();
            BindExamMonth();
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
                    chkdegree.Checked = true;
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
                    chkdegree.Checked = false;
                }
                chklstbranch.Items.Clear();
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            BindExamYear();
            BindExamMonth();
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
            FpSpread1.Visible = false;
            lblreptname.Visible = false;
            txtreptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            string clg = string.Empty;
            int commcount = 0;
            chkdegree.Checked = false;
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
                else
                {
                    chklstbranch.Items.Clear();
                    chkbranch.Checked = false;
                    txtbranch.Text = "---Select---";
                }
            }
            if (commcount == 0)
            {
                txtdegree.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (commcount > 0)
            {
                chkdegree.Checked = false;
            }
            BindExamYear();
            BindExamMonth();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
        }
    }

    protected void rbtoporbelow_selectedindexchanged(object sender, EventArgs e)
    {
        if (rbtoporbelow.SelectedValue == "0")
        {
            Label2.Text = "Top";
        }
        else if (rbtoporbelow.SelectedValue == "1")
        {
            Label2.Text = "Below";
        }
        FpSpread1.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
    }

    protected void ddlyear_Selectedindex(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.Visible = false;
            lblreptname.Visible = false;
            txtreptname.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            //string strsql = "select distinct Exam_Month  from Exam_Details  where exam_year='" + ddlyear.SelectedItem.Value.ToString() + "'";
            //ds = d2.select_method_wo_parameter(strsql, "Text");
            //ddlmonth.Items.Clear();
            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    int month = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());
            //    if (month == 1)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jan", "1"));
            //    }
            //    if (month == 2)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Feb", "2"));
            //    }
            //    if (month == 3)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Mar", "3"));
            //    }
            //    if (month == 4)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Apr", "4"));
            //    }
            //    if (month == 5)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("May", "5"));
            //    }
            //    if (month == 6)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jun", "6"));
            //    }
            //    if (month == 7)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Jul", "7"));
            //    }
            //    if (month == 8)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Aug", "8"));
            //    }
            //    if (month == 9)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Sep", "9"));
            //    }
            //    if (month == 10)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Oct", "10"));
            //    }
            //    if (month == 11)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Nov", "11"));
            //    }
            //    if (month == 12)
            //    {
            //        ddlmonth.Items.Insert(i, new System.Web.UI.WebControls.ListItem("Dec", "12"));
            //    }
            //}
            BindExamMonth();
        }
        catch (Exception ex)
        {
        }
    }

    protected void ddlmonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpSpread1.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
    }

    protected void rdsem_CheckedChanged(object sender, EventArgs e)
    {
        //chklsbatch.ClearSelection();
        lblmonth.Visible = true;
        ddlmonth.Visible = true;
        lblyear.Visible = true;
        ddlyear.Visible = true;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        rdover.Checked = false;
        FpSpread1.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        BindExamYear();
        BindExamMonth();
    }

    protected void rrdover_CheckedChanged(object sender, EventArgs e)
    {
        lblmonth.Visible = false;
        ddlmonth.Visible = false;
        lblyear.Visible = false;
        ddlyear.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        FpSpread1.Visible = false;
        lblreptname.Visible = false;
        txtreptname.Visible = false;
        btnprintmaster.Visible = false;
        btnxl.Visible = false;
    }

    protected void btn_printmarks(object sender, EventArgs e)
    {
        mpgetamount.Show();
        int activerow = FpSpread1.Sheets[0].ActiveRow;
        int activecol = FpSpread1.Sheets[0].ActiveColumn;
        string reg_no = FpSpread1.Sheets[0].Cells[activerow, 2].Text;
        string roll_no = FpSpread1.Sheets[0].Cells[activerow, 1].Text;
        string student_name = FpSpread1.Sheets[0].Cells[activerow, 3].Text;
        string cgpavalue = FpSpread1.Sheets[0].Cells[activerow, 4].Text;
        string classification = FpSpread1.Sheets[0].Cells[activerow, 5].Text;
        string totalmarks = FpSpread1.Sheets[0].Cells[activerow, 6].Text;
        string rank = FpSpread1.Sheets[0].Cells[activerow, 7].Text;
        string sam5 = string.Empty;
        if (rdover.Checked == true)
        {
            sam5 = "            " + "Reg No" + " : " + reg_no + "                                                                                                                                " + "CGPA" + " : " + cgpavalue;
        }
        else
        {
            sam5 = "            " + "Reg No" + " : " + reg_no + "                                                                                                                                " + "GPA" + " : " + cgpavalue;
        }
        string sam = "            " + "Roll No" + " : " + roll_no + "                                                                                                                              " + "Total Marks" + " : " + totalmarks;
        string sam4 = "            " + "Student Name " + " : " + student_name;
        string sam3 = "            " + "Classification " + " : " + classification;
        string sam2 = "            " + "Rank" + " : " + rank;
        FpSpread1.Visible = true;
        string date = "@" + sam5 + "@" + sam + "@" + sam4 + "@" + sam3 + "@" + sam2;
        string pagename = "student_rank_topperlist.aspx";
        string degreedetails = "StudentMarksList" + date;
        Printcontrol.loadspreaddetails(Fpstudentmark, pagename, degreedetails);
        Printcontrol.Visible = true;
        mpgetamount.Hide();
        //Fpstudentmark.Visible = true;
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            DataView dvcount = new DataView();
            Printcontrol.Visible = false;
            txtreptname.Visible = true;
            lblreptname.Visible = true;
            Hashtable hatstutotal = new Hashtable();
            string finaltotal = string.Empty;
            string classcalcu = string.Empty;
            string batch = string.Empty;
            string batch1 = string.Empty;
            int semwisebatchcount = 0;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    semwisebatchcount++;
                    batch = chklsbatch.Items[i].Text.ToString();
                    if (batch1 == "")
                    {
                        batch1 = batch;
                    }
                    else
                    {
                        batch1 = batch1 + "'" + "," + "'" + batch;
                    }
                }
            }
            if (semwisebatchcount > 1 && rdsem.Checked == true)
            {
                txtreptname.Visible = false;
                lblreptname.Visible = false;
                lblnorec.Text = "Please Select Only One Batch Year";
                lblnorec.Visible = true;
                FpSpread1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                return;
            }
            if (batch1 != "")
            {
                Hashtable htrank = new Hashtable();
                ArrayList addarray = new ArrayList();
                ds.Dispose();
                ds = d2.select_method("select * from sysobjects where name='tbl_Topperrank' and Type='U'", hat, "text ");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                {
                    int p = d2.insert_method("create table tbl_Topperrank (roll_no nvarchar(50),cgpa float (8),stud_name nvarchar(200),degree nvarchar(500),user_code nvarchar(25))", hat, "text");
                }
                else
                {
                    int p = d2.insert_method("IF not EXISTS (SELECT * FROM   INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME = 'tbl_Topperrank' AND COLUMN_NAME = 'user_code') alter table tbl_Topperrank add user_code nvarchar(15)", hat, "text");
                }
                ds.Dispose();
                dsfind = d2.select_method("select name from sysobjects where xtype='p' and name='sp_ins_upd_topperrank' ", hat, "text");
                if (dsfind.Tables.Count > 0 && dsfind.Tables[0].Rows.Count == 0)
                {
                    string spcreation = " CREATE procedure sp_ins_upd_topperrank (@RollNumber varchar(50), @cgpa varchar(20), @stud_name varchar(20), @degree varchar(200) ,@user_code nvarchar(25))  as  declare @cou_nt  int set @cou_nt=(select count(Roll_no)from tbl_Topperrank where Roll_no=@RollNumber) if @cou_nt=0 BEGIN insert into tbl_Topperrank(Roll_no,cgpa,stud_name,degree,user_code) values (@RollNumber,@cgpa,@stud_name,@degree,@user_code) End Else BEGIN update  tbl_Topperrank set cgpa=@cgpa where Roll_no=@RollNumber and user_code=@user_code end";
                    int s = d2.insert_method(spcreation, hat, "Text");
                }
                else
                {
                    string spalter = " alter procedure sp_ins_upd_topperrank (@RollNumber varchar(50), @cgpa   varchar(20), @stud_name varchar(20), @degree varchar(200) ,@user_code nvarchar(25))    as  declare @cou_nt  int set @cou_nt=(select count(Roll_no)from tbl_Topperrank    where Roll_no=@RollNumber) if @cou_nt=0 BEGIN insert into tbl_Topperrank(Roll_no,   cgpa,stud_name,degree,user_code)values(@RollNumber,@cgpa,@stud_name,@degree,   @user_code) End Else BEGIN update  tbl_Topperrank set cgpa=@cgpa where    Roll_no=@RollNumber and user_code=@user_code End";
                    int gf = d2.insert_method(spalter, hat, "Text");
                }
                string strquerydelrecordes = "delete from tbl_Topperrank";
                int ag = d2.update_method_wo_parameter(strquerydelrecordes, "text");
                ds.Dispose();
                string query_value = string.Empty;
                string sqlbatch = string.Empty;
                string sqlbranch = string.Empty;
                FpSpread1.Visible = true;
                lblnorec.Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 0;
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 10;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
              //  FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FarPoint.Web.Spread.TextCellType txt = new TextCellType();
                FarPoint.Web.Spread.TextCellType txt1 = new TextCellType();
                FarPoint.Web.Spread.TextCellType txt2 = new TextCellType();
                FpSpread1.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
                FpSpread1.Sheets[0].Columns[2].CellType = txt;
                FpSpread1.Sheets[0].Columns[3].CellType = txt;
                FpSpread1.Sheets[0].Columns[0].CellType = txt;
                FpSpread1.Sheets[0].Columns[1].CellType = txt;
                FpSpread1.Sheets[0].RowCount++;
                if (txtbatch.Text != "--Select--" || chklsbatch.Items.Count != null)
                {
                    for (int itemcount = 0; itemcount < chklsbatch.Items.Count; itemcount++)
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
                        sqlbatch = "and r.batch_year " + sqlbatch + "";
                    }
                }
                if (txtbranch.Text != "---Select---" || chklstbranch.Items.Count != null)
                {
                    for (int itemcount = 0; itemcount < chklstbranch.Items.Count; itemcount++)
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
                        sqlbranch = "and r.degree_code " + sqlbranch + "";
                    }
                }
                string degreecode = string.Empty;
                string batchyear = string.Empty;
                string cureentsem = string.Empty;
                string section = string.Empty;
                string cgpav = string.Empty;
                string branch = string.Empty;
                string name = string.Empty;
                string degree = string.Empty;
                string regno = string.Empty;
                string mode = string.Empty;
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                style.Font.Size = 10;
                style.Font.Bold = true;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpSpread1.Sheets[0].AllowTableCorner = true;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.ActiveSheetView.DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].ColumnCount = 12;
                //MyImg mi = new MyImg();
                //mi.ImageUrl = "~/Student Photo/10BIT001.jpeg";
                //mi.ImageUrl = "Handler/Handler2.ashx?";
                //MyImg mi2 = new MyImg();
                //mi2.ImageUrl = "~/images/10BIT001.jpeg";
                //mi2.ImageUrl = "Handler/Handler5.ashx?";
                query_header = "select collname,address3,pincode from collinfo where college_code=" + Session["collegecode"] + "";
                DataSet dshead = new DataSet();
                dshead = d2.select_method(query_header, hat, "Text");
                if (dshead.Tables.Count > 0 && dshead.Tables[0].Rows.Count > 0)
                {
                    string collegename = dshead.Tables[0].Rows[0]["collname"].ToString();
                    string address = dshead.Tables[0].Rows[0]["address3"].ToString() + " - " + dshead.Tables[0].Rows[0]["pincode"].ToString() + ".";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Columns[1].Width = 75;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    if (Session["Rollflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[1].Visible = false;
                    }
                    if (Session["Regflag"].ToString() == "1")
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Columns[2].Visible = false;
                    }
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt1;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[1].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = " Student Name";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[3].Width = 250;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Batch";
                    FpSpread1.Sheets[0].SetColumnMerge(4, FarPoint.Web.Spread.Model.MergePolicy.Always);
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Branch";
                    FpSpread1.Sheets[0].Columns[5].Width = 150;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].SetColumnMerge(5, FarPoint.Web.Spread.Model.MergePolicy.Always);

                    if (rdover.Checked == true)
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "CGPA";
                    }
                    else
                    {
                        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "GPA";
                    }
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[6].Width = 50;
                    ////=================Addedby jeyagandhi (19/6/2015)==============/////
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Classification";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[7].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Total Marks";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[8].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Rank";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[9].Width = 50;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Photo";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[10].Width = 100;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Text = "View Marks/Grade";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[11].Width = 120;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                    ////=================Addedby jeyagandhi (19/6/2015)==============/////
                }
                for (int i = 0; i < FpSpread1.Sheets[0].Columns.Count - 1; i++)
                {
                    FpSpread1.Sheets[0].Columns[i].Locked = true;
                }
                if (rdover.Checked == true)
                {
                    query_value = "select distinct r.stud_name,r.Reg_No,r.batch_year,r.current_semester,r.sections,r.degree_code,d.dept_name,course.course_name,dg.course_id,r.mode ,r.roll_no,r.degree_code from registration r,mark_entry me ,department d,course,degree dg where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = course.course_id and dg.dept_code = d.dept_code  and me.result='pass' " + sqlbatch + " " + sqlbranch + "";
                    ds.Dispose();
                    ds.Reset();
                    ds = d2.select_method(query_value, hat, "Text");
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        batchyear = ds.Tables[0].Rows[0]["batch_year"].ToString();
                        cureentsem = ds.Tables[0].Rows[0]["current_semester"].ToString();
                        degreecode = ds.Tables[0].Rows[0]["degree_code"].ToString();
                        // string exam = "select Exam_Month,Exam_year from Exam_Details where degree_code='" + degreecode + "' and batch_year='" + batchyear + "' and current_semester='2'";
                        //DataSet dsexam = d2.select_method_wo_parameter(exam, "text");
                        //if (dsexam.Tables[0].Rows.Count > 0)
                        //{
                        for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                        {
                            roll_value = ds.Tables[0].Rows[rolcount]["roll_no"].ToString();
                            name = ds.Tables[0].Rows[rolcount]["stud_name"].ToString();
                            batchyear = ds.Tables[0].Rows[rolcount]["batch_year"].ToString();
                            cureentsem = ds.Tables[0].Rows[rolcount]["current_semester"].ToString();
                            mode = ds.Tables[0].Rows[rolcount]["mode"].ToString();
                            degree = ds.Tables[0].Rows[rolcount]["course_name"].ToString();
                            degreecode = ds.Tables[0].Rows[rolcount]["degree_code"].ToString();
                            branch = ds.Tables[0].Rows[rolcount]["dept_name"].ToString();
                            section = ds.Tables[0].Rows[rolcount]["sections"].ToString();
                            // string exam_month = dsexam.Tables[0].Rows[0]["Exam_Month"].ToString();
                            // string exam_year = dsexam.Tables[0].Rows[0]["Exam_Year"].ToString();
                            string degreevalue = batchyear + '-' + degree + '[' + branch + ']' + '-' + "sem" + ' ' + cureentsem + ' ' + '[' + section + ']';
                            int failcount = Convert.ToInt32(d2.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No  and roll_no='" + roll_value + "' and result<>'pass' and Subject.subject_no not in(select m.subject_no from mark_entry m where roll_no='" + roll_value + "' and m.result='Pass')"));
                            if (failcount == 0)
                            {
                                Calculete_CGPA(roll_value, cureentsem, degreecode, batchyear, mode, collegecode);
                                cgpav = Convert.ToString(calculate.ToString());
                                total1 = Math.Round(total1, 0, MidpointRounding.AwayFromZero);
                                if (cgpav != "0" && cgpav != "" && cgpav != "-" && cgpav != "NaN")
                                {
                                    Double num = 0;
                                    if (Double.TryParse(cgpav, out num))
                                    {
                                        hat.Clear();
                                        hat.Add("RollNumber", roll_value);
                                        hat.Add("cgpa", cgpav.ToString());
                                        hat.Add("stud_name", name.ToString());
                                        hat.Add("degree", degreevalue.ToString());
                                        hat.Add("user_code", usercode.ToString());
                                        int o = d2.insert_method("sp_ins_upd_topperrank", hat, "sp");
                                        if (!hatstutotal.Contains(roll_value.Trim().ToLower()))
                                        {
                                            hatstutotal.Add(roll_value.Trim().ToLower(), total1);
                                        }
                                    }
                                }
                            }
                        }
                       FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                        DataSet ds3 = new DataSet();
                        string query = string.Empty;
                        if (txttop.Text != "")
                        {
                            query = "top " + txttop.Text + " ";
                            // query = "select " + query + " row_number() OVER (ORDER BY  cgpa asc) As SrNo,degree,roll_no,stud_name,cgpa,rank() OVER (ORDER BY  cgpa asc) as rank from tbl_Topperrank order by cgpa asc";
                        }
                        if (rbtoporbelow.SelectedValue == "0")
                        {
                            query = "select " + query + " row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,rank() OVER (ORDER BY  cgpa desc) as rank from tbl_Topperrank where user_code='" + usercode + "' order by cgpa desc";
                        }
                        else if (rbtoporbelow.SelectedValue == "1")
                        {
                            query = "select " + query + " row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,rank() OVER (ORDER BY  cgpa desc) as rank from tbl_Topperrank  where user_code='" + usercode + "'  order by rank desc";
                            DataSet dsrank = new DataSet();
                            // query = "select  row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,rank() OVER (ORDER BY  cgpa desc) as rank from tbl_Topperrank order by cgpa desc";
                            dsrank = d2.select_method(query, hat, "Text");
                            if (dsrank.Tables[0].Rows.Count > 0)
                            {
                                for (int ran = 0; ran < dsrank.Tables[0].Rows.Count; ran++)
                                {
                                    string rollnum = Convert.ToString(dsrank.Tables[0].Rows[ran]["roll_no"]);
                                    //string regnum = Convert.ToString(dsrank.Tables[0].Rows[ran]["Reg_No"]);
                                    string rank = Convert.ToString(dsrank.Tables[0].Rows[ran]["rank"]);
                                    htrank.Add(rollnum, rank);
                                }
                            }
                        }
                        ds3 = d2.select_method(query, hat, "Text");
                        FarPoint.Web.Spread.ButtonCellType objbtncell = new FarPoint.Web.Spread.ButtonCellType();
                        objbtncell.Text = "Marks/Grade";
                        int slno = 0;
                        if (ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            btnxl.Visible = true;
                            btnprintmaster.Visible = true;
                            for (int value = 0; value < ds3.Tables[0].Rows.Count; value++)
                            {
                                FpSpread1.Sheets[0].RowCount++;
                                slno++;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt1;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = txt2;
                                string rollvalueget = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                string regvalue = "select Reg_No from Registration where Roll_No='" + rollvalueget + "' ";
                                DataSet regvalueds = d2.select_method_wo_parameter(regvalue, "text");
                                string regvaluefinal = regvalueds.Tables[0].Rows[0][0].ToString();
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regvaluefinal);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds3.Tables[0].Rows[value]["stud_name"]);

                                string degbranch = "select distinct r.stud_name,r.Reg_No,r.batch_year,r.current_semester,r.sections,r.degree_code,d.dept_name,course.course_name,dg.course_id,r.mode ,r.roll_no,r.degree_code from registration r,mark_entry me ,department d,course,degree dg where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = course.course_id and dg.dept_code = d.dept_code  and me.result='pass'  and r.roll_no='" + rollvalueget + "'";
                            DataSet ds6 = d2.select_method_wo_parameter(degbranch, "text");
                            if (ds6.Tables[0].Rows.Count > 0 && ds6.Tables.Count > 0)
                            {
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds6.Tables[0].Rows[0]["batch_year"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds6.Tables[0].Rows[0]["course_name"]) + "-" + Convert.ToString(ds6.Tables[0].Rows[0]["dept_name"]);
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds3.Tables[0].Rows[value]["cgpa"]);
                                string totalval = string.Empty;
                                if (hatstutotal.Contains(ds3.Tables[0].Rows[value]["roll_no"].ToString().Trim().ToLower()))
                                {
                                    totalval = hatstutotal[ds3.Tables[0].Rows[value]["roll_no"].ToString().Trim().ToLower()].ToString();
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = totalval;
                                ////=================Addedby jeyagandhi (19/6/2015)==============/////
                                string classfi = Convert.ToString(ds3.Tables[0].Rows[value]["cgpa"]);
                                string gettype = d2.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + degreecode + "'");
                                string classcal = "select classification from coe_classification where edu_level = '" + gettype + "' and collegecode = '" + Session["collegecode"].ToString() + "'  and '" + classfi + "' between frompoint and topoint ";
                                DataSet dsclass = d2.select_method_wo_parameter(classcal, "text");
                                if (dsclass.Tables[0].Rows.Count > 0)
                                {
                                    classcalcu = dsclass.Tables[0].Rows[0]["classification"].ToString();
                                }
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(classcalcu);
                                ////=================Addedby jeyagandhi (19/6/2015)==============/////
                                if (rbtoporbelow.SelectedValue == "0")
                                {
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds3.Tables[0].Rows[value]["rank"]);
                                }
                                else
                                {
                                    string roll = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                    string rank = string.Empty;
                                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds3.Tables[0].Rows[value]["rank"]);
                                }
                                //----------- Student photo Added------------   By Solairaj 24-4-15
                                string roll_no = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"].ToString());
                                MyImg mi5 = new MyImg();
                                mi5.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = mi5;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Note = roll_no;
                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].CellType = objbtncell;


                              

                            }
                            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                            FpSpread1.Height = 800;
                            FpSpread1.Visible = true;
                        }//Added By Srinath 13/*3/2014
                        else
                        {
                            btnprintmaster.Visible = false;
                            FpSpread1.Visible = false;
                            btnxl.Visible = false;
                            lblreptname.Visible = false;
                            txtreptname.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                        }
                        //}
                        //else
                        //{
                        //    btnprintmaster.Visible = false;
                        //    FpSpread1.Visible = false;
                        //    btnxl.Visible = false;
                        //    lblreptname.Visible = false;
                        //    txtreptname.Visible = false;
                        //    lblnorec.Visible = true;
                        //    lblnorec.Text = "No Records Found";
                        //}
                    }
                    else
                    {
                        btnprintmaster.Visible = false;
                        FpSpread1.Visible = false;
                        btnxl.Visible = false;
                        lblreptname.Visible = false;
                        txtreptname.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Records Found";
                    }
                }
                //-------------------Reg No and Photo Added  by Solairaj-----------------24-4-2015
                else
                {
                    if (ddlmonth.Text != "0" && ddlyear.Text != " ")
                    {
                        htrank.Clear();
                        string degreevalue = string.Empty;
                        string dept = string.Empty;
                        string exammonth = ddlmonth.SelectedValue.ToString();
                        string examyear = ddlyear.Text;
                        query_value = "select distinct r.stud_name,r.Reg_No,r.batch_year,r.current_semester,r.sections,r.degree_code,d.dept_name,course.course_name,dg.course_id,r.mode ,r.roll_no,r.degree_code from registration r,mark_entry me ,department d,course,degree dg,exam_details em where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = course.course_id and  dg.dept_code = d.dept_code  and me.result='pass' and em.exam_code=me.exam_code and em.exam_year='" + examyear + "' and em.exam_month='" + exammonth + "' " + sqlbatch + " " + sqlbranch + "order by r.batch_year,r.degree_code,r.current_semester,r.sections ";
                        ds.Dispose();
                        ds = d2.select_method(query_value, hat, "Text");
                        ////=================Addedby jeyagandhi (19/6/2015)==============/////
                        string getdegreedetails = string.Empty;
                        string getexamncode = string.Empty;
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                            {
                                roll_value = ds.Tables[0].Rows[rolcount]["roll_no"].ToString();
                                regno = ds.Tables[0].Rows[rolcount]["Reg_No"].ToString();
                                name = ds.Tables[0].Rows[rolcount]["stud_name"].ToString();
                                batchyear = ds.Tables[0].Rows[rolcount]["batch_year"].ToString();
                                cureentsem = ds.Tables[0].Rows[rolcount]["current_semester"].ToString();
                                mode = ds.Tables[0].Rows[rolcount]["mode"].ToString();
                                degree = ds.Tables[0].Rows[rolcount]["course_name"].ToString();
                                degreecode = ds.Tables[0].Rows[rolcount]["degree_code"].ToString();
                                branch = ds.Tables[0].Rows[rolcount]["dept_name"].ToString();
                                section = ds.Tables[0].Rows[rolcount]["sections"].ToString();
                                string tempdegreedetails = batchyear + '-' + degreecode + '-' + cureentsem + '-' + section;
                                if (tempdegreedetails != getdegreedetails)
                                {
                                    getexamncode = d2.GetFunction("select exam_code from Exam_Details where batch_year='" + batchyear + "' and degree_code='" + degreecode + "' and Exam_Month='" + exammonth + "' and Exam_year='" + examyear + "'");
                                }
                                if ((tempdegreedetails != getdegreedetails && getdegreedetails != "") || (rolcount == ds.Tables[0].Rows.Count - 1))
                                {
                                    if (rolcount == ds.Tables[0].Rows.Count - 1)
                                    {
                                        if (getexamncode != null && getexamncode.Trim() != "" & getexamncode.Trim() != "0")
                                        {
                                            int failcount = Convert.ToInt32(d2.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code ='" + getexamncode + "'  and roll_no='" + roll_value + "' and result='fail' and result='Fail' and attempts=1 "));
                                            if (failcount == 0)
                                            {
                                                degreevalue = batchyear + '-' + degree + '[' + branch + ']' + '-' + "sem" + ' ' + cureentsem + ' ' + '[' + section + ']';
                                                Calculete_CGPA(roll_value, cureentsem, degreecode, batchyear, mode, collegecode);
                                                cgpav = calculate.ToString();
                                                total1 = Math.Round(total1, 0, MidpointRounding.AwayFromZero);
                                                if (cgpav != "0" && cgpav != "" && cgpav != "NaN")
                                                {
                                                    Double num = 0;
                                                    if (Double.TryParse(cgpav, out num))
                                                    {
                                                        hat.Clear();
                                                        hat.Add("RollNumber", roll_value);
                                                        hat.Add("cgpa", cgpav.ToString());
                                                        hat.Add("stud_name", name.ToString());
                                                        hat.Add("degree", degreevalue.ToString());
                                                        hat.Add("user_code", usercode.ToString());
                                                        int o = d2.insert_method("sp_ins_upd_topperrank", hat, "sp");
                                                        if (!hatstutotal.Contains(roll_value.Trim().ToLower()))
                                                        {
                                                            hatstutotal.Add(roll_value.Trim().ToLower(), total1);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    FpSpread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                    DataSet ds3 = new DataSet();
                                    string query = string.Empty;
                                    if (txttop.Text != "")
                                    {
                                        query = "top " + txttop.Text + " ";
                                    }
                                    if (rbtoporbelow.SelectedValue == "0")
                                    {
                                        query = "select " + query + " row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,dense_rank() OVER (ORDER BY  cgpa desc)as rank from tbl_Topperrank  where user_code='" + usercode + "'  ";
                                    }
                                    else if (rbtoporbelow.SelectedValue == "1")
                                    {
                                        DataSet dsrank = new DataSet();
                                        query = "select " + query + "  row_number() OVER (ORDER BY  cgpa desc) As SrNo,degree,roll_no,stud_name,cgpa,dense_rank() OVER (ORDER BY  cgpa desc)as rank from tbl_Topperrank  where user_code='" + usercode + "'  order by rank desc";
                                        dsrank = d2.select_method(query, hat, "Text");
                                        if (dsrank.Tables[0].Rows.Count > 0)
                                        {
                                            for (int ran = 0; ran < dsrank.Tables[0].Rows.Count; ran++)
                                            {
                                                string rollnum = Convert.ToString(dsrank.Tables[0].Rows[ran]["roll_no"]);
                                                string rank = Convert.ToString(dsrank.Tables[0].Rows[ran]["rank"]);
                                                if (!htrank.ContainsKey(rollnum))
                                                {
                                                    htrank.Add(rollnum, rank);
                                                }
                                            }
                                        }
                                    }
                                    ds3 = d2.select_method(query, hat, "Text");
                                    FarPoint.Web.Spread.ButtonCellType objbtncell = new FarPoint.Web.Spread.ButtonCellType();
                                    objbtncell.Text = "Marks/Grade";
                                    int slno = 0;
                                    if (ds3.Tables[0].Rows.Count > 0)
                                    {
                                        FpSpread1.Sheets[0].RowCount++;
                                        btnxl.Visible = true;
                                        btnprintmaster.Visible = true;
                                        DataView dv_degree = new DataView();
                                        string[] spval = getdegreedetails.Split('-');
                                        if (spval.GetUpperBound(0) >= 1)
                                        {
                                            ds.Tables[0].DefaultView.RowFilter = "degree_code='" + Convert.ToString(spval[1]) + "'";
                                            dv_degree = ds.Tables[0].DefaultView;
                                            if (dv_degree.Count > 0)
                                            {
                                                branch = dv_degree[0]["dept_name"].ToString();
                                                dept = batchyear + " - " + degree + " [" + branch + "] -  Sem " + cureentsem;
                                                if (section.Trim() != "" && section.Trim() != "-1")
                                                {
                                                    dept = dept + " [" + section + "]";
                                                }
                                                addarray.Add(dv_degree[0]["dept_name"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = dept.ToString();
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Note = dept.ToString();
                                                FpSpread1.Sheets[0].SpanModel.Add(FpSpread1.Sheets[0].RowCount - 1, 0, 1, 10);
                                            }
                                        }
                                        for (int value = 0; value < ds3.Tables[0].Rows.Count; value++)
                                        {
                                            FpSpread1.Sheets[0].RowCount++;
                                            slno++;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].CellType = txt1;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                            string rollvalueget = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                            string regvalue = "select Reg_No from Registration where Roll_No='" + rollvalueget + "' ";
                                            DataSet regvalueds = d2.select_method_wo_parameter(regvalue, "text");
                                            string regvaluefinal = regvalueds.Tables[0].Rows[0][0].ToString();
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(regvaluefinal);
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds3.Tables[0].Rows[value]["stud_name"]);
                                            string degbranch = "select distinct r.stud_name,r.Reg_No,r.batch_year,r.current_semester,r.sections,r.degree_code,d.dept_name,course.course_name,dg.course_id,r.mode ,r.roll_no,r.degree_code from registration r,mark_entry me ,department d,course,degree dg where me.roll_no=r.roll_no and r.degree_code=dg.degree_code and dg.course_id = course.course_id and dg.dept_code = d.dept_code  and me.result='pass' " + sqlbatch + " " + sqlbranch + "";
                                            DataSet ds6 = d2.select_method_wo_parameter(degbranch, "text");
                                            if (ds6.Tables[0].Rows.Count > 0 && ds6.Tables.Count > 0)
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds6.Tables[0].Rows[value]["batch_year"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds6.Tables[0].Rows[value]["course_name"]) + "-" + Convert.ToString(ds6.Tables[0].Rows[value]["dept_name"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds3.Tables[0].Rows[value]["cgpa"]);
                                            string totalval = string.Empty;
                                            if (hatstutotal.Contains(ds3.Tables[0].Rows[value]["roll_no"].ToString().Trim().ToLower()))
                                            {
                                                totalval = hatstutotal[ds3.Tables[0].Rows[value]["roll_no"].ToString().Trim().ToLower()].ToString();
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = totalval;
                                            Calculete_CGPA(roll_value, cureentsem, degreecode, batchyear, mode, collegecode);
                                            cgpav = calculate.ToString();
                                            finaltotal = Math.Round(total1, 2).ToString();
                                            string classfi = Convert.ToString(ds3.Tables[0].Rows[value]["cgpa"]);
                                            string gettype = d2.GetFunction("Select Edu_Level from course c,degree d where c.course_id=d.course_id and d.degree_code='" + degreecode + "'");
                                            string classcal = "select classification from coe_classification where edu_level = '" + gettype + "' and collegecode = '" + Session["collegecode"].ToString() + "'  and '" + classfi + "' between frompoint and topoint ";
                                            DataSet dsclass = d2.select_method_wo_parameter(classcal, "text");
                                            if (dsclass.Tables[0].Rows.Count > 0)
                                            {
                                                classcalcu = dsclass.Tables[0].Rows[0]["classification"].ToString();
                                            }
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(classcalcu);
                                            if (rbtoporbelow.SelectedValue == "0")
                                            {
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds3.Tables[0].Rows[value]["rank"]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = txt1;
                                            }
                                            else
                                            {
                                                string roll = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"]);
                                                string rank = "-";
                                                rank = Convert.ToString(htrank[roll]);
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = rank;
                                                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].CellType = txt1;
                                            }
                                            //----------- Student photo Added------------   By Solairaj 24-4-15
                                            string roll_no = Convert.ToString(ds3.Tables[0].Rows[value]["roll_no"].ToString());
                                            MyImg mi5 = new MyImg();
                                            mi5.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll_no;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].CellType = mi5;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Note = roll_no;
                                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].CellType = objbtncell;
                                        }
                                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                                        FpSpread1.Height = 800;
                                        FpSpread1.Visible = true;
                                    }
                                    string strquerydelrecorde = "delete from tbl_Topperrank";
                                    int a = d2.update_method_wo_parameter(strquerydelrecorde, "text");
                                }
                                if (getexamncode != null && getexamncode.Trim() != "" & getexamncode.Trim() != "0")
                                {
                                    int failcount = Convert.ToInt32(d2.GetFunction(" Select COUNT(*) from Mark_Entry,Subject where  Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code ='" + getexamncode + "'  and roll_no='" + roll_value + "' and result='fail' and result='Fail'"));
                                    if (failcount == 0)
                                    {
                                        degreevalue = batchyear + '-' + degree + '[' + branch + ']' + '-' + "sem" + ' ' + cureentsem + ' ' + '[' + section + ']';
                                        Calulat_GPA_Semwise(roll_value, degreecode, batchyear, exammonth, examyear, collegecode);
                                        cgpav = calculate.ToString();
                                        total1 = Math.Round(total1, 0, MidpointRounding.AwayFromZero);
                                        if (cgpav != "0" && cgpav != "" && cgpav != "NaN")
                                        {
                                            Double num = 0;
                                            if (Double.TryParse(cgpav, out num))
                                            {
                                                hat.Clear();
                                                hat.Add("RollNumber", roll_value);
                                                hat.Add("cgpa", cgpav.ToString());
                                                hat.Add("stud_name", name.ToString());
                                                hat.Add("degree", degreevalue.ToString());
                                                hat.Add("user_code", usercode.ToString());
                                                int o = d2.insert_method("sp_ins_upd_topperrank", hat, "sp");
                                                if (!hatstutotal.Contains(roll_value.Trim().ToLower()))
                                                {
                                                    hatstutotal.Add(roll_value.Trim().ToLower(), total1);
                                                }
                                            }
                                        }
                                    }
                                }
                                getdegreedetails = tempdegreedetails;
                            }
                        }
                        int rowcount = FpSpread1.Sheets[0].RowCount;
                        if (FpSpread1.Sheets[0].RowCount > 1)
                        {
                            FpSpread1.Visible = true;
                            btnxl.Visible = true;
                            btnprintmaster.Visible = true;
                            lblreptname.Visible = true;
                            txtreptname.Visible = true;
                            lblnorec.Visible = false;
                            lblnorec.Text = string.Empty;
                        }
                        else
                        {
                            FpSpread1.Visible = false;
                            btnxl.Visible = false;
                            btnprintmaster.Visible = false;
                            lblreptname.Visible = false;
                            txtreptname.Visible = false;
                            lblnorec.Visible = true;
                            lblnorec.Text = "No Records Found";
                            btnprintmaster.Visible = false;
                        }
                        FpSpread1.SaveChanges();
                    }
                    else
                    {
                        FpSpread1.Visible = false;
                        btnxl.Visible = false;
                        lblnorec.Visible = true;
                        lblnorec.Text = "Please Select Exam Month And Year";
                        FpSpread1.Visible = false;
                        btnprintmaster.Visible = false;
                        lblreptname.Visible = false;
                        txtreptname.Visible = false;
                        btnprintmaster.Visible = false;
                    }
                }
            }
            else
            {
                FpSpread1.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                lblreptname.Visible = false;
                txtreptname.Visible = false;
                lblnorec.Visible = true;
                lblnorec.Text = "Please Select Any one Batch";
                btnprintmaster.Visible = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
        finally
        {
            //ds.Dispose();
            //ds = d2.select_method("select name from sysobjects where name='tbl_Topperrank' and Type='U'", hat, "text ");
            //if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    int q = d2.insert_method("drop table tbl_Topperrank", hat, "text");
            //}
        }
    }

    public void bindmarkspread()
    {
        Fpstudentmark.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
        Fpstudentmark.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
        Fpstudentmark.CommandBar.Visible = false;
        //Fpstudentmark.Sheets[0].AutoPostBack = true;
        Fpstudentmark.Sheets[0].AutoPostBack = false;
        Fpstudentmark.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
        Fpstudentmark.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
        Fpstudentmark.Sheets[0].ColumnHeader.RowCount = 1;
        Fpstudentmark.Sheets[0].ColumnCount = 4;
        Fpstudentmark.Sheets[0].Columns[0].Width = 50;
        Fpstudentmark.Sheets[0].Columns[1].Width = 150;
        Fpstudentmark.Sheets[0].Columns[2].Width = 300;
        Fpstudentmark.Sheets[0].Columns[3].Width = 100;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Sub Code";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Subject Name";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Marks/Grade";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        Fpstudentmark.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        Fpstudentmark.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        Fpstudentmark.Sheets[0].RowCount = 0;
        Fpstudentmark.Height = 500;
        Fpstudentmark.Sheets[0].RowHeader.Visible = false;
    }

    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        int slno = 0;
        int activerow = FpSpread1.Sheets[0].ActiveRow;
        int activecol = FpSpread1.Sheets[0].ActiveColumn;
        string roll_no = FpSpread1.Sheets[0].Cells[activerow, 1].Text;
        DataSet dsexamcodevalue = new DataSet();
        DataSet dsmark = new DataSet();
        SqlCommand cmd;
        string grade_setting = string.Empty;
        SqlDataReader dr_grade_val;
        bindmarkspread();
        for (int i = 0; i < Fpstudentmark.Sheets[0].Columns.Count; i++)
        {
            Fpstudentmark.Sheets[0].Columns[i].Locked = true;
        }
        string getval = d2.GetFunction("select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "");
        if (getval.Trim() != "")
        {
            grade_setting = getval.ToString();
        }
        if (rdsem.Checked == true)
        {
            string getexamdetails = "select exam_code,ed.current_semester,Exam_Month,Exam_year,ed.batch_year,ed.degree_code from Exam_Details ed,Registration r    where ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and r.roll_no='" + roll_no + "'    and exam_month='" + ddlmonth.SelectedValue + "' and exam_year='" + ddlyear.SelectedItem.Text + "' order by ed.current_semester,Exam_Month ";
            dsexamcodevalue = d2.select_method(getexamdetails, hat, "text ");
            if (dsexamcodevalue.Tables[0].Rows.Count > 0)
            {
                for (int cntvalue = 0; cntvalue < dsexamcodevalue.Tables[0].Rows.Count; cntvalue++)
                {
                    string gradeflag = string.Empty;
                    string examcode = Convert.ToString(dsexamcodevalue.Tables[0].Rows[0]["exam_code"]);
                    string degreecode = Convert.ToString(dsexamcodevalue.Tables[0].Rows[0]["degree_code"]);
                    string batchyear = Convert.ToString(dsexamcodevalue.Tables[0].Rows[0]["batch_year"]);
                    string semester = Convert.ToString(dsexamcodevalue.Tables[0].Rows[cntvalue]["current_semester"]);
                    string headersemester = getsemester(semester);
                    //Modified By Srinath 13/3/2014
                    //string getgradequery = "select grade_flag from grademaster where Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'";
                    string getgradequery = d2.GetFunction("select grade_flag from grademaster where Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "'  and exam_month='" + ddlmonth.SelectedValue + "' and exam_year='" + ddlyear.SelectedItem.Text + "'");
                    if (getgradequery.Trim() != "")
                    {
                        gradeflag = Convert.ToString(getgradequery);
                    }
                    string markquery = "select *, s.subject_name,s.subject_code,s.subject_no as subnum from mark_entry m,subject s where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and m.subject_no= s.subject_no";
                    dsmark = d2.select_method(markquery, hat, "text ");
                    if (dsmark.Tables[0].Rows.Count > 0)
                    {
                        Fpstudentmark.Sheets[0].RowCount++;
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antique";
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Text = headersemester + " " + "Semester";
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                       Fpstudentmark.Sheets[0].SpanModel.Add(Fpstudentmark.Sheets[0].RowCount - 1, 0, 1, 4);
                        for (int markval = 0; markval < dsmark.Tables[0].Rows.Count; markval++)
                        {
                            slno++;
                            Fpstudentmark.Sheets[0].RowCount++;
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                            string subjectcode = Convert.ToString(dsmark.Tables[0].Rows[markval]["subject_code"]);
                            string subjectname = Convert.ToString(dsmark.Tables[0].Rows[markval]["subject_name"]);
                            string subjectnumber = Convert.ToString(dsmark.Tables[0].Rows[markval]["subnum"]);
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 1].Text = subjectcode;
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 2].Text = subjectname;
                            string grade = string.Empty;
                            if (gradeflag != "")
                            {
                                if (gradeflag == "2")
                                {
                                    grade = Convert.ToString(dsmark.Tables[0].Rows[markval]["grade"]);
                                    Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grade);
                                }
                                else if (gradeflag == "3")
                                {
                                    if (grade_setting == "0") //bindtotal
                                    {
                                        //Modified By Srinath 12/3/2014
                                        double total = 0;
                                        string gettotalmark = dsmark.Tables[0].Rows[markval]["total"].ToString();
                                        if (gettotalmark != null && gettotalmark.Trim() != "")
                                        {
                                            total = Convert.ToDouble(dsmark.Tables[0].Rows[markval]["total"]);
                                        }
                                        //Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(total);
                                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(total);
                                    }
                                    else   //bind grade
                                    {
                                        convertgrade(roll_no, subjectnumber, degreecode, batchyear, examcode);
                                        //Modified By Srinath 12/3/2014
                                        //  Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 4].Text = funcgrade;
                                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = funcgrade;
                                    }
                                }
                            }
                        }
                        mpgetamount.Show();
                    }
                }
                Fpstudentmark.Sheets[0].PageSize = Fpstudentmark.Sheets[0].RowCount;
            }
        }
        else
        {
            string getexamdetails = "select exam_code,ed.current_semester,Exam_Month,Exam_year,ed.batch_year,ed.degree_code from Exam_Details ed,Registration r    where ed.degree_code=r.degree_code  and ed.batch_year=r.Batch_Year and r.roll_no='" + roll_no + "'  order by ed.current_semester,Exam_Month ";
            dsexamcodevalue = d2.select_method(getexamdetails, hat, "text ");
            if (dsexamcodevalue.Tables[0].Rows.Count > 0)
            {
                for (int cntvalue = 0; cntvalue < dsexamcodevalue.Tables[0].Rows.Count; cntvalue++)
                {
                    string gradeflag = string.Empty;
                    string examcode = Convert.ToString(dsexamcodevalue.Tables[0].Rows[cntvalue]["exam_code"]);
                    string degreecode = Convert.ToString(dsexamcodevalue.Tables[0].Rows[cntvalue]["degree_code"]);
                    string batchyear = Convert.ToString(dsexamcodevalue.Tables[0].Rows[cntvalue]["batch_year"]);
                    string semester = Convert.ToString(dsexamcodevalue.Tables[0].Rows[cntvalue]["current_semester"]);
                    string headersemester = getsemester(semester);
                    string getgradequery = d2.GetFunction("select grade_flag from grademaster where Degree_Code='" + degreecode + "' and batch_year='" + batchyear + "' ");
                    if (getgradequery.Trim() != "")
                    {
                        gradeflag = Convert.ToString(getgradequery);
                    }
                    string markquery = "select *, s.subject_name,s.subject_code,s.subject_no as subnum from mark_entry m,subject s where exam_code='" + examcode + "' and roll_no='" + roll_no + "' and m.subject_no= s.subject_no";
                    dsmark = d2.select_method(markquery, hat, "text ");
                    if (dsmark.Tables[0].Rows.Count > 0)
                    {
                        Fpstudentmark.Sheets[0].RowCount++;
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antique";
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Text = headersemester + " " + "Semester";
                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                        Fpstudentmark.Sheets[0].SpanModel.Add(Fpstudentmark.Sheets[0].RowCount - 1, 0, 1, 4);
                        for (int markval = 0; markval < dsmark.Tables[0].Rows.Count; markval++)
                        {
                            slno++;
                            Fpstudentmark.Sheets[0].RowCount++;
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 0].Text = slno.ToString();
                            string subjectcode = Convert.ToString(dsmark.Tables[0].Rows[markval]["subject_code"]);
                            string subjectname = Convert.ToString(dsmark.Tables[0].Rows[markval]["subject_name"]);
                            string subjectnumber = Convert.ToString(dsmark.Tables[0].Rows[markval]["subnum"]);
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 1].Text = subjectcode;
                            Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 2].Text = subjectname;
                            string grade = string.Empty;
                            if (gradeflag != "")
                            {
                                if (gradeflag == "2")
                                {
                                    grade = Convert.ToString(dsmark.Tables[0].Rows[markval]["grade"]);
                                    Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(grade);
                                }
                                else if (gradeflag == "3")
                                {
                                    if (grade_setting == "0") //bindtotal
                                    {
                                        double total = Convert.ToDouble(dsmark.Tables[0].Rows[markval]["total"]);
                                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(total);
                                    }
                                    else   //bind grade
                                    {
                                        convertgrade(roll_no, subjectnumber, degreecode, batchyear, examcode);
                                        Fpstudentmark.Sheets[0].Cells[Fpstudentmark.Sheets[0].RowCount - 1, 3].Text = funcgrade;
                                    }
                                }
                            }
                        }
                        mpgetamount.Show();
                    }
                }
                Fpstudentmark.Sheets[0].PageSize = Fpstudentmark.Sheets[0].RowCount;
            }
        }
        if (Fpstudentmark.Sheets[0].RowCount > 0)
        {
            Fpstudentmark.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            Fpstudentmark.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            Fpstudentmark.Width = 615;
            pnlstudemark.Width = 625;
        }
        else
        {
            Fpstudentmark.Width = 600;
            lblnorec.Visible = true;
            lblnorec.Text = "No Marks Assigned for this Student";
        }
    }

    public void convertgrade(string roll, string subj, string degreecode, string batchyear, string IntExamCode)
    {
        string strexam = string.Empty;
        strexam = "Select subject_name,subject_code,total,actual_total,result,cp,mark_entry.subject_no from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + "  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";
        DataSet dssubmarks = d2.select_method_wo_parameter(strexam, "Text");
        //while (dr_convert.Read())
        if (dssubmarks.Tables[0].Rows.Count > 0)
        {
            //   funcsemester = dr_convert["semester"].ToString();
            funcsubname = dssubmarks.Tables[0].Rows[0]["subject_name"].ToString();
            funcsubno = dssubmarks.Tables[0].Rows[0]["subject_no"].ToString();
            funcsubcode = dssubmarks.Tables[0].Rows[0]["subject_code"].ToString();
            funcresult = dssubmarks.Tables[0].Rows[0]["result"].ToString();
            funccredit = dssubmarks.Tables[0].Rows[0]["cp"].ToString();
            mark = dssubmarks.Tables[0].Rows[0]["total"].ToString();
            funcgrade = string.Empty;
            string strgrade = string.Empty;
            //if (dr_convert["total"].ToString() != string.Empty)
            if (mark != string.Empty)
            {
                strgrade = "select mark_grade from grade_master where degree_code=" + degreecode + " and batch_year=" + batchyear + " and college_code=" + Session["collegecode"] + " and " + mark + " between frange and trange";
            }
            else
            {
                strgrade = "select mark_grade from grade_master where degree_code=" + degreecode + " and batch_year=" + batchyear + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
            }
            string getstrgrade = d2.GetFunction(strgrade);
            if (getstrgrade.Trim() != "")
            {
                funcgrade = getstrgrade;
            }
            else
            {
                funcgrade = string.Empty;
            }
        }
    }

    protected void btnexitpanel_Click(object sender, EventArgs e)
    {
        mpgetamount.Hide();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        string appPath = HttpContext.Current.Server.MapPath("~");
        string print = string.Empty;
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                if (txtreptname.Text == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter the Report Name";
                }
                else
                {
                    errmsg.Visible = false;
                    errmsg.Text = string.Empty;
                    string reportname = txtreptname.Text;
                    d2.printexcelreport(FpSpread1, reportname);
                }
            }
            catch
            {
                i++;
                goto e;
            }
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdover.Checked == true)
            {
                lblreptname.Visible = true;
                txtreptname.Visible = true;
                int i = 1;
                int j = 0;
                int minus = 0;
                int addcount = 1;
                double pagecount = 0;
                int rowcount = FpSpread1.Sheets[0].RowCount;
                int columcount = FpSpread1.Sheets[0].ColumnCount;
                columcount = columcount - 1;
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                Font fontmedium = new Font("Book Antique", 10, FontStyle.Regular);
                Font fonthead = new Font("Book Antique", 12, FontStyle.Bold);
                Font fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
                string query = "Select * from collinfo where college_code='" + collegecode + "'";
                DataSet dsinfo = new DataSet();
                dsinfo = d2.select_method_wo_parameter(query, "Text");
                string collname = dsinfo.Tables[0].Rows[0]["collname"].ToString();
                string address = dsinfo.Tables[0].Rows[0]["address1"].ToString() + "-" + dsinfo.Tables[0].Rows[0]["address2"].ToString() + dsinfo.Tables[0].Rows[0]["district"].ToString();
                string phone = "Phone" + " : " + dsinfo.Tables[0].Rows[0]["phoneno"].ToString() + " " + "Fax" + " : " + dsinfo.Tables[0].Rows[0]["faxno"].ToString();
                string email = "E-Mail" + " : " + dsinfo.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site" + " : " + dsinfo.Tables[0].Rows[0]["website"].ToString();
                PdfTextArea ptc = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                PdfTextArea ptc1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                PdfTextArea ptc2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                PdfTextArea ptc3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                PdfTextArea ptc4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                mypdfpage.Add(ptc);
                mypdfpage.Add(ptc1);
                mypdfpage.Add(ptc2);
                mypdfpage.Add(ptc3);
                mypdfpage.Add(ptc4);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 25, 25, 350);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 485, 25, 300);
                }
                Gios.Pdf.PdfTable table;
                int row = 1;
                if (rowcount > 11)
                {
                    table = mydocument.NewTable(fontsmall, 11, columcount, 5);
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    int check = 0;
                    while (rowcount > 11)
                    {
                        string value = string.Empty;
                        if (check != 0)
                        {
                            Gios.Pdf.PdfTablePage pdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                            mypdfpage.Add(pdftable);
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydocument.NewPage();
                            table = mydocument.NewTable(fontsmall, 11, 9, 5);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            PdfTextArea pt3c = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                            PdfTextArea pt3c1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                            PdfTextArea pt3c2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                            PdfTextArea pt3c3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                            PdfTextArea pt3c4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                            mypdfpage.Add(pt3c);
                            mypdfpage.Add(pt3c1);
                            mypdfpage.Add(pt3c2);
                            mypdfpage.Add(pt3c3);
                            mypdfpage.Add(pt3c4);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, 25, 350);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 485, 25, 300);
                            }
                        }
                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 0).SetContent("S.No");
                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 1).SetContent("Roll No");
                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 2).SetContent("Reg No");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Student Name");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Student Name");
                        table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 4).SetContent("Batch");
                        table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 5).SetContent("Branch");
                        table.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 6).SetContent("CGPA");
                        table.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 7).SetContent("Classification");
                        table.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 8).SetContent("Total Marks");
                        table.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 9).SetContent("Rank");
                        table.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 10).SetContent("Photo");
                        table.Columns[0].SetWidth(10);
                        table.Columns[1].SetWidth(40);
                        table.Columns[2].SetWidth(60);
                        table.Columns[3].SetWidth(60);
                        table.Columns[4].SetWidth(20);
                        table.Columns[5].SetWidth(60);
                        table.Columns[6].SetWidth(25);
                        table.Columns[7].SetWidth(40);
                        table.Columns[8].SetWidth(20);
                        table.Columns[9].SetWidth(20);
                        table.Columns[10].SetWidth(50);
                        table.VisibleHeaders = false;
                        rowcount = rowcount - 11;
                        minus = rowcount;
                        int colval = 180;
                        for (i = 1; i < 11; i++)
                        {
                            for (j = 0; j < columcount; j++)
                            {
                                value = FpSpread1.Sheets[0].Cells[row, j].Text;
                                table.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                                table.Cell(i, j).SetContent(value);
                                table.Cell(i, j).SetCellPadding(17);
                                string regn = string.Empty;
                                if (regn == "")
                                {
                                    if (j == 1)
                                    {
                                        regn = FpSpread1.Sheets[0].Cells[row, 1].Text;
                                        MemoryStream memoryStream = new MemoryStream();
                                        DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                        if (dsstuphoto.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                            {
                                                byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                    {
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    }
                                                }
                                                memoryStream.Dispose();
                                                memoryStream.Close();
                                            }
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                        {
                                            table.Cell(addcount - 1 + i, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                            mypdfpage.Add(leftimage, 510, colval, 50);
                                        }
                                        colval = colval + 50;
                                    }
                                }
                            }
                            row++;
                        }
                        check++;
                    }
                    addcount = row;
                    int final = FpSpread1.Sheets[0].RowCount - addcount;
                    pagecount++;
                    table.VisibleHeaders = false;
                    Gios.Pdf.PdfTablePage mainpdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydocument.NewPage();
                    Gios.Pdf.PdfTable table1 = mydocument.NewTable(fontsmall, final + 1, columcount, 5);
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    if (rowcount < 11)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 25, 450);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 485, 25, 450);
                        }
                        PdfTextArea ptc5 = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                        PdfTextArea ptc6 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                        PdfTextArea ptc7 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                        PdfTextArea ptc8 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                        PdfTextArea ptc9 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                        mypdfpage.Add(ptc5);
                        mypdfpage.Add(ptc6);
                        mypdfpage.Add(ptc7);
                        mypdfpage.Add(ptc8);
                        mypdfpage.Add(ptc9);
                        table1.VisibleHeaders = false;
                        table1.Cell(0, 0).SetContent("S.No");
                        table1.Cell(0, 1).SetContent("Roll No");
                        table1.Cell(0, 2).SetContent("Reg No");
                        table1.Cell(0, 3).SetContent("Student Name");
                        table1.Cell(0, 4).SetContent("Batch");
                        table1.Cell(0, 5).SetContent("Branch");
                        table1.Cell(0, 6).SetContent("CGPA");
                        table1.Cell(0, 7).SetContent("Classification");
                        table1.Cell(0, 8).SetContent("Total Marks");
                        table1.Cell(0, 9).SetContent("Rank");
                        table1.Cell(0, 10).SetContent("Photo");
                        table1.Columns[0].SetWidth(10);
                        table1.Columns[1].SetWidth(40);
                        table1.Columns[2].SetWidth(60);
                        table1.Columns[3].SetWidth(60);
                        table1.Columns[4].SetWidth(20);
                        table1.Columns[5].SetWidth(60);
                        table1.Columns[6].SetWidth(25);
                        table1.Columns[7].SetWidth(40);
                        table1.Columns[8].SetWidth(20);
                        table1.Columns[9].SetWidth(20);
                        table1.Columns[10].SetWidth(50);
                        table1.VisibleHeaders = false;
                        int colval = 180;
                        int col = 1;
                        for (i = 1; i < final + 1; i++)
                        {
                            for (j = 0; j < columcount; j++)
                            {
                                string value = FpSpread1.Sheets[0].Cells[addcount - 1 + i, j].Text;
                                table1.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                                table1.Cell(col, j).SetContent(value);
                                table1.Cell(i, j).SetCellPadding(17);
                                string regn = string.Empty;
                                if (regn == "")
                                {
                                    if (j == 1)
                                    {
                                        regn = FpSpread1.Sheets[0].Cells[addcount - 1 + i, 1].Text;
                                        MemoryStream memoryStream = new MemoryStream();
                                        DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                        if (dsstuphoto.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                            {
                                                byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                    {
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    }
                                                }
                                            }
                                            memoryStream.Dispose();
                                            memoryStream.Close();
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                        {
                                            table1.Cell(col, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                            mypdfpage.Add(leftimage, 510, colval, 50);
                                        }
                                        colval = colval + 50;
                                    }
                                }
                            }
                            col++;
                        }
                    }
                    Gios.Pdf.PdfTablePage mainpdftable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable1);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydocument.NewPage();
                }
                else
                {
                    Gios.Pdf.PdfTable table2 = mydocument.NewTable(fontsmall, rowcount, columcount, 1);
                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 0).SetContent("S.No");
                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 1).SetContent("Roll No");
                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 2).SetContent("Reg No");
                    table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 3).SetContent("Student Name");
                    table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 4).SetContent("Batch");
                    table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 5).SetContent("Branch");
                    table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 6).SetContent("CGPA");
                    table2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 7).SetContent("Classification");
                    table2.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 8).SetContent("Total Marks");
                    table2.Cell(0, 9).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 9).SetContent("Rank");
                    table2.Cell(0, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 10).SetContent("Photo");
                    table2.Columns[0].SetWidth(10);
                    table2.Columns[1].SetWidth(40);
                    table2.Columns[2].SetWidth(60);
                    table2.Columns[3].SetWidth(60);
                    table2.Columns[4].SetWidth(20);
                    table2.Columns[5].SetWidth(60);
                    table2.Columns[6].SetWidth(25);
                    table2.Columns[7].SetWidth(40);
                    table2.Columns[8].SetWidth(20);
                    table2.Columns[9].SetWidth(20);
                    table2.Columns[10].SetWidth(50);
                    table2.VisibleHeaders = false;
                    int colval = 170;
                    for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        for (j = 0; j < columcount; j++)
                        {
                            string value = FpSpread1.Sheets[0].Cells[i, j].Text;
                            table2.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                            table2.Cell(i, j).SetContent(value);
                            table2.Cell(i, j).SetCellPadding(17);
                            string regn = string.Empty;
                            if (regn == "")
                            {
                                if (j == 1)
                                {
                                    regn = FpSpread1.Sheets[0].Cells[i, 1].Text;
                                    MemoryStream memoryStream = new MemoryStream();
                                    DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regn + "')", "Text");
                                    if (dsstuphoto.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                        {
                                            if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"] != "")
                                            {
                                                byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")) == false)
                                                    {
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    }
                                                }
                                            }
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg")))
                                    {
                                        table2.Cell(i, 10).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regn + ".jpeg"));
                                        mypdfpage.Add(leftimage, 510, colval, 50);
                                    }
                                    colval = colval + 45;
                                }
                            }
                        }
                    }
                    Gios.Pdf.PdfTablePage mainpdftable5 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable5);
                    mypdfpage.SaveToDocument();
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Studenttopperlist" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
            else
            {
                int i = 1;
                int j = 0;
                int minus = 0;
                int addcount = 1;
                double pagecount = 0;
                int rowcount = FpSpread1.Sheets[0].RowCount;
                int columcount = FpSpread1.Sheets[0].ColumnCount;
                columcount = columcount - 1;
                Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
                Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
                Font fontmedium = new Font("Book Antique", 10, FontStyle.Regular);
                Font fonthead = new Font("Book Antique", 12, FontStyle.Bold);
                Font fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
                string query = "Select * from collinfo where college_code='" + collegecode + "'";
                DataSet dsinfo = new DataSet();
                dsinfo = d2.select_method_wo_parameter(query, "Text");
                string collname = dsinfo.Tables[0].Rows[0]["collname"].ToString();
                string address = dsinfo.Tables[0].Rows[0]["address1"].ToString() + "-" + dsinfo.Tables[0].Rows[0]["address2"].ToString() + dsinfo.Tables[0].Rows[0]["district"].ToString();
                string phone = "Phone" + " : " + dsinfo.Tables[0].Rows[0]["phoneno"].ToString() + " " + "Fax" + " : " + dsinfo.Tables[0].Rows[0]["faxno"].ToString();
                string email = "E-Mail" + " : " + dsinfo.Tables[0].Rows[0]["email"].ToString() + " " + "Web Site" + " : " + dsinfo.Tables[0].Rows[0]["website"].ToString();
                PdfTextArea ptc = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                        new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                PdfTextArea ptc1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                   new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                PdfTextArea ptc2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                PdfTextArea ptc3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                PdfTextArea ptc4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                  new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                mypdfpage.Add(ptc);
                mypdfpage.Add(ptc1);
                mypdfpage.Add(ptc2);
                mypdfpage.Add(ptc3);
                mypdfpage.Add(ptc4);
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 25, 25, 350);
                }
                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                {
                    PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                    mypdfpage.Add(LogoImage, 485, 25, 300);
                }
                Gios.Pdf.PdfTable table;
                int row = 1;
                if (rowcount > 12)
                {
                    table = mydocument.NewTable(fontsmall, 12, columcount, 5);
                    table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    int check = 0;
                    while (rowcount > 12)
                    {
                        string value = string.Empty;
                        if (check != 0)
                        {
                            Gios.Pdf.PdfTablePage pdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                            mypdfpage.Add(pdftable);
                            mypdfpage.SaveToDocument();
                            mypdfpage = mydocument.NewPage();
                            table = mydocument.NewTable(fontsmall, 12, 9, 5);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            PdfTextArea pt3c = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                      new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                            PdfTextArea pt3c1 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                               new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                            PdfTextArea pt3c2 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                            PdfTextArea pt3c3 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                            PdfTextArea pt3c4 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                            mypdfpage.Add(pt3c);
                            mypdfpage.Add(pt3c1);
                            mypdfpage.Add(pt3c2);
                            mypdfpage.Add(pt3c3);
                            mypdfpage.Add(pt3c4);
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 25, 25, 350);
                            }
                            if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                            {
                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                                mypdfpage.Add(LogoImage, 485, 25, 300);
                            }
                        }
                        table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 0).SetContent("S.No");
                        table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 1).SetContent("Roll No");
                        table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 2).SetContent("Reg No");
                        table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 3).SetContent("Student Name");
                        table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 4).SetContent("CGPA");
                        table.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 5).SetContent("Classification");
                        table.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 6).SetContent("Total Marks");
                        table.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 7).SetContent("Rank");
                        table.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table.Cell(0, 8).SetContent("Photo");
                        table.Columns[0].SetWidth(20);
                        table.Columns[1].SetWidth(50);
                        table.Columns[2].SetWidth(60);
                        table.Columns[3].SetWidth(100);
                        table.Columns[4].SetWidth(20);
                        table.Columns[5].SetWidth(40);
                        table.Columns[6].SetWidth(50);
                        table.Columns[7].SetWidth(30);
                        table.Columns[8].SetWidth(50);
                        table.VisibleHeaders = false;
                        rowcount = rowcount - 12;
                        minus = rowcount;
                        int colval = 180;
                        for (i = 1; i < 12; i++)
                        {
                            for (j = 0; j < columcount; j++)
                            {
                                value = FpSpread1.Sheets[0].Cells[row, j].Text;
                                int colspan = FpSpread1.Sheets[0].Cells[row, j].ColumnSpan;
                                if (colspan > 1)
                                {
                                    if (i < 12)
                                    {
                                        foreach (PdfCell pr in table.CellRange(i, 0, i, 1).Cells)
                                        {
                                            pr.ColSpan = 8;
                                        }
                                        colval = colval + 35;
                                    }
                                }
                                string regb = string.Empty;
                                table.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                                table.Cell(i, j).SetContent(value);
                                table.Cell(i, j).SetCellPadding(17);
                                if (j < 0 && j >= 3)
                                {
                                    value = FpSpread1.Sheets[0].Cells[row, j].Text;
                                    table.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                                    table.Cell(i, 3).SetContent(value);
                                    table.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                }
                                else
                                {
                                    table.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                }
                                //table.Cell(i, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table.Cell(i, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (regb == "")
                                {
                                    if (j == 2)
                                    {
                                        regb = FpSpread1.Sheets[0].Cells[row, 1].Text;
                                        MemoryStream memoryStream = new MemoryStream();
                                        DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regb + "')", "Text");
                                        if (dsstuphoto.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                            {
                                                byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")) == false)
                                                    {
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    }
                                                }
                                                memoryStream.Dispose();
                                                memoryStream.Close();
                                            }
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")))
                                        {
                                            table.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"));
                                            mypdfpage.Add(leftimage, 510, colval, 50);
                                        }
                                        colval = colval + 52;
                                    }
                                }
                            }
                            row++;
                        }
                        check++;
                    }
                    addcount = row;
                    int final = FpSpread1.Sheets[0].RowCount - addcount;
                    pagecount++;
                    table.VisibleHeaders = false;
                    Gios.Pdf.PdfTablePage mainpdftable = table.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydocument.NewPage();
                    Gios.Pdf.PdfTable table1 = mydocument.NewTable(fontsmall, final + 1, columcount, 5);
                    table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    if (rowcount < 11)
                    {
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 25, 25, 450);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))
                        {
                            PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            mypdfpage.Add(LogoImage, 485, 25, 450);
                        }
                        PdfTextArea ptc5 = new PdfTextArea(fonthead, System.Drawing.Color.Black,
                                                       new PdfArea(mydocument, 50, 20, 500, 50), System.Drawing.ContentAlignment.MiddleCenter, "" + collname + "");
                        PdfTextArea ptc6 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                           new PdfArea(mydocument, 50, 60, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + address + "");
                        PdfTextArea ptc7 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 80, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + phone + "");
                        PdfTextArea ptc8 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 100, 500, 50), System.Drawing.ContentAlignment.TopCenter, "" + email + "");
                        PdfTextArea ptc9 = new PdfTextArea(fontmedium, System.Drawing.Color.Black,
                                                          new PdfArea(mydocument, 50, 120, 500, 50), System.Drawing.ContentAlignment.TopCenter, "Student Rank & Topper List");
                        mypdfpage.Add(ptc5);
                        mypdfpage.Add(ptc6);
                        mypdfpage.Add(ptc7);
                        mypdfpage.Add(ptc8);
                        mypdfpage.Add(ptc9);
                        table1.VisibleHeaders = false;
                        table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 0).SetContent("S.No");
                        table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 1).SetContent("Roll No");
                        table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 2).SetContent("Reg No");
                        table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 3).SetContent("Student Name");
                        table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 4).SetContent("CGPA");
                        table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 5).SetContent("Classification");
                        table1.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 6).SetContent("Total Marks");
                        table1.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 7).SetContent("Rank");
                        table1.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                        table1.Cell(0, 8).SetContent("Photo");
                        table1.Columns[0].SetWidth(20);
                        table1.Columns[1].SetWidth(50);
                        table1.Columns[2].SetWidth(60);
                        table1.Columns[3].SetWidth(80);
                        table1.Columns[4].SetWidth(20);
                        table1.Columns[5].SetWidth(40);
                        table1.Columns[6].SetWidth(50);
                        table1.Columns[7].SetWidth(30);
                        table1.Columns[8].SetWidth(50);
                        table1.VisibleHeaders = false;
                        int colval = 190;
                        int col = 1;
                        for (i = 1; i < final + 1; i++)
                        {
                            for (j = 0; j < columcount; j++)
                            {
                                string value = FpSpread1.Sheets[0].Cells[addcount - 1 + i, j].Text;
                                int colspan = FpSpread1.Sheets[0].Cells[addcount - 1 + i, j].ColumnSpan;
                                table1.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                                table1.Cell(col, j).SetContent(value);
                                table1.Cell(col, j).SetCellPadding(17);
                                if (colspan > 1)
                                {
                                    if (i < 12)
                                    {
                                        foreach (PdfCell pr in table1.CellRange(i, 0, i, 1).Cells)
                                        {
                                            pr.ColSpan = 8;
                                        }
                                        colval = colval + 35;
                                    }
                                }
                                string regb = string.Empty;
                                if (j < 0 && j >= 3)
                                {
                                    table1.Cell(col, j).SetContentAlignment(ContentAlignment.MiddleLeft);
                                }
                                else
                                {
                                    table1.Cell(col, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                                }
                                //table1.Cell(col, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1.Cell(col, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                //table1.Cell(col, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                if (regb == "")
                                {
                                    if (j == 2)
                                    {
                                        regb = FpSpread1.Sheets[0].Cells[addcount - 1 + i, 1].Text;
                                        MemoryStream memoryStream = new MemoryStream();
                                        DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regb + "')", "Text");
                                        if (dsstuphoto.Tables[0].Rows.Count > 0)
                                        {
                                            if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                            {
                                                byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                                memoryStream.Write(file, 0, file.Length);
                                                if (file.Length > 0)
                                                {
                                                    System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                    System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")) == false)
                                                    {
                                                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                    }
                                                }
                                                memoryStream.Dispose();
                                                memoryStream.Close();
                                            }
                                        }
                                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")))
                                        {
                                            table1.Cell(col, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"));
                                            mypdfpage.Add(leftimage, 510, colval, 50);
                                        }
                                        colval = colval + 50;
                                    }
                                }
                            }
                            col++;
                        }
                    }
                    Gios.Pdf.PdfTablePage mainpdftable1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable1);
                    mypdfpage.SaveToDocument();
                    mypdfpage = mydocument.NewPage();
                }
                else
                {
                    Gios.Pdf.PdfTable table2 = mydocument.NewTable(fontsmall, rowcount, columcount, 1);
                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.TopLeft);
                    table2.Cell(0, 0).SetContent("S.No");
                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.TopLeft);
                    table2.Cell(0, 1).SetContent("Roll No");
                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.TopLeft);
                    table2.Cell(0, 2).SetContent("Reg No");
                    table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 3).SetContent("Student Name");
                    table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 4).SetContent("CGPA");
                    table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 5).SetContent("Classification");
                    table2.Cell(0, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 6).SetContent("Total Marks");
                    table2.Cell(0, 7).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 7).SetContent("Rank");
                    table2.Cell(0, 8).SetContentAlignment(ContentAlignment.MiddleCenter);
                    table2.Cell(0, 8).SetContent("Photo");
                    table2.Columns[0].SetWidth(20);
                    table2.Columns[1].SetWidth(50);
                    table2.Columns[2].SetWidth(60);
                    table2.Columns[3].SetWidth(80);
                    table2.Columns[4].SetWidth(20);
                    table2.Columns[5].SetWidth(40);
                    table2.Columns[6].SetWidth(50);
                    table2.Columns[7].SetWidth(30);
                    table2.Columns[8].SetWidth(50);
                    table2.VisibleHeaders = false;
                    int colval = 180;
                    for (i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                    {
                        for (j = 0; j < columcount; j++)
                        {
                            string regb = string.Empty;
                            string value = FpSpread1.Sheets[0].Cells[i, j].Text;
                            table2.CellRange(0, 0, 0, 4).SetFont(fontsmall);
                            table2.Cell(i, j).SetContent(value);
                            table2.Cell(i, j).SetCellPadding(17);
                            if (j < 0 && j >= 3)
                            {
                                table2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleLeft);
                            }
                            else
                            {
                                table2.Cell(i, j).SetContentAlignment(ContentAlignment.MiddleCenter);
                            }
                            int colspan = FpSpread1.Sheets[0].Cells[i, j].ColumnSpan;
                            if (colspan > 1)
                            {
                                if (i < 12)
                                {
                                    foreach (PdfCell pr in table2.CellRange(i, 0, i, 1).Cells)
                                    {
                                        pr.ColSpan = 8;
                                    }
                                    colval = colval + 35;
                                }
                            }
                            if (regb == "")
                            {
                                if (j == 2)
                                {
                                    regb = FpSpread1.Sheets[0].Cells[i, 1].Text;
                                    MemoryStream memoryStream = new MemoryStream();
                                    DataSet dsstuphoto = d2.select_method_wo_parameter("select photo from stdphoto where app_no in(select app_no from registration where roll_no='" + regb + "')", "Text");
                                    if (dsstuphoto.Tables[0].Rows.Count > 0)
                                    {
                                        if (dsstuphoto.Tables[0].Rows[0]["photo"] != null && dsstuphoto.Tables[0].Rows[0]["photo"].ToString().Trim() != "")
                                        {
                                            byte[] file = (byte[])dsstuphoto.Tables[0].Rows[0]["photo"];
                                            memoryStream.Write(file, 0, file.Length);
                                            if (file.Length > 0)
                                            {
                                                System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                                                System.Drawing.Image thumb = imgx.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                                                if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")) == false)
                                                {
                                                    thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                                                }
                                            }
                                        }
                                        memoryStream.Dispose();
                                        memoryStream.Close();
                                    }
                                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg")))
                                    {
                                        table2.Cell(i, 6).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        PdfImage leftimage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/college/" + regb + ".jpeg"));
                                        mypdfpage.Add(leftimage, 510, colval, 50);
                                    }
                                    colval = colval + 50;
                                }
                            }
                        }
                    }
                    Gios.Pdf.PdfTablePage mainpdftable5 = table2.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 150, 550, 950));
                    mypdfpage.Add(mainpdftable5);
                    mypdfpage.SaveToDocument();
                }
                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Studenttopperlist" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public string Calculete_CGPA(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        double total = 0;
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = string.Empty;
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            double gpacal1 = 0;
            string strsubcrd = string.Empty;
            int gtempejval = 0;
            string syll_code = string.Empty;
            string examcodevalg = string.Empty;
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = string.Empty;
            string strtotgrac = string.Empty;
            string sqlcmdgraderstotal = string.Empty;
            int attemptswith = 0;
            string strattmaxmark = string.Empty;
            int attmpt = 0, maxmark = 0;
            strattmaxmark = d2.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
            string[] semecount = strattmaxmark.Split(new Char[] { '-' });
            if (semecount.GetUpperBound(0) == 1)
            {
                attmpt = Convert.ToInt32(semecount[0].ToString());
                maxmark = Convert.ToInt32(semecount[1].ToString());
                flag = true;
            }
            else
            {
                flag = false;
            }
            sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");
            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' ";
            if (strsubcrd != null && strsubcrd != "")
            {
                DataSet dssubmark = d2.select_method_wo_parameter(strsubcrd, "text");
                for (int s = 0; s < dssubmark.Tables[0].Rows.Count; s++)
                {
                    if ((dssubmark.Tables[0].Rows[s]["total"].ToString() != string.Empty) && (dssubmark.Tables[0].Rows[s]["total"].ToString() != "0"))
                    {
                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtot = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["total"].ToString());
                            inte = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["internal_mark"].ToString());
                            exte = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["external_mark"].ToString());
                            attemptswith = Convert.ToInt32(dssubmark.Tables[0].Rows[s]["attempts"].ToString());
                            total = Convert.ToDouble(strtot) + Convert.ToDouble(total);
                            if (flag == true)
                            {
                                if (attmpt > attemptswith)//ATTEMPTS compared with attempts in coe settings if attempts lower than coe settings
                                {
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    inte = 0;
                                    strtot = exte;// total only consider extermarks only
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                strtot = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["total"].ToString());
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                        if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if ((dssubmark.Tables[0].Rows[s]["grade"].ToString() != string.Empty))
                    {
                        if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        {
                            strtotgrac = Convert.ToString(dssubmark.Tables[0].Rows[s]["grade"].ToString());
                            foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                            {
                                strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                    creditval = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    if (creditsum1 == 0)
                    {
                        creditsum1 = Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    }
                    else
                    {
                        creditsum1 = creditsum1 + Convert.ToDouble(dssubmark.Tables[0].Rows[s]["credit_points"].ToString());
                    }
                    if (gpacal1 == 0)
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                    }
                    else
                    {
                        if (strgrade != "")
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }
            creditval = 0;
            strgrade = string.Empty;
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            calculate = Convert.ToString(finalgpa1);
            creditsum1 = 0;
            gpacal1 = 0;
            finalgpa1 = 0;
            total1 = Convert.ToDouble(total);
        }
        catch (Exception vel)
        {
            string exce = vel.ToString();
        }
        if (calculate == "NaN")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }

    public void Calulat_GPA_Semwise(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = string.Empty;
        string strgrade = string.Empty;
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = string.Empty;
        string examcodeval = string.Empty;
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = string.Empty;
        string strgradetempgrade = string.Empty;
        string syll_code = string.Empty;
        DataSet dggradetot = new DataSet();
        try
        {
            dggradetot.Dispose();
            daload.Reset();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }
        syll_code = d2.GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = d2.GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        if (strsubcrd != "" && strsubcrd != null)
        {
            DataSet dssubgrd = d2.select_method_wo_parameter(strsubcrd, "Text");
            for (int s = 0; s < dssubgrd.Tables[0].Rows.Count; s++)
            {
                if ((dssubgrd.Tables[0].Rows[s]["total"].ToString() != string.Empty) && (dssubgrd.Tables[0].Rows[s]["total"].ToString() != "0"))
                {
                    if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                    {
                        strtot = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["total"].ToString());
                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                        {
                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                            {
                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                {
                                    strgrade = gratemp["credit_points"].ToString();
                                    break;
                                }
                            }
                        }
                    }
                }
                else if ((dssubgrd.Tables[0].Rows[s]["grade"].ToString() != string.Empty))
                {
                    if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                    {
                        strtotgrac = Convert.ToString(dssubgrd.Tables[0].Rows[s]["grade"].ToString());
                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                        {
                            strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                            if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                            {
                                strgrade = gratemp["credit_points"].ToString();
                                break;
                            }
                        }
                    }
                }
                if (strgrade != "" && strgrade != null)
                {
                    if (dssubgrd.Tables[0].Rows[s]["credit_points"].ToString() != null && dssubgrd.Tables[0].Rows[s]["credit_points"].ToString() != "")
                    {
                        creditval = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dssubgrd.Tables[0].Rows[s]["credit_points"].ToString());
                        }
                    }
                    if (gpacal1 == 0)
                    {
                        gpacal1 = Convert.ToDouble(strgrade) * creditval;
                    }
                    else
                    {
                        gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        calculate = finalgpa1.ToString();
        total1 = Math.Round(total1, 0, MidpointRounding.AwayFromZero);
    }

}
