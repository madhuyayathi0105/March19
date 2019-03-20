using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Gios.Pdf;
using InsproDataAccess;
using wc = System.Web.UI.WebControls;
using Farpoint = FarPoint.Web.Spread;
using System.Configuration;

public partial class StudentMod_eligiblilysetting : System.Web.UI.Page
{
    Hashtable hat = new Hashtable();
    DAccess2 da = new DAccess2();
    DAccess2 dt = new DAccess2();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    InsproStoreAccess storeAcc = new InsproStoreAccess();
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    string k = string.Empty;

    string k1 = string.Empty;

    DataTable dt1 = new DataTable();

    DAccess2 d2 = new DAccess2();
    Hashtable ht = new Hashtable();
    DataTable dtCommon = new DataTable();
    ReuasableMethods rs = new ReuasableMethods();
    Dictionary<string, string> dicQueryParameter = new Dictionary<string, string>();
    FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
    string collegeCode = string.Empty;
    string userCode = string.Empty;
    string userCollegeCode = string.Empty;
    string singleUser = string.Empty;
    string groupUserCode = string.Empty;
    string qryUserOrGroupCode = string.Empty;
    string build = "", buildvalue = string.Empty;
    string qryCollege = string.Empty;
    string qry = string.Empty;
    string qryBatch = string.Empty;
    string testDate;
    string[] arrang;
    string[] arran;
    string norow = string.Empty;
    string nocol = string.Empty;
    string allotseat = string.Empty;
    string[] spcel;
    int hss = 0;
    int a = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            errormsg.Visible = false;
            string str = string.Empty;
            string strname = string.Empty;
            foreach (GridViewRow gvrow in grdsubjectDetails.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect");
                if (chk != null & chk.Checked)
                {
                    str += "" + gvrow.Cells[1].Text + ";";

                    str += "";
                }
            }
            la.Text = str;
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
                    return;
                }
            }

            userCollegeCode = Convert.ToString(Session["collegecode"]).Trim();
            userCode = (Session["usercode"] != null) ? Convert.ToString(Session["usercode"]).Trim() : "";
            singleUser = (Session["single_user"] != null) ? Convert.ToString(Session["single_user"]).Trim() : "";
            groupUserCode = (Session["group_code"] != null) ? Convert.ToString(Session["group_code"]).Trim() : "";
            //bindbranch();

            if (!IsPostBack)
            {
                Bindcollege();
                year();
                binddegree();
                bindbranch();
                Div2.Visible = false;
                Div1.Visible = false;
                div.Visible = false;
                bindsubject2();
                DropDownList2.SelectedIndex = 0;
                DropDownList3.SelectedIndex = 0;
            }
        }
        catch (Exception ex) { }
    }

    public void year()
    {
        qry = " select distinct batch_year from Registration where batch_year<>'-1' and CC=0 and DelFlag=0 and Exam_Flag<>'debar'order by batch_year desc";
        DataSet ds1 = new DataSet();
        ds1 = d2.select_method_wo_parameter(qry, "text");
        ddlbatch.DataSource = ds1;

        ddlbatch.DataTextField = "batch_year";

        ddlbatch.DataValueField = "batch_year";

        ddlbatch.DataBind();

    }



    public void clear()
    {

        ddlCollege.SelectedIndex = 0;
        ddlbranch.SelectedIndex = 0;
        ddldegree.SelectedIndex = 0;
        ddlbatch.SelectedIndex = 0;
        //Radioformat1.Checked = false;
        //Radioformat2.Checked = false;
        grdsubjectDetails.DataSource = null;
        grdsubjectDetails.DataBind();


        GridView1.DataSource = null;
        GridView1.DataBind();

        Div2.Visible = false;
        Div1.Visible = false;
        div.Visible = false;








    }
    public void load()
    {
        DropDownList1.SelectedIndex = 0;
        DropDownList2.SelectedIndex = 0;
        DropDownList3.SelectedIndex = 0;


    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }


    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //divFormat1.Visible = false;
            //divFormat2.Visible = false;
            //chkReport.Checked = false;

            binddegree();
            bindbranch();


        }
        catch (Exception ex)
        {
        }
    }

    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            txt.Text = deft;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }

    private void checkBoxListselectOrDeselect(CheckBoxList cbl, bool selected = true)
    {
        try
        {
            foreach (wc.ListItem li in cbl.Items)
            {
                li.Selected = selected;
            }
        }
        catch
        {
        }
    }



    protected void ddlbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {



            binddegree();
            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }



    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            bindbranch();

        }
        catch (Exception ex)
        {
        }
    }



    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {




        }
        catch (Exception ex)
        {
        }
    }




    protected void cblBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //divFormat1.Visible = false;
            //divFormat2.Visible = false;
            //chkReport.Checked = false;

            binddegree();
            bindbranch();


        }
        catch (Exception ex)
        {
        }
    }




    public void binddegree()
    {
        try
        {
            ds.Clear();

            string batchCode = string.Empty;


            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();

            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            string valBatch = ddlbatch.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(collegeCode))
            {
                string selDegree = "SELECT DISTINCT c.course_id,c.course_name,c.Priority,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') " + columnfield + " ORDER BY CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selDegree, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddldegree.DataSource = ds;
                ddldegree.DataTextField = "course_name";
                ddldegree.DataValueField = "course_id";
                ddldegree.DataBind();


            }
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {
            string degreecode = string.Empty;


            ds.Clear();
            collegeCode = string.Empty;
            if (ddlCollege.Items.Count > 0)
                collegeCode = ddlCollege.SelectedValue.ToString().Trim();
            string selBranch = string.Empty;
            string columnfield = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["single_user"] != null && (Convert.ToString(Session["single_user"]) != "1" && Convert.ToString(Session["single_user"]) != "true" && Convert.ToString(Session["single_user"]) != "TRUE" && Convert.ToString(Session["single_user"]) != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null)
            {
                columnfield = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }


            string valBatch = ddlbatch.SelectedValue.ToString();
            string valDegree = ddldegree.SelectedValue.ToString();







            if (!string.IsNullOrEmpty(collegeCode) && !string.IsNullOrEmpty(valBatch) && !string.IsNullOrEmpty(valDegree))
            {
                selBranch = "SELECT DISTINCT dg.Degree_Code,dt.Dept_Name,CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END OrderBy FROM Degree dg,Course c,Department dt,DeptPrivilages dp,Registration r WHERE r.degree_code = dg.Degree_Code AND dp.degree_code = dg.Degree_Code AND dg.Course_Id = c.Course_Id AND dg.Dept_Code = dt.Dept_Code AND r.college_code = c.college_code AND r.college_code = dg.college_code AND dt.college_code = r.college_code AND c.college_code = dg.college_code AND r.CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' AND r.college_code in('" + collegeCode + "') AND r.Batch_Year in('" + valBatch + "') AND c.Course_Id in('" + valDegree + "') " + columnfield + " ORDER BY dg.Degree_Code, CASE WHEN c.Priority IS NULL THEN c.Course_Id ELSE c.Priority END ";
                ds = da.select_method_wo_parameter(selBranch, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddlbranch.DataSource = ds;
                ddlbranch.DataTextField = "dept_name";
                ddlbranch.DataValueField = "degree_code";
                ddlbranch.DataBind();

            }

        }
        catch (Exception ex)
        {

        }
    }
    public void Bindcollege()
    {
        try
        {
            ddlCollege.Items.Clear();
            dtCommon.Clear();
            ddlCollege.Enabled = false;
            DataSet dsprint = new DataSet();
            dsprint.Clear();
            string qryUserCodeOrGroupCode = string.Empty;
            string group_user = ((Session["group_code"] != null) ? Convert.ToString(Session["group_code"]) : string.Empty);
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]);
            }
            if ((Convert.ToString(group_user).Trim() != "") && Session["group_code"] != null && Session["single_user"] != null && Convert.ToString(Session["single_user"]).Trim() != "1" && Convert.ToString(Session["single_user"]).Trim().ToLower() != "true")
            {
                qryUserCodeOrGroupCode = " and group_code='" + group_user + "'";
            }
            else if (Session["usercode"] != null && !string.IsNullOrEmpty(Convert.ToString(Session["usercode"]).Trim()))
            {
                qryUserCodeOrGroupCode = " and user_code='" + Convert.ToString(Session["usercode"]).Trim() + "'";
            }
            if (!string.IsNullOrEmpty(qryUserCodeOrGroupCode))
            {
                dicQueryParameter.Clear();
                dicQueryParameter.Add("column_field", Convert.ToString(qryUserCodeOrGroupCode));
                dtCommon = storeAcc.selectDataTable("bind_college", dicQueryParameter);
            }

            if (dtCommon.Rows.Count > 0)
            {
                ddlCollege.DataSource = dtCommon;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
                ddlCollege.SelectedIndex = 0;
                ddlCollege.Enabled = true;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void religrid_Onrowdatabound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int count = e.Row.Cells.Count;


                int rowvalue = e.Row.RowIndex;
                string id = e.Row.ClientID;
                string value = (e.Row.FindControl("lblreligcode") as Label).Text;
                string selectquery = "select priority  from admitcolumnset where column_name='" + value + "'  and textcriteria ='relig' and user_code ='" + userCode + "'  and college_code ='" + collegeCode + "'";
                ds1 = dt.select_method_wo_parameter(selectquery, "Text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string priority = Convert.ToString(ds1.Tables[0].Rows[0]["priority"]);
                    if (priority != "")
                    {
                        (e.Row.FindControl("txt_percentageornumber") as TextBox).Text = priority;
                    }
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void Radioformat1_CheckedChanged(object sender, EventArgs e)
    {
        //religiondiv.Visible = true;

        //vocinaldiv.Visible = false;
    }

    protected void Radioformat2_CheckedChanged(object sender, EventArgs e)
    {
        //    religiondiv.Visible = true;

        //    vocinaldiv.Visible = false;


    }

    protected void bindsubject()
    {
        string batchquery = string.Empty;
        batchquery = "select textval from textvaltable where TextCriteria='subje' and textval<>'' and textval<>'---Select---'   and college_code ='" + ddlCollege.SelectedItem.Value + "' ";
        //ds.Clear();
        ds = da.select_method_wo_parameter(batchquery, "Text");



        if (ds.Tables.Count > 0)
        {
            grdsubjectDetails.DataSource = ds;
            grdsubjectDetails.DataBind();
        }
        else
        {
            grdsubjectDetails.DataSource = null;
            grdsubjectDetails.DataBind();
        }


    }

    protected void bindsubject2()
    {

        string batchquery = string.Empty;
        batchquery = "select textval,TextCode from textvaltable where TextCriteria='subje' and textval<>'' and textval<>'---Select---' and college_code ='" + ddlCollege.SelectedItem.Value + "' ";
        //ds.Clear();
        ds = da.select_method_wo_parameter(batchquery, "Text");



        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DropDownList1.DataSource = ds;
            DropDownList1.DataTextField = "textval";
            DropDownList1.DataValueField = "TextCode";
            DropDownList1.DataBind();

        }

        else
        {

        }


    }

    public void bindcommunity()
    {






        string batchquery = string.Empty;

        //select  MAX (TextCode),textval from textvaltable where TextCriteria='comm' group by textval
        batchquery = "select  MAX (TextCode),textval from textvaltable where TextCriteria='comm' and textval<>''  and college_code ='" + ddlCollege.SelectedItem.Value + "'  group by textval ";
        //ds.Clear();
        ds = da.select_method_wo_parameter(batchquery, "Text");



        if (ds.Tables.Count > 0)
        {
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }
        else
        {
            GridView1.DataSource = null;
            GridView1.DataBind();
        }


    }


    protected void btnsave_Click(object sender, EventArgs e)
    {





        string str = string.Empty;
        string strname = string.Empty;
        foreach (GridViewRow gvrow in grdsubjectDetails.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect");
            if (chk != null & chk.Checked)
            {
                str += "" + gvrow.Cells[1].Text + ", ";

                str += "";
            }
        }


        txtsubject.Text = str;
        la.Text = str;
        droup.Visible = true;
        community();
        value_Click();



    }

    public void community()
    {
        string str = string.Empty;
        string str2 = string.Empty;
        string strname = string.Empty;
        DAccess2 d2 = new DAccess2();
        string batchquery = string.Empty;
        foreach (GridViewRow gvrow in GridView1.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect2");
            if (chk != null & chk.Checked)
            {
                //str += "" + gvrow.Cells[1].Text + "";


                str += d2.GetFunction("select  MAX (TextCode) from textvaltable where TextCriteria='comm' and textval<>''  and college_code ='" + ddlCollege.SelectedItem.Value + "' and textval='" + gvrow.Cells[1].Text + "' ");
                //ds.Clear();


                str += "=";
                str += (gvrow.FindControl("TextBox1") as TextBox).Text;
                str += "/";



            }
        }



        txtcommunity.Text = str;






    }

    public void inservalue()
    {
        DAccess2 dacc = new DAccess2();
        string batchquery = string.Empty;


        string collegeCode = ddlCollege.SelectedItem.Value;
        string isvocational = Radioformat1.Checked ? "1" : "0";

        string batch = ddlbatch.SelectedItem.Value;

        string degree = ddlbranch.SelectedItem.Value;




        if (txtsubject.Text == "" && txtcommunity.Text == "")
        {
            lblAlertMsg.Visible = true;
            lblAlertMsg.Text = "Enter The value of subject and community";
            divPopAlert.Visible = true;

            txtsubject.Visible = false;
            txtcommunity.Visible = false;
        }
        string sql = "if exists(select  * from stu_eligiblemaster where deg_code='" + degree + "' and isVocational='" + isvocational + "' and batch_year='" + batch + "' ) update stu_eligiblemaster set colleg_code='" + collegeCode + "',batch_year='" + batch + "',deg_code='" + degree + "',isVocational='" + isvocational + "',subject='" + txtsubject.Text + "',Community='" + txtcommunity.Text + "',formula='" + foumla.Text + "',fomulavalue='" + TextBox2.Text + "' where batch_year='" + batch + "' and isVocational='" + isvocational + "' and colleg_code='" + collegeCode + "'else insert into stu_eligiblemaster(colleg_code ,batch_year ,deg_code,isVocational,subject , Community ,formula,fomulavalue ) values('" + collegeCode + "','" + batch + "','" + degree + "','" + isvocational + "','" + txtsubject.Text + "','" + txtcommunity.Text + "','" + foumla.Text + "','" + TextBox2.Text + "') ";
        dacc.update_method_wo_parameter(sql, "text");
        //int result = d2.update_method_wo_parameter(splhr_query_master, "text");


        ddlbatch.SelectedIndex = 0;
        ddldegree.SelectedIndex = 0;
        ddlbranch.SelectedIndex = 0;
        ddlCollege.SelectedIndex = 0;





    }
    public void value_Click()
    {
        lblAlertMsg.Visible = true;
        lblAlertMsg.Text = "The Entered Value are.......";
        divPopAlert.Visible = true;

    }



    protected void btnPopAlertClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;

            divPopAlert.Visible = false;

            inservalue();
            //div.Visible = false;
            //Radioformat1.Checked = false;
            //Radioformat2.Checked = false;




        }

        catch (Exception ex)
        {

        }
    }


    protected void btnPopAlertcancel_Click(object sender, EventArgs e)
    {
        try
        {

            lblAlertMsg.Visible = false;

            divPopAlert.Visible = false;



        }

        catch (Exception ex)
        {

        }
    }


    protected void btnMissingStudent_Click(object sender, EventArgs e)
    {

        if (!Radioformat1.Checked && !Radioformat2.Checked)
        {

            errormsg.Visible = true;
            errormsg.Text = "Select any one of the Vocational value.";

        }
        else
        {
            Div2.Visible = true;
            Div1.Visible = true;
            div.Visible = true;
            droup.Visible = true;
            bindsubject();
            bindcommunity();
            updatesubject();
            updatecomunity();
            fomu();


        }



    }
    protected void btnPopAlertClose1_Click(object sender, EventArgs e)
    {
        try
        {
            lblAlertMsg.Text = string.Empty;
            divPopAlert.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
    {

        string str = string.Empty;
        string str2 = string.Empty;
        string strname = string.Empty;
        foreach (GridViewRow gvrow in GridView1.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect2");
            if (chk != null & chk.Checked)
            {
                str += "" + gvrow.Cells[1].Text + "";

                str += "=";
                str += (gvrow.FindControl("TextBox1") as TextBox).Text;
                str += "/";
            }
        }
        Label1.Text = str;




    }
    protected void grdsubjectDetails_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        k1 = TextBox2.Text + DropDownList1.SelectedItem.Text;
        k = foumla.Text + DropDownList1.SelectedItem.Value;
        foumla.Text = k;
        TextBox2.Text = k1;
        load();
    }
    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        k1 = TextBox2.Text + ";" + DropDownList2.SelectedItem.Text + ";";
        k = foumla.Text + ";" + DropDownList2.SelectedItem.Text + ";";
        foumla.Text = k;
        TextBox2.Text = k1;
        load();
    }
    protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
    {
        k1 = TextBox2.Text + DropDownList3.SelectedItem.Text;
        k = foumla.Text + DropDownList3.SelectedItem.Text;
        foumla.Text = k;
        TextBox2.Text = k1;
        load();
    }


    protected void clrfomula(object sender, EventArgs e)
    {
        foumla.Text = "";
        TextBox2.Text = "";
    }


    public void clrtext()
    {
        Clr.Text = " ";
    }

    public void updatecomunity()
    {
        string batchquery = string.Empty;
        string collegeCode = ddlCollege.SelectedItem.Value;
        string isvocational = Radioformat1.Checked ? "1" : "0";

        string batch = ddlbatch.SelectedItem.Value;

        string degree = ddlbranch.SelectedItem.Value;
        string str = string.Empty;
        batchquery = d2.GetFunction("select community from stu_eligiblemaster  where colleg_code='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");
        string[] kc = batchquery.Split('/');
        for (int i = 0; i < kc.Length; i++)
        {
            string[] k1 = kc[i].Split('=');

            for (int ii = 0; ii < k1.Length; ii++)
            {
                string vcomunity = k1[ii];
                string namecom = d2.GetFunction("select  textval  from textvaltable where TextCriteria='comm' and textval<>''  and college_code ='" + ddlCollege.SelectedItem.Value + "' and TextCode='" + vcomunity + "' ");
                foreach (GridViewRow gvrow in GridView1.Rows)
                {


                    if (namecom == gvrow.Cells[1].Text)
                    {
                        CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect2");
                        chk.Checked = true;


                        (gvrow.FindControl("TextBox1") as TextBox).Text = k1[1];

                    }

                }
            }



        }

    }

    public void updatesubject()
    {
        string batchquery = string.Empty;
        string collegeCode = ddlCollege.SelectedItem.Value;
        string isvocational = Radioformat1.Checked ? "1" : "0";

        string batch = ddlbatch.SelectedItem.Value;

        string degree = ddlbranch.SelectedItem.Value;
        string str = string.Empty;
        batchquery = d2.GetFunction("select formula from stu_eligiblemaster  where colleg_code ='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");

        //batchquery = d2.GetFunction("select subject from stu_eligiblemaster  where colleg_code ='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");

        string[] kc = batchquery.Split('/', ';', ',');
        for (int i = 0; i < kc.Length; i++)
        {
            string[] k1 = kc[i].Split('+');

            for (int ii = 0; ii < k1.Length; ii++)
            {
                string vcomunity = k1[ii];
                string namecom = d2.GetFunction("select  textval  from textvaltable where TextCriteria='subje' and textval<>''and textval<>' 	---Select---'  and college_code ='" + ddlCollege.SelectedItem.Value + "' and TextCode='" + vcomunity + "' ");

                foreach (GridViewRow gvrow in grdsubjectDetails.Rows)
                {
                    //string subb =.ToUpper();
                    if ((namecom == gvrow.Cells[1].Text))
                    //if ((vcomunity == gvrow.Cells[1].Text))
                    {
                        CheckBox chk = (CheckBox)gvrow.FindControl("cbSelect");
                        chk.Checked = true;



                    }

                }
            }
        }







    }

    public void fomu()
    {
        string batchquery = string.Empty;
        string collegeCode = ddlCollege.SelectedItem.Value;
        string isvocational = Radioformat1.Checked ? "1" : "0";

        string batch = ddlbatch.SelectedValue.ToString();

        string degree = ddlbranch.SelectedValue.ToString();
        string str = string.Empty;
        batchquery = d2.GetFunction("select formula from stu_eligiblemaster  where colleg_code ='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");

        foumla.Text = batchquery;



        batchquery = d2.GetFunction("select fomulavalue from stu_eligiblemaster  where colleg_code ='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");


        TextBox2.Text = batchquery;



        batchquery = d2.GetFunction("select fomulavalue from stu_eligiblemaster  where colleg_code ='" + collegeCode + "' and batch_year='" + batch + "' and isVocational='" + isvocational + "'");



    }



}























