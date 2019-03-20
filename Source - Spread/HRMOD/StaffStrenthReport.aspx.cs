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
public partial class StaffStrenthReport : System.Web.UI.Page
{
    public SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["LocalConn"].ToString());
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
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    //Added By Srinath 1/4/2013
    string collegecode = "";
    string usercode = "";
    string singleuser = "";
    string group_user = "";
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
            if (!Page.IsPostBack)
            {
                //Fpstaff.Width = 1000;
                Fpstaff.Sheets[0].AutoPostBack = true;
                Fpstaff.CommandBar.Visible = true;
                Fpstaff.Sheets[0].SheetName = " ";
                Fpstaff.Sheets[0].SheetCorner.Columns[0].Visible = false;
                Fpstaff.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                Fpstaff.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                Fpstaff.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                Fpstaff.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].DefaultStyle.Font.Bold = false;
                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpstaff.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpstaff.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpstaff.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].AllowTableCorner = true;
                //---------------page number
                Fpstaff.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                Fpstaff.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                Fpstaff.Pager.Align = HorizontalAlign.Right;
                Fpstaff.Pager.Font.Bold = true;
                Fpstaff.Pager.Font.Name = "Book Antiqua";
                Fpstaff.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                Fpstaff.Pager.BackColor = System.Drawing.Color.Beige;
                Fpstaff.Pager.BackColor = System.Drawing.Color.AliceBlue;
                Fpstaff.Pager.PageCount = 100;
                Fpstaff.Visible = false;
                btnprintmaster.Visible = false;
                btnxl.Visible = false;
                txtrptname.Visible = false;
                lblrptname.Visible = false;
                BindDesignation();
                BindDepartment();
                BindCategory();
                BindType();
                BindDesignation();
            }
            errmsg.Visible = false;
        }
        catch (Exception ex) { }
    }
    //Load Designation
    public void BindDesignation()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            string strdesigquery = "select distinct desig_name,desig_code from desig_master where  collegeCode=" + Session["collegecode"].ToString() + "";
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
    public void BindDepartment()
    {
        try
        {
            ds.Dispose();
            ds.Reset();
            //Added By Srinath 1/4/2013
            // ds = d2.loaddepartment(Session["collegecode"].ToString());
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
    //Load Category
    public void BindCategory()
    {
        try
        {
            ds.Clear();
            string strcategory = "select  distinct category_code,category_name from staffcategorizer where college_code='" + Session["collegecode"].ToString() + "'";
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
            string strtype = "select distinct stftype from stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and college_code='" + Session["collegecode"].ToString() + "'";
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
    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            Fpstaff.Visible = true;
            btnprintmaster.Visible = true;
            btnxl.Visible = true;
            lblrptname.Visible = true;
            txtrptname.Visible = true;
            errmsg.Visible = false;
            string deptcode = "";
            string mdeptcode = "";
            string fdeptcode = "";
            string tdeptcode = "";
            LoadHeader();
            string designcode = "";
            string mdesigncode = "";
            string fdesigncode = "";
            string tdesigncode = "";
            for (int item = 0; item < chklsdesign.Items.Count; item++)
            {
                if (chklsdesign.Items[item].Selected == true)
                {
                    if (designcode == "")
                    {
                        designcode = chklsdesign.Items[item].Value;
                    }
                    else
                    {
                        designcode = designcode + ',' + chklsdesign.Items[item].Value;
                    }
                }
            }
            if (designcode != "")
            {
                mdesigncode = "and st.desig_code in(" + designcode + ")";
                fdesigncode = "and sta.desig_code in(" + designcode + ")";
                tdesigncode = "and stt.desig_code in(" + designcode + ")";
                designcode = "and stc.desig_code in(" + designcode + ")";
            }
            string catecode = "";
            string mcatecode = "";
            string fcatecode = "";
            string tcatecode = "";
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
                mcatecode = " and st.category_code in(" + catecode + ")";
                fcatecode = " and sta.category_code in(" + catecode + ")";
                tcatecode = " and stt.category_code in(" + catecode + ")";
                catecode = " and stc.category_code in(" + catecode + ")";
            }
            string type = "";
            string mtype = "";
            string ftype = "";
            string ttype = "";
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
                mtype = " and st.stftype in(" + type + ")";
                ftype = " and sta.stftype in(" + type + ")";
                ttype = " and stt.stftype in(" + type + ")";
                type = " and stc.stftype in(" + type + ")";
            }
            int srno = 0;
            Fpstaff.Sheets[0].RowCount = 0;
            if (rdobtn_CategoryWise.Items[0].Selected == true)
            {
                for (int item1 = 0; item1 < chklsdept.Items.Count; item1++)
                {
                    if (chklsdept.Items[item1].Selected == true)
                    {
                        deptcode = "";
                        if (deptcode == "")
                        {
                            deptcode = chklsdept.Items[item1].Value;
                            if (deptcode != "")
                            {
                                mdeptcode = "and st.dept_code in(" + deptcode + ")";
                                fdeptcode = "and sta.dept_code in(" + deptcode + ")";
                                tdeptcode = "and stt.dept_code in(" + deptcode + ")";
                                deptcode = "and stc.dept_code in(" + deptcode + ")";
                            }
                            string strloadQuery = "select hrde.dept_name,hrde.dept_code,isnull((select count(sama.sex) from staff_appl_master sama,hrdept_master hr,stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and st.dept_code=hr.dept_code and sama.appl_no=sm.appl_no and ((sm.settled=0 and sm.resign=0) and (sm.Discontinue='0' or sm.Discontinue is null)) and st.latestrec=1 and sama.sex = 'Male' " + mdeptcode + " " + mcatecode + " " + mdesigncode + " " + mtype + " group by hr.dept_name),'0') as Male,";
                            strloadQuery = strloadQuery + " isnull((select count(samb.sex) from staff_appl_master samb,hrdept_master hra,stafftrans sta,staffmaster smb where sta.staff_code=smb.staff_code and sta.dept_code=hra.dept_code and samb.appl_no=smb.appl_no and ((smb.settled=0 and smb.resign=0) and (smb.Discontinue='0' or smb.Discontinue is null)) and sta.latestrec=1 and samb.sex = 'Female' " + fdeptcode + " " + fcatecode + " " + fdesigncode + " " + ftype + " group by hra.dept_name ),'0') as Female,";
                            strloadQuery = strloadQuery + "isnull((select count(samt.sex) from staff_appl_master samt,hrdept_master hrt,stafftrans stt,staffmaster smt where stt.staff_code=smt.staff_code and stt.dept_code=hrt.dept_code and samt.appl_no=smt.appl_no  and ((smt.settled=0 and smt.resign=0) and (smt.Discontinue='0' or smt.Discontinue is null)) and stt.latestrec=1 " + tdeptcode + " " + tcatecode + " " + tdesigncode + " " + ttype + " group by hrt.dept_name ),'0') as Total";
                            strloadQuery = strloadQuery + " from staff_appl_master samc,hrdept_master hrde,stafftrans stc,staffmaster smc where  stc.staff_code=smc.staff_code and stc.dept_code=hrde.dept_code and samc.appl_no=smc.appl_no and ((smc.settled=0 and smc.resign=0) and (smc.Discontinue='0' or smc.Discontinue is null)) and stc.latestrec=1 " + deptcode + " " + catecode + " " + designcode + " " + type + " group by hrde.dept_name,hrde.dept_code order by hrde.dept_name ";
                            ds.Dispose();
                            ds.Reset();
                            ds = d2.select_method(strloadQuery, hat, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                srno++;
                                Fpstaff.Visible = true;
                                btnprintmaster.Visible = true;
                                Fpstaff.Sheets[0].RowCount++;
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = ds.Tables[0].Rows[0]["dept_name"].ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = ds.Tables[0].Rows[0]["Male"].ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = ds.Tables[0].Rows[0]["Female"].ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = ds.Tables[0].Rows[0]["Total"].ToString();
                            }
                        }
                    }
                }
                if (srno == 0)
                {
                    Fpstaff.Visible = false;
                    btnprintmaster.Visible = false;
                    btnxl.Visible = false;
                    lblrptname.Visible = false;
                    txtrptname.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }
                else
                {
                    deptcode = "";
                    for (int item1 = 0; item1 < chklsdept.Items.Count; item1++)
                    {
                        if (chklsdept.Items[item1].Selected == true)
                        {
                            if (deptcode == "")
                            {
                                deptcode = chklsdept.Items[item1].Value;
                            }
                            else
                            {
                                deptcode = deptcode + ',' + chklsdept.Items[item1].Value;
                            }
                        }
                    }
                    if (deptcode != "")
                    {
                        mdeptcode = "and st.dept_code in(" + deptcode + ")";
                        fdeptcode = "and sta.dept_code in(" + deptcode + ")";
                        tdeptcode = "and stt.dept_code in(" + deptcode + ")";
                        deptcode = "and stc.dept_code in(" + deptcode + ")";
                    }
                    string strloadQuerytotal = "select distinct (select count(sama.sex) from staff_appl_master sama,hrdept_master hr,stafftrans st,staffmaster sm where st.staff_code=sm.staff_code and st.dept_code=hr.dept_code and sama.appl_no=sm.appl_no and ((sm.resign=0 and sm.settled=0) and (sm.Discontinue=0 or sm.Discontinue is null)) and st.latestrec=1 and sama.sex = 'Male' " + mdeptcode + " " + mcatecode + " " + mdesigncode + " " + mtype + " ) as Male,";
                    strloadQuerytotal = strloadQuerytotal + " (select count(samb.sex) from staff_appl_master samb,hrdept_master hra,stafftrans sta,staffmaster smb where sta.staff_code=smb.staff_code and sta.dept_code=hra.dept_code and samb.appl_no=smb.appl_no and ((smb.resign=0 and smb.settled=0) and (smb.Discontinue=0 or smb.Discontinue is null)) and sta.latestrec=1 and samb.sex = 'Female' " + fdeptcode + " " + fcatecode + " " + fdesigncode + " " + ftype + " ) as Female,";
                    strloadQuerytotal = strloadQuerytotal + "(select count(samt.sex) from staff_appl_master samt,hrdept_master hrt,stafftrans stt,staffmaster smt where stt.staff_code=smt.staff_code and stt.dept_code=hrt.dept_code and samt.appl_no=smt.appl_no and ((smt.resign=0 and smt.settled=0) and (smt.Discontinue=0 or smt.Discontinue is null)) and stt.latestrec=1 " + tdeptcode + " " + tcatecode + " " + tdesigncode + " " + ttype + " ) as Total";
                    strloadQuerytotal = strloadQuerytotal + " from staff_appl_master samc,hrdept_master hrde,stafftrans stc,staffmaster smc where  stc.staff_code=smc.staff_code and stc.dept_code=hrde.dept_code and samc.appl_no=smc.appl_no and ((smc.resign=0 and smc.settled=0) and (smc.Discontinue=0 or smc.Discontinue is null)) and stc.latestrec=1 " + deptcode + " " + catecode + " " + designcode + " " + type + "  ";
                    Fpstaff.Sheets[0].RowCount++;
                    DataSet dstotal = d2.select_method(strloadQuerytotal, hat, "Text");
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = "Total";
                    Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 2);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = dstotal.Tables[0].Rows[0]["Male"].ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = dstotal.Tables[0].Rows[0]["Female"].ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = dstotal.Tables[0].Rows[0]["Total"].ToString();
                }
            }
            else if (rdobtn_CategoryWise.Items[1].Selected == true)
            {
                //txttype.Text = "";
                Fpstaff.Width = 750;
                txttype.Enabled = false;
                Fpstaff.Sheets[0].RowCount = 0;
                Fpstaff.Sheets[0].ColumnCount = 2;
                int col_count = 1;
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Department";
                Fpstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].ColumnHeader.Columns[0].Width = 30;
                //Fpstaff.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                for (int cnt_chklstcategory = 0; cnt_chklstcategory < chklscategory.Items.Count; cnt_chklstcategory++)
                {
                    if (chklscategory.Items[cnt_chklstcategory].Selected == true)
                    {
                        col_count++;
                        Fpstaff.Sheets[0].ColumnCount++;
                        Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, col_count].Text = chklscategory.Items[cnt_chklstcategory].Text.ToString();
                        Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, col_count].Tag = chklscategory.Items[cnt_chklstcategory].Value;
                        Fpstaff.Sheets[0].ColumnHeader.Columns[col_count].Width = 50;
                        //Fpstaff.Sheets[0].Columns[col_count].Width = 50;
                        Fpstaff.Sheets[0].Columns[col_count].HorizontalAlign = HorizontalAlign.Center;
                        Fpstaff.Visible = true;
                        btnprintmaster.Visible = true;
                    }
                }
                Fpstaff.Sheets[0].ColumnCount++;
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, Fpstaff.Sheets[0].ColumnCount - 1].Text = "Total";
                Fpstaff.Sheets[0].ColumnHeader.Columns[Fpstaff.Sheets[0].ColumnCount - 1].Width = 40;
                Fpstaff.Sheets[0].Columns[Fpstaff.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                string cat_code = string.Empty;
                string dept_code = string.Empty;
                string deptname = string.Empty;
                string desination_code = string.Empty;
                for (int cnt_dept = 0; cnt_dept < chklsdept.Items.Count; cnt_dept++)
                {
                    int staff_count = 0;
                    int staff_tot_count = 0;
                    int row_incre = 0;
                    if (chklsdept.Items[cnt_dept].Selected == true)
                    {
                        dept_code = chklsdept.Items[cnt_dept].Value;
                        deptname = chklsdept.Items[cnt_dept].Text.ToString();
                        //for (int cnt_desig = 0; cnt_desig < chklsdesign.Items.Count; cnt_desig++)
                        //{
                        //    if (chklsdesign.Items[cnt_desig].Selected == true)
                        //    {
                        //        desination_code = chklsdesign.Items[cnt_desig].Value;
                        for (int cnt_cat = 2; cnt_cat < Fpstaff.Sheets[0].ColumnCount - 1; cnt_cat++)
                        {
                            cat_code = Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, cnt_cat].Tag.ToString();
                            if (cat_code != "" && dept_code != "")
                            {
                                row_incre++;
                                SqlCommand cmd_category_tot_cnt = new SqlCommand("SELECT COUNT(*) as count FROM StaffMaster sm,StaffTrans st WHERE sm.Staff_Code = st.Staff_Code AND College_Code ='" + Session["collegecode"].ToString() + "' and ((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) and latestrec=1 AND Dept_Code=" + dept_code + " AND Category_Code='" + cat_code + "' " + mdesigncode + "", con);
                                SqlDataAdapter da_category_tot_cnt = new SqlDataAdapter(cmd_category_tot_cnt);
                                DataTable dt_category_tot_cnt = new DataTable();
                                da_category_tot_cnt.Fill(dt_category_tot_cnt);
                                if (row_incre == 1)
                                {
                                    Fpstaff.Sheets[0].RowCount++;
                                    srno++;
                                }
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = deptname.ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_cat].Text = dt_category_tot_cnt.Rows[0]["count"].ToString();
                                staff_tot_count = staff_tot_count + Convert.ToInt32(Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_cat].Text.ToString());
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, Fpstaff.Sheets[0].ColumnCount - 1].Text = staff_tot_count.ToString();
                            }
                            //}
                            //}
                            //}
                        }
                    }
                }
                Fpstaff.Sheets[0].RowCount++;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = "Total";
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                for (int cnt_sp_col = 2; cnt_sp_col < Fpstaff.Sheets[0].ColumnCount; cnt_sp_col++)
                {
                    int total_cnt = 0;
                    for (int cnt_sp_row = 0; cnt_sp_row < Fpstaff.Sheets[0].RowCount - 1; cnt_sp_row++)
                    {
                        total_cnt = total_cnt + Convert.ToInt32(Fpstaff.Sheets[0].Cells[cnt_sp_row, cnt_sp_col].Text.ToString());
                    }
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Text = total_cnt.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Bold = true;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Name = "Book Antiqua";
                }
                Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].RowCount;
                Fpstaff.SaveChanges();
            }
            else if (rdobtn_CategoryWise.Items[2].Selected == true)
            {
                //txtcategory.Text = "";
                Fpstaff.Width = 750;
                txtcategory.Enabled = false;
                Fpstaff.Sheets[0].RowCount = 0;
                Fpstaff.Sheets[0].ColumnCount = 2;
                int col_count = 1;
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "Sl.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Department";
                Fpstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpstaff.Sheets[0].ColumnHeader.Columns[0].Width = 30;
                //Fpstaff.Sheets[0].ColumnHeader.Columns[1].Width = 100;
                for (int cnt_stafftype = 0; cnt_stafftype < chklstype.Items.Count; cnt_stafftype++)
                {
                    if (chklstype.Items[cnt_stafftype].Selected == true)
                    {
                        col_count++;
                        Fpstaff.Sheets[0].ColumnCount++;
                        Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, col_count].Text = chklstype.Items[cnt_stafftype].Text.ToString();
                        Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, col_count].Tag = chklstype.Items[cnt_stafftype].Text.ToString();
                        Fpstaff.Sheets[0].ColumnHeader.Columns[col_count].Width = 50;
                        Fpstaff.Sheets[0].Columns[col_count].HorizontalAlign = HorizontalAlign.Center;
                        Fpstaff.Visible = true;
                        btnprintmaster.Visible = true;
                    }
                }
                Fpstaff.Sheets[0].ColumnCount++;
                Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, Fpstaff.Sheets[0].ColumnCount - 1].Text = "Total";
                Fpstaff.Sheets[0].ColumnHeader.Columns[Fpstaff.Sheets[0].ColumnCount - 1].Width = 40;
                Fpstaff.Sheets[0].Columns[Fpstaff.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                string stafftype = string.Empty;
                string dept_code = string.Empty;
                string deptname = string.Empty;
                string desination_code = string.Empty;
                for (int cnt_dept = 0; cnt_dept < chklsdept.Items.Count; cnt_dept++)
                {
                    int staff_count = 0;
                    int staff_tot_count = 0;
                    int row_incre = 0;
                    if (chklsdept.Items[cnt_dept].Selected == true)
                    {
                        dept_code = chklsdept.Items[cnt_dept].Value;
                        deptname = chklsdept.Items[cnt_dept].Text.ToString();
                        //for (int cnt_desig = 0; cnt_desig < chklsdesign.Items.Count; cnt_desig++)
                        //{
                        //    if (chklsdesign.Items[cnt_desig].Selected == true)
                        //    {
                        //        desination_code = chklsdesign.Items[cnt_desig].Value;
                        for (int cnt_cat = 2; cnt_cat < Fpstaff.Sheets[0].ColumnCount - 1; cnt_cat++)
                        {
                            stafftype = Fpstaff.Sheets[0].ColumnHeader.Cells[Fpstaff.Sheets[0].ColumnHeader.RowCount - 1, cnt_cat].Tag.ToString();
                            if (stafftype != "" && dept_code != "")
                            {
                                row_incre++;
                                SqlCommand cmd_stafftype_tot_cnt = new SqlCommand("SELECT COUNT(*) as count FROM StaffMaster sm,StaffTrans st WHERE sm.Staff_Code = st.Staff_Code AND College_Code = 13 and ((resign=0 and settled=0) and (Discontinue=0 or Discontinue is null)) and latestrec=1 AND Dept_Code =" + dept_code + " AND StfType = '" + stafftype + "' " + mdesigncode + "", con);
                                SqlDataAdapter da_stafftype_tot_cnt = new SqlDataAdapter(cmd_stafftype_tot_cnt);
                                DataTable dt_stafftype_tot_cnt = new DataTable();
                                da_stafftype_tot_cnt.Fill(dt_stafftype_tot_cnt);
                                if (row_incre == 1)
                                {
                                    Fpstaff.Sheets[0].RowCount++;
                                    srno++;
                                }
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = deptname.ToString();
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_cat].Text = dt_stafftype_tot_cnt.Rows[0]["count"].ToString();
                                staff_tot_count = staff_tot_count + Convert.ToInt32(Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_cat].Text.ToString());
                                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, Fpstaff.Sheets[0].ColumnCount - 1].Text = staff_tot_count.ToString();
                            }
                            //}
                            //}
                            //}
                        }
                    }
                }
                Fpstaff.Sheets[0].RowCount++;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = "Total";
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Right;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Bold = true;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                for (int cnt_sp_col = 2; cnt_sp_col < Fpstaff.Sheets[0].ColumnCount; cnt_sp_col++)
                {
                    int total_cnt = 0;
                    for (int cnt_sp_row = 0; cnt_sp_row < Fpstaff.Sheets[0].RowCount - 1; cnt_sp_row++)
                    {
                        total_cnt = total_cnt + Convert.ToInt32(Fpstaff.Sheets[0].Cells[cnt_sp_row, cnt_sp_col].Text.ToString());
                    }
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Text = total_cnt.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Bold = true;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, cnt_sp_col].Font.Name = "Book Antiqua";
                }
                Fpstaff.Sheets[0].PageSize = Fpstaff.Sheets[0].RowCount;
                Fpstaff.SaveChanges();
            }
            int rowcount = Fpstaff.Sheets[0].RowCount;
            Fpstaff.Height = 300;
            Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
            Fpstaff.SaveChanges();
        }
        catch (Exception ex)
        {
            errmsg.Visible = true;
            errmsg.Text = ex.ToString();
            d2.sendErrorMail(ex, "13", "StaffStrenthReport.aspx");
        }
    }
    public void LoadHeader()
    {
        if (rdobtn_CategoryWise.Items[0].Selected == true)
        {
            Fpstaff.Width = 750;
            Fpstaff.Sheets[0].ColumnHeader.RowCount = 1;
            Fpstaff.Sheets[0].ColumnCount = 5;
            Fpstaff.Sheets[0].ColumnHeader.Columns[0].Width = 30;
            Fpstaff.Sheets[0].ColumnHeader.Columns[1].Width = 100;
            Fpstaff.Sheets[0].ColumnHeader.Columns[2].Width = 50;
            Fpstaff.Sheets[0].ColumnHeader.Columns[3].Width = 50;
            Fpstaff.Sheets[0].ColumnHeader.Columns[4].Width = 50;
            Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
            Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Male";
            Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Female";
            Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Total";
            Fpstaff.Sheets[0].Columns[0].Width = 50;
            Fpstaff.Sheets[0].Columns[1].Width = 150;
            Fpstaff.Sheets[0].Columns[2].Width = 80;
            Fpstaff.Sheets[0].Columns[3].Width = 80;
            Fpstaff.Sheets[0].Columns[4].Width = 80;
            Fpstaff.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
            Fpstaff.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
            Fpstaff.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
            Fpstaff.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
            Fpstaff.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
            Fpstaff.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            Fpstaff.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
            Fpstaff.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            Fpstaff.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Center;
            Fpstaff.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Center;
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        string reportname = txtrptname.Text.ToString().Trim();
        if (reportname != "")
        {
            d2.printexcelreport(Fpstaff, reportname);
        }
    }
    protected void rdobtn_CategoryWise_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdobtn_CategoryWise.Items[0].Selected == true)
        {
            txttype.Enabled = true;
            txtcategory.Enabled = true;
        }
        else if (rdobtn_CategoryWise.Items[1].Selected == true)
        {
            txttype.Enabled = false;
            txtcategory.Enabled = true;
        }
        else if (rdobtn_CategoryWise.Items[2].Selected == true)
        {
            txtcategory.Enabled = false;
            txttype.Enabled = true;
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        string degreedetails = string.Empty;
        Session["column_header_row_count"] = Fpstaff.Sheets[0].ColumnHeader.RowCount;
        degreedetails = "Staff Strength Report ";
        string pagename = "StudentTestReport.aspx";
        Printcontrol.loadspreaddetails(Fpstaff, pagename, degreedetails);
        Printcontrol.Visible = true;
    }
}