using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Configuration;

public partial class CertificationMaster : System.Web.UI.Page
{
    DataSet ds = new DataSet();
    DAccess2 d2 = new DAccess2();
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    static string collegecode = "";
    int i = 0;
    Boolean Cellclick = false;
    ReuasableMethods rs = new ReuasableMethods();
    Boolean checkk = false;
   
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
                setLabelText();
                loadcollegebase();
                if (ddlclg.Items.Count > 0)
                {
                    collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
                }
                educationLevelbase();
                binddegbase();
                binddept();
                CertificatNamebase();
                categorytypebase();
                txt_UDdate.Text = Convert.ToString(DateTime.Now.ToString("dd/MM/yyyy"));
                txt_UDdate.Attributes.Add("readonly", "readonly");
                //  btngo_Click(sender, e);
            }
        }
        catch (Exception ex) { }
    }
   
    protected void lb3_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

    #region Basce Filter

    public void loadcollegebase()
    {
        try
        {
            ddlclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlclg.DataSource = ds;
                ddlclg.DataTextField = "collname";
                ddlclg.DataValueField = "college_code";
                ddlclg.DataBind();
            }
        }
        catch
        { }
    }


    protected void educationLevelbase()
    {
        try
        {
            ddledu.Items.Clear();
            string SelectQ = "select distinct Edu_Level  from course  where  college_code='" + collegecode + "'  order by Edu_Level desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ddledu.DataSource = ds;
                ddledu.DataTextField = "Edu_Level";
                ddledu.DataValueField = "Edu_Level";
                ddledu.DataBind();
            }
        }
        catch { }
    }

    protected void binddegbase()
    {
        try
        {
            cbldegree.Items.Clear();
            string edu = Convert.ToString(ddledu.SelectedItem.Value);
            string selqry = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + ddlclg.SelectedItem.Value + "' and Edu_Level in('" + edu + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
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
                    txtdegree.Text = lblDeg.Text + "(" + cbldegree.Items.Count + ")";
                    cbdegree.Checked = true;
                }
            }

        }
        catch { }
    }

    protected void binddept()
    {
        try
        {
            cbldept.Items.Clear();
            string degree = "";
            for (i = 0; i < cbldegree.Items.Count; i++)
            {
                if (cbldegree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(cbldegree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(cbldegree.Items[i].Value);
                    }
                }
            }
            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cbldept.DataSource = ds;
                    cbldept.DataTextField = "dept_name";
                    cbldept.DataValueField = "degree_code";
                    cbldept.DataBind();
                    if (cbldept.Items.Count > 0)
                    {
                        for (i = 0; i < cbldept.Items.Count; i++)
                        {
                            cbldept.Items[i].Selected = true;
                        }
                        txtdept.Text = "Department(" + cbldept.Items.Count + ")";
                        cbdept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }

    public void CertificatNamebase()
    {
        try
        {
            collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
            cblctf.Items.Clear();
            string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + collegecode + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                cblctf.DataSource = ds;
                cblctf.DataTextField = "MasterValue";
                cblctf.DataValueField = "MasterCode";
                cblctf.DataBind();
                for (int i = 0; i < cblctf.Items.Count; i++)
                {
                    cblctf.Items[i].Selected = true;
                }
                txtctfname.Text = "Certificate Name(" + cblctf.Items.Count + ")";
                cbctf.Checked = true;
            }
            else
            {
                for (int i = 0; i < cblctf.Items.Count; i++)
                {
                    cblctf.Items[i].Selected = true;
                }
                txtctfname.Text = "Certificate Name(" + cblctf.Items.Count + ")";
                cbctf.Checked = true;
            }
        }
        catch (Exception ex)
        { }
    }

    protected DataSet LoadDatasetbase()
    {
        DataSet dsload = new DataSet();
        try
        {
            string clgcode = "";
            if (ddlclg.Items.Count > 0)
            {
                clgcode = Convert.ToString(ddlclg.SelectedItem.Value);
            }
            string edulevel = "";
            if (ddledu.Items.Count > 0)
            {
                edulevel = Convert.ToString(ddledu.SelectedItem.Value);
            }

            string degree = Convert.ToString(getCblSelectedValue(cbldegree));
            string certvalue = Convert.ToString(getCblSelectedValue(cblctf));
            string catgvalue = Convert.ToString(getCblSelectedValue(cbl_catbase));


            string SelectQ = "select (Select MasterValue FROM CO_MasterValues T WHERE CertName = T.MasterCode) as CertName,CertName as certificatid, cm.CourseID,IsStaff,Course_Name,Edu_Level, Categorytype,case when isOrginal=1 then 'Yes' else 'No' end as isOrginal,case when isDuplicate=1 then 'Yes' else 'No' end as isDuplicate,CONVERT(VARCHAR(11),lastdate,103) as lastdate from CertMasterDet cm, Course c where cm.CourseID in('" + degree + "') and cm.CertName in('" + certvalue + "') and cm.CourseID=c.Course_Id and c.college_code='" + ddlclg.SelectedItem.Value + "' and cm.Categorytype in('" + catgvalue + "') order by cm.CourseID,CertName ";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }


    protected void ddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlclg.Items.Count > 0)
        {
            collegecode = Convert.ToString(ddlclg.SelectedItem.Value);
            educationLevelbase();
            binddegbase();
            binddept();
            CertificatNamebase();
            addnewddlclg.SelectedIndex = addnewddlclg.Items.IndexOf(addnewddlclg.Items.FindByValue(collegecode));
            FpSpreadbase.Visible = false;
            div_report.Visible = false;
        }
    }
    protected void ddledu_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddegbase();
        binddept();
    }
    protected void cbdegree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdegree, cbldegree, txtdegree, lblDeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbdegree, cbldegree, txtdegree, lblDeg.Text, "--Select--");
            binddept();
        }
        catch { }
    }
    protected void cbdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbdept, cbldept, txtdept, "Department", "--Select--");

        }
        catch { }
    }
    protected void cbldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbdept, cbldept, txtdept, "Department", "--Select--");

        }
        catch { }
    }
    protected void cbctf_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(cbctf, cblctf, txtctfname, "Certificat Name", "--Select--");

        }
        catch { }
    }
    protected void cblctf_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(cbctf, cblctf, txtctfname, "Certificat Name", "--Select--");

        }
        catch { }
    }

    protected void btnaddnew_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = true;
        loadcollegeadd();
        addnewddlclg.SelectedIndex = addnewddlclg.Items.IndexOf(addnewddlclg.Items.FindByValue(ddlclg.SelectedItem.Value));
        educationLeveladd();
        binddegadd();
        binddeptadd();
        CertificatName();
        categorytype();
        CertificatNamebase();
        FpSpreadadd.Visible = false;
        btnsave.Visible = false;

    }

    protected void btngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = LoadDatasetbase();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            LoadSpreadValue();
        }
        else
        {
            string degree = Convert.ToString(getCblSelectedValue(cbldegree));
            string certvalue = Convert.ToString(getCblSelectedValue(cblctf));
            string catgvalue = Convert.ToString(getCblSelectedValue(cbl_catbase));
            if (degree == "")
            {
                FpSpreadbase.Visible = false;
                div_report.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Degree";
            }
            if (certvalue == "")
            {
                FpSpreadbase.Visible = false;
                div_report.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Certificate Name";
            }
            if (catgvalue == "")
            {
                FpSpreadbase.Visible = false;
                div_report.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Category Type";
            }
            if (degree != "" && certvalue != "" && catgvalue != "")
            {
                FpSpreadbase.Visible = false;
                div_report.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record Found";
                div_report.Visible = false;
            }
            CertificatNamebase();
            categorytypebase();

        }

    }

    protected void LoadSpreadValue()
    {
        try
        {
            #region design

            FpSpreadbase.Sheets[0].RowCount = 0;
            FpSpreadbase.Sheets[0].ColumnCount = 0;
            FpSpreadbase.CommandBar.Visible = false;
            FpSpreadbase.Sheets[0].AutoPostBack = true;
            FpSpreadbase.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadbase.Sheets[0].RowHeader.Visible = false;
            FpSpreadbase.Sheets[0].ColumnCount = 7;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpreadbase.ActiveSheetView.ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            int check = 0;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadbase.Sheets[0].Columns[0].Width = 50;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 1].Text = lblDeg.Text;
            FpSpreadbase.Sheets[0].Columns[1].Width = 70;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Certificate Name";
            FpSpreadbase.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Right;
            FpSpreadbase.Sheets[0].Columns[2].Width = 250;
            FpSpreadbase.Sheets[0].Columns[3].Width = 200;
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Category Type";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Orginal";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Duplicate";
            FpSpreadbase.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Due Date";
            #endregion
            #region value
            div_report.Visible = false;
            FpSpreadbase.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int sel = 0; sel < ds.Tables[0].Rows.Count; sel++)
                {
                    FpSpreadbase.Sheets[0].RowCount++;
                    string name = typeeetext(Convert.ToString(ds.Tables[0].Rows[sel]["Categorytype"]));
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(sel + 1);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(ds.Tables[0].Rows[sel]["Course_Name"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 1].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Font.Name = "Book Antiqua";
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(ds.Tables[0].Rows[sel]["CertName"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["certificatid"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Left;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Text = name;
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 3].Tag = Convert.ToString(ds.Tables[0].Rows[sel]["Categorytype"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(ds.Tables[0].Rows[sel]["isOrginal"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(ds.Tables[0].Rows[sel]["isDuplicate"]);
                    FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].Value = Convert.ToString(ds.Tables[0].Rows[sel]["Lastdate"]);
                    // FpSpreadbase.Sheets[0].Cells[FpSpreadbase.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;

                }
            }
            FpSpreadbase.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpSpreadbase.Sheets[0].PageSize = FpSpreadbase.Sheets[0].RowCount;
            divspread.Visible = true;
            //FpSpreadbase.Height = 250;
            FpSpreadbase.ShowHeaderSelection = false;
            FpSpreadbase.SaveChanges();
            div_report.Visible = true;
            #endregion
        }
        catch { }
    }

    #endregion

    #region Add New Screen

    public void loadcollegeadd()
    {
        try
        {
            addnewddlclg.Items.Clear();
            ds.Clear();
            ds = d2.BindCollege();
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewddlclg.DataSource = ds;
                addnewddlclg.DataTextField = "collname";
                addnewddlclg.DataValueField = "college_code";
                addnewddlclg.DataBind();
            }
        }
        catch
        { }
    }

    protected void educationLeveladd()
    {
        try
        {
            addnewddledu.Items.Clear();
            string SelectQ = "select distinct Edu_Level  from course  where  college_code='" + addnewddlclg.SelectedItem.Value + "'  order by Edu_Level desc";
            ds.Clear();
            ds = d2.select_method_wo_parameter(SelectQ, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                addnewddledu.DataSource = ds;
                addnewddledu.DataTextField = "Edu_Level";
                addnewddledu.DataValueField = "Edu_Level";
                addnewddledu.DataBind();
            }
        }
        catch { }
    }

    protected void binddegadd()
    {
        try
        {
            string edu = Convert.ToString(addnewddledu.SelectedItem.Value);
            addnewcbldegree.Items.Clear();
            string selqry = "select distinct d.Course_Id,c.Course_Name from Degree d,course c where d.Course_Id=c.Course_Id and d.college_code=c.college_code and d.college_code='" + addnewddlclg.SelectedItem.Value + "' and Edu_Level in('" + edu + "')";
            ds.Clear();
            ds = d2.select_method_wo_parameter(selqry, "Text");
            if (ds.Tables[0].Rows.Count > 0)
            {
                addnewcbldegree.DataSource = ds;
                addnewcbldegree.DataTextField = "course_name";
                addnewcbldegree.DataValueField = "course_id";
                addnewcbldegree.DataBind();
                if (addnewcbldegree.Items.Count > 0)
                {
                    for (i = 0; i < addnewcbldegree.Items.Count; i++)
                    {
                        addnewcbldegree.Items[i].Selected = true;
                    }
                    addnewtxtdegree.Text = lblAddDeg.Text + "(" + addnewcbldegree.Items.Count + ")";
                    addnewcbdegree.Checked = true;
                }
            }

        }
        catch { }
    }

    protected void binddeptadd()
    {
        try
        {
            addnewcbldept.Items.Clear();
            string degree = "";
            for (i = 0; i < addnewcbldegree.Items.Count; i++)
            {
                if (addnewcbldegree.Items[i].Selected == true)
                {
                    if (degree == "")
                    {
                        degree = Convert.ToString(addnewcbldegree.Items[i].Value);
                    }
                    else
                    {
                        degree += "," + Convert.ToString(addnewcbldegree.Items[i].Value);
                    }
                }
            }
            if (degree != "")
            {
                ds.Clear();
                ds = d2.BindBranchMultiple(singleuser, group_user, degree, collegecode, usercode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    addnewcbldept.DataSource = ds;
                    addnewcbldept.DataTextField = "dept_name";
                    addnewcbldept.DataValueField = "degree_code";
                    addnewcbldept.DataBind();
                    if (addnewcbldept.Items.Count > 0)
                    {
                        for (i = 0; i < addnewcbldept.Items.Count; i++)
                        {
                            addnewcbldept.Items[i].Selected = true;
                        }
                        addnewtxtdept.Text = "Department(" + addnewcbldept.Items.Count + ")";
                        addnewcbdept.Checked = true;
                    }
                }
            }

        }
        catch { }
    }


    protected void addnewddlclg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (addnewddlclg.Items.Count > 0)
        {
            collegecode = Convert.ToString(addnewddlclg.SelectedItem.Value);
            educationLeveladd();
            binddegadd();
            binddeptadd();
            CertificatName();
        }

    }
    protected void addnewddledu_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        binddegadd();
    }
    protected void addnewcbdegree_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbdegree, addnewcbldegree, addnewtxtdegree, lblAddDeg.Text, "--Select--");
            binddeptadd();
        }
        catch { }
    }
    protected void addnewcbldegree_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbdegree, addnewcbldegree, addnewtxtdegree, lblAddDeg.Text, "--Select--");
            binddeptadd();
        }
        catch { }
    }
    protected void addnewcbdept_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbdept, addnewcbldept, addnewtxtdept, "Department", "--Select--");

        }
        catch { }
    }
    protected void addnewcbldept_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbdept, addnewcbldept, addnewtxtdept, "Department", "--Select--");

        }
        catch { }
    }
    protected void addnewcbctf_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxChange(addnewcbctf, addnewcblctf, addnewtxtctf, "Certificat Name", "--Select--");

        }
        catch { }
    }
    protected void addnewcblctf_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            CallCheckboxListChange(addnewcbctf, addnewcblctf, addnewtxtctf, "Certificat Name", "--Select--");

        }
        catch { }
    }

    protected DataSet LoadDataset()
    {
        DataSet dsload = new DataSet();
        try
        {
            string clgcode = "";
            if (addnewddlclg.Items.Count > 0)
            {
                clgcode = Convert.ToString(addnewddlclg.SelectedItem.Value);
            }
            string edulevel = "";
            if (addnewddledu.Items.Count > 0)
            {
                edulevel = Convert.ToString(addnewddledu.SelectedItem.Value);
            }

            string degree = Convert.ToString(getCblSelectedValue(addnewcbldegree));
            string certvalue = Convert.ToString(getCblSelectedValue(addnewcblctf));

            string SelectQ = "select Course_Name,course_id from Course where Course_Id in('" + degree + "') and college_code='" + clgcode + "' and Edu_Level='" + edulevel + "'";
            dsload.Clear();
            dsload = d2.select_method_wo_parameter(SelectQ, "Text");
        }
        catch { }
        return dsload;
    }

    protected void addnewbtngo_Click(object sender, EventArgs e)
    {
        ds.Clear();
        ds = LoadDataset();
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            LoadAddspread();
        }
        else
        {
            string degree = Convert.ToString(getCblSelectedValue(addnewcbldegree));
            string certvalue = Convert.ToString(getCblSelectedValue(addnewcblctf));
            string catgvalue = Convert.ToString(getCblSelectedValue(cbl_certtype));
            if (degree == "")
            {
                FpSpreadadd.Visible = false;
                btnsave.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Degree";
            }
            if (certvalue == "")
            {
                FpSpreadadd.Visible = false;
                btnsave.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Certificate Name";
            }
            if (catgvalue == "")
            {
                FpSpreadadd.Visible = false;
                btnsave.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Category Type";
            }
            if (degree != "" && certvalue != "" && catgvalue != "")
            {
                imgdiv2.Visible = true;
                btnsave.Visible = false;
                lbl_alert.Visible = true;
                lbl_alert.Text = "No Record Found";
            }
        }
    }
    protected void LoadAddspread()
    {
        try
        {
            string catgvalue = Convert.ToString(getCblSelectedValue(cbl_certtype));
            if (catgvalue == "")
            {
                FpSpreadadd.Visible = false;
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Please Select Any Category Type";
                btnsave.Visible = false;
                return;
            }
            int count = 0;
            int ss = 0;
            Hashtable hasctf = new Hashtable();
            FpSpreadadd.Sheets[0].RowCount = 0;
            FpSpreadadd.Sheets[0].ColumnCount = 0;
            FpSpreadadd.CommandBar.Visible = false;
            FpSpreadadd.Sheets[0].AutoPostBack = false;
            FpSpreadadd.Sheets[0].ColumnHeader.RowCount = 1;
            FpSpreadadd.Sheets[0].RowHeader.Visible = false;
            FpSpreadadd.Sheets[0].ColumnCount = 8;
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            FpSpreadadd.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpSpreadadd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpSpreadadd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            FpSpreadadd.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpSpreadadd.ActiveSheetView.ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            FarPoint.Web.Spread.CheckBoxCellType cb = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cb1 = new FarPoint.Web.Spread.CheckBoxCellType();
            FarPoint.Web.Spread.CheckBoxCellType cball = new FarPoint.Web.Spread.CheckBoxCellType();
            cb.AutoPostBack = false;
            cball.AutoPostBack = true;

            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 0].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadadd.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;

            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 1].Text = "Certificate Name";
            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpSpreadadd.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Left;

            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Category Type";

            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Orginal";
            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Duplicate";
            FpSpreadadd.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Due Date(Day Month Year)";
            FpSpreadadd.Sheets[0].ColumnHeaderSpanModel.Add(0, 5, 1, 3);

            string[] dtday = new string[32];
            for (int id = 1; id < 32; id++)
            {
                string id1 = "";
                int len = Convert.ToString(id).Length;
                if (len == 1)
                {
                    id1 = Convert.ToString("0") + Convert.ToString(id);
                }
                else
                {
                    id1 = Convert.ToString(id);
                }
                dtday[id] = Convert.ToString(id1);
            }
            string[] dtmon = new string[12] { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
            string[] droparray = new string[2];
            string[] loadyear = new string[10] { "2016", "2017", "2018", "2019", "2020", "2021", "2022", "2023", "2024", "2025" };
            string[] droparray1 = new string[cbl_certtype.Items.Count + 1];

            string currentdate = DateTime.Now.ToString("dd/MM/yyyy");
            string[] sp = currentdate.Split('/');

            //dtday[0] = Convert.ToString(sp[0]);
            //dtmon[0] = Convert.ToString(sp[1]);
            if (cbl_certtype.Items.Count > 0)
            {
                for (int re = 0; re < cbl_certtype.Items.Count; re++)
                {
                    if (cbl_certtype.Items[re].Selected == true)
                    {
                        droparray1[re + 1] = Convert.ToString(cbl_certtype.Items[re].Text);

                    }
                }
                droparray1[0] = "Select";
            }
            FarPoint.Web.Spread.ComboBoxCellType cbday1 = new FarPoint.Web.Spread.ComboBoxCellType(dtday);
            cbday1.UseValue = true;
            cbday1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbmon1 = new FarPoint.Web.Spread.ComboBoxCellType(dtmon);
            cbmon1.UseValue = true;
            cbmon1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbyear1 = new FarPoint.Web.Spread.ComboBoxCellType(loadyear);
            cbyear1.UseValue = true;
            cbyear1.ShowButton = true;

            FarPoint.Web.Spread.ComboBoxCellType cbtype = new FarPoint.Web.Spread.ComboBoxCellType(droparray1);
            cbtype.UseValue = true;
            cbtype.ShowButton = true;
            FpSpreadadd.Visible = true;
            string certificatename = "";
            for (int i = 0; i < addnewcblctf.Items.Count; i++)
            {
                if (addnewcblctf.Items[i].Selected == true)
                {
                    ss = 1;
                    certificatename = addnewcblctf.Items[i].Text.ToString();
                    FpSpreadadd.Sheets[0].RowCount++;
                    count++;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].Text = count.ToString();
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].Column.Width = 50;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].Font.Name = "Book Antiqua";
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 0].Locked = true;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].Locked = true;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].Text = addnewcblctf.Items[i].Text;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].Column.Width = 280;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].Tag = addnewcblctf.Items[i].Value;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].VerticalAlign = VerticalAlign.Middle;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 1].Font.Name = "Book Antiqua";
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 2].CellType = cbtype;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 2].Column.Width = 150;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 3].Font.Name = "Book Antiqua";
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 4].Font.Name = "Book Antiqua";
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Center;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 3].CellType = cb;

                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 4].CellType = cb1;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 5].CellType = cbday1;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(sp[0]);
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 5].Column.Width = 40;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 6].CellType = cbmon1;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 6].Text = Convert.ToString(sp[1]);
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 6].Column.Width = 40;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 7].CellType = cbyear1;
                    FpSpreadadd.Sheets[0].Cells[FpSpreadadd.Sheets[0].RowCount - 1, 7].Column.Width = 70;

                }
            }

            if (ss == 1)
            {
                FpSpreadadd.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
                FpSpreadadd.Sheets[0].PageSize = FpSpreadadd.Sheets[0].RowCount;
                FpSpreadadd.SaveChanges();
                divaddspread.Visible = true;
                FpSpreadadd.ShowHeaderSelection = false;
                btnsave.Visible = true;
                FpSpreadadd.Height = 250;
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Kindly Select Certificate Name";
                FpSpreadadd.Visible = false;
                btnsave.Visible = false;
            }
        }
        catch
        { }
    }

    public string categ(string vv)
    {
        string v = "";
        string[] neww = vv.Split(',');
        for (int i = 0; i < neww.Length; i++)
        {

            if (v == "")
            {
                v = neww[i];
            }
            else
            {
                v = v + "," + neww[i];
            }
        }

        return v;
    }

    protected void FpSpreadadd_OnButtonCommand(object sender, FarPoint.Web.Spread.SpreadCommandEventArgs e)
    {
        try
        {
            string actrow = FpSpreadadd.Sheets[0].ActiveRow.ToString();
            string actcol = FpSpreadadd.Sheets[0].ActiveColumn.ToString();
            int colval = Convert.ToInt32(actcol);
            double value = 0;
            if (actrow != "" && actcol != "")
            {
                for (int sel = 0; sel < FpSpreadadd.Sheets[0].Rows.Count; sel++)
                {
                    double.TryParse(Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, colval].Value), out value);
                    if (value == 1)
                    {
                        for (int i = 0; i < FpSpreadadd.Sheets[0].Rows.Count; i++)
                        {
                            FpSpreadadd.Sheets[0].Cells[i, Convert.ToInt32(actcol)].Value = 1;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < FpSpreadadd.Sheets[0].Rows.Count; i++)
                        {
                            FpSpreadadd.Sheets[0].Cells[i, Convert.ToInt32(actcol)].Value = 0;
                        }
                    }
                }
            }
        }
        catch { }
    }

    protected void btnplus_Click(object sender, EventArgs e)
    {
        divcertf.Visible = true;

        lblctfname.Text = "Certificate Name";
    }
    protected void btnminus_Click(object sender, EventArgs e)
    {
        div3.Visible = true;
    }
    protected void btnsavecertf_Click(object sender, EventArgs e)
    {
        try
        {
            string add = "";
            if (txtcertf.Text.Trim() != "")
            {
                string check = d2.GetFunction("select MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + addnewddlclg.SelectedItem.Value + "' and MasterValue='" + txtcertf.Text + "'");
                if (check == "" || check == "0")
                {
                    string clgcode = "";
                    if (addnewddlclg.Items.Count > 0)
                    {
                        clgcode = Convert.ToString(addnewddlclg.SelectedItem.Value);
                    }
                    string name = Convert.ToString(txtcertf.Text);
                    name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(name);
                    if (lblctfname.Text == "Certificate Name")
                    {
                        add = " insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode)  values('" + name + "','CertificateName','" + addnewddlclg.SelectedItem.Value + "')";
                    }
                    else
                    {
                        add = " insert into CO_MasterValues (MasterValue,MasterCriteria,CollegeCode)  values('" + name + "','CertificateCategory','" + addnewddlclg.SelectedItem.Value + "')";
                    }
                    int a = d2.update_method_wo_parameter(add, "Text");
                    if (a > 0)
                    {
                        if (lblctfname.Text == "Certificate Name")
                        {
                            CertificatName();
                            CertificatNamebase();
                        }
                        else
                        {
                            categorytype();
                        }
                        txtcertf.Text = "";
                        Div1.Visible = true;
                        lblerr.Visible = true;
                        lblerr.Text = "Saved Successfully";
                    }
                }
                else
                {
                    Div1.Visible = true;
                    lblerr.Visible = true;
                    lblerr.Text = "Certificate Name Already Exist";
                }

            }
            else
            {
                imgdiv2.Visible = true;
                if (lblctfname.Text == "Certificate Name")
                {
                    lbl_alert.Text = "Please Enter Type of Certification Name";
                }
                else
                {
                    lbl_alert.Text = "Please Enter Type of Category Type";
                }
                lbl_alert.ForeColor = Color.Red;
            }

        }
        catch (Exception ex)
        { }
    }
    public void CertificatName()
    {
        try
        {

            addnewcblctf.Items.Clear();
            string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateName' and CollegeCode='" + addnewddlclg.SelectedItem.Value + "'";
            ds.Clear();
            ds = d2.select_method_wo_parameter(query, "Text");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                addnewcblctf.DataSource = ds;
                addnewcblctf.DataTextField = "MasterValue";
                addnewcblctf.DataValueField = "MasterCode";
                addnewcblctf.DataBind();
                for (int i = 0; i < addnewcblctf.Items.Count; i++)
                {
                    addnewcblctf.Items[i].Selected = true;
                }
                addnewtxtctf.Text = "Certificate Name(" + addnewcblctf.Items.Count + ")";
                addnewcbctf.Checked = true;
            }
            else
            {
                for (int i = 0; i < addnewcblctf.Items.Count; i++)
                {
                    addnewcblctf.Items[i].Selected = true;
                }
                addnewtxtctf.Text = "Certificate Name(" + addnewcblctf.Items.Count + ")";
                addnewcbctf.Checked = true;
            }
        }
        catch (Exception ex)
        { }
    }

    //public void categorytype()
    //{
    //    try
    //    {

    //        cbl_certtype.Items.Clear();
    //        string query = "select distinct MasterCode,MasterValue from CO_MasterValues where MasterCriteria='CertificateCategory' and CollegeCode='" + addnewddlclg.SelectedItem.Value + "'";
    //        ds.Clear();
    //        ds = d2.select_method_wo_parameter(query, "Text");
    //        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
    //        {
    //            cbl_certtype.DataSource = ds;
    //            cbl_certtype.DataTextField = "MasterValue";
    //            cbl_certtype.DataValueField = "MasterCode";
    //            cbl_certtype.DataBind();
    //            for (int i = 0; i < cbl_certtype.Items.Count; i++)
    //            {
    //                cbl_certtype.Items[i].Selected = true;
    //            }
    //            txt_certtype.Text = "Category Type(" + cbl_certtype.Items.Count + ")";
    //            cb_certtype.Checked = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    { }
    //}

    public void categorytype()
    {
        try
        {
            cbl_certtype.Items.Clear();
            string[] reqname = { "Tamil Origin", "Ex-serviceman", "First generation learner", "Sports", "Co-Curricular Activites", "General" };
            for (int i = 0; i < 6; i++)
            {

                cbl_certtype.Items.Add(new ListItem(reqname[i], Convert.ToString(i + 1)));
                for (int ii = 0; ii < cbl_certtype.Items.Count; ii++)
                {
                    cbl_certtype.Items[ii].Selected = true;
                }
                txt_certtype.Text = "Category Type(" + cbl_certtype.Items.Count + ")";
                cb_certtype.Checked = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void categorytypebase()
    {

        try
        {
            cbl_catbase.Items.Clear();
            string[] reqname = { "Tamil Origin", "Ex-serviceman", "First generation learner", "Sports", "Co-Curricular Activites", "General" };
            for (int i = 0; i < 6; i++)
            {
                cbl_catbase.Items.Add(new ListItem(reqname[i], Convert.ToString(i + 1)));


                for (int ii = 0; ii < cbl_catbase.Items.Count; ii++)
                {
                    cbl_catbase.Items[ii].Selected = true;
                }
                txt_catbase.Text = "Category Type(" + cbl_catbase.Items.Count + ")";
                cb_catbase.Checked = true;
            }
        }
        catch (Exception ex)
        { }
    }
    public void categorytypebaseupdate()
    {

        try
        {
            ddl_UDcategorytype.Items.Clear();
            string[] reqname = { "Tamil Origin", "Ex-serviceman", "First generation learner", "Sports", "Co-Curricular Activites", "General" };
            for (int i = 0; i < 6; i++)
            {
                ddl_UDcategorytype.Items.Add(new ListItem(reqname[i], Convert.ToString(i + 1)));
            }
        }
        catch (Exception ex)
        { }
    }
    protected void btnexistcertf_Click(object sender, EventArgs e)
    {
        divcertf.Visible = false;
    }
    protected void btnaddcertf_Click(object sender, EventArgs e)
    {
        divcertf.Visible = false;
    }
    protected void imgaddmew_Click(object sender, EventArgs e)
    {
        divaddnew.Visible = false;
    }

    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {


            int value = 0;
            int isval1 = 0;
            string courseid = "";
            string certname = "";
            bool saveflag = false;
            string day = "";
            string month = "";
            string year = "";
            checkk = false;
            FpSpreadadd.SaveChanges();
            for (int c = 0; c < addnewcbldegree.Items.Count; c++)
            {
                if (addnewcbldegree.Items[c].Selected == true)
                {
                    for (int sel = 0; sel < FpSpreadadd.Sheets[0].Rows.Count; sel++)
                    {
                        string categorytype = "";
                        string date = "";
                        string orginal = "";
                        string duplicate = "";
                        courseid = Convert.ToString(addnewcbldegree.Items[c].Value);
                        categorytype = Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, 2].Value);

                        if (categorytype == "Select")
                        {
                            checkk = true;
                        }
                        categorytype = typeee(categorytype);
                        certname = Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, 1].Tag);
                        isval1 = Convert.ToInt32(FpSpreadadd.Sheets[0].Cells[sel, 3].Value);
                        value = Convert.ToInt32(FpSpreadadd.Sheets[0].Cells[sel, 4].Value);
                        day = Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, 5].Text);
                        month = Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, 6].Text);
                        year = Convert.ToString(FpSpreadadd.Sheets[0].Cells[sel, 7].Text);
                        date = month + "/" + day + "/" + year;
                        string dt = date;
                        string[] Split = dt.Split('/');
                        DateTime todate = Convert.ToDateTime(Split[0] + "/" + Split[1] + "/" + Split[2]);
                        string enddt = DateTime.Now.ToString("dd/MM/yyyy");
                        Split = enddt.Split('/');
                        DateTime fromdate = Convert.ToDateTime(Split[1] + "/" + Split[0] + "/" + Split[2]);

                        if (fromdate > todate)
                        {
                            imgdiv2.Visible = true;
                            lbl_alert.Visible = true;
                            lbl_alert.Text = "Select Valid Date";
                            return;

                        }
                        int ins = 0;
                        if (checkk == false)
                        {
                            string InsertQ = "if exists (select * from CertMasterDet where CourseID='" + courseid + "' and CertName='" + certname + "' and Categorytype='" + categorytype + "')update CertMasterDet set CertName='" + certname + "',isOrginal='" + Convert.ToString(isval1) + "',isDuplicate='" + Convert.ToString(value) + "',LastDate='" + date + "',Categorytype='" + categorytype + "' where CourseID='" + courseid + "' and CertName='" + certname + "' and LastDate='" + date + "' else insert into CertMasterDet(CourseID,CertName,IsStaff,isOrginal,isDuplicate,LastDate,Categorytype)values('" + courseid + "','" + certname + "','0','" + Convert.ToString(isval1) + "','" + Convert.ToString(value) + "','" + date + "','" + categorytype + "')";
                            ins = d2.update_method_wo_parameter(InsertQ, "Text");
                        }
                        else
                        {

                            saveflag = false;
                        }
                        if (ins > 0)
                        {
                            saveflag = true;
                        }
                    }
                }
            }

            if (saveflag == true)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Saved Successfully";

            }
            else
            {
                if (checkk == true)
                {
                    imgdiv2.Visible = true;
                    lbl_alert.Visible = true;
                    lbl_alert.Text = "Please Select Any Category";
                }
            }
            addnewbtngo_Click(sender, e);
        }

        catch { }
    }

    public string typeee(string v)
    {
        string val = "";
        if (v == "Tamil Origin")
        {
            val = "1";
        }
        else if (v == "Ex-serviceman")
        {
            val = "2";
        }
        else if (v == "First generation learner")
        {
            val = "3";
        }
        else if (v == "Sports")
        {
            val = "4";
        }
        else if (v == "Co-Curricular Activites")
        {
            val = "5";
        }
        else if (v == "General")
        {
            val = "6";
        }
        return val;
    }
    public string typeeetext(string v)
    {
        string val = "";
        if (v == "1")
        {
            val = "Tamil Origin";
        }
        else if (v == "2")
        {
            val = "Ex-serviceman";
        }
        else if (v == "3")
        {
            val = "First generation learner";
        }
        else if (v == "4")
        {
            val = "Sports";
        }
        else if (v == "5")
        {
            val = "Co-Curricular Activites";
        }
        else if (v == "6")
        {
            val = "General";
        }
        return val;
    }

    #endregion

    public void btn_printmaster_Click(object sender, EventArgs e)
    {
        try
        {
            string attendance = "Certification  Master Report";
            string pagename = "CertificationMaster.aspx";
            Printcontrol.loadspreaddetails(FpSpreadbase, pagename, attendance);
            Printcontrol.Visible = true;
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
                d2.printexcelreport(FpSpreadbase, report);
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
        catch (Exception ex)
        {

        }
    }
    
    #region Common Checkbox and Checkboxlist Event

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
    private string getCblSelectedText(CheckBoxList cblSelected)
    {
        System.Text.StringBuilder selectedText = new System.Text.StringBuilder();
        try
        {
            for (int sel = 0; sel < cblSelected.Items.Count; sel++)
            {
                if (cblSelected.Items[sel].Selected == true)
                {
                    if (selectedText.Length == 0)
                    {
                        selectedText.Append(Convert.ToString(cblSelected.Items[sel].Text));
                    }
                    else
                    {
                        selectedText.Append("','" + Convert.ToString(cblSelected.Items[sel].Text));
                    }
                }
            }
        }
        catch { cblSelected.Items.Clear(); }
        return selectedText.ToString();
    }
    private void CallCheckboxChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dispst, string deft)
    {
        try
        {
            int sel = 0;
            string name = "";
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
    private void CallCheckboxListChange(CheckBox cb, CheckBoxList cbl, TextBox txt, string dipst, string deft)
    {
        try
        {
            int sel = 0;
            int count = 0;
            string name = "";
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

    #endregion

    public void btn_errorclose_Click(object sender, EventArgs e)
    {
        imgdiv2.Visible = false;
        fpreadbutton();
        loadcollegeadd();
        educationLeveladd();
        binddegadd();
        binddeptadd();
        CertificatName();
        categorytype();
        CertificatNamebase();

    }

    protected void fpreadbutton()
    {

    }

    protected void btnclose_Click(object sender, EventArgs e)
    {
        CertificatName();
        categorytype();
        divcertf.Visible = false;
        Div1.Visible = false;
        CertificatNamebase();
    }

    // zzz 11.5.2016

    public void btn_plusnew_Click(object sender, EventArgs e)
    {
        divcertf.Visible = true;
        lblctfname.Text = "Category Type";
    }
    
    public void cb_certtype_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_certtype, cbl_certtype, txt_certtype, "Category Type", "--Select--");

    }
    
    public void cb_certtype_OnCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_certtype, cbl_certtype, txt_certtype, "Category Type", "--Select--");

    }
    
    public void cb_catbaseCheckedChanged(object sender, EventArgs e)
    {
        CallCheckboxChange(cb_catbase, cbl_catbase, txt_catbase, "Category Type", "--Select--");
    }
    
    public void cbl_catbase_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        CallCheckboxListChange(cb_catbase, cbl_catbase, txt_catbase, "Category Type", "--Select--");
    }
    
    public void btn_minusnew_Click(object sender, EventArgs e)
    {
        bool delflag = false;
        for (int i = 0; i < cbl_certtype.Items.Count; i++)
        {
            if (cbl_certtype.Items[i].Selected == true)
            {
                string text = cbl_certtype.Items[i].Text;
                string value = cbl_certtype.Items[i].Value;
                string deleteQ = "delete from CO_MasterValues where MasterCode='" + value + "' and  MasterValue='" + text + "' and MasterCriteria='CertificateCategory'";
                int ins = d2.update_method_wo_parameter(deleteQ, "Text");
                delflag = true;
            }
        }
        if (delflag == true)
        {
            categorytype();
            Div1.Visible = true;
            lblerr.Visible = true;
            lblerr.Text = "Deleted Successfully";
        }
        else
        {
            Div1.Visible = true;
            lblerr.Visible = true;
            lblerr.Text = "Please Select Any One Certificate";
        }
    }

    public void btn_cnfmok_Click(object sender, EventArgs e)
    {
        bool delflag = false;
        div3.Visible = false;
        for (int i = 0; i < addnewcblctf.Items.Count; i++)
        {
            if (addnewcblctf.Items[i].Selected == true)
            {
                string text = addnewcblctf.Items[i].Text;
                string value = addnewcblctf.Items[i].Value;
                string check = d2.GetFunction("select MasterCode from StudCertDetails_New s,CO_MasterValues c where c.MasterCode=s.CertificateId and MasterCode='" + value + "'");
                if (check == "0")
                {

                    string deleteQ = "delete from CO_MasterValues where MasterCode='" + value + "' and  MasterValue='" + text + "' and MasterCriteria='CertificateName'";
                    int ins = d2.update_method_wo_parameter(deleteQ, "Text");
                    delflag = true;
                }
                else
                {
                    Div1.Visible = true;
                    lblerr.Visible = true;
                    lblerr.Text = "Alredy Some Students Submit the" + text + " Please Change The Students Records After That Procced ";
                }
            }
        }
        if (delflag == true)
        {
            CertificatName();
            Div1.Visible = true;
            lblerr.Visible = true;
            lblerr.Text = "Deleted Successfully";
        }
        else
        {
            Div1.Visible = true;
            lblerr.Visible = true;
            lblerr.Text = "Please Select Any One Certificate";
        }
        CertificatName();
        CertificatNamebase();
    }
    
    public void btn_cnfromcancel_click(object sender, EventArgs e)
    {
        div3.Visible = false;
    }
    
    public void FpSpreadbase_CellClick(object sender, EventArgs e)
    {
        Cellclick = true;
    }
    
    public void FpSpreadbase_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cellclick == true)
            {
                categorytypebaseupdate();
                string activerow = "";
                string activecol = "";
                activerow = FpSpreadbase.ActiveSheetView.ActiveRow.ToString();
                activecol = FpSpreadbase.ActiveSheetView.ActiveColumn.ToString();
                string certificatid = Convert.ToString(FpSpreadbase.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
                txt_UDcertificatename.Text = Convert.ToString(FpSpreadbase.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Text);
                popview.Visible = true;
            }
        }
        catch
        {
        }
    }
    
    public void btn_popclose_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }
    
    public void btn_UDexit_Click(object sender, EventArgs e)
    {
        popview.Visible = false;
    }
    
    public DateTime TextToDate(TextBox txt)
    {
        DateTime dt = new DateTime();
        string firstdate = Convert.ToString(txt.Text);

        string[] split = firstdate.Split('/');
        dt = Convert.ToDateTime(split[1] + "/" + split[0] + "/" + split[2]);
        return dt;
    }
    
    public void btn_UDupdate_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            string original = "";
            string duplicate = "";
            DateTime updatedate = new DateTime();
            updatedate = TextToDate(txt_UDdate);
            activerow = FpSpreadbase.ActiveSheetView.ActiveRow.ToString();
            string certificatid = Convert.ToString(FpSpreadbase.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            string categorytype = Convert.ToString(FpSpreadbase.Sheets[0].Cells[Convert.ToInt32(activerow), 3].Tag);
            if (cb_UDorginal.Checked == true)
            {
                original = "1";
            }
            else
            {
                original = "0";
            }
            if (cb_UDorginal.Checked == true)
            {
                duplicate = "1";
            }
            else
            {
                duplicate = "0";
            }
            string update_query = " if exists (select * from CertMasterDet where CertName='" + certificatid + "' and Categorytype='" + categorytype + "') update CertMasterDet set CertName='" + certificatid + "',Categorytype='" + ddl_UDcategorytype.SelectedItem.Value + "',isOrginal='" + original + "',isDuplicate='" + duplicate + "',LastDate='" + updatedate + "' where CertName='" + certificatid + "'";
            int s = d2.update_method_wo_parameter(update_query, "text");
            if (s != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Updated Successfully";
                btngo_Click(sender, e);
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Not Update";
            }
        }
        catch
        {
        }
    }
    
    public void btn_UDdelete_Click(object sender, EventArgs e)
    {
        try
        {
            string activerow = "";
            activerow = FpSpreadbase.ActiveSheetView.ActiveRow.ToString();
            string certificatid = Convert.ToString(FpSpreadbase.Sheets[0].Cells[Convert.ToInt32(activerow), 2].Tag);
            string del_query = " delete from CertMasterDet where CertName='" + certificatid + "'";
            int s = d2.update_method_wo_parameter(del_query, "text");
            if (s != 0)
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Deleted Successfully";
            }
            else
            {
                imgdiv2.Visible = true;
                lbl_alert.Visible = true;
                lbl_alert.Text = "Not Deleted";
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
        lbl.Add(Label1);
        fields.Add(0);

        lbl.Add(lblAddDeg);
        fields.Add(2);

        lbl.Add(lblclg);
        fields.Add(0);

        lbl.Add(lblDeg);
        fields.Add(2);

        //Name -0, Stream - 1 ,Degree - 2, Branch - 3, Term - 4
        new HeaderLabelText().setLabels(grouporusercode, ref lbl, fields);
    }
}