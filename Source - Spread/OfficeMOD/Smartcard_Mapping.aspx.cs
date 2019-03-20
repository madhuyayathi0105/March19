using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Configuration;

public partial class Smartcard_Mapping : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string collegecode = string.Empty;
    string collegecode1 = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    ReuasableMethods rs = new ReuasableMethods();
    static int chosedmode = 0;
    static int personmode = 0;
    bool cellclick = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        //string PageLogOut = "";
        //string sess = Convert.ToString(Session["IsLogin"]);
        //PageLogOut = Convert.ToString(Session["PageLogout"]);
        //if (sess == "")
        //{
        //}
        //else
        //{
        //    if (!Request.FilePath.Contains("OffM"))
        //    {
        //        string strPreviousPage = "";
        //        if (Request.UrlReferrer != null)
        //        {
        //            strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
        //        }
        //        if (strPreviousPage == "")
        //        {
        //            string redrURI = ConfigurationManager.AppSettings["Office"].Trim();
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

        if (!Request.FilePath.Contains("OfficeHome"))
        {
            string strPreviousPage = "";
            if (Request.UrlReferrer != null)
            {
                strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
            }
            if (strPreviousPage == "")
            {
                Response.Redirect("~/OfficeMOD/OfficeHome.aspx");
                return;
            }
        }
        collegecode = Convert.ToString(Session["collegecode"]);
        usercode = Convert.ToString(Session["usercode"]);
        lbl_str.Text = Convert.ToString(Session["streamcode"]);
        lbl_stream.Text = Convert.ToString(Session["streamcode"]);
        singleuser = Convert.ToString(Session["single_user"]);
        group_user = Convert.ToString(Session["group_code"]);
        Page.SetFocus(txt_smart);
        if (!IsPostBack)
        {
            rdo_stud1.Checked = true;
            rdo_stud1_onchecked(sender, e);
            bindclg();
            loadclg();
            if (ddl_maincol.Items.Count > 0)
            {
                collegecode1 = Convert.ToString(ddl_maincol.SelectedItem.Value);
            }
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
            loadfromsetting();
            txt_rdate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            bindcbldept();
            binddesig();
            bindstafftype();

        }
        if (ddl_maincol.Items.Count > 0)
        {
            collegecode1 = Convert.ToString(ddl_maincol.SelectedItem.Value);
        }
    }

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("default.aspx", false);
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string reportname = txtexcelname.Text;
            if (reportname.ToString().Trim() != "")
            {
                d2.printexcelreport(Fpspread1, reportname);
                lblprinterr.Visible = false;
            }
            else
            {
                lblprinterr.Text = "Please Enter Your Report Name";
                lblprinterr.Visible = true;
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
            string degreedetails = "Smart Card Mapping";
            string pagename = "Smartcard_Mapping.aspx";
            Printcontrol.loadspreaddetails(Fpspread1, pagename, degreedetails);
            Printcontrol.Visible = true;
            lblprinterr.Visible = false;
        }
        catch
        {

        }
    }

    protected void ddl_college_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ddl_college.SelectedIndex = ddl_college.Items.IndexOf(ddl_college.Items.FindByValue(ddl_college.SelectedItem.Value));
        }
        catch
        {

        }
    }

    protected void ddl_maincol_selectchanged(object sender, EventArgs e)
    {
        try
        {
            ddl_maincol.SelectedIndex = ddl_maincol.Items.IndexOf(ddl_maincol.Items.FindByValue(ddl_maincol.SelectedItem.Value));
            loadstrm();
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();
        }
        catch
        {

        }
    }

    protected void cb_batch_changed(object sender, EventArgs e)
    {
        try
        {
            txt_batch.Text = "--Select--";
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
            }
            binddeg();
            binddept();
            bindsem();
            bindsec();
        }
        catch { }
    }

    protected void cbl_batch_selected(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_batch.Checked = false;
            int commcount = 0;
            txt_batch.Text = "--Select--";
            for (i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_batch.Items.Count)
                {
                    cb_batch.Checked = true;
                }
                txt_batch.Text = "Batch(" + commcount.ToString() + ")";
            }
            binddeg();
            binddept();
            bindsem();
            bindsec();
        }
        catch { }
    }

    protected void ddlstream_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //string stream = ddlstream.SelectedItem.Text.ToString();
            //string selqry = "select distinct c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and type  in('" + stream + "') and d.college_code='" + collegecode1 + "'";
            //ds.Clear();
            //ds = d2.select_method_wo_parameter(selqry, "Text");

            //if (ds.Tables[0].Rows.Count > 0)
            //{

            //    cbl_degree.DataSource = ds;
            //    cbl_degree.DataTextField = "Course_Name";
            //    cbl_degree.DataValueField = "Course_Id";
            //    cbl_degree.DataBind();
            //}
            //for (int j = 0; j < cbl_degree.Items.Count; j++)
            //{
            //    cbl_degree.Items[j].Selected = true;
            //    cb_degree.Checked = true;
            //}
            //txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
            bindBtch();
            binddeg();
            binddept();
            bindsem();
            bindsec();

        }
        catch { }
    }

    protected void cb_degree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            if (cb_degree.Checked == true)
            {

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
                }
            }
            binddept();
            bindsem();
            bindsec();
        }
        catch { }
    }

    protected void cbl_degree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_dept.Checked = false;
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
                txt_degree.Text = "Degree(" + commcount.ToString() + ")";
            }
            binddept();
            bindsem();
            bindsec();
        }
        catch { }
    }

    protected void cb_dept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            txt_dept.Text = "--Select--";
            if (cb_dept.Checked == true)
            {

                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = true;
                }
                txt_dept.Text = "Department(" + (cbl_dept.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbl_dept.Items.Count; i++)
                {
                    cbl_dept.Items[i].Selected = false;
                }
            }
            bindsem();
            bindsec();

        }
        catch { }
    }

    protected void cbl_dept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int i = 0;
            cb_dept.Checked = false;
            int commcount = 0;
            txt_dept.Text = "--Select--";
            for (i = 0; i < cbl_dept.Items.Count; i++)
            {
                if (cbl_dept.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
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
            bindsem();
            bindsec();

        }
        catch { }
    }

    protected void cb_sem_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_seme.Text = "--Select--";
            string sem = "";
            if (cb_sem.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
                if (cbl_sem.Items.Count == 1)
                {
                    txt_seme.Text = "Semester(" + sem + ")";
                }
                else
                {
                    txt_seme.Text = "Semester(" + (cbl_sem.Items.Count) + ")";
                }
            }
            else
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = false;
                }
            }
            bindsec();
        }
        catch (Exception ex)
        {

        }
    }

    protected void cbl_sem_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            cb_sem.Checked = false;
            int commcount = 0;
            txt_seme.Text = "--Select--";
            string sem = "";

            for (int i = 0; i < cbl_sem.Items.Count; i++)
            {
                if (cbl_sem.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sem.Checked = false;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sem.Items.Count)
                {
                    cb_sem.Checked = true;
                }
                if (commcount == 1)
                {
                    txt_seme.Text = "Semester(" + sem + ")";
                }
                else
                {
                    txt_seme.Text = "Semester(" + commcount.ToString() + ")";
                }
            }

            bindsec();

        }
        catch (Exception ex)
        {

        }
    }

    protected void cb_sect_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            int cout = 0;
            txt_sect.Text = "--Select--";
            if (cb_sect.Checked == true)
            {
                cout++;
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = true;
                }
                txt_sect.Text = "Section(" + (cbl_sect.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_sect.Items.Count; i++)
                {
                    cbl_sect.Items[i].Selected = false;
                }
                txt_sect.Text = "--Select--";
            }

        }


        catch (Exception ex)
        {

        }
    }

    protected void cbl_sect_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            int commcount = 0;
            txt_sect.Text = "--Select--";
            cb_sect.Checked = false;

            for (int i = 0; i < cbl_sect.Items.Count; i++)
            {
                if (cbl_sect.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                    cb_sect.Checked = false;
                }
            }
            if (commcount > 0)
            {
                if (commcount == cbl_sect.Items.Count)
                {

                    cb_sect.Checked = true;
                }
                txt_sect.Text = "Section(" + commcount.ToString() + ")";

            }
        }

        catch (Exception ex)
        {

        }
    }

    protected void rbl_rollno_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clear();
            //bindspread();
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rerollno.Attributes.Add("Placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rerollno.Attributes.Add("Placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rerollno.Attributes.Add("Placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rerollno.Attributes.Add("Placeholder", "App No");
                    chosedmode = 3;
                    break;
            }
        }
        catch
        { }
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
                    query = "select top 100 Roll_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_No like '" + prefixText + "%' order by Roll_No";
                }
                else if (chosedmode == 1)
                {
                    query = "select  top 100 Reg_No from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Reg_No like '" + prefixText + "%' order by Reg_No";
                }
                else if (chosedmode == 2)
                {
                    query = "select  top 100 Roll_admit from Registration where CC=0 and DelFlag =0 and Exam_Flag <>'DEBAR' and Roll_admit like '" + prefixText + "%' order by Roll_admit";
                }
                else
                {
                    query = "  select  top 100 app_formno from applyn where admission_status =0 and isconfirm ='1' and app_formno like '" + prefixText + "%' order by app_formno";
                }
            }
            else if (personmode == 1)
            {
                //staff query
            }
            else if (personmode == 2)
            {
                //Vendor query
            }
            else
            {
                //Others query
            }

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }

    [System.Web.Services.WebMethod]
    [System.Web.Script.Services.ScriptMethod()]
    public static List<string> getstaffcode(string prefixText)
    {
        List<string> name = new List<string>();
        try
        {
            string query = "";
            WebService ws = new WebService();

            query = " select s.staff_Code as roll_no from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.staff_Code like '" + prefixText + "%' ";

            name = ws.Getname(query);
            return name;
        }
        catch { return name; }
    }
    protected void txt_rerollno_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string rollno = Convert.ToString(txt_rerollno.Text);
            string app_no = "";
            Session["App_No"] = "";
            lblvalidation1.Visible = false;
            string query = "select a.parent_name,a.stud_name, r.Roll_no,r.App_No,r.Stud_Type,c.Course_Name,dt.Dept_Name,r.Sections,r.Current_Semester,r.Batch_Year,d.Degree_Code,a.parent_addressP,a.parent_pincodec,Streetp,Cityp,StuPer_Id,Student_Mobile,(select TextVal from TextValTable where TextCode =ISNULL( parent_statep,0))as State,co.collname,co.college_code,C.type,isnull(r.smart_serial_no,'')smart_serial_no,isnull(r.Smart_BiouserID,'')Smart_BiouserID    from applyn a,Registration r ,Degree d,course c,Department dt,collinfo co where a.app_no=r.app_no and r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and co.college_code =d.college_code ";
            if (rollno != "" && rollno != null)
            {
                if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) != 3)
                {
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 0)
                    {
                        query = query + "and r.Roll_no='" + rollno + "' and d.college_code=" + collegecode1 + "";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        query = query + "and r.Reg_No='" + rollno + "' and d.college_code=" + collegecode1 + "";
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        query = query + "and r.Roll_Admit='" + rollno + "' and d.college_code=" + collegecode1 + "";
                    }
                }

                else
                {
                    query = "select stud_name,c.Course_Name+' - '+ dt.Dept_Name as degree ,ISNULL( type,'') as type from applyn a,Degree d,Department dt,Course c where a.degree_code =d.Degree_Code and dt.Dept_Code=d.Dept_Code and c.Course_Id =d.Course_Id and admission_status =0 and isconfirm ='1' and app_formno = '" + rollno + "'";
                }

                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        // txt_rerollno.Text = ds.Tables[0].Rows[i]["Roll_no"].ToString();
                        txt_rename.Text = ds.Tables[0].Rows[i]["stud_name"].ToString() + "-" + ds.Tables[0].Rows[i]["parent_name"].ToString() + "-" + ds.Tables[0].Rows[i]["Course_Name"].ToString() + "-" + ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_rebatch.Text = ds.Tables[0].Rows[i]["Batch_Year"].ToString();
                        txt_redegree.Text = ds.Tables[0].Rows[i]["Course_Name"].ToString();
                        txt_redept.Text = ds.Tables[0].Rows[i]["Dept_Name"].ToString();
                        txt_resec.Text = ds.Tables[0].Rows[i]["Sections"].ToString();
                        ddl_college.SelectedValue = ds.Tables[0].Rows[i]["college_code"].ToString();
                        txt_restrm.Text = ds.Tables[0].Rows[i]["type"].ToString();
                        txt_sem.Text = ds.Tables[0].Rows[i]["Current_Semester"].ToString();
                        txt_smart.Text = ds.Tables[0].Rows[i]["smart_serial_no"].ToString();
                        txt_smart.Attributes["value"] = txt_smart.Text;
                        TextBioUserid.Text = ds.Tables[0].Rows[i]["Smart_BiouserID"].ToString();
                        string degcode = Convert.ToString(ds.Tables[0].Rows[i]["Degree_Code"]);
                        ViewState["degid"] = degcode;
                        app_no = Convert.ToString(ds.Tables[0].Rows[i]["App_No"]);
                        Session["App_No"] = app_no;
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 1)
                    {
                        rollno = d2.GetFunction("select roll_no from registration where reg_no='" + rollno + "'");
                    }
                    if (Convert.ToInt32(rbl_rollno.SelectedItem.Value) == 2)
                    {
                        rollno = d2.GetFunction("select roll_no from registration where Roll_admit='" + rollno + "'");
                    }
                    image3.ImageUrl = "~/Handler4.ashx?rollno=" + rollno;
                }
            }
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                clear();
            }
            //bindGrid2();
            //bindspread();
        }
        catch
        {

        }
    }


    protected void txt_smart_change(object sender, EventArgs e)
    {
        try
        {
            txt_smart.Attributes.Add("TextMode", "Password");
        }
        catch
        {

        }
    }

    protected void imagebtnpopclose_Click(object sender, EventArgs e)
    {
        poperrjs.Visible = false;
    }

    protected void btnaddnew_click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = true;
            rdo_stud.Checked = true;
            rdo_staff.Checked = false;
            rdo_staff_onchecked(sender, e);
        }
        catch
        {

        }
    }

    protected void Cell_Click(object sender, EventArgs e)
    {
        try
        {
            cellclick = true;
        }
        catch
        {

        }
    }

    protected void Fpspread1_render(object sender, EventArgs e)
    {
        try
        {
            if (cellclick == true)
            {
                string actrow = "";
                string actcol = "";
                string degcode = "";
                string clgcode = "";
                string batch = "";
                string sem = "";
                string sec = "";
                string sel = "";

                actrow = Convert.ToString(Fpspread1.ActiveSheetView.ActiveRow);
                actcol = Convert.ToString(Fpspread1.ActiveSheetView.ActiveColumn);

                if (actrow.Trim() != "")
                {
                    clgcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 2].Tag);
                    degcode = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 3].Tag);
                    batch = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 1].Text);
                    sem = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 4].Text);
                    sec = Convert.ToString(Fpspread1.Sheets[0].Cells[Convert.ToInt32(actrow), 5].Text);

                    if (Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(actcol)].Text == "Students")
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                    }
                    else if (Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(actcol)].Text == "Smart Card Registered")
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and ISNULL(r.smart_serial_no,'')<>''";
                    }
                    else if (Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(actcol)].Text == "Smart Card Not Registered")
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and ISNULL(r.smart_serial_no,'')=''";
                    }
                    else if (Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(actcol)].Text == "Biometric UserID Registered")
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and ISNULL(r.Smart_BiouserID,'')<>''";
                    }
                    else if (Fpspread1.Sheets[0].ColumnHeader.Cells[0, Convert.ToInt32(actcol)].Text == "Biometric UserID Not Registered")
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar' and ISNULL(r.Smart_BiouserID,'')=''";
                    }
                    else
                    {
                        sel = "select r.stud_name,r.Roll_No,r.Reg_No from Registration r,Course c,Department dt,Degree d where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code =dt.Dept_Code  and d.Degree_Code in('" + degcode + "') and r.college_code in('" + clgcode + "') and r.Batch_Year in('" + batch + "') and r.Current_Semester in('" + sem + "') and r.Sections in('" + sec + "') and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                    }

                    ds.Clear();
                    ds = d2.select_method_wo_parameter(sel, "Text");
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            Fpspread2.Sheets[0].RowCount = 0;
                            Fpspread2.Sheets[0].ColumnCount = 0;
                            Fpspread2.CommandBar.Visible = false;
                            Fpspread2.Sheets[0].AutoPostBack = true;
                            Fpspread2.Sheets[0].ColumnHeader.RowCount = 1;
                            Fpspread2.Sheets[0].RowHeader.Visible = false;
                            Fpspread2.Sheets[0].ColumnCount = 4;

                            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                            darkstyle.ForeColor = Color.Black;
                            Fpspread2.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread2.Columns[0].Width = 50;

                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Roll Number";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                            // Fpspread1.Columns[2].Width = 200;
                            //Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Reg Number";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                            Fpspread2.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                            for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                            {
                                Fpspread2.Sheets[0].RowCount++;

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Roll_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Reg_No"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["stud_name"]);
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                                Fpspread2.Sheets[0].Cells[Fpspread2.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            }

                            Fpspread1.Sheets[0].SelectionBackColor = Color.Green;
                            Fpspread2.Sheets[0].PageSize = Fpspread2.Sheets[0].RowCount;
                            div2.Visible = true;
                            Fpspread2.Visible = true;
                            lbl_sprerr.Visible = false;
                            Fpspread1.SaveChanges();
                        }
                        else
                        {
                            lbl_sprerr.Visible = true;
                            lbl_sprerr.Text = "There are no Students!";
                            div2.Visible = false;
                            Fpspread2.Visible = false;
                        }
                    }
                }
            }
            else
            {
                div2.Visible = false;
                Fpspread2.Visible = false;
            }
        }
        catch
        {

        }
    }

    protected void FpSpread1_ButtonCommand(object sender, EventArgs e)
    {

    }

    protected void Cellcont_Click(object sender, EventArgs e)
    {

    }

    protected void Fpspread2_render(object sender, EventArgs e)
    {

    }

    protected void Fpspread2_ButtonCommand(object sender, EventArgs e)
    {

    }

    protected void btngo_click(object sender, EventArgs e)
    {
        try
        {
            Printcontrol.Visible = false;
            DataView dvnew = new DataView();
            DataView dsnew = new DataView();
            if (rdo_stud1.Checked == true)
            {
                string batchyear = "";
                string courseid = "";
                string degreecode = "";
                string semcode = "";
                string section = "";
                double total = 0.0;
                double regtot = 0.0;
                double notregtot = 0.0;

                double total1 = 0.0;
                double regtot1 = 0.0;
                double notregtot1 = 0.0;

                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        if (cbl_batch.Items[i].Selected == true)
                        {
                            if (batchyear.Trim() == "")
                            {
                                batchyear = "" + Convert.ToString(cbl_batch.Items[i].Value) + "";
                            }
                            else
                            {
                                batchyear = batchyear + "'" + "," + "'" + cbl_batch.Items[i].Value.ToString();
                            }
                        }
                    }
                }

                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        if (cbl_degree.Items[i].Selected == true)
                        {
                            if (courseid.Trim() == "")
                            {
                                courseid = "" + Convert.ToString(cbl_degree.Items[i].Value) + "";
                            }
                            else
                            {
                                courseid = courseid + "'" + "," + "'" + cbl_degree.Items[i].Value.ToString();
                            }
                        }
                    }
                }

                if (cbl_dept.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_dept.Items.Count; i++)
                    {
                        if (cbl_dept.Items[i].Selected == true)
                        {
                            if (degreecode.Trim() == "")
                            {
                                degreecode = "" + Convert.ToString(cbl_dept.Items[i].Value) + "";
                            }
                            else
                            {
                                degreecode = degreecode + "'" + "," + "'" + cbl_dept.Items[i].Value.ToString();
                            }
                        }
                    }
                }

                if (cbl_sem.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sem.Items.Count; i++)
                    {
                        if (cbl_sem.Items[i].Selected == true)
                        {
                            if (semcode.Trim() == "")
                            {
                                semcode = "" + Convert.ToString(cbl_sem.Items[i].Value) + "";
                            }
                            else
                            {
                                semcode = semcode + "'" + "," + "'" + cbl_sem.Items[i].Value.ToString();
                            }
                        }
                    }
                }

                if (cbl_sect.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_sect.Items.Count; i++)
                    {
                        if (cbl_sect.Items[i].Selected == true)
                        {
                            if (section.Trim() == "")
                            {
                                section = "" + Convert.ToString(cbl_sect.Items[i].Value) + "";
                            }
                            else
                            {
                                section = section + "'" + "," + "'" + cbl_sect.Items[i].Value.ToString();
                            }
                        }
                    }
                }

                string selq = "";
                string sql = "";
                string bothq = "";
                string sql1 = "";
                string selq1 = "";
                selq = "select c.Course_Name,dt.Dept_Name,COUNT(r.App_No) as total,r.Batch_Year,r.Current_Semester,r.Sections,r.degree_code,r.college_code from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(r.Smart_BiouserID,'')='' and r.college_code='" + collegecode1 + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                selq1 = "select c.Course_Name,dt.Dept_Name,COUNT(r.App_No) as total,r.Batch_Year,r.Current_Semester,r.Sections,r.degree_code,r.college_code from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(r.smart_serial_no,'')='' and r.college_code='" + collegecode1 + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                sql = "select c.Course_Name,dt.Dept_Name,COUNT(r.App_No) as total,r.Batch_Year,r.Current_Semester,r.Sections,r.degree_code,r.college_code from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(r.smart_serial_no,'')<>'' and r.college_code='" + collegecode1 + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                bothq = "select c.Course_Name,dt.Dept_Name,COUNT(r.App_No) as total,r.Batch_Year,r.Current_Semester,r.Sections,r.degree_code,r.college_code from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and r.college_code='" + collegecode1 + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";
                sql1 = "select c.Course_Name,dt.Dept_Name,COUNT(r.App_No) as total,r.Batch_Year,r.Current_Semester,r.Sections,r.degree_code,r.college_code from Course c,Degree d,Department dt,Registration r where r.degree_code=d.Degree_Code and d.Course_Id=c.Course_Id and d.Dept_Code=dt.Dept_Code and ISNULL(r.Smart_BiouserID,'')<>'' and r.college_code='" + collegecode1 + "' and CC=0 and DelFlag =0 and Exam_Flag <>'Debar'";

                if (batchyear.Trim() != "")
                {
                    selq = selq + " and r.Batch_Year in ('" + batchyear + "')";
                    sql = sql + " and r.Batch_Year in ('" + batchyear + "')";
                    selq1 = selq1 + " and r.Batch_Year in ('" + batchyear + "')";
                    sql1 = sql1 + " and r.Batch_Year in ('" + batchyear + "')";
                    bothq = bothq + " and r.Batch_Year in ('" + batchyear + "')";
                }
                if (courseid.Trim() != "")
                {
                    selq = selq + " and c.Course_Id in('" + courseid + "')";
                    sql = sql + " and c.Course_Id in('" + courseid + "')";
                    selq1 = selq1 + " and c.Course_Id in('" + courseid + "')";
                    sql1 = sql1 + " and c.Course_Id in('" + courseid + "')";
                    bothq = bothq + " and c.Course_Id in('" + courseid + "')";
                }
                if (degreecode.Trim() != "")
                {
                    selq = selq + " and r.degree_code in('" + degreecode + "')";
                    sql = sql + " and r.degree_code in('" + degreecode + "')";
                    selq1 = selq1 + " and r.degree_code in('" + degreecode + "')";
                    sql1 = sql1 + " and r.degree_code in('" + degreecode + "')";
                    bothq = bothq + " and r.degree_code in('" + degreecode + "')";
                }
                if (semcode.Trim() != "")
                {
                    selq = selq + " and r.Current_Semester in('" + semcode + "')";
                    sql = sql + " and r.Current_Semester in('" + semcode + "')";
                    selq1 = selq1 + " and r.Current_Semester in('" + semcode + "')";
                    sql1 = sql1 + " and r.Current_Semester in('" + semcode + "')";
                    bothq = bothq + " and r.Current_Semester in('" + semcode + "')";
                }
                if (section.Trim() != "")
                {
                    selq = selq + " and Sections in('" + section + "')";
                    sql = sql + " and Sections in('" + section + "')";
                    selq1 = selq1 + " and Sections in('" + section + "')";
                    sql1 = sql1 + " and Sections in('" + section + "')";
                    bothq = bothq + " and Sections in('" + section + "')";
                }
                selq = selq + " group by Course_Name,Dept_Name,r.degree_code,r.college_code,r.Batch_Year,r.Current_Semester,r.Sections";
                sql = sql + " group by Course_Name,Dept_Name,r.degree_code,r.college_code,r.Batch_Year,r.Current_Semester,r.Sections";
                selq1 = selq1 + " group by Course_Name,Dept_Name,r.degree_code,r.college_code,r.Batch_Year,r.Current_Semester,r.Sections";
                sql1 = sql1 + " group by Course_Name,Dept_Name,r.degree_code,r.college_code,r.Batch_Year,r.Current_Semester,r.Sections";
                bothq = bothq + " group by Course_Name,Dept_Name,r.degree_code,r.college_code,r.Batch_Year,r.Current_Semester,r.Sections";

                selq = bothq + " " + sql + " " + selq + " " + sql1 + " " + selq1;

                ds.Clear();
                ds = d2.select_method_wo_parameter(selq, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 11;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[0].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Batch";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
                        // Fpspread1.Columns[2].Width = 200;
                        //Fpspread1.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Degree";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
                        //Fpspread1.Columns[2].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Department";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Semester";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Section";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Students";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Smart Card Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Smart Card Not Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Text = "Biometric UserID Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 9].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Biometric UserID Not Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 10].HorizontalAlign = HorizontalAlign.Center;

                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["Course_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[row]["college_code"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["Dept_Name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Columns[3].Width = 175;

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[row]["Sections"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(ds.Tables[0].Rows[row]["total"]);
                            total = total + Convert.ToDouble(ds.Tables[0].Rows[row]["total"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";

                            ds.Tables[1].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[row]["college_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[row]["Sections"]) + "'";
                            dvnew = ds.Tables[1].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["total"]);
                                regtot = regtot + Convert.ToDouble(dvnew[0]["total"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            ds.Tables[2].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[row]["college_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[row]["Sections"]) + "'";
                            dsnew = ds.Tables[2].DefaultView;
                            if (dsnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dsnew[0]["total"]);
                                notregtot = notregtot + Convert.ToDouble(dsnew[0]["total"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";




                            ds.Tables[3].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[row]["college_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[row]["Sections"]) + "'";
                            dvnew = ds.Tables[3].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(dvnew[0]["total"]);
                                regtot1 = regtot1 + Convert.ToDouble(dvnew[0]["total"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";

                            ds.Tables[4].DefaultView.RowFilter = "degree_code='" + Convert.ToString(ds.Tables[0].Rows[row]["degree_code"]) + "' and college_code='" + Convert.ToString(ds.Tables[0].Rows[row]["college_code"]) + "' and Batch_Year='" + Convert.ToString(ds.Tables[0].Rows[row]["Batch_Year"]) + "' and Current_Semester='" + Convert.ToString(ds.Tables[0].Rows[row]["Current_Semester"]) + "' and Sections='" + Convert.ToString(ds.Tables[0].Rows[row]["Sections"]) + "'";
                            dsnew = ds.Tables[4].DefaultView;
                            if (dsnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(dsnew[0]["total"]);
                                notregtot1 = notregtot1 + Convert.ToDouble(dsnew[0]["total"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        }

                        Fpspread1.Sheets[0].RowCount++;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = "Total No.of Students";
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(total);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(regtot);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(notregtot);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";


                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Text = Convert.ToString(regtot1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Text = Convert.ToString(notregtot1);
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 10].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].SpanModel.Add(Fpspread1.Sheets[0].RowCount - 1, 0, 1, 6);
                        Fpspread1.Sheets[0].Rows[Fpspread1.Sheets[0].RowCount - 1].BackColor = ColorTranslator.FromHtml("#0CA6CA");

                        Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                        div1.Visible = true;
                        Fpspread1.Visible = true;
                        rptprint.Visible = true;
                        lbl_err.Visible = false;
                    }
                    else
                    {
                        div1.Visible = false;
                        Fpspread1.Visible = false;
                        rptprint.Visible = false;
                        lbl_err.Visible = true;
                        lbl_err.Text = "No Record Found!";
                    }
                }
            }
            if (rdo_staff2.Checked == true)
            {
                string desigcode = "";
                string stafftype = "";
                string deptcode = ""; double total = 0; double regtot = 0; double notregtot = 0; double regtot1 = 0; double notregtot1 = 0;

                desigcode = rs.GetSelectedItemsValueAsString(cbl_design);
                stafftype = rs.GetSelectedItemsValueAsString(cbl_stafftype);
                deptcode = rs.GetSelectedItemsValueAsString(cbl_department);


                string q1 = " select h.dept_name,h.dept_code,sa.staff_type ,COUNT(s.Staff_code)reg,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d ,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.Staff_code =s.staff_code and s.staff_code =st.staff_code and s.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1  ";
                if (desigcode.Trim() != "")
                    q1 += " and  d.desig_code in ('" + desigcode + "')";
                if (stafftype.Trim() != "")
                    q1 += " and st.StfType in('" + stafftype + "')";
                if (deptcode.Trim() != "")
                    q1 += " and h.dept_code in('" + deptcode + "')";
                q1 += " group by h.dept_name,h.dept_code,sa.staff_type ,d.desig_code,d.desig_name";
                q1 += " select h.dept_name,h.dept_code,sa.staff_type ,COUNT(s.Staff_code)reg,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d ,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.Staff_code =s.staff_code and s.staff_code =st.staff_code and s.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and s.Smartcard_serial_no<>'' ";
                if (desigcode.Trim() != "")
                    q1 += " and  d.desig_code in ('" + desigcode + "')";
                if (stafftype.Trim() != "")
                    q1 += " and st.StfType in('" + stafftype + "')";
                if (deptcode.Trim() != "")
                    q1 += " and h.dept_code in('" + deptcode + "')";
                q1 += " group by h.dept_name,h.dept_code,sa.staff_type ,d.desig_code,d.desig_name";
                q1 += "  select h.dept_name,h.dept_code,sa.staff_type ,COUNT(s.Staff_code)reg,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d ,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.Staff_code =s.staff_code and s.staff_code =st.staff_code and s.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and isnull(s.Smartcard_serial_no,'')=''";
                if (desigcode.Trim() != "")
                    q1 += " and  d.desig_code in ('" + desigcode + "')";
                if (stafftype.Trim() != "")
                    q1 += " and st.StfType in('" + stafftype + "')";
                if (deptcode.Trim() != "")
                    q1 += " and h.dept_code in('" + deptcode + "')";
                q1 += " group by h.dept_name,h.dept_code,sa.staff_type ,d.desig_code,d.desig_name";

                string q2 = " select h.dept_name,h.dept_code,sa.staff_type ,COUNT(s.Staff_code)reg,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d ,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.Staff_code =s.staff_code and s.staff_code =st.staff_code and s.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and s.Smartcard_BiouserID<>'' ";
                if (desigcode.Trim() != "")
                    q2 += " and  d.desig_code in ('" + desigcode + "')";
                if (stafftype.Trim() != "")
                    q2 += " and st.StfType in('" + stafftype + "')";
                if (deptcode.Trim() != "")
                    q2 += " and h.dept_code in('" + deptcode + "')";
                q2 += " group by h.dept_name,h.dept_code,sa.staff_type ,d.desig_code,d.desig_name";
                q2 += "  select h.dept_name,h.dept_code,sa.staff_type ,COUNT(s.Staff_code)reg,d.desig_code,d.desig_name from staffmaster s,stafftrans st,hrdept_master h,desig_master d ,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.Staff_code =s.staff_code and s.staff_code =st.staff_code and s.Staff_code =st.staff_code and st.dept_code =h.dept_code and st.desig_code =d.desig_code and latestrec =1 and isnull(s.Smartcard_BiouserID,'')=''";
                if (desigcode.Trim() != "")
                    q2 += " and  d.desig_code in ('" + desigcode + "')";
                if (stafftype.Trim() != "")
                    q2 += " and st.StfType in('" + stafftype + "')";
                if (deptcode.Trim() != "")
                    q2 += " and h.dept_code in('" + deptcode + "')";
                q2 += " group by h.dept_name,h.dept_code,sa.staff_type ,d.desig_code,d.desig_name";
                q1 = q1 + " " + q2;
                ds = d2.select_method_wo_parameter(q1, "Text");
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Fpspread1.Sheets[0].RowCount = 0;
                        Fpspread1.Sheets[0].ColumnCount = 0;
                        Fpspread1.CommandBar.Visible = false;
                        Fpspread1.Sheets[0].AutoPostBack = true;
                        Fpspread1.Sheets[0].ColumnHeader.RowCount = 1;
                        Fpspread1.Sheets[0].RowHeader.Visible = false;
                        Fpspread1.Sheets[0].ColumnCount = 9;

                        FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
                        darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                        darkstyle.ForeColor = Color.Black;
                        Fpspread1.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
                        Fpspread1.Columns[0].Width = 50;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Department";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Designation";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Staff Type";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 3].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Staff";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 4].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Text = "SmartCard Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 5].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Text = "SmartCard Not Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 6].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Text = "Biometric Userid Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 7].HorizontalAlign = HorizontalAlign.Center;

                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Text = "Biometric Userid Not Registered";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Bold = true;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Name = "Book Antiqua";
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].Font.Size = FontUnit.Medium;
                        Fpspread1.Sheets[0].ColumnHeader.Cells[0, 8].HorizontalAlign = HorizontalAlign.Center;
                        for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                        {
                            Fpspread1.Sheets[0].RowCount++;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(row + 1);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[row]["dept_name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[row]["desig_name"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(ds.Tables[0].Rows[row]["staff_type"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                            Fpspread1.Sheets[0].Columns[3].Width = 175;

                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[row]["reg"]);
                            total = total + Convert.ToDouble(ds.Tables[0].Rows[row]["reg"]);
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";

                            ds.Tables[1].DefaultView.RowFilter = " desig_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]) + "' and staff_type ='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_type"]) + "' and dept_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]) + "'";
                            dvnew = ds.Tables[1].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dvnew[0]["reg"]);
                                regtot = regtot + Convert.ToDouble(dvnew[0]["reg"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";

                            ds.Tables[2].DefaultView.RowFilter = " desig_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]) + "' and staff_type ='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_type"]) + "' and dept_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]) + "'";
                            dsnew = ds.Tables[2].DefaultView;
                            if (dsnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dsnew[0]["reg"]);
                                notregtot = notregtot + Convert.ToDouble(dsnew[0]["reg"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";


                            ds.Tables[3].DefaultView.RowFilter = " desig_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]) + "' and staff_type ='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_type"]) + "' and dept_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]) + "'";
                            dvnew = ds.Tables[3].DefaultView;
                            if (dvnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dvnew[0]["reg"]);
                                regtot1 = regtot1 + Convert.ToDouble(dvnew[0]["reg"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";

                            ds.Tables[4].DefaultView.RowFilter = " desig_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["desig_code"]) + "' and staff_type ='" + Convert.ToString(ds.Tables[0].Rows[row]["staff_type"]) + "' and dept_code ='" + Convert.ToString(ds.Tables[0].Rows[row]["dept_code"]) + "'";
                            dsnew = ds.Tables[4].DefaultView;
                            if (dsnew.Count > 0)
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dsnew[0]["reg"]);
                                notregtot1 = notregtot1 + Convert.ToDouble(dsnew[0]["reg"]);
                            }
                            else
                            {
                                Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Text = "-";
                            }
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                            Fpspread1.Sheets[0].Cells[Fpspread1.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";

                            Fpspread1.Sheets[0].PageSize = Fpspread1.Sheets[0].RowCount;
                            div1.Visible = true;
                            Fpspread1.Visible = true;
                            rptprint.Visible = true;
                            lbl_err.Visible = false;
                        }
                    }
                }
                else
                {
                    div1.Visible = false;
                    Fpspread1.Visible = false;
                    rptprint.Visible = false;
                    lbl_err.Visible = true;
                    lbl_err.Text = "No Record Found!";
                }
            }
        }
        catch
        {

        }
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (rdo_stud.Checked == true)
            {
                string app_no = Convert.ToString(Session["App_No"]);
                string serialno = Convert.ToString(txt_smart.Text);
                string BioUserid = Convert.ToString(TextBioUserid.Text);
                int updcount = 0;
                //Added By Saranyadevi2.2.2019
                if (app_no.Trim() != "")
                {
                    if (BioUserid == "" && serialno == "")
                    {
                        lblvalidation1.Visible = true;
                        lblvalidation1.Text = "Please Enter the SmartCard No Or Biometric UserID!";
                    }

                    if (BioUserid != "")
                    {

                        string update = "update Registration set Smart_BiouserID='" + BioUserid + "' where App_No='" + app_no + "'";
                        updcount = d2.update_method_wo_parameter(update, "Text");


                    }
                    if (serialno.Trim() != "")
                    {
                        string update = "update Registration set smart_serial_no='" + serialno + "' where App_No='" + app_no + "'";
                        //string update = "update Registration set smart_serial_no='" + serialno + "',Smart_BiouserID='" + BioUserid + "'  where App_No='" + app_no + "'";
                        updcount = d2.update_method_wo_parameter(update, "Text");

                    }
                    if (updcount > 0)
                    {
                        lblvalidation1.Visible = true;
                        lblvalidation1.Text = "Saved Successfully";
                        clear();
                    }


                }
            }
            if (rdo_staff.Checked == true)
            {
                string appl_id = Convert.ToString(ViewState["staff_applid"]);
                string serialno = Convert.ToString(txt_smart.Text);
                string BioUserid = Convert.ToString(TextBioUserid.Text);
                int updcount = 0;

                if (appl_id.Trim() != "")
                {

                    if (BioUserid == "" && serialno == "")
                    {
                        lblvalidation1.Visible = true;
                        lblvalidation1.Text = "Please Enter the SmartCard No Or Biometric UserID!";
                    }
                    if (BioUserid != "")
                    {
                        string update = "update staffmaster  set Smartcard_BiouserID='" + BioUserid + "'  from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and sa.appl_id='" + appl_id + "'";
                        updcount = d2.update_method_wo_parameter(update, "Text");
                    }

                    if (serialno.Trim() != "")
                    {
                        string update = " update staffmaster  set Smartcard_serial_no='" + serialno + "'  from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and sa.appl_id='" + appl_id + "'";
                        updcount = d2.update_method_wo_parameter(update, "Text");

                    }
                    if (updcount > 0)
                    {
                        lblvalidation1.Visible = true;
                        lblvalidation1.Text = "Saved Successfully";
                        clear();
                    }

                }
            }
        }
        catch
        {

        }
    }

    protected void btncancel_Click(object sender, EventArgs e)
    {
        try
        {
            poperrjs.Visible = false;
        }
        catch
        {

        }
    }

    public void loadfromsetting()
    {
        try
        {
            ListItem list1 = new ListItem("Roll No", "0");
            ListItem list2 = new ListItem("Reg No", "1");
            ListItem list3 = new ListItem("Admission No", "2");
            ListItem list4 = new ListItem("App No", "3");

            //Roll Number or Reg Number or Admission No or Application Number
            rbl_rollno.Items.Clear();
            string insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";

            int save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list1);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRegNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list2);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptRollAdmit' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));
            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list3);
            }

            insqry1 = "select LinkValue from New_InsSettings where LinkName='ChallanReceiptAppFormNo' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "' ";
            save1 = Convert.ToInt32(d2.GetFunction(insqry1));

            if (save1 == 1)
            {
                rbl_rollno.Items.Add(list4);
            }
            if (rbl_rollno.Items.Count == 0)
            {
                rbl_rollno.Items.Add(list1);
            }
            switch (Convert.ToUInt32(rbl_rollno.SelectedItem.Value))
            {
                case 0:
                    txt_rerollno.Attributes.Add("placeholder", "Roll No");
                    chosedmode = 0;
                    break;
                case 1:
                    txt_rerollno.Attributes.Add("placeholder", "Reg No");
                    chosedmode = 1;
                    break;
                case 2:
                    txt_rerollno.Attributes.Add("placeholder", "Admin No");
                    chosedmode = 2;
                    break;
                case 3:
                    txt_rerollno.Attributes.Add("placeholder", "App No");
                    chosedmode = 3;
                    break;
            }

        }
        catch { }
    }

    public void bindclg()
    {
        try
        {
            ds.Clear();
            ddl_college.Items.Clear();

            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
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

    public void loadclg()
    {
        try
        {
            ds.Clear();
            ddl_maincol.Items.Clear();

            string selectQuery = "select cp.college_code,cf.collname from collegeprivilages cp,collinfo cf where user_code=" + Session["usercode"] + " and cp.college_code=cf.college_code";
            ds = d2.select_method_wo_parameter(selectQuery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_maincol.DataSource = ds;
                ddl_maincol.DataTextField = "collname";
                ddl_maincol.DataValueField = "college_code";
                ddl_maincol.DataBind();

                ddl_staffclg.DataSource = ds;
                ddl_staffclg.DataTextField = "collname";
                ddl_staffclg.DataValueField = "college_code";
                ddl_staffclg.DataBind();
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindBtch()
    {
        try
        {
            cbl_batch.Items.Clear();
            cb_batch.Checked = false;
            txt_batch.Text = "---Select---";
            ds.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_batch.DataSource = ds;
                cbl_batch.DataTextField = "batch_year";
                cbl_batch.DataValueField = "batch_year";
                cbl_batch.DataBind();
                if (cbl_batch.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_batch.Items.Count; i++)
                    {
                        cbl_batch.Items[i].Selected = true;
                    }
                    txt_batch.Text = "Batch(" + cbl_batch.Items.Count + ")";
                    cb_batch.Checked = true;
                }
            }
        }
        catch { }
    }

    public void loadstrm()
    {
        try
        {
            ddlstream.Items.Clear();
            string selqry = "select distinct type  from Course where college_code ='" + collegecode1 + "' and type<>''";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlstream.DataSource = ds;
                ddlstream.DataTextField = "type";
                ddlstream.DataValueField = "type";
                ddlstream.DataBind();
                ddlstream.Enabled = true;
            }
            else
            {
                ddlstream.Enabled = false;
            }

        }
        catch
        { }
    }

    public void binddeg()
    {
        try
        {
            cbl_degree.Items.Clear();
            cb_degree.Checked = false;
            txt_degree.Text = "---Select---";
            string stream = "";
            if (ddlstream.Items.Count > 0)
            {
                if (ddlstream.SelectedItem.Text != "")
                {
                    stream = ddlstream.SelectedItem.Text.ToString();
                }
            }

            cbl_degree.Items.Clear();
            ds.Clear();
            string selqry = "select distinct  c.Course_Name,c.Course_Id  from Degree d,Department dt,Course c where d.Dept_Code =dt.Dept_Code and d.Course_Id =c.Course_Id  and d.college_code='" + collegecode1 + "'";
            if (stream != "")
            {
                selqry = selqry + " and type  in('" + stream + "')";
            }
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                cbl_degree.DataSource = ds;
                cbl_degree.DataTextField = "course_name";
                cbl_degree.DataValueField = "course_id";
                cbl_degree.DataBind();
                if (cbl_degree.Items.Count > 0)
                {
                    for (int i = 0; i < cbl_degree.Items.Count; i++)
                    {
                        cbl_degree.Items[i].Selected = true;
                    }
                    txt_degree.Text = "Degree(" + cbl_degree.Items.Count + ")";
                    cb_degree.Checked = true;
                }
            }
        }
        catch { }
    }

    public void binddept()
    {
        try
        {
            cbl_dept.Items.Clear();
            cb_dept.Checked = false;
            txt_dept.Text = "---Select---";
            string batch2 = "";
            for (int i = 0; i < cbl_batch.Items.Count; i++)
            {
                if (cbl_batch.Items[i].Selected == true)
                {
                    if (batch2 == "")
                    {
                        batch2 = Convert.ToString(cbl_batch.Items[i].Text);
                    }
                    else
                    {
                        batch2 += "','" + Convert.ToString(cbl_batch.Items[i].Text);
                    }
                }

            }

            string degree = "";
            for (int i = 0; i < cbl_degree.Items.Count; i++)
            {
                if (cbl_degree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbl_degree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbl_degree.Items[i].Value);
                    }
                }

            }

            if (batch2 != "" && degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode1, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_dept.DataSource = ds;
                    cbl_dept.DataTextField = "dept_name";
                    cbl_dept.DataValueField = "degree_code";
                    cbl_dept.DataBind();
                    if (cbl_dept.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_dept.Items.Count; i++)
                        {
                            cbl_dept.Items[i].Selected = true;
                        }
                        txt_dept.Text = "Department(" + cbl_dept.Items.Count + ")";
                        cb_dept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }

    public void bindsem()
    {
        string sem = "";
        cbl_sem.Items.Clear();
        txt_seme.Text = "---Select---";
        //string settingquery = "select * from New_InsSettings where linkname = 'Fee Yearwise' and user_code ='" + usercode + "' and college_code ='" + collegecode1 + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(settingquery, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    string linkvalue = Convert.ToString(ds.Tables[0].Rows[0]["LinkValue"]);
        //if (linkvalue == "0")
        //{
        //string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Semester' and textval not like '-1%' and college_code ='" + collegecode1 + "' order by textval asc";
        string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
        string branch = rs.GetSelectedItemsValueAsString(cbl_dept);


        if (batch != "" && branch != "")
        {
            string semesterquery = "select distinct Current_Semester from Registration where college_code='" + collegecode1 + "' and Batch_year in('" + batch + "') and Degree_Code in('" + branch + "') order by Current_Semester";
            ds.Clear();
            ds = d2.select_method_wo_parameter(semesterquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                //text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
                cbl_sem.DataSource = ds;
                cbl_sem.DataTextField = "Current_Semester";
                cbl_sem.DataValueField = "Current_Semester";
                cbl_sem.DataBind();
            }
            if (cbl_sem.Items.Count > 0)
            {
                for (int i = 0; i < cbl_sem.Items.Count; i++)
                {
                    cbl_sem.Items[i].Selected = true;
                    sem = Convert.ToString(cbl_sem.Items[i].Text);
                }
                if (cbl_sem.Items.Count == 1)
                {
                    txt_seme.Text = "Semester(" + sem + ")";
                }
                else
                {
                    txt_seme.Text = "Semester(" + cbl_sem.Items.Count + ")";
                }
                cb_sem.Checked = true;
            }
        }
        //}
        //else
        //{
        //string semesterquery = "select * from textvaltable where TextCriteria = 'FEECA'and textval like '%Year' and textval not like '-1%' and college_code ='" + collegecode1 + "'";
        //ds.Clear();
        //ds = d2.select_method_wo_parameter(semesterquery, "Text");
        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    // text_circode = Convert.ToString(ds.Tables[0].Rows[0]["TextCode"]);
        //    cbl_sem.DataSource = ds;
        //    cbl_sem.DataTextField = "TextVal";
        //    cbl_sem.DataValueField = "TextCode";
        //    cbl_sem.DataBind();
        //}
        //if (cbl_sem.Items.Count > 0)
        //{
        //    for (int i = 0; i < cbl_sem.Items.Count; i++)
        //    {
        //        cbl_sem.Items[i].Selected = true;
        //        sem = Convert.ToString(cbl_sem.Items[i].Text);
        //    }
        //    if (cbl_sem.Items.Count == 1)
        //    {
        //        txt_sem.Text = "Semester(" + sem + ")";
        //    }
        //    else
        //    {
        //        txt_sem.Text = "Semester(" + cbl_sem.Items.Count + ")";
        //    }
        //    cb_sem.Checked = true;
        //}
        //}
    }

    public void bindsec()
    {
        try
        {
            cbl_sect.Items.Clear();
            txt_sect.Text = "---Select---";
            cb_sect.Checked = false;
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
            string batch = rs.GetSelectedItemsValueAsString(cbl_batch);
            string branch = rs.GetSelectedItemsValueAsString(cbl_dept);


            if (build != "" && batch != "" && branch != "")
            {

                ds = d2.select_method_wo_parameter("select distinct sections from registration where college_code='" + collegecode1 + "' and Batch_year in('" + batch + "') and Degree_Code in('" + branch + "') and sections<>'-1' and ltrim(sections)<>'' and sections is not null and delflag=0 and exam_flag<>'Debar'", "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_sect.DataSource = ds;
                    cbl_sect.DataTextField = "sections";
                    cbl_sect.DataValueField = "sections";
                    cbl_sect.DataBind();
                    if (cbl_sect.Items.Count > 0)
                    {
                        for (int row = 0; row < cbl_sect.Items.Count; row++)
                        {
                            cbl_sect.Items[row].Selected = true;
                        }
                        txt_sect.Text = "Section(" + cbl_sect.Items.Count + ")";
                        cb_sect.Checked = true;
                    }

                }
            }
            else
            {
                cb_sect.Checked = false;
                txt_sect.Text = "--Select--";
            }
        }

        catch (Exception ex)
        {
        }
    }

    public void clear()
    {
        txt_rerollno.Text = "";
        txt_rebatch.Text = "";
        txt_redegree.Text = "";
        txt_redept.Text = "";
        txt_resec.Text = "";
        txt_sem.Text = "";
        txt_smart.Text = "";
        ddl_college.SelectedIndex = 0;
        txt_restrm.Text = "";
        txt_rename.Text = "";
        image3.ImageUrl = "";

        txt_smart.Attributes["value"] = "";
        TextBioUserid.Text = "";
        txt_studentname.Text = "";
        txt_staff_code.Text = "";
        txt_studenttype.Text = "";
        txt_desig.Text = "";
        lbl_studimage.ImageUrl = "";
        txt_staffcode.Text = "";
    }
    protected void rdo_stud_onchecked(object sender, EventArgs e)
    {
        if (rdo_stud.Checked == true)
        {
            rbl_rollno.Visible = true;
            txt_rerollno.Visible = true;
            ddl_staffcode.Visible = false;
            txt_staffcode.Visible = false;
            stafftbl_det.Visible = false;
            div_refund.Visible = true;
        }
        else
        {
            rbl_rollno.Visible = false;
            txt_rerollno.Visible = false;
            ddl_staffcode.Visible = true;
            txt_staffcode.Visible = true;
            stafftbl_det.Visible = true;
            div_refund.Visible = false;
        }
    }
    protected void rdo_staff_onchecked(object sender, EventArgs e)
    {
        if (rdo_stud.Checked == true)
        {
            rbl_rollno.Visible = true;
            txt_rerollno.Visible = true;
            ddl_staffcode.Visible = false;
            txt_staffcode.Visible = false;
            stafftbl_det.Visible = false;
            div_refund.Visible = true;
        }
        else
        {
            rbl_rollno.Visible = false;
            txt_rerollno.Visible = false;
            ddl_staffcode.Visible = true;
            txt_staffcode.Visible = true;
            stafftbl_det.Visible = true;
            div_refund.Visible = false;
        }
    }
    protected void txt_staffcode_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ds.Clear();
            string staffcode = Convert.ToString(txt_staffcode.Text);
            lblvalidation1.Visible = false;
            string query = " select sa.appl_id ,s.staff_Code ,s.staff_name ,desig_name ,sa.staff_type ,s.college_code,isnull(Smartcard_serial_no,'')Smartcard_serial_no,isnull(Smartcard_BiouserID,'')Smartcard_BiouserID from staffmaster s,staff_appl_master sa where s.appl_no=sa.appl_no and settled=0 and resign =0 and s.staff_Code='" + staffcode + "'";
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    txt_studentname.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_name"]);
                    txt_staff_code.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_Code"]);
                    txt_studenttype.Text = Convert.ToString(ds.Tables[0].Rows[0]["staff_type"]);
                    txt_desig.Text = Convert.ToString(ds.Tables[0].Rows[0]["desig_name"]);
                    txt_smart.Text = ds.Tables[0].Rows[i]["Smartcard_serial_no"].ToString();
                    TextBioUserid.Text = ds.Tables[0].Rows[i]["Smartcard_BiouserID"].ToString();
                    ViewState["staff_applid"] = Convert.ToString(ds.Tables[0].Rows[0]["appl_id"]);
                }
                image3.ImageUrl = "../Handler/staffphoto.ashx?staff_code=" + staffcode;
            }
            else
            {
                clear();
            }
        }
        catch
        {
        }
    }
    protected void rdo_stud1_onchecked(object sender, EventArgs e)
    {
        if (rdo_stud1.Checked == true)
        {
            studtbl.Visible = true;
            stafftbl.Visible = false;
        }
        else
        {
            studtbl.Visible = false;
            stafftbl.Visible = true;
        }
    }
    protected void rdo_staff2_onchecked(object sender, EventArgs e)
    {
        Fpspread1.Visible = false; rptprint.Visible = false;
        if (rdo_stud1.Checked == true)
        {
            studtbl.Visible = true;
            stafftbl.Visible = false;
        }
        else
        {
            studtbl.Visible = false;
            stafftbl.Visible = true;
        }
    }
    protected void bindcbldept()
    {
        try
        {
            if (ddl_staffclg.Items.Count > 0)
            {
                ds.Clear();
                string query = "";
                query = "select distinct dept_name,dept_code from hrdept_master where college_code='" + Convert.ToString(ddl_staffclg.SelectedItem.Value) + "'";
                ds = d2.select_method_wo_parameter(query, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_department.DataSource = ds;
                    cbl_department.DataTextField = "dept_name";
                    cbl_department.DataValueField = "dept_code";
                    cbl_department.DataBind();
                    if (cbl_department.Items.Count > 0)
                    {
                        for (int i = 0; i < cbl_department.Items.Count; i++)
                        {
                            cbl_department.Items[i].Selected = true;
                        }
                        txt_department.Text = "Department(" + cbl_department.Items.Count + ")";
                    }
                }
                else
                {
                    txt_department.Text = "--Select--";
                }
            }
        }
        catch { }

    }
    public void cb_department_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            if (cb_department.Checked == true)
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = true;
                }
                txt_department.Text = "Department(" + (cbl_department.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_department.Items.Count; i++)
                {
                    cbl_department.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_department_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_department.Text = "--Select--";
            cb_department.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_department.Items.Count; i++)
            {
                if (cbl_department.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_department.Text = "Department(" + commcount.ToString() + ")";
                if (commcount == cbl_department.Items.Count)
                {
                    cb_department.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void binddesig()
    {
        try
        {
            if (ddl_staffclg.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.binddesi(Convert.ToString(ddl_staffclg.SelectedItem.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_design.DataSource = ds;
                    cbl_design.DataTextField = "desig_name";
                    cbl_design.DataValueField = "desig_code";
                    cbl_design.DataBind();
                    if (cbl_design.Items.Count > 0)
                    {
                        for (int ro = 0; ro < cbl_design.Items.Count; ro++)
                        {
                            cbl_design.Items[ro].Selected = true;
                        }
                        txt_design.Text = "Designation(" + cbl_design.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void cb_desig_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (cb_design.Checked == true)
            {
                for (int i = 0; i < cbl_design.Items.Count; i++)
                {
                    cbl_design.Items[i].Selected = true;
                }
                txt_design.Text = "Designation(" + (cbl_design.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_design.Items.Count; i++)
                {
                    cbl_design.Items[i].Selected = false;
                }
                txt_design.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_desig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_design.Text = "--Select--";
            cb_design.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_design.Items.Count; i++)
            {
                if (cbl_design.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_design.Text = "Designation(" + commcount.ToString() + ")";
                if (commcount == cbl_design.Items.Count)
                {
                    cb_design.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindstafftype()
    {
        try
        {
            if (ddl_staffclg.Items.Count > 0)
            {
                ds.Clear();
                ds = d2.loadstafftype(Convert.ToString(ddl_staffclg.SelectedItem.Value));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbl_stafftype.DataSource = ds;
                    cbl_stafftype.DataTextField = "StfType";
                    cbl_stafftype.DataValueField = "StfType";
                    cbl_stafftype.DataBind();
                    if (cbl_stafftype.Items.Count > 0)
                    {
                        for (int ro = 0; ro < cbl_stafftype.Items.Count; ro++)
                        {
                            cbl_stafftype.Items[ro].Selected = true;
                        }
                        txt_stafftype.Text = "Staff Type(" + cbl_stafftype.Items.Count + ")";
                    }
                }
            }
        }
        catch
        {
        }
    }
    public void cb_stafftype_checkedchange(object sender, EventArgs e)
    {
        try
        {
            if (cb_stafftype.Checked == true)
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = true;
                }
                txt_stafftype.Text = "Staff Type(" + (cbl_stafftype.Items.Count) + ")";
            }
            else
            {
                for (int i = 0; i < cbl_stafftype.Items.Count; i++)
                {
                    cbl_stafftype.Items[i].Selected = false;
                }
                txt_stafftype.Text = "--Select--";
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void cbl_stafftype_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_stafftype.Text = "--Select--";
            cb_stafftype.Checked = false;
            int commcount = 0;
            for (int i = 0; i < cbl_stafftype.Items.Count; i++)
            {
                if (cbl_stafftype.Items[i].Selected == true)
                {
                    commcount = commcount + 1;
                }
            }
            if (commcount > 0)
            {
                txt_stafftype.Text = "Staff Type(" + commcount.ToString() + ")";
                if (commcount == cbl_stafftype.Items.Count)
                {
                    cb_stafftype.Checked = true;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }
}