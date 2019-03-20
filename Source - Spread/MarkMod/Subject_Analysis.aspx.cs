using System;
using System.Collections;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Drawing;
using System.Configuration;

public partial class Subject_Analysis : System.Web.UI.Page
{
    string group_user = "", singleuser = "", usercode = "", collegecode = "";
    DAccess2 da = new DAccess2();
    DataSet ds = new DataSet();
    Hashtable hat = new Hashtable();
    int cout = 0;
    Dictionary<int, string> testname = new Dictionary<int, string>();
    Dictionary<int, string> teststaffname = new Dictionary<int, string>();
    Dictionary<int, string> headspancol = new Dictionary<int, string>();
    System.Text.StringBuilder textpass = new System.Text.StringBuilder();
    DataTable data = new DataTable();
    DataRow drow;


    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Visible = false;
        lblnorec.Visible = false;
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
        if (!IsPostBack)
        {
            bindbatch();
            binddegree();
            binddept();
            bindsem();
            bindsec();
            GetSubject();
            category();
            test();
            clear();
            TextBox1.Attributes.Add("readonly", "readonly");
            TextBox2.Attributes.Add("readonly", "readonly");
        }
    }

    public void bindbatch()
    {
        try
        {
            int count = 0;
            chcklistbatch.Items.Clear();
            chckbatch.Checked = false;
            txtbatch.Text = "--Select--";
            ds.Dispose();
            ds.Reset();
            ds = da.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                chcklistbatch.DataSource = ds;
                chcklistbatch.DataTextField = "Batch_year";
                chcklistbatch.DataValueField = "Batch_year";
                chcklistbatch.DataBind();
                if (chcklistbatch.Items.Count > 0)
                {
                    for (int i = 0; i < chcklistbatch.Items.Count; i++)
                    {
                        chcklistbatch.Items[i].Selected = true;
                        count++;
                    }
                    if (count > 0)
                    {
                        if (chcklistbatch.Items.Count == count)
                        {
                            chckbatch.Checked = true;
                        }
                        txtbatch.Text = "Batch  (" + (chcklistbatch.Items.Count) + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void bindsec()
    {
        try
        {
            clear();
            Cblsec.Items.Clear();
            string buildvalue = "";
            TextBox3.Text = "--Select--";
            TextBox3.Enabled = false;
            int takecount = 0;
            Cblsec.Items.Clear();

            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    if (buildvalue == "")
                    {
                        buildvalue = "'" + chcklistbatch.Items[i].Value.ToString() + "'";
                    }
                    else
                    {
                        buildvalue = buildvalue + "," + "'" + chcklistbatch.Items[i].Value.ToString() + "'";
                    }

                }
            }
            string strsection = "";
            if (buildvalue == "")
            {
                strsection = "select distinct sections from registration where degree_code in(" + degbranch.SelectedValue + ") and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            }
            else
            {
                strsection = "select distinct sections from registration where batch_year in(" + buildvalue + ") and degree_code in(" + degbranch.SelectedValue + ") and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar' order by sections";
            }
            ds = da.select_method_wo_parameter(strsection, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                takecount = ds.Tables[0].Rows.Count;
                Cblsec.DataSource = ds;
                Cblsec.DataTextField = "sections";
                Cblsec.DataBind();
                Cblsec.Items.Insert(takecount, "Empty");
                if (Convert.ToString(ds.Tables[0].Columns["sections"]) == string.Empty)
                {
                    TextBox3.Enabled = false;
                }
                else
                {
                    TextBox3.Enabled = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void binddegree()
    {
        try
        {
            ddldeg.Items.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Clear();
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_degree", hat, "sp");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddldeg.DataSource = ds;
                ddldeg.DataTextField = "course_name";
                ddldeg.DataValueField = "course_id";
                ddldeg.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void binddept()
    {
        try
        {
            degbranch.Items.Clear();
            hat.Clear();
            usercode = Session["usercode"].ToString();
            collegecode = Session["collegecode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (group_user.Contains(';'))
            {
                string[] group_semi = group_user.Split(';');
                group_user = group_semi[0].ToString();
            }
            hat.Add("single_user", singleuser);
            hat.Add("group_code", group_user);
            hat.Add("course_id", ddldeg.SelectedValue);
            hat.Add("college_code", collegecode);
            hat.Add("user_code", usercode);
            ds = da.select_method("bind_branch", hat, "sp");
            if (ds.Tables[0].Rows.Count > 0)
            {
                degbranch.DataSource = ds;
                degbranch.DataTextField = "dept_name";
                degbranch.DataValueField = "degree_code";
                degbranch.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void bindsem()
    {
        try
        {
            ddlsem.Items.Clear();
            ds.Clear();
            if (degbranch.SelectedValue != "" && txtbatch.Text != "--Select--")
            {
                ds = da.BindSem(degbranch.SelectedValue, chcklistbatch.SelectedValue, Session["collegecode"].ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 1; i <= Convert.ToInt32(ds.Tables[0].Rows[0][0]); i++)
                    {
                        ddlsem.Items.Add(Convert.ToString(i));
                    }
                    ddlsem.Enabled = true;
                }
                else
                {
                    ddlsem.Enabled = false;
                    ddlsem.SelectedItem.Text = "--Select--";
                }
            }
            else
            {
                ddlsem.Text = "--Select--";
                ddlsem.Enabled = false;

            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    public void GetSubject()
    {
        try
        {
            ddlsubj.Items.Clear();
            string buildvalue = "";
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    string build = chcklistbatch.Items[i].Value.ToString();
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
            string sections = Cblsec.SelectedValue.ToString();
            string sems = "";
            //if (ddlsem.SelectedValue != "")
            {
                if (ddlsem.SelectedValue == "")
                {
                    sems = "";
                }
                else
                {
                    sems = "and SM.semester=" + ddlsem.SelectedValue.ToString() + "";
                }
                string Sqlstr = "";
                string sk = "";
                if (buildvalue != "")
                {
                    sk = "select distinct subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + degbranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year in ('" + buildvalue + "')";
                }
                else
                {
                    sk = "select distinct subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + degbranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  ";
                }
                if (Session["Staff_Code"].ToString() == "")
                {
                    Sqlstr = "" + sk + " and  S.subtype_no = Sem.subtype_no and promote_count=1  order by subject_code ";
                }
                else if (Session["Staff_Code"].ToString() != "")
                {
                    Sqlstr = "" + sk + " and S.subtype_no = Sem.subtype_no and promote_count=1 and staff_code='" + Session["Staff_Code"].ToString() + "'  order by subject_code ";
                }
                ds = da.select_method_wo_parameter(Sqlstr, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    ddlsubj.Enabled = true;
                    ddlsubj.DataSource = ds;
                    ddlsubj.DataValueField = "subject_code";
                    ddlsubj.DataTextField = "Subject_Name";
                    ddlsubj.DataBind();
                    ddlsubj.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                }
                else
                {
                    ddlsubj.Enabled = false;
                    ddlsubj.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "-1"));
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    public void category()
    {
        try
        {
            CheckBoxList1.Visible = true;
            CheckBoxList1.Items.Clear();
            ds.Clear();
            int cnt = 0;
            string sk = "SELECT DISTINCT TEXTVAL,TextCode FROM TextValTable where TextCriteria = 'seat' and college_code = '" + Session["collegecode"] + "'";
            ds = da.select_method_wo_parameter(sk, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                CheckBoxList1.DataSource = ds.Tables[0];
                CheckBoxList1.DataTextField = "TEXTVAL";
                CheckBoxList1.DataValueField = "TextCode";
                CheckBoxList1.DataBind();
                if (CheckBoxList1.Items.Count > 0)
                {
                    for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                    {
                        CheckBoxList1.Items[i].Selected = true;
                        cnt++;
                    }
                    if (cnt > 0)
                    {
                        if (CheckBoxList1.Items.Count == cnt)
                        {
                            CheckBox1.Checked = true;
                        }
                        TextBox1.Text = "SeatType  (" + (CheckBoxList1.Items.Count) + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    public void test()
    {
        try
        {
            TextBox2.Text = "---Select---";
            TextBox2.Enabled = false;
            Cbltesttyp.Visible = true;
            Cbltesttyp.Items.Clear();
            ds.Clear();
            string section = "";
            if (section != "")
            {
                section = Cblsec.SelectedItem.Text;
            }
            string deg = degbranch.SelectedItem.Value;
            string deg1 = degbranch.SelectedItem.Text;
            string sk = "";
            int cnt = 0;
            Cbtesttyp.Checked = false;
            string buildvalue = "";
            string buildsec = "";
            string buildsec1 = "";
            Boolean emval = false;
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    string build = chcklistbatch.Items[i].Value.ToString();
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
            if (TextBox3.Text != "---Select---")
            {
                int itemcount = 0;
                for (itemcount = 0; itemcount < Cblsec.Items.Count; itemcount++)
                {
                    if (Cblsec.Items[itemcount].Selected == true)
                    {
                        if (Cblsec.Items[itemcount].Text.ToString() == "Empty Section")
                        {
                            if (buildsec1 == "")
                            {
                                buildsec1 = "''";
                            }
                            else
                            {
                                buildsec1 = buildsec1 + "," + "'',''";
                                emval = true;
                            }
                        }
                        else
                        {
                            if (buildsec1 == "")
                            {
                                buildsec1 = "'" + Cblsec.Items[itemcount].Value.ToString() + "'";
                            }
                            else
                            {
                                buildsec1 = buildsec1 + "," + "'" + Cblsec.Items[itemcount].Value.ToString() + "'";
                            }
                        }
                    }
                }
                if (buildsec1 != "")
                {
                    if (emval == false)
                    {
                        buildsec = " (" + buildsec1 + ")";
                    }
                    else
                    {
                        buildsec = " (" + buildsec1 + ") or e.sections is null";
                    }
                }
                else
                {
                    buildsec = " ";
                }
            }
            string sk1 = "";
            if (buildvalue != "")
            {
                if (ddlsubj.SelectedValue != "" || buildsec == "")
                {

                    sk1 = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,subject s,Registration r where c.syll_code=sy.syll_code and s.syll_code=c.syll_code and s.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and c.Criteria_no=e.criteria_no  and sy.degree_code='" + degbranch.SelectedItem.Value + "' and sy.semester='" + ddlsem.SelectedItem.Text + "'and s.subject_code='" + ddlsubj.SelectedValue + "'";

                }
                else
                {
                    if (ddlsem.SelectedValue != "")
                    {
                        sk1 = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,subject s,Registration r where c.syll_code=sy.syll_code and s.syll_code=c.syll_code and s.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and c.Criteria_no=e.criteria_no  and sy.degree_code='" + degbranch.SelectedItem.Value + "' and sy.semester='" + ddlsem.SelectedItem.Text + "'  ";
                    }
                }
            }
            else
            {
                if (buildvalue != "")
                {
                    if (ddlsubj.SelectedValue != "" || buildsec == "")
                    {
                        sk1 = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,subject s,Registration r where c.syll_code=sy.syll_code and s.syll_code=c.syll_code and s.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and c.Criteria_no=e.criteria_no and sy.batch_year in ('" + buildvalue + "') and sy.degree_code='" + degbranch.SelectedItem.Value + "' and sy.semester='" + ddlsem.SelectedItem.Text + "'and s.subject_no='" + ddlsubj.SelectedValue + "'";
                    }
                    else
                    {
                        if (ddlsem.SelectedValue != "")
                        {
                            sk1 = "select distinct c.criteria from CriteriaForInternal c,syllabus_master sy,Exam_type e,subject s,Registration r where c.syll_code=sy.syll_code and s.syll_code=c.syll_code and s.syll_code=sy.syll_code and c.criteria_no=e.criteria_no and c.Criteria_no=e.criteria_no  and sy.batch_year in ('" + buildvalue + "') and sy.degree_code='" + degbranch.SelectedItem.Value + "' and sy.semester='" + ddlsem.SelectedItem.Text + "'";
                        }
                    }
                }
            }
            if (TextBox3.Text == "--Select--")
            {
                sk = "" + sk1 + " ";
            }
            else
            {
                if (Chksec.Checked == false)
                {
                    sk = "" + sk1 + "";
                }
                else
                {
                    sk = "" + sk1 + " and r.sections in " + buildsec + "";
                }
            }
            if (buildvalue != "")
            {
                if (ddlsem.SelectedValue != "")
                {
                    ds = da.select_method_wo_parameter(sk, "Text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Cbltesttyp.DataSource = ds.Tables[0];
                        Cbltesttyp.DataTextField = "criteria";
                        Cbltesttyp.DataValueField = "criteria";
                        Cbltesttyp.DataBind();
                        TextBox2.Enabled = true;
                    }
                }
            }
            if (Cbltesttyp.Items.Count > 0)
            {
                for (int i = 0; i < Cbltesttyp.Items.Count; i++)
                {
                    Cbltesttyp.Items[i].Selected = true;
                    cnt++;
                }
                if (cnt > 0)
                {
                    if (Cbltesttyp.Items.Count == cnt)
                    {
                        Cbtesttyp.Checked = true;
                    }
                    TextBox2.Text = "Test Type(" + (Cbltesttyp.Items.Count) + ")";
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    public void clear()
    {
        Showgrid.Visible = false;
        Print.Visible = false;
        btnPrint.Visible = false;
        Label1.Visible = false;
        txtreptname.Visible = false;
        Excel.Visible = false;
        lblexcel.Visible = false;
    }

    protected void ddlbatselect(object sender, EventArgs e)
    {
        try
        {
            binddegree();
            clear();
            chkhost.Checked = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void checkBatch_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            chkhost.Checked = false;
            txtbatch.Text = "--Select--";
            if (chckbatch.Checked == true)
            {
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    chcklistbatch.Items[i].Selected = true;
                }
                txtbatch.Text = "Batch(" + (chcklistbatch.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < chcklistbatch.Items.Count; i++)
                {
                    chcklistbatch.Items[i].Selected = false;
                }
            }
            binddegree();
            binddept();
            bindsem();
            bindsec();
            GetSubject();
            category();
            test();

        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void cheklistBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int i = 0;
            txtbatch.Text = "--Select--";
            chckbatch.Checked = false;
            int commcount = 0;
            for (i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == chcklistbatch.Items.Count)
                {
                    chckbatch.Checked = true;
                }
                txtbatch.Text = "Batch(" + commcount.ToString() + ")";
            }
            binddegree();
            binddept();
            bindsem();
            bindsec();
            GetSubject();
            category();
            test();
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void ddldegselect(object sender, EventArgs e)
    {
        try
        {
            binddept();
            bindsem();
            GetSubject();
            test();
            clear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void degbranchselect(object sender, EventArgs e)
    {
        try
        {
            GetSubject();
            bindsec();
            test();
            bindsem();
            clear();
            chkhost.Checked = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void ddlsemselect(object sender, EventArgs e)
    {
        try
        {
            GetSubject();
            test();
            clear();
            chkhost.Checked = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void ddlsecselect(object sender, EventArgs e)
    {
        try
        {
            test();
            clear();
            chkhost.Checked = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void ddlsubject(object sender, EventArgs e)
    {
        try
        {
            test();
            clear();
            chkhost.Checked = false;
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void cbsubtyp_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            TextBox1.Text = "---Select---";
            chckbatch.Checked = false;
            int cout = 0;
            if (CheckBox1.Checked == true)
            {
                cout++;
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    CheckBoxList1.Items[i].Selected = true;
                }
                TextBox1.Text = "SeatType(" + (CheckBoxList1.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                {
                    CheckBoxList1.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void cblsubtyp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            clear();
            CheckBox1.Checked = false;
            TextBox1.Text = "---Select---";

            for (int i = 0; i < CheckBoxList1.Items.Count; i++)
            {
                if (CheckBoxList1.Items[i].Selected == true)
                {
                    cout = cout + 1;
                }
            }
            if (cout > 0)
            {
                TextBox1.Text = "SeatType(" + cout.ToString() + ")";
                if (cout == CheckBoxList1.Items.Count)
                {
                    CheckBox1.Checked = true;
                }
            }

        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void Cbtesttyp_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            TextBox2.Text = "---Select---";
            if (Cbtesttyp.Checked == true)
            {
                cout++;
                for (int i = 0; i < Cbltesttyp.Items.Count; i++)
                {
                    Cbltesttyp.Items[i].Selected = true;
                }
                TextBox2.Text = "TestType(" + (Cbltesttyp.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cbltesttyp.Items.Count; i++)
                {
                    Cbltesttyp.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void Cbltesttyp_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            int cout = 0;
            TextBox2.Text = "--Select--";
            for (int i = 0; i < Cbltesttyp.Items.Count; i++)
            {
                if (Cbltesttyp.Items[i].Selected == true)
                {
                    cout = cout + 1;
                    Cbtesttyp.Checked = false;
                }
            }
            if (cout > 0)
            {
                if (cout == Cbltesttyp.Items.Count)
                {
                    Cbtesttyp.Checked = true;
                }
                TextBox2.Text = "TestType(" + cout.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void chkhost1(object sender, EventArgs e)
    {
        try
        {
            clear();
        }
        catch (Exception ex)
        {
            lblnorec.Visible = false;
            lblnorec.Text = ex.ToString();
        }
    }

    protected void Chksec_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            TextBox3.Text = "--Select--";
            clear();
            if (Chksec.Checked == true)
            {
                for (int i = 0; i < Cblsec.Items.Count; i++)
                {
                    Cblsec.Items[i].Selected = true;
                }
                TextBox3.Text = "Section(" + (Cblsec.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < Cblsec.Items.Count; i++)
                {
                    Cblsec.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void Cblsec_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            test();
            int i = 0;
            Chksec.Checked = false;
            int commcount = 0;
            TextBox3.Text = "--Select--";
            for (i = 0; i < Cblsec.Items.Count; i++)
            {
                if (Cblsec.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    Chksec.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == Cblsec.Items.Count)
                {
                    Chksec.Checked = true;
                }
                TextBox3.Text = "Section(" + commcount.ToString() + ")";
            }
        }
        catch (Exception ex)
        {
            lblnorec.Text = ex.ToString();
            lblnorec.Visible = true;
        }
    }

    protected void gobtn_Click(object sender, EventArgs e)
    {
        try
        {
            btnPrint11();
            testname.Clear();
            teststaffname.Clear();
            headspancol.Clear();
            ArrayList avoidcol = new ArrayList();
            avoidcol.Clear();
            if (txtbatch.Text == "--Select--")
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Batch\");", true);
            }
            else if (TextBox3.Text == "--Select--" && TextBox3.Enabled == true)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Section\");", true);
            }
            else if (ddlsubj.SelectedIndex == 0)
            {
                ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Subject\");", true);
                clear();
            }
            else
            {
                if (TextBox1.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Seat Type\");", true);
                }
                else if (TextBox2.Text == "---Select---")
                {
                    ScriptManager.RegisterStartupScript(base.Page, this.GetType(), ("dialogJavascript" + this.ID), "alert(\"Please Select Test Type\");", true);
                }
                else
                {
                    lblnorec.Visible = false;
                    Boolean reportflag = false;

                    int check = 0, count = 0, lm = 1;
                    string dept2value = "", seattype = "";
                    if (CheckBoxList1.Items.Count > 0)
                    {
                        for (int i = 0; i < CheckBoxList1.Items.Count; i++)
                        {
                            if (CheckBoxList1.Items[i].Selected == true)
                            {
                                check++;
                                string value1 = CheckBoxList1.Items[i].Value;
                                if (seattype == "")
                                {
                                    seattype = value1;
                                }
                                else
                                {
                                    seattype = seattype + "'" + "," + "'" + value1;
                                }
                            }
                        }
                    }
                    if (Cbltesttyp.Items.Count > 0)
                    {
                        for (int i = 0; i < Cbltesttyp.Items.Count; i++)
                        {
                            if (Cbltesttyp.Items[i].Selected == true)
                            {
                                count++;
                                string value = Cbltesttyp.Items[i].Text;
                                if (dept2value == "")
                                {
                                    dept2value = value;
                                }
                                else
                                {
                                    dept2value = dept2value + "'" + "," + "'" + value;
                                }
                            }
                        }
                    }
                    string buildvalue = "", buildsec = "", buildsec1 = "";
                    Boolean emval = false;
                    for (int i = 0; i < chcklistbatch.Items.Count; i++)
                    {
                        if (chcklistbatch.Items[i].Selected == true)
                        {
                            hat.Add(chcklistbatch.Items[i].Text, chcklistbatch.Items[i].Text);
                            string build = chcklistbatch.Items[i].Value.ToString();
                            if (buildvalue == "")
                            {
                                buildvalue = "'" + build + "'";
                            }
                            else
                            {
                                buildvalue = buildvalue + ",'" + build + "'";
                            }
                        }
                    }
                    if (TextBox3.Text != "---Select---")
                    {
                        int itemcount = 0;
                        for (itemcount = 0; itemcount < Cblsec.Items.Count; itemcount++)
                        {
                            if (Cblsec.Items[itemcount].Selected == true)
                            {
                                if (Cblsec.Items[itemcount].Text.ToString() == "Empty")
                                {
                                    if (buildsec1 == "")
                                    {
                                        buildsec1 = "''";
                                    }
                                    else
                                    {
                                        buildsec1 = buildsec1 + "," + "'',''";

                                        emval = true;
                                    }
                                }
                                else
                                {
                                    if (buildsec1 == "")
                                    {
                                        buildsec1 = "'" + Cblsec.Items[itemcount].Value.ToString() + "'";
                                    }
                                    else
                                    {
                                        buildsec1 = buildsec1 + "," + "'" + Cblsec.Items[itemcount].Value.ToString() + "'";
                                    }
                                }
                            }
                        }
                        if (buildsec1 != "")
                        {
                            if (emval == false)
                            {
                                buildsec = " (" + buildsec1 + ")";
                            }
                            else
                            {
                                buildsec = " (" + buildsec1 + ")";
                            }
                        }
                        else
                        {
                            buildsec = " ";
                        }
                    }
                    ArrayList arrColHdrNames1 = new ArrayList();
                    ArrayList arrColHdrNames2 = new ArrayList();
                    arrColHdrNames1.Add("Batch");
                    arrColHdrNames2.Add("Batch");
                    data.Columns.Add("Batch", typeof(string));
                    int testcnt = 1;
                    if (Cbltesttyp.Items.Count > 0)
                    {
                        for (int i = 0; i < Cbltesttyp.Items.Count; i++)
                        {
                            if (Cbltesttyp.Items[i].Selected == true)
                            {
                                reportflag = true;
                                string valuess = Cbltesttyp.Items[i].Text;
                                string textcode = Cbltesttyp.Items[i].Value;

                                arrColHdrNames1.Add(valuess);
                                arrColHdrNames2.Add("Appear");
                                arrColHdrNames1.Add(valuess);
                                arrColHdrNames2.Add("Pass Count");
                                arrColHdrNames1.Add(valuess);
                                arrColHdrNames2.Add("Fail Count");

                                testname.Add(testcnt, valuess.ToString());
                                testcnt = testcnt + 3;


                                textpass = new System.Text.StringBuilder("Appear");

                                AddTableColumn(data, textpass);

                                textpass = new System.Text.StringBuilder("Pass Count");

                                AddTableColumn(data, textpass);

                                textpass = new System.Text.StringBuilder("Fail Count");

                                AddTableColumn(data, textpass);


                                //lm++;
                                lm = lm + 3;
                            }
                        }
                    }
                    data.Columns.Add("STAFF NAME", typeof(string));
                    arrColHdrNames1.Add("STAFF NAME");
                    arrColHdrNames2.Add("STAFF NAME");
                    DataRow drHdr1 = data.NewRow();
                    DataRow drHdr2 = data.NewRow();
                    for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    {
                        drHdr1[grCol] = arrColHdrNames1[grCol];
                        drHdr2[grCol] = arrColHdrNames2[grCol];

                    }

                    data.Rows.Add(drHdr1);
                    data.Rows.Add(drHdr2);
                    string querystaf = "", name = "", staff = "", query = "", query1 = "";
                    Hashtable ht = new Hashtable();
                    Hashtable htc = new Hashtable();
                    Hashtable htblr = new Hashtable();
                    DataView dv1 = new DataView();
                    DataView dv2 = new DataView();
                    DataSet ds1 = new DataSet();
                    DataSet dset1 = new DataSet();
                    DataSet dsfail = new DataSet();
                    DataView dv11 = new DataView();
                    DataView dv21 = new DataView();
                    int cnt = 0, rr = 1;
                    string queryfail = "";
                    string sk = "select count(r.roll_no) as strength,reg.Stud_Type,reg.Batch_Year,reg.Sections,c.criteria,c.Criteria_no,t.TextVal,t.TextCode,reg.Stud_Type from CriteriaForInternal c,syllabus_master sy,Exam_type e,Result r,Registration reg,applyn a,textvaltable t,subject s where c.syll_code=sy.syll_code and sy.syll_code=s.syll_code and e.subject_no=s.subject_no and reg.App_No=a.app_no and a.seattype=t.TextCode and c.Criteria_no=e.criteria_no and r.exam_code=e.exam_code and r.roll_no=reg.Roll_No and reg.cc=0 and reg.DelFlag=0 and reg.Exam_Flag<>'debar' and c.Criteria in ('" + dept2value + "') and sy.Batch_Year=reg.Batch_Year and reg.degree_code=sy.degree_code and s.subject_code = '" + ddlsubj.SelectedValue + "' and sy.batch_year in (" + buildvalue + ") and sy.degree_code = '" + degbranch.SelectedItem.Value + "' and sy.semester = '" + ddlsem.SelectedItem.Text + "' ";
                    queryfail = "select count(r.roll_no) as strength,reg.Stud_Type,reg.Batch_Year,reg.Sections,c.criteria,c.Criteria_no,t.TextVal,t.TextCode,reg.Stud_Type from CriteriaForInternal c,syllabus_master sy,Exam_type e,Result r,Registration reg,applyn a,textvaltable t,subject s where c.syll_code=sy.syll_code and sy.syll_code=s.syll_code and e.subject_no=s.subject_no and reg.App_No=a.app_no and a.seattype=t.TextCode and c.Criteria_no=e.criteria_no and r.exam_code=e.exam_code and r.roll_no=reg.Roll_No and reg.cc=0 and reg.DelFlag=0 and reg.Exam_Flag<>'debar' and c.Criteria in ('" + dept2value + "') and sy.Batch_Year=reg.Batch_Year and reg.degree_code=sy.degree_code and s.subject_code = '" + ddlsubj.SelectedValue + "' and sy.batch_year in (" + buildvalue + ") and sy.degree_code = '" + degbranch.SelectedItem.Value + "' and sy.semester = '" + ddlsem.SelectedItem.Text + "' ";
                    if (TextBox3.Enabled == false)
                    {
                        query = "" + sk + " and (r.marks_obtained>=e.min_mark or r.marks_obtained=-3)and reg.Stud_Type='Day Scholar' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                        queryfail = sk + " and (r.marks_obtained<e.min_mark and r.marks_obtained<>-3)and reg.Stud_Type='Day Scholar' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                    }
                    else
                    {
                        query = "" + sk + " and reg.Sections in " + buildsec + " and (r.marks_obtained>=e.min_mark or r.marks_obtained=-3)and reg.Stud_Type='Day Scholar' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                        queryfail = sk + " and reg.Sections in " + buildsec + " and (r.marks_obtained<e.min_mark and r.marks_obtained<>-3)and reg.Stud_Type='Day Scholar' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                    }
                    DataSet dset = da.select_method_wo_parameter(query, "text");
                    if (chkhost.Checked == true)
                    {
                        if (TextBox3.Enabled == false)
                        {
                            query1 = "" + sk + " and (r.marks_obtained>=e.min_mark or r.marks_obtained=-3)and reg.Stud_Type='Hostler' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode ";
                            // queryfail = sk + " and (r.marks_obtained<e.min_mark and r.marks_obtained<>-3)and reg.Stud_Type='Hostler' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode ";
                        }
                        else
                        {
                            query1 = "" + sk + " and reg.Sections in " + buildsec + " and (r.marks_obtained>=e.min_mark or r.marks_obtained=-3)and reg.Stud_Type='Hostler' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                            //queryfail = sk + " and reg.Sections in " + buildsec + " and (r.marks_obtained<e.min_mark and r.marks_obtained<>-3)and reg.Stud_Type='Hostler' and a.seattype in ('" + seattype + "') group by reg.Batch_Year,reg.Sections,reg.Stud_Type,c.criteria,c.Criteria_no ,reg.Stud_Type,t.TextVal,t.TextCode , reg.Stud_Type order by c.criteria,reg.Stud_Type,t.TextVal,t.TextCode,reg.Sections";
                        }
                        dset1 = da.select_method_wo_parameter(query1, "text");
                    }
                    dsfail = da.select_method_wo_parameter(queryfail, "text");

                    string subjectquery = "select distinct sm.staff_name,sm.staff_code,sy.Batch_Year,sy.degree_code,sy.semester,st.Sections,s.subject_code from syllabus_master sy,sub_sem ss,subject s,staff_selector st,staffmaster sm where sy.syll_code=ss.syll_code and sy.syll_code=s.syll_code and ss.syll_code=s.syll_code and ss.subType_no=s.subType_no and s.subject_no=st.subject_no and st.staff_code=sm.staff_code and st.batch_year=sy.Batch_Year and sy.Batch_Year in(" + buildvalue + ") and sy.degree_code = '" + degbranch.SelectedItem.Value + "' and sy.semester = '" + ddlsem.SelectedItem.Text + "'";
                    DataSet dssubject = da.select_method_wo_parameter(subjectquery, "text");

                    Hashtable hasname = new Hashtable();
                    Hashtable htbl1 = new Hashtable();
                    Hashtable hasname1 = new Hashtable();
                    Hashtable hasname2 = new Hashtable();

                    int cnt11 = 0, cntd = 0, cntd1 = 0;
                    if (dset.Tables[0].Rows.Count > 0)
                    {

                        Print.Visible = true;
                        btnPrint.Visible = true;
                        txtreptname.Visible = true;
                        Excel.Visible = true;
                        lblexcel.Visible = true;
                        lblnorec.Visible = false;
                        Label1.Visible = false;
                        drow = data.NewRow();
                        drow["Batch"] = "DAYS SCHOLER STUDENTS";
                        data.Rows.Add(drow);
                        headspancol.Add(data.Rows.Count - 1, "blue");
                        if (TextBox3.Enabled == true)
                        {
                            for (int i = 0; i < chcklistbatch.Items.Count; i++)
                            {
                                if (chcklistbatch.Items[i].Selected == true)
                                {
                                    for (int ik = 0; ik < CheckBoxList1.Items.Count; ik++)
                                    {
                                        if (CheckBoxList1.Items[ik].Selected == true)
                                        {
                                            for (int j = 0; j < Cblsec.Items.Count; j++)
                                            {
                                                if (Cblsec.Items[j].Selected == true)
                                                {
                                                    dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "'";
                                                    dv1 = dset.Tables[0].DefaultView;
                                                    if (dv1.Count > 0)
                                                    {
                                                        for (int jk = 0; jk < dv1.Count; jk++)
                                                        {

                                                            if (!htbl1.ContainsKey(dv1[jk]["TextCode"].ToString()))
                                                            {
                                                                htbl1.Add(dv1[jk]["TextCode"].ToString(), cnt11);
                                                                cnt11++;
                                                                drow = data.NewRow();
                                                                drow["Batch"] = Convert.ToString(CheckBoxList1.Items[ik].Text) + "   Quota Students";
                                                                data.Rows.Add(drow);
                                                                headspancol.Add(data.Rows.Count - 1, "red");
                                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(CheckBoxList1.Items[ik].Text);

                                                                avoidcol.Add(data.Rows.Count - 1);


                                                            }
                                                            if (TextBox3.Enabled == false)
                                                            {
                                                                drow = data.NewRow();
                                                                drow["Batch"] = Convert.ToString(chcklistbatch.Items[i].Text);
                                                                data.Rows.Add(drow);

                                                            }
                                                            else
                                                            {

                                                                drow = data.NewRow();
                                                                data.Rows.Add(drow);

                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = "";
                                                                if (TextBox3.Enabled == false && buildsec == "")
                                                                {
                                                                    data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text);

                                                                }
                                                                else
                                                                {
                                                                    if (Cblsec.Items[j].Value == "Empty")
                                                                    {
                                                                        data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text) + "   SEC  ";

                                                                    }
                                                                    else
                                                                    {
                                                                        data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text) + "   SEC  " + Cblsec.Items[j].Value;

                                                                    }

                                                                }
                                                            }


                                                            //// ---------------- staff name start
                                                            //if (Cblsec.Items[j].Value == "Empty")
                                                            //{
                                                            //    querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.Sections in ('') and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                            //}
                                                            //else
                                                            //{
                                                            //    querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.Sections in ('" + Cblsec.Items[j].Value + "') and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                            //}
                                                            //ds1 = da.select_method_wo_parameter(querystaf, "text");
                                                            // ---------------- staff name end
                                                            int col = 0;
                                                            string buildvalue1 = "";
                                                            //for (int ij = 0; ij < Cbltesttyp.Items.Count; ij++)
                                                            for (int ij = 1; ij < data.Columns.Count - 1; ij = ij + 3)
                                                            {
                                                                //if (Cbltesttyp.Items[ij].Selected == true)
                                                                {
                                                                    if (Cblsec.Items[j].Value == "Empty")
                                                                    {
                                                                        //dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and Sections='' and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria_no='" + Cbltesttyp.Items[ij].Value + "'";
                                                                        dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and Sections='' and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria='" + testname[ij] + "'";
                                                                    }
                                                                    else
                                                                    {
                                                                        //dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + "and Sections='" + Cblsec.Items[j].Value + "' and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria_no='" + Cbltesttyp.Items[ij].Value + "'";
                                                                        dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + "and Sections='" + Cblsec.Items[j].Value + "' and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria='" + testname[ij] + "'";
                                                                    }

                                                                    dv2 = dset.Tables[0].DefaultView;
                                                                    if (dv2.Count > 0)
                                                                    {
                                                                        col++;
                                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dv2[0]["strength"]);
                                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;
                                                                        string failva = "0";
                                                                        dsfail.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                                        DataView dvfail = dsfail.Tables[0].DefaultView;
                                                                        if (dvfail.Count > 0)
                                                                        {
                                                                            failva = dvfail[0]["strength"].ToString();
                                                                        }

                                                                        data.Rows[data.Rows.Count - 1][ij + 2] = failva;
                                                                        data.Rows[data.Rows.Count - 1][ij + 1] = Convert.ToString(dv2[0]["strength"]);
                                                                        int totcount = Convert.ToInt32(failva) + Convert.ToInt32(dv2[0]["strength"]);
                                                                        data.Rows[data.Rows.Count - 1][ij] = totcount.ToString();


                                                                        col++;

                                                                        // ---------------- staff name start
                                                                        name = "";
                                                                        string sectval = "";
                                                                        if (Cblsec.Items[j].Value != "Empty")
                                                                        {
                                                                            sectval = " and Sections='" + Cblsec.Items[j].Value.ToString() + "'";
                                                                        }
                                                                        dssubject.Tables[0].DefaultView.RowFilter = "subject_code='" + ddlsubj.SelectedValue + "' and batch_year='" + chcklistbatch.Items[i].Text + "' and semester='" + ddlsem.SelectedItem.ToString() + "' " + sectval + "";
                                                                        DataView dvstaff = dssubject.Tables[0].DefaultView;
                                                                        for (int s = 0; s < dvstaff.Count; s++)
                                                                        {
                                                                            if (name == "")
                                                                            {
                                                                                name = dvstaff[s]["staff_name"].ToString();
                                                                            }
                                                                            else
                                                                            {
                                                                                name = name + ", " + dvstaff[s]["staff_name"].ToString();
                                                                            }

                                                                        }
                                                                        //if (ds1.Tables[0].Rows.Count > 0)
                                                                        //{
                                                                        //    staff = ds1.Tables[0].Rows[0]["staff_code"].ToString();

                                                                        //    for (int isasd = 0; isasd < ds1.Tables[0].Rows.Count; isasd++)
                                                                        //    {
                                                                        //        string build1 = ds1.Tables[0].Rows[isasd]["staff_code"].ToString();
                                                                        //        if (buildvalue1 == "")
                                                                        //        {
                                                                        //            buildvalue1 = build1;
                                                                        //        }
                                                                        //        else
                                                                        //        {
                                                                        //            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                                                                        //        }
                                                                        //    }
                                                                        //    string skr = "select staff_name from staffmaster where  staff_code in('" + buildvalue1 + "')";
                                                                        //    DataSet ds2 = da.select_method_wo_parameter(skr, "text");
                                                                        //    if (ds2.Tables[0].Rows.Count > 0)
                                                                        //    {
                                                                        //        hasname.Clear();
                                                                        //        name = "";
                                                                        //        for (int kh = 0; kh < ds2.Tables[0].Rows.Count; kh++)
                                                                        //        {

                                                                        //            if (!hasname.ContainsKey(ds2.Tables[0].Rows[kh]["staff_name"].ToString()))
                                                                        //            {
                                                                        //                hasname.Add(ds2.Tables[0].Rows[kh]["staff_name"].ToString(), cnt);
                                                                        //                cnt++;
                                                                        //                if (Convert.ToString(name) == "")
                                                                        //                {
                                                                        //                    name = ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                        //                }
                                                                        //                else
                                                                        //                {
                                                                        //                    name = name + "," + ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                        //                }
                                                                        //            }
                                                                        //        }
                                                                        //    }
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    name = "-";
                                                                        //}
                                                                        if (rr == 1)
                                                                        {
                                                                            teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));

                                                                            rr++;
                                                                        }
                                                                        else
                                                                        {
                                                                            //data.Rows[data.Rows.Count - 1][data.Columns.Count - 1] = Convert.ToString(name);
                                                                            if (teststaffname.ContainsKey(data.Rows.Count - 1))
                                                                            {
                                                                                teststaffname.Remove(data.Rows.Count - 1);
                                                                                teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                            }
                                                                            else
                                                                                teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                            //FpSpread1.Sheets[0].SetColumnMerge(FpSpread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                                        }
                                                                        // ---------------- staff name end
                                                                    }
                                                                    else
                                                                    {
                                                                        col++;

                                                                        //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString("-");


                                                                    }
                                                                }
                                                            }

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < chcklistbatch.Items.Count; i++)
                            {
                                if (chcklistbatch.Items[i].Selected == true)
                                {
                                    for (int ik = 0; ik < CheckBoxList1.Items.Count; ik++)
                                    {
                                        if (CheckBoxList1.Items[ik].Selected == true)
                                        {
                                            dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "'";
                                            dv1 = dset.Tables[0].DefaultView;
                                            if (dv1.Count > 0)
                                            {
                                                cnt11++;
                                                drow = data.NewRow();
                                                drow["Batch"] = Convert.ToString(CheckBoxList1.Items[ik].Text) + "   Quota Students";
                                                data.Rows.Add(drow);
                                                headspancol.Add(data.Rows.Count - 1, "red");
                                                //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(CheckBoxList1.Items[ik].Text);

                                                avoidcol.Add(data.Rows.Count - 1);
                                                drow = data.NewRow();
                                                drow["Batch"] = Convert.ToString(chcklistbatch.Items[i].Text);
                                                data.Rows.Add(drow);

                                                // ---------------- staff name start
                                                //querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                //ds1 = da.select_method_wo_parameter(querystaf, "text");
                                                // ---------------- staff name end
                                                int col = 0;
                                                string buildvalue1 = "";
                                                //for (int ij = 0; ij < Cbltesttyp.Items.Count; ij++)
                                                for (int ij = 1; ij < data.Columns.Count - 1; ij = ij + 3)
                                                {
                                                    //if (Cbltesttyp.Items[ij].Selected == true)
                                                    {
                                                        //dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria_no='" + Cbltesttyp.Items[ij].Value + "'";
                                                        dset.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                        dv2 = dset.Tables[0].DefaultView;
                                                        if (dv2.Count > 0)
                                                        {
                                                            string failva = "0";
                                                            dsfail.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                            DataView dvfail = dsfail.Tables[0].DefaultView;
                                                            if (dvfail.Count > 0)
                                                            {
                                                                failva = dvfail[0]["strength"].ToString();
                                                            }

                                                            data.Rows[data.Rows.Count - 1][ij + 2] = failva;

                                                            col++;
                                                            data.Rows[data.Rows.Count - 1][ij + 1] = Convert.ToString(dv2[0]["strength"]);


                                                            int totcount = Convert.ToInt32(failva) + Convert.ToInt32(dv2[0]["strength"]);
                                                            data.Rows[data.Rows.Count - 1][ij] = totcount.ToString();




                                                            // ---------------- staff name start
                                                            name = "";
                                                            dssubject.Tables[0].DefaultView.RowFilter = "subject_code='" + ddlsubj.SelectedValue + "' and batch_year='" + chcklistbatch.Items[i].Text + "' and semester='" + ddlsem.SelectedItem.ToString() + "'";
                                                            DataView dvstaff = dssubject.Tables[0].DefaultView;
                                                            for (int s = 0; s < dvstaff.Count; s++)
                                                            {
                                                                if (name == "")
                                                                {
                                                                    name = dvstaff[s]["staff_name"].ToString();
                                                                }
                                                                else
                                                                {
                                                                    name = name + ", " + dvstaff[s]["staff_name"].ToString();
                                                                }

                                                            }
                                                            //if (ds1.Tables[0].Rows.Count > 0)
                                                            //{
                                                            //    staff = ds1.Tables[0].Rows[0]["staff_code"].ToString();

                                                            //    for (int isasd = 0; isasd < ds1.Tables[0].Rows.Count; isasd++)
                                                            //    {
                                                            //        string build1 = ds1.Tables[0].Rows[isasd]["staff_code"].ToString();
                                                            //        if (buildvalue1 == "")
                                                            //        {
                                                            //            buildvalue1 = build1;
                                                            //        }
                                                            //        else
                                                            //        {
                                                            //            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                                                            //        }
                                                            //    }
                                                            //    string skr = "select staff_name from staffmaster where  staff_code in('" + buildvalue1 + "')";
                                                            //    DataSet ds2 = da.select_method_wo_parameter(skr, "text");
                                                            //    if (ds2.Tables[0].Rows.Count > 0)
                                                            //    {
                                                            //        for (int kh = 0; kh < ds2.Tables[0].Rows.Count; kh++)
                                                            //        {
                                                            //            if (!hasname.ContainsKey(ds2.Tables[0].Rows[kh]["staff_name"].ToString()))
                                                            //            {
                                                            //                hasname.Add(ds2.Tables[0].Rows[kh]["staff_name"].ToString(), cnt);
                                                            //                cnt++;
                                                            //                if (Convert.ToString(name) == "")
                                                            //                {
                                                            //                    name = ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                            //                }
                                                            //                else
                                                            //                {
                                                            //                    name = name + "," + ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                            //                }
                                                            //            }
                                                            //        }
                                                            //    }
                                                            //}
                                                            //else
                                                            //{
                                                            //    name = "-";
                                                            //}
                                                            if (rr == 1)
                                                            {

                                                                teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));



                                                                rr++;
                                                            }
                                                            else
                                                            {
                                                                if (teststaffname.ContainsKey(data.Rows.Count - 1))
                                                                {
                                                                    teststaffname.Remove(data.Rows.Count - 1);
                                                                    teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                }
                                                                else
                                                                    teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));


                                                                // FpSpread1.Sheets[0].SetColumnMerge(FpSpread1.Sheets[0].ColumnCount - 1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                                                            }
                                                            // ---------------- staff name end
                                                        }
                                                        else
                                                        {
                                                            col++;

                                                            //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].Text = Convert.ToString("-");
                                                            // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Hashtable htbl = new Hashtable();
                        int cnt1 = 0;
                        // ----------------------- for hostler start
                        if (chkhost.Checked == true)
                        {
                            drow = data.NewRow();
                            drow["Batch"] = "HOSTEL STUDENTS";
                            data.Rows.Add(drow);
                            headspancol.Add(data.Rows.Count - 1, "blue");


                            if (TextBox3.Enabled == true)
                            {
                                if (dset1.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < chcklistbatch.Items.Count; i++)
                                    {
                                        if (chcklistbatch.Items[i].Selected == true)
                                        {
                                            for (int ik = 0; ik < CheckBoxList1.Items.Count; ik++)
                                            {
                                                if (CheckBoxList1.Items[ik].Selected == true)
                                                {
                                                    for (int j = 0; j < Cblsec.Items.Count; j++)
                                                    {
                                                        if (Cblsec.Items[j].Selected == true)
                                                        {
                                                            dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "'";
                                                            dv11 = dset1.Tables[0].DefaultView;
                                                            if (dv11.Count > 0)
                                                            {
                                                                for (int jk = 0; jk < dv11.Count; jk++)
                                                                {
                                                                    if (!htbl.ContainsKey(dv11[jk]["TextCode"].ToString()))
                                                                    {
                                                                        htbl.Add(dv11[jk]["TextCode"].ToString(), cnt);
                                                                        cnt1++;
                                                                        drow = data.NewRow();
                                                                        drow["Batch"] = Convert.ToString(CheckBoxList1.Items[ik].Text) + "   Quota Students";
                                                                        data.Rows.Add(drow);
                                                                        headspancol.Add(data.Rows.Count - 1, "red");
                                                                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(CheckBoxList1.Items[ik].Text);

                                                                        avoidcol.Add(data.Rows.Count - 1);


                                                                    }
                                                                    if (TextBox3.Enabled == false)
                                                                    {
                                                                        drow = data.NewRow();
                                                                        drow["Batch"] = Convert.ToString(chcklistbatch.Items[i].Text);
                                                                        data.Rows.Add(drow);


                                                                    }
                                                                    else
                                                                    {
                                                                        drow = data.NewRow();

                                                                        data.Rows.Add(drow);


                                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = "";
                                                                        if (TextBox3.Enabled == false && buildsec == "")
                                                                        {
                                                                            data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text);

                                                                        }
                                                                        else
                                                                        {
                                                                            if (Cblsec.Items[j].Value == "Empty")
                                                                            {
                                                                                data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text) + "   SEC  ";

                                                                            }
                                                                            else
                                                                            {
                                                                                data.Rows[data.Rows.Count - 1][0] = Convert.ToString(chcklistbatch.Items[i].Text) + "   SEC  " + Cblsec.Items[j].Value;

                                                                            }

                                                                        }
                                                                    }
                                                                    // ---------------- staff name start
                                                                    //if (Cblsec.Items[j].Value == "Empty")
                                                                    //{
                                                                    //    querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.Sections in ('') and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.Sections in ('" + Cblsec.Items[j].Value + "') and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                                    //}
                                                                    //ds1 = da.select_method_wo_parameter(querystaf, "text");
                                                                    // ---------------- staff name end
                                                                    int col = 0;
                                                                    string buildvalue1 = "";
                                                                    //  for (int ij = 0; ij < Cbltesttyp.Items.Count; ij++)
                                                                    for (int ij = 1; ij < data.Columns.Count - 1; ij = ij + 3)
                                                                    {
                                                                        // if (Cbltesttyp.Items[ij].Selected == true)
                                                                        {

                                                                            if (Cblsec.Items[j].Value == "Empty")
                                                                            {
                                                                                dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + "and Sections='' and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                                            }
                                                                            else
                                                                            {
                                                                                dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + "and Sections='" + Cblsec.Items[j].Value + "'  and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                                            }

                                                                            dv21 = dset1.Tables[0].DefaultView;
                                                                            if (dv21.Count > 0)
                                                                            {
                                                                                col++;
                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ij+2].Text = Convert.ToString(dv21[0]["strength"]);
                                                                                // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, ij + 2].HorizontalAlign = HorizontalAlign.Center;
                                                                                string failva = "0";
                                                                                dsfail.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                                                DataView dvfail = dsfail.Tables[0].DefaultView;
                                                                                if (dvfail.Count > 0)
                                                                                {
                                                                                    failva = dvfail[0]["strength"].ToString();
                                                                                }
                                                                                data.Rows[data.Rows.Count - 1][ij + 2] = failva;

                                                                                col++;
                                                                                data.Rows[data.Rows.Count - 1][ij + 1] = Convert.ToString(dv21[0]["strength"]);


                                                                                int totcount = Convert.ToInt32(failva) + Convert.ToInt32(dv21[0]["strength"]);
                                                                                data.Rows[data.Rows.Count - 1][ij] = totcount.ToString();


                                                                                // ---------------- staff name start
                                                                                name = "";
                                                                                string sectval = "";
                                                                                if (Cblsec.Items[j].Value != "Empty")
                                                                                {
                                                                                    sectval = " and Sections='" + Cblsec.Items[j].Value.ToString() + "'";
                                                                                }
                                                                                dssubject.Tables[0].DefaultView.RowFilter = "subject_code='" + ddlsubj.SelectedValue + "' and batch_year='" + chcklistbatch.Items[i].Text + "' and semester='" + ddlsem.SelectedItem.ToString() + "' " + sectval + "";
                                                                                DataView dvstaff = dssubject.Tables[0].DefaultView;
                                                                                for (int s = 0; s < dvstaff.Count; s++)
                                                                                {
                                                                                    if (name == "")
                                                                                    {
                                                                                        name = dvstaff[s]["staff_name"].ToString();
                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        name = name + ", " + dvstaff[s]["staff_name"].ToString();
                                                                                    }

                                                                                }
                                                                                if (rr == 1)
                                                                                {

                                                                                    teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));

                                                                                    rr++;
                                                                                }
                                                                                else
                                                                                {
                                                                                    if (teststaffname.ContainsKey(data.Rows.Count - 1))
                                                                                    {
                                                                                        teststaffname.Remove(data.Rows.Count - 1);
                                                                                        teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                                    }
                                                                                    else
                                                                                        teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                                }
                                                                                //if (ds1.Tables[0].Rows.Count > 0)
                                                                                //{
                                                                                //    staff = ds1.Tables[0].Rows[0]["staff_code"].ToString();

                                                                                //    for (int isasd = 0; isasd < ds1.Tables[0].Rows.Count; isasd++)
                                                                                //    {
                                                                                //        string build1 = ds1.Tables[0].Rows[isasd]["staff_code"].ToString();
                                                                                //        if (buildvalue1 == "")
                                                                                //        {
                                                                                //            buildvalue1 = build1;
                                                                                //        }
                                                                                //        else
                                                                                //        {
                                                                                //            buildvalue1 = buildvalue1 + "'" + "," + "'" + build1;
                                                                                //        }
                                                                                //    }
                                                                                //    string skr = "select staff_name from staffmaster where  staff_code in('" + buildvalue1 + "')";
                                                                                //    DataSet ds2 = da.select_method_wo_parameter(skr, "text");
                                                                                //    if (ds2.Tables[0].Rows.Count > 0)
                                                                                //    {
                                                                                //        hasname.Clear();
                                                                                //        name = "";
                                                                                //        for (int kh = 0; kh < ds2.Tables[0].Rows.Count; kh++)
                                                                                //        {
                                                                                //            if (!hasname.ContainsKey(ds2.Tables[0].Rows[kh]["staff_name"].ToString()))
                                                                                //            {
                                                                                //                hasname.Add(ds2.Tables[0].Rows[kh]["staff_name"].ToString(), cnt);
                                                                                //                cnt++;
                                                                                //                if (Convert.ToString(name) == "")
                                                                                //                {
                                                                                //                    name = ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                                //                }
                                                                                //                else
                                                                                //                {
                                                                                //                    name = name + "," + ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                                //                }
                                                                                //            }
                                                                                //        }
                                                                                //    }
                                                                                //    if (rr == 1)
                                                                                //    {
                                                                                //        FpSpread1.Sheets[0].ColumnCount++;
                                                                                //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "STAFF NAME";
                                                                                //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                                                                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(name);
                                                                                //        rr++;
                                                                                //    }
                                                                                //    else
                                                                                //    {
                                                                                //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(name);
                                                                                //    }
                                                                                //    // ---------------- staff name end
                                                                                //}
                                                                            }
                                                                            else
                                                                            {
                                                                                col++;
                                                                                // data.Columns.Add("STAFF NAME", typeof(string));
                                                                            }
                                                                        }
                                                                    }

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (dset1.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < chcklistbatch.Items.Count; i++)
                                    {
                                        if (chcklistbatch.Items[i].Selected == true)
                                        {
                                            for (int ik = 0; ik < CheckBoxList1.Items.Count; ik++)
                                            {
                                                if (CheckBoxList1.Items[ik].Selected == true)
                                                {
                                                    dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "'";
                                                    dv11 = dset1.Tables[0].DefaultView;
                                                    if (dv11.Count > 0)
                                                    {
                                                        drow = data.NewRow();
                                                        drow["Batch"] = Convert.ToString(CheckBoxList1.Items[ik].Text) + "   Quota Students";
                                                        data.Rows.Add(drow);


                                                        //  FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(CheckBoxList1.Items[ik].Text);

                                                        avoidcol.Add(data.Rows.Count - 1);

                                                        // FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Tag = "";
                                                        drow = data.NewRow();
                                                        drow["Batch"] = Convert.ToString(chcklistbatch.Items[i].Text);
                                                        data.Rows.Add(drow);

                                                        // ---------------- staff name start
                                                        //querystaf = "select distinct s.staff_code from staff_selector st,staffmaster s where st.staff_code=s.staff_code and st.subject_no='" + ddlsubj.SelectedValue + "' and st.batch_year in ('" + chcklistbatch.Items[i].Text + "')";
                                                        //ds1 = da.select_method_wo_parameter(querystaf, "text");
                                                        // ---------------- staff name end
                                                        int col = 0;
                                                        string buildvalue1 = "";
                                                        //for (int ij = 0; ij < Cbltesttyp.Items.Count; ij++)
                                                        for (int ij = 1; ij < data.Columns.Count; ij = ij + 3)
                                                        {
                                                            //if (Cbltesttyp.Items[ij].Selected == true)
                                                            {
                                                                //dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria_no='" + Cbltesttyp.Items[ij].Value + "'";
                                                                dset1.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and Criteria='" + testname[ij] + "'";
                                                                dv21 = dset1.Tables[0].DefaultView;
                                                                if (dv21.Count > 0)
                                                                {
                                                                    col++;
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col ].Text = Convert.ToString(dv21[0]["strength"]);
                                                                    //FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, col].HorizontalAlign = HorizontalAlign.Center;

                                                                    string failva = "0";
                                                                    dsfail.Tables[0].DefaultView.RowFilter = "batch_year=" + chcklistbatch.Items[i].Text + " and TextCode='" + CheckBoxList1.Items[ik].Value + "' and criteria='" + testname[ij] + "'";
                                                                    DataView dvfail = dsfail.Tables[0].DefaultView;
                                                                    if (dvfail.Count > 0)
                                                                    {
                                                                        failva = dvfail[0]["strength"].ToString();
                                                                    }

                                                                    data.Rows[data.Rows.Count - 1][ij + 2] = failva;


                                                                    col++;
                                                                    data.Rows[data.Rows.Count - 1][ij + 1] = Convert.ToString(dv21[0]["strength"]);


                                                                    int totcount = Convert.ToInt32(failva) + Convert.ToInt32(dv21[0]["strength"]);
                                                                    data.Rows[data.Rows.Count - 1][ij] = totcount.ToString();


                                                                    // ---------------- staff name start

                                                                    name = "";
                                                                    dssubject.Tables[0].DefaultView.RowFilter = "subject_code='" + ddlsubj.SelectedValue + "' and batch_year='" + chcklistbatch.Items[i].Text + "' and semester='" + ddlsem.SelectedItem.ToString() + "'";
                                                                    DataView dvstaff = dssubject.Tables[0].DefaultView;
                                                                    for (int s = 0; s < dvstaff.Count; s++)
                                                                    {
                                                                        if (name == "")
                                                                        {
                                                                            name = dvstaff[s]["staff_name"].ToString();
                                                                        }
                                                                        else
                                                                        {
                                                                            name = name + ", " + dvstaff[s]["staff_name"].ToString();
                                                                        }

                                                                    }
                                                                    if (rr == 1)
                                                                    {
                                                                        teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));

                                                                        rr++;
                                                                    }
                                                                    else
                                                                    {
                                                                        if (teststaffname.ContainsKey(data.Rows.Count - 1))
                                                                        {
                                                                            teststaffname.Remove(data.Rows.Count - 1);
                                                                            teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                        }
                                                                        else
                                                                            teststaffname.Add(data.Rows.Count - 1, Convert.ToString(name));
                                                                    }
                                                                    //if (ds1.Tables[0].Rows.Count > 0)
                                                                    //{
                                                                    //    staff = ds1.Tables[0].Rows[0]["staff_code"].ToString();
                                                                    //    string valsen = "";
                                                                    //    //string var = ds1.Tables[0].Rows[0]["CRITERIA_NO"].ToString();
                                                                    //    string[] array = staff.Split(',');
                                                                    //    if (array.Length > 0)
                                                                    //    {
                                                                    //        for (int rr1 = 0; rr1 < array.Length; rr1++)
                                                                    //        {
                                                                    //            valsen = array[rr1].ToString();
                                                                    //            if (buildvalue1 == "")
                                                                    //            {
                                                                    //                buildvalue1 = valsen;
                                                                    //            }
                                                                    //            else
                                                                    //            {
                                                                    //                buildvalue1 = buildvalue1 + "'" + "," + "'" + valsen;
                                                                    //            }
                                                                    //        }
                                                                    //    }
                                                                    //    string skr = "select staff_name from staffmaster where  staff_code in('" + buildvalue1 + "')";
                                                                    //    DataSet ds2 = da.select_method_wo_parameter(skr, "text");
                                                                    //    if (ds2.Tables[0].Rows.Count > 0)
                                                                    //    {
                                                                    //        for (int kh = 0; kh < ds2.Tables[0].Rows.Count; kh++)
                                                                    //        {
                                                                    //            if (!hasname.ContainsKey(ds2.Tables[0].Rows[kh]["staff_name"].ToString()))
                                                                    //            {
                                                                    //                hasname.Add(ds2.Tables[0].Rows[kh]["staff_name"].ToString(), cnt);
                                                                    //                cnt++;
                                                                    //                if (Convert.ToString(name) == "")
                                                                    //                {
                                                                    //                    name = ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                    //                }
                                                                    //                else
                                                                    //                {
                                                                    //                    name = name + "," + ds2.Tables[0].Rows[kh]["staff_name"].ToString();
                                                                    //                }
                                                                    //            }
                                                                    //        }
                                                                    //    }
                                                                    //    if (rr == 1)
                                                                    //    {
                                                                    //        FpSpread1.Sheets[0].ColumnCount++;
                                                                    //        FpSpread1.Sheets[0].ColumnHeader.Cells[0, FpSpread1.Sheets[0].ColumnCount - 1].Text = "Staff Name";
                                                                    //        FpSpread1.Sheets[0].ColumnHeaderSpanModel.Add(0, FpSpread1.Sheets[0].ColumnCount - 1, 2, 1);
                                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(name);
                                                                    //        rr++;
                                                                    //    }
                                                                    //    else
                                                                    //    {
                                                                    //        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, FpSpread1.Sheets[0].ColumnCount - 1].Text = Convert.ToString(name);
                                                                    //    }
                                                                    //    // ---------------- staff name end
                                                                    //}
                                                                }
                                                                else
                                                                {
                                                                    col++;
                                                                    //  data.Columns.Add("STAFF NAME", typeof(string));

                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // ---------------------------- for hostel end
                        if (TextBox3.Enabled == true)
                        {
                            for (int i = 0; i < data.Rows.Count; i++)
                            {
                                if (teststaffname.ContainsKey(i))
                                {
                                    //string valllll = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 0].Tag.ToString());

                                    data.Rows[i][data.Columns.Count - 1] = teststaffname[i];

                                }

                            }
                        }

                        if (data.Columns.Count > 0 && data.Rows.Count > 2)
                        {
                            Showgrid.DataSource = data;
                            Showgrid.DataBind();
                            Showgrid.Visible = true;

                            int col = data.Columns.Count;
                            foreach (KeyValuePair<int, string> dr in headspancol)
                            {
                                int r = dr.Key;
                                string c = dr.Value;
                                Showgrid.Rows[r].Cells[0].HorizontalAlign = HorizontalAlign.Center;
                                Showgrid.Rows[r].Cells[0].ColumnSpan = col;
                                if (c.ToUpper() == "BLUE")
                                {
                                    Showgrid.Rows[r].Cells[0].ForeColor = Color.Blue;
                                    Showgrid.Rows[r].Cells[0].BorderColor = Color.Black;
                                }
                                else
                                {
                                    Showgrid.Rows[r].Cells[0].ForeColor = Color.Chocolate;
                                    Showgrid.Rows[r].Cells[0].BorderColor = Color.Black;
                                }
                                for (int a = 1; a < col; a++)
                                    Showgrid.Rows[r].Cells[a].Visible = false;
                            }


                            int rowcnt = Showgrid.Rows.Count - 2;
                            //Rowspan
                            for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                            {
                                GridViewRow row = Showgrid.Rows[rowIndex];
                                GridViewRow previousRow = Showgrid.Rows[rowIndex + 1];
                                Showgrid.Rows[rowIndex].BackColor = ColorTranslator.FromHtml("#0CA6CA");
                                Showgrid.Rows[rowIndex].Font.Bold = true;
                                Showgrid.Rows[rowIndex].HorizontalAlign = HorizontalAlign.Center;

                                for (int i = 0; i < row.Cells.Count; i++)
                                {
                                    if (row.Cells[i].Text == previousRow.Cells[i].Text)
                                    {
                                        row.Cells[i].RowSpan = previousRow.Cells[i].RowSpan < 2 ? 2 :
                                                               previousRow.Cells[i].RowSpan + 1;
                                        previousRow.Cells[i].Visible = false;
                                    }

                                }


                            }

                            //ColumnSpan
                            for (int rowIndex = Showgrid.Rows.Count - rowcnt - 1; rowIndex >= 0; rowIndex--)
                            {


                                for (int cell = Showgrid.Rows[rowIndex].Cells.Count - 1; cell > 0; cell--)
                                {
                                    TableCell colum = Showgrid.Rows[rowIndex].Cells[cell];
                                    TableCell previouscol = Showgrid.Rows[rowIndex].Cells[cell - 1];
                                    if (colum.Text == previouscol.Text)
                                    {
                                        if (previouscol.ColumnSpan == 0)
                                        {
                                            if (colum.ColumnSpan == 0)
                                            {
                                                previouscol.ColumnSpan += 2;

                                            }
                                            else
                                            {
                                                previouscol.ColumnSpan += colum.ColumnSpan + 1;

                                            }
                                            colum.Visible = false;

                                        }
                                    }
                                }

                            }

                        }

                    }
                    else
                    {
                        lblnorec.Visible = true;
                        lblnorec.Text = "No Records Found";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = false;
            lblnorec.Text = ex.ToString();
        }
    }

    private static void AddTableColumn(DataTable resultsTable, StringBuilder ColumnName)
    {
        try
        {
            DataColumn tableCol = new DataColumn(ColumnName.ToString());
            resultsTable.Columns.Add(tableCol);
        }
        catch (System.Data.DuplicateNameException)
        {
            ColumnName.Append(" ");
            AddTableColumn(resultsTable, ColumnName);
        }
    }


    protected void Showgrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int grCol = 0; grCol < data.Columns.Count; grCol++)
                    e.Row.Cells[grCol].Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                for (int j = 1; j < data.Columns.Count; j++)
                    e.Row.Cells[j].HorizontalAlign = HorizontalAlign.Center;
            }

        }
        catch
        {


        }

    }


    protected void Excel_OnClick(object sender, EventArgs e)
    {
        try
        {
            string report = txtreptname.Text;
            if (report.ToString().Trim() != "")
            {
                da.printexcelreportgrid(Showgrid, report);
                lblnorec.Visible = false;
            }
            else
            {
                Label1.Text = "Please Enter Your Report Name";
                Label1.Visible = true;
            }
            Print.Focus();
        }

        catch (Exception ex)
        {
            Label1.Visible = true;
            Label1.Text = ex.ToString();
        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    { }

    protected void Print_OnClick(object sender, EventArgs e)
    {
        try
        {
            string buildvalue = "";
            for (int i = 0; i < chcklistbatch.Items.Count; i++)
            {
                if (chcklistbatch.Items[i].Selected == true)
                {
                    hat.Add(chcklistbatch.Items[i].Text, chcklistbatch.Items[i].Text);
                    string build = chcklistbatch.Items[i].Value.ToString();
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
            string strsem = "";
            string strsem1 = "";
            string regsem = "";
            string sems = "";
            if (ddlsem.SelectedValue != "")
            {
                if (ddlsem.SelectedValue == "")
                {
                    strsem = "";
                    strsem1 = "";
                    regsem = "";
                    sems = "";
                }
                else
                {
                    strsem = " and semester =" + ddlsem.SelectedValue.ToString() + "";
                    strsem1 = "and syllabus_master.semester=" + ddlsem.SelectedValue.ToString() + "";
                    regsem = " and registration.current_semester>=" + ddlsem.SelectedValue.ToString() + "";
                    sems = "and SM.semester=" + ddlsem.SelectedValue.ToString() + "";
                }

                string Sqlstr = "";
                string sk = "select distinct S.subject_no,subject_code,subject_name,sem.subject_type from subject as S,syllabus_master  as SM,subjectchooser as SC,Sub_sem as Sem,staff_selector st where S.subject_no=SC.Subject_no and  s.syll_code=SM.syll_code and SM.degree_code=" + degbranch.SelectedValue.ToString() + " " + sems.ToString() + " and st.subject_no=s.subject_no  and  SM.batch_year in ('" + buildvalue + "') and  S.subtype_no = Sem.subtype_no and promote_count=1 and s.subject_no='" + ddlsubj.SelectedValue + "'";
                if (Session["Staff_Code"].ToString() == "")
                {
                    Sqlstr = "" + sk + "  order by subject_code ";
                }
                else if (Session["Staff_Code"].ToString() != "")
                {
                    Sqlstr = "" + sk + " and staff_code='" + Session["Staff_Code"].ToString() + "'  order by subject_code ";
                }
                DataSet dsss = da.select_method_wo_parameter(Sqlstr, "Text");
                if (dsss.Tables[0].Rows.Count > 0)
                {
                    string ss = Session["usercode"].ToString();
                    string degreedetails = "SUBJECT ANALYSIS" + "@" + "                                                                                 " + "SUBJECT NAME: " + ddlsubj.SelectedItem.Text + "  CODE: " + dsss.Tables[0].Rows[0]["subject_code"].ToString();
                    string pagename = "Pay_Bill_Reconceliation.aspx";
                    Printcontrol.loadspreaddetails(Showgrid, pagename, degreedetails, 0, ss);
                    Printcontrol.Visible = true;
                    lblnorec.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblnorec.Visible = true;
            lblnorec.Text = ex.ToString();
        }
    }


    public void btnPrint11()
    {
        DAccess2 ddd2 = new DAccess2();
        string college_code = Convert.ToString(Session["collegecode"].ToString());
        string colQ = "select * from collinfo where college_code='" + college_code + "'";
        DataSet dsCol = new DataSet();
        dsCol = ddd2.select_method_wo_parameter(colQ, "Text");
        string collegeName = string.Empty;
        string collegeCateg = string.Empty;
        string collegeAff = string.Empty;
        string collegeAdd = string.Empty;
        string collegePhone = string.Empty;
        string collegeFax = string.Empty;
        string collegeWeb = string.Empty;
        string collegeEmai = string.Empty;
        string collegePin = string.Empty;
        string acr = string.Empty;
        string City = string.Empty;
        if (dsCol.Tables.Count > 0 && dsCol.Tables[0].Rows.Count > 0)
        {
            collegeName = Convert.ToString(dsCol.Tables[0].Rows[0]["Collname"]);
            City = Convert.ToString(dsCol.Tables[0].Rows[0]["address3"]);
            collegeAff = "(Affiliated to " + Convert.ToString(dsCol.Tables[0].Rows[0]["university"]) + ")";
            collegeAdd = Convert.ToString(dsCol.Tables[0].Rows[0]["address1"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["address2"]) + " , " + Convert.ToString(dsCol.Tables[0].Rows[0]["district"]) + " - " + Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePin = Convert.ToString(dsCol.Tables[0].Rows[0]["pincode"]);
            collegePhone = "OFFICE: " + Convert.ToString(dsCol.Tables[0].Rows[0]["phoneno"]);
            collegeFax = "FAX: " + Convert.ToString(dsCol.Tables[0].Rows[0]["faxno"]);
            collegeWeb = "Website: " + Convert.ToString(dsCol.Tables[0].Rows[0]["website"]);
            collegeEmai = "E-Mail: " + Convert.ToString(dsCol.Tables[0].Rows[0]["email"]);
            collegeCateg = "(" + Convert.ToString(dsCol.Tables[0].Rows[0]["category"]) + ")";
        }
        DateTime dt = DateTime.Now;
        int year = dt.Year;
        spCollegeName.InnerHtml = collegeName;
        spAddr.InnerHtml = collegeAdd;
        spDegreeName.InnerHtml = acr;
        spReportName.InnerHtml = "Subject Analysis Report";
        // spSection.InnerHtml ="Satff: "+ Convert.ToString(ddlSearchOption.SelectedItem.Text);


    }
}