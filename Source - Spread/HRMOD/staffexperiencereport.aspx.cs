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
public partial class staffexperiencereport : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    string usercode = "", singleuser = "", group_user = "";
    string collegecode = "";
    string strquery = "";
    int count = 0;

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
            errmsg.Visible = false;
            if (!Page.IsPostBack)
            {
                txtfromdate.Attributes.Add("readonly", "readonly");
                txttodate.Attributes.Add("readonly", "readonly");

                string cdate = DateTime.Now.ToString("dd/MM/yyyy");
                txtfromdate.Text = cdate;
                txttodate.Text = cdate;

                Fpexperience.Sheets[0].SheetName = " ";
                Fpexperience.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                Fpexperience.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Center;
                Fpexperience.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                Fpexperience.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                Fpexperience.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = Color.Black;
                style1.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpexperience.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpexperience.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                Fpexperience.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                Fpexperience.Sheets[0].AllowTableCorner = true;
                //---------------page number
                Fpexperience.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                Fpexperience.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                Fpexperience.Pager.Align = HorizontalAlign.Right;
                Fpexperience.Pager.Font.Bold = true;
                Fpexperience.Pager.Font.Name = "Book Antiqua";
                Fpexperience.Pager.ForeColor = Color.DarkGreen;
                Fpexperience.Pager.BackColor = Color.Beige;
                Fpexperience.Pager.BackColor = Color.AliceBlue;
                Fpexperience.Pager.PageCount = 5;
                Fpexperience.CommandBar.Visible = false;
                //---------------------------

                ds = new DataSet();
                ddlcollege.Items.Insert(0, "All");
                ds = d2.select_method_wo_parameter("select collname,college_code,acr from collinfo", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                    collegecode = ddlcollege.SelectedValue.ToString();
                    BindDepartment();
                    BindDesignation();
                    Bindcategory();
                    bindtype();
                    bindselecttype();
                }
                else
                {
                    btnMainGo.Visible = false;
                }
                Printcontrol.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Fpexperience.Visible = false;
                lblnorec.Visible = false;
                Chkfilter.Checked = true;
                lbldate.Enabled = false;
                txtfromdate.Enabled = false;
                lbltodate.Enabled = false;
                txttodate.Enabled = false;
            }
            lblnorec.Visible = false;
        }
        catch (Exception ex) { }
    }
    public void BindDepartment()
    {

        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            chklsdept.Items.Clear();
            ds.Clear();
            string singleuser = Session["single_user"].ToString();
            if (singleuser == "True")
            {
                strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where user_code=" + Session["usercode"] + " and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            else
            {
                group_user = Session["group_code"].ToString();
                if (group_user.Contains(';'))
                {
                    string[] group_semi = group_user.Split(';');
                    group_user = group_semi[0].ToString();
                }
                strquery = "SELECT DISTINCT hp.dept_code,dept_name from hr_privilege hp,hrdept_master hr  where group_code='" + group_user + "' and hr.dept_code=hp.dept_code  and hp.dept_code in (select dept_code from hrdept_master where college_code='" + collegecode + "') order by dept_name";
            }
            if (strquery != "")
            {
                ds = d2.select_method_wo_parameter(strquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklsdept.DataSource = ds;
                    chklsdept.DataTextField = "dept_name";
                    chklsdept.DataValueField = "Dept_Code";
                    chklsdept.DataBind();

                    for (int i = 0; i < chklsdept.Items.Count; i++)
                    {
                        chklsdept.Items[i].Selected = true;
                    }
                    txtdept.Text = "Dept (" + chklsdept.Items.Count.ToString() + ")";
                    chkdept.Checked = true;
                }
            }
        }
        catch
        {
        }
    }
    public void BindDesignation()
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            chklsdesign.Visible = true;
            chklsdesign.Items.Clear();
            ds.Clear();
            ds = d2.loaddesignation(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdesign.DataSource = ds;
                chklsdesign.DataTextField = "desig_name";
                chklsdesign.DataValueField = "Desig_Code";
                chklsdesign.DataBind();
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = true;
                }
                txtdesign.Text = "Design (" + chklsdesign.Items.Count.ToString() + ")";
                chkdesign.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void Bindcategory()
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            chklscetegory.Visible = true;
            chklscetegory.Items.Clear();
            ds.Clear();
            ds = d2.loadcategory(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklscetegory.DataSource = ds;
                chklscetegory.DataTextField = "category_name";
                chklscetegory.DataValueField = "Category_Code";
                chklscetegory.DataBind();
                for (int i = 0; i < chklscetegory.Items.Count; i++)
                {
                    chklscetegory.Items[i].Selected = true;
                }
                txtcategory.Text = "Category (" + chklscetegory.Items.Count.ToString() + ")";
                chkcategory.Checked = true;
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
            collegecode = ddlcollege.SelectedValue.ToString();
            chklstype.Visible = true;
            chklstype.Items.Clear();
            ds.Clear();
            ds = d2.loadstafftype(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklstype.DataSource = ds;
                chklstype.DataTextField = "StfType";
                chklstype.DataBind();
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;
                }
                txttype.Text = "Type (" + chklstype.Items.Count.ToString() + ")";
                chktype.Checked = true;
            }
        }
        catch
        {
        }
    }

    public void bindselecttype()
    {
        string collac = d2.GetFunction("select Coll_acronymn from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'");
        ddltype.Items.Insert(0, "Both");
        ddltype.Items.Insert(1, collac + " Experience");

    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDepartment();
            BindDesignation();
            Bindcategory();
            bindtype();
            bindselecttype();
        }
        catch
        {
        }
    }
    public void chkdept_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdept.Checked == true)
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = true;
                }
                txtdept.Text = "Dept (" + (chklsdept.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsdept.Items.Count; i++)
                {
                    chklsdept.Items[i].Selected = false;
                }
                txtdept.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    protected void chklsdept_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";
            txtdept.Text = "---Select---";
            chkdept.Checked = false;
            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    value = chklsdept.Items[i].Text;
                    code = chklsdept.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                }

            }
            if (batchcount == chklsdept.Items.Count)
            {
                txtdept.Text = "Dept (" + batchcount.ToString() + ")";
                chkdept.Checked = true;
            }
            else if (batchcount > 0)
            {
                txtdept.Text = "Dept (" + batchcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void chktype_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chktype.Checked == true)
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = true;

                }
                txttype.Text = "Type (" + (chklstype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstype.Items.Count; i++)
                {
                    chklstype.Items[i].Selected = false;
                }
                txttype.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    protected void chklstype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";
            txttype.Text = "---Select---";
            chktype.Checked = false;
            for (int i = 0; i < chklstype.Items.Count; i++)
            {
                if (chklstype.Items[i].Selected == true)
                {
                    value = chklstype.Items[i].Text;
                    code = chklstype.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                }

            }
            if (batchcount == chklstype.Items.Count)
            {
                txttype.Text = "Type (" + batchcount.ToString() + ")";
                chktype.Checked = true;
            }
            else if (batchcount > 0)
            {
                txttype.Text = "Type (" + batchcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void chkdesign_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkdesign.Checked == true)
            {
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = true;

                }
                txtdesign.Text = "Design (" + (chklsdesign.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklsdesign.Items.Count; i++)
                {
                    chklsdesign.Items[i].Selected = false;
                }
                txtdesign.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    protected void chklsdesign_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";
            txtdesign.Text = "---Select---";
            chkdesign.Checked = false;
            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    value = chklsdesign.Items[i].Text;
                    code = chklsdesign.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                }

            }
            if (batchcount == chklsdesign.Items.Count)
            {
                txtdesign.Text = "Design (" + batchcount.ToString() + ")";
                chkdesign.Checked = true;
            }
            else if (batchcount > 0)
            {
                txtdesign.Text = "Design (" + batchcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void chkcategory_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkcategory.Checked == true)
            {
                for (int i = 0; i < chklscetegory.Items.Count; i++)
                {
                    chklscetegory.Items[i].Selected = true;

                }
                txtcategory.Text = "Category (" + (chklscetegory.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklscetegory.Items.Count; i++)
                {
                    chklscetegory.Items[i].Selected = false;
                }
                txtcategory.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    protected void chklscetegory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int batchcount = 0;
            string value = "";
            string code = "";
            txtcategory.Text = "---Select---";
            chkcategory.Checked = false;
            for (int i = 0; i < chklscetegory.Items.Count; i++)
            {
                if (chklscetegory.Items[i].Selected == true)
                {
                    value = chklscetegory.Items[i].Text;
                    code = chklscetegory.Items[i].Value.ToString();
                    batchcount = batchcount + 1;
                }

            }
            if (batchcount == chklscetegory.Items.Count)
            {
                txtcategory.Text = "Category (" + batchcount.ToString() + ")";
                chkcategory.Checked = true;
            }
            else if (batchcount > 0)
            {
                txtcategory.Text = "Category (" + batchcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void btnMainGo_Click(object sender, EventArgs e)
    {
        try
        {
            Fpexperience.Sheets[0].ColumnCount = 0;
            Printcontrol.Visible = false;
            btnxl.Visible = false;
            btnprintmaster.Visible = false;
            txtexcelname.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Text = "";
            string deptcode = "";
            string design = "";
            string category = "";
            string type = "";
            int avaexperince = 0;
            if (chkdate.Checked == true)
            {
                datefunction();
            }
            string fdate = txtfromdate.Text.ToString();
            string tdate = txttodate.Text.ToString();

            string[] sf = fdate.Split('/');
            string[] st = tdate.Split('/');
            DateTime dtf = Convert.ToDateTime(sf[1] + '/' + sf[0] + '/' + sf[2]);
            DateTime dtt = Convert.ToDateTime(st[1] + '/' + st[0] + '/' + st[2]);
            string getexper = txtexperince.Text.ToString();
            if (getexper.Trim() != null && getexper.Trim() != "")
            {
                avaexperince = Convert.ToInt32(getexper);
            }
            if (Chkfilter.Checked == true)
            {
                if (getexper.Trim() == null || getexper.Trim() == "")
                {
                    txtexcelname.Text = "";
                    Printcontrol.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    Fpexperience.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter Value and Then Proceed";
                    return;
                }
                else if (Convert.ToInt32(getexper.ToString()) == 0)
                {
                    txtexcelname.Text = "";
                    Printcontrol.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    Fpexperience.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter Value Greater Than Zero";
                    return;
                }
            }

            for (int i = 0; i < chklsdept.Items.Count; i++)
            {
                if (chklsdept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        deptcode = deptcode + ",'" + chklsdept.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (deptcode.Trim() != "")
            {
                deptcode = " and st.dept_code in(" + deptcode + ")";
            }

            for (int i = 0; i < chklsdesign.Items.Count; i++)
            {
                if (chklsdesign.Items[i].Selected == true)
                {
                    if (design == "")
                    {
                        design = "'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        design = design + ",'" + chklsdesign.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (design.Trim() != "")
            {
                design = " and st.desig_code in(" + design + ")";
            }
            for (int i = 0; i < chklscetegory.Items.Count; i++)
            {
                if (chklscetegory.Items[i].Selected == true)
                {
                    if (category == "")
                    {
                        category = "'" + chklscetegory.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        category = category + ",'" + chklscetegory.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (category.Trim() != "")
            {
                category = " and st.category_code in(" + category + ")";
            }

            for (int i = 0; i < chklstype.Items.Count; i++)
            {
                if (chklstype.Items[i].Selected == true)
                {
                    if (type == "")
                    {
                        type = "'" + chklstype.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        type = type + ",'" + chklstype.Items[i].Value.ToString() + "'";
                    }
                }
            }
            if (type.Trim() != "")
            {
                type = " and st.stftype in(" + type + ")";
            }
            lblnorec.Visible = false;
            errmsg.Visible = false;
            strquery = "select distinct hd.priority,dm.priority,sm.join_date,sm.staff_code,sm.staff_code,hd.dept_name,len(sm.staff_code),sm.staff_name,sa.experience_info,convert(nvarchar(15),sm.join_date,101) as jdate,hd.dept_name,dm.desig_name,sc.category_name,st.stftype from staff_appl_master sa,staffmaster sm,stafftrans st,hrdept_master hd,desig_master dm,staffcategorizer sc where st.staff_code=sm.staff_code and sa.appl_no=sm.appl_no and sm.resign=0 and settled=0 and st.latestrec=1  and hd.dept_code=st.dept_code and dm.desig_code=st.desig_code and st.category_code=sc.category_code " + deptcode + " " + design + " " + category + " " + type + " ";
            if (chkdate.Checked == true)
            {
                strquery = strquery + " and sm.join_date Between '" + dtf.ToString() + "' and '" + dtt.ToString() + "' ";
            }
            if (ddlorder.SelectedItem.ToString() == "Priority")
            {
                strquery = strquery + "  order by hd.priority,dm.priority,sm.join_date";
            }
            else
            {
                strquery = strquery + "  order by hd.dept_name,sm.join_date,len(sm.staff_code),sm.staff_code";
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtexcelname.Text = "";
                Printcontrol.Visible = false;
                btnxl.Visible = true;
                btnprintmaster.Visible = true;
                txtexcelname.Visible = true;
                lblrptname.Visible = true;
                Fpexperience.Visible = true;
                Fpexperience.SheetCorner.ColumnCount = 0;
                Fpexperience.Sheets[0].ColumnHeader.RowCount = 1;
                Fpexperience.Sheets[0].ColumnHeader.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                Fpexperience.Sheets[0].ColumnCount = 11;

                Fpexperience.Sheets[0].Columns[0].Width = 50;
                Fpexperience.Sheets[0].Columns[1].Width = 100;
                Fpexperience.Sheets[0].Columns[2].Width = 150;
                Fpexperience.Sheets[0].Columns[3].Width = 150;
                Fpexperience.Sheets[0].Columns[4].Width = 150;
                Fpexperience.Sheets[0].Columns[5].Width = 150;
                Fpexperience.Sheets[0].Columns[6].Width = 150;
                Fpexperience.Sheets[0].Columns[7].Width = 150;
                Fpexperience.Sheets[0].Columns[8].Width = 150;
                Fpexperience.Sheets[0].Columns[9].Width = 150;
                Fpexperience.Sheets[0].Columns[10].Width = 150;

                Fpexperience.Sheets[0].Columns[0].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[1].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[2].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[3].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[4].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[5].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[6].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[7].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[8].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[9].Font.Name = "Book Antiqua";
                Fpexperience.Sheets[0].Columns[10].Font.Name = "Book Antiqua";


                Fpexperience.Sheets[0].Columns[0].Locked = true;
                Fpexperience.Sheets[0].Columns[1].Locked = true;
                Fpexperience.Sheets[0].Columns[2].Locked = true;
                Fpexperience.Sheets[0].Columns[3].Locked = true;
                Fpexperience.Sheets[0].Columns[4].Locked = true;
                Fpexperience.Sheets[0].Columns[5].Locked = true;
                Fpexperience.Sheets[0].Columns[6].Locked = true;
                Fpexperience.Sheets[0].Columns[7].Locked = true;
                Fpexperience.Sheets[0].Columns[8].Locked = true;
                Fpexperience.Sheets[0].Columns[9].Locked = true;
                Fpexperience.Sheets[0].Columns[10].Locked = true;

                Fpexperience.Width = 900;


                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                Fpexperience.Sheets[0].Columns[0].CellType = txt;
                Fpexperience.Sheets[0].Columns[1].CellType = txt;
                Fpexperience.Sheets[0].Columns[2].CellType = txt;
                Fpexperience.Sheets[0].Columns[3].CellType = txt;
                Fpexperience.Sheets[0].Columns[4].CellType = txt;
                Fpexperience.Sheets[0].Columns[5].CellType = txt;
                Fpexperience.Sheets[0].Columns[6].CellType = txt;
                Fpexperience.Sheets[0].Columns[7].CellType = txt;
                Fpexperience.Sheets[0].Columns[8].CellType = txt;
                Fpexperience.Sheets[0].Columns[9].CellType = txt;
                Fpexperience.Sheets[0].Columns[10].CellType = txt;

                Fpexperience.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                Fpexperience.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Left;
                Fpexperience.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Left;

                string collac = d2.GetFunction("select Coll_acronymn from collinfo where college_code='" + ddlcollege.SelectedValue.ToString() + "'");

                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Category";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Type";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Joining Date";
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Other Experience";
                if (collac.Trim() != "" && collac != null && collac.Trim() != "0")
                {
                    Fpexperience.Sheets[0].ColumnHeader.Cells[0, 9].Text = "" + collac + " Experience";
                }
                else
                {
                    Fpexperience.Sheets[0].ColumnHeader.Cells[0, 9].Text = "College Experience";
                }
                Fpexperience.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Total Experience";
                if (Chkfilter.Checked == true)
                {
                    if (ddltype.SelectedIndex.ToString() == "0")
                    {
                        Fpexperience.Sheets[0].Columns[8].Visible = true;
                        Fpexperience.Sheets[0].Columns[9].Visible = true;
                        Fpexperience.Sheets[0].Columns[10].Visible = true;
                    }
                    else if (ddltype.SelectedIndex.ToString() == "1")
                    {
                        Fpexperience.Sheets[0].Columns[8].Visible = false;
                        Fpexperience.Sheets[0].Columns[9].Visible = true;
                        Fpexperience.Sheets[0].Columns[10].Visible = false;
                    }
                    else if (ddltype.SelectedIndex.ToString() == "2")
                    {
                        Fpexperience.Sheets[0].Columns[8].Visible = true;
                        Fpexperience.Sheets[0].Columns[9].Visible = false;
                        Fpexperience.Sheets[0].Columns[10].Visible = false;
                    }
                }
                else
                {
                    Fpexperience.Sheets[0].Columns[8].Visible = true;
                    Fpexperience.Sheets[0].Columns[9].Visible = true;
                    Fpexperience.Sheets[0].Columns[10].Visible = true;
                }
                Fpexperience.Sheets[0].RowCount = 0;
                int srno = 0;
                string tempdept = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string staffcode = ds.Tables[0].Rows[i]["staff_code"].ToString();
                    string staffname = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    string dept = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    string designname = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    string categoryname = ds.Tables[0].Rows[i]["category_name"].ToString();
                    string typename = ds.Tables[0].Rows[i]["stftype"].ToString();
                    string perexp = ds.Tables[0].Rows[i]["experience_info"].ToString();
                    string joindate = ds.Tables[0].Rows[i]["jdate"].ToString();


                    Boolean valflag = false;

                    int expyear = 0;
                    int expmon = 0;
                    string previousexperience = "";
                    string[] spit = perexp.Split('\\');
                    for (int s = 0; s <= spit.GetUpperBound(0); s++)
                    {
                        if (spit[s].Trim().ToString() != "" && spit[s] != "")
                        {
                            string[] sporg = spit[s].Split(';');
                            if (sporg.GetUpperBound(0) > 10)
                            {
                                string yer = sporg[6].ToString();
                                if (yer.ToString().Trim() != "" && yer != null)
                                {
                                    expyear = expyear + Convert.ToInt32(yer);
                                }
                                string mon = sporg[7].ToString();
                                if (mon.ToString().Trim() != "" && mon != null)
                                {
                                    expmon = expmon + Convert.ToInt32(mon);
                                }
                            }
                        }
                    }
                    int exy = 0;
                    int exaxcm = 0;
                    if (expmon.ToString().Trim() != "" && expmon != null)
                    {
                        if (expmon > 11)
                        {
                            exy = expmon / 12;
                            exaxcm = expmon % 12;
                        }
                        else
                        {
                            exaxcm = expmon;
                        }
                    }
                    expyear = expyear + exy;
                    if (expyear > 0 || exaxcm > 0)
                    {
                        if (expyear > 0)
                        {
                            previousexperience = " Years :" + expyear + "";
                        }
                        if (exaxcm > 0)
                        {
                            if (previousexperience.Trim() != "")
                            {
                                previousexperience = previousexperience + " Months :" + exaxcm + "";
                            }
                            else
                            {
                                previousexperience = " Months :" + exaxcm + "";
                            }
                        }
                        if (ddltype.SelectedIndex.ToString() == "2")
                        {
                            if (avaexperince > 0)
                            {
                                if (ddlselect.SelectedIndex.ToString() == "0")
                                {
                                    if (avaexperince == expyear && exaxcm == 0)
                                    {
                                        valflag = true;
                                    }
                                }
                                else if (ddlselect.SelectedIndex.ToString() == "1")
                                {
                                    if (avaexperince > expyear)
                                    {
                                        valflag = true;
                                    }
                                }
                                else if (ddlselect.SelectedIndex.ToString() == "2")
                                {
                                    if (avaexperince < expyear)
                                    {
                                        valflag = true;
                                    }
                                    else if (avaexperince == expyear && exaxcm > 0)
                                    {
                                        valflag = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        previousexperience = "-";
                    }

                    int cureyear = 0;
                    int curemonth = 0;
                    string collexperience = "";
                    string joindatestaff = "-";
                    if (joindate.Trim() != "" && joindate != null)
                    {
                        DateTime dtexp = Convert.ToDateTime(joindate);
                        joindatestaff = dtexp.ToString("dd/MM/yyyy");
                    }
                    if (joindate.Trim() != "" && joindate != null)
                    {
                        DateTime dt = DateTime.Now;
                        DateTime dtexp = Convert.ToDateTime(joindate);
                        int cury = Convert.ToInt32(dt.ToString("yyyy"));
                        int jyear = Convert.ToInt32(dtexp.ToString("yyyy"));
                        cureyear = cury - jyear;

                        int curmon = Convert.ToInt32(dt.ToString("MM"));
                        int jmon = Convert.ToInt32(dtexp.ToString("MM"));
                        if (curmon < jmon)
                        {
                            curemonth = (curmon + 12) - jmon;
                            cureyear--;
                        }
                        else
                        {
                            curemonth = curmon - jmon;
                        }

                        if (cureyear > 0 || curemonth > 0)
                        {
                            collexperience = "";
                            if (cureyear > 0)
                            {
                                collexperience = " Years :" + cureyear + "";
                            }
                            if (curemonth > 0)
                            {
                                if (collexperience.Trim() != "")
                                {
                                    collexperience = collexperience + " Months :" + curemonth + "";
                                }
                                else
                                {
                                    collexperience = " Months :" + curemonth + "";
                                }
                            }
                            if (ddltype.SelectedIndex.ToString() == "1")
                            {
                                if (avaexperince > 0)
                                {
                                    if (ddlselect.SelectedIndex.ToString() == "0")
                                    {
                                        if (avaexperince == cureyear && curemonth == 0)
                                        {
                                            valflag = true;
                                        }
                                    }
                                    else if (ddlselect.SelectedIndex.ToString() == "1")
                                    {
                                        if (avaexperince > cureyear)
                                        {
                                            valflag = true;
                                        }
                                    }
                                    else if (ddlselect.SelectedIndex.ToString() == "2")
                                    {
                                        if (avaexperince < cureyear)
                                        {
                                            valflag = true;
                                        }
                                        else if (avaexperince == cureyear && curemonth > 0)
                                        {
                                            valflag = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        collexperience = "-";
                    }



                    int totalexpyear = cureyear + expyear;
                    int totalexpmonth = curemonth + exaxcm;
                    string totalexperience = "";
                    if (totalexpmonth > 11)
                    {
                        totalexpmonth = totalexpmonth - 12;
                        totalexpyear++;
                    }
                    if (totalexpyear > 0 || totalexpmonth > 0)
                    {
                        totalexperience = "";
                        if (totalexpyear > 0)
                        {
                            totalexperience = " Years :" + totalexpyear + "";
                        }
                        if (totalexpmonth > 0)
                        {
                            if (totalexperience.Trim() != "")
                            {
                                totalexperience = totalexperience + " Months :" + totalexpmonth + "";
                            }
                            else
                            {
                                totalexperience = " Months :" + totalexpmonth + "";
                            }
                        }
                        if (ddltype.SelectedIndex.ToString() == "0")
                        {
                            if (avaexperince > 0)
                            {
                                if (ddlselect.SelectedIndex.ToString() == "0")
                                {
                                    if (avaexperince == totalexpyear && totalexpmonth == 0)
                                    {
                                        valflag = true;
                                    }
                                }
                                else if (ddlselect.SelectedIndex.ToString() == "1")
                                {
                                    if (avaexperince > totalexpyear)
                                    {
                                        valflag = true;
                                    }
                                }
                                else if (ddlselect.SelectedIndex.ToString() == "2")
                                {
                                    if (avaexperince < totalexpyear)
                                    {
                                        valflag = true;
                                    }
                                    else if (avaexperince == totalexpyear && totalexpmonth > 0)
                                    {
                                        valflag = true;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        totalexperience = "-";
                    }

                    if (Chkfilter.Checked == false)
                    {
                        valflag = true;
                    }
                    if (valflag == true)
                    {
                        if (tempdept != dept)
                        {
                            Fpexperience.Sheets[0].RowCount++;
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 0].Text = dept;
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 0].BackColor = Color.LightYellow;
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpexperience.Sheets[0].SpanModel.Add(Fpexperience.Sheets[0].RowCount - 1, 0, 1, 10);
                            tempdept = dept;
                        }
                        srno++;
                        Fpexperience.Sheets[0].RowCount++;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 1].Text = staffcode;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 2].Text = staffname;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 3].Text = dept;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 4].Text = designname;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 5].Text = categoryname;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 6].Text = typename;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 7].Text = joindatestaff;
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                        if (joindatestaff.Trim() == "-")
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        }
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 8].Text = previousexperience;
                        if (previousexperience == "-")
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                        }
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 9].Text = collexperience;
                        if (collexperience == "-")
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                        }
                        Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 10].Text = totalexperience;
                        if (totalexperience == "-")
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        }
                        else
                        {
                            Fpexperience.Sheets[0].Cells[Fpexperience.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                        }
                    }
                }
                if (Fpexperience.Sheets[0].RowCount == 0)
                {
                    txtexcelname.Text = "";
                    Printcontrol.Visible = false;
                    btnxl.Visible = false;
                    btnprintmaster.Visible = false;
                    txtexcelname.Visible = false;
                    lblrptname.Visible = false;
                    Fpexperience.Visible = false;
                    errmsg.Visible = true;
                    errmsg.Text = "No Records Found";
                }
            }
            else
            {
                txtexcelname.Text = "";
                Printcontrol.Visible = false;
                btnxl.Visible = false;
                btnprintmaster.Visible = false;
                txtexcelname.Visible = false;
                lblrptname.Visible = false;
                Fpexperience.Visible = false;
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
            }
            Fpexperience.Sheets[0].PageSize = Fpexperience.Sheets[0].RowCount;
        }
        catch
        {
        }
    }
    protected void Chkfilter_CheckedChanged(object sender, EventArgs e)
    {
        txtexcelname.Text = "";
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        Fpexperience.Visible = false;
        txtexperince.Text = "";
        if (Chkfilter.Checked == true)
        {
            ddltype.Enabled = true;
            ddlselect.Enabled = true;
            txtexperince.Enabled = true;
        }
        else
        {
            ddltype.Enabled = false;
            ddlselect.Enabled = false;
            txtexperince.Enabled = false;
        }
    }
    public void datefunction()
    {
        try
        {
            string fdate = txtfromdate.Text.ToString();
            string tdate = txttodate.Text.ToString();

            string[] sf = fdate.Split('/');
            string[] st = tdate.Split('/');
            DateTime dtf = Convert.ToDateTime(sf[1] + '/' + sf[0] + '/' + sf[2]);
            DateTime dtt = Convert.ToDateTime(st[1] + '/' + st[0] + '/' + st[2]);
            DateTime dtcur = DateTime.Now;
            if (dtf > dtcur)
            {
                txttodate.Text = dtcur.ToString("dd/MM/yyyy");
                txtfromdate.Text = dtcur.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser then Current Date";
                return;
            }
            if (dtt > dtcur)
            {
                txttodate.Text = dtcur.ToString("dd/MM/yyyy");
                txtfromdate.Text = dtcur.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "To Date Must Be Lesser then Current Date";
                return;
            }
            if (dtf > dtt)
            {
                txttodate.Text = dtcur.ToString("dd/MM/yyyy");
                txtfromdate.Text = dtcur.ToString("dd/MM/yyyy");
                errmsg.Visible = true;
                errmsg.Text = "From Date Must Be Lesser then To Date";
                return;
            }
        }
        catch
        {
        }
    }
    protected void chkdate_CheckedChanged(object sender, EventArgs e)
    {
        string cdate = DateTime.Now.ToString("dd/MM/yyyy");
        txtfromdate.Text = cdate;
        txttodate.Text = cdate;
        Printcontrol.Visible = false;
        btnxl.Visible = false;
        btnprintmaster.Visible = false;
        txtexcelname.Visible = false;
        lblrptname.Visible = false;
        txtexcelname.Text = "";
        if (chkdate.Checked == true)
        {
            lbldate.Enabled = true;
            txtfromdate.Enabled = true;
            lbltodate.Enabled = true;
            txttodate.Enabled = true;
        }
        else
        {
            lbldate.Enabled = false;
            txtfromdate.Enabled = false;
            lbltodate.Enabled = false;
            txttodate.Enabled = false;
        }
    }
    protected void txtfromdate_TextChanged(object sender, EventArgs e)
    {
        datefunction();
    }
    protected void txttodate_TextChanged(object sender, EventArgs e)
    {
        datefunction();
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpexperience, reportname);
            }
            else
            {
                lblnorec.Text = "Please Enter Your Report Name";
                lblnorec.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Printcontrol.loadspreaddetails(Fpexperience, "staffexperincereport.aspx", "Staff Experience Report @ Printed at :" + DateTime.Now.ToString("dd/MM/yyyy") + "");
        Printcontrol.Visible = true;
    }
}