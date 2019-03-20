using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
public partial class applno_generation_settings : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string educationlevel = "";
    string usercode = string.Empty;
    static string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
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
            //****************************************************//
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Session["college_code"] = collegecode1;
            if (!IsPostBack)
            {
                setLabelText();

                sub.Visible = true;
                bindcollege();
                bindtype();
                degreebind();
                Session["educationlevel"] = null;
                Session["educationlevel"] = d2.GetFunction("select value from Master_Settings where settings='Education level Rights' and usercode='" + Session["college_code"].ToString() + "'");
            }
        }
        catch (Exception ex) { }
    }
    
    public void bindcollege()
    {
        try
        {
            string religionquery = "  select collname ,college_code  from collinfo ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(religionquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds.Tables[0];
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();

            }
            if (ds.Tables[0].Rows.Count == 1)
            {
                ddlcollege.Enabled = false;
            }
            else
            {
                ddlcollege.Enabled = true;
            }
        }
        catch
        {
        }
    }

    public void bindtype()
    {
        try
        {
            // Session["college_code"] = ddlcollege.SelectedItem.Value;
            string typequery = "select distinct type  from course where college_code =" + Session["college_code"] + "";
            typequery = " select collname ,college_code  from collinfo ";
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                typegrid.DataSource = ds;
                typegrid.DataBind();
                typegrid.Visible = true;
            }
        }
        catch
        {

        }
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
        catch
        {

        }
    }
    
    protected void typegrid_SelectedIndexChanege(object sender, EventArgs e)
    {
        try
        {
            string type = Convert.ToString((typegrid.SelectedRow.FindControl("typelnk") as LinkButton).Text);
            // string click = Convert.ToString((typegrid.SelectedRow.FindControl("lnk_typeinstruction") as LinkButton).Text);

            Session["type"] = type;

            string educationlev = "";
            if (Convert.ToString(Session["educationlevel"]).Trim() != "")
            {
                string[] e_level = Convert.ToString(Session["educationlevel"]).Split(',');
                if (e_level.Length > 0)
                {
                    for (int i = 0; i < e_level.Length; i++)
                    {
                        if (educationlev.Trim() == "")
                        {
                            educationlev = e_level[i];
                        }
                        else
                        {
                            educationlev = educationlev + "','" + e_level[i];
                        }
                    }
                }
            }
            string query = "select distinct Edu_Level  from course where type='" + type + "' and Edu_Level in('" + educationlev + "')   and college_code=" + Session["college_code"] + " order by Edu_Level desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                grid_edulevel.DataSource = ds;
                grid_edulevel.DataBind();

                coursediv.Visible = true;
                sub2.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void marks_rowdatabound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //e.Row.Cells[5].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grid_edulevel, "Generate$" + e.Row.RowIndex);
            //e.Row.Cells[6].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grid_edulevel, "Execl$" + e.Row.RowIndex);
            // e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grid_edulevel, "instruction$" + e.Row.RowIndex);
        }
    }
    
    protected void OnDataBound(object sender, EventArgs e)
    {
        for (int i = Course_gird.Rows.Count - 1; i > 0; i--)
        {
            GridViewRow row = Course_gird.Rows[i];
            GridViewRow previousRow = Course_gird.Rows[i - 1];
            for (int j = 1; j <= 1; j++)
            {
                Label lnlname = (Label)row.FindControl("lblcoursename");
                Label lnlname1 = (Label)previousRow.FindControl("lblcoursename");

                if (lnlname.Text == lnlname1.Text)
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

        if (Course_gird.Rows.Count > 0)
        {
            Hashtable hat = new Hashtable();
            hat.Add(1, "0");
            hat.Add(2, "00");
            hat.Add(3, "000");
            hat.Add(4, "0000");
            hat.Add(5, "00000");
            hat.Add(6, "000000");
            hat.Add(7, "0000000");
            hat.Add(8, "00000000");
            hat.Add(9, "000000000");
            hat.Add(10, "0000000000");
            string app_adm = "";
            for (int row = 0; row < Course_gird.Rows.Count; row++)
            {
                if (rdb_applicationno.Checked == true)
                {
                    app_adm = "0";
                }
                else
                {
                    app_adm = "1";
                }
                string genType = "0";
                if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
                {
                    genType = "3";

                }
                else if (rbSelMode.SelectedIndex == 1)
                {
                    genType = "1";
                }
                else if (rbSelMode.SelectedIndex == 2)
                {
                    genType = "2";
                }
                string degree_code = (Course_gird.Rows[row].FindControl("lbldegreecode") as Label).Text;
                string generatequery = "select appcode,app_startwith,app_serial from code_generation where batch_year =" + DateTime.Now.Year + " and degree_code ='" + degree_code + "' and college_code =" + Session["college_code"] + " and app_code_flag='" + app_adm + "' and isnull(cg_generationType,0)='" + genType + "' ";

                if (cbSeatType.Checked)
                {
                    string mode = (Course_gird.Rows[row].FindControl("lblMode") as Label).Text;
                    string textcode = (Course_gird.Rows[row].FindControl("lbltextcode") as Label).Text;

                    generatequery = "select appcode,app_startwith,app_serial from code_generation where batch_year =" + DateTime.Now.Year + " and degree_code ='" + degree_code + "' and college_code =" + Session["college_code"] + " and app_code_flag='" + app_adm + "'  and cb_mode='" + mode + "' and cg_seattype='" + textcode + "' and isnull(cg_generationType,0)='" + genType + "'";

                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(generatequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                    int len = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]).Length;
                    string start_number = "";
                    if (len == Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]))
                    {
                        start_number = start_with;
                    }
                    else
                    {
                        int remain = Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]) - len;
                        string addnumber = Convert.ToString(hat[remain]);
                        start_number = addnumber + "" + start_with;
                    }
                    (Course_gird.Rows[row].FindControl("lblnotgenerate") as Label).Text = Convert.ToString(ds.Tables[0].Rows[0]["appcode"]) + "" + start_number;
                    (Course_gird.Rows[row].FindControl("lblnotgenerate") as Label).ForeColor = System.Drawing.Color.Black;
                    (Course_gird.Rows[row].FindControl("lnkapply") as LinkButton).Text = "Re-Generate";
                }
            }
        }
    }

    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[4].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(Course_gird, "Select$" + e.Row.RowIndex);

            //DropDownList ddlRegMode = (DropDownList)e.Row.FindControl("ddlRegMode");
            //DropDownList ddlLateMode = (DropDownList)e.Row.FindControl("ddlLateMode");
            //DropDownList ddlTransMode = (DropDownList)e.Row.FindControl("ddlTransMode");
            //DataSet dsMode = d2.select_method_wo_parameter("select TEXTVAL,Textcode from TextValTable where TextCriteria='seat'  and college_code='" + Session["college_code"].ToString() + "' ", "TEXT");
            //if (dsMode.Tables.Count > 0 && dsMode.Tables[0].Rows.Count > 0)
            //{
            //    ddlRegMode.DataSource = dsMode.Tables[0];
            //    ddlRegMode.DataTextField = "TEXTVAL";
            //    ddlRegMode.DataValueField = "Textcode";
            //    ddlRegMode.DataBind();

            //    ddlLateMode.DataSource = dsMode.Tables[0];
            //    ddlLateMode.DataTextField = "TEXTVAL";
            //    ddlLateMode.DataValueField = "Textcode";
            //    ddlLateMode.DataBind();

            //    ddlTransMode.DataSource = dsMode.Tables[0];
            //    ddlTransMode.DataTextField = "TEXTVAL";
            //    ddlTransMode.DataValueField = "Textcode";
            //    ddlTransMode.DataBind();
            //}

        }
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.Cells[1].Text = lblscltype.Text;
            e.Row.Cells[2].Text = lblstandard.Text;
        }
    }
    
    protected void course_gird_SelectedIndexChanege(object sender, EventArgs e)
    {
        try
        {
            lblclgacr.Text = "";
            lbldeptacr.Text = "";
            lblclgacr.Text = lblclgs.Text + " Acronym";
            lbldeptacr.Text = lbldept.Text + " Acronym";

            txt_perviousdate.Enabled = false;
            string course_name = Convert.ToString((Course_gird.SelectedRow.FindControl("lblcoursename") as Label).Text);
            string degree_name = Convert.ToString((Course_gird.SelectedRow.FindControl("lbldeptname") as Label).Text);
            string degree_code = Convert.ToString((Course_gird.SelectedRow.FindControl("lbldegreecode") as Label).Text);
            string depart_code = Convert.ToString((Course_gird.SelectedRow.FindControl("lbldeptcode") as Label).Text);
            string seattextcode = Convert.ToString((Course_gird.SelectedRow.FindControl("lbltextcode") as Label).Text);
            string seatmode = Convert.ToString((Course_gird.SelectedRow.FindControl("lblMode") as Label).Text);
            Session["dept_code"] = Convert.ToString(depart_code);
            Session["degree_Code"] = Convert.ToString(degree_code);
            txt_batch.Text = System.DateTime.Now.ToString("yyyy");
            // txt_batch.Attributes.Add("readonly", "readonly");
            txt_degree.Attributes.Add("readonly", "readonly");
            txt_branch.Attributes.Add("readonly", "readonly");
            txt_perviousdate.Attributes.Add("readonly", "readonly");

            if (course_name.Trim() != "")
            {
                txt_degree.Text = course_name.ToString();
            }
            else
            {
                txt_degree.Text = "";
            }
            if (degree_name.Trim() != "")
            {
                txt_branch.Text = degree_name.ToString();
            }
            else
            {
                txt_branch.Text = "";
            }
            txt_modifydate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txt_modifydate.Attributes.Add("readonly", "readonly");
            txt_collegeacr.Text = "";
            txt_deptacr.Text = "";
            txt_otheracr.Text = "";
            lblSeatMode.Text = "";
            lblSeatTextcode.Text = "";
            txt_serialstartwith.Text = "";
            txt_serialsize.Text = "";
            txt_perviousdate.Text = "";
            cbcollegeacr.Checked = false;
            cbdeptacr.Checked = false;
            cbothracr.Checked = false;

            string select_query = "select batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,CONVERT(varchar, app_modifydate,103)as date,  cb_mode, cg_seattype from code_generation where college_code=" + Session["college_code"] + " and degree_code =" + Session["degree_Code"] + "";
            if (cbSeatType.Checked)
            {
                select_query = "select batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,CONVERT(varchar, app_modifydate,103)as date,  cb_mode, cg_seattype from code_generation where college_code=" + Session["college_code"] + " and degree_code =" + Session["degree_Code"] + "   and cb_mode='" + seatmode + "' and cg_seattype='" + seattextcode + "' ";

                lblSeatMode.Text = seatmode;
                lblSeatTextcode.Text = seattextcode;
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                string college_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_acr"]);
                string dept_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_dept_acr"]);
                string other_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_other_acr"]);
                string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                string count = Convert.ToString(ds.Tables[0].Rows[0]["app_serial"]);
                string deg_code = Convert.ToString(ds.Tables[0].Rows[0]["degree_code"]);
                string date = Convert.ToString(ds.Tables[0].Rows[0]["date"]);

                //lblSeatMode.Text = Convert.ToString(ds.Tables[0].Rows[0]["cb_mode"]);
                //lblSeatTextcode.Text = Convert.ToString(ds.Tables[0].Rows[0]["cg_seattype"]);

                if (college_acr.Trim() != "" && college_acr.Trim() != null)
                {
                    txt_collegeacr.Text = college_acr.ToString();
                    cbcollegeacr.Checked = true;
                }
                else
                {
                    txt_collegeacr.Text = "";
                    cbcollegeacr.Checked = false;
                }

                if (dept_acr.Trim() != "" && dept_acr.Trim() != null)
                {
                    txt_deptacr.Text = dept_acr.ToString();
                    cbdeptacr.Checked = true;
                }
                else
                {
                    txt_deptacr.Text = "";
                    cbdeptacr.Checked = false;
                }

                if (other_acr.Trim() != "" && other_acr.Trim() != null)
                {
                    txt_otheracr.Text = other_acr.ToString();
                    cbothracr.Checked = true;
                    txt_otheracr.Enabled = true;
                }
                else
                {
                    txt_otheracr.Text = "";
                    cbothracr.Checked = false;
                }

                if (start_with.Trim() != "" && start_with.Trim() != null)
                {
                    txt_serialstartwith.Text = start_with.ToString();
                }
                else
                {
                    txt_serialstartwith.Text = "";
                }

                if (count.Trim() != "" && count.Trim() != null)
                {
                    txt_serialsize.Text = count.ToString();
                }
                else
                {
                    txt_serialsize.Text = "";
                }

                if (date.Trim() != "" && date.Trim() != null)
                {
                    txt_perviousdate.Text = date.ToString();
                    txt_perviousdate.Enabled = false;
                }
                else
                {
                    txt_perviousdate.Text = "";

                }

            }
            //mpemsgboxdelete.Show();
            step2.Visible = true;

        }
        catch
        {

        }

    }

    protected void Generate_Click(object sender, EventArgs e)
    {
        try
        {


            string degreecode = Convert.ToString(Session["degree_code"]);
            string college_code = Convert.ToString(Session["college_code"]);
            string seatmode = lblSeatMode.Text.Trim();
            string seatTextcode = lblSeatTextcode.Text.Trim();
            if (!cbSeatType.Checked)
            {
                seatmode = "0";
                seatTextcode = "0";
            }

            string batch_year = Convert.ToString(txt_batch.Text);
            string college_acr = Convert.ToString(txt_collegeacr.Text);
            string dept_acr = Convert.ToString(txt_deptacr.Text);
            string other_acr = Convert.ToString(txt_otheracr.Text);
            string start_with = Convert.ToString(txt_serialstartwith.Text);
            string count = Convert.ToString(txt_serialsize.Text);
            string date = Convert.ToString(txt_modifydate.Text);
            string[] split_date = date.Split('/');
            DateTime da1 = Convert.ToDateTime(split_date[1] + "/" + split_date[0] + "/" + split_date[2]);
            string concateacr = college_acr + "" + dept_acr + "" + other_acr;
            string app_adm = "0";
            if (rdb_applicationno.Checked == true)
            {
                app_adm = "0";
            }
            else
            {
                app_adm = "1";
            }
            string genType = "0";
            if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
            {
                genType = "3";
                app_adm = "3";
            }
            else if (rbSelMode.SelectedIndex == 1)
            {
                genType = "1";
            }
            else if (rbSelMode.SelectedIndex == 2)
            {
                genType = "2";
            }
            if (start_with.Trim() != "" && count.Trim() != "")
            {
                string updatequery = "";

                updatequery = "if not exists (select * from code_generation where college_code='" + college_code + "' and degree_code ='" + degreecode + "' and app_code_flag='" + app_adm + "'  and isnull(cb_mode,0)='" + seatmode + "' and isnull(cg_seattype,'0')='" + seatTextcode + "'  and isnull(cg_generationType,0)='" + genType + "')";
                updatequery = updatequery + " insert into code_generation(batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,app_modifydate,app_code_flag,appcode,college_code,cb_mode,cg_seattype, cg_generationType)";
                updatequery = updatequery + " values ('" + batch_year + "','" + college_acr + "','" + dept_acr + "','" + other_acr + "','" + start_with + "','" + count + "','" + degreecode + "','" + da1.ToString("MM/dd/yyyy") + "','" + app_adm + "','" + concateacr + "','" + college_code + "','" + seatmode + "','" + seatTextcode + "','" + genType + "')";
                updatequery = updatequery + " else";
                updatequery = updatequery + " update code_generation set batch_year ='" + batch_year + "' ,app_acr ='" + college_acr + "' ,app_dept_acr ='" + dept_acr + "' ,app_other_acr='" + other_acr + "',app_code_flag='" + app_adm + "',appcode='" + concateacr + "',app_startwith='" + start_with + "',app_serial='" + count + "',app_modifydate='" + da1.ToString("MM/dd/yyyy") + "' where college_code='" + college_code + "' and degree_code ='" + degreecode + "' and app_code_flag='" + app_adm + "'    and isnull(cb_mode,0)='" + seatmode + "' and isnull(cg_seattype,'0')='" + seatTextcode + "'  and isnull(cg_generationType,0)='" + genType + "'";


                int insert = d2.update_method_wo_parameter(updatequery, "Text");
                if (insert != 0)
                {
                    grid_edulevel_SelectedIndexChanege(sender, e);
                    if (rdb_applicationno.Checked == true)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Application Number Generated Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Admission Number Generated Successfully')", true);
                    }
                }
                //  mpemsgboxdelete.Hide();
            }
            else
            {
                // mpemsgboxdelete.Show();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Fill Required Fields')", true);
            }
        }
        catch
        {

        }
    }
    
    protected void cbcollegeacr_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbcollegeacr.Checked == true)
            {
                string query = "select Coll_acronymn  from collinfo where college_code=" + Session["college_code"] + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string collacr = Convert.ToString(ds.Tables[0].Rows[0]["Coll_acronymn"]);
                    if (collacr.Trim() != "")
                    {
                        txt_collegeacr.Text = collacr.ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                }
            }
            else
            {
                txt_collegeacr.Text = "";
            }


        }
        catch
        {

        }
    }
    
    protected void cbdeptacr_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbdeptacr.Checked == true)
            {
                string query = "select dept_acronym   from Department  where college_code =" + Session["college_code"] + " and Dept_Code=" + Session["dept_code"] + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string collacr = Convert.ToString(ds.Tables[0].Rows[0]["dept_acronym"]);
                    if (collacr.Trim() != "")
                    {
                        txt_deptacr.Text = collacr.ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                }
            }
            else
            {
                txt_deptacr.Text = "";
            }
        }
        catch
        {
        }

    }
    
    protected void cbothracr_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbothracr.Checked == true)
            {
                txt_otheracr.Enabled = true;
            }
            else
            {
                txt_otheracr.Enabled = false;
            }
        }
        catch
        {

        }
    }
    
    protected void Cancel_Click(object sender, EventArgs e)
    {
        try
        {
            //mpemsgboxdelete.Hide();
            step2.Visible = false;
        }
        catch
        {
        }
    }

    protected void Showreport_Changed(object sender, EventArgs e)
    {
        try
        {
            if (showreport.Checked == true)
            {
                string colval = string.Empty;
                if (rbSelMode.SelectedIndex == 1)
                    colval = " and isnull(iscollege,'0')='1'";
                if (rbSelMode.SelectedIndex == 2)
                    colval = " and isnull(iscollege,'0')='2'";

                string app_adm = "";
                if (rdb_applicationno.Checked == true)
                {
                    app_adm = "0";
                }
                else
                {
                    app_adm = "1";
                }
                string genType = "0";
                if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
                {
                    genType = "3";
                }
                else if (rbSelMode.SelectedIndex == 1)
                {
                    genType = "1";
                }
                else if (rbSelMode.SelectedIndex == 2)
                {
                    genType = "2";
                }
                string selectquery = "";
                if (colval != "")
                {
                    selectquery = " select cd.appcode,cd.app_startwith,cd.app_serial,cd.app_acr,''Edu_Level,''coursename ,cd.app_dept_acr,cd.app_other_acr,CONVERT(varchar,cd.app_modifydate,103)as Modifydate,collname as type from code_generation cd,collinfo c  where  cd.college_code=c.college_code   and cd.app_code_flag='" + app_adm + "' and cd.college_code=" + Session["college_code"] + " " + colval + "  and isnull(cg_generationType,0)='" + genType + "' order by collname";
                    if (rbSelMode.SelectedIndex == 2)
                    {
                        selectquery = " select cd.appcode,cd.app_startwith,cd.app_serial,cd.app_acr,EduLevel as Edu_Level ,''coursename ,cd.app_dept_acr,cd.app_other_acr,CONVERT(varchar,cd.app_modifydate,103)as Modifydate, collname as type from code_generation cd left join collinfo c  on  cd.college_code=c.college_code and cd.app_code_flag= '" + app_adm + "'  " + colval + "  and isnull(cg_generationType,0)='" + genType + "' order by EduLevel desc";
                    }
                }
                else
                {
                    selectquery = "select c.type as t,c.Edu_Level,(c.Course_Name+'-'+dt.Dept_Name)as coursename,cd.appcode,cd.app_startwith,cd.app_serial,cd.app_acr ,cd.app_dept_acr,cd.app_other_acr,CONVERT(varchar,cd.app_modifydate,103)as Modifydate,collname as type from code_generation cd,course c,Degree d,Department dt,collinfo ci  where cd.degree_code=d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id=c.Course_Id and cd.college_code=c.college_code and c.college_code=d.college_code and d.college_code =cd.college_code and d.college_code=dt.college_code and ci.college_code = dt.college_code  and cd.app_code_flag='" + app_adm + "' and cd.college_code=" + Session["college_code"] + "  and isnull(cg_generationType,0)='" + genType + "' order by c.type ,coursename,collname";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ReportGrid.DataSource = ds;
                    ReportGrid.DataBind();
                    ReportGrid.Visible = true;
                    Reportstep.Visible = true;
                    divrpt.Visible = true;
                }
            }
            else
            {
                ReportGrid.Visible = false;
                Reportstep.Visible = false;
                divrpt.Visible = false;
            }
        }
        catch
        {
        }
    }

    protected void report_Databound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            int row = e.Row.RowIndex;
            if (row % 2 == 0)
            {
                e.Row.BackColor = System.Drawing.Color.LightBlue;
            }
            //e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(typegrid, "Select$" + e.Row.RowIndex);
            //e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(typegrid, "instruction$" + e.Row.RowIndex);


        }
    }
    
    protected void export_Click(object sender, EventArgs e)
    {
        ExportGridToPDF();
    }
    
    private void ExportGridToPDF()
    {

        //Response.ContentType = "application/pdf";
        //Response.AddHeader("content-disposition", "attachment;filename=Vithal_Wadje.pdf");
        //Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //StringWriter sw = new StringWriter();
        //HtmlTextWriter hw = new HtmlTextWriter(sw);
        //ReportGrid.RenderControl(hw);
        //StringReader sr = new StringReader(sw.ToString());
        //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
        //PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
        //pdfDoc.Open();
        //htmlparser.Parse(sr);
        //pdfDoc.Close();
        //Response.Write(pdfDoc);
        //Response.End();
        //ReportGrid.AllowPaging = true;
        //ReportGrid.DataBind();

        iTextSharp.text.Table table = new iTextSharp.text.Table(ReportGrid.Columns.Count);

        table.Cellpadding = 2;

        table.Width = 100;

        //Transfer rows from GridView to table

        for (int i = 0; i < ReportGrid.Columns.Count; i++)
        {

            string cellText = Server.HtmlDecode

                                      (ReportGrid.Columns[i].HeaderText);

            iTextSharp.text.Cell cell = new iTextSharp.text.Cell(cellText);

            cell.BackgroundColor = new Color(System.Drawing

                                           .ColorTranslator.FromHtml("#93a31d"));

            table.AddCell(cell);

        }

        for (int i = 0; i < ReportGrid.Rows.Count; i++)
        {

            if (ReportGrid.Rows[i].RowType == DataControlRowType.DataRow)
            {
                string cellText = "";
                iTextSharp.text.Cell cell = new iTextSharp.text.Cell();


                cellText = Server.HtmlDecode

                                  ((ReportGrid.Rows[i].FindControl("lblsno") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }

                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblcoursename") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);

                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lbldeptname") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("deptlable") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblapplicationacr") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblstartdigit") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblappsize") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblcollacr") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lbldeptacr") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {

                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));

                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblotheracr") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {
                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));
                }
                table.AddCell(cell);
                cellText = Server.HtmlDecode

                                 ((ReportGrid.Rows[i].FindControl("lblmodifydate") as Label).Text);

                cell = new iTextSharp.text.Cell(cellText);
                if (i % 2 != 0)
                {
                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));
                }
                table.AddCell(cell);


                if (i % 2 != 0)
                {
                    cell.BackgroundColor = new Color(System.Drawing.ColorTranslator.FromHtml("#dce0bc"));
                }



            }

        }

        Document pdfDoc1 = new Document(PageSize.A4, 10f, 10f, 10f, 0f);

        PdfWriter.GetInstance(pdfDoc1, Response.OutputStream);

        pdfDoc1.Open();

        pdfDoc1.Add(table);

        pdfDoc1.Close();

        Response.ContentType = "application/pdf";

        Response.AddHeader("content-disposition", "attachment;" +

                                       "filename=GridView.pdf");

        Response.Cache.SetCacheability(HttpCacheability.NoCache);

        Response.Write(pdfDoc1);

        Response.End();

    }

    //Modified  by Idhris
    
    protected void typegrid_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.typegrid, "Type$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(this.typegrid, "instruction$" + e.Row.RowIndex);
            }
        }
        catch
        {

        }


    }
    
    protected void typebound(object sender, EventArgs e)
    {
        try
        {
            if (typegrid.Rows.Count > 0)
            {
                for (int i = 0; i < typegrid.Rows.Count; i++)
                {
                    string typevalue = ((typegrid.Rows[i].FindControl("typelnk1") as Label).Text);
                    if (typevalue.ToString().ToUpper() == "DAY")
                    {
                        (typegrid.Rows[i].FindControl("typeextendlilnk") as Label).Text = "Govt Aided Stream (Day)";
                    }
                    if (typevalue.ToString().ToUpper() == "EVENING")
                    {
                        (typegrid.Rows[i].FindControl("typeextendlilnk") as Label).Text = "Self Financed Stream (Evening)";
                    }
                    if (typevalue.ToString().Trim() == "MCA")
                    {
                        (typegrid.Rows[i].FindControl("typeextendlilnk") as Label).Text = "MCA-Self Financed Stream (Day)";
                    }
                }
            }
        }
        catch
        {
        }
    }
    
    protected void gridMembersList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (e.CommandName == "Type")
            {

                int row = Convert.ToInt32(e.CommandArgument);
                string type = Convert.ToString((typegrid.Rows[row].FindControl("typelnk1") as Label).Text);
                string college = Convert.ToString((typegrid.Rows[row].FindControl("typeextendlilnk") as Label).Text);
                string click = Convert.ToString((typegrid.Rows[row].FindControl("lnk_typeinstruction") as Label).Text);

                Session["type"] = type;
                Session["college_Code"] = Convert.ToString(college);

                string educationlev = "";
                if (Convert.ToString(Session["educationlevel"]).Trim() != "")
                {
                    string[] e_level = Convert.ToString(Session["educationlevel"]).Split(',');
                    if (e_level.Length > 0)
                    {
                        for (int i = 0; i < e_level.Length; i++)
                        {
                            if (educationlev.Trim() == "")
                            {
                                educationlev = e_level[i];
                            }
                            else
                            {
                                educationlev = educationlev + "','" + e_level[i];
                            }
                        }
                    }
                }

                string query = "select distinct Edu_Level,Priority  from course  where  Edu_Level in('" + educationlev + "') and college_code=" + Convert.ToString(Session["college_Code"]) + " order by Priority asc";

                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    grid_edulevel.DataSource = ds;
                    grid_edulevel.DataBind();

                    coursediv.Visible = true;
                    sub2.Visible = false;
                }

                foreach (GridViewRow row1 in typegrid.Rows)
                {
                    if (row == row1.DataItemIndex)
                    {
                        row1.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2");
                    }
                    else
                    {
                        row1.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                    }
                }

            }
        }
        catch
        {

        }
    }

    protected void grid_edulevel_SelectedIndexChanege(object sender, EventArgs e)
    {
        try
        {
            DataTable data = new DataTable();
            DataRow dr = null;
            string today = System.DateTime.Now.ToString("MM/dd/yyyy");
            string type = Convert.ToString((grid_edulevel.SelectedRow.FindControl("edulevellink") as Label).Text);
            Session["catagory1"] = type;
            ViewState["tempcate"] = type;
            string query = "";
            string type1 = Session["type"].ToString();

            if (type1 != "MCA")
            {
                // query = "Select course.Course_Name,Department.Dept_Name ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code from Department ,course,Degree where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and Edu_Level ='" + type + "' and Department.college_code ='" + ddlcollege.SelectedItem.Value + "'and Dept_Name<>'MCA' and course.type='" + type1 + "'";
                query = "Select distinct course.Course_Name,Department.Dept_Name ,course.Course_Id ,Department.Dept_Code,'' mode ,'' TExtcode,degree.Degree_Code,CONVERT(varchar, fromdate,103)as fromdate ,CONVERT(varchar, todate,103)as todate,RegCode from Department ,course,Degree,setting_application,DeptPrivilages dp where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and degree.Degree_Code =dp.degree_code and user_code =30 and course.Edu_Level =setting_application.Edu_level  and ISNULL( course.type,'') =setting_application.type_College and degree.college_code=setting_application.college_code  and course.Edu_Level ='" + type + "' and Department.college_code ='" + Convert.ToString(Session["college_Code"]) + "'and Dept_Name<>'MCA'  and setting_type='Code Settings' -- and '" + today + "' between fromdate and todate";

                if (cbSeatType.Checked)
                {
                    query = "Select distinct course.Course_Name,Department.Dept_Name+' ('+TExtval+' - Regular)' as Dept_Name,1 [mode] ,'Regular' modeval ,TEXTVAL,Textcode ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code,CONVERT(varchar, fromdate,103)as fromdate ,CONVERT(varchar, todate,103)as todate,RegCode  from Department ,course,Degree,setting_application,DeptPrivilages dp ,TextValTable tv  where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and degree.Degree_Code =dp.degree_code and user_code =30 and course.Edu_Level =setting_application.Edu_level  and ISNULL( course.type,'') =setting_application.type_College and degree.college_code=setting_application.college_code  and course.Edu_Level ='" + type + "' and Department.college_code ='" + Convert.ToString(Session["college_Code"]) + "'and Dept_Name<>'MCA'  and setting_type='Code Settings' and  tv.TextCriteria='seat'  and tv.college_code='" + Convert.ToString(Session["college_Code"]) + "'  union Select distinct course.Course_Name,Department.Dept_Name+' ('+TExtval+' - Transfer)' as Dept_Name,2 [mode],'Transfer' modeval  ,TEXTVAL,Textcode ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code,CONVERT(varchar, fromdate,103)as fromdate ,CONVERT(varchar, todate,103)as todate,RegCode  from Department ,course,Degree,setting_application,DeptPrivilages dp ,TextValTable tv  where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and degree.Degree_Code =dp.degree_code and user_code =30 and course.Edu_Level =setting_application.Edu_level  and ISNULL( course.type,'') =setting_application.type_College and degree.college_code=setting_application.college_code  and course.Edu_Level ='" + type + "' and Department.college_code ='" + Convert.ToString(Session["college_Code"]) + "'and Dept_Name<>'MCA'  and setting_type='Code Settings' and  tv.TextCriteria='seat'  and tv.college_code='" + Convert.ToString(Session["college_Code"]) + "'  union Select distinct course.Course_Name,Department.Dept_Name+' ('+TExtval+' - Lateral)' as Dept_Name,3 [mode],'Lateral' modeval  ,TEXTVAL,Textcode ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code,CONVERT(varchar, fromdate,103)as fromdate ,CONVERT(varchar, todate,103)as todate,RegCode  from Department ,course,Degree,setting_application,DeptPrivilages dp ,TextValTable tv  where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and degree.Degree_Code =dp.degree_code and user_code =30 and course.Edu_Level =setting_application.Edu_level  and ISNULL( course.type,'') =setting_application.type_College and degree.college_code=setting_application.college_code  and course.Edu_Level ='" + type + "' and Department.college_code ='" + Convert.ToString(Session["college_Code"]) + "'and Dept_Name<>'MCA'  and setting_type='Code Settings' and  tv.TextCriteria='seat'  and tv.college_code='" + Convert.ToString(Session["college_Code"]) + "' ";
                }
            }
            else
            {
                //query = "Select course.Course_Name,Department.Dept_Name ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code from Department ,course,Degree where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code  and Department.college_code ='" + ddlcollege.SelectedItem.Value + "' and Dept_Name='" + type1 + "' and Edu_Level ='" + type + "'";
                query = "Select distinct course.Course_Name,Department.Dept_Name,'' mode ,'' TExtcode ,course.Course_Id ,Department.Dept_Code ,degree.Degree_Code,CONVERT(varchar, fromdate,103)as fromdate ,CONVERT(varchar, todate,103)as todate,RegCode from Department ,course,Degree,setting_application,DeptPrivilages dp where course.Course_Id =Degree.Course_Id and Degree.Dept_Code =Department.Dept_Code and degree.Degree_Code =dp.degree_code and user_code =30 and course.Edu_Level =setting_application.Edu_level  and ISNULL( course.type,'') =setting_application.type_College and degree.college_code=setting_application.college_code and Department.college_code ='" + Convert.ToString(Session["college_Code"]) + "' and Dept_Name='" + type1 + "' and course.Edu_Level ='" + type + "' and '" + today + "' between fromdate and todate and setting_type='Code Settings'";

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Course_gird.DataSource = ds;
                Course_gird.DataBind();
                sub2.Visible = true;
            }
            else
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"You Cannot Apply\");", true);
            }
        }
        catch
        {

        }
    }
    
    protected void grid_edulevel_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grid_edulevel, "Select$" + e.Row.RowIndex);
                e.Row.Cells[2].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grid_edulevel, "instruction$" + e.Row.RowIndex);

            }
        }
        catch
        {

        }

    }
    
    protected void eduleveldatabound(object sender, EventArgs e)
    {
        try
        {
            if (grid_edulevel.Rows.Count > 0)
            {
                for (int row = 0; row < grid_edulevel.Rows.Count; row++)
                {
                    string eduvalue = ((grid_edulevel.Rows[row].FindControl("edulevellink") as Label).Text);
                    if (eduvalue.ToString().Trim() == "UG")
                    {
                        (grid_edulevel.Rows[row].FindControl("link_addvalue") as Label).Text = "Undergraduate";
                    }
                    else if (eduvalue.ToString().Trim() == "PG")
                    {
                        (grid_edulevel.Rows[row].FindControl("link_addvalue") as Label).Text = "Postgraduate";
                    }
                    else
                    {
                        (grid_edulevel.Rows[row].FindControl("edulevellink") as Label).Visible = true;
                    }

                }
            }
        }
        catch
        {

        }
    }
    
    protected void grid_edulevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            int row = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "instruction")
            {
                Label ltrlslno = (Label)typegrid.Rows[row].FindControl("edulevellink");
                //  Literal ltrlName = (Literal)typegrid.Rows[index].FindControl("ltrlName");
                // poperrjs.Visible = true;
                //mpemsgboxdelete.Show();
            }
            foreach (GridViewRow row1 in grid_edulevel.Rows)
            {
                if (row == row1.DataItemIndex)
                {
                    row1.BackColor = System.Drawing.ColorTranslator.FromHtml("#A1DCF2");
                }
                else
                {
                    row1.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
                }
            }
        }
        catch
        {

        }
    }
    
    public void rdb_applicationno_CheckedChanged(object sender, EventArgs e)
    {
        coursediv.Visible = false;
        sub2.Visible = false;
        lblapplno.Text = "Application Number Starts With";
        lblapplsize.Text = "Application Number Size";

        Label6.Text = "Application Number Starts With";
        Label7.Text = "Application Number Size";
        if (rbSelMode.SelectedIndex == 1)
        {
            bindCollegeGrid();
            gdclgwise_OnDataBound(sender, e);
        }
        showreport.Checked = false;
        ReportGrid.Visible = false;
        divrpt.Visible = false;
        if (rbSelMode.SelectedIndex == 2)
        {
            bindeducationGrid();
            grideduwise_OnDataBound(sender, e);
        }
    }
    
    public void rdb_admissionnoCheckedChanged(object sender, EventArgs e)
    {
        coursediv.Visible = false;
        sub2.Visible = false;
        lblapplno.Text = "Admission Number Starts With";
        lblapplsize.Text = "Admission Number Size";

        Label6.Text = "Admission Number Starts With";
        Label7.Text = "Admission Number Size";
        if (rbSelMode.SelectedIndex == 1)
        {
            bindCollegeGrid();
            gdclgwise_OnDataBound(sender, e);
        }
        showreport.Checked = false;
        ReportGrid.Visible = false;
        divrpt.Visible = false;

        if (rbSelMode.SelectedIndex == 2)
        {
            bindeducationGrid();
            grideduwise_OnDataBound(sender, e);
        }
    }
    
    protected void cbl_degreename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_degreename.Text = "--Select--";
            cb_degreename.Checked = false;
            for (int i = 0; i < cbl_degreename.Items.Count; i++)
            {
                if (cbl_degreename.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_degreename.Items.Count)
            {
                txt_degreename.Text = "Education Level (" + commcount.ToString() + ")";
                cb_degreename.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degreename.Text = "--Select--";
            }
            else
            {
                txt_degreename.Text = "Education Level (" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    
    protected void cb_degreename_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degreename.Checked == true)
            {
                for (int i = 0; i < cbl_degreename.Items.Count; i++)
                {
                    cbl_degreename.Items[i].Selected = true;
                }
                txt_degreename.Text = "Education Level (" + (cbl_degreename.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degreename.Items.Count; i++)
                {
                    cbl_degreename.Items[i].Selected = false;
                }
                txt_degreename.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    
    protected void degreebind()
    {
        try
        {
            ds.Clear();
            cbl_degreename.Items.Clear();
            string q1 = " select distinct Edu_Level,Priority  from course  where  college_code='" + Session["college_code"].ToString() + "' order by Priority asc";
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degreename.DataSource = ds;
                cbl_degreename.DataTextField = "Edu_Level";
                cbl_degreename.DataValueField = "Edu_Level";
                cbl_degreename.DataBind();
            }

            ds.Clear();
            string edulevel = d2.GetFunction("select value from Master_Settings where settings='Education level Rights' and usercode='" + Session["college_code"].ToString() + "'");

            if (edulevel.Trim() != "")
            {
                string[] e_level = edulevel.Split(',');
                if (e_level.Length > 0)
                {
                    for (int i = 0; i < e_level.Length; i++)
                    {
                        cbl_degreename.Items.FindByValue(e_level[i]).Selected = true;
                        txt_degreename.Text = "Education Level(" + (e_level.Length) + ")";
                    }
                }
            }
        }
        catch { }
    }
    
    protected void btn_degreename_Click(object sender, EventArgs e)
    {
        if (txt_degreename.Text != "--Select--")
        {
            if (cbl_degreename.Items.Count > 0)
            {
                string degreecode = "";
                for (int i = 0; i < cbl_degreename.Items.Count; i++)
                {
                    if (cbl_degreename.Items[i].Selected == true)
                    {
                        if (degreecode.Trim() == "")
                        {
                            degreecode = cbl_degreename.Items[i].Value;
                        }
                        else
                        {
                            degreecode = degreecode + "," + cbl_degreename.Items[i].Value;
                        }
                    }
                }
                if (degreecode.Trim() != "")
                {
                    string q1 = "if exists(select*from Master_Settings where settings='Education level Rights' and usercode='" + Session["college_code"].ToString() + "') update Master_Settings set value='" + degreecode + "' where usercode='" + Session["college_code"].ToString() + "' and settings='Education level Rights' else insert into Master_Settings (usercode,settings,value)values('" + Session["college_code"].ToString() + "','Education level Rights','" + degreecode + "')";
                    int up = d2.update_method_wo_parameter(q1, "Text");
                    Session["educationlevel"] = degreecode;
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Saved Successfully\");", true);
                    binddegreegrid();
                }
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please select Degree\");", true);
        }
    }
    
    protected void binddegreegrid()
    {
        ds.Clear();
        grid_edulevel.DataSource = null;
        grid_edulevel.DataBind();
        string educationlev = "";
        if (Convert.ToString(Session["educationlevel"]).Trim() != "")
        {
            string[] e_level = Convert.ToString(Session["educationlevel"]).Split(',');
            if (e_level.Length > 0)
            {
                for (int i = 0; i < e_level.Length; i++)
                {
                    if (educationlev.Trim() == "")
                    {
                        educationlev = e_level[i];
                    }
                    else
                    {
                        educationlev = educationlev + "','" + e_level[i];
                    }
                }
            }
        }

        string query = "select distinct Edu_Level,Priority  from course  where  Edu_Level in('" + educationlev + "') and college_code=" + Convert.ToString(Session["college_Code"]) + " order by Priority asc";

        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            grid_edulevel.DataSource = ds;
            grid_edulevel.DataBind();
        }
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
        lbl.Add(lblclgs);
        //lbl.Add(lbl_stream);
        lbl.Add(lbldeg);
        lbl.Add(lbldept);
        //lbl.Add(lbl_sem);
        fields.Add(0);
        // fields.Add(1);
        fields.Add(2);
        fields.Add(3);
        //  fields.Add(4);

        lbl.Add(lblscltype);
        lbl.Add(lblstandard);
        fields.Add(2);
        fields.Add(3);


        lbl.Add(Label1);
        fields.Add(0);

        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    //added by sudhagar 10.12.2016
    protected void rbSelMode_Selected(object sender, EventArgs e)
    {
        fdChkSeat.Visible = false;
        cbSeatType.Checked = false;
        Fieldsetincludeclg.Visible = false;
        if (rbSelMode.SelectedIndex == 0)
        {
            fdChkSeat.Visible = true;

            divedu.Visible = true;
            step2.Visible = false;
            divclg.Visible = false;
            typegrid.Visible = true;
            gdclgwise.Visible = false;
            showreport.Checked = false;
            ReportGrid.Visible = false;
            divrpt.Visible = false;
            grideduwise.Visible = false;
        }
        else if (rbSelMode.SelectedIndex == 1)
        {
            divedu.Visible = false;
            step2.Visible = false;
            divclg.Visible = false;
            typegrid.Visible = false;
            gdclgwise.Visible = true;
            bindCollegeGrid();
            gdclgwise_OnDataBound(sender, e);
            showreport.Checked = false;
            ReportGrid.Visible = false;
            divrpt.Visible = false;
            grideduwise.Visible = false;
        }
        else
        {
            step2.Visible = false;
            divclg.Visible = false;
            typegrid.Visible = true;
            gdclgwise.Visible = false;
            showreport.Checked = false;
            ReportGrid.Visible = false;
            divrpt.Visible = false;

            divedu.Visible = false;
            step2.Visible = false;
            divclg.Visible = false;
            typegrid.Visible = false;
            showreport.Checked = false;
            ReportGrid.Visible = false;
            divrpt.Visible = false;
            gdclgwise_OnDataBound(sender, e);
            bindeducationGrid();
            grideduwise.Visible = true;
            Fieldsetincludeclg.Visible = true;
        }
    }

    protected void btnclgGent_Click(object sender, EventArgs e)
    {
        try
        {

            // string degreecode = Convert.ToString(Session["degree_code"]);
            string college_code = Convert.ToString(Session["college_code"]);

            string batch_year = Convert.ToString(txtclgbatch.Text);
            Session["batch"] = batch_year;
            string college_acr = Convert.ToString(txtclgacr.Text).ToUpper();
            //  string dept_acr = Convert.ToString(txt_deptacr.Text);
            string other_acr = Convert.ToString(txtclgother.Text).ToUpper();
            string start_with = Convert.ToString(txtclgappl.Text);
            string count = Convert.ToString(txtclgadmis.Text);
            string date = Convert.ToString(txtclgmdate.Text);
            string[] split_date = date.Split('/');
            DateTime da1 = Convert.ToDateTime(split_date[1] + "/" + split_date[0] + "/" + split_date[2]);
            string concateacr = college_acr + "" + other_acr;
            string app_adm = "0";
            if (rdb_applicationno.Checked == true)
                app_adm = "0";
            else
                app_adm = "1";

            string genType = "0";
            if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
            {
                genType = "3";

            }
            else if (rbSelMode.SelectedIndex == 1)
            {
                genType = "1";
            }
            else if (rbSelMode.SelectedIndex == 2)
            {
                genType = "2";
            }
            if (rbSelMode.Items[1].Selected == true)
            {
                if (start_with.Trim() != "" && count.Trim() != "")
                {
                    string updatequery = "";
                    updatequery = "if not exists (select * from code_generation where college_code='" + college_code + "' and app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='1'  and isnull(cg_generationType,0)='" + genType + "') insert into code_generation(batch_year,app_acr,app_other_acr,app_startwith,app_serial,app_modifydate,app_code_flag,appcode,college_code,IsCollege, cg_generationType) values ('" + batch_year + "','" + college_acr + "','" + other_acr + "','" + start_with + "','" + count + "','" + da1.ToString("MM/dd/yyyy") + "','" + app_adm + "','" + concateacr + "','" + college_code + "','1','" + genType + "') else update code_generation set batch_year ='" + batch_year + "' ,app_acr ='" + college_acr + "' ,app_other_acr='" + other_acr + "',app_code_flag='" + app_adm + "',appcode='" + concateacr + "',app_startwith='" + start_with + "',app_serial='" + count + "',app_modifydate='" + da1.ToString("MM/dd/yyyy") + "' where college_code='" + college_code + "' and app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='1'  and isnull(cg_generationType,0)='" + genType + "'";

                    int insert = d2.update_method_wo_parameter(updatequery, "Text");
                    if (insert != 0)
                    {
                        gdclgwise_OnDataBound(sender, e);
                        if (rdb_applicationno.Checked == true)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Application Number Generated Successfully')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Admission Number Generated Successfully')", true);
                        }
                    }
                    //  mpemsgboxdelete.Hide();
                }
                else
                {
                    // mpemsgboxdelete.Show();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Fill Required Fields')", true);
                }
            }
            if (rbSelMode.Items[2].Selected == true)//barath 09.03.17
            {
                if (start_with.Trim() != "" && count.Trim() != "")
                {
                    if (ViewState["educationlevel"] != null)
                    {
                        string updatequery = ""; int insert = 0;
                        if (cb_includeclg.Checked)
                        {
                            updatequery = "if not exists (select * from code_generation where app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='2' and Edulevel='" + Convert.ToString(ViewState["educationlevel"]) + "'  and isnull(cg_generationType,0)='" + genType + "' and college_code='" + college_code + "') insert into code_generation(batch_year,app_acr,app_other_acr,app_startwith,app_serial,app_modifydate,app_code_flag,appcode,IsCollege,Edulevel, cg_generationType,college_code) values ('" + batch_year + "','" + college_acr + "','" + other_acr + "','" + start_with + "','" + count + "','" + da1.ToString("MM/dd/yyyy") + "','" + app_adm + "','" + concateacr + "','2','" + Convert.ToString(ViewState["educationlevel"]) + "','" + genType + "','" + college_code + "') else update code_generation set batch_year ='" + batch_year + "' ,app_acr ='" + college_acr + "' ,app_other_acr='" + other_acr + "',app_code_flag='" + app_adm + "',appcode='" + concateacr + "',app_startwith='" + start_with + "',app_serial='" + count + "',app_modifydate='" + da1.ToString("MM/dd/yyyy") + "' , college_code='" + college_code + "' where app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='2' and Edulevel='" + Convert.ToString(ViewState["educationlevel"]) + "'  and isnull(cg_generationType,0)='" + genType + "' and college_code='" + college_code + "'";
                            insert = d2.update_method_wo_parameter(updatequery, "Text");
                        }
                        else
                        {
                            updatequery = "if not exists (select * from code_generation where app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='2' and Edulevel='" + Convert.ToString(ViewState["educationlevel"]) + "'  and isnull(cg_generationType,0)='" + genType + "') insert into code_generation(batch_year,app_acr,app_other_acr,app_startwith,app_serial,app_modifydate,app_code_flag,appcode,IsCollege,Edulevel, cg_generationType) values ('" + batch_year + "','" + college_acr + "','" + other_acr + "','" + start_with + "','" + count + "','" + da1.ToString("MM/dd/yyyy") + "','" + app_adm + "','" + concateacr + "','2','" + Convert.ToString(ViewState["educationlevel"]) + "','" + genType + "') else update code_generation set batch_year ='" + batch_year + "' ,app_acr ='" + college_acr + "' ,app_other_acr='" + other_acr + "',app_code_flag='" + app_adm + "',appcode='" + concateacr + "',app_startwith='" + start_with + "',app_serial='" + count + "',app_modifydate='" + da1.ToString("MM/dd/yyyy") + "' where app_code_flag='" + app_adm + "' and batch_year='" + batch_year + "' and IsCollege='2' and Edulevel='" + Convert.ToString(ViewState["educationlevel"]) + "'  and isnull(cg_generationType,0)='" + genType + "'";
                            insert = d2.update_method_wo_parameter(updatequery, "Text");
                        }
                        if (insert != 0)
                        {
                            grideduwise_OnDataBound(sender, e);
                            if (rdb_applicationno.Checked == true)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Application Number Generated Successfully')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Admission Number Generated Successfully')", true);
                            }
                        }
                    }
                    else { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Select Education Level')", true); }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "clientScript", "alert('Please Fill Required Fields')", true);
                }
            }
        }
        catch
        {

        }
    }
    
    protected void btnclgCan_Click(object sender, EventArgs e)
    {
        divclg.Visible = false;
    }

    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {

    }

    //acr 

    protected void cbclgacr_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbclgacr.Checked == true)
            {
                string query = "select Coll_acronymn  from collinfo where college_code=" + Session["college_code"] + "";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string collacr = Convert.ToString(ds.Tables[0].Rows[0]["Coll_acronymn"]);
                    if (collacr.Trim() != "")
                    {
                        txtclgacr.Text = collacr.ToString();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Define College Acronym')", true);
                }
            }
            else
            {
                txtclgacr.Text = "";
            }


        }
        catch
        {

        }
    }
    
    protected void cbclgother_Change(object sender, EventArgs e)
    {
        try
        {
            if (cbclgother.Checked == true)
            {
                txtclgother.Enabled = true;
            }
            else
            {
                txtclgother.Text = "";
                txtclgother.Enabled = false;
            }
        }
        catch
        {

        }
    }

    protected void loadDetails(string collegecode)
    {
        try
        {
            txtclgpdate.Enabled = false;
            int Year = 0;
            int.TryParse(Convert.ToString(System.DateTime.Now.ToString("yyyy")), out Year);
            txtclgbatch.Text = Convert.ToString(Year);
            //  txtclgbatch.Attributes.Add("readonly", "readonly");
            txtclgpdate.Attributes.Add("readonly", "readonly");
            txtclgmdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtclgmdate.Attributes.Add("readonly", "readonly");
            txtclgacr.Text = "";
            txtclgother.Text = "";
            txtclgappl.Text = "";
            txtclgadmis.Text = "";
            txtclgpdate.Text = "";
            cbclgacr.Checked = false;
            cbclgother.Checked = false;

            string select_query = "select batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,CONVERT(varchar, app_modifydate,103)as date from code_generation where college_code=" + collegecode + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                string college_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_acr"]);
                string other_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_other_acr"]);
                string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                string count = Convert.ToString(ds.Tables[0].Rows[0]["app_serial"]);
                string date = Convert.ToString(ds.Tables[0].Rows[0]["date"]);
                if (college_acr.Trim() != "" && college_acr.Trim() != null)
                {
                    txtclgacr.Text = college_acr.ToString();
                    cbclgacr.Checked = true;
                }
                else
                {
                    txtclgacr.Text = "";
                    cbclgacr.Checked = false;
                }

                if (other_acr.Trim() != "" && other_acr.Trim() != null)
                {
                    txtclgother.Text = other_acr.ToString();
                    cbclgother.Checked = true;
                    txtclgother.Enabled = true;
                }
                else
                {
                    txtclgother.Text = "";
                    cbclgother.Checked = false;
                }
                if (start_with.Trim() != "" && start_with.Trim() != null)
                    txtclgappl.Text = start_with.ToString();
                else
                    txtclgappl.Text = "";

                if (count.Trim() != "" && count.Trim() != null)
                    txtclgadmis.Text = count.ToString();
                else
                    txtclgadmis.Text = "";

                if (date.Trim() != "" && date.Trim() != null)
                {
                    txtclgpdate.Text = date.ToString();
                    txtclgpdate.Enabled = false;
                }
                else
                    txtclgpdate.Text = "";
            }
        }
        catch { }
    }

    //grid
    public void bindCollegeGrid()
    {
        try
        {
            //Session["college_code"] = ddlcollege.SelectedItem.Value;
            string typequery = "select distinct type  from course where college_code =" + Session["college_code"] + "";
            typequery = " select collname ,college_code  from collinfo ";
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                gdclgwise.DataSource = ds;
                gdclgwise.DataBind();

            }
        }
        catch
        {

        }
    }
    
    protected void gdclgwise_OnDataBound(object sender, EventArgs e)
    {
        //for (int i = Course_gird.Rows.Count - 1; i > 0; i--)
        //{
        //    GridViewRow row = Course_gird.Rows[i];
        //    GridViewRow previousRow = Course_gird.Rows[i - 1];
        //    for (int j = 1; j <= 1; j++)
        //    {
        //        Label lnlname = (Label)row.FindControl("lblcoursename");
        //        Label lnlname1 = (Label)previousRow.FindControl("lblcoursename");

        //        if (lnlname.Text == lnlname1.Text)
        //        {
        //            if (previousRow.Cells[j].RowSpan == 0)
        //            {
        //                if (row.Cells[j].RowSpan == 0)
        //                {
        //                    previousRow.Cells[j].RowSpan += 2;
        //                }
        //                else
        //                {
        //                    previousRow.Cells[j].RowSpan = row.Cells[j].RowSpan + 1;
        //                }
        //                row.Cells[j].Visible = false;
        //            }
        //        }
        //    }
        //}

        if (gdclgwise.Rows.Count > 0)
        {
            Hashtable hat = new Hashtable();
            hat.Add(1, "0");
            hat.Add(2, "00");
            hat.Add(3, "000");
            hat.Add(4, "0000");
            hat.Add(5, "00000");
            hat.Add(6, "000000");
            hat.Add(7, "0000000");
            hat.Add(8, "00000000");
            hat.Add(9, "000000000");
            hat.Add(10, "0000000000");
            string app_adm = "";
            string batch = "";
            int Year = 0;
            if (Session["batch"] != null && Session["batch"] != "")
                batch = Convert.ToString(Session["batch"]);
            else
            {

                int.TryParse(Convert.ToString(System.DateTime.Now.ToString("yyyy")), out Year);
                batch = Convert.ToString(Year);
            }
            for (int row = 0; row < gdclgwise.Rows.Count; row++)
            {
                if (rdb_applicationno.Checked == true)
                {
                    app_adm = "0";
                }
                else
                {
                    app_adm = "1";
                }
                string genType = "0";
                if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
                {
                    genType = "3";

                }
                else if (rbSelMode.SelectedIndex == 1)
                {
                    genType = "1";
                }
                else if (rbSelMode.SelectedIndex == 2)
                {
                    genType = "2";
                }
                string collegecode = (gdclgwise.Rows[row].FindControl("lblclgcode") as Label).Text;



                string generatequery = "select appcode,app_startwith,app_serial from code_generation where batch_year =" + batch + " and college_code =" + collegecode + " and app_code_flag='" + app_adm + "' and isnull(IsCollege,'0')<>'0'  and isnull(cg_generationType,0)='" + genType + "' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(generatequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                    int len = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]).Length;
                    string start_number = "";
                    if (len == Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]))
                    {
                        start_number = start_with;
                    }
                    else
                    {
                        int remain = Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]) - len;
                        string addnumber = Convert.ToString(hat[remain]);
                        start_number = addnumber + "" + start_with;
                    }
                    (gdclgwise.Rows[row].FindControl("lblnotgent") as Label).Text = Convert.ToString(ds.Tables[0].Rows[0]["appcode"]) + "" + start_number;
                    (gdclgwise.Rows[row].FindControl("lblnotgent") as Label).ForeColor = System.Drawing.Color.Black;
                    (gdclgwise.Rows[row].FindControl("applylnk") as LinkButton).Text = "Re-Generate";
                }
            }
        }
    }
    
    protected void gdclgwise_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //  e.Row.Cells[1].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gdclgwise, "Select$" + e.Row.RowIndex);
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gdclgwise, "Select$" + e.Row.RowIndex);

        }
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    e.Row.Cells[1].Text = lblscltype.Text;
        //    e.Row.Cells[2].Text = lblstandard.Text;
        //}
    }
    
    protected void gdclgwise_SelectedIndexChanege(object sender, EventArgs e)
    {
        try
        {

            string clgname = Convert.ToString((gdclgwise.SelectedRow.FindControl("lblclgname") as Label).Text);
            string collegecode = Convert.ToString((gdclgwise.SelectedRow.FindControl("lblclgcode") as Label).Text);
            Session["college_code"] = collegecode;
            txtclgpdate.Enabled = false;
            int Year = 0;
            int.TryParse(Convert.ToString(System.DateTime.Now.ToString("yyyy")), out Year);
            txtclgbatch.Text = Convert.ToString(Year);
            //  txtclgbatch.Attributes.Add("readonly", "readonly");
            txtclgpdate.Attributes.Add("readonly", "readonly");
            txtclgmdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtclgmdate.Attributes.Add("readonly", "readonly");
            txtclgacr.Text = "";
            txtclgother.Text = "";
            txtclgappl.Text = "";
            txtclgadmis.Text = "";
            txtclgpdate.Text = "";
            cbclgacr.Checked = false;
            cbclgother.Checked = false;
            string app_adm = "";
            if (rdb_applicationno.Checked == true)
            {
                app_adm = "0";
            }
            else
            {
                app_adm = "1";
            }
            string genType = "0";
            if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
            {
                genType = "3";

            }
            else if (rbSelMode.SelectedIndex == 1)
            {
                genType = "1";
            }
            else if (rbSelMode.SelectedIndex == 2)
            {
                genType = "2";
            }
            string select_query = "select batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,CONVERT(varchar, app_modifydate,103)as date from code_generation where college_code=" + collegecode + " and isnull(IsCollege,'0')<>'0' and app_code_flag='" + app_adm + "'  and isnull(cg_generationType,0)='" + genType + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                string college_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_acr"]);
                string other_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_other_acr"]);
                string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                string count = Convert.ToString(ds.Tables[0].Rows[0]["app_serial"]);
                string date = Convert.ToString(ds.Tables[0].Rows[0]["date"]);
                if (college_acr.Trim() != "" && college_acr.Trim() != null)
                {
                    txtclgacr.Text = college_acr.ToString();
                    cbclgacr.Checked = true;
                }
                else
                {
                    txtclgacr.Text = "";
                    cbclgacr.Checked = false;
                }

                if (other_acr.Trim() != "" && other_acr.Trim() != null)
                {
                    txtclgother.Text = other_acr.ToString();
                    cbclgother.Checked = true;
                    txtclgother.Enabled = true;
                }
                else
                {
                    txtclgother.Text = "";
                    cbclgother.Checked = false;
                }
                if (start_with.Trim() != "" && start_with.Trim() != null)
                    txtclgappl.Text = start_with.ToString();
                else
                    txtclgappl.Text = "";

                if (count.Trim() != "" && count.Trim() != null)
                    txtclgadmis.Text = count.ToString();
                else
                    txtclgadmis.Text = "";

                if (date.Trim() != "" && date.Trim() != null)
                {
                    txtclgpdate.Text = date.ToString();
                    txtclgpdate.Enabled = false;
                }
                else
                    txtclgpdate.Text = "";
            }
            Label3.Text = "";
            Label3.Text = Label1.Text + " Acronym";
            divclg.Visible = true;
        }
        catch
        {

        }

    }
    
    //barath 09.03.17
    
    #region Education level wise application code and admission number generated
    public void bindeducationGrid()
    {
        try
        {
            // Session["college_code"] = ddlcollege.SelectedItem.Value;
            string typequery = " select distinct Edu_Level from course order by Edu_Level desc";// where college_code ='" + ddlcollege.SelectedItem.Value + "'
            ds = d2.select_method_wo_parameter(typequery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                grideduwise.DataSource = ds;
                grideduwise.DataBind();

            }
        }
        catch
        {

        }
    }
    protected void grideduwise_OnDataBound(object sender, EventArgs e)
    {
        if (grideduwise.Rows.Count > 0)
        {
            //bindeducationGrid();
            Hashtable hat = new Hashtable();
            hat.Add(1, "0");
            hat.Add(2, "00");
            hat.Add(3, "000");
            hat.Add(4, "0000");
            hat.Add(5, "00000");
            hat.Add(6, "000000");
            hat.Add(7, "0000000");
            hat.Add(8, "00000000");
            hat.Add(9, "000000000");
            hat.Add(10, "0000000000");
            string app_adm = "";
            string batch = "";
            int Year = 0;
            if (Session["batch"] != null && Session["batch"] != "")
                batch = Convert.ToString(Session["batch"]);
            else
            {
                int.TryParse(Convert.ToString(System.DateTime.Now.ToString("yyyy")), out Year);
                batch = Convert.ToString(Year);
            }
            for (int row = 0; row < grideduwise.Rows.Count; row++)
            {
                if (rdb_applicationno.Checked == true)
                {
                    app_adm = "0";
                }
                else
                {
                    app_adm = "1";
                }
                string genType = "0";
                if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
                {
                    genType = "3";

                }
                else if (rbSelMode.SelectedIndex == 1)
                {
                    genType = "1";
                }
                else if (rbSelMode.SelectedIndex == 2)
                {
                    genType = "2";
                }
                string colCode = string.Empty;
                if (cb_includeclg.Checked)
                    colCode = Convert.ToString(Session["college_code"]);
                string edulevel = (grideduwise.Rows[row].FindControl("lblclgname") as Label).Text;
                string generatequery = "select appcode,app_startwith,app_serial from code_generation where batch_year =" + batch + " and app_code_flag='" + app_adm + "' and IsCollege='2' and Edulevel='" + Convert.ToString(edulevel) + "'  and isnull(cg_generationType,0)='" + genType + "'";
                if (!string.IsNullOrEmpty(colCode))
                    generatequery += " and college_code='" + colCode + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(generatequery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                    int len = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]).Length;
                    string start_number = "";
                    if (len == Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]))
                    {
                        start_number = start_with;
                    }
                    else
                    {
                        int remain = Convert.ToInt32(ds.Tables[0].Rows[0]["app_serial"]) - len;
                        string addnumber = Convert.ToString(hat[remain]);
                        start_number = addnumber + "" + start_with;
                    }
                    (grideduwise.Rows[row].FindControl("lblnotgent") as Label).Text = Convert.ToString(ds.Tables[0].Rows[0]["appcode"]) + "" + start_number;
                    (grideduwise.Rows[row].FindControl("lblnotgent") as Label).ForeColor = System.Drawing.Color.Black;
                    (grideduwise.Rows[row].FindControl("applylnk") as LinkButton).Text = "Re-Generate";
                }
            }
        }
    }
    protected void grideduwise_OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Cells[3].Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(grideduwise, "Select$" + e.Row.RowIndex);

        }
    }
    protected void grideduwise_SelectedIndexChanege(object sender, EventArgs e)
    {
        try
        {

            string educationlevel = Convert.ToString((grideduwise.SelectedRow.FindControl("lblclgname") as Label).Text);
            ViewState["educationlevel"] = educationlevel;
            //string collegecode = Convert.ToString((gdclgwise.SelectedRow.FindControl("lblclgcode") as Label).Text);
            //Session["college_code"] = collegecode;
            txtclgpdate.Enabled = false;
            int Year = 0;
            int.TryParse(Convert.ToString(System.DateTime.Now.ToString("yyyy")), out Year);
            txtclgbatch.Text = Convert.ToString(Year);
            txtclgpdate.Attributes.Add("readonly", "readonly");
            txtclgmdate.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
            txtclgmdate.Attributes.Add("readonly", "readonly");
            txtclgacr.Text = "";
            txtclgother.Text = "";
            txtclgappl.Text = "";
            txtclgadmis.Text = "";
            txtclgpdate.Text = "";
            cbclgacr.Checked = false;
            cbclgother.Checked = false;
            string app_adm = "";
            if (rdb_applicationno.Checked == true)
            {
                app_adm = "0";
            }
            else
            {
                app_adm = "1";
            }
            string genType = "0";
            if (cbSeatType.Checked && rbSelMode.SelectedIndex == 0)
            {
                genType = "3";

            }
            else if (rbSelMode.SelectedIndex == 1)
            {
                genType = "1";
            }
            else if (rbSelMode.SelectedIndex == 2)
            {
                genType = "2";
            }
            string colCode = string.Empty;
            if (cb_includeclg.Checked)
                colCode = Convert.ToString(Session["college_code"]);
            string select_query = "select batch_year,app_acr,app_dept_acr,app_other_acr,app_startwith,app_serial,degree_code,CONVERT(varchar, app_modifydate,103)as date from code_generation where IsCollege='2' and app_code_flag='" + app_adm + "' and edulevel='" + Convert.ToString(ViewState["educationlevel"]) + "'  and isnull(cg_generationType,0)='" + genType + "'";//college_code=" + collegecode + " and isnull(IsCollege,'0')<>'0'
            if (!string.IsNullOrEmpty(colCode))
                select_query += " and college_code='" + colCode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string batch_year = Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]);
                string college_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_acr"]);
                string other_acr = Convert.ToString(ds.Tables[0].Rows[0]["app_other_acr"]);
                string start_with = Convert.ToString(ds.Tables[0].Rows[0]["app_startwith"]);
                string count = Convert.ToString(ds.Tables[0].Rows[0]["app_serial"]);
                string date = Convert.ToString(ds.Tables[0].Rows[0]["date"]);
                if (college_acr.Trim() != "" && college_acr.Trim() != null)
                {
                    txtclgacr.Text = college_acr.ToString();
                    cbclgacr.Checked = true;
                }
                else
                {
                    txtclgacr.Text = "";
                    cbclgacr.Checked = false;
                }

                if (other_acr.Trim() != "" && other_acr.Trim() != null)
                {
                    txtclgother.Text = other_acr.ToString();
                    cbclgother.Checked = true;
                    txtclgother.Enabled = true;
                }
                else
                {
                    txtclgother.Text = "";
                    cbclgother.Checked = false;
                }
                if (start_with.Trim() != "" && start_with.Trim() != null)
                    txtclgappl.Text = start_with.ToString();
                else
                    txtclgappl.Text = "";

                if (count.Trim() != "" && count.Trim() != null)
                    txtclgadmis.Text = count.ToString();
                else
                    txtclgadmis.Text = "";

                if (date.Trim() != "" && date.Trim() != null)
                {
                    txtclgpdate.Text = date.ToString();
                    txtclgpdate.Enabled = false;
                }
                else
                    txtclgpdate.Text = "";
            }
            Label3.Text = "";
            Label3.Text = Label1.Text + " Acronym";
            divclg.Visible = true;
        }
        catch
        {

        }
    }


    #endregion
    
    //Added by Idhris 04-05-2017
    
    protected void cbSeatType_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            Course_gird.DataBind();
        }
        catch { }
    }
    
    protected void cb_includeclg_Changed(object sender, EventArgs e)
    {
        if (rbSelMode.SelectedIndex == 2)
        {
            bindeducationGrid();
            grideduwise_OnDataBound(sender, e);
        }
    }

}


