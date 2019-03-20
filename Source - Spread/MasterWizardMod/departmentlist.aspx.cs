using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using InsproDataAccess;
using System.Drawing;
using System.Configuration;
using System.Data.SqlClient;


public partial class departmentlist : System.Web.UI.Page
{

    Hashtable ht = new Hashtable();
    DataSet ds = new DataSet();
    DAccess2 da = new DAccess2();
    DataRow dryr;
    static int searchby = 0;

    static Boolean deanclick = false;
    static Boolean hodclick = false;
    static Boolean coordinatorclick = false;

    DataTable dtable = new DataTable();
    DataRow dr1;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("MasterWizardMenuIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/MasterWizardMod/MasterWizardMenuIndex.aspx");
                    return;
                }
            }
            if (Session["collegecode"] == null)
            {

                string redrURI = ConfigurationManager.AppSettings["Logout"].Trim();
                Response.Redirect(redrURI, false);
                return;
            }
            if (!IsPostBack)
            {
                bindclg();
                bindfromyear();
            }
        }
        catch
        {
        }
    }

    #region BindHeader

    public void bindclg()
    {

        try
        {
            ddlclg.Items.Clear();
            string columnfield = string.Empty;
            string group_user = (Session["collegecode"] != null) ? Convert.ToString(Session["collegecode"]).Trim() : "";
            if (group_user.Contains(";"))
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
            ht.Clear();
            ht.Add("column_field", Convert.ToString(columnfield));
            DataSet dsprint = da.select_method("bind_college", ht, "sp");
            ddlclg.Items.Clear();
            if (dsprint.Tables.Count > 0 && dsprint.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = dsprint;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
                ddlclg.SelectedIndex = 0;


                ddlcollege1.DataSource = dsprint;
                ddlcollege1.DataTextField = "collname";
                ddlcollege1.DataValueField = "college_code";
                ddlcollege1.DataBind();
                ddlcollege1.SelectedIndex = 0;
            }
        }


        catch
        {
        }
    }

    public void bindfromyear()
    {
        try
        {
            DataTable dtyr = new DataTable();
            dtyr.Columns.Add("year");
            string curryr = DateTime.Now.ToString("yyyy");
            int k = 0;
            for (int j = 1950; j <= Convert.ToInt32(curryr); j++)
            {
                dryr = dtyr.NewRow();
                dryr["year"] = Convert.ToString(j);
                dtyr.Rows.Add(dryr);
                k++;
            }
            if (dtyr.Rows.Count > 0)
            {
                ddlfromyr.DataSource = dtyr;
                ddlfromyr.DataTextField = "year";
                ddlfromyr.DataValueField = "year";
                ddlfromyr.DataBind();
                ddlfromyr.SelectedIndex = k - 1;

                ddltoyear.DataSource = dtyr;
                ddltoyear.DataTextField = "year";
                ddltoyear.DataValueField = "year";
                ddltoyear.DataBind();
                ddltoyear.SelectedIndex = k - 1;

            }
        }
        catch
        {
        }
    }

    public void loadstaffdep()
    {

        string cmd = "select distinct dept_name,dept_code from hrdept_master where college_code=" + ddlclg.SelectedValue.ToString() + "";

        DataSet ds1 = new DataSet();
        ds1 = da.select_method_wo_parameter(cmd, "Text");

        ddldepratstaff.DataSource = ds1;
        ddldepratstaff.DataTextField = "dept_name";
        ddldepratstaff.DataValueField = "dept_code";
        ddldepratstaff.DataBind();
        ddldepratstaff.Items.Insert(0, "All");


    }

    #endregion


    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getsearchby(string prefixText)
    {
        string query = "";

        WebService ws = new WebService();
        List<string> values = new List<string>();


        if (searchby == 0)
        {

            query = "select distinct Dept_Name from Department where isacademic=0";
        }
        else if (searchby == 1)
        {
            query = "select distinct Dept_Name from Department where isacademic=1";
        }
        else
        {
            query = "select distinct Dept_Name from Department";
        }




        values = ws.Getname(query);
        return values;
    }

    protected void btnaddnew_Click(object sender, EventArgs e)
    {

        divPopAlertbackvolume.Visible = true;
        divPopAlertback.Visible = true;
        
    }
    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            dtable.Clear();
            dtable.Columns.Add("S.No");
            dtable.Columns.Add("Dean Name");
            dtable.Columns.Add("Department Name");
            dtable.Columns.Add("Head Of The Deaprtment");
            dtable.Columns.Add("Coordinator Name");
            dtable.Columns.Add("Year Of Introduction");
            dtable.Columns.Add("departtype");
            dtable.Columns.Add("acronym");
            dtable.Columns.Add("grpdept");

            dryr = dtable.NewRow();
            dryr["S.No"] = "S.No";
            dryr["Dean Name"] = "Dean Name";
            dryr["Department Name"] = "Department Name";
            dryr["Head Of The Deaprtment"] = "Head Of The Deaprtment";
            dryr["Coordinator Name"] = "Coordinator Name";
            dryr["Year Of Introduction"] = "Year Of Introduction";
            dryr["departtype"] = "departtype";
            dryr["acronym"] = "acronym";
            dryr["grpdept"] = "grpdept";
            dtable.Rows.Add(dryr);

            string acad = ddldepartmenttype.SelectedValue.ToString();
            string academic = string.Empty;
            if (acad != "2")
            {
                academic = " and isacademic='" + acad + "'";
            }
            string searchby = Convert.ToString(txtsearch.Text);
            string deptnam = string.Empty;
            if (!string.IsNullOrEmpty(searchby))
                deptnam = " and dept_name='" + searchby.Trim() + "'";

            string qry = "select dean_name,Dept_Name,Head_Of_Dept,coordinator_name,start_year,grp_dept,dept_acronym,isacademic from Department where college_code='" + ddlclg.SelectedValue.ToString() + "' " + academic + " and start_year between '" + ddlfromyr.SelectedItem.Text.ToString() + "' and '" + ddltoyear.SelectedItem.Text.ToString() + "' " + deptnam + "";
            ds.Clear();
            ds = da.select_method_wo_parameter(qry, "text");
            int sno = 0;
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    sno++;
                    dryr = dtable.NewRow();
                    dryr["S.No"] = Convert.ToString(sno);
                    dryr["Dean Name"] = Convert.ToString(ds.Tables[0].Rows[i]["dean_name"]);
                    dryr["Department Name"] = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    dryr["Head Of The Deaprtment"] = Convert.ToString(ds.Tables[0].Rows[i]["Head_Of_Dept"]);
                    dryr["Coordinator Name"] = Convert.ToString(ds.Tables[0].Rows[i]["coordinator_name"]);
                    dryr["Year Of Introduction"] = Convert.ToString(ds.Tables[0].Rows[i]["start_year"]);
                    dryr["departtype"] = Convert.ToString(ds.Tables[0].Rows[i]["isacademic"]);
                    dryr["acronym"] = Convert.ToString(ds.Tables[0].Rows[i]["dept_acronym"]);
                    dryr["grpdept"] = Convert.ToString(ds.Tables[0].Rows[i]["grp_dept"]);
                    dtable.Rows.Add(dryr);
                }
                GridView1.DataSource = dtable;
                GridView1.DataBind();

                GridView1.HeaderRow.Visible = false;
                GridView1.Rows[0].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                GridView1.Rows[0].Font.Bold = true;
                GridView1.Rows[0].HorizontalAlign = HorizontalAlign.Center;
                GridView1.Rows[0].Height = 35;


                GridView1.Visible = true;

            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "No Records Found";
            }
        }
        catch
        {
        }
    }

    protected void gridview1_DataBound(object sender, EventArgs e)
    {
    }
    protected void SelectedIndexChanged1(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            ViewState["CurrentTable"] = null;
            divPopAlertback.Visible = true;
            divPopAlertbackvolume.Visible = true;
            GridView1.Visible = true;
            if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "1")
            {
                string depttype = GridView1.Rows[rowIndex].Cells[6].Text;
                if (depttype.Trim() == "&nbsp;")
                    depttype = string.Empty;
                string deptname = GridView1.Rows[rowIndex].Cells[2].Text;
                if (deptname.Trim() == "&nbsp;")
                    deptname = string.Empty;
                string yeraofintro = GridView1.Rows[rowIndex].Cells[5].Text;
                if (yeraofintro.Trim() == "&nbsp;")
                    yeraofintro = string.Empty;
                string acronym = GridView1.Rows[rowIndex].Cells[7].Text;
                if (acronym.Trim() == "&nbsp;")
                    acronym = string.Empty;
                string deptgrp = GridView1.Rows[rowIndex].Cells[8].Text;
                if (deptgrp.Trim() == "&nbsp;")
                    deptgrp = string.Empty;
                string deanname = GridView1.Rows[rowIndex].Cells[1].Text;
                if (deanname.Trim() == "&nbsp;")
                    deanname = string.Empty;
                string hodname = GridView1.Rows[rowIndex].Cells[3].Text;
                if (hodname.Trim() == "&nbsp;")
                    hodname = string.Empty;
                string coordinate = GridView1.Rows[rowIndex].Cells[4].Text;
                if (coordinate.Trim() == "&nbsp;")
                    coordinate = string.Empty;

                if (!string.IsNullOrEmpty(depttype))
                {
                    if (depttype == "1")
                    {
                        rbacademic.Items[0].Selected = true;
                    }
                    if (depttype == "0")
                    {
                        rbacademic.Items[1].Selected = true;
                    }
                }
                if (!string.IsNullOrEmpty(deptname))
                    txtdepartname.Text = deptname;
                if (!string.IsNullOrEmpty(yeraofintro))
                    txtyrofintro.Text = yeraofintro;
                if (!string.IsNullOrEmpty(acronym))
                    txtarconym.Text = acronym;
                if (!string.IsNullOrEmpty(deptgrp))
                {
                    if (deptgrp == "0")
                    {
                        cbgroup.Checked = false;
                    }
                    else
                    {
                        cbgroup.Checked = true;
                    }
                }
                if (!string.IsNullOrEmpty(deanname))
                    Txtdean.Text = deanname;
                if (!string.IsNullOrEmpty(hodname))
                    txthod.Text = hodname;
                if (!string.IsNullOrEmpty(coordinate))
                    txtarconym.Text = coordinate;

                string dept_code = da.GetFunction("select Dept_Code from Department where Dept_Name='" + deptname + "'  and dept_acronym ='" + acronym + "'");
                if (string.IsNullOrEmpty(dept_code))
                    dept_code = "0";

                lbldept_code.Text = dept_code;
                btnsave.Text = "Update";
                string buildnam = string.Empty;
                string floornam = string.Empty;
                string roomnam = string.Empty;
                string inst = "select building_name,floor_name,room_name from room_detail where selectionflag='1' and dept_code='" + dept_code + "'  and college_code='" + ddlclg.SelectedValue.ToString() + "' ";
                DataSet ds2 = da.select_method_wo_parameter(inst, "text");
                if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                {
                    for (int l = 0; l < ds2.Tables[0].Rows.Count; l++)
                    {
                        if (string.IsNullOrEmpty(buildnam))
                            buildnam = Convert.ToString(ds2.Tables[0].Rows[0]["room_name"]);
                        else
                            buildnam = buildnam + "," + Convert.ToString(ds2.Tables[0].Rows[0]["room_name"]);
                        if (string.IsNullOrEmpty(floornam))
                            floornam = Convert.ToString(ds2.Tables[0].Rows[0]["floor_name"]);
                        else
                            floornam = floornam + "," + Convert.ToString(ds2.Tables[0].Rows[0]["floor_name"]);
                        if (string.IsNullOrEmpty(roomnam))
                            roomnam = Convert.ToString(ds2.Tables[0].Rows[0]["building_name"]);
                        else
                            roomnam =roomnam+ "," + Convert.ToString(ds2.Tables[0].Rows[0]["building_name"]);
                    }
                    txtbuilacr.Text = buildnam;
                    txtfloor.Text = floornam;
                    txtroom.Text = roomnam;
                }
                else
                {
                    txtbuilacr.Text = string.Empty;
                    txtfloor.Text = string.Empty;
                    txtroom.Text = string.Empty;

                }

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
            e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[5].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[0].Width = 30;
            e.Row.Cells[1].Width = 100;
            e.Row.Cells[2].Width = 400;
            e.Row.Cells[3].Width = 100;
            e.Row.Cells[4].Width = 150;
            e.Row.Cells[5].Width = 100;
            e.Row.Cells[6].Visible = false;
            e.Row.Cells[7].Visible = false;
            e.Row.Cells[8].Visible = false;
        }
    }

    protected void OnRowCreated1(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void ddldepartmenttype_selectedindexchange(object sender, EventArgs e)
    {
        if (ddldepartmenttype.SelectedValue.ToString() == "0")
        {
            searchby = 0;
        }
        else if (ddldepartmenttype.SelectedValue.ToString() == "1")
        {
            searchby = 1;
        }
        else
        {
            searchby = 2;
        }
    }
    protected void ddlcollege_selectedindexchange(object sender, EventArgs e)
    {
    }


    protected void loadfsstaff()
    {
        staffok.Visible = false;
        string sql = "";
        DataTable dtable1 = new DataTable();
        DataRow dtrow2;
        if (ddldepratstaff.SelectedIndex != 0)
        {
            if (txt_search.Text != "")
            {
                if (ddlstaff.SelectedIndex == 0)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0)and  (staffmaster.settled = 0)  and (staff_name like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege1.SelectedValue + "' and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
                else if (ddlstaff.SelectedIndex == 1)
                {
                    sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '" + txt_search.Text + "%') and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege1.SelectedValue + "'  and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
                }
            }
            else
            {

                sql = "SELECT staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code  WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (hrdept_master.dept_code = '" + ddldepratstaff.SelectedValue + "') AND (staffmaster.college_code = '" + ddlcollege1.SelectedValue + "') and (staffmaster.college_code =hrdept_master.college_code)  and StaffCategorizer.college_code=staffmaster.college_code";
            }
        }
        else if (txt_search.Text != "")
        {
            if (ddlstaff.SelectedIndex == 0)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_name like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege1.SelectedValue + "'  and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
            }
            else if (ddlstaff.SelectedIndex == 1)
            {
                sql = "SELECT distinct staffmaster.staff_code, staffmaster.staff_name FROM staffmaster INNER JOIN stafftrans ON staffmaster.staff_code = stafftrans.staff_code INNER JOIN hrdept_master ON stafftrans.dept_code = hrdept_master.dept_code inner join   StaffCategorizer on stafftrans.category_code=StaffCategorizer.category_code WHERE (stafftrans.latestrec <> 0) AND (staffmaster.resign = 0) and (staffmaster.settled = 0) and (staffmaster.staff_code like '%" + txt_search.Text + "%') and (staffmaster.college_code =hrdept_master.college_code) and staffmaster.college_code='" + ddlcollege1.SelectedValue + "' and StaffCategorizer.college_code=staffmaster.college_code";//Modifed By Srinath 9/5/2013
            }
            else if (ddlcollege1.SelectedIndex != -1)
            {
                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege1.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
            else
            {
                sql = "select distinct staffmaster.staff_code, staff_name from stafftrans,staffmaster,hrdept_master.dept_name where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and staffmaster.college_code='" + ddlcollege1.SelectedValue + "'";//Modifed By Srinath 9/5/2013
            }
        }
        else
            if (ddldepratstaff.SelectedValue.ToString() == "All")
            {

                sql = "select distinct staffmaster.staff_code, staff_name  from stafftrans,staffmaster,StaffCategorizer where stafftrans.staff_code=staffmaster.staff_code and latestrec<>0 and resign=0 and settled=0 and staffmaster.college_code='" + ddlcollege1.SelectedValue + "' and StaffCategorizer.category_code= stafftrans.category_code and StaffCategorizer.college_code=staffmaster.college_code";
            }

        DataSet dsbindspread = new DataSet();
        dsbindspread = da.select_method_wo_parameter(sql, "Text");

        if (dsbindspread.Tables[0].Rows.Count > 0)
        {
            int sno = 0;

            dtable1.Columns.Add("Staff_Code");
            dtable1.Columns.Add("Staff_Name");
            for (int rolcount = 0; rolcount < dsbindspread.Tables[0].Rows.Count; rolcount++)
            {
                sno++;
                string name = dsbindspread.Tables[0].Rows[rolcount]["staff_name"].ToString();
                string code = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();

                dtrow2 = dtable1.NewRow();
                dtrow2["Staff_Code"] = dsbindspread.Tables[0].Rows[rolcount]["staff_code"].ToString();
                dtrow2["Staff_Name"] = name;
                dtable1.Rows.Add(dtrow2);
            }


            gviewstaff.DataSource = dtable1;
            gviewstaff.DataBind();
            gviewstaff.Visible = true;
            Labelstaffalert.Visible = false;
            staffok.Visible = true;

        }
        else
        {
            gviewstaff.Visible = false;
            staffok.Visible = false;
            Labelstaffalert.Visible = true;

        }
    }



    protected void btnsave_click(object sender, EventArgs e)
    {
        try
        {
            if (txtdepartname.Text == string.Empty)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Enter Department Name";
                return;
            }
            if (txtarconym.Text == string.Empty)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Please Enter The Acronym";
                return;
            }
            if (rbacademic.Items[0].Selected == true && rbacademic.Items[1].Selected == false)
            {
            }
            else if (rbacademic.Items[0].Selected == false && rbacademic.Items[1].Selected == true)
            {
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Please Select Academic or Others";
                return;
            }
            string depttype = string.Empty;
            string grpdept = string.Empty;
            if (rbacademic.SelectedIndex.ToString() == "0")
                depttype = "1";
            if (rbacademic.SelectedIndex.ToString() == "1")
                depttype = "0";

            string deptname = txtdepartname.Text;
            string yrofintro = txtyrofintro.Text;
            string acronym = txtarconym.Text;
            if (cbgroup.Checked == true)
                grpdept = "1";
            else
                grpdept = "0";
            string deanname = Txtdean.Text;
            string hodname = txthod.Text;
            string coordinate = txtcordinator.Text;
            string buildingacrn = string.Empty;
            string flooracrn = string.Empty;
            string roomarn = string.Empty;
            string dept_code = lbldept_code.Text;
            if (string.IsNullOrEmpty(dept_code))
                dept_code = "0";

            string qry = "if exists (select * from Department where Dept_Code='" + dept_code + "' and college_code='" + ddlclg.SelectedValue.ToString() + "') update Department set dept_acronym='" + acronym + "' ,isacademic='" + depttype + "',dean_name='" + deanname + "',Dept_Name='" + deptname + "',start_year='" + yrofintro + "',grp_dept='" + grpdept + "',Head_Of_Dept='" + hodname + "',coordinator_name='" + coordinate + "' where Dept_Code='" + dept_code + "' and college_code='" + ddlclg.SelectedValue.ToString() + "' else insert into Department (Dept_Name,isacademic,start_year,dept_acronym,grp_dept,dean_name,Head_Of_Dept,coordinator_name,college_code) values('" + deptname + "','" + depttype + "','" + yrofintro + "','" + acronym + "','" + grpdept + "','" + deanname + "','" + hodname + "','" + coordinate + "','" + ddlclg.SelectedValue.ToString() + "')";

            int save = da.update_method_wo_parameter(qry, "text");

            string dptcd = da.GetFunction("select Dept_Code from department where Dept_Name='" + deptname + "' and college_code='" + ddlclg.SelectedValue.ToString() + "'");



            if (txtroom.Text != "" && txtbuilacr.Text != "" && txtfloor.Text != "")
            {
                bool flagtr = false;
                string[] spt = null;
                string[] spt1 = null;
                string[] spt2 = null;
                string roomname = txtroom.Text;
                if (roomname.Contains(','))
                {
                    flagtr = true;
                    spt = roomname.Split(',');
                }

                string floorname = txtfloor.Text;
                if (floorname.Contains(','))
                {
                    flagtr = true;
                    spt1 = floorname.Split(',');
                }

                string buildname = txtbuilacr.Text;
                if (buildname.Contains(','))
                {
                    flagtr = true;
                    spt2 = buildname.Split(',');
                }
                if (flagtr == true)
                {
                    for (int r = 0; r < spt.Length; r++)
                    {
                        roomname = spt[r].ToString();
                        floorname = spt1[r].ToString();
                        buildname = spt2[r].ToString();
                        string inst = "update room_detail set selectionflag='1',dept_code='" + dptcd + "'  where Building_Name='" + buildname + "' and Floor_Name='" + floorname + "' and college_code='" + ddlclg.SelectedValue.ToString() + "' and room_name='" + roomname + "'";
                        int instroom = da.update_method_wo_parameter(inst, "text");
                    }
                }
                else
                {
                    string inst = "update room_detail set selectionflag='1',dept_code='" + dptcd + "'  where Building_Name='" + txtbuilacr.Text.Trim() + "' and Floor_Name='" + txtfloor.Text.Trim() + "' and college_code='" + ddlclg.SelectedValue.ToString() + "' and room_name='" + txtroom.Text.Trim() + "' ";
                    int instroom = da.update_method_wo_parameter(inst, "text");
                }
            }




            if (save > 0)
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                if (btnsave.Text.Trim().ToLower() == "update")
                {
                    Label3.Text = "Updated Successfully";
                }
                else
                {
                    Label3.Text = "Saved Successfully";
                }
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                if (btnsave.Text.Trim().ToLower() == "update")
                {
                    Label3.Text = "Not Updated";
                }
                else
                {
                    Label3.Text = "Not Saved";
                }
            }

        }
        catch
        {
        }
    }

    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            string depttype = string.Empty;
            string grpdept = string.Empty;
            if (rbacademic.SelectedIndex.ToString() == "0")
                depttype = "1";
            if (rbacademic.SelectedIndex.ToString() == "1")
                depttype = "0";

            string deptname = txtdepartname.Text;
            string yrofintro = txtyrofintro.Text;
            string acronym = txtarconym.Text;
            if (cbgroup.Checked == true)
                grpdept = "1";
            else
                grpdept = "0";
            string deanname = Txtdean.Text;
            string hodname = txthod.Text;
            string coordinate = txtcordinator.Text;
            string buildingacrn = string.Empty;
            string flooracrn = string.Empty;
            string roomarn = string.Empty;
            string dept_code = lbldept_code.Text;
            string del = string.Empty;
            if (string.IsNullOrEmpty(dept_code))
            {
                dept_code = da.GetFunction("select dept_code from department where dept_name='" + deptname + "' and college_code='" + ddlclg.SelectedValue.ToString() + "'");
            }

            string count = "select degree_code from degree where dept_code='" + dept_code + "' and college_code='" + ddlclg.SelectedValue.ToString() + "'; SELECT dept_code  FROM Staff_Appl_Master WHERE Dept_Code ='" + dept_code + "' AND College_Code ='" + ddlclg.SelectedValue.ToString() + "'; SELECT Dept_Code FROM StaffTrans T,StaffMaster M WHERE T.Staff_Code = M.Staff_Code AND Dept_Code ='" + dept_code + "' AND College_Code ='" + ddlclg.SelectedValue.ToString() + "'";
            DataSet ds2 = new DataSet();
            ds2 = da.select_method_wo_parameter(count, "text");
            if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
            {
                div1.Visible = false;
                div2.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Some Additional Information has been present for example degree,student information etc.,So, Departments Cannot Be Deleted From the list.";
                return;
            }
            else if (ds2.Tables.Count > 0 && ds2.Tables[1].Rows.Count > 0)
            {
                div1.Visible = false;
                div2.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Some Staff's are under this department .So, Department Cannot Be Deleted From the list";
                return;
            }
            else if (ds2.Tables.Count > 0 && ds2.Tables[2].Rows.Count > 0)
            {
                div1.Visible = false;
                div2.Visible = false;
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Some Staff's are under this department .So, Department Cannot Be Deleted From the list";
                return;
            }
            else
            {

                del = "delete Department where Dept_Code='" + dept_code + "'";

                int del1 = da.update_method_wo_parameter(del, "text");
                if (del1 > 0)
                {
                    div1.Visible = false;
                    div2.Visible = false;
                    div4.Visible = true;
                    div5.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Deleted Successfully";
                }
                else
                {
                    div1.Visible = false;
                    div2.Visible = false;
                    div4.Visible = true;
                    div5.Visible = true;
                    Label3.Visible = true;
                    Label3.Text = "Not Deleted";
                }
            }

        }
        catch
        {
        }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        div1.Visible = false;
        div2.Visible = false;
    }

    protected void btndelete_click(object sender, EventArgs e)
    {
        try
        {
            div1.Visible = true;
            div2.Visible = true;
            Label2.Visible = true;
            Label2.Text = "This will Delete The Degrees Under This Department.Do you Want To Continue Any Way?";
        }
        catch
        {
        }
    }
    protected void btnexit_click(object sender, EventArgs e)
    {
        txtdepartname.Text = string.Empty;
        rbacademic.Items[0].Selected = false;
        rbacademic.Items[1].Selected = false;
        txtyrofintro.Text = string.Empty;
        txtarconym.Text = string.Empty;
        cbgroup.Checked = false;
        Txtdean.Text = string.Empty;
        txthod.Text = string.Empty;
        txtcordinator.Text = string.Empty;
        lbldept_code.Text = string.Empty;
        txtroom.Text = string.Empty;
        txtfloor.Text = string.Empty;
        txtbuilacr.Text = string.Empty;

        divPopAlertbackvolume.Visible = false;
        divPopAlertback.Visible = false;
    }

    protected void ddlfromyr_selectedindexchange(object sender, EventArgs e)
    {
    }
    protected void ddltoyear_selectedindexchange(object sender, EventArgs e)
    {
    }


    #region Room Selection

    protected void lnkroomalloc_Click(object sender, EventArgs e)
    {
        try
        {
            GridView3.Visible = false;
            divroomalloc.Visible = true;
            divroomalloc1.Visible = true;

            string qry = "select Building_Name,College_Code,StartingSerial,selectionflag from building_master where college_code='" + ddlclg.SelectedValue.ToString() + "';select Floor_Name,Building_Name from floor_master where College_Code='" + ddlclg.SelectedValue.ToString() + "'";
            DataSet dss = da.select_method_wo_parameter(qry, "text");

            DataTable dtroom = new DataTable();
            DataRow drroom = null;

            if (dss.Tables.Count > 0 && dss.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    string buildname = Convert.ToString(dss.Tables[0].Rows[i]["Building_Name"]);
                    dtroom.Columns.Add(buildname);
                }
                int count1 = 0;
                int ct = dss.Tables[0].Rows.Count;
                for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                {
                    string buildingname = Convert.ToString(dss.Tables[0].Rows[j]["Building_Name"]);
                    dss.Tables[1].DefaultView.RowFilter = " Building_Name='" + Convert.ToString(dss.Tables[0].Rows[j]["Building_Name"]) + "'";
                    DataView dv = new DataView();
                    dv = dss.Tables[1].DefaultView;
                    int count = dv.Count;
                    if (count == 0)
                        count = count1;
                    if (j != 0)
                    {

                        if (count > count1)
                        {
                            int ct1 = count - count1;
                            for (int k1 = 0; k1 < ct1; k1++)
                            {
                                drroom = dtroom.NewRow();
                                dtroom.Rows.Add(drroom);
                            }
                        }
                    }
                    if (dv.Count > 0)
                    {
                        for (int k = 0; k < dv.Count; k++)
                        {
                            if (j == 0)
                            {
                                drroom = dtroom.NewRow();
                                drroom["" + buildingname + ""] = dv[k]["Floor_Name"].ToString();
                                dtroom.Rows.Add(drroom);
                            }
                            else
                            {
                                dtroom.Rows[k]["" + buildingname + ""] = dv[k]["Floor_Name"].ToString();
                                // drroom["" + buildingname + ""][k] = dv[k]["Floor_Name"].ToString();
                            }
                        }



                    }

                    count1 = dv.Count;
                    if (count1 == 0)
                        count1 = count;
                }

                GridView2.DataSource = dtroom;
                GridView2.DataBind();
            }


        }
        catch
        {
        }
    }
    protected void gridview2_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    protected void gridview2_DataBound(object sender, EventArgs e)
    {
    }

    protected void SelectedIndexChanged_gridview2(object sender, EventArgs e)
    {
        try
        {
            var grid = (GridView)sender;
            GridViewRow selectedRow = grid.SelectedRow;
            int rowIndex = grid.SelectedIndex;
            int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
            DataTable dt1 = new DataTable();
            DataRow dr1 = null;

            dt1.Columns.Add("roomAcronymn");
            dt1.Columns.Add("roomtype");
            dt1.Columns.Add("buildacronym");
            dt1.Columns.Add("flooracronymn");

            if (Convert.ToString(rowIndex) != "" && Convert.ToString(selectedCellIndex) != "")
            {
                string floorname = Convert.ToString(GridView2.Rows[rowIndex].Cells[selectedCellIndex].Text);
                if (floorname.Trim() == "&nbsp;")
                    floorname = string.Empty;

                string buildingname = Convert.ToString(GridView2.HeaderRow.Cells[selectedCellIndex].Text);
                if (buildingname.Trim() == "&nbsp;")
                    buildingname = string.Empty;


                string qry = "select Room_Name,Building_Name,Room_type,Floor_Name from room_detail where Building_Name in ('" + buildingname + "') and Floor_Name in ('" + floorname + "') and college_code='" + ddlclg.SelectedValue.ToString() + "'";
                DataSet ds1 = new DataSet();
                ds1 = da.select_method_wo_parameter(qry, "text");
                if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                    {
                        dr1 = dt1.NewRow();
                        dr1["roomAcronymn"] = Convert.ToString(ds1.Tables[0].Rows[j]["Room_Name"]);
                        dr1["buildacronym"] = Convert.ToString(ds1.Tables[0].Rows[j]["Building_Name"]);
                        dr1["roomtype"] = Convert.ToString(ds1.Tables[0].Rows[j]["Room_type"]);
                        dr1["flooracronymn"] = Convert.ToString(ds1.Tables[0].Rows[j]["Floor_Name"]);
                        dt1.Rows.Add(dr1);

                    }
                    GridView3.DataSource = dt1;
                    GridView3.DataBind();
                    GridView3.Visible = true;
                    lblerrormsg.Visible = false;
                }
                else
                {
                    GridView3.Visible = false;
                    lblerrormsg.Visible = true;
                    lblerrormsg.Text = "No Records Found";
                }



            }
        }
        catch
        {
        }
    }

    protected void OnRowCreated2(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }

    protected void btnoknew_OnClick(object sender, EventArgs e)
    {
        bool alertflaf = false;
        int k = 0;
        string flooracr = string.Empty;
        string buildacr = string.Empty;
        string roomacr = string.Empty;
        foreach (GridViewRow gr in GridView3.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gr.FindControl("cbselect");
            if (chk.Checked == true)
            {
                alertflaf = true;

                Label floorname = gr.FindControl("lblflooracronymn") as Label;
                if (string.IsNullOrEmpty(flooracr))
                    flooracr = floorname.Text;
                else
                    flooracr = flooracr + "," + floorname.Text;



                Label buildname = gr.FindControl("lblbuildacronym") as Label;
                if (string.IsNullOrEmpty(buildacr))
                    buildacr = buildname.Text;
                else
                    buildacr = buildacr + "," + buildname.Text;


                Label roomname = gr.FindControl("lblroomAcronymn") as Label;
                if (string.IsNullOrEmpty(roomacr))
                    roomacr = roomname.Text;
                else
                    roomacr = roomacr + "," + roomname.Text;


            }
        }

        if (alertflaf == false)
        {
            txtfloor.Text = string.Empty;
            txtroom.Text = string.Empty;
            txtbuilacr.Text = string.Empty;
            div4.Visible = true;
            div5.Visible = true;
            Label3.Visible = true;
            Label3.Text = "Please Select Alteast One Room";
        }
        else
        {
            txtfloor.Text = flooracr;
            txtroom.Text = roomacr;
            txtbuilacr.Text = buildacr;
            divroomalloc.Visible = false;
            divroomalloc1.Visible = false;
        }

    }
    protected void btnclosenew_OnClick(object sender, EventArgs e)
    {
        divroomalloc.Visible = false;
        divroomalloc1.Visible = false;
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        divroomalloc.Visible = false;
        divroomalloc1.Visible = false;
    }

    #endregion




    #region Staff Lookup

    protected void btnhod_click(object sender, EventArgs e)
    {
        hodclick = true;
        deanclick = false;
        coordinatorclick = false;
        panel3.Visible = true;
        divPopAlertback.Visible = false;
        divPopAlertbackvolume.Visible = false;
        bindclg();
        loadstaffdep();
        loadfsstaff();
    }
    protected void btncoordinator_click(object sender, EventArgs e)
    {
        coordinatorclick = true;
        hodclick = false;
        deanclick = false;
        panel3.Visible = true;
        divPopAlertback.Visible = false;
        divPopAlertbackvolume.Visible = false;
        bindclg();
        loadstaffdep();
        loadfsstaff();
    }
    protected void btndean_click(object sender, EventArgs e)
    {
        try
        {
            deanclick = true;
            coordinatorclick = false;
            hodclick = false;
            panel3.Visible = true;
            divPopAlertback.Visible = false;
            divPopAlertbackvolume.Visible = false;
            bindclg();
            loadstaffdep();
            loadfsstaff();
        }
        catch
        {
        }
    }
    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        divPopAlertbackvolume.Visible = true;
        divPopAlertback.Visible = true;
    }
    protected void ddlstaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }
    protected void txt_search_TextChanged(object sender, EventArgs e)
    {
        //fsstaff.Sheets[0].RowCount = 0;
        loadfsstaff();
    }
    protected void SelectedIndexChanged(Object sender, EventArgs e)
    {
        var grid = (GridView)sender;
        GridViewRow selectedRow = grid.SelectedRow;
        int RowIndex = grid.SelectedIndex;
        int selectedCellIndex = int.Parse(this.SelectedGridCellIndex.Value);
        Session["Gridcellrowstaff"] = Convert.ToString(RowIndex);


    }
    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                TableCell cell = e.Row.Cells[i];
                cell.Attributes["onmouseover"] = "this.style.cursor='pointer';";
                cell.Attributes["onmouseout"] = "this.style.textDecoration='none';";
                cell.Attributes["onclick"] = string.Format("document.getElementById('{0}').value = {1}; {2}"
                   , SelectedGridCellIndex.ClientID, i
                   , Page.ClientScript.GetPostBackClientHyperlink((GridView)sender, string.Format("Select${0}", e.Row.RowIndex)));
            }
        }
    }
    protected void imagebtnpopclose5_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        divPopAlertbackvolume.Visible = true;
        divPopAlertback.Visible = true;

    }
    protected void btnaltok_Click(object sender, EventArgs e)
    {
        div4.Visible = false;
        div5.Visible = false;
    }
    protected void ddldepratstaff_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadfsstaff();
    }
    protected void ddlcollege1_SelectedIndexChanged(object sender, EventArgs e)
    {

        loadfsstaff();
    }
    protected void btn_staffok_Click(object sender, EventArgs e)
    {
        try
        {


            if (Session["Gridcellrowstaff"] != "" && Session["Gridcellrowstaff"] != null)
            {
                int RowIndex = Convert.ToInt32(Session["Gridcellrowstaff"]);
                if (RowIndex != -1)
                {
                    Label lblstaffcode = (gviewstaff.Rows[RowIndex].FindControl("lblstaff") as Label);
                    Label lblstaff = (gviewstaff.Rows[RowIndex].FindControl("lblname") as Label);
                    string staffname = lblstaff.Text;
                    if (deanclick == true)
                    {
                        Txtdean.Text = staffname;
                    }
                    if (hodclick == true)
                        txthod.Text = staffname;
                    if (coordinatorclick == true)
                        txtcordinator.Text = staffname;
                    panel3.Visible = false;
                    divPopAlertbackvolume.Visible = true;
                    divPopAlertback.Visible = true;
                }
            }
            else
            {
                div4.Visible = true;
                div5.Visible = true;
                Label3.Visible = true;
                Label3.Text = "Please Select Atleast One Staff!";

            }
        }
        catch
        {

        }
    }

    #endregion
}