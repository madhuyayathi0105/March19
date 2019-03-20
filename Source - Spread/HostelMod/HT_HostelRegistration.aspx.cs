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
using Gios.Pdf;
using System.IO;
public partial class HT_HostelRegistration : System.Web.UI.Page
{
    string user_code;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    Boolean Cellclick = false;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string collegecode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string Rollflag1 = string.Empty;
    string Regflag1 = string.Empty;
    string Studflag1 = string.Empty;
    string college_code = "";
    string college = "";
    string course_id = string.Empty;
    static string Hostelcode = "";
    static string hosname = "";
    DataSet ds2 = new DataSet();
    DataSet ds3 = new DataSet();
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();
    ReuasableMethods rs = new ReuasableMethods();
    static string query = "";
    int count = 0;
    string sqladd = "";
    static string mm = "";
    static string cln = "";
    string build = "";
    string floor = "";
    string room = "";
    string[] fr;
    string[] address;
    string buildvalue1 = "";
    string build1 = "";
    string buildvalue2 = "";
    string build2 = "";
    string buildvalue3 = "";
    string build3 = "";
    string buildvalue4 = "";
    string build4 = "";
    string buildvalue6 = "";
    string build6 = "";
    string buildvalue7 = "";
    string build7 = "";
    string builldvalue5 = "";
    string builld5 = "";
    string buildvalue8 = "";
    string build8 = "";
    string batch = "";
    string batchval = "";
    string grouporusercode = "";
    string[] datesp;
    int i = 0;
    private EventArgs e;
    private object sender;
    static string statichostelfk = "";
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
        hat.Add("single_user", singleuser);
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        //31.10.15 barath
        if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
        {
            grouporusercode = " group_code=" + Session["group_code"].ToString().Trim() + "";
        }
        else
        {
            grouporusercode = " usercode=" + Session["usercode"].ToString().Trim() + "";
        }
        //31.10.15 barath
        string Master = "select * from Master_Settings where " + grouporusercode + "";//31.10.15 barath
        DataSet ds = d2.select_method(Master, hat, "Text");
        if (ds.Tables.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Rollflag"] = "1";
                    Rollflag1 = Session["Rollflag"].ToString();
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Regflag"] = "1";
                    Regflag1 = Session["Regflag"].ToString();
                }
                if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                {
                    Session["Studflag"] = "1";
                    Studflag1 = Session["Studflag"].ToString();
                }
            }
        }
        caladmin.EndDate = DateTime.Now;
        //calvacatedate.EndDate = DateTime.Now;
        caldisdate.EndDate = DateTime.Now;
        CalendarExtender3.EndDate = DateTime.Now;
        cext_fromdate.EndDate = DateTime.Now;
        cext_todate.StartDate = DateTime.Now;
        CalendarExtender1.StartDate = DateTime.Now;
        if (statichostelfk.Trim() == "")
        {
            try
            {
                ds3.Clear();
                string q1 = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
                ds3 = d2.select_method_wo_parameter(" select HostelMasterPK from HM_HostelMaster where MessMasterFK in(" + q1 + ")", "Text");
                if (ds3.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        if (statichostelfk == "")
                        {
                            statichostelfk = Convert.ToString(ds3.Tables[0].Rows[i][0]);
                        }
                        else
                        {
                            statichostelfk = statichostelfk + "','" + Convert.ToString(ds3.Tables[0].Rows[i][0]);
                        }
                    }
                }
            }
            catch { }
        }
        if (!IsPostBack)
        {
            bindcollege();
            bindpop2collegename();
            bindhostel();
            txt_vacate.Enabled = false;
            btn_pop1update.Visible = false;
            txt_batch.Enabled = false;
            txt_branch.Enabled = false;
            txt_degree.Enabled = false;
            clgroomtype(floor, room);
            degree();
            cb_hostelname.Checked = true;
            cb_hostelname_checkedchange(sender, e);
            cb_hostelname_checkedchange(sender, e);
            div1.Visible = false;
            bindbatch();
            bindbranch(college);
            bindpop2hostel();
            bindpop2degree();
            bindpop2batchyear();
            bindcommunity();
            Hostelcode = "";
            bindpop1college();
            bindmessmaster();
            loaddate();
            loadyear();
            //btn_go_Click(sender, e);
            fproll.Sheets[0].AutoPostBack = false;
            fproll.Sheets[0].RowCount = 0;
            fproll.Visible = false;
            FpSpread3.Visible = false;
            FpSpread3.Sheets[0].RowCount = 0;
            FpSpread3.Sheets[0].ColumnCount = 0;
            //Radiobtnstype.Items[0].Selected = true;
            idgeneration();
            lblerr.Visible = false;
            txt_discontinuedate.Enabled = false;
            txt_vacatedate.Enabled = false;
            txt_pop1reason.Enabled = false;
            txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfrmdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txttodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtfrmdate.Attributes.Add("ReadOnly", "readonly");
            txttodate.Attributes.Add("ReadOnly", "readonly");
            txt_pop1admindate.Attributes.Add("ReadOnly", "readonly");
            txt_pop1roomtype.Attributes.Add("ReadOnly", "readonly");
            ddl_pop2sex.Items.Add(new ListItem("All", "0"));
            ddl_pop2sex.Items.Add(new ListItem("Male", "1"));
            ddl_pop2sex.Items.Add(new ListItem("Female", "2"));
            ddl_pop2sex.Items.Add(new ListItem("Transgender", "3"));
            //ddlpop1dm.Items.Add(new ListItem("Select", "0"));
            ddl_pop1month.Items.Add(new ListItem("January", "01"));
            ddl_pop1month.Items.Add(new ListItem("February", "02"));
            ddl_pop1month.Items.Add(new ListItem("March", "03"));
            ddl_pop1month.Items.Add(new ListItem("April", "04"));
            ddl_pop1month.Items.Add(new ListItem("May", "05"));
            ddl_pop1month.Items.Add(new ListItem("June", "06"));
            ddl_pop1month.Items.Add(new ListItem("July", "07"));
            ddl_pop1month.Items.Add(new ListItem("August", "08"));
            ddl_pop1month.Items.Add(new ListItem("September", "09"));
            ddl_pop1month.Items.Add(new ListItem("October", "10"));
            ddl_pop1month.Items.Add(new ListItem("November", "11"));
            ddl_pop1month.Items.Add(new ListItem("December", "12"));
            Fpspread1.Sheets[0].RowCount = 0;
            Fpspread1.Sheets[0].ColumnCount = 0;
            toalrooms.Visible = false;
            totalvaccants.Visible = false;
            fill.Visible = false;
            partialfill.Visible = false;
            unfill.Visible = false;
            tblStatus.Visible = false;
            Fpspread1.Visible = false;
            btn_pop1delete.Visible = false;
            btn_pop1exit1.Visible = false;
            txt_trhosdate.Attributes.Add("readonly", "readonly");
            txt_trhosdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_rejoindate.Attributes.Add("readonly", "readonly");
            txt_rejoindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_vacate.Attributes.Add("readonly", "readonly");
            txt_vacate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            ViewState["buil"] = null;
            ViewState["fl"] = null;
            ViewState["ro"] = null;
            BindStudentType();
        }
        lblpop3err.Visible = false;
        errmsg.Visible = false;
        lbl_errmsg.Visible = false;
    }
    public void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }
    //main page
    protected void bindcollege()
    {
        try
        {
            ds.Clear();
            //ddl_college.Items.Clear();
            string query = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_college.DataSource = ds;
                //ddl_college.DataTextField = "collname";
                //ddl_college.DataValueField = "college_code";
                //ddl_college.DataBind();
                cbl_clg.DataSource = ds;
                cbl_clg.DataTextField = "collname";
                cbl_clg.DataValueField = "college_code";
                cbl_clg.DataBind();
                int count = 0;
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                    if (cbl_clg.Items[i].Selected == true)
                    {
                        txt_college.Text = "College(" + Convert.ToString(cbl_clg.Items.Count) + ")";
                        cb_clg.Checked = true;
                    }
                }
            }
        }
        catch
        {
        }
    }
    //public void bindclg(string college)
    //{
    //    try
    //    {
    //        string clgname = "select college_code,collname from collinfo ";
    //        if (clgname != "")
    //        {
    //            ds = d2.select_method(clgname, hat, "Text");
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                ddl_college.DataSource = ds;
    //                ddl_college.DataTextField = "collname";
    //                ddl_college.DataValueField = "college_code";
    //                ddl_college.DataBind();
    //                ddl_college.Items.Insert(0, "--Select--");
    //                cln = ddl_college.SelectedValue;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //    }
    //}
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_degree.Text = "--Select--";
        txt_branch.Text = "--Select--";
        bindhostel();
        degree();
        bindbranch(college);
    }
    protected void bindhostel()
    {
        try
        {
            //cbl_hostelname.Items.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            ////magesh 21.6.18
            //MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            //ds = d2.select_method_wo_parameter(MessmasterFK,"text");
            cbl_hostelname.Items.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //magesh 21.6.18
            //  MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_hostelname.DataSource = ds;
                cbl_hostelname.DataTextField = "HostelName";
                cbl_hostelname.DataValueField = "HostelMasterPK";
                cbl_hostelname.DataBind();
                mm = cbl_hostelname.SelectedValue;
            }
            else
            {
                // cbl_hostelname.Items.Insert(0, "--Select--");
                txt_hostelname.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindmessmaster()
    {
        try
        {
            string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK='" + ddl_pop1hostelname.SelectedValue + "'");
            //string selectQuery1 =d2.GetFunction("select MessMasterPK from HM_MessMaster where MessMasterPK in(" + selectQuery + ") order by MessMasterPK asc");
            string[] spl = selectQuery.Split('-');
            if (spl.Length > 0)
            {
                string typ = string.Empty;
                if (spl.Count() > 0)
                {
                    for (int i = 0; i < spl.Count(); i++)
                    {
                        if (typ == "")
                        {
                            typ = "" + spl[i] + "";
                        }
                        else
                        {
                            typ = typ + "'" + "," + "'" + spl[i] + "";
                        }

                    }

                }
                selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
            }

            ds = d2.select_method_wo_parameter(selectQuery, "text");
            // ddl_messmaster.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                //magesh 20.6.18
                ddlmess.DataSource = ds;
                ddlmess.DataTextField = "MessName";
                ddlmess.DataValueField = "MessMasterPK";
                ddlmess.DataBind();
            }
            else
            {
                ddlmess.Items.Insert(0, "");
            }
            //    ddl_messmaster.DataSource = ds;
            //    ddl_messmaster.DataTextField = "MessName";
            //    ddl_messmaster.DataValueField = "MessMasterPK";
            //    ddl_messmaster.DataBind();
            //}
            //ddl_messmaster.Items.Insert(0, "Select");
        }
        catch
        {
            //ddl_messmaster.Items.Clear();
        }
    }
    //magesh 21.6.18
    public void ddl_pop1hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == false)
            {

                idgeneration();
            }
            bindmessmaster();
        }
        catch
        {
        }
    }
    public void cb_hostelname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_buildingname.Text = "--Select--";
            txt_floorname.Text = "--Select--";
            txt_roomname.Text = "--Select--";
            if (cb_hostelname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                if (cb_hostelname.Checked == true)
                {
                    for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                    {
                        if (cb_hostelname.Checked == true)
                        {
                            cbl_hostelname.Items[i].Selected = true;
                            txt_hostelname.Text = "Hostel(" + (cbl_hostelname.Items.Count) + ")";
                            build1 = cbl_hostelname.Items[i].Value.ToString();
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
                    Hostelcode = buildvalue1;
                    clgbuild(buildvalue1);
                }
            }
            else
            {
                for (int i = 0; i < cbl_hostelname.Items.Count; i++)
                {
                    cbl_hostelname.Items[i].Selected = false;
                    txt_hostelname.Text = "--Select--";
                    cbl_buildname.ClearSelection();
                    cbl_floorname.ClearSelection();
                    cbl_roomname.ClearSelection();
                    cb_buildname.Checked = false;
                    cb_floorname.Checked = false;
                    cb_roomname.Checked = false;
                }
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_hostelname_SelectedIndexChanged(object sender, EventArgs e)
    {
        int i = 0;
        cb_hostelname.Checked = false;
        int commcount = 0;
        string buildvalue = "";
        string build = "";
        txt_hostelname.Text = "--Select--";
        for (i = 0; i < cbl_hostelname.Items.Count; i++)
        {
            if (cbl_hostelname.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                cb_hostelname.Checked = false;
                ///new 22/08/15
                build = cbl_hostelname.Items[i].Value.ToString();
                if (buildvalue == "")
                {
                    buildvalue = build;
                }
                else
                {
                    buildvalue = buildvalue + "'" + "," + "'" + build;
                }
                clgbuild(buildvalue);
                Hostelcode = buildvalue;
            }
        }
        if (commcount > 0)
        {
            if (commcount == cbl_hostelname.Items.Count)
            {
                cb_hostelname.Checked = true;
            }
            txt_hostelname.Text = "Hostel Name(" + commcount.ToString() + ")";
        }
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = "";
        //query = "select  r.Stud_Name from Registration r, HT_HostelRegistration h where r.App_No=h.App_No and r.Stud_Name like '" + prefixText + "%'  order by r.Stud_Name";
        query = " select r.Stud_Name from Registration r, HT_HostelRegistration h where r.App_No=h.App_No and  r.Stud_Name like '" + prefixText + "%' and HostelMasterFK in('" + statichostelfk + "') order by Stud_Name";
        //AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0'
        name = ws.Getname(query);
        return name;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll(string prefixText)
    {
        WebService ws = new WebService();
        List<string> roll = new List<string>();
        query = " select distinct top 10 r.Roll_No from HT_HostelRegistration h,Registration r where r.App_No=h.APP_No and r.Roll_No like '" + prefixText + "%' and HostelMasterFK in('" + statichostelfk + "') order by Roll_No ";
        //AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0'
        roll = ws.Getname(query);
        return roll;
    }
    public void cb1_CheckedChanged(object sender, EventArgs e)
    {
        if (cb1.Checked == true)
        {
            txt_batch.Enabled = true;
            txt_branch.Enabled = true;
            txt_degree.Enabled = true;
        }
        else if (cb1.Checked == false)
        {
            txt_batch.Enabled = false;
            txt_branch.Enabled = false;
            txt_degree.Enabled = false;
        }
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
            }
        }
        catch
        {
        }
    }
    public void cb_batch_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_batch.Checked == true)
            {
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
            txt_batch.Text = "--Select--";
            cb_batch.Checked = false;
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void degree()
    {
        try
        {
            user_code = Session["usercode"].ToString();
            //college_code = Session["collegecode"].ToString();
            college_code = rs.GetSelectedItemsValueAsString(cbl_clg); //Convert.ToString(ddl_college.SelectedItem.Value);
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            //hat.Clear();
            //hat.Add("single_user", singleuser.ToString());
            //hat.Add("group_code", group_user);
            //hat.Add("college_code", college_code);
            //hat.Add("user_code", user_code);
            //ds.Clear();
            //ds = d2.select_method("bind_degree", hat, "sp");
            string q1 = "";
            if (!string.IsNullOrEmpty(group_user) && group_user != "0")
            {
                q1 = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in('" + college_code + "') and deptprivilages.Degree_code=degree.Degree_code and group_code='" + group_user + "'";
            }
            else
            {
                q1 = " select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in('" + college_code + "') and deptprivilages.Degree_code=degree.Degree_code and user_code='" + user_code + "' ";
            }
            ds = d2.select_method_wo_parameter(q1, "text");
            int count1 = ds.Tables[0].Rows.Count;
            ddl_pop2degre.Items.Clear();
            cbl_degree.Items.Clear();
            if (count1 > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                //for (int i = 0; i < cbl_build.Items.Count; i++)
                //{
                //    cbl_degree.Items[i].Selected = true;
                //    txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
                //    cb_degree.Checked = true;
                //}
                ddl_pop2degre.DataSource = ds;
                ddl_pop2degre.DataTextField = "course_name";
                ddl_pop2degre.DataValueField = "course_id";
                ddl_pop2degre.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
            string buildvalue1 = "";
            string build1 = "";
            if (cb_degree.Checked == true)
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    if (cb_degree.Checked == true)
                    {
                        cbl_degree.Items[i].Selected = true;
                        txt_degree.Text = "Degree(" + (cbl_degree.Items.Count) + ")";
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
            // Button2.Focus();
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
            string buildvalue = "";
            string build = "";
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
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
                cb_degree.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_degree.Text = "--Select--";
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_degree.Text = "Degree(" + seatcount.ToString() + ")";
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
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
            }
            {
                ds = d2.select_method(commname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_branch.DataSource = ds;
                    cbl_branch.DataTextField = "dept_name";
                    cbl_branch.DataValueField = "degree_code";
                    cbl_branch.DataBind();
                    ddl_pop2branch.DataSource = ds;
                    ddl_pop2branch.DataTextField = "dept_name";
                    ddl_pop2branch.DataValueField = "degree_code";
                    ddl_pop2branch.DataBind();
                }
            }
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
        }
        catch (Exception ex)
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
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else if (commcount == 0)
            {
                txt_degree.Text = "--Select--";
            }
            else
            {
                txt_branch.Text = "Branch(" + commcount.ToString() + ")";
            }
            // Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgbuild(string hostelname)
    {
        try
        {
            cbl_buildname.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(hostelname);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_buildname.DataSource = ds;
                cbl_buildname.DataTextField = "Building_Name";
                cbl_buildname.DataValueField = "code";
                cbl_buildname.DataBind();
            }
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                cbl_buildname.Items[i].Selected = true;
                txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                cb_buildname.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    string builname = cbl_buildname.Items[i].Text;
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
    protected void cb_buildname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_buildname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    if (cb_buildname.Checked == true)
                    {
                        cbl_buildname.Items[i].Selected = true;
                        txt_buildingname.Text = "Building(" + (cbl_buildname.Items.Count) + ")";
                        //txt_floorname.Text = "--Select--";
                        build1 = cbl_buildname.Items[i].Text.ToString();
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
                for (int i = 0; i < cbl_buildname.Items.Count; i++)
                {
                    cbl_buildname.Items[i].Selected = false;
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
    protected void cbl_buildname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_buildname.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    // txt_floorname.Text = "--Select--";
                    cb_floorname.Checked = true;
                    build = cbl_buildname.Items[i].Text.ToString();
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
            if (seatcount == cbl_buildname.Items.Count)
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
                cb_buildname.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_buildingname.Text = "--Select--";
            }
            else
            {
                txt_buildingname.Text = "Building(" + seatcount + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void clgfloor(string buildname)
    {
        try
        {
            cbl_floorname.Items.Clear();
            //ds = d2.BindFloor_new(buildname);
            string itemname = "select distinct Floor_Name,FloorPK from Floor_Master where Building_Name in('" + buildname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
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
                    string flrname = cbl_floorname.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
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
    protected void cb_floorname_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_floorname.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_buildname.Checked == true)
                {
                    for (int i = 0; i < cbl_buildname.Items.Count; i++)
                    {
                        build1 = cbl_buildname.Items[i].Text.ToString();
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
    protected void cbl_floorname_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floorname.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_buildname.Items.Count; i++)
            {
                if (cbl_buildname.Items[i].Selected == true)
                {
                    build1 = cbl_buildname.Items[i].Text.ToString();
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
            //ds = d2.BindRoom(floorname, buildname);changed at sairam 29.09.16//11.04.17 barath
            string itemname = "select Room_Name,Roompk from Room_Detail where Building_Name in('" + buildname + "') and floor_name in('" + floorname + "') order by (len(Room_Name)) asc,Room_Name";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
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
            string room = "";
            for (int i = 0; i < cbl_roomname.Items.Count; i++)
            {
                if (cbl_roomname.Items[i].Selected == true)
                {
                    string flrname = cbl_roomname.Items[i].Text;
                    if (room == "")
                    {
                        room = flrname;
                    }
                    else
                    {
                        room = room + "'" + "," + "'" + flrname;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_roomname_checkedchange(object sender, EventArgs e)
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
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_sex_checkedchange(object sender, EventArgs e)
    {
        if (cb_sex.Checked == true)
        {
            for (int i = 0; i < cbl_sex.Items.Count; i++)
            {
                cbl_sex.Items[i].Selected = true;
            }
            txt_sex.Text = "Gender(" + cbl_sex.Items.Count + ")";
        }
        else
        {
            for (int i = 0; i < cbl_sex.Items.Count; i++)
            {
                cbl_sex.Items[i].Selected = false;
            }
            txt_sex.Text = "--Select--";
        }
    }
    public void cbl_sex_SelectedIndexChanged(object sender, EventArgs e)
    {
        txt_sex.Text = "--Select--";
        cb_sex.Checked = false;
        int ccount = 0;
        for (int i = 0; i < cbl_sex.Items.Count; i++)
        {
            if (cbl_sex.Items[i].Selected == true)
            {
                ccount = ccount + 1;
                cb_sex.Checked = false;
            }
        }
        if (ccount > 0)
        {
            txt_sex.Text = "Gender(" + ccount.ToString() + ")";
            if (ccount == cbl_sex.Items.Count)
            {
                cb_sex.Checked = true;
            }
        }
    }
    public void bindcommunity()
    {
        try
        {
            string college = rs.GetSelectedItemsValueAsString(cbl_clg);// ddl_college.SelectedValue;
            string commname = "select distinct textcode,textval from applyn a,textvaltable t  where textval <> '' and a.community = t.TextCode and t.college_code in('" + college + "')";
            {
                ds = d2.select_method_wo_parameter(commname, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_community.DataSource = ds;
                    cbl_community.DataTextField = "textval";
                    cbl_community.DataValueField = "textcode";
                    cbl_community.DataBind();
                    //if (cbl_community.Items.Count > 0)
                    //{
                    //    for (int i = 0; i < cbl_community.Items.Count; i++)
                    //    {
                    //        cbl_community.Items[i].Selected = true;
                    //    }
                    //    txt_community.Text = "Community(" + cbl_community.Items.Count + ")";
                    //}
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_community_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_community.Checked == true)
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = true;
                }
                txt_community.Text = "Community(" + (cbl_community.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_community.Items.Count; i++)
                {
                    cbl_community.Items[i].Selected = false;
                }
                txt_community.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_community_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_community.Text = "--Select--";
            cb_community.Checked = false;
            for (int i = 0; i < cbl_community.Items.Count; i++)
            {
                if (cbl_community.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_community.Text = "Community(" + commcount.ToString() + ")";
                if (commcount == cbl_community.Items.Count)
                {
                    cb_community.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_status_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_status.Checked == true)
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = true;
                }
                txt_status.Text = "Status(" + (cbl_status.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_status.Items.Count; i++)
                {
                    cbl_status.Items[i].Selected = false;
                }
                txt_status.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_status_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_status.Text = "--Select--";
            cb_status.Checked = false;
            for (int i = 0; i < cbl_status.Items.Count; i++)
            {
                if (cbl_status.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_status.Text = "Status(" + commcount.ToString() + ")";
                if (commcount == cbl_status.Items.Count)
                {
                    cb_status.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_vacatedatebetween_onclick(object sender, EventArgs e)
    {
        if (cb_vacatedatebetween.Checked)
        {
            txtfrmdate.Enabled = true;
            txttodate.Enabled = true;
        }
        else
        {
            txtfrmdate.Enabled = false;
            txttodate.Enabled = false;
        }
    }
    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            DataView dv1 = new DataView();
            Printcontrol.Visible = false;
            buildvalue1 = rs.GetSelectedItemsValueAsString(cbl_hostelname);
            buildvalue2 = rs.GetSelectedItemsValueAsString(cbl_buildname);
            buildvalue3 = rs.GetSelectedItemsValueAsString(cbl_floorname);
            buildvalue4 = rs.GetSelectedItemsValueAsString(cbl_roomname);
            buildvalue7 = rs.GetSelectedItemsValueAsString(cbl_community);
            builldvalue5 = rs.GetSelectedItemsValueAsString(cbl_sex);
            string status = rs.GetSelectedItemsValueAsString(cbl_status);
            string code = rs.GetSelectedItemsValueAsString(cbl_clg);
            if (cb1.Checked == true)
            {
                batch = rs.GetSelectedItemsValueAsString(cbl_batch);
                buildvalue8 = rs.GetSelectedItemsValueAsString(cbl_degree);
                buildvalue6 = rs.GetSelectedItemsValueAsString(cbl_branch);
            }
            if (ItemList.Count == 0)
            {
                ItemList.Add("Roll_No");
                ItemList.Add("Roll_Admit");
                ItemList.Add("Stud_Name");
                ItemList.Add("Degree");
                ItemList.Add("id");
            }
            Hashtable columnhash = new Hashtable();
            columnhash.Clear();
            columnhash.Add("Roll_No", "Roll No");
            columnhash.Add("Reg_No", "Reg No");
            columnhash.Add("Roll_Admit", "Admission No");
            columnhash.Add("Stud_Name", "Name");
            columnhash.Add("Degree", "Course");
            columnhash.Add("DOB", "DOB");
            columnhash.Add("Parent_AddressP", "Address");
            columnhash.Add("PAddress", "Street");
            columnhash.Add("CityP", "City");
            columnhash.Add("Ppincode", "Pincode");
            columnhash.Add("districtp", "District");
            columnhash.Add("StateP", "State");
            columnhash.Add("community", "Community");
            columnhash.Add("region", "Religion");
            columnhash.Add("Admin_Date", "Admit Date");
            columnhash.Add("BuildingFK", "Building");
            columnhash.Add("FloorFK", "Floor");
            columnhash.Add("RoomFK", "Room");
            columnhash.Add("Room_type", "Room Type");
            columnhash.Add("DiscontinueDate", "Discontinue");
            columnhash.Add("HostelName", "Hostel Name");
            columnhash.Add("Student_Mobile", "Student Mobile");
            columnhash.Add("StudMessType", "Student Mess Type");
            columnhash.Add("id", "Student Id");
            string messtype = "";
            if (ddl_basemesstype.SelectedItem.Value == "1" || ddl_basemesstype.SelectedItem.Value == "0")
            {
                messtype = " and StudMessType in('" + ddl_basemesstype.SelectedItem.Value + "')";
            }
            if (ddl_basemesstype.SelectedItem.Value == "2")
            {
                messtype = "";
            }
            string sql = "";
            if (buildvalue1.Trim() != "")//&& buildvalue2.Trim() != "" && buildvalue3.Trim() != "" && buildvalue4.Trim() != "" //change sairam 29.09.16
            {
                string roll = "";
                if (txt_roll.Text.Trim() != "")
                {
                    roll = " and R.Roll_No ='" + txt_roll.Text + "'";
                }
                else if (txt_name.Text.Trim() != "")
                {
                    roll = "  and R.Stud_Name ='" + txt_name.Text + "'";
                }
                if (roll.Trim() != "")
                {
                    sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.Roll_Admit,hr.id,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a  where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id " + roll + "  and h.HostelMasterPK in ('" + buildvalue1 + "')";
                    sql = sql + " AND isnull(IsVacated,'0')='0'";//Added by rajasekar 18/07/2018


                    //AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0' case when StudMessType=0 then 'Veg' when StudMessType=1 then 'Non Veg' else '' end StudMessType
                }
                else
                {
                    sql = "select hr.APP_No,R.Roll_No,R.Reg_No,r.Roll_Admit,hr.id,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,'' as Room_type,(select StudentTypeName From HostelStudentType where StudentType-1=StudMessType)StudMessType from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a  where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id   and d.college_code in ('" + code + "')  and h.HostelMasterPK in ('" + buildvalue1 + "') " + messtype + "";
                    if (txt_buildingname.Text.Trim() != "--Select--")
                    {
                        sql = sql + " and hr.BuildingFK in ('" + buildvalue2 + "')";
                    }
                    if (txt_floorname.Text.Trim() != "--Select--")
                    {
                        sql = sql + "  and hr.FloorFK in ('" + buildvalue3 + "') ";
                    }
                    if (txt_roomname.Text.Trim() != "--Select--")
                    {
                        sql = sql + "  and hr.RoomFK in ('" + buildvalue4 + "')";
                    }
                    if (buildvalue6.Trim() != "")
                    {
                        sql = sql + " and d.Degree_Code in ('" + buildvalue6 + "') ";
                    }
                    if (batch.Trim() != "")
                    {
                        sql = sql + " and r.Batch_Year in ('" + batch + "') ";
                    }
                    if (builldvalue5.Trim() != "")
                    {
                        sql = sql + " and a.sex in ('" + builldvalue5 + "') ";
                    }
                    if (buildvalue7.Trim() != "")
                    {
                        sql = sql + " and a.community in ('" + buildvalue7 + "') ";
                    }
                    if (cb_status.Checked == true)
                    {
                        sql = sql + " AND  (IsSuspend='1' or IsDiscontinued='1' or IsVacated='1') ";
                    }
                    else if (cb_status.Checked == false)
                    {
                        if (cbl_status.Items[0].Selected == true && cbl_status.Items[1].Selected == true)
                        {
                            sql = sql + " AND (IsSuspend='1' or IsDiscontinued='1')";
                        }
                        else if (cbl_status.Items[0].Selected == true && cbl_status.Items[2].Selected == true)
                        {
                            sql = sql + " AND (IsSuspend='1' or IsVacated='1')";
                        }
                        else if (cbl_status.Items[1].Selected == true && cbl_status.Items[2].Selected == true)
                        {
                            sql = sql + " AND (IsDiscontinued='1' or IsVacated='1')";
                        }
                        else if (cbl_status.Items[0].Selected)
                        {
                            sql = sql + " AND IsSuspend='1'";
                        }
                        else if (cbl_status.Items[1].Selected)
                        {
                            sql = sql + " and IsDiscontinued='1'";
                        }
                        else if (cbl_status.Items[2].Selected)
                        {
                            sql = sql + "and IsVacated='1'";
                        }
                        else
                        {
                            sql = sql + " AND ISNULL( IsSuspend,'0')='0'  AND isnull(IsDiscontinued,'0')='0' AND isnull(IsVacated,'0')='0'and r.cc=0 and r.Exam_Flag<>'debar' and  r.DelFlag=0";

                        }

                    }
                    string[] ay = txtfrmdate.Text.Split('/');
                    string[] ay1 = txttodate.Text.Split('/');
                    string currdate = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime fromdate = new DateTime();
                    DateTime todate = new DateTime();
                    fromdate = Convert.ToDateTime(ay[1] + "/" + ay[0] + "/" + ay[2]);
                    todate = Convert.ToDateTime(ay1[1] + "/" + ay1[0] + "/" + ay1[2]);
                    if (cb_vacatedatebetween.Checked == true)
                        sql += " and VacatedDate between '" + fromdate.ToString("MM/dd/yyyy") + "' and '" + todate.ToString("MM/dd/yyyy") + "'";
                }
                sql = sql + " order by r.Roll_No,hr.BuildingFK,hr.FloorFK,hr.RoomFK ";
                sql = sql + " select Building_Name,Code  from Building_Master";
                sql = sql + " select Floor_Name,Floorpk  from Floor_Master";
                sql = sql + " SELECT Room_type,Floor_Name,Room_Name,Roompk,Building_Name FROM Room_Detail";
                ds.Clear();
                ds = d2.select_method_wo_parameter(sql, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].ColumnCount = 0;
                    Fpspread1.CommandBar.Visible = false;
                    Fpspread1.Sheets[0].AutoPostBack = true;
                    Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                    Fpspread1.Sheets[0].RowHeader.Visible = false;
                    Fpspread1.Sheets[0].ColumnCount = ItemList.Count + 2;
                    Fpspread1.Sheets[0].RowCount = 1;
                    FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                    darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                    darkstyle.ForeColor = Color.White;
                    Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Columns[0].Width = 50;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                    Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                    // FpSpread1.Sheets[0].Columns[1].Width = 20;
                    //////true for select all//////// 
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                    FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    check.AutoPostBack = true;
                    FarPoint.Web.Spread.CheckBoxCellType check1 = new FarPoint.Web.Spread.CheckBoxCellType();
                    check1.AutoPostBack = false;
                    if (cb_vacate.Checked == true)
                    {
                        Fpspread1.Sheets[0].Columns[1].Visible = true;
                        Fpspread1.Sheets[0].AutoPostBack = false;
                        Fpspread1.Width = 923;
                        Fpspread1.Height = 300;
                        btn_vacate.Visible = true;
                        txt_vatreason.Visible = true;
                        lnk_vacated.Visible = true;
                        lbl_vatreason.Visible = true;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Columns[1].Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        btn_vacate.Visible = false;
                        lnk_vacated.Visible = false; txt_vatreason.Visible = false;
                        lbl_vatreason.Visible = false;
                    }
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                        if (ItemList.Contains(Convert.ToString(colno)))
                        {
                            int insdex = ItemList.IndexOf(Convert.ToString(colno));
                            //FpSpread1.Columns[insdex].Locked = true;
                            Fpspread1.Columns[insdex + 2].Width = 150;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Text = Convert.ToString(columnhash[colno]);
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Bold = true;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].ColumnHeader.Cells[0, insdex + 2].HorizontalAlign = HorizontalAlign.Center;
                            if (colno == "Stud_Name")
                            {
                                Fpspread1.Columns[insdex + 2].Width = 200;
                            }
                        }
                    }
                    Fpspread1.Sheets[0].RowCount = 0;
                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    int indRelDate = 0;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Fpspread1.Sheets[0].RowCount++;
                        count++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = ds.Tables[0].Rows[i]["Roll_No"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Note = ds.Tables[0].Rows[i]["HostelMasterFK"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = ds.Tables[0].Rows[i]["APP_No"].ToString();
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Note = ds.Tables[0].Rows[i]["RoomFK"].ToString();

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check1;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                        ViewState["Floor_Name"] = null;
                        ViewState["Building_Name"] = null;
                        ViewState["Room_Name"] = null;
                        for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                        {
                            if (ItemList.Contains(Convert.ToString(ds.Tables[0].Columns[j].ToString())))
                            {
                                int insdex = ItemList.IndexOf(Convert.ToString(ds.Tables[0].Columns[j].ToString()));
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].CellType = txt;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = ds.Tables[0].Rows[i][j].ToString();
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Font.Name = "Book Antiqua";
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Font.Size = FontUnit.Medium;
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Locked = true;
                                //Fpspread1.Columns[insdex].Width = 150;
                                string colno = Convert.ToString(ds.Tables[0].Columns[j]);
                                if (Convert.ToString(ds.Tables[0].Columns[j].ToString()) == "DiscontinueDate")
                                {
                                    indRelDate = insdex + 2;
                                }
                                if (colno.Trim() == "BuildingFK")
                                {
                                    if (ds.Tables[1].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[1].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Building_Name"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                ViewState["Building_Name"] = buildvalue;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() == "FloorFK")
                                {
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[2].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Floor_Name"]);
                                                    }
                                                }
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                ViewState["Floor_Name"] = buildvalue;
                                            }
                                        }
                                    }
                                }
                                if (colno.Trim() == "RoomFK")
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        if (Convert.ToString(ds.Tables[0].Rows[i][j]).Trim() != "")
                                        {
                                            ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + Convert.ToString(ds.Tables[0].Rows[i][j]) + ")";
                                            dv1 = ds.Tables[3].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                string buildvalue = "";
                                                for (int r = 0; r < dv1.Count; r++)
                                                {
                                                    if (buildvalue == "")
                                                    {
                                                        buildvalue = Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                    else
                                                    {
                                                        buildvalue = buildvalue + "," + Convert.ToString(dv1[r]["Room_Name"]);
                                                    }
                                                }
                                                ViewState["Room_Name"] = buildvalue;
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                            }
                                        }
                                    }
                                }
                                //06.07.16
                                if (colno.Trim() == "Room_type")
                                {
                                    if (ds.Tables[3].Rows.Count > 0)
                                    {
                                        //BuildingFK,FloorFK,RoomFK
                                        //ds.Tables[3].DefaultView.RowFilter = "Roompk =" + Convert.ToString(ds.Tables[0].Rows[i]["ROOMfK"]) + " and Floor_Name='" + Convert.ToString(ViewState["Floor_Name"]) + "' and Building_Name='" + Convert.ToString(ViewState["Building_Name"]) + "'";
                                        ds.Tables[2].DefaultView.RowFilter = "FloorPK in (" + Convert.ToString(ds.Tables[0].Rows[i]["FloorFK"]) + ")";
                                        DataView dv3 = ds.Tables[2].DefaultView;
                                        if (dv3.Count > 0)
                                        {
                                            string floor_name = Convert.ToString(dv3[0]["Floor_Name"]);
                                            ds.Tables[1].DefaultView.RowFilter = "code in (" + Convert.ToString(ds.Tables[0].Rows[i]["BuildingFK"]) + ")";
                                            DataView dv2 = ds.Tables[1].DefaultView;
                                            if (dv2.Count > 0)
                                            {
                                                string bulid = Convert.ToString(dv2[0]["Building_Name"]);
                                                ds.Tables[3].DefaultView.RowFilter = "Roompk =" + Convert.ToString(ds.Tables[0].Rows[i]["ROOMfK"]) + " and Floor_Name='" + floor_name + "' and Building_Name='" + bulid + "'";
                                                //Convert.ToString(ds.Tables[0].Rows[i][j])
                                                dv1 = ds.Tables[3].DefaultView;
                                                if (dv1.Count > 0)
                                                {
                                                    string buildvalue = "";
                                                    buildvalue = Convert.ToString(dv1[0]["Room_type"]);
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].Text = Convert.ToString(buildvalue);
                                                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, insdex + 2].HorizontalAlign = HorizontalAlign.Left;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    div1.Visible = true;
                    Fpspread1.Visible = true;
                    div_report.Visible = true;
                    lblerr.Visible = false;
                    pheaderfilter.Visible = true;
                    pcolumnorder.Visible = true;
                    lbl_stucnt.Visible = true;
                    lbl_stucnt.Text = "No of Students:" + count.ToString();
                    Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                    if (ItemList.Contains("DiscontinueDate"))
                    {
                        try
                        {
                            string reliveddate1 = "";
                            reliveddate1 = Convert.ToString(ds.Tables[0].Columns["DiscontinueDate"]);
                            for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                            {
                                reliveddate1 = Convert.ToString(ds.Tables[0].Rows[k]["DiscontinueDate"]);
                                string reliveddate = "01/01/1900";
                                if (reliveddate1 == reliveddate)
                                {
                                    Fpspread1.Sheets[0].Cells[k, indRelDate].Text = "";
                                }
                            }
                        }
                        catch { }
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    lblerr.Visible = true;
                    lblerr.Text = "No Records Found";
                    lbl_stucnt.Visible = false;
                    pheaderfilter.Visible = false;
                    pcolumnorder.Visible = false;
                    div_report.Visible = false;
                    btn_vacate.Visible = false;
                    lnk_vacated.Visible = false;
                    txt_vatreason.Visible = false;
                    lbl_vatreason.Visible = false;
                }
            }
            else
            {
                div1.Visible = false;
                Fpspread1.Visible = false;
                lblerr.Visible = true;
                lblerr.Text = "Please Select All Field";
                lbl_stucnt.Visible = false;
                pheaderfilter.Visible = false;
                pcolumnorder.Visible = false;
                div_report.Visible = false;
                btn_vacate.Visible = false;
                txt_vatreason.Visible = false;
                lnk_vacated.Visible = false;
                lbl_vatreason.Visible = false;
            }
        }
        catch
        {
        }
    }
    public void btn_add_Click(object sender, EventArgs e)
    {
        loaddate();
        clear();
        idgeneration();
        txt_pop1admindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        popwindow1.Visible = true;
        lnkbtn_transferhos.Visible = false;
        lnkbtn_suspension.Visible = false;
        linkwithdrawal.Visible = false;
        btn_pop1save.Visible = true;
        btn_pop1exit.Visible = true;
        btn_pop1update.Visible = false;
        btn_pop1delete.Visible = false;
        btn_pop1exit1.Visible = false;
        cb_discontinue_CheckedChanged(sender, e);
        cb_pop1vacate_CheckedChange(sender, e);
        cb_discontinue.Enabled = false;
        cb_pop1vacate.Enabled = false;
        cb_discontinue.Checked = false;
        txt_discontinuedate.Enabled = false;
        txt_pop1reason.Enabled = false;
        cb_pop1vacate.Checked = false;
        txt_vacatedate.Enabled = false;
        txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        txt_pop1rollno.Enabled = true;
        txt_pop1name.Enabled = true;
        cb_Hostelfeesallot.Visible = true;
        cb_Hostelfeesallot.Checked = false;
    }
    public void FpSpread1_CellClick(object sender, EventArgs e)
    {
        try
        {
            Cellclick = true;
            //ddl_pop1collegename.Enabled = false;//11.03.16
            popwindow1.Visible = true;
            btn_pop1save.Visible = false;
            btn_pop1exit.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }
    public void FpSpread1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cellclick == true)
        {
            try
            {

                DataView dv1 = new DataView();
                string building = "";
                string floor = "";

                string roomno = "";
                lnkbtn_transferhos.Visible = true;
                lnkbtn_suspension.Visible = true;
                linkwithdrawal.Visible = true;
                ddl_fromhostel.Enabled = false;
                btn_pop1save.Visible = false;
                btn_pop1exit.Visible = false;
                btn_pop1update.Visible = true;
                btn_pop1delete.Visible = true;
                btn_pop1exit1.Visible = true;
                cb_discontinue.Enabled = true;
                cb_discontinue.Checked = false;
                cb_pop1vacate.Enabled = true;
                cb_pop1vacate.Checked = false;
                txt_discontinuedate.Enabled = false;
                txt_vacatedate.Enabled = false;
                txt_pop1reason.Enabled = false;
                txt_pop1rollno.Enabled = false;//21.09.16
                txt_pop1name.Enabled = false;//21.09.16
                cb_Hostelfeesallot.Visible = false;
                txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                string activerow = "";
                activerow = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                string purpose = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                txt_pop1rollno.Text = purpose;
                string clgcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                string collegecode = d2.GetFunction("select CollegeCode from HT_HostelRegistration  where APP_No='" + clgcode + "'");
                bindpop1college();
                ddl_pop1collegename.SelectedIndex = ddl_pop1collegename.Items.IndexOf(ddl_pop1collegename.Items.FindByValue(collegecode));
                //commented and added by saranya devi(27.08.2018)
                //string text = "select StudMessType ,r.app_no,R.Roll_No,r.Roll_Admit,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,CONVERT(VARCHAR(11),VacatedDate,103) as VacatedDate,(select mastervalue from CO_MasterValues where convert(varchar,mastercode)=convert(varchar,reason))as Reason,IsVacated,IsDiscontinued,IsSuspend,id,Messcode  from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a  where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)  and R.Roll_No='" + txt_pop1rollno.Text + "'";
                string text = "select isnull(StudMessType,'') StudMessType ,r.app_no,R.Roll_No,r.Roll_Admit,R.Stud_Name,Course_Name+'-'+Dept_Name Degree,CONVERT(VARCHAR(11),A.DOB,103) as DOB,A.Parent_AddressP,A.StreetP PAddress,A.CityP,A.Parent_PincodeP Ppincode,case when isnumeric(a.districtp) = 1 then  (select textval from textvaltable where str(textvaltable.textcode) = str(a.districtp)) else A.districtp end districtp, (SELECT TextVal FROM TextValTable ST WHERE ST.TextCode =A.Parent_StateP) StateP,(select TextVal from TextValTable where TextCode= a.community) as community ,(select TextVal from TextValTable where TextCode= a.region) as region, CONVERT(VARCHAR(11),HostelAdmDate,103) as Admin_Date,BuildingFK,FloorFK,RoomFK, CONVERT(VARCHAR(11),DiscontinueDate,103) as DiscontinueDate,HostelName,hr.HostelMasterFK,ISNULL(A.Student_Mobile,'') Student_Mobile,CONVERT(VARCHAR(11),VacatedDate,103) as VacatedDate,(select mastervalue from CO_MasterValues where convert(varchar,mastercode)=convert(varchar,reason))as Reason,IsVacated,IsDiscontinued,IsSuspend,hr.id,Messcode  from HT_HostelRegistration hr ,HM_HostelMaster h, Registration r,Degree d,Department dt,Course c,applyn a  where hr.APP_No =r.App_No and a.app_no =r.App_No and hr.HostelMasterFK=h.HostelMasterPK and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)  and R.Roll_No='" + txt_pop1rollno.Text + "'";
                text = text + " select Building_Name,Code  from Building_Master";
                text = text + " select Floor_Name,Floorpk  from Floor_Master";
                text = text + " select Room_Name,Roompk from Room_Detail";
                // text = text + " select MasterValue from CO_MasterValues where MasterCriteria='HSVAC'";
                ds = d2.select_method_wo_parameter(text, "Text");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddl_pop1hostelname.SelectedIndex = ddl_pop1hostelname.Items.IndexOf(ddl_pop1hostelname.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"])));
                    int messtype1 = Convert.ToInt16(ds.Tables[0].Rows[0]["StudMessType"]);
                    ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByValue(Convert.ToString(messtype1 + 1)));
                    //Added By saranya 26/9/2018
                    string selectQuery = d2.GetFunction("select MessMasterFK1 from HM_HostelMaster where HostelMasterPK='" + Convert.ToString(ds.Tables[0].Rows[0]["HostelMasterFK"]) + "'");
                    string[] spl = selectQuery.Split('-');
                    if (spl.Length > 0)
                    {
                        string typ = string.Empty;
                        if (spl.Count() > 0)
                        {
                            for (int i = 0; i < spl.Count(); i++)
                            {
                                if (typ == "")
                                {
                                    typ = "" + spl[i] + "";
                                }
                                else
                                {
                                    typ = typ + "'" + "," + "'" + spl[i] + "";
                                }

                            }

                        }
                        selectQuery = ("select MessMasterPK,MessName from HM_MessMaster where MessMasterPK in('" + typ + "') order by MessMasterPK asc");
                    }
                    DataSet dsMessMaster = d2.select_method_wo_parameter(selectQuery, "text");
                    if (dsMessMaster.Tables[0].Rows.Count > 0)
                    {
                        ddlmess.DataSource = dsMessMaster;
                        ddlmess.DataTextField = "MessName";
                        ddlmess.DataValueField = "MessMasterPK";
                        ddlmess.DataBind();
                    }
                    //=================================//
                    //ddlmess.SelectedIndex = ddlmess.Items.IndexOf(ddlmess.Items.FindByValue(Convert.ToString(ds.Tables[0].Rows[0]["Messcode"])));
                    txt_pop1rolladmin.Text = Convert.ToString(ds.Tables[0].Rows[0]["Roll_Admit"]);
                    txt_pop1name.Text = Convert.ToString(ds.Tables[0].Rows[0]["Stud_Name"]);
                    txt_pop1degree.Text = Convert.ToString(ds.Tables[0].Rows[0]["Degree"]);
                    ViewState["app_no"] = Convert.ToString(ds.Tables[0].Rows[0]["App_no"]);
                    ViewState["id"] = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                    building = ds.Tables[0].Rows[0]["BuildingFK"].ToString();
                    ViewState["Code"] = Convert.ToString(building);
                    txtid.Text = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                    if (building != "")
                    {
                        ds.Tables[1].DefaultView.RowFilter = "Code in (" + building + ")";
                        dv1 = ds.Tables[1].DefaultView;
                        if (dv1.Count > 0)
                        {
                            for (int row = 0; row < dv1.Count; row++)
                            {
                                build1 = Convert.ToString(dv1[row]["Building_Name"]);
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
                        txt_pop1building.Text = Convert.ToString(buildvalue1);
                    }
                    floor = ds.Tables[0].Rows[0]["FloorFK"].ToString();
                    ViewState["Floorpk"] = Convert.ToString(floor);
                    if (floor != "")
                    {
                        string build2 = "";
                        string buildvalue2 = "";
                        ds.Tables[2].DefaultView.RowFilter = "Floorpk in (" + floor + ")";
                        dv1 = ds.Tables[2].DefaultView;
                        if (dv1.Count > 0)
                        {
                            for (int row = 0; row < dv1.Count; row++)
                            {
                                build2 = Convert.ToString(dv1[row]["Floor_Name"]);
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
                        txt_pop1floor.Text = Convert.ToString(buildvalue2);
                    }
                    roomno = ds.Tables[0].Rows[0]["RoomFK"].ToString();
                    ViewState["Roompk"] = Convert.ToString(roomno);
                    if (roomno != "")
                    {
                        string build3 = "";
                        string buildvalue3 = "";
                        ds.Tables[3].DefaultView.RowFilter = "Roompk in (" + roomno + ")";
                        dv1 = ds.Tables[3].DefaultView;
                        if (dv1.Count > 0)
                        {
                            for (int row = 0; row < dv1.Count; row++)
                            {
                                build3 = Convert.ToString(dv1[row]["Room_Name"]);
                                if (buildvalue3 == "")
                                {
                                    buildvalue3 = build3;
                                }
                                else
                                {
                                    buildvalue3 = buildvalue3 + "'" + "," + "'" + build3;
                                }
                            }
                        }
                        txt_pop1roomno.Text = Convert.ToString(buildvalue3);
                    }
                    ViewState["bulid1fk"] = Convert.ToString(ds.Tables[0].Rows[0]["BuildingFK"]);
                    ViewState["floor1fk"] = Convert.ToString(ds.Tables[0].Rows[0]["FloorFK"]);
                    ViewState["Room1fk"] = Convert.ToString(ds.Tables[0].Rows[0]["RoomFK"]);
                    txt_pop1admindate.Text = ds.Tables[0].Rows[0]["Admin_Date"].ToString();
                    txt_pop1address.Text = ds.Tables[0].Rows[0]["Parent_AddressP"].ToString();
                    txt_pop1address1.Text = ds.Tables[0].Rows[0]["PAddress"].ToString();
                    txt_pop1address2.Text = ds.Tables[0].Rows[0]["CityP"].ToString();
                    txt_pop1pin.Text = ds.Tables[0].Rows[0]["Ppincode"].ToString();
                    txt_pop1community.Text = ds.Tables[0].Rows[0]["community"].ToString();
                    txt_discontinuedate.Text = ds.Tables[0].Rows[0]["DiscontinueDate"].ToString();
                    txt_vacatedate.Text = ds.Tables[0].Rows[0]["VacatedDate"].ToString();
                    txt_pop1mob.Text = ds.Tables[0].Rows[0]["Student_Mobile"].ToString();
                    //  txt_pop1reason.Text = ds.Tables[0].Rows[0]["Reason"].ToString();
                    txt_date.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
                    string vacate = "";
                    vacate = Convert.ToString(ds.Tables[0].Rows[0]["IsVacated"]);
                    if (vacate != "Null" && vacate != "True")
                    {
                        cb_pop1vacate.Checked = false;
                        txt_vacatedate.Enabled = false;
                        txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        //  txt_pop1reason.Enabled = false;
                    }
                    else
                    {
                        cb_pop1vacate.Checked = true;
                        txt_vacatedate.Enabled = true;
                        //  txt_pop1reason.Enabled = true;
                        // txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        // txt_pop1reason.Enabled = false;
                    }
                    string discon = "";
                    discon = Convert.ToString(ds.Tables[0].Rows[0]["IsDiscontinued"]);
                    if (discon == "True" || discon == "1")
                    {
                        cb_discontinue.Checked = true;
                        txt_discontinuedate.Enabled = true;
                        // txt_pop1reason.Enabled = true;
                    }
                    else
                    {
                        cb_discontinue.Checked = false;
                        txt_discontinuedate.Enabled = false;
                        // txt_pop1reason.Enabled = false;
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["IsVacated"]) == "1" || Convert.ToString(ds.Tables[0].Rows[0]["IsVacated"]) == "True")
                    {
                        txt_pop1reason.Enabled = true;
                        txt_pop1reason.Text = Convert.ToString(ds.Tables[0].Rows[0]["Reason"]);
                    }
                    if (Convert.ToString(ds.Tables[0].Rows[0]["IsDiscontinued"]) == "1" || Convert.ToString(ds.Tables[0].Rows[0]["IsDiscontinued"]) == "True")
                    {
                        txt_pop1reason.Enabled = true;
                        txt_pop1reason.Text = Convert.ToString(ds.Tables[0].Rows[0]["Reason"]);
                    }
                    int messtype = 0;//Barath 15.02.18
                    int.TryParse(Convert.ToString(ds.Tables[0].Rows[0]["StudMessType"]), out messtype);
                    ddlStudType.SelectedIndex = ddlStudType.Items.IndexOf(ddlStudType.Items.FindByValue(Convert.ToString(messtype + 1)));
                    //{
                    //    if (messtype == "0")
                    //    {
                    //        Radiobtnstype.Items[0].Selected = true;
                    //        Radiobtnstype.Items[1].Selected = false;
                    //    }
                    //    else if (messtype == "1")
                    //    {
                    //        Radiobtnstype.Items[1].Selected = true;
                    //        Radiobtnstype.Items[0].Selected = false;
                    //    }
                    //}
                }
                string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_pop1roomno.Text + "'";
                ds = d2.select_method_wo_parameter(room, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                    txt_pop1roomtype.Text = roomtype;
                }
                string blood = "select textval from applyn a,textvaltable t,Registration b where a.bldgrp = t.TextCode and a.app_no =b.App_No and  Roll_No='" + txt_pop1rollno.Text + "'";
                ds = d2.select_method_wo_parameter(blood, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string bloodgroup = ds.Tables[0].Rows[0]["textval"].ToString();
                    txt_pop1blood.Text = bloodgroup;
                }

                //string discontxt = "select co.MasterValue from CO_MasterValues co,HT_HostelRegistration hr,Registration r where MasterCriteria='HSDSC' and co.MasterCode=hr.Reason and r.App_No=hr.APP_No and r.Roll_No='" + txt_pop1rollno.Text + "'";
                //ds = d2.select_method_wo_parameter(discontxt, "Text");barath 15.04.17
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    string dis = ds.Tables[0].Rows[0]["MasterValue"].ToString();
                //    txt_pop1reason.Text = dis;
                //    txt_pop1reason.Enabled = true;
                //}
                //string vactxt = "select co.MasterValue from CO_MasterValues co,HT_HostelRegistration hr,Registration r where MasterCriteria='HSVAC' and co.MasterCode=hr.Reason and r.App_No=hr.APP_No and r.Roll_No='" + txt_pop1rollno.Text + "'";
                //ds = d2.select_method_wo_parameter(vactxt, "Text");
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    //if (cb_discontinue.Checked == true)
                //    //{
                //    string dis = ds.Tables[0].Rows[0]["MasterValue"].ToString();
                //    txt_pop1reason.Text = dis;
                //    txt_pop1reason.Enabled = true;
                //    //}
                //    //else
                //    //{
                //    //    txt_pop1reason.Enabled = false;
                //    //}
                //}




                //Added By SaranyaDevi 5.3.2019 Request Vacate
                string appno = d2.GetFunction("select App_No from Registration R where  R.Roll_No='" + txt_pop1rollno.Text + "'");
                string reason = d2.GetFunction("select Req_VacateReason from RQ_Requisition where MemType='1' and ReqAppNo ='" + appno + "' and stud='1' and RequestType='14'");
                string Req_VacateDate = d2.GetFunction("select CONVERT(VARCHAR(11),Req_VacateDate,103)Req_VacateDate from RQ_Requisition where MemType='1' and ReqAppNo ='" + appno + "' and stud='1' and RequestType='14'");
                if (reason != "" && reason != "0" && Req_VacateDate != "" && Req_VacateDate != "0")
                {
                    cb_pop1vacate.Checked = true;
                    cb_pop1vacate.Enabled = false;
                    txt_vacatedate.Text = Req_VacateDate;
                    txt_pop1reason.Text = reason;
                }

                for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    if (i == Convert.ToInt32(activerow))
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                        Fpspread1.Sheets[0].SelectionBackColor = Color.LightBlue;
                        //Fpspread1.Sheets[0].SelectionForeColor = Color.White;
                    }
                    else
                    {
                        Fpspread1.Sheets[0].Rows[i].BackColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
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
        }
        catch (Exception ex)
        {
        }
    }
    public void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string report = txt_excelname.Text;
            if (report.ToString().Trim() != "")
            {
                //  FpSpread1.Sheets[0].Columns[1].Visible = false;
                d2.printexcelreport(Fpspread1, report);
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
    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "@" + "Date :" + System.DateTime.Now.ToString("dd/MM/yyy");
            string batch = "";
            if (cb1.Checked == true)
            {
                batch = "@" + " Batch : " + cbl_batch.SelectedItem.ToString() + "-" + " Degree :" + cbl_degree.SelectedItem.Text.ToString() + "-" + " Branch :" + cbl_branch.SelectedItem.Text.ToString();
            }
            string rbname = string.Empty;
            if (cb_status.Checked == true)
            {
                rbname = "Hostel Student  Vacated,suspended and discontinue Details";
            }
            else if (cb_status.Checked == false)
            {
                if (cbl_status.Items[0].Selected == true && cbl_status.Items[1].Selected == true)
                {
                    rbname = "Hostel Student suspended and Vacated Details";

                }
                else if (cbl_status.Items[0].Selected == true && cbl_status.Items[2].Selected == true)
                {
                    rbname = "Hostel  Student suspended and discontinue Details";

                }
                else if (cbl_status.Items[1].Selected == true && cbl_status.Items[2].Selected == true)
                {
                    rbname = "Hostel Student Vacated and discontinue Details";

                }
                else if (cbl_status.Items[0].Selected)
                {
                    rbname = "Hostel Student suspended Details";

                }
                else if (cbl_status.Items[1].Selected)
                {
                    rbname = "Hostel Student discontinue Details";

                }
                else if (cbl_status.Items[2].Selected)
                {
                    rbname = "Hostel Student Vacated Details";

                }
                else
                {
                    rbname = "Hostel Registration Report";

                }

            }
            string pagename = "HostelRegistration.aspx";
            //   string degreedetails = "Hostel Registration Report" + batch + date;
            string degreedetails = rbname + batch + date;
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch (Exception ex)
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
    /*popwindow1 Hostel Registration*/
    protected void imagebtnpop1close_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    protected void bindpop1college()
    {
        try
        {
            string clgname = "select college_code,collname from collinfo ";
            if (clgname != "")
            {
                ds = d2.select_method(clgname, hat, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //ddl_college.DataSource = ds;
                    //ddl_college.DataTextField = "collname";
                    //ddl_college.DataValueField = "college_code";
                    //ddl_college.DataBind();
                    // ddlcollege.Items.Insert(0,"");
                    ddl_pop2collgname.DataSource = ds;
                    ddl_pop2collgname.DataTextField = "collname";
                    ddl_pop2collgname.DataValueField = "college_code";
                    ddl_pop2collgname.DataBind();
                    ddl_pop1collegename.DataSource = ds;
                    ddl_pop1collegename.DataTextField = "collname";
                    ddl_pop1collegename.DataValueField = "college_code";
                    ddl_pop1collegename.DataBind();
                    cln = ddl_pop1collegename.SelectedValue;
                }
            }
            bindpop2hostel();
            bindpop2degree();
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddl_pop2collgname_selectedindexchange(object sender, EventArgs e)
    {
        try
        {
            bindpop2degree();
            branch();
        }
        catch
        {
        }
    }
    protected void bindpop2hostel()
    {
        try
        {
            //ds.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            //ds = d2.select_method_wo_parameter(MessmasterFK, "text");
            ds.Clear();
            //string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            //ds = d2.BindHostelbaseonmessrights_inv(MessmasterFK);
            //MessmasterFK = "select HostelMasterPK,HostelName from HM_HostelMaster  order by hostelname";
            string MessmasterFK = string.Empty;
            if (usercode != "" && usercode != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + usercode + "'");
            if (group_user != "" && group_user != "0")
                MessmasterFK = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Rights' and user_code='" + group_user + "'");
            string itemname = "select HostelMasterPK,HostelName from HM_HostelMaster where  HostelMasterPK in (" + MessmasterFK + ") order by hostelname ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "text");
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
        catch
        {
        }
    }
    protected void ddl_pop1collegename_selected_indexchange(object sender, EventArgs e)
    {
        // bindpop2hostel();
    }
    public void btn1_Click(object sender, EventArgs e)
    {
        try
        {
            //div2.Visible = false;
            fproll.Visible = false;
            lblcounttxt.Visible = false;
            lblcount.Visible = false;
            btn_pop2ok.Visible = false;
            btn_pop2exit.Visible = false;
            bindpop2degree();
            branch();
            bindpop2batchyear();
            //ddl_pop2sex.SelectedItem.Text = "All";
            ddl_pop2sex.SelectedIndex = 0;
            //ddl_pop2studenttype.SelectedItem.Text = "Both";
            ddl_pop2studenttype.SelectedIndex = 0;
            int activerow = 0;
            activerow = Convert.ToInt32(fproll.ActiveSheetView.ActiveRow.ToString());
            for (int i = 0; i < fproll.Sheets[0].RowCount; i++)
            {
                if (i == Convert.ToInt32(activerow))
                {
                    fproll.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                    //fproll.Sheets[0].SelectionBackColor = Color.IndianRed;
                    //fproll.Sheets[0].SelectionForeColor = Color.White;
                }
                else
                {
                    fproll.Sheets[0].Rows[i].BackColor = Color.LightBlue;
                }
            }
            if (ddl_pop1hostelname.SelectedItem.Text != "--Select--")
            {
                popwindow2.Visible = true;
                btn_pop2ok.Visible = false;
                btn_pop2exit.Visible = false;
                //  fproll.Sheets[0].SelectionBackColor = Color.LightBlue;
                // fproll.Sheets[0].SelectionForeColor = Color.Black;
                fproll.CommandBar.Visible = false;
                fproll.SheetCorner.ColumnCount = 0;
                //fproll.Sheets[0].PageSize = fproll.Sheets[0].RowCount;
                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.Black;
                fproll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                fproll.Sheets[0].ColumnCount = 4;
                fproll.Sheets[0].ColumnHeader.Columns[0].Label = "S.No";
                fproll.Sheets[0].ColumnHeader.Columns[0].Font.Name = "Book Antiqua";
                fproll.Sheets[0].ColumnHeader.Columns[0].Font.Size = FontUnit.Medium;
                fproll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                fproll.Sheets[0].ColumnHeader.Columns[1].Label = "Roll No";
                fproll.Sheets[0].ColumnHeader.Columns[1].Font.Name = "Book Antiqua";
                fproll.Sheets[0].ColumnHeader.Columns[1].Font.Size = FontUnit.Medium;
                fproll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                fproll.Sheets[0].ColumnHeader.Columns[2].Label = "Admission No";
                fproll.Sheets[0].ColumnHeader.Columns[2].Font.Name = "Book Antiqua";
                fproll.Sheets[0].ColumnHeader.Columns[2].Font.Size = FontUnit.Medium;
                fproll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                fproll.Sheets[0].Columns[2].Visible = false;
                fproll.Sheets[0].ColumnHeader.Columns[3].Label = "Name";
                fproll.Sheets[0].ColumnHeader.Columns[3].Font.Name = "Book Antiqua";
                fproll.Sheets[0].ColumnHeader.Columns[3].Font.Size = FontUnit.Medium;
                fproll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                fproll.Sheets[0].Columns[0].Width = 50;
                fproll.Sheets[0].Columns[1].Width = 120;
                fproll.Sheets[0].Columns[2].Width = 100;
                fproll.Sheets[0].Columns[3].Width = 240;
                fproll.Sheets[0].Columns[4].Width = 280;
                fproll.Width = 426;
                fproll.Columns[0].Locked = true;
                fproll.Columns[1].Locked = true;
                fproll.Columns[2].Locked = true;
                fproll.Columns[3].Locked = true;
                fproll.Columns[4].Locked = true;
            }
            else
            {
                lblpop2error.Visible = true;
                lblcounttxt.Visible = false;
                lblcount.Visible = false;
                lblpop2error.Text = "Please Select Hostel";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_discontinue_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_discontinue.Checked == true)
        {
            txt_discontinuedate.Enabled = true;
            txt_pop1reason.Enabled = true;
            txt_discontinuedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }
        else
        {
            txt_discontinuedate.Enabled = false;
            txt_pop1reason.Enabled = false;
        }
    }
    public void cb_pop1vacate_CheckedChange(object sender, EventArgs e)
    {
        if (cb_pop1vacate.Checked == true)
        {
            txt_vacatedate.Enabled = true;
            txt_vacatedate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_pop1reason.Enabled = true;
        }
        else
        {
            txt_vacatedate.Enabled = false;
            txt_pop1reason.Enabled = false;
        }
    }
    public void loaddate()
    {
        try
        {
            for (int i = 1; i <= 31; i++)
            {
                string v = Convert.ToString(i);
                if (v.Length == 1)
                {
                    v = "0" + "" + v;
                }
                ddl_pop1date.Items.Add(Convert.ToString(v));
            }
        }
        catch
        {
        }
    }
    protected void loadyear()
    {
        int y = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));
        for (int i = 1960; i <= y; i++)
        {
            ddl_pop1year.Items.Add(Convert.ToString(i));
        }
    }
    public void btn2_Click(object sender, EventArgs e)
    {
        bindbuilding();
        btn_pop3save.Visible = false;
        btn_pop3exit.Visible = false;
        chck1.Checked = false;
        popwindow3.Visible = true;
        ddl_pop3vaccant.SelectedIndex = 0;
        roomchecklist.Items[0].Selected = false;
        roomchecklist.Items[1].Selected = false;
        roomchecklist.Items[2].Selected = false;
        FpSpread3.Visible = false;
        tblStatus.Visible = false;
        toalrooms.Visible = false;
        totalvaccants.Visible = false;
        fill.Visible = false;
        partialfill.Visible = false;
        unfill.Visible = false;
    }
    public void clear()
    {
        try
        {
            txt_date.Text = "";
            txt_pop1name.Text = "";
            txt_pop1rollno.Text = "";
            txt_pop1roomno.Text = "";
            txt_pop1roomtype.Text = "";
            txt_pop1mob.Text = "";
            txt_pop1degree.Text = "";
            txt_pop1floor.Text = "";
            txt_pop1pin.Text = "";
            txt_pop1community.Text = "";
            txt_pop1building.Text = "";
            txt_pop1address.Text = "";
            txt_pop1address1.Text = "";
            txt_pop1address2.Text = "";
            txt_pop1reason.Text = "";
            txt_pop1blood.Text = "";
            txt_pop1reason.Text = "";
            txt_pop1rolladmin.Text = "";
            ddl_pop1hostelname.Enabled = true;
        }
        catch (Exception ex)
        {
        }
    }
    public void savedetails()
    {
        try
        {
            if (txt_pop1roomno.Text != "" && txt_pop1building.Text != "" && txt_pop1name.Text != "" && txt_pop1rollno.Text != "")
            {
                string vecated = "";
                string discontinue = "";
                string vecated_date = "";
                string discontinuedate = "";
                string reason_discontinue = "";
                string dtaccessdate = DateTime.Now.ToString();
                string dtaccesstime = DateTime.Now.ToLongTimeString();
                string studmesstype = string.Empty;
                if (cb_pop1vacate.Checked == true)
                {
                    vecated = "1";
                    vecated_date = Convert.ToString(txt_vacatedate.Text);
                }
                else
                {
                    vecated = "0";
                    vecated_date = "";
                }
                if (cb_discontinue.Checked == true)
                {
                    discontinue = "1";
                    discontinuedate = Convert.ToString(txt_vacatedate.Text);
                    reason_discontinue = Convert.ToString(txt_pop1reason.Text);
                }
                else
                {
                    discontinue = "0";
                    discontinuedate = "";
                    reason_discontinue = "";
                }
                //if (Radiobtnstype.Items[0].Selected == true)
                //    studmesstype = "0";
                //else if (Radiobtnstype.Items[1].Selected == true)
                //    studmesstype = "1";
                int messtype = 0;
                int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
                studmesstype = Convert.ToString(messtype - 1);

                string[] ay = txt_pop1admindate.Text.Split('/');
                txt_pop1admindate.Text = ay[1].ToString() + "/" + ay[0].ToString() + "/" + ay[2].ToString();
                string[] ay1 = txt_discontinuedate.Text.Split('/');
                txt_discontinuedate.Text = ay1[1].ToString() + "/" + ay1[0].ToString() + "/" + ay1[2].ToString();
                string[] ay11 = txt_vacatedate.Text.Split('/');
                txt_vacatedate.Text = ay11[1].ToString() + "/" + ay11[0].ToString() + "/" + ay11[2].ToString();
                string building = "";
                string floor = "";
                string roomno = "";
                string appnumber = "";
                building = Convert.ToString(ViewState["Code"]);
                floor = Convert.ToString(ViewState["Floorpk"]);
                roomno = Convert.ToString(ViewState["Roompk"]);
                appnumber = Convert.ToString(ViewState["App_No"]);
                string id = Convert.ToString(ViewState["Id"]);
                #region roomupdation
                string hostelgender = d2.GetFunction("select HostelType from HM_HostelMaster where HostelMasterPK ='" + ddl_pop1hostelname.SelectedValue + "'");
                string studgender = d2.GetFunction("select sex from applyn where app_no='" + appnumber + "'");//stud_name='" + txt_pop1name.Text + "'");
                string studentgen = "";
                if (studgender.Trim() == "0")
                {
                    studentgen = "1";
                }
                else if (studgender.Trim() == "1")
                {
                    studentgen = "2";
                }
                else if (studgender.Trim() == "2")
                {
                    studentgen = "0";
                }
                if (ddl_pop1collegename.SelectedItem.Text.Trim() != "" && ddl_pop1hostelname.SelectedItem.Text.Trim() != "" && txt_pop1rollno.Text.Trim() != "" && txt_pop1name.Text.Trim() != "" && txt_pop1roomno.Text.Trim() != "" && txt_pop1building.Text.Trim() != "" && txt_pop1floor.Text.Trim() != "" && txt_pop1admindate.Text.Trim() != "" && txt_discontinuedate.Text.Trim() != "" && txt_vacatedate.Text.Trim() != "" && txt_pop1roomtype.Text.Trim() != "" && ddlStudType.SelectedValue != "") //magesh 4.4.18 add ddlStudType.SelectedValue
                {
                    if (hostelgender.Trim() == studentgen.Trim() || hostelgender.Trim() == "0")
                    {
                        string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "' and Building_Name='" + txt_pop1building.Text + "'";
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(q, "text");
                        double comp1 = 0; double comp2 = 0;
                        double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString()), out comp1);
                        double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"]), out comp2);
                        if (comp1 >= comp2 && comp1 != comp2)
                        {
                            bool feeallot = false;
                            if (vecated.Trim() == "1" || discontinue.Trim() == "1")
                            {
                                string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "' and Building_Name='" + txt_pop1building.Text + "'";
                                int k = d2.update_method_wo_parameter(up, "text");
                            }
                            else
                            {
                                string up = " update Room_Detail set Avl_Student= isnull(Avl_Student,0) + 1 where Room_Type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "' and Building_Name='" + txt_pop1building.Text + "'";
                                int k = d2.update_method_wo_parameter(up, "text");
                            }
                            string quary = "insert into HT_HostelRegistration(MemType,APP_No,HostelAdmDate,BuildingFK,FloorFK,RoomFK,StudMessType,IsDiscontinued,DiscontinueDate,HostelMasterFK,collegecode,Messcode,id)values(1,'" + appnumber + "','" + txt_pop1admindate.Text + "','" + building + "','" + floor + "','" + roomno + "','" + studmesstype + "','" + discontinue + "','" + discontinuedate + "','" + ddl_pop1hostelname.SelectedValue + "','" + ddl_pop1collegename.SelectedItem.Value + "','" + Convert.ToString(ddlmess.SelectedItem.Value) + "','" + id + "')";
                            int h = d2.insert_method(quary, hat, "Text");
                            string regupdate = " update Registration set Stud_Type='Hostler' where App_No='" + appnumber + "'";
                            int regup = d2.update_method_wo_parameter(regupdate, "Text");
                            regupdate = " update applyn set stud_type='Hostler' where App_No='" + appnumber + "'";
                            regup = d2.update_method_wo_parameter(regupdate, "Text");
                            if (cb_Hostelfeesallot.Checked)
                            {
                                if (h != 0 && regup != 0)
                                {
                                    feeallot = Hostelfeesallot(Convert.ToString(ddl_pop1collegename.SelectedItem.Value), "", "", "", "", appnumber, Convert.ToString(ddl_pop1hostelname.SelectedValue), txt_pop1roomtype.Text);
                                    if (!feeallot)
                                    {
                                        imgdiv2.Visible = true;
                                        lblalerterr.Text = "Fees Not Alloted";
                                    }
                                }
                            }
                            else { feeallot = true; }
                            if (regup != 0 && feeallot)
                            {
                                imgdiv2.Visible = true;
                                lblalerterr.Text = "Saved Successfully";
                                idgeneration();
                            }
                            btn_add_Click(sender, e);
                            clear();
                        }
                        else
                        {
                            errmsg.Visible = true;
                            errmsg.Text = "Room fill please select another room";
                        }
                    }
                    else
                    {
                        txt_pop1roomno.Text = "";
                        txt_pop1building.Text = "";
                        txt_pop1floor.Text = "";
                        txt_pop3roomtype.Text = "";
                        errmsg.Visible = true;
                        errmsg.Text = "Please select valid hostel in this student";
                    }
                }
                else
                {
                    if (txt_pop1rollno.Text == "")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please enter student details";
                    }
                    else if (txt_pop1roomno.Text == "")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please enter room details";
                    }
                    else if (txt_pop1roomtype.Text == "")
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please update room type";
                    }
                    //magesh 4.4.18
                    else if (ddlStudType.SelectedValue == "")
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "Please set Student Mess type";
                    }
                }
                #endregion
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Please select room details";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop1save_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_pop1name.Text.Trim() != "")
            {
                savedetails();
                ViewState["buil"] = null;
                ViewState["fl"] = null;
                ViewState["ro"] = null;
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Visible = true;
                lblalerterr.Text = "Please enter student details";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop1exit_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
        clear();
    }
    public void btn_pop1update_Click(object sender, EventArgs e)
    {
        try
        {
            string vecated = "";
            string discontinue = "";
            string reason = "";
            string vecated_date = "";
            string discontinuedate = "";
            string reason_discontinue = "";
            DateTime vd = new DateTime();
            string studentname = "";
            Boolean Hostelsetting = false;

            if (cb_pop1vacate.Checked == true)
            {
                vecated = "1";
                vecated_date = Convert.ToString(txt_vacatedate.Text);
                string[] vdate = vecated_date.Split('/');
                vd = Convert.ToDateTime(vdate[1] + "/" + vdate[0] + "/" + vdate[2]);
                vecated_date = Convert.ToString(vd.ToString("MM/dd/yyyy"));
                reason = Convert.ToString(txt_pop1reason.Text);
                string appnumber1 = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
                string collcode = d2.GetFunction("select college_code  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
                string Hostelvacate = d2.GetFunction("select LinkValue from New_InsSettings where LinkName='Hostel Vacate Rights' and user_code ='" + user_code + "' and college_code ='" + collcode + "'");
                if (Hostelvacate != "")
                {

                    string vacate = HostelVacate(appnumber1, Hostelvacate);

                    if (vacate == "2")
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "This Student Hostel Fees Not Paid,So You Cannot Vacate The Student!";
                        return;
                    }
                    else if (vacate == "3")
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "This Student Request Will Be Waitnig,So You Cannot Vacate The Student!";
                        return;
                    }
                    else if (vacate == "4")
                    {
                        imgdiv2.Visible = true;
                        lblalerterr.Text = "You Cannot Vacate This Student,We Can Vacate The Student By Request Process!";
                        return;
                    }

                }
                else
                {

                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Please Set Hostel Vacate Rights!";
                    return;

                }
            }
            else
            {
                vecated = "0";
                vecated_date = "";
                reason = "";
            }
            if (cb_discontinue.Checked == true)
            {
                discontinue = "1";
                string todate = "";
                todate = Convert.ToString(txt_discontinuedate.Text);
                string[] splittodate = todate.Split('-');
                splittodate = splittodate[0].Split('/');
                DateTime dttodate = new DateTime();
                if (splittodate.Length > 0)
                {
                    dttodate = Convert.ToDateTime(splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2]);
                }
                discontinuedate = dttodate.ToString("MM/dd/yyyy");
                reason_discontinue = Convert.ToString(txt_pop1reason.Text);
            }
            else
            {
                discontinue = "0";
                discontinuedate = "";
                reason_discontinue = "";
            }
            string dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = DateTime.Now.ToLongTimeString();
            string getdate = Convert.ToString(txt_pop1admindate.Text);
            string[] splitdate = getdate.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            bool checkvalue = false;
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string activerow = "";
            string activecol = "";
            activerow = fproll.ActiveSheetView.ActiveRow.ToString();
            activecol = fproll.ActiveSheetView.ActiveColumn.ToString();
            string building = "";
            string floor = "";
            string roomno = "";
            string appnumber = "";
            building = Convert.ToString(ViewState["Code"]);
            floor = Convert.ToString(ViewState["Floorpk"]);
            roomno = Convert.ToString(ViewState["Roompk"]);
            appnumber = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
            ViewState["App_No"] = Convert.ToString(appnumber);
            ViewState["Id"] = txtid.Text;
            string admindate = dt.ToString("MM/dd/yyyy");
            string studmesstype = string.Empty;
            //if (Radiobtnstype.Items[0].Selected == true)
            //{
            //    studmesstype = "0";
            //}
            //else if (Radiobtnstype.Items[1].Selected == true)
            //{
            //    studmesstype = "1";
            //}
            //else
            //{
            //}
            int messtype = 0;
            int.TryParse(Convert.ToString(ddlStudType.SelectedValue), out messtype);
            if (messtype > 0)
                studmesstype = Convert.ToString(messtype - 1);
            #region roomupdation
            string hostelgender = d2.GetFunction("select HostelType from HM_HostelMaster where HostelMasterPK ='" + ddl_pop1hostelname.SelectedValue + "'");
            string studgender = d2.GetFunction("select sex from applyn a,Registration r where a.app_no =r.App_No and Roll_No='" + txt_pop1rollno.Text + "'");
            string studentgen = "";
            if (studgender.Trim() == "0")
            {
                studentgen = "1";
            }
            else if (studgender.Trim() == "1")
            {
                studentgen = "2";
            }
            else if (studgender.Trim() == "2")
            {
                studentgen = "0";
            }
            if (ddl_pop1collegename.Text.ToString() != "" && ddl_pop1hostelname.Text.ToString() != "" && txt_pop1rollno.Text != "" && txt_pop1name.Text != "" && txt_pop1roomno.Text != "" && txt_pop1building.Text != "" && txt_pop1floor.Text != "" && txt_pop1admindate.Text != "")
            {
                if (hostelgender.Trim() == studentgen.Trim() || hostelgender.Trim() == "0")
                {
                    string activerow1 = Fpspread1.ActiveSheetView.ActiveRow.ToString();
                    string activecol1 = Fpspread1.ActiveSheetView.ActiveColumn.ToString();
                    string rollnum = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(activerow1), 1].Text);
                    string q1 = " select distinct hd.BuildingFK,hd.FloorFK,hd.RoomFK,r.Building_Name,r.Floor_Name,r.Room_Name,r.Room_type from Room_Detail r,HT_HostelRegistration hd,Floor_Master fm,Building_Master bm where hd.App_No='" + appnumber + "' and r.Roompk=hd.RoomFK and fm.Floorpk=hd.FloorFK and bm.Code=hd.BuildingFK";
                    ds3.Clear();
                    q1 += " select stud_name from registration where app_no='" + appnumber + "'";
                    ds3 = d2.select_method_wo_parameter(q1, "text");
                    studentname = Convert.ToString(ds3.Tables[1].Rows[0][0]);
                    string bulname = "";
                    bulname = Convert.ToString(ds3.Tables[0].Rows[0]["Building_Name"].ToString());
                    string flrname = "";
                    flrname = Convert.ToString(ds3.Tables[0].Rows[0]["Floor_Name"].ToString());
                    string roomname = "";
                    roomname = Convert.ToString(ds3.Tables[0].Rows[0]["Room_Name"].ToString());
                    string roomtype = "";
                    roomtype = Convert.ToString(ds3.Tables[0].Rows[0]["Room_type"].ToString());
                    string dbbuildfk = Convert.ToString(ds3.Tables[0].Rows[0]["BuildingFK"].ToString());
                    string dbfloorfk = Convert.ToString(ds3.Tables[0].Rows[0]["FloorFK"].ToString());
                    string dbroomfk = Convert.ToString(ds3.Tables[0].Rows[0]["RoomFK"].ToString());
                    int k = 0;
                    string reasonds = "";
                    string reasoncode = "";
                    string DC = "";
                    if (discontinue.Trim() == "1" && vecated.Trim() == "1")
                    {
                        DC = "HSDSC";
                        reasonds = Convert.ToString(txt_pop1reason.Text);
                        reasoncode = subjectcodenew(DC, reasonds);
                    }
                    else if (vecated.Trim() == "1")
                    {
                        DC = "HSVAC"; reasonds = Convert.ToString(txt_pop1reason.Text);
                        reasoncode = subjectcodevac(DC, reasonds);
                    }
                    else if (discontinue.Trim() == "1")
                    {
                        DC = "HSDSC";
                        reasonds = Convert.ToString(txt_pop1reason.Text);
                        reasoncode = subjectcodenew(DC, reasonds);
                    }
                    else
                    {
                    }
                    if (reasoncode.Trim() == "")
                    {
                        reasoncode = "0";
                    }
                    if (roomname.Trim() == txt_pop1roomno.Text.Trim() && bulname.Trim() == txt_pop1building.Text.Trim() && roomtype.Trim() == txt_pop1roomtype.Text.Trim() && flrname.Trim() == txt_pop1floor.Text.Trim())
                    {
                        if (vecated.Trim() == "1" && discontinue.Trim() == "1")
                        {
                            string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                            k = d2.update_method_wo_parameter(up, "text");
                            string del = "update Registration set DelFlag='1',Stud_Type='Day Scholar'  where APP_No ='" + appnumber + "'";
                            int s = d2.update_method_wo_parameter(del, "Text");
                            string regupdate = " update applyn set stud_type='Day Scholar' where App_No='" + appnumber + "'";
                            int regup = d2.update_method_wo_parameter(regupdate, "Text");
                        }
                        else if (vecated.Trim() == "1")
                        {


                            string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                            k = d2.update_method_wo_parameter(up, "text");
                            //magesh 22.5.18
                            string del = "update Registration set Stud_Type='Day Scholar' where APP_No ='" + appnumber + "'";
                            int s = d2.update_method_wo_parameter(del, "Text");
                            string regupdate = " update applyn set stud_type='Day Scholar' where App_No='" + appnumber + "'";
                            int regup = d2.update_method_wo_parameter(regupdate, "Text");

                        }
                        else if (discontinue.Trim() == "1")
                        {
                            string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                            k = d2.update_method_wo_parameter(up, "text");
                            string del = "update Registration set DelFlag='1' where APP_No ='" + appnumber + "'";
                            int s = d2.update_method_wo_parameter(del, "Text");
                        }
                        else
                        {
                            //string up = " update Room_Detail set Avl_Student= Avl_Student + 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                            //k = d2.update_method_wo_parameter(up, "text");
                        }
                    }
                    else
                    {
                        string up = "update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                        k = d2.update_method_wo_parameter(up, "text");
                        string up1 = " update Room_Detail set Avl_Student= Avl_Student + 1 where Room_Type='" + Convert.ToString(txt_pop1roomtype.Text.Trim()) + "' and Floor_Name='" + Convert.ToString(txt_pop1floor.Text.Trim()) + "' and Room_Name='" + Convert.ToString(txt_pop1roomno.Text.Trim()) + "' and Building_Name='" + Convert.ToString(txt_pop1building.Text.Trim()) + "'";
                        k = d2.update_method_wo_parameter(up1, "text");
                        if (vecated.Trim() == "1")
                        {
                            string del = "update Registration set Stud_Type='Day Scholar' where APP_No ='" + appnumber + "'";
                            int s = d2.update_method_wo_parameter(del, "Text");
                            string regupdate = " update applyn set stud_type='Day Scholar' where App_No='" + appnumber + "'";
                            int regup = d2.update_method_wo_parameter(regupdate, "Text");
                        }
                    }
                    string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "' and Building_Name='" + txt_pop1building.Text + "'";
                    ds2.Clear();
                    string comp1 = string.Empty;
                    string comp2 = string.Empty;
                    ds2 = d2.select_method_wo_parameter(q, "text");
                    if (ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                    {
                        comp1 = Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString());
                        comp2 = Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"].ToString());
                    }
                    string query = "";
                    if (dbbuildfk.Trim() == building && dbfloorfk.Trim() == floor && dbroomfk.Trim() == roomno)
                    {
                        query = "update  HT_HostelRegistration set MemType='1',HostelAdmDate='" + admindate + "',StudMessType='" + studmesstype + "',IsDiscontinued='" + discontinue + "',DiscontinueDate='" + discontinuedate + "',Reason='" + reasoncode + "',IsVacated='" + vecated + "',VacatedDate='" + vecated_date + "',collegecode='" + ddl_pop1collegename.SelectedItem.Value + "',Messcode='" + Convert.ToString(ddlmess.SelectedItem.Value) + "',id='" + txtid.Text + "'  where HostelMasterFK='" + ddl_pop1hostelname.SelectedValue + "'  and APP_No ='" + appnumber + "'";
                        int h = d2.insert_method(query, hat, "Text");
                        if (h != 0)
                        {
                            checkvalue = true;
                        }
                        clear();
                    }
                    else
                    {
                        //magesh 27.5.18
                        if (comp2 == "")
                            comp2 = "0";
                        if (Convert.ToInt32(comp1) >= Convert.ToInt32(comp2))//&& Convert.ToInt32(comp1) == Convert.ToInt32(comp2)
                        {
                            query = "update  HT_HostelRegistration set MemType='1',HostelAdmDate='" + admindate + "',BuildingFK='" + building + "',FloorFK='" + floor + "',RoomFK='" + roomno + "',StudMessType='" + studmesstype + "',IsDiscontinued='" + discontinue + "',DiscontinueDate='" + discontinuedate + "',Reason='" + reasoncode + "',IsVacated='" + vecated + "',VacatedDate='" + vecated_date + "',collegecode='" + ddl_pop1collegename.SelectedItem.Value + "',Messcode='" + Convert.ToString(ddlmess.SelectedItem.Value) + "',id='" + txtid.Text + "'  where HostelMasterFK='" + ddl_pop1hostelname.SelectedValue + "'  and APP_No ='" + appnumber + "'";
                            int h = d2.insert_method(query, hat, "Text");
                            if (h != 0)
                            {
                                checkvalue = true;
                            }
                            clear();
                        }
                        else
                        {
                            string up = "update Room_Detail set Avl_Student= Avl_Student - 1 where Room_Type='" + roomtype + "' and Floor_Name='" + flrname + "' and Room_Name='" + roomname + "' and Building_Name='" + bulname + "'";
                            k = d2.update_method_wo_parameter(up, "text");
                            errmsg.Visible = true;
                            errmsg.Text = "Room fill please select another room";
                        }
                    }
                    if (cb_pop1vacate.Checked == true)
                    {
                        string smsrights = d2.GetFunction("select value from Master_Settings where settings='Hostel Vacated sms' and usercode='" + usercode + "'");
                        if (smsrights.Trim() == "1")
                        {
                            string messagetext = ""; string sendno = "";
                            string sendsmsfor = d2.GetFunction("select value from Master_Settings where settings='SMS Mobile Rights' and usercode='" + usercode + "'");
                            string userid = d2.GetFunction(" select sms_user_id from Track_Value where college_code='" + Convert.ToString(ddl_pop1collegename.SelectedItem.Value) + "'");
                            messagetext = " Hi " + Convert.ToString(studentname) + " your vacate the " + Convert.ToString(ddl_pop1hostelname.SelectedItem.Text) + ", Hostel Details : Building Name:" + bulname + ", Floor Name:" + flrname + ", RoomName:" + roomname + ", Room Type=" + roomtype + "  \n Thank you.";
                            if (messagetext.Trim() != "")
                            {
                                string mobile = d2.GetFunction("select ParentF_Mobile+'-'+ParentM_Mobile+'-'+Student_Mobile from applyn where app_no='" + appnumber + "'").Trim();
                                if (mobile.Trim() != "--")
                                {
                                    sendno = "";
                                    string[] sendmobilenumber = mobile.Split('-');
                                    string[] numbers = sendsmsfor.Split(',');
                                    if (numbers.Length > 0)
                                    {
                                        foreach (string no in numbers)
                                        {
                                            if (no == "1")
                                                sendno = "," + Convert.ToString(sendmobilenumber[0]);
                                            if (no == "2")
                                                sendno = "," + Convert.ToString(sendmobilenumber[1]);
                                            if (no == "3")
                                                sendno = "," + Convert.ToString(sendmobilenumber[2]);
                                        }
                                        if (sendno != "0")
                                        {
                                            d2.send_sms(userid, Convert.ToString(ddl_pop1collegename.SelectedItem.Value), usercode, sendno.TrimStart(','), messagetext, "0");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    txt_pop1roomno.Text = "";
                    txt_pop1building.Text = "";
                    txt_pop1floor.Text = "";
                    txt_pop3roomtype.Text = "";
                    errmsg.Visible = true;
                    errmsg.Text = "Please select valid hostel in this student";
                }
            }
            else
            {
                if (txt_pop1rollno.Text == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please enter student details";
                }
                else if (txt_pop1roomno.Text == "")
                {
                    errmsg.Visible = true;
                    errmsg.Text = "Please enter room details";
                }
            }
            #endregion
            if (checkvalue == true)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Updated Successfully";
                popwindow1.Visible = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void delete()
    {
        try
        {
            string appnumber = "";
            appnumber = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
            // ViewState["App_No"] = Convert.ToString(appnumber);
            surediv.Visible = false;
            string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Room_type='" + txt_pop1roomtype.Text + "' and Floor_Name='" + txt_pop1floor.Text + "' and Room_Name='" + txt_pop1roomno.Text + "'";
            int k1 = d2.update_method_wo_parameter(up, "text");
            string del = "update Registration set Stud_Type='Day Scholar' where Roll_No='" + txt_pop1rollno.Text + "'";
            int s = d2.update_method_wo_parameter(del, "Text");
            string regupdate = " update applyn set stud_type='Day Scholar' where App_No='" + appnumber + "'";
            int regup = d2.update_method_wo_parameter(regupdate, "Text");
            string del1 = "delete HT_HostelRegistration where APP_No='" + appnumber + "'";
            int j = d2.update_method_wo_parameter(del1, "Text");
            imgdiv2.Visible = true;
            lblalerterr.Text = "Deleted Successfully";
            clear();
            popwindow1.Visible = false;
            //btn_go_Click(sender, e);
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop1delete_Click(object sender, EventArgs e)
    {
        try
        {
            if (btn_pop1delete.Text == "Delete")
            {
                surediv.Visible = true;
                lbl_sure.Text = "Do you want to Delete this Record?";
            }
        }
        catch
        {
        }
    }
    public void btn_pop1exit1_Click(object sender, EventArgs e)
    {
        popwindow1.Visible = false;
    }
    /*popwindow2 Select Student*/
    protected void imagebtnpop2close_Click(object sender, EventArgs e)
    {
        popwindow2.Visible = false;
    }
    protected void bindpop2collegename()
    {
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
        catch
        {
        }
    }
    public void branch()
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
    public void ddl_pop2degre_SelectedIndexChanged(object sender, EventArgs e)
    {
        branch();
    }
    protected void bindpop2batchyear()
    {
        try
        {
            //ddlpop2batchyr.Items.Clear();
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
        catch
        {
        }
    }
    public void ldroll()
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
            //  if (ddl_pop2studenttype.SelectedItem.Text != "")
            if (ddl_pop2studenttype.SelectedItem.Text != "Both")
            {
                string buildvalue2 = "";
                string build2 = "";
                build2 = ddl_pop2studenttype.SelectedValue.ToString();
                if (buildvalue2 == "")
                {
                    buildvalue2 = build2;
                }
                else
                {
                    buildvalue2 = buildvalue2 + "'" + "," + "'" + build2;
                }
                if (buildvalue2 != "")
                {
                    sqladd = sqladd + " AND r.stud_type in ('" + buildvalue2 + "')";
                }
                else
                {
                    sqladd = sqladd + "";
                }
            }
            if (Rollflag1 == "1")
            {
                fproll.Columns[1].Visible = true;
                fproll.Width = 426;
            }
            else
            {
                fproll.Columns[1].Visible = false;
                fproll.Width = 326;
            }
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
            fproll.Sheets[0].RowCount = 0;
            fproll.Sheets[0].RowHeader.Visible = false;
            fproll.SaveChanges();
            fproll.Sheets[0].AutoPostBack = false;
            ds.Clear();
            string q = sqladd + strorderby;
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count <= 0)
            {
                //div2.Visible = false;
                fproll.Visible = false;
                lblcounttxt.Visible = false;
                lblcount.Visible = false;
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
                fproll.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fproll.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                    fproll.Columns[0].Locked = true;
                    fproll.Columns[1].Locked = true;
                    fproll.Columns[2].Locked = true;
                    fproll.Columns[3].Locked = true;
                    int sno = 0;
                    lblpop2error.Visible = false;
                    lblcounttxt.Visible = true;
                    lblcounttxt.Text = "No of Students:";
                    lblcount.Visible = true;
                    lblcount.Text = Convert.ToString(ds.Tables[0].Rows.Count);
                    fproll.Visible = true;
                    //div2.Visible = true;
                    fproll.CommandBar.Visible = false;
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
                                fproll.Sheets[0].RowCount = fproll.Sheets[0].RowCount + 1;
                                fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["Degree_Code"]);
                                fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(dv[0]["branch"]);
                                fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                fproll.Sheets[0].AddSpanCell(fproll.Sheets[0].RowCount - 1, 0, 1, 4);
                                sno++;
                                for (int row = 0; row < dv.Count; row++)
                                {
                                    studcount++;
                                    fproll.Sheets[0].RowCount++;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].CellType = txt;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(dv[0]["App_No"]);
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dv[row]["roll_no"]);
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].CellType = txt;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dv[0]["branch"]);
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].CellType = txt;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dv[row]["stud_name"]);
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                    fproll.Sheets[0].Cells[fproll.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                                }
                            }
                        }
                    }
                    int rowcount = fproll.Sheets[0].RowCount;
                    fproll.Height = 270;
                    fproll.Sheets[0].PageSize = 15 + (rowcount * 5);
                    fproll.SaveChanges();
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadroll()
    {
        try
        {
            string hostelsex = d2.GetFunction("select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            hostelsex = " and a.sex in(" + hostelsex + ")";
            string studtype = "";
            if (ddl_pop2studenttype.SelectedItem.Text != "Both")
            {
                studtype = " and r.Stud_Type in ('" + Convert.ToString(ddl_pop2studenttype.SelectedItem.Text) + "')";
            }
            else
            {
                studtype = "";
            }
            if (ddl_pop2sex.SelectedItem.Text == "All")
            {
                sqladd = "select r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code  and g.college_code = d.college_code and r.Batch_Year ='" + ddl_pop2batchyear.SelectedItem + "' and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'' and ISNULL(IsVacated,'0')='0') " + hostelsex + " " + studtype + " and r.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedValue) + "'";
                ldroll();
            }
            else if (ddl_pop2sex.SelectedItem.Text == "Male")
            {
                sqladd = "select r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='0' and g.college_code = d.college_code and r.Batch_Year ='" + ddl_pop2batchyear.SelectedItem + "' and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'' and ISNULL(IsVacated,'0')='0') " + hostelsex + " " + studtype + " and r.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedValue) + "' ";
                ldroll();
            }
            else if (ddl_pop2sex.SelectedItem.Text == "Female")
            {
                sqladd = "select r.App_No, roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='1' and g.college_code = d.college_code and r.Batch_Year ='" + ddl_pop2batchyear.SelectedItem + "' and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'' and ISNULL(IsVacated,'0')='0')" + hostelsex + " " + studtype + " and r.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedValue) + "' ";
                ldroll();
            }
            else
            {
                sqladd = "select r.App_No,roll_no,r.stud_name,g.Degree_Code,course_name+'-'+dept_name branch,roll_admit from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag<>'debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex ='2' and g.college_code = d.college_code and r.Batch_Year ='" + ddl_pop2batchyear.SelectedItem + "' and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'' and ISNULL(IsVacated,'0')='0') " + hostelsex + " " + studtype + " and r.college_code='" + Convert.ToString(ddl_pop2collgname.SelectedValue) + "' ";
                ldroll();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop2go_Click(object sender, EventArgs e)
    {
        loadroll();
    }
    public void btn_pop2ok_Click(object sender, EventArgs e)
    {
        try
        {
            popwindow2.Visible = false;
            string activerow = "";
            string activecol = "";
            string appno = " ";
            activerow = fproll.ActiveSheetView.ActiveRow.ToString();
            activecol = fproll.ActiveSheetView.ActiveColumn.ToString();
            string purpose = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text;
            string retroll = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text;
            appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + purpose + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            ViewState["Id"] = txtid.Text;
            string branch = Convert.ToString(fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Tag);
            string name = fproll.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Text;
            txt_pop1rollno.Text = purpose;
            txt_pop1rolladmin.Text = retroll;
            txt_pop1degree.Text = branch;
            txt_pop1name.Text = name;
            string sql = " select Textval,Student_Mobile from applyn a, Registration b,textvaltable c where a.app_no =b.App_No and c.TextCode=a.community and b.Roll_No='" + txt_pop1rollno.Text + "'";
            ds = d2.select_method_wo_parameter(sql, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string comm = ds.Tables[0].Rows[0]["Textval"].ToString();
                txt_pop1community.Text = comm;
                string mobile = ds.Tables[0].Rows[0]["Student_Mobile"].ToString();
                txt_pop1mob.Text = mobile;
            }
            string sql1 = "select parent_addressP,Streetp,Cityp+'-'+Districtp city,parent_pincodep from applyn a, Registration b where a.app_no =b.App_No and  Roll_No ='" + txt_pop1rollno.Text + "'";
            ds = d2.select_method_wo_parameter(sql1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string add = ds.Tables[0].Rows[0]["parent_addressP"].ToString();
                txt_pop1address.Text = add;
                string add2 = ds.Tables[0].Rows[0]["Streetp"].ToString();
                txt_pop1address1.Text = add2;
                string add3 = ds.Tables[0].Rows[0]["city"].ToString();
                txt_pop1address2.Text = add3;
                string add5 = ds.Tables[0].Rows[0]["parent_pincodep"].ToString();
                txt_pop1pin.Text = add5;
            }
            string dob1 = "select dob,b.college_code  from applyn a,Registration b where a.app_no=b.App_No and b.Roll_No='" + txt_pop1rollno.Text + "'";
            ds = d2.select_method_wo_parameter(dob1, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string dob = ds.Tables[0].Rows[0]["dob"].ToString();
                string collegecode = ds.Tables[0].Rows[0]["college_code"].ToString();
                ddl_pop1collegename.SelectedIndex = ddl_pop1collegename.Items.IndexOf(ddl_pop1collegename.Items.FindByValue(collegecode));
                datesp = dob.Split(' ');
                dob = datesp[0].ToString();
                string[] db = dob.Split('/');
                dob = db[1].ToString() + "/" + db[0].ToString() + "/" + db[2].ToString();
                txt_date.Text = Convert.ToString(dob);
            }
            string sql2 = "select textval from applyn a,textvaltable t,Registration b where a.bldgrp = t.TextCode and a.app_no =b.App_No and  Roll_No='" + txt_pop1rollno.Text + "'";
            ds = d2.select_method_wo_parameter(sql2, "Text");
            {
                string blood = ds.Tables[0].Rows[0]["textval"].ToString();
                txt_pop1blood.Text = blood;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop2exit_Click(object sender, EventArgs e)
    {
        popwindow2.Visible = false;
    }
    /*popwindow3  Select Room */
    protected void imagebtnpop3close_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
    }
    public void bindbuilding()
    {
        try
        {
            cbl_pop3build.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(ddl_pop1hostelname.SelectedItem.Value);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop3build.DataSource = ds;
                cbl_pop3build.DataTextField = "Building_Name";
                cbl_pop3build.DataValueField = "code";
                cbl_pop3build.DataBind();
            }
            else
            {
                txt_pop3build.Text = "--Select--";
            }
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                cbl_pop3build.Items[i].Selected = true;
                txt_pop3build.Text = "Building(" + (cbl_pop3build.Items.Count) + ")";
                cb_pop3build.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    string builname = cbl_pop3build.Items[i].Text;
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
            bindroompopfloor(locbuild);
        }
        catch
        {
        }
    }
    public void cb_pop3build_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_pop3build.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                {
                    if (cb_pop3build.Checked == true)
                    {
                        cbl_pop3build.Items[i].Selected = true;
                        txt_pop3build.Text = "Build(" + (cbl_pop3build.Items.Count) + ")";
                        txt_pop3floor.Text = "--Select--";
                        txt_pop3roomtype.Text = "--Select--";
                        build1 = cbl_pop3build.Items[i].Text.ToString();
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
                bindroompopfloor(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_pop3build.Items.Count; i++)
                {
                    cbl_pop3build.Items[i].Selected = false;
                    txt_pop3build.Text = "--Select--";
                    cbl_pop3floor.ClearSelection();
                    cbl_pop3roomtype.ClearSelection();
                    cb_pop3floor.Checked = false;
                    cb_pop3roomtype.Checked = false;
                    txt_pop3floor.Text = "--Select--";
                    txt_pop3roomtype.Text = "--Select--";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop3build_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3build.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_pop3floor.Text = "--Select--";
                    txt_pop3roomtype.Text = "--Select--";
                    cb_pop3floor.Checked = false;
                    cb_pop3roomtype.Checked = false;
                    build = cbl_pop3build.Items[i].Text.ToString();
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
            bindroompopfloor(buildvalue);
            if (seatcount == cbl_pop3build.Items.Count)
            {
                txt_pop3build.Text = "Build(" + seatcount + ")";
                cb_pop3build.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_pop3build.Text = "--Select--";
            }
            else
            {
                txt_pop3build.Text = "Build(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroompopfloor(string buildname)
    {
        try
        {
            cbl_pop3floor.Items.Clear();
            //ds = d2.BindFloor(buildname);
            string itemname = "select Floor_Name,Floorpk  from Floor_Master where Building_Name in('" + buildname + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(itemname, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_pop3floor.DataSource = ds;
                cbl_pop3floor.DataTextField = "Floor_Name";
                cbl_pop3floor.DataValueField = "Floorpk";
                cbl_pop3floor.DataBind();
            }
            else
            {
                txt_pop3floor.Text = "--Select--";
            }
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                cbl_pop3floor.Items[i].Selected = true;
                cb_pop3floor.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                    string flrname = cbl_pop3floor.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
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
            clgroomtype(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_pop3floor_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_pop3roomtype.Text = "--Select--";
            if (cb_pop3floor.Checked == true)
            {
                string buildvalue1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_pop3build.Checked == true)
                {
                    buildvalue1 = rs.GetSelectedItemsText(cbl_pop3build);
                    if (cb_pop3floor.Checked == true)
                    {
                        for (int j = 0; j < cbl_pop3floor.Items.Count; j++)
                        {
                            cbl_pop3floor.Items[j].Selected = true;
                            txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                            txt_pop3roomtype.Text = "--Select--";
                            build2 = cbl_pop3floor.Items[j].Text.ToString();
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
                }
                if (cb_pop3build.Checked == false)
                {
                    buildvalue1 = rs.GetSelectedItemsText(cbl_pop3build);
                }
                if (cb_pop3floor.Checked == true)
                {
                    for (int j = 0; j < cbl_pop3floor.Items.Count; j++)
                    {
                        cbl_pop3floor.Items[j].Selected = true;
                        txt_pop3floor.Text = "Floor(" + (cbl_pop3floor.Items.Count) + ")";
                        txt_pop3roomtype.Text = "--Select--";
                        build2 = cbl_pop3floor.Items[j].Text.ToString();
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
                clgroomtype(buildvalue1, buildvalue2);
            }
            else
            {
                for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
                {
                    cbl_pop3floor.Items[i].Selected = false;
                    txt_pop3floor.Text = "--Select--";
                    cbl_pop3roomtype.ClearSelection();
                    cb_pop3roomtype.Checked = false;
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop3floor_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3floor.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    build1 = cbl_pop3build.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_pop3roomtype.Text = "--Select--";
                    build2 = cbl_pop3floor.Items[i].Text.ToString();
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
            clgroomtype(buildvalue1, buildvalue2);
            if (seatcount == cbl_pop3floor.Items.Count)
            {
                txt_pop3floor.Text = "Floor(" + seatcount.ToString() + ")";
                cb_pop3floor.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_pop3floor.Text = "--Select--";
            }
            else
            {
                txt_pop3floor.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgroomtype(string floor, string room)
    {
        try
        {
            if (floor != "" && room != "")
            {
                ds.Clear();
                ds = d2.BindRoomtype(floor, room);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_pop3roomtype.DataSource = ds;
                    cbl_pop3roomtype.DataTextField = "Room_Type";
                    cbl_pop3roomtype.DataValueField = "Room_Type";
                    cbl_pop3roomtype.DataBind();
                }
                else
                {
                    txt_pop3roomtype.Text = "--Select--";
                    cbl_pop3roomtype.ClearSelection();
                }
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = true;
                    txt_pop3roomtype.Text = "Room(" + (cbl_pop3roomtype.Items.Count) + ")";
                    cb_pop3roomtype.Checked = true;
                }
            }
            else
            {
                txt_pop3roomtype.Text = "--Select--";
                cbl_pop3roomtype.ClearSelection();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_pop3roomtype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_pop3roomtype.Checked == true)
            {
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = true;
                }
                txt_pop3roomtype.Text = "Room(" + (cbl_pop3roomtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
                {
                    cbl_pop3roomtype.Items[i].Selected = false;
                }
                txt_pop3roomtype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_pop3roomtype_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_pop3roomtype.Checked = false;
            for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
            {
                if (cbl_pop3roomtype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_pop3roomtype.Items.Count)
            {
                txt_pop3roomtype.Text = "Room(" + seatcount.ToString() + ")";
                cb_pop3roomtype.Checked = true;
            }
            else if (seatcount == 0)
            {
                cbl_pop3roomtype.Text = "--Select--";
            }
            else
            {
                txt_pop3roomtype.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void chck1_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chck1.Checked == true)
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < roomchecklist.Items.Count; i++)
                {
                    roomchecklist.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop3save_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread3.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread3.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) != -1 && Convert.ToInt32(activecol) != -1)
            {
                if (Convert.ToInt32(activecol) != 0)
                {
                    //29.10.15
                    string purpose = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                    string room1 = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    // string building_value = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                    string building_value = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol) - 1].Tag);
                    if (purpose.Trim() != "")
                    {
                        address = purpose.Split('-');
                        purpose = address[0].ToString();
                        fr = room1.Split('-');
                        string rname = fr[0].ToString();
                        string rtype = "";
                        if (fr.Length == 3)
                        {
                            rtype = fr[1].ToString() + "-" + fr[2].ToString();
                        }
                        else
                        {
                            rtype = fr[1].ToString();
                        }
                        string building = "";
                        string build_name = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol) - 1].Tag);
                        string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + rtype + "' and Floor_Name='" + fr[0].ToString() + "' and Room_Name='" + purpose + "' and Building_Name='" + build_name + "'";
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(q, "text");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            string comp1 = Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString());
                            string comp2 = Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"].ToString());
                            //magesh 20.5.18
                            if (comp2 == "")
                                comp2 = "0";
                            if (Convert.ToInt32(comp1) >= Convert.ToInt32(comp2) && Convert.ToInt32(comp1) != Convert.ToInt32(comp2))
                            {
                                if (FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                                {
                                    txt_pop1roomno.Text = purpose;
                                    //string build = Convert.ToString(cbl_pop3build.Items[0].Text);
                                    txt_pop1building.Text = building_value;
                                    string buil = "";
                                    building = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol) - 1].Tag);
                                    buil = d2.GetFunction("select Code  from Building_Master sm where  sm.Building_Name='" + building + "'");
                                    ViewState["Code"] = Convert.ToString(buil);
                                    string floorroom = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                    fr = floorroom.Split('-');
                                    txt_pop1floor.Text = fr[0].ToString();
                                    if (txt_pop1floor.Text == "")
                                    {
                                        txt_pop1floor.Text = fr[1].ToString();
                                    }
                                    // string floor = "";
                                    string fl = "";
                                    fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + building + "'");
                                    ViewState["Floorpk"] = Convert.ToString(fl);
                                    string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_pop1roomno.Text + "' and Building_Name ='" + building + "'";
                                    ds = d2.select_method_wo_parameter(room, "Text");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                                        txt_pop1roomtype.Text = roomtype;
                                        popwindow3.Visible = false;
                                    }
                                    string roomname = "";
                                    string ro = "";
                                    roomname = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                                    ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                                    ViewState["Roompk"] = Convert.ToString(ro);
                                    if (txt_pop3roomtype.Text == "")
                                    {
                                        lblpop3err.Visible = true;
                                        lblpop3err.Text = "Please select  room";
                                        txt_pop1floor.Text = "";
                                        txt_pop1building.Text = "";
                                        txt_pop3roomtype.Text = "";
                                    }
                                }
                                else
                                {
                                    lblpop3err.Visible = true;
                                    lblpop3err.Text = "Please select unfilled room";
                                }
                                if (FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                                {
                                    txt_pop1roomno.Text = purpose;
                                    string build = cbl_buildname.SelectedItem.Text.ToString();
                                    string floorroom = FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                    fr = floorroom.Split('-');
                                    txt_pop1floor.Text = fr[0].ToString();
                                    if (fr.Length == 3)
                                    {
                                        txt_pop1roomtype.Text = fr[1].ToString() + "-" + fr[2].ToString();
                                    }
                                    else
                                    {
                                        txt_pop1roomtype.Text = fr[1].ToString();
                                    }
                                    string fl = "";
                                    fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0].ToString() + "'and Building_Name ='" + building + "'");
                                    ViewState["Floorpk"] = Convert.ToString(fl);
                                    string NON = "NON";
                                    if (txt_pop1roomtype.Text == NON)
                                    {
                                        txt_pop1roomtype.Text = txt_pop1roomtype.Text + "-AC";
                                        popwindow3.Visible = false;
                                    }
                                    if (txt_pop1roomno.Text == "")
                                    {
                                        lblpop3err.Visible = true;
                                        lblpop3err.Text = "Please select  room";
                                        txt_pop1floor.Text = "";
                                        txt_pop1roomtype.Text = "";
                                        txt_pop1building.Text = "";
                                    }
                                    string roomname = "";
                                    string ro = "";
                                    roomname = Convert.ToString(FpSpread3.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                                    ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + building + "'");
                                    ViewState["Roompk"] = Convert.ToString(ro);
                                }
                                else
                                {
                                    lblpop3err.Visible = true;
                                    lblpop3err.Text = "Please select unfilled room";
                                }
                            }
                            else
                            {
                                lblpop3err.Visible = true;
                                lblpop3err.Text = " Room fill please select other room";
                            }
                        }
                        else
                        {
                            lblpop3err.Visible = true;
                            lblpop3err.Text = "Please select correct room";
                        }
                    }
                }
                else
                {
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "Please select correct room";
                }
            }
            else
            {
                lblpop3err.Visible = true;
                lblpop3err.Text = "Please select room";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_pop3exit_Click(object sender, EventArgs e)
    {
        popwindow3.Visible = false;
        cbl_buildname.Items.Clear();
        cbl_pop3floor.Items.Clear();
        cbl_pop3roomtype.Items.Clear();
        FpSpread3.Visible = false;
    }
    public void roomchecklist_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (roomchecklist.Items[0].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[1].Selected == false)
            {
                chck1.Checked = false;
            }
            if (roomchecklist.Items[2].Selected == false)
            {
                chck1.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void search()
    {
        try
        {
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpread3.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            toalrooms.Visible = true;
            totalvaccants.Visible = true;
            fill.Visible = true;
            partialfill.Visible = true;
            unfill.Visible = true;
            btn_pop3save.Visible = true;
            btn_pop3exit.Visible = true;
            FpSpread3.Sheets[0].AutoPostBack = false;
            FpSpread3.Sheets[0].ColumnCount = 0;
            FpSpread3.Sheets[0].RowCount = 0;
            string hostelcode = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);
            string building = "";
            for (int i = 0; i < cbl_pop3build.Items.Count; i++)
            {
                if (cbl_pop3build.Items[i].Selected == true)
                {
                    if (building == "")
                    {
                        building = "" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        building = building + "'" + "," + "'" + cbl_pop3build.Items[i].Text.ToString() + "";
                    }
                }
            }
            string vaccanttype = Convert.ToString(ddl_pop3vaccant.SelectedItem.Text);
            string floor = "";
            for (int i = 0; i < cbl_pop3floor.Items.Count; i++)
            {
                if (cbl_pop3floor.Items[i].Selected == true)
                {
                    if (floor == "")
                    {
                        floor = "" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                    else
                    {
                        floor = floor + "'" + "," + "'" + cbl_pop3floor.Items[i].Text.ToString() + "";
                    }
                }
            }
            string roomtype0 = "";
            for (int i = 0; i < cbl_pop3roomtype.Items.Count; i++)
            {
                if (cbl_pop3roomtype.Items[i].Selected == true)
                {
                    if (roomtype0 == "")
                    {
                        roomtype0 = "" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                    else
                    {
                        roomtype0 = roomtype0 + "'" + "," + "'" + cbl_pop3roomtype.Items[i].Value.ToString() + "";
                    }
                }
            }
            if (floor.Trim() != "" && roomtype0.Trim() != "" && building.Trim() != "")
            {
                // 24.02.16
                // string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name left join HM_HostelMaster h on h.collegecode =b.College_Code where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";
                string bcode = d2.GetFunction(" select HostelBuildingFK  from HM_HostelMaster where HostelMasterPK ='" + hostelcode + "'");
                string selectquery = " select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student,r.Building_Name,b.College_Code from Building_Master B,Room_Detail R where b.Building_Name =r.Building_Name and b.College_Code =r.College_Code and b.Building_Name in ('" + building + "')";//bcode//15.04.17
                if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Filled")
                {
                    selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
                }
                else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Un Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student = 0";
                }
                else if (ddl_pop3vaccant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
                {
                    selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
                }
                selectquery = selectquery + " Select Distinct ltrim(rtrim(f.floor_name))+'-'+ltrim(rtrim(room_type)) RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY ltrim(rtrim(f.floor_name))+'-'+ltrim(rtrim(room_type))";
                selectquery = selectquery + " select ISNULL(Room_Cost,0)as Room_Cost,Hostel_Code,Room_Type  from RoomCost_Master";
                ds.Clear();
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                int IntRoomLen = 0;
                int totalunfill = 0;
                int totalfill = 0;
                int totalpartialfill = 0;
                int totalvaccant = 0;
                string strRoomDetail = "";
                int colcnt = 0;
                FpSpread3.Sheets[0].ColumnCount = 0;
                if (ds.Tables[0].Rows.Count > 0)
                {
                    FpSpread3.CommandBar.Visible = false;
                    FpSpread3.Sheets[0].RowHeader.Visible = false;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            FpSpread3.Sheets[0].RowHeader.Visible = false;
                            FpSpread3.Sheets[0].RowCount = FpSpread3.Sheets[0].RowCount + 1;
                            colcnt = 0;
                            if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
                            {
                                FpSpread3.Sheets[0].ColumnCount++;
                            }
                            string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                            string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                            string alldetails = floorname + "-" + roomtype;
                            // string buildingname = Convert.ToString(ds.Tables[0].Rows[i]["Building_Name"]);
                            FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                            FpSpread3.Sheets[0].Columns[colcnt].CellType = textcel_type;
                            FpSpread3.Sheets[0].Cells[i, colcnt].Text = alldetails;
                            // 29.10.15
                            //  FpSpread3.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[i, 0].Font.Bold = true;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                            DataView dv = new DataView();
                            ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                            dv = ds.Tables[0].DefaultView;
                            if (dv.Count > 0)
                            {
                                //29.02.16
                                //FpSpread3.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
                                int columncount = dv.Count;
                                for (int cnt = 0; cnt < dv.Count; cnt++)
                                {
                                    colcnt++;
                                    FpSpread3.Sheets[0].Cells[i, cnt].Tag = Convert.ToString(dv[cnt]["Building_Name"]);
                                    string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]);// +Convert.ToString(dv[cnt]["Room_Cost"]);
                                    //24.02.16
                                    DataView cost = new DataView(); string rmcost = "";
                                    if (ds.Tables[2].Rows.Count > 0)
                                    {
                                        for (int rmc = 0; rmc < ds.Tables[2].Rows.Count; rmc++)
                                        {
                                            ds.Tables[2].DefaultView.RowFilter = " Hostel_Code='" + hostelcode + "' and Room_Type='" + roomtype + "'";
                                            cost = ds.Tables[2].DefaultView;
                                            if (cost.Count > 0)
                                            {
                                                rmcost = Convert.ToString(cost[rmc]["Room_Cost"]);
                                            }
                                        }
                                    }
                                    if (rmcost.Trim() == "")
                                    {
                                        rmcost = "0";
                                    }
                                    s = s + rmcost;
                                    if (FpSpread3.Sheets[0].ColumnCount - 1 < colcnt)
                                    {
                                        FpSpread3.Sheets[0].ColumnCount = FpSpread3.Sheets[0].ColumnCount + 1;
                                        FpSpread3.Sheets[0].Columns[0].Locked = true;
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                        FpSpread3.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpSpread3.Sheets[0].ColumnCount - 1);
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpSpread3.Sheets[0].ColumnHeader.Cells[0, FpSpread3.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                    }
                                    if (chck1.Checked == true)
                                    {
                                        FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                        FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                        if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                            totalfill = totalfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                            totalpartialfill = totalpartialfill + 1;
                                        }
                                        else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                        {
                                            FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                            totalunfill = totalunfill + 1;
                                        }
                                        //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + rmcost.Length;
                                        totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                    }
                                    else
                                    {
                                        try
                                        {
                                            if (chck1.Checked == false)
                                            {
                                                if (roomchecklist.Items[0].Selected == false && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                                {
                                                    FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                        totalfill = totalfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                        totalpartialfill = totalpartialfill + 1;
                                                    }
                                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                    {
                                                        FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                        totalunfill = totalunfill + 1;
                                                    }
                                                    strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);
                                                    if (IntRoomLen < strRoomDetail.Length)
                                                    {
                                                        IntRoomLen = strRoomDetail.Length;
                                                    }
                                                    FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                    //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                                }
                                            }
                                            if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[2].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + rmcost;// (dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[1].Selected == true && roomchecklist.Items[2].Selected == false && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == false && roomchecklist.Items[0].Selected == false)
                                            {
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            else if (roomchecklist.Items[0].Selected == true && roomchecklist.Items[2].Selected == true && roomchecklist.Items[1].Selected == true)
                                            {
                                                chck1.Checked = true;
                                                FpSpread3.Sheets[0].Columns[colcnt].Locked = true;
                                                if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                                {
                                                    IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                                }
                                                FpSpread3.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + rmcost;//(dv[cnt]["Room_Cost"]);
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread3.Sheets[0].Cells[FpSpread3.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                //IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(rmcost).Length;
                                            }
                                            totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    for (int j = 1; j < FpSpread3.Sheets[0].ColumnCount; j++)
                                    {
                                        totalvaccants.Text = " ";
                                        toalrooms.Text = " ";
                                        int totalroom = totalunfill + totalfill + totalpartialfill;
                                        toalrooms.Text = "Total No.of Rooms :" + totalroom;
                                        totalvaccants.Text = "Total No.of Vacant :" + totalvaccant;
                                        fill.Text = ("Filled(" + totalfill + ")");
                                        unfill.Text = ("UnFilled(" + totalunfill + ")");
                                        partialfill.Text = ("Partially Filled(" + totalpartialfill + ")");
                                    }
                                }
                                FpSpread3.SaveChanges();
                                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].ColumnCount;
                                FpSpread3.Sheets[0].FrozenColumnCount = 1;
                                FpSpread3.Sheets[0].PageSize = FpSpread3.Sheets[0].RowCount;
                            }
                        }
                        FpSpread3.Visible = true;
                        tblStatus.Visible = true;
                        lblpop3err.Visible = false;
                        lblpop3err.Text = "No Records Found";
                        btn_pop3save.Visible = true;
                        btn_pop3exit.Visible = true;
                    }
                }
                else
                {
                    FpSpread3.Visible = false;
                    tblStatus.Visible = false;
                    lblpop3err.Visible = true;
                    lblpop3err.Text = "No Records Found";
                    btn_pop3save.Visible = false;
                    btn_pop3exit.Visible = false;
                }
            }
            else
            {
                tblStatus.Visible = false;
                FpSpread3.Visible = false;
                lblpop3err.Visible = true;
                lblpop3err.Text = "No Records Found";
                btn_pop3save.Visible = false;
                btn_pop3exit.Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblpop3err.Visible = true;
            lblpop3err.Text = ex.ToString();
        }
    }
    public void btn_gopop3_Click(object sender, EventArgs e)
    {
        try
        {
            btn_pop3save.Visible = true;
            btn_pop3exit.Visible = true;
            search();
        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
    }
    protected void btn_sureyes_Click(object sender, EventArgs e)
    {
        delete();
    }
    protected void btn_sureno_Click(object sender, EventArgs e)
    {
        surediv.Visible = false;
        imgdiv2.Visible = false;
        popwindow1.Visible = true;
    }
    //12.10.15
    protected void ddl_pop1hostelname_SelectedIndexchange(object sender, EventArgs e)
    {
        hosname = ddl_pop1hostelname.SelectedItem.Value;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getroll1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> roll = new List<string>();
        query = "select distinct top 10 Roll_No from Registration r,applyn a, Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag!='debar'and r.degree_code = g.Degree_Code  and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and  g.Dept_Code = d.Dept_Code and g.college_code = d.college_code  and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'') and  r.roll_no like '" + prefixText + "%' order by roll_no";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                roll.Add(dw.Tables[0].Rows[i]["roll_no"].ToString());
            }
        }
        return roll;
    }
    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> Getname1(string prefixText)
    {
        DAccess2 dn = new DAccess2();
        DataSet dw = new DataSet();
        List<string> name = new List<string>();
        query = "select r.stud_name from Registration r,applyn a,Degree g,course c,Department d where cc=0 and delflag=0 and exam_flag!='debar'and r.degree_code = g.Degree_Code and r.App_No = a.app_no and g.Course_Id = c.Course_Id and g.college_code = c.college_code and g.Dept_Code = d.Dept_Code and a.sex in (0,1) and g.college_code = d.college_code and  r.App_No not in (select App_No from HT_HostelRegistration) and r.Stud_Name like '" + prefixText + "%'";
        dw = dn.select_method_wo_parameter(query, "Text");
        if (dw.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < dw.Tables[0].Rows.Count; i++)
            {
                name.Add(dw.Tables[0].Rows[i]["stud_name"].ToString());
            }
        }
        return name;
    }
    protected void roll_txtchange(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string sex = d2.GetFunction(" select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            string q1 = "select Textval,Student_Mobile from applyn a, Registration b,textvaltable c where a.app_no =b.App_No  and c.TextCode=a.community and b.Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + " select parent_addressP,Streetp,Cityp+'-'+Districtp city,parent_pincodep,b.Stud_Name from applyn a, Registration b where a.app_no =b.App_No and  Roll_No ='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "  select dob,b.college_code from applyn a,Registration b where a.app_no=b.App_No and b.Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "  select textval from applyn a,textvaltable t,Registration b where a.bldgrp = t.TextCode and a.app_no =b.App_No and  Roll_No='" + txt_pop1rollno.Text + "'";
            q1 = q1 + "    select distinct c.Course_Name+' - '+de.dept_name branch from degree d,department de,course c,Registration r,applyn a where r.App_No=a.app_no and r.degree_code=d.Degree_Code  and c.course_id=d.course_id  and de.dept_code=d.dept_code and c.college_code = d.college_code and de.college_code = d.college_code and r.App_No not in (select APP_No from HT_HostelRegistration where (isnull(isvacated,0)=0 and isnull(isdiscontinued,0)=0)) and a.sex in(" + sex + ") and r.Roll_No='" + txt_pop1rollno.Text + "'";
            string appno = " ";
            appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            ViewState["id"] = txtid.Text;
            ds = d2.select_method_wo_parameter(q1, "Text");
            if (ds.Tables[4].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string comm = ds.Tables[0].Rows[0]["Textval"].ToString();
                    txt_pop1community.Text = comm;
                    string mobile = ds.Tables[0].Rows[0]["Student_Mobile"].ToString();
                    txt_pop1mob.Text = mobile;
                }
                else
                {
                    txt_pop1community.Text = "";
                    txt_pop1mob.Text = "";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string add = ds.Tables[1].Rows[0]["parent_addressP"].ToString();
                    txt_pop1address.Text = add;
                    string add2 = ds.Tables[1].Rows[0]["Streetp"].ToString();
                    txt_pop1address1.Text = add2;
                    string add3 = ds.Tables[1].Rows[0]["city"].ToString();
                    txt_pop1address2.Text = add3;
                    string add5 = ds.Tables[1].Rows[0]["parent_pincodep"].ToString();
                    txt_pop1pin.Text = add5;
                    string add6 = ds.Tables[1].Rows[0]["Stud_Name"].ToString();
                    txt_pop1name.Text = add6;
                }
                else
                {
                    txt_pop1rollno.Text = "";
                    txt_pop1address.Text = "";
                    txt_pop1address1.Text = "";
                    txt_pop1address2.Text = "";
                    txt_pop1pin.Text = "";
                    txt_pop1name.Text = "";
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    string dob = ds.Tables[2].Rows[0]["dob"].ToString();
                    string collegecode = ds.Tables[2].Rows[0]["college_code"].ToString();
                    ddl_pop1collegename.SelectedIndex = ddl_pop1collegename.Items.IndexOf(ddl_pop1collegename.Items.FindByValue(collegecode));
                    if (dob.Trim() != "")
                    {
                        datesp = dob.Split(' ');
                        dob = datesp[0].ToString();
                        string[] db = dob.Split('/');
                        dob = db[1].ToString() + "/" + db[0].ToString() + "/" + db[2].ToString();
                    }
                    txt_date.Text = Convert.ToString(dob);
                }
                else
                {
                    txt_date.Text = "";
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    string blood = ds.Tables[3].Rows[0]["textval"].ToString();
                    txt_pop1blood.Text = blood;
                }
                else
                {
                    txt_pop1blood.Text = "";
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    string degree = ds.Tables[4].Rows[0]["branch"].ToString();
                    txt_pop1degree.Text = degree;
                }
                else
                {
                    txt_pop1degree.Text = "";
                }
            }
            else
            {
                txt_pop1rollno.Text = "";
                txt_pop1address.Text = "";
                txt_pop1address1.Text = "";
                txt_pop1address2.Text = "";
                txt_pop1pin.Text = "";
                txt_pop1name.Text = "";
                txt_pop1degree.Text = "";
                txt_pop1blood.Text = "";
                txt_date.Text = "";
                txt_pop1community.Text = "";
                txt_pop1mob.Text = "";
            }
        }
        catch { }
    }
    protected void stud_name_txtchanged(object sender, EventArgs e)
    {
        try
        {
            //txt_pop1rollno.Text = "";
            ds.Clear();
            string sex = d2.GetFunction(" select case when HostelType=1 then '0' when HostelType=2 then '1' when HostelType=0 then '0,1' end HostelType  from HM_HostelMaster where HostelMasterPK in ('" + ddl_pop1hostelname.SelectedItem.Value + "')");
            string q1 = "select Textval,Student_Mobile from applyn a, Registration b,textvaltable c where a.app_no =b.App_No  and c.TextCode=a.community and b.Stud_Name='" + txt_pop1name.Text + "'";
            q1 = q1 + " select parent_addressP,Streetp,Cityp+'-'+Districtp city,parent_pincodep,b.Roll_no from applyn a, Registration b where a.app_no =b.App_No and  b.Stud_Name ='" + txt_pop1name.Text + "'";
            q1 = q1 + "  select dob from applyn a,Registration b where a.app_no=b.App_No and b.Stud_Name='" + txt_pop1name.Text + "'";
            q1 = q1 + "  select textval from applyn a,textvaltable t,Registration b where a.bldgrp = t.TextCode and a.app_no =b.App_No and  b.Stud_Name='" + txt_pop1name.Text + "'";
            q1 = q1 + "  select distinct c.Course_Name+' - '+de.dept_name branch from degree d,department de,course c,Registration r,applyn a where r.App_No=a.app_no and r.degree_code=d.Degree_Code  and c.course_id=d.course_id  and de.dept_code=d.dept_code and c.college_code = d.college_code and de.college_code = d.college_code and r.App_No not in (select App_No from HT_HostelRegistration where ISNULL(App_No,'')<>'') and a.sex in(" + sex + ")  and r.Stud_Name='" + txt_pop1name.Text + "'";
            ds = d2.select_method_wo_parameter(q1, "Text");
            string appno = " ";
            appno = d2.GetFunction("select APP_No from Registration sm where sm.Stud_Name='" + txt_pop1name.Text + "'");
            ViewState["App_No"] = Convert.ToString(appno);
            ViewState["id"] = txtid.Text;
            if (ds.Tables[4].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string comm = ds.Tables[0].Rows[0]["Textval"].ToString();
                    txt_pop1community.Text = comm;
                    string mobile = ds.Tables[0].Rows[0]["Student_Mobile"].ToString();
                    txt_pop1mob.Text = mobile;
                }
                else
                {
                    txt_pop1community.Text = "";
                    txt_pop1mob.Text = "";
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    string add = ds.Tables[1].Rows[0]["parent_addressP"].ToString();
                    txt_pop1address.Text = add;
                    string add2 = ds.Tables[1].Rows[0]["Streetp"].ToString();
                    txt_pop1address1.Text = add2;
                    string add3 = ds.Tables[1].Rows[0]["city"].ToString();
                    txt_pop1address2.Text = add3;
                    string add5 = ds.Tables[1].Rows[0]["parent_pincodep"].ToString();
                    txt_pop1pin.Text = add5;
                    string add6 = ds.Tables[1].Rows[0]["Roll_no"].ToString();
                    txt_pop1rollno.Text = add6;
                }
                else
                {
                    txt_pop1address.Text = "";
                    txt_pop1address1.Text = "";
                    txt_pop1address2.Text = "";
                    txt_pop1pin.Text = "";
                    txt_pop1name.Text = "";
                    txt_pop1rollno.Text = "";
                }
                if (ds.Tables[2].Rows.Count > 0)
                {
                    string dob = ds.Tables[2].Rows[0]["dob"].ToString();
                    datesp = dob.Split(' ');
                    dob = datesp[0].ToString();
                    string[] db = dob.Split('/');
                    dob = db[1].ToString() + "/" + db[0].ToString() + "/" + db[2].ToString();
                    txt_date.Text = Convert.ToString(dob);
                }
                else
                {
                    txt_date.Text = "";
                }
                if (ds.Tables[3].Rows.Count > 0)
                {
                    string blood = ds.Tables[3].Rows[0]["textval"].ToString();
                    txt_pop1blood.Text = blood;
                }
                else
                {
                    txt_pop1blood.Text = "";
                }
                if (ds.Tables[4].Rows.Count > 0)
                {
                    string degree = ds.Tables[4].Rows[0]["branch"].ToString();
                    txt_pop1degree.Text = degree;
                }
                else
                {
                    txt_pop1degree.Text = "";
                }
            }
            else
            {
                txt_pop1rollno.Text = "";
                txt_pop1address.Text = "";
                txt_pop1address1.Text = "";
                txt_pop1address2.Text = "";
                txt_pop1pin.Text = "";
                txt_pop1name.Text = "";
                txt_pop1degree.Text = "";
                txt_pop1blood.Text = "";
                txt_date.Text = "";
                txt_pop1community.Text = "";
                txt_pop1mob.Text = "";
            }
        }
        catch { }
    }
    public void lnkbtn_transferhos_Click(object sender, EventArgs e)
    {
        try
        {
            txt_trhosdate.Attributes.Add("readonly", "readonly");
            txt_trhosdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            bindhosteltrhos();
            bindhosteltrhostel();
            txt_room.Text = "";
            txt_building.Text = "";
            txt_floor.Text = "";
            txt_roomtype.Text = "";
            txt_reason.Text = "";
            loadreason();
            poperrjs.Visible = true;
        }
        catch
        {
        }
    }
    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    public void btntransferhos_Click(object sender, EventArgs e)
    {
        bindbuildingtrhos();
        btn_trhos.Visible = false;
        btn_trhosexit.Visible = false;
        chtrhos.Checked = false;
        popwindow3trhos.Visible = true;
        ddl_pop3vaccant.SelectedIndex = 0;
        cbl_trhos.Items[0].Selected = false;
        cbl_trhos.Items[1].Selected = false;
        cbl_trhos.Items[2].Selected = false;
        FpSpread2.Visible = false;
        lblertrhos.Visible = false;
        tblStatusguest.Visible = false;
        toalroomsguest.Visible = false;
        totalvaccantsguest.Visible = false;
        fillguest.Visible = false;
        partialfillguest.Visible = false;
        unfillguest.Visible = false;
    }
    public void bindbuildingtrhos()
    {
        try
        {
            cbl_build.Items.Clear();
            string bul = "";
            bul = d2.GetBuildingCode_inv(ddl_tohostel.SelectedItem.Value);
            ds = d2.BindBuilding(bul);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_build.DataSource = ds;
                cbl_build.DataTextField = "Building_Name";
                cbl_build.DataValueField = "code";
                cbl_build.DataBind();
            }
            else
            {
                txt_build.Text = "--Select--";
            }
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                cbl_build.Items[i].Selected = true;
                txt_build.Text = "Building(" + (cbl_build.Items.Count) + ")";
                cb_build.Checked = true;
            }
            string locbuild = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    string builname = cbl_build.Items[i].Text;
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
            bindroompopfloorguest(locbuild);
        }
        catch
        {
        }
    }
    public void cb_build_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_build.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                for (int i = 0; i < cbl_build.Items.Count; i++)
                {
                    if (cb_build.Checked == true)
                    {
                        cbl_build.Items[i].Selected = true;
                        txt_build.Text = "Build(" + (cbl_build.Items.Count) + ")";
                        txt_floorguest.Text = "--Select--";
                        txt_roomtypeguest.Text = "--Select--";
                        build1 = cbl_build.Items[i].Text.ToString();
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
                bindroompopfloorguest(buildvalue1);
            }
            else
            {
                for (int i = 0; i < cbl_build.Items.Count; i++)
                {
                    cbl_build.Items[i].Selected = false;
                    txt_build.Text = "--Select--";
                    cbl_floor.ClearSelection();
                    cbl_roomtype.ClearSelection();
                    cb_floor.Checked = false;
                    cb_roomtype.Checked = false;
                    txt_floorguest.Text = "--Select--";
                    txt_roomtypeguest.Text = "--Select--";
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_build_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_build.Checked = false;
            string buildvalue = "";
            string build = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_floorguest.Text = "--Select--";
                    txt_roomtypeguest.Text = "--Select--";
                    cb_floor.Checked = false;
                    cb_roomtype.Checked = false;
                    build = cbl_build.Items[i].Text.ToString();
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
            bindroompopfloorguest(buildvalue);
            if (seatcount == cbl_build.Items.Count)
            {
                txt_build.Text = "Build(" + seatcount + ")";
                cb_build.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_build.Text = "--Select--";
            }
            else
            {
                txt_build.Text = "Build(" + seatcount + ")";
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void bindroompopfloorguest(string buildname)
    {
        try
        {
            //chklstfloorpo3.Items.Clear();
            cbl_floor.Items.Clear();
            ds = d2.BindFloor(buildname);
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_floor.DataSource = ds;
                cbl_floor.DataTextField = "Floor_Name";
                cbl_floor.DataValueField = "Floorpk";
                cbl_floor.DataBind();
            }
            else
            {
                txt_floorguest.Text = "--Select--";
            }
            //for selected floor
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                cbl_floor.Items[i].Selected = true;
                cb_floor.Checked = true;
            }
            string locfloor = "";
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                    string flrname = cbl_floor.Items[i].Text; //cbl_floorname.SelectedItem.Text; 
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
            clgroomtypeguest(locfloor, buildname);
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_floor_checkedchange(object sender, EventArgs e)
    {
        try
        {
            txt_roomtypeguest.Text = "--Select--";
            if (cb_floor.Checked == true)
            {
                string buildvalue1 = "";
                string build1 = "";
                string build2 = "";
                string buildvalue2 = "";
                if (cb_build.Checked == true)
                {
                    for (int i = 0; i < cbl_build.Items.Count; i++)
                    {
                        build1 = cbl_build.Items[i].Text.ToString();
                        if (buildvalue1 == "")
                        {
                            buildvalue1 = build1;
                        }
                        else
                        {
                            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                        }
                    }
                    if (cb_floor.Checked == true)
                    {
                        for (int j = 0; j < cbl_floor.Items.Count; j++)
                        {
                            cbl_floor.Items[j].Selected = true;
                            txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                            txt_roomtypeguest.Text = "--Select--";
                            build2 = cbl_floor.Items[j].Text.ToString();
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
                }
                if (cb_build.Checked == false)
                {
                    for (int i = 0; i < cbl_build.Items.Count; i++)
                    {
                        build1 = cbl_build.Items[i].Text.ToString();
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
                if (cb_floor.Checked == true)
                {
                    for (int j = 0; j < cbl_floor.Items.Count; j++)
                    {
                        cbl_floor.Items[j].Selected = true;
                        txt_floorguest.Text = "Floor(" + (cbl_floor.Items.Count) + ")";
                        txt_roomtypeguest.Text = "--Select--";
                        build2 = cbl_floor.Items[j].Text.ToString();
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
                clgroomtypeguest(buildvalue1, buildvalue2);
            }
            else
            {
                for (int i = 0; i < cbl_floor.Items.Count; i++)
                {
                    cbl_floor.Items[i].Selected = false;
                    txt_floorguest.Text = "--Select--";
                    cbl_roomtype.ClearSelection();
                    cb_roomtype.Checked = false;
                }
            }
            //  Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_floor_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_floor.Checked = false;
            string buildvalue1 = "";
            string build1 = "";
            string build2 = "";
            string buildvalue2 = "";
            for (int i = 0; i < cbl_build.Items.Count; i++)
            {
                if (cbl_build.Items[i].Selected == true)
                {
                    build1 = cbl_build.Items[i].Text.ToString();
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
            for (int i = 0; i < cbl_floor.Items.Count; i++)
            {
                if (cbl_floor.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_roomtypeguest.Text = "--Select--";
                    build2 = cbl_floor.Items[i].Text.ToString();
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
            clgroomtypeguest(buildvalue1, buildvalue2);
            if (seatcount == cbl_floor.Items.Count)
            {
                txt_floorguest.Text = "Floor(" + seatcount.ToString() + ")";
                cb_floor.Checked = true;
            }
            else if (seatcount == 0)
            {
                txt_floorguest.Text = "--Select--";
            }
            else
            {
                txt_floorguest.Text = "Floor(" + seatcount.ToString() + ")";
            }
            //   Button2.Focus();
        }
        catch (Exception ex)
        {
        }
    }
    public void clgroomtypeguest(string floor, string room)
    {
        try
        {
            if (floor != "" && room != "")
            {
                ds.Clear();
                ds = d2.BindRoomtype(floor, room);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_roomtype.DataSource = ds;
                    cbl_roomtype.DataTextField = "Room_Type";
                    cbl_roomtype.DataValueField = "Room_Type";
                    cbl_roomtype.DataBind();
                }
                else
                {
                    txt_roomtypeguest.Text = "--Select--";
                    cbl_roomtype.ClearSelection();
                }
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = true;
                    txt_roomtypeguest.Text = "Room(" + (cbl_roomtype.Items.Count) + ")";
                    cb_roomtype.Checked = true;
                }
            }
            else
            {
                txt_roomtypeguest.Text = "--Select--";
                cbl_roomtype.ClearSelection();
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cb_roomtype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_roomtype.Checked == true)
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = true;
                }
                txt_roomtypeguest.Text = "Room(" + (cbl_roomtype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_roomtype.Items.Count; i++)
                {
                    cbl_roomtype.Items[i].Selected = false;
                }
                txt_roomtypeguest.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_roomtype_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            int seatcount = 0;
            cb_roomtype.Checked = false;
            for (int i = 0; i < cbl_roomtype.Items.Count; i++)
            {
                if (cbl_roomtype.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                }
            }
            if (seatcount == cbl_roomtype.Items.Count)
            {
                txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
                cb_roomtype.Checked = true;
            }
            else if (seatcount == 0)
            {
                cbl_roomtype.Text = "--Select--";
            }
            else
            {
                txt_roomtypeguest.Text = "Room(" + seatcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void chtrhos_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chtrhos.Checked == true)
            {
                for (int i = 0; i < cbl_trhos.Items.Count; i++)
                {
                    cbl_trhos.Items[i].Selected = true;
                }
            }
            else
            {
                for (int i = 0; i < cbl_trhos.Items.Count; i++)
                {
                    cbl_trhos.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_trhos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (cbl_trhos.Items[0].Selected == false)
            {
                chtrhos.Checked = false;
            }
            if (cbl_trhos.Items[1].Selected == false)
            {
                chtrhos.Checked = false;
            }
            if (cbl_trhos.Items[2].Selected == false)
            {
                chtrhos.Checked = false;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_trhosgo_Click(object sender, EventArgs e)
    {
        try
        {
            btn_trhos.Visible = true;
            btn_trhosexit.Visible = true;
            //Button4.Visible = false;
            //Button5.Visible = false;
            //Button6.Visible = false;
            searchguest();
        }
        catch { }
    }
    public void btn_trhos_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string activecol = "";
            activerow = FpSpread2.ActiveSheetView.ActiveRow.ToString();
            activecol = FpSpread2.ActiveSheetView.ActiveColumn.ToString();
            if (Convert.ToInt32(activerow) != -1 && Convert.ToInt32(activecol) != -1)
            {
                if (Convert.ToInt32(activecol) != 0)
                {
                    //29.10.15
                    string purpose = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].Text;
                    string room1 = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                    if (purpose.Trim() != "")
                    {
                        address = purpose.Split('-');
                        purpose = address[0].ToString();
                        fr = room1.Split('-');
                        string rname = fr[0].ToString();
                        string rtype = "";
                        if (fr.Length == 3)
                        {
                            rtype = fr[1].ToString() + "-" + fr[2].ToString();
                        }
                        else
                        {
                            rtype = fr[1].ToString();
                        }
                        string build_name = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(0)].Tag);
                        string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + rtype + "' and Floor_Name='" + fr[0].ToString() + "' and Room_Name='" + purpose + "' and Building_Name='" + build_name + "'";
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(q, "text");
                        string comp1 = Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString());
                        string comp2 = Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"].ToString());
                        if (Convert.ToInt32(comp1) >= Convert.ToInt32(comp2) && Convert.ToInt32(comp1) != Convert.ToInt32(comp2))
                        {
                            if (FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor != Color.GreenYellow)
                            {
                                txt_room.Text = purpose;
                                string build = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Tag);
                                txt_building.Text = build;
                                string buil = "";
                                buil = d2.GetFunction("select Code  from Building_Master sm where  sm.Building_Name='" + build + "'");
                                ViewState["Code"] = Convert.ToString(buil);
                                string floorroom = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                fr = floorroom.Split('-');
                                txt_floor.Text = fr[0].ToString();
                                if (txt_pop1floor.Text == "")
                                {
                                    txt_floor.Text = fr[1].ToString();
                                }
                                string fl = "";
                                fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + build + "'");
                                ViewState["Floorpk"] = Convert.ToString(fl);
                                string room = "select Room_Name,Room_type from Room_Detail where Room_Name='" + txt_room.Text + "'";
                                ds = d2.select_method_wo_parameter(room, "Text");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string roomtype = ds.Tables[0].Rows[0]["Room_type"].ToString();
                                    txt_roomtype.Text = roomtype;
                                    popwindow3trhos.Visible = false;
                                }
                                string roomname = "";
                                string ro = "";
                                roomname = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                                ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + build + "'");
                                ViewState["Roompk"] = Convert.ToString(ro);
                                if (txt_roomtypeguest.Text == "")
                                {
                                    lblertrhos.Visible = true;
                                    lblertrhos.Text = "Please select  room";
                                    txt_floor.Text = "";
                                    txt_building.Text = "";
                                    txt_roomtypeguest.Text = "";
                                }
                            }
                            else if (FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), Convert.ToInt32(activecol)].BackColor == Color.GreenYellow)
                            {
                                txt_room.Text = purpose;
                                string floorroom = FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 0].Text;
                                fr = floorroom.Split('-');
                                txt_floor.Text = fr[0].ToString();
                                txt_roomtype.Text = fr[1].ToString();
                                string fl = "";
                                fl = d2.GetFunction("select Floorpk  from Floor_Master sm where  sm.Floor_Name='" + fr[0] + "'and Building_Name ='" + build + "'");
                                ViewState["Floorpk"] = Convert.ToString(fl);
                                string roomname = "";
                                string ro = "";
                                roomname = Convert.ToString(FpSpread2.Sheets[0].Cells[Convert.ToInt32(activerow), 1].Text);
                                ro = d2.GetFunction("select Roompk  from Room_Detail sm where  sm.Room_Name='" + purpose + "' and Building_Name ='" + build + "'");
                                ViewState["Roompk"] = Convert.ToString(ro);
                                string NON = "NON";
                                if (txt_roomtype.Text == NON)
                                {
                                    txt_roomtype.Text = txt_roomtype.Text + "-AC";
                                    popwindow3trhos.Visible = false;
                                }
                                if (txt_room.Text == "")
                                {
                                    lblertrhos.Visible = true;
                                    lblertrhos.Text = "Please select  room";
                                    txt_floor.Text = "";
                                    txt_roomtype.Text = "";
                                    txt_building.Text = "";
                                }
                            }
                            else
                            {
                                lblertrhos.Visible = true;
                                lblertrhos.Text = "Please select unfilled room";
                            }
                        }
                        else
                        {
                            lblertrhos.Visible = true;
                            lblertrhos.Text = " Room fill please select other room";
                        }
                    }
                    else
                    {
                        lblertrhos.Visible = true;
                        lblertrhos.Text = "Please select correct room";
                    }
                }
                else
                {
                    lblertrhos.Visible = true;
                    lblertrhos.Text = "Please select correct room";
                }
            }
            else
            {
                lblertrhos.Visible = true;
                lblertrhos.Text = "Please select room";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void btn_trhosexit_Click(object sender, EventArgs e)
    {
        popwindow3trhos.Visible = false;
        //cbl_buildname.Items.Clear();
        cbl_floor.Items.Clear();
        cbl_roomtype.Items.Clear();
        FpSpread2.Visible = false;
    }
    public void searchguest()
    {
        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        darkstyle.ForeColor = Color.Black;
        FpSpread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
        toalroomsguest.Visible = true;
        totalvaccantsguest.Visible = true;
        fillguest.Visible = true;
        partialfillguest.Visible = true;
        unfillguest.Visible = true;
        btn_trhos.Visible = true;
        btn_trhosexit.Visible = true;
        FpSpread2.Sheets[0].AutoPostBack = false;
        FpSpread2.Sheets[0].ColumnCount = 0;
        FpSpread2.Sheets[0].RowCount = 0;
        string hostelcode = Convert.ToString(ddl_tohostel.SelectedValue);
        string building = "";
        // change
        // building = Convert.ToString(cbl_build.SelectedItem.Text);
        for (int i = 0; i < cbl_build.Items.Count; i++)
        {
            if (cbl_build.Items[i].Selected == true)
            {
                if (building == "")
                {
                    building = "" + cbl_build.Items[i].Text.ToString() + "";
                }
                else
                {
                    building = building + "'" + "," + "'" + cbl_build.Items[i].Text.ToString() + "";
                }
            }
        }
        string vaccanttype = Convert.ToString(ddl_vacant.SelectedItem.Text);
        string floor = "";
        for (int i = 0; i < cbl_floor.Items.Count; i++)
        {
            if (cbl_floor.Items[i].Selected == true)
            {
                if (floor == "")
                {
                    floor = "" + cbl_floor.Items[i].Text.ToString() + "";
                }
                else
                {
                    floor = floor + "'" + "," + "'" + cbl_floor.Items[i].Text.ToString() + "";
                }
            }
        }
        string roomtype0 = "";
        for (int i = 0; i < cbl_roomtype.Items.Count; i++)
        {
            if (cbl_roomtype.Items[i].Selected == true)
            {
                if (roomtype0 == "")
                {
                    roomtype0 = "" + cbl_roomtype.Items[i].Value.ToString() + "";
                }
                else
                {
                    roomtype0 = roomtype0 + "'" + "," + "'" + cbl_roomtype.Items[i].Value.ToString() + "";
                }
            }
        }
        if (floor.Trim() != "" && roomtype0.Trim() != "")
        {
            string selectquery = "Select r.Room_type,r.Floor_Name, Room_Name,ISNULL(Students_Allowed,0) Students_Allowed,ISNULL(Avl_Student,0) Avl_Student, ISNULL(m.Room_Cost,0) Room_Cost,h.HostelMasterPK,r.Building_Name FROM Room_Detail R left join RoomCost_Master m on r.Room_type = m.Room_Type  left join Building_Master b on b.Building_Name =r.Building_Name,HM_HostelMaster h   where R.Building_Name in ('" + building + "') and r.Room_Type in ('" + roomtype0 + "') and Floor_Name in ('" + floor + "') and h.HostelMasterPK ='" + hostelcode + "'";//h.collegecode =b.College_Code 
            if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Filled")
            {
                selectquery = selectquery + " AND R.Students_Allowed =  R.Avl_Student AND R.Avl_Student != 0";
            }
            else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Un Filled")
            {
                selectquery = selectquery + " AND R.Avl_Student = 0";
            }
            else if (ddl_vacant.SelectedItem.Text.Trim().ToString() == "Partially Filled")
            {
                selectquery = selectquery + " AND R.Avl_Student != 0 And (R.Students_Allowed != R.Avl_Student)";
            }
            selectquery = selectquery + " Select Distinct F.Floor_Name+' - '+Room_Type RoomType,r.Room_type RT,f.Floor_Name FN  FROM Floor_Master F INNER JOIN Room_Detail R ON R.Floor_Name = F.Floor_Name INNER JOIN Building_Master B ON   B.Building_Name = F.Building_Name WHERE R.Building_Name in ('" + building + "') AND R.Floor_Name in ('" + floor + "') AND R.Room_Type in ('" + roomtype0 + "') ORDER BY F.Floor_Name+' - '+Room_Type";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selectquery, "Text");
            int IntRoomLen = 0;
            int totalunfill = 0;
            int totalfill = 0;
            int totalpartialfill = 0;
            int totalvaccant = 0;
            string strRoomDetail = "";
            int colcnt = 0;
            FpSpread2.Sheets[0].ColumnCount = 0;
            if (ds.Tables[0].Rows.Count > 0)
            {
                FpSpread2.CommandBar.Visible = false;
                FpSpread2.Sheets[0].RowHeader.Visible = false;
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        FpSpread2.Sheets[0].RowHeader.Visible = false;
                        FpSpread2.Sheets[0].RowCount = FpSpread2.Sheets[0].RowCount + 1;
                        colcnt = 0;
                        if (FpSpread2.Sheets[0].ColumnCount - 1 < colcnt)
                        {
                            FpSpread2.Sheets[0].ColumnCount++;
                        }
                        string floorname = Convert.ToString(ds.Tables[1].Rows[i]["FN"]);
                        string roomtype = Convert.ToString(ds.Tables[1].Rows[i]["RT"]);
                        string alldetails = floorname + "-" + roomtype;
                        FarPoint.Web.Spread.TextCellType textcel_type = new FarPoint.Web.Spread.TextCellType();
                        FpSpread2.Sheets[0].Columns[colcnt].CellType = textcel_type;
                        FpSpread2.Sheets[0].Cells[i, colcnt].Text = alldetails;
                        // 29.10.15
                        // FpSpread2.Sheets[0].Cells[i, colcnt].Tag = ds.Tables[0].Rows[colcnt]["Building_Name"].ToString();
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Floor/RoomType";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].Cells[i, 0].Font.Bold = true;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.LightSteelBlue;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].Font.Size = FontUnit.Medium;
                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].Font.Name = "Book Antiqua";
                        DataView dv = new DataView();
                        ds.Tables[0].DefaultView.RowFilter = "floor_name='" + floorname + "' and room_type='" + roomtype + "' ";
                        dv = ds.Tables[0].DefaultView;
                        if (dv.Count > 0)
                        {
                            int columncount = dv.Count;
                            FpSpread2.Sheets[0].Cells[i, colcnt].Tag = Convert.ToString(dv[0]["Building_Name"]);
                            for (int cnt = 0; cnt < dv.Count; cnt++)
                            {
                                colcnt++;
                                //// 29.10.15 Building_Name
                                string s = Convert.ToString(dv[cnt]["room_name"]) + Convert.ToString(dv[cnt]["Students_Allowed"]) + Convert.ToString(dv[cnt]["Avl_Student"]) + Convert.ToString(dv[cnt]["Room_Cost"]);
                                if (FpSpread2.Sheets[0].ColumnCount - 1 < colcnt)
                                {
                                    FpSpread2.Sheets[0].ColumnCount = FpSpread2.Sheets[0].ColumnCount + 1;
                                    FpSpread2.Sheets[0].Columns[0].Locked = true;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Text = "Room Details";
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Name = "Book Antiqua";
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                                    FpSpread2.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, FpSpread2.Sheets[0].ColumnCount - 1);
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
                                    FpSpread2.Sheets[0].ColumnHeader.Cells[0, FpSpread2.Sheets[0].ColumnCount - 1].Font.Bold = true;
                                }
                                if (chtrhos.Checked == true)
                                {
                                    FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
                                    FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                    if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                        totalunfill = totalunfill + 1;
                                    }
                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                        totalpartialfill = totalpartialfill + 1;
                                    }
                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                        totalfill = totalfill + 1;
                                    }
                                    else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                        totalpartialfill = totalpartialfill + 1;
                                    }
                                    else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                    {
                                        FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                        totalunfill = totalunfill + 1;
                                    }
                                    IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                    totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                }
                                else
                                {
                                    try
                                    {
                                        if (chtrhos.Checked == false)
                                        {
                                            if (cbl_trhos.Items[0].Selected == false && cbl_trhos.Items[1].Selected == false && cbl_trhos.Items[2].Selected == false)
                                            {
                                                FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                                if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                                {
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                                {
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                    totalfill = totalfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                                {
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                    totalpartialfill = totalpartialfill + 1;
                                                }
                                                else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0)
                                                {
                                                    FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                    totalunfill = totalunfill + 1;
                                                }
                                                strRoomDetail = strRoomDetail + (dv[cnt]["Room_Name"]);
                                                if (IntRoomLen < strRoomDetail.Length)
                                                {
                                                    IntRoomLen = strRoomDetail.Length;
                                                }
                                                FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "";
                                                IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                            }
                                        }
                                        if (cbl_trhos.Items[0].Selected == true && cbl_trhos.Items[1].Selected == false && cbl_trhos.Items[2].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[0].Selected == true && cbl_trhos.Items[1].Selected == true && cbl_trhos.Items[2].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[1].Selected == true && cbl_trhos.Items[2].Selected == true && cbl_trhos.Items[0].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[0].Selected == true && cbl_trhos.Items[2].Selected == true && cbl_trhos.Items[1].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Room_Cost"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[1].Selected == true && cbl_trhos.Items[2].Selected == false && cbl_trhos.Items[0].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Avl_Student"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[2].Selected == true && cbl_trhos.Items[1].Selected == false && cbl_trhos.Items[0].Selected == false)
                                        {
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Room_Cost"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        else if (cbl_trhos.Items[0].Selected == true && cbl_trhos.Items[2].Selected == true && cbl_trhos.Items[1].Selected == true)
                                        {
                                            chtrhos.Checked = true;
                                            FpSpread2.Sheets[0].Columns[colcnt].Locked = true;
                                            if (IntRoomLen < Convert.ToString(dv[cnt]["Room_Name"]).Length)
                                            {
                                                IntRoomLen = Convert.ToString(dv[cnt]["Room_Name"]).Length + 2;
                                            }
                                            FpSpread2.Sheets[0].Cells[i, colcnt].Text = (dv[cnt]["Room_Name"]) + "-" + (dv[cnt]["Students_Allowed"]) + "-" + (dv[cnt]["Avl_Student"]) + "-" + (dv[cnt]["Room_Cost"]);
                                            if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) == Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.GreenYellow;
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, 1].Note = "filled";
                                                totalfill = totalfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Avl_Student"]) != 0 && Convert.ToInt16(dv[cnt]["Students_Allowed"]) != Convert.ToInt16(dv[cnt]["Avl_Student"]))
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.Coral;
                                                totalpartialfill = totalpartialfill + 1;
                                            }
                                            else if (Convert.ToInt16(dv[cnt]["Students_Allowed"]) != 0 && Convert.ToInt16(dv[cnt]["Avl_Student"]) == 0)
                                            {
                                                FpSpread2.Sheets[0].Cells[FpSpread2.Sheets[0].RowCount - 1, colcnt].BackColor = Color.MistyRose;
                                                totalunfill = totalunfill + 1;
                                            }
                                            IntRoomLen = Convert.ToString(dv[cnt]["room_name"]).Length + Convert.ToString(dv[cnt]["Students_Allowed"]).Length + Convert.ToString(dv[cnt]["Avl_Student"]).Length + Convert.ToString(dv[cnt]["Room_Cost"]).Length;
                                        }
                                        totalvaccant = totalvaccant + Convert.ToInt16(dv[cnt]["students_allowed"]) - Convert.ToInt16(dv[cnt]["avl_student"]);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                for (int j = 1; j < FpSpread2.Sheets[0].ColumnCount; j++)
                                {
                                    totalvaccantsguest.Text = " ";
                                    toalroomsguest.Text = " ";
                                    int totalroom = totalunfill + totalfill + totalpartialfill;
                                    toalroomsguest.Text = "Total No.of Rooms :" + totalroom;
                                    totalvaccantsguest.Text = "Total No.of Vacant :" + totalvaccant;
                                    fillguest.Text = ("Filled(" + totalfill + ")");
                                    unfillguest.Text = ("UnFilled(" + totalunfill + ")");
                                    partialfillguest.Text = ("Partially Filled(" + totalpartialfill + ")");
                                }
                            }
                            FpSpread2.SaveChanges();
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].ColumnCount;
                            FpSpread2.Sheets[0].FrozenColumnCount = 1;
                            FpSpread2.Sheets[0].PageSize = FpSpread2.Sheets[0].RowCount;
                        }
                    }
                    FpSpread2.Visible = true;
                    tblStatusguest.Visible = true;
                    lblertrhos.Visible = false;
                    lblertrhos.Text = "No Records Found";
                    btn_trhos.Visible = true;
                    btn_trhosexit.Visible = true;
                }
            }
            else
            {
                FpSpread2.Visible = false;
                tblStatusguest.Visible = false;
                lblertrhos.Visible = true;
                lblertrhos.Text = "No Records Found";
                btn_trhos.Visible = false;
                btn_trhosexit.Visible = false;
            }
        }
        else
        {
            tblStatusguest.Visible = false;
            FpSpread2.Visible = false;
            lblertrhos.Visible = true;
            lblertrhos.Text = "Please Select All Field";
            btn_trhos.Visible = false;
            btn_trhosexit.Visible = false;
        }
    }
    protected void imagebtnpop3closeguest_Click(object sender, EventArgs e)
    {
        popwindow3trhos.Visible = false;
    }
    protected void btn_exittrhos_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }
    protected void btn_savetrhos_Click(object sender, EventArgs e)
    {
        savedetailstrhos();
        btn_go_Click(sender, e);
    }
    protected void savedetailstrhos()
    {
        try
        {
            string hoscode = Convert.ToString(ddl_tohostel.SelectedItem.Value);
            string fromhoscode = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);
            string roomno = Convert.ToString(ViewState["Roompk"]);
            string fromroomno = Convert.ToString(ViewState["Room1fk"]);
            string bulname = Convert.ToString(ViewState["Code"]);
            string frombulname = Convert.ToString(ViewState["bulid1fk"]);
            string floor = Convert.ToString(ViewState["Floorpk"]);
            string fromfloor = Convert.ToString(ViewState["floor1fk"]);
            string roomtype = Convert.ToString(txt_roomtype.Text);
            string date = Convert.ToString(txt_trhosdate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = "";
            getday = dt.ToString("MM/dd/yyyy");
            string dtaccessdate = "";
            dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = "";
            dtaccesstime = DateTime.Now.ToLongTimeString();
            string rollno = "";
            rollno = Convert.ToString(txt_pop1rollno.Text);
            string reason = "";
            string HTRES = "HTRES";
            string reasoncode = "";
            reason = Convert.ToString(ddl_reason.SelectedItem.Text);
            if (reason.Trim() != "Others")
            {
                reasoncode = Convert.ToString(ddl_reason.SelectedItem.Value);
            }
            else
            {
                reason = Convert.ToString(txt_reason.Text);
                reasoncode = subheadercode(HTRES, reason);
            }
            string hostelgender = d2.GetFunction("select HostelType from HM_HostelMaster where HostelMasterPK ='" + hoscode + "'");
            string studgender = d2.GetFunction("select Sex from applyn a,Registration r where a.app_no=r.App_No and r.Roll_No='" + txt_pop1rollno.Text + "'");
            string studentgen = "";
            if (studgender.Trim() == "0")
            {
                studentgen = "1";
            }
            else if (studgender.Trim() == "1")
            {
                studentgen = "2";
            }
            else if (studgender.Trim() == "2")
            {
                studentgen = "0";
            }
            int iv = 0;
            if (hostelgender.Trim() == studentgen.Trim() || hostelgender.Trim() == "0")
            {
                string q = "select students_allowed,Avl_Student from Room_Detail where Room_Type='" + txt_roomtype.Text + "' and Floor_Name='" + txt_floor.Text + "' and Room_Name='" + txt_room.Text + "' and Building_Name='" + txt_building.Text + "'";
                ds2.Clear();
                ds2 = d2.select_method_wo_parameter(q, "text");
                double comp1 = 0; double comp2 = 0;
                double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["students_allowed"].ToString()), out comp1);
                double.TryParse(Convert.ToString(ds2.Tables[0].Rows[0]["Avl_Student"]), out comp2);
                if (comp1 >= comp2 && comp1 != comp2)
                {
                    bool feeallot = false;
                    string up = " update Room_Detail set Avl_Student= Avl_Student - 1 where Roompk='" + fromroomno + "'";
                    int k = d2.update_method_wo_parameter(up, "text");
                    string query = " update Room_Detail set Avl_Student= isnull(Avl_Student,0) + 1 where Roompk='" + roomno + "'";
                    query += " update HT_HostelRegistration set BuildingFK ='" + bulname + "',RoomFK ='" + roomno + "',FloorFK ='" + floor + "',HostelAdmDate ='" + dt.ToString("MM/dd/yyyy") + "',HostelMasterFK='" + hoscode + "' where APP_No ='" + Convert.ToString(ViewState["app_no"]) + "' and HostelMasterFK='" + fromhoscode + "'";
                    query += "  insert into HT_Hostel_TransferDetails (App_no,TransferDate,From_HostelFk,To_HostelFk,Trans_Reasoncode,From_BuildingFk,To_BuildingFk,From_FloorFk,To_FloorFk,From_RoomFk,To_RoomFk)values('" + Convert.ToString(ViewState["app_no"]) + "','" + dt.ToString("MM/dd/yyyy") + "','" + fromhoscode + "','" + hoscode + "','" + frombulname + "','" + bulname + "','" + fromfloor + "','" + floor + "','" + fromroomno + "','" + roomno + "','" + reasoncode + "')";
                    iv = d2.update_method_wo_parameter(query, "Text");
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Please Select Correct Hostel";
                popwindow1.Visible = false;
            }
            if (iv != 0)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Transferred Successfully";
                popwindow1.Visible = false;
                poperrjs.Visible = false;
                btn_go_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
        }
    }
    public string subheadercode(string textcri, string subjename)
    {
        int subjec_no = 0;
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + Convert.ToString(ddl_pop1collegename.SelectedItem.Value) + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + Convert.ToString(ddl_pop1collegename.SelectedItem.Value) + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + Convert.ToString(ddl_pop1collegename.SelectedItem.Value) + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return Convert.ToString(subjec_no);
    }
    public string subjectcodenew(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + ddl_pop1collegename.SelectedItem.Value + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    public string subheadercodesus(string textcri, string subjename)
    {
        int subjec_no = 0;
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + ddl_pop1collegename.SelectedItem.Value + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToInt32(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return Convert.ToString(subjec_no);
    }
    public string subjectcodevac(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            string select_subno = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
            }
            else
            {
                string insertquery = "insert into CO_MasterValues(MasterCriteria,MasterValue,CollegeCode) values('" + textcri + "','" + subjename + "','" + ddl_pop1collegename.SelectedItem.Value + "')";
                int result = d2.update_method_wo_parameter(insertquery, "Text");
                if (result != 0)
                {
                    string select_subno1 = "select MasterCode from CO_MasterValues where MasterCriteria='" + textcri + "' and CollegeCode =" + ddl_pop1collegename.SelectedItem.Value + " and MasterValue='" + subjename + "'";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(select_subno1, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["MasterCode"]);
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    protected void bindhosteltrhos()
    {
        try
        {
            ds.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            string q = "select HostelMasterPK,HostelName  from HM_HostelMaster where HostelMasterPK ='" + ddl_pop1hostelname.SelectedItem.Value + "' and MessMasterFK   in (" + MessmasterFK + ") order by HostelMasterPK";
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_fromhostel.DataSource = ds;
                ddl_fromhostel.DataTextField = "HostelName";
                ddl_fromhostel.DataValueField = "HostelMasterPK";
                ddl_fromhostel.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void bindhosteltrhostel()
    {
        try
        {
            ds.Clear();
            string MessmasterFK = d2.GetFunction("select value from Master_Settings where settings='Mess Rights' and usercode='" + usercode + "'");
            string q = "select HostelMasterPK,HostelName  from HM_HostelMaster   where HostelMasterPK not in ('" + ddl_pop1hostelname.SelectedItem.Value + "') and MessmasterFK in(" + MessmasterFK + ") order by HostelMasterPK";
            ds = d2.select_method_wo_parameter(q, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_tohostel.DataSource = ds;
                ddl_tohostel.DataTextField = "HostelName";
                ddl_tohostel.DataValueField = "HostelMasterPK";
                ddl_tohostel.DataBind();
            }
        }
        catch
        {
        }
    }
    public void loadreason()
    {
        try
        {
            string headerquery = "";
            ddl_reason.Items.Clear();
            headerquery = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='HTRES' and CollegeCode='" + ddl_pop1collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_reason.DataSource = ds;
                ddl_reason.DataTextField = "MasterValue";
                ddl_reason.DataValueField = "MasterCode";
                ddl_reason.DataBind();
                ddl_reason.Items.Insert(0, "Select");
                ddl_reason.Items.Insert(ddl_reason.Items.Count, "Others");
            }
            else
            {
                ddl_reason.Items.Insert(0, "Select");
                ddl_reason.Items.Insert(ddl_reason.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    public void lnkbtn_suspension_Click(object sender, EventArgs e)
    {
        try
        {
            txt_fromdate.Attributes.Add("readonly", "readonly");
            txt_fromdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_todate.Attributes.Add("readonly", "readonly");
            txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txt_rejoindate.Attributes.Add("readonly", "readonly");
            txt_rejoindate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            Div4.Visible = true;
            cb_date.Checked = false;
            cb_rejoin.Checked = false;
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
            txt_susreason.Text = "";
            cb_rejoin.Checked = false;
            txt_rejoindate.Enabled = false;
            loadsusreason();
        }
        catch
        {
        }
    }
    protected void imgsuspension_Click(object sender, EventArgs e)
    {
        Div4.Visible = false;
    }
    protected void cb_date_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_date.Checked == true)
        {
            txt_fromdate.Enabled = true;
            txt_todate.Enabled = true;
        }
        if (cb_date.Checked == false)
        {
            txt_fromdate.Enabled = false;
            txt_todate.Enabled = false;
        }
    }
    protected void txt_todate_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (txt_fromdate.Text != "" && txt_todate.Text != "")
            {
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
                    lblalerterr.Text = "Enter ToDate greater than or equal to the FromDate ";
                    txt_todate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void loadsusreason()
    {
        try
        {
            string headerquery = "";
            ddl_susreason.Items.Clear();
            headerquery = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='HTSRE' and CollegeCode='" + ddl_pop1collegename.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(headerquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_susreason.DataSource = ds;
                ddl_susreason.DataTextField = "MasterValue";
                ddl_susreason.DataValueField = "MasterCode";
                ddl_susreason.DataBind();
                ddl_susreason.Items.Insert(0, "Select");
                ddl_susreason.Items.Insert(ddl_susreason.Items.Count, "Others");
            }
            else
            {
                ddl_susreason.Items.Insert(0, "Select");
                ddl_susreason.Items.Insert(ddl_susreason.Items.Count, "Others");
            }
        }
        catch
        {
        }
    }
    protected void cb_rejoin_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_rejoin.Checked == true)
        {
            txt_rejoindate.Enabled = true;
        }
        if (cb_rejoin.Checked == false)
        {
            txt_rejoindate.Enabled = false;
        }
    }
    protected void btn_exitsus_Click(object sender, EventArgs e)
    {
        Div4.Visible = false;
    }
    protected void btn_savesus_Click(object sender, EventArgs e)
    {
        try
        {
            string date = "";
            date = Convert.ToString(txt_fromdate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = "";
            getday = dt.ToString("MM/dd/yyyy");
            string todate = "";
            todate = Convert.ToString(txt_todate.Text);
            string[] splittodate = todate.Split('-');
            splittodate = splittodate[0].Split('/');
            DateTime dttodate = new DateTime();
            if (splittodate.Length > 0)
            {
                dttodate = Convert.ToDateTime(splittodate[1] + "/" + splittodate[0] + "/" + splittodate[2]);
            }
            string getday1 = "";
            getday1 = dttodate.ToString("MM/dd/yyyy");
            string rejoindate = "";
            rejoindate = Convert.ToString(txt_rejoindate.Text);
            string[] splitrejoindate = rejoindate.Split('-');
            splitrejoindate = splitrejoindate[0].Split('/');
            DateTime dtrejoindate = new DateTime();
            if (splitrejoindate.Length > 0)
            {
                dtrejoindate = Convert.ToDateTime(splitrejoindate[1] + "/" + splitrejoindate[0] + "/" + splitrejoindate[2]);
            }
            string getday2 = "";
            getday2 = dtrejoindate.ToString("MM/dd/yyyy");
            string dtaccessdate = "";
            dtaccessdate = DateTime.Now.ToString();
            string dtaccesstime = "";
            dtaccesstime = DateTime.Now.ToLongTimeString();
            string rollno = "";
            rollno = Convert.ToString(txt_pop1rollno.Text);
            string reason = "";
            string HTSRE = "HTSRE";
            string reasoncode = "";
            reason = Convert.ToString(ddl_susreason.SelectedItem.Text);
            if (reason.Trim() != "Others")
            {
                reasoncode = Convert.ToString(ddl_susreason.SelectedItem.Value);
            }
            else
            {
                reason = Convert.ToString(txt_susreason.Text);
                reasoncode = subheadercodesus(HTSRE, reason);
            }
            string fromhoscode = "";
            fromhoscode = Convert.ToString(ddl_pop1hostelname.SelectedItem.Value);
            string query = "";
            string chkdate = "";
            string fromdate = "";
            string todat = "";
            if (cb_date.Checked == true)
            {
                chkdate = "1";
                fromdate = getday;
                todat = getday1;
            }
            else
            {
                chkdate = "0";
                fromdate = null;
                todat = null;
            }
            string chkdate1 = "";
            string rejoindat = "";
            if (cb_rejoin.Checked == true)
            {
                chkdate1 = "1";
                rejoindat = getday2;
            }
            else
            {
                chkdate1 = "0";
                rejoindat = null;
            }
            if (dtrejoindate > dttodate)
            {
                string appno = " ";
                appno = d2.GetFunction("select APP_No  from Registration sm where  sm.Roll_No='" + txt_pop1rollno.Text + "'");
                ViewState["App_No"] = Convert.ToString(appno);
                ViewState["id"] = txtid.Text;
                query = "update HT_HostelRegistration set IsSuspend='1'  where APP_No ='" + appno + "'";
                query = query + " insert into HT_Suspension_Details (App_no,Suspension,FromDate,ToDate,Suspend_Reasoncode,Rejoin,ReJoinDate,HotelMasterFk) values ('" + appno + "','" + chkdate + "','" + fromdate + "','" + todat + "','" + reasoncode + "','" + chkdate1 + "','" + rejoindat + "','" + fromhoscode + "')";
                int iv = d2.update_method_wo_parameter(query, "Text");
                if (iv != 0)
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Suspended Successfully";
                    popwindow1.Visible = false;
                    Div4.Visible = false;
                    btn_go_Click(sender, e);
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Please Change Rejoin Date";
            }
        }
        catch
        {
        }
    }
    protected void cb_vacate_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_vacate.Checked == true)
        {
            txt_vacate.Enabled = true;
            vacateformdate_div.Visible = true;
        }
        else
        {
            cb_vacatedatebetween.Checked = false;
            txt_vacate.Enabled = false;
            vacateformdate_div.Visible = false;
        }
    }
    protected void Fpspread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
            string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "1")
            {
                if (Fpspread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[0, 1].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
                        {
                            Fpspread1.Sheets[0].Cells[i, 1].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
    protected void btn_vacate_Click(object sender, EventArgs e)
    {
        try
        {
            string vacatedDet = string.Empty;
            string RoomPK = string.Empty;
            int insert = 0;
            string date = Convert.ToString(txt_vacate.Text);
            string[] splitdate = date.Split('-');
            splitdate = splitdate[0].Split('/');
            DateTime dt = new DateTime();
            if (splitdate.Length > 0)
            {
                dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            string getday = dt.ToString("MM/dd/yyyy");
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                for (int i = 0; i < Fpspread1.Sheets[0].RowCount; i++)
                {
                    Fpspread1.SaveChanges();
                    int checkval = Convert.ToInt32(Fpspread1.Sheets[0].Cells[i, 1].Value);
                    if (checkval == 1)
                    {
                        if (string.IsNullOrEmpty(vacatedDet))
                            vacatedDet = Convert.ToString(Fpspread1.Sheets[0].GetTag(i, 2)) + "-" + Convert.ToString(Fpspread1.Sheets[0].GetNote(i, 0)) + "-" + Convert.ToString(Fpspread1.Sheets[0].GetNote(i, 2));
                        else
                            vacatedDet = vacatedDet + "$" + Convert.ToString(Fpspread1.Sheets[0].GetTag(i, 2)) + "-" + Convert.ToString(Fpspread1.Sheets[0].GetNote(i, 0)) + "-" + Convert.ToString(Fpspread1.Sheets[0].GetNote(i, 2));
                    }
                }
                string reasonds = Convert.ToString(txt_vatreason.Text);
                string reasoncode = subjectcodevac("HSVAC", reasonds);
                string[] details = vacatedDet.Split('$');
                if (details.Length > 0)
                {
                    for (i = 0; i < details.Length; i++)
                    {
                        string[] rowDet = Convert.ToString(details[i]).Split('-');
                        string app_no = Convert.ToString(rowDet[0]);
                        string HostelmasterPk = Convert.ToString(rowDet[1]);
                        string roompk = Convert.ToString(rowDet[2]);
                        string sql = " update Room_Detail set Avl_Student= Avl_Student - 1 where Roompk='" + roompk + "'";
                        sql += " update HT_HostelRegistration set IsVacated='1',reason='" + reasoncode + "',VacatedDate='" + getday + "' where APP_No ='" + app_no + "' and HostelMasterFK ='" + HostelmasterPk + "' ";
                        insert = d2.update_method_wo_parameter(sql, "TEXT");
                    }
                }
                if (insert != 0)
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Vacated Successfully";
                    btn_go_Click(sender, e);
                }
                else
                {
                    imgdiv2.Visible = true;
                    lblalerterr.Text = "Please Select Any Record";
                    btn_go_Click(sender, e);
                }
            }
            else
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "No Records Found";
                btn_go_Click(sender, e);
            }
        }
        catch
        {
        }
    }
    protected void cbl_clg_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int commcount = 0;
            txt_college.Text = "--Select--";
            cb_clg.Checked = false;
            for (int i = 0; i < cbl_clg.Items.Count; i++)
            {
                if (cbl_clg.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_college.Text = "College(" + commcount.ToString() + ")";
                if (commcount == cbl_clg.Items.Count)
                {
                    cb_clg.Checked = true;
                }
            }
            if (cb1.Checked)
            {
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
                bindhostel();
                degree();
                string branch = rs.GetSelectedItemsValueAsString(cbl_degree);
                bindbranch(branch);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void cb_clg_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_clg.Checked == true)
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = true;
                }
                txt_college.Text = "College(" + (cbl_clg.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_clg.Items.Count; i++)
                {
                    cbl_clg.Items[i].Selected = false;
                }
                txt_college.Text = "--Select--";
            }
            if (cb1.Checked)
            {
                txt_degree.Text = "--Select--";
                txt_branch.Text = "--Select--";
                bindhostel();
                degree();
                bindbranch(college);
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void linkwithdrawal_click(object sender, EventArgs e)
    {
        studentdetailspdf();
    }
    public void studentdetailspdf()
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            mypdfpage = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Font Fontsmall1 = new Font("Arial", 11, FontStyle.Regular);
            int left1 = 1;
            #region college details
            string strquery = "";
            strquery = "Select * from collinfo where college_code='" + ddl_pop1collegename.SelectedItem.Value + "'";
            strquery += " select batch_year,college_code from registration where app_no='" + Convert.ToString(ViewState["app_no"]) + "'";
            DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
            string collname = "";
            string address1 = "";
            string address2 = "";
            string address3 = "";
            string pincode = "";
            string affliated = ""; string category = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                collname = ds.Tables[0].Rows[0]["collname"].ToString();
                if (Convert.ToString(ds.Tables[0].Rows[0]["com_name"]).Trim() != "")
                    collname = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
                address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                category = ds.Tables[0].Rows[0]["category"].ToString();
            }
            #endregion
            string batch_year = "";
            if (ds.Tables[1].Rows.Count > 0)
            {
                batch_year = Convert.ToString(ds.Tables[1].Rows[0]["batch_year"]);
            }
            string[] coursename = txt_pop1degree.Text.Split('-');
            string[] split = collname.Split('(');
            int coltop = 23;
            #region With draw from 1
            mypdfpage = mydocument.NewPage();
            PdfArea P4 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
            PdfRectangle P4R = new PdfRectangle(mydocument, P4, Color.Black);
            mypdfpage.Add(P4R);
            coltop = 20;
            PdfTextArea ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " HOSTEL");
            mypdfpage.Add(ptc);
            coltop = coltop + 10;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " + 
            mypdfpage.Add(ptc);
            coltop = coltop + 25;
            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                                        new PdfArea(mydocument, 0, coltop - 20, mydocument.PageWidth, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
            //mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
            mypdfpage.Add(ptc);
            left1 = 65; coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 3, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, txt_pop1name.Text);// Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 3, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, batch_year + " / " + coursename[0]);// Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 3, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1roomno.Text);//Convert.ToString(ds.Tables[0].Rows[0]["room_name"]));
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 3, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1rollno.Text);//Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += -2;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy      hh:mm:ss tt")));
            mypdfpage.Add(ptc);



            coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
            mypdfpage.Add(ptc);
            left1 = 50; coltop += 40;
            PdfArea br = new PdfArea(mydocument, left1, coltop, 500, 180);// 14, 12, 560, 825);
            PdfRectangle brr = new PdfRectangle(mydocument, br, Color.Black);
            mypdfpage.Add(brr);
            coltop -= 15; left1 = 60;
            ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Clearance must be obtained for the following particulars Breakage Report");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " a. Cot ");
            mypdfpage.Add(ptc);
            PdfArea a = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle ar = new PdfRectangle(mydocument, a, Color.Black);
            mypdfpage.Add(ar);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 300, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " g. miscellaneous ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " b. Table ");
            mypdfpage.Add(ptc);
            PdfArea b = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle bbr = new PdfRectangle(mydocument, b, Color.Black);
            mypdfpage.Add(bbr);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " c. chair ");
            mypdfpage.Add(ptc);
            PdfArea c = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle cr = new PdfRectangle(mydocument, c, Color.Black);
            mypdfpage.Add(cr);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " d. Cupboard ");
            mypdfpage.Add(ptc);
            PdfArea d = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle dr = new PdfRectangle(mydocument, d, Color.Black);
            mypdfpage.Add(dr);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " e. Fans ");
            mypdfpage.Add(ptc);
            PdfArea ee = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle er = new PdfRectangle(mydocument, ee, Color.Black);
            mypdfpage.Add(er);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " f. Lights ");
            mypdfpage.Add(ptc);
            PdfArea f = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
            PdfRectangle fr = new PdfRectangle(mydocument, f, Color.Black);
            mypdfpage.Add(fr);
            coltop += 13; left1 = 50;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 10;
            left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " h. Sports Items ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " i. Identity Card ");
            mypdfpage.Add(ptc);
            coltop -= 50; left1 = 250;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Room Boy");
            mypdfpage.Add(ptc);
            left1 = 450;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Supervisor");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
            mypdfpage.Add(ptc);
            coltop += 50; left1 = 400;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hostel Office");//Store keeper
            mypdfpage.Add(ptc);
            coltop += 40; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
            mypdfpage.Add(ptc);
            coltop += 30; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deposit                       Rs _________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Less: Dues                 Rs _________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Balance Amount to be");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Refunded / Collected  Rs _________________");
            mypdfpage.Add(ptc);
            left1 = 360;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hostel Office Clerk with Office Seal");
            mypdfpage.Add(ptc);
            coltop += 50; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Fees Dues if Any .............................................................................");
            mypdfpage.Add(ptc);
            left1 = 360;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of College Office Clerk with Office Seal");
            mypdfpage.Add(ptc);
            coltop += 50; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
            mypdfpage.Add(ptc); left1 = 400;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
            mypdfpage.Add(ptc);
            coltop += 5; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
            mypdfpage.Add(ptc);
            coltop += 5; left1 = 400;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Principal & Warden");
            mypdfpage.Add(ptc);
            coltop += 25; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " Submitted to the Hony.Secretary & Correspondent for approval ");
            mypdfpage.Add(ptc);
            coltop += 15; left1 = 50;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Rupees .........................................................................................................");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
            mypdfpage.Add(ptc);
            mypdfpage.SaveToDocument();
            #endregion
            //page 6
            #region withdraw form2
            mypdfpage = mydocument.NewPage();
            PdfArea P5 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
            PdfRectangle P5R = new PdfRectangle(mydocument, P5, Color.Black);
            mypdfpage.Add(P5R);
            coltop = 20;
            ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                              new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " HOSTEL");
            mypdfpage.Add(ptc);
            coltop = coltop + 10;
            ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                    new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " + 
            mypdfpage.Add(ptc);
            coltop = coltop + 25;
            //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
            //                                        new PdfArea(mydocument, 0, coltop - 20, mydocument.PageWidth, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
            mypdfpage.Add(ptc);
            left1 = 65; coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 3, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, txt_pop1name.Text);// Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 3, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, batch_year + " / " + coursename[0]);//txt_pop1degree.Text);//Convert.ToString(ds.Tables[0].Rows[0]["batch_year"]) + " / " + Convert.ToString(ds.Tables[0].Rows[0]["Course_Name"]));
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 3, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1roomno.Text); //Convert.ToString(ds.Tables[0].Rows[0]["room_name"]));
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 3, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1rollno.Text);//Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += -2;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy      hh:mm:ss tt")));
            mypdfpage.Add(ptc);
            coltop += 40;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
            mypdfpage.Add(ptc);
            left1 = 50; coltop += 40;
            PdfArea m = new PdfArea(mydocument, left1, coltop, 500, 120);// 14, 12, 560, 825);
            PdfRectangle mr = new PdfRectangle(mydocument, m, Color.Black);
            mypdfpage.Add(mr);
            coltop -= 0; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " No. of days Mess Bill : Rs ");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
            mypdfpage.Add(ptc);
            coltop += 30; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Extra                           : Rs ");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 170;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            coltop += 15; left1 = 120;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Total Rs");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 170;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Mess Contractor with seal");
            mypdfpage.Add(ptc);
            coltop += 50; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 60;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mess Advance Amount  Rs ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Total Mess Bill       Rs ");
            mypdfpage.Add(ptc);
            coltop += 15;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Others                   Rs ");
            mypdfpage.Add(ptc);
            coltop += 15;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Due Refundable             Rs ");
            mypdfpage.Add(ptc);
            coltop += 15;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "______________________");
            mypdfpage.Add(ptc);
            coltop += 50;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 - 35, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "______________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop - 10, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_________________________________");
            mypdfpage.Add(ptc);
            coltop += 5; left1 = 50;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 - 20, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 360, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of Office Clerk with Office Seal");
            mypdfpage.Add(ptc);
            //coltop += 40; left1 = 40;
            //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " ");//Submitted to the Hony.Secretary & Correspondent for approval 
            //mypdfpage.Add(ptc);
            //coltop += 5; 
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Signature of the Principal & Warden");
            mypdfpage.Add(ptc);
            coltop += 20; left1 = 50;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop - 10, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "_____________________________________________________________________________________________________________");
            mypdfpage.Add(ptc);
            coltop += 10; left1 = 40;
            ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Rupees .........................................................................................................");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
            mypdfpage.Add(ptc);
            coltop += 20;
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
            mypdfpage.Add(ptc);
            ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 60, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
            mypdfpage.Add(ptc);
            mypdfpage.SaveToDocument();
            #endregion
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
        }
        catch (Exception ex)
        {
        }
    }
    public string subjectcode(string textcri)
    {
        string subjec_no = "";
        try
        {
            DataSet ds23 = new DataSet();
            string select_subno = "select TextVal from textvaltable where TextCode ='" + textcri + "'";// and college_code ='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' ";
            ds23.Clear();
            ds23 = d2.select_method_wo_parameter(select_subno, "Text");
            if (ds23.Tables[0].Rows.Count > 0)
            {
                subjec_no = Convert.ToString(ds23.Tables[0].Rows[0]["TextVal"]);
            }
        }
        catch
        {
        }
        return subjec_no;
    }
    protected void lnk_vacated_Click(object sender, EventArgs e)
    {
        multiplestudentdetailspdf();
    }
    public void multiplestudentdetailspdf()
    {
        try
        {
            Gios.Pdf.PdfDocument mydocument = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            Gios.Pdf.PdfPage mypdfpage = mydocument.NewPage();
            mypdfpage = mydocument.NewPage();
            Font header = new Font("Arial", 15, FontStyle.Bold);
            Font header1 = new Font("Arial", 14, FontStyle.Bold);
            Font Fonthead = new Font("Arial", 12, FontStyle.Bold);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Font Fontbold2 = new Font("Times New Roman", 9, FontStyle.Bold);
            Font Fonttimes = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontsmall = new Font("Arial", 9, FontStyle.Regular);
            Font FontsmallBold = new Font("Arial", 10, FontStyle.Bold);
            Font fontitalic = new Font("Arial", 9, FontStyle.Italic);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Font Fontsmall1 = new Font("Arial", 11, FontStyle.Regular);
            int left1 = 1; string value = "";

            string q = " select r.stud_name,convert(varchar, r.batch_year) +' / '+c.Course_Name as class,rd.room_Name,r.college_code,r.roll_no,h.app_no,(select mastervalue from CO_MasterValues where convert(varchar,mastercode)=convert(varchar,h.reason))as reason from room_detail rd,HT_HostelRegistration h,Registration r,Degree d,Department dt,Course c where h.roomfk=rd.roompk and h.app_no=r.app_no and r.degree_code =d.Degree_Code and  d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id  ";
            ds2.Clear();
            ds2 = d2.select_method_wo_parameter(q, "text");
            Fpspread1.SaveChanges();
            for (int i = 1; i < Fpspread1.Sheets[0].RowCount; i++)
            {
                value = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 1].Value);
                if (value == "1")
                {
                    #region college details
                    string strquery = "";
                    string clgcode = "";
                    string classname = "";
                    string rollno = "";
                    string roomno = "";
                    string studentname = "";
                    string app_no = Convert.ToString(Fpspread1.Sheets[0].Cells[i, 2].Tag);
                    string reason = "";
                    ds2.Tables[0].DefaultView.RowFilter = " app_no=" + app_no + "";
                    DataView dv = ds2.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        classname = Convert.ToString(dv[0]["class"]);
                        clgcode = Convert.ToString(dv[0]["college_code"]);
                        rollno = Convert.ToString(dv[0]["roll_no"]);
                        roomno = Convert.ToString(dv[0]["room_Name"]);
                        studentname = Convert.ToString(dv[0]["stud_name"]);
                        reason = Convert.ToString(dv[0]["reason"]).ToUpper();
                    }
                    if (reason.Trim() == "")
                        reason = txt_vatreason.Text.ToUpper();
                    if (clgcode.Trim() == "")
                        clgcode = "13";
                    strquery = "Select * from collinfo where college_code='" + clgcode + "'";
                    DataSet ds = d2.select_method_wo_parameter(strquery, "Text");
                    string collname = "";
                    string address1 = "";
                    string address2 = "";
                    string address3 = "";
                    string pincode = "";
                    string affliated = ""; string category = "";
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        collname = ds.Tables[0].Rows[0]["collname"].ToString();
                        if (Convert.ToString(ds.Tables[0].Rows[0]["com_name"]).Trim() != "")
                            collname = Convert.ToString(ds.Tables[0].Rows[0]["com_name"]);
                        address1 = ds.Tables[0].Rows[0]["address1"].ToString();
                        address2 = ds.Tables[0].Rows[0]["address2"].ToString();
                        address3 = ds.Tables[0].Rows[0]["address3"].ToString();
                        pincode = ds.Tables[0].Rows[0]["pincode"].ToString();
                        affliated = ds.Tables[0].Rows[0]["affliatedby"].ToString();
                        category = ds.Tables[0].Rows[0]["category"].ToString();
                    }
                    #endregion

                    string[] coursename = txt_pop1degree.Text.Split('-');
                    string[] split = collname.Split('(');
                    int coltop = 23;
                    #region With draw from 1
                    mypdfpage = mydocument.NewPage();
                    PdfArea P4 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                    PdfRectangle P4R = new PdfRectangle(mydocument, P4, Color.Black);
                    mypdfpage.Add(P4R);
                    coltop = 20;
                    PdfTextArea ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " HOSTEL");
                    mypdfpage.Add(ptc);
                    coltop = coltop + 10;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " + 
                    mypdfpage.Add(ptc);
                    coltop = coltop + 25;
                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                    //                                        new PdfArea(mydocument, 0, coltop - 20, mydocument.PageWidth, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                    //mypdfpage.Add(ptc);
                    coltop += 20; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    mypdfpage.Add(ptc);
                    left1 = 65; coltop += 40;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 3, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, studentname);// Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 3, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, classname);//batch_year + " / " + coursename[0]
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 3, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, roomno);//txt_pop1roomno.Text
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 3, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, rollno);//Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop - 3, 250, 50), System.Drawing.ContentAlignment.MiddleCenter, reason);//reason batch_year + " / " + coursename[0]
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
                    mypdfpage.Add(ptc);
                    coltop += -2;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy      hh:mm:ss tt")));
                    mypdfpage.Add(ptc);



                    coltop += 40;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                    mypdfpage.Add(ptc);
                    left1 = 50; coltop += 40;
                    PdfArea br = new PdfArea(mydocument, left1, coltop, 500, 180);// 14, 12, 560, 825);
                    PdfRectangle brr = new PdfRectangle(mydocument, br, Color.Black);
                    mypdfpage.Add(brr);
                    coltop -= 15; left1 = 60;
                    ptc = new PdfTextArea(FontsmallBold, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Clearance must be obtained for the following particulars Breakage Report");
                    mypdfpage.Add(ptc);
                    coltop += 20; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " a. Cot ");
                    mypdfpage.Add(ptc);
                    PdfArea a = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle ar = new PdfRectangle(mydocument, a, Color.Black);
                    mypdfpage.Add(ar);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 300, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " g. miscellaneous ");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " b. Table ");
                    mypdfpage.Add(ptc);
                    PdfArea b = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle bbr = new PdfRectangle(mydocument, b, Color.Black);
                    mypdfpage.Add(bbr);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " c. chair ");
                    mypdfpage.Add(ptc);
                    PdfArea c = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle cr = new PdfRectangle(mydocument, c, Color.Black);
                    mypdfpage.Add(cr);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " d. Cupboard ");
                    mypdfpage.Add(ptc);
                    PdfArea d = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle dr = new PdfRectangle(mydocument, d, Color.Black);
                    mypdfpage.Add(dr);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " e. Fans ");
                    mypdfpage.Add(ptc);
                    PdfArea ee = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle er = new PdfRectangle(mydocument, ee, Color.Black);
                    mypdfpage.Add(er);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " f. Lights ");
                    mypdfpage.Add(ptc);
                    PdfArea f = new PdfArea(mydocument, left1 + 70, coltop + 15, 15, 15);// 14, 12, 560, 825);
                    PdfRectangle fr = new PdfRectangle(mydocument, f, Color.Black);
                    mypdfpage.Add(fr);
                    coltop += 13; left1 = 50;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
                    mypdfpage.Add(ptc);
                    coltop += 10;
                    left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " h. Sports Items ");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " i. Identity Card ");
                    mypdfpage.Add(ptc);
                    coltop -= 50; left1 = 250;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Room Boy");
                    mypdfpage.Add(ptc);
                    left1 = 450;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Supervisor");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "___________________");
                    mypdfpage.Add(ptc);
                    coltop += 50; left1 = 400;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hostel Office");//Store keeper
                    mypdfpage.Add(ptc);
                    coltop += 40; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
                    mypdfpage.Add(ptc);
                    coltop += 30; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Deposit                       Rs _________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Less: Dues                 Rs _________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Balance Amount to be");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Refunded / Collected  Rs _________________");
                    mypdfpage.Add(ptc);
                    left1 = 360;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of Hostel Office Clerk with Office Seal");
                    mypdfpage.Add(ptc);
                    coltop += 50; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Fees Dues if Any .............................................................................");
                    mypdfpage.Add(ptc);
                    left1 = 360;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of College Office Clerk with Office Seal");
                    mypdfpage.Add(ptc);
                    coltop += 50; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                    mypdfpage.Add(ptc); left1 = 400;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                    mypdfpage.Add(ptc);
                    coltop += 5; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
                    mypdfpage.Add(ptc);
                    coltop += 5; left1 = 400;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Principal & Warden");
                    mypdfpage.Add(ptc);
                    coltop += 25; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " Submitted to the Hony.Secretary & Correspondent for approval ");
                    mypdfpage.Add(ptc);
                    coltop += 15; left1 = 50;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "____________________________________________________________________________________________________");
                    mypdfpage.Add(ptc);
                    coltop += 10; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Rupees .........................................................................................................");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                    mypdfpage.Add(ptc);
                    mypdfpage.SaveToDocument();
                    #endregion
                    //page 6
                    #region withdraw form2
                    mypdfpage = mydocument.NewPage();
                    PdfArea P5 = new PdfArea(mydocument, 14, 14, 565, 810);// 14, 12, 560, 825);
                    PdfRectangle P5R = new PdfRectangle(mydocument, P5, Color.Black);
                    mypdfpage.Add(P5R);
                    coltop = 20;
                    ptc = new PdfTextArea(header, System.Drawing.Color.Black,
                                                                      new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, Convert.ToString(split[0]) + " HOSTEL");
                    mypdfpage.Add(ptc);
                    coltop = coltop + 10;
                    ptc = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, address3 + " - " + pincode + ".  INDIA");//address1 + " , " + address2 + " , " + 
                    mypdfpage.Add(ptc);
                    coltop = coltop + 25;
                    //ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                    //                                        new PdfArea(mydocument, 0, coltop - 20, mydocument.PageWidth, 55), System.Drawing.ContentAlignment.MiddleCenter, affliated);
                    mypdfpage.Add(ptc);
                    coltop += 20; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Withdrawal Form ");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    mypdfpage.Add(ptc);
                    left1 = 65; coltop += 40;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Name of the Boarder _________________________________________________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 100, coltop - 3, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, studentname);// Convert.ToString(ds.Tables[0].Rows[0]["stud_name"]));
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Class ________________________________ Room No _________________ Roll No _____________________ ");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 40, coltop - 3, 150, 50), System.Drawing.ContentAlignment.MiddleCenter, classname);//txt_pop1degree.Text
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 3, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, roomno);//txt_pop1roomno.Text
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 3, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, rollno);//Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 240, coltop - 3, 70, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1roomno.Text); //Convert.ToString(ds.Tables[0].Rows[0]["room_name"]));
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 3, 100, 50), System.Drawing.ContentAlignment.MiddleCenter, txt_pop1rollno.Text);//Convert.ToString(ds.Tables[0].Rows[0]["roll_no"]));
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Purpose of vacating Room _____________________________________________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop - 3, 250, 50), System.Drawing.ContentAlignment.MiddleCenter, reason);//reason batch_year + " / " + coursename[0]
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date & Time of vacating   ______________________________________________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 200, coltop, 400, 50), System.Drawing.ContentAlignment.MiddleLeft, Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy      hh:mm:ss tt")));
                    mypdfpage.Add(ptc);
                    coltop += 40;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                    mypdfpage.Add(ptc);
                    left1 = 50; coltop += 40;
                    PdfArea m = new PdfArea(mydocument, left1, coltop, 500, 120);// 14, 12, 560, 825);
                    PdfRectangle mr = new PdfRectangle(mydocument, m, Color.Black);
                    mypdfpage.Add(mr);
                    coltop -= 0; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " No. of days Mess Bill : Rs ");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Date   : " + System.DateTime.Now.ToString("dd/MM/yyyy"));
                    mypdfpage.Add(ptc);
                    coltop += 30; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Extra                           : Rs ");
                    mypdfpage.Add(ptc);
                    coltop += 10; left1 = 170;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    coltop += 15; left1 = 120;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Total Rs");
                    mypdfpage.Add(ptc);
                    coltop += 10; left1 = 170;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "__________________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 170, coltop + 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Mess Contractor with seal");
                    mypdfpage.Add(ptc);
                    coltop += 50; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " For Office use Only ");
                    mypdfpage.Add(ptc);
                    coltop += 20; left1 = 60;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Mess Advance Amount  Rs ");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Total Mess Bill       Rs ");
                    mypdfpage.Add(ptc);
                    coltop += 15;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Less Others                   Rs ");
                    mypdfpage.Add(ptc);
                    coltop += 15;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_______________________");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Due Refundable             Rs ");
                    mypdfpage.Add(ptc);
                    coltop += 15;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 120, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "______________________");
                    mypdfpage.Add(ptc);
                    coltop += 50;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 - 35, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "______________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop - 10, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "______________________________");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 350, coltop - 10, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "_________________________________");
                    mypdfpage.Add(ptc);
                    coltop += 5; left1 = 50;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 - 20, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of the Deputy Warden");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 360, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Signature of Office Clerk with Office Seal");
                    mypdfpage.Add(ptc);
                    //coltop += 40; left1 = 40;
                    //ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 565, 50), System.Drawing.ContentAlignment.MiddleCenter, " ");//Submitted to the Hony.Secretary & Correspondent for approval 
                    //mypdfpage.Add(ptc);
                    //coltop += 5; 
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Signature of the Principal & Warden");
                    mypdfpage.Add(ptc);
                    coltop += 20; left1 = 50;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop - 10, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, "_____________________________________________________________________________________________________________");
                    mypdfpage.Add(ptc);
                    coltop += 10; left1 = 40;
                    ptc = new PdfTextArea(Fonthead, System.Drawing.Color.Black, new PdfArea(mydocument, 0, coltop, mydocument.PageWidth, 50), System.Drawing.ContentAlignment.MiddleCenter, " Voucher ");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Received Cheque / Cash Rs ................................... Rupees .........................................................................................................");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " ...................................................................................................................................................................................................................");
                    mypdfpage.Add(ptc);
                    coltop += 20;
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1, coltop, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, " Cheque No................................................................................ Date ...........................");
                    mypdfpage.Add(ptc);
                    ptc = new PdfTextArea(Fontsmall, System.Drawing.Color.Black, new PdfArea(mydocument, left1 + 400, coltop + 60, 550, 50), System.Drawing.ContentAlignment.MiddleLeft, "Signature of the Boarder");
                    mypdfpage.Add(ptc);
                    mypdfpage.SaveToDocument();
                    #endregion
                }
            }
            string appPath = HttpContext.Current.Server.MapPath("~");
            if (appPath != "")
            {
                string szPath = appPath + "/Report/";
                string szFile = "ApplicationForm" + DateTime.Now.ToString("ddMMyyyy") + ".pdf";
                mydocument.SaveToFile(szPath + szFile);
                Response.ClearHeaders();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/pdf";
                Response.WriteFile(szPath + szFile);
                Response.End();
            }
            else
            { }
        }
        catch (Exception ex)
        {
        }
    }

    protected bool Hostelfeesallot(string collegecode, string Batchyear, string Degreecode, string Current_semester, string Seattype, string App_no, string HostelFk, string Roomtype)
    {
        bool feeallotcheck = false;
        if (string.IsNullOrEmpty(Batchyear.Trim()) || string.IsNullOrEmpty(Degreecode.Trim()) || string.IsNullOrEmpty(Current_semester.Trim()) || string.IsNullOrEmpty(Seattype.Trim()))
        {
            query = "select r.batch_year,r.degree_code,r.current_semester,a.seattype,r.app_no from applyn a,registration r where r.app_no=a.app_no and r.app_no='" + App_no + "'";
            DataSet studentdet_ds = d2.select_method_wo_parameter(query, "text");
            if (studentdet_ds.Tables[0].Rows.Count > 0)
            {
                Batchyear = Convert.ToString(studentdet_ds.Tables[0].Rows[0]["batch_year"]);
                Degreecode = Convert.ToString(studentdet_ds.Tables[0].Rows[0]["degree_code"]);
                Current_semester = Convert.ToString(studentdet_ds.Tables[0].Rows[0]["current_semester"]);
                Seattype = Convert.ToString(studentdet_ds.Tables[0].Rows[0]["seattype"]);
            }
        }
        string type = string.Empty;
        bool semesterCheck = false;
        string feecatagory = string.Empty;// getFeecategory(collegecode, Convert.ToString(Session["usercode"]), Current_semester + " ", ref type);
        if (cblmulterm.Items.Count > 0)
        {
            for (int i = 0; i < cblmulterm.Items.Count; i++)
            {
                if (cblmulterm.Items[i].Selected == true)
                {
                    semesterCheck = true;
                    feecatagory = Convert.ToString(cblmulterm.Items[i].Value);
                    if (!string.IsNullOrEmpty(feecatagory))
                    {
                        DataSet studentFeesdet_ds = d2.select_method_wo_parameter("select TotalAmount,headerfk,ledgerfk from FT_FeeAllotDegree where isHostelFees =1 and batchyear='" + Batchyear + "' and degreecode='" + Degreecode + "' and hostelmasterfk='" + HostelFk + "' and roomtype='" + Roomtype + "' and feecategory='" + feecatagory + "' and seattype='" + Seattype + "'", "text");
                        string FinyearFk = string.Empty;
                        string Hostelheader = string.Empty;
                        string Hostelledger = string.Empty;
                        string cost = string.Empty;
                        FinyearFk = d2.getCurrentFinanceYear(Convert.ToString(Session["usercode"]), collegecode);
                        if (!string.IsNullOrEmpty(FinyearFk) || FinyearFk.Trim() != "0")
                        {
                            int feeallot = 0;
                            foreach (DataRow dr in studentFeesdet_ds.Tables[0].Rows)
                            {
                                Hostelheader = Convert.ToString(dr["headerfk"]);
                                Hostelledger = Convert.ToString(dr["ledgerfk"]);
                                cost = Convert.ToString(dr["TotalAmount"]);
                                string insupdquery = "if exists (select * from FT_FeeAllot where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + feecatagory + "')  and App_No in('" + App_no + "')) update FT_FeeAllot set AllotDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',MemType='1',FeeAmount='" + cost + "',PaidAmount='0' ,DeductAmout='0',DeductReason='0',FromGovtAmt='0',TotalAmount='" + cost + "',RefundAmount='0',IsFeeDeposit='0',FeeAmountMonthly='',PayMode='0',PayStartDate='',PaidStatus='0',DueDate='" + DateTime.Now.ToString("MM/dd/yyyy") + "',DueAmount='0',FineAmount='0',BalAmount='" + cost + "' where LedgerFK in('" + Hostelledger + "') and HeaderFK in('" + Hostelheader + "') and FeeCategory in('" + feecatagory + "') and App_No in('" + App_no + "') and isnull(PaidAmount,0)='0' else INSERT INTO FT_FeeAllot(AllotDate,MemType,App_No,LedgerFK,HeaderFK,FeeAmount, DeductAmout,DeductReason,FromGovtAmt,TotalAmount,RefundAmount, IsFeeDeposit, FeeAmountMonthly,PayMode,FeeCategory,PayStartDate,PaidStatus,DueDate,DueAmount,FineAmount,BalAmount,FinYearFK) VALUES('" + DateTime.Now.ToString("MM/dd/yyyy") + "',1," + App_no + ",'" + Hostelledger + "','" + Hostelheader + "','" + cost + "','0','0','0','" + cost + "','0','0','','0','" + feecatagory + "','','0','','0','0','" + cost + "','" + FinyearFk + "')";
                                feeallot = d2.update_method_wo_parameter(insupdquery, "text");
                                if (feeallot != 0)
                                    feeallotcheck = true;
                            }
                        }
                        else
                        {
                            imgdiv2.Visible = true;
                            lblalerterr.Text = "Set Financial year settings";
                        }
                    }
                    else
                    {
                        errmsg.Visible = true;
                        errmsg.Text = "Please select semester ";
                    }
                }
            }
            if (!semesterCheck)
            {
                imgdiv2.Visible = true;
                lblalerterr.Text = "Please select semester";
            }
        }
        return feeallotcheck;
    }
    public string getFeecategory(string collegecode, string usercode, string currentsem, ref string linkName)
    {
        string feecatagory = string.Empty;
        try
        {
            string linkValue = string.Empty;
            string SelectQ = string.Empty;
            linkValue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'SemesterandYear' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
            if (!string.IsNullOrEmpty(linkValue) && linkValue != "0")
            {
                feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and (textval like '%" + currentsem + "Semester' or textval like '%Year') and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");
                linkName = "SemesterandYear";
            }
            else
            {
                linkValue = d2.GetFunction("select LinkValue from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode + "'");
                if (!string.IsNullOrEmpty(linkValue) && linkValue == "0")
                {
                    feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '%" + currentsem + "Semester' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");

                    linkName = "Semester";
                }
                else if (!string.IsNullOrEmpty(linkValue) && linkValue == "1")
                {
                    string csem = returnYearforSem(currentsem);
                    feecatagory = d2.GetFunction("select TextCode from textvaltable where TextCriteria = 'FEECA'and textval like '%" + csem + " Year' and textval not like '-1%' and college_code ='" + collegecode + "' order by len(textval),textval asc");
                    linkName = "Year";
                }
                else if (!string.IsNullOrEmpty(linkValue) && linkValue == "2")
                {
                    feecatagory = d2.GetFunction("select distinct TextCode from textvaltable t,Fee_degree_match f where t.textcode=f.feecategory and t.college_code=f.college_code and  TextCriteria = 'FEECA' and textval like '%Term" + currentsem + "%' and textval not like '-1%' and t.college_code ='" + collegecode + "' ");
                    //if (!string.IsNullOrEmpty(featDegreeCode))
                    //    SelectQ += "  and f.degree_code in('" + featDegreeCode + "') ";
                    //SelectQ += " order by len(textval),textval asc";

                    linkName = "Term";
                }
            }
        }
        catch { feecatagory = ""; }
        return feecatagory;
    }
    public string returnYearforSem(string cursem)
    {
        switch (cursem)
        {
            case "1":
            case "2":
                cursem = "1";
                break;
            case "3":
            case "4":
                cursem = "2";
                break;
            case "5":
            case "6":
                cursem = "3";
                break;
            case "7":
            case "8":
                cursem = "4";
                break;
            case "9":
            case "10":
                cursem = "5";
                break;
        }
        return cursem;
    }
    protected void cb_Hostelfeesallot_Checked(object sender, EventArgs e)
    {
        if (cb_Hostelfeesallot.Checked)
        {
            bindMulTerm(usercode);
            feecatagory();
            mulsemTD.Visible = true;

        }
        else
        {
            mulsemTD.Visible = false;
            txtmulsem.Text = "--Select--";
            cblmulterm.ClearSelection();
        }
    }
    protected void cbmulterm_CheckedChanged(object sender, EventArgs e)
    {
        string sem = string.Empty;
        if (cbmulterm.Checked == true)
        {
            for (int i = 0; i < cblmulterm.Items.Count; i++)
            {
                cblmulterm.Items[i].Selected = true;
                sem = Convert.ToString(cblmulterm.Items[i].Text);
            }
            if (cblmulterm.Items.Count == 1)
            {
                txtmulsem.Text = "" + sem + "";
            }
            else
            {
                txtmulsem.Text = "Sem(" + (cblmulterm.Items.Count) + ")";
            }
        }
        else
        {
            for (int i = 0; i < cblmulterm.Items.Count; i++)
            {
                cblmulterm.Items[i].Selected = false;
            }
            txtmulsem.Text = "--Select--";
        }
    }
    protected void cblmulterm_SelectedIndexChanged(object sender, EventArgs e)
    {
        txtmulsem.Text = "--Select--";
        cbmulterm.Checked = false;
        string sem = string.Empty;
        int commcount = 0;
        for (int i = 0; i < cblmulterm.Items.Count; i++)
        {
            if (cblmulterm.Items[i].Selected == true)
            {
                commcount = commcount + 1;
                sem = Convert.ToString(cblmulterm.Items[i].Text);
            }
        }
        if (commcount > 0)
        {
            if (commcount == 1)
            {
                txtmulsem.Text = "" + sem + "";
            }
            else
            {
                txtmulsem.Text = "Sem(" + commcount.ToString() + ")";
            }
            if (commcount == cblmulterm.Items.Count)
            {
                cbmulterm.Checked = true;
            }
        }
    }
    protected void bindMulTerm(string usercodes)
    {
        try
        {
            cblmulterm.Items.Clear();
            ds.Clear();
            string linkName = string.Empty;
            string cbltext = string.Empty;
            ds = d2.loadFeecategory(Convert.ToString(ddl_pop1collegename.SelectedItem.Value), usercodes, ref linkName);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblmulterm.DataSource = ds;
                cblmulterm.DataTextField = "TextVal";
                cblmulterm.DataValueField = "TextCode";
                cblmulterm.DataBind();
            }
        }
        catch { }
    }
    protected void feecatagory()
    {
        query = "select LinkValue from New_InsSettings where LinkName='IncludeMultipleTermSettings' and user_code ='" + usercode + "' and college_code ='" + ddl_pop1collegename.SelectedItem.Value + "' ";
        int set = Convert.ToInt32(d2.GetFunction(query));
        if (set == 1)
        {
            cbmulterm.Checked = true;
            query = string.Empty;
            query = "select LinkValue from New_InsSettings where LinkName='SelectedMultipleFeecategoryCode' and user_code ='" + usercode + "' and college_code ='" + ddl_pop1collegename.SelectedItem.Value + "' ";
            string feeCode = Convert.ToString(d2.GetFunction(query));
            if (feeCode != "0" && feeCode.Contains(","))
            {
                string[] splcode = feeCode.Split(',');
                if (splcode.Length > 0)
                {
                    for (int sp = 0; sp < splcode.Length; sp++)
                    {
                        try
                        {
                            cblmulterm.Items.FindByValue(Convert.ToString(splcode[sp].Trim())).Selected = true;
                        }
                        catch { }
                        //for (int sel = 0; sel < cblmulterm.Items.Count; sel++)
                        //{
                        //    if (splcode[sp].Trim() == cblmulterm.Items[sel].Value)
                        //    {
                        //        cblmulterm.Items[sel].Selected = true;
                        //    }
                        //}
                    }
                }
            }
        }
        else
            cbmulterm.Checked = false;
    }
    protected void BindStudentType()
    {
        try
        {
            ddlStudType.Items.Clear();
            ds.Clear();
            string sql = "select StudentType,StudentTypeName from HostelStudentType where CollegeCode='" + collegecode1 + "' order by StudentType ";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlStudType.DataSource = ds;
                ddlStudType.DataTextField = "StudentTypeName";
                ddlStudType.DataValueField = "StudentType";
                ddlStudType.DataBind();
            }
        }
        catch
        {
        }
    }
    protected void idgeneration()
    {
        try
        {
            string newitemcode = "";

            string ishostel = string.Empty;
            string memtype = string.Empty;
            string newins = string.Empty;
            string hos_code = string.Empty;
            string hos_code1 = string.Empty;
            string colcode = ddl_pop1collegename.SelectedValue;
            if (usercode != "")
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + usercode + "' and college_code ='" + ddl_pop1collegename.SelectedValue + "'";
            }
            else
            {
                newins = "select * from New_InsSettings where LinkName='hostelid generation' and user_code ='" + group_user + "' and college_code ='" + ddl_pop1collegename.SelectedValue + "'";
            }
            ds = d2.select_method_wo_parameter(newins, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ishostel = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
            }
            if (ishostel != "")
            {
                if (ishostel == "1")
                {
                    hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                    hos_code1 = Convert.ToString(ddl_pop1hostelname.SelectedValue);

                    memtype = "3";
                }
                if (ishostel == "0")
                {
                    hos_code = Convert.ToString(ddl_pop1hostelname.SelectedValue);
                    hos_code1 = "0";

                    ishostel = "0";

                    memtype = "0";

                }
                string selectquery = "select idAcr,idStNo,idSize from Hostelidgeneration where college_code='" + colcode + "' and hostelcode='" + hos_code1 + "' and ishostel='" + ishostel + "' and memtype='" + memtype + "' order by FromDate desc";//where Latestrec =1"
                ds = d2.select_method_wo_parameter(selectquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string itemacronym = Convert.ToString(ds.Tables[0].Rows[0]["idAcr"]);
                    string itemstarno = Convert.ToString(ds.Tables[0].Rows[0]["idStNo"]);
                    string itemsize = Convert.ToString(ds.Tables[0].Rows[0]["idSize"]);
                    if (itemacronym.Trim() != "" && itemstarno.Trim() != "")
                    {
                        selectquery = " select distinct top (1) id  from HT_HostelRegistration where id like '" + Convert.ToString(itemacronym) + "[0-9]%'  and HostelMasterFK='" + hos_code + "' order by id desc";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(selectquery, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string itemcode = Convert.ToString(ds.Tables[0].Rows[0]["id"]);
                            string itemacr = Convert.ToString(itemacronym);
                            int len = itemacr.Length;
                            itemcode = itemcode.Remove(0, len);
                            int len1 = Convert.ToString(itemcode).Length;
                            string newnumber = Convert.ToString((Convert.ToInt32(itemcode) + 1));
                            len = Convert.ToString(newnumber).Length;
                            len1 = len1 - len;
                            if (len1 == 2)
                            {
                                newitemcode = "00" + newnumber;
                            }
                            else if (len1 == 1)
                            {
                                newitemcode = "0" + newnumber;
                            }
                            else if (len1 == 3)
                            {
                                newitemcode = "000" + newnumber;
                            }
                            else if (len1 == 4)
                            {
                                newitemcode = "0000" + newnumber;
                            }
                            else if (len1 == 5)
                            {
                                newitemcode = "00000" + newnumber;
                            }
                            else if (len1 == 6)
                            {
                                newitemcode = "000000" + newnumber;
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
                            int len1 = Convert.ToInt32(items);
                            int size = len1 - len;
                            if (size == 2)
                            {
                                newitemcode = "00" + itemstarno;
                            }
                            else if (size == 1)
                            {
                                newitemcode = "0" + itemstarno;
                            }
                            else if (size == 3)
                            {
                                newitemcode = "000" + itemstarno;
                            }
                            else if (size == 4)
                            {
                                newitemcode = "0000" + itemstarno;
                            }
                            else if (size == 5)
                            {
                                newitemcode = "00000" + itemstarno;
                            }
                            else if (size == 6)
                            {
                                newitemcode = "000000" + itemstarno;
                            }
                            else
                            {
                                newitemcode = Convert.ToString(itemstarno);
                            }
                            newitemcode = Convert.ToString(itemacronym) + "" + Convert.ToString(newitemcode);
                        }
                        txtid.Text = Convert.ToString(newitemcode);

                        //poperrjs.Visible = true;
                        //btnsave.Visible = true;
                        //SelectdptGrid.Visible = false;
                        //btnupdate.Visible = false;
                        // btndelete.Visible = false;
                        // bindstore();
                        // bindunitddl();
                        // loadheadername();
                        //loadsubheadername();
                        // loaditem();
                        // bind_subheader();
                    }
                    else
                    {
                        imgdiv2.Visible = true;
                        //lbl_alert.Text = "Please Update Code Master";
                    }
                }
            }
            else
                txtid.Text = "";
        }
        catch
        {
        }
    }


    //Added By SaranyDevi 5.3.2016

    public string HostelVacate(string appno, string hostelset)
    {
        string Hostelvat = "";
        try
        {


            string Reqcnt = d2.GetFunction("select Count(*) from RQ_Requisition where MemType='1' and ReqAppNo ='" + appno + "' and stud='1' and RequestType='14'");
            if (Reqcnt != "" && Reqcnt != "0")
            {
                DataSet dshedled = new DataSet();
                string hedled = "select distinct bd.HeaderFK,bd.LedgerFK,ht.APP_No,bd.FeeCategory from HT_HostelRegistration ht,IT_BreakageDetails bd where ht.APP_No=bd.MemCode and ht.HostelMasterFK=bd.DeptFK and isnull(ht.IsDiscontinued,'0')='0' and isnull(ht.IsVacated,'0')='0' and ht.APP_No='" + appno + "'";
                dshedled.Clear();
                dshedled = d2.select_method_wo_parameter(hedled, "Text");
                if (dshedled.Tables.Count > 0 && dshedled.Tables[0].Rows.Count > 0)
                {
                    string reqappstaus = d2.GetFunction("select ReqAppStatus from RQ_Requisition where MemType='1' and ReqAppNo ='" + appno + "' and stud='1' and RequestType='14'");
                    if (reqappstaus == "1")
                    {
                        string headfk = Convert.ToString(dshedled.Tables[0].Rows[0]["HeaderFK"]);
                        string Leadfk = Convert.ToString(dshedled.Tables[0].Rows[0]["LedgerFK"]);
                        string feecat = Convert.ToString(dshedled.Tables[0].Rows[0]["FeeCategory"]);

                    

                        string vacate = d2.GetFunction("select BalAmount from FT_FeeAllot where LedgerFK ='" + Leadfk + "' and HeaderFK ='" + headfk + "'  and App_No ='" + appno + "' and MemType='1' and FeeCategory='" + feecat + "'");
                        if (vacate == "" || vacate == "0")
                        {
                            Hostelvat = "1";
                        }
                        else
                        {
                            Hostelvat = "2";

                        }

                    }
                    else
                    {
                        Hostelvat = "3";

                    }

                }
                else
                {
                    Hostelvat = "3";

                }


            }
            else
            {
                if (hostelset == "0")
                    Hostelvat = "0";
                else
                    Hostelvat = "4";


            }



        }
        catch
        {


        }
        return Hostelvat;

    }
}
