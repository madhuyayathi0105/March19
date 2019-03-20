using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using Gios.Pdf;
using System.IO;
using System.Configuration;

public partial class InventoryMod_Student_Kit_Report : System.Web.UI.Page
{

    #region FieldDeclaration
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string sessstream = string.Empty;
    string selectQuery = "";
    DAccess2 d2 = new DAccess2();
    DataSet ds = new DataSet();
    ReuasableMethods rs = new ReuasableMethods();
    Hashtable has = new Hashtable();
    Hashtable hat = new Hashtable();
    int selDegree = 0;
    int selBranch = 0;
    int selSec = 0;
    int selCondo = 0;
    string newCollegeCode = string.Empty;
    string newBatchYear = string.Empty;
    string newDegreeCode = string.Empty;
    string newBranchCode = string.Empty;
    string newsemester = string.Empty;

    string qryCollege = string.Empty;
    string qryBatch = string.Empty;
    string qryDegree = string.Empty;
    string qryBranch = string.Empty;
    string qrySem = string.Empty;
    string qrySec = string.Empty;
    private string usercode;
    bool check = false;
    static Hashtable htRowCount = new Hashtable();

    #endregion


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ////****************************************************//
            //string PageLogOut = "";
            //string sess = Convert.ToString(Session["IsLogin"]);
            //PageLogOut = Convert.ToString(Session["PageLogout"]);
            //if (sess == "")
            //{
            //}
            //else
            //{
            //    if (!Request.FilePath.Contains("invM"))
            //    {
            //        string strPreviousPage = "";
            //        if (Request.UrlReferrer != null)
            //        {
            //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            //        }
            //        if (strPreviousPage == "")
            //        {
            //            string redrURI = ConfigurationManager.AppSettings["Inventory"].Trim();
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
            ////****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("inventoryindex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/InventoryMod/inventoryindex.aspx");
                    return;
                }
            }
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindcollege();
                BindDegree();
                BindBatch();
                BindBranch();
                loadkit();
                columnorder.Visible = true;
                pcolumnorder.Visible = true;
                CheckBox_column.Checked = true;
                pheaderfilter.Visible = true;
                LinkButtonsremove_Click(sender, e);
                spreadDet1.Visible = false;
                rptprint.Visible = false;

            }
        }
        catch (Exception ex) { }
    }


    #region College
    public void bindcollege()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCollege.DataSource = ds;
                ddlCollege.DataTextField = "collname";
                ddlCollege.DataValueField = "college_code";
                ddlCollege.DataBind();
            }

        }
        catch
        {
        }
    }
    protected void ddlCollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindBatch();
            BindDegree();
            BindBranch();
            loadkit();
            rptprint.Visible = false;
            spreadDet1.Visible = false;


        }
        catch
        {

        }

    }
    #endregion

    #region Batch
    public void BindBatch()
    {
        try
        {
            chklsbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables.Count > 0)
            {
                int count = ds.Tables[0].Rows.Count;
                if (count > 0)
                {
                    chklsbatch.DataSource = ds;
                    chklsbatch.DataTextField = "batch_year";
                    chklsbatch.DataValueField = "batch_year";
                    chklsbatch.DataBind();
                }
                for (int i = 0; i < chklsbatch.Items.Count; i++)
                {
                    chklsbatch.Items[i].Selected = true;

                }
                txtbatch.Text = lblBatch.Text + "(" + chklsbatch.Items.Count + ")";
                chkbatch.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkbatch_CheckedChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");
        BindDegree();
        BindBranch();
        rptprint.Visible = false;
        spreadDet1.Visible = false;

    }
    protected void chklstbatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(chkbatch, chklsbatch, txtbatch, lblBatch.Text, "--Select--");

        BindDegree();
        BindBranch();
        rptprint.Visible = false;
        spreadDet1.Visible = false;

    }
    #endregion

    #region Degree
    public void BindDegree()
    {
        try
        {

            cblDegree.Items.Clear();
            chkDegree.Checked = false;
            txtDegree.Text = "-- Select --";
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            has.Clear();
            has.Add("single_user", singleuser);
            has.Add("group_code", group_user);
            has.Add("college_code", Convert.ToString(ddlCollege.SelectedValue).Trim());
            has.Add("user_code", usercode);
            ds = d2.select_method("bind_degree", has, "sp");
            if (ds.Tables.Count > 0)
            {
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cblDegree.DataSource = ds;
                    cblDegree.DataTextField = "course_name";
                    cblDegree.DataValueField = "course_id";
                    cblDegree.DataBind();

                    foreach (ListItem li in cblDegree.Items)
                    {
                        li.Selected = true;
                    }
                    txtDegree.Text = "Degree" + "(" + cblDegree.Items.Count + ")";
                    chkDegree.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkDegree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            if (chkDegree.Checked == true)
            {
                count++;
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = true;
                }
                txtDegree.Text = "Degree (" + (cblDegree.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblDegree.Items.Count; i++)
                {
                    cblDegree.Items[i].Selected = false;
                }
                txtDegree.Text = "-- Select --";
            }
            BindBranch();
            rptprint.Visible = false;
            spreadDet1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    protected void cblDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtDegree.Text = "-- Select --";
            chkDegree.Checked = false;
            for (int i = 0; i < cblDegree.Items.Count; i++)
            {
                if (cblDegree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblDegree.Items.Count)
                {
                    chkDegree.Checked = true;
                }
                txtDegree.Text = "Degree (" + Convert.ToString(commcount) + ")";
            }
            BindBranch();
            rptprint.Visible = false;
            spreadDet1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }
    #endregion


    #region Branch
    public void BindBranch()
    {
        try
        {

            chkBranch.Checked = false;
            cblBranch.Items.Clear();
            txtBranch.Text = "-- Select --";
            hat.Clear();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = Convert.ToString(group_semi[0]).Trim();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);


            selDegree = 0;
            newDegreeCode = string.Empty;
            qryDegree = string.Empty;
            string coursecode = string.Empty;
            foreach (ListItem li in cblDegree.Items)
            {
                if (li.Selected)
                {
                    selDegree++;
                    if (string.IsNullOrEmpty(newDegreeCode.Trim()))
                    {
                        newDegreeCode = "'" + li.Value + "'";
                    }
                    else
                    {
                        newDegreeCode += ",'" + li.Value + "'";
                    }
                }
            }
            if (selDegree > 0)
            {
                coursecode = " and degree.course_id in(" + newDegreeCode + ")";

                string strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' and user_code='" + usercode + "' " + " " + coursecode + "";
                if ((Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
                {
                    strquery = "select distinct degree.degree_code,de.dept_name from degree,course, deptprivilages,department de where course.course_id=degree.course_id and de.dept_code=degree.dept_code and course.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "' and course.college_code ='" + Convert.ToString(ddlCollege.SelectedValue).Trim() + "' " + "  " + coursecode + "";
                }
                ds = d2.select_method_wo_parameter(strquery, "Text");
            }
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int count2 = ds.Tables[0].Rows.Count;
                if (count2 > 0)
                {
                    cblBranch.DataSource = ds;
                    cblBranch.DataTextField = "dept_name";
                    cblBranch.DataValueField = "degree_code";
                    cblBranch.DataBind();

                    foreach (ListItem li in cblBranch.Items)
                    {
                        li.Selected = true;
                    }

                    txtBranch.Text = "Branch" + "(" + cblBranch.Items.Count + ")";
                    chkBranch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void chkBranch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {

            int count = 0;
            if (chkBranch.Checked == true)
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    count++;
                    cblBranch.Items[i].Selected = true;
                }
                txtBranch.Text = "Branch (" + (cblBranch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblBranch.Items.Count; i++)
                {
                    cblBranch.Items[i].Selected = false;
                }
                txtBranch.Text = "-- Select --";
            }
            rptprint.Visible = false;
            spreadDet1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }

    protected void cblBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txtBranch.Text = "-- Select --";
            chkBranch.Checked = false;
            for (int i = 0; i < cblBranch.Items.Count; i++)
            {
                if (cblBranch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cblBranch.Items.Count)
                {
                    chkBranch.Checked = true;
                }
                txtBranch.Text = "Branch (" + Convert.ToString(commcount) + ")";
            }
            rptprint.Visible = false;
            spreadDet1.Visible = false;

        }
        catch (Exception ex)
        {

        }
    }
    #endregion

    #region Load_Kit_name
    public void loadkit()
    {
        try
        {
            string appno = string.Empty;
            string q1 = "";
            string coll_code = "";
            cbl_kitname.Items.Clear();
            if (ddlCollege.Items.Count > 0)
                coll_code = Convert.ToString(ddlCollege.SelectedValue);
            if (!string.IsNullOrEmpty(coll_code))
            {
                q1 = " select distinct cm.MasterValue,cm.MasterCode from IM_StudentKit_Details sd,IM_KitMaster km,CO_MasterValues cm  where sd.KitCode=km.KitCode and cm.CollegeCode=km.CollegeCode and cm.MasterCode=sd.KitCode and km.KitCode=cm.MasterCode and km.ItemCode=sd.ItemCode and cm.CollegeCode='" + coll_code + "'";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_kitname.DataSource = ds;
                cbl_kitname.DataTextField = "MasterValue";
                cbl_kitname.DataValueField = "MasterCode";
                cbl_kitname.DataBind();
                //cbl_section.Items.Insert(0, new ListItem(" ", " "));

            }
            for (int i = 0; i < cbl_kitname.Items.Count; i++)
            {
                cbl_kitname.Items[i].Selected = true;

            }
            txt_kitname.Text = lbl_kitname.Text + "(" + cbl_kitname.Items.Count + ")";
            cb_kitname.Checked = true;
        }
        catch
        {

        }


    }

    protected void cb_kitname_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }

    protected void cbl_kitname_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_kitname, cbl_kitname, txt_kitname, "Kit Name", "--Select--");


    }
    #endregion

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetRoll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct r.Roll_No from Registration r where  r.roll_no  like '" + prefixText + "%' order by Roll_No";
        name = ws.Getname(query);
        return name;
    }

    protected void txt_roll_changed(object sender, EventArgs e)
    {
        try
        {
            spreadDet1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void txt_item_changed(object sender, EventArgs e)
    {
        try
        {
            spreadDet1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    protected void txt_kit_changed(object sender, EventArgs e)
    {
        try
        {
            spreadDet1.Visible = false;
            rptprint.Visible = false;
        }
        catch
        {

        }

    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetItemname(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        string query = "select distinct itemname from IM_ItemMaster WHERE itemname like '" + prefixText + "%' ";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["itemname"].ToString());
            }
        }
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetKitname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        query = "select distinct MasterValue from CO_MasterValues where MasterCriteria='Kit' and MasterValue  like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    protected void LinkButtonsremove_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    #region Go
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            ds = getstudentdetails();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                loadspread(ds);
            }
            else
            {
                alertimg.Visible = true;
                lbl_alert.Text = "No Records Found";

            }
        }
        catch
        {

        }


    }


    #endregion

    #region Fspread
    private DataSet getstudentdetails()
    {

        DataSet dsload = new DataSet();
        try
        {
            #region get Value
            string collegecode = string.Empty;
            string batch = string.Empty;
            string courseid = string.Empty;
            string dept = string.Empty;
            string kit = string.Empty;


            if (ddlCollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlCollege.SelectedValue);
            if (chklsbatch.Items.Count > 0)
                batch = Convert.ToString(d2.getCblSelectedValue(chklsbatch));
            if (cblDegree.Items.Count > 0)
                courseid = Convert.ToString(d2.getCblSelectedValue(cblDegree));
            if (cblBranch.Items.Count > 0)
                dept = Convert.ToString(d2.getCblSelectedValue(cblBranch));
            if (cbl_kitname.Items.Count > 0)
                kit = Convert.ToString(d2.getCblSelectedValue(cbl_kitname));

            string Rollno = Convert.ToString(txt_roll.Text);
            string itemname = Convert.ToString(txt_item.Text);
            string kitname = Convert.ToString(txt_kit.Text);


            string selQ = string.Empty;
            if (!string.IsNullOrEmpty(collegecode) && !string.IsNullOrEmpty(batch) && !string.IsNullOrEmpty(courseid) && !string.IsNullOrEmpty(dept) && !string.IsNullOrEmpty(kit))
            {
                if (Rollno != "")
                {

                    selQ = "select distinct sd.Stu_AppNo,r.Roll_No,r.Roll_Admit,r.reg_no,r.Stud_Name,dp.Dept_Name,mv.MasterValue,i.ItemCode,i.ItemHeaderName,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date,convert(varchar(10),r.Batch_Year)+'-'+dp.dept_acronym+'-'++'Sem'+convert(varchar(10),r.Current_Semester)+'-'+r.Sections as degree   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and r.Roll_No='" + Rollno + "'  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date,r.Batch_Year,dp.dept_acronym,r.Current_Semester,r.Sections,r.Roll_Admit,dp.Dept_Name,i.ItemHeaderName";

                }
                else if (itemname.Trim() != "")
                {

                    selQ = "select distinct sd.Stu_AppNo,r.Roll_No,r.Roll_Admit,r.reg_no,r.Stud_Name,dp.Dept_Name,mv.MasterValue,i.ItemCode,i.ItemHeaderName,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date,convert(varchar(10),r.Batch_Year)+'-'+dp.dept_acronym+'-'++'Sem'+convert(varchar(10),r.Current_Semester)+'-'+r.Sections as degree   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and i.ItemName='" + itemname + "'  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date,r.Batch_Year,dp.dept_acronym,r.Current_Semester,r.Sections,r.Roll_Admit,dp.Dept_Name,i.ItemHeaderName";


                }
                else if (kitname.Trim() != "")
                {
                    selQ = "select distinct sd.Stu_AppNo,r.Roll_No,r.Roll_Admit,r.reg_no,r.Stud_Name,dp.Dept_Name,mv.MasterValue,i.ItemCode,i.ItemHeaderName,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date,convert(varchar(10),r.Batch_Year)+'-'+dp.dept_acronym+'-'++'Sem'+convert(varchar(10),r.Current_Semester)+'-'+r.Sections as degree   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and mv.MasterValue='" + kitname + "'  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date,r.Batch_Year,dp.dept_acronym,r.Current_Semester,r.Sections,r.Roll_Admit,dp.Dept_Name,i.ItemHeaderName";

                }
                else
                {
                   selQ = "  select distinct sd.Stu_AppNo,r.Roll_No,r.Roll_Admit,r.reg_no,r.Stud_Name,dp.Dept_Name,mv.MasterValue,i.ItemCode,i.ItemHeaderName,i.ItemName,sd.Qty,sum(se.BalQty)as BalQty,sm.StorePK,i.ItemPK,se.InwardRPU,mv.MasterCode,sd.Date,convert(varchar(10),r.Batch_Year)+'-'+dp.dept_acronym+'-'++'Sem'+convert(varchar(10),r.Current_Semester)+'-'+r.Sections as degree   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and r.batch_year in('" + batch + "') and  c.Course_Id in('" + courseid + "')  and r.degree_code in('" + dept + "') and r.college_code ='" + collegecode + "' and mv.MasterCode in('" + kit + "')  group by sd.Stu_AppNo,r.Roll_No,r.reg_no,r.Stud_Name,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,BalQty,sm.StorePK,i.ItemPK,se.InwardRPU, mv.MasterCode,sd.Date,r.Batch_Year,dp.dept_acronym,r.Current_Semester,r.Sections,r.Roll_Admit,dp.Dept_Name,i.ItemHeaderName";

                }
                dsload.Clear();
                dsload = d2.select_method_wo_parameter(selQ, "Text");

            }
            #endregion
        }
        catch (Exception ex)
        { }

        return dsload;
    }

    public void loadspread(DataSet ds)
    {
        try
        {
            DataView dv = new DataView();
            DataSet dskit = new DataSet();
            spreadDet1.Sheets[0].RowCount = 0;
            spreadDet1.Sheets[0].ColumnCount = 10;
            spreadDet1.CommandBar.Visible = false;
            spreadDet1.Sheets[0].AutoPostBack = false;
            spreadDet1.Sheets[0].ColumnHeader.RowCount = 1;
            spreadDet1.Sheets[0].RowHeader.Visible = false;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].Columns[0].Locked = true;
            spreadDet1.Columns[0].Width = 50;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Columns[1].Width = 50;
            spreadDet1.Columns[1].Visible = true;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Columns[2].Width = 160;
            spreadDet1.Columns[2].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[3].Locked = true;
            spreadDet1.Columns[3].Width = 100;
            spreadDet1.Columns[3].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[4].Locked = true;
            spreadDet1.Columns[4].Width = 150;
            spreadDet1.Columns[4].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Kit Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[5].Locked = true;
            spreadDet1.Columns[5].Width = 100;
            spreadDet1.Columns[5].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Item Name";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[6].Locked = true;
            spreadDet1.Columns[6].Width = 200;
            spreadDet1.Columns[6].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Alloted Qty";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[7].Locked = true;
            spreadDet1.Columns[7].Width = 60;
            spreadDet1.Columns[7].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Issued Qty";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[8].Locked = true;
            spreadDet1.Columns[8].Width = 60;
            spreadDet1.Columns[8].Visible = false;

            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Balance Qty";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
            spreadDet1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
            spreadDet1.Sheets[0].Columns[9].Locked = true;
            spreadDet1.Columns[9].Width = 60;
            spreadDet1.Columns[9].Visible = false;

            FarPoint.Web.Spread.TextCellType txtCell = new FarPoint.Web.Spread.TextCellType();
            int sno = 0;
            int allot = 0;
            int issue = 0;
            int bal = 0;
            bool chk3 = false;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            spreadDet1.Sheets[0].RowCount = spreadDet1.Sheets[0].RowCount + 1;
            spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = chkcell1;
            spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
            chkcell1.AutoPostBack = true;
            int row1 = 0;
            int rowCnt = 0;
            Hashtable htkit1 = new Hashtable();
            DataSet dsfilter = new DataSet();
            htRowCount.Clear();
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    string appno = Convert.ToString(ds.Tables[0].Rows[row]["Stu_AppNo"]).Trim();
                    int rowcnt1 = 0;
                    if (!htkit1.Contains(appno))
                    {
                        htkit1.Add(appno, "");
                        ds.Tables[0].DefaultView.RowFilter = "Stu_AppNo ='" + appno + "'";
                        dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            int rowCnt1 = 0;
                            for (int row2 = 0; row2 < dv.Count; row2++)
                            {
                                if (row2 == 0)
                                    rowCnt1 = spreadDet1.Sheets[0].RowCount;
                                string degree = Convert.ToString(dv[row2]["degree"]).Trim();
                                string admno = Convert.ToString(dv[row2]["Roll_Admit"]).Trim();

                                string deptname = Convert.ToString(dv[row2]["Dept_Name"]).Trim();
                                string rollno = Convert.ToString(dv[row2]["roll_no"]).Trim();
                                string stuname = Convert.ToString(dv[row2]["Stud_Name"]).Trim();
                                string kitname = Convert.ToString(dv[row2]["MasterValue"]).Trim();
                                string itemheadname = Convert.ToString(dv[row2]["ItemHeaderName"]).Trim();
                                string itmname = Convert.ToString(dv[row2]["ItemName"]).Trim();
                                string itempk = Convert.ToString(dv[row2]["ItemPK"]).Trim();
                                string Allotqty = Convert.ToString(dv[row2]["Qty"]).Trim();
                                allot = Convert.ToInt32(Allotqty);
                                string balqty = "";
                                string issueQty = d2.GetFunction("select IssuedQuantity from Indivitual_student_ItemIssue where App_no='" + appno + "' and ItemFK='" + itempk + "' and Kit='1'");
                                if (issueQty != "")
                                {
                                    string[] issqty = issueQty.Split('.');
                                    issue = Convert.ToInt32(issqty[0]);
                                    bal = allot - issue;
                                    balqty = Convert.ToString(bal);
                                }
                                spreadDet1.Sheets[0].RowCount++;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].CellType = chkcell1;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].CellType = txtCell;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].CellType = txtCell;

                                if (sno == 0)
                                    sno = 1;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sno);
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Tag = appno;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Tag = itemheadname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Text = degree;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Tag = admno;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Text = rollno;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Tag = deptname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Text = stuname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Tag = stuname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Text = kitname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Tag = kitname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].Text = itmname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].Tag = itmname;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].Text = Allotqty;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].Tag = Allotqty;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(issue);
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(issue);
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].Text = balqty;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].Tag = balqty;


                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;

                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].VerticalAlign = VerticalAlign.Middle;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].VerticalAlign = VerticalAlign.Middle;



                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 0].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 1].Locked = false;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 2].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 3].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 4].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 5].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 6].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 7].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 8].Locked = true;
                                spreadDet1.Sheets[0].Cells[spreadDet1.Sheets[0].RowCount - 1, 9].Locked = true;
                                rowcnt1++;

                            }

                            if (rowcnt1 > 0)
                            {
                                spreadDet1.Sheets[0].SpanModel.Add(rowCnt1, 0, rowcnt1, 1);
                                spreadDet1.Sheets[0].SpanModel.Add(rowCnt1, 1, rowcnt1, 1);
                                spreadDet1.Sheets[0].SpanModel.Add(rowCnt1, 2, rowcnt1, 1);
                                spreadDet1.Sheets[0].SpanModel.Add(rowCnt1, 3, rowcnt1, 1);
                                spreadDet1.Sheets[0].SpanModel.Add(rowCnt1, 4, rowcnt1, 1);
                                htRowCount.Add(appno, rowcnt1);
                                sno++;

                            }
                        }


                    }

                }

                #region columnorder
                if (cblcolumnorder.Items.Count > 0)
                {
                    for (int k1 = 0; k1 < cblcolumnorder.Items.Count; k1++)
                    {
                        if (cblcolumnorder.Items[k1].Selected == true)
                        {
                            string headername1 = Convert.ToString(cblcolumnorder.Items[k1].ToString());

                            if (headername1 == "Degree")
                            {
                                spreadDet1.Columns[2].Visible = true;

                            }
                            else if (headername1 == "Roll No")
                            {
                                spreadDet1.Columns[3].Visible = true;

                            }
                            else if (headername1 == "Student Name")
                            {
                                spreadDet1.Columns[4].Visible = true;

                            }
                            else if (headername1 == "Kit Name")
                            {
                                spreadDet1.Columns[5].Visible = true;

                            }

                            else if (headername1 == "Item Name")
                            {
                                spreadDet1.Columns[6].Visible = true;

                            }
                            else if (headername1 == "Alloted Qty")
                            {
                                spreadDet1.Columns[7].Visible = true;

                            }
                            else if (headername1 == "Issued Qty")
                            {
                                spreadDet1.Columns[8].Visible = true;
                            }
                            else if (headername1 == "Balance Qty")
                            {
                                spreadDet1.Columns[9].Visible = true;
                            }
                            chk3 = true;
                        }
                    }
                }
                if (chk3 == false)
                {
                    CheckBox_column.Checked = true;


                    for (int k2 = 0; k2 < cblcolumnorder.Items.Count; k2++)
                    {
                        //if (cblcolumnorder.Items[k].Selected == true)
                        //{
                        string headername2 = Convert.ToString(cblcolumnorder.Items[k2].ToString());

                        if (headername2 == "Degree")
                        {
                            spreadDet1.Columns[2].Visible = true;

                        }
                        else if (headername2 == "Roll No")
                        {
                            spreadDet1.Columns[3].Visible = true;

                        }
                        else if (headername2 == "Student Name")
                        {
                            spreadDet1.Columns[4].Visible = true;

                        }
                        else if (headername2 == "Kit Name")
                        {
                            spreadDet1.Columns[5].Visible = true;

                        }

                        else if (headername2 == "Item Name")
                        {
                            spreadDet1.Columns[6].Visible = true;

                        }
                        else if (headername2 == "Alloted Qty")
                        {
                            spreadDet1.Columns[7].Visible = true;

                        }
                        else if (headername2 == "Issued Qty")
                        {
                            spreadDet1.Columns[8].Visible = true;
                        }
                        else if (headername2 == "Balance Qty")
                        {
                            spreadDet1.Columns[9].Visible = true;
                        }

                    }
                }
                #endregion
                spreadDet1.Sheets[0].PageSize = spreadDet1.Sheets[0].RowCount;
                spreadDet1.SaveChanges();
                spreadDet1.Height = 280;
                spreadDet1.Width = 950;
                spreadDet1.Visible = true;
                rptprint.Visible = true;
            }
        }

        catch
        {

        }

    }

    protected void spreadDet1_UpdateCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        //Fpspread2.Visible = true;
        try
        {
            string actrow = spreadDet1.Sheets[0].ActiveRow.ToString();
            string actcol = spreadDet1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (spreadDet1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(spreadDet1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < spreadDet1.Sheets[0].RowCount; i++)
                        {
                            spreadDet1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < spreadDet1.Sheets[0].RowCount; i++)
                        {
                            spreadDet1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "Individual_StudentFeeStatus"); 
        }
    }

    #endregion


    #region Print

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Student Kit Report";
            string pagename = "Student_Kit_Report.aspx";
            Printcontrol.loadspreaddetails(spreadDet1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {
        }
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(spreadDet1, reportname);
                lblvalidation1.Visible = false;
            }
            else
            {
                lblvalidation1.Text = "Please Enter Your Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        {
        }
    }
    #endregion

    #region Individual_Print
    protected void btn_individual_Print_Click(object sender, EventArgs e)
    {
        try
        {
            int selectedcount = 0;
            if (spreadDet1.Rows.Count > 0)
            {
                string StudAppno = "";
                spreadDet1.SaveChanges();
                for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                {
                    int checkval1 = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 1].Value);
                    if (checkval1 == 1)
                    {
                        selectedcount++;
                    }
                }
                if (selectedcount == 0)
                {
                    alertimg.Visible = true;
                    lbl_alert.Text = "Please Select the Student and then Proceed";
                    return;

                }
                else
                {
                    IndividualStudentDetails();
                    btn_go_Click(sender, e);
                }
            }


        }
        //string app_no = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 2].Tag);
        //           if (app_no != "")
        //           {
        //               int rowCnt = 0;
        //               if (htRowCount.ContainsKey(app_no))
        //               {
        //                   if (StudAppno != app_no)
        //                   {

             //                   }
        //               }
        //           }
        catch
        {

        }


    }
    #endregion


    #region Individual_Student_Print
   

    public void IndividualStudentDetails()
    {

        try
        {
            int g = 1;
            string collgr = string.Empty;
            string colldetails = string.Empty;
            string collname = string.Empty;
            string pincode = string.Empty;
            string district = string.Empty;
            string Date = string.Empty;
            int mm = 0;
            int y = 0;
            string HallNo = string.Empty;
            string session = string.Empty;
            string hdeg = "", hroll = "", bndlee = string.Empty;
            string batch = string.Empty;
            string subno = string.Empty;
            string hall = string.Empty;
            DataSet dsdisplay = new DataSet();

            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            Font Fontbold = new Font("Book Antique", 10, FontStyle.Bold);
            Font Fontnormal = new Font("Book Antique", 10, FontStyle.Regular);
            Font Fonttitle = new Font("Book Antique", 9, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antique", 8, FontStyle.Regular);
            Font Fonthead = new Font("Book Antique", 10, FontStyle.Regular);
            Font head = new Font("Book Antique", 16, FontStyle.Bold);
            Font fontCoverNo = new Font("IDAutomationHC39M", 10, FontStyle.Bold);
            Boolean chkgenflag = false;
            DateTime dt = new DateTime();
            int coltop = 10;
            coltop = coltop + 5;
            int coltop1 = coltop;
            int finctop = coltop;
            int yq = 180;
            string strquery = string.Empty;
            int isval = 0;
            int ji = 0;
            int tablepadding = 10;
            bool chek1 = false;
            if (spreadDet1.Rows.Count > 0)
            {
                string StudAppno = "";
                spreadDet1.SaveChanges();
                for (int row = 0; row < spreadDet1.Sheets[0].RowCount; row++)
                {
                    int checkval1 = Convert.ToInt32(spreadDet1.Sheets[0].Cells[row, 1].Value);
                    if (checkval1 == 1)
                    {
                        chek1 = true;
                        string app_no = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 0].Tag);
                        if (app_no != "")
                        {
                            int rowCnt = 0;
                            if (htRowCount.ContainsKey(app_no))
                            {
                                if (StudAppno != app_no)
                                {
                                    int.TryParse(Convert.ToString(htRowCount[app_no]), out rowCnt);
                                    string admino = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 2].Tag);
                                    string depname = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 3].Tag);
                                    string StudentName = Convert.ToString(spreadDet1.Sheets[0].Cells[row, 4].Tag);
                                    string collcode = Convert.ToString(ddlCollege.SelectedValue).Trim();
                                    strquery = "select collname,address1+''+address2+''+address3+'-'+pincode as CollDetails  from collinfo where college_code='" + collcode + "'";
                                    DataSet ds = d2.select_method_wo_parameter(strquery, "Text");

                                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                    {
                                        ds = d2.select_method_wo_parameter(strquery, "Text");
                                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        {
                                            collname = ds.Tables[0].Rows[0]["collname"].ToString();
                                            colldetails = ds.Tables[0].Rows[0]["CollDetails"].ToString();
                                            int u = 0;

                                            PdfArea tete = new PdfArea(mydocument, 15, 10, 565, 825);
                                            PdfRectangle pr1 = new PdfRectangle(mydocument, tete, Color.Black);
                                            PdfTextArea ptc;

                                          
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collcode + ".jpeg")))
                                            {
                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Right_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 35, 25, 700);
                                            }
                                            if (File.Exists(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collcode + ".jpeg")))
                                            {
                                                PdfImage LogoImage = mydocument.NewImage(HttpContext.Current.Server.MapPath("~/FinanceLogo/Left_Logo" + collcode + ".jpeg"));
                                                mypdfpage.Add(LogoImage, 525, 25, 700);
                                            }
                                            ptc = new PdfTextArea(head, System.Drawing.Color.Black,
                                                                                       new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, collname);
                                            mypdfpage.Add(ptc);
                                            coltop = coltop + 15;
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter, colldetails);
                                            mypdfpage.Add(ptc);

                                            coltop = coltop + 15;
                                          
                                            coltop = coltop + 10;
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 0, coltop, 600, 50), System.Drawing.ContentAlignment.MiddleCenter,
                                                                                    "______________________________________________________________________________________________________");
                                            mypdfpage.Add(ptc);
                                            //Line2
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                    new PdfArea(mydocument, 35, 80, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Dear Parents,");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 35, 90, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Greetings from " + collname + "!!");
                                            mypdfpage.Add(ptc);

                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                               new PdfArea(mydocument, 35, 100, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "We are glad to provide the following items for 2018-19 academic year.");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                            new PdfArea(mydocument, 35, 90, 400, 100), System.Drawing.ContentAlignment.MiddleLeft, "If you have any problem in the items, please return and rectify within 10 days of the issue (Uniform, Shoe &Socks to be checked at the time of issue). ");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 35, 130, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "We will be happy to provide quality items to your satisfaction.");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 35, 140, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purchase & Store Team.");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                           new PdfArea(mydocument, 35, 150, 600, 50), System.Drawing.ContentAlignment.MiddleLeft, ""+ collname +"");
                                            mypdfpage.Add(ptc);
                                            //Line3 Student Details

                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 35, 190, 600, 50), System.Drawing.ContentAlignment.TopLeft, "NAME OF THE STUDENT");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 160, 190, 180, 100), System.Drawing.ContentAlignment.TopLeft, ":  " + StudentName);
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 325, 190, 600, 50), System.Drawing.ContentAlignment.TopLeft, "ADMISSION NO");
                                            mypdfpage.Add(ptc);
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 420, 190, 600, 50), System.Drawing.ContentAlignment.TopLeft, ":  " + admino);
                                            mypdfpage.Add(ptc);
                                            //coltop = coltop + 25;
                                            ptc = new PdfTextArea(Fontnormal, System.Drawing.Color.Black,
                                                                                  new PdfArea(mydocument, 35, 210, 600, 50), System.Drawing.ContentAlignment.TopLeft, "" + depname);
                                            mypdfpage.Add(ptc);



                                            int sno = 1;
                                            Gios.Pdf.PdfTable table1 = mydocument.NewTable(Fontbold, rowCnt + 21, 6, 10);
                                            table1.VisibleHeaders = false;

                                            table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetContent("S.No");
                                            table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 0).SetFont(Fontbold);
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetContent("Kit Name");
                                            table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 1).SetFont(Fontbold);
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetContent("Alloted Qty");
                                            table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 2).SetFont(Fontbold);
                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetContent("Issue Qty");
                                            table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 3).SetFont(Fontbold);
                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 4).SetContent("Balance Qty");
                                            table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table1.Cell(0, 4).SetFont(Fontbold);

                                            table1.Columns[0].SetWidth(20);
                                            table1.Columns[1].SetWidth(100);
                                            table1.Columns[2].SetWidth(50);
                                            table1.Columns[3].SetWidth(50);
                                            table1.Columns[4].SetWidth(50);
                                            table1.Columns[5].SetWidth(1);
                                            table1.Columns[5].SetColors(Color.White, Color.White);
                                            table1.Rows[20].SetRowHeight(1.0);
                                            //table1.Columns[5].MergeCells();
                                           
                                            int tablerow = 4;
                                            int rowindextbl1 = 0;
                                            string Prekitcode = "";
                                            string preitemheadcode = "";

                                            DataSet dsprt = new DataSet();
                                            DataView dvprt = new DataView();
                                            DataView dvprt1 = new DataView();
                                            int allot = 0;
                                            int issue = 0;
                                            int bal = 0;
                                            int g1 = 0;
                                            string prevkit = "";
                                            string prevhe = "";
                                            string sqlgetkit = "select distinct sd.Stu_AppNo,mv.MasterValue,i.ItemHeaderName,i.ItemCode,i.ItemName,sd.Qty,i.itempk,i.ItemHeaderCode,mv.MasterCode   from registration r,degree de,course c,department dp,IM_KitMaster km,IM_StudentKit_Details sd,IM_ItemMaster i,CO_MasterValues mv,IT_StockDetail se,IM_StoreMaster sm   where km.CollegeCode=r.college_code and km.CollegeCode=mv.CollegeCode and sd.ItemCode=i.ItemCode and sd.Stu_AppNo=r.App_No and km.ItemCode=sd.ItemCode and km.ItemCode=i.ItemCode    and km.KitCode=mv.MasterCode and sd.KitCode=mv.MasterCode and km.CollegeCode=mv.CollegeCode and r.degree_code=de.degree_code and c.course_id=de.course_id and dp.dept_code = de.dept_code and r.delflag=0 and r.cc=0 and r.exam_flag<>'debar'  and se.ItemFK=i.ItemPK and sm.StorePK=se.StoreFK and i.StoreFK=sm.StorePK and r.App_No='" + app_no + "'  group by sd.Stu_AppNo,mv.MasterValue,i.ItemCode,i.ItemName,sd.Qty,i.ItemHeaderName,i.itempk,i.ItemHeaderCode,mv.MasterCode ";
                                            dsprt.Clear();
                                            dsprt = d2.select_method_wo_parameter(sqlgetkit, "Text");
                                            if (dsprt.Tables.Count > 0 && dsprt.Tables[0].Rows.Count > 0)
                                            {
                                                for (int srow = 0; srow < dsprt.Tables[0].Rows.Count; srow++)
                                                {
                                                   

                                                    //string itemheadname = Convert.ToString(dv[row2]["ItemHeaderName"]).Trim();

                                                    string kitcode = Convert.ToString(dsprt.Tables[0].Rows[srow]["MasterCode"]);
                                                    string kit = Convert.ToString(dsprt.Tables[0].Rows[srow]["MasterValue"]).Trim();
                                                    if (prevkit != kit)
                                                    {
                                                        table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                        table1.Cell(g, 1).SetContent("kit-"+kit.ToString());
                                                        table1.Cell(g, 1).SetFont(Fontbold);
                                                       
                                                       
                                                        g = g + 1;
                                                        prevkit = kit;
                                                    }
                                                    if (Prekitcode != kitcode)
                                                    {
                                                        dsprt.Tables[0].DefaultView.RowFilter = "MasterCode ='" + kitcode + "'";
                                                        dvprt = dsprt.Tables[0].DefaultView;
                                                        if (dvprt.Count > 0)
                                                        {
                                                            for (int row2 = 0; row2 < dvprt.Count; row2++)
                                                            {

                                                                string itemhecode = Convert.ToString(dvprt[row2]["ItemHeaderCode"]).Trim();
                                                                string ithead = Convert.ToString(dvprt[row2]["ItemHeaderName"]).Trim();

                                                                if (Prekitcode != kitcode)
                                                                {
                                                                    if (prevhe != ithead || prevhe == ithead)
                                                                    {

                                                                        table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                        table1.Cell(g, 1).SetContent("Header-"+ithead.ToString());
                                                                        table1.Cell(g, 1).SetFont(Fontbold);
                                                                        g = g + 1;
                                                                        prevhe = ithead;
                                                                    }
                                                                }

                                                                if (Prekitcode != kitcode)
                                                                {
                                                                    if (preitemheadcode != itemhecode || preitemheadcode == itemhecode)
                                                                    {
                                                                        dsprt.Tables[0].DefaultView.RowFilter = "ItemHeaderCode ='" + itemhecode + "' and MasterCode ='" + kitcode + "'";
                                                                        dvprt1 = dsprt.Tables[0].DefaultView;
                                                                        if (dvprt1.Count > 0)
                                                                        {
                                                                            int allotqty = 0;
                                                                            int isueqty = 0;
                                                                            int blqty = 0;

                                                                            for (int row3 = 0; row3 < dvprt1.Count; row3++)
                                                                            {
                                                                                string itmname = Convert.ToString(dvprt1[row3]["ItemName"]).Trim();
                                                                                string itempk = Convert.ToString(dvprt1[row3]["ItemPK"]).Trim();
                                                                                string Allotqty = Convert.ToString(dvprt1[row3]["Qty"]).Trim();
                                                                                allot = Convert.ToInt32(Allotqty);
                                                                                string balqty = "";
                                                                                string issueQty = d2.GetFunction("select IssuedQuantity from Indivitual_student_ItemIssue where App_no='" + app_no + "' and ItemFK='" + itempk + "' and Kit='1'");
                                                                                if (issueQty != "")
                                                                                {
                                                                                    string[] issqty = issueQty.Split('.');
                                                                                    issue = Convert.ToInt32(issqty[0]);
                                                                                    bal = allot - issue;
                                                                                    balqty = Convert.ToString(bal);
                                                                                }


                                                                                table1.Cell(g, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                table1.Cell(g, 0).SetContent(sno.ToString());
                                                                                table1.Cell(g, 0).SetFont(Fontnormal);
                                                                                table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                table1.Cell(g, 1).SetContent("Item-"+itmname.ToString());
                                                                                table1.Cell(g, 1).SetFont(Fontnormal);

                                                                                table1.Cell(g, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                table1.Cell(g, 2).SetContent(Allotqty.ToString());
                                                                                table1.Cell(g, 2).SetFont(Fontnormal);
                                                                                table1.Cell(g, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                table1.Cell(g, 3).SetContent(issue.ToString());
                                                                                table1.Cell(g, 3).SetFont(Fontnormal);
                                                                                table1.Cell(g, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                                table1.Cell(g, 4).SetContent(balqty.ToString());
                                                                                table1.Cell(g, 4).SetFont(Fontnormal);
                                                                                g = g + 1;
                                                                                sno++;
                                                                                allotqty += allot;
                                                                                isueqty += issue;
                                                                                blqty += Convert.ToInt32(balqty);
                                                                            }
                                                                            table1.Cell(g, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            table1.Cell(g, 1).SetContent("Total");
                                                                            table1.Cell(g, 1).SetFont(Fontbold);

                                                                            table1.Cell(g, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            table1.Cell(g, 2).SetContent(allotqty.ToString());
                                                                            table1.Cell(g, 2).SetFont(Fontnormal);
                                                                            table1.Cell(g, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            table1.Cell(g, 3).SetContent(isueqty.ToString());
                                                                            table1.Cell(g, 3).SetFont(Fontnormal);
                                                                            table1.Cell(g, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                                                            table1.Cell(g, 4).SetContent(blqty.ToString());
                                                                            table1.Cell(g, 4).SetFont(Fontnormal);
                                                                            g = g + 1;

                                                                        }

                                                                        preitemheadcode = itemhecode;
                                                                    }
                                                                }
                                                            }


                                                        }
                                                        Prekitcode = kitcode;
                                                    }
                                                }
                                                int span1 = 0;
                                                if (g < 20)
                                                {
                                                    span1 = 20 - g;
                                                    span1 = span1 + 2;
                                                    table1.Cell(g, 0).RowSpan = span1;
                                                    table1.Cell(g, 1).RowSpan = span1;
                                                    table1.Cell(g, 2).RowSpan = span1;
                                                    table1.Cell(g, 3).RowSpan = span1;
                                                    table1.Cell(g, 4).RowSpan = span1;
                                                    table1.Rows[g].MergeCells();

                                                    //table1.Rows[span1 + 1].MergeCells();

                                                }


                                                Gios.Pdf.PdfTablePage newpdftabpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(mydocument, 20, 230, 550, 750));
                                                mypdfpage.Add(newpdftabpage1);
                                                mypdfpage.Add(pr1);

                                                PdfTextArea pdfSignExaminer = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 775, 200, 50), ContentAlignment.MiddleLeft, "Signature of the Parent/Student :");
                                                mypdfpage.Add(pdfSignExaminer);
                                                PdfTextArea pdfDate = new PdfTextArea(Fontbold, Color.Black, new PdfArea(mydocument, 25, 795, 200, 50), ContentAlignment.MiddleLeft, "Date\t\t:\t\t");
                                                mypdfpage.Add(pdfDate);

                                                g = 1;
                                                if (yq >= 180)
                                                {
                                                    mypdfpage.SaveToDocument();
                                                    mypdfpage = mydocument.NewPage();
                                                    yq = 180;
                                                }
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                }

                string appPath = HttpContext.Current.Server.MapPath("~");
                if (appPath != "")
                {
                    string szPath = appPath + "/Report/";
                    string szFile = "Student_Kit_Report" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                    mydocument.SaveToFile(szPath + szFile);
                    Response.ClearHeaders();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                    Response.ContentType = "application/pdf";
                    Response.WriteFile(szPath + szFile);
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
    #endregion

    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        try
        {
            alertimg.Visible = false;
        }
        catch
        {

        }
    }
}