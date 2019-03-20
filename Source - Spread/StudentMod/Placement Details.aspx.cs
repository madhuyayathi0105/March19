using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Data.SqlClient;
using System.Configuration;

public partial class Placement_Details : System.Web.UI.Page
{
    ArrayList addcertificate = new ArrayList();
    static ArrayList ItemList_stud = new ArrayList();
    static ArrayList Itemindex_stud = new ArrayList();
    bool appliedbool = false;
    bool Cellclick = false;
    bool admitedbool = false;
    bool delbool = false;
    bool debarbool = false;
    bool cm_coursebool = false;
    bool ccccc = false;
    DataSet ds = new DataSet();
    DataSet nofar = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    Hashtable addtotalhash = new Hashtable();
    Hashtable hat1 = new Hashtable();
    Hashtable totalmode = new Hashtable();
    Hashtable newhash = new Hashtable();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string loadval = string.Empty;
    static string colval = string.Empty;
    static string printval = string.Empty;
    static string loadval1 = string.Empty;
    static string colval1 = string.Empty;
    static string savecolumnoder = string.Empty;
    static string columnname = string.Empty;
    static string columnname1 = string.Empty;
    static string cgpas = string.Empty;
    static int removerow=0;
    int n_arrear;
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    ReuasableMethods rs = new ReuasableMethods();
    static byte roll = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("StudentHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/StudentMod/StudentHome.aspx");
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
                BindCollege();
                bindbatch();
                edu_level();
                degree();
                bindsem();
                bindsem1();
                BindSectionDetail();
                loadstutype();
                loadstream();
                loadseat();
                loadtype();
                loadState();
                loadreligion();
                loadcommunity();
                loadallotedcommunity();
                loadTypeName();
                loadTypeSize();
                bindstatus();
                columnordertype();
                ItemList_stud.Clear();
                loadquota();
                //CalendarExtender10.EndDate = DateTime.Now;
                //CalendarExtender1.EndDate = DateTime.Now;
                detailcolumn();
                bindresidency();
                bindsports();
                bindlanguage();
                bindmothertongue();
                bindphysicalchallaged();
                bindtransport();

