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
using System.Configuration;
public partial class Routtransfer : System.Web.UI.Page
{
    #region "Basic Function"
    string collegecode = "", course_id = "";
    string usercode = "", singleuser = "", group_user = "";
    Hashtable hat = new Hashtable();
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    string PageLogOut = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("TransportIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/TransportMod/TransportIndex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                setLabelText();
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].SheetName = " ";
                FpSpread1.Sheets[0].SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;

                FarPoint.Web.Spread.StyleInfo style1 = new FarPoint.Web.Spread.StyleInfo();
                style1.Font.Size = 12;
                style1.Font.Bold = true;
                style1.HorizontalAlign = HorizontalAlign.Center;
                style1.ForeColor = System.Drawing.Color.Black;
                FpSpread1.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style1);
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].AllowTableCorner = true;


                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Right;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = System.Drawing.Color.DarkGreen;
                FpSpread1.Pager.BackColor = System.Drawing.Color.Beige;
                FpSpread1.Pager.BackColor = System.Drawing.Color.AliceBlue;
                FpSpread1.Pager.PageCount = 5;
                FpSpread1.Visible = false;


                FpSpread1.Pager.Position = FarPoint.Web.Spread.PagerPosition.Bottom;
                FpSpread1.Pager.Mode = FarPoint.Web.Spread.PagerMode.Both;
                FpSpread1.Pager.Align = HorizontalAlign.Left;
                FpSpread1.Pager.Font.Bold = true;
                FpSpread1.Pager.Font.Name = "Book Antiqua";
                FpSpread1.Pager.ForeColor = Color.DarkGreen;
                FpSpread1.Sheets[0].Columns.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].Rows.Default.HorizontalAlign = HorizontalAlign.Left;
                FpSpread1.Sheets[0].Rows.Default.VerticalAlign = VerticalAlign.Middle;
                FpSpread1.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.SheetCorner.Columns[0].Visible = false;
                FpSpread1.Sheets[0].ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].DefaultStyle.Font.Bold = false;
                FpSpread1.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
                FpSpread1.CommandBar.Visible = false;

                string group_code = Session["group_code"].ToString();
                string columnfield = "";
                string ucode = "";
                if (group_code.Contains(';'))
                {
                    string[] group_semi = group_code.Split(';');
                    group_code = group_semi[0].ToString();
                }
                if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
                {
                    columnfield = " and group_code='" + group_code + "'";
                    ucode = " group_code='" + group_code + "'";
                }
                else
                {
                    columnfield = " and user_code='" + Session["usercode"] + "'";
                    ucode = " usercode='" + Session["usercode"] + "'";
                }
                hat.Clear();
                hat.Add("column_field", columnfield.ToString());
                ds = d2.select_method("bind_college", hat, "sp");
                ddlcollege.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlcollege.Enabled = true;
                    ddlcollege.DataSource = ds;
                    ddlcollege.DataTextField = "collname";
                    ddlcollege.DataValueField = "college_code";
                    ddlcollege.DataBind();
                }
                Session["Rollflag"] = "0";
                Session["Regflag"] = "0";
                string Master1 = "select * from Master_Settings where " + ucode + "";
                ds = d2.select_method_wo_parameter(Master1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                    }
                }
                collegecode = ddlcollege.SelectedValue.ToString();
                //rbsem.Checked = true;
                rbstudent.Checked = true;
                loadstuorst();
                bindplace();
                BindRouteID();
                bindVehicleID();
                feeset();
            }
            errmsg.Visible = false;
        }
        catch
        {
        }
    }
    public void loadstuorst()
    {
        try
        {
            chkbatch.Checked = false;
            chkdegree.Checked = false;
            chkbranch.Checked = false;
            FpSpread1.Visible = false;
            Fieldset2.Visible = false;
            fromno.Text = "";
            tono.Text = "";
            collegecode = ddlcollege.SelectedValue.ToString();
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            btntransfer.Visible = false;
            if (rbstudent.Checked == true)
            {
                lblbatch.Text = "Batch";
                lbldegree.Text = lbldegree.Text;
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                lblbranch.Visible = true;
                txtbranch.Visible = true;
                pbranch.Visible = true;
                chkbatch.Checked = false;
                chkdegree.Checked = false;
                lblfeecat.Visible = false;
                fee_cate.Visible = false;
            }
            else if (rbstaff.Checked == true)
            {
                lblbatch.Text = "Department";
                lbldegree.Text = "Designation";
                BindDesignation();
                BindDepartment();
                lblbranch.Visible = false;
                txtbranch.Visible = false;
                pbranch.Visible = false;
                lblfeecat.Visible = true;
                fee_cate.Visible = true;
            }
        }
        catch
        {
        }
    }
    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                txtbatch.Text = "Batch (1)";
            }
            else
            {
                txtbatch.Text = "---Select---";
            }
        }
        catch
        {

        }
    }
    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            chklsdegree.Items.Clear();
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }

            ds.Dispose();
            ds.Reset();
            ds = d2.BindDegree(singleuser, group_user, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdegree.DataSource = ds;
                chklsdegree.DataTextField = "course_name";
                chklsdegree.DataValueField = "course_id";
                chklsdegree.DataBind();
                chklsdegree.Items[0].Selected = true;
                txtdegree.Text = lbldegree.Text + "(1)";
            }
            else
            {
                txtdegree.Text = "---Select---";
            }
        }
        catch
        {

        }
    }
    public void BindBranchMultiple(string singleuser, string group_user, string course_id, string collegecode, string usercode)
    {
        try
        {
            chklsbranch.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            course_id = "";
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                if (chklsdegree.Items[i].Selected == true)
                {
                    if (course_id == "")
                    {
                        course_id = "'" + chklsdegree.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        course_id = course_id + "," + "'" + chklsdegree.Items[i].Value.ToString() + "'";
                    }
                }
            }

            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsbranch.DataSource = ds;
                chklsbranch.DataTextField = "dept_name";
                chklsbranch.DataValueField = "degree_code";
                chklsbranch.DataBind();
                chklsbranch.Items[0].Selected = true;
                txtbranch.Text = lblbranch.Text+" (1)";
            }
            else
            {
                txtbranch.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    public void BindDepartment()
    {
        try
        {
            chklsbatch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            collegecode = ddlcollege.SelectedValue.ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);

            string strdeptquery = "";
            if (singleuser.ToLower().Trim() == "true")
            {
                strdeptquery = "select distinct d.Dept_Code,d.Dept_Name from hrdept_master d,hr_privilege p,stafftrans t where d.dept_code = p.dept_code and d.dept_code = t.dept_code and d.college_code = '" + collegecode + "' and t.stftype like 'Tea%' and p.user_code='" + usercode + "'";
            }
            else
            {
                strdeptquery = "select distinct d.Dept_Code,d.Dept_Name from hrdept_master d,hr_privilege p,stafftrans t where d.dept_code = p.dept_code and d.dept_code = t.dept_code and d.college_code = '" + collegecode + "' and t.stftype like 'Tea%' and p.group_code='" + group_user + "'";
            }
            ds.Dispose();
            ds.Reset();
            ds = d2.select_method(strdeptquery, hat, "Text");
            int count2 = ds.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                chklsbatch.DataSource = ds;
                chklsbatch.DataTextField = "dept_name";
                chklsbatch.DataValueField = "Dept_Code";
                chklsbatch.DataBind();
            }
            txtbatch.Text = "---Select---";
        }
        catch
        {
        }
    }
    public void BindDesignation()
    {
        try
        {
            chklsdegree.Items.Clear();
            collegecode = ddlcollege.SelectedValue.ToString();
            ds = d2.binddesi(collegecode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklsdegree.DataSource = ds;
                chklsdegree.DataValueField = "desig_code";
                chklsdegree.DataTextField = "desig_name";
                chklsdegree.DataBind();
            }
            txtdegree.Text = "---Select---";
        }
        catch
        {
        }
    }
    public void bindVehicleID()
    {
        try
        {
            ddlvechile.Items.Clear();
            string sql = "";
            if (ddlboarding.SelectedItem.ToString() != "All" && ddlroute.SelectedItem.ToString() != "All")
            {
                sql = "select distinct v.veh_id from vehicle_master v,routemaster r,Stage_Master s where s.stage_id=r.stage_name and v.veh_id=r.veh_id and s.stage_id='" + ddlboarding.SelectedValue.ToString() + "' and r.Route_ID='" + ddlroute.SelectedItem.ToString() + "'";
            }
            else if (ddlboarding.SelectedItem.ToString() != "All" && ddlroute.SelectedItem.ToString() == "All")
            {
                sql = "select distinct v.veh_id from vehicle_master v,routemaster r,Stage_Master s where s.stage_id=r.stage_name and v.veh_id=r.veh_id and s.stage_id='" + ddlboarding.SelectedValue.ToString() + "' ";
            }
            else if (ddlboarding.SelectedItem.ToString() == "All" && ddlroute.SelectedItem.ToString() != "All")
            {
                sql = "select distinct v.veh_id from vehicle_master v,routemaster r,Stage_Master s where s.stage_id=r.stage_name and v.veh_id=r.veh_id and r.Route_ID='" + ddlroute.SelectedItem.ToString() + "'";
            }
            else
            {
                sql = "select distinct Veh_ID from vehicle_master order by Veh_ID";
            }
            ds = d2.select_method_wo_parameter(sql, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlvechile.DataSource = ds;
                ddlvechile.DataValueField = "Veh_ID";
                ddlvechile.DataTextField = "Veh_ID";
                ddlvechile.DataBind();
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    ddlvechile.Items.Add(ds.Tables[0].Rows[i]["Veh_ID"].ToString());
                //}
                //ddlvechile.SelectedIndex = 0;
            }
        }
        catch
        {
        }
    }
    public void BindRouteID()
    {
        try
        {
            ddlroute.Items.Clear();
            string sql = "";
            if (ddlboarding.SelectedItem.ToString() != "All")
            {
                sql = "select distinct Route_ID from routemaster where stage_name='" + ddlboarding.SelectedValue.ToString() + "' order by Route_ID";
            }
            else
            {
                sql = "select distinct Route_ID from routemaster order by Route_ID";
            }
            ds = d2.select_method_wo_parameter(sql, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlroute.DataSource = ds;
                ddlroute.DataValueField = "Route_ID";
                ddlroute.DataTextField = "Route_ID";
                ddlroute.DataBind();
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    ddlroute.Items.Add(ds.Tables[0].Rows[i]["Route_ID"].ToString());
                //}
            }
        }
        catch
        {
        }
    }
    public void bindplace()
    {
        try
        {
            hat.Clear();
            ddlboarding.Items.Clear();
            string sql = "select Distinct s.Stage_Name,s.Stage_id from Stage_Master s,RouteMaster r,registration reg where cast(r.stage_name as varchar(100))=cast(s.stage_id as varchar(100))  and cc=0 and exam_flag<>'degar' and delflag=0";   //and cast(reg.boarding as varchar(100))=cast(s.stage_id as varchar(100))   modified by prabha on feb 19 2018
            ds = d2.select_method_wo_parameter(sql, "txt");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlboarding.DataSource = ds;
                ddlboarding.DataTextField = "stage_name";
                ddlboarding.DataValueField = "stage_id";
                ddlboarding.DataBind();

                chklsstrplace.DataSource = ds;
                chklsstrplace.DataTextField = "stage_name";
                chklsstrplace.DataValueField = "stage_id";
                chklsstrplace.DataBind();

                txtstrplace.Text = "---Select---";
            }
        }
        catch
        {
        }
    }
    protected void ddlboarding_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindRouteID();
        bindVehicleID();
    }
    protected void ddlroute_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindVehicleID();
    }
    protected void lb2_Click(object sender, EventArgs e) //Aruna For Back Button
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    protected void rbstudent_CheckedChange(object sender, EventArgs e)
    {
        loadstuorst();
    }
    protected void rbstaff_CheckedChange(object sender, EventArgs e)
    {
        loadstuorst();
    }
    protected void chklsbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        for (int i = 0; i < chklsbatch.Items.Count; i++)
        {
            if (chklsbatch.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }

        if (commcount == 0)
        {
            txtbatch.Text = "--Select--";
            chkbatch.Checked = false;
        }
        else if (commcount == chklsbatch.Items.Count)
        {
            if (rbstudent.Checked == true)
            {
                txtbatch.Text = "Batch (" + commcount + ")";
            }
            else
            {
                txtbatch.Text = "Department (" + commcount + ")";
            }
            chkbatch.Checked = true;
        }
        else
        {
            if (rbstudent.Checked == true)
            {
                txtbatch.Text = "Batch (" + commcount + ")";
            }
            else
            {
                txtbatch.Text = "Department (" + commcount + ")";
            }
            chkbatch.Checked = false;
        }
    }
    protected void chkbatch_ChekedChange(object sender, EventArgs e)
    {
        if (chkbatch.Checked == true)
        {
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                chklsbatch.Items[i].Selected = true;
            }
            if (rbstudent.Checked == true)
            {
                txtbatch.Text = "Batch (" + chklsbatch.Items.Count + ")";
            }
            else
            {
                txtbatch.Text = "Department (" + chklsbatch.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                chklsbatch.Items[i].Selected = false;
            }
            txtbatch.Text = "--Select--";
        }
    }
    protected void chkbranch_ChekedChange(object sender, EventArgs e)
    {
        if (chkbranch.Checked == true)
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = true;
            }
            if (rbstudent.Checked == true)
            {
                txtbranch.Text = lblbranch.Text+" (" + chklsbranch.Items.Count + ")";
            }
            else
            {
                txtbranch.Text = "Degisnation (" + chklsbranch.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklsbranch.Items.Count; i++)
            {
                chklsbranch.Items[i].Selected = false;
            }
            txtbranch.Text = "--Select--";
        }
    }
    protected void chklsbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        for (int i = 0; i < chklsbranch.Items.Count; i++)
        {
            if (chklsbranch.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }

        if (commcount == 0)
        {
            txtbranch.Text = "--Select--";
            chkbranch.Checked = false;
        }
        else if (commcount == chklsbranch.Items.Count)
        {
            if (rbstudent.Checked == true)
            {
                txtbranch.Text = lblbranch.Text+" (" + commcount + ")";
            }
            chkbranch.Checked = true;
        }
        else
        {
            if (rbstudent.Checked == true)
            {
                txtbranch.Text = lblbranch.Text + " (" + commcount + ")";
            }
            chkbranch.Checked = false;
        }
    }
    protected void chkdegree_ChekedChange(object sender, EventArgs e)
    {
        if (chkdegree.Checked == true)
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = true;
            }
            if (rbstudent.Checked == true)
            {
                txtdegree.Text = "Degree (" + chklsdegree.Items.Count + ")";
            }
            else
            {
                txtdegree.Text = "Degisnation (" + chklsdegree.Items.Count + ")";
            }
        }
        else
        {
            for (int i = 0; i < chklsdegree.Items.Count; i++)
            {
                chklsdegree.Items[i].Selected = false;
            }
            txtdegree.Text = "--Select--";
        }
        if (rbstudent.Checked == true)
        {
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }
    protected void chklsdegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        for (int i = 0; i < chklsdegree.Items.Count; i++)
        {
            if (chklsdegree.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }

        if (commcount == 0)
        {
            txtdegree.Text = "--Select--";
            chkdegree.Checked = false;
        }
        else if (commcount == chklsdegree.Items.Count)
        {
            if (rbstudent.Checked == true)
            {
                txtdegree.Text = "Degree (" + commcount + ")";
            }
            else
            {
                txtdegree.Text = "Degisnation (" + commcount + ")";
            }
            chkdegree.Checked = true;
        }
        else
        {
            if (rbstudent.Checked == true)
            {
                txtdegree.Text = "Degree (" + commcount + ")";
            }
            else
            {
                txtdegree.Text = "Degisnation (" + commcount + ")";
            }
            chkdegree.Checked = false;
        }
        if (rbstudent.Checked == true)
        {
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
        }
    }
    protected void chklsstrplace_SelectedIndexChanged(object sender, EventArgs e)
    {
        int commcount = 0;
        for (int i = 0; i < chklsstrplace.Items.Count; i++)
        {
            if (chklsstrplace.Items[i].Selected == true)
            {
                commcount = commcount + 1;
            }
        }

        if (commcount == 0)
        {
            txtstrplace.Text = "--Select--";
            chkstrplace.Checked = false;
        }
        else if (commcount == chklsstrplace.Items.Count)
        {
            txtstrplace.Text = "Boarding (" + commcount + ")";
            chkstrplace.Checked = true;
        }
        else
        {

            txtstrplace.Text = "Boarding (" + commcount + ")";
            chkstrplace.Checked = false;
        }
    }
    protected void chkstrplace_ChekedChange(object sender, EventArgs e)
    {
        if (chkstrplace.Checked == true)
        {
            for (int i = 0; i < chklsstrplace.Items.Count; i++)
            {
                chklsstrplace.Items[i].Selected = true;
            }
            txtstrplace.Text = "Boarding (" + chklsstrplace.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < chklsstrplace.Items.Count; i++)
            {
                chklsstrplace.Items[i].Selected = false;
            }
            txtstrplace.Text = "--Select--";
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            loadstuorst();
        }
        catch
        {
        }
    }
    protected void selectgo_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            int frra = 0, tora = 0;
            if (fromno.Text.Trim().ToString() != "" && fromno.Text.Trim().ToString() != null && fromno.Text.Trim().ToString() != "0")
            {
                if (tono.Text.Trim().ToString() != "" && tono.Text.Trim().ToString() != null && tono.Text.Trim().ToString() != "0")
                {
                    frra = Convert.ToInt32(fromno.Text.ToString());
                    tora = Convert.ToInt32(tono.Text.ToString());

                    if (frra <= tora)
                    {
                        if (frra <= FpSpread1.Sheets[0].RowCount)
                        {
                            if (tora <= FpSpread1.Sheets[0].RowCount)
                            {
                                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                                {
                                    if (frra - 1 <= i && tora - 1 >= i)
                                    {
                                        FpSpread1.Sheets[0].Cells[i, 5].Value = 1;
                                    }
                                    else
                                    {
                                        FpSpread1.Sheets[0].Cells[i, 5].Value = 0;
                                    }

                                }
                                fromno.Text = "";
                                tono.Text = "";
                            }
                            else
                            {
                                errmsg.Visible = true;
                                errmsg.Text = "Please Enter To Range Must Below Student Count";
                            }
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Please Enter From Range Must Below Student Count";
                        }
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please Enter From Range Must Be Smaller Than To Range";
                    }
                }
                else
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please Enter To Range";
                }
            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "Please Enter From Range";
            }
            FpSpread1.SaveChanges();
        }
        catch
        {
        }
    }
    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
            }
            else
            {
                errmsg.Text = "Please Enter Your Report Name";
                errmsg.Visible = true;
            }
        }
        catch
        {
        }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        Printcontrol.loadspreaddetails(FpSpread1, "Routetransfer.aspx", "Route Transfer");
        Printcontrol.Visible = true;
    }

    public void feeset()
    {

        string strquery = d2.GetFunction("select linkvalue from New_InsSettings where college_code=" + collegecode + " and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
        if (strquery == "1")
        {
            strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel'  and right(TextVal,4) ='Year' order by textval";
        }
        else
        {
            strquery = "SELECT * FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code=" + collegecode + " and textval <> 'Hostel' and right(TextVal,4) <>'Year' order by textval";
        }
        ds.Reset();
        ds.Dispose();
        ds = d2.select_method_wo_parameter(strquery, "Text");
        fee_cate.Items.Clear();
        if (ds.Tables[0].Rows.Count > 0)
        {

            fee_cate.DataSource = ds;
            fee_cate.DataTextField = "Textval";
            fee_cate.DataValueField = "textCode";
            fee_cate.DataBind();

        }

    }
    #endregion

    protected void btngo_Click(object sender, EventArgs e)
    {
        try
        {
            fromno.Text = "";
            tono.Text = "";
            collegecode = ddlcollege.SelectedValue.ToString();
            btnprintmaster.Visible = false;
            Printcontrol.Visible = false;
            lblrptname.Visible = false;
            txtexcelname.Visible = false;
            btnxl.Visible = false;
            FarPoint.Web.Spread.CheckBoxCellType chk = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
            FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;

            FpSpread1.Sheets[0].ColumnCount = 6;
            FpSpread1.Sheets[0].RowCount = 0;
            FpSpread1.Sheets[0].ColumnHeader.Rows[0].BackColor = Color.AliceBlue;


            FpSpread1.Sheets[0].Columns[0].Width = 80;
            FpSpread1.Sheets[0].Columns[1].Width = 150;
            FpSpread1.Sheets[0].Columns[2].Width = 150;
            FpSpread1.Sheets[0].Columns[3].Width = 220;
            FpSpread1.Sheets[0].Columns[4].Width = 220;
            FpSpread1.Sheets[0].Columns[5].Width = 64;

            FpSpread1.Sheets[0].Columns[0].CellType = txt;
            FpSpread1.Sheets[0].Columns[1].CellType = txt;
            FpSpread1.Sheets[0].Columns[2].CellType = txt;
            FpSpread1.Sheets[0].Columns[3].CellType = txt;
            FpSpread1.Sheets[0].Columns[4].CellType = txt;

            FpSpread1.Sheets[0].Columns[0].Locked = true;
            FpSpread1.Sheets[0].Columns[1].Locked = true;
            FpSpread1.Sheets[0].Columns[2].Locked = true;
            FpSpread1.Sheets[0].Columns[3].Locked = true;
            FpSpread1.Sheets[0].Columns[4].Locked = true;

            FpSpread1.Sheets[0].Columns[0].Visible = true;
            FpSpread1.Sheets[0].Columns[1].Visible = true;
            FpSpread1.Sheets[0].Columns[2].Visible = true;
            FpSpread1.Sheets[0].Columns[3].Visible = true;
            FpSpread1.Sheets[0].Columns[4].Visible = true;

            FpSpread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpSpread1.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Center;

            string strbatch = "", strbranch = "", strquery = "";
            for (int i = 0; i < chklsbatch.Items.Count; i++)
            {
                if (chklsbatch.Items[i].Selected == true)
                {
                    if (strbatch == "")
                    {
                        strbatch = "'" + chklsbatch.Items[i].Value + "'";
                    }
                    else
                    {
                        strbatch = strbatch + ",'" + chklsbatch.Items[i].Value + "'";
                    }
                }
            }


            string strboadring = "";
            for (int i = 0; i < chklsstrplace.Items.Count; i++)
            {
                if (chklsstrplace.Items[i].Selected == true)
                {
                    if (strboadring == "")
                    {
                        strboadring = "'" + chklsstrplace.Items[i].Value + "'";
                    }
                    else
                    {
                        strboadring = strboadring + ",'" + chklsstrplace.Items[i].Value + "'";
                    }
                }
            }



            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Name";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Boarding";
            FpSpread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Select";

            if (rbstudent.Checked == true)
            {
                if (strboadring != "")
                {
                    strboadring = " and r.boarding in (" + strboadring + ")";
                }

                string strorder = "ORDER BY roll_no";
                string orderby_Setting = d2.GetFunction("select LinkValue from inssettings where college_code=" + Session["collegecode"].ToString() + " and linkname='Student Attendance'");
                if (orderby_Setting == "1")
                {
                    strorder = "ORDER BY serialno";
                }
                else
                {
                    orderby_Setting = d2.GetFunction("select value from master_Settings where settings='order_by'");
                    if (orderby_Setting == "1")
                    {

                        strorder = "ORDER BY Reg_No";
                    }
                    else if (orderby_Setting == "2")
                    {
                        strorder = "ORDER BY Stud_Name";
                    }
                    else if (orderby_Setting == "0,1,2")
                    {
                        strorder = "ORDER BY roll_no,Reg_No,Stud_Name";
                    }
                    else if (orderby_Setting == "0,1")
                    {
                        strorder = "ORDER BY roll_no,Reg_No";
                    }
                    else if (orderby_Setting == "1,2")
                    {
                        strorder = "ORDER BY Reg_No,Stud_Name";
                    }
                    else if (orderby_Setting == "0,2")
                    {
                        strorder = "ORDER BY roll_no,Stud_Name";
                    }
                }
                if (Session["Rollflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].Columns[1].Visible = false;
                    FpSpread1.Sheets[0].Columns[3].Width = 370;

                }
                if (Session["Regflag"].ToString() == "0")
                {
                    FpSpread1.Sheets[0].Columns[2].Visible = false;
                    FpSpread1.Sheets[0].Columns[4].Width = 370;
                }


                for (int i = 0; i < chklsbranch.Items.Count; i++)
                {
                    if (chklsbranch.Items[i].Selected == true)
                    {
                        if (strbranch == "")
                        {
                            strbranch = "'" + chklsbranch.Items[i].Value + "'";
                        }
                        else
                        {
                            strbranch = strbranch + ",'" + chklsbranch.Items[i].Value + "'";
                        }
                    }
                }

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";

                if (strbatch != "")
                {
                    strbatch = "and batch_year in(" + strbatch + ")";
                }
                if (strbranch != "")
                {
                    strbranch = "and degree_code in(" + strbranch + ")";
                }
                strquery = "select roll_no as code,reg_no as type,stud_name as name,boarding as bcode,serialno,s.stage_name as boarding,r.roll_admit as admitno  from registration r,stage_master s where cast(r.boarding as varchar(100))=cast(s.stage_id as varchar(100)) and cc=0 and delflag=0 and exam_flag<>'debar' and boarding!='' and boarding is not null and college_code='" + collegecode + "' " + strbatch + " " + strbranch + " " + strboadring + " " + strorder + "";

            }
            else
            {
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Type";
                strbranch = "";
                for (int i = 0; i < chklsdegree.Items.Count; i++)
                {
                    if (chklsdegree.Items[i].Selected == true)
                    {
                        if (strbranch == "")
                        {
                            strbranch = "'" + chklsdegree.Items[i].Value + "'";
                        }
                        else
                        {
                            strbranch = strbranch + ",'" + chklsdegree.Items[i].Value + "'";
                        }
                    }
                }

                if (strbatch != "")
                {
                    strbatch = "and st.dept_code in (" + strbatch + ")";
                }
                if (strbranch != "")
                {
                    strbranch = "and st.desig_code in(" + strbranch + ")";
                }
                strquery = "select sm.staff_name as name,sm.staff_code as code,st.stftype as type,boarding as bcode,s.stage_name as boarding from staffmaster sm,stafftrans st,stage_master s where sm.staff_code=st.staff_code and cast(s.Stage_id as varchar(100))=cast(sm.Boarding as varchar(100)) and sm.college_code='" + collegecode + "' and latestrec = 1 and resign = 0 and settled = 0 and  boarding!='' and boarding is not null " + strbatch + " " + strbranch + " order by st.dept_code,st.desig_code,sm.staff_name";
            }

            ds.Dispose();
            ds.Reset();
            ds = d2.select_method_wo_parameter(strquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                Fieldset2.Visible = true;
                btnprintmaster.Visible = true;
                Printcontrol.Visible = false;
                lblrptname.Visible = true;
                txtexcelname.Visible = true;
                btnxl.Visible = true;
                btntransfer.Visible = true;
                int srno = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = ds.Tables[0].Rows[i]["name"].ToString();
                    string roll = ds.Tables[0].Rows[i]["code"].ToString();
                    string reg = ds.Tables[0].Rows[i]["type"].ToString();
                    string boarding = ds.Tables[0].Rows[i]["Boarding"].ToString();
                    string bcode = ds.Tables[0].Rows[i]["bcode"].ToString();

                    FpSpread1.Sheets[0].RowCount++;
                    srno++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = srno.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = roll.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = reg.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = name.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = boarding.ToString();
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Note = bcode;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = chk;

                }

            }
            else
            {
                errmsg.Visible = true;
                errmsg.Text = "No Records Found";
                FpSpread1.Visible = false;
                Fieldset2.Visible = false;
                btnprintmaster.Visible = false;
                Printcontrol.Visible = false;
                lblrptname.Visible = false;
                txtexcelname.Visible = false;
                btnxl.Visible = false;
                btntransfer.Visible = false;
            }
            FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
        }
        catch (Exception ex)
        {
        }
    }
    protected void btntransfer_Click(object sender, EventArgs e)
    {
        try
        {
            collegecode = ddlcollege.SelectedValue.ToString();
            FpSpread1.SaveChanges();
            Boolean saveflag = false;
            string rights = d2.GetFunction("select LinkValue from inssettings where linkname = 'Transport Link' and college_code ='" + collegecode + "'");
            if (rbstudent.Checked == true && rbstaff.Checked == false)
            {
                string strsemval1 = "SELECT textcode,textval FROM TEXTVALTABLE WHERE textcriteria='FEECA' and college_code='" + collegecode + "' and textval <> 'Hostel' order by textval";
                ds.Dispose(); ds.Reset();
                ds = d2.select_method_wo_parameter(strsemval1, "Text");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string textval = ds.Tables[0].Rows[i]["textval"].ToString().Trim().ToLower();
                    if (!hat.Contains(textval))
                    {
                        string textcode = ds.Tables[0].Rows[i]["textcode"].ToString();
                        hat.Add(textval, textcode);
                    }
                }

                int insupdate = 0;

                //string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                //string strtype = "";
                //if (rbsem.Checked == true)
                //{
                //    strtype = "Semester";
                //}
                //else if (rbyear.Checked == true)
                //{
                //    strtype = "Yearly";
                //}
                //else
                //{
                //    strtype = "Monthly";
                //}
                string strtype = ddltype.SelectedItem.ToString();
                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 5].Value);
                    if (isval == 1)
                    {
                        string Roll = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                        string header_id = "", tcode = "", Fee_Code = "", strsemval = "", Cost = "", type = "", category = "", Roll_Adm = "", BatchFee = "";
                        string squry = ddlboarding.SelectedValue.ToString();
                        string stdname = FpSpread1.Sheets[0].Cells[i, 3].Text.ToString();
                        string strquery = "Select Roll_admit,Current_semester,batch_year from registration where roll_no='" + Roll + "'";
                        ds.Dispose(); ds.Reset();
                        ds = d2.select_method_wo_parameter(strquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Roll_Adm = ds.Tables[0].Rows[0]["Roll_admit"].ToString();
                            int sem = Convert.ToInt32(ds.Tables[0].Rows[0]["Current_semester"].ToString());
                            BatchFee = ds.Tables[0].Rows[0]["batch_year"].ToString();
                            if (strtype == "Semester")
                            {
                                strsemval = sem + " " + strtype;
                                if (sem % 2 == 0)
                                {
                                    category = "Even";
                                }
                                else
                                {
                                    category = "Odd";
                                }
                            }
                            else if (strtype == "Yearly")
                            {
                                if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "1" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "2")
                                {
                                    strsemval = "1 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "3" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "4")
                                {
                                    strsemval = "2 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "5" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "6")
                                {
                                    strsemval = "3 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "7" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "8")
                                {
                                    strsemval = "4 Year";
                                }
                            }
                            else
                            {
                                string month = DateTime.Now.Month.ToString();
                                string month_amt = month + ";" + Cost;
                                if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "1" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "2")
                                {
                                    strsemval = "1 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "3" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "4")
                                {
                                    strsemval = "2 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "5" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "6")
                                {
                                    strsemval = "3 Year";
                                }
                                else if (ds.Tables[0].Rows[0]["current_semester"].ToString() == "7" || ds.Tables[0].Rows[0]["current_semester"].ToString() == "8")
                                {
                                    strsemval = "4 Year";
                                }
                            }

                            if (hat.Contains(strsemval.ToString().Trim().ToLower()))
                            {
                                tcode = hat[strsemval.ToString().Trim().ToLower()].ToString();
                            }
                            else
                            {
                                tcode = "";
                            }
                        }

                        string selectquery = "select cost,Fee_Code from FeeInfo  where StrtPlace = '" + squry + "' and payType = '" + strtype + "' and category='" + category + "'";
                        DataSet dsselectquery = d2.select_method_wo_parameter(selectquery, "Text");
                        for (int i1 = 0; i1 < dsselectquery.Tables[0].Rows.Count; i1++)
                        {
                            Fee_Code = dsselectquery.Tables[0].Rows[i1]["Fee_Code"].ToString();
                            Cost = dsselectquery.Tables[0].Rows[i1]["Cost"].ToString();
                        }

                        if (Fee_Code.Trim() != "" && Fee_Code != null)
                        {
                            if (rights == "1")
                            {
                                header_id = d2.GetFunction("select distinct header_id from fee_info where fee_code = '" + Fee_Code + "'");
                                try
                                {
                                    Boolean allotflag = false;
                                    double Already_allot_amt = 0;
                                    double Already_allot_tot_amt = 0;
                                    double Aleady_allot_duduct_amt = 0;
                                    double paid_amout = 0;
                                    double Already_paid_excess_amount = 0;

                                    int insert = 0;
                                    string queryUpdate1 = "select * from fee_allot where roll_admit='" + Roll_Adm + "' and fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "'";
                                    DataSet dtnewupdate = d2.select_method_wo_parameter(queryUpdate1, "text");
                                    if (dtnewupdate.Tables[0].Rows.Count > 0)
                                    {
                                        Already_allot_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["fee_amount"]);
                                        Already_allot_tot_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["total"]);
                                        Aleady_allot_duduct_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["deduct"]);
                                        string name = Roll + "-" + stdname;
                                        string paidamt = d2.GetFunction("select isnull(sum(credit),0) as paid from dailytransaction where fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "' and name='" + name + "' and debit=0 and studorothers=1 and vouchertype=1");
                                        paid_amout = Convert.ToDouble(paidamt);
                                        if (paid_amout == 0)
                                        {
                                            string querystu1;
                                            double allot_amt = Convert.ToDouble(Cost);
                                            double allot_tot = 0;
                                            Boolean flag_status = false;
                                            if (allot_amt >= Aleady_allot_duduct_amt)
                                            {
                                                allot_tot = allot_amt - Aleady_allot_duduct_amt;
                                            }
                                            else
                                            {
                                                allot_tot = 0;
                                            }
                                            if (paid_amout > 0)
                                            {
                                                if (allot_tot == paid_amout)
                                                {
                                                    flag_status = true;
                                                }
                                                else if (paid_amout >= allot_tot)
                                                {
                                                    flag_status = true;
                                                    Already_paid_excess_amount = paid_amout - allot_tot;
                                                }
                                                else
                                                {
                                                    flag_status = false;
                                                }
                                            }
                                            else
                                            {
                                                flag_status = false;
                                            }

                                            querystu1 = "update fee_allot set fee_amount=" + allot_amt + ",deduct=" + Aleady_allot_duduct_amt + ",total=" + allot_tot + ",flag_status='" + flag_status + "' where roll_admit='" + Roll_Adm + "' and fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "'";
                                            insert = d2.update_method_wo_parameter(querystu1, "Text");
                                            flag_status = false;
                                            queryUpdate1 = "select * from fee_status where roll_admit='" + Roll_Adm + "' and header_id='" + header_id + "' and fee_category='" + tcode + "'";

                                            DataSet dtnewupdate1 = d2.select_method_wo_parameter(queryUpdate1, "Text");
                                            if (dtnewupdate1.Tables[0].Rows.Count > 0)
                                            {
                                                string queryup;
                                                double total_alloted = 0;
                                                double total_paid = 0;
                                                string totalalloted = d2.GetFunction("select isnull(sum(total),0) as fee from fee_allot f,fee_info fi where f.fee_code=fi.fee_code and roll_admit='" + Roll_Adm + "' and fee_category='" + tcode + "' and f.fee_code = '" + Fee_Code + "' and header_id='" + header_id + "'");
                                                total_alloted = Convert.ToDouble(totalalloted);
                                                total_paid = Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["amount_paid"]);
                                                if (total_paid >= total_alloted)
                                                {
                                                    flag_status = true;
                                                }
                                                else
                                                {
                                                    flag_status = false;
                                                }

                                                queryup = "update fee_status set amount = '" + total_alloted + "' ,balance = '" + (total_alloted - total_paid) + "',flag_status='" + flag_status + "' where roll_admit ='" + Roll_Adm + "' and fee_category = '" + tcode + "' and header_id = '" + header_id + "'";
                                                insert = d2.update_method_wo_parameter(queryup, "Text");

                                            }
                                        }
                                    }
                                    else if (dtnewupdate.Tables[0].Rows.Count == 0)
                                    {

                                        string querystu1;
                                        querystu1 = "insert into fee_allot(roll_admit,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,batch,refound_amt,semyearflg,seatcate,modeofpay,app_formno,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt)values('" + Roll_Adm + "','" + Fee_Code + "','','false','" + Cost + "','" + tcode + "','',0,0,0,'" + Cost + "','N',0,'',0,'',0,'',0,'',0,1,'" + BatchFee + "',0,0,0,'Regular','','','','','','','','')";
                                        insert = d2.update_method_wo_parameter(querystu1, "Text");
                                        allotflag = true;

                                        queryUpdate1 = "select * from fee_status where roll_admit='" + Roll_Adm + "' and header_id='" + header_id + "' and fee_category='" + tcode + "'";
                                        DataSet dtnewupdate1 = d2.select_method_wo_parameter(queryUpdate1, "Text");
                                        if (dtnewupdate1.Tables[0].Rows.Count > 0 && allotflag == true)
                                        {
                                            string queryup;
                                            queryup = "update fee_status set amount = '" + Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["amount"]) + Convert.ToDouble(Cost) + "' ,balance = '" + Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["balance"]) + Convert.ToDouble(Cost) + "',flag_status='false' where roll_admit ='" + Roll_Adm + "' and fee_category = '" + tcode + "' and header_id = '" + header_id + "'";
                                            insert = d2.update_method_wo_parameter(queryup, "Text");
                                        }
                                        else if (dtnewupdate1.Tables[0].Rows.Count == 0 && allotflag == true)
                                        {
                                            string queryins;
                                            queryins = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno)values('" + Roll_Adm + "','" + Cost + "',0,'" + Cost + "','false','" + tcode + "','" + header_id + "',0,'')";
                                            insert = d2.update_method_wo_parameter(queryins, "Text");
                                        }
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        strquery = "update registration set boarding='" + squry + "' ,bus_routeid='" + ddlroute.SelectedValue.ToString() + "',vehid='" + ddlvechile.SelectedValue.ToString() + "' where roll_no='" + Roll + "'";
                        insupdate = d2.update_method_wo_parameter(strquery, "Text");
                        saveflag = true;
                    }
                }
            }
            else if (rbstaff.Checked == true && rbstudent.Checked == false)
            {
                string type = ddltype.SelectedItem.ToString();
                // string strtype = d2.GetFunction("select LinkValue from New_InsSettings where college_code='" + collegecode + "' and LinkName='Fee Yearwise' And user_code = '" + usercode + "'");
                //if (strtype == "1")
                //{
                //    type = "Yearly";
                //}
                //else
                //{
                //    type = "Semester";
                //}
                //if (rbsem.Checked == true)
                //{
                //    type = "Semester";
                //}
                //else if (rbyear.Checked == true)
                //{
                //    type = "Yearly";
                //}
                //else
                //{
                //    type = "Monthly";
                //}

                for (int i = 0; i < FpSpread1.Sheets[0].RowCount; i++)
                {
                    int isval = 0;
                    isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 5].Value);
                    if (isval == 1)
                    {
                        saveflag = true;
                        string scode = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                        string sname = FpSpread1.Sheets[0].Cells[i, 3].Text.ToString();
                        string Roll = FpSpread1.Sheets[0].Cells[i, 1].Text.ToString();
                        string header_id = "", tcode = "", Fee_Code = "", Cost = "", category = "";
                        string squry = ddlboarding.SelectedValue.ToString();
                        int insert = 0;
                        double Already_allot_amt = 0;
                        double Already_allot_tot_amt = 0;
                        double Aleady_allot_duduct_amt = 0;
                        double paid_amout = 0;
                        double Already_paid_excess_amount = 0;

                        tcode = fee_cate.SelectedValue.ToString();
                        string semval = fee_cate.SelectedItem.ToString();
                        string querystu1 = "";


                        //Insert Fees For Staff===============================================================================================
                        if (rights == "1")
                        {
                            if (type == "Semester")
                            {
                                string[] spl_sem = semval.Split(' ');
                                string curr_sem = spl_sem[0].ToString();

                                if (curr_sem != "")
                                {
                                    int num = 0;
                                    if (int.TryParse(curr_sem, out num))
                                    {
                                        if (Convert.ToInt32(curr_sem) % 2 == 0)
                                        {
                                            category = "Even";
                                        }
                                        else
                                        {
                                            category = "Odd";
                                        }
                                    }
                                }
                            }

                            squry = ddlboarding.SelectedValue.ToString();
                            if (squry.Trim() != "" && squry.Trim() != "0" && squry.Trim() != null)
                            {
                                string selectquery = "select f.cost,f.Fee_Code from FeeInfo f where f.StrtPlace = '" + squry + "' and f.payType = '" + type + "' and f.category='" + category + "'";
                                DataSet dsselectquery = d2.select_method_wo_parameter(selectquery, "Text");
                                for (int i1 = 0; i1 < dsselectquery.Tables[0].Rows.Count; i1++)
                                {
                                    Fee_Code = dsselectquery.Tables[0].Rows[i1]["Fee_Code"].ToString();
                                    Cost = dsselectquery.Tables[0].Rows[i1]["Cost"].ToString();
                                }
                            }

                            if (Fee_Code != "")
                            {
                                string queryUpdate1 = "select * from fee_allot where roll_admit='" + scode + "' and fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "'";
                                DataSet dtnewupdate = d2.select_method_wo_parameter(queryUpdate1, "Text");
                                Boolean allotflag = false;

                                header_id = d2.GetFunction("select distinct header_id from fee_info where fee_code = '" + Fee_Code + "'");

                                if (dtnewupdate.Tables[0].Rows.Count > 0)
                                {
                                    Already_allot_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["fee_amount"]);
                                    Already_allot_tot_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["total"]);
                                    Aleady_allot_duduct_amt = Convert.ToDouble(dtnewupdate.Tables[0].Rows[0]["deduct"]);
                                    string name = scode + "-" + sname;
                                    string paidamt = d2.GetFunction("select isnull(sum(credit),0) as paid from dailytransaction where fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "' and name='" + name + "' and debit=0 and studorothers=0 and vouchertype=1");
                                    paid_amout = Convert.ToDouble(paidamt);
                                    if (paid_amout == 0)
                                    {
                                        double allot_amt = Convert.ToDouble(Cost);
                                        double allot_tot = 0;
                                        Boolean flag_status = false;
                                        if (allot_amt >= Aleady_allot_duduct_amt)
                                        {
                                            allot_tot = allot_amt - Aleady_allot_duduct_amt;
                                        }
                                        else
                                        {
                                            allot_tot = 0;
                                        }
                                        if (paid_amout > 0)
                                        {
                                            if (allot_tot == paid_amout)
                                            {
                                                flag_status = true;
                                            }
                                            else if (paid_amout >= allot_tot)
                                            {
                                                flag_status = true;
                                                Already_paid_excess_amount = paid_amout - allot_tot;
                                            }
                                            else
                                            {
                                                flag_status = false;
                                            }
                                        }
                                        else
                                        {
                                            flag_status = false;
                                        }

                                        querystu1 = "update fee_allot set fee_amount=" + allot_amt + ",deduct=" + Aleady_allot_duduct_amt + ",total=" + allot_tot + ",flag_status='" + flag_status + "' where roll_admit='" + scode + "' and fee_category='" + tcode + "' and fee_code = '" + Fee_Code + "'";
                                        insert = d2.update_method_wo_parameter(querystu1, "Text");

                                        flag_status = false;
                                        queryUpdate1 = "select * from fee_status where roll_admit='" + scode + "' and header_id='" + header_id + "' and fee_category='" + tcode + "'";
                                        DataSet dtnewupdate1 = d2.select_method_wo_parameter(queryUpdate1, "Text");
                                        if (dtnewupdate1.Tables[0].Rows.Count > 0)
                                        {
                                            string queryup;
                                            double total_alloted = 0;
                                            double total_paid = 0;
                                            string totalalloted = d2.GetFunction("select isnull(sum(total),0) as fee from fee_allot f,fee_info fi where f.fee_code=fi.fee_code and roll_admit='" + scode + "' and fee_category='" + tcode + "' and f.fee_code = '" + Fee_Code + "' and header_id='" + header_id + "'");
                                            total_alloted = Convert.ToDouble(totalalloted);

                                            total_paid = Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["amount_paid"]);
                                            if (total_paid >= total_alloted)
                                            {
                                                flag_status = true;
                                            }
                                            else
                                            {
                                                flag_status = false;
                                            }

                                            queryup = "update fee_status set amount = '" + total_alloted + "' ,balance = '" + Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["balance"]) + Convert.ToDouble(allot_tot) + "',flag_status='" + flag_status + "' where roll_admit ='" + scode + "' and fee_category = '" + tcode + "' and header_id = '" + header_id + "'";
                                            insert = d2.update_method_wo_parameter(queryup, "Text");
                                        }
                                    }
                                }
                                else if (dtnewupdate.Tables[0].Rows.Count == 0)
                                {
                                    querystu1 = "insert into fee_allot(roll_admit,fee_code,allotdate,flag_status,fee_amount,fee_category,DueDate,permittedby,fine,deduct,total,intvallot,AdmisFees,DueExtDate1,fine1,DueExtDate2,fine2,DueExtDate3,fine3,DueExtDate4,fine4,Allot_Flg,refound_amt,semyearflg,seatcate,modeofpay,app_formno,dedect_reason,reason_fine,reason_fine1,reason_fine2,reason_fine3,Deduction_date,govetamt)values('" + scode + "','" + Fee_Code + "','','false','" + Cost + "','" + tcode + "','',0,0,0,'" + Cost + "','N',0,'',0,'',0,'',0,'',0,1,0,0,0,'Regular','','','','','','','','')";
                                    insert = d2.update_method_wo_parameter(querystu1, "Text");
                                    allotflag = true;

                                    queryUpdate1 = "select * from fee_status where roll_admit='" + scode + "' and header_id='" + header_id + "' and fee_category='" + tcode + "'";
                                    DataSet dtnewupdate1 = d2.select_method_wo_parameter(queryUpdate1, "Text");
                                    if (dtnewupdate1.Tables[0].Rows.Count > 0 && allotflag == true)
                                    {
                                        string queryup = "update fee_status set amount = '" + Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["amount"]) + Convert.ToDouble(Cost) + "' ,balance = '" + Convert.ToDouble(dtnewupdate1.Tables[0].Rows[0]["balance"]) + Convert.ToDouble(Cost) + "',flag_status='false' where roll_admit ='" + scode + "' and fee_category = '" + tcode + "' and header_id = '" + header_id + "'";
                                        insert = d2.update_method_wo_parameter(queryup, "Text");
                                    }
                                    else if (dtnewupdate1.Tables[0].Rows.Count == 0 && allotflag == true)
                                    {
                                        string queryins = "insert into fee_status(roll_admit,amount,amount_paid,balance,flag_status,fee_category,header_id,refound,app_formno)values('" + scode + "','" + Cost + "',0,'" + Cost + "','false','" + tcode + "','" + header_id + "',0,'')";
                                        insert = d2.update_method_wo_parameter(queryins, "Text");
                                    }
                                }
                            }
                        }
                        querystu1 = "update staffmaster set Bus_RouteID='" + ddlroute.SelectedValue.ToString() + "',Boarding='" + ddlboarding.SelectedValue.ToString() + "',VehID='" + ddlvechile.SelectedValue.ToString() + "' where staff_code='" + scode + "' and staff_name='" + sname + "'";
                        insert = d2.update_method_wo_parameter(querystu1, "Text");
                    }
                }
            }
            if (saveflag == true)
            {
                btngo_Click(sender, e);
                imgAlert.Visible = true;
                lbl_alert.Text = "Saved Sucessfully";
                //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
            }
            else
            {
                errmsg.Visible = true;
                if (rbstudent.Checked == true && rbstaff.Checked == false)
                {
                    errmsg.Text = "Please Select Student(s) and Proceed";
                }
                else if (rbstudent.Checked == false && rbstaff.Checked == true)
                {
                    errmsg.Text = "Please Select Staff(s) and Proceed";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_alertclose_Click(object sender, EventArgs e)
    {
        
            imgAlert.Visible = false;
        
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
        lbl.Add(lblcollege);
        //lbl.Add(lbl_stream);
        //lbl.Add(lbl_course);
        lbl.Add(lbldegree);
        lbl.Add(lblbranch);
        fields.Add(0);
        // fields.Add(1);
        //fields.Add(2);
        fields.Add(2);
        fields.Add(3);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 22-10-2016 sudhagar
}