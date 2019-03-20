using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Drawing;
using System.IO;
using Gios.Pdf;
using System.Text.RegularExpressions;

public partial class languagewise_stngth : System.Web.UI.Page
{
    string batchyearselected = "";
    string semselected = "";
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string course_id = string.Empty;
    string strbatch = string.Empty;
    string strbranch = string.Empty;
    string SenderID = string.Empty;
    string Password = string.Empty;
    int count = 0;
    ArrayList avoidcol = new ArrayList();
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataSet studgradeds = new DataSet();
    FarPoint.Web.Spread.TextCellType txtceltype = new FarPoint.Web.Spread.TextCellType();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Visible = false;
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
            if (!IsPostBack)
            {
                loadsubjecttype();
                bindedulevel();
                cklgender.Items.Add("Male");
                cklgender.Items.Add("Female");
                cklgender.Items.Add("Transgender");
                final.Visible = false;
                loadtype();
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                FpSpread1.Visible = false;
                bindsemester();
            }
        }
        catch (Exception ex) { }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblnorec.Text = "";



            hide();
            lblerrormsg.Visible = true;
            int count = 0;
            //for (int i = 0; i < chklsbatch.Items.Count; i++)
            //{
            //    if (chklsbatch.Items[i].Selected == true)
            //    {
            //        count++;
            //    }
            //}

            //if (count == 0)
            //{
            //    lblerrormsg.Text = "Please Select Atleast One Batch";
            //    hide();
            //    lblerrormsg.Visible = true;
            //    return;


            //}
            //else
            //{
            //    lblerrormsg.Text = "";

            //}
            count = 0;
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Group";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            count = 0;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Branch";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }
            //count = 0;
            //for (int i = 0; i < cblterm.Items.Count; i++)
            //{
            //    if (cblterm.Items[i].Selected == true)
            //    {
            //        count++;
            //    }
            //}

            //if (count == 0)
            //{
            //    lblerrormsg.Text = "Please Select Atleast One Sem";
            //    hide();
            //    lblerrormsg.Visible = true;
            //    return;


            //}
            //else
            //{
            //    lblerrormsg.Text = "";

            //}
            count = 0;
            for (int i = 0; i < cklgender.Items.Count; i++)
            {
                if (cklgender.Items[i].Selected == true)
                {
                    count++;
                }
            }

            if (count == 0)
            {
                lblerrormsg.Text = "Please Select Atleast One Gender";
                hide();
                lblerrormsg.Visible = true;
                return;


            }
            else
            {
                lblerrormsg.Text = "";

            }


            bindheader();
            bindvalue();



            // bindvalue();
            if (FpSpread1.Sheets[0].Rows.Count > 0)
            {
                final.Visible = true;
            }

            for (int i = 0; i < FpSpread1.Sheets[0].ColumnHeader.Columns.Count; i++)
            {
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].ForeColor = Color.Black;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Columns[i].VerticalAlign = VerticalAlign.Middle;

                FpSpread1.Sheets[0].Columns[i].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].Columns[i].Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].Columns[i].Font.Bold = true;
                FpSpread1.Sheets[0].Columns[i].ForeColor = Color.Black;
                if (i >= 4)
                {
                    FpSpread1.Sheets[0].Columns[i].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[i].VerticalAlign = VerticalAlign.Middle;
                }
                FpSpread1.Sheets[0].Columns[i].Locked = true;

            }

            ds.Clear();
            ds.Dispose();
            avoidcol.Clear();
            // avoirows.Clear();

        }


        catch
        {
        }
    }








    protected void btnExcel_Click(object sender, EventArgs e)
    {
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

                    FpSpread1.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                    Response.Clear();
                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.Flush();
                    Response.WriteFile(szPath + szFile);
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
            lblnorec.Text = ex.ToString();
        }

    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = true;
            lblnorec.Text = "";


            //// string date_filt = "From : " + tbstart_date.Text.ToString() + "   " + "To : " + tbend_date.Text.ToString();
            //string degreeset = da.GetFunction("select (Course_Name+' - '+Acronym) as degreeset from course c, degree d where c.Course_Id=d.Course_Id and Degree_Code='" + ddstandard.SelectedItem.Value.ToString() + "'");
            //degreeset=degreeset+" - "+ ddlSemYr.SelectedItem.Text.ToString();
            //string strsec = "";
            //if (ddlSec.Enabled == true)
            //{
            //    string sections = ddlSec.SelectedItem.Text.ToString();
            //    if (sections.ToString() == "All" || sections.ToString() == "" || sections.ToString() == "-1")
            //    {
            //        strsec = "";
            //    }
            //    else
            //    {
            //        strsec = " - " + sections.ToString();
            //    }

            //}
            //degreeset = degreeset +  strsec;

            //int batchyear = Convert.ToInt32(dropyear.SelectedItem.Text.ToString());

            //string date_filt = "Batch : "+ddlBatch.SelectedItem.Text.ToString()+" ";
            int batchh = Convert.ToInt32(ddlBatch.SelectedItem.Text.ToString());

            //date_filt = date_filt + "@" + "Degree : ";
            string degreedetails = string.Empty;
            // string sqlschool = "select value from Master_Settings where settings='Academic year'";
            //ds.Clear();
            //                ds = da.select_method_wo_parameter(sqlschool, "Text");
            //                string splitvalue = ds.Tables[0].Rows[0]["value"].ToString();
            //                string[] dsplit = splitvalue.Split(',');

            //                string fvalue = dsplit[0].ToString();
            //                string lvalue = dsplit[1].ToString();
            //                string acdmic_date = fvalue + "-" + lvalue;
            degreedetails = "Languages strength Report " + (batchh) + " - " + (batchh + 1);
            string pagename = "languagewise_stngth.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
        {
            //lblerrormsg.Text = ex.ToString();
            //lblerrormsg.Visible = true;
        }

    }

    public void loadtype()
    {
        try
        {
            collegecode = Session["collegecode"].ToString();
            ddltype.Items.Clear();
            string strquery = "select distinct type from course where college_code='" + collegecode + "' and type is not null and type<>''";
            ds = da.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {


                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();

                //ddltype.Items.Insert(0, "Select");
                ddltype.Enabled = true;


            }
            else
            {
                ddltype.Enabled = false;
            }

        }
        catch
        {
        }
    }

    public void Bindcollege()
    {
        try
        {
            string columnfield = "";
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_user + "'";
            }
            else
            {
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            DataSet dsprint = d2.select_method("bind_college", hat, "sp");
            ddlcollege.Items.Clear();
            if (dsprint.Tables[0].Rows.Count > 0)
            {

                ddlcollege.DataSource = dsprint;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
                loadtype();
            }
            else
            {
                lblerrormsg.Text = "Set college rights to the staff";
                lblerrormsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void BindBatch()
    {
        try
        {
            ds2.Dispose();
            ds2.Reset();
            ds2 = d2.BindBatch();
            if (ds2.Tables[0].Rows.Count > 0)
            {
                ddlBatch.DataSource = ds2;
                ddlBatch.DataTextField = "Batch_year";
                ddlBatch.DataValueField = "Batch_year";
                ddlBatch.DataBind();
                // ddlBatch.SelectedIndex = ddlBatch.Items.Count - 1;
                //for (int i = 0; i < chklsbatch.Items.Count; i++)
                //{
                //    chklsbatch.Items[i].Selected = true;
                //    if (chklsbatch.Items[i].Selected == true)
                //    {
                //        count += 1;
                //    }
                //    if (chklsbatch.Items.Count == count)
                //    {
                //        chkbatch.Checked = true;
                //    }
                //}
                //if (chkbatch.Checked == true)
                //{
                //    for (int i = 0; i < chklsbatch.Items.Count; i++)
                //    {
                //        chklsbatch.Items[i].Selected = true;
                //        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < chklsbatch.Items.Count; i++)
                //    {
                //        chklsbatch.Items[i].Selected = false;
                //        txtbatch.Text = "---Select---";
                //    }
                //}
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {

            lblerrormsg.Visible = false;
            count = 0;
            chklstdegree.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            // ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ddltype.Items.Count > 0)
            {
            if (singleuser == "True")
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id  and course.college_code = degree.college_code   and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "  and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                ds2 = da.select_method_wo_parameter(strquery, "Text");
            }
            else
            {
                ds2.Dispose();
                ds2.Reset();
                string strquery1 = "select distinct degree.course_id,course.course_name from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code  and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                ds2 = da.select_method_wo_parameter(strquery1, "Text");
            }
            }
            else
            {
                lblerrormsg.Visible = true;
                lblerrormsg.Text = "No type were found";
            }
            if (ds2.Tables.Count>0 && ds2.Tables[0].Rows.Count > 0)
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
                        count += 1;
                    }
                    if (chklstdegree.Items.Count == count)
                    {
                        chkdegree.Checked = true;
                    }
                }
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
                txtdegree.Enabled = true;
                // BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }

    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            count = 0;
            collegecode = ddlcollege.SelectedValue.ToString();
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
            chklstbranch.Items.Clear();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                //ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                if (singleuser == "True")
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery = "select distinct degree.degree_code, department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + "  and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery, "Text");
                }
                else
                {
                    ds2.Dispose();
                    ds2.Reset();
                    string strquery1 = "select distinct degree.degree_code, department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + Session["collegecode"].ToString() + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + " and course.type='" + ddltype.SelectedItem.Text.ToString() + "' and course.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery1, "Text");
                }
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
                            count += 1;
                        }
                        if (chklstbranch.Items.Count == count)
                        {
                            chkbranch.Checked = true;
                        }
                    }
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
                            chkbranch.Checked = false;
                            chklstbranch.Items[i].Selected = false;
                            txtbranch.Text = "---Select---";
                        }
                    }
                    bindsemester();
                }
            }
            else
            {
                txtbranch.Text = "---Select---";
                chklstbranch.Items.Clear();
            }
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
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
                }
                txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = false;
                }
                txtbatch.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
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
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {

        }
    }

    protected void chkdegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            if (chkdegree.Checked == true)
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = true;
                }
                txtdegree.Text = "Degree(" + (chklstdegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstdegree.Items.Count; i++)
                {
                    chklstdegree.Items[i].Selected = false;
                }
                txtdegree.Text = "---Select---";
                txtbranch.Text = "---Select---";
            }
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {

        }
    }

    protected void chklstdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            collegecode = ddlcollege.SelectedValue.ToString();
            int commcount = 0;
            chkdegree.Checked = false;
            txtdegree.Text = "---Select---";
            for (int i = 0; i < chklstdegree.Items.Count; i++)
            {
                if (chklstdegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtdegree.Text = "Degree(" + commcount.ToString() + ")";
                if (commcount == chklstdegree.Items.Count)
                {
                    chkdegree.Checked = true;
                }
            }

            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
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
                }
                txtbranch.Text = "Branch(" + (chklstbranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    chklstbranch.Items[i].Selected = false;
                }
                chkbranch.Checked = false;
                txtbranch.Text = "---Select---";
            }
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            lblerrormsg.Visible = false;
            string clg = "";
            int commcount = 0;
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
            for (int i = 0; i < chklstbranch.Items.Count; i++)
            {
                if (chklstbranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtbranch.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == chklstbranch.Items.Count)
                {
                    chkbranch.Checked = true;
                }
            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            lblerrormsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddlrpttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            lblerrormsg.Text = "";
            hide();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            lblerrormsg.Visible = true;
            return;

        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    protected void ckgender_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            txtgender.Text = "---Select---";
            if (ckgender.Checked == true)
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = true;
                }
                txtgender.Text = "Gender(" + (cklgender.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cklgender.Items.Count; i++)
                {
                    cklgender.Items[i].Selected = false;
                }
                txtgender.Text = "---Select---";

            }

            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {

        }
    }

    protected void cklgender_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {


            int commcount = 0;
            ckgender.Checked = false;
            txtgender.Text = "---Select---";
            for (int i = 0; i < cklgender.Items.Count; i++)
            {
                if (cklgender.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txtgender.Text = "Gender(" + commcount.ToString() + ")";
                if (commcount == cklgender.Items.Count)
                {
                    ckgender.Checked = true;
                }
            }


            lblerrormsg.Text = "";
            hide();
            lblerrormsg.Visible = true;
            return;
        }
        catch (Exception ex)
        {
            lblerrormsg.Text = ex.ToString();
            lblerrormsg.Visible = true;
        }
    }
    public void hide()
    {
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;

    }
    public void bindedulevel()
    {
        string sql = "select distinct Edu_Level from course where college_code='" + Session["collegecode"].ToString() + "' order by Edu_Level desc";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        ddledulevel.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddledulevel.DataSource = ds;
            ddledulevel.DataTextField = "Edu_Level";
            ddledulevel.DataBind();
        }
    }
    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        lblerrormsg.Visible = true;
        return;
    }
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
                txtterm.Text = "Sem (" + cout + ")";
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
                txtterm.Text = "Sem (" + cout + ")";
                if (cout == cblterm.Items.Count)
                {
                    cbterm.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            //lblmsg.Visible = true;
            //lblmsg.Text = ex.ToString();
        }
    }

    public void bindsemester()
    {
        ddlSemYr.Items.Clear();
        cblterm.Items.Clear();
        DataSet studgradeds = new DataSet();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        // int i = 0;
        string strstandard = "";


        //if (ddstandard.SelectedValue != "")
        //{
        //    strstandard = ddstandard.SelectedValue;
        //}
        string batch = ddlBatch.SelectedItem.Text.ToString();
        //for (int i = 0; i < chklsbatch.Items.Count; i++)
        //{
        //    if (chklsbatch.Items[i].Selected == true)
        //    {
        //        if (batch == "")
        //        {
        //            batch = chklsbatch.Items[i].Text.ToString();
        //        }
        //        else
        //        {
        //            batch = batch + "," + chklsbatch.Items[i].Text.ToString();
        //        }

        //    }
        //}


        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (strstandard == "")
                {
                    strstandard = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    strstandard = strstandard + "','" + chklstbranch.Items[i].Value.ToString();
                }

            }
        }
        //if (strstandard.Trim() != "")
        //{
        //    strstandard = " and degree_code in(" + strstandard + ")";
        //}
        if (String.IsNullOrEmpty(strstandard))
            strstandard = "0";
        string strquery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + Session["collegecode"].ToString() + " and batch_year in (" + batch + ") and degree_code in ('" + strstandard + "') order by NDurations desc";
        studgradeds.Reset();
        studgradeds.Dispose();
        studgradeds = d2.select_method_wo_parameter(strquery, "Text");

        //studgradeds = d2.BindSem(strstandard, batch, Session["collegecode"].ToString());
        if (studgradeds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(studgradeds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(studgradeds.Tables[0].Rows[0][0].ToString());
            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    // cblterm.Items.Add(i.ToString());
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    // cblterm.Items.Add(i.ToString());
                    ddlSemYr.Items.Add(i.ToString());
                }
            }

            //if (cblterm.Items.Count > 0)
            //{
            //    int cout = 0;
            //    for (int iq = 0; iq < cblterm.Items.Count; iq++)
            //    {
            //        cout++;
            //        cblterm.Items[iq].Selected = true;
            //    }
            //    cbterm.Checked = true;
            //    txtterm.Text = "Sem (" + cout + ")";
            //}
            //else
            //{
            //    cbterm.Checked = false;
            //    txtterm.Text = "-Select-";
            //}
        }
    }

    protected void dropsubjecttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblnorec.Text = "";



        hide();
        lblerrormsg.Visible = true;
        //if (dropsubjecttype.Items.Count > 0)
        //{

        //    //bindsubject();
        //    //  hiddenfiels();
        //}

    }

    public void loadsubjecttype()
    {
        try
        {


            string strquery = "  select distinct subject_type from sub_sem order by subject_type";
            ds.Clear();
            ds = da.select_method_wo_parameter(strquery, "Text");
            dropsubjecttype.Items.Clear();

            if (ds.Tables[0].Rows.Count > 0)
            {


                dropsubjecttype.DataSource = ds;
                dropsubjecttype.DataTextField = "subject_type";
                dropsubjecttype.DataBind();
                //dropsubjecttype.Items.Insert(0, "All");


            }


        }
        catch
        {
        }
    }

    public void bindheader()
    {
        //dtshow.Columns.Add("UG/PG");
        //dtshow.Columns.Add("M/W/TR");
        //dtshow.Columns.Add("colno");
        int genderselcount = 0;
        for (int j = 0; j < cklgender.Items.Count; j++)
        {
            if (cklgender.Items[j].Selected == true)
            {
                genderselcount++;

            }
        }
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = System.Drawing.ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = System.Drawing.Color.Black;
        darkstyle.Font.Name = "Book Antiqua";
        darkstyle.Font.Size = FontUnit.Medium;
        darkstyle.Border.BorderSize = 1;
        darkstyle.Border.BorderColor = System.Drawing.Color.Black;
        FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


        //for (int i = 0; i < chklsbatch.Items.Count; i++)
        //{
        //    if (chklsbatch.Items[i].Selected == true)
        //    {
        //        if (batchyearselected == "")
        //        {
        //            batchyearselected = chklsbatch.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
        //        }

        //    }
        //}


        //for (int i = 0; i < cblterm.Items.Count; i++)
        //{
        //    if (cblterm.Items[i].Selected == true)
        //    {
        //        if (semselected == "")
        //        {
        //            semselected = cblterm.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            semselected = semselected + "','" + cblterm.Items[i].Value.ToString();
        //        }

        //    }
        //}
        batchyearselected = ddlBatch.SelectedItem.Text.ToString();
        semselected = ddlSemYr.SelectedItem.Text.ToString();


        FpSpread1.Sheets[0].RowHeader.Visible = false;
        FpSpread1.Sheets[0].AutoPostBack = true;
        FpSpread1.CommandBar.Visible = false;
        FpSpread1.Sheets[0].RowCount = 0;
        FpSpread1.Sheets[0].ColumnCount = 0;
        FpSpread1.Sheets[0].ColumnHeader.RowCount = 2;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + 1;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "DEPARTMENT";
        FpSpread1.Sheets[0].ColumnHeader.Cells[1, 0].Text = ddltype.SelectedItem.Text.ToString();
        string sql = "select distinct Edu_Level from  course where  Edu_Level ='" + ddledulevel.SelectedItem.Text.ToString() + "' order by Edu_Level desc";
        //sql = "select distinct  s.subject_name,s.subject_code from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year in ('" + batchyearselected + "') and sc.semester in('" + semselected + "') and ss.subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' ";
        sql = "select distinct subject_name,subject_code, subject_code+'-'+subject_name as subject_name from course c , Degree d,subject s,syllabus_master y, sub_sem ss  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and c.Course_Id=d.Course_Id and d.Degree_Code=y.degree_code and y.syll_code=ss.syll_code    and subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "'  and y.Batch_Year in ('" + batchyearselected + "')  and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "'  and semester in ('" + semselected + "') ; ";
        ds.Clear();
        ds = da.select_method_wo_parameter(sql, "Text");
        int col = 1;
        int stracol = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + genderselcount;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Text = ds.Tables[0].Rows[i][0].ToString();
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, col].Tag = ds.Tables[0].Rows[i][1].ToString();
                stracol = col;
                for (int j = 0; j < cklgender.Items.Count; j++)
                {
                    if (cklgender.Items[j].Selected == true)
                    {
                        string sex = "";
                        if (j == 0)
                        {
                            sex = "M";
                        }
                        else if (j == 1)
                        {
                            sex = "W";
                        }
                        else
                        {
                            sex = "TR";
                        }
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = sex;
                        FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = ds.Tables[0].Rows[i][1].ToString();

                        col++;
                    }
                }
                FpSpread1.Sheets[0].Columns.Count++;
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Text = "Total";
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();

                FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, genderselcount + 1);

                col++;
            }
        }
        stracol = col;
        //FpSpread1.Sheets[0].Columns.Count++;
        FpSpread1.Sheets[0].Columns.Count = FpSpread1.Sheets[0].Columns.Count + genderselcount;
        FpSpread1.Sheets[0].ColumnHeader.Cells[0, stracol].Text = "Total";
        //  avoidcol.Add(stracol);
        //FpSpread1.Sheets[0].ColumnHeader.Cells[1, FpSpread1.Sheets[0].Columns.Count - 1].Tag = ds.Tables[0].Rows[i][0].ToString();
        //  dtshow.Rows.Add("Total", "", FpSpread1.Sheets[0].Columns.Count - 1);
        for (int j = 0; j < cklgender.Items.Count; j++)
        {
            if (cklgender.Items[j].Selected == true)
            {
                string sex = "";
                if (j == 0)
                {
                    sex = "M";
                }
                else if (j == 1)
                {
                    sex = "W";
                }
                else
                {
                    sex = "TR";
                }
                avoidcol.Add(col);
                FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Text = sex;
                //FpSpread1.Sheets[0].ColumnHeader.Cells[1, col].Tag = year;
                // dtshow.Rows.Add(ds.Tables[0].Rows[i][0].ToString(), sex, col);
                col++;
            }
        }
        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, stracol, 1, genderselcount);

        if (FpSpread1.Sheets[0].Columns.Count > 1)
        {
            FpSpread1.Sheets[0].Columns.Count++;
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].Columns.Count - 1].Text = "G.T";
            FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].Columns.Count - 1, 2, 1);
        }
        FpSpread1.SaveChanges();
        FpSpread1.Visible = true;

    }

    public void bindvalue()
    {
        DataView dv = new DataView();
        string type = "";
        if (ddltype.Items.Count > 0)
        {
            type = ddltype.SelectedItem.Text.ToString();
        }
        string sex = "";

        ArrayList avoirows = new ArrayList();
        //Hashtable avoirows = new Hashtable();
        string edulevelid = "";
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {

        //        if (edulevelid == "")
        //        {

        //            edulevelid = ds.Tables[0].Rows[i][0].ToString();
        //        }
        //        else
        //        {

        //            edulevelid = edulevelid + "','" + ds.Tables[0].Rows[i][0].ToString();
        //        }

        //    }
        //}

        // string courseid = "";
        string deptid = "";
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                if (deptid == "")
                {
                    deptid = chklstbranch.Items[i].Value.ToString();
                }
                else
                {
                    deptid = deptid + "','" + chklstbranch.Items[i].Value.ToString();
                }

            }
        }
        for (int i = 0; i < chklstbranch.Items.Count; i++)
        {
            if (chklstbranch.Items[i].Selected == true)
            {
                // courseid = chklstdegree.Items[i].Value.ToString();

                //string sql = " select distinct (select  Dept_Name from Department d where d.Dept_Code=tb.Deptcode ) as dept_name,Deptcode from tbl_DeptGrouping tb,course c,Degree d where d.Course_Id=c.Course_Id and d.Dept_Code=tb.Deptcode  and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and c.college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'   and Groupcode in ('" + courseid + "') and Deptcode in ('" + deptid + "')";
                //ds.Clear();
                //ds = da.select_method_wo_parameter(sql, "Text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = chklstbranch.Items[i].Text.ToString();
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = chklstbranch.Items[i].Value.ToString();

                //FpSpread1.Sheets[0].Rows.Count++;
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
                //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;
                //avoirows.Add(FpSpread1.Sheets[0].RowCount - 1, courseid);
                // avoirows.Add(FpSpread1.Sheets[0].RowCount - 1);
                //FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#219DA5");
                //}
            }
        }



        //string batchyearselected = "";
        //for (int i = 0; i < chklsbatch.Items.Count; i++)
        //{
        //    if (chklsbatch.Items[i].Selected == true)
        //    {
        //        if (batchyearselected == "")
        //        {
        //            batchyearselected = chklsbatch.Items[i].Value.ToString();
        //        }
        //        else
        //        {
        //            batchyearselected = batchyearselected + "','" + chklsbatch.Items[i].Value.ToString();
        //        }

        //    }
        //}




        double ugpgtotal = 0;
        double endugpgtotal = 0;
        double overallugpgtotal = 0;
        //Hashtable tot = new Hashtable();
        //int 
        //   string sqlnew = "SELECT d.Dept_Code,dept_name,upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code   and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "') group by d.Dept_Code,dept_name,Edu_Level,sex order by d.Dept_Code ;SELECT upper(Edu_Level) as Edu_Level,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.App_No) as total FROM Registration R,applyn A,Degree G,Course C, Department d WHERE R.App_No = A.app_no AND R.degree_code = G.Degree_Code and g.Course_Id = c.Course_Id   and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and g.college_code = d.college_code    and CC=0 and DelFlag=0 and Exam_Flag='OK' and r.Batch_Year in ('" + batchyearselected + "')   group by Edu_Level,sex   ";
        //string sqlnew = "select  s.subject_name,s.subject_code,c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year in ('" + batchyearselected + "') and sc.semester in ('" + semselected + "') and ss.subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and r.degree_code in ('" + deptid + "')  and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' group by s.subject_name,s.subject_code,c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code,a.sex order by s.subject_name,c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code";
        // sqlnew = sqlnew + " ; select  c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year in ('" + batchyearselected + "') and sc.semester in ('" + semselected + "') and ss.subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "' and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "' and r.cc=0 and r.delflag=0 and r.exam_flag<>'Debar' group by c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code,a.sex order by c.type,c.Edu_Level,c.Course_Name,c.Course_Id,de.Dept_Name,d.Degree_Code";
        //sqlnew = "select  s.subject_code,c.type,c.Edu_Level,c.Course_Name,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from course c , Degree d,subject s,syllabus_master y, sub_sem ss,Registration r,applyn a,subjectChooser sc  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and c.Course_Id=d.Course_Id and d.Degree_Code=y.degree_code and y.syll_code=ss.syll_code    and subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "'  and y.Batch_Year in ('" + batchyearselected + "')  and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "'  and y.semester in ('" + semselected + "') and r.degree_code in ('" + deptid + "')  and r.Roll_No=sc.roll_no and r.App_No=a.app_no and sc.subtype_no=s.subType_no  and r.CC=0 and r.Exam_Flag<>'debar' and r.DelFlag=0  group by s.subject_code,c.type,c.Edu_Level,c.Course_Name,d.Degree_Code,sex ";
        //sqlnew = sqlnew + " ; " + "select  c.type,c.Edu_Level,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from course c , Degree d,subject s,syllabus_master y, sub_sem ss,Registration r,applyn a,subjectChooser sc  where s.syll_code = y.syll_code and ss.syll_code=s.syll_code and s.subType_no=ss.subType_no and c.Course_Id=d.Course_Id and d.Degree_Code=y.degree_code and y.syll_code=ss.syll_code    and subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "'  and y.Batch_Year in ('" + batchyearselected + "')  and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "'  and y.semester in ('" + semselected + "') and r.degree_code in ('" + deptid + "')  and r.Roll_No=sc.roll_no and r.App_No=a.app_no and sc.subtype_no=s.subType_no  and r.CC=0 and r.Exam_Flag<>'debar' and r.DelFlag=0  group by c.type,c.Edu_Level,d.Degree_Code,sex ";
        // sqlnew = "select  s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex,count(r.Roll_No) from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year='"++"' and sc.semester='"++"' and ss.subject_type='"++"' and d.Degree_Code in ('"++"') group by s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex order by s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code";
        //sqlnew = "select  s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex,count(r.Roll_No) from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "'  and y.Batch_Year in ('" + batchyearselected + "')  and c.Edu_Level='" + ddledulevel.SelectedItem.Text.ToString() + "' and c.type='" + ddltype.SelectedItem.Text.ToString() + "'  and y.semester in ('" + semselected + "') and r.degree_code in ('" + deptid + "')  group by s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex order by s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code";
        string sqlnew = "";
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                sqlnew = "select  s.subject_name,s.subject_code,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year='" + batchyearselected + "' and sc.semester='" + semselected + "' and ss.subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "' and r.degree_code='" + deptid + "' group by s.subject_name,s.subject_code,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex order by s.subject_name,c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code";
                sqlnew = sqlnew + " ; select  c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,case when sex = 0 THEN 'M'  when sex=1 then 'W' ELSE 'TR' END Sex,count(r.Roll_No) total from subjectChooser sc,subject s,sub_sem ss,Registration r,applyn a,Degree d,Course c,Department de where sc.roll_no=r.Roll_No and r.App_No=a.app_no and sc.subject_no=s.subject_no and s.subType_no=ss.subType_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=de.Dept_Code and r.Batch_Year='" + batchyearselected + "' and sc.semester='" + semselected + "' and ss.subject_type='" + dropsubjecttype.SelectedItem.Text.ToString() + "' and r.degree_code='" + deptid + "' group by c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code,a.sex order by c.type,c.Edu_Level,c.Course_Name,de.Dept_Name,d.Degree_Code";
                ds.Clear();
                ds = da.select_method_wo_parameter(sqlnew, "Text");
                if (!avoirows.Contains(i))
                {

                    for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount - 1; j++)
                    {
                        if (!avoidcol.Contains(j))
                        {
                            // string    edulevelidnn = FpSpread1.Sheets[0].ColumnHeader.Cells[0, j].Text.ToString();
                            edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();

                            // string deptidnn = FpSpread1.Sheets[0].Cells[i, 0].Text.ToString();
                            string filter = "Degree_Code='" + deptid + "' and sex='" + sex + "' and subject_code='" + edulevelid + "'";
                            if (sex.ToUpper().Trim() != "TOTAL")
                            {

                                ds.Tables[0].DefaultView.RowFilter = filter;
                                dv = ds.Tables[0].DefaultView;
                                if (dv.Count > 0)
                                {
                                    for (int m = 0; m < dv.Count; m++)
                                    {
                                        //if (!tot.Contains(sex))
                                        //{
                                        //}
                                        //else
                                        //{
                                        //}
                                        FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
                                        ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
                                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                    }


                                }
                                else
                                {
                                    FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                                }
                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(ugpgtotal);
                                ugpgtotal = 0;

                            }
                            FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                        }
                        else
                        {
                            //edulevelid = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Tag.ToString();
                            sex = FpSpread1.Sheets[0].ColumnHeader.Cells[1, j].Text.ToString();
                            deptid = FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString();
                            string filter = "Degree_Code='" + deptid + "' and sex='" + sex + "'";
                            ds.Tables[1].DefaultView.RowFilter = filter;
                            dv = ds.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                for (int m = 0; m < dv.Count; m++)
                                {
                                    //if (!tot.Contains(sex))
                                    //{
                                    //}
                                    //else
                                    //{
                                    //}
                                    FpSpread1.Sheets[0].Cells[i, j].Text = dv[0]["total"].ToString();
                                    //ugpgtotal = ugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());
                                    //overallugpgtotal = overallugpgtotal + Convert.ToDouble(dv[0]["total"].ToString());

                                }


                            }
                            else
                            {
                                FpSpread1.Sheets[0].Cells[i, j].Text = "--";

                            }
                        }
                    }
                    FpSpread1.Sheets[0].Cells[i, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(overallugpgtotal);
                    overallugpgtotal = 0;
                }


            }

        }
        int outnum = 0;
        overallugpgtotal = 0;
        FpSpread1.Sheets[0].Rows.Count++;
        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = "Total";
        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].ForeColor = Color.White;

        FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#008080");
        FpSpread1.SaveChanges();
        for (int j = 1; j < FpSpread1.Sheets[0].ColumnCount; j++)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].Rows.Count - 1; i++)
            {
                if (FpSpread1.Sheets[0].Cells[i, 0].Text.Trim().ToUpper() != "TOTAL")
                {


                    string tt = FpSpread1.Sheets[0].Cells[i, j].Text.ToString();
                    if (Int32.TryParse(tt, out outnum))
                    {
                        overallugpgtotal = overallugpgtotal + Convert.ToDouble(tt);
                        endugpgtotal = endugpgtotal + Convert.ToDouble(tt);

                    }
                }
                else
                {
                    FpSpread1.Sheets[0].Cells[i, j].Text = Convert.ToString(endugpgtotal);
                    //FpSpread1.Sheets[0].Cells[i, j].ForeColor = Color.White;
                    endugpgtotal = 0;
                    FpSpread1.Sheets[0].Cells[i, j].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[i, j].VerticalAlign = VerticalAlign.Middle;
                }

            }
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].Text = Convert.ToString(overallugpgtotal);
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].VerticalAlign = VerticalAlign.Middle;
            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, j].ForeColor = Color.White;
            overallugpgtotal = 0;

        }


        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();
        //ds.Clear();
        //ds.Dispose();
        //avoidcol.Clear();
        //avoirows.Clear();



    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        BindDegree(singleuser, group_user, collegecode, usercode);
        BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        lblerrormsg.Visible = true;
        return;
    }

}