                //Added By Saranyadevi24.2.2018
                LoadDisContinueReason();
                loadBoardUniv();
                bindsimple();
            }
            if (Request.Params["lst_setting1"] != null && (string)Request.Params["lst_setting1"] == "doubleclicked")
            {
            }
        }
        catch (Exception ex) { }
    }
    public void bindsimple()
    {
        string type = string.Empty;
        string[] statusname = { "<", ">", "<=", ">=","==" };//modified
       
        for (int i = 0; i < 5; i++)
        {
            drbless.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
        }

        
    }
    protected void loadquota()
    {
        try
        {
            ds.Clear();
            cblQuota.Items.Clear();

            string itemheader = "";
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    if (itemheader == "")
                    {
                        itemheader = "" + cbl_seat.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        itemheader = itemheader + "'" + "," + "" + "'" + cbl_seat.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (itemheader.Trim() != "")
            {
                // string deptquery = "select distinct LedgerName,LedgerPK from FM_LedgerMaster l,FM_HeaderMaster h where l.HeaderFK = h.HeaderPK and l.HeaderFK in('" + itemheader + "') and l.LedgerMode='0' and l.CollegeCode =" + collegecode1 + "";
                // string degree = Convert.ToString(getCblSelectedValue(cbl_dept));
                //string deptquery = "select distinct cat_code,category_name from seattype_cat where quota in('" + itemheader + "') and  degree_code in('" + degree + "')";
                //select distinct quota from seattype_cat where quota in('" + itemheader + "') and college_code='" + collegecode1 + "';

                string deptquery = "select distinct quotaid,quotaname from stu_quotaseetinges where settype in('" + itemheader + "') and  collegecode in('" + ddlcollege.SelectedItem.Value + "')";//abar

                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cblQuota.DataSource = ds;
                    cblQuota.DataTextField = "quotaname";
                    cblQuota.DataValueField = "quotaid";
                    cblQuota.DataBind();

                    for (int i = 0; i < cblQuota.Items.Count; i++)
                    {
                        cblQuota.Items[i].Selected = true;
                    }
                    txtQuota.Text = "Quota(" + cblQuota.Items.Count + ")";
                    cbQuota.Checked = true;

                }
                else
                {
                    txtQuota.Text = "--Select--";
                    cbQuota.Checked = false;
                }
            }
            else
            {
                txtQuota.Text = "--Select--";
                cbQuota.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void detailcolumn()
    {
    //    tdlblstudtype.Visible = false;
    //    tdstudetype.Visible = false;
    //    tdseattype.Visible = false;
    //    tdseattype1.Visible = false;
    //    tdtype.Visible = false;
    //    tdtype1.Visible = false;
    //    tdrelichk.Visible = false;
    //    tdrelichk1.Visible = false;
    //    tdcommchk.Visible = false;
    //    tdcommchk1.Visible = false;
    //    tdresident.Visible = false;
    //    tdresident1.Visible = false;
    //    tdsports.Visible = false;
    //    tdsports1.Visible = false;
    //    tdlang.Visible = false;
    //    tdlang1.Visible = false;
    //    tdmothertng.Visible = false;
    //    tdphychallange.Visible = false;
    //    tdtransport.Visible = false;
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        loadstream();
        edu_level();
        degree();
        bindbatch();
        bindsem();
        bindsem1();
        BindSectionDetail();
        loadstutype();
        //loadstream();
        loadseat();
        loadtype();
        loadreligion();
        loadcommunity();
        loadallotedcommunity();
        loadTypeName();
        loadTypeSize();
        columnordertype();
        bindsports();
        bindresidency();
        bindlanguage();
        bindmothertongue();
        // bindstatus();
        loadquota();
        loadBoardUniv();
    }
    public void bindbatch()
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
                //ddl_batch1.SelectedIndex = 3;
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[0].Selected = true;
                    }
                    // txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    txt_batch.Text = "Batch(" + 1 + ")";
                    //cb_batch.Checked = true;
                }
                else
                {
                    txt_batch.Text = "--Select--";
                    cb_batch.Checked = false;
                }
            }
        }
        catch
        {
        }
    }
    public void degree()
    {
        try
        {
            string edulvl = string.Empty;
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    string build = cbl_graduation.Items[i].Value.ToString();
                    if (edulvl == "")
                    {
                        edulvl = build;
                    }
                    else
                    {
                        edulvl = edulvl + "','" + build;
                    }
                }
            }
            string query = string.Empty;
            string type = string.Empty;
            if (txt_stream.Enabled == true)
            {
                type = rs.GetSelectedItemsValueAsString(cbl_stream);
            }
            string rights = string.Empty;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            if (type != "")
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') " + rights + " and type in('" + type + "')";
            }
            else
            {
                query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' and Edu_Level in('" + edulvl + "') " + rights + "";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            int count1 = ds.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //    {
                    // cbl_degree.Items[0].Selected = true;
                    //cbl_sem.Items[i].Selected = true;
                    //studtype = Convert.ToString(cbl_sem.Items[i].Text);
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_sem.Items[i].Selected = true;
                        txt_degree.Text = Convert.ToString(cbl_degree.Items[i].Text);
                    }
                    if (cbl_sem.Items.Count == 1)
                    {
                        txt_sem.Text = "Degree(" + txt_degree.Text + ")";
                    }
                    else
                    {
                        txt_sem.Text = "Degree(" + txt_degree.Text + ")";
                    }
                }
                txt_degree.Text = lbl_degree.Text + "(" + 1 + ")";
                // cb_degree.Checked = true;
                //}
                //else
                //{
                //    txt_degree.Text = "--Select--";
                //    cb_degree.Checked = false;
                //}
                string deg = string.Empty;
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                bindbranch(deg);
            }
            else
            {
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindbranch(string branch)
    {
        try
        {
            branch = string.Empty;
            if (cbl_degree.Items.Count > 0)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cbl_degree.Items[i].Selected == true)
                    {
                        string build = cbl_degree.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            string rights = string.Empty;
            if (group_user.Contains(";"))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            if ((group_user.ToString().Trim() != "") && (group_user.Trim() != "0") && (group_user.ToString().Trim() != "-1"))
            {
                rights = "and group_code='" + group_user + "'";
            }
            else
            {
                rights = " and user_code='" + usercode + "'";
            }
            cb_branch.Checked = false;
            string commname = string.Empty;
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds.Clear();
            cbl_branch.Items.Clear();
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    //    for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //    {
                    cbl_branch.Items[0].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + 1 + ")";
                //}
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void bindsem()
    {
        try
        {
            string branch = string.Empty;
            string build = string.Empty;
            string build1 = string.Empty;
            string batch = string.Empty;
            int j = 0;
            cbl_sem.Items.Clear();
            string studtype = string.Empty;
            if (cbl_branch.Items.Count > 0)
            {
                for (j = 0; j < cbl_branch.Items.Count; j++)
                {
                    if (cbl_branch.Items[j].Selected == true)
                    {
                        build = cbl_branch.Items[j].Value.ToString();
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
            if (branch.Trim() != "")
            {
                string deptquery = "select distinct Current_Semester from Registration where degree_code in (" + branch + ")  order by Current_Semester";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sem.DataSource = ds;
                    cbl_sem.DataTextField = "Current_Semester";
                    cbl_sem.DataBind();
                    if (cbl_sem.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_sem.Items.Count; i++)
                        {
                            cbl_sem.Items[i].Selected = true;
                            studtype = Convert.ToString(cbl_sem.Items[i].Text);
                        }
                        if (cbl_sem.Items.Count == 1)
                        {
                            txt_sem.Text = "Semester(" + studtype + ")";
                        }
                        else
                        {
                            txt_sem.Text = "Semester(" + cbl_studtype.Items.Count + ")";
                        }
                        cb_sem.Checked = true;
                    }
                }
                else
                {
                    txt_sem.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    void BindCollege()
    {
        try
        {
            //string srisql = "select collname,college_code from collinfo";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(srisql, "Text");
            byte userType = 0;
            string userOrGroupCode = string.Empty;
            if (Session["group_code"] != null && (Convert.ToString(Session["group_code"]).Trim() != "") && (Convert.ToString(Session["group_code"]).Trim() != "0") && (Convert.ToString(Session["group_code"]).Trim() != "-1"))
            {
                userOrGroupCode = Convert.ToString(Session["group_code"]).Trim();
                userType = 0;
            }
            else if (Session["usercode"] != null)
            {
                userOrGroupCode = Convert.ToString(Session["usercode"]).Trim();
                userType = 1;
            }
            ds.Clear();
            ds = d2.BindCollegebaseonrights(userOrGroupCode, userType);
            ddlcollege.DataSource = ds;
            ddlcollege.DataTextField = "collname";
            ddlcollege.DataValueField = "college_code";
            ddlcollege.DataBind();
        }
        catch
        {
        }
    }
    private void loadBoardUniv()
    {
        cbl_BoardUniv.Items.Clear();
        try
        {
            //degree();
            string batchyear = Convert.ToString(cbl_batch.SelectedItem.Value);
            //string degreecode = Convert.ToString(cbl_degree.SelectedItem.Value);
            string branch = rs.GetSelectedItemsValueAsString(cbl_branch);
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

            //string query = "  select distinct t.TextVal,p.university_code from Registration R inner join applyn a on r.App_No = a.app_no inner join Stud_prev_details p on p.app_no = r.App_No and isnull(p.university_code,0)<>0 inner join textvaltable t on  isnull(t.TextVal,'')<>'' and textcode=convert(varchar(100),isnull(p.university_code,0)) and r.Batch_Year in('" + batchyear + "') and r.college_code in('" + collegecode + "') ";//and c.type='" + type + "'commented by abarna
            string query = "   select distinct TextVal,TextCode from textvaltable t,Stud_prev_details s,applyn a where T.TextCode= S.course_code and a.app_no=s.app_no and a.batch_year in('" + batchyear + "') and a.degree_code in('" + branch + "')  and t.college_code in(" + collegecode + ") and Textval is not null and Textval<>'' order by Textval asc";//
            // DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");
            DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");// and r.degree_code in('" + degreecode + "') 
            if (dsBoardUniv.Tables.Count > 0 && dsBoardUniv.Tables[0].Rows.Count > 0)
            {
                cbl_BoardUniv.DataSource = dsBoardUniv;
                cbl_BoardUniv.DataTextField = "TextVal";
                cbl_BoardUniv.DataValueField = "TextCode";
                cbl_BoardUniv.DataBind();
                for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                {
                    cbl_BoardUniv.Items[i].Selected = true;
                }
                txtBoardUniv.Text = "Board(" + cbl_BoardUniv.Items.Count + ")";
                cb_BoardUniv.Checked = true;
            }
        }
        catch { }
    }

    public void BindSectionDetail()
    {
        try
        {
            cbl_sec.Items.Clear();
            string batch = string.Empty;
            string branch = string.Empty;
            int i = 0;
            if (cbl_branch.Items.Count > 0)
            {
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string build = cbl_branch.Items[i].Value.ToString();
                        if (branch == "")
                        {
                            branch = build;
                        }
                        else
                        {
                            branch = branch + "','" + build;
                        }
                    }
                }
            }
            if (cbl_batch.Items.Count > 0)
            {
                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string build = cbl_batch.Items[i].Value.ToString();
                        if (batch == "")
                        {
                            batch = build;
                        }
                        else
                        {
                            batch = batch + "','" + build;
                        }
                    }
                }
            }
            if (batch.Trim() != "" && branch.Trim() != "")
            {
                string sqlquery = "select distinct sections from registration where batch_year in('" + batch + "') and degree_code in('" + branch + "') and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
                DataSet ds = new DataSet();
                ds = d2.select_method_wo_parameter(sqlquery, "text");
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
                    else
                    {
                        txt_sec.Text = "--Select--";
                    }
                }
                else
                {
                    txt_sec.Text = "--Select--";
                }
            }
        }
        catch
        {
        }
    }
    public void loadstutype()
    {
        try
        {
            cbl_studtype.Items.Clear();
            string studtype = string.Empty;
            string deptquery = "select distinct Stud_Type from Registration where college_code in('" + ddlcollege.SelectedItem.Value + "') and Stud_Type is not null and Stud_Type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_studtype.DataSource = ds;
                cbl_studtype.DataTextField = "Stud_Type";
                cbl_studtype.DataBind();
                if (cbl_studtype.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_studtype.Items.Count; i++)
                    {
                        cbl_studtype.Items[i].Selected = true;
                        studtype = Convert.ToString(cbl_studtype.Items[i].Text);
                    }
                    if (cbl_studtype.Items.Count == 1)
                    {
                        txt_studtype.Text = "Student Type(" + studtype + ")";
                    }
                    else
                    {
                        txt_studtype.Text = "Student Type(" + cbl_studtype.Items.Count + ")";
                    }
                    cb_studtype.Checked = true;
                }
            }
            else
            {
                txt_studtype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void loadstream()
    {
        try
        {
            string stream = string.Empty;
            cbl_stream.Items.Clear();
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + ddlcollege.SelectedItem.Value + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_stream.DataSource = ds;
                cbl_stream.DataTextField = "type";
                cbl_stream.DataBind();
                if (cbl_stream.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_stream.Items.Count; i++)
                    {
                        cbl_stream.Items[i].Selected = true;
                    }
                    txt_stream.Text = lbl_Stream.Text + "(" + cbl_stream.Items.Count + ")";
                    cb_stream.Checked = true;
                    txt_stream.Enabled = true;
                }
                else
                {
                    txt_stream.Text = "--Select--";
                    cb_stream.Checked = false;
                    txt_stream.Enabled = false;
                }
            }
            else
            {
                txt_stream.Enabled = false;
            }
        }
        catch
        {
        }
    }
    public void edu_level()
    {
        string st = string.Empty;
        string type = rs.GetSelectedItemsValueAsString(cbl_stream);
        if (type != "")
        {
            st = "select distinct edu_level,priority from course where college_code='" + ddlcollege.SelectedItem.Value + "' and type in('" + type + "') order by priority";
        }
        else
        {
            st = "select distinct edu_level,priority from course where college_code='" + ddlcollege.SelectedItem.Value + "' order by priority";
        }
        ds = d2.select_method_wo_parameter(st, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_graduation.DataSource = ds;
            cbl_graduation.DataTextField = "edu_level";
            cbl_graduation.DataValueField = "edu_level";
            cbl_graduation.DataBind();
            if (cbl_graduation.Items.Count > 0)
            {

                cbl_graduation.Items[0].Selected = true;
            }
            txt_graduation.Text = "Graduation(" + 1 + ")";

        }
    }
    public void loadseat()
    {
        try
        {
            cbl_seat.Items.Clear();
            string seat = string.Empty;
            string deptquery = "select Distinct TextCode,TextVal  from TextValTable where TextCriteria ='Seat' and college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_seat.DataSource = ds;
                cbl_seat.DataTextField = "TextVal";
                cbl_seat.DataValueField = "TextCode";
                cbl_seat.DataBind();
                if (cbl_seat.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        cbl_seat.Items[i].Selected = true;
                        seat = Convert.ToString(cbl_seat.Items[i].Text);
                    }
                    if (cbl_seat.Items.Count == 1)
                    {
                        txt_seat.Text = "Seat(" + seat + ")";
                    }
                    else
                    {
                        txt_seat.Text = "Seat(" + cbl_seat.Items.Count + ")";
                    }
                    cb_seat.Checked = true;
                }
            }
            else
            {
                txt_seat.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void loadtype()
    {
        try
        {
            string type = string.Empty;
            cbl_type.Items.Clear();
            string[] typemode = { "Regular", "Transfer", "Lateral", "IrRegular" };
            for (int i = 0; i < 4; i++) //Transfer
            {
                cbl_type.Items.Add(new System.Web.UI.WebControls.ListItem(typemode[i], Convert.ToString(i + 1)));
            }
            if (cbl_type.Items.Count > 0)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                    type = Convert.ToString(cbl_type.Items[i].Text);
                }
                if (cbl_type.Items.Count == 1)
                {
                    txt_type.Text = "Type(" + type + ")";
                }
                else
                {
                    txt_type.Text = "Type(" + cbl_type.Items.Count + ")";
                }
                cb_type.Checked = true;
            }
        }
        catch
        {
        }
    }
    public void loadreligion()
    {
        try
        {
            string religion = string.Empty;
            cbl_religion.Items.Clear();
            string reliquery = "SELECT Distinct religion,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.religion AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(reliquery, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_religion.DataSource = ds;
                    cbl_religion.DataTextField = "TextVal";
                    cbl_religion.DataValueField = "religion";
                    cbl_religion.DataBind();
                    if (cbl_religion.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_religion.Items.Count; i++)
                        {
                            cbl_religion.Items[i].Selected = true;
                            religion = Convert.ToString(cbl_religion.Items[i].Text);
                        }
                        if (cbl_religion.Items.Count == 1)
                        {
                            txt_religion.Text = "" + religion + "";
                        }
                        else
                        {
                            txt_religion.Text = "Religion(" + cbl_religion.Items.Count + ")";
                        }
                        cb_religion.Checked = true;
                    }
                }
            }
            else
            {
                txt_religion.Text = "--Select--";
                cb_religion.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void loadcommunity()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct community,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.community  AND TextVal<>''AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_comm.DataSource = ds;
                    cbl_comm.DataTextField = "TextVal";
                    cbl_comm.DataValueField = "community";
                    cbl_comm.DataBind();
                    if (cbl_comm.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_comm.Items.Count; i++)
                        {
                            cbl_comm.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_comm.Items[i].Text);
                        }
                        if (cbl_comm.Items.Count == 1)
                        {
                            txt_comm.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_comm.Text = "Community(" + cbl_comm.Items.Count + ")";
                        }
                        cb_comm.Checked = true;
                    }
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_comm.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void loadallotedcommunity()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct allotcomm,T.TextVal  FROM applyn A,Registration R,TextValTable T WHERE A.app_no=R.App_No AND T.TextCode =A.allotcomm  AND TextVal<>''AND R.college_code ='" + ddlcollege.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_allotcomm.DataSource = ds;
                    cbl_allotcomm.DataTextField = "TextVal";
                    cbl_allotcomm.DataValueField = "allotcomm";
                    cbl_allotcomm.DataBind();
                    if (cbl_allotcomm.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                        {
                            cbl_allotcomm.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_allotcomm.Items[i].Text);
                        }
                        if (cbl_allotcomm.Items.Count == 1)
                        {
                            txt_Allotcomm.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_Allotcomm.Text = "Community(" + cbl_allotcomm.Items.Count + ")";
                        }
                        cb_allotcomm.Checked = true;
                    }
                }
            }
            else
            {
                txt_comm.Text = "--Select--";
                cb_allotcomm.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void loadTypeName()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct mastervalue,mastercode  FROM St_personalInfod s,CO_MasterValues c WHERE c.mastercode=s.typenamevalue";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_typename.DataSource = ds;
                    cbl_typename.DataTextField = "mastervalue";
                    cbl_typename.DataValueField = "mastercode";
                    cbl_typename.DataBind();
                    if (cbl_typename.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_typename.Items.Count; i++)
                        {
                            cbl_typename.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_typename.Items[i].Text);
                        }
                        if (cbl_typename.Items.Count == 1)
                        {
                            txt_Typename.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_Typename.Text = "TypeName(" + cbl_typename.Items.Count + ")";
                        }
                        cb_typename.Checked = true;
                    }
                }
            }
            else
            {
                txt_Typename.Text = "--Select--";
                cb_typename.Checked = false;
            }
        }
        catch
        {
        }
    }
    public void loadTypeSize()
    {
        try
        {
            string comm = string.Empty;
            string selq = "SELECT Distinct mastervalue,mastercode  FROM St_personalInfod s,CO_MasterValues c WHERE c.mastercode=s.typesizevalue";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selq, "Text");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_typesize.DataSource = ds;
                    cbl_typesize.DataTextField = "mastervalue";
                    cbl_typesize.DataValueField = "mastercode";
                    cbl_typesize.DataBind();
                    if (cbl_typesize.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_typesize.Items.Count; i++)
                        {
                            cbl_typesize.Items[i].Selected = true;
                            comm = Convert.ToString(cbl_typesize.Items[i].Text);
                        }
                        if (cbl_typesize.Items.Count == 1)
                        {
                            txt_typesize.Text = "" + comm + "";
                        }
                        else
                        {
                            txt_typesize.Text = "TypeSize(" + cbl_typesize.Items.Count + ")";
                        }
                        cb_typesize.Checked = true;
                    }
                }
            }
            else
            {
                txt_typesize.Text = "--Select--";
                cb_typesize.Checked = false;
            }
        }
        catch
        {
        }
    }
    private void loadState()//delsij
    {
        cbl_state.Items.Clear();
        try
        {
            string batchyear = rs.GetSelectedItemsValueAsString(cbl_batch);
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch); 
            string collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);

            string query = "  select distinct t.TextVal,p.uni_state from Registration R inner join applyn a on r.App_No = a.app_no inner join Stud_prev_details p on p.app_no = r.App_No and isnull(p.uni_state,0)<>0 inner join textvaltable t on  isnull(t.TextVal,'')<>'' and textcode=convert(varchar(100),isnull(p.uni_state,0)) and r.Batch_Year in('" + batchyear + "') and r.degree_code in('" + degreecode + "') and r.college_code in('" + collegecode + "')";
            DataSet dsBoardUniv = d2.select_method_wo_parameter(query, "Text");
            if (dsBoardUniv.Tables.Count > 0 && dsBoardUniv.Tables[0].Rows.Count > 0)
            {
                cbl_state.DataSource = dsBoardUniv;
                cbl_state.DataTextField = "TextVal";
                cbl_state.DataValueField = "uni_state";
                cbl_state.DataBind();
                for (int i = 0; i < cbl_state.Items.Count; i++)
                {
                    cbl_state.Items[i].Selected = true;
                }
                txtstate.Text = "State(" + cbl_state.Items.Count + ")";
                cb_states.Checked = true;
            }
        }
        catch { }
    }
    public void bindresidency()
    {
        string type = string.Empty;
        string[] residency = { "Not Required", "Campus Required" };
        for (int i = 0; i < 2; i++)
        {
            cbl_residency.Items.Add(new System.Web.UI.WebControls.ListItem(residency[i], Convert.ToString(i)));
        }
        if (cbl_residency.Items.Count > 0)
        {
            for (int i = 0; i < cbl_residency.Items.Count; i++)
            {
                cbl_residency.Items[i].Selected = true;
                type = Convert.ToString(cbl_residency.Items[i].Text);
            }
            if (cbl_residency.Items.Count == 1)
            {
                txt_resident.Text = "Residency(" + type + ")";
            }
            else
            {
                txt_resident.Text = "Residency(" + cbl_residency.Items.Count + ")";
            }
            cb_residency.Checked = true;
        }
    }
    public void bindsports()
    {
        string type = string.Empty;
        cbl_sport.Items.Clear();
        string qur = "select Distinct DistinctSport,T.TextVal  from applyn a,TextValTable t where convert(varchar(100), a.DistinctSport) =convert(varchar(100), t.TextCode) and a.college_code ='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_sport.DataSource = ds;
            cbl_sport.DataTextField = "TextVal";
            cbl_sport.DataValueField = "DistinctSport";
            cbl_sport.DataBind();
            cbl_sport.Items.Insert(0, new ListItem("IsSports", "DistinctSport"));
        }
        if (cbl_sport.Items.Count > 0)
        {
            for (int i = 0; i < cbl_sport.Items.Count; i++)
            {
                cbl_sport.Items[i].Selected = true;
                type = Convert.ToString(cbl_sport.Items[i].Text);
            }
            if (cbl_sport.Items.Count == 1)
            {
                txt_sports.Text = "Sports(" + type + ")";
            }
            else
            {
                txt_sports.Text = "Sports(" + cbl_sport.Items.Count + ")";
            }
            cb_sport.Checked = true;
        }
    }
    public void bindlanguage()
    {
        string type = string.Empty;
        cbl_language.Items.Clear();
        string qur = "select Distinct Part1Language,T.TextVal  from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no and s.Part1Language =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  t.TextVal not in('---Select---') ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_language.DataSource = ds;
            cbl_language.DataTextField = "TextVal";
            cbl_language.DataValueField = "Part1Language";
            cbl_language.DataBind();
        }
        if (cbl_language.Items.Count > 0)
        {
            for (int i = 0; i < cbl_language.Items.Count; i++)
            {
                cbl_language.Items[i].Selected = true;
                type = Convert.ToString(cbl_language.Items[i].Text);
            }
            if (cbl_language.Items.Count == 1)
            {
                txt_lang.Text = "Language(" + type + ")";
            }
            else
            {
                txt_lang.Text = "Language(" + cbl_language.Items.Count + ")";
            }
            cb_language.Checked = true;
        }
    }
    public void bindmothertongue()
    {
        string type = string.Empty;
        cbl_mothertongue.Items.Clear();
        string qur = "select Distinct mother_tongue ,T.TextVal  from applyn a,TextValTable t where  a.mother_tongue =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' ";
        ds.Clear();
        ds = d2.select_method_wo_parameter(qur, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            cbl_mothertongue.DataSource = ds;
            cbl_mothertongue.DataTextField = "TextVal";
            cbl_mothertongue.DataValueField = "mother_tongue";
            cbl_mothertongue.DataBind();
        }
        if (cbl_mothertongue.Items.Count > 0)
        {
            for (int i = 0; i < cbl_mothertongue.Items.Count; i++)
            {
                cbl_mothertongue.Items[i].Selected = true;
                type = Convert.ToString(cbl_mothertongue.Items[i].Text);
            }
            if (cbl_mothertongue.Items.Count == 1)
            {
                txt_mothertng.Text = "Mother Tongue(" + type + ")";
            }
            else
            {
                txt_mothertng.Text = "Mother Tongue(" + cbl_mothertongue.Items.Count + ")";
            }
            cb_mothertongue.Checked = true;
        }
    }
    public void bindphysicalchallaged()
    {
        string type = string.Empty;
        string[] physical = { "IsDisable", "Visually Challanged", "Physically Challanged", "Learning Disability", "Others" };
        string[] physical1 = { "isdisable", "visualhandy ", "handy", "islearningdis", "isdisabledisc" };
        for (int i = 0; i < 5; i++)
        {
            cbl_phychlg.Items.Add(new System.Web.UI.WebControls.ListItem(physical[i], Convert.ToString(physical1[i])));
        }
        if (cbl_phychlg.Items.Count > 0)
        {
            for (int i = 0; i < cbl_phychlg.Items.Count; i++)
            {
                cbl_phychlg.Items[i].Selected = true;
                type = Convert.ToString(cbl_phychlg.Items[i].Text);
            }
            if (cbl_phychlg.Items.Count == 1)
            {
                txt_phychallage.Text = "Physical Challanged(" + type + ")";
            }
            else
            {
                txt_phychallage.Text = "Physical Challanged(" + cbl_phychlg.Items.Count + ")";
            }
            cb_phychlg.Checked = true;
        }
    }
    public void LoadDisContinueReason()
    {
        try
        {

            cbl_reason.Items.Clear();
            cb_reason.Checked = false;
            txt_reason.Text = "---Select---";
            ds.Clear();

            string Query = "select distinct Reason from Discontinue where isnull(reason,'')<>'' group by Reason ";
            //string Query = " select distinct case when isnull(Reason,'')='' then 'Empty' else Reason end  as Reason from Discontinue";
            ds = d2.select_method_wo_parameter(Query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_reason.DataSource = ds;
                cbl_reason.DataTextField = "Reason";
                //cbl_reason.DataValueField = "Criteria_no";
                cbl_reason.DataBind();
                cbl_reason.Items.Insert(0, "Empty");
                if (cbl_reason.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_reason.Items.Count; i++)
                    {
                        cbl_reason.Items[i].Selected = true;
                    }
                    txt_reason.Text = "Reason(" + cbl_reason.Items.Count + ")";
                    cb_reason.Checked = true;
                }
            }

        }
        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode, "InvigilationSelection"); }
        catch
        {
        }


    }
    public void bindstatus()
    {
        string type = string.Empty;
        string[] statusname = { "Discontinue", "De-Bar", "Course Completed", "Prolong Absent" };//modified
        for (int i = 0; i < 4; i++)
        {
            cblstutype.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
        }
        for (int i = 0; i < 4; i++)
        {
            ddl_status.Items.Add(new System.Web.UI.WebControls.ListItem(statusname[i], Convert.ToString(i + 1)));
        }

        if (cblstutype.Items.Count > 0)
        {
            for (int i = 0; i < cblstutype.Items.Count; i++)
            {
                cblstutype.Items[i].Selected = true;
                type = Convert.ToString(cblstutype.Items[i].Text);
            }
            if (cblstutype.Items.Count == 1)
            {
                txt_stutype.Text = "Student Type(" + type + ")";
            }
            else
            {
                txt_stutype.Text = "Student Type(" + cblstutype.Items.Count + ")";
            }

        }
    }
    public void bindtransport()
    {
        string type = string.Empty;
        string[] bindtrans = { "Own Transport", "Institution Transport" };
        for (int i = 0; i < 2; i++)
        {
            cbl_transport.Items.Add(new System.Web.UI.WebControls.ListItem(bindtrans[i], Convert.ToString(i + 1)));
        }
        if (cbl_transport.Items.Count > 0)
        {
            for (int i = 0; i < cbl_transport.Items.Count; i++)
            {
                cbl_transport.Items[i].Selected = true;
                type = Convert.ToString(cbl_transport.Items[i].Text);
            }
            if (cbl_transport.Items.Count == 1)
            {
                txt_transport.Text = "Transport(" + type + ")";
            }
            else
            {
                txt_transport.Text = "Transport(" + cbl_transport.Items.Count + ")";
            }
        }
    }
 
    public void cb_stream_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_stream.Checked == true)
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    cbl_stream.Items[i].Selected = true;
                }
                txt_stream.Text = lbl_Stream.Text + "(" + (cbl_stream.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stream.Items.Count; i++)
                {
                    cbl_stream.Items[i].Selected = false;
                }
                txt_stream.Text = "--Select--";
            }
            edu_level();
            degree();
        }
        catch
        {
        }
    }
    public void cbl_stream_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_stream.Text = "--Select--";
            cb_stream.Checked = false;
            for (int i = 0; i < cbl_stream.Items.Count; i++)
            {
                if (cbl_stream.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            edu_level();
            degree();
            if (commcount == cbl_stream.Items.Count)
            {
                txt_stream.Text = lbl_Stream.Text + "(" + commcount.ToString() + ")";
                cb_stream.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_stream.Text = "--Select--";
            }
            else
            {
                txt_stream.Text = lbl_Stream.Text + "(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_graduation_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string deg = string.Empty;
            if (cb_graduation.Checked == true)
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = true;
                }
                txt_graduation.Text = "Edu Level(" + (cbl_graduation.Items.Count) + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                degree();
                bindbranch(deg);
                bindsem();
                bindsem1();
                BindSectionDetail();
            }
            else
            {
                for (int i = 0; i < cbl_graduation.Items.Count; i++)
                {
                    cbl_graduation.Items[i].Selected = false;
                }
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
        }
        catch
        {
        }
    }
    public void cbl_graduation_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            string deg = string.Empty;
            txt_graduation.Text = "--Select--";
            cb_graduation.Checked = false;
            for (int i = 0; i < cbl_graduation.Items.Count; i++)
            {
                if (cbl_graduation.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_graduation.Items.Count)
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                cb_graduation.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_graduation.Text = "--Select--";
                txt_graduation.Text = "--Select--";
                txt_degree.Text = "--Select--";
                cb_degree.Checked = false;
                cbl_degree.Items.Clear();
                txt_branch.Text = "--Select--";
                cb_branch.Checked = false;
                cbl_branch.Items.Clear();
                cb_sem.Checked = false;
                txt_sem.Text = "--Select--";
                cbl_sem.Items.Clear();
                cb_sec.Checked = false;
                txt_sec.Text = "--Select--";
                cbl_sec.Items.Clear();
            }
            else
            {
                txt_graduation.Text = "Edu Level(" + commcount.ToString() + ")";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            string build = cbl_degree.Items[i].Value.ToString();
                            if (deg == "")
                            {
                                deg = build;
                            }
                            else
                            {
                                deg = deg + "','" + build;
                            }
                        }
                    }
                }
                //degree();
                //bindbranch(deg);
                //bindsem();
                //BindSectionDetail();
            }
            degree();
            bindbranch(deg);
            bindsem();
            bindsem1();
            BindSectionDetail();
        }
        catch
        {
        }
    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_batch.Checked == true)
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cb_batch.Checked == true)
                    {
                        cbl_batch.Items[i].Selected = true;
                        txt_batch.Text = "Batch(" + (cbl_batch.Items.Count) + ")";
                        build1 = cbl_batch.Items[i].Value.ToString();
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
            }
            else
            {
                for (int i = 0; i < cbl_batch.Items.Count; i++)
                {
                    cbl_batch.Items[i].Selected = false;
                    txt_batch.Text = "--Select--";
                }
            }
            bindsem();
            bindsem1();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_batch.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_batch.Text = "--Select--";
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
            if (seatcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_batch.Text = "--Select--";
                cb_batch.Text = "--Select--";
            }
            else
            {
                txt_batch.Text = "Batch(" + seatcount.ToString() + ")";
            }
            bindsem();
            bindsem1();
            BindSectionDetail();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = string.Empty;
            string build1 = string.Empty;
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = lbl_degree.Text + "(" + (cbl_degree.Items.Count) + ")";
                        build1 = cbl_degree.Items[i].Value.ToString();
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
                bindbranch(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cb_branch.Checked = false;
                }
            }
            bindsem();
            BindSectionDetail();
            loadState();

        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_degree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_degree.Checked = false;
            string buildvalue = string.Empty;
            string build = string.Empty;
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
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
            bindbranch(buildvalue);
            if (seatcount == cbl_degree.Items.Count)
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = lbl_degree.Text + "(" + seatcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
            loadState();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_branch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_branch.Checked == true)
            {
                for (int i = 0; i < cbl_branch.Items.Count; i++)
                {
                    cbl_branch.Items[i].Selected = true;
                }
                txt_branch.Text = lbl_branch.Text + "(" + (cbl_branch.Items.Count) + ")";
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
            BindSectionDetail();
            bindsem1();
            loadState();
        }
        catch
        {
        }
    }
    public void cbl_branch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_branch.Text = "--Select--";
            cb_branch.Checked = false;
            for (int i = 0; i < cbl_branch.Items.Count; i++)
            {
                if (cbl_branch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_branch.Items.Count)
            {
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = lbl_branch.Text + "(" + commcount.ToString() + ")";
            }
            bindsem();
            BindSectionDetail();
            loadState();
            bindsem1();
        }
        catch
        {
        }
    }
    public void cb_sem_checkedchange(object sender, EventArgs e)
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
                txt_sem.Text = lbl_org_sem.Text + "(" + (cbl_sem.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
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
                txt_sem.Text = lbl_org_sem.Text + "(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_sec_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_sec.Checked == true)
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = true;
                }
                txt_sec.Text = "Section(" + (cbl_sec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sec.Items.Count; i++)
                {
                    cbl_sec.Items[i].Selected = false;
                }
                txt_sec.Text = "--Select--";
            }
        }
        catch
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
                }
            }
            if (commcount == cbl_sec.Items.Count)
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
                cb_sec.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_sec.Text = "--Select--";
            }
            else
            {
                txt_sec.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_stutype_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_stutype.Checked == true)
        {
            txt_stutype.Enabled = true;
            ddl_status.Enabled = true;

        }
        else
        {
            txt_stutype.Enabled = false;
            ddl_status.Enabled = false;

        }
    }
    public void cb_seatchk_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_seatchk.Checked == true)
        {
            txt_seat.Enabled = true;
        }
        else
        {
            txt_seat.Enabled = false;
        }
    }
    public void cb_typechk_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_typechk.Checked == true)
        {
            txt_type.Enabled = true;
        }
        else
        {
            txt_type.Enabled = false;
        }
    }
    public void cb_relichk_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_relichk.Checked == true)
        {
            txt_religion.Enabled = true;
        }
        else
        {
            txt_religion.Enabled = false;
        }
    }
    public void cb_commchk_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_commchk.Checked == true)
        {
            txt_comm.Enabled = true;
        }
        else
        {
            txt_comm.Enabled = false;
        }
    }
    public void chk_stutype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (chk_stutype.Checked == true)
            {
                for (int i = 0; i < cblstutype.Items.Count; i++)
                {
                    cblstutype.Items[i].Selected = true;
                }
                txt_stutype.Text = "Student Type(" + (cblstutype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cblstutype.Items.Count; i++)
                {
                    cblstutype.Items[i].Selected = false;
                }
                txt_stutype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cblstutype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_stutype.Text = "--Select--";
            chk_stutype.Checked = false;
            for (int i = 0; i < cblstutype.Items.Count; i++)
            {
                if (cblstutype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cblstutype.Items.Count)
            {
                txt_stutype.Text = "Student Type(" + commcount.ToString() + ")";
                chk_stutype.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_stutype.Text = "--Select--";
            }
            else
            {
                txt_stutype.Text = "Student Type(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_studtypechk_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_studtypechk.Checked == true)
        {
            txt_studtype.Enabled = true;
            div_report.Visible = false;
            rptprint.Visible = false;
            lblvalidation1.Visible = false;
            lbl_norec.Visible = false;
        }
        else
        {
            txt_studtype.Enabled = false;
        }
    }
    public void cb_studtype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_studtype.Checked == true)
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = true;
                }
                txt_studtype.Text = "Student Type(" + (cbl_studtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_studtype.Items.Count; i++)
                {
                    cbl_studtype.Items[i].Selected = false;
                }
                txt_studtype.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_studtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_studtype.Text = "--Select--";
            cb_studtype.Checked = false;
            for (int i = 0; i < cbl_studtype.Items.Count; i++)
            {
                if (cbl_studtype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_studtype.Items.Count)
            {
                txt_studtype.Text = "Student Type(" + commcount.ToString() + ")";
                cb_studtype.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_studtype.Text = "--Select--";
            }
            else
            {
                txt_studtype.Text = "Student Type(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_seat_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_seat.Checked == true)
            {
                for (int i = 0; i < cbl_seat.Items.Count; i++)
                {
                    cbl_seat.Items[i].Selected = true;
                }
                txt_seat.Text = "Seat(" + (cbl_seat.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_seat.Items.Count; i++)
                {
                    cbl_seat.Items[i].Selected = false;
                }
                txt_seat.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_seat_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_seat.Text = "--Select--";
            cb_seat.Checked = false;
            for (int i = 0; i < cbl_seat.Items.Count; i++)
            {
                if (cbl_seat.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_seat.Items.Count)
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
                cb_seat.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_seat.Text = "--Select--";
            }
            else
            {
                txt_seat.Text = "Seat(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_type_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_type.Checked == true)
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = true;
                }
                txt_type.Text = "Type(" + (cbl_type.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_type.Items.Count; i++)
                {
                    cbl_type.Items[i].Selected = false;
                }
                txt_type.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_type_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_type.Text = "--Select--";
            cb_type.Checked = false;
            for (int i = 0; i < cbl_type.Items.Count; i++)
            {
                if (cbl_type.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_type.Items.Count)
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
                cb_type.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_type.Text = "--Select--";
            }
            else
            {
                txt_type.Text = "Type(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_religion_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_religion.Checked == true)
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = true;
                }
                txt_religion.Text = "Religion(" + (cbl_religion.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_religion.Items.Count; i++)
                {
                    cbl_religion.Items[i].Selected = false;
                }
                txt_religion.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_religion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_religion.Text = "--Select--";
            cb_religion.Checked = false;
            for (int i = 0; i < cbl_religion.Items.Count; i++)
            {
                if (cbl_religion.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_religion.Items.Count)
            {
                txt_religion.Text = "Religion(" + commcount.ToString() + ")";
                cb_religion.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_religion.Text = "--Select--";
            }
            else
            {
                txt_religion.Text = "Religion(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_comm_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_comm.Checked == true)
            {
                for (int i = 0; i < cbl_comm.Items.Count; i++)
                {
                    cbl_comm.Items[i].Selected = true;
                }
                txt_comm.Text = "Community(" + (cbl_comm.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_comm.Items.Count; i++)
                {
                    cbl_comm.Items[i].Selected = false;
                }
                txt_comm.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_comm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_comm.Text = "--Select--";
            cb_comm.Checked = false;
            for (int i = 0; i < cbl_comm.Items.Count; i++)
            {
                if (cbl_comm.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_comm.Items.Count)
            {
                txt_comm.Text = "Community(" + commcount.ToString() + ")";
                cb_comm.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_comm.Text = "--Select--";
            }
            else
            {
                txt_comm.Text = "Community(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }

    public void cb_allotcomm_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_allotcomm.Checked == true)
            {
                for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                {
                    cbl_allotcomm.Items[i].Selected = true;
                }
                txt_Allotcomm.Text = "Community(" + (cbl_allotcomm.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
                {
                    cbl_allotcomm.Items[i].Selected = false;
                }
                txt_Allotcomm.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_allotcomm_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_Allotcomm.Text = "--Select--";
            cb_allotcomm.Checked = false;
            for (int i = 0; i < cbl_allotcomm.Items.Count; i++)
            {
                if (cbl_allotcomm.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_allotcomm.Items.Count)
            {
                txt_Allotcomm.Text = "Community(" + commcount.ToString() + ")";
                cb_allotcomm.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_Allotcomm.Text = "--Select--";
            }
            else
            {
                txt_Allotcomm.Text = "Community(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    public void cb_resident_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_resident.Checked == true)
        {
            txt_resident.Enabled = true;
        }
        else
        {
            txt_resident.Enabled = false;
        }
    }
    public void cb_residency_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_residency, cbl_residency, txt_resident, "Residency", "--Select--");
    }
    public void cbl_residency_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_residency, cbl_residency, txt_resident, "Residency", "--Select--");
    }
    public void cb_sports_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_sports.Checked == true)
        {
            txt_sports.Enabled = true;
        }
        else
        {
            txt_sports.Enabled = false;
        }
    }
    public void cb_sport_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_sport, cbl_sport, txt_sports, "Sports", "--Select--");
    }
    public void cbl_sport_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_sport, cbl_sport, txt_sports, "Sports", "--Select--");
    }
    public void cb_lang_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_lang.Checked == true)
        {
            txt_lang.Enabled = true;
        }
        else
        {
            txt_lang.Enabled = false;
        }
    }
    public void cb_language_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_language, cbl_language, txt_lang, "Language", "--Select--");
    }
    public void cbl_language_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_language, cbl_language, txt_lang, "Language", "--Select--");
    }
    public void cb_mothertng_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_mothertng.Checked == true)
        {
            txt_mothertng.Enabled = true;
        }
        else
        {
            txt_mothertng.Enabled = false;
        }
    }
    public void cb_mothertongue_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_mothertongue, cbl_mothertongue, txt_mothertng, "Mother Tongue", "--Select--");
    }
    public void cbl_mothertongue_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_mothertongue, cbl_mothertongue, txt_mothertng, "Mother Tongue", "--Select--");
    }
    public void cb_phychallange_CheckedChanged(object sender, EventArgs e)
    {
        //clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_phychallange.Checked == true)
        {
            txt_phychallage.Enabled = true;
        }
        else
        {
            txt_phychallage.Enabled = false;
        }
    }

    public void cb_board_CheckedChanged(object sender, EventArgs e)
    {
        //clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_board.Checked == true)
        {
            txtBoardUniv.Enabled = true;
            loadBoardUniv();
        }
        else
        {
            txtBoardUniv.Enabled = false;
        }

    }


    public void cb_unistate_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_state.Checked == true)
        {
            txtstate.Enabled = true;
        }
        else
        {
            txtstate.Enabled = false;
        }
    }

    public void cb_quota_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cbquotacheck.Checked == true)
        {
            txtQuota.Enabled = true;
        }
        else
        {
            txtQuota.Enabled = false;
        }
    }

    public void cb_phychlg_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged", "--Select--");
    }
    public void cbl_phychlg_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_phychlg, cbl_phychlg, txt_phychallage, "Physical Challaged", "--Select--");
    }
    public void cb_trans_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_trans.Checked == true)
        {
            txt_transport.Enabled = true;
        }
        else
        {
            txt_transport.Enabled = false;
        }
    }
    public void cb_transport_checkedchange(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_transport, cbl_transport, txt_transport, "Transport", "--Select--");
    }
    public void cbl_transport_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_transport, cbl_transport, txt_transport, "Transport", "--Select--");
    }
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = string.Empty;
            cb.Checked = false;
            for (sel = 0; sel < cbl.Items.Count; sel++)
            {
                if (cbl.Items[sel].Selected == true)
                {
                    count++;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
            }
            if (count > 0)
            {
                if (count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dipst + "(" + count + ")";
                }
                if (cbl.Items.Count == count)
                {
                    cb.Checked = true;
                }
            }
        }
        catch { }
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = string.Empty;
            txt.Text = deft;
            if (cb.Checked == true)
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = true;
                    name = Convert.ToString(cbl.Items[sel].Text);
                }
                if (cbl.Items.Count == 1)
                {
                    txt.Text = "" + name + "";
                }
                else
                {
                    txt.Text = dispst + "(" + cbl.Items.Count + ")";
                }
            }
            else
            {
                for (sel = 0; sel < cbl.Items.Count; sel++)
                {
                    cbl.Items[sel].Selected = false;
                }
                txt.Text = deft;
            }
        }
        catch { }
    }
    public void cb_Gender_CheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_gen, cbl_gen, txt_gen, "Gender", "--Select--");
    }
    public void cbl_gen_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_gen, cbl_gen, txt_gen, "Gender", "--Select--");
    }
    protected void cb_EnGender_CheckedChanged(object sender, EventArgs e)
    {
      //  clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_Gender.Checked == true)
        {
            txt_gen.Enabled = true;
            cb_gen.Checked = true;
            cb_Gender_CheckedChanged(sender, e);
        }
        else
        {
            txt_gen.Enabled = false;
        }
    }
   
    protected void cb_BoardUniv_checkedchange(object sender, EventArgs e)
    {

        {
            txtBoardUniv.Text = "Board";
            if (cb_BoardUniv.Checked)
            {
                for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                {
                    cbl_BoardUniv.Items[i].Selected = true;
                }
                txtBoardUniv.Text = "Board(" + cbl_BoardUniv.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
                {
                    cbl_BoardUniv.Items[i].Selected = false;
                }
            }
        }

    }
    protected void cbl_BoardUniv_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_BoardUniv.Checked = false;
        int cnt = 0;
        for (int i = 0; i < cbl_BoardUniv.Items.Count; i++)
        {
            if (cbl_BoardUniv.Items[i].Selected)
            {
                cnt++;
            }
        }
        if (cnt == cbl_BoardUniv.Items.Count)
        {
            cb_BoardUniv.Checked = true;
        }
        txtBoardUniv.Text = "Board(" + cnt + ")";


    }

    protected void cbl_reason_SelectedIndexChanged(object sender, EventArgs e)
    {
        rs.CallCheckboxListChange(cb_reason, cbl_reason, txt_reason, "Reason", "--Select--");
    }


    protected void cb_Disreaason_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (cb_Disreaason.Checked == true)
        {
            txt_reason.Enabled = true;
        }
        else
        {
            txt_reason.Enabled = false;
        }
    }
    protected void cb_reason_checkedchange(object sender, EventArgs e)
    {
        rs.CallCheckboxChange(cb_reason, cbl_reason, txt_reason, "Reason", "--Select--");


    }



    public void allotcommchk_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (allotcommchk.Checked == true)
        {
            txt_Allotcomm.Enabled = true;
        }
        else
        {
            txt_Allotcomm.Enabled = false;
        }
    }
    public void chk_typename_CheckedChanged(object sender, EventArgs e)
    {
        //clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chk_typename.Checked == true)
        {
            txt_Typename.Enabled = true;
        }
        else
        {
            txt_Typename.Enabled = false;
        }
    }

    public void cb_typesize_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_typesize.Checked == true)
            {
                for (int i = 0; i < cbl_typesize.Items.Count; i++)
                {
                    cbl_typesize.Items[i].Selected = true;
                }
                txt_typesize.Text = "Typesize(" + (cbl_typesize.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_typesize.Items.Count; i++)
                {
                    cbl_typesize.Items[i].Selected = false;
                }
                txt_typesize.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_typesize_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            cbl_typesize.Text = "--Select--";
            cb_typesize.Checked = false;
            for (int i = 0; i < cbl_typesize.Items.Count; i++)
            {
                if (cbl_typesize.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_typesize.Items.Count)
            {
                txt_typesize.Text = "Typesize(" + commcount.ToString() + ")";
                cb_typesize.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_typesize.Text = "--Select--";
            }
            else
            {
                txt_typesize.Text = "Typesize(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }




    public void chk_typesizename_CheckedChanged(object sender, EventArgs e)
    {
       // clear();
        div_report.Visible = false;
        rptprint.Visible = false;
        lblvalidation1.Visible = false;
        lbl_norec.Visible = false;
        if (chk_typesizename.Checked == true)
        {
            txt_typesize.Enabled = true;
        }
        else
        {
            txt_typesize.Enabled = false;
        }
    }
    public void cb_typename_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_typename.Checked == true)
            {
                for (int i = 0; i < cbl_typename.Items.Count; i++)
                {
                    cbl_typename.Items[i].Selected = true;
                }
                txt_Typename.Text = "Typename(" + (cbl_typename.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_typename.Items.Count; i++)
                {
                    cbl_typename.Items[i].Selected = false;
                }
                txt_Typename.Text = "--Select--";
            }
        }
        catch
        {
        }
    }
    public void cbl_typename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_Typename.Text = "--Select--";
            cb_typename.Checked = false;
            for (int i = 0; i < cbl_typename.Items.Count; i++)
            {
                if (cbl_typename.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount == cbl_typename.Items.Count)
            {
                txt_Typename.Text = "Typename(" + commcount.ToString() + ")";
                cb_typename.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_Typename.Text = "--Select--";
            }
            else
            {
                txt_Typename.Text = "Typename(" + commcount.ToString() + ")";
            }
        }
        catch
        {
        }
    }
    protected void cb_state_checkedchange(object sender, EventArgs e)
    {

        {
            txtstate.Text = "State";
            if (cb_states.Checked)
            {
                for (int i = 0; i < cbl_state.Items.Count; i++)
                {
                    cbl_state.Items[i].Selected = true;
                }
                txtstate.Text = "State(" + cbl_state.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl_state.Items.Count; i++)
                {
                    cbl_state.Items[i].Selected = false;
                }
            }
        }

    }
    protected void cbl_state_SelectedIndexChanged(object sender, EventArgs e)
    {
        cb_states.Checked = false;
        int cnt = 0;
        for (int i = 0; i < cbl_state.Items.Count; i++)
        {
            if (cbl_state.Items[i].Selected)
            {
                cnt++;
            }
        }
        if (cnt == cbl_state.Items.Count)
        {
            cb_states.Checked = true;
        }
        txtstate.Text = "State(" + cnt + ")";


    }
    protected void cbQuota_checkedchange(object sender, EventArgs e)
    {
        string ledgername = "";
        if (cbQuota.Checked == true)
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = true;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
            if (cblQuota.Items.Count == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + (cblQuota.Items.Count) + ")";
            }
            // txt_ledger.Text = "Ledger(" + (cbl_ledger.Items.Count) + ")";
        }
        else
        {
            for (int i = 0; i < cblQuota.Items.Count; i++)
            {
                cblQuota.Items[i].Selected = false;
            }
            txtQuota.Text = "--Select--";
        }

    }
    protected void cblQuota_SelectedIndexChange(object sender, EventArgs e)
    {
        string ledgername = "";
        txtQuota.Text = "--Select--";
        cbQuota.Checked = false;
        int commcount = 0;
        for (int i = 0; i < cblQuota.Items.Count; i++)
        {
            if (cblQuota.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                ledgername = Convert.ToString(cblQuota.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            //   txt_ledger.Text = "Ledger(" + commcount.ToString() + ")";
            if (commcount == cblQuota.Items.Count)
            {
                cbQuota.Checked = true;
            }
            if (commcount == 1)
            {
                txtQuota.Text = "" + ledgername + "";
            }
            else
            {
                txtQuota.Text = "Quota (" + commcount.ToString() + ")";
            }
        }

    }

    public void imgbtn_all_Click(object sender, EventArgs e)
    {
        poppernew.Visible = true;
        load();
        lb_column1.Items.Clear();
    }
    public void btndetailgo_Click(object sender, EventArgs e)
    {
        savecolumnoder = "1";

        lbl_headernamespd2.Visible = false;
        imgbtn_columsetting.Visible = true;
      
        div_report.Visible = false;

        fpspread1go1();
       
    }
    private void RollAndRegSettings()
    {
        try
        {
            DataSet dsl = new DataSet();
            string Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
            dsl = d2.select_method_wo_parameter(Master1, "text");
            Session["Rollflag"] = "0";
            Session["Regflag"] = "0";
            Session["Admission"] = "0";
            if (dsl.Tables[0].Rows.Count > 0)
            {
                for (int hf = 0; hf < dsl.Tables[0].Rows.Count; hf++)
                {
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Roll No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Rollflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Register No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Regflag"] = "1";
                    }
                    if (dsl.Tables[0].Rows[hf]["settings"].ToString() == "Admission No" && dsl.Tables[0].Rows[hf]["value"].ToString() == "1")
                    {
                        Session["Admission"] = "1";
                    }
                }
                settingValueRollAndReg(Convert.ToString(Session["Rollflag"]), Convert.ToString(Session["Regflag"]), Convert.ToString(Session["Admission"]));
            }
        }
        catch { }
    }
    private void settingValueRollAndReg(string rollvalue, string regvalue, string addmis)
    {
        // Tuple<byte, byte>
        string rollval = rollvalue;
        string regval = regvalue;
        string addVal = addmis;
        try
        {
            if (rollval != "" && regval != "")
            {
                if (rollval == "0" && regval == "0" && addVal == "0")
                    roll = 0;
                else if (rollval == "1" && regval == "1" && addVal == "1")
                    roll = 1;
                else if (rollval == "1" && regval == "0" && addVal == "0")
                    roll = 2;
                else if (rollval == "0" && regval == "1" && addVal == "0")
                    roll = 3;
                else if (rollval == "0" && regval == "0" && addVal == "1")
                    roll = 4;
                else if (rollval == "1" && regval == "1" && addVal == "0")
                    roll = 5;
                else if (rollval == "0" && regval == "1" && addVal == "1")
                    roll = 6;
                else if (rollval == "1" && regval == "0" && addVal == "1")
                    roll = 7;
            }
        }
        catch { }
        // return new Tuple<byte, byte>(roll,reg);

    }
    public void fpspread1go1()
    {
        try
        {
         

                RollAndRegSettings();
                Hashtable hscolumn = new Hashtable();
                Hashtable hscolumnvalue = new Hashtable();
                string orderStr = d2.GetFunction("select value from Master_Settings where settings='order_by'");
                if (orderStr == "0")
                {
                    if (roll == 0)
                        orderStr = " Order by roll_no,reg_no,roll_admit ";
                    else if (roll == 1)
                        orderStr = " Order by roll_no,reg_no,roll_admit ";
                    else if (roll == 2)
                        orderStr = " Order by roll_no ";
                    else if (roll == 3)
                        orderStr = " Order by reg_no ";
                    else if (roll == 4)
                        orderStr = " Order by roll_admit ";
                    else if (roll == 5)
                        orderStr = " Order by roll_no,reg_no ";
                    else if (roll == 6)
                        orderStr = " Order by reg_no,roll_admit ";
                    else if (roll == 7)
                        orderStr = " Order by roll_no,roll_admit ";
                }
                else
                {
                    if (orderStr == "0")
                        orderStr = "ORDER BY r.Roll_No";
                    else if (orderStr == "1")
                        orderStr = "ORDER BY r.Reg_No";
                    else if (orderStr == "2")
                        orderStr = "ORDER BY r.Stud_Name";
                    else if (orderStr == "0,1,2")
                        orderStr = "ORDER BY r.Roll_No,r.Reg_No,r.Stud_Name";
                    else if (orderStr == "0,1")
                        orderStr = "ORDER BY r.Roll_No,r.Reg_No";
                    else if (orderStr == "1,2")
                        orderStr = "ORDER BY r.Reg_No,r.Stud_Name";
                    else if (orderStr == "0,2")
                        orderStr = "ORDER BY r.Roll_No,r.Stud_Name";
                }


                #region datatable
                DataRow drrow = null;
                DataTable dtTTDisp = new DataTable();
                dtTTDisp.Columns.Add("SNo.");
                dtTTDisp.Columns.Add("Student Name");
                dtTTDisp.Columns.Add("Roll No");
                dtTTDisp.Columns.Add("Reg No");
                dtTTDisp.Columns.Add("Admission No");
                dtTTDisp.Columns.Add("Application No");
                dtTTDisp.Columns.Add("Applied Date");
                dtTTDisp.Columns.Add("Batch");
                dtTTDisp.Columns.Add("Degree");
                dtTTDisp.Columns.Add("Branch");
                dtTTDisp.Columns.Add("Semester");
                dtTTDisp.Columns.Add("Section");
                dtTTDisp.Columns.Add("SeatType");
                dtTTDisp.Columns.Add("Student Type");
                dtTTDisp.Columns.Add("HostelName");
                dtTTDisp.Columns.Add("Mode");
                dtTTDisp.Columns.Add("Boarding");
                dtTTDisp.Columns.Add("Vehicle Id");
                dtTTDisp.Columns.Add("Gender");
                dtTTDisp.Columns.Add("DOB");
                dtTTDisp.Columns.Add("Blood Group");
                dtTTDisp.Columns.Add("Father Name");
                dtTTDisp.Columns.Add("Father Income");
                dtTTDisp.Columns.Add("Father Occupation");
                dtTTDisp.Columns.Add("Father Mob No");
                dtTTDisp.Columns.Add("Father Email Id");
                dtTTDisp.Columns.Add("Mother Name");
                dtTTDisp.Columns.Add("Mother Income");
                dtTTDisp.Columns.Add("Mother Occupation");
                dtTTDisp.Columns.Add("Mother Mob No");
                dtTTDisp.Columns.Add("Mother Email Id");
                dtTTDisp.Columns.Add("Guardian Name");
                dtTTDisp.Columns.Add("Guardian Email Id");
                dtTTDisp.Columns.Add("Guardian Mob No");
                dtTTDisp.Columns.Add("Place Of Birth");
                dtTTDisp.Columns.Add("Adhaar Card No");
                dtTTDisp.Columns.Add("Voter ID");
                dtTTDisp.Columns.Add("Mother Tongue");
                dtTTDisp.Columns.Add("Religion");
                dtTTDisp.Columns.Add("Community");
                dtTTDisp.Columns.Add("Caste");
                dtTTDisp.Columns.Add("Sub Caste");
                dtTTDisp.Columns.Add("Citizen");
                dtTTDisp.Columns.Add("TamilOrginFromAndaman");
                dtTTDisp.Columns.Add("Ex-serviceman");
                dtTTDisp.Columns.Add("Rank");
                dtTTDisp.Columns.Add("Place");
                dtTTDisp.Columns.Add("Number");
                dtTTDisp.Columns.Add("IsDisable");
                dtTTDisp.Columns.Add("VisualHandy");
                dtTTDisp.Columns.Add("Residency");
                dtTTDisp.Columns.Add("Physically challange");
                dtTTDisp.Columns.Add("Learning Disability");
                dtTTDisp.Columns.Add("Other Disability");
                dtTTDisp.Columns.Add("Sports");
                dtTTDisp.Columns.Add("First Graduate");
                dtTTDisp.Columns.Add("MissionaryChild");
                dtTTDisp.Columns.Add("missionarydisc");
                dtTTDisp.Columns.Add("Hostel accommodation");
                dtTTDisp.Columns.Add("Blood Donor");
                dtTTDisp.Columns.Add("Reserved Caste");
                dtTTDisp.Columns.Add("Economic Backward");
                dtTTDisp.Columns.Add("Parents Old Student");
                dtTTDisp.Columns.Add("Driving License");
                dtTTDisp.Columns.Add("License No");
                dtTTDisp.Columns.Add("Tuition Fee Waiver");
                dtTTDisp.Columns.Add("Insurance");
                dtTTDisp.Columns.Add("Insurance Amount");
                dtTTDisp.Columns.Add("Insurance InsBy");
                dtTTDisp.Columns.Add("Insurance Nominee");
                dtTTDisp.Columns.Add("Insurance NominRelation");
                dtTTDisp.Columns.Add("Address");
                dtTTDisp.Columns.Add("Street");
                dtTTDisp.Columns.Add("City");
                dtTTDisp.Columns.Add("State");
                dtTTDisp.Columns.Add("Country");
                dtTTDisp.Columns.Add("PinCode");
                dtTTDisp.Columns.Add("Communication Address");
                dtTTDisp.Columns.Add("Communication Street");
                dtTTDisp.Columns.Add("Communication City");
                dtTTDisp.Columns.Add("Communication State");
                dtTTDisp.Columns.Add("Communication Country");
                dtTTDisp.Columns.Add("Communication PinCode");
                dtTTDisp.Columns.Add("Student Mobile");
                dtTTDisp.Columns.Add("Alternate Mob No");
                dtTTDisp.Columns.Add("Student EmailId");
                dtTTDisp.Columns.Add("Parent Phone No");
                dtTTDisp.Columns.Add("Curricular");
                dtTTDisp.Columns.Add("Institute Name");
                dtTTDisp.Columns.Add("Institute Address");
                dtTTDisp.Columns.Add("X Medium");
                dtTTDisp.Columns.Add("X11 Medium");
                dtTTDisp.Columns.Add("Part1 Language");
                dtTTDisp.Columns.Add("Part2 Language");
                dtTTDisp.Columns.Add("Percentage");
                dtTTDisp.Columns.Add("Secured Mark");
                dtTTDisp.Columns.Add("Total Mark");
                dtTTDisp.Columns.Add("Pass Month");
                dtTTDisp.Columns.Add("Pass Year");
                dtTTDisp.Columns.Add("Vocational Stream");
                dtTTDisp.Columns.Add("Mark Priority");
                dtTTDisp.Columns.Add("Cut Of Mark");
                dtTTDisp.Columns.Add("University Name");
                dtTTDisp.Columns.Add("Last TC No");
                dtTTDisp.Columns.Add("Last TC Date");
                dtTTDisp.Columns.Add("A/C No");
                dtTTDisp.Columns.Add("DebitCard No");
                dtTTDisp.Columns.Add("IFSCCode");
                dtTTDisp.Columns.Add("Bank Name");
                dtTTDisp.Columns.Add("Bank Branch");
                dtTTDisp.Columns.Add("Relative Name");
                dtTTDisp.Columns.Add("RelationShip");
                dtTTDisp.Columns.Add("Student/Staff");
                dtTTDisp.Columns.Add("Admission Date");
                dtTTDisp.Columns.Add("Enrollment Date");
                dtTTDisp.Columns.Add("Join Date");
                dtTTDisp.Columns.Add("CGPA");
                dtTTDisp.Columns.Add("No of arrear");
                dtTTDisp.Columns.Add("Refered By");
                dtTTDisp.Columns.Add("Dob[DD]");
                dtTTDisp.Columns.Add("Dob[MM]");
                dtTTDisp.Columns.Add("Dob[YYYY]");
                dtTTDisp.Columns.Add("Language");
                dtTTDisp.Columns.Add("Language Acronym");
                dtTTDisp.Columns.Add("Hall");
                dtTTDisp.Columns.Add("Hall Acronym");
                dtTTDisp.Columns.Add("Tc");
                int y = dtTTDisp.Columns.Count;
                drrow = dtTTDisp.NewRow();
                drrow["SNo."] = "SNo.";
                drrow["Student Name"] = "Student Name";
                drrow["Roll No"] = "Roll No";
                drrow["Reg No"] = "Reg No";
                drrow["Admission No"] = "Admission No";
                drrow["Application No"] = "Application No";
                drrow["Applied Date"] = "Applied Date";
                drrow["Batch"] = "Batch";
                drrow["Degree"] = "Degree";
                drrow["Branch"] = "Branch";
                drrow["Semester"] = "Semester";
                drrow["Section"] = "Section";
                drrow["SeatType"] = "SeatType";
                drrow["Student Type"] = "Student Type";
                drrow["HostelName"] = "HostelName";
                drrow["Mode"] = "Mode";
                drrow["Boarding"] = "Boarding";
                drrow["Vehicle Id"] = "Vehicle Id";
                drrow["Gender"] = "Gender";
                drrow["DOB"] = "DOB";
                drrow["Blood Group"] = "Blood Group";
                drrow["Father Name"] = "Father Name";
                drrow["Father Income"] = "Father Income";
                drrow["Father Occupation"] = "Father Occupation";
                drrow["Father Mob No"] = "Father Mob No";
                drrow["Father Email Id"] = "Father Email Id";
                drrow["Mother Name"] = "Mother Name";
                drrow["Mother Income"] = "Mother Income";
                drrow["Mother Occupation"] = "Mother Occupation";
                drrow["Mother Mob No"] = "Mother Mob No";
                drrow["Mother Email Id"] = "Mother Email Id";
                drrow["Guardian Name"] = "Guardian Name";
                drrow["Guardian Email Id"] = "Guardian Email Id";
                drrow["Guardian Mob No"] = "Guardian Mob No";
                drrow["Place Of Birth"] = "Place Of Birth";
                drrow["Adhaar Card No"] = "Adhaar Card No";
                drrow["Voter ID"] = "Voter ID";
                drrow["Mother Tongue"] = "Mother Tongue";
                drrow["Religion"] = "Religion";
                drrow["Community"] = "Community";
                drrow["Caste"] = "Caste";
                drrow["Sub Caste"] = "Sub Caste";
                drrow["Citizen"] = "Citizen";
                drrow["TamilOrginFromAndaman"] = "TamilOrginFromAndaman";
                drrow["Ex-serviceman"] = "Ex-serviceman";
                drrow["Rank"] = "Rank";
                drrow["Place"] = "Place";
                drrow["Number"] = "Number";
                drrow["IsDisable"] = "IsDisable";
                drrow["VisualHandy"] = "VisualHandy";
                drrow["Residency"] = "Residency";
                drrow["Physically challange"] = "Physically challange";
                drrow["Learning Disability"] = "Learning Disability";
                drrow["Other Disability"] = "Other Disability";
                drrow["Sports"] = "Sports";

                drrow["First Graduate"] = "First Graduate";
                drrow["MissionaryChild"] = "MissionaryChild";
                drrow["missionarydisc"] = "missionarydisc";
                drrow["Hostel accommodation"] = "Hostel accommodation";
                drrow["Blood Donor"] = "Blood Donor";
                drrow["Reserved Caste"] = "Reserved Caste";

                drrow["Economic Backward"] = "Economic Backward";
                drrow["Parents Old Student"] = "Parents Old Student";
                drrow["Driving License"] = "Driving License";
                drrow["License No"] = "License No";
                drrow["Tuition Fee Waiver"] = "Tuition Fee Waiver";
                drrow["Insurance"] = "Insurance";
                drrow["Insurance Amount"] = "Insurance Amount";
                drrow["Insurance InsBy"] = "Insurance InsBy";

                drrow["Insurance Nominee"] = "Insurance Nominee";
                drrow["Insurance NominRelation"] = "Insurance NominRelation";
                drrow["Address"] = "Address";
                drrow["Street"] = "Street";
                drrow["City"] = "City";
                drrow["State"] = "State";
                drrow["Country"] = "Country";
                drrow["PinCode"] = "PinCode";
                drrow["Communication Address"] = "Communication Address";
                drrow["Communication Street"] = "Communication Street";
                drrow["Communication City"] = "Communication City";
                drrow["Communication State"] = "Communication State";
                drrow["Communication Country"] = "Communication Country";
                drrow["Communication PinCode"] = "Communication PinCode";
                drrow["Student Mobile"] = "Student Mobile";
                drrow["Alternate Mob No"] = "Alternate Mob No";
                drrow["Student EmailId"] = "Student EmailId";
                drrow["Parent Phone No"] = "Parent Phone No";
                drrow["Curricular"] = "Curricular";
                drrow["Institute Name"] = "Institute Name";
                drrow["Institute Address"] = "Institute Address";
                drrow["X Medium"] = "X Medium";
                drrow["X11 Medium"] = "X11 Medium";
                drrow["Part1 Language"] = "Part1 Language";
                drrow["Part2 Language"] = "Part2 Language";
                drrow["Percentage"] = "Percentage";
                drrow["Secured Mark"] = "Secured Mark";
                drrow["Total Mark"] = "Total Mark";
                drrow["Pass Month"] = "Pass Month";
                drrow["Pass Year"] = "Pass Year";
                drrow["Vocational Stream"] = "Vocational Stream";
                drrow["Mark Priority"] = "Mark Priority";
                drrow["Cut Of Mark"] = "Cut Of Mark";
                drrow["University Name"] = "University Name";
                drrow["Last TC No"] = "Last TC No";
                drrow["Last TC Date"] = "Last TC Date";
                drrow["A/C No"] = "A/C No";
                drrow["DebitCard No"] = "DebitCard No";
                drrow["IFSCCode"] = "IFSCCode";
                drrow["Bank Name"] = "Bank Name";
                drrow["Bank Branch"] = "Bank Branch";
                drrow["Relative Name"] = "Relative Name";
                drrow["RelationShip"] = "RelationShip";
                drrow["Student/Staff"] = "Student/Staff";
                drrow["Admission Date"] = "Admission Date";
                drrow["Enrollment Date"] = "Enrollment Date";
                drrow["Join Date"] = "Join Date";
                drrow["CGPA"] = "CGPA";
                drrow["No of arrear"] = "No of arrear";
                drrow["Refered By"] = "Refered By";
                drrow["Dob[DD]"] = "Dob[DD]";
                drrow["Dob[MM]"] = "Dob[MM]";
                drrow["Dob[YYYY]"] = "Dob[YYYY]";
                drrow["Language"] = "Language";
                drrow["Language Acronym"] = "Language Acronym";
                drrow["Hall"] = "Hall";
                drrow["Hall Acronym"] = "Hall Acronym";
                drrow["Tc"] = "Tc";
                dtTTDisp.Rows.Add(drrow);

                #endregion

                div_report.Visible = true;

                string boards = string.Empty;
                string states = string.Empty;
                int val = 0;
                int count = 0;
                int count1 = 0;
                int i = 0;
                string header = string.Empty;
                string actval = string.Empty;
                string sectionvalue = string.Empty;
                string headertype1 = string.Empty;
                string headertype = string.Empty;



                string Batch_tagvalue = string.Empty;
                string dept_tagvalue = string.Empty;
                string course_tagvalue = string.Empty;
                string sem_tagvalue = string.Empty;
                string sec_tagvalue = string.Empty;
                string FromdateApplyn = string.Empty;
                string FromdateReg = string.Empty;
                string FromdateTransHostel = string.Empty;
                DateTime from = new DateTime();
                DateTime to = new DateTime();



                for (i = 0; i < cbl_batch.Items.Count; i++)
                {
                    if (cbl_batch.Items[i].Selected == true)
                    {
                        string addbatch1 = cbl_batch.Items[i].Value.ToString();
                        if (Batch_tagvalue == "")
                        {
                            Batch_tagvalue = addbatch1;
                        }
                        else
                        {
                            Batch_tagvalue = Batch_tagvalue + "'" + "," + "'" + addbatch1;
                        }
                    }
                }
                for (i = 0; i < cbl_branch.Items.Count; i++)
                {
                    if (cbl_branch.Items[i].Selected == true)
                    {
                        string adddeg1 = cbl_branch.Items[i].Value.ToString();
                        if (dept_tagvalue == "")
                        {
                            dept_tagvalue = adddeg1;
                        }
                        else
                        {
                            dept_tagvalue = dept_tagvalue + "'" + "," + "'" + adddeg1;
                        }
                    }
                }
                for (i = 0; i < cbl_sem.Items.Count; i++)
                {
                    if (cbl_sem.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sem.Items[i].Value.ToString();
                        if (sem_tagvalue == "")
                        {
                            sem_tagvalue = addsem1;
                        }
                        else
                        {
                            sem_tagvalue = sem_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }
                for (i = 0; i < cbl_sec.Items.Count; i++)
                {
                    if (cbl_sec.Items[i].Selected == true)
                    {
                        string addsem1 = cbl_sec.Items[i].Value.ToString();
                        if (sec_tagvalue == "")
                        {
                            sec_tagvalue = addsem1;
                        }
                        else
                        {
                            sec_tagvalue = sec_tagvalue + "'" + "," + "'" + addsem1;
                        }
                    }
                }


                string stu_type = string.Empty;
                string addqur = string.Empty;
                if (cb_studtypechk.Checked == true)
                {
                    for (i = 0; i < cbl_studtype.Items.Count; i++)
                    {
                        if (cbl_studtype.Items[i].Selected == true)
                        {
                            string stutype = cbl_studtype.Items[i].Value.ToString();
                            if (stu_type == "")
                            {
                                stu_type = stutype;
                            }
                            else
                            {
                                stu_type = stu_type + "'" + "," + "'" + stutype;
                            }
                        }
                    }
                    if (addqur == "")
                        addqur = " and r.Stud_Type in('" + stu_type + "')";

                }
                if (cb_seatchk.Checked == true)
                {

                    string mulseat = string.Empty;
                    for (i = 0; i < cbl_seat.Items.Count; i++)
                    {
                        if (cbl_seat.Items[i].Selected == true)
                        {
                            string seat = cbl_seat.Items[i].Value.ToString();
                            if (mulseat == "")
                            {
                                mulseat = seat;
                            }
                            else
                            {
                                mulseat = mulseat + "'" + "," + "'" + seat;
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        addqur = " and a.seattype in('" + mulseat + "')";
                    }
                    else
                    {
                        addqur = addqur + " and a.seattype in('" + mulseat + "')";
                    }

                }

                if (cb_typechk.Checked == true)
                {

                    string stumode = string.Empty;
                    for (i = 0; i < cbl_type.Items.Count; i++)
                    {
                        if (cbl_type.Items[i].Selected == true)
                        {
                            string stu_mode = cbl_type.Items[i].Value.ToString();
                            if (stumode == "")
                            {
                                stumode = stu_mode;
                            }
                            else
                            {
                                stumode = stumode + "'" + "," + "'" + stu_mode;
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        addqur = " and r.mode in('" + stumode + "')";
                    }
                    else
                    {
                        addqur = addqur + " and r.mode in('" + stumode + "')";
                    }

                }

                if (cb_relichk.Checked == true)
                {

                    string mul_religion = string.Empty;
                    for (i = 0; i < cbl_religion.Items.Count; i++)
                    {
                        if (cbl_religion.Items[i].Selected == true)
                        {
                            string religion = cbl_religion.Items[i].Value.ToString();
                            if (mul_religion == "")
                            {
                                mul_religion = religion;
                            }
                            else
                            {
                                mul_religion = mul_religion + "'" + "," + "'" + religion;
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        addqur = " and a.religion in('" + mul_religion + "')";
                    }
                    else
                    {
                        addqur = addqur + " and a.religion in('" + mul_religion + "')";
                    }

                }
                if (cb_commchk.Checked == true)
                {

                    string mul_communiti = string.Empty;
                    for (i = 0; i < cbl_comm.Items.Count; i++)
                    {
                        if (cbl_comm.Items[i].Selected == true)
                        {
                            string communiti = cbl_comm.Items[i].Value.ToString();
                            if (mul_communiti == "")
                            {
                                mul_communiti = communiti;
                            }
                            else
                            {
                                mul_communiti = mul_communiti + "'" + "," + "'" + communiti;
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        addqur = " and a.community in('" + mul_communiti + "')";
                    }
                    else
                    {
                        addqur = addqur + " and a.community in('" + mul_communiti + "')";
                    }

                }


                if (cb_resident.Checked == true)
                {

                    string mul_residency = string.Empty;
                    for (i = 0; i < cbl_residency.Items.Count; i++)
                    {
                        if (cbl_residency.Items[i].Selected == true)
                        {
                            string residency = cbl_residency.Items[i].Value.ToString();
                            if (mul_residency == "")
                            {
                                mul_residency = residency;
                            }
                            else
                            {
                                mul_residency = mul_residency + "'" + "," + "'" + residency;
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        addqur = " and CampusReq in('" + mul_residency + "')";
                    }
                    else
                    {
                        addqur = addqur + " and CampusReq in('" + mul_residency + "')";
                    }

                }

                if (cb_sports.Checked == true)
                {

                    string mul_sports = string.Empty;
                    for (i = 0; i < cbl_sport.Items.Count; i++)
                    {
                        if (cbl_sport.Items[i].Selected == true)
                        {
                            string sports = cbl_sport.Items[i].Value.ToString();
                            if (sports != "DistinctSport")
                            {
                                if (mul_sports == "")
                                {
                                    mul_sports = sports;
                                }
                                else
                                {
                                    mul_sports = mul_sports + "'" + "," + "'" + sports;
                                }
                            }
                        }
                    }
                    if (addqur == "")
                    {
                        // if (mul_sports="DistinctSport")

                        addqur = " and DistinctSport in('" + mul_sports + "')";
                    }
                    else
                    {
                        addqur = addqur + " and DistinctSport in('" + mul_sports + "')";
                    }

                }

                if (cb_lang.Checked == true)
                {

                    string mul_partlan = string.Empty;
                    for (i = 0; i < cbl_language.Items.Count; i++)
                    {
                        if (cbl_language.Items[i].Selected == true)
                        {
                            string partlan = cbl_language.Items[i].Value.ToString();
                            if (mul_partlan == "")
                            {
                                mul_partlan = partlan;
                            }
                            else
                            {
                                mul_partlan = mul_partlan + "'" + "," + "'" + partlan;
                            }
                        }
                    }
                    mul_partlan = d2.GetFunction("select  Part1Language  from applyn a,Stud_prev_details s,TextValTable t where a.app_no =s.app_no and s.Part1Language =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal in('" + mul_partlan + "')  ");
                    if (mul_partlan != "0")
                    {
                        if (addqur == "")
                        {
                            addqur = " and Part1Language in('" + mul_partlan + "')";
                        }
                        else
                        {
                            addqur = addqur + " and Part1Language in('" + mul_partlan + "')";
                        }
                    }

                }

                if (cb_mothertng.Checked == true)
                {

                    string mul_montun = string.Empty;
                    for (i = 0; i < cbl_mothertongue.Items.Count; i++)
                    {
                        if (cbl_mothertongue.Items[i].Selected == true)
                        {
                            string montun = cbl_mothertongue.Items[i].Value.ToString();
                            if (mul_montun == "")
                            {
                                mul_montun = montun;
                            }
                            else
                            {
                                mul_montun = mul_montun + "'" + "," + "'" + montun;
                            }
                        }
                    }
                    mul_montun = d2.GetFunction("select  mother_tongue  from applyn a,TextValTable t where  a.mother_tongue =t.TextCode and a.college_code='" + ddlcollege.SelectedItem.Value + "' and  T.TextVal in('" + mul_montun + "')");
                    if (mul_montun != "0")
                    {
                        if (addqur == "")
                        {
                            addqur = " and mother_tongue in('" + mul_montun + "')";
                        }
                        else
                        {
                            addqur = addqur + " and mother_tongue in('" + mul_montun + "')";
                        }
                    }

                }

                if (cb_stutype.Checked == true)
                    headertype1 = Convert.ToString(ddl_status.SelectedItem.Text);
                string BoardFilter = string.Empty;
                string StateFilter = string.Empty;
                //if (cb_board.Checked == true)//delsij
                //{
                //    string board = rs.GetSelectedItemsValueAsString(cbl_BoardUniv);
                //    //boards = " And p.university_code in('" + board + "')";
                //    //BoardFilter = " And s.university_code in('" + board + "')";
                //   // boards = " And p.course_code in('" + board + "')";
                //    BoardFilter = " And s.course_code in('" + board + "')";
                //    headertype = "Board";
                //}
                if (cb_state.Checked == true)
                {
                    string state = rs.GetSelectedItemsValueAsString(cbl_state);
                    states = " And p.uni_state in('" + state + "') ";
                    StateFilter = " And s.uni_state in('" + state + "')";
                    // headertype = "State";
                }
                if (sec_tagvalue != "")
                {
                    sectionvalue = " AND ISNULL( r.Sections,'') in('','" + sec_tagvalue + "')";
                    //sectionvalue =string.Empty;
                }
                else
                {
                    sectionvalue = string.Empty;
                }
                #region

                string name = header;

                if (headertype == "Stud_Type" || headertype == "seattype" || headertype == "mode" || headertype == "religion" || headertype == "community" || headertype == "Transport" || headertype == "course_code" || headertype == "allotcomm" || headertype == "typenamevalue" || headertype == "typesizevalue")
                {
                    val = 13;
                    if (headertype == "Stud_Type")
                    {
                        addqur = " and r.Stud_Type='" + header + "'";
                    }
                    if (headertype == "course_code")//abarna
                    {

                        header = d2.GetFunction("SELECT course_code FROM stud_prev_details A,TextValTable T WHERE T.TextCode =A.course_code AND   T.TextVal='" + header + "' and T.college_code ='" + collegecode1 + "' ");
                        addqur = " and s.course_code='" + header + "'";
                    }
                    if (headertype == "seattype")
                    {
                        header = d2.GetFunction("SELECT seattype FROM applyn A,TextValTable T WHERE T.TextCode =A.seattype AND a.college_code ='" + collegecode1 + "' and T.TextVal='" + header + "' ");
                        addqur = " and a.seattype='" + header + "'";
                    }
                    if (headertype == "mode")
                    {
                        headertype = "Type";
                        if (header == "Regular")
                        {
                            header = "1";
                        }
                        else if (header == "Lateral")
                        {
                            header = "3";
                        }
                        else if (header == "Transfer")
                        {
                            header = "2";
                        }
                        else if (header == "IrRegular")
                        {
                            header = "4";
                        }

                    }

                }




                if (headertype1 == "De-Bar")
                {
                    header = string.Empty;
                    val = 10;
                }
                if (headertype1 == "Discontinue")
                {
                    header = string.Empty;
                    val = 11;
                }
                if (headertype1 == "Course Completed")
                {
                    header = string.Empty;
                    val = 12;
                }
                if (headertype1 == "Prolong Absent")
                {
                    header = string.Empty;
                    val = 14;
                }

                if (headertype == "Board" || headertype == "State")
                    val = 13;
                string queryadd = string.Empty;
                if (headertype == "residency" || headertype == "DistinctSport" || headertype == "Part1Language" || headertype == "mother_tongue" || headertype == "PhysicalChallanged")
                {
                    val = 13;
                    if (headertype == "residency")
                    {
                        if (header.Trim() == "Campus Required")
                        {
                            header = "1";
                        }
                        else
                        {
                            header = "0";
                        }
                        queryadd = " and CampusReq='" + header + "'";
                    }
                    if (headertype == "DistinctSport")
                    {
                        if (header.Trim() == "IsSports")
                        {
                            queryadd = " and DistinctSport IS NOT NULL and DistinctSport <>'0'";
                        }
                        else
                        {
                            header = d2.GetFunction("SELECT DistinctSport FROM applyn A,TextValTable T WHERE T.TextCode =A.DistinctSport AND a.college_code ='" + ddlcollege.SelectedItem.Value + "' and T.TextVal='" + header + "' ");
                            queryadd = " and DistinctSport='" + header + "'";
                        }
                    }

                    if (headertype == "PhysicalChallanged")
                    {
                        if (header.Trim() == "IsDisable")
                        {
                            queryadd = " and  isdisable ='" + "True" + "'";
                        }
                        else if (header.Trim() == "Visually Challanged")
                        {
                            queryadd = " and  visualhandy ='" + 1 + "'";
                        }
                        else if (header.Trim() == "Physically Challanged")
                        {
                            queryadd = " and  handy ='" + 1 + "'";
                        }
                        else if (header.Trim() == "Learning Disability")
                        {
                            queryadd = " and  islearningdis ='" + "True" + "'";
                        }
                        else
                        {
                            queryadd = " and  isdisabledisc<>''";
                        }
                    }
                }

                #endregion

                loadlcolumns();
                string query = string.Empty;
                columnname = string.Empty;
                columnname1 = string.Empty;
                selectcolumnload();
                if (columnname != "")
                {
                    columnname = "," + columnname;
                }
                if (columnname1 != "")
                {
                    columnname1 = "," + columnname1;
                }
                if (val == 1 || val == 125 || val == 126)
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No
                    query = "select  distinct  r.Roll_No,r.Stud_Name,r.Batch_Year,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " " + orderStr + "  ";//order by r.Roll_No,r.Stud_Name,r.Reg_No

                }


                else if (val == 10)
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + "  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.Exam_Flag='DEBAR' " + FromdateReg + " " + orderStr + " ";// order by r.Roll_No,r.Stud_Name,r.Reg_No
                    query = "select  distinct  r.Roll_No,r.Batch_Year,r.Current_Semester,r.Stud_Name,r.Reg_No,r.degree_code,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + addqur + " and r.Exam_Flag='DEBAR' " + FromdateReg + " " + orderStr + " ";
                }
                else if (val == 11)
                {
                    ////query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' " + FromdateReg + " " + orderStr + " ";//  order by r.Roll_No,r.Stud_Name,r.Reg_No
                    //query = "select  distinct  r.Roll_No,r.Batch_Year,r.Current_Semester,r.Stud_Name,r.Reg_No,r.degree_code,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' " + FromdateReg + " " + orderStr + " ";

                    query = "select  distinct  r.Roll_No,r.Batch_Year,r.Current_Semester,r.Stud_Name,r.Reg_No,r.degree_code,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C,Stud_prev_details s where s.app_no=r.App_No and  r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + "  " + addqur + "  " + StateFilter + " and r.DelFlag<>'0' " + FromdateReg + " " + orderStr + " ";

                }
                else if (val == 12)
                {
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.cc=1 " + FromdateReg + " " + orderStr + " ";// order by r.Roll_No,r.Stud_Name,r.Reg_No
                    query = "select  distinct  r.Roll_No,r.Batch_Year,r.Stud_Name,r.Current_Semester,r.degree_code,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + "  " + addqur + "  " + StateFilter + " and r.cc=1 " + FromdateReg + " " + orderStr + " ";

                }

                else if (val == 14)
                {
                    query = "select  distinct StuPer_Id,(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue,case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = t.TextCode)   end as DistinctSport,case when CampusReq='0' then 'No' else 'Yes' end as CampusReq,case when handy='0' then 'No' else 'Yes' end as handy,case when islearningdis='0' then 'No' else 'Yes' end as islearningdis,isdisabledisc,case when isdisable='0' then 'No' else 'Yes' end as isdisable ,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no,a.missionarydisc,a.MissionaryChild,a.parent_phnoc,a.parent_pincodec,a.Student_Mobile,(Select textval FROM textvaltable T WHERE Countryc = t.TextCode) Countryc,(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec,a.Cityc,a.Streetc,a.parent_addressC,(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular,(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen,a.visualhandy,a.first_graduate,(Select textval FROM textvaltable T WHERE TamilOrginFromAndaman = t.TextCode) TamilOrginFromAndaman ,(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype,(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu,(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste,(Select textval FROM textvaltable T WHERE community = t.TextCode) community,(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion,a.parent_name,r.Reg_No ,r.Roll_No,r.Stud_Name,a.Batch_Year,a.Current_Semester,c.Course_Name,Dt.Dept_Name,isnull( r.Sections,'') as Sections,CONVERT(VARCHAR(11),dob,103) as dob  from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'   and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "')  " + addqur + "  " + StateFilter + " and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " and r.DelFlag<>'0' and ProlongAbsend<>'0' " + orderStr + "  ";
                }


                else if (val == 13)
                {
                    string leftwaiting = string.Empty;



                    if (cbl_studtype.SelectedIndex == 1)
                    {
                        leftwaiting = " and admission_status='1' and DelFlag='0' and Exam_Flag='OK' and CC='False' ";
                    }
                    else if (cbl_studtype.SelectedIndex == 2)
                    {
                        leftwaiting = " and DelFlag<>'0'";
                    }
                    else if (cbl_studtype.SelectedIndex == 3)
                    {
                        leftwaiting = " and Exam_Flag='DEBAR' ";
                    }
                    else if (cbl_studtype.SelectedIndex == 4)
                    {
                        leftwaiting = " and CC='True' ";
                    }
                    //query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + " from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";
                    if (chk_typename.Checked == true || chk_typesizename.Checked == true)
                    {
                        query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C,St_personalInfod st where st.appno=r.app_no and a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";
                    }
                    else
                    {

                        query = "  select   distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,degree d,Stud_prev_details s, applyn a ,Department dt,Course C where a.app_no =s.app_no and r.App_No =a.app_no and isconfirm ='1'  and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "'  and d.Degree_Code =r.degree_code  and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  " + sectionvalue + " " + queryadd + " " + leftwaiting + "  " + addqur + " " + FromdateReg + "  " + BoardFilter + " " + StateFilter + "   " + orderStr + " ";
                    }

                }

                else
                {

                    columnname1 = columnname1.Replace("a.stud_type", "r.stud_type");
                    columnname1 = columnname1.Replace("'' roll_admit", "roll_admit");
                    columnname1 = columnname1.Replace("a.Current_Semester", "r.Current_Semester");//Rajkumar on 9-6-2018
                    columnname1 = columnname1.Replace("''Sections", "sections");
                    //query = "select  distinct  r.Roll_No,r.Stud_Name,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,a.app_no" + columnname1 + " from Registration r,applyn a,degree d,Department dt,Course C where r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') and r.Batch_Year in('" + Batch_tagvalue + "') and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + FromdateReg + " " + orderStr + " ";//columnname 30.07.16
                    // order by r.Roll_No,r.Stud_Name,r.Reg_No
                    query = "select  distinct  r.Roll_No,r.Batch_Year,r.Stud_Name,r.degree_code,r.Reg_No,a.Admission_Status,r.Exam_Flag,r.CC,r.DelFlag,r.Current_Semester,a.app_no" + columnname1 + ",case when direct_refer='0'  then 'Direct'   when direct_refer='1'  then 'Staff'   when direct_refer='2'  then 'Others'  when direct_refer='3'  then 'Student' end referby,case when a.direct_refer=1 then (select appl_name from staff_appl_master sa,staffmaster s where sa.appl_no=s.appl_no and convert(varchar(20),s.staff_code)=a.refer_stcode) when direct_refer=3 then (select Stud_Name from applyn where convert(varchar(20),App_No)=a.refer_stcode) when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_name)) else '' end refer_stcode,case when direct_refer=2 then ((select textval from textvaltable where convert(varchar,TextCode)= refer_agent)) else '' end as refer_agent from Registration r,applyn a,degree d,Department dt,Course C  where  r.App_No=a.app_no and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and admission_status ='1' and CC='0' and DelFlag='0' and Exam_Flag<>'DEBAR' and r.degree_code in('" + dept_tagvalue + "') " + addqur + "  " + StateFilter + " and r.Batch_Year in('" + Batch_tagvalue + "')  ";
                    //string attme = "";
                    //if (rdb_cumm.Checked == true)
                    //{
                    //     attme = "m.attempts<='1'";
                    //}
                    //if (chksingle.Checked == true)
                    //                    {
                    //                        attme = "m.attempts='2'";
                    //                        }
                    //else if (chkmulti.Checked == true)
                    //                    {
                    //                        attme = "m.attempts>'2'";
                    //                    }
                    //query = query + "and " + attme + " ";
                    query = query + "and  r.Current_Semester in('" + sem_tagvalue + "') " + sectionvalue + " " + FromdateReg + " " + orderStr + "";

                }



                query = query + " sELECT LastTCNo,convert(varchar(10),LastTCDate,103)LastTCDate,instaddress,(Select textval FROM textvaltable T WHERE Xmedium = t.TextCode) Xmedium,(Select textval FROM textvaltable T WHERE medium = CONVERT(nvarchar(20),t.TextCode)) medium,percentage,securedmark,totalmark,passyear,passmonth,case when Vocational_stream='0' then 'No' else 'Yes' end as Vocational_stream,markPriority,Cut_Of_Mark ,a.App_No,us.textval as uni_state,type_semester,university_code,ISNULL(pt.TExtVal,'') Part2Language,(Select textval FROM textvaltable T WHERE Part1Language = t.TextCode) Part1Language,ISNULL(C.TExtVal,'') Course,ISNULL(U.TextVal,'') University,ISNULL(Institute_Name,'') Institute_Name,ISNULL(PassMonth,'') PassMonth,ISNULL(PassYear,'') PassYear FROM Stud_prev_details P INNER JOIN applyn A ON P.app_no = A.app_no LEFT JOIN TextValTable C ON C.TextCode = P.course_code  LEFT JOIN TextValTable U ON U.TextCode = P.university_code LEFT JOIN TextValTable pa ON pa.TextCode = P.Part1Language LEFT JOIN TextValTable pt ON pt.TextCode = P.Part2Language LEFT JOIN TextValTable us ON CONVERT(nvarchar(20),us.TextCode) = P.uni_state Where p.app_no = a.app_no  and a.Batch_Year in('" + Batch_tagvalue + "') and IsConfirm='1' and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'" + boards + "" + states + " and isnull(markPriority,1)=1";//and a.degree_code in('" + dept_tagvalue + "')"; us.TextCode varchar change 11.09.2018
                query = query + "select * from StudCertDetails_New s,applyn a where a.App_No=s.App_No  and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "') and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + "select Branch as BankBranch,* from studbankdet s,applyn a where a.App_No=s.App_No   and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + "select * from stud_relation s,applyn a where a.App_No=s.application_no   and a.degree_code in('" + dept_tagvalue + "') and a.Batch_Year in('" + Batch_tagvalue + "')  and  a.Current_Semester in('" + sem_tagvalue + "') and a.college_code='" + ddlcollege.SelectedItem.Value + "'  ";
                query = query + " select HostelName,APP_No from HT_HostelRegistration hr,HM_HostelMaster hm where hr.HostelMasterFK=hm.HostelMasterPK ";
                query = query + "  select COUNT (subject_no)as noofarrear,m.roll_no,a.app_no from mark_entry m , applyn a,Registration r  where r.App_No=a.app_no and  r.Roll_No=m.roll_no and subject_no not in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and m.result='pass') group by m.roll_no,a.app_no";
                query = query + " select roll_no,acronym,subject_name from subjectchooser c,subject s ,sub_sem u where c.subject_no = s.subject_no and s.subType_no = u.subType_no and subject_type = 'Foundation Course - I' and roll_no in (select roll_no from Registration r where  r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.Current_Semester in('" + sem_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' )  and s.subject_name not like 'Tamil%'";//and semester = '" + ddlSemYr.SelectedItem.Text.ToString() + "'
                query = query + " select r.app_no,r.Current_Semester,r.degree_code,r.stud_name,R.Batch_year,course_name+'-'+dept_name degree,isnull(r.Sections,'') as Sections,(select isnull(Building_acronym,'') from HT_HostelRegistration s,Building_Master b where s.BuildingFK = b.Code and s.APP_No = r.App_No and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0) as hall,(select isnull(building_description ,'') from HT_HostelRegistration s,Building_Master b where s.BuildingFK = b.Code and s.APP_No = r.App_No and ISNULL(IsVacated,0)=0 and ISNULL(IsDiscontinued,0)=0 and ISNULL(IsSuspend,0)=0) as hallname,(select textval from textvaltable t where t.TextCode = a.religion) religion,(select textval from textvaltable t where t.TextCode = a.community ) community,(select textval from textvaltable t where t.TextCode = a.caste) caste, r.roll_no,a.app_no,a.religion as religioncode,a.community as communitycode,a.sex,1 TotalStrength from Registration r,applyn a,Degree g,course c,department d where r.App_No = a.app_no and r.degree_code = g.Degree_Code and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code  and r.batch_year in( '" + Batch_tagvalue + "') and r.degree_code in ('" + dept_tagvalue + "') and r.college_code='" + ddlcollege.SelectedItem.Value + "' and r.Current_Semester in('" + sem_tagvalue + "') and r.cc=0 and r.DelFlag=0 and r.Exam_Flag<>'debar' ";
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");
                string latemode = "1";
                string noarrear = string.Empty;

                if (query == "")
                {
                    gview.Visible = false;
                    lblerror.Visible = true;
                    lblerror.Text = "Kindly Select All List ";
                    div_report.Visible = false;
                    lblvalidation1.Text = string.Empty;
                    return;
                }
                else
                {
                    if (query != "")
                    {
                        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count == 0)
                        {

                            gview.Visible = false;
                            lblerror.Visible = true;
                            lblerror.Text = "No Records Found";
                            div_report.Visible = false;
                            lbl_headernamespd2.Visible = false;
                            lblvalidation1.Text = string.Empty;
                            return;
                        }
                        else
                        {
                            lblerror.Visible = false;
                            lbl_err_stud.Visible = false;

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                div_report.Visible = true;
                                lbl_headernamespd2.Visible = true;
                                if (name == "All")
                                {
                                    lbl_headernamespd2.Text = name;
                                }
                                else
                                {
                                    lbl_headernamespd2.Text = headertype + "-" + name;
                                }

                                // Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                                // string
                                int cc = 2;
                                int j = 0;
                                //loadlcolumns();
                                DataSet dss = new DataSet();
                                string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
                                string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and user_code='" + usercode + "' ";
                                dss.Clear();
                                dss = d2.select_method_wo_parameter(selcol1, "Text");
                                if (dss.Tables.Count > 0)
                                {
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        for (int c = 0; c < dss.Tables[0].Rows.Count; c++)
                                        {
                                            string value = Convert.ToString(dss.Tables[0].Rows[c]["LinkValue"]);
                                            if (value != "")
                                            {
                                                string[] valuesplit = value.Split(',');
                                                if (valuesplit.Length > 0)
                                                {
                                                    for (int k = 0; k < valuesplit.Length; k++)
                                                    {
                                                        cc++;
                                                        colval = Convert.ToString(valuesplit[k]);
                                                        loadtext();
                                                        hscolumn.Add(colval, loadval);
                                                        hscolumnvalue.Add(loadval, printval);



                                                        //if (Convert.ToInt32(colval) > 134)
                                                        //{
                                                        //    Fpspread2.Sheets[0].ColumnHeader.Cells[0, cc].Note = "Certificate";
                                                        //}
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        imgdiv2.Visible = true;
                                        lbl_alert.Text = "No Records Found";
                                        gview.Visible = false;
                                        div_report.Visible = false;

                                        lbl_headernamespd2.Visible = false;
                                        lblvalidation1.Text = string.Empty;
                                        return;
                                    }
                                }
                                else
                                {
                                    imgdiv2.Visible = true;
                                    lbl_alert.Text = "Set Column Order";
                                    gview.Visible = false;
                                    div_report.Visible = false;

                                    lbl_headernamespd2.Visible = false;
                                    lblvalidation1.Text = string.Empty;
                                    return;
                                }
                                if (dtTTDisp.Columns.Count > 0)
                                {
                                    for (int im = 1; im < dtTTDisp.Columns.Count; im++)
                                    {
                                        string coluname = dtTTDisp.Columns[im].ToString();
                                        if (hscolumn.ContainsValue(dtTTDisp.Columns[im].ToString()))
                                        {
                                        }
                                        else
                                        {
                                            dtTTDisp.Columns.Remove(dtTTDisp.Columns[im].ToString());




                                            im--;


                                        }
                                    }
                                }
                                string txt1 = string.Empty;
                                string txt2 = string.Empty;
                                string txt3 = string.Empty;
                                string txt4 = string.Empty;
                                string txt5 = string.Empty;

                                for (i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    drrow = dtTTDisp.NewRow();
                                    if (i == 0)
                                    {
                                        //Fpspread2.Sheets[0].RowCount++;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cball;
                                        //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                    }

                                    count++;
                                    drrow["SNo."] = count.ToString();


                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = cb;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]); ;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Locked = true;
                                    //Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Locked = true;

                                    if (val == 15 || val == 16)
                                    {
                                    }
                                    else
                                    {
                                        string admi_status = Convert.ToString(ds.Tables[0].Rows[i]["Admission_Status"]);
                                        string delflag = Convert.ToString(ds.Tables[0].Rows[i]["DelFlag"]);
                                        string examflg = Convert.ToString(ds.Tables[0].Rows[i]["Exam_Flag"]);
                                        string coursecomp = Convert.ToString(ds.Tables[0].Rows[i]["CC"]);
                                    }

                                    cc = 0;
                                    string text = string.Empty;
                                    DataView dv = new DataView();
                                    DataView dv1 = new DataView();
                                    string linkname = Convert.ToString(ddl_colord.SelectedItem.Text);
                                    string columnvalue = string.Empty;
                                    DataSet dscol = new DataSet();


                                    string rollno = ds.Tables[0].Rows[i]["roll_no"].ToString();
                                    sem_tagvalue = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                                    Batch_tagvalue = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                                    dept_tagvalue = ds.Tables[0].Rows[i]["degree_code"].ToString();
                                   string tosem_tagvalue = ddrtosem.SelectedItem.Text;
                                   string fromsem_tagvalue = ddrsem.SelectedItem.Text;
                                    if (rdb_cumm.Checked == true)
                                    {
                                        string attme = d2.GetFunction("select max(attempts) from mark_entry where roll_no='" + rollno + "' ");

                                        //   string attme = ds.Tables[0].Rows[i]["attempts"].ToString();

                                        if (attme == "1" || attme == "0" || attme == "")
                                        {
                                            double cgpamark = 0.0;
                                            string ddset = Convert.ToString(drbless.SelectedValue);
                                            text = Calculete_CGPA(rollno, fromsem_tagvalue,tosem_tagvalue, dept_tagvalue, Batch_tagvalue, latemode, Convert.ToString(ddlcollege.SelectedValue).Trim());
                                            cgpas = text;
                                            double.TryParse(text, out cgpamark);

                                            if (Txtrange.Text != "" && Txtto.Text != "")
                                            {
                                                if (ddset == "1")
                                                {

                                                    if (cgpamark < Convert.ToDouble(Txtto.Text) && cgpamark > Convert.ToDouble(Txtrange.Text))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        removerow = 1;
                                                    }
                                                }
                                                else if (ddset == "2")
                                                {

                                                    if (cgpamark > Convert.ToDouble(Txtrange.Text) && cgpamark < Convert.ToDouble(Txtto.Text))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        removerow = 1;
                                                    }
                                                }
                                                else if (ddset == "3")
                                                {
                                                    if (cgpamark >= Convert.ToDouble(Txtrange.Text) && cgpamark <= Convert.ToDouble(Txtto.Text))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        removerow = 1;
                                                    }
                                                }
                                                else if (ddset == "4")
                                                {
                                                    if (cgpamark >= Convert.ToDouble(Txtrange.Text) && cgpamark <= Convert.ToDouble(Txtto.Text))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        removerow = 1;
                                                    }
                                                }
                                                else if (ddset == "5")
                                                {
                                                    if (cgpamark == Convert.ToDouble(Txtto.Text))
                                                    {

                                                    }
                                                    else
                                                    {
                                                        removerow = 1;
                                                    }
                                                }


                                            }
                                        }
                                        else
                                        {
                                            removerow = 1;
                                        }
                                    }
                                    else if (rdb_detail.Checked == true)
                                    {
                                        string attme = d2.GetFunction("select max(attempts) from mark_entry where roll_no='" + rollno + "' ");
                                        // string attme = ds.Tables[0].Rows[i]["attempts"].ToString();
                                        int att = 0;
                                        int.TryParse(attme, out att);
                                        if (chksingle.Checked == true && chkmulti.Checked == true)
                                        {
                                            if (att >= 2)
                                            {

                                            }
                                            else
                                            {
                                                removerow = 1;
                                            }
                                        }
                                        else if (chksingle.Checked == true && chkstill.Checked == true)
                                        {
                                            string excheck = d2.GetFunction("Select top 1 isnull(Exam_Code,-1),exam_month,exam_year from Exam_Details where Degree_Code = '" + dept_tagvalue.ToString() + "' and Current_Semester <= '" + sem_tagvalue + "' and Batch_Year = '" + Batch_tagvalue.ToString() + "' order by exam_year desc");

                                            string chkarr = d2.GetFunction("select COUNT(distinct subject_no) from mark_entry where exam_code='" + excheck + "' and roll_no='" + rollno + "' and result<>'Pass'");

                                            if (att == 2 || chkarr != "0")
                                            {

                                            }
                                            else
                                            {
                                                removerow = 1;
                                            }
                                        }
                                        else if (chkmulti.Checked == true && chkstill.Checked == true)
                                        {
                                            string excheck = d2.GetFunction("Select top 1 isnull(Exam_Code,-1),exam_month,exam_year from Exam_Details where Degree_Code = '" + dept_tagvalue.ToString() + "' and Current_Semester <= '" + sem_tagvalue + "' and Batch_Year = '" + Batch_tagvalue.ToString() + "' order by exam_year desc");

                                            string chkarr = d2.GetFunction("select COUNT(distinct subject_no) from mark_entry where exam_code='" + excheck + "' and roll_no='" + rollno + "' and result<>'Pass'");

                                            if (att > 2 || chkarr != "0")
                                            {

                                            }
                                            else
                                            {
                                                removerow = 1;
                                            }
                                        }
                                        else if (chksingle.Checked == true)
                                        {
                                            if (att == 2)
                                            {

                                            }
                                            else
                                            {
                                                removerow = 1;
                                            }
                                        }
                                        else if (chkmulti.Checked == true)
                                        {
                                            if (att > 2)
                                            {

                                            }
                                            else
                                            {
                                                removerow = 1;
                                            }
                                        }
                                        else if (chkstill.Checked == true)
                                        {

                                            string excheck = d2.GetFunction("Select top 1 isnull(Exam_Code,-1),exam_month,exam_year from Exam_Details where Degree_Code = '" + dept_tagvalue.ToString() + "' and Current_Semester <= '" + sem_tagvalue + "' and Batch_Year = '" + Batch_tagvalue.ToString() + "' order by exam_year desc");

                                            string chkarr = d2.GetFunction("select COUNT(distinct subject_no) from mark_entry where exam_code='" + excheck + "' and roll_no='" + rollno + "' and result<>'Pass'");
                                            if (chkarr != "0")
                                            {
                                            }
                                            else
                                                removerow = 1;

                                        }



                                    }
                                    if (removerow != 1)
                                    {

                                        for (int k = 1; k < dtTTDisp.Columns.Count; k++)
                                        {
                                            cc++;
                                            string col = Convert.ToString(hscolumnvalue[dtTTDisp.Columns[cc].ToString()]);
                                            string colval = dtTTDisp.Columns[cc].ToString();
                                            string cerificate = "0";
                                            //string cerificate = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and MasterCode='" + col + "' ");
                                            if (col == "type_semester" || col == "Institute_name" || col == "isgrade" || col == "Part1Language" || col == "Part2Language" || col == "university_code" || col == "instaddress" || col == "Xmedium" || col == "medium" || col == "percentage" || col == "securedmark" || col == "totalmark" || col == "passyear" || col == "passmonth" || col == "Vocational_stream" || col == "markPriority" || col == "Cut_Of_Mark" || col == "LastTCNo" || col == "LastTCDate" || col == "uni_state" || col == "University")
                                            {
                                                if (ds.Tables[1].Rows.Count > 0)
                                                {
                                                    if (ds.Tables[1].Rows.Count > 0)
                                                    {
                                                        if (col.ToLower() == "uni_state")
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                            dv = ds.Tables[1].DefaultView;
                                                            DataTable temp = new DataTable();
                                                            temp = dv.ToTable();
                                                            if (dv.Count > 0)
                                                            {
                                                                temp.DefaultView.RowFilter = " uni_state is not null ";
                                                                dv1 = temp.DefaultView;
                                                                if (dv1.Count > 0)
                                                                {
                                                                    text = Convert.ToString(dv1[0]["uni_state"]);
                                                                    drrow["State"] = text;
                                                                }
                                                                else
                                                                {
                                                                    text = string.Empty;
                                                                    drrow["State"] = text;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                                drrow["State"] = text;
                                                            }
                                                        }

                                                        else if (col == "University")
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                            dv = ds.Tables[1].DefaultView;
                                                            DataTable temp = new DataTable();
                                                            temp = dv.ToTable();
                                                            if (dv.Count > 0)
                                                            {
                                                                temp.DefaultView.RowFilter = " university_code is not null ";
                                                                dv1 = temp.DefaultView;
                                                                if (dv1.Count > 0)
                                                                {
                                                                    text = Convert.ToString(dv1[0]["University"]);
                                                                    drrow["University Name"] = text;
                                                                }
                                                                else
                                                                {
                                                                    text = string.Empty;
                                                                    drrow["University Name"] = text;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                                drrow["University Name"] = text;
                                                            }
                                                        }

                                                        else
                                                        {
                                                            ds.Tables[1].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "'";
                                                            dv = ds.Tables[1].DefaultView;
                                                            if (dv.Count > 0)
                                                            {
                                                                text = Convert.ToString(dv[0][col]);
                                                                drrow[colval] = text;
                                                            }
                                                            else
                                                            {
                                                                text = string.Empty;
                                                                drrow[colval] = text;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                }
                                            }
                                            else if (col.ToLower() == "subject_name")
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                                dv = ds.Tables[7].DefaultView;
                                                DataTable temp = new DataTable();
                                                temp = dv.ToTable();
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " subject_name is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["subject_name"]);
                                                        drrow["Language"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow["Language"] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow["Language"] = text;
                                                }
                                            }
                                            else if (col.ToLower() == "acronym")
                                            {
                                                ds.Tables[7].DefaultView.RowFilter = "roll_no='" + Convert.ToString(ds.Tables[0].Rows[i]["roll_no"]) + "' ";
                                                dv = ds.Tables[7].DefaultView;
                                                DataTable temp = new DataTable();
                                                temp = dv.ToTable();
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " acronym is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["acronym"]);
                                                        drrow["Language Acronym"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow["Language Acronym"] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow["Language Acronym"] = text;
                                                }
                                            }
                                            else if (col.ToLower() == "building_description")
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                dv = ds.Tables[8].DefaultView;
                                                DataTable temp = new DataTable();
                                                temp = dv.ToTable();
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " hallname is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["hallname"]);
                                                        drrow["Hall"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow["Hall"] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow["Hall"] = text;
                                                }
                                            }
                                            else if (col.ToLower() == "building_acronym")
                                            {
                                                ds.Tables[8].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["App_No"]) + "' ";
                                                dv = ds.Tables[8].DefaultView;
                                                DataTable temp = new DataTable();
                                                temp = dv.ToTable();
                                                if (dv.Count > 0)
                                                {
                                                    temp.DefaultView.RowFilter = " hall is not null ";
                                                    dv1 = temp.DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["hall"]);
                                                        drrow["Hall Acronym"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow["Hall Acronym"] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow["Hall Acronym"] = text;
                                                }
                                            }
                                            else if (col == "AccNo" || col == "DebitCardNo" || col == "IFSCCode" || col == "BankName" || col == "BankBranch")
                                            {
                                                if (ds.Tables[3].Rows.Count > 0)
                                                {
                                                    ds.Tables[3].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                    dv1 = ds.Tables[3].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0][col]);
                                                        drrow[colval] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow[colval] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow[colval] = text;
                                                }
                                            }
                                            else if (col == "name_roll" || col == "relationship" || col == "isstaff")
                                            {
                                                if (ds.Tables[4].Rows.Count > 0)
                                                {
                                                    ds.Tables[4].DefaultView.RowFilter = "application_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                    dv1 = ds.Tables[4].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        if (col == "isstaff")
                                                        {
                                                            text = Convert.ToString(dv1[0][col]);
                                                            if (text == "0")
                                                            {
                                                                text = "Student";
                                                                drrow[colval] = text;
                                                            }
                                                            else
                                                            {
                                                                text = "Staff";
                                                                drrow[colval] = text;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            text = Convert.ToString(dv1[0][col]);
                                                            drrow[colval] = text;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow[colval] = text;
                                                    }
                                                }
                                                else
                                                {

                                                    text = string.Empty;
                                                    drrow[colval] = text;
                                                }
                                            }
                                            else if (col == "HostelName")
                                            {
                                                if (ds.Tables[5].Rows.Count > 0)
                                                {
                                                    ds.Tables[5].DefaultView.RowFilter = "APP_No='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "'";
                                                    dv1 = ds.Tables[5].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        text = Convert.ToString(dv1[0]["HostelName"]);
                                                        drrow["HostelName"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        drrow["HostelName"] = text;
                                                    }
                                                }
                                                else
                                                {
                                                    text = string.Empty;
                                                    drrow["HostelName"] = text;
                                                }
                                            }
                                            //**added by Mullai
                                            else if (col == "noofarrear")
                                            {

                                                string arrearcount = " select COUNT (distinct subject_no)as noofarrear from mark_entry where roll_no='" + rollno + "' and subject_no not in(select s.subject_no from subject s,mark_entry m where m.subject_no=s.subject_no and m.result='pass' and roll_no='" + rollno + "')";
                                                nofar.Clear();
                                                nofar = d2.select_method_wo_parameter(arrearcount, "text");

                                                if (nofar.Tables[0].Rows.Count > 0)
                                                {

                                                    dv1 = nofar.Tables[0].DefaultView;

                                                    if (dv1.Count > 0)
                                                    {


                                                        text = Convert.ToString(dv1[0]["noofarrear"]);
                                                        noarrear = Convert.ToString(dv1[0]["noofarrear"]);
                                                        drrow["No of arrear"] = text;
                                                    }
                                                    else
                                                    {
                                                        text = string.Empty;
                                                        noarrear = string.Empty;
                                                        drrow["No of arrear"] = text;
                                                    }
                                                    n_arrear = Convert.ToInt32(noarrear);


                                                }
                                            }

                                            else if (col == "CGPA")
                                            {
                                                double cgpamark = 0.0;
                                                string ddset = Convert.ToString(drbless.SelectedValue);
                                                if (n_arrear == 0)
                                                {
                                                    text = cgpas;
                                                    drrow["CGPA"] = text;
                                                    cgpas = "";
                                                }
                                                else
                                                {
                                                    text = "";
                                                    drrow["CGPA"] = text;
                                                    cgpas = "";
                                                }

                                            }
                                            else if (col == "referby")
                                            {
                                                string code = Convert.ToString(ds.Tables[0].Rows[i]["refer_stcode"]);//Added by saranya on 12/7/2018
                                                string refer = Convert.ToString(ds.Tables[0].Rows[i]["referby"]);//abarna
                                                text = refer + "-" + code;
                                                drrow["Refered By"] = text;

                                            }
                                            //**
                                            else
                                            {
                                                string Note = "";
                                                if (Note.Trim() == "")
                                                {
                                                    text = Convert.ToString(ds.Tables[0].Rows[i][col]);
                                                    drrow[colval] = text;
                                                }
                                                else
                                                {
                                                    if (ds.Tables[2].Rows.Count > 0)
                                                    {
                                                        ds.Tables[2].DefaultView.RowFilter = "app_no='" + Convert.ToString(ds.Tables[0].Rows[i]["app_no"]) + "' and CertificateId='" + col + "'";
                                                        dv1 = ds.Tables[2].DefaultView;
                                                        if (dv1.Count > 0)
                                                        {

                                                            text = Convert.ToString(dv1[0]["certificateno"]);
                                                            drrow["certificateno"] = text;
                                                        }
                                                        else
                                                        {
                                                            text = string.Empty;
                                                            drrow["certificateno"] = text;
                                                        }
                                                    }
                                                    else
                                                    {

                                                        text = string.Empty;
                                                        drrow["certificateno"] = text;
                                                    }
                                                }
                                            }
                                            if (col == "visualhandy")
                                            {
                                                if (text == "0")
                                                {
                                                    text = "No";
                                                    drrow["visualhandy"] = text;
                                                }
                                                else if (text == "1")
                                                {
                                                    text = "Yes";
                                                    drrow["visualhandy"] = text;
                                                }
                                            }
                                            if (col == "first_graduate")
                                            {
                                                if (text == "0")
                                                {
                                                    text = "No";
                                                    drrow["visualhandy"] = text;
                                                }
                                                else if (text == "1")
                                                {
                                                    text = "Yes";
                                                }
                                            }
                                            if (col == "Countryp" || col == "Countryc")
                                            {
                                                text = d2.GetFunction("select textval from textvaltable where TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                            }

                                            if (col.ToLower() == "cityp" || col.ToLower() == "cityc")
                                            {
                                                if (!Convert.ToString(ds.Tables[0].Rows[i][col]).Any(char.IsLetter))
                                                    text = d2.GetFunction("select textval from textvaltable where TextCriteria ='city' and TextCode='" + Convert.ToString(ds.Tables[0].Rows[i][col]) + "'");
                                            }
                                            if (text == "0")
                                            {
                                                text = string.Empty;
                                            }
                                            if (text == "")
                                            {
                                                text = string.Empty;
                                            }


                                        }
                                    }


                                    dtTTDisp.Rows.Add(drrow);
                                    if (removerow == 1)
                                    {
                                        dtTTDisp.Rows.Remove(drrow);
                                        removerow = 0;
                                        count--;
                                    }
                                }

                                // }
                                //}
                            }
                        }


                        if (dtTTDisp.Rows.Count > 1)
                        {
                            //Fpspread2.Sheets[0].SetColumnMerge(u, FarPoint.Web.Spread.Model.MergePolicy.Always);

                            lblvalidation1.Text = string.Empty;
                            gview.DataSource = dtTTDisp;
                            gview.DataBind();
                            gview.Visible = true;

                            RowHead(gview);
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Text = "No Records Found";
                            gview.Visible = false;
                        }

                        //if (gview.Columns.Count > 0)
                        //{
                        //    for (int i = 0; i < gview.Columns.Count; i++)
                        //    {
                        //        if(gview.Columns)
                        //    }
                        //}

                    }
                }
                lbl_headernamespd2.Visible = false;
            
           
        }

        catch (Exception ex)
        {
            //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    protected void RowHead(GridView gview)
    {
        for (int head = 0; head < 1; head++)
        {
            gview.Rows[head].BackColor = ColorTranslator.FromHtml("#0CA6CA");
            gview.Rows[head].Font.Bold = true;
            gview.Rows[head].HorizontalAlign = HorizontalAlign.Center;
            gview.Rows[head].Font.Name = "Book Antique";
        }
    }
    public void loadtext()//delsii
    {
        if (colval == "1")
        {
            loadval = "Course";
            printval = "Course_Name";
        }
        if (colval == "2")
        {
            loadval = "Branch";
            printval = "Dept_Name";
        }
        if (colval == "3")
        {
            loadval = "Batch";
            printval = "Batch_Year";
        }
        if (colval == "4")
        {
            loadval = "Semester";
            printval = "Current_Semester";
        }
        if (colval == "5")
        {
            loadval = "Parent Name";
            printval = "parent_name";
        }
        if (colval == "6")
        {
            loadval = "DOB";
            printval = "dob";
        }
        if (colval == "7")
        {
            loadval = "Parent Occupation";
            printval = "parent_occu";
        }
        if (colval == "8")
        {
            loadval = "Mother Tongue";
            printval = "mother_tongue";
        }
        if (colval == "9")
        {
            loadval = "Religion";
            printval = "religion";
        }
        if (colval == "10")
        {
            loadval = "Citizen";
            printval = "citizen";
        }
        if (colval == "11")
        {
            loadval = "Community";
            printval = "community";
        }
        if (colval == "12")
        {
            loadval = "Caste";
            printval = "caste";
        }
        if (colval == "13")
        {
            loadval = "TamilOrginFromAndaman";
            printval = "TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            loadval = "VisualHandy";
            printval = "visualhandy";
        }
        if (colval == "15")
        {
            loadval = "First Graduate";
            printval = "first_graduate";
        }
        if (colval == "16")
        {
            loadval = "SeatType";
            printval = "seattype";
        }
        if (colval == "17")
        {
            loadval = "Curricular";
            printval = "co_curricular";
        }
        if (colval == "18")
        {
            loadval = "Address";
            printval = "parent_addressP";
        }
        if (colval == "19")
        {
            loadval = "Street";
            printval = "Streetp";
        }
        if (colval == "20")
        {
            loadval = "City";
            printval = "cityp";
        }
        if (colval == "21")
        {
            loadval = "State";
            printval = "parent_statep";
        }
        if (colval == "22")
        {
            loadval = "Country";
            printval = "Countryp";
        }
        if (colval == "23")
        {
            loadval = "Student Mobile";
            printval = "Student_Mobile";
        }
        if (colval == "24")
        {
            loadval = "PinCode";
            printval = "parent_pincodep";
        }
        if (colval == "25")
        {
            loadval = "Parent Phone No";
            printval = "parent_phnop";
        }
        if (colval == "26")
        {
            loadval = "MissionaryChild";
            printval = "MissionaryChild";
        }
        if (colval == "27")
        {
            loadval = "missionarydisc";
            printval = "missionarydisc";
        }
        if (colval == "28")
        {
            loadval = "Institute Name";
            printval = "Institute_name";
        }
        if (colval == "29")
        {
            loadval = "Part1 Language";
            printval = "Part1Language";
        }
        if (colval == "30")
        {
            loadval = "Part2 Language";
            printval = "Part2Language";
        }
        if (colval == "31")
        {
            loadval = "University Name";
            printval = "University";
        }
        if (colval == "40")
        {
            loadval = "State";
            printval = "uni_state";

        }
        if (colval == "32")
        {
            loadval = "LastTC No";
            printval = "LastTCNo";
        }
        if (colval == "33")
        {
            loadval = "LastTC Date";
            printval = "LastTCDate";
        }
        if (colval == "34")
        {
            loadval = "HostelName";
            printval = "HostelName";
        }
        if (colval == "35")
        {
            loadval = "Voter ID";
            printval = "ElectionID_No";
        }
        if (colval == "36")
        {
            loadval = "Diploma-Provisional No";
        }
        if (colval == "37")
        {
            loadval = "Diploma-Consolidate";
        }
        if (colval == "38")
        {
            loadval = "Diploma-Degree";
        }
        if (colval == "39")
        {
            loadval = "Diploma- No of Semester";
        }
        if (colval == "40")
        {
            loadval = "UG-Provisional No";
        }
        if (colval == "41")
        {
            loadval = "UG-Consolidate";
        }
        if (colval == "42")
        {
            loadval = "UG-Degree";
        }
        if (colval == "43")
        {
            loadval = "UG- No of Semester";
        }
        if (colval == "44")
        {
            loadval = "PG-Provisional No";
        }
        if (colval == "45")
        {
            loadval = "PG-Consolidate";
        }
        if (colval == "46")
        {
            loadval = "PG-Degree";
        }
        if (colval == "47")
        {
            loadval = "PG- No of Semester";
        }
        if (colval == "48")
        {
            loadval = "Residency";
            printval = "CampusReq";
        }
        if (colval == "49")
        {
            loadval = "Physically challange";
            printval = "handy";
        }
        if (colval == "50")
        {
            printval = "DistinctSport";
            loadval = "Sports";
        }
        if (colval == "51")
        {
            printval = "islearningdis";
            loadval = "Learning Disability";
        }
        if (colval == "52")
        {
            printval = "isdisabledisc";
            loadval = "Other Disability";
        }
        if (colval == "53")
        {
            loadval = "IsDisable";
            printval = "isdisable";
        }
        if (colval == "54")
        {
            loadval = "Student Name";
            printval = "stud_name";
        }
        if (colval == "55")
        {
            loadval = "Roll No";
            printval = "Roll_no";
        }
        if (colval == "56")
        {
            loadval = "Student EmailId";
            printval = "StuPer_Id";
        }
        if (colval == "57")
        {
            loadval = "Reg No";
            printval = "reg_no";
        }
        if (colval == "58")
        {
            loadval = "Admission No";
            printval = "roll_admit";
        }
        if (colval == "59")
        {
            loadval = "Application No";
            printval = "app_formno";
        }
        if (colval == "60")
        {
            loadval = "Section";
            printval = "sections";
        }
        if (colval == "61")
        {
            loadval = "Gender";
            printval = "sex";
        }
        if (colval == "62")
        {
            loadval = "Blood Group";
            printval = "bldgrp";
        }
        if (colval == "63")
        {
            loadval = "Student Type";
            printval = "stud_type";
        }
        if (colval == "64")
        {
            loadval = "Ex-serviceman";
            printval = "IsExService";
        }
        if (colval == "65")
        {
            loadval = "Hostel accommodation";
            printval = "CampusReq";
        }
        if (colval == "66")
        {
            loadval = "Blood Donor";
            printval = "isdonar";
        }
        if (colval == "67")
        {
            loadval = "Reserved Caste";
            printval = "ReserveCategory";
        }
        if (colval == "68")
        {
            loadval = "Economic Backward";
            printval = "EconBackword";
        }
        if (colval == "69")
        {
            loadval = "Parents Old Student";
            printval = "parentoldstud";
        }
        if (colval == "70")
        {
            loadval = "Driving License";
            printval = "IsDrivingLic";
        }
        if (colval == "71")
        {
            loadval = "License No";
            printval = "Driving_details";
        }
        if (colval == "72")
        {
            loadval = "Tuition Fee Waiver";
            printval = "tutionfee_waiver";
        }
        if (colval == "73")
        {
            loadval = "Insurance";
            printval = "IsInsurance";
        }
        if (colval == "74")
        {
            loadval = "Rank";
            printval = "ExsRank";
        }
        if (colval == "75")
        {
            loadval = "Place";
            printval = "ExSPlace";
        }
        if (colval == "76")
        {
            loadval = "Number";
            printval = "ExsNumber";
        }
        if (colval == "77")
        {
            loadval = "Insurance Amount";
            printval = "Insurance_Amount";
        }
        if (colval == "78")
        {
            loadval = "Insurance InsBy";
            printval = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            loadval = "Insurance Nominee";
            printval = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            loadval = "Insurance NominRelation";
            printval = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            loadval = "Applied Date";
            printval = "date_applied";
        }
        if (colval == "82")
        {
            loadval = "Alternate Mob No";
            printval = "alter_mobileno";
        }
        if (colval == "83")
        {
            loadval = "Sub Caste";
            printval = "SubCaste";
        }
        if (colval == "84")
        {
            loadval = "Father Income";
            printval = "parent_income";
        }
        if (colval == "85")
        {
            loadval = "Father Mob No";
            printval = "parentF_Mobile";
        }
        if (colval == "86")
        {
            loadval = "Father EmailId";
            printval = "emailp";
        }
        if (colval == "87")
        {
            loadval = "Mother";
            printval = "mother";
        }
        if (colval == "88")
        {
            loadval = "Mother Income";
            printval = "mIncome";
        }
        if (colval == "89")
        {
            loadval = "Mother Mob No";
            printval = "parentM_Mobile";
        }
        if (colval == "90")
        {
            loadval = "Mother EmailId";
            printval = "emailM";
        }
        if (colval == "91")
        {
            loadval = "Guardian Name";
            printval = "guardian_name";
        }
        if (colval == "92")
        {
            loadval = "Guardian Mob No";
            printval = "guardian_mobile";
        }
        if (colval == "93")
        {
            loadval = "Guardian Email Id";
            printval = "emailg";
        }
        if (colval == "94")
        {
            loadval = "Place Of Birth";
            printval = "place_birth";
        }
        if (colval == "95")
        {
            loadval = "Adhaar Card No";
            printval = "Aadharcard_no";
        }
        if (colval == "96")
        {
            loadval = "Mother Occupation";
            printval = "motherocc";
        }
        if (colval == "97")
        {
            loadval = "Institution Address";
            printval = "instaddress";
        }
        if (colval == "98")
        {
            loadval = "X medium";
            printval = "Xmedium";
        }
        if (colval == "99")
        {
            loadval = "X11 Medium";
            printval = "medium";
        }
        if (colval == "100")
        {
            loadval = "Percentage";
            printval = "percentage";
        }
        if (colval == "101")
        {
            loadval = "Secured Mark";
            printval = "securedmark";
        }
        if (colval == "102")
        {
            printval = "totalmark";
            loadval = "Total Mark";
        }
        if (colval == "103")
        {
            loadval = "Pass Month";
            printval = "passmonth";
        }
        if (colval == "104")
        {
            loadval = "Pass Year";
            printval = "passyear";
        }
        if (colval == "105")
        {
            loadval = "Vocational Stream";
            printval = "Vocational_stream";
        }
        if (colval == "106")
        {
            loadval = "Mark Priority";
            printval = "markPriority";
        }
        if (colval == "107")
        {
            loadval = "Cut Of Mark";
            printval = "Cut_Of_Mark";
        }
        if (colval == "108")
        {
            loadval = "Communication Address";
            printval = "parent_addressc";
        }
        if (colval == "109")
        {
            loadval = "Communication Street";
            printval = "Streetc";
        }
        if (colval == "110")
        {
            loadval = "Communication City";
            printval = "cityc";
        }
        if (colval == "111")
        {
            loadval = "Communication State";
            printval = "parent_statec";
        }
        if (colval == "112")
        {
            loadval = "Communication Country";
            printval = "Countryc";
        }
        if (colval == "113")
        {
            printval = "parent_pincodec";
            loadval = "Communication PinCode";
        }
        if (colval == "114")
        {
            loadval = "A/C No";
            printval = "AccNo";
        }
        if (colval == "115")
        {
            printval = "DebitCardNo";
            loadval = "DebitCard No";
        }
        if (colval == "116")
        {
            loadval = "IFSCCode";
            printval = "IFSCCode";
        }
        if (colval == "117")
        {
            loadval = "Bank Name";
            printval = "BankName";
        }
        if (colval == "118")
        {
            printval = "BankBranch";
            loadval = "Bank Branch";
        }
        if (colval == "119")
        {
            printval = "name_roll";
            loadval = "Relation Name";
        }
        if (colval == "120")
        {
            printval = "relationship";
            loadval = "Relationship";
        }
        if (colval == "121")
        {
            printval = "isstaff";
            loadval = "Staff/Student";
        }
        if (colval == "122")
        {
            printval = "Boarding";
            loadval = "Boarding";
        }
        if (colval == "123")
        {
            printval = "vehid";
            loadval = "Vehicle Id";
        }
        if (colval == "43")
        {
            printval = "Mode";
            loadval = "Mode";
        }
        if (colval == "36")
        {
            printval = "Adm_Date";
            loadval = "Admission Date";
        }
        if (colval == "37")
        {
            printval = "enrollment_confirm_date";
            loadval = "Enrollment Date";
        }
        if (colval == "38")
        {
            printval = "Adm_Date";
            loadval = "Join Date";
        }
        if (colval == "125")
        {
            printval = "CGPA";
            loadval = "CGPA";
        }
        if (colval == "126")
        {
            printval = "noofarrear";
            loadval = "No of arrear";
        }
        if (colval == "127")//added
        {
            printval = "referby";
            loadval = "Refered By";
        }
        if (colval == "128")
        {
            loadval = "Dob[DD]";
            printval = "day";
        }
        if (colval == "129")
        {
            loadval = "Dob[MM]";
            printval = "Month";
        }
        if (colval == "130")
        {
            loadval = "Dob[YYYY]";
            printval = "Year";
        }
        if (colval == "131")
        {
            loadval = "Language";
            printval = "subject_name";
        }
        if (colval == "132")
        {
            loadval = "Language Acronym";
            printval = "acronym";
        }
        if (colval == "133")
        {
            loadval = "Hall";
            printval = "building_description";
        }
        if (colval == "134")
        {
            loadval = "Hall Acronym";
            printval = "Building_Acronym";
        }
        if (Convert.ToInt32(colval) > 134)
        {
            loadval = d2.GetFunction("select distinct MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
            printval = d2.GetFunction("select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' and MasterCode='" + colval + "' ");
        }
    }
    public void loadlcolumns()
    {
        try
        {
            string linkname = "StudentStrengthCommon column order settings";
            DataSet dscol = new DataSet();
            string selcol = "select LinkValue from New_InsSettings where LinkName='" + linkname + "' and  user_code='" + usercode + "' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
            dscol.Clear();
            dscol = d2.select_method_wo_parameter(selcol, "Text");
            if (dscol.Tables.Count > 0)
            {
                if (dscol.Tables[0].Rows.Count > 0)
                {
                    for (int col = 0; col < dscol.Tables[0].Rows.Count; col++)
                    {
                        string value = Convert.ToString(dscol.Tables[0].Rows[col]["LinkValue"]);
                        string[] valuesplit = value.Split(',');
                        if (valuesplit.Length > 0)
                        {
                            for (int k = 0; k < valuesplit.Length; k++)
                            {
                                colval = Convert.ToString(valuesplit[k]);
                                loadtext();
                                lb_column1.Items.Add(new ListItem(loadval, colval));
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    public void savecolumnorder()
    {
        string columnvalue = string.Empty;
        DataSet dscol = new DataSet();
        string linkname = Convert.ToString(ddl_coltypeadd.SelectedItem.Text);
        string val = string.Empty;
        for (int j = 0; j < lb_column1.Items.Count; j++)
        {
            val = lb_column1.Items[j].Value;
            if (columnvalue == "")
            {
                columnvalue = val;
            }
            else
            {
                columnvalue = columnvalue + ',' + val;
            }
        }
        string clsinsert = " if exists(select * from New_InsSettings where LinkName='" + linkname + "' and college_code='" + ddlcollege.SelectedItem.Value + "'  ) update New_InsSettings set LinkValue='" + columnvalue + "' where LinkName='" + linkname + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' else insert into New_InsSettings (LinkName,LinkValue,user_code,college_code)values('" + linkname + "','" + columnvalue + "','" + usercode + "','" + ddlcollege.SelectedItem.Value + "')";
        int clsupdate = d2.update_method_wo_parameter(clsinsert, "Text");
    }

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string ss = Session["usercode"].ToString();
            string degreedetails = "Student Placement Report";
            string pagename = "Placement Details.aspx";
            NEWPrintMater1.loadspreaddetails(gview, pagename, degreedetails, 0, ss);
            ////Printcontrol.loadspreaddetails(attnd_report, pagename, degreedetails);
           NEWPrintMater1.Visible = true;
        }
        catch (Exception ex) { //d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx");
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                d2.printexcelreportgrid(gview, report);
                lbl_norec.Visible = false;
            }
            else
            {
                lbl_norec.Text = "Please Enter Your Report Name";
                lbl_norec.Visible = true;
            }
            btn_Excel.Focus();
        }
        catch (Exception ex)
        {
            lbl_norec.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void btnExcelNew_Click(object sender, EventArgs e)
    {
       // try
       // {
            //string reportname = txtexcelname.Text;
            //if (reportname.ToString().Trim() != "")
            //{
            //    d2.printexcelreport(Fpspread1, reportname);
            //    lblvalidation1.Visible = false;
            //}
            //else
            //{
            //    lblvalidation1.Text = "Please Enter Your Report Name";
            //    lblvalidation1.Visible = true;
            //    txtexcelname.Focus();
            //}
        //}
        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    protected void btnprintmasterNew_Click(object sender, EventArgs e)
    {
        //try
        //{
        //    string degreedetails = "Admission Report";
        //    string pagename = "StudentStrengthStatusReport.aspx";
        //    Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
        //    rptprint.Visible = true;
        //    Printmaster1.Visible = true;
        //    // 
        //}
        //catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }

    protected void txtexcelname_TextChanged(object sender, EventArgs e)
    {
        try
        {
            txt_excelname.Visible = true;
            btn_Excel.Visible = true;
            btn_printmaster.Visible = true;
            lbl_reportname.Visible = true;
            btn_Excel.Focus();
            if (txt_excelname.Text == "")
            {
                lbl_norec.Visible = true;
            }
            else
            {
                lbl_norec.Visible = false;
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    protected void btnclose_click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    protected void btnok_click(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedItem.Text != "Select")
        {
            if (lb_column1.Items.Count > 0)
            {
                poppernew.Visible = false;
                savecolumnorder();
                if (savecolumnoder == "")
                {
                    fpspread1go1();
                }
               
                lblalerterr.Visible = false;
            }
            else
            {
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please select atleast one colunm then proceed!";
            }
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Please Select Report Type";
        }
    }
    protected void btnMvOneRt_Click(object sender, EventArgs e)
    {
        try
        {
            bool ok = true;
            if (lb_selectcolumn.Items.Count > 0 && lb_selectcolumn.SelectedItem.Value != "")
            {
                for (int j = 0; j < lb_column1.Items.Count; j++)
                {
                    if (lb_column1.Items[j].Value == lb_selectcolumn.SelectedItem.Value)
                    {
                        ok = false;
                    }
                }
                if (ok)
                {
                    ListItem lst = new ListItem(lb_selectcolumn.SelectedItem.Text, lb_selectcolumn.SelectedItem.Value);
                    lb_column1.Items.Add(lst);
                }
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    protected void btnMvTwoRt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            if (lb_selectcolumn.Items.Count > 0)
            {
                for (int j = 0; j < lb_selectcolumn.Items.Count; j++)
                {
                    lb_column1.Items.Add(new ListItem(lb_selectcolumn.Items[j].Text.ToString(), lb_selectcolumn.Items[j].Value.ToString()));
                }
            }
            lb_selectcolumn.Items.Clear();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    protected void btnMvOneLt_Click(object sender, EventArgs e)
    {
        try
        {
            if (lb_column1.Items.Count > 0 && lb_column1.SelectedItem.Value != "")
            {
                lb_column1.Items.RemoveAt(lb_column1.SelectedIndex);
            }
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    protected void btnMvTwoLt_Click(object sender, EventArgs e)
    {
        try
        {
            lb_column1.Items.Clear();
            load();
        }
        catch (Exception ex) { d2.sendErrorMail(ex, collegecode1, "StudentStrengthStatusReport.aspx"); }
    }
    public void load()
    {
        lb_selectcolumn.Items.Clear();
        lb_selectcolumn.Items.Add(new ListItem("Student Name", "54"));
        lb_selectcolumn.Items.Add(new ListItem("Roll No", "55"));
        lb_selectcolumn.Items.Add(new ListItem("Reg No", "57"));
        lb_selectcolumn.Items.Add(new ListItem("Admission No", "58"));
        lb_selectcolumn.Items.Add(new ListItem("Application No", "59"));
        lb_selectcolumn.Items.Add(new ListItem("Applied Date", "81"));
        lb_selectcolumn.Items.Add(new ListItem("Batch", "3"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_degree.Text, "1"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_branch.Text, "2"));
        lb_selectcolumn.Items.Add(new ListItem(lbl_org_sem.Text, "4"));
        lb_selectcolumn.Items.Add(new ListItem("Section", "60"));
        lb_selectcolumn.Items.Add(new ListItem("SeatType", "16"));
        lb_selectcolumn.Items.Add(new ListItem("Student Type", "63"));
        lb_selectcolumn.Items.Add(new ListItem("HostelName", "34"));
        //30.07.16
        lb_selectcolumn.Items.Add(new ListItem("Mode", "43"));
        lb_selectcolumn.Items.Add(new ListItem("Boarding", "122"));
        lb_selectcolumn.Items.Add(new ListItem("Vehicle Id", "123"));
        lb_selectcolumn.Items.Add(new ListItem("Gender", "61"));
        lb_selectcolumn.Items.Add(new ListItem("DOB", "6"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Group", "62"));
        lb_selectcolumn.Items.Add(new ListItem("Father Name", "5"));
        lb_selectcolumn.Items.Add(new ListItem("Father Income", "84"));
        lb_selectcolumn.Items.Add(new ListItem("Father Occupation", "7"));
        lb_selectcolumn.Items.Add(new ListItem("Father Mob No", "85"));
        lb_selectcolumn.Items.Add(new ListItem("Father Email Id", "86"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Name", "87"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Income", "88"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Occupation", "96"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Mob No", "89"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Email Id", "90"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Name", "91"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Email Id", "92"));
        lb_selectcolumn.Items.Add(new ListItem("Guardian Mob No", "93"));
        lb_selectcolumn.Items.Add(new ListItem("Place Of Birth", "94"));
        lb_selectcolumn.Items.Add(new ListItem("Adhaar Card No", "95"));
        lb_selectcolumn.Items.Add(new ListItem("Voter ID", "35"));
        lb_selectcolumn.Items.Add(new ListItem("Mother Tongue", "8"));
        lb_selectcolumn.Items.Add(new ListItem("Religion", "9"));
        lb_selectcolumn.Items.Add(new ListItem("Community", "11"));
        lb_selectcolumn.Items.Add(new ListItem("Caste", "12"));
        lb_selectcolumn.Items.Add(new ListItem("Sub Caste", "83"));
        lb_selectcolumn.Items.Add(new ListItem("Citizen", "10"));
        lb_selectcolumn.Items.Add(new ListItem("TamilOrginFromAndaman", "13"));
        lb_selectcolumn.Items.Add(new ListItem("Ex-serviceman", "64"));
        lb_selectcolumn.Items.Add(new ListItem("Rank", "74"));
        lb_selectcolumn.Items.Add(new ListItem("Place", "75"));
        lb_selectcolumn.Items.Add(new ListItem("Number", "76"));
        lb_selectcolumn.Items.Add(new ListItem("IsDisable", "53"));
        lb_selectcolumn.Items.Add(new ListItem("VisualHandy", "14"));
        lb_selectcolumn.Items.Add(new ListItem("Residency", "48"));
        lb_selectcolumn.Items.Add(new ListItem("Physically challange", "49"));
        lb_selectcolumn.Items.Add(new ListItem("Learning Disability", "51"));
        lb_selectcolumn.Items.Add(new ListItem("Other Disability", "52"));
        lb_selectcolumn.Items.Add(new ListItem("Sports", "50"));
        lb_selectcolumn.Items.Add(new ListItem("First Graduate", "15"));
        lb_selectcolumn.Items.Add(new ListItem("MissionaryChild", "26"));
        lb_selectcolumn.Items.Add(new ListItem("missionarydisc", "27"));
        lb_selectcolumn.Items.Add(new ListItem("Hostel accommodation", "65"));
        lb_selectcolumn.Items.Add(new ListItem("Blood Donor", "66"));
        lb_selectcolumn.Items.Add(new ListItem("Reserved Caste", "67"));
        lb_selectcolumn.Items.Add(new ListItem("Economic Backward", "68"));
        lb_selectcolumn.Items.Add(new ListItem("Parents Old Student", "69"));
        lb_selectcolumn.Items.Add(new ListItem("Driving License", "70"));
        lb_selectcolumn.Items.Add(new ListItem("License No", "71"));
        lb_selectcolumn.Items.Add(new ListItem("Tuition Fee Waiver", "72"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance", "73"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Amount", "77"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance InsBy", "78"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance Nominee", "79"));
        lb_selectcolumn.Items.Add(new ListItem("Insurance NominRelation", "80"));
        lb_selectcolumn.Items.Add(new ListItem("Address", "18"));
        lb_selectcolumn.Items.Add(new ListItem("Street", "19"));
        lb_selectcolumn.Items.Add(new ListItem("City", "20"));
        lb_selectcolumn.Items.Add(new ListItem("State", "21"));
        lb_selectcolumn.Items.Add(new ListItem("Country", "22"));
        lb_selectcolumn.Items.Add(new ListItem("PinCode", "24"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Address", "108"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Street", "109"));
        lb_selectcolumn.Items.Add(new ListItem("Communication City", "110"));
        lb_selectcolumn.Items.Add(new ListItem("Communication State", "111"));
        lb_selectcolumn.Items.Add(new ListItem("Communication Country", "112"));
        lb_selectcolumn.Items.Add(new ListItem("Communication PinCode", "113"));
        lb_selectcolumn.Items.Add(new ListItem("Student Mobile", "23"));
        lb_selectcolumn.Items.Add(new ListItem("Alternate Mob No", "82"));
        lb_selectcolumn.Items.Add(new ListItem("Student EmailId", "56"));
        lb_selectcolumn.Items.Add(new ListItem("Parent Phone No", "25"));
        lb_selectcolumn.Items.Add(new ListItem("Curricular", "17"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Name", "28"));
        lb_selectcolumn.Items.Add(new ListItem("Institute Address", "97"));
        lb_selectcolumn.Items.Add(new ListItem("X Medium", "98"));
        lb_selectcolumn.Items.Add(new ListItem("X11 Medium", "99"));
        lb_selectcolumn.Items.Add(new ListItem("Part1 Language", "29"));
        lb_selectcolumn.Items.Add(new ListItem("Part2 Language", "30"));
        lb_selectcolumn.Items.Add(new ListItem("Percentage", "100"));
        lb_selectcolumn.Items.Add(new ListItem("Secured Mark", "101"));
        lb_selectcolumn.Items.Add(new ListItem("Total Mark", "102"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Month", "103"));
        lb_selectcolumn.Items.Add(new ListItem("Pass Year", "104"));
        lb_selectcolumn.Items.Add(new ListItem("Vocational Stream", "105"));
        lb_selectcolumn.Items.Add(new ListItem("Mark Priority", "106"));
        lb_selectcolumn.Items.Add(new ListItem("Cut Of Mark", "107"));
        lb_selectcolumn.Items.Add(new ListItem("University Name", "31"));
        lb_selectcolumn.Items.Add(new ListItem("State", "40"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC No", "32"));
        lb_selectcolumn.Items.Add(new ListItem("Last TC Date", "33"));//delsii
        //lb_selectcolumn.Items.Add(new ListItem("12th MS", "34"));
        //lb_selectcolumn.Items.Add(new ListItem("Community Certificate No", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Provisional No", "36"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Consolidate", "35"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma-Degree", "38"));
        //lb_selectcolumn.Items.Add(new ListItem("Diploma- No of Semester", "39"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Provisional No", "40"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Consolidate", "41"));
        //lb_selectcolumn.Items.Add(new ListItem("UG-Degree", "42"));
        //lb_selectcolumn.Items.Add(new ListItem("UG- No of Semester", "43"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Provisional No", "44"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Consolidate", "45"));
        //lb_selectcolumn.Items.Add(new ListItem("PG-Degree", "46"));
        //lb_selectcolumn.Items.Add(new ListItem("PG- No of Semester", "47"));
        lb_selectcolumn.Items.Add(new ListItem("A/C No", "114"));
        lb_selectcolumn.Items.Add(new ListItem("DebitCard No", "115"));
        lb_selectcolumn.Items.Add(new ListItem("IFSCCode", "116"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Name", "117"));
        lb_selectcolumn.Items.Add(new ListItem("Bank Branch", "118"));
        lb_selectcolumn.Items.Add(new ListItem("Relative Name", "119"));
        lb_selectcolumn.Items.Add(new ListItem("RelationShip", "120"));
        lb_selectcolumn.Items.Add(new ListItem("Student/Staff", "121"));
        lb_selectcolumn.Items.Add(new ListItem("Admission Date", "36"));
        lb_selectcolumn.Items.Add(new ListItem("Enrollment Date", "37"));
        lb_selectcolumn.Items.Add(new ListItem("Join Date", "38"));
        lb_selectcolumn.Items.Add(new ListItem("CGPA", "125"));
        lb_selectcolumn.Items.Add(new ListItem("No Of Arrear", "126"));
        lb_selectcolumn.Items.Add(new ListItem("Refered By", "127"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[DD]", "128"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[MM]", "129"));
        lb_selectcolumn.Items.Add(new ListItem("Dob[YYYY]", "130"));
        lb_selectcolumn.Items.Add(new ListItem("Language", "131"));
        lb_selectcolumn.Items.Add(new ListItem("LanguageAcronym", "132"));
        lb_selectcolumn.Items.Add(new ListItem("Hall", "133"));
        lb_selectcolumn.Items.Add(new ListItem("Hall Acronym", "134"));
        string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int y = 0; y < ds.Tables[0].Rows.Count; y++)
            {
                lb_selectcolumn.Items.Add(new System.Web.UI.WebControls.ListItem(Convert.ToString(ds.Tables[0].Rows[y]["MasterValue"]), Convert.ToString(ds.Tables[0].Rows[y]["MasterCode"])));
            }
        }
    }
    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    public void selectcolumnload()
    {
        columnname = string.Empty;
        columnname1 = string.Empty;
        string linkname1 = Convert.ToString(ddl_colord.SelectedItem.Text);
        int cc = 0;
        string selcol1 = "select LinkValue from New_InsSettings where LinkName='" + linkname1 + "'  and college_code='" + ddlcollege.SelectedItem.Value + "' and user_code='" + usercode + "' ";
        ds = d2.select_method_wo_parameter(selcol1, "text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int c = 0; c < ds.Tables[0].Rows.Count; c++)
            {
                string value = Convert.ToString(ds.Tables[0].Rows[c]["LinkValue"]);
                if (value != "")
                {
                    string[] valuesplit = value.Split(',');
                    if (valuesplit.Length > 0)
                    {
                        for (int k = 0; k < valuesplit.Length; k++)
                        {
                            cc++;
                            colval = Convert.ToString(valuesplit[k]);
                            string c_name = columnload(colval);
                            string c_name1 = columnload1(colval);
                            if (c_name != "")
                            {
                                if (columnname == "")
                                {
                                    columnname = c_name;
                                }
                                else
                                {
                                    columnname = columnname + "," + c_name;
                                }
                                if (columnname1 == "")
                                {
                                    columnname1 = c_name1;
                                }
                                else
                                {
                                    columnname1 = columnname1 + "," + c_name1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    public string columnload(string v)//delsi
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "isnull( r.Sections,'') as Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "r.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "(Select Stage_Name FROM Stage_Master T WHERE Boarding = T.stage_id) Boarding";
        }
        if (colval == "123")
        {
            value = "vehid";
        }
        if (colval == "43")
        {
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")//delsii
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = "CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "128")
        {
            value = "DATEPART (day,dob) 'day'";
        }
        if (colval == "129")
        {
            value = "DATEPART(MONTH, dob) 'Month'";
        }
        if (colval == "130")
        {
            value = "DATEPART(YEAR, dob) 'Year'";
        }
        return value;
    }
    public string columnload1(string v)
    {
        string value = string.Empty;
        if (colval == "1")
        {
            value = "c.Course_Name";
        }
        if (colval == "2")
        {
            value = "Dt.Dept_Name";
        }
        if (colval == "3")
        {
            value = "a.Batch_Year";
        }
        if (colval == "4")
        {
            value = "a.Current_Semester";
        }
        if (colval == "5")
        {
            value = "a.parent_name";
        }
        if (colval == "6")
        {
            value = "CONVERT(VARCHAR(11),dob,103) as dob ";
        }
        if (colval == "7")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_occu = t.TextCode) parent_occu";
        }
        if (colval == "8")
        {
            value = "(Select textval FROM textvaltable T WHERE mother_tongue = t.TextCode) mother_tongue";
        }
        if (colval == "9")
        {
            value = "(Select textval FROM textvaltable T WHERE religion = t.TextCode) religion";
        }
        if (colval == "10")
        {
            value = "(Select textval FROM textvaltable T WHERE citizen = t.TextCode) citizen";
        }
        if (colval == "11")
        {
            value = "(Select textval FROM textvaltable T WHERE community = t.TextCode) community";
        }
        if (colval == "12")
        {
            value = "(Select textval FROM textvaltable T WHERE caste = t.TextCode) caste";
        }
        if (colval == "13")
        {
            value = "case when TamilOrginFromAndaman='0' then 'No' else 'Yes' end as  TamilOrginFromAndaman";
        }
        if (colval == "14")
        {
            value = "a.visualhandy";
        }
        if (colval == "15")
        {
            value = "a.first_graduate";
        }
        if (colval == "16")
        {
            value = "(Select textval FROM textvaltable T WHERE seattype = t.TextCode) seattype";
        }
        if (colval == "17")
        {
            value = "(Select textval FROM textvaltable T WHERE co_curricular = t.TextCode) co_curricular";
        }
        if (colval == "18")
        {
            value = "a.parent_addressP";
        }
        if (colval == "19")
        {
            value = "a.Streetp";
        }
        if (colval == "20")
        {
            value = "a.Cityp";
        }
        if (colval == "21")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statep = t.TextCode)parent_statep";
        }
        if (colval == "22")
        {
            value = "Countryp";
        }
        if (colval == "23")
        {
            value = "a.Student_Mobile";
        }
        if (colval == "24")
        {
            value = "a.parent_pincodep";
        }
        if (colval == "25")
        {
            value = "a.parent_phnop";
        }
        if (colval == "26")
        {
            value = "case when MissionaryChild='0' then 'No' else 'Yes' end as MissionaryChild";
        }
        if (colval == "27")
        {
            value = "a.missionarydisc";
        }
        //if (colval == "28")
        //{
        //    value = "Institute_name";
        //}
        //if (colval == "29")
        //{
        //    value = "Part1Language";
        //}
        //if (colval == "30")
        //{
        //    value = "Part2Language";
        //}
        //if (colval == "31")
        //{
        //    value = "university_code";
        //}
        if (colval == "34")
        {
            value = "''HostelName";
        }
        if (colval == "35")
        {
            value = "ElectionID_No";
        }
        if (colval == "48")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "49")
        {
            value = "case when handy='0' then 'No' else 'Yes' end as handy";
        }
        if (colval == "50")
        {
            value = "case when  DistinctSport='0' then 'No' else (Select textval FROM textvaltable T WHERE DistinctSport = CONVERT(nvarchar(20), t.TextCode))   end as DistinctSport";
        }
        if (colval == "51")
        {
            value = "case when islearningdis='0' then 'No' else 'Yes' end as islearningdis";
        }
        if (colval == "52")
        {
            value = "isdisabledisc";
        }
        if (colval == "53")
        {
            value = "case when isdisable='0' then 'No' else 'Yes' end as isdisable";
        }
        //if (colval == "54")
        //{
        //    value = "r.Stud_Name";
        //}
        //if (colval == "55")
        //{
        //    value = "r.Roll_No";
        //}
        if (colval == "56")
        {
            value = "StuPer_Id";
        }
        //if (colval == "57")
        //{
        //    value = "r.Reg_No";
        //}
        if (colval == "58")
        {
            value = "'' roll_admit";
        }
        if (colval == "59")
        {
            value = "app_formno";
        }
        if (colval == "60")
        {
            value = "''Sections";
        }
        if (colval == "61")
        {
            value = "case when sex='0' then 'Male' else 'Female' end as sex";
        }
        if (colval == "62")
        {
            value = "(Select textval FROM textvaltable T WHERE bldgrp = t.TextCode) bldgrp";
        }
        if (colval == "63")
        {
            value = "a.stud_type";
        }
        if (colval == "64")
        {
            value = "case when IsExService='0' then 'No' else 'Yes' end as IsExService";
        }
        if (colval == "65")
        {
            value = "case when CampusReq='0' then 'No' else 'Yes' end as CampusReq";
        }
        if (colval == "66")
        {
            value = "case when isdonar='0' then 'No' else 'Yes' end as isdonar";
        }
        if (colval == "67")
        {
            value = "case when ReserveCategory='0' then 'No' else 'Yes' end as  ReserveCategory";
        }
        if (colval == "68")
        {
            value = "case when EconBackword='0' then 'No' else 'Yes' end as EconBackword";
        }
        if (colval == "69")
        {
            value = "case when parentoldstud='0' then 'No' else 'Yes' end as parentoldstud";
        }
        if (colval == "70")
        {
            value = "case when IsDrivingLic='0' then 'No' else 'Yes' end as IsDrivingLic";
        }
        if (colval == "71")
        {
            value = "Driving_details";
        }
        if (colval == "72")
        {
            value = "case when tutionfee_waiver='0' then 'No' else 'Yes' end as tutionfee_waiver";
        }
        if (colval == "73")
        {
            value = "case when IsInsurance='0' then 'No' else 'Yes' end as IsInsurance";
        }
        if (colval == "74")
        {
            value = "ExsRank";
        }
        if (colval == "75")
        {
            value = "ExSPlace";
        }
        if (colval == "76")
        {
            value = "ExsNumber";
        }
        if (colval == "77")
        {
            value = "Insurance_Amount";
        }
        if (colval == "78")
        {
            value = "Insurance_InsBy";
        }
        if (colval == "79")
        {
            value = "Insurance_Nominee";
        }
        if (colval == "80")
        {
            value = "Insurance_NominRelation";
        }
        if (colval == "81")
        {
            value = "CONVERT(VARCHAR(11),date_applied,103) as date_applied";
        }
        if (colval == "82")
        {
            value = "alter_mobileno";
        }
        if (colval == "83")
        {
            //magesh 29/1/18
            value = "(Select textval FROM textvaltable T WHERE SubCaste = t.TextCode) SubCaste";
            // value = "SubCaste";
        }
        if (colval == "84")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_income = t.TextCode) parent_income";
        }
        if (colval == "85")
        {
            value = "parentF_Mobile";
        }
        if (colval == "86")
        {
            value = "emailp";
        }
        if (colval == "87")
        {
            value = "mother";
        }
        if (colval == "88")
        {
            value = "(Select textval FROM textvaltable T WHERE mIncome = t.TextCode) mIncome";
        }
        if (colval == "89")
        {
            value = "parentM_Mobile";
        }
        if (colval == "90")
        {
            value = "emailM";
        }
        if (colval == "91")
        {
            value = "guardian_name";
        }
        if (colval == "92")
        {
            value = "guardian_mobile";
        }
        if (colval == "93")
        {
            value = "emailg";
        }
        if (colval == "94")
        {
            value = "place_birth";
        }
        if (colval == "95")
        {
            value = "Aadharcard_no";
        }
        if (colval == "96")
        {
            value = "(Select textval FROM textvaltable T WHERE motherocc = t.TextCode) motherocc";
        }
        if (colval == "108")
        {
            value = "a.parent_addressC";
        }
        if (colval == "109")
        {
            value = "a.Streetc";
        }
        if (colval == "110")
        {
            value = "a.Cityc";
        }
        if (colval == "111")
        {
            value = "(Select textval FROM textvaltable T WHERE parent_statec = t.TextCode) parent_statec";
        }
        if (colval == "112")
        {
            value = "Countryc";
        }
        if (colval == "113")
        {
            value = "a.parent_pincodec";
        }
        if (colval == "122")
        {
            value = "'' Boarding";
        }
        if (colval == "123")
        {
            value = "''vehid";
        }
        if (colval == "43")
        {
            // value = "case when r.Mode='1' then 'Regular' when r.mode='2' then 'Transfer' when r.mode='3' then 'Lateral' end Mode ";
            value = "case when a.Mode='1' then 'Regular' when a.mode='2' then 'Transfer' when a.mode='3' then 'Lateral' end Mode ";
        }
        if (colval == "36")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "37")
        {
            value = " CONVERT(varchar(10), a.enrollment_confirm_date,103)enrollment_confirm_date";
        }
        if (colval == "38")
        {
            value = "CONVERT(varchar(10), r.Adm_Date,103)Adm_Date";
        }
        if (colval == "128")
        {
            value = "DATEPART (day,dob) 'day'";
        }
        if (colval == "129")
        {
            value = "DATEPART(MONTH, dob) 'Month'";
        }
        if (colval == "130")
        {
            value = "DATEPART(YEAR, dob) 'Year'";
        }
        return value;//delsii
    }
    protected void imagebtnpopclose1_Click(object sender, EventArgs e)
    {
        poppernew.Visible = false;
    }
    public void btn_addtype_OnClick(object sender, EventArgs e)
    {
        imgdiv33.Visible = true;
        panel_description11.Visible = true;
    }
    public void btn_deltype_OnClick(object sender, EventArgs e)
    {
        if (ddl_coltypeadd.SelectedIndex == -1)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
        else if (ddl_coltypeadd.SelectedIndex == 0)
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "Select any record";
        }
        else if (ddl_coltypeadd.SelectedIndex != 0)
        {
            string sql = "delete from CO_MasterValues where MasterCode='" + ddl_coltypeadd.SelectedItem.Value.ToString() + "' and MasterCriteria='StudentPlacementDetails' and CollegeCode='" + ddlcollege.SelectedItem.Value + "' ";
            int delete = d2.update_method_wo_parameter(sql, "TEXT");
            if (delete != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Deleted Sucessfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "No records found";
            }
            columnordertype();
        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "No records found";
        }
    }
    public void columnordertype()
    {
        ddl_colord.Items.Clear();
        ddl_coltypeadd.Items.Clear();
        string query = "select MasterCode,MasterValue from CO_MasterValues where MasterCriteria='StudentPlacementDetails' and CollegeCode='" + ddlcollege.SelectedItem.Value + "'";
        ds.Clear();
        ds = d2.select_method_wo_parameter(query, "Text");
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddl_colord.DataSource = ds;
            ddl_colord.DataTextField = "MasterValue";
            ddl_colord.DataValueField = "MasterCode";
            ddl_colord.DataBind();
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.DataSource = ds;
            ddl_coltypeadd.DataTextField = "MasterValue";
            ddl_coltypeadd.DataValueField = "MasterCode";
            ddl_coltypeadd.DataBind();
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
        else
        {
            ddl_colord.Items.Insert(0, new ListItem("Select", "0"));
            ddl_coltypeadd.Items.Insert(0, new ListItem("Select", "0"));
        }
    }

    public void ddl_coltypeadd_SelectedIndexChanged(object sender, EventArgs e)
    {
        viewcolumorder();
    }
    public void viewcolumorder()
    {
        try
        {
            lb_column1.Items.Clear();
            if (ddl_coltypeadd.SelectedItem.Text != "Select")
            {
                string q = "select LinkValue from New_InsSettings where LinkName='" + ddl_coltypeadd.SelectedItem.Text + "' and college_code='" + ddlcollege.SelectedItem.Value + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(q, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string vall = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
                    string[] sp = vall.Split(',');
                    for (int y = 0; y < sp.Length; y++)
                    {
                        colval = sp[y];
                        loadtext();
                        lb_column1.Items.Add(new System.Web.UI.WebControls.ListItem(loadval, Convert.ToString(sp[y])));
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void btndescpopadd_Click(object sender, EventArgs e)
    {
        if (txt_description11.Text != "")
        {
            string sql = "if exists ( select * from CO_MasterValues where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentPlacementDetails' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "') update CO_MasterValues set MasterValue ='" + txt_description11.Text + "' where MasterValue ='" + txt_description11.Text + "' and MasterCriteria ='StudentPlacementDetails' and CollegeCode ='" + ddlcollege.SelectedItem.Value + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values ('" + txt_description11.Text + "','StudentPlacementDetails','" + ddlcollege.SelectedItem.Value + "')";
            int insert = d2.update_method_wo_parameter(sql, "TEXT");
            if (insert != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Text = "Added sucessfully";
                txt_description11.Text = string.Empty;
                //imgdiv33.Visible = false;           
            }
        }
        else
        {
            imgdiv2.Visible = true;
            pnl2.Visible = true;
            lbl_alert.Text = "Enter the description";
        }
        columnordertype();
    }
    public void btndescpopexit_Click(object sender, EventArgs e)
    {
        panel_description11.Visible = false;
        imgdiv33.Visible = false;
    }

    protected void without_CheckedChanged(object sender, EventArgs e)
    {
        if (rdb_cumm.Checked == true)
        {
            arrear.Visible = false;
            noarrear.Visible = true;
            Td1.Visible = true;
            Txtrange.Text = "";
            Txtto.Text = "";
        }
        else
        {
            arrear.Visible = true;
            noarrear.Visible = false;
            Td1.Visible = false;
            Txtrange.Text = "";
            Txtto.Text = "";
        }
    }
    protected void ddrtosem_selected(object sender, EventArgs e)
    {
        string fromsem = ddrsem.SelectedItem.Text;
        string tosem = ddrtosem.SelectedItem.Text;
        int fromsemes = 0;
        int tosemester = 0;
        int.TryParse(fromsem, out fromsemes);
        int.TryParse(tosem, out tosemester);
        if (fromsemes < tosemester)
        {

        }
        else
        {
            imgdiv2.Visible = true;
            lbl_alert.Text = "From Semester less than To Semester";
            gview.Visible = false;
        }
     
    }
    //public void bindsem1()
    //{
    //    try
    //    {
    //        //--------------------semester load
    //        DataSet ds3 = new DataSet();
    //        ddrsem.Items.Clear();
    //        ddrtosem.Items.Clear();
    //        Boolean first_year;
    //        first_year = false;
    //        int duration = 0;
    //        int i = 0;
    //        string build = string.Empty;
    //        string branch = string.Empty;
    //        string batch = string.Empty;
    //        string batch_year = string.Empty;
    //        string sqluery = string.Empty;
    //        if (cbl_branch.Items.Count > 0)
    //        {
    //            for (int j = 0; j < cbl_branch.Items.Count; j++)
    //            {
    //                if (cbl_branch.Items[j].Selected == true)
    //                {
    //                    build = cbl_branch.Items[j].Value.ToString();
    //                    if (branch == "")
    //                    {
    //                        branch = build;
    //                    }
    //                    else
    //                    {
    //                        branch = branch + "," + build;
    //                    }
    //                }
    //            }
    //        }
    //        if (cbl_batch.Items.Count > 0)
    //        {
    //            for (int j = 0; j < cbl_batch.Items.Count; j++)
    //            {
    //                if (cbl_batch.Items[j].Selected == true)
    //                {
    //                    batch = cbl_batch.Items[j].Value.ToString();
    //                    if (batch_year == "")
    //                    {
    //                        batch_year = batch;
    //                    }
    //                    else
    //                    {
    //                        batch_year = batch_year + "," + batch;
    //                    }
    //                }
    //            }
    //        }
    //        if (branch.Trim() != "" && batch_year!="")
    //        {
    //             sqluery = "select distinct ndurations,first_year_nonsemester from ndegree where college_code=" + ddlcollege.SelectedValue + " and degree_code in(" + branch + ") and batch_year in (" + batch_year + ") ";
    //            ds3 = d2.select_method_wo_parameter(sqluery, "text");

    //            if (ds3.Tables[0].Rows.Count > 0)
    //            {
    //                first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
    //                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);
    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {

    //                        ddrsem.Items.Add(Convert.ToString(i));
    //                        ddrtosem.Items.Add(Convert.ToString(i));
                            

                            
    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {

    //                        ddrsem.Items.Add(Convert.ToString(i));
    //                        ddrtosem.Items.Add(Convert.ToString(i));

    //                        //ddrsem.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), Convert.ToString(i)));
    //                        //ddrtosem.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), Convert.ToString(i)));
                         
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            sqluery = "select distinct duration,first_year_nonsemester  from degree where college_code=" +ddlcollege.SelectedValue+ "";
    //            ddrsem.Items.Clear();
    //            ds3 = d2.select_method_wo_parameter(sqluery, "text");
    //            if (ds3.Tables[0].Rows.Count > 0)
    //            {
    //                first_year = Convert.ToBoolean(ds3.Tables[0].Rows[0]["first_year_nonsemester"]);
    //                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["duration"]);
    //                for (i = 1; i <= duration; i++)
    //                {
    //                    if (first_year == false)
    //                    {

    //                        ddrsem.Items.Add(Convert.ToString(i));
    //                        ddrtosem.Items.Add(Convert.ToString(i));

    //                    }
    //                    else if (first_year == true && i != 2)
    //                    {

    //                        ddrsem.Items.Add(Convert.ToString(i));
    //                        ddrtosem.Items.Add(Convert.ToString(i));

    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch
    //    {
    //    }
    //}

    public string Calculete_CGPA(string RollNo, string fromsem_tagvalue, string tosem_tagvalue, string degree_code, string batch_year, string latmode, string collegecode, bool transferflag = false)
    {
        string calculate = "";
        bool flag = true;
        try
        {
            int jvalue = 0;
            string strgrade = "";
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            double gpacal1 = 0;
            string strsubcrd = "";
            int gtempejval = 0;
            string syll_code = "";
            string examcodevalg = "";
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            double strtot = 0, inte = 0, exte = 0;
            double strgradetempfrm = 0;
            double strgradetempto = 0;
            string strgradetempgrade = "";
            string strtotgrac = "";
            string sqlcmdgraderstotal = "";
            int attemptswith = 0;
            string strattmaxmark = "";
            int attmpt = 0, maxmark = 0;
            strattmaxmark = d2.GetFunction("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
            string[] semecount = strattmaxmark.Split(new Char[] { '-' });
            if (semecount.GetUpperBound(0) == 1)
            {
                attmpt = Convert.ToInt32(semecount[0].ToString());
                maxmark = Convert.ToInt32(semecount[1].ToString());
                flag = true;
            }
            else
            {
                flag = false;
            }
            sqlcmdgraderstotal = " select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            dggradetot =d2.select_method(sqlcmdgraderstotal, hat, "Text");
            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            if (!transferflag) //modified by prabha feb 10 2018
                strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester between '"+fromsem_tagvalue+"' and '"+tosem_tagvalue+"' ) ";

            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester between '"+fromsem_tagvalue+"' and '"+tosem_tagvalue+"' AND UPPER(Result) ='PASS' ";
            if (strsubcrd != null && strsubcrd != "")
            {
              
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
               
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != string.Empty) && (dr_subcrd["total"].ToString() != "0"))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                inte = Convert.ToDouble(dr_subcrd["internal_mark"].ToString());
                                exte = Convert.ToDouble(dr_subcrd["external_mark"].ToString());
                                attemptswith = Convert.ToInt32(dr_subcrd["attempts"].ToString());
                                if (flag == true)
                                {
                                    if (attmpt > attemptswith)//ATTEMPTS compared with attempts in coe settings if attempts lower than coe settings
                                    {
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        inte = 0;
                                        strtot = exte;// total only consider extermarks only
                                        foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                        {
                                            if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                            {
                                                strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                                strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                                if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                                {
                                                    strgrade = gratemp["credit_points"].ToString();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    strtot = Convert.ToDouble(dr_subcrd["total"].ToString());
                                    foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                    {
                                        if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                        {
                                            strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                            strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                            if (strgradetempfrm <= strtot && strgradetempto >= strtot)
                                            {
                                                strgrade = gratemp["credit_points"].ToString();
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if ((dr_subcrd["grade"].ToString() != string.Empty))
                        {
                            if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                            {
                                strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());
                                //magesh 23/2/18
                                strgrade = "";
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    strgradetempgrade = Convert.ToString(gratemp["mark_grade"].ToString());
                                    if (strgradetempgrade.ToString().Trim() == strtotgrac.ToString().Trim())
                                    {
                                        strgrade = gratemp["credit_points"].ToString();
                                        break;
                                    }
                                }
                            }
                        }
                        creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        if (creditsum1 == 0)
                        {
                            creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        else
                        {
                            creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                        }
                        if (gpacal1 == 0)
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = Convert.ToDouble(strgrade) * creditval;
                            }
                        }
                        else
                        {
                            if (strgrade != "")
                            {
                                gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                            }
                        }
                    }
                }
            }
            creditval = 0;
            strgrade = "";
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
            calculate = Convert.ToString(finalgpa1);
            creditsum1 = 0;
            gpacal1 = 0;
            finalgpa1 = 0;
        }
        catch (Exception vel)
        {
            string exce = vel.ToString();
        }
        if (calculate == "NaN")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }
    public void bindsem1()
    {
        

      
           
            int duration = 0;
            int i = 0;
          
            string sqluery = string.Empty;
            sqluery = "select max(ndurations) as ndurations from ndegree where college_code=" + ddlcollege.SelectedValue + "";
           DataSet ds3 = d2.select_method_wo_parameter(sqluery, "text");

            if (ds3.Tables[0].Rows.Count > 0)
            {
              
                duration = Convert.ToInt16(ds3.Tables[0].Rows[0]["ndurations"]);

                for (int m = 1; m < duration;m++)
                {

                    ddrsem.Items.Add(Convert.ToString(m));
                    ddrtosem.Items.Add(Convert.ToString(m));
                }



            }
        }
    

        
}