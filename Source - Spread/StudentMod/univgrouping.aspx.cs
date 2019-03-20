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

public partial class univgrouping : System.Web.UI.Page
{
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
    DAccess2 d2 = new DAccess2();
    DataSet ds2 = new DataSet();
    Hashtable hat = new Hashtable();
    DataView dv = new DataView();
    DataSet ds = new DataSet();

    DAccess2 da = new DAccess2();
    FarPoint.Web.Spread.CheckBoxCellType chkboxcol = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.CheckBoxCellType chkboxsel_all = new FarPoint.Web.Spread.CheckBoxCellType();
    FarPoint.Web.Spread.StyleInfo darkStyle = new FarPoint.Web.Spread.StyleInfo();
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
                final.Visible = false;
                loadtype();
                Bindcollege();
                bindedulevel();
                Titlename();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindDegree(singleuser, group_user, collegecode, usercode);
                FpSpread2.Width = 320;
                FpSpread1.Width = 320;
                FpSpread1.Height = 500;
                FpSpread2.Height = 500;
                //FpSpread1.Visible = false;
                //FpSpread2.Visible = false;
                movdiv.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            lblerrmsg2.Text = "";
            lblgrperr.Text = "";
            ds.Dispose();
            ds = da.select_method("select * from sysobjects where name='tbl_UnivGrouping' and Type='U'", hat, "text ");
            if (ds.Tables[0].Rows.Count > 0)
            {

            }
            else
            {
                int p = da.insert_method(" create table tbl_UnivGrouping (Univ_code nvarchar(100),UnivText_code nvarchar(100),type nvarchar(100),college_code nvarchar(100))", hat, "text");
            }
            hide();
            lblerrormsg.Visible = true;
            FpSpread2.Sheets[0].RowHeader.Visible = false;
            FpSpread2.Sheets[0].Rows.Count = 0;
            FpSpread2.SaveChanges();
            FpSpread2.CommandBar.Visible = false;
            string groupnn = "";
            if (ddltitlename.Items.Count > 0)
            {
                groupnn = ddltitlename.SelectedItem.Value.ToString();
                groupnn = " and Univ_code ='" + groupnn + "'";
            }
            else
            {
                lblerrmsg2.Text = "Please Create Atleast One group";
                lblerrmsg2.Visible = true;
                return;
            }
            FpSpread2.Sheets[0].ColumnCount = 3;
            FpSpread2.Sheets[0].ColumnHeader.RowCount = 1;
            darkStyle.Font.Name = "Book Antiqua";
            darkStyle.Font.Bold = true;
            darkStyle.Font.Size = FontUnit.Medium;
            darkStyle.HorizontalAlign = HorizontalAlign.Center;
            darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkStyle.ForeColor = Color.Black;
            FpSpread2.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;
            FpSpread2.Sheets[0].AutoPostBack = false;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "University Name";

            FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
            FpSpread2.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpSpread2.Sheets[0].Columns[0].Locked = true;
            FpSpread2.Sheets[0].Columns[1].Locked = true;
            FpSpread2.Sheets[0].Columns[0].Width = 40;
            FpSpread2.Sheets[0].Columns[1].Width = 220;
            FpSpread2.Sheets[0].Columns[2].Width = 50;
            //FpSpread1.Sheets[0].Columns[2].Locked = true;

            //FpSpread2.Sheets[0].Columns[2].Locked = true;

            //bindsubjectpdf();
            FpSpread2.Sheets[0].RowCount = 0;

