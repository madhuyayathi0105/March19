using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Configuration;

public partial class HouseAutoGeneration : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DataSet ds1 = new DataSet();
    DAccess2 d2 = new DAccess2();
    ReuasableMethods rs = new ReuasableMethods();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string collegecode1 = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string clgcode = string.Empty;
    Hashtable hat = new Hashtable();
    int i = 0;
    bool GenerateBool = false;
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
                setLabelText();
                bindcollege();
                if (ddl_collegename.Items.Count > 0)
                {
                    clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
                }
                loadstream();
                loadedulevel();
                BindBatch();
                Bindcourse();
                binddept();
            }
        }
        catch (Exception ex) { }
    }
    protected void ddl_collegename_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            clgcode = Convert.ToString(ddl_collegename.SelectedItem.Value);
            loadstream();
            loadedulevel();
            BindBatch();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    protected void type_Change(object sender, EventArgs e)
    {
        try
        {
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    protected void edulevel_SelectedIndexChange(object sender, EventArgs e)
    {
        try
        {
            Bindcourse();
            binddept();
        }
        catch { }
    }
    protected void batch_SelectedIndexChange(object sender, EventArgs e)
    {
    }
    protected void cbdegree_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdegree.Checked == true)
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = true;
                }
                txt_degree.Text = lblDeg.Text + "(" + Convert.ToString(cbldegree.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldegree.Items.Count; i++)
                {
                    cbldegree.Items[i].Selected = false;
                }
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch { }
    }
    protected void cbldegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_degree.Text = "--Select--";
            cbdegree.Checked = false;
            int count = 0;
            for (i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_degree.Text = lblDeg.Text + "(" + count + ")";
                if (count == cbldegree.Items.Count)
                {
                    cbdegree.Checked = true;
                }
            }
            binddept();
        }
        catch { }
    }
    protected void cbdepartment_Changed(object sender, EventArgs e)
    {
        try
        {
            if (cbdepartment1.Checked == true)
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = true;
                }
                txt_department.Text = lblBran.Text + "(" + Convert.ToString(cbldepartment.Items.Count) + ")";
            }
            else
            {
                for (i = 0; i < cbldepartment.Items.Count; i++)
                {
                    cbldepartment.Items[i].Selected = false;
                }
                txt_department.Text = "--Select--";
            }
        }
        catch { }
    }
    protected void cbldepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            txt_department.Text = "--Select--";
            cbdepartment1.Checked = false;
            int count = 0;
            for (i = 0; i < cbldepartment.Items.Count; i++)
            {
                if (cbldepartment.Items[i].Selected == true)
                {
                    count = count + 1;
                }
            }
            if (count > 0)
            {
                txt_department.Text = lblBran.Text + "(" + count + ")";
                if (count == cbldepartment.Items.Count)
                {
                    cbdepartment1.Checked = true;
                }
            }
        }
        catch { }
    }
    public void bindcollege()
    {
        try
        {
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
            hat.Clear();
            hat.Add("column_field", columnfield.ToString());
            ds = d2.select_method("bind_college", hat, "sp");
            ddl_collegename.Items.Clear();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_collegename.Enabled = true;
                ddl_collegename.DataSource = ds;
                ddl_collegename.DataTextField = "collname";
                ddl_collegename.DataValueField = "college_code";
                ddl_collegename.DataBind();
            }
        }
        catch (Exception e) { }
    }
    public void binddept()
    {
        try
        {
            cbldepartment.Items.Clear();
            string build = "";
            string build2 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            build2 = rs.GetSelectedItemsValueAsString(cbldegree);
            if (build != "" && build2 != "")
            {
                string deptquery = "select distinct degree.degree_code,department.dept_name,department.dept_code from degree,department,course,deptprivilages where course.course_id=degree.course_id and  department .dept_code=degree.dept_code and course.college_code = degree.college_code and department.college_code = degree.college_code and degree.course_id in('" + build2 + "') and degree.college_code in ('" + clgcode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldepartment.DataSource = ds;
                    cbldepartment.DataTextField = "dept_name";
                    cbldepartment.DataValueField = "degree_code";
                    cbldepartment.DataBind();
                    if (cbldepartment.Items.Count > 0)
                    {
                        for (i = 0; i < cbldepartment.Items.Count; i++)
                        {
                            cbldepartment.Items[i].Selected = true;
                        }
                        cbdepartment1.Checked = true;
                        txt_department.Text = lblBran.Text + "(" + cbldepartment.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdepartment1.Checked = false;
                txt_department.Text = "--Select--";
            }
        }
        catch (Exception ex) { }
    }
    public void loadstream()
    {
        try
        {
            ddltype.Items.Clear();
            collegecode1 = ddl_collegename.SelectedItem.Value;
            string deptquery = "select distinct type from Course where type is not null and type<>'' and college_code  in ('" + collegecode1 + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddltype.DataSource = ds;
                ddltype.DataTextField = "type";
                ddltype.DataBind();
                ddltype.Enabled = true;
            }
            else
            {
                ddltype.Enabled = false;
            }
            loadedulevel();
            Bindcourse();
            binddept();
        }
        catch { }
    }
    public void loadedulevel()
    {
        try
        {
            ds.Clear();
            ddledulevel.Items.Clear();
            string itemheader = "";
            string deptquery = "";
            if (ddltype.Enabled)
            {
                itemheader = Convert.ToString(ddltype.SelectedItem.Value);
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and type in ('" + itemheader + "') and college_code in ('" + clgcode + "') order by Edu_Level desc";
            }
            else
            {
                deptquery = "select distinct Edu_Level  from Course where Edu_Level is not null and Edu_Level<>'' and college_code in ('" + clgcode + "') order by Edu_Level desc";
            }
            ds.Clear();
            ds = d2.select_method_wo_parameter(deptquery, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddledulevel.DataSource = ds;
                ddledulevel.DataTextField = "Edu_Level";
                ddledulevel.DataBind();
            }
            Bindcourse();
            binddept();
        }
        catch { }
    }
    public void BindBatch()
    {
        try
        {
            ddlbatch.Items.Clear();
            ds = d2.BindBatch();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlbatch.DataSource = ds;
                ddlbatch.DataTextField = "batch_year";
                ddlbatch.DataValueField = "batch_year";
                ddlbatch.DataBind();
            }
        }
        catch { }
    }
    public void Bindcourse()
    {
        try
        {
            cbldegree.Items.Clear();
            string build = "";
            string build1 = "";
            build = Convert.ToString(ddledulevel.SelectedItem.Value);
            if (build != "")
            {
                string deptquery = "select distinct degree.course_id,course.course_name from degree,course,deptprivilages where course.course_id=degree.course_id and course.college_code = degree.college_code and degree.college_code in ('" + clgcode + "') and deptprivilages.Degree_code=degree.Degree_code and user_code in ('" + usercode + "') and course.Edu_Level in ('" + build + "')";
                if (ddltype.Enabled)
                {
                    build1 = Convert.ToString(ddltype.SelectedItem.Value);
                    deptquery = deptquery + " and type in ('" + build1 + "')";
                }
                ds.Clear();
                ds = d2.select_method_wo_parameter(deptquery, "Text");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldegree.DataSource = ds;
                    cbldegree.DataTextField = "course_name";
                    cbldegree.DataValueField = "course_id";
                    cbldegree.DataBind();
                    if (cbldegree.Items.Count > 0)
                    {
                        for (i = 0; i < cbldegree.Items.Count; i++)
                        {
                            cbldegree.Items[i].Selected = true;
                        }
                        cbdegree.Checked = true;
                        txt_degree.Text = lblDeg.Text + "(" + cbldegree.Items.Count + ")";
                    }
                }
            }
            else
            {
                cbdegree.Checked = false;
                txt_degree.Text = "--Select--";
            }
            binddept();
        }
        catch (Exception ex) { }
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
        lbl.Add(lbl_collegename);
        fields.Add(0);
        lbl.Add(lblStr);
        fields.Add(1);
        lbl.Add(lblDeg);
        fields.Add(2);
        lbl.Add(lblBran);
        fields.Add(3);
        //lbl.Add(lbl_org_sem);
        //fields.Add(4);
        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
    protected void btnexcel_Click(object sender, EventArgs e)
    {
        try
        {
            FpSpread.SaveChanges();
            string reportname = txtexcel.Text;
            if (reportname.ToString().Trim() != "")
            {
                txtexcel.Text = "";
                d2.printexcelreport(FpSpread, reportname);
                lblsmserror.Visible = false;
            }
            else
            {
                lblsmserror.Text = "Please Enter Your Report Name";
                lblsmserror.Visible = true;
            }
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        try
        {
            lblsmserror.Text = "";
            txtexcel.Text = "";
            string degreedetails = "Student Housing Auto Generation";
            string pagename = "HouseAutoGeneration.aspx";
            Printcontrol.loadspreaddetails(FpSpread, pagename, degreedetails);
            Printcontrol.Visible = true;
            btnprintmaster.Focus();
        }
        catch { }
    }
    protected void btngo_click(object sender, EventArgs e)
    {
        GenerateBool = false;
        SaveDetails();
    }
    protected void btngenerate_click(object sender, EventArgs e)
    {
        GenerateBool = true;
        SaveDetails();
    }
    protected void SaveDetails()
    {
        try
        {
            string degreecode = rs.GetSelectedItemsValue(cbldepartment);
            string batchyear = Convert.ToString(ddlbatch.SelectedItem.Text);
            string type = Convert.ToString(ddltype.SelectedItem.Text);
            string edulevel = Convert.ToString(ddledulevel.SelectedItem.Text);
            string Query = " select r.App_No,r.roll_no,r.Reg_No,r.Roll_Admit,r.stud_name,case when sex='0' then 'Male' when sex='1' then 'Female' end sex ,r.degree_code,dt.Dept_Name,c.Course_Name ,r.Batch_Year,r.Current_Semester,c.Course_Id,r.sections,h.HostelMasterFK,hm.HostelName,isnull(a.studhouse,'')studhouse from applyn a, degree d,Department dt,Course C,Registration r left join HT_HostelRegistration h on r.App_No=h.APP_No left join hm_hostelmaster hm on h.HostelMasterFK=hm.HostelMasterPK where a.app_no =r.App_No and d.Degree_Code =r.degree_code and d.Dept_Code =dt.Dept_Code and c.Course_Id =d.Course_Id and isconfirm ='1' and c.type ='" + type + "' and c.Edu_Level ='" + edulevel + "' and  a.college_code='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' and r.batch_year in('" + batchyear + "') and d.Degree_Code in ('" + degreecode + "') order by isnull(d.Dept_Priority,10000),r.Reg_No asc";//c.Course_Id,d.Degree_Code,AdmitedDate";
            Query += " select HouseName+'$'+CONVERT(varchar(10), HousePK) as HouseName,Gender,GenderPriority from HousingDetails where collegecode='" + Convert.ToString(ddl_collegename.SelectedItem.Value) + "' order by isnull(HousePriority,0),ISNULL(GenderPriority,0),isnull(OrderBy,0) asc";
            ds.Clear();
            //ds = d2.select_method_wo_parameter(Query, "Text");
            Hashtable HouseDetHash = new Hashtable();
            HouseDetHash.Add("stream", type);
            HouseDetHash.Add("eduLevel", edulevel);
            HouseDetHash.Add("collegeCode", Convert.ToString(ddl_collegename.SelectedItem.Value));
            HouseDetHash.Add("degreecode", degreecode);
            HouseDetHash.Add("batchYear", batchyear);
            ds = d2.select_method("AutohouseGeneration", HouseDetHash, "sp");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                rs.Fpreadheaderbindmethod("S.No-50/Student Name-220/Roll No-150/Reg No-150/Admission No-150/Gender-70/Section-70/" + lblDeg.Text + "-100/" + lblBran.Text + "-200/House Name-180", FpSpread, "FALSE");
                int MaleCurrentRow = 0;
                int FemaleCurrentRow = 0;
                List<String> MaleHousingArr = new List<string>();
                List<String> FemaleHousingArr = new List<string>();
                if (ds.Tables[1].Rows.Count > 0)
                {
                    MaleHousingArr = ds.Tables[1].AsEnumerable().Where(r => r.Field<byte>("Gender") == 0).Select(r => r.Field<String>("HouseName")).ToList<String>();
                    FemaleHousingArr = ds.Tables[1].AsEnumerable().Where(r => r.Field<byte>("Gender") == 1).Select(r => r.Field<String>("HouseName")).ToList<String>();
                }
                //if (MaleHousingArr.Contains(""))
                //    MaleHousingArr.IndexOf("hha");
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    FpSpread.Sheets[0].Rows.Count++;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(FpSpread.Sheets[0].Rows.Count);
                    FpSpread.Sheets[0].Columns[2].Visible = false;
                    FpSpread.Sheets[0].Columns[3].Visible = false;
                    FpSpread.Sheets[0].Columns[4].Visible = false;
                    if (Convert.ToString(Session["Rollflag"]) == "1")
                        FpSpread.Sheets[0].Columns[2].Visible = true;
                    if (Convert.ToString(Session["Regflag"]) == "1")
                        FpSpread.Sheets[0].Columns[3].Visible = true;
                    if (Convert.ToString(Session["Admissionflag"]) == "1")
                        FpSpread.Sheets[0].Columns[4].Visible = true;
                    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dr["App_No"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Note = Convert.ToString(dr["Stud_Type"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dr["stud_name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].CellType = txt;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dr["roll_no"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].CellType = txt;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dr["Reg_No"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].CellType = txt;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dr["Roll_Admit"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dr["sex"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(dr["sections"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Text = Convert.ToString(dr["Course_Name"]);
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Text = Convert.ToString(dr["Dept_Name"]);
                    string studhouse = Convert.ToString(dr["HouseName"]);
                    string studhousePK = Convert.ToString(dr["HousePK"]);
                    string Hostelname = Convert.ToString(dr["HostelName"]);
                    string HousePk = Convert.ToString(dr["HostelMasterFK"]);
                    if (GenerateBool)
                    {
                        if (string.IsNullOrEmpty(Hostelname) && string.IsNullOrEmpty(HousePk) && string.IsNullOrEmpty(studhouse))
                        {
                            #region Auto Allotment
                            if (Convert.ToString(dr["sex"]).ToUpper() == "MALE")
                            {
                                if (MaleHousingArr.Count <= MaleCurrentRow)
                                {
                                    MaleCurrentRow = 0;
                                    string[] houseDet = Convert.ToString(MaleHousingArr[MaleCurrentRow]).Split('$');
                                    Hostelname = Convert.ToString(houseDet[0]);
                                    HousePk = Convert.ToString(houseDet[1]);
                                    MaleCurrentRow++;
                                }
                                else
                                {
                                    string[] houseDet = Convert.ToString(MaleHousingArr[MaleCurrentRow]).Split('$');
                                    Hostelname = Convert.ToString(houseDet[0]);
                                    HousePk = Convert.ToString(houseDet[1]);
                                    MaleCurrentRow++;
                                }
                            }
                            if (Convert.ToString(dr["sex"]).ToUpper() == "FEMALE")
                            {
                                if (FemaleHousingArr.Count <= FemaleCurrentRow)
                                {
                                    FemaleCurrentRow = 0;
                                    string[] houseDet = Convert.ToString(FemaleHousingArr[FemaleCurrentRow]).Split('$');
                                    Hostelname = Convert.ToString(houseDet[0]);
                                    HousePk = Convert.ToString(houseDet[1]);
                                    FemaleCurrentRow++;
                                }
                                else
                                {
                                    string[] houseDet = Convert.ToString(FemaleHousingArr[FemaleCurrentRow]).Split('$');
                                    Hostelname = Convert.ToString(houseDet[0]);
                                    HousePk = Convert.ToString(houseDet[1]);
                                    FemaleCurrentRow++;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(Hostelname))
                            {
                                Hostelname = Convert.ToString(dr["HouseName"]);
                                HousePk = Convert.ToString(dr["HousePK"]);
                                string HouseName = Hostelname + '$' + HousePk;
                                if (Convert.ToString(dr["sex"]).ToUpper() == "MALE")
                                {
                                    if (MaleHousingArr.Count <= MaleCurrentRow)
                                    {
                                        if (MaleHousingArr.Contains(HouseName))
                                        {
                                            //MaleCurrentRow = MaleHousingArr.IndexOf(HouseName);
                                            MaleCurrentRow = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (MaleHousingArr.Contains(HouseName))
                                        {
                                            MaleCurrentRow = MaleHousingArr.IndexOf(HouseName);
                                            MaleCurrentRow++;
                                        }
                                    }
                                }
                                if (Convert.ToString(dr["sex"]).ToUpper() == "FEMALE")
                                {
                                    if (FemaleHousingArr.Count <= FemaleCurrentRow)
                                    {
                                        if (FemaleHousingArr.Contains(HouseName))
                                        {
                                            //FemaleCurrentRow = FemaleHousingArr.IndexOf(HouseName);
                                            FemaleCurrentRow = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (FemaleHousingArr.Contains(HouseName))
                                        {
                                            FemaleCurrentRow = FemaleHousingArr.IndexOf(HouseName);
                                            FemaleCurrentRow++;
                                        }
                                    }
                                }



                                //if (Convert.ToString(dr["sex"]).ToUpper() == "MALE")
                                //{
                                //    if (MaleHousingArr.Count <= MaleCurrentRow)
                                //    {
                                //        MaleCurrentRow = 0;
                                //        Hostelname = Convert.ToString(dr["HouseName"]);
                                //        HousePk = Convert.ToString(dr["HousePK"]);
                                //        MaleCurrentRow++;
                                //    }
                                //    else
                                //    {
                                //        Hostelname = Convert.ToString(dr["HouseName"]);
                                //        HousePk = Convert.ToString(dr["HousePK"]);
                                //        MaleCurrentRow++;
                                //    }
                                //}
                                //if (Convert.ToString(dr["sex"]).ToUpper() == "FEMALE")
                                //{
                                //    if (FemaleHousingArr.Count <= FemaleCurrentRow)
                                //    {
                                //        FemaleCurrentRow = 0;
                                //        Hostelname = Convert.ToString(dr["HouseName"]);
                                //        HousePk = Convert.ToString(dr["HousePK"]);
                                //        FemaleCurrentRow++;
                                //    }
                                //    else
                                //    {
                                //        Hostelname = Convert.ToString(dr["HouseName"]);
                                //        HousePk = Convert.ToString(dr["HousePK"]);
                                //        FemaleCurrentRow++;
                                //    }
                                //}
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Hostelname))
                        {
                            Hostelname = Convert.ToString(dr["HouseName"]);
                            HousePk = Convert.ToString(dr["HousePK"]);
                        }
                    }
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Text = Hostelname;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Tag = HousePk;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Locked = true;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Locked = true;

                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 7].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].HorizontalAlign = HorizontalAlign.Center;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 8].Font.Name = "Book Antiqua";
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].HorizontalAlign = HorizontalAlign.Left;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Size = FontUnit.Medium;
                    FpSpread.Sheets[0].Cells[FpSpread.Sheets[0].RowCount - 1, 9].Font.Name = "Book Antiqua";
                }
                FpSpread.Sheets[0].PageSize = FpSpread.Sheets[0].RowCount;
                FpSpread.Visible = true;
                FpSpread.SaveChanges();
                lbl_error.Visible = false;
                rprint.Visible = true;
            }
            else
            {
                rprint.Visible = false;
                FpSpread.Visible = false;
                lbl_error.Visible = true;
                lbl_error.Text = "No Record Founds";
            }
        }
        catch (Exception ex)
        {
            alertpopwindow.Visible = true;
            lblalerterr.Text = ex.ToString();
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool insertBool = false;
            for (int i = 0; i < FpSpread.Sheets[0].Rows.Count; i++)
            {
                string HouseName = Convert.ToString(FpSpread.Sheets[0].Cells[i, 9].Text);
                string HousePK = Convert.ToString(FpSpread.Sheets[0].Cells[i, 9].Tag);
                string appNo = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Tag);
                string studType = Convert.ToString(FpSpread.Sheets[0].Cells[i, 1].Note);
                if (studType.ToUpper() != "hostler")
                {
                    string Query = " update applyn set studhouse='" + HousePK + "' where app_no='" + appNo + "'";
                    int insertQ = d2.update_method_wo_parameter(Query, "Text");
                    if (insertQ > 0)
                    {
                        insertBool = true;
                    }
                }
            }
            if (insertBool)
            {
                alertpopwindow.Visible = true;
                lblalerterr.Text = "Generated Successfully";
            }
        }
        catch { }
    }
    protected void btnerrclose_Click(object sender, EventArgs e)
    {
        alertpopwindow.Visible = false;
    }


}