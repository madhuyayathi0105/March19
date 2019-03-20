using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Text;

public partial class CRSettings : System.Web.UI.Page
{
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string StreamShift = string.Empty;

    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();

    DAccess2 queryObject = new DAccess2();
    DAccess2 da = new DAccess2();
    SqlConnection ssql = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
                usercode = Session["group_code"].ToString();
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
                usercode = Session["usercode"].ToString();
            }
            if (!IsPostBack)
            {
                settext();
                bindclg();
                loadDefault(sender, e);
            }

            if (ddl_college.Items.Count > 0)
            {
                collegecode = ddl_college.SelectedItem.Value.ToString();
                collegecode1 = ddl_college.SelectedItem.Value.ToString();
            }
            else collegecode1 = "0";
            if (rbl_ReceiptFormat.SelectedItem.Text == "Format14")//abarna
            {
                print.Visible = true;
                printformat.Visible = true;
            }
            else
            {
                print.Visible = false;
                printformat.Visible = false;
            }
            if (printformat.SelectedItem.Text == "10*6")//abarna
            {
                txtheight.Visible = true;
            }
            else
            {
                txtheight.Visible = false;
            }
        }
        catch (Exception ex) { }
    }
    private void settext()
    {
        if (checkSchoolSetting() == 0)
        {
            cb_colgname.Text = "School Name";
            cb_degree.Text = "School TypeAcr";
            cb_sem.Text = "Term";
            cb_degname.Text = "School TypeName";
            cb_CommonClgname.Text = "Common School Name";
        }
        else
        {
            cb_colgname.Text = "College Name";
            cb_degree.Text = "College Acronym";
            cb_sem.Text = "Semester";
            cb_degname.Text = "Degree Name";
            cb_CommonClgname.Text = "Common College Name";
        }
    }
    public void loadDefault(object sender, EventArgs e)
    {
        try
        {
            rdo_receipt_OnCheckedChanged(sender, e);
            loadTitle();
            txt_valid.Attributes.Add("readonly", "readonly");
            txt_valid.Text = DateTime.Now.ToString("dd/MM/yyyy");
            collegebank();

            if (ddl_college.Items.Count > 0)
            {
                collegecode1 = ddl_college.SelectedItem.Value.ToString();
            }
            defaultSetting();
            try
            {
                StreamShift = Convert.ToString(Session["streamcode"]);
                if (StreamShift.Trim() == "")
                {
                    StreamShift = "Stream";
                }
            }
            catch { StreamShift = "Stream"; }
            lbl_stream.Text = StreamShift;
            lbl_strm.Text = StreamShift;
            lbl_disp.Text = "Display " + StreamShift;
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void lb2_Click(object sender, EventArgs e)
    {
        try
        {
            Session.Abandon();
            Session.Clear();
            Session.RemoveAll();
            System.Web.Security.FormsAuthentication.SignOut();
            Response.Redirect("default.aspx", false);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    //**********************************************//
    //   Code Started by Idhris from 16/10/2015     //
    //**********************************************//
    public void bindclg()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddl_college.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void lbtn_hdrsettings_Click(object sender, EventArgs e)
    {
        try
        {
            pop_hdrsettings.Visible = true;
            loadHeaderNamePop();
            loadStreamPop();
            loadDispStream();
            loaddesc1();
            txt_dispstream.Text = "";
            txt_grphdr.Text = "";
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btn_exit_Click(object sender, EventArgs e)
    {
    }
    protected void btn_exit1_Click(object sender, EventArgs e)
    {
    }
    protected void ddl_college_OnselectChange(object sender, EventArgs e)
    {
        try
        {
            loadDefault(sender, e);
            rdo_challan_OnCheckedChanged(sender, e);
            cb_GHwise.Checked = false;
            cb_Degwise.Checked = false;
            cb_GHwise_OncheckedChanged(sender, e);
            cb_Degwise_OncheckedChanged(sender, e);
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void ddl_grphdr_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    public void loaddesc1()
    {
        try
        {
            ddl_title.Items.Clear();
            ds.Tables.Clear();

            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='ChHed' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_title.DataSource = ds;
                ddl_title.DataTextField = "TextVal";
                ddl_title.DataValueField = "TextCode";
                ddl_title.DataBind();
            }

        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        pop_hdrsettings.Visible = false;
    }
    public void rdo_receipt_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdo_receipt.Checked == true)
        {
            div_receipt.Visible = true;
            div_challan.Visible = false;

        }
        else if (rdo_receipt.Checked == false)
        {
            div_challan.Visible = true;
            div_receipt.Visible = false;
        }
    }
    public void rdo_challan_OnCheckedChanged(object sender, EventArgs e)
    {
        if (rdo_challan.Checked == true)
        {
            div_challan.Visible = true;
            div_receipt.Visible = false;
            loadTitle();
            loadHeaderName();
            bindType();
            binddegree2();
            binddegree();
        }
        else if (rdo_challan.Checked == false)
        {
            div_challan.Visible = false;
            div_receipt.Visible = true;
        }
    }
    public void loadTitle()
    {
        try
        {
            ddl_title.Items.Clear();
            string query = " select Distinct TextVal,TextCode from TextValTable where TextCriteria ='ChHed' and college_code ='" + collegecode1 + "' order by TextVal asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_title.DataSource = ds;
                    ddl_title.DataTextField = "TextVal";
                    ddl_title.DataValueField = "TextCode";
                    ddl_title.DataBind();
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void loadHeaderName()
    {
        try
        {
            lb_selecthdr.Items.Clear();
            //string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "   ";
            string query = " SELECT distinct G.ChlGroupHeader FROM FS_ChlGroupHeaderSettings G,FS_HeaderPrivilage P WHERE G.HeaderFK = P.HeaderFK AND P. UserCode = '" + usercode + "'  AND P.CollegeCode = " + collegecode1 + " ";

            DataSet dsHeader = new DataSet();
            ds.Clear();
            dsHeader = d2.select_method_wo_parameter(query, "Text");
            if (dsHeader.Tables[0].Rows.Count > 0)
            {
                lb_selecthdr.DataSource = dsHeader;
                lb_selecthdr.DataTextField = "ChlGroupHeader";
                lb_selecthdr.DataValueField = "ChlGroupHeader";
                lb_selecthdr.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void loadHeaderNamePop()
    {
        try
        {
            cb_hdrname.Checked = false;
            cbl_hdrname.Items.Clear();
            txt_hdrname.Text = "--Select--";
            string query = " SELECT HeaderPK,HeaderName FROM FM_HeaderMaster H,FS_HeaderPrivilage P WHERE H.HeaderPK = P.HeaderFK AND P.CollegeCode = H.CollegeCode AND P. UserCode = " + usercode + " AND H.CollegeCode = " + collegecode1 + "  ";
            DataSet dsHeader = new DataSet();
            ds.Clear();
            dsHeader = d2.select_method_wo_parameter(query, "Text");
            if (dsHeader.Tables[0].Rows.Count > 0)
            {
                cbl_hdrname.DataSource = dsHeader;
                cbl_hdrname.DataTextField = "HeaderName";
                cbl_hdrname.DataValueField = "HeaderPK";
                cbl_hdrname.DataBind();
                if (cbl_hdrname.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_hdrname.Items.Count; i++)
                    {
                        cbl_hdrname.Items[i].Selected = true;
                    }
                    txt_hdrname.Text = "Header Name(" + cbl_hdrname.Items.Count + ")";
                    cb_hdrname.Checked = true;
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            if (cbl_strm.Items.Count > 0)
            {
                for (int i = 0; i < cbl_strm.Items.Count; i++)
                {
                    if (cbl_strm.Items[i].Selected == true)
                    {
                        if (stream == "")
                        {
                            stream = Convert.ToString(cbl_strm.Items[i].Value);
                        }
                        else
                        {
                            stream = stream + "'" + "," + "'" + Convert.ToString(cbl_strm.Items[i].Value);
                        }
                    }
                }
            }
            txt_degree2.Text = "--Select--";

            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            if (stream.Trim() != "")
            {
                query += " and course.type in ('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
                    cb_degree2.Checked = true;
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void binddegree()
    {
        try
        {
            cbl_dept.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (branch.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();



                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
                else
                {
                    txt_dept.Text = "--Select--";
                }
            }
            else
            {
                txt_dept.Text = "--Select--";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void bindType()
    {
        try
        {
            cbl_strm.Items.Clear();
            cb_strm.Checked = false;
            txt_strm.Text = "--Select--";
            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and isnull(type,'')<>'' order by type asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_strm.DataSource = ds;
                cbl_strm.DataTextField = "type";
                cbl_strm.DataValueField = "type";
                cbl_strm.DataBind();
                if (cbl_strm.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_strm.Items.Count; i++)
                    {
                        cbl_strm.Items[i].Selected = true;
                    }
                    txt_strm.Text = "Stream(" + cbl_strm.Items.Count + ")";
                    cb_strm.Checked = true;
                }
            }
            if (cbl_strm.Items.Count == 0)
            {
                txt_strm.Enabled = false;
            }
            else
            {
                txt_strm.Enabled = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void loadStreamPop()
    {
        try
        {
            cbl_strm1.Items.Clear();

            string query = "select Distinct ISNULL( type,'') as type  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and isnull(type,'')<>'' order by type asc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_strm1.DataSource = ds;
                cbl_strm1.DataTextField = "type";
                cbl_strm1.DataValueField = "type";
                cbl_strm1.DataBind();
                for (int i = 0; i < cbl_strm1.Items.Count; i++)
                {
                    cbl_strm1.Items[i].Selected = true;

                }

                txt_strm1.Text = "Stream(" + cbl_strm1.Items.Count + ")";
                cb_strm1.Checked = true;

            }
            if (cbl_strm1.Items.Count == 0)
            {
                txt_strm1.Enabled = false;
                txt_dispstream.Enabled = false;
                txt_strm1.Text = "";
                txt_dispstream.Text = "";
            }
            else
            {
                txt_strm1.Enabled = true;
                txt_dispstream.Enabled = true;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void loadDispStream()
    {
        // try
        //{
        //    ddl_disp.Items.Clear();

        //    string query = "select distinct ISNULL(DispStream,'') as DispStream from ChlHeaderSettings where DispStream <>' ' order by DispStream asc";
        //    ds.Clear();
        //    ds = d2.select_method_wo_parameter(query, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddl_disp.DataSource = ds;
        //        ddl_disp.DataTextField = "DispStream";
        //        ddl_disp.DataValueField = "DispStream";
        //        ddl_disp.DataBind();

        //    }
        //}
        //catch
        //{
        //}

    }
    protected void cb_hdrname_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_hdrname, cb_hdrname, txt_hdrname, "Header");
    }
    protected void cbl_hdrname_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_hdrname, cb_hdrname, txt_hdrname, "Header");
    }
    protected void cb_strm_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        binddegree();
    }
    protected void cbl_strm_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_strm, cb_strm, txt_strm, lbl_stream.Text);
        binddegree2();
        binddegree();
    }
    protected void cb_strm1_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_strm1, cb_strm1, txt_strm1, lbl_strm.Text);
    }
    protected void cbl_strm1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_strm1, cb_strm1, txt_strm1, lbl_strm.Text);
    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        binddegree();
    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        binddegree();
    }
    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_dept, cb_dept, txt_dept, "Department");
    }
    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_dept, cb_dept, txt_dept, "Department");
    }
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selecthdr.Items.Count > 0 && lb_selecthdr.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_hdr.Items.Count; j++)
                {
                    if (lb_hdr.Items[j].Value == lb_selecthdr.SelectedItem.Value)
                    {
                        ok = false;
                    }

                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selecthdr.SelectedItem.Text, lb_selecthdr.SelectedItem.Value);
                    lb_hdr.Items.Add(lst);
                }

            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_hdr.Items.Clear();
            if (lb_selecthdr.Items.Count > 0)
            {
                for (int j = 0; j < lb_selecthdr.Items.Count; j++)
                {
                    ListItem lst = new ListItem(lb_selecthdr.Items[j].Text.ToString(), lb_selecthdr.Items[j].Value.ToString());
                    lb_hdr.Items.Add(lst);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_hdr.Items.Count > 0 && lb_hdr.SelectedItem.Value != "")
            {
                lb_hdr.Items.RemoveAt(lb_hdr.SelectedIndex);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_hdr.Items.Clear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    //TITLE
    protected void btnplus1_OnClick(object sender, EventArgs e)
    {
        imgdiv3.Visible = true;
        panel_description.Visible = true;
    }
    protected void btnminus1_OnClick(object sender, EventArgs e)
    {
        if (ddl_title.Items.Count > 0)
        {
            surediv.Visible = true;
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "No Title Selected";
        }
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        try
        {
            surediv.Visible = false;
            if (ddl_title.Items.Count > 0)
            {
                string sql = "delete from textvaltable where TextCode='" + ddl_title.SelectedItem.Value.ToString() + "' and TextCriteria='ChHed' and college_code='" + collegecode1 + "' ";
                int delete = d2.update_method_wo_parameter(sql, "TEXT");
                if (delete != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Deleted Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not deleted";
                }
                loadTitle();
            }

            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "No Title Selected";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btndescpopadd_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_description11.Text != "")
            {
                string sql = "if exists ( select * from TextValTable where TextVal ='" + txt_description11.Text + "' and TextCriteria ='ChHed' and college_code ='" + collegecode1 + "') update TextValTable set TextVal ='" + txt_description11.Text + "' where TextVal ='" + txt_description11.Text + "' and TextCriteria ='ChHed' and college_code ='" + collegecode1 + "' else insert into TextValTable (TextVal,TextCriteria,college_code) values ('" + txt_description11.Text + "','ChHed','" + collegecode1 + "')";
                int insert = d2.update_method_wo_parameter(sql, "TEXT");
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved sucessfully";
                    txt_description11.Text = "";
                    imgdiv3.Visible = false;
                    panel_description.Visible = false;
                }
                loaddesc1();
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Enter the description";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btndescpopexit_Click(object sender, EventArgs e)
    {
        imgdiv3.Visible = false;
        panel_description.Visible = false;
        loaddesc1();
    }
    //Challan Print Settings
    protected void ImageButtonChal_Click(object sender, EventArgs e)
    {
        divChlanPrintSet.Visible = false;
    }
    protected void lnkChlPageSet_Click(object sender, EventArgs e)
    {
        try
        {
            divChlanPrintSet.Visible = true;

            int save1 = 0;
            string insqry1 = string.Empty;
            //Fee Counter for Challan
            insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
            if (insqry1.Trim() == "0")
                insqry1 = "";
            txt_ChlCounter.Text = insqry1.Trim();

            //Challan Particular
            insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
            if (insqry1.Trim() == "0")
                insqry1 = "";
            txt_ChlParticulars.Text = insqry1.Trim();

            //Challan Office Footer MCC
            insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ChallanOfficeFooter' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
            if (insqry1.Trim() == "0")
                insqry1 = "";
            txtChallanOfficeFooter.Text = insqry1.Trim();

            //Load Titles for Hiding Institute address --- Mcc School
            loaddescSet();
            insqry1 = "";
            if (ddl_title1.Items.Count > 0)
            {
                insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='HideInstituteAddressInChallan" + ddl_title1.SelectedValue.Trim() + "' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
                if (insqry1.Trim() == "0")
                    insqry1 = "";
            }
            txtInstituteHideValue.Text = insqry1.Trim();

            //TermDisplay
            save1 = 0;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayTermForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 0)
            {
                cblTermDisp.Checked = false;
            }
            else
            {
                cblTermDisp.Checked = true;
            }

            //IFSCDisplay
            save1 = 0;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 0)
            {
                chkUseIfsc.Checked = false;
            }
            else
            {
                chkUseIfsc.Checked = true;
            }

            //ACR Display
            save1 = 0;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 0)
            {
                chkChlanDegAcr.Checked = false;
            }
            else
            {
                chkChlanDegAcr.Checked = true;
            }
            //Ledger wise amount display in challan 

            save1 = 0;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='ShowLedgerwiseFeesinChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 0)
            {
                cbShowledgerwise.Checked = false;
            }
            else
            {
                cbShowledgerwise.Checked = true;
            }

            //Smartcard Number Display
            save1 = 0;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            rblSmartNodisplay.SelectedIndex = save1;

            //Display Denomination in Challan
            save1 = 0;
            cblDenom.Items[0].Selected = false;
            cblDenom.Items[1].Selected = false;
            cblDenom.Items[2].Selected = false;

            insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //College
                cblDenom.Items[0].Selected = true;
            }
            if (save1 == 2)
            {
                //Bank
                cblDenom.Items[1].Selected = true;
            }
            if (save1 == 3)
            {
                //Student
                cblDenom.Items[2].Selected = true;

            }
            if (save1 == 4)
            {
                //All

                cblDenom.Items[0].Selected = true;
                cblDenom.Items[1].Selected = true;
                cblDenom.Items[2].Selected = true;
            }
            if (save1 == 5)
            {
                //College and Bank
                cblDenom.Items[0].Selected = true;
                cblDenom.Items[1].Selected = true;
            }
            if (save1 == 6)
            {
                //Student and Bank                        
                cblDenom.Items[1].Selected = true;
                cblDenom.Items[2].Selected = true;
            }
            if (save1 == 7)
            {
                //College and Student
                cblDenom.Items[0].Selected = true;
                cblDenom.Items[2].Selected = true;
            }


            //Academic Year
            ddlacefromyear.Items.Clear();
            string bindbatchquery = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year";
            DataSet dsbatch = d2.select_method_wo_parameter(bindbatchquery, "Text");
            if (dsbatch.Tables.Count > 0 && dsbatch.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < dsbatch.Tables[0].Rows.Count; b++)
                {
                    ddlacefromyear.Items.Add(new System.Web.UI.WebControls.ListItem(dsbatch.Tables[0].Rows[b]["batch_year"].ToString(), dsbatch.Tables[0].Rows[b]["batch_year"].ToString()));

                }
                int yr = DateTime.Now.Year;
                int.TryParse(ddlacefromyear.Items[ddlacefromyear.Items.Count - 1].Value, out yr);
                if (yr != DateTime.Now.Year && yr != 0)
                {
                    for (int y = (yr + 1); y <= DateTime.Now.Year; y++)
                    {
                        ddlacefromyear.Items.Add(new System.Web.UI.WebControls.ListItem(y.ToString(), y.ToString()));
                    }
                }
                loadToYear();
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            string resul = d2.GetFunction(insqry1).Trim();

            if (resul != "0")
            {
                ddlacefromyear.SelectedIndex = ddlacefromyear.Items.IndexOf(ddlacefromyear.Items.FindByText(DateTime.Now.Year.ToString()));
                ddlacetoyear.SelectedIndex = ddlacetoyear.Items.IndexOf(ddlacetoyear.Items.FindByText((DateTime.Now.Year + 1).ToString()));
            }
            else
            {
                string[] acaYr = resul.Split(',');
                ddlacefromyear.SelectedIndex = ddlacefromyear.Items.IndexOf(ddlacefromyear.Items.FindByText(acaYr[0]));
                if (acaYr.Length > 1)
                    ddlacetoyear.SelectedIndex = ddlacetoyear.Items.IndexOf(ddlacetoyear.Items.FindByText(acaYr[1]));
            }

            //added by sudhagar
            txthostelname.Text = string.Empty;
            insqry1 = "";
            insqry1 = "select LinkValue from New_InsSettings where LinkName='IncludeHostelName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            string hstlName = d2.GetFunction(insqry1).Trim();
            if (!string.IsNullOrEmpty(hstlName) && hstlName != "0")
                txthostelname.Text = hstlName;
            insqry1 = "";
            cbincshift.Checked = false;
            insqry1 = "select LinkValue from New_InsSettings where LinkName='IncludeShiftName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            string shftName = d2.GetFunction(insqry1).Trim();
            if (shftName == "1")
                cbincshift.Checked = true;

        }
        catch { }
    }
    protected void ddl_title1_IndexChanged(object sender, EventArgs e)
    {
        try
        {
            string insqry1 = "";
            if (ddl_title1.Items.Count > 0)
            {
                insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='HideInstituteAddressInChallan" + ddl_title1.SelectedValue.Trim() + "' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ");
                if (insqry1.Trim() == "0")
                    insqry1 = "";
            }
            txtInstituteHideValue.Text = insqry1.Trim();
        }
        catch { }
    }
    protected void ddlacefromyear_Indexchange(object sender, EventArgs e)
    {
        loadToYear();
    }
    private void loadToYear()
    {
        ddlacetoyear.Items.Clear();
        int acefrom = DateTime.Now.Year;
        if (ddlacefromyear.Items.Count > 0)
        {
            int.TryParse(ddlacefromyear.SelectedValue, out acefrom);
            for (int yr = acefrom; yr <= (DateTime.Now.Year + 1); yr++)
            {
                ddlacetoyear.Items.Add(new System.Web.UI.WebControls.ListItem(yr.ToString(), yr.ToString()));
            }
        }
    }
    protected void btnSavePrint_Click(object sender, EventArgs e)
    {
        try
        {
            int storevalue = 0;
            int save1 = 0;
            string insqry1 = string.Empty;
            //Fee Counter for Challan
            insqry1 = "if exists (select * from New_InsSettings where LinkName='ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + txt_ChlCounter.Text.Trim() + "' where LinkName='ChallanFeeCounterValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ChallanFeeCounterValue','" + txt_ChlCounter.Text.Trim() + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Particulars for Challan
            insqry1 = "if exists (select * from New_InsSettings where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + txt_ChlParticulars.Text.Trim() + "' where LinkName='ChallanParticular' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ChallanParticular','" + txt_ChlParticulars.Text.Trim() + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");


            //Challan Office Footer MCC
            insqry1 = "if exists (select * from New_InsSettings where LinkName='ChallanOfficeFooter' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + txtChallanOfficeFooter.Text.Trim() + "' where LinkName='ChallanOfficeFooter' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ChallanOfficeFooter','" + txtChallanOfficeFooter.Text.Trim() + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Load Titles for Hiding Institute address --- Mcc School
            insqry1 = "";
            if (ddl_title1.Items.Count > 0)
            {
                insqry1 = "if exists (select * from New_InsSettings where LinkName='HideInstituteAddressInChallan" + ddl_title1.SelectedValue.Trim() + "' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + txtInstituteHideValue.Text.Trim() + "' where LinkName='HideInstituteAddressInChallan" + ddl_title1.SelectedValue.Trim() + "' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('HideInstituteAddressInChallan" + ddl_title1.SelectedValue.Trim() + "','" + txtInstituteHideValue.Text.Trim() + "','" + usercode + "','" + collegecode1 + "')";
                save1 = d2.update_method_wo_parameter(insqry1, "Text");
            }


            //TermDisplay
            storevalue = 0;
            if (cblTermDisp.Checked)
            {
                storevalue = 1;
            }

            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayTermForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='DisplayTermForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayTermForChallan','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Use IFSC  

            storevalue = 0;
            if (chkUseIfsc.Checked)
            {
                storevalue = 1;
            }

            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='DisplayIFSCForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayIFSCForChallan','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Use Acronym
            storevalue = 0;
            if (chkChlanDegAcr.Checked)
            {
                storevalue = 1;
            }

            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='DisplayAcrForChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayAcrForChallan','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Show Ledgerwise fee in challan 

            storevalue = 0;
            if (cbShowledgerwise.Checked)
            {
                storevalue = 1;
            }

            insqry1 = "if exists (select * from New_InsSettings where LinkName='ShowLedgerwiseFeesinChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='ShowLedgerwiseFeesinChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ShowLedgerwiseFeesinChallan','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");



            //Smartcard Number Display
            storevalue = 0;
            storevalue = rblSmartNodisplay.SelectedIndex;

            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayNumberForSmartCd','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //College or Bank or Student

            storevalue = 0;
            if (cblDenom.Items[0].Selected == true)
            {
                //College
                storevalue = 1;
            }
            if (cblDenom.Items[1].Selected == true)
            {
                //Bank
                storevalue = 2;
            }
            if (cblDenom.Items[2].Selected == true)
            {
                //Bank
                storevalue = 3;
            }
            if (cblDenom.Items[0].Selected == true && cblDenom.Items[1].Selected == true)
            {
                //College and Bank
                storevalue = 5;
            }
            if (cblDenom.Items[1].Selected == true && cblDenom.Items[2].Selected == true)
            {
                //Bank and Student
                storevalue = 6;
            }
            if (cblDenom.Items[0].Selected == true && cblDenom.Items[2].Selected == true)
            {
                //College and Student
                storevalue = 7;
            }
            if (cblDenom.Items[0].Selected == true && cblDenom.Items[1].Selected == true && cblDenom.Items[2].Selected == true)
            {
                //All
                storevalue = 4;
            }

            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='DisplayDenominationChallan' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayDenominationChallan','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            //Academic Year
            string store = DateTime.Now.Year + "," + (DateTime.Now.Year + 1);
            if (ddlacefromyear.Items.Count > 0 && ddlacetoyear.Items.Count > 0)
            {
                store = ddlacefromyear.SelectedValue + "," + ddlacetoyear.SelectedValue;
            }
            insqry1 = "if exists (select * from New_InsSettings where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + store + "' where LinkName='ChallanAcademicYear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ChallanAcademicYear','" + store + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");


            //hostel name  and include shift
            //Load Titles for Hiding Institute address --- Mcc School

            //added by sudhagar 29.03.2017
            insqry1 = "";
            string hstlName = txthostelname.Text.Trim();
            insqry1 = "if exists (select * from New_InsSettings where LinkName='IncludeHostelName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + hstlName + "' where LinkName='IncludeHostelName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('IncludeHostelName','" + hstlName + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");

            insqry1 = "";
            int chkCnt = 0;
            if (cbincshift.Checked)
                chkCnt = 1;
            insqry1 = "if exists (select * from New_InsSettings where LinkName='IncludeShiftName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + chkCnt + "' where LinkName='IncludeShiftName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('IncludeShiftName','" + chkCnt + "','" + usercode + "','" + collegecode1 + "')";
            save1 = d2.update_method_wo_parameter(insqry1, "Text");


            imgdiv2.Visible = true;
            lbl_erroralert.Text = "Saved Sucessfully";
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode1, "CRSettings");
            imgdiv2.Visible = true;
            lbl_erroralert.Text = "Not Saved.";
        }
    }
    //HEADER SETTINGS SAVE
    protected void cb_GHwise_OncheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gridDegReport.DataSource = null;
            gridDegReport.DataBind();

            if (cb_GHwise.Checked)
            {
                cb_Degwise.Checked = false;
                loadGHwise();
            }
            else
            {
                gridGHreport.DataSource = null;
                gridGHreport.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void cb_Degwise_OncheckedChanged(object sender, EventArgs e)
    {
        try
        {
            gridGHreport.DataSource = null;
            gridGHreport.DataBind();

            if (cb_Degwise.Checked)
            {
                cb_GHwise.Checked = false;
                loadDegwise();
            }
            else
            {
                gridDegReport.DataSource = null;
                gridDegReport.DataBind();
            }

        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    private void loadGHwise()
    {
        gridGHreport.DataSource = null;
        gridGHreport.DataBind();
        StringBuilder sbGh = new StringBuilder();
        for (int gh = 0; gh < lb_selecthdr.Items.Count; gh++)
        {
            sbGh.Append("'" + lb_selecthdr.Items[gh] + "',");
        }
        if (sbGh.Length > 0)
        {
            sbGh.Remove(sbGh.Length - 1, 1);
            DataSet dsGH = new DataSet();
            string selQ = "select ChlGroupHeader,HeaderName,h.HeaderPk from FS_ChlGroupHeaderSettings g,FM_HeaderMaster h where h.HeaderPK=g.HeaderFK and h.CollegeCode=g.CollegeCode  and ChlGroupHeader in (" + sbGh.ToString() + ") order by ChlGroupHeader asc";
            dsGH = d2.select_method_wo_parameter(selQ, "Text");
            if (dsGH.Tables.Count > 0 && dsGH.Tables[0].Rows.Count > 0)
            {
                DataTable dtGh = new DataTable();
                dtGh.Columns.Add("S.No");
                dtGh.Columns.Add("GroupHeader");
                dtGh.Columns.Add("Header");
                dtGh.Columns.Add("HeaderPk");
                for (int gh = 0; gh < dsGH.Tables[0].Rows.Count; gh++)
                {
                    DataRow drGH = dtGh.NewRow();
                    drGH[0] = gh + 1;
                    drGH[1] = Convert.ToString(dsGH.Tables[0].Rows[gh]["ChlGroupHeader"]);
                    drGH[2] = Convert.ToString(dsGH.Tables[0].Rows[gh]["HeaderName"]);
                    drGH[3] = Convert.ToString(dsGH.Tables[0].Rows[gh]["HeaderPk"]);
                    dtGh.Rows.Add(drGH);
                }
                if (dtGh.Rows.Count > 0)
                {
                    gridGHreport.DataSource = dtGh;
                    gridGHreport.DataBind();
                }
            }
        }
    }
    private void loadDegwise()
    {
        gridDegReport.DataSource = null;
        gridDegReport.DataBind();
        StringBuilder sbGh = new StringBuilder();
        for (int gh = 0; gh < lb_selecthdr.Items.Count; gh++)
        {
            sbGh.Append("'" + lb_selecthdr.Items[gh] + "',");
        }
        if (sbGh.Length > 0)
        {
            sbGh.Remove(sbGh.Length - 1, 1);
            DataSet dsGH = new DataSet();
            string degcode = GetSelectedItemsValue(cbl_dept);
            string stream = GetSelectedItemsText(cbl_strm);
            if (stream != "")
            {
                stream = " and c.type in ('" + stream + "') ";
            }

            string selQ = "select c.Course_Name as Degree,Dt.Dept_Name as Department, Semester, T.TextVal ,ChlGroupHeader,C.type,C.Edu_Level,d.Degree_Code,d.Dept_Code from FM_ChlBankPrintSettings F,Degree d,Department dt,Course C,TextValTable T where T.TextCode =F.Semester and F.DegreeCode =d.Degree_Code and d.Dept_Code =dt.Dept_Code and C.Course_Id =D.Course_Id and SettingType =0 and d.college_code =" + collegecode + " " + stream + " and d.Degree_Code in (" + degcode + ")  order by d.Degree_Code ";

            dsGH = d2.select_method_wo_parameter(selQ, "Text");
            if (dsGH.Tables.Count > 0 && dsGH.Tables[0].Rows.Count > 0)
            {
                DataTable dtGh = new DataTable();
                dtGh.Columns.Add("S.No");
                dtGh.Columns.Add("Degree");
                dtGh.Columns.Add("Department");
                dtGh.Columns.Add("GroupHeader");
                dtGh.Columns.Add("TextVal");
                dtGh.Columns.Add("TextValCode");
                dtGh.Columns.Add("DegCode");
                dtGh.Columns.Add("DeptCode");
                for (int gh = 0; gh < dsGH.Tables[0].Rows.Count; gh++)
                {
                    DataRow drGH = dtGh.NewRow();
                    drGH[0] = gh + 1;
                    drGH[1] = Convert.ToString(dsGH.Tables[0].Rows[gh]["Degree"]);
                    drGH[2] = Convert.ToString(dsGH.Tables[0].Rows[gh]["Department"]);
                    drGH[3] = Convert.ToString(dsGH.Tables[0].Rows[gh]["ChlGroupHeader"]);
                    drGH[4] = Convert.ToString(dsGH.Tables[0].Rows[gh]["TextVal"]);
                    drGH[5] = Convert.ToString(dsGH.Tables[0].Rows[gh]["Semester"]);
                    drGH[6] = Convert.ToString(dsGH.Tables[0].Rows[gh]["Degree_Code"]);
                    drGH[7] = Convert.ToString(dsGH.Tables[0].Rows[gh]["Dept_Code"]);
                    dtGh.Rows.Add(drGH);
                }
                if (dtGh.Rows.Count > 0)
                {
                    gridDegReport.DataSource = dtGh;
                    gridDegReport.DataBind();
                }
            }
        }
    }
    protected void btn_modifyClickGH(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = true;
            int rowIndx = rowIndxClicked();
            if (rowIndx >= 0 && gridGHreport.Rows.Count > rowIndx)
            {
                Label lblGh = (Label)gridGHreport.Rows[rowIndx].FindControl("lbl_ghdr");
                Label lblhdrId = (Label)gridGHreport.Rows[rowIndx].FindControl("lbl_hdrPk");

                string isRemovable = d2.GetFunction("select DegreeCode from FM_ChlBankPrintSettings where  (ChlGroupHeader like '%," + lblGh.Text + "'  or ChlGroupHeader like '" + lblGh.Text + ",%' or ChlGroupHeader like '%," + lblGh.Text + ",%'  or ChlGroupHeader ='" + lblGh.Text + "') and CollegeCode=" + collegecode + "").Trim();
                if (isRemovable == "0" || isRemovable == "")
                {

                    string delQ = "delete from FS_ChlGroupHeaderSettings where  CollegeCode=" + collegecode + " and HeaderFK=" + lblhdrId.Text + " and ChlGroupHeader='" + lblGh.Text + "'";
                    string stream = GetSelectedItemsText(cbl_strm);
                    if (stream != "")
                    {
                        delQ += " and Stream in('" + stream + "') ";
                    }
                    d2.update_method_wo_parameter(delQ, "Text");
                    lbl_erroralert.Text = "Header Removed From Group";
                    loadGHwise();
                }
                else
                {
                    lbl_erroralert.Text = "Group Header Alloted In Department. Cannot Remove";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); lbl_erroralert.Text = "Cannot Remove"; }
    }
    protected void btn_modifyClickDeg(object sender, EventArgs e)
    {
        try
        {
            imgdiv2.Visible = true;
            int rowIndx = rowIndxClicked();
            if (rowIndx >= 0 && gridDegReport.Rows.Count > rowIndx)
            {
                Label lblGh = (Label)gridDegReport.Rows[rowIndx].FindControl("lbl_ghdr");
                Label lblFeecat = (Label)gridDegReport.Rows[rowIndx].FindControl("lbl_hdrPk");
                Label lblDegCode = (Label)gridDegReport.Rows[rowIndx].FindControl("lbldegCode");
                Label lblDeptCode = (Label)gridDegReport.Rows[rowIndx].FindControl("lbldeptCode");

                string delQ = "delete from FM_ChlBankPrintSettings where DegreeCode =" + lblDegCode.Text + " and ChlGroupHeader='" + lblGh.Text + "' and Semester='" + lblFeecat.Text + "' and SettingType=0 and CollegeCode=" + collegecode + "";
                //string stream = GetSelectedItemsText(cbl_strm);
                //if (stream != "")
                //{
                //    delQ += " and Stream in('" + stream + "') ";
                //}
                d2.update_method_wo_parameter(delQ, "Text");
                lbl_erroralert.Text = "Group Header Removed From Department";
                loadDegwise();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); lbl_erroralert.Text = "Cannot Remove"; }
    }
    public void collegebank()
    {
        try
        {
            string queru = "SELECT DISTINCT BankName+'-'+AccNo+'-'+City as BankName,BankPk FROM FM_FinBankMaster where CollegeCode=" + collegecode1 + "";
            DataSet dsBank = d2.select_method_wo_parameter(queru, "Text");
            ddl_collegebank.Items.Clear();

            if (dsBank.Tables[0].Rows.Count > 0)
            {
                ddl_collegebank.DataSource = dsBank;
                ddl_collegebank.DataTextField = "BankName";
                ddl_collegebank.DataValueField = "BankPk";
                ddl_collegebank.DataBind();
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            string stream = "";
            string dispstream = "";
            string title = "";
            string bankfk = "";
            bool saved = false;
            if (ddl_collegebank.Items.Count > 0)
            {
                bankfk = Convert.ToString(ddl_collegebank.SelectedValue);
            }
            title = txt_grphdr.Text.Trim();
            dispstream = txt_dispstream.Text.Trim();

            if (title != "" && bankfk != "")
            {
                if (cbl_strm1.Items.Count != 0)
                {
                    List<string> lstStrm = GetSelectedItemsValueList(cbl_strm1);
                    for (int j = 0; j < lstStrm.Count; j++)
                    {
                        stream = lstStrm[j];
                        List<string> lstHdrname = GetSelectedItemsValueList(cbl_hdrname);
                        for (int i = 0; i < lstHdrname.Count; i++)
                        {
                            string insertQuery = " if exists (select * from FS_ChlGroupHeaderSettings where  HeaderFK=" + Convert.ToString(lstHdrname[i]) + " and Stream='" + stream + "' and CollegeCode=" + collegecode + ") update  FS_ChlGroupHeaderSettings set DispStream='" + dispstream + "',BankFk=" + bankfk + ",ChlGroupHeader='" + title + "' where  HeaderFK=" + Convert.ToString(lstHdrname[i]) + " and Stream='" + stream + "' and CollegeCode=" + collegecode + " else INSERT INTO FS_ChlGroupHeaderSettings (HeaderFK, Stream, DispStream, ChlGroupHeader, BankFk,CollegeCode  ) VALUES (" + Convert.ToString(lstHdrname[i]) + ",'" + stream + "','" + dispstream + "','" + title + "'," + bankfk + "," + collegecode + ")";
                            if (d2.update_method_wo_parameter(insertQuery, "Text") > 0)
                            {
                                saved = true;
                            }
                        }
                    }
                }
                else
                {
                    List<string> lstHdrname = GetSelectedItemsValueList(cbl_hdrname);
                    for (int i = 0; i < lstHdrname.Count; i++)
                    {
                        string insertQuery = " if exists (select * from FS_ChlGroupHeaderSettings where  HeaderFK=" + Convert.ToString(lstHdrname[i]) + " and Stream='" + stream + "' and CollegeCode=" + collegecode + ") update  FS_ChlGroupHeaderSettings set DispStream='" + dispstream + "',BankFk=" + bankfk + ",ChlGroupHeader='" + title + "' where  HeaderFK=" + Convert.ToString(lstHdrname[i]) + " and Stream='" + stream + "' and CollegeCode=" + collegecode + " else INSERT INTO FS_ChlGroupHeaderSettings (HeaderFK, Stream, DispStream, ChlGroupHeader, BankFk,CollegeCode ) VALUES (" + Convert.ToString(lstHdrname[i]) + ",'" + stream + "','" + dispstream + "','" + title + "'," + bankfk + "," + collegecode + ")";
                        if (d2.update_method_wo_parameter(insertQuery, "Text") > 0)
                        {
                            saved = true;
                        }
                    }
                }
                if (saved)
                {
                    lbtn_hdrsettings_Click(sender, e);
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Saved Sucessfully";
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not Saved. Try Later Or Select Header";
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_erroralert.Text = "Please Enter All Fields";
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
        rdo_challan_OnCheckedChanged(sender, e);
    }
    public void loaddescSet()
    {
        try
        {
            ddl_title1.Items.Clear();
            ds.Tables.Clear();

            string sql = "select TextCode,TextVal from TextValTable where TextCriteria ='ChHed' and college_code ='" + collegecode1 + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_title1.DataSource = ds;
                ddl_title1.DataTextField = "TextVal";
                ddl_title1.DataValueField = "TextCode";
                ddl_title1.DataBind();
            }

        }
        catch { }
    }
    //SAVE RECEIPT AND CHALLAN
    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string colCode = collegecode1;
            if (rdo_receipt.Checked)
            {
                if (colCode != "")
                {
                    bool OKINSERT = true;
                    //Header Div Values
                    bool collegeid = cb_colgname.Checked;
                    bool commonclgid = cb_CommonClgname.Checked;
                    bool hostel = cb_hostelname.Checked;
                    bool address1 = cb_addr1.Checked;
                    bool address2 = cb_addr2.Checked;
                    bool address3 = cb_addr3.Checked;
                    bool dist = cb_dist.Checked;
                    bool state = cb_state.Checked;
                    bool university = cb_univ.Checked;
                    bool time = cb_time.Checked;
                    bool mobile = cb_mobile.Checked;
                    bool email = cb_mail.Checked;
                    bool website = cb_website.Checked;
                    bool degACR = cb_degree.Checked;
                    bool semester = cb_sem.Checked;
                    bool rollno = cb_rollno.Checked;
                    bool regno = cb_regno.Checked;
                    bool adminno = cb_appno.Checked;
                    bool rightLogo = cb_rlogo.Checked;
                    bool leftlogo = cb_llogo.Checked;
                    bool year = cb_year.Checked;

                    bool fathername = cb_fname.Checked;
                    bool studname = cb_studname.Checked;
                    bool seattype = cb_seattype.Checked;
                    bool setRollAsAdmin = cb_adminno.Checked;
                    bool boarding = cb_boarding.Checked;
                    bool mothername = cb_mname.Checked;
                    bool degName = cb_degname.Checked;
                    bool validdate = cb_validity.Checked;
                    string recptValid = txt_valid.Text.Trim();


                    //Body Div Values
                    bool allotedAmt = cb_alloted.Checked;
                    bool fineAmt = cb_fine.Checked;
                    bool balAmt = cb_balance.Checked;
                    bool semOrYear = cb_semester.Checked;
                    bool prevPaidAmt = cb_previous.Checked;
                    bool excessAmt = cb_excess.Checked;
                    bool totDetails = cb_total.Checked;
                    bool fineInRow = cb_fineinrow.Checked;
                    bool totWTselectCol = cb_totalcolumn.Checked;
                    bool concession = cb_concession.Checked;
                    string concessionValue = string.Empty;
                    if (concession)
                    {
                        concessionValue = txt_concession.Text.Trim();
                        if (concessionValue == "")
                        {
                            OKINSERT = false;
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Please Enter Concession Value";
                        }
                    }

                    //Footer Div Values

                    bool studCopy = cb_student.Checked;
                    bool officopy = cb_office.Checked;
                    bool transCopy = cb_transport.Checked;
                    bool narration = cb_narration.Checked;
                    bool deduction = cb_deduction.Checked;
                    bool forclgName = cb_forcolgname.Checked;
                    bool authSign = cb_authsign.Checked;
                    string authSignValue = "";
                    if (authSign)
                    {
                        authSignValue = txt_authsign.Text.Trim();
                        //if (authSignValue == "")
                        //{
                        //    OKINSERT = false;
                        //    imgdiv2.Visible = true;
                        //    lbl_erroralert.Text = "Please Enter Authorizer Name";
                        //}
                    }

                    bool studOffiCopy = socopypage.Checked;
                    bool dispModeWTcash = cb_modecash.Checked;
                    //bool acayear = cb_acayear.Checked;//added by abarna
                    bool signFile = cb_sign.Checked;

                    if (OKINSERT)
                    {
                        string insertQuery = "if exists (select * from FM_RcptChlPrintSettings where CollegeCode ='" + colCode + "')   UPDATE FM_RcptChlPrintSettings SET  IsCollegeName='" + collegeid + "'  ,isCollegeCom_name='" + commonclgid + "',IsCollegeAdd1='" + address1 + "'  ,IsCollegeAdd2='" + address2 + "'  ,IsCollegeAdd3 ='" + address3 + "' ,IsCollegeDist='" + dist + "'  ,IsCollegeState='" + state + "'  ,IsCollegeUniversity='" + university + "'  ,IsRightLogo='" + rightLogo + "' , IsLeftLogo='" + leftlogo + "' , IsRollNo='" + rollno + "' , IsRegNo='" + regno + "' , IsAdminNo='" + adminno + "' , IsStudName='" + studname + "' , IsDegreeName='" + degName + "' , IsDegreeAcr='" + degACR + "' , IsFatherName='" + fathername + "' , IsMontherName='" + mothername + "' ,   IsBoarding='" + boarding + "' , IsValidUpto='" + validdate + "' , IsAllotedAmt='" + allotedAmt + "' ,  IsBalanceAmt='" + balAmt + "' , IsFineAmt='" + fineAmt + "' , IsSemYear='" + semOrYear + "' ,  IsPrevPaid='" + prevPaidAmt + "' , IsExcessAmt='" + excessAmt + "' , IsFineinRow ='" + fineInRow + "', IsConcession='" + concession + "' , ConcessionName='" + concessionValue + "' , IsStudCopy='" + studCopy + "' , IsOfficeCopy='" + officopy + "' , IsTransportCopy ='" + transCopy + "', IsNarration='" + narration + "' , IsTotConcession='" + deduction + "' , IsForCollegeName='" + forclgName + "' , IsAuthSign='" + authSign + "' , AuthName='" + authSignValue + "' , PageType='" + studOffiCopy + "'  ,cashier_sign='" + signFile + "',IsTime='" + time + "',ValidDate='" + recptValid + "',IsYear='" + year + "',IsSemester='" + semester + "',IsSeatType='" + seattype + "',isMobile='" + mobile + "',isEmail='" + email + "',isWebsite='" + website + "',ishostelname='" + hostel + "' where collegecode='" + colCode + "'                                                                                else    INSERT INTO FM_RcptChlPrintSettings (isCollegeCom_name,CollegeCode,IsCollegeName  ,IsCollegeAdd1  ,IsCollegeAdd2  ,IsCollegeAdd3  ,IsCollegeDist  ,IsCollegeState  ,IsCollegeUniversity  ,IsRightLogo , IsLeftLogo , IsRollNo , IsRegNo , IsAdminNo , IsStudName , IsDegreeName , IsDegreeAcr , IsFatherName , IsMontherName ,   IsBoarding , IsValidUpto , IsAllotedAmt ,  IsBalanceAmt , IsFineAmt , IsSemYear ,  IsPrevPaid , IsExcessAmt , IsFineinRow , IsConcession , ConcessionName , IsStudCopy , IsOfficeCopy , IsTransportCopy , IsNarration , IsTotConcession , IsForCollegeName , IsAuthSign , AuthName , PageType ,cashier_sign,IsTime,ValidDate,IsYear,IsSemester, IsSeatType,isMobile,isEmail,isWebsite,ishostelname) VALUES ('" + commonclgid + "','" + colCode + "','" + collegeid + "','" + address1 + "','" + address2 + "','" + address3 + "','" + dist + "','" + state + "','" + university + "','" + rightLogo + "','" + leftlogo + "','" + rollno + "','" + regno + "','" + adminno + "','" + studname + "','" + degName + "','" + degACR + "','" + fathername + "','" + mothername + "','" + boarding + "','" + validdate + "','" + allotedAmt + "','" + balAmt + "','" + fineAmt + "','" + semOrYear + "','" + prevPaidAmt + "','" + excessAmt + "','" + fineInRow + "','" + concession + "','" + concessionValue + "','" + studCopy + "','" + officopy + "','" + transCopy + "','" + narration + "','" + deduction + "','" + forclgName + "','" + authSign + "','" + authSignValue + "','" + studOffiCopy + "','" + signFile + "','" + time + "','" + recptValid + "','" + year + "','" + semester + "','" + seattype + "','" + mobile + "','" + email + "','" + website + "','" + hostel + "')";

                        int insertOK = d2.update_method_wo_parameter(insertQuery, "Text");
                        if (insertOK > 0)
                        {

                            if (signFile)
                            {
                                if (FileUpload1.HasFile)
                                {
                                    if (FileUpload1.FileName.EndsWith(".jpg") || FileUpload1.FileName.EndsWith(".gif") || FileUpload1.FileName.EndsWith(".png"))
                                    {
                                        string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                                        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                                        string documentType = string.Empty;
                                        switch (fileExtension)
                                        {

                                            case ".gif":
                                                documentType = "image/gif";
                                                break;

                                            case ".png":
                                                documentType = "image/png";
                                                break;

                                            case ".jpg":
                                                documentType = "image/jpg";
                                                break;

                                        }
                                    }
                                    int fileSize = FileUpload1.PostedFile.ContentLength;
                                    //Create array and read the file into it
                                    byte[] documentBinary = new byte[fileSize];
                                    FileUpload1.PostedFile.InputStream.Read(documentBinary, 0, fileSize);

                                    SqlCommand cmdnotes = new SqlCommand();
                                    cmdnotes.CommandText = "UPDATE FM_RcptChlPrintSettings SET SignImage=@imgsign where collegecode=" + colCode + "";
                                    SqlParameter uploadedDocument = new SqlParameter("@imgsign", SqlDbType.Binary, fileSize);
                                    uploadedDocument.Value = documentBinary;
                                    cmdnotes.Parameters.Add(uploadedDocument);
                                    cmdnotes.CommandType = CommandType.Text;
                                    cmdnotes.Connection = ssql;
                                    ssql.Close();
                                    ssql.Open();
                                    int result = cmdnotes.ExecuteNonQuery();

                                }
                                else
                                {
                                    OKINSERT = false;
                                    imgdiv2.Visible = true;
                                    lbl_erroralert.Text = "Please Select An Image File";
                                }
                            }


                            #region Save common settings

                            int storevalue = 1;
                            storevalue = rbl_ReceiptFormat.SelectedIndex + 1;
                            string insqry = "if exists (select * from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue + "' where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ReceiptPrintFormat','" + storevalue + "','" + usercode + "','" + collegecode1 + "')";
                            int save = d2.update_method_wo_parameter(insqry, "Text");

                            int storevalue1 = 1;
                            storevalue1 = rbl_ChallanFormat.SelectedIndex + 1;
                            string insqry1 = "if exists (select * from New_InsSettings where LinkName='ChallanPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='ChallanPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ChallanPrintFormat','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            int save1 = d2.update_method_wo_parameter(insqry1, "Text");


                            int storevalue2 = 1;//abarna
                            storevalue2 = printformat.SelectedIndex + 1;
                            string insqry2 = "if exists (select * from New_InsSettings where LinkName='ReceiptPrintFormatsheet' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue2 + "' where LinkName='ReceiptPrintFormatsheet' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ReceiptPrintFormatsheet','" + storevalue2 + "','" + usercode + "','" + collegecode1 + "')";
                            int save2 = d2.update_method_wo_parameter(insqry2, "Text");

                            string storevalue3 = txtheight.Text.Trim();//abarna

                            string insqry3 = "if exists (select * from New_InsSettings where LinkName='ReceiptPrintFormatSheetTextboxValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue3 + "' where LinkName='ReceiptPrintFormatSheetTextboxValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ReceiptPrintFormatSheetTextboxValue','" + storevalue3 + "','" + usercode + "','" + collegecode1 + "')";
                            int save3 = d2.update_method_wo_parameter(insqry3, "Text");

                            #endregion

                            #region Hide Grid Column
                            storevalue1 = 0;
                            if (cbFeeAmount.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='HideGridFeeAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='HideGridFeeAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('HideGridFeeAmount','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            storevalue1 = 0;
                            if (cbDedAmount.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='HideGridDedAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='HideGridDedAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('HideGridDedAmount','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            storevalue1 = 0;
                            if (cbTotAmount.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='HideGridTotAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='HideGridTotAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('HideGridTotAmount','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            storevalue1 = 0;
                            if (cbPaidAmount.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='HideGridPaidAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='HideGridPaidAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('HideGridPaidAmount','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            storevalue1 = 0;
                            if (cbShowBalOnly.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='ShowBalanceOnlyGrid' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='ShowBalanceOnlyGrid' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('ShowBalanceOnlyGrid','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            //added by sudhagar 17.08.2017 
                            storevalue1 = 0;
                            if (cbCurSem.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='CurrentAndPreviousSemWithBalOnly' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='CurrentAndPreviousSemWithBalOnly' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('CurrentAndPreviousSemWithBalOnly','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                            //added by abarna 04.10.2017
                            storevalue1 = 0;
                            if (showdatetime.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='showdatetime' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='showdatetime' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('showdatetime','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                            //added by abarna
                            storevalue1 = 0;
                            if (cb_acayear.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='showacademicyear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='showacademicyear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('showacademicyear','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                            storevalue1 = 0;
                            if (cb_exclude.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='Excludecopys' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='Excludecopys' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('Excludecopys','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");
                            //abarna
                            storevalue1 = 0;
                            if (cb_Username.Checked)
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='DisplayUserName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='DisplayUserName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('DisplayUserName','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            if (cb_collectedby.Checked)//abarna 13.08.2018
                            {
                                storevalue1 = 1;
                            }
                            insqry1 = "if exists (select * from New_InsSettings where LinkName='collectedby' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ) update New_InsSettings set LinkValue ='" + storevalue1 + "' where LinkName='collectedby' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' else insert into New_InsSettings(LinkName,LinkValue,user_code,college_code) values ('collectedby','" + storevalue1 + "','" + usercode + "','" + collegecode1 + "')";
                            save1 = d2.update_method_wo_parameter(insqry1, "Text");

                            #endregion

                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Saved Sucessfully";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Not Saved. Try Later";
                        }
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not Saved. Try Later";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    protected void btn_saveChallan_Click(object sender, EventArgs e)
    {
        try
        {
            string colCode = collegecode1;
            if (rdo_challan.Checked)
            {
                if (colCode != "")
                {
                    string title = "";
                    string dept = "";
                    string pagecode = "";
                    string selcHeader = "";

                    if (ddl_title.Items.Count > 0)
                    {
                        title = Convert.ToString(ddl_title.SelectedItem.Text);
                        pagecode = Convert.ToString(ddl_title.SelectedItem.Value);
                    }

                    for (int i = 0; i < lb_hdr.Items.Count; i++)
                    {
                        if (selcHeader == "")
                        {
                            selcHeader = String.Format(Convert.ToString(lb_hdr.Items[i].Text));
                        }
                        else
                        {
                            selcHeader = String.Format(selcHeader + "," + Convert.ToString(lb_hdr.Items[i].Text));
                        }
                    }

                    if (pagecode != "" && selcHeader != "")
                    {
                        bool insOk = false;
                        List<string> lstDept = GetSelectedItemsValueList(cbl_dept);
                        for (int i = 0; i < lstDept.Count; i++)
                        {
                            dept = lstDept[i];

                            string insQuery = "if exists (select * from FM_ChlBankPrintSettings where DegreeCode='" + dept + "' and PageCode='" + pagecode + "' and CollegeCode='" + colCode + "' and SettingType=1) update FM_ChlBankPrintSettings set ChlGroupHeader='" + selcHeader + "' where   DegreeCode='" + dept + "' and PageCode='" + pagecode + "' and CollegeCode='" + colCode + "' and SettingType=1 else  INSERT INTO FM_ChlBankPrintSettings (PageCode, DegreeCode,  ChlGroupHeader,CollegeCode, SettingType) VALUES('" + pagecode + "','" + dept + "','" + selcHeader + "','" + colCode + "',1) ";

                            if (d2.update_method_wo_parameter(insQuery, "Text") > 0)
                            {
                                insOk = true;
                            }
                        }

                        if (insOk)
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Saved Successfully";
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_erroralert.Text = "Not Saved. Try Later Or Select Department";
                        }
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_erroralert.Text = "Please Provide All Inputs";
                    }
                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_erroralert.Text = "Not Saved. Try Later";
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    public void defaultSetting()
    {
        try
        {
            string selectQ = "select * from FM_RcptChlPrintSettings where collegecode =" + collegecode1 + "";

            ds.Clear();
            ds = d2.select_method_wo_parameter(selectQ, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeName"]) == 1)
                        cb_colgname.Checked = true;
                    else
                        cb_colgname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["isCollegeCom_name"]) == 1)
                        cb_CommonClgname.Checked = true;
                    else
                        cb_CommonClgname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeAdd1"]) == 1)
                        cb_addr1.Checked = true;
                    else
                        cb_addr1.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeAdd2"]) == 1)
                        cb_addr2.Checked = true;
                    else
                        cb_addr2.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeAdd3"]) == 1)
                        cb_addr3.Checked = true;
                    else
                        cb_addr3.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeDist"]) == 1)
                        cb_dist.Checked = true;
                    else
                        cb_dist.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeState"]) == 1)
                        cb_state.Checked = true;
                    else
                        cb_state.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsCollegeUniversity"]) == 1)
                        cb_univ.Checked = true;
                    else
                        cb_univ.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsRightLogo"]) == 1)
                        cb_rlogo.Checked = true;
                    else
                        cb_llogo.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsLeftLogo"]) == 1)
                        cb_llogo.Checked = true;
                    else
                        cb_rlogo.Checked = false;
                    if (Convert.ToBoolean(Convert.ToString(ds.Tables[0].Rows[0]["IsTime"])))
                        cb_time.Checked = true;
                    else
                        cb_time.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsRollNo"]) == 1)
                        cb_rollno.Checked = true;
                    else
                        cb_rollno.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsAdminNo"]) == 1)
                        cb_appno.Checked = true;
                    else
                        cb_appno.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsRegNo"]) == 1)
                        cb_regno.Checked = true;
                    else
                        cb_regno.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["isHostelName"]) == 1)//added by abarna
                        cb_hostelname.Checked = true;
                    else
                        cb_hostelname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsStudName"]) == 1)
                        cb_studname.Checked = true;
                    else
                        cb_studname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsDegreeName"]) == 1)
                        cb_degname.Checked = true;
                    else
                        cb_degname.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsDegreeAcr"]) == 1)
                        cb_degree.Checked = true;
                    else
                        cb_degree.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsFatherName"]) == 1)
                        cb_fname.Checked = true;
                    else
                        cb_fname.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsMontherName"]) == 1)
                        cb_mname.Checked = true;
                    else
                        cb_mname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsBoarding"]) == 1)
                        cb_boarding.Checked = true;
                    else
                        cb_boarding.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsValidUpto"]) == 1)
                    {
                        cb_validity.Checked = true;
                        txt_valid.Text = Convert.ToString(ds.Tables[0].Rows[0]["ValidDate"]);
                    }
                    else
                        cb_validity.Checked = false;



                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsAllotedAmt"]) == 1)
                        cb_alloted.Checked = true;
                    else
                        cb_alloted.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsFineAmt"]) == 1)
                        cb_fine.Checked = true;
                    else
                        cb_fine.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsBalanceAmt"]) == 1)
                        cb_balance.Checked = true;
                    else
                        cb_balance.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsSemYear"]) == 1)
                        cb_semester.Checked = true;
                    else
                        cb_semester.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsPrevPaid"]) == 1)
                        cb_previous.Checked = true;
                    else
                        cb_previous.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsExcessAmt"]) == 1)
                        cb_excess.Checked = true;
                    else
                        cb_excess.Checked = false;
                    //if (Convert.ToInt16(ds.Tables[0].Rows[0]["Total_Details"]) == 1)
                    //    cb_total.Checked = true;
                    //else
                    //    cb_total.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsFineinRow"]) == 1)
                        cb_fineinrow.Checked = true;
                    else
                        cb_fineinrow.Checked = false;
                    //if (Convert.ToInt16(ds.Tables[0].Rows[0]["TotalSelCol"]) == 1)
                    //    cb_totalcolumn.Checked = true;
                    //else
                    //    cb_totalcolumn.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsConcession"]) == 1)
                    {
                        cb_concession.Checked = true;
                        txt_concession.Text = Convert.ToString(ds.Tables[0].Rows[0]["ConcessionName"]);
                    }
                    else
                        cb_concession.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsStudCopy"]) == 1)
                        cb_student.Checked = true;
                    else
                        cb_student.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsOfficeCopy"]) == 1)
                        cb_office.Checked = true;
                    else
                        cb_office.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsTransportCopy"]) == 1)
                        cb_transport.Checked = true;
                    else
                        cb_transport.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsNarration"]) == 1)
                        cb_narration.Checked = true;
                    else
                        cb_narration.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsTotConcession"]) == 1)
                        cb_deduction.Checked = true;
                    else
                        cb_deduction.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsForCollegeName"]) == 1)
                        cb_forcolgname.Checked = true;
                    else
                        cb_forcolgname.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsAuthSign"]) == 1)
                    {
                        cb_authsign.Checked = true;
                        txt_authsign.Text = Convert.ToString(ds.Tables[0].Rows[0]["AuthName"]);
                    }
                    else
                        cb_authsign.Checked = false;


                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["PageType"]) == 1)
                        socopypage.Checked = true;
                    else
                        socopypage.Checked = false;
                    //if (Convert.ToInt16(ds.Tables[0].Rows[0]["DisModeWithCash"]) == 1)
                    //    cb_modecash.Checked = true;
                    //else
                    //    cb_modecash.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["cashier_sign"]) == 1)
                        cb_sign.Checked = true;
                    else
                        cb_sign.Checked = false;


                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["isMobile"]) == 1)
                        cb_mobile.Checked = true;
                    else
                        cb_mobile.Checked = false;


                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["isEmail"]) == 1)
                        cb_mail.Checked = true;
                    else
                        cb_mail.Checked = false;
                    //Remaining


                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["isWebsite"]) == 1)
                        cb_website.Checked = true;
                    else
                        cb_website.Checked = false;
                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsSemester"]) == 1)
                        cb_sem.Checked = true;
                    else
                        cb_sem.Checked = false;

                    if (Convert.ToInt16(ds.Tables[0].Rows[0]["IsSeatType"]) == 1)
                        cb_seattype.Checked = true;
                    else
                        cb_seattype.Checked = false;

                    //if (Convert.ToInt16(ds.Tables[0].Rows[0]["rollas_adm"]) == 1)
                    //    cb_adminno.Checked = true;
                    //else
                    //    cb_adminno.Checked = false;
                    try
                    {
                        string insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        int save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            rbl_ReceiptFormat.SelectedIndex = 0;
                        }
                        else
                        {
                            rbl_ReceiptFormat.SelectedIndex = save1 - 1;
                        }
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormatsheet' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";//abarna 22.06.2018
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            printformat.SelectedIndex = 0;
                        }
                        else
                        {
                            printformat.SelectedIndex = save1 - 1;
                        }

                        insqry1 = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='ReceiptPrintFormatSheetTextboxValue' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'");//abarna 
                        txtheight.Text = insqry1;
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanPrintFormat' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            rbl_ChallanFormat.SelectedIndex = 0;
                        }
                        else
                        {
                            rbl_ChallanFormat.SelectedIndex = save1 - 1;
                        }
                        #region Hide Grid Column
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='HideGridFeeAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbFeeAmount.Checked = false;
                        }
                        else
                        {
                            cbFeeAmount.Checked = true;
                        }

                        insqry1 = "select LinkValue from New_InsSettings where LinkName='HideGridDedAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbDedAmount.Checked = false;
                        }
                        else
                        {
                            cbDedAmount.Checked = true;
                        }

                        insqry1 = "select LinkValue from New_InsSettings where LinkName='HideGridTotAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbTotAmount.Checked = false;
                        }
                        else
                        {
                            cbTotAmount.Checked = true;
                        }
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='HideGridPaidAmount' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbPaidAmount.Checked = false;
                        }
                        else
                        {
                            cbPaidAmount.Checked = true;
                        }
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='ShowBalanceOnlyGrid' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbShowBalOnly.Checked = false;
                        }
                        else
                        {
                            cbShowBalOnly.Checked = true;
                        }
                        //added by sudhagar 17.08.2017
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='CurrentAndPreviousSemWithBalOnly' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cbCurSem.Checked = false;
                        }
                        else
                        {
                            cbCurSem.Checked = true;
                        }
                        //added by abarna  04.10.2017
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='showdatetime' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            showdatetime.Checked = false;
                        }
                        else
                        {
                            showdatetime.Checked = true;
                        }
                        //added by abarna
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='showacademicyear' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cb_acayear.Checked = false;
                        }
                        else
                        {
                            cb_acayear.Checked = true;
                        }
                        //added by abarna 11.06.2018

                        insqry1 = "select LinkValue from New_InsSettings where LinkName='ExcludeCopys' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cb_exclude.Checked = false;
                        }
                        else
                        {
                            cb_exclude.Checked = true;
                        }

                        //added by abarna

                        insqry1 = "select LinkValue from New_InsSettings where LinkName='DisplayUserName' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cb_Username.Checked = false;
                        }
                        else
                        {
                            cb_Username.Checked = true;
                        }


                        //added by abarna 13.08.2018
                        insqry1 = "select LinkValue from New_InsSettings where LinkName='cb_collectedby' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                        save1 = Convert.ToInt32(d2.GetFunction(insqry1));
                        if (save1 == 0)
                        {
                            cb_collectedby.Checked = false;
                        }
                        else
                        {
                            cb_collectedby.Checked = true;
                        }

                        #endregion
                    }
                    catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "CRSettings"); }
    }
    private string GetSelectedItemsValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("," + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsValueAsString(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private string GetSelectedItemsText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder sbSelected = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (sbSelected.Length == 0)
                    {
                        sbSelected.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        sbSelected.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { sbSelected.Clear(); }
        return sbSelected.ToString();
    }
    private List<string> GetSelectedItemsValueList(CheckBoxList cblSelected)
    {
        System.Collections.Generic.List<string> lsSelected = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblSelected.Items.Count; list++)
            {
                if (cblSelected.Items[list].Selected)
                {
                    lsSelected.Add(cblSelected.Items[list].Value);
                }
            }
        }
        catch { lsSelected.Clear(); }
        return lsSelected;
    }
    private List<string> GetItemsValueList(CheckBoxList cblItems)
    {
        System.Collections.Generic.List<string> lsItems = new System.Collections.Generic.List<string>();
        try
        {
            for (int list = 0; list < cblItems.Items.Count; list++)
            {
                lsItems.Add(cblItems.Items[list].Value);
            }
        }
        catch { lsItems.Clear(); }
        return lsItems;
    }
    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }
    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }
    public static Control GetPostBackControl(Page page)
    {
        Control control = null;
        string ctrlname = page.Request.Params.Get("__EVENTTARGET");
        if (ctrlname != null && ctrlname != string.Empty)
        {
            control = page.FindControl(ctrlname);
        }
        else
        {
            foreach (string ctl in page.Request.Form)
            {
                Control c = page.FindControl(ctl);
                if (c is System.Web.UI.WebControls.Button)
                {
                    control = c;
                    break;
                }
            }
        }
        return control;
    }
    public int rowIndxClicked()
    {
        int rownumber = -1;
        try
        {
            Control ctrlid = GetPostBackControl(this.Page);
            string rno = Convert.ToString(ctrlid.UniqueID).Split('$')[1].Replace("ctl", "");
            int.TryParse(rno, out rownumber);
            rownumber -= 2;
        }
        catch { rownumber = -1; }

        return rownumber;
    }
    //************************************************//
    // Code Ended by Idhris -Last modified 01-03-2017 //
    //************************************************//  
    protected void ReceiptFormat_OnselectChange(object sender, EventArgs e)//abarna
    {
        if (rbl_ReceiptFormat.SelectedItem.Text == "Format14")
        {
            print.Visible = true;
            printformat.Visible = true;
        }
        else
        {
            print.Visible = false;
            printformat.Visible = false;
        }
    }
    protected void printformat_OnselectChange(object sender, EventArgs e)//abarna
    {
        if (printformat.SelectedItem.Text == "10*6")
        {
            txtheight.Visible = true;
        }
        else
        {
            txtheight.Visible = false;
        }
    }

    private double checkSchoolSetting()
    {
        double getVal = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select  value from Master_Settings where settings='schoolorcollege' and usercode='" + usercode + "'")), out getVal);
        return getVal;
    }

}
