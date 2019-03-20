using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Data.SqlClient;
using System.Configuration;

public partial class GroupMaster : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    static string collegestat = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("FinanceIndex"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/FinanceMod/FinanceIndex.aspx");
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
            //string finyear="";
            //try
            //{
            //    finyear = d2.GetFunction("select FinYearPK from FM_FinYearMaster where CollegeCode='" + ddlcolload.SelectedItem.Value + "'");
            //    if (finyear == "" || finyear == "0")
            //    {
            //        imgdiv2.Visible = true;
            //        lbl_alerterr.Text = "Please Create Finance Year";
            //    }
            //}
            //catch
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please Create Finance Year";
            //}
        }
        catch (Exception ex) { }
    }

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
                string queryledname = dd.GetFunction("select distinct GroupName,FinGroupPK from FM_FinGroupMaster where GroupName='" + grp_name + "' and CollegeCode=" + collegestat + "");
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

    protected void ddlcolload_SelectedIndexChanged(object sender, EventArgs e)
    {
        bttngo_Click(sender, e);
    }
    //protected void lb2_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Session.Abandon();
    //        Session.Clear();
    //        Session.RemoveAll();
    //        System.Web.Security.FormsAuthentication.SignOut();
    //        Response.Redirect("GroupMaster.aspx", false);
    //    }
    //    catch
    //    {
    //    }
    //}

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
                    grpname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(grpname);
                    string currcode = ViewState["currcode"].ToString();
                    if (currcode != null)
                    {
                        string inserttb = "insert into FM_FinGroupMaster (ParentCode,GroupName,GroupDesc,CollegeCode) values ('" + currcode + "','" + grpname + "','" + desc + "','" + ddlcolload.SelectedItem.Value + "')";
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
                groupname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(groupname);
                string insertquery = "Insert into FM_FinGroupMaster (ParentCode,GroupName,GroupDesc,CollegeCode) values('0','" + groupname + "','" + description + "','" + ddlcolload.SelectedItem.Value + "')";
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

            string selquery = "select * from FM_LedgerMaster where FinGroupFK='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                imgdiv1.Visible = false;
                lblalert.Visible = false;
                imgdiv2.Visible = true;
                lbl_alerterr.Visible = true;
                lbl_alerterr.Text = "You Can't Delete This Record";
            }
            else
            {
                string parcode = "";
                DataSet dsPcode = new DataSet();
                dsPcode = d2.select_method_wo_parameter("select Parentcode from FM_FinGroupMaster where CollegeCode=" + ddlcolload.SelectedItem.Value + " and FinGroupPK=" + currcode + "", "Text");
                if (dsPcode.Tables.Count > 0 && dsPcode.Tables[0].Rows.Count > 0)
                {
                    parcode = dsPcode.Tables[0].Rows[0][0].ToString();
                }

                string delquery = "";

                delquery = "delete from FM_FinGroupMaster where FinGroupPK='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "' ";
                if (parcode == "0")
                {
                    delquery += " delete from FM_FinGroupMaster where Parentcode='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
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
        }
        else
        {
            panel3.Visible = false;
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

                    string selquery = "select GroupDesc from FM_FinGroupMaster where FinGroupPK='" + currnodecode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(selquery, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        desc = ds.Tables[0].Rows[0]["GroupDesc"].ToString();
                    }

                    txtgroup.Text = currnodevalue;
                    txtdesc.Text = desc;
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
                TreeNode currnode = TreeView1.SelectedNode;
                TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
                string currnodevalue = currnode.Text;
                string currnodecode = currnode.Value;
                ViewState["currcode"] = currnodecode;
                string desc = "";

                string selquery = "select GroupDesc from FM_FinGroupMaster where FinGroupPK='" + currnodecode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    desc = ds.Tables[0].Rows[0]["GroupDesc"].ToString();
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
    protected void ddlcolload1_SelectedIndexChanged(object sender, EventArgs e)
    {

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
                groupname = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(groupname);
                string desc = txtdesc.Text.ToString();
                string currcode = ViewState["currcode"].ToString();

                string updatequery = "update FM_FinGroupMaster set GroupName='" + groupname + "',GroupDesc='" + desc + "' where FinGroupPK='" + currcode + "' and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
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
    protected void bindcollege()
    {
        try
        {
            string strUser = d2.getUserCode(Convert.ToString(Session["group_code"]), Convert.ToString(Session["usercode"]), 1);
            ds.Clear();
            ddlcolload.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where " + strUser + " and cp.college_code=cf.college_code";
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

    protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
    {
        TreeView1.SelectedNodeStyle.ForeColor = System.Drawing.Color.Red;
    }

    protected void TreeView1_DataBound(object sender, EventArgs e)
    {

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
    }

    protected void BtnExitTree_Click(object sender, EventArgs e)
    {
        panel3.Visible = false;
        lblerr.Visible = false;
    }

    protected void bindtreeview()
    {
        try
        {
            string dt_groupcode = "";
            string dt_parentcode = "";

            if (collegecode != "")
            {
                this.TreeView1.Nodes.Clear();
                HierarchyTree hierarchy = new HierarchyTree();
                HierarchyTree.HGroup objhtree = null;

                string selgroup = "select distinct FinGroupPK,GroupName,ParentCode from FM_FinGroupMaster where CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selgroup, "Text");
                this.TreeView1.Nodes.Clear();
                hierarchy.Clear();
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    objhtree = new HierarchyTree.HGroup();
                    objhtree.group_code = int.Parse(ds.Tables[0].Rows[i]["FinGroupPK"].ToString());
                    objhtree.parent_code = int.Parse(ds.Tables[0].Rows[i]["ParentCode"].ToString());
                    objhtree.group_name = ds.Tables[0].Rows[i]["GroupName"].ToString();
                    hierarchy.Add(objhtree);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    string get_topic_no = "";
                    string get_topic_no1 = "";
                    string get_topic_no2 = "";

                    for (int dt_row_cnt = 0; dt_row_cnt < ds.Tables[0].Rows.Count; dt_row_cnt++)
                    {
                        dt_groupcode = ds.Tables[0].Rows[dt_row_cnt][0].ToString();
                        string[] split_topics2 = dt_groupcode.Split('/');
                        for (int i = 0; split_topics2.GetUpperBound(0) >= i; i++)
                        {
                            if (get_topic_no == "")
                            {
                                get_topic_no = "'" + split_topics2[i] + "'";
                            }
                            else
                            {
                                get_topic_no = get_topic_no + ',' + "'" + split_topics2[i] + "'";
                            }
                        }
                    }

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int dt_dailyentdet1_row_cnt = 0; dt_dailyentdet1_row_cnt < ds.Tables[0].Rows.Count; dt_dailyentdet1_row_cnt++)
                        {
                            dt_parentcode = ds.Tables[0].Rows[dt_dailyentdet1_row_cnt][1].ToString();
                            string[] split_topics3 = dt_parentcode.Split('/');
                            for (int i = 0; split_topics3.GetUpperBound(0) >= i; i++)
                            {
                                if (get_topic_no1 == "")
                                {
                                    get_topic_no1 = "'" + split_topics3[i] + "'";
                                }
                                else
                                {
                                    get_topic_no1 = get_topic_no1 + ',' + "'" + split_topics3[i] + "'";
                                }
                            }
                        }

                    }
                    if (get_topic_no1 != "")
                    {
                        get_topic_no2 = get_topic_no + "," + get_topic_no1;
                    }
                    else
                    {
                        get_topic_no2 = get_topic_no;
                    }

                    selgroup = "select FinGroupPK,ParentCode,GroupName from FM_FinGroupMaster where convert(varchar,FinGroupPK) in(" + get_topic_no2 + ") and CollegeCode='" + ddlcolload.SelectedItem.Value + "' order by ParentCode,FinGroupPK";
                    DataSet dsloadtopic = d2.select_method_wo_parameter(selgroup, "Text");
                    if (dsloadtopic.Tables[0].Rows.Count > 0)
                    {
                        hierarchy.Clear();

                        for (int at = 0; at < dsloadtopic.Tables[0].Rows.Count; at++)
                        {
                            string sqlquery = "select isnull(count(*),0) as ischild from FM_FinGroupMaster where ParentCode=" + dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString() + " and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                            string ischild = d2.GetFunction(sqlquery);
                            string sqlquery1 = "select isnull(count(*),0) as isavailable from FM_FinGroupMaster where convert(varchar,FinGroupPK) in(" + get_topic_no2 + ") and ParentCode=" + dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString() + " and CollegeCode='" + ddlcolload.SelectedItem.Value + "'";
                            string isavailable = d2.GetFunction(sqlquery1);

                            if (Convert.ToInt16(ischild) == 0)
                            {
                                objhtree = new HierarchyTree.HGroup();
                                objhtree.group_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString());
                                objhtree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["ParentCode"].ToString());
                                objhtree.group_name = dsloadtopic.Tables[0].Rows[at]["GroupName"].ToString();
                                hierarchy.Add(objhtree);
                            }
                            else if (Convert.ToInt16(ischild) > 0 && Convert.ToInt16(isavailable) > 0)
                            {
                                objhtree = new HierarchyTree.HGroup();
                                objhtree.group_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["FinGroupPK"].ToString());
                                objhtree.parent_code = int.Parse(dsloadtopic.Tables[0].Rows[at]["ParentCode"].ToString());
                                objhtree.group_name = dsloadtopic.Tables[0].Rows[at]["GroupName"].ToString();
                                hierarchy.Add(objhtree);
                            }

                        }
                    }

                    panel3.Visible = true;
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
                }

                //if (TreeView1.Nodes.Count < 1)
                //{

                //    //BtnNewTree.Enabled = false;
                //}
                //else
                //{
                //    //BtnNewTree.Enabled = true;
                //}
            }
        }
        catch
        {

        }
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

        lbl.Add(lblcol);
        fields.Add(0);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    // last modified 04-10-2016 sudhagar
}