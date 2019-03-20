using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Drawing;
using System.Configuration;

public partial class HRMOD_ITGroupMapping : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string collegecode = string.Empty;
    static string collegestat = string.Empty;

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
            collegecode = Session["collegecode"].ToString();
            lblerr.Visible = false;
            if (!IsPostBack)
            {
                setLabelText();
                bindcollege();
                bttngo_Click(sender, e);
            }
            if (ddlcolload.Items.Count > 0)
            {
                collegestat = ddlcolload.SelectedItem.Value.ToString();
            }
        }
        catch (Exception ex) { }
    }

    #region HierachyTree
    public class HierarchyTree : List<HierarchyTree.HGroup>
    {
        public class HGroup
        {
            private int m_group_code;
            private int m_parent_code;
            private string m_group_name;

            public int group_code
            {
                get { return m_group_code; }
                set { m_group_code = value; }
            }

            public int parent_code
            {
                get { return m_parent_code; }
                set { m_parent_code = value; }
            }
            public string group_name
            {
                get { return m_group_name; }
                set { m_group_name = value; }
            }

        }
    }
    public void RecursiveChild(TreeNode tn, string searchValue, HierarchyTree.HGroup hTree)
    {
        try
        {
            if (tn.Value == searchValue)
            {
                tn.ChildNodes.Add(new TreeNode(hTree.group_name.ToString(), hTree.group_code.ToString()));
            }
            if (tn.ChildNodes.Count > 0)
            {
                foreach (TreeNode ctn in tn.ChildNodes)
                {
                    RecursiveChild(ctn, searchValue, hTree);
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
    #endregion
    #region Webmethod
    [WebMethod]
    public static string checkGroupName(string grpname)
    {
        string returnValue = "1";
        try
        {
            DAccess2 dd = new DAccess2();
            string grp_name = grpname;
            if (grp_name.Trim() != "" && grp_name != null)
            {
                string queryledname = dd.GetFunction("select distinct GroupName,ITGroupPK from IT_GroupMaster where GroupName='" + grp_name + "' and CollegeCode=" + collegestat + "");
                if (queryledname.Trim() == "" || queryledname == null || queryledname == "0" || queryledname == "-1")
                {
                    returnValue = "0";
                }

            }
            else
            {
                returnValue = "2";
            }
        }
        catch (SqlException ex)
        {
            returnValue = "error" + ex.ToString();
        }
        return returnValue;
    }
    #endregion
    #region Button Click Event
    protected void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btnadd_click(object sender, EventArgs e)
    {
        try
        {
            TreeNode tn = TreeView1.SelectedNode;
            TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
            if (tn != null)
            {
                if (txtgroup.Text.Trim() != "")
                {
                    bool check = false;
                    string grpname = txtgroup.Text.ToString();
                    string desc = txtdesc.Text.ToString();
                    //grpname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(grpname);
                    string currcode = ViewState["currcode"].ToString();
                    if (currcode != null)
                    {
                        string inserttb = "insert into IT_GroupMaster (ParentCode,GroupName,GroupDesc,CollegeCode) values ('" + currcode + "','" + grpname + "','" + desc + "','" + ddlcolload.SelectedItem.Value + "')";
                        int count = d2.update_method_wo_parameter(inserttb, "Text");
                        if (count > 0)
                        {
                            check = true;
                        }
                        if (check == true)
                        {
                            bttngo_Click(sender, e);
                            popupwindow.Visible = true;
                            imgdiv2.Visible = true;
                            lbl_alerterr.Text = "Saved Successfully";
                            popupwindow.Visible = false;
                            txtgroup.Text = "";
                            txtdesc.Text = "";
                        }
                    }
                    else
                    {
                        lblerr.Visible = true;
                        lblerr.Text = "Please Select Group Node";
                    }
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Please Enter The Sub Group Name";
                }
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Select The Parent Node";
            }
        }
        catch
        {

        }
    }
    protected void btnAddnew_click(object sender, EventArgs e)
    {
        try
        {
            bool test = false;
            if (txtgroup.Text.Trim() != "")
            {
                string groupname = txtgroup.Text.ToString();
                string description = txtdesc.Text.ToString();
                string MaxAmount = txtMaxLimtAmount.Text.ToString();
                if (MaxAmount.Trim() == "")
                {
                    MaxAmount = "0";
                }
                //groupname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(groupname);
                string insertquery = "Insert into IT_GroupMaster (ParentCode,GroupName,GroupDesc,CollegeCode,MaxLimitAmount) values('0','" + groupname + "','" + description + "','" + ddlcolload.SelectedItem.Value + "','" + MaxAmount + "')";
                int insq = d2.update_method_wo_parameter(insertquery, "Text");
                if (insq > 0)
                {
                    test = true;
                }
                if (test == true)
                {
                    bttngo_Click(sender, e);
                    popupwindow.Visible = true;
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Saved Successfully";
                    popupwindow.Visible = false;
                    txtgroup.Text = "";
                    txtdesc.Text = "";
                }
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter the Group Name";
            }
        }
        catch
        {

        }


    }
    protected void btndelete_Click(object sender, EventArgs e)
    {
        if (txtgroup.Text.Trim() != "")
        {
            imgdiv1.Visible = true;
            lblalert.Visible = true;
            lblalert.Text = "Do You Want To Delete This Node?";
        }
        else
        {
            lblerr.Visible = true;
            lblerr.Text = "Please Enter Group To Delete";
            imgdiv1.Visible = false;
            lblalert.Visible = false;
        }
    }
    protected void btnyes_Click(object sender, EventArgs e)
    {
        try
        {
            string currcode = ViewState["currcode"].ToString();
            string parcode = "";
            DataSet dsPcode = new DataSet();
            dsPcode = d2.select_method_wo_parameter("select Parentcode from IT_GroupMaster where CollegeCode=" + ddlcolload.SelectedItem.Value + " and ITGroupPK=" + currcode + "", "Text");
            if (dsPcode.Tables.Count > 0 && dsPcode.Tables[0].Rows.Count > 0)
            {
                parcode = dsPcode.Tables[0].Rows[0][0].ToString();
            }

            string delquery = "";

            delquery = "delete from IT_GroupMaster where ITGroupPK='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "' ";
            if (parcode == "0")
            {
                delquery += " delete from IT_GroupMaster where Parentcode='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
            }
            int clearcount = d2.update_method_wo_parameter(delquery, "Text");
            if (clearcount > 0)
            {

                bttngo_Click(sender, e);
                imgdiv2.Visible = true;
                imgdiv1.Visible = false;
                lblalert.Visible = false;
                lbl_alerterr.Text = "Deleted Successfully";
                popupwindow.Visible = false;
                txtgroup.Text = "";
                txtdesc.Text = "";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Cannot Be Deleted";
            }

        }
        catch
        {

        }
    }
    protected void btnno_Click(object sender, EventArgs e)
    {
        imgdiv1.Visible = false;
        lblalert.Visible = false;
    }
    protected void btnpopexit_Click(object sender, EventArgs e)
    {
        popupwindow.Visible = false;
        lblerr.Visible = false;
    }
    protected void bttngo_Click(object sender, EventArgs e)
    {
        bindtreeview();
        popupwindow.Visible = false;
        if (TreeView1.Nodes.Count > 0)
        {
            panel3.Visible = true;
            CreateGroup.Visible = true;
            CreatePriority.Visible = false;
            CreateMapping.Visible = false;
        }
        else
        {
            panel3.Visible = false;
            CreatePriority.Visible = false;
            CreateMapping.Visible = false;
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "No Records Found";
        }
    }
    protected void btnsave_click(object sender, EventArgs e)
    {

    }
    protected void btnexitmain_Click(object sender, EventArgs e)
    {

    }
    protected void BtnUpdateTree_Click(object sender, EventArgs e)
    {
        try
        {
            if (TreeView1.SelectedNode.Value != "")
            {
                txtgroup.Text = "";
                txtdesc.Text = "";
                TreeNode currnode = TreeView1.SelectedNode;
                if (currnode != null)
                {
                    popupwindow.Visible = true;
                    btnupdate.Visible = true;
                    btndelete.Visible = true;
                    btnadd.Visible = false;
                    btnAddnew.Visible = false;
                    lblerr.Visible = false;
                    lblhead.Visible = true;
                    lblhead.Text = "Modify Group";
                    TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
                    string currnodevalue = currnode.Text;
                    string currnodecode = currnode.Value;
                    ViewState["currcode"] = currnodecode;
                    string desc = "";
                    string Amount = string.Empty;

                    string selquery = "select GroupDesc,MaxLimitAmount from IT_GroupMaster where ITGroupPK='" + currnodecode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        desc = ds.Tables[0].Rows[0]["GroupDesc"].ToString();
                        Amount = ds.Tables[0].Rows[0]["MaxLimitAmount"].ToString();
                    }
                    txtgroup.Text = currnodevalue;
                    txtdesc.Text = desc;
                    txtMaxLimtAmount.Text = Amount;
                }
                else
                {
                    lblerr.Visible = true;
                    lblerr.Text = "Please select any Group Name";
                    popupwindow.Visible = false;
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Select A Node";
                popupwindow.Visible = false;
            }
        }
        catch
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Please Select A Node";
            popupwindow.Visible = false;
        }
    }
    protected void BtnAddChild_Click(object sender, EventArgs e)
    {
        try
        {
            if (TreeView1.SelectedNode.Value != "")
            {
                TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
                popupwindow.Visible = true;
                btnAddnew.Visible = true;
                btnadd.Visible = false;
                btnupdate.Visible = false;
                btndelete.Visible = false;
                btnpopexit.Visible = true;
                lblerr.Visible = false;
                lblhead.Visible = true;
                lblhead.Text = "Add New SubGroup";
                txtgroup.Text = "";
                txtdesc.Text = "";
                txtMaxLimtAmount.Text = "";
                TreeNode currnode = TreeView1.SelectedNode;
                TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
                string currnodevalue = currnode.Text;
                string currnodecode = currnode.Value;
                ViewState["currcode"] = currnodecode;
                string desc = "";

                //string selquery = "select GroupDesc,MaxLimitAmount from IT_GroupMaster where ITGroupPK='" + currnodecode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                //ds.Clear();
                //ds = d2.select_method_wo_parameter(selquery, "Text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    desc = ds.Tables[0].Rows[0]["GroupDesc"].ToString();
                //}
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Select A Node";
                popupwindow.Visible = false;
            }
        }
        catch
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Please Select A Node";
            popupwindow.Visible = false;
        }
    }
    protected void btngo_click(object sender, EventArgs e)
    {

    }
    protected void btnNew_Click(object sender, EventArgs e)
    {

    }
    protected void btnupdate_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtgroup.Text.Trim() != "")
            {
                string groupname = txtgroup.Text.ToString();
                //groupname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(groupname);
                string desc = txtdesc.Text.ToString();
                string MaxAmount = txtMaxLimtAmount.Text.ToString();
                string currcode = ViewState["currcode"].ToString();

                string updatequery = "update IT_GroupMaster set GroupName='" + groupname + "',GroupDesc='" + desc + "',MaxLimitAmount='" + MaxAmount + "' where ITGroupPK='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                int insq = d2.update_method_wo_parameter(updatequery, "Text");
                bttngo_Click(sender, e);
                popupwindow.Visible = false;
                imgdiv2.Visible = true;
                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "Updated Successfully";
            }
            else
            {
                lblerr.Visible = true;
                lblerr.Text = "Please Enter Name To Update";
            }
        }
        catch
        {

        }
    }
    public void Prioriry()
    {
        try
        {
            CreateGroup.Visible = false;
            CreatePriority.Visible = true;
            CreateMapping.Visible = false;
            string Query = "select GroupName,ITGroupPK,Priority from IT_GroupMaster where parentCode=0 and collegeCode ='" + ddlcolload.SelectedValue + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].RowCount = 0;
                FpSpread1.Sheets[0].ColumnCount = 0;
                FpSpread1.CommandBar.Visible = false;
                FpSpread1.Sheets[0].AutoPostBack = false;
                FpSpread1.Sheets[0].ColumnHeader.RowCount = 1;
                FpSpread1.Sheets[0].RowHeader.Visible = false;
                FpSpread1.Sheets[0].ColumnCount = 4;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                FpSpread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[0].Locked = true;
                FpSpread1.Columns[0].Width = 50;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Group Name";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[1].Width = 200;

                FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                chkall.AutoPostBack = true;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Set Priority";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[2].Width = 75;

                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Priority";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                FpSpread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                FpSpread1.Columns[3].Locked = true;
                FpSpread1.Columns[3].Width = 75;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    FpSpread1.Sheets[0].RowCount++;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(i + 1);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[i]["GroupName"]);
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(ds.Tables[0].Rows[i]["ITGroupPK"]);

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = chkall;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Value = 0;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    string Prioriry = Convert.ToString(ds.Tables[0].Rows[i]["Priority"]);
                    if (Prioriry.Trim() != "" && Prioriry.Trim() != "0")
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Prioriry.ToString();
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Value = 1;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                    }
                    else
                    {
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = "";
                    }
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                }
                FpSpread1.Visible = true;
                FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                FpSpread1.SaveChanges();
                FpSpread1.Height = 400;
                FpSpread1.Width = 600;
                lblalert.Visible = false;

            }
            else
            {
                FpSpread1.Visible = false;
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Not Updated";
            }

        }
        catch
        {
        }
    }
    protected void FpSpread1_ButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string activrow = "";
            activrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string activecol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            int actcol = Convert.ToInt16(activecol);
            int hy_order = 0;
            for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, actcol].Value);
                if (isval == 1)
                {
                    hy_order++;
                    FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol].Locked = true;
                }
            }
            FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].Text = hy_order.ToString();
            FpSpread1.Sheets[0].Cells[Convert.ToInt32(activrow), actcol + 1].HorizontalAlign = HorizontalAlign.Center;
        }
        catch
        {

        }
    }
    protected void btnPrioritySave_Click(object sender, EventArgs e)
    {
        try
        {
            bool UpdateFlag = false;
            FpSpread1.SaveChanges();
            for (int i = 0; i <= Convert.ToInt16(FpSpread1.Sheets[0].RowCount) - 1; i++)
            {
                int isval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[i, 2].Value);
                if (isval == 1)
                {
                    string GetPriority = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 3].Text);
                    string GetID = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 1].Tag);
                    string UpdQuery = "update IT_GroupMaster set Priority='" + GetPriority + "' where ITGroupPK='" + GetID + "' and collegeCode='" + ddlcolload.SelectedValue + "'";
                    int Upd = d2.update_method_wo_parameter(UpdQuery, "Text");
                    if (Upd > 0)
                    {
                        UpdateFlag = true;
                    }
                }
            }
            if (UpdateFlag)
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Update Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Not Updated";

            }
        }
        catch
        {

        }
    }
    protected void btnPriorityReset_Click(object sender, EventArgs e)
    {
        try
        {
            bool UpdateFlag = false;
            string UpdQuery = "update IT_GroupMaster set Priority=null where parentCode=0 and collegeCode='" + ddlcolload.SelectedValue + "'";
            int Upd = d2.update_method_wo_parameter(UpdQuery, "Text");
            if (Upd > 0)
            {
                UpdateFlag = true;
            }
            if (UpdateFlag)
            {
                imgdiv2.Visible = true;
                Prioriry();
                lbl_alerterr.Text = "Reset Sucessfully";

            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Not Rested";

            }
        }
        catch
        {

        }
    }
    protected void btnMappingChild_Click(object sender, EventArgs e)
    {
        try
        {
            bool InsertFlag = false;
            string ITGroupType = string.Empty;
            string NodCode = lblNode.Text;
            if (ddlHeadtype.SelectedValue == "1")
            {
                ITGroupType = "1";
            }
            else if (ddlHeadtype.SelectedValue == "2")
            {
                ITGroupType = "2";
            }
            if (NodCode.Trim() != "" && ITGroupType.Trim() != "" && txtIncomeHead.Text != "Select")
            {
                if (cblincome.Items.Count > 0)
                {
                    string Query = "if exists (select * from  IT_GroupMapping where ITGroupFK='" + NodCode.Trim() + "') delete  IT_GroupMapping where ITGroupFK='" + NodCode.Trim() + "'";
                    int del = d2.update_method_wo_parameter(Query, "Text");
                    for (int intCbl = 0; intCbl < cblincome.Items.Count; intCbl++)
                    {
                        if (cblincome.Items[intCbl].Selected)
                        {
                            string UpdQuery = " update IT_GroupMaster set ITGroupType='" + ITGroupType + "' where ITGroupPK ='" + NodCode + "'";
                            UpdQuery += " insert into IT_GroupMapping(ITGroupFK,IT_IDFK) values ('" + NodCode.Trim() + "','" + cblincome.Items[intCbl].Value + "')";
                            int Upd = d2.update_method_wo_parameter(UpdQuery, "Text");
                            if (Upd > 0)
                            {
                                InsertFlag = true;
                            }
                        }
                    }
                    if (InsertFlag)
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Saved Sucessfully";
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        lbl_alerterr.Text = "Not Saved";
                    }
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Select Any One Mapped Values";
            }
        }
        catch
        {

        }
    }

    #endregion
    #region BindMethod
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            ddlcolload.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcolload.DataSource = ds;
                ddlcolload.DataTextField = "collname";
                ddlcolload.DataValueField = "college_code";
                ddlcolload.DataBind();
            }
        }
        catch
        {

        }
    }
    public void BindIncomeHeade()
    {
        try
        {
            string Query = "select IT_ID,ITAllowDeductName from IT_OtherAllowanceDeducation where ITType='1' and collegeCode='" + ddlcolload.SelectedValue + "' and isnull(IsIncomeTax,'0')='0'";
            ds.Clear();
            cblincome.Items.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblincome.DataSource = ds;
                cblincome.DataTextField = "ITAllowDeductName";
                cblincome.DataValueField = "IT_ID";
                cblincome.DataBind();
                txtIncomeHead.Text = "Select";
                cbIncome.Checked = false;
            }
        }
        catch
        {

        }
    }
    public void BindDeductionHead()
    {
        try
        {
            string Query = "select IT_ID,ITAllowDeductName from IT_OtherAllowanceDeducation where ITType='2' and collegeCode='" + ddlcolload.SelectedValue + "' and isnull(IsIncomeTax,'0')='0'";
            ds.Clear();
            cblincome.Items.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblincome.DataSource = ds;
                cblincome.DataTextField = "ITAllowDeductName";
                cblincome.DataValueField = "IT_ID";
                cblincome.DataBind();
                txtIncomeHead.Text = "Select";
                cbIncome.Checked = false;
            }
        }
        catch
        {

        }
    }
    protected void bindtreeview()
    {
        try
        {

            this.TreeView1.Nodes.Clear();
            HierarchyTree hierarchy = new HierarchyTree();
            HierarchyTree.HGroup objhtree = null;

            string selgroup = "select distinct ITGroupPK,GroupName,ParentCode from IT_GroupMaster where CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selgroup, "Text");
            this.TreeView1.Nodes.Clear();
            hierarchy.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                objhtree = new HierarchyTree.HGroup();
                objhtree.group_code = int.Parse(ds.Tables[0].Rows[i]["ITGroupPK"].ToString());
                objhtree.parent_code = int.Parse(ds.Tables[0].Rows[i]["ParentCode"].ToString());
                objhtree.group_name = ds.Tables[0].Rows[i]["GroupName"].ToString();
                hierarchy.Add(objhtree);
            }

            foreach (HierarchyTree.HGroup hTree in hierarchy)
            {
                HierarchyTree.HGroup parentNode = hierarchy.Find(delegate(HierarchyTree.HGroup emp) { return emp.group_code == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView1.Nodes)
                    {
                        if (tn.Value == parentNode.group_code.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.group_name.ToString(), hTree.group_code.ToString()));
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.group_code.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    TreeView1.Nodes.Add(new TreeNode(hTree.group_name, hTree.group_code.ToString()));
                }
                TreeView1.ExpandAll();
                //TreeView1.ExpandAll();
            }

        }
        catch
        {

        }
    }
    protected void bindtreeviewMaping()
    {
        try
        {

            this.TreeView2.Nodes.Clear();
            HierarchyTree hierarchy = new HierarchyTree();
            HierarchyTree.HGroup objhtree = null;

            string selgroup = "select distinct ITGroupPK,GroupName,ParentCode from IT_GroupMaster where CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selgroup, "Text");
            this.TreeView2.Nodes.Clear();
            hierarchy.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                objhtree = new HierarchyTree.HGroup();
                objhtree.group_code = int.Parse(ds.Tables[0].Rows[i]["ITGroupPK"].ToString());
                objhtree.parent_code = int.Parse(ds.Tables[0].Rows[i]["ParentCode"].ToString());
                objhtree.group_name = ds.Tables[0].Rows[i]["GroupName"].ToString();
                hierarchy.Add(objhtree);
            }

            foreach (HierarchyTree.HGroup hTree in hierarchy)
            {
                HierarchyTree.HGroup parentNode = hierarchy.Find(delegate(HierarchyTree.HGroup emp) { return emp.group_code == hTree.parent_code; });
                if (parentNode != null)
                {
                    foreach (TreeNode tn in TreeView2.Nodes)
                    {
                        if (tn.Value == parentNode.group_code.ToString())
                        {
                            tn.ChildNodes.Add(new TreeNode(hTree.group_name.ToString(), hTree.group_code.ToString()));
                        }
                        if (tn.ChildNodes.Count > 0)
                        {
                            foreach (TreeNode ctn in tn.ChildNodes)
                            {
                                RecursiveChild(ctn, parentNode.group_code.ToString(), hTree);
                            }
                        }
                    }
                }
                else
                {
                    TreeView2.Nodes.Add(new TreeNode(hTree.group_name, hTree.group_code.ToString()));
                }
                TreeView2.ExpandAll();
                //TreeView2.ExpandAll();
            }

        }
        catch
        {

        }
    }
    #endregion
    #region Selected Index Changed
    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
    }
    protected void TreeView2_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView2.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
        string value = TreeView2.SelectedValue;
        int CountChild = TreeView2.SelectedNode.ChildNodes.Count;
        if (CountChild == 0)
        {
            lblNode.Text = value.Trim();
            string Query = "select IT_IDFK,ITGroupType from IT_GroupMaster G,IT_GroupMapping M where  G.ITGroupPk=M.ITGroupFK and ITGroupPk ='" + value.Trim() + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string ITGroupType = Convert.ToString(ds.Tables[0].Rows[0]["ITGroupType"]);
                List<Int64> GroupID = ds.Tables[0].AsEnumerable().Select(r => r.Field<Int64>("IT_IDFK")).ToList<Int64>();
                if (ITGroupType.Trim() == "1")
                {
                    ddlHeadtype.SelectedIndex = 0;
                    BindIncomeHeade();
                }
                else if (ITGroupType.Trim() == "2")
                {
                    ddlHeadtype.SelectedIndex = 1;
                    BindDeductionHead();
                }
                int Count = 0;
                if (cblincome.Items.Count > 0)
                {
                    for (int intcbl = 0; intcbl < cblincome.Items.Count; intcbl++)
                    {
                        if (GroupID.Contains(Convert.ToInt64(cblincome.Items[intcbl].Value)))
                        {
                            cblincome.Items[intcbl].Selected = true;
                            Count++;
                        }
                    }
                    if (Count > 0)
                    {
                        txtIncomeHead.Text = "Head(" + Count + ")";
                    }
                }
                btnMappingChild.Visible = true;
            }
            else
            {
                txtIncomeHead.Text = "Select";
                for (int intcbl = 0; intcbl < cblincome.Items.Count; intcbl++)
                {
                    cblincome.Items[intcbl].Selected = false;
                }
                btnMappingChild.Visible = true;
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alerterr.Text = "Please Select Child Node";
            btnMappingChild.Visible = false;
        }
    }
    protected void TreeView1_DataBound(object sender, EventArgs e)
    {

    }
    protected void BtnExitTree_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        lblerr.Visible = false;
    }
    protected void cblincome_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {

            chklstchange(cbIncome, cblincome, txtIncomeHead, "Head");
        }
        catch
        {

        }
    }
    protected void cbIncome_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            chkchange(cbIncome, cblincome, txtIncomeHead, "Head");
        }
        catch
        {

        }
    }
    protected void rdbincomeHead_Click(object sender, EventArgs e)
    {
        try
        {
            TitleSpan.InnerHtml = "Income Head";
            BindIncomeHeade();
        }
        catch
        {

        }
    }
    protected void rdbDeductHead_Click(object sender, EventArgs e)
    {
        try
        {
            TitleSpan.InnerHtml = "Deduction Head";
            BindDeductionHead();
        }
        catch
        {

        }
    }
    protected void ddlcolload_SelectedIndexChanged(object sender, EventArgs e)
    {
        bttngo_Click(sender, e);
    }
    protected void chkchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            if (chkchange.Checked == true)
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = true;
                }
                txtchange.Text = label + "(" + Convert.ToString(chklstchange.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chklstchange.Items.Count; i++)
                {
                    chklstchange.Items[i].Selected = false;
                }
                txtchange.Text = "Select";
            }
        }
        catch { }
    }
    protected void chklstchange(CheckBox chkchange, CheckBoxList chklstchange, TextBox txtchange, string label)
    {
        try
        {
            txtchange.Text = "Select";
            chkchange.Checked = false;
            int count = 0;
            for (int i = 0; i < chklstchange.Items.Count; i++)
            {
                if (chklstchange.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txtchange.Text = label + "(" + count + ")";
                if (count == chklstchange.Items.Count)
                {
                    chkchange.Checked = true;
                }
            }
        }
        catch { }
    }
    protected void ddlHeadtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlHeadtype.SelectedValue == "1")
            {
                TitleSpan.InnerHtml = "Income Head";
                BindIncomeHeade();
            }
            else if (ddlHeadtype.SelectedValue == "2")
            {
                TitleSpan.InnerHtml = "Deduction Head";
                BindDeductionHead();
            }
        }
        catch
        {

        }
    }
    #endregion
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

        lbl.Add(lblcol);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 04-10-2016 sudhagar
    #region Step Button Events
    protected void btnGroupMapping_Click(object sender, EventArgs e)
    {
        try
        {
            CreateGroup.Visible = false;
            CreatePriority.Visible = false;
            CreateMapping.Visible = true;
            bindtreeviewMaping();
            BindIncomeHeade();
            TitleSpan.InnerHtml = "Income Head";
        }
        catch
        {

        }
    }
    protected void btnGroupPrioriry_Click(object sender, EventArgs e)
    {
        try
        {
            Prioriry();
        }
        catch
        {

        }
    }
    protected void BtnNewTree_Click(object sender, EventArgs e)
    {
        TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Blue;
        popupwindow.Visible = true;
        btnadd.Visible = true;
        btnAddnew.Visible = false;
        btnupdate.Visible = false;
        btndelete.Visible = false;
        btnpopexit.Visible = true;
        lblerr.Visible = false;
        lblhead.Visible = true;
        lblhead.Text = "Add New Group";
        txtgroup.Text = "";
        txtdesc.Text = "";
        txtMaxLimtAmount.Text = "";
        CreateGroup.Visible = true;
        CreatePriority.Visible = false;
        CreateMapping.Visible = false;
    }
    #endregion



}