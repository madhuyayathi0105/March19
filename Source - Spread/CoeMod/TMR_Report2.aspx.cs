using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using FarPoint.Web.Spread;
using System.Configuration;

public partial class TMR_Report2 : System.Web.UI.Page
{
    DataSet ds_load = new DataSet();
    DataSet dsMaster = new DataSet();
    DAccess2 daccess = new DAccess2();
    DAccess2 d2 = new DAccess2();
    Hashtable hat = new Hashtable();

    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            img1.ImageUrl = this.ImageUrl;
            img1.Width = Unit.Percentage(100);
            img1.Height = Unit.Percentage(70);
            return img1;
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl;
            img.Width = Unit.Percentage(105);
            img.Height = Unit.Percentage(70);
            return img;
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl;
            img2.Width = Unit.Percentage(100);
            img2.Height = Unit.Percentage(70);
            return img2;
        }
    }

    string grade_setting = string.Empty;
    string collnamenew1 = string.Empty;
    string address1 = string.Empty;
    string address2 = string.Empty;
    string address3 = string.Empty;
    string pincode = string.Empty;
    string categery = string.Empty;
    string Affliated = string.Empty;
    string address = string.Empty;
    string Phoneno = string.Empty;
    string Faxno = string.Empty;
    string phnfax = string.Empty;
    string district = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string strsec = string.Empty;
    string sections = string.Empty;
    string funcgrade = string.Empty;
    string mark = string.Empty;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string fromdate = string.Empty;
    string degree_code = string.Empty;
    string current_sem = string.Empty;
    string batch_year = string.Empty;
    string getgradeflag = string.Empty;
    string exam_month = string.Empty;
    string exam_year = string.Empty;
    string getsubno = string.Empty;
    string getsubtype = string.Empty;
    string strmnthyear = string.Empty;
    string strexam = string.Empty;
    string grade = string.Empty;
    string funcsubno = string.Empty;
    string funcsubname = string.Empty;
    string funcsubcode = string.Empty;
    string funcresult = string.Empty;
    string funcsemester = string.Empty;
    string funccredit = string.Empty;
    string EarnedVal = string.Empty;
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;
    string section = string.Empty;
    string attmtreal = string.Empty;
    string qry = string.Empty;
    int overallcredit = 0;
    int rcnt;
    int ExamCode = 0;
    int serialno = 0;
    int subjectcount = 0;
    int semdec = 0;
    int IntExamCode = 0;
    int column_count = 0;
    int cou = 0;
    int noof_subcode = 0;
    int find_subjrow_count = 0;
    int attmpt = 0;
    int maxmarkve = 0;
    bool markflag = false;
    bool flagvetri = true;
    bool InsFlag;
    bool flag;
    double cgpa2 = 0;
    double exte = 0;
    double inte = 0;
    double itrnl = 0;
    double extrnl = 0;
    static ArrayList ItemList = new ArrayList();
    static ArrayList Itemindex = new ArrayList();
    String ccqry = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //****************************************************//
            if (Session["collegecode"] == null)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!Request.FilePath.Contains("CoeHome"))
            {
                string strPreviousPage = "";
                if (Request.UrlReferrer != null)
                {
                    strPreviousPage = Request.UrlReferrer.Segments[Request.UrlReferrer.Segments.Length - 1];
                }
                if (strPreviousPage == "")
                {
                    Response.Redirect("~/CoeMod/CoeHome.aspx");
                    return;
                }
            }
            //****************************************************//
            collegecode = Convert.ToString(Session["collegecode"]).Trim();
            usercode = Convert.ToString(Session["usercode"]).Trim();
            singleuser = Convert.ToString(Session["single_user"]).Trim();
            group_user = Convert.ToString(Session["group_code"]).Trim();
            lblnorec.Visible = false;
            lblError.Visible = false;
            if (!IsPostBack)
            {
                btnprintmaster.Visible = false;
                btnPrint.Visible = false;
                pnlSpread.Visible = false;
                Printcontrol.Visible = false;
                chkShowValuationMarks.Checked = false;
                chkShowsSectionWise.Checked = false;
                chkShowNoteDescription.Checked = true;
                chkIncludeNotRegistered.Checked = true;
                chkIncludeRedoSuspended.Checked = true;
                chkIncludePassedOut.Checked = false;
                divRedo.Visible = false;
                divFailValue.Visible = false;
                txtFailValue.Text = string.Empty;
                txtCollegeHeader.Text = string.Empty;
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                txtOrder.Visible = false;
                chkColumnOrderAll.Checked = false;
                string value = string.Empty;
                int index;
                value = string.Empty;
                //ccqry = "and cc='0'";
                if (chkShowValuationMarks.Checked)
                {
                    if (rblOfficeDeptCopy.Items.Count > 0)
                    {
                        rblOfficeDeptCopy.Items[0].Selected = true;
                        rblOfficeDeptCopy.Items[1].Selected = false;
                        rblOfficeDeptCopy.Items[2].Selected = false;
                    }
                }
                else
                {
                    if (rblOfficeDeptCopy.Items.Count > 0)
                    {
                        rblOfficeDeptCopy.Items[0].Selected = false;
                        rblOfficeDeptCopy.Items[1].Selected = false;
                        rblOfficeDeptCopy.Items[2].Selected = true;
                    }
                }
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    liOrder.Selected = true;
                    liOrder.Enabled = true;
                    switch (liValue)
                    {
                        case "5":
                        case "10":
                        case "13":
                        case "14":
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            break;
                        case "6":
                            if (chkshowsub_name.Checked)
                                liOrder.Text = "Subject Name";
                            else
                                liOrder.Text = "Subject Code";
                            break;
                        default:
                            liOrder.Selected = true;
                            liOrder.Enabled = true;
                            break;
                    }
                    if (liOrder.Selected == false)
                    {
                        ItemList.Remove(liOrder.Text);
                        Itemindex.Remove(Convert.ToString(liOrder.Value));
                    }
                    else
                    {
                        if (!Itemindex.Contains(liOrder.Value))
                        {
                            ItemList.Add(liOrder.Text);
                            Itemindex.Add(liOrder.Value);
                        }
                    }
                }
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    if (txtOrder.Text == "")
                    {
                        txtOrder.Text = ItemList[i].ToString();
                    }
                    else
                    {
                        txtOrder.Text = txtOrder.Text + "," + ItemList[i].ToString();
                    }
                }
                if (ItemList.Count == cblColumnOrder.Items.Count)
                {
                    chkColumnOrderAll.Checked = true;
                }
                if (ItemList.Count > 0)
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = true;
                }
                else
                {
                    txtOrder.Visible = false;
                    lbtnRemoveAll.Visible = false;
                }
                collegecode = Convert.ToString(Session["collegecode"]).Trim();
                usercode = Convert.ToString(Session["usercode"]).Trim();
                singleuser = Convert.ToString(Session["single_user"]).Trim();
                group_user = Convert.ToString(Session["group_code"]).Trim();
                bindbatch();
                binddegree();
                bindbranch();
                bindsem();
                bindsec();
                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.ForeColor = Color.Black;
                style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
                style.HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].AllowTableCorner = true;
                FpExternal.Sheets[0].SheetCorner.Cells[0, 0].Text = " S.No ";
                FpExternal.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                string getbranch = ddlBranch.Text.ToString();
                FpExternal.Visible = false;
                FpExternalHeader.Visible = false;
                if (Session["usercode"] != "")
                {
                    Master = "select * from Master_Settings where usercode='" + Convert.ToString(Session["usercode"]).Trim() + "'";
                    dsMaster.Clear();
                    dsMaster.Reset();
                    dsMaster = d2.select_method_wo_parameter(Master, "Text");
                    Session["strvar"] = string.Empty;
                    Session["Rollflag"] = "0";
                    Session["Regflag"] = "0";
                    Session["Studflag"] = "0";
                    if (dsMaster.Tables.Count > 0 && dsMaster.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow mtrdr in dsMaster.Tables[0].Rows)
                        {
                            if (mtrdr["settings"].ToString() == "Roll No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Rollflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Register No" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Regflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Student_Type" && mtrdr["value"].ToString() == "1")
                            {
                                Session["Studflag"] = "1";
                            }
                            if (mtrdr["settings"].ToString() == "Days Scholor" && mtrdr["value"].ToString() == "1")
                            {
                                strdayflag = " and (registration.Stud_Type='Day Scholar'";
                            }
                            if (mtrdr["settings"].ToString() == "Hostel" && mtrdr["value"].ToString() == "1")
                            {
                                if (strdayflag != "" && strdayflag != "\0")
                                {
                                    strdayflag = strdayflag + " or registration.Stud_Type='Hostler'";
                                }
                                else
                                {
                                    strdayflag = " and (registration.Stud_Type='Hostler'";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Regular")
                            {
                                regularflag = "and ((registration.mode=1)";
                            }
                            if (mtrdr["settings"].ToString() == "Lateral")
                            {
                                if (regularflag != "")
                                {
                                    regularflag = regularflag + " or (registration.mode=3)";
                                }
                                else
                                {
                                    regularflag = regularflag + " and ((registration.mode=3)";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Transfer")
                            {
                                if (regularflag != "")
                                {
                                    regularflag = regularflag + " or (registration.mode=2)";
                                }
                                else
                                {
                                    regularflag = regularflag + " and ((registration.mode=2)";
                                }
                            }
                            if (mtrdr["settings"].ToString() == "Male" && mtrdr["value"].ToString() == "1")
                            {
                                genderflag = " and (sex='0'";
                            }
                            if (mtrdr["settings"].ToString() == "Female" && mtrdr["value"].ToString() == "1")
                            {
                                if (genderflag != "" && genderflag != "\0")
                                {
                                    genderflag = genderflag + " or sex='1'";
                                }
                                else
                                {
                                    genderflag = " and (sex='1'";
                                }
                            }
                        }
                    }
                    if (strdayflag != "")
                    {
                        strdayflag = strdayflag + ")";
                    }
                    Session["strvar"] = strdayflag;
                    if (regularflag != "")
                    {
                        regularflag = regularflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag;
                    if (genderflag != "")
                    {
                        genderflag = genderflag + ")";
                    }
                    Session["strvar"] = Session["strvar"] + regularflag + genderflag;
                    rb1.Checked = true;
                }
                ddlMonth.Items.Clear();
                ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                int year = Convert.ToInt16(DateTime.Now.ToString("yyyy"));
                ddlYear.Items.Clear();
                for (int l = 0; l <= 20; l++)
                {
                    ddlYear.Items.Add(Convert.ToString(year - l));
                }
                txtDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                chksubjtype.Items[0].Selected = true;
                chksubjtype.Items[1].Selected = true;
            }
        }
        catch(Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        ds_load.Clear();
        ds_load = daccess.BindBatch();
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count = ds_load.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds_load;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
            }
        }
    }

    public void bindbranch()
    {
        ddlBranch.Items.Clear();
        hat.Clear();
        usercode = Convert.ToString(Session["usercode"]).Trim();
        collegecode = Convert.ToString(Session["collegecode"]).Trim();
        singleuser = Convert.ToString(Session["single_user"]).Trim();
        group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]).Trim();
        }
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load.Clear();
        ds_load = daccess.select_method("bind_branch", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count2 = ds_load.Tables[0].Rows.Count;
            if (count2 > 0)
            {
                ddlBranch.DataSource = ds_load;
                ddlBranch.DataTextField = "dept_name";
                ddlBranch.DataValueField = "degree_code";
                ddlBranch.DataBind();
            }
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
        usercode = Convert.ToString(Session["usercode"]).Trim();
        collegecode = Convert.ToString(Session["collegecode"]).Trim();
        singleuser = Convert.ToString(Session["single_user"]).Trim();
        group_user = Convert.ToString(Session["group_code"]).Trim();
        if (group_user.Contains(';'))
        {
            string[] group_semi = group_user.Split(';');
            group_user = Convert.ToString(group_semi[0]).Trim();
        }
        hat.Clear();
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load.Clear();
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count1 = ds_load.Tables[0].Rows.Count;
            if (count1 > 0)
            {
                ddlDegree.DataSource = ds_load;
                ddlDegree.DataTextField = "course_name";
                ddlDegree.DataValueField = "course_id";
                ddlDegree.DataBind();
            }
        }
    }

    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load.Clear();
        ds_load = daccess.select_method("bind_sec", hat, "sp");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            int count5 = ds_load.Tables[0].Rows.Count;
            if (count5 > 0)
            {
                ddlSec.DataSource = ds_load;
                ddlSec.DataTextField = "sections";
                ddlSec.DataValueField = "sections";
                ddlSec.DataBind();
                ddlSec.Items.Insert(0, "All");
                ddlSec.Enabled = true;
            }
            else
            {
                ddlSec.Enabled = false;
            }
        }
        else
        {
            ddlSec.Enabled = false;
        }
    }

    public void bindsem()
    {
        ddlSemYr.Items.Clear();
        Boolean first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        qry = "select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
        ds_load.Clear();
        ds_load = d2.select_method_wo_parameter(qry, "Text");
        if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds_load.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds_load.Tables[0].Rows[0][0].ToString());
            for (i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
            }
        }
        else
        {
            qry = "select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "";
            ds_load.Clear();
            ds_load = d2.select_method_wo_parameter(qry, "Text");
            if (ds_load.Tables.Count > 0 && ds_load.Tables[0].Rows.Count > 0)
            {
                first_year = Convert.ToBoolean(ds_load.Tables[0].Rows[0][1].ToString());
                duration = Convert.ToInt16(ds_load.Tables[0].Rows[0][0].ToString());
                for (i = 1; i <= duration; i++)
                {
                    if (first_year == false)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                    else if (first_year == true && i != 2)
                    {
                        ddlSemYr.Items.Add(i.ToString());
                    }
                }
            }
        }
    }

    public void Get_Semester()
    {
        Boolean first_year;
        first_year = false;
        ddlSemYr.Items.Clear();
        int duration = 0;
        string batch_calcode_degree;
        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Convert.ToString(Session["collegecode"]).Trim();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            first_year = Convert.ToBoolean(ds.Tables[0].Rows[0][1].ToString());
            duration = Convert.ToInt16(ds.Tables[0].Rows[0][0].ToString());
            for (int i = 1; i <= duration; i++)
            {
                if (first_year == false)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
                else if (first_year == true && i != 2)
                {
                    ddlSemYr.Items.Add(i.ToString());
                }
            }
        }
    }

    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            binddegree();
            bindbranch();
            bindsem();
            bindsec();
            bind_arrear_sem();
        }
        ddlSec.SelectedIndex = -1;
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        ddlBranch.Items.Clear();
        string course_id = ddlDegree.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
        bindbranch();
        bindsem();
        bindsec();
        bind_arrear_sem();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        if (!Page.IsPostBack == false)
        {
        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                bindsem();
                bindsec();
                bind_arrear_sem();
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        bindsec();
        bind_arrear_sem();
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        bind_arrear_sem();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxother.Text != "")
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt16(TextBoxother.Text.ToString());
                CalculateTotalPages();
            }
        }
        catch
        {
            TextBoxother.Text = string.Empty;
        }
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (TextBoxpage.Text.Trim() != "")
            {
                if (Convert.ToInt16(TextBoxpage.Text) > Convert.ToInt16(Session["totalPages"]))
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Exceed The Page Limit";
                    FpExternal.Visible = true; FpExternalHeader.Visible = true;
                    TextBoxpage.Text = string.Empty;
                }
                else if (Convert.ToInt32(TextBoxpage.Text) == 0)
                {
                    LabelE.Visible = true;
                    LabelE.Text = "Search should be greater than zero";
                    TextBoxpage.Text = string.Empty;
                }
                else
                {
                    LabelE.Visible = false;
                    FpExternal.CurrentPage = Convert.ToInt16(TextBoxpage.Text) - 1;
                    FpExternal.Visible = true; FpExternalHeader.Visible = true;
                }
            }
        }
        catch
        {
            TextBoxpage.Text = string.Empty;
        }
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        TextBoxother.Text = string.Empty;
        if (DropDownListpage.Text == "Others")
        {
            LabelE.Visible = false;
            TextBoxother.Visible = true;
            TextBoxother.Focus();
        }
        else
        {
            LabelE.Visible = false;
            TextBoxother.Visible = false;
            FpExternal.Visible = true; FpExternalHeader.Visible = true;
            FpExternal.Sheets[0].PageSize = Convert.ToInt16(DropDownListpage.Text.ToString());
            CalculateTotalPages();
        }
        FpExternal.CurrentPage = 0;
    }

    protected void txtsubjtype_TextChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        bind_arrear_sem();
    }

    protected void txtarrear_sem_TextChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
    }

    protected void chksubjtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
        bind_arrear_sem();
        string code = string.Empty;
        string text = string.Empty;
        for (int i = 0; i < chksubjtype.Items.Count; i++)
        {
            if (chksubjtype.Items[i].Selected == true)
            {
                if (text == "")
                {
                    code = chksubjtype.Items[i].Value;
                    text = chksubjtype.Items[i].Text;
                    txtsubjtype.Text = text;
                }
                else
                {
                    code = code + "," + chksubjtype.Items[i].Value;
                    txtsubjtype.Text = text + "," + chksubjtype.Items[i].Text;
                }
            }
            else if (chksubjtype.Items[i].Selected != true)
            {
                txtsubjtype.Text = text + string.Empty;
            }
        }
    }

    protected void chkarrear_Sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        FpExternal.Visible = false;
        btnprintmaster.Visible = false;
        btnPrint.Visible = false;
        Printcontrol.Visible = false;
        pnlSpread.Visible = false;
    }

    public void bind_arrear_sem()
    {
        chkarrear_Sem.Items.Clear();
        if (chksubjtype.Items[0].Selected == true && chksubjtype.Items[1].Selected == true)
        {
            for (int arr_sem = 1; arr_sem <= Convert.ToInt16(ddlSemYr.SelectedItem.ToString()); arr_sem++)
            {
                chkarrear_Sem.Items.Add(arr_sem.ToString());
                chkarrear_Sem.Items[(arr_sem - 1)].Selected = true;
            }
            chkarrear_Sem.Visible = true;
            pnlarrear_Sem.Visible = true;
            lblarrear_sem.Visible = true;
            txtarrear_sem.Visible = true;
        }
        else if (chksubjtype.Items[1].Selected == true)
        {
            for (int arr_sem = 1; arr_sem <= Convert.ToInt16(ddlSemYr.SelectedItem.ToString()); arr_sem++)
            {
                chkarrear_Sem.Items.Add(arr_sem.ToString());
                chkarrear_Sem.Items[(arr_sem - 1)].Selected = true;
            }
            chkarrear_Sem.Visible = true;
            pnlarrear_Sem.Visible = true;
            lblarrear_sem.Visible = true;
            txtarrear_sem.Visible = true;
        }
        else if (chksubjtype.Items[0].Selected == true)
        {
            chkarrear_Sem.Visible = false;
            pnlarrear_Sem.Visible = false;
            lblarrear_sem.Visible = false;
            txtarrear_sem.Visible = false;
        }
        else if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected != true)
        {
            chkarrear_Sem.Visible = false;
            pnlarrear_Sem.Visible = false;
            lblarrear_sem.Visible = false;
            txtarrear_sem.Visible = false;
        }
        string code = string.Empty;
        string text = string.Empty;
        for (int i = 0; i < chksubjtype.Items.Count; i++)
        {
            if (chksubjtype.Items[i].Selected == true)
            {
                if (text == "")
                {
                    code = chksubjtype.Items[i].Value;
                    text = chksubjtype.Items[i].Text;
                    txtsubjtype.Text = text;
                }
                else
                {
                    code = code + "," + chksubjtype.Items[i].Value;
                    txtsubjtype.Text = text + "," + chksubjtype.Items[i].Text;
                }
            }
            else if (chksubjtype.Items[i].Selected != true)
            {
                txtsubjtype.Text = text + string.Empty;
            }
        }
        int count = 0;
        for (int i = 0; i < chkarrear_Sem.Items.Count; i++)
        {
            if (chkarrear_Sem.Items[i].Selected == true)
            {
                count++;
                if (text == "")
                {
                    text = "ArrearSem(" + count + ")";
                }
                else
                {
                    text = "ArrearSem(" + count + ")";
                }
                txtarrear_sem.Text = text;
            }
            else if (chkarrear_Sem.Items[i].Selected != true)
            {
                txtarrear_sem.Text = string.Empty;
            }
        }
    }

    protected void ddlpage_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Text = "Page.No: " + ddlpage.SelectedIndex.ToString();
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].VerticalAlign = VerticalAlign.Top;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 22].Border.BorderColorBottom = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 22].Border.BorderColorTop = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.White;
            for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = false;
            }
            int start = 0;
            int end = 0;
            string[] spl_pageval = Convert.ToString(ddlpage.SelectedValue).Split('-');
            if (spl_pageval.GetUpperBound(0) >= 1)
            {
                start = Convert.ToInt32(spl_pageval[0].ToString());
                end = Convert.ToInt32(spl_pageval[1].ToString());
            }
            if (end >= FpExternal.Sheets[0].RowCount)
            {
                end = FpExternal.Sheets[0].RowCount;
            }
            int rowstart = (FpExternal.Sheets[0].RowCount) - Convert.ToInt32(start);
            int rowend = (FpExternal.Sheets[0].RowCount) - Convert.ToInt32(end);
            for (int i = start; i < end; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = true;
            }
            if (Convert.ToInt32(ddlpage.SelectedItem.Text) == (ddlpage.Items.Count - 1))
            {
                FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Visible = true;
                FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 2].Visible = true;
                FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 3].Visible = true;
            }
            if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
            {
                for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
                {
                    FpExternal.Sheets[0].Rows[i].Visible = true;
                }
                Double totalRows = 0;
                totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
                Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
                Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                DropDownListpage.Items.Clear();
                if (totalRows >= 10)
                {
                    FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                    {
                        DropDownListpage.Items.Add((k + 10).ToString());
                    }
                    DropDownListpage.Items.Add("Others");
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpExternal.Height = 335;
                }
                else if (totalRows == 0)
                {
                    DropDownListpage.Items.Add("0");
                    FpExternal.Height = 100;
                }
                else
                {
                    FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                    FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
                }
                if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
                {
                    DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    CalculateTotalPages();
                }
                Buttontotal.Visible = true;
                lblrecord.Visible = true;
                DropDownListpage.Visible = true;
                TextBoxother.Visible = false;
                lblpage.Visible = true;
                TextBoxpage.Visible = true;
            }
            else
            {
                Buttontotal.Visible = false;
                lblrecord.Visible = false;
                DropDownListpage.Visible = false;
                TextBoxother.Visible = false;
                lblpage.Visible = false;
                TextBoxpage.Visible = false;
            }
        }
        catch
        {
        }
    }

    public void SPL_function_load_header()
    {
        try
        {
            MyImg mi = new MyImg();
            mi.ImageUrl = "~/college/Left_Logo.jpeg";
            mi.ImageUrl = "Handler2.ashx?";
            MyImg mi2 = new MyImg();
            mi2.ImageUrl = "~/images/10BIT001.jpeg";
            mi2.ImageUrl = "Handler5.ashx?";
            FpExternalHeader.Visible = false;
            FpExternalHeader.Sheets[0].RowCount = 0;
            FpExternalHeader.Sheets[0].ColumnCount = 0;
            FpExternalHeader.CommandBar.Visible = false;
            FpExternalHeader.RowHeader.Visible = false;
            FpExternalHeader.ColumnHeader.Visible = true;
            if (chk_subjectwisegrade.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnCount = 28;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnCount = 27;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.RowCount = 1;
            FpExternalHeader.Sheets[0].RowHeader.Visible = false;
            FpExternalHeader.Sheets[0].AutoPostBack = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpExternalHeader.Sheets[0].Columns[0].Width = 53;
            FpExternalHeader.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
            FpExternalHeader.Sheets[0].Columns[1].Width = 100;
            FpExternalHeader.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Regn.No";
            FpExternalHeader.Sheets[0].Columns[2].Width = 100;
            FpExternalHeader.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpExternalHeader.Sheets[0].Columns[3].Width = 175;
            FpExternalHeader.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            FpExternalHeader.Sheets[0].Columns[4].Width = 100;
            FpExternalHeader.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            if (chkshowsub_name.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subject Name";
                FpExternalHeader.Sheets[0].Columns[5].Width = 250;
                FpExternalHeader.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Subcode";
                FpExternalHeader.Sheets[0].Columns[5].Width = 80;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 6].Text = "CIA";
            FpExternalHeader.Sheets[0].Columns[6].Width = 50;
            FpExternalHeader.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 7].Text = "ESE";
            FpExternalHeader.Sheets[0].Columns[7].Width = 50;
            FpExternalHeader.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 8].Text = "TOTAL";
            FpExternalHeader.Sheets[0].Columns[8].Width = 85;
            FpExternalHeader.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 9].Text = "G";
            FpExternalHeader.Sheets[0].Columns[9].Width = 50;
            FpExternalHeader.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 10].Text = "Result";
            FpExternalHeader.Sheets[0].Columns[10].Width = 66;
            FpExternalHeader.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Y";
            FpExternalHeader.Sheets[0].Columns[11].Width = 50;
            FpExternalHeader.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
            if (chkshowsub_name.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Subject Name";
                FpExternalHeader.Sheets[0].Columns[12].Width = 250;
                FpExternalHeader.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Subcode";
                FpExternalHeader.Sheets[0].Columns[12].Width = 80;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 13].Text = "CIA";
            FpExternalHeader.Sheets[0].Columns[13].Width = 50;
            FpExternalHeader.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 14].Text = "ESE";
            FpExternalHeader.Sheets[0].Columns[14].Width = 50;
            FpExternalHeader.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 15].Text = "TOTAL";
            FpExternalHeader.Sheets[0].Columns[15].Width = 85;
            FpExternalHeader.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 16].Text = "G";
            FpExternalHeader.Sheets[0].Columns[16].Width = 50;
            FpExternalHeader.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 17].Text = "Result";
            FpExternalHeader.Sheets[0].Columns[17].Width = 66;
            FpExternalHeader.Sheets[0].Columns[17].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Y";
            FpExternalHeader.Sheets[0].Columns[18].Width = 50;
            FpExternalHeader.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Center;
            if (chkshowsub_name.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Subject Name";
                FpExternalHeader.Sheets[0].Columns[19].Width = 200;
                FpExternalHeader.Sheets[0].Columns[19].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Subcode";
                FpExternalHeader.Sheets[0].Columns[19].Width = 80;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 20].Text = "CIA";
            FpExternalHeader.Sheets[0].Columns[20].Width = 50;
            FpExternalHeader.Sheets[0].Columns[20].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 21].Text = "ESE";
            FpExternalHeader.Sheets[0].Columns[21].Width = 50;
            FpExternalHeader.Sheets[0].Columns[21].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 22].Text = "TOTAL";
            FpExternalHeader.Sheets[0].Columns[22].Width = 85;
            FpExternalHeader.Sheets[0].Columns[22].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 23].Text = "G";
            FpExternalHeader.Sheets[0].Columns[23].Width = 50;
            FpExternalHeader.Sheets[0].Columns[23].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 24].Text = "Result";
            FpExternalHeader.Sheets[0].Columns[24].Width = 66;
            FpExternalHeader.Sheets[0].Columns[24].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Y";
            FpExternalHeader.Sheets[0].Columns[25].Width = 50;
            FpExternalHeader.Sheets[0].Columns[25].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 26].Text = "SGPA";
            FpExternalHeader.Sheets[0].Columns[26].Width = 50;
            FpExternalHeader.Sheets[0].Columns[26].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].Columns[9].Visible = false;
            FpExternalHeader.Sheets[0].Columns[11].Visible = false;
            FpExternalHeader.Sheets[0].Columns[16].Visible = false;
            FpExternalHeader.Sheets[0].Columns[18].Visible = false;
            FpExternalHeader.Sheets[0].Columns[23].Visible = false;
            FpExternalHeader.Sheets[0].Columns[25].Visible = false;
            FpExternalHeader.Sheets[0].Columns[26].Visible = false;
            if (chk_subjectwisegrade.Checked == true)
            {
                FpExternalHeader.Sheets[0].Columns[27].Visible = false;
                FpExternalHeader.Sheets[0].Columns[9].Visible = true;
                FpExternalHeader.Sheets[0].Columns[16].Visible = true;
                FpExternalHeader.Sheets[0].Columns[23].Visible = true;
                FpExternalHeader.Sheets[0].Columns[26].Visible = true;
                FpExternalHeader.Sheets[0].Columns[27].Visible = true;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 26].Text = "GPA";
                FpExternalHeader.Sheets[0].Columns[26].Width = 50;
                FpExternalHeader.Sheets[0].Columns[26].HorizontalAlign = HorizontalAlign.Center;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 27].Text = "CGPA";
                FpExternalHeader.Sheets[0].Columns[27].Width = 50;
                FpExternalHeader.Sheets[0].Columns[27].HorizontalAlign = HorizontalAlign.Center;
            }
            FarPoint.Web.Spread.StyleInfo darkstyle = new FarPoint.Web.Spread.StyleInfo();
            darkstyle.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            darkstyle.ForeColor = Color.Black;
            darkstyle.Font.Name = "Book Antiqua";
            darkstyle.Font.Size = FontUnit.Large;
            darkstyle.Border.BorderSize = 10;
            darkstyle.HorizontalAlign = HorizontalAlign.Center;
            darkstyle.VerticalAlign = VerticalAlign.Middle;
            darkstyle.Border.BorderColor = System.Drawing.Color.Black;
            FpExternalHeader.ActiveSheetView.ColumnHeader.DefaultStyle = darkstyle;
            FpExternalHeader.Width = 1800;
            FpExternalHeader.SaveChanges();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void function_load_header()
    {
        try
        {
            MyImg mi = new MyImg();
            mi.ImageUrl = "~/college/Left_Logo.jpeg";
            mi.ImageUrl = "Handler2.ashx?";
            MyImg mi2 = new MyImg();
            mi2.ImageUrl = "~/images/10BIT001.jpeg";
            mi2.ImageUrl = "Handler5.ashx?";
            FpExternal.Visible = true; FpExternalHeader.Visible = true;
            FpExternal.Sheets[0].RowCount = 0;
            FpExternal.Sheets[0].ColumnCount = 0;
            FpExternal.Sheets[0].ColumnCount = 29;
            FpExternal.Sheets[0].ColumnHeader.RowCount = 1;
            FpExternal.Sheets[0].RowHeader.Visible = false;
            FpExternal.Sheets[0].AutoPostBack = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Text = "S.No";
            FpExternal.Sheets[0].Columns[0].Width = 60;
            FpExternal.Sheets[0].Columns[0].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = "RollNo";
            FpExternal.Sheets[0].Columns[1].Width = 120;
            FpExternal.Sheets[0].Columns[1].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text = "Regn.No";
            FpExternal.Sheets[0].Columns[2].Width = 120;
            FpExternal.Sheets[0].Columns[2].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Text = "Student Name";
            FpExternal.Sheets[0].Columns[3].Width = 235;
            FpExternal.Sheets[0].Columns[3].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 4].Text = "Student Type";
            FpExternal.Sheets[0].Columns[4].Width = 150;
            FpExternal.Sheets[0].Columns[4].HorizontalAlign = HorizontalAlign.Left;
            if (chkshowsub_name.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subject Name";
                FpExternal.Sheets[0].Columns[6].Width = 290;
                FpExternal.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 6].Text = "Subcode";
                FpExternal.Sheets[0].Columns[6].Width = 135;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 5].Text = "Gender";
            FpExternal.Sheets[0].Columns[5].Width = 100;
            FpExternal.Sheets[0].Columns[5].HorizontalAlign = HorizontalAlign.Left;
            if (chkgender.Checked == false)
            {
                FpExternal.Sheets[0].Columns[5].Visible = false;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 7].Text = "CIA";
            FpExternal.Sheets[0].Columns[7].Width = 80;
            FpExternal.Sheets[0].Columns[7].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 8].Text = "ESE" + ((chkShowValuationMarks.Checked) ? " [I,II,III]" : "");
            FpExternal.Sheets[0].Columns[8].Width = 80;
            FpExternal.Sheets[0].Columns[8].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 9].Text = "TOTAL";
            FpExternal.Sheets[0].Columns[9].Width = 95;
            FpExternal.Sheets[0].Columns[9].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 10].Text = "G";
            FpExternal.Sheets[0].Columns[10].Width = 65;
            FpExternal.Sheets[0].Columns[10].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 11].Text = "Result";
            FpExternal.Sheets[0].Columns[11].Width = 70;
            FpExternal.Sheets[0].Columns[11].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 12].Text = "Y";
            FpExternal.Sheets[0].Columns[12].Width = 65;
            FpExternal.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Center;
            if (chkshowsub_name.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Subject Name";
                FpExternal.Sheets[0].Columns[13].Width = 290;
                FpExternal.Sheets[0].Columns[13].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 13].Text = "Subcode";
                FpExternal.Sheets[0].Columns[13].Width = 135;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 14].Text = "CIA";
            FpExternal.Sheets[0].Columns[14].Width = 80;
            FpExternal.Sheets[0].Columns[14].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 15].Text = "ESE" + ((chkShowValuationMarks.Checked) ? " [I,II,III]" : "");
            FpExternal.Sheets[0].Columns[15].Width = 80;
            FpExternal.Sheets[0].Columns[15].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 16].Text = "TOTAL";
            FpExternal.Sheets[0].Columns[16].Width = 95;
            FpExternal.Sheets[0].Columns[16].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 17].Text = "G";
            FpExternal.Sheets[0].Columns[17].Width = 65;
            FpExternal.Sheets[0].Columns[17].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 18].Text = "Result";
            FpExternal.Sheets[0].Columns[18].Width = 70;
            FpExternal.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 19].Text = "Y";
            FpExternal.Sheets[0].Columns[19].Width = 65;
            FpExternal.Sheets[0].Columns[19].HorizontalAlign = HorizontalAlign.Center;
            if (chkshowsub_name.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Subject Name";
                FpExternal.Sheets[0].Columns[20].Width = 290;
                FpExternal.Sheets[0].Columns[20].HorizontalAlign = HorizontalAlign.Left;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 20].Text = "Subcode";
                FpExternal.Sheets[0].Columns[20].Width = 135;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 21].Text = "CIA";
            FpExternal.Sheets[0].Columns[21].Width = 80;
            FpExternal.Sheets[0].Columns[21].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Text = "ESE" + ((chkShowValuationMarks.Checked) ? " [I,II,III]" : "");
            FpExternal.Sheets[0].Columns[22].Width = 80;
            FpExternal.Sheets[0].Columns[22].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 23].Text = "TOTAL";
            FpExternal.Sheets[0].Columns[23].Width = 95;
            FpExternal.Sheets[0].Columns[23].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 24].Text = "G";
            FpExternal.Sheets[0].Columns[24].Width = 65;
            FpExternal.Sheets[0].Columns[24].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 25].Text = "Result";
            FpExternal.Sheets[0].Columns[25].Width = 70;
            FpExternal.Sheets[0].Columns[25].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 26].Text = "Y";
            FpExternal.Sheets[0].Columns[26].Width = 65;
            FpExternal.Sheets[0].Columns[26].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 27].Text = "GPA";
            FpExternal.Sheets[0].Columns[27].Width = 65;
            FpExternal.Sheets[0].Columns[27].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].Columns[10].Visible = false;
            FpExternal.Sheets[0].Columns[12].Visible = false;
            FpExternal.Sheets[0].Columns[17].Visible = false;
            FpExternal.Sheets[0].Columns[19].Visible = false;
            FpExternal.Sheets[0].Columns[24].Visible = false;
            FpExternal.Sheets[0].Columns[26].Visible = false;
            FpExternal.Sheets[0].Columns[27].Visible = false;
            FpExternal.Sheets[0].Columns[28].Visible = false;
            //if (rb1.Checked)
            //{
            FpExternal.Sheets[0].Columns[12].Visible = true;
            FpExternal.Sheets[0].Columns[19].Visible = true;
            FpExternal.Sheets[0].Columns[26].Visible = true;
            //}
            if (chk_subjectwisegrade.Checked == true)
            {
                FpExternal.Sheets[0].Columns[10].Visible = true;
                FpExternal.Sheets[0].Columns[17].Visible = true;
                FpExternal.Sheets[0].Columns[24].Visible = true;
                FpExternal.Sheets[0].Columns[27].Visible = true;
                FpExternal.Sheets[0].Columns[28].Visible = true;
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 27].Text = "GPA";
                FpExternal.Sheets[0].Columns[27].Width = 65;
                FpExternal.Sheets[0].Columns[27].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 28].Text = "CGPA";
                FpExternal.Sheets[0].Columns[28].Width = 65;
                FpExternal.Sheets[0].Columns[28].HorizontalAlign = HorizontalAlign.Center;
            }
            if (chkgrade.Checked == true)
            {
                FpExternal.Sheets[0].Columns[27].Visible = true;
            }
            else
            {
                FpExternal.Sheets[0].Columns[27].Visible = false;
            }
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.ForeColor = Color.Black;
            style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
            style.HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            column_count = FpExternal.Sheets[0].ColumnCount;
            if (cblColumnOrder.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    switch (liValue)
                    {
                        case "0":
                            FpExternal.Sheets[0].Columns[0].Visible = liOrder.Selected;
                            break;
                        case "1":
                            FpExternal.Sheets[0].Columns[1].Visible = liOrder.Selected;
                            break;
                        case "2":
                            FpExternal.Sheets[0].Columns[2].Visible = liOrder.Selected;
                            break;
                        case "3":
                            FpExternal.Sheets[0].Columns[3].Visible = liOrder.Selected;
                            break;
                        case "4":
                            FpExternal.Sheets[0].Columns[4].Visible = liOrder.Selected;
                            break;
                        case "5":
                            FpExternal.Sheets[0].Columns[5].Visible = liOrder.Selected;
                            break;
                        case "6":
                            FpExternal.Sheets[0].Columns[6].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[13].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[20].Visible = liOrder.Selected;
                            break;
                        case "7":
                            FpExternal.Sheets[0].Columns[7].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[14].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[21].Visible = liOrder.Selected;
                            break;
                        case "8":
                            FpExternal.Sheets[0].Columns[8].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[15].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[22].Visible = liOrder.Selected;
                            break;
                        case "9":
                            FpExternal.Sheets[0].Columns[9].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[16].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[23].Visible = liOrder.Selected;
                            break;
                        case "10":
                            FpExternal.Sheets[0].Columns[10].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[17].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[24].Visible = liOrder.Selected;
                            break;
                        case "11":
                            FpExternal.Sheets[0].Columns[11].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[18].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[25].Visible = liOrder.Selected;
                            break;
                        case "12":
                            FpExternal.Sheets[0].Columns[12].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[19].Visible = liOrder.Selected;
                            FpExternal.Sheets[0].Columns[26].Visible = liOrder.Selected;
                            break;
                        case "13":
                            FpExternal.Sheets[0].Columns[27].Visible = liOrder.Selected;
                            break;
                        case "14":
                            FpExternal.Sheets[0].Columns[28].Visible = liOrder.Selected;
                            break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void function_radioheader()
    {
        ddlpage.Items.Clear();
        int totrowcount = 0;
        for (int find_tot_rowcnt = 0; find_tot_rowcnt < (FpExternal.Sheets[0].RowCount); find_tot_rowcnt++)
        {
            totrowcount++;
        }
        int intialrow = 1;
        int pages = 0;
        int remainrows = 0;
        int i6 = 0;
        int cal_row = 0;
        int fromrow = 30;
        int torow = 0;
        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    forloop:
        for (cal_row = fromrow; cal_row > torow; cal_row--)
        {
            if (cal_row < FpExternal.Sheets[0].RowCount)
            {
                if (FpExternal.Sheets[0].Cells[cal_row, 5].Text == "")
                {
                    i6++;
                    ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), torow.ToString() + "-" + cal_row.ToString()));
                    break;
                }
            }
        }
        if (cal_row < FpExternal.Sheets[0].RowCount)
        {
            torow = cal_row + 1;
            fromrow = 30 + fromrow + 1;
            goto forloop;
        }
        //=========================================================================
        //pages = totrowcount / 30;//hided on 30.07.12
        /// pages = totrowcount / (6 * (find_subjrow_count + 2));
        //  remainrows = totrowcount % 30;//hided on 30.07.12
        //if (FpExternal.Sheets[0].RowCount > 0)
        //{
        //    int i5 = 0;
        //    int i6 = 0;
        //    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //    for (i6 = 1; i6 <= pages; i6++)
        //    {
        //        i5 = i6;
        //        ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), intialrow.ToString()));
        //        intialrow = intialrow + (6 * (find_subjrow_count + 2));
        //        //intialrow = intialrow + 30; //hided on 30.07.12 (6 * (find_subjrow_count + 2));
        //    }
        //    if (remainrows > 0)
        //    {
        //        i6 = i5 + 1;
        //        ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), intialrow.ToString()));
        //    }
        //}
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpExternal.Height = 335;
            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpExternal.Height = 100;
            }
            else
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                CalculateTotalPages();
            }
            Buttontotal.Visible = true;
            lblrecord.Visible = true;
            DropDownListpage.Visible = true;
            TextBoxother.Visible = false;
            lblpage.Visible = true;
            TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
        }
    }

    public void func_footer()
    {
        try
        {
            if (FpExternal.Sheets[0].RowCount > 0)
            {
                if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                {
                    //string address2 = daccess.GetFunction("select address2 from collinfo where college_code=" + Session["collegecode"] + "");
                    //string coename = daccess.GetFunction("select coe from collinfo where college_code=" + Session["collegecode"] + "");
                    int startColumn = 0;
                    if (cblColumnOrder.Items.Count > 0)
                    {
                        foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                        {
                            string liValue = Convert.ToString(liOrder.Value).Trim();
                            if (liOrder.Selected)
                            {
                                break;
                            }
                            startColumn++;
                        }
                    }
                    //FpExternal.Sheets[0].RowCount += 4;
                    //if (Convert.ToInt16(Session["Rollflag"]) == 1 && Convert.ToInt16(Session["Regflag"]) == 0)//clmn 2
                    //{
                    //    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, FpExternal.Sheets[0].ColumnCount);
                    //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, startColumn, 1, FpExternal.Sheets[0].ColumnCount - startColumn);
                    //    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    if (txtDate.Text != "")
                    //    {
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Text = "Date : " + txtDate.Text.ToString();
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    }
                    //}
                    //else if (Convert.ToInt16(Session["Regflag"]) == 1 && Convert.ToInt16(Session["Rollflag"]) == 0)//clmn 3
                    //{
                    //    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, FpExternal.Sheets[0].ColumnCount);
                    //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, startColumn, 1, FpExternal.Sheets[0].ColumnCount - startColumn);
                    //    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    if (txtDate.Text != "")
                    //    {
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Text = "Date : " + txtDate.Text.ToString();
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
                    //    }
                    //}
                    //else if (Convert.ToInt16(Session["Regflag"]) == 0 && Convert.ToInt16(Session["Rollflag"]) == 0)
                    //{
                    //    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, FpExternal.Sheets[0].ColumnCount);
                    //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, startColumn, 1, FpExternal.Sheets[0].ColumnCount - startColumn);
                    //    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = "Place :" + address2.ToString();
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Right;
                    //    if (txtDate.Text != "")
                    //    {
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Text = txtDate.Text.ToString();
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Text = "Date : ";
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    }
                    //}
                    //else if (Convert.ToInt16(Session["Rollflag"]) == 1 && Convert.ToInt16(Session["Regflag"]) == 1)
                    //{
                    //    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, FpExternal.Sheets[0].ColumnCount);
                    //    FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, startColumn, 1, FpExternal.Sheets[0].ColumnCount - startColumn);
                    //    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    if (txtDate.Text != "")
                    //    {
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Text = "Date : " + txtDate.Text.ToString();
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //    }
                    //}
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorRight = Color.White;
                    //if (FpExternal.Sheets[0].ColumnCount > 6)
                    //{
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Bold = true;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Left;
                    //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Border.BorderColorLeft = Color.White;
                    //    if (txtDate.Text != "")
                    //    {
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Border.BorderColorLeft = Color.White;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Border.BorderColorBottom = Color.White;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Border.BorderColorBottom = Color.White;
                    //        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 6, 1, FpExternal.Sheets[0].ColumnCount - 4);
                    //        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 6, 1, FpExternal.Sheets[0].ColumnCount - 4);
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorLeft = Color.White;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.White;
                    //        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].HorizontalAlign = HorizontalAlign.Left;
                    //    }
                    //}
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, startColumn].Text = coename;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, startColumn].Font.Bold = true;
                    //FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 4].Height = 45;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, startColumn].Font.Size = FontUnit.Medium;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, startColumn].VerticalAlign = VerticalAlign.Top;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, startColumn].HorizontalAlign = HorizontalAlign.Right;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Text = "Controller of Examinations";
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].VerticalAlign = VerticalAlign.Bottom;
                    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, startColumn, 1, FpExternal.Sheets[0].ColumnCount - startColumn);
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Font.Bold = true;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Font.Size = FontUnit.Medium;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].HorizontalAlign = HorizontalAlign.Right;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Border.BorderColorLeft = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Bold = true;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Font.Size = FontUnit.Medium;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].HorizontalAlign = HorizontalAlign.Left;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, startColumn].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, startColumn].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.White;
                    //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, startColumn, 2, FpExternal.Sheets[0].ColumnCount - startColumn);
                    if (chkShowNoteDescription.Checked)
                    {
                        FpExternal.Sheets[0].RowCount += 2;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Text = " P - PASS , " + ((string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? "F - FAIL" : (txtFailValue.Text) + " - " + ((txtFailValue.Text.Trim().ToLower() == "f" || (txtFailValue.Text).Trim().ToLower().Contains("f")) ? "FAIL" : ((txtFailValue.Text).Trim().ToLower() == "ra" || (txtFailValue.Text).Trim().ToLower().Contains("ra")) ? " RE APPEAR" : "FAIL")) + " , A - ABSENT , W - WITHHELD [L - LACK OF ATTENDANCE , M - MALPRACTICE , F - FEES NOT PAID , D - DUES]";
                        divFooterResult.InnerHtml = " P - PASS , " + ((string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? "F - FAIL" : (txtFailValue.Text) + " - " + ((txtFailValue.Text.Trim().ToLower() == "f" || (txtFailValue.Text).Trim().ToLower().Contains("f")) ? "FAIL" : ((txtFailValue.Text).Trim().ToLower() == "ra" || (txtFailValue.Text).Trim().ToLower().Contains("ra")) ? " RE APPEAR" : "FAIL")) + " , A - ABSENT , W - WITHHELD [L - LACK OF ATTENDANCE , M - MALPRACTICE , F - FEES NOT PAID , D - DUES]";
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Font.Bold = true;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].Border.BorderColor = Color.Wheat;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].VerticalAlign = VerticalAlign.Middle;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, startColumn, 2, FpExternal.Sheets[0].ColumnCount - startColumn);
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, startColumn].VerticalAlign = VerticalAlign.Bottom;
                    }
                    FpExternal.Sheets[0].PageSize = FpExternal.Sheets[0].RowCount;
                    FpExternal.Height = (FpExternal.Sheets[0].RowCount * 20) + 200;
                    FpExternal.Sheets[0].SheetName = " ";
                }
            }
        }
        catch
        {
        }
    }

    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch)
    {
        string GetUnivExamCode = string.Empty;
        string degree_code = string.Empty;
        string current_sem = string.Empty;
        string batch_year = string.Empty;
        string strExam_code = string.Empty;
        //Added By Malang Raja
        //string qryExamCode = "Select Exam_Code from Exam_Details where Degree_Code ='" + DegreeCode.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Exam_year='" + ddlYear.SelectedValue.ToString() + "' and Batch_Year ='" + Batch.ToString() + "' and current_semester='" + Semester + "'";
        //DataSet dsExamCodeNew = new DataSet();
        //dsExamCodeNew = d2.select_method_wo_parameter(qryExamCode, "text");
        strExam_code = "Select Exam_Code from Exam_Details where Degree_Code ='" + DegreeCode.ToString() + "' and Exam_Month='" + ddlMonth.SelectedValue.ToString() + "' and Exam_year='" + ddlYear.SelectedValue.ToString() + "' and Batch_Year ='" + Batch.ToString() + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(strExam_code, "text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_examcode in ds.Tables[0].Rows)
            {
                string examCode = Convert.ToString(dr_examcode["Exam_Code"]).Trim();
                if (!string.IsNullOrEmpty(examCode))
                {
                    GetUnivExamCode = examCode;
                }
            }
        }
        if (!string.IsNullOrEmpty(GetUnivExamCode))
        {
            return Convert.ToInt32(GetUnivExamCode);
        }
        else
        {
            return 0;
        }
    }
    
    public List<string> GetExamCodes(int degreeCode, int batchYear, int examMonth, int examYear)
    {
        List<string> lstExamCode = new List<string>();
        string qry = "Select distinct Exam_Code from Exam_Details where Degree_Code ='" + degreeCode.ToString() + "' and Exam_Month='" + examMonth.ToString() + "' and Exam_year='" + examYear.ToString() + "' and Batch_Year ='" + batchYear.ToString() + "'";
        DataSet ds = new DataSet();
        ds = d2.select_method_wo_parameter(qry, "text");
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_examcode in ds.Tables[0].Rows)
            {
                string examCode = Convert.ToString(dr_examcode["Exam_Code"]).Trim();
                if (!string.IsNullOrEmpty(examCode))
                {
                    if (!lstExamCode.Contains(examCode))
                    {
                        lstExamCode.Add(examCode);
                    }
                }
            }
        }
        return lstExamCode;
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            btnprintmaster.Visible = false;
            btnPrint.Visible = false;
            Printcontrol.Visible = false;
            pnlSpread.Visible = false;
            int visibleColumns = 0;
            if (rdotmr.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Visible = true;
                function_load_header();
                load_students_data();
                func_footer();
                function_radioheader();
                if (FpExternal.Sheets[0].RowCount > 0)
                {
                    if (FpExternal.Sheets[0].ColumnCount > 0)
                    {
                        for (int col = 0; col < FpExternal.Sheets[0].ColumnCount; col++)
                        {
                            if (FpExternal.Sheets[0].Columns[col].Visible == true)
                            {
                                visibleColumns++;
                            }
                        }
                    }
                }
                if (FpExternal.Sheets[0].RowCount == 0 || visibleColumns == 0)
                {
                    lblnorec.Visible = true;
                    lblnorec.Text = "No Records Found";
                    Buttontotal.Visible = false;
                    lblrecord.Visible = false;
                    DropDownListpage.Visible = false;
                    TextBoxother.Visible = false;
                    lblpage.Visible = false;
                    TextBoxpage.Visible = false;
                    FpExternal.Visible = false;
                    btnprintmaster.Visible = false;
                    btnPrint.Visible = false;
                    Printcontrol.Visible = false;
                    FpExternalHeader.Visible = false;
                    btnExcel.Visible = false;
                    pnlSpread.Visible = false;
                }
                else
                {
                    pnlSpread.Visible = true;
                    lblnorec.Visible = false;
                    btnExcel.Visible = true;
                    FpExternal.Visible = true;
                    FpExternalHeader.Visible = true;
                    btnprintmaster.Visible = true;
                    btnPrint.Visible = true;

                }
                FpExternalHeader.Visible = false;
            }
            else
            {
                spl_function_load_headercons();
                function_load_headercons();
                load_students_datacons();
                func_footercons();
                function_radioheadercons();
                if (FpExternal.Sheets[0].RowCount == 0 || FpExternal.Sheets[0].RowCount <= 4)
                {

                    lblnorec.Visible = true;
                    Buttontotal.Visible = false;
                    lblrecord.Visible = false;
                    DropDownListpage.Visible = false;
                    TextBoxother.Visible = false;
                    lblpage.Visible = false;
                    TextBoxpage.Visible = false;
                    FpExternal.Visible = false;
                    FpExternalHeader.Visible = false;
                    btnExcel.Visible = false;

                }
                else
                {
                    pnlSpread.Visible = true;
                    lblnorec.Visible = false;
                    btnExcel.Visible = true;
                    FpExternal.Visible = true;
                    FpExternalHeader.Visible = true;
                }
            }
            pnlrecordcount.Visible = false;
            FpExternal.CommandBar.Visible = false;
            if (FpExternal.Sheets[0].RowCount > 0)
            {
                if (!string.IsNullOrEmpty(txtCollegeHeader.Text.Trim()))
                {
                    spnCollegeHeader.InnerHtml = txtCollegeHeader.Text.Trim();
                }
                else
                {
                    if (Session["collegecode"] != null)
                    {
                        spnCollegeHeader.InnerHtml = daccess.GetFunctionv("select collname from collinfo where college_code='" + Session["collegecode"].ToString() + "'");
                    }
                }
                string strExam_month = string.Empty;
                exam_month = exam_month.Trim();
                switch (exam_month)
                {
                    case "1":
                        strExam_month = "January";
                        break;
                    case "2":
                        strExam_month = "February";
                        break;
                    case "3":
                        strExam_month = "March";
                        break;
                    case "4":
                        strExam_month = "April";
                        break;
                    case "5":
                        strExam_month = "May";
                        break;
                    case "6":
                        strExam_month = "June";
                        break;
                    case "7":
                        strExam_month = "July";
                        break;
                    case "8":
                        strExam_month = "Augest";
                        break;
                    case "9":
                        strExam_month = "September";
                        break;
                    case "10":
                        strExam_month = "October";
                        break;
                    case "11":
                        strExam_month = "November";
                        break;
                    case "12":
                        strExam_month = "December";
                        break;
                }
                string qry = "select clg.collname,c.Edu_Level,c.Course_Name,dt.Dept_Name,ltrim(rtrim(ISNULL(c.type,''))) as Type,'Class :'+c.Course_Name+' '+dt.Dept_Name  as Degree_Details from collinfo clg,Course c,Degree dg,Department dt where c.college_code=clg.college_code and clg.college_code=dg.college_code and  clg.college_code=dt.college_code and dt.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'";
                DataSet dsDegreeDetails = new DataSet();
                dsDegreeDetails = d2.select_method_wo_parameter(qry, "text");
                string className = string.Empty;
                string sectionDetails = string.Empty;
                string collegeHeader = txtCollegeHeader.Text.Trim();
                string copyOfReport = string.Empty;
                string selectedValue = string.Empty;
                if (rblOfficeDeptCopy.Items.Count > 0)
                {
                    selectedValue = Convert.ToString(rblOfficeDeptCopy.SelectedValue).Trim();
                }
                switch (selectedValue)
                {
                    case "0":
                        copyOfReport = "(Office Copy)";
                        break;
                    case "1":
                        copyOfReport = "(Dept. Copy)";
                        break;
                    case "2":
                        copyOfReport = string.Empty;
                        break;
                    default:
                        copyOfReport = string.Empty;
                        break;
                }
                if (chkShowValuationMarks.Checked)
                {
                    copyOfReport = "(Office Copy)";
                }
                if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
                {
                    className = Convert.ToString(dsDegreeDetails.Tables[0].Rows[0]["Degree_Details"]).Trim();
                }
                else
                {
                    className = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " " + ddlBranch.SelectedItem.ToString();
                }
                if (ddlSec.Enabled)
                {
                    if (ddlSec.Items.Count > 0)
                    {
                        if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                        {
                            sectionDetails = " '" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "' SECTION";
                        }
                    }
                }
                spnExamYearMonth.InnerHtml = ((chksubjtype.Items.Count > 0 && chksubjtype.Items[1].Selected && !chksubjtype.Items[0].Selected) ? "Arrear " : "") + ((rb1.Checked) ? "Result of the Semester Examination " : "TABULATED MARK REGISTER - ") + strExam_month + " " + ddlYear.SelectedItem.ToString() + copyOfReport;
                spnDegreeDetails.InnerHtml = className +" "+ sectionDetails+" "+dsDegreeDetails.Tables[0].Rows[0]["Type"];
                spnSemester.InnerHtml = ((chksubjtype.Items.Count > 0 && (!chksubjtype.Items[1].Selected || chksubjtype.Items[0].Selected)) ? "Semester : " + Convert.ToString(ddlSemYr.SelectedItem).Trim() : "");
            }
        }
        catch (Exception ex)
        {
            string vetri = ex.ToString();
            daccess.sendErrorMail(ex, Convert.ToString(Session["collegecode"]).Trim(), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }

    }

    public void load_students_data()
    {
        try
        {
            FpExternal.Visible = true;
            FpExternalHeader.Visible = true;
            FpExternal.Sheets[0].RowCount = 0;
            FpExternal.Sheets[0].RowHeader.Visible = false;
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Rows[0].Font.Size = FontUnit.Large;
            FpExternal.Sheets[0].SetColumnMerge(0, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpExternal.Sheets[0].SetColumnMerge(1, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpExternal.Sheets[0].SetColumnMerge(2, FarPoint.Web.Spread.Model.MergePolicy.Always);
            FpExternal.Sheets[0].SetColumnMerge(3, FarPoint.Web.Spread.Model.MergePolicy.Always);
            Dictionary<string, int> dicNotEligible = new Dictionary<string, int>();
            Dictionary<string, int> dicNotRegistered = new Dictionary<string, int>();
            Dictionary<string, int> dicContoNotPaid = new Dictionary<string, int>();
            Dictionary<string, int> dicFeesNotPaid = new Dictionary<string, int>();
            Dictionary<string, int> dicMalPractice = new Dictionary<string, int>();
            Dictionary<string, string> dicSyllCodeSem = new Dictionary<string, string>();
            Hashtable htDegreeDetails = new Hashtable();
            string strStudents = string.Empty;
            string result = string.Empty;
            bool isDepartmentCopy = false;
            bool isRegularResult = false;
            if (rblOfficeDeptCopy.Items.Count > 0)
            {
                isDepartmentCopy = false;
                string value = Convert.ToString(rblOfficeDeptCopy.SelectedValue).Trim();
                switch (value)
                {
                    case "1":
                        isDepartmentCopy = true;
                        break;
                    default:
                        isDepartmentCopy = false;
                        break;
                }
            }
            if (chksubjtype.Items[0].Selected || !chksubjtype.Items[1].Selected)
            {
                isRegularResult = true;
            }
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            string[] spl_exmyr = exam_year.Split('0');
            degree_code = ddlBranch.SelectedValue.ToString();
            semdec = Convert.ToInt32(ddlSemYr.SelectedValue.ToString());
            batch_year = ddlBatch.SelectedItem.ToString();
            List<string> lstExamCode = GetExamCodes(Convert.ToInt32(degree_code), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            //IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(semdec), Convert.ToInt32(batch_year));
            string examCodeList = string.Join(",", lstExamCode.ToArray());
            string strExam_month = string.Empty;
            exam_month = exam_month.Trim();
            switch (exam_month)
            {
                case "1":
                    strExam_month = "J ";
                    break;
                case "2":
                    strExam_month = "F ";
                    break;
                case "3":
                    strExam_month = "M ";
                    break;
                case "4":
                    strExam_month = "A ";
                    break;
                case "5":
                    strExam_month = "M ";
                    break;
                case "6":
                    strExam_month = "J ";
                    break;
                case "7":
                    strExam_month = "J ";
                    break;
                case "8":
                    strExam_month = "A ";
                    break;
                case "9":
                    strExam_month = "S ";
                    break;
                case "10":
                    strExam_month = "O ";
                    break;
                case "11":
                    strExam_month = "N ";
                    break;
                case "12":
                    strExam_month = "D ";
                    break;
            }
            string arr_sem_in = string.Empty;
            string all_syllcode = string.Empty;
            string qryCondoSem = string.Empty;
            if (chkarrear_Sem.Items.Count > 0)
            {
                for (int xarr = 0; xarr < chkarrear_Sem.Items.Count; xarr++)
                {
                    if (chkarrear_Sem.Items[xarr].Selected == true)
                    {
                        if (arr_sem_in == "")
                        {
                            arr_sem_in = "'" + chkarrear_Sem.Items[xarr].Value.Trim() + "'";
                        }
                        else
                        {
                            arr_sem_in += ",'" + chkarrear_Sem.Items[xarr].Value + "'";
                        }
                    }
                }
            }
            else
            {
                if (ddlSemYr.Items.Count > 0)
                {
                    arr_sem_in = "'" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'";
                    //qryCondoSem = " and el.semester in('" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "')";
                }
            }
            if (!string.IsNullOrEmpty(arr_sem_in))
            {
                arr_sem_in = " and Semester in(" + arr_sem_in + ")";
            }
            if (ddlSemYr.Items.Count > 0)
            {
                qryCondoSem = " and el.semester in('" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "')";
            }
            string strsyllcode = "Select Syll_Code,Semester from Syllabus_master where Degree_Code =" + ddlBranch.SelectedValue.ToString() + arr_sem_in + " and Batch_Year = " + ddlBatch.SelectedValue.ToString() + string.Empty;
            DataSet ds_syllcode = new DataSet();
            string qrySyllCode = string.Empty;
            ds_syllcode = d2.select_method_wo_parameter(strsyllcode, "Text");
            if (ds_syllcode.Tables.Count > 0 && ds_syllcode.Tables[0].Rows.Count > 0)
            {
                if (chksubjtype.Items[1].Selected == true)
                {
                    for (int syll = 0; syll < ds_syllcode.Tables[0].Rows.Count; syll++)
                    {
                        string syllCode = string.Empty;
                        string semesterNew = string.Empty;
                        syllCode = Convert.ToString(ds_syllcode.Tables[0].Rows[syll]["Syll_Code"]).Trim();
                        semesterNew = Convert.ToString(ds_syllcode.Tables[0].Rows[syll]["Semester"]).Trim();
                        if (!dicSyllCodeSem.ContainsKey(syllCode))
                        {
                            dicSyllCodeSem.Add(syllCode, semesterNew);
                        }
                        if (chkarrear_Sem.Items.Count > 0)
                        {
                            if (chkarrear_Sem.Items[syll].Selected == true)
                            {
                                if (all_syllcode == "")
                                {
                                    all_syllcode = "'" + ds_syllcode.Tables[0].Rows[syll]["Syll_Code"].ToString() + "'";
                                }
                                else
                                {
                                    all_syllcode = all_syllcode + ",'" + ds_syllcode.Tables[0].Rows[syll]["Syll_Code"].ToString() + "'";
                                }
                            }
                        }
                    }
                    if (all_syllcode != string.Empty)
                    {
                        qrySyllCode = " and Subject.syll_code  in(" + all_syllcode + ")";
                    }
                }
                else
                {
                    for (int syll = 0; syll < ds_syllcode.Tables[0].Rows.Count; syll++)
                    {
                        string syllCode = string.Empty;
                        string semesterNew = string.Empty;
                        syllCode = Convert.ToString(ds_syllcode.Tables[0].Rows[syll]["Syll_Code"]).Trim();
                        semesterNew = Convert.ToString(ds_syllcode.Tables[0].Rows[syll]["Semester"]).Trim();
                        if (!dicSyllCodeSem.ContainsKey(syllCode))
                        {
                            dicSyllCodeSem.Add(syllCode, semesterNew);
                        }
                    }
                    qrySyllCode = " and Subject.syll_code  in('" + Convert.ToString(ds_syllcode.Tables[0].Rows[0]["Syll_Code"]).Trim() + "')";
                }
            }
            qry = "select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "";
            DataSet dsGrade = new DataSet();
            dsGrade = d2.select_method_wo_parameter(qry, "text");
            if (dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr_grade_val in dsGrade.Tables[0].Rows)
                {
                    grade_setting = Convert.ToString(dr_grade_val[0]).Trim();
                }
            }
            Boolean flag_stud_u = false;
            Boolean flag_subj_rowcnt = false;
            string sections = string.Empty;
            if (ddlSec.Enabled && ddlSec.Items.Count > 0)
            {
                sections = Convert.ToString(ddlSec.SelectedValue).Trim();
                if (sections.Trim().ToLower() == "all" || sections.Trim().ToLower() == "0" || sections.Trim().ToLower() == "" || sections.Trim().ToLower() == "-1")
                {
                    sections = string.Empty;
                }
                else
                {
                    sections = "and r.sections='" + Convert.ToString(ddlSec.SelectedValue).Trim() + "'";
                }
            }
            string qryStudentList = string.Empty;
            string qryNRStudents = string.Empty;
            DataSet dsStudenList = new DataSet();
            DataSet dsNotEligible = new DataSet();
            DataSet dsNotRegistered = new DataSet();
            DataSet dsCondoNotPaid = new DataSet();
            DataSet dsFeesNotPaid = new DataSet();
            DataSet dsMalPractice = new DataSet();
            dsFeesNotPaid = d2.select_method_wo_parameter("select len(r.reg_no), r.Batch_Year,r.Roll_No,r.Reg_No,r.Stud_Name,CASE WHEN ltrim(rtrim(isnull(ea.is_confirm,'0')))='1' THEN 'Paid' ELSE 'Unpaid' END status,r.Current_Semester from Exam_Details ed,exam_appl_details ead,exam_application ea,Registration r where r.Batch_Year=ed.batch_year and ed.degree_code=r.degree_code and r.Roll_No=ea.roll_no and ea.appl_no=ead.appl_no and ltrim(rtrim(isnull(ea.is_confirm,'0')))='0' and ed.Exam_Month='" + Convert.ToString(ddlMonth.SelectedValue).Trim() + "' and ed.Exam_year='" + Convert.ToString(ddlYear.SelectedItem.Text).Trim() + "' and r.Degree_Code = '" + degree_code + "' and r.Batch_Year = '" + batch_year + "' " + sections + "  order by len(r.reg_no),r.reg_no,r.stud_name", "text");
            if (dsFeesNotPaid.Tables.Count > 0 && dsFeesNotPaid.Tables[0].Rows.Count > 0)
            {
                dicFeesNotPaid.Clear();
                foreach (DataRow dr in dsFeesNotPaid.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                    if (!dicFeesNotPaid.ContainsKey(rollNo))
                    {
                        dicFeesNotPaid.Add(rollNo, 1);
                    }
                }
            }
            string qryMP = "Select r.Roll_No from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and r.Degree_Code ='" + degree_code + "' and r.Batch_Year ='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and result='whd' and Exam_Code in(" + examCodeList + ") " + sections + "";
            if (chkRedo.Checked)
            {
                qryMP += " union Select r.Roll_No from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r,StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and  sm.Batch_Year=sr.BatchYear and sm.degree_code=sr.DegreeCode and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and sr.DegreeCode ='" + degree_code + "' and ISNULL(r.isRedo,'0')='1' and sr.BatchYear='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and result='whd' and Exam_Code in(" + examCodeList + ") " + sections + "";
            }

            dsMalPractice = d2.select_method_wo_parameter(qryMP, "Text");
            if (dsMalPractice.Tables.Count > 0 && dsMalPractice.Tables[0].Rows.Count > 0)
            {
                dicMalPractice.Clear();
                foreach (DataRow dr in dsMalPractice.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                    if (!dicMalPractice.ContainsKey(rollNo))
                    {
                        dicMalPractice.Add(rollNo, 1);
                    }
                }
            }
            dsCondoNotPaid = d2.select_method_wo_parameter("select len(r.reg_no),r.Batch_Year,r.Roll_No,r.Reg_No,r.Stud_Name,isnull(el.fine_amt,'0') total_fee,CASE WHEN isnull(el.isCondonationFee,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status,el.semester,r.Current_Semester from Registration r,Eligibility_list el where el.Roll_no=r.Roll_No and r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.App_No=el.app_no and el.is_eligible='2' and isnull(el.isCondonationFee,'0')='0' and r.Degree_Code = '" + degree_code + "' and r.Batch_Year = '" + batch_year + "' " + sections + Convert.ToString(qryCondoSem).Trim() + "  order by len(r.reg_no),r.reg_no,r.stud_name", "Text");
            if (dsCondoNotPaid.Tables.Count > 0 && dsCondoNotPaid.Tables[0].Rows.Count > 0)
            {
                dicContoNotPaid.Clear();
                foreach (DataRow dr in dsCondoNotPaid.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                    if (!dicContoNotPaid.ContainsKey(rollNo))
                    {
                        dicContoNotPaid.Add(rollNo, 1);
                    }
                }
            }
            dsNotEligible = d2.select_method_wo_parameter("select len(r.reg_no),r.Batch_Year,r.Roll_No,r.Reg_No,r.Stud_Name,isnull(el.fine_amt,'0') total_fee,CASE WHEN isnull(el.isCondonationFee,'0')='1' THEN 'Paid' ELSE 'Unpaid' END status,el.semester,r.Current_Semester from Registration r,Eligibility_list el where el.Roll_no=r.Roll_No and r.Batch_Year=el.batch_year and el.degree_code=r.degree_code and r.App_No=el.app_no and el.is_eligible='3' and r.Degree_Code = '" + degree_code + "' and r.Batch_Year = '" + batch_year + "' " + Convert.ToString(qryCondoSem).Trim() + sections + " order by len(r.reg_no),r.reg_no,r.stud_name", "Text");
            if (dsNotEligible.Tables.Count > 0 && dsNotEligible.Tables[0].Rows.Count > 0)
            {
                dicNotEligible.Clear();
                foreach (DataRow dr in dsNotEligible.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    rollNo = Convert.ToString(dr["Roll_No"]).Trim();
                    if (!dicNotEligible.ContainsKey(rollNo))
                    {
                        dicNotEligible.Add(rollNo, 1);
                    }
                }
            }
            qryNRStudents = "select distinct r.roll_no as RlNo,isnull(r.Reg_No,'') as RgNo ,isnull(r.Stud_Name,'') as SName,isnull(r.stud_type,'') as type,roll_admit,r.mode as mode,a.sex,'Class :'+c.Course_Name+' '+dt.Dept_Name+case when (ltrim(rtrim(ISNULL(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(ISNULL(r.Sections,'')))+'' else '' end+ case when (ltrim(rtrim(ISNULL(c.type,'')))<>'') then ' ( '+ltrim(rtrim(ISNULL(c.type,'')))+' ) ' else '' end   as Degree_Details,'0' as Type,'Regular' as Status from registration r,applyn a,Course c,Degree dg,Department dt,tbl_not_registred m,Subject s,sub_sem ss,syllabus_master sm where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and a.app_no=r.app_no and m.roll_no=r.roll_no and m.subject_no=s.subject_no  and m.exam_year='" + Convert.ToString(ddlYear.SelectedItem.Text).Trim() + "' and m.exam_month='" + Convert.ToString(ddlMonth.SelectedItem.Value).Trim() + "'" + "  and c.college_code=dg.college_code and r.college_code=c.college_code and r.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.college_code=r.college_code and r.Degree_Code = '" + degree_code + "' and r.Batch_Year ='" + batch_year + "' " + sections + "";
            dsNotRegistered = d2.select_method_wo_parameter(qryNRStudents, "text");
            if (dsNotRegistered.Tables.Count > 0 && dsNotRegistered.Tables[0].Rows.Count > 0)
            {
                dicNotRegistered.Clear();
                foreach (DataRow dr in dsNotRegistered.Tables[0].Rows)
                {
                    string rollNo = string.Empty;
                    rollNo = Convert.ToString(dr["RlNo"]).Trim();
                    if (!dicNotRegistered.ContainsKey(rollNo))
                    {
                        dicNotRegistered.Add(rollNo, 1);
                    }
                }
            }
            ccqry = string.Empty;
            if (!chkIncludePrivate.Checked)
                ccqry = " and cc='0'";
            string studREdo = "select distinct r.roll_no as RlNo,isnull(r.Reg_No,'') as RgNo ,isnull(r.Stud_Name,'') as SName,isnull(r.stud_type,'') as type,roll_admit,r.mode as mode,a.sex,'Class :'+c.Course_Name+' '+dt.Dept_Name+case when (ltrim(rtrim(ISNULL(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(ISNULL(r.Sections,'')))+'' else '' end+ case when (ltrim(rtrim(ISNULL(c.type,'')))<>'') then ' ( '+ltrim(rtrim(ISNULL(c.type,'')))+' ) ' else '' end   as Degree_Details,'0' as Type,'Regular' as Status from StudentRedoDetails sr,Registration r,applyn a,Course c,Degree dg,Department dt  ";

            qryStudentList = "select distinct r.roll_no as RlNo,isnull(r.Reg_No,'') as RgNo ,isnull(r.Stud_Name,'') as SName,isnull(r.stud_type,'') as type,roll_admit,r.mode as mode,a.sex,'Class :'+c.Course_Name+' '+dt.Dept_Name+case when (ltrim(rtrim(ISNULL(r.Sections,'')))<>'') then ' - '+ltrim(rtrim(ISNULL(r.Sections,'')))+'' else '' end+ case when (ltrim(rtrim(ISNULL(c.type,'')))<>'') then ' ( '+ltrim(rtrim(ISNULL(c.type,'')))+' ) ' else '' end   as Degree_Details,'0' as Type,'Regular' as Status from registration r,applyn a,Course c,Degree dg,Department dt  ";
            if (chksubjtype.Items.Count > 0)
            {
                if (!chksubjtype.Items[0].Selected && chksubjtype.Items[1].Selected)
                {
                    qryStudentList += " ,mark_entry m,Subject s,sub_sem ss,syllabus_master sm where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and a.app_no=r.app_no and m.roll_no=r.roll_no and m.subject_no=s.subject_no and exam_code in(" + examCodeList + ") " + ((chksubjtype.Items[0].Selected) ? " and sm.semester='" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'" : "") + ((chksubjtype.Items[1].Selected) ? ((chkIncludePassedOut.Checked) ? "" : "  and sm.semester<>'" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'") : "");

                    studREdo += " ,mark_entry m,Subject s,sub_sem ss,syllabus_master sm where sm.Batch_Year=sr.BatchYear and sm.degree_code=sr.DegreeCode and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and a.app_no=r.app_no and m.roll_no=r.roll_no and m.subject_no=s.subject_no  and exam_code in(" + examCodeList + ") " + ((chksubjtype.Items[0].Selected) ? " and sm.semester='" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'" : "") + ((chksubjtype.Items[1].Selected) ? ((chkIncludePassedOut.Checked) ? "" : "  and sm.semester<>'" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'") : "");
                }
                else
                {
                    qryStudentList += " where a.app_no=r.app_no ";
                    studREdo += " where r.App_No=sr.Stud_AppNo and a.app_no=sr.Stud_AppNo ";
                }
                qryStudentList += " and c.college_code=dg.college_code and r.college_code=c.college_code and r.degree_code=dg.Degree_Code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.college_code=r.college_code and r.Degree_Code = " + degree_code + " and r.Batch_Year ='" + batch_year + "' " + sections + "  and Exam_Flag <>'debar'" + ((chkIncludeDiscontinue.Checked) ? "" : " and DelFlag ='0'");
                studREdo += " and ISNULL(r.isRedo,'0')='1' and c.college_code=dg.college_code and r.college_code=c.college_code and sr.DegreeCode=dg.Degree_Code and c.Course_Id=dg.Course_Id and dt.Dept_Code=dg.Dept_Code and dg.college_code=r.college_code and sr.DegreeCode = " + degree_code + " and sr.BatchYear ='" + batch_year + "' " + sections + "  and Exam_Flag <>'debar'" + ((chkIncludeDiscontinue.Checked) ? "" : " and DelFlag ='0'");
            }
            strStudents = "select distinct m.roll_no as RlNo,isnull(r.Reg_No,'') as RgNo ,isnull(r.Stud_Name,'') as SName,isnull(r.stud_type,'') as type,roll_admit,r.mode as mode,a.sex from registration r,mark_entry m,applyn a where a.app_no=r.app_no and m.roll_no=r.roll_no  and exam_code in(" + examCodeList + ") " + sections + " ";
            if (chksubjtype.Items[0].Selected == true && chksubjtype.Items[1].Selected == false)
            {
                strStudents = strStudents + "  and DelFlag ='0'" + ccqry + " and Exam_Flag <>'debar' ";  //modified
            }
            strStudents = strStudents + "  order by RgNo,SName";
            if (chksubjtype.Items[0].Selected || !chksubjtype.Items[1].Selected)
            {
                qryStudentList += " union " + qryNRStudents;
            }
            if (chkRedo.Checked)
            {
                qryStudentList += " union " + studREdo;
            }
            qryStudentList += " order by RgNo,SName";

            string orderBYPref = string.Empty;
            string orderBYSort = string.Empty;

            if (ddlOrderby.SelectedIndex == 0)
            {
                orderBYPref = " order by sm.semester,subjectpriority ";
                orderBYSort = " semester asc,subjectpriority asc ";
            }
            else
            {
                orderBYPref = " order by sm.semester desc,subject_type desc,m.subject_no ";
                orderBYSort = " semester desc,subject_type desc,subject_no ";
            }
            string submark = string.Empty;
            if (chksubjtype.Items[0].Selected == true && chksubjtype.Items[1].Selected == true)
            {
                submark = " Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and r.Degree_Code ='" + degree_code + "' and r.Batch_Year ='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code  in(" + examCodeList + ") " + sections + " ";
                if (chkRedo.Checked)
                {
                    submark += " union Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r, StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and  sm.Batch_Year=sr.BatchYear and sm.degree_code=sr.DegreeCode and ISNULL(r.isRedo,'0')='1' and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and sr.DegreeCode ='" + degree_code + "' and sr.BatchYear ='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code  in(" + examCodeList + ") " + sections + " ";
                }
                submark += orderBYPref;// " order by sm.semester,subjectpriority";
            }
            else if (chksubjtype.Items[0].Selected == true) //for regular
            {
                //submark = "Select m.*,maxtotal,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,min_ext_marks,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from Mark_Entry m,Subject s,sub_sem ss,registration r where r.roll_no=m.roll_no and r.Degree_Code = " + degree_code + " and r.Batch_Year = " + batch_year + " and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code  in(" + examCodeList + ") and m.Attempts =1 order by subjectpriority --and m.Attempts =0 ";//mam
                //,m.attempts,subject_type desc,m.subject_no
                submark = "Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no and r.Degree_Code ='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + " and sm.semester='" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "' ";
                if (chkRedo.Checked)
                {
                    submark += " union Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r , StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and  sm.Batch_Year=sr.BatchYear and sm.degree_code=sr.DegreeCode and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no and sr.DegreeCode ='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.Subject_No = s.Subject_No and ISNULL(r.isRedo,'0')='1' and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + " and sm.semester='" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "' ";
                }
                submark += orderBYPref;//" order by sm.semester,subjectpriority";
            }
            else if (chksubjtype.Items[1].Selected == true) //for arrear
            {
                //submark = "Select m.*,maxtotal,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,min_ext_marks,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from m,Subject,sub_sem,registration r where r.roll_no=m.roll_no and r.Degree_Code = " + degree_code + " and r.Batch_Year = " + batch_year + " and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code  in(" + examCodeList + ")  and s.syll_code=ss.syll_code " + qrySyllCode + " order by subjectpriority";
                //,m.attempts,subject_type desc,m.subject_no and m.Attempts >1
                submark = "Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no and r.Degree_Code ='" + degree_code + "' and r.Batch_Year='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + ((chkIncludePassedOut.Checked) ? "" : "   and sm.semester<>'" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'") + "  ";
                if (chkRedo.Checked)
                {
                    submark += " union Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,sm.semester,m.remarks,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority  from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r , StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and  sm.Batch_Year=sr.BatchYear and sm.degree_code=sr.DegreeCode and ISNULL(r.isRedo,'0')='1' and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no and sr.DegreeCode ='" + degree_code + "' and sr.BatchYear='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + ((chkIncludePassedOut.Checked) ? "" : "   and sm.semester<>'" + Convert.ToString(ddlSemYr.SelectedItem.Text).Trim() + "'") + " ";
                }
                submark += orderBYPref;//" order by sm.semester,subjectpriority";
            }
            else if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected != true) //both not selected
            {
                submark = "Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,m.remarks,sm.semester,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r where sm.Batch_Year=r.Batch_Year and sm.degree_code=r.degree_code and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and r.Degree_Code ='" + degree_code + "' and r.Batch_Year ='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + " "; //mam
                //,m.attempts,subject_type desc,m.subject_no
                if (chkRedo.Checked)
                {
                    submark = "Select m.roll_no,m.subject_no,m.internal_mark,m.external_mark,m.total,m.result,m.passorfail,m.type,m.remarks,sm.semester,m.exam_code,m.attempts,m.grade,m.cp,m.evaluation1,m.evaluation2,m.evaluation3,m.Average,Subject_type,s.externalValuationCount,subject_code,s.syll_code,min_int_marks,max_int_marks,min_ext_marks,max_ext_marks,mintotal,maxtotal,r.roll_no,ISNULL(subjectpriority,'0') subjectpriority from Mark_Entry m,Subject s,sub_sem ss,syllabus_master sm,registration r , StudentRedoDetails sr where r.App_No=sr.Stud_AppNo and  sm.Batch_Year=sr.BatchYear and ISNULL(r.isRedo,'0')='1' and sm.degree_code=sr.DegreeCode and sm.syll_code=s.syll_code and ss.syll_code=sm.syll_code and r.roll_no=m.roll_no  and sr.DegreeCode ='" + degree_code + "' and sr.BatchYear ='" + batch_year + "' and m.Subject_No = s.Subject_No and s.subtype_no= ss.subtype_no and Exam_Code in(" + examCodeList + ") " + sections + ""; //mam
                }
                submark += orderBYPref;//" order by sm.semester,subjectpriority";
            }
            DataSet ds_Students = daccess.select_method_wo_parameter(strStudents, "Text");
            dsStudenList = d2.select_method_wo_parameter(qryStudentList, "text");
            if (dsStudenList.Tables.Count > 0 && dsStudenList.Tables[0].Rows.Count > 0)
            {
                DataSet dssubmarkdetails = daccess.select_method_wo_parameter(submark, "Text");
                string getattmaxmark = daccess.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
                string[] semecount = getattmaxmark.Split(new Char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (semecount.GetUpperBound(0) == 1)
                {
                    attmpt = Convert.ToInt32(semecount[0].ToString());
                    maxmarkve = Convert.ToInt32(semecount[1].ToString());
                    flagvetri = true;
                }
                else
                {
                    flagvetri = false;
                }
                if (dssubmarkdetails.Tables.Count > 0 && dssubmarkdetails.Tables[0].Rows.Count > 0)
                {
                    for (int stud = 0; stud < dsStudenList.Tables[0].Rows.Count; stud++)
                    {
                        flag_stud_u = false;
                        string chk = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                        string degreeDetails = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["Degree_Details"]).Trim();
                        DataView dvsubload = new DataView();
                        string minInternalMarks = string.Empty;
                        string maxInternalMarks = string.Empty;
                        string minExternalMarks = string.Empty;
                        string maxExternalMarks = string.Empty;
                        string minTotalMarks = string.Empty;
                        string maxTotalMarks = string.Empty;
                        string internalMarksNew = string.Empty;
                        string externalMarksNew = string.Empty;
                        string totalMarksNew = string.Empty;
                        string passOrFailNew = string.Empty;
                        string resultActualNew = string.Empty;
                        string actualGradeNew = string.Empty;
                        string yearofPassingNew = string.Empty;
                        bool showStudentMarks = true;
                        if (dssubmarkdetails.Tables.Count > 0 && dssubmarkdetails.Tables[0].Rows.Count > 0)
                        {
                            dssubmarkdetails.Tables[0].DefaultView.RowFilter = "roll_no='" + chk + "'";
                            dvsubload = dssubmarkdetails.Tables[0].DefaultView;
                            dvsubload.Sort = orderBYSort;//"semester asc,subjectpriority asc";
                        }
                        if (chkShowsSectionWise.Checked)
                        {
                            if (!htDegreeDetails.Contains(degreeDetails))
                            {
                                FpExternal.Sheets[0].RowCount++;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = degreeDetails;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Locked = true;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].BackColor = Color.Gray;
                                FpExternal.Sheets[0].AddSpanCell(FpExternal.Sheets[0].RowCount - 1, 0, 1, FpExternal.Sheets[0].ColumnCount);
                                htDegreeDetails.Add(degreeDetails, "1");
                            }
                        }
                        if (isRegularResult)
                        {
                            showStudentMarks = false;
                            if (dicNotRegistered.ContainsKey(chk.Trim()))
                            {
                                if (chksubjtype.Items[0].Selected || !chksubjtype.Items[1].Selected)
                                {
                                    if (chkIncludeNotRegistered.Checked)
                                    {
                                        serialno++;
                                        int studRowCount = FpExternal.Sheets[0].RowCount++;
                                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                                        FpExternal.Sheets[0].Columns[1].CellType = tt;
                                        FpExternal.Sheets[0].Columns[2].CellType = tt;
                                        string sex = dsStudenList.Tables[0].Rows[stud]["sex"].ToString();
                                        if (sex == "1")
                                        {
                                            sex = "Female";
                                        }
                                        else if (sex == "2")
                                        {
                                            sex = "Transend";
                                        }
                                        else
                                        {
                                            sex = "Male";
                                        }
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dsStudenList.Tables[0].Rows[stud]["RgNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "   " + dsStudenList.Tables[0].Rows[stud]["SName"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = dsStudenList.Tables[0].Rows[stud]["type"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = serialno.ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = sex;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = "Not Registered";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpExternal.Sheets[0].AddSpanCell(FpExternal.Sheets[0].RowCount - 1, 6, 1, FpExternal.Sheets[0].ColumnCount);
                                        if (rb2.Checked)
                                        {
                                            FpExternal.Sheets[0].RowCount += 2;
                                        }
                                        if (rb1.Checked)
                                        {
                                            FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;
                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderSize = 1;
                                        }
                                        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                                    }
                                }
                            }
                            else if (dicNotEligible.ContainsKey(chk.Trim()))
                            {
                                if (chksubjtype.Items[0].Selected || !chksubjtype.Items[1].Selected)
                                {
                                    if (chkIncludeRedoSuspended.Checked)
                                    {
                                        serialno++;
                                        int studRowCount = FpExternal.Sheets[0].RowCount++;
                                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                                        FpExternal.Sheets[0].Columns[1].CellType = tt;
                                        FpExternal.Sheets[0].Columns[2].CellType = tt;
                                        string sex = dsStudenList.Tables[0].Rows[stud]["sex"].ToString();
                                        if (sex == "1")
                                        {
                                            sex = "Female";
                                        }
                                        else if (sex == "2")
                                        {
                                            sex = "Transend";
                                        }
                                        else
                                        {
                                            sex = "Male";
                                        }
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dsStudenList.Tables[0].Rows[stud]["RgNo"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "   " + dsStudenList.Tables[0].Rows[stud]["SName"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = dsStudenList.Tables[0].Rows[stud]["type"].ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = serialno.ToString();
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = sex;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = "REDO/SUSPENDED";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                        FpExternal.Sheets[0].AddSpanCell(FpExternal.Sheets[0].RowCount - 1, 6, 1, FpExternal.Sheets[0].ColumnCount);
                                        if (rb2.Checked)
                                        {
                                            FpExternal.Sheets[0].RowCount += 2;
                                        }
                                        if (rb1.Checked)
                                        {
                                            FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;
                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderSize = 1;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.Black;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderSize = 1;
                                        }
                                        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                                    }
                                }
                            }
                            else
                            {
                                showStudentMarks = true;
                            }
                        }
                        if (dvsubload.Count > 0 && showStudentMarks)
                        {
                            bool isShowMarkResult = true;
                            bool isLackOfAttendance = false;
                            bool isFeesNotPaid = false;
                            bool isMalPracticeStudent = false;
                            bool isShowFeesNotPaid = false;
                            bool isShowMalPractice = false;
                            bool isShowLackOfAttendance = false;
                            if (isDepartmentCopy)// && isRegularResult
                            {
                                DataTable dtStudentMarks = new DataTable();
                                DataTable dtMalPracticeList = new DataTable();
                                dtStudentMarks = dvsubload.ToTable();
                                dtStudentMarks.DefaultView.RowFilter = "result='whd'";
                                dtMalPracticeList = dtStudentMarks.DefaultView.ToTable();
                                if (dicFeesNotPaid.ContainsKey(chk.Trim()))
                                {
                                    isShowMarkResult = false;
                                    isFeesNotPaid = true;
                                    if (chkIncludeFeesNotPaid.Checked)
                                    {
                                        isShowFeesNotPaid = true;
                                    }
                                    else
                                    {
                                        isShowMarkResult = true;
                                    }
                                }
                                else if (dicContoNotPaid.ContainsKey(chk.Trim()))
                                {
                                    isShowMarkResult = false;
                                    isLackOfAttendance = true;
                                    if (chkIncludeLackOfAttendance.Checked)
                                    {
                                        isShowLackOfAttendance = true;
                                    }
                                    else
                                    {
                                        isShowMarkResult = true;
                                    }
                                }
                                else if (dicMalPractice.ContainsKey(chk.Trim()))
                                {
                                    isShowMarkResult = false;
                                    isMalPracticeStudent = true;
                                    if (chkIncludeMalPractice.Checked)
                                    {
                                        isShowMalPractice = true;
                                    }
                                    else
                                    {
                                        isShowMarkResult = true;
                                    }
                                }
                                else
                                {
                                    isShowMarkResult = true;
                                }
                                if (!isShowMarkResult && (isShowFeesNotPaid || isShowLackOfAttendance || isShowMalPractice))
                                {
                                    serialno++;
                                    int studRowCount = FpExternal.Sheets[0].RowCount++;
                                    FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                                    FpExternal.Sheets[0].Columns[1].CellType = tt;
                                    FpExternal.Sheets[0].Columns[2].CellType = tt;
                                    string sex = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["sex"]).Trim();
                                    if (sex == "1")
                                    {
                                        sex = "Female";
                                    }
                                    else if (sex == "2")
                                    {
                                        sex = "Transend";
                                    }
                                    else
                                    {
                                        sex = "Male";
                                    }
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RgNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "   " + Convert.ToString(dsStudenList.Tables[0].Rows[stud]["SName"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["type"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialno).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = sex;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = ((isFeesNotPaid) ? "FEES NOT PAID" : ((isLackOfAttendance) ? "LACK OF ATTENDANCE" : ((isMalPracticeStudent) ? "MALPRACTICE" : "")));
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Center;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].VerticalAlign = VerticalAlign.Middle;
                                    FpExternal.Sheets[0].AddSpanCell(FpExternal.Sheets[0].RowCount - 1, 6, 1, FpExternal.Sheets[0].ColumnCount);
                                    if (rb2.Checked)
                                    {
                                        FpExternal.Sheets[0].RowCount += 2;
                                    }
                                    if (rb1.Checked)
                                    {
                                        FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;
                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderSize = 1;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.Black;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderSize = 1;
                                    }
                                    FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                                }
                            }
                            else
                            {
                                isShowMarkResult = true;
                            }
                            if (isShowMarkResult)
                            {
                                serialno++;
                                int studRowCount = FpExternal.Sheets[0].RowCount++;
                                FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                                FpExternal.Sheets[0].Columns[1].CellType = tt;
                                FpExternal.Sheets[0].Columns[2].CellType = tt;
                                string sex = dsStudenList.Tables[0].Rows[stud]["sex"].ToString();
                                if (sex == "1")
                                {
                                    sex = "Female";
                                }
                                else if (sex == "2")
                                {
                                    sex = "Transend";
                                }
                                else
                                {
                                    sex = "Male";
                                }
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RgNo"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "  " + Convert.ToString(dsStudenList.Tables[0].Rows[stud]["SName"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["type"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialno).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = sex;
                                int sub_increment = 1;
                                int subIncrementCount = 11;
                                int spanRowCount = 1;
                                for (int subcode = 0; subcode < dvsubload.Count; subcode++)
                                {
                                    minInternalMarks = string.Empty;
                                    maxInternalMarks = string.Empty;
                                    minExternalMarks = string.Empty;
                                    maxExternalMarks = string.Empty;
                                    minTotalMarks = string.Empty;
                                    maxTotalMarks = string.Empty;
                                    internalMarksNew = string.Empty;
                                    externalMarksNew = string.Empty;
                                    totalMarksNew = string.Empty;
                                    passOrFailNew = string.Empty;
                                    resultActualNew = string.Empty;
                                    actualGradeNew = string.Empty;
                                    yearofPassingNew = string.Empty;
                                    bool internalOnly = false;
                                    bool externalOnly = false;
                                    string SyllCode = Convert.ToString(dvsubload[subcode]["syll_code"]).Trim();
                                    getsubno = Convert.ToString(dvsubload[subcode]["Subject_No"]).Trim();
                                    minInternalMarks = Convert.ToString(dvsubload[subcode]["min_int_marks"]).Trim();
                                    maxInternalMarks = Convert.ToString(dvsubload[subcode]["max_int_marks"]).Trim();
                                    internalMarksNew = Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim();
                                    minExternalMarks = Convert.ToString(dvsubload[subcode]["min_ext_marks"]).Trim();
                                    maxExternalMarks = Convert.ToString(dvsubload[subcode]["max_ext_marks"]).Trim();
                                    externalMarksNew = Convert.ToString(dvsubload[subcode]["external_mark"]).Trim();
                                    minTotalMarks = Convert.ToString(dvsubload[subcode]["mintotal"]).Trim();
                                    maxTotalMarks = Convert.ToString(dvsubload[subcode]["maxtotal"]).Trim();
                                    totalMarksNew = Convert.ToString(dvsubload[subcode]["total"]).Trim();
                                    passOrFailNew = Convert.ToString(dvsubload[subcode]["passorfail"]).Trim();
                                    resultActualNew = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                    actualGradeNew = Convert.ToString(dvsubload[subcode]["grade"]).Trim();
                                    string passorfail = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                    string externalValuationCount = Convert.ToString(dvsubload[subcode]["externalValuationCount"]).Trim();
                                    int externalMarkValuationCount = 0;
                                    int.TryParse(externalValuationCount.Trim(), out externalMarkValuationCount);
                                    yearofPassingNew = string.Empty;
                                    double internalMarksValue = 0;
                                    double ExternalMarksValue = 0;
                                    double totalMarksValue = 0;
                                    double.TryParse(internalMarksNew, out internalMarksValue);
                                    double.TryParse(externalMarksNew, out ExternalMarksValue);
                                    double.TryParse(totalMarksNew, out totalMarksValue);
                                    string totalValues = (string.IsNullOrEmpty(totalMarksNew) ? "--" : ((totalMarksValue == -1) ? "AA" : (totalMarksValue == -2) ? "NE" : (totalMarksValue == -3) ? "NR" : (totalMarksValue == -4) ? "LT" : Math.Round(Convert.ToDouble(Convert.ToString(totalMarksValue).Trim()), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0')));
                                    string resultValue = (string.IsNullOrEmpty(resultActualNew) ? "--" : ((totalMarksValue == -1 || internalMarksValue == -1 || ExternalMarksValue == -1) ? "AA" : (totalMarksValue == -2 || internalMarksValue == -2 || ExternalMarksValue == -2) ? "NE" : (totalMarksValue == -3 || internalMarksValue == -3 || ExternalMarksValue == -3) ? "NR" : (totalMarksValue == -4 || internalMarksValue == -4 || ExternalMarksValue == -4) ? "LT" : Math.Round(Convert.ToDouble(totalMarksValue), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0')));
                                    string resultSubject = resultActualNew;
                                    if (!string.IsNullOrEmpty(resultSubject))
                                    {
                                        if (resultSubject.Trim().ToLower() == "pass")
                                        {
                                            resultSubject = "P";
                                        }
                                        else if (resultSubject.Trim().ToLower().Contains("fail"))
                                        {
                                            resultSubject = ((!string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? txtFailValue.Text.Trim() : resultSubject.Trim());
                                        }
                                        else if (string.Equals(resultSubject.Trim().ToLower(), "sa"))
                                        {
                                            resultSubject = "SA";
                                        }
                                        else if (resultSubject.Trim().ToLower().Contains("aaa") || resultSubject.Trim().ToLower().Contains("a") || resultSubject.Trim().ToLower().Contains("a") || resultSubject.Trim().ToLower().Contains("ab"))
                                        {
                                            resultSubject = "A";
                                        }
                                        else
                                        {
                                            resultSubject = ((!string.IsNullOrEmpty(txtFailValue.Text.Trim())) ? txtFailValue.Text.Trim() : resultSubject.Trim());
                                        }
                                    }
                                    else
                                    {
                                        resultSubject = string.Empty;
                                    }
                                    //if (internalMarksValue == -1 && totalMarksValue >= 0 && ExternalMarksValue >= 0)
                                    //{
                                    //    resultSubject = "RA";
                                    //}
                                    //if (ExternalMarksValue == -1 || totalMarksValue == -1)
                                    //{
                                    //    resultSubject = "A";
                                    //}
                                    if (string.IsNullOrEmpty(maxInternalMarks) || maxInternalMarks.Trim() == "0")
                                    {
                                        externalOnly = true;
                                        if (ExternalMarksValue < 0)
                                        {
                                            totalValues = "--";
                                        }
                                    }
                                    if (string.IsNullOrEmpty(maxExternalMarks) || maxExternalMarks.Trim() == "0")
                                    {
                                        internalOnly = true;
                                        if (internalMarksValue < 0)
                                        {
                                            totalValues = "--";
                                        }
                                    }
                                    if (internalOnly)
                                    {
                                        if (internalMarksValue == -1)
                                        {
                                            resultSubject = "RA";
                                        }
                                    }
                                    else if (externalOnly)
                                    {
                                        if (ExternalMarksValue == -1)
                                        {
                                            resultSubject = "A";
                                        }
                                        else if (ExternalMarksValue == -1 || totalMarksValue == -1)
                                        {
                                            resultSubject = "A";
                                        }
                                    }
                                    else
                                    {
                                        if (internalMarksValue == -1 && (ExternalMarksValue >= 0 && totalMarksValue >= 0))
                                        {
                                            resultSubject = "RA";
                                        }
                                        else if (ExternalMarksValue == -1)
                                        {
                                            resultSubject = "A";
                                        }
                                        else if (ExternalMarksValue == -1 || totalMarksValue == -1)
                                        {
                                            resultSubject = "A";
                                        }
                                    }

                                    if (subcode % 3 == 0 & subcode != 0)
                                    {
                                        FpExternal.Sheets[0].RowCount++;
                                    }
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RlNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["RgNo"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["SName"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = Convert.ToString(dsStudenList.Tables[0].Rows[stud]["type"]).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = Convert.ToString(serialno).Trim();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].HorizontalAlign = HorizontalAlign.Left;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = sex;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].CellType = new TextCellType();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].CellType = new TextCellType();
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].CellType = new TextCellType();
                                    string valuation1 = string.Empty;
                                    string valuation2 = string.Empty;
                                    string valuation3 = string.Empty;
                                    string valuationMarks = string.Empty;
                                    string valuationMarksABNENR = string.Empty;
                                    string valuationAB = "AA";
                                    string valuationNE = "NE";
                                    string valuationNR = "NR";
                                    string valuationMP = "MP";
                                    string valuationLT = "L";
                                    string valuationExtOnly = "--";
                                    bool isMalPractice = false;
                                    bool isAbsent = false;
                                    bool isNotRegitered = false;
                                    bool isNotEntry = false;
                                    string semesterValue = string.Empty;

                                    if (chkshowsub_name.Checked == true)
                                    {
                                        string getsubject = daccess.GetFunctionv("select top 1 subject_name from subject where subject_code='" + Convert.ToString(dvsubload[subcode]["Subject_Code"]).Trim() + "'");
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5 + sub_increment].Text = getsubject;
                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5 + sub_increment].Text = Convert.ToString(dvsubload[subcode]["Subject_Code"]).Trim();
                                    }
                                    attmtreal = Convert.ToString(dvsubload[subcode]["attempts"]).Trim();
                                    string stuinternalmarks = Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim();
                                    string stuexternammarks = Convert.ToString(dvsubload[subcode]["external_mark"]).Trim();
                                    valuation1 = Convert.ToString(dvsubload[subcode]["evaluation1"]).Trim();
                                    valuation2 = Convert.ToString(dvsubload[subcode]["evaluation2"]).Trim();
                                    valuation3 = Convert.ToString(dvsubload[subcode]["evaluation3"]).Trim();
                                    if (chkShowValuationMarks.Checked)
                                    {
                                        string tempValue = string.Empty;
                                        if (externalMarkValuationCount >= 2)
                                        {
                                            valuationMarksABNENR = valuationMarks = " [--,--,--] ";
                                            valuationAB = "AA [AA,AA,--] ";
                                            valuationMP = "MP [MP,MP,--] ";
                                            valuationNE = "NE [NE,NE,--] ";
                                            valuationNR = "NR [NR,NR,--] ";
                                            valuationLT = "L [L,L,--] ";
                                            valuationExtOnly = "-- [--,--,--]";
                                            if (string.IsNullOrEmpty(valuation1.Trim()))
                                            {
                                                tempValue = "--";
                                            }
                                            else
                                            {
                                                tempValue = valuation1.PadLeft(2, '0');
                                            }
                                            if (string.IsNullOrEmpty(valuation2.Trim()))
                                            {
                                                if (string.IsNullOrEmpty(tempValue))
                                                {
                                                    tempValue = "--,--";
                                                }
                                                else
                                                {
                                                    tempValue += ",--";
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(tempValue))
                                                {
                                                    tempValue = "--," + valuation2.PadLeft(2, '0');
                                                }
                                                else
                                                {
                                                    tempValue += "," + valuation2.PadLeft(2, '0');
                                                }
                                            }
                                            if (string.IsNullOrEmpty(valuation3.Trim()))
                                            {
                                                if (string.IsNullOrEmpty(tempValue))
                                                {
                                                    tempValue = "--,--,--";
                                                }
                                                else
                                                {
                                                    tempValue += ",--";
                                                }
                                            }
                                            else
                                            {
                                                if (string.IsNullOrEmpty(tempValue))
                                                {
                                                    tempValue = "--,--," + valuation3.PadLeft(2, '0');
                                                }
                                                else
                                                {
                                                    tempValue += "," + valuation3.PadLeft(2, '0');
                                                }
                                            }
                                        }
                                        else
                                        {
                                            tempValue = (string.IsNullOrEmpty(valuation1.Trim().PadLeft(2, '0')) ? stuexternammarks : valuation1.PadLeft(2, '0'));
                                        }
                                        if (!string.IsNullOrEmpty(tempValue))
                                        {
                                            valuationMarks = " [" + tempValue + "] ";
                                        }
                                    }
                                    int gradeflag = Convert.ToInt32(d2.GetFunction("select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + ""));
                                    if (gradeflag == 0)
                                    {
                                        gradeflag = 3;
                                    }
                                    if (passorfail.Trim().ToLower().Contains("whd"))
                                    {
                                        isMalPractice = true;
                                    }
                                    if (gradeflag == 1) //mark and grade
                                    {
                                        if (grade_setting == "0") // display the marks
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"])) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks;
                                            if (stuinternalmarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString("AA").Trim());
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NE");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NR");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "L");
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"])) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            }
                                            if (stuexternammarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationAB);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNE);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNR);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationLT);
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks : valuationMP);
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(Convert.ToString(dvsubload[subcode]["total"]).Trim()), 0, MidpointRounding.AwayFromZero).ToString().Trim().PadLeft(2, '0'));
                                            string resultcheck = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                            if (resultcheck.Trim().ToUpper() == "AAA" || resultcheck.Trim().ToUpper() == "AA" || resultcheck.Trim().ToUpper() == "A" || resultcheck.Trim().ToUpper() == "AB")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = "A";
                                            }
                                            else
                                            {
                                                // check jairam
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = Convert.ToString(dvsubload[subcode]["grade"]).Trim();
                                            }
                                            if (passorfail.Trim().ToLower() == "pass" || chkfailshow.Checked == true)
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : Convert.ToString(dvsubload[subcode]["result"]).Trim())) : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : (passorfail.Trim().ToLower() == "pass") ? "P" : ((resultcheck.Trim().ToUpper() == "AAA" || resultcheck.Trim().ToUpper() == "AA" || resultcheck.Trim().ToUpper() == "A" || resultcheck.Trim().ToUpper() == "AB" || internalMarksValue == -1 || totalMarksValue == -1 || ExternalMarksValue == -1) ? "A" : Convert.ToString(dvsubload[subcode]["result"]).Trim())));
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                            }
                                            if (passorfail.Trim().ToLower() == "pass")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + spl_exmyr[1].ToString();
                                            }
                                            else
                                            {
                                                flag_stud_u = true;
                                            }
                                        }
                                        else //display corresponding grade convert grade
                                        {
                                            if (Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim() != string.Empty && Convert.ToString(dvsubload[subcode]["external_mark"]).Trim() != string.Empty)
                                            {
                                                if (Convert.ToDouble(dvsubload[subcode]["internal_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_int_marks"].ToString()) && Convert.ToDouble(dvsubload[subcode]["external_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_ext_marks"].ToString()))
                                                {
                                                    convertgrade(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), dvsubload[subcode]["Subject_no"].ToString(), dvsubload[subcode]["exam_code"].ToString());
                                                    result = "P";
                                                }
                                                else if (string.Equals(Convert.ToString(dvsubload[subcode]["result"]).Trim().ToLower(), "sa"))
                                                {
                                                    result = "SA"; // check jairam
                                                    funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                }
                                                else if (Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "AAA")
                                                {
                                                    funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                    result = "A";
                                                }
                                                else
                                                {
                                                    qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                                    DataSet dsFailGrade = new DataSet();
                                                    dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                                    if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                                    {
                                                        if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                        {
                                                            funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        funcgrade = "-";
                                                    }
                                                    result = ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : Convert.ToString(dvsubload[subcode]["result"]).Trim());
                                                }
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"])) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks;
                                            if (stuinternalmarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "AA");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NE");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NR");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "L");
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"])) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            }
                                            if (stuexternammarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationAB);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNE);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNR);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationLT);
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks : valuationMP);
                                            }
                                            if (dvsubload[subcode]["total"].ToString() != "")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(dvsubload[subcode]["total"].ToString()), 0, MidpointRounding.AwayFromZero).ToString().Trim().PadLeft(2, '0'));
                                            }
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = funcgrade.ToString();
                                            if (passorfail.Trim().ToLower() == "pass" || chkfailshow.Checked == true)
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : Convert.ToString(dvsubload[subcode]["result"]).Trim())) : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : (passorfail.Trim().ToLower() == "pass") ? "P" : ((passorfail.Trim().ToUpper() == "AAA" || passorfail.Trim().ToUpper() == "AA" || passorfail.Trim().ToUpper() == "A" || passorfail.Trim().ToUpper() == "AB" || internalMarksValue == -1 || totalMarksValue == -1 || ExternalMarksValue == -1) ? "A" : Convert.ToString(dvsubload[subcode]["result"]).Trim())));
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                            }
                                            if (passorfail.Trim().ToLower() == "pass")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + spl_exmyr[1].ToString();
                                            }
                                            else
                                            {
                                                flag_stud_u = true;
                                            }
                                        }
                                    }
                                    else if (gradeflag == 2) //grade only
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks;
                                        if (stuinternalmarks.ToString().Trim() == "-1")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "AA");
                                        }
                                        else if (stuinternalmarks.ToString().Trim() == "-2")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NE");
                                        }
                                        else if (stuinternalmarks.ToString().Trim() == "-3")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NR");
                                        }
                                        else if (stuinternalmarks.ToString().Trim() == "-4")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "L");
                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                        }
                                        if (stuexternammarks.ToString().Trim() == "-1")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationAB);
                                        }
                                        else if (stuexternammarks.ToString().Trim() == "-2")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNE);
                                        }
                                        else if (stuexternammarks.ToString().Trim() == "-3")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNR);
                                        }
                                        else if (stuexternammarks.ToString().Trim() == "-4")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationLT);
                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks : valuationMP);
                                        }
                                        if (dvsubload[subcode]["total"].ToString().Trim() != "")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(totalMarksValue.ToString().Trim()), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0'));
                                        }
                                        if (Convert.ToString(dvsubload[subcode]["result"]).ToUpper().Trim() == "AAA")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = "A";
                                        }
                                        else
                                        {
                                            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "MP") : (passorfail.Trim().ToLower() == "pass") ? "P" : Convert.ToString(dvsubload[subcode]["result"]).Trim());
                                            // check jairam
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                        }
                                        if (passorfail.Trim().ToLower() == "pass" || chkfailshow.Checked == true)
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : Convert.ToString(dvsubload[subcode]["result"]).Trim())) : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : (passorfail.Trim().ToLower() == "pass") ? "P" : ((passorfail.Trim().ToUpper() == "AAA" || passorfail.Trim().ToUpper() == "AA" || passorfail.Trim().ToUpper() == "A" || passorfail.Trim().ToUpper() == "AB" || internalMarksValue == -1 || totalMarksValue == -1 || ExternalMarksValue == -1) ? "A" : Convert.ToString(dvsubload[subcode]["result"]).Trim())));

                                        }
                                        else
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                        }
                                        if (passorfail.Trim().ToLower() == "pass")
                                        {
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + spl_exmyr[1].ToString();
                                        }
                                        else
                                        {
                                            flag_stud_u = true;
                                        }
                                    }
                                    else if (gradeflag == 3) //mark ly
                                    {
                                        if (grade_setting == "0") // display the marks
                                        {
                                            string internalMarks = dvsubload[subcode]["internal_mark"].ToString().Trim().PadLeft(2, '0');
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks;
                                            if (stuinternalmarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "AA");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NE");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NR");
                                            }
                                            else if (stuinternalmarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "L");
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0'));
                                            }
                                            if (stuexternammarks.ToString().Trim() == "-1")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationAB);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-2")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNE);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-3")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNR);
                                            }
                                            else if (stuexternammarks.ToString().Trim() == "-4")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationLT);
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["external_mark"]).Trim()) || string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : Convert.ToString(dvsubload[subcode]["external_mark"]).Trim().PadLeft(2, '0')) + valuationMarks : valuationMP);
                                            }
                                            if (dvsubload[subcode]["total"].ToString() != "")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(totalMarksValue.ToString().Trim()), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0'));
                                                if (stuinternalmarks.Trim() != "" && stuexternammarks.ToString().Trim() != "")
                                                {
                                                    Double intmarkchkek = Convert.ToDouble(stuinternalmarks);
                                                    Double extmarkchkek = Convert.ToDouble(stuexternammarks);
                                                    if (intmarkchkek < 0 && extmarkchkek < 0)
                                                    {
                                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : "00");
                                                    }
                                                    else if (intmarkchkek < 0 && extmarkchkek >= 0)
                                                    {
                                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(extmarkchkek), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0'));
                                                    }
                                                    else if (intmarkchkek >= 0 && extmarkchkek < 0)
                                                    {
                                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(intmarkchkek), 0, MidpointRounding.AwayFromZero).ToString().PadLeft(2, '0'));
                                                    }
                                                }
                                            }
                                            if (Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "AAA" || Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "AA" || Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "A" || Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "AB")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = "A";
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : ((string.IsNullOrEmpty(Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim())) ? "--" : Convert.ToString(dvsubload[subcode]["internal_mark"]).Trim().PadLeft(2, '0')));
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = dvsubload[subcode]["grade"].ToString();
                                            }
                                            if (passorfail.Trim().ToLower() == "pass" || chkfailshow.Checked == true)
                                            {
                                                string strgetresul = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                if (strgetresul.Trim().ToLower() == "pass")
                                                {
                                                    strgetresul = "P";
                                                }
                                                else if (string.Equals(strgetresul.Trim().ToLower(), "sa"))
                                                {
                                                    strgetresul = "sa";
                                                }
                                                else if (strgetresul.Trim().ToLower() == "fail")
                                                {
                                                    strgetresul = ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "F");
                                                }
                                                else if (strgetresul.Trim().ToLower() == "aaa" || strgetresul.Trim().ToLower() == "aa" || strgetresul.Trim().ToLower() == "a" || strgetresul.Trim().ToLower() == "ab")
                                                {
                                                    strgetresul = "A";
                                                }
                                                if (isMalPractice)
                                                {
                                                    strgetresul = ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "MP");
                                                }
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : Convert.ToString(dvsubload[subcode]["result"]).Trim())) : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : (passorfail.Trim().ToLower() == "pass") ? "P" : ((strgetresul.Trim().ToUpper() == "AAA" || strgetresul.Trim().ToUpper() == "AA" || strgetresul.Trim().ToUpper() == "A" || strgetresul.Trim().ToUpper() == "AB" || internalMarksValue == -1 || totalMarksValue == -1 || ExternalMarksValue == -1) ? "A" : Convert.ToString(dvsubload[subcode]["result"]).Trim())));
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                            }
                                            if (passorfail.Trim().ToLower() == "pass")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + spl_exmyr[1].ToString();
                                            }
                                            else
                                            {
                                                flag_stud_u = true;
                                            }
                                        }
                                        else
                                        {
                                            if (flagvetri == true)
                                            {
                                                attmtreal = Convert.ToString(dvsubload[subcode]["attempts"].ToString());
                                                inte = Convert.ToDouble(dvsubload[subcode]["internal_mark"].ToString());
                                                exte = Convert.ToDouble(dvsubload[subcode]["external_mark"].ToString());
                                                if (attmpt > Convert.ToInt32(attmtreal))
                                                {
                                                    if (dvsubload[subcode]["internal_mark"].ToString() != string.Empty && dvsubload[subcode]["external_mark"].ToString() != string.Empty)
                                                    {
                                                        if (Convert.ToDouble(dvsubload[subcode]["internal_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_int_marks"].ToString()) && Convert.ToDouble(dvsubload[subcode]["external_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_ext_marks"].ToString()))
                                                        {
                                                            convertgradev(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), dvsubload[subcode]["Subject_no"].ToString(), dvsubload[subcode]["exam_code"].ToString(), maxmarkve, attmpt);
                                                            result = "P";
                                                        }
                                                        else if (string.Equals(Convert.ToString(dvsubload[subcode]["result"]).Trim().ToLower(), "sa"))
                                                        {
                                                            result = "SA";
                                                            funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                        }
                                                        else if (Convert.ToString(dvsubload[subcode]["result"]) == "AAA")
                                                        {
                                                            funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                            result = "A";
                                                        }
                                                        else
                                                        {
                                                            qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                                            DataSet dsFailGrade = new DataSet();
                                                            dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                                            if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                                            {
                                                                if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                                {
                                                                    funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                funcgrade = "-";
                                                            }
                                                            result = ((string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "F");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (maxmarkve <= exte)
                                                    {
                                                        convertgradev(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), dvsubload[subcode]["Subject_No"].ToString(), dvsubload[subcode]["exam_code"].ToString(), maxmarkve, attmpt);
                                                        result = "P";
                                                    }
                                                    else if (string.Equals(Convert.ToString(dvsubload[subcode]["result"]).Trim().ToLower(), "sa"))
                                                    {
                                                        result = "SA";
                                                        funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                    }
                                                    else if (Convert.ToString(dvsubload[subcode]["result"]) == "AAA")
                                                    {
                                                        funcgrade = Convert.ToString(dvsubload[subcode]["result"]);
                                                        result = "A";
                                                    }
                                                    else
                                                    {
                                                        qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                                        DataSet dsFailGrade = new DataSet();
                                                        dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                                        if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                            {
                                                                funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            funcgrade = "-";
                                                        }
                                                        result = ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "F");
                                                        inte = 0;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (dvsubload[subcode]["internal_mark"].ToString() != string.Empty && dvsubload[subcode]["external_mark"].ToString() != string.Empty)
                                                {
                                                    inte = Convert.ToDouble(dvsubload[subcode]["internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvsubload[subcode]["external_mark"].ToString());
                                                    if (Convert.ToDouble(dvsubload[subcode]["internal_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_int_marks"].ToString()) && Convert.ToDouble(dvsubload[subcode]["external_mark"].ToString()) >= Convert.ToDouble(dvsubload[subcode]["min_ext_marks"].ToString()))
                                                    {
                                                        convertgrade(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), dvsubload[subcode]["Subject_no"].ToString(), dvsubload[subcode]["exam_code"].ToString());
                                                        result = "P";
                                                    }
                                                    else if (string.Equals(Convert.ToString(dvsubload[subcode]["result"]).Trim().ToLower(), "sa"))
                                                    {
                                                        result = "SA";
                                                        funcgrade = Convert.ToString(dvsubload[subcode]["result"]).Trim();
                                                    }
                                                    else if (Convert.ToString(dvsubload[subcode]["result"]).Trim().ToUpper() == "AAA")
                                                    {
                                                        funcgrade = Convert.ToString(dvsubload[subcode]["result"]);
                                                        result = "A";
                                                    }
                                                    else
                                                    {
                                                        qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                                        DataSet dsFailGrade = new DataSet();
                                                        dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                                        if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                                        {
                                                            if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                            {
                                                                funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                            }
                                                        }
                                                        else
                                                        {
                                                            funcgrade = "-";
                                                        }
                                                        result = ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : "F");
                                                    }
                                                }
                                            }
                                            if (attmpt > Convert.ToInt32(attmtreal))
                                            {
                                                if (inte.ToString().Trim() == "-1")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "AA");
                                                }
                                                else if (inte.ToString().Trim() == "-2")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NE");
                                                }
                                                else if (inte.ToString().Trim() == "-3")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "NR");
                                                }
                                                else if (inte.ToString().Trim() == "-4")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : "L");
                                                }
                                                else
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : inte.ToString().Trim().PadLeft(2, '0'));
                                                }
                                                if (exte.ToString().Trim() == "-1")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationAB);
                                                }
                                                else if (exte.ToString().Trim() == "-2")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNE);
                                                }
                                                else if (exte.ToString().Trim() == "-3")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationNR);
                                                }
                                                else if (exte.ToString().Trim() == "-4")
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : valuationLT);
                                                }
                                                else
                                                {
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : exte.ToString().Trim().PadLeft(2, '0') + valuationMarks) : valuationMP);
                                                }
                                                if (dvsubload[subcode]["total"].ToString().Trim() != "")
                                                {
                                                    double total = Convert.ToDouble((inte) + (exte));
                                                    if (exte < 0)
                                                    {
                                                        total = Convert.ToDouble(inte);
                                                    }
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : Math.Round(Convert.ToDouble(total), 0, MidpointRounding.AwayFromZero).ToString().Trim().PadLeft(2, '0'));
                                                }
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = ((string.IsNullOrEmpty(Convert.ToString(maxInternalMarks).Trim()) || Convert.ToString(maxInternalMarks).Trim() == "0") ? "--" : inte.ToString().Trim().PadLeft(2, '0'));
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = (internalOnly) ? valuationExtOnly : ((!isMalPractice) ? ((string.IsNullOrEmpty(Convert.ToString(maxExternalMarks).Trim()) || Convert.ToString(maxExternalMarks).Trim() == "0") ? "--" : exte.ToString().Trim().PadLeft(2, '0') + valuationMarks) : valuationMP);
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = ((!string.IsNullOrEmpty(totalValues)) ? totalValues : exte.ToString().Trim().PadLeft(2, '0'));
                                            }

                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = funcgrade.ToString();
                                            if (passorfail.Trim().ToLower() == "pass" || chkfailshow.Checked == true)
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = ((isMalPractice) ? ((!string.IsNullOrEmpty(Convert.ToString(txtFailValue.Text).Trim())) ? Convert.ToString(txtFailValue.Text).Trim() : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : Convert.ToString(dvsubload[subcode]["result"]).Trim())) : ((!string.IsNullOrEmpty(resultSubject)) ? resultSubject : (passorfail.Trim().ToLower() == "pass") ? "P" : ((passorfail.Trim().ToUpper() == "AAA" || passorfail.Trim().ToUpper() == "AA" || passorfail.Trim().ToUpper() == "A" || passorfail.Trim().ToUpper() == "AB" || internalMarksValue == -1 || totalMarksValue == -1 || ExternalMarksValue == -1) ? "A" : Convert.ToString(dvsubload[subcode]["result"]).Trim())));
                                            }
                                            else
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                            }
                                            if (passorfail.Trim().ToLower() == "pass")
                                            {
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + spl_exmyr[1].ToString();
                                            }
                                            else
                                            {
                                                flag_stud_u = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = "--";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = "--";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = "--";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = "--";
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = string.Empty;
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = strExam_month + exam_year;
                                    }
                                    if ((11 + sub_increment) == (FpExternal.Sheets[0].ColumnCount - 3))
                                    {
                                        if (flag_subj_rowcnt == false)
                                        {
                                            find_subjrow_count++;
                                        }
                                        sub_increment = 1;
                                    }
                                    else
                                    {
                                        sub_increment += 7;
                                    }
                                }
                                double gpsround = 0;
                                if (chk_subjectwisegrade.Checked == true)
                                {
                                    int exammothhs = Convert.ToInt32(ddlMonth.SelectedIndex.ToString());
                                    exammothhs = exammothhs + 1;
                                    string gpa = Calulat_GPA_cgpaformate1(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), ddlBranch.SelectedValue.ToString(), ddlBatch.SelectedItem.Text.ToString(), exammothhs.ToString(), ddlYear.SelectedItem.Text.ToString(), Session["collegecode"].ToString());
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 26].Text = Convert.ToString(gpa);
                                    string cgpa = Calulat_CGPA_cgpaformate1(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), ddlSemYr.SelectedValue.ToString(), ddlBranch.SelectedValue.ToString(), ddlBatch.SelectedItem.Text.ToString(), Session["collegecode"].ToString());
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 27].Text = cgpa;
                                }
                                else
                                {
                                    if (flag_stud_u == false)
                                    {
                                        string gpa = Calulat_GPA(dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString(), ddlSemYr.SelectedValue.ToString());
                                        gpsround = Convert.ToDouble(gpa);
                                        gpsround = Math.Round(gpsround, 2, MidpointRounding.AwayFromZero);
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 26].Text = Convert.ToString(gpsround);
                                    }
                                    else
                                    {
                                        // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 26].Text = "0.00";
                                    }
                                }
                                #region Added By Malang Raja

                                double spanCount = Math.Ceiling(Convert.ToDouble(dvsubload.Count) / 3);
                                spanRowCount = Convert.ToInt32(spanCount);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 0, spanRowCount, 1);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 1, spanRowCount, 1);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 2, spanRowCount, 1);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 3, spanRowCount, 1);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 4, spanRowCount, 1);
                                FpExternal.Sheets[0].AddSpanCell(studRowCount, 5, spanRowCount, 1);

                                #endregion Added By Malang Raja

                                flag_subj_rowcnt = true;
                                if (rb2.Checked)
                                {
                                    FpExternal.Sheets[0].RowCount += 2;
                                }
                                if (rb1.Checked)
                                {
                                    FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 0].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 1].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 2].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 3].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 4].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[studRowCount, 5].Border.BorderSize = 1;
                                }
                                else
                                {
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderSize = 1;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.Black;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderSize = 1;
                                }
                                FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderColorBottom = Color.Black;
                                FpExternal.Sheets[0].Rows[FpExternal.Sheets[0].RowCount - 1].Border.BorderSize = 1;
                            }
                        }
                    }
                }
                else
                {
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public string GetMonth(string strmon)
    {
        string strExam_month = string.Empty;
        strmon = strmon.Trim();
        switch (strmon)
        {
            case "1":
                strExam_month = "J ";
                break;
            case "2":
                strExam_month = "F ";
                break;
            case "3":
                strExam_month = "M ";
                break;
            case "4":
                strExam_month = "A ";
                break;
            case "5":
                strExam_month = "M ";
                break;
            case "6":
                strExam_month = "J ";
                break;
            case "7":
                strExam_month = "J ";
                break;
            case "8":
                strExam_month = "A ";
                break;
            case "9":
                strExam_month = "S ";
                break;
            case "10":
                strExam_month = "O ";
                break;
            case "11":
                strExam_month = "N ";
                break;
            case "12":
                strExam_month = "D ";
                break;
        }
        return strExam_month;
    }

    public void convertgrade(string roll, string subj, string examCode)
    {
        strexam = "Select subject_name,subject_code,total,result,cp,m.subject_no from Mark_Entry m,Subject,sub_sem where m.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code in(" + examCode + ")  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";
        DataSet dsMarks = new DataSet();
        dsMarks = d2.select_method_wo_parameter(strexam, "Text");
        if (dsMarks.Tables.Count > 0 && dsMarks.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_convert in dsMarks.Tables[0].Rows)
            {
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                if (chkshowsub_name.Checked == true)
                {
                    funcsubcode = dr_convert["subject_name"].ToString();
                }
                else
                {
                    funcsubcode = dr_convert["subject_code"].ToString();
                }
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["total"].ToString();
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                if (dr_convert["total"].ToString() != string.Empty)
                {
                    strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_convert["total"] + " between frange and trange";
                }
                else
                {
                    strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                }
                DataSet dsGrade = new DataSet();
                dsGrade = d2.select_method_wo_parameter(strgrade, "Text");
                if (dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_grade in dsGrade.Tables[0].Rows)
                    {
                        funcgrade = dr_grade["mark_grade"].ToString();
                    }
                }
                else
                {
                    funcgrade = string.Empty;
                }
            }
        }
    }

    public void convertgradel(string roll, string subj, string examcodec)
    {
        strexam = "Select subject_name,subject_code,total,result,cp,m.subject_no from Mark_Entry m,Subject,sub_sem where m.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code in(" + examcodec + ")  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";
        DataSet dsMarks = new DataSet();
        dsMarks = d2.select_method_wo_parameter(strexam, "Text");
        if (dsMarks.Tables.Count > 0 && dsMarks.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_convert in dsMarks.Tables[0].Rows)
            {
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                if (chkshowsub_name.Checked == true)
                {
                    funcsubcode = dr_convert["subject_name"].ToString();
                }
                else
                {
                    funcsubcode = dr_convert["subject_code"].ToString();
                }
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["total"].ToString();
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                if (dr_convert["total"].ToString() != string.Empty)
                {
                    strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_convert["total"] + " between frange and trange";
                }
                else
                {
                    strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                }
                DataSet dsGrade = new DataSet();
                dsGrade = d2.select_method_wo_parameter(strgrade, "Text");
                if (dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_grade in dsGrade.Tables[0].Rows)
                    {
                        funcgrade = dr_grade["mark_grade"].ToString();
                    }
                }
                else
                {
                    funcgrade = string.Empty;
                }
            }
        }
    }

    void CalculateTotalPages()
    {
        Double totalRows = 0;
        totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
        Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
        Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        Buttontotal.Visible = true;
    }

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        string print = string.Empty;
        string appPath = HttpContext.Current.Server.MapPath("~");
        if (appPath != "")
        {
            int i = 1;
            appPath = appPath.Replace("\\", "/");
        e:
            try
            {
                print = "TMRReport" + i;
                string szPath = appPath + "/Report/";
                string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")
                FpExternal.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.WriteFile(szPath + szFile);
            }
            catch
            {
                goto e;
                i++;
            }
        }
    }

    public void convertgradev(string roll, string subj, string exmcode, int maxmarkve, int attmptreal)
    {
        strexam = "Select subject_name,subject_code,internal_mark,external_mark,attempts,total,result,cp,mark_entry.subject_no from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + exmcode + "  and roll_no='" + roll + "' and subject.subject_no=" + subj + string.Empty;
        double inte = 0, exte = 0;
        int attmpt = 0;
        DataSet dsMarks = new DataSet();
        dsMarks = d2.select_method_wo_parameter(strexam, "Text");
        if (dsMarks.Tables.Count > 0 && dsMarks.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_convert in dsMarks.Tables[0].Rows)
            {
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                if (chkshowsub_name.Checked == true)
                {
                    funcsubcode = dr_convert["subject_name"].ToString();
                }
                else
                {
                    funcsubcode = dr_convert["subject_code"].ToString();
                }
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["total"].ToString();
                inte = Convert.ToDouble(dr_convert["internal_mark"].ToString());
                exte = Convert.ToDouble(dr_convert["external_mark"].ToString());
                attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                if (attmptreal > attmpt)
                {
                    if (dr_convert["total"].ToString() != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_convert["total"] + " between frange and trange";
                    }
                    else
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                    }
                }
                else
                {
                    if (dr_convert["total"].ToString() != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + exte.ToString() + " between frange and trange";
                    }
                    else
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                    }
                }
                DataSet dsGrade = new DataSet();
                dsGrade = d2.select_method_wo_parameter(strgrade, "Text");
                if (dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_grade in dsGrade.Tables[0].Rows)
                    {
                        funcgrade = dr_grade["mark_grade"].ToString();
                    }
                }
                else
                {
                    funcgrade = string.Empty;
                }
            }
        }
    }

    public string GetMonthnov(string strmon)
    {
        if (strmon == "January")
            return "1";
        else if (strmon == "February")
            return "2";
        else if (strmon == "March")
            return "3";
        else if (strmon == "April")
            return "4";
        else if (strmon == "May")
            return "5 ";
        else if (strmon == "June")
            return "6";
        else if (strmon == "July")
            return "7";
        else if (strmon == "Auguest")
            return "8";
        else if (strmon == "September")
            return "9";
        else if (strmon == "Actober")
            return "10";
        else if (strmon == "November")
            return "11";
        else if (strmon == "December")
            return "12";
        else
            return string.Empty;
    }

    public void spl_function_load_headercons()
    {
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler5.ashx?";
        FpExternalHeader.Visible = true;
        FpExternalHeader.Sheets[0].ColumnHeader.RowCount = 0;
        FpExternalHeader.Sheets[0].ColumnCount = 0;
        FpExternalHeader.Sheets[0].ColumnCount = 26;
        FpExternalHeader.Sheets[0].ColumnHeader.RowCount = 6;
        FpExternalHeader.Sheets[0].RowHeader.Visible = false;
        FpExternalHeader.Sheets[0].AutoPostBack = true;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 0].Text = "S.No";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 1].Text = "RollNo";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 2].Text = "Regn.No";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Student Name";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Student Type";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 5].Text = "Semester";
        if (chkshowsub_name.Checked == true)
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subject Name";
            FpExternalHeader.Sheets[0].Columns[6].Width = 200;
            FpExternalHeader.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subcode";
            FpExternalHeader.Sheets[0].Columns[6].Width = 80;
        }
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subcode";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 7].Text = "INT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 8].Text = "EXT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 9].Text = "T";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 10].Text = "G";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 11].Text = "Y";
        if (chkshowsub_name.Checked == true)
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 12].Text = "Subject Name";
            FpExternalHeader.Sheets[0].Columns[12].Width = 200;
            FpExternalHeader.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 12].Text = "Subcode";
            FpExternalHeader.Sheets[0].Columns[12].Width = 80;
        }
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 13].Text = "INT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 14].Text = "EXT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 15].Text = "T";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 16].Text = "G";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 17].Text = "Y";
        if (chkshowsub_name.Checked == true)
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subject Name";
            FpExternalHeader.Sheets[0].Columns[18].Width = 200;
            FpExternalHeader.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subcode";
            FpExternalHeader.Sheets[0].Columns[18].Width = 80;
        }
        //  FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subcode";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 19].Text = "INT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 20].Text = "EXT";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 21].Text = "T";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 22].Text = "G";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 23].Text = "Y";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 24].Text = "GPA";
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 25].Text = "CGPA";
        if (rb1.Checked == true)
        {
            FpExternalHeader.Sheets[0].Columns[3].Font.Bold = false;
            FpExternalHeader.Sheets[0].Columns[10].Visible = true;
            FpExternalHeader.Sheets[0].Columns[16].Visible = true;
            FpExternalHeader.Sheets[0].Columns[22].Visible = true;
            FpExternalHeader.Sheets[0].Columns[11].Visible = true;
            FpExternalHeader.Sheets[0].Columns[17].Visible = true;
            FpExternalHeader.Sheets[0].Columns[23].Visible = true;
            FpExternalHeader.Sheets[0].Columns[25].Visible = true;
        }
        else
        {
            FpExternalHeader.Sheets[0].Columns[3].Font.Bold = true;
            FpExternalHeader.Sheets[0].Columns[10].Visible = false;
            FpExternalHeader.Sheets[0].Columns[16].Visible = false;
            FpExternalHeader.Sheets[0].Columns[22].Visible = false;
            FpExternalHeader.Sheets[0].Columns[11].Visible = false;
            FpExternalHeader.Sheets[0].Columns[17].Visible = false;
            FpExternalHeader.Sheets[0].Columns[23].Visible = false;
            FpExternalHeader.Sheets[0].Columns[25].Visible = false;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 7].Text = "I";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 8].Text = "E";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 13].Text = "I";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 14].Text = "E";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 19].Text = "I";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 20].Text = "E";
        }
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 12].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 13].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 14].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 15].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 16].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 18].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 19].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 20].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 21].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 22].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 23].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].Columns[0].Width = 100;
        FpExternalHeader.Sheets[0].Columns[1].Width = 100;
        FpExternalHeader.Sheets[0].Columns[2].Width = 100;
        FpExternalHeader.Sheets[0].Columns[3].Width = 280;
        FpExternalHeader.Sheets[0].Columns[4].Width = 150;
        FpExternalHeader.Sheets[0].Columns[5].Width = 80;
        FpExternalHeader.Sheets[0].Columns[7].Width = 30;
        FpExternalHeader.Sheets[0].Columns[8].Width = 30;
        FpExternalHeader.Sheets[0].Columns[9].Width = 30;
        FpExternalHeader.Sheets[0].Columns[10].Width = 30;
        FpExternalHeader.Sheets[0].Columns[11].Width = 50;
        FpExternalHeader.Sheets[0].Columns[13].Width = 30;
        FpExternalHeader.Sheets[0].Columns[14].Width = 30;
        FpExternalHeader.Sheets[0].Columns[15].Width = 30;
        FpExternalHeader.Sheets[0].Columns[16].Width = 30;
        FpExternalHeader.Sheets[0].Columns[17].Width = 50;
        FpExternalHeader.Sheets[0].Columns[19].Width = 30;
        FpExternalHeader.Sheets[0].Columns[20].Width = 30;
        FpExternalHeader.Sheets[0].Columns[21].Width = 30;
        FpExternalHeader.Sheets[0].Columns[22].Width = 30;
        FpExternalHeader.Sheets[0].Columns[23].Width = 90;
        FpExternalHeader.Sheets[0].Columns[24].Width = 100;
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website,isnull(address3,'-') as address3 from collinfo where college_code=" + Session["collegecode"] + string.Empty;
            DataSet dsCollegeDetails = new DataSet();
            dsCollegeDetails = d2.select_method_wo_parameter(str, "Text");
            if (dsCollegeDetails.Tables.Count > 0 && dsCollegeDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow collegename in dsCollegeDetails.Tables[0].Rows)
                {
                    collnamenew1 = collegename["collname"].ToString();
                    address1 = collegename["address1"].ToString();
                    address2 = collegename["address2"].ToString();
                    district = collegename["district"].ToString();
                    address3 = collegename["address3"].ToString();
                    address = address1 + "-" + address2 + "-" + district;
                    pincode = collegename["pincode"].ToString();
                    categery = collegename["category"].ToString();
                    Affliated = collegename["affliated"].ToString();
                    Phoneno = collegename["phoneno"].ToString();
                    Faxno = collegename["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                    email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                }
            }
        }
        string[] spsp = Affliated.Split(',');
        Affliated = string.Empty;
        for (int s = 0; s < spsp.GetUpperBound(0); s++)
        {
            if (Affliated == "")
            {
                Affliated = spsp[s].ToString();
            }
            else
            {
                Affliated = Affliated + "," + spsp[s].ToString();
            }
        }
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        style.ForeColor = Color.Black;
        style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        FpExternalHeader.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternalHeader.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternalHeader.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        column_count = FpExternalHeader.Sheets[0].ColumnCount;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorLeft = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorLeft = Color.Black;
        exam_month = ddlMonth.SelectedValue.ToString();
        exam_year = ddlYear.SelectedValue.ToString();
        string strExam_month = string.Empty;
        exam_month = exam_month.Trim();
        switch (exam_month)
        {
            case "1":
                strExam_month = "January";
                break;
            case "2":
                strExam_month = "February";
                break;
            case "3":
                strExam_month = "March";
                break;
            case "4":
                strExam_month = "April";
                break;
            case "5":
                strExam_month = "May";
                break;
            case "6":
                strExam_month = "June";
                break;
            case "7":
                strExam_month = "July";
                break;
            case "8":
                strExam_month = "Augest";
                break;
            case "9":
                strExam_month = "September";
                break;
            case "10":
                strExam_month = "October";
                break;
            case "11":
                strExam_month = "November";
                break;
            case "12":
                strExam_month = "December";
                break;
        }
        string rolflgvel = Session["Rollflag"].ToString();
        if (rolflgvel == "1")
        {
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 26);
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1 + ", " + address3 + "- " + pincode;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Algerian";
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Text = "( An " + categery + " Institution - Affiliated to " + Affliated + ".)";
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Text = Affliated;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, (FpExternalHeader.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColor = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            //'----------------------------------------------------new----------------------------
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Text = "TABULATED MARK REGISTER - " + strExam_month + " " + ddlYear.SelectedValue.ToString() + "  Examinations.";
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Result of the Semester Examination - " + strExam_month + " / " + ddlYear.SelectedValue.ToString() + string.Empty;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + "   " + ddlBranch.SelectedItem.ToString();
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 10);
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 11, 1, 11);
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 11].Border.BorderColorRight = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 7].Text = "Semester :" + ddlSemYr.SelectedItem.ToString(); //on 24.07.12
            FpExternalHeader.Sheets[0].ColumnHeader.Rows[FpExternalHeader.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
            //=========hide phoneno and email in column header row count
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Text = phnfax;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Text = email;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, column_count - 3);
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, column_count - 3);
            // FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, column_count - 3);
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.Black;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 1].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Left;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].HorizontalAlign = HorizontalAlign.Right;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 11].Border.BorderColorBottom = Color.Black;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].Font.Bold = true;
            //'-----------------------------------------------------------------------------------
        }
        else if (Session["Regflag"] == "1")
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Rows[FpExternalHeader.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
            //'---------------------------------------new
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1 + ", " + address3 + "- " + pincode;
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Algerian";
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Text = "( An " + categery + " Institution - Affiliated to " + Affliated + ".)";
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Text = Affliated;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, (FpExternalHeader.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColor = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Text = "TABULATED MARK REGISTER - " + strExam_month + "  " + ddlYear.SelectedValue.ToString() + "Examinations.";
            }
            else
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Result of the Semester Examination - " + strExam_month + " / " + ddlYear.SelectedValue.ToString() + string.Empty;
            }
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
            if (rb1.Checked == true)
            {
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " & " + ddlBranch.SelectedItem.ToString();
            }
            else
            {
                int batch = Convert.ToInt32(ddlBatch.SelectedItem.ToString()) + (ddlSemYr.Items.Count / 2);
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Batch : " + ddlBatch.SelectedItem.ToString() + " - " + batch;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 5].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " & " + ddlBranch.SelectedItem.ToString();
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 5].Font.Bold = true;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 5].HorizontalAlign = HorizontalAlign.Left;
                FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 5].Font.Size = FontUnit.Medium;
            }
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, 3);
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 5, 1, 13);
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 11, 1, 11);
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].Border.BorderColorRight = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 7].Text = "Semester :" + ddlSemYr.SelectedItem.ToString();//on 24.07.12
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 5].Border.BorderColorBottom = Color.Black;
            //=========hide phoneno and email in column header row count
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Text = phnfax;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Text = email;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
            //FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, column_count - 3);
            FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, column_count - 3);
            // FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, column_count - 3);
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.Black;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].HorizontalAlign = HorizontalAlign.Right;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].Font.Size = FontUnit.Medium;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].HorizontalAlign = HorizontalAlign.Left;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[3, 2].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 2].Font.Bold = true;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 9].Font.Bold = true;
            //'-----------------------------------------------------------------------------------
        }
        FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);//'----------spaning for logo
        //FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternalHeader.Sheets[0].ColumnCount - 1, 1, 1);
        FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, 23, 5, 1);
        //   FpExternalHeader.Sheets[0].ColumnHeaderSpanModel.Add(0, 22, 5, 1);//hided on 31.07.12
        //        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;//hided on 24.07.12
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 23].CellType = mi2;//hided on 24.07.12
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, FpExternalHeader.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorLeft = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.White;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
        // FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorTop = Color.Black;
        //   FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, FpExternalHeader.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[2].Border.BorderColor = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[3].Border.BorderColor = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
        FpExternalHeader.Sheets[0].ColumnHeader.Rows[5].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 5].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 7].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 9].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 10].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 11].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 12].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 13].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 14].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 15].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 16].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 17].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 18].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 19].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 20].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 21].Border.BorderColorBottom = Color.Black;
        //FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 22].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, FpExternalHeader.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 5].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 7].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 9].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 10].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 11].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 12].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 13].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 14].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 15].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 16].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 17].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 18].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 19].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 20].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 21].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 22].Border.BorderColorRight = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 23].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 24].Border.BorderColorBottom = Color.White;
        //   FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 23].Border.BorderColorTop = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 23].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 22].Border.BorderColorTop = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 22].Border.BorderColorBottom = Color.Black;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[1, 24].Border.BorderColorTop = Color.White;
        FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 24].Border.BorderColorBottom = Color.Black;
        if (rdotmr1.Checked == true)
        {
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 25].Text = "CGPA";
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[4, 25].Border.BorderColorBottom = Color.Black;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 25].Border.BorderColorTop = Color.Black;
            FpExternalHeader.Sheets[0].ColumnHeader.Cells[5, 25].Border.BorderColorBottom = Color.White;
        }
    }

    public void function_load_headercons()
    {
        MyImg mi = new MyImg();
        mi.ImageUrl = "~/images/10BIT001.jpeg";
        mi.ImageUrl = "Handler2.ashx?";
        MyImg mi2 = new MyImg();
        mi2.ImageUrl = "~/images/10BIT001.jpeg";
        mi2.ImageUrl = "Handler5.ashx?";
        FpExternal.Visible = true; FpExternalHeader.Visible = true;
        FpExternal.Sheets[0].ColumnHeader.RowCount = 0;
        FpExternal.Sheets[0].ColumnCount = 0;
        FpExternal.Sheets[0].ColumnCount = 26;//count changed from 25
        FpExternal.Sheets[0].ColumnHeader.RowCount = 6;
        FpExternal.Sheets[0].RowHeader.Visible = false;
        FpExternal.Sheets[0].AutoPostBack = true;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Text = "S.No";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Text = "RollNo";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 2].Text = "Regn.No";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 3].Text = "Student Name";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Student Type";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 5].Text = "Semester";
        if (chkshowsub_name.Checked == true)
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subject Name";
            FpExternal.Sheets[0].Columns[6].Width = 200;
            FpExternal.Sheets[0].Columns[6].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subcode";
            FpExternal.Sheets[0].Columns[6].Width = 80;
        }
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Text = "Subcode";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 7].Text = "INT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Text = "EXT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 9].Text = "T";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 10].Text = "G";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 11].Text = "Y";
        if (chkshowsub_name.Checked == true)
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 12].Text = "Subject Name";
            FpExternal.Sheets[0].Columns[12].Width = 200;
            FpExternal.Sheets[0].Columns[12].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 12].Text = "Subcode";
            FpExternal.Sheets[0].Columns[12].Width = 80;
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 13].Text = "INT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 14].Text = "EXT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 15].Text = "T";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 16].Text = "G";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 17].Text = "Y";
        if (chkshowsub_name.Checked == true)
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subject Name";
            FpExternal.Sheets[0].Columns[18].Width = 200;
            FpExternal.Sheets[0].Columns[18].HorizontalAlign = HorizontalAlign.Left;
        }
        else
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subcode";
            FpExternal.Sheets[0].Columns[18].Width = 80;
        }
        //  FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Text = "Subcode";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 19].Text = "INT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Text = "EXT";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 21].Text = "T";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 22].Text = "G";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 23].Text = "Y";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 24].Text = "GPA";
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 25].Text = "CGPA";
        if (rb1.Checked == true)
        {
            FpExternal.Sheets[0].Columns[3].Font.Bold = false;
            FpExternal.Sheets[0].Columns[10].Visible = true;
            FpExternal.Sheets[0].Columns[16].Visible = true;
            FpExternal.Sheets[0].Columns[22].Visible = true;
            FpExternal.Sheets[0].Columns[11].Visible = true;
            FpExternal.Sheets[0].Columns[17].Visible = true;
            FpExternal.Sheets[0].Columns[23].Visible = true;
            FpExternal.Sheets[0].Columns[25].Visible = true;
        }
        else
        {
            FpExternal.Sheets[0].Columns[3].Font.Bold = true;
            FpExternal.Sheets[0].Columns[10].Visible = false;
            FpExternal.Sheets[0].Columns[16].Visible = false;
            FpExternal.Sheets[0].Columns[22].Visible = false;
            FpExternal.Sheets[0].Columns[11].Visible = false;
            FpExternal.Sheets[0].Columns[17].Visible = false;
            FpExternal.Sheets[0].Columns[23].Visible = false;
            FpExternal.Sheets[0].Columns[25].Visible = false;
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 7].Text = "I";
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Text = "E";
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 13].Text = "I";
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 14].Text = "E";
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 19].Text = "I";
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Text = "E";
        }
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 12].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 13].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 14].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 15].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 16].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 18].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 19].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 20].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 21].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 22].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 23].Border.BorderColorBottom = Color.Black;
        //==================set the column width
        FpExternal.Sheets[0].Columns[0].Width = 100;
        FpExternal.Sheets[0].Columns[1].Width = 100;
        FpExternal.Sheets[0].Columns[2].Width = 100;
        FpExternal.Sheets[0].Columns[3].Width = 280;
        FpExternal.Sheets[0].Columns[4].Width = 150;
        FpExternal.Sheets[0].Columns[5].Width = 80;
        FpExternal.Sheets[0].Columns[7].Width = 30;
        FpExternal.Sheets[0].Columns[8].Width = 30;
        FpExternal.Sheets[0].Columns[9].Width = 30;
        FpExternal.Sheets[0].Columns[10].Width = 30;
        FpExternal.Sheets[0].Columns[11].Width = 50;
        FpExternal.Sheets[0].Columns[13].Width = 30;
        FpExternal.Sheets[0].Columns[14].Width = 30;
        FpExternal.Sheets[0].Columns[15].Width = 30;
        FpExternal.Sheets[0].Columns[16].Width = 30;
        FpExternal.Sheets[0].Columns[17].Width = 50;
        FpExternal.Sheets[0].Columns[19].Width = 30;
        FpExternal.Sheets[0].Columns[20].Width = 30;
        FpExternal.Sheets[0].Columns[21].Width = 30;
        FpExternal.Sheets[0].Columns[22].Width = 30;
        FpExternal.Sheets[0].Columns[23].Width = 90;
        FpExternal.Sheets[0].Columns[24].Width = 100;
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website,isnull(address3,'-') as address3 from collinfo where college_code=" + Session["collegecode"] + string.Empty;
            DataSet dsCollegeDetails = new DataSet();
            dsCollegeDetails = d2.select_method_wo_parameter(str, "Text");
            if (dsCollegeDetails.Tables.Count > 0 && dsCollegeDetails.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow collegename in dsCollegeDetails.Tables[0].Rows)
                {
                    collnamenew1 = collegename["collname"].ToString();
                    address1 = collegename["address1"].ToString();
                    address2 = collegename["address2"].ToString();
                    district = collegename["district"].ToString();
                    address3 = collegename["address3"].ToString();
                    address = address1 + "-" + address2 + "-" + district;
                    pincode = collegename["pincode"].ToString();
                    categery = collegename["category"].ToString();
                    Affliated = collegename["affliated"].ToString();
                    Phoneno = collegename["phoneno"].ToString();
                    Faxno = collegename["faxno"].ToString();
                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                    email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                }
            }
        }
        string[] spsp = Affliated.Split(',');
        Affliated = string.Empty;
        for (int s = 0; s < spsp.GetUpperBound(0); s++)
        {
            if (Affliated == "")
            {
                Affliated = spsp[s].ToString();
            }
            else
            {
                Affliated = Affliated + "," + spsp[s].ToString();
            }
        }
        FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
        style.Font.Size = 10;
        style.Font.Bold = true;
        style.ForeColor = Color.Black;
        style.BackColor = ColorTranslator.FromHtml("#0CA6CA");
        FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
        FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
        column_count = FpExternal.Sheets[0].ColumnCount;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[2, 0].Border.BorderColorLeft = Color.Black;
        exam_month = ddlMonth.SelectedValue.ToString();
        exam_year = ddlYear.SelectedValue.ToString();
        string strExam_month = string.Empty;
        exam_month = exam_month.Trim();
        switch (exam_month)
        {
            case "1":
                strExam_month = "January";
                break;
            case "2":
                strExam_month = "February";
                break;
            case "3":
                strExam_month = "March";
                break;
            case "4":
                strExam_month = "April";
                break;
            case "5":
                strExam_month = "May";
                break;
            case "6":
                strExam_month = "June";
                break;
            case "7":
                strExam_month = "July";
                break;
            case "8":
                strExam_month = "Augest";
                break;
            case "9":
                strExam_month = "September";
                break;
            case "10":
                strExam_month = "October";
                break;
            case "11":
                strExam_month = "November";
                break;
            case "12":
                strExam_month = "December";
                break;
        }
        string rolflgvel = Session["Rollflag"].ToString();
        if (rolflgvel == "1")
        {
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 26);
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1 + ", " + address3 + "- " + pincode;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = collnamenew1;
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Name = "Algerian";
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorLeft = Color.White;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Text = "( An " + categery + " Institution - Affiliated to " + Affliated + ".)";
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Text = Affliated;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColor = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].HorizontalAlign = HorizontalAlign.Center;
            //'----------------------------------------------------new----------------------------
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Text = "TABULATED MARK REGISTER - " + strExam_month + " " + ddlYear.SelectedValue.ToString() + "  Examinations."; //address;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Text = "Result of the Semester Examination - " + strExam_month + " / " + ddlYear.SelectedValue.ToString() + string.Empty; //address;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + "   " + ddlBranch.SelectedItem.ToString();
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 10);//4,1,1,6
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 11, 1, 11);//4,7,1,15
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 11].Border.BorderColorRight = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 7].Text = "Semester :" + ddlSemYr.SelectedItem.ToString(); //on 24.07.12
            FpExternal.Sheets[0].ColumnHeader.Rows[FpExternal.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
            //=========hide phoneno and email in column header row count
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Text = phnfax;
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Text = email;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, column_count - 3);
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, column_count - 3);
            // FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, column_count - 3);
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].HorizontalAlign = HorizontalAlign.Right;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 11].Border.BorderColorBottom = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].Font.Bold = true;
            //'-----------------------------------------------------------------------------------
        }
        else if (Session["Regflag"] == "1")
        {
            FpExternal.Sheets[0].ColumnHeader.Rows[FpExternal.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;
            //'---------------------------------------new
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1 + ", " + address3 + "- " + pincode;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text = collnamenew1;
                FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Font.Name = "Algerian";
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
            //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorTop = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].HorizontalAlign = HorizontalAlign.Center;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, column_count - 3);
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Text = "( An " + categery + " Institution - Affiliated to " + Affliated + ".)";
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Text = Affliated;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColor = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].HorizontalAlign = HorizontalAlign.Center;
            //'----------------------------------------------------new----------------------------
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Text = "TABULATED MARK REGISTER - " + strExam_month + "  " + ddlYear.SelectedValue.ToString() + "Examinations."; //address;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Text = "Result of the Semester Examination - " + strExam_month + " / " + ddlYear.SelectedValue.ToString() + string.Empty; //address;
            }
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Border.BorderColorRight = Color.White;
            if (rb1.Checked == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " & " + ddlBranch.SelectedItem.ToString();
            }
            else
            {
                int batch = Convert.ToInt32(ddlBatch.SelectedItem.ToString()) + (ddlSemYr.Items.Count / 2);
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Text = "Batch : " + ddlBatch.SelectedItem.ToString() + " - " + batch;
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 5].Text = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " & " + ddlBranch.SelectedItem.ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 5].Font.Bold = true;
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 5].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].ColumnHeader.Cells[4, 5].Font.Size = FontUnit.Medium;
            }
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, 3);
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 5, 1, 13);
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 11, 1, 11);
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].Border.BorderColorRight = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 7].Text = "Semester :" + ddlSemYr.SelectedItem.ToString();//on 24.07.12
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 5].Border.BorderColorBottom = Color.Black;
            //=========hide phoneno and email in column header row count
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Text = phnfax;
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorBottom = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Border.BorderColorRight = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Text = email;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.White;
            //FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, column_count - 3);
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, column_count - 3);
            // FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, column_count - 3);
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorBottom = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].HorizontalAlign = HorizontalAlign.Right;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 9].Font.Bold = true;
        }
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);//'----------spaning for logo
        //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 1, 1);
        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 23, 5, 1);
        //   FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 22, 5, 1);//hided on 31.07.12
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].CellType = mi;//hided on 24.07.12
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 23].CellType = mi2;//hided on 24.07.12
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].HorizontalAlign = HorizontalAlign.Center;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 0].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.White;
        //FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorTop = Color.Black;
        // FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorTop = Color.Black;
        //   FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Rows[3].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[1].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[2].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[2].Border.BorderColor = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[3].Border.BorderColor = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Rows[5].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].ColumnHeader.Rows[5].Font.Bold = true;
        FpExternal.Sheets[0].ColumnHeader.Rows[5].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 5].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 7].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 9].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 10].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 11].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 12].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 13].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 14].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 15].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 16].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 17].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 19].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 21].Border.BorderColorBottom = Color.Black;
        //FpExternal.Sheets[0].ColumnHeader.Cells[5, 22].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 2].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 2].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 3].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 5].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 7].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 9].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 10].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 11].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 12].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 13].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 14].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 15].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 16].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 17].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 19].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 21].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 22].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 23].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[5, 24].Border.BorderColorBottom = Color.White;
        //   FpExternal.Sheets[0].ColumnHeader.Cells[0, 23].Border.BorderColorTop = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 23].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[0, 22].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 22].Border.BorderColorTop = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 22].Border.BorderColorBottom = Color.Black;
        FpExternal.Sheets[0].ColumnHeader.Cells[1, 24].Border.BorderColorTop = Color.White;
        FpExternal.Sheets[0].ColumnHeader.Cells[4, 24].Border.BorderColorBottom = Color.Black;
        if (rdotmr1.Checked == true)
        {
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 25].Text = "CGPA";
            FpExternal.Sheets[0].ColumnHeader.Cells[4, 25].Border.BorderColorBottom = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 25].Border.BorderColorTop = Color.Black;
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 25].Border.BorderColorBottom = Color.White;
        }
    }

    private string Calulat_GPA(string RollNo, string sem)
    {
        int Subno = 0;
        int jvalue = 0;
        string examcodeval = string.Empty;
        string gradestr = string.Empty;
        string ccva = string.Empty;
        string strgrade = string.Empty;
        //  int creditval=0;
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        //  int creditsum1=0;
        double gpacal1 = 0;
        string strsubcrd = string.Empty;
        string graders = string.Empty;
        examcodeval = d2.GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = d2.GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            //attempts=1 marks
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts<=1";
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts<=1";
        }
        DataSet dsMarks = new DataSet();
        dsMarks = d2.select_method_wo_parameter(strsubcrd, "Text");
        if (dsMarks.Tables.Count > 0 && dsMarks.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr_subcrd in dsMarks.Tables[0].Rows)
            {
                if ((dr_subcrd["total"].ToString() != string.Empty))
                {
                    graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_subcrd["total"].ToString() + " between frange and trange";
                }
                else
                {
                    graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and credit_points between frange and trange";
                }
                DataSet dsGrade = new DataSet();
                dsGrade = d2.select_method_wo_parameter(graders, "text");
                if (dsGrade.Tables.Count > 0 && dsGrade.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr_grades in dsGrade.Tables[0].Rows)
                    {
                        strgrade = dr_grades["credit_points"].ToString();
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
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                        else
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                }
            }
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        return finalgpa1.ToString();
    }

    public void load_students_datacons()
    {
        string rollNO = string.Empty;
        string regNO = string.Empty;
        lblError.Visible = false;
        try
        {
            FpExternal.Visible = true; FpExternalHeader.Visible = true;
            FpExternal.Sheets[0].RowCount = 0;
            FpExternal.Sheets[0].ColumnCount = 26;
            //FpExternal.Sheets[0].ColumnCount = 25;
            FpExternal.Sheets[0].ColumnHeader.RowCount = 6;
            FpExternal.Sheets[0].RowHeader.Visible = false;
            string strStudents = string.Empty;
            string result = string.Empty;
            string sql = string.Empty;
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            string[] spl_exmyr = exam_year.Split('0');
            //-----------------------------------------------
            if (Session["Rollflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[1].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[4].Visible = false;
            }
            degree_code = ddlBranch.SelectedValue.ToString();
            semdec = Convert.ToInt32(ddlSemYr.SelectedValue.ToString());
            batch_year = ddlBatch.SelectedItem.ToString();
            section = string.Empty;
            if (ddlSec.Items.Count > 0)
            {
                section = ddlSec.SelectedItem.ToString().Trim();
                if (section != null && section != "-1" && section != "" && section.ToLower().Trim() != "all")
                {
                    section = "and registration.sections='" + section + "'";
                }
                else
                {
                    section = string.Empty;
                }
            }
            Boolean flag_stud_u = false;
            Boolean flag_subj_rowcnt = false;
            string inexamcode = string.Empty;
            DataSet dsexamcode = new DataSet();
            DataSet dsstudent = new DataSet();
            DataSet dsindimarkal = new DataSet();
            string semstr = daccess.GetFunction("select distinct max(current_semester) from registration where degree_code='" + degree_code + "' and batch_year='" + batch_year + "'");
            semdec = Convert.ToInt32(semstr);
            if (semdec > 0)
            {
                for (int b = 1; b <= semdec; b++)
                {
                    if (inexamcode == "")
                    {
                        inexamcode = b.ToString();
                    }
                    else
                    {
                        inexamcode = inexamcode + "," + b.ToString();
                    }
                }
            }
            if (inexamcode != string.Empty)
            {
                inexamcode = " in(" + inexamcode + ")";
            }
            string strcmdinexamcode = "Select Exam_Code from Exam_Details where Degree_Code ='" + degree_code + "' and Current_Semester " + inexamcode + " and Batch_Year = '" + batch_year + "'";
            dsexamcode.Dispose();
            dsexamcode.Reset();
            dsexamcode = daccess.select_method(strcmdinexamcode, hat, "");
            inexamcode = string.Empty;
            if (dsexamcode.Tables.Count > 0 && dsexamcode.Tables[0].Rows.Count > 0)
            {
                for (int b = 0; b < dsexamcode.Tables[0].Rows.Count; b++)
                {
                    if (inexamcode == "")
                    {
                        inexamcode = dsexamcode.Tables[0].Rows[b]["Exam_Code"].ToString();
                    }
                    else
                    {
                        inexamcode = inexamcode + "," + dsexamcode.Tables[0].Rows[b]["Exam_Code"].ToString();
                    }
                }
            }
            string strcmdstudent = "select distinct mark_entry.roll_no as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode,Current_Semester,sections from registration,mark_entry where mark_entry.roll_no=registration.roll_no " + section + "  and exam_code in(" + inexamcode + ") order by RgNo";
            dsstudent = daccess.select_method(strcmdstudent, hat, "");
            if (dsstudent.Tables.Count > 0 && dsstudent.Tables[0].Rows.Count > 0)
            {
                string getattmaxmark = daccess.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
                string[] semecount = getattmaxmark.Split(new Char[] { '-' });
                if (semecount.GetUpperBound(0) == 1)
                {
                    attmpt = Convert.ToInt32(semecount[0].ToString());
                    maxmarkve = Convert.ToInt32(semecount[1].ToString());
                    flagvetri = true;
                }
                else
                {
                    flagvetri = false;
                }
                for (int stud = 0; stud < dsstudent.Tables[0].Rows.Count; stud++)
                {
                    string strollno = dsstudent.Tables[0].Rows[stud]["RlNo"].ToString();
                    rollNO = strollno;
                    regNO = dsstudent.Tables[0].Rows[stud]["RgNo"].ToString();
                    string cursemester = dsstudent.Tables[0].Rows[stud]["Current_Semester"].ToString();
                    string latmode = dsstudent.Tables[0].Rows[stud]["mode"].ToString();
                    Boolean failflag = false;
                    string strgetindimarka = @"(select syllabus_master.semester,type,subject.subject_code,subject.min_ext_marks,subject.min_int_marks,subject.subject_name,(select datename(mm,str(exam_month) + '/01/2000') + str(exam_year) from exam_details where exam_code=me.exam_code) as mon_year,roll_no,subject.subtype_no,subject.subject_no,me.exam_code,me.internal_mark,me.external_mark,me.attempts,me.result from mark_entry as me,subject,sub_sem,syllabus_master where me.result<>'REJ'and sub_sem.subtype_no=subject.subtype_no  and total <= maxtotal and internal_mark <= max_int_marks and external_mark <= max_ext_marks  and roll_no in  ('@','" + strollno + "') and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no and me.exam_code in (-1," + inexamcode + ") and me.MYData = (select min(MYData) from mark_entry as me1 where me1.subject_no=me.subject_no and me1.roll_no=me.roll_no and me1.total=(select max(total) from mark_entry as me11 where me11.roll_no=me.roll_no and me11.subject_no=me.subject_no)) and me.subject_no not in (select subject_no from mark_entry where mark_entry.type='*' and mark_entry.roll_no=me.roll_no)) union (select syllabus_master.semester,type,subject.subject_code,subject.min_ext_marks,subject.min_int_marks,subject.subject_name,(select datename(mm,str(exam_month) + '/01/2000') + str(exam_year) from exam_details where exam_code=me.exam_code) as mon_year, roll_no,subject.subtype_no,subject.subject_no,me.exam_code,me.internal_mark,me.external_mark,ISNULL(me.attempts,'1') as attempts,me.result from mark_entry as me,subject,sub_sem,syllabus_master where me.result<>'REJ'and sub_sem.subtype_no=subject.subtype_no  and total <= maxtotal and internal_mark <= max_int_marks and external_mark <= max_ext_marks  and roll_no in  ('@','" + strollno + "') and syllabus_master.syll_code = subject.syll_code and subject.subject_no=me.subject_no and me.exam_code in (-1," + inexamcode + ") and me.MYData = (select max(MYData) from mark_entry as me1 where me1.subject_no= me.subject_no and me1.roll_no=me.roll_no) and me.type = '*' ) order by syllabus_master.semester,subject.subject_code";
                    dsindimarkal = daccess.select_method(strgetindimarka, hat, "");
                    if (dsindimarkal.Tables.Count > 0 && dsindimarkal.Tables[0].Rows.Count > 0)
                    {
                        serialno++;
                        FpExternal.Sheets[0].RowCount++;
                        FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                        FpExternal.Sheets[0].Columns[1].CellType = tt;
                        FpExternal.Sheets[0].Columns[2].CellType = tt;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Text = dsstudent.Tables[0].Rows[stud]["RlNo"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Tag = dsstudent.Tables[0].Rows[stud]["RlNo"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dsstudent.Tables[0].Rows[stud]["RgNo"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = dsstudent.Tables[0].Rows[stud]["SName"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = dsstudent.Tables[0].Rows[stud]["type"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = dsstudent.Tables[0].Rows[stud]["Current_Semester"].ToString();
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = serialno.ToString();
                        int sub_increment = 0;
                        string semestervfst = string.Empty;
                        for (int markind = 0; markind < dsindimarkal.Tables[0].Rows.Count; markind++)
                        {
                            string exam_month_year = string.Empty;
                            string exammthl = string.Empty;
                            string examyr = string.Empty;
                            string exammth = string.Empty;
                            string getmnthno = string.Empty;
                            double minEXT = 0;
                            //double maxEXT = 0;
                            double minINT = 0;
                            //double maxINT = 0;
                            double INT = 0;
                            double EXT = 0;
                            //double TOT = 0;

                            double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"]).Trim(), out minEXT);
                            //double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"]).Trim(), out maxEXT);
                            double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["min_int_marks"]).Trim(), out minINT);
                            //double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"]).Trim(), out maxINT);
                            double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["internal_mark"]).Trim(), out INT);
                            double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["external_mark"]).Trim(), out EXT);
                            //double.TryParse(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"]).Trim(), out TOT);
                            if (markind == 0)
                            {
                                semestervfst = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["semester"]).Trim();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["semester"]).Trim();
                            }
                            string semestervsec = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["semester"]).Trim();
                            string svexamcode = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["exam_code"]).Trim();
                            getsubno = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["Subject_No"]).Trim();
                            exam_month_year = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["mon_year"]).Trim();
                            attmtreal = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["attempts"]).Trim();
                            if (semestervfst != semestervsec)
                            {
                                FpExternal.Sheets[0].RowCount++;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Text = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["semester"]).Trim();
                                semestervfst = semestervsec;
                                sub_increment = 0;
                            }
                            string[] mnthyr = exam_month_year.Split(' ');
                            if (mnthyr.GetUpperBound(0) > 0)
                            {
                                exammth = mnthyr[0].ToString();
                                getmnthno = GetMonthnov(exammth);
                                exammthl = exammth.Substring(0, 1);
                                exammthl = exammthl + mnthyr[1].ToString();
                                examyr = mnthyr[(mnthyr.GetUpperBound(0))].ToString();
                                examyr = examyr.Substring(2, 2);
                            }
                            if (chkshowsub_name.Checked == true)
                            {
                                string getsubject = daccess.GetFunctionv("select top 1 subject_name from subject where subject_code='" + dsindimarkal.Tables[0].Rows[markind]["Subject_Code"].ToString() + "'");
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = getsubject;
                            }
                            else
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6 + sub_increment].Text = dsindimarkal.Tables[0].Rows[markind]["Subject_Code"].ToString();
                            }
                            if (dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString() != string.Empty && dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString() != string.Empty)
                            {
                                exte = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"].ToString());
                                inte = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_int_marks"].ToString());
                                itrnl = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString());
                                extrnl = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString());
                                if (flagvetri == true)
                                {
                                    if (semecount.GetUpperBound(0) == 1)
                                    {
                                        attmpt = Convert.ToInt32(semecount[0].ToString());
                                        maxmarkve = Convert.ToInt32(semecount[1].ToString());
                                    }
                                    if (attmpt > Convert.ToInt32(attmtreal))
                                    {
                                        if (Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString()) >= Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_int_marks"].ToString()) && Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString()) >= Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"].ToString()))
                                        {
                                            convertgradev(dsstudent.Tables[0].Rows[stud]["RlNo"].ToString(), dsindimarkal.Tables[0].Rows[markind]["Subject_no"].ToString(), dsindimarkal.Tables[0].Rows[markind]["exam_code"].ToString(), maxmarkve, attmpt);
                                            result = "P";
                                        }
                                        else if (string.Equals(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim().ToLower(), "sa"))
                                        {
                                            result = "SA";
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim();
                                        }
                                        else if (Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]) == "AAA")
                                        {
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]);
                                            result = "F";
                                            failflag = true;
                                        }
                                        else
                                        {
                                            qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                            DataSet dsFailGrade = new DataSet();
                                            dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                            if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                {
                                                    funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                }
                                            }
                                            else
                                            {
                                                funcgrade = "-";
                                            }
                                            result = "F";
                                            failflag = true;
                                        }
                                    }
                                    else
                                    {
                                        if (maxmarkve <= extrnl)
                                        {
                                            convertgradev(dsstudent.Tables[0].Rows[stud]["RlNo"].ToString(), dsindimarkal.Tables[0].Rows[markind]["Subject_no"].ToString(), dsindimarkal.Tables[0].Rows[markind]["exam_code"].ToString(), maxmarkve, attmpt);
                                            result = "P";
                                        }
                                        else if (string.Equals(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim().ToLower(), "sa"))
                                        {
                                            result = "SA";
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim();
                                        }
                                        else if (Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim().ToUpper() == "AAA")
                                        {
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]);
                                            result = "F";
                                            failflag = true;
                                        }
                                        else
                                        {
                                            qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                            DataSet dsFailGrade = new DataSet();
                                            dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                            if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                {
                                                    funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                }
                                            }
                                            else
                                            {
                                                funcgrade = "-";
                                            }
                                            result = "F";
                                            itrnl = 0;
                                            failflag = true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString() != string.Empty && dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString() != string.Empty)
                                    {
                                        inte = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString());
                                        exte = Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString());
                                        if (Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["internal_mark"].ToString()) >= Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_int_marks"].ToString()) && Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["external_mark"].ToString()) >= Convert.ToDouble(dsindimarkal.Tables[0].Rows[markind]["min_ext_marks"].ToString()))
                                        {
                                            convertgradel(dsstudent.Tables[0].Rows[stud]["RlNo"].ToString(), dsindimarkal.Tables[0].Rows[markind]["Subject_no"].ToString(), dsindimarkal.Tables[0].Rows[markind]["exam_code"].ToString());
                                            result = "P";
                                        }
                                        else if (string.Equals(Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim().ToLower(), "sa"))
                                        {
                                            result = "SA";
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim();
                                        }
                                        else if (Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]).Trim().ToUpper() == "AAA")
                                        {
                                            funcgrade = Convert.ToString(dsindimarkal.Tables[0].Rows[markind]["result"]);
                                            result = "F";
                                            failflag = true;
                                        }
                                        else
                                        {
                                            qry = "select value from COE_Master_Settings where settings='Fail Grade'";
                                            DataSet dsFailGrade = new DataSet();
                                            dsFailGrade = d2.select_method_wo_parameter(qry, "text");
                                            if (dsFailGrade.Tables.Count > 0 && dsFailGrade.Tables[0].Rows.Count > 0)
                                            {
                                                if (dsFailGrade.Tables[0].Rows[0]["value"] != null && string.IsNullOrEmpty(Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim()))
                                                {
                                                    funcgrade = Convert.ToString(dsFailGrade.Tables[0].Rows[0]["value"]).Trim();
                                                }
                                            }
                                            else
                                            {
                                                funcgrade = "-";
                                            }
                                            result = "F";
                                            failflag = true;
                                        }
                                    }
                                }
                            }
                            if (attmpt > Convert.ToInt32(attmtreal))
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = itrnl.ToString();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = extrnl.ToString();
                                {
                                    double v1 = Convert.ToDouble(itrnl);
                                    double v3 = Convert.ToDouble(extrnl);
                                    double totv = Convert.ToDouble((v1) + (v3));
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = Math.Round(Convert.ToDouble(totv), 0, MidpointRounding.AwayFromZero).ToString();
                                }
                            }
                            else
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 7 + sub_increment].Text = "0";
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 8 + sub_increment].Text = extrnl.ToString();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 9 + sub_increment].Text = extrnl.ToString();
                            }
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 10 + sub_increment].Text = funcgrade.ToString();
                            if (funcgrade != "U" && funcgrade != "AAA")//checked this condition as per nec clg asked .on 25.07.12
                            {
                                // to find last pass year
                                if (dsindimarkal.Tables[0].Rows[markind]["result"] == "Pass")
                                {
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = exammthl + " " + examyr;// //exam_year;
                                }
                                else
                                {
                                    //sql = "SELECT Exam_Code FROM Mark_Entry WHERE Result = 'Pass' AND Roll_No ='" + dsStudenList.Tables[0].Rows[stud]["RlNo"].ToString() + "' AND Subject_No=" + getsubno;
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 11 + sub_increment].Text = exammthl + " " + examyr;// //exam_year;
                                }
                            }
                            else
                            {
                                flag_stud_u = true;//26.07.12
                                failflag = true;//added by srinath 10/3/2014
                            }
                            // if ((10 + sub_increment) == (FpExternal.Sheets[0].ColumnCount - 3))
                            if ((10 + sub_increment) == (FpExternal.Sheets[0].ColumnCount - 4))
                            {
                                if (flag_subj_rowcnt == false)
                                {
                                    find_subjrow_count++;
                                }
                                FpExternal.Sheets[0].RowCount += 1;
                                sub_increment = 0;
                            }//subchkcnt
                            else
                            {
                                sub_increment += 6;
                            }
                        }
                        //modified by srinath 10/3/2014
                        string gpa = string.Empty;
                        if (failflag == false)
                        {
                            gpa = daccess.Calulat_GPA_Semwise(strollno, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                        }
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 24].Text = gpa.ToString();//changed to 24
                        string cgpa = daccess.Calculete_CGPA(strollno, cursemester, degree_code, batch_year, latmode, Session["collegecode"].ToString());
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 25].Text = cgpa.ToString();//changed to 24
                        FpExternal.Sheets[0].RowCount += 1;
                        flag_subj_rowcnt = true;
                    }
                }
            }
        }
        catch (Exception evel)
        {
            string vetri = evel.ToString();
            lblError.Text = evel.ToString() + " Roll No:" + rollNO + " Reg No:" + regNO;
            lblError.Visible = true;
            daccess.sendErrorMail(evel, Convert.ToString(Session["collegecode"]).Trim(), System.IO.Path.GetFileName(Request.Url.AbsolutePath).ToString());
        }
    }

    public void func_footercons()
    {
        string coe = string.Empty;
        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        {
            string str = "select address2,coe from collinfo where college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "'";
            DataSet dsCollege = new DataSet();
            dsCollege = d2.select_method_wo_parameter(str, "Text");
            if (dsCollege.Tables.Count > 0 && dsCollege.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow collegename in dsCollege.Tables[0].Rows)
                {
                    address2 = Convert.ToString(collegename["address2"]).Trim(); //Street
                    coe = Convert.ToString(collegename["coe"]).Trim();
                }
            }
        }
        FpExternal.Sheets[0].RowCount += 4;
        if (Convert.ToInt16(Session["Rollflag"]) == 1 && Convert.ToInt16(Session["Regflag"]) == 0)//clmn 2
        {
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 2);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = address2.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
            if (txtDate.Text != "")
            {
                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Date : " + txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        else if (Convert.ToInt16(Session["Regflag"]) == 1 && Convert.ToInt16(Session["Rollflag"]) == 0)//clmn 3
        {
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 3);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
            //    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = address2.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
            if (txtDate.Text != "")
            {
                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Date : " + txtDate.Text.ToString();/// +txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Right;
            }
        }
        else if (Convert.ToInt16(Session["Regflag"]) == 0 && Convert.ToInt16(Session["Rollflag"]) == 0)
        {
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 3);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 3);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = "Place :" + address2.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Right;
            if (txtDate.Text != "")
            {
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = "Date : ";/// +txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        else if (Convert.ToInt16(Session["Rollflag"]) == 1 && Convert.ToInt16(Session["Regflag"]) == 1)
        {
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 2);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 2);
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Text = "Place :" + address2.ToString();
            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Text = address2.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Left;
            if (txtDate.Text != "")
            {
                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Text = "Date : " + txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Border.BorderColorRight = Color.White;
        if (FpExternal.Sheets[0].ColumnCount > 6)
        {
            // FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Text = address2.ToString();
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Bold = true;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Left;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Border.BorderColorLeft = Color.White;
            if (txtDate.Text != "")
            {
                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Text = txtDate.Text.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Border.BorderColorLeft = Color.White;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 6].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].Border.BorderColorBottom = Color.White;
                //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 5, 1, 3);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 6, 1, FpExternal.Sheets[0].ColumnCount - 4);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 6, 1, FpExternal.Sheets[0].ColumnCount - 4);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorLeft = Color.White;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 5].Border.BorderColorBottom = Color.White;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 6].HorizontalAlign = HorizontalAlign.Left;
            }
        }
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Text = coe;
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 5, 1, FpExternal.Sheets[0].ColumnCount);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].HorizontalAlign = HorizontalAlign.Right;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 5].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Text = "Controller of Examinations";
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Right;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Border.BorderColorLeft = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Bold = true;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Font.Size = FontUnit.Medium;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].HorizontalAlign = HorizontalAlign.Left;
        //==========spanning rows
        //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 3);
        //FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 3);
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 0].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 0].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorRight = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Border.BorderColorBottom = Color.White;
        //     FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 5, 0, 1, FpExternal.Sheets[0].ColumnCount );
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, 0, 1, FpExternal.Sheets[0].ColumnCount);
        FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, FpExternal.Sheets[0].ColumnCount);
        //=========color
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.White;
        FpExternal.Sheets[0].PageSize = FpExternal.Sheets[0].RowCount;
        //FpExternal.Height = (FpExternal.Sheets[0].RowCount * 20) + 200; srinath
        //FpExternal.Width = 1350;
        FpExternal.Sheets[0].SheetName = " ";
    }

    public void function_radioheadercons()
    {
        ddlpage.Items.Clear();
        int totrowcount = 0;
        for (int find_tot_rowcnt = 0; find_tot_rowcnt < (FpExternal.Sheets[0].RowCount); find_tot_rowcnt++)
        {
            totrowcount++;
        }
        //================
        int intialrow = 1;
        int pages = 0;
        int remainrows = 0;
        int i6 = 0;
        int cal_row = 0;
        int fromrow = 30;
        int torow = 0;
        ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
    forloop:
        for (cal_row = fromrow; cal_row > torow; cal_row--)
        {
            if (cal_row < FpExternal.Sheets[0].RowCount)
            {
                if (FpExternal.Sheets[0].Cells[cal_row, 5].Text == "")
                {
                    i6++;
                    ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), torow.ToString() + "-" + cal_row.ToString()));
                    break;
                }
            }
        }
        if (cal_row < FpExternal.Sheets[0].RowCount)
        {
            torow = cal_row + 1;
            fromrow = 30 + fromrow + 1;
            goto forloop;
        }
        //=========================================================================
        //pages = totrowcount / 30;//hided on 30.07.12
        /// pages = totrowcount / (6 * (find_subjrow_count + 2));
        //  remainrows = totrowcount % 30;//hided on 30.07.12
        //if (FpExternal.Sheets[0].RowCount > 0)
        //{
        //    int i5 = 0;
        //    int i6 = 0;
        //    ddlpage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
        //    for (i6 = 1; i6 <= pages; i6++)
        //    {
        //        i5 = i6;
        //        ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), intialrow.ToString()));
        //        intialrow = intialrow + (6 * (find_subjrow_count + 2));
        //        //intialrow = intialrow + 30; //hided on 30.07.12 (6 * (find_subjrow_count + 2));
        //    }
        //    if (remainrows > 0)
        //    {
        //        i6 = i5 + 1;
        //        ddlpage.Items.Insert(i6, new System.Web.UI.WebControls.ListItem(i6.ToString(), intialrow.ToString()));
        //    }
        //}
        if ((ddlpage.SelectedValue.ToString() == string.Empty) || (ddlpage.SelectedValue.ToString() == "0"))
        {
            for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = true;
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            DropDownListpage.Items.Clear();
            if (totalRows >= 10)
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                for (int k = 0; k < Convert.ToInt32(totalRows); k = k + 10)
                {
                    DropDownListpage.Items.Add((k + 10).ToString());
                }
                DropDownListpage.Items.Add("Others");
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpExternal.Height = 335;
            }
            else if (totalRows == 0)
            {
                DropDownListpage.Items.Add("0");
                FpExternal.Height = 100;
            }
            else
            {
                FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                DropDownListpage.Items.Add(FpExternal.Sheets[0].PageSize.ToString());
                FpExternal.Height = 100 + (10 * Convert.ToInt32(totalRows));
            }
            if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
            {
                DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                FpExternal.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                CalculateTotalPages();
            }
            Buttontotal.Visible = true;
            lblrecord.Visible = true;
            DropDownListpage.Visible = true;
            TextBoxother.Visible = false;
            lblpage.Visible = true;
            TextBoxpage.Visible = true;
        }
        else
        {
            Buttontotal.Visible = false;
            lblrecord.Visible = false;
            DropDownListpage.Visible = false;
            TextBoxother.Visible = false;
            lblpage.Visible = false;
            TextBoxpage.Visible = false;
        }
    }

    protected void btnprintmaster_Click(object sender, EventArgs e)
    {
        exam_month = ddlMonth.SelectedValue.ToString();
        string strExam_month = string.Empty;
        exam_month = exam_month.Trim();
        switch (exam_month)
        {
            case "1":
                strExam_month = "January";
                break;
            case "2":
                strExam_month = "February";
                break;
            case "3":
                strExam_month = "March";
                break;
            case "4":
                strExam_month = "April";
                break;
            case "5":
                strExam_month = "May";
                break;
            case "6":
                strExam_month = "June";
                break;
            case "7":
                strExam_month = "July";
                break;
            case "8":
                strExam_month = "Augest";
                break;
            case "9":
                strExam_month = "September";
                break;
            case "10":
                strExam_month = "October";
                break;
            case "11":
                strExam_month = "November";
                break;
            case "12":
                strExam_month = "December";
                break;
        }
        string copyOfReport = string.Empty;
        string selectedValue = string.Empty;
        if (rblOfficeDeptCopy.Items.Count > 0)
        {
            selectedValue = Convert.ToString(rblOfficeDeptCopy.SelectedValue).Trim();
        }
        switch (selectedValue)
        {
            case "0":
                copyOfReport = "(Office Copy)";
                break;
            case "1":
                copyOfReport = "(Dept. Copy)";
                break;
            case "2":
                copyOfReport = string.Empty;
                break;
            default:
                copyOfReport = string.Empty;
                break;
        }
        if (chkShowValuationMarks.Checked)
        {
            copyOfReport = "(Office Copy)";
        }
        string qry = "select clg.collname,c.Edu_Level,c.Course_Name,dt.Dept_Name,ltrim(rtrim(ISNULL(c.type,''))) as Type,'Class :'+c.Course_Name+' '+dt.Dept_Name as Degree_Details from collinfo clg,Course c,Degree dg,Department dt where c.college_code=clg.college_code and clg.college_code=dg.college_code and  clg.college_code=dt.college_code and dt.college_code=dg.college_code and dt.college_code=dg.college_code and dt.college_code=c.college_code and c.college_code=dg.college_code and dt.Dept_Code=dg.Dept_Code and dg.Course_Id=c.Course_Id and dg.Degree_Code='" + Convert.ToString(ddlBranch.SelectedValue).Trim() + "'";
        DataSet dsDegreeDetails = new DataSet();
        dsDegreeDetails = d2.select_method_wo_parameter(qry, "text");
        string className = string.Empty;
        string sectionDetails = string.Empty;
        string collegeHeader = txtCollegeHeader.Text.Trim();
        if (dsDegreeDetails.Tables.Count > 0 && dsDegreeDetails.Tables[0].Rows.Count > 0)
        {
            className = Convert.ToString(dsDegreeDetails.Tables[0].Rows[0]["Degree_Details"]).Trim();
        }
        else
        {
            className = "Degree & Branch : " + ddlDegree.SelectedItem.ToString() + " " + ddlBranch.SelectedItem.ToString();
        }
        if (ddlSec.Enabled)
        {
            if (ddlSec.Items.Count > 0)
            {
                if (Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "all" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "" && Convert.ToString(ddlSec.SelectedItem.Text).Trim().ToLower() != "-1")
                {
                    sectionDetails = " '" + Convert.ToString(ddlSec.SelectedItem.Text).Trim() + "' SECTION";
                }
            }
        }
        string degreedetails = ((string.IsNullOrEmpty(collegeHeader)) ? "" : collegeHeader + "$") + "Office of the Controller of Examinations$" + ((chksubjtype.Items.Count > 0 && chksubjtype.Items[1].Selected && !chksubjtype.Items[0].Selected) ? "Arrear " : "") + "Result of the Semester Examination " + strExam_month + " " + ddlYear.SelectedItem.ToString() + copyOfReport + "$" + className + sectionDetails + ((chksubjtype.Items.Count > 0 && (!chksubjtype.Items[1].Selected || chksubjtype.Items[0].Selected)) ? "@ Semester :" + ddlSemYr.SelectedItem.ToString() : "");
        if (rb2.Checked == true)
        {
            degreedetails = ((string.IsNullOrEmpty(collegeHeader)) ? "" : collegeHeader + "$") + "Office of the Controller of Examinations$" + ((chksubjtype.Items.Count > 0 && chksubjtype.Items[1].Selected && !chksubjtype.Items[0].Selected) ? "Arrear " : "") + "TABULATED MARK REGISTER - " + strExam_month + " " + ddlYear.SelectedItem.ToString() + copyOfReport + "$" + className + sectionDetails + dsDegreeDetails.Tables[0].Rows[0]["Type"] + ((chksubjtype.Items.Count > 0 && (!chksubjtype.Items[1].Selected || chksubjtype.Items[0].Selected)) ? "@ Semester :" + ddlSemYr.SelectedItem.ToString() : "");
        }
        string pagename = "tmr_report2.aspx";
        Printcontrol.loadspreaddetails(FpExternal, pagename, degreedetails);
        Printcontrol.Visible = true;
    }

    public string Calulat_GPA_cgpaformate1(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = string.Empty;
        string sql = string.Empty;
        double creditandtotal = 0;
        double credittotal = 0;
        string syll_code = string.Empty;
        string strcredits = string.Empty;
        DataSet dggradetot = new DataSet();
        syll_code = daccess.GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = d2.GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {
            sql = " Select distinct Subject.subject_code,Subject.credit_points,SubWiseGrdeMaster.credit_points as gradepoint,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and SubWiseGrdeMaster.Grade=Mark_Entry.grade and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        else if (ccva == "True")
        {
            sql = " Select distinct Subject.subject_code,Subject.credit_points,SubWiseGrdeMaster.credit_points as gradepoint,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and SubWiseGrdeMaster.Grade=Mark_Entry.grade and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";
        }
        DataSet marksdata = new DataSet();
        marksdata.Clear();
        if (sql != "" && sql != null)
        {
            marksdata = daccess.select_method_wo_parameter(sql, "Text");
            if (marksdata.Tables.Count > 0 && marksdata.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
                {
                    double total = 0; double gp = 0;
                    //double total = Convert.ToDouble(marksdata.Tables[0].Rows[i]["total"].ToString());
                    //double gp = Convert.ToDouble(marksdata.Tables[0].Rows[i]["gradepoint"].ToString());
                    double.TryParse(Convert.ToString(marksdata.Tables[0].Rows[i]["total"]), out total);
                    double.TryParse(Convert.ToString(marksdata.Tables[0].Rows[i]["gradepoint"]), out gp);
                    // string data = "select Credit_Points from SubWiseGrdeMaster where subjectcode='" + marksdata.Tables[0].Rows[i]["subject_code"].ToString() + "'  and  '" + total + "'<= frange and '" + total + "'>= trange and college_code='" + collegecode + "'  and exam_month=" + exam_month + " and exam_year=" + exam_year + string.Empty;
                    strcredits = Convert.ToString(marksdata.Tables[0].Rows[i]["credit_points"].ToString());
                    creditandtotal = creditandtotal + (gp * Convert.ToDouble(strcredits));
                    credittotal = credittotal + Convert.ToDouble(strcredits);
                }
            }
            else
            {
                return "-";
            }
        }
        creditandtotal = (credittotal > 0 && creditandtotal > 0) ? Math.Round((creditandtotal / credittotal), 2, MidpointRounding.AwayFromZero) : 0.00;
        return creditandtotal.ToString();
    }

    public string Calulat_CGPA_cgpaformate1(string RollNo, string semval, string degree_code, string batch_year, string collegecode)
    {
        string calculate = string.Empty;
        string strsubcrd = string.Empty;
        DataSet dggradetot = new DataSet();
        DataSet dssem = new DataSet();
        string strcredits = string.Empty;
        double creditandtotal = 0;
        double credittotal = 0;
        strsubcrd = " Select distinct Syllabus_Master.Semester,Subject.subject_code,Mark_Entry.exam_code, Subject.credit_points,SubWiseGrdeMaster.credit_points as gradepoint ,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
        strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
        //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
        strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' and SubWiseGrdeMaster.Grade=Mark_Entry.grade order by Syllabus_Master.Semester";
        DataSet marksdata = new DataSet();
        marksdata.Clear();
        ArrayList seminfo = new ArrayList();
        seminfo.Clear();
        DataView dvgr = new DataView();
        Boolean semnewadd = false;
        double semgpa = 0;
        if (strsubcrd != null && strsubcrd != "")
        {
            marksdata = daccess.select_method_wo_parameter(strsubcrd, "Text");
            if (marksdata.Tables.Count > 0 && marksdata.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
                {
                    if (!seminfo.Contains(marksdata.Tables[0].Rows[i]["Semester"].ToString()))
                    {
                        marksdata.Tables[0].DefaultView.RowFilter = "Semester='" + marksdata.Tables[0].Rows[i]["Semester"].ToString() + "'";
                        dvgr = marksdata.Tables[0].DefaultView;
                        if (dvgr.Count > 0)
                        {
                            semgpa = 0;
                            credittotal = 0;
                            for (int j = 0; j < dvgr.Count; j++)
                            {
                                double total = Convert.ToDouble(dvgr[j]["total"].ToString());
                                string exam_months = daccess.GetFunction(" select Exam_Month from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                string exam_years = daccess.GetFunction(" select Exam_year from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                double gp = 0;
                                double.TryParse(Convert.ToString(dvgr[j]["gradepoint"]), out gp);
                                // string data = " select  Credit_Points  from SubWiseGrdeMaster where subjectcode='" + dvgr[j]["subject_code"].ToString() + "'  and  '" + dvgr[j]["total"].ToString() + "'<= frange and '" + dvgr[j]["total"].ToString() + "'>= trange and college_code='" + collegecode + "'  and exam_month='" + exam_months + "' and exam_year='" + exam_years + "'";
                                strcredits = Convert.ToString(dvgr[j]["credit_points"].ToString());
                                semgpa = semgpa + (gp * Convert.ToDouble(strcredits));
                                credittotal = credittotal + Convert.ToDouble(strcredits);
                            }
                            semnewadd = true;
                            seminfo.Add(marksdata.Tables[0].Rows[i]["Semester"].ToString());
                        }
                    }
                    if (semnewadd == true)
                    {
                        semgpa = Math.Round((semgpa / credittotal), 2, MidpointRounding.AwayFromZero);
                        creditandtotal = creditandtotal + semgpa;
                        semnewadd = false;
                    }
                }
            }
        }
        if (seminfo.Count > 0)
        {
            creditandtotal = (seminfo.Count > 0 && creditandtotal > 0) ? Math.Round((creditandtotal / seminfo.Count), 2, MidpointRounding.AwayFromZero) : 0;
            calculate = creditandtotal.ToString();
        }
        if (calculate == "NaN" || calculate.Trim() == "" || calculate.Trim() == "0")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }

    #region Added By Malang Raja on Dec 23 2016

    #region Column Order

    protected void chkColumnOrderAll_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkColumnOrderAll.Checked == true)
            {
                txtOrder.Text = string.Empty;
                ItemList.Clear();
                Itemindex.Clear();
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    liOrder.Selected = true;
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    lbtnRemoveAll.Visible = true;
                    ItemList.Add(Convert.ToString(liOrder.Text).Trim());
                    Itemindex.Add(liValue);
                    switch (liValue)
                    {
                        case "5":
                            if (!chkgender.Checked)
                            {
                                liOrder.Enabled = false;
                                liOrder.Selected = false;
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                            break;
                        case "10":
                        case "13":
                            if (!chkgrade.Checked)
                            {
                                liOrder.Enabled = false;
                                liOrder.Selected = false;
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                            break;
                        case "14":
                            if (!chk_subjectwisegrade.Checked)
                            {
                                liOrder.Enabled = false;
                                liOrder.Selected = false;
                                if (Itemindex.Contains(liValue))
                                {
                                    ItemList.Remove(liOrder.Text);
                                    Itemindex.Remove(liOrder.Value);
                                }
                            }
                            else
                            {
                                liOrder.Enabled = true;
                                liOrder.Selected = true;
                                if (!Itemindex.Contains(liValue))
                                {
                                    ItemList.Add(liOrder.Text);
                                    Itemindex.Add(liOrder.Value);
                                }
                            }
                            break;
                        case "6":
                            if (chkshowsub_name.Checked)
                                liOrder.Text = "Subject Name";
                            else
                                liOrder.Text = "Subject Code";
                            break;
                        default:
                            liOrder.Selected = true;
                            liOrder.Enabled = true;
                            break;
                    }
                }
                lbtnRemoveAll.Visible = true;
                txtOrder.Visible = true;
                txtOrder.Text = string.Empty;
                int j = 0;
                string colname12 = string.Empty;
                for (int i = 0; i < ItemList.Count; i++)
                {
                    j = j + 1;
                    if (colname12 == "")
                    {
                        colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                    else
                    {
                        colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString(j).Trim() + ")";
                    }
                }
                txtOrder.Text = colname12;
            }
            else
            {
                ItemList.Clear();
                Itemindex.Clear();
                foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
                {
                    liOrder.Selected = false;
                    string liValue = Convert.ToString(liOrder.Value).Trim();
                    switch (liValue)
                    {
                        case "5":
                            if (!chkgender.Checked)
                            {
                                liOrder.Enabled = false;
                                liOrder.Selected = false;
                            }
                            break;
                        case "10":
                        case "13":
                        case "14":
                            if (!chkgrade.Checked)
                            {
                                liOrder.Enabled = false;
                                liOrder.Selected = false;
                            }
                            break;
                        case "6":
                            if (chkshowsub_name.Checked)
                                liOrder.Text = "Subject Name";
                            else
                                liOrder.Text = "Subject Code";
                            break;
                    }
                }
                lbtnRemoveAll.Visible = false;
                txtOrder.Text = string.Empty;
                txtOrder.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void lbtnRemoveAll_Click(object sender, EventArgs e)
    {
        try
        {
            cblColumnOrder.ClearSelection();
            chkColumnOrderAll.Checked = false;
            lbtnRemoveAll.Visible = false;
            ItemList.Clear();
            Itemindex.Clear();
            txtOrder.Text = string.Empty;
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    protected void cblColumnOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            chkColumnOrderAll.Checked = false;
            string value = string.Empty;
            int index;
            //cblColumnOrder.Items[0].Selected = true;
            value = string.Empty;
            string result = Request.Form["__EVENTTARGET"];
            string[] checkedBox = result.Split('$');
            index = int.Parse(checkedBox[checkedBox.Length - 1]);
            string sindex = Convert.ToString(index).Trim();
            if (cblColumnOrder.Items[index].Selected)
            {
                if (!Itemindex.Contains(sindex))
                {
                    ItemList.Add(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                    Itemindex.Add(sindex);
                }
            }
            else
            {
                ItemList.Remove(Convert.ToString(cblColumnOrder.Items[index].Text).Trim());
                Itemindex.Remove(sindex);
            }
            for (int i = 0; i < cblColumnOrder.Items.Count; i++)
            {
                if (cblColumnOrder.Items[i].Selected == false)
                {
                    sindex = Convert.ToString(i).Trim();
                    ItemList.Remove(Convert.ToString(cblColumnOrder.Items[i].Text).Trim());
                    Itemindex.Remove(sindex);
                }
            }
            lbtnRemoveAll.Visible = true;
            txtOrder.Visible = false;
            txtOrder.Text = string.Empty;
            string colname12 = string.Empty;
            for (int i = 0; i < ItemList.Count; i++)
            {
                if (colname12 == "")
                {
                    colname12 = Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
                else
                {
                    colname12 = colname12 + "," + Convert.ToString(ItemList[i]).Trim() + "(" + Convert.ToString((i + 1)).Trim() + ")";
                }
            }
            txtOrder.Text = colname12;
            if (ItemList.Count == 14)
            {
                chkColumnOrderAll.Checked = true;
            }
            if (ItemList.Count == 0)
            {
                txtOrder.Visible = false;
                lbtnRemoveAll.Visible = false;
            }
            txtOrder.Visible = false;
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

    protected void chkfailshow_CheckedChanged(object sender, EventArgs e)
    {
        divFailValue.Visible = false;
        txtFailValue.Text = string.Empty;
        if (chkfailshow.Checked)
        {
            divFailValue.Visible = true;
            txtFailValue.Text = "F";
        }
    }

    protected void chkgrade_CheckedChanged(object sender, EventArgs e)
    {
        if (cblColumnOrder.Items.Count > 0)
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                string liValue = Convert.ToString(liOrder.Value).Trim();
                switch (liValue)
                {
                    case "10":
                    case "13":
                        if (!chkgrade.Checked)
                        {
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                    case "14":
                        if (!chk_subjectwisegrade.Checked)
                        {
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                }
            }
        }
    }

    protected void chkgender_CheckedChanged(object sender, EventArgs e)
    {
        if (cblColumnOrder.Items.Count > 0)
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                string liValue = Convert.ToString(liOrder.Value).Trim();
                switch (liValue)
                {
                    case "5":
                        if (!chkgender.Checked)
                        {
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                }
            }
        }
    }

    protected void chkshowsub_name_CheckedChanged(object sender, EventArgs e)
    {
        if (cblColumnOrder.Items.Count > 0)
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                string liValue = Convert.ToString(liOrder.Value).Trim();
                switch (liValue)
                {
                    case "6":
                        if (!chkshowsub_name.Checked)
                        {
                            liOrder.Text = "Subject Code";
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Text = "Subject Name";
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                }
            }
        }
    }

    protected void chk_subjectwisegrade_CheckedChanged(object sender, EventArgs e)
    {
        if (cblColumnOrder.Items.Count > 0)
        {
            foreach (System.Web.UI.WebControls.ListItem liOrder in cblColumnOrder.Items)
            {
                string liValue = Convert.ToString(liOrder.Value).Trim();
                switch (liValue)
                {
                    case "10":
                    case "13":
                        if (!chkgrade.Checked)
                        {
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                    case "14":
                        if (!chk_subjectwisegrade.Checked)
                        {
                            liOrder.Enabled = false;
                            liOrder.Selected = false;
                            if (Itemindex.Contains(liValue))
                            {
                                ItemList.Remove(liOrder.Text);
                                Itemindex.Remove(liOrder.Value);
                            }
                        }
                        else
                        {
                            liOrder.Enabled = true;
                            liOrder.Selected = true;
                            if (!Itemindex.Contains(liValue))
                            {
                                ItemList.Add(liOrder.Text);
                                Itemindex.Add(liOrder.Value);
                            }
                        }
                        break;
                }
            }
        }
    }

    private void SetColumnVisibility()
    {
        try
        {
        }
        catch
        {
        }
    }

    protected void chkShowValuationMarks_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkShowValuationMarks.Checked)
            {
                if (rblOfficeDeptCopy.Items.Count > 0)
                {
                    rblOfficeDeptCopy.Items[0].Selected = true;
                    rblOfficeDeptCopy.Items[1].Selected = false;
                    rblOfficeDeptCopy.Items[2].Selected = false;
                }
            }
            else
            {
                if (rblOfficeDeptCopy.Items.Count > 0)
                {
                    rblOfficeDeptCopy.Items[0].Selected = false;
                    rblOfficeDeptCopy.Items[1].Selected = false;
                    rblOfficeDeptCopy.Items[2].Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    protected void chkIncludePassedOut_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            divRedo.Visible = false;
            if (chkIncludePassedOut.Checked)
            {
                divRedo.Visible = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    #endregion

}