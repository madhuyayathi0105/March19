using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class CAM_Calculation_Lock : System.Web.UI.Page
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
    DataTable dt = new DataTable();
    DataRow dr;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }
            if (!Request.FilePath.Contains("CAMHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MarkMod/CAMHome.aspx");
                    return;
                }
            }  
            usercode = Session["usercode"].ToString();
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                Bindcollege();
                collegecode = ddlcollege.SelectedValue.ToString();
                BindBatch();
                BindDegree(singleuser, group_user, collegecode, usercode);
                BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
                FarPoint.Web.Spread.StyleInfo MyStyle = new FarPoint.Web.Spread.StyleInfo();
                MyStyle.Font.Bold = true;
                MyStyle.Font.Size = FontUnit.Medium;
                MyStyle.HorizontalAlign = HorizontalAlign.Center;
                MyStyle.ForeColor = Color.Black;
                MyStyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            }
        }
        catch (Exception ex) { }
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
            }
            else
            {
                errmsg.Text = "Set college rights to the staff";
                errmsg.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
                chklsbatch.DataSource = ds2;
                chklsbatch.DataTextField = "Batch_year";
                chklsbatch.DataValueField = "Batch_year";
                chklsbatch.DataBind();
                chklsbatch.SelectedIndex = chklsbatch.Items.Count - 1;
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        count += 1;
                    }
                    if (chklsbatch.Items.Count == count)
                    {
                        chkbatch.Checked = true;
                    }
                }
                if (chkbatch.Checked == true)
                {
                    for (int i = 0; i < chklsbatch.Items.Count; i++)
                    {
                        chklsbatch.Items[i].Selected = true;
                        txtbatch.Text = "Batch(" + (chklsbatch.Items.Count) + ")";
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
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    public void BindDegree(string singleuser, string group_user, string collegecode, string usercode)
    {
        try
        {

            errmsg.Visible = false;
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
            if (ds2.Tables[0].Rows.Count > 0)
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
            }
            else
            {
                txtdegree.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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
                ds2 = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
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
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
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

        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void chklstbranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            errmsg.Visible = false;
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


        }
        catch (Exception ex)
        {
            errmsg.Text = ex.ToString();
            errmsg.Visible = true;
        }
    }

    protected void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            errmsg.Visible = false;
            collegecode = ddlcollege.SelectedValue.ToString();
            BindDegree(singleuser, group_user, collegecode, usercode);
            BindBranchMultiple(singleuser, group_user, course_id, collegecode, usercode);
            btnsave.Visible = false;
            btnreset.Visible = false;
            Gridview1.Visible = false;

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

            dt.Columns.Add("batch");
            dt.Columns.Add("degree_code");
            dt.Columns.Add("degree");
            dt.Columns.Add("department");
            dt.Columns.Add("Sem");
            dt.Columns.Add("day");
            dt.Columns.Add("month");
            dt.Columns.Add("year");
            ArrayList batcharray = new ArrayList();
            ArrayList degreearray = new ArrayList();
            string[] date = new string[32];
            string[] Month = new string[13];
            string[] year = new string[2];

           
            string mainbatch = "";
            if (chklsbatch.Items.Count > 0)
            {
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    if (chklsbatch.Items[i].Selected == true)
                    {
                        if (!batcharray.Contains(chklsbatch.Items[i].Value))
                        {
                            batcharray.Add(chklsbatch.Items[i].Value);
                        }
                        if (mainbatch == "")
                        {
                            mainbatch = chklsbatch.Items[i].Value;
                        }
                        else
                        {
                            mainbatch = mainbatch + "," + chklsbatch.Items[i].Value;
                        }
                    }
                }
            }
            string mainvalue = "";
            if (chklstbranch.Items.Count > 0)
            {
                for (int i = 0; i < chklstbranch.Items.Count; i++)
                {
                    if (chklstbranch.Items[i].Selected == true)
                    {
                        if (!degreearray.Contains(chklstbranch.Items[i].Value))
                        {
                            degreearray.Add(chklstbranch.Items[i].Value);
                        }
                        if (mainvalue == "")
                        {
                            mainvalue = chklstbranch.Items[i].Value;
                        }
                        else
                        {
                            mainvalue = mainvalue + "," + chklstbranch.Items[i].Value;
                        }
                    }
                }
            }
            if (mainvalue.Trim() != "" && mainbatch.Trim() != "")
            {

                string selectquery = "select ROW_NUMBER()  OVER (ORDER BY  batch_year desc) As SNo, r.batch_year as Batch, c.Course_Name as Degree, dbo.ProperCase(dp.Dept_Name) AS Department, r.current_semester as Sem,CAST(NULL AS VARCHAR(30)) as Day,CAST(NULL AS VARCHAR(30)) as Month,CAST(NULL AS VARCHAR(30)) as Year,r.degree_code as degree_code  from registration r,degree de,course c,department dp where r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar' and r.batch_year   in(" + mainbatch + ")  and r.degree_code   in(" + mainvalue + ") group by r.degree_code,r.batch_year,course_name,dept_acronym,current_semester,dp.Dept_Name order by  r.batch_year desc,current_semester asc, r.degree_code";
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds2.Tables.Count>0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int row = 0; row < ds2.Tables[0].Rows.Count; row++)
                        {
                            string day = string.Empty;
                            string month = string.Empty;
                            string yr = string.Empty;
                            string batch_year = Convert.ToString(ds2.Tables[0].Rows[row]["Batch"]);
                            string semester = Convert.ToString(ds2.Tables[0].Rows[row]["Sem"]);
                            string degree = Convert.ToString(ds2.Tables[0].Rows[row]["Degree"]);
                            string degcode = Convert.ToString(ds2.Tables[0].Rows[row]["degree_code"]);
                            string dept = Convert.ToString(ds2.Tables[0].Rows[row]["Department"]); 
                            string getdate = d2.GetFunction("select convert(varchar(10), LockDate,103) as LockDate from InsLockSettings where Batch_Year ='" + batch_year + "' and Degree_Code='" + degcode + "' and Semester='" + semester + "' and SettingType=1");
                            if (getdate.Trim() != "" && getdate.Trim() != "0")
                            {
                                string[] splitdate = getdate.Split('/');
                                if (splitdate.Length > 0)
                                {
                                    day = Convert.ToString(splitdate[0]);
                                    month = Convert.ToString(splitdate[1]);
                                    yr= Convert.ToString(splitdate[2]);
                                }

                            }
                            dr = dt.NewRow();
                            dr["batch"] = batch_year;
                            dr["degree_code"] = degcode;
                            dr["degree"] = degree;
                            dr["department"] = dept;
                            dr["Sem"] = semester;
                            dr["day"] = day;
                            dr["month"] = month;
                            dr["year"] = yr;
                            dt.Rows.Add(dr);
                                                                               
                        }
                    Gridview1.DataSource = dt;
                    Gridview1.DataBind();
                    
                   
                    btnsave.Visible = true;
                    btnreset.Visible = true;
               
                }
                else
                {
                    btnsave.Visible = false;
                    btnreset.Visible = false;
                    Gridview1.Visible = false;
                    divpopalter.Visible = true;
                    divpopaltercontent.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "No Records Found";
                }
            }
            else
            {
                btnsave.Visible = false;
                btnreset.Visible = false;
                Gridview1.Visible = false;
                divpopalter.Visible = true;
                divpopaltercontent.Visible = true;
                lblaltermsgs.Visible = true;
                lblaltermsgs.Text = "Please Select All Fields";
            }
        }
        catch
        {

        }

    }

    protected void gridview1_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DropDownList ddldt = (e.Row.FindControl("ddlday") as DropDownList);
            DropDownList ddlmon = (e.Row.FindControl("ddlmonth") as DropDownList);
            DropDownList ddlyr = (e.Row.FindControl("ddlyear") as DropDownList);
            string currdat = DateTime.Now.ToString("yyyy");
         
           
            ddldt.Items.Insert(0, " ");
            ddlmon.Items.Insert(0, " ");
            ddlyr.Items.Insert(0, " ");
            ddlyr.Items.Insert(1, currdat);

            for (int i = 1; i < 13; i++)
            {
                string item = i.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddlmon.Items.Add(item);
              
            }
            for (int i3 = 0; i3 <= 31; i3++)
            {
                string item = i3.ToString();
                if (item.Length < 2)
                {
                    item = "0" + item;
                }
                ddldt.Items.Add(item);
            }


            Label exdt = e.Row.FindControl("lblday") as Label;
            string exdtt = exdt.Text;
            if (exdtt == "")
                ddldt.Items[0].Selected = true;
            else
                ddldt.Items.FindByText(exdtt).Selected = true;

            Label exmon = e.Row.FindControl("lblmonth") as Label;
            string exmonn = exmon.Text;
            if (exmonn == "")
                ddlmon.Items[0].Selected = true;
            else
                ddlmon.Items.FindByText(exmonn).Selected = true;

            Label exyr = e.Row.FindControl("lblyear") as Label;
            string exyrr = exyr.Text;
            if (exyrr == "")
                ddlyr.Items[0].Selected = true;
            else
                ddlyr.Items.FindByText(exyrr).Selected = true;
           
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            bool testflage = false;
            if (Gridview1.Rows.Count> 0)
            {
                //for (int row = 0; row < Gridview1.Rows.Count; row++)
                foreach(GridViewRow gr in Gridview1.Rows)
                {
                    testflage = true;
                    Label bat = gr.FindControl("lblbatch") as Label;
                    string batch_year = bat.Text;
                    Label sems = gr.FindControl("lblsem") as Label;
                    string semester = sems.Text;
                    DropDownList da1 = (DropDownList)gr.FindControl("ddlday");
                    string day = da1.Text;
                    DropDownList mon = (DropDownList)gr.FindControl("ddlmonth");
                    string month = mon.Text;
                    DropDownList yr = (DropDownList)gr.FindControl("ddlyear");
                    string year = yr.Text;
                    Label deg = (Label)gr.FindControl("lbldegcode");
                    string degree = deg.Text;
                    if (day.Trim() != "" && month.Trim() != "" && year.Trim() != "")
                    {

                        string normaldate = Convert.ToString(month.Trim() + "/" + day.Trim() + "/" + year.Trim());
                        DateTime temp;
                        if (DateTime.TryParse(normaldate, out temp))
                        {
                          
                            string updatequery = "if not exists (select * from InsLockSettings where Batch_Year =" + batch_year + " and Degree_Code='" + degree + "' and Semester='" + semester + "' and SettingType=1 )  insert into InsLockSettings (Batch_Year,Degree_Code,Semester,LockDate,SettingType) values ('" + batch_year + "','" + degree + "','" + semester + "','" + normaldate + "','1')  else  update InsLockSettings set LockDate ='" + normaldate + "' where Batch_Year ='" + batch_year + "' and Degree_Code='" + degree + "' and Semester='" + semester + "' and SettingType=1";
                            int up = d2.update_method_wo_parameter(updatequery, "Text");
                        }

                    }
                }
                if (testflage ==true)
                {
                    divpopalter.Visible = true;
                    divpopaltercontent.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Saved successfully";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Saved successfully')", true);
                }
                else
                {
                    divpopalter.Visible = true;
                    divpopaltercontent.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Please Set Values";
                   // ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Please Set Values')", true);
                }

            }
        }
        catch
        {

        }
    }

    protected void btnreset_Click(object sender, EventArgs e)
    {
        try
        {
            bool testflage = false;
            if (Gridview1.Rows.Count > 0)
            {
                //for (int row = 0; row < Gridview1.Rows.Count; row++)
                foreach (GridViewRow gr in Gridview1.Rows)
                {
                    testflage = true;
                    Label bat = gr.FindControl("lblbatch") as Label;
                    string batch_year = bat.Text;
                    Label sems = gr.FindControl("lblsem") as Label;
                    string semester = sems.Text;
                    Label deg = (Label)gr.FindControl("lbldegcode");
                    string degree = deg.Text;
                   
                    string updatequery = " delete InsLockSettings  where Batch_Year =" + batch_year + " and Degree_Code='" + degree + "' and Semester='" + semester + "' and SettingType=1";
                    int up = d2.update_method_wo_parameter(updatequery, "Text");

                }
                if (testflage == true)
                {
                    divpopalter.Visible = true;
                    divpopaltercontent.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Reset successfully";
                    //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Reset successfully')", true);
                }
                else
                {
                    divpopalter.Visible = true;
                    divpopaltercontent.Visible = true;
                    lblaltermsgs.Visible = true;
                    lblaltermsgs.Text = "Not Reseted";
                }
            }
        }
        catch
        {

        }
    }

    protected void ddlday_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlmonth_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }
    protected void ddlyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void gridview1_DataBound(object sender, EventArgs e)
    {
        try
        {
            for (int rowIndex = Gridview1.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = Gridview1.Rows[rowIndex];
                GridViewRow previousRow = Gridview1.Rows[rowIndex + 1];


                string l3 = (row.FindControl("lblbatch") as Label).Text;
                string l4 = (previousRow.FindControl("lblbatch") as Label).Text;
                if (l3 == l4)
                {
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;
                    previousRow.Cells[1].Visible = false;
                    string l5 = (row.FindControl("lbldegree") as Label).Text;
                    string l6 = (previousRow.FindControl("lbldegree") as Label).Text;
                    if (l5 == l6)
                    {
                        row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                               previousRow.Cells[2].RowSpan + 1;
                        previousRow.Cells[2].Visible = false;

                    }
                    string l7 = (row.FindControl("lblsem") as Label).Text;
                    string l8 = (previousRow.FindControl("lblsem") as Label).Text;
                    if (l7 == l8)
                    {
                        row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                               previousRow.Cells[4].RowSpan + 1;
                        previousRow.Cells[4].Visible = false;
                    }
                }
               
               
                
            }
        }

        catch
        {
        }
    }
    protected void btnokclk_Click(object sender, EventArgs e)
    {
        divpopalter.Visible = false;
        divpopaltercontent.Visible = false;
    }
}