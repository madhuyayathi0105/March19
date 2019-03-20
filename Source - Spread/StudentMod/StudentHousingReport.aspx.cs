using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Text;
using System.Configuration;
public partial class StudentMod_StudentHousingReport : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DataSet ds2 = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string group_user = string.Empty;
    Hashtable hat = new Hashtable();
    string branch = string.Empty;
    string college_code = string.Empty;
    string q1 = string.Empty;
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
            group_user = Session["group_code"].ToString();
            setLabelText();
            lbl_clgname.Text = lbl_clgT.Text;
            lbl_degree.Text = lbl_degreeT.Text;
            lbl_branch.Text = lbl_branchT.Text;
            if (!IsPostBack)
            {
                BindCollege();
                bindbatch();
                degree();
                bindbranch();
                bindhousing();
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
                ddl_searchtype.Items.Add(new ListItem("Roll No", "0"));
                ddl_searchtype.Items.Add(new ListItem("Reg No", "1"));
                ddl_searchtype.Items.Add(new ListItem("Admission No", "2"));
                ddl_searchtype.Items.Add(new ListItem("App No", "3"));
                txt_searchappno.Attributes.Add("placeholder", "Roll No");
            }
            lblvalidation1.Text = "";
        }
        catch (Exception ex) { }
    }
    public void ddlcollege_SelectedIndexChanged(object sender, EventArgs e)
    {
        degree();
        bindbatch();
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
    public void cb_degree_checkedchange(object sender, EventArgs e)
    {
        try
        {
           
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
                        
                    }
                }
                bindbranch();

            }
            else
            {
                for (int i = 0; i < cbl_degree.Items.Count; i++)
                {
                    cbl_degree.Items[i].Selected = false;
                    txt_degree.Text = "--Select--";
                    txt_branch.Text = "--Select--";
                    cbl_branch.ClearSelection();
                    cbl_branch.Items.Clear();
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
           
            string build = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    seatcount = seatcount + 1;
                    txt_branch.Text = "--Select--";
                    build = cbl_degree.Items[i].Value.ToString();
                   
                }
            }
            bindbranch();
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
                txt_batch.Text = lbl_batch.Text + "(" + (cbl_batch.Items.Count) + ")";
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
        catch
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
            if (commcount == cbl_batch.Items.Count)
            {
                txt_batch.Text = lbl_batch.Text + "(" + commcount.ToString() + ")";
                cb_batch.Checked = true;
            }
            else
            {
                txt_batch.Text = lbl_batch.Text + "(" + commcount.ToString() + ")";
            }

        }
        catch
        {
        }
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
        fields.Add(0);
        fields.Add(2);
        fields.Add(3);
        fields.Add(4);
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    public void bindbatch()
    {
        try
        {

            hat.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    //for (int i = 0; i < cbl_degree.Items.Count; i++)
                    //{
                    cbl_batch.Items[0].Selected = true;
                    //}
                    txt_batch.Text = lbl_batch.Text + " (" + 1 + ")";
                    cb_batch.Checked = true;
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
              
                bindbranch();
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
    public void bindbranch()
    {
        try
        {
            string rights = "";
            txt_branch.Text = "--Select--";
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
            string branch = rs.GetSelectedItemsValueAsString(cbl_degree);
            if (branch != "")
            {
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code and degree.college_code='" + ddlcollege.SelectedItem.Value + "' " + rights + " ";
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
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_go_OnClick(object sender, EventArgs e)
    {
        try
        {
            string deptcode = rs.GetSelectedItemsValueAsString(cbl_degree);
            string degreecode = rs.GetSelectedItemsValueAsString(cbl_branch);
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);

            string housepk = rs.GetSelectedItemsValueAsString(cb1_housing);
            if (deptcode.Trim() != "" && degreecode.Trim() != "" && batch.Trim()!="" && !string.IsNullOrEmpty(housepk.Trim()))
            {
                rs.Fpreadheaderbindmethod("S.No-50/House Name-100/Student Name-200/Roll No-150/Reg No-150/Admission No-150/Gender-90/Section-70/" + lbl_degreeT.Text + "-100/" + lbl_branchT.Text + "-200/ Semester-100", FpSpread1, "FALSE");
                q1 = "select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,hd.housename from Registration r, degree d,Department dt,Course C,applyn a inner join HousingDetails hd on a.studhouse=hd.housePK where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1'  and hd.housePK in('" + housepk + "')";
                string type = string.Empty;
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
                else
                    q1 += " and r.degree_code in('" + degreecode + "')and r.Batch_Year in('" + batch + "') and c.Course_Id in('" + deptcode + "') and r.college_code='" + Convert.ToString(ddlcollege.SelectedItem.Value) + "'  ";

                ds.Clear();
                ds = d2.select_method_wo_parameter(q1, "text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        FpSpread1.Sheets[0].Rows.Count++;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread1.Sheets[0].Rows.Count);
                        FpSpread1.Sheets[0].Columns[3].Visible = false;
                        FpSpread1.Sheets[0].Columns[4].Visible = false;
                        FpSpread1.Sheets[0].Columns[5].Visible = false;
                        if (Convert.ToString(Session["Rollflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[3].Visible = true;
                        if (Convert.ToString(Session["Regflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[4].Visible = true;
                        if (Convert.ToString(Session["Admissionflag"]) == "1")
                            FpSpread1.Sheets[0].Columns[5].Visible = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["housename"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["stud_name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["roll_no"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Reg_No"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["Roll_Admit"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["sex"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["sections"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["Course_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dr["Dept_Name"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dr["Current_Semester"]);
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 3].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 4].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 5].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 6].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 7].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 8].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 9].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Locked = true;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
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
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        FpSpread1.Sheets[0].Cells[FpSpread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                    }
                    FpSpread1.Sheets[0].PageSize = FpSpread1.Sheets[0].RowCount;
                    FpSpread1.Visible = true;
                    FpSpread1.SaveChanges();
                    lbl_error.Visible = false;
                    rptprint.Visible = true;
                }
                else
                {
                    rptprint.Visible = false;
                    FpSpread1.Visible = false;
                    lbl_error.Visible = true;
                    lbl_error.Text = "No Record Founds";
                }
            }
            else
            {
                rptprint.Visible = false;
                FpSpread1.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "Please Select All The Fields";
            }
        }
        catch (Exception f)
        {
            rptprint.Visible = false;
            FpSpread1.Visible = false;
            lbl_error.Visible = true;
            lbl_error.Text = f.ToString();

        }
    }
    public void cb_housing_checkedchange(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            Txt_housing.Text = "--Select--";
            if (cb_housing.Checked == true)
            {
                cout++;
                for (int i = 0; i < cb1_housing.Items.Count; i++)
                {
                    cb1_housing.Items[i].Selected = true;
                }
                Txt_housing.Text = Lbl_housing.Text + "(" + (cb1_housing.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cb1_housing.Items.Count; i++)
                {
                    cb1_housing.Items[i].Selected = false;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_housing_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_housing.Checked = false;
            int commcount = 0;
            Txt_housing.Text = "--Select--";
            for (int i = 0; i < cb1_housing.Items.Count; i++)
            {
                if (cb1_housing.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_housing.Checked = false;
                }
                Txt_housing.Text = Lbl_housing.Text + "(" + commcount.ToString() + ")";
            }


        }
        catch (Exception ex)
        {
        }
    }
    public void bindhousing()
    {

        try
        {
            cb1_housing.Items.Clear();
            Txt_housing.Text = "--Select--";
            string query = "select HousePK,HouseName from HousingDetails where CollegeCode in('" + ddlcollege.SelectedItem.Value + "') order by HouseName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cb1_housing.DataSource = ds;
                cb1_housing.DataTextField = "HouseName";
                cb1_housing.DataValueField = "HousePK";
                cb1_housing.DataBind();
                if (cb1_housing.Items.Count > 0)
                {

                    cb1_housing.Items[0].Selected = true;
                    Txt_housing.Text = Lbl_housing.Text + "(1)";
                }
            }
        }
        catch (Exception e)
        {
        }
    }
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        lblvalidation1.Visible = false;
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(FpSpread1, reportname);
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
            string degreedetails = "Housing Report";
            string pagename = "StudentHousingReport.aspx";
            Printcontrol.loadspreaddetails(FpSpread1, pagename, degreedetails);
            Printcontrol.Visible = true;
        }
        catch
        { }
    }
    protected string returnwithsinglecodevalue(CheckBoxList cb)
    {
        string empty = "";
        for (int i = 0; i < cb.Items.Count; i++)
        {
            if (cb.Items[i].Selected == true)
            {
                if (empty == "")
                {
                    empty = Convert.ToString(cb.Items[i].Value);
                }
                else
                {
                    empty = empty + "','" + Convert.ToString(cb.Items[i].Value);
                }
            }
        }
        return empty;
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
}





