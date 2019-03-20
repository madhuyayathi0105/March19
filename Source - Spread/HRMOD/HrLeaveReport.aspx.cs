using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Drawing;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class HrLeaveReport : System.Web.UI.Page
{
    #region "Load Details"
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
    Hashtable hat = new Hashtable();
    string strisstaff = "";
    string grouporusercode = "";
    Boolean oldNew = false;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);

    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(55);
            img.Height = Unit.Percentage(55);
            return img;


        }
    }

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
        try
        {
            //****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("HRM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["HR"].Trim();
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

            if (!Request.FilePath.Contains("HRMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/HRMOD/HRMenuIndex.aspx");
                    return;
                }
            }
            //****************************************************//
            //rdbtnlst.Items[0].Selected = true;
            if (rdbtnlst.Items[0].Selected == true)
            {
                lblleave.Visible = true;
                chklsleave.Visible = true;
            }
            else if (rdbtnlst.Items[1].Selected == true)
            {
                //lblleave.Visible = false;
                chklsleave.Visible = false;
            }

            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
            {
                grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
            }
            else
            {
                grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
            }

            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();

            strisstaff = "" + Session["Staff_Code"].ToString();
            if (!Page.IsPostBack)
            {
                FpstaffLeave.Width = 1000;
                FpstaffLeave.Sheets[0].AutoPostBack = true;
                FpstaffLeave.CommandBar.Visible = true;
                FpstaffLeave.Sheets[0].SheetName = " ";
                FpstaffLeave.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpstaffLeave.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpstaffLeave.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpstaffLeave.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpstaffLeave.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpstaffLeave.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpstaffLeave.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                FpstaffLeave.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpstaffLeave.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpstaffLeave.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpstaffLeave.Sheets[0].AllowTableCorner = true;

                //---------------page number

                FpstaffLeave.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpstaffLeave.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpstaffLeave.Pager.Align = HorizontalAlign.Right;
                FpstaffLeave.Pager.Font.Bold = true;
                FpstaffLeave.Pager.Font.Name = "Book Antiqua";
                FpstaffLeave.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpstaffLeave.Pager.BackColor = System.Drawing.Color.Beige;
                FpstaffLeave.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpstaffLeave.Pager.PageCount = 100;
                FpstaffLeave.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                txtrptname.Visible = false;
                lblrptname.Visible = false;
                txtfrom.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtto.Text = DateTime.Now.ToString("dd/MM/yyyy");
                BindLeave();
                string strval = d2.GetFunction("select value from Master_Settings where settings='Staff Leave Report Visible Department Wise' and " + grouporusercode + "");
                if (strisstaff.ToLower().Trim() == "" || strval == "1")
                {

                    txtcategory.Enabled = true;
                    txtdept.Enabled = true;
                    txtdesign.Enabled = true;
                    txtstaff.Enabled = true;
                    txttype.Enabled = true;
                    ddlleavevalue.Enabled = true;
                    txtvalue.Enabled = true;
                    BindDesignation();
                    BindDepartment();
                    BindCategory();
                    BindType();
                    //BindDesignation();
                    BindStaff();

                }
                else
                {
                    ddlleavevalue.Enabled = false;
                    txtvalue.Enabled = false;
                    txtcategory.Enabled = false;
                    txtdept.Enabled = false;
                    txtdesign.Enabled = false;
                    txtstaff.Enabled = false;
                    txttype.Enabled = false;
                    // loaddetails();
                }

                btnprintmaster.Visible = false;
                //added by srinath 28/4/2014
                // chkdate.Enabled = false;
                //chkdate.Checked = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
        //}
    }
    //Load Degisnation
    public void BindDesignation()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            string strdesigquery = "select distinct desig_name,desig_code from desig_master where  collegeCode=" + collegecode + "";
            ds = d2.select_method(strdesigquery, hat, "Text");
            chklsdesign.DataSource = ds;
            chklsdesign.DataValueField = "desig_code";
            chklsdesign.DataTextField = "desig_name";
            chklsdesign.DataBind();
            for (int item = 0; item < chklsdesign.Items.Count; item++)
            {
                chklsdesign.Items[item].Selected = true;
            }

            chkdesign.Checked = true;
            txtdesign.Text = "Desig (" + chklsdesign.Items.Count + ")";
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    //Load Department
    //public void BindDepartment()
    //{
    //    try
    //    {
    //        ds.Dispose();
    //        ds.Reset();
    //        //ds = d2.loaddepartment(collegecode);
    //        string deptquery = "";
    //        string singleuser = Session["single_user"].ToString();
    //        if (singleuser == "True")
    //        {
    //            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
    //        }

    //        else
    //        {

    //            group_user = Session["group_code"].ToString();
    //            if (group_user.Contains(';'))
    //            {
    //                string[] group_semi = group_user.Split(';');
    //                group_user = group_semi[0].ToString();
    //            }
    //            deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') ";
    //        }
    //        if (deptquery != "")
    //        {
    //            ds = d2.select_method(deptquery, hat, "Text");
    //            chklsdept.DataSource = ds;
    //            chklsdept.DataTextField = "dept_name";
    //            chklsdept.DataValueField = "Dept_Code";
    //            chklsdept.DataBind();
    //            for (int item = 0; item < chklsdept.Items.Count; item++)
    //            {
    //                chklsdept.Items[item].Selected = true;
    //            }
    //            if (chklsdesign.Items.Count > 0)
    //            {
    //                txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
    //                chkdept.Checked = true;
    //            }
    //            else
    //            {
    //                txtdept.Text = "---Select---";
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        errmsg.Text = ex.ToString();
    //        errmsg.Visible = true;
    //    }
    //}
    //Load Category

    public void BindDepartment()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            //ds = d2.loaddepartment(collegecode);
            string deptquery = "";
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
            }

            else
            {

                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                deptquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + Session["collegecode"] + "') order by dept_name";
            }
            if (deptquery != "")
            {
                ds = d2.select_method(deptquery, hat, "Text");
                chklsdept.DataSource = ds;
                chklsdept.DataTextField = "dept_name";
                chklsdept.DataValueField = "Dept_Code";
                chklsdept.DataBind();
                for (int item = 0; item < chklsdept.Items.Count; item++)
                {
                    chklsdept.Items[item].Selected = true;
                }
                if (chklsdesign.Items.Count > 0)
                {
                    txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
                    chkdept.Checked = true;
                }
                else
                {
                    txtdept.Text = "---Select---";
                }
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    public void BindCategory()
    {
        try
        {
            ds.Clear();
            string strcategory = "select  distinct category_code,category_name from staffcategorizer where college_code='" + collegecode + "'";
            ds = d2.select_method(strcategory, hat, "Text");
            chklscategory.DataSource = ds.Tables[0];
            chklscategory.DataTextField = "category_name";
            chklscategory.DataValueField = "category_code";
            chklscategory.DataBind();
            for (int item = 0; item < chklscategory.Items.Count; item++)
            {
                chklscategory.Items[item].Selected = true;
            }
            if (chklscategory.Items.Count > 0)
            {
                txtcategory.Text = "Category (" + chklscategory.Items.Count + ")";
                chkcategory.Checked = true;
            }
            else
            {
                txtcategory.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }
    //Load Type
    public void BindType()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            string strtype = "select distinct stftype from stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and college_code='" + collegecode + "'";
            ds = d2.select_method(strtype, hat, "Text");
            chklstype.DataSource = ds;
            chklstype.DataTextField = "stftype";
            chklstype.DataValueField = "stftype";
            chklstype.DataBind();
            for (int item = 0; item < chklstype.Items.Count; item++)
            {
                chklstype.Items[item].Selected = true;
            }


            if (chklscategory.Items.Count > 0)
            {
                txttype.Text = "Type (" + chklstype.Items.Count + ")";
                chktype.Checked = true;
            }
            else
            {
                txttype.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }
    //Load Leave 
    public void BindLeave()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            string strleave = "select distinct category,LeaveMasterPK from leave_category where college_code=" + collegecode + "";
            ds = d2.select_method(strleave, hat, "Text");
            chklsleave.DataSource = ds;
            chklsleave.DataTextField = "category";
            chklsleave.DataValueField = "LeaveMasterPK";

            chklsleave.DataBind();
            //if (ds.Tables[0].Rows.Count != 0)
            //{
            //    chklsleave.Items.Insert(1, "PERMISSION");
            //    chklsleave.Items.Insert(1, "LATE");
            //}
            for (int item = 0; item < chklsleave.Items.Count; item++)
            {
                chklsleave.Items[item].Selected = true;
            }
            if (chklsleave.Items.Count > 0)
            {
                txtleave.Text = "Leave (" + chklsleave.Items.Count + ")";
                chkleave.Checked = true;
            }
            else
            {
                txtleave.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    //Load Staff
    public void BindStaff()
    {
        try
        {
            string deptcode = "";
            for (int item = 0; item < chklsdept.Items.Count; item++)
            {
                if (chklsdept.Items[item].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = chklsdept.Items[item].Value;
                    }
                    else
                    {
                        deptcode = deptcode + ',' + chklsdept.Items[item].Value;
                    }
                }
            }
            if (deptcode != "")
            {
                deptcode = "and st.dept_code in(" + deptcode + ")";
            }

            string designcode = "";
            for (int item = 0; item < chklsdesign.Items.Count; item++)
            {
                if (chklsdesign.Items[item].Selected == true)
                {
                    if (designcode == "")
                    {
                        designcode = "'" + chklsdesign.Items[item].Value + "'";
                    }
                    else
                    {
                        designcode = designcode + ',' + "'" + chklsdesign.Items[item].Value + "'";
                    }
                }
            }
            if (designcode != "")
            {
                designcode = "and st.desig_code in(" + designcode + ")";
            }

            string catecode = "";
            for (int item = 0; item < chklscategory.Items.Count; item++)
            {
                if (chklscategory.Items[item].Selected == true)
                {
                    if (catecode == "")
                    {
                        catecode = "'" + chklscategory.Items[item].Value + "'";
                    }
                    else
                    {
                        catecode = catecode + ',' + "'" + chklscategory.Items[item].Value + "'";
                    }
                }
            }
            if (catecode != "")
            {
                catecode = " and st.category_code in(" + catecode + ")";
            }
            string type = "";
            for (int item = 0; item < chklstype.Items.Count; item++)
            {
                if (chklstype.Items[item].Selected == true)
                {
                    if (type == "")
                    {
                        type = "'" + chklstype.Items[item].Value + "'";
                    }
                    else
                    {
                        type = type + ',' + "'" + chklstype.Items[item].Value + "'";
                    }
                }
            }
            if (type != "")
            {
                type = " and st.stftype in(" + type + ")";
            }
            string strstaffquery = "select distinct sm.staff_name,st.staff_code from stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and sm.settled=0 and sm.resign=0 and st.latestrec=1 " + deptcode + " " + designcode + " " + catecode + " " + type + " and college_code =" + collegecode + "";
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(strstaffquery, hat, "Text");
            chklsstaff.DataSource = ds;
            chklsstaff.DataTextField = "staff_name";
            chklsstaff.DataValueField = "staff_code";
            chklsstaff.DataBind();
            for (int item = 0; item < chklsstaff.Items.Count; item++)
            {
                chklsstaff.Items[item].Selected = true;
            }

            if (chklsstaff.Items.Count > 0)
            {
                chkstaff.Checked = true;
                txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
            }
            else
            {
                chkstaff.Checked = false;
                txtstaff.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }
    protected void chkdept_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkdept.Checked == true)
            {
                for (int item = 0; item < chklsdept.Items.Count; item++)
                {
                    chklsdept.Items[item].Selected = true;
                }
                txtdept.Text = "Dept (" + chklsdept.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklsdept.Items.Count; item++)
                {
                    chklsdept.Items[item].Selected = false;
                }
                txtdept.Text = "---Select---";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdept.Text = "Dept (" + commcount.ToString() + ")";

                }
            }
            if (commcount == 0)
            {
                txtdept.Text = "--Select--";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chkdesign_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkdesign.Checked == true)
            {
                for (int item = 0; item < chklsdesign.Items.Count; item++)
                {
                    chklsdesign.Items[item].Selected = true;
                }
                txtdesign.Text = "Desig (" + chklsdesign.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklsdesign.Items.Count; item++)
                {
                    chklsdesign.Items[item].Selected = false;
                }
                txtdesign.Text = "---Select---";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }

    }
    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtdesign.Text = "Desig (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtdesign.Text = "--Select--";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chkcategory_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkcategory.Checked == true)
            {
                for (int item = 0; item < chklscategory.Items.Count; item++)
                {
                    chklscategory.Items[item].Selected = true;
                }
                txtcategory.Text = "Category (" + chklscategory.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklscategory.Items.Count; item++)
                {
                    chklscategory.Items[item].Selected = false;
                }
                txtcategory.Text = "---Select---";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chktype_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chktype.Checked == true)
            {
                for (int item = 0; item < chklstype.Items.Count; item++)
                {
                    chklstype.Items[item].Selected = true;
                }
                txttype.Text = "Type (" + chklstype.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklstype.Items.Count; item++)
                {
                    chklstype.Items[item].Selected = false;
                }
                txttype.Text = "---Select---";
            }
            BindStaff();
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chklstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklstype.Items.Count; i++)
            {
                if (chklstype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txttype.Text = "Type (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txttype.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chkstaff_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkstaff.Checked == true)
            {
                for (int item = 0; item < chklsstaff.Items.Count; item++)
                {
                    chklsstaff.Items[item].Selected = true;
                }
                txtstaff.Text = "Staff (" + chklsstaff.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklsstaff.Items.Count; item++)
                {
                    chklsstaff.Items[item].Selected = false;
                }
                txtstaff.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chklsstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            for (int i = 0; i < chklsstaff.Items.Count; i++)
            {
                if (chklsstaff.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtstaff.Text = "Staff (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtstaff.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chklsleave_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            for (int i = 0; i < chklsleave.Items.Count; i++)
            {
                if (chklsleave.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    txtleave.Text = "Leave (" + commcount.ToString() + ")";
                }
            }
            if (commcount == 0)
            {
                txtleave.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void chkleave_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (chkleave.Checked == true)
            {
                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = true;
                }
                txtleave.Text = "Leave (" + chklsleave.Items.Count + ")";
            }
            else
            {
                for (int item = 0; item < chklsleave.Items.Count; item++)
                {
                    chklsleave.Items[item].Selected = false;
                }
                txtleave.Text = "---Select---";
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void txtfrom_TextChanged(object sender, EventArgs e)
    {
        string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
        DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
        string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
        DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
        if (fromdate > todate)
        {
            errmsg.Text = "Please Enter From Date Less Than To Date";
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    protected void txtto_TextChanged(object sender, EventArgs e)
    {
        string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
        DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
        string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
        DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
        if (fromdate > todate)
        {
            errmsg.Text = "Please Enter From Date Less Than To Date";
            errmsg.Visible = true;
        }
        else
        {
            errmsg.Visible = false;
        }
    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)//delsi
    {
        try
        {
            bool NeworOld = false;
            bool old_newRecord = false;

            //    if (NeworOld == true)
            //    {
            #region OldReport
            string strlrights = d2.GetFunction("select value from Master_Settings where settings='Staff Leave Report Visible Department Wise' and " + grouporusercode + "");
            //senthil(24.06.15)
            if (strisstaff.ToLower().Trim() == "" || strlrights == "1")
            {
                if (txtdept.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Department Name\");", true);
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    return;
                }

                if (txtdesign.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Designation Name\");", true);
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    return;
                }
                if (txtcategory.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Category\");", true);
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    return;

                }
                if (txttype.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select StaffType\");", true);
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    return;
                }
            }
            if (txtleave.Text == "---Select---")
            {
                if (rdbtnlst.SelectedItem.Text.Trim().ToUpper() == "LEAVE")
                {

                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select LeaveType\");", true);
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    return;
                }
            }
            if (rdbtnlst.Items[0].Selected == true)
            {
                if (strisstaff.ToLower().Trim() == "" || strlrights == "1")
                {
                    errmsg.Visible = false;
                    btnxl.Visible = true;
                    lblrptname.Visible = true;
                    txtrptname.Visible = true;

                    string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
                    DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
                    string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
                    DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
                    if (fromdate > todate)
                    {
                        errmsg.Text = "Please Enter From Date Less Than To Date";
                        errmsg.Visible = true;
                    }
                    else
                    {
                        string leavestaffacode = "";
                        for (int i = 0; i < chklsstaff.Items.Count; i++)
                        {
                            if (chklsstaff.Items[i].Selected == true)
                            {
                                if (leavestaffacode == "")
                                {
                                    leavestaffacode = "'" + chklsstaff.Items[i].Value + "'";
                                }
                                else
                                {
                                    leavestaffacode = leavestaffacode + ',' + "'" + chklsstaff.Items[i].Value + "'";
                                }

                            }
                        }
                        string leavetype = "";
                        for (int i = 0; i < chklsleave.Items.Count; i++)
                        {
                            if (chklsleave.Items[i].Selected == true)
                            {
                                if (leavetype == "")
                                {
                                    leavetype = "sl.lt_taken='" + chklsleave.Items[i].Text + "'";
                                    leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Text + "'";
                                    leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Text + "'";
                                }
                                else
                                {
                                    leavetype = "" + leavetype + " or " + "sl.lt_taken='" + chklsleave.Items[i].Text + "'";
                                    leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Text + "'";
                                    leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Text + "'";
                                }

                            }
                        }
                        if (leavetype != "")
                        {
                            leavetype = "and (" + leavetype + ")";
                        }
                        if (leavestaffacode != "")
                        {
                            string[] splitdatefrom = txtfrom.Text.ToString().Trim().Split('/');
                            string fromdate1 = splitdatefrom[1] + '/' + splitdatefrom[0] + '/' + splitdatefrom[2];
                            string[] splitdateto = txtto.Text.ToString().Trim().Split('/');
                            string todate1 = splitdateto[1] + '/' + splitdateto[0] + '/' + splitdateto[2];

                            string tempstaffcode = "";
                            double totlalleavedays = 0;
                            //bind query
                            string strstaffleavequery = " select distinct sm.staff_code,sm.Staff_name,hm.dept_name,sl.lt_taken,sl.fdate,sl.tdate,sl.Half_Days,sl.remarks from Staff_leave_details sl,staffmaster sm,stafftrans st,hrdept_master hm where sl.college_code=sm.college_code and sm.college_code=hm.college_code and sl.staff_code=sm.staff_code and sm.staff_code=st.staff_code and apply_approve=1 and st.dept_code=hm.dept_code and sm.settled=0 and sm.resign=0 and ISNULL(sm.Discontinue,'0')='0' and st.latestrec=1 and sm.college_code='" + Convert.ToString(Session["collegecode"]) + "' and st.staff_code in(" + leavestaffacode.ToString().Trim() + ") and ((fdate between '" + fromdate1 + "' and '" + todate1 + "') or (tdate between '" + fromdate1 + "' and '" + todate1 + "')) " + leavetype + " order by sm.Staff_name,hm.dept_name,sl.fdate";//this condition (apply_approve=1) added in this query by Manikandan on 04/09/2013
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method(strstaffleavequery, hat, "Text");
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                old_newRecord = true;

                            }

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                LoadHeader();
                                int spancolumn = 0;
                                int startrow = 0;
                                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                                {
                                    string staffcode = ds.Tables[0].Rows[row]["staff_code"].ToString();
                                    string[] fromdateset = ds.Tables[0].Rows[row]["fdate"].ToString().Split(' ');
                                    string[] fromdateset1 = fromdateset[0].Split('/');
                                    string fromdateset2 = Convert.ToString(fromdateset1[1] + '/' + fromdateset1[0] + '/' + fromdateset1[2]);
                                    string[] todateset = ds.Tables[0].Rows[row]["tdate"].ToString().Split(' ');
                                    string[] tomdateset1 = todateset[0].Split('/');
                                    string todateset2 = Convert.ToString(tomdateset1[1] + '/' + tomdateset1[0] + '/' + tomdateset1[2]);
                                    DateTime dtfrom = Convert.ToDateTime(fromdateset[0]);
                                    DateTime dtto = Convert.ToDateTime(todateset[0]);
                                    DateTime reporttodate = Convert.ToDateTime(todate1);
                                    DateTime reportfromdate = Convert.ToDateTime(fromdate1);
                                    DateTime difftodate;
                                    if (dtto > reporttodate)
                                    {
                                        difftodate = reporttodate;
                                    }
                                    else
                                    {
                                        difftodate = dtto;
                                    }

                                    DateTime difffromdate;
                                    if (dtfrom < reportfromdate)
                                    {
                                        difffromdate = reportfromdate;
                                    }
                                    else
                                    {
                                        difffromdate = dtfrom;
                                    }
                                    TimeSpan t = difftodate - difffromdate;
                                    double NrOfDays = t.TotalDays;
                                    NrOfDays++;
                                    int haldaycount = 0;
                                    string hallfday = ds.Tables[0].Rows[row]["Half_Days"].ToString();
                                    if (hallfday.ToString().Trim() != "" && hallfday.ToString().Trim() != "0")
                                    {
                                        haldaycount = Convert.ToInt32(hallfday);
                                        NrOfDays = NrOfDays - (Convert.ToDouble(haldaycount * 0.5));

                                    }

                                    string gethalfleavedetails = "";
                                    string LeaveTake = ds.Tables[0].Rows[row]["lt_taken"].ToString();
                                    string[] spitLeavetake = LeaveTake.Split('@');
                                    if (spitLeavetake.GetUpperBound(0) == 2)
                                    {
                                        if (spitLeavetake[1].ToString().Trim().ToLower() == "fh")
                                        {
                                            gethalfleavedetails = " (Morning)";
                                        }
                                        else if (spitLeavetake[1].ToString().Trim().ToLower() == "sh")
                                        {
                                            gethalfleavedetails = " (Evening)";
                                        }
                                        LeaveTake = spitLeavetake[2].ToString();
                                    }




                                    if (txtvalue.Text != "")
                                    {
                                        int days = Convert.ToInt32(txtvalue.Text);
                                        if (ddlleavevalue.SelectedValue.ToString().Trim() == "2")
                                        {
                                            if (days == NrOfDays)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[row]["Staff_code"].ToString();

                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[row]["Staff_Name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[row]["dept_name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = LeaveTake;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = fromdateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = todateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = NrOfDays.ToString() + gethalfleavedetails;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[row]["remarks"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                                if (tempstaffcode == "")
                                                {
                                                    tempstaffcode = staffcode;
                                                    totlalleavedays = NrOfDays;
                                                    spancolumn = 1;
                                                    startrow = 0;
                                                }
                                                else
                                                {
                                                    if (tempstaffcode == staffcode)
                                                    {
                                                        totlalleavedays = totlalleavedays + NrOfDays;
                                                        spancolumn++;
                                                    }
                                                    else
                                                    {
                                                        if (FpstaffLeave.Sheets[0].RowCount > 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                            FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        else
                                                        {
                                                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        if (row == ds.Tables[0].Rows.Count - 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                            FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();

                                                        }
                                                        totlalleavedays = NrOfDays;
                                                        tempstaffcode = staffcode;
                                                        spancolumn = 1;
                                                        startrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                    }
                                                }
                                            }
                                        }
                                        else if (ddlleavevalue.SelectedValue.ToString().Trim() == "1")
                                        {
                                            if (days > NrOfDays)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[row]["Staff_code"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[row]["Staff_Name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[row]["dept_name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = LeaveTake;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = fromdateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = todateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = NrOfDays.ToString() + gethalfleavedetails;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[row]["remarks"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                                if (tempstaffcode == "")
                                                {
                                                    tempstaffcode = staffcode;
                                                    totlalleavedays = NrOfDays;
                                                    startrow = 0;
                                                    spancolumn = 1;
                                                }
                                                else
                                                {
                                                    if (tempstaffcode == staffcode)
                                                    {
                                                        totlalleavedays = totlalleavedays + NrOfDays;
                                                        spancolumn++;
                                                    }
                                                    else
                                                    {
                                                        if (FpstaffLeave.Sheets[0].RowCount > 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                            FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        else
                                                        {
                                                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        if (row == ds.Tables[0].Rows.Count - 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                            FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        totlalleavedays = NrOfDays;
                                                        tempstaffcode = staffcode;
                                                        startrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                        spancolumn = 1;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (days < NrOfDays)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[row]["Staff_code"].ToString();

                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[row]["Staff_Name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[row]["dept_name"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = LeaveTake;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = fromdateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = todateset2;
                                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = NrOfDays.ToString() + gethalfleavedetails;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[row]["remarks"].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                                if (tempstaffcode == "")
                                                {
                                                    tempstaffcode = staffcode;
                                                    totlalleavedays = NrOfDays;
                                                    startrow = 0;
                                                    spancolumn = 1;
                                                }
                                                else
                                                {
                                                    if (tempstaffcode == staffcode)
                                                    {
                                                        totlalleavedays = totlalleavedays + NrOfDays;
                                                        spancolumn++;
                                                    }
                                                    else
                                                    {
                                                        if (FpstaffLeave.Sheets[0].RowCount > 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 2, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        else
                                                        {
                                                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        if (row == ds.Tables[0].Rows.Count - 1)
                                                        {
                                                            FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                            FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                                        }
                                                        totlalleavedays = NrOfDays;
                                                        tempstaffcode = staffcode;
                                                        spancolumn++;
                                                        startrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        FpstaffLeave.Sheets[0].RowCount++;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[row]["Staff_code"].ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[row]["Staff_Name"].ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[row]["dept_name"].ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = LeaveTake;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = fromdateset2;
                                        //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = todateset2;
                                        //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = NrOfDays.ToString() + gethalfleavedetails;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[row]["remarks"].ToString();

                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                                        if (tempstaffcode == "")
                                        {
                                            tempstaffcode = staffcode;
                                            totlalleavedays = NrOfDays;
                                            spancolumn = 1;
                                            startrow = 0;
                                        }
                                        else
                                        {
                                            if (tempstaffcode == staffcode)
                                            {
                                                totlalleavedays = totlalleavedays + NrOfDays;
                                                spancolumn++;
                                            }
                                            else
                                            {
                                                if (FpstaffLeave.Sheets[0].RowCount > 1)
                                                {
                                                    FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                    FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();

                                                }
                                                else
                                                {
                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 9].Text = totlalleavedays.ToString();
                                                }

                                                if (row == ds.Tables[0].Rows.Count - 1)
                                                {
                                                    FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                                    FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                                }
                                                totlalleavedays = NrOfDays;
                                                tempstaffcode = staffcode;
                                                spancolumn = 1;
                                                startrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                            }
                                        }
                                    }
                                }
                                if (startrow > 0 && spancolumn > 0)
                                {
                                    FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                    FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                }
                                else
                                {
                                    FpstaffLeave.Sheets[0].SpanModel.Add(startrow, 9, spancolumn, 1);
                                    FpstaffLeave.Sheets[0].Cells[startrow, 9].Text = totlalleavedays.ToString();
                                }

                            }
                            else
                            {
                                FpstaffLeave.Visible = false;
                                btnprintmaster.Visible = false;
                                btnxl.Visible = false;
                                lblrptname.Visible = false;
                                txtrptname.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";

                            }
                            int rowcount = FpstaffLeave.Sheets[0].RowCount;
                            FpstaffLeave.Height = 300;
                            FpstaffLeave.Sheets[0].PageSize = 25 + (rowcount * 20);
                            FpstaffLeave.SaveChanges();

                            if (FpstaffLeave.Sheets[0].RowCount == 0)
                            {
                                FpstaffLeave.Visible = false;
                                btnprintmaster.Visible = false;
                                btnxl.Visible = false;
                                lblrptname.Visible = false;
                                txtrptname.Visible = false;
                                errmsg.Visible = true;
                                errmsg.Text = "No Records Found";
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Staff Name\");", true);
                            FpstaffLeave.Visible = false;
                            btnprintmaster.Visible = false;
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtrptname.Visible = false;
                        }
                    }
                }
                //}
                else
                {
                    loaddetails();
                }
            }
            else
            {
                staffAbsent(strlrights);
            }
            if (FpstaffLeave.Sheets[0].RowCount == 0)
            {
                FpstaffLeave.Visible = false;
                btnprintmaster.Visible = false;
                lblrptname.Visible = false;
                txtrptname.Visible = false;
                btnxl.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            #endregion
            //  }
            //   else if (NeworOld == false)
            //    {
            if (rdbtnlst.SelectedItem.Text.Trim().ToUpper() == "LEAVE")
            {
                #region NewReport

                string[] fromdatespiltNEw = txtfrom.Text.ToString().Trim().Split('/');
                DateTime fromdateNEw = Convert.ToDateTime(fromdatespiltNEw[1] + '/' + fromdatespiltNEw[0] + '/' + fromdatespiltNEw[2]);
                string[] todatespiltNEw = txtto.Text.ToString().Trim().Split('/');
                DateTime todateNEw = Convert.ToDateTime(todatespiltNEw[1] + '/' + todatespiltNEw[0] + '/' + todatespiltNEw[2]);
                string staff = "";
                for (int i = 0; i < chklsstaff.Items.Count; i++)
                {
                    if (chklsstaff.Items[i].Selected == true)
                    {
                        if (staff == "")
                        {
                            staff = "'" + chklsstaff.Items[i].Value + "'";
                        }
                        else
                        {
                            staff = staff + ',' + "'" + chklsstaff.Items[i].Value + "'";
                        }

                    }
                }
                string Leave = "";
                for (int i = 0; i < chklsleave.Items.Count; i++)
                {
                    if (chklsleave.Items[i].Selected == true)
                    {
                        if (Leave == "")
                        {
                            Leave = "'" + chklsleave.Items[i].Value + "'";
                        }
                        else
                        {
                            Leave = Leave + ',' + "'" + chklsleave.Items[i].Value + "'";
                        }

                    }
                }
                if (strisstaff.ToLower().Trim() != "")
                {
                    staff = "'" + strisstaff + "'";
                }

                string SelectQuery = "select RequestCode,RequestDate,leaveFrom,LeaveTo,ishalfday,convert(varchar(10),leaveFrom,103) as FromL,convert(varchar(10),LeaveTo,103) as ToL,convert(varchar(10),Halfdate,103) as Halfd,case when leavesession=0 then 'Full' when leavesession=1 then 'Morning' when leavesession=2 then 'Evening' end as leavesession,Halfdate,ReqstaffAppNo,(select Category from leave_category where LeaveMasterPk= LeaveMasterFK) as LeaveMasterFK,(select MasterValue from CO_MasterValues where Mastercode=isnull(GateReqReason,0)) as GateReqReason,leaveChangeReason,HodAlterInchargeAppno,sm.staff_code,ltrim(RTRIM(sm.staff_name))as staff_name,(Datediff(dd,leaveFrom,leaveTo)+1)as Total,dt.dept_Name,dt.dept_code from RQ_Requisition r,staff_appl_Master sa,staffmaster sm,stafftrans t,hrDept_Master dt where dt.dept_code=t.dept_code and sa.appl_id=r.ReqstaffAppNo and sa.appl_no=sm.appl_no and t.staff_code=sm.staff_code and t.latestrec='1' and RequestType=5 and memtype=2 and ReqAppStatus='1' and (leaveFrom between '" + fromdateNEw.ToString("MM/dd/yyyy") + "' and '" + todateNEw.ToString("MM/dd/yyyy") + "' or leaveTo between '" + fromdateNEw.ToString("MM/dd/yyyy") + "' and '" + todateNEw.ToString("MM/dd/yyyy") + "') and sm.staff_code in (" + staff + ") and LeaveMasterFK in (" + Leave + ") ";
                if (txtvalue.Text.Trim() != "")
                {
                    if (ddlleavevalue.SelectedIndex == 0)
                    {
                        SelectQuery += " and (Datediff(dd,leaveFrom,leaveTo)+1) >" + txtvalue.Text.Trim() + "";
                    }
                    else if (ddlleavevalue.SelectedIndex == 1)
                    {
                        SelectQuery += " and (Datediff(dd,leaveFrom,leaveTo)+1) <" + txtvalue.Text.Trim() + "";
                    }
                    else if (ddlleavevalue.SelectedIndex == 2)
                    {
                        SelectQuery += " and (Datediff(dd,leaveFrom,leaveTo)+1) =" + txtvalue.Text.Trim() + "";
                    }
                }
                // SelectQuery += " order by LeaveFrom asc,sm.staff_code,dt.dept_code";
                SelectQuery += " order by sm.staff_name,sm.staff_code,dt.dept_code";
                ds.Reset();
                ds = d2.select_method(SelectQuery, hat, "Text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    if (old_newRecord == true || oldNew == true)
                    {
                        LoadHeader();
                    }
                    DataView dvnew = new DataView();
                    DataTable dtStaff = ds.Tables[0].DefaultView.ToTable(true, "staff_code");
                    for (int intfor = 0; intfor < dtStaff.Rows.Count; intfor++)
                    {
                        ds.Tables[0].DefaultView.RowFilter = "staff_code='" + Convert.ToString(dtStaff.Rows[intfor]["staff_code"]) + "'";
                        dvnew = ds.Tables[0].DefaultView;
                        if (dvnew.Count > 0)
                        {
                            double CumTotal = 0;
                            for (int intdiv = 0; intdiv < dvnew.Count; intdiv++)
                            {
                                FpstaffLeave.Sheets[0].RowCount++;
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(intfor + 1);
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dvnew[intdiv]["staff_code"].ToString();
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dvnew[intdiv]["Staff_Name"].ToString();
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = dvnew[intdiv]["dept_name"].ToString();
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = dvnew[intdiv]["LeaveMasterFK"].ToString();
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = dvnew[intdiv]["FromL"].ToString();
                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = dvnew[intdiv]["ToL"].ToString();
                                //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                                double Total = 0;
                                string Tot = dvnew[intdiv]["Total"].ToString();
                                string Half = dvnew[intdiv]["leavesession"].ToString();
                                if (Half.Trim() != "Full")
                                {
                                    double.TryParse(Tot, out Total);
                                    Total = Total - 0.5;
                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(Total) + "  " + dvnew[intdiv]["leavesession"].ToString();
                                }
                                else
                                {
                                    double.TryParse(Tot, out Total);
                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = dvnew[intdiv]["Total"].ToString();
                                }
                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = dvnew[intdiv]["GateReqReason"].ToString();
                                CumTotal += Total;
                            }

                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - dvnew.Count, 9].Text = CumTotal.ToString();
                            FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - dvnew.Count, 9].HorizontalAlign = HorizontalAlign.Center;
                            FpstaffLeave.Sheets[0].SpanModel.Add(FpstaffLeave.Sheets[0].RowCount - dvnew.Count, 9, dvnew.Count, 1);

                        }
                    }
                    int rowcount = FpstaffLeave.Sheets[0].RowCount;
                    FpstaffLeave.Height = 300;
                    FpstaffLeave.Sheets[0].PageSize = 25 + (rowcount * 20);
                    FpstaffLeave.SaveChanges();
                    errmsg.Visible = false;
                }
                //else commented by delsi 30/04/2018
                //{
                //    FpstaffLeave.Visible = false;
                //    btnprintmaster.Visible = false;
                //    btnxl.Visible = false;
                //    lblrptname.Visible = false;
                //    txtrptname.Visible = false;
                //    errmsg.Visible = true;
                //    errmsg.Text = "No Records Found";
                //}
                #endregion
                // }
            }
        }
        catch (Exception ex)
        {
            FpstaffLeave.Visible = false;
            btnprintmaster.Visible = false;
            btnxl.Visible = false;
            lblrptname.Visible = false;
            txtrptname.Visible = false;
            errmsg.Visible = true;
            errmsg.Text = "No Records Found";
        }


    }

    public void LoadHeader()
    {
        FpstaffLeave.Visible = true;
        btnprintmaster.Visible = true;
        FpstaffLeave.Sheets[0].ColumnHeader.RowCount = 1;
        FpstaffLeave.Sheets[0].ColumnCount = 10;

        FpstaffLeave.Sheets[0].RowCount = 0;


        FpstaffLeave.Sheets[0].ColumnHeader.Columns[0].Width = 30;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[1].Width = 50;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[2].Width = 100;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[3].Width = 120;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[4].Width = 100;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[5].Width = 50;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[6].Width = 50;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[7].Width = 50;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[8].Width = 50;


        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";

        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Leave Type";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 5].Text = "From Date";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 6].Text = "To Date";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Days";//Reason
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Reason";//Days
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Total No Of Days";

        FpstaffLeave.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;

        FpstaffLeave.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpstaffLeave.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpstaffLeave.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpstaffLeave.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);

    }

    //Added by Manikandan---------------------------------------
    public void LoadHeader_Absent()
    {
        //==================Hided by Manikandan 22/05/2013
        FpstaffLeave.Visible = true;
        btnprintmaster.Visible = true;
        ////FpstaffLeave.Sheets[0].ColumnHeader.RowCount = 4;
        FpstaffLeave.Sheets[0].ColumnCount = 4;
        //mOdified by srinath 28/4/2014
        FpstaffLeave.Sheets[0].RowCount = 0;
        //MyImg mi = new MyImg();
        //mi.ImageUrl = "../images/10BIT001.jpeg";
        //mi.ImageUrl = "Handler/Handler2.ashx?";
        //MyImg mi2 = new MyImg();
        //mi2.ImageUrl = "../images/10BIT001.jpeg";
        //mi2.ImageUrl = "Handler/Handler5.ashx?";

        FpstaffLeave.Sheets[0].ColumnHeader.Columns[0].Width = 30;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[1].Width = 50;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[2].Width = 100;
        FpstaffLeave.Sheets[0].ColumnHeader.Columns[3].Width = 50;

        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorRight = Color.White;        

        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 2, 1);

        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].CellType = mi2;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 2, 1);

        string collegename = "";
        string Address = "";
        string strcollegeinfo = "select collname,address1,address2,address3,pincode from collinfo where college_code=" + collegecode + "";
        DataSet dscoll = d2.select_method(strcollegeinfo, hat, "Text");
        if (dscoll.Tables[0].Rows.Count > 0)
        {
            collegename = dscoll.Tables[0].Rows[0]["collname"].ToString();
            Address = dscoll.Tables[0].Rows[0]["address1"].ToString() + ',' + dscoll.Tables[0].Rows[0]["address2"].ToString() + ',' + dscoll.Tables[0].Rows[0]["address3"].ToString() + '-' + dscoll.Tables[0].Rows[0]["pincode"].ToString();
        }
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 1].Text = collegename;
        ////FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 5);
        //FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 2);

        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[1, 1].Text = Address;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[1].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[1].Font.Bold = true;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[1].Font.Size = FontUnit.Medium;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[1].Font.Name = "Book Antiqua";

        //FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 2);

        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        //FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";

        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[2, 0].Text = "Staff Absent Details";
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[2, 0].HorizontalAlign = HorizontalAlign.Center;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;
        //FpstaffLeave.Sheets[0].ColumnHeader.Cells[2, 0].Font.Name = "Book Antiqua";
        //FpstaffLeave.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 3);
        //==========================
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Medium;
        FpstaffLeave.Sheets[0].ColumnHeader.Rows[0].Font.Name = "Book Antiqua";

        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Name";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Department";
        FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total Days";

        //mOdified by srinath 28/4/2014
        FpstaffLeave.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
        FpstaffLeave.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
        FpstaffLeave.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
        if (chkdatewise.Checked == true)
        {
            FpstaffLeave.Sheets[0].ColumnCount = FpstaffLeave.Sheets[0].ColumnCount + 2;
            FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Date";
            FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Days";
            FpstaffLeave.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Total Days";
            FpstaffLeave.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpstaffLeave.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            FpstaffLeave.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;
        }

        FpstaffLeave.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
        FpstaffLeave.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
    }

    //------------------------------------

    public void loaddetails()
    {
        try
        {
            btnxl.Visible = true;
            txtrptname.Visible = true;
            lblrptname.Visible = true;
            FpstaffLeave.Visible = true;
            btnprintmaster.Visible = true;
            errmsg.Visible = false;

            string Staffcode = "" + Session["Staff_Code"].ToString();
            if (Staffcode.ToString().Trim() != "")
            {
                string[] splitdatefrom = txtfrom.Text.ToString().Trim().Split('/');
                string fromdate1 = splitdatefrom[1] + '/' + splitdatefrom[0] + '/' + splitdatefrom[2];
                string[] splitdateto = txtto.Text.ToString().Trim().Split('/');
                string todate1 = splitdateto[1] + '/' + splitdateto[0] + '/' + splitdateto[2];

                string leavetype = "";
                //for (int i = 0; i < chklsleave.Items.Count; i++)
                //{
                //    if (chklsleave.Items[i].Selected == true)
                //    {
                //        if (leavetype == "")
                //        {
                //            leavetype = "sl.lt_taken='" + chklsleave.Items[i].Value + "'";
                //            leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Value + "'";
                //            leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Value + "'";
                //        }
                //        else
                //        {
                //            leavetype = "" + leavetype + " or " + "sl.lt_taken='" + chklsleave.Items[i].Value + "'";
                //            leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Value + "'";
                //            leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Value + "'";
                //        }

                //    }
                //}
                //if (leavetype != "")
                //{
                //    leavetype = "and (" + leavetype + ")";
                //}


                for (int i = 0; i < chklsleave.Items.Count; i++)
                {
                    if (chklsleave.Items[i].Selected == true)
                    {
                        if (leavetype == "")
                        {
                            leavetype = "sl.lt_taken='" + chklsleave.Items[i].Text + "'";
                            leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Text + "'";
                            leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Text + "'";
                        }
                        else
                        {
                            leavetype = "" + leavetype + " or " + "sl.lt_taken='" + chklsleave.Items[i].Text + "'";
                            leavetype = leavetype + " or sl.lt_taken='HalfDay@fh@" + chklsleave.Items[i].Text + "'";
                            leavetype = leavetype + " or sl.lt_taken='HalfDay@sh@" + chklsleave.Items[i].Text + "'";
                        }

                    }
                }
                if (leavetype != "")
                {
                    leavetype = "and (" + leavetype + ")";
                }

                string instaffleave = "";

                if (txtto.Text.ToString().Trim() != "" && txtfrom.Text.ToString().Trim() != "")
                {
                    //instaffleave = "select distinct sm.Staff_name,hm.dept_name,sl.lt_taken,sl.fdate,sl.tdate,sl.Half_Days,sl.remarks from Staff_leave_details sl,staffmaster sm,stafftrans st,hrdept_master hm where sl.staff_code=sm.staff_code and sm.staff_code=st.staff_code and st.dept_code=hm.dept_code and st.latestrec=1 and st.staff_code in('" + Staffcode + "') and ((fdate between '" + fromdate1 + "' and '" + todate1 + "') or (tdate between '" + fromdate1 + "' and '" + todate1 + "')) " + leavetype + " order by sm.Staff_name,hm.dept_name,sl.fdate";

                    instaffleave = "select distinct sm.staff_code,sm.Staff_name,hm.dept_name,sl.lt_taken,sl.fdate,sl.tdate,sl.Half_Days,sl.remarks from Staff_leave_details sl,staffmaster sm,stafftrans st,hrdept_master hm where sl.college_code=sm.college_code and sm.college_code=hm.college_code and sl.staff_code=sm.staff_code and sm.staff_code=st.staff_code and apply_approve=1 and st.dept_code=hm.dept_code and sm.settled=0 and sm.resign=0 and ISNULL(sm.Discontinue,'0')='0' and st.latestrec=1 and sm.college_code='" + Convert.ToString(Session["collegecode"]) + "' and st.staff_code in('" + Staffcode.ToString().Trim() + "') and ((fdate between '" + fromdate1 + "' and '" + todate1 + "') or (tdate between '" + fromdate1 + "' and '" + todate1 + "')) " + leavetype + " order by sm.Staff_name,hm.dept_name,sl.fdate";

                }
                else
                {
                    instaffleave = "select distinct sm.Staff_name,hm.dept_name,sl.lt_taken,sl.fdate,sl.tdate,sl.Half_Days,sl.remarks from Staff_leave_details sl,staffmaster sm,stafftrans st,hrdept_master hm where sl.staff_code=sm.staff_code and sm.staff_code=st.staff_code and st.dept_code=hm.dept_code and st.latestrec=1 and st.staff_code in('" + Staffcode + "') " + leavetype + " order by sm.Staff_name,hm.dept_name,sl.fdate";
                }
                ds.Reset();
                ds.Dispose();
                ds = d2.select_method(instaffleave, hat, "Text");
                if (ds.Tables[0].Rows.Count == 0)
                {
                    oldNew = true;

                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    LoadHeader();
                    double overallcount = 0;
                    FpstaffLeave.Sheets[0].Columns[8].Visible = false;
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        string[] fromdateset = ds.Tables[0].Rows[row]["fdate"].ToString().Split(' ');
                        string[] fromdateset1 = fromdateset[0].Split('/');
                        string fromdateset2 = Convert.ToString(fromdateset1[1] + '/' + fromdateset1[0] + '/' + fromdateset1[2]);
                        string[] todateset = ds.Tables[0].Rows[row]["tdate"].ToString().Split(' ');
                        string[] tomdateset1 = todateset[0].Split('/');
                        string todateset2 = Convert.ToString(tomdateset1[1] + '/' + tomdateset1[0] + '/' + tomdateset1[2]);
                        DateTime dtfrom = Convert.ToDateTime(fromdateset[0]);
                        DateTime dtto = Convert.ToDateTime(todateset[0]);
                        DateTime reporttodate = Convert.ToDateTime(todate1);
                        DateTime reportfromdate = Convert.ToDateTime(fromdate1);
                        DateTime difftodate;

                        if (dtto > reporttodate)
                        {
                            difftodate = reporttodate;
                        }
                        else
                        {
                            difftodate = dtto;
                        }

                        DateTime difffromdate;
                        if (dtfrom < reportfromdate)
                        {
                            difffromdate = reportfromdate;
                        }
                        else
                        {
                            difffromdate = dtfrom;
                        }
                        TimeSpan t = difftodate - difffromdate;
                        double NrOfDays = t.TotalDays;
                        NrOfDays++;

                        int haldaycount = 0;
                        string hallfday = ds.Tables[0].Rows[row]["Half_Days"].ToString();
                        if (hallfday.ToString().Trim() != "" && hallfday.ToString().Trim() != "0")
                        {
                            haldaycount = Convert.ToInt32(hallfday);
                            NrOfDays = NrOfDays - (Convert.ToDouble(haldaycount * 0.5));

                        }


                        string LeaveTake = ds.Tables[0].Rows[row]["lt_taken"].ToString();
                        string[] spitLeavetake = LeaveTake.Split('@');
                        if (spitLeavetake.GetUpperBound(0) == 2)
                        {
                            LeaveTake = spitLeavetake[2].ToString();
                        }
                        overallcount = overallcount + NrOfDays;
                        FpstaffLeave.Sheets[0].RowCount++;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[row]["Staff_code"].ToString();
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[row]["Staff_Name"].ToString();
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[row]["dept_name"].ToString();
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = LeaveTake;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = fromdateset2;
                        //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = txtfrom.Text.Trim();//This line modified by Manikandan from above commented line
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].Text = todateset2;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].Text = ds.Tables[0].Rows[row]["remarks"].ToString();
                        //FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = txtto.Text.Trim();//This line modified by Manikandan from above commented line
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].Text = NrOfDays.ToString();




                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                        //    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                    }
                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - ds.Tables[0].Rows.Count, 9].Text = overallcount.ToString();//delsi11/05/2018
                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - ds.Tables[0].Rows.Count, 9].HorizontalAlign = HorizontalAlign.Center;
                    // FpstaffLeave.Sheets[0].SpanModel.Add(FpstaffLeave.Sheets[0].RowCount - dvnew.Count, 9, dvnew.Count, 1);

                    FpstaffLeave.Sheets[0].SpanModel.Add(FpstaffLeave.Sheets[0].RowCount - ds.Tables[0].Rows.Count, 9, ds.Tables[0].Rows.Count, 1);


                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "No Record Found";
                    FpstaffLeave.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    txtrptname.Visible = false;
                    lblrptname.Visible = false;
                }
                int rowcount = FpstaffLeave.Sheets[0].RowCount;
                FpstaffLeave.Height = 300;
                FpstaffLeave.Sheets[0].PageSize = 25 + (rowcount * 20);
                FpstaffLeave.SaveChanges();

            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reportname = txtrptname.Text.ToString().Trim();
        if (reportname != "")
        {
            d2.printexcelreport(FpstaffLeave, reportname);
        }
        //senthil(24.06.15)
        else
        {
            Label2.Visible = true;
            Label2.Text = "Please Enter Your Report Name";
        }
    }
    protected void rdbtnlst_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdbtnlst.Items[0].Selected == true)
        {
            //lblleave.Visible = true;
            txtleave.Enabled = true;
            chklsleave.Enabled = true;
            txtvalue.Enabled = true;
            //added by srinath 28/4/2014
            chkdatewise.Enabled = false;
            chkdatewise.Checked = false;
            ddlleavevalue.Enabled = true;
        }
        else if (rdbtnlst.Items[1].Selected == true)
        {
            //added by srinath 28/4/2014
            chkdatewise.Enabled = true;
            chkdatewise.Checked = false;
            txtleave.Enabled = false;
            chklsleave.Enabled = false;
            txtvalue.Enabled = true;
            ddlleavevalue.Enabled = true;
        }
    }

    void staffAbsent(string strrights)
    {

        try
        {
            string tempstaffcode = "";
            int strrow = 0;
            if (strisstaff.ToLower().Trim() == "" || strrights.Trim() == "1")
            {
                errmsg.Visible = false;
                btnxl.Visible = true;
                lblrptname.Visible = true;
                txtrptname.Visible = true;

                string[] fromdatespilt = txtfrom.Text.ToString().Trim().Split('/');
                DateTime fromdate = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
                string[] todatespilt = txtto.Text.ToString().Trim().Split('/');
                DateTime todate = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
                if (fromdate > todate)
                {
                    errmsg.Text = "Please Enter From Date Less Than To Date";
                    errmsg.Visible = true;
                }
                else
                {
                    string leavestaffacode = "";
                    for (int i = 0; i < chklsstaff.Items.Count; i++)
                    {
                        if (chklsstaff.Items[i].Selected == true)
                        {
                            if (leavestaffacode == "")
                            {
                                leavestaffacode = "'" + chklsstaff.Items[i].Value + "'";
                            }
                            else
                            {
                                leavestaffacode = leavestaffacode + ',' + "'" + chklsstaff.Items[i].Value + "'";
                            }

                        }
                    }
                    if (leavestaffacode != "")
                    {
                        string[] splitdatefrom = txtfrom.Text.ToString().Trim().Split('/');
                        DateTime mon_year = Convert.ToDateTime(fromdatespilt[1] + '/' + fromdatespilt[0] + '/' + fromdatespilt[2]);
                        string fromdate1 = splitdatefrom[1] + '/' + splitdatefrom[2]; //+'/' + splitdatefrom[2];
                        string[] splitdateto = txtto.Text.ToString().Trim().Split('/');
                        DateTime mon_year1 = Convert.ToDateTime(todatespilt[1] + '/' + todatespilt[0] + '/' + todatespilt[2]);
                        string todate1 = splitdateto[1] + '/' + splitdateto[2]; // +'/' + splitdateto[2];

                        //DateTime mon_year = Convert.ToDateTime(txtfrom.Text.ToString());
                        //DateTime mon_year1 = Convert.ToDateTime(txtto.Text.ToString());

                        string get_monyear = "";

                        for (DateTime caldate = mon_year; caldate <= mon_year1; caldate = caldate.AddMonths(1))
                        {

                            string staf_leavemonyr = caldate.ToString("M/d/yyyy");
                            string[] split_monyr = staf_leavemonyr.Split('/');
                            string splited_mon = split_monyr[0]; // + '/' + split_monyr[2];

                            string yr_time = split_monyr[2];
                            string[] split_yrnly = yr_time.Split(' ');
                            string splited_yr = split_yrnly[0];
                            string mon_yr_value = splited_mon + '/' + splited_yr;

                            if (get_monyear == "")
                            {
                                get_monyear = "'" + mon_yr_value + "'";
                            }
                            else
                            {
                                get_monyear = get_monyear + "," + "'" + mon_yr_value + "'";
                            }
                            Session["caldate"] = caldate;
                        }

                        string strqueery = "select distinct s.staff_code,staff_name,dept_name from staffmaster s,stafftrans t,hrdept_master d,desig_master g where s.staff_code = t.staff_code and t.dept_code = d.dept_code and t.desig_code = g.desig_code and resign = 0 and settled = 0 and latestrec = 1 and t.staff_code in(" + leavestaffacode + ") order by s.staff_code";
                        DataTable dt_abs = d2.select_method_wop_table(strqueery, "text");

                        string strstaffanttd = "select * from staff_attnd where staff_code in(" + leavestaffacode + ") and mon_year in(" + get_monyear + ") order by mon_year, staff_code";
                        DataTable dt_abs1 = d2.select_method_wop_table(strstaffanttd, "text");//delsi

                        if (dt_abs.Rows.Count > 0)
                        {
                            LoadHeader_Absent();
                            //start========added by Manikandan 27/10/2013====
                            string[] str_date_from = txtfrom.Text.Split(new char[] { '/' });
                            string[] str_date_to = txtto.Text.Split(new char[] { '/' });

                            DateTime dt1 = Convert.ToDateTime(str_date_from[1] + "/" + str_date_from[0] + "/" + str_date_from[2]);
                            DateTime dt2 = Convert.ToDateTime(str_date_to[1] + "/" + str_date_to[0] + "/" + str_date_to[2]);
                            //=================End===========================
                            for (int cnt_staff = 0; cnt_staff < dt_abs.Rows.Count; cnt_staff++)
                            {
                                double abs_count = 0;
                                double tot_abs = 0;

                                DataView dv_staff = new DataView();
                                string staff_code = dt_abs.Rows[cnt_staff]["staff_code"].ToString();
                                //string str_abs = dt_abs1.Rows[0][cnt_staff].ToString();                                

                                if (dt_abs1.Rows.Count > 0)
                                {

                                    int temp_1 = -1;
                                    for (DateTime caldate = mon_year; caldate <= mon_year1; caldate = caldate.AddMonths(1))
                                    {
                                        temp_1++;
                                        string staf_leavemonyr = caldate.ToString("M/d/yyyy");
                                        string[] split_monyr = staf_leavemonyr.Split('/');

                                        string splited_date = split_monyr[1]; // + '/' + split_monyr[2];
                                        string splited_mon = split_monyr[0]; // + '/' + split_monyr[2];
                                        string yr_time = split_monyr[2];

                                        string[] split_yrnly = yr_time.Split(' ');
                                        string splited_yr = split_yrnly[0];

                                        int num_spltdate = Convert.ToInt32(splited_date);
                                        string mon_yr_value = splited_mon + '/' + splited_yr;

                                        dt_abs1.DefaultView.RowFilter = "staff_code='" + staff_code + "' and mon_year='" + mon_yr_value + "'";
                                        dv_staff = dt_abs1.DefaultView;

                                        if (caldate.Month == mon_year1.Month)
                                        {
                                            if (dt1.Month != dt2.Month)
                                            {
                                                //for (int i = 4; i < mon_year1.Day + 4; i++)
                                                for (int i = 4; i <= mon_year1.Day + 4; i++)
                                                {
                                                    string att = "";
                                                    if (dv_staff.Count > 0)
                                                    {
                                                        att = dv_staff[0][i].ToString();
                                                    }
                                                    if (att != "")
                                                    {
                                                        if (att.Length > 1)
                                                        {
                                                            string[] split_str_abs = att.Split('-');
                                                            if (split_str_abs.GetUpperBound(0) >= 1)//Condition Added by Manikandan 22/08/2013
                                                            {
                                                                string FN = split_str_abs[0];
                                                                string AN = split_str_abs[1];
                                                                Double le = 0;
                                                                if (FN == "A")
                                                                {
                                                                    abs_count++;
                                                                    le = le + 0.5;
                                                                }
                                                                if (AN == "A")
                                                                {
                                                                    abs_count++;
                                                                    le = le + 0.5;
                                                                }
                                                                //modified by srinath 28/4/2014
                                                                if (chkdatewise.Checked == true && ddlleavevalue.Enabled == false && le > 0)
                                                                {
                                                                    FpstaffLeave.Sheets[0].RowCount++;
                                                                    if (tempstaffcode != staff_code)
                                                                    {
                                                                        tempstaffcode = staff_code;
                                                                        strrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                                    }
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                                    string datev = (i - 3).ToString() + '/' + mon_yr_value;
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = datev.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = le.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //Start========Added by Manikandan on 27/10/2013s========
                                            else
                                            {
                                                for (int i = num_spltdate + 3; i < mon_year1.Day + 4; i++)
                                                {
                                                    string att = "";
                                                    if (dv_staff.Count > 0)
                                                    {
                                                        att = dv_staff[0][i].ToString();
                                                    }
                                                    if (att != "")
                                                    {
                                                        if (att.Length > 1)
                                                        {
                                                            string[] split_str_abs = att.Split('-');
                                                            if (split_str_abs.GetUpperBound(0) >= 1)//Condition Added by Manikandan 22/08/2013
                                                            {
                                                                Double le = 0;
                                                                string FN = split_str_abs[0];
                                                                string AN = split_str_abs[1];
                                                                if (FN == "A")
                                                                {
                                                                    abs_count++;
                                                                    le = le + 0.5;
                                                                }
                                                                if (AN == "A")
                                                                {
                                                                    abs_count++;
                                                                    le = le + 0.5;
                                                                }
                                                                //modified by srinath 28/4/2014
                                                                if (chkdatewise.Checked == true && ddlleavevalue.Enabled == false && le > 0)
                                                                {
                                                                    FpstaffLeave.Sheets[0].RowCount++;
                                                                    if (tempstaffcode != staff_code)
                                                                    {
                                                                        tempstaffcode = staff_code;
                                                                        strrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                                    }
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                                    string datev = (i - 3).ToString() + '/' + mon_yr_value;
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = datev.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = le.ToString();
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                                    FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 5].Text = tot_abs.ToString();
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //=========End================
                                        }
                                        else
                                        {
                                            //for (int i = num_spltdate + 4; i < dt_abs1.Columns.Count; i++)
                                            for (int i = num_spltdate + 3; i < dt_abs1.Columns.Count; i++)
                                            {

                                                string att = "";
                                                if (dv_staff.Count > 0)
                                                {
                                                    att = dv_staff[0][i].ToString();
                                                }

                                                if (att != "")
                                                {
                                                    if (att.Length > 1 && att != null)
                                                    {
                                                        string[] split_str_abs = att.Split('-');
                                                        if (split_str_abs.GetUpperBound(0) >= 1)//Condition Added by Manikandan 22/08/2013
                                                        {
                                                            string FN = split_str_abs[0];
                                                            string AN = split_str_abs[1];
                                                            Double le = 0;
                                                            if (FN == "A")
                                                            {
                                                                abs_count++;
                                                                le = le + 0.5;
                                                            }
                                                            if (AN == "A")
                                                            {
                                                                abs_count++;
                                                                le = le + 0.5;
                                                            }
                                                            //modified by srinath 28/4/2014
                                                            if (chkdatewise.Checked == true && ddlleavevalue.Enabled == false && le > 0)
                                                            {
                                                                FpstaffLeave.Sheets[0].RowCount++;
                                                                if (tempstaffcode != staff_code)
                                                                {
                                                                    tempstaffcode = staff_code;
                                                                    strrow = FpstaffLeave.Sheets[0].RowCount - 1;
                                                                }
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                                string datev = (i - 3).ToString() + '/' + mon_yr_value;
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = datev.ToString();
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].Text = le.ToString();
                                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            //}
                                        }
                                        //}                                        
                                    }
                                    //-----------------------------------------------------------------------
                                    //-----------------------------------------------------------------------
                                    if (chkdatewise.Checked == true)
                                    {
                                        if (abs_count != 0)
                                        {
                                            tot_abs = Convert.ToDouble(abs_count * 0.5);
                                            int noofrow = FpstaffLeave.Sheets[0].RowCount - strrow;
                                            FpstaffLeave.Sheets[0].Cells[strrow, 5].Text = tot_abs.ToString();
                                            if (noofrow == 0)
                                            {
                                                noofrow = 1;
                                            }
                                            FpstaffLeave.Sheets[0].AddSpanCell(strrow, 5, noofrow, 1);
                                        }
                                        abs_count = 0;
                                    }
                                    if (txtvalue.Text != "" && abs_count != 0)
                                    {
                                        tot_abs = Convert.ToDouble(abs_count * 0.5);

                                        int days = Convert.ToInt32(txtvalue.Text);
                                        if (ddlleavevalue.SelectedValue.ToString().Trim() == "2")
                                        {
                                            if (days == tot_abs)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = tot_abs.ToString();

                                            }
                                        }
                                        else if (ddlleavevalue.SelectedValue.ToString().Trim() == "1")
                                        {
                                            if (days > tot_abs)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = tot_abs.ToString();
                                            }
                                        }
                                        else
                                        {
                                            if (days < tot_abs)
                                            {
                                                FpstaffLeave.Sheets[0].RowCount++;
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                                FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = tot_abs.ToString();
                                            }
                                        }
                                    }

                                    //--------------------------------------------------------------------------------
                                    //--------------------------------------------------------------------------------
                                    if (abs_count != 0 && txtvalue.Text == "")
                                    {

                                        tot_abs = Convert.ToDouble(abs_count * 0.5);

                                        FpstaffLeave.Sheets[0].RowCount++;
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 0].Text = FpstaffLeave.Sheets[0].RowCount.ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 1].Text = dt_abs.Rows[cnt_staff][1].ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 2].Text = dt_abs.Rows[cnt_staff][2].ToString();
                                        FpstaffLeave.Sheets[0].Cells[FpstaffLeave.Sheets[0].RowCount - 1, 3].Text = tot_abs.ToString();
                                        FpstaffLeave.Visible = true;
                                        btnprintmaster.Visible = true;
                                    }

                                }

                            }
                        }

                        else
                        {
                            FpstaffLeave.Visible = false;
                            btnprintmaster.Visible = false;
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtrptname.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";

                        }
                        int rowcount = FpstaffLeave.Sheets[0].RowCount;
                        FpstaffLeave.Height = 300;
                        FpstaffLeave.Sheets[0].PageSize = 25 + (rowcount * 20);
                        FpstaffLeave.SaveChanges();

                        if (FpstaffLeave.Sheets[0].RowCount == 0)
                        {
                            FpstaffLeave.Visible = false;
                            btnprintmaster.Visible = false;
                            btnxl.Visible = false;
                            lblrptname.Visible = false;
                            txtrptname.Visible = false;
                            errmsg.Visible = true;
                            errmsg.Text = "No Records Found";
                        }
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Select Staff";
                    }
                }
            }

            else
            {
                loaddetails();
            }
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;

        Session["column_header_row_count"] = FpstaffLeave.Sheets[0].ColumnHeader.RowCount;

        degreedetails = "Staff Leave Report @Date: " + txtfrom.Text.ToString() + " To " + txtto.Text.ToString();
        string pagename = "StudentTestReport.aspx";

        Printcontrol.loadspreaddetails(FpstaffLeave, pagename, degreedetails);
        Printcontrol.Visible = true;

    }
    protected void chkdate_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            txtvalue.Text = "";
            if (chkdatewise.Checked == true)
            {
                ddlleavevalue.Enabled = false;
                txtvalue.Enabled = false;
            }
            else
            {
                ddlleavevalue.Enabled = true;
                txtvalue.Enabled = true;

            }
        }
        catch
        {
        }
    }

}