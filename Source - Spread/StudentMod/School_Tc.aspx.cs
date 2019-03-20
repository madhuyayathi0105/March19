using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using InsproDataAccess;
using System.Configuration;
public partial class StudentMod_School_Tc : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string branch = string.Empty;
    string college_code = string.Empty;
    string q1 = string.Empty;
    InsproDirectAccess dirAcc = new InsproDirectAccess();

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
            setLabelText();
            lbl_clgname.Text = lbl_clgT.Text;
            lbl_degree.Text = lbl_degreeT.Text;
            lbl_branch.Text = lbl_branchT.Text;
            lbl_org_sem.Text = lbl_semT.Text;
            if (!IsPostBack)
            {
                BindCollege();
                bindbatch();
                degree();
                bindbranch(branch);
                bindsem();
                bindtextvalues();
                ddldobdate.Items.Insert(0, "DD");
                ddldobdate1.Items.Insert(0, "DD");
                for (int i = 1; i <= 31; i++)
                {
                    ddldobdate.Items.Insert(i, Convert.ToString(i));
                    ddldobdate1.Items.Insert(i, Convert.ToString(i));
                }
                ddldobYear.Items.Insert(0, "YYYY");
                ddldobYear1.Items.Insert(0, "YYYY");
                string year1 = System.DateTime.Now.ToString("yyyy");
                int a2 = 0;
                for (int y = Convert.ToInt32(year1) - 3; y >= Convert.ToInt32(year1) - 80; y--)  //modified by mullai
                {
                    a2++;
                    ddldobYear.Items.Insert(a2, Convert.ToString(y));
                    ddldobYear1.Items.Insert(a2, Convert.ToString(y));
                }
                txt_applicationcerticate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txt_dateofissueofcertificate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txt_CertificatedateH.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txt_laststudieddate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                txt_applicationcerticate.Attributes.Add("readonly", "readonly");
                txt_dateofissueofcertificate.Attributes.Add("readonly", "readonly");
                txt_CertificatedateH.Attributes.Add("readonly", "readonly");
                txt_laststudieddate.Attributes.Add("readonly", "readonly");
                txt_admissiondate.Attributes.Add("readonly", "readonly");
                string grouporusercode = "";
                if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                {
                    grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
                }
                else
                {
                    grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
                }
                string Master = "select * from Master_Settings where settings in('Roll No','Register No','Admission No') " + grouporusercode + "";
                DataSet ds = d2.select_method(Master, hat, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Roll No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Rollflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Register No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Regflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Student_Type" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Studflag"] = "1";
                        }
                        if (ds.Tables[0].Rows[i]["settings"].ToString() == "Admission No" && ds.Tables[0].Rows[i]["value"].ToString() == "1")
                        {
                            Session["Admissionflag"] = "1";
                        }
                    }
                }
                loadclass();
                rdb_general.Checked = true;
                bindgeneralconduct();
                bindtcdate();
                bind_TcFormate();
                ddl_searchtype.Items.Add(new ListItem("Roll No", "0"));
                ddl_searchtype.Items.Add(new ListItem("Reg No", "1"));
                ddl_searchtype.Items.Add(new ListItem("Admission No", "2"));
                ddl_searchtype.Items.Add(new ListItem("App No", "3"));
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
            }
        }
        catch (Exception ex) { }
    }

    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindbatch();
        degree();
        bindbranch(branch);
        bindsem();
    }

    public void ddl_batch_SelectedIndexChanged(object sender, EventArgs e)
    {
        bindsem();
        
      
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
                        txt_degree.Text = lbl_degreeT.Text + "(" + (cbl_degree.Items.Count) + ")";
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
                bindsem();
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
                txt_degree.Text = lbl_degreeT.Text + "(" + seatcount.ToString() + ")";
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
            bindsem();
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
                txt_branch.Text = lbl_branchT.Text + "(" + (cbl_branch.Items.Count) + ")";
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
                txt_branch.Text = lbl_branchT.Text + "(" + commcount.ToString() + ")";
                cb_branch.Checked = true;
            }
            else
            {
                txt_branch.Text = lbl_branchT.Text + "(" + commcount.ToString() + ")";
            }
            bindsem();
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
                txt_sem.Text = lbl_semT.Text + "(" + (cbl_sem.Items.Count) + ")";
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
                txt_sem.Text = lbl_semT.Text + "(" + commcount.ToString() + ")";
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
            college_code = Convert.ToString(ddlcollege.SelectedItem.Value);
            string rights = "";
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
            string query = "select distinct d.Course_Id,c.Course_Name from Degree d,course c ,DeptPrivilages p where p.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
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
                    //for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //{
                    cbl_degree.Items[0].Selected = true;
                    //}
                    txt_degree.Text = lbl_degreeT.Text + " (" + 1 + ")";
                    cb_degree.Checked = true;
                }
                else
                {
                    txt_degree.Text = "--Select--";
                    cb_degree.Checked = false;
                }
                string build = "";
                string deg = "";
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            build = cbl_degree.Items[i].Value.ToString();
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
            string rights = "";
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
            cbl_branch.Items.Clear();
            string commname = "";
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + " ";
            }
            else
            {
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + "";
            }
            ds = d2.select_method(commname, hat, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_branch.DataSource = ds;
                cbl_branch.DataTextField = "dept_name";
                cbl_branch.DataValueField = "degree_code";
                cbl_branch.DataBind();
                if (cbl_branch.Items.Count > 0)
                {
                    //for (int i = 0; i < cbl_branch.Items.Count; i++)
                    //{
                    cbl_branch.Items[0].Selected = true;
                    //}
                    txt_branch.Text = lbl_branchT.Text + "(" + 1 + ")";
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindsem()
    {
        cbl_sem.Items.Clear();
        txt_sem.Text = "--Select--";
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
        if (ddl_batch.Items.Count > 0)
        {
            batch = Convert.ToString(ddl_batch.SelectedItem.Value);
        }
        //batch = build;
        if (branch.Trim() != "" && batch.Trim() != "")
        {
            // ds = d2.BindSem(branch, batch, ddlcollege.SelectedItem.Value);
            string strsql1 = "select distinct duration+1 as duration,first_year_nonsemester  from degree where degree_code in (" + branch + ") and college_code=" + ddlcollege.SelectedItem.Value + " order by Duration desc";
            ds = d2.select_method_wo_parameter(strsql1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                int dur = 0;
                int.TryParse(Convert.ToString(ds.Tables[0].Rows[0][0]), out dur);
                for (i = 1; i <= dur; i++)
                {
                    cbl_sem.Items.Add(Convert.ToString(i));
                    cbl_sem.Items[i - 1].Selected = true;
                    cb_sem.Checked = true;
                }
                txt_sem.Text = lbl_org_sem.Text + "(" + cbl_sem.Items.Count + ")";
            }
        }
    }

    void BindCollege()
    {
        try
        {
            ds.Clear();
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
                columnfield = " and user_code='" + Session["usercode"] + "'";
            }
            Hashtable hat1 = new Hashtable();
            hat1.Clear();
            hat1.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat1, "sp");
            ddlcollege.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch
        {
        }
    }

    public void bindbatch()
    {
        try
        {
            ddl_batch.Items.Clear();
            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_batch.DataSource = ds;
                ddl_batch.DataTextField = "batch_year";
                ddl_batch.DataValueField = "batch_year";
                ddl_batch.DataBind();
            }
        }
        catch
        {
        }
    }

    protected void bindtextvalues()
    {
        if (ddlcollege.Items.Count > 0)
        {
            q1 = "  select distinct co_curricular ,isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,co_curricular) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='cocur' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'') as texval from applyn where isnull(co_curricular,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' and isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,co_curricular) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='cocur' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'')<>''";
            q1 = q1 + "   select distinct citizen ,isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,citizen) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='citi' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'') as textval from applyn where isnull(citizen,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' and isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,citizen) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='citi' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'')<>''";
            q1 = q1 + "   select distinct caste ,isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,caste) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='caste' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'') as textval from applyn where isnull(caste,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' and isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,caste) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='caste' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'')<>''";
            q1 = q1 + "   select distinct religion ,isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,religion) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='relig' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'') as textval from applyn where isnull(caste,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' and isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,religion) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='relig' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'')<>''";

            q1 = q1 + "    select distinct community ,isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,community) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='comm' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'') as textval from applyn where isnull(community,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' and isnull((select textval from textvaltable where convert(varchar,textcode)=convert(varchar,community) and textval is not null and isnull(textval,-1)<>'-1' and TextCriteria='comm' and college_code='" + ddlcollege.SelectedItem.Value + "' ),'')<>''";
            q1 = q1 + "   select distinct TextCode,textval from textvaltable where TextCriteria like '%ATTYP%' and college_code='" + ddlcollege.SelectedItem.Value + "' and textval<>''and textval<>'-' order by textval";


            // q1 = q1 + "  select distinct citizen,(select textval from textvaltable where convert(varchar,textcode)=convert(varchar,citizen) and isnull(textval,-1)<>'-1' and TextCriteria='comm' and college_code='" + ddlcollege.SelectedItem.Value + "' )as citizen from applyn where isnull(citizen,'0')<>'0' and college_code='" + ddlcollege.SelectedItem.Value + "' ";
            //q1 = "  select textval,textcode from textvaltable where TextCriteria='cocur' and textval is not null and textval<>'' and college_code='" + ddlcollege.SelectedItem.Value + "'  order by textval  ";
            // select textval,textcode from textvaltable where TextCriteria='citi' and textval is not null and textval<>'' and college_code='" + ddlcollege.SelectedItem.Value + "' and TextCriteria2='citi1' order by textval";
            //q1 = q1 + "   select TextCode,textval from textvaltable where TextCriteria like '%comm%' and college_code='" + ddlcollege.SelectedItem.Value + "' and textval<>''and textval<>'-' and TextCriteria2='comm1' order by textval";
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    DropDownList4.DataSource = ds.Tables[0];
            //    DropDownList4.DataTextField = "texval";
            //    DropDownList4.DataValueField = "co_curricular";
            //    DropDownList4.DataBind();
            //}
            if (ds.Tables[1].Rows.Count > 0)
            {
                ddlcountry.DataSource = ds.Tables[1];
                ddlcountry.DataTextField = "textval";
                ddlcountry.DataValueField = "citizen";
                ddlcountry.DataBind();

                ddlcountry1.DataSource = ds.Tables[1];
                ddlcountry1.DataTextField = "textval";
                ddlcountry1.DataValueField = "citizen";
                ddlcountry1.DataBind();
            }
            if (ds.Tables[2].Rows.Count > 0)
            {
                ddlCaste.DataSource = ds.Tables[2];
                ddlCaste.DataTextField = "textval";
                ddlCaste.DataValueField = "caste";
                ddlCaste.DataBind();

                ddl_caste1.DataSource = ds.Tables[2];
                ddl_caste1.DataTextField = "textval";
                ddl_caste1.DataValueField = "caste";
                ddl_caste1.DataBind();
            }
            if (ds.Tables[3].Rows.Count > 0)
            {
                ddlreligion.DataSource = ds.Tables[3];
                ddlreligion.DataTextField = "textval";
                ddlreligion.DataValueField = "religion";
                ddlreligion.DataBind();

                ddlreligion1.DataSource = ds.Tables[3];
                ddlreligion1.DataTextField = "textval";
                ddlreligion1.DataValueField = "religion";
                ddlreligion1.DataBind();
            }
            if (ds.Tables[4].Rows.Count > 0)
            {
                ddlcoummunity.DataSource = ds.Tables[4];
                ddlcoummunity.DataTextField = "textval";
                ddlcoummunity.DataValueField = "community";
                ddlcoummunity.DataBind();

                ddlcoummunity1.DataSource = ds.Tables[4];
                ddlcoummunity1.DataTextField = "textval";
                ddlcoummunity1.DataValueField = "community";
                ddlcoummunity1.DataBind();
            }
            if (ds.Tables[5].Rows.Count > 0)
            {
                ddl_attendance.DataSource = ds.Tables[5];
                ddl_attendance.DataTextField = "textval";
                ddl_attendance.DataValueField = "TextCode";
                ddl_attendance.DataBind();
            }
            //DropDownList4.Items.Insert(0, "Select");
            //DropDownList4.Items.Insert(DropDownList4.Items.Count, "Others");
            ddlcountry.Items.Insert(0, "Select");
            ddlcountry.Items.Insert(ddlcountry.Items.Count, "Others");
            ddlcoummunity.Items.Insert(0, "Select");
            ddlcoummunity.Items.Insert(ddlcoummunity.Items.Count, "Others");
            ddlreligion.Items.Insert(0, "Select");
            ddlreligion.Items.Insert(ddlreligion.Items.Count, "Others");
            ddlCaste.Items.Insert(0, "Select");
            ddlCaste.Items.Insert(ddlCaste.Items.Count, "Others");
            ddl_caste1.Items.Insert(0, "Select");
            ddl_caste1.Items.Insert(ddl_caste1.Items.Count, "Others");
            ddl_attendance.Items.Insert(0, "Select");
            ddl_attendance.Items.Insert(ddl_attendance.Items.Count, "Others");
            ddlcountry1.Items.Insert(0, "Select");
            ddlcountry1.Items.Insert(ddlcountry1.Items.Count, "Others");
            ddlcoummunity1.Items.Insert(0, "Select");
            ddlcoummunity1.Items.Insert(ddlcoummunity1.Items.Count, "Others");
            ddlreligion1.Items.Insert(0, "Select");
            ddlreligion1.Items.Insert(ddlreligion1.Items.Count, "Others");
        }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstudentadmit(string prefixText)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();
        string query = " select r.Stud_Name+'-'+r.Roll_Admit as roll_admit from applyn a,Registration r where a.app_no=r.App_No  and r.Stud_Name+'-'+r.Roll_Admit like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getappfrom(string prefixText, string contextKey)
    {
        WebService ws = new WebService();
        List<string> name = new List<string>();

        //"Roll No", "0"
        //"Reg No", "1"
        //"Admission No", "2"
        //"App No", "3"
        int SEARCHTYPE = 0;
        int.TryParse(contextKey, out SEARCHTYPE);
        string type = "";
        switch (SEARCHTYPE)
        {
            case 0:
                type = "r.roll_no";
                break;
            case 1:
                type = "r.reg_no";
                break;
            case 2:
                type = "r.Roll_Admit";
                break;
            case 3:
                type = "r.app_no";
                break;
        }
        string query = " select " + type + "  from applyn a,Registration r where a.app_no=r.App_No  and " + type + " like '" + prefixText + "%' ";
        name = ws.Getname(query);
        return name;
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
        lbl.Add(lbl_clgT);
        lbl.Add(lbl_degreeT);
        lbl.Add(lbl_branchT);
        lbl.Add(lbl_semT);
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        lbl.Add(lbl_semT);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }

    protected void clear()
    {
        txt_serialno.Text = "";
        txt_tmrno.Text = "";
        txt_CertificatenoH.Text = "";
        txt_part1language.Text = "";
        txt_mudiumofstudy.Text = "";
        txt_identification.Text = "";
        //txt_joinclass.Text = "";
        ddl_joinclass.SelectedIndex = 0;
        txt_educationdistrict.Text = ""; txt_disposelno.Text = "";
        txt_affilicationno.Text = "";
        txt_admissionno.Text = "";
        txt_studname.Text = "";
        txt_qualified.Text = "";
        txt_mothername.Text = "";
        txt_paidschool.Text = "";
        txt_fathername.Text = "";
        txt_feecon.Text = "";
        txt_GuardianName.Text = "";
        txt_totalnoofworkingdays.Text = "";
        ddldobdate.SelectedIndex = 0;
        ddldobMonth.SelectedIndex = 0;
        ddldobYear.SelectedIndex = 0;
        //DropDownList4.SelectedIndex = 0;
        ddlCaste.SelectedIndex = 0;
        ddl_catagory.SelectedIndex = 0;
        ddlreligion.SelectedIndex = 0;
        txt_totalnoofworkingdayspresent.Text = "";
        ddlcountry.SelectedIndex = 0;
        txt_generalconduct.Text = "";
        txtCommunity.Text = "";
        ddlcoummunity.SelectedIndex = 0;
        txt_applicationcerticate.Text = "";
        txt_admdate.Text = "";
        txt_dateofissueofcertificate.Text = "";
        txt_laststudiedclass.Text = "";
        txt_leaving.Text = "";
        txt_schoolorboard.Text = "";
        txt_remarks.Text = "";
        txt_failsameclass.Text = "";
        txt_subjectstudied.Text = "";
        txt_extraactivites.Text = "";
        //rdbextraactivitesNo.Checked = true;
        //txt_specifyNcc.Text = "";
        txt_regno.Text = "";
        txt_studname1.Text = "";
        txt_mothername1.Text = "";
        txt_fathername1.Text = "";
        ddldobdate1.SelectedIndex = 0;
        ddldobMonth1.SelectedIndex = 0;
        ddldobYear1.SelectedIndex = 0;
        ddlcountry1.SelectedIndex = 0;
        ddlcoummunity1.SelectedIndex = 0;
        ddlreligion1.SelectedIndex = 0;
        programeCompleted.Text = "";
        txt_exammonthandyear.Text = "";
        txt_migrationserielno.Text = "";
        txt_generalconduct.Text = "";
        //txt_leavinginstition.Text = "";
        //txt_commencementofclass.Text = "";
        txt_mudiumofstudy1.Text = "";
        txt_periodofstudied.Text = "";
        ddl_dateoofissuemigration.SelectedIndex = 0;
        //ddl_commencementofclass.SelectedIndex = 0;
        //ddl_dateofissuecertificate.SelectedIndex = 0;
        //ddl_lastattendedclass.SelectedIndex = 0;
        //   ddl_tccertificateissuedate.SelectedIndex = 0;
        ddl_caste1.SelectedIndex = 0;
        ddl_attendance.SelectedIndex = 0;
        ddl_generalconduct.SelectedIndex = 0;
        txt_part1language1.Text = "";
        txt_remarks1.Text = "";
        txt_Aadharcardno.Text = "";
        txt_Aadharcardno2.Text = "";
        txt_Aadharcardno3.Text = "";
        txt_serial_no.Text = "";

    }

    protected void imagebtnpopclose3_Click(object sender, EventArgs e)
    {
        txt_admissionno.Enabled = true;
        pop_studdetails.Visible = false;
        clear();
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getadmissionnoname(string prefixText, string contextKey)
    {
        List<string> name = new List<string>();
        try
        {
            string college_code = contextKey;
            WebService ws = new WebService();
            string query = " select Roll_Admit+'-'+r.Stud_Name as name from applyn a,Registration r where a.app_no=r.App_No and  Roll_Admit like '" + prefixText + "%' and r.college_code='" + college_code + "'";
            name = ws.Getname(query);
        }
        catch { return name; }
        return name;
    }

    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }

    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string sem = rs.GetSelectedItemsValueAsString(cbl_sem);
            if (ddlAppFormat.Items.Count > 0)
            {
                if (deptcode.Trim() != "" && degreecode.Trim() != "" && sem.Trim() != "")
                {
                    rs.Fpreadheaderbindmethod("S.No-50/Edit-50/Select-50/Student Name-200/Roll No-150/Reg No-150/Admission No-150/Gender-120/Section-70/" + lbl_degreeT.Text + "-150/" + lbl_branchT.Text + "-200/" + lbl_semT.Text + "-100", FpSpread1, "FALSE");
                    if (rdb_general.Checked)
                    {
                        q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections from applyn a,Registration r, degree d,Department dt,Course C where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' ";
                    }
                    else if (rdb_Request.Checked)
                    {
                        q1 = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections from applyn a,Registration r, degree d,Department dt,Course C,RQ_Requisition rq where r.App_No=rq.ReqAppNo and a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and RequestType='11' and ReqAppStatus=1 ";
                        //CertReqType='1' studentrequest CertReqType='2' staffrequest
                    }
                    string type = "";
                    switch (Convert.ToUInt32(ddl_searchtype.SelectedValue))
                    {
                        case 0:
                            type = "r.roll_no";
                            break;
                        case 1:
                            type = "r.reg_no";
                            break;
                        case 2:
                            type = "r.Roll_Admit";
                            break;
                        case 3:
                            type = "r.app_no";
                            break;
                    }
                    if (txt_searchappno.Text.Trim() != "")
                        q1 += " and " + type + "='" + txt_searchappno.Text.Trim() + "'";
                    else if (txt_searchstudname.Text.Trim() != "")
                        q1 += " and r.Stud_Name+'-'+r.Roll_Admit='" + txt_searchstudname.Text.Trim() + "'";
                    else
                        q1 += " and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + Convert.ToString(ddl_batch.SelectedItem.Value) + "') and c.Course_Id in('" + deptcode + "') and r.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' ";
                    if (sem.Trim() != "")
                        q1 += " and  r.Current_Semester in('" + sem + "') ";
                    ds.Clear();
                    ds = d2.select_method_wo_parameter(q1, "text");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        FarPoint.Web.Spread.ButtonCellType btn = new FarPoint.Web.Spread.ButtonCellType();
                        FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
                        FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();
                        cb.AutoPostBack = false;
                        cball.AutoPostBack = true;
                        btn.Text = "Edit";
                        btn.CssClass = "textbox btn1";
                        btn.ForeColor = System.Drawing.Color.Blue;
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = cball;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            FpSpread1.Sheets[0].Rows.Count++;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count - 1);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].CellType = btn;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["App_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].CellType = cb;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(dr["degree_code"]);
                            FpSpread1.Sheets[0].Columns[4].Visible = false;
                            FpSpread1.Sheets[0].Columns[5].Visible = false;
                            FpSpread1.Sheets[0].Columns[6].Visible = false;
                            if (Convert.ToString(Session["Rollflag"]) == "1")
                                FpSpread1.Sheets[0].Columns[4].Visible = true;
                            if (Convert.ToString(Session["Regflag"]) == "1")
                                FpSpread1.Sheets[0].Columns[5].Visible = true;
                            if (Convert.ToString(Session["Admissionflag"]) == "1")
                                FpSpread1.Sheets[0].Columns[6].Visible = true;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].CellType = txt;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].CellType = txt;

                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["stud_name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["roll_no"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["Reg_No"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["Roll_Admit"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["sex"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["sections"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dr["Course_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dr["Dept_Name"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Text = Convert.ToString(dr["Current_Semester"]);
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Locked = true;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Left;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].HorizontalAlign = HorizontalAlign.Center;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Size = FontUnit.Medium;
                            FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 11].Font.Name = "Book Antiqua";
                        }
                        FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                        FpSpread1.Visible = true;
                        FpSpread1.SaveChanges();
                        lbl_error.Visible = false;
                        btn_print.Visible = true;
                    }
                    else
                    {
                        btn_print.Visible = false;
                        FpSpread1.Visible = false;
                        lbl_error.Visible = true;
                        lbl_error.Text = "No Record Founds";
                    }
                }
                else
                {
                    btn_print.Visible = false;
                    FpSpread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "Please Select All Fields";
                }
            }
            else
            {
                btn_print.Visible = false;
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Set Transfer Certificate Format";
            }
        }
        catch (Exception ex)
        {
            btn_print.Visible = false;
            FpSpread1.Visible = false;
            lbl_error.Visible = true;
            lbl_error.Text = ex.ToString();
        }
    }

    protected void btn_addnew_OnClick(object sender, EventArgs e)
    {
        if (ddlAppFormat.Items.Count > 0)
        {
            if (ddlAppFormat.SelectedValue == "0" || ddlAppFormat.SelectedValue == "1" || ddlAppFormat.SelectedValue == "2")
            { pop_studdetails.Visible = true; }
            if (ddlAppFormat.SelectedValue == "3" || ddlAppFormat.SelectedValue == "4" || ddlAppFormat.SelectedValue == "5" || ddlAppFormat.SelectedValue == "6" || ddlAppFormat.SelectedValue == "7" || ddlAppFormat.SelectedValue == "8")
            { pop_clg_tc.Visible = true; txt_regno.Enabled = true; }
        }
        else
        {
            lbl_error.Visible = true;
            lbl_error.Text = "Please Set Transfer Certificate Format";
        }
        clear();
    }

    protected void fp_btn_Click(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {

            FpSpread1.SaveChanges();
            string actrow = e.SheetView.ActiveRow.ToString();
            string actcol = e.SheetView.ActiveColumn.ToString();
            txt_studname.Enabled = false;
            txt_mothername.Enabled = false;
            txt_fathername.Enabled = false;
            //txt_joinclass.Enabled = false;
            txt_laststudiedclass.Enabled = false;
            //txt_admdate.Enabled = false;
            txt_part1language1.Text = "ENGLISH";
            txt_mudiumofstudy1.Text = "ENGLISH";
            ddl_attendance.SelectedIndex = ddl_attendance.Items.IndexOf(ddl_attendance.Items.FindByText("REGULAR"));
            ddl_generalconduct.SelectedIndex = ddl_generalconduct.Items.IndexOf(ddl_generalconduct.Items.FindByText("GOOD"));

            if (actrow.Trim() != "0" && actcol.Trim() == "1")
            {
                txt_doLeaving.Text = DateTime.Now.ToString("dd/mm/yyyy");
                txt_doAdmission.Text = DateTime.Now.ToString("dd/mm/yyyy");
                string app_no = Convert.ToString(FpSpread1.Sheets[0].Cells[Convert.ToInt32(actrow), Convert.ToInt32(actcol)].Tag).Trim();
                lbl_app_no.Text = app_no;
                lbl_app_no1.Text = app_no;


                string edu_level = d2.GetFunction("select c.edu_level from Registration r,applyn a,Course c,Degree dg,Department dt where dt.DepT_Code=dg.Dept_code and dg.degree_code=r.degree_code and r.app_no=a.app_no and c.Course_id=dg.Course_id and r.college_code=dg.College_code and r.app_no='" + app_no + "'");

                if (edu_level.ToLower().Contains("m.phil") || edu_level.ToLower().Contains("mphil") || edu_level.ToLower().Contains("m phil") || edu_level.ToLower().Contains("pg"))
                {
                    txt_part1language1.Text = "";
                }
                if (edu_level.ToLower().Contains("m.phil") || edu_level.ToLower().Contains("mphil") || edu_level.ToLower().Contains("m phil"))
                {
                    txt_mudiumofstudy1.Text = "";
                }


                q1 = "    select (select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Part1Language) and TextCriteria='Cplan')Part1Language,a.idmark, isnull(t.Vocationorgeneral,0)Vocationorgeneral, (select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Medium_study) and TextCriteria='PLang') Medium_study,a.TMR_NO,a.Certificate_No,CONVERT(varchar(10),a.Certificate_Date,103)Certificate_Date,r.Roll_Admit,r.Stud_Name,a.parent_name,a.mother,a.guardian_name,a.dob,a.citizen,a.caste,CONVERT(varchar(10),r.Adm_Date,103)Adm_Date,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.remarks))remarks,a.degree_code as First_Joinclass,r.degree_code Last_studiedclass, t.Annualexamination_result, t.noofattempts,t.subjectstudied,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.NCC_BoyScout_GirlGuide) and TextCriteria='NccSc')NCC_BoyScout_GirlGuide,t.Qualified_promotion,t.Paid_dues,t.General_conduct, t.Dateofapplcertificate,t.dateofissuecertificate,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.reasonforrelive))reasonforrelive,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.co_curricular) AND TextCriteria='cocur'))co_curricular,dis_extra_Activity ,t.Fee_concession, t.Totalnoofworkingdays ,t.Totalnoofpresentdays,a.caste,a.religion,a.community,t.categorytype,t.MedicalInspection, t.laststudieddate,Serial_no,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Class_teacher_name) and TextCriteria='ClsTe') as Class_teacher_name,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.checkedby_name) and TextCriteria='ChkBy') as checkedby_name, (select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.checkby_design) and TextCriteria='ChkDe') as checkby_design ,t.program_completed,t.Last_exam_mon_year,t.Migration_Sl_No,General_conduct as Conduct_Character,Last_Studied_Class,commencementofclass, Medium_study,migration_date,commencement_date,CONVERT(varchar(10),dateofissuecertificate,103)dateofissuecertificate1,CONVERT(varchar(10),laststudieddate,103)laststudieddate1,r.Reg_No,dateofissuecertificate as dateofissuecertificate1  , periodofstudied, Transfer_cert_made,a.Aadharcard_no ,t.Attendance_type,r.tcserialNo as AutoSerialno,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.partI_Language)))as ApartI_Language,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.medium_ins)))as Amedium_ins,CONVERT(varchar(10),dateofleaving,103)dateofleaving,r.delflag from applyn a,Registration r left join Tc_details t on r.App_No=t.App_no where  a.app_no=r.App_No and r.App_No='" + app_no + "'";//CONVERT(varchar(10),a.dob,103),(select textval from textvaltable where CONVERT(varchar,TextCode)=a.dis_extra_Activity and TextCriteria='cocur')
                q1 += "   select Dept_Name,Degree_Code from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                q1 += " select Affiliation_No,educationdistrict,disposal_no from collinfo where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                q1 += " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                // dateofleaving='" + dateofleaving + "',Stud_Name='" + studentname + "',Adm_Date='" + (admissiondateDt.ToString("MM/dd/yyyy"
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(q1, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    delFlagValue.Text = Convert.ToString(ds1.Tables[0].Rows[0]["delflag"]);
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        ddl_joinclass.DataSource = ds1.Tables[1];
                        ddl_joinclass.DataTextField = "Dept_Name";
                        ddl_joinclass.DataValueField = "Degree_Code";
                        ddl_joinclass.DataBind();
                    }
                    if (ds1.Tables[2].Rows.Count > 0)
                    {
                        txt_affilicationno.Text = Convert.ToString(ds1.Tables[2].Rows[0]["Affiliation_No"]);
                        txt_educationdistrict.Text = Convert.ToString(ds1.Tables[2].Rows[0]["educationdistrict"]);
                        txt_disposelno.Text = Convert.ToString(ds1.Tables[2].Rows[0]["disposal_no"]);
                    }
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        txt_studname.Enabled = false;
                        txt_mothername.Enabled = false;
                        txt_fathername.Enabled = false;
                        //txt_GuardianName.Enabled = false;
                        //txt_totalnoofworkingdays.Enabled = false;
                        //txt_totalnoofworkingdayspresent.Enabled = false;
                        //txt_admdate.Enabled = false;
                        string fathername = Convert.ToString(dr["parent_name"]);
                        string mothername = Convert.ToString(dr["mother"]);
                        string guardian = Convert.ToString(dr["guardian_name"]);
                        string dob = Convert.ToString(dr["dob"]);
                        string country = Convert.ToString(dr["citizen"]);
                        string caste = Convert.ToString(dr["caste"]);
                        string religion = Convert.ToString(dr["religion"]);
                        string community = Convert.ToString(dr["community"]);
                        string category = Convert.ToString(dr["categorytype"]);

                        string Adm_Date = Convert.ToString(dr["Adm_Date"]);
                        string remarks = Convert.ToString(dr["remarks"]);
                        string Attempts = Convert.ToString(dr["noofattempts"]);
                        string studname = Convert.ToString(dr["stud_name"]);
                        string roll_admit = Convert.ToString(dr["roll_admit"]);
                        string regno = Convert.ToString(dr["Reg_No"]);
                        string dateOfLeaving = Convert.ToString(dr["dateofleaving"]);

                        if (ddlAppFormat.SelectedValue == "0" || ddlAppFormat.SelectedValue == "1" || ddlAppFormat.SelectedValue == "2")
                        {
                            #region school
                            ddlCaste.SelectedIndex = ddlCaste.Items.IndexOf(ddlCaste.Items.FindByValue(caste));
                            ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(country));
                            ddlcoummunity.SelectedIndex = ddlcoummunity.Items.IndexOf(ddlcoummunity.Items.FindByValue(community));
                            ddlreligion.SelectedIndex = ddlreligion.Items.IndexOf(ddlreligion.Items.FindByValue(religion));
                            ddl_catagory.SelectedIndex = ddl_catagory.Items.IndexOf(ddl_catagory.Items.FindByValue(category));

                            txt_admissionno.Text = roll_admit + '-' + studname;
                            txt_admissionno.Enabled = false;
                            txt_studname.Text = studname;
                            txt_mothername.Text = mothername;
                            txt_fathername.Text = fathername;
                            txt_GuardianName.Text = guardian;
                            txt_tmrno.Text = Convert.ToString(dr["TMR_NO"]);
                            txt_CertificatenoH.Text = Convert.ToString(dr["Certificate_No"]);
                            string MedicalInspection = Convert.ToString(dr["MedicalInspection"]);
                            txt_doAdmission.Text = Adm_Date;
                            txt_doLeaving.Text = dateOfLeaving;
                            if (MedicalInspection.Trim() == "1" || MedicalInspection.Trim() == "True")
                            {
                                rdb_medical.Checked = true;
                                rdb_medical1.Checked = false;
                            }
                            else
                            {
                                rdb_medical.Checked = true;
                                rdb_medical1.Checked = false;
                            }
                            if (dob.Trim() != "")
                            {
                                try
                                {
                                    DateTime dobdate = new DateTime();
                                    DateTime.TryParse(dob, out dobdate);
                                    ddldobdate.SelectedIndex = ddldobdate.Items.IndexOf(ddldobdate.Items.FindByText(Convert.ToString((dobdate.ToString("dd"))).TrimStart('0')));
                                    ddldobMonth.SelectedIndex = ddldobMonth.Items.IndexOf(ddldobMonth.Items.FindByValue(dobdate.ToString("MM")));
                                    ddldobYear.SelectedIndex = ddldobYear.Items.IndexOf(ddldobYear.Items.FindByText(dobdate.ToString("yyyy")));
                                }
                                catch { }
                            }
                            txt_admdate.Text = Adm_Date;
                            txt_remarks.Text = remarks;
                            txt_failsameclass.Text = Attempts;
                            txt_generalconduct.Text = Convert.ToString(dr["General_conduct"]);
                            DateTime cerapplidate = new DateTime(); DateTime cerissueidate = new DateTime();
                            DateTime.TryParse(Convert.ToString(dr["Dateofapplcertificate"]), out cerapplidate);
                            DateTime.TryParse(Convert.ToString(dr["dateofissuecertificate"]), out cerissueidate);
                            if (Convert.ToString(cerapplidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                                txt_applicationcerticate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            else
                                txt_applicationcerticate.Text = Convert.ToString(cerapplidate.ToString("dd/MM/yyyy"));
                            if (Convert.ToString(cerissueidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                                txt_dateofissueofcertificate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            else
                                txt_dateofissueofcertificate.Text = Convert.ToString(cerissueidate.ToString("dd/MM/yyyy"));
                            DateTime ceritificatedateH_dt = new DateTime();
                            DateTime.TryParse(Convert.ToString(dr["Certificate_Date"]), out ceritificatedateH_dt);
                            if (Convert.ToString(ceritificatedateH_dt.ToString("dd/MM/yyyy")) == "01/01/0001")
                                txt_CertificatedateH.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            else
                                txt_CertificatedateH.Text = Convert.ToString(ceritificatedateH_dt.ToString("dd/MM/yyyy"));
                            DateTime lastdate_dt = new DateTime();
                            DateTime.TryParse(Convert.ToString(dr["laststudieddate"]), out lastdate_dt);
                            if (Convert.ToString(lastdate_dt.ToString("dd/MM/yyyy")) == "01/01/0001")
                                txt_laststudieddate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                            else
                                txt_laststudieddate.Text = Convert.ToString(lastdate_dt.ToString("dd/MM/yyyy"));
                            DataView dv = new DataView();
                            ds1.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(dr["Last_studiedclass"]) + "'";
                            dv = ds1.Tables[1].DefaultView;
                            if (dv.Count > 0)
                            {
                                txt_laststudiedclass.Text = Convert.ToString(dv[0]["Dept_Name"]);
                            }
                            else { txt_laststudiedclass.Text = ""; }
                            ddl_joinclass.SelectedIndex = ddl_joinclass.Items.IndexOf(ddl_joinclass.Items.FindByValue(Convert.ToString(dr["First_Joinclass"])));
                            //DataView dv1 = new DataView();
                            //ds1.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(dr["First_Joinclass"]) + "'";
                            //dv1 = ds1.Tables[1].DefaultView;
                            //if (dv1.Count > 0)
                            //{
                            //    //txt_joinclass.Text = Convert.ToString(dv1[0]["Dept_Name"]);
                            //    //ddl_joinclass
                            //}
                            //else { //txt_joinclass.Text = ""; }


                            txt_leaving.Text = Convert.ToString(dr["reasonforrelive"]);
                            txt_schoolorboard.Text = Convert.ToString(dr["Annualexamination_result"]);
                            if (Convert.ToString(dr["co_curricular"]) != "")
                            {
                                txt_extraactivites.Text = Convert.ToString(dr["co_curricular"]);
                                //rdbextraactivitesNo.Checked = false;
                                //rdbextraactivitesYes.Checked = true;
                                //DropDownList4.SelectedIndex = DropDownList4.Items.IndexOf(DropDownList4.Items.FindByValue(Convert.ToString(dr["co_curricular"])));
                                //txt_specifyNcc.Text = Convert.ToString(dr["dis_extra_Activity"]);
                                //DropDownList4.Style.Add("display", "block");
                                //txt_specifyNcc.Style.Add("display", "block");
                            }
                            else
                            {
                                txt_extraactivites.Text = " - ";
                                //rdbextraactivitesYes.Checked = false;
                                //rdbextraactivitesNo.Checked = true;
                            }
                            if (Convert.ToString(dr["NCC_BoyScout_GirlGuide"]) != "")
                            {
                                txt_ncc.Text = Convert.ToString(dr["NCC_BoyScout_GirlGuide"]);
                            }
                            else
                            {
                                txt_ncc.Text = Convert.ToString(dr["NCC_BoyScout_GirlGuide"]);
                            }
                            txt_subjectstudied.Text = Convert.ToString(dr["subjectstudied"]);
                            txt_qualified.Text = Convert.ToString(dr["Qualified_promotion"]);
                            txt_paidschool.Text = Convert.ToString(dr["Paid_dues"]);
                            txt_feecon.Text = Convert.ToString(dr["Fee_concession"]);
                            txt_totalnoofworkingdays.Text = Convert.ToString(dr["Totalnoofworkingdays"]);
                            txt_totalnoofworkingdayspresent.Text = Convert.ToString(dr["Totalnoofpresentdays"]);
                            txt_part1language.Text = Convert.ToString(dr["Part1Language"]);
                            txt_identification.Text = Convert.ToString(dr["idmark"]);
                            txt_serialno.Text = Convert.ToString(dr["Serial_no"]);
                            if (Convert.ToString(dr["Vocationorgeneral"]) == "0" || Convert.ToString(dr["Vocationorgeneral"]) == "True")
                            {
                                rdo_voc.Checked = true;
                                rdo_voc1.Checked = false;
                            }
                            else
                            {
                                rdo_voc.Checked = false;
                                rdo_voc1.Checked = true;
                            }
                            txt_mudiumofstudy.Text = Convert.ToString(dr["Medium_study"]);

                            txt_classteacher.Text = Convert.ToString(dr["Class_teacher_name"]); ;
                            txt_checkedby.Text = Convert.ToString(dr["checkedby_name"]); ;
                            txt_design.Text = Convert.ToString(dr["checkby_design"]); ;

                            #endregion
                        }
                        if (ddlAppFormat.SelectedValue == "3" || ddlAppFormat.SelectedValue == "4" || ddlAppFormat.SelectedValue == "5" || ddlAppFormat.SelectedValue == "6" || ddlAppFormat.SelectedValue == "7" || ddlAppFormat.SelectedValue == "8")
                        {
                            #region college
                            txt_regno.Enabled = false;
                            txt_regno.Text = regno;
                            txt_studname1.Text = studname;
                            txt_mothername1.Text = mothername;
                            txt_fathername1.Text = fathername;
                            txt_doAdmission.Text = Adm_Date;
                            txt_doLeaving.Text = dateOfLeaving;
                            if (dob.Trim() != "")
                            {
                                try
                                {
                                    DateTime dobdate = new DateTime();
                                    DateTime.TryParse(dob, out dobdate);
                                    ddldobdate1.SelectedIndex = ddldobdate1.Items.IndexOf(ddldobdate1.Items.FindByText(Convert.ToString((dobdate.ToString("dd"))).TrimStart('0')));
                                    ddldobMonth1.SelectedIndex = ddldobMonth1.Items.IndexOf(ddldobMonth1.Items.FindByValue(dobdate.ToString("MM")));
                                    ddldobYear1.SelectedIndex = ddldobYear1.Items.IndexOf(ddldobYear1.Items.FindByText(dobdate.ToString("yyyy")));
                                }
                                catch { }
                            }
                            string Attendance = string.Empty;
                            if (Convert.ToString(dr["Attendance_type"]) != "")
                            {
                                Attendance = Convert.ToString(dr["Attendance_type"]);
                                ddl_attendance.SelectedIndex = ddl_attendance.Items.IndexOf(ddl_attendance.Items.FindByValue(Attendance));
                            }

                            ddl_caste1.SelectedIndex = ddl_caste1.Items.IndexOf(ddl_caste1.Items.FindByValue(caste));
                            ddlcountry1.SelectedIndex = ddlcountry1.Items.IndexOf(ddlcountry1.Items.FindByValue(country));
                            ddlcoummunity1.SelectedIndex = ddlcoummunity1.Items.IndexOf(ddlcoummunity1.Items.FindByValue(community));
                            ddlreligion1.SelectedIndex = ddlreligion1.Items.IndexOf(ddlreligion1.Items.FindByValue(religion));


                            txt_remarks1.Text = remarks;
                            programeCompleted.Text = Convert.ToString(dr["program_completed"]);
                            txt_exammonthandyear.Text = Convert.ToString(dr["Last_exam_mon_year"]);
                            txt_migrationserielno.Text = Convert.ToString(dr["Migration_Sl_No"]);
                            txt_serial_no.Text = Convert.ToString(dr["Serial_no"]);
                            if (ds1.Tables[3].Rows.Count > 0)
                            {
                                if (Convert.ToString(ds1.Tables[3].Rows[0]["linkvalue"]) == "1")
                                {
                                    txt_serial_no.Text = Convert.ToString(dr["AutoSerialno"]);
                                    cb_serialnoSettings.Checked = true;
                                    txt_serial_no.Enabled = false;
                                }
                                else
                                    txt_serial_no.Enabled = true;
                            }
                            //   txt_leavinginstition.Text = Convert.ToString(dr["Last_Studied_Class"]);
                            //  txt_commencementofclass.Text = Convert.ToString(dr["commencementofclass"]);



                            string Student_yearduration = string.Empty;
                            int studbatchyear = 0;
                            string degree = Convert.ToString(ds1.Tables[1].Rows[0]["degree_code"]);
                            string collcode = collegecode1;
                            studbatchyear = Convert.ToInt32(d2.GetFunction("select batch_year,mode from registration where app_no=" + lbl_app_no1.Text.Trim()));
                            int mode = Convert.ToInt32(d2.GetFunction("select mode from registration where app_no=" + lbl_app_no1.Text.Trim()));
                            int duration = Convert.ToInt32(d2.GetFunction("select distinct duration/2 as year from degree where degree_code in ('" + degree + "') and college_code='" + collcode + "' "));

                            studbatchyear = Convert.ToString(mode) == "3" ? studbatchyear + 1 : studbatchyear;
                            duration = Convert.ToString(mode) == "3" ? duration - 1 : duration;
                            duration = studbatchyear + duration;
                            string pos = Convert.ToString(dr["periodofstudied"]);
                            if (pos == "")
                            {
                                pos = Convert.ToString(studbatchyear + " - " + duration);
                                txt_periodofstudied.Text = pos;
                            }
                            else { txt_periodofstudied.Text = pos; }


                            //  txt_periodofstudied.Text = Convert.ToString(dr["periodofstudied"]);
                            txt_admissiondate.Text = Convert.ToString(dr["Adm_Date"]);
                            //txt_aadharcardno.Text = Convert.ToString(dr["Aadharcard_no"]);

                            if (ddlAppFormat.SelectedValue == "5")
                            {
                                if (Convert.ToString(dr["ApartI_Language"]).ToUpper() != "" && !(edu_level.ToLower().Contains("m.phil") || edu_level.ToLower().Contains("mphil") || edu_level.ToLower().Contains("m phil") || edu_level.ToLower().Contains("pg")))
                                {
                                    txt_part1language1.Text = Convert.ToString(dr["ApartI_Language"]).ToUpper();
                                }

                                if (Convert.ToString(dr["Amedium_ins"]).ToUpper() != "" && !(edu_level.ToLower().Contains("m.phil") || edu_level.ToLower().Contains("mphil") || edu_level.ToLower().Contains("m phil")))
                                {
                                    txt_mudiumofstudy1.Text = Convert.ToString(dr["Amedium_ins"]).ToUpper();
                                }
                            }
                            else
                            {
                                txt_mudiumofstudy1.Text = Convert.ToString(dr["Medium_study"]);
                                txt_part1language1.Text = Convert.ToString(dr["Part1Language"]);
                            }
                            if (Convert.ToString(dr["Aadharcard_no"]).Trim() != "")
                            {
                                try
                                {
                                    string aadhar = Convert.ToString(dr["Aadharcard_no"]).Trim();
                                    if (aadhar.Length == 12)
                                    {
                                        txt_Aadharcardno.Text = aadhar.Substring(0, 4);
                                        txt_Aadharcardno2.Text = aadhar.Substring(4, 4);
                                        txt_Aadharcardno3.Text = aadhar.Substring(8, 4);
                                    }
                                }
                                catch { }
                            }
                            string dateoofissuemigration = Convert.ToString(dr["migration_date"]);
                            string commencementofclassdate = Convert.ToString(dr["dateofleaving"]);//commencement_date
                            string dateofissuecertificate = Convert.ToString(dr["dateofissuecertificate1"]);
                            string lastattendedclass = Convert.ToString(dr["laststudieddate1"]);
                            string tccertificateissuedate = Convert.ToString(dr["Transfer_cert_made"]);

                            ddl_dateoofissuemigration.SelectedIndex = ddl_dateoofissuemigration.Items.IndexOf(ddl_dateoofissuemigration.Items.FindByValue(dateoofissuemigration));
                            //ddl_commencementofclass.SelectedIndex = ddl_commencementofclass.Items.IndexOf(ddl_commencementofclass.Items.FindByText(commencementofclassdate));
                            //  ddl_dateofissuecertificate.SelectedIndex = ddl_dateofissuecertificate.Items.IndexOf(ddl_dateofissuecertificate.Items.FindByText(dateofissuecertificate));
                            txt_dateofissuecertificate.Text = dateofissuecertificate;
                            //  ddl_lastattendedclass.SelectedIndex = ddl_lastattendedclass.Items.IndexOf(ddl_lastattendedclass.Items.FindByText(lastattendedclass));
                            // ddl_tccertificateissuedate.SelectedIndex = ddl_tccertificateissuedate.Items.IndexOf(ddl_tccertificateissuedate.Items.FindByValue(tccertificateissuedate));
                            if (Convert.ToString(dr["Conduct_Character"]) != "")
                            {
                                ddl_generalconduct.SelectedIndex = ddl_generalconduct.Items.IndexOf(ddl_generalconduct.Items.FindByValue(Convert.ToString(dr["Conduct_Character"])));
                            }
                            #endregion
                        }
                    }
                }
                if (ddlAppFormat.SelectedValue == "5")
                {
                    format5.Visible = false;
                }

                if (ddlAppFormat.SelectedValue == "3" || ddlAppFormat.SelectedValue == "4" || ddlAppFormat.SelectedValue == "5" || ddlAppFormat.SelectedValue == "6" || ddlAppFormat.SelectedValue == "7" || ddlAppFormat.SelectedValue == "8")
                    pop_clg_tc.Visible = true;
                else
                    pop_studdetails.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void txt_admissionno_Onchange(object sender, EventArgs e)
    {
        try
        {
            if (txt_admissionno.Text.Trim() != "")
            {
                string rolladmitandstudname = Convert.ToString(txt_admissionno.Text.Trim()).Trim();
                q1 = "  select (select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Part1Language) and TextCriteria='Cplan')Part1Language,a.idmark, isnull(t.Vocationorgeneral,0)Vocationorgeneral, (select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Medium_study) and TextCriteria='PLang') Medium_study,a.TMR_NO,a.Certificate_No,CONVERT(varchar(10),a.Certificate_Date,103)Certificate_Date , r.app_no,r.Roll_Admit,r.Stud_Name,a.parent_name,a.mother,a.guardian_name,a.dob,a.citizen,a.caste,CONVERT(varchar(10),r.Adm_Date,103)Adm_Date,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.remarks))remarks,a.degree_code as First_Joinclass,r.degree_code Last_studiedclass, t.Annualexamination_result, t.noofattempts,t.subjectstudied,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.NCC_BoyScout_GirlGuide) and TextCriteria='NccSc')NCC_BoyScout_GirlGuide,t.Qualified_promotion,t.Paid_dues,t.General_conduct, t.Dateofapplcertificate,t.dateofissuecertificate,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.reasonforrelive))reasonforrelive,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.co_curricular) AND TextCriteria='cocur'))as co_curricular,dis_extra_Activity ,t.Fee_concession,t.Totalnoofworkingdays ,t.Totalnoofpresentdays,a.caste,a.religion,a.community, t.categorytype ,t.MedicalInspection ,t.Serial_no from applyn a,Registration r left join Tc_details t on r.App_No=t.App_no where a.app_no=r.App_No and r.Roll_Admit+'-'+r.Stud_Name='" + rolladmitandstudname + "'";
                q1 += "   select Dept_Name,Degree_Code from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id ";
                q1 += " select Affiliation_No,educationdistrict,disposal_no from collinfo where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(q1, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    if (ds1.Tables[2].Rows.Count > 0)
                    {
                        txt_affilicationno.Text = Convert.ToString(ds1.Tables[2].Rows[0]["Affiliation_No"]);
                        txt_educationdistrict.Text = Convert.ToString(ds1.Tables[2].Rows[0]["educationdistrict"]);
                        txt_disposelno.Text = Convert.ToString(ds1.Tables[2].Rows[0]["disposal_no"]);
                    }
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        txt_studname.Enabled = false;
                        txt_mothername.Enabled = false;
                        txt_fathername.Enabled = false;
                        txt_admdate.Enabled = false;
                        txt_serialno.Text = Convert.ToString(dr["Serial_no"]);
                        lbl_app_no.Text = Convert.ToString(dr["App_No"]); ;
                        string fathername = Convert.ToString(dr["parent_name"]);
                        string mothername = Convert.ToString(dr["mother"]);
                        string guardian = Convert.ToString(dr["guardian_name"]);
                        string dob = Convert.ToString(dr["dob"]);
                        string country = Convert.ToString(dr["citizen"]);
                        string caste = Convert.ToString(dr["caste"]);
                        string religion = Convert.ToString(dr["religion"]);
                        string community = Convert.ToString(dr["community"]);
                        string category = Convert.ToString(dr["categorytype"]);
                        ddlCaste.SelectedIndex = ddlCaste.Items.IndexOf(ddlCaste.Items.FindByValue(caste));
                        ddlcountry.SelectedIndex = ddlcountry.Items.IndexOf(ddlcountry.Items.FindByValue(country));
                        ddlcoummunity.SelectedIndex = ddlcoummunity.Items.IndexOf(ddlcoummunity.Items.FindByValue(community));
                        ddlreligion.SelectedIndex = ddlreligion.Items.IndexOf(ddlreligion.Items.FindByValue(religion));
                        ddl_catagory.SelectedIndex = ddl_catagory.Items.IndexOf(ddl_catagory.Items.FindByValue(category));

                        string Adm_Date = Convert.ToString(dr["Adm_Date"]);
                        string remarks = Convert.ToString(dr["remarks"]);
                        string Attempts = Convert.ToString(dr["noofattempts"]);
                        string studname = Convert.ToString(dr["stud_name"]);
                        string roll_admit = Convert.ToString(dr["roll_admit"]);
                        //txt_admissionno.Text = roll_admit + '-' + studname;
                        txt_studname.Text = studname;
                        txt_mothername.Text = mothername;
                        txt_fathername.Text = fathername;
                        txt_GuardianName.Text = guardian;

                        txt_tmrno.Text = Convert.ToString(dr["TMR_NO"]);
                        txt_CertificatenoH.Text = Convert.ToString(dr["Certificate_No"]);
                        string MedicalInspection = Convert.ToString(dr["MedicalInspection"]);
                        if (MedicalInspection.Trim() == "1" || MedicalInspection.Trim() == "True")
                        {
                            rdb_medical.Checked = true;
                            rdb_medical1.Checked = false;
                        }
                        else
                        {
                            rdb_medical.Checked = true;
                            rdb_medical1.Checked = false;
                        }
                        if (dob.Trim() != "")
                        {
                            try
                            {
                                DateTime dobdate = new DateTime();
                                DateTime.TryParse(dob, out dobdate);
                                ddldobdate.SelectedIndex = ddldobdate.Items.IndexOf(ddldobdate.Items.FindByText(Convert.ToString((dobdate.ToString("dd"))).TrimStart('0')));
                                ddldobMonth.SelectedIndex = ddldobMonth.Items.IndexOf(ddldobMonth.Items.FindByValue(dobdate.ToString("MM")));
                                ddldobYear.SelectedIndex = ddldobYear.Items.IndexOf(ddldobYear.Items.FindByText(dobdate.ToString("yyyy")));
                            }
                            catch { }
                        }
                        txt_admdate.Text = Adm_Date;
                        txt_remarks.Text = remarks;
                        txt_failsameclass.Text = Attempts;
                        txt_generalconduct.Text = Convert.ToString(dr["General_conduct"]);
                        DateTime cerapplidate = new DateTime(); DateTime cerissueidate = new DateTime();
                        DateTime.TryParse(Convert.ToString(dr["Dateofapplcertificate"]), out cerapplidate);
                        DateTime.TryParse(Convert.ToString(dr["dateofissuecertificate"]), out cerissueidate);
                        if (Convert.ToString(cerapplidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                            txt_applicationcerticate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                        else
                            txt_applicationcerticate.Text = Convert.ToString(cerapplidate.ToString("dd/MM/yyyy"));
                        if (Convert.ToString(cerissueidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                            txt_dateofissueofcertificate.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                        else
                            txt_dateofissueofcertificate.Text = Convert.ToString(cerissueidate.ToString("dd/MM/yyyy"));

                        DateTime ceritificatedateH_dt = new DateTime();
                        DateTime.TryParse(Convert.ToString(dr["Certificate_Date"]), out ceritificatedateH_dt);
                        if (Convert.ToString(ceritificatedateH_dt.ToString("dd/MM/yyyy")) == "01/01/0001")
                            txt_CertificatedateH.Text = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                        else
                            txt_CertificatedateH.Text = Convert.ToString(ceritificatedateH_dt.ToString("dd/MM/yyyy"));

                        DataView dv = new DataView();
                        ds1.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(dr["Last_studiedclass"]) + "'";
                        dv = ds1.Tables[1].DefaultView;
                        if (dv.Count > 0)
                        {
                            txt_laststudiedclass.Text = Convert.ToString(dv[0]["Dept_Name"]);
                        }
                        else { txt_laststudiedclass.Text = ""; }

                        ddl_joinclass.SelectedIndex = ddl_joinclass.Items.IndexOf(ddl_joinclass.Items.FindByValue(Convert.ToString(dr["First_Joinclass"])));
                        //DataView dv1 = new DataView();
                        //ds1.Tables[1].DefaultView.RowFilter = " Degree_Code='" + Convert.ToString(dr["First_Joinclass"]) + "'";
                        //dv1 = ds1.Tables[1].DefaultView;
                        //if (dv1.Count > 0)
                        //{
                        //    txt_joinclass.Text = Convert.ToString(dv1[0]["Dept_Name"]);
                        //}
                        //else { txt_joinclass.Text = ""; }
                        txt_leaving.Text = Convert.ToString(dr["reasonforrelive"]);
                        txt_schoolorboard.Text = Convert.ToString(dr["Annualexamination_result"]);
                        if (Convert.ToString(dr["co_curricular"]) != "")
                        {
                            txt_extraactivites.Text = Convert.ToString(dr["co_curricular"]);
                            //co_curricular
                            //rdbextraactivitesNo.Checked = false;
                            //rdbextraactivitesYes.Checked = true;
                            //DropDownList4.SelectedIndex = DropDownList4.Items.IndexOf(DropDownList4.Items.FindByValue(Convert.ToString(dr["co_curricular"])));
                            //txt_specifyNcc.Text = Convert.ToString(dr["dis_extra_Activity"]);
                            //DropDownList4.Style.Add("display", "block");
                            //txt_specifyNcc.Style.Add("display", "block");
                        }
                        else
                        {
                            txt_extraactivites.Text = " - ";
                            //rdbextraactivitesYes.Checked = false;
                            //rdbextraactivitesNo.Checked = true;
                        }
                        if (Convert.ToString(dr["NCC_BoyScout_GirlGuide"]) != "")
                            txt_ncc.Text = Convert.ToString(dr["NCC_BoyScout_GirlGuide"]);
                        else
                            txt_ncc.Text = " - ";
                        txt_subjectstudied.Text = Convert.ToString(dr["subjectstudied"]);
                        txt_qualified.Text = Convert.ToString(dr["Qualified_promotion"]);
                        txt_paidschool.Text = Convert.ToString(dr["Paid_dues"]);
                        txt_feecon.Text = Convert.ToString(dr["Fee_concession"]);
                        txt_totalnoofworkingdays.Text = Convert.ToString(dr["Totalnoofworkingdays"]);
                        txt_totalnoofworkingdayspresent.Text = Convert.ToString(dr["Totalnoofpresentdays"]);
                        txt_part1language.Text = Convert.ToString(dr["Part1Language"]);
                        txt_identification.Text = Convert.ToString(dr["idmark"]);
                        if (Convert.ToString(dr["Vocationorgeneral"]) == "0" || Convert.ToString(dr["Vocationorgeneral"]) == "True")
                        {
                            rdo_voc.Checked = true;
                            rdo_voc1.Checked = false;
                        }
                        else
                        {
                            rdo_voc.Checked = false;
                            rdo_voc1.Checked = true;
                        }
                        txt_mudiumofstudy.Text = Convert.ToString(dr["Medium_study"]);
                    }
                }
                pop_studdetails.Visible = true;
            }
            else { clear(); }
        }
        catch { }
    }

    protected void FpSpread1_Command(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpread1.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpread1.Sheets[0].ActiveColumn.ToString();
            if (actrow.Trim() == "0" && actcol.Trim() == "2")
            {
                if (FpSpread1.Sheets[0].RowCount > 0)
                {
                    int checkval = Convert.ToInt32(FpSpread1.Sheets[0].Cells[0, 2].Value);
                    if (checkval == 0)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 2].Value = 1;
                        }
                    }
                    if (checkval == 1)
                    {
                        for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
                        {
                            FpSpread1.Sheets[0].Cells[i, 2].Value = 0;
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            if (txt_admissionno.Text.Trim() != "")
            {
                string admissionno = txt_admissionno.Text;
                string nationality = "0";
                if (Convert.ToString(ddlcountry.SelectedItem.Text.ToString().ToUpper()) == "OTHERS")
                {
                    string national = Convert.ToString(txt_othernationality.Text.ToString().ToUpper());
                    nationality = gettextvalue("citi", national);
                }
                else
                {
                    if (Convert.ToString(ddlcountry.SelectedItem.Value) != "Select")
                    {
                        nationality = Convert.ToString(ddlcountry.SelectedItem.Value);
                    }
                }
                string admdate = txt_admdate.Text;
                string remarks = gettextvalue("remrk", txt_remarks.Text.ToUpper());
                string relive = gettextvalue("reliv", txt_leaving.Text);
                string classteacher = gettextvalue("ClsTe", txt_classteacher.Text.ToUpper());
                string checkedby = gettextvalue("ChkBy", txt_checkedby.Text.ToUpper());
                string checkbydesign = gettextvalue("ChkDe", txt_design.Text.ToUpper());
                string applicationcerticate = txt_applicationcerticate.Text;
                string issuecertificate = txt_dateofissueofcertificate.Text;
                DateTime certificatedateappl_dt = new DateTime();
                if (applicationcerticate.Trim() != "")
                {
                    string[] splitdate = applicationcerticate.Split('/');
                    certificatedateappl_dt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                }
                DateTime issuecertificate_dt = new DateTime();
                if (issuecertificate.Trim() != "")
                {
                    string[] splitdate1 = issuecertificate.Split('/');
                    issuecertificate_dt = Convert.ToDateTime(splitdate1[1] + "/" + splitdate1[0] + "/" + splitdate1[2]);
                }
                DateTime admdate_dt = new DateTime();
                if (admdate.Trim() != "")
                {
                    string[] splitdate2 = admdate.Split('/');
                    admdate_dt = Convert.ToDateTime(splitdate2[1] + "/" + splitdate2[0] + "/" + splitdate2[2]);
                }
                DateTime certificatedate_dt = new DateTime();
                if (txt_CertificatedateH.Text.Trim() != "")
                {
                    string[] splitdate3 = txt_CertificatedateH.Text.Split('/');
                    certificatedate_dt = Convert.ToDateTime(splitdate3[1] + "/" + splitdate3[0] + "/" + splitdate3[2]);
                }
                DateTime laststudieddate = new DateTime();
                if (txt_laststudieddate.Text.Trim() != "")
                {
                    string[] splitdate4 = txt_laststudieddate.Text.Split('/');
                    laststudieddate = Convert.ToDateTime(splitdate4[1] + "/" + splitdate4[0] + "/" + splitdate4[2]);
                }
                DateTime dobdate = new DateTime();
                string dd = Convert.ToString(ddldobdate.SelectedItem.Text).TrimStart('0');
                string mm = Convert.ToString(ddldobMonth.SelectedItem.Value);
                string yyyy = Convert.ToString(ddldobYear.SelectedItem.Text);
                DateTime.TryParse(mm + "/" + dd + "/" + yyyy, out dobdate);
                int community = 0;
                int religion = 0;
                int caste = 0;
                if (ddlreligion.SelectedItem.Text != "Select")
                {
                    if (ddlreligion.SelectedItem.Text != "Others" && ddlreligion.SelectedItem.Text.ToUpper() != "CHRISTIAN")
                    {
                        int.TryParse(Convert.ToString(ddlreligion.SelectedItem.Value), out religion);
                    }
                    else if (ddlreligion.SelectedItem.Text.ToUpper() == "CHRISTIAN")
                    {
                        int.TryParse(Convert.ToString(ddlreligion.SelectedItem.Value), out religion);
                    }
                    else
                    {
                        if (txt_otherreligion.Text.Trim() != "")
                        {
                            string relig = Convert.ToString(txt_otherreligion.Text.First().ToString().ToUpper() + txt_otherreligion.Text.Substring(1));
                            if (relig.Trim() != "")
                            {
                                int.TryParse((gettextvalue("relig", relig)), out religion);
                            }
                        }
                    }
                }
                if (ddlcoummunity.SelectedItem.Text != "Select")
                {
                    if (ddlcoummunity.SelectedItem.Text != "Others")
                        int.TryParse(Convert.ToString(ddlcoummunity.SelectedItem.Value), out community);
                    else
                    {
                        string comm = Convert.ToString(txtCommunity.Text.ToString().ToUpper());
                        int.TryParse(gettextvalue("comm", comm), out community);
                    }
                }
                if (ddlCaste.SelectedItem.Text != "Select")
                {
                    if (ddlCaste.SelectedItem.Text != "Others")
                    {
                        int.TryParse(Convert.ToString(ddlCaste.SelectedItem.Value), out caste);
                    }
                    else
                    {
                        string cast = Convert.ToString(txtCasteOther.Text.ToString().ToUpper());
                        int.TryParse(gettextvalue("caste", cast), out caste);
                    }
                }
                string nccscout = gettextvalue("NccSc", txt_ncc.Text.ToUpper());
                string extracocurr = ""; string specifyactivites = "";
                //if (rdbextraactivitesYes.Checked == true)
                //{
                //    if (DropDownList4.SelectedItem.Text != "Select")
                //    {
                //        if (DropDownList4.SelectedItem.Text != "Others")
                //        {
                //            extracocurr = Convert.ToString(DropDownList4.SelectedItem.Value);
                //            specifyactivites = Convert.ToString(txt_specifyNcc.Text);
                //        }
                //        else
                //        {
                //            string co_curricular1 = Convert.ToString(txt_extraactivites.Text.ToString().ToUpper());
                //            extracocurr = gettextvalue("cocur", co_curricular1);
                //            specifyactivites = Convert.ToString(txt_specifyNcc.Text).ToUpper();
                //        }
                //    }
                //    else if (DropDownList4.SelectedItem.Text.Trim() == "Select")
                //    {
                //        extracocurr = "0";
                //    }
                //}
                //else { extracocurr = "0"; }

                string co_curricular1 = Convert.ToString(txt_extraactivites.Text.ToString().ToUpper().Trim());
                extracocurr = gettextvalue("cocur", co_curricular1);

                string workingdays = txt_totalnoofworkingdays.Text;
                string presentdays = txt_totalnoofworkingdayspresent.Text;
                if (workingdays.Trim() == "")
                    workingdays = "0";
                if (presentdays.Trim() == "")
                    presentdays = "0";
                string part1language = gettextvalue("Cplan", txt_part1language.Text.ToString().ToUpper());
                string mediumofstudy = gettextvalue("PLang", txt_mudiumofstudy.Text.ToString().ToUpper());
                string vocgroup = "";
                if (rdo_voc.Checked == true)
                { vocgroup = "0"; }
                if (rdo_voc1.Checked == true)
                { vocgroup = "1"; }
                string medical = "";
                if (rdb_medical1.Checked == true)
                { medical = "0"; }
                if (rdb_medical.Checked == true)
                { medical = "1"; }

                //First_Joinclass='" + txt_joinclass.Text + "',Last_studiedclass='" + txt_laststudiedclass.Text + "',
                q1 = "  update Registration set Adm_Date='" + admdate_dt.ToString("MM/dd/yyyy") + "' where App_No='" + lbl_app_no.Text.Trim() + "'";
                q1 += "  update applyn set degree_code='" + Convert.ToString(ddl_joinclass.SelectedItem.Value) + "',dob='" + (dobdate.ToString("MM/dd/yyyy") == "01/01/0001" ? "" : dobdate.ToString("MM/dd/yyyy")) + "', idmark='" + txt_identification.Text + "',citizen='" + nationality + "',caste='" + caste + "',community='" + community + "',remarks='" + remarks + "', co_curricular='" + extracocurr + "',dis_extra_Activity='" + specifyactivites + "',religion='" + religion + "',Certificate_Date='" + Convert.ToString(certificatedate_dt.ToString("MM/dd/yyyy")) + "',TMR_NO='" + txt_tmrno.Text.ToUpper() + "',Certificate_No='" + txt_CertificatenoH.Text.ToUpper() + "' where App_No='" + lbl_app_no.Text + "'";//,SubCaste='" + subreligion + "'
                q1 += "  if exists(select app_no from Tc_details where app_no='" + lbl_app_no.Text.Trim() + "') update Tc_details set app_no='" + lbl_app_no.Text.Trim() + "', Annualexamination_result='" + txt_schoolorboard.Text.Trim() + "',noofattempts='" + txt_failsameclass.Text.Trim() + "',subjectstudied='" + txt_subjectstudied.Text.Trim() + "',NCC_BoyScout_GirlGuide='" + nccscout + "',Qualified_promotion='" + txt_qualified.Text.Trim() + "',Paid_dues='" + txt_paidschool.Text.Trim() + "',General_conduct='" + txt_generalconduct.Text.Trim() + "',Dateofapplcertificate='" + Convert.ToString(certificatedateappl_dt.ToString("MM/dd/yyyy")) + "', dateofissuecertificate='" + Convert.ToString(issuecertificate_dt.ToString("MM/dd/yyyy")) + "',reasonforrelive='" + relive.Trim() + "',Fee_concession='" + txt_feecon.Text.Trim() + "' ,Totalnoofworkingdays ='" + workingdays.Trim() + "',Totalnoofpresentdays='" + presentdays.Trim() + "' , Part1Language ='" + part1language + "',Vocationorgeneral='" + vocgroup + "',Medium_study='" + mediumofstudy + "',categorytype='" + ddl_catagory.SelectedItem.Value + "', MedicalInspection='" + medical + "',laststudieddate='" + Convert.ToString(laststudieddate.ToString("MM/dd/yyyy")) + "',Serial_no='" + txt_serialno.Text + "',Class_teacher_name='" + classteacher + "',checkedby_name='" + checkedby + "',checkby_design='" + checkbydesign + "' where App_No='" + lbl_app_no.Text.Trim() + "' else insert into Tc_details (App_no,Annualexamination_result,noofattempts,subjectstudied,NCC_BoyScout_GirlGuide,Qualified_promotion,Paid_dues,General_conduct,Dateofapplcertificate,dateofissuecertificate,reasonforrelive,Fee_concession,Totalnoofworkingdays,Totalnoofpresentdays,Part1Language,Vocationorgeneral,Medium_study,categorytype,MedicalInspection,laststudieddate,Serial_no,Class_teacher_name,checkedby_name,checkby_design)values('" + lbl_app_no.Text.Trim() + "','" + txt_schoolorboard.Text.Trim() + "','" + txt_failsameclass.Text.Trim() + "','" + txt_subjectstudied.Text.Trim() + "','" + nccscout + "','" + txt_qualified.Text.Trim() + "','" + txt_paidschool.Text.Trim() + "','" + txt_generalconduct.Text.Trim() + "','" + Convert.ToString(certificatedateappl_dt.ToString("MM/dd/yyyy")) + "', '" + Convert.ToString(issuecertificate_dt.ToString("MM/dd/yyyy")) + "','" + relive.Trim() + "','" + txt_feecon.Text.Trim() + "','" + workingdays.Trim() + "','" + presentdays.Trim() + "','" + part1language + "','" + vocgroup + "','" + mediumofstudy + "','" + ddl_catagory.SelectedItem.Value + "','" + medical + "','" + Convert.ToString(laststudieddate.ToString("MM/dd/yyyy")) + "','" + txt_serialno.Text + "','" + classteacher + "','" + checkedby + "','" + checkbydesign + "')  ";
                q1 += "  update collinfo set Affiliation_No='" + txt_affilicationno.Text.Trim() + "',educationdistrict='" + txt_educationdistrict.Text.Trim() + "',disposal_no=N'" + txt_disposelno.Text.Trim() + "' where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                int updat = d2.update_method_wo_parameter(q1, "text");
                if (updat != 0)
                {
                    lblalerterr.Text = "Updated Successfully";
                    alertpopwindow.Visible = true;
                    clear();
                    pop_studdetails.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = ex.ToString();
        }
    }

    protected void btn_print_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread1.SaveChanges();
            contentDiv.InnerHtml = ""; StringBuilder html = new StringBuilder();
            #region variable
            string collname = string.Empty; string value = string.Empty;
            string address1 = string.Empty;
            string address2 = string.Empty;
            string address3 = string.Empty;
            string address11 = string.Empty;
            string address22 = string.Empty;
            string address33 = string.Empty;
            string pincode = string.Empty;
            string affliated = string.Empty;
            string phone = string.Empty;
            string university = string.Empty;
            string email = string.Empty; string catagory = string.Empty;
            string fax = string.Empty; string classtoclass = string.Empty;
            string website = string.Empty;
            string state = string.Empty;
            string photo = string.Empty; string stdphoto = string.Empty;
            string district = string.Empty;
            string affiliationno = string.Empty;
            string slno = string.Empty;
            string studname = string.Empty;
            string monthofleaving = string.Empty;
            string mothername = string.Empty;
            string fathername = string.Empty;
            string dob = string.Empty;
            string nationality = string.Empty;
            string caste = string.Empty;
            string admissiondateclass = string.Empty;
            string laststudied = string.Empty;
            string lastexamresult = string.Empty;
            string attempts = string.Empty;
            string subjectstudied = string.Empty;
            string promationclass = string.Empty;
            string feespaid = string.Empty;
            string consession = string.Empty;
            string workingdays = string.Empty;
            string persentdays = string.Empty;
            string ncc = string.Empty;
            string extraactivity = string.Empty;
            string generalconduct = string.Empty;
            string dateofappcertificate = string.Empty;
            string dateofissue = string.Empty;
            string releavingreson = string.Empty;
            string remarks = string.Empty;
            string schoolcode = string.Empty;
            string admissionno = string.Empty;
            string classteachar = string.Empty;
            string Checkedby = string.Empty;
            string collegenamedistrict = string.Empty;
            string districtname = string.Empty;
            string disposlno = string.Empty;
            string nationalityandregion = string.Empty;
            string religion = string.Empty;
            string communititytypebc = string.Empty;
            string communititytypembc = string.Empty;
            string communititytypesc = string.Empty;
            string communititytypeconvert = string.Empty;
            string sex = string.Empty;
            string dobandwords = string.Empty;
            string identificationmark = string.Empty;
            string partonelang = string.Empty;
            string vacationalornot = string.Empty;
            string mediumofstudy = string.Empty;
            string medicalfit = string.Empty;
            string lastdate = string.Empty;
            string coursename = string.Empty;
            string batchyear = string.Empty;
            string degreecode = string.Empty;
            string applfromno = string.Empty;
            string regno = string.Empty;
            string tmrno = string.Empty;
            string certificateno = string.Empty;
            string certificatedate = string.Empty;
            string leavingtimeinstitution = string.Empty;
            string rollno = string.Empty;
            string affliatedby1 = string.Empty;
            string affliatedby2 = string.Empty;
            string affliatedby3 = string.Empty;
            string affliatedby = string.Empty;
            string BonafidePurpose = string.Empty;
            Dictionary<string, string> deptname_dic = new Dictionary<string, string>();
            Dictionary<string, string> deptnameandcourse_dic = new Dictionary<string, string>();
            Dictionary<string, string> coursename_dic = new Dictionary<string, string>();
            string strquery = "Select * from Collinfo where college_code=" + Convert.ToString(ddlcollege.SelectedItem.Value) + "";
            strquery += " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds1.Clear();
            ds1 = d2.select_method_wo_parameter(strquery, "Text");
            #endregion
            for (int i = 1; i < FpSpread1.Sheets[0].RowCount; i++)
            {
                value = Convert.ToString(FpSpread1.Sheets[0].Cells[i, 2].Value);
                if (value == "1")
                {
                    #region
                    string SerialNosettings = "0";
                    if (ds1.Tables[1].Rows.Count > 0 && ds1.Tables != null)
                    {
                        SerialNosettings = Convert.ToString(ds1.Tables[1].Rows[0]["linkvalue"]);
                    }
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        #region college
                        collname = ds1.Tables[0].Rows[0]["collname"].ToString().ToUpper();
                        address1 = ds1.Tables[0].Rows[0]["address1"].ToString().ToUpper();
                        address2 = ds1.Tables[0].Rows[0]["address2"].ToString().ToUpper();
                        address3 = ds1.Tables[0].Rows[0]["address3"].ToString().ToUpper();
                        address11 = ds1.Tables[0].Rows[0]["address1"].ToString();
                        address22 = ds1.Tables[0].Rows[0]["address2"].ToString();
                        address33 = ds1.Tables[0].Rows[0]["address3"].ToString();
                        pincode = ds1.Tables[0].Rows[0]["pincode"].ToString().ToUpper();
                        affliated = ds1.Tables[0].Rows[0]["affliatedby"].ToString().ToUpper();
                        district = ds1.Tables[0].Rows[0]["district"].ToString().ToUpper();
                        state = ds1.Tables[0].Rows[0]["State"].ToString().ToUpper();
                        university = ds1.Tables[0].Rows[0]["university"].ToString().ToUpper();
                        phone = ds1.Tables[0].Rows[0]["Phoneno"].ToString().ToUpper();
                        fax = ds1.Tables[0].Rows[0]["Faxno"].ToString().ToUpper();
                        email = ds1.Tables[0].Rows[0]["Email"].ToString();
                        website = ds1.Tables[0].Rows[0]["Website"].ToString();
                        schoolcode = ds1.Tables[0].Rows[0]["acr"].ToString().ToUpper();
                        districtname = Convert.ToString(ds1.Tables[0].Rows[0]["district"]).ToUpper();
                        collegenamedistrict = Convert.ToString(ds1.Tables[0].Rows[0]["educationdistrict"]).ToUpper();
                        affiliationno = Convert.ToString(ds1.Tables[0].Rows[0]["Affiliation_No"]).ToUpper();
                        disposlno = Convert.ToString(ds1.Tables[0].Rows[0]["disposal_no"]).ToUpper();
                        catagory = "(" + Convert.ToString(ds1.Tables[0].Rows[0]["category"]).ToUpper() + ")";
                        if (!string.IsNullOrEmpty(Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]).Trim()))
                        {
                            try
                            {
                                string[] affli = Convert.ToString(ds1.Tables[0].Rows[0]["affliatedby"]).Split('\\');
                                affliatedby1 = affli[0].Split(',')[0];
                                affliatedby2 = affli[2].Split(',')[0];
                                affliatedby3 = affli[1].Split(',')[0];
                                affliatedby = affliatedby1 + "<br>" + affliatedby2 + "<br>" + affliatedby3;
                            }
                            catch { }
                        }
                        #endregion

                        string app_no = Convert.ToString(FpSpread1.Sheets[0].GetTag(Convert.ToInt32(i), Convert.ToInt32(1))).Trim();
                        string degree = Convert.ToString(FpSpread1.Sheets[0].GetTag(Convert.ToInt32(i), Convert.ToInt32(2))).Trim();
                        q1 = "  select r.roll_no,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Medium_study) and TextCriteria='PLang' ))Medium_study, UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Part1Language) and TextCriteria='Cplan' ))partI_Language,UPPER(convert(varchar,(a.batch_year)) +' To ' + convert(varchar,(r.batch_year))) as batch_year, UPPER(a.TMR_NO)TMR_NO,UPPER(a.Certificate_No)Certificate_No,a.Certificate_Date,UPPER(r.reg_no)reg_no,UPPER(A.app_formno)app_formno,UPPER(idmark)idmark,case when sex='0' then 'MALE' when sex='1' then 'FEMALE' end sex,r.app_no,UPPER(r.Roll_Admit)Roll_Admit,UPPER(r.Stud_Name)Stud_Name,UPPER(a.parent_name)parent_name,UPPER(a.mother)mother,UPPER(a.guardian_name)guardian_name,UPPER(CONVERT(varchar(10), a.dob,103))dob,dob as dob1,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.citizen)))citizen,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.caste)))caste,CONVERT(varchar(10),r.Adm_Date,103)Adm_Date,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.remarks)))remarks,a.degree_code as First_Joinclass,r.degree_code Last_studiedclass, UPPER(t.Annualexamination_result)Annualexamination_result, t.noofattempts,UPPER(t.subjectstudied)subjectstudied,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.NCC_BoyScout_GirlGuide) and TextCriteria='NccSc'))NCC_BoyScout_GirlGuide,UPPER(t.Qualified_promotion)Qualified_promotion, UPPER(t.Paid_dues)Paid_dues,UPPER(t.General_conduct)General_conduct,UPPER( t.Dateofapplcertificate)Dateofapplcertificate,convert(varchar(10), t.dateofissuecertificate,101)dateofissuecertificate,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.reasonforrelive)))reasonforrelive,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.co_curricular) AND TextCriteria='cocur')) co_curricular,UPPER(dis_extra_Activity)dis_extra_Activity ,UPPER(t.Fee_concession)Fee_concession, UPPER(t.Totalnoofworkingdays)Totalnoofworkingdays ,UPPER(t.Totalnoofpresentdays)Totalnoofpresentdays,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.religion)))religion ,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.caste)))caste  ,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.community)))community, t.categorytype,case when ISNULL(t.MedicalInspection,0)='0' then 'NO'  when ISNULL(t.MedicalInspection,0)='1' then 'YES' end MedicalInspection,case when ISNULL(Vocationorgeneral,0)=0 then 'GENERAL EDUCATION' when ISNULL(Vocationorgeneral,0)=1 then 'VOCATIONAL EDUCATION' end Vocationorgeneral, convert(varchar(10), t.laststudieddate,101)laststudieddate ,t.Serial_no,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Class_teacher_name) and TextCriteria='ClsTe'))Class_teacher_name,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.checkedby_name) and TextCriteria='ChkBy'))checkedby_name,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.checkby_design) and TextCriteria='ChkDe'))checkby_design,r.Current_Semester ,t.program_completed,t.Last_exam_mon_year,t.Migration_Sl_No,(select mastervalue from CO_MasterValues where convert(nvarchar(100), MasterCode)=t.General_conduct and MasterCriteria ='General conduct') as Conduct_Character,Last_Studied_Class,commencementofclass, Medium_study, (select mastervalue from CO_MasterValues where  convert(varchar(100), MasterCode)=convert(varchar(100),t.migration_date) and MasterCriteria ='Tc Date') migration_date, (select mastervalue from CO_MasterValues where  convert(varchar(100), MasterCode)=convert(varchar(100),t.commencement_date) and MasterCriteria ='Tc Date') commencement_date,  r.Reg_No,  periodofstudied,(select mastervalue from CO_MasterValues where  convert(varchar(100), MasterCode)=convert(varchar(100),t.Transfer_cert_made) and MasterCriteria ='Tc Date') Transfer_cert_made,a.Aadharcard_no,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Attendance_type) and TextCriteria='ATTYP'))Attendance_type ,r.Batch_Year as RegBatch_Year,r.DelFlag,r.mode,r.tcserialNo as AutoSerialno,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.partI_Language)))as ApartI_Language,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.medium_ins)))as Amedium_ins ,convert(varchar(10), dateofleaving,103)dateofleaving,r.CC,t.BonafidePurpose from applyn a,Registration r left join Tc_details t on r.App_No=t.App_no where a.app_no=r.App_No and r.app_no='" + app_no + "'";
                        q1 += " select Photo from StdPhoto where app_no='" + app_no + "'";
                        q1 += " select distinct duration/2 as year,'0' type,degree_code,'' batch_year from degree where degree_code in ('" + degree + "') and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' union select distinct NDurations/2 as year,'1' type,degree_code,convert(varchar, batch_year)batch_year from Ndegree as degree where degree_code in ('" + degree + "') and batch_year='" + Convert.ToString(ddl_batch.SelectedItem.Value) + "' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                        q1 += " select CertAcrNo,RunningSerialNo from TEmCertSerialSettings WHERE CertificateName='Transfer Certificate' and College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                        ds2.Clear();
                        ds2 = d2.select_method_wo_parameter(q1, "text");
                        if (ds2.Tables[0].Rows.Count > 0)
                        {
                            foreach (DataRow dr in ds2.Tables[0].Rows)
                            {
                                #region values

                                string serialno = string.Empty;
                                int RunningSerialNo = 0;
                                if (SerialNosettings.Trim() == "1")
                                {
                                    string Acr = string.Empty;
                                    if (ds2.Tables[3].Rows.Count > 0 && ds2.Tables != null)
                                    {
                                        Acr = Convert.ToString(ds2.Tables[3].Rows[0]["CertAcrNo"]);
                                        int.TryParse(Convert.ToString(ds2.Tables[3].Rows[0]["RunningSerialNo"]), out RunningSerialNo);
                                        RunningSerialNo++;
                                        serialno = Acr + Convert.ToString(RunningSerialNo);
                                    }
                                    if (string.IsNullOrEmpty(Convert.ToString(dr["AutoSerialno"]).Trim()))
                                    {
                                        string serialQry = " update Registration set tcserialNo='" + RunningSerialNo + "' where App_No='" + app_no + "'";
                                        serialQry += " update TEmCertSerialSettings set RunningSerialNo='" + RunningSerialNo + "' where  CertificateName='Transfer Certificate' and College_Code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";

                                        d2.update_method_wo_parameter(serialQry, "text");
                                    }
                                    else
                                        serialno = Acr + Convert.ToString(dr["AutoSerialno"]);
                                }
                                else
                                {
                                    serialno = Convert.ToString(dr["Serial_no"]).ToUpper();
                                }
                                studname = Convert.ToString(dr["Stud_Name"]).ToUpper();
                                fathername = Convert.ToString(dr["parent_name"]).ToUpper();
                                mothername = Convert.ToString(dr["mother"]).ToUpper();
                                string guardian = Convert.ToString(dr["guardian_name"]).ToUpper();
                                dob = Convert.ToString(dr["dob"]).ToUpper();
                                dobandwords = dob + " (" + dateinwords(dob).ToUpper() + ")";
                                //string F5dobwords = "(" + dateinwords(dob).ToUpper() + ")";

                                DateTime dobdate = new DateTime();
                                DateTime.TryParse(Convert.ToString(dr["dob1"]).Trim(), out dobdate);
                                string F5dobwords = "(" + DateToText(dobdate) + ")";
                                F5dobwords = (F5dobwords == "()") ? " - " : F5dobwords;
                                if (dobandwords.Trim() == "()")
                                    dobandwords = " - ";
                                regno = Convert.ToString(dr["reg_no"]).ToUpper();
                                rollno = Convert.ToString(dr["roll_no"]).ToUpper();
                                applfromno = Convert.ToString(dr["app_formno"]).ToUpper();
                                nationality = Convert.ToString(dr["citizen"]).ToUpper() == "" ? " - " : Convert.ToString(dr["citizen"]).ToUpper();
                                //caste = Convert.ToString(dr["caste"]).ToUpper() == "" ? " - " : Convert.ToString(dr["caste"]).ToUpper();
                                caste = Convert.ToString(dr["caste"]).ToUpper() == "" ? "Refer Community Certificate" : "Refer Community Certificate";  //modified by Mullai
                                admissionno = Convert.ToString(dr["Roll_Admit"]).ToUpper();
                                lastexamresult = Convert.ToString(dr["Annualexamination_result"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Annualexamination_result"]).ToUpper();
                                attempts = Convert.ToString(dr["noofattempts"]).ToUpper();
                                //Rajkumar 21-12-2017
                                string newroll = string.Empty;
                                if (!string.IsNullOrEmpty(rollno))
                                {
                                    string output = rollno.Substring(rollno.Length - 3);
                                    string proOutput = string.Empty;
                                    if (rollno.Length > 3)
                                        proOutput = rollno.Substring(rollno.Length - 4);

                                    if (!string.IsNullOrEmpty(output) || !string.IsNullOrEmpty(proOutput))
                                    {
                                        if (output == "DIS")
                                        {
                                            newroll = rollno.Remove(rollno.Length - 3);
                                            rollno = newroll;
                                        }
                                        if (proOutput == "PROL")
                                        {
                                            newroll = rollno.Remove(rollno.Length - 4);
                                            rollno = newroll;
                                        }
                                    }
                                }
                                //
                                if (attempts.Trim() == "" || attempts.Trim() == "0")
                                    attempts = " - ";
                                releavingreson = Convert.ToString(dr["reasonforrelive"]).ToUpper() == "" ? " - " : Convert.ToString(dr["reasonforrelive"]).ToUpper();
                                religion = Convert.ToString(dr["religion"]).ToUpper();
                                nationalityandregion = nationality + ", " + religion + " & " + caste;
                                if (nationalityandregion.Trim() == ",&")
                                    nationalityandregion = " - ";
                                sex = Convert.ToString(dr["sex"]).ToUpper();
                                subjectstudied = Convert.ToString(dr["subjectstudied"]).ToUpper() == "" ? " - " : Convert.ToString(dr["subjectstudied"]).ToUpper();
                                promationclass = Convert.ToString(dr["Qualified_promotion"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Qualified_promotion"]).ToUpper();
                                feespaid = Convert.ToString(dr["Paid_dues"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Paid_dues"]).ToUpper();
                                consession = Convert.ToString(dr["Fee_concession"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Fee_concession"]).ToUpper();
                                workingdays = Convert.ToString(dr["Totalnoofworkingdays"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Totalnoofworkingdays"]).ToUpper();
                                persentdays = Convert.ToString(dr["Totalnoofpresentdays"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Totalnoofpresentdays"]).ToUpper();
                                ncc = Convert.ToString(dr["NCC_BoyScout_GirlGuide"]).ToUpper() == "" ? " - " : Convert.ToString(dr["NCC_BoyScout_GirlGuide"]).ToUpper();
                                identificationmark = Convert.ToString(dr["idmark"]).ToUpper() == "" ? " - " : Convert.ToString(dr["idmark"]).ToUpper();
                                BonafidePurpose = Convert.ToString(dr["BonafidePurpose"]) == "" ? " - " : Convert.ToString(dr["BonafidePurpose"]);
                                if (ddlAppFormat.SelectedValue == "5")
                                {

                                    partonelang = Convert.ToString(dr["ApartI_Language"]).ToUpper() == "" ? " ENGLISH " : Convert.ToString(dr["ApartI_Language"]).ToUpper();
                                    mediumofstudy = Convert.ToString(dr["Amedium_ins"]).ToUpper() == "" ? " ENGLISH " : Convert.ToString(dr["Amedium_ins"]).ToUpper();
                                }
                                else
                                {
                                    partonelang = Convert.ToString(dr["partI_Language"]).ToUpper() == "" ? " - " : Convert.ToString(dr["partI_Language"]).ToUpper();
                                    mediumofstudy = Convert.ToString(dr["Medium_study"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Medium_study"]).ToUpper();
                                }

                                batchyear = Convert.ToString(dr["batch_year"]);
                                degreecode = Convert.ToString(dr["Last_studiedclass"]);
                                double duration = 0;
                                if (ds2.Tables[2].Rows.Count > 0)
                                {
                                    string s = "degree_code='" + degreecode + "'  and batch_year='" + Convert.ToString(dr["RegBatch_Year"]) + "' and type='1'";
                                    double.TryParse(Convert.ToString(ds2.Tables[2].Compute("Sum(year)", "degree_code='" + degreecode + "'  and batch_year='" + Convert.ToString(dr["RegBatch_Year"]) + "' and type='1'")), out duration);
                                    if (duration == 0)
                                    {
                                        double.TryParse(Convert.ToString(ds2.Tables[2].Compute("Sum(year)", "degree_code='" + degreecode + "' and type='0'")), out duration);
                                    }
                                }
                                string Student_yearduration = string.Empty;
                                int studbatchyear = 0;
                                int.TryParse(Convert.ToString(dr["RegBatch_Year"]), out studbatchyear);
                                studbatchyear = Convert.ToString(dr["mode"]) == "3" ? studbatchyear + 1 : studbatchyear;
                                duration = Convert.ToString(dr["mode"]) == "3" ? duration - 1 : duration;

                                string discontinueYear = string.Empty;
                                if (Convert.ToString(dr["DelFlag"]) == "1")
                                {
                                    if (txt_periodofstudied.Text.Trim() != "")
                                    {
                                        Student_yearduration = Convert.ToString(txt_periodofstudied.Text.Trim());
                                    }
                                    else
                                    {
                                        discontinueYear = d2.GetFunction("select datepart(yyyy,discontinue_date)discontinue_date from discontinue where app_no='" + app_no + "'");
                                        Student_yearduration = Convert.ToString(studbatchyear + " - " + discontinueYear);
                                    }
                                }
                                else
                                    Student_yearduration = Convert.ToString(studbatchyear + " - " + (studbatchyear + duration));

                                if (ncc.Trim() == "")
                                    ncc = " - ";
                                medicalfit = Convert.ToString(dr["MedicalInspection"]).ToUpper() == "" ? " - " : Convert.ToString(dr["MedicalInspection"]).ToUpper(); ;
                                extraactivity = (Convert.ToString(dr["co_curricular"])).ToUpper() == "" ? " - " : Convert.ToString(dr["co_curricular"]).ToUpper(); ;// +" - " + Convert.ToString(dr["dis_extra_Activity"]).ToUpper();
                                generalconduct = Convert.ToString(dr["General_conduct"]).ToUpper() == "" ? " - " : Convert.ToString(dr["General_conduct"]).ToUpper();
                                DateTime cerapplidate = new DateTime();
                                DateTime cerissueidate = new DateTime();
                                DateTime ceritificatedate_dt = new DateTime();
                                DateTime laststudieddate = new DateTime();
                                DateTime admissionDateDt = new DateTime();
                                DateTime.TryParse(Convert.ToString(dr["Certificate_Date"]), out ceritificatedate_dt);
                                DateTime.TryParse(Convert.ToString(dr["Dateofapplcertificate"]), out cerapplidate);
                                DateTime.TryParse(Convert.ToString(dr["dateofissuecertificate"]), out cerissueidate);
                                DateTime.TryParse(Convert.ToString(dr["laststudieddate"]), out laststudieddate);
                                string Adm_Date = Convert.ToString(dr["Adm_Date"]);
                                DateTime.TryParse(Adm_Date, out admissionDateDt);
                                if (Convert.ToString(ceritificatedate_dt.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(ceritificatedate_dt.ToString("dd/MM/yyyy")) == "01/01/0001")
                                    certificatedate = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                else
                                    certificatedate = Convert.ToString(ceritificatedate_dt.ToString("dd/MM/yyyy"));
                                if (Convert.ToString(cerapplidate.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(cerapplidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                                    dateofappcertificate = Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                else
                                    dateofappcertificate = Convert.ToString(cerapplidate.ToString("dd/MM/yyyy"));

//added by Mullai
                                string dateofissue1 = d2.GetFunction("select CONVERT(varchar(10),a.LastTCDate,103)as TCIsssuedDate from applyn a,Registration r where r.App_No=a.app_no and r.Reg_No='" + regno + "'");
                          if (!string.IsNullOrEmpty(dateofissue1))
                          {
                              dateofissue = dateofissue1;
                          }
                          else if (Convert.ToString(cerissueidate.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(cerissueidate.ToString("dd/MM/yyyy")) == "01/01/0001")
                          {
                              dateofissue = " - ";
                          }
                          else
                              dateofissue = Convert.ToString(cerissueidate.ToString("dd/MM/yyyy"));
                                //
                                //Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                         // else
                                   // dateofissue = Convert.ToString(cerissueidate.ToString("dd/MM/yyyy"));
                               // dateofissue = d2.GetFunction("select a.LastTCDate from applyn a,Registration r where r.App_No=a.app_no and r.Reg_No='"+regno+"'"); 
                                if (Convert.ToString(laststudieddate.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(laststudieddate.ToString("dd/MM/yyyy")) == "01/01/0001")
                                    lastdate = " - ";//Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy"));
                                else
                                    lastdate = Convert.ToString(laststudieddate.ToString("dd/MM/yyyy"));

                                if (Convert.ToString(laststudieddate.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(laststudieddate.ToString("dd/MM/yyyy")) == "01/01/0001")
                                    monthofleaving = " - ";
                                else
                                    monthofleaving = Convert.ToString(laststudieddate.ToString("MMM - yyyy"));
                                string monthofadmission = admissionDateDt.ToString("MMM - yyyy");


                                remarks = Convert.ToString(dr["remarks"]) == "" ? " -- " : Convert.ToString(dr["remarks"]).ToUpper();
                                string admClass = "";
                                if (deptname_dic.ContainsKey(Convert.ToString(dr["First_Joinclass"])))
                                {
                                    admClass = deptname_dic[Convert.ToString(dr["First_Joinclass"])];
                                }
                                else
                                {
                                    admClass = d2.GetFunction("select Dept_Name from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code='" + Convert.ToString(dr["First_Joinclass"]) + "'");
                                    if (admClass.Trim() != "0")
                                        deptname_dic.Add(Convert.ToString(dr["First_Joinclass"]), admClass);
                                }
                                string studied = "";
                                if (deptnameandcourse_dic.ContainsKey(Convert.ToString(dr["Last_studiedclass"])))
                                {
                                    string deptnamecourse = deptnameandcourse_dic[Convert.ToString(dr["Last_studiedclass"])];
                                    string[] dept = deptnamecourse.Split('$');
                                    if (dept.Length > 1)
                                    {
                                        laststudied = Convert.ToString(dept[0]);
                                        studied = Convert.ToString(dept[0]);
                                        laststudied = laststudied + " ( " + romanLetter(laststudied) + " )";
                                        coursename = Convert.ToString(dept[1]);
                                    }
                                }
                                else
                                {
                                    string deptnamecourse = d2.GetFunction("select  Dept_Name+'$'+Course_Name from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and d.Degree_Code='" + Convert.ToString(dr["Last_studiedclass"]) + "'");
                                    string[] dept = deptnamecourse.Split('$');
                                    if (dept.Length > 1)
                                    {
                                        laststudied = Convert.ToString(dept[0]);
                                        studied = Convert.ToString(dept[0]);
                                        laststudied = laststudied + " ( " + romanLetter(laststudied) + " )";
                                        coursename = Convert.ToString(dept[1]);
                                    }
                                    if (laststudied.Trim() != "0")
                                        deptnameandcourse_dic.Add(Convert.ToString(dr["Last_studiedclass"]), deptnamecourse);
                                }
                                classtoclass = Convert.ToString(admClass).ToUpper() + " TO " + studied;
                                string courseofstudy = Convert.ToString(dr["Vocationorgeneral"]) == "" ? " - " : Convert.ToString(dr["Vocationorgeneral"]).ToUpper();
                                string collegename = Convert.ToString(ddlcollege.SelectedItem.Text);
                                admissiondateclass = Convert.ToString(dr["Adm_Date"]) + " - " + Convert.ToString(admClass).ToUpper() + " (" + dateinwords(Convert.ToString(dr["Adm_Date"])).ToUpper() + ")";
                                string admissionclasswoword = "(" + Convert.ToString(dr["Adm_Date"]) + " - " + Convert.ToString(admClass).ToUpper() + ")";
                                string DenotifiedComm = "";
                                communititytypesc = " - ";
                                communititytypebc = " - ";
                                communititytypembc = " - ";
                                communititytypeconvert = " - ";
                                DenotifiedComm = " - ";
                                classteachar = Convert.ToString(dr["Class_teacher_name"]) == "" ? " - " : Convert.ToString(dr["Class_teacher_name"]);
                                Checkedby = Convert.ToString(dr["checkedby_name"]) == "" ? " - " : Convert.ToString(dr["checkedby_name"]);
                                string checkedbydesign = Convert.ToString(dr["checkby_design"]) == "" ? " - " : Convert.ToString(dr["checkby_design"]);
                                string communitity = Convert.ToString(dr["community"]).Trim().ToUpper();
                                if (Convert.ToString(dr["categorytype"]).Trim() != "" || Convert.ToString(dr["categorytype"]).Trim() != "0")
                                {
                                    string type = Convert.ToString(dr["categorytype"]).Trim();
                                    if (type == "1")
                                        communititytypesc = "YES";
                                    else if (type == "2")
                                        communititytypebc = "YES";
                                    else if (type == "3")
                                        communititytypembc = "YES";
                                    else if (type == "4")
                                        communititytypeconvert = "YES";
                                    else if (type == "5")
                                        DenotifiedComm = "YES";
                                }
                                string sem = string.Empty;
                                if (Convert.ToString(dr["CC"]).Trim() == "1" || Convert.ToString(dr["CC"]).Trim().ToUpper() == "TRUE")
                                {
                                    int currentsem = 0;
                                    int.TryParse(Convert.ToString(dr["Current_Semester"]).Trim(), out currentsem);
                                    sem = Convert.ToString(currentsem - 1);
                                }
                                else
                                    sem = Convert.ToString(dr["Current_Semester"]).Trim();

                                string year = rs.returnYearforSem(sem);

                                // string Studentyearduration = string.Empty;
                                int studbyear = 0;
                                string degre = Convert.ToString(ds2.Tables[2].Rows[0]["degree_code"]);
                                string collcode = collegecode1;
                                studbyear = Convert.ToInt32(d2.GetFunction("select batch_year,mode from registration where app_no=" + app_no));
                                int mode = Convert.ToInt32(d2.GetFunction("select mode from registration where app_no=" + app_no));
                                int durtion = Convert.ToInt32(d2.GetFunction("select distinct duration/2 as year from degree where degree_code in ('" + degre + "') and college_code='" + collcode + "' "));

                                studbyear = Convert.ToString(mode) == "3" ? studbyear + 1 : studbyear;
                                durtion = Convert.ToString(mode) == "3" ? durtion - 1 : durtion;
                                durtion = studbyear + durtion;
                                //string pos = Convert.ToString(dr["periodofstudied"]);
                                //if (pos == "")
                                //{
                                //    pos = Convert.ToString(studbatchyear + " - " + duration);
                                //    txt_periodofstudied.Text = pos;
                                //}
                                //else { txt_periodofstudied.Text = pos; }




                                string yearLetters = returnYearfornum(year) + " ";
                                string programcompleteddb = year + " - Year " + coursename + " DEGREE PROGRAMME IN " + studied + " Course Completed ";
                                leavingtimeinstitution = Convert.ToString(dr["Last_Studied_Class"]) == "" ? " - " : Convert.ToString(dr["Last_Studied_Class"]).ToUpper();

                                string commencementofclassdate = Convert.ToString(dr["dateofleaving"]) + " " + Convert.ToString(dr["commencementofclass"]); //commencement_date
                                commencementofclassdate = (commencementofclassdate == " " ? " - " : commencementofclassdate);
                                //string lastattendedclassdate = Convert.ToString(dr["laststudieddate1"]);
                                string Conduct_Character = Convert.ToString(dr["Conduct_Character"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Conduct_Character"]).ToUpper();
                                string Transfer_cert_made = Convert.ToString(dr["Transfer_cert_made"]) == "" ? " - " : Convert.ToString(dr["Transfer_cert_made"]);
                                string programcommpleted = Convert.ToString(dr["program_completed"]) == "" ? " - " : Convert.ToString(dr["program_completed"]);
                                string periodofstudy = Convert.ToString(dr["periodofstudied"]).ToUpper() == "" ? Convert.ToString(studbyear + " - " + durtion) : Convert.ToString(dr["periodofstudied"]);
                                string Last_exam_mon_year = Convert.ToString(dr["Last_exam_mon_year"]) == "" ? " - " : Convert.ToString(dr["Last_exam_mon_year"]);
                                string migration_date = Convert.ToString(dr["migration_date"]) == "" ? " - " : Convert.ToString(dr["migration_date"]);
                                string migraslno = Convert.ToString(dr["Migration_Sl_No"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Migration_Sl_No"]);
                                string Aadharcard_no = Convert.ToString(dr["Aadharcard_no"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Aadharcard_no"]);
                               // string attendance = Convert.ToString(dr["Attendance_type"]).ToUpper() == "" ? " - " : Convert.ToString(dr["Attendance_type"]);
                                string attendance = Convert.ToString(dr["Attendance_type"]).ToUpper() == "" ? " Regular " : Convert.ToString(dr["Attendance_type"]);  //modified by Mullai
                                string examflag = Convert.ToString(dr["DelFlag"]) == "1" ? " DISCONTINUED " : "Refer Mark Statements";  //modified by Mullai     
                                string Lateral = Convert.ToString(dr["mode"]) == "3" ? " (SECOND YEAR LATERAL ENTRY)" : "";
                                string lateralOrRegular = string.Empty;
                                if (coursename.ToUpper().Trim() == "MCA" || coursename.ToLower().Trim() == "mca" || coursename.ToUpper().Trim() == "M.C.A.")
                                {
                                    if (Convert.ToString(dr["mode"]) == "3")
                                    {

                                        if (ddlAppFormat.SelectedValue == "5")  //added by Mullai 
                                        {
                                            lateralOrRegular = "(SECOND YEAR LATERAL ENTRY)"; //jamal
                                        }
                                        else
                                        {
                                            lateralOrRegular = " (LATERAL ENTRY) ";
                                        }
                                    }
                                }
                                    //else if (Convert.ToString(dr["mode"]) == "1") { lateralOrRegular = " (REGULAR) "; }
                                    //  }

                                #endregion
                                    #region Photo
                                    byte[] photoid = new byte[0];
                                    if (ds1.Tables[0].Rows.Count > 0)
                                    {
                                        if (ds1.Tables[0].Rows[0]["logo1"] != null && Convert.ToString(ds1.Tables[0].Rows[0]["logo1"]) != "")
                                        {
                                            photoid = (byte[])(ds1.Tables[0].Rows[0]["logo1"]);
                                            if (photoid.Length > 0)
                                            {
                                                photo = "'data:image/png;base64," + Convert.ToBase64String(photoid) + "'";
                                            }
                                        }
                                    }
                                    byte[] std_photo = new byte[0];
                                    if (ds2.Tables[1].Rows.Count > 0)
                                    {
                                        if (ds2.Tables[1].Rows[0]["Photo"] != null && Convert.ToString(ds2.Tables[1].Rows[0]["Photo"]) != "")
                                        {
                                            std_photo = (byte[])(ds2.Tables[1].Rows[0]["Photo"]);
                                            if (std_photo.Length > 0)
                                            {
                                                stdphoto = "'data:image/png;base64," + Convert.ToBase64String(std_photo) + "'";
                                            }
                                        }
                                    }
                                    #endregion
                                    if (ddlAppFormat.SelectedValue == "0")
                                    {
                                        #region Cbse
                                        //html.Append("<div style='height: 1200px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'><table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'><tr><td> <img src=" + photo + " alt='' style='height: 150px; width: 150px;' /></td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: XX-Large;font-weight:bold;'>" + collname + "</span> <br><span style='font-size: medium;'>" + address11 + " , " + address22 + " , " + address33 + " - " + pincode + "<br>" + " Phone No: " + phone + " , " + " Fax No: " + fax + "<br>Email: " + email + "&nbsp;&nbsp;Website: " + website + "  </span><br><span style='font-size: 10px; font-family: Times New Roman; font-weight: bold;'></span></tr></table><table style='width: 700px;style='font-size: 10px;'><tr><br><td style='align: left;'>Affiliation No :" + affiliationno + "</td><td></td><td style='align: right;' align='left'>School Code : " + schoolcode + "</td>  <td> </td>     <td>    </td>    <td>&nbsp;&nbsp;        Admission No: " + admissionno + "    </td>    <td>        </td>    </tr> <tr><td></td><td></td><td></td><td></td><br><td style='align: right;' align=left' colspan='2'>         &nbsp;</td><td style='font-weight: bold'> &nbsp;</td> </tr> <tr>    <td style=' font-family: Arial; '         align='center' colspan='7'><div id='squre' style='border-style: solid; border-width: thin;  width: 250px; height: 20px;font-weight:bold; font-size: Large;' ><center> TRANSFER CERTIFICATE </center></div></td> </tr></table><br><table style='width: 750px;' cellpadding='4' cellspacing='4'> <tr><td align='left' width='380'> 1. Name of the Pupil</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + studname + "</td> </tr> <tr><td align='left' width='380'> 2. Mother's Name</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + mothername + "</td> </tr> <tr><td align='left' width='380'> 3. Father's Name / Guardian's Name</td><td style='font-weight: bold width='10'> :</td><td style='font-weight: bold'> " + fathername + "</td> </tr> <tr><td align='left' width='400'> 4. Date of birth (in Christian Era) According to </br>  &nbsp;&nbsp;&nbsp;&nbsp;Admission & Withdrawal Register (in figures)</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + dobandwords + "</td> </tr> <tr><td align='left' width='380'> 5. Nationality</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + nationality + "</td> </tr><tr><td align='left' width='380'>  6. Whether the candidate belongs to Schedule Caste or<br /> &nbsp;&nbsp;&nbsp;&nbsp; Schedule Tribe or OBC</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + communitity + "</td> </tr> <tr><td align='left' width='380'> 7. Date of first admission in the School with Class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + admissionclasswoword + "</td> </tr> <tr><td align='left' width='380'> 8. Class in which the pupil last studied (in figures)</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + studied + "</td> </tr> <tr><td align='left' width='380'> 9. School / Board Annual Examination last taken with <br>&nbsp;&nbsp;&nbsp;&nbsp; result</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + lastexamresult + "</td> </tr> <tr><td align='left' width='380'>  10. Whether failed, if so once / twice in the same class </td> <td style='font-weight: bold' width='10'> : </td> <td style='font-weight: bold'>  " + attempts + " </td></tr><tr><td align='left' width='380'> 11. Subject Studied</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + subjectstudied + "</td> </tr> <tr><td align='left' width='380'> 12. Whether qualified for promotion to the higher class.<br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If so, to which class (in fig)</td><td style='font-weight: bold' width='10'> :</td> <td style='font-weight: bold'> " + promationclass + "</td> </tr> <tr><td align='left' width='380'> 13. Month upto which the pupil has paid school dues</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + feespaid + "</td> </tr> <tr><td align='left' width='380'> 14. Any fee concession availed of. if so, the nature of <br />  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;such concession</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + consession + "  </td> </tr> <tr>  <td align='left' width='380'>15. Total No. of working days in the academic session  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + workingdays + "</td> </tr> <tr>  <td align='left' width='380'>16. Total No. of working days pupil present in the school  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + persentdays + "</td></tr> <tr>  <td align='left' width='380'>17. Whether NCC Cadet / Boy Scout / Girl Guide </br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(details may be given)  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + ncc + "  </td> </tr> <tr>  <td align='left' width='380'>18. Games played or extra-curricular activities in which the <br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; pupil usually tookpart (mention achievement level therein)</td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + extraactivity + "  </td></tr> <tr>  <td align='left' width='380'>19. General conduct  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + generalconduct + "  </td> </tr> <tr>  <td align='left' width='380'>20. Date of application for certificate  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + dateofappcertificate + "  </td> </tr> <tr>  <td align='left' width='380'>21. Date of issue of certificate  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + dateofissue + "  </td> </tr> <tr>  <td align='left' width='380'>22. Reasons for leaving the school  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + releavingreson + "  </td> </tr> <tr>  <td align='left' width='380'>23. Any other remarks  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + remarks + "  </td> </tr></table><br><table style='width: 700px;' cellpadding='5' cellspacing='5'> <tr>  <td width='210' align='left'>Class Teacher  </td>  <td width='210' align='left' colspan='2'>Checked by  </td>  <td width='210'>  Principal </tr> <tr>  <td width='210' align='left' >Name:" + classteachar + "  </td>  <td width='210' align='left' style='width: 0' >Name  </td>  <td width='210' align='left style='width: 105px'>:  " + Checkedby + "  </td>  </td> <td  width='250'>(With School Seal & Date)  </td>  </tr> <tr>  <td width='210' align='left'>  </td>  <td width='210' align='left' style='width: 0'>Designation&nbsp;  </td>  <td width='210' align='left' style='width: 105px'>:  " + checkedbydesign + "</td> </tr></table>  </div> <br>");
                                        schoolcode = ": " + schoolcode;
                                        admissionno = ": " + admissionno;
                                        #endregion
                                        #region new  last modified by sudhagar 26.05.2017 4.20 PM
                                        html.Append("<div style='height: 1200px; width: 700px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'>");

                                        html.Append("<table cellspacing='0' cellpadding='0' style='width: 700px; ' border='0'>");

                                        html.Append("<tr><td style='width: 100px;'></td><td style='text-align: right;' > <img src=" + photo + " alt='' style='height: 100px; width: 120px;' /></td><td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: XX-Large;font-weight:bold;'>" + collname + "</span> <br><span style='font-size: medium;'>" + address11 + " , " + address22 + " , " + address33 + " - " + pincode + "<br>" + " Phone No: " + phone + " , " + " Fax No: " + fax + "<br>Email: " + email + "&nbsp;&nbsp;Website: " + website + "  </span></tr>");

                                        html.Append(" </table>");

                                        html.Append("<div>");

                                        html.Append("<table style='width: 950px;style='font-size: 10px;'>");

                                        html.Append("<tr style='font-size:18px;'><td style='align: left; width:110px;'>Affiliation No</td><td>: " + affiliationno + "</td><td></td>  <td> </td>     <td>    </td>    <td style='text-align:left; width:110px;'>School Code</td> <td style='text-align:left; width:100px;'>" + schoolcode + " </td> </tr>");

                                        html.Append("<tr style='font-size:18px;'><br><td style='align: left; width:110px;'>Sl No</td><td>: " + serialno + "</td><td></td>  <td> </td>     <td>    </td>    <td style='text-align:left; width:110px;'> Admission No </td> <td style='text-align:left; width:100px;'> " + admissionno + " </td> </tr> ");

                                        html.Append(" <tr style='font-size:18px;'><td></td><td></td><td></td><td></td><br><td style='align: right;' align=left' colspan='2'>         &nbsp;</td><td style='font-weight: bold'> &nbsp;</td> </tr>");

                                        html.Append("<tr><td style=' font-family: Arial; ' align='center' colspan='7'><div id='squre' style='border-style: solid; border-width: thin;  width: 350px; height: 30px;font-weight:bold; font-size: x-Large;' ><center>TRANSFER CERTIFICATE </center></div></td> ");

                                        html.Append("</tr></table>");

                                        html.Append("<br/>");

                                        html.Append("<table style='width: 950px;' cellpadding='4' cellspacing='4'>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='435px'> 1. Name of the Pupil</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + studname + "</td> </tr> ");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 2. Mother's Name</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + mothername + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 3. Father's Name / Guardian's Name</td><td style='font-weight: bold width='10'> :</td><td style='font-weight: bold'> " + fathername + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='400'> 4. Date of birth (in Christian Era) According to </br>  &nbsp;&nbsp;&nbsp;&nbsp;Admission & withdrawal Register (in figures)</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + dobandwords + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 5. Nationality</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + nationality + "</td> </tr>");

                                        html.Append("<tr style='height:38px;'><td align='left' width='380'>  6. Whether the candidate belongs to Schedule Caste or<br /> &nbsp;&nbsp;&nbsp;&nbsp; Schedule Tribe or OBC</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + communitity + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 7. Date of first admission in the School with Class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + admissionclasswoword + "</td> </tr> ");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 8. Class in which the pupil last studied (in figures)</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + studied + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 9. School / Board Annual Examination last taken with <br>&nbsp;&nbsp;&nbsp;&nbsp; result</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + lastexamresult + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'>  10. Whether failed, if so once / twice in the same class </td> <td style='font-weight: bold' width='10'> : </td> <td style='font-weight: bold'>  " + attempts + " </td></tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 11. Subjects Studied</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + subjectstudied + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 12. Whether qualified for promotion to the higher class<br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;If so, to which class (in fig)</td><td style='font-weight: bold' width='10'> :</td> <td style='font-weight: bold'> " + promationclass + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 13. Month upto which the pupil has paid school dues</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + feespaid + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'><td align='left' width='380'> 14. Any fee concession availed. If so, the nature of <br />  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;such concession</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: bold'> " + consession + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>15. Total No. of working days in the academic session  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + workingdays + "</td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'> <td align='left' width='380'>16. Total No. of working days pupil present in the school  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + persentdays + "</td></tr> ");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>17. Whether NCC Cadet / Boy Scout / Girl Guide </br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;(details may be given)  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + ncc + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>18. Games played or extra-curricular activities in which the <br /> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; pupil usually took part (mention achievement level <br/> &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;therein)</td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + extraactivity + "  </td></tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>19. General Conduct  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + generalconduct + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>20. Date of application for certificate  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + dateofappcertificate + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>21. Date of issue of certificate  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + dateofissue + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>22. Reasons for leaving the school  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + releavingreson + "  </td> </tr>");

                                        html.Append("<tr style='height:38px; font-size:18px;'>  <td align='left' width='380'>23. Any other remarks  </td>  <td style='font-weight: bold' width='10'>:  </td>  <td style='font-weight: bold'>" + remarks + "  </td> </tr>");

                                        html.Append("</table>");

                                        html.Append("<br/>");
                                        html.Append("<br/>");
                                        //html.Append("<br/>");
                                        //html.Append("<br/>");
                                        //html.Append("<br/>");

                                        html.Append("<table style='width: 900px;'>"); //cellpadding='5' cellspacing='5'

                                        html.Append("<tr style='height:35px; font-size:18px;' >  <td width='500'>Class Teacher  </td>  <td width='100' style='text-align:left;'>Checked by </td><td width='150' style='text-align:left;'></td> <td width='300' style='text-align:center;'>  Principal <span style='color:white; widh=10px;'></span> </td> </tr>");

                                        html.Append("<tr style='height:35px; font-size:18px;'>  <td width='500' align='left' >Name: " + classteachar + "  </td>  <td width='100' style='text-align:left;' >Name   </td> <td width='300' style='text-align:left;'> :  " + Checkedby + " </td>  <td  width='300' style='text-align:right;'>(With School Seal & Date) </td>  </tr>");

                                        html.Append("<tr style='height:35px; font-size:18px;'>  <td width='500' align='left'>  </td>  <td width='100' style='text-align:left;'>Designation </td> <td width='300' style='text-align:left;'> :  " + checkedbydesign + "</td> <td width='300' style='text-align:right;'></td> </tr>");

                                        html.Append("</table>");
                                        html.Append("</div>");
                                        html.Append("</div>");

                                        html.Append("<br>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "1")
                                    {
                                        #region SSLC
                                        photo = "";
                                        html.Append("<div style='height:1200px; width: 650px; border: 0px solid black; margin:0px; margin-left: 5px;'><table cellspacing='0' cellpadding='0' style='width: 750px;height:120px;' border='0' ><tr> <td colspan='2' align='center'>  <span style='font-size: medium;'>" + collname + "<br>" + address1 + " , " + address2 + " , " + address3 + " - " + pincode + " <br />" + disposlno + " </span> <br /><br /></td></tr><tr> <br>    <td  align='center' colspan='4' > <div id='squre' style='border-style: solid; border-width: thin;  width: 220px; height: 20px;' ><center> TRANSFER CERTIFICATE </center></div></td></tr>  </table> <br> <table style='width: 750px;font-size:medium;'><tr>     <td style='font-family: Arial; ' align='left'  width='200'> <p>வரிசை எண்</p> </td><td style='font-family: Arial; width: 300px;' align='center' width='130'         rowspan='2'><div style='width: 50px;height: 50px;-webkit-border-radius: 100px;-moz-border-radius: 100px; border-radius: 100px;border-style: solid; border-width: thin;'></div></td><td> <p>சேர்க்கை எண்</p></td></tr><tr>    <td style='font-family: Arial; ' align='left' width='200'> Serial No. " + serialno + "</td> <td style='font-family: Arial; ' align='left'  width='200'> Admission No. " + admissionno + "</td></tr>  </table>  <br><table style='width: 700px; height:1000px;font-size:medium;' cellpadding='5' cellspacing='5' ><tr> <td width='10' align='center'>  1. </td><td align='left' width='300' rowspan='2' >  <p>அ. பள்ளியின் பெயர்</p> a. Name of the School  <br />  </td><td style='font-weight: bold' width='10' align='center' rowspan='2' >  : </td> <td style='font-weight: bold' width='280' rowspan='2'>  " + collegename + " </td></tr><tr> <td width='10' align='center'></td> </tr><tr> <td width='10' align='center'> </td> <td align='left' width='300'>  <p>ஆ. கல்வி மாவட்டப் பெயர்</p> b. Name of the Educational District  <br />  </td> <td style='font-weight: bold' width='10' align='center'>  : </td> <td style='font-weight: bold' width='220'>  " + collegenamedistrict + " </td></tr><tr> <td width='10' align='center'> </td> <td align='left' width='300'>  <p>இ. வருவாய் மாவட்டப் பெயர்</p> c. Name of the Revenue District  </td> <td style='font-weight: bold' width='10' align='center'>  : </td> <td style='font-weight: bold' width='220'>  " + districtname + " </td></tr><tr> <td align='center' width='10'>  2. </td><td width='300' rowspan='2'>  <p>மாணவர் பெயர் (தனித்தனி எழுத்துகளில்)</p> Name of the pupil (in block letters)</td><td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td><td style='font-weight: bold' width='220' rowspan='2'>  " + studname + " </td></tr><tr> <td align='center' width='10'></td> </tr><tr> <td align='center' width='10'>  3. </td> <td width='300' rowspan='2'>  <p>  தந்தையின் பெயர் (அல்லது) தாயாரின் பெயர்</p> Name of the Father or Mother of the Pupil</td> <td style='font-weight: bold' align='center' width='10' rowspan='2'> : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + fathername + " </td></tr><tr> <td align='center' width='10'></td> </tr><tr> <td align='center' width='10'>  4. </td> <td width='300' rowspan='2'>  <p>தேசிய இனம் மற்றும் சமயம் & சாதி</p> Nationality, Religion and Caste  </td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + nationalityandregion + " </td></tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  5. </td>     <td width='300'  rowspan='3'>  <p>இனம் அவன் / அவள் பின்வரும் ஐந்து பிரிவுகளில் எவையேனும் ஒன்றைச் சார்ந்தவராயின் ஆம் என்றும் இனமும்எழுதவும்</p> If he / she belongs to any of the 5 categories say 'YES' & indicate community  </td>     <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>         <td style='font-weight: bold' width='220' rowspan='3'>  " + communitity + " </td></tr><tr> <td align='center' width='10'>  </td> </tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'> </td> <td width='300'>  <p>அ.ஆதி திராவிடர் (எஸ்.சி) அல்லது (எஸ்.டி)</p> a. Adi Dravidar (SC) or (S.T.)</td> <td style='font-weight: bold' align='center' width='10'>  :  </td> <td style='font-weight: bold' width='220'>  " + communititytypesc + " </td></tr><tr> <td align='center' width='10'> </td><td width='300'>  <p>ஆ.பின்தங்கிய வகுப்பு</p> b. Backward Class</td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypebc + "</td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  <p>இ.மிகவும் பின்தங்கிய வகுப்பு</p> c. Most Backward Class</td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypembc + "  </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  <p>ஈ.ஆதி திராவிடர் இனத்திலிருந்து கிறிஸ்தவ மதத்திற்கு மாறியவர் (அல்லது)</p> d. Convert Christianity from Scheduled Caste or</td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypeconvert + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  <p>உ. அட்டவணையிலிருந்து நீக்கப்பட்ட இனம்</p> e. Denotified Communities</td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>" + DenotifiedComm + " </td></tr><tr> <td align='center' width='10'>  6. </td>   <td width='300' rowspan='2'>  <p>பாலினம்</p> Sex</td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + sex + " </td></tr><tr> <td align='center' width='10'>  </td> </tr><tr> <td align='center' width='10'>  7. </td>       <td width='300' rowspan='3'>  <p>பிறந்த தேதி எண்ணிலும் எழுத்திலும்(மாணவர் சேர்க்கைப் பதிவேட்டில் உள்ளபடி)</p> Date of Birth as entered in the Admission Register (in figures and words)</td>       <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>     <td style='font-weight: bold' width='220' rowspan='3'>  " + dobandwords + " </td></tr><tr> <td align='center' width='10'>      </td>   </tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  8. </td> <td width='300'  rowspan='2'>  <p>உடலில் அமைந்த அடையாளக் குறிகள்</p> Personal marks of identification</td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + identificationmark + " </td></tr><tr> <td align='center' width='10'></td> </tr><tr> <td align='center' width='10'>  9. </td>     <td width='300'  rowspan='3'>  <p>பள்ளியில் சேர்க்கப்பட்ட தேதி மற்றும் சேர்க்கப்பட்ட வகுப்பு (வருடத்தை எழுத்தால் எழுதுக)  </p> Date of Admission and standard in which admitted (the year to be entered in words)</td>     <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>     <td style='font-weight: bold' width='220' rowspan='3'>  " + admissiondateclass + " </td></tr><tr> <td align='center' width='10'>      </td> </tr><tr> <td align='center' width='10'>  </td> </tr><tr> <td align='center' width='10'>  10. </td>         <td width='300'  rowspan='3'>  <p>  அ.பள்ளியைவிட்டு நீங்கும் காலத்தில் படித்து வந்த வகுப்பு (எழுத்தில்)</p> a. Standard in which the pupil was studying at the time of leaving (in words)  </td>        <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>        <td style='font-weight: bold' width='220' rowspan='3'>  " + laststudied + " </td></tr><tr> <td align='center' width='10'>      </td>     </tr><tr> <td align='center' width='10'></td> </tr><tr> <td align='center' width='10'> </td> <td width='300'> </td> <td style='font-weight: bold' align='center' width='10'> </td> <td style='font-weight: bold' width='220'> </td></tr><tr> <td align='center' width='10'> </td> <td width='300'> </td> <td style='font-weight: bold' align='center' width='10'> </td> <td style='font-weight: bold' width='220'> </td></tr>  </table></div> <br /><br /><br /><br /><br /><br /><br> <div style='height: 1200px; width: 650px; border: 0px solid black; margin-left: 5px;margin:0px;page-break-after: always;'> <br><br><table style='width: 650px;font-size:medium;' cellpadding='3' cellspacing='3'><tr> <td align='center' width='10'>  11. </td>   <td width='300' rowspan='2'>  <p>மேல்வகுப்பிற்கு உயர்வு பெறத் தகுதியுடையவரா?  </p> Whether qualified for promotion to  </td>   <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td>   <td style='font-weight: bold' width='220' rowspan='2'>  " + promationclass + " </td></tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  12. </td>         <td width='300'  rowspan='3'>  <p>மாணவர் படிப்பு உதவித்தொகை எதுவும் பெற்றவரா? (அதன் விவரத்தைக் குறிப்பிடுக)</p> Whether the pupil was in receipt of any scholarship (Nature of the scholarship to  be specified)</td>         <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>         <td style='font-weight: bold' width='220' rowspan='3'>  " + consession + " </td></tr><tr> <td align='center' width='10'>          </td> </tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  13. </td>         <td width='300'  rowspan='4'>  <p>மாணவர் கடைசி பள்ளி வருடத்தில் மருத்துவ ஆய்வுக்குச் சென்றவரா? (முதல் தடவை) அல்லது அதற்கு மேல்குறிப்பிட்டு எழுதவும்</p> Whether the pupil has undergone Medical Inspection during the academic year (First  or repeat to be specified)</td>         <td style='font-weight: bold' align='center' width='10' rowspan='4'>  : </td>         <td style='font-weight: bold' width='220' rowspan='4'>  " + medicalfit + " </td></tr><tr> <td align='center' width='10'>          </td> </tr><tr> <td align='center' width='10'>  </td> </tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  14. </td> <td width='300'  rowspan='2'>  <p>மாணவர் பள்ளியை விட்டுச் சென்ற தேதி</p> Date on which the pupil actually left the school  </td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + lastdate + " </td></tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  15. </td> <td width='300'  rowspan='2'>  <p>மாணவரின் ஒழுக்கமும் பண்பும்</p> The pupil's conduct and character  </td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + generalconduct + " </td></tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  16. </td>           <td width='300'  rowspan='3'>  <p>பெற்றோர் (அல்லது) காப்பாளர் மாணவரின் மாற்றுச் சான்றிதழ் கோரி விண்ணப்பித்த தேதி</p> Date on which application for Transfer certificate was made on behalf of the pupil  by the parent or guardian</td>             <td style='font-weight: bold' align='center' width='10' rowspan='3'>  : </td>             <td style='font-weight: bold' width='220' rowspan='3'>  " + dateofappcertificate + " </td></tr><tr> <td align='center' width='10'>  </td> </tr><tr> <td align='center' width='10'>    </td> </tr><tr> <td align='center' width='10'>  17. </td> <td width='300'  rowspan='2'>  <p>மாற்றுச் சான்றிதழ் தேதி</p> Date of Transfer Certificate</td> <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td> <td style='font-weight: bold' width='220' rowspan='2'>  " + dateofissue + " </td></tr>  <tr> <td align='center' width='10'>    </td> </tr>  <tr> <td align='center' width='10'>18.</td> <td width='300'  rowspan='2'>  <p>படிப்புக் காலம்</p> Course of study</td> <td style='font-weight: bold' lign='center' width='10' rowspan='2'>:</td> <td style='font-weight: bold' width='220' rowspan='2'>  " + courseofstudy + " </td></tr>  <tr> <td align='center' width='10'>  </td> </tr>  </table>  <div><table style='width: 750px;height:150px;' cellpadding='2' cellspacing='2' border='1'> <tr>  <td align='center'><p> பள்ளியின் பெயர்</p>Name of the School  </td>  <td align='center'><p> கல்வி ஆண்டு</p>Academic Year(s)  </td>  <td align='center' width='150'><p> படித்த வகுப்பு</p>Standard(s)<br />Studied  </td>  <td align='center'><p> முதல் மொழி</p>First Language  </td> <td align='center'><p> பயிற்று மொழி</p>Medium of Instruction  </td> </tr> <tr>  <td align='center'>" + collegename + "  </td> <td align='center'>" + batchyear + "  </td>      <td align='center' width='150'>" + classtoclass + " </td>  <td align='center'>  " + partonelang + "  </td>  <td align='center'>" + mediumofstudy + "  </td> </tr></table>  </div> <br> <table style='width: 650px;' cellpadding='3' cellspacing='3'><tr> <td align='center' width='10'>19. </td>     <td width='300' rowspan='2'>  <p>பள்ளித் தலைமையாசிரியரின் கையொப்பம் தேதியுடன் முத்திரையும்</p>  Signature of the H.M. with date and school seal </td>         <td style='font-weight: bold' align='center' width='10' rowspan='2'>  : </td>         <td style='font-weight: bold' width='220' rowspan='2'> </td></tr><tr> <td align='center' width='10'></td> </tr><tr> <td colspan='4'>  <hr width='645' /> </td></tr>  </table><table style='width: 645px;height:300px;' cellpadding='2' cellspacing='2'><tr> <td width='70' align='right' style='margin-top:0px; margin:0px; padding:0px;'> <p> குறிப்பு </p></td><td>  <p>1.இச்சான்றிதழில் அழித்தல்கள் மற்றும் நம்பகமற்ற அல்லது மோசமான, திருத்தங்கள் செய்வது</p> </td></tr><tr><td></td><td rowspan='2'><p> சான்றிதழை ரத்து செய்ய வழி வகுப்பதாகும்.</p>Erasures and unauthenticated or fraudulent alterations in the certificate will  lead to its cancellation.</td></tr><tr> <td width='70' align='right'> </td> </tr><tr> <td width='70' align='right'>  </td> </tr><tr> <td width='70' height='40'> </td>     <td>  <p>2.பள்ளி தலைமையாசிரியர் மையினால் கையொப்பமிட வேண்டும். பதிவு செய்யப்பட்ட விவரங்கள் சரியானவை என்பதற்கு அவரே பொறுப்பாளானவர். </p>Should be signed in ink by the Head of the Institution, who will be held responsible  for the correctness of the entries.</td>  </tr><tr><td></td></tr><tr><td>    </td></tr><tr> <td align='center' width='70' height='46'> </td><td align='center'> <div id='squre1' style='border-style: solid; border-width: thin;  width: 450px; height: 40px;' ><center><p>பெற்றோர் அல்லது காப்பாளர் அளிக்கும் உறுதிமொழி</p>  DECLARATION BY THE PARENT OR GUARDIAN</center></div></td></tr><tr> <td align='center' width='70' height='46'>     </td><td align='center'> </td></tr><tr><td></td> <td><p>3.மேலே 2 முதல் 7 வரையிலான இனங்களுக்கெதிரே பதிவு செய்யப்பட்டுள்ள விவரங்கள் சரியானவை என்றும் எதிர்காலத்தில் அவற்றில் மாற்றம் எதுவும் கேட்கமாட்டேன் என்றும் நான் உறுதியளிக்கிறேன்.</p> I hereby declare that the particulars given against items 2 to 7 are correct and  that no change will be demanded by me in future.</td></tr></table> <table  style='width: 750px;' cellpadding='2' cellspacing='2'><tr> <td width='70' height='8'> </td> <td colspan='3'> </td></tr><tr> <td width='70' height='8'></td> <td colspan='3'> </td></tr><tr><td colspan='2' width='300'><br>  <p>மாணவரின் கையொப்பம்</p> Signature of the Pupil  </td> <td width='100'> </td> <td> <br> <p>பெற்றோர் அல்லது காப்பாளரின் கையொப்பம்</p> Signature of the Parent / Guardian</td></tr><tr> <td width='70' height='10'> </td> <td colspan='3'> </td></tr>  </table></div>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "2")
                                    {
                                        #region Hsc
                                        html.Append("<div style='height:1200px; width: 595px; border: 0px solid black; margin:0px; margin-left: 5px;'><table cellspacing='0' cellpadding='0' style='width: 645px;' border='0'><tr> <td colspan='2'>  <img src=" + photo + " alt='' style='height: 80px; width: 70px;' /> </td> <td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black;  text-align: center;' colspan='2'>  <span style='font-size: medium;'>" + collname + "<br>" + address1 + " , " + address2 + " , " + address3 + " - " + pincode + ". INDIA" + "<br>" + affliated + " </span> </td></tr><tr> <td style='border-width: 1px; border-style: solid; font-family: Arial; '  align='center' colspan='4' > மாற்றுச் சான்றிதழ் TRANSFER CERTIFICATE </td></tr>  </table>  <br><table style='width: 595px;font-size:small;'><tr> <td style='font-family: Arial; ' align='left'  width='120'> Serial No:" + applfromno + "</td><td style='font-family: Arial; ' align='center' width='150'> </td> <td style='font-family: Arial;  ' align='left'  width='150'>  Reg No:" + regno + " </td> <td style='font-family: Arial; ' align='center'> </td></tr><tr> <td style='font-family: Arial; ' align='left' width='120'> <p>வரிசை எண்</p> </td> <td style='font-family: Arial; ' align='center' width='150'> </td> <td style='font-family: Arial;  ' align='left'  width='150'>  T.M.R.Code No:" + tmrno + " </td> <td style='font-family: Arial; ' align='center'> </td></tr><tr> <td style='font-family: Arial; ' align='left' width='180'> Admission No: " + admissionno + "</td> <td style='font-family: Arial; ' align='center' width='150'> </td> <td style='font-family: Arial;  ' align='left'  width='150'>  Certificate No:" + certificateno + " </td> <td style='font-family: Arial; ' align='center'> </td></tr><tr><td style='font-family: Arial; ' align='left'><p>சேர்க்கை எண்</p> </td> <td style='font-family: Arial; ' align='center' width='100'> </td> <td style='font-family: Arial; ' align='left'  width='150'>  Date: " + certificatedate + "</td> <td style='font-family: Arial; ' align='center'> </td></tr><tr> <td colspan='4'>  Department of School Education, Government of Tamilnadu, Recognised by the the Director  of Education.<p>பள்ளிக் கல்வித்துறை, தமிழ்நாடு அரசு, இயக்குனரால் அங்கீகரிக்கப்பட்டது</p> </td></tr>  </table>  <br><table style='width: 645px;height:1000px; font-size:small;' cellpadding='3' cellspacing='3'><tr> <td width='10' align='center'>  1. </td> <td align='left' width='300'>  a. Name of the School  <br />  <p>அ. பள்ளியின் பெயர்</p> </td> <td style='font-weight: bold' width='10' align='center'>  : </td> <td style='font-weight: bold' width='220'>  " + collegename + " </td></tr><tr> <td width='10' align='center'> </td> <td align='left' width='300'>  b. Name of the Educational District  <br />  <p>ஆ. கல்வியின் மாவட்டப் பெயர்</p> </td> <td style='font-weight: bold' width='10' align='center'>  : </td> <td style='font-weight: bold' width='220'>  " + collegenamedistrict + " </td></tr><tr> <td width='10' align='center'> </td> <td align='left' width='300'>  c. Name of the Revenue District  <p>இ. வருவாய் மாவட்டப் பெயர்</p> </td> <td style='font-weight: bold' width='10' align='center'>  : </td> <td style='font-weight: bold' width='220'>  " + districtname + " </td></tr><tr> <td align='center' width='10'>  2. </td> <td width='300'>  Name of the pupil (in block letters)<p>மாணவர் பெயர் (தனித்தனி எழுத்துகளில்)</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + studname + " </td></tr><tr> <td align='center' width='10'>  3. </td> <td width='300'>  Name of the Father or Mother of the Pupil<p>  தந்தையின் (அல்லது) தாயின் பெயர்</p> </td> <td style='font-weight: bold' align='center' width='10'> : </td> <td style='font-weight: bold' width='220'>  " + fathername + " </td></tr><tr> <td align='center' width='10'>  4. </td> <td width='300'>  Nationality, Religion and Caste  <p>தேசிய இனம் மற்றும் சமயம் சாதி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + nationalityandregion + " </td></tr><tr> <td align='center' width='10'>  5. </td> <td width='300'>  If he / she belongs to any of the 5 categories say 'YES' indicate community  <p>இனம் அவன் அவள் பின்வரும் ஐந்து பிரிவுகளில் எவையேனும் ஒன்றைச் சார்ந்தவராயின் ஆம்என்றும்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + caste + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  a. Adi Dravidar (SC) or (S.T.)<p>அ.ஆதி திராவிடர் (எஸ்.சி) அல்லது (எஸ்.டி)</p> </td> <td style='font-weight: bold' align='center' width='10'>  :  </td> <td style='font-weight: bold' width='220'>  " + communititytypesc + " </td></tr><tr> <td align='center' width='10'> </td><td width='300'>  b. Backward Class<p>ஆ.பின்தங்கிய வகுப்பு</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypebc + "   </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  c. Most Backward Class<p>இ.மிகவும் பின்தங்கிய வகுப்பு</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypembc + "  </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  d. Convert Christianity from Scheduled Caste or<p>ஈ.ஆதி திராவிடர் இனத்திலிருந்து கிறிஸ்தவ மதத்திற்கு மாறியவர்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + communititytypeconvert + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  e. Denotified Communities<p>உ. அட்டவணையிலிருந்து நீக்கப்பட்ட இனம்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'> </td></tr><tr> <td align='center' width='10'>  6. </td> <td width='300'>  Sex<p>பாலினம்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + sex + " </td></tr><tr> <td align='center' width='10'>  7. </td> <td width='300'>  Date of Birth as entered in the Admission Register<br />  (in figures and words)<p>பிறந்த தேதி எண்ணிலும் எழுத்திலும்(மாணவர் சேர்க்கைப் பதிவேட்டில் உள்ளபடி)</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + dobandwords + " </td></tr><tr> <td align='center' width='10'>  8. </td> <td width='300'>  Personal marks of identification<p>உடலில் அமைந்த அடையாளக் குறிகள்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + identificationmark + " </td></tr><tr> <td align='center' width='10'>  9 </td> <td width='300'>  Date of Admission and standard in which admitted (the year to be entered in words)<p>பள்ளியில் சேர்க்கப்பட்ட தேதி மற்றும் சேர்க்கப்பட்ட வகுப்பு (வருடத்தை எழுத்தால் எழுதுக)  </p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + admissiondateclass + " </td></tr><tr> <td align='center' width='10'>  10 </td> <td width='300'>  a. Standard in which the pupil was studying at the time of leaving (in words)  <p>  அ.பள்ளியைவிட்டு நீங்கும் காலத்தில் படித்து வந்த வகுப்பு (எழுத்தில்)</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + laststudied + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  b. The course offered, i.e. General Education or Vocational Education  <p> ஆ.தேர்ந்தெடுக்கப்பட்ட பாடப்பிரிவு அதாவது பொதுக்கல்வி தொழிற்கல்வி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + vacationalornot + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  c. In the case of General Education the subject offered under part-III, Group A  & medium of Instruction  <p>இ. பொதுக்கல்வியாயின், பகுதி III-தொகுதி (அ)ல் தேர்ந்தெடுத்த விருப்பப் பாடங்கள் மற்றும்போதனை மொழி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + subjectstudied + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'> </td> <td style='font-weight: bold' align='center' width='10'> </td> <td style='font-weight: bold' width='220'> </td></tr><tr> <td align='center' width='10'> </td> <td width='300'> </td> <td style='font-weight: bold' align='center' width='10'> </td> <td style='font-weight: bold' width='220'> </td></tr>  </table> </div> <br><br>          <div style='height: 1200px; width: 595px; border: 0px solid black; margin-left: 5px;margin:0px;'>  <table style='width: 645px;font-size:smaller;' cellpadding='2' cellspacing='2'><tr> <td align='center' width='10'> </td> <td width='300'>  d. In the case of Vocational Education, the Vocational Subject Subject offered under  part III Group (A) <p>ஈ.தொழிற்கல்வியாயின், பகுதி III-தொகுதி (ஆ)ல் தேர்ந்தெடுத்த தொழிற்பாடம்  மற்றும் பகுதி III-தொகுதி (அ)ல் எடுத்த தொடர்புடைய பாடம்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'> </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  e. Language offered under part - I<p>உ.பகுதி I ல் தேர்ந்தெடுத்த மொழி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + partonelang + " </td></tr><tr> <td align='center' width='10'> </td> <td width='300'>  f. Medium of Study  <p>ஊ. பயிற்று மொழி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + mediumofstudy + " </td></tr><tr> <td align='center' width='10'>  11. </td> <td width='300'>  Whether qualified for promotion to higher standard under Higher Sec. Education rules  <p>மேல்நிலைக் கல்வி விதிகளின்படி மேல்வகுப்பிற்கு உயர்வு பெறத் தகுதியுடையவரா?  </p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + promationclass + " </td></tr><tr> <td align='center' width='10'>  12. </td> <td width='300'>  Whether the pupil has paid all the fees due to the school?  <p>பள்ளிக்குச் செலுத்த வேண்டிய கட்டணத் தொகை அனைத்தையும் மாணவர் செலுத்திவிட்டாரா?</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + feespaid + " </td></tr><tr> <td align='center' width='10'>  13. </td> <td width='300'>  Whether the pupil was in receipt of any scholarship (Nature of the scholarship to  be specified)<p>மாணவர் படிப்பு உதவித்தொகை எதுவும் பெற்றவரா? (அதன் விவரத்தைக் குறிப்பிடுக)</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + consession + " </td></tr><tr> <td align='center' width='10'>  14. </td> <td width='300'>  Whether the pupil has undergone Medical Inspection during the academic year (First  or repeat to be specified)<p>மாணவர் கடைசி பள்ளி வருடத்தில் மருத்துவ ஆய்வுக்குச் சென்றவரா? முதல் தடவை அதற்கு மேல்குறிப்பிட்டு எழுதவும்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + medicalfit + " </td></tr><tr> <td align='center' width='10'>  15. </td> <td width='300'>  Date on which the pupil actually left the school  <p>மாணவர் பள்ளியை விட்டுச் சென்ற தேதி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + lastdate + " </td></tr><tr> <td align='center' width='10'>  16. </td> <td width='300'>  The pupil's conduct and character  <p>மாணவரின் ஒழுக்கமும் பண்பும்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + generalconduct + " </td></tr><tr> <td align='center' width='10'>  17. </td> <td width='300'>  Date on which application for Transfer certificate was made on behalf of the pupil  by the parent or guardian<p>பெற்றோர் காப்பாளர் மாணவரின் மாற்றுச் சான்றிதழ் கோரி விண்ணப்பித்த தேதி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + dateofappcertificate + " </td></tr><tr> <td align='center' width='10'>  18. </td> <td width='300'>  Course of study<p>படிப்புக் காலம்</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + coursename + " </td></tr><tr> <td align='center' width='10'>  19. </td> <td width='300'>  Date of Transfer Certificate<p>மாற்றுச் சான்றிதழ் வழங்கிய தேதி</p> </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'>  " + dateofissue + " </td></tr>  </table>  <div><table style='width: 645px;' cellpadding='2' cellspacing='2' border='1'> <tr>  <td><p> பள்ளியின் பெயர்</p>Name of the School  </td>  <td><p> கல்வி ஆண்டு</p>Academic Year(s)  </td>  <td><p> படித்த வகுப்பு</p>Standard(s)<br />Studied  </td>  <td><p> முதல் மொழி</p>First Language  </td> <td><p> பயிற்று மொழி</p>Medium of Instruction  </td> </tr> <tr>  <td>" + collegename + "  </td> <td>" + batchyear + "  </td>  <td>" + admissiondateclass + "" + laststudied + "  </td>  <td>  " + partonelang + "  </td>  <td>" + mediumofstudy + "  </td> </tr></table>  </div>  <table style='width: 645px;' cellpadding='2' cellspacing='2'><tr> <td align='center' width='10'>  20. </td> <td width='300'>  <p>பள்ளித் தலைமையாசிரியரின் கையொப்பம் தேதியுடன் முத்திரையும்</p>  Signature of the H.M. with date and school seal </td> <td style='font-weight: bold' align='center' width='10'>  : </td> <td style='font-weight: bold' width='220'> </td></tr><tr> <td colspan='4'>  <hr width='645' /> </td></tr>  </table>  <table style='width: 645px;' cellpadding='2' cellspacing='2'><tr> <td width='70'>  குறிப்பு </td> <td colspan='3'>  1. Erasures and unauthenticated or fraudulent alterations in the certificate will  lead to its cancellation.<p>இச்சான்றிதழில் அழித்தல்கள் மற்றும் நம்பகமற்ற அல்லது மோசமான, திருத்தங்கள் செய்வது சான்றிதழை ரத்து செய்ய வழி வகுப்பதாகும்.  </p> </td></tr><tr> <td width='70'> </td> <td colspan='3'>  2. Should be signed in ink by the Head of the Institution, who will be held responsible  for the correctness of the entries.<p>பள்ளி தலைமையாசிரியர் மையினால் கையொப்பமிட வேண்டும். பதிவு செய்யப்பட்ட விவரங்கள் சரியானவைஎன்பதற்கு அவரே பொறுப்பாளானவர்.  </p> </td>  </tr><tr> <td align='center' width='70'> </td> <td align='center' colspan='3'>  DECLARATION BY THE PARENT OR GUARDIAN<p>பெற்றோர் காப்பாளர் அளிக்கும் உறுதிமொழி</p> </td></tr><tr> <td width='70'> </td> <td colspan='3'> </td></tr><tr> <td colspan='4'>  I hereby declare that the particulars given against item 2 to 7 are correct and  that no change will be demanded by me in future.<p>மேலே 2 முதல் 7 வரையிலான இனங்களுக்கெதிரே பதிவு செய்யப்பட்டுள்ள விவரங்கள் சரியானவைமற்றும் எதிர்காலத்தில் அவற்றில் மாற்றம் எதுவும் கேட்கமாட்டேன் என்றும் நான் உறுதியளிக்கிறேன்</p> </td></tr><tr> <td colspan='2'> </td></tr><tr> <td width='70'> </td> <td colspan='3'> </td></tr><tr> <td colspan='2'>  Signature of the Pupil  <p>மாணவரின் கையொப்பம்</p> </td> <td width='150'> </td> <td>  Signature of the Parent / Guardian<p>பெற்றோர் அல்லது காப்பாளரின் கையொப்பம்</p> </td></tr><tr> <td width='70'> </td> <td colspan='3'> </td></tr>  </table> </div>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "3")
                                    {
                                        #region Transfer certificate
                                        string dob1 = dobdate.ToString("dd-MMM-yyyy");
                                        dob1 = (dob1 == "01-Jan-1900" ? " - " : dob1);
                                        //<center><div style='font-size: x-large; font-weight: bold; font-family:Arial Rounded MT Bold;color: #3366CC;'>TRANSFER CERTIFICATE</div></center>
                                        html.Append("<div style='height: 845px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center>    <table style='width: 100%; margin-top: 130px; font-size: small;' cellpadding='6' cellspacing='6'> <tr> <td colspan='7' ></td></tr><tr><td width='5'>  </td>  <td align='left' width='280'>  </td>  <td style='font-weight: bold' >  </td>      <td style='font-weight: bold' colspan='3' align='left'></td> <td align='left'>Sl. No :" + serialno + "</td></tr> <tr>   <td width='5'>1.  </td>          <td align='left' width='280'>Name of the Student  </td>      <td style='font-weight: bold;float:right;' >:  </td>      <td style='font-weight: bold' colspan='3' align='left' width='150'>" + studname + "  </td><td rowspan='4' width='100'><img src=" + stdphoto + " alt=''  style='height: 100px; width: 80px; margin-top: 1px;' />  </td> </tr> <tr>     <td width='5'>2.  </td><td align='left' width='280'>Registered No  </td>      <td style='font-weight: bold' >:  </td><td style='font-weight: bold' align='left' colspan='3' width='150'>" + regno + "  </td>   </tr> <tr>      <td width='5'>3.  </td> <td align='left' width='280' >Father's Name  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' width='170' colspan='3'>" + fathername + "  </td>      </tr> <tr>      <td width='5'>4.  </td> <td align='left' width='280'>Mother's Name  </td>          <td style='font-weight: bold' >:  </td> <td style='font-weight: normal' align='left' colspan='3'>" + mothername + "  </td>      </tr> <tr>      <td width='5'>5.  </td> <td align='left' width='280'>Date of Birth  </td>      <td style='font-weight: bold' >:  </td>         <td style='font-weight: normal' align='left' colspan='2'>" + dob1 + "  </td>");
                                        if (Aadharcard_no != " - ")
                                            html.Append("<td align='right'  >Aadhar No : </td>         <td style='font-weight: normal;width:100px;' align='left' >" + Aadharcard_no + "</td>   ");
                                        html.Append("  </tr> <tr>      <td width='5'>6.  </td><td align='left' width='280'>Nationality  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + nationality + "  </td>  </tr> <tr>      <td width='5'>7.  </td><td align='left' width='280'>Religion / Community  </td>      <td style='font-weight: bold'>:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + nationality + " / " + religion + "  </td>  </tr> <tr>      <td width='5'>8.  </td><td align='left' width='280'>Class in which the Student was studying at the time of leaving the Institution  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + leavingtimeinstitution + "  </td>  </tr> <tr>      <td width='5'>9.  </td><td align='left' width='280'>Date of commencement of classes  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + commencementofclassdate + "  </td>  </tr> <tr>      <td width='5'>10.  </td><td align='left' width='280'>Whether qualified for promotion to a higher class  </td>      <td style='font-weight: bold' >:  </td>     <td style='font-weight: normal' align='left' colspan='4'>Refer to the Grade Sheet</td>  </tr> <tr>      <td width='5'>11.  </td><td align='left' width='280'>Whether the student has paid all the fees to the institution  </td>      <td style='font-weight: bold' >:  </td><td style='font-weight: normal' align='left' >Yes</td><td style='font-weight: normal' align='left'></td>  <td style='font-weight: bold'>&nbsp;  </td> </tr> <tr>      <td width='5'>12.  </td><td align='left' width='280'>Date on which the student last attended the class  </td>      <td style='font-weight: bold' >:  </td> <td style='font-weight: normal' align='left' colspan='4'>" + lastdate + "  </td>  </tr> <tr>      <td width='5'>13.  </td><td align='left' width='280'>Date on which application for Transfer Certificate was made  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + Transfer_cert_made + "  </td>  </tr> <tr>      <td width='5'>14.  </td><td align='left' width='280'>Conduct & Character  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: bold' align='left' colspan='4'>" + Conduct_Character + "  </td>  </tr> <tr>      <td width='5'>15.  </td><td align='left' width='280'>Medium of Instruction  </td>      <td style='font-weight: bold' >:  </td>      <td style='font-weight: normal' align='left' colspan='4'>" + mediumofstudy + "</td>   </tr> <tr> <td width='5'></td><td align='left' width='280'></td><td style='font-weight: bold' ></td>      <td style='font-weight: normal' align='left' colspan='4'></td>  </tr> <tr>      <td width='5'></td><td align='left' width='280'></td>      <td style='font-weight: bold' ></td>      <td style='font-weight: normal' align='left' colspan='4'> </td>  </tr> <tr>      <td width='5' colspan='2' style='width: 285px' align='left'>    Date : " + dateofissue + "   </td>      <td style='font-weight: bold' > </td>      <td style='font-weight: normal' align='left' colspan='4'></td>  </tr> <tr><td width='5' colspan='2' style='width: 285px' align='left'>    </td>      <td style='font-weight: bold' ></td>      <td style='font-weight: normal' align='left' colspan='4'></td>  </tr> <tr><td width='5' colspan='2' style='width: 285px' align='left'>    </td>      <td style='font-weight: bold' >        &nbsp;</td>      <td style='font-weight: normal' align='right' colspan='2'>  </td> <td style='width:30px' align='left' colspan='2'><span >REGISTRAR</span></td>  </tr></table>  </center>  <br>  <br />  <br />  <br />  </div> <br> <br>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "4")
                                    {
                                        #region MIGRATION certificate
                                        string prefix = "";
                                        if (sex.ToUpper() == "MALE")
                                            prefix = " Mr. ";
                                        else if (sex.ToUpper() == "FEMALE")
                                            prefix = " Miss. ";
                                        html.Append(" <div style='height: 845px; width: 100%; border: 0px solid black; margin-left: 10px; margin: 0px; page-break-after: always;'>   <table style='width: 100%; margin-top: 130px; font-size: medium;' cellpadding='7' cellspacing='7'> <tr>  <td align='Left' style='font-weight: bold'>Sl. No:" + migraslno + "  </td>  <td>  </td><td align='right' style='font-weight: bold'>Date :" + migration_date + "  </td> <td align='right' style='font-weight: bold' width='100px'></td> </tr></table><table style='width: 100%; margin-top: 50px; font-size: medium;' cellpadding='7' cellspacing='7'> <tr>  <td colspan='3' style='font-size: large'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This University has no objection to " + prefix + " " + studname + ", whose details are given below,in migrating and pursuing his further studies in any other University / Educational Institution.  </td> </tr><table style='width: 100%; margin-top: 50px; font-size: medium;' cellpadding='7' cellspacing='7'>  <tr><td align='Left' width='280' style='font-weight: bold'>The Student's details:  </td> </tr> <tr>  <td align='Left' width='280'>Name  </td>  <td width='10'>:  </td>  <td align='Left' width='300'>" + prefix + "" + studname + "  </td> </tr> <tr>  <td align='Left' width='280'>Registered / Roll No  </td>   <td width='10'>:  </td> <td align='Left' width='300'>" + regno + "  </td> </tr> <tr>  <td align='Left' width='280'>Programme completed  </td>      <td width='10'>: </td>  <td align='Left' width='300' style='font-weight: bold'>" + programcompleteddb + "  </td> </tr> <tr>  <td align='Left' width='280'>Period of Study  </td>      <td width='10'>:  </td>  <td align='Left' width='300'>" + periodofstudy + "  </td> </tr> <tr>  <td align='Left' width='280'>The month and year of the last examination appeared  </td>      <td width='10'>:  </td>  <td align='Left' width='300'>" + Last_exam_mon_year + "  </td> </tr></table><br /><br /><br /><table style='margin-left: 40px; margin-top: 70px; float: left;width: 80%; '> <tr>  <td>Seal  </td> <td style='font-weight: bold;float:right;'>REGISTRAR  </td></tr></table><br /><br /><br /></div><br> ");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "5")
                                    {
                                        #region Jamal Tc
                                        //<!--<td style='font-size: 12px; font-family: Times New Roman;  border: 0px solid black; text-align: center;'><span style='font-size: X-Large;font-weight:bold;'>" + collname + "</span> <br><span style='font-size: medium;'>" + catagory + "<br>" + affliatedby + " <br>" + district + " - " + pincode + " </span></td>-->
                                        dob = dob.Replace('/', '-');
                                        commencementofclassdate = commencementofclassdate.Replace('/', '-');
                                        Adm_Date = Adm_Date.Replace('/', '-');
                                        dateofissue = dateofissue.Replace('/', '-');
                                        Conduct_Character = Convert.ToString(dr["Conduct_Character"]).ToUpper() == "" ? "GOOD" : Convert.ToString(dr["Conduct_Character"]).ToUpper();
                                        string coll_name = collname;
                                        if (coll_name.Contains("("))
                                        {
                                            coll_name = coll_name.Split('(')[0];
                                        }

                                        string edu_level = d2.GetFunction("select c.edu_level from Registration r,applyn a,Course c,Degree dg,Department dt where dt.DepT_Code=dg.Dept_code and dg.degree_code=r.degree_code and r.app_no=a.app_no and c.Course_id=dg.Course_id and r.college_code=dg.College_code and r.app_no='" + app_no + "'");
                                        //string fullOrPartTime = string.Empty;
                                        //string isFullOrPartTime = d2.GetFunction("");
                                        //if (isFullOrPartTime == "0")
                                        //{
                                        //    fullOrPartTime = " (FULL TIME) ";
                                        //}
                                        //else
                                        //{
                                        //    fullOrPartTime = " (PART TIME) ";
                                        //}

                                        //   html.Append("<div style='height: 950px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'> <center> <div style='border:1px solid black'><table style='width: 100%; margin-top: 0px; font-size: small;' cellpadding='6' cellspacing='6'> <tr><td width='100px' > <img src=" + photo + " alt=''  style='height: 100px; width: 80px; margin-top: 1px;' />  </td><td></td><td width='120'>  <img src=" + stdphoto + " alt=''  style='height: 100px; width: 80px; margin-top: 1px;' /></td></tr><tr><td></td><td align='center' style='font-size: large; font-weight: bold'>TRANSFER & CONDUCT CERTIFICATE</td><td>Sl. No :" + serialno + "</td></tr></table><table style='width: 100%; margin-top: 0px; font-size: medium;' cellpadding='1' cellspacing='3'> <tr>    <td width='3px' align='center'>1.  </td> <td align='left' width='150'>Name of the Student  </td><td style='font-weight: bold;' width='10' >:  </td><td style='font-weight: bold'  align='left' width='150'>" + studname + "  </td> </tr> <tr>    <td width='3px' align='center'>2.  </td>  <td align='left' width='150'> Name of the Father</td><td style='font-weight: bold' width='10' >:  </td> <td align='left'  width='150'>" + fathername + "</td></tr> <tr>    <td width='3px' align='center' >3.  </td><td align='left' width='150'>Date of Birth as entered in +2 or Equivant certificate(in words)</td><td style='font-weight: bold'  width='10' >:  </td><td style='font-weight: normal' align='left' width='170'  > " + dob + "<br /></brL><span style='font-size: x-small;'>" + F5dobwords + "</span>  </td></tr> <tr>    <td width='3px' align='center'>4.  </td><td align='left' width='150'> Sex</td> <td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >" + sex + "  </td></tr> <tr>    <td width='3px' align='center'>5.  </td><td align='left' width='150'>Nationality </td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left'> " + nationality + "  </td> </tr> <tr>    <td width='3px' align='center'>6.  </td>  <td align='left' width='150'> Religion  </td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >" + religion + " </td>  </tr> <tr>    <td width='3px' align='center'>7.  </td>  <td align='left' width='150'>Caste  </td><td style='font-weight: bold' width='10'>:  </td><td style='font-weight: normal' align='left' >" + caste + "  </td>  </tr> <tr>    <td width='3px' align='center'>8.  </td>  <td align='left' width='150'>Class in which the Student was studying at the time of leaving college</td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >" + yearLetters + coursename + ". " + studied + " </td>  </tr> <tr>    <td width='3px' align='center'>9.  </td>  <td align='left' width='150'>First Language</td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >" + partonelang + "  </td>  </tr> <tr>    <td width='3px' align='center'>10.</td>  <td align='left' width='150'>Medium of Instruction</td><td style='font-weight: bold' width='10' > :</td> <td style='font-weight: normal' align='left' >" + mediumofstudy + "</td>  </tr> <tr>     <td width='3px' align='center'>11.  </td>  <td align='left' width='150'>Whether qualified for promotion to  the higher class  </td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >" + examflag + "</td>  </tr> <tr>     <td width='3px' align='center'>12.  </td>  <td align='left' width='150'>Whether the student has paid all the fees  due to the college  </td><td style='font-weight: bold' width='10' >:  </td><td style='font-weight: normal' align='left' >Yes</td> </tr> <tr>     <td width='3px' align='center'> 13.</td>  <td align='left' width='150'>Date of Admission to the class</td> <td style='font-weight: bold' width='10' > :</td><td style='font-weight: normal' align='left' >" + Adm_Date + Lateral + "</td> </tr> <tr>     <td width='3px' align='center'>14.  </td>  <td align='left' width='150'> Date on which the student left the college  </td> <td style='font-weight: bold' width='10' >:  </td>  <td style='font-weight: normal' align='left' >" + commencementofclassdate + "  </td>  </tr> <tr>     <td width='3px' align='center'> 15.</td>  <td align='left' width='150'>Date of issue of TC</td> <td style='font-weight: bold' width='10' > :</td>  <td style='font-weight: normal' align='left' >" + dateofissue + "  </td>  </tr> <tr>     <td width='3px' align='center'>16.  </td>  <td align='left' width='150'> Student's Conduct and Character  </td><td style='font-weight: bold'width='10' >:  </td> <td style='font-weight: bold' align='left' >" + Conduct_Character + "</td></tr> <tr>     <td width='3px' align='center'>17.  </td>  <td align='left' width='150'> Attendance  </td><td style='font-weight: bold' width='10' >:  </td> <td style='font-weight: normal' align='left' >" + attendance + "</td></tr> <tr>     <td width='3px' align='center'> 18.</td>  <td align='left' width='150'>Remarks</td><td style='font-weight: bold'width='10' >:</td> <td style='font-weight: normal' align='left' >" + remarks + "</td>  </tr> </table>  <center><br /><table  style='width: 500px; margin-top: 0px; font-size: small;' cellpadding='0' cellspacing='0' ><tr ><td style='border: thin solid #000000; font-weight: bold;' align='center'>Course of Study</td><td style='border: thin solid #000000; font-weight: bold;' align='center' >Academic Year</td><td style='border: thin solid #000000; font-weight: bold;' align='center' >Roll No</td><td style='border: thin solid #000000; font-weight: bold;' align='center' >Register No</td></tr><tr><td style='border: thin solid #000000;font-size: x-small;' align='center'>" + coursename + ". " + studied + "</td><td style='border: thin solid #000000' align='center'>" + Student_yearduration + "</td><td style='border: thin solid #000000' align='center'>" + rollno + "</td><td style='border: thin solid #000000' align='center'>" + regno + "</td></tr></table></center> <br /> </div><table  style=' width: 100%; margin-top: 30px; font-size: small;' cellpadding='5' cellspacing='5'><tr><td colspan='2' align='left' >Note: Erasure, fradulent or unauthenticated alteration in the certificate will lead to its cancellation</td><td  style='font-weight: bold'></td></tr><tr><td align='left' style='font-weight: bold'>Declaration by the Student </td><td></td><td></td></tr><tr><td align='left' colspan='2'>I hereby declare that the particulars recorded against items 1 to 7 are correct and that no change will be demanded by me in future</td><td style='font-weight: 700'>PRINCIPAL</td></tr><tr><td colspan='3' style='font-weight: bold' align='center'>COLLEGE SEAL</td></tr><tr><td colspan='3'></td></tr><tr><td align='left' colspan='3' style='font-weight: bold'>Signature of the Student</td></tr></table></center></div>");
                                        if (edu_level.ToLower().Contains("m.phil") || edu_level.ToLower().Contains("mphil") || edu_level.ToLower().Contains("m phil"))
                                        {

                                            html.Append("<center><div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'><center><div style='border: 1px solid black'> <table style='width: 100%; margin-top: 0px; font-size: small;' cellpadding='6' cellspacing='6'>  <tr><td width='100px'> <img src=" + photo + " alt='' style='height: 100px; width: 80px; margin-top: 1px;' /></td><td align='center' style='color:#8B4513'><div style='font-weight:bold;font-size:x-large;'>  " + coll_name + "</div> <div style='font-weight:bold;font-size:medium;'>  " + catagory + "</div> <div style='font-weight:bold;font-size:small;'> " + affliatedby1 + "</div> <div style='font-size:small;'>  " + affliatedby2 + "</div> <div style='font-size:small;'>  " + affliatedby3 + "</div> <div style='font-weight:bold;font-size:small;'>  " + address3 + " - " + pincode + "</div></td><td width='120'> <img src=" + stdphoto + " alt='' style='height: 90px; width: 80px; margin-top: 1px;' /></td>  </tr></table><table style='width: 100%; margin: 0px; padding: 0px; font-size: small;' cellpadding='0'cellspacing='0'>  <tr style='margin: 0px; padding: 0px;'><td width='100px'></td><td align='center' style='font-size: large; font-weight: bold'> TRANSFER & CONDUCT CERTIFICATE</td><td style='font-weight: bold'> SL. No. :" + serialno + "</td>  </tr><tr><td ><br/></td></tr> </table> <table style='width: 100%; margin-top: 0px; font-size: medium;' cellpadding='1' cellspacing='3'>  <tr><td width='3px' align='center'> 1.</td><td align='left' width='150'> Name of the Student</td><td style='font-weight: bold;' width='10'>:</td><td style='font-weight: bold' align='left' width='150'> " + studname + "</td></tr>  <tr><td width='3px' align='center'> 2.</td><td align='left' width='150'> Name of the Father</td><td style='font-weight: bold' width='10'> :</td><td align='left' width='150'> " + fathername + "</td></tr><tr><td width='3px' align='center'>    3.</td><td align='left' width='150'>Date of Birth as entered in +2 or Equivant certificate(in words)</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left' width='170'>" + dob + "<br /></brL><span style='font-size: x-small;'>" + F5dobwords + "</span></td></tr><tr><td width='3px' align='center'> 4.</td><td align='left' width='150'> Sex</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left'> " + sex + "</td> </tr> <tr> <td width='3px' align='center'> 5.</td><td align='left' width='150'> Nationality</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + nationality + "</td> </tr><tr><td width='3px' align='center'> 6.</td><td align='left' width='150'> Religion</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + religion + "</td>  </tr>  <tr><td width='3px' align='center'> 7.</td><td align='left' width='150'> Caste</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + caste + "</td>  </tr>  <tr><td width='3px' align='center'> 8.</td><td align='left' width='150'> Class in which the Student was studying at the time of leaving college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + coursename + ". " + studied + "</td>  </tr>  <tr><td width='3px' align='center'> 9.</td><td align='left' width='150'> Whether qualified for promotion to the higher class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + examflag + "</td>  </tr>  <tr><td width='3px' align='center'> 10.</td><td align='left' width='150'> Whether the student has paid all the fees due to the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> YES</td>  </tr>  <tr><td width='3px' align='center'> 11.</td><td align='left' width='150'> Date of Admission to the class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Adm_Date + "</td>  </tr>  <tr><td width='3px' align='center'> 12.</td><td align='left' width='150'> Date on which the student left the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + commencementofclassdate + "</td>  </tr>  <tr><td width='3px' align='center'> 13.</td><td align='left' width='150'> Date of issue of TC</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + dateofissue + "</td>  </tr>  <tr><td width='3px' align='center'> 14.</td><td align='left' width='150'> Student's Conduct and Character</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Conduct_Character + "</td>  </tr>  <tr><td width='3px' align='center'> 15.</td><td align='left' width='150'> Attendance</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + attendance + "</td>  </tr>  <tr><td width='3px' align='center'> 16.</td><td align='left' width='150'> Remarks</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + remarks + "</td>  </tr> </table> <center>  <table style='width: 95%; margin-top: 0px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'><tr> <td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>  Course of Study </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Academic Year </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Roll No </td> <td style='border: thin solid #000000;' align='center' class='style2'>  Register No </td></tr><tr> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center' class='style1'>  " + coursename + ". " + studied + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + periodofstudy + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + rollno + " </td> <td style='border: thin solid #000000; font-size: medium; border-top-style: none;'  align='center' class='style2'>  " + regno + " </td></tr>  </table> </center></div><div style='font-size: small; width: 95%; margin-top: 1px' align='left' class='style1'> Note: Erasure, fradulent or unauthenticated alteration in the certificate will lead to its cancellation</div><table style='width: 95%; margin-top: 83px; font-size: small;' cellpadding='0' cellspacing='0'>  <td class='style3'>  </td> </tr> <tr>  <td align='left' style='font-size: large' class='style3'>Declaration by the Student  </td>  <td style='font-weight: 600; font-size: large;' align='left'>COLLEGE SEAL  </td>  <td style='font-weight: bold; font-size: large;'>PRINCIPAL  </td> </tr> <tr>  <td align='left' style='height: 25px; margin-top: 5px' colspan='5'>I hereby declare that the particulars recorded against items 1 to 7 are correct and that no change will be demanded by me in future.  </td> </tr> <tr>  <td align='left' colspan='2' style='font-weight: bold; font-size: large; height: 25px;padding-top: 25px;'>Signature of the Student  </td> </tr></table>  </center> </div></center>");
                                        }
                                        else if (edu_level.ToLower().Contains("pg"))
                                        {
                                            html.Append("<center><div style='height: 990px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px; page-break-after: always;'><center><div style='border: 1px solid black'> <table style='width: 100%; margin-top: 0px; font-size: small;' cellpadding='6' cellspacing='6'>  <tr><td width='100px'> <img src=" + photo + " alt='' style='height: 100px; width: 80px; margin-top: 1px;' /></td><td align='center' style='color:#8B4513'><div style='font-weight:bold;font-size:x-large;'>  " + coll_name + "</div> <div style='font-weight:bold;font-size:medium;'>  " + catagory + "</div> <div style='font-weight:bold;font-size:small;'> " + affliatedby1 + "</div> <div style='font-size:small;'>  " + affliatedby2 + "</div> <div style='font-size:small;'>  " + affliatedby3 + "</div> <div style='font-weight:bold;font-size:small;'>  " + address3 + " - " + pincode + "</div></td><td width='120'> <img src=" + stdphoto + " alt='' style='height: 90px; width: 80px; margin-top: 1px;' /></td>  </tr>  </table><table style='width: 100%; margin: 0px; padding: 0px; font-size: small;' cellpadding='0'cellspacing='0'>  <tr style='margin: 0px; padding: 0px;'><td width='100px'></td><td align='center' style='font-size: large; font-weight: bold'> TRANSFER & CONDUCT CERTIFICATE</td><td style='font-weight: bold'> SL. No. :" + serialno + "</td>  </tr><tr><td ><br/></td></tr> </table> <table style='width: 100%; margin-top: 0px; font-size: medium;' cellpadding='1' cellspacing='3'>  <tr><td width='3px' align='center'> 1.</td><td align='left' width='150'> Name of the Student</td><td style='font-weight: bold;' width='10'>:</td><td style='font-weight: bold' align='left' width='150'> " + studname + "</td></tr>  <tr><td width='3px' align='center'> 2.</td><td align='left' width='150'> Name of the Father</td><td style='font-weight: bold' width='10'> :</td><td align='left' width='150'> " + fathername + "</td></tr><tr><td width='3px' align='center'>    3.</td><td align='left' width='150'>Date of Birth as entered in +2 or Equivant certificate(in words)</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left' width='170'>" + dob + "<br /></brL><span style='font-size: x-small;'>" + F5dobwords + "</span></td></tr><tr><td width='3px' align='center'> 4.</td><td align='left' width='150'> Sex</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left'> " + sex + "</td> </tr> <tr> <td width='3px' align='center'> 5.</td><td align='left' width='150'> Nationality</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + nationality + "</td> </tr><tr><td width='3px' align='center'> 6.</td><td align='left' width='150'> Religion</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + religion + "</td>  </tr>  <tr><td width='3px' align='center'> 7.</td><td align='left' width='150'> Caste</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + caste + "</td>  </tr>  <tr><td width='3px' align='center'> 8.</td><td align='left' width='150'> Class in which the Student was studying at the time of leaving college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + yearLetters + coursename + ". " + studied + "</td>  <tr><td width='3px' align='center'> 9.</td><td align='left' width='150'> Medium of Instruction</td><td style='font-weight: old' width='10'> :</td><td style='font-weight: normal' align='left'> " + mediumofstudy + "</td>  </tr>  <tr><td width='3px' align='center'> 10.</td><td align='left' width='150'> Whether qualified for promotion to the higher class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + examflag + "</td>  </tr>  <tr><td width='3px' align='center'> 11.</td><td align='left' width='150'> Whether the student has paid all the fees due to the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> YES</td>  </tr>  <tr><td width='3px' align='center'> 12.</td><td align='left' width='150'> Date of Admission to the class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Adm_Date + " " + lateralOrRegular + "</td>  </tr>  <tr><td width='3px' align='center'> 13.</td><td align='left' width='150'> Date on which the student left the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + commencementofclassdate + "</td>  </tr>  <tr><td width='3px' align='center'> 14.</td><td align='left' width='150'> Date of issue of TC</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + dateofissue + "</td>  </tr>  <tr><td width='3px' align='center'> 15.</td><td align='left' width='150'> Student's Conduct and Character</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Conduct_Character + "</td>  </tr>  <tr><td width='3px' align='center'> 16.</td><td align='left' width='150'> Attendance</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + attendance + "</td>  </tr>  <tr><td width='3px' align='center'> 17.</td><td align='left' width='150'> Remarks</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + remarks + "</td>  </tr> </table> <center>  <table style='width: 95%; margin-top: 0px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'><tr> <td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>  Course of Study </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Academic Year </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Roll No </td> <td style='border: thin solid #000000;' align='center' class='style2'>  Register No </td></tr><tr> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center' class='style1'>  " + coursename + ". " + studied + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + periodofstudy + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + rollno + " </td> <td style='border: thin solid #000000; font-size: medium; border-top-style: none;'  align='center' class='style2'>  " + regno + " </td></tr>  </table> </center></div><div style='font-size: small; width: 95%; margin-top: 1px' align='left' class='style1'> Note: Erasure, fradulent or unauthenticated alteration in the certificate will lead to its cancellation</div><table style='width: 95%; margin-top: 80px; font-size: small;' cellpadding='0' cellspacing='0'>  <td class='style3'>  </td> </tr> <tr>  <td align='left' style='font-size: large' class='style3'>Declaration by the Student  </td>  <td style='font-weight: 600; font-size: large;' align='left'>COLLEGE SEAL  </td>  <td style='font-weight: bold; font-size: large;'>PRINCIPAL  </td> </tr> <tr>  <td align='left' style='height: 25px; margin-top: 5px' colspan='5'>I hereby declare that the particulars recorded against items 1 to 7 are correct and that no change will be demanded by me in future.  </td> </tr>  <tr>  <td align='left' colspan='2' style='font-weight: bold; font-size: large; height: 20px;padding-top: 20px;'>Signature of the Student  </td> </tr></table>  </center> </div></center>");
                                        }
                                        else
                                        {
                                            html.Append("<center><div style='height: 1046px; width: 100%; border: 0px solid black; margin-left: 5px; margin: 0px;'><center><div style='border: 1px solid black'> <table style='width: 100%; margin-top: 0px; font-size: small;' cellpadding='6' cellspacing='6'><tr style='margin-bottom:0px; padding-bottom:0px;'><td width='100px' style='margin-bottom:0px; padding-bottom:0px;'> <img src=" + photo + " alt='' style='height: 100px; width: 80px; margin-top: 1px;' /></td><td align='center' style='color:#8B4513; margin-bottom:0px; padding-bottom:0px;'><div style='font-weight:bold;font-size:x-large;'>  " + coll_name + "</div> <div style='font-weight:bold;font-size:medium;'>  " + catagory + "</div> <div style='font-weight:bold;font-size:small;'> " + affliatedby1 + "</div> <div style='font-size:small;'>  " + affliatedby2 + "</div> <div style='font-size:small;'>  " + affliatedby3 + "</div> <div style='font-weight:bold;font-size:small;'>  " + address3 + " - " + pincode + "</div></td><td width='120' style=' margin-bottom:0px; padding-bottom:0px;'> <img src=" + stdphoto + " alt='' style='height: 100px; width: 80px; margin-top: 1px;' /></td></tr></table> <table style='width: 100%; margin: 0px; padding:0px; font-size: small;' cellpadding='0' cellspacing='0'><tr style='margin:0px; padding:0px;'><td width='100px'></td><td align='center' style='font-size: large; font-weight: bold'> TRANSFER & CONDUCT CERTIFICATE</td><td style='font-weight: bold'> SL. No. :" + serialno + "</td></tr><tr><td><br/></td></tr> </table> <table style='width: 100%; margin-top: 0px; font-size: medium;' cellpadding='1' cellspacing='3'>  <tr><td width='3px' align='center'> 1.</td><td align='left' width='150'> Name of the Student</td><td style='font-weight: bold;' width='10'>:</td><td style='font-weight: bold' align='left' width='150'> " + studname + "</td></tr>  <tr><td width='3px' align='center'> 2.</td><td align='left' width='150'> Name of the Father</td><td style='font-weight: bold' width='10'> :</td><td align='left' width='150'> " + fathername + "</td></tr><tr><td width='3px' align='center'>    3.</td><td align='left' width='150'>Date of Birth as entered in +2 or Equivant certificate(in words)</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left' width='170'>" + dob + "<br /></brL><span style='font-size: x-small;'>" + F5dobwords + "</span></td></tr><tr><td width='3px' align='center'> 4.</td><td align='left' width='150'> Sex</td><td style='font-weight: bold' width='10'>:</td><td style='font-weight: normal' align='left'> " + sex + "</td> </tr> <tr> <td width='3px' align='center'> 5.</td><td align='left' width='150'> Nationality</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + nationality + "</td> </tr><tr><td width='3px' align='center'> 6.</td><td align='left' width='150'> Religion</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + religion + "</td>  </tr>  <tr><td width='3px' align='center'> 7.</td><td align='left' width='150'> Caste</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + caste + "</td>  </tr>  <tr><td width='3px' align='center'> 8.</td><td align='left' width='150'> Class in which the Student was studying at the time of leaving college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + yearLetters + coursename + ". " + studied + "</td>  </tr>  <tr><td width='3px' align='center'> 9.</td><td align='left' width='150'> First Language</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal'align='left'> " + partonelang + "</td>  </tr>  <tr><td width='3px' align='center'> 10.</td><td align='left' width='150'> Medium of Instruction</td><td style='font-weight: old' width='10'> :</td><td style='font-weight: normal' align='left'> " + mediumofstudy + "</td>  </tr>  <tr><td width='3px' align='center'> 11.</td><td align='left' width='150'> Whether qualified for promotion to the higher class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + examflag + "</td>  </tr>  <tr><td width='3px' align='center'> 12.</td><td align='left' width='150'> Whether the student has paid all the fees due to the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> YES</td>  </tr>  <tr><td width='3px' align='center'> 13.</td><td align='left' width='150'> Date of Admission to the class</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Adm_Date + "</td>  </tr>  <tr><td width='3px' align='center'> 14.</td><td align='left' width='150'> Date on which the student left the college</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + commencementofclassdate + "</td>  </tr>  <tr><td width='3px' align='center'> 15.</td><td align='left' width='150'> Date of issue of TC</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + dateofissue + "</td>  </tr>  <tr><td width='3px' align='center'> 16.</td><td align='left' width='150'> Student's Conduct and Character</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + Conduct_Character + "</td>  </tr>  <tr><td width='3px' align='center'> 17.</td><td align='left' width='150'> Attendance</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + attendance + "</td>  </tr>  <tr><td width='3px' align='center'> 18.</td><td align='left' width='150'> Remarks</td><td style='font-weight: bold' width='10'> :</td><td style='font-weight: normal' align='left'> " + remarks + "</td>  </tr> </table> <center>  <table style='width: 95%; margin-top: 0px; margin-bottom: 2px; font-size: medium;'cellpadding='5' cellspacing='0'><tr> <td style='border: thin solid #000000; border-right-style: none;' align='center'  class='style1'>  Course of Study </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Academic Year </td> <td style='border: thin solid #000000; border-right-style: none;' align='center'>  Roll No </td> <td style='border: thin solid #000000;' align='center' class='style2'>  Register No </td></tr><tr> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center' class='style1'>  " + coursename + ". " + studied + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + periodofstudy + " </td> <td style='border: thin solid #000000; font-size: medium; border-right-style: none;  border-top-style: none;' align='center'>  " + rollno + " </td> <td style='border: thin solid #000000; font-size: medium; border-top-style: none;'  align='center' class='style2'>  " + regno + " </td></tr>  </table> </center></div><div style='font-size: small; width: 95%; margin-top: 1px' align='left' class='style1'> Note: Erasure, fradulent or unauthenticated alteration in the certificate will lead to its cancellation</div><table style='width: 95%; margin-top: 69px; font-size: small;' cellpadding='0' cellspacing='0'>  <td class='style3'>  </td> </tr> <tr>  <td align='left' style='font-size: large' class='style3'>Declaration by the Student  </td>  <td style='font-weight: 600; font-size: large;' align='left'>COLLEGE SEAL  </td>  <td style='font-weight: bold; font-size: large;'>PRINCIPAL  </td> </tr> <tr>  <td align='left' style='height: 20px; margin-top: 3px' colspan='4'>I hereby declare that the particulars recorded against items 1 to 7 are correct and that no change will be demanded by me in future.  </td> </tr>  <tr>  <td align='left' colspan='2' style='font-weight: bold; font-size: large; height: 20px;padding-top: 20px;'>Signature of the Student  </td> </tr></table>  </center> </div></center>");
                                        }
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "6")
                                    {
                                        #region Bonafide
                                        html.Append(" <div style='height: 15cm; width: 21.1cm; border: 0px solid black; margin-left: 5px;  margin: 0px; page-break-after: always;'>  <center><div> <table style='width: 21.1cm; margin-top: 4.5cm; font-size: 14px; justify-content: space-around;  line-height: 40px; vertical-align: 10px;' cellpadding='8' cellspacing='8'>  <tr><td colspan='3' style='text-indent: 50px;'> I hereby certify that <b>" + studname + "</b>, S/O Mr. <b>" + fathername + "</b> with Register No. <b>" + regno + "</b> is a bonafide student of this College pursuing year <b>" + coursename + " - " + studied + "</b> course during the academic year <b>" + Convert.ToString(System.DateTime.Now.ToString("yyyy")) + " - " + (Convert.ToInt32(System.DateTime.Now.ToString("yyyy")) + 1) + "</b>. This College is affiliated to the <b>" + university + "</b>.</td>  </tr>  <tr><td colspan='3'></td>  </tr>  <tr><td colspan='3' align='left'> This Certificate is issued for the purpose of <b>" + BonafidePurpose + ".</b></td>  </tr>  <tr><td align='left'><b>DATE :" + Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy")) + "</b></td><td align='right'><b> For PRINCIPAL</b></td><td align='right'> &nbsp;</td>  </tr> </table></div>  </center> </div> <br>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "7")
                                    {
                                        #region course completed
                                        html.Append("  <div style='height: 15cm; width: 21.1cm; border: 0px solid black; margin-left: 5px;  margin: 0px; page-break-after: always;'>  <center><div> <table style='width: 21.1cm; margin-top: 4.5cm; font-size: 14px; justify-content: space-around;  line-height: 40px; vertical-align: 10px;' cellpadding='8' cellspacing='8'>  <tr><td colspan='3' style='text-indent: 50px;'>  I hereby certify that <b>" + studname + "</b>, with Register No. <b>" + regno + "</b>, was a student of this college in the <b>" + coursename + " - " + studied + "</b> course during the Academic years <b>" + periodofstudy + "</b>. He has completed the course successfully in <b> " + Last_exam_mon_year + "</b>. <br />" + collname + " is affiliated to the " + university + " and the medium of instruction is English. </td>  </tr>  <tr><td colspan='3'></td>  </tr>  <tr><td colspan='3'></td>  </tr>  <tr><td align='left'><b>DATE : " + Convert.ToString(System.DateTime.Now.ToString("dd/MM/yyyy")) + "</b></td><td align='right'> <b>For PRINCIPAL</b></td><td align='right'> </td>  </tr> </table></div>  </center> </div> <br>");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                    else if (ddlAppFormat.SelectedValue == "8")
                                    {
                                        #region MccSchool Tc
                                        html.Append("<div style='height: 845px; width: 21.4cm; border: 0px solid black; margin: 0px; page-break-after: always;'>  <center><div><table style='width: 100%;height: 845px; margin-top: 180px;margin-left: 10px; line-height: 8px;font-family: Book Antiqua; font-size: medium;' cellpadding='5' cellspacing='8'><tr><td colspan='2' align='left' width='48%' style='line-height: 2cm;'><b> SERIAL NO: " + serialno + "</b></td> </tr> <tr> <td align='left' width='48%'>01. Name of the Student</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + studname + "</td>  </tr>  <tr>  <td align='left' width='48%'>02. Register Number</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + regno + "</td>  </tr>  <tr> <td align='left'   width='48%'>03. Name of the Father / Mother</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + fathername + "</td>  </tr>  <tr>  <td align='left' width='48%'>04. Gender</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + sex + "</td>  </tr>  <tr> <td align='left'   width='48%'>05. Date of Birth</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + dob + "</td>  </tr>  <tr> <td align='left'   width='48%'> 06. Nationality</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + nationality + "</td>  </tr>  <tr>  <td align='left' width='48%'> 07. Religion, Caste and Community</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> Refer Community Certificate</td>  </tr>  <tr>  <td align='left' width='48%'> 08. Month of Admission</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + monthofadmission + "</td>  </tr>  <tr>  <td align='left' width='48%'>09. Month of Leaving</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + monthofleaving + "</td>  </tr>  <tr>  <td align='left' width='48%' style='line-height: 16px;'> 10. Course and Major subject studied</td><td align='center' width='5px'> :</td> <td align='left' width='48%'  style='font-weight: bold;line-height: 16px;'> " + coursename + " - " + studied + "</td>  </tr>  <tr>  <td align='left' width='48%'> 11. Language studied under Part I</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + partonelang + "</td>  </tr>  <tr>  <td align='left' width='48%'> 12. Medium of Instruction</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + mediumofstudy + "</td>  </tr>  <tr>  <td align='left' width='48%' style='line-height: 15px;'>  13. Whether qualified for promotion to a higher class ?</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> Refer Mark Sheet</td>  </tr>  <tr>  <td align='left' width='48%'> 14. Conduct & Character</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: normal'> " + Conduct_Character + "</td>  </tr>  <tr>  <td align='left' width='48%'>15. Date of Issue</td><td align='center' width='5px'> :</td> <td align='left' width='48%' style='font-weight: bold'> " + dateofissue + "</td>  </tr>  <tr>  <td align='left' style='font-weight: bold; line-height: 4cm;' colspan='2'  width='48%'> Seal</td><td align='right' style='font-weight: bold;'   width='48%'> Signature of the Principal</td>  </tr>  <tr> <td align='left' style='font-weight: bold; line-height: 5px;' colspan='2'width='48%'> Declaration by the Student :</td>  </tr><tr>  <td colspan='4' style='font-weight: bold;line-height: 18px;'>I hereby declare that the particulars recorded above are correct and that no change will be demanded by me in future.</td></tr>  <tr>  <td width='48%'></td><td width='5px'></td>  <td align='right' style='font-weight: bold; line-height: 3cm;' width='48%'> Signature of the Student</td>  </tr> </table></div></center> </div></br> ");
                                        contentDiv.InnerHtml = html.ToString();
                                        #endregion
                                    }
                                }
                            }
                        }
                    #endregion
                    }
                }
                contentDiv.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "btn_print", "PrintDiv();", true);
            }
        
        catch 
        { 

        }
    }

    public string gettextvalue(string textcri, string subjename)
    {
        string subjec_no = "";
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                string select_subno = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + Convert.ToString(ddlcollege.SelectedItem.Value) + " and TextVal='" + subjename + "'";
                ds.Clear();
                ds = d2.select_method_wo_parameter(select_subno, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                }
                else
                {
                    string insertquery = "insert into textvaltable(TextCriteria,TextVal,college_code) values('" + textcri + "','" + subjename + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "')";
                    int result = d2.update_method_wo_parameter(insertquery, "Text");
                    if (result != 0)
                    {
                        string select_subno1 = "select TextCode from textvaltable where TextCriteria='" + textcri + "' and college_code =" + Convert.ToString(ddlcollege.SelectedItem.Value) + " and TextVal='" + subjename + "'";
                        ds.Clear();
                        ds = d2.select_method_wo_parameter(select_subno1, "Text");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            subjec_no = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                        }
                    }
                }
            }
        }
        catch
        {
        }
        return subjec_no;
    }

    protected string dateinwords(string dob)
    {
        string dig = "";
        try
        {
            if (dob.Trim() != "")
            {
                string[] dobdate = dob.Split('/');
                int dat = 0;
                if (dobdate.Length > 0)
                {
                    for (int k = 0; k < dobdate.Length; k++)
                    {
                        string date = "";
                        if (k != 1)
                        {
                            int.TryParse(Convert.ToString(dobdate[k]), out dat);
                            date = ReuasableMethods.ConvertNumbertoWords(dat);
                        }
                        else
                        {
                            date = rs.returnMonthName(Convert.ToInt32(dobdate[1].TrimStart('0')));
                        }
                        dig += " " + date;
                    }
                }
            }
        }
        catch { }
        return dig;
    }

    public string romanLetter(string numeral)
    {
        string romanLettervalue = String.Empty;
        if (numeral.Trim() != String.Empty)
        {
            romanLettervalue = numeral;
            switch (numeral)
            {
                case "I STD":
                    romanLettervalue = "First Standard";
                    break;
                case "II STD":
                    romanLettervalue = "Second Standard";
                    break;
                case "III STD":
                    romanLettervalue = "Thrid Standard";
                    break;
                case "IV STD":
                    romanLettervalue = "Fourth Standard";
                    break;
                case "V STD":
                    romanLettervalue = "Fifth Standard";
                    break;
                case "VI STD":
                    romanLettervalue = "Sixth Standard";
                    break;
                case "VII STD":
                    romanLettervalue = "Seventh Standard";
                    break;
                case "VIII STD":
                    romanLettervalue = "Eigth Standard";
                    break;
                case "IX STD":
                    romanLettervalue = "Nineth Standard";
                    break;
                case "X STD":
                    romanLettervalue = "Tenth Standard"; break;
                case "XI STD":
                    romanLettervalue = "Eleventh Standard"; break;
                case "XII STD":
                    romanLettervalue = "Twelveth Standard";
                    break;
            }
        }
        return romanLettervalue.ToUpper();
    }

    public string returnYearfornum(string cursem)
    {
        switch (cursem)
        {
            case "1":
                cursem = "FIRST YEAR";
                break;
            case "2":
                cursem = "SECOND YEAR";
                break;
            case "3":
                cursem = "THIRD YEAR";   
                break;
            case "4":
                cursem = "FOURTH YEAR";
                break;
            case "5":
                cursem = "FIFTH YEAR";
                break;
            case "6":
                cursem = "SIXTH YEAR";
                break;
            case "7":
                cursem = "SEVENTH YEAR";
                break;
        }
        return cursem;
    }

    public void loadclass()
    {
        try
        {
            String sql = "   select Dept_Name,Degree_Code from Degree d,Department dt,course c where d.Dept_Code=dt.Dept_Code and c.Course_Id=d.Course_Id and d.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            ds1.Clear();
            ds1.Reset();
            ds1 = d2.select_method_wo_parameter(sql, "text");

            if (ds1.Tables[0].Rows.Count > 0)
            {
                ddl_joinclass.DataSource = ds1.Tables[0];
                ddl_joinclass.DataTextField = "Dept_Name";
                ddl_joinclass.DataValueField = "Degree_Code";
                ddl_joinclass.DataBind();
            }

        }
        catch
        {
        }
    }

    /// <summary>
    /// college tc add by barath
    /// </summary>  
    protected void imagebtnpopclose4_Click(object sender, EventArgs e)
    {
        //txt_admissionno.Enabled = true;
        pop_clg_tc.Visible = false;
        //clear();
    }

    protected void btn_saveclg_Click(object sender, EventArgs e)
    {
        if (txt_regno.Text.Trim() != "")
        {
            #region community region nation caste
            string nationality = "0";
            if (Convert.ToString(ddlcountry1.SelectedItem.Text.ToString().ToUpper()) == "OTHERS")
            {
                string national = Convert.ToString(txt_othernationality1.Text.ToString().ToUpper());
                nationality = gettextvalue("citi", national);
            }
            else
            {
                if (Convert.ToString(ddlcountry1.SelectedItem.Value) != "Select")
                {
                    nationality = Convert.ToString(ddlcountry1.SelectedItem.Value);
                }
            }
            int religion = 0; int community = 0; int caste = 0; int attendance = 0;
            if (ddlreligion1.SelectedItem.Text != "Select")
            {
                if (ddlreligion1.SelectedItem.Text != "Others" && ddlreligion1.SelectedItem.Text.ToUpper() != "CHRISTIAN")
                {
                    int.TryParse(Convert.ToString(ddlreligion1.SelectedItem.Value), out religion);
                }
                else if (ddlreligion1.SelectedItem.Text.ToUpper() == "CHRISTIAN")
                {
                    int.TryParse(Convert.ToString(ddlreligion1.SelectedItem.Value), out religion);
                }
                else
                {
                    if (txt_otherreligion1.Text.Trim() != "")
                    {
                        string relig = Convert.ToString(txt_otherreligion1.Text.First().ToString().ToUpper() + txt_otherreligion1.Text.Substring(1));
                        if (relig.Trim() != "")
                        {
                            int.TryParse((gettextvalue("relig", relig)), out religion);
                        }
                    }
                }
            }
            if (ddlcoummunity1.SelectedItem.Text != "Select")
            {
                if (ddlcoummunity1.SelectedItem.Text != "Others")
                    int.TryParse(Convert.ToString(ddlcoummunity1.SelectedItem.Value), out community);
                else
                {
                    string comm = Convert.ToString(txtCommunity1.Text.ToString().ToUpper());
                    int.TryParse(gettextvalue("comm", comm), out community);
                }
            }
            if (ddl_caste1.SelectedItem.Text != "Select")
            {
                if (ddl_caste1.SelectedItem.Text != "Others")
                {
                    int.TryParse(Convert.ToString(ddl_caste1.SelectedItem.Value), out caste);
                }
                else
                {
                    string cast = Convert.ToString(txt_caste1.Text.ToString().ToUpper());
                    int.TryParse(gettextvalue("caste", cast), out caste);
                }
            }
            if (ddl_attendance.SelectedItem.Text != "Select")
            {
                if (ddl_attendance.SelectedItem.Text != "Others")
                {
                    int.TryParse(Convert.ToString(ddl_attendance.SelectedItem.Value), out attendance);
                }
                else
                {
                    string cast = Convert.ToString(txt_attendance.Text.ToString().ToUpper());
                    int.TryParse(gettextvalue("ATTYP", cast), out attendance);
                }
            }


            #endregion

            string generalconduct = ddl_generalconduct.SelectedItem.Value;
            DateTime dobdate = new DateTime();
            string dd = Convert.ToString(ddldobdate1.SelectedItem.Text).TrimStart('0');
            string mm = Convert.ToString(ddldobMonth1.SelectedItem.Value);
            string yyyy = Convert.ToString(ddldobYear1.SelectedItem.Text);
            DateTime.TryParse(mm + "/" + dd + "/" + yyyy, out dobdate);
            string mediumofstudy = gettextvalue("PLang", txt_mudiumofstudy1.Text.ToString().ToUpper());
            string studentname = txt_studname1.Text;
            string mothername = txt_mothername1.Text;
            string fathername = txt_fathername1.Text;
            string programmcompleted = programeCompleted.Text;
            string exammonyear = txt_exammonthandyear.Text;
            string migrationslno = txt_migrationserielno.Text;
            string serialno = txt_serial_no.Text;
            string bonafidePurpose = txtPurpose.Text;
            //string leavinginstition = txt_leavinginstition.Text;
            //string commencementofclass = txt_commencementofclass.Text;
            string leavinginstition = txt_doLeaving.Text;
            string dateOfAdmission = txt_doAdmission.Text;
            //string periodofstudied = txt_periodofstudied.Text;
            string periodofstudied = string.Empty;

            if (delFlagValue.Text == "1")
            {
                if (txt_periodofstudied.Text.Trim() != "")
                {
                    periodofstudied = Convert.ToString(txt_periodofstudied.Text.Trim());
                }
                else
                {
                    string discontinueYear = d2.GetFunction("select datepart(yyyy,discontinue_date)discontinue_date from discontinue where app_no='" + lbl_app_no1.Text.Trim() + "'");
                    string studbatchyear = d2.GetFunction("select batch_year from registration where app_no=" + lbl_app_no1.Text.Trim());
                    periodofstudied = Convert.ToString(studbatchyear + " - " + discontinueYear);
                }
            }
            else
            {

                periodofstudied = txt_periodofstudied.Text;
            }
            string part1language = gettextvalue("Cplan", txt_part1language1.Text.ToString().ToUpper());
            string remarks = gettextvalue("remrk", txt_remarks1.Text.ToUpper());

            //string lastattendclass = returndatetime(ddl_lastattendedclass.SelectedItem.Text);
            string dateofissuecertificate = returndatetime(txt_dateofissuecertificate.Text);
            //  string commenceofclassdate = ddl_commencementofclass.SelectedValue;

            string migrationdate = ddl_dateoofissuemigration.SelectedValue;
            // string dateofissuetcmade = ddl_tccertificateissuedate.SelectedValue;
            // commenceofclassdate = (commenceofclassdate == "0" ? "" : commenceofclassdate);
            migrationdate = (migrationdate == "0" ? "" : migrationdate);
            //dateofissuetcmade = (dateofissuetcmade == "0" ? "" : dateofissuetcmade);

            string aadharcardno = Convert.ToString(txt_Aadharcardno.Text) + "" + Convert.ToString(txt_Aadharcardno2.Text) + "" + Convert.ToString(txt_Aadharcardno3.Text);
            //string admissiondate = Convert.ToString(txt_admissiondate.Text);
            string admissiondate = Convert.ToString(txt_doAdmission.Text);

            DateTime dateofleavingcollegeDt = new DateTime();
            string dateofleaving = string.Empty;
            if (!string.IsNullOrEmpty(txt_doLeaving.Text))
            {

                string[] splitdate = txt_doLeaving.Text.Split('/');
                dateofleavingcollegeDt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
                dateofleaving = dateofleavingcollegeDt.ToString("MM/dd/yyyy");

            }
            if (Convert.ToString(dateofleavingcollegeDt.ToString("dd/MM/yyyy")) == "01/01/1900" || Convert.ToString(dateofleavingcollegeDt.ToString("dd/MM/yyyy")) == "01/01/0001")
                dateofleaving = "";
            else
                dateofleaving = dateofleavingcollegeDt.ToString("MM/dd/yyyy");
            DateTime admissiondateDt = new DateTime();
            if (admissiondate.Trim() != "")
            {
                string[] splitdate = admissiondate.Split('/');
                admissiondateDt = Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]);
            }
            int serialSet = 0;
            if (cb_serialnoSettings.Checked)
                serialSet = 1;
            q1 = "  update Registration set dateofleaving='" + dateofleaving + "',Stud_Name='" + studentname + "',Adm_Date='" + (admissiondateDt.ToString("MM/dd/yyyy") == "01/01/0001" ? "" : admissiondateDt.ToString("MM/dd/yyyy")) + "' where App_No='" + lbl_app_no1.Text.Trim() + "'";
            q1 += "  update applyn set medium_ins='" + mediumofstudy + "',partI_Language='" + part1language + "', Aadharcard_no='" + aadharcardno + "',parent_name='" + fathername + "',mother='" + mothername + "',dob='" + (dobdate.ToString("MM/dd/yyyy") == "01/01/0001" ? "" : dobdate.ToString("MM/dd/yyyy")) + "', citizen='" + nationality + "',community='" + community + "',religion='" + religion + "',caste='" + caste + "',remarks='" + remarks + "',LastTCDate='" + dateofissuecertificate + "' where App_No='" + lbl_app_no1.Text + "'";    //modified by Mullai
            q1 += " if exists(select linkname from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' )update New_InsSettings set LinkName='" + serialSet + "' where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' else insert into New_InsSettings (LinkName,LinkValue,college_code,user_code)values('TC_SerialNoSettings','" + serialSet + "','" + Convert.ToString(ddlcollege.SelectedItem.Value) + "','" + usercode + "')";
            q1 += "   if exists(select app_no from Tc_details where app_no='" + lbl_app_no1.Text.Trim() + "') update Tc_details set program_completed='" + programmcompleted + "',Last_exam_mon_year='" + exammonyear + "',Migration_Sl_No='" + migrationslno + "',General_conduct='" + generalconduct + "' ,migration_date='" + migrationdate + "',Medium_study='" + mediumofstudy + "',Last_Studied_Class='" + leavinginstition + "',periodofstudied='" + periodofstudied + "',dateofissuecertificate='" + dateofissuecertificate + "',Serial_no='" + serialno + "',Part1Language ='" + part1language + "',Attendance_type='" + attendance + "',BonafidePurpose='" + bonafidePurpose + "' where App_no='" + lbl_app_no1.Text + "' else insert into Tc_details (program_completed,Last_exam_mon_year, Migration_Sl_No, General_conduct,migration_date, Medium_study,Last_Studied_Class,periodofstudied,app_no,dateofissuecertificate, Serial_no,Part1Language,Attendance_type,BonafidePurpose) values('" + programmcompleted + "','" + exammonyear + "','" + migrationslno + "','" + generalconduct + "','" + migrationdate + "','" + mediumofstudy + "','" + leavinginstition + "','" + periodofstudied + "','" + lbl_app_no1.Text + "','" + dateofissuecertificate + "','" + serialno + "','" + part1language + "','" + attendance + "','" + bonafidePurpose + "')";//,commencement_date='" + commenceofclassdate + "' , commencement_date ,'" + commenceofclassdate + "' ,commencementofclass='" + commencementofclass + "' , commencementofclass  ,'" + commencementofclass + "'  ,laststudieddate='" + lastattendclass + "'  ,laststudieddate  ,'" + lastattendclass + "'  ,Transfer_cert_made= '" + dateofissuetcmade + "'
            int updat = d2.update_method_wo_parameter(q1, "text");
            if (updat != 0)
            {
                lblalerterr.Text = "Updated Successfully";
                alertpopwindow.Visible = true;
                clear();
                pop_clg_tc.Visible = false;
            }
        }
    }

    protected string returndatetime(string date)
    {
        string date1 = "";
        if (date.Trim() != "")
        {
            string[] splitdate = date.Split('/');
            if (splitdate.Length == 3)
                date1 = Convert.ToString(Convert.ToDateTime(splitdate[1] + "/" + splitdate[0] + "/" + splitdate[2]));
        }
        return date1;
    }

    protected void btn_addgroup_Click(object sender, EventArgs e)
    {
        try
        {
            string group = Convert.ToString(txt_addgroup.Text);
            group = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(group);
            if (lbl_addgroup.Text.ToUpper() == "CONDUCT & CHARACTER")
            {
                if (txt_addgroup.Text != "")
                {
                    string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='General conduct' and CollegeCode='" + collegecode1 + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='General conduct' and CollegeCode='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','General conduct','" + collegecode1 + "')";
                    int insert = d2.update_method_wo_parameter(sql, "Text");
                    if (insert != 0)
                    {
                        alertpopwindow.Visible = true;
                        lblalerterr.Text = "Saved Successfully";
                        txt_addgroup.Text = "";
                        plusdiv.Visible = false;
                        panel_addgroup.Visible = false;
                    }
                    bindgeneralconduct();
                    txt_addgroup.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Conduct & Character";
                }
            }
            else if (lbl_addgroup.Text.ToUpper() == "DATE")
            {
                if (txt_addgroup.Text != "")
                {
                    bool date = checkdate(group);
                    if (date)
                    {
                        string sql = "if exists ( select * from CO_MasterValues where MasterValue='" + group + "' and MasterCriteria='Tc Date' and CollegeCode='" + collegecode1 + "') Update CO_MasterValues set MasterValue='" + group + "' where MasterValue='" + group + "' and MasterCriteria='Tc Date' and CollegeCode='" + collegecode1 + "' else insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode) values('" + group + "','Tc Date','" + collegecode1 + "')";
                        int insert = d2.update_method_wo_parameter(sql, "Text");
                        if (insert != 0)
                        {
                            alertpopwindow.Visible = true;
                            lblalerterr.Text = "Saved Successfully";
                            txt_addgroup.Text = "";
                            plusdiv.Visible = false;
                            panel_addgroup.Visible = false;
                        }
                        bindtcdate();
                    }
                    else
                    {
                        lblerror.Visible = true;
                        lblerror.Text = "Enter the Valid Date";
                    }
                    txt_addgroup.Text = "";
                }
                else
                {
                    plusdiv.Visible = true;
                    lblerror.Visible = true;
                    lblerror.Text = "Enter the Date";
                }
            }
        }
        catch
        {
        }
    }

    protected bool checkdate(string date)
    {
        bool chk = false;
        DateTime dDate = new DateTime();
        string dat = (date.Split('/')[1] + "/" + date.Split('/')[0] + "/" + date.Split('/')[2]);
        if (DateTime.TryParse(dat, out dDate))
            chk = true;
        return chk;
    }

    protected void btn_exitaddgroup_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = false;
        panel_addgroup.Visible = false;
        txt_addgroup.Text = "";
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lbl_addgroup.Text = "Conduct & Character";
        txt_addgroup.Attributes.Add("placeholder", "");
        txt_addgroup.Attributes.Add("maxlength", "150");
        lblerror.Visible = false;
    }

    protected void btnminus_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_generalconduct.SelectedIndex != 0)
            {
                string sql = "delete from CO_MasterValues where MasterCode='" + ddl_generalconduct.SelectedItem.Value.ToString() + "' and MasterCriteria='General conduct' and collegecode='" + ddlcollege.SelectedValue + "'";
                int delete = d2.update_method_wo_parameter(sql, "Text");
                if (delete != 0)
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "Deleted Successfully";
                }
                else
                {
                    alertpopwindow.Visible = true;
                    lblalerterr.Text = "No Record Selected";
                }
            }
            else
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "No Record Selected";
            }
            bindgeneralconduct();
        }
        catch { }
    }

    protected void btnplus1_Click(object sender, EventArgs e)
    {
        lbl_addgroup.Text = "Date"; txt_addgroup.Attributes.Add("maxlength", "10");
        txt_addgroup.Attributes.Add("placeholder", "DD/MM/YYYY");
        plusdiv.Visible = true;
        panel_addgroup.Visible = true;
        lblerror.Visible = false;
    }

    protected void btnminus1_Click(object sender, EventArgs e)
    {
        try
        {
            //if (ddl_tccertificateissuedate.SelectedIndex != 0)
            //{
            //    string sql = "delete from CO_MasterValues where MasterCode='" + ddl_tccertificateissuedate.SelectedItem.Value.ToString() + "' and MasterCriteria='Tc Date' and collegecode='" + ddlcollege.SelectedValue + "'";
            //    int delete = d2.update_method_wo_parameter(sql, "Text");
            //    if (delete != 0)
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "Deleted Successfully";
            //    }
            //    else
            //    {
            //        alertpopwindow.Visible = true;
            //        lblalerterr.Text = "No Record Selected";
            //    }
            //}
            //else
            //{
            //    alertpopwindow.Visible = true;
            //    lblalerterr.Text = "No Record Selected";
            //}
            //bindtcdate();
        }
        catch { }
    }

    protected void bindgeneralconduct()
    {
        try
        {
            ddl_generalconduct.Items.Clear();
            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue  from CO_MasterValues where MasterCriteria ='General conduct' and CollegeCode ='" + ddlcollege.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_generalconduct.DataSource = ds;
                ddl_generalconduct.DataTextField = "MasterValue";
                ddl_generalconduct.DataValueField = "MasterCode";
                ddl_generalconduct.DataBind();
            }
            ddl_generalconduct.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch { }
    }
    protected void bindtcdate()
    {
        try
        {
            //ddl_tccertificateissuedate.Items.Clear();
            ddl_dateoofissuemigration.Items.Clear();
            //ddl_commencementofclass.Items.Clear();
            // ddl_dateofissuecertificate.Items.Clear();
            // ddl_lastattendedclass.Items.Clear();

            ds.Clear();
            string sql = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria ='Tc Date' and CollegeCode ='" + ddlcollege.SelectedValue + "'";
            ds = d2.select_method_wo_parameter(sql, "TEXT");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //ddl_tccertificateissuedate.DataSource = ds;
                //ddl_tccertificateissuedate.DataTextField = "MasterValue";
                //ddl_tccertificateissuedate.DataValueField = "MasterCode";
                //ddl_tccertificateissuedate.DataBind();

                ddl_dateoofissuemigration.DataSource = ds;
                ddl_dateoofissuemigration.DataTextField = "MasterValue";
                ddl_dateoofissuemigration.DataValueField = "MasterCode";
                ddl_dateoofissuemigration.DataBind();

                //ddl_commencementofclass.DataSource = ds;
                //ddl_commencementofclass.DataTextField = "MasterValue";
                //ddl_commencementofclass.DataValueField = "MasterCode";
                //ddl_commencementofclass.DataBind();

                //ddl_dateofissuecertificate.DataSource = ds;
                //ddl_dateofissuecertificate.DataTextField = "MasterValue";
                //ddl_dateofissuecertificate.DataValueField = "MasterCode";
                //ddl_dateofissuecertificate.DataBind();

                //ddl_lastattendedclass.DataSource = ds;
                //ddl_lastattendedclass.DataTextField = "MasterValue";
                //ddl_lastattendedclass.DataValueField = "MasterCode";
                //ddl_lastattendedclass.DataBind();
            }

            //  ddl_tccertificateissuedate.Items.Insert(0, new ListItem("Select", "0"));
            ddl_dateoofissuemigration.Items.Insert(0, new ListItem("Select", "0"));
            //ddl_commencementofclass.Items.Insert(0, new ListItem("Select", "0"));
            //  ddl_dateofissuecertificate.Items.Insert(0, new ListItem("Select", "0"));
            // ddl_lastattendedclass.Items.Insert(0, new ListItem("Select", "0"));
        }
        catch { }
    }

    protected void txt_regno_Onchange(object sender, EventArgs e)
    {
        try
        {
            if (txt_regno.Text.Trim() != "")
            {
                string regno = Convert.ToString(txt_regno.Text.Trim()).Trim();
                q1 = "    select r.app_no,(select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Medium_study) and TextCriteria='PLang') Medium_study,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, t.Part1Language) and TextCriteria='Cplan' ))partI_Language,CONVERT(varchar(10),a.Certificate_Date,103)Certificate_Date,r.Roll_Admit,r.Stud_Name,a.parent_name,a.mother,a.guardian_name,a.dob,a.citizen,a.caste,CONVERT(varchar(10),r.Adm_Date,103)Adm_Date,r.degree_code Last_studiedclass, t.Annualexamination_result, t.noofattempts,t.subjectstudied,t.Qualified_promotion,t.Paid_dues,t.General_conduct, t.Dateofapplcertificate,t.dateofissuecertificate,a.caste,a.religion,a.community,t.categorytype,t.MedicalInspection, t.laststudieddate,Serial_no,t.program_completed,t.Last_exam_mon_year,t.Migration_Sl_No,General_conduct as Conduct_Character,Last_Studied_Class,commencementofclass, Medium_study,migration_date,commencement_date,CONVERT(varchar(10),dateofissuecertificate,103)dateofissuecertificate1,CONVERT(varchar(10),laststudieddate,103)laststudieddate1,r.Reg_No,dateofissuecertificate as dateofissuecertificate1  , periodofstudied, Transfer_cert_made,a.Aadharcard_no,t.Attendance_type,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.remarks)))remarks,r.tcserialNo as AutoSerialno,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.partI_Language)))as ApartI_Language,UPPER((select textval from textvaltable where CONVERT(varchar,textcode)=CONVERT(varchar, a.medium_ins)))as Amedium_ins,convert(varchar(10), dateofleaving,103)dateofleaving,t.BonafidePurpose from applyn a,Registration r left join Tc_details t on r.App_No=t.App_no where  a.app_no=r.App_No and r.Reg_No='" + regno + "'";
                q1 += " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                q1 += " SELECT certacrno FROM TEmCertSerialSettings where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                ds1.Clear();
                ds1 = d2.select_method_wo_parameter(q1, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    #region college
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        //txt_regno.Enabled = false;
                        txt_regno.Text = regno;
                        lbl_app_no1.Text = Convert.ToString(dr["app_no"]);
                        string fathername = Convert.ToString(dr["parent_name"]);
                        string mothername = Convert.ToString(dr["mother"]);
                        string guardian = Convert.ToString(dr["guardian_name"]);
                        string dob = Convert.ToString(dr["dob"]);
                        string country = Convert.ToString(dr["citizen"]);
                        string caste = Convert.ToString(dr["caste"]);
                        string religion = Convert.ToString(dr["religion"]);
                        string community = Convert.ToString(dr["community"]);
                        string studname = Convert.ToString(dr["stud_name"]);
                        string roll_admit = Convert.ToString(dr["roll_admit"]);
                        string Attendance_type = Convert.ToString(dr["Attendance_type"]);
                        string part1language = Convert.ToString(dr["ApartI_Language"]);//partI_Language
                        string remarks = Convert.ToString(dr["remarks"]);
                        string Serial_no = Convert.ToString(dr["Serial_no"]);
                        if (ds1.Tables[1].Rows.Count > 0)
                        {
                            if (Convert.ToString(ds1.Tables[1].Rows[0]["linkvalue"]) == "1")
                            {
                                if (ds1.Tables[2].Rows.Count > 0)
                                    Serial_no = Convert.ToString(ds1.Tables[2].Rows[0]["certacrno"]) + Convert.ToString(dr["AutoSerialno"]);
                                cb_serialnoSettings.Checked = true;
                                txt_serial_no.Enabled = false;
                            }
                            else
                                txt_serial_no.Enabled = true;
                        }
                        txt_studname1.Text = studname;
                        txt_mothername1.Text = mothername;
                        txt_fathername1.Text = fathername;
                        txt_part1language1.Text = part1language;
                        txt_remarks1.Text = remarks;
                        txt_serial_no.Text = Serial_no;
                        if (dob.Trim() != "")
                        {
                            try
                            {
                                DateTime dobdate = new DateTime();
                                DateTime.TryParse(dob, out dobdate);
                                ddldobdate1.SelectedIndex = ddldobdate1.Items.IndexOf(ddldobdate1.Items.FindByText(Convert.ToString((dobdate.ToString("dd"))).TrimStart('0')));
                                ddldobMonth1.SelectedIndex = ddldobMonth1.Items.IndexOf(ddldobMonth1.Items.FindByValue(dobdate.ToString("MM")));
                                ddldobYear1.SelectedIndex = ddldobYear1.Items.IndexOf(ddldobYear1.Items.FindByText(dobdate.ToString("yyyy")));
                            }
                            catch { }
                        }
                        ddl_attendance.SelectedIndex = ddl_attendance.Items.IndexOf(ddl_attendance.Items.FindByValue(Attendance_type));
                        ddl_caste1.SelectedIndex = ddl_caste1.Items.IndexOf(ddl_caste1.Items.FindByValue(caste));
                        ddlcountry1.SelectedIndex = ddlcountry1.Items.IndexOf(ddlcountry1.Items.FindByValue(country));
                        ddlcoummunity1.SelectedIndex = ddlcoummunity1.Items.IndexOf(ddlcoummunity1.Items.FindByValue(community));
                        ddlreligion1.SelectedIndex = ddlreligion1.Items.IndexOf(ddlreligion1.Items.FindByValue(religion));

                        programeCompleted.Text = Convert.ToString(dr["program_completed"]);
                        txt_exammonthandyear.Text = Convert.ToString(dr["Last_exam_mon_year"]);
                        txt_migrationserielno.Text = Convert.ToString(dr["Migration_Sl_No"]);

                        //txt_leavinginstition.Text = Convert.ToString(dr["Last_Studied_Class"]);
                        //txt_commencementofclass.Text = Convert.ToString(dr["commencementofclass"]);
                        txt_mudiumofstudy1.Text = Convert.ToString(dr["Amedium_ins"]);//Medium_study
                        txt_periodofstudied.Text = Convert.ToString(dr["periodofstudied"]);
                        txt_admissiondate.Text = Convert.ToString(dr["Adm_Date"]);
                        //txt_aadharcardno.Text = Convert.ToString(dr["Aadharcard_no"]);
                        if (Convert.ToString(dr["Aadharcard_no"]).Trim() != "")
                        {
                            try
                            {
                                string aadhar = Convert.ToString(dr["Aadharcard_no"]).Trim();
                                if (aadhar.Length == 12)
                                {
                                    txt_Aadharcardno.Text = aadhar.Substring(0, 4);
                                    txt_Aadharcardno2.Text = aadhar.Substring(4, 4);
                                    txt_Aadharcardno3.Text = aadhar.Substring(8, 4);
                                }
                            }
                            catch { }
                        }
                        string dateoofissuemigration = Convert.ToString(dr["migration_date"]);
                        string commencementofclassdate = Convert.ToString(dr["dateofleaving"]);//commencement_date
                        string dateofissuecertificate = Convert.ToString(dr["dateofissuecertificate1"]);
                        string lastattendedclass = Convert.ToString(dr["laststudieddate1"]);
                        string tccertificateissuedate = Convert.ToString(dr["Transfer_cert_made"]);
                        txtPurpose.Text = Convert.ToString(dr["BonafidePurpose"]);

                        ddl_dateoofissuemigration.SelectedIndex = ddl_dateoofissuemigration.Items.IndexOf(ddl_dateoofissuemigration.Items.FindByValue(dateoofissuemigration));
                        //ddl_commencementofclass.SelectedIndex = ddl_commencementofclass.Items.IndexOf(ddl_commencementofclass.Items.FindByText(commencementofclassdate));
                        //   ddl_dateofissuecertificate.SelectedIndex = ddl_dateofissuecertificate.Items.IndexOf(ddl_dateofissuecertificate.Items.FindByText(dateofissuecertificate));
                        // ddl_lastattendedclass.SelectedIndex = ddl_lastattendedclass.Items.IndexOf(ddl_lastattendedclass.Items.FindByText(lastattendedclass));
                        //   ddl_tccertificateissuedate.SelectedIndex = ddl_tccertificateissuedate.Items.IndexOf(ddl_tccertificateissuedate.Items.FindByValue(tccertificateissuedate));
                        ddl_generalconduct.SelectedIndex = ddl_generalconduct.Items.IndexOf(ddl_generalconduct.Items.FindByValue(Convert.ToString(dr["Conduct_Character"])));
                    }
                    #endregion
                }
                else
                {
                    lblalerterr.Text = "Please Enter Valid Register No";
                    alertpopwindow.Visible = true;
                }
            }
        }
        catch { }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getregno(string prefixText, string contextKey)
    {
        List<string> name = new List<string>();
        try
        {
            string college_code = contextKey;
            WebService ws = new WebService();
            string query = " select r.reg_no from applyn a,Registration r where a.app_no=r.App_No and  r.reg_no like '" + prefixText + "%' and r.college_code='" + college_code + "'";
            name = ws.Getname(query);
        }
        catch { return name; }
        return name;
    }

    protected void bind_TcFormate()
    {
        try
        {
            string grouporusercode = "";
            if ((Session["group_code"].ToString().Trim() != "") && (Session["group_code"].ToString().Trim() != "0") && (Session["group_code"].ToString().Trim() != "-1"))
                grouporusercode = " and group_code=" + Session["group_code"].ToString().Trim() + "";
            else
                grouporusercode = " and usercode=" + Session["usercode"].ToString().Trim() + "";
            //string q1 = "select value from Master_Settings where settings='TC Format Rights' " + grouporusercode + "";
            //string q1 = " select CertificateName,certifcateformat from CertificateNameDetail where Collegecode='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "' " + grouporusercode + " and isnull(CertifcateFormat,'-1')<>'-1'";

            string q1 = "select * from Master_Settings where settings='CertificateFormatSetting'  " + grouporusercode + "";

            DataSet tcset = new DataSet();
            ds.Clear();
            ds = d2.select_method_wo_parameter(q1, "text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlAppFormat.Items.Clear();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    string[] splitvalue = Convert.ToString(row["value"]).Split('$');  //modified by prabha on feb 15 2018
                    if (splitvalue.Length > 0)
                    {
                        string certName = dirAcc.selectScalarString("select CertificateName from CertificateNameDetail where Certificate_ID='" + splitvalue[0].ToString().Trim() + "'");
                        ListItem li = new ListItem();
                        li.Text = certName;
                        li.Value = splitvalue[1].ToString().Trim();

                        ddlAppFormat.Items.Add(li);
                    }
                }
            }
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    ddlAppFormat.DataSource = ds.Tables[0];
            //    ddlAppFormat.DataTextField = "CertificateName";
            //    ddlAppFormat.DataValueField = "certifcateformat";
            //    ddlAppFormat.DataBind();
            //}
        }
        catch (Exception)
        {
            
        }
        //string tc_setvalue = d2.GetFunction(q1); ddlAppFormat.Items.Clear();
        //if (tc_setvalue.Trim() != "")
        //{
        //    string[] tc_setval = tc_setvalue.Split(',');
        //    foreach (string val in tc_setval)
        //    {
        //        if (val == "1")
        //            ddlAppFormat.Items.Add(new ListItem("CBSE", "0"));
        //        else if (val == "2")
        //            ddlAppFormat.Items.Add(new ListItem("SSLC", "1"));
        //        else if (val == "3")
        //            ddlAppFormat.Items.Add(new ListItem("HSC", "2"));
        //        else if (val == "4")
        //            ddlAppFormat.Items.Add(new ListItem("TRANSFER CERTIFICATE", "3"));
        //        else if (val == "5")
        //            ddlAppFormat.Items.Add(new ListItem("MIGRATION CERTIFICATE", "4"));
        //        else if (val == "6")
        //            ddlAppFormat.Items.Add(new ListItem("TRANSFER CERTIFICATE", "5"));
        //    }
        //}
    }

    protected void ddl_searchtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        loadsearchtype();
    }

    protected void loadsearchtype()
    {
        switch (Convert.ToUInt32(ddl_searchtype.SelectedItem.Value))
        {
            case 0:
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
                break;
            case 1:
                txt_searchappno.Attributes.Add("placeholder", "Reg No");
                break;
            case 2:
                txt_searchappno.Attributes.Add("placeholder", "Admission No");
                break;
            case 3:
                txt_searchappno.Attributes.Add("placeholder", "App No");
                break;
        }
    }

    protected void cb_serialnoSettings_onchange(object sender, EventArgs e)
    {
        if (cb_serialnoSettings.Checked)
        {
            txt_serial_no.Enabled = false;
            //added by Mullai
            if (txt_regno.Text.Trim() != "")
            {
                string regno = Convert.ToString(txt_regno.Text.Trim()).Trim();
                q1 = "  select Serial_no,r.tcserialNo as AutoSerialno from applyn a,Registration r left join Tc_details t on r.App_No=t.App_no where  a.app_no=r.App_No and r.Reg_No='" + regno + "'";
                q1 += " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                q1 += " SELECT certacrno FROM TEmCertSerialSettings where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
                 ds1.Clear();
                ds1 = d2.select_method_wo_parameter(q1, "text");
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    string Serial_no = Convert.ToString(ds1.Tables[0].Rows[0]["Serial_no"]);
                    if (ds1.Tables[1].Rows.Count > 0)
                    {
                        if (Convert.ToString(ds1.Tables[1].Rows[0]["linkvalue"]) == "1")
                        {
                            if (ds1.Tables[2].Rows.Count > 0)
                                Serial_no = Convert.ToString(ds1.Tables[2].Rows[0]["certacrno"]) + Convert.ToString(ds1.Tables[0].Rows[0]["AutoSerialno"]);
                            cb_serialnoSettings.Checked = true;
                            txt_serial_no.Enabled = false;
                        }
                        else
                            txt_serial_no.Enabled = true;
                    }
                    txt_serial_no.Text = Serial_no;
                }
    
            }
            //****

            //q1 = " select linkvalue from New_InsSettings where LinkName='TC_SerialNoSettings' and college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            //q1 += " SELECT CertAcrNo,RunningSerialNo  FROM TEmCertSerialSettings where college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'";
            //q1 += " select tcserialNo from Registration where App_No='" + lbl_app_no1.Text + "'";
            //DataSet settings = d2.select_method_wo_parameter(q1, "text");
            //if (settings.Tables[0].Rows.Count > 0)
            //{
            //    int RunningSerialNo = 0;
            //    if (Convert.ToString(settings.Tables[0].Rows[0]["linkvalue"]) == "1")
            //    {
            //        string Acr = string.Empty;
            //        if (settings.Tables[1].Rows.Count > 0 && settings.Tables != null)
            //        {
            //            Acr = Convert.ToString(settings.Tables[1].Rows[0]["CertAcrNo"]);
            //            int.TryParse(Convert.ToString(settings.Tables[1].Rows[0]["RunningSerialNo"]), out RunningSerialNo);
            //            RunningSerialNo++;
            //            txt_serial_no.Text = Acr + Convert.ToString(RunningSerialNo);
            //        }
            //        if (!string.IsNullOrEmpty(Convert.ToString(settings.Tables[2].Rows[0]["tcserialNo"]).Trim()))
            //            txt_serial_no.Text = Acr + Convert.ToString(settings.Tables[2].Rows[0]["tcserialNo"]).Trim();

            //        cb_serialnoSettings.Checked = true;
            //        txt_serial_no.Enabled = false;
            //    }
            //    else
            //    {
            //        txt_serial_no.Enabled = true;
            //        txt_serial_no.Text = "";
            //    }
            //}
            //else
            //{
            //    txt_serial_no.Enabled = true;
            //    txt_serial_no.Text = "";
            //}
        }
        else
        {
            txt_serial_no.Enabled = true;
            //txt_serial_no.Text = "";
        }
    }

    public string DateToText(DateTime dt)
    {
        string date = string.Empty;
        try
        {
            string[] ordinals =
        { "First","Second","Third","Fourth","Fifth","Sixth","Seventh","Eighth","Ninth","Tenth","Eleventh","Twelfth",
"Thirteenth","Fourteenth","Fifteenth","Sixteenth","Seventeenth","Eighteenth","Nineteenth","Twentieth","Twenty First","Twenty Second","Twenty Third","Twenty Fourth","Twenty Fifth","Twenty Sixth","Twenty Seventh","Twenty Eighth","Twenty Ninth","Thirtieth",           "Thirty First"};
            int day = dt.Day;
            int month = dt.Month;
            int year = dt.Year;
            DateTime dtm = new DateTime(1, month, 1);

            string[] yearval = new string[2];
            yearval[0] = Convert.ToString(year).Substring(0, 2);
            yearval[1] = Convert.ToString(year).Substring(2, 2);
            string yearwords = string.Empty;
            if (Convert.ToString(yearval[1]) != "00")
            {
                int yearvalue = 0;
                int.TryParse(Convert.ToString(yearval[0]), out yearvalue);
                int yearvalue1 = 0;
                int.TryParse(Convert.ToString(yearval[1]), out yearvalue1);
                if (year < 2000)
                    yearwords = NumberToText(yearvalue) + " " + NumberToText(yearvalue1);
                else
                    yearwords = NumberToText(year);
                       // ReuasableMethods. ConvertNumbertoWords(year);
            }
            else
            {
                yearwords = NumberToText(year);
            }
            date = ordinals[day - 1] + " - " + dtm.ToString("MMMM") + " - " + yearwords;
        }
        catch { }
        return date;
    }

    public static string NumberToText(int number)
    {
        if (number == 0) return "Zero";
        bool isUK = false;
        string and = isUK ? "and " : ""; // deals with UK or US numbering
        if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
        "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
        "Six Hundred " + and + "Forty Eight";
        int[] num = new int[4];
        int first = 0;
        int u, h, t;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        if (number < 0)
        {
            sb.Append("Minus ");
            number = -number;
        }
        string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
        string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
        string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
        string[] words3 = { "Thousand ", "Million ", "Billion " };
        num[0] = number % 1000;           // units
        num[1] = number / 1000;
        num[2] = number / 1000000;
        num[1] = num[1] - 1000 * num[2];  // thousands
        num[3] = number / 1000000000;     // billions
        num[2] = num[2] - 1000 * num[3];  // millions
        for (int i = 3; i > 0; i--)
        {
            if (num[i] != 0)
            {
                first = i;
                break;
            }
        }
        for (int i = first; i >= 0; i--)
        {
            if (num[i] == 0) continue;
            u = num[i] % 10;              // ones
            t = num[i] / 10;
            h = num[i] / 100;             // hundreds
            t = t - 10 * h;               // tens
            if (h > 0) sb.Append(words0[h] + "Hundred ");
            if (u > 0 || t > 0)
            {
                if (h > 0 || i < first) sb.Append(and);
                if (t == 0)
                    sb.Append(words0[u]);
                else if (t == 1)
                    sb.Append(words1[u]);
                else
                    sb.Append(words2[t - 2] + words0[u]);
            }
            if (i != 0) sb.Append(words3[i - 1]);
        }
        return sb.ToString().TrimEnd();
    }

}