            FpSpread2.SaveChanges();
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

            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds2.Dispose();
            ds2.Reset();
            if (course_id.Trim() != "")
            {
                if (course_id.ToString().Trim() != "")
                {
                    //if (singleuser == "True")
                    //{
                    //    ds2.Dispose();
                    //    ds2.Reset();
                    //    string strquery = "select distinct department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and user_code=" + usercode + " ";
                    //    ds2 = da.select_method_wo_parameter(strquery, "Text");
                    //}
                    //else
                    //{
                    //    ds2.Dispose();
                    //    ds2.Reset();
                    //    string strquery1 = "select distinct department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in(" + course_id + ") and degree.college_code=" + collegecode + "  and deptprivilages.Degree_code=degree.Degree_code and group_code=" + group_user + "";
                    //    ds2 = da.select_method_wo_parameter(strquery1, "Text");
                    //}


                    ds2.Dispose();
                    ds2.Reset();
                    string strquery1 = "select distinct TextCode as dept_code,TextVal as dept_name from textvaltable where TextCriteria = 'cours' and college_code = '" + Session["collegecode"].ToString() + "'";
                    ds2 = da.select_method_wo_parameter(strquery1, "Text");
                }
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    FpSpread2.Sheets[0].Rows.Count++;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
                    chkboxsel_all.AutoPostBack = true;
                    ds.Clear();
                    string slq = " select UnivText_code,(select TextVal from textvaltable d where tbl_UnivGrouping.UnivText_code=d.TextCode)dept_name,(select textval from textvaltable t where tbl_UnivGrouping.Univ_code=t.TextCode)as grp  from tbl_UnivGrouping where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "' " + groupnn + "";

                    ds = da.select_method_wo_parameter(slq, "Text");
                    slq = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].Rows.Count++;

                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"].ToString());
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["UnivText_code"].ToString()); ;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;

                        if (slq == "")
                        {
                            slq = ds.Tables[0].Rows[i][0].ToString();
                        }
                        else
                        {
                            slq = slq + "','" + ds.Tables[0].Rows[i][0].ToString();

                        }
                    }
                    //if (ds.Tables[0].Rows.Count>0)
                    //{
                    //    ddltitlename.SelectedIndex = ddltitlename.Items.IndexOf(ddltitlename.Items.FindByValue(ds.Tables[0].Rows[0][2].ToString()));
                    //}
                    FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;


                    FpSpread2.SaveChanges();
                    if (slq.Trim() != "")
                    {
                        slq = "Dept_code not in ('" + slq + "')";
                        ds2.Tables[0].DefaultView.RowFilter = slq;
                        dv = ds2.Tables[0].DefaultView;
                    }
                    else
                    {
                        dv = ds2.Tables[0].DefaultView;
                    }
                    movdiv.Visible = true;
                    FpSpread1.Sheets[0].RowHeader.Visible = false;

                    FpSpread1.CommandBar.Visible = false;

                    FpSpread1.Sheets[0].ColumnCount = 3;
                    FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                    darkStyle.Font.Name = "Book Antiqua";
                    darkStyle.Font.Bold = true;
                    darkStyle.Font.Size = FontUnit.Medium;
                    darkStyle.HorizontalAlign = HorizontalAlign.Center;
                    darkStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkStyle.ForeColor = Color.Black;
                    FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = darkStyle;
                    FpSpread1.Sheets[0].AutoPostBack = false;
                    FpSpread1.Sheets[0].Columns[0].Locked = true;
                    FpSpread1.Sheets[0].Columns[1].Locked = true;
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "University Name";

                    FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Select";
                    FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Columns[0].Locked = true;
                    FpSpread1.Sheets[0].Columns[1].Locked = true;
                    FpSpread1.Sheets[0].Columns[0].Width = 40;
                    FpSpread1.Sheets[0].Columns[1].Width = 220;
                    FpSpread1.Sheets[0].Columns[2].Width = 50;
                    //FpSpread1.Sheets[0].Columns[2].Locked = true;

                    //bindsubjectpdf();
                    FpSpread1.Sheets[0].RowCount = 0;

                    FpSpread1.SaveChanges();
                    if (dv.Count > 0)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
                        chkboxsel_all.AutoPostBack = true;
                    }
                    for (int ii = 0; ii < dv.Count; ii++)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;

                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(ii + 1);
                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dv[ii]["dept_name"].ToString());
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dv[ii]["dept_code"].ToString()); ;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;

                        // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].Text = ds.Tables[0].Rows[ii]["Reg_No"].ToString();
                    }
                    //int rowheigth = FpSpread1.Sheets[0].Rows[FpSpread1.Sheets[0].RowCount - 1].Height;
                    //rowheigth = rowheigth * FpSpread1.Sheets[0].Rows.Count;
                    //FpSpread1.Height = rowheigth + 100;
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.SaveChanges();

                    FpSpread1.Visible = true;
                }
            }
            else
            {

            }






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

            string date_filt = "Batch : ";

            date_filt = date_filt + "@" + "Degree : ";
            string degreedetails = string.Empty;

            degreedetails = "Subjectwise Multiple Test Result Report" + "@" + date_filt;

            string pagename = "subjectwisemultitest.aspx";
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

                //ddltype.Items.Insert(0, "All");
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

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            ds.Clear();
            string slq = "select Course_Id from course where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'";
            //if (ddltype.SelectedItem.Text.ToString().Trim().ToUpper()=="ALL")
            //{
            //    slq = "select Course_Id from course where college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'";
            //}
            ds = da.select_method_wo_parameter(slq, "Text");
            slq = "";
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (slq == "")
                {
                    slq = ds.Tables[0].Rows[i][0].ToString();
                }
                else
                {
                    slq = slq + "','" + ds.Tables[0].Rows[i][0].ToString();

                }
            }
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
            ds2 = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (slq.Trim() != "")
            {
                slq = "course_id in ('" + slq + "')";
                ds2.Tables[0].DefaultView.RowFilter = slq;
                dv = ds2.Tables[0].DefaultView;
            }
            else
            {
                dv = ds2.Tables[0].DefaultView;
            }

            if (dv.Count > 0)
            {
                chklstdegree.DataSource = dv;
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
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            BindDegree(singleuser, group_user, collegecode1, usercode);
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
        movdiv.Visible = false;
        Printcontrol.Visible = false;
        FpSpread1.Visible = false;
        final.Visible = false;

    }

    protected void titleplus_OnClick(object sener, EventArgs e)
    {
        imgdiv1.Visible = true;
        pnltitle.Visible = true;


    }

    protected void titleminus_OnClick(object sener, EventArgs e)
    {
        try
        {
            string add = "delete from textvaltable where TextCode='" + ddltitlename.SelectedValue + "'and TextCriteria='UnvGp' and  college_code='" + Session["collegecode"].ToString() + "' ";
            int a = da.update_method_wo_parameter(add, "text");

            Titlename();
            lblerrmsg2.Text = "Group Deleted Successfully";
            lblerrmsg2.Visible = true;
            //// ------------------ start
            //string strdset = "SELECT * FROM tbl_UnivGrouping where Univ_code='" + ddltitlename.SelectedValue + "'";
            //DataSet dsetdlt = da.select_method_wo_parameter(strdset, "text");
            //// ------------------ end
            //if (dsetdlt.Tables[0].Rows.Count > 0)
            //{
            //    lblerrmsg2.Text = "This Group Name Already Used so Can't Be Deleted";
            //    lblerrmsg2.Visible = true;
            //}
            //else
            //{
            //    string add = "delete from textvaltable where TextCode='" + ddltitlename.SelectedValue + "'and TextCriteria='UnvGp' and  college_code='" + Session["collegecode"].ToString() + "' ";
            //    int a = da.update_method_wo_parameter(add, "text");
            //    // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Group Deleted Successfully')", true);
            //    Titlename();
            //    lblerrmsg2.Text = "Group Deleted Successfully";
            //    lblerrmsg2.Visible = true;
            //}
        }
        catch (Exception ex)
        {

        }
    }

    protected void btnadd2_OnClick(object sender, EventArgs e)
    {
        try
        {
            if (txttitle.Text.Trim() != "")
            {
                string ttname = ddledulevel.SelectedItem.Text.ToString() + "-" + txttitle.Text.ToString();
                string add = " if exists(select * from textvaltable where TextVal='" + ttname + "' and TextCriteria='UnvGp' and college_code='" + Session["collegecode"].ToString() + "' ) update textvaltable set TextVal='" + ttname + "',TextCriteria='UnvGp',college_code='" + Session["collegecode"].ToString() + "' where TextVal='" + ttname + "' and TextCriteria='UnvGp' and college_code='" + Session["collegecode"].ToString() + "' else insert into textvaltable (TextVal,TextCriteria,college_code) values ('" + ttname + "', 'UnvGp','" + Session["collegecode"].ToString() + "')";
                int a = da.update_method_wo_parameter(add, "text");
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Subtitle Master Name Added Successfully')", true);
                lblgrperr.Text = "Saved Successfully";
                lblgrperr.Visible = true;
                Titlename();
                txttitle.Text = "";
                imgdiv1.Visible = false;
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Enter Subtitle Master Name')", true);
            }

        }
        catch (Exception ex)
        {

        }
    }
   
    protected void btnexit2_OnClick(object sender, EventArgs e)
    {
        try
        {
            imgdiv1.Visible = false;
            pnltitle.Visible = false;
        }
        catch (Exception ex)
        {

        }
    }
    
    public void Titlename()
    {
        try
        {
            ddltitlename.Items.Clear();
            string query = "select distinct TextCode,TextVal from textvaltable where TextCriteria = 'UnvGp' and college_code = '" + Session["collegecode"].ToString() + "'";
            string strtit = "";
            ds.Clear();
            int sno = 0;
            ds = da.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string ss = ds.Tables[0].Rows[i][1].ToString();
                    string[] splitstr = ss.Split('-');
                    ListItem li = new ListItem();
                    li.Text = splitstr[1];
                    li.Value = ds.Tables[0].Rows[i][0].ToString();
                    if (ddledulevel.SelectedItem.Text == splitstr[0])
                    {
                        ddltitlename.Items.Insert(sno, li);
                        sno++;
                    }
                }
                //ddltitlename.DataSource = ds;
                //ddltitlename.DataTextField = "TextVal";
                //ddltitlename.DataValueField = "TextCode";
                //ddltitlename.DataBind();

            }

        }
        catch (Exception ex)
        {
        }
    }
    
    protected void ddltitlename_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrmsg2.Visible = false;
        hide();
    }

    protected void Btnmoveright_OnClick(object sender, EventArgs e)
    {
        //   lblgrperr.Visible = false;
        //   FpSpread1.SaveChanges();
        ////   FpSpread2.Sheets[0].RowCount = 0;
        //   FpSpread2.SaveChanges();
        //   for (int iy = 0; iy < FpSpread1.Sheets[0].RowCount; iy++)
        //   {
        //       if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[iy, 2].Value) == 1)
        //       {
        //           FpSpread2.Sheets[0].Rows.Count++;

        //           FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].Rows.Count);
        //           // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
        //           FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = FpSpread1.Sheets[0].Cells[iy, 1].Text.ToString(); ;
        //           FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(FpSpread1.Sheets[0].Cells[iy, 1].Tag.ToString());
        //           FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;
        //       }

        //   }
        //   //int rowheigth = FpSpread2.Sheets[0].Rows[FpSpread2.Sheets[0].RowCount - 1].Height;
        //   //rowheigth = rowheigth * FpSpread2.Sheets[0].Rows.Count;
        // //  FpSpread2.Height = rowheigth + 100;
        //   FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        //   FpSpread2.SaveChanges();

        //   FpSpread2.Visible = true;
        //   if (FpSpread2.Sheets[0].RowCount == 0)
        //   {
        //       lblgrperr.Text = "Please Select Atleast One Department";
        //       lblgrperr.Visible = true;
        //   }
        //   else
        //   {
        //       lblgrperr.Visible = false;
        //   }

        lblgrperr.Visible = false;
        FpSpread1.SaveChanges();
        int cccheck = 0;
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Deptname");
        dt.Columns.Add("UnivText_code");
        dt.Rows.Clear();
        DataTable dt1 = new DataTable();
        dt1.Columns.Add("Sno");
        dt1.Columns.Add("Deptname");
        dt1.Columns.Add("UnivText_code");
        dt1.Rows.Clear();

        for (int iy = 1; iy < FpSpread1.Sheets[0].RowCount; iy++)
        {
            if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[iy, 2].Value) == 0)
            {


                dt.Rows.Add(Convert.ToString(dt.Rows.Count + 1), FpSpread1.Sheets[0].Cells[iy, 1].Text.ToString(), FpSpread1.Sheets[0].Cells[iy, 1].Tag.ToString());

            }
            else
            {
                dt1.Rows.Add(Convert.ToString(dt.Rows.Count + 1), FpSpread1.Sheets[0].Cells[iy, 1].Text.ToString(), FpSpread1.Sheets[0].Cells[iy, 1].Tag.ToString());
                cccheck++;
            }
            //  FpSpread2.Sheets[0].Cells[iy, 2].Value = 0;
        }
        if (dt.Rows.Count > 0)
        {
            //FpSpread2.DataSource = dt;
            //FpSpread2.DataBind();
            FpSpread1.Sheets[0].Rows.Count = 0;
            FpSpread1.Sheets[0].Rows.Count++;
            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
            chkboxsel_all.AutoPostBack = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Rows.Count++;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(dt.Rows[i][0].ToString());
                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dt.Rows[i][1].ToString());
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dt.Rows[i][2].ToString()); ;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;


            }
        }
        else
        {
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.SaveChanges();
        }
        if (dt1.Rows.Count > 0)
        {
            //FpSpread2.DataSource = dt;
            //FpSpread2.DataBind();
            if (FpSpread2.Sheets[0].Rows.Count == 0)
            {
                FpSpread2.Sheets[0].Rows.Count++;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
                chkboxsel_all.AutoPostBack = true;
            }
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                FpSpread2.Sheets[0].Rows.Count++;

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread2.Sheets[0].Rows.Count - 1);
                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dt1.Rows[i][1].ToString());
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dt1.Rows[i][2].ToString()); ;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;


            }
            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;

            FpSpread2.SaveChanges();
        }

        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        FpSpread1.SaveChanges();

        FpSpread1.Visible = true;
        if (cccheck == 0)
        {
            lblgrperr.Text = "Please Select Atleast One Department";
            lblgrperr.Visible = true;
        }
        else
        {
            lblgrperr.Visible = false;
        }


    }
    
    protected void Btnmoveleft_OnClick(object sender, EventArgs e)
    {
        lblgrperr.Visible = false;
        FpSpread2.SaveChanges();
        int cccheck = 0;
        DataTable dt = new DataTable();
        dt.Columns.Add("Sno");
        dt.Columns.Add("Deptname");
        dt.Columns.Add("UnivText_code");
        dt.Rows.Clear();
        DataTable dt1 = new DataTable();
        dt1.Columns.Add("Sno");
        dt1.Columns.Add("Deptname");
        dt1.Columns.Add("UnivText_code");
        dt1.Rows.Clear();

        for (int iy = 1; iy < FpSpread2.Sheets[0].RowCount; iy++)
        {
            if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[iy, 2].Value) == 0)
            {


                dt.Rows.Add(Convert.ToString(dt.Rows.Count + 1), FpSpread2.Sheets[0].Cells[iy, 1].Text.ToString(), FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString());

            }
            else
            {
                dt1.Rows.Add(Convert.ToString(dt1.Rows.Count + 1), FpSpread2.Sheets[0].Cells[iy, 1].Text.ToString(), FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString());
                cccheck++;
            }
            //  FpSpread2.Sheets[0].Cells[iy, 2].Value = 0;
        }
        if (dt.Rows.Count > 0)
        {
            //FpSpread2.DataSource = dt;
            //FpSpread2.DataBind();
            FpSpread2.Sheets[0].Rows.Count = 0;
            FpSpread2.Sheets[0].Rows.Count++;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
            chkboxsel_all.AutoPostBack = true;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                FpSpread2.Sheets[0].Rows.Count++;

                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(dt.Rows[i][0].ToString());
                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dt.Rows[i][1].ToString());
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dt.Rows[i][2].ToString()); ;
                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;


            }
        }
        else
        {
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.SaveChanges();
        }
        if (dt1.Rows.Count > 0)
        {
            //FpSpread2.DataSource = dt;
            //FpSpread2.DataBind();
            if (FpSpread1.Sheets[0].Rows.Count == 0)
            {
                FpSpread1.Sheets[0].Rows.Count++;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxsel_all;
                chkboxsel_all.AutoPostBack = true;
            }

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                FpSpread1.Sheets[0].Rows.Count++;

                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(dt1.Rows[i][1].ToString());
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(dt1.Rows[i][2].ToString()); ;
                FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;


            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;

            FpSpread1.SaveChanges();
        }

        FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
        FpSpread2.SaveChanges();

        FpSpread2.Visible = true;
        if (cccheck == 0)
        {
            lblgrperr.Text = "Please Select Atleast One Department";
            lblgrperr.Visible = true;
        }
        else
        {
            lblgrperr.Visible = false;
        }

    }
    
    protected void btnsave_OnClick(object sender, EventArgs e)
    {
        int p = 0;
        if (ddltitlename.Items.Count > 0)
        {
            if (FpSpread2.Sheets[0].RowCount > 0)
            {
                string grpcode = ddltitlename.SelectedItem.Value.ToString();
                ds.Dispose();
                ds = da.select_method("select * from sysobjects where name='tbl_UnivGrouping' and Type='U'", hat, "text ");
                if (ds.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    p = da.insert_method(" create table tbl_UnivGrouping (Univ_code nvarchar(100),UnivText_code nvarchar(100),type nvarchar(100),college_code nvarchar(100))", hat, "text");
                }
                p = da.insert_method("delete  from tbl_UnivGrouping where Univ_code='" + grpcode + "' and type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'", hat, "text");
                for (int iy = 1; iy < FpSpread2.Sheets[0].RowCount; iy++)
                {
                    string UnivText_code = Convert.ToString(FpSpread2.Sheets[0].Cells[iy, 1].Tag.ToString());

                    p = da.insert_method("insert into tbl_UnivGrouping (Univ_code,UnivText_code,type,college_code) values ('" + grpcode + "', '" + UnivText_code + "','" + ddltype.SelectedItem.Text.ToString() + "','" + ddlcollege.SelectedItem.Value.ToString() + "')", hat, "text");


                }
                lblgrperr.Text = "Saved Successfully";
                lblgrperr.Visible = true;
            }
            else
            {
                lblgrperr.Text = "Please Select Atleast One Department";
                lblgrperr.Visible = true;
            }
        }
        else
        {
            lblgrperr.Text = "Please Create Group Name";
            lblgrperr.Visible = true;
            //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Create Group Name')", true);

        }
    }
    
    public void bindgrpfp()
    {

        ds.Clear();
        string slq = " select UnivText_code,(select dept_name from Department d where tbl_UnivGrouping.UnivText_code=d.dept_code)dept_name,(select textval from textvaltable t where tbl_UnivGrouping.Univ_code=t.TextCode)as grp  from tbl_UnivGrouping where type='" + ddltype.SelectedItem.Text.ToString() + "' and college_code='" + ddlcollege.SelectedItem.Value.ToString() + "'";

        ds = da.select_method_wo_parameter(slq, "Text");
        slq = "";
        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        {
            FpSpread2.Sheets[0].Rows.Count++;

            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 0].Text = Convert.ToString(i + 1);
            // FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 2].CellType = txtceltype;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["dept_name"].ToString());
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].Rows.Count - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["UnivText_code"].ToString()); ;
            FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].CellType = chkboxcol;

            if (slq == "")
            {
                slq = ds.Tables[0].Rows[i][0].ToString();
            }
            else
            {
                slq = slq + "','" + ds.Tables[0].Rows[i][0].ToString();

            }
        }
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddltitlename.SelectedIndex = ddltitlename.Items.IndexOf(ddltitlename.Items.FindByValue(ds.Tables[0].Rows[0][2].ToString()));
        }
        FpSpread2.SaveChanges();
    }
    
    protected void ddledulevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerrormsg.Text = "";
        hide();
        Titlename();
        // BindDegree(singleuser, group_user, collegecode, usercode);
        //BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        lblerrormsg.Visible = true;
        return;
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
    
    protected void Fpspread1_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 2].Value) == 1)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 2].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 2].Value) == 0)
        {
            for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                FpSpread1.Sheets[0].Cells[i, 2].Value = 0;
            }

        }

        FpSpread1.Visible = true;
    }
    
    protected void Fpspread2_Command(object sender, EventArgs e)
    {

        if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 2].Value) == 1)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 2].Value = 1;
            }
        }
        else if (Convert.ToInt32(FpSpread2.Sheets[0].Cells[0, 2].Value) == 0)
        {
            for (int i = 0; i < FpSpread2.Sheets[0].RowCount; i++)
            {
                FpSpread2.Sheets[0].Cells[i, 2].Value = 0;
            }

        }

        FpSpread2.Visible = true;
    }
}