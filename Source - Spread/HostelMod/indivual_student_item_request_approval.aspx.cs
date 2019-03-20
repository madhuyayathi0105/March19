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

public partial class indivual_student_item_request_approval : System.Web.UI.Page
{
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();

    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string course_id = string.Empty;
    string dept_id = string.Empty;

    DataSet ds2 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    bool check = false;

    int i = 0;



    protected void Page_Load(object sender, EventArgs e)
    {
        //****************************************************//
        if (Session["collegecode"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }

        if (!Request.FilePath.Contains("hostelindex"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/HostelMod/hostelindex.aspx");
                return;
            }
        }
        //****************************************************//
        usercode = Session["usercode"].ToString();
        collegecode1 = Session["collegecode"].ToString();
        singleuser = Session["single_user"].ToString();
        group_user = Session["group_code"].ToString();


        if (!IsPostBack)
        {
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            Fpspread1.Visible = false;

            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.Visible = false;
            btn_go1_Click(sender, e);
            BindBatch();
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();
            bindcollege();
            binddepartment();
            btn_save1.Visible = false;
            btn_exit2.Visible = false;
            rdb_wfa.Checked = true;
            // btn_go_Click(sender, e);
        }
        lbl_error.Visible = false;


    }
    public void rdb_wfa_CheckedChanged(object sender, EventArgs e)
    {
        lbl_staff.Visible = false;
        txt_staff.Visible = false;
        Panel6.Visible = false;
    }
    public void rdb_reject_CheckedChanged(object sender, EventArgs e)
    {
        lbl_staff.Visible = true;
        txt_staff.Visible = true;
        Panel6.Visible = true;
        bindrejstaff();
    }
    public void rdb_app_CheckedChanged(object sender, EventArgs e)
    {
        lbl_staff.Visible = true;
        txt_staff.Visible = true;
        Panel6.Visible = true;
        bindappstaff();
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
    public void cb_batch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_batch.Text = "--Select--";
            if (cb_batch.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = true;
                }
                txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";


            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                }
                txt_batch.Text = "--Select--";
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();

        }
        catch (Exception ex)
        {

        }

    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string buildvalue = "";
            string build = "";
            cb_batch.Checked = false;
            txt_batch.Text = "--Select--";


            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    //cb_batch.Checked = false;
                    build = cbl_batch.Items[i].Value.ToString();
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


            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            BindDegree();
            bindbranch();
            bindsem();
            bindsec();

        }
        catch (Exception ex)
        {

        }
    }

    public void BindBatch()
    {
        try
        {
            cbl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();

            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (int row = 0; row < cbl_batch.Items.Count; row++)
                {
                    cbl_batch.Items[row].Selected = true;
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
            }

            else
            {

                txt_batch.Text = "--Select--";
            }

        }
        catch
        {
        }

    }
    public void cb_degree_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int count = 0;
            txt_degree.Text = "--Select--";

            if (cb_degree.Checked == true)
            {
                count++;
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = true;
                }
                txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";

            }


            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    //txt_degree.Text = "--Select--";
                    //txtbranch.Text = "--Select--";
                    //chklstbranch.ClearSelection();
                    //chkbranch.Checked = false;
                }
                txt_degree.Text = "--Select--";
            }


            bindbranch();
            bindsem();
            bindsec();
            // bindhostelname();

        }
        catch (Exception ex)
        {
        }

    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            int i = 0;
            int commcount = 0;
            cb_degree.Checked = false;
            txt_degree.Text = "--Select--";
            for (i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_degree.Items.Count)
                {
                    cb_degree.Checked = true;
                }
                txt_degree.Text = "Degree (" + commcount.ToString() + ")";
            }


            bindbranch();
            bindsem();
            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }

    }
    public void BindDegree()
    {
        try
        {
            cbl_degree.Items.Clear();
            string build = "";
            if (cbl_batch.Items.Count > 0)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_batch.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_batch.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = d2.BindDegree(singleuser, group_user, collegecode1, usercode);
                int count1 = ds.Tables[0].Rows.Count;
                if (count1 > 0)
                {
                    cbl_degree.DataSource = ds;
                    cbl_degree.DataTextField = "course_name";
                    cbl_degree.DataValueField = "course_id";
                    cbl_degree.DataBind();
                    if (cbl_degree.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_degree.Items.Count; row++)
                        {
                            cbl_degree.Items[row].Selected = true;
                        }
                        cb_degree.Checked = true;
                        txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_degree.Checked = false;
                txt_degree.Text = "--Select--";
            }

        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_branch.Text = "--Select--";
            if (cb_branch.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = "Branch(" + (cbl_branch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = false;
                }
                txt_branch.Text = "--Select--";
            }


            bindsem();
            bindsec();
            //bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cbl_sem.Items.Clear();

            int commcount = 0;
            cb_branch.Checked = false;
            txt_branch.Text = "--Select--";
            int commcount1 = 0;

            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_branch.Items.Count)
                {

                    cb_branch.Checked = true;
                }
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";

            }

            bindsem();
            bindsec();
            //bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void bindbranch()
    {
        try
        {

            cbl_branch.Items.Clear();
            string course_id = "";
            if (cbl_degree.Items.Count > 0)
            {
                for (int row = 0; row < cbl_degree.Items.Count; row++)
                {
                    if (cbl_degree.Items[row].Selected == true)
                    {
                        if (course_id == "")
                        {
                            course_id = Convert.ToString(cbl_degree.Items[row].Value);
                        }
                        else
                        {
                            course_id = course_id + "," + Convert.ToString(cbl_degree.Items[row].Value);
                        }
                    }
                }

            }
            if (course_id != "")
            {
                ds = d2.BindBranchMultiple(singleuser, group_user, course_id, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    if (cbl_branch.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_branch.Items.Count; row++)
                        {
                            cbl_branch.Items[row].Selected = true;
                        }
                        cb_branch.Checked = true;
                        txt_branch.Text = "Branch(" + cbl_branch.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_branch.Checked = false;
                txt_branch.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    public void cb_sem_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sem.Text = "--Select--";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                }
                txt_sem.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }
    }
    public void cbl_sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_sem.Text = "--Select--";

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {

                    cb_sem.Checked = true;
                }
                txt_sem.Text = "Semester(" + commcount.ToString() + ")";

            }

            bindsec();
            // bindhostelname();
        }
        catch (Exception ex)
        {

        }

    }


    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        ds.Clear();
        string branch = "";
        string build = "";
        string batch = "";
        if (cbl_branch.Items.Count > 0)
        {
            for (i = 0; i < cbl_branch.Items.Count; i++)
            {

                if (cbl_branch.Items[i].Selected == true)
                {
                    build = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = build;
                    }
                    else
                    {
                        branch = branch + "," + build;

                    }
                }
            }
        }
        build = "";
        if (cbl_batch.Items.Count > 0)
        {
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {

                if (cbl_batch.Items[i].Selected == true)
                {
                    build = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = build;
                    }
                    else
                    {
                        batch = batch + "," + build;

                    }

                }
            }

        }
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            ds = d2.BindSem(branch, batch, collegecode1);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dur = Convert.ToString(ds.Tables[0].Rows[i][0]);
                    if (dur.Trim() != "")
                    {
                        if (duration < Convert.ToInt32(dur))
                        {
                            duration = Convert.ToInt32(dur);
                        }
                    }
                }
            }
            if (duration != 0)
            {
                for (i = 1; i <= duration; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                }
                if (cbl_sem.Items.Count > 0)
                {
                    for (int row = 0; row < cbl_sem.Items.Count; row++)
                    {
                        cbl_sem.Items[row].Selected = true;
                        cb_sem.Checked = true;
                    }
                    txt_sem.Text = "Sem(" + cbl_sem.Items.Count + ")";
                }
            }
        }



    }
    public void cb_sec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sec.Text = "--Select--";
            if (cb_sec.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Semester(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
            //bindhostelname();
        }


        catch (Exception ex)
        {

        }
    }
    public void cbl_sec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_sec.Text = "--Select--";
            cb_sec.Checked = false;

            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sec.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sec.Items.Count)
                {

                    cb_sec.Checked = true;
                }
                txt_sec.Text = "Section(" + commcount.ToString() + ")";

            }
            //bindhostelname();


        }

        catch (Exception ex)
        {

        }
    }


    public void bindsec()
    {
        try
        {
            cbl_sec.Items.Clear();
            txt_sec.Text = "---Select---";
            cb_sec.Checked = false;
            string build = "";
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        if (build == "")
                        {
                            build = Convert.ToString(cbl_sem.Items[i].Value);
                        }
                        else
                        {
                            build = build + "'" + "," + "'" + Convert.ToString(cbl_sem.Items[i].Value);
                        }
                    }
                }
            }
            if (build != "")
            {
                ds = d2.BindSectionDetailmult(collegecode1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sec.DataSource = ds;
                    cbl_sec.DataTextField = "sections";
                    cbl_sec.DataValueField = "sections";
                    cbl_sec.DataBind();
                    if (cbl_sec.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sec.Items.Count; row++)
                        {
                            cbl_sec.Items[row].Selected = true;
                            cb_sec.Checked = true;
                        }
                        txt_sec.Text = "Section(" + cbl_sec.Items.Count + ")";
                    }

                }
            }
            else
            {
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }
    protected void cb_staff_CheckedChanged(object sender, EventArgs e)
    {
        int cout = 0;

        txt_staff.Text = "--Select--";
        if (cb_staff.Checked == true)
        {
            cout++;
            for (int i = 0; i < cbl_staff.Items.Count; i++)
            {
                cbl_staff.Items[i].Selected = true;
            }
            txt_staff.Text = "Staff Name(" + (cbl_staff.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cbl_staff.Items.Count; i++)
            {
                cbl_staff.Items[i].Selected = false;
            }
            txt_staff.Text = "--Select--";
        }


    }
    protected void cbl_staff_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int i = 0;
            cb_staff.Checked = false;
            int commcount = 0;

            txt_staff.Text = "--Select--";
            for (i = 0; i < cbl_staff.Items.Count; i++)
            {
                if (cbl_staff.Items[i].Selected == true)
                {
                    commcount = commcount + 1;

                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_staff.Items.Count)
                {

                    cb_staff.Checked = true;
                }
                txt_staff.Text = "Staff Name(" + commcount.ToString() + ")";

            }
        }
        catch (Exception ex)
        {

        }


    }

    public void bindappstaff()
    {

        try
        {
            cbl_staff.Items.Clear();
            string deptquery = "select distinct s.staff_code,s.staff_name from StudItemRequestMaster si,staffmaster s where si.AppStaffCode =s.staff_code and AppStatus ='1'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff.DataSource = ds;
                cbl_staff.DataTextField = "staff_name";
                cbl_staff.DataValueField = "staff_code";
                cbl_staff.DataBind();

                if (cbl_staff.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff.Items.Count; i++)
                    {
                        cbl_staff.Items[i].Selected = true;
                    }
                    cb_staff.Checked = true;
                    txt_staff.Text = "Staff Name(" + cbl_staff.Items.Count + ")";
                }
            }
            else
            {
                txt_staff.Text = "--Select--";
            }
        }
        catch
        {

        }

    }
    public void bindrejstaff()
    {

        try
        {
            cbl_staff.Items.Clear();
            string deptquery = "select distinct s.staff_code,s.staff_name from StudItemRequestMaster si,staffmaster s where si.AppStaffCode =s.staff_code and AppStatus ='2'  ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_staff.DataSource = ds;
                cbl_staff.DataTextField = "staff_name";
                cbl_staff.DataValueField = "staff_code";
                cbl_staff.DataBind();

                if (cbl_staff.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_staff.Items.Count; i++)
                    {
                        cbl_staff.Items[i].Selected = true;
                    }
                    cb_staff.Checked = true;
                    txt_staff.Text = "Staff Name(" + cbl_staff.Items.Count + ")";
                }
            }
            else
            {
                txt_staff.Text = "--Select--";
            }
        }
        catch
        {

        }

    }
    //protected void btn_addnew_Click(object sender, EventArgs e)
    //{
    //    Fpspread2.Sheets[0].RowCount = 0;
    //    poperrjs.Visible = true;
    //    clearaddnewpopup();

    //    //btn_go1_Click(sender, e);
    //}
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
            cblcolumnorder.ClearSelection();
            CheckBox_column.Checked = false;
            lnk_columnorder.Visible = false;
            //cblcolumnorder.Items[0].Selected = true;
            ItemList.Clear();
            Itemindex.Clear();
            tborder.Text = "";
            tborder.Visible = false;
            //Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }


    protected void btn_exit_Click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    public void btn_go1_Click(object sender, EventArgs e)
    {
        Fpspread2.Sheets[0].RowCount = 0;
        Fpspread2.Sheets[0].ColumnCount = 0;
        Fpspread2.CommandBar.Visible = false;
        Fpspread2.Sheets[0].AutoPostBack = false;
        Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
        Fpspread2.Sheets[0].RowHeader.Visible = false;
        Fpspread2.Sheets[0].ColumnCount = 6;


        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.White;
        Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[0].Width = 50;
        Fpspread2.Columns[0].Locked = true;


        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[2].Width = 100;
        Fpspread2.Columns[2].Locked = true;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[2].Width = 130;
        Fpspread2.Columns[2].Locked = true;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Specification";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[4].Width = 130;
        Fpspread2.Columns[4].Locked = true;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Approval";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
        Fpspread2.Columns[2].Width = 130;

        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Purpose";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
        Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;


        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
        cb.AutoPostBack = true;
        //FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
        //cb1.AutoPostBack = false;
        FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
        db.ErrorMessage = "Only Allow Numbers";
        db.MinimumValue = 1;
        db.MaximumValue = 10;

        //Fpspread2.Sheets[0].RowCount++;
        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;//cb select all
        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;


        for (int row = 0; row < 1; row++)
        {
            Fpspread2.Sheets[0].RowCount++;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
            cb1.AutoPostBack = false;

            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb1;
            Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
        }
        Fpspread2.Visible = true;
        //div1.Visible = true;
        //lblerror.Visible = false;
        //btnsave.Visible = true;
        Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
    }

    protected void Fpspread2_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread2.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread2.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread2.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread2.Sheets[0].RowCount; i++)
                        {
                            Fpspread2.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }

    protected void btn_errclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void btn_save1_Click(object sender, EventArgs e)
    {
        try
        {
            string name = "";
            string wardencode = "";
            string activerow = "";
            string activecol = "";
            if (Fpstaff.Sheets[0].RowCount != 0)
            {
                activerow = Fpstaff.ActiveSheetView.ActiveRow.ToString();
                activecol = Fpstaff.ActiveSheetView.ActiveColumn.ToString();
                if (activerow != Convert.ToString(-1))
                {
                    if (txt_searchby.Text == "" || txt_searchby.Text != "")
                    {
                        name = Fpstaff.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
                        txt_appstuname.Text = name;
                        wardencode = Convert.ToString(Fpstaff.Sheets[0].Cells[i, 1].Text);
                    }
                    ViewState["WardenCode"] = Convert.ToString(wardencode);
                }
                popupsscode1.Visible = false;
            }
            else
            {
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "Please Select Any One Staff";
            }

        }
        catch (Exception ex)
        {

        }
    }
    public void btn_exit2_Click(object sender, EventArgs e)
    {
        try
        {
            popupsscode1.Visible = false;
        }
        catch
        {
        }
    }
    public void btn_go2_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            int rowcount;
            int rolcount = 0;
            int sno = 0;
            if (txt_searchby.Text != "")
            {
                if (ddl_searchby.SelectedIndex == 0)
                {
                    sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.Staff_name ='" + Convert.ToString(txt_searchby.Text) + "'";
                }
            }
            //else if (txt_wardencode.Text.Trim() != "")
            //{
            //    if (ddl_searchby.SelectedIndex == 1)
            //    {
            //        sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and s.staff_code ='" + Convert.ToString(txt_wardencode.Text) + "'";
            //    }
            //}
            else
            {
                sql = "select s.staff_code,s.staff_name ,h.dept_code,h.dept_name,d.desig_code,desig_name  from staffmaster s,stafftrans st,hrdept_master h ,desig_master d where s.staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and resign =0 and settled =0 and s.college_code =h.college_code and h.dept_code in ('" + ddl_department.SelectedItem.Value + "')";
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
            Fpstaff.Sheets[0].AutoPostBack = false;
            ds = d2.select_method_wo_parameter(sql, "Text");
            Fpstaff.Sheets[0].RowCount = 0;
            Fpstaff.Sheets[0].ColumnCount = 5;

            if (ds.Tables[0].Rows.Count > 0)
            {
                btn_exit2.Visible = true;
                btn_save1.Visible = true;
                Fpstaff.Visible = true;
                btn_save1.Visible = true;
                btn_exit2.Visible = true;
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
                btndiv1.Visible = true;
                lbl_errorsearch.Visible = true;
                lbl_errorsearch.Text = "No Records Found";
                lbl_errorsearch.Text = "No of Staff :" + sno.ToString();
                rowcount = Fpstaff.Sheets[0].RowCount;
                Fpstaff.Height = 370;
                Fpstaff.Sheets[0].PageSize = 25 + (rowcount * 20);
                Fpstaff.SaveChanges();

            }
            else
            {
                btndiv1.Visible = false;
                Fpstaff.Visible = false;
                //err.Visible = true;
                //err.Text = "No Records Found";
            }
        }
        catch (Exception ex)
        {
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "select staff_name  from staffmaster where resign =0 and settled =0 and staff_name like  '" + prefixText + "%'";
        name = ws.Getname(query);
        return name;
    }

    protected void bindcollege()
    {
        try
        {
            string clgname = "";
            ds.Clear();
            ddl_college.Items.Clear();
            clgname = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(clgname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_college.DataSource = ds;
                ddl_college.DataTextField = "collname";
                ddl_college.DataValueField = "college_code";
                ddl_college.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void binddepartment()
    {
        ds.Clear();
        //query = "";
        //query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + ddl_college2.SelectedValue.ToString() + "'";
        ds = d2.loaddepartment(ddl_college.SelectedValue.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_department.DataSource = ds;
            ddl_department.DataTextField = "Dept_Name";
            ddl_department.DataValueField = "Dept_Code";
            ddl_department.DataBind();
            //ddl_department3.Items.Insert(0, "All");
        }
    }
    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = false;
    }
    public void btn_appstuname_Click(object sender, EventArgs e)
    {
        popupsscode1.Visible = true;
        btndiv1.Visible = false;
    }
    //protected void btn_go_Click(object sender, EventArgs e)
    //{
    //    try
    //    {

    //                Fpspread1.Sheets[0].RowCount = 0;
    //                Fpspread1.Sheets[0].ColumnCount = 0;
    //                Fpspread1.CommandBar.Visible = false;
    //                Fpspread1.Sheets[0].AutoPostBack = true;
    //                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
    //                Fpspread1.Sheets[0].RowHeader.Visible = false;
    //                Fpspread1.Sheets[0].ColumnCount = 8;
    //                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
    //                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
    //                darkstyle.ForeColor = Color.White;
    //                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread1.Columns[0].Width = 50;

    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Student Name";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread1.Columns[1].Width = 150;

    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Roll No";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Total No Of Item";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
    //                Fpspread1.Columns[3].Width = 150;


    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Degree";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

    //         Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Branch";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

    //         Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Semester";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

    //         Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Section";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
    //                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

    //                for (int row = 0; row < 1; row++)
    //                {
    //                    Fpspread1.Sheets[0].RowCount++;
    //                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
    //                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
    //                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
    //                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

    //                }
    //                Fpspread1.Visible = true;
    //                //rptprint.Visible = true;
    //                div1.Visible = true;
    //                lbl_error.Visible = false;
    //                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
    //        //    }
    //        //    else
    //        //    {
    //        //        div1.Visible = false;
    //        //        Fpspread1.Visible = false;
    //        //        rptprint.Visible = false;
    //        //        lbl_error.Visible = true;
    //        //        lbl_error.Text = "No Records Found";
    //        //    }
    //        //}
    //        //else
    //        //{
    //        //    div1.Visible = false;
    //        //    Fpspread1.Visible = false;
    //        //    rptprint.Visible = false;
    //        //    lbl_error.Visible = true;
    //        //    lbl_error.Text = "Please Select Any one Item Name";
    //        //}
    //    }
    //    catch
    //    {

    //    }
    //}
    public void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string sql = "";
            //string hoscode = "";
            string batch = "";
            string degree = "";
            string branch = "";
            string semester = "";
            string section = "";
            //string studtype = "";
            // int index;
            string colno = "";
            // int j = 0;

            //for batch
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    string batch1 = cbl_batch.Items[i].Value.ToString();
                    if (batch == "")
                    {
                        batch = batch1;
                    }
                    else
                    {
                        batch = batch + "'" + "," + "'" + batch1;
                    }
                }
            }

            //for degree
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    string degree1 = cbl_degree.Items[i].Value.ToString();
                    if (degree == "")
                    {
                        degree = degree1;
                    }
                    else
                    {
                        degree = degree + "'" + "," + "'" + degree1;
                    }
                }
            }

            //for branch
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    string branch1 = cbl_branch.Items[i].Value.ToString();
                    if (branch == "")
                    {
                        branch = branch1;
                    }
                    else
                    {
                        branch = branch + "'" + "," + "'" + branch1;
                    }
                }
            }

            //for semester
            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    string semester1 = cbl_sem.Items[i].Text.ToString();
                    if (semester == "")
                    {
                        semester = semester1;
                    }
                    else
                    {
                        semester = semester + "'" + "," + "'" + semester1;
                    }
                }
            }

            //for section
            for (int i = 0; i < cbl_sec.Items.Count; i++)
            {
                if (cbl_sec.Items[i].Selected == true)
                {
                    string section1 = cbl_sec.Items[i].Value.ToString();
                    if (section == "")
                    {
                        section = section1;
                    }
                    else
                    {
                        section = section + "'" + "," + "'" + section1;
                    }
                }
            }

            //for hostelcode


            Hashtable columnhash = new Hashtable();
            columnhash.Clear();

            int colinc = 0;
            columnhash.Add("Roll_No", "Roll No");
            columnhash.Add("Stud_Name", "Name");
            //columnhash.Add("Stud_Type", "Student Type");

            //columnhash.Add("Student_Mobile", "Mobile Number");
            //columnhash.Add("parent_phnop", "Phone Number");
            columnhash.Add("Course_Name", "Degree");
            columnhash.Add("Dept_Name", "Branch");
            columnhash.Add("Current_Semester", "Semester");
            columnhash.Add("Sections", "Section");
            columnhash.Add("TotItemQty", "Total No Of Item");

            if (ItemList.Count == 0)
            {
                ItemList.Add("Roll_No");
                ItemList.Add("Stud_Name");
                ItemList.Add("Course_Name");
                ItemList.Add("Dept_Name");
            }
            for (int i = 0; i <= 3; i++)
            {
                cblcolumnorder.Items[i].Selected = true;
                lnk_columnorder.Visible = true;

                //tborder.Visible = true;

            }
            cblcolumnorder_SelectedIndexChanged(sender, e);



            string getday = "";
            string gettoday = "";
            string from = "";
            string to = "";
            from = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = from.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            getday = dt.ToString("MM/dd/yyyy");

            to = Convert.ToString(txt_todate.Text);
            string[] splitdate1 = to.Split('-');
            splitdate1 = splitdate1[0].Split('/');
            DateTime dt1 = new DateTime();
            if (splitdate1.Length > 0)
            {
                dt1 = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
            }
            gettoday = dt1.ToString("MM/dd/yyyy");

            if (rdb_wfa.Checked == true)
            {
                sql = "select distinct r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir,StudItemRequestDetail ird where ir.StudItemRequestMasterID=ird.StudItemRequestMasterID and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and a.batch_year in ('" + batch + "') and D.Degree_Code in('" + branch + "') and r.Current_Semester in('" + semester + "') and Sections in('" + section + "','')  and ir.ReqDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ( ir.AppStatus='0' or ir.AppStatus ='1') and ird.AppStatus='0'";
            }
            else if (rdb_app.Checked == true)
            {
                sql = "select distinct r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir,StudItemRequestDetail ird  where ir.StudItemRequestMasterID=ird.StudItemRequestMasterID and  r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and a.batch_year in ('" + batch + "') and D.Degree_Code in('" + branch + "') and r.Current_Semester in('" + semester + "') and Sections in('" + section + "','')  and ir.ReqDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ird.AppStatus='1' ";
            }
            else if (rdb_reject.Checked == true)
            {
                sql = "select distinct r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections, ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, Course c,StudItemRequestMaster ir,StudItemRequestDetail ird  where ir.StudItemRequestMasterID=ird.StudItemRequestMasterID and  r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and a.batch_year in ('" + batch + "') and D.Degree_Code in('" + branch + "') and r.Current_Semester in('" + semester + "') and Sections in('" + section + "','')  and ir.ReqDate between '" + dt.ToString("MM/dd/yyyy") + "' and '" + dt1.ToString("MM/dd/yyyy") + "' and ird.AppStatus='2' ";
            }
            else
            {
            }

            //            sql = "select distinct r.Stud_Name,r.Roll_No,r.Stud_Type,c.Course_Name ,dt.Dept_Name ,r.Current_Semester,Sections," +
            //                "a.Student_Mobile,parent_phnop,ir.TotItemQty from Registration r,applyn a,Degree d,Department dt, " +
            //"Course c,StudItemRequestMaster ir,Hostel_StudentDetails hs,Hostel_Details hd where r.App_No =a.app_no and " +
            //"d.Degree_Code =r.degree_code and d.college_code =r.college_code  and dt.Dept_Code =d.Dept_Code " +
            //"and d.Course_Id =c.Course_Id and r.Roll_No=ir.Roll_No and r.Roll_Admit=hs.Roll_Admit and hs.Hostel_Code=hd.Hostel_code " +
            //" and ir.ReQDate between '" + dt.ToString("MM/dd/yyyy") + "'and '" + dt1.ToString("MM/dd/yyyy") + "' and " +
            //  " a.batch_year in ('" + batch + "') and D.Degree_Code in('" + branch + "') and " +
            //            "r.Current_Semester in('" + semester + "') and Sections in('" + section + "','')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                pcolumnorder.Visible = true;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                //Fpspread1.Sheets[0].ColumnCount = 11;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.SheetCorner.ColumnCount = 0;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 1;
                Fpspread1.Sheets[0].RowCount = ds.Tables[0].Rows.Count;
                Fpspread1.Sheets[0].AutoPostBack = true;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
                FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                {
                    colno = Convert.ToString(ds.Tables[0].Columns[j]);
                    if (ItemList.Contains(Convert.ToString(colno)))
                    {
                        int index = ItemList.IndexOf(Convert.ToString(colno));
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Text = Convert.ToString(columnhash[colno]);
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, index + 1].HorizontalAlign = HorizontalAlign.Center;
                    }
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Fpspread1.Sheets[0].Cells[i, 0].Text = Convert.ToString(i + 1);
                    Fpspread1.Sheets[0].Cells[i, 0].HorizontalAlign = HorizontalAlign.Center;

                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                        {
                            int index = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                            Fpspread1.Sheets[0].Columns[index + 1].Width = 150;
                            Fpspread1.Sheets[0].Columns[index + 1].Locked = true;
                            Fpspread1.Sheets[0].Cells[i, index + 1].CellType = txt;
                            Fpspread1.Sheets[0].Cells[i, index + 1].Text = ds.Tables[0].Rows[i][j].ToString();
                            Fpspread1.Sheets[0].Cells[i, index + 1].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Cells[i, index + 1].Font.Size = FontUnit.Medium;

                            if (ds.Tables[0].Columns[j].ToString() == "Current_Semester")
                            {
                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                            if (ds.Tables[0].Columns[j].ToString() == "Sections")
                            {
                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                            if (ds.Tables[0].Columns[j].ToString() == "TotItemQty")
                            {
                                Fpspread1.Sheets[0].Cells[i, index + 1].HorizontalAlign = HorizontalAlign.Center;

                            }
                        }
                    }
                }
                rptprint.Visible = true;
                Fpspread1.Visible = true;
                pcolumnorder.Visible = true;
                pheaderfilter.Visible = true;
                div1.Visible = true;
                lbl_error.Visible = false;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
            }
            else
            {
                rptprint.Visible = false;
                //imgdiv2.Visible = true;
                //lbl_alerterr.Text = "No records found";
                lbl_error.Visible = true;
                lbl_error.Text = "No records found";
                pcolumnorder.Visible = false;
                pheaderfilter.Visible = false;
                div1.Visible = false;
                Fpspread1.Visible = false;
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
        try
        {
            if (check == true)
            {
                if (rdb_wfa.Checked == true)
                {
                    poperrjs.Visible = true;
                    btn_save.Visible = false;
                    btn_exit.Visible = false;
                    btn_exit_app.Visible = true;
                    //btn_update.Visible = true;
                    //btn_delete.Visible = true;
                    txt_appstuname.Text = "";
                    txt_searchby.Text = "";
                    Fpstaff.Visible = false;
                    lbl_errorsearch.Visible = false;
                    string activerow = "";
                    string activecol = "";
                    activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    activecol = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    if (activerow.Trim() != "")
                    {
                        string rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                        //string roll_admit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        // string studname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                        // string degree = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text);
                        //string hosname = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 4].Text);
                        //string hoscode = d2.GetFunction("select Hostel_Code from Hostel_Details where Hostel_Name='" + hosname + "'");
                        txt_rollno.Text = Convert.ToString(rollnum);
                        //btn_go2_Click(sender, e);
                        //                        select convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',ird.StudItemMasterID,
                        //ir.StudItemRequestMasterID,ir.TotItemQty,r.Stud_Name,ir.Roll_No,
                        //c.Course_Name ,dt.Dept_Name ,r.Stud_Type,r.Current_Semester,Sections,
                        //a.Student_Mobile,parent_phnop from Registration r,applyn a,Degree d,
                        //Department dt,Course c,StudItemRequestMaster ir,StudItemRequestDetail ird where r.Roll_No=ir.Roll_No
                        // and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code
                        //  and dt.Dept_Code =d.Dept_Code  and d.Course_Id =c.Course_Id and ir.StudItemRequestMasterID=ird.StudItemRequestMasterID 
                        //   and ir.Roll_No ='12JEAE226' and  ird.AppStatus=0

                        string query = "select convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',ir.StudItemRequestMasterID,ir.TotItemQty,r.Stud_Name,ir.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Stud_Type,r.Current_Semester,Sections,a.Student_Mobile,parent_phnop from Registration r,applyn a,Degree d,Department dt,Course c,StudItemRequestMaster ir where r.Roll_No=ir.Roll_No and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and  ir.Roll_No ='" + txt_rollno.Text + "' ";
                        //string query = "select convert(varchar,convert(datetime,ir.ReqDate,103),103) as 'ReqDate',ir.StudItemRequestMasterID,ir.TotItemQty,r.Stud_Name,r.Roll_No,c.Course_Name ,dt.Dept_Name ,r.Stud_Type,r.Current_Semester,Sections,a.Student_Mobile,parent_phnop,hd.Hostel_Name,hs.Room_Name from Registration r,applyn a,Degree d,Department dt,Course c,Hostel_StudentDetails hs,Hostel_Details hd,StudItemRequestMaster as ir,StudItemRequestDetail as ird where r.Roll_No=ir.Roll_No and ir.StudItemRequestMasterID=ird.StudItemRequestMasterID and r.Roll_Admit=hs.Roll_Admit and hs.Hostel_Code=hd.Hostel_Code and r.App_No =a.app_no and d.Degree_Code =r.degree_code and d.college_code =r.college_code and dt.Dept_Code =d.Dept_Code and d.Course_Id =c.Course_Id and r.Roll_No ='" + txt_rollno.Text + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(query, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            txt_name.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);

                            txt_degree1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]);

                            txt_branch1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Dept_Name"]);

                            txt_sem1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Current_Semester"]);

                            txt_sec1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Sections"]);

                            //txt_mono.Text = Convert.ToString(ds.Tables[0].Rows[0]["Student_Mobile"]);

                            //txt_phoneno.Text = Convert.ToString(ds.Tables[0].Rows[0]["parent_phnop"]);

                            //txt_hostelname1.Text = Convert.ToString(ds.Tables[0].Rows[0]["Hostel_Name"]);

                            //txt_roomno.Text = Convert.ToString(ds.Tables[0].Rows[0]["Room_Name"]);

                            //txt_date.Text = Convert.ToString(ds.Tables[0].Rows[0]["ReqDate"]);

                            txt_totnoofitem.Text = Convert.ToString(ds.Tables[0].Rows[0]["TotItemQty"]);

                            int reqid = Convert.ToInt16(ds.Tables[0].Rows[0]["StudItemRequestMasterID"]);
                            Session["ReqID"] = reqid;

                            //string studtype = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Type"]);
                            //if (studtype == rdb_dayscholar.Text)
                            //{
                            //    rdb_dayscholar.Enabled = true;
                            //    rdb_hostelr.Enabled = false;
                            //}
                            //else if (studtype == rdb_hostelr.Text)
                            //{
                            //    rdb_hostelr.Enabled = true;
                            //    rdb_dayscholar.Enabled = false;
                            //}
                            //if (rdb_wfa.Checked == true)
                            //{
                            //    btn_appr.Visible = true;
                            //    btn_rej.Visible = true;
                            //}
                            //else if (rdb_app.Checked == true)
                            //{
                            //    btn_appr.Visible = false;
                            //    btn_rej.Visible = false;


                            //}
                            //else if (rdb_reject.Checked == true)
                            //{
                            //    btn_appr.Visible = false;
                            //    btn_rej.Visible = false;

                            //}
                            //else
                            //{
                            //}

                            string sql = "select * from StudItemRequestDetail ird,StudItemRequestMaster ir,StudItemMaster im,TextValTable tv where ird.StudItemRequestMasterID=ir.StudItemRequestMasterID and ird.StudItemMasterID=im.StudItemMasterID and im.StudItemCode=tv.TextCode and ir.Roll_No='" + txt_rollno.Text + "' and ir.TotItemQty='" + Convert.ToInt16(txt_totnoofitem.Text) + "' and ird.AppStatus='0'";
                            loadspread2(sql);
                            popupsscode1.Visible = false;
                            poperrjs.Visible = true;
                        }

                        divv.Visible = true;

                    }
                }

            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_exit__app_Click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }
    //public void clearaddnewpopup()
    //{
    //    txt_rollno.Text = txt_totnoofitem.Text = txt_name.Text = txt_degree1.Text = "";
    //    txt_branch1.Text = txt_sem1.Text = txt_sec1.Text = "";
    //    btn_save.Visible = true;
    //    btn_exit.Visible = true;
    //    btn_exit_app.Visible = false;
    //    divv.Visible = true;
    //    txt_appstuname.Text = "";
    //    txt_searchby.Text = "";
    //    Fpstaff.Visible = false;
    //    lbl_errorsearch.Visible = false;
    //    btn_save1.Visible = true;
    //    btn_exit2.Visible = true;
    //    //Fpspread2.Sheets[0].ColumnCount = 0;
    //}
    public void loadspread2(string sqlcmd)
    {
        ds = d2.select_method_wo_parameter(sqlcmd, "TEXT");
        if (ds.Tables[0].Rows.Count > 0)
        {
            Fpspread2.Sheets[0].RowCount = 0;
            Fpspread2.Sheets[0].ColumnCount = 0;
            Fpspread2.CommandBar.Visible = false;
            Fpspread2.Sheets[0].AutoPostBack = false;
            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
            Fpspread2.Sheets[0].RowHeader.Visible = false;
            Fpspread2.Sheets[0].ColumnCount = 6;


            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.White;
            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[0].Width = 50;
            Fpspread2.Columns[0].Locked = true;


            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Item Name";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[2].Width = 100;
            Fpspread2.Columns[2].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Quantity";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[2].Width = 130;
            Fpspread2.Columns[2].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Specification";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[4].Width = 130;
            Fpspread2.Columns[4].Locked = true;

            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Approval";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            Fpspread2.Columns[2].Width = 130;


            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Purpose";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;


            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = true;
            //FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
            //cb1.AutoPostBack = false;
            FarPoint.Web.Spread.DoubleCellType db = new FarPoint.Web.Spread.DoubleCellType();
            db.ErrorMessage = "Only Allow Numbers";
            db.MinimumValue = 1;
            db.MaximumValue = 10;

            FarPoint.Web.Spread.TextCellType txtspecification = new FarPoint.Web.Spread.TextCellType();

            //Fpspread2.Sheets[0].RowCount++;
            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;//cb select all
            //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;



            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
            {
                Fpspread2.Sheets[0].RowCount++;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;

                FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
                cb1.AutoPostBack = false;


                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb1;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["TextVal"]);
                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["TextCode"]);
                //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = db;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemReqQty"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].CellType = db;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;

                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["StudItemSpec"]);
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].CellType = txtspecification;
                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
            }
            Fpspread2.Visible = true;
            //div1.Visible = true;
            //lblerror.Visible = false;
            //btnsave.Visible = true;
            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
        }

    }
    protected void btn_appr_Click(object sender, EventArgs e)
    {
        try
        {
            string TextVal = "";
            lbl_alerterr.Text = "";
            //if (txt_appstuname.Text.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please Select Any One Staff";
            //    btn_go1_Click(sender, e);
            //}
            ////else
            ////{

            ////}

            //if (TextVal.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please select any Approval";
            //    //btn_go2_Click(sender, e);
            //}
            //if (txt_appstuname.Text.Trim() != "" && TextVal.Trim() != "")
            //{
            for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
            {
                Fpspread2.SaveChanges();
                int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                if (checkval == 1)
                {

                    if (TextVal == "")
                    {
                        TextVal = "" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }
                    else
                    {
                        TextVal = TextVal + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";

                    }
                    string qty = "";
                    if (qty == "")
                    {
                        qty = "" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        qty = qty + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";

                    }
                    string itemname = "";
                    string[] separators = { ",", "'" };
                    string[] rno = TextVal.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] iname = itemname.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] iqty = qty.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < rno.Length; j++)
                    {
                        string icode = d2.GetFunction("select TextCode from TextValTable where TextVal='" + rno[j].ToString() + "' and TextCriteria='Sitem'");
                        string itemid = d2.GetFunction("select StudItemMasterID from StudItemMaster where StudItemCode='" + icode + "'");
                        //int reqid = Convert.ToInt16(Session["ReqID"]);
                        //DateTime dt = new DateTime();
                        //dt = Convert.ToDateTime(d2.GetFunction("select ReqDate from StudItemRequestMaster where StudItemRequestMasterID='"+reqid+"'"));
                        int reqid = Convert.ToInt16(d2.GetFunction("select StudItemRequestMasterID from StudItemRequestMaster where Roll_No='" + txt_rollno.Text + "' "));
                        string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + txt_appstuname.Text + "'");
                        string sql = "update StudItemRequestMaster set AppStaffCode ='" + staffcode + "' ,AppStatus ='1'where StudItemRequestMasterID='" + reqid + "'";

                        string sql1 = "update StudItemRequestDetail set StudItemAppQty ='" + iqty[j] + "' ,AppStatus ='1'where StudItemMasterID ='" + itemid + "' and StudItemRequestMasterID='" + reqid + "'";
                        //string sql = "update StudItemRequestMaster set AppStaffCode='"+staffcode+"' , AppStatus='1' where StudItemRequestMasterID=ir.StudItemRequestMasterID and Roll_No='" + txt_rollno.Text + "' and ReqDate='"+dt+"'";
                        //string sql1 = "update StudItemRequestDetail set StudItemAppQty='"+iqty[j]+"',AppStatus='1' where StudItemRequestMasterID=ir.StudItemRequestMasterID and StudItemMasterID=im.StudItemMasterID";
                        int insert = d2.update_method_wo_parameter(sql, "TEXT");
                        int insert1 = d2.update_method_wo_parameter(sql1, "TEXT");
                        if (insert != 0 && insert1 != 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alerterr.Text = "Request Approved";
                            poperrjs.Visible = false;
                            btn_go_Click(sender, e);

                        }
                    }



                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Please select any Approval";
                }



            }
            //}
            if (txt_appstuname.Text.Trim() == "")
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Select Any One Staff";
            }
            else
            {

                //btn_go1_Click(sender, e);
            }

            //if (TextVal.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please select any Approval";
            //    // btn_go1_Click(sender, e);
            //}



        }

        catch (Exception ex)
        {
        }
    }
    //protected void btnerrclose_Click(object sender, EventArgs e)
    //{
    //    Div2.Visible = false;
    //}
    protected void btn_rej_Click(object sender, EventArgs e)
    {
        try
        {
            string TextVal = "";
            lbl_alerterr.Text = "";
            //if (txt_appstuname.Text.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please Select Any One Staff";
            //    //btn_go1_Click(sender, e);
            //}
            ////else
            ////{

            ////}

            //if (TextVal.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please select any Reject";
            //    // btn_go1_Click(sender, e);
            //}
            //if (txt_appstuname.Text.Trim() != "" && TextVal.Trim() != "")
            //{
            for (int i = 0; i < Fpspread2.Sheets[0].RowCount; i++)
            {
                Fpspread2.SaveChanges();
                int checkval = Convert.ToInt32(Fpspread2.Sheets[0].Cells[i, 1].Value);
                if (checkval == 1)
                {

                    if (TextVal == "")
                    {
                        TextVal = "" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";
                    }
                    else
                    {
                        TextVal = TextVal + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 2].Text + "";

                    }
                    string qty = "";
                    if (qty == "")
                    {
                        qty = "" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";
                    }
                    else
                    {
                        qty = qty + "'" + "," + "'" + Fpspread2.Sheets[0].Cells[i, 3].Text + "";

                    }
                    string itemname = "";
                    string[] separators = { ",", "'" };
                    string[] rno = TextVal.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] iname = itemname.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string[] iqty = qty.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < rno.Length; j++)
                    {
                        string icode = d2.GetFunction("select TextCode from TextValTable where TextVal='" + rno[j].ToString() + "' and TextCriteria='Sitem'");
                        string itemid = d2.GetFunction("select StudItemMasterID from StudItemMaster where StudItemCode='" + icode + "'");
                        //int reqid = Convert.ToInt16(Session["ReqID"]);
                        //DateTime dt = new DateTime();
                        //dt = Convert.ToDateTime(d2.GetFunction("select ReqDate from StudItemRequestMaster where StudItemRequestMasterID='"+reqid+"'"));
                        int reqid = Convert.ToInt16(d2.GetFunction("select StudItemRequestMasterID from StudItemRequestMaster where Roll_No='" + txt_rollno.Text + "' "));
                        string staffcode = d2.GetFunction("select staff_code from staffmaster where staff_name='" + txt_appstuname.Text + "'");
                        string sql = "update StudItemRequestMaster set AppStaffCode ='" + staffcode + "' ,AppStatus ='2'where StudItemRequestMasterID='" + reqid + "'";

                        string sql1 = "update StudItemRequestDetail set AppStatus ='2' where StudItemMasterID ='" + itemid + "' and StudItemRequestMasterID='" + reqid + "'";
                        //string sql = "update StudItemRequestMaster set AppStaffCode='"+staffcode+"' , AppStatus='1' where StudItemRequestMasterID=ir.StudItemRequestMasterID and Roll_No='" + txt_rollno.Text + "' and ReqDate='"+dt+"'";
                        //string sql1 = "update StudItemRequestDetail set StudItemAppQty='"+iqty[j]+"',AppStatus='1' where StudItemRequestMasterID=ir.StudItemRequestMasterID and StudItemMasterID=im.StudItemMasterID";
                        int insert = d2.update_method_wo_parameter(sql, "TEXT");
                        int insert1 = d2.update_method_wo_parameter(sql1, "TEXT");
                        if (insert != 0 && insert1 != 0)
                        {
                            imgdiv2.Visible = true;
                            lbl_alerterr.Text = "Request Rejected";
                            poperrjs.Visible = false;
                            btn_go_Click(sender, e);
                        }
                    }



                }
                else
                {
                    imgdiv2.Visible = true;
                    lbl_alerterr.Text = "Please select any Reject";
                }

            }

            //}
            if (txt_appstuname.Text.Trim() == "")
            {
                imgdiv2.Visible = true;
                lbl_alerterr.Text = "Please Select Any One Staff";

            }
            else
            {

                //btn_go1_Click(sender, e);
            }

            //if (TextVal.Trim() == "")
            //{
            //    imgdiv2.Visible = true;
            //    lbl_alerterr.Text = "Please select any Reject";
            //    // btn_go1_Click(sender, e);
            //}
        }

        catch (Exception ex)
        {
        }
    }
    protected void btn_excel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txt_excelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lbl_validation.Visible = false;
            }
            else
            {
                lbl_validation.Text = "Please Enter Your Report Name";
                lbl_validation.Visible = true;
                txt_excelname.Focus();
            }
        }
        catch
        {

        }
    }
    protected void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string degreedetails = "Individual Student Item Request Approval Report";
            string pagename = "indivual_student_item_request_approval.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        {

        }

    }
}