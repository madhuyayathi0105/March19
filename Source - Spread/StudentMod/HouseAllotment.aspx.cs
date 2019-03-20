using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Configuration;


public partial class HouseAllotment : System.Web.UI.Page
{
    DAccess2 d2 = new DAccess2();
        DataSet ds = new DataSet();
    // string collegecode = "13";
    string collegecode = string.Empty;
    static string collegecodestat = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static int chosedmode = 0;
    bool check = false;
    static int personmode = 0;

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
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            if (!IsPostBack)
            {
                bindclg();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedValue);
                    collegecodestat = Convert.ToString(ddlcollege.SelectedValue);
                }
                bindhousing();
                LoadFromSettings();
                if (ddlcollege.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlcollege.SelectedValue);
                    collegecodestat = Convert.ToString(ddlcollege.SelectedValue);
                }
            }
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = Convert.ToString(ddlcollege.SelectedValue);
                collegecodestat = Convert.ToString(ddlcollege.SelectedValue);
            }
        }
        catch (Exception ex) { }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddlcollege.Items.Clear();
            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and                                                  cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlcollege.DataSource = ds;
                ddlcollege.DataTextField = "collname";
                ddlcollege.DataValueField = "college_code";
                ddlcollege.DataBind();
            }
        }
        catch (Exception ex) { }
    }

    public void bindhousing()
    {

        //try
        //{
        //    ddlhousename.Items.Clear();

        //    string query = "select HousePK,HouseName from HousingDetails where CollegeCode=13 order by HouseName";
        //    ds.Clear();
        //    ds = d2.select_method_wo_parameter(query, "Text");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        ddlhousename.DataSource = ds;
        //        ddlhousename.DataTextField = "HouseName";
        //        ddlhousename.DataValueField = "HousePK";
        //        ddlhousename.DataBind();
        //        ddlhousename.Items.Insert(0, "All");

        //    }
        //}
        //catch (Exception e) { }
        try
        {
            cbl_house.Items.Clear();
            cb_house.Checked = true;
            ds.Clear();
            if (!string.IsNullOrEmpty(collegecode.Trim()))
            {
                string query = "select HouseName,HousePK from HousingDetails where collegecode='" + collegecode + "'";
                ds = d2.select_method_wo_parameter(query, "TEXT");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_house.DataSource = ds;
                    cbl_house.DataTextField = "HouseName";
                    cbl_house.DataValueField = "HousePK";
                    cbl_house.DataBind();
                    CollCheckBoxChangedEvent(cbl_house, cb_house, txthouse, lbl_House1.Text);
                }
                else
                {

                }
            }
        }
        catch (Exception e) { }


    }

    private void CollCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    public void bindhousing1()
    {

        try
        {
            ddlhousename1.Items.Clear();

            string query = "select HousePK,HouseName from HousingDetails where CollegeCode=13 order by HouseName";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlhousename1.DataSource = ds;
                ddlhousename1.DataTextField = "HouseName";
                ddlhousename1.DataValueField = "HousePK";
                ddlhousename1.DataBind();

            }
        }
        catch (Exception e) { }
    }

    public void LoadFromSettings()
    {
        try
        {

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }


            int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + collegecode + ")").Trim());


            ListItem lst1 = new ListItem("Roll No", "0");
            ListItem lst2 = new ListItem("Reg No", "1");
            ListItem lst3 = new ListItem("Admission No", "2");
            ListItem lst4 = new ListItem("App No", "3");
            ListItem lst5 = new ListItem("Smartcard No", "4");

            //Roll Number or Reg Number or Admission No or Application Number
            //rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Roll No
                rbl_rollno.Items.Add(lst1);
            }


            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //RegNo
                rbl_rollno.Items.Add(lst2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ")";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                //Admission No - Roll Admit
                rbl_rollno.Items.Add(lst3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //App Form Number - Application Number
                rbl_rollno.Items.Add(lst4);

            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptSmartNo' and user_code ='" + usercode + "' --and college_code in(" + collegecode + ") ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                //Smartcard No - smart_serial_no
                rbl_rollno.Items.Add(lst5);
            }

            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(lst1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                case1:
                    txt_rollno.Attributes.Add("placeholder", "Roll No");
                    //lbl_rollno3.Text = "Roll No";
                    chosedmode = 0;
                    break;
                case 1:
                case2:
                    txt_rollno.Attributes.Add("placeholder", "Reg No");
                    // lbl_rollno3.Text = "Reg No";
                    chosedmode = 1;
                    break;
                case 2:
                case3:
                    txt_rollno.Attributes.Add("placeholder", "Admin No");
                    //  lbl_rollno3.Text = "Admin No";
                    chosedmode = 2;
                    break;
                case 3:
                case4:
                    txt_rollno.Attributes.Add("placeholder", "App No");
                    // lbl_rollno3.Text = "App No";
                    chosedmode = 3;
                    break;
                case 4:
                    txt_rollno.Attributes.Add("placeholder", "Smartcard No");
                    //   lbl_rollno3.Text = "SmartCard No";
                    chosedmode = 4;
                    switch (smartDisp)
                    {
                        case 0:
                            goto case1;
                        case 1:
                            goto case2;
                        case 2:
                            goto case3;
                        case 3:
                            goto case4;
                    }
                    break;
            }

        }

        catch (Exception ex) { }
    }


    protected void btn_addnew_Click(object sender, EventArgs e)
    {
        bindhousing1();
        txt_rollno.Text = "";
        txt_name.Text = "";
        addnew_popup.Visible = true;
        lbl_house.Text = "House Entry";
        btn_delete.Visible = false;
        btnsave.Text = "Save";
        btn_roll.Visible = true;
        rbl_rollno.Enabled = true;
        txt_rollno.Enabled = true;
       
        btn_exit.Visible = true;

    }

    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        txt_rollno.Text = "";
        txt_name.Text = "";
        int smartDisp = Convert.ToInt32(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='DisplayNumberForSmartCd' and user_code ='" + usercode + "' --and college_code in (" + collegecode + ")").Trim());
        switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
        {
            case 0:
            case1:
                txt_rollno.Attributes.Add("placeholder", "Roll No");
                //  lbl_rollno3.Text = "Roll No";
                chosedmode = 0;
                break;
            case 1:
            case2:
                txt_rollno.Attributes.Add("placeholder", "Reg No");
                // lbl_rollno3.Text = "Reg No";
                chosedmode = 1;
                break;
            case 2:
            case3:
                txt_rollno.Attributes.Add("placeholder", "Admin No");
                //  lbl_rollno3.Text = "Admin No";
                chosedmode = 2;
                break;
            case 3:
            case4:
                txt_rollno.Attributes.Add("placeholder", "App No");
                //  lbl_rollno3.Text = "App No";
                chosedmode = 3;
                break;
            case 4:
                txt_rollno.Attributes.Add("placeholder", "Smartcard No");
                //  lbl_rollno3.Text = "SmartCard No";
                chosedmode = 4;
                switch (smartDisp)
                {
                    case 0:
                        goto case1;
                    case 1:
                        goto case2;
                    case 2:
                        goto case3;
                    case 3:
                        goto case4;
                }
                break;
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
            if (personmode == 0)
            {
                //student query
                if (chosedmode == 0)
                {
                    query = "select top 100 Roll_No from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' and college_code=" + collegecodestat + " and  app_no not in (select a.app_no from applyn a where a.app_no=r.app_no and isnull(studhouse,'')<>'') order by Roll_No asc";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' and college_code=" + collegecodestat + " and  app_no not in (select a.app_no from applyn a where a.app_no=r.app_no and isnull(studhouse,'')<>'')  order by Reg_No asc";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + " and  app_no not in (select a.app_no from applyn a where a.app_no=r.app_no and isnull(studhouse,'')<>'')  order by Roll_admit asc";
                }
                else if (chosedmode == 3)
                {
                    // query = "select  top 100 App_no from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' and college_code=" + collegecodestat + "  order by App_no asc";
                    query = "  select  top 100 app_formno from applyn where  app_formno like '" + prefixText + "%' and college_code=" + collegecodestat + " and isnull(studhouse,'')=''  order by app_formno asc";
                }
                else if (chosedmode == 4)
                {
                    query = "select  top 100 smart_serial_no from Registration r where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and smart_serial_no like '" + prefixText + "%' and college_code=" + collegecodestat + " and  app_no not in (select a.app_no from applyn a where a.app_no=r.app_no and isnull(studhouse,'')<>'')  order by smart_serial_no asc";
                }
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void txt_rollno_Changed(object sender, EventArgs e)
    {
        textRoll();
    }


    private void textRoll()
    {
        string appNo = "-1";
        try
        {
            string name = "";
            string degree = "";
            string stType = "";
            string fname = "";
            string query = "";
            string roll_no = Convert.ToString(txt_rollno.Text.Trim());
            string cursemvalue = "1";
            if (roll_no != "")
            {
                //smartCno = txt_rollno.Text.Trim();
                //if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                //{
                query = "select r.Roll_No,r.Roll_Admit,r.app_no,Stud_Name,d.Degree_Code ,c.Course_Name +'-'+dt.dept_acronym as Degree,(select TextVal from TextValTable where TextCode=(select seattype from Applyn where app_no=r.app_no) and TextCriteria='seat' ) as StType,(select parent_name from applyn where app_no=r.app_no) as fname, ISNULL( type,'') as type,R.Current_Semester  from Registration r,Degree d,Department dt,Course c where r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and r.college_code='" + collegecode + "' and  app_no not in (select a.app_no from applyn a where a.app_no=r.app_no and isnull(studhouse,'')<>'') ";
                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    query += " and r.Roll_No like '" + roll_no + "'";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    query += " and r.Reg_No like '" + roll_no + "'";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    query += " and r.Roll_Admit like '" + roll_no + "'";
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
                {
                    //app_no
                    query += " and r.app_no like '" + roll_no + "'";
                }
                //else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                //{
                //    //Smart card No
                //    query += " and r.smart_serial_no like '" + txt_Smartno.Text.Trim() + "'";
                //}
                //else
                //{
                //    query = "";
                //}
                //}
                ds.Clear();
                ds = d2.select_method_wo_parameter(query, "Text");


                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            name = Convert.ToString(ds.Tables[0].Rows[i]["Stud_Name"]);
                            degree = Convert.ToString(ds.Tables[0].Rows[i]["Degree"]);
                            stType = Convert.ToString(ds.Tables[0].Rows[i]["stType"]);
                            fname = Convert.ToString(ds.Tables[0].Rows[i]["fname"]);
                            //  lbltype.Text = Convert.ToString(ds.Tables[0].Rows[i]["type"]);
                            appNo = Convert.ToString(ds.Tables[0].Rows[i]["app_no"]);
                            if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) != 3)
                            {
                                cursemvalue = Convert.ToString(ds.Tables[0].Rows[i]["Current_Semester"]);
                            }
                        }
                    }
                }
                txt_name.Enabled = false;
                txt_name.Text = name;

            }
            else
            {
                txt_name.Text = "";
            }
        }
        catch (Exception ex) { }

    }


    protected void btn_roll_Click(object sender, EventArgs e)
    {
        popwindow.Visible = true;
        bindType();
        bindbatch1();
        binddegree2();
        bindbranch1();
        bindsec2();
        //txt_rollno3.Text = "";
        btn_studOK.Visible = false;

        btn_exitstud.Visible = false;
        Fpspread1.Visible = false;
        lbl_errormsg.Visible = false;
    }

    public void bindType()
    {
        try
        {
            if (ddlcollege.Items.Count > 0)
            {
                collegecode = ddlcollege.SelectedItem.Value.ToString();
            }
            ddl_strm.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_strm.DataSource = ds;
                ddl_strm.DataTextField = "type";
                ddl_strm.DataValueField = "type";
                ddl_strm.DataBind();
            }
            if (ddl_strm.Items.Count > 0)
            {
                if (streamEnabled() == 1)
                    ddl_strm.Enabled = true;
                else
                    ddl_strm.Enabled = false;
            }
            else
                ddl_strm.Enabled = false;
        }
        catch
        { }
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
        catch (Exception ex) { }
    }
    public void binddegree2()
    {
        try
        {
            ds.Clear();
            cbl_degree2.Items.Clear();
            string stream = "";
            stream = ddl_strm.Items.Count > 0 ? ddl_strm.SelectedValue : "";
            txt_degree2.Text = "--Select--";

            string useCOdeSet = "select LinkValue from New_InsSettings where LinkName='MultipleCollegeUserRights' and user_code ='" + usercode + "' and college_code ='" + collegecode + "' ";
            string colleges = Convert.ToString(d2.GetFunction(useCOdeSet)).Trim();
            if (colleges == "" || colleges == "0")
            {
                colleges = collegecode;
            }
            //string query = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages  where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode1 + ") and deptprivilages.Degree_code=degree.Degree_code and   user_code=" + usercode + " ";
            string query = "select distinct degree.course_id,course.course_name from degree,course where course.course_id=degree.course_id and course.college_code = degree.college_code  and degree.college_code in (" + collegecode + ") ";
            if (ddl_strm.Enabled)//if (txt_strm.Enabled)
            {
                query += " and course.type in ('" + stream + "')";
            }
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
                    cb_degree2.Checked = true;
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
        catch (Exception ex) { }
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
                //commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') and deptprivilages.Degree_code=degree.Degree_code ";
                commname = "select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + branch + "') ";
            }
            else
            {
                //commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course,deptprivilages where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and deptprivilages.Degree_code=degree.Degree_code";
                commname = " select distinct degree.degree_code,department.dept_name,degree.Acronym  from degree,department,course where course.course_id=degree.course_id  and department.dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code ";
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
                        cb_branch1.Checked = true;
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
        catch (Exception ex) { }
    }
    public void bindsec2()
    {
        try
        {
            cbl_sec2.Items.Clear();
            txt_sec2.Text = "--Select--";
            ListItem item = new ListItem("Empty", " ");
            if (ddl_batch1.Items.Count > 0)
            {
                string strbatch = Convert.ToString(ddl_batch1.SelectedItem.Value);
                string branch = "";
                for (int i = 0; i < cbl_branch1.Items.Count; i++)
                {
                    if (cbl_branch1.Items[i].Selected == true)
                    {
                        if (branch == "")
                        {
                            branch = "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                        else
                        {
                            branch = branch + "" + "," + "" + "" + cbl_branch1.Items[i].Value.ToString() + "";
                        }
                    }
                }
                if (branch != "")
                {
                    DataSet dsSec = d2.BindSectionDetail(strbatch, branch);
                    if (dsSec.Tables.Count > 0)
                    {
                        if (dsSec.Tables[0].Rows.Count > 0)
                        {
                            cbl_sec2.DataSource = dsSec;
                            cbl_sec2.DataTextField = "sections";
                            cbl_sec2.DataValueField = "sections";
                            cbl_sec2.DataBind();


                        }
                    }
                    cbl_sec2.Items.Insert(0, item);
                    for (int i = 0; i < cbl_sec2.Items.Count; i++)
                    {
                        cbl_sec2.Items[i].Selected = true;
                    }
                    cb_sec2.Checked = true;
                    txt_sec2.Text = "Section(" + cbl_sec2.Items.Count + ")";

                }
            }


        }
        catch (Exception ex) { }
    }


    protected void cb_degree2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cbl_degree2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_degree2, cb_degree2, txt_degree2, "Degree");
        bindbranch1();
        bindsec2();
    }

    protected void cb_branch1_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cbl_branch1_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_branch1, cb_branch1, txt_branch2, "Branch");
        bindsec2();
    }

    protected void cb_sec2_ChekedChange(object sender, EventArgs e)
    {
        CallCheckBoxChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }

    protected void cbl_sec2_SelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckBoxListChangedEvent(cbl_sec2, cb_sec2, txt_sec2, "Section");
    }


    protected void btn_popup_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread1.SaveChanges();
            string feecat = string.Empty;
            string ddlstream = Convert.ToString(ddl_strm.SelectedItem.Value);
            string batch = Convert.ToString(ddl_batch1.SelectedItem.Value);
            string degree = Convert.ToString(getCblSelectedValue(cbl_degree2));
            string branch = Convert.ToString(getCblSelectedValue(cbl_branch1));
            string sec = Convert.ToString(getCblSelectedValue(cbl_sec2));
            string selqry = " select r.app_no,r.Roll_No,r.Reg_No,r.roll_admit,r.Stud_Name,a.app_formno,r.batch_year,r.Current_Semester,r.sections,d.Degree_Code ,c.Course_Name +'-'+dt.Dept_Name as Degree,smart_serial_no from applyn a,Registration r,Degree d,Department dt,Course c where a.app_no =r.App_No and  r.degree_code =d.Degree_Code and d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id and CC=0 and DelFlag=0 and Exam_Flag<>'debar'  and d.Degree_Code in ('" + branch + "') and r.Batch_Year='" + batch + "' and isnull(studhouse,'')='' ";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpspread1.Sheets[0].RowCount = 0;
                Fpspread1.Sheets[0].ColumnCount = 0;
                Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread1.CommandBar.Visible = false;
                Fpspread1.Sheets[0].ColumnCount = 9;

                Fpspread1.Sheets[0].RowHeader.Visible = false;
                Fpspread1.Sheets[0].AutoPostBack = false;

                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].Columns[0].Locked = true;
                Fpspread1.Columns[0].Width = 50;

                //FarPoint.Web.Spread.CheckBoxCellType chkall = new FarPoint.Web.Spread.CheckBoxCellType();
                //chkall.AutoPostBack = true;

                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Select";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                //Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                //Fpspread1.Columns[1].Width = 80;
                //Fpspread1.Sheets[0].Columns[1].Locked = false;

                //Fpspread1.Sheets[0].Cells[0, 1].CellType = chkall;
                //Fpspread1.Sheets[0].Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Admission No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";

                Fpspread1.Sheets[0].Columns[2].Locked = true;
                Fpspread1.Columns[2].Width = 130;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Roll No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[3].Locked = true;
                Fpspread1.Columns[3].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Reg No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[4].Locked = true;
                Fpspread1.Columns[4].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Smartcard No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[5].Locked = true;
                Fpspread1.Columns[5].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "App No";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[6].Locked = true;
                Fpspread1.Columns[6].Width = 100;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Name";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[7].Locked = true;
                Fpspread1.Columns[7].Width = 200;

                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Degree";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpspread1.Sheets[0].Columns[8].Locked = true;
                Fpspread1.Sheets[0].Columns[8].VerticalAlign = VerticalAlign.Middle;
                Fpspread1.Sheets[0].SetColumnMerge(8, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread1.Columns[8].Width = 270;



                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRollAd = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAppno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtSmartno = new FarPoint.Web.Spread.TextCellType();

                if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                {
                    //roll no
                    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[6].Visible = false;
                    Fpspread1.Sheets[0].Columns[1].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 1)
                {
                    //reg no
                    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[6].Visible = false;
                    Fpspread1.Sheets[0].Columns[1].Visible = false;

                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                {
                    //Admin no
                    Fpspread1.Sheets[0].Columns[2].Visible = true;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[6].Visible = false;
                    Fpspread1.Sheets[0].Columns[1].Visible = false;
                }
                else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 4)
                {
                    //Smartcard no
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[6].Visible = false;
                    Fpspread1.Sheets[0].Columns[1].Visible = false;
                    //if (smartDisp == 0)
                    //    Fpspread1.Sheets[0].Columns[3].Visible = true;
                    //else if (smartDisp == 1)
                    //    Fpspread1.Sheets[0].Columns[4].Visible = true;
                    //else if (smartDisp == 2 || smartDisp == 3)
                    //    Fpspread1.Sheets[0].Columns[2].Visible = true;
                }
                else
                {
                    //App no
                    Fpspread1.Sheets[0].Columns[6].Visible = true;
                    Fpspread1.Sheets[0].Columns[2].Visible = false;
                    Fpspread1.Sheets[0].Columns[4].Visible = false;
                    Fpspread1.Sheets[0].Columns[3].Visible = false;
                    Fpspread1.Sheets[0].Columns[5].Visible = false;
                    Fpspread1.Sheets[0].Columns[1].Visible = false;
                }

                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {

                    Fpspread1.Sheets[0].RowCount++;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    //
                    //FarPoint.Web.Spread.CheckBoxCellType check = new FarPoint.Web.Spread.CheckBoxCellType();
                    //check.AutoPostBack = false;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].CellType = check;
                    //Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;

                    //
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].CellType = txtRollAd;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_Admit"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].CellType = txtRollno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].CellType = txtRegno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].CellType = txtSmartno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["smart_serial_no"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    //bind app_no
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].CellType = txtAppno;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["Stud_Name"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Tag = Convert.ToString(ds.Tables[0].Rows[row]["Degree_Code"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["Degree"]);
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                }
                Fpspread1.Visible = true;
                Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                Fpspread1.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                Fpspread1.Sheets[0].FrozenRowCount = 1;

                Fpspread1.SaveChanges();

                btn_studOK.Visible = true;
                btn_exitstud.Visible = true;
            }
            else
            {
                Fpspread1.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                btn_studOK.Visible = false;
                btn_exitstud.Visible = false;
            }

        }
        catch (Exception ex)
        { //d2.sendErrorMail(ex, collegecode, "ChallanReceipt");
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
        catch (Exception ex) { }
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


    protected void fpspread2_rowselect(object sender, EventArgs e)
    {
        try
        {
            if (check == true)
            {
                bindhousing1();
                int actRow = 0;
                int actCol = 0;
                string activerow = Fpspread2.ActiveSheetView.ActiveRow.ToString();
                string activecol = Fpspread2.ActiveSheetView.ActiveColumn.ToString();
                int.TryParse(activerow, out actRow);
                int.TryParse(activecol, out actCol);
                if (actRow != -1 && actCol != -1)
                {
                    string appNo = Convert.ToString(Fpspread2.Sheets[0].Cells[actRow, 0].Tag);
                    if (!string.IsNullOrEmpty(appNo))
                    {
                        string selQ = " select r.stud_name,roll_no,studhouse from registration r,applyn a where r.app_no=a.app_no and  r.app_no='" + appNo + "'";

                        DataSet dsVal = d2.select_method_wo_parameter(selQ, "Text");
                        if (dsVal.Tables.Count > 0 && dsVal.Tables[0].Rows.Count > 0)
                        {
                            string studName = Convert.ToString(dsVal.Tables[0].Rows[0]["stud_name"]);
                            string rollNo = Convert.ToString(dsVal.Tables[0].Rows[0]["roll_no"]);
                            string studHouse = Convert.ToString(dsVal.Tables[0].Rows[0]["studhouse"]);
                            bindhousing();
                            txt_rollno.Text = rollNo;
                            txt_name.Text = studName;
                            ddlhousename1.SelectedIndex = ddlhousename1.Items.IndexOf(ddlhousename1.Items.FindByValue(studHouse));
                            addnew_popup.Visible = true;
                            lbl_house.Text = "Update House";
                            btn_delete.Visible = true;
                            btnsave.Text = "Update";
                            btn_roll.Visible = false;
                            rbl_rollno.Enabled = false;
                            txt_rollno.Enabled = false;
                            txt_name.Enabled = false;
                            btn_exit.Visible = true;

                        }

                    }
                }


            }
        }
        catch (Exception ex) { }
    }


    private double streamEnabled()
    {
        double strValue = 0;
        double.TryParse(Convert.ToString(d2.GetFunction("select LinkValue from New_InsSettings where LinkName='JournalEnableStreamShift' and user_code ='" + usercode + "' and college_code ='" + ddlcollege.SelectedValue + "'")), out strValue);
        return strValue;
    }

    private void CallCheckBoxChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            tb.Text = dispString;
            if (cb.Checked)
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = true;
                }
                tb.Text = dispString + "(" + cbl.Items.Count + ")";
            }
            else
            {
                for (int i = 0; i < cbl.Items.Count; i++)
                {
                    cbl.Items[i].Selected = false;
                }
            }
        }
        catch { }
    }

    private void CallCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    protected void cb_house_CheckedChanged(object sender, EventArgs e)
    {
        CollCheckBoxChangedEvent(cbl_house, cb_house, txthouse, lbl_House1.Text);
    }

    protected void cbl_house_SelectedIndexChanged(object sender, EventArgs e)
    {
        CollCheckBoxListChangedEvent(cbl_house, cb_house, txthouse, lbl_House1.Text);
    }

    private void CollCheckBoxListChangedEvent(CheckBoxList cbl, CheckBox cb, TextBox tb, string dispString)
    {
        try
        {
            cb.Checked = false;
            tb.Text = dispString;
            int count = 0;
            for (int i = 0; i < cbl.Items.Count; i++)
            {
                if (cbl.Items[i].Selected == true)
                {
                    count++;
                }
            }
            tb.Text = dispString + "(" + count + ")";
            if (count == cbl.Items.Count)
            {
                cb.Checked = true;
            }
        }
        catch { }
    }

    private string getCblSelectedValue(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedvalue = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedvalue.Length == 0)
                    {
                        selectedvalue.Append(Convert.ToString(cblSelected.Items[sel].Value));
                    }
                    else
                    {
                        selectedvalue.Append("','" + Convert.ToString(cblSelected.Items[sel].Value));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedvalue.ToString();
    }

    protected void btn_studOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Fpspread1.Sheets[0].RowCount > 0)
            {
                Fpspread1.SaveChanges();
                string rollno = "";
                string app_no = "";
                string rolladmit = "";
                string degreename1 = "";
                string name1 = "";
                string degreecode1 = "";
                string regno1 = "";
                string smartno = string.Empty;

                string actrow = Fpspread1.Sheets[0].ActiveRow.ToString();
                string actcol = Fpspread1.Sheets[0].ActiveColumn.ToString();
                if (actrow != "-1")
                {
                    rollno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Text);
                    app_no = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 6].Text);
                    rolladmit = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Text);
                    degreename1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 8].Text);
                    degreecode1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 8].Tag);
                    name1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 7].Text);
                    regno1 = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Text);
                    smartno = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Text);

                    if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        //roll no
                        rollno = rollno;
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 3)
                    {
                        //app no
                        rollno = app_no;
                    }
                    else if (Convert.ToUInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        //Admin no
                        // rollno = rolladmit;
                    }

                }
                Fpspread1.Sheets[0].ActiveRow = -1;
                Fpspread1.Sheets[0].ActiveColumn = -1;
                Fpspread1.SaveChanges();
                txt_rollno.Text = Convert.ToString(rollno);
                txt_rollno_Changed(sender, e);
                // Session["degreecodenew"] = Convert.ToString(degreecode1);
                popwindow.Visible = false;
            }
        }
        catch (Exception ex) { }
    }

    protected void btn_exitstud_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        popwindow.Visible = false;


    }
    protected void imagebtnaddnewpopclose_Click(object sender, EventArgs e)
    {
        fps_print.Visible = false;
        lbl_error.Visible = false;
        addnew_popup.Visible = false;

    }

    protected void btn_save_Click(object sender, EventArgs e)
    {
        try
        {
            string rollNo = Convert.ToString(txt_rollno.Text);

            if (!string.IsNullOrEmpty(rollNo))
            {
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                string appNo = getAppNo(rollNo, collegecode);
                if (appNo != "0")
                {


                    string sqlcmd = "update applyn set studhouse=" + ddlhousename1.SelectedItem.Value + " where app_no=" + appNo;
                    d2.update_method_wo_parameter(sqlcmd, "text");
                    if (btnsave.Text == "Save")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Saved Successfully')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Updated Successfully')", true);
                        addnew_popup.Visible = false;
                        btn_go_Click(sender, e);
                    }

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Invalid " + rbl_rollno.SelectedItem.Text + "!')", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Please Enter Student " + rbl_rollno.SelectedItem.Text + "!')", true);
            }

        }
        catch { }
    }

    protected void btn_delete_Click(object sender, EventArgs e)
    {
        try
        {
            string rollNo = Convert.ToString(txt_rollno.Text);

            if (!string.IsNullOrEmpty(rollNo))
            {
                if (ddlcollege.Items.Count > 0)
                    collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
                string appNo = getAppNo(rollNo, collegecode);
                string sqlcmd = "update applyn set studhouse=null where app_no=" + appNo;
                d2.update_method_wo_parameter(sqlcmd, "text");

                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), UniqueID, "alert('Deleted Successfully')", true);
                addnew_popup.Visible = false;
                btn_go_Click(sender, e);
            }


        }
        catch { }

    }

    protected void btn_exit_Click(object sender, EventArgs e)
    {
        addnew_popup.Visible = false;
    }

    protected string getAppNo(string rollNo, string collegecode)
    {
        string appNo = string.Empty;
        try
        {
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
            {
                appNo = d2.GetFunction("select app_no from Registration where roll_no='" + rollNo + "' and college_code='" + collegecode + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
            {
                appNo = d2.GetFunction("select app_no from Registration where Reg_no='" + rollNo + "' and college_code='" + collegecode + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
            {
                appNo = d2.GetFunction("select app_no from Registration where Roll_admit='" + rollNo + "' and college_code='" + collegecode + "'");
            }
            if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 3)
            {
                appNo = d2.GetFunction("select app_no from applyn where app_no='" + rollNo + "' and college_code='" + collegecode + "'");
            }
        }
        catch { }
        return appNo;
    }

    protected void btn_go_Click(object sender, EventArgs e)
    {
        try
        {
            string selectquery = "";
            Fpspread2.SaveChanges();
            if (ddlcollege.Items.Count > 0)
                collegecode = Convert.ToString(ddlcollege.SelectedItem.Value);
            //   string house = Convert.ToString(ddlhousename.SelectedItem.Value);
            string houseName = Convert.ToString(getCblSelectedValue(cbl_house));
            //string strHouse = string.Empty;
            //if (house.Trim()!="All")
            //{
            //    strHouse=" and studhouse='" + house+"'";
            //}

            string selqry = " select r.app_no,r.roll_no,r.reg_no,roll_admit,r.stud_name,r.batch_year,(select c.course_name from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code) as degree,(select dt.dept_name from degree d,course c,department dt where d.course_id=c.course_id and d.dept_code=dt.dept_code and d.degree_code=r.degree_code) as deptname,(select housename from HousingDetails h where h.housepk=a.studhouse) as houseName from registration r,applyn a where r.app_no=a.app_no and r.college_code=" + collegecode + " and studhouse in('" + houseName + "') and studhouse<>''";
            //if (string.IsNullOrEmpty(strHouse))
            //{
            //    selqry += strHouse;
            //}

            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                Fpspread2.Sheets[0].RowCount = 0;
                Fpspread2.Sheets[0].ColumnCount = 0;
                Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                Fpspread2.CommandBar.Visible = false;
                Fpspread2.Sheets[0].ColumnCount = 9;

                Fpspread2.Sheets[0].RowHeader.Visible = false;
                Fpspread2.Sheets[0].AutoPostBack = true;


                FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                darkstyle.ForeColor = Color.White;
                Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = " S.No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].Columns[0].Locked = true;
                Fpspread2.Columns[0].Width = 50;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[1].Locked = true;
                Fpspread2.Columns[1].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[2].Locked = true;
                Fpspread2.Columns[2].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Admit No";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[3].Locked = true;
                Fpspread2.Columns[3].Width = 100;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Name";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[4].Locked = true;
                Fpspread2.Columns[4].Width = 200;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Batch";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[5].Locked = true;
                Fpspread2.Columns[5].Width = 200;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Degree";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[6].Locked = true;
                Fpspread2.Sheets[0].Columns[6].VerticalAlign = VerticalAlign.Middle;
                Fpspread2.Sheets[0].SetColumnMerge(6, FarPoint.Web.Spread.Model.MergePolicy.Always);
                Fpspread2.Columns[6].Width = 270;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Branch";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[7].Locked = true;
                Fpspread2.Columns[7].Width = 200;

                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Text = "House";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                Fpspread2.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                Fpspread2.Sheets[0].Columns[8].Locked = true;
                Fpspread2.Columns[8].Width = 200;

                FarPoint.Web.Spread.TextCellType txtRollno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtRegno = new FarPoint.Web.Spread.TextCellType();
                FarPoint.Web.Spread.TextCellType txtAdmitno = new FarPoint.Web.Spread.TextCellType();

                int rowCnt = 0;
                int height = 0;
                for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                {
                    Fpspread2.Sheets[0].RowCount++;
                    height += 10;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(Fpspread2.Sheets[0].RowCount);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Tag = Convert.ToString(ds.Tables[0].Rows[row]["App_No"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].CellType = txtRollno;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_no"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txtRegno;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["reg_no"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].CellType = txtAdmitno;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["roll_admit"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["batch_year"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["degree"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(ds.Tables[0].Rows[row]["deptname"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(ds.Tables[0].Rows[row]["housename"]);
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Left;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                }
                fps_print.Visible = true;
                Fpspread2.Visible = true;
                print.Visible = true;
                lbl_error.Visible = false;
                Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                // Fpspread2.Sheets[0].SpanModel.Add(0, 2, 1, 4);
                // Fpspread2.Sheets[0].FrozenRowCount = 1;
                Fpspread2.Height = height;
                Fpspread2.SaveChanges();
            }
            else
            {
                Fpspread2.Visible = false;
                lbl_errormsg.Visible = true;
                lbl_errormsg.Text = "No Records Found";
                lbl_error.Visible = true;
                lbl_error.Text = "No Records Found ";
                print.Visible = false;
                fps_print.Visible = false;
            }

        }
        catch (Exception ex)
        {
        }

    }

    //protected void rblType_Selected(object sender, EventArgs e)
    //{
    //    if (rblType.SelectedIndex == 0)
    //    {
    //        txt_rollno.Text = "";
    //        txt_name.Text = "";
    //        btn_show.Visible = false;
    //        Fpspread2.Visible = false;
    //        main_filter.Visible = true;
    //        print.Visible = false;
    //        lbl_error.Visible = false;
    //        txtexcelname.Text = "";
    //        lblvalidation1.Visible = false;

    //    }
    //    else
    //    {
    //        btn_show.Visible = true;
    //        main_filter.Visible = false;
    //    }
    //}
    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {

            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                lblvalidation1.Visible = false;
                d2.printexcelreport(Fpspread2, reportname);

            }
            else
            {
                lblvalidation1.Text = "Please Enter Report Name";
                lblvalidation1.Visible = true;
                txtexcelname.Focus();
            }
        }
        catch
        { }
    }

    public void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            // lblvalidation1.Text = "";
            //string clgAcr = getclgAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            //string ledgerAcr = getledgerAcr(Convert.ToString(getCblSelectedValue(cblclg)));
            txtexcelname.Text = "";
            string degreedetails;
            string pagename;
            // degreedetails = "Headerwise Collection Report\n" + clgAcr + "\n Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "";
            degreedetails = "Housing Manual Allotment";
            //\n" + clgAcr + '@' + " Date   : " + txt_fromdate.Text + " To " + txt_todate.Text + "" + '@' + "Current Date : " + DateTime.Now.ToString("dd/MM/yyyy") + '@' + "Ledger : " + '@' + ledgerAcr;
            pagename = "HousingManualAllotment.aspx";
            Printcontrolhed.loadspreaddetails(Fpspread2, pagename, degreedetails);
            Printcontrolhed.Visible = true;
        }
        catch { }
    }

}