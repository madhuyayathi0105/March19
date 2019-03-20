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
public partial class breakage_entry : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    bool check = false;
    bool check1 = false;
    bool guestcheck = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    static string collegecodestat = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    DataTable dt = new DataTable();
    DataTable dt2 = new DataTable();
    Hashtable hat = new Hashtable();
    string sqladd = "";
    string Rollflag1 = string.Empty;
    DataRow dr;
    string Roll_No = "";
    string Item_Name = "";
    string Staff_Code = "";
    static string checknew = "";

    string itemcode = "";
    string itempk = "";
    string name = "";
    string measure = "";
    string dept = "";
    string deptcode = "";
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
            collegecode1 = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            lblvalidation1.Text = "";
            cext_fromdate.EndDate = DateTime.Now;
            cext_todate.EndDate = DateTime.Now;
            caltodate.EndDate = DateTime.Now;

            if (!IsPostBack)
            {
                bindhostelhostel();
                txt_fromdate.Attributes.Add("readonly", "readonly");
                txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_todate.Attributes.Add("readonly", "readonly");
                txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_date.Attributes.Add("readonly", "readonly");
                txt_date.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //  Fpitem.Visible = false;

                ddl_breakagedby.Items.Add(new ListItem("select", "0"));
                ddl_breakagedby.Items.Add(new ListItem("student", "1"));
                ddl_breakagedby.Items.Add(new ListItem("staff", "2"));
                ddl_breakagedby.Items.Add(new ListItem("Guest", "3"));
                //  ddl_breakagedby.Items.Add(new ListItem("unknown", "4"));

                ddl_breakgedbyadd.Items.Add(new ListItem("select", "0"));//delsi0803
                ddl_breakgedbyadd.Items.Add(new ListItem("student", "1"));
                ddl_breakgedbyadd.Items.Add(new ListItem("staff", "2"));
                ddl_breakgedbyadd.Items.Add(new ListItem("Guest", "3"));
                // ddl_breakgedbyadd.Items.Add(new ListItem("unknown", "4"));


                ddl_status.Items.Add(new ListItem("select", "0"));
                ddl_status.Items.Add(new ListItem("Scrapped", "1"));
                ddl_status.Items.Add(new ListItem("Repair", "2"));
                ddl_status.Items.Add(new ListItem("Missing", "3"));

                txt_status.Items.Add(new ListItem("select", "0"));
                txt_status.Items.Add(new ListItem("Scrapped", "1"));
                txt_status.Items.Add(new ListItem("Repair", "2"));
                txt_status.Items.Add(new ListItem("Missing", "3"));

                loadcollegestaffpopup();
                bindstaffdepartmentpopup();
                txt_staffnamesearch.Visible = true;
                binditem1();
                binddept();
                bindbatch1();
                bindbranch1();
                binddegree2();
                Session["depcode"] = null;
                Session["itmpk"] = null;
                Session["stfcode"] = null;
                Session["deptname"] = null;
                loaditem();
            }
            collegecodestat = collegecode1;
        }
        catch (Exception ex) { }
    }
    protected void binddept()
    {
        try
        {
            ds.Clear();
            cbl_dept.Items.Clear();
            string itempk = "";
            if (cbl_itemname.Items.Count > 0)
            {
                for (int i = 0; i < cbl_itemname.Items.Count; i++)
                {
                    if (cbl_itemname.Items[i].Selected == true)
                    {

                        if (itempk == "")
                        {
                            itempk = "" + cbl_itemname.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            itempk = itempk + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                        }
                    }
                }
            }
            string bindquery = "select distinct h.dept_name,d.dept_code from IM_ItemDeptMaster dm,HRDept_Master h, department d where d.dept_code=dm.ItemDeptFK and h.dept_code=d.dept_code and dm.itemfk in('" + itempk + "')";
            ds = d2.select_method_wo_parameter(bindquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_dept.DataSource = ds;
                cbl_dept.DataTextField = "Dept_Name";
                cbl_dept.DataValueField = "Dept_Code";
                cbl_dept.DataBind();
                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        cbl_dept.Items[i].Selected = true;
                    }
                    txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                }
                binditem();
            }
            else
            {
                txt_dept.Text = "--Select--";
            }
        }
        catch
        {

        }
    }
    protected void binditem()
    {
        try
        {
            ds.Clear();
            chklst_pop2itemtyp.Items.Clear();
            int i = 0;

            string item = "select distinct ItemPk,itemname from IM_ItemMaster where ItemType='1' ";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                chklst_pop2itemtyp.DataSource = ds;
                chklst_pop2itemtyp.DataTextField = "itemname";
                chklst_pop2itemtyp.DataValueField = "ItemPk";
                chklst_pop2itemtyp.DataBind();
                if (chklst_pop2itemtyp.Items.Count > 0)
                {
                    for (i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                    {
                        chklst_pop2itemtyp.Items[i].Selected = true;
                    }
                    txt_itemname3.Text = "Item (" + chklst_pop2itemtyp.Items.Count + ")";
                }
            }
            else
            {
                txt_itemname3.Text = "--Select--";
            }

        }
        catch
        {

        }
    }

    protected void binditem1()
    {
        try
        {
            ds.Clear();
            //cbl_itemname.Items.Clear();
            int i = 0;

            string item = "select distinct ItemPk,itemname from IM_ItemMaster where ItemType='1'";
            ds = d2.select_method_wo_parameter(item, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemname.DataSource = ds;
                cbl_itemname.DataTextField = "itemname";
                cbl_itemname.DataValueField = "ItemPk";
                cbl_itemname.DataBind();
                if (cbl_itemname.Items.Count > 0)
                {
                    for (i = 0; i < cbl_itemname.Items.Count; i++)
                    {
                        cbl_itemname.Items[i].Selected = true;
                    }
                    txt_itemname.Text = "Item (" + cbl_itemname.Items.Count + ")";
                }
            }
            else
            {
                txt_itemname.Text = "--Select--";
            }

        }
        catch
        {

        }
    }

    protected void cb_itemname_CheckedChange(object sender, EventArgs e)
    {
        int cout1 = 0;
        txt_itemname.Text = "--Select--";

        if (cb_itemname.Checked == true)
        {
            cout1++;
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                cbl_itemname.Items[i].Selected = true;
            }
            txt_itemname.Text = "Item(" + (cbl_itemname.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                cbl_itemname.Items[i].Selected = false;
            }
        }
        //item();
        binddept();

    }

    protected void cbl_itemname_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_itemname.Checked = false;
        //item();
        int commcount1 = 0;
        txt_itemname.Text = "--Select--";
        for (i = 0; i < cbl_itemname.Items.Count; i++)
        {
            if (cbl_itemname.Items[i].Selected == true)
            {
                commcount1 = commcount1 + 1;
                cb_itemname.Checked = false;
            }
        }
        if (commcount1 > 0)
        {
            if (commcount1 == cbl_itemname.Items.Count)
            {
                cb_itemname.Checked = true;
            }
            txt_itemname.Text = "Item(" + commcount1.ToString() + ")";
        }
        binddept();
    }

    protected void cb_dept_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_dept.Text = "--Select--";

        if (cb_dept.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = true;
            }
            txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                cbl_dept.Items[i].Selected = false;
            }
        }
        //binditem();
    }
    protected void cbl_dept_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_dept.Checked = false;
        //item();
        int commcount = 0;
        txt_dept.Text = "--Select--";
        for (i = 0; i < cbl_dept.Items.Count; i++)
        {
            if (cbl_dept.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_dept.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_dept.Items.Count)
            {
                cb_dept.Checked = true;
            }
            txt_dept.Text = "Department(" + commcount.ToString() + ")";
        }

    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        Printcontrol.Visible = false;
        if (ddl_breakagedby.SelectedIndex != 0)
        {
            bool chk = false; string uncheck = "";
            lblnorecr.Visible = false;
            string selectquery = "";
            string itempk = "";
            for (int i = 0; i < cbl_itemname.Items.Count; i++)
            {
                if (cbl_itemname.Items[i].Selected == true)
                {
                    if (itempk == "")
                    {
                        itempk = "" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itempk = itempk + "'" + "," + "'" + cbl_itemname.Items[i].Value.ToString() + "";
                    }
                }
            }
            string deptcode = "";
            for (int i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    if (deptcode == "")
                    {
                        deptcode = "" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        deptcode = deptcode + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (ddl_breakagedby.SelectedIndex == 1)
            {
                selectquery = " select distinct staff_name, h.HeaderName,l.LedgerName,DeptFK,ItemFK,AssetNo,InchargeStaff,case when BreakageBy=1 then 'Student' when BreakageBy=2 then 'Staff' else 'Unknown' end BreakageByvalue ,BreakageBy,case when MemCode=1 then 'Student' when MemCode=2 then 'Staff' else 'Unknown' end MemCodevalue,MemCode,case when ItemStatus=1 then 'Scrapped' when ItemStatus=2 then 'Repair' when ItemStatus=3 then 'Missing' else 'Unknown' end ItemStatus,case when PayMethod=1 then 'Self' when PayMethod=2 then 'Management'  end PayMethodvalue, PayMethod,b.Remarks,b.HeaderFK,b.LedgerFK,PayAmount,i.ItemName,r.Stud_Name,d.Dept_Name ,b.InchargeStaff from IT_BreakageDetails b,IM_ItemMaster i,Department d,Registration r,FM_HeaderMaster H,FM_LedgerMaster l, staffmaster s,staff_appl_master a where b.ItemFK =i.ItemPK  and d.Dept_Code =b.DeptFK and r.App_No =b.MemCode and l.LedgerPK=b.LedgerFK   AND H.HeaderPK=B.HeaderFK and s.appl_no=a.appl_no and a.appl_id=b.InchargeStaff and b.DeptFK in('" + deptcode + "')and b.itemfk in('" + itempk + "')";//(select staff_name from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and a.appl_id = b.InchargeStaff) as InchargeStaffname,
            }
            if (ddl_breakagedby.SelectedIndex == 2)
            {
                selectquery = " select  h.HeaderName,l.LedgerName, DeptFK,ItemFK,AssetNo,InchargeStaff,case when BreakageBy=1 then 'Student' when BreakageBy=2 then 'Staff' else 'Unknown' end BreakageByvalue ,BreakageBy,case when MemCode=1 then 'Student' when MemCode=2 then 'Staff' else 'Unknown'  end MemCodevalue,MemCode,case when ItemStatus=1 then 'Scrapped' when ItemStatus=2 then 'Repair' when ItemStatus=3 then 'Missing' else 'Unknown' end ItemStatus,case when PayMethod=1 then 'Self' when PayMethod=2 then 'Management'  end PayMethodvalue, PayMethod,b.Remarks,b.HeaderFK,b.LedgerFK,PayAmount,i.ItemName,s.staff_name,d.Dept_Name  from IT_BreakageDetails b,IM_ItemMaster i,Department d,staffmaster s,staff_appl_master a,FM_HeaderMaster H,FM_LedgerMaster l  where b.ItemFK =i.ItemPK  and d.Dept_Code =b.DeptFK and s.appl_no =a.appl_no and a.appl_id =b.MemCode and l.LedgerPK=b.LedgerFK   AND H.HeaderPK=B.HeaderFK  and b.DeptFK in('" + deptcode + "')and b.itemfk in('" + itempk + "')";
            }
            if (ddl_breakagedby.SelectedIndex == 3)
            {
                selectquery = "select  h.HeaderName,l.LedgerName,DeptFK,ItemFK,AssetNo,InchargeStaff,case when BreakageBy=1 then 'Student' when BreakageBy=2 then 'Staff' when BreakageBy=3 then 'Guest' else 'Unknown' end BreakageByvalue ,BreakageBy,case when MemCode=1 then 'Student' when MemCode=2 then 'Staff' when MemCode=2 then 'Guest' else 'Unknown'  end MemCodevalue,MemCode,case when ItemStatus=1 then 'Scrapped' when ItemStatus=2 then 'Repair' when ItemStatus=3 then 'Missing' else 'Unknown' end ItemStatus,case when PayMethod=1 then 'Self' when PayMethod=2 then 'Management'  end PayMethodvalue, PayMethod,b.Remarks,b.HeaderFK,b.LedgerFK,PayAmount,i.ItemName,d.Dept_Name  from IT_BreakageDetails b,IM_ItemMaster i,Department d ,FM_HeaderMaster H,FM_LedgerMaster l where  b.ItemFK =i.ItemPK  and d.Dept_Code =b.DeptFK and BreakageBy =3 and l.LedgerPK=b.LedgerFK   AND H.HeaderPK=B.HeaderFK  and b.DeptFK in('" + deptcode + "')and b.itemfk in('" + itempk + "')";
            }

            if (ddl_status.SelectedIndex != 0)
            {
                selectquery = selectquery + " and ItemStatus='" + ddl_status.SelectedItem.Value + "' order by AssetNo";
            }
            else
            {
                selectquery = selectquery + " order by AssetNo";
            }
            stu.Visible = false;
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                Fpmain.Sheets[0].RowHeader.Visible = false;
                Fpmain.CommandBar.Visible = false;
                Fpmain.Sheets[0].RowCount = 0;
                Fpmain.SheetCorner.ColumnCount = 0;
                Fpmain.Sheets[0].ColumnHeader.RowCount = 1;

                Fpmain.Sheets[0].AutoPostBack = true;
                Fpmain.Sheets[0].ColumnCount = 13;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpmain.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                //Fpmain.Columns[0].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Text = "DeptName";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[1].Width = 250;
                Fpmain.Columns[1].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[2].Width = 150;
                Fpmain.Columns[2].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Text = "AssetNo";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[3].Width = 150;
                Fpmain.Columns[3].Visible = false;
                if (ddl_breakagedby.SelectedIndex == 1)
                {
                    Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Incharge Staff";
                    Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpmain.Sheets[0].Columns[4].Width = 150;
                }
                else
                {
                    Fpmain.Columns[4].Visible = false;
                }
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Breakage By";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[5].Width = 150;
                Fpmain.Columns[5].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Mem Code";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[6].Width = 150;
                Fpmain.Columns[6].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Item Status";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[7].Width = 150;
                Fpmain.Columns[7].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Pay Method";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[8].Width = 150;
                Fpmain.Columns[8].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Remarks";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[9].Width = 150;
                Fpmain.Columns[9].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Header Name";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[10].Width = 150;
                Fpmain.Columns[10].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Ledger Name";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 11].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 11].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 11].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 11].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[11].Width = 150;
                Fpmain.Columns[11].Visible = false;

                Fpmain.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Pay Amount";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 12].Font.Bold = true;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 12].Font.Name = "Book Antiqua";
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 12].Font.Size = FontUnit.Medium;
                Fpmain.Sheets[0].ColumnHeader.Cells[0, 12].HorizontalAlign = HorizontalAlign.Center;
                Fpmain.Sheets[0].Columns[12].Width = 150;
                Fpmain.Columns[12].Visible = false;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpmain.Sheets[0].RowCount++;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["Dept_Name"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["DeptFK"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["AssetNo"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    if (ddl_breakagedby.SelectedIndex == 1)
                    {
                        string staffname = "";
                        staffname = d2.GetFunction("select staff_name from staff_appl_master a,staffmaster s where a.appl_no =s.appl_no and a.appl_id ='" + Convert.ToString(ds.Tables[0].Rows[i]["InchargeStaff"]) + "'");

                        Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Text = staffname;// Convert.ToString(ds.Tables[0].Rows[i]["staff_name"]);//InchargeStaff 
                        Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[i]["InchargeStaff"]);
                        Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    }



                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[i]["BreakageByvalue"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Tag = Convert.ToString(ds.Tables[0].Rows[i]["BreakageBy"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[i]["MemCode"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[i]["MemCodevalue"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemStatus"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[i]["PayMethodvalue"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(ds.Tables[0].Rows[i]["Remarks"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Tag = Convert.ToString(ds.Tables[0].Rows[i]["HeaderFK"]); Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(ds.Tables[0].Rows[i]["HeaderName"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(ds.Tables[0].Rows[i]["LedgerName"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 11].Tag = Convert.ToString(ds.Tables[0].Rows[i]["LedgerFK"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";

                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 12].Text = Convert.ToString(ds.Tables[0].Rows[i]["PayAmount"]);
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 12].HorizontalAlign = HorizontalAlign.Left;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 12].Font.Size = FontUnit.Medium;
                    Fpmain.Sheets[0].Cells[Fpmain.Sheets[0].RowCount - 1, 12].Font.Name = "Book Antiqua";
                    chk = true;
                    if (chk == true)
                    {
                        uncheck = "1";
                    }
                }
                if (cblcolumnorder.Items.Count > 0)
                {
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            string headername = Convert.ToString(cblcolumnorder.Items[i].ToString());

                            if (headername == "DeptName")
                            {
                                Fpmain.Columns[1].Visible = true;
                            }
                            if (headername == "Item Name")
                            {
                                Fpmain.Columns[2].Visible = true;
                            }
                            else if (headername == "AssetNo")
                            {
                                Fpmain.Columns[3].Visible = true;
                            }
                            else if (headername == "Incharge Staff")
                            {
                                if (ddl_breakagedby.SelectedIndex == 1)
                                {
                                    Fpmain.Columns[4].Visible = true;
                                }
                                else
                                {
                                    Fpmain.Columns[4].Visible = false;
                                }
                            }
                            else if (headername == "Breakage By")
                            {
                                Fpmain.Columns[5].Visible = true;
                            }
                            else if (headername == "Mem Code")
                            {
                                Fpmain.Columns[6].Visible = true;
                            }
                            else if (headername == "Item Status")
                            {
                                Fpmain.Columns[7].Visible = true;
                            }
                            else if (headername == "Pay Method")
                            {
                                Fpmain.Columns[8].Visible = true;
                            }
                            else if (headername == "Remarks")
                            {
                                Fpmain.Columns[9].Visible = true;
                            }
                            else if (headername == "Header Name")
                            {
                                Fpmain.Columns[10].Visible = true;
                            }
                            else if (headername == "Ledger Name")
                            {
                                Fpmain.Columns[11].Visible = true;
                            }
                            else if (headername == "Pay Amount")
                            {
                                Fpmain.Columns[12].Visible = true;
                            }

                            check1 = true;
                        }
                    }
                }
                if (check1 == false)
                {
                    CheckBox_column.Checked = true;
                    LinkButtonsremove_Click(sender, e);
                    for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                    {
                        if (cblcolumnorder.Items[i].Selected == true)
                        {
                            string headername = Convert.ToString(cblcolumnorder.Items[i].ToString());

                            if (headername == "DeptName")
                            {
                                Fpmain.Columns[1].Visible = true;
                            }
                            if (headername == "Item Name")
                            {
                                Fpmain.Columns[2].Visible = true;
                            }
                            else if (headername == "AssetNo")
                            {
                                Fpmain.Columns[3].Visible = true;
                            }
                            else if (headername == "Incharge Staff")
                            {
                                if (ddl_breakagedby.SelectedIndex == 1)
                                {
                                    Fpmain.Columns[4].Visible = true;
                                }
                                else
                                {
                                    Fpmain.Columns[4].Visible = false;
                                }
                            }
                            else if (headername == "Breakage By")
                            {
                                Fpmain.Columns[5].Visible = true;
                            }
                            else if (headername == "Mem Code")
                            {
                                Fpmain.Columns[6].Visible = true;
                            }
                            else if (headername == "Item Status")
                            {
                                Fpmain.Columns[7].Visible = true;
                            }
                            else if (headername == "Pay Method")
                            {
                                Fpmain.Columns[8].Visible = true;
                            }
                            else if (headername == "Remarks")
                            {
                                Fpmain.Columns[9].Visible = true;
                            }
                            else if (headername == "Header Name")
                            {
                                Fpmain.Columns[10].Visible = true;
                            }
                            else if (headername == "Ledger Name")
                            {
                                Fpmain.Columns[11].Visible = true;
                            }
                            else if (headername == "Pay Amount")
                            {
                                Fpmain.Columns[12].Visible = true;
                            }
                        }
                    }
                }
                Fpmain.Visible = true;
                rptprint.Visible = true;
                fpmain_div.Visible = true;
                Fpmain.Sheets[0].PageSize = Fpmain.Sheets[0].RowCount;
                Fpmain.SaveChanges();

            }
            else
            {
                Fpmain.Visible = false;
                rptprint.Visible = false;
                fpmain_div.Visible = false;
                lblnorecr.Visible = true;
                lblnorecr.Text = "No Records Founds";
            }
        }
        else
        {
            Fpmain.Visible = false;
            rptprint.Visible = false;
            fpmain_div.Visible = false;
            lblnorecr.Visible = true;
            lblnorecr.Text = "Please Select All Fields";
        }
    }
    protected void Cell1_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
            poperrjs.Visible = true;
            btn_brkdetsave.Visible = false;
            btn_brkdetexit.Visible = false;
            btn_brkupdate.Visible = true;
            btn_brkdelete.Visible = true;
            btn_exit1.Visible = true;
        }
        catch
        {

        }
    }

    protected void Fpmain_render(object sender, EventArgs e)
    {


        if (check == true)
        {
            string activerow = "";
            string activecol = "";
            activerow = Fpmain.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpmain.ActiveSheetView.ActiveColumn.ToString();
            // collegecode = Session["collegecode"].ToString();
            //bindcollege1();
            if (activerow.Trim() != "")
            {
                //ddl_breakagedby.Items.Add(new ListItem("select", "0"));
                //ddl_breakagedby.Items.Add(new ListItem("student", "1"));
                //ddl_breakagedby.Items.Add(new ListItem("staff", "2"));
                //ddl_breakagedby.Items.Add(new ListItem("unknown", "3"));
                //txt_status.Items.Add(new ListItem("select", "0"));
                //txt_status.Items.Add(new ListItem("Scrapped", "1"));
                //txt_status.Items.Add(new ListItem("Repair", "2"));
                //txt_status.Items.Add(new ListItem("Missing", "3"));

                string deptfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);

                //string deptfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
                string itemfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                string assetno = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                string Incstaff = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                string Incstf = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Tag);
                string breakageby = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
                string Memcode = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
                string Memcodevalue = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Tag);
                string itemstat = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);
                string paymeth = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
                string remarks = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
                string headerfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Tag);
                string ledgerfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Tag);
                string payamt = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);
                ddl_breakgedbyadd.SelectedIndex = ddl_breakgedbyadd.Items.IndexOf(ddl_breakgedbyadd.Items.FindByValue(breakageby));

                txt_deptadd.Text = deptfk;
                string itemquery = "select distinct ItemUnit from IM_ItemMaster where ItemName='" + itemfk + "'";
                ds = d2.select_method_wo_parameter(itemquery, "Text");

                string iunit = ds.Tables[0].Rows[0]["ItemUnit"].ToString();
                txt_measure.Text = iunit;

                txt_deptadd.Enabled = false;
                ddl_breakgedbyadd.Enabled = false;
                if (breakageby == "1")
                {
                    rdb_student.Checked = true;
                    lbl_rollno.Visible = true;
                    txt_rollno.Visible = true;
                    string strno = d2.GetFunction("select Roll_No from Registration where App_No='" + Memcode + "'");
                    txt_rollno.Text = strno;

                    string stddet = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.Roll_no='" + strno + "'";
                    ds = d2.select_method_wo_parameter(stddet, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string sname = ds.Tables[0].Rows[0]["stud_name"].ToString();
                        txt_name.Text = sname;
                        string sec = ds.Tables[0].Rows[0]["Sections"].ToString();
                        txt_sec.Text = sec;
                        string deg = ds.Tables[0].Rows[0]["Course_Name"].ToString();
                        txt_deg.Text = deg;
                        string sem = ds.Tables[0].Rows[0]["Current_Semester"].ToString();
                        txt_sem.Text = sem;
                        string deptstu = ds.Tables[0].Rows[0]["Dept_Name"].ToString();
                        txt_deptstu.Text = deptstu;
                    }
                    lbl_deg.Visible = true;
                    txt_deg.Visible = true;
                    lbl_des.Visible = true;
                    txt_des.Visible = true;
                    lbl_sem.Visible = true;
                    txt_sem.Visible = true;
                    lbl_pop1staffname.Visible = true;
                    txt_pop1staffname.Visible = true;
                    lbl_name.Visible = true;
                    txt_name.Visible = true;
                    lbl_deptstu.Visible = true;
                    txt_deptstu.Visible = true;
                    lbl_sec.Visible = true;
                    txt_sec.Visible = true;
                    lbl_photo.Visible = true;
                    ImageButton3.Visible = true;
                    btn_staffquestion.Visible = true;
                    staff.Visible = true;
                    btn_roll.Visible = true;
                    roll.Visible = true;
                    rdb_student.Enabled = true;
                    rdb_staff.Enabled = false;
                }
                if (breakageby == "2")
                {
                    rdb_staff.Enabled = true;
                    rdb_staff.Visible = true;
                    rdb_student.Enabled = false;
                    //rdb_student.Enabled = true;

                    string stf = d2.GetFunction("select s.staff_code from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and appl_id ='" + Memcode + "'");
                    string stfdet = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_Code='" + stf + "'";
                    ds = d2.select_method_wo_parameter(stfdet, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txt_staffcode.Text = stf;
                        string stfname = ds.Tables[0].Rows[0]["staff_name"].ToString();
                        txt_staffname.Text = stfname;
                        string stfdept = ds.Tables[0].Rows[0]["dept_name"].ToString();
                        txt_deptstaff.Text = stfdept;
                        string stfdes = ds.Tables[0].Rows[0]["desig_name"].ToString();
                        txt_des.Text = stfdes;
                        string stftyp = ds.Tables[0].Rows[0]["staffcategory"].ToString();
                        txt_stafftype.Text = stftyp;
                    }
                    lbl_staffcode.Visible = true;
                    txt_staffcode.Visible = true;
                    code.Visible = true;
                    btn_staffcode.Visible = true;
                    lbl_staffname.Visible = true;
                    txt_staffname.Visible = true;
                    lbl_deptstaff.Visible = true;
                    txt_deptstaff.Visible = true;
                    lbl_des.Visible = true;
                    txt_des.Visible = true;
                    lbl_stafftypr.Visible = true;
                    txt_stafftype.Visible = true;
                    lbl_staffphoto.Visible = true;
                    ImageButton4.Visible = true;
                }
                else if (breakageby == "3")
                {
                    rdb_staff.Enabled = false;
                    rdb_student.Enabled = false;
                }

                txt_itemnameadd.Text = itemfk;
                string pm = paymeth;
                string incstf = d2.GetFunction("select s.staff_name  from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and appl_id  ='" + Incstf + "'");
                txt_pop1staffname.Text = incstf;
                ddl_header.SelectedIndex = ddl_header.Items.IndexOf(ddl_header.Items.FindByValue(headerfk));
                ddl_ledger.SelectedIndex = ddl_ledger.Items.IndexOf(ddl_ledger.Items.FindByValue(ledgerfk));
                if (pm == "Self")
                {
                    rdb_sel.Checked = true;
                    loadledger();
                    loadHeader();
                    ddl_header.Visible = true;
                    lbl_ledger.Visible = true;
                    txt_status.Visible = true;
                    lbl_statuspay.Visible = true;
                    ddl_ledger.Visible = true;
                    lbl_narr.Visible = true;
                    txt_narr.Visible = true;
                    lbl_payamt.Visible = true;
                    txt_payamt.Visible = true;
                    sppay.Visible = true;
                    lbl_sltheader.Visible = true;
                }
                if (pm == "Management")
                {

                    rdb_mgmt.Checked = true;
                    loadledger();
                    loadHeader();
                    lbl_sltheader.Visible = true;
                    ddl_header.Visible = true;
                    lbl_ledger.Visible = true;
                    //txt_status.Visible = true;
                    lbl_statuspay.Visible = true;
                    ddl_ledger.Visible = true;
                    lbl_narr.Visible = true;
                    txt_narr.Visible = true;
                    lbl_payamt.Visible = true;
                    txt_payamt.Visible = true;
                    sppay.Visible = true;
                }
                txt_payamt.Text = payamt;
                // txt_status.SelectedItem.Value = Convert.ToString(itemstat);
                txt_status.SelectedIndex = txt_status.Items.IndexOf(txt_status.Items.FindByText(itemstat));
                string sd = txt_status.SelectedItem.Value;
                txt_status.Visible = true;


                if (ddl_breakgedbyadd.SelectedIndex == 1)
                {
                    rdb_student.Checked = true;
                    rdb_student_CheckedChanged(sender, e);
                    rdb_staff.Checked = false;


                }
                if (ddl_breakgedbyadd.SelectedIndex == 2)
                {
                    rdb_staff.Checked = true;
                    rdb_staff_CheckedChanged(sender, e);
                    rdb_student.Checked = false;

                }
                txt_narr.Text = remarks;
                txt_assetno.Text = assetno;
            }
        }
    }

    protected void btn_brkdelete_Click(object sender, EventArgs e)
    {
        int brkdelete = 0;
        string activerow = "";
        string activecol = "";
        activerow = Fpmain.ActiveSheetView.ActiveRow.ToString();
        activecol = Fpmain.ActiveSheetView.ActiveColumn.ToString();
        string assetno = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);

        //string breakage = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Text);
        //string breakageby = breakage.value;
        string del = "delete from IT_BreakageDetails where AssetNo='" + assetno + "' ";
        brkdelete = d2.update_method_wo_parameter(del, "Text");
        alertpopwindow.Visible = true;
        lblalerterr.Text = "Deleted Successfully ";
    }

    protected void btn_brkupdate_Click(object sender, EventArgs e)
    {
        int brekageupdate = 0;
        string assetno1 = Convert.ToString(txt_assetno.Text);
        string status = Convert.ToString(txt_status.SelectedItem.Text);
        string reference = Convert.ToString(txt_narr.Text);
        string payamt1 = Convert.ToString(txt_payamt.Text);
        string depfk = txt_deptadd.Text;
        string dfk = d2.GetFunction("select distinct Dept_Code from Department where Dept_Name='" + depfk + "'");

        string incstf = txt_pop1staffname.Text;
        string inc = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_name ='" + incstf + "'");
        string itmfk = txt_itemnameadd.Text;
        string ifk = d2.GetFunction("select distinct ItemPK from IM_ItemMaster where ItemName='" + itmfk + "'");
        string paymethod = "";
        if (rdb_sel.Checked == true)
        {
            paymethod = "1";
        }
        if (rdb_mgmt.Checked == true)
        {

            paymethod = "2";
        }

        string mmc = "";
        if (rdb_student.Checked == true)
        {
            string rno = txt_rollno.Text;
            mmc = d2.GetFunction("select App_No from Registration where Roll_No='" + rno + "'");
        }
        if (rdb_staff.Checked == true)
        {
            string sno = txt_staffcode.Text;
            mmc = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_code ='" + sno + "'");
        }
        string activerow = "";
        string activecol = "";
        loadHeader();
        loadledger();
        activerow = Fpmain.ActiveSheetView.ActiveRow.ToString();
        activecol = Fpmain.ActiveSheetView.ActiveColumn.ToString();

        ddl_breakagedby.Items.Clear();
        ddl_breakagedby.Items.Add(new ListItem("select", "0"));
        ddl_breakagedby.Items.Add(new ListItem("student", "1"));
        ddl_breakagedby.Items.Add(new ListItem("staff", "2"));
        ddl_breakagedby.Items.Add(new ListItem("Guest", "3"));//delsi1003
        //   ddl_breakagedby.Items.Add(new ListItem("unknown", "3"));

        txt_status.Items.Clear();
        txt_status.Items.Add(new ListItem("select", "0"));
        txt_status.Items.Add(new ListItem("Scrapped", "1"));
        txt_status.Items.Add(new ListItem("Repair", "2"));
        txt_status.Items.Add(new ListItem("Missing", "3"));




        string deptfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
        string itemfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
        string assetno = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
        string Incstaff = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
        string breakageby = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 5].Tag);
        string Memcode = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 6].Text);
        string itemstat = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 7].Text);
        string paymeth = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 8].Text);
        string remarks = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 9].Text);
        string headerfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 10].Tag);
        string ledgerfk = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 11].Tag);
        string payamt = Convert.ToString(Fpmain.Sheets[0].Cells[Convert.ToInt32(activerow), 12].Text);

        ddl_breakgedbyadd.SelectedIndex = ddl_breakgedbyadd.Items.IndexOf(ddl_breakgedbyadd.Items.FindByValue(breakageby));
        txt_status.SelectedIndex = txt_status.Items.IndexOf(txt_status.Items.FindByText(status));
        //string dpc = Convert.ToString(Session["depcode"]);
        //string ipk = Convert.ToString(Session["itmpk"]);
        //string stf = Convert.ToString(Session["stfcode"]);

        string updatequery = "if exists (select * from IT_BreakageDetails where DeptFK='" + deptfk + "' and AssetNo='" + assetno + "' and BreakageBy ='" + ddl_breakgedbyadd.SelectedItem.Value + "'  )update IT_BreakageDetails set DeptFK='" + dfk + "',ItemFK='" + ifk + "', InchargeStaff='" + inc + "', BreakageBy='" + ddl_breakgedbyadd.SelectedItem.Value + "',MemCode='" + mmc + "',ItemStatus='" + txt_status.SelectedItem.Value + "',PayMethod='" + paymethod + "',Remarks='" + reference + "',HeaderFK='" + ddl_header.SelectedItem.Value + "',LedgerFK='" + ddl_ledger.SelectedItem.Value + "',PayAmount='" + payamt1 + "' where DeptFK='" + deptfk + "' and Assetno='" + assetno + "' and BreakageBy ='" + ddl_breakgedbyadd.SelectedItem.Value + "'  else insert into IT_BreakageDetails(DeptFK,ItemFK,AssetNo,InchargeStaff,BreakageBy,MemCode,ItemStatus,PayMethod,Remarks,HeaderFK,LedgerFK,PayAmount) values('" + dfk + "','" + ifk + "','" + assetno + "','" + inc + "','" + ddl_breakgedbyadd.SelectedItem.Value + "','" + ddl_breakgedbyadd.SelectedItem.Value + "','" + txt_status.SelectedItem.Value + "','" + paymethod + "','" + reference + "','" + ddl_header.SelectedItem.Value + "','" + ddl_ledger.SelectedItem.Value + "','" + payamt1 + "')";
        brekageupdate = d2.update_method_wo_parameter(updatequery, "Text");
        if (brekageupdate != 0)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Updated Successfully ";
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
            Response.Redirect("~/Default.aspx", false);
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
                d2.printexcelreport(Fpmain, reportname);
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
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Breakage Entry Report";
            string pagename = "breakage_entry.aspx";
            Printcontrol.loadspreaddetails(Fpmain, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
    public void cblcolumnorder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox_column.Checked = false;
            string value = "";
            int index;
            cblcolumnorder.Items[0].Selected = true;
            // cblcolumnorder.Items[0].Enabled = false;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index);
            if (cblcolumnorder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    //if (tborder.Text == "")
                    //{
                    //    ItemList.Add("Company Code");
                    //}
                    //ItemList.Add("Admission No");
                    //ItemList.Add("Name");
                    ItemList.Add(cblcolumnorder.Items[index].Value.ToString());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(cblcolumnorder.Items[index].Value.ToString());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblcolumnorder.Items.Count; i++)
            {
                //if (i == 0 || i == 1 || i == 2)
                //{
                //    cblcolumnorder.Items[0].Selected = true;
                //    cblcolumnorder.Items[1].Selected = true;
                //    cblcolumnorder.Items[2].Selected = true;
                //}
                if (cblcolumnorder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i);
                    ItemList.Remove(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Remove(sindex);

                }
            }

            lnk_columnorder.Visible = true;
            tborder.Visible = true;
            tborder.Text = "";
            string colname12 = "";
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";

                }
                else
                {
                    colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (i + 1).ToString() + ")";
                }
                //tborder.Text = tborder.Text + ItemList[i].ToString();

                //tborder.Text = tborder.Text + "(" + (i + 1).ToString() + ")  ";

            }
            tborder.Text = colname12;
            if (ItemList.Count == 14)
            {
                CheckBox_column.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                tborder.Visible = false;
                lnk_columnorder.Visible = false;
            }

            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }

    public void CheckBox_column_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (CheckBox_column.Checked == true)
            {
                tborder.Text = "";
                ItemList.Clear();
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    string si = Convert.ToString(i);
                    cblcolumnorder.Items[i].Selected = true;
                    lnk_columnorder.Visible = true;
                    ItemList.Add(cblcolumnorder.Items[i].Value.ToString());
                    Itemindex.Add(si);
                }
                lnk_columnorder.Visible = true;
                tborder.Visible = true;
                tborder.Text = "";
                int j = 0;
                string colname12 = "";
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = ItemList[i].ToString() + "(" + (j).ToString() + ")";

                    }
                    else
                    {
                        colname12 = colname12 + "," + ItemList[i].ToString() + "(" + (j).ToString() + ")";
                    }
                    // tborder.Text = tborder.Text + ItemList[i].ToString();



                }
                tborder.Text = colname12;

            }
            else
            {
                for (int i = 0; i < cblcolumnorder.Items.Count; i++)
                {
                    cblcolumnorder.Items[i].Selected = false;
                    lnk_columnorder.Visible = false;
                    ItemList.Clear();
                    Itemindex.Clear();
                    //cblcolumnorder.Items[0].Selected = true;
                }

                tborder.Text = "";
                tborder.Visible = false;

            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {

        }
    }
    public void LinkButtonsremove_Click(object sender, EventArgs e)
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
        catch (Exception ex)
        {
        }
    }
    protected void btn_addnew_click(object sender, EventArgs e)
    {
        // ddl_breakgedbyadd_SelectedIndexChanged(sender, e);


        rdb_staff.Enabled = false;
        rdb_staff.Checked = false;
        rdb_Guest.Enabled = false;
        rdb_Guest.Checked = false;

        Printcontrol.Visible = false;
        btn1sturoll.Visible = false;
        txt_deptadd.Text = "";
        txt_itemnameadd.Text = "";
        fs_student.Visible = false;
        //  ddl_breakgedbyadd.Text = "select";
        rdb_student.Checked = false;
        //rdb_staff.Checked = false;
        //rdb_mgmt.Checked = false;
        rdb_sel.Checked = false;
        txt_rollno.Text = "";
        txt_measure.Text = "";
        txt_name.Text = "";
        txt_narr.Text = "";
        txt_payamt.Text = "";
        txt_deg.Text = "";
        txt_sem.Text = "";
        txt_pop1staffname.Text = "";
        ImageButton3.Visible = false;
        ImageButton4.Visible = false;
        // ddl_breakagedby.Items.Clear();
        ddl_header.Items.Clear();
        ddl_ledger.Items.Clear();
        txt_sec.Text = "";
        txt_deptstu.Text = "";
        txt_deptstaff.Text = "";
        // txt_status.Items.Clear();
        loadHeader();
        loadledger();
        txt_staffname.Text = "";
        txt_des.Text = "";
        stu.Visible = false;
        poperrjs.Visible = true;
        btn_brkdetsave.Visible = true;
        btn_brkdetexit.Visible = true;
        btn_brkupdate.Visible = false;
        btn_brkdelete.Visible = false;
        btn_exit1.Visible = false;
        bindrequestcode();
        txt_deptadd.Enabled = true;
        ddl_breakgedbyadd.Enabled = true;
        stdlblerr.Text = "";

    }
    protected void txt_fromdate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;

                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Enter FromDate less than or equal to the ToDate";
                    txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }
            }
        }
        catch (Exception ex)
        {
        }
        // PopupMessage("Enter FromDate less than or equal to the ToDate", cv_fromtodt1);
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
                //txt_leavedays.Text = "";
                DateTime dt = new DateTime();
                DateTime dt1 = new DateTime();
                string firstdate = Convert.ToString(txt_fromdate.Text);
                string seconddate = Convert.ToString(txt_todate.Text);
                string[] split = firstdate.Split('/');
                dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                split = seconddate.Split('/');
                dt1 = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
                TimeSpan ts = dt1 - dt;
                int days = ts.Days;
                if (dt > dt1)
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                    //txt_leavedays.Text = "";
                    //txt_rebatedays.Text = "";
                }
                else
                {
                    //txt_leavedays.Text = Convert.ToString(days);
                    //txt_rebatedays.Text = Convert.ToString(days);
                }

            }
        }
        catch (Exception ex)
        {
        }

        // PopupMessage("Enter ToDate greater than or equal to the FromDate", cv_fromtodt2);
    }
    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        //poperrjs.Visible = false;
        popwindow1.Visible = false;
        btn_itemsave4.Visible = false;
        btn_conexist4.Visible = false;
    }
    protected void imagebtnpopcloseadd_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
        //popwindow1.Visible = false;
    }
    protected void btn_dept_Click(object sender, EventArgs e)
    {

        Newdiv.Visible = true;
        binddepartment();
    }
    protected void btn_itemname_Click(object sender, EventArgs e)
    {

        popwindow1.Visible = true;
        loadheadername();

        // binditem();
        Fpitem.Visible = false;
        Fpitem_div.Visible = false;
        txt_searchby.Visible = true;
        btn_conexist4.Visible = false;
        btn_itemsave4.Visible = false;
    }

    protected void btndept_exit(object sender, EventArgs e)
    {
        try
        {
            Newdiv.Visible = false;
        }
        catch
        {

        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        Newdiv.Visible = false;
    }
    protected void imagebtnpopcloserollno_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }
    //protected void cbselectAll_Change(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (cbselectall.Checked == true)
    //        {
    //            if (dptgrid.Rows.Count > 0)
    //            {
    //                for (int i = 0; i < dptgrid.Rows.Count; i++)
    //                {
    //                    (dptgrid.Rows[i].FindControl("cbcheck") as CheckBox).Checked = true;
    //                }
    //            }
    //        }
    //        if (cbselectall.Checked == false)
    //        {
    //            if (dptgrid.Rows.Count > 0)
    //            {
    //                for (int i = 0; i < dptgrid.Rows.Count; i++)
    //                {
    //                    (dptgrid.Rows[i].FindControl("cbcheck") as CheckBox).Checked = false;
    //                }
    //            }
    //        }

    //    }
    //    catch
    //    {

    //    }
    //}
    public void binddepartment()
    {
        try
        {
            string deptquery = "select Dept_Code as DeptCode ,Dept_Name as DeptName from Department where college_code ='" + collegecode1 + "' order by Dept_Code ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //dptgrid.DataSource = ds;
                //dptgrid.DataBind();

                Fpdept.Visible = true;
                Fpdept.Sheets[0].RowHeader.Visible = false;
                Fpdept.CommandBar.Visible = false;
                Fpdept.Sheets[0].RowCount = 0;
                Fpdept.SheetCorner.ColumnCount = 0;
                Fpdept.Sheets[0].ColumnHeader.RowCount = 1;

                Fpdept.Sheets[0].AutoPostBack = true;
                Fpdept.Sheets[0].ColumnCount = 3;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpdept.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                Fpdept.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                Fpdept.Sheets[0].ColumnHeader.Cells[0, 1].Text = "DeptCode";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpdept.Sheets[0].Columns[1].Width = 150;

                Fpdept.Sheets[0].ColumnHeader.Cells[0, 2].Text = "DeptName";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                Fpdept.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpdept.Sheets[0].Columns[2].Width = 150;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpdept.Sheets[0].RowCount++;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["DeptCode"]);
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["DeptName"]);
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpdept.Sheets[0].Cells[Fpdept.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                }


                Fpdept.Visible = true;
                // div1.Visible = true;
                btn_itemsave4.Visible = true;
                btn_conexist4.Visible = true;
                lbl_errormsg.Visible = false;
                Fpdept.Sheets[0].PageSize = Fpdept.Sheets[0].RowCount;
                Fpdept.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpdept.Columns[1].VerticalAlign = VerticalAlign.Middle;


                Fpdept.SaveChanges();


            }
        }


        catch
        {

        }
    }


    public void loadheadername()
    {
        try
        {
            cbl_itemheader3.Items.Clear();


            string group_code = Session["group_code"].ToString();
            string columnfield = "";
            if (group_code.Contains(';'))
            {
                string[] group_semi = group_code.Split(';');
                group_code = group_semi[0].ToString();
            }
            if ((group_code.ToString().Trim() != "") && (Session["single_user"].ToString() != "1" && Session["single_user"].ToString() != "true" && Session["single_user"].ToString() != "TRUE" && Session["single_user"].ToString() != "True"))
            {
                columnfield = " and group_code='" + group_code + "'";
            }
            else
            {
                columnfield = " and usercode='" + Session["usercode"] + "'";
            }
            string maninvalue = "";
            string selectnewquery = d2.GetFunction("select value  from Master_Settings where settings='ItemHeaderRights' " + columnfield + "");
            if (selectnewquery.Trim() != "" && selectnewquery.Trim() != "0")
            {
                string[] splitnew = selectnewquery.Split(',');
                if (splitnew.Length > 0)
                {
                    for (int row = 0; row <= splitnew.GetUpperBound(0); row++)
                    {
                        if (maninvalue == "")
                        {
                            maninvalue = Convert.ToString(splitnew[row]);
                        }
                        else
                        {
                            maninvalue = maninvalue + "'" + "," + "'" + Convert.ToString(splitnew[row]);
                        }
                    }
                }
            }


            ds.Clear();
            ds = d2.BindItemHeaderWithOutRights_inv();


            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_itemheader3.DataSource = ds;
                cbl_itemheader3.DataTextField = "ItemHeaderName";
                cbl_itemheader3.DataValueField = "ItemHeaderCode";
                cbl_itemheader3.DataBind();


                if (cbl_itemheader3.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
                    {
                        cbl_itemheader3.Items[i].Selected = true;
                    }
                    txt_itemheader3.Text = "Header Name(" + cbl_itemheader3.Items.Count + ")";
                }
            }
            else
            {

                txt_itemheader3.Text = "--Select--";
            }
            loaditem();
        }
        catch
        {
        }
    }


    protected void cb_itemheader3_CheckedChange(object sender, EventArgs e)
    {
        int cout = 0;
        txt_itemheader3.Text = "--Select--";

        if (cb_itemheader3.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = true;
            }
            txt_itemheader3.Text = "HeaderName(" + (cbl_itemheader3.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                cbl_itemheader3.Items[i].Selected = false;
            }
        }
        //item();
        loaditem();

    }
    protected void cbl_itemheader3_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        cb_itemheader3.Checked = false;
        //item();
        int commcount = 0;
        txt_itemheader3.Text = "--Select--";
        for (i = 0; i < cbl_itemheader3.Items.Count; i++)
        {
            if (cbl_itemheader3.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_itemheader3.Checked = false;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_itemheader3.Items.Count)
            {
                cb_itemheader3.Checked = true;
            }
            txt_itemheader3.Text = "HeaderName(" + commcount.ToString() + ")";
        }
        loaditem();
    }

    protected void chk_pop2itemtyp_CheckedChange(object sender, EventArgs e)
    {
        int cout1 = 0;
        txt_itemname3.Text = "--Select--";

        if (chk_pop2itemtyp.Checked == true)
        {
            cout1++;
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = true;
            }
            txt_itemname3.Text = "Item(" + (chklst_pop2itemtyp.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                chklst_pop2itemtyp.Items[i].Selected = false;
            }
        }
        loaditem();

    }
    protected void chklst_pop2itemtyp_SelectedIndexChange(object sender, EventArgs e)
    {
        int i = 0;
        chk_pop2itemtyp.Checked = false;
        //item();
        int commcount1 = 0;
        txt_itemname3.Text = "--Select--";
        for (i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
        {
            if (chklst_pop2itemtyp.Items[i].Selected == true)
            {
                commcount1 = commcount1 + 1;
                chk_pop2itemtyp.Checked = false;
            }
        }
        if (commcount1 > 0)
        {
            if (commcount1 == chklst_pop2itemtyp.Items.Count)
            {
                chk_pop2itemtyp.Checked = true;
            }
            txt_itemname3.Text = "Item(" + commcount1.ToString() + ")";
        }
        loaditem();
    }
    public void loaditem()
    {
        try
        {
            chklst_pop2itemtyp.Items.Clear();
            string itemheader = "";

            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                }
            }


            if (itemheader.Trim() != "")
            {
                ds.Clear();
                ds = d2.BindItemCodeAll_inv(itemheader);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    chklst_pop2itemtyp.DataSource = ds;
                    chklst_pop2itemtyp.DataTextField = "ItemName";
                    chklst_pop2itemtyp.DataValueField = "ItemCode";
                    chklst_pop2itemtyp.DataBind();
                    if (chklst_pop2itemtyp.Items.Count > 0)
                    {
                        for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
                        {
                            chklst_pop2itemtyp.Items[i].Selected = true;
                        }
                        txt_itemname3.Text = "Item Name(" + chklst_pop2itemtyp.Items.Count + ")";
                    }
                    if (chklst_pop2itemtyp.Items.Count > 5)
                    {
                        Panel1.Width = 300;
                        Panel1.Height = 300;
                    }
                }
                else
                {
                    txt_itemname3.Text = "--Select--";
                }
            }
            else
            {
                txt_itemname3.Text = "--Select--";
            }
        }
        catch
        {
        }
    }

    public void rdb_student_CheckedChanged(object sender, EventArgs e)
    {
        fs_student.Visible = true;
        rdb_hostler.Enabled = true;
        rdb_hostler.Checked = false;
        rdb_DayScholer.Checked = false;
        rdb_DayScholer.Enabled = true;
        rdb_student.Checked = true;

        txt_itemnameadd.Text = Convert.ToString(Session["itemname"]);
        txt_measure.Text = Convert.ToString(Session["itemmeasure"]);
        txt_deptadd.Text = Convert.ToString(Session["dept"]);
        //if (rdb_student.Checked == true)
        //{
        //    lbl_rollno.Visible = true;
        //    txt_rollno.Visible = true;

        //    btn_roll.Visible = true;
        //    roll.Visible = true;
        //    lbl_name.Visible = true;
        //    txt_name.Visible = true;
        //    lbl_deg.Visible = true;
        //    txt_deg.Visible = true;
        //    lbl_deptstu.Visible = true;
        //    txt_deptstu.Visible = true;

        //    lbl_sem.Visible = true;
        //    txt_sem.Visible = true;
        //    lbl_sec.Visible = true;
        //    txt_sec.Visible = true;

        //    lbl_pop1staffname.Visible = true;
        //    txt_pop1staffname.Visible = true;
        //    btn_staffquestion.Visible = true;
        //    staff.Visible = true;
        //    lbl_photo.Visible = true;
        //    ImageButton3.Visible = true;

        //    lbl_staffcode.Visible = false;
        //    txt_staffcode.Visible = false;
        //    btn_staffcode.Visible = false;
        //    code.Visible = false;
        //    lbl_staffname.Visible = false;
        //    txt_staffname.Visible = false;

        //    lbl_deptstaff.Visible = false;
        //    txt_deptstaff.Visible = false;
        //    lbl_des.Visible = false;
        //    txt_des.Visible = false;


        //    lbl_stafftypr.Visible = false;
        //    txt_stafftype.Visible = false;
        //    lbl_staffphoto.Visible = false;
        //    ImageButton4.Visible = false;

        //    lbl_pay.Visible = true;
        //    rdb_sel.Visible = true;
        //    rdb_mgmt.Visible = true;

        //}
        //else
        //{
        //}
    }



    public void txt_rollno_OnTextChanged(object sender, EventArgs e)
    {
        Roll_No = txt_rollno.Text.ToString();
        getData(Roll_No);
        //getAppno(Roll_No);
    }
    public void getData(string Roll_No)
    {
        try
        {

            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.Roll_no='" + Roll_No + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_rollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                    txt_name.Text = ds.Tables[0].Rows[i]["stud_name"].ToString();
                    //txt_batch.Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                    txt_deg.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    txt_deptstu.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString(); ;
                    txt_sem.Text = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                    txt_sec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                    ImageButton3.Visible = true;
                    ImageButton3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Roll_No;
                }
                // popupstud.Visible = false;
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";
                // txt_batch.Text = "";
                txt_deg.Text = "";
                txt_deptstu.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
            }
            string app_no = d2.getappno(Roll_No);
            string photo = d2.GetFunction("select photo from stdphoto app_no='" + app_no + "'");
            if (photo == "0")
            {
                ImageButton3.ImageUrl = "images/photodummy.png";
            }
        }
        catch
        {

        }
    }
    public void rdb_staff_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_staff.Checked == true)
        {
            lbl_rollno.Visible = false;
            txt_rollno.Visible = false;
            btn_roll.Visible = false;
            roll.Visible = false;
            lbl_name.Visible = false;
            txt_name.Visible = false;
            lbl_deg.Visible = false;
            txt_deg.Visible = false;
            lbl_deptstu.Visible = false;
            txt_deptstu.Visible = false;

            btn1sturoll.Visible = false;
            fs_student.Visible = false;

            lbl_sem.Visible = false;
            txt_sem.Visible = false;
            lbl_sec.Visible = false;
            txt_sec.Visible = false;

            lbl_pop1staffname.Visible = false;
            txt_pop1staffname.Visible = false;
            btn_staffquestion.Visible = false;
            staff.Visible = false;
            lbl_photo.Visible = false;
            ImageButton3.Visible = false;

            lbl_staffcode.Visible = true;
            txt_staffcode.Visible = true;
            btn_staffcode.Visible = true;
            code.Visible = true;
            lbl_staffname.Visible = true;
            txt_staffname.Visible = true;

            lbl_deptstaff.Visible = true;
            txt_deptstaff.Visible = true;
            lbl_des.Visible = true;
            txt_des.Visible = true;

            lbl_stafftypr.Visible = true;
            txt_stafftype.Visible = true;
            lbl_staffphoto.Visible = true;
            ImageButton4.Visible = true;


            txt_status.Visible = true;//delsi0803
            lbl_statuspay.Visible = true;
            lbl_narr.Visible = true;
            txt_narr.Visible = true;
            lbl_payamt.Visible = true;
            txt_payamt.Visible = true;
            sppay.Visible = true;
            txt_itemnameadd.Text = Convert.ToString(Session["itemname"]);
            txt_measure.Text = Convert.ToString(Session["itemmeasure"]);
            txt_deptadd.Text = Convert.ToString(Session["dept"]);
            //  lbl_pay.Visible = true;
            //   rdb_sel.Visible = true;
            //   rdb_mgmt.Visible = true;

        }
        else
        {
        }
    }
    public void rdb_self_CheckedChanged(object sender, EventArgs e)
    {

        loadledger();
        loadHeader();
        //txt_sltheader.Visible = true;
        //btn_header.Visible = true;
        ddl_header.Visible = true;
        lbl_ledger.Visible = true;
        txt_status.Visible = true;
        lbl_statuspay.Visible = true;
        ddl_ledger.Visible = true;
        lbl_narr.Visible = true;
        txt_narr.Visible = true;
        lbl_payamt.Visible = true;
        txt_payamt.Visible = true;
        sppay.Visible = true;
        lbl_sltheader.Visible = true;
        txt_itemnameadd.Text = Convert.ToString(Session["itemname"]);
        txt_measure.Text = Convert.ToString(Session["itemmeasure"]);
        txt_deptadd.Text = Convert.ToString(Session["dept"]);
    }
    //public void rdb_unknown_CheckedChanged(object sender, EventArgs e)
    //{
    //    if (rdb_mgmt.Checked == true)
    //    {
    //        lbl_sltheader.Visible = true;

    //        ddl_header.Visible = true;
    //        lbl_ledger.Visible = true;
    //        txt_ledger.Visible = true;
    //        lbl_statuspay.Visible = true;
    //        ddl_statuspay.Visible = true;
    //        lbl_narr.Visible = true;
    //        txt_narr.Visible = true;

    //    }
    //    else
    //    {
    //        lbl_sltheader.Visible = false;

    //        ddl_header.Visible = false;
    //        lbl_ledger.Visible = false;
    //        txt_status.Visible = false;
    //        lbl_statuspay.Visible = false;
    //        ddl_ledger.Visible = false;
    //        lbl_narr.Visible = false;
    //        txt_narr.Visible = false;
    //    }
    //        lbl_pay.Visible = true;
    //        rdb_sel.Visible = false;
    //        rdb_mgmt.Visible = true;

    //        lbl_rollno.Visible = false;
    //        txt_rollno.Visible = false;
    //        btn_roll.Visible = false;
    //        lbl_name.Visible = false;
    //        txt_name.Visible = false;
    //        lbl_deg.Visible = false;
    //        txt_deg.Visible = false;
    //        lbl_deptstu.Visible = false;
    //        txt_deptstu.Visible = false;

    //        lbl_sem.Visible = false;
    //        txt_sem.Visible = false;
    //        lbl_sec.Visible = false;
    //        txt_sec.Visible = false;

    //        lbl_pop1staffname.Visible = false;
    //        txt_pop1staffname.Visible = false;
    //        btn_staffquestion.Visible = false;
    //        lbl_photo.Visible = false;
    //        ImageButton3.Visible = false;

    //        lbl_staffcode.Visible = false;
    //        txt_staffcode.Visible = false;
    //        btn_staffcode.Visible = false;
    //        lbl_staffname.Visible = false;
    //        txt_staffname.Visible = false;

    //        lbl_deptstaff.Visible = false;
    //        txt_deptstaff.Visible = false;
    //        lbl_des.Visible = false;
    //        txt_des.Visible = false;

    //        lbl_stafftypr.Visible = false;
    //        txt_stafftype.Visible = false;
    //        lbl_staffphoto.Visible = false;
    //        ImageButton4.Visible = false;



    //}
    public void rdb_mgmt_CheckedChanged(object sender, EventArgs e)
    {
        loadledger();
        loadHeader();
        lbl_sltheader.Visible = true;
        //txt_sltheader.Visible = true;
        //btn_header.Visible = true;
        ddl_header.Visible = true;
        lbl_ledger.Visible = true;
        txt_status.Visible = true;
        lbl_statuspay.Visible = true;
        ddl_ledger.Visible = true;
        lbl_narr.Visible = true;
        txt_narr.Visible = true;
        lbl_payamt.Visible = true;
        txt_payamt.Visible = true;
        sppay.Visible = true;
    }
    protected void btn_conexist4_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = true;
        popwindow1.Visible = false;
        btn_conexist4.Visible = false;
        btn_itemsave4.Visible = false;
    }



    protected void ddl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_type.SelectedValue == "0")
        {
            txt_searchby.Visible = true;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = false;
            txt_searchheadername.Text = "";
            txt_searchitemcode.Text = "";
        }
        else if (ddl_type.SelectedValue == "1")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = true;
            txt_searchheadername.Visible = false;
            txt_searchby.Text = "";
            txt_searchheadername.Text = "";

        }
        else if (ddl_type.SelectedValue == "2")
        {
            txt_searchby.Visible = false;
            txt_searchitemcode.Visible = false;
            txt_searchheadername.Visible = true;
            txt_searchby.Text = "";
            txt_searchitemcode.Text = "";
        }
    }


    protected void btn_go3_Click(object sender, EventArgs e)
    {
        try
        {
            string itemheadercode = "";
            for (int i = 0; i < cbl_itemheader3.Items.Count; i++)
            {
                if (cbl_itemheader3.Items[i].Selected == true)
                {
                    if (itemheadercode == "")
                    {
                        itemheadercode = "" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheadercode = itemheadercode + "'" + "," + "'" + cbl_itemheader3.Items[i].Value.ToString() + "";
                    }
                }
            }
            string itemcode = "";
            for (int i = 0; i < chklst_pop2itemtyp.Items.Count; i++)
            {
                if (chklst_pop2itemtyp.Items[i].Selected == true)
                {
                    if (itemcode == "")
                    {
                        itemcode = "" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemcode = itemcode + "'" + "," + "'" + chklst_pop2itemtyp.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemcode.Trim() != "" && itemheadercode.Trim() != "")
            {
                string selectquery = "";

                if (txt_searchby.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemCode,ItemPK,ItemName,ItemUnit from IM_ItemMaster where itemtype='1' and ItemName='" + txt_searchby.Text + "' order by ItemHeaderCode";
                }
                else if (txt_searchitemcode.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemCode,ItemPK,ItemName,ItemUnit from IM_ItemMaster where itemtype='1' and ItemCode='" + txt_searchitemcode.Text + "' order by ItemHeaderCode";
                }
                else if (txt_searchheadername.Text.Trim() != "")
                {
                    selectquery = "select ItemHeaderName,ItemCode,ItemPK,ItemName,ItemUnit from IM_ItemMaster where itemtype='1' and ItemHeaderName='" + txt_searchheadername.Text + "' order by ItemHeaderCode";
                }
                else
                {
                    //selectquery = "select distinct ItemHeaderName,ItemCode,ItemPK,ItemName,ItemUnit from IM_ItemMaster where itemtype='1' and ItemHeaderCode in('" + itemheadercode + "') and ItemPK in('" + itemcode + "') ";
                    selectquery = "select distinct ItemHeaderName,ItemCode,ItemPK,ItemName,ItemUnit from IM_ItemMaster where itemtype='1' and ItemHeaderCode in('" + itemheadercode + "') and ItemCode in('" + itemcode + "') ";


                }
                //string item = "select distinct ItemHeaderName, ItemCode,ItemName,ItemUnit from IM_ItemMaster where ItemHeaderCode in('" + itemheadercode + "') and ItemPK in('" + itemcode + "') ";
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpitem.Visible = true;
                    Fpitem.Sheets[0].RowHeader.Visible = false;
                    Fpitem.CommandBar.Visible = false;
                    Fpitem.Sheets[0].RowCount = 0;
                    Fpitem.SheetCorner.ColumnCount = 0;
                    Fpitem.Sheets[0].ColumnHeader.RowCount = 1;

                    Fpitem.Sheets[0].AutoPostBack = true;
                    Fpitem.Sheets[0].ColumnCount = 5;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpitem.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;


                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Item Header";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpitem.Sheets[0].Columns[1].Width = 150;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Code";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    Fpitem.Sheets[0].Columns[2].Width = 150;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Item Name";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Item Measure";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                    Fpitem.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                    Fpitem.Sheets[0].Columns[4].Width = 150;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpitem.Sheets[0].RowCount++;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemHeaderName"]);
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemCode"]);
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ItemPK"]);
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemName"]);
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[i]["ItemUnit"]);

                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                        Fpitem.Sheets[0].Cells[Fpitem.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    }
                    Fpitem.Visible = true;
                    Fpitem_div.Visible = true;
                    btn_itemsave4.Visible = true;
                    btn_conexist4.Visible = true;
                    lbl_errormsg.Visible = false;
                    Fpitem.Sheets[0].PageSize = Fpitem.Sheets[0].RowCount;
                    Fpitem.SaveChanges();


                }

                else
                {


                    Fpitem.Visible = false;
                    Fpitem_div.Visible = false;
                    lbl_errormsg.Visible = true;
                    lbl_errormsg.Text = "No Records Found";
                    btn_conexist4.Visible = false;
                    btn_itemsave4.Visible = false;
                }
            }
            else
            {
                div1.Visible = false;
                Fpitem.Visible = false;
                Fpitem_div.Visible = false;
                lbl_errormsg.Visible = true;

                lbl_errormsg.Text = "Please Select Any Record";
            }
            txt_searchby.Text = "";
            txt_searchitemcode.Text = "";
            txt_searchheadername.Text = "";

        }
        catch
        {

        }
    }
    public void btn_itemsave4_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow1.Visible = false;


            string activerow = "";
            string activecol = "";
            activerow = Fpitem.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpitem.ActiveSheetView.ActiveColumn.ToString();

            if (activerow.Trim() != "-1" && activecol.Trim() != "-1")
            {
                itemcode = Fpitem.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                itempk = Convert.ToString(Fpitem.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                name = Fpitem.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
                measure = Fpitem.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text;
                txt_itemnameadd.Text = name;
                txt_measure.Text = measure;
                Session["itmpk"] = itempk;
                Session["itemname"] = name;
                Session["itemmeasure"] = measure;

            }
        }
        catch { }
    }

    public void btndeptsave_Click(object sender, EventArgs e)
    {

        try
        {

            Newdiv.Visible = false;
            string activerow = "";
            string activecol = "";
            activerow = Fpdept.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpdept.ActiveSheetView.ActiveColumn.ToString();
            if (activerow.Trim() != "-1" && activecol.Trim() != "-1")
            {
                dept = Fpdept.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                deptcode = Fpdept.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
                txt_deptadd.Text = dept;
                Session["depcode"] = deptcode;
                Session["dept"] = dept;
            }
        }
        catch { }


    }
    protected void btn_brkdetexit_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }


    protected void btn_brkdetsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_deptadd.Text.Trim() != "" && txt_itemnameadd.Text.Trim() != "" && txt_rollno.Text.Trim() != "" && txt_payamt.Text.Trim() != "" || txt_deptadd.Text.Trim() != "" && txt_itemnameadd.Text.Trim() != "" && txt_payamt.Text.Trim() != "")// txt_staffcode.Text.Trim() != "" &&
            {
                DataSet breakageDS = new DataSet();
                string qurey = "select * from HM_Feessetting where Type='Breakage' and collegecode=" + collegecode1 + "";
                breakageDS = d2.select_method_wo_parameter(qurey, "text");
                string header_val = string.Empty;
                string ledger_val = string.Empty;
                string inc_exc_messtype = string.Empty;
                if (breakageDS.Tables[0].Rows.Count > 0)
                {
                    header_val = Convert.ToString(breakageDS.Tables[0].Rows[0]["header"]);
                    ledger_val = Convert.ToString(breakageDS.Tables[0].Rows[0]["ledger"]);
                    inc_exc_messtype = Convert.ToString(breakageDS.Tables[0].Rows[0]["Text_value"]);
                }

                string textcode = "";
                int brekageinsert = 0;
                int feeallot = 0;
                string assetno = Convert.ToString(txt_assetno.Text);
                // string status = Convert.ToString(txt_status.Text);
                string reference = Convert.ToString(txt_narr.Text);
                string payamt = Convert.ToString(txt_payamt.Text);
                string getsemester = Convert.ToString(txt_sem.Text);
                string paymethod = "";
                //if (rdb_sel.Checked == true)
                //{
                //    paymethod = "1";
                //}
                //if (rdb_mgmt.Checked == true)
                //{

                //    paymethod = "2";
                //}

                string mmc = "";
                if (rdb_student.Checked == true || rdb_DayScholer.Checked == true || rdb_hostler.Checked == true)
                {
                    string rno = txt_rollno.Text;
                    mmc = d2.GetFunction("select App_No from Registration where Roll_No='" + rno + "'");
                }
                if (rdb_staff.Checked == true)
                {
                    string sno = txt_staffcode.Text;
                    mmc = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_code ='" + sno + "'");
                }
                if (rdb_Guest.Checked == true)//delsi092018
                {
                    string gno = txt_guestcode.Text;
                    mmc = gno;

                }
                string Financialyear = d2.GetFunction("select LinkValue  from InsSettings where LinkName = 'Current Financial Year'");
                int fincyr = Convert.ToInt32(Financialyear);
                string memtype = "";
                string hostler_dayscholer = string.Empty;

                if (rdb_student.Checked == true || rdb_DayScholer.Checked == true || rdb_hostler.Checked == true)
                {
                    memtype = "1";
                    if (rdb_DayScholer.Checked == true)
                    {
                        hostler_dayscholer = "0";
                    }
                    if (rdb_hostler.Checked == true)
                    {
                        hostler_dayscholer = "1";
                    }
                }
                else if (rdb_staff.Checked == true)
                {
                    memtype = "2";
                }
                else if (rdb_Guest.Checked == true)
                {
                    memtype = "3";
                }
                else
                {
                    memtype = "4";
                }
                string incstf = txt_pop1staffname.Text;
                string inc = d2.GetFunction("select appl_id from staffmaster s,staff_appl_master a where s.appl_no=a.appl_no and s.staff_name ='" + incstf + "'");
                //}
                //string inc = d2.GetFunction("select distinct staff_code from staffmaster where staff_name='" + incstf + "'");
                //string itmfk = txt_itemnameadd.Text;
                //string ifk = d2.GetFunction("select distinct ItemPK from IM_ItemMaster where ItemName='" + itmfk + "'");


                string insertquery = "insert into IT_BreakageDetails(DeptFK,ItemFK,AssetNo,InchargeStaff,BreakageBy,MemCode,ItemStatus,PayMethod,Remarks,HeaderFK,LedgerFK,PayAmount) values('" + Convert.ToString(Session["depcode"]) + "','" + Convert.ToString(Session["itmpk"]) + "','" + assetno + "','" + inc + "','" + ddl_breakgedbyadd.SelectedItem.Value + "','" + mmc + "','" + txt_status.SelectedItem.Value + "','" + paymethod + "','" + reference + "','" + header_val + "','" + ledger_val + "','" + payamt + "')";
                brekageinsert = d2.update_method_wo_parameter(insertquery, "Text");

                #region feecategory
                if (inc_exc_messtype == "0")
                {

                    if (rdb_student.Checked == true || rdb_staff.Checked == true || rdb_DayScholer.Checked == true || rdb_hostler.Checked == true)
                    {
                        if (rdb_student.Checked == true)
                        {
                            string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
                            ds.Clear();
                            ds = d2.select_method_wo_parameter(settingquery, "Text");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);

                                if (linkvalue == "0")
                                {
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + " Semester' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        textcode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                        Session["fee_category"] = Convert.ToString(textcode);
                                    }
                                }
                                else
                                {
                                    if (getsemester.Trim() == "1" || getsemester.Trim() == "2")
                                    {
                                        getsemester = "1 Year";
                                    }
                                    else if (getsemester.Trim() == "3" || getsemester.Trim() == "4")
                                    {
                                        getsemester = "2 Year";
                                    }
                                    else if (getsemester.Trim() == "5" || getsemester.Trim() == "6")
                                    {
                                        getsemester = "3 Year";
                                    }
                                    else if (getsemester.Trim() == "7" || getsemester.Trim() == "8")
                                    {
                                        getsemester = "4 Year";
                                    }
                                    else if (getsemester.Trim() == "9" || getsemester.Trim() == "10")
                                    {
                                        getsemester = "5 Year";
                                    }
                                    else if (getsemester.Trim() == "11" || getsemester.Trim() == "12")
                                    {
                                        getsemester = "6 Year";
                                    }
                                    string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval = '" + getsemester.Trim() + "' and textval not like '-1%'";
                                    ds.Clear();
                                    ds = d2.select_method_wo_parameter(semesterquery, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        textcode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                                        Session["fee_category"] = Convert.ToString(textcode);
                                    }
                                }
                            }
                        }
                #endregion// ddl_ledger.SelectedItem.Value--- ddl_header.SelectedItem.Value

                        string feeallottable = "if exists (select * from FT_FeeAllot where LedgerFK ='" + ledger_val + "' and HeaderFK ='" + header_val + "' and FeeCategory ='" + textcode + "' and  FinYearFK='" + fincyr + "' and App_No ='" + mmc + "') update FT_FeeAllot set AllotDate='" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "',MemType='" + memtype + "',FeeAmount='" + payamt + "',TotalAmount='" + payamt + "',BalAmount='" + payamt + "' where LedgerFK ='" + ledger_val + "' and HeaderFK ='" + header_val + "' and FeeCategory in('" + textcode + "') and  FinYearFK='" + fincyr + "' and App_No ='" + mmc + "' else INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount, FeeCategory,DueAmount,FineAmount,BalAmount,FinYearFK,PayMode) VALUES('" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "','" + memtype + "','" + mmc + "','" + ledger_val + "','" + header_val + "','" + payamt + "','0','0','0','" + payamt + "','0','" + textcode + "','0','0','" + payamt + "','" + fincyr + "','0')";

                        feeallot = d2.update_method_wo_parameter(feeallottable, "Text");
                    }
                    else if (rdb_student.Checked == false && rdb_staff.Checked == false && rdb_DayScholer.Checked == false && rdb_hostler.Checked == false)
                    {
                        string feeallottable = "if exists (select * from FT_FeeAllot where LedgerFK ='" + ledger_val + "' and HeaderFK ='" + header_val + "' and  FinYearFK='" + fincyr + "' and App_No ='" + mmc + "') update FT_FeeAllot set AllotDate='" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "',MemType='" + memtype + "',FeeAmount='" + payamt + "',TotalAmount='" + payamt + "',BalAmount='" + payamt + "' where LedgerFK ='" + ledger_val + "' and HeaderFK ='" + header_val + "'  and  FinYearFK='" + fincyr + "' and App_No ='" + mmc + "' else INSERT INTO FT_FeeAllot (AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount, FeeCategory,DueAmount,FineAmount,BalAmount,FinYearFK,PayMode) VALUES('" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "','" + memtype + "','" + mmc + "','" + ledger_val + "','" + header_val + "','" + payamt + "','0','0','0','" + payamt + "','0','0','0','0','" + payamt + "','" + fincyr + "','0')";

                        //string feeallottable = "if exists (select * from FT_FeeAllot where LedgerFK ='" + ledger_val + "' and HeaderFK ='" + header_val + "' and  FinYearFK='" + fincyr + "' and MemType='3') update FT_FeeAllot set AllotDate='" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "',MemType='" + memtype + "',FeeAmount='" + payamt + "',TotalAmount='" + payamt + "',BalAmount='" + payamt + "' where LedgerFK ='" + header_val + "' and HeaderFK ='" + header_val + "' and  FinYearFK='" + fincyr + "'  and MemType='3' else INSERT INTO FT_FeeAllot (AllotDate,MemType,LedgerFK,HeaderFK,FeeAmount,DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount, DueAmount,FineAmount,BalAmount,FinYearFK,PayMode,app_no,FeeCategory) VALUES('" + Convert.ToString(DateTime.Now.ToString("MM/dd/yyyy")) + "','" + memtype + "','" + ledger_val + "','" + header_val + "','" + payamt + "','0','0','0','" + payamt + "','0','0','0','" + payamt + "','" + fincyr + "','0','0','0')";
                        feeallot = d2.update_method_wo_parameter(feeallottable, "Text");
                    }
                    if (feeallot == 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Not Saved";
                        return;
                    }
                }
                if (brekageinsert != 0)//&& feeallot != 0
                {
                    bindrequestcode();
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Saved Successfully ";
                    Clear();
                    btn_go_Click(sender, e);
                }

            }
            else
            {
                stdlblerr.Visible = true;
                stdlblerr.Text = "Please Enter Mandatory Fields";
            }
        }
        catch
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = "Some Issues Occurs ";
        }
    }

    protected void btnerrclose1_Click(object sender, EventArgs e)
    {
        //ddl_breakagedby.Items.Add(new ListItem("select", "0"));
        //ddl_breakagedby.Items.Add(new ListItem("student", "1"));
        //ddl_breakagedby.Items.Add(new ListItem("staff", "2"));
        //ddl_breakagedby.Items.Add(new ListItem("unknown", "3"));
        alertpopwindow.Visible = false;
        poperrjs.Visible = false;
        txt_deptadd.Text = "";
        txt_itemnameadd.Text = "";
        rdb_student.Checked = false;
        rdb_staff.Checked = false;
        rdb_mgmt.Checked = false;
        rdb_sel.Checked = false;
        txt_rollno.Text = "";
        txt_measure.Text = "";
        txt_name.Text = "";
        txt_narr.Text = "";
        txt_payamt.Text = "";
        txt_deg.Text = "";
        txt_sem.Text = "";
        txt_pop1staffname.Text = "";
        ImageButton3.Visible = false;
        ImageButton4.Visible = false;
        //ddl_breakagedby.Items.Clear();
        ddl_header.Items.Clear();
        ddl_ledger.Items.Clear();
        txt_sec.Text = "";
        txt_deptstu.Text = "";
        txt_deptstaff.Text = "";
        // txt_status.Items.Clear();
        txt_staffname.Text = "";
        txt_des.Text = "";
    }
    protected void Clear()
    {
        txt_deptadd.Text = "";
        txt_itemnameadd.Text = "";
        txt_rollno.Text = "";
        txt_measure.Text = "";
        txt_name.Text = "";
        txt_narr.Text = "";
        txt_payamt.Text = "";
        txt_deg.Text = "";
        txt_sem.Text = "";
        txt_pop1staffname.Text = "";
        txt_sec.Text = "";
        txt_deptstu.Text = "";
        txt_deptstaff.Text = "";
        txt_staffname.Text = "";
        txt_des.Text = "";
        lbl_errorsearch1.Text = "";
        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        txt_status.SelectedIndex = 0;
    }
    //protected void btnquestion_Click(object sender, EventArgs e)
    //{

    //}
    //[System.Web.Services.WebMethod]
    //[System.Web.Script.Services.ScriptMethod()]
    //public static List<string> Getrno(string prefixText)
    //{
    //    WebService ws = new WebService();
    //    List<string> name = new List<string>();
    //    //string query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and Stud_Type ='Hostler' and Roll_No like '" + prefixText + "%' order by stud_name";
    //    string query = "select r.Roll_No from Registration as r join Hostel_StudentDetails as hs on r.Roll_Admit=hs.Roll_Admit join Hostel_Details as hd on hs.Hostel_Code=hd.Hostel_Code where r.Delflag=0 and r.cc=0 and r.roll_no like '" + prefixText + "%' order by r.Roll_No desc";

    //    name = ws.Getname(query);
    //    return name;
    //}
    protected void btn_staffquestion_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = true;
        Fpstaff.Visible = false;
        div1.Visible = false;
        btn_staffsave.Visible = false;
        btn_staffexit.Visible = false;
        lbl_errorsearch1.Text = "";
        txt_staffcodesearch.Text = "";
        txt_staffnamesearch.Text = "";
        bindstaffdepartmentpopup();

    }
    protected void btn_staffcode_Click(object sender, EventArgs e)
    {
        btn_staffquestion_Click(sender, e);
    }
    public void bindstaffdepartmentpopup()
    {
        try
        {
            ds.Clear();
            //string query = "";
            //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + collegecode1 + "'";
            ds = d2.loaddepartment(collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_department3.DataSource = ds;
                ddl_department3.DataTextField = "dept_name";
                ddl_department3.DataValueField = "dept_code";
                ddl_department3.DataBind();

                ddl_department3.Items.Insert(0, "All");
            }

        }
        catch { }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffName(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetItemName()
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct ItemName from IM_ItemMaster where itemtype='1' ";
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetDeptName()
    {

        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select distinct Dept_Name from Department ";
        name = ws.Getname(query);
        return name;

    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> GetStaffCode(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_code from staffmaster where resign =0 and settled =0 and staff_code like  '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }
    protected void btn_staffselectgo_Click(object sender, EventArgs e)
    {
        try
        {
            int rolcount = 0;
            int sno = 0;
            string sql = "";
            int rowcount;
            //Fpstaff.Visible = true;
            if (txt_staffnamesearch.Text != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 0)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_staffnamesearch.Text) + "' order by s.staff_code";
                }
            }
            else if (txt_staffcodesearch.Text.Trim() != "")
            {
                if (ddl_searchbystaff.SelectedIndex == 1)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_staffcodesearch.Text) + "' order by s.staff_code";
                }
            }
            else
            {
                if (ddl_department3.SelectedItem.Text == "All")
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code order by s.staff_code";
                }
                else
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department3.SelectedItem.Value + "') order by s.staff_code";
                }
            }
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.SaveChanges();
            Fpstaff.SheetCorner.ColumnCount = 0;
            Fpstaff.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpstaff.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
            Fpstaff.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            Fpstaff.Sheets[0].AutoPostBack = true;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;

            if (ds.Tables[0].Rows.Count > 0)
            {

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[0].Locked = true;
                Fpstaff.Columns[0].Width = 80;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Staff Code";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[1].Locked = true;
                Fpstaff.Columns[1].Width = 100;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Staff Name";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[2].Locked = true;
                Fpstaff.Columns[2].Width = 200;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[3].Locked = true;
                Fpstaff.Columns[3].Width = 250;

                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Designation";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpstaff.Columns[4].Width = 200;
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpstaff.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpstaff.Sheets[0].Columns[4].Locked = true;
                Fpstaff.Width = 700;

                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;
                    //Fpstaff.Sheets[0].RowCount++;
                    //name = ds.Tables[0].Rows[rolcount]["staff_name"].ToString();
                    //code = ds.Tables[0].Rows[rolcount]["staff_code"].ToString();

                    Fpstaff.Sheets[0].RowCount = Fpstaff.Sheets[0].RowCount + 1;
                    //Fpstaff.Sheets[0].Rows[Fpstaff.Sheets[0].RowCount - 1].Font.Bold = false;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["staff_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["dept_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_name"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[rolcount]["desig_code"]);
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpstaff.Sheets[0].Cells[Fpstaff.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                }
                lbl_errorsearch1.Visible = true;
                lbl_errorsearch1.Text = "No Records Found";
                lbl_errorsearch1.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                btn_staffsave.Visible = true;
                btn_staffexit.Visible = true;
                Fpstaff.Visible = true;
                div1.Visible = true;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                Fpstaff.Visible = false;
                btn_staffsave.Visible = false;
                btn_staffexit.Visible = false;
                div1.Visible = false;
                //err.Visible = true;
                //err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            check = true;
        }
        catch
        {
        }
    }
    protected void Fpspread1_render(object sender, EventArgs e)
    {
        if (check == true)
        {
            string activerow = "";
            string activecol = "";

            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();

            if (activerow.Trim() != "")
            {
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].BackColor = Color.DarkCyan;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].BackColor = Color.DarkCyan;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].BackColor = Color.DarkCyan;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].BackColor = Color.DarkCyan;
                Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 0].BackColor = Color.DarkCyan;
            }
        }
    }
    protected void btn_staffsave_Click(object sender, EventArgs e)
    {
        try
        {
            //string name = "";
            string stfcode = "";
            //string batch = "";
            //string deg = "";
            //string dept = "";
            //string sem = "";
            //string sec = "";
            string activerow = "";
            string activecol = "";

            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
            stfcode = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_Code='" + stfcode + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_staffcode.Text = ds.Tables[0].Rows[i]["staff_Code"].ToString();
                    if (rdb_staff.Checked == true)
                    {
                        txt_staffname.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                        txt_stafftype.Text = ds.Tables[0].Rows[i]["staffcategory"].ToString();
                    }
                    if (rdb_student.Checked == true)
                    {
                        txt_pop1staffname.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                        Session["stfcode"] = ds.Tables[0].Rows[i]["staff_Code"].ToString();

                    }
                    txt_deptstaff.Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    txt_des.Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    //txt_stafftype.Text = ds.Tables[0].Rows[i][""].ToString();
                    txt_pop1staffname.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();

                    ImageButton4.Visible = true;
                    ImageButton4.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Staff_Code;

                }
            }
            else
            {
                txt_staffcode.Text = "";
                txt_staffname.Text = "";

                txt_deptstaff.Text = "";
                txt_des.Text = "";
                txt_stafftype.Text = "";

            }



            popupstaffcode1.Visible = false;




        }
        catch (Exception ex)
        {
        }



        //try
        //{
        //    if (txt_staffcodesearch.Text != "" || txt_staffnamesearch.Text != "" || ddl_searchbystaff.SelectedIndex != -1)
        //    {
        //        if (Fpstaff.Visible == true)
        //        {
        //            // popwindow1.Visible = true;
        //            popupstaffcode1.Visible = false;
        //            //btn_pop1save.Visible = true;
        //            //btn_pop1exit.Visible = true;
        //            //btn_pop1update.Visible = false;
        //            //btn_pop1delete.Visible = false;
        //            //btn_pop1exit1.Visible = false;
        //            string activerow = "";
        //            string activecol = "";
        //            string sql = "";
        //            activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
        //            activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();

        //            if (activerow.Trim() != "")
        //            {
        //                //Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor = Color.DarkCyan;
        //                string StaffCode = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
        //                string applno = d2.GetFunction("select appl_no from staffmaster where staff_code='" + StaffCode + "'");
        //                sql = "select convert(varchar,convert(datetime,date_of_birth,103),103) from staff_appl_master where appl_no='" + applno + "'";
        //                string StaffDob = d2.GetFunction(sql);
        //                string StaffName = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
        //                string StaffDepartment = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
        //                string StaffDesignation = Convert.ToString(Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
        //                if (rdb_student.Checked == true)
        //                {
        //                    txt_pop1staffname.Text = StaffName;
        //                }
        //                else
        //                {
        //                    txt_staffcode.Text = StaffCode;
        //                    txt_staffname.Text = StaffName;
        //                    txt_deptstaff.Text = StaffDepartment;
        //                    txt_des.Text = StaffDesignation;
        //                    //txt_pop1dob.Text = StaffDob;
        //                }
        //            }
        //            txt_staffcodesearch.Text = "";
        //            txt_staffnamesearch.Text = "";
        //        }
        //        else
        //        {
        //            imgdiv2.Visible = true;
        //            lbl_alerterr.Text = "No records found";
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //}
    }
    protected void btn_staffexit_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }
    protected void imagebtnpopclose2_Click(object sender, EventArgs e)
    {
        popupstaffcode1.Visible = false;
    }
    protected void ddl_searchbystaff_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_searchbystaff.SelectedItem.Text == "Staff Name")
        {
            txt_staffnamesearch.Visible = true;
            txt_staffcodesearch.Visible = false;
            txt_staffnamesearch.Text = "";

        }
        else if (ddl_searchbystaff.SelectedItem.Text == "Staff Code")
        {
            txt_staffcodesearch.Visible = true;
            txt_staffnamesearch.Visible = false;
            txt_staffnamesearch.Text = "";
        }
    }
    public void loadcollegestaffpopup()
    {
        try
        {
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college2.DataSource = ds;
                ddl_college2.DataTextField = "collname";
                ddl_college2.DataValueField = "college_code";
                ddl_college2.DataBind();
            }
            //binddept(ddl_collegename.SelectedItem.Value.ToString());
        }
        catch
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getrno(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            query = "select Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " ";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }


    protected void btn_roll_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindbatch1();
        binddegree2();
        bindbranch1();
        btn_studOK.Visible = false;
        btn_exitstud.Visible = false;
        Fpspread1.Visible = false;
    }
    public void bindbatch1()
    {
        try
        {
            ddl_batch1.Items.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method_wo_parameter(sqlyear, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch1.DataSource = ds;
                ddl_batch1.DataTextField = "batch_year";
                ddl_batch1.DataValueField = "batch_year";
                ddl_batch1.DataBind();
            }
        }
        catch
        {
        }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + collegecode1 + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree2.DataSource = ds;
                cbl_degree2.DataTextField = "course_name";
                cbl_degree2.DataValueField = "course_id";
                cbl_degree2.DataBind();
                if (cbl_degree2.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree2.Items.Count; i++)
                    {
                        cbl_degree2.Items[i].Selected = true;
                    }
                    txt_degree2.Text = "Degree(" + cbl_degree2.Items.Count + ")";
                }
                else
                {
                    txt_degree2.Text = "--Select--";
                }
            }
            else
            {
                txt_degree2.Text = "--Select--";
            }
        }

        catch
        {

        }
    }
    public void bindbranch1()
    {
        try
        {
            cbl_branch1.Items.Clear();

            string branch = "";
            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    if (branch == "")
                    {
                        branch = "" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        branch = branch + "'" + "," + "" + "'" + cbl_degree2.Items[i].Value.ToString() + "";
                    }
                }
            }
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            if (branch.Trim() != "")
            {
                ds = d2.select_method_wo_parameter(commname, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch1.DataSource = ds;
                    cbl_branch1.DataTextField = "dept_name";
                    cbl_branch1.DataValueField = "degree_code";
                    cbl_branch1.DataBind();



                    if (cbl_branch1.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_branch1.Items.Count; i++)
                        {
                            cbl_branch1.Items[i].Selected = true;
                        }
                        txt_branch2.Text = "Branch(" + cbl_branch1.Items.Count + ")";
                    }
                }
                else
                {
                    txt_branch2.Text = "--Select--";
                }
            }
            else
            {
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void stdbtn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread1.SaveChanges();
            string itemheader = "";
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_branch1.Items[i].Value.ToString() + "";
                    }
                }
            }

            string batch_year = Convert.ToString(ddl_batch1.SelectedItem.Text);


            if (txt_rollno3.Text == "")
            {
                selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Batch_Year =" + batch_year + " and r.degree_code in ('" + itemheader + "')  order by Roll_No,d.Degree_Code ";
            }
            else
            {
                selectquery = "select Roll_No,Roll_Admit,Stud_Name,d.Degree_Code ,(C.Course_Name +' - '+ dt.Dept_Name) as Department,Reg_No  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and dt.Dept_Code =d.Dept_Code and c.Course_Id =d.Course_Id and CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR'  and Roll_No ='" + txt_rollno3.Text + "'  ";

            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 1;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 5;

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                //chkall.AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Columns[1].Width = 150;
                Fpspread1.Sheets[0].Columns[1].Locked = false;
                Fpspread1.Sheets[0].Columns[1].Visible = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 150;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Reg No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Department";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 250;

                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Semester";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Columns[5].Locked = true;
                //Fpspread1.Columns[5].Width = 200;

                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Section";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Sheets[0].Columns[6].Locked = true;
                //Fpspread1.Columns[6].Width = 250;

                Fpspread1.Sheets[0].Columns[3].Visible = true;
                Fpspread1.Sheets[0].Columns[4].Visible = true;
                Fpspread1.Sheets[0].Columns[2].Visible = true;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = false;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = txt;

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["Department"]);
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                }
                Fpspread1.SaveChanges();
                Fpspread1.Visible = true;

                Fpspread1.SaveChanges();
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
            }
            else
            {
                Fpspread1.Visible = false;
                // lbl_errormsg.Visible = true;
                // lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
                // btn_exitstud_Click.Visible = false;
            }

        }
        catch (Exception ex)
        {
        }
    }


    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch2.Text = "--Select--";
            cb_branch1.Checked = false;
            for (int i = 0; i < cbl_branch1.Items.Count; i++)
            {
                if (cbl_branch1.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_branch2.Text = "Branch(" + commcount.ToString() + ")";
                if (commcount == cbl_branch1.Items.Count)
                {
                    cb_branch1.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch1.Checked == true)
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = true;
                }
                txt_branch2.Text = "Branch(" + (cbl_branch1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    cbl_branch1.Items[i].Selected = false;
                }
                txt_branch2.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree2.Checked = false;

            for (int i = 0; i < cbl_degree2.Items.Count; i++)
            {
                if (cbl_degree2.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }

            if (seatcount == cbl_degree2.Items.Count)
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree2.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree2.Text = "--Select--";
            }
            else
            {
                txt_degree2.Text = "Degree(" + seatcount.ToString() + ")";
            }
            bindbranch1();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_degree2.Checked == true)
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = true;
                }
                txt_degree2.Text = "Degree(" + (cbl_degree2.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_degree2.Items.Count; i++)
                {
                    cbl_degree2.Items[i].Selected = false;
                }
                txt_degree2.Text = "--Select--";
            }
            bindbranch1();
        }
        catch (Exception ex)
        {
        }
    }


    public void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            string roll = "";
            string activerow = "";
            string activecol = "";
            activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
            activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
            roll = Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Current_Semester ,r.Sections ,r.Batch_Year,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State  from applyn a,Registration r ,Degree d,course c,Department dt where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code  and r.Roll_no='" + roll + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_rollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                    txt_name.Text = ds.Tables[0].Rows[i]["stud_name"].ToString();

                    txt_deg.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                    txt_deptstu.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString(); ;
                    txt_sem.Text = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                    txt_sec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                    ImageButton3.Visible = true;
                    ImageButton3.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + roll;
                }
            }
            else
            {
                txt_rollno.Text = "";
                txt_name.Text = "";

                txt_deg.Text = "";
                txt_deptstu.Text = "";
                txt_sem.Text = "";
                txt_sec.Text = "";
            }
            popwindow.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    public void txt_deptadd_OnTextChanged(object sender, EventArgs e)
    {
        string deptcode = "";

        string dptname = txt_deptadd.Text;
        deptcode = d2.GetFunction("select dept_code from Department where Dept_Name ='" + dptname + "'");
        Session["depcode"] = deptcode;

    }


    public void txt_itemnameadd_OnTextChanged(object sender, EventArgs e)
    {
        Item_Name = txt_itemnameadd.Text.ToString();
        getitemcode(Item_Name);
        btn_itemsave4_Click(sender, e);
        string itempk = "";
        //if (Convert.ToString(Session["itmpk"]) != "" && Convert.ToString(Session["itmpk"]) != "0")
        //{
        string itemname = txt_itemnameadd.Text;
        itempk = d2.GetFunction("select ItemPK  from IM_ItemMaster  where ItemName ='" + itemname + "'");
        Session["itmpk"] = itempk;

        //}
        //else
        //{

        //itempk = Convert.ToString(Session["itmpk"]);
        //}
    }

    public void txt_pop1staffname_OnTextChanged(object sender, EventArgs e)
    {

        string istf = txt_pop1staffname.Text;
        string staffcode = d2.GetFunction("select appl_id from staff_appl_master  a,staffmaster s where a.appl_no =s.appl_no and s.staff_name ='" + istf + "'");
        if (staffcode.Trim() != "0")
        {
            Session["stfcode"] = staffcode;
        }
        else
        {
            txt_pop1staffname.Text = "";
        }
    }




    public void getitemcode(string Item_name)
    {
        try
        {
            string query = "select distinct ItemHeaderName, ItemCode,ItemName,ItemUnit from IM_ItemMaster where itemtype='1'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_itemnameadd.Text = ds.Tables[0].Rows[i]["ItemName"].ToString();
                    txt_measure.Text = ds.Tables[0].Rows[i]["ItemUnit"].ToString();
                }
            }
        }
        catch { }
    }

    public void txt_staffcode_OnTextChanged(object sender, EventArgs e)
    {
        Staff_Code = txt_staffcode.Text.ToString();
        getstaffcode(Staff_Code);


    }

    public void getstaffcode(string Staff_Code)
    {
        try
        {
            string query = "select s.staff_Code,s.staff_name,dm.desig_name,hr.dept_name,sa.ccity,dm.staffcategory,sa.comm_address,sa.comm_address1,sa.com_mobileno,sa.com_phone,sa.cstate,sa.email,sa.com_pincode,CONVERT(varchar(10), s.join_date,103) as join_date  from staffmaster s,staff_appl_master sa,hrdept_master hr,desig_master dm where s.appl_no=sa.appl_no and sa.dept_code=hr.dept_code and dm.desig_code=sa.desig_code and settled=0 and resign =0 and s.staff_Code='" + Staff_Code + "'";

            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_staffcode.Text = ds.Tables[0].Rows[i]["staff_Code"].ToString();
                    txt_staffname.Text = ds.Tables[0].Rows[i]["staff_name"].ToString();
                    txt_deptstaff.Text = ds.Tables[0].Rows[i]["dept_name"].ToString();
                    txt_des.Text = ds.Tables[0].Rows[i]["desig_name"].ToString();
                    // txt_stafftype.Text = ds.Tables[0].Rows[i][""].ToString();

                    ImageButton4.Visible = true;
                    ImageButton4.ImageUrl = "~/Handler/Handler4.ashx?rollno=" + Staff_Code;
                }
            }
            else
            {
                txt_staffcode.Text = "";
                txt_staffname.Text = "";
                txt_deptstaff.Text = "";
                txt_des.Text = "";
            }
            string photo = d2.GetFunction("select photo from staffphoto where staff_code='" + Staff_Code + "'");
            if (photo == "0")
            {
                ImageButton4.ImageUrl = "images/photodummy.png";
            }


        }
        catch
        {

        }
    }

    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
        // rdo_receipt.Checked = true;
    }
    protected void ddl_breakgedbyadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_itemnameadd.Text = Convert.ToString(Session["itemname"]);
            txt_measure.Text = Convert.ToString(Session["itemmeasure"]);
            txt_deptadd.Text = Convert.ToString(Session["dept"]);
            if (ddl_breakgedbyadd.SelectedValue == "1")
            {

                rdb_student.Enabled = true;
                rdb_student.Checked = true;
                rdb_student_CheckedChanged(sender, e);
                rdb_staff.Enabled = false;
                rdb_staff.Checked = false;
                rdb_Guest.Enabled = false;
                rdb_Guest.Checked = false;
                btn_guestLookup.Visible = false;
                lbl_guestCode.Visible = false;
                txt_guestcode.Visible = false;
                lbl_guestName.Visible = false;
                txt_guestName.Visible = false;

            }
            if (ddl_breakgedbyadd.SelectedValue == "2")
            {

                rdb_staff.Enabled = true;
                rdb_staff.Checked = true;
                rdb_staff_CheckedChanged(sender, e);
                rdb_student.Enabled = false;
                rdb_student.Checked = false;
                rdb_Guest.Enabled = false;
                rdb_Guest.Checked = false;
                lbl_guestCode.Visible = false;
                txt_guestName.Visible = false;
                txt_guestcode.Visible = false;
                lbl_guestName.Visible = false;
                btn_guestLookup.Visible = false;
                lbl_guestCode.Visible = false;
                txt_guestcode.Visible = false;
                lbl_guestName.Visible = false;
                txt_guestName.Visible = false;

            }
            if (ddl_breakgedbyadd.SelectedValue == "3")
            {
                //rdb_mgmt.Checked = true;

                btn1sturoll.Visible = false;
                btn_guestLookup.Visible = true;
                fs_student.Visible = false;
                rdb_staff.Enabled = false;
                rdb_staff.Checked = false;
                rdb_student.Enabled = false;
                rdb_student.Checked = false;
                btn_guestLookup.Visible = true;
                rdb_Guest.Enabled = true;
                rdb_Guest.Checked = true;
                lbl_guestCode.Visible = true;
                txt_guestName.Visible = true;
                txt_guestName.Text = "";
                txt_guestcode.Visible = true;
                txt_guestcode.Text = "";
                lbl_guestName.Visible = true;
                // lbl_sltheader.Visible = true;
                // ddl_header.Visible = true;
                // lbl_ledger.Visible = true;
                txt_status.Visible = true;
                lbl_statuspay.Visible = true;
                //  ddl_ledger.Visible = true;
                lbl_narr.Visible = true;
                txt_narr.Visible = true;
                lbl_payamt.Visible = true;
                txt_payamt.Visible = true;

                lbl_pop1staffname.Visible = true;
                sppay.Visible = true;

                //else
                //{
                //    rdb_student.Enabled = false;
                //    rdb_staff.Enabled = false;
                //    lbl_sltheader.Visible = false;
                //    rdb_staff.Checked = false;
                //    rdb_student.Checked = false;
                //    ddl_header.Visible = false;
                //    lbl_ledger.Visible = false;
                //    txt_status.Visible = false;
                //    lbl_statuspay.Visible = false;
                //    ddl_ledger.Visible = false;
                //    lbl_narr.Visible = false;
                //    txt_narr.Visible = false;
                //    lbl_payamt.Visible = false;
                //    txt_payamt.Visible = false;
                //    sppay.Visible = false;
                //}
                //  lbl_pay.Visible = true;
                //  rdb_sel.Visible = false;
                // rdb_mgmt.Visible = true;

                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                btn_roll.Visible = false;
                roll.Visible = false;
                lbl_name.Visible = false;
                txt_name.Visible = false;
                lbl_deg.Visible = false;
                txt_deg.Visible = false;
                lbl_deptstu.Visible = false;
                txt_deptstu.Visible = false;

                lbl_sem.Visible = false;
                txt_sem.Visible = false;
                lbl_sec.Visible = false;
                txt_sec.Visible = false;

                lbl_pop1staffname.Visible = false;
                txt_pop1staffname.Visible = false;
                btn_staffquestion.Visible = false;
                staff.Visible = false;
                lbl_photo.Visible = false;
                ImageButton3.Visible = false;

                lbl_staffcode.Visible = false;
                txt_staffcode.Visible = false;
                btn_staffcode.Visible = false;
                code.Visible = false;
                lbl_staffname.Visible = false;
                txt_staffname.Visible = false;

                lbl_deptstaff.Visible = false;
                txt_deptstaff.Visible = false;
                lbl_des.Visible = false;
                txt_des.Visible = false;

                lbl_stafftypr.Visible = false;
                txt_stafftype.Visible = false;
                lbl_staffphoto.Visible = false;
                ImageButton4.Visible = false;
            }
            if (ddl_breakgedbyadd.SelectedValue == "0")
            {
                fs_student.Visible = false;
                lbl_rollno.Visible = false;
                txt_rollno.Visible = false;
                lbl_deg.Visible = false;
                txt_deg.Visible = false;
                lbl_sem.Visible = false;
                txt_pop1staffname.Visible = false;
                btn_staffquestion.Visible = false;
                btn_roll.Visible = false;
                lbl_name.Visible = false;
                txt_name.Visible = false;
                lbl_deptstu.Visible = false;
                txt_deptstu.Visible = false;
                txt_sec.Visible = false;
                lbl_sec.Visible = false;
                lbl_photo.Visible = false;
                ImageButton3.Visible = false;
                btn1sturoll.Visible = false;
                lbl_pop1staffname.Visible = false;
                lbl_pop1staffname.Visible = false;
                staff.Visible = false;
                roll.Visible = false;
                txt_sem.Visible = false;


                lbl_staffcode.Visible = false;
                txt_staffcode.Visible = false;
                btn_staffcode.Visible = false;
                lbl_staffname.Visible = false;
                txt_staffname.Visible = false;
                lbl_deptstaff.Visible = false;
                txt_deptstaff.Visible = false;
                lbl_des.Visible = false;
                txt_des.Visible = false;
                lbl_stafftypr.Visible = false;
                txt_stafftype.Visible = false;
                lbl_staffphoto.Visible = false;
                ImageButton4.Visible = false;
                code.Visible = false;

                lbl_guestCode.Visible = false;
                txt_guestcode.Visible = false;
                btn_guestLookup.Visible = false;
                lbl_guestName.Visible = false;
                txt_guestName.Visible = false;

                rdb_Guest.Checked = false;
                rdb_staff.Checked = false;
                rdb_student.Checked = false;

            }
            //Clear();
        }
        catch
        {
        }

    }

    public void loadledger()
    {
        try
        {
            ds.Clear();
            ddl_ledger.Items.Clear();
            string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.CollegeCode =" + collegecode1 + "";
            // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.CollegeCode =" + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_ledger.DataSource = ds;
                ddl_ledger.DataTextField = "LedgerName";
                ddl_ledger.DataValueField = "LedgerPK";
                ddl_ledger.DataBind();
            }
        }
        catch
        {
        }
    }

    public void loadHeader()
    {
        try
        {
            ds.Clear();
            ddl_header.Items.Clear();
            string queryddlh = "select distinct HeaderName,HeaderPK from FM_HeaderMaster where CollegeCode='" + collegecode1 + "'";
            // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.CollegeCode =" + collegecode1 + "";
            ds.Clear();
            ds = d2.select_method_wo_parameter(queryddlh, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_header.DataSource = ds;
                ddl_header.DataTextField = "HeaderName";
                ddl_header.DataValueField = "HeaderPK";
                ddl_header.DataBind();
            }
        }
        catch
        {
        }
    }
    public void bindrequestcode()
    {
        try
        {

            string newitemcode = "";

            //string selectquery = "select Requestcode,RequestType from RQ_Requisition";
            string selectquery = "select AssetAcr,AssetStNo,AssetSize from IM_CodeSettings order by startdate desc";
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string ordcode = Convert.ToString(ds.Tables[0].Rows[0]["AssetAcr"]);
                string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["AssetAcr"]);
                string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["AssetStNo"]);
                string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["AssetSize"]);
                selectquery = "select top(1) AssetNo  from IT_BreakageDetails where AssetNo like '" + Convert.ToString(ordcode) + "%' order by AssetNo desc";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["AssetNo"]);
                    string itemacr = Convert.ToString(itemacronym);
                    int len = itemacr.Length;
                    itemcode = itemcode.Remove(0, len);
                    int len1 = Convert.ToString(itemcode).Length;
                    string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                    len = Convert.ToString(newnumber).Length;
                    len1 = Convert.ToInt32(itemsize) - len;
                    if (len1 == 2)
                    {
                        newitemcode = "00" + newnumber;
                    }
                    else if (len1 == 1)
                    {
                        newitemcode = "0" + newnumber;
                    }
                    else if (len1 == 4)
                    {
                        newitemcode = "0000" + newnumber;
                    }
                    else if (len1 == 3)
                    {
                        newitemcode = "000" + newnumber;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(newnumber);
                    }
                    if (newitemcode.Trim() != "")
                    {
                        newitemcode = itemacr + "" + newitemcode;
                    }
                }
                else
                {
                    string itemacr = Convert.ToString(itemstarno);
                    int len = itemacr.Length;

                    string items = Convert.ToString(itemsize);
                    int len1 = Convert.ToInt32(items);//items.Length;

                    int size = len1 - len;//Convert.ToInt32(itemacr);

                    if (size == 2)
                    {
                        newitemcode = "00" + itemstarno;
                    }
                    else if (size == 1)
                    {
                        newitemcode = "0" + itemstarno;
                    }
                    else if (size == 4)
                    {
                        newitemcode = "0000" + itemstarno;
                    }
                    else if (size == 3)
                    {
                        newitemcode = "000" + itemstarno;
                    }
                    else
                    {
                        newitemcode = Convert.ToString(itemstarno);
                    }
                    newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                    // newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(itemstarno);
                }
                txt_assetno.Text = Convert.ToString(newitemcode);
            }
        }
        catch
        {

        }
    }
    public void rdb_Guest_CheckedChanged(object sender, EventArgs e)
    {


    }
    public void rdb_dayscholer_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_DayScholer.Checked == true)
        {

            lbl_rollno.Visible = true;
            txt_rollno.Visible = true;
            fs_student.Visible = true;
            btn1sturoll.Visible = false;
            btn_roll.Visible = true;
            roll.Visible = true;
            lbl_name.Visible = true;
            txt_name.Visible = true;
            lbl_deg.Visible = true;
            txt_deg.Visible = true;
            lbl_deptstu.Visible = true;
            txt_deptstu.Visible = true;

            lbl_sem.Visible = true;
            txt_sem.Visible = true;
            lbl_sec.Visible = true;
            txt_sec.Visible = true;

            lbl_pop1staffname.Visible = true;
            txt_pop1staffname.Visible = true;
            btn_staffquestion.Visible = true;
            staff.Visible = true;
            lbl_photo.Visible = true;
            ImageButton3.Visible = true;

            lbl_staffcode.Visible = false;
            txt_staffcode.Visible = false;
            btn_staffcode.Visible = false;
            code.Visible = false;
            lbl_staffname.Visible = false;
            txt_staffname.Visible = false;

            lbl_deptstaff.Visible = false;
            txt_deptstaff.Visible = false;
            lbl_des.Visible = false;
            txt_des.Visible = false;


            lbl_stafftypr.Visible = false;
            txt_stafftype.Visible = false;
            lbl_staffphoto.Visible = false;
            ImageButton4.Visible = false;



            txt_status.Visible = true;//delsi0803
            lbl_statuspay.Visible = true;
            lbl_narr.Visible = true;
            txt_narr.Visible = true;
            lbl_payamt.Visible = true;
            txt_payamt.Visible = true;
            sppay.Visible = true;

            //lbl_pay.Visible = true;
            // rdb_sel.Visible = true;
            // rdb_mgmt.Visible = true;

        }
        else
        {
        }
    }

    public void rdb_hostler_CheckedChanged(object sender, EventArgs e)
    {
        btn1sturoll.Visible = true;
        fs_student.Visible = true;
        btn_roll.Visible = false;
        txt_sem.Visible = false;
        lbl_sem.Visible = false;
        lbl_sec.Visible = false;
        txt_sec.Visible = false;




        lbl_rollno.Visible = true;
        txt_rollno.Visible = true;
        fs_student.Visible = true;
        // btn1sturoll.Visible = false;
        // btn_roll.Visible = true;
        roll.Visible = true;
        lbl_name.Visible = true;
        txt_name.Visible = true;
        lbl_deg.Visible = true;
        txt_deg.Visible = true;
        lbl_deptstu.Visible = true;
        txt_deptstu.Visible = true;

        lbl_pop1staffname.Visible = true;
        txt_pop1staffname.Visible = true;
        btn_staffquestion.Visible = true;
        staff.Visible = true;
        lbl_photo.Visible = true;
        ImageButton3.Visible = true;

        lbl_staffcode.Visible = false;
        txt_staffcode.Visible = false;
        btn_staffcode.Visible = false;
        code.Visible = false;
        lbl_staffname.Visible = false;
        txt_staffname.Visible = false;

        lbl_deptstaff.Visible = false;
        txt_deptstaff.Visible = false;
        lbl_des.Visible = false;
        txt_des.Visible = false;


        lbl_stafftypr.Visible = false;
        txt_stafftype.Visible = false;
        lbl_staffphoto.Visible = false;
        ImageButton4.Visible = false;



        txt_status.Visible = true;//delsi0803
        lbl_statuspay.Visible = true;
        lbl_narr.Visible = true;
        txt_narr.Visible = true;
        lbl_payamt.Visible = true;
        txt_payamt.Visible = true;
        sppay.Visible = true;
    }

    protected void btnsturollno_Click(object sender, EventArgs e)
    {
        try
        {

            popwindowstudent.Visible = true;
            fpsturoll.Visible = false;
            btn_pop2ok.Visible = false;
            btn_pop2exit.Visible = false;
            bindpop2collegename();
            bindpop2degree();
            loadbranch();
            bindpop2batchyear();
            ddl_pop2sex.SelectedIndex = 0;
            int activerow = 0;
            activerow = Convert.ToInt32(fpsturoll.ActiveSheetView.ActiveRow.ToString());
            for (int i = 0; i < fpsturoll.Sheets[0].RowCount; i++)
            {
                if (i == Convert.ToInt32(activerow))
                {
                    fpsturoll.Sheets[0].Rows[i].BackColor = Color.LightBlue;

                }
                else
                {
                    fpsturoll.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                }
            }

            popwindowstudent.Visible = true;
            btn_pop2ok.Visible = false;
            btn_pop2exit.Visible = false;

            fpsturoll.CommandBar.Visible = false;
            fpsturoll.SheetCorner.ColumnCount = 0;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            fpsturoll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            fpsturoll.Sheets[0].ColumnCount = 4;
            fpsturoll.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
            fpsturoll.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
            fpsturoll.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
            fpsturoll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            fpsturoll.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
            fpsturoll.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
            fpsturoll.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
            fpsturoll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            fpsturoll.Sheets[0].ColumnHeader.Columns[2].Label = "Admission No";
            fpsturoll.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";
            fpsturoll.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
            fpsturoll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            fpsturoll.Sheets[0].Columns[2].Visible = false;
            fpsturoll.Sheets[0].ColumnHeader.Columns[3].Label = "Name";
            fpsturoll.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";
            fpsturoll.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
            fpsturoll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            fpsturoll.Sheets[0].Columns[0].Width = 50;
            fpsturoll.Sheets[0].Columns[1].Width = 120;
            fpsturoll.Sheets[0].Columns[2].Width = 100;
            fpsturoll.Sheets[0].Columns[3].Width = 240;
            fpsturoll.Sheets[0].Columns[4].Width = 280;
            fpsturoll.Width = 426;
            fpsturoll.Columns[0].Locked = true;
            fpsturoll.Columns[1].Locked = true;
            fpsturoll.Columns[2].Locked = true;
            fpsturoll.Columns[3].Locked = true;
            fpsturoll.Columns[4].Locked = true;

            //else
            //{
            //    lblpop2error.Visible = true;
            //    lblpop2error.Text = "Please Select Hostel";
            //}
        }
        catch (Exception ex)
        {


        }
    }
    protected void bindpop2collegename()
    {
        try
        {
            string clgname = "select college_code,collname from collinfo ";
            if (clgname != "")
            {
                ds = d2.select_method(clgname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {

                    ddl_pop2collgname.DataSource = ds;
                    ddl_pop2collgname.DataTextField = "collname";
                    ddl_pop2collgname.DataValueField = "college_code";
                    ddl_pop2collgname.DataBind();

                }
            }

            bindpop2hostel();
            bindpop2degree();
        }
        catch (Exception ex)
        {
        }

    }

    protected void bindpop2degree()
    {
        try
        {
            ds.Clear();
            string query = "";
            if (usercode != "")
            {
                query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + "";
            }
            else
            {
                query = "select distinct degree.course_id,course.course_name  from degree,course,deptprivilages where  course.course_id=degree.course_id and course.college_code = degree.college_code   and degree.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedItem.Value) + "' and deptprivilages.Degree_code=degree.Degree_code  and group_code=" + group_user + "";
            }
            ds = d2.select_method_wo_parameter(query, "Text");
            ddl_pop2degre.DataSource = ds;
            ddl_pop2degre.DataTextField = "course_name";
            ddl_pop2degre.DataValueField = "course_id";
            ddl_pop2degre.DataBind();
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "breakage_entry"); 

        }

    }

    protected void bindpop2hostel()
    {
        try
        {
            ds.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop1hostelname.DataSource = ds;
                ddl_pop1hostelname.DataTextField = "HostelName";
                ddl_pop1hostelname.DataValueField = "HostelMasterPK";
                ddl_pop1hostelname.DataBind();
            }
            else
            {
                ddl_pop1hostelname.Items.Clear();
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void bindpop2batchyear()
    {
        try
        {

            hat.Clear();
            string sqlyear = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and cc=0 and delflag=0 and exam_flag<>'debar' order by batch_year desc";
            ds = d2.select_method(sqlyear, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_pop2batchyear.DataSource = ds;
                ddl_pop2batchyear.DataTextField = "batch_year";
                ddl_pop2batchyear.DataValueField = "batch_year";
                ddl_pop2batchyear.DataBind();
            }
        }
        catch (Exception ex)
        {
        }

    }
    public void loadbranch()
    {
        try
        {
            string query1 = "";
            string buildvalue1 = "";
            string build1 = "";
            ddl_pop2branch.Items.Clear();
            if (ddl_pop2degre.Items.Count > 0)
            {
                for (int i = 0; i < ddl_pop2degre.Items.Count; i++)
                {
                    build1 = ddl_pop2degre.SelectedValue;
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }
                }
                query1 = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + buildvalue1 + "') and degree.college_code='" + ddl_pop2collgname.SelectedValue + "' and deptprivilages.Degree_code=degree.Degree_code";
                ds = d2.select_method(query1, hat, "Text");
                ddl_pop2branch.DataSource = ds;
                ddl_pop2branch.DataTextField = "dept_name";
                ddl_pop2branch.DataValueField = "degree_code";
                ddl_pop2branch.DataBind();
                //  ddl_pop2branch.Items.Insert(0, "All");
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void ddl_pop2collgname_selectedindexchange(object sender, EventArgs e)
    {
        try
        {

            bindpop2batchyear();
            bindpop2degree();
            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";

        }
        catch (Exception ex)
        {

        }

    }

    protected void ddl_pop2batchyear_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            bindpop2degree();
            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";
        }
        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode, "GymAllotment");
        }

    }
    protected void ddl_pop2degre_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            loadbranch();
            fpsturoll.Visible = false;
            lblpop2error.Visible = false;
            lblpop2error.Text = "";
        }
        catch (Exception ex)
        {// d2.sendErrorMail(ex, collegecode, "GymAllotment");
        }

    }

    protected void btn_pop2ok_Click(object sender, EventArgs e)//delsiref
    {
        try
        {
            popwindowstudent.Visible = false;
            string activerow = "";
            string activecol = "";
            string appno = " ";
            activerow = fpsturoll.ActiveSheetView.ActiveRow.ToString();
            activecol = fpsturoll.ActiveSheetView.ActiveColumn.ToString();
            string purpose = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string retroll = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + purpose + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            string branch = Convert.ToString(fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            string[] split1 = branch.Split('-');
            string degree = Convert.ToString(split1[0]);
            string department = Convert.ToString(split1[1]);
            string name = fpsturoll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            txt_rollno.Text = purpose;
            txt_deg.Text = degree;
            txt_name.Text = name;
            txt_deptstu.Text = department;

        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "GymAllotment"); 
        }
    }

    protected void btn_pop2exit_Click(object sender, EventArgs e)
    {
        popwindowstudent.Visible = false;
    }
    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindowstudent.Visible = false;
    }

    protected void btn_pop2go_Click(object sender, EventArgs e)
    {
        try
        {
            string hostudcollcode = string.Empty;
            string hostuddegree = string.Empty;
            string hostudbranch = string.Empty;
            string hostudbatch = string.Empty;

            string hostelgender = d2.GetFunction("select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            hostelgender = " and a.sex in(" + hostelgender + ")";

            if (ddl_pop2collgname.Items.Count > 0)
                hostudcollcode = Convert.ToString(ddl_pop2collgname.SelectedValue);
            if (ddl_pop2degre.Items.Count > 0)
                hostuddegree = Convert.ToString(ddl_pop2degre.SelectedValue);
            if (ddl_pop2batchyear.Items.Count > 0)
                hostudbatch = Convert.ToString(ddl_pop2batchyear.SelectedValue);
            if (ddl_pop2branch.Items.Count > 0)
                hostudbranch = Convert.ToString(ddl_pop2branch.SelectedValue);
            if (!string.IsNullOrEmpty(hostudcollcode) && !string.IsNullOrEmpty(hostuddegree) && !string.IsNullOrEmpty(hostudbatch) && !string.IsNullOrEmpty(hostudbranch))
            {
                if (ddl_pop2sex.SelectedItem.Text == "All")
                {
                    sqladd = "select distinct r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1' and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and  G.Course_ID ='" + hostuddegree + "' " + hostelgender + " ";
                    loadStudentdetails();
                }
                else if (ddl_pop2sex.SelectedItem.Text == "Male")
                {
                    sqladd = "select r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='0' and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + " ";
                    loadStudentdetails();
                }
                else if (ddl_pop2sex.SelectedItem.Text == "Female")
                {
                    sqladd = "select r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='1' and g.college_code = d.college_code and r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + " ";
                    loadStudentdetails();
                }
                else
                {
                    sqladd = "select r.App_No,roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d,HT_HostelRegistration hr where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='2' and g.college_code = d.college_codeand r.App_No = hr.APP_No and a.app_no=hr.APP_No and MemType='1'  and r.Batch_Year ='" + hostudbatch + "' and  g.Degree_Code='" + hostudbranch + "' and   G.Course_ID ='" + hostuddegree + "' " + hostelgender + "  ";
                    loadStudentdetails();
                }
            }
        }
        catch (Exception ex)
        {// d2.sendErrorMail(ex, collegecode, "GymAllotment"); 
        }

    }
    public void loadStudentdetails()
    {
        try
        {
            if (ddl_pop2branch.Items.Count > 0)
            {
                string buildvalue1 = "";
                string build1 = "";
                build1 = ddl_pop2branch.SelectedValue;
                if (buildvalue1 == "")
                {
                    buildvalue1 = build1;
                }
                else
                {
                    buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                }
                if (buildvalue1 != "" && buildvalue1 != "All")
                {
                    sqladd = sqladd + " AND g.degree_code in ('" + buildvalue1 + "')";
                }
                else
                {
                    sqladd = sqladd + "";
                }
            }

            //  if (Rollflag1 == "1")
            // {
            fpsturoll.Columns[1].Visible = true;
            fpsturoll.Width = 426;
            //  }
            //else
            //{
            //    fpsturoll.Columns[1].Visible = false;
            //    fpsturoll.Width = 326;
            //}
            string strorderby = d2.GetFunction("select value from Master_Settings where settings='order_by'");
            if (strorderby == "")
            {
                strorderby = "";
            }
            else
            {
                if (strorderby == "0")
                {
                    strorderby = "ORDER BY r.Roll_No";
                }
                else if (strorderby == "2")
                {
                    strorderby = "ORDER BY r.Stud_Name";
                }
                else if (strorderby == "0,2")
                {
                    strorderby = "ORDER BY r.Roll_No,r.Stud_Name";
                }
                else
                {
                    strorderby = "";
                }
            }
            fpsturoll.Sheets[0].RowCount = 0;
            fpsturoll.Sheets[0].RowHeader.Visible = false;
            fpsturoll.SaveChanges();
            fpsturoll.Sheets[0].AutoPostBack = false;
            ds.Clear();
            string q = sqladd + strorderby;
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count <= 0)
            {

                fpsturoll.Visible = false;
                lblpop2error.Visible = true;
                lblpop2error.Text = "No Students Found Or Roll numbers might not be generated";
                btn_pop2ok.Visible = false;
                btn_pop2exit.Visible = false;
            }
            else
            {
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                fpsturoll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    fpsturoll.Columns[0].Locked = true;
                    fpsturoll.Columns[1].Locked = true;
                    fpsturoll.Columns[2].Locked = true;
                    fpsturoll.Columns[3].Locked = true;
                    int sno = 0;
                    lblpop2error.Visible = false;
                    fpsturoll.Visible = true;
                    fpsturoll.CommandBar.Visible = false;
                    btn_pop2ok.Visible = true;
                    btn_pop2exit.Visible = true;
                    sno = 0;
                    int studcount = 0;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    for (int row1 = 0; row1 < ddl_pop2branch.Items.Count; row1++)
                    {
                        if (ddl_pop2branch.Items[row1].Selected)
                        {
                            ds.Tables[0].DefaultView.RowFilter = "Degree_Code='" + Convert.ToSingle(ddl_pop2branch.Items[row1].Value) + "'";
                            DataView dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                fpsturoll.Sheets[0].RowCount = fpsturoll.Sheets[0].RowCount + 1;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["branch"]);
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fpsturoll.Sheets[0].AddSpanCell(fpsturoll.Sheets[0].RowCount - 1, 0, 1, 4);
                                sno++;
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    studcount++;
                                    fpsturoll.Sheets[0].RowCount++;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["App_No"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["roll_no"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[0]["branch"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["stud_name"]);
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    fpsturoll.Sheets[0].Cells[fpsturoll.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                    }
                    int rowcount = fpsturoll.Sheets[0].RowCount;
                    fpsturoll.Height = 270;
                    fpsturoll.Sheets[0].PageSize = 15 + (rowcount * 5);
                    fpsturoll.SaveChanges();
                    lblpop2error.Visible = false;
                }
                else
                {
                    lblpop2error.Visible = true;
                    lblpop2error.Text = "No Record Found";

                }
            }
        }
        catch (Exception ex)
        {
            d2.sendErrorMail(ex, collegecode, "breakage_entry");

        }

    }
    public void btn_guestLookup_click(object sender, EventArgs e)
    {
        popwindowguest.Visible = true;

        FpSpread2.Visible = false;
        div3.Visible = false;
        btn_guestSave.Visible = false;
        btn_guestClose.Visible = false;
        lbl_guesterror.Text = "";
        txt_guestcode.Text = "";
        txt_guestName.Text = "";
        bindhostelhostel();


    }

    protected void imagebtnpop_Click(object sender, EventArgs e)
    {
        popwindowguest.Visible = false;
    }
    protected void cb_hostelname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_hostelname.Checked == true)
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                }
                txt_hostelname.Text = "Hostel Name(" + (cbl_hostelname.Items.Count) + ")";

                cbl_hostelname_SelectedIndexChanged(sender, e);
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                }
                txt_hostelname.Text = "--Select--";
                cbl_buildingname.Items.Clear();
                txt_buildingname.Text = "--Select--";
                cb_buildingname.Checked = false;

            }

        }
        catch (Exception ex)
        {
        }
    }
    protected void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_hostelname.Checked = false;

            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_hostelname.Text = "--Select--";
                    cb_hostelname.Checked = false;
                    build = cbl_hostelname.Items[i].Value.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;
                    }
                }
            }
            clgbuild(buildvalue);

            if (seatcount == cbl_hostelname.Items.Count)
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
                cb_hostelname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_hostelname.Text = "--Select--";
            }
            else
            {
                txt_hostelname.Text = "Hostel Name(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }

    }


    public void bindhostelhostel()
    {
        try
        {
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            if (ds.Tables[0].Rows.Count > 0)
            {

                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();

                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = true;
                    txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                    cb_hostelname.Checked = true;
                }

                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }

                clgbuild(lochosname);

            }
            else
            {
                cbl_hostelname.Items.Insert(0, "--Select--");
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_guestGo_Click(object sender, EventArgs e)//delsi0309
    {
        try
        {
            int rowcount;
            string hoscode = "";
            string floorname = "";
            int rolcount = 0;
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hoscode1 = cbl_hostelname.Items[i].Value.ToString();
                    if (hoscode == "")
                    {
                        hoscode = hoscode1;
                    }
                    else
                    {
                        hoscode = hoscode + "'" + "," + "'" + hoscode1;
                    }
                }
            }
            string date1 = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    string floorname1 = cbl_floorname.Items[i].Value.ToString();
                    if (floorname == "")
                    {
                        floorname = floorname1;
                    }
                    else
                    {
                        floorname = floorname + "'" + "," + "'" + floorname1;
                    }
                }
            }
            string buildingname = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string buildingname1 = cbl_buildingname.Items[i].Value.ToString();
                    if (buildingname == "")
                    {
                        buildingname = buildingname1;
                    }
                    else
                    {
                        buildingname = buildingname + "'" + "," + "'" + buildingname1;
                    }
                }
            }
            string roomname = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string roomname1 = cbl_roomname.Items[i].Value.ToString();
                    if (roomname == "")
                    {
                        roomname = roomname1;
                    }
                    else
                    {
                        roomname = roomname + "'" + "," + "'" + roomname1;
                    }
                }
            }

            string q = "select HM.HostelName as Hostel_Name,Vi.VenContactName as Guest_Name,Vi.VendorContactPK as GuestCode,V.VendorAddress as Guest_Address,Vi.VendorMobileNo as MobileNo,V.VendorCompName as From_Company,f.Floor_Name as Floor_Name,r.Room_Name as Room_Name,HM.HostelMasterPK as Hostel_Code,B.Building_Name,B.Code,V.VendorStreet as Guest_Street,V.VendorCity as Guest_City,V.VendorPin as Guest_PinCode from HT_HostelRegistration H,CO_VendorMaster V,IM_VendorContactMaster Vi,Building_Master B,Floor_Master f,Room_Detail r,HM_HostelMaster HM where hm.HostelMasterPK =h.HostelMasterFK and v.VendorPK=vi.VendorFK and b.Code =h.BuildingFK and f.FloorPK=H.FloorFK and r.RoomPk=H.RoomFK and B.Code in('" + buildingname + "') and H.FloorFK in('" + floorname + "') and H.RoomFK in('" + roomname + "') and HM.HostelMasterPK in('" + hoscode + "') and H.GuestVendorFK=v.VendorPK and vi.VendorContactPK=h.APP_No";

            ds.Clear();

            ds = d2.select_method_wo_parameter(q, "Text");
            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.SaveChanges();
            FpSpread2.SheetCorner.ColumnCount = 0;
            FpSpread2.CommandBar.Visible = false;

            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FarPoint.Web.Spread.CheckBoxCellType chkcell1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
            FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
            // FpSpread2.Sheets[0].SpanModel.Add(Fpstaff.Sheets[0].RowCount - 1, 0, 1, 3);
            FpSpread2.Sheets[0].AutoPostBack = true;

            FpSpread2.Sheets[0].RowCount = 0;
            FpSpread2.Sheets[0].ColumnCount = 3;
            FpSpread2.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;
            FpSpread2.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.Always;

            if (ds.Tables[0].Rows.Count > 0)
            {

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[0].Locked = true;
                FpSpread2.Columns[0].Width = 80;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Guest Code";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[1].Locked = true;
                FpSpread2.Columns[1].Width = 100;

                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Guest Name";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread2.Sheets[0].Columns[2].Locked = true;
                FpSpread2.Columns[2].Width = 200;

                int sno = 0;
                for (rolcount = 0; rolcount < ds.Tables[0].Rows.Count; rolcount++)
                {
                    sno++;


                    FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Text = sno.ToString();
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["GuestCode"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[rolcount]["Guest_Name"]);
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";


                }

                rowcount = FpSpread2.Sheets[0].RowCount;
                FpSpread2.Height = 370;
                btn_guestSave.Visible = true;
                btn_guestClose.Visible = true;

                FpSpread2.Visible = true;
                div3.Visible = true;
                FpSpread2.Sheets[0].PageSize = 25 + (rowcount * 20);
                FpSpread2.SaveChanges();

            }
            else
            {
                //Fpstaff.Visible = false;
                //btn_staffsave.Visible = false;
                //btn_staffexit.Visible = false;
                //div1.Visible = false;
                //err.Visible = true;
                //err.Text = "No Records Found";
            }


        }
        catch (Exception ex)
        {


        }
    }

    protected void Cell_Click1(object sender, EventArgs e)
    {
        try
        {
            guestcheck = true;
        }
        catch
        {
        }
    }
    protected void Fpspread2_render(object sender, EventArgs e)
    {
        if (guestcheck == true)
        {
            string activerow = "";
            string activecol = "";

            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();

            if (activerow.Trim() != "")
            {
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].BackColor = Color.DarkCyan;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].BackColor = Color.DarkCyan;
                // FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 3].BackColor = Color.DarkCyan;
                // FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 4].BackColor = Color.DarkCyan;
                FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].BackColor = Color.DarkCyan;
            }
        }

    }

    public void btn_GuestSave_Click(object sender, EventArgs e)
    {

        try
        {

            string guestcode = string.Empty;
            string guestName = string.Empty;

            string activerow = "";
            string activecol = "";

            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            guestcode = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            guestName = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            txt_guestcode.Text = guestcode;
            txt_guestName.Text = guestName;

            popwindowguest.Visible = false;




        }
        catch (Exception ex)
        {
        }


    }
    public void btn_GuestExit_Click(object sender, EventArgs e)
    {
        popwindowguest.Visible = false;
    }
    protected void cbbuildname_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildingname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string lochosname = "";
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    if (cbl_hostelname.Items[i].Selected == true)
                    {
                        string hosname = cbl_hostelname.Items[i].Value.ToString();
                        if (lochosname == "")
                        {
                            lochosname = hosname;
                        }
                        else
                        {
                            lochosname = lochosname + "'" + "," + "'" + hosname;
                        }
                    }
                }
                cbl_buildingname.Items.Clear();
                clgbuild(lochosname);

                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    if (cb_buildingname.Checked == true)
                    {
                        cbl_buildingname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;

                        }

                    }
                }
                clgfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                {
                    cbl_buildingname.Items[i].Selected = false;
                    txt_buildingname.Text = "--Select--";
                    cbl_floorname.Items.Clear();
                    cb_floorname.Checked = false;
                    txt_floorname.Text = "--Select--";
                    txt_roomname.Text = "--Select--";
                    cb_roomname.Checked = false;
                    cbl_roomname.Items.Clear();
                }
            }


        }
        catch (Exception ex)
        {
        }
    }

    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildingname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildingname.DataSource = ds;
                cbl_buildingname.DataTextField = "Building_Name";
                cbl_buildingname.DataValueField = "code";
                cbl_buildingname.DataBind();
            }

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                cbl_buildingname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildingname.Items.Count) + ")";
                cb_buildingname.Checked = true;
            }

            string locbuild = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    string builname = cbl_buildingname.Items[i].Text;
                    if (locbuild == "")
                    {
                        locbuild = builname;
                    }
                    else
                    {
                        locbuild = locbuild + "'" + "," + "'" + builname;
                    }
                }
            }
            clgfloor(locbuild);
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblbuildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildingname.Checked = false;

            string buildvalue = "";
            string build = "";
            string lochosname = "";
            for (int i = 0; i < cbl_hostelname.Items.Count; i++)
            {
                if (cbl_hostelname.Items[i].Selected == true)
                {
                    string hosname = cbl_hostelname.Items[i].Value.ToString();
                    if (lochosname == "")
                    {
                        lochosname = hosname;
                    }
                    else
                    {
                        lochosname = lochosname + "'" + "," + "'" + hosname;
                    }
                }
            }
            //cbl_buildingname.Items.Clear();
            //clgbuild(lochosname);

            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    cb_floorname.Checked = true;
                    build = cbl_buildingname.Items[i].Text.ToString();
                    if (buildvalue == "")
                    {
                        buildvalue = build;
                    }
                    else
                    {
                        buildvalue = buildvalue + "'" + "," + "'" + build;

                    }

                }
            }

            clgfloor(buildvalue);

            if (seatcount == cbl_buildingname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildingname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }

    public void clgfloor(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floorname.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floorname.DataSource = ds;
                cbl_floorname.DataTextField = "Floor_Name";
                cbl_floorname.DataValueField = "FloorPK";
                cbl_floorname.DataBind();

            }
            else
            {
                txt_floorname.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                cbl_floorname.Items[i].Selected = true;
                cb_floorname.Checked = true;
            }

            string locfloor = "";
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                    string flrname = cbl_floorname.Items[i].Text;
                    if (locfloor == "")
                    {
                        locfloor = flrname;
                    }
                    else
                    {
                        locfloor = locfloor + "'" + "," + "'" + flrname;
                    }
                }

            }
            clgroom(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbfloorname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";

                if (cb_buildingname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildingname.Items.Count; i++)
                    {
                        build1 = cbl_buildingname.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                }
                if (cb_floorname.Checked == true)
                {
                    for (int j = 0; j < cbl_floorname.Items.Count; j++)
                    {
                        cbl_floorname.Items[j].Selected = true;
                        txt_floorname.Text = "Floor(" + (cbl_floorname.Items.Count) + ")";
                        build2 = cbl_floorname.Items[j].Text.ToString();
                        if (buildvalue2 == "")
                        {
                            buildvalue2 = build2;
                        }
                        else
                        {
                            buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                        }
                    }
                }
                clgroom(buildvalue2, buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_floorname.Items.Count; i++)
                {
                    cbl_floorname.Items[i].Selected = false;
                    txt_floorname.Text = "--Select--";
                }
                cb_roomname.Checked = false;
                cbl_roomname.Items.Clear();
                txt_roomname.Text = "--Select--";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblfloorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildingname.Items.Count; i++)
            {
                if (cbl_buildingname.Items[i].Selected == true)
                {
                    build1 = cbl_buildingname.Items[i].Text.ToString();
                    if (buildvalue1 == "")
                    {
                        buildvalue1 = build1;
                    }
                    else
                    {
                        buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                    }

                }
            }
            for (int i = 0; i < cbl_floorname.Items.Count; i++)
            {
                if (cbl_floorname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    build2 = cbl_floorname.Items[i].Text.ToString();
                    if (buildvalue2 == "")
                    {
                        buildvalue2 = build2;
                    }
                    else
                    {
                        buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                    }
                }
            }
            clgroom(buildvalue2, buildvalue1);

            if (seatcount == cbl_floorname.Items.Count)
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floorname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorname.Text = "--Select--";
            }
            else
            {
                txt_floorname.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
            //  clgroom(buildvalue1, buildvalue2);
        }
        catch (Exception ex)
        {
        }
    }

    public void clgroom(string floorname, string buildname)
    {
        try
        {
            cbl_roomname.Items.Clear();
            ds = d2.BindRoom(floorname, buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_roomname.DataSource = ds;
                cbl_roomname.DataTextField = "Room_Name";
                cbl_roomname.DataValueField = "Roompk";
                cbl_roomname.DataBind();
            }
            else
            {
                txt_roomname.Text = "--Select--";
            }

            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                cbl_roomname.Items[i].Selected = true;
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
                cb_roomname.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cbroomname_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomname.Checked == true)
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = true;
                }
                txt_roomname.Text = "Room(" + (cbl_roomname.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomname.Items.Count; i++)
                {
                    cbl_roomname.Items[i].Selected = false;
                }
                txt_roomname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cblroomname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomname.Checked = false;
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }

            }
            if (seatcount == cbl_roomname.Items.Count)
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_roomname.Text = "--Select--";
            }
            else
            {
                txt_roomname.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
}