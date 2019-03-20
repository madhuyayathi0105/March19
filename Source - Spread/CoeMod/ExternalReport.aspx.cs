using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BalAccess;
using FarPoint.Web.Spread;
using Gios.Pdf;
using InsproDataAccess;
using System.Net.Mail;
using System.Net;
public partial class ExternalReport : System.Web.UI.Page
{
    [Serializable]
    public class MyImg : ImageCellType
    {
        public override Control PaintCell(String id, TableCell parent, FarPoint.Web.Spread.Appearance style, FarPoint.Web.Spread.Inset margin, object val, bool ul)
        {
            System.Web.UI.WebControls.Image img1 = new System.Web.UI.WebControls.Image();
            img1.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img1.Width = Unit.Percentage(90);
            img1.Height = Unit.Percentage(70);
            return img1;
            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
            img.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img.Width = Unit.Percentage(90);
            img.Height = Unit.Percentage(70);
            return img;
            //'-------------coe sign
            System.Web.UI.WebControls.Image img2 = new System.Web.UI.WebControls.Image();
            img2.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img2.Width = Unit.Percentage(90);
            img2.Height = Unit.Percentage(70);
            return img2;
            //'-------------Class Advisor
            System.Web.UI.WebControls.Image img3 = new System.Web.UI.WebControls.Image();
            img3.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img3.Width = Unit.Percentage(90);
            img3.Height = Unit.Percentage(70);
            return img3;
            //'-------------HOD
            System.Web.UI.WebControls.Image img4 = new System.Web.UI.WebControls.Image();
            img4.ImageUrl = this.ImageUrl; //base.ImageUrl;  
            img4.Width = Unit.Percentage(90);
            img4.Height = Unit.Percentage(70);
            return img4;
        }
    }

    #region Field Declaration

    SqlCommand cmd;
    SqlDataReader dr_exam;
    SqlDataReader dr_mnthyr;
    SqlDataReader dr_convert;
    string grade_setting = string.Empty;
    SqlConnection con = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem2 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Photo = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection setcon = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Load = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Inssetting = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Getfunc = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Examcode = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_loadSubject = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Stud = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade1 = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_mrkentry = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_currsem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_getdetail = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_daters = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_course = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_exam = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_secrs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_new = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_grademas = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_credit = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_option = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_sem = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_result = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_convertgrade = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_subcrd = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_rs = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    SqlConnection con_Grade_flag = new SqlConnection(ConfigurationManager.AppSettings["DSN"]);
    DAccess2 d2 = new DAccess2();
    int gcheck = 0;
    string prov_collnamenew1 = "", prov_address1 = "", collnamenew1 = "", address1 = "", address2 = "", address3 = "", pincode = "", categery = "", Affliated = string.Empty;

    string address = string.Empty;
    string Phoneno = string.Empty;
    string Faxno = string.Empty;
    string phnfax = string.Empty;
    int serialno = 0;

    int subjectcount = 0;
    int rankposition = 0;
    int totalarear = 0;
    int arear = 0;
    int arearfail = 0;
    int arearpass = 0;
    int arearabsent = 0;
    string district = string.Empty;
    string email = string.Empty;
    string website = string.Empty;
    string strsec = string.Empty;
    int semdec = 0;
    string sections = string.Empty;
    string funcgrade = string.Empty;
    string mark = string.Empty;
    bool markflag = false;
    string rol_no = string.Empty;
    string courseid = string.Empty;
    string atten = string.Empty;
    string Master1 = string.Empty;
    string regularflag = string.Empty;
    string genderflag = string.Empty;
    string strdayflag = string.Empty;
    string fromdate = string.Empty;
    bool InsFlag;
    bool flag;
    int IntExamCode = 0;
    int column_count = 0;
    string degree_code = string.Empty;
    string current_sem = string.Empty;
    string batch_year = string.Empty;
    string getgradeflag = string.Empty;
    string exam_month = string.Empty;
    string exam_year = string.Empty;
    string getsubno = string.Empty;
    string getsubtype = string.Empty;
    int rcnt;
    int ExamCode = 0;
    string strmnthyear = string.Empty;
    string strexam = string.Empty;
    int overallcredit = 0;
    string grade = string.Empty;
    string funcsubno = string.Empty;
    string funcsubname = string.Empty;
    string funcsubcode = string.Empty;
    string funcresult = string.Empty;
    string funcsemester = string.Empty;
    string funccredit = string.Empty;
    string EarnedVal = string.Empty;
    string prov_exam_month;
    string prov_batch_year;
    string prov_exam_year;
    string prov_degree_code;
    double cgpa2 = 0;
    int cou = 0;
    Hashtable hat = new Hashtable();
    Hashtable hrank = new Hashtable();
    DataSet ds_load = new DataSet();
    DAccess2 daccess = new DAccess2();
    string collegecode = string.Empty;
    string usercode = string.Empty;
    string singleuser = string.Empty;
    string group_user = string.Empty;

    string find_staff_code1 = string.Empty;
    SqlDataAdapter da_find_staffcode1 = new SqlDataAdapter();
    DataSet ds_find_staffcode1;
    string find_staff_codex = string.Empty;
    SqlDataAdapter da_find_staffcodex = new SqlDataAdapter();
    InsproDirectAccess dirAcc = new InsproDirectAccess();
    DataSet ds_find_staffcodex;
    bool gradeflag = false;
    bool flagchknew = true;
    DataSet srids = new DataSet();
    DAccess2 commonaccess = new DAccess2();
    DataSet ds = new DataSet();
    string ggender = string.Empty;
    string ggpa = string.Empty;
    string gcgpa = string.Empty;
    string gregisternumber = string.Empty;
    string gparentname = string.Empty;
    string gparentaddress = string.Empty;
    string gstreet = string.Empty;
    string gcity = string.Empty;
    string gdistrict = string.Empty;
    string gstudentname = string.Empty;
    FarPoint.Web.Spread.TextCellType txt = new FarPoint.Web.Spread.TextCellType();

    #endregion

    protected void lb2_Click(object sender, EventArgs e)
    {
        Session.Abandon();
        Session.Clear();
        Session.RemoveAll();
        System.Web.Security.FormsAuthentication.SignOut();
        Response.Redirect("~/Default.aspx", false);
    }

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
            collegecode = Session["collegecode"].ToString();
            usercode = Session["usercode"].ToString();
            singleuser = Session["single_user"].ToString();
            group_user = Session["group_code"].ToString();
            Session["Semester"] = Convert.ToString(ddlSemYr.SelectedValue);
            lblxlerr.Visible = false;
            if (!IsPostBack)
            {
                txtDOP.Attributes.Add("Readonly", "Readonly");
                txtDate.Attributes.Add("Readonly", "Readonly");
                txtvsbl_setting.Attributes.Add("Readonly", "Readonly");
                txtsubjtype.Attributes.Add("Readonly", "Readonly");
                btnxl.Visible = false;
                lblxl.Visible = false;
                txtxlname.Visible = false;
                lbl_selectall.Visible = false;
                chk_select_all.Visible = false;
                lbl_hideall.Visible = false;
                chk_hide_all.Visible = false;
                panelchech.Visible = false;

                collegecode = Session["collegecode"].ToString();
                usercode = Session["usercode"].ToString();
                singleuser = Session["single_user"].ToString();
                group_user = Session["group_code"].ToString();

                bindbatch();

                binddegree();

                bindbranch();

                bindsem();
                bindsec();

                FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                style.Font.Size = 12;
                style.Font.Bold = true;
                style.HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
                FpExternal.Sheets[0].AllowTableCorner = true;
                FpExternal.Sheets[0].SheetCorner.Cells[0, 0].Text = " S.No ";
                FpExternal.ActiveSheetView.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
                //FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Border.BorderColor = Color.Black;
                FpExternal.Sheets[0].SheetName = "  ";
                rdGrade.Checked = true;

                string getbranch = ddlBranch.Text.ToString();
                FpExternal.Visible = false;
                btnxl.Visible = false;
                lblxl.Visible = false;
                txtxlname.Visible = false;
                btnLetterFormat.Visible = false;
                lblxl.Visible = false;
                txtxlname.Visible = false;
                if (Session["usercode"].ToString() != "")
                {
                    Master1 = "select * from Master_Settings where usercode=" + Session["usercode"] + "";
                    setcon.Close();
                    setcon.Open();
                    SqlDataReader mtrdr;
                    SqlCommand mtcmd = new SqlCommand(Master1, setcon);
                    mtrdr = mtcmd.ExecuteReader();
                    Session["strvar"] = string.Empty;
                    Session["Rollflag"] = "0";
                    Session["Regflag"] = "0";
                    Session["Studflag"] = "0";
                    if (mtrdr.HasRows)
                    {
                        while (mtrdr.Read())
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
                }
                //  ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                //ddlMonth.Items.Insert(0, new System.Web.UI.WebControls.ListItem("Jan", "1"));
                //ddlMonth.Items.Insert(1, new System.Web.UI.WebControls.ListItem("Feb", "2"));
                //ddlMonth.Items.Insert(2, new System.Web.UI.WebControls.ListItem("Mar", "3"));
                //ddlMonth.Items.Insert(3, new System.Web.UI.WebControls.ListItem("Apr", "4"));
                //ddlMonth.Items.Insert(4, new System.Web.UI.WebControls.ListItem("May", "5"));
                //ddlMonth.Items.Insert(5, new System.Web.UI.WebControls.ListItem("Jun", "6"));
                //ddlMonth.Items.Insert(6, new System.Web.UI.WebControls.ListItem("Jul", "7"));
                //ddlMonth.Items.Insert(7, new System.Web.UI.WebControls.ListItem("Aug", "8"));
                //ddlMonth.Items.Insert(8, new System.Web.UI.WebControls.ListItem("Sep", "9"));
                //ddlMonth.Items.Insert(9, new System.Web.UI.WebControls.ListItem("Oct", "10"));
                //ddlMonth.Items.Insert(10, new System.Web.UI.WebControls.ListItem("Nov", "11"));
                //ddlMonth.Items.Insert(11, new System.Web.UI.WebControls.ListItem("Dec", "12"));
                //int year;
                //year = Convert.ToInt16(DateTime.Today.Year);
                //ddlYear.Items.Clear();
                //for (int l = 0; l <= 20; l++)
                //{
                //    ddlYear.Items.Add(Convert.ToString(year - l));
                //}
                //  ddlYear.Items.Insert(0, new System.Web.UI.WebControls.ListItem("  ", "0"));
                BindExamYear();
                BindExamMonth();
                chksubjtype.Items[0].Selected = true;
                chksubjtype.Items[1].Selected = true;
                lblarrear_sem.Visible = false;
                txtarrear_sem.Visible = false;
                chkarrear_Sem.Visible = false;
                pnlarrear_Sem.Visible = false;
                string strDate = Convert.ToString(System.DateTime.Now.Date);
                string[] spl_strDate = strDate.Split(' ');
                string[] spl_date = spl_strDate[0].Split('/');
                txtDate.Text = spl_date[1].ToString() + "/" + spl_date[0].ToString() + "/" + spl_date[2].ToString();
                txtDOP.Text = spl_date[1].ToString() + "/" + spl_date[0].ToString() + "/" + spl_date[2].ToString();
                //txtDate.Text = spl_strDate[0].ToString();
                //txtDOP.Text = spl_strDate[0].ToString();
                chkvsbl_setting.Items[0].Selected = true;
                chkvsbl_setting.Items[1].Selected = true;
                chkvsbl_setting.Items[2].Selected = true;
            }
        }
        catch (Exception ex)
        {
        }
    }

    public void bindbatch()
    {
        ddlBatch.Items.Clear();
        //ds_load = daccess.select_method_wo_parameter("bind_batch", "sp");
        string qry = "select distinct batch_year from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' " + ((Session["collegecode"] != null) ? " and college_code='" + Convert.ToString(Session["collegecode"]).Trim() + "' " : "") + " order by batch_year desc";
        ds_load = daccess.select_method_wo_parameter(qry, "text");
        if (ds_load.Tables.Count > 0)
        {
            int count = ds_load.Tables[0].Rows.Count;
            if (count > 0)
            {
                ddlBatch.DataSource = ds_load;
                ddlBatch.DataTextField = "batch_year";
                ddlBatch.DataValueField = "batch_year";
                ddlBatch.DataBind();
                ddlBatch.SelectedIndex = 0;
            }
            //int count1 = ds_load.Tables[1].Rows.Count;
            //if (count > 0)
            //{
            //    int max_bat = 0;
            //    max_bat = Convert.ToInt32(ds_load.Tables[1].Rows[0][0].ToString());
            //    ddlBatch.SelectedValue = max_bat.ToString();
            //    con.Close();
            //}
        }
    }

    public void bindbranch()
    {
        ddlBranch.Items.Clear();
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
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("course_id", ddlDegree.SelectedValue);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_branch", hat, "sp");
        int count2 = ds_load.Tables[0].Rows.Count;
        if (count2 > 0)
        {
            ddlBranch.DataSource = ds_load;
            ddlBranch.DataTextField = "dept_name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
        }
    }

    public void binddegree()
    {
        ddlDegree.Items.Clear();
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
        hat.Add("single_user", singleuser.ToString());
        hat.Add("group_code", group_user);
        hat.Add("college_code", collegecode);
        hat.Add("user_code", usercode);
        ds_load = daccess.select_method("bind_degree", hat, "sp");
        int count1 = ds_load.Tables[0].Rows.Count;
        if (count1 > 0)
        {
            ddlDegree.DataSource = ds_load;
            ddlDegree.DataTextField = "course_name";
            ddlDegree.DataValueField = "course_id";
            ddlDegree.DataBind();
        }
    }

    public void bindsec()
    {
        ddlSec.Items.Clear();
        hat.Clear();
        hat.Add("batch_year", ddlBatch.SelectedValue.ToString());
        hat.Add("degree_code", ddlBranch.SelectedValue);
        ds_load = daccess.select_method("bind_sec", hat, "sp");
        int count5 = ds_load.Tables[0].Rows.Count;
        if (count5 > 0)
        {
            ddlSec.DataSource = ds_load;
            ddlSec.DataTextField = "sections";
            ddlSec.DataValueField = "sections";
            ddlSec.DataBind();
            ddlSec.Enabled = true;
        }
        else
        {
            ddlSec.Enabled = false;
        }
        ddlSec.Items.Add("ALL");//@@@@@@ added on 29.06.12
    }

    protected override void Render(System.Web.UI.HtmlTextWriter writer)
    {
        Control cntUpdateBtn = FpMarkSheet.FindControl("Update");
        Control cntCancelBtn = FpMarkSheet.FindControl("Cancel");
        Control cntCopyBtn = FpMarkSheet.FindControl("Copy");
        Control cntCutBtn = FpMarkSheet.FindControl("Clear");
        Control cntPasteBtn = FpMarkSheet.FindControl("Paste");
        //Control cntPagePrintBtn = FpMarkSheet.FindControl("Print");
        Control cntUpdateBtn1 = FpExternal.FindControl("Update");
        Control cntCancelBtn1 = FpExternal.FindControl("Cancel");
        Control cntCopyBtn1 = FpExternal.FindControl("Copy");
        Control cntCutBtn1 = FpExternal.FindControl("Clear");
        Control cntPasteBtn1 = FpExternal.FindControl("Paste");
        // Control cntPagePrintBtn1 = FpExternal.FindControl("Print");
        Control cntPageNextBtn1 = FpExternal.FindControl("Next");
        Control cntPagePreviousBtn1 = FpExternal.FindControl("Previous");
        //Control cntPagePrintBtn3 = sprdLetterFormat.FindControl("Print");
        //if ((cntPagePrintBtn3 != null))
        //{
        //    TableCell tc3 = (TableCell)cntPagePrintBtn3.Parent;
        //    TableRow tr3 = (TableRow)tc3.Parent;
        //    tr3.Cells.Remove(tc3);
        //}
        if ((cntUpdateBtn != null))
        {
            TableCell tc = (TableCell)cntUpdateBtn.Parent;
            TableRow tr = (TableRow)tc.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCancelBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCopyBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntCutBtn.Parent;
            tr.Cells.Remove(tc);
            tc = (TableCell)cntPasteBtn.Parent;
            tr.Cells.Remove(tc);
            //tc = (TableCell)cntPageNextBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntPagePreviousBtn.Parent;
            //tr.Cells.Remove(tc);
            //tc = (TableCell)cntPagePrintBtn.Parent;
            //tr.Cells.Remove(tc);
        }
        if ((cntUpdateBtn1 != null))
        {
            TableCell tc1 = (TableCell)cntUpdateBtn1.Parent;
            TableRow tr1 = (TableRow)tc1.Parent;
            tr1.Cells.Remove(tc1);
            tc1 = (TableCell)cntCancelBtn1.Parent;
            tr1.Cells.Remove(tc1);
            tc1 = (TableCell)cntCopyBtn1.Parent;
            tr1.Cells.Remove(tc1);
            tc1 = (TableCell)cntCutBtn1.Parent;
            tr1.Cells.Remove(tc1);
            tc1 = (TableCell)cntPasteBtn1.Parent;
            tr1.Cells.Remove(tc1);
            //tc1 = (TableCell)cntPageNextBtn1.Parent;
            //tr1.Cells.Remove(tc1);
            //tc1 = (TableCell)cntPagePreviousBtn1.Parent;
            //tr1.Cells.Remove(tc1);
            //tc1 = (TableCell)cntPagePrintBtn1.Parent;
            //tr1.Cells.Remove(tc1);
        }
        base.Render(writer);
    }

    public void BindBatch()
    {
        ddlBatch.Items.Clear();
        string sqlstr = string.Empty;
        int max_bat = 0;
        DataSet ds = ClsAttendanceAccess.GetBatchDetail();
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBatch.DataSource = ds;
            ddlBatch.DataTextField = "batch_year";
            ddlBatch.DataValueField = "batch_year";
            ddlBatch.DataBind();
            sqlstr = "select max(batch_year) from Registration where batch_year<>'-1' and batch_year<>'' and delflag=0 and exam_flag<>'debar' ";
            max_bat = Convert.ToInt32(GetFunction(sqlstr));
            ddlBatch.SelectedValue = max_bat.ToString();
            // ddlBatch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }

    public void BindDegree()
    {
        ddlDegree.Items.Clear();
        collegecode = Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetDegreeDetail(collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlDegree.DataSource = ds;
            ddlDegree.DataValueField = "Course_Id";
            ddlDegree.DataTextField = "Course_Name";
            ddlDegree.DataBind();
        }
    }

    public void BindSectionDetail()
    {
        string branch = ddlBranch.SelectedValue.ToString();
        string batch = ddlBatch.SelectedValue.ToString();
        con_Load.Close();
        con_Load.Open();
        cmd = new SqlCommand("select distinct sections from registration where batch_year=" + ddlBatch.SelectedValue.ToString() + " and degree_code=" + ddlBranch.SelectedValue.ToString() + " and sections<>'-1' and sections<>' ' and delflag=0 and exam_flag<>'Debar'", con_Load);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataSet ds = new DataSet();
        da.Fill(ds);
        ddlSec.DataSource = ds;
        ddlSec.DataTextField = "sections";
        ddlSec.DataValueField = "sections";
        ddlSec.DataBind();
        SqlDataReader dr_sec;
        dr_sec = cmd.ExecuteReader();
        dr_sec.Read();
        if (dr_sec.HasRows == true)
        {
            if (dr_sec["sections"].ToString() == "")
            {
                ddlSec.Enabled = false;
                //  RequiredFieldValidator5.Visible = false;
            }
            else
            {
                ddlSec.Enabled = true;
                //   RequiredFieldValidator5.Visible = true;
            }
        }
        else
        {
            ddlSec.Enabled = false;
            //   RequiredFieldValidator5.Visible = false;
        }
    }

    public void Get_Semester()
    {
        bool first_year;
        first_year = false;
        int duration = 0;
        string batch_calcode_degree;
        ddlSemYr.Items.Clear();
        //int typeval = 4;
        string batch = ddlBatch.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        string degree = ddlBranch.SelectedValue.ToString();
        batch_calcode_degree = batch.ToString() + "/" + collegecode.ToString() + "/" + degree.ToString();
        //Session["collegecode"].ToString();
        DataSet ds = ClsAttendanceAccess.Getsemster_Detail(batch_calcode_degree.ToString());
        if (ds.Tables[0].Rows.Count > 0)
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
            //ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    public void BindExamYear()
    {
        try
        {
            ddlYear.Items.Clear();
            ds.Clear();
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            //if (ddl.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCode))
            //    {
            //        collegeCode = " and dg.college_code in (" + collegeCode + ")";
            //    }
            //}
            //if (cblStream.Items.Count > 0)
            //{
            //    streamNames = getCblSelectedText(cblStream);
            //    if (!string.IsNullOrEmpty(streamNames))
            //    {
            //        qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
            //    }
            //}
            //if (ddlEduLevel.Items.Count > 0)
            //{
            //    eduLevels = string.Empty;
            //    foreach (ListItem li in ddlEduLevel.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (string.IsNullOrEmpty(eduLevels))
            //            {
            //                eduLevels = "'" + li.Text + "'";
            //            }
            //            else
            //            {
            //                eduLevels += ",'" + li.Text + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(eduLevels))
            //    {
            //        qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
            //    }
            //}
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            string qryDegreeCode = string.Empty;
            string qryBatch = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                //degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBatch.SelectedValue).Trim()))
                {
                    qryBatch = " and ed.batch_year in(" + Convert.ToString(ddlBatch.SelectedValue).Trim() + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                //degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBranch.SelectedValue).Trim()))
                {
                    qryDegreeCode = " and ed.degree_code in(" + Convert.ToString(ddlBranch.SelectedValue).Trim() + ")";
                }
            }
            if (!string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_year from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_year<>'0' " + qryDegreeCode + qryBatch + " order by ed.Exam_year desc";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = daccess.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlYear.DataSource = ds;
                    ddlYear.DataTextField = "Exam_year";
                    ddlYear.DataValueField = "Exam_year";
                    ddlYear.DataBind();
                    ddlYear.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// Developed By Malang Raja
    /// </summary>
    private void BindExamMonth()
    {
        try
        {
            string collegeCode = string.Empty;
            string batchYear = string.Empty;
            string degreeCode = string.Empty;
            ddlMonth.Items.Clear();
            ds.Clear();
            //if (cblCollege.Items.Count > 0)
            //{
            //    collegeCode = getCblSelectedValue(cblCollege);
            //    if (!string.IsNullOrEmpty(collegeCode))
            //    {
            //        collegeCode = " and dg.college_code in (" + collegeCode + ")";
            //    }
            //}
            //if (cblStream.Items.Count > 0)
            //{
            //    streamNames = getCblSelectedText(cblStream);
            //    if (!string.IsNullOrEmpty(streamNames))
            //    {
            //        qryStream = " and LTRIM(RTRIM(ISNULL(c.type,''))) in(" + streamNames + ")";
            //    }
            //}
            //if (ddlEduLevel.Items.Count > 0)
            //{
            //    eduLevels = string.Empty;
            //    foreach (ListItem li in ddlEduLevel.Items)
            //    {
            //        if (li.Selected)
            //        {
            //            if (string.IsNullOrEmpty(eduLevels))
            //            {
            //                eduLevels = "'" + li.Text + "'";
            //            }
            //            else
            //            {
            //                eduLevels += ",'" + li.Text + "'";
            //            }
            //        }
            //    }
            //    if (!string.IsNullOrEmpty(eduLevels))
            //    {
            //        qryEduLevel = " and c.Edu_Level in(" + eduLevels + ")";
            //    }
            //}
            //if (cblDegree.Items.Count > 0)
            //{
            //    courseIds = getCblSelectedValue(cblDegree);
            //    if (!string.IsNullOrEmpty(courseIds))
            //    {
            //        qryCourseId = " and c.Course_Id in(" + courseIds + ")";
            //    }
            //}
            string qryDegreeCode = string.Empty;
            string qryBatch = string.Empty;
            if (ddlBatch.Items.Count > 0)
            {
                //degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBatch.SelectedValue).Trim()))
                {
                    qryBatch = " and ed.batch_year in(" + Convert.ToString(ddlBatch.SelectedValue).Trim() + ")";
                }
            }
            if (ddlBranch.Items.Count > 0)
            {
                //degreeCodes = getCblSelectedValue(cblBranch);
                if (!string.IsNullOrEmpty(Convert.ToString(ddlBranch.SelectedValue).Trim()))
                {
                    qryDegreeCode = " and ed.degree_code in(" + Convert.ToString(ddlBranch.SelectedValue).Trim() + ")";
                }
            }
            string ExamYear = string.Empty;
            if (ddlYear.Items.Count > 0)
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlYear.Items)
                {
                    if (li.Selected)
                    {
                        if (string.IsNullOrEmpty(ExamYear))
                        {
                            ExamYear = "'" + li.Value + "'";
                        }
                        else
                        {
                            ExamYear += ",'" + li.Value + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(ExamYear))
                {
                    ExamYear = " and Exam_year in (" + ExamYear + ")";
                }
            }
            if (!string.IsNullOrEmpty(ExamYear) && !string.IsNullOrEmpty(qryBatch) && !string.IsNullOrEmpty(qryDegreeCode))
            {
                string qry = "select distinct ed.Exam_Month,upper(convert(varchar(3),DateAdd(month,ed.Exam_Month,-1))) as Month_Name from exam_details ed,Degree dg,Course c,Department dt where dg.Degree_Code=ed.degree_code and  c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and ed.Exam_Month<>'0' " + qryBatch + qryDegreeCode + ExamYear + " order by Exam_Month";
                ds.Clear();
                ds.Reset();
                ds.Dispose();
                ds = daccess.select_method_wo_parameter(qry, "text");
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ddlMonth.DataSource = ds;
                    ddlMonth.DataTextField = "Month_Name";
                    ddlMonth.DataValueField = "Exam_Month";
                    ddlMonth.DataBind();
                    ddlMonth.SelectedIndex = 0;
                }
            }
        }
        catch (Exception ex)
        {
        }
    }

    public string GetFunction(string sqlQuery)
    {
        string sqlstr;
        sqlstr = sqlQuery;
        con_Getfunc.Close();
        con_Getfunc.Open();
        SqlDataAdapter sqlAdapter1 = new SqlDataAdapter(sqlstr, con_Getfunc);
        SqlDataReader drnew;
        SqlCommand funcmd = new SqlCommand(sqlstr);
        funcmd.Connection = con_Getfunc;
        drnew = funcmd.ExecuteReader();
        drnew.Read();
        if (drnew.HasRows == true)
        {
            return drnew[0].ToString();
        }
        else
        {
            return "0";
        }
    }

    protected void ddlBatch_SelectedIndexChanged(object sender, EventArgs e)
    {
        if ((ddlDegree.SelectedIndex != 0) && (ddlBranch.SelectedIndex != 0))
        {
            ddlSemYr.Items.Clear();
            Get_Semester();
        }
        //   ddlSemYr.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        // ddlDegree.SelectedIndex = 0;
        //  ddlBranch.SelectedIndex = 0;
        // ddlSemYr.SelectedIndex = -1;
        ddlSec.SelectedIndex = -1;
        ddlletterformat.Text = " - - -  Select - - - ";
        BindExamYear();
        BindExamMonth();
        chk_IncludePassedOut_OnCheckedChanged(sender, e);
    }

    protected void ddlDegree_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBranch.Items.Clear();
        //string  a = 13;
        string course_id = ddlDegree.SelectedValue.ToString();
        //string sem = ddlSem.SelectedValue.ToString();
        collegecode = Session["collegecode"].ToString();
        usercode = Session["UserCode"].ToString();//Session["UserCode"].ToString();
        DataSet ds = ClsAttendanceAccess.GetBranchDetail(course_id.ToString(), collegecode.ToString());
        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlBranch.DataSource = ds;
            ddlBranch.DataTextField = "Dept_Name";
            ddlBranch.DataValueField = "degree_code";
            ddlBranch.DataBind();
            // ddlBranch.Items.Insert(0, new ListItem("- -Select- -", "-1"));
        }
        if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
        {
            bindsem();
            bindsec();
            ddlletterformat.Text = " - - -  Select - - - ";
            BindExamYear();
            BindExamMonth();
        }
    }

    public void clear()
    {
        ddlSemYr.Items.Clear();
        ddlSec.Items.Clear();
    }

    protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
    {
        clear();
        if (!Page.IsPostBack == false)
        {
        }
        try
        {
            if ((ddlBranch.SelectedIndex != 0) || (ddlBranch.SelectedIndex > 0) || (ddlBranch.SelectedIndex == 0))
            {
                bindsem();
                bindsec();
                BindExamYear();
                BindExamMonth();
                ddlletterformat.Text = " - - -  Select - - - ";
            }
        }
        catch (Exception ex)
        {
            string s = ex.ToString();
            Response.Write(s);
        }
    }

    public void bindsem()
    {
        //--------------------semester load
        ddlSemYr.Items.Clear();
        bool first_year;
        first_year = false;
        int duration = 0;
        int i = 0;
        con.Open();
        SqlDataReader dr;
        cmd = new SqlCommand("select distinct ndurations,first_year_nonsemester from ndegree where degree_code=" + ddlBranch.Text.ToString() + " and batch_year=" + ddlBatch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
        dr = cmd.ExecuteReader();
        dr.Read();
        if (dr.HasRows == true)
        {
            first_year = Convert.ToBoolean(dr[1].ToString());
            duration = Convert.ToInt16(dr[0].ToString());
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
            dr.Close();
            SqlDataReader dr1;
            cmd = new SqlCommand("select distinct duration,first_year_nonsemester  from degree where degree_code=" + ddlBranch.Text.ToString() + " and college_code=" + Session["collegecode"] + "", con);
            //     ddlSemYr.Items.Clear();
            dr1 = cmd.ExecuteReader();
            dr1.Read();
            if (dr1.HasRows == true)
            {
                first_year = Convert.ToBoolean(dr1[1].ToString());
                duration = Convert.ToInt16(dr1[0].ToString());
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
            dr1.Close();
        }
        //    ddlSemYr.Items.Insert(0, new ListItem("--Select--", "-1"));
        con.Close();
    }

    protected void ddlSemYr_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!Page.IsPostBack == false)
        {
            ddlSec.Items.Clear();
        }
        // BindSectionDetail();
        bindsec();
        bind_arrear_sem();
        BindExamYear();
        BindExamMonth();
        ddlletterformat.Text = " - - -  Select - - - ";
    }

    protected void ddlSec_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindExamYear();
        BindExamMonth();
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            txtxlname.Text = string.Empty;
            con_sem2.Close();
            con_sem2.Open();
            string q = "select distinct current_semester from registration  where batch_year='" + ddlBatch.SelectedItem + "' and degree_code='" + ddlBranch.SelectedValue.ToString() + "' and delflag='0'";
            SqlCommand com_sem2 = new SqlCommand(q, con_sem2);
            SqlDataReader sdr_sem2 = com_sem2.ExecuteReader();
            sdr_sem2.Read();
            if (sdr_sem2.HasRows == true)
            {
                Session["sem2"] = sdr_sem2["current_semester"];
            }
            btnClose.Visible = true;
            rdGrade.Visible = true;
            rdMark.Visible = true;
            btnLoad.Visible = true;
            lbl_selectall.Visible = true;
            chk_select_all.Visible = true;
            lbl_hideall.Visible = true;
            chk_hide_all.Visible = true;
            Panel5.Visible = true;
            // btnLetterFormat.Visible = true;
            // btnletterformat1.Visible = true;
            // tamilbutton.Visible = true;
            FpExternal.Visible = true;
            btnxl.Visible = true;//added by srinath 24/5/2014
            lblxl.Visible = true;
            txtxlname.Visible = true;
            //string strStudents =string.Empty;
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            FpExternal.Sheets[0].ColumnCount = 0;
            batch_year = ddlBatch.SelectedValue.ToString();
            FpExternal.Sheets[0].ColumnCount = 6;//'------------------new
            FpExternal.Sheets[0].RowCount = 0;
            //FpExternal.Sheets[0].ColumnHeader.RowCount = 11;//'-----changed from 10 to 11 on 070712
            FpExternal.Sheets[0].ColumnHeader.RowCount = 5;//added by srinath 28/8/2013
            FpExternal.Sheets[0].Columns[0].Width = 50;//===changed from 50 to 100 29.06.12
            FpExternal.Sheets[0].Columns[3].Width = 100;
            FpExternal.Sheets[0].Columns[4].Width = 250;//=====changed 05.07.12
            FpExternal.Sheets[0].Columns[2].Width = 100;
            FpExternal.Sheets[0].Columns[2].Locked = true;
            FpExternal.Sheets[0].Columns[3].Locked = true;
            FpExternal.Sheets[0].Columns[4].Locked = true;
            FpExternal.Sheets[0].AutoPostBack = false;
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Size = FontUnit.Medium;
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Name = "Book Antiqua";
            FpExternal.ActiveSheetView.ColumnHeader.DefaultStyle.Font.Bold = true;
            //Hidden by srinath 28/8/2013
            //FpExternal.Sheets[0].SheetCorner.Cells[4, 0].Text = "S.No";//'------------------------------new
            //FpExternal.Sheets[0].SheetCorner.Cells[4, 0].Font.Size = FontUnit.Medium;
            //FpExternal.Sheets[0].SheetCorner.Cells[4, 0].Font.Name = "Book Antiqua";
            //FpExternal.Sheets[0].SheetCorner.Cells[4, 0].Font.Bold = true;
            FpExternal.VerticalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpExternal.HorizontalScrollBarPolicy = FarPoint.Web.Spread.ScrollBarPolicy.AsNeeded;
            FpExternal.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpExternal.Sheets[0].DefaultStyle.Font.Bold = false;
            FpExternal.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
            FpExternal.Sheets[0].RowHeader.Visible = false;
            External_Students();
            // function_radioheader();
            // func_footer();
            // set_batch_degree_branch();
            if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) == 0)
            {
                lblnorec.Visible = true;
                Buttontotal.Visible = false;
                FpExternal.Visible = false;
                btnxl.Visible = false;//added by srinath 24/5/2014
                lblxl.Visible = false;
                txtxlname.Visible = false;
                btnLoad.Visible = false;
                rdGrade.Visible = false;
                rdMark.Visible = false;
                lbl_selectall.Visible = false;
                chk_select_all.Visible = false;
                lbl_hideall.Visible = false;
                chk_hide_all.Visible = false;
                btnPrint.Visible = false;
                btnLetterFormat.Visible = false;
                tamilbutton.Visible = false;
                btnxl.Visible = false;//added by srinath 24/5/2014
                lblxl.Visible = false;
                txtxlname.Visible = false;
                divSendSMS.Visible = false;
            }
            else
            {
                btnLoad.Visible = true;
                btnxl.Visible = true;//added by srinath 24/5/2014
                lblxl.Visible = true;
                txtxlname.Visible = true;
                //tamilbutton.Visible = true;
                Buttontotal.Visible = true;
                FpExternal.Visible = true;
                btnxl.Visible = true;//added by srinath 24/5/2014
                lblnorec.Visible = false;
                rdGrade.Visible = true;
                rdMark.Visible = true;
                lbl_selectall.Visible = true;
                chk_select_all.Visible = true;
                lbl_hideall.Visible = true;
                chk_hide_all.Visible = true;
                divSendSMS.Visible = true; //added by Prabha 17/11/2017
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            FpExternal.Sheets[0].PageSize = Convert.ToInt32(totalRows);
            FpExternal.Height = (FpExternal.Sheets[0].RowCount * 20) + 200;
        }
        catch (Exception ex)
        {
            divSendSMS.Visible = false;
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void External_Students()
    {
        try
        {
            if (ddlSec.Items.Count > 0)
            {
                sections = ddlSec.SelectedValue.ToString();
                if (sections.ToString().ToLower() == "all" || sections.ToString() == "" || sections.ToString() == "-1")
                {
                    strsec = string.Empty;
                }
                else
                {
                    strsec = " and registration.sections='" + sections.ToString() + "'";
                }
            }
            //'-------------------- select Exam_Code
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            ExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
            //SetHeader
            if ((exam_year != ""))
            {
                // IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), Convert.ToInt32(semdec), Convert.ToInt32(batch_year));
                IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
                if (IntExamCode > 0)
                {
                    if (LoadSubject(IntExamCode) > 0)
                    {
                        string grade = daccess.GetFunction("select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "");
                        if (grade.Trim() != "" && grade.Trim() != "0")
                        {
                            Load_Students(ExamCode);
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Please Set Grade Flag and then proceed";
                        }
                        //grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
                        //con_Grade1.Close();
                        //con_Grade1.Open();
                        //cmd = new SqlCommand(grade, con_Grade1);
                        //SqlDataReader drexgrade;
                        //drexgrade = cmd.ExecuteReader();
                        //while (drexgrade.Read())
                        //{
                        //    if (drexgrade.HasRows == true)
                        //    {
                        //        Load_Students(ExamCode);
                        //        // FpExternal.Sheets[0].RowCount+=1;
                        //    }
                        //    else
                        //    {
                        //    }
                        //}//'----- end while(drexgrade)
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

    public int Get_UnivExamCode(int DegreeCode, int Semester, int Batch, int exammonth, int examyear)
    {
        string GetUnivExamCode = string.Empty;
        try
        {
            string degree_code = string.Empty;
            string current_sem = string.Empty;
            string batch_year = string.Empty;
            //  degree_code = ddlBranch.SelectedValue.ToString();
            //   current_sem = ddlSemYr.SelectedValue.ToString();
            //    batch_year = ddlBatch.SelectedValue.ToString();
            string strExam_code = string.Empty;
            strExam_code = "Select Exam_Code from Exam_Details where Degree_Code = " + DegreeCode.ToString() + " and Current_Semester = " + Semester.ToString() + " and Batch_Year = " + Batch.ToString() + " and exam_month='" + exammonth + "' and exam_year='" + examyear + "'";
            con_Examcode.Close();
            con_Examcode.Open();
            SqlDataReader dr_examcode;
            SqlCommand cmd_examcode = new SqlCommand(strExam_code, con_Examcode);
            dr_examcode = cmd_examcode.ExecuteReader();
            while (dr_examcode.Read())
            {
                if (dr_examcode.HasRows == true)
                {
                    if (dr_examcode["Exam_Code"].ToString() != "")
                    {
                        GetUnivExamCode = dr_examcode["Exam_Code"].ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        if (GetUnivExamCode != "")
        {
            return Convert.ToInt32(GetUnivExamCode);
        }
        else
        {
            return 0;
        }
    }

    public int LoadSubject(int intExamCode)
    {
        int IntSCount = 0;
        try
        {
            int i = 0;
            int Stno = 0;
            string Stype = string.Empty;
            string strsubject = string.Empty;
            string grade = string.Empty;
            string degree_code = string.Empty;
            string current_sem = string.Empty;
            string batch_year = string.Empty;
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            FpExternal.Sheets[0].ColumnHeader.Rows[FpExternal.Sheets[0].ColumnHeader.RowCount - 1].BackColor = Color.AliceBlue;//===06.07.12
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Text = "S.No";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Text = "RollNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Text = "RegNo";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Text = "Student Name";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Text = "Student Type";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 2].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 3].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 4].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 5].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 1].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 1].Font.Bold = true;
            //FpExternal.Sheets[0].Columns[4].Width = 200;
            //FpExternal.Sheets[0].Columns[2].Width = 90;
            //@@@@@@@@@@@@@ added on 06.07.12@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            string arr_sem_in = string.Empty;
            //=====get the sem for arrear from the chkarrear_Sem
            for (int xarr = 0; xarr < chkarrear_Sem.Items.Count; xarr++)
            {
                if (chkarrear_Sem.Items[xarr].Selected == true)
                {
                    if (arr_sem_in == "")
                    {
                        arr_sem_in = chkarrear_Sem.Items[xarr].Value;
                    }
                    else
                    {
                        arr_sem_in = arr_sem_in + "," + chkarrear_Sem.Items[xarr].Value;
                    }
                }
            }
            if (arr_sem_in != string.Empty)
            {
                arr_sem_in = " and semester in(" + arr_sem_in + ")";



            }
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
            ////strsubject = "Select distinct Subject.Subject_No, isnull(Subject_Code,'') as Subject_Code,credit_points, (select distinct subject_type from subject s,sub_sem ss where s.subtype_no=ss.subtype_no and subject_no=subject.subject_no) subtype  from Mark_Entry,Subject,Syllabus_Master where Syllabus_Master.Syll_Code = Subject.Syll_Code and SubjecT_Code is not null and Syllabus_Master.Semester = " + semdec + " and Degree_Code = " + degree_code + " and Batch_Year = " + batch_year + " and Mark_Entry.Subject_No =  Subject.Subject_No and  Exam_Code = " + intExamCode + " and Type='' and attempts=1 Order by subtype desc,subject.subject_no ";//hided on 05.07.12
            if (chksubjtype.Items[0].Selected == true && chksubjtype.Items[1].Selected == true) //both
            {
                strsubject = "Select distinct subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + intExamCode + "  " + arr_sem_in.ToString() + "  order by semester desc,subject_type desc, mark_entry.subject_no asc";
            }
            else if (chksubjtype.Items[0].Selected == true)//for regular paper
            {
                strsubject = "Select distinct subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + intExamCode + " and attempts=1 order by semester desc,subject_type desc, mark_entry.subject_no asc";
            }
            else if (chksubjtype.Items[1].Selected == true) // for arrear paper
            {
                strsubject = "Select distinct subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + intExamCode + " and attempts<>1 " + arr_sem_in.ToString() + " order by semester desc,subject_type desc, mark_entry.subject_no asc";
            }
            else if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected != true)//both not selected
            {
                strsubject = "Select distinct subject_name,subject_code as Subject_Code,mark_entry.subject_no as Subject_No,semester,subject_type as Subtype,credit_points from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + intExamCode + "  order by semester desc,subject_type desc, mark_entry.subject_no asc";
            }
            con_loadSubject.Close();
            con_loadSubject.Open();
            SqlCommand cmd_loadSub = new SqlCommand(strsubject, con_loadSubject);
            SqlDataReader dr_loadSub;
            dr_loadSub = cmd_loadSub.ExecuteReader();
            getgradeflag = daccess.GetFunction("select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "");
            while (dr_loadSub.Read())
            {
                if (dr_loadSub["Subject_Code"].ToString() != "")
                {
                    //grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
                    //cmd = new SqlCommand(grade, con_Grade);
                    //con_Grade.Close();
                    //con_Grade.Open();
                    //SqlDataReader dr_grade;
                    //dr_grade = cmd.ExecuteReader();
                    //while (dr_grade.Read())
                    //{
                    //    if (dr_grade.HasRows == true)
                    //    {
                    if (getgradeflag.Trim() != "" && getgradeflag.Trim() != "0")
                    {
                        //  getgradeflag = Convert.ToString(dr_grade["grade_flag"]);
                        getsubno = Convert.ToString(dr_loadSub["Subject_No"]);
                        //'---------------------------- setting the chkbox cell type
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, 1].Text = "Select";
                        FarPoint.Web.Spread.CheckBoxCellType chkcell = new FarPoint.Web.Spread.CheckBoxCellType();
                        FpExternal.Sheets[0].Columns[1].CellType = chkcell;
                        //'----------------------------------------------------
                        FpExternal.Sheets[0].ColumnCount += 2;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, FpExternal.Sheets[0].ColumnCount - 2].Note = getsubno;
                        //'--------------new
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(FpExternal.Sheets[0].ColumnHeader.RowCount - 5, FpExternal.Sheets[0].ColumnCount - 2, 1, 2);
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(FpExternal.Sheets[0].ColumnHeader.RowCount - 5, FpExternal.Sheets[0].ColumnCount - 2, 1, 2);
                        //'--------------------------
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, FpExternal.Sheets[0].ColumnCount - 2].Text = dr_loadSub["Subject_Code"].ToString();
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 4, FpExternal.Sheets[0].ColumnCount - 2].Text = dr_loadSub["credit_points"].ToString();
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = "Grade/Mark";
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "Result";
                        FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 2].Locked = true;
                        //@@@@@@@@@@@@@ added on 29.06.12
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 4, FpExternal.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 4, FpExternal.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 4, FpExternal.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, FpExternal.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Font.Bold = true;
                        FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 2].Width = 50;
                        FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 2].Width = 90;
                        //@@@@@@@@@@@@
                        i = i + 1;
                        if (Stype != dr_loadSub["Subtype"].ToString())
                        {
                            flag = false;
                            if (i > 1)
                            {
                                Stno = 4;
                            }
                            Stype = dr_loadSub["Subtype"].ToString();
                            i = 1;
                        }
                        IntSCount = IntSCount + 1;
                        //    }
                    }
                }
            } //'--- end dr_loadsub
            Session["colcount"] = FpExternal.Sheets[0].ColumnCount;
            FpExternal.Sheets[0].ColumnCount++;
            rcnt = FpExternal.Sheets[0].ColumnCount - 1;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(FpExternal.Sheets[0].ColumnHeader.RowCount - 5, rcnt, 4, 1);
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, rcnt].Text = "GPA";
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, rcnt].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 5, rcnt].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, rcnt].Text = " ";
            FpExternal.Sheets[0].Columns[rcnt].Locked = true;
            FpExternal.Sheets[0].ColumnCount++;
            rcnt = FpExternal.Sheets[0].ColumnCount - 1;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, rcnt, 4, 1);
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Text = "CGPA";
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, rcnt].Text = " ";
            FpExternal.Sheets[0].ColumnCount++;
            rcnt = FpExternal.Sheets[0].ColumnCount - 1;
            FpExternal.Sheets[0].Columns[rcnt].Locked = true;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, rcnt, 4, 1);
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Text = "Result";
            FpExternal.Sheets[0].Columns[rcnt].Locked = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, rcnt].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[FpExternal.Sheets[0].ColumnHeader.RowCount - 1, rcnt].Text = " ";
            if (flag == false)
            {
            }
            else
            {
            }
            column_count = FpExternal.Sheets[0].ColumnCount;
            if (Session["Rollflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[3].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[5].Visible = false;
            }
            FarPoint.Web.Spread.StyleInfo style = new FarPoint.Web.Spread.StyleInfo();
            style.Font.Size = 10;
            style.Font.Bold = true;
            FpExternal.Sheets[0].SheetCornerStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpExternal.Sheets[0].ColumnHeader.DefaultStyle = new FarPoint.Web.Spread.StyleInfo(style);
            FpExternal.Sheets[0].ColumnHeader.DefaultStyle.HorizontalAlign = HorizontalAlign.Center;
            int secn_vsbl_cnt = 0;
            int secon_vsbl = 0;
            //'---------------------------------------new
            if (Convert.ToInt32(Session["Rollflag"]) == 1 && Convert.ToInt32(Session["Regflag"]) == 1)
            {
                secon_vsbl = 3;
            }
            else if (Convert.ToInt32(Session["Rollflag"]) == 1)
            {
                secon_vsbl = 3;
            }
            else if (Convert.ToInt32(Session["Regflag"]) == 1)
            {
                secon_vsbl = 4;
            }
            int secon_vsbl1 = secon_vsbl;
            for (secon_vsbl = secon_vsbl1; secon_vsbl < column_count; secon_vsbl++)
                FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 2].Border.BorderColorRight = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorLeft = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 1, 6);
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Text = "Subject Code";
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorLeft = Color.Black;
            FpExternal.Sheets[0].SheetCorner.Cells[1, 0].Border.BorderColorRight = Color.Black;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 0, 1, 6);
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Text = "Credits";
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[1, 0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 0, 1, 6);
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 0].Text = "MinMarks";
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[2, 0].Font.Bold = true;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 0, 1, 6);
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 0].Text = "MaxMarks";
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 0].Font.Size = FontUnit.Medium;
            FpExternal.Sheets[0].ColumnHeader.Cells[3, 0].Font.Bold = true;
            //@@@@@@@@@@@@@@@@ added vsbl setting on 06.07.12
            if (chkvsbl_setting.Items[0].Selected == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Rows[2].Visible = true;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Rows[2].Visible = false;
            }
            if (chkvsbl_setting.Items[1].Selected == true)
            {
                FpExternal.Sheets[0].ColumnHeader.Rows[3].Visible = true;
            }
            else
            {
                FpExternal.Sheets[0].ColumnHeader.Rows[3].Visible = false;
            }
            if (chkvsbl_setting.Items[2].Selected == true)
            {
                FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 1].Visible = true;
            }
            else
            {
                FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 1].Visible = false;
            }
            FpExternal.Sheets[0].ColumnHeader.Rows[4].BackColor = Color.AliceBlue;
            //@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return IntSCount;
    }

    public void Load_Students(int ExamCode)
    {
        try
        {
            FpExternal.Sheets[0].SheetName = "  ";
            string grade_get = string.Empty;
            string gpa = string.Empty;
            string strStudents = string.Empty;
            string grade = string.Empty;
            bool chkflag = false;
            bool failflag = false;
            //'--------new
            string mintotal = string.Empty;
            string maxtotal = string.Empty;
            string display_cgpa = string.Empty;
            string section = string.Empty;
            //----------------------added on 29.06,12
            if (ddlSec.Text != string.Empty)
            {
                section = ddlSec.SelectedItem.Text;
            }
            //-----------------------------------------------
            if (Session["Rollflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[2].Visible = false;
            }
            if (Session["Regflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[3].Visible = false;
            }
            if (Session["Studflag"].ToString() == "0")
            {
                FpExternal.Sheets[0].ColumnHeader.Columns[5].Visible = false;
            }
            string gradesett = "select * from gradesettings where college_code =" + Session["collegecode"] + "";
            string edulvl = d2.GetFunction("select Edu_Level from course  where Course_Id='" + ddlDegree.SelectedValue.ToString() + "' and college_code=" + Session["collegecode"] + "");

            DataSet gradesettings = d2.select_method_wo_parameter(gradesett, "Text");
            SqlDataReader dr_grade_val;
            con.Close();
            con.Open();
            cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and college_code=" + Session["collegecode"] + "", con);
            dr_grade_val = cmd.ExecuteReader();
            while (dr_grade_val.Read())
            {
                if (dr_grade_val.HasRows == true)
                {
                    grade_setting = dr_grade_val[0].ToString();
                }
            }
            //===============load the arrear students only
            if (chksubjtype.Items[1].Selected == true && chksubjtype.Items[0].Selected != true)
            {
                strStudents = "select distinct mark_entry.roll_no as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode from registration,mark_entry where mark_entry.roll_no=registration.roll_no and mark_entry.attempts<>1 and exam_code=" + IntExamCode + " order by RgNo";
            }
            else
            {
                if (section.ToUpper().Trim() == "ALL")
                {
                    strStudents = "Select isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode,applyn.Student_Mobile ,applyn.parentF_Mobile,applyn.parentM_Mobile from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.app_no=applyn.app_no and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' order by len(registration.Reg_No),registration.Reg_No ";
                }
                else
                {
                    strStudents = "Select isnull(registration.Roll_No,'') as RlNo,isnull(registration.Reg_No,'') as RgNo ,isnull(registration.Stud_Name,'') as SName,isnull(registration.stud_type,'') as type,roll_admit,registration.mode as mode,applyn.Student_Mobile ,applyn.parentF_Mobile,applyn.parentM_Mobile from registration,applyn where registration.Degree_Code = " + degree_code + " and registration.Batch_Year = " + batch_year + " " + Session["strvar"] + " and registration.Current_Semester >= " + semdec + " and registration.sections='" + section.ToString() + "' and registration.app_no=applyn.app_no  and delflag =0 and exam_flag <>'Debar' and RollNo_Flag=1 and Roll_No is not null and ltrim(rtrim(Roll_No)) <>'' order by len(registration.Reg_No),registration.Reg_No ";
                }
            }
            //===========================================
            con_Stud.Close();
            con_Stud.Open();
            SqlCommand cmd_Subject = new SqlCommand(strStudents, con_Stud);
            SqlDataReader dr_Students;
            dr_Students = cmd_Subject.ExecuteReader();
            int attept = 0, maxmrk = 0;
            string getattmaxmark = daccess.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + Session["collegecode"].ToString() + "'");
            string[] semecount = getattmaxmark.Split(new Char[] { '-' });
            if (semecount.GetUpperBound(0) == 1)
            {
                attept = Convert.ToInt32(semecount[0].ToString());
                maxmrk = Convert.ToInt32(semecount[1].ToString());
                flagchknew = true;
            }
            else
            {
                flagchknew = false;
            }
            grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
            getgradeflag = daccess.GetFunction(grade);
            string sql = string.Empty;
            if (chksubjtype.Items[0].Selected == true && chksubjtype.Items[1].Selected == true) //both regular and arrear
            {
                sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks,subject.mintotal from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " order by subject_type desc,mark_entry.subject_no";
            }
            else if (chksubjtype.Items[0].Selected == true) //for regular
            {
                sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks,subject.mintotal from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " and Attempts =1  order by subject_type desc,mark_entry.subject_no";
            }
            else if (chksubjtype.Items[1].Selected == true) //for arrear
            {
                sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks,subject.mintotal from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + " and Attempts<>1 order by subject_type desc,mark_entry.subject_no";
            }
            else if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected != true) //both not selected
            {
                sql = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks,subject.mintotal from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + ExamCode + "  order by subject_type desc,mark_entry.subject_no";
            }
            DataSet dsstumark = daccess.select_method_wo_parameter(sql, "text");
            string result = string.Empty;
            string failgrade = daccess.GetFunction("select value from COE_Master_Settings where settings='Fail Grade'");
            if (failgrade.Trim() == "" || failgrade.Trim() == "0")
            {
                failgrade = "-";
            }
            bool studflag = false;
            while (dr_Students.Read())
            {
                studflag = false;
                failflag = false;
                FarPoint.Web.Spread.TextCellType tt = new FarPoint.Web.Spread.TextCellType();
                FpExternal.Sheets[0].Columns[2].CellType = tt;
                FpExternal.Sheets[0].Columns[3].CellType = tt;
                chkflag = false;
                string stud = dr_Students["RlNo"].ToString();
                FpExternal.Sheets[0].RowCount += 1;
                serialno++;
                FpExternal.Sheets[0].Rows[0].Border.BorderColor = Color.Black;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = dr_Students["RlNo"].ToString(); //parentF_Mobile
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Tag = dr_Students["RlNo"].ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Note = Convert.ToString(dr_Students["parentF_Mobile"]);

                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 3, dr_Students["RgNo"].ToString());
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Note = Convert.ToString(dr_Students["parentM_Mobile"]);

                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 4, dr_Students["SName"].ToString());
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Note = Convert.ToString(dr_Students["Student_Mobile"]);
                FpExternal.Sheets[0].Columns[4].Width = 250;//=====changed 05.07.12
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 5, dr_Students["type"].ToString());

                FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, 0, serialno.ToString());
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 1].Value = 0;
                //gowthaman 13aug2013========================================================
                //gpa = Calulat_GPA(stud, ddlSemYr.SelectedValue.ToString());
                //display_cgpa = Calculete_CGPA(stud, ddlSemYr.SelectedValue.ToString());
                string syll_code = string.Empty;
                syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
                if (chk_subjectwisegrade.Checked == true)
                {
                    gpa = Calulat_GPA_cgpaformate1(dr_Students["RlNo"].ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());

                }
                else
                {
                    string arrear = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + Convert.ToString(dr_Students["RlNo"]) + "') and roll_no ='" + Convert.ToString(dr_Students["RlNo"]) + "'  and subject.syll_code=" + syll_code.ToString() + ")";//magesh 31.8.18
                    DataSet dscheckresult1 = d2.select_method(arrear, hat, "Text");

                    string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");
                    if (val1.Trim() == "true" || val1.Trim() == "1")
                    {
                        if (chkincludeRoundOff.Checked)
                        {
                            gpa = Calulat_GPA_Semwiseforpg(dr_Students["RlNo"].ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                        }
                        else
                        {
                            gpa = commonaccess.Calulat_GPA_Semwise(dr_Students["RlNo"].ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                        }
                        if (gpa != "0")
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = gpa;
                    }
                    else
                    {

                        if (dscheckresult1.Tables[0].Rows.Count == 0)
                        {
                            if (chkincludeRoundOff.Checked)
                            {
                                gpa = Calulat_GPA_Semwiseforpg(dr_Students["RlNo"].ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                            }
                            else
                            {
                                gpa = commonaccess.Calulat_GPA_Semwise(dr_Students["RlNo"].ToString(), degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                            }
                            if (gpa != "0")
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = gpa;
                        }
                    }
                }
                string mode = string.Empty;
                mode = dr_Students["mode"].ToString();
                //string syll_code = string.Empty;
                //syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
                string checkresult = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + gregisternumber + "') and roll_no ='" + gregisternumber + "'  and subject.syll_code=" + syll_code.ToString() + " )";
                DataSet dscheckresult = commonaccess.select_method(checkresult, hat, "Text");
                if (dscheckresult.Tables[0].Rows.Count > 0)
                {
                    display_cgpa = " ";
                }
                else
                {
                    if (chk_subjectwisegrade.Checked == true)
                    {
                        if (chkincludeRoundOff.Checked)
                        {
                            display_cgpa = Calulat_CGPA_cgpaformate1(dr_Students["RlNo"].ToString(), Session["Semester"].ToString(), degree_code, batch_year, mode, Session["collegecode"].ToString());
                        }
                        else
                        {
                            display_cgpa = Calulat_CGPA_cgpaformate1(dr_Students["RlNo"].ToString(), Session["Semester"].ToString(), degree_code, batch_year, mode, Session["collegecode"].ToString());
                        }
                    }
                    else
                    {
                        if (chkincludeRoundOff.Checked)
                        {
                            display_cgpa = Calculete_CGPAPG(dr_Students["RlNo"].ToString(), Session["Semester"].ToString(), degree_code, batch_year, mode, Session["collegecode"].ToString());
                        }
                        else
                        {
                            display_cgpa = commonaccess.Calculete_CGPA(dr_Students["RlNo"].ToString(), Session["Semester"].ToString(), degree_code, batch_year, mode, Session["collegecode"].ToString());
                        }
                    }
                }
                //===========================================================================
                Hashtable hatsub = new Hashtable();
                for (int col = 6; col <= FpExternal.Sheets[0].ColumnCount - 4; col += 2)
                {
                    //grade = "select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year='" + batch_year + "' and exam_month=" + exam_month + " and exam_year= " + exam_year + "";
                    //con_Grade_flag.Close();
                    //con_Grade_flag.Open();
                    //SqlDataReader drgrade;
                    //SqlCommand cmd_grade = new SqlCommand(grade, con_Grade_flag);
                    //drgrade = cmd_grade.ExecuteReader();
                    //while (drgrade.Read())
                    if (getgradeflag.Trim() != "" && getgradeflag.Trim() != "0")
                    {
                        // getgradeflag = drgrade["grade_flag"].ToString();
                        getsubno = FpExternal.Sheets[0].ColumnHeader.Cells[0, col].Note.ToString();
                        //'------------------------------get the  minmark and maxmark ----
                        if (getsubno != "")
                        {
                            if (!hatsub.Contains(getsubno))
                            {
                                string getminmaxmark = "select mintotal,maxtotal from subject where subject_no='" + getsubno.ToString() + "'";
                                DataSet ds_getmrk = new DataSet();
                                SqlDataAdapter da_getmrk = new SqlDataAdapter(getminmaxmark, con);
                                con.Close();
                                con.Open();
                                da_getmrk.Fill(ds_getmrk);
                                maxtotal = ds_getmrk.Tables[0].Rows[0]["maxtotal"].ToString();
                                mintotal = ds_getmrk.Tables[0].Rows[0]["mintotal"].ToString();
                                hatsub.Add(getsubno, maxtotal + '-' + mintotal);
                            }
                            else
                            {
                                string[] stv = hatsub[getsubno].ToString().Split('-');
                                mintotal = stv[1].ToString();
                                maxtotal = stv[0].ToString();
                            }
                        }
                        //'---------------------------------------------------
                        Session["e_code"] = ExamCode;
                        ////   sql = "select s.subject_name as subjectname,s.subject_no,s.min_ext_marks,s.min_int_marks,m.internal_mark,m.external_mark from Subject s,mark_entry m,exam_details ex where s.subject_no=m.subject_no  and m.roll_no='" + dr_Students["RlNo"].ToString() + "' and m.exam_code=ex.exam_code and  ex.degree_code='" + Session["Branchcode"] + "' and ex.current_semester='" + Session["Semester"] + "'";
                        //@@@ query on 10.08.12--------------
                        //select * from subjectchooser,subject where subjectchooser.subject_no=2530 and subjectchooser.roll_no='112086' and subject.subtype_no=subjectchooser.subtype_no
                        //@@@@ 
                        //select distinct subject.subject_name,subject.subject_no from subjectchooser,subject where subjectchooser.roll_no='112001' and subject.subtype_no=subjectchooser.subtype_no and semester=3 and subjectchooser.subject_no=subject.subject_no
                        //select distinct subject.subject_name,subject.subject_no from subjectchooser,subject where subjectchooser.roll_no='112086' and subject.subtype_no=subjectchooser.subtype_no and subjectchooser.subject_no=subject.subject_no  and subjectchooser.subject_no='2812'
                        //=======added on 05.07.12
                        //================================
                        //con_mrkentry.Close();
                        //con_mrkentry.Open();
                        //SqlDataReader drmrkentry;
                        //SqlCommand cmd_mrkentry = new SqlCommand(sql, con_mrkentry);
                        //drmrkentry = cmd_mrkentry.ExecuteReader();
                        dsstumark.Tables[0].DefaultView.RowFilter = "roll_no='" + dr_Students["RlNo"].ToString() + "' and subject_no='" + getsubno + "'";
                        DataView dvstumark = dsstumark.Tables[0].DefaultView;
                        //if (drmrkentry.HasRows == true)
                        //{
                        //while (drmrkentry.Read())
                        //{
                        if (dvstumark.Count > 0)
                        {
                            studflag = true;
                            result = dvstumark[0]["result"].ToString();
                            //if (drmrkentry["grade"].ToString() != "")
                            //{
                            if (Convert.ToInt32(getgradeflag) == 1)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Text = mintotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Text = maxtotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Size = FontUnit.Medium;
                                ////   FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, drmrkentry["grade"].ToString());   //old
                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col + 1, result.ToString());

                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();

                                if ((dvstumark[0]["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
                                {
                                    //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = "LE";
                                }
                                //else if (string.Equals(result.Trim().ToLower(), "sa"))
                                //{
                                //    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "SA");
                                //}
                                else if ((result == "AAA") || (result == "-1"))
                                {
                                    //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = "AAA";
                                }
                                else
                                {
                                    if (grade_setting == "0")//if 0 means display only marks(in settings mark conversion unchecked)
                                    {
                                        if (dvstumark[0]["total"].ToString() != "")
                                        {
                                            if (Convert.ToInt32(dvstumark[0]["total"].ToString()) < Convert.ToInt32(mintotal))
                                            {
                                                failflag = true;
                                            }
                                        }
                                        //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, dvstumark[0]["total"].ToString());
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dvstumark[0]["total"]);
                                    }
                                    else//grade_setting 1 means display corresponding grade for mark(in setting mark conversion checked)
                                    {
                                        if (Convert.ToInt16(dvstumark[0]["internal_mark"].ToString()) >= Convert.ToInt16(dvstumark[0]["min_int_marks"].ToString()) && Convert.ToInt16(dvstumark[0]["External_mark"].ToString()) >= Convert.ToInt16(dvstumark[0]["min_ext_marks"].ToString()))
                                        {
                                            convertgrade(stud, getsubno);
                                            result = "Pass";
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Note = getsubno.ToString();
                                        }
                                        else
                                        {
                                            //=====new 07.07.12
                                            //con.Close();
                                            //con.Open();
                                            //SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                            //SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                            //dr_failgrade = cmd_failgrade.ExecuteReader();
                                            //if (dr_failgrade.HasRows == true)
                                            //{
                                            //    if (dr_failgrade.Read())
                                            //    {
                                            //        if (dr_failgrade["value"].ToString() != "")
                                            //        {
                                            //            failgrade = dr_failgrade["value"].ToString();
                                            //        }
                                            //    }
                                            //}
                                            //else
                                            //{
                                            //    failgrade = "-";
                                            //}
                                            //===============07.07.12
                                            //   funcgrade = "RA";
                                            result = "Fail";
                                            failflag = true;
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = failgrade.ToString();
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = getsubno.ToString();
                                        }
                                    }
                                }
                                if (chkflag == false)
                                {
                                    if (result.Trim() != "" && result != null)//Added By Srinath 27/03/2014 =Start
                                    {
                                        if (result == "Pass")
                                        {
                                            //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = result;
                                        }
                                        else
                                        {
                                            if ((dvstumark[0]["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "LE";
                                                chkflag = true;
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "Fail";
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }//===========================================End
                                }
                                //@@@@@@ added on 29.06.12 by mythili visible false the column result
                                FpExternal.Sheets[0].Columns[col + 1].Visible = false;
                                //@@@@@@@@@@@@@@@@@@@@@@
                            }
                            if (Convert.ToInt32(getgradeflag) == 2)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Text = mintotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Text = maxtotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Size = FontUnit.Medium;
                                gradesettings.Tables[0].DefaultView.RowFilter = "ActualGrade='" + dvstumark[0]["grade"].ToString() + "'";
                                DataView dvholiday = gradesettings.Tables[0].DefaultView;
                                if (dvholiday.Count > 0)
                                {
                                    if (Convert.ToString(dvholiday[0]["grade"]) != "")
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = Convert.ToString(dvholiday[0]["grade"]);

                                    }
                                    else
                                    {
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = dvstumark[0]["grade"].ToString();
                                    }
                                }
                                else
                                {
                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = dvstumark[0]["grade"].ToString();
                                }


                                //  FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = dvstumark[0]["grade"].ToString();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();
                                if ((dvstumark[0]["grade"].ToString() == "") && (dr_Students["mode"].ToString() == "3"))
                                {
                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
                                }
                                //else if (string.Equals(result.Trim().ToLower(), "sa"))
                                //{
                                //    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "SA");
                                //}
                                else if ((result == "AAA") || (result == "-1"))
                                {
                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
                                }
                                if (chkflag == false)
                                {
                                    if (result.Trim() != "" && result != null)//Added By Srinath 27/03/2014 =Start
                                    {
                                        if (result == "Pass")
                                        {
                                            //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, result);
                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = result;
                                        }
                                        else
                                        {
                                            if ((dvstumark[0]["grade"].ToString() == "") && (dr_Students["mode"].ToString() == "3"))
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "LE");
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "LE";
                                                chkflag = true;
                                            }
                                            else
                                            {
                                                //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1, "Fail");
                                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 1].Text = "Fail";
                                                chkflag = true;
                                                failflag = true;
                                            }
                                        }
                                    }//=======================end
                                }
                                //@@@@@@ added on 29.06.12 by mythili visible false the column result
                                FpExternal.Sheets[0].Columns[col + 1].Visible = false;
                                //@@@@@@@@@@@@@@@@@@@@@@
                            }
                            if (Convert.ToInt32(getgradeflag) == 3)//gradeflag=3 means display only marks(based on grade setting)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, col, 1, 2);
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Text = mintotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Text = maxtotal.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[2, col].Font.Size = FontUnit.Medium;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[3, col].Font.Size = FontUnit.Medium;
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = dvstumark[0]["grade"].ToString();
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();
                                if ((dvstumark[0]["total"].ToString() == "0") && (dr_Students["mode"].ToString() == "3"))
                                {
                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "LE");
                                    if (FpExternal.Sheets[0].GetText(FpExternal.Sheets[0].RowCount - 1, col) == "LE")
                                    {
                                        result = "LE";
                                    }
                                }
                                //else if (string.Equals(result.Trim().ToLower(), "sa"))
                                //{
                                //    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "SA");
                                //}
                                else if ((result == "AAA") || (result == "-1"))
                                {
                                    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, col, "AAA");
                                }
                                else
                                {
                                    if (grade_setting == "0")//if 0 means display only marks(in settings mark conversion unchecked)
                                    {
                                        if (dvstumark[0]["total"].ToString() != "")
                                        {
                                            if (Convert.ToDouble(dvstumark[0]["total"].ToString()) < Convert.ToDouble(mintotal))//added on 11.08.12 mythili
                                            {
                                                failflag = true;
                                            }
                                        }
                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = dvstumark[0]["total"].ToString();
                                    }
                                    else //grade_setting 1 means display corresponding grade for mark(in setting mark conversion checked)
                                    {
                                        if (chk_subjectwisegrade.Checked == true)
                                        {
                                            if (dvstumark[0]["result"].ToString().Trim().ToLower() != "pass")
                                            {
                                                result = "Fail";
                                                failflag = true;
                                            }
                                        }
                                        else
                                        {
                                            if (flagchknew == true)
                                            {
                                                double inte = 0, exte = 0, realattpt = 0;
                                                if ((dvstumark[0]["internal_mark"].ToString() != string.Empty) && (dvstumark[0]["External_mark"].ToString() != string.Empty) && (dvstumark[0]["min_int_marks"].ToString() != string.Empty) && (dvstumark[0]["min_ext_marks"].ToString() != string.Empty) && (dvstumark[0]["mintotal"].ToString() != string.Empty))
                                                {
                                                    inte = Convert.ToDouble(dvstumark[0]["internal_mark"].ToString());
                                                    exte = Convert.ToDouble(dvstumark[0]["external_mark"].ToString());
                                                    realattpt = Convert.ToInt32(dvstumark[0]["attempts"].ToString());
                                                    if (attept > realattpt)
                                                    {
                                                        if (Convert.ToDouble(dvstumark[0]["internal_mark"].ToString()) >= Convert.ToDouble(dvstumark[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvstumark[0]["External_mark"].ToString()) >= Convert.ToDouble(dvstumark[0]["min_ext_marks"].ToString()) && ((inte + exte) >= Convert.ToDouble((dvstumark[0]["mintotal"].ToString()))))
                                                        {
                                                            convertgradev(stud, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();
                                                        }
                                                        else
                                                        {
                                                            //=====new 07.07.12
                                                            //con.Close();
                                                            //con.Open();
                                                            //SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            //SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            //dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            //if (dr_failgrade.HasRows == true)
                                                            //{
                                                            //    if (dr_failgrade.Read())
                                                            //    {
                                                            //        if (dr_failgrade["value"].ToString() != "")
                                                            //        {
                                                            //            failgrade = dr_failgrade["value"].ToString();
                                                            //        }
                                                            //    }
                                                            //}
                                                            //else
                                                            //{
                                                            //    failgrade = "-";
                                                            //}
                                                            //===============07.07.12
                                                            //   funcgrade = "RA";//07.07.12
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = failgrade.ToString();
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (maxmrk <= exte)
                                                        {
                                                            convertgradev(stud, getsubno, maxmrk, attept);
                                                            result = "Pass";
                                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();
                                                        }
                                                        else
                                                        {
                                                            //=====new 07.07.12
                                                            //con.Close();
                                                            //con.Open();
                                                            //SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            //SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            //dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            //if (dr_failgrade.HasRows == true)
                                                            //{
                                                            //    if (dr_failgrade.Read())
                                                            //    {
                                                            //        if (dr_failgrade["value"].ToString() != "")
                                                            //        {
                                                            //            failgrade = dr_failgrade["value"].ToString();
                                                            //        }
                                                            //    }
                                                            //}
                                                            //else
                                                            //{
                                                            //    failgrade = "-";
                                                            //}
                                                            //===============07.07.12
                                                            //   funcgrade = "RA";//07.07.12
                                                            result = "Fail";
                                                            failflag = true;
                                                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = failgrade.ToString();
                                                        }
                                                    }
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();
                                                    //@@@@@@ added on 29.06.12 by mythili visible false the column result
                                                    FpExternal.Sheets[0].Columns[col + 1].Visible = false;
                                                    //@@@@@@@@@@@@@@@@@@@@@@
                                                }
                                            }
                                            else
                                            {
                                                if ((dvstumark[0]["internal_mark"].ToString() != string.Empty) && (dvstumark[0]["External_mark"].ToString() != string.Empty) && (dvstumark[0]["min_int_marks"].ToString() != string.Empty) && (dvstumark[0]["min_ext_marks"].ToString() != string.Empty) && (dvstumark[0]["mintotal"].ToString() != string.Empty))
                                                {
                                                    double inter = 0, exter = 0, tota = 0;
                                                    inter = Convert.ToDouble(dvstumark[0]["internal_mark"].ToString());
                                                    exter = Convert.ToDouble(dvstumark[0]["External_mark"].ToString());
                                                    tota = inter + exter;
                                                    if (Convert.ToDouble(dvstumark[0]["internal_mark"].ToString()) >= Convert.ToDouble(dvstumark[0]["min_int_marks"].ToString()) && Convert.ToDouble(dvstumark[0]["External_mark"].ToString()) >= Convert.ToDouble(dvstumark[0]["min_ext_marks"].ToString()) && (tota >= Convert.ToDouble((dvstumark[0]["mintotal"].ToString()))))
                                                    {
                                                        convertgrade(stud, getsubno);
                                                        result = "Pass";
                                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = funcgrade.ToString();
                                                    }
                                                    else
                                                    {
                                                        //=====new 07.07.12
                                                        //con.Close();
                                                        //con.Open();
                                                        //SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                        //SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                        //dr_failgrade = cmd_failgrade.ExecuteReader();
                                                        //if (dr_failgrade.HasRows == true)
                                                        //{
                                                        //    if (dr_failgrade.Read())
                                                        //    {
                                                        //        if (dr_failgrade["value"].ToString() != "")
                                                        //        {
                                                        //            failgrade = dr_failgrade["value"].ToString();
                                                        //        }
                                                        //    }
                                                        //}
                                                        //else
                                                        //{
                                                        //    failgrade = "-";
                                                        //}
                                                        //===============07.07.12
                                                        //   funcgrade = "RA";//07.07.12
                                                        result = "Fail";
                                                        failflag = true;
                                                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = failgrade.ToString();
                                                    }
                                                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col + 1].Text = result.ToString();
                                                    //@@@@@@ added on 29.06.12 by mythili visible false the column result
                                                    FpExternal.Sheets[0].Columns[col + 1].Visible = false;
                                                    //@@@@@@@@@@@@@@@@@@@@@@
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //}
                        }
                        //}
                        else //if no marks/grade for student then put -
                        {
                            string elective = "select distinct subject.subject_no,subject.min_int_marks,subject.min_ext_marks from subjectchooser,subject where subjectchooser.roll_no='" + dr_Students["RlNo"].ToString() + "' and subject.subtype_no=subjectchooser.subtype_no and subjectchooser.subject_no=subject.subject_no  and subjectchooser.subject_no='" + getsubno + "'";
                            SqlDataAdapter da_elect = new SqlDataAdapter(elective, con);
                            con.Close();
                            con.Open();
                            DataSet ds_elect = new DataSet();
                            da_elect.Fill(ds_elect);
                            if (ds_elect.Tables[0].Rows.Count > 0)
                            {
                                string int_ext_marks = "select internal_mark,external_mark from mark_entry where subject_no='" + ds_elect.Tables[0].Rows[0]["subject_no"] + "' and roll_no='" + dr_Students["RlNo"].ToString() + "' and exam_code=" + ExamCode + "";
                                SqlDataAdapter da_iemark = new SqlDataAdapter(int_ext_marks, con_new);
                                con_new.Close();
                                con_new.Open();
                                DataSet ds_iemark = new DataSet();
                                da_iemark.Fill(ds_iemark);
                                if (ds_iemark.Tables[0].Rows.Count > 0)
                                {
                                    if ((ds_iemark.Tables[0].Rows[0]["internal_mark"].ToString() != string.Empty) && (ds_iemark.Tables[0].Rows[0]["External_mark"].ToString() != string.Empty) && (ds_elect.Tables[0].Rows[0]["min_int_marks"].ToString() != string.Empty) && (ds_elect.Tables[0].Rows[0]["min_ext_marks"].ToString() != string.Empty))
                                    {
                                        if (Convert.ToDouble(ds_iemark.Tables[0].Rows[0]["internal_mark"].ToString()) < Convert.ToDouble(ds_elect.Tables[0].Rows[0]["min_int_marks"].ToString()) && Convert.ToDouble(ds_iemark.Tables[0].Rows[0]["External_mark"].ToString()) < Convert.ToDouble(ds_elect.Tables[0].Rows[0]["min_ext_marks"].ToString()))
                                        {
                                            failflag = true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, col].Text = "-";
                            }
                            // failflag = true;
                        }
                    }
                }
                if (chkflag == false) //@@@@ modified on 29.06.12
                {
                    if (failflag == true)
                    {
                        if (display_cgpa == "NaN")
                        {
                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 2), "0");
                        }
                        else
                        {
                            FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 2), "-");
                        }
                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, (FpExternal.Sheets[0].ColumnCount - 3), "-");
                        //@@@@@@@@@@@@@@@@@
                        //FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, "Fail");
                        FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, "RA");//changed
                        chkflag = true;
                    }
                    else
                    {
                        if (display_cgpa == "NaN")
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = "0";
                        }
                        else
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = display_cgpa;
                        }
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = gpa;
                        if (studflag == false)
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, rcnt].Text = " ";
                        else
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, rcnt].Text = "Pass";
                    }
                    ////if (result == "Pass")
                    ////{
                    ////    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, result);
                    ////}
                    ////else
                    ////{
                    ////    FpExternal.Sheets[0].SetText(FpExternal.Sheets[0].RowCount - 1, rcnt, "Fail");
                    ////    chkflag = true;
                    ////}
                }
                else
                {
                    string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");//rajkumar 28/5/2016
                    if (val1.Trim() == "1" || val1.Trim() == "true")
                    {
                        if (display_cgpa == "NaN")
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = "0";
                        }
                        else
                        {
                            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 2].Text = display_cgpa;
                        }
                        FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, FpExternal.Sheets[0].ColumnCount - 3].Text = gpa;
                    }
                }

            }
            FpExternal.Width = 1400;
            //FpExternal.Width =((FpExternal.Sheets[0].ColumnCount - 5)*80) + 550;
            //FpExternal.Sheets[0].AutoPostBack = true;
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 1, 1);
            FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 5, 1);
            if (chksubjtype.Items[1].Selected == true && chksubjtype.Items[0].Selected != true)
            {
                FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 2].Visible = false;
                FpExternal.Sheets[0].Columns[FpExternal.Sheets[0].ColumnCount - 3].Visible = false;
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void convertgradev(string roll, string subj, int maxmarkve, int attmptreal)
    {
        try
        {
            strexam = "Select subject_name,subject_code,internal_mark,external_mark,attempts,total,result,cp,mark_entry.subject_no from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + "  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";
            double inte = 0, exte = 0;
            int attmpt = 0;
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con_convertgrade);
            con_convertgrade.Close();
            con_convertgrade.Open();
            dr_convert = cmd_exam1.ExecuteReader();
            while (dr_convert.Read())
            {
                //   funcsemester = dr_convert["semester"].ToString();
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
                funcresult = dr_convert["result"].ToString();
                funccredit = dr_convert["cp"].ToString();
                mark = dr_convert["total"].ToString();
                // string edulevel = d2.GetFunction("select edu_level from course  where course_id='" + ddlDegree.SelectedValue.ToString() + "'");
                if (chkincludeRoundOff.Checked == true) //added by Mullai
                {
                    double mk1 = Math.Round(Convert.ToDouble(mark), 0, MidpointRounding.AwayFromZero);
                    mark = Convert.ToString(mk1);
                }

                inte = Convert.ToDouble(dr_convert["internal_mark"].ToString());
                exte = Convert.ToDouble(dr_convert["external_mark"].ToString());
                attmpt = Convert.ToInt32(dr_convert["attempts"].ToString());
                funcgrade = string.Empty;
                string strgrade = string.Empty;
                if (attmptreal > attmpt)
                {
                    if (dr_convert["total"].ToString() != string.Empty)
                    {
                        strgrade = "select mark_grade from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + mark + " between frange and trange";
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
                SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
                con_Grade.Close();
                con_Grade.Open();
                SqlDataReader dr_grade;
                dr_grade = cmd_grade.ExecuteReader();
                if (dr_grade.HasRows == true)
                {
                    while (dr_grade.Read())
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
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }


    public string Calulat_GPA_Semwiseforpg(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        string ccva = "";
        string strgrade = "";
        double creditval = 0;
        double finalgpa1 = 0;
        double creditsum1 = 0;
        double gpacal1 = 0;
        string strsubcrd = "";
        string examcodeval = "";
        double strtot = 0;
        double strgradetempfrm = 0;
        double strgradetempto = 0;
        string strtotgrac = "";
        string strgradetempgrade = "";
        string syll_code = "";
        DataSet dggradetot = new DataSet();
        connection connection = new connection();
        SqlDataAdapter adaload;
        try
        {
            dggradetot.Dispose();
            DataSet daload = new DataSet();
            string strsqlstaffname = "select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "";
            cmd = new SqlCommand(strsqlstaffname);
            cmd.Connection = connection.CreateConnection();
            adaload = new SqlDataAdapter(cmd);
            adaload.Fill(dggradetot);
        }
        catch (SqlException qle)
        {
            throw qle;
        }
        finally
        {
            connection.Close();
        }

        string CheckingQuery = string.Empty;

        examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");//madhumathi 
        syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
        if (ccva == "False")
        {

            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and Exam_code = '" + examcodeval + "' ";//added by madhumathi

            CheckingQuery = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Fail' or result='fail' or result='AAA') and Exam_code = '" + examcodeval + "' ";//added by madhumathi
        }
        else if (ccva == "True")
        {
            strsubcrd = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') ";


            CheckingQuery = " Select Subject.credit_points,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Fail' or result='fail' or result='AAA')";
        }
        if (strsubcrd != "" && strsubcrd != null && CheckingQuery.Trim() != "")
        {
            bool ArrerCheckFlag = false;
            DataSet DtArrerCheck = d2.select_method_wo_parameter(CheckingQuery, "Text");
            if (DtArrerCheck.Tables.Count > 0 && DtArrerCheck.Tables[0].Rows.Count > 0)
            {
                ArrerCheckFlag = true;
            }
            string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");//Rajkumar on 28/5/2018
            if (val1.Trim() == "true" || val1.Trim() == "1")
                ArrerCheckFlag = false;
            if (!ArrerCheckFlag)
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
                                double strt1 = Math.Round(strtot, 0, MidpointRounding.AwayFromZero);
                                foreach (DataRow gratemp in dggradetot.Tables[0].Rows)
                                {
                                    if (gratemp["frange"].ToString() != null && gratemp["frange"].ToString() != "" && gratemp["trange"].ToString() != null && gratemp["trange"].ToString() != "")
                                    {
                                        strgradetempfrm = Convert.ToDouble(gratemp["frange"].ToString());
                                        strgradetempto = Convert.ToDouble(gratemp["trange"].ToString());
                                        if (strgradetempfrm <= strt1 && strgradetempto >= strt1)
                                        {
                                            strgrade = gratemp["credit_points"].ToString();
                                            break;
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
                        if (strgrade != "" && strgrade != null)
                        {
                            if (dr_subcrd["credit_points"].ToString() != null && dr_subcrd["credit_points"].ToString() != "")
                            {
                                creditval = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                if (creditsum1 == 0)
                                {
                                    creditsum1 = Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                }
                                else
                                {
                                    creditsum1 = creditsum1 + Convert.ToDouble(dr_subcrd["credit_points"].ToString());
                                }
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
        }
        if (creditsum1 != 0)
        {
            finalgpa1 = Math.Round((gpacal1 / creditsum1), 2, MidpointRounding.AwayFromZero);
        }
        return finalgpa1.ToString();
    }

    public string Calculete_CGPAPG(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode, bool transferflag = false)
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
            strattmaxmark = d2.GetFunctionv("select convert(varchar,attempts)+'-'+convert(varchar,maxmark) as amm from coe_attmaxmark where collegecode='" + collegecode + "'");
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
            dggradetot = d2.select_method(sqlcmdgraderstotal, hat, "Text");
            strsubcrd = " Select Subject.credit_points,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            if (!transferflag) //modified by prabha feb 10 2018
                strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";

            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS' ";
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
                                strtot = Math.Round(strtot, 0, MidpointRounding.AwayFromZero);
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
                                    strtot = Math.Round(strtot, 0, MidpointRounding.AwayFromZero);
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


    public int GetSemester_AsNumber(int IpValue)
    {
        try
        {
            InsFlag = false;
            string strinssetting = string.Empty;
            string VarProcessValue = string.Empty;
            int GetSemesterAsNumber = 0;
            //strinssetting="select * from inssettings where college_code="+ Session["collegecode"]+" and LinkName='Semester Display'";
            strinssetting = "select * from inssettings where LinkName='Semester Display'";
            con_Inssetting.Close();
            con_Inssetting.Open();
            SqlCommand cmd_ins = new SqlCommand(strinssetting, con_Inssetting);
            SqlDataReader dr_ins;
            dr_ins = cmd_ins.ExecuteReader();
            while (dr_ins.Read())
            {
                if (dr_ins.HasRows == true)
                {
                    if (dr_ins["LinkName"].ToString() == "Semester Display")
                    {
                        InsFlag = true;
                    }
                    if (Convert.ToInt32(dr_ins["LinkValue"]) == 0)
                    {
                        GetSemesterAsNumber = IpValue;
                    }
                    else if (Convert.ToInt32(dr_ins["LinkValue"]) == 1)
                    {
                        VarProcessValue = Convert.ToString(IpValue).Trim();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return IpValue;
    }

    protected void FpExternal_SelectedIndexChanged(Object sender, EventArgs e)
    {
        btnPrint.Visible = true;
    }

    protected void FpExternal_CellClick(Object sender, EventArgs e)
    {
        int isval;
        isval = Convert.ToInt32(FpExternal.Sheets[0].Cells[0, 0].Value.ToString());
    }

    protected void DropDownListpage_SelectedIndexChanged(object sender, EventArgs e)
    {
    }

    void CalculateTotalPages()
    {
        try
        {
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
            Buttontotal.Visible = true;
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void TextBoxother_TextChanged(object sender, EventArgs e)
    {
    }

    protected void TextBoxpage_TextChanged(object sender, EventArgs e)
    {
    }

    protected void btnLetterFormat_Click(object sender, EventArgs e)
    {
        try
        {
            panelchech.Visible = false;
            FpExternal.SaveChanges();
            Session["Branch"] = ddlBranch.SelectedItem.Text;
            Session["Degree"] = ddlDegree.SelectedItem.Text;
            Session["Branchcode"] = ddlBranch.SelectedValue;
            Session["Batch"] = ddlBatch.SelectedValue;
            Session["ExamMonth"] = ddlMonth.SelectedValue.ToString();
            Session["ExmMonth"] = ddlMonth.SelectedItem.Text.ToString();
            Session["ExamYear"] = ddlYear.SelectedItem.Text.ToString();
            Session["Semester"] = ddlSemYr.SelectedValue;
            Session["BranchCode"] = ddlBranch.SelectedValue.ToString();
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue;
            exam_year = ddlYear.SelectedItem.Text.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            // IntExamCode = Get_UnivExamCode(Convert.ToInt32(ddlBranch.SelectedValue.ToString()), Convert.ToInt16(Session["Semester"].ToString()), Convert.ToInt32(ddlBatch.SelectedValue));
            IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            Session["ExamCode"] = IntExamCode;
            string str_gradeflage = daccess.GetFunction("select grade_flag from grademaster where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue + " and exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedItem.Text.ToString() + "");
            Session["grade_flag"] = str_gradeflage;
            loadpdf();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void loadpdf()
    {
        try
        {
            DataTable dtem = new DataTable();
            dtem.Columns.Clear();
            dtem.Rows.Clear();
            DataColumn dc;
            dc = new DataColumn();
            dc.ColumnName = "Sno";
            dtem.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "sem";
            dtem.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "subjectname";
            dtem.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "result";
            dtem.Columns.Add(dc);
            dc = new DataColumn();
            dc.ColumnName = "grademark";
            dtem.Columns.Add(dc);
            DataRow dr;
            DataTable dtgrade = new DataTable();
            dtgrade.Columns.Clear();
            dtgrade.Rows.Clear();
            DataColumn dcgrade;
            dcgrade = new DataColumn();
            dcgrade.ColumnName = "Grade";
            dtgrade.Columns.Add(dcgrade);
            dcgrade = new DataColumn();
            dcgrade.ColumnName = "GradePoint";
            dtgrade.Columns.Add(dcgrade);
            DataRow drgrade;
            string mode = string.Empty;
            Font Fontbold = new Font("Book Antiqua", 11, FontStyle.Bold);
            Font Fontsmall = new Font("Book Antiqua", 9, FontStyle.Regular);
            Font Fontbold1 = new Font("Book Antiqua", 9, FontStyle.Bold);
            Font tamil = new Font("AMUDHAM.TTF", 16, FontStyle.Regular);
            Gios.Pdf.PdfDocument mydoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4_Horizontal);
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            batch_year = "" + ddlBatch.SelectedValue.ToString() + "";
            degree_code = "" + ddlBranch.SelectedValue.ToString() + "";
            panelchech.Visible = false;
            //sprdLetterFormat.Visible = true;
            int rowcount;
            rowcount = FpExternal.Sheets[0].RowCount;
            string Register = string.Empty;
            for (int i1 = 0; i1 < rowcount; i1++)
            {
                int st = 0;
                st = Convert.ToInt16(FpExternal.Sheets[0].Cells[i1, 1].Value);
                if (st == 1)
                {
                    dtem.Rows.Clear();
                    dtgrade.Rows.Clear();
                    string RegisterNumber = Convert.ToString(FpExternal.Sheets[0].Cells[i1, 2].Tag);
                    Register = Register + "-" + RegisterNumber;
                    gregisternumber = RegisterNumber;
                    panelchech.Visible = false;
                    panelchech.Visible = false;
                    FpMarkSheet.Visible = false;
                    ModalPopupExtender1.Hide();
                    panelchech.Visible = false;
                    panelchech.Visible = false;
                    sprdLetterFormat.Visible = true;
                    panelchech.Visible = false;
                    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                    {
                        con.Close();
                        con.Open();
                        string s;
                        s = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        s = s + "select r.mode,isnull(a.stud_name,'  ') as studentname,isnull(a.sex,' ') as Gender,isnull(a.parent_name,' ') as parentname,isnull(a.parent_addressC,' ') as parentaddress,isnull(a.StreetC,'') as street,isnull(a.Cityc,' ') as city,isnull(a.Districtc,' ') as district  from Registration r,applyn a  where a.app_no=r.App_No and  r.degree_code= '" + Session["Branchcode"] + "' and r.batch_year='" + Session["Batch"] + "' and r.roll_no='" + gregisternumber + "'";
                        SqlDataAdapter da = new SqlDataAdapter(s, con);
                        DataSet ds = new DataSet();
                        da.Fill(ds);
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                        {
                            mode = ds.Tables[1].Rows[0]["mode"].ToString();
                            collnamenew1 = ds.Tables[0].Rows[0]["collname"].ToString();
                            collnamenew1 = collnamenew1 + ',';
                            address = ds.Tables[0].Rows[0]["address1"].ToString() + " , " + ds.Tables[0].Rows[0]["address2"].ToString();
                            address3 = ds.Tables[0].Rows[0]["address3"].ToString() + "," + " " + "Pincode - " + ds.Tables[0].Rows[0]["pincode"].ToString();
                            gstudentname = ds.Tables[1].Rows[0]["studentname"].ToString();
                            gparentname = ds.Tables[1].Rows[0]["parentname"].ToString();
                            gparentaddress = ds.Tables[1].Rows[0]["parentaddress"].ToString();
                            gstreet = ds.Tables[1].Rows[0]["street"].ToString();
                            gcity = ds.Tables[1].Rows[0]["city"].ToString();
                            gdistrict = ds.Tables[1].Rows[0]["district"].ToString();
                            if (ds.Tables[1].Rows[0]["Gender"].ToString() != null || ds.Tables[1].Rows[0]["Gender"].ToString() != " ")
                            {
                                if (ds.Tables[1].Rows[0]["Gender"].ToString() == "1")
                                {
                                    ggender = "daughter";
                                }
                                else if (ds.Tables[1].Rows[0]["Gender"].ToString() == "0")
                                {
                                    ggender = "son";
                                }
                            }
                        }
                        string grade_setting = string.Empty;
                        con.Close();
                        con.Open();
                        SqlCommand cmd;
                        cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade' and  college_code='" + Session["collegecode"].ToString() + "'", con);
                        SqlDataReader dr_grade_val = cmd.ExecuteReader();
                        while (dr_grade_val.Read())
                        {
                            if (dr_grade_val.HasRows == true)
                            {
                                grade_setting = dr_grade_val[0].ToString();
                            }
                        }
                        int incr_grade_display = 0;
                        string grade = string.Empty;
                        string result1 = string.Empty;
                        con.Close();
                        con.Open();
                        string s1;
                        int sno = 0;
                        string checkgradeflag = string.Empty;
                        checkgradeflag = GetFunction("select grade_flag from grademaster where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year= " + exam_year + "");
                        s1 = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + gregisternumber + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                        SqlDataAdapter da1 = new SqlDataAdapter(s1, con);
                        DataSet ds1 = new DataSet();
                        da1.Fill(ds1);
                        if (ds1.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                            {
                                sno++;
                                result1 = ds1.Tables[0].Rows[i]["result"].ToString();
                                if (checkgradeflag == "2")
                                {
                                    if (ds1.Tables[0].Rows[i]["grade"] != "")
                                    {
                                        grade = ds1.Tables[0].Rows[i]["grade"].ToString();
                                        dr = dtem.NewRow();
                                        dr["Sno"] = sno.ToString();
                                        dr["sem"] = ds1.Tables[0].Rows[i]["semester"].ToString();
                                        dr["subjectname"] = ds1.Tables[0].Rows[i]["subject_name"].ToString();
                                        dr["result"] = result1;
                                        dr["grademark"] = grade;
                                        dtem.Rows.Add(dr);
                                    }
                                    else
                                    {
                                        grade = " ";
                                    }
                                }
                                else if (checkgradeflag == "3")
                                {
                                    if (grade_setting == "1")
                                    {
                                        if ((ds1.Tables[0].Rows[i]["internal_mark"].ToString() != "") && (ds1.Tables[0].Rows[i]["External_mark"].ToString() != "") && (ds1.Tables[0].Rows[i]["internal_mark"].ToString() != " ") && (ds1.Tables[0].Rows[i]["External_mark"].ToString() != " ")) //new condition 14.03.2012
                                        {
                                            if (Convert.ToDouble(ds1.Tables[0].Rows[i]["internal_mark"].ToString()) >= Convert.ToDouble(ds1.Tables[0].Rows[i]["min_int_marks"].ToString()) && Convert.ToDouble(ds1.Tables[0].Rows[i]["External_mark"].ToString()) >= Convert.ToDouble(ds1.Tables[0].Rows[i]["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(gregisternumber, ds1.Tables[0].Rows[i]["subject_no"].ToString());
                                                result1 = "Pass";
                                            }
                                            else
                                            {
                                                funcgrade = "RA";
                                                result1 = "Fail";
                                            }
                                        }
                                        grade = funcgrade.ToString();
                                    }
                                    else
                                    {
                                        grade = ds1.Tables[0].Rows[i]["grade"].ToString();
                                    }
                                    dr = dtem.NewRow();
                                    dr["Sno"] = sno.ToString();
                                    dr["sem"] = ds1.Tables[0].Rows[i]["semester"].ToString();
                                    dr["subjectname"] = ds1.Tables[0].Rows[i]["subject_name"].ToString();
                                    dr["result"] = result1;
                                    dr["grademark"] = grade;
                                    dtem.Rows.Add(dr);
                                }
                            }
                            string gpa = commonaccess.Calulat_GPA_Semwise(gregisternumber, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                            ggpa = gpa;
                            string syll_code = string.Empty;
                            syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
                            string checkresult = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + gregisternumber + "') and roll_no ='" + gregisternumber + "'  and subject.syll_code=" + syll_code.ToString() + " )";
                            DataSet dscheckresult = commonaccess.select_method(checkresult, hat, "Text");
                            if (dscheckresult.Tables[0].Rows.Count > 0)
                            {
                            }
                            else
                            {
                                string cgpa = commonaccess.Calculete_CGPA(gregisternumber, Session["Semester"].ToString(), degree_code, batch_year, mode, Session["collegecode"].ToString());
                                gcgpa = cgpa;
                            }
                            string strdisplaygrad = "select distinct mark_grade from grade_master where batch_year=" + Session["Batch"].ToString() + " and degree_code=" + Session["BranchCode"].ToString() + "";
                            con.Close();
                            con.Open();
                            SqlDataAdapter da_displaygrade = new SqlDataAdapter(strdisplaygrad, con);
                            DataSet ds_displaygrade = new DataSet();
                            da_displaygrade.Fill(ds_displaygrade);
                            int clas_adv_row = 0;
                            int cnt_noof_grades = 0;
                            int x = 0;
                            for (int sub_grade = 0; sub_grade < ds_displaygrade.Tables[0].Rows.Count; sub_grade++)
                            {
                                string gradename = ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString();
                                string gradepoint = GetFunction("select distinct credit_points from grade_master where mark_grade='" + ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString() + "' and  batch_year=" + Session["Batch"].ToString() + " and degree_code=" + Session["BranchCode"].ToString() + "");
                                drgrade = dtgrade.NewRow();
                                drgrade["Grade"] = gradename;
                                drgrade["GradePoint"] = gradepoint;
                                dtgrade.Rows.Add(drgrade);
                            }
                        }
                        Bindpdfn(mydoc, Fontsmall, Fontbold, Fontbold1, dtem, dtgrade, Response);
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

    public void Bindpdfn(Gios.Pdf.PdfDocument mydoc, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable dt, DataTable dtgrade, HttpResponse response)
    {
        try
        {
            // added by sridhar.....................Start
            degree_code = ddlBranch.SelectedValue.ToString();
            string hodcode = string.Empty;
            string principlecode = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            string srisql = "select s.StaffSign,s.staff_code from Department d,StaffPhoto s where d.Dept_Code='" + degree_code + "' and d.college_code='" + Session["collegecode"] + "' and d.Head_Of_Dept=s.staff_code ";
            srids.Clear();
            srids = daccess.select_method_wo_parameter(srisql, "Text");
            if (srids.Tables[0].Rows.Count > 0)
            {
                hodcode = srids.Tables[0].Rows[0]["staff_code"].ToString();
                hodcode = hodcode + degree_code;
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg")))
                {
                    byte[] file = (byte[])srids.Tables[0].Rows[0]["StaffSign"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
            }
            srisql = "select principal_sign,acr,college_code from collinfo where college_code='" + Session["collegecode"] + "' and principal_sign is not null";
            srids.Clear();
            srids = daccess.select_method_wo_parameter(srisql, "Text");
            if (srids.Tables[0].Rows.Count > 0)
            {
                principlecode = srids.Tables[0].Rows[0]["acr"].ToString() + srids.Tables[0].Rows[0]["college_code"].ToString();
                principlecode = principlecode + degree_code;
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + principlecode + ".jpeg")))
                {
                    byte[] file = (byte[])srids.Tables[0].Rows[0]["principal_sign"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + principlecode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
            }
            // added by sridhar.....................end  
            int cnt;
            int sno;
            int gcount = dtgrade.Rows.Count;
            double bindgrade = gcount / 2;
            double remainingcount = gcount % 2;
            int balancecount = 0;
            if (remainingcount > 0)
            {
                balancecount = 1;
            }
            int gradecount = (int)Math.Round(bindgrade) + balancecount;
            sno = dt.Rows.Count;
            int subno = 0;
            int pagecount = sno / 20;
            int repage = sno % 20;
            int nopages = pagecount;
            if (repage > 0)
            {
                nopages++;
            }
            if (nopages > 0)
            {
                for (int row = 0; row < nopages; row++)
                {
                    subno++;
                    Gios.Pdf.PdfPage mypdfpage = mydoc.NewPage();
                    int y = 40;
                    PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 80, y, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, collnamenew1);
                    PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 80, y + 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address);
                    PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                      new PdfArea(mydoc, 80, y + 50, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, address3);
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 50, 20, 370);
                    }
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                    {
                        PdfImage LogoImage = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                        mypdfpage.Add(LogoImage, 430, 20, 370);
                    }
                    PdfArea tete = new PdfArea(mydoc, 700, 50, 60, 60);
                    PdfRectangle pr1 = new PdfRectangle(mydoc, tete, Color.Black);
                    mypdfpage.Add(pr1);
                    //PdfArea teteto = new PdfArea(mydoc, 530, 120, 280, 150);
                    PdfArea teteto = new PdfArea(mydoc, 550, 120, 260, 150);
                    PdfRectangle pr2 = new PdfRectangle(mydoc, teteto, Color.Black);
                    mypdfpage.Add(pr2);
                    //PdfArea tetefrom = new PdfArea(mydoc, 600, 300, 280, 150);
                    // PdfArea tetefrom = new PdfArea(mydoc, 550, 350, 260, 150);
                    PdfArea tetefrom = new PdfArea(mydoc, 550, 350, 270, 150);
                    PdfRectangle pr3 = new PdfRectangle(mydoc, tetefrom, Color.Black);
                    mypdfpage.Add(pr3);
                    PdfTextArea pt123S = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 710, 50, 60, 30), System.Drawing.ContentAlignment.MiddleLeft, "STAMP");
                    PdfTextArea pt123to = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(mydoc, 560, 130, 100, 30), System.Drawing.ContentAlignment.MiddleLeft, "To");
                    PdfTextArea pt123to1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 580, 150, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, gparentname);
                    PdfTextArea pt123to2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 580, 170, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, gparentaddress);
                    PdfTextArea pt123to3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 580, 190, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, gstreet);
                    PdfTextArea pt123to4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                       new PdfArea(mydoc, 580, 210, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, gcity);
                    PdfTextArea pt123from = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 540, 310, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sender's Name and Address");
                    PdfTextArea pt123sender = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 560, 360, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, "From");
                    PdfTextArea pt123from1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 570, 380, 300, 30), System.Drawing.ContentAlignment.MiddleLeft, "THE PRINCIPAL");
                    PdfTextArea pt123from111 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                          new PdfArea(mydoc, 570, 400, 250, 40), System.Drawing.ContentAlignment.MiddleLeft, collnamenew1);
                    PdfTextArea pt123from2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(mydoc, 570, 430, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, address);
                    PdfTextArea pt123from3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(mydoc, 570, 450, 250, 30), System.Drawing.ContentAlignment.MiddleLeft, address3);
                    mypdfpage.Add(pt123S);
                    mypdfpage.Add(pt123sender);
                    mypdfpage.Add(ptc);
                    mypdfpage.Add(pts);
                    mypdfpage.Add(ptss);
                    mypdfpage.Add(pt123to);
                    mypdfpage.Add(pt123to1);
                    mypdfpage.Add(pt123to2);
                    mypdfpage.Add(pt123to3);
                    mypdfpage.Add(pt123to4);
                    mypdfpage.Add(pt123from);
                    mypdfpage.Add(pt123from1);
                    mypdfpage.Add(pt123from2);
                    mypdfpage.Add(pt123from3);
                    mypdfpage.Add(pt123from111);
                    cnt = subno * sno;
                    int cnt1 = subno * 20;
                    Gios.Pdf.PdfTable table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 1);
                    table.VisibleHeaders = false;
                    if (subno == 1)
                    {
                        table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 1);
                        table.VisibleHeaders = false;
                        table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                        PdfTextArea pt123 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                           new PdfArea(mydoc, 40, 110, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Parents");
                        PdfTextArea ptc21 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 50, 130, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "Sub :" + " " + ddlMonth.SelectedItem.Text + "  /" + ddlYear.SelectedItem.Text + "  Exam Performance Report,");
                        PdfTextArea ptc22 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, 150, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, " Your " + ggender + "  " + gstudentname + "  [" + gregisternumber + " ]");
                        PdfTextArea ptc22stu = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                               new PdfArea(mydoc, 430, 150, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, "studying in");
                        mypdfpage.Add(ptc22stu);
                        if (ggender == "son")
                        {
                            PdfTextArea ptc2line = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 83, 155, 550, 20), System.Drawing.ContentAlignment.MiddleLeft, "_________________________________________________");
                            mypdfpage.Add(ptc2line);
                        }
                        else if (ggender == "daughter")
                        {
                            PdfTextArea ptc2line = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                       new PdfArea(mydoc, 103, 155, 550, 20), System.Drawing.ContentAlignment.MiddleLeft, "_________________________________________________");
                            mypdfpage.Add(ptc2line);
                        }
                        PdfTextArea ptc222 = new PdfTextArea(Fontsmall, System.Drawing.Color.Black,
                                                                        new PdfArea(mydoc, 40, 170, 550, 30), System.Drawing.ContentAlignment.MiddleLeft, ddlDegree.SelectedItem.Text + " [" + ddlBranch.SelectedItem.Text + "] has secured the following marks in");
                        mypdfpage.Add(pt123);
                        mypdfpage.Add(ptc21);
                        mypdfpage.Add(ptc22);
                        mypdfpage.Add(ptc222);
                    }
                    int val = 0;
                    if (subno == 1)
                    {
                        if (cnt < 20)
                        {
                            table = mydoc.NewTable(Fontsmall, cnt + 1, 5, 1);
                            //table = mydoc.NewTable(Fontsmall,
                            table.VisibleHeaders = false;
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(480);
                            table.Columns[3].SetWidth(90);
                            table.Columns[4].SetWidth(100);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.no");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sem");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Name");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Result");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Grade/Mark");
                            for (int i = 0; i < cnt; i++)
                            {
                                val++;
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 0).SetContent(val);
                                string scode = dt.Rows[i]["sem"].ToString();
                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 1).SetContent(scode);
                                string sname = dt.Rows[i]["subjectname"].ToString();
                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(val, 2).SetContent(sname);
                                string markobtained = dt.Rows[i]["result"].ToString();
                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 3).SetContent(markobtained);
                                string result = dt.Rows[i]["grademark"].ToString();
                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 4).SetContent(result);
                            }
                            int xcout = 435;
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 200, 480, 500));
                            mypdfpage.Add(newpdftabpage);
                            PdfTextArea pt219 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 40, xcout + 20, 400, 40), System.Drawing.ContentAlignment.MiddleCenter, "GPA:" + "  " + ggpa.ToString());
                            mypdfpage.Add(pt219);
                            PdfTextArea pt2199 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 40, xcout + 20, 400, 40), System.Drawing.ContentAlignment.MiddleRight, "CGPA:" + "  " + gcgpa.ToString());
                            mypdfpage.Add(pt2199);
                            Gios.Pdf.PdfTable tablegrade = mydoc.NewTable(Fontsmall, gradecount + 1, 4, 1);
                            tablegrade.VisibleHeaders = false;
                            tablegrade.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablegrade.Columns[0].SetWidth(50);
                            tablegrade.Columns[1].SetWidth(50);
                            tablegrade.Columns[2].SetWidth(50);
                            tablegrade.Columns[3].SetWidth(50);
                            tablegrade.CellRange(0, 0, 0, 3).SetFont(Fontsmall);
                            tablegrade.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 0).SetContent("Grade");
                            tablegrade.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 1).SetContent("GradePoint");
                            tablegrade.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 2).SetContent("Grade");
                            tablegrade.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 3).SetContent("GradePoint");
                            int growcount = 0;
                            int gcolcount = 0;
                            for (int i = 0; i < gcount; i++)
                            {
                                if (growcount < gradecount)
                                {
                                    growcount++;
                                    string scode = dtgrade.Rows[i]["grade"].ToString();
                                    tablegrade.Cell(growcount, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(growcount, 0).SetContent(scode);
                                    string sname = dtgrade.Rows[i]["gradepoint"].ToString();
                                    tablegrade.Cell(growcount, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(growcount, 1).SetContent(sname);
                                }
                                else
                                {
                                    gcolcount++;
                                    string markobtained = dtgrade.Rows[i]["grade"].ToString();
                                    tablegrade.Cell(gcolcount, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(gcolcount, 2).SetContent(markobtained);
                                    string result = dtgrade.Rows[i]["gradepoint"].ToString();
                                    tablegrade.Cell(gcolcount, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(gcolcount, 3).SetContent(result);
                                }
                            }
                            xcout = xcout + 60;
                            Gios.Pdf.PdfTablePage newpdftabpage1 = tablegrade.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, xcout, 400, 400));
                            mypdfpage.Add(newpdftabpage1);
                            tablegrade.VisibleHeaders = false;
                            table.VisibleHeaders = false;
                        }
                        else
                        {
                            table = mydoc.NewTable(Fontsmall, cnt1 + 1, 5, 1);
                            table.VisibleHeaders = false;
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(480);
                            table.Columns[3].SetWidth(90);
                            table.Columns[4].SetWidth(100);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.no");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sem");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Name");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Result");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Grade/Mark");
                            for (int i = 0; i < cnt1; i++)
                            {
                                val++;
                                table.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 0).SetContent(val);
                                string scode = dt.Rows[i]["sem"].ToString();
                                table.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 1).SetContent(scode);
                                string sname = dt.Rows[i]["subjectname"].ToString();
                                table.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(val, 2).SetContent(sname);
                                string markobtained = dt.Rows[i]["result"].ToString();
                                table.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 3).SetContent(markobtained);
                                string result = dt.Rows[i]["grademark"].ToString();
                                table.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(val, 4).SetContent(result);
                            }
                            int xcout = (val * 15) + 240;
                            Gios.Pdf.PdfTablePage newpdftabpage = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 200, 480, 500));
                            mypdfpage.Add(newpdftabpage);
                        }
                    }
                    if (subno > 1)
                    {
                        val = (subno - 1) * 20;
                        int ro = 0;
                        int remaindsubs = sno - val;
                        int yaxis = 180;
                        if (remaindsubs < 20)
                        {
                            table = mydoc.NewTable(Fontsmall, remaindsubs + 1, 5, 1);
                            table.VisibleHeaders = false;
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(480);
                            table.Columns[3].SetWidth(90);
                            table.Columns[4].SetWidth(100);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.no");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sem");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Name");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Result");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Grade/Mark");
                            for (int fg = 0; fg < remaindsubs; fg++)
                            {
                                yaxis += 10;
                                ro++;
                                table.Cell(ro, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 0).SetContent(val + 1);
                                string scode = dt.Rows[val]["sem"].ToString();
                                table.Cell(ro, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 1).SetContent(scode);
                                string sname = dt.Rows[val]["subjectname"].ToString();
                                table.Cell(ro, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(ro, 2).SetContent(sname);
                                string markobtained = dt.Rows[val]["result"].ToString();
                                table.Cell(ro, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 3).SetContent(markobtained);
                                string result = dt.Rows[val]["grademark"].ToString();
                                table.Cell(ro, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 4).SetContent(result);
                                val++;
                            }
                            int xcout = 435;
                            PdfTextArea pt219 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                          new PdfArea(mydoc, 40, xcout + 20, 400, 40), System.Drawing.ContentAlignment.MiddleCenter, "GPA:" + "  " + ggpa.ToString());
                            mypdfpage.Add(pt219);
                            PdfTextArea pt2199 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                         new PdfArea(mydoc, 40, xcout + 20, 400, 40), System.Drawing.ContentAlignment.MiddleRight, "CGPA:" + "  " + gcgpa.ToString());
                            mypdfpage.Add(pt2199);
                            Gios.Pdf.PdfTable tablegrade = mydoc.NewTable(Fontsmall, gradecount + 1, 4, 1);
                            tablegrade.VisibleHeaders = false;
                            tablegrade.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            tablegrade.Columns[0].SetWidth(50);
                            tablegrade.Columns[1].SetWidth(50);
                            tablegrade.Columns[2].SetWidth(50);
                            tablegrade.Columns[3].SetWidth(50);
                            tablegrade.CellRange(0, 0, 0, 3).SetFont(Fontsmall);
                            tablegrade.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 0).SetContent("Grade");
                            tablegrade.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 1).SetContent("GradePoint");
                            tablegrade.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 2).SetContent("Grade");
                            tablegrade.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            tablegrade.Cell(0, 3).SetContent("GradePoint");
                            int growcount = 0;
                            int gcolcount = 0;
                            for (int i = 0; i < gcount; i++)
                            {
                                if (growcount < gradecount)
                                {
                                    growcount++;
                                    string scode = dtgrade.Rows[i]["grade"].ToString();
                                    tablegrade.Cell(growcount, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(growcount, 0).SetContent(scode);
                                    string sname = dtgrade.Rows[i]["gradepoint"].ToString();
                                    tablegrade.Cell(growcount, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(growcount, 1).SetContent(sname);
                                }
                                else
                                {
                                    gcolcount++;
                                    string markobtained = dtgrade.Rows[i]["grade"].ToString();
                                    tablegrade.Cell(gcolcount, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(gcolcount, 2).SetContent(markobtained);
                                    string result = dtgrade.Rows[i]["gradepoint"].ToString();
                                    tablegrade.Cell(gcolcount, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    tablegrade.Cell(gcolcount, 3).SetContent(result);
                                }
                            }
                            xcout = xcout + 60;
                            Gios.Pdf.PdfTablePage newpdftabpage1 = tablegrade.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, xcout, 400, 400));
                            mypdfpage.Add(newpdftabpage1);
                        }
                        else
                        {
                            table = mydoc.NewTable(Fontsmall, cnt1 + 1, 5, 1);
                            table.VisibleHeaders = false;
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(480);
                            table.Columns[3].SetWidth(90);
                            table.Columns[4].SetWidth(100);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.no");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sem");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Name");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Result");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Grade/Mark");
                            table = mydoc.NewTable(Fontsmall, 5 + 1, 5, 1);
                            table.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                            table.Columns[0].SetWidth(50);
                            table.Columns[1].SetWidth(100);
                            table.Columns[2].SetWidth(480);
                            table.Columns[3].SetWidth(90);
                            table.Columns[4].SetWidth(100);
                            table.CellRange(0, 0, 0, 4).SetFont(Fontsmall);
                            table.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 0).SetContent("S.no");
                            table.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 1).SetContent("Sem");
                            table.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 2).SetContent("Subject Name");
                            table.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 3).SetContent("Result");
                            table.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                            table.Cell(0, 4).SetContent("Grade/Mark");
                            for (int fg = 0; fg < 11; fg++)
                            {
                                ro++;
                                table.Cell(ro, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 0).SetContent(val);
                                string scode = dt.Rows[val]["sem"].ToString();
                                table.Cell(ro, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 1).SetContent(scode);
                                string sname = dt.Rows[val]["subjectname"].ToString();
                                table.Cell(ro, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table.Cell(ro, 2).SetContent(sname);
                                string markobtained = dt.Rows[val]["result"].ToString();
                                table.Cell(ro, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 3).SetContent(markobtained);
                                string result = dt.Rows[val]["grademark"].ToString();
                                table.Cell(ro, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table.Cell(ro, 4).SetContent(result);
                                val++;
                            }
                        }
                    }
                    //if (subno == 1)
                    //{
                    //    Gios.Pdf.PdfTablePage newpdftabpagessss = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 230, 480, 500));
                    //    mypdfpage.Add(newpdftabpagessss);
                    //}
                    if (subno > 1)
                    {
                        Gios.Pdf.PdfTablePage newpdftabpagess = table.CreateTablePage(new Gios.Pdf.PdfArea(mydoc, 40, 170, 480, 500));
                        mypdfpage.Add(newpdftabpagess);
                    }
                    PdfTextArea ptclassadv = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                  new PdfArea(mydoc, 40, 580, 400, 20), System.Drawing.ContentAlignment.MiddleLeft, "CLASS ADVISOR");
                    mypdfpage.Add(ptclassadv);
                    PdfTextArea pthod = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                 new PdfArea(mydoc, 40, 580, 400, 20), System.Drawing.ContentAlignment.MiddleCenter, "HOD");
                    mypdfpage.Add(pthod);
                    // added by sridhar.....................Start
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg")))
                    {
                        PdfImage hodsign = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg"));
                        mypdfpage.Add(hodsign, 220, 540, 800);
                    }
                    // added by sridhar.....................end
                    // added by sridhar.....................Start
                    if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + principlecode + ".jpeg")))
                    {
                        PdfImage prinsign = mydoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + principlecode + ".jpeg"));
                        mypdfpage.Add(prinsign, 380, 540, 800);
                    }
                    // added by sridhar.....................end
                    PdfTextArea ptprincipal = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                                new PdfArea(mydoc, 40, 580, 400, 20), System.Drawing.ContentAlignment.MiddleRight, "PRINCIPAL");
                    mypdfpage.Add(ptprincipal);
                    table.VisibleHeaders = false;
                    string appPath = HttpContext.Current.Server.MapPath("~");
                    if (appPath != "")
                    {
                        string szPath = appPath + "/Report/";
                        string szFile = "Format1.pdf";
                        mypdfpage.SaveToDocument();
                        mydoc.SaveToFile(szPath + szFile);
                        Response.ClearHeaders();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                        Response.ContentType = "application/pdf";
                        Response.WriteFile(szPath + szFile);
                    }
                }
                string sFilePath = Server.MapPath("~/college/" + hodcode + ".jpg");
                FileInfo fi = new FileInfo(sFilePath);
                fi.Delete();
                sFilePath = Server.MapPath("~/college/" + principlecode + ".jpg");
                fi = new FileInfo(sFilePath);
                fi.Delete();
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void convertgrade(string roll, string subj)
    {
        try
        {
            strexam = "Select subject_name,subject_code,total,result,cp,mark_entry.subject_no from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + "  and roll_no='" + roll + "' and subject.subject_no=" + subj + "";
            SqlCommand cmd_exam1 = new SqlCommand(strexam, con_convertgrade);
            con_convertgrade.Close();
            con_convertgrade.Open();
            dr_convert = cmd_exam1.ExecuteReader();
            while (dr_convert.Read())
            {
                //   funcsemester = dr_convert["semester"].ToString();
                funcsubname = dr_convert["subject_name"].ToString();
                funcsubno = dr_convert["subject_no"].ToString();
                funcsubcode = dr_convert["subject_code"].ToString();
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
                SqlCommand cmd_grade = new SqlCommand(strgrade, con_Grade);
                con_Grade.Close();
                con_Grade.Open();
                SqlDataReader dr_grade;
                dr_grade = cmd_grade.ExecuteReader();
                if (dr_grade.HasRows == true)
                {
                    while (dr_grade.Read())
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
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void MarkSheet()
    {
        try
        {
            FpExternal.SaveChanges();//Added By Srinath 5/4/2013
            FpExternal.Visible = true;
            btnxl.Visible = true;//added by srinath 24/5/2014
            lblxl.Visible = true;
            txtxlname.Visible = true;
            lblnorec.Visible = false;
            FpMarkSheet.Visible = true;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            FpMarkSheet.Sheets[0].RowHeader.Visible = false;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Bold = true;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
            FpMarkSheet.Sheets[0].RowCount = 0;
            FpMarkSheet.Sheets[0].PageSize = 40;
            string maxsem = string.Empty;
            int i = 0;
            int semdec = 0;
            //int k = 0;
            int j = 0;
            int no = 0;
            int nos = 0;
            string Subno = string.Empty;
            string RegNo = string.Empty;
            string RollNo = string.Empty;
            int Varsel;
            string credit = string.Empty;
            string result = string.Empty;
            string name = string.Empty;
            string course = string.Empty;
            string dob = string.Empty;
            string dobtemp = string.Empty;
            string mont = string.Empty;
            //string mon =string.Empty;
            string yr = string.Empty;
            string gender = string.Empty;
            string grade = string.Empty;
            //Dim courseRs As New ADODB.Recordset
            //Dim RsStudent As New ADODB.Recordset
            //Dim rsgrademas As New ADODB.Recordset
            //  int grcredit1 = 0;
            double grcredit1 = 0;
            string grcredit = string.Empty;
            //  int gpacal = 0;
            double gpacal = 0;
            //  int gpa = 0;
            double gpa = 0;
            int pval = 0;
            bool flag = false;
            // int gpa1 = 0;
            double gpa1 = 0;
            string sem = string.Empty;
            string regulation = string.Empty;
            string department = string.Empty;
            string Subname = string.Empty;
            string SubCode = string.Empty;
            int TotalPages = 0;
            //int g = 0;
            double g = 0;
            bool once = false;
            string sem1 = string.Empty;
            string sem2 = string.Empty;
            string sem3 = string.Empty;
            // int l = 0;
            bool optionflag = false;
            string oldsem = string.Empty;
            string getmnth = string.Empty;
            string getyear = string.Empty;
            string grademas = string.Empty;
            //  string grpoints =string.Empty;
            double grpoints = 0;
            string concat = string.Empty;
            string dept_name = string.Empty;
            string strExam_month = string.Empty;
            int subjcnt = 0;
            string strcourse = string.Empty;
            int chkstud_selected_cnt = 0;
            FpMarkSheet.Sheets[0].ColumnCount = 9;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            //---------------------------------------get the examcode
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            if (current_sem != "")
            {
                semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
            }
            if ((exam_month != "") && (exam_year != "") && (exam_month != "0") && (exam_year != "0"))
            {
                //  IntExamCode = Convert.ToInt16(GetFunction("select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month.ToString() + " and exam_year= " + exam_year.ToString() + ""));
                IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + exam_month.ToString() + "' and exam_year= '" + exam_year.ToString() + "'"));
                if (IntExamCode != 0)
                {
                    lblnorec.Visible = false;
                    FpExternal.Visible = true;
                    btnxl.Visible = true;//added by srinath 24/5/2014
                    lblxl.Visible = true;
                    txtxlname.Visible = true;
                    int sel;
                    //'-------loop for get chkbox val
                    FpExternal.SaveChanges();
                    string chkrollno = string.Empty;
                    string temprollno = string.Empty;
                    string chkRegNo = string.Empty;
                    string chkname = string.Empty;
                    string tempRegNo = string.Empty;
                    string tempname = string.Empty;
                    for (j = 0; j <= FpExternal.Sheets[0].RowCount - 1; j++)                        //Modified By Srinath 5/4/2013
                    {
                        // isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(flagrow, 0).ToString());
                        sel = 0;// Convert.ToInt32(FpExternal.Sheets[0].Cells[j, 1].Value.ToString());
                        int.TryParse(Convert.ToString(FpExternal.Sheets[0].Cells[j, 1].Value).Trim(), out sel);
                        if (sel == 1)
                        {
                            chkstud_selected_cnt += 1;
                            pval += 1;
                            chkRegNo = FpExternal.Sheets[0].Cells[j, 3].Text;
                            chkname = FpExternal.Sheets[0].Cells[j, 4].Text;
                            chkrollno = FpExternal.Sheets[0].Cells[j, 2].Tag.ToString();
                            //string GPANEW = FpExternal.Sheets[0].Cells[j, FpExternal.Sheets[0].ColumnCount-3].Text;
                            //string CGPANEW = FpExternal.Sheets[0].Cells[j, FpExternal.Sheets[0].ColumnCount - ].Text;
                            if (temprollno == "")
                            {
                                temprollno = chkrollno;
                            }
                            else
                            {
                                temprollno = temprollno + "," + chkrollno;
                            }
                            if (tempRegNo == "")
                            {
                                tempRegNo = chkRegNo;
                            }
                            else
                            {
                                tempRegNo = tempRegNo + "," + chkRegNo;
                            }
                            if (tempname == "")
                            {
                                tempname = chkname;
                            }
                            else
                            {
                                tempname = tempname + "," + chkname;
                            }
                        }
                    }
                    int chkstudent_count = 0;
                    string[] split_temprollno = temprollno.Split(',');
                    string[] split_tempRegNo = tempRegNo.Split(',');
                    string[] split_tempname = tempname.Split(',');
                    if (chkstud_selected_cnt > 0)
                    {
                        for (i = 0; i <= split_temprollno.GetUpperBound(0); i++)
                        {
                            lblstudselect.Visible = false;
                            btnPrint.Visible = true;
                            lblError.Visible = false;
                            chkstudent_count += 1;
                            lblnorec.Visible = false;
                            subjcnt = 0;
                            no = 0;
                            string EarnedCredit = string.Empty;
                            grcredit1 = 0;
                            string[] studarr = new string[pval];
                            //Varsel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(i, 1).ToString());
                            Varsel = 0;
                            int.TryParse(Convert.ToString(FpExternal.Sheets[0].Cells[i, 1].Value).Trim(), out Varsel);
                            if (Varsel == 1)
                            {
                                TotalPages += 1;
                            }
                            RollNo = split_temprollno[i].ToString();
                            RegNo = split_tempRegNo[i].ToString();
                            name = split_tempname[i].ToString();
                            //'----------------query for get the branch,degreecode,batchyear etc
                            string strgetdetail = string.Empty;
                            strgetdetail = "select branch_code,registration.current_semester ,registration.degree_code,sex,registration.batch_year,dob from registration,applyn where applyn.app_no=registration.App_no and Roll_no='" + RollNo + "'";
                            SqlCommand cmd_getdetail = new SqlCommand(strgetdetail, con_getdetail);
                            con_getdetail.Close();
                            con_getdetail.Open();
                            SqlDataReader dr_getdetail;
                            dr_getdetail = cmd_getdetail.ExecuteReader();
                            dr_getdetail.Read();
                            if (dr_getdetail.HasRows)
                            {
                                sem1 = dr_getdetail["current_semester"].ToString();
                                if (dr_getdetail["dob"].ToString() != "")
                                {
                                    dobtemp = dr_getdetail["dob"].ToString();
                                    string[] split_dobtemp = dobtemp.Split(new char[] { ' ' });
                                    string[] split_dob = split_dobtemp[0].Split(new char[] { '/' });
                                    string getday = split_dob[1].ToString();
                                    getmnth = split_dob[0].ToString();
                                    getyear = split_dob[2].ToString();
                                    concat = getday + '/' + getmnth + '/' + getyear;
                                }
                                else
                                {
                                    concat = string.Empty;
                                }
                                if (dr_getdetail["sex"].ToString() == "1")
                                {
                                    gender = "Female";
                                }
                                else
                                {
                                    gender = "Male";
                                }
                                if (dr_getdetail["batch_year"].ToString() != null)
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                                else
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                            }//'-----end hasrows
                            //'---------------------------query for get the exam_mopnth,exam_year
                            string strdaters = string.Empty;
                            strdaters = "select exam_month,Exam_year,current_semester from exam_details where Exam_Code=" + IntExamCode + "";
                            ExamCode = IntExamCode;
                            SqlCommand cmd_daters = new SqlCommand(strdaters, con_daters);
                            con_daters.Close();
                            con_daters.Open();
                            SqlDataReader dr_daters;
                            dr_daters = cmd_daters.ExecuteReader();
                            dr_daters.Read();
                            if (dr_daters.HasRows)
                            {
                                mont = dr_daters["exam_month"].ToString();
                                yr = dr_daters["Exam_year"].ToString();
                                oldsem = dr_daters["current_semester"].ToString();
                                sem = dr_daters["current_semester"].ToString();
                                if (sem == "1")
                                    sem3 = "I";
                                else if (sem == "2")
                                    sem3 = "II";
                                else if (sem == "3")
                                    sem3 = "III";
                                else if (sem == "4")
                                    sem3 = "IV";
                                else if (sem == "5")
                                    sem3 = "V";
                                else if (sem == "6")
                                    sem3 = "VI";
                                else if (sem == "7")
                                    sem3 = "VII";
                                else if (sem == "8")
                                    sem3 = "VIII";
                                else if (sem == "9")
                                    sem3 = "IX";
                                else if (sem == "10")
                                    sem3 = "X";
                                //'-------------------
                            }//'- end dr_daters hasrows
                            //'-------------------------------------display the month as string-----------
                            if (exam_month == "1")
                                strExam_month = "Jan";
                            else if (exam_month == "2")
                                strExam_month = "Feb";
                            else if (exam_month == "3")
                                strExam_month = "Mar";
                            else if (exam_month == "4")
                                strExam_month = "Apr";
                            else if (exam_month == "5")
                                strExam_month = "May";
                            else if (exam_month == "6")
                                strExam_month = "Jun";
                            else if (exam_month == "7")
                                strExam_month = "Jul";
                            else if (exam_month == "8")
                                strExam_month = "Aug";
                            else if (exam_month == "9")
                                strExam_month = "Sep";
                            else if (exam_month == "10")
                                strExam_month = "Oct";
                            else if (exam_month == "11")
                                strExam_month = "Nov";
                            else if (exam_month == "12")
                                strExam_month = "DEc";
                            //'---------------------------------------query for get the course name dept name for the strudent
                            // if (strcourse != "")
                            //{
                            strcourse = "select course_name,dept_name from course,department,degree where course.course_id=degree.course_id and degree.dept_code=department.dept_code and degree_code='" + dr_getdetail["degree_code"] + "'";
                            SqlCommand cmd_course = new SqlCommand(strcourse, con_course);
                            con_course.Close();
                            con_course.Open();
                            SqlDataReader dr_course;
                            dr_course = cmd_course.ExecuteReader();
                            dr_course.Read();
                            if (dr_course.HasRows)
                            {
                                if (txtGetDegree.Text == "")
                                {
                                    course = dr_course["course_name"].ToString();
                                }
                                else
                                {
                                    course = txtGetDegree.Text.Trim();
                                }
                                if (txtDepartment.Text == "")
                                {
                                    dept_name = dr_course["dept_name"].ToString();
                                }
                                else
                                {
                                    dept_name = txtDepartment.Text.Trim();
                                }
                                if (Chkbxcou.Checked == true)
                                {
                                    cou = 1;
                                }
                                else
                                {
                                    cou = 0;
                                }
                            }//'------end if dr_course
                            //  }
                            //NextPage: 
                            //RegNo = FpExternal.Sheets[0].Cells[i, 2].Text;
                            //name = FpExternal.Sheets[0].Cells[i, 3].Text;
                            //'----------------------------------------------incremen the row count
                            FpMarkSheet.Sheets[0].RowCount += 40;
                            //'------------------------------------load the clg information
                            string collnamenew1 = string.Empty;
                            string address1 = string.Empty;
                            string address3 = string.Empty;
                            string address = string.Empty;
                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                            {
                                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                SqlCommand collegecmd = new SqlCommand(college, con);
                                SqlDataReader collegename;
                                con.Close();
                                con.Open();
                                collegename = collegecmd.ExecuteReader();
                                if (collegename.HasRows)
                                {
                                    while (collegename.Read())
                                    {
                                        collnamenew1 = collegename["collname"].ToString();
                                        address1 = collegename["address1"].ToString();
                                        address3 = collegename["address3"].ToString();
                                        address = address1 + "," + address3;
                                    }
                                }
                            }
                            //'---------------------------------------------load theclg logo photo-------------------------------------
                            MyImg mi3 = new MyImg();
                            mi3.ImageUrl = "Handler/Handler2.ashx?";

                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi3;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                            //'---------------------------------------------load the student photo-------------------------------------
                            //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                            MyImg mi1 = new MyImg();
                            mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi1;
                            //'----------------------------------------------------------------
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            if (txtCOE.Text != "")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                            //'--------------------------------------------set the heading for the columns--------
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                            //   FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade/Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColor = Color.Black;
                            //'--------------------------------------------------------------------------------------
                            int p;
                            string getmark = string.Empty;
                            string getsem = string.Empty;
                            string getsubno = string.Empty;
                            string getsubname = string.Empty;
                            string getsubcode = string.Empty;
                            string getresult = string.Empty;
                            string Enrolledcredit = string.Empty;
                            int count = FpMarkSheet.Sheets[0].RowCount - 29;
                            //'-----------------------------------query for select the subject name and details
                            strexam = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "' order by semester desc,subject_type desc,subject.subject_no asc";
                            SqlCommand cmd_exam = new SqlCommand(strexam, con_exam);
                            con_exam.Close();
                            con_exam.Open();
                            dr_exam = cmd_exam.ExecuteReader();
                            if (dr_exam.HasRows)
                            {
                                nos += 1;
                                p = 0;
                                int sub_val = 1;
                                while (dr_exam.Read())
                                {
                                    if (subjcnt > (10 * sub_val))
                                    {
                                        sub_val = sub_val + 1;
                                        FpMarkSheet.Sheets[0].Rows[count + subjcnt + 1].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 2, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 2].Text = "- - -Continued- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        string coe = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                                        MyImg coeimg = new MyImg();
                                        coeimg.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //'----------------------------------------------incremen the row count
                                        FpMarkSheet.Sheets[0].RowCount += 40;
                                        //'------------------------------------load the clg information
                                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                        {
                                            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                            SqlCommand collegecmd = new SqlCommand(college, con);
                                            SqlDataReader collegename;
                                            con.Close();
                                            con.Open();
                                            collegename = collegecmd.ExecuteReader();
                                            if (collegename.HasRows)
                                            {
                                                while (collegename.Read())
                                                {
                                                    collnamenew1 = collegename["collname"].ToString();
                                                    address1 = collegename["address1"].ToString();
                                                    address3 = collegename["address3"].ToString();
                                                    address = address1 + "," + address3;
                                                }
                                            }
                                        }
                                        //'---------------------------------------------load theclg logo photo-------------------------------------
                                        //'----------------------
                                        MyImg mi4 = new MyImg();
                                        mi4.ImageUrl = "Handler/Handler2.ashx?";
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi4;
                                        // FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi4;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //'---------------------------------------------load the photo-------------------------------------
                                        //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                                        MyImg mi5 = new MyImg();
                                        mi5.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi5;
                                        //'----------------------------------------------------------------
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        if (txtCOE.Text != "")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        }
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------set the heading for the columns--------
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                                        //   FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade/Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------------------------------------------------
                                        count = FpMarkSheet.Sheets[0].RowCount - 29;
                                        subjcnt = 0;
                                        //'--------------------------------------------------------------------------------------
                                    }
                                    subjcnt += 1;
                                    //'--------------------------------get the semester value
                                    maxsem = GetFunction("Select max(semester) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "'");
                                    p += 1;
                                    Subno = dr_exam["subject_no"].ToString();
                                    getsem = dr_exam["semester"].ToString();
                                    getresult = dr_exam["result"].ToString();
                                    getsubno = dr_exam["subject_no"].ToString();
                                    getsubcode = dr_exam["subject_code"].ToString();
                                    getsubname = dr_exam["subject_name"].ToString();
                                    ////int gettotal = int.Parse(dr_exam["total"].ToString());
                                    if (dr_exam["grade"].ToString() != "")
                                    {
                                        grade = dr_exam["grade"].ToString();
                                        Session["grade_new"] = grade;
                                        credit = dr_exam["cp"].ToString();
                                        flag = true;
                                        //  mark = dr_exam["total"].ToString();
                                    }
                                    else
                                    {
                                        credit = dr_exam["cp"].ToString();//added on 30.05.12
                                        mark = dr_exam["total"].ToString();//added on 30.05.12
                                        //'------------------------------------query for get the link value
                                        string strsecrs = "select linkvalue from inssettings where linkname='Corresponding Grade' and college_code=" + Session["collegecode"] + "";
                                        SqlCommand cmd_secrs = new SqlCommand(strsecrs, con_secrs);
                                        con_secrs.Close();
                                        con_secrs.Open();
                                        SqlDataReader dr_secrs;
                                        dr_secrs = cmd_secrs.ExecuteReader();
                                        dr_secrs.Read();
                                        if (dr_secrs["linkvalue"].ToString() == "0")
                                        {
                                            string strnew = string.Empty;
                                            //'----------------------- query for get the ponits for grade details 
                                            strnew = " select * from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + "";
                                            SqlCommand cmd_new = new SqlCommand(strnew, con_new);
                                            con_new.Close();
                                            con_new.Open();
                                            SqlDataReader dr_new;
                                            dr_new = cmd_new.ExecuteReader();
                                            dr_new.Read();
                                            if (dr_new.HasRows == true)
                                            {
                                                flag = true;
                                                //convertgrade(RollNo, Subno);
                                                //credit = funccredit;
                                                //grade = funcgrade;
                                                grade = Session["grade_new"].ToString();
                                                if (mark != "")
                                                {
                                                    getmark = mark;
                                                    markflag = true;
                                                }
                                                else
                                                {
                                                    mark = "'" + " " + "'";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            getmark = mark;
                                            markflag = true;
                                            con_new.Close();
                                            con_new.Open();
                                            string query_new = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + IntExamCode + " and Attempts =1 and roll_no='" + RollNo + "' and  Mark_Entry.subject_no='" + getsubno + "' order by subject_type desc,mark_entry.subject_no";
                                            SqlCommand com_new = new SqlCommand(query_new, con_new);
                                            SqlDataReader drmrkentry = com_new.ExecuteReader();
                                            drmrkentry.Read();
                                            if (drmrkentry.HasRows == true)
                                            {
                                                if ((drmrkentry["internal_mark"].ToString() != "") && (drmrkentry["External_mark"].ToString() != ""))
                                                {
                                                    if (Convert.ToDouble(drmrkentry["internal_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_int_marks"].ToString()) && Convert.ToDouble(drmrkentry["External_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_ext_marks"].ToString()))
                                                    {
                                                        convertgrade(RollNo, getsubno);
                                                        result = "Pass";
                                                    }
                                                    else
                                                    {
                                                        funcgrade = "RA";
                                                        result = "Fail";
                                                    }
                                                    //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = funcgrade.ToString();         
                                                    grade = funcgrade.ToString();
                                                }
                                            }
                                            else
                                            {
                                                grade = "-";
                                                result = "-";
                                            }
                                        }
                                    }
                                    if (grade != "")
                                    {
                                        //   grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  " + mark + " between frange and trange";
                                        grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  Mark_Grade='" + grade.ToString() + "'";
                                        SqlCommand cmd_grademas = new SqlCommand(grademas, con_grademas);
                                        con_grademas.Close();
                                        con_grademas.Open();
                                        SqlDataReader dr_grademas;
                                        dr_grademas = cmd_grademas.ExecuteReader();
                                        while (dr_grademas.Read())
                                        {
                                            grpoints = Convert.ToDouble(dr_grademas["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                    string strcredit = string.Empty;
                                    strcredit = "select credit_points from subject where subject_no= " + Subno + " ";
                                    SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                                    con_credit.Close();
                                    con_credit.Open();
                                    SqlDataReader dr_credit;
                                    dr_credit = cmd_credit.ExecuteReader();
                                    dr_credit.Read();
                                    if (dr_credit.HasRows == true)
                                    {
                                        grcredit = dr_credit["credit_points"].ToString();
                                        grcredit1 = grcredit1 + Convert.ToDouble(grcredit);
                                    }
                                    else
                                    {
                                        grcredit = "0";
                                    }
                                    gpa = grpoints * Convert.ToDouble(grcredit);
                                    gpa1 = gpa1 + gpa;
                                    if (grcredit1 > 0)
                                        g = gpa1 / grcredit1;
                                    else
                                        g = 0;
                                    if (getsem == "")
                                    {
                                        sem = string.Empty;
                                    }
                                    else
                                    {
                                        sem = getsem;
                                    }
                                    //string tformat =string.Empty;
                                    //string tattr =string.Empty;
                                    //string trowrtf =string.Empty;
                                    //'----------chk the condition for oldsem
                                    if (sem != oldsem)
                                    {
                                        //'----------condn for once flag
                                        if (once == false)
                                        {
                                            string remark = string.Empty;
                                            //'-------------------------condn for optionflag
                                            if (optionflag == true)
                                            {
                                                string stroption = string.Empty;
                                                stroption = "select distinct uncompulsory_subject.subject_no,subject_name,subject_code,remarks from uncompulsory_subject,subject where uncompulsory_subject.subject_no=subject.subject_no and degree_code=" + degree_code + " and semester=" + current_sem + " and batch_year=" + batch_year + " and roll_no='" + RollNo + "' order by subject_code asc";
                                                con_option.Close();
                                                con_option.Open();
                                                SqlCommand cmd_option = new SqlCommand(stroption, con_option);
                                                SqlDataReader dr_option;
                                                dr_option = cmd_option.ExecuteReader();
                                                while (dr_option.Read())
                                                {
                                                    if (dr_option.HasRows)
                                                    {
                                                        if (dr_option["subject_name"].ToString() != "")
                                                        {
                                                            Subname = dr_option["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subname = string.Empty;
                                                        }
                                                        if (dr_option["subject_code"].ToString() != "")
                                                        {
                                                            SubCode = dr_option["subject_code"].ToString();
                                                        }
                                                        else
                                                        {
                                                            SubCode = string.Empty;
                                                        }
                                                        if (dr_option["subject_no"].ToString() != "")
                                                        {
                                                            Subno = dr_option["subject_no"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subno = string.Empty;
                                                        }
                                                        if (dr_option["remarks"].ToString() != "")
                                                        {
                                                            remark = dr_option["remarks"].ToString();
                                                        }
                                                        else
                                                        {
                                                            remark = string.Empty;
                                                        }
                                                        sem = GetSemester_AsNumber(Convert.ToInt32(current_sem)).ToString();
                                                        if (sem == "1")
                                                            sem2 = "I";
                                                        else if (sem == "2")
                                                            sem2 = "II";
                                                        else if (sem == "3")
                                                            sem2 = "III";
                                                        else if (sem == "4")
                                                            sem2 = "IV";
                                                        else if (sem == "5")
                                                            sem2 = "V";
                                                        else if (sem == "6")
                                                            sem2 = "VI";
                                                        else if (sem == "7")
                                                            sem2 = "VII";
                                                        else if (sem == "8")
                                                            sem2 = "VIII";
                                                        else if (sem == "9")
                                                            sem2 = "IX";
                                                        else if (sem == "10")
                                                            sem2 = "X";
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        if (Chkbxcou.Checked == false)
                                                        {
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            // FpMarkSheet.Sheets[0].Cells[count + subjcnt ,1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }//'--end hasrows of dr_option
                                                }//-end while dr_option
                                            }//-end of optionflag
                                        }//'--------end once flag condn
                                        once = true;
                                    }//'-------------------------end for condn sem!=oldsem
                                    //'===================================================================================
                                    if (getsubname != "")
                                    {
                                        Subname = getsubname;
                                    }
                                    else
                                    {
                                        Subname = string.Empty;
                                    }
                                    if (getsubcode != "")
                                    {
                                        SubCode = getsubcode;
                                    }
                                    else
                                    {
                                        SubCode = string.Empty;
                                    }
                                    if (getsubno != "")
                                    {
                                        Subno = getsubno;
                                    }
                                    else
                                    {
                                        Subno = string.Empty;
                                    }
                                    if (getresult != "")
                                    {
                                        result = getresult;
                                    }
                                    else
                                    {
                                        result = string.Empty;
                                    }
                                    if (getsem != "")
                                    {
                                        current_sem = getsem;
                                    }
                                    else
                                    {
                                        current_sem = string.Empty;
                                    }
                                    if (sem == "1")
                                        sem2 = "I";
                                    else if (sem == "2")
                                        sem2 = "II";
                                    else if (sem == "3")
                                        sem2 = "III";
                                    else if (sem == "4")
                                        sem2 = "IV";
                                    else if (sem == "5")
                                        sem2 = "V";
                                    else if (sem == "6")
                                        sem2 = "VI";
                                    else if (sem == "7")
                                        sem2 = "VII";
                                    else if (sem == "8")
                                        sem2 = "VIII";
                                    else if (sem == "9")
                                        sem2 = "IX";
                                    else if (sem == "10")
                                        sem2 = "X";
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    if (Chkbxcou.Checked == false)
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    //'==================================================================================
                                    if (credit == "0")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = credit;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //'---------------------------- chk the condn for markflag true
                                    if (markflag == true)
                                    {
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = mark;
                                    }
                                    else
                                    {
                                        if (result == "Pass" || result == "pass")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else if (result.ToUpper() == "FAIL")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = grpoints.ToString();
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result.ToUpper() == "FAIL")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = grpoints.ToString();
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = result;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "SA" || result == "sa")
                                    {
                                        //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "NS" || result == "ns")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "NS";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "AAA" || result == "aaa")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "AB";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result.ToUpper() == "FAIL")
                                    {
                                        if (grade.ToUpper() == "RA")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "FAIL";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    else
                                    {
                                        if (credit == "0")
                                        {
                                            //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColor = Color.Black;
                                }
                            }
                            FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorBottom = Color.Black;
                            //'---------------------------------------------------------------after while nxt subject will be read
                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            string coe1 = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                            MyImg coeimg1 = new MyImg();
                            coeimg1.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                            //   FpMarkSheet.Sheets[0].RowCount += 1;
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 6].Text = sem3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 6].HorizontalAlign = HorizontalAlign.Center;
                            string ccva = string.Empty;
                            ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
                            //'-----------------------------query for calculating the sum of credit points
                            //   FpMarkSheet.Sheets[0].RowCount += 1;
                            Enrolledcredit = GetFunction("select sum(s.credit_points) from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no and sy.batch_year=" + batch_year + " and sy.degree_code=" + degree_code + " and sc.semester<=" + semdec + " and roll_no='" + RollNo + "'");
                            if (ccva == "False")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit:";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit;
                            }
                            else
                            {
                                //'-------------------condn for out gone chkbox value
                                if (ChkOutgone.Checked == true)
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit OutGone:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit + " " + "(Out Gone)";
                                }
                                else
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit;
                                }
                            }
                            //   EnrolledCredit =string.Empty;
                            sem = semdec.ToString();
                            //'=============================================calculate the cgpa value
                            string CGPA_Val = string.Empty;
                            string CPA_Val = string.Empty;
                            string sem4 = string.Empty;
                            if (flag == false)
                            {
                                cgpa(RollNo, Convert.ToInt32(sem));
                            }
                            else//Rajkumar on 16-6-2018
                            {
                                //CPA_Val = Calulat_GPA(RollNo, sem);
                                //CGPA_Val = Calculete_CGPA(RollNo, sem);
                                CPA_Val = d2.Calulat_GPA_Semwise(RollNo, degree_code, batch_year, exam_month, exam_year, collegecode);
                                CGPA_Val = d2.Calculete_CGPA(RollNo, sem, degree_code, batch_year, "", collegecode, false);
                            }
                            string val1 = d2.GetFunctionv("select value from Master_Settings where settings = 'include gpa for fail student'");//Rajkumar 28/5/2018
                            if (val1.Trim() == "true" || val1.Trim() == "1")
                            {
                                CPA_Val = d2.Calulat_GPA_Semwise(RollNo, degree_code, batch_year, exam_month, exam_year, collegecode);
                                CGPA_Val = d2.Calculete_CGPA(RollNo, sem, degree_code, batch_year, "", collegecode, false);
                            }
                            // if (Chkbxcou.Checked == false)
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //  else if (Chkbxcou.Checked == true)
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ChkOutgone.Checked == true)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].Text = sem3;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                if (maxsem != "")
                                {
                                    if (Convert.ToInt32(maxsem) == 1)
                                    {
                                        sem4 = "I";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 2)
                                    {
                                        sem4 = "II";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 3)
                                    {
                                        sem4 = "III";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 4)
                                    {
                                        sem4 = "IV";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 5)
                                    {
                                        sem4 = "V";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 6)
                                    {
                                        sem4 = "VI";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 7)
                                    {
                                        sem4 = "VII";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 8)
                                    {
                                        sem4 = "VIII";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 9)
                                    {
                                        sem4 = "IX";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 10)
                                    {
                                        sem4 = "X";
                                    }
                                }
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].Text = sem4;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ccva == "False")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val;
                            }
                            else
                            {
                                if (ChkOutgone.Checked == true)
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA OutGone";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val + " " + "(Out Gone)";
                                }
                                else
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val;
                                }
                            }
                            //if (Chkbxcou.Checked == false)
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //else if (Chkbxcou.Checked == true)
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 2].Text = sem3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 2].HorizontalAlign = HorizontalAlign.Center;
                            if (ccva == "False")
                            {
                                EarnedVal = GetEarnedCreditoutgone(RollNo);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit:";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal;
                            }
                            else
                            {
                                if (ChkOutgone.Checked == true)
                                {
                                    EarnedVal = GetEarnedCreditoutgone(RollNo);
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit OutGone:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal + " " + "(Out Gone)";
                                }
                                else
                                {
                                    EarnedVal = GetEarnedCredit(RollNo);
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal;
                                }
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 7].Text = "CGPA";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 8].Text = CGPA_Val;
                            //---current date time
                            DateTime currentdate;
                            currentdate = System.DateTime.Now;
                            string[] split_currentdate = Convert.ToString(currentdate).Split(new char[] { ' ' });
                            string[] split_date = split_currentdate[0].Split(new char[] { '/' });
                            string concat_date = split_date[1].ToString() + '/' + split_date[0].ToString() + '/' + split_date[2].ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = "Date";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 1].Text = concat_date.ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                            g = 0;
                            overallcredit = 0;
                            cgpa2 = 0;
                            gpa = 0;
                            gpa1 = 0;
                            g = 0;
                            grcredit = string.Empty;
                            markflag = false;
                            flag = false;
                            once = false;
                            // }
                        }//'----------end for loop
                    }
                    else
                    {
                        lblError.Text = string.Empty;
                        lblError.Visible = false;
                        lblstudselect.Visible = true;
                        lblstudselect.Text = "Please Select Atleast One Student To Print The GradeSheet";
                        btnPrint.Visible = false;
                    }
                    //'-------------------------------going to get the second student
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpMarkSheet.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpMarkSheet.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                }
                else
                {
                    FpExternal.Visible = false;
                    btnxl.Visible = false;//added by srinath 24/5/2014
                    lblxl.Visible = false;
                    txtxlname.Visible = false;
                    lblnorec.Visible = true;
                    Buttontotal.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void MarkSheet_ddl2()
    {
        try
        {
            FpExternal.Visible = true;
            btnxl.Visible = true;//added by srinath 24/5/2014
            lblxl.Visible = true;
            txtxlname.Visible = true;
            lblnorec.Visible = false;
            FpMarkSheet.Visible = true;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            FpMarkSheet.Sheets[0].RowHeader.Visible = false;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Bold = true;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
            FpMarkSheet.Sheets[0].RowCount = 0;
            FpMarkSheet.Sheets[0].PageSize = 40;
            string maxsem = string.Empty;
            int i = 0;
            int semdec = 0;
            int k = 0;
            int j = 0;
            int no = 0;
            int nos = 0;
            string Subno = string.Empty;
            string RegNo = string.Empty;
            string RollNo = string.Empty;
            int Varsel;
            string credit = string.Empty;
            string result = string.Empty;
            string name = string.Empty;
            string course = string.Empty;
            string dob = string.Empty;
            string dobtemp = string.Empty;
            string mont = string.Empty;
            //string mon =string.Empty;
            string yr = string.Empty;
            string gender = string.Empty;
            string grade = string.Empty;
            //Dim courseRs As New ADODB.Recordset
            //Dim RsStudent As New ADODB.Recordset
            //Dim rsgrademas As New ADODB.Recordset
            //  int grcredit1 = 0;
            double grcredit1 = 0;
            string grcredit = string.Empty;
            //  int gpacal = 0;
            double gpacal = 0;
            //  int gpa = 0;
            double gpa = 0;
            int pval = 0;
            bool flag = false;
            // int gpa1 = 0;
            double gpa1 = 0;
            string sem = string.Empty;
            string regulation = string.Empty;
            string department = string.Empty;
            string Subname = string.Empty;
            string SubCode = string.Empty;
            int TotalPages = 0;
            //int g = 0;
            double g = 0;
            bool once = false;
            string sem1 = string.Empty;
            string sem2 = string.Empty;
            string sem3 = string.Empty;
            // int l = 0;
            bool optionflag = false;
            string oldsem = string.Empty;
            string getmnth = string.Empty;
            string getyear = string.Empty;
            string grademas = string.Empty;
            //  string grpoints =string.Empty;
            double grpoints = 0;
            string concat = string.Empty;
            string dept_name = string.Empty;
            string strExam_month = string.Empty;
            int subjcnt = 0;
            string strcourse = string.Empty;
            int chkstud_selected_cnt = 0;
            FpMarkSheet.Sheets[0].ColumnCount = 9;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            //---------------------------------------get the examcode
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            if (current_sem != "")
            {
                semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
            }
            if ((exam_month != "") && (exam_year != "") && (exam_month != "0") && (exam_year != "0"))
            {
                //  IntExamCode = Convert.ToInt16(GetFunction("select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month.ToString() + " and exam_year= " + exam_year.ToString() + ""));
                IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + exam_month.ToString() + "' and exam_year= '" + exam_year.ToString() + "'"));
                if (IntExamCode != 0)
                {
                    lblnorec.Visible = false;
                    FpExternal.Visible = true;
                    btnxl.Visible = true;//added by srinath 24/5/2014
                    lblxl.Visible = true;
                    txtxlname.Visible = true;
                    int sel;
                    //'-------loop for get chkbox val
                    FpExternal.SaveChanges();
                    string chkrollno = string.Empty;
                    string temprollno = string.Empty;
                    string chkRegNo = string.Empty;
                    string chkname = string.Empty;
                    string tempRegNo = string.Empty;
                    string tempname = string.Empty;
                    for (j = 0; j <= FpExternal.Sheets[0].RowCount - 1; j++)
                    {
                        // isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(flagrow, 0).ToString());
                        sel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(j, 1).ToString());
                        if (sel == 1)
                        {
                            chkstud_selected_cnt += 1;
                            pval += 1;
                            chkRegNo = FpExternal.Sheets[0].Cells[j, 3].Text;
                            chkname = FpExternal.Sheets[0].Cells[j, 4].Text;
                            chkrollno = FpExternal.Sheets[0].Cells[j, 2].Tag.ToString();
                            if (temprollno == "")
                            {
                                temprollno = chkrollno;
                            }
                            else
                            {
                                temprollno = temprollno + "," + chkrollno;
                            }
                            if (tempRegNo == "")
                            {
                                tempRegNo = chkRegNo;
                            }
                            else
                            {
                                tempRegNo = tempRegNo + "," + chkRegNo;
                            }
                            if (tempname == "")
                            {
                                tempname = chkname;
                            }
                            else
                            {
                                tempname = tempname + "," + chkname;
                            }
                        }
                    }
                    int chkstudent_count = 0;
                    string[] split_temprollno = temprollno.Split(',');
                    string[] split_tempRegNo = tempRegNo.Split(',');
                    string[] split_tempname = tempname.Split(',');
                    if (chkstud_selected_cnt > 0)
                    {
                        for (i = 0; i <= split_temprollno.GetUpperBound(0); i++)
                        {
                            lblstudselect.Visible = false;
                            btnPrint.Visible = true;
                            lblError.Visible = false;
                            chkstudent_count += 1;
                            lblnorec.Visible = false;
                            subjcnt = 0;
                            no = 0;
                            string EarnedCredit = string.Empty;
                            grcredit1 = 0;
                            string[] studarr = new string[pval];
                            Varsel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(i, 1).ToString());
                            if (Varsel == 1)
                            {
                                TotalPages += 1;
                            }
                            RollNo = split_temprollno[i].ToString();
                            RegNo = split_tempRegNo[i].ToString();
                            name = split_tempname[i].ToString();
                            //'----------------query for get the branch,degreecode,batchyear etc
                            string strgetdetail = string.Empty;
                            strgetdetail = "select branch_code,registration.current_semester ,registration.degree_code,sex,registration.batch_year,dob from registration,applyn where applyn.app_no=registration.App_no and Roll_no='" + RollNo + "'";
                            SqlCommand cmd_getdetail = new SqlCommand(strgetdetail, con_getdetail);
                            con_getdetail.Close();
                            con_getdetail.Open();
                            SqlDataReader dr_getdetail;
                            dr_getdetail = cmd_getdetail.ExecuteReader();
                            dr_getdetail.Read();
                            if (dr_getdetail.HasRows)
                            {
                                sem1 = dr_getdetail["current_semester"].ToString();
                                if (dr_getdetail["dob"].ToString() != "")
                                {
                                    dobtemp = dr_getdetail["dob"].ToString();
                                    string[] split_dobtemp = dobtemp.Split(new char[] { ' ' });
                                    string[] split_dob = split_dobtemp[0].Split(new char[] { '/' });
                                    string getday = split_dob[1].ToString();
                                    getmnth = split_dob[0].ToString();
                                    getyear = split_dob[2].ToString();
                                    concat = getday + '/' + getmnth + '/' + getyear;
                                }
                                else
                                {
                                    concat = string.Empty;
                                }
                                if (dr_getdetail["sex"].ToString() == "1")
                                {
                                    gender = "Female";
                                }
                                else
                                {
                                    gender = "Male";
                                }
                                if (dr_getdetail["batch_year"].ToString() != null)
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                                else
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                            }//'-----end hasrows
                            //'---------------------------query for get the exam_mopnth,exam_year
                            string strdaters = string.Empty;
                            strdaters = "select exam_month,Exam_year,current_semester from exam_details where Exam_Code=" + IntExamCode + "";
                            ExamCode = IntExamCode;
                            SqlCommand cmd_daters = new SqlCommand(strdaters, con_daters);
                            con_daters.Close();
                            con_daters.Open();
                            SqlDataReader dr_daters;
                            dr_daters = cmd_daters.ExecuteReader();
                            dr_daters.Read();
                            if (dr_daters.HasRows)
                            {
                                mont = dr_daters["exam_month"].ToString();
                                yr = dr_daters["Exam_year"].ToString();
                                oldsem = dr_daters["current_semester"].ToString();
                                sem = dr_daters["current_semester"].ToString();
                                if (sem == "1")
                                    sem3 = "I";
                                else if (sem == "2")
                                    sem3 = "II";
                                else if (sem == "3")
                                    sem3 = "III";
                                else if (sem == "4")
                                    sem3 = "IV";
                                else if (sem == "5")
                                    sem3 = "V";
                                else if (sem == "6")
                                    sem3 = "VI";
                                else if (sem == "7")
                                    sem3 = "VII";
                                else if (sem == "8")
                                    sem3 = "VIII";
                                else if (sem == "9")
                                    sem3 = "IX";
                                else if (sem == "10")
                                    sem3 = "X";
                                //'-------------------
                            }//'- end dr_daters hasrows
                            //'-------------------------------------display the month as string-----------
                            if (exam_month == "1")
                                strExam_month = "Jan";
                            else if (exam_month == "2")
                                strExam_month = "Feb";
                            else if (exam_month == "3")
                                strExam_month = "Mar";
                            else if (exam_month == "4")
                                strExam_month = "Apr";
                            else if (exam_month == "5")
                                strExam_month = "May";
                            else if (exam_month == "6")
                                strExam_month = "Jun";
                            else if (exam_month == "7")
                                strExam_month = "Jul";
                            else if (exam_month == "8")
                                strExam_month = "Aug";
                            else if (exam_month == "9")
                                strExam_month = "Sep";
                            else if (exam_month == "10")
                                strExam_month = "Oct";
                            else if (exam_month == "11")
                                strExam_month = "Nov";
                            else if (exam_month == "12")
                                strExam_month = "DEc";
                            //'---------------------------------------query for get the course name dept name for the strudent
                            // if (strcourse != "")
                            //{
                            strcourse = "select course_name,dept_name from course,department,degree where course.course_id=degree.course_id and degree.dept_code=department.dept_code and degree_code='" + dr_getdetail["degree_code"] + "'";
                            SqlCommand cmd_course = new SqlCommand(strcourse, con_course);
                            con_course.Close();
                            con_course.Open();
                            SqlDataReader dr_course;
                            dr_course = cmd_course.ExecuteReader();
                            dr_course.Read();
                            if (dr_course.HasRows)
                            {
                                if (txtGetDegree.Text == "")
                                {
                                    course = dr_course["course_name"].ToString();
                                }
                                else
                                {
                                    course = txtGetDegree.Text.Trim();
                                }
                                if (txtDepartment.Text == "")
                                {
                                    dept_name = dr_course["dept_name"].ToString();
                                }
                                else
                                {
                                    dept_name = txtDepartment.Text.Trim();
                                }
                                if (Chkbxcou.Checked == true)
                                {
                                    cou = 1;
                                }
                                else
                                {
                                    cou = 0;
                                }
                            }//'------end if dr_course
                            //  }
                            //NextPage: 
                            //RegNo = FpExternal.Sheets[0].Cells[i, 2].Text;
                            //name = FpExternal.Sheets[0].Cells[i, 3].Text;
                            //'----------------------------------------------incremen the row count
                            FpMarkSheet.Sheets[0].RowCount += 40;
                            //'------------------------------------load the clg information
                            string collnamenew1 = string.Empty;
                            string address1 = string.Empty;
                            string address3 = string.Empty;
                            string address = string.Empty;
                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                            {
                                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                SqlCommand collegecmd = new SqlCommand(college, con);
                                SqlDataReader collegename;
                                con.Close();
                                con.Open();
                                collegename = collegecmd.ExecuteReader();
                                if (collegename.HasRows)
                                {
                                    while (collegename.Read())
                                    {
                                        collnamenew1 = collegename["collname"].ToString();
                                        address1 = collegename["address1"].ToString();
                                        address3 = collegename["address3"].ToString();
                                        address = address1 + "," + address3;
                                    }
                                }
                            }
                            //'---------------------------------------------load theclg logo photo-------------------------------------
                            MyImg mi3 = new MyImg();
                            mi3.ImageUrl = "Handler/Handler2.ashx?";
                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi3;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                            //'---------------------------------------------load the student photo-------------------------------------
                            //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                            MyImg mi1 = new MyImg();
                            mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi1;
                            //'----------------------------------------------------------------
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            if (txtCOE.Text != "")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                            //'--------------------------------------------set the heading for the columns--------
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                            //   FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade/Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColor = Color.Black;
                            //'--------------------------------------------------------------------------------------
                            int p;
                            string getmark = string.Empty;
                            string getsem = string.Empty;
                            string getsubno = string.Empty;
                            string getsubname = string.Empty;
                            string getsubcode = string.Empty;
                            string getresult = string.Empty;
                            string Enrolledcredit = string.Empty;
                            int count = FpMarkSheet.Sheets[0].RowCount - 29;
                            //'-----------------------------------query for select the subject name and details
                            strexam = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "' order by semester desc,subject_type desc,subject.subject_no asc";
                            SqlCommand cmd_exam = new SqlCommand(strexam, con_exam);
                            con_exam.Close();
                            con_exam.Open();
                            dr_exam = cmd_exam.ExecuteReader();
                            if (dr_exam.HasRows)
                            {
                                nos += 1;
                                p = 0;
                                int sub_val = 1;
                                while (dr_exam.Read())
                                {
                                    if (subjcnt > (10 * sub_val))
                                    {
                                        sub_val = sub_val + 1;
                                        FpMarkSheet.Sheets[0].Rows[count + subjcnt + 1].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 2, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 2].Text = "- - -Continued- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        string coe = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                                        MyImg coeimg = new MyImg();
                                        coeimg.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //'----------------------------------------------incremen the row count
                                        FpMarkSheet.Sheets[0].RowCount += 40;
                                        //'------------------------------------load the clg information
                                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                        {
                                            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                            SqlCommand collegecmd = new SqlCommand(college, con);
                                            SqlDataReader collegename;
                                            con.Close();
                                            con.Open();
                                            collegename = collegecmd.ExecuteReader();
                                            if (collegename.HasRows)
                                            {
                                                while (collegename.Read())
                                                {
                                                    collnamenew1 = collegename["collname"].ToString();
                                                    address1 = collegename["address1"].ToString();
                                                    address3 = collegename["address3"].ToString();
                                                    address = address1 + "," + address3;
                                                }
                                            }
                                        }
                                        //'---------------------------------------------load theclg logo photo-------------------------------------
                                        //'----------------------
                                        MyImg mi4 = new MyImg();
                                        mi4.ImageUrl = "Handler/Handler2.ashx?";
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi4;
                                        // FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi4;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //'---------------------------------------------load the photo-------------------------------------
                                        //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                                        MyImg mi5 = new MyImg();
                                        mi5.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi5;
                                        //'----------------------------------------------------------------
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        if (txtCOE.Text != "")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        }
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------set the heading for the columns--------
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                                        //   FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade/Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------------------------------------------------
                                        count = FpMarkSheet.Sheets[0].RowCount - 29;
                                        subjcnt = 0;
                                        //'--------------------------------------------------------------------------------------
                                    }
                                    subjcnt += 1;
                                    //'--------------------------------get the semester value
                                    maxsem = GetFunction("Select max(semester) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "'");
                                    p += 1;
                                    Subno = dr_exam["subject_no"].ToString();
                                    getsem = dr_exam["semester"].ToString();
                                    getresult = dr_exam["result"].ToString();
                                    getsubno = dr_exam["subject_no"].ToString();
                                    getsubcode = dr_exam["subject_code"].ToString();
                                    getsubname = dr_exam["subject_name"].ToString();
                                    //int gettotal = int.Parse(dr_exam["total"].ToString());
                                    if (dr_exam["grade"].ToString() != "")
                                    {
                                        grade = dr_exam["grade"].ToString();
                                        Session["grade_new"] = grade;
                                        credit = dr_exam["cp"].ToString();
                                        //mark = dr_exam["total"].ToString();
                                    }
                                    else
                                    {
                                        //'------------------------------------query for get the link value
                                        string strsecrs = "select linkvalue from inssettings where linkname='Corresponding Grade' and college_code=" + Session["collegecode"] + "";
                                        SqlCommand cmd_secrs = new SqlCommand(strsecrs, con_secrs);
                                        con_secrs.Close();
                                        con_secrs.Open();
                                        SqlDataReader dr_secrs;
                                        dr_secrs = cmd_secrs.ExecuteReader();
                                        dr_secrs.Read();
                                        if (dr_secrs["linkvalue"].ToString() == "1")
                                        {
                                            string strnew = string.Empty;
                                            //'----------------------- query for get the ponits for grade details 
                                            strnew = " select * from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + "";
                                            SqlCommand cmd_new = new SqlCommand(strnew, con_new);
                                            con_new.Close();
                                            con_new.Open();
                                            SqlDataReader dr_new;
                                            dr_new = cmd_new.ExecuteReader();
                                            dr_new.Read();
                                            if (dr_new.HasRows == true)
                                            {
                                                flag = true;
                                                //convertgrade(RollNo, Subno);
                                                //credit = funccredit;
                                                //grade = funcgrade;
                                                grade = Session["grade_new"].ToString();
                                                if (mark != "")
                                                {
                                                    getmark = mark;
                                                    markflag = true;
                                                }
                                                else
                                                {
                                                    mark = "'" + " " + "'";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            getmark = mark;
                                            markflag = true;
                                            con_new.Close();
                                            con_new.Open();
                                            string query_new = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + IntExamCode + " and Attempts =1 and roll_no='" + RollNo + "' and  Mark_Entry.subject_no='" + getsubno + "' order by subject_type desc,mark_entry.subject_no";
                                            SqlCommand com_new = new SqlCommand(query_new, con_new);
                                            SqlDataReader drmrkentry = com_new.ExecuteReader();
                                            drmrkentry.Read();
                                            if (Convert.ToInt16(drmrkentry["internal_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_int_marks"].ToString()) && Convert.ToInt16(drmrkentry["External_mark"].ToString()) >= Convert.ToInt16(drmrkentry["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(RollNo, getsubno);
                                                result = "Pass";
                                            }
                                            else
                                            {
                                                funcgrade = "RA";
                                                result = "Fail";
                                            }
                                            //FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = funcgrade.ToString();         
                                            grade = funcgrade.ToString();
                                        }
                                    }
                                    if (grade != "")
                                    {
                                        //   grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  " + mark + " between frange and trange";
                                        grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  Mark_Grade='" + grade.ToString() + "'";
                                        SqlCommand cmd_grademas = new SqlCommand(grademas, con_grademas);
                                        con_grademas.Close();
                                        con_grademas.Open();
                                        SqlDataReader dr_grademas;
                                        dr_grademas = cmd_grademas.ExecuteReader();
                                        while (dr_grademas.Read())
                                        {
                                            grpoints = Convert.ToDouble(dr_grademas["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                    string strcredit = string.Empty;
                                    strcredit = "select credit_points from subject where subject_no= " + Subno + " ";
                                    SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                                    con_credit.Close();
                                    con_credit.Open();
                                    SqlDataReader dr_credit;
                                    dr_credit = cmd_credit.ExecuteReader();
                                    dr_credit.Read();
                                    if (dr_credit.HasRows == true)
                                    {
                                        grcredit = dr_credit["credit_points"].ToString();
                                        grcredit1 = grcredit1 + Convert.ToDouble(grcredit);
                                    }
                                    else
                                    {
                                        grcredit = "0";
                                    }
                                    gpa = grpoints * Convert.ToDouble(grcredit);
                                    gpa1 = gpa1 + gpa;
                                    if (grcredit1 > 0)
                                        g = gpa1 / grcredit1;
                                    else
                                        g = 0;
                                    if (getsem == "")
                                    {
                                        sem = string.Empty;
                                    }
                                    else
                                    {
                                        sem = getsem;
                                    }
                                    //string tformat =string.Empty;
                                    //string tattr =string.Empty;
                                    //string trowrtf =string.Empty;
                                    //'----------chk the condition for oldsem
                                    if (sem != oldsem)
                                    {
                                        //'----------condn for once flag
                                        if (once == false)
                                        {
                                            string remark = string.Empty;
                                            //'-------------------------condn for optionflag
                                            if (optionflag == true)
                                            {
                                                string stroption = string.Empty;
                                                stroption = "select distinct uncompulsory_subject.subject_no,subject_name,subject_code,remarks from uncompulsory_subject,subject where uncompulsory_subject.subject_no=subject.subject_no and degree_code=" + degree_code + " and semester=" + current_sem + " and batch_year=" + batch_year + " and roll_no='" + RollNo + "' order by subject_code asc";
                                                con_option.Close();
                                                con_option.Open();
                                                SqlCommand cmd_option = new SqlCommand(stroption, con_option);
                                                SqlDataReader dr_option;
                                                dr_option = cmd_option.ExecuteReader();
                                                while (dr_option.Read())
                                                {
                                                    if (dr_option.HasRows)
                                                    {
                                                        if (dr_option["subject_name"].ToString() != "")
                                                        {
                                                            Subname = dr_option["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subname = string.Empty;
                                                        }
                                                        if (dr_option["subject_code"].ToString() != "")
                                                        {
                                                            SubCode = dr_option["subject_code"].ToString();
                                                        }
                                                        else
                                                        {
                                                            SubCode = string.Empty;
                                                        }
                                                        if (dr_option["subject_no"].ToString() != "")
                                                        {
                                                            Subno = dr_option["subject_no"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subno = string.Empty;
                                                        }
                                                        if (dr_option["remarks"].ToString() != "")
                                                        {
                                                            remark = dr_option["remarks"].ToString();
                                                        }
                                                        else
                                                        {
                                                            remark = string.Empty;
                                                        }
                                                        sem = GetSemester_AsNumber(Convert.ToInt32(current_sem)).ToString();
                                                        if (sem == "1")
                                                            sem2 = "I";
                                                        else if (sem == "2")
                                                            sem2 = "II";
                                                        else if (sem == "3")
                                                            sem2 = "III";
                                                        else if (sem == "4")
                                                            sem2 = "IV";
                                                        else if (sem == "5")
                                                            sem2 = "V";
                                                        else if (sem == "6")
                                                            sem2 = "VI";
                                                        else if (sem == "7")
                                                            sem2 = "VII";
                                                        else if (sem == "8")
                                                            sem2 = "VIII";
                                                        else if (sem == "9")
                                                            sem2 = "IX";
                                                        else if (sem == "10")
                                                            sem2 = "X";
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        if (Chkbxcou.Checked == false)
                                                        {
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            // FpMarkSheet.Sheets[0].Cells[count + subjcnt ,1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }//'--end hasrows of dr_option
                                                }//-end while dr_option
                                            }//-end of optionflag
                                        }//'--------end once flag condn
                                        once = true;
                                    }//'-------------------------end for condn sem!=oldsem
                                    //'===================================================================================
                                    if (getsubname != "")
                                    {
                                        Subname = getsubname;
                                    }
                                    else
                                    {
                                        Subname = string.Empty;
                                    }
                                    if (getsubcode != "")
                                    {
                                        SubCode = getsubcode;
                                    }
                                    else
                                    {
                                        SubCode = string.Empty;
                                    }
                                    if (getsubno != "")
                                    {
                                        Subno = getsubno;
                                    }
                                    else
                                    {
                                        Subno = string.Empty;
                                    }
                                    if (getresult != "")
                                    {
                                        result = getresult;
                                    }
                                    else
                                    {
                                        result = string.Empty;
                                    }
                                    if (getsem != "")
                                    {
                                        current_sem = getsem;
                                    }
                                    else
                                    {
                                        current_sem = string.Empty;
                                    }
                                    if (sem == "1")
                                        sem2 = "I";
                                    else if (sem == "2")
                                        sem2 = "II";
                                    else if (sem == "3")
                                        sem2 = "III";
                                    else if (sem == "4")
                                        sem2 = "IV";
                                    else if (sem == "5")
                                        sem2 = "V";
                                    else if (sem == "6")
                                        sem2 = "VI";
                                    else if (sem == "7")
                                        sem2 = "VII";
                                    else if (sem == "8")
                                        sem2 = "VIII";
                                    else if (sem == "9")
                                        sem2 = "IX";
                                    else if (sem == "10")
                                        sem2 = "X";
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    if (Chkbxcou.Checked == false)
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    //'==================================================================================
                                    if (credit == "0")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = credit;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //'---------------------------- chk the condn for markflag true
                                    if (markflag == true)
                                    {
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = mark;
                                    }
                                    else
                                    {
                                        if (result == "Pass" || result == "pass")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = grpoints.ToString();
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = result;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "SA" || result == "sa")
                                    {
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "NS" || result == "ns")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "NS";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "AAA" || result == "aaa")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "AB";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        if (credit == "0")
                                        {
                                            // FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColor = Color.Black;
                                }//'---------end dr_exam
                            }//'--------end while dr_exam
                            FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorBottom = Color.Black;
                            //'---------------------------------------------------------------after while nxt subject will be read
                            //  FpMarkSheet.Sheets[0].RowCount += 1;
                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            string coe1 = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                            MyImg coeimg1 = new MyImg();
                            coeimg1.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                            //   FpMarkSheet.Sheets[0].RowCount += 1;
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 5].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 6].Text = sem3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 6].HorizontalAlign = HorizontalAlign.Center;
                            string ccva = string.Empty;
                            ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
                            //'-----------------------------query for calculating the sum of credit points
                            //   FpMarkSheet.Sheets[0].RowCount += 1;
                            Enrolledcredit = GetFunction("select sum(s.credit_points) from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no and sy.batch_year=" + batch_year + " and sy.degree_code=" + degree_code + " and sc.semester<=" + semdec + " and roll_no='" + RollNo + "'");
                            if (ccva == "False")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit:";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit;
                            }
                            else
                            {
                                //'-------------------condn for out gone chkbox value
                                if (ChkOutgone.Checked == true)
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit OutGone:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit + " " + "(Out Gone)";
                                }
                                else
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 0].Text = "EnrolledCredit:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 3].Text = Enrolledcredit;
                                }
                            }
                            //   EnrolledCredit =string.Empty;
                            sem = semdec.ToString();
                            //'=============================================calculate the cgpa value
                            string CGPA_Val = string.Empty;
                            string CPA_Val = string.Empty;
                            string sem4 = string.Empty;
                            if (flag == false)
                            {
                                cgpa(RollNo, Convert.ToInt32(sem));
                            }
                            else
                            {
                                CPA_Val = Calulat_GPA(RollNo, sem);
                                CGPA_Val = Calculete_CGPA(RollNo, sem);
                            }
                            // if (Chkbxcou.Checked == false)
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //  else if (Chkbxcou.Checked == true)
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ChkOutgone.Checked == true)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].Text = sem3;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            else
                            {
                                if (maxsem != "")
                                {
                                    if (Convert.ToInt32(maxsem) == 1)
                                    {
                                        sem4 = "I";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 2)
                                    {
                                        sem4 = "II";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 3)
                                    {
                                        sem4 = "III";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 4)
                                    {
                                        sem4 = "IV";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 5)
                                    {
                                        sem4 = "V";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 6)
                                    {
                                        sem4 = "VI";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 7)
                                    {
                                        sem4 = "VII";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 8)
                                    {
                                        sem4 = "VIII";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 9)
                                    {
                                        sem4 = "IX";
                                    }
                                    else if (Convert.ToInt32(maxsem) == 10)
                                    {
                                        sem4 = "X";
                                    }
                                }
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].Text = sem4;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 2].HorizontalAlign = HorizontalAlign.Center;
                            }
                            if (ccva == "False")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val;
                            }
                            else
                            {
                                if (ChkOutgone.Checked == true)
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA OutGone";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val + " " + "(Out Gone)";
                                }
                                else
                                {
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 7].Text = "GPA";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 14, 8].Text = CPA_Val;
                                }
                            }
                            //if (Chkbxcou.Checked == false)
                            if (cou == 0)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            //else if (Chkbxcou.Checked == true)
                            else if (cou == 1)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].Text = "Semester";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 1].HorizontalAlign = HorizontalAlign.Center;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 2].Text = sem3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 2].HorizontalAlign = HorizontalAlign.Center;
                            if (ccva == "False")
                            {
                                EarnedVal = GetEarnedCreditoutgone(RollNo);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit:";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal;
                            }
                            else
                            {
                                if (ChkOutgone.Checked == true)
                                {
                                    EarnedVal = GetEarnedCreditoutgone(RollNo);
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit OutGone:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal + " " + "(Out Gone)";
                                }
                                else
                                {
                                    EarnedVal = GetEarnedCredit(RollNo);
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Text = "EarnedCredit:";
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 3].Text = EarnedVal;
                                }
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 7].Text = "CGPA";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 8].Text = CGPA_Val;
                            //---current date time
                            DateTime currentdate;
                            currentdate = System.DateTime.Now;
                            string[] split_currentdate = Convert.ToString(currentdate).Split(new char[] { ' ' });
                            string[] split_date = split_currentdate[0].Split(new char[] { '/' });
                            string concat_date = split_date[1].ToString() + '/' + split_date[0].ToString() + '/' + split_date[2].ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = "Date";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 1].Text = concat_date.ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                            g = 0;
                            overallcredit = 0;
                            cgpa2 = 0;
                            gpa = 0;
                            gpa1 = 0;
                            g = 0;
                            grcredit = string.Empty;
                            markflag = false;
                            flag = false;
                            once = false;
                            // }
                        }//'----------end for loop
                    }
                    else
                    {
                        lblError.Text = string.Empty;
                        lblError.Visible = false;
                        lblstudselect.Visible = true;
                        lblstudselect.Text = "Please Select Atleast One Student To Print The GradeSheet";
                        btnPrint.Visible = false;
                    }
                    //'-------------------------------going to get the second student
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpMarkSheet.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpMarkSheet.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                }
                else
                {
                    FpExternal.Visible = false;
                    btnxl.Visible = false;//added by srinath 24/5/2014
                    lblxl.Visible = false;
                    txtxlname.Visible = false;
                    lblnorec.Visible = true;
                    Buttontotal.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void MarkSheet1()
    {
        try
        {
            string syllabuscode = string.Empty;
            string hh = string.Empty;
            string sections_str = string.Empty;
            string coe1 = string.Empty;
            FpMarkSheet.Sheets[0].SheetName = " ";
            MyImg coeimg1 = new MyImg();
            coeimg1.ImageUrl = "principalHandler/Handler.ashx?";
            string month_new = string.Empty;
            int r = 0, r1 = 0;
            FpExternal.Visible = true;
            btnxl.Visible = true;//added by srinath 24/5/2014
            lblxl.Visible = true;
            txtxlname.Visible = true;
            lblnorec.Visible = false;
            FpMarkSheet.Visible = true;
            FpMarkSheet.Width = 690;
            FpMarkSheet.Sheets[0].ColumnCount = 9;
            FpMarkSheet.Sheets[0].Columns[0].Width = 35;
            FpMarkSheet.Sheets[0].Columns[1].Width = 55;
            FpMarkSheet.Sheets[0].Columns[2].Width = 55;
            FpMarkSheet.Sheets[0].Columns[3].Width = 105;
            FpMarkSheet.Sheets[0].Columns[4].Width = 120;
            FpMarkSheet.Sheets[0].Columns[5].Width = 100;
            FpMarkSheet.Sheets[0].Columns[6].Width = 90;
            FpMarkSheet.Sheets[0].Columns[7].Width = 70;
            FpMarkSheet.Sheets[0].Columns[8].Width = 60;
            FpMarkSheet.Sheets[0].AutoPostBack = false;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            FpMarkSheet.Sheets[0].RowHeader.Visible = false;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Bold = true;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
            FpMarkSheet.Sheets[0].RowCount = 0;
            // FpMarkSheet.Sheets[0].PageSize = 40;
            string maxsem = string.Empty;
            int i = 0;
            int semdec = 0;
            int k = 0;
            int j = 0;
            int no = 0;
            int nos = 0;
            string Subno = string.Empty;
            string RegNo = string.Empty;
            string RollNo = string.Empty;
            int Varsel;
            string credit = string.Empty;
            string result = string.Empty;
            string name = string.Empty;
            string course = string.Empty;
            string dob = string.Empty;
            string dobtemp = string.Empty;
            string mont = string.Empty;
            string mon = string.Empty;
            string yr = string.Empty;
            string gender = string.Empty;
            string grade = string.Empty;
            double grcredit1 = 0;
            string grcredit = string.Empty;
            double gpacal = 0;
            double gpa = 0;
            int pval = 0;
            bool flag = false;
            double gpa1 = 0;
            string sem = string.Empty;
            string regulation = string.Empty;
            string department = string.Empty;
            string Subname = string.Empty;
            string SubCode = string.Empty;
            int TotalPages = 0;
            double g = 0;
            bool once = false;
            string sem1 = string.Empty;
            string sem2 = string.Empty;
            string sem3 = string.Empty;
            int l = 0;
            bool optionflag = false;
            string oldsem = string.Empty;
            string getmnth = string.Empty;
            string getyear = string.Empty;
            string grademas = string.Empty;
            double grpoints = 0;
            string concat = string.Empty;
            string dept_name = string.Empty;
            string strExam_month = string.Empty;
            int subjcnt = 0;
            string strcourse = string.Empty;
            int chkstud_selected_cnt = 0;
            string failgrade = string.Empty;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            FpMarkSheet.Sheets[0].AutoPostBack = false;
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            string mode = string.Empty;
            if (current_sem != "")
            {
                semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
            }
            if ((exam_month != "") && (exam_year != "") && (exam_month != "0") && (exam_year != "0"))
            {
                syllabuscode = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
                IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + exam_month.ToString() + "' and exam_year= '" + exam_year.ToString() + "'"));
                if (IntExamCode != 0)
                {
                    lblnorec.Visible = false;
                    FpExternal.Visible = true;
                    btnxl.Visible = true;//added by srinath 24/5/2014
                    lblxl.Visible = true;
                    txtxlname.Visible = true;
                    int sel;
                    FpExternal.SaveChanges();
                    string chkrollno = string.Empty;
                    string temprollno = string.Empty;
                    string chkRegNo = string.Empty;
                    string chkname = string.Empty;
                    string tempRegNo = string.Empty;
                    string tempname = string.Empty;
                    //for (j = 0; j <= FpExternal.Sheets[0].RowCount - 5; j++) //hided by gowtham
                    for (j = 0; j <= FpExternal.Sheets[0].RowCount - 1; j++)
                    {
                        //sel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(j, 1).ToString());
                        sel = Convert.ToInt32(FpExternal.Sheets[0].Cells[j, 1].Value);
                        if (sel == 1)
                        {
                            chkstud_selected_cnt += 1;
                            pval += 1;
                            chkRegNo = FpExternal.Sheets[0].Cells[j, 3].Text;
                            chkname = FpExternal.Sheets[0].Cells[j, 4].Text;
                            chkrollno = FpExternal.Sheets[0].Cells[j, 2].Tag.ToString();
                            if (temprollno == "")
                            {
                                temprollno = chkrollno;
                            }
                            else
                            {
                                temprollno = temprollno + "," + chkrollno;
                            }
                            if (tempRegNo == "")
                            {
                                tempRegNo = chkRegNo;
                            }
                            else
                            {
                                tempRegNo = tempRegNo + "," + chkRegNo;
                            }
                            if (tempname == "")
                            {
                                tempname = chkname;
                            }
                            else
                            {
                                tempname = tempname + "," + chkname;
                            }
                        }
                    }
                    int chkstudent_count = 0;
                    string[] split_temprollno = temprollno.Split(',');
                    string[] split_tempRegNo = tempRegNo.Split(',');
                    string[] split_tempname = tempname.Split(',');
                    if (chkstud_selected_cnt > 0)
                    {
                        for (i = 0; i <= split_temprollno.GetUpperBound(0); i++)
                        {
                            lblstudselect.Visible = false;
                            btnPrint.Visible = true;
                            lblError.Visible = false;
                            chkstudent_count += 1;
                            lblnorec.Visible = false;
                            subjcnt = 0;
                            no = 0;
                            string EarnedCredit = string.Empty;
                            grcredit1 = 0;
                            string[] studarr = new string[pval];
                            // Varsel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(i, 1).ToString());
                            Varsel = Convert.ToInt32(FpExternal.Sheets[0].Cells[i, 1].Value);
                            if (Varsel == 1)
                            {
                                TotalPages += 1;
                            }
                            RollNo = split_temprollno[i].ToString();
                            RegNo = split_tempRegNo[i].ToString();
                            name = split_tempname[i].ToString();
                            string strgetdetail = string.Empty;
                            strgetdetail = "select branch_code,registration.current_semester ,registration.degree_code,registration.mode as mode,sex,registration.batch_year,dob from registration,applyn where applyn.app_no=registration.App_no and Roll_no='" + RollNo + "'";
                            SqlCommand cmd_getdetail = new SqlCommand(strgetdetail, con_getdetail);
                            con_getdetail.Close();
                            con_getdetail.Open();
                            SqlDataReader dr_getdetail;
                            dr_getdetail = cmd_getdetail.ExecuteReader();
                            dr_getdetail.Read();
                            if (dr_getdetail.HasRows)
                            {
                                sem1 = dr_getdetail["current_semester"].ToString();
                                mode = dr_getdetail["mode"].ToString();
                                if (dr_getdetail["sex"].ToString() == "1")
                                {
                                    gender = "Female";
                                }
                                else
                                {
                                    gender = "Male";
                                }
                                if (dr_getdetail["batch_year"].ToString() != null)
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                                else
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                            }
                            string strdaters = string.Empty;
                            strdaters = "select exam_month,Exam_year,current_semester from exam_details where Exam_Code=" + IntExamCode + "";
                            ExamCode = IntExamCode;
                            SqlCommand cmd_daters = new SqlCommand(strdaters, con_daters);
                            con_daters.Close();
                            con_daters.Open();
                            SqlDataReader dr_daters;
                            dr_daters = cmd_daters.ExecuteReader();
                            dr_daters.Read();
                            if (dr_daters.HasRows)
                            {
                                mont = dr_daters["exam_month"].ToString();
                                yr = dr_daters["Exam_year"].ToString();
                                oldsem = dr_daters["current_semester"].ToString();
                                sem = dr_daters["current_semester"].ToString();
                                if (sem == "1")
                                    sem3 = "I";
                                else if (sem == "2")
                                    sem3 = "II";
                                else if (sem == "3")
                                    sem3 = "III";
                                else if (sem == "4")
                                    sem3 = "IV";
                                else if (sem == "5")
                                    sem3 = "V";
                                else if (sem == "6")
                                    sem3 = "VI";
                                else if (sem == "7")
                                    sem3 = "VII";
                                else if (sem == "8")
                                    sem3 = "VIII";
                                else if (sem == "9")
                                    sem3 = "IX";
                                else if (sem == "10")
                                    sem3 = "X";
                                //'-------------------
                            }
                            if (exam_month == "1")
                                strExam_month = "Jan";
                            else if (exam_month == "2")
                                strExam_month = "Feb";
                            else if (exam_month == "3")
                                strExam_month = "Mar";
                            else if (exam_month == "4")
                                strExam_month = "Apr";
                            else if (exam_month == "5")
                                strExam_month = "May";
                            else if (exam_month == "6")
                                strExam_month = "Jun";
                            else if (exam_month == "7")
                                strExam_month = "Jul";
                            else if (exam_month == "8")
                                strExam_month = "Aug";
                            else if (exam_month == "9")
                                strExam_month = "Sep";
                            else if (exam_month == "10")
                                strExam_month = "Oct";
                            else if (exam_month == "11")
                                strExam_month = "Nov";
                            else if (exam_month == "12")
                                strExam_month = "Dec";
                            int fffff = int.Parse(dr_getdetail["degree_code"].ToString());
                            strcourse = "select course_name,dept_name from course,department,degree where course.course_id=degree.course_id and degree.dept_code=department.dept_code and degree_code='" + dr_getdetail["degree_code"] + "'";
                            SqlCommand cmd_course = new SqlCommand(strcourse, con_course);
                            con_course.Close();
                            con_course.Open();
                            SqlDataReader dr_course;
                            dr_course = cmd_course.ExecuteReader();
                            dr_course.Read();
                            if (dr_course.HasRows)
                            {
                                if (txtGetDegree.Text == "")
                                {
                                    course = dr_course["course_name"].ToString();
                                }
                                else
                                {
                                    course = txtGetDegree.Text.Trim();
                                }
                                if (txtDepartment.Text == "")
                                {
                                    dept_name = dr_course["dept_name"].ToString();
                                }
                                else
                                {
                                    dept_name = txtDepartment.Text.Trim();
                                }
                                if (Chkbxcou.Checked == true)
                                {
                                    cou = 1;
                                }
                                else
                                {
                                    cou = 0;
                                }
                            }
                            // FpMarkSheet.Sheets[0].RowCount += 40;
                            FpMarkSheet.Sheets[0].RowCount += 40;
                            string collnamenew1 = string.Empty;
                            string address1 = string.Empty;
                            string address3 = string.Empty;
                            string address = string.Empty;
                            string cat = string.Empty;
                            string univ = string.Empty;
                            string aut = string.Empty;
                            string phno = string.Empty;
                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                            {
                                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,'-')as pincode,logo1 as logo,Category,university,phoneno from collinfo where college_code=" + Session["collegecode"] + "";
                                SqlCommand collegecmd = new SqlCommand(college, con);
                                SqlDataReader collegename;
                                con.Close();
                                con.Open();
                                collegename = collegecmd.ExecuteReader();
                                if (collegename.HasRows)
                                {
                                    while (collegename.Read())
                                    {
                                        string today = DateTime.Now.ToString("dd-MM-yyyy");
                                        collnamenew1 = collegename["collname"].ToString();
                                        address1 = collegename["address1"].ToString();
                                        address2 = collegename["address2"].ToString();
                                        phno = collegename["phoneno"].ToString();
                                        //phno = "Phone: " + collegename["phoneno"].ToString();
                                        address = address1 + "," + address2 + "-" + collegename["pincode"].ToString() + " , Phone:" + phno.ToString();
                                        //address = address1 + "," + address2 + "-" + collegename["pincode"].ToString(); 
                                        cat = collegename["Category"].ToString();
                                        univ = collegename["university"].ToString();
                                    }
                                }
                            }
                            //Header start============================================================================================================
                            if (rad_header.Checked == true)
                            {
                                MyImg mi3 = new MyImg();
                                mi3.ImageUrl = "Handler/Handler2.ashx?";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 2, 1);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].CellType = mi3; //added aruna 11oct2012
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 2].Font.Size = 18;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 2].Font.Bold = true;
                                FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 40].Border.BorderColorBottom = Color.Black;
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 2, 1, 7);
                                //FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Height = 155;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 40].Border.BorderColorBottom = Color.White;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 2].Text = collnamenew1.ToString(); //added aruna 11oct2012
                                if (cat.ToString() == "Autonomous")
                                {
                                    aut = "(An Autonomous Institution, Affiliated to " + univ + ")";
                                    FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                                    FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 2, 1, 7);
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 2].HorizontalAlign = HorizontalAlign.Center;
                                    FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.White;
                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 2].Text = aut.ToString(); //added aruna 11oct2012
                                }
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 38, 2].Font.Size = 12;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 38, 2].Font.Bold = true;
                                FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 38].Border.BorderColorBottom = Color.Black;
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 38, 2, 1, 7);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 38, 2].HorizontalAlign = HorizontalAlign.Center;
                                FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 38].Border.BorderColorBottom = Color.White;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 38, 2].Text = address.ToString(); //added aruna 11oct2012
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 37, 4, 1, 4);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 37, 4].HorizontalAlign = HorizontalAlign.Right;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 37, 4].Text = "Date " + DateTime.Now.ToString("dd-MM-yyyy");
                                MyImg mi1 = new MyImg();
                                mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                            }
                            //End =========================================================================================================
                            //'----raja ------------------------------------------------------------
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                            if (Convert.ToInt32(exam_month) == 1)
                            {
                                month_new = "[dthp";
                            }
                            if (Convert.ToInt32(exam_month) == 2)
                            {
                                month_new = "gpg;uthp";
                            }
                            if (Convert.ToInt32(exam_month) == 3)
                            {
                                month_new = "khh;r;";
                            }
                            if (Convert.ToInt32(exam_month) == 4)
                            {
                                month_new = "Vg;uy;";
                            }
                            if (Convert.ToInt32(exam_month) == 5)
                            {
                                month_new = "Nk";
                            }
                            if (Convert.ToInt32(exam_month) == 6)
                            {
                                month_new = "[Pd;";
                            }
                            if (Convert.ToInt32(exam_month) == 7)
                            {
                                month_new = "[Piy";
                            }
                            if (Convert.ToInt32(exam_month) == 8)
                            {
                                month_new = "Mf];l;";
                            }
                            if (Convert.ToInt32(exam_month) == 9)
                            {
                                month_new = "nrg;lk;gh;";
                            }
                            if (Convert.ToInt32(exam_month) == 10)
                            {
                                month_new = "mf;Nlhgh;";
                            }
                            if (Convert.ToInt32(exam_month) == 11)
                            {
                                month_new = "etk;gh;";
                            }
                            if (Convert.ToInt32(exam_month) == 12)
                            {
                                month_new = "brk;gh;";
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = exam_year + " Mk; Mz;L " + month_new + " khjk; ele;j gUt ,Wjpj;Njh;T KbT mwpf;if";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Font.Underline = true;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "vq;fs; fy;Yhhpapy; ";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].HorizontalAlign = HorizontalAlign.Right;
                            //FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 4);
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 3);
                            int dd = dept_name.Length;
                            if (30 < dd)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = course + "-" + dept_name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Font.Size = 8;
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = course + "-" + dept_name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Font.Size = 10;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Margin.Right = 15;
                            //FpMarkSheet.Sheets[0].
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].Text = Session["sem2"] + " Mk; ";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 6, 1, 4);
                            //****++++++++one+++++++*****///
                            //***+++++two++++*************//
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "gUtk; gbf;Fk; jq;fs;  ";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].HorizontalAlign = HorizontalAlign.Left;
                            if (gender == "Male")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = "kfd;";
                                hh = "kfd;";
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = "kfs;";
                                hh = "kfs;";
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 4, 1, 4);
                            int ggf = name.Length;
                            if (ggf <= 14)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Text = name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Font.Size = 12;
                            }
                            else if (15 <= ggf && 20 >= ggf)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Text = name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Font.Size = 12;
                            }
                            else if (21 <= ggf && 25 >= ggf)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Text = name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Font.Size = 11;
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Text = name;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 4].Font.Size = 9;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "(gjpT vz;";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 2);
                            int aax = RegNo.Length;
                            FarPoint.Web.Spread.TextCellType txtcel = new FarPoint.Web.Spread.TextCellType();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 2].CellType = txtcel;
                            if (aax < 7)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 2].Text = RegNo;
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 2].Text = RegNo;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 2].Font.Size = 10;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 2].Text = RegNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 2, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 4].Text = ")-d; " + exam_year + " Mk; Mz;L " + month_new;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 4].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 4].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 4, 1, 4);
                            //***+++++two++++*************//
                            //***+++++three++++*************//
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 7);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = " khjk; ele;j gUt ,Wjpj;Njh;T KbT mwpf;if nfhLf;fg;gl;Ls;sJ.";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].VerticalAlign = VerticalAlign.Middle;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                            //'--------------------------------------------set the heading for the columns--------
                            FpMarkSheet.Sheets[0].Columns[7].Visible = false;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "Credit Point";
                            //FpMarkSheet.Sheets[0].Columns[5].Width = 30;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade";
                            //FpMarkSheet.Sheets[0].Columns[6].Width = 35;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                            //FpMarkSheet.Sheets[0].Columns[7].Width = 70;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                            //FpMarkSheet.Sheets[0].Columns[8].Width = 35;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColor = Color.Black;
                            //'--------------------------------------------------------------------------------------
                            int p;
                            int gg = 0;
                            string getmark = string.Empty;
                            string getsem = string.Empty;
                            string getsubno = string.Empty;
                            string getsubname = string.Empty;
                            string getsubcode = string.Empty;
                            string getresult = string.Empty;
                            string Enrolledcredit = string.Empty;
                            int count = FpMarkSheet.Sheets[0].RowCount - 29;
                            //'-----------------------------------query for select the subject name and details
                            strexam = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "' order by semester desc,subject_type desc,subject.subject_no asc";
                            SqlCommand cmd_exam = new SqlCommand(strexam, con_exam);
                            con_exam.Close();
                            con_exam.Open();
                            dr_exam = cmd_exam.ExecuteReader();
                            if (dr_exam.HasRows)
                            {
                                nos += 1;
                                p = 0;
                                int sub_val = 1;
                                while (dr_exam.Read())
                                {
                                    gg++;
                                    if (subjcnt > (15 * sub_val)) //if (subjcnt > (11 * sub_val)) 
                                    {
                                        sub_val = sub_val + 1;
                                        //FpMarkSheet.Sheets[0].Rows[count + subjcnt + 1].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 2].Text = "- - -Continued- - -";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                                        sections_str = ddlSec.SelectedValue.ToString();
                                        if (sections_str.ToString() == "All" || sections_str.ToString() == "" || sections_str.ToString() == "-1")
                                        {
                                            sections_str = string.Empty;
                                        }
                                        else
                                        {
                                            sections_str = " and sections='" + sections_str.ToString() + "'";
                                        }
                                        find_staff_code1 = "select top 1 class_advisor from semester_schedule where class_advisor<>'' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + "";
                                        da_find_staffcode1 = new SqlDataAdapter(find_staff_code1, con);
                                        con.Close();
                                        con.Open();
                                        ds_find_staffcode1 = new DataSet();
                                        da_find_staffcode1.Fill(ds_find_staffcode1);
                                        if (ds_find_staffcode1.Tables[0].Rows.Count > 0)
                                        {
                                            string[] spl_classadv_code1 = (ds_find_staffcode1.Tables[0].Rows[0]["class_advisor"].ToString()).Split(',');
                                            Session["class_adv_staffcode"] = spl_classadv_code1[0].ToString();
                                            MyImg coeimg2 = new MyImg();
                                            coeimg2.ImageUrl = "Handler/Class_Advisor.ashx?id=" + Session["class_adv_staffcode"];
                                            //=============class advisor sign
                                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 0, 2, 2);
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 0].CellType = coeimg2;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 0].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        else
                                        {
                                            Session["class_adv_staffcode"] = string.Empty;
                                        }
                                        //======================hod sign
                                        con.Close();
                                        con.Open();
                                        //find_staff_codex = "select top 1 staff_code from staffmaster where  staff_name=(select head_of_dept from department where dept_code in (select distinct dept_code from department where dept_name='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                                        find_staff_codex = "select top 1 staff_code from staffmaster where  staff_code=(select top 1 head_of_dept from department where dept_code in (select distinct dept_code from degree where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                                        // find_staff_codex = "select top 1 staff_code from staffmaster where  staff_code=(select head_of_dept from department where dept_code in (select distinct dept_code from department where dept_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                                        da_find_staffcodex = new SqlDataAdapter(find_staff_codex, con);
                                        ds_find_staffcodex = new DataSet();
                                        da_find_staffcodex.Fill(ds_find_staffcodex);
                                        if (ds_find_staffcodex.Tables[0].Rows.Count > 0)
                                        {
                                            Session["class_hod_staffcode"] = ds_find_staffcodex.Tables[0].Rows[0]["staff_code"].ToString();
                                            MyImg coeimg3 = new MyImg();
                                            coeimg3.ImageUrl = "Handler/Hod.ashx?id=" + Session["class_hod_staffcode"];
                                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 3, 2, 2);
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 3].CellType = coeimg3;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 3].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        else
                                        {
                                            Session["class_hod_staffcode"] = string.Empty;
                                        }
                                        //=========================================
                                        Session["college_coe"] = "True";
                                        coe1 = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 5, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].Text = coe1;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 5, 2, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].CellType = coeimg1;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].HorizontalAlign = HorizontalAlign.Left;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Text = "nghWg;ghrphpau;";  //class advisor
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Font.Name = "SunTommy";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Text = "Jiwj;jiytu;";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Font.Name = "SunTommy";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 5, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Text = "Kjy;th;";  //principal
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Font.Name = "SunTommy";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].HorizontalAlign = HorizontalAlign.Left;
                                        // grade value and mark
                                        DataTable dtvals = new DataTable();
                                        con.Close();
                                        con.Open();
                                        string gradedetailsss = "select * from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' order by frange";
                                        SqlDataAdapter sqldappps = new SqlDataAdapter(gradedetailsss, con);
                                        sqldappps.Fill(dtvals);
                                        FarPoint.Web.Spread.TextCellType objlabels = new FarPoint.Web.Spread.TextCellType();
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].CellType = objlabels;
                                        if (dtvals.Rows.Count > 0)
                                        {
                                            string markrange = "Mark Range & Letter Grade:      ";
                                            string lettergrade = "Letter Grade & Grade Points:     ";
                                            for (int ss = 0; ss < dtvals.Rows.Count; ss++)
                                            {
                                                markrange = string.Empty;
                                                if (ss == 0)
                                                {
                                                    markrange = "Mark Range & Letter Grade:      ";
                                                }
                                                string frange = Convert.ToString(dtvals.Rows[ss]["frange"]);
                                                string trange = Convert.ToString(dtvals.Rows[ss]["trange"]);
                                                string mark_grade = Convert.ToString(dtvals.Rows[ss]["Mark_Grade"]);
                                                string credits = Convert.ToString(dtvals.Rows[ss]["credit_points"]);
                                                if (frange == "0")
                                                {
                                                    markrange = markrange.PadRight(5) + "        <" + trange + "" + " : " + "   " + mark_grade + " " + "";
                                                    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString();
                                                    // lettergrade = lettergrade + " " + mark_grade;
                                                }
                                                else
                                                {
                                                    if (FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text == "")
                                                    {
                                                        markrange = markrange.ToString();
                                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString();
                                                    }
                                                    else
                                                    {
                                                        markrange = markrange.PadRight(5) + "    " + frange + " - " + trange + " : " + " " + mark_grade + " " + "";
                                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text + markrange.ToString();
                                                    }
                                                    //lettergrade = lettergrade + " " + mark_grade;
                                                }
                                            }
                                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 3, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                            //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString(); //Grade
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Font.Bold = true;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
                                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 2, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Text = "AAA-ABSENT   WH-WITHHELD";
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
                                            string allsyllcode = string.Empty;
                                            DataTable dtsyllcodes = new DataTable();
                                            string previoussyllacodes = "select syll_code from syllabus_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester < '" + ddlSemYr.SelectedItem.Text + "' ";
                                            con.Close();
                                            con.Open();
                                            SqlDataAdapter sqlds = new SqlDataAdapter(previoussyllacodes, con);
                                            sqlds.Fill(dtsyllcodes);
                                            if (dtsyllcodes.Rows.Count > 0)
                                            {
                                                for (int sc = 0; sc < dtsyllcodes.Rows.Count; sc++)
                                                {
                                                    if (allsyllcode == "")
                                                    {
                                                        allsyllcode = dtsyllcodes.Rows[sc]["syll_code"].ToString();
                                                    }
                                                    else
                                                    {
                                                        allsyllcode = allsyllcode + "," + dtsyllcodes.Rows[sc]["syll_code"].ToString();
                                                    }
                                                }
                                            }
                                            DataTable dsparrears = new DataTable();
                                            SqlDataAdapter sqds;
                                            if (allsyllcode != "")
                                            {
                                                string previousarrears = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + RollNo + "') and roll_no ='" + RollNo + "'  and subject.syll_code in (" + allsyllcode.ToString() + ") )";
                                                con.Close();
                                                con.Open();
                                                sqds = new SqlDataAdapter(previousarrears, con);
                                                sqds.Fill(dsparrears);
                                            }
                                            DAccess2 da = new DAccess2();
                                            bool checkfailstatus = false;
                                            string cgpa = string.Empty;
                                            string allgpa = string.Empty;
                                            string checkresult = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + RollNo + "') and roll_no ='" + RollNo + "'  and subject.syll_code=" + syllabuscode.ToString() + " )";
                                            DataSet dscheckresults = da.select_method(checkresult, hat, "Text");
                                            if (dscheckresults.Tables[0].Rows.Count > 0)
                                            {
                                                // cgpa = "-";
                                                // checkfailstatus = true;
                                                allgpa = "-";
                                            }
                                            else
                                            {
                                                /// cgpa = da.Calculete_CGPA(RollNo, Convert.ToString(ddlSemYr.SelectedItem.Text), degree_code, batch_year, mode, collegecode);
                                                allgpa = da.Calulat_GPA_Semwise(RollNo, degree_code, batch_year, exam_month, exam_year, collegecode);
                                            }
                                            if (dsparrears.Rows.Count > 0)
                                            {
                                                cgpa = "-";
                                            }
                                            if (dscheckresults.Tables[0].Rows.Count > 0 && dsparrears.Rows.Count > 0)
                                            {
                                                allgpa = "-";
                                                cgpa = "-";
                                            }
                                            if (dscheckresults.Tables[0].Rows.Count == 0 && dsparrears.Rows.Count == 0)
                                            {
                                                cgpa = da.Calculete_CGPA(RollNo, Convert.ToString(ddlSemYr.SelectedItem.Text), degree_code, batch_year, mode, collegecode);
                                            }
                                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 1, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Text = sem3 + " Sem Gpa:" + allgpa + "  UPTO " + sem3 + "Sem cgpa" + cgpa;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                                        }
                                        //
                                        FpMarkSheet.Sheets[0].RowCount += 40;
                                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                        {
                                            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                            SqlCommand collegecmd = new SqlCommand(college, con);
                                            SqlDataReader collegename;
                                            con.Close();
                                            con.Open();
                                            collegename = collegecmd.ExecuteReader();
                                            if (collegename.HasRows)
                                            {
                                                while (collegename.Read())
                                                {
                                                    collnamenew1 = collegename["collname"].ToString();
                                                    address1 = collegename["address1"].ToString();
                                                    address3 = collegename["address3"].ToString();
                                                    address = address1 + "," + address3;
                                                }
                                            }
                                        }
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Border.BorderColorRight = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------set the heading for the columns--------
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "Credit Point";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                                        //FpMarkSheet.Sheets[0].Columns[5].Width = 30;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "Grade/Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                                        //FpMarkSheet.Sheets[0].Columns[6].Width = 35;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "GradePoint";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //FpMarkSheet.Sheets[0].Columns[7].Width = 70;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                                        //FpMarkSheet.Sheets[0].Columns[8].Width = 35;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                                        count = FpMarkSheet.Sheets[0].RowCount - 29;
                                        subjcnt = 0;
                                    }
                                    subjcnt += 1;
                                    maxsem = GetFunction("Select max(semester) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "'");
                                    p += 1;
                                    Subno = dr_exam["subject_no"].ToString();
                                    getsem = dr_exam["semester"].ToString();
                                    getresult = dr_exam["result"].ToString();
                                    getsubno = dr_exam["subject_no"].ToString();
                                    getsubcode = dr_exam["subject_code"].ToString();
                                    getsubname = dr_exam["subject_name"].ToString();
                                    mark = dr_exam["total"].ToString();
                                    if (dr_exam["grade"].ToString() != "")
                                    {
                                        grade = dr_exam["grade"].ToString();
                                        Session["grade_new"] = grade;
                                        credit = dr_exam["cp"].ToString();
                                    }
                                    else
                                    {
                                        string strsecrs = "select linkvalue from inssettings where linkname='Corresponding Grade' and college_code=" + Session["collegecode"] + "";
                                        SqlCommand cmd_secrs = new SqlCommand(strsecrs, con_secrs);
                                        con_secrs.Close();
                                        con_secrs.Open();
                                        SqlDataReader dr_secrs;
                                        dr_secrs = cmd_secrs.ExecuteReader();
                                        dr_secrs.Read();
                                        if (dr_secrs["linkvalue"].ToString() == "0")
                                        {
                                            string strnew = string.Empty;
                                            strnew = " select * from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + "";
                                            SqlCommand cmd_new = new SqlCommand(strnew, con_new);
                                            con_new.Close();
                                            con_new.Open();
                                            SqlDataReader dr_new;
                                            dr_new = cmd_new.ExecuteReader();
                                            dr_new.Read();
                                            if (dr_new.HasRows == true)
                                            {
                                                flag = true;
                                                grade = Session["grade_new"].ToString();
                                                if (mark != "")
                                                {
                                                    getmark = mark;
                                                    markflag = true;
                                                }
                                                else
                                                {
                                                    mark = "'" + " " + "'";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            getmark = mark;
                                            markflag = true;
                                            con_new.Close();
                                            con_new.Open();
                                            string query_new = "Select mark_entry.*,maxtotal,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + IntExamCode + " and Attempts =1 and roll_no='" + RollNo + "' and  Mark_Entry.subject_no='" + getsubno + "' order by subject_type desc,mark_entry.subject_no";
                                            SqlCommand com_new = new SqlCommand(query_new, con_new);
                                            SqlDataReader drmrkentry = com_new.ExecuteReader();
                                            drmrkentry.Read();
                                            if (drmrkentry.HasRows == true)
                                            {
                                                if (dr_secrs["linkvalue"].ToString() == "1")
                                                {
                                                    //}
                                                    //else
                                                    //{
                                                    if (drmrkentry["internal_mark"].ToString() != " " && drmrkentry["internal_mark"].ToString() != "" && drmrkentry["External_mark"].ToString() != " " && drmrkentry["External_mark"].ToString() != "") //added condition on 17.07.12
                                                    {
                                                        if (Convert.ToDouble(drmrkentry["internal_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_int_marks"].ToString()) && Convert.ToDouble(drmrkentry["External_mark"].ToString()) >= Convert.ToDouble(drmrkentry["min_ext_marks"].ToString()))
                                                        {
                                                            convertgrade(RollNo, getsubno);
                                                            result = "Pass";
                                                            grade = funcgrade.ToString();
                                                        }
                                                        else
                                                        {
                                                            //=====new 09.07.12 by ,mytthili
                                                            con.Close();
                                                            con.Open();
                                                            SqlCommand cmd_failgrade = new SqlCommand("select value from COE_Master_Settings where settings='Fail Grade'", con);
                                                            SqlDataReader dr_failgrade;//= new SqlDataReader(cmd_failgrade);
                                                            dr_failgrade = cmd_failgrade.ExecuteReader();
                                                            if (dr_failgrade.HasRows == true)
                                                            {
                                                                if (dr_failgrade.Read())
                                                                {
                                                                    if (dr_failgrade["value"].ToString() != "")
                                                                    {
                                                                        failgrade = dr_failgrade["value"].ToString();
                                                                    }
                                                                }
                                                            }
                                                            else
                                                            {
                                                                failgrade = "-";
                                                            }
                                                            //===============09.07.12 by mythili
                                                            funcgrade = "RA";
                                                            result = "Fail";
                                                            grade = failgrade.ToString();
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                grade = "-";
                                                result = "-";
                                            }
                                        }
                                    }
                                    if (grade != "")
                                    {
                                        grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  Mark_Grade='" + grade.ToString() + "'";
                                        SqlCommand cmd_grademas = new SqlCommand(grademas, con_grademas);
                                        con_grademas.Close();
                                        con_grademas.Open();
                                        SqlDataReader dr_grademas;
                                        dr_grademas = cmd_grademas.ExecuteReader();
                                        while (dr_grademas.Read())
                                        {
                                            grpoints = Convert.ToDouble(dr_grademas["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                    string strcredit = string.Empty;
                                    strcredit = "select credit_points from subject where subject_no= " + Subno + " ";
                                    SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                                    con_credit.Close();
                                    con_credit.Open();
                                    SqlDataReader dr_credit;
                                    dr_credit = cmd_credit.ExecuteReader();
                                    dr_credit.Read();
                                    if (dr_credit.HasRows == true)
                                    {
                                        grcredit = dr_credit["credit_points"].ToString();
                                        if (grcredit != "")
                                        {
                                            grcredit1 = grcredit1 + Convert.ToDouble(grcredit);
                                        }
                                        else
                                        {
                                            gcheck = 1;
                                            lblError.Text = "Kindly set credit point for all subject";
                                            lblError.Visible = true;
                                            return;
                                        }
                                        credit = grcredit;
                                    }
                                    else
                                    {
                                        grcredit = "0";
                                    }
                                    gpa = grpoints * Convert.ToDouble(grcredit);
                                    gpa1 = gpa1 + gpa;
                                    if (grcredit1 > 0)
                                        g = gpa1 / grcredit1;
                                    else
                                        g = 0;
                                    if (getsem == "")
                                    {
                                        sem = string.Empty;
                                    }
                                    else
                                    {
                                        sem = getsem;
                                    }
                                    //string tformat =string.Empty;
                                    //string tattr =string.Empty;
                                    //string trowrtf =string.Empty;
                                    if (sem != oldsem)
                                    {
                                        if (once == false)
                                        {
                                            string remark = string.Empty;
                                            if (optionflag == true)
                                            {
                                                string stroption = string.Empty;
                                                stroption = "select distinct uncompulsory_subject.subject_no,subject_name,subject_code,remarks from uncompulsory_subject,subject where uncompulsory_subject.subject_no=subject.subject_no and degree_code=" + degree_code + " and semester=" + current_sem + " and batch_year=" + batch_year + " and roll_no='" + RollNo + "' order by subject_code asc";
                                                con_option.Close();
                                                con_option.Open();
                                                SqlCommand cmd_option = new SqlCommand(stroption, con_option);
                                                SqlDataReader dr_option;
                                                dr_option = cmd_option.ExecuteReader();
                                                while (dr_option.Read())
                                                {
                                                    if (dr_option.HasRows)
                                                    {
                                                        if (dr_option["subject_name"].ToString() != "")
                                                        {
                                                            Subname = dr_option["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subname = string.Empty;
                                                        }
                                                        if (dr_option["subject_code"].ToString() != "")
                                                        {
                                                            SubCode = dr_option["subject_code"].ToString();
                                                        }
                                                        else
                                                        {
                                                            SubCode = string.Empty;
                                                        }
                                                        if (dr_option["subject_no"].ToString() != "")
                                                        {
                                                            Subno = dr_option["subject_no"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subno = string.Empty;
                                                        }
                                                        if (dr_option["remarks"].ToString() != "")
                                                        {
                                                            remark = dr_option["remarks"].ToString();
                                                        }
                                                        else
                                                        {
                                                            remark = string.Empty;
                                                        }
                                                        sem = GetSemester_AsNumber(Convert.ToInt32(current_sem)).ToString();
                                                        if (sem == "1")
                                                            sem2 = "I";
                                                        else if (sem == "2")
                                                            sem2 = "II";
                                                        else if (sem == "3")
                                                            sem2 = "III";
                                                        else if (sem == "4")
                                                            sem2 = "IV";
                                                        else if (sem == "5")
                                                            sem2 = "V";
                                                        else if (sem == "6")
                                                            sem2 = "VI";
                                                        else if (sem == "7")
                                                            sem2 = "VII";
                                                        else if (sem == "8")
                                                            sem2 = "VIII";
                                                        else if (sem == "9")
                                                            sem2 = "IX";
                                                        else if (sem == "10")
                                                            sem2 = "X";
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        if (Chkbxcou.Checked == false)
                                                        {
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                        }
                                                        else
                                                        {
                                                            SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        once = true;
                                    }
                                    if (getsubname != "")
                                    {
                                        Subname = getsubname;
                                    }
                                    else
                                    {
                                        Subname = string.Empty;
                                    }
                                    if (getsubcode != "")
                                    {
                                        SubCode = getsubcode;
                                    }
                                    else
                                    {
                                        SubCode = string.Empty;
                                    }
                                    if (getsubno != "")
                                    {
                                        Subno = getsubno;
                                    }
                                    else
                                    {
                                        Subno = string.Empty;
                                    }
                                    if (getresult != "")
                                    {
                                        result = getresult;
                                    }
                                    else
                                    {
                                        result = string.Empty;
                                    }
                                    if (getsem != "")
                                    {
                                        current_sem = getsem;
                                    }
                                    else
                                    {
                                        current_sem = string.Empty;
                                    }
                                    if (sem == "1")
                                        sem2 = "I";
                                    else if (sem == "2")
                                        sem2 = "II";
                                    else if (sem == "3")
                                        sem2 = "III";
                                    else if (sem == "4")
                                        sem2 = "IV";
                                    else if (sem == "5")
                                        sem2 = "V";
                                    else if (sem == "6")
                                        sem2 = "VI";
                                    else if (sem == "7")
                                        sem2 = "VII";
                                    else if (sem == "8")
                                        sem2 = "VIII";
                                    else if (sem == "9")
                                        sem2 = "IX";
                                    else if (sem == "10")
                                        sem2 = "X";
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    if (Chkbxcou.Checked == false)
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                    }
                                    else
                                    {
                                        SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                    }
                                    if (credit == "0")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = credit;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (markflag == true)
                                    {
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = mark;//old
                                    }
                                    else
                                    {
                                        if (result == "Pass" || result == "pass")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = grpoints.ToString();
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        //SANKAR ADDED July15'2013
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = result;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "SA" || result == "sa")
                                    {
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "NS" || result == "ns")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "NS";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "AAA" || result == "aaa")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "Absent";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "AAA";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == " " || result == "")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        if (credit == "0")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "Fail";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            //SANKAR added..............
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    //FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorRight = Color.Black;
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColor = Color.Black;
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorLeft = Color.Black;
                                    //@@@@@@@@@@@@@ 17.07.12
                                    grade = string.Empty;
                                    result = string.Empty;
                                }
                                FpMarkSheet.SaveChanges();
                                int gp = gg;
                                for (int t = 0; t < gg; t++)
                                {
                                    gp--;
                                    string n = FpMarkSheet.Sheets[0].Cells[count + subjcnt - gp, 8].Text;
                                    if (n == "Pass")
                                    {
                                        r = r + 1;
                                    }
                                    else
                                    {
                                        r1 = r1 + 1;
                                    }
                                }
                            }
                            string bc = string.Empty;
                            if (r1 == 0)
                            {
                                bc = "ed;W";
                            }
                            if (r1 == 1)
                            {
                                bc = "gpd;jq;fpAs;shH> bA+l;liu re;jpf;fTk;";
                            }
                            if (r1 == 2)
                            {
                                bc = ": gpd;jq;fpAs;shH> bA+l;lH kw;Wk; Jiwj;jiytiu re;jpf;fTk;";
                            }
                            if (3 <= r1)
                            {
                                bc = "gpd;jq;fpAs;shH> bA+l;lH> Jiwj;jiytH kw;Wk; Kjy;tiu re;jpf;fTk;";
                            }
                            //FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 17].Border.BorderColorBottom = Color.Black;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 13, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 13, 0].HorizontalAlign = HorizontalAlign.Center;
                            //********************
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 12, 0, 1, 7);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 12, 0].Text = "      Njh;T Kd;Ndw;w mwpf;if ngw;wikf;F xg;Gif mDg;Gf.";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 12, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 11, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 11, 0].Text = "gpd;Fwpg;G :-";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 11, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 0].Text = bc;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 0].Font.Name = "SunTommy";
                            //********************
                            string strfoot = "fy;Yhhp kw;Wk; tpLjp tshfj;jpy; khzt / khztpah;fs; ifNgrp gad;gLj;j jil nra;ag;gl;Ls;sJ.";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = strfoot;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Font.Name = "SunTommy";
                            //
                            sections_str = ddlSec.SelectedValue.ToString();
                            if (sections_str.ToString() == "All" || sections_str.ToString() == "" || sections_str.ToString() == "-1")
                            {
                                sections_str = string.Empty;
                            }
                            else
                            {
                                sections_str = " and sections='" + sections_str.ToString() + "'";
                            }
                            find_staff_code1 = "select top 1 class_advisor from semester_schedule where class_advisor<>'' and degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue.ToString() + " and semester=" + ddlSemYr.SelectedValue.ToString() + "";
                            da_find_staffcode1 = new SqlDataAdapter(find_staff_code1, con);
                            con.Close();
                            con.Open();
                            ds_find_staffcode1 = new DataSet();
                            da_find_staffcode1.Fill(ds_find_staffcode1);
                            if (ds_find_staffcode1.Tables[0].Rows.Count > 0)
                            {
                                string[] spl_classadv_code1 = (ds_find_staffcode1.Tables[0].Rows[0]["class_advisor"].ToString()).Split(',');
                                Session["class_adv_staffcode"] = spl_classadv_code1[0].ToString();
                                MyImg coeimg2 = new MyImg();
                                coeimg2.ImageUrl = "Handler/Class_Advisor.ashx?id=" + Session["class_adv_staffcode"];
                                //=============class advisor sign
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 0, 2, 2);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 0].CellType = coeimg2;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 0].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                Session["class_adv_staffcode"] = string.Empty;
                            }
                            //======================hod sign
                            con.Close();
                            con.Open();
                            //modified by gowtham 28/jan/2014
                            // find_staff_codex = "select top 1 staff_code from staffmaster where  staff_name=(select head_of_dept from department where dept_code in (select distinct dept_code from department where dept_name='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                            //find_staff_codex = "select top 1 staff_code from staffmaster where  staff_code=(select head_of_dept from department where dept_code in (select distinct dept_code from department where dept_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                            find_staff_codex = "select top 1 staff_code from staffmaster where  staff_code=(select top 1 head_of_dept from department where dept_code in (select distinct dept_code from degree where degree_code='" + ddlBranch.SelectedValue.ToString() + "' and college_code='" + Session["collegecode"] + "'))";
                            da_find_staffcodex = new SqlDataAdapter(find_staff_codex, con);
                            ds_find_staffcodex = new DataSet();
                            da_find_staffcodex.Fill(ds_find_staffcodex);
                            if (ds_find_staffcodex.Tables[0].Rows.Count > 0)
                            {
                                Session["class_hod_staffcode"] = ds_find_staffcodex.Tables[0].Rows[0]["staff_code"].ToString();
                                MyImg coeimg3 = new MyImg();
                                coeimg3.ImageUrl = "Handler/Hod.ashx?id=" + Session["class_hod_staffcode"];
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 3, 2, 2);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 3].CellType = coeimg3;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 3].HorizontalAlign = HorizontalAlign.Left;
                            }
                            else
                            {
                                Session["class_hod_staffcode"] = string.Empty;
                            }
                            //=========================================
                            Session["college_coe"] = "True";
                            coe1 = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 5, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].Text = coe1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 7, 5, 2, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].CellType = coeimg1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 7, 5].HorizontalAlign = HorizontalAlign.Left;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Text = "nghWg;ghrphpau;";  //class advisor
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Text = "Jiwj;jiytu;";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 3].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 5, 5, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Text = "Kjy;th;";  //principal
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Font.Name = "SunTommy";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 5, 5].HorizontalAlign = HorizontalAlign.Left;
                            DateTime currentdate;
                            currentdate = System.DateTime.Now;
                            string[] split_currentdate = Convert.ToString(currentdate).Split(new char[] { ' ' });
                            string[] split_date = split_currentdate[0].Split(new char[] { '/' });
                            string concat_date = split_date[1].ToString() + '/' + split_date[0].ToString() + '/' + split_date[2].ToString();
                            if (radiobtn2.Checked == true)
                            {
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 3, 1, 1, 8);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 1].Text = "gUt ,Wjpj; Njh;T mwpf;ifapy; jq;fs;" + hh + " gjpT vz;  kl;Lk; jtWjyhf Fwpg;gplg;gl;lJ.";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 1].Font.Name = "SunTommy";
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 2, 0, 1, 8);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Text = " Mdhy; kjpg;ngz;zpy; ve;j khw;wKk ,y;iy. jw;;NghJ  rhpahd gjpT vz;Zld; ";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Font.Name = "SunTommy";
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 1, 0, 1, 8);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Text = "jpUk;gTk; gUt ,Wjpj; Njh;T mwpf;ifia mDg;gpAs;Nshk;.";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Font.Name = "SunTommy";
                            }
                            if (radiobtn1.Checked == true)
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 1].Text = string.Empty;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 1].Text = string.Empty;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 1].Text = string.Empty;
                            }
                            if (radiobtn1.Checked == false)
                            {
                                FpMarkSheet.Sheets[0].RowCount = FpMarkSheet.Sheets[0].RowCount + 3;
                            }
                            // grade value and mark
                            DataTable dtval = new DataTable();
                            con.Close();
                            con.Open();
                            string gradedetailss = "select * from grade_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' order by frange";
                            SqlDataAdapter sqldappp = new SqlDataAdapter(gradedetailss, con);
                            sqldappp.Fill(dtval);
                            FarPoint.Web.Spread.TextCellType objlabel = new FarPoint.Web.Spread.TextCellType();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].CellType = objlabel;
                            if (dtval.Rows.Count > 0)
                            {
                                string markrange = "Mark Range & Letter Grade:      ";
                                string lettergrade = "Letter Grade & Grade Points:     ";
                                for (int ss = 0; ss < dtval.Rows.Count; ss++)
                                {
                                    markrange = string.Empty;
                                    if (ss == 0)
                                    {
                                        markrange = "Mark Range & Letter Grade:      ";
                                    }
                                    string frange = Convert.ToString(dtval.Rows[ss]["frange"]);
                                    string trange = Convert.ToString(dtval.Rows[ss]["trange"]);
                                    string mark_grade = Convert.ToString(dtval.Rows[ss]["Mark_Grade"]);
                                    string credits = Convert.ToString(dtval.Rows[ss]["credit_points"]);
                                    if (frange == "0")
                                    {
                                        markrange = markrange.PadRight(5) + "        <" + trange + "" + " : " + "   " + mark_grade + " " + "";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString();
                                        // lettergrade = lettergrade + " " + mark_grade;
                                    }
                                    else
                                    {
                                        if (FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text == "")
                                        {
                                            markrange = markrange.ToString();
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString();
                                        }
                                        else
                                        {
                                            markrange = markrange.PadRight(5) + "    " + frange + " - " + trange + " : " + " " + mark_grade + " " + "";
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text + markrange.ToString();
                                        }
                                        //lettergrade = lettergrade + " " + mark_grade;
                                    }
                                }
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 3, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Text = markrange.ToString(); //Grade
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].Font.Bold = true;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 3, 0].HorizontalAlign = HorizontalAlign.Left;
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 2, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Text = "AAA-ABSENT   WH-WITHHELD";
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].Font.Bold = true;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 2, 0].HorizontalAlign = HorizontalAlign.Left;
                                string allsyllcode = string.Empty;
                                DataTable dtsyllcode = new DataTable();
                                string previoussyllacode = "select syll_code from syllabus_master where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and semester < '" + ddlSemYr.SelectedItem.Text + "' ";
                                con.Close();
                                con.Open();
                                SqlDataAdapter sqld = new SqlDataAdapter(previoussyllacode, con);
                                sqld.Fill(dtsyllcode);
                                if (dtsyllcode.Rows.Count > 0)
                                {
                                    for (int sc = 0; sc < dtsyllcode.Rows.Count; sc++)
                                    {
                                        if (allsyllcode == "")
                                        {
                                            allsyllcode = dtsyllcode.Rows[sc]["syll_code"].ToString();
                                        }
                                        else
                                        {
                                            allsyllcode = allsyllcode + "," + dtsyllcode.Rows[sc]["syll_code"].ToString();
                                        }
                                    }
                                }
                                DataTable dsparrear = new DataTable();
                                SqlDataAdapter sqd;
                                if (allsyllcode != "")
                                {
                                    string previousarrear = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + RollNo + "') and roll_no ='" + RollNo + "'  and subject.syll_code in (" + allsyllcode.ToString() + ") )";
                                    con.Close();
                                    con.Open();
                                    sqd = new SqlDataAdapter(previousarrear, con);
                                    sqd.Fill(dsparrear);
                                }
                                DAccess2 da = new DAccess2();
                                bool checkfailstatus = false;
                                string cgpa = string.Empty;
                                string allgpa = string.Empty;
                                string checkresult = "Select isnull(Subject_Code,'') as scode , isnull(subjecT_name,'') as sname , semester from subject,syllabus_master as smas where smas.syll_code = subject.syll_code and subject_no in (select distinct subject_no from mark_entry where subject_no not in (select distinct subject_no from mark_entry where passorfail=1 and result='Pass' and ltrim(rtrim(roll_no))='" + RollNo + "') and roll_no ='" + RollNo + "'  and subject.syll_code=" + syllabuscode.ToString() + " )";
                                DataSet dscheckresult = da.select_method(checkresult, hat, "Text");
                                if (dscheckresult.Tables[0].Rows.Count > 0)
                                {
                                    // cgpa = "-";
                                    // checkfailstatus = true;
                                    allgpa = "-";
                                }
                                else
                                {
                                    /// cgpa = da.Calculete_CGPA(RollNo, Convert.ToString(ddlSemYr.SelectedItem.Text), degree_code, batch_year, mode, collegecode);
                                    allgpa = da.Calulat_GPA_Semwise(RollNo, degree_code, batch_year, exam_month, exam_year, collegecode);
                                }
                                if (dsparrear.Rows.Count > 0)
                                {
                                    cgpa = "-";
                                }
                                if (dscheckresult.Tables[0].Rows.Count > 0 && dsparrear.Rows.Count > 0)
                                {
                                    allgpa = "-";
                                    cgpa = "-";
                                }
                                if (dscheckresult.Tables[0].Rows.Count == 0 && dsparrear.Rows.Count == 0)
                                {
                                    cgpa = da.Calculete_CGPA(RollNo, Convert.ToString(ddlSemYr.SelectedItem.Text), degree_code, batch_year, mode, collegecode);
                                }
                                FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 1, 0, 1, FpMarkSheet.Sheets[0].ColumnCount - 1);
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Text = sem3 + " Sem Gpa:" + allgpa + "  UPTO " + sem3 + "Sem cgpa" + cgpa;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].Font.Bold = true;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 1, 0].HorizontalAlign = HorizontalAlign.Left;
                            }
                            //
                            g = 0;
                            overallcredit = 0;
                            cgpa2 = 0;
                            gpa = 0;
                            gpa1 = 0;
                            g = 0;
                            grcredit = string.Empty;
                            markflag = false;
                            flag = false;
                            once = false;
                        }
                    }
                    else
                    {
                        lblError.Text = string.Empty;
                        lblError.Visible = false;
                        lblstudselect.Visible = true;
                        lblstudselect.Text = "Please Select Atleast One Student To Print The GradeSheet";
                        btnPrint.Visible = false;
                    }
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpMarkSheet.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpMarkSheet.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    FpMarkSheet.SaveChanges();
                    if (FpMarkSheet.Sheets[0].RowCount > 0)
                    {
                        FpMarkSheet.Sheets[0].PageSize = FpMarkSheet.Sheets[0].RowCount;
                    }
                }
                else
                {
                    FpExternal.Visible = false;
                    btnxl.Visible = false;//added by srinath 24/5/2014
                    lblxl.Visible = false;
                    txtxlname.Visible = false;
                    lblnorec.Visible = true;
                    Buttontotal.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    private string Calulat_GPA(string RollNo, string sem)
    {
        double finalgpa1 = 0;
        try
        {
            //int Subno = 0;
            int jvalue = 0;
            string examcodeval = string.Empty;
            string gradestr = string.Empty;
            string ccva = string.Empty;
            string strgrade = string.Empty;
            //  int creditval=0;
            double creditval = 0;
            double creditsum1 = 0;
            //  int creditsum1=0;
            double gpacal1 = 0;
            string strsubcrd = string.Empty;
            string graders = string.Empty;
            examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
            ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
            if (ccva == "False")
            {
                //attempts=1 marks
                strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
            }
            else if (ccva == "True")
            {
                if (ChkOutgone.Checked == true)
                {
                    strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
                }
                else
                {
                    strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                }
            }
            SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
            con_subcrd.Close();
            con_subcrd.Open();
            SqlDataReader dr_subcrd;
            dr_subcrd = cmd_subcrd.ExecuteReader();
            DataSet dggradetot = new DataSet();
            dggradetot = d2.select_method_wo_parameter("select distinct frange,trange,credit_points,mark_grade  from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + collegecode + "", "Text");

            while (dr_subcrd.Read())
            {
                if (dr_subcrd.HasRows)
                {
                    //and " +dr_subcrd["total"].ToString()+ "
                    if ((dr_subcrd["total"].ToString() != string.Empty))
                    {
                        graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_subcrd["total"].ToString() + " between frange and trange";
                    }
                    else if (!string.IsNullOrEmpty(Convert.ToString(dr_subcrd["credit_points"])))
                    {
                        graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and  " + dr_subcrd["credit_points"].ToString() + " between frange and trange";
                    }
                    cmd = new SqlCommand(graders, con_Grade);
                    con_Grade.Close();
                    con_Grade.Open();
                    SqlDataReader dr_grades;
                    dr_grades = cmd.ExecuteReader();
                    //while (dr_grades.Read())
                    //{
                    if (dr_grades.Read())
                    {
                        if (dr_grades.HasRows)
                        {
                            strgrade = dr_grades["credit_points"].ToString();
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
                        //if (strgrade == "0")
                        //{
                        //    if (dggradetot != null && dggradetot.Tables[0] != null && dggradetot.Tables[0].Rows.Count > 0)
                        //    {
                        //       string strtotgrac = Convert.ToString(dr_subcrd["grade"].ToString());

                        //    }
                        //}
                        if (gpacal1 == 0)
                        {
                            gpacal1 = Convert.ToDouble(strgrade) * creditval;
                        }
                        else
                        {
                            gpacal1 = gpacal1 + (Convert.ToDouble(strgrade) * creditval);
                        }
                    }
                    //}
                }
            }
            if (creditsum1 != 0)
            {
                finalgpa1 = Math.Round((gpacal1 / creditsum1), 2);
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return finalgpa1.ToString();
    }

    private string Calculete_CGPA(string RollNo, string semval)
    {
        string calculate = string.Empty;
        try
        {
            //int Subno = 0;
            int jvalue = 0;
            string examcodeval = string.Empty;
            string gradestr = string.Empty;
            string ccva = string.Empty;
            string strgrade = string.Empty;
            //  int creditval = 0;
            double creditval = 0;
            double finalgpa1 = 0;
            double creditsum1 = 0;
            //  int creditsum1 = 0;
            double gpacal1 = 0;
            int se = 0;
            string latsem = string.Empty;
            string syll_code = string.Empty;
            string strsubcrd = string.Empty;
            string latmode = string.Empty;
            for (jvalue = 1; jvalue <= Convert.ToInt32(semval); jvalue++)
            {
                syll_code = GetFunction("select distinct syll_code from syllabus_master where degree_code=" + degree_code + " and semester =" + jvalue + " and batch_year=" + batch_year + "");
                examcodeval = GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
                if (syll_code != "")
                {
                    if (jvalue == Convert.ToInt32(semval))
                    {
                        ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
                        if (ccva == "False")
                        {
                            strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                        }
                        else if (ccva == "True")
                        {
                            if (ChkOutgone.Checked == true)
                            {
                                //strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
                                strsubcrd = "Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
                            }
                            else
                            {
                                strsubcrd = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts=1";
                            }
                        }
                    }//'''''''
                    else
                    {
                        strsubcrd = "Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass')and exam_code in (select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and current_semester<=" + semdec + ")";
                    }
                }
                SqlCommand cmd_subcrd = new SqlCommand(strsubcrd, con_subcrd);
                con_subcrd.Close();
                con_subcrd.Open();
                SqlDataReader dr_subcrd;
                dr_subcrd = cmd_subcrd.ExecuteReader();
                while (dr_subcrd.Read())
                {
                    if (dr_subcrd.HasRows)
                    {
                        if ((dr_subcrd["total"].ToString() != "NULL") && (dr_subcrd["total"].ToString() != string.Empty))
                        {
                            string graders = "select distinct credit_points from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + dr_subcrd["total"].ToString() + " between frange and trange";
                            cmd = new SqlCommand(graders, con_Grade);
                            con_Grade.Close();
                            con_Grade.Open();
                            SqlDataReader dr_grades;
                            dr_grades = cmd.ExecuteReader();
                            dr_grades.Read();
                            if (dr_grades.HasRows)
                            {
                                strgrade = dr_grades["credit_points"].ToString();
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
                        if (strgrade == "0")
                            strgrade = Convert.ToString(dr_subcrd["credit_points"]);
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
                if (creditsum1 != 0)
                {
                    if (finalgpa1 == 0)
                    {
                        finalgpa1 = Math.Round((gpacal1 / creditsum1), 2);
                    }
                    else
                    {
                        finalgpa1 = finalgpa1 + Math.Round((gpacal1 / creditsum1), 2);
                    }
                }
                creditsum1 = 0;
                gpacal1 = 0;
                creditval = 0;
                strgrade = string.Empty;
            }
            latmode = GetFunction("select mode from registration where roll_no='" + RollNo + "'");
            latsem = GetFunction("select min(semester) from subjectchooser where roll_no='" + RollNo + "'");
            int latsemes = 0;
            calculate = string.Empty;
            if (Convert.ToInt32(semval) >= Convert.ToInt32(latsem))
            {
                for (se = Convert.ToInt32(latsem); se <= Convert.ToInt32(semval); se++)
                {
                    latsemes = latsemes + 1;
                }
            }
            if (Convert.ToInt32(latmode) == 1)
            {
                calculate = Math.Round((finalgpa1 / Convert.ToInt32(semval)), 2).ToString();
            }
            else
            {
                calculate = Math.Round((finalgpa1 / Convert.ToInt32(latsemes)), 2).ToString();
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return calculate;
    }

    private double cgpa(string RollNo, int semval)
    {
        int grcredit1 = 0;
        try
        {
            string strgrade = string.Empty;
            string strcredit = string.Empty;
            string sem = string.Empty;
            int i = 0;
            int gpa1 = 0;
            overallcredit = 0;
            sem = semval.ToString();
            int gpacal2 = 0;
            int gpacal = 0;
            string strsem = string.Empty;
            string mgrade = string.Empty;
            int gpa = 0;
            int grpoints = 0;
            int grcredit = 0;
            con_sem.Close();
            con_sem.Open();
            strsem = "select exam_system,first_year_nonsemester from ndegree where degree_code=" + degree_code + " and batch_year=" + batch_year + "";
            SqlCommand cmd_sem = new SqlCommand(strsem, con_sem);
            SqlDataReader dr_sem;
            dr_sem = cmd_sem.ExecuteReader();
            if (dr_sem.Read())
            {
                string examsys = Convert.ToString(dr_sem["first_year_nonsemester"]);
                if (examsys == "False")
                {
                    for (int j = 0; j <= Convert.ToInt32(sem); j++)
                    {
                        IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + ""));
                        string strresult = string.Empty;
                        strresult = "Select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + IntExamCode + " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" + RollNo + "'";
                        SqlCommand cmd_result = new SqlCommand(strresult, con_result);
                        con_result.Close();
                        con_result.Open();
                        SqlDataReader dr_result;
                        dr_result = cmd_result.ExecuteReader();

                        if (dr_result.Read())
                        {
                            if (Convert.ToString(dr_result["grade"]) == "")
                            {
                                mgrade = string.Empty;
                            }
                            else
                            {
                                mgrade = dr_result["grade"].ToString();
                            }
                            if (mgrade == "")
                            {
                                mgrade = "-";
                            }
                            if (mgrade != "-")
                            {
                                //'--------------------------wuery for gradepoint
                                strgrade = "select credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + degree_code + " and batch_year='" + batch_year + "'";
                                SqlCommand cmd_grad = new SqlCommand(strgrade, con_Grade1);
                                con_Grade1.Close();
                                con_Grade1.Open();
                                SqlDataReader dr_grad;
                                dr_grad = cmd_grad.ExecuteReader();
                                dr_grad.Read();
                                if (dr_grad.HasRows)
                                {
                                    if (dr_grad["credit_points"].ToString() != "")
                                    {
                                        grpoints = Convert.ToInt32(dr_grad["credit_points"].ToString());
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                }
                            }
                            else //'------else of mgrade
                            {
                                grpoints = 0;
                            }
                            //'------------query for creditpoint
                            strcredit = "select credit_points from subject where subject_no= " + dr_result["subject_no"] + " ";
                            SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                            con_credit.Close();
                            con_credit.Open();
                            SqlDataReader dr_credit;
                            dr_credit = cmd_credit.ExecuteReader();
                            dr_credit.Read();
                            if (dr_credit.HasRows)
                            {
                                if (dr_credit["credit_points"].ToString() != "")
                                {
                                    grcredit = Convert.ToInt32(dr_credit["credit_points"].ToString());
                                    grcredit1 = grcredit1 + grcredit;
                                }
                            }
                            else
                            {
                                grcredit = 0;
                            }
                            gpa = grpoints * grcredit;
                            gpa1 = gpa1 + gpa;
                        }
                        //}

                    }
                    if (grcredit1 != 0)
                    {
                        gpacal = gpa1 / grcredit1;
                    }
                    else
                    {
                        gpacal = 0;
                    }
                    cgpa2 = Math.Round(Convert.ToDouble(gpacal), 2);
                }
            }

            else//'-----------------------------else of examsys condn-----------------------
            {
                for (int j = 1; j <= Convert.ToInt32(sem); j++)
                {
                    if (j == 2)
                    {
                        gpa = 0;
                        grpoints = 0;
                        grcredit = 0;
                        gpa1 = 0;
                        grcredit1 = 0;
                        IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
                        string strresult = string.Empty;
                        strresult = " Select mark_entry.*,maxtotal from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + IntExamCode + " and ltrim(rtrim(type))='' and  Attempts =1 and roll_no='" + RollNo + "'";
                        SqlCommand cmd_result = new SqlCommand(strresult, con_result);
                        con_result.Close();
                        con_result.Open();
                        SqlDataReader dr_result;
                        dr_result = cmd_result.ExecuteReader();
                        dr_result.Read();
                        if (dr_result.HasRows)
                        {
                            if (dr_result["grade"].ToString() == "")
                            {
                                mgrade = string.Empty;
                            }
                            else
                            {
                                mgrade = dr_result["grade"].ToString();
                            }
                            if (mgrade == "")
                            {
                                mgrade = "-";
                            }
                            if (mgrade != "-")
                            {
                                //'--------------------------query for gradepoint
                                strgrade = "select credit_points from grade_master where mark_grade= '" + mgrade + "' and degree_code= " + degree_code + " and batch_year='" + batch_year + "'";
                                SqlCommand cmd_grad = new SqlCommand(strgrade, con_Grade1);
                                con_Grade1.Close();
                                con_Grade1.Open();
                                SqlDataReader dr_grad;
                                dr_grad = cmd_grad.ExecuteReader();
                                dr_grad.Read();
                                if (dr_grad.HasRows)
                                {
                                    if (dr_grad["credit_points"].ToString() != "")
                                    {
                                        grpoints = Convert.ToInt32(dr_grad["credit_points"].ToString());
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                }
                            }
                            else //'------else of mgrade
                            {
                                grpoints = 0;
                            }
                            //'------------query for creditpoint
                            strcredit = "select credit_points from subject where subject_no= " + dr_result["subject_no"] + " ";
                            SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                            con_credit.Close();
                            con_credit.Open();
                            SqlDataReader dr_credit;
                            dr_credit = cmd_credit.ExecuteReader();
                            dr_credit.Read();
                            if (dr_credit.HasRows)
                            {
                                if (dr_credit["credit_points"].ToString() != "")
                                {
                                    grcredit = Convert.ToInt32(dr_credit["credit_points"].ToString());
                                    grcredit1 = grcredit1 + grcredit;
                                }
                            }
                            else
                            {
                                grcredit = 0;
                            }
                            gpa = grpoints * grcredit;
                            gpa1 = gpa1 + gpa;
                        }
                    }
                    gpacal = gpa1 / grcredit1;
                    gpacal2 = gpacal2 + gpacal;
                }//'-------------end loop
                int cgpa1 = 0;
                cgpa1 = gpacal2 / (Convert.ToInt32(sem) - 1);
                double cgpa2 = Math.Round(Convert.ToDouble(cgpa1), 2);
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        overallcredit = grcredit1;
        return overallcredit;
    }

    public string GetEarnedCreditoutgone(string RollNumber)
    {
        int EarnedCredit = 0;
        try
        {
            string syll_code = string.Empty;
            string new_rs = string.Empty;
            int ivalue = 0;
            string examcodeval = string.Empty;
            EarnedCredit = 0;
            //if (semdec > 0)
            //{
            for (ivalue = 1; ivalue <= Convert.ToInt32(ddlSemYr.SelectedValue.ToString()); ivalue++)
            {
                syll_code = GetFunction("select distinct syll_code from syllabus_master where degree_code=" + degree_code + " and semester =" + ivalue + " and batch_year=" + batch_year + "");
                new_rs = " Select Subject.credit_points,Mark_Entry.total from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and syll_Code = " + syll_code + "  and roll_no='" + RollNumber + "' and (result='Pass' or result='pass')and exam_code in (select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and current_semester<=" + ddlSemYr.SelectedValue.ToString() + ")";//'current_semester<=" + semdec +"
                SqlCommand cmd_rs = new SqlCommand(new_rs, con_rs);
                con_rs.Close();
                con_rs.Open();
                SqlDataReader dr_rs;
                dr_rs = cmd_rs.ExecuteReader();
                while (dr_rs.Read())
                {
                    if (dr_rs.HasRows)
                    {
                        if (dr_rs["credit_points"].ToString() != "")
                        {
                            EarnedCredit = EarnedCredit + Convert.ToInt32(dr_rs["credit_points"].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        //    }
        return EarnedCredit.ToString();
    }

    public string GetEarnedCredit(string RollNumber)
    {
        int EarnedCredit = 0;
        try
        {
            //string syll_code =string.Empty;
            string new_rs = string.Empty;
            int ivalue = 0;
            int examcodeval;
            EarnedCredit = 0;
            if (semdec > 0)
            {
                for (ivalue = 1; ivalue <= semdec; ivalue++)
                {
                    examcodeval = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
                    new_rs = "Select Subject.credit_points from Mark_Entry,Subject where Mark_Entry.Subject_No = Subject.Subject_No and Exam_Code = " + examcodeval + "  and roll_no='" + RollNumber + "' and (result='Pass' or result='pass')";
                    SqlCommand cdm_rs = new SqlCommand(new_rs, con_rs);
                    con_rs.Close();
                    con_rs.Open();
                    SqlDataReader dr_rss;
                    dr_rss = cdm_rs.ExecuteReader();
                    while (dr_rss.Read())
                    {
                        if (dr_rss.HasRows)
                        {
                            if (dr_rss["credit_points"].ToString() != "")
                            {
                                EarnedCredit = EarnedCredit + Convert.ToInt32(dr_rss["credit_points"].ToString());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return EarnedCredit.ToString();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        // btnGo_Click( sender,  e);
        //FpExternal.Visible = true;
        //lblnorec.Visible = false;
        BindExamMonth();
    }

    public void PrintMarkSheet()
    {
        try
        {
            FpExternal.Visible = true;
            btnxl.Visible = true;//added by srinath 24/5/2014
            lblxl.Visible = true;
            txtxlname.Visible = true;
            lblnorec.Visible = false;
            FpMarkSheet.Visible = true;
            // FpMarkSheet.Sheets[0].AutoPostBack = false;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            FpMarkSheet.Sheets[0].RowHeader.Visible = false;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Name = "Book Antiqua";
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Bold = true;
            FpMarkSheet.Sheets[0].DefaultStyle.Font.Size = FontUnit.Small;
            FpMarkSheet.Sheets[0].RowCount = 0;
            FpMarkSheet.Sheets[0].PageSize = 40;
            string maxsem = string.Empty;
            int i = 0;
            int k = 0;
            int j = 0;
            int no = 0;
            int nos = 0;
            string Subno = string.Empty;
            string RegNo = string.Empty;
            string RollNo = string.Empty;
            int Varsel;
            string credit = string.Empty;
            string result = string.Empty;
            string name = string.Empty;
            string course = string.Empty;
            string dob = string.Empty;
            string dobtemp = string.Empty;
            string mont = string.Empty;
            string mon = string.Empty;
            string yr = string.Empty;
            string gender = string.Empty;
            string grade = string.Empty;
            //  int grcredit1 = 0;
            double grcredit1 = 0;
            string grcredit = string.Empty;
            //  int gpacal = 0;
            double gpacal = 0;
            double g = 0;
            double gpa = 0;
            int pval = 0;
            bool flag = false;
            double gpa1 = 0;
            string sem = string.Empty;
            string regulation = string.Empty;
            string department = string.Empty;
            string Subname = string.Empty;
            string SubCode = string.Empty;
            int TotalPages = 0;
            bool once = false;
            string sem1 = string.Empty;
            string sem2 = string.Empty;
            string sem3 = string.Empty;
            int l = 0;
            bool optionflag = false;
            string oldsem = string.Empty;
            string getmnth = string.Empty;
            string getyear = string.Empty;
            string grademas = string.Empty;
            //  string grpoints =string.Empty;
            double grpoints = 0;
            string concat = string.Empty;
            string dept_name = string.Empty;
            string strExam_month = string.Empty;
            int subjcnt = 0;
            int chkstud_selected_cnt = 0;
            FpMarkSheet.Sheets[0].ColumnCount = 9;
            FpMarkSheet.Sheets[0].ColumnHeader.Visible = false;
            //---------------------------------------get the examcode
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            if (current_sem != "")
            {
                semdec = GetSemester_AsNumber(Convert.ToInt32(current_sem));
            }
            if ((exam_month != "") && (exam_year != "") && (exam_month != "0") && (exam_year != "0"))
            {
                //  IntExamCode = Convert.ToInt16(GetFunction("select distinct exam_code from exam_details where degree_code=" + degree_code + " and batch_year=" + batch_year + " and exam_month=" + exam_month.ToString() + " and exam_year= " + exam_year.ToString() + ""));
                IntExamCode = Convert.ToInt32(GetFunction("select distinct exam_code from exam_details where degree_code='" + degree_code + "' and batch_year='" + batch_year + "' and exam_month='" + exam_month.ToString() + "' and exam_year= '" + exam_year.ToString() + "'"));
                if (IntExamCode != 0)
                {
                    lblnorec.Visible = false;
                    FpExternal.Visible = true;
                    btnxl.Visible = true;//added by srinath 24/5/2014
                    lblxl.Visible = true;
                    txtxlname.Visible = true;
                    int sel;
                    //'-------loop for get chkbox val
                    FpExternal.SaveChanges();
                    string chkrollno = string.Empty;
                    string temprollno = string.Empty;
                    string chkRegNo = string.Empty;
                    string chkname = string.Empty;
                    string tempRegNo = string.Empty;
                    string tempname = string.Empty;
                    for (j = 0; j <= FpExternal.Sheets[0].RowCount - 1; j++)
                    {
                        // isval = Convert.ToInt32(FpReport.Sheets[0].GetValue(flagrow, 0).ToString());
                        sel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(j, 1).ToString());
                        if (sel == 1)
                        {
                            pval += 1;
                            chkstud_selected_cnt += 1;
                            chkRegNo = FpExternal.Sheets[0].Cells[j, 3].Text;
                            chkname = FpExternal.Sheets[0].Cells[j, 4].Text;
                            chkrollno = FpExternal.Sheets[0].Cells[j, 2].Text;
                            if (temprollno == "")
                            {
                                temprollno = chkrollno;
                            }
                            else
                            {
                                temprollno = temprollno + "," + chkrollno;
                            }
                            if (tempRegNo == "")
                            {
                                tempRegNo = chkRegNo;
                            }
                            else
                            {
                                tempRegNo = tempRegNo + "," + chkRegNo;
                            }
                            if (tempname == "")
                            {
                                tempname = chkname;
                            }
                            else
                            {
                                tempname = tempname + "," + chkname;
                            }
                        }
                    }
                    //'---------loop for get the rollno
                    // FpExternal.SaveChanges();
                    int chkstudent_count = 0;
                    string[] split_temprollno = temprollno.Split(',');
                    string[] split_tempRegNo = tempRegNo.Split(',');
                    string[] split_tempname = tempname.Split(',');
                    if (chkstud_selected_cnt > 0)
                    {
                        for (i = 0; i <= split_temprollno.GetUpperBound(0); i++)
                        {
                            btnPrint.Visible = true;
                            lblstudselect.Visible = false;
                            lblError.Visible = false;
                            chkstudent_count += 1;
                            lblnorec.Visible = false;
                            subjcnt = 0;
                            no = 0;
                            string EarnedCredit = string.Empty;
                            grcredit1 = 0;
                            string[] studarr = new string[pval];
                            Varsel = Convert.ToInt32(FpExternal.Sheets[0].GetValue(i, 1).ToString());
                            if (Varsel == 1)
                            {
                                TotalPages += 1;
                            }
                            RollNo = split_temprollno[i].ToString();
                            RegNo = split_tempRegNo[i].ToString();
                            name = split_tempname[i].ToString();
                            //   RollNo=FpExternal.Sheets[0].Cells[i, 1].Text;
                            studarr[l] = RollNo;
                            l = l + 1;
                            //'----------------query for get the branch,degreecode,batchyear etc
                            string strgetdetail = string.Empty;
                            //if (split_temprollno.GetUpperBound(i) >= 0)
                            //{
                            strgetdetail = "select branch_code,registration.current_semester ,registration.degree_code,sex,registration.batch_year,dob from registration,applyn where applyn.app_no=registration.App_no and Roll_no='" + RollNo + "'";
                            SqlCommand cmd_getdetail = new SqlCommand(strgetdetail, con_getdetail);
                            con_getdetail.Close();
                            con_getdetail.Open();
                            SqlDataReader dr_getdetail;
                            dr_getdetail = cmd_getdetail.ExecuteReader();
                            dr_getdetail.Read();
                            if (dr_getdetail.HasRows)
                            {
                                sem1 = dr_getdetail["current_semester"].ToString();
                                if (dr_getdetail["dob"].ToString() != "")
                                {
                                    dobtemp = dr_getdetail["dob"].ToString();
                                    string[] split_dobtemp = dobtemp.Split(new char[] { ' ' });
                                    string[] split_dob = split_dobtemp[0].Split(new char[] { '/' });
                                    string getday = split_dob[1].ToString();
                                    getmnth = split_dob[0].ToString();
                                    getyear = split_dob[2].ToString();
                                    concat = getday + '/' + getmnth + '/' + getyear;
                                }
                                else
                                {
                                    concat = string.Empty;
                                }
                                if (dr_getdetail["sex"].ToString() == "1")
                                {
                                    gender = "Female";
                                }
                                else
                                {
                                    gender = "Male";
                                }
                                if (dr_getdetail["batch_year"].ToString() != null)
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                                else
                                {
                                    regulation = txtRegulation.Text.Trim();
                                }
                            }//'-----end hasrows
                            //'---------------------------query for get the exam_mopnth,exam_year
                            string strdaters = string.Empty;
                            strdaters = "select exam_month,Exam_year,current_semester from exam_details where Exam_Code=" + IntExamCode + "";
                            ExamCode = IntExamCode;
                            SqlCommand cmd_daters = new SqlCommand(strdaters, con_daters);
                            con_daters.Close();
                            con_daters.Open();
                            SqlDataReader dr_daters;
                            dr_daters = cmd_daters.ExecuteReader();
                            dr_daters.Read();
                            if (dr_daters.HasRows)
                            {
                                mont = dr_daters["exam_month"].ToString();
                                yr = dr_daters["Exam_year"].ToString();
                                oldsem = dr_daters["current_semester"].ToString();
                                sem = dr_daters["current_semester"].ToString();
                                if (sem == "1")
                                    sem3 = "I";
                                else if (sem == "2")
                                    sem3 = "II";
                                else if (sem == "3")
                                    sem3 = "III";
                                else if (sem == "4")
                                    sem3 = "IV";
                                else if (sem == "5")
                                    sem3 = "V";
                                else if (sem == "6")
                                    sem3 = "VI";
                                else if (sem == "7")
                                    sem3 = "VII";
                                else if (sem == "8")
                                    sem3 = "VIII";
                                else if (sem == "9")
                                    sem3 = "IX";
                                else if (sem == "10")
                                    sem3 = "X";
                                //'-------------------
                            }//'- end dr_daters hasrows
                            //'-------------------------------------display the month as string-----------
                            if (exam_month == "1")
                                strExam_month = "Jan";
                            else if (exam_month == "2")
                                strExam_month = "Feb";
                            else if (exam_month == "3")
                                strExam_month = "Mar";
                            else if (exam_month == "4")
                                strExam_month = "Apr";
                            else if (exam_month == "5")
                                strExam_month = "May";
                            else if (exam_month == "6")
                                strExam_month = "Jun";
                            else if (exam_month == "7")
                                strExam_month = "Jul";
                            else if (exam_month == "8")
                                strExam_month = "Aug";
                            else if (exam_month == "9")
                                strExam_month = "Sep";
                            else if (exam_month == "10")
                                strExam_month = "Oct";
                            else if (exam_month == "11")
                                strExam_month = "Nov";
                            else if (exam_month == "12")
                                strExam_month = "DEc";
                            //'---------------------------------------query for get the course name dept name for the strudent
                            string strcourse = string.Empty;
                            strcourse = "select course_name,dept_name from course,department,degree where course.course_id=degree.course_id and degree.dept_code=department.dept_code and degree_code='" + dr_getdetail["degree_code"] + "'";
                            SqlCommand cmd_course = new SqlCommand(strcourse, con_course);
                            con_course.Close();
                            con_course.Open();
                            SqlDataReader dr_course;
                            dr_course = cmd_course.ExecuteReader();
                            dr_course.Read();
                            if (dr_course.HasRows)
                            {
                                if (txtGetDegree.Text == "")
                                {
                                    course = dr_course["course_name"].ToString();
                                }
                                else
                                {
                                    course = txtGetDegree.Text.Trim();
                                }
                                if (txtDepartment.Text == "")
                                {
                                    dept_name = dr_course["dept_name"].ToString();
                                }
                                else
                                {
                                    dept_name = txtDepartment.Text.Trim();
                                }
                                if (Chkbxcou.Checked == true)
                                {
                                    cou = 1;
                                }
                                else
                                {
                                    cou = 0;
                                }
                            }//'------end if dr_course
                            //NextPage: 
                            //RegNo = FpExternal.Sheets[0].Cells[i, 2].Text;
                            //name = FpExternal.Sheets[0].Cells[i, 3].Text;
                            //'----------------------------------------------incremen the row count
                            FpMarkSheet.Sheets[0].RowCount += 40;
                            //'------------------------------------load the clg information
                            string collnamenew1 = string.Empty;
                            string address1 = string.Empty;
                            string address3 = string.Empty;
                            string address = string.Empty;
                            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                            {
                                string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                SqlCommand collegecmd = new SqlCommand(college, con);
                                SqlDataReader collegename;
                                con.Close();
                                con.Open();
                                collegename = collegecmd.ExecuteReader();
                                if (collegename.HasRows)
                                {
                                    while (collegename.Read())
                                    {
                                        collnamenew1 = collegename["collname"].ToString();
                                        address1 = collegename["address1"].ToString();
                                        address3 = collegename["address3"].ToString();
                                        address = address1 + "," + address3;
                                    }
                                }
                            }
                            //'---------------------------------------------load theclg logo photo-------------------------------------
                            MyImg mi3 = new MyImg();
                            mi3.ImageUrl = "Handler/Handler2.ashx?";
                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi3;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                            FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi3;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                            //'---------------------------------------------load the student photo-------------------------------------
                            //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                            MyImg mi1 = new MyImg();
                            mi1.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi1;
                            //'----------------------------------------------------------------
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            if (txtCOE.Text != "")
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                            }
                            else
                            {
                                FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                            }
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                            //'--------------------------------------------set the heading for the columns--------
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 4);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                            //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                            //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "MaxMark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "Mark";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColor = Color.Black;
                            FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                            //'--------------------------------------------------------------------------------------
                            int p;
                            string getmark = string.Empty;
                            string getsem = string.Empty;
                            string getsubno = string.Empty;
                            string getsubname = string.Empty;
                            string getsubcode = string.Empty;
                            string getresult = string.Empty;
                            string Enrolledcredit = string.Empty;
                            string maxmark = string.Empty;
                            int count = FpMarkSheet.Sheets[0].RowCount - 29;
                            //'-----------------------------------query for select the subject name and details
                            strexam = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,semester,maxtotal from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "' order by semester desc,subject_type desc,subject.subject_no asc";
                            SqlCommand cmd_exam = new SqlCommand(strexam, con_exam);
                            con_exam.Close();
                            con_exam.Open();
                            dr_exam = cmd_exam.ExecuteReader();
                            if (dr_exam.HasRows)
                            {
                                nos += 1;
                                p = 0;
                                int sub_val = 1;
                                while (dr_exam.Read())
                                {
                                    if (subjcnt > (10 * sub_val))
                                    {
                                        sub_val = 1;
                                        FpMarkSheet.Sheets[0].Rows[count + subjcnt + 1].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 2, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 2].Text = "- - -Continued- - -";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt + 2, 0].HorizontalAlign = HorizontalAlign.Center;
                                        string coe = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                                        MyImg coeimg = new MyImg();
                                        coeimg.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                                        //'----------------------------------------------incremen the row count
                                        FpMarkSheet.Sheets[0].RowCount += 40;
                                        //'------------------------------------load the clg information
                                        //string collnamenew1 =string.Empty;
                                        //string address1 =string.Empty;
                                        //string address3 =string.Empty;
                                        //string address =string.Empty;
                                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                                        {
                                            string college = "select isnull(collname,'') as collname,isnull(address1,'') as address1,isnull(address3,'') as address3,isnull(pincode,'-')as pincode,logo1 as logo from collinfo where college_code=" + Session["collegecode"] + "";
                                            SqlCommand collegecmd = new SqlCommand(college, con);
                                            SqlDataReader collegename;
                                            con.Close();
                                            con.Open();
                                            collegename = collegecmd.ExecuteReader();
                                            if (collegename.HasRows)
                                            {
                                                while (collegename.Read())
                                                {
                                                    collnamenew1 = collegename["collname"].ToString();
                                                    address1 = collegename["address1"].ToString();
                                                    address3 = collegename["address3"].ToString();
                                                    address = address1 + "," + address3;
                                                }
                                            }
                                        }
                                        //'---------------------------------------------load theclg logo photo-------------------------------------
                                        //'----------------------
                                        MyImg mi4 = new MyImg();
                                        mi4.ImageUrl = "Handler/Handler2.ashx?";
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 0].CellType = mi4;
                                        // FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Size = FontUnit.Medium;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 1, 1, 7);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 40, 1].Text = collnamenew1;
                                        FpMarkSheet.Sheets[0].Cells[Convert.ToInt16(FpMarkSheet.Sheets[0].RowCount) - 40, 8].CellType = mi4;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 39].Border.BorderColorBottom = Color.Black;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 39, 0, 1, 9);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].Text = address;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 39, 0].HorizontalAlign = HorizontalAlign.Center;
                                        //'---------------------------------------------load the photo-------------------------------------
                                        //  FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 40, 6, 7, 1);
                                        MyImg mi5 = new MyImg();
                                        mi5.ImageUrl = "Handler/Handler4.ashx?rollno=" + RollNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 6, 6, 1);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 6].CellType = mi5;
                                        //'----------------------------------------------------------------
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Text = "Name of the candidate" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 35, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 35, 3].Text = name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Text = "Date Of Birth" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 33, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 33, 3].Text = concat;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 0].Text = "Registration Number" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 34, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].CellType = txt;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 34, 3].Text = RegNo;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 0, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 0].Text = "Branch" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 36, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 36, 3].Text = course;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 0].Text = "Gender" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 1].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 3, 1, 2);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        if (txtCOE.Text != "")
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = txtCOE.Text.Trim();
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 3].Text = gender;
                                        }
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 6].Text = regulation;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 32, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Text = "Department" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 31, 3, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 31, 3].Text = dept_name;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 30, 0, 1, 3);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Text = "ExamMonth & Year" + ":";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 0].Margin.Left = 15;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 30, 3].Text = strExam_month + '-' + exam_year;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 30].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------set the heading for the columns--------
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Text = "Sem";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Text = "SubCode";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 29, 2, 1, 4);
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Text = "SubName";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].HorizontalAlign = HorizontalAlign.Center;
                                        //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Text = "CreditPoint";
                                        //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].HorizontalAlign = HorizontalAlign.Center;
                                        //   FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 32, 5].Text = "Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Text = "MaxMark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Text = "Mark";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Text = "Result";
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 0].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 1].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 2].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 3].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 4].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 5].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 6].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 7].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 29, 8].Font.Bold = true;
                                        FpMarkSheet.Sheets[0].Rows[FpMarkSheet.Sheets[0].RowCount - 29].Border.BorderColorBottom = Color.Black;
                                        //'--------------------------------------------------------------------------------------
                                        count = FpMarkSheet.Sheets[0].RowCount - 29;
                                        subjcnt = 0;
                                        //'--------------------------------------------------------------------------------------
                                    }
                                    subjcnt += 1;
                                    //'--------------------------------get the semester value
                                    maxsem = GetFunction("Select max(semester) from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + IntExamCode + " and roll_no='" + RollNo + "'");
                                    p += 1;
                                    Subno = dr_exam["subject_no"].ToString();
                                    getsem = dr_exam["semester"].ToString();
                                    getresult = dr_exam["result"].ToString();
                                    getsubno = dr_exam["subject_no"].ToString();
                                    getsubcode = dr_exam["subject_code"].ToString();
                                    getsubname = dr_exam["subject_name"].ToString();
                                    maxmark = dr_exam["maxtotal"].ToString();
                                    grade = dr_exam["grade"].ToString();
                                    Session["grade_new"] = grade;
                                    string tot = dr_exam["total"].ToString();
                                    mark = dr_exam["total"].ToString();
                                    if (dr_exam["grade"].ToString() != "")
                                    {
                                        grade = dr_exam["grade"].ToString();
                                        credit = dr_exam["cp"].ToString();
                                    }
                                    else
                                    {
                                        //'------------------------------------query for get the link value
                                        string strsecrs = "select linkvalue from inssettings where linkname='Corresponding Grade' and college_code=" + Session["collegecode"] + "";
                                        SqlCommand cmd_secrs = new SqlCommand(strsecrs, con_secrs);
                                        con_secrs.Close();
                                        con_secrs.Open();
                                        SqlDataReader dr_secrs;
                                        dr_secrs = cmd_secrs.ExecuteReader();
                                        dr_secrs.Read();
                                        if (dr_secrs["linkvalue"].ToString() == "1")
                                        {
                                            string strnew = string.Empty;
                                            //'----------------------- query for get the ponits for grade details 
                                            strnew = " select * from grade_master where degree_code=" + degree_code + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + "";
                                            SqlCommand cmd_new = new SqlCommand(strnew, con_new);
                                            con_new.Close();
                                            con_new.Open();
                                            SqlDataReader dr_new;
                                            dr_new = cmd_new.ExecuteReader();
                                            dr_new.Read();
                                            if (dr_new.HasRows == true)
                                            {
                                                flag = true;
                                                //convertgrade(RollNo, Subno);
                                                //credit = funccredit;
                                                //grade = funcgrade;
                                                grade = Session["session_new"].ToString();
                                                if (mark != "")
                                                {
                                                    getmark = mark;
                                                    markflag = true;
                                                }
                                                else
                                                {
                                                    mark = string.Empty;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            getmark = mark;
                                            markflag = true;
                                        }
                                    }
                                    if (grade != "")
                                    {
                                        // grademas = "select distinct credit_points from grade_master where degree_code=" + dr_getdetail["degree_code"] + " and batch_year=" + batch_year + " and college_code=" + Session["collegecode"] + " and " + mark + " between frange and trange";
                                        grademas = "select credit_points from grade_master where mark_grade= '" + grade + "' and degree_code= " + dr_getdetail["degree_code"] + " ";
                                        SqlCommand cmd_grademas = new SqlCommand(grademas, con_grademas);
                                        con_grademas.Close();
                                        con_grademas.Open();
                                        SqlDataReader dr_grademas;
                                        dr_grademas = cmd_grademas.ExecuteReader();
                                        while (dr_grademas.Read())
                                        {
                                            grpoints = Convert.ToDouble(dr_grademas["credit_points"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        grpoints = 0;
                                    }
                                    string strcredit = string.Empty;
                                    strcredit = "select credit_points from subject where subject_no= " + Subno + " ";
                                    SqlCommand cmd_credit = new SqlCommand(strcredit, con_credit);
                                    con_credit.Close();
                                    con_credit.Open();
                                    SqlDataReader dr_credit;
                                    dr_credit = cmd_credit.ExecuteReader();
                                    dr_credit.Read();
                                    if (dr_credit.HasRows == true)
                                    {
                                        grcredit = dr_credit["credit_points"].ToString();
                                        grcredit1 = grcredit1 + Convert.ToDouble(grcredit);
                                    }
                                    else
                                    {
                                        grcredit = "0";
                                    }
                                    gpa = Convert.ToDouble(grpoints) * Convert.ToDouble(grcredit);
                                    gpa1 = gpa1 + gpa;
                                    if (grcredit1 > 0)
                                        g = gpa1 / grcredit1;
                                    else
                                        g = 0;
                                    if (getsem == "")
                                    {
                                        sem = string.Empty;
                                    }
                                    else
                                    {
                                        sem = getsem;
                                    }
                                    //string tformat =string.Empty;
                                    //string tattr =string.Empty;
                                    //string trowrtf =string.Empty;
                                    //'----------chk the condition for oldsem
                                    if (sem != oldsem)
                                    {
                                        //'----------condn for once flag
                                        if (once == false)
                                        {
                                            string remark = string.Empty;
                                            //'-------------------------condn for optionflag
                                            if (optionflag == true)
                                            {
                                                string stroption = string.Empty;
                                                stroption = "select distinct uncompulsory_subject.subject_no,subject_name,subject_code,remarks from uncompulsory_subject,subject where uncompulsory_subject.subject_no=subject.subject_no and degree_code=" + degree_code + " and semester=" + current_sem + " and batch_year=" + batch_year + " and roll_no='" + RollNo + "' order by subject_code asc";
                                                con_option.Close();
                                                con_option.Open();
                                                SqlCommand cmd_option = new SqlCommand(stroption, con_option);
                                                SqlDataReader dr_option;
                                                dr_option = cmd_option.ExecuteReader();
                                                while (dr_option.Read())
                                                {
                                                    if (dr_option.HasRows)
                                                    {
                                                        if (dr_option["subject_name"].ToString() != "")
                                                        {
                                                            Subname = dr_option["subject_name"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subname = string.Empty;
                                                        }
                                                        if (dr_option["subject_code"].ToString() != "")
                                                        {
                                                            SubCode = dr_option["subject_code"].ToString();
                                                        }
                                                        else
                                                        {
                                                            SubCode = string.Empty;
                                                        }
                                                        if (dr_option["subject_no"].ToString() != "")
                                                        {
                                                            Subno = dr_option["subject_no"].ToString();
                                                        }
                                                        else
                                                        {
                                                            Subno = string.Empty;
                                                        }
                                                        if (dr_option["remarks"].ToString() != "")
                                                        {
                                                            remark = dr_option["remarks"].ToString();
                                                        }
                                                        else
                                                        {
                                                            remark = string.Empty;
                                                        }
                                                        sem = GetSemester_AsNumber(Convert.ToInt32(current_sem)).ToString();
                                                        if (sem == "1")
                                                            sem2 = "I";
                                                        else if (sem == "2")
                                                            sem2 = "II";
                                                        else if (sem == "3")
                                                            sem2 = "III";
                                                        else if (sem == "4")
                                                            sem2 = "IV";
                                                        else if (sem == "5")
                                                            sem2 = "V";
                                                        else if (sem == "6")
                                                            sem2 = "VI";
                                                        else if (sem == "7")
                                                            sem2 = "VII";
                                                        else if (sem == "8")
                                                            sem2 = "VIII";
                                                        else if (sem == "9")
                                                            sem2 = "IX";
                                                        else if (sem == "10")
                                                            sem2 = "X";
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                                        if (Chkbxcou.Checked == false)
                                                        {
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 4);
                                                            // FpMarkSheet.Sheets[0].Cells[count + subjcnt ,1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                        else
                                                        {
                                                            SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 4);
                                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                                            //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                                        }
                                                    }//'--end hasrows of dr_option
                                                }//-end while dr_option
                                            }//-end of optionflag
                                        }//'--------end once flag condn
                                        once = true;
                                    }//'-------------------------end for condn sem!=oldsem
                                    //'===================================================================================
                                    if (getsubname != "")
                                    {
                                        Subname = getsubname;
                                    }
                                    else
                                    {
                                        Subname = string.Empty;
                                    }
                                    if (getsubcode != "")
                                    {
                                        SubCode = getsubcode;
                                    }
                                    else
                                    {
                                        SubCode = string.Empty;
                                    }
                                    if (getsubno != "")
                                    {
                                        Subno = getsubno;
                                    }
                                    else
                                    {
                                        Subno = string.Empty;
                                    }
                                    if (getresult != "")
                                    {
                                        result = getresult;
                                    }
                                    else
                                    {
                                        result = string.Empty;
                                    }
                                    if (getsem != "")
                                    {
                                        current_sem = getsem;
                                    }
                                    else
                                    {
                                        current_sem = string.Empty;
                                    }
                                    if (sem == "1")
                                        sem2 = "I";
                                    else if (sem == "2")
                                        sem2 = "II";
                                    else if (sem == "3")
                                        sem2 = "III";
                                    else if (sem == "4")
                                        sem2 = "IV";
                                    else if (sem == "5")
                                        sem2 = "V";
                                    else if (sem == "6")
                                        sem2 = "VI";
                                    else if (sem == "7")
                                        sem2 = "VII";
                                    else if (sem == "8")
                                        sem2 = "VIII";
                                    else if (sem == "9")
                                        sem2 = "IX";
                                    else if (sem == "10")
                                        sem2 = "X";
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].Text = sem2;
                                    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 0].HorizontalAlign = HorizontalAlign.Center;
                                    if (Chkbxcou.Checked == false)
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt , 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 4);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //   FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        SubCode = GetFunction("select subcourse_code from subject where subject_no=" + Subno + "");
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].Text = SubCode;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt, 1].HorizontalAlign = HorizontalAlign.Center;
                                        FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt, 2, 1, 4);
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 2].Text = Subname;
                                        //  FpMarkSheet.Sheets[0].Cells[count + subjcnt , 2].HorizontalAlign = HorizontalAlign.Left;
                                    }
                                    //'==================================================================================
                                    //if (credit == "0")
                                    //{
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = "-";
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                    //else
                                    //{
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].Text = credit;
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 5].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                    if (maxmark != "")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = maxmark;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //'---------------------------- chk the condn for markflag true
                                    if (markflag == true)
                                    {
                                        //'------------------------------to print the mark-----------------
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = mark;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = tot.ToString();
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    //else
                                    //{
                                    //    if (result == "Pass" || result == "pass")
                                    //    {
                                    //        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                    //        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //    }
                                    //    else
                                    //    {
                                    //        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                    //        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //    }
                                    //}
                                    //if (result == "Pass" || result == "pass")
                                    //{
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = grade;
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = grpoints;
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                    //else
                                    //{
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].Text = "-";
                                    //    //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 6].HorizontalAlign = HorizontalAlign.Center;
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].Text = "-";
                                    //    FpMarkSheet.Sheets[0].Cells[count + subjcnt, 7].HorizontalAlign = HorizontalAlign.Center;
                                    //}
                                    if (result == "Pass" || result == "pass")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = result;
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "SA" || result == "sa")
                                    {
                                        // FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "NS" || result == "ns")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "NS";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else if (result == "AAA" || result == "aaa")
                                    {
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "AB";
                                        FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                    }
                                    else
                                    {
                                        if (credit == "0")
                                        {
                                            //FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "SA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA"; // added by mullai
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                        else
                                        {
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].Text = "RA";
                                            FpMarkSheet.Sheets[0].Cells[count + subjcnt, 8].HorizontalAlign = HorizontalAlign.Center;
                                        }
                                    }
                                    //  no+=1;
                                    ////   FpMarkSheet.Sheets[0].RowCount += 2;
                                    //   if (nos != no)
                                    //   {
                                    //       FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 0, 1, 9);
                                    //       FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = "--End Of Statement---";
                                    //       FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                                    //   }
                                    //   else
                                    //   {
                                    //       FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 0, 1, 9);
                                    //       FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = "--Continued---";
                                    //       FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                                    //   }
                                    //'--------------coding for load the picture
                                    //LoadPicture(photoAccess(photoGet, collegelogo, genForAcad.collegecode, , 4));
                                    //===================================================================================
                                    //string coe = GetFunction("select coe from collinfo where college_code='"+Session["collegecode"]+"'");
                                    //FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                                    //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].Text = coe;
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorBottom = Color.Black;
                                    FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColor = Color.Black;
                                }//'---------end dr_exam
                            }//'--------end while dr_exam
                            FpMarkSheet.Sheets[0].Rows[count + subjcnt].Border.BorderColorBottom = Color.Black;
                            //'---------------------------------------------------------------after while nxt subject will be read
                            //  FpMarkSheet.Sheets[0].RowCount += 1;
                            FpMarkSheet.Sheets[0].SpanModel.Add(count + subjcnt + 1, 0, 1, 9);
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Text = "- - -End Of Statement- - -";
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].Font.Bold = true;
                            FpMarkSheet.Sheets[0].Cells[count + subjcnt + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                            string coe1 = GetFunction("select coe from collinfo where college_code='" + Session["collegecode"] + "'");
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 8, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 8, 7].Text = "Controller Of Examinations";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 9, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].Text = coe1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 7].HorizontalAlign = HorizontalAlign.Center;
                            MyImg coeimg1 = new MyImg();
                            coeimg1.ImageUrl = "Handler/CoeHandler/Handler.ashx?";
                            FpMarkSheet.Sheets[0].SpanModel.Add(FpMarkSheet.Sheets[0].RowCount - 10, 7, 1, 3);
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].CellType = coeimg1;
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 10, 7].HorizontalAlign = HorizontalAlign.Center;
                            ////   FpMarkSheet.Sheets[0].RowCount += 1;
                            //if (cou == 0)
                            //{
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 5].Text = "Semester";
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 5].HorizontalAlign = HorizontalAlign.Center;
                            //}
                            //else if (cou == 1)
                            //{
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 5].Text = "Semester";
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 5].HorizontalAlign = HorizontalAlign.Center;
                            //}
                            //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 6].Text = sem3;
                            //FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 6].HorizontalAlign = HorizontalAlign.Center;
                            //string ccva =string.Empty;
                            //ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
                            //'-----------------------------query for calculating the sum of credit points
                            //   FpMarkSheet.Sheets[0].RowCount += 1;
                            //    Enrolledcredit = GetFunction("select sum(s.credit_points) from syllabus_master as sy,subject as s,subjectchooser as sc where sy.syll_code=s.syll_code and sc.subject_no=s.subject_no and sy.batch_year=" + batch_year + " and sy.degree_code=" + degree_code + " and sc.semester<=" + semdec + " and roll_no='" + RollNo + "'");
                            //    if (ccva == "False")
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 0].Text = "EnrolledCredit:";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 3].Text = Enrolledcredit;
                            //    }
                            //    else
                            //    {
                            //        //'-------------------condn for out gone chkbox value
                            //        if (ChkOutgone.Checked == true)
                            //        {
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 0].Text = "EnrolledCredit OutGone:";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 3].Text = Enrolledcredit + " " + "(Out Gone)";
                            //        }
                            //        else
                            //        {
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 0].Text = "EnrolledCredit:";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 3].Text = Enrolledcredit;
                            //        }
                            //    }
                            //    //   EnrolledCredit =string.Empty;
                            //    sem = semdec.ToString();
                            //    //'=============================================calculate the cgpa value
                            //    string CGPA_Val =string.Empty;
                            //    string CPA_Val =string.Empty;
                            //    string sem4 =string.Empty;
                            //    if (flag == false)
                            //    {
                            //        cgpa(RollNo, Convert.ToInt32(sem));
                            //    }
                            //    else
                            //    {
                            //        CPA_Val = Calulat_GPA(RollNo, sem);
                            //        CGPA_Val = Calculete_CGPA(RollNo, sem);
                            //    }
                            //    // if (Chkbxcou.Checked == false)
                            //    if (cou == 0)
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 1].Text = "Semester";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    //  else if (Chkbxcou.Checked == true)
                            //    else if (cou == 1)
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 1].Text = "Semester";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    if (ChkOutgone.Checked == true)
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 2].Text = sem3;
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 2].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    else
                            //    {
                            //        if (Convert.ToInt32(maxsem) == 1)
                            //        {
                            //            sem4 = "I";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 2)
                            //        {
                            //            sem4 = "II";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 3)
                            //        {
                            //            sem4 = "III";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 4)
                            //        {
                            //            sem4 = "IV";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 5)
                            //        {
                            //            sem4 = "V";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 6)
                            //        {
                            //            sem4 = "VI";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 7)
                            //        {
                            //            sem4 = "VII";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 8)
                            //        {
                            //            sem4 = "VIII";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 9)
                            //        {
                            //            sem4 = "IX";
                            //        }
                            //        else if (Convert.ToInt32(maxsem) == 10)
                            //        {
                            //            sem4 = "X";
                            //        }
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 2].Text = sem4;
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 2].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    if (ccva == "False")
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 7].Text = "GPA";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 8].Text = CPA_Val;
                            //    }
                            //    else
                            //    {
                            //        if (ChkOutgone.Checked == true)
                            //        {
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 7].Text = "GPA OutGone";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 8].Text = CPA_Val + " " + "(Out Gone)";
                            //        }
                            //        else
                            //        {
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 7].Text = "GPA";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 16, 8].Text = CPA_Val;
                            //        }
                            //    }
                            //    //if (Chkbxcou.Checked == false)
                            //    if (cou == 0)
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 1].Text = "Semester";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    //else if (Chkbxcou.Checked == true)
                            //    else if (cou == 1)
                            //    {
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 1].Text = "Semester";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 1].HorizontalAlign = HorizontalAlign.Center;
                            //    }
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 2].Text = sem3;
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 2].HorizontalAlign = HorizontalAlign.Center;
                            //    if (ccva == "False")
                            //    {
                            //        EarnedVal = GetEarnedCreditoutgone(RollNo);
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 0].Text = "EarnedCredit:";
                            //        FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 3].Text = EarnedVal;
                            //    }
                            //    else
                            //    {
                            //        if (ChkOutgone.Checked == true)
                            //        {
                            //            EarnedVal = GetEarnedCreditoutgone(RollNo);
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 0].Text = "EarnedCredit OutGone:";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 3].Text = EarnedVal + " " + "(Out Gone)";
                            //        }
                            //        else
                            //        {
                            //            EarnedVal = GetEarnedCredit(RollNo);
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 0].Text = "EarnedCredit:";
                            //            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 3].Text = EarnedVal;
                            //        }
                            //    }
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 7].Text = "CGPA";
                            //    FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 15, 8].Text = CGPA_Val;
                            //    //---current date time
                            DateTime currentdate;
                            currentdate = System.DateTime.Now;
                            string[] split_currentdate = Convert.ToString(currentdate).Split(new char[] { ' ' });
                            string[] split_date = split_currentdate[0].Split(new char[] { '/' });
                            string concat_date = split_date[1].ToString() + '/' + split_date[0].ToString() + '/' + split_date[2].ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].Text = "Date";
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 1].Text = concat_date.ToString();
                            FpMarkSheet.Sheets[0].Cells[FpMarkSheet.Sheets[0].RowCount - 9, 0].HorizontalAlign = HorizontalAlign.Center;
                            g = 0;
                            overallcredit = 0;
                            cgpa2 = 0;
                            gpa = 0;
                            gpa1 = 0;
                            g = 0;
                            grcredit = string.Empty;
                            markflag = false;
                            flag = false;
                            once = false;
                            // }
                        }//'----------end for loop
                    }
                    else
                    {
                        lblstudselect.Text = "Please Select Atleast One Student To Print The MarkSheet";
                        //  lblError.Visible = true;
                        lblstudselect.Visible = true;
                        btnPrint.Visible = false;
                    }
                    //if (chkstudent_count == 0)
                    //{
                    //    lblError.Text = "Please Select Atleast One Student To Print The GradeSheet";
                    //    lblError.Visible = true;
                    //}
                    //'-------------------------------going to get the second student
                    Double totalRows = 0;
                    totalRows = Convert.ToInt32(FpMarkSheet.Sheets[0].RowCount);
                    Session["totalPages"] = (int)Math.Ceiling(totalRows / FpMarkSheet.Sheets[0].PageSize);
                    Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
                    //else
                    //{
                    //    FpMarkSheet.Sheets[0].PageSize = Convert.ToInt32(totalRows);
                    //  //  DropDownListpage.Items.Add(FpMarkSheet.Sheets[0].PageSize.ToString());
                    //    FpMarkSheet.Height = 100 + (10 * Convert.ToInt32(totalRows));
                    //}
                    //if (Convert.ToInt32(FpExternal.Sheets[0].RowCount) > 10)
                    //{
                    //    //DropDownListpage.Text = DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text;
                    //  //  FpMarkSheet.Sheets[0].PageSize = int.Parse(DropDownListpage.Items[DropDownListpage.Items.Count - 2].Text);
                    //  //  CalculateTotalPages();
                    //}
                }
                else
                {
                    FpExternal.Visible = false;
                    btnxl.Visible = false;//added by srinath 24/5/2014
                    lblxl.Visible = false;
                    txtxlname.Visible = false;
                    lblnorec.Visible = true;
                    Buttontotal.Visible = false;
                    btnLoad.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void rdMark_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void rdGrade_CheckedChanged(object sender, EventArgs e)
    {
    }

    protected void tamilbutton_Click(object sender, EventArgs e)
    {
        panelchech.Visible = true;
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        try
        {
            panelchech.Visible = false;
            MarkSheet();
            FpMarkSheet.Visible = true;
            ModalPopupExtender1.Show();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void one()
    {
        try
        {
            panelchech.Visible = false;
            if (rdGrade.Checked == true)
            {
                MarkSheet();
            }
            else
            {
                PrintMarkSheet();
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void two()
    {
        try
        {
            panelchech.Visible = false;
            if (rdGrade.Checked == true)
            {
                MarkSheet_ddl2();
            }
            else
            {
                PrintMarkSheet();
            }
            Double totalRows = 0;
            totalRows = Convert.ToInt32(FpExternal.Sheets[0].RowCount);
            Session["totalPages"] = (int)Math.Ceiling(totalRows / FpExternal.Sheets[0].PageSize);
            Buttontotal.Text = "Records : " + totalRows + "          Pages : " + Session["totalPages"];
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        try
        {
            one();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void btnpop_Click(object sender, EventArgs e)
    {
        try
        {
            panelchech.Visible = false;
            MarkSheet1();
            panelchech.Visible = false;
            if (gcheck == 0)
            {
                FpMarkSheet.Visible = true;
                ModalPopupExtender1.Show();
            }
            panelchech.Visible = false;
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void chk_select_all_CheckedChanged(object sender, EventArgs e)
    {
        if (chk_select_all.Checked == true)
        {
            for (int chk_all_row = 0; chk_all_row < FpExternal.Sheets[0].RowCount; chk_all_row++)
            {
                FpExternal.Sheets[0].Cells[chk_all_row, 1].Value = true;
            }
        }
        else
        {
            for (int chk_all_row = 0; chk_all_row < FpExternal.Sheets[0].RowCount; chk_all_row++)
            {
                FpExternal.Sheets[0].Cells[chk_all_row, 1].Value = false;
            }
        }
    }

    protected void chk_hide_all_CheckedChanged(object sender, EventArgs e)
    {
        for (int chk_all_row = 0; chk_all_row < FpExternal.Sheets[0].RowCount; chk_all_row++)
        {
            FpExternal.Sheets[0].Cells[chk_all_row, 1].Value = 0;
        }
        if (chk_hide_all.Checked == true)
        {
            FpExternal.Sheets[0].AutoPostBack = false;
            FpExternal.Sheets[0].Columns[1].Visible = false;
        }
        else
        {
            FpExternal.Sheets[0].AutoPostBack = false;
            FpExternal.Sheets[0].Columns[1].Visible = true;
        }
        FpExternal.SaveChanges();
        //if (chk_hide_all.Checked == true)
        //{
        //    FpExternal.Sheets[0].Columns[1].Visible = false;
        //    function_header_forhide();
        //}
        //else
        //{
        //    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
        //    {
        //        string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
        //        SqlCommand collegecmd = new SqlCommand(str, con);
        //        SqlDataReader collegename;
        //        con.Close();
        //        con.Open();
        //        collegename = collegecmd.ExecuteReader();
        //        if (collegename.HasRows)
        //        {
        //            while (collegename.Read())
        //            {
        //                collnamenew1 = collegename["collname"].ToString();
        //                address1 = collegename["address1"].ToString();
        //                address2 = collegename["address2"].ToString();
        //                district = collegename["district"].ToString();
        //                address = address1 + "-" + address2 + "-" + district;
        //                //  pincode = collegename["pincode"].ToString();
        //                categery = collegename["category"].ToString();
        //                Affliated = collegename["affliated"].ToString();
        //                Phoneno = collegename["phoneno"].ToString();
        //                Faxno = collegename["faxno"].ToString();
        //                phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
        //                email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
        //            }
        //        }
        //        con.Close();
        //    }
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 5, 1);//'----------spaning for logo col count 1
        //    FpExternal.Sheets[0].Columns[1].Visible = true;
        //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[2, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[3, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[5, 2].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 2, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[1, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 3, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[2, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 3, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[3, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 3, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, 1);
        //    FpExternal.Sheets[0].ColumnHeader.Cells[4, 3].Text =string.Empty;
        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 3, 1, 1);
        //    int text_vsbl_count = 0;
        //    int text_vsbl = 0;
        //    if (Convert.ToInt32(Session["Rollflag"]) == 1 && Convert.ToInt32(Session["Regflag"]) == 1)
        //    {
        //        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 6, 1);//'----------spaning for logo col count 1
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.Black;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
        //        text_vsbl = 3;
        //    }
        //    else if (Convert.ToInt32(Session["Rollflag"]) == 1)
        //    {
        //        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 2, 6, 1);//'----------spaning for logo col count 1
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorBottom = Color.Black;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorLeft = Color.White;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 2].Border.BorderColorRight = Color.White;
        //        text_vsbl = 3;
        //    }
        //    else if (Convert.ToInt32(Session["Regflag"]) == 1)
        //    {
        //        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 3, 6, 1);//'----------spaning for logo col count 1
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorBottom = Color.Black;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorLeft = Color.White;
        //        FpExternal.Sheets[0].ColumnHeader.Cells[0, 3].Border.BorderColorRight = Color.White;
        //        text_vsbl = 4;
        //    }
        //    int text_vsbl1 = text_vsbl;
        //    for (text_vsbl = text_vsbl1; text_vsbl < FpExternal.Sheets[0].ColumnCount; text_vsbl++)
        //    {
        //        if (FpExternal.Sheets[0].Columns[text_vsbl].Visible == true)
        //        {
        //            text_vsbl_count++;
        //            if (text_vsbl_count == 2)
        //            {
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, text_vsbl].Text = collnamenew1;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, text_vsbl].Text = "An " + categery + " Institution - Affiliated to " + Affliated + ".";
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, text_vsbl].Font.Size = FontUnit.Medium;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, text_vsbl].Font.Bold = true;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, text_vsbl].Font.Bold = true;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, text_vsbl].Font.Bold = true;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, text_vsbl].Font.Bold = true;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Font.Bold = true;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, text_vsbl].Font.Bold = true;
        //                //'----------------------------------------------------new----------------------------
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, text_vsbl].Text = address;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, text_vsbl].Text = "Provisional Results " + ddlMonth.SelectedItem.ToString() + " " + ddlYear.SelectedItem.ToString(); //phnfax; phnfax;
        //                //@@@@@@@@@new header added @@@@@@@@@@@@@@   on 07.07.12
        //                if (txtDOP.Text != string.Empty)
        //                {
        //                    string[] spl_dop = txtDOP.Text.Split('/');
        //                    if (Convert.ToInt32(spl_dop[0].ToString()) < 10)
        //                    {
        //                        string day_00 = func_dateformat_00(spl_dop[0].ToString());
        //                        FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Text = "Date of publication of results:" + day_00 + "/" + spl_dop[1].ToString() + "/" + spl_dop[2].ToString();
        //                    }
        //                    else
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Text = "Date of publication of results:" + txtDOP.Text.ToString();
        //                    }
        //                }
        //                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        //                //  FpExternal.Sheets[0].ColumnHeader.Cells[5, text_vsbl].Text = "Batch: " + ddlBatch.Text.ToString() +"  "+",         ,Degree & Branch: " + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString() +","+ "     Semester  :  " + ddlSemYr.Text.ToString(); //email;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Batch: " + ddlBatch.Text.ToString();
        //                //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 8, 1, 12);
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Text = "Degree & Branch :" + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString();
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorRight = Color.White;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Text = "Semester : " + ddlSemYr.Text.ToString();
        //                //FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 20, 1, (FpExternal.Sheets[0].ColumnCount - 4));
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorRight = Color.White;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].HorizontalAlign = HorizontalAlign.Right;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Bold = true;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Font.Bold = true;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Size = FontUnit.Medium;
        //                //FpExternal.Sheets[0].ColumnHeader.Cells[5, 18].Font.Size = FontUnit.Medium;
        //                set_batch_degree_branch();
        //                if (Convert.ToInt32(Session["Rollflag"]) == 1 && Convert.ToInt32(Session["Regflag"]) == 1)
        //                {
        //                    if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 8);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 8);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 8);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 8);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 8);
        //                    }
        //                    else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                    }
        //                    else
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                        //   FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 4);
        //                    }
        //                }
        //                else if (Convert.ToInt32(Session["Rollflag"]) == 1)
        //                {
        //                    if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                    }
        //                    else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        //   FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                    }
        //                    else
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        //   FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                    }
        //                }
        //                else if (Convert.ToInt32(Session["Regflag"]) == 1)
        //                {
        //                    if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 9);
        //                    }
        //                    else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 6);
        //                    }
        //                    else
        //                    {
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                        //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, text_vsbl, 1, FpExternal.Sheets[0].ColumnCount - 5);
        //                    }
        //                }
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, 5].Border.BorderColorTop = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 1, 1);
        //                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 5, 1);//'----------spaning for logo col count 1
        //                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 2, 1, 1);
        //                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 2, 5, 1);
        //                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 2);//'----------spaning for logo
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorTop = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Border.BorderColorRight = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, text_vsbl].Border.BorderColorBottom = Color.White;
        //                FpExternal.Sheets[0].ColumnHeader.Cells[4, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
        //                break;
        //            }
        //        }
        //    }
        //}
        ////FpExternal.Sheets[0].ColumnHeader.Cells[0, column_count - 1].CellType = mi2;
        //FpExternal.Sheets[0].AutoPostBack = true;
    }

    public void function_header_forhide()
    {
        try
        {
            int text_set_cnt = 0;
            int i = 0;
            if (Session["Rollflag"].ToString() == "1" && Session["Regflag"].ToString() == "1")
            {
                i = 3;
            }
            else if (Session["Rollflag"].ToString() == "1")
            {
                i = 3;
            }
            else if (Session["Regflag"].ToString() == "1")
            {
                i = 4;
            }
            int i1 = i;
            for (i = i1; i <= FpExternal.Sheets[0].ColumnCount - 1; i++)
            {
                if (FpExternal.Sheets[0].Columns[i].Visible == true)
                {
                    text_set_cnt++;
                    if (text_set_cnt == 2)
                    {
                        if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                        {
                            string str = "select isnull(collname,'') as collname,isnull(category,'') as category,isnull(affliatedby,'') as affliated,isnull(address1,'') as address1,isnull(address2,'') as address2,isnull(pincode,' ') as pincode,isnull(phoneno,'-') as phoneno,isnull(faxno,'-') as faxno,district,email,website from collinfo where college_code=" + Session["collegecode"] + "";
                            SqlCommand collegecmd = new SqlCommand(str, con);
                            SqlDataReader collegename;
                            con.Close();
                            con.Open();
                            collegename = collegecmd.ExecuteReader();
                            if (collegename.HasRows)
                            {
                                while (collegename.Read())
                                {
                                    collnamenew1 = collegename["collname"].ToString();
                                    address1 = collegename["address1"].ToString();
                                    address2 = collegename["address2"].ToString();
                                    district = collegename["district"].ToString();
                                    address = address1 + "-" + address2 + "-" + district;
                                    //  pincode = collegename["pincode"].ToString();
                                    categery = collegename["category"].ToString();
                                    Affliated = collegename["affliated"].ToString();
                                    Phoneno = collegename["phoneno"].ToString();
                                    Faxno = collegename["faxno"].ToString();
                                    phnfax = "Phone :" + " " + Phoneno + " " + "Fax :" + " " + Faxno;
                                    email = "E-Mail:" + collegename["email"].ToString() + " " + "Web Site:" + collegename["website"].ToString();
                                }
                            }
                            con.Close();
                        }
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 1, 5, 1);//'----------spaning for logo col count 1
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].Text = collnamenew1;
                        int value = Convert.ToInt32(FpExternal.Sheets[0].ColumnHeader.RowCount);
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, i].HorizontalAlign = HorizontalAlign.Center;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, 1].Text = string.Empty;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 1, 1, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].Text = "An " + categery + " Institution - Affiliated to " + Affliated + ".";
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, (FpExternal.Sheets[0].ColumnCount - 1)].Border.BorderColorLeft = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].Border.BorderColorBottom = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].Border.BorderColorBottom = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].Border.BorderColorBottom = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].Border.BorderColorBottom = Color.White;
                        //'----------------------------------------------------new----------------------------
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].Text = address;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].Text = "Provisional Results " + ddlMonth.SelectedItem.ToString() + " " + ddlYear.SelectedItem.ToString(); //phnfax; phnfax;
                        //@@@@@@@@@new header added @@@@@@@@@@@@@@   on 07.07.12
                        if (txtDOP.Text != string.Empty)
                        {
                            string[] spl_dop = txtDOP.Text.Split('/');
                            if (Convert.ToInt32(spl_dop[0].ToString()) < 10)
                            {
                                string day_00 = func_dateformat_00(spl_dop[0].ToString());
                                FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Text = "Date of publication of results:" + day_00 + "/" + spl_dop[1].ToString() + "/" + spl_dop[2].ToString();
                            }
                            else
                            {
                                FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Text = "Date of publication of results:" + txtDOP.Text.ToString();
                            }
                        }
                        //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Batch: " + ddlBatch.Text.ToString();
                        ////FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 8, 1, 12);
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Text = "Degree & Branch :" + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString();
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorRight = Color.White;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Text = "Semester : " + ddlSemYr.Text.ToString();
                        ////FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 20, 1, (FpExternal.Sheets[0].ColumnCount - 4));
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorRight = Color.White;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].HorizontalAlign = HorizontalAlign.Right;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Bold = true;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Font.Bold = true;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Size = FontUnit.Medium;
                        ////FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Font.Size = FontUnit.Medium;
                        set_batch_degree_branch();
                        //      FpExternal.Sheets[0].ColumnHeader.Cells[5, i].Text = "Batch: " + ddlBatch.Text.ToString() +",            Degree & Branch: " + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString() +","+ "     Semester :  " + ddlSemYr.Text.ToString(); //email;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 5, 1);//'----------spaning for logo
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, i].Font.Size = FontUnit.Medium;
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, i].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[1, i].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[2, i].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[3, i].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Font.Bold = true;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, i].Font.Bold = true;
                        if (Convert.ToInt32(Session["Rollflag"]) == 1 && Convert.ToInt32(Session["Regflag"]) == 1)
                        {
                            if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 8);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 8);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 8);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 8);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 8);
                            }
                            else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                //  FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                            }
                            else
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                                //  FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 4);
                            }
                        }
                        else if (Convert.ToInt32(Session["Rollflag"]) == 1)
                        {
                            if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                            }
                            else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                //    FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                            }
                            else
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                //  FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                            }
                        }
                        else if (Convert.ToInt32(Session["Regflag"]) == 1)
                        {
                            if (chksubjtype.Items[0].Selected != true && chksubjtype.Items[1].Selected == true && chkvsbl_setting.Items[2].Selected == true)
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 9);
                            }
                            else if (chkvsbl_setting.Items[2].Selected == true)//if result column hided means
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                                //   FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 6);
                            }
                            else
                            {
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(1, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(2, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(3, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(4, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                                //  FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, i, 1, FpExternal.Sheets[0].ColumnCount - 5);
                            }
                        }
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, 0, 6, 2);//'----------spaning for logo
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, 0].Border.BorderColorBottom = Color.Black;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[5, 0].Border.BorderColorTop = Color.White;
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 2, 1, 1);
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(0, FpExternal.Sheets[0].ColumnCount - 2, 5, 1);
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Border.BorderColorRight = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, i].Border.BorderColorBottom = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[0, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
                        FpExternal.Sheets[0].ColumnHeader.Cells[4, FpExternal.Sheets[0].ColumnCount - 1].Border.BorderColorLeft = Color.White;
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
        try
        {
            ddl_Page.Items.Clear();
            int i = 0;
            int totrowcount = FpExternal.Sheets[0].RowCount;
            int pages = totrowcount / 14;
            int intialrow = 1;
            int remainrows = totrowcount % 14;
            if (FpExternal.Sheets[0].RowCount > 0)
            {
                int i5 = 0;
                ddl_Page.Items.Insert(0, new System.Web.UI.WebControls.ListItem("", "0"));
                for (i = 1; i <= pages; i++)
                {
                    i5 = i;
                    ddl_Page.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                    intialrow = intialrow + 14;
                }
                if (remainrows > 0)
                {
                    i = i5 + 1;
                    ddl_Page.Items.Insert(i, new System.Web.UI.WebControls.ListItem(i.ToString(), intialrow.ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void ddlletter_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlletterformat.SelectedValue == "0")
            {
                btnLetterFormat_Click(sender, e);
            }
            else if (ddlletterformat.SelectedValue == "1")
            {
                btnLetterformat1_Click(sender, e);
            }
            else if (ddlletterformat.SelectedValue == "2")
            {
                tamilbutton_Click(sender, e);
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void ddl_Page_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            // btnGo_Click(sender, e);
            for (int i = 0; i < FpExternal.Sheets[0].RowCount; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = false;
            }
            int start = Convert.ToInt32(ddl_Page.SelectedValue.ToString());
            int end = (start + 14) - 1;   //14 old changed on 26.06.12
            if (end >= FpExternal.Sheets[0].RowCount)
            {
                end = FpExternal.Sheets[0].RowCount;
            }
            int rowstart = FpExternal.Sheets[0].RowCount - Convert.ToInt32(start);
            int rowend = FpExternal.Sheets[0].RowCount - Convert.ToInt32(end);
            for (int i = start - 1; i < end; i++)
            {
                FpExternal.Sheets[0].Rows[i].Visible = true;
            }
            for (int h = 0; h < FpExternal.Sheets[0].ColumnHeader.RowCount; h++)
            {
                FpExternal.Sheets[0].ColumnHeader.Rows[h].Visible = true;
            }
            FpExternal.Height = 150 + (20 * Convert.ToInt32(15));
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void chksubjtype_SelectedIndexChanged(object sender, EventArgs e)
    {
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
                txtsubjtype.Text = text + "";
            }
        }
    }

    protected void txtsubjtype_TextChanged(object sender, EventArgs e)
    {
        try
        {
            bind_arrear_sem();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void bind_arrear_sem()
    {
        try
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
                for (int arr_sem = 1; arr_sem < Convert.ToInt16(ddlSemYr.SelectedItem.ToString()); arr_sem++)
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
                    txtsubjtype.Text = text + "";
                }
            }
            //===============================
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
            //=====================================
            int count1 = 0;
            for (int i = 0; i < chkvsbl_setting.Items.Count; i++)
            {
                if (chkvsbl_setting.Items[i].Selected == true)
                {
                    count1++;
                    if (text == "")
                    {
                        text = "Visible(" + count1 + ")";
                    }
                    else
                    {
                        text = "Visible(" + count1 + ")";
                    }
                    txtvsbl_setting.Text = text;
                }
                else if (chkvsbl_setting.Items[i].Selected != true)
                {
                    txtvsbl_setting.Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void func_footer()
    {
        try
        {
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                string str = "select address2 from collinfo where college_code=" + Session["collegecode"] + "";
                SqlCommand collegecmd = new SqlCommand(str, con);
                SqlDataReader collegename;
                con.Close();
                con.Open();
                collegename = collegecmd.ExecuteReader();
                if (collegename.HasRows)
                {
                    while (collegename.Read())
                    {
                        address2 = collegename["address2"].ToString();
                    }
                }
                con.Close();
            }
            string[] spl_dop = txtDate.Text.Split('/'); //condn added on 11.08.12 mythili
            string day_date = string.Empty;
            if (Convert.ToInt32(spl_dop[0].ToString()) < 10)
            {
                day_date = func_dateformat_00(spl_dop[0].ToString()) + "/" + spl_dop[1].ToString() + "/" + spl_dop[2].ToString();
            }
            else
            {
                day_date = txtDate.Text.ToString();
            }
            FpExternal.Sheets[0].RowCount += 4;
            if (Convert.ToInt16(Session["Rollflag"]) == 1 && Convert.ToInt16(Session["Regflag"]) == 0)//clmn 2
            {
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 2);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 2);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Text = "Place :";// + address2.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = address2.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Right;
                if (txtDate.Text != "")
                {
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = day_date;// txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = "Date : ";// + txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
                }
            }
            else if (Convert.ToInt16(Session["Regflag"]) == 1 && Convert.ToInt16(Session["Rollflag"]) == 0)//clmn 3
            {
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 3, 0, 1, 3);
                FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 1, 0, 1, 3);
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Text = "Place :";
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Text = address2.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Right;
                if (txtDate.Text != "")
                {
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = day_date;// txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = "Date : ";/// +txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].HorizontalAlign = HorizontalAlign.Left;
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
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Text = day_date;// txtDate.Text.ToString();
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
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Text = "Place :";/// +address2.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Text = address2.ToString();
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 2].HorizontalAlign = HorizontalAlign.Right;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Bold = true;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 3].HorizontalAlign = HorizontalAlign.Left;
                if (txtDate.Text != "")
                {
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 3].Text = day_date;// txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Text = "Date : ";/// +txtDate.Text.ToString();
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Bold = true;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].Font.Size = FontUnit.Medium;
                    FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 2].HorizontalAlign = HorizontalAlign.Right;
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
            FpExternal.Sheets[0].Columns[0].Locked = true;
            //     FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 5, 0, 1, FpExternal.Sheets[0].ColumnCount );
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 4, 0, 1, FpExternal.Sheets[0].ColumnCount);
            FpExternal.Sheets[0].SpanModel.Add(FpExternal.Sheets[0].RowCount - 2, 0, 1, FpExternal.Sheets[0].ColumnCount);
            //=========color
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 4, 0].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 2, 0].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, 4].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 3, FpExternal.Sheets[0].ColumnCount - 4].Border.BorderColorBottom = Color.White;
            FpExternal.Sheets[0].Cells[FpExternal.Sheets[0].RowCount - 1, 4].Border.BorderColorBottom = Color.White;
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void chkarrear_Sem_SelectedIndexChanged(object sender, EventArgs e)
    {
        string code = string.Empty;
        string text = string.Empty;
        int count = 0;
        for (int i = 0; i < chkarrear_Sem.Items.Count; i++)
        {
            if (chkarrear_Sem.Items[i].Selected == true)
            {
                count++;
                if (text == "")
                {
                    code = chkarrear_Sem.Items[i].Value;
                    //  text = chkarrear_Sem.Items[i].Text;
                    // txtarrear_sem.Text = text;
                    text = "ArrearSem(" + count + ")";
                }
                else
                {
                    code = code + "," + chkarrear_Sem.Items[i].Value;
                    // txtarrear_sem.Text = text + "," + chkarrear_Sem.Items[i].Text;
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

    protected void chkvsbl_setting_SelectedIndexChanged(object sender, EventArgs e)
    {
        //=====================================
        string text = string.Empty;
        int count1 = 0;
        for (int i = 0; i < chkvsbl_setting.Items.Count; i++)
        {
            if (chkvsbl_setting.Items[i].Selected == true)
            {
                count1++;
                if (text == "")
                {
                    text = "Visible(" + count1 + ")";
                }
                else
                {
                    text = "Visible(" + count1 + ")";
                }
                txtvsbl_setting.Text = text;
            }
            else if (chkvsbl_setting.Items[i].Selected != true)
            {
                txtvsbl_setting.Text = string.Empty;
            }
        }
    }

    public void set_batch_degree_branch()
    {
        try
        {
            int fifth_hdr = ((FpExternal.Sheets[0].ColumnCount - 6) / 2);
            FpExternal.Sheets[0].ColumnHeader.Cells[5, 4].Text = "Batch: " + ddlBatch.Text.ToString();
            if (FpExternal.Sheets[0].ColumnCount > 20)
            {
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 8, 1, 12);
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Text = "Degree & Branch :" + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString();
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Border.BorderColorRight = Color.White;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 6].Border.BorderColorRight = Color.White;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].HorizontalAlign = HorizontalAlign.Center;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Text = "Semester : " + ddlSemYr.Text.ToString();
                FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 20, 1, (FpExternal.Sheets[0].ColumnCount - 4));
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Bold = true;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Font.Bold = true;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 20].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, 8].HorizontalAlign = HorizontalAlign.Center;
            }
            else
            {
                //-----------find how many columns are visible to set the degree branch section
                int total_count_deg_vsbl_clmn = 0;
                int count1 = 0;
                int last_clmn = 0;
                for (int deg_vsbl_clmn = 5; deg_vsbl_clmn < FpExternal.Sheets[0].ColumnCount; deg_vsbl_clmn++)
                {
                    if (FpExternal.Sheets[0].Columns[deg_vsbl_clmn].Visible == true)
                    {
                        count1++;
                        //   total_count_deg_vsbl_clmn++;
                        if (count1 == 1)
                        {
                            if (FpExternal.Sheets[0].ColumnCount > 6)
                            {
                                FpExternal.Sheets[0].ColumnHeader.Cells[5, 5 + count1].Text = "Degree & Branch :" + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString();
                                FpExternal.Sheets[0].ColumnHeader.Cells[5, 5 + count1].Font.Bold = true;
                                FpExternal.Sheets[0].ColumnHeader.Cells[5, 5 + count1].Font.Size = FontUnit.Medium;
                                FpExternal.Sheets[0].ColumnHeader.Cells[5, 5 + count1].Border.BorderColorRight = Color.White;
                            }
                        }
                        break;
                        //   last_clmn = deg_vsbl_clmn;
                    }
                }
                //=============================================================================
                int count2 = 0;
                for (int deg_vsbl_clmn = 5; deg_vsbl_clmn < FpExternal.Sheets[0].ColumnCount; deg_vsbl_clmn++)
                {
                    if (FpExternal.Sheets[0].Columns[deg_vsbl_clmn].Visible == true)
                    {
                        count2++;
                        total_count_deg_vsbl_clmn++;
                        last_clmn = deg_vsbl_clmn;
                    }
                }
                int div_deg_vsbl_clm = total_count_deg_vsbl_clmn / 2;
                if (count2 == 1)//for only one arrear
                {
                    FpExternal.Sheets[0].ColumnHeader.Cells[5, last_clmn].Text = "Degree & Branch :" + ddlDegree.SelectedItem.ToString() + "&" + ddlBranch.SelectedItem.ToString() + "Semester : " + ddlSemYr.Text.ToString();
                }
                else
                {
                    if (FpExternal.Sheets[0].ColumnCount > 6)
                    {
                        FpExternal.Sheets[0].ColumnHeaderSpanModel.Add(5, 5 + count1, 1, (last_clmn - total_count_deg_vsbl_clmn) - 2);
                    }
                    FpExternal.Sheets[0].ColumnHeader.Cells[5, last_clmn].Text = "Semester : " + ddlSemYr.Text.ToString();
                }
                FpExternal.Sheets[0].ColumnHeader.Cells[5, last_clmn].Font.Bold = true;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, last_clmn].Font.Size = FontUnit.Medium;
                FpExternal.Sheets[0].ColumnHeader.Cells[5, last_clmn].Border.BorderColorRight = Color.White;
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public string func_dateformat_00(string day)
    {
        switch (day)
        {
            case "1":
                day = "01";
                break;
            case "2":
                day = "02";
                break;
            case "3":
                day = "03";
                break;
            case "4":
                day = "04";
                break;
            case "5":
                day = "05";
                break;
            case "6":
                day = "06";
                break;
            case "7":
                day = "07";
                break;
            case "8":
                day = "08";
                break;
            case "9":
                day = "09";
                break;
            default:
                day = string.Empty;
                break;
        }
        return day;
    }

    protected void btnxl_Click(object sender, EventArgs e)
    {
        try
        {
            string rname = txtxlname.Text.ToString();
            if (rname.Trim() != "" && rname != null)
            {
                daccess.printexcelreport(FpExternal, rname);
            }
            else
            {
                lblxlerr.Visible = true;
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        //string appPath = HttpContext.Current.Server.MapPath("~");
        //string print =string.Empty;
        //if (appPath != "")
        //{
        //    int i = 1;
        //    appPath = appPath.Replace("\\", "/");
        //e:
        //    try
        //    {
        //        print = "Provisional Result" + i;
        //        //FpExternal.SaveExcel(appPath + "/Report/" + print + ".xls", FarPoint.Web.Spread.Model.IncludeHeaders.BothCustomOnly); //Print the sheet
        //        //Aruna on 26feb2013============================
        //        string szPath = appPath + "/Report/";
        //        string szFile = print + ".xls"; // + DateTime.Now.ToString("yyyyMMddHHmmss")
        //        FpExternal.SaveExcel(szPath + szFile, FarPoint.Web.Spread.Model.IncludeHeaders.ColumnHeadersCustomOnly);
        //        Response.Clear();
        //        Response.ClearHeaders();
        //        Response.ClearContent();
        //        Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
        //        Response.ContentType = "application/vnd.ms-excel";
        //        Response.Flush();
        //        Response.WriteFile(szPath + szFile);
        //        //=============================================
        //    }
        //    catch
        //    {
        //        i++;
        //        goto e;
        //    }
        //}
        //ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('" + print + ".xls" + " saved in" + " " + appPath + "/Report" + " successfully')", true);
    }

    public void LoadSpread()
    {
        try
        {
            exam_month = ddlMonth.SelectedValue.ToString();
            exam_year = ddlYear.SelectedValue.ToString();
            batch_year = "" + ddlBatch.SelectedValue.ToString() + "";
            degree_code = "" + ddlBranch.SelectedValue.ToString() + "";
            panelchech.Visible = false;
            sprdLetterFormat.Visible = true;
            string Gender = string.Empty;
            string RegisterNumber = ddlPage.SelectedValue.ToString();
            sprdLetterFormat.Sheets[0].RowCount = 0;
            MyImg mi = new MyImg();
            mi.ImageUrl = "~/images/10BIT001.jpeg";
            mi.ImageUrl = "Handler/Handler2.ashx?";
            MyImg mi2 = new MyImg();
            mi2.ImageUrl = "~/images/10BIT001.jpeg";
            mi2.ImageUrl = "Handler/Handler5.ashx?";
            sprdLetterFormat.SaveChanges();
            sprdLetterFormat.Sheets[0].PageSize = 24;
            sprdLetterFormat.Sheets[0].RowHeader.Visible = false;
            sprdLetterFormat.Sheets[0].DefaultStyle.Font.Name = "Book Antique";
            sprdLetterFormat.Sheets[0].DefaultStyle.Font.Bold = false;
            //sprdLetterFormat.Sheets[0].DefaultStyle.Font.Size = FontUnit.Medium;
            sprdLetterFormat.Sheets[0].DefaultStyle.Border.BorderSize = 1;
            sprdLetterFormat.Sheets[0].ColumnHeader.Visible = false;
            sprdLetterFormat.BorderColor = Color.Black;
            sprdLetterFormat.BorderStyle = BorderStyle.Solid;
            sprdLetterFormat.BorderWidth = 1;
            //sprdLetterFormat.Sheets[0].RowCount = 30; //old rowcount
            sprdLetterFormat.Sheets[0].RowCount = 29;  //new 14.03.2012
            sprdLetterFormat.Sheets[0].ColumnCount = 8;
            sprdLetterFormat.Sheets[0].Columns[0].Width = 100;
            sprdLetterFormat.Sheets[0].Columns[1].Width = 400;
            sprdLetterFormat.Sheets[0].Columns[2].Width = 120;
            sprdLetterFormat.Sheets[0].Columns[3].Width = 90;
            sprdLetterFormat.Sheets[0].Columns[4].Width = 100;
            sprdLetterFormat.Sheets[0].Columns[5].Width = 180;
            // sprdLetterFormat.Width = 1000;
            sprdLetterFormat.Sheets[0].AutoPostBack = true;
            sprdLetterFormat.Height = 610;
            // sprdLetterFormat.Height = 500;
            if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
            {
                con.Open();
                string s;
                s = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                s = s + "select isnull(a.stud_name,'  ') as studentname,isnull(a.sex,' ') as Gender,isnull(a.parent_name,' ') as parentname,isnull(a.parent_addressC,' ') as parentaddress,isnull(a.StreetC,'') as street,isnull(a.Cityc,' ') as city,isnull(a.Districtc,' ') as district  from Registration r,applyn a  where a.app_no=r.App_No and  r.degree_code= '" + Session["Branchcode"] + "' and r.batch_year='" + Session["Batch"] + "' and r.roll_no='" + RegisterNumber + "'";
                SqlDataAdapter da = new SqlDataAdapter(s, con);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0 && ds.Tables[1].Rows.Count > 0)
                {
                    if (ds.Tables[1].Rows[0]["Gender"].ToString() != null || ds.Tables[1].Rows[0]["Gender"].ToString() != " ")
                    {
                        if (ds.Tables[1].Rows[0]["Gender"].ToString() == "1")
                        {
                            Gender = "daughter";
                        }
                        else if (ds.Tables[1].Rows[0]["Gender"].ToString() == "0")
                        {
                            Gender = "son";
                        }
                    }
                    sprdLetterFormat.Sheets[0].ScrollingContentVisible = true;
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 29, 0, 3, 1); //29
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 29, 3, 3, 1);//29
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 29, 1, 1, 2);//30
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 28, 1, 1, 2);//29
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 27, 1, 1, 2);//28
                    //sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 26, 1, 1, 2);//27
                    //sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 25, 0, 1, 3);//25
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 25, 0, 3, 3);//25
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 26, 0, 1, 2);//26
                    //sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 23, 0, 1, 3);//23
                    //sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 22, 0, 1, 3);//22
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 26, 2, 1, 2);//26
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 22, 5, 1, 3);//23
                    //sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 24, 0, 1, 4);//24
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 21, 5, 1, 3);//22
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 20, 5, 1, 3);//21
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 19, 5, 1, 3);//20
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 18, 5, 1, 3);//19
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 17, 5, 1, 3);//18
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 13, 5, 1, 3);//14
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 12, 5, 1, 3);//13
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 11, 5, 1, 3);//12
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 10, 5, 1, 3);//11
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 9, 5, 1, 3);//10
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 8, 5, 1, 3);//9
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].Border.BorderStyle = BorderStyle.Solid;//27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].Border.BorderColor = Color.Black;//27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Border.BorderStyle = BorderStyle.Solid;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Border.BorderColor = Color.Black;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Border.BorderColorBottom = Color.White;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Border.BorderStyle = BorderStyle.Solid;//32
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Border.BorderColor = Color.Black;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Border.BorderColorBottom = Color.White;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Border.BorderColorTop = Color.White;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Border.BorderStyle = BorderStyle.Solid;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Border.BorderColorTop = Color.White;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Border.BorderStyle = BorderStyle.Solid;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Border.BorderColorTop = Color.White;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Border.BorderStyle = BorderStyle.Solid;//19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Border.BorderColorTop = Color.White;//19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Border.BorderStyle = BorderStyle.Solid;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Border.BorderColorTop = Color.White;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Border.BorderStyle = BorderStyle.Solid;//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Border.BorderColorBottom = Color.White;//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Border.BorderStyle = BorderStyle.Solid;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Border.BorderColorTop = Color.White;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Border.BorderStyle = BorderStyle.Solid;//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Border.BorderColorTop = Color.White;//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Border.BorderStyle = BorderStyle.Solid;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Border.BorderColorBottom = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Border.BorderColorTop = Color.White;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Border.BorderStyle = BorderStyle.Solid;//9
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Border.BorderColorTop = Color.White;//9
                    //top border of sno,subjectname,...
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Border.BorderColorBottom = Color.Black;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Border.BorderColorLeft = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 3].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 3].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 3].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 3].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 3].Border.BorderStyle = BorderStyle.Solid;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Border.BorderColorBottom = Color.Black;//28
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Border.BorderStyle = BorderStyle.Solid;//28
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 5].Border.BorderColorBottom = Color.White;//27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 5].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 5].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 5].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 5].Border.BorderStyle = BorderStyle.Solid;//27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 5].Border.BorderColorBottom = Color.White;//26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 5].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 5].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 5].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 5].Border.BorderStyle = BorderStyle.Solid;//26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 5].Border.BorderColorBottom = Color.White;//25
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 5].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 5].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 5].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 5].Border.BorderStyle = BorderStyle.Solid;//25
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 4].Border.BorderColorBottom = Color.White;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 4].Border.BorderStyle = BorderStyle.Solid;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 5].Border.BorderColorBottom = Color.Black;//24
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 5].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 5].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 5].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 5].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 6].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 6].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 6].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 6].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 6].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 7].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 7].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 7].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 7].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 7].Border.BorderStyle = BorderStyle.Solid;//24
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Border.BorderColorBottom = Color.Black;//14
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 6].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 6].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 6].Border.BorderColorRight = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 6].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 6].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 7].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 7].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 7].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 7].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 7].Border.BorderStyle = BorderStyle.Solid;//14
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 4].Border.BorderColorBottom = Color.White;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 4].Border.BorderStyle = BorderStyle.Solid;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 4].Border.BorderColorBottom = Color.White;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 4].Border.BorderStyle = BorderStyle.Solid;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 4].Border.BorderColorBottom = Color.White;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 4].Border.BorderStyle = BorderStyle.Solid;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 4].Border.BorderColorBottom = Color.White;//19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 4].Border.BorderStyle = BorderStyle.Solid;//19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 4].Border.BorderColorBottom = Color.White;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 4].Border.BorderStyle = BorderStyle.Solid;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 4].Border.BorderColorBottom = Color.White;//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 4].Border.BorderStyle = BorderStyle.Solid;//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 4].Border.BorderColorBottom = Color.White;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 4].Border.BorderStyle = BorderStyle.Solid;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 4].Border.BorderColorBottom = Color.White;//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 4].Border.BorderStyle = BorderStyle.Solid;//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 4].Border.BorderColorBottom = Color.White;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 4].Border.BorderStyle = BorderStyle.Solid;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 4].Border.BorderColorBottom = Color.White;//9
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 4].Border.BorderColorLeft = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 4].Border.BorderColorRight = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 4].Border.BorderColorTop = Color.White;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 4].Border.BorderStyle = BorderStyle.Solid;//9
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Border.BorderStyle = BorderStyle.Solid;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 1].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 1].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 2].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 2].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 3].Border.BorderStyle = BorderStyle.Solid;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 3].Border.BorderColor = Color.Black;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 0].CellType = mi;//29
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 3].CellType = mi2;//29
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 4].HorizontalAlign = HorizontalAlign.Left;//30
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 1].Font.Bold = true;//30
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 1].HorizontalAlign = HorizontalAlign.Center;//30
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 29, 1].Text = ds.Tables[0].Rows[0]["collname"].ToString();//30
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 28, 1].Font.Bold = true;//29
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 28, 1].HorizontalAlign = HorizontalAlign.Center;//29
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 28, 1].Text = ds.Tables[0].Rows[0]["address1"].ToString() + "  " + ds.Tables[0].Rows[0]["address2"].ToString();//29
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 1].Font.Bold = true; //28
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 1].HorizontalAlign = HorizontalAlign.Center; //28
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 1].Text = ds.Tables[0].Rows[0]["address3"].ToString() + "," + " " + "Pincode - " + ds.Tables[0].Rows[0]["pincode"].ToString(); //28
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 27, 6].Text = "Inland Letter"; //28
                    sprdLetterFormat.Sheets[0].SpanModel.Add(sprdLetterFormat.Sheets[0].RowCount - 26, 6, 3, 1); //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].Font.Bold = true; //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].HorizontalAlign = HorizontalAlign.Center; //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].VerticalAlign = VerticalAlign.Middle; //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 6].Text = "STAMP"; //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 1].Font.Bold = true; //27
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 1].HorizontalAlign = HorizontalAlign.Center;//27
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 1].Text = "Pincode - " + ds.Tables[0].Rows[0]["pincode"].ToString(); //27
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 0].Font.Bold = true; //26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 0].HorizontalAlign = HorizontalAlign.Left; //26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 0].Text = "Dear Parents,"; //26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 2].Font.Bold = true; //26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 2].HorizontalAlign = HorizontalAlign.Center; //26
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 26, 2].Text = "Date  : " + DateTime.Now.Date.ToString("dd-MMM-yyyy"); //26
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].HorizontalAlign = HorizontalAlign.Center; //25
                    string nn = Session["ExmMnth"].ToString();
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Text = "Sub: " + nn.ToUpper() + " / " + Session["ExamYear"].ToString() + "  Exam Performance Report" + "," + " " + "Your  " + Gender + " " + ds.Tables[1].Rows[0]["studentname"].ToString() + " [" + RegisterNumber.ToString() + "]    studying in" + " " + Session["Degree"] + " [" + Session["Branch"] + "]" + " " + "has secured the following  marks in "; //25
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 0].HorizontalAlign = HorizontalAlign.Left; //23
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 0].VerticalAlign = VerticalAlign.Middle; //24
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 25, 0].Margin.Left = 25; //24
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 24, 0].Text = ds.Tables[1].Rows[0]["studentname"].ToString() + " [" + RegisterNumber.ToString() + "]    studying in" + Session["Degree"] + " [" + Session["Branch"] + "]";  //24
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 0].Margin.Left = 25; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 0].HorizontalAlign = HorizontalAlign.Left; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 0].VerticalAlign = VerticalAlign.Middle; //23
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 23, 0].Text =string.Empty;//23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Margin.Left = 10; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].HorizontalAlign = HorizontalAlign.Left; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Text = "To"; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 5].Font.Bold = true; //23
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Margin.Left = 30; //22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].HorizontalAlign = HorizontalAlign.Left;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].VerticalAlign = VerticalAlign.Middle;//22
                    //sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Text = "has secured the following  marks in ";//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Margin.Left = 20; //22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].HorizontalAlign = HorizontalAlign.Left;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Text = ds.Tables[1].Rows[0]["Parentname"].ToString();//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 21, 5].Font.Bold = true;//22
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].HorizontalAlign = HorizontalAlign.Center;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Font.Bold = true;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 0].Text = "S.No";//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 1].HorizontalAlign = HorizontalAlign.Center;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 1].Font.Bold = true;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 1].Text = "Subject Name";//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 2].HorizontalAlign = HorizontalAlign.Center;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 2].Font.Bold = true;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 2].Text = "Result";//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 3].HorizontalAlign = HorizontalAlign.Center;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 3].Font.Bold = true;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 22, 3].Text = "Grade/Mark";//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Margin.Left = 20;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].HorizontalAlign = HorizontalAlign.Left;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Text = ds.Tables[1].Rows[0]["Parentaddress"].ToString();//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 20, 5].Font.Bold = true;//21
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Margin.Left = 20;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].HorizontalAlign = HorizontalAlign.Left;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Text = ds.Tables[1].Rows[0]["street"].ToString();//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 19, 5].Font.Bold = true;//20
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Margin.Left = 20;//19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Text = ds.Tables[1].Rows[0]["city"].ToString();
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 18, 5].Font.Bold = true; //19
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Margin.Left = 20;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Text = ds.Tables[1].Rows[0]["district"].ToString();
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 17, 5].Font.Bold = true;//18
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Margin.Left = 0;//14
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 13, 5].Text = "SENDER'S NAME AND ADDRESS";//14
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Font.Bold = true;//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Margin.Left = 20;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 12, 5].Text = "THE PRINCIPAL";//13
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Font.Bold = true;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Margin.Left = 20;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Text = ds.Tables[0].Rows[0]["collname"].ToString();
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 11, 5].Font.Bold = true;//12
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Font.Bold = true;//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Margin.Left = 20;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 10, 5].Text = ds.Tables[0].Rows[0]["address1"].ToString();//11
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Font.Bold = true;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Margin.Left = 20;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].HorizontalAlign = HorizontalAlign.Left;//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 9, 5].Text = ds.Tables[0].Rows[0]["address2"].ToString();//10
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Font.Bold = true;//9
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Margin.Left = 20;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[sprdLetterFormat.Sheets[0].RowCount - 8, 5].Text = ds.Tables[0].Rows[0]["address3"].ToString();//9
                }
                string grade_setting = string.Empty;
                con.Close();
                con.Open();
                SqlCommand cmd;
                cmd = new SqlCommand("select linkvalue from inssettings where linkname='corresponding grade'", con);
                SqlDataReader dr_grade_val = cmd.ExecuteReader();
                while (dr_grade_val.Read())
                {
                    if (dr_grade_val.HasRows == true)
                    {
                        grade_setting = dr_grade_val[0].ToString();
                    }
                }
                int incr_grade_display = 0;
                string grade = string.Empty;
                string result1 = string.Empty;
                con.Close();
                con.Open();
                string s1;
                //s1 = "select distinct s.subject_name as subjectname,s.subject_no,s.min_ext_marks,s.min_int_marks,isnull(m.internal_mark,0) as internal_mark ,isnull(m.external_mark,0) as external_mark from Subject s,mark_entry m,exam_details ex where s.subject_no=m.subject_no  and m.roll_no='" + RegisterNumber.ToString() + "' and m.exam_code=ex.exam_code and  ex.degree_code='" + Session["Branchcode"] + "' and ex.current_semester='" + Session["Semester"] + "'";
                s1 = "Select mark_entry.*,maxtotal,subject_name,Subject_type,subject.min_ext_marks,subject.min_int_marks from Mark_Entry,Subject,sub_sem where Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and Exam_Code = " + Session["e_code"] + " and Attempts =1 and roll_no='" + RegisterNumber.ToString() + "'  order by subject_type desc,mark_entry.subject_no";
                SqlDataAdapter da1 = new SqlDataAdapter(s1, con);
                DataSet ds1 = new DataSet();
                da1.Fill(ds1);
                int count = sprdLetterFormat.Sheets[0].RowCount - 21;
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        //++++++++++++++++++++++++
                        {
                            // result1 = drmrkentry["result"].ToString();
                            result1 = ds1.Tables[0].Rows[i]["result"].ToString();
                            if (ds1.Tables[0].Rows[i]["grade"] != "")
                            {
                                if (grade_setting == "1")//mark based
                                {
                                    if ((ds1.Tables[0].Rows[i]["internal_mark"].ToString() != "") && (ds1.Tables[0].Rows[i]["External_mark"].ToString() != "") && (ds1.Tables[0].Rows[i]["internal_mark"].ToString() != " ") && (ds1.Tables[0].Rows[i]["External_mark"].ToString() != " ")) //new condition 14.03.2012
                                    {
                                        if (Convert.ToInt16(ds1.Tables[0].Rows[i]["internal_mark"].ToString()) >= Convert.ToInt16(ds1.Tables[0].Rows[i]["min_int_marks"].ToString()) && Convert.ToInt16(ds1.Tables[0].Rows[i]["External_mark"].ToString()) >= Convert.ToInt16(ds1.Tables[0].Rows[i]["min_ext_marks"].ToString()))
                                        {
                                            convertgrade(RegisterNumber, ds1.Tables[0].Rows[i]["subject_no"].ToString());
                                            result1 = "Pass";
                                        }
                                        else
                                        {
                                            funcgrade = "RA";
                                            result1 = "Fail";
                                        }
                                    }
                                    grade = funcgrade.ToString();
                                }
                                else //grade based
                                {
                                    grade = ds1.Tables[0].Rows[i]["grade"].ToString();
                                }
                                //+++++++++++++++++++++++++
                                //if (Convert.ToInt32(ds1.Tables[0].Rows[i]["internal_mark"].ToString()) >= Convert.ToInt32(ds1.Tables[0].Rows[i]["min_int_marks"].ToString()) && Convert.ToInt32(ds1.Tables[0].Rows[i]["external_mark"].ToString()) >= Convert.ToInt32(ds1.Tables[0].Rows[i]["min_ext_marks"].ToString()))
                                //{
                                //    grade = convertgrade(RegisterNumber, ds1.Tables[0].Rows[i]["subject_no"].ToString());
                                //    result1 = "Pass";
                                //}
                                //else
                                //{
                                //    grade = "RA";
                                //    result1 = "Fail";
                                //}
                                sprdLetterFormat.Sheets[0].Cells[count, 0].Text = Convert.ToInt16(i + 1).ToString();
                                sprdLetterFormat.Sheets[0].Cells[count, 0].Border.BorderStyle = BorderStyle.Solid;
                                sprdLetterFormat.Sheets[0].Cells[count, 0].Border.BorderColor = Color.Black;
                                sprdLetterFormat.Sheets[0].Cells[count, 0].HorizontalAlign = HorizontalAlign.Center;
                                sprdLetterFormat.Sheets[0].Cells[count, 1].Border.BorderStyle = BorderStyle.Solid;
                                sprdLetterFormat.Sheets[0].Cells[count, 1].Border.BorderColor = Color.Black;
                                sprdLetterFormat.Sheets[0].Cells[count, 1].Text = ds1.Tables[0].Rows[i]["subject_name"].ToString();
                                sprdLetterFormat.Sheets[0].Cells[count, 2].Border.BorderStyle = BorderStyle.Solid;
                                sprdLetterFormat.Sheets[0].Cells[count, 2].Border.BorderColor = Color.Black;
                                sprdLetterFormat.Sheets[0].Cells[count, 2].Text = result1;
                                sprdLetterFormat.Sheets[0].Cells[count, 3].Border.BorderStyle = BorderStyle.Solid;
                                sprdLetterFormat.Sheets[0].Cells[count, 3].Border.BorderColor = Color.Black;
                                sprdLetterFormat.Sheets[0].Cells[count, 2].HorizontalAlign = HorizontalAlign.Center;
                                sprdLetterFormat.Sheets[0].Cells[count, 3].Text = grade;
                                sprdLetterFormat.Sheets[0].Cells[count, 3].HorizontalAlign = HorizontalAlign.Center;
                                count++;
                            }
                        }
                    }
                    //--------------display cga and cgpa
                    //count = sprdLetterFormat.Sheets[0].RowCount - 10;
                    count = 17;
                    string gpa = Calulat_GPA(RegisterNumber, Session["Semester"].ToString());
                    //sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Text = gpa;
                    //sprdLetterFormat.Sheets[0].Cells[count + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                    //sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Font.Bold = true;
                    sprdLetterFormat.Sheets[0].Cells[count, 2].Text = "GPA : " + gpa;
                    sprdLetterFormat.Sheets[0].Cells[count, 2].HorizontalAlign = HorizontalAlign.Center;
                    sprdLetterFormat.Sheets[0].Cells[count, 2].Font.Bold = true;
                    string cgpa = Calculete_CGPA(RegisterNumber, Session["Semester"].ToString());
                    sprdLetterFormat.Sheets[0].Cells[count, 3].Text = "CGPA: " + cgpa;
                    sprdLetterFormat.Sheets[0].Cells[count, 3].HorizontalAlign = HorizontalAlign.Center;
                    sprdLetterFormat.Sheets[0].Cells[count, 3].Font.Bold = true;
                    //'------------------display the gradepoints   
                    sprdLetterFormat.Sheets[0].Cells[count, 0].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count, 1].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].Text = "GRADE";
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].Text = "GRADE POINT";
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].Border.BorderColor = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].Border.BorderColorTop = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].Border.BorderColorTop = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].Border.BorderColorBottom = Color.Black;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].Font.Bold = true;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].Font.Bold = true;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 1].HorizontalAlign = HorizontalAlign.Center;
                    sprdLetterFormat.Sheets[0].Cells[count + 1, 0].HorizontalAlign = HorizontalAlign.Center;
                    string strdisplaygrad = "select distinct mark_grade from grade_master where batch_year=" + Session["Batch"].ToString() + " and degree_code=" + Session["BranchCode"].ToString() + "";
                    con.Close();
                    con.Open();
                    SqlDataAdapter da_displaygrade = new SqlDataAdapter(strdisplaygrad, con);
                    DataSet ds_displaygrade = new DataSet();
                    da_displaygrade.Fill(ds_displaygrade);
                    int clas_adv_row = 0;
                    int cnt_noof_grades = 0;
                    int x = 0;
                    for (int sub_grade = 0; sub_grade < ds_displaygrade.Tables[0].Rows.Count; sub_grade++)
                    {
                        cnt_noof_grades++;
                        if (cnt_noof_grades <= 5)
                        {
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 0].Text = ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString();
                            string Grade_Point = GetFunction("select distinct credit_points from grade_master where mark_grade='" + ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString() + "' and  batch_year=" + Session["Batch"].ToString() + " and degree_code=" + Session["BranchCode"].ToString() + "");
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 1].Text = Grade_Point.ToString();
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 0].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 1].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 1].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 0].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 1].Border.BorderColor = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + sub_grade, 0].Border.BorderColor = Color.Black;
                        }
                        else if (cnt_noof_grades > 5)
                        {
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 2].Border.BorderColor = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Border.BorderColor = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count, 2].Border.BorderColorBottom = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count, 3].Border.BorderColorBottom = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 2].Border.BorderColorBottom = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Border.BorderColorBottom = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 2].Text = "GRADE";
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Text = "GRADE POINT";
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 2].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 3].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 2].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 1, 3].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 2].Text = ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString();
                            string Grade_Point = GetFunction("select distinct credit_points from grade_master where mark_grade='" + ds_displaygrade.Tables[0].Rows[sub_grade]["mark_grade"].ToString() + "' and  batch_year=" + Session["Batch"].ToString() + " and degree_code=" + Session["BranchCode"].ToString() + "");
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 3].Text = Grade_Point.ToString();
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 2].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 3].Font.Bold = true;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 2].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 3].HorizontalAlign = HorizontalAlign.Center;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 2].Border.BorderColor = Color.Black;
                            sprdLetterFormat.Sheets[0].Cells[count + 2 + x, 3].Border.BorderColor = Color.Black;
                            x++;
                        }
                        clas_adv_row = count + 2 + sub_grade;
                    }
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 1].HorizontalAlign = HorizontalAlign.Left;
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 1].Text = "CLASS ADVISOR";
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 1].Font.Bold = true;
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 3].HorizontalAlign = HorizontalAlign.Center;
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 3].Text = "H.O.D";
                    sprdLetterFormat.Sheets[0].Cells[clas_adv_row + 3, 3].Font.Bold = true;
                    for (int kk = 0; kk < 29; kk++)
                    {
                        for (int ll = 0; ll < 8; ll++)
                        {
                            sprdLetterFormat.Sheets[0].Cells[kk, ll].Border.BorderSize = 1;
                        }
                    }
                    for (int i = 1; i < 27; i++)
                    {
                        sprdLetterFormat.Sheets[0].Rows[sprdLetterFormat.Sheets[0].RowCount - i].Height = 5;
                        sprdLetterFormat.Sheets[0].Rows[sprdLetterFormat.Sheets[0].RowCount - i].Font.Size = 10;
                    }
                    con.Close();
                    sprdLetterFormat.Sheets[0].PageSize = sprdLetterFormat.Sheets[0].RowCount;//Added By Srinath 6/4/2013
                }
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void PageNumberSelected(object sender, EventArgs e)
    {
        try
        {
            panelchech.Visible = false;
            panelchech.Visible = false;
            FpMarkSheet.Visible = false;
            ModalPopupExtender1.Hide();
            panelchech.Visible = false;
            LoadSpread();
            panelchech.Visible = false;
            sprdLetterFormat.Visible = true;
            ModalPopupExtender2.Show();
            panelchech.Visible = false;
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void loaprovdpdf()
    {
        try
        {
            Hashtable ht = new Hashtable();
            string grade = string.Empty;
            DataTable provtable = new DataTable();
            provtable.Columns.Clear();
            provtable.Rows.Clear();
            DataColumn prov_data_col;
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Sno";
            provtable.Columns.Add(prov_data_col);
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Course Code";
            provtable.Columns.Add(prov_data_col);
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Course Name";
            provtable.Columns.Add(prov_data_col);
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Credit";
            provtable.Columns.Add(prov_data_col);
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Letter Credit";
            provtable.Columns.Add(prov_data_col);
            prov_data_col = new DataColumn();
            prov_data_col.ColumnName = "Subject Grade Points";
            provtable.Columns.Add(prov_data_col);
            DataRow prov_data_row;
            DataTable provtable1 = new DataTable();
            provtable1.Columns.Clear();
            provtable1.Rows.Clear();
            DataColumn prov_data_col1;
            prov_data_col1 = new DataColumn();
            prov_data_col1.ColumnName = "Marks";
            provtable1.Columns.Add(prov_data_col1);
            prov_data_col1 = new DataColumn();
            prov_data_col1.ColumnName = "Letter Grade";
            provtable1.Columns.Add(prov_data_col1);
            prov_data_col1 = new DataColumn();
            prov_data_col1.ColumnName = "Grade Points";
            provtable1.Columns.Add(prov_data_col1);
            prov_data_col1 = new DataColumn();
            prov_data_col1.ColumnName = "Trange";
            provtable1.Columns.Add(prov_data_col1);
            DataRow prov_data_row1;
            Font Fontbold = new Font("Times New Roman", 15, FontStyle.Bold);
            Font Fontsmall = new Font("Times New Roman", 10, FontStyle.Regular);
            Font Fontbold1 = new Font("Times New Roman", 10, FontStyle.Bold);
            Gios.Pdf.PdfDocument myprovdoc = new Gios.Pdf.PdfDocument(PdfDocumentFormat.A4);
            prov_exam_month = ddlMonth.SelectedValue.ToString();
            prov_exam_year = ddlYear.SelectedValue.ToString();
            prov_batch_year = "" + ddlBatch.SelectedValue.ToString() + "";
            prov_degree_code = "" + ddlBranch.SelectedValue.ToString() + "";
            panelchech.Visible = false;
            int prov_sno = 0;
            int prov_rowcount;
            int values = 0;
            int rankvalue = 0;
            DAccess2 d2 = new DAccess2();
            DataSet ds5 = new DataSet();
            Hashtable hashrank = new Hashtable();
            Hashtable h = new Hashtable();
            if (FpExternal.Sheets[0].RowCount > 0)
            {
                ds5 = d2.select_method("select * from sysobjects where name='rank_order' ", h, "text ");
                if (ds5.Tables[0].Rows.Count > 0)
                {
                    int q = d2.insert_method("drop table rank_order", h, "text");
                    int p = d2.insert_method("create table rank_order (id int identity primary key,roll_no nvarchar(50),cgpa float (8),rank int)", h, "text");
                }
                else
                {
                    int p = d2.insert_method("create table rank_order (id int identity primary key,roll_no nvarchar(50),cgpa float (8),rank int)", h, "text");
                }
                for (int a = 0; a < FpExternal.Sheets[0].RowCount; a++)
                {
                    string rge = Convert.ToString(FpExternal.Sheets[0].Cells[a, 2].Tag);
                    string graderesult = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + rge + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                    int pass = 0;
                    int fail = 0;
                    int arrear_reslut = 0;
                    int value = 0;
                    string r_value = string.Empty;
                    SqlDataAdapter dataresult = new SqlDataAdapter(graderesult, con);
                    DataSet dgs = new DataSet();
                    dataresult.Fill(dgs);
                    for (int i = 0; i < dgs.Tables[0].Rows.Count; i++)
                    {
                        value++;
                        r_value = dgs.Tables[0].Rows[i]["semester"].ToString();
                        string s = dgs.Tables[0].Rows[i]["result"].ToString();
                        if (r_value == ddlSemYr.SelectedValue.Trim())
                        {
                            if (s == "Pass")
                            {
                                pass++;
                            }
                            else
                            {
                                fail++;
                            }
                        }
                        else
                        {
                            arrear_reslut++;
                        }
                    }
                    if (fail == 0)
                    {
                        string gp = commonaccess.Calulat_GPA_Semwise(rge, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                        int st = 0;
                        st = d2.insert_method("insert into rank_order(roll_no,cgpa) values('" + rge + "','" + gp + "')", h, "Text");
                    }
                }
            }
            string rankcalc = string.Empty;
            rankcalc = " select roll_no,cgpa from rank_order order by cgpa desc";
            SqlDataAdapter dr4 = new SqlDataAdapter(rankcalc, con);
            DataSet dsr = new DataSet();
            dr4.Fill(dsr);
            string rank_value = string.Empty;
            int rank = 0;
            double temp = 0;
            double ranktemp = 0;
            for (int i = 0; i < dsr.Tables[0].Rows.Count; i++)
            {
                ranktemp = 0;
                if (dsr.Tables[0].Rows[i]["cgpa"].ToString() != null && dsr.Tables[0].Rows[i]["cgpa"].ToString().Trim() != "")
                {
                    ranktemp = Convert.ToDouble(dsr.Tables[0].Rows[i]["cgpa"].ToString());
                }
                if (temp == 0)
                {
                    rank++;
                    rank_value = dsr.Tables[0].Rows[i]["roll_no"].ToString();
                    string rankstring = ("update rank_order set rank=" + rank + " where roll_no='" + rank_value + "'");
                    temp = Convert.ToDouble((dsr.Tables[0].Rows[i]["cgpa"].ToString()));
                }
                else if (ranktemp < temp)
                {
                    rank++;
                    rank_value = dsr.Tables[0].Rows[i]["roll_no"].ToString();
                    string rankstring = ("update rank_order set rank=" + rank + " where roll_no='" + rank_value + "'");
                    temp = Convert.ToDouble((dsr.Tables[0].Rows[i]["cgpa"].ToString()));
                }
                else if (ranktemp == temp)
                {
                    rank_value = dsr.Tables[0].Rows[i]["roll_no"].ToString();
                    string rankstring = ("update rank_order set rank=" + rank + " where roll_no='" + rank_value + "'");
                    temp = Convert.ToDouble((dsr.Tables[0].Rows[i]["cgpa"].ToString()));
                }
                hrank.Add(rank_value, rank);
            }
            //int q1 = d2.insert_method("drop table rank_order", h, "text");
            string selectgrade = string.Empty;
            selectgrade = " select distinct Mark_Grade,Frange,Trange ,Credit_Points from grade_master where college_code='" + Session["collegecode"].ToString() + "'and degree_code=" + degree_code + " and batch_year=" + batch_year + " and Semester=" + ddlSemYr.SelectedItem.Text + " order by frange desc";
            SqlDataAdapter dr3 = new SqlDataAdapter(selectgrade, con);
            DataSet dsg = new DataSet();
            dr3.Fill(dsg);
            if (dsg.Tables[0].Rows.Count == 0)
            {
                selectgrade = " select distinct Mark_Grade,Frange,Trange ,Credit_Points from grade_master where college_code='" + Session["collegecode"].ToString() + "'and degree_code=" + degree_code + " and batch_year=" + batch_year + " and Semester=" + 0 + " order by frange desc";
                dsg = daccess.select_method_wo_parameter(selectgrade, "Text");
            }
            Hashtable hash = new Hashtable();
            if (dsg.Tables[0].Rows.Count > 0)
            {
                gradeflag = true;
                for (int a = 0; a < dsg.Tables[0].Rows.Count; a++)
                {
                    if (!hash.ContainsKey(dsg.Tables[0].Rows[a]["Mark_Grade"].ToString()))
                    {
                        hash.Add(dsg.Tables[0].Rows[a]["Mark_Grade"].ToString(), dsg.Tables[0].Rows[a]["Credit_Points"].ToString());
                    }
                }
            }
            prov_rowcount = FpExternal.Sheets[0].RowCount;
            for (int i1 = 0; i1 < prov_rowcount; i1++)
            {
                int prov_st = 0;
                prov_st = Convert.ToInt16(FpExternal.Sheets[0].Cells[i1, 1].Value);
                if (prov_st == 1)
                {
                    ht.Clear();
                    provtable.Rows.Clear();
                    provtable1.Rows.Clear();
                    string prov_RegisterNumber = Convert.ToString(FpExternal.Sheets[0].Cells[i1, 2].Tag);
                    gregisternumber = prov_RegisterNumber;
                    if (Session["collegecode"].ToString() != null && Session["collegecode"].ToString() != "")
                    {
                        con.Close();
                        con.Open();
                        string prov_str;
                        prov_str = "select isnull(collname, ' ') as collname,isnull(address1, ' ') as address1,isnull(address2,' ') as address2,isnull(address3, ' ') as address3,isnull(pincode,' ') as pincode from collinfo where college_code='" + Session["collegecode"].ToString() + "'";
                        prov_str = prov_str + "select r.mode,isnull(a.stud_name,'  ') as studentname,isnull(a.sex,' ') as Gender,isnull(a.parent_name,' ') as parentname,isnull(a.parent_addressC,' ') as parentaddress,isnull(a.StreetC,'') as street,isnull(a.Cityc,' ') as city,isnull(a.Districtc,' ') as district  from Registration r,applyn a  where a.app_no=r.App_No and  r.degree_code= '" + Session["Branchcode"] + "' and r.batch_year='" + Session["Batch"] + "' and r.roll_no='" + gregisternumber + "'";
                        prov_str = prov_str + "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + gregisternumber + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                        SqlDataAdapter da1 = new SqlDataAdapter(prov_str, con);
                        DataSet ds1 = new DataSet();
                        da1.Fill(ds1);
                        if (ds1.Tables[0].Rows.Count > 0 && ds1.Tables[1].Rows.Count > 0)
                        {
                            prov_collnamenew1 = ds1.Tables[0].Rows[0]["collname"].ToString();
                            prov_address1 = ds1.Tables[0].Rows[0]["address1"].ToString() + " , " + ds1.Tables[0].Rows[0]["address2"].ToString() + "," + ds1.Tables[0].Rows[0]["address3"].ToString() + "," + ds1.Tables[0].Rows[0]["pincode"].ToString();
                            gstudentname = ds1.Tables[1].Rows[0]["studentname"].ToString();
                        }
                        string s = "select grade_flag from grademaster where batch_year='" + ddlBatch.SelectedItem.Text + "' and degree_code= '" + ddlBranch.SelectedValue.ToString() + "' and exam_month='" + ddlMonth.SelectedItem.Value + "' and exam_year='" + ddlYear.SelectedItem.Text + "'";
                        SqlDataAdapter dr1 = new SqlDataAdapter(s, con);
                        DataSet d = new DataSet();
                        dr1.Fill(d);
                        string gradevalue = string.Empty;
                        string r = d.Tables[0].Rows[0]["grade_flag"].ToString();
                        if (r == "2")
                        {
                            for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                            {
                                prov_sno++;
                                string sub_no = string.Empty;
                                grade = ds1.Tables[2].Rows[i]["grade"].ToString();
                                sub_no = ds1.Tables[2].Rows[i]["subject_no"].ToString();
                                string arearesult = ds1.Tables[2].Rows[i]["result"].ToString();
                                string semster = ds1.Tables[2].Rows[i]["semester"].ToString();
                                if (semster == ddlSemYr.SelectedValue)
                                {
                                    prov_data_row = provtable.NewRow();
                                    prov_data_row["Sno"] = prov_sno.ToString();
                                    prov_data_row["Course Code"] = ds1.Tables[2].Rows[i]["subject_code"].ToString();
                                    prov_data_row["Course Name"] = ds1.Tables[2].Rows[i]["subject_name"].ToString();
                                    prov_data_row["Credit"] = ds1.Tables[2].Rows[i]["cp"].ToString();
                                    prov_data_row["Letter Credit"] = grade.ToString();
                                    values = Convert.ToInt16(hash[grade]);
                                    prov_data_row["Subject Grade Points"] = Convert.ToInt16(ds1.Tables[2].Rows[i]["cp"].ToString()) * values;
                                    provtable.Rows.Add(prov_data_row);
                                }
                                else
                                {
                                    arear++;
                                    if (arearesult == "Pass")
                                    {
                                        arearpass++;
                                    }
                                    else if (arearesult == "Fail")
                                    {
                                        arearfail++;
                                    }
                                    else if (arearesult == "AAA")
                                    {
                                        arearabsent++;
                                    }
                                }
                            }
                        }
                        else if (r == "3")
                        {
                            string dd = "select linkvalue from inssettings where linkname='corresponding grade' and college_code='" + Session["collegecode"].ToString() + "'";
                            SqlDataAdapter df = new SqlDataAdapter(dd, con);
                            DataSet df1 = new DataSet();
                            df.Fill(df1);
                            string rr = df1.Tables[0].Rows[0]["linkvalue"].ToString();
                            if (rr == "0")
                            {
                                double total = 0;
                                for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                                {
                                    prov_sno++;
                                    string sub_no = string.Empty;
                                    sub_no = ds1.Tables[2].Rows[i]["subject_no"].ToString();
                                    string arearesult = ds1.Tables[2].Rows[i]["result"].ToString();
                                    string semster = ds1.Tables[2].Rows[i]["semester"].ToString();
                                    if (semster == ddlSemYr.SelectedValue)
                                    {
                                        prov_data_row = provtable.NewRow();
                                        prov_data_row["Sno"] = prov_sno.ToString();
                                        prov_data_row["Course Code"] = ds1.Tables[2].Rows[i]["subject_code"].ToString();
                                        prov_data_row["Course Name"] = ds1.Tables[2].Rows[i]["subject_name"].ToString();
                                        prov_data_row["Credit"] = ds1.Tables[2].Rows[i]["cp"].ToString();
                                        double internel = Convert.ToDouble(ds1.Tables[2].Rows[i]["internal_mark"].ToString());
                                        double xternel = Convert.ToDouble(ds1.Tables[2].Rows[i]["External_mark"].ToString());
                                        total = internel + xternel;
                                        grade = total.ToString();
                                        prov_data_row["Letter Credit"] = grade.ToString();
                                        values = Convert.ToInt16(hash[grade]);
                                        prov_data_row["Subject Grade Points"] = Convert.ToInt16(ds1.Tables[2].Rows[i]["cp"].ToString()) * values;
                                        provtable.Rows.Add(prov_data_row);
                                    }
                                    else
                                    {
                                        arear++;
                                        if (arearesult == "Pass")
                                        {
                                            arearpass++;
                                        }
                                        else if (arearesult == "Fail")
                                        {
                                            arearfail++;
                                        }
                                        else if (arearesult == "AAA")
                                        {
                                            arearabsent++;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (rr == "1")
                                {
                                    for (int x = 0; x < ds1.Tables[2].Rows.Count; x++)
                                    {
                                        if ((ds1.Tables[2].Rows[x]["internal_mark"].ToString() != "") && (ds1.Tables[2].Rows[x]["External_mark"].ToString() != "") && (ds1.Tables[2].Rows[x]["internal_mark"].ToString() != " ") && (ds1.Tables[2].Rows[x]["External_mark"].ToString() != " ")) //new condition 14.03.2012
                                        {
                                            string sss = ds1.Tables[2].Rows[x]["subject_no"].ToString();
                                            if (Convert.ToDouble(ds1.Tables[2].Rows[x]["internal_mark"].ToString()) >= Convert.ToDouble(ds1.Tables[2].Rows[x]["min_int_marks"].ToString()) && Convert.ToDouble(ds1.Tables[2].Rows[x]["External_mark"].ToString()) >= Convert.ToDouble(ds1.Tables[2].Rows[x]["min_ext_marks"].ToString()))
                                            {
                                                convertgrade(gregisternumber, sss);
                                                grade = funcgrade;
                                            }
                                            else
                                            {
                                                grade = "U";
                                            }
                                            ht.Add(sss, grade);
                                        }
                                    }
                                    for (int i = 0; i < ds1.Tables[2].Rows.Count; i++)
                                    {
                                        prov_sno++;
                                        string sub_no = string.Empty;
                                        sub_no = ds1.Tables[2].Rows[i]["subject_no"].ToString();
                                        string arearesult = ds1.Tables[2].Rows[i]["result"].ToString();
                                        string semster = ds1.Tables[2].Rows[i]["semester"].ToString();
                                        if (semster == ddlSemYr.SelectedValue)
                                        {
                                            prov_data_row = provtable.NewRow();
                                            prov_data_row["Sno"] = prov_sno.ToString();
                                            prov_data_row["Course Code"] = ds1.Tables[2].Rows[i]["subject_code"].ToString();
                                            prov_data_row["Course Name"] = ds1.Tables[2].Rows[i]["subject_name"].ToString();
                                            prov_data_row["Credit"] = ds1.Tables[2].Rows[i]["cp"].ToString();
                                            string deval = Convert.ToString(ht[sub_no]);
                                            prov_data_row["Letter Credit"] = deval.ToString();
                                            values = Convert.ToInt16(hash[deval]);
                                            prov_data_row["Subject Grade Points"] = Convert.ToInt16(ds1.Tables[2].Rows[i]["cp"].ToString()) * values;
                                            provtable.Rows.Add(prov_data_row);
                                        }
                                        else
                                        {
                                            arear++;
                                            if (arearesult == "Pass")
                                            {
                                                arearpass++;
                                            }
                                            else if (arearesult == "Fail")
                                            {
                                                arearfail++;
                                            }
                                            else if (arearesult == "AAA")
                                            {
                                                arearabsent++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (dsg.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < dsg.Tables[0].Rows.Count; i++)
                            {
                                string tgrade = string.Empty;
                                tgrade = dsg.Tables[0].Rows[i]["Trange"].ToString();
                                prov_data_row1 = provtable1.NewRow();
                                prov_data_row1["Marks"] = dsg.Tables[0].Rows[i]["Frange"].ToString();//;+ (dsg.Tables [0].Rows [i]["Trange"].ToString ());
                                prov_data_row1["Letter Grade"] = dsg.Tables[0].Rows[i]["Mark_Grade"].ToString();
                                prov_data_row1["Grade Points"] = dsg.Tables[0].Rows[i]["Credit_Points"].ToString();
                                prov_data_row1["Trange"] = tgrade.ToString();
                                provtable1.Rows.Add(prov_data_row1);
                            }
                        }
                        Bindpdfn1(myprovdoc, Fontsmall, Fontbold, Fontbold1, provtable, provtable1, Response);
                    }
                }
            }
            if (gradeflag == false)
            {
                ScriptManager.RegisterStartupScript(this, typeof(Page), UniqueID, "alert('Grade Master Not Created')", true);
            }
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public void Bindpdfn1(Gios.Pdf.PdfDocument myprovdoc, Font Fontsmall, Font Fontbold, Font Fontbold1, DataTable prov, DataTable prov1, HttpResponse response)
    {
        //System.Drawing.Image img = null;
        ////string appno =string.Empty;
        //string sFilePath =string.Empty;
        //byte[] bytearray = null;
        //string stafffcode =string.Empty;
        try
        {
            // added by sridhar.....................Start
            degree_code = ddlBranch.SelectedValue.ToString();
            string hodcode = string.Empty;
            MemoryStream memoryStream = new MemoryStream();
            string srisql = "select s.StaffSign,s.staff_code from Department d,StaffPhoto s where d.Dept_Code='" + degree_code + "' and d.college_code='" + Session["collegecode"] + "' and d.Head_Of_Dept=s.staff_code ";
            srids.Clear();
            srids = daccess.select_method_wo_parameter(srisql, "Text");
            if (srids.Tables[0].Rows.Count > 0)
            {
                hodcode = srids.Tables[0].Rows[0]["staff_code"].ToString();
                hodcode = hodcode + degree_code;
                if (!File.Exists(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg")))
                {
                    byte[] file = (byte[])srids.Tables[0].Rows[0]["StaffSign"];
                    memoryStream.Write(file, 0, file.Length);
                    if (file.Length > 0)
                    {
                        System.Drawing.Image imgx = System.Drawing.Image.FromStream(memoryStream, true, true);
                        System.Drawing.Image thumb = imgx.GetThumbnailImage(400, 400, null, IntPtr.Zero);
                        thumb.Save(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg"), System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    memoryStream.Dispose();
                    memoryStream.Close();
                }
            }
            // added by sridhar.....................end  
            int prov_cnt;
            int prov_sno = 0;
            int rowspan = 0;
            prov_sno = prov.Rows.Count;
            if (prov_sno <= 20)
            {
                int prov_subno = 0;
                int prov_pagecount = prov_sno / 2;
                int prov_repage = prov_sno % 2;
                int prov_nopages = prov_pagecount;
                if (prov_repage > 0)
                {
                    prov_nopages++;
                }
                if (prov_nopages > 0)
                {
                    for (int row = 0; row < prov_nopages; row++)
                    {
                        prov_subno++;
                        int y = 30;
                        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 20, 20, 370);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 500, 20, 370);
                        }
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 110, y, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_collnamenew1);
                        PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(myprovdoc, 30, y + 30, 500, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_address1);
                        PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 80, y + 45, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Department of " + ddlBranch.SelectedItem.Text + "");
                        PdfTextArea ptss1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 80, y + 60, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "UNIVERSITY RESULTS INTIMATION TO PARENTS");
                        PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 160, y + 75, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Date:" + (System.DateTime.Now.ToString("dd/MM/yyyy")));
                        PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 20, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Parent,");
                        PdfTextArea ptss4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 35, y + 100, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "The following are the grades obtained by your Son / Daughter in the Anna University");
                        PdfTextArea ptss5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 20, y + 120, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Examinations held during " + ddlMonth.SelectedItem.Text + " / " + ddlYear.SelectedItem.Text + "");
                        PdfTextArea ptss6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 140, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name Of the Student:" + gstudentname);
                        PdfTextArea ptss7 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Register Number:" + gregisternumber);
                        if (ddlSemYr.SelectedValue == "1" || ddlSemYr.SelectedValue == "2")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: I / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "3" || ddlSemYr.SelectedValue == "4")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: II / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "5" || ddlSemYr.SelectedValue == "6")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: III / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "7" || ddlSemYr.SelectedValue == "8")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: IV / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        PdfTextArea ptss9 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 25, y + 750, 120, 30), System.Drawing.ContentAlignment.MiddleRight, "Class Advisor");
                        // added by sridhar.....................Start
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg")))
                        {
                            PdfImage hodsign = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/" + hodcode + ".jpeg"));
                            myprov_pdfpage.Add(hodsign, 420, y + 710, 800);
                        }
                        // added by sridhar.....................end
                        PdfTextArea ptss10 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 400, y + 750, 50, 30), System.Drawing.ContentAlignment.MiddleRight, "HOD");
                        myprov_pdfpage.Add(ptc);
                        myprov_pdfpage.Add(pts);
                        myprov_pdfpage.Add(ptss);
                        myprov_pdfpage.Add(ptss1);
                        myprov_pdfpage.Add(ptss2);
                        myprov_pdfpage.Add(ptss3);
                        myprov_pdfpage.Add(ptss4);
                        myprov_pdfpage.Add(ptss5);
                        myprov_pdfpage.Add(ptss6);
                        myprov_pdfpage.Add(ptss7);
                        // myprov_pdfpage.Add(ptss8);
                        myprov_pdfpage.Add(ptss9);
                        myprov_pdfpage.Add(ptss10);
                        prov_cnt = prov_subno * prov_sno;
                        int nst = prov_subno * 9;
                        Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
                        table1.VisibleHeaders = false;
                        int val = 0;
                        if (prov_cnt <= 20)
                        {
                            if (prov_subno == 1)
                            {
                                table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Columns[0].SetWidth(50);
                                table1.Columns[1].SetWidth(100);
                                table1.Columns[2].SetWidth(480);
                                table1.Columns[3].SetWidth(90);
                                table1.Columns[4].SetWidth(100);
                                table1.Columns[5].SetWidth(100);
                                table1.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("S.no");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Course Code");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Course Name");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Credits");
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetContent("Letter Grade");
                                table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 5).SetContent("Subject Grade Points");
                                for (int i = 0; i < prov_cnt; i++)
                                {
                                    val++;
                                    table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 0).SetContent(val);
                                    string course_code = prov.Rows[i]["Course code"].ToString();
                                    table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(val, 1).SetContent(course_code);
                                    string course_name = prov.Rows[i]["Course Name"].ToString();
                                    table1.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(val, 2).SetContent(course_name);
                                    string credits = prov.Rows[i]["Credit"].ToString();
                                    table1.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 3).SetContent(credits);
                                    string credits1 = prov.Rows[i]["Letter Credit"].ToString();
                                    table1.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 4).SetContent(credits1);
                                    string grde = prov.Rows[i]["Subject Grade Points"].ToString();
                                    table1.Cell(val, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 5).SetContent(grde);
                                }
                                int b = 0;
                                for (int a = 0; a < prov_cnt; a++)
                                {
                                    b = b + Convert.ToInt16(prov.Rows[a]["credit"].ToString());
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 1, 0, prov_cnt + 1, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 1, 0).SetContent("Total");
                                table1.Cell(prov_cnt + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table1.Cell(prov_cnt + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(prov_cnt + 1, 3).SetContent(b);
                                int pass = 0;
                                int fail = 0;
                                int value = 0;
                                int rankvalue = 0;
                                string r_value = string.Empty;
                                int arrear_value = 0;
                                int arrear_pass_value = 0;
                                int arrear_fail_value = 0;
                                int arrear_absent_value = 0;
                                int total_no_arrear = 0;
                                string graderesult = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + gregisternumber + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                                SqlDataAdapter dataresult = new SqlDataAdapter(graderesult, con);
                                DataSet dg = new DataSet();
                                dataresult.Fill(dg);
                                if (dg.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dg.Tables[0].Rows.Count; i++)
                                    {
                                        value++;
                                        string semster = dg.Tables[0].Rows[i]["semester"].ToString();
                                        string s = dg.Tables[0].Rows[i]["result"].ToString();
                                        if (semster == ddlSemYr.SelectedValue)
                                        {
                                            if (s == "Pass")
                                            {
                                                pass++;
                                            }
                                            else
                                            {
                                                fail++;
                                            }
                                        }
                                        else
                                        {
                                            arrear_value++;
                                            if (s == "Pass")
                                            {
                                                arrear_pass_value++;
                                            }
                                            else if (s == "Fail")
                                            {
                                                arrear_fail_value++;
                                            }
                                            else if (s == "AAA")
                                            {
                                                arrear_absent_value++;
                                            }
                                        }
                                    }
                                    total_no_arrear = arrear_absent_value + arrear_fail_value + fail;
                                }
                                table1.Cell(prov_cnt + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 4, 0).SetContent("No Subject Passed:" + pass);
                                table1.Cell(prov_cnt + 4, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 4, 3).SetContent("No Subject Failed:" + fail);
                                if (fail == 0)
                                {
                                    string gpa = commonaccess.Calulat_GPA_Semwise(gregisternumber, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                                    ggpa = gpa;
                                }
                                else
                                {
                                    ggpa = "-";
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 2, 0, prov_cnt + 2, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 2, 0).SetContent("GPA :" + ggpa);
                                Hashtable h1 = new Hashtable();
                                if (ggpa != "-")
                                {
                                    rankvalue = Convert.ToInt16(hrank[gregisternumber]);
                                    foreach (PdfCell pr in table1.CellRange(prov_cnt + 2, 3, prov_cnt + 2, 3).Cells)
                                    {
                                        pr.ColSpan = 3;
                                    }
                                    table1.Cell(prov_cnt + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(prov_cnt + 2, 3).SetContent("Rank Position: " + rankvalue);
                                }
                                else
                                {
                                    foreach (PdfCell pr in table1.CellRange(prov_cnt + 2, 3, prov_cnt + 2, 3).Cells)
                                    {
                                        pr.ColSpan = 3;
                                    }
                                    table1.Cell(prov_cnt + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(prov_cnt + 2, 3).SetContent("Rank Position: " + "-");
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 3, 0, prov_cnt + 3, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                // string sem = ddlSemYr.SelectedValue;
                                string sem = daccess.GetFunction("select current_semester from Exam_Details where exam_code='" + Session["e_code"] + "'");
                                if (sem == "4")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("IV SEMESTER ");
                                }
                                else if (sem == "3")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("III SEMESTER ");
                                }
                                else if (sem == "2")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("II SEMESTER ");
                                }
                                else if (sem == "1")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("I SEMESTER ");
                                }
                                else if (sem == "5")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("V SEMESTER ");
                                }
                                else if (sem == "6")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("VI SEMESTER ");
                                }
                                else if (sem == "7")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("VII SEMESTER ");
                                }
                                else if (sem == "8")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("VIII SEMESTER ");
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 4, 0, prov_cnt + 4, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 4, 3, prov_cnt + 4, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 5, 0, prov_cnt + 5, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 5, 0).SetContent("No.of Arrear subjects appeared:" + arrear_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 5, 3, prov_cnt + 5, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 5, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 5, 3).SetContent("No.of Arrear Subjects Passed:" + arrear_pass_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 6, 0, prov_cnt + 6, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 6, 0).SetContent("No. of Arrear subjects not appeared:" + arrear_absent_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 6, 3, prov_cnt + 6, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 6, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 6, 3).SetContent("No.of Arrear Subjects Failed:  " + arrear_fail_value);
                                totalarear = arearfail + arearabsent + fail;
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 7, 0, prov_cnt + 7, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                table1.Cell(prov_cnt + 7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 7, 0).SetContent("Total No fo Arrear:" + total_no_arrear);
                                int g_count = prov1.Rows.Count;
                                int g_countvalue = g_count / 2;
                                if (g_count > 0)
                                {
                                    Gios.Pdf.PdfTable table2 = myprovdoc.NewTable(Fontsmall, g_count, 6, 1);
                                    table2 = myprovdoc.NewTable(Fontsmall, g_count, 6, 1);
                                    table2.VisibleHeaders = false;
                                    table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                    table2.Columns[0].SetWidth(100);
                                    table2.Columns[1].SetWidth(100);
                                    table2.Columns[2].SetWidth(100);
                                    table2.Columns[3].SetWidth(100);
                                    table2.Columns[4].SetWidth(100);
                                    table2.Columns[5].SetWidth(100);
                                    table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                    table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 0).SetContent("Marks");
                                    table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 1).SetContent("Letter Grade");
                                    table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 2).SetContent("Grade Point");
                                    table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 3).SetContent("Marks");
                                    table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 4).SetContent("Letter Grade");
                                    table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(0, 5).SetContent("Grade Point");
                                    int growcount = 0;
                                    int gcolcount = 0;
                                    for (int i = 0; i < g_count; i++)
                                    {
                                        if (growcount < g_countvalue + 2)
                                        {
                                            growcount++;
                                            string scode = (prov1.Rows[i]["Marks"].ToString()) + "-" + (prov1.Rows[i]["Trange"].ToString());
                                            table2.Cell(growcount, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(growcount, 0).SetContent(scode);
                                            string sname = prov1.Rows[i]["Letter Grade"].ToString();
                                            table2.Cell(growcount, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(growcount, 1).SetContent(sname);
                                            string sname1 = prov1.Rows[i]["Grade Points"].ToString();
                                            table2.Cell(growcount, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(growcount, 2).SetContent(sname1);
                                        }
                                        else
                                        {
                                            gcolcount++;
                                            string markobtained = prov1.Rows[i]["Marks"].ToString() + "-" + prov1.Rows[i]["Trange"].ToString();
                                            table2.Cell(gcolcount, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(gcolcount, 3).SetContent(markobtained);
                                            string result = prov1.Rows[i]["Letter Grade"].ToString();
                                            table2.Cell(gcolcount, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(gcolcount, 4).SetContent(result);
                                            string result1 = prov1.Rows[i]["Grade Points"].ToString();
                                            table2.Cell(gcolcount, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                            table2.Cell(gcolcount, 5).SetContent(result1);
                                        }
                                    }
                                    table2.Cell(g_count - 4, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 4, 3).SetContent("Absent");
                                    table2.Cell(g_count - 4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 4, 4).SetContent("Ab");
                                    table2.Cell(g_count - 4, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 4, 5).SetContent("0");
                                    table2.Cell(g_count - 3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 3, 3).SetContent("Inadequate Attendance");
                                    table2.Cell(g_count - 3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 3, 4).SetContent("I");
                                    table2.Cell(g_count - 3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 3, 5).SetContent("0");
                                    table2.Cell(g_count - 2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 2, 3).SetContent("Withdrawal");
                                    table2.Cell(g_count - 2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 2, 4).SetContent("W");
                                    table2.Cell(g_count - 2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 2, 5).SetContent("0");
                                    foreach (PdfCell pr in table2.CellRange(g_count - 1, 0, g_count - 1, 0).Cells)
                                    {
                                        pr.ColSpan = 6;
                                    }
                                    table2.Cell(g_count - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table2.Cell(g_count - 1, 0).SetContent(" U denotes Reappearance is required for the examination in the course");

                                    Gios.Pdf.PdfTablePage myprov_pdfpage2 = table2.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 550, 550, 550));
                                    myprov_pdfpage.Add(myprov_pdfpage2);
                                }
                                Gios.Pdf.PdfTablePage myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 225, 550, 550));
                                myprov_pdfpage.Add(myprov_pdfpage1);
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Format1.pdf";
                                    myprov_pdfpage.SaveToDocument();
                                    myprovdoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                    }
                }
            }
            else if (prov_sno <= 30)
            {
                int prov_subno = 0;
                int prov_pagecount = prov_sno / 2;
                int prov_repage = prov_sno % 2;
                int prov_nopages = prov_pagecount;
                if (prov_repage > 0)
                {
                    prov_nopages++;
                }
                if (prov_nopages > 0)
                {
                    for (int row = 0; row < prov_nopages; row++)
                    {
                        prov_subno++;
                        int y = 40;
                        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 20, 20, 370);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 500, 20, 370);
                        }
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 110, y, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_collnamenew1);
                        PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(myprovdoc, 80, y + 15, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_address1);
                        PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 80, y + 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Department of " + ddlBranch.SelectedItem.Text + "");
                        PdfTextArea ptss1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 80, y + 45, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "UNIVERSITY RESULTS INTIMATION TO PARENTS");
                        PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 160, y + 60, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Date:" + (System.DateTime.Now.ToString("dd/m/yyyy")));
                        PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 20, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Parent,");
                        PdfTextArea ptss4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 35, y + 100, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "The following are the grades obtained by your Son / Daughter in the Anna University");
                        PdfTextArea ptss5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 20, y + 120, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Examinations held during " + ddlMonth.SelectedItem.Text + " / " + ddlYear.SelectedItem.Text + "");
                        PdfTextArea ptss6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 140, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name Of the Student:" + gstudentname);
                        PdfTextArea ptss7 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Register Number:" + gregisternumber);
                        PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester:" + ddlYear.SelectedItem.Text + " / " + ddlSemYr.SelectedItem.Text + "");
                        Gios.Pdf.PdfPage mypage = myprovdoc.NewPage();
                        PdfTextArea ptss9 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 25, y + 720, 120, 30), System.Drawing.ContentAlignment.MiddleRight, "Class Advisor");
                        PdfTextArea ptss10 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 400, y + 720, 50, 30), System.Drawing.ContentAlignment.MiddleRight, "HOD");
                        myprov_pdfpage.Add(ptc);
                        myprov_pdfpage.Add(pts);
                        myprov_pdfpage.Add(ptss);
                        myprov_pdfpage.Add(ptss1);
                        myprov_pdfpage.Add(ptss2);
                        myprov_pdfpage.Add(ptss3);
                        myprov_pdfpage.Add(ptss4);
                        myprov_pdfpage.Add(ptss5);
                        myprov_pdfpage.Add(ptss6);
                        myprov_pdfpage.Add(ptss7);
                        myprov_pdfpage.Add(ptss8);
                        mypage.Add(ptss9);
                        mypage.Add(ptss10);
                        prov_cnt = prov_subno * prov_sno;
                        int nst = prov_subno * 9;
                        Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
                        table1.VisibleHeaders = false;
                        int val = 0;
                        if (prov_cnt <= 30)
                        {
                            if (prov_subno == 1)
                            {
                                table1 = myprovdoc.NewTable(Fontsmall, prov_cnt + 8, 6, 1);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Columns[0].SetWidth(50);
                                table1.Columns[1].SetWidth(100);
                                table1.Columns[2].SetWidth(480);
                                table1.Columns[3].SetWidth(90);
                                table1.Columns[4].SetWidth(100);
                                table1.Columns[5].SetWidth(100);
                                table1.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("S.no");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Course Code");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Course Name");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Credits");
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetContent("Letter Grade");
                                table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 5).SetContent("Subject Grade Points");
                                for (int i = 0; i < prov_cnt; i++)
                                {
                                    val++;
                                    table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 0).SetContent(val);
                                    string course_code = prov.Rows[i]["Course code"].ToString();
                                    table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(val, 1).SetContent(course_code);
                                    string course_name = prov.Rows[i]["Course Name"].ToString();
                                    table1.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(val, 2).SetContent(course_name);
                                    string credits = prov.Rows[i]["Credit"].ToString();
                                    table1.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 3).SetContent(credits);
                                    string credits1 = prov.Rows[i]["Letter Credit"].ToString();
                                    table1.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 4).SetContent(credits1);
                                    string grde = prov.Rows[i]["Subject Grade Points"].ToString();
                                    table1.Cell(val, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(val, 5).SetContent(grde);
                                }
                                int b = 0;
                                for (int a = 0; a < prov_cnt; a++)
                                {
                                    b = b + Convert.ToInt16(prov.Rows[a]["credit"].ToString());
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 1, 0, prov_cnt + 1, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 1, 0).SetContent("Total");
                                table1.Cell(prov_cnt + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table1.Cell(prov_cnt + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(prov_cnt + 1, 3).SetContent(b);
                                string graderesult = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + gregisternumber + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                                int pass = 0;
                                int fail = 0;
                                int arrear_value = 0;
                                int arrear_pass_value = 0;
                                int arrear_fail_value = 0;
                                int arrear_absent_value = 0;
                                int total_no_arrear = 0;
                                string sems = string.Empty;
                                SqlDataAdapter dataresult = new SqlDataAdapter(graderesult, con);
                                DataSet dgs = new DataSet();
                                dataresult.Fill(dgs);
                                for (int i = 0; i < dgs.Tables[0].Rows.Count; i++)
                                {
                                    sems = dgs.Tables[0].Rows[i]["semester"].ToString();
                                    string s = dgs.Tables[0].Rows[i]["result"].ToString();
                                    if (sems == ddlSemYr.SelectedValue)
                                    {
                                        if (s == "Pass")
                                        {
                                            pass++;
                                        }
                                        else
                                        {
                                            fail++;
                                        }
                                    }
                                    else
                                    {
                                        arrear_value++;
                                        if (s == "Pass")
                                        {
                                            arrear_pass_value++;
                                        }
                                        else if (s == "Fail")
                                        {
                                            arrear_fail_value++;
                                        }
                                        else if (s == "AAA")
                                        {
                                            arrear_absent_value++;
                                        }
                                    }
                                }
                                total_no_arrear = arrear_fail_value + arrear_absent_value + fail;
                                if (fail == 0)
                                {
                                    string gpa = commonaccess.Calulat_GPA_Semwise(gregisternumber, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                                    ggpa = gpa;
                                }
                                else
                                {
                                    ggpa = "-";
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 4, 0, prov_cnt + 4, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 4, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 4, 0).SetContent("No Subject Passed:" + pass);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 4, 3, prov_cnt + 4, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 4, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 4, 3).SetContent("No Subject Failed:" + fail);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 2, 0, prov_cnt + 2, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 2, 0).SetContent("GPA :" + ggpa);
                                int rankvalue = 0;
                                rankvalue = Convert.ToInt16(hrank[gregisternumber]);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 2, 3, prov_cnt + 2, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                if (ggpa != "-")
                                {
                                    table1.Cell(prov_cnt + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(prov_cnt + 2, 3).SetContent("Rank Position: " + rankvalue);
                                }
                                else
                                {
                                    table1.Cell(prov_cnt + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table1.Cell(prov_cnt + 2, 3).SetContent("Rank Position: " + "-");
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 3, 0, prov_cnt + 3, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                string sem = ddlSemYr.SelectedValue;
                                if (sem == "4")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("III SEMESTER ");
                                }
                                else if (sem == "3")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("II SEMESTER ");
                                }
                                else if (sem == "2")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("I SEMESTER ");
                                }
                                else if (sem == "1")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("I SEMESTER ");
                                }
                                else if (sem == "5")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("IV SEMESTER ");
                                }
                                else if (sem == "6")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("V SEMESTER ");
                                }
                                else if (sem == "7")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("VI SEMESTER ");
                                }
                                else if (sem == "8")
                                {
                                    table1.Cell(prov_cnt + 3, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table1.Cell(prov_cnt + 3, 0).SetContent("VII SEMESTER ");
                                }
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 5, 0, prov_cnt + 5, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 5, 0).SetContent("No.of Arrear subjects appeared:" + arrear_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 5, 3, prov_cnt + 5, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 5, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 5, 3).SetContent("No.of Arrear Subjects Passed:" + arrear_pass_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 6, 0, prov_cnt + 6, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 6, 0).SetContent("No. of Arrear subjects not appeared:" + arrear_absent_value);
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 6, 3, prov_cnt + 6, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table1.Cell(prov_cnt + 6, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 6, 3).SetContent("No.of Arrear Subjects Failed:  " + arrear_fail_value);
                                totalarear = arearfail + arearabsent + fail;
                                foreach (PdfCell pr in table1.CellRange(prov_cnt + 7, 0, prov_cnt + 7, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                table1.Cell(prov_cnt + 7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table1.Cell(prov_cnt + 7, 0).SetContent("Total No fo Arrear:" + total_no_arrear);
                                int g_count = prov1.Rows.Count;
                                Gios.Pdf.PdfTable table2 = myprovdoc.NewTable(Fontsmall, g_count, 6, 1);
                                table2 = myprovdoc.NewTable(Fontsmall, g_count, 6, 1);
                                table2.VisibleHeaders = false;
                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table2.Columns[0].SetWidth(100);
                                table2.Columns[1].SetWidth(100);
                                table2.Columns[2].SetWidth(100);
                                table2.Columns[3].SetWidth(100);
                                table2.Columns[4].SetWidth(100);
                                table2.Columns[5].SetWidth(100);
                                table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 0).SetContent("Marks");
                                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 1).SetContent("Letter Grade");
                                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 2).SetContent("Grade Point");
                                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 3).SetContent("Marks");
                                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 4).SetContent("Letter Grade");
                                table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 5).SetContent("Grade Point");
                                int g_countvalue = g_count / 2;
                                int growcount = 0;
                                int gcolcount = 0;
                                for (int i = 0; i < g_count; i++)
                                {
                                    if (growcount < g_countvalue + 2)
                                    {
                                        growcount++;
                                        string scode = (prov1.Rows[i]["Marks"].ToString()) + "-" + (prov1.Rows[i]["Trange"].ToString());
                                        table2.Cell(growcount, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 0).SetContent(scode);
                                        string sname = prov1.Rows[i]["Letter Grade"].ToString();
                                        table2.Cell(growcount, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 1).SetContent(sname);
                                        string sname1 = prov1.Rows[i]["Grade Points"].ToString();
                                        table2.Cell(growcount, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 2).SetContent(sname1);
                                    }
                                    else
                                    {
                                        gcolcount++;
                                        string markobtained = prov1.Rows[i]["Marks"].ToString() + "-" + prov1.Rows[i]["Trange"].ToString();
                                        table2.Cell(gcolcount, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 3).SetContent(markobtained);
                                        string result = prov1.Rows[i]["Letter Grade"].ToString();
                                        table2.Cell(gcolcount, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 4).SetContent(result);
                                        string result1 = prov1.Rows[i]["Grade Points"].ToString();
                                        table2.Cell(gcolcount, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 5).SetContent(result1);
                                    }
                                }
                                table2.Cell(g_count - 4, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 3).SetContent("Absent");
                                table2.Cell(g_count - 4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 4).SetContent("Ab");
                                table2.Cell(g_count - 4, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 5).SetContent("0");
                                table2.Cell(g_count - 3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 3).SetContent("Inadequate Attendance");
                                table2.Cell(g_count - 3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 4).SetContent("I");
                                table2.Cell(g_count - 3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 5).SetContent("0");
                                table2.Cell(g_count - 2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 3).SetContent("Withdrawal");
                                table2.Cell(g_count - 2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 4).SetContent("W");
                                table2.Cell(g_count - 2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 5).SetContent("0");
                                foreach (PdfCell pr in table2.CellRange(g_count - 1, 0, g_count - 1, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                table2.Cell(g_count - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 1, 0).SetContent(" U denotes Reappearance is required for the examination in the course");
                                Gios.Pdf.PdfTablePage myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 225, 550, 550));
                                myprov_pdfpage.Add(myprov_pdfpage1);
                                Gios.Pdf.PdfTablePage myprov_pdfpage2 = table2.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 650, 550, 550));
                                mypage.Add(myprov_pdfpage2);
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Format1.pdf";
                                    myprov_pdfpage.SaveToDocument();
                                    mypage.SaveToDocument();
                                    myprovdoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                    }
                }
            }
            else if (prov_sno <= 40)
            {
                int prov_subno = 0;
                int prov_pagecount = prov_sno / 2;
                int prov_repage = prov_sno % 2;
                int prov_nopages = prov_pagecount;
                if (prov_repage > 0)
                {
                    prov_nopages++;
                }
                if (prov_nopages > 0)
                {
                    for (int row = 0; row < prov_nopages; row++)
                    {
                        prov_subno++;
                        int y = 40;
                        Gios.Pdf.PdfPage myprov_pdfpage = myprovdoc.NewPage();
                        Gios.Pdf.PdfPage mypage = myprovdoc.NewPage();
                        Gios.Pdf.PdfPage mypage1 = myprovdoc.NewPage();
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Left_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 20, 20, 370);
                        }
                        if (File.Exists(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg")))//Aruna
                        {
                            PdfImage LogoImage = myprovdoc.NewImage(HttpContext.Current.Server.MapPath("~/college/Right_Logo.jpeg"));
                            myprov_pdfpage.Add(LogoImage, 500, 20, 370);
                        }
                        PdfTextArea ptc = new PdfTextArea(Fontbold, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 110, y, 350, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_collnamenew1);
                        PdfTextArea pts = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                           new PdfArea(myprovdoc, 120, y + 15, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, prov_address1);
                        PdfTextArea ptss = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 120, y + 30, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "Department of " + ddlBranch.SelectedItem.Text + "");
                        PdfTextArea ptss1 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 120, y + 45, 400, 30), System.Drawing.ContentAlignment.MiddleCenter, "UNIVERSITY RESULTS INTIMATION TO PARENTS");
                        PdfTextArea ptss2 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 160, y + 60, 400, 30), System.Drawing.ContentAlignment.MiddleRight, "Date:" + (System.DateTime.Now.ToString("dd/m/yyyy")));
                        PdfTextArea ptss3 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                         new PdfArea(myprovdoc, 20, y + 80, 400, 30), System.Drawing.ContentAlignment.MiddleLeft, "Dear Parent,");
                        PdfTextArea ptss4 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 35, y + 100, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "The following are the grades obtained by your Son / Daughter in the Anna University");
                        PdfTextArea ptss5 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 20, y + 120, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Examinations held during " + ddlMonth.SelectedItem.Text + " / " + ddlYear.SelectedItem.Text + "");
                        PdfTextArea ptss6 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 140, 600, 30), System.Drawing.ContentAlignment.MiddleLeft, "Name Of the Student:" + gstudentname);
                        PdfTextArea ptss7 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 100, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleLeft, "Register Number:" + gregisternumber);
                        if (ddlSemYr.SelectedValue == "1" || ddlSemYr.SelectedValue == "2")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: I / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "3" || ddlSemYr.SelectedValue == "4")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: II / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "5" || ddlSemYr.SelectedValue == "6")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: III / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        else if (ddlSemYr.SelectedValue == "7" || ddlSemYr.SelectedValue == "8")
                        {
                            PdfTextArea ptss8 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                            new PdfArea(myprovdoc, 250, y + 160, 200, 30), System.Drawing.ContentAlignment.MiddleRight, "Year/Semester: IV / " + ddlSemYr.SelectedItem.Text + " ");
                            myprov_pdfpage.Add(ptss8);
                        }
                        PdfTextArea ptss9 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 25, y + 720, 120, 30), System.Drawing.ContentAlignment.MiddleRight, "Class Advisor");
                        PdfTextArea ptss10 = new PdfTextArea(Fontbold1, System.Drawing.Color.Black,
                                                        new PdfArea(myprovdoc, 400, y + 720, 50, 30), System.Drawing.ContentAlignment.MiddleRight, "HOD");
                        myprov_pdfpage.Add(ptc);
                        myprov_pdfpage.Add(pts);
                        myprov_pdfpage.Add(ptss);
                        myprov_pdfpage.Add(ptss1);
                        myprov_pdfpage.Add(ptss2);
                        myprov_pdfpage.Add(ptss3);
                        myprov_pdfpage.Add(ptss4);
                        myprov_pdfpage.Add(ptss5);
                        myprov_pdfpage.Add(ptss6);
                        myprov_pdfpage.Add(ptss7);
                        mypage1.Add(ptss9);
                        mypage1.Add(ptss10);
                        prov_cnt = prov_subno * prov_sno;
                        int tablecount = prov_cnt / 2;
                        Gios.Pdf.PdfTable table1 = myprovdoc.NewTable(Fontsmall, prov_cnt - tablecount, 6, 1);
                        Gios.Pdf.PdfTable table3 = myprovdoc.NewTable(Fontsmall, tablecount + 8, 6, 1);
                        table1.VisibleHeaders = false;
                        int val = 0;
                        int increment = 0;
                        if (prov_cnt <= 40)
                        {
                            if (prov_subno == 1)
                            {
                                table1 = myprovdoc.NewTable(Fontsmall, prov_cnt - tablecount, 6, 1);
                                table1.VisibleHeaders = false;
                                table1.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table1.Columns[0].SetWidth(50);
                                table1.Columns[1].SetWidth(100);
                                table1.Columns[2].SetWidth(480);
                                table1.Columns[3].SetWidth(90);
                                table1.Columns[4].SetWidth(100);
                                table1.Columns[5].SetWidth(100);
                                table1.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table1.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 0).SetContent("S.no");
                                table1.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 1).SetContent("Course Code");
                                table1.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 2).SetContent("Course Name");
                                table1.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 3).SetContent("Credits");
                                table1.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 4).SetContent("Letter Grade");
                                table1.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table1.Cell(0, 5).SetContent("Subject Grade Points");
                                table3 = myprovdoc.NewTable(Fontsmall, tablecount + 8, 6, 1);
                                table3.VisibleHeaders = false;
                                table3.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table3.Columns[0].SetWidth(50);
                                table3.Columns[1].SetWidth(100);
                                table3.Columns[2].SetWidth(480);
                                table3.Columns[3].SetWidth(90);
                                table3.Columns[4].SetWidth(100);
                                table3.Columns[5].SetWidth(100);
                                table3.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table3.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 0).SetContent("S.no");
                                table3.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 1).SetContent("Course Code");
                                table3.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 2).SetContent("Course Name");
                                table3.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 3).SetContent("Credits");
                                table3.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 4).SetContent("Letter Grade");
                                table3.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(0, 5).SetContent("Subject Grade Points");
                                for (int i = 0; i < prov_cnt; i++)
                                {
                                    val++;
                                    if (val < prov_cnt - tablecount)
                                    {
                                        table1.Cell(val, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(val, 0).SetContent(val);
                                        string course_code = prov.Rows[i]["Course code"].ToString();
                                        table1.Cell(val, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(val, 1).SetContent(course_code);
                                        string course_name = prov.Rows[i]["Course Name"].ToString();
                                        table1.Cell(val, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table1.Cell(val, 2).SetContent(course_name);
                                        string credits = prov.Rows[i]["Credit"].ToString();
                                        table1.Cell(val, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(val, 3).SetContent(credits);
                                        string credits1 = prov.Rows[i]["Letter Credit"].ToString();
                                        table1.Cell(val, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(val, 4).SetContent(credits1);
                                        string grde = prov.Rows[i]["Subject Grade Points"].ToString();
                                        table1.Cell(val, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table1.Cell(val, 5).SetContent(grde);
                                    }
                                    else
                                    {
                                        increment++;
                                        table3.Cell(increment, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(increment, 0).SetContent(val);
                                        string course_code = prov.Rows[i]["Course code"].ToString();
                                        table3.Cell(increment, 1).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table3.Cell(increment, 1).SetContent(course_code);
                                        string course_name = prov.Rows[i]["Course Name"].ToString();
                                        table3.Cell(increment, 2).SetContentAlignment(ContentAlignment.MiddleLeft);
                                        table3.Cell(increment, 2).SetContent(course_name);
                                        string credits = prov.Rows[i]["Credit"].ToString();
                                        table3.Cell(increment, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(increment, 3).SetContent(credits);
                                        string credits1 = prov.Rows[i]["Letter Credit"].ToString();
                                        table3.Cell(increment, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(increment, 4).SetContent(credits1);
                                        double gradevalues = Convert.ToDouble(prov.Rows[i]["Subject Grade Points"].ToString()) / 10;
                                        table3.Cell(increment, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table3.Cell(increment, 5).SetContent(gradevalues);
                                    }
                                }
                                int b = 0;
                                for (int a = 0; a < tablecount; a++)
                                {
                                    b = b + Convert.ToInt16(prov.Rows[a]["credit"].ToString());
                                }
                                foreach (PdfCell pr in table3.CellRange(tablecount + 1, 0, tablecount + 1, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 1, 0).SetContentAlignment(ContentAlignment.MiddleRight);
                                table3.Cell(tablecount + 1, 0).SetContent("Total");
                                table3.Cell(tablecount + 1, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table3.Cell(tablecount + 1, 3).SetContent(b);
                                string graderesult = "Select subject_name,subject_code,subject.subject_no,result,total,grade,cp,mark_entry.subject_no,subject.min_ext_marks,subject.min_int_marks,isnull(Mark_Entry.internal_mark,0) as internal_mark ,isnull(Mark_Entry.external_mark,0) as external_mark,semester from Mark_Entry,Subject,sub_sem,syllabus_master where syllabus_master.syll_code=subject.syll_code and Mark_Entry.Subject_No = Subject.Subject_No and subject.subtype_no= sub_sem.subtype_no and  Exam_Code = " + Session["e_code"] + " and roll_no='" + gregisternumber + "'  order by semester desc,subject_type desc,subject.subject_no asc";
                                int pass = 0;
                                int fail = 0;
                                int arrear_value = 0;
                                int arrear_pass_value = 0;
                                int arrear_fail_value = 0;
                                int arrear_absent_value = 0;
                                int total_no_arrear = 0;
                                string semster = string.Empty;
                                SqlDataAdapter dataresult = new SqlDataAdapter(graderesult, con);
                                DataSet dgs = new DataSet();
                                dataresult.Fill(dgs);
                                for (int i = 0; i < dgs.Tables[0].Rows.Count; i++)
                                {
                                    semster = dgs.Tables[0].Rows[i]["semester"].ToString();
                                    string s = dgs.Tables[0].Rows[i]["result"].ToString();
                                    if (semster == ddlSemYr.SelectedValue)
                                    {
                                        if (s == "Pass")
                                        {
                                            pass++;
                                        }
                                        else
                                        {
                                            fail++;
                                        }
                                    }
                                    else
                                    {
                                        arrear_value++;
                                        if (s == "Pass")
                                        {
                                            arrear_pass_value++;
                                        }
                                        else if (s == "Fail")
                                        {
                                            arrear_fail_value++;
                                        }
                                        else if (s == "AAA")
                                        {
                                            arrear_absent_value++;
                                        }
                                    }
                                }
                                total_no_arrear = arrear_fail_value + arrear_absent_value + fail;
                                if (fail == 0)
                                {
                                    string gpa = commonaccess.Calulat_GPA_Semwise(gregisternumber, degree_code, batch_year, exam_month, exam_year, Session["collegecode"].ToString());
                                    ggpa = gpa;
                                }
                                else
                                {
                                    ggpa = "-";
                                }
                                foreach (PdfCell pr in table3.CellRange(tablecount + 2, 0, tablecount + 2, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 2, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 2, 0).SetContent("GPA :" + ggpa);
                                int rankvalue = 0;
                                foreach (PdfCell pr in table3.CellRange(tablecount + 2, 3, tablecount + 2, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                if (ggpa != "-")
                                {
                                    rankvalue = Convert.ToInt16(hrank[gregisternumber]);
                                    table3.Cell(tablecount + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table3.Cell(tablecount + 2, 3).SetContent("Rank Position: " + rankvalue);
                                }
                                else
                                {
                                    table3.Cell(tablecount + 2, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                    table3.Cell(tablecount + 2, 3).SetContent("Rank Position: " + "-");
                                }
                                foreach (PdfCell pr in table3.CellRange(tablecount + 3, 0, tablecount + 3, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 3, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 3, 0).SetContent("No subject Pass: " + pass);
                                foreach (PdfCell pr in table3.CellRange(tablecount + 3, 3, tablecount + 3, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 3, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 3, 3).SetContent("No Subjbect Fail: " + fail);
                                foreach (PdfCell pr in table3.CellRange(tablecount + 4, 0, tablecount + 4, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                string sem = ddlSemYr.SelectedValue;
                                if (sem == "4")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("III SEMESTER ");
                                }
                                else if (sem == "3")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("II SEMESTER ");
                                }
                                else if (sem == "2")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("I SEMESTER ");
                                }
                                else if (sem == "1")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("I SEMESTER ");
                                }
                                else if (sem == "5")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("IV SEMESTER ");
                                }
                                else if (sem == "6")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("V SEMESTER ");
                                }
                                else if (sem == "7")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("VI SEMESTER ");
                                }
                                else if (sem == "8")
                                {
                                    table3.Cell(tablecount + 4, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                    table3.Cell(tablecount + 4, 0).SetContent("VII SEMESTER ");
                                }
                                foreach (PdfCell pr in table3.CellRange(tablecount + 5, 0, tablecount + 5, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 5, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 5, 0).SetContent("No.of Arrear subjects appeared:" + arrear_value);
                                foreach (PdfCell pr in table3.CellRange(tablecount + 5, 3, tablecount + 5, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 5, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 5, 3).SetContent("No.of Arrear Subjects Passed:" + arrear_pass_value);
                                foreach (PdfCell pr in table3.CellRange(tablecount + 6, 0, tablecount + 6, 0).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 6, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 6, 0).SetContent("No. of Arrear subjects not appeared:" + arrear_absent_value);
                                foreach (PdfCell pr in table3.CellRange(tablecount + 6, 3, tablecount + 6, 3).Cells)
                                {
                                    pr.ColSpan = 3;
                                }
                                table3.Cell(tablecount + 6, 3).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 6, 3).SetContent("No.of Arrear Subjects Failed:" + arrear_fail_value);
                                totalarear = arearfail + arearabsent + fail;
                                foreach (PdfCell pr in table3.CellRange(tablecount + 7, 0, tablecount + 7, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                table3.Cell(tablecount + 7, 0).SetContentAlignment(ContentAlignment.MiddleLeft);
                                table3.Cell(tablecount + 7, 0).SetContent("Total Number of Arrear" + total_no_arrear);
                                int g_count = prov1.Rows.Count;
                                Gios.Pdf.PdfTable table2 = myprovdoc.NewTable(Fontsmall, g_count + 1, 6, 1);
                                table2 = myprovdoc.NewTable(Fontsmall, g_count + 1, 6, 1);
                                table2.VisibleHeaders = false;
                                table2.SetBorders(Color.Black, 1, BorderType.CompleteGrid);
                                table2.Columns[0].SetWidth(100);
                                table2.Columns[1].SetWidth(100);
                                table2.Columns[2].SetWidth(100);
                                table2.Columns[3].SetWidth(100);
                                table2.Columns[4].SetWidth(100);
                                table2.Columns[5].SetWidth(100);
                                table2.CellRange(0, 0, 0, 5).SetFont(Fontsmall);
                                table2.Cell(0, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 0).SetContent("Marks");
                                table2.Cell(0, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 1).SetContent("Letter Grade");
                                table2.Cell(0, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 2).SetContent("Grade Point");
                                table2.Cell(0, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 3).SetContent("Marks");
                                table2.Cell(0, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 4).SetContent("Letter Grade");
                                table2.Cell(0, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(0, 5).SetContent("Grade Point");
                                int g_countvalue = g_count / 2;
                                int growcount = 0;
                                int gcolcount = 0;
                                for (int i = 0; i < g_count; i++)
                                {
                                    if (growcount < g_countvalue + 2)
                                    {
                                        growcount++;
                                        string scode = (prov1.Rows[i]["Marks"].ToString()) + "-" + (prov1.Rows[i]["Trange"].ToString());
                                        table2.Cell(growcount, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 0).SetContent(scode);
                                        string sname = prov1.Rows[i]["Letter Grade"].ToString();
                                        table2.Cell(growcount, 1).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 1).SetContent(sname);
                                        string sname1 = prov1.Rows[i]["Grade Points"].ToString();
                                        table2.Cell(growcount, 2).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(growcount, 2).SetContent(sname1);
                                    }
                                    else
                                    {
                                        gcolcount++;
                                        string markobtained = prov1.Rows[i]["Marks"].ToString() + "-" + prov1.Rows[i]["Trange"].ToString();
                                        table2.Cell(gcolcount, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 3).SetContent(markobtained);
                                        string result = prov1.Rows[i]["Letter Grade"].ToString();
                                        table2.Cell(gcolcount, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 4).SetContent(result);
                                        string result1 = prov1.Rows[i]["Grade Points"].ToString();
                                        table2.Cell(gcolcount, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                        table2.Cell(gcolcount, 5).SetContent(result1);
                                    }
                                }
                                table2.Cell(g_count - 4, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 3).SetContent("Absent");
                                table2.Cell(g_count - 4, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 4).SetContent("Ab");
                                table2.Cell(g_count - 4, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 4, 5).SetContent("0");
                                table2.Cell(g_count - 3, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 3).SetContent("Inadequate Attendance");
                                table2.Cell(g_count - 3, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 4).SetContent("I");
                                table2.Cell(g_count - 3, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 3, 5).SetContent("0");
                                table2.Cell(g_count - 2, 3).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 3).SetContent("Withdrawal");
                                table2.Cell(g_count - 2, 4).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 4).SetContent("W");
                                table2.Cell(g_count - 2, 5).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 2, 5).SetContent("0");
                                foreach (PdfCell pr in table2.CellRange(g_count - 1, 0, g_count - 1, 0).Cells)
                                {
                                    pr.ColSpan = 6;
                                }
                                table2.Cell(g_count - 1, 0).SetContentAlignment(ContentAlignment.MiddleCenter);
                                table2.Cell(g_count - 1, 0).SetContent("U denotes Reappearance is required for the examination in the course");
                                Gios.Pdf.PdfTablePage myprov_pdfpage1 = table1.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 225, 550, 550));
                                myprov_pdfpage.Add(myprov_pdfpage1);
                                Gios.Pdf.PdfTablePage myprov_pdfpage3 = table2.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 650, 550, 550));
                                mypage1.Add(myprov_pdfpage3);
                                Gios.Pdf.PdfTablePage myprov_pdfpage2 = table3.CreateTablePage(new Gios.Pdf.PdfArea(myprovdoc, 20, 100, 550, 550));
                                mypage.Add(myprov_pdfpage2);
                                string appPath = HttpContext.Current.Server.MapPath("~");
                                if (appPath != "")
                                {
                                    string szPath = appPath + "/Report/";
                                    string szFile = "Format1.pdf";
                                    myprov_pdfpage.SaveToDocument();
                                    mypage.SaveToDocument();
                                    mypage1.SaveToDocument();
                                    myprovdoc.SaveToFile(szPath + szFile);
                                    Response.ClearHeaders();
                                    Response.AddHeader("Content-Disposition", "attachment; filename=" + szFile);
                                    Response.ContentType = "application/pdf";
                                    Response.WriteFile(szPath + szFile);
                                }
                            }
                        }
                    }
                }
            }
            string sFilePath = Server.MapPath("~/college/" + hodcode + ".jpg");
            FileInfo fi = new FileInfo(sFilePath);
            fi.Delete();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    protected void btnLetterformat1_Click(object sender, EventArgs e)
    {
        try
        {
            panelchech.Visible = false;
            FpExternal.SaveChanges();
            Session["Branch"] = ddlBranch.SelectedItem.Text;
            Session["Degree"] = ddlDegree.SelectedItem.Text;
            Session["Branchcode"] = ddlBranch.SelectedValue;
            Session["Batch"] = ddlBatch.SelectedValue;
            Session["ExamMonth"] = ddlMonth.SelectedValue.ToString();
            Session["ExmMnth"] = ddlMonth.SelectedItem.ToString();
            Session["ExamYear"] = ddlYear.SelectedItem.Text.ToString();
            Session["Semester"] = ddlSemYr.SelectedValue;
            Session["BranchCode"] = ddlBranch.SelectedValue.ToString();
            degree_code = ddlBranch.SelectedValue.ToString();
            current_sem = ddlSemYr.SelectedValue.ToString();
            batch_year = ddlBatch.SelectedValue;
            exam_year = ddlYear.SelectedItem.Text.ToString();
            exam_month = ddlMonth.SelectedValue.ToString();
            IntExamCode = Get_UnivExamCode(Convert.ToInt32(degree_code), GetSemester_AsNumber(Convert.ToInt32(current_sem)), Convert.ToInt32(batch_year), Convert.ToInt32(exam_month), Convert.ToInt32(exam_year));
            Session["ExamCode"] = IntExamCode;
            string str_gradeflage = daccess.GetFunction("select grade_flag from grademaster where degree_code=" + ddlBranch.SelectedValue.ToString() + " and batch_year=" + ddlBatch.SelectedValue + " and exam_month=" + ddlMonth.SelectedValue.ToString() + " and exam_year=" + ddlYear.SelectedItem.Text.ToString() + "");
            Session["grade_flag"] = str_gradeflage;
            loaprovdpdf();
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
    }

    public string Calulat_GPA_cgpaformate1(string RollNo, string degree_code, string batch_year, string exam_month, string exam_year, string collegecode)
    {
        double creditandtotal = 0;
        try
        {
            string ccva = string.Empty;
            string sql = string.Empty;
            double credittotal = 0;
            string syll_code = string.Empty;
            string strcredits = string.Empty;
            DataSet dggradetot = new DataSet();
            syll_code = GetFunction("select distinct syll_code from exam_details e,syllabus_master s where e.degree_code=s.degree_code and e.batch_year=s.batch_year and e.current_semester=s.semester and e.degree_code='" + degree_code + "' and e.batch_year=" + batch_year + " and exam_month=" + exam_month + " and exam_year=" + exam_year + "");
            ccva = GetFunction("select cc from registration where roll_no='" + RollNo + "'");
            if (ccva == "False")
            {
                sql = " Select distinct Subject.subject_code,Subject.credit_points,isnull (SubWiseGrdeMaster.credit_points,'0') as gradepoints,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No  and SubWiseGrdeMaster.Grade=Mark_Entry.grade and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and isnull (SubWiseGrdeMaster.credit_points,'0')>0";
            }
            else if (ccva == "True")
            {
                sql = " Select distinct Subject.subject_code,Subject.credit_points,isnull (SubWiseGrdeMaster.credit_points,'0') as gradepoints,Mark_Entry.total,Mark_Entry.grade from Mark_Entry,Subject,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No  and SubWiseGrdeMaster.Grade=Mark_Entry.grade and subject.syll_code=" + syll_code.ToString() + "  and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and isnull (SubWiseGrdeMaster.credit_points,'0')>0";
            }
            DataSet marksdata = new DataSet();
            marksdata.Clear();
            if (sql != "" && sql != null)
            {
                marksdata = daccess.select_method_wo_parameter(sql, "Text");
                if (marksdata.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
                    {
                        double total = Convert.ToDouble(marksdata.Tables[0].Rows[i]["total"].ToString());
                        double gp = Convert.ToDouble(marksdata.Tables[0].Rows[i]["gradepoints"].ToString());
                        // string data = "select Credit_Points  from SubWiseGrdeMaster where subjectcode='" + marksdata.Tables[0].Rows[i]["subject_code"].ToString() + "'  and  '" + total + "'<= frange and '" + total + "'>= trange and college_code='" + collegecode + "'  and exam_month=" + exam_month + " and exam_year=" + exam_year + "";
                        string data = Convert.ToString(marksdata.Tables[0].Rows[i]["credit_points"].ToString());
                        strcredits = data;
                        creditandtotal = creditandtotal + (gp * Convert.ToDouble(strcredits));
                        credittotal = credittotal + Convert.ToDouble(strcredits);
                    }
                }
                else
                {
                    return "-";
                }
            }
            creditandtotal = Math.Round((creditandtotal / credittotal), 2, MidpointRounding.AwayFromZero);
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        return creditandtotal.ToString();
    }

    public string Calulat_CGPA_cgpaformate1(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        string calculate = string.Empty;
        try
        {
            string strsubcrd = string.Empty;
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            string strcredits = string.Empty;
            double creditandtotal = 0;
            double credittotal = 0;
            strsubcrd = " Select distinct Syllabus_Master.Semester,Subject.subject_code,Mark_Entry.exam_code, Subject.credit_points,isnull(SubWiseGrdeMaster.credit_points,'0') as gradepoint,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS'  and SubWiseGrdeMaster.Grade=Mark_Entry.grade and isnull (SubWiseGrdeMaster.credit_points,'0')>0 order by Syllabus_Master.Semester";
            DataSet marksdata = new DataSet();
            marksdata.Clear();
            ArrayList seminfo = new ArrayList();
            seminfo.Clear();
            DataView dvgr = new DataView();
            bool semnewadd = false;
            double semgpa = 0;
            credittotal = 0;
            semgpa = 0;
            if (strsubcrd != null && strsubcrd != "")
            {
                marksdata = daccess.select_method_wo_parameter(strsubcrd, "Text");
                if (marksdata.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
                    {
                        if (!seminfo.Contains(marksdata.Tables[0].Rows[i]["Semester"].ToString()))
                        {
                            marksdata.Tables[0].DefaultView.RowFilter = "Semester='" + marksdata.Tables[0].Rows[i]["Semester"].ToString() + "'";
                            dvgr = marksdata.Tables[0].DefaultView;
                            if (dvgr.Count > 0)
                            {
                                //credittotal = 0; //Hide by aruna 17apr2017
                                //semgpa = 0;
                                for (int j = 0; j < dvgr.Count; j++)
                                {
                                    double total = Convert.ToDouble(dvgr[j]["total"].ToString());
                                    string exam_months = daccess.GetFunction(" select Exam_Month from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                    string exam_years = daccess.GetFunction(" select Exam_year from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                    double gp = Convert.ToDouble(dvgr[j]["gradepoint"].ToString());
                                    strcredits = Convert.ToString(dvgr[j]["credit_points"].ToString()); //GetFunction(data);
                                    semgpa = semgpa + (gp * Convert.ToDouble(strcredits));
                                    credittotal = credittotal + Convert.ToDouble(strcredits);
                                }
                                semnewadd = true;
                                seminfo.Add(marksdata.Tables[0].Rows[i]["Semester"].ToString());
                            }
                        }

                    }
                }
            }

            creditandtotal = Math.Round((semgpa / credittotal), 2, MidpointRounding.AwayFromZero); // Add by aruna 17apr2017
            calculate = Convert.ToString(creditandtotal);
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        if (calculate == "NaN" || calculate.Trim() == "")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }

    //public string Calulat_CGPA_cgpaformate1(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    //{
    //    string calculate =string.Empty;
    //    try
    //    {
    //        string strsubcrd =string.Empty;
    //        DataSet dggradetot = new DataSet();
    //        DataSet dssem = new DataSet();
    //        string strcredits =string.Empty;
    //        double creditandtotal = 0;
    //        double credittotal = 0;
    //        strsubcrd = " Select distinct Syllabus_Master.Semester,Subject.subject_code,Mark_Entry.exam_code, Subject.credit_points,isnull(SubWiseGrdeMaster.credit_points,'0') as gradepoint,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
    //        strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
    //        //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
    //        strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS'  and SubWiseGrdeMaster.Grade=Mark_Entry.grade and isnull (SubWiseGrdeMaster.credit_points,'0')>0 order by Syllabus_Master.Semester";
    //        DataSet marksdata = new DataSet();
    //        marksdata.Clear();
    //        ArrayList seminfo = new ArrayList();
    //        seminfo.Clear();
    //        DataView dvgr = new DataView();
    //        bool semnewadd = false;
    //        double semgpa = 0;
    //        if (strsubcrd != null && strsubcrd != "")
    //        {
    //            marksdata = daccess.select_method_wo_parameter(strsubcrd, "Text");
    //            if (marksdata.Tables[0].Rows.Count > 0)
    //            {
    //                for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
    //                {
    //                    if (!seminfo.Contains(marksdata.Tables[0].Rows[i]["Semester"].ToString()))
    //                    {
    //                        marksdata.Tables[0].DefaultView.RowFilter = "Semester='" + marksdata.Tables[0].Rows[i]["Semester"].ToString() + "'";
    //                        dvgr = marksdata.Tables[0].DefaultView;
    //                        if (dvgr.Count > 0)
    //                        {
    //                            credittotal = 0;
    //                            semgpa = 0;
    //                            for (int j = 0; j < dvgr.Count; j++)
    //                            {
    //                                double total = Convert.ToDouble(dvgr[j]["total"].ToString());
    //                                string exam_months = daccess.GetFunction(" select Exam_Month from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
    //                                string exam_years = daccess.GetFunction(" select Exam_year from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
    //                                double gp = Convert.ToDouble(dvgr[j]["gradepoint"].ToString());
    //                                // string data = " select  Credit_Points  from SubWiseGrdeMaster where subjectcode='" + dvgr[j]["subject_code"].ToString() + "'  and  '" + dvgr[j]["total"].ToString() + "'<= frange and '" + dvgr[j]["total"].ToString() + "'>= trange and college_code='" + collegecode + "'  and exam_month='" + exam_months + "' and exam_year='" + exam_years + "'";
    //                                strcredits = Convert.ToString(dvgr[j]["credit_points"].ToString()); //GetFunction(data);
    //                                semgpa = semgpa + (gp * Convert.ToDouble(strcredits));
    //                                credittotal = credittotal + Convert.ToDouble(strcredits);
    //                            }
    //                            semnewadd = true;
    //                            seminfo.Add(marksdata.Tables[0].Rows[i]["Semester"].ToString());
    //                        }
    //                    }
    //                    if (semnewadd == true)
    //                    {
    //                        semgpa = Math.Round((semgpa / credittotal), 2, MidpointRounding.AwayFromZero);
    //                        creditandtotal = creditandtotal + semgpa;
    //                        semnewadd = false;
    //                    }
    //                }
    //            }
    //        }
    //        if (seminfo.Count > 0)
    //        {
    //            creditandtotal = Math.Round((creditandtotal / seminfo.Count), 2, MidpointRounding.AwayFromZero);
    //            calculate = creditandtotal.ToString();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        lblError.Visible = true;
    //        lblError.Text = ex.ToString();
    //    }
    //    if (calculate == "NaN" || calculate.Trim() == "")
    //    {
    //        return "-";
    //    }
    //    else
    //    {
    //        return calculate;
    //    }
    //}

    protected void btnsendSmsandEmail_OnClick(object sender, EventArgs e)
    {

        #region My Variables

        SMSSettings smsObject = new SMSSettings();
        bool toFather = cbSendToFather.Checked;
        bool toMother = cbSendToMother.Checked;
        bool toStudent = cbSendToStudent.Checked;

        bool ViaSms = cbViaSms.Checked;
        bool ViaEmail = cbViaEmail.Checked;
        int selected = 0;

        bool nonumflag = false;
        string studentName = string.Empty;
        string RollNo = string.Empty;
        string RegNo = string.Empty;
        string ReultString = string.Empty;
        string StudentNumber = string.Empty;
        string FatherNumber = string.Empty;
        string MotherNumber = string.Empty;

        string studentmailid = string.Empty;
        string fathermailid = string.Empty;
        string mothermailid = string.Empty;
        string mailtobesent = string.Empty;

        string subjectNumber = string.Empty;
        string Gpa = string.Empty;
        string Cgpa = string.Empty;
        string smstext = string.Empty;
        string Result = string.Empty;
        string subjecttext = string.Empty;
        string Grade = string.Empty;
        string subjectName = string.Empty;
        string collegeName = string.Empty;
        string ApplicationNo = string.Empty;
        string AdmissionNo = string.Empty;
        string Degree = string.Empty;
        string Department = string.Empty;
        string userCode = Convert.ToString(Session["usercode"]).Trim();
        string AppNo = string.Empty;
        string mobilenos = string.Empty;
        string smsnotSent = string.Empty;
        string mailnotSent = string.Empty;

        lblError.Visible = false;
        lblError.Text = string.Empty;

        #endregion

        FpExternal.SaveChanges();

        for (int row = 0; row < FpExternal.Sheets[0].RowCount; row++)
        {
            if (FpExternal.Sheets[0].Cells[row, 1].Value.ToString() == "1")
                selected++;
        }
        string Finalmsg = string.Empty;
        if (selected > 0)
        {
            for (int row = 0; row < FpExternal.Sheets[0].RowCount; row++)
            {
                if (FpExternal.Sheets[0].Cells[row, 1].Value.ToString() == "1")
                {
                    RollNo = FpExternal.Sheets[0].Cells[row, 2].Text;
                    RegNo = FpExternal.Sheets[0].Cells[row, 3].Text;
                    studentName = FpExternal.Sheets[0].Cells[row, 4].Text;
                    FatherNumber = FpExternal.Sheets[0].Cells[row, 2].Note;
                    MotherNumber = FpExternal.Sheets[0].Cells[row, 3].Note;
                    StudentNumber = FpExternal.Sheets[0].Cells[row, 4].Note;

                    if (ViaSms)
                    {
                        if (toFather)
                            mobilenos = Convert.ToString(FpExternal.Sheets[0].Cells[row, 2].Note);
                        if (toMother)
                            if (string.IsNullOrEmpty(mobilenos))
                                mobilenos = Convert.ToString(FpExternal.Sheets[0].Cells[row, 3].Note);
                            else
                                mobilenos += "," + Convert.ToString(FpExternal.Sheets[0].Cells[row, 3].Note);
                        if (toStudent)
                            if (string.IsNullOrEmpty(mobilenos))
                                mobilenos = Convert.ToString(FpExternal.Sheets[0].Cells[row, 4].Note);
                            else
                                mobilenos += "," + Convert.ToString(FpExternal.Sheets[0].Cells[row, 4].Note);
                    }

                    Gpa = FpExternal.Sheets[0].Cells[row, FpExternal.Sheets[0].ColumnCount - 3].Text;
                    Cgpa = FpExternal.Sheets[0].Cells[row, FpExternal.Sheets[0].ColumnCount - 2].Text;
                    Result = FpExternal.Sheets[0].Cells[row, FpExternal.Sheets[0].ColumnCount - 1].Text;
                    ReultString = string.Empty;
                    DataTable dtstudinfo = dirAcc.selectDataTable("select col.collname,a.app_formno applicationNo,r.App_No,r.Roll_No,r.Reg_No,r.Roll_Admit,r.Stud_Name,r.Stud_Type,LTRIM(RTRIM(ISNULL(r.Sections,''))) as Sections,c.Course_Name,dt.Dept_Name,dt.dept_acronym,r.college_code,r.Batch_Year,r.current_semester,isnull(a.parentF_Mobile,'0') as FatherMobile,isnull(a.parentM_Mobile,'0')as MotherMobile,isnull(a.Student_Mobile,'0') as StudentMobile,a.sex as Gender,a.emailM,a.StuPer_Id,a.emailp,r.degree_code from Registration r,applyn a,Degree dg,Department dt,Course c,collinfo col where col.college_code=r.college_code and col.college_code=dg.college_code and col.college_code=dt.college_code and col.college_code=c.college_code and c.Course_Id=dg.Course_Id and dg.Dept_Code=dt.Dept_Code and dg.Degree_Code=r.degree_code and r.App_No=a.app_no and CC='0' and r.DelFlag='0' and r.Exam_Flag<>'debar' and r.Roll_No='" + RollNo + "' and r.Reg_No='" + RegNo + "' order by r.Batch_Year,r.degree_code,Sections");

                    if (dtstudinfo.Rows.Count > 0)
                    {
                        for (int col = 0; col < FpExternal.Sheets[0].ColumnCount; col++)
                        {
                            Grade = string.Empty;
                            try
                            {
                                subjecttext = FpExternal.Sheets[0].ColumnHeader.Cells[4, col].Text;
                                subjectNumber = FpExternal.Sheets[0].ColumnHeader.Cells[0, col].Note.ToString();
                                subjectName = dirAcc.selectScalarString("select acronym from Subject where subject_no='" + subjectNumber + "'");
                                Grade = FpExternal.Sheets[0].Cells[row, col].Text;
                                if (!string.IsNullOrEmpty(Grade))
                                {
                                    if (subjecttext.ToUpper() == "GRADE/MARK")
                                    {
                                        if (Grade.Contains('+'))
                                        {
                                            Grade += "plus";
                                            ReultString += subjectName + " - " + Grade + "\n";
                                        }
                                        else
                                        {
                                            ReultString += subjectName + " - " + Grade + "\n";
                                        }
                                    }
                                }

                            }
                            catch
                            {
                            }
                        }
                        Finalmsg += ReultString + "With Result " + Result + "\n CGPA :" + Cgpa + "\n Gpa " + Gpa + " ";
                        string MsgText = string.Empty;

                        DataSet ds1 = dirAcc.selectDataSet("select template from Master_Settings where value='4' and usercode='" + userCode + "'");

                        if (ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                        {
                            string templatevlaue = Convert.ToString(ds1.Tables[0].Rows[0]["Template"]);
                            if (templatevlaue.Trim() != "")
                            {
                                string[] splittemplate = templatevlaue.Split('$');
                                if (splittemplate.Length > 0)
                                {
                                    for (int j = 0; j <= splittemplate.GetUpperBound(0); j++)
                                    {
                                        if (splittemplate[j].ToString() != "")
                                        {
                                            if (splittemplate[j].ToString() == "College Name")
                                                MsgText = MsgText + "  " + Convert.ToString(dtstudinfo.Rows[0]["collname"]) + "";
                                            else if (splittemplate[j].ToString() == "Student Name")
                                                MsgText = MsgText + "  " + studentName + "";
                                            else if (splittemplate[j].ToString() == "Roll No")
                                                MsgText = MsgText + " - " + RollNo + "";
                                            else if (splittemplate[j].ToString() == "Register No")
                                                MsgText = MsgText + " - " + RegNo + "";
                                            else if (splittemplate[j].ToString() == "Degree")
                                                MsgText = MsgText + " - " + Convert.ToString(dtstudinfo.Rows[0]["Course_Name"]) + "";
                                            else if (splittemplate[j].ToString() == "Application No")
                                                MsgText = MsgText + " - " + Convert.ToString(dtstudinfo.Rows[0]["Roll_Admit"]) + "";
                                            else if (splittemplate[j].ToString() == "Admission No")
                                                MsgText = MsgText + " - " + Convert.ToString(dtstudinfo.Rows[0]["collname"]) + "";
                                            else if (splittemplate[j].ToString() == "University Result")
                                                MsgText = MsgText + " \n " + ReultString + "\n";
                                            else if (splittemplate[j].ToString() == "Thank You")
                                                MsgText = MsgText + " Thank You. ";
                                            else
                                                if (MsgText == "")
                                                    MsgText = splittemplate[j].ToString();
                                                else
                                                    MsgText = MsgText + " " + splittemplate[j].ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblError.Visible = true;
                            lblError.Text = "Message Template Not Found";
                            return;
                        }
                        int sentsms = 0;
                        try
                        {
                            if (ViaSms)
                            {

                                AppNo = Convert.ToString(dtstudinfo.Rows[0]["App_No"]);
                                Degree = Convert.ToString(dtstudinfo.Rows[0]["degree_code"]);
                                smsObject.User_degreecode = Convert.ToInt32(Degree);
                                smsObject.User_collegecode = Convert.ToInt32(Convert.ToString(dtstudinfo.Rows[0]["college_code"]));
                                smsObject.User_usercode = userCode;
                                smsObject.Text_message = MsgText;
                                smsObject.IsStaff = 0;

                                if (toStudent)
                                {
                                    mobilenos = Convert.ToBoolean(string.IsNullOrEmpty(StudentNumber)) ? null : StudentNumber;
                                }
                                if (toFather)
                                {
                                    if (!String.IsNullOrEmpty(mobilenos))
                                        mobilenos = Convert.ToBoolean(string.IsNullOrEmpty(FatherNumber)) ? null : FatherNumber;
                                    else
                                        mobilenos += "," + Convert.ToString(Convert.ToBoolean(string.IsNullOrEmpty(FatherNumber)) ? null : FatherNumber);
                                }
                                if (toMother)
                                {
                                    if (!String.IsNullOrEmpty(mobilenos))
                                        mobilenos = Convert.ToBoolean(string.IsNullOrEmpty(MotherNumber)) ? null : MotherNumber;
                                    else
                                        mobilenos += "," + Convert.ToString(Convert.ToBoolean(string.IsNullOrEmpty(MotherNumber)) ? null : MotherNumber);
                                }
                                if (string.IsNullOrEmpty(mobilenos) || mobilenos == "")
                                {
                                    lblError.Visible = true;
                                    lblError.Text = "Mobile Number Not Found";
                                    return;
                                }
                                else if (mobilenos.Length > 0)
                                {
                                    smsObject.MobileNos = mobilenos;
                                    smsObject.AdmissionNos = Convert.ToString(dtstudinfo.Rows[0]["Roll_Admit"]);
                                    sentsms = smsObject.sendTextMessage();
                                }
                                else
                                {
                                    if (sentsms == 0)
                                        smsnotSent += "   " + studentName + "\n";
                                }
                            }
                            if (ViaEmail)
                            {
                                string send_mail = string.Empty;
                                string send_pw = string.Empty;
                                DataTable dtEmailInfo = new DataTable();
                                string app_no = string.Empty;
                                string listAppNo = string.Empty;
                                studentmailid = string.IsNullOrEmpty(Convert.ToString(dtstudinfo.Rows[0]["StuPer_Id"])) ? "" : Convert.ToString(dtstudinfo.Rows[0]["StuPer_Id"]);
                                fathermailid = string.IsNullOrEmpty(Convert.ToString(dtstudinfo.Rows[0]["emailp"])) ? "" : Convert.ToString(dtstudinfo.Rows[0]["emailp"]);
                                mothermailid = string.IsNullOrEmpty(Convert.ToString(dtstudinfo.Rows[0]["emailM"])) ? "" : Convert.ToString(dtstudinfo.Rows[0]["emailM"]);

                                if (!string.IsNullOrEmpty(studentmailid))
                                {
                                    string strquery = "select massemail,masspwd from collinfo where college_code ='" + Convert.ToString(dtstudinfo.Rows[0]["college_code"]) + "' ";
                                    dtEmailInfo.Dispose();
                                    dtEmailInfo.Reset();
                                    dtEmailInfo = dirAcc.selectDataTable(strquery);
                                    {
                                        send_mail = Convert.ToString(dtEmailInfo.Rows[0]["massemail"]);
                                        send_pw = Convert.ToString(dtEmailInfo.Rows[0]["masspwd"]);
                                    }
                                    SmtpClient Mail = new SmtpClient("smtp.gmail.com", 587);
                                    Mail.EnableSsl = true;
                                    MailMessage mailmsg = new MailMessage();
                                    MailAddress mfrom = new MailAddress(send_mail);
                                    mailmsg.From = mfrom;

                                    if (toStudent)
                                    {
                                        if (!string.IsNullOrEmpty(studentmailid))
                                            mailmsg.To.Add(studentmailid);
                                    }
                                    if (toFather)
                                    {
                                        if (!string.IsNullOrEmpty(fathermailid))
                                            mailmsg.To.Add(studentmailid);
                                    }
                                    if (toMother)
                                    {
                                        if (!string.IsNullOrEmpty(mothermailid))
                                            mailmsg.To.Add(studentmailid);
                                    }
                                    if (mailmsg.To.Count > 0)
                                    {
                                        mailmsg.Subject = "University Result ";
                                        mailmsg.IsBodyHtml = true;
                                        mailmsg.Body = MsgText;
                                        Mail.EnableSsl = true;
                                        Mail.UseDefaultCredentials = false;
                                        NetworkCredential credentials = new NetworkCredential(send_mail, send_pw);
                                        Mail.Credentials = credentials;
                                        Mail.Send(mailmsg);
                                    }
                                    else
                                    {
                                        mailnotSent += "  " + studentName + "\n";
                                    }
                                }

                            }
                        }
                        catch
                        {
                        }
                    }

                }
            }
        }
        else
        {
            lblError.Visible = true;
            lblError.Text = "Please Select Atleast one Student and then Proceed";
        }

    }

    protected void chk_IncludePassedOut_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chk_IncludePassedOut.Checked)
        {
            ddlSemYr.Items.Clear();
            string degreecode = ddlBranch.SelectedValue;
            string batch = ddlBatch.SelectedValue;
            string semqry = "select max(current_semester) from  Registration where degree_code='" + degreecode + "' and Batch_Year='" + batch + "'";
            int semmaxcount = 0;
            Int32.TryParse(dirAcc.selectScalarString(semqry), out semmaxcount);
            for (int i = 1; i <= semmaxcount; i++)
            {
                ddlSemYr.Items.Add(i.ToString());
            }
        }
        else
        {
            Get_Semester();
        }
    }

    public string Calulat_CGPA_cgpaformate1PG(string RollNo, string semval, string degree_code, string batch_year, string latmode, string collegecode)
    {
        string calculate = string.Empty;
        try
        {
            string strsubcrd = string.Empty;
            DataSet dggradetot = new DataSet();
            DataSet dssem = new DataSet();
            string strcredits = string.Empty;
            double creditandtotal = 0;
            double credittotal = 0;
            strsubcrd = " Select distinct Syllabus_Master.Semester,Subject.subject_code,Mark_Entry.exam_code, Subject.credit_points,isnull(SubWiseGrdeMaster.credit_points,'0') as gradepoint,Mark_Entry.internal_mark,Mark_Entry.external_mark,Mark_Entry.total,Mark_Entry.grade,Mark_Entry.attempts from Mark_Entry,Subject,Syllabus_Master,SubWiseGrdeMaster where Mark_Entry.Subject_No = Subject.Subject_No and Syllabus_Master.syll_Code = Subject.syll_Code and roll_no='" + RollNo + "' and (result='Pass' or result='pass') and attempts>=1";
            strsubcrd = strsubcrd + " AND Exam_Code IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' AND Batch_Year =" + batch_year + " AND current_semester<=" + semval + " ) ";
            //strsubcrd = strsubcrd + " AND Exam_Code NOT IN (SELECT Exam_Code FROM Exam_Details WHERE Degree_Code ='" + degree_code + "' and batch_year=" + batch_year + " AND Exam_Year =2013 AND Exam_Month >4)";
            strsubcrd = strsubcrd + " AND Roll_No='" + RollNo + "' AND Syllabus_Master.Semester <=" + semval + " AND UPPER(Result) ='PASS'  and SubWiseGrdeMaster.Grade=Mark_Entry.grade and isnull (SubWiseGrdeMaster.credit_points,'0')>0 order by Syllabus_Master.Semester";
            DataSet marksdata = new DataSet();
            marksdata.Clear();
            ArrayList seminfo = new ArrayList();
            seminfo.Clear();
            DataView dvgr = new DataView();
            bool semnewadd = false;
            double semgpa = 0;
            credittotal = 0;
            semgpa = 0;
            if (strsubcrd != null && strsubcrd != "")
            {
                marksdata = daccess.select_method_wo_parameter(strsubcrd, "Text");
                if (marksdata.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < marksdata.Tables[0].Rows.Count; i++)
                    {
                        if (!seminfo.Contains(marksdata.Tables[0].Rows[i]["Semester"].ToString()))
                        {
                            marksdata.Tables[0].DefaultView.RowFilter = "Semester='" + marksdata.Tables[0].Rows[i]["Semester"].ToString() + "'";
                            dvgr = marksdata.Tables[0].DefaultView;
                            if (dvgr.Count > 0)
                            {
                                //credittotal = 0; //Hide by aruna 17apr2017
                                //semgpa = 0;
                                for (int j = 0; j < dvgr.Count; j++)
                                {
                                    double total = Convert.ToDouble(dvgr[j]["total"].ToString());
                                    string exam_months = daccess.GetFunction(" select Exam_Month from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                    string exam_years = daccess.GetFunction(" select Exam_year from Exam_Details where exam_code='" + dvgr[j]["exam_code"].ToString() + "'");
                                    double gp = Convert.ToDouble(dvgr[j]["gradepoint"].ToString());
                                    strcredits = Convert.ToString(dvgr[j]["credit_points"].ToString()); //GetFunction(data);
                                    semgpa = semgpa + (gp * Convert.ToDouble(strcredits));
                                    credittotal = credittotal + Convert.ToDouble(strcredits);
                                }
                                semnewadd = true;
                                seminfo.Add(marksdata.Tables[0].Rows[i]["Semester"].ToString());
                            }
                        }

                    }
                }
            }

            creditandtotal = Math.Round((semgpa / credittotal), 2, MidpointRounding.AwayFromZero); // Add by aruna 17apr2017
            calculate = Convert.ToString(creditandtotal);
        }
        catch (Exception ex)
        {
            lblError.Visible = true;
            lblError.Text = ex.ToString();
        }
        if (calculate == "NaN" || calculate.Trim() == "")
        {
            return "-";
        }
        else
        {
            return calculate;
        }
    }

